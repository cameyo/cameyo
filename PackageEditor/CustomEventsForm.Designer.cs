namespace PackageEditor
{
    partial class CustomEventsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomEventsForm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.bottomPanel = new System.Windows.Forms.Panel();
            this.panel12 = new System.Windows.Forms.Panel();
            this.panel11 = new System.Windows.Forms.Panel();
            this.bkPanel = new System.Windows.Forms.Panel();
            this.panel8 = new System.Windows.Forms.Panel();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblTotalEvents = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnDown = new System.Windows.Forms.Button();
            this.listBox = new System.Windows.Forms.ListBox();
            this.comboBox = new System.Windows.Forms.ComboBox();
            this.btnUp = new System.Windows.Forms.Button();
            this.btnErase = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtCmd = new System.Windows.Forms.TextBox();
            this.boxWait = new System.Windows.Forms.CheckBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.panel4 = new System.Windows.Forms.Panel();
            this.btnAddSave = new System.Windows.Forms.Button();
            this.txtArgs = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.bottomPanel.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.bottomPanel);
            this.panel1.Controls.Add(this.bkPanel);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // bottomPanel
            // 
            this.bottomPanel.Controls.Add(this.panel12);
            this.bottomPanel.Controls.Add(this.panel11);
            resources.ApplyResources(this.bottomPanel, "bottomPanel");
            this.bottomPanel.Name = "bottomPanel";
            // 
            // panel12
            // 
            this.panel12.BackgroundImage = global::PackageEditor.Properties.Resources.PackedgeEditorBG_BottomClient;
            resources.ApplyResources(this.panel12, "panel12");
            this.panel12.Name = "panel12";
            // 
            // panel11
            // 
            this.panel11.BackgroundImage = global::PackageEditor.Properties.Resources.PackedgeEditorBG_BottomRight;
            resources.ApplyResources(this.panel11, "panel11");
            this.panel11.Name = "panel11";
            // 
            // bkPanel
            // 
            this.bkPanel.BackgroundImage = global::PackageEditor.Properties.Resources.PackedgeEditorBG_Client;
            resources.ApplyResources(this.bkPanel, "bkPanel");
            this.bkPanel.Name = "bkPanel";
            // 
            // panel8
            // 
            resources.ApplyResources(this.panel8, "panel8");
            this.panel8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
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
            this.panel2.Controls.Add(this.lblTotalEvents);
            this.panel2.Controls.Add(this.pictureBox1);
            this.panel2.Controls.Add(this.btnDown);
            this.panel2.Controls.Add(this.listBox);
            this.panel2.Controls.Add(this.comboBox);
            this.panel2.Controls.Add(this.btnUp);
            this.panel2.Controls.Add(this.btnErase);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Name = "panel2";
            // 
            // lblTotalEvents
            // 
            resources.ApplyResources(this.lblTotalEvents, "lblTotalEvents");
            this.lblTotalEvents.Name = "lblTotalEvents";
            // 
            // pictureBox1
            // 
            resources.ApplyResources(this.pictureBox1, "pictureBox1");
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.TabStop = false;
            // 
            // btnDown
            // 
            resources.ApplyResources(this.btnDown, "btnDown");
            this.btnDown.Image = global::PackageEditor.Properties.Resources.down;
            this.btnDown.Name = "btnDown";
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // listBox
            // 
            resources.ApplyResources(this.listBox, "listBox");
            this.listBox.FormattingEnabled = true;
            this.listBox.Name = "listBox";
            this.listBox.SelectedIndexChanged += new System.EventHandler(this.listBox_SelectedIndexChanged);
            // 
            // comboBox
            // 
            this.comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox.FormattingEnabled = true;
            this.comboBox.Items.AddRange(new object[] {
            resources.GetString("comboBox.Items"),
            resources.GetString("comboBox.Items1"),
            resources.GetString("comboBox.Items2"),
            resources.GetString("comboBox.Items3"),
            resources.GetString("comboBox.Items4"),
            resources.GetString("comboBox.Items5"),
            resources.GetString("comboBox.Items6"),
            resources.GetString("comboBox.Items7")});
            resources.ApplyResources(this.comboBox, "comboBox");
            this.comboBox.Name = "comboBox";
            this.comboBox.SelectedIndexChanged += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            // 
            // btnUp
            // 
            resources.ApplyResources(this.btnUp, "btnUp");
            this.btnUp.Image = global::PackageEditor.Properties.Resources.up;
            this.btnUp.Name = "btnUp";
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // btnErase
            // 
            resources.ApplyResources(this.btnErase, "btnErase");
            this.btnErase.Image = global::PackageEditor.Properties.Resources.delete_16_h;
            this.btnErase.Name = "btnErase";
            this.btnErase.UseVisualStyleBackColor = true;
            this.btnErase.Click += new System.EventHandler(this.btnErase_Click);
            // 
            // panel3
            // 
            resources.ApplyResources(this.panel3, "panel3");
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.groupBox1);
            this.panel3.Name = "panel3";
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.txtCmd);
            this.groupBox1.Controls.Add(this.boxWait);
            this.groupBox1.Controls.Add(this.btnBrowse);
            this.groupBox1.Controls.Add(this.panel4);
            this.groupBox1.Controls.Add(this.txtArgs);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // txtCmd
            // 
            resources.ApplyResources(this.txtCmd, "txtCmd");
            this.txtCmd.Name = "txtCmd";
            // 
            // boxWait
            // 
            resources.ApplyResources(this.boxWait, "boxWait");
            this.boxWait.Name = "boxWait";
            this.boxWait.UseVisualStyleBackColor = true;
            // 
            // btnBrowse
            // 
            resources.ApplyResources(this.btnBrowse, "btnBrowse");
            this.btnBrowse.Image = global::PackageEditor.Properties.Resources.folder_closed_16_h;
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            // 
            // panel4
            // 
            resources.ApplyResources(this.panel4, "panel4");
            this.panel4.Controls.Add(this.btnAddSave);
            this.panel4.Name = "panel4";
            // 
            // btnAddSave
            // 
            resources.ApplyResources(this.btnAddSave, "btnAddSave");
            this.btnAddSave.Name = "btnAddSave";
            this.btnAddSave.UseVisualStyleBackColor = true;
            this.btnAddSave.Click += new System.EventHandler(this.btnAddSave_Click);
            // 
            // txtArgs
            // 
            resources.ApplyResources(this.txtArgs, "txtArgs");
            this.txtArgs.Name = "txtArgs";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // CustomEventsForm
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
            this.Name = "CustomEventsForm";
            this.Load += new System.EventHandler(this.CustomEventsForm_Load);
            this.panel1.ResumeLayout(false);
            this.bottomPanel.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ComboBox comboBox;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.CheckBox boxWait;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button btnAddSave;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCmd;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtArgs;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.Button btnErase;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel bottomPanel;
        private System.Windows.Forms.Panel panel12;
        private System.Windows.Forms.Panel panel11;
        private System.Windows.Forms.Panel bkPanel;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ListBox listBox;
        private System.Windows.Forms.Label lblTotalEvents;
    }
}