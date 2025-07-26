using Microsoft.Web.WebView2.WinForms;

namespace TestCoverageUI.UI
{
  partial class MainForm
  {
    private System.ComponentModel.IContainer components = null;
    private MenuStrip menuStrip;
    private ToolStripMenuItem menuNovoPerfil;
    private ToolStripMenuItem menuEditarPerfil;
    private ComboBox comboProfiles;
    private TextBox txtBinPath;
    private Button btnGerarRelatorio;
    private TabControl tabControl;
    private TabPage tabRelatorio;
    private TabPage tabLog;
    private WebView2 webViewRelatorio;
    private RichTextBox txtLog;

    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
      menuStrip = new MenuStrip();
      menuNovoPerfil = new ToolStripMenuItem();
      menuEditarPerfil = new ToolStripMenuItem();
      comboProfiles = new ComboBox();
      txtBinPath = new TextBox();
      btnGerarRelatorio = new Button();
      tabControl = new TabControl();
      tabRelatorio = new TabPage();
      webViewRelatorio = new WebView2();
      tabLog = new TabPage();
      txtLog = new RichTextBox();
      menuStrip.SuspendLayout();
      tabControl.SuspendLayout();
      tabRelatorio.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)webViewRelatorio).BeginInit();
      tabLog.SuspendLayout();
      SuspendLayout();
      // 
      // menuStrip
      // 
      menuStrip.ImageScalingSize = new Size(20, 20);
      menuStrip.Items.AddRange(new ToolStripItem[] { menuNovoPerfil, menuEditarPerfil });
      menuStrip.Location = new Point(0, 0);
      menuStrip.Name = "menuStrip";
      menuStrip.Size = new Size(979, 28);
      menuStrip.TabIndex = 0;
      // 
      // menuNovoPerfil
      // 
      menuNovoPerfil.Name = "menuNovoPerfil";
      menuNovoPerfil.Size = new Size(168, 26);
      menuNovoPerfil.Text = "Novo Perfil";
      menuNovoPerfil.Click += menuNovoPerfil_Click;
      // 
      // menuEditarPerfil
      // 
      menuEditarPerfil.Name = "menuEditarPerfil";
      menuEditarPerfil.Size = new Size(168, 26);
      menuEditarPerfil.Text = "Editar Perfil";
      menuEditarPerfil.Click += menuEditarPerfil_Click;
      // 
      // comboProfiles
      // 
      comboProfiles.DropDownStyle = ComboBoxStyle.DropDownList;
      comboProfiles.FormattingEnabled = true;
      comboProfiles.Location = new Point(16, 36);
      comboProfiles.Name = "comboProfiles";
      comboProfiles.Size = new Size(284, 28);
      comboProfiles.TabIndex = 0;
      comboProfiles.SelectedIndexChanged += comboProfiles_SelectedIndexChanged;
      // 
      // txtBinPath
      // 
      txtBinPath.Location = new Point(303, 37);
      txtBinPath.Name = "txtBinPath";
      txtBinPath.ReadOnly = true;
      txtBinPath.Size = new Size(495, 27);
      txtBinPath.TabIndex = 2;
      // 
      // btnGerarRelatorio
      // 
      btnGerarRelatorio.Location = new Point(804, 37);
      btnGerarRelatorio.Name = "btnGerarRelatorio";
      btnGerarRelatorio.Size = new Size(140, 29);
      btnGerarRelatorio.TabIndex = 3;
      btnGerarRelatorio.Text = "Gerar Relatório";
      btnGerarRelatorio.Click += BtnGerarRelatorio_Click;
      // 
      // tabControl
      // 
      tabControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      tabControl.Controls.Add(tabRelatorio);
      tabControl.Controls.Add(tabLog);
      tabControl.Location = new Point(12, 70);
      tabControl.Name = "tabControl";
      tabControl.SelectedIndex = 0;
      tabControl.Size = new Size(939, 512);
      tabControl.TabIndex = 4;
      // 
      // tabRelatorio
      // 
      tabRelatorio.Controls.Add(webViewRelatorio);
      tabRelatorio.Location = new Point(4, 29);
      tabRelatorio.Name = "tabRelatorio";
      tabRelatorio.Size = new Size(931, 479);
      tabRelatorio.TabIndex = 0;
      tabRelatorio.Text = "Relatório";
      // 
      // webViewRelatorio
      // 
      webViewRelatorio.AllowExternalDrop = true;
      webViewRelatorio.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      webViewRelatorio.CreationProperties = null;
      webViewRelatorio.DefaultBackgroundColor = Color.White;
      webViewRelatorio.Location = new Point(3, 0);
      webViewRelatorio.Name = "webViewRelatorio";
      webViewRelatorio.Size = new Size(925, 479);
      webViewRelatorio.TabIndex = 0;
      webViewRelatorio.ZoomFactor = 1D;
      // 
      // tabLog
      // 
      tabLog.Controls.Add(txtLog);
      tabLog.Location = new Point(4, 29);
      tabLog.Name = "tabLog";
      tabLog.Size = new Size(892, 479);
      tabLog.TabIndex = 1;
      tabLog.Text = "Log";
      // 
      // txtLog
      // 
      txtLog.BackColor = Color.Black;
      txtLog.BorderStyle = BorderStyle.None;
      txtLog.Dock = DockStyle.Fill;
      txtLog.Font = new Font("Consolas", 10F);
      txtLog.ForeColor = Color.White;
      txtLog.Location = new Point(0, 0);
      txtLog.Name = "txtLog";
      txtLog.ReadOnly = true;
      txtLog.Size = new Size(892, 479);
      txtLog.TabIndex = 0;
      txtLog.Text = "";
      // 
      // MainForm
      // 
      ClientSize = new Size(979, 612);
      Controls.Add(menuStrip);
      Controls.Add(txtBinPath);
      Controls.Add(btnGerarRelatorio);
      Controls.Add(comboProfiles);
      Controls.Add(tabControl);
      Icon = (Icon)resources.GetObject("$this.Icon");
      MainMenuStrip = menuStrip;
      Name = "MainForm";
      StartPosition = FormStartPosition.CenterScreen;
      Text = "TestCoverageUI";
      WindowState = FormWindowState.Maximized;
      menuStrip.ResumeLayout(false);
      menuStrip.PerformLayout();
      tabControl.ResumeLayout(false);
      tabRelatorio.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)webViewRelatorio).EndInit();
      tabLog.ResumeLayout(false);
      ResumeLayout(false);
      PerformLayout();
    }
  }
}
