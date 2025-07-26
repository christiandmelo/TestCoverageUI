using System;
using System.IO;
using System.Windows.Forms;
using TestCoverageUI.Models;
using TestCoverageUI.Services;

namespace TestCoverageUI.UI
{
  public partial class MainForm : Form
  {
    private ProfilesConfig _profilesConfig;
    private CoverageProfile _selectedProfile;

    public MainForm()
    {
      InitializeComponent();
      LoadConfig();
    }

    private void LoadConfig()
    {
      _profilesConfig = ProfilesConfig.Load();
      comboProfiles.Items.Clear();

      foreach (var name in _profilesConfig.GetProfileNames())
      {
        comboProfiles.Items.Add(name);
      }

      if (comboProfiles.Items.Count > 0)
        comboProfiles.SelectedIndex = 0;
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

    private void comboProfiles_SelectedIndexChanged(object sender, EventArgs e)
    {
      var profileName = comboProfiles.SelectedItem?.ToString();
      if (string.IsNullOrEmpty(profileName))
        return;

      _selectedProfile = _profilesConfig.GetProfile(profileName);
      txtBinPath.Text = _selectedProfile.BinPath;
    }

    private async void BtnGerarRelatorio_Click(object sender, EventArgs e)
    {
      if (_selectedProfile == null)
      {
        AppendLog("Nenhum perfil selecionado.", Color.Red);
        return;
      }

      // Limpar log e ir para aba de Log
      txtLog.Clear();
      tabControl.SelectedTab = tabLog;
      btnGerarRelatorio.Enabled = false;

      // Executa o serviço
      var service = new CoverageService(_selectedProfile, log =>
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

      string? reportPath = await service.GenerateCoverageAsync();

      btnGerarRelatorio.Enabled = true;

      if (!string.IsNullOrEmpty(reportPath) && File.Exists(reportPath))
      {
        await webViewRelatorio.EnsureCoreWebView2Async(null);
        webViewRelatorio.Source = new Uri(reportPath);
        tabControl.SelectedTab = tabRelatorio;
      }
      else
      {
        tabControl.SelectedTab = tabLog;
        AppendLog("Falha ao gerar relatório.", Color.Red);
      }
    }

    private void menuEditarPerfil_Click(object sender, EventArgs e)
    {
      string perfilSelecionado = comboProfiles.SelectedItem?.ToString();
      if (string.IsNullOrEmpty(perfilSelecionado))
      {
        MessageBox.Show("Selecione um perfil para editar.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
      }

      using (var configForm = new ConfigForm(perfilSelecionado))
      {
        if (configForm.ShowDialog() == DialogResult.OK)
        {
          LoadConfig();
          comboProfiles.SelectedItem = perfilSelecionado; // mantém seleção
        }
      }
    }

    private void menuNovoPerfil_Click(object sender, EventArgs e)
    {
      using (var configForm = new ConfigForm()) // sem parâmetros → novo perfil
      {
        if (configForm.ShowDialog() == DialogResult.OK)
        {
          LoadConfig();
        }
      }
    }


  }
}
