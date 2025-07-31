using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;

namespace TestCoverageUI.Updater
{
  internal class Program
  {
    static void Main(string[] args)
    {
      Console.WriteLine("=== Iniciando processo de atualização ===");

      if (args.Length < 2)
      {
        Console.WriteLine("Uso incorreto: Updater.exe <zipPath> <targetExePath>");
        Console.WriteLine("Parâmetros recebidos:");
        foreach (var arg in args)
          Console.WriteLine($" - {arg}");
        EsperarFechar();
        return;
      }

      string zipPath = args[0];
      string targetExePath = args[1];
      string targetDir = Path.GetDirectoryName(targetExePath)!;

      Console.WriteLine($"ZIP recebido: {zipPath}");
      Console.WriteLine($"Pasta de destino: {targetDir}");

      try
      {
        // 1. Aguardar fechamento do aplicativo principal
        Console.WriteLine($"Aguardando fechamento do {Path.GetFileNameWithoutExtension(targetExePath)}...");
        WaitForProcessExit(Path.GetFileNameWithoutExtension(targetExePath));

        // 2. Validar ZIP
        if (!File.Exists(zipPath))
        {
          Console.WriteLine("ERRO: Arquivo ZIP não encontrado.");
          EsperarFechar();
          return;
        }

        // 3. Extrair atualização para pasta temporária
        string tempExtractPath = Path.Combine(Path.GetTempPath(), "TestCoverageUI_Extract");
        if (Directory.Exists(tempExtractPath))
          Directory.Delete(tempExtractPath, true);
        Directory.CreateDirectory(tempExtractPath);

        Console.WriteLine("Extraindo atualização para pasta temporária...");
        ZipFile.ExtractToDirectory(zipPath, tempExtractPath, true);

        // 4. Copiar arquivos ignorando o Updater.exe
        string currentUpdaterName = Path.GetFileName(Process.GetCurrentProcess().MainModule.FileName);

        foreach (var file in Directory.GetFiles(tempExtractPath, "*", SearchOption.AllDirectories))
        {
          string fileName = Path.GetFileName(file);

          // Ignorar o próprio Updater.exe
          if (fileName.Equals(currentUpdaterName, StringComparison.OrdinalIgnoreCase))
          {
            Console.WriteLine($"Ignorando {fileName} para evitar sobrescrever o Updater em uso.");
            continue;
          }

          // Caminho de destino
          string relativePath = Path.GetRelativePath(tempExtractPath, file);
          string destFile = Path.Combine(targetDir, relativePath);

          // Garantir que a pasta exista
          Directory.CreateDirectory(Path.GetDirectoryName(destFile)!);

          // Copiar substituindo
          File.Copy(file, destFile, true);
          Console.WriteLine($"Atualizado: {relativePath}");
        }

        // 5. Limpar pasta temporária
        Directory.Delete(tempExtractPath, true);

        // 6. Iniciar aplicação atualizada
        Console.WriteLine("Iniciando aplicação atualizada...");
        Process.Start(targetExePath);

        Console.WriteLine("Atualização concluída com sucesso.");
      }
      catch (Exception ex)
      {
        Console.WriteLine($"ERRO durante atualização: {ex.Message}");
        Console.WriteLine(ex.StackTrace);
      }

      EsperarFechar();
    }

    private static void WaitForProcessExit(string processName)
    {
      Process[] processes;
      do
      {
        Thread.Sleep(500);
        processes = Process.GetProcessesByName(processName);
      } while (processes.Any());
    }

    private static void EsperarFechar()
    {
      Console.WriteLine("Pressione qualquer tecla para fechar...");
      Console.ReadKey();
    }
  }
}
