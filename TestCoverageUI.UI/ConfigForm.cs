using TestCoverageUI.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TestCoverageUI.UI
{
  public partial class ConfigForm : Form
  {
    private CoverageProfile _editingProfile;
    private bool _isEditing = false;

    public ConfigForm()
    {
      InitializeComponent();

      txtOpenCover.Text = "Tools\\OpenCover\\OpenCover.Console.exe";
      txtReportGen.Text = "Tools\\ReportGenerator\\net8.0\\reportgenerator.exe";
      txtVSTest.Text = "C:\\Program Files\\Microsoft Visual Studio\\2022\\Professional\\Common7\\IDE\\CommonExtensions\\Microsoft\\TestWindow\\vstest.console.exe";
    }

    public ConfigForm(string profileName)
    {
      InitializeComponent();

      var config = ProfilesConfig.Load();
      _editingProfile = config.GetProfile(profileName);

      if (_editingProfile != null)
      {
        _isEditing = true;
        PreencherCampos(_editingProfile);
      }
    }

    private void PreencherCampos(CoverageProfile profile)
    {
      txtProfileName.Text = profile.Name;
      txtOpenCover.Text = profile.OpenCoverPath;
      txtReportGen.Text = profile.ReportGeneratorPath;
      txtVSTest.Text = profile.VSTestPath;
      txtBinPath.Text = profile.BinPath;
      txtPrefixDll.Text = profile.PrefixDll;
      txtSuffixDll.Text = profile.SuffixDll;
      chkUseEmbedded.Checked = profile.UseEmbeddedTools;

      // Bloqueia o nome do perfil se estiver editando (evita renomear sem querer)
      if (_isEditing)
        txtProfileName.ReadOnly = false;

      EnabledOrNotExeInputs();
    }


    private void EnabledOrNotExeInputs()
    {
      if (chkUseEmbedded.Checked)
      {
        btnBrowseOpenCover.Enabled = false;
        btnBrowseReportGen.Enabled = false;
        btnBrowseVSTest.Enabled = false;
      }
      else
      {
        btnBrowseOpenCover.Enabled = true;
        btnBrowseReportGen.Enabled = true;
        btnBrowseVSTest.Enabled = true;
      }
    }

    private void BtnSave_Click(object sender, EventArgs e)
    {
      if (string.IsNullOrWhiteSpace(txtProfileName.Text))
      {
        MessageBox.Show("Informe um nome para o perfil.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
      }

      var profile = new CoverageProfile
      {
        Name = txtProfileName.Text.Trim(),
        OpenCoverPath = txtOpenCover.Text.Trim(),
        ReportGeneratorPath = txtReportGen.Text.Trim(),
        VSTestPath = txtVSTest.Text.Trim(),
        BinPath = txtBinPath.Text.Trim(),
        PrefixDll = txtPrefixDll.Text.Trim(),
        SuffixDll = txtSuffixDll.Text.Trim(),
        UseEmbeddedTools = chkUseEmbedded.Checked
      };

      var config = ProfilesConfig.Load();
      config.AddOrUpdateProfile(profile);

      this.DialogResult = DialogResult.OK;
      this.Close();
    }


    private void BtnCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
      Close();
    }

    private void BtnBrowseOpenCover_Click(object sender, EventArgs e)
    {
      var caminho = OpenFilePicker("Executável OpenCover|*.exe");

      if (!string.IsNullOrEmpty(caminho))
        txtOpenCover.Text = caminho;
    }

    private void BtnBrowseReportGen_Click(object sender, EventArgs e)
    {
      var caminho = OpenFilePicker("Executável ReportGenerator|*.exe");

      if (!string.IsNullOrEmpty(caminho))
        txtReportGen.Text = caminho;
    }

    private void BtnBrowseVSTest_Click(object sender, EventArgs e)
    {
      var caminho = OpenFilePicker("Executável VSTest|*.exe");

      if (!string.IsNullOrEmpty(caminho))
        txtVSTest.Text = caminho;
    }

    private void BtnBrowseBin_Click(object sender, EventArgs e)
    {
      using (var dialog = new FolderBrowserDialog())
      {
        if (dialog.ShowDialog() == DialogResult.OK)
          txtBinPath.Text = dialog.SelectedPath;
      }
    }

    private string OpenFilePicker(string filter)
    {
      using (var dialog = new OpenFileDialog())
      {
        dialog.Filter = filter;
        return dialog.ShowDialog() == DialogResult.OK ? dialog.FileName : string.Empty;
      }
    }

    private void chkUseEmbedded_Click(object sender, EventArgs e)
    {
      EnabledOrNotExeInputs();
    }
  }
}
