namespace Semnox.Parafait.Currency
{
    partial class frmCurrency
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCurrency));
            this.btnSave = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.dgvCurrency = new System.Windows.Forms.DataGridView();
            this.currencyDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.grpCurrency = new System.Windows.Forms.GroupBox();
            this.lblNote = new System.Windows.Forms.Label();
            this.currencyIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.currencyCodeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.currencySymbolDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buyRateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sellRateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.effectiveDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.createdDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastModifiedDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastModifiedByDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isActiveDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCurrency)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.currencyDTOBindingSource)).BeginInit();
            this.grpCurrency.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.Location = new System.Drawing.Point(15, 339);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRefresh.Location = new System.Drawing.Point(122, 339);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 11;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.Location = new System.Drawing.Point(234, 339);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 12;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // dgvCurrency
            // 
            this.dgvCurrency.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvCurrency.AutoGenerateColumns = false;
            this.dgvCurrency.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCurrency.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.currencyIdDataGridViewTextBoxColumn,
            this.currencyCodeDataGridViewTextBoxColumn,
            this.currencySymbolDataGridViewTextBoxColumn,
            this.descriptionDataGridViewTextBoxColumn,
            this.buyRateDataGridViewTextBoxColumn,
            this.sellRateDataGridViewTextBoxColumn,
            this.effectiveDateDataGridViewTextBoxColumn,
            this.createdDateDataGridViewTextBoxColumn,
            this.lastModifiedDateDataGridViewTextBoxColumn,
            this.lastModifiedByDataGridViewTextBoxColumn,
            this.isActiveDataGridViewCheckBoxColumn});
            this.dgvCurrency.DataSource = this.currencyDTOBindingSource;
            this.dgvCurrency.Location = new System.Drawing.Point(19, 46);
            this.dgvCurrency.Name = "dgvCurrency";
            this.dgvCurrency.Size = new System.Drawing.Size(794, 201);
            this.dgvCurrency.TabIndex = 13;
            this.dgvCurrency.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dgvCurrency_CellValidating);
            // 
            // currencyDTOBindingSource
            // 
            this.currencyDTOBindingSource.DataSource = typeof(Semnox.Parafait.Currency.CurrencyDTO);
            // 
            // grpCurrency
            // 
            this.grpCurrency.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpCurrency.Controls.Add(this.lblNote);
            this.grpCurrency.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpCurrency.Location = new System.Drawing.Point(9, 22);
            this.grpCurrency.Name = "grpCurrency";
            this.grpCurrency.Size = new System.Drawing.Size(819, 311);
            this.grpCurrency.TabIndex = 14;
            this.grpCurrency.TabStop = false;
            this.grpCurrency.Text = "Currency Details";
            // 
            // lblNote
            // 
            this.lblNote.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblNote.AutoSize = true;
            this.lblNote.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNote.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblNote.Location = new System.Drawing.Point(7, 237);
            this.lblNote.Name = "lblNote";
            this.lblNote.Size = new System.Drawing.Size(605, 65);
            this.lblNote.TabIndex = 15;
            this.lblNote.Text = resources.GetString("lblNote.Text");
            // 
            // currencyIdDataGridViewTextBoxColumn
            // 
            this.currencyIdDataGridViewTextBoxColumn.DataPropertyName = "CurrencyId";
            this.currencyIdDataGridViewTextBoxColumn.HeaderText = "CurrencyId";
            this.currencyIdDataGridViewTextBoxColumn.Name = "currencyIdDataGridViewTextBoxColumn";
            this.currencyIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.currencyIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // currencyCodeDataGridViewTextBoxColumn
            // 
            this.currencyCodeDataGridViewTextBoxColumn.DataPropertyName = "CurrencyCode";
            this.currencyCodeDataGridViewTextBoxColumn.HeaderText = "Currency Code";
            this.currencyCodeDataGridViewTextBoxColumn.Name = "currencyCodeDataGridViewTextBoxColumn";
            // 
            // currencySymbolDataGridViewTextBoxColumn
            // 
            this.currencySymbolDataGridViewTextBoxColumn.DataPropertyName = "CurrencySymbol";
            this.currencySymbolDataGridViewTextBoxColumn.HeaderText = "Currency Symbol";
            this.currencySymbolDataGridViewTextBoxColumn.Name = "currencySymbolDataGridViewTextBoxColumn";
            // 
            // descriptionDataGridViewTextBoxColumn
            // 
            this.descriptionDataGridViewTextBoxColumn.DataPropertyName = "Description";
            this.descriptionDataGridViewTextBoxColumn.HeaderText = "Description";
            this.descriptionDataGridViewTextBoxColumn.Name = "descriptionDataGridViewTextBoxColumn";
            // 
            // buyRateDataGridViewTextBoxColumn
            // 
            this.buyRateDataGridViewTextBoxColumn.DataPropertyName = "BuyRate";
            this.buyRateDataGridViewTextBoxColumn.HeaderText = "Buy Rate";
            this.buyRateDataGridViewTextBoxColumn.Name = "buyRateDataGridViewTextBoxColumn";
            // 
            // sellRateDataGridViewTextBoxColumn
            // 
            this.sellRateDataGridViewTextBoxColumn.DataPropertyName = "SellRate";
            this.sellRateDataGridViewTextBoxColumn.HeaderText = "Sell Rate";
            this.sellRateDataGridViewTextBoxColumn.Name = "sellRateDataGridViewTextBoxColumn";
            // 
            // effectiveDateDataGridViewTextBoxColumn
            // 
            this.effectiveDateDataGridViewTextBoxColumn.DataPropertyName = "EffectiveDate";
            this.effectiveDateDataGridViewTextBoxColumn.HeaderText = "Effective Date";
            this.effectiveDateDataGridViewTextBoxColumn.Name = "effectiveDateDataGridViewTextBoxColumn";
            this.effectiveDateDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // createdDateDataGridViewTextBoxColumn
            // 
            this.createdDateDataGridViewTextBoxColumn.DataPropertyName = "CreatedDate";
            this.createdDateDataGridViewTextBoxColumn.HeaderText = "Created Date";
            this.createdDateDataGridViewTextBoxColumn.Name = "createdDateDataGridViewTextBoxColumn";
            this.createdDateDataGridViewTextBoxColumn.ReadOnly = true;
            this.createdDateDataGridViewTextBoxColumn.Visible = false;
            // 
            // lastModifiedDateDataGridViewTextBoxColumn
            // 
            this.lastModifiedDateDataGridViewTextBoxColumn.DataPropertyName = "LastModifiedDate";
            this.lastModifiedDateDataGridViewTextBoxColumn.HeaderText = "Last Modified Date";
            this.lastModifiedDateDataGridViewTextBoxColumn.Name = "lastModifiedDateDataGridViewTextBoxColumn";
            this.lastModifiedDateDataGridViewTextBoxColumn.ReadOnly = true;
            this.lastModifiedDateDataGridViewTextBoxColumn.Visible = false;
            // 
            // lastModifiedByDataGridViewTextBoxColumn
            // 
            this.lastModifiedByDataGridViewTextBoxColumn.DataPropertyName = "LastModifiedBy";
            this.lastModifiedByDataGridViewTextBoxColumn.HeaderText = "LastModifiedBy";
            this.lastModifiedByDataGridViewTextBoxColumn.Name = "lastModifiedByDataGridViewTextBoxColumn";
            this.lastModifiedByDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // isActiveDataGridViewCheckBoxColumn
            // 
            this.isActiveDataGridViewCheckBoxColumn.DataPropertyName = "IsActive";
            this.isActiveDataGridViewCheckBoxColumn.HeaderText = "IsActive";
            this.isActiveDataGridViewCheckBoxColumn.Name = "isActiveDataGridViewCheckBoxColumn";
            // 
            // frmCurrency
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(837, 374);
            this.Controls.Add(this.dgvCurrency);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.grpCurrency);
            this.Name = "frmCurrency";
            this.Text = "Currency";
            ((System.ComponentModel.ISupportInitialize)(this.dgvCurrency)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.currencyDTOBindingSource)).EndInit();
            this.grpCurrency.ResumeLayout(false);
            this.grpCurrency.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.DataGridView dgvCurrency;
        private System.Windows.Forms.GroupBox grpCurrency;
        private System.Windows.Forms.BindingSource currencyDTOBindingSource;
        private System.Windows.Forms.Label lblNote;
        private System.Windows.Forms.DataGridViewTextBoxColumn currencyIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn currencyCodeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn currencySymbolDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn buyRateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn sellRateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn effectiveDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn createdDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastModifiedDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastModifiedByDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isActiveDataGridViewCheckBoxColumn;
    }
}

