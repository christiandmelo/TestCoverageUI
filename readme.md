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

```bash
dotnet publish -c Release -r win-x64 --self-contained false
```

Isso gera os binários em `bin/Release/net8.0/win-x64/publish/` junto com a pasta `Tools`.

---

## Licença

Este projeto é open-source sob a licença MIT.

