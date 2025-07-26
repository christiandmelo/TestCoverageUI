using System.Text.Json;

namespace TestCoverageUI.Models
{
  public class ProfilesConfig
  {
    public List<CoverageProfile> Profiles { get; set; } = new();

    private static string ConfigPath =>
        Path.Combine(AppContext.BaseDirectory, "configs.json");

    public static ProfilesConfig Load()
    {
      string json = File.ReadAllText(ConfigPath);
      return JsonSerializer.Deserialize<ProfilesConfig>(json) ?? new ProfilesConfig();
    }

    public static void Save(ProfilesConfig config)
    {
      string json = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
      File.WriteAllText(ConfigPath, json);
    }

    public CoverageProfile? GetProfile(string name)
    {
      return Profiles.FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    public void AddOrUpdateProfile(CoverageProfile profile)
    {
      var existing = GetProfile(profile.Name);
      if (existing != null)
      {
        Profiles.Remove(existing);
      }
      Profiles.Add(profile);
      Save(this);
    }

    public void RemoveProfile(string name)
    {
      Profiles.RemoveAll(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
      Save(this);
    }

    public List<string> GetProfileNames()
    {
      return Profiles.Select(p => p.Name).ToList();
    }

    public static void EnsureConfigExists()
    {
      if (!File.Exists(ConfigPath))
      {
        // Cria perfil padrão
        var defaultProfile = new CoverageProfile
        {
          Name = "Default",
          OpenCoverPath = "Tools\\OpenCover\\OpenCover.Console.exe",
          ReportGeneratorPath = "Tools\\ReportGenerator\\net8.0\\reportgenerator.exe",
          VSTestPath = "C:\\Program Files\\Microsoft Visual Studio\\2022\\Professional\\Common7\\IDE\\CommonExtensions\\Microsoft\\TestWindow\\vstest.console.exe",
          BinPath = "C:\\RM\\Atual\\Release\\Bin",
          PrefixDll = "RM.Cst",
          SuffixDll = ".TesteUnitario",
          UseEmbeddedTools = true
        };

        var profilesConfig = new ProfilesConfig();
        profilesConfig.Profiles.Add(defaultProfile);

        Save(profilesConfig);
      }
    }
  }
}
