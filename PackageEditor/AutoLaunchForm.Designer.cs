namespace PackageEditor
{
    partial class AutoLaunchForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AutoLaunchForm));
            this.bottomPanel = new System.Windows.Forms.Panel();
            this.panel12 = new System.Windows.Forms.Panel();
            this.panel11 = new System.Windows.Forms.Panel();
            this.bkPanel = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.propertyCmdRadio = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnDown = new System.Windows.Forms.Button();
            this.btnUp = new System.Windows.Forms.Button();
            this.btnModify = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.propertyMenuLV = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnVirtFilesBrowse = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.propertyCmdArgs = new System.Windows.Forms.TextBox();
            this.propertyCmdText = new System.Windows.Forms.ComboBox();
            this.propertyMenuRadio = new System.Windows.Forms.RadioButton();
            this.bottomPanel.SuspendLayout();
            this.bkPanel.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
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
            this.bkPanel.Controls.Add(this.panel1);
            this.bkPanel.Name = "bkPanel";
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.propertyCmdRadio);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.btnVirtFilesBrowse);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.btnOk);
            this.panel1.Controls.Add(this.propertyCmdArgs);
            this.panel1.Controls.Add(this.propertyCmdText);
            this.panel1.Controls.Add(this.propertyMenuRadio);
            this.panel1.Name = "panel1";
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // propertyCmdRadio
            // 
            resources.ApplyResources(this.propertyCmdRadio, "propertyCmdRadio");
            this.propertyCmdRadio.Name = "propertyCmdRadio";
            this.propertyCmdRadio.TabStop = true;
            this.propertyCmdRadio.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.btnDown);
            this.groupBox2.Controls.Add(this.btnUp);
            this.groupBox2.Controls.Add(this.btnModify);
            this.groupBox2.Controls.Add(this.btnRemove);
            this.groupBox2.Controls.Add(this.btnAdd);
            this.groupBox2.Controls.Add(this.propertyMenuLV);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // btnDown
            // 
            resources.ApplyResources(this.btnDown, "btnDown");
            this.btnDown.Image = global::PackageEditor.Properties.Resources.down;
            this.btnDown.Name = "btnDown";
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // btnUp
            // 
            resources.ApplyResources(this.btnUp, "btnUp");
            this.btnUp.Image = global::PackageEditor.Properties.Resources.up;
            this.btnUp.Name = "btnUp";
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // btnModify
            // 
            resources.ApplyResources(this.btnModify, "btnModify");
            this.btnModify.Name = "btnModify";
            this.btnModify.UseVisualStyleBackColor = true;
            this.btnModify.Click += new System.EventHandler(this.btnModify_Click);
            // 
            // btnRemove
            // 
            resources.ApplyResources(this.btnRemove, "btnRemove");
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnAdd
            // 
            resources.ApplyResources(this.btnAdd, "btnAdd");
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // propertyMenuLV
            // 
            resources.ApplyResources(this.propertyMenuLV, "propertyMenuLV");
            this.propertyMenuLV.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.propertyMenuLV.FullRowSelect = true;
            this.propertyMenuLV.HideSelection = false;
            this.propertyMenuLV.MultiSelect = false;
            this.propertyMenuLV.Name = "propertyMenuLV";
            this.propertyMenuLV.UseCompatibleStateImageBehavior = false;
            this.propertyMenuLV.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            resources.ApplyResources(this.columnHeader1, "columnHeader1");
            // 
            // columnHeader2
            // 
            resources.ApplyResources(this.columnHeader2, "columnHeader2");
            // 
            // columnHeader3
            // 
            resources.ApplyResources(this.columnHeader3, "columnHeader3");
            // 
            // columnHeader4
            // 
            resources.ApplyResources(this.columnHeader4, "columnHeader4");
            // 
            // btnVirtFilesBrowse
            // 
            this.btnVirtFilesBrowse.Image = global::PackageEditor.Properties.Resources.folder_closed_16_h;
            resources.ApplyResources(this.btnVirtFilesBrowse, "btnVirtFilesBrowse");
            this.btnVirtFilesBrowse.Name = "btnVirtFilesBrowse";
            this.btnVirtFilesBrowse.UseVisualStyleBackColor = true;
            this.btnVirtFilesBrowse.Click += new System.EventHandler(this.btnVirtFilesBrowse_Click);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // btnOk
            // 
            resources.ApplyResources(this.btnOk, "btnOk");
            this.btnOk.Name = "btnOk";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // propertyCmdArgs
            // 
            resources.ApplyResources(this.propertyCmdArgs, "propertyCmdArgs");
            this.propertyCmdArgs.Name = "propertyCmdArgs";
            // 
            // propertyCmdText
            // 
            this.propertyCmdText.FormattingEnabled = true;
            resources.ApplyResources(this.propertyCmdText, "propertyCmdText");
            this.propertyCmdText.Name = "propertyCmdText";
            // 
            // propertyMenuRadio
            // 
            resources.ApplyResources(this.propertyMenuRadio, "propertyMenuRadio");
            this.propertyMenuRadio.Name = "propertyMenuRadio";
            this.propertyMenuRadio.TabStop = true;
            this.propertyMenuRadio.UseVisualStyleBackColor = true;
            // 
            // AutoLaunchForm
            // 
            this.AcceptButton = this.btnOk;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.Controls.Add(this.bottomPanel);
            this.Controls.Add(this.bkPanel);
            this.Name = "AutoLaunchForm";
            this.ShowIcon = false;
            this.Load += new System.EventHandler(this.AutoLaunchForm_Load);
            this.bottomPanel.ResumeLayout(false);
            this.bkPanel.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel bottomPanel;
        private System.Windows.Forms.Panel panel12;
        private System.Windows.Forms.Panel panel11;
        private System.Windows.Forms.Panel bkPanel;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.RadioButton propertyCmdRadio;
        private System.Windows.Forms.Button btnVirtFilesBrowse;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.ComboBox propertyCmdText;
        private System.Windows.Forms.RadioButton propertyMenuRadio;
        private System.Windows.Forms.TextBox propertyCmdArgs;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnModify;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.ListView propertyMenuLV;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.Button btnDown;
    }
}