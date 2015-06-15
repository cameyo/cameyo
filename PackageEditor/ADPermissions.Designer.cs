namespace PackageEditor
{
    partial class ADPermissionsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ADPermissionsForm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel8 = new System.Windows.Forms.Panel();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.tbAuthDenyMsg = new System.Windows.Forms.TextBox();
            this.cbAuthDenyMsg = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblOfflineUsage = new System.Windows.Forms.Label();
            this.numOfflineUsage = new System.Windows.Forms.NumericUpDown();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbNestedCheck = new System.Windows.Forms.CheckBox();
            this.btnRemove = new System.Windows.Forms.Button();
            this.lblTotalEvents = new System.Windows.Forms.Label();
            this.btnAddSave = new System.Windows.Forms.Button();
            this.txtCmd = new System.Windows.Forms.TextBox();
            this.comboBox = new System.Windows.Forms.ComboBox();
            this.listBox = new System.Windows.Forms.ListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tbRequireDomainConnection = new System.Windows.Forms.TextBox();
            this.cbRequireDomainConnection = new System.Windows.Forms.CheckBox();
            this.panel8.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numOfflineUsage)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // panel8
            // 
            resources.ApplyResources(this.panel8, "panel8");
            this.panel8.Controls.Add(this.btnOk);
            this.panel8.Controls.Add(this.btnCancel);
            this.panel8.Controls.Add(this.panel2);
            this.panel8.Name = "panel8";
            // 
            // btnOk
            // 
            resources.ApplyResources(this.btnOk, "btnOk");
            this.btnOk.Name = "btnOk";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Controls.Add(this.groupBox4);
            this.panel2.Controls.Add(this.groupBox3);
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Controls.Add(this.groupBox2);
            this.panel2.Name = "panel2";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.tbAuthDenyMsg);
            this.groupBox4.Controls.Add(this.cbAuthDenyMsg);
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
            // 
            // tbAuthDenyMsg
            // 
            resources.ApplyResources(this.tbAuthDenyMsg, "tbAuthDenyMsg");
            this.tbAuthDenyMsg.Name = "tbAuthDenyMsg";
            // 
            // cbAuthDenyMsg
            // 
            resources.ApplyResources(this.cbAuthDenyMsg, "cbAuthDenyMsg");
            this.cbAuthDenyMsg.Name = "cbAuthDenyMsg";
            this.cbAuthDenyMsg.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.lblOfflineUsage);
            this.groupBox3.Controls.Add(this.numOfflineUsage);
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // lblOfflineUsage
            // 
            resources.ApplyResources(this.lblOfflineUsage, "lblOfflineUsage");
            this.lblOfflineUsage.Name = "lblOfflineUsage";
            // 
            // numOfflineUsage
            // 
            resources.ApplyResources(this.numOfflineUsage, "numOfflineUsage");
            this.numOfflineUsage.Name = "numOfflineUsage";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbNestedCheck);
            this.groupBox1.Controls.Add(this.btnRemove);
            this.groupBox1.Controls.Add(this.lblTotalEvents);
            this.groupBox1.Controls.Add(this.btnAddSave);
            this.groupBox1.Controls.Add(this.txtCmd);
            this.groupBox1.Controls.Add(this.comboBox);
            this.groupBox1.Controls.Add(this.listBox);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // cbNestedCheck
            // 
            resources.ApplyResources(this.cbNestedCheck, "cbNestedCheck");
            this.cbNestedCheck.Name = "cbNestedCheck";
            this.cbNestedCheck.UseVisualStyleBackColor = true;
            // 
            // btnRemove
            // 
            resources.ApplyResources(this.btnRemove, "btnRemove");
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // lblTotalEvents
            // 
            resources.ApplyResources(this.lblTotalEvents, "lblTotalEvents");
            this.lblTotalEvents.Name = "lblTotalEvents";
            // 
            // btnAddSave
            // 
            resources.ApplyResources(this.btnAddSave, "btnAddSave");
            this.btnAddSave.Name = "btnAddSave";
            this.btnAddSave.UseVisualStyleBackColor = true;
            this.btnAddSave.Click += new System.EventHandler(this.btnAddSave_Click);
            // 
            // txtCmd
            // 
            resources.ApplyResources(this.txtCmd, "txtCmd");
            this.txtCmd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCmd.Name = "txtCmd";
            // 
            // comboBox
            // 
            resources.ApplyResources(this.comboBox, "comboBox");
            this.comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox.FormattingEnabled = true;
            this.comboBox.Items.AddRange(new object[] {
            resources.GetString("comboBox.Items"),
            resources.GetString("comboBox.Items1")});
            this.comboBox.Name = "comboBox";
            this.comboBox.SelectedIndexChanged += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            // 
            // listBox
            // 
            resources.ApplyResources(this.listBox, "listBox");
            this.listBox.FormattingEnabled = true;
            this.listBox.Name = "listBox";
            this.listBox.SelectedIndexChanged += new System.EventHandler(this.listBox_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tbRequireDomainConnection);
            this.groupBox2.Controls.Add(this.cbRequireDomainConnection);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // tbRequireDomainConnection
            // 
            resources.ApplyResources(this.tbRequireDomainConnection, "tbRequireDomainConnection");
            this.tbRequireDomainConnection.Name = "tbRequireDomainConnection";
            // 
            // cbRequireDomainConnection
            // 
            resources.ApplyResources(this.cbRequireDomainConnection, "cbRequireDomainConnection");
            this.cbRequireDomainConnection.Name = "cbRequireDomainConnection";
            this.cbRequireDomainConnection.UseVisualStyleBackColor = true;
            this.cbRequireDomainConnection.CheckedChanged += new System.EventHandler(this.cbRequireDomainConnection_CheckedChanged);
            // 
            // ADPermissionsForm
            // 
            this.AcceptButton = this.btnOk;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.Controls.Add(this.panel8);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ADPermissionsForm";
            this.Load += new System.EventHandler(this.ADPermissionsForm_Load);
            this.panel8.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numOfflineUsage)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ComboBox comboBox;
        private System.Windows.Forms.Button btnAddSave;
        private System.Windows.Forms.TextBox txtCmd;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ListBox listBox;
        private System.Windows.Forms.Label lblTotalEvents;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox cbRequireDomainConnection;
        private System.Windows.Forms.TextBox tbRequireDomainConnection;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox tbAuthDenyMsg;
        private System.Windows.Forms.CheckBox cbAuthDenyMsg;
        private System.Windows.Forms.NumericUpDown numOfflineUsage;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblOfflineUsage;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.CheckBox cbNestedCheck;
        private System.Windows.Forms.GroupBox groupBox4;
    }
}