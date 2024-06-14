using Semnox.Parafait.Waiver;

namespace Semnox.Parafait.Waiver
{
    partial class waiverSetUI
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
            this.dgvWaiverSet = new System.Windows.Forms.DataGridView();
            this.waiverSetIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.waiverSetNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isActiveDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.btnWaiverSignedOption = new System.Windows.Forms.DataGridViewButtonColumn();
            this.lastUpdatedByDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastUpdatedDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.waiverSetDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.btnSave = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.searchByWaiverSignedParametersBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dgvLanguage = new System.Windows.Forms.DataGridView();
            this.idDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.languageIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.elementGuidDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableObjectDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.translationDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgTxtWaiverLanguageFileLocation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnfileUpload = new System.Windows.Forms.DataGridViewButtonColumn();
            this.objectTranslationsDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.chbShowActiveEntries = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.waiverSetDetailDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.dgvWaiverDetail = new System.Windows.Forms.DataGridView();
            this.waiverSetDetailIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgTxtWaiverFileLocation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.waiverSetDetailNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.waiverSetDetailGUIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.waiverFileNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnFileImage = new System.Windows.Forms.DataGridViewButtonColumn();
            this.validForDaysDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.effectiveDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isActiveDataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.lastUpdatedByDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastUpdatedDateDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvWaiverSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.waiverSetDTOBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchByWaiverSignedParametersBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLanguage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.objectTranslationsDTOBindingSource)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.waiverSetDetailDTOBindingSource)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvWaiverDetail)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvWaiverSet
            // 
            this.dgvWaiverSet.AllowUserToDeleteRows = false;
            this.dgvWaiverSet.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvWaiverSet.AutoGenerateColumns = false;
            this.dgvWaiverSet.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvWaiverSet.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.waiverSetIdDataGridViewTextBoxColumn,
            this.waiverSetNameDataGridViewTextBoxColumn,
            this.isActiveDataGridViewCheckBoxColumn,
            this.btnWaiverSignedOption,
            this.lastUpdatedByDataGridViewTextBoxColumn,
            this.lastUpdatedDateDataGridViewTextBoxColumn});
            this.dgvWaiverSet.DataSource = this.waiverSetDTOBindingSource;
            this.dgvWaiverSet.Location = new System.Drawing.Point(8, 18);
            this.dgvWaiverSet.Name = "dgvWaiverSet";
            this.dgvWaiverSet.Size = new System.Drawing.Size(986, 165);
            this.dgvWaiverSet.TabIndex = 1;
            this.dgvWaiverSet.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvWaiverSet_CellContentClick);
            this.dgvWaiverSet.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvWaiverSet_RowEnter);
            // 
            // waiverSetIdDataGridViewTextBoxColumn
            // 
            this.waiverSetIdDataGridViewTextBoxColumn.DataPropertyName = "WaiverSetId";
            this.waiverSetIdDataGridViewTextBoxColumn.HeaderText = "Id";
            this.waiverSetIdDataGridViewTextBoxColumn.Name = "waiverSetIdDataGridViewTextBoxColumn";
            this.waiverSetIdDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // waiverSetNameDataGridViewTextBoxColumn
            // 
            this.waiverSetNameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.waiverSetNameDataGridViewTextBoxColumn.HeaderText = "Waiver Name";
            this.waiverSetNameDataGridViewTextBoxColumn.Name = "waiverSetNameDataGridViewTextBoxColumn";
            // 
            // isActiveDataGridViewCheckBoxColumn
            // 
            this.isActiveDataGridViewCheckBoxColumn.DataPropertyName = "IsActive";
            this.isActiveDataGridViewCheckBoxColumn.HeaderText = "Active?";
            this.isActiveDataGridViewCheckBoxColumn.Name = "isActiveDataGridViewCheckBoxColumn";
            // 
            // btnWaiverSignedOption
            // 
            this.btnWaiverSignedOption.HeaderText = "Signing Options";
            this.btnWaiverSignedOption.Name = "btnWaiverSignedOption";
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
            // waiverSetDTOBindingSource
            // 
            this.waiverSetDTOBindingSource.DataSource = typeof(Semnox.Parafait.Waiver.WaiverSetDTO);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.BackColor = System.Drawing.SystemColors.Control;
            this.btnSave.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnSave.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnSave.Location = new System.Drawing.Point(12, 621);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRefresh.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.Location = new System.Drawing.Point(122, 621);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 5;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnClose.Location = new System.Drawing.Point(232, 621);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "WaiverSetId";
            this.dataGridViewTextBoxColumn1.HeaderText = "WaiverSet Id";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "Name";
            this.dataGridViewTextBoxColumn2.HeaderText = "Name";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "LastUpdatedDate";
            this.dataGridViewTextBoxColumn3.HeaderText = "Last Updated Date";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.DataPropertyName = "LastUpdatedBy";
            this.dataGridViewTextBoxColumn4.HeaderText = "Last Updated By";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.DataPropertyName = "WaiverSetDetailId";
            this.dataGridViewTextBoxColumn5.HeaderText = "WaiverSetDetail Id";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.DataPropertyName = "Name";
            this.dataGridViewTextBoxColumn6.HeaderText = "Name";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.DataPropertyName = "WaiverFileName";
            this.dataGridViewTextBoxColumn7.HeaderText = "Waiver File Name";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.DataPropertyName = "ValidForDays";
            this.dataGridViewTextBoxColumn8.HeaderText = "Valid For Days";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            // 
            // dataGridViewTextBoxColumn9
            // 
            this.dataGridViewTextBoxColumn9.DataPropertyName = "EffectiveDate";
            this.dataGridViewTextBoxColumn9.HeaderText = "Effective Date";
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            // 
            // dataGridViewTextBoxColumn10
            // 
            this.dataGridViewTextBoxColumn10.DataPropertyName = "LastUpdatedDate";
            this.dataGridViewTextBoxColumn10.HeaderText = "Last Updated Date";
            this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            this.dataGridViewTextBoxColumn10.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn11
            // 
            this.dataGridViewTextBoxColumn11.DataPropertyName = "LastUpdatedBy";
            this.dataGridViewTextBoxColumn11.HeaderText = "Last Updated By";
            this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
            this.dataGridViewTextBoxColumn11.ReadOnly = true;
            // 
            // searchByWaiverSignedParametersBindingSource
            // 
            this.searchByWaiverSignedParametersBindingSource.DataSource = typeof(Semnox.Parafait.Waiver.WaiverSignatureDTO.SearchByWaiverSignatureParameters);
            // 
            // dgvLanguage
            // 
            this.dgvLanguage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvLanguage.AutoGenerateColumns = false;
            this.dgvLanguage.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLanguage.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idDataGridViewTextBoxColumn,
            this.languageIdDataGridViewTextBoxColumn,
            this.elementGuidDataGridViewTextBoxColumn,
            this.tableObjectDataGridViewTextBoxColumn,
            this.translationDataGridViewTextBoxColumn,
            this.dgTxtWaiverLanguageFileLocation,
            this.btnfileUpload});
            this.dgvLanguage.DataSource = this.objectTranslationsDTOBindingSource;
            this.dgvLanguage.Location = new System.Drawing.Point(8, 19);
            this.dgvLanguage.Name = "dgvLanguage";
            this.dgvLanguage.Size = new System.Drawing.Size(980, 134);
            this.dgvLanguage.TabIndex = 7;
            this.dgvLanguage.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvLanguage_CellContentClick);
            // 
            // idDataGridViewTextBoxColumn
            // 
            this.idDataGridViewTextBoxColumn.DataPropertyName = "Id";
            this.idDataGridViewTextBoxColumn.HeaderText = "Id";
            this.idDataGridViewTextBoxColumn.Name = "idDataGridViewTextBoxColumn";
            this.idDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // languageIdDataGridViewTextBoxColumn
            // 
            this.languageIdDataGridViewTextBoxColumn.DataPropertyName = "LanguageId";
            this.languageIdDataGridViewTextBoxColumn.HeaderText = "Language";
            this.languageIdDataGridViewTextBoxColumn.Name = "languageIdDataGridViewTextBoxColumn";
            this.languageIdDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.languageIdDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // elementGuidDataGridViewTextBoxColumn
            // 
            this.elementGuidDataGridViewTextBoxColumn.DataPropertyName = "ElementGuid";
            this.elementGuidDataGridViewTextBoxColumn.HeaderText = "ElementGuid";
            this.elementGuidDataGridViewTextBoxColumn.Name = "elementGuidDataGridViewTextBoxColumn";
            this.elementGuidDataGridViewTextBoxColumn.Visible = false;
            // 
            // tableObjectDataGridViewTextBoxColumn
            // 
            this.tableObjectDataGridViewTextBoxColumn.DataPropertyName = "TableObject";
            this.tableObjectDataGridViewTextBoxColumn.HeaderText = "Table";
            this.tableObjectDataGridViewTextBoxColumn.Name = "tableObjectDataGridViewTextBoxColumn";
            this.tableObjectDataGridViewTextBoxColumn.Visible = false;
            // 
            // translationDataGridViewTextBoxColumn
            // 
            this.translationDataGridViewTextBoxColumn.DataPropertyName = "Translation";
            this.translationDataGridViewTextBoxColumn.HeaderText = "Waiver Language File Name";
            this.translationDataGridViewTextBoxColumn.Name = "translationDataGridViewTextBoxColumn";
            this.translationDataGridViewTextBoxColumn.ReadOnly = true;
            this.translationDataGridViewTextBoxColumn.Width = 150;
            // 
            // dgTxtWaiverLanguageFileLocation
            // 
            this.dgTxtWaiverLanguageFileLocation.HeaderText = "WaiverLanguageFileLocation";
            this.dgTxtWaiverLanguageFileLocation.Name = "dgTxtWaiverLanguageFileLocation";
            this.dgTxtWaiverLanguageFileLocation.Visible = false;
            // 
            // btnfileUpload
            // 
            this.btnfileUpload.HeaderText = "     ...";
            this.btnfileUpload.Name = "btnfileUpload";
            // 
            // objectTranslationsDTOBindingSource
            // 
            this.objectTranslationsDTOBindingSource.DataSource = typeof(Semnox.Core.GenericUtilities.ObjectTranslationsDTO);
            // 
            // chbShowActiveEntries
            // 
            this.chbShowActiveEntries.AutoSize = true;
            this.chbShowActiveEntries.Checked = true;
            this.chbShowActiveEntries.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbShowActiveEntries.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.chbShowActiveEntries.Location = new System.Drawing.Point(8, 20);
            this.chbShowActiveEntries.Name = "chbShowActiveEntries";
            this.chbShowActiveEntries.Size = new System.Drawing.Size(124, 19);
            this.chbShowActiveEntries.TabIndex = 10;
            this.chbShowActiveEntries.Text = "Show Active Only";
            this.chbShowActiveEntries.UseVisualStyleBackColor = true;
            this.chbShowActiveEntries.CheckedChanged += new System.EventHandler(this.chbShowActiveEntries_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.btnSearch);
            this.groupBox1.Controls.Add(this.chbShowActiveEntries);
            this.groupBox1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(9, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(999, 48);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Filter";
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnSearch.Location = new System.Drawing.Point(189, 18);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 5;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Visible = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // waiverSetDetailDTOBindingSource
            // 
            this.waiverSetDetailDTOBindingSource.DataSource = typeof(Semnox.Parafait.Waiver.WaiverSetDetailDTO);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.dgvWaiverSet);
            this.groupBox2.Location = new System.Drawing.Point(9, 59);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(999, 190);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Waiver Set";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.dgvWaiverDetail);
            this.groupBox3.Location = new System.Drawing.Point(9, 255);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(999, 183);
            this.groupBox3.TabIndex = 13;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Waiver Set Details";
            // 
            // dgvWaiverDetail
            // 
            this.dgvWaiverDetail.AllowUserToDeleteRows = false;
            this.dgvWaiverDetail.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvWaiverDetail.AutoGenerateColumns = false;
            this.dgvWaiverDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvWaiverDetail.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.waiverSetDetailIdDataGridViewTextBoxColumn,
            this.dgTxtWaiverFileLocation,
            this.waiverSetDetailNameDataGridViewTextBoxColumn,
            this.waiverSetDetailGUIDDataGridViewTextBoxColumn,
            this.waiverFileNameDataGridViewTextBoxColumn,
            this.btnFileImage,
            this.validForDaysDataGridViewTextBoxColumn,
            this.effectiveDateDataGridViewTextBoxColumn,
            this.isActiveDataGridViewCheckBoxColumn1,
            this.lastUpdatedByDataGridViewTextBoxColumn1,
            this.lastUpdatedDateDataGridViewTextBoxColumn1});
            this.dgvWaiverDetail.DataSource = this.waiverSetDetailDTOBindingSource;
            this.dgvWaiverDetail.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvWaiverDetail.Location = new System.Drawing.Point(8, 18);
            this.dgvWaiverDetail.Name = "dgvWaiverDetail";
            this.dgvWaiverDetail.Size = new System.Drawing.Size(986, 159);
            this.dgvWaiverDetail.TabIndex = 3;
            this.dgvWaiverDetail.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvWaiverDetail_CellContentClick);
            this.dgvWaiverDetail.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvWaiverDetail_CellFormatting);
            this.dgvWaiverDetail.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvWaiverSetDetail_DataError);
            this.dgvWaiverDetail.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvWaiverSetDetail_RowEnter);
            // 
            // waiverSetDetailIdDataGridViewTextBoxColumn
            // 
            this.waiverSetDetailIdDataGridViewTextBoxColumn.DataPropertyName = "WaiverSetDetailId";
            this.waiverSetDetailIdDataGridViewTextBoxColumn.HeaderText = "Id";
            this.waiverSetDetailIdDataGridViewTextBoxColumn.Name = "waiverSetDetailIdDataGridViewTextBoxColumn";
            this.waiverSetDetailIdDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // dgTxtWaiverFileLocation
            // 
            this.dgTxtWaiverFileLocation.HeaderText = "FileLocation";
            this.dgTxtWaiverFileLocation.Name = "dgTxtWaiverFileLocation";
            this.dgTxtWaiverFileLocation.Visible = false;
            // 
            // waiverSetDetailNameDataGridViewTextBoxColumn
            // 
            this.waiverSetDetailNameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.waiverSetDetailNameDataGridViewTextBoxColumn.HeaderText = "Waiver Detail";
            this.waiverSetDetailNameDataGridViewTextBoxColumn.MinimumWidth = 10;
            this.waiverSetDetailNameDataGridViewTextBoxColumn.Name = "waiverSetDetailNameDataGridViewTextBoxColumn";
            // 
            // waiverSetDetailGUIDDataGridViewTextBoxColumn
            // 
            this.waiverSetDetailGUIDDataGridViewTextBoxColumn.DataPropertyName = "Guid";
            this.waiverSetDetailGUIDDataGridViewTextBoxColumn.HeaderText = "Column1";
            this.waiverSetDetailGUIDDataGridViewTextBoxColumn.Name = "waiverSetDetailGUIDDataGridViewTextBoxColumn";
            this.waiverSetDetailGUIDDataGridViewTextBoxColumn.Visible = false;
            // 
            // waiverFileNameDataGridViewTextBoxColumn
            // 
            this.waiverFileNameDataGridViewTextBoxColumn.DataPropertyName = "WaiverFileName";
            this.waiverFileNameDataGridViewTextBoxColumn.HeaderText = "Waiver File Name";
            this.waiverFileNameDataGridViewTextBoxColumn.Name = "waiverFileNameDataGridViewTextBoxColumn";
            this.waiverFileNameDataGridViewTextBoxColumn.ReadOnly = true;
            this.waiverFileNameDataGridViewTextBoxColumn.Width = 150;
            // 
            // btnFileImage
            // 
            this.btnFileImage.DataPropertyName = "btnFileImageData";
            this.btnFileImage.HeaderText = "...";
            this.btnFileImage.Name = "btnFileImage";
            this.btnFileImage.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.btnFileImage.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // validForDaysDataGridViewTextBoxColumn
            // 
            this.validForDaysDataGridViewTextBoxColumn.DataPropertyName = "ValidForDays";
            this.validForDaysDataGridViewTextBoxColumn.HeaderText = "Valid For Days";
            this.validForDaysDataGridViewTextBoxColumn.Name = "validForDaysDataGridViewTextBoxColumn";
            this.validForDaysDataGridViewTextBoxColumn.Visible = false;
            // 
            // effectiveDateDataGridViewTextBoxColumn
            // 
            this.effectiveDateDataGridViewTextBoxColumn.DataPropertyName = "EffectiveDate";
            this.effectiveDateDataGridViewTextBoxColumn.HeaderText = "Effective Date";
            this.effectiveDateDataGridViewTextBoxColumn.Name = "effectiveDateDataGridViewTextBoxColumn";
            // 
            // isActiveDataGridViewCheckBoxColumn1
            // 
            this.isActiveDataGridViewCheckBoxColumn1.DataPropertyName = "IsActive";
            this.isActiveDataGridViewCheckBoxColumn1.HeaderText = "Active?";
            this.isActiveDataGridViewCheckBoxColumn1.Name = "isActiveDataGridViewCheckBoxColumn1";
            this.isActiveDataGridViewCheckBoxColumn1.Width = 60;
            // 
            // lastUpdatedByDataGridViewTextBoxColumn1
            // 
            this.lastUpdatedByDataGridViewTextBoxColumn1.DataPropertyName = "LastUpdatedBy";
            this.lastUpdatedByDataGridViewTextBoxColumn1.HeaderText = "Last Updated By";
            this.lastUpdatedByDataGridViewTextBoxColumn1.Name = "lastUpdatedByDataGridViewTextBoxColumn1";
            this.lastUpdatedByDataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // lastUpdatedDateDataGridViewTextBoxColumn1
            // 
            this.lastUpdatedDateDataGridViewTextBoxColumn1.DataPropertyName = "LastUpdatedDate";
            this.lastUpdatedDateDataGridViewTextBoxColumn1.HeaderText = "Last Updated Date";
            this.lastUpdatedDateDataGridViewTextBoxColumn1.Name = "lastUpdatedDateDataGridViewTextBoxColumn1";
            this.lastUpdatedDateDataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.dgvLanguage);
            this.groupBox4.Location = new System.Drawing.Point(9, 444);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(999, 159);
            this.groupBox4.TabIndex = 14;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Waiver Set Detail Language";
            // 
            // waiverSetUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1017, 652);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.groupBox4);
            this.Name = "waiverSetUI";
            this.Text = "Waivers";
            this.Load += new System.EventHandler(this.WaiverSetUI_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvWaiverSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.waiverSetDTOBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchByWaiverSignedParametersBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLanguage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.objectTranslationsDTOBindingSource)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.waiverSetDetailDTOBindingSource)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvWaiverDetail)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataGridView dgvWaiverSet;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.BindingSource waiverSetDTOBindingSource;
        private System.Windows.Forms.BindingSource searchByWaiverSignedParametersBindingSource;
        private System.Windows.Forms.BindingSource waiverSetDetailDTOBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;
        private System.Windows.Forms.DataGridView dgvLanguage;
        private System.Windows.Forms.BindingSource objectTranslationsDTOBindingSource;
        private System.Windows.Forms.CheckBox chbShowActiveEntries;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.DataGridView dgvWaiverDetail;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.DataGridViewTextBoxColumn waiverSetIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn waiverSetNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isActiveDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewButtonColumn btnWaiverSignedOption;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastUpdatedByDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastUpdatedDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn languageIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn elementGuidDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn tableObjectDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn translationDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgTxtWaiverLanguageFileLocation;
        private System.Windows.Forms.DataGridViewButtonColumn btnfileUpload;
        private System.Windows.Forms.DataGridViewTextBoxColumn waiverSetDetailIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgTxtWaiverFileLocation;
        private System.Windows.Forms.DataGridViewTextBoxColumn waiverSetDetailNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn waiverSetDetailGUIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn waiverFileNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewButtonColumn btnFileImage;
        private System.Windows.Forms.DataGridViewTextBoxColumn validForDaysDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn effectiveDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isActiveDataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastUpdatedByDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastUpdatedDateDataGridViewTextBoxColumn1;
    }
}

