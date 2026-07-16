# CLAUDE.md

Este arquivo fornece orientações ao Claude Code (claude.ai/code) ao trabalhar com o código deste repositório.

## O que é

Uma interface desktop em Windows Forms (.NET 8) para rodar cobertura de testes .NET sem linha de comando. A aplicação chama o **OpenCover** (que executa o `vstest.console.exe` sob o profiler de cobertura) e o **ReportGenerator** (que transforma o XML bruto em HTML), e depois exibe o HTML resultante em um controle **WebView2** embutido.

O projeto não é uma biblioteca — ele orquestra executáveis externos via `Process`. A maior parte dos bugs está na montagem das strings de argumento e na resolução de caminhos, não no C#.

## Comandos

```bash
dotnet restore
dotnet build TestCoverageUI.sln                       # também publica o Updater (ver abaixo)
dotnet run --project TestCoverageUI.UI                # rodar em desenvolvimento
dotnet publish TestCoverageUI.UI -c Release -r win-x64 --self-contained false -o publish
```

**Não existem projetos de teste nem configuração de linter** nesta solução. O `dotnet test` não vai encontrar nada — não inclua esse passo esperando alguma saída.

## Arquitetura

Quatro projetos, em camadas bem definidas (`UI → Services → Models`; o `Updater` é independente):

- **TestCoverageUI.Models** — `CoverageProfile` (uma configuração nomeada) e `ProfilesConfig` (a lista, junto com a própria lógica de carregar/salvar/semear o JSON). A persistência mora no próprio model; não há repositório nem container de injeção de dependência em lugar nenhum.
- **TestCoverageUI.Services** — `CoverageService` (o pipeline OpenCover/ReportGenerator), `UpdateService` (verificação de atualização) e `DllCleanupService` (apaga em lote as DLLs que casam com prefixo/sufixo na pasta bin).
- **TestCoverageUI.UI** — `MainForm` (seletor de perfil, aba de log, aba de relatório) e `ConfigForm` (editor de perfil). O assembly é renomeado para `TestCoverageUI.exe`.
- **TestCoverageUI.Updater** — console app single-file separado (`Updater.exe`). Precisa ser um processo à parte porque sobrescreve os binários da aplicação principal enquanto ela está fechando.

### Pipeline de cobertura (`CoverageService.GenerateCoverageAsync`)

Busca `{PrefixDll}*{SuffixDll}.dll` no `BinPath`, roda o OpenCover **uma vez por DLL de teste** com `-mergeoutput` acumulando tudo em um único `coverage.xml`, e por fim roda o ReportGenerator uma vez sobre esse XML. As saídas caem em `Environment.CurrentDirectory` (`coverage.xml`, `coverage-report/index.htm`) — ou seja, relativas ao diretório de trabalho, e não ao `BinPath` do perfil. Retorna o caminho do relatório ou `null`; todo caminho de falha registra no log e retorna `null` em vez de lançar exceção.

O log é um callback `Action<string>` injetado no service. O `MainForm` colore as linhas comparando trechos do texto (`"Passed"` → verde, `"Erro"` → vermelho, etc.), então **mudar o texto de uma mensagem no service altera silenciosamente as cores do log na UI**.

### Configuração

`configs.json` fica ao lado do executável (`AppContext.BaseDirectory`), criado com um perfil `Default` hardcoded pelo `ProfilesConfig.EnsureConfigExists()` na inicialização. Repare que o `ProfilesConfig.Load()` chama `File.ReadAllText` sem proteção — ele assume que o `EnsureConfigExists()` rodou antes, no `Program.Main`.

Os caminhos das ferramentas no perfil podem ser relativos; o `CoverageService.ResolveToolPath` os rebaseia a partir do `AppContext.BaseDirectory`. A pasta `Tools/`, que contém o OpenCover e o ReportGenerator, **não está no git e não é copiada por nenhum csproj** — ela existe apenas nas pastas locais de staging `publish/` e `TestCoverageUI/`. Um clone novo compila, mas os caminhos padrão de ferramentas não vão resolver enquanto a `Tools/` não for colocada ao lado do binário.

O `CoverageProfile.UseEmbeddedTools` é persistido e habilita/desabilita os botões Browse no `ConfigForm`, mas o `CoverageService` nunca lê esse campo. O `-filter` do OpenCover é montado em tempo de execução a partir de `PrefixDll`/`SuffixDll` — não existe campo de filtro armazenado, apesar do que diz o `readme.md`.

### Fluxo de atualização automática

`MainForm_Load` → `UpdateService.CheckForUpdateAsync()` busca o `version.json` na **branch `main` via raw.githubusercontent.com** e compara com a versão do assembly em execução. Se for mais nova, baixa o zip da release para a pasta temp, dispara `Updater.exe <zip> <exePath>` e chama `Environment.Exit(0)`. O Updater fica em polling até o processo principal sumir, extrai por cima do diretório de destino, **pula o próprio nome de arquivo** para não se sobrescrever em uso, e reinicia a aplicação.

O `TestCoverageUI.UI.csproj` tem um target `AfterTargets="Build"` que roda `dotnet publish` do Updater para `publish-updater/` e copia o `Updater.exe` para o diretório de saída — ou seja, o Updater é recompilado a cada build da UI, e um csproj quebrado do Updater quebra o build da UI.

### Publicando uma nova versão

O número de versão precisa ser atualizado em **três** lugares, ou o fluxo de atualização se comporta mal:

1. `TestCoverageUI.UI/TestCoverageUI.UI.csproj` (`Version`/`AssemblyVersion`/`FileVersion`)
2. `TestCoverageUI.Services/TestCoverageUI.Services.csproj` — **crítico**: o `VersionHelper.GetCurrentVersion()` usa `Assembly.GetExecutingAssembly()`, que resolve para o assembly de *Services*, não o da UI. Se a versão do Services ficar para trás, o app compara contra o número errado e volta a oferecer uma atualização que já foi instalada.
3. `version.json` na raiz do repositório, mais uma release no GitHub naquela tag contendo o `TestCoverageUI.zip`.

## Convenções

- **Indentação de 2 espaços**; `Nullable` e `ImplicitUsings` habilitados em todos os projetos.
- O código é em **português (pt-BR)**: textos de UI, mensagens de log, mensagens de exceção, comentários e mensagens de commit. Os nomes de métodos misturam português e inglês (`ApagarDllsComFiltro`, `GenerateCoverageAsync`) — siga o padrão do arquivo em que você está mexendo, em vez de tentar normalizar.
- Os arquivos do designer do WinForms (`*.Designer.cs`) podem ser editados à mão, mas são regravados pelo designer do Visual Studio; prefira colocar lógica no `.cs` correspondente.
