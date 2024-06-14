using Semnox.Core.GenericUtilities;

namespace Semnox.Parafait.Customer
{
    /// <summary>
    /// /
    /// </summary>
    partial class CustomerRelationshipListUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomerRelationshipListUI));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmbSearchCustomerRelationshipType = new System.Windows.Forms.ComboBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.lblName = new System.Windows.Forms.Label();
            this.chbShowActiveEntries = new Semnox.Core.GenericUtilities.CustomCheckBox();
            this.dgvCustomerRelationshipDTOList = new System.Windows.Forms.DataGridView();
            this.viewCustomerDetailDataGridViewButtonColumn = new System.Windows.Forms.DataGridViewButtonColumn();
            this.idDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.grpCustomerRelationships = new System.Windows.Forms.GroupBox();
            this.horizontalScrollBarView1 = new Semnox.Core.GenericUtilities.HorizontalScrollBarView();
            this.verticalScrollBarView1 = new Semnox.Core.GenericUtilities.VerticalScrollBarView();
            this.lblStatus = new System.Windows.Forms.Label();
            this.btnNew = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.customerNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.relatedCustomerNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customerRelationshipTypeIdDataGridViewComboBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.effectiveDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.expiryDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isActiveDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.customerRelationshipDTOListBS = new System.Windows.Forms.BindingSource(this.components);
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCustomerRelationshipDTOList)).BeginInit();
            this.grpCustomerRelationships.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.customerRelationshipDTOListBS)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmbSearchCustomerRelationshipType);
            this.groupBox1.Controls.Add(this.btnSearch);
            this.groupBox1.Controls.Add(this.lblName);
            this.groupBox1.Controls.Add(this.chbShowActiveEntries);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(10, 10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(914, 62);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Filter";
            // 
            // cmbSearchCustomerRelationshipType
            // 
            this.cmbSearchCustomerRelationshipType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSearchCustomerRelationshipType.Font = new System.Drawing.Font("Arial", 15F);
            this.cmbSearchCustomerRelationshipType.FormattingEnabled = true;
            this.cmbSearchCustomerRelationshipType.Location = new System.Drawing.Point(268, 20);
            this.cmbSearchCustomerRelationshipType.Name = "cmbSearchCustomerRelationshipType";
            this.cmbSearchCustomerRelationshipType.Size = new System.Drawing.Size(196, 31);
            this.cmbSearchCustomerRelationshipType.TabIndex = 5;
            // 
            // btnSearch
            // 
            this.btnSearch.BackgroundImage = global::Semnox.Parafait.Customer.Properties.Resources.customer_button_normal;
            this.btnSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSearch.FlatAppearance.BorderSize = 0;
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnSearch.ForeColor = System.Drawing.Color.White;
            this.btnSearch.Location = new System.Drawing.Point(758, 13);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(150, 40);
            this.btnSearch.TabIndex = 4;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // lblName
            // 
            this.lblName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblName.Location = new System.Drawing.Point(145, 26);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(123, 20);
            this.lblName.TabIndex = 2;
            this.lblName.Text = "Relationship Type :";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chbShowActiveEntries
            // 
            this.chbShowActiveEntries.Appearance = System.Windows.Forms.Appearance.Button;
            this.chbShowActiveEntries.BackColor = System.Drawing.Color.SlateGray;
            this.chbShowActiveEntries.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.chbShowActiveEntries.Checked = true;
            this.chbShowActiveEntries.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbShowActiveEntries.FlatAppearance.BorderSize = 0;
            this.chbShowActiveEntries.FlatAppearance.CheckedBackColor = System.Drawing.Color.WhiteSmoke;
            this.chbShowActiveEntries.FlatAppearance.MouseDownBackColor = System.Drawing.Color.WhiteSmoke;
            this.chbShowActiveEntries.FlatAppearance.MouseOverBackColor = System.Drawing.Color.WhiteSmoke;
            this.chbShowActiveEntries.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chbShowActiveEntries.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.chbShowActiveEntries.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.chbShowActiveEntries.ImageIndex = 0;
            this.chbShowActiveEntries.Location = new System.Drawing.Point(6, 20);
            this.chbShowActiveEntries.Name = "chbShowActiveEntries";
            this.chbShowActiveEntries.Size = new System.Drawing.Size(133, 32);
            this.chbShowActiveEntries.TabIndex = 1;
            this.chbShowActiveEntries.Text = "Show Active Only";
            this.chbShowActiveEntries.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chbShowActiveEntries.UseVisualStyleBackColor = false;
            // 
            // dgvCustomerRelationshipDTOList
            // 
            this.dgvCustomerRelationshipDTOList.AllowUserToAddRows = false;
            this.dgvCustomerRelationshipDTOList.AllowUserToDeleteRows = false;
            this.dgvCustomerRelationshipDTOList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvCustomerRelationshipDTOList.AutoGenerateColumns = false;
            this.dgvCustomerRelationshipDTOList.BackgroundColor = System.Drawing.Color.SlateGray;
            this.dgvCustomerRelationshipDTOList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCustomerRelationshipDTOList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.viewCustomerDetailDataGridViewButtonColumn,
            this.idDataGridViewTextBoxColumn,
            this.customerNameDataGridViewTextBoxColumn,
            this.relatedCustomerNameDataGridViewTextBoxColumn,
            this.customerRelationshipTypeIdDataGridViewComboBoxColumn,
            this.effectiveDateDataGridViewTextBoxColumn,
            this.expiryDateDataGridViewTextBoxColumn,
            this.isActiveDataGridViewCheckBoxColumn});
            this.dgvCustomerRelationshipDTOList.DataSource = this.customerRelationshipDTOListBS;
            this.dgvCustomerRelationshipDTOList.Location = new System.Drawing.Point(6, 19);
            this.dgvCustomerRelationshipDTOList.Name = "dgvCustomerRelationshipDTOList";
            this.dgvCustomerRelationshipDTOList.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Arial", 15F);
            this.dgvCustomerRelationshipDTOList.RowTemplate.Height = 40;
            this.dgvCustomerRelationshipDTOList.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvCustomerRelationshipDTOList.Size = new System.Drawing.Size(858, 479);
            this.dgvCustomerRelationshipDTOList.TabIndex = 33;
            this.dgvCustomerRelationshipDTOList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCustomerRelationshipDTOList_CellContentClick);
            this.dgvCustomerRelationshipDTOList.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvCustomerRelationshipDTOList_DataError);
            // 
            // viewCustomerDetailDataGridViewButtonColumn
            // 
            this.viewCustomerDetailDataGridViewButtonColumn.HeaderText = "Details";
            this.viewCustomerDetailDataGridViewButtonColumn.Name = "viewCustomerDetailDataGridViewButtonColumn";
            this.viewCustomerDetailDataGridViewButtonColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.viewCustomerDetailDataGridViewButtonColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // idDataGridViewTextBoxColumn
            // 
            this.idDataGridViewTextBoxColumn.DataPropertyName = "Id";
            this.idDataGridViewTextBoxColumn.HeaderText = "Id";
            this.idDataGridViewTextBoxColumn.Name = "idDataGridViewTextBoxColumn";
            this.idDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.BackColor = System.Drawing.Color.Transparent;
            this.btnSave.BackgroundImage = global::Semnox.Parafait.Customer.Properties.Resources.customer_button_normal;
            this.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(3, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(150, 40);
            this.btnSave.TabIndex = 37;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.BackgroundImage = global::Semnox.Parafait.Customer.Properties.Resources.customer_button_normal;
            this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(627, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(150, 40);
            this.btnClose.TabIndex = 36;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.BackgroundImage = global::Semnox.Parafait.Customer.Properties.Resources.customer_button_normal;
            this.btnDelete.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnDelete.FlatAppearance.BorderSize = 0;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnDelete.ForeColor = System.Drawing.Color.White;
            this.btnDelete.Location = new System.Drawing.Point(471, 3);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(150, 40);
            this.btnDelete.TabIndex = 35;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRefresh.BackgroundImage = global::Semnox.Parafait.Customer.Properties.Resources.customer_button_normal;
            this.btnRefresh.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnRefresh.FlatAppearance.BorderSize = 0;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.Location = new System.Drawing.Point(315, 3);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(150, 40);
            this.btnRefresh.TabIndex = 34;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // grpCustomerRelationships
            // 
            this.grpCustomerRelationships.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpCustomerRelationships.Controls.Add(this.horizontalScrollBarView1);
            this.grpCustomerRelationships.Controls.Add(this.verticalScrollBarView1);
            this.grpCustomerRelationships.Controls.Add(this.dgvCustomerRelationshipDTOList);
            this.grpCustomerRelationships.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.grpCustomerRelationships.Location = new System.Drawing.Point(10, 78);
            this.grpCustomerRelationships.Name = "grpCustomerRelationships";
            this.grpCustomerRelationships.Size = new System.Drawing.Size(914, 557);
            this.grpCustomerRelationships.TabIndex = 51;
            this.grpCustomerRelationships.TabStop = false;
            this.grpCustomerRelationships.Text = "Relationships";
            // 
            // horizontalScrollBarView1
            // 
            this.horizontalScrollBarView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.horizontalScrollBarView1.AutoHide = false;
            this.horizontalScrollBarView1.DataGridView = this.dgvCustomerRelationshipDTOList;
            this.horizontalScrollBarView1.LeftButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("horizontalScrollBarView1.LeftButtonBackgroundImage")));
            this.horizontalScrollBarView1.LeftButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("horizontalScrollBarView1.LeftButtonDisabledBackgroundImage")));
            this.horizontalScrollBarView1.Location = new System.Drawing.Point(6, 506);
            this.horizontalScrollBarView1.Margin = new System.Windows.Forms.Padding(0);
            this.horizontalScrollBarView1.Name = "horizontalScrollBarView1";
            this.horizontalScrollBarView1.RightButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("horizontalScrollBarView1.RightButtonBackgroundImage")));
            this.horizontalScrollBarView1.RightButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("horizontalScrollBarView1.RightButtonDisabledBackgroundImage")));
            this.horizontalScrollBarView1.ScrollableControl = null;
            this.horizontalScrollBarView1.ScrollViewer = null;
            this.horizontalScrollBarView1.Size = new System.Drawing.Size(858, 40);
            this.horizontalScrollBarView1.TabIndex = 35;
            // 
            // verticalScrollBarView1
            // 
            this.verticalScrollBarView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.verticalScrollBarView1.AutoHide = false;
            this.verticalScrollBarView1.DataGridView = this.dgvCustomerRelationshipDTOList;
            this.verticalScrollBarView1.DownButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("verticalScrollBarView1.DownButtonBackgroundImage")));
            this.verticalScrollBarView1.DownButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("verticalScrollBarView1.DownButtonDisabledBackgroundImage")));
            this.verticalScrollBarView1.Location = new System.Drawing.Point(871, 19);
            this.verticalScrollBarView1.Margin = new System.Windows.Forms.Padding(0);
            this.verticalScrollBarView1.Name = "verticalScrollBarView1";
            this.verticalScrollBarView1.ScrollableControl = null;
            this.verticalScrollBarView1.ScrollViewer = null;
            this.verticalScrollBarView1.Size = new System.Drawing.Size(40, 479);
            this.verticalScrollBarView1.TabIndex = 34;
            this.verticalScrollBarView1.UpButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("verticalScrollBarView1.UpButtonBackgroundImage")));
            this.verticalScrollBarView1.UpButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("verticalScrollBarView1.UpButtonDisabledBackgroundImage")));
            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(13, 638);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 13);
            this.lblStatus.TabIndex = 52;
            // 
            // btnNew
            // 
            this.btnNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnNew.BackColor = System.Drawing.SystemColors.Control;
            this.btnNew.BackgroundImage = global::Semnox.Parafait.Customer.Properties.Resources.customer_button_normal;
            this.btnNew.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnNew.FlatAppearance.BorderSize = 0;
            this.btnNew.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNew.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnNew.ForeColor = System.Drawing.Color.White;
            this.btnNew.Location = new System.Drawing.Point(159, 3);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(150, 40);
            this.btnNew.TabIndex = 54;
            this.btnNew.Text = "New";
            this.btnNew.UseVisualStyleBackColor = false;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.Controls.Add(this.btnSave);
            this.flowLayoutPanel1.Controls.Add(this.btnNew);
            this.flowLayoutPanel1.Controls.Add(this.btnRefresh);
            this.flowLayoutPanel1.Controls.Add(this.btnDelete);
            this.flowLayoutPanel1.Controls.Add(this.btnClose);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(6, 650);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(780, 46);
            this.flowLayoutPanel1.TabIndex = 55;
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
            // 
            // expiryDateDataGridViewTextBoxColumn
            // 
            this.expiryDateDataGridViewTextBoxColumn.DataPropertyName = "ExpiryDate";
            this.expiryDateDataGridViewTextBoxColumn.HeaderText = "Expiry Date";
            this.expiryDateDataGridViewTextBoxColumn.Name = "expiryDateDataGridViewTextBoxColumn";
            // 
            // isActiveDataGridViewCheckBoxColumn
            // 
            this.isActiveDataGridViewCheckBoxColumn.DataPropertyName = "IsActive";
            this.isActiveDataGridViewCheckBoxColumn.HeaderText = "Active?";
            this.isActiveDataGridViewCheckBoxColumn.Name = "isActiveDataGridViewCheckBoxColumn";
            // 
            // customerRelationshipDTOListBS
            // 
            this.customerRelationshipDTOListBS.DataSource = typeof(Semnox.Parafait.Customer.CustomerRelationshipDTO);
            // 
            // CustomerRelationshipListUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SlateGray;
            this.ClientSize = new System.Drawing.Size(934, 701);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.grpCustomerRelationships);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.groupBox1);
            this.Name = "CustomerRelationshipListUI";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Customer Relationship";
            this.Load += new System.EventHandler(this.CustomerRelationshipListUI_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCustomerRelationshipDTOList)).EndInit();
            this.grpCustomerRelationships.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.customerRelationshipDTOListBS)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cmbSearchCustomerRelationshipType;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label lblName;
        private CustomCheckBox chbShowActiveEntries;
        private System.Windows.Forms.DataGridView dgvCustomerRelationshipDTOList;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.BindingSource customerRelationshipDTOListBS;
        private System.Windows.Forms.GroupBox grpCustomerRelationships;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Button btnNew;
        private HorizontalScrollBarView horizontalScrollBarView1;
        private VerticalScrollBarView verticalScrollBarView1;
        private System.Windows.Forms.DataGridViewButtonColumn viewCustomerDetailDataGridViewButtonColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn customerNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn relatedCustomerNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn customerRelationshipTypeIdDataGridViewComboBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn effectiveDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn expiryDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isActiveDataGridViewCheckBoxColumn;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    }
}