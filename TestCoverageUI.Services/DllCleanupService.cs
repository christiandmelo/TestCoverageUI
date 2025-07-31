using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestCoverageUI.Services
{
  public class DllCleanupService
  {
    public void ApagarDllsComFiltro(string binPath, string prefixDll, string suffixDll)
    {
      if (string.IsNullOrWhiteSpace(binPath) || !Directory.Exists(binPath))
        throw new DirectoryNotFoundException($"Pasta não encontrada: {binPath}");

      // Buscar arquivos que comecem e terminem conforme o filtro
      var arquivos = Directory.GetFiles(binPath)
          .Where(file =>
          {
            var nome = Path.GetFileName(file);
            return nome.StartsWith(prefixDll, StringComparison.OrdinalIgnoreCase) &&
                     nome.EndsWith(suffixDll, StringComparison.OrdinalIgnoreCase);
          })
          .ToList();

      foreach (var arquivo in arquivos)
      {
        try
        {
          File.Delete(arquivo);
        }
        catch (Exception ex)
        {
          throw new IOException($"Erro ao apagar o arquivo {arquivo}: {ex.Message}", ex);
        }
      }
    }
  }
}
