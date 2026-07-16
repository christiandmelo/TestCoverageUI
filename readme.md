# TestCoverageUI

**TestCoverageUI** é uma ferramenta Windows Forms que permite executar e visualizar a cobertura de testes em projetos .NET Framework e .NET Core/5+/6+/7+/8+ usando **OpenCover** e **ReportGenerator**, sem a necessidade de usar linha de comando.

---

## Recursos

- **Execução simplificada**: clique único para rodar testes e gerar o relatório de cobertura.
- **Visualização integrada**: relatório exibido diretamente na aplicação usando **WebView2**.
- **Configuração personalizável**:
  - Caminhos para executáveis (`OpenCover`, `ReportGenerator`, `vstest.console.exe`).
  - Filtro de cobertura.
  - Prefixo e sufixo para localizar DLLs de teste.
- **Ferramentas embutidas**: versão do OpenCover e ReportGenerator já incluídas na pasta `Tools`.
- **Logs coloridos**: acompanhamento em tempo real estilo terminal (fundo preto, cores para status).

---

## Estrutura do Projeto

```
TestCoverageUI/
│
├── UI/                 # Telas principais (MainForm, ConfigForm)
├── Services/           # Lógica de execução de cobertura
├── Models/             # Modelos (ex.: CoverageConfig)
├── Utils/              # Helpers (ex.: leitura de config)
├── Tools/              # OpenCover e ReportGenerator embutidos
└── Program.cs          # Ponto de entrada
```

---

## Como funciona

1. Ao iniciar, o projeto verifica se existe `config.json` (em `%AppData%/TestCoverageUI`).  
   - Se não existir, cria automaticamente com valores padrão.
2. Tela principal mostra caminho dos binários e permite rodar o relatório.
3. Logs coloridos são exibidos na aba **Log**.
4. Ao concluir, a aba **Relatório** é exibida automaticamente com o HTML gerado.

---

## Requisitos

- .NET 8 SDK ou runtime instalado
- Windows 10 ou superior
- [WebView2 Runtime](https://developer.microsoft.com/en-us/microsoft-edge/webview2/) (necessário para exibir o relatório)

---

## Como usar

### 1. Clonar o repositório

```bash
git clone https://github.com/seuusuario/TestCoverageUI.git
cd TestCoverageUI
```

### 2. Restaurar dependências

```bash
dotnet restore
```

### 3. Rodar no modo desenvolvimento

```bash
dotnet run --project TestCoverageUI.UI
```

---

## Configuração

Arquivo `config.json` é criado automaticamente em `%AppData%/TestCoverageUI/config.json`:

```json
{
  "OpenCoverPath": "",
  "ReportGeneratorPath": "",
  "VSTestPath": "",
  "BinPath": "",
  "PrefixDll": "RM.Cst",
  "SuffixDll": ".TesteUnitario",
  "CoverageFilter": "+[*]* -[*.TesteUnitario]*",
  "UseEmbeddedTools": true
}
```

- `UseEmbeddedTools: true` → Usa executáveis na pasta `Tools` incluída na build
- Alterar pelo **Configurações** no menu do app ou manualmente no JSON

---

## Filtros de cobertura

Exemplo de filtro padrão usado:

```
+[RM.Cst*]* -[*.TesteUnitario]*
```

- Inclui apenas DLLs com prefixo `RM.Cst`
- Exclui DLLs de teste (`*.TesteUnitario`)

---

## Build para distribuição

Publicar uma nova versão é um único comando, via PowerShell:

```powershell
.\Release.ps1 -Version 1.0.3
```

O script:

1. Atualiza o número da versão em `TestCoverageUI.UI/TestCoverageUI.UI.csproj`, `TestCoverageUI.Services/TestCoverageUI.Services.csproj` e `version.json` (os três precisam estar sincronizados para a atualização automática do app funcionar corretamente).
2. Roda `dotnet publish`, que já gera a aplicação completa — incluindo `Updater.exe`, `Preloader.exe` e a pasta `Tools` (OpenCover + ReportGenerator), sem nenhuma cópia manual.
3. Compacta o resultado em `TestCoverageUI.zip`, na raiz do repositório.

O script **não** commita, dá push nem cria a release no GitHub — esses passos continuam manuais.

### Passo a passo para publicar a próxima versão

Supondo que a versão atual seja `1.0.3` e a próxima seja `1.0.4`:

```powershell
# 1. Gera a versão (atualiza os 3 arquivos de versão, publica e zipa)
.\Release.ps1 -Version 1.0.4

# 2. Confere o que mudou e commita
git add TestCoverageUI.UI/TestCoverageUI.UI.csproj TestCoverageUI.Services/TestCoverageUI.Services.csproj version.json
git commit -m "Bump de versão para 1.0.4"

# 3. Sobe pro GitHub
git push origin main
```

4. Crie a release no GitHub na tag `1.0.4`, anexando o `TestCoverageUI.zip` gerado na raiz do repositório:
   - Pelo site: **Releases → Draft a new release**, tag `1.0.4`, anexar `TestCoverageUI.zip` e publicar.
   - Ou via [GitHub CLI](https://cli.github.com/):
     ```bash
     gh release create 1.0.4 TestCoverageUI.zip --title "1.0.4" --notes "Descrição das mudanças"
     ```

O nome do arquivo anexado precisa ser exatamente `TestCoverageUI.zip` e a tag precisa bater com o número de versão — é isso que a `url` em `version.json` (`.../releases/download/{versão}/TestCoverageUI.zip`) espera para o auto-update funcionar.

---

## Licença

Este projeto é open-source sob a licença MIT.

