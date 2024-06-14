namespace Semnox.Parafait.Device.PaymentGateway
{
    partial class PaymentModeChannelUI
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
            this.components = new System.ComponentModel.Container();
            this.lookupValuesDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.dgLookupValues = new System.Windows.Forms.DataGridView();
            this.chkPaymentEnable = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.LookupValueId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lookupIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lookupNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lookupValueDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.lookupValuesDTOBindingSource)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgLookupValues)).BeginInit();
            this.SuspendLayout();
            // 
            // lookupValuesDTOBindingSource
            // 
            this.lookupValuesDTOBindingSource.DataSource = typeof(Semnox.Core.Utilities.LookupValuesDTO);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.btnClose);
            this.groupBox1.Controls.Add(this.btnRefresh);
            this.groupBox1.Controls.Add(this.btnSave);
            this.groupBox1.Controls.Add(this.dgLookupValues);
            this.groupBox1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(722, 318);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Select Payment Channel";
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.Location = new System.Drawing.Point(238, 280);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(90, 23);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRefresh.Location = new System.Drawing.Point(127, 280);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(90, 23);
            this.btnRefresh.TabIndex = 2;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.Location = new System.Drawing.Point(16, 280);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(90, 23);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // dgLookupValues
            // 
            this.dgLookupValues.AllowUserToAddRows = false;
            this.dgLookupValues.AllowUserToDeleteRows = false;
            this.dgLookupValues.AllowUserToOrderColumns = true;
            this.dgLookupValues.AllowUserToResizeColumns = false;
            this.dgLookupValues.AllowUserToResizeRows = false;
            this.dgLookupValues.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgLookupValues.AutoGenerateColumns = false;
            this.dgLookupValues.BackgroundColor = System.Drawing.SystemColors.InactiveCaption;
            this.dgLookupValues.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgLookupValues.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.chkPaymentEnable,
            this.LookupValueId,
            this.lookupIdDataGridViewTextBoxColumn,
            this.lookupNameDataGridViewTextBoxColumn,
            this.lookupValueDataGridViewTextBoxColumn,
            this.descriptionDataGridViewTextBoxColumn});
            this.dgLookupValues.DataSource = this.lookupValuesDTOBindingSource;
            this.dgLookupValues.Location = new System.Drawing.Point(16, 20);
            this.dgLookupValues.MultiSelect = false;
            this.dgLookupValues.Name = "dgLookupValues";
            this.dgLookupValues.Size = new System.Drawing.Size(689, 239);
            this.dgLookupValues.TabIndex = 0;
            this.dgLookupValues.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgLookupValues_CellContentClick);
            this.dgLookupValues.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgLookupValues_DataBindingComplete);
            // 
            // chkPaymentEnable
            // 
            this.chkPaymentEnable.HeaderText = "Enable";
            this.chkPaymentEnable.Name = "chkPaymentEnable";
            this.chkPaymentEnable.Width = 83;
            // 
            // LookupValueId
            // 
            this.LookupValueId.DataPropertyName = "LookupValueId";
            this.LookupValueId.HeaderText = "Lookup Value Id";
            this.LookupValueId.Name = "LookupValueId";
            this.LookupValueId.ReadOnly = true;
            this.LookupValueId.Visible = false;
            // 
            // lookupIdDataGridViewTextBoxColumn
            // 
            this.lookupIdDataGridViewTextBoxColumn.DataPropertyName = "LookupId";
            this.lookupIdDataGridViewTextBoxColumn.HeaderText = "Lookup Id";
            this.lookupIdDataGridViewTextBoxColumn.Name = "lookupIdDataGridViewTextBoxColumn";
            this.lookupIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // lookupNameDataGridViewTextBoxColumn
            // 
            this.lookupNameDataGridViewTextBoxColumn.DataPropertyName = "LookupName";
            this.lookupNameDataGridViewTextBoxColumn.DividerWidth = 100;
            this.lookupNameDataGridViewTextBoxColumn.HeaderText = "Payment Channel";
            this.lookupNameDataGridViewTextBoxColumn.Name = "lookupNameDataGridViewTextBoxColumn";
            this.lookupNameDataGridViewTextBoxColumn.ReadOnly = true;
            this.lookupNameDataGridViewTextBoxColumn.Visible = false;
            // 
            // lookupValueDataGridViewTextBoxColumn
            // 
            this.lookupValueDataGridViewTextBoxColumn.DataPropertyName = "LookupValue";
            this.lookupValueDataGridViewTextBoxColumn.HeaderText = "Channel";
            this.lookupValueDataGridViewTextBoxColumn.Name = "lookupValueDataGridViewTextBoxColumn";
            this.lookupValueDataGridViewTextBoxColumn.ReadOnly = true;
            this.lookupValueDataGridViewTextBoxColumn.Width = 200;
            // 
            // descriptionDataGridViewTextBoxColumn
            // 
            this.descriptionDataGridViewTextBoxColumn.DataPropertyName = "Description";
            this.descriptionDataGridViewTextBoxColumn.HeaderText = "Description";
            this.descriptionDataGridViewTextBoxColumn.MinimumWidth = 300;
            this.descriptionDataGridViewTextBoxColumn.Name = "descriptionDataGridViewTextBoxColumn";
            this.descriptionDataGridViewTextBoxColumn.ReadOnly = true;
            this.descriptionDataGridViewTextBoxColumn.Width = 300;
            // 
            // PaymentModeChannelUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(746, 342);
            this.Controls.Add(this.groupBox1);
            this.DoubleBuffered = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PaymentModeChannelUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Payment Mode Channel";
            ((System.ComponentModel.ISupportInitialize)(this.lookupValuesDTOBindingSource)).EndInit();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgLookupValues)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.BindingSource lookupValuesDTOBindingSource;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.DataGridView dgLookupValues;
        private System.Windows.Forms.DataGridViewCheckBoxColumn chkPaymentEnable;
        private System.Windows.Forms.DataGridViewTextBoxColumn LookupValueId;
        private System.Windows.Forms.DataGridViewTextBoxColumn lookupIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lookupNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lookupValueDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn;

    }
}

