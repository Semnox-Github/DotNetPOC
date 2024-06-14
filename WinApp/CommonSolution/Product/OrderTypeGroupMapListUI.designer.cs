namespace Semnox.Parafait.Product
{
    partial class OrderTypeGroupMapListUI
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmbOrderTypeGroup = new System.Windows.Forms.ComboBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.lblName = new System.Windows.Forms.Label();
            this.chbShowActiveEntries = new System.Windows.Forms.CheckBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.dgvOrderTypeGroupMapDTOList = new System.Windows.Forms.DataGridView();
            this.idDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.orderTypeGroupIdDataGridViewComboBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.isActiveDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.orderTypeGroupMapDTOListBS = new System.Windows.Forms.BindingSource(this.components);
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrderTypeGroupMapDTOList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.orderTypeGroupMapDTOListBS)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmbOrderTypeGroup);
            this.groupBox1.Controls.Add(this.btnSearch);
            this.groupBox1.Controls.Add(this.lblName);
            this.groupBox1.Controls.Add(this.chbShowActiveEntries);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(10, 10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(500, 47);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Filter";
            // 
            // cmbOrderTypeGroup
            // 
            this.cmbOrderTypeGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbOrderTypeGroup.FormattingEnabled = true;
            this.cmbOrderTypeGroup.Location = new System.Drawing.Point(252, 17);
            this.cmbOrderTypeGroup.Name = "cmbOrderTypeGroup";
            this.cmbOrderTypeGroup.Size = new System.Drawing.Size(121, 23);
            this.cmbOrderTypeGroup.TabIndex = 5;
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnSearch.Location = new System.Drawing.Point(409, 17);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 4;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // lblName
            // 
            this.lblName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblName.Location = new System.Drawing.Point(129, 18);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(123, 20);
            this.lblName.TabIndex = 2;
            this.lblName.Text = "Order Type Group :";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chbShowActiveEntries
            // 
            this.chbShowActiveEntries.AutoSize = true;
            this.chbShowActiveEntries.Checked = true;
            this.chbShowActiveEntries.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbShowActiveEntries.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.chbShowActiveEntries.Location = new System.Drawing.Point(6, 20);
            this.chbShowActiveEntries.Name = "chbShowActiveEntries";
            this.chbShowActiveEntries.Size = new System.Drawing.Size(124, 19);
            this.chbShowActiveEntries.TabIndex = 1;
            this.chbShowActiveEntries.Text = "Show Active Only";
            this.chbShowActiveEntries.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnSave.Location = new System.Drawing.Point(10, 445);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 31;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnClose.Location = new System.Drawing.Point(340, 445);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 30;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnDelete.Location = new System.Drawing.Point(230, 445);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 29;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRefresh.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.Location = new System.Drawing.Point(120, 445);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 28;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // dgvOrderTypeGroupMapDTOList
            // 
            this.dgvOrderTypeGroupMapDTOList.AutoGenerateColumns = false;
            this.dgvOrderTypeGroupMapDTOList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOrderTypeGroupMapDTOList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idDataGridViewTextBoxColumn,
            this.orderTypeGroupIdDataGridViewComboBoxColumn,
            this.isActiveDataGridViewCheckBoxColumn});
            this.dgvOrderTypeGroupMapDTOList.DataSource = this.orderTypeGroupMapDTOListBS;
            this.dgvOrderTypeGroupMapDTOList.Location = new System.Drawing.Point(10, 63);
            this.dgvOrderTypeGroupMapDTOList.Name = "dgvOrderTypeGroupMapDTOList";
            this.dgvOrderTypeGroupMapDTOList.Size = new System.Drawing.Size(500, 376);
            this.dgvOrderTypeGroupMapDTOList.TabIndex = 32;
            this.dgvOrderTypeGroupMapDTOList.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvOrderTypeGroupMapDTOList_DataError);
            // 
            // idDataGridViewTextBoxColumn
            // 
            this.idDataGridViewTextBoxColumn.DataPropertyName = "Id";
            this.idDataGridViewTextBoxColumn.HeaderText = "SI#";
            this.idDataGridViewTextBoxColumn.Name = "idDataGridViewTextBoxColumn";
            this.idDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // orderTypeGroupIdDataGridViewComboBoxColumn
            // 
            this.orderTypeGroupIdDataGridViewComboBoxColumn.DataPropertyName = "OrderTypeGroupId";
            this.orderTypeGroupIdDataGridViewComboBoxColumn.HeaderText = "Order Type Group";
            this.orderTypeGroupIdDataGridViewComboBoxColumn.Name = "orderTypeGroupIdDataGridViewComboBoxColumn";
            this.orderTypeGroupIdDataGridViewComboBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.orderTypeGroupIdDataGridViewComboBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // isActiveDataGridViewCheckBoxColumn
            // 
            this.isActiveDataGridViewCheckBoxColumn.DataPropertyName = "IsActive";
            this.isActiveDataGridViewCheckBoxColumn.HeaderText = "Active";
            this.isActiveDataGridViewCheckBoxColumn.Name = "isActiveDataGridViewCheckBoxColumn";
            // 
            // orderTypeGroupMapDTOListBS
            // 
            this.orderTypeGroupMapDTOListBS.DataSource = typeof(OrderTypeGroupMapDTO);
            this.orderTypeGroupMapDTOListBS.AddingNew += new System.ComponentModel.AddingNewEventHandler(this.orderTypeGroupMapDTOListBS_AddingNew);
            // 
            // OrderTypeGroupMapListUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(520, 481);
            this.Controls.Add(this.dgvOrderTypeGroupMapDTOList);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.groupBox1);
            this.Name = "OrderTypeGroupMapListUI";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Text = "Order Type Group Map";
            this.Load += new System.EventHandler(this.OrderTypeGroupMapListUI_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrderTypeGroupMapDTOList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.orderTypeGroupMapDTOListBS)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cmbOrderTypeGroup;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.CheckBox chbShowActiveEntries;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.DataGridView dgvOrderTypeGroupMapDTOList;
        private System.Windows.Forms.BindingSource orderTypeGroupMapDTOListBS;
        private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn orderTypeGroupIdDataGridViewComboBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isActiveDataGridViewCheckBoxColumn;
    }
}