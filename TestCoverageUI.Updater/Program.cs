using System.Diagnostics;
using System.IO.Compression;

namespace TestCoverageUI.Updater
{
  internal class Program
  {
    static void Main(string[] args)
    {
      if (args.Length < 2)
      {
        Console.WriteLine("Uso: Updater.exe <zipPath> <targetExePath>");
        return;
      }

      string zipPath = args[0];
      string targetExePath = args[1];
      string targetDir = Path.GetDirectoryName(targetExePath)!;

      try
      {
        Console.WriteLine("Aguardando fechamento do TestCoverageUI...");
        WaitForProcessExit(Path.GetFileNameWithoutExtension(targetExePath));

        Console.WriteLine("Extraindo atualização...");
        ZipFile.ExtractToDirectory(zipPath, targetDir, overwriteFiles: true);

        Console.WriteLine("Iniciando aplicação atualizada...");
        Process.Start(targetExePath);
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Erro ao atualizar: {ex.Message}");
      }
    }

    private static void WaitForProcessExit(string processName)
    {
      Process[] processes;
      do
      {
        Thread.Sleep(500);
        processes = Process.GetProcessesByName(processName);
      } while (processes.Length > 0);
    }
  }
}
