using System.Diagnostics;
using TestCoverageUI.Models;

namespace TestCoverageUI.Services
{
  public class CoverageService
  {
    private readonly CoverageConfig _config;
    private readonly Action<string> _logCallback;

    public CoverageService(CoverageConfig config, Action<string> logCallback)
    {
      _config = config;
      _logCallback = logCallback;
    }

    /// <summary>
    /// Executa o fluxo de geração de cobertura.
    /// </summary>
    public async Task<string?> GenerateCoverageAsync()
    {
      try
      {
        Log("Iniciando geração de cobertura...");

        // Validar caminhos ou usar embutidos
        string openCoverPath = ResolveToolPath(_config.OpenCoverPath, "OpenCover.Console.exe");
        string reportGenPath = ResolveToolPath(_config.ReportGeneratorPath, "reportgenerator.exe");

        if (!File.Exists(openCoverPath))
        {
          Log("ERRO: OpenCover não encontrado.");
          return null;
        }

        if (!File.Exists(reportGenPath))
        {
          Log("ERRO: ReportGenerator não encontrado.");
          return null;
        }

        // Limpar arquivos anteriores
        string coverageXml = Path.Combine(Environment.CurrentDirectory, "coverage.xml");
        string reportFolder = Path.Combine(Environment.CurrentDirectory, "coverage-report");

        if (File.Exists(coverageXml)) File.Delete(coverageXml);
        if (Directory.Exists(reportFolder)) Directory.Delete(reportFolder, true);

        // Procurar DLLs de teste
        string pattern = $"{_config.PrefixDll}*{_config.SuffixDll}.dll";
        string[] dlls = Directory.GetFiles(_config.BinPath, pattern, SearchOption.TopDirectoryOnly);

        if (dlls.Length == 0)
        {
          Log("Nenhuma DLL encontrada com o padrão especificado.");
          return null;
        }

        // Executar OpenCover para cada DLL
        foreach (var dll in dlls)
        {
          Log($"Executando cobertura para: {Path.GetFileName(dll)}");

          string arguments = $"-register:user " +
                             $"-target:\"{_config.VSTestPath}\" " +
                             $"-targetargs:\"\\\"{dll}\\\" --logger:trx\" " +
                             $"-output:\"{coverageXml}\" " +
                             $"-mergeoutput " +
                             $"-filter:\"+[{_config.PrefixDll}*]* -[*.{_config.SuffixDll}]*\" " +
                             $"-log:All";

          bool success = await RunProcessAsync(openCoverPath, arguments);
          if (!success)
            return null;
        }

        // Gerar relatório com ReportGenerator
        Log("Gerando relatório HTML...");

        string reportArgs = $"-reports:\"{coverageXml}\" " +
                            $"-targetdir:\"{reportFolder}\" " +
                            $"-reporttypes:Html";

        bool reportSuccess = await RunProcessAsync(reportGenPath, reportArgs);
        if (!reportSuccess)
          return null;

        string indexPath = Path.Combine(reportFolder, "index.htm");
        Log($"Relatório gerado em: {indexPath}");

        return indexPath;
      }
      catch (Exception ex)
      {
        Log($"Erro inesperado: {ex.Message}");
        return null;
      }
    }

    /// <summary>
    /// Resolve se usa o caminho definido pelo usuário ou o executável embutido.
    /// </summary>
    private string ResolveToolPath(string userPath, string embeddedFileName)
    {
      // Se usuário definiu caminho válido, usa ele
      if (!string.IsNullOrWhiteSpace(userPath) && File.Exists(userPath))
        return userPath;

      if (_config.UseEmbeddedTools)
      {
        // Caminho onde extrairemos os executáveis embutidos
        string embeddedDir = Path.Combine(AppContext.BaseDirectory, "Tools");
        string embeddedExe = Path.Combine(embeddedDir, embeddedFileName);

        if (!Directory.Exists(embeddedDir))
          Directory.CreateDirectory(embeddedDir);

        // TODO: Extrair executável embutido do recurso (faremos depois)
        return embeddedExe;
      }

      return userPath;
    }

    /// <summary>
    /// Executa um processo externo e redireciona a saída para o log.
    /// </summary>
    private async Task<bool> RunProcessAsync(string exePath, string arguments)
    {
      Log($"> {exePath} {arguments}");

      var psi = new ProcessStartInfo
      {
        FileName = exePath,
        Arguments = arguments,
        RedirectStandardOutput = true,
        RedirectStandardError = true,
        UseShellExecute = false,
        CreateNoWindow = true,

        // CORREÇÃO DE ENCODING
        StandardOutputEncoding = System.Text.Encoding.UTF8,
        StandardErrorEncoding = System.Text.Encoding.UTF8
      };

      var process = new Process { StartInfo = psi };

      process.OutputDataReceived += (s, e) =>
      {
        if (e.Data != null)
          Log(e.Data);
      };

      process.ErrorDataReceived += (s, e) =>
      {
        if (e.Data != null)
          Log($"[ERRO] {e.Data}");
      };

      process.Start();
      process.BeginOutputReadLine();
      process.BeginErrorReadLine();

      await process.WaitForExitAsync();

      return process.ExitCode == 0;
    }


    private void Log(string message)
    {
      _logCallback?.Invoke($"[{DateTime.Now:HH:mm:ss}] {message}\r\n");
    }
  }
}
