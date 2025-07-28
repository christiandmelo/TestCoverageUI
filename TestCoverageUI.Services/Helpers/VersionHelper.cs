using System.Reflection;

namespace TestCoverageUI.Services.Helpers
{
  public static class VersionHelper
  {
    public static string GetCurrentVersion()
    {
      return Assembly.GetExecutingAssembly()
                     .GetName()
                     .Version?
                     .ToString() ?? "1.0.0";
    }
  }
}
