using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace TestCoverageUI.Preloader
{
  internal class Program
  {
    static void Main(string[] args)
    {
      Console.OutputEncoding = Encoding.UTF8;

      if (args.Length < 2)
      {
        Console.WriteLine("Uso incorreto: Preloader.exe <binPath> <prefixDll>");
        return;
      }

      string binPath = args[0];
      string prefixDll = args[1];

      if (!Directory.Exists(binPath))
      {
        Console.WriteLine($"ERRO: Pasta não encontrada: {binPath}");
        return;
      }

      // Além da raiz do Bin, o RM resolve assemblies também em subpastas de
      // customização (mesma convenção do <probing privatePath="Custom;...;assemblies">
      // já usada nos .dll.config do próprio ambiente). Em caso de nome duplicado,
      // a versão em "Custom" prevalece, por ser a pasta de customização.
      string[] pastasDeBusca =
      {
        binPath,
        Path.Combine(binPath, "Custom"),
        Path.Combine(binPath, "assemblies")
      };

      var dllsPorNome = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

      foreach (var pasta in pastasDeBusca)
      {
        if (!Directory.Exists(pasta))
          continue;

        foreach (var dll in Directory.GetFiles(pasta, $"{prefixDll}*.dll", SearchOption.TopDirectoryOnly))
        {
          dllsPorNome[Path.GetFileName(dll)] = dll;
        }
      }

      Console.WriteLine($"Pré-carregando {dllsPorNome.Count} DLL(s) que batem com o prefixo \"{prefixDll}\"...");

      int carregadas = 0;

      foreach (var dll in dllsPorNome.Values)
      {
        try
        {
          Assembly.LoadFrom(dll);
          carregadas++;
        }
        catch (Exception ex)
        {
          Console.WriteLine($"[AVISO] Falha ao carregar {Path.GetFileName(dll)}: {ex.Message}");
        }
      }

      Console.WriteLine($"{carregadas} de {dllsPorNome.Count} DLLs carregadas.");
    }
  }
}
