namespace TestCoverageUI.Models
{
  public class CoverageProfile
  {
    public string Name { get; set; } = string.Empty;
    public string OpenCoverPath { get; set; } = string.Empty;
    public string ReportGeneratorPath { get; set; } = string.Empty;
    public string VSTestPath { get; set; } = string.Empty;
    public string BinPath { get; set; } = string.Empty;
    public string PrefixDll { get; set; } = "";
    public string SuffixDll { get; set; } = "";
    public bool UseEmbeddedTools { get; set; } = true;
  }
}
