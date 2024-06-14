namespace Semnox.Parafait.DigitalSignage
{
    partial class EventListUI
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
            if(disposing && (components != null))
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
            this.cbEventType = new System.Windows.Forms.ComboBox();
            this.lblEventType = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.chbShowActiveEntries = new System.Windows.Forms.CheckBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.dgvEventDTOList = new System.Windows.Forms.DataGridView();
            this.dgvEventDTOListIdTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvEventDTOListNameTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvEventDTOListTypeIdComboBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dgvEventDTOListParameterTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvEventDTOListDescriptionTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvEventDTOListIsActiveCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.eventDTOListBS = new System.Windows.Forms.BindingSource(this.components);
            this.lnkPublish = new System.Windows.Forms.LinkLabel();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEventDTOList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.eventDTOListBS)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbEventType);
            this.groupBox1.Controls.Add(this.lblEventType);
            this.groupBox1.Controls.Add(this.btnSearch);
            this.groupBox1.Controls.Add(this.txtName);
            this.groupBox1.Controls.Add(this.lblName);
            this.groupBox1.Controls.Add(this.chbShowActiveEntries);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(10, 10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(964, 47);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Filter";
            // 
            // cbEventType
            // 
            this.cbEventType.FormattingEnabled = true;
            this.cbEventType.Location = new System.Drawing.Point(518, 18);
            this.cbEventType.Name = "cbEventType";
            this.cbEventType.Size = new System.Drawing.Size(121, 23);
            this.cbEventType.TabIndex = 6;
            // 
            // lblEventType
            // 
            this.lblEventType.AutoSize = true;
            this.lblEventType.Location = new System.Drawing.Point(439, 21);
            this.lblEventType.Name = "lblEventType";
            this.lblEventType.Size = new System.Drawing.Size(73, 15);
            this.lblEventType.TabIndex = 5;
            this.lblEventType.Text = "Event Type :";
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnSearch.Location = new System.Drawing.Point(684, 18);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 4;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtName
            // 
            this.txtName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.txtName.Location = new System.Drawing.Point(269, 18);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(136, 21);
            this.txtName.TabIndex = 3;
            // 
            // lblName
            // 
            this.lblName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblName.Location = new System.Drawing.Point(167, 18);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(96, 20);
            this.lblName.TabIndex = 2;
            this.lblName.Text = "Name :";
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
            this.btnSave.TabIndex = 23;
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
            this.btnClose.TabIndex = 22;
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
            this.btnDelete.TabIndex = 21;
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
            this.btnRefresh.TabIndex = 20;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // dgvEventDTOList
            // 
            this.dgvEventDTOList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvEventDTOList.AutoGenerateColumns = false;
            this.dgvEventDTOList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvEventDTOList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvEventDTOListIdTextBoxColumn,
            this.dgvEventDTOListNameTextBoxColumn,
            this.dgvEventDTOListTypeIdComboBoxColumn,
            this.dgvEventDTOListParameterTextBoxColumn,
            this.dgvEventDTOListDescriptionTextBoxColumn,
            this.dgvEventDTOListIsActiveCheckBoxColumn});
            this.dgvEventDTOList.DataSource = this.eventDTOListBS;
            this.dgvEventDTOList.Location = new System.Drawing.Point(10, 65);
            this.dgvEventDTOList.Margin = new System.Windows.Forms.Padding(5);
            this.dgvEventDTOList.Name = "dgvEventDTOList";
            this.dgvEventDTOList.Size = new System.Drawing.Size(964, 372);
            this.dgvEventDTOList.TabIndex = 24;
            this.dgvEventDTOList.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvEventDTOList_CellDoubleClick);
            this.dgvEventDTOList.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvEventDTOList_DataError);
            // 
            // dgvEventDTOListIdTextBoxColumn
            // 
            this.dgvEventDTOListIdTextBoxColumn.DataPropertyName = "Id";
            this.dgvEventDTOListIdTextBoxColumn.HeaderText = "SI#";
            this.dgvEventDTOListIdTextBoxColumn.Name = "dgvEventDTOListIdTextBoxColumn";
            this.dgvEventDTOListIdTextBoxColumn.ReadOnly = true;
            // 
            // dgvEventDTOListNameTextBoxColumn
            // 
            this.dgvEventDTOListNameTextBoxColumn.DataPropertyName = "Name";
            this.dgvEventDTOListNameTextBoxColumn.HeaderText = "Name";
            this.dgvEventDTOListNameTextBoxColumn.Name = "dgvEventDTOListNameTextBoxColumn";
            // 
            // dgvEventDTOListTypeIdComboBoxColumn
            // 
            this.dgvEventDTOListTypeIdComboBoxColumn.DataPropertyName = "TypeId";
            this.dgvEventDTOListTypeIdComboBoxColumn.HeaderText = "Event Type";
            this.dgvEventDTOListTypeIdComboBoxColumn.Name = "dgvEventDTOListTypeIdComboBoxColumn";
            this.dgvEventDTOListTypeIdComboBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvEventDTOListTypeIdComboBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // dgvEventDTOListParameterTextBoxColumn
            // 
            this.dgvEventDTOListParameterTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dgvEventDTOListParameterTextBoxColumn.DataPropertyName = "Parameter";
            this.dgvEventDTOListParameterTextBoxColumn.HeaderText = "Parameter";
            this.dgvEventDTOListParameterTextBoxColumn.Name = "dgvEventDTOListParameterTextBoxColumn";
            // 
            // dgvEventDTOListDescriptionTextBoxColumn
            // 
            this.dgvEventDTOListDescriptionTextBoxColumn.DataPropertyName = "Description";
            this.dgvEventDTOListDescriptionTextBoxColumn.HeaderText = "Description";
            this.dgvEventDTOListDescriptionTextBoxColumn.Name = "dgvEventDTOListDescriptionTextBoxColumn";
            // 
            // dgvEventDTOListIsActiveCheckBoxColumn
            // 
            this.dgvEventDTOListIsActiveCheckBoxColumn.DataPropertyName = "IsActive";
            this.dgvEventDTOListIsActiveCheckBoxColumn.HeaderText = "Active";
            this.dgvEventDTOListIsActiveCheckBoxColumn.Name = "dgvEventDTOListIsActiveCheckBoxColumn";
            this.dgvEventDTOListIsActiveCheckBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvEventDTOListIsActiveCheckBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // eventDTOListBS
            // 
            this.eventDTOListBS.DataSource = typeof(Semnox.Parafait.DigitalSignage.EventDTO);
            // 
            // lnkPublish
            // 
            this.lnkPublish.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lnkPublish.AutoSize = true;
            this.lnkPublish.BackColor = System.Drawing.Color.Blue;
            this.lnkPublish.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkPublish.ForeColor = System.Drawing.Color.White;
            this.lnkPublish.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lnkPublish.LinkColor = System.Drawing.Color.White;
            this.lnkPublish.Location = new System.Drawing.Point(878, 449);
            this.lnkPublish.Name = "lnkPublish";
            this.lnkPublish.Size = new System.Drawing.Size(96, 15);
            this.lnkPublish.TabIndex = 40;
            this.lnkPublish.TabStop = true;
            this.lnkPublish.Text = "Publish To Sites";
            this.lnkPublish.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkPublish_LinkClicked);
            // 
            // EventListUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 481);
            this.Controls.Add(this.lnkPublish);
            this.Controls.Add(this.dgvEventDTOList);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.groupBox1);
            this.Name = "EventListUI";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Text = "Event Definition";
            this.Load += new System.EventHandler(this.EventListUI_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEventDTOList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.eventDTOListBS)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.CheckBox chbShowActiveEntries;
        private System.Windows.Forms.Label lblEventType;
        private System.Windows.Forms.ComboBox cbEventType;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.BindingSource eventDTOListBS;
        private System.Windows.Forms.DataGridView dgvEventDTOList;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvEventDTOListIdTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvEventDTOListNameTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn dgvEventDTOListTypeIdComboBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvEventDTOListParameterTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvEventDTOListDescriptionTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dgvEventDTOListIsActiveCheckBoxColumn;
        private System.Windows.Forms.LinkLabel lnkPublish;
    }
}