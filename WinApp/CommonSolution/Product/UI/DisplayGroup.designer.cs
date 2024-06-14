namespace Semnox.Parafait.Product
{
    partial class DisplayGroup
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
            this.dgvDisplayGroup = new System.Windows.Forms.DataGridView();
            this.idDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.displayGroupDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buttonColorDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.textColorDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fontDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sortOrderDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.imageFileNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastUpdatedByDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastUpdatedDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.searchByDisplayGroupsParametersBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.grpDisplayGroup = new System.Windows.Forms.GroupBox();
            this.lnkHQPublish = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDisplayGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchByDisplayGroupsParametersBindingSource)).BeginInit();
            this.grpDisplayGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvDisplayGroup
            // 
            this.dgvDisplayGroup.AllowUserToDeleteRows = false;
            this.dgvDisplayGroup.AllowUserToResizeColumns = false;
            this.dgvDisplayGroup.AllowUserToResizeRows = false;
            this.dgvDisplayGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvDisplayGroup.AutoGenerateColumns = false;
            this.dgvDisplayGroup.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDisplayGroup.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idDataGridViewTextBoxColumn,
            this.displayGroupDataGridViewTextBoxColumn,
            this.Description,
            this.buttonColorDataGridViewTextBoxColumn,
            this.textColorDataGridViewTextBoxColumn,
            this.fontDataGridViewTextBoxColumn,
            this.sortOrderDataGridViewTextBoxColumn,
            this.imageFileNameDataGridViewTextBoxColumn,
            this.lastUpdatedByDataGridViewTextBoxColumn,
            this.lastUpdatedDateDataGridViewTextBoxColumn});
            this.dgvDisplayGroup.DataSource = this.searchByDisplayGroupsParametersBindingSource;
            this.dgvDisplayGroup.Location = new System.Drawing.Point(10, 21);
            this.dgvDisplayGroup.MultiSelect = false;
            this.dgvDisplayGroup.Name = "dgvDisplayGroup";
            this.dgvDisplayGroup.Size = new System.Drawing.Size(643, 328);
            this.dgvDisplayGroup.TabIndex = 0;
            this.dgvDisplayGroup.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dgvDisplayGroup_CellValidating);
            this.dgvDisplayGroup.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvDisplayGroup_DataError);
            // 
            // idDataGridViewTextBoxColumn
            // 
            this.idDataGridViewTextBoxColumn.DataPropertyName = "Id";
            this.idDataGridViewTextBoxColumn.HeaderText = "Id";
            this.idDataGridViewTextBoxColumn.Name = "idDataGridViewTextBoxColumn";
            this.idDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // displayGroupDataGridViewTextBoxColumn
            // 
            this.displayGroupDataGridViewTextBoxColumn.DataPropertyName = "DisplayGroup";
            this.displayGroupDataGridViewTextBoxColumn.HeaderText = "Display Group";
            this.displayGroupDataGridViewTextBoxColumn.Name = "displayGroupDataGridViewTextBoxColumn";
            // 
            // Description
            // 
            this.Description.DataPropertyName = "Description";
            this.Description.HeaderText = "Description";
            this.Description.Name = "Description";
            // 
            // buttonColorDataGridViewTextBoxColumn
            // 
            this.buttonColorDataGridViewTextBoxColumn.DataPropertyName = "ButtonColor";
            this.buttonColorDataGridViewTextBoxColumn.HeaderText = "ButtonColor";
            this.buttonColorDataGridViewTextBoxColumn.Name = "buttonColorDataGridViewTextBoxColumn";
            this.buttonColorDataGridViewTextBoxColumn.Visible = false;
            // 
            // textColorDataGridViewTextBoxColumn
            // 
            this.textColorDataGridViewTextBoxColumn.DataPropertyName = "TextColor";
            this.textColorDataGridViewTextBoxColumn.HeaderText = "TextColor";
            this.textColorDataGridViewTextBoxColumn.Name = "textColorDataGridViewTextBoxColumn";
            this.textColorDataGridViewTextBoxColumn.Visible = false;
            // 
            // fontDataGridViewTextBoxColumn
            // 
            this.fontDataGridViewTextBoxColumn.DataPropertyName = "Font";
            this.fontDataGridViewTextBoxColumn.HeaderText = "Font";
            this.fontDataGridViewTextBoxColumn.Name = "fontDataGridViewTextBoxColumn";
            this.fontDataGridViewTextBoxColumn.Visible = false;
            // 
            // sortOrderDataGridViewTextBoxColumn
            // 
            this.sortOrderDataGridViewTextBoxColumn.DataPropertyName = "SortOrder";
            this.sortOrderDataGridViewTextBoxColumn.HeaderText = "Sort Order";
            this.sortOrderDataGridViewTextBoxColumn.Name = "sortOrderDataGridViewTextBoxColumn";
            // 
            // imageFileNameDataGridViewTextBoxColumn
            // 
            this.imageFileNameDataGridViewTextBoxColumn.DataPropertyName = "ImageFileName";
            this.imageFileNameDataGridViewTextBoxColumn.HeaderText = "ImageFileName";
            this.imageFileNameDataGridViewTextBoxColumn.Name = "imageFileNameDataGridViewTextBoxColumn";
            this.imageFileNameDataGridViewTextBoxColumn.Visible = false;
            // 
            // lastUpdatedByDataGridViewTextBoxColumn
            // 
            this.lastUpdatedByDataGridViewTextBoxColumn.DataPropertyName = "LastUpdatedBy";
            this.lastUpdatedByDataGridViewTextBoxColumn.HeaderText = "Last Updated By";
            this.lastUpdatedByDataGridViewTextBoxColumn.Name = "lastUpdatedByDataGridViewTextBoxColumn";
            this.lastUpdatedByDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // lastUpdatedDateDataGridViewTextBoxColumn
            // 
            this.lastUpdatedDateDataGridViewTextBoxColumn.DataPropertyName = "LastUpdatedDate";
            this.lastUpdatedDateDataGridViewTextBoxColumn.HeaderText = "Last Updated Date";
            this.lastUpdatedDateDataGridViewTextBoxColumn.Name = "lastUpdatedDateDataGridViewTextBoxColumn";
            this.lastUpdatedDateDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // searchByDisplayGroupsParametersBindingSource
            // 
            this.searchByDisplayGroupsParametersBindingSource.DataSource = typeof(Semnox.Parafait.Product.ProductDisplayGroupFormatDTO);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.CausesValidation = false;
            this.btnDelete.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelete.Location = new System.Drawing.Point(217, 379);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 12;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(318, 379);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "Close";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRefresh.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefresh.Location = new System.Drawing.Point(116, 379);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 11;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(15, 379);
            this.btnSave.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // grpDisplayGroup
            // 
            this.grpDisplayGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpDisplayGroup.Controls.Add(this.dgvDisplayGroup);
            this.grpDisplayGroup.Location = new System.Drawing.Point(10, 7);
            this.grpDisplayGroup.Margin = new System.Windows.Forms.Padding(3, 3, 5, 5);
            this.grpDisplayGroup.Name = "grpDisplayGroup";
            this.grpDisplayGroup.Padding = new System.Windows.Forms.Padding(0);
            this.grpDisplayGroup.Size = new System.Drawing.Size(660, 358);
            this.grpDisplayGroup.TabIndex = 14;
            this.grpDisplayGroup.TabStop = false;
            this.grpDisplayGroup.Text = "Enter Display Group";
            // 
            // lnkHQPublish
            // 
            this.lnkHQPublish.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lnkHQPublish.AutoSize = true;
            this.lnkHQPublish.BackColor = System.Drawing.Color.Blue;
            this.lnkHQPublish.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkHQPublish.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lnkHQPublish.LinkColor = System.Drawing.Color.White;
            this.lnkHQPublish.Location = new System.Drawing.Point(573, 391);
            this.lnkHQPublish.Name = "lnkHQPublish";
            this.lnkHQPublish.Size = new System.Drawing.Size(97, 15);
            this.lnkHQPublish.TabIndex = 39;
            this.lnkHQPublish.TabStop = true;
            this.lnkHQPublish.Text = "Publish To Sites";
            this.lnkHQPublish.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkHQPublish_LinkClicked);
            // 
            // DisplayGroup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(681, 415);
            this.Controls.Add(this.lnkHQPublish);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.grpDisplayGroup);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DisplayGroup";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Display Group";
            ((System.ComponentModel.ISupportInitialize)(this.dgvDisplayGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchByDisplayGroupsParametersBindingSource)).EndInit();
            this.grpDisplayGroup.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvDisplayGroup;
        private System.Windows.Forms.BindingSource searchByDisplayGroupsParametersBindingSource;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.GroupBox grpDisplayGroup;
        private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn displayGroupDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Description;
        private System.Windows.Forms.DataGridViewTextBoxColumn buttonColorDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn textColorDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn fontDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn sortOrderDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn imageFileNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastUpdatedByDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastUpdatedDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.LinkLabel lnkHQPublish;
    }
}

