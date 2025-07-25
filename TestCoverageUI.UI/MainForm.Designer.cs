using Microsoft.Web.WebView2.WinForms;

namespace TestCoverageUI.UI
{
  partial class MainForm
  {
    private System.ComponentModel.IContainer components = null;
    private MenuStrip menuStrip;
    private ToolStripMenuItem configuracoesMenuItem;
    private Label lblBinPath;
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
      configuracoesMenuItem = new ToolStripMenuItem();
      lblBinPath = new Label();
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
      menuStrip.Items.AddRange(new ToolStripItem[] { configuracoesMenuItem });
      menuStrip.Location = new Point(0, 0);
      menuStrip.Name = "menuStrip";
      menuStrip.Size = new Size(940, 28);
      menuStrip.TabIndex = 0;
      // 
      // configuracoesMenuItem
      // 
      configuracoesMenuItem.Name = "configuracoesMenuItem";
      configuracoesMenuItem.Size = new Size(118, 24);
      configuracoesMenuItem.Text = "Configurações";
      configuracoesMenuItem.Click += ConfiguracoesMenuItem_Click;
      // 
      // lblBinPath
      // 
      lblBinPath.AutoSize = true;
      lblBinPath.Location = new Point(12, 40);
      lblBinPath.Name = "lblBinPath";
      lblBinPath.Size = new Size(124, 20);
      lblBinPath.TabIndex = 1;
      lblBinPath.Text = "Pasta de Binários:";
      // 
      // txtBinPath
      // 
      txtBinPath.Location = new Point(134, 37);
      txtBinPath.Name = "txtBinPath";
      txtBinPath.ReadOnly = true;
      txtBinPath.Size = new Size(628, 27);
      txtBinPath.TabIndex = 2;
      // 
      // btnGerarRelatorio
      // 
      btnGerarRelatorio.Location = new Point(768, 37);
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
      tabControl.Size = new Size(900, 512);
      tabControl.TabIndex = 4;
      // 
      // tabRelatorio
      // 
      tabRelatorio.Controls.Add(webViewRelatorio);
      tabRelatorio.Location = new Point(4, 29);
      tabRelatorio.Name = "tabRelatorio";
      tabRelatorio.Size = new Size(892, 479);
      tabRelatorio.TabIndex = 0;
      tabRelatorio.Text = "Relatório";
      // 
      // webViewRelatorio
      // 
      webViewRelatorio.AllowExternalDrop = true;
      webViewRelatorio.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      webViewRelatorio.CreationProperties = null;
      webViewRelatorio.DefaultBackgroundColor = Color.White;
      webViewRelatorio.Location = new Point(0, 0);
      webViewRelatorio.Name = "webViewRelatorio";
      webViewRelatorio.Size = new Size(892, 479);
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
      ClientSize = new Size(940, 612);
      Controls.Add(menuStrip);
      Controls.Add(lblBinPath);
      Controls.Add(txtBinPath);
      Controls.Add(btnGerarRelatorio);
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
