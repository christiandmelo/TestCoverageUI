using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using TestCoverageUI.Services.Helpers;

namespace TestCoverageUI.Services
{
  public class UpdateInfo
  {
    public string version { get; set; } = string.Empty;
    public string url { get; set; } = string.Empty;
  }

  public class UpdateService
  {
    private const string VersionApiUrl = "https://raw.githubusercontent.com/christiandmelo/TestCoverageUI/main/version.json";

    /// <summary>
    /// Verifica no GitHub se existe uma nova versão.
    /// </summary>
    public async Task<UpdateInfo?> CheckForUpdateAsync()
    {
      try
      {
        using var http = new HttpClient();
        var json = await http.GetStringAsync(VersionApiUrl);

        var latest = JsonSerializer.Deserialize<UpdateInfo>(json);

        if (latest == null || string.IsNullOrWhiteSpace(latest.version))
          return null;

        var currentVersion = VersionHelper.GetCurrentVersion();

        return IsNewerVersion(currentVersion, latest.version) ? latest : null;
      }
      catch
      {
        // Se falhar a verificação, apenas ignora (não quebra o app)
        return null;
      }
    }

    /// <summary>
    /// Baixa a atualização e executa o Updater.exe
    /// </summary>
    public async Task DownloadAndRunUpdaterAsync(string downloadUrl)
    {
      string tempZip = Path.Combine(Path.GetTempPath(), "TestCoverageUI_Update.zip");

      using var http = new HttpClient();
      var data = await http.GetByteArrayAsync(downloadUrl);
      await File.WriteAllBytesAsync(tempZip, data);

      string updaterPath = Path.Combine(AppContext.BaseDirectory, "Updater.exe");

      if (!File.Exists(updaterPath))
      {
        throw new FileNotFoundException($"Updater.exe não encontrado no caminho esperado: {updaterPath}");
      }

      string exePath = Environment.ProcessPath!;

      Process.Start(new ProcessStartInfo
      {
        FileName = updaterPath,
        Arguments = $"\"{tempZip}\" \"{exePath}\"",
        UseShellExecute = true
      });

      Environment.Exit(0);
    }


    private bool IsNewerVersion(string current, string latest)
    {
      Version vCurrent = new Version(current);
      Version vLatest = new Version(latest);
      return vLatest > vCurrent;
    }
  }
}
