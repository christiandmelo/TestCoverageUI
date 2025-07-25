using System;
using System.IO;
using System.Text.Json;

namespace TestCoverageUI.Models
{
  public class CoverageConfig
  {
    public string OpenCoverPath { get; set; } = string.Empty;
    public string ReportGeneratorPath { get; set; } = string.Empty;
    public string VSTestPath { get; set; } = string.Empty;
    public string BinPath { get; set; } = string.Empty;

    public string PrefixDll { get; set; } = "";
    public string SuffixDll { get; set; } = "";

    public bool UseEmbeddedTools { get; set; } = true;

    private static string ConfigDirectory =>
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TestCoverageUI");

    private static string ConfigPath => Path.Combine(ConfigDirectory, "config.json");

    public static CoverageConfig LoadConfig()
    {
      if (File.Exists(ConfigPath))
      {
        string json = File.ReadAllText(ConfigPath);
        return JsonSerializer.Deserialize<CoverageConfig>(json) ?? new CoverageConfig();
      }

      return new CoverageConfig();
    }

    public static void EnsureConfigExists()
    {
      if (!Directory.Exists(ConfigDirectory))
        Directory.CreateDirectory(ConfigDirectory);

      if (!File.Exists(ConfigPath))
      {
        var defaultConfig = new CoverageConfig
        {
          OpenCoverPath = string.Empty, // Pode ficar em branco para ser configurado
          ReportGeneratorPath = string.Empty,
          VSTestPath = "C:\\Program Files\\Microsoft Visual Studio\\2022\\Professional\\Common7\\IDE\\CommonExtensions\\Microsoft\\TestWindow\\vstest.console.exe",
          BinPath = "C:\\RM\\Atual\\Release\\Bin",
          PrefixDll = "RM.Cst",
          SuffixDll = ".TesteUnitario",
          UseEmbeddedTools = true
        };

        SaveConfig(defaultConfig);
      }
    }

    public static void SaveConfig(CoverageConfig config)
    {
      if (!Directory.Exists(ConfigDirectory))
        Directory.CreateDirectory(ConfigDirectory);

      string json = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
      File.WriteAllText(ConfigPath, json);
    }
  }
}
