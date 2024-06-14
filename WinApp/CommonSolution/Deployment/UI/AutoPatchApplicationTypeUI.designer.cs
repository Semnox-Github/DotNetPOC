 namespace Semnox.Parafait.Deployment
{
    partial class AutoPatchApplicationTypeUI
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
            this.applicationTypeDeleteBtn = new System.Windows.Forms.Button();
            this.applicationTypeCloseBtn = new System.Windows.Forms.Button();
            this.applicationTypeRefreshBtn = new System.Windows.Forms.Button();
            this.applicationTypeSaveBtn = new System.Windows.Forms.Button();
            this.applicationTypeDataGridView = new System.Windows.Forms.DataGridView();
            this.patchApplicationTypeDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.patchApplicationTypeIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.applicationTypeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isActiveDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.applicationTypeDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.patchApplicationTypeDTOBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // applicationTypeDeleteBtn
            // 
            this.applicationTypeDeleteBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.applicationTypeDeleteBtn.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.applicationTypeDeleteBtn.Location = new System.Drawing.Point(269, 255);
            this.applicationTypeDeleteBtn.Name = "applicationTypeDeleteBtn";
            this.applicationTypeDeleteBtn.Size = new System.Drawing.Size(75, 23);
            this.applicationTypeDeleteBtn.TabIndex = 15;
            this.applicationTypeDeleteBtn.Text = "Delete";
            this.applicationTypeDeleteBtn.UseVisualStyleBackColor = true;
            this.applicationTypeDeleteBtn.Click += new System.EventHandler(this.applicationTypeDeleteBtn_Click);
            // 
            // applicationTypeCloseBtn
            // 
            this.applicationTypeCloseBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.applicationTypeCloseBtn.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.applicationTypeCloseBtn.Location = new System.Drawing.Point(397, 255);
            this.applicationTypeCloseBtn.Name = "applicationTypeCloseBtn";
            this.applicationTypeCloseBtn.Size = new System.Drawing.Size(75, 23);
            this.applicationTypeCloseBtn.TabIndex = 16;
            this.applicationTypeCloseBtn.Text = "Close";
            this.applicationTypeCloseBtn.UseVisualStyleBackColor = true;
            this.applicationTypeCloseBtn.Click += new System.EventHandler(this.applicationTypeCloseBtn_Click);
            // 
            // applicationTypeRefreshBtn
            // 
            this.applicationTypeRefreshBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.applicationTypeRefreshBtn.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.applicationTypeRefreshBtn.Location = new System.Drawing.Point(141, 255);
            this.applicationTypeRefreshBtn.Name = "applicationTypeRefreshBtn";
            this.applicationTypeRefreshBtn.Size = new System.Drawing.Size(75, 23);
            this.applicationTypeRefreshBtn.TabIndex = 14;
            this.applicationTypeRefreshBtn.Text = "Refresh";
            this.applicationTypeRefreshBtn.UseVisualStyleBackColor = true;
            this.applicationTypeRefreshBtn.Click += new System.EventHandler(this.applicationTypeRefreshBtn_Click);
            // 
            // applicationTypeSaveBtn
            // 
            this.applicationTypeSaveBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.applicationTypeSaveBtn.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.applicationTypeSaveBtn.Location = new System.Drawing.Point(13, 255);
            this.applicationTypeSaveBtn.Name = "applicationTypeSaveBtn";
            this.applicationTypeSaveBtn.Size = new System.Drawing.Size(75, 23);
            this.applicationTypeSaveBtn.TabIndex = 13;
            this.applicationTypeSaveBtn.Text = "Save";
            this.applicationTypeSaveBtn.UseVisualStyleBackColor = true;
            this.applicationTypeSaveBtn.Click += new System.EventHandler(this.applicationTypeSaveBtn_Click);
            // 
            // applicationTypeDataGridView
            // 
            this.applicationTypeDataGridView.AllowUserToDeleteRows = false;
            this.applicationTypeDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.applicationTypeDataGridView.AutoGenerateColumns = false;
            this.applicationTypeDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.applicationTypeDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.patchApplicationTypeIdDataGridViewTextBoxColumn,
            this.applicationTypeDataGridViewTextBoxColumn,
            this.isActiveDataGridViewTextBoxColumn});
            this.applicationTypeDataGridView.DataSource = this.patchApplicationTypeDTOBindingSource;
            this.applicationTypeDataGridView.Location = new System.Drawing.Point(12, 12);
            this.applicationTypeDataGridView.Name = "applicationTypeDataGridView";
            this.applicationTypeDataGridView.Size = new System.Drawing.Size(817, 225);
            this.applicationTypeDataGridView.TabIndex = 12;
            // 
            // patchApplicationTypeDTOBindingSource
            // 
            this.patchApplicationTypeDTOBindingSource.DataSource = typeof(AutoPatchApplTypeDTO);
            // 
            // patchApplicationTypeIdDataGridViewTextBoxColumn
            // 
            this.patchApplicationTypeIdDataGridViewTextBoxColumn.DataPropertyName = "PatchApplicationTypeId";
            this.patchApplicationTypeIdDataGridViewTextBoxColumn.HeaderText = "Application Type Id";
            this.patchApplicationTypeIdDataGridViewTextBoxColumn.Name = "patchApplicationTypeIdDataGridViewTextBoxColumn";
            this.patchApplicationTypeIdDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // applicationTypeDataGridViewTextBoxColumn
            // 
            this.applicationTypeDataGridViewTextBoxColumn.DataPropertyName = "ApplicationType";
            this.applicationTypeDataGridViewTextBoxColumn.HeaderText = "Application Type";
            this.applicationTypeDataGridViewTextBoxColumn.Name = "applicationTypeDataGridViewTextBoxColumn";
            // 
            // isActiveDataGridViewTextBoxColumn
            // 
            this.isActiveDataGridViewTextBoxColumn.DataPropertyName = "IsActive";
            this.isActiveDataGridViewTextBoxColumn.FalseValue = "N";
            this.isActiveDataGridViewTextBoxColumn.HeaderText = "Active?";
            this.isActiveDataGridViewTextBoxColumn.Name = "isActiveDataGridViewTextBoxColumn";
            this.isActiveDataGridViewTextBoxColumn.ReadOnly = true;
            this.isActiveDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.isActiveDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.isActiveDataGridViewTextBoxColumn.TrueValue = "Y";
            // 
            // AutoPatchApplicationTypeUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(841, 295);
            this.Controls.Add(this.applicationTypeDeleteBtn);
            this.Controls.Add(this.applicationTypeCloseBtn);
            this.Controls.Add(this.applicationTypeRefreshBtn);
            this.Controls.Add(this.applicationTypeSaveBtn);
            this.Controls.Add(this.applicationTypeDataGridView);
            this.Name = "AutoPatchApplicationTypeUI";
            this.Text = "Application Type";
            ((System.ComponentModel.ISupportInitialize)(this.applicationTypeDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.patchApplicationTypeDTOBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button applicationTypeDeleteBtn;
        private System.Windows.Forms.Button applicationTypeCloseBtn;
        private System.Windows.Forms.Button applicationTypeRefreshBtn;
        private System.Windows.Forms.Button applicationTypeSaveBtn;
        private System.Windows.Forms.DataGridView applicationTypeDataGridView;
        private System.Windows.Forms.BindingSource patchApplicationTypeDTOBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn patchApplicationTypeIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn applicationTypeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isActiveDataGridViewTextBoxColumn;
    }
}