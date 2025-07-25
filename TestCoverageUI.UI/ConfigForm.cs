using TestCoverageUI.Models;

namespace TestCoverageUI.UI
{
  public partial class ConfigForm : Form
  {
    private CoverageConfig _config;

    public ConfigForm()
    {
      InitializeComponent();
      LoadConfig();
    }

    private void LoadConfig()
    {
      _config = CoverageConfig.LoadConfig();

      txtOpenCover.Text = _config.OpenCoverPath;
      txtReportGen.Text = _config.ReportGeneratorPath;
      txtVSTest.Text = _config.VSTestPath;
      txtBinPath.Text = _config.BinPath;
      txtPrefixDll.Text = _config.PrefixDll;
      txtSuffixDll.Text = _config.SuffixDll;
      chkUseEmbedded.Checked = _config.UseEmbeddedTools;
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
      _config.OpenCoverPath = txtOpenCover.Text;
      _config.ReportGeneratorPath = txtReportGen.Text;
      _config.VSTestPath = txtVSTest.Text;
      _config.BinPath = txtBinPath.Text;
      _config.PrefixDll = txtPrefixDll.Text;
      _config.SuffixDll = txtSuffixDll.Text;
      _config.UseEmbeddedTools = chkUseEmbedded.Checked;

      CoverageConfig.SaveConfig(_config);
      this.DialogResult = DialogResult.OK;
      Close();
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
