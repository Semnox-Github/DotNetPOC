namespace Semnox.Parafait.Customer
{
    partial class CreateRelatedCustomerForm
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
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnCustomerLookup = new System.Windows.Forms.Button();
            this.lblMessage = new System.Windows.Forms.Label();
            this.flpTitle = new System.Windows.Forms.FlowLayoutPanel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlTitle = new System.Windows.Forms.Panel();
            this.cmbCustomerRelationshipType = new System.Windows.Forms.ComboBox();
            this.txtEffectiveDate = new Semnox.Parafait.User.CueTextBox();
            this.dtpEffectiveDate = new System.Windows.Forms.DateTimePicker();
            this.lblEffectiveDate = new System.Windows.Forms.Label();
            this.txtExpiryDate = new Semnox.Parafait.User.CueTextBox();
            this.dtpExpiryDate = new System.Windows.Forms.DateTimePicker();
            this.lblExpiryDate = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.flpButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.flpTitle.SuspendLayout();
            this.pnlTitle.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.flpButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.BackColor = System.Drawing.Color.Transparent;
            this.btnSave.BackgroundImage = global::Semnox.Parafait.Customer.Properties.Resources.customer_button_normal;
            this.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSave.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSave.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(159, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(150, 40);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.BackgroundImage = global::Semnox.Parafait.Customer.Properties.Resources.customer_button_normal;
            this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnClose.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(315, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(150, 40);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnCustomerLookup
            // 
            this.btnCustomerLookup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCustomerLookup.BackColor = System.Drawing.Color.Transparent;
            this.btnCustomerLookup.BackgroundImage = global::Semnox.Parafait.Customer.Properties.Resources.customer_button_normal;
            this.btnCustomerLookup.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCustomerLookup.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnCustomerLookup.FlatAppearance.BorderSize = 0;
            this.btnCustomerLookup.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCustomerLookup.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCustomerLookup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCustomerLookup.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCustomerLookup.ForeColor = System.Drawing.Color.White;
            this.btnCustomerLookup.Location = new System.Drawing.Point(3, 3);
            this.btnCustomerLookup.Name = "btnCustomerLookup";
            this.btnCustomerLookup.Size = new System.Drawing.Size(150, 40);
            this.btnCustomerLookup.TabIndex = 6;
            this.btnCustomerLookup.Text = "Lookup";
            this.btnCustomerLookup.UseVisualStyleBackColor = false;
            this.btnCustomerLookup.Click += new System.EventHandler(this.btnCustomerLookup_Click);
            // 
            // lblMessage
            // 
            this.lblMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblMessage.AutoSize = true;
            this.lblMessage.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage.ForeColor = System.Drawing.Color.White;
            this.lblMessage.Location = new System.Drawing.Point(12, 623);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(148, 22);
            this.lblMessage.TabIndex = 7;
            this.lblMessage.Text = "Error message";
            // 
            // flpTitle
            // 
            this.flpTitle.Controls.Add(this.lblTitle);
            this.flpTitle.Controls.Add(this.pnlTitle);
            this.flpTitle.Location = new System.Drawing.Point(10, 20);
            this.flpTitle.Margin = new System.Windows.Forms.Padding(0);
            this.flpTitle.Name = "flpTitle";
            this.flpTitle.Size = new System.Drawing.Size(215, 35);
            this.flpTitle.TabIndex = 8;
            // 
            // lblTitle
            // 
            this.lblTitle.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblTitle.Location = new System.Drawing.Point(2, 2);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(2);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(85, 32);
            this.lblTitle.TabIndex = 1001;
            this.lblTitle.Tag = "";
            this.lblTitle.Text = "Relationship:";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pnlTitle
            // 
            this.pnlTitle.BackColor = System.Drawing.Color.Transparent;
            this.pnlTitle.Controls.Add(this.cmbCustomerRelationshipType);
            this.pnlTitle.Location = new System.Drawing.Point(91, 0);
            this.pnlTitle.Margin = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.pnlTitle.Name = "pnlTitle";
            this.pnlTitle.Size = new System.Drawing.Size(120, 35);
            this.pnlTitle.TabIndex = 64;
            // 
            // cmbCustomerRelationshipType
            // 
            this.cmbCustomerRelationshipType.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cmbCustomerRelationshipType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCustomerRelationshipType.Font = new System.Drawing.Font("Arial", 15F);
            this.cmbCustomerRelationshipType.FormattingEnabled = true;
            this.cmbCustomerRelationshipType.Location = new System.Drawing.Point(1, 2);
            this.cmbCustomerRelationshipType.Margin = new System.Windows.Forms.Padding(2);
            this.cmbCustomerRelationshipType.Name = "cmbCustomerRelationshipType";
            this.cmbCustomerRelationshipType.Size = new System.Drawing.Size(118, 31);
            this.cmbCustomerRelationshipType.TabIndex = 4;
            this.cmbCustomerRelationshipType.Tag = "Title";
            // 
            // txtEffectiveDate
            // 
            this.txtEffectiveDate.Cue = null;
            this.txtEffectiveDate.Font = new System.Drawing.Font("Arial", 15F);
            this.txtEffectiveDate.Location = new System.Drawing.Point(331, 24);
            this.txtEffectiveDate.Name = "txtEffectiveDate";
            this.txtEffectiveDate.Size = new System.Drawing.Size(183, 30);
            this.txtEffectiveDate.TabIndex = 46;
            // 
            // dtpEffectiveDate
            // 
            this.dtpEffectiveDate.Font = new System.Drawing.Font("Arial", 15F);
            this.dtpEffectiveDate.Location = new System.Drawing.Point(514, 24);
            this.dtpEffectiveDate.Name = "dtpEffectiveDate";
            this.dtpEffectiveDate.Size = new System.Drawing.Size(19, 30);
            this.dtpEffectiveDate.TabIndex = 47;
            this.dtpEffectiveDate.ValueChanged += new System.EventHandler(this.dtpEffectiveDate_ValueChanged);
            // 
            // lblEffectiveDate
            // 
            this.lblEffectiveDate.AutoSize = true;
            this.lblEffectiveDate.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblEffectiveDate.Location = new System.Drawing.Point(237, 31);
            this.lblEffectiveDate.Name = "lblEffectiveDate";
            this.lblEffectiveDate.Size = new System.Drawing.Size(91, 15);
            this.lblEffectiveDate.TabIndex = 48;
            this.lblEffectiveDate.Text = "Effective Date :";
            this.lblEffectiveDate.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtExpiryDate
            // 
            this.txtExpiryDate.Cue = null;
            this.txtExpiryDate.Font = new System.Drawing.Font("Arial", 15F);
            this.txtExpiryDate.Location = new System.Drawing.Point(635, 24);
            this.txtExpiryDate.Name = "txtExpiryDate";
            this.txtExpiryDate.Size = new System.Drawing.Size(183, 30);
            this.txtExpiryDate.TabIndex = 49;
            // 
            // dtpExpiryDate
            // 
            this.dtpExpiryDate.Font = new System.Drawing.Font("Arial", 15F);
            this.dtpExpiryDate.Location = new System.Drawing.Point(818, 24);
            this.dtpExpiryDate.Name = "dtpExpiryDate";
            this.dtpExpiryDate.Size = new System.Drawing.Size(19, 30);
            this.dtpExpiryDate.TabIndex = 50;
            this.dtpExpiryDate.ValueChanged += new System.EventHandler(this.dtpExpiryDate_ValueChanged);
            // 
            // lblExpiryDate
            // 
            this.lblExpiryDate.AutoSize = true;
            this.lblExpiryDate.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblExpiryDate.Location = new System.Drawing.Point(555, 32);
            this.lblExpiryDate.Name = "lblExpiryDate";
            this.lblExpiryDate.Size = new System.Drawing.Size(77, 15);
            this.lblExpiryDate.TabIndex = 51;
            this.lblExpiryDate.Text = "Expiry Date :";
            this.lblExpiryDate.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.flpTitle);
            this.groupBox1.Controls.Add(this.txtExpiryDate);
            this.groupBox1.Controls.Add(this.lblEffectiveDate);
            this.groupBox1.Controls.Add(this.dtpExpiryDate);
            this.groupBox1.Controls.Add(this.dtpEffectiveDate);
            this.groupBox1.Controls.Add(this.lblExpiryDate);
            this.groupBox1.Controls.Add(this.txtEffectiveDate);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(934, 65);
            this.groupBox1.TabIndex = 52;
            this.groupBox1.TabStop = false;
            // 
            // flpButtons
            // 
            this.flpButtons.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flpButtons.AutoSize = true;
            this.flpButtons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpButtons.Controls.Add(this.btnCustomerLookup);
            this.flpButtons.Controls.Add(this.btnSave);
            this.flpButtons.Controls.Add(this.btnClose);
            this.flpButtons.Location = new System.Drawing.Point(10, 648);
            this.flpButtons.Name = "flpButtons";
            this.flpButtons.Size = new System.Drawing.Size(468, 46);
            this.flpButtons.TabIndex = 53;
            // 
            // CreateRelatedCustomerForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.SlateGray;
            this.ClientSize = new System.Drawing.Size(934, 701);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.flpButtons);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "CreateRelatedCustomerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Add Customer Relationship";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CustomerDetailForm_FormClosing);
            this.Load += new System.EventHandler(this.CustomerDetailForm_Load);
            this.Shown += new System.EventHandler(this.CreateRelatedCustomerForm_Shown);
            this.flpTitle.ResumeLayout(false);
            this.pnlTitle.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.flpButtons.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnCustomerLookup;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.FlowLayoutPanel flpTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlTitle;
        private System.Windows.Forms.ComboBox cmbCustomerRelationshipType;
        private Semnox.Parafait.User.CueTextBox txtEffectiveDate;
        private System.Windows.Forms.DateTimePicker dtpEffectiveDate;
        private System.Windows.Forms.Label lblEffectiveDate;
        private Semnox.Parafait.User.CueTextBox txtExpiryDate;
        private System.Windows.Forms.DateTimePicker dtpExpiryDate;
        private System.Windows.Forms.Label lblExpiryDate;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.FlowLayoutPanel flpButtons;
    }
}