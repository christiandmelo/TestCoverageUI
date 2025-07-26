namespace TestCoverageUI.UI
{
  partial class ConfigForm
  {
    private System.ComponentModel.IContainer components = null;
    private TextBox txtOpenCover;
    private TextBox txtReportGen;
    private TextBox txtVSTest;
    private TextBox txtBinPath;
    private TextBox txtPrefixDll;
    private TextBox txtSuffixDll;
    private TextBox txtProfileName;
    private CheckBox chkUseEmbedded;
    private Button btnSave;
    private Button btnCancel;
    private Button btnBrowseOpenCover;
    private Button btnBrowseReportGen;
    private Button btnBrowseVSTest;
    private Button btnBrowseBin;

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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigForm));
      txtOpenCover = new TextBox();
      txtReportGen = new TextBox();
      txtVSTest = new TextBox();
      txtBinPath = new TextBox();
      txtPrefixDll = new TextBox();
      txtSuffixDll = new TextBox();
      txtProfileName = new TextBox();
      chkUseEmbedded = new CheckBox();
      btnSave = new Button();
      btnCancel = new Button();
      btnBrowseOpenCover = new Button();
      btnBrowseReportGen = new Button();
      btnBrowseVSTest = new Button();
      btnBrowseBin = new Button();
      label1 = new Label();
      label2 = new Label();
      label3 = new Label();
      label4 = new Label();
      label5 = new Label();
      label6 = new Label();
      label7 = new Label();
      SuspendLayout();
      // 
      // txtOpenCover
      // 
      txtOpenCover.Location = new Point(12, 157);
      txtOpenCover.Name = "txtOpenCover";
      txtOpenCover.ReadOnly = true;
      txtOpenCover.Size = new Size(629, 27);
      txtOpenCover.TabIndex = 4;
      // 
      // txtReportGen
      // 
      txtReportGen.Location = new Point(12, 99);
      txtReportGen.Name = "txtReportGen";
      txtReportGen.ReadOnly = true;
      txtReportGen.Size = new Size(629, 27);
      txtReportGen.TabIndex = 2;
      // 
      // txtVSTest
      // 
      txtVSTest.Location = new Point(12, 215);
      txtVSTest.Name = "txtVSTest";
      txtVSTest.ReadOnly = true;
      txtVSTest.Size = new Size(629, 27);
      txtVSTest.TabIndex = 6;
      // 
      // txtBinPath
      // 
      txtBinPath.Location = new Point(12, 273);
      txtBinPath.Name = "txtBinPath";
      txtBinPath.ReadOnly = true;
      txtBinPath.Size = new Size(629, 27);
      txtBinPath.TabIndex = 8;
      // 
      // txtPrefixDll
      // 
      txtPrefixDll.Location = new Point(12, 331);
      txtPrefixDll.Name = "txtPrefixDll";
      txtPrefixDll.Size = new Size(364, 27);
      txtPrefixDll.TabIndex = 10;
      // 
      // txtSuffixDll
      // 
      txtSuffixDll.Location = new Point(382, 331);
      txtSuffixDll.Name = "txtSuffixDll";
      txtSuffixDll.Size = new Size(365, 27);
      txtSuffixDll.TabIndex = 11;
      // 
      // txtProfileName
      // 
      txtProfileName.Location = new Point(12, 41);
      txtProfileName.Name = "txtProfileName";
      txtProfileName.Size = new Size(450, 27);
      txtProfileName.TabIndex = 0;
      // 
      // chkUseEmbedded
      // 
      chkUseEmbedded.Checked = true;
      chkUseEmbedded.CheckState = CheckState.Checked;
      chkUseEmbedded.Location = new Point(489, 48);
      chkUseEmbedded.Name = "chkUseEmbedded";
      chkUseEmbedded.Size = new Size(258, 20);
      chkUseEmbedded.TabIndex = 12;
      chkUseEmbedded.Text = "Utilizar ferramentas embutidas";
      chkUseEmbedded.Click += chkUseEmbedded_Click;
      // 
      // btnSave
      // 
      btnSave.Location = new Point(647, 383);
      btnSave.Name = "btnSave";
      btnSave.Size = new Size(100, 30);
      btnSave.TabIndex = 14;
      btnSave.Text = "Salvar";
      btnSave.Click += BtnSave_Click;
      // 
      // btnCancel
      // 
      btnCancel.Location = new Point(541, 383);
      btnCancel.Name = "btnCancel";
      btnCancel.Size = new Size(100, 30);
      btnCancel.TabIndex = 13;
      btnCancel.Text = "Cancelar";
      btnCancel.Click += BtnCancel_Click;
      // 
      // btnBrowseOpenCover
      // 
      btnBrowseOpenCover.Enabled = false;
      btnBrowseOpenCover.Location = new Point(647, 157);
      btnBrowseOpenCover.Name = "btnBrowseOpenCover";
      btnBrowseOpenCover.Size = new Size(100, 27);
      btnBrowseOpenCover.TabIndex = 5;
      btnBrowseOpenCover.Text = "Selecionar";
      btnBrowseOpenCover.Click += BtnBrowseOpenCover_Click;
      // 
      // btnBrowseReportGen
      // 
      btnBrowseReportGen.Enabled = false;
      btnBrowseReportGen.Location = new Point(647, 99);
      btnBrowseReportGen.Name = "btnBrowseReportGen";
      btnBrowseReportGen.Size = new Size(100, 27);
      btnBrowseReportGen.TabIndex = 3;
      btnBrowseReportGen.Text = "Selecionar";
      btnBrowseReportGen.Click += BtnBrowseReportGen_Click;
      // 
      // btnBrowseVSTest
      // 
      btnBrowseVSTest.Enabled = false;
      btnBrowseVSTest.Location = new Point(647, 215);
      btnBrowseVSTest.Name = "btnBrowseVSTest";
      btnBrowseVSTest.Size = new Size(100, 27);
      btnBrowseVSTest.TabIndex = 7;
      btnBrowseVSTest.Text = "Selecionar";
      btnBrowseVSTest.Click += BtnBrowseVSTest_Click;
      // 
      // btnBrowseBin
      // 
      btnBrowseBin.Location = new Point(647, 273);
      btnBrowseBin.Name = "btnBrowseBin";
      btnBrowseBin.Size = new Size(100, 27);
      btnBrowseBin.TabIndex = 9;
      btnBrowseBin.Text = "Selecionar";
      btnBrowseBin.Click += BtnBrowseBin_Click;
      // 
      // label1
      // 
      label1.AutoSize = true;
      label1.Location = new Point(12, 76);
      label1.Name = "label1";
      label1.Size = new Size(201, 20);
      label1.TabIndex = 21;
      label1.Text = "Executável Report Generator:";
      // 
      // label2
      // 
      label2.AutoSize = true;
      label2.Location = new Point(12, 134);
      label2.Name = "label2";
      label2.Size = new Size(164, 20);
      label2.TabIndex = 22;
      label2.Text = "Executável Open Cover:";
      // 
      // label3
      // 
      label3.AutoSize = true;
      label3.Location = new Point(12, 192);
      label3.Name = "label3";
      label3.Size = new Size(129, 20);
      label3.TabIndex = 23;
      label3.Text = "Executável VSTest:";
      // 
      // label4
      // 
      label4.AutoSize = true;
      label4.Location = new Point(12, 250);
      label4.Name = "label4";
      label4.Size = new Size(215, 20);
      label4.TabIndex = 24;
      label4.Text = "Caminho Com as Dll's de Teste:";
      // 
      // label5
      // 
      label5.AutoSize = true;
      label5.Location = new Point(382, 308);
      label5.Name = "label5";
      label5.Size = new Size(169, 20);
      label5.TabIndex = 25;
      label5.Text = "Sufixo das Dll's de teste:";
      // 
      // label6
      // 
      label6.AutoSize = true;
      label6.Location = new Point(12, 308);
      label6.Name = "label6";
      label6.Size = new Size(176, 20);
      label6.TabIndex = 26;
      label6.Text = "Prefixo das Dll's de Teste:";
      // 
      // label7
      // 
      label7.AutoSize = true;
      label7.Location = new Point(12, 18);
      label7.Name = "label7";
      label7.Size = new Size(90, 20);
      label7.TabIndex = 27;
      label7.Text = "Nome Perfil:";
      // 
      // ConfigForm
      // 
      ClientSize = new Size(759, 430);
      Controls.Add(label7);
      Controls.Add(label6);
      Controls.Add(label5);
      Controls.Add(label4);
      Controls.Add(label3);
      Controls.Add(label2);
      Controls.Add(label1);
      Controls.Add(txtProfileName);
      Controls.Add(txtOpenCover);
      Controls.Add(btnBrowseOpenCover);
      Controls.Add(txtReportGen);
      Controls.Add(btnBrowseReportGen);
      Controls.Add(txtVSTest);
      Controls.Add(btnBrowseVSTest);
      Controls.Add(txtBinPath);
      Controls.Add(btnBrowseBin);
      Controls.Add(txtPrefixDll);
      Controls.Add(txtSuffixDll);
      Controls.Add(chkUseEmbedded);
      Controls.Add(btnSave);
      Controls.Add(btnCancel);
      Icon = (Icon)resources.GetObject("$this.Icon");
      Name = "ConfigForm";
      StartPosition = FormStartPosition.CenterParent;
      Text = "Configurações";
      ResumeLayout(false);
      PerformLayout();
    }

    private Label label1;
    private Label label2;
    private Label label3;
    private Label label4;
    private Label label5;
    private Label label6;
    private Label label7;
  }
}
