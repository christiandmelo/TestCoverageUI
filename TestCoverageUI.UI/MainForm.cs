using System;
using System.IO;
using System.Windows.Forms;
using TestCoverageUI.Models;
using TestCoverageUI.Services;

namespace TestCoverageUI.UI
{
  public partial class MainForm : Form
  {
    private CoverageConfig _config;

    public MainForm()
    {
      InitializeComponent();
      LoadConfig();
    }

    private void LoadConfig()
    {
      _config = CoverageConfig.LoadConfig();
      txtBinPath.Text = _config.BinPath;
    }

    private void AppendLog(string message, Color color)
    {
      // Thread-safe: redireciona para a UI Thread
      if (txtLog.InvokeRequired)
      {
        txtLog.Invoke(new Action(() => AppendLog(message, color)));
        return;
      }

      // Define posição para inserir texto colorido
      txtLog.SelectionStart = txtLog.TextLength;
      txtLog.SelectionLength = 0;

      // Cor customizada para esta mensagem
      txtLog.SelectionColor = color;

      // Garante quebra de linha automática se não existir
      if (!message.EndsWith(Environment.NewLine))
        message += Environment.NewLine;

      txtLog.AppendText(message);

      // Restaura cor padrão (branco)
      txtLog.SelectionColor = Color.White;

      // Auto-scroll para o final
      txtLog.ScrollToCaret();
    }



    private void ConfiguracoesMenuItem_Click(object sender, EventArgs e)
    {
      using (var configForm = new ConfigForm())
      {
        if (configForm.ShowDialog() == DialogResult.OK)
        {
          LoadConfig();
        }
      }
    }

    private async void BtnGerarRelatorio_Click(object sender, EventArgs e)
    {
      // Limpar log e ir para aba Log
      txtLog.Clear();
      tabControl.SelectedTab = tabLog;

      // Desabilitar botão durante execução
      btnGerarRelatorio.Enabled = false;

      // Criar serviço com callback para atualizar log
      var service = new CoverageService(_config, log =>
      {
        Color color = Color.White;

        if (log.Contains("Passed") || log.Contains("Successful") || log.Contains("Aprovado"))
          color = Color.Green;
        else if (log.Contains("Erro") || log.Contains("Falha") || log.Contains("error"))
          color = Color.Red;
        else if (log.Contains("Executando") || log.Contains("Gerando"))
          color = Color.Cyan;
        else if (log.Contains("..."))
          color = Color.Yellow;

        AppendLog(log, color);
      });

      // Executar cobertura
      string? reportPath = await service.GenerateCoverageAsync();

      // Reabilitar botão
      btnGerarRelatorio.Enabled = true;

      // Se gerou relatório com sucesso, trocar para aba Relatório
      if (!string.IsNullOrEmpty(reportPath) && File.Exists(reportPath))
      {
        await webViewRelatorio.EnsureCoreWebView2Async(null);
        webViewRelatorio.Source = new Uri(reportPath);
        tabControl.SelectedTab = tabRelatorio;
      }
      else
      {
        // Caso de erro: permanece na aba Log
        txtLog.AppendText("Falha ao gerar relatório.\r\n");
      }
    }
  }
}
