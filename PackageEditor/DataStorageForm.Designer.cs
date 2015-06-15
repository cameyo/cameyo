namespace PackageEditor
{
    partial class DataStorageForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DataStorageForm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.propertyDataDir = new System.Windows.Forms.CheckBox();
            this.propertyDataDirName = new System.Windows.Forms.TextBox();
            this.propertyLocalStorageExeDir = new System.Windows.Forms.RadioButton();
            this.propertyLocalStorageCustomDir = new System.Windows.Forms.TextBox();
            this.propertyLocalStorageCustom = new System.Windows.Forms.RadioButton();
            this.propertyLocalStorageDefault = new System.Windows.Forms.RadioButton();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel2);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnOk);
            this.panel2.Controls.Add(this.btnCancel);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // btnOk
            // 
            resources.ApplyResources(this.btnOk, "btnOk");
            this.btnOk.Name = "btnOk";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.groupBox1);
            resources.ApplyResources(this.panel3, "panel3");
            this.panel3.Name = "panel3";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.propertyDataDir);
            this.groupBox1.Controls.Add(this.propertyDataDirName);
            this.groupBox1.Controls.Add(this.propertyLocalStorageExeDir);
            this.groupBox1.Controls.Add(this.propertyLocalStorageCustomDir);
            this.groupBox1.Controls.Add(this.propertyLocalStorageCustom);
            this.groupBox1.Controls.Add(this.propertyLocalStorageDefault);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // propertyDataDir
            // 
            resources.ApplyResources(this.propertyDataDir, "propertyDataDir");
            this.propertyDataDir.Name = "propertyDataDir";
            this.propertyDataDir.UseVisualStyleBackColor = true;
            this.propertyDataDir.CheckedChanged += new System.EventHandler(this.propertyDataDir_CheckedChanged);
            // 
            // propertyDataDirName
            // 
            resources.ApplyResources(this.propertyDataDirName, "propertyDataDirName");
            this.propertyDataDirName.Name = "propertyDataDirName";
            // 
            // propertyLocalStorageExeDir
            // 
            resources.ApplyResources(this.propertyLocalStorageExeDir, "propertyLocalStorageExeDir");
            this.propertyLocalStorageExeDir.Name = "propertyLocalStorageExeDir";
            this.propertyLocalStorageExeDir.TabStop = true;
            this.propertyLocalStorageExeDir.UseVisualStyleBackColor = true;
            // 
            // propertyLocalStorageCustomDir
            // 
            resources.ApplyResources(this.propertyLocalStorageCustomDir, "propertyLocalStorageCustomDir");
            this.propertyLocalStorageCustomDir.Name = "propertyLocalStorageCustomDir";
            // 
            // propertyLocalStorageCustom
            // 
            resources.ApplyResources(this.propertyLocalStorageCustom, "propertyLocalStorageCustom");
            this.propertyLocalStorageCustom.Name = "propertyLocalStorageCustom";
            this.propertyLocalStorageCustom.TabStop = true;
            this.propertyLocalStorageCustom.UseVisualStyleBackColor = true;
            this.propertyLocalStorageCustom.CheckedChanged += new System.EventHandler(this.propertyLocalStorageCustom_CheckedChanged);
            // 
            // propertyLocalStorageDefault
            // 
            resources.ApplyResources(this.propertyLocalStorageDefault, "propertyLocalStorageDefault");
            this.propertyLocalStorageDefault.Name = "propertyLocalStorageDefault";
            this.propertyLocalStorageDefault.TabStop = true;
            this.propertyLocalStorageDefault.UseVisualStyleBackColor = true;
            // 
            // DataStorageForm
            // 
            this.AcceptButton = this.btnOk;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Name = "DataStorageForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Load += new System.EventHandler(this.DataStorageForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox propertyLocalStorageCustomDir;
        private System.Windows.Forms.RadioButton propertyLocalStorageCustom;
        private System.Windows.Forms.RadioButton propertyLocalStorageDefault;
        private System.Windows.Forms.RadioButton propertyLocalStorageExeDir;
        private System.Windows.Forms.CheckBox propertyDataDir;
        private System.Windows.Forms.TextBox propertyDataDirName;
    }
}