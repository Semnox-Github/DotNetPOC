namespace Semnox.Parafait.Customer.UI
{
    partial class SelectRelatedCustomerUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectRelatedCustomerUI));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chbSelectAll = new Semnox.Core.GenericUtilities.CustomCheckBox();
            this.grpCustomerRelationships = new System.Windows.Forms.GroupBox();
            this.verticalScrollBarView2 = new Semnox.Core.GenericUtilities.VerticalScrollBarView();
            this.dgvCustomerRelationshipDTOList = new System.Windows.Forms.DataGridView();
            this.customerSelected = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.viewCustomerDetailDataGridViewButtonColumn = new System.Windows.Forms.DataGridViewButtonColumn();
            this.horizontalScrollBarView2 = new Semnox.Core.GenericUtilities.HorizontalScrollBarView();
            this.btnConfirmSelection = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chbMainCustomer = new Semnox.Core.GenericUtilities.CustomCheckBox();
            this.idDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customerNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.relatedCustomerNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customerRelationshipTypeIdDataGridViewComboBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.effectiveDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.expiryDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isActiveDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.customerRelationshipDTOListBS = new System.Windows.Forms.BindingSource(this.components);
            this.btnCustomerDetail = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.grpCustomerRelationships.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCustomerRelationshipDTOList)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.customerRelationshipDTOListBS)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chbSelectAll);
            this.groupBox1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(6, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(733, 66);
            this.groupBox1.TabIndex = 53;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Selection";
            // 
            // chbSelectAll
            // 
            this.chbSelectAll.Appearance = System.Windows.Forms.Appearance.Button;
            this.chbSelectAll.BackColor = System.Drawing.Color.WhiteSmoke;
            this.chbSelectAll.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.chbSelectAll.FlatAppearance.BorderSize = 0;
            this.chbSelectAll.FlatAppearance.CheckedBackColor = System.Drawing.Color.WhiteSmoke;
            this.chbSelectAll.FlatAppearance.MouseDownBackColor = System.Drawing.Color.WhiteSmoke;
            this.chbSelectAll.FlatAppearance.MouseOverBackColor = System.Drawing.Color.WhiteSmoke;
            this.chbSelectAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chbSelectAll.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.chbSelectAll.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.chbSelectAll.ImageIndex = 1;
            this.chbSelectAll.Location = new System.Drawing.Point(6, 20);
            this.chbSelectAll.Name = "chbSelectAll";
            this.chbSelectAll.Size = new System.Drawing.Size(94, 32);
            this.chbSelectAll.TabIndex = 2;
            this.chbSelectAll.Text = "Select All";
            this.chbSelectAll.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chbSelectAll.UseVisualStyleBackColor = false;
            this.chbSelectAll.Click += new System.EventHandler(this.ChbSelectAll_Click);
            // 
            // grpCustomerRelationships
            // 
            this.grpCustomerRelationships.Controls.Add(this.verticalScrollBarView2);
            this.grpCustomerRelationships.Controls.Add(this.horizontalScrollBarView2);
            this.grpCustomerRelationships.Controls.Add(this.dgvCustomerRelationshipDTOList);
            this.grpCustomerRelationships.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.grpCustomerRelationships.Location = new System.Drawing.Point(10, 158);
            this.grpCustomerRelationships.Name = "grpCustomerRelationships";
            this.grpCustomerRelationships.Size = new System.Drawing.Size(729, 286);
            this.grpCustomerRelationships.TabIndex = 54;
            this.grpCustomerRelationships.TabStop = false;
            this.grpCustomerRelationships.Text = "Relationships";
            // 
            // verticalScrollBarView2
            // 
            this.verticalScrollBarView2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.verticalScrollBarView2.AutoHide = false;
            this.verticalScrollBarView2.DataGridView = this.dgvCustomerRelationshipDTOList;
            this.verticalScrollBarView2.DownButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("verticalScrollBarView2.DownButtonBackgroundImage")));
            this.verticalScrollBarView2.DownButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("verticalScrollBarView2.DownButtonDisabledBackgroundImage")));
            this.verticalScrollBarView2.Location = new System.Drawing.Point(677, 19);
            this.verticalScrollBarView2.Margin = new System.Windows.Forms.Padding(0);
            this.verticalScrollBarView2.Name = "verticalScrollBarView2";
            this.verticalScrollBarView2.ScrollableControl = null;
            this.verticalScrollBarView2.ScrollViewer = null;
            this.verticalScrollBarView2.Size = new System.Drawing.Size(49, 221);
            this.verticalScrollBarView2.TabIndex = 35;
            this.verticalScrollBarView2.UpButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("verticalScrollBarView2.UpButtonBackgroundImage")));
            this.verticalScrollBarView2.UpButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("verticalScrollBarView2.UpButtonDisabledBackgroundImage")));
            // 
            // dgvCustomerRelationshipDTOList
            // 
            this.dgvCustomerRelationshipDTOList.AllowUserToAddRows = false;
            this.dgvCustomerRelationshipDTOList.AllowUserToDeleteRows = false;
            this.dgvCustomerRelationshipDTOList.AutoGenerateColumns = false;
            this.dgvCustomerRelationshipDTOList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvCustomerRelationshipDTOList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCustomerRelationshipDTOList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.customerSelected,
            this.idDataGridViewTextBoxColumn,
            this.customerNameDataGridViewTextBoxColumn,
            this.relatedCustomerNameDataGridViewTextBoxColumn,
            this.customerRelationshipTypeIdDataGridViewComboBoxColumn,
            this.effectiveDateDataGridViewTextBoxColumn,
            this.expiryDateDataGridViewTextBoxColumn,
            this.isActiveDataGridViewCheckBoxColumn,
            this.viewCustomerDetailDataGridViewButtonColumn});
            this.dgvCustomerRelationshipDTOList.DataSource = this.customerRelationshipDTOListBS;
            this.dgvCustomerRelationshipDTOList.Location = new System.Drawing.Point(6, 19);
            this.dgvCustomerRelationshipDTOList.Name = "dgvCustomerRelationshipDTOList";
            this.dgvCustomerRelationshipDTOList.ReadOnly = true;
            this.dgvCustomerRelationshipDTOList.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Arial", 15F);
            this.dgvCustomerRelationshipDTOList.RowTemplate.Height = 30;
            this.dgvCustomerRelationshipDTOList.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvCustomerRelationshipDTOList.Size = new System.Drawing.Size(668, 209);
            this.dgvCustomerRelationshipDTOList.TabIndex = 33;
            this.dgvCustomerRelationshipDTOList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvCustomerRelationshipDTOList_CellContentClick);
            this.dgvCustomerRelationshipDTOList.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvCustomerRelationshipDTOList_DataError);
            // 
            // customerSelected
            // 
            this.customerSelected.FalseValue = "0";
            this.customerSelected.HeaderText = "Select";
            this.customerSelected.Name = "customerSelected";
            this.customerSelected.ReadOnly = true;
            this.customerSelected.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.customerSelected.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.customerSelected.TrueValue = "1";
            // 
            // viewCustomerDetailDataGridViewButtonColumn
            // 
            this.viewCustomerDetailDataGridViewButtonColumn.HeaderText = "Details";
            this.viewCustomerDetailDataGridViewButtonColumn.Name = "viewCustomerDetailDataGridViewButtonColumn";
            this.viewCustomerDetailDataGridViewButtonColumn.ReadOnly = true;
            this.viewCustomerDetailDataGridViewButtonColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.viewCustomerDetailDataGridViewButtonColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // horizontalScrollBarView2
            // 
            this.horizontalScrollBarView2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.horizontalScrollBarView2.AutoHide = false;
            this.horizontalScrollBarView2.DataGridView = this.dgvCustomerRelationshipDTOList;
            this.horizontalScrollBarView2.LeftButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("horizontalScrollBarView2.LeftButtonBackgroundImage")));
            this.horizontalScrollBarView2.LeftButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("horizontalScrollBarView2.LeftButtonDisabledBackgroundImage")));
            this.horizontalScrollBarView2.Location = new System.Drawing.Point(2, 243);
            this.horizontalScrollBarView2.Margin = new System.Windows.Forms.Padding(0);
            this.horizontalScrollBarView2.Name = "horizontalScrollBarView2";
            this.horizontalScrollBarView2.RightButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("horizontalScrollBarView2.RightButtonBackgroundImage")));
            this.horizontalScrollBarView2.RightButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("horizontalScrollBarView2.RightButtonDisabledBackgroundImage")));
            this.horizontalScrollBarView2.ScrollableControl = null;
            this.horizontalScrollBarView2.ScrollViewer = null;
            this.horizontalScrollBarView2.Size = new System.Drawing.Size(668, 40);
            this.horizontalScrollBarView2.TabIndex = 34;
            // 
            // btnConfirmSelection
            // 
            this.btnConfirmSelection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConfirmSelection.BackColor = System.Drawing.Color.Transparent;
            this.btnConfirmSelection.BackgroundImage = global::Semnox.Parafait.Customer.Properties.Resources.normal2;
            this.btnConfirmSelection.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnConfirmSelection.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnConfirmSelection.FlatAppearance.BorderSize = 0;
            this.btnConfirmSelection.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnConfirmSelection.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnConfirmSelection.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConfirmSelection.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConfirmSelection.ForeColor = System.Drawing.Color.White;
            this.btnConfirmSelection.Location = new System.Drawing.Point(591, 450);
            this.btnConfirmSelection.Name = "btnConfirmSelection";
            this.btnConfirmSelection.Size = new System.Drawing.Size(148, 41);
            this.btnConfirmSelection.TabIndex = 57;
            this.btnConfirmSelection.Text = "Confirm Selection";
            this.btnConfirmSelection.UseVisualStyleBackColor = false;
            this.btnConfirmSelection.Click += new System.EventHandler(this.BtnConfirmSelection_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnCustomerDetail);
            this.groupBox2.Controls.Add(this.chbMainCustomer);
            this.groupBox2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.groupBox2.Location = new System.Drawing.Point(6, 85);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(730, 67);
            this.groupBox2.TabIndex = 58;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Customer";
            // 
            // chbMainCustomer
            // 
            this.chbMainCustomer.Appearance = System.Windows.Forms.Appearance.Button;
            this.chbMainCustomer.BackColor = System.Drawing.Color.WhiteSmoke;
            this.chbMainCustomer.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.chbMainCustomer.FlatAppearance.BorderSize = 0;
            this.chbMainCustomer.FlatAppearance.CheckedBackColor = System.Drawing.Color.WhiteSmoke;
            this.chbMainCustomer.FlatAppearance.MouseDownBackColor = System.Drawing.Color.WhiteSmoke;
            this.chbMainCustomer.FlatAppearance.MouseOverBackColor = System.Drawing.Color.WhiteSmoke;
            this.chbMainCustomer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chbMainCustomer.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.chbMainCustomer.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.chbMainCustomer.ImageIndex = 1;
            this.chbMainCustomer.Location = new System.Drawing.Point(10, 20);
            this.chbMainCustomer.Name = "chbMainCustomer";
            this.chbMainCustomer.Size = new System.Drawing.Size(94, 32);
            this.chbMainCustomer.TabIndex = 3;
            this.chbMainCustomer.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chbMainCustomer.UseVisualStyleBackColor = false;
            this.chbMainCustomer.CheckedChanged += new System.EventHandler(this.chbMainCustomer_CheckedChanged);
            // 
            // idDataGridViewTextBoxColumn
            // 
            this.idDataGridViewTextBoxColumn.DataPropertyName = "Id";
            this.idDataGridViewTextBoxColumn.HeaderText = "Id";
            this.idDataGridViewTextBoxColumn.Name = "idDataGridViewTextBoxColumn";
            this.idDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // customerNameDataGridViewTextBoxColumn
            // 
            this.customerNameDataGridViewTextBoxColumn.DataPropertyName = "CustomerName";
            this.customerNameDataGridViewTextBoxColumn.HeaderText = "Customer";
            this.customerNameDataGridViewTextBoxColumn.Name = "customerNameDataGridViewTextBoxColumn";
            this.customerNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // relatedCustomerNameDataGridViewTextBoxColumn
            // 
            this.relatedCustomerNameDataGridViewTextBoxColumn.DataPropertyName = "RelatedCustomerName";
            this.relatedCustomerNameDataGridViewTextBoxColumn.HeaderText = "Related Customer";
            this.relatedCustomerNameDataGridViewTextBoxColumn.Name = "relatedCustomerNameDataGridViewTextBoxColumn";
            this.relatedCustomerNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // customerRelationshipTypeIdDataGridViewComboBoxColumn
            // 
            this.customerRelationshipTypeIdDataGridViewComboBoxColumn.DataPropertyName = "CustomerRelationshipTypeId";
            this.customerRelationshipTypeIdDataGridViewComboBoxColumn.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.Nothing;
            this.customerRelationshipTypeIdDataGridViewComboBoxColumn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.customerRelationshipTypeIdDataGridViewComboBoxColumn.HeaderText = "Customer Relationship Type";
            this.customerRelationshipTypeIdDataGridViewComboBoxColumn.Name = "customerRelationshipTypeIdDataGridViewComboBoxColumn";
            this.customerRelationshipTypeIdDataGridViewComboBoxColumn.ReadOnly = true;
            this.customerRelationshipTypeIdDataGridViewComboBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.customerRelationshipTypeIdDataGridViewComboBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // effectiveDateDataGridViewTextBoxColumn
            // 
            this.effectiveDateDataGridViewTextBoxColumn.DataPropertyName = "EffectiveDate";
            this.effectiveDateDataGridViewTextBoxColumn.HeaderText = "Effective Date";
            this.effectiveDateDataGridViewTextBoxColumn.Name = "effectiveDateDataGridViewTextBoxColumn";
            this.effectiveDateDataGridViewTextBoxColumn.ReadOnly = true;
            this.effectiveDateDataGridViewTextBoxColumn.Visible = false;
            // 
            // expiryDateDataGridViewTextBoxColumn
            // 
            this.expiryDateDataGridViewTextBoxColumn.DataPropertyName = "ExpiryDate";
            this.expiryDateDataGridViewTextBoxColumn.HeaderText = "Expiry Date";
            this.expiryDateDataGridViewTextBoxColumn.Name = "expiryDateDataGridViewTextBoxColumn";
            this.expiryDateDataGridViewTextBoxColumn.ReadOnly = true;
            this.expiryDateDataGridViewTextBoxColumn.Visible = false;
            // 
            // isActiveDataGridViewCheckBoxColumn
            // 
            this.isActiveDataGridViewCheckBoxColumn.DataPropertyName = "IsActive";
            this.isActiveDataGridViewCheckBoxColumn.HeaderText = "Active?";
            this.isActiveDataGridViewCheckBoxColumn.Name = "isActiveDataGridViewCheckBoxColumn";
            this.isActiveDataGridViewCheckBoxColumn.ReadOnly = true;
            this.isActiveDataGridViewCheckBoxColumn.Visible = false;
            // 
            // customerRelationshipDTOListBS
            // 
            this.customerRelationshipDTOListBS.DataSource = typeof(Semnox.Parafait.Customer.CustomerRelationshipDTO);
            // 
            // btnCustomerDetail
            // 
            this.btnCustomerDetail.Location = new System.Drawing.Point(584, 11);
            this.btnCustomerDetail.Name = "btnCustomerDetail";
            this.btnCustomerDetail.Size = new System.Drawing.Size(140, 50);
            this.btnCustomerDetail.TabIndex = 4;
            this.btnCustomerDetail.Text = "...";
            this.btnCustomerDetail.UseVisualStyleBackColor = true;
            this.btnCustomerDetail.Click += new System.EventHandler(this.btnCustomerDetail_Click);
            // 
            // SelectRelatedCustomerUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(760, 501);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnConfirmSelection);
            this.Controls.Add(this.grpCustomerRelationships);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "SelectRelatedCustomerUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select Customers";
            this.Load += new System.EventHandler(this.SelectRelatedCustomerUI_Load);
            this.groupBox1.ResumeLayout(false);
            this.grpCustomerRelationships.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCustomerRelationshipDTOList)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.customerRelationshipDTOListBS)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox1;
        private Core.GenericUtilities.CustomCheckBox chbSelectAll;
        private System.Windows.Forms.GroupBox grpCustomerRelationships;
        private System.Windows.Forms.DataGridView dgvCustomerRelationshipDTOList;
        private System.Windows.Forms.BindingSource customerRelationshipDTOListBS;
        private Core.GenericUtilities.VerticalScrollBarView verticalScrollBarView2;
        private Core.GenericUtilities.HorizontalScrollBarView horizontalScrollBarView2;
        private System.Windows.Forms.Button btnConfirmSelection;
        private System.Windows.Forms.DataGridViewCheckBoxColumn customerSelected;
        private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn customerNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn relatedCustomerNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn customerRelationshipTypeIdDataGridViewComboBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn effectiveDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn expiryDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isActiveDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewButtonColumn viewCustomerDetailDataGridViewButtonColumn;
        private System.Windows.Forms.GroupBox groupBox2;
        private Core.GenericUtilities.CustomCheckBox chbMainCustomer;
        private System.Windows.Forms.Button btnCustomerDetail;
    }
}