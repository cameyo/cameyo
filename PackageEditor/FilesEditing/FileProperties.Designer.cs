namespace PackageEditor.FilesEditing
{
  partial class FileProperties
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FileProperties));
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.tbFullPath = new System.Windows.Forms.TextBox();
            this.chkFileFlagDEPLOYED = new System.Windows.Forms.CheckBox();
            this.chkFileFlagDELETED = new System.Windows.Forms.CheckBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.chkFileFlagISFILE = new System.Windows.Forms.CheckBox();
            this.chkFileFlagPKG_FILE = new System.Windows.Forms.CheckBox();
            this.chkFileFlagDISCONNECTED = new System.Windows.Forms.CheckBox();
            this.chkFileFlagDEPLOYED_RAM = new System.Windows.Forms.CheckBox();
            this.groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox
            // 
            this.groupBox.Controls.Add(this.chkFileFlagDEPLOYED_RAM);
            this.groupBox.Controls.Add(this.tbFullPath);
            this.groupBox.Controls.Add(this.chkFileFlagDEPLOYED);
            this.groupBox.Controls.Add(this.chkFileFlagDELETED);
            resources.ApplyResources(this.groupBox, "groupBox");
            this.groupBox.Name = "groupBox";
            this.groupBox.TabStop = false;
            // 
            // tbFullPath
            // 
            resources.ApplyResources(this.tbFullPath, "tbFullPath");
            this.tbFullPath.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbFullPath.Name = "tbFullPath";
            this.tbFullPath.ReadOnly = true;
            // 
            // chkFileFlagDEPLOYED
            // 
            resources.ApplyResources(this.chkFileFlagDEPLOYED, "chkFileFlagDEPLOYED");
            this.chkFileFlagDEPLOYED.Name = "chkFileFlagDEPLOYED";
            this.chkFileFlagDEPLOYED.UseVisualStyleBackColor = true;
            // 
            // chkFileFlagDELETED
            // 
            resources.ApplyResources(this.chkFileFlagDELETED, "chkFileFlagDELETED");
            this.chkFileFlagDELETED.Name = "chkFileFlagDELETED";
            this.chkFileFlagDELETED.UseVisualStyleBackColor = true;
            // 
            // buttonOK
            // 
            resources.ApplyResources(this.buttonOK, "buttonOK");
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            resources.ApplyResources(this.buttonCancel, "buttonCancel");
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // chkFileFlagISFILE
            // 
            resources.ApplyResources(this.chkFileFlagISFILE, "chkFileFlagISFILE");
            this.chkFileFlagISFILE.Name = "chkFileFlagISFILE";
            this.chkFileFlagISFILE.UseVisualStyleBackColor = true;
            // 
            // chkFileFlagPKG_FILE
            // 
            resources.ApplyResources(this.chkFileFlagPKG_FILE, "chkFileFlagPKG_FILE");
            this.chkFileFlagPKG_FILE.Name = "chkFileFlagPKG_FILE";
            this.chkFileFlagPKG_FILE.UseVisualStyleBackColor = true;
            // 
            // chkFileFlagDISCONNECTED
            // 
            resources.ApplyResources(this.chkFileFlagDISCONNECTED, "chkFileFlagDISCONNECTED");
            this.chkFileFlagDISCONNECTED.Name = "chkFileFlagDISCONNECTED";
            this.chkFileFlagDISCONNECTED.UseVisualStyleBackColor = true;
            // 
            // chkFileFlagDEPLOYED_RAM
            // 
            resources.ApplyResources(this.chkFileFlagDEPLOYED_RAM, "chkFileFlagDEPLOYED_RAM");
            this.chkFileFlagDEPLOYED_RAM.Name = "chkFileFlagDEPLOYED_RAM";
            this.chkFileFlagDEPLOYED_RAM.UseVisualStyleBackColor = true;
            // 
            // FileProperties
            // 
            this.AcceptButton = this.buttonOK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.Controls.Add(this.chkFileFlagISFILE);
            this.Controls.Add(this.chkFileFlagPKG_FILE);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.chkFileFlagDISCONNECTED);
            this.Controls.Add(this.groupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "FileProperties";
            this.groupBox.ResumeLayout(false);
            this.groupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.GroupBox groupBox;
    private System.Windows.Forms.Button buttonOK;
    private System.Windows.Forms.Button buttonCancel;
    private System.Windows.Forms.CheckBox chkFileFlagISFILE;
    private System.Windows.Forms.CheckBox chkFileFlagPKG_FILE;
    private System.Windows.Forms.CheckBox chkFileFlagDELETED;
    private System.Windows.Forms.CheckBox chkFileFlagDISCONNECTED;
    private System.Windows.Forms.CheckBox chkFileFlagDEPLOYED;
    private System.Windows.Forms.TextBox tbFullPath;
    private System.Windows.Forms.CheckBox chkFileFlagDEPLOYED_RAM;
  }
}