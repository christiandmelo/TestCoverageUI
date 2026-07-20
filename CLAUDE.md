# CLAUDE.md

Este arquivo fornece orientações ao Claude Code (claude.ai/code) ao trabalhar com o código deste repositório.

## O que é

Uma interface desktop em Windows Forms (.NET 8) para rodar cobertura de testes .NET sem linha de comando. A aplicação chama o **OpenCover** (que executa o `vstest.console.exe` sob o profiler de cobertura) e o **ReportGenerator** (que transforma o XML bruto em HTML), e depois exibe o HTML resultante em um controle **WebView2** embutido.

O projeto não é uma biblioteca — ele orquestra executáveis externos via `Process`. A maior parte dos bugs está na montagem das strings de argumento e na resolução de caminhos, não no C#.

Este repositório é um **submódulo do [apps-cm](https://github.com/christiandmelo/apps)**, o shell que centraliza e distribui todas as ferramentas do time. Ele continua compilando e rodando standalone (útil para desenvolvimento), mas o executável distribuído aos usuários finais é o `AppsCm.exe` do apps-cm, que hospeda o `MainForm` deste projeto reparenteado dentro de uma aba.

## Comandos

```bash
dotnet restore
dotnet build TestCoverageUI.sln
dotnet run --project TestCoverageUI.UI                # rodar em desenvolvimento
dotnet publish TestCoverageUI.UI -c Release -r win-x64 --self-contained false -o publish
```

**Não existem projetos de teste nem configuração de linter** nesta solução. O `dotnet test` não vai encontrar nada — não inclua esse passo esperando alguma saída.

## Arquitetura

Três projetos, em camadas bem definidas (`UI → Services → Models`), mais o `Preloader`, que é um helper standalone:

- **TestCoverageUI.Models** — `CoverageProfile` (uma configuração nomeada), `ProfilesConfig` (a lista, com a lógica de carregar/salvar/semear o JSON) e `CaminhoConfig` (resolve o diretório de configuração em `%LOCALAPPDATA%\apps-cm\test-coverage\` e migra um `configs.json` legado que ficava ao lado do exe). A persistência mora no próprio model; não há repositório nem container de injeção de dependência em lugar nenhum.
- **TestCoverageUI.Services** — `CoverageService`, o pipeline OpenCover/ReportGenerator, e `DllCleanupService`, que apaga em lote as DLLs que casam com prefixo/sufixo na pasta bin.
- **TestCoverageUI.UI** — `MainForm` (seletor de perfil, aba de log, aba de relatório) e `ConfigForm` (editor de perfil). O assembly é renomeado para `TestCoverageUI.exe`. Note que, quando hospedado pelo shell (`apps-cm`), esse `.exe` nunca chega a rodar — o shell referencia o projeto e reparenteia o `MainForm` diretamente.
- **TestCoverageUI.Preloader** — `.exe` separado (`net472`) invocado como processo filho pelo `CoverageService` para pré-carregar DLLs de produção antes do profiler do OpenCover entrar em ação. Precisa ser um processo à parte porque roda sob o profiler.

### Pipeline de cobertura (`CoverageService.GenerateCoverageAsync`)

Busca `{PrefixDll}*{SuffixDll}.dll` no `BinPath`, roda o OpenCover **uma vez por DLL de teste** com `-mergeoutput` acumulando tudo em um único `coverage.xml`, e por fim roda o ReportGenerator uma vez sobre esse XML. As saídas caem em `%LOCALAPPDATA%\apps-cm\test-coverage\saida\` (`coverage.xml`, `coverage-report/index.htm`) — um diretório fixo, não relativo ao diretório de trabalho do processo, para não depender de quem lançou o exe (o Updater do shell, por exemplo, relança sem setar `WorkingDirectory` explicitamente do lado da UI). Retorna o caminho do relatório ou `null`; todo caminho de falha registra no log e retorna `null` em vez de lançar exceção.

O log é um callback `Action<string>` injetado no service. O `MainForm` colore as linhas comparando trechos do texto (`"Passed"` → verde, `"Erro"` → vermelho, etc.), então **mudar o texto de uma mensagem no service altera silenciosamente as cores do log na UI**.

### Configuração

`configs.json` fica em `%LOCALAPPDATA%\apps-cm\test-coverage\configs.json` (ver `CaminhoConfig.cs`), criado com um perfil `Default` hardcoded pelo `ProfilesConfig.EnsureConfigExists()`, chamado de `MainForm.LoadConfig()` — ou seja, roda tanto no `Program.Main` standalone quanto sob o shell, já que o shell nunca invoca este `Program.Main`. Na primeira execução depois de uma instalação antiga, `CaminhoConfig.MigrarSeNecessario()` copia (não move) um `configs.json` legado que estivesse ao lado do exe, validando antes que o schema bate (`Profiles` presente) — isso evita que a config de um outro app do bundle (ex.: `git-submodule-sync`, que também usava esse nome de arquivo no mesmo diretório) seja adotada por engano.

Os caminhos das ferramentas no perfil podem ser relativos; o `CoverageService.ResolveToolPath` os rebaseia a partir do `AppContext.BaseDirectory` — que, sob o shell, é a pasta do `AppsCm.exe`, não a deste projeto. A pasta `Tools/`, com o OpenCover e o ReportGenerator embutidos, **está versionada** neste repositório (`TestCoverageUI.UI/Tools/**`) e é copiada automaticamente para bin/publish via `<None Include="Tools\**" CopyToOutputDirectory="PreserveNewest" />` — esse item flui transitivamente para quem referenciar `TestCoverageUI.UI.csproj` via `ProjectReference` (como o `AppsCm.Shell`), então não precisa de nenhum passo extra no shell.

O `CoverageProfile.UseEmbeddedTools` é persistido e habilita/desabilita os botões Browse no `ConfigForm`, mas o `CoverageService` nunca lê esse campo. O `-filter` do OpenCover é montado em tempo de execução a partir de `PrefixDll`/`SuffixDll` — não existe campo de filtro armazenado, apesar do que diz o `readme.md`.

### Preloader e $(RaizRepo)

O `TestCoverageUI.UI.csproj` builda o Preloader (`net472`, sem RID/publish, porque ele roda sob o profiler do OpenCover) via um target `BeforeTargets="PrepareForBuild"` e inclui o `.exe` resultante como item dinâmico `BeforeTargets="AssignTargetPaths"` — precisa ser dinâmico (não um `<None>` estático) porque o arquivo ainda não existe quando o csproj é avaliado, e precisa ser antes de `AssignTargetPaths` para fluir transitivamente a quem referencia este projeto (o shell). Os caminhos usam `$(MSBuildThisFileDirectory)..\`, não `$(SolutionDir)` — `$(SolutionDir)` resolveria para o `.sln` que está dirigindo o build no momento (`TestCoverageUI.sln` num build standalone, mas `AppsCm.sln` quando o shell builda este projeto via `ProjectReference`), quebrando o caminho no segundo caso.

### Auto-update

Não existe mais neste repositório. Antes o `TestCoverageUI.Updater` e o `UpdateService` cuidavam disso; agora só o **apps-cm** (o shell) se atualiza — um único `AppsCm.exe`, uma única versão, um único fluxo de update para todos os apps do bundle. Ver `apps-cm/CLAUDE.md` (ou o `Release.ps1` na raiz do apps-cm) para o fluxo de release.

### Versionamento

A versão vem de um único `Directory.Build.props` na raiz deste repositório (`<Version>`/`<AssemblyVersion>`/`<FileVersion>`), aplicado a todos os projetos automaticamente — não precisa mais ser duplicada manualmente em cada `.csproj` (esse era o bug antigo: o `VersionHelper` do updater lia a versão do assembly *Services*, que podia ficar dessincronizada da versão do assembly *UI*). Essa versão identifica apenas este submódulo (aparece, por exemplo, no título de janela ou em logs) — ela não é comparada com nada; quem decide se há atualização disponível é o shell, comparando a versão *dele*, não a deste projeto.

## Convenções

- **Indentação de 2 espaços**; `Nullable`, `ImplicitUsings` e `LangVersion=latest` habilitados via `Directory.Build.props`.
- O código é em **português (pt-BR)**: textos de UI, mensagens de log, mensagens de exceção, comentários e mensagens de commit. Os nomes de métodos misturam português e inglês (`ApagarDllsComFiltro`, `GenerateCoverageAsync`) — siga o padrão do arquivo em que você está mexendo, em vez de tentar normalizar.
- Os arquivos do designer do WinForms (`*.Designer.cs`) podem ser editados à mão, mas são regravados pelo designer do Visual Studio; prefira colocar lógica no `.cs` correspondente.
