namespace PackageEditor
{
    partial class AutoUpdateForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AutoUpdateForm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.bkPanel = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.literalFinish = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.tabWizard = new WizardPages();
            this.tabEnableDisable = new System.Windows.Forms.TabPage();
            this.radioDisableFeature = new System.Windows.Forms.RadioButton();
            this.radioEnableFeature = new System.Windows.Forms.RadioButton();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tabGenerateXml = new System.Windows.Forms.TabPage();
            this.label7 = new System.Windows.Forms.Label();
            this.tbVersion = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.btnGenerateXml = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tabLocation = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbLocation = new System.Windows.Forms.TextBox();
            this.tabTest = new System.Windows.Forms.TabPage();
            this.label8 = new System.Windows.Forms.Label();
            this.tbVersionTest = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.btnTest = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.bkPanel.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabWizard.SuspendLayout();
            this.tabEnableDisable.SuspendLayout();
            this.tabGenerateXml.SuspendLayout();
            this.tabLocation.SuspendLayout();
            this.tabTest.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.bkPanel);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // bkPanel
            // 
            resources.ApplyResources(this.bkPanel, "bkPanel");
            this.bkPanel.Controls.Add(this.panel2);
            this.bkPanel.Name = "bkPanel";
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.literalFinish);
            this.panel2.Controls.Add(this.pictureBox1);
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Controls.Add(this.btnBack);
            this.panel2.Controls.Add(this.btnNext);
            this.panel2.Controls.Add(this.tabWizard);
            this.panel2.Name = "panel2";
            // 
            // literalFinish
            // 
            resources.ApplyResources(this.literalFinish, "literalFinish");
            this.literalFinish.Name = "literalFinish";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.pictureBox1, "pictureBox1");
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.TabStop = false;
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnBack
            // 
            resources.ApplyResources(this.btnBack, "btnBack");
            this.btnBack.Name = "btnBack";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // btnNext
            // 
            resources.ApplyResources(this.btnNext, "btnNext");
            this.btnNext.Name = "btnNext";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // tabWizard
            // 
            this.tabWizard.Controls.Add(this.tabEnableDisable);
            this.tabWizard.Controls.Add(this.tabGenerateXml);
            this.tabWizard.Controls.Add(this.tabLocation);
            this.tabWizard.Controls.Add(this.tabTest);
            resources.ApplyResources(this.tabWizard, "tabWizard");
            this.tabWizard.Name = "tabWizard";
            this.tabWizard.SelectedIndex = 0;
            // 
            // tabEnableDisable
            // 
            this.tabEnableDisable.BackColor = System.Drawing.SystemColors.Control;
            this.tabEnableDisable.Controls.Add(this.radioDisableFeature);
            this.tabEnableDisable.Controls.Add(this.radioEnableFeature);
            this.tabEnableDisable.Controls.Add(this.label6);
            this.tabEnableDisable.Controls.Add(this.groupBox3);
            this.tabEnableDisable.Controls.Add(this.label3);
            resources.ApplyResources(this.tabEnableDisable, "tabEnableDisable");
            this.tabEnableDisable.Name = "tabEnableDisable";
            // 
            // radioDisableFeature
            // 
            resources.ApplyResources(this.radioDisableFeature, "radioDisableFeature");
            this.radioDisableFeature.Name = "radioDisableFeature";
            this.radioDisableFeature.UseVisualStyleBackColor = true;
            // 
            // radioEnableFeature
            // 
            resources.ApplyResources(this.radioEnableFeature, "radioEnableFeature");
            this.radioEnableFeature.Checked = true;
            this.radioEnableFeature.Name = "radioEnableFeature";
            this.radioEnableFeature.TabStop = true;
            this.radioEnableFeature.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // groupBox3
            // 
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // tabGenerateXml
            // 
            this.tabGenerateXml.BackColor = System.Drawing.SystemColors.Control;
            this.tabGenerateXml.Controls.Add(this.label7);
            this.tabGenerateXml.Controls.Add(this.tbVersion);
            this.tabGenerateXml.Controls.Add(this.textBox3);
            this.tabGenerateXml.Controls.Add(this.btnGenerateXml);
            this.tabGenerateXml.Controls.Add(this.groupBox4);
            this.tabGenerateXml.Controls.Add(this.label4);
            resources.ApplyResources(this.tabGenerateXml, "tabGenerateXml");
            this.tabGenerateXml.Name = "tabGenerateXml";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // tbVersion
            // 
            resources.ApplyResources(this.tbVersion, "tbVersion");
            this.tbVersion.Name = "tbVersion";
            // 
            // textBox3
            // 
            this.textBox3.BackColor = System.Drawing.SystemColors.Control;
            this.textBox3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.textBox3, "textBox3");
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            // 
            // btnGenerateXml
            // 
            resources.ApplyResources(this.btnGenerateXml, "btnGenerateXml");
            this.btnGenerateXml.Name = "btnGenerateXml";
            this.btnGenerateXml.UseVisualStyleBackColor = true;
            this.btnGenerateXml.Click += new System.EventHandler(this.btnGenerateXml_Click);
            // 
            // groupBox4
            // 
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // tabLocation
            // 
            this.tabLocation.BackColor = System.Drawing.SystemColors.Control;
            this.tabLocation.Controls.Add(this.groupBox2);
            this.tabLocation.Controls.Add(this.label2);
            this.tabLocation.Controls.Add(this.textBox2);
            this.tabLocation.Controls.Add(this.label1);
            this.tabLocation.Controls.Add(this.tbLocation);
            resources.ApplyResources(this.tabLocation, "tabLocation");
            this.tabLocation.Name = "tabLocation";
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // textBox2
            // 
            resources.ApplyResources(this.textBox2, "textBox2");
            this.textBox2.BackColor = System.Drawing.SystemColors.Control;
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // tbLocation
            // 
            resources.ApplyResources(this.tbLocation, "tbLocation");
            this.tbLocation.Name = "tbLocation";
            // 
            // tabTest
            // 
            this.tabTest.BackColor = System.Drawing.SystemColors.Control;
            this.tabTest.Controls.Add(this.label8);
            this.tabTest.Controls.Add(this.tbVersionTest);
            this.tabTest.Controls.Add(this.textBox4);
            this.tabTest.Controls.Add(this.btnTest);
            this.tabTest.Controls.Add(this.groupBox5);
            this.tabTest.Controls.Add(this.label5);
            resources.ApplyResources(this.tabTest, "tabTest");
            this.tabTest.Name = "tabTest";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // tbVersionTest
            // 
            resources.ApplyResources(this.tbVersionTest, "tbVersionTest");
            this.tbVersionTest.Name = "tbVersionTest";
            // 
            // textBox4
            // 
            this.textBox4.BackColor = System.Drawing.SystemColors.Control;
            this.textBox4.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.textBox4, "textBox4");
            this.textBox4.Name = "textBox4";
            this.textBox4.ReadOnly = true;
            // 
            // btnTest
            // 
            resources.ApplyResources(this.btnTest, "btnTest");
            this.btnTest.Name = "btnTest";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // groupBox5
            // 
            resources.ApplyResources(this.groupBox5, "groupBox5");
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.TabStop = false;
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // AutoUpdateForm
            // 
            this.AcceptButton = this.btnNext;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AutoUpdateForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.panel1.ResumeLayout(false);
            this.bkPanel.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabWizard.ResumeLayout(false);
            this.tabEnableDisable.ResumeLayout(false);
            this.tabEnableDisable.PerformLayout();
            this.tabGenerateXml.ResumeLayout(false);
            this.tabGenerateXml.PerformLayout();
            this.tabLocation.ResumeLayout(false);
            this.tabLocation.PerformLayout();
            this.tabTest.ResumeLayout(false);
            this.tabTest.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel bkPanel;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Button btnNext;
        private WizardPages tabWizard;
        private System.Windows.Forms.TabPage tabEnableDisable;
        private System.Windows.Forms.TabPage tabGenerateXml;
        private System.Windows.Forms.TabPage tabLocation;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbLocation;
        private System.Windows.Forms.TabPage tabTest;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.RadioButton radioDisableFeature;
        private System.Windows.Forms.RadioButton radioEnableFeature;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Button btnGenerateXml;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Label literalFinish;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tbVersion;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tbVersionTest;
    }
}