namespace Semnox.Parafait.DigitalSignage
{
    partial class ThemeListUI
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
            this.cbThemeType = new System.Windows.Forms.ComboBox();
            this.lblEventType = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.chbShowActiveEntries = new System.Windows.Forms.CheckBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dgvThemeDTOList = new System.Windows.Forms.DataGridView();
            this.idDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.typeIdDataGridViewComboBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.initialScreenIdDataGridViewComboBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.themeNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isActiveDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.themeDTOListBS = new System.Windows.Forms.BindingSource(this.components);
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.dgvScreenTransitionsDTOList = new System.Windows.Forms.DataGridView();
            this.dgvScreenTransitionsDTOListIdTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvScreenTransitionsDTOListFromScreenIdComboBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dgvScreenTransitionsDTOListEventIdComboBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dgvScreenTransitionsDTOListToScreenIdComboBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dgvScreenTransitionsDTOListIsActiveCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.screenTransitionsDTOListBS = new System.Windows.Forms.BindingSource(this.components);
            this.lnkHQPublish = new System.Windows.Forms.LinkLabel();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvThemeDTOList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.themeDTOListBS)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvScreenTransitionsDTOList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.screenTransitionsDTOListBS)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbThemeType);
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
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Filter";
            // 
            // cbThemeType
            // 
            this.cbThemeType.FormattingEnabled = true;
            this.cbThemeType.Location = new System.Drawing.Point(487, 18);
            this.cbThemeType.Name = "cbThemeType";
            this.cbThemeType.Size = new System.Drawing.Size(121, 23);
            this.cbThemeType.TabIndex = 6;
            // 
            // lblEventType
            // 
            this.lblEventType.AutoSize = true;
            this.lblEventType.Location = new System.Drawing.Point(403, 21);
            this.lblEventType.Name = "lblEventType";
            this.lblEventType.Size = new System.Drawing.Size(81, 15);
            this.lblEventType.TabIndex = 5;
            this.lblEventType.Text = "Theme Type :";
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnSearch.Location = new System.Drawing.Point(653, 18);
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
            this.txtName.Location = new System.Drawing.Point(238, 18);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(136, 21);
            this.txtName.TabIndex = 3;
            // 
            // lblName
            // 
            this.lblName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblName.Location = new System.Drawing.Point(136, 18);
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
            this.btnSave.Location = new System.Drawing.Point(13, 442);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 26);
            this.btnSave.TabIndex = 23;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnClose.Location = new System.Drawing.Point(343, 442);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 26);
            this.btnClose.TabIndex = 22;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnDelete.Location = new System.Drawing.Point(233, 442);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 26);
            this.btnDelete.TabIndex = 21;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRefresh.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.Location = new System.Drawing.Point(123, 442);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 26);
            this.btnRefresh.TabIndex = 20;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox3, 0, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(10, 63);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(964, 373);
            this.tableLayoutPanel1.TabIndex = 24;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.dgvThemeDTOList);
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(958, 180);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Theme List";
            // 
            // dgvThemeDTOList
            // 
            this.dgvThemeDTOList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvThemeDTOList.AutoGenerateColumns = false;
            this.dgvThemeDTOList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvThemeDTOList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idDataGridViewTextBoxColumn,
            this.nameDataGridViewTextBoxColumn,
            this.descriptionDataGridViewTextBoxColumn,
            this.typeIdDataGridViewComboBoxColumn,
            this.initialScreenIdDataGridViewComboBoxColumn,
            this.themeNumberDataGridViewTextBoxColumn,
            this.isActiveDataGridViewCheckBoxColumn});
            this.dgvThemeDTOList.DataSource = this.themeDTOListBS;
            this.dgvThemeDTOList.Location = new System.Drawing.Point(6, 19);
            this.dgvThemeDTOList.Name = "dgvThemeDTOList";
            this.dgvThemeDTOList.Size = new System.Drawing.Size(946, 155);
            this.dgvThemeDTOList.TabIndex = 0;
            this.dgvThemeDTOList.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvThemeDTOList_DataError);
            // 
            // idDataGridViewTextBoxColumn
            // 
            this.idDataGridViewTextBoxColumn.DataPropertyName = "Id";
            this.idDataGridViewTextBoxColumn.HeaderText = "SI#";
            this.idDataGridViewTextBoxColumn.Name = "idDataGridViewTextBoxColumn";
            this.idDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "Theme Name";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            // 
            // descriptionDataGridViewTextBoxColumn
            // 
            this.descriptionDataGridViewTextBoxColumn.DataPropertyName = "Description";
            this.descriptionDataGridViewTextBoxColumn.HeaderText = "Description";
            this.descriptionDataGridViewTextBoxColumn.Name = "descriptionDataGridViewTextBoxColumn";
            // 
            // typeIdDataGridViewComboBoxColumn
            // 
            this.typeIdDataGridViewComboBoxColumn.DataPropertyName = "TypeId";
            this.typeIdDataGridViewComboBoxColumn.HeaderText = "Theme Type";
            this.typeIdDataGridViewComboBoxColumn.Name = "typeIdDataGridViewComboBoxColumn";
            this.typeIdDataGridViewComboBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.typeIdDataGridViewComboBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // initialScreenIdDataGridViewComboBoxColumn
            // 
            this.initialScreenIdDataGridViewComboBoxColumn.DataPropertyName = "InitialScreenId";
            this.initialScreenIdDataGridViewComboBoxColumn.HeaderText = "Initial Screen";
            this.initialScreenIdDataGridViewComboBoxColumn.Name = "initialScreenIdDataGridViewComboBoxColumn";
            this.initialScreenIdDataGridViewComboBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.initialScreenIdDataGridViewComboBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // themeNumberDataGridViewTextBoxColumn
            // 
            this.themeNumberDataGridViewTextBoxColumn.DataPropertyName = "ThemeNumber";
            this.themeNumberDataGridViewTextBoxColumn.HeaderText = "Theme Number";
            this.themeNumberDataGridViewTextBoxColumn.Name = "themeNumberDataGridViewTextBoxColumn";
            // 
            // isActiveDataGridViewCheckBoxColumn
            // 
            this.isActiveDataGridViewCheckBoxColumn.DataPropertyName = "IsActive";
            this.isActiveDataGridViewCheckBoxColumn.HeaderText = "Active";
            this.isActiveDataGridViewCheckBoxColumn.Name = "isActiveDataGridViewCheckBoxColumn";
            // 
            // themeDTOListBS
            // 
            this.themeDTOListBS.DataSource = typeof(Semnox.Parafait.DigitalSignage.ThemeDTO);
            this.themeDTOListBS.CurrentChanged += new System.EventHandler(this.themeListBS_CurrentItemChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.dgvScreenTransitionsDTOList);
            this.groupBox3.Location = new System.Drawing.Point(3, 189);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(958, 181);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Screen Transitions";
            // 
            // dgvScreenTransitionsDTOList
            // 
            this.dgvScreenTransitionsDTOList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvScreenTransitionsDTOList.AutoGenerateColumns = false;
            this.dgvScreenTransitionsDTOList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvScreenTransitionsDTOList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvScreenTransitionsDTOListIdTextBoxColumn,
            this.dgvScreenTransitionsDTOListFromScreenIdComboBoxColumn,
            this.dgvScreenTransitionsDTOListEventIdComboBoxColumn,
            this.dgvScreenTransitionsDTOListToScreenIdComboBoxColumn,
            this.dgvScreenTransitionsDTOListIsActiveCheckBoxColumn});
            this.dgvScreenTransitionsDTOList.DataSource = this.screenTransitionsDTOListBS;
            this.dgvScreenTransitionsDTOList.Location = new System.Drawing.Point(6, 19);
            this.dgvScreenTransitionsDTOList.Name = "dgvScreenTransitionsDTOList";
            this.dgvScreenTransitionsDTOList.Size = new System.Drawing.Size(946, 156);
            this.dgvScreenTransitionsDTOList.TabIndex = 0;
            this.dgvScreenTransitionsDTOList.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvScreenTransitionsDTOList_DataError);
            // 
            // dgvScreenTransitionsDTOListIdTextBoxColumn
            // 
            this.dgvScreenTransitionsDTOListIdTextBoxColumn.DataPropertyName = "Id";
            this.dgvScreenTransitionsDTOListIdTextBoxColumn.HeaderText = "SI#";
            this.dgvScreenTransitionsDTOListIdTextBoxColumn.Name = "dgvScreenTransitionsDTOListIdTextBoxColumn";
            this.dgvScreenTransitionsDTOListIdTextBoxColumn.ReadOnly = true;
            // 
            // dgvScreenTransitionsDTOListFromScreenIdComboBoxColumn
            // 
            this.dgvScreenTransitionsDTOListFromScreenIdComboBoxColumn.DataPropertyName = "FromScreenId";
            this.dgvScreenTransitionsDTOListFromScreenIdComboBoxColumn.HeaderText = "From Screen";
            this.dgvScreenTransitionsDTOListFromScreenIdComboBoxColumn.Name = "dgvScreenTransitionsDTOListFromScreenIdComboBoxColumn";
            this.dgvScreenTransitionsDTOListFromScreenIdComboBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvScreenTransitionsDTOListFromScreenIdComboBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // dgvScreenTransitionsDTOListEventIdComboBoxColumn
            // 
            this.dgvScreenTransitionsDTOListEventIdComboBoxColumn.DataPropertyName = "EventId";
            this.dgvScreenTransitionsDTOListEventIdComboBoxColumn.HeaderText = "Event";
            this.dgvScreenTransitionsDTOListEventIdComboBoxColumn.Name = "dgvScreenTransitionsDTOListEventIdComboBoxColumn";
            this.dgvScreenTransitionsDTOListEventIdComboBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvScreenTransitionsDTOListEventIdComboBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // dgvScreenTransitionsDTOListToScreenIdComboBoxColumn
            // 
            this.dgvScreenTransitionsDTOListToScreenIdComboBoxColumn.DataPropertyName = "ToScreenId";
            this.dgvScreenTransitionsDTOListToScreenIdComboBoxColumn.HeaderText = "To Screen";
            this.dgvScreenTransitionsDTOListToScreenIdComboBoxColumn.Name = "dgvScreenTransitionsDTOListToScreenIdComboBoxColumn";
            this.dgvScreenTransitionsDTOListToScreenIdComboBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvScreenTransitionsDTOListToScreenIdComboBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // dgvScreenTransitionsDTOListIsActiveCheckBoxColumn
            // 
            this.dgvScreenTransitionsDTOListIsActiveCheckBoxColumn.DataPropertyName = "IsActive";
            this.dgvScreenTransitionsDTOListIsActiveCheckBoxColumn.HeaderText = "Active";
            this.dgvScreenTransitionsDTOListIsActiveCheckBoxColumn.Name = "dgvScreenTransitionsDTOListIsActiveCheckBoxColumn";
            this.dgvScreenTransitionsDTOListIsActiveCheckBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvScreenTransitionsDTOListIsActiveCheckBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // screenTransitionsDTOListBS
            // 
            this.screenTransitionsDTOListBS.DataSource = typeof(Semnox.Parafait.DigitalSignage.ScreenTransitionsDTO);
            // 
            // lnkHQPublish
            // 
            this.lnkHQPublish.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lnkHQPublish.AutoSize = true;
            this.lnkHQPublish.BackColor = System.Drawing.Color.Blue;
            this.lnkHQPublish.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkHQPublish.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lnkHQPublish.LinkColor = System.Drawing.Color.White;
            this.lnkHQPublish.Location = new System.Drawing.Point(877, 456);
            this.lnkHQPublish.Name = "lnkHQPublish";
            this.lnkHQPublish.Size = new System.Drawing.Size(97, 15);
            this.lnkHQPublish.TabIndex = 41;
            this.lnkHQPublish.TabStop = true;
            this.lnkHQPublish.Text = "Publish To Sites";
            this.lnkHQPublish.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkHQPublish_LinkClicked);
            // 
            // ThemeListUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 481);
            this.Controls.Add(this.lnkHQPublish);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnRefresh);
            this.Name = "ThemeListUI";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Screen Transitions";
            this.Load += new System.EventHandler(this.ThemeListUI_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvThemeDTOList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.themeDTOListBS)).EndInit();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvScreenTransitionsDTOList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.screenTransitionsDTOListBS)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cbThemeType;
        private System.Windows.Forms.Label lblEventType;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.CheckBox chbShowActiveEntries;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.DataGridView dgvThemeDTOList;
        private System.Windows.Forms.DataGridView dgvScreenTransitionsDTOList;
        private System.Windows.Forms.BindingSource themeDTOListBS;
        private System.Windows.Forms.BindingSource screenTransitionsDTOListBS;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvScreenTransitionsDTOListIdTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn dgvScreenTransitionsDTOListFromScreenIdComboBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn dgvScreenTransitionsDTOListEventIdComboBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn dgvScreenTransitionsDTOListToScreenIdComboBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dgvScreenTransitionsDTOListIsActiveCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn typeIdDataGridViewComboBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn initialScreenIdDataGridViewComboBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn themeNumberDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isActiveDataGridViewCheckBoxColumn;
        private System.Windows.Forms.LinkLabel lnkHQPublish;
    }
}