namespace Semnox.Parafait.Achievements
{
    partial class AchievementClassUI
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
            this.dgvAchievementClass = new System.Windows.Forms.DataGridView();
            this.dgvAchievementClassLevel = new System.Windows.Forms.DataGridView();
            this.dgTxtImagePath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btndgSaveImage = new System.Windows.Forms.DataGridViewButtonColumn();
            this.dgvAchievementScoreConversion = new System.Windows.Forms.DataGridView();
            this.drpProjects = new System.Windows.Forms.ComboBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnRefesh = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lnkPublish = new System.Windows.Forms.LinkLabel();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fromDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isActiveDataGridViewCheckBoxColumn2 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Ratio = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.achievementClassLevelIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ConversionEntitlement = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.masterEntityIdDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.achievementScoreConversionDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.AchievementClassLevelId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.levelNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.achievementClassIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ParentLevelId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.QualifyingScore = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.drpQualifyingLevelId = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.RegistrationRequired = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.BonusAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BonusEntitlement = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.isActiveDataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.masterEntityIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.externalSystemReferenceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Picture = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.achievementClassLevelDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.AchievementClassId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ClassName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.achievementProjectIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.drpGameId = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.isActiveDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.masterEntityIdDataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.externalSystemReferenceDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProductId = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.achievementClassDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAchievementClass)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAchievementClassLevel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAchievementScoreConversion)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.achievementScoreConversionDTOBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.achievementClassLevelDTOBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.achievementClassDTOBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvAchievementClass
            // 
            this.dgvAchievementClass.AllowUserToDeleteRows = false;
            this.dgvAchievementClass.AutoGenerateColumns = false;
            this.dgvAchievementClass.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAchievementClass.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.AchievementClassId,
            this.ClassName,
            this.achievementProjectIdDataGridViewTextBoxColumn,
            this.drpGameId,
            this.isActiveDataGridViewCheckBoxColumn,
            this.masterEntityIdDataGridViewTextBoxColumn2,
            this.externalSystemReferenceDataGridViewTextBoxColumn1,
            this.ProductId});
            this.dgvAchievementClass.DataSource = this.achievementClassDTOBindingSource;
            this.dgvAchievementClass.Location = new System.Drawing.Point(12, 85);
            this.dgvAchievementClass.Name = "dgvAchievementClass";
            this.dgvAchievementClass.Size = new System.Drawing.Size(933, 139);
            this.dgvAchievementClass.TabIndex = 0;
            this.dgvAchievementClass.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAchievementClass_CellClick);
            this.dgvAchievementClass.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAchievementClass_CellContentClick);
            this.dgvAchievementClass.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAchievementClass_CellValueChanged);
            this.dgvAchievementClass.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvAchievementClass_DataBindingComplete);
            this.dgvAchievementClass.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvAchievementClass_DataError);
            this.dgvAchievementClass.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAchievementClass_RowEnter);
            // 
            // dgvAchievementClassLevel
            // 
            this.dgvAchievementClassLevel.AllowUserToDeleteRows = false;
            this.dgvAchievementClassLevel.AllowUserToOrderColumns = true;
            this.dgvAchievementClassLevel.AutoGenerateColumns = false;
            this.dgvAchievementClassLevel.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAchievementClassLevel.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.AchievementClassLevelId,
            this.levelNameDataGridViewTextBoxColumn,
            this.achievementClassIdDataGridViewTextBoxColumn,
            this.ParentLevelId,
            this.QualifyingScore,
            this.drpQualifyingLevelId,
            this.RegistrationRequired,
            this.BonusAmount,
            this.BonusEntitlement,
            this.isActiveDataGridViewCheckBoxColumn1,
            this.masterEntityIdDataGridViewTextBoxColumn,
            this.externalSystemReferenceDataGridViewTextBoxColumn,
            this.dgTxtImagePath,
            this.Picture,
            this.btndgSaveImage});
            this.dgvAchievementClassLevel.DataSource = this.achievementClassLevelDTOBindingSource;
            this.dgvAchievementClassLevel.Location = new System.Drawing.Point(12, 260);
            this.dgvAchievementClassLevel.Name = "dgvAchievementClassLevel";
            this.dgvAchievementClassLevel.Size = new System.Drawing.Size(933, 117);
            this.dgvAchievementClassLevel.TabIndex = 1;
            this.dgvAchievementClassLevel.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAchievementClassLevel_CellContentClick);
            this.dgvAchievementClassLevel.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAchievementClassLevel_CellValueChanged);
            this.dgvAchievementClassLevel.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvAchievementClassLevel_DataError);
            this.dgvAchievementClassLevel.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAchievementClassLevel_RowEnter);
            // 
            // dgTxtImagePath
            // 
            this.dgTxtImagePath.HeaderText = "dgTxtImagePath";
            this.dgTxtImagePath.Name = "dgTxtImagePath";
            this.dgTxtImagePath.Visible = false;
            // 
            // btndgSaveImage
            // 
            this.btndgSaveImage.HeaderText = "...";
            this.btndgSaveImage.Name = "btndgSaveImage";
            this.btndgSaveImage.Width = 20;
            // 
            // dgvAchievementScoreConversion
            // 
            this.dgvAchievementScoreConversion.AllowUserToDeleteRows = false;
            this.dgvAchievementScoreConversion.AutoGenerateColumns = false;
            this.dgvAchievementScoreConversion.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAchievementScoreConversion.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Id,
            this.fromDateDataGridViewTextBoxColumn,
            this.toDateDataGridViewTextBoxColumn,
            this.isActiveDataGridViewCheckBoxColumn2,
            this.Ratio,
            this.achievementClassLevelIdDataGridViewTextBoxColumn,
            this.ConversionEntitlement,
            this.masterEntityIdDataGridViewTextBoxColumn1});
            this.dgvAchievementScoreConversion.DataSource = this.achievementScoreConversionDTOBindingSource;
            this.dgvAchievementScoreConversion.Location = new System.Drawing.Point(12, 422);
            this.dgvAchievementScoreConversion.Name = "dgvAchievementScoreConversion";
            this.dgvAchievementScoreConversion.Size = new System.Drawing.Size(933, 139);
            this.dgvAchievementScoreConversion.TabIndex = 2;
            this.dgvAchievementScoreConversion.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAchievementScoreConversion_CellValueChanged);
            this.dgvAchievementScoreConversion.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvAchievementScoreConversion_DataError);
            this.dgvAchievementScoreConversion.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAchievementScoreConversion_RowEnter);
            // 
            // drpProjects
            // 
            this.drpProjects.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.drpProjects.FormattingEnabled = true;
            this.drpProjects.Location = new System.Drawing.Point(6, 16);
            this.drpProjects.Name = "drpProjects";
            this.drpProjects.Size = new System.Drawing.Size(270, 23);
            this.drpProjects.TabIndex = 3;
            this.drpProjects.SelectedIndexChanged += new System.EventHandler(this.drpProjects_SelectedIndexChanged);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnSave.Location = new System.Drawing.Point(12, 567);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 14;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnRefesh
            // 
            this.btnRefesh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRefesh.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnRefesh.Location = new System.Drawing.Point(107, 567);
            this.btnRefesh.Name = "btnRefesh";
            this.btnRefesh.Size = new System.Drawing.Size(75, 23);
            this.btnRefesh.TabIndex = 15;
            this.btnRefesh.Text = "Refresh";
            this.btnRefesh.UseVisualStyleBackColor = true;
            this.btnRefesh.Click += new System.EventHandler(this.btnRefesh_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.drpProjects);
            this.groupBox1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(13, 11);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(806, 43);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Select Project";
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnDelete.Location = new System.Drawing.Point(202, 567);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 17;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnClose.Location = new System.Drawing.Point(297, 567);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 18;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 241);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(149, 15);
            this.label1.TabIndex = 19;
            this.label1.Text = "Achievement Class Level";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(13, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(116, 15);
            this.label2.TabIndex = 20;
            this.label2.Text = "Achievement Class";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(12, 403);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(185, 15);
            this.label3.TabIndex = 21;
            this.label3.Text = "Achievement Score Conversion";
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
            this.lnkPublish.Location = new System.Drawing.Point(400, 573);
            this.lnkPublish.Name = "lnkPublish";
            this.lnkPublish.Size = new System.Drawing.Size(96, 15);
            this.lnkPublish.TabIndex = 46;
            this.lnkPublish.TabStop = true;
            this.lnkPublish.Text = "Publish To Sites";
            this.lnkPublish.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkPublish_LinkClicked);
            // 
            // Id
            // 
            this.Id.DataPropertyName = "Id";
            this.Id.HeaderText = "Id";
            this.Id.Name = "Id";
            this.Id.ReadOnly = true;
            // 
            // fromDateDataGridViewTextBoxColumn
            // 
            this.fromDateDataGridViewTextBoxColumn.DataPropertyName = "FromDate";
            this.fromDateDataGridViewTextBoxColumn.HeaderText = "From Date";
            this.fromDateDataGridViewTextBoxColumn.Name = "fromDateDataGridViewTextBoxColumn";
            // 
            // toDateDataGridViewTextBoxColumn
            // 
            this.toDateDataGridViewTextBoxColumn.DataPropertyName = "ToDate";
            this.toDateDataGridViewTextBoxColumn.HeaderText = "To Date";
            this.toDateDataGridViewTextBoxColumn.Name = "toDateDataGridViewTextBoxColumn";
            // 
            // isActiveDataGridViewCheckBoxColumn2
            // 
            this.isActiveDataGridViewCheckBoxColumn2.DataPropertyName = "IsActive";
            this.isActiveDataGridViewCheckBoxColumn2.FalseValue = "false";
            this.isActiveDataGridViewCheckBoxColumn2.HeaderText = "Active";
            this.isActiveDataGridViewCheckBoxColumn2.IndeterminateValue = "";
            this.isActiveDataGridViewCheckBoxColumn2.Name = "isActiveDataGridViewCheckBoxColumn2";
            this.isActiveDataGridViewCheckBoxColumn2.TrueValue = "true";
            // 
            // Ratio
            // 
            this.Ratio.DataPropertyName = "Ratio";
            this.Ratio.HeaderText = "Ratio";
            this.Ratio.Name = "Ratio";
            // 
            // achievementClassLevelIdDataGridViewTextBoxColumn
            // 
            this.achievementClassLevelIdDataGridViewTextBoxColumn.DataPropertyName = "AchievementClassLevelId";
            this.achievementClassLevelIdDataGridViewTextBoxColumn.HeaderText = "AchievementClassLevelId";
            this.achievementClassLevelIdDataGridViewTextBoxColumn.Name = "achievementClassLevelIdDataGridViewTextBoxColumn";
            this.achievementClassLevelIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // ConversionEntitlement
            // 
            this.ConversionEntitlement.DataPropertyName = "ConversionEntitlement";
            this.ConversionEntitlement.HeaderText = "Conversion Entitlement";
            this.ConversionEntitlement.Name = "ConversionEntitlement";
            this.ConversionEntitlement.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ConversionEntitlement.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // masterEntityIdDataGridViewTextBoxColumn1
            // 
            this.masterEntityIdDataGridViewTextBoxColumn1.DataPropertyName = "MasterEntityId";
            this.masterEntityIdDataGridViewTextBoxColumn1.HeaderText = "MasterEntityId";
            this.masterEntityIdDataGridViewTextBoxColumn1.Name = "masterEntityIdDataGridViewTextBoxColumn1";
            this.masterEntityIdDataGridViewTextBoxColumn1.Visible = false;
            // 
            // achievementScoreConversionDTOBindingSource
            // 
            this.achievementScoreConversionDTOBindingSource.DataSource = typeof(Semnox.Parafait.Achievements.AchievementScoreConversionDTO);
            // 
            // AchievementClassLevelId
            // 
            this.AchievementClassLevelId.DataPropertyName = "AchievementClassLevelId";
            this.AchievementClassLevelId.HeaderText = "Achievement Class Level Id";
            this.AchievementClassLevelId.Name = "AchievementClassLevelId";
            this.AchievementClassLevelId.ReadOnly = true;
            // 
            // levelNameDataGridViewTextBoxColumn
            // 
            this.levelNameDataGridViewTextBoxColumn.DataPropertyName = "LevelName";
            this.levelNameDataGridViewTextBoxColumn.HeaderText = "Level Name";
            this.levelNameDataGridViewTextBoxColumn.Name = "levelNameDataGridViewTextBoxColumn";
            // 
            // achievementClassIdDataGridViewTextBoxColumn
            // 
            this.achievementClassIdDataGridViewTextBoxColumn.DataPropertyName = "AchievementClassId";
            this.achievementClassIdDataGridViewTextBoxColumn.HeaderText = "Achievement Class Id";
            this.achievementClassIdDataGridViewTextBoxColumn.Name = "achievementClassIdDataGridViewTextBoxColumn";
            this.achievementClassIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // ParentLevelId
            // 
            this.ParentLevelId.DataPropertyName = "ParentLevelId";
            this.ParentLevelId.HeaderText = "ParentLevelId";
            this.ParentLevelId.Name = "ParentLevelId";
            this.ParentLevelId.Visible = false;
            // 
            // QualifyingScore
            // 
            this.QualifyingScore.DataPropertyName = "QualifyingScore";
            this.QualifyingScore.HeaderText = "Qualifying Score";
            this.QualifyingScore.Name = "QualifyingScore";
            // 
            // drpQualifyingLevelId
            // 
            this.drpQualifyingLevelId.DataPropertyName = "QualifyingLevelId";
            this.drpQualifyingLevelId.HeaderText = "Qualifying Level";
            this.drpQualifyingLevelId.Name = "drpQualifyingLevelId";
            this.drpQualifyingLevelId.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.drpQualifyingLevelId.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // RegistrationRequired
            // 
            this.RegistrationRequired.DataPropertyName = "RegistrationRequired";
            this.RegistrationRequired.FalseValue = "false";
            this.RegistrationRequired.HeaderText = "Registration Required";
            this.RegistrationRequired.Name = "RegistrationRequired";
            this.RegistrationRequired.TrueValue = "true";
            // 
            // BonusAmount
            // 
            this.BonusAmount.DataPropertyName = "BonusAmount";
            this.BonusAmount.HeaderText = "Bonus Amount";
            this.BonusAmount.Name = "BonusAmount";
            // 
            // BonusEntitlement
            // 
            this.BonusEntitlement.DataPropertyName = "BonusEntitlement";
            this.BonusEntitlement.HeaderText = "Bonus Entitlement";
            this.BonusEntitlement.Name = "BonusEntitlement";
            // 
            // isActiveDataGridViewCheckBoxColumn1
            // 
            this.isActiveDataGridViewCheckBoxColumn1.DataPropertyName = "IsActive";
            this.isActiveDataGridViewCheckBoxColumn1.FalseValue = "false";
            this.isActiveDataGridViewCheckBoxColumn1.HeaderText = "Active";
            this.isActiveDataGridViewCheckBoxColumn1.Name = "isActiveDataGridViewCheckBoxColumn1";
            this.isActiveDataGridViewCheckBoxColumn1.TrueValue = "true";
            // 
            // masterEntityIdDataGridViewTextBoxColumn
            // 
            this.masterEntityIdDataGridViewTextBoxColumn.DataPropertyName = "MasterEntityId";
            this.masterEntityIdDataGridViewTextBoxColumn.HeaderText = "MasterEntityId";
            this.masterEntityIdDataGridViewTextBoxColumn.Name = "masterEntityIdDataGridViewTextBoxColumn";
            this.masterEntityIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // externalSystemReferenceDataGridViewTextBoxColumn
            // 
            this.externalSystemReferenceDataGridViewTextBoxColumn.DataPropertyName = "ExternalSystemReference";
            this.externalSystemReferenceDataGridViewTextBoxColumn.HeaderText = "ExternalSystemReference";
            this.externalSystemReferenceDataGridViewTextBoxColumn.Name = "externalSystemReferenceDataGridViewTextBoxColumn";
            this.externalSystemReferenceDataGridViewTextBoxColumn.Visible = false;
            // 
            // Picture
            // 
            this.Picture.DataPropertyName = "Picture";
            this.Picture.HeaderText = "Medal";
            this.Picture.Name = "Picture";
            // 
            // achievementClassLevelDTOBindingSource
            // 
            this.achievementClassLevelDTOBindingSource.DataSource = typeof(Semnox.Parafait.Achievements.AchievementClassLevelDTO);
            // 
            // AchievementClassId
            // 
            this.AchievementClassId.DataPropertyName = "AchievementClassId";
            this.AchievementClassId.HeaderText = "Achievement Class Id";
            this.AchievementClassId.Name = "AchievementClassId";
            this.AchievementClassId.ReadOnly = true;
            this.AchievementClassId.Width = 150;
            // 
            // ClassName
            // 
            this.ClassName.DataPropertyName = "ClassName";
            this.ClassName.HeaderText = "Class Name";
            this.ClassName.Name = "ClassName";
            // 
            // achievementProjectIdDataGridViewTextBoxColumn
            // 
            this.achievementProjectIdDataGridViewTextBoxColumn.DataPropertyName = "AchievementProjectId";
            this.achievementProjectIdDataGridViewTextBoxColumn.HeaderText = "Achievement Project Id";
            this.achievementProjectIdDataGridViewTextBoxColumn.Name = "achievementProjectIdDataGridViewTextBoxColumn";
            this.achievementProjectIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // drpGameId
            // 
            this.drpGameId.DataPropertyName = "GameId";
            this.drpGameId.HeaderText = "Game Name";
            this.drpGameId.Name = "drpGameId";
            this.drpGameId.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.drpGameId.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // isActiveDataGridViewCheckBoxColumn
            // 
            this.isActiveDataGridViewCheckBoxColumn.DataPropertyName = "IsActive";
            this.isActiveDataGridViewCheckBoxColumn.HeaderText = "Active";
            this.isActiveDataGridViewCheckBoxColumn.Name = "isActiveDataGridViewCheckBoxColumn";
            // 
            // masterEntityIdDataGridViewTextBoxColumn2
            // 
            this.masterEntityIdDataGridViewTextBoxColumn2.DataPropertyName = "MasterEntityId";
            this.masterEntityIdDataGridViewTextBoxColumn2.HeaderText = "MasterEntityId";
            this.masterEntityIdDataGridViewTextBoxColumn2.Name = "masterEntityIdDataGridViewTextBoxColumn2";
            this.masterEntityIdDataGridViewTextBoxColumn2.Visible = false;
            // 
            // externalSystemReferenceDataGridViewTextBoxColumn1
            // 
            this.externalSystemReferenceDataGridViewTextBoxColumn1.DataPropertyName = "ExternalSystemReference";
            this.externalSystemReferenceDataGridViewTextBoxColumn1.HeaderText = "External System Reference Id";
            this.externalSystemReferenceDataGridViewTextBoxColumn1.Name = "externalSystemReferenceDataGridViewTextBoxColumn1";
            // 
            // ProductId
            // 
            this.ProductId.DataPropertyName = "ProductId";
            this.ProductId.HeaderText = "Product";
            this.ProductId.Name = "ProductId";
            this.ProductId.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ProductId.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // achievementClassDTOBindingSource
            // 
            this.achievementClassDTOBindingSource.DataSource = typeof(Semnox.Parafait.Achievements.AchievementClassDTO);
            // 
            // AchievementClassUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(964, 602);
            this.Controls.Add(this.lnkPublish);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnRefesh);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.dgvAchievementScoreConversion);
            this.Controls.Add(this.dgvAchievementClassLevel);
            this.Controls.Add(this.dgvAchievementClass);
            this.Name = "AchievementClassUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Achievement Class";
            ((System.ComponentModel.ISupportInitialize)(this.dgvAchievementClass)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAchievementClassLevel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAchievementScoreConversion)).EndInit();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.achievementScoreConversionDTOBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.achievementClassLevelDTOBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.achievementClassDTOBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvAchievementClass;
        private System.Windows.Forms.DataGridView dgvAchievementClassLevel;
        private System.Windows.Forms.DataGridView dgvAchievementScoreConversion;
        private System.Windows.Forms.ComboBox drpProjects;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnRefesh;
        private System.Windows.Forms.BindingSource achievementClassDTOBindingSource;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnDelete;
        //private System.Windows.Forms.DataGridViewTextBoxColumn LevelName;
        //private System.Windows.Forms.DataGridViewCheckBoxColumn registrationRequiredDataGridViewCheckBoxColumn;
        private System.Windows.Forms.BindingSource achievementScoreConversionDTOBindingSource;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.BindingSource achievementClassLevelDTOBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn AchievementClassId;
        private System.Windows.Forms.DataGridViewTextBoxColumn ClassName;
        private System.Windows.Forms.DataGridViewTextBoxColumn achievementProjectIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn drpGameId;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isActiveDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn masterEntityIdDataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn externalSystemReferenceDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewComboBoxColumn ProductId;
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn fromDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn toDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isActiveDataGridViewCheckBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Ratio;
        private System.Windows.Forms.DataGridViewTextBoxColumn achievementClassLevelIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn ConversionEntitlement;
        private System.Windows.Forms.DataGridViewTextBoxColumn masterEntityIdDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn AchievementClassLevelId;
        private System.Windows.Forms.DataGridViewTextBoxColumn levelNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn achievementClassIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ParentLevelId;
        private System.Windows.Forms.DataGridViewTextBoxColumn QualifyingScore;
        private System.Windows.Forms.DataGridViewComboBoxColumn drpQualifyingLevelId;
        private System.Windows.Forms.DataGridViewCheckBoxColumn RegistrationRequired;
        private System.Windows.Forms.DataGridViewTextBoxColumn BonusAmount;
        private System.Windows.Forms.DataGridViewComboBoxColumn BonusEntitlement;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isActiveDataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn masterEntityIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn externalSystemReferenceDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgTxtImagePath;
        private System.Windows.Forms.DataGridViewTextBoxColumn Picture;
        private System.Windows.Forms.DataGridViewButtonColumn btndgSaveImage;
        private System.Windows.Forms.LinkLabel lnkPublish;
    }
}
