namespace Semnox.Parafait.DigitalSignage
{
    partial class ScreenSetupUI
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
            this.gpFilter = new System.Windows.Forms.GroupBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.chbShowActiveEntries = new System.Windows.Forms.CheckBox();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnRefersh = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.dgvScreenSetupGrid = new System.Windows.Forms.DataGridView();
            this.screenIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.alignmentDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.scrDivHorizontalDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.scrDivVerticalDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isActiveDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.AddUpdateZones = new System.Windows.Forms.DataGridViewButtonColumn();
            this.ContentMap = new System.Windows.Forms.DataGridViewButtonColumn();
            this.screenSetupDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.gpFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvScreenSetupGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.screenSetupDTOBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // gpFilter
            // 
            this.gpFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gpFilter.Controls.Add(this.btnSearch);
            this.gpFilter.Controls.Add(this.txtName);
            this.gpFilter.Controls.Add(this.lblName);
            this.gpFilter.Controls.Add(this.chbShowActiveEntries);
            this.gpFilter.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.gpFilter.Location = new System.Drawing.Point(12, 10);
            this.gpFilter.Name = "gpFilter";
            this.gpFilter.Size = new System.Drawing.Size(743, 47);
            this.gpFilter.TabIndex = 19;
            this.gpFilter.TabStop = false;
            this.gpFilter.Text = "Filter";
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnSearch.Location = new System.Drawing.Point(395, 16);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 3;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtName
            // 
            this.txtName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.txtName.Location = new System.Drawing.Point(253, 17);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(136, 21);
            this.txtName.TabIndex = 2;
            // 
            // lblName
            // 
            this.lblName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblName.Location = new System.Drawing.Point(151, 17);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(96, 20);
            this.lblName.TabIndex = 1;
            this.lblName.Text = "Name";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chbShowActiveEntries
            // 
            this.chbShowActiveEntries.AutoSize = true;
            this.chbShowActiveEntries.Checked = true;
            this.chbShowActiveEntries.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbShowActiveEntries.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.chbShowActiveEntries.Location = new System.Drawing.Point(12, 18);
            this.chbShowActiveEntries.Name = "chbShowActiveEntries";
            this.chbShowActiveEntries.Size = new System.Drawing.Size(139, 19);
            this.chbShowActiveEntries.TabIndex = 0;
            this.chbShowActiveEntries.Text = "Show Active Entries";
            this.chbShowActiveEntries.UseVisualStyleBackColor = true;
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnDelete.Location = new System.Drawing.Point(269, 280);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 18;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnClose.Location = new System.Drawing.Point(397, 280);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 17;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnRefersh
            // 
            this.btnRefersh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRefersh.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnRefersh.Location = new System.Drawing.Point(141, 280);
            this.btnRefersh.Name = "btnRefersh";
            this.btnRefersh.Size = new System.Drawing.Size(75, 23);
            this.btnRefersh.TabIndex = 16;
            this.btnRefersh.Text = "Refresh";
            this.btnRefersh.UseVisualStyleBackColor = true;
            this.btnRefersh.Click += new System.EventHandler(this.btnRefersh_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnSave.Location = new System.Drawing.Point(13, 280);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 15;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // dgvScreenSetupGrid
            // 
            this.dgvScreenSetupGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvScreenSetupGrid.AutoGenerateColumns = false;
            this.dgvScreenSetupGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvScreenSetupGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.screenIdDataGridViewTextBoxColumn,
            this.nameDataGridViewTextBoxColumn,
            this.alignmentDataGridViewTextBoxColumn,
            this.scrDivHorizontalDataGridViewTextBoxColumn,
            this.scrDivVerticalDataGridViewTextBoxColumn,
            this.descriptionDataGridViewTextBoxColumn,
            this.isActiveDataGridViewTextBoxColumn,
            this.AddUpdateZones,
            this.ContentMap});
            this.dgvScreenSetupGrid.DataSource = this.screenSetupDTOBindingSource;
            this.dgvScreenSetupGrid.Location = new System.Drawing.Point(12, 63);
            this.dgvScreenSetupGrid.Name = "dgvScreenSetupGrid";
            this.dgvScreenSetupGrid.Size = new System.Drawing.Size(743, 201);
            this.dgvScreenSetupGrid.TabIndex = 14;
            this.dgvScreenSetupGrid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvScreenSetupGrid_CellContentClick);
            this.dgvScreenSetupGrid.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvScreenSetupGrid_ComboDataError);
            // 
            // screenIdDataGridViewTextBoxColumn
            // 
            this.screenIdDataGridViewTextBoxColumn.DataPropertyName = "ScreenId";
            this.screenIdDataGridViewTextBoxColumn.HeaderText = "ID";
            this.screenIdDataGridViewTextBoxColumn.Name = "screenIdDataGridViewTextBoxColumn";
            this.screenIdDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "Screen Name";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            // 
            // alignmentDataGridViewTextBoxColumn
            // 
            this.alignmentDataGridViewTextBoxColumn.DataPropertyName = "Alignment";
            this.alignmentDataGridViewTextBoxColumn.HeaderText = "Alignment";
            this.alignmentDataGridViewTextBoxColumn.Name = "alignmentDataGridViewTextBoxColumn";
            this.alignmentDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.alignmentDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // scrDivHorizontalDataGridViewTextBoxColumn
            // 
            this.scrDivHorizontalDataGridViewTextBoxColumn.DataPropertyName = "ScrDivHorizontal";
            this.scrDivHorizontalDataGridViewTextBoxColumn.HeaderText = "Rows";
            this.scrDivHorizontalDataGridViewTextBoxColumn.Name = "scrDivHorizontalDataGridViewTextBoxColumn";
            // 
            // scrDivVerticalDataGridViewTextBoxColumn
            // 
            this.scrDivVerticalDataGridViewTextBoxColumn.DataPropertyName = "ScrDivVertical";
            this.scrDivVerticalDataGridViewTextBoxColumn.HeaderText = "Columns";
            this.scrDivVerticalDataGridViewTextBoxColumn.Name = "scrDivVerticalDataGridViewTextBoxColumn";
            // 
            // descriptionDataGridViewTextBoxColumn
            // 
            this.descriptionDataGridViewTextBoxColumn.DataPropertyName = "Description";
            this.descriptionDataGridViewTextBoxColumn.HeaderText = "Description";
            this.descriptionDataGridViewTextBoxColumn.Name = "descriptionDataGridViewTextBoxColumn";
            // 
            // isActiveDataGridViewTextBoxColumn
            // 
            this.isActiveDataGridViewTextBoxColumn.DataPropertyName = "IsActive";
            this.isActiveDataGridViewTextBoxColumn.HeaderText = "Active?";
            this.isActiveDataGridViewTextBoxColumn.Name = "isActiveDataGridViewTextBoxColumn";
            this.isActiveDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.isActiveDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // AddUpdateZones
            // 
            this.AddUpdateZones.HeaderText = "Add/Update Zones";
            this.AddUpdateZones.Name = "AddUpdateZones";
            this.AddUpdateZones.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.AddUpdateZones.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.AddUpdateZones.Text = "Add/Update Zones";
            this.AddUpdateZones.UseColumnTextForButtonValue = true;
            // 
            // ContentMap
            // 
            this.ContentMap.HeaderText = "Content Map";
            this.ContentMap.Name = "ContentMap";
            this.ContentMap.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ContentMap.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.ContentMap.Text = "Content Map";
            this.ContentMap.UseColumnTextForButtonValue = true;
            // 
            // screenSetupDTOBindingSource
            // 
            this.screenSetupDTOBindingSource.DataSource = typeof(ScreenSetupDTO);
            // 
            // ScreenSetupUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(767, 313);
            this.Controls.Add(this.gpFilter);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnRefersh);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.dgvScreenSetupGrid);
            this.Name = "ScreenSetupUI";
            this.Text = "Screen Designs";
            this.Load += new System.EventHandler(this.ScreenSetupUI_Load);
            this.gpFilter.ResumeLayout(false);
            this.gpFilter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvScreenSetupGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.screenSetupDTOBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gpFilter;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.CheckBox chbShowActiveEntries;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnRefersh;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.DataGridView dgvScreenSetupGrid;
        private System.Windows.Forms.BindingSource screenSetupDTOBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn screenIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn alignmentDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn scrDivHorizontalDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn scrDivVerticalDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isActiveDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewButtonColumn AddUpdateZones;
        private System.Windows.Forms.DataGridViewButtonColumn ContentMap;
    }
}

