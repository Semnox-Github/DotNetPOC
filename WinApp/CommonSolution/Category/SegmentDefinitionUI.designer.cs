namespace Semnox.Parafait.Category
{
    partial class SegmentDefinitionUI
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
            this.segmentDefinitionDeleteBtn = new System.Windows.Forms.Button();
            this.segmentDefinitionCloseBtn = new System.Windows.Forms.Button();
            this.segmentDefinitionRefreshBtn = new System.Windows.Forms.Button();
            this.segmentDefinitionSaveBtn = new System.Windows.Forms.Button();
            this.segmentDefinitionDataGridView = new System.Windows.Forms.DataGridView();
            this.segmentDefinitionIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.segmentNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.applicableEntityDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.sequenceOrderDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isMandatoryDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.isActiveDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.segmentDefinitionDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.lnkPublishToSite = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.segmentDefinitionDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.segmentDefinitionDTOBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // segmentDefinitionDeleteBtn
            // 
            this.segmentDefinitionDeleteBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.segmentDefinitionDeleteBtn.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.segmentDefinitionDeleteBtn.Location = new System.Drawing.Point(265, 548);
            this.segmentDefinitionDeleteBtn.Name = "segmentDefinitionDeleteBtn";
            this.segmentDefinitionDeleteBtn.Size = new System.Drawing.Size(75, 23);
            this.segmentDefinitionDeleteBtn.TabIndex = 20;
            this.segmentDefinitionDeleteBtn.Text = "Delete";
            this.segmentDefinitionDeleteBtn.UseVisualStyleBackColor = true;
            this.segmentDefinitionDeleteBtn.Click += new System.EventHandler(this.segmentDefinitionDeleteBtn_Click);
            // 
            // segmentDefinitionCloseBtn
            // 
            this.segmentDefinitionCloseBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.segmentDefinitionCloseBtn.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.segmentDefinitionCloseBtn.Location = new System.Drawing.Point(393, 548);
            this.segmentDefinitionCloseBtn.Name = "segmentDefinitionCloseBtn";
            this.segmentDefinitionCloseBtn.Size = new System.Drawing.Size(75, 23);
            this.segmentDefinitionCloseBtn.TabIndex = 21;
            this.segmentDefinitionCloseBtn.Text = "Close";
            this.segmentDefinitionCloseBtn.UseVisualStyleBackColor = true;
            this.segmentDefinitionCloseBtn.Click += new System.EventHandler(this.segmentDefinitionCloseBtn_Click);
            // 
            // segmentDefinitionRefreshBtn
            // 
            this.segmentDefinitionRefreshBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.segmentDefinitionRefreshBtn.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.segmentDefinitionRefreshBtn.Location = new System.Drawing.Point(137, 548);
            this.segmentDefinitionRefreshBtn.Name = "segmentDefinitionRefreshBtn";
            this.segmentDefinitionRefreshBtn.Size = new System.Drawing.Size(75, 23);
            this.segmentDefinitionRefreshBtn.TabIndex = 19;
            this.segmentDefinitionRefreshBtn.Text = "Refresh";
            this.segmentDefinitionRefreshBtn.UseVisualStyleBackColor = true;
            this.segmentDefinitionRefreshBtn.Click += new System.EventHandler(this.segmentDefinitionRefreshBtn_Click);
            // 
            // segmentDefinitionSaveBtn
            // 
            this.segmentDefinitionSaveBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.segmentDefinitionSaveBtn.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.segmentDefinitionSaveBtn.Location = new System.Drawing.Point(9, 548);
            this.segmentDefinitionSaveBtn.Name = "segmentDefinitionSaveBtn";
            this.segmentDefinitionSaveBtn.Size = new System.Drawing.Size(75, 23);
            this.segmentDefinitionSaveBtn.TabIndex = 18;
            this.segmentDefinitionSaveBtn.Text = "Save";
            this.segmentDefinitionSaveBtn.UseVisualStyleBackColor = true;
            this.segmentDefinitionSaveBtn.Click += new System.EventHandler(this.segmentDefinitionSaveBtn_Click);
            // 
            // segmentDefinitionDataGridView
            // 
            this.segmentDefinitionDataGridView.AllowUserToDeleteRows = false;
            this.segmentDefinitionDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.segmentDefinitionDataGridView.AutoGenerateColumns = false;
            this.segmentDefinitionDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.segmentDefinitionDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.segmentDefinitionIdDataGridViewTextBoxColumn,
            this.segmentNameDataGridViewTextBoxColumn,
            this.applicableEntityDataGridViewTextBoxColumn,
            this.sequenceOrderDataGridViewTextBoxColumn,
            this.isMandatoryDataGridViewTextBoxColumn,
            this.isActiveDataGridViewTextBoxColumn});
            this.segmentDefinitionDataGridView.DataSource = this.segmentDefinitionDTOBindingSource;
            this.segmentDefinitionDataGridView.Location = new System.Drawing.Point(7, 6);
            this.segmentDefinitionDataGridView.Name = "segmentDefinitionDataGridView";
            this.segmentDefinitionDataGridView.Size = new System.Drawing.Size(1159, 524);
            this.segmentDefinitionDataGridView.TabIndex = 17;
            // 
            // segmentDefinitionIdDataGridViewTextBoxColumn
            // 
            this.segmentDefinitionIdDataGridViewTextBoxColumn.DataPropertyName = "SegmentDefinitionId";
            this.segmentDefinitionIdDataGridViewTextBoxColumn.HeaderText = "Definition Id";
            this.segmentDefinitionIdDataGridViewTextBoxColumn.Name = "segmentDefinitionIdDataGridViewTextBoxColumn";
            this.segmentDefinitionIdDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // segmentNameDataGridViewTextBoxColumn
            // 
            this.segmentNameDataGridViewTextBoxColumn.DataPropertyName = "SegmentName";
            this.segmentNameDataGridViewTextBoxColumn.HeaderText = "Name";
            this.segmentNameDataGridViewTextBoxColumn.Name = "segmentNameDataGridViewTextBoxColumn";
            // 
            // applicableEntityDataGridViewTextBoxColumn
            // 
            this.applicableEntityDataGridViewTextBoxColumn.DataPropertyName = "ApplicableEntity";
            this.applicableEntityDataGridViewTextBoxColumn.HeaderText = "Applicable Entity";
            this.applicableEntityDataGridViewTextBoxColumn.Items.AddRange(new object[] {
            "PRODUCT",
            "POS PRODUCTS"});
            this.applicableEntityDataGridViewTextBoxColumn.Name = "applicableEntityDataGridViewTextBoxColumn";
            this.applicableEntityDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.applicableEntityDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // sequenceOrderDataGridViewTextBoxColumn
            // 
            this.sequenceOrderDataGridViewTextBoxColumn.DataPropertyName = "SequenceOrder";
            this.sequenceOrderDataGridViewTextBoxColumn.HeaderText = "Sequence Order";
            this.sequenceOrderDataGridViewTextBoxColumn.Name = "sequenceOrderDataGridViewTextBoxColumn";
            // 
            // isMandatoryDataGridViewTextBoxColumn
            // 
            this.isMandatoryDataGridViewTextBoxColumn.DataPropertyName = "IsMandatory";
            this.isMandatoryDataGridViewTextBoxColumn.FalseValue = "N";
            this.isMandatoryDataGridViewTextBoxColumn.HeaderText = "Mandatory?";
            this.isMandatoryDataGridViewTextBoxColumn.Name = "isMandatoryDataGridViewTextBoxColumn";
            this.isMandatoryDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.isMandatoryDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.isMandatoryDataGridViewTextBoxColumn.TrueValue = "Y";
            // 
            // isActiveDataGridViewTextBoxColumn
            // 
            this.isActiveDataGridViewTextBoxColumn.DataPropertyName = "IsActive";
            this.isActiveDataGridViewTextBoxColumn.FalseValue = "false";
            this.isActiveDataGridViewTextBoxColumn.HeaderText = "Active?";
            this.isActiveDataGridViewTextBoxColumn.Name = "isActiveDataGridViewTextBoxColumn";
            this.isActiveDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.isActiveDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.isActiveDataGridViewTextBoxColumn.TrueValue = "true";
            // 
            // segmentDefinitionDTOBindingSource
            // 
            this.segmentDefinitionDTOBindingSource.AllowNew = true;
            this.segmentDefinitionDTOBindingSource.DataSource = typeof(Semnox.Parafait.Product.SegmentDefinitionDTO);
            // 
            // lnkPublishToSite
            // 
            this.lnkPublishToSite.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lnkPublishToSite.AutoSize = true;
            this.lnkPublishToSite.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkPublishToSite.Location = new System.Drawing.Point(719, 569);
            this.lnkPublishToSite.Name = "lnkPublishToSite";
            this.lnkPublishToSite.Size = new System.Drawing.Size(99, 13);
            this.lnkPublishToSite.TabIndex = 51;
            this.lnkPublishToSite.TabStop = true;
            this.lnkPublishToSite.Text = "Publish To Sites";
            this.lnkPublishToSite.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkPublishToSite_LinkClicked);
            // 
            // SegmentDefinitionUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1174, 591);
            this.Controls.Add(this.lnkPublishToSite);
            this.Controls.Add(this.segmentDefinitionDeleteBtn);
            this.Controls.Add(this.segmentDefinitionCloseBtn);
            this.Controls.Add(this.segmentDefinitionRefreshBtn);
            this.Controls.Add(this.segmentDefinitionSaveBtn);
            this.Controls.Add(this.segmentDefinitionDataGridView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "SegmentDefinitionUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Segment Definition";
            ((System.ComponentModel.ISupportInitialize)(this.segmentDefinitionDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.segmentDefinitionDTOBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button segmentDefinitionDeleteBtn;
        private System.Windows.Forms.Button segmentDefinitionCloseBtn;
        private System.Windows.Forms.Button segmentDefinitionRefreshBtn;
        private System.Windows.Forms.Button segmentDefinitionSaveBtn;
        private System.Windows.Forms.DataGridView segmentDefinitionDataGridView;
        private System.Windows.Forms.BindingSource segmentDefinitionDTOBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn segmentDefinitionIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn segmentNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn applicableEntityDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn sequenceOrderDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isMandatoryDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isActiveDataGridViewTextBoxColumn;
        private System.Windows.Forms.LinkLabel lnkPublishToSite;
    }
}

