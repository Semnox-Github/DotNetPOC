namespace Semnox.Parafait.Inventory.Recipe
{
    partial class frmAddRecipeUI
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
            this.dtpStartDate = new System.Windows.Forms.DateTimePicker();
            this.grpReccur = new System.Windows.Forms.GroupBox();
            this.dtpEndDate = new System.Windows.Forms.DateTimePicker();
            this.pnlMonth = new System.Windows.Forms.Panel();
            this.rdRecurMonthlyByWeekDay = new System.Windows.Forms.RadioButton();
            this.rdRecurMonthlyByDate = new System.Windows.Forms.RadioButton();
            this.rdMonthly = new System.Windows.Forms.RadioButton();
            this.cbxRecur = new System.Windows.Forms.CheckBox();
            this.pnlDays = new System.Windows.Forms.Panel();
            this.cbxWen = new System.Windows.Forms.CheckBox();
            this.cbxSat = new System.Windows.Forms.CheckBox();
            this.cbxSun = new System.Windows.Forms.CheckBox();
            this.cbxFri = new System.Windows.Forms.CheckBox();
            this.cbxMon = new System.Windows.Forms.CheckBox();
            this.cbxThu = new System.Windows.Forms.CheckBox();
            this.cbxTue = new System.Windows.Forms.CheckBox();
            this.rdDaily = new System.Windows.Forms.RadioButton();
            this.rdWeekly = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dgvAddRecipe = new System.Windows.Forms.DataGridView();
            this.RecipeName = new System.Windows.Forms.DataGridViewLinkColumn();
            this.txtUOM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblMessage = new System.Windows.Forms.Label();
            this.recipePlanDetailIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.recipePlanHeaderIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.productIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.plannedQtyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.incrementalQtyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.finalQtyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.uOMIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.recipeEstimationDetailIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.qtyModifiedDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isActiveDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.createdByDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.creationDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastUpdatedByDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastUpdateDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.siteIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.masterEntityIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.synchStatusDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.guidDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isChangedDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.recipePlanDetailsDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.groupBox1.SuspendLayout();
            this.grpReccur.SuspendLayout();
            this.pnlMonth.SuspendLayout();
            this.pnlDays.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAddRecipe)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.recipePlanDetailsDTOBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dtpStartDate);
            this.groupBox1.Controls.Add(this.grpReccur);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(12, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(999, 112);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Schedule Setup";
            // 
            // dtpStartDate
            // 
            this.dtpStartDate.CustomFormat = "ddd, dd-MMM-yyyy";
            this.dtpStartDate.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.dtpStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpStartDate.Location = new System.Drawing.Point(26, 67);
            this.dtpStartDate.Name = "dtpStartDate";
            this.dtpStartDate.Size = new System.Drawing.Size(177, 26);
            this.dtpStartDate.TabIndex = 63;
            this.dtpStartDate.Value = new System.DateTime(2020, 8, 21, 0, 0, 0, 0);
            this.dtpStartDate.ValueChanged += new System.EventHandler(this.dtpStartDate_ValueChanged);
            // 
            // grpReccur
            // 
            this.grpReccur.Controls.Add(this.dtpEndDate);
            this.grpReccur.Controls.Add(this.pnlMonth);
            this.grpReccur.Controls.Add(this.rdMonthly);
            this.grpReccur.Controls.Add(this.cbxRecur);
            this.grpReccur.Controls.Add(this.pnlDays);
            this.grpReccur.Controls.Add(this.rdDaily);
            this.grpReccur.Controls.Add(this.rdWeekly);
            this.grpReccur.Controls.Add(this.label1);
            this.grpReccur.Location = new System.Drawing.Point(221, 9);
            this.grpReccur.Name = "grpReccur";
            this.grpReccur.Size = new System.Drawing.Size(761, 91);
            this.grpReccur.TabIndex = 15;
            this.grpReccur.TabStop = false;
            this.grpReccur.Text = "Recurrence";
            // 
            // dtpEndDate
            // 
            this.dtpEndDate.CustomFormat = "ddd, dd-MMM-yyyy";
            this.dtpEndDate.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.dtpEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpEndDate.Location = new System.Drawing.Point(109, 55);
            this.dtpEndDate.Name = "dtpEndDate";
            this.dtpEndDate.Size = new System.Drawing.Size(177, 26);
            this.dtpEndDate.TabIndex = 64;
            this.dtpEndDate.Value = new System.DateTime(2020, 8, 21, 0, 0, 0, 0);
            this.dtpEndDate.ValueChanged += new System.EventHandler(this.dtpEndDate_ValueChanged);
            // 
            // pnlMonth
            // 
            this.pnlMonth.Controls.Add(this.rdRecurMonthlyByWeekDay);
            this.pnlMonth.Controls.Add(this.rdRecurMonthlyByDate);
            this.pnlMonth.Location = new System.Drawing.Point(449, 53);
            this.pnlMonth.Name = "pnlMonth";
            this.pnlMonth.Size = new System.Drawing.Size(141, 30);
            this.pnlMonth.TabIndex = 14;
            // 
            // rdRecurMonthlyByWeekDay
            // 
            this.rdRecurMonthlyByWeekDay.AutoSize = true;
            this.rdRecurMonthlyByWeekDay.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdRecurMonthlyByWeekDay.Location = new System.Drawing.Point(68, 8);
            this.rdRecurMonthlyByWeekDay.Name = "rdRecurMonthlyByWeekDay";
            this.rdRecurMonthlyByWeekDay.Size = new System.Drawing.Size(70, 18);
            this.rdRecurMonthlyByWeekDay.TabIndex = 15;
            this.rdRecurMonthlyByWeekDay.TabStop = true;
            this.rdRecurMonthlyByWeekDay.Text = "Weekday";
            this.rdRecurMonthlyByWeekDay.UseVisualStyleBackColor = true;
            // 
            // rdRecurMonthlyByDate
            // 
            this.rdRecurMonthlyByDate.AutoSize = true;
            this.rdRecurMonthlyByDate.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdRecurMonthlyByDate.Location = new System.Drawing.Point(15, 8);
            this.rdRecurMonthlyByDate.Name = "rdRecurMonthlyByDate";
            this.rdRecurMonthlyByDate.Size = new System.Drawing.Size(47, 18);
            this.rdRecurMonthlyByDate.TabIndex = 14;
            this.rdRecurMonthlyByDate.TabStop = true;
            this.rdRecurMonthlyByDate.Text = "Date";
            this.rdRecurMonthlyByDate.UseVisualStyleBackColor = true;
            this.rdRecurMonthlyByDate.CheckedChanged += new System.EventHandler(this.rdRecurMonthlyByDate_CheckedChanged);
            // 
            // rdMonthly
            // 
            this.rdMonthly.BackColor = System.Drawing.Color.Beige;
            this.rdMonthly.Location = new System.Drawing.Point(372, 59);
            this.rdMonthly.Name = "rdMonthly";
            this.rdMonthly.Size = new System.Drawing.Size(63, 19);
            this.rdMonthly.TabIndex = 5;
            this.rdMonthly.TabStop = true;
            this.rdMonthly.Text = "Monthly";
            this.rdMonthly.UseVisualStyleBackColor = false;
            this.rdMonthly.CheckedChanged += new System.EventHandler(this.rdMonthly_CheckedChanged);
            // 
            // cbxRecur
            // 
            this.cbxRecur.AutoSize = true;
            this.cbxRecur.Location = new System.Drawing.Point(9, 25);
            this.cbxRecur.Name = "cbxRecur";
            this.cbxRecur.Size = new System.Drawing.Size(55, 17);
            this.cbxRecur.TabIndex = 0;
            this.cbxRecur.Text = "Recur";
            this.cbxRecur.UseVisualStyleBackColor = true;
            this.cbxRecur.CheckedChanged += new System.EventHandler(this.cbxRecur_CheckedChanged);
            // 
            // pnlDays
            // 
            this.pnlDays.Controls.Add(this.cbxWen);
            this.pnlDays.Controls.Add(this.cbxSat);
            this.pnlDays.Controls.Add(this.cbxSun);
            this.pnlDays.Controls.Add(this.cbxFri);
            this.pnlDays.Controls.Add(this.cbxMon);
            this.pnlDays.Controls.Add(this.cbxThu);
            this.pnlDays.Controls.Add(this.cbxTue);
            this.pnlDays.Location = new System.Drawing.Point(372, 18);
            this.pnlDays.Name = "pnlDays";
            this.pnlDays.Size = new System.Drawing.Size(380, 30);
            this.pnlDays.TabIndex = 13;
            // 
            // cbxWen
            // 
            this.cbxWen.AutoSize = true;
            this.cbxWen.Location = new System.Drawing.Point(169, 7);
            this.cbxWen.Name = "cbxWen";
            this.cbxWen.Size = new System.Drawing.Size(49, 17);
            this.cbxWen.TabIndex = 9;
            this.cbxWen.Text = "Wed";
            this.cbxWen.UseVisualStyleBackColor = true;
            this.cbxWen.CheckedChanged += new System.EventHandler(this.weekday_CheckedChanged);
            // 
            // cbxSat
            // 
            this.cbxSat.AutoSize = true;
            this.cbxSat.Location = new System.Drawing.Point(329, 7);
            this.cbxSat.Name = "cbxSat";
            this.cbxSat.Size = new System.Drawing.Size(42, 17);
            this.cbxSat.TabIndex = 12;
            this.cbxSat.Text = "Sat";
            this.cbxSat.UseVisualStyleBackColor = true;
            this.cbxSat.CheckedChanged += new System.EventHandler(this.weekday_CheckedChanged);
            // 
            // cbxSun
            // 
            this.cbxSun.AutoSize = true;
            this.cbxSun.Location = new System.Drawing.Point(16, 7);
            this.cbxSun.Name = "cbxSun";
            this.cbxSun.Size = new System.Drawing.Size(45, 17);
            this.cbxSun.TabIndex = 6;
            this.cbxSun.Text = "Sun";
            this.cbxSun.UseVisualStyleBackColor = true;
            this.cbxSun.CheckedChanged += new System.EventHandler(this.weekday_CheckedChanged);
            // 
            // cbxFri
            // 
            this.cbxFri.AutoSize = true;
            this.cbxFri.Location = new System.Drawing.Point(277, 7);
            this.cbxFri.Name = "cbxFri";
            this.cbxFri.Size = new System.Drawing.Size(37, 17);
            this.cbxFri.TabIndex = 11;
            this.cbxFri.Text = "Fri";
            this.cbxFri.UseVisualStyleBackColor = true;
            this.cbxFri.CheckedChanged += new System.EventHandler(this.weekday_CheckedChanged);
            // 
            // cbxMon
            // 
            this.cbxMon.AutoSize = true;
            this.cbxMon.Location = new System.Drawing.Point(67, 7);
            this.cbxMon.Name = "cbxMon";
            this.cbxMon.Size = new System.Drawing.Size(47, 17);
            this.cbxMon.TabIndex = 7;
            this.cbxMon.Text = "Mon";
            this.cbxMon.UseVisualStyleBackColor = true;
            this.cbxMon.CheckedChanged += new System.EventHandler(this.weekday_CheckedChanged);
            // 
            // cbxThu
            // 
            this.cbxThu.AutoSize = true;
            this.cbxThu.Location = new System.Drawing.Point(224, 7);
            this.cbxThu.Name = "cbxThu";
            this.cbxThu.Size = new System.Drawing.Size(45, 17);
            this.cbxThu.TabIndex = 10;
            this.cbxThu.Text = "Thu";
            this.cbxThu.UseVisualStyleBackColor = true;
            this.cbxThu.CheckedChanged += new System.EventHandler(this.weekday_CheckedChanged);
            // 
            // cbxTue
            // 
            this.cbxTue.AutoSize = true;
            this.cbxTue.Location = new System.Drawing.Point(118, 7);
            this.cbxTue.Name = "cbxTue";
            this.cbxTue.Size = new System.Drawing.Size(45, 17);
            this.cbxTue.TabIndex = 8;
            this.cbxTue.Text = "Tue";
            this.cbxTue.UseVisualStyleBackColor = true;
            this.cbxTue.CheckedChanged += new System.EventHandler(this.weekday_CheckedChanged);
            // 
            // rdDaily
            // 
            this.rdDaily.AutoSize = true;
            this.rdDaily.Location = new System.Drawing.Point(304, 60);
            this.rdDaily.Name = "rdDaily";
            this.rdDaily.Size = new System.Drawing.Size(48, 17);
            this.rdDaily.TabIndex = 1;
            this.rdDaily.TabStop = true;
            this.rdDaily.Text = "Daily";
            this.rdDaily.UseVisualStyleBackColor = true;
            this.rdDaily.CheckedChanged += new System.EventHandler(this.rdDaily_CheckedChanged);
            // 
            // rdWeekly
            // 
            this.rdWeekly.AutoSize = true;
            this.rdWeekly.Location = new System.Drawing.Point(304, 25);
            this.rdWeekly.Name = "rdWeekly";
            this.rdWeekly.Size = new System.Drawing.Size(61, 17);
            this.rdWeekly.TabIndex = 2;
            this.rdWeekly.TabStop = true;
            this.rdWeekly.Text = "Weekly";
            this.rdWeekly.UseVisualStyleBackColor = true;
            this.rdWeekly.CheckedChanged += new System.EventHandler(this.rdDaily_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "Recur End Date:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(25, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Plan Date:";
            // 
            // dgvAddRecipe
            // 
            this.dgvAddRecipe.AutoGenerateColumns = false;
            this.dgvAddRecipe.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAddRecipe.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.recipePlanDetailIdDataGridViewTextBoxColumn,
            this.RecipeName,
            this.recipePlanHeaderIdDataGridViewTextBoxColumn,
            this.productIdDataGridViewTextBoxColumn,
            this.plannedQtyDataGridViewTextBoxColumn,
            this.incrementalQtyDataGridViewTextBoxColumn,
            this.finalQtyDataGridViewTextBoxColumn,
            this.uOMIdDataGridViewTextBoxColumn,
            this.recipeEstimationDetailIdDataGridViewTextBoxColumn,
            this.qtyModifiedDateDataGridViewTextBoxColumn,
            this.txtUOM,
            this.isActiveDataGridViewCheckBoxColumn,
            this.createdByDataGridViewTextBoxColumn,
            this.creationDateDataGridViewTextBoxColumn,
            this.lastUpdatedByDataGridViewTextBoxColumn,
            this.lastUpdateDateDataGridViewTextBoxColumn,
            this.siteIdDataGridViewTextBoxColumn,
            this.masterEntityIdDataGridViewTextBoxColumn,
            this.synchStatusDataGridViewCheckBoxColumn,
            this.guidDataGridViewTextBoxColumn,
            this.isChangedDataGridViewCheckBoxColumn});
            this.dgvAddRecipe.DataSource = this.recipePlanDetailsDTOBindingSource;
            this.dgvAddRecipe.Location = new System.Drawing.Point(6, 19);
            this.dgvAddRecipe.Name = "dgvAddRecipe";
            this.dgvAddRecipe.Size = new System.Drawing.Size(976, 339);
            this.dgvAddRecipe.TabIndex = 1;
            this.dgvAddRecipe.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAddRecipe_CellClick);
            this.dgvAddRecipe.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAddRecipe_CellValueChanged);
            this.dgvAddRecipe.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvAddRecipe_DataError);
            this.dgvAddRecipe.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.dgvAddRecipe_RowPrePaint);
            // 
            // RecipeName
            // 
            this.RecipeName.DataPropertyName = "RecipeName";
            this.RecipeName.HeaderText = "Recipe Name";
            this.RecipeName.Name = "RecipeName";
            this.RecipeName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.RecipeName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // txtUOM
            // 
            this.txtUOM.DataPropertyName = "UOM";
            this.txtUOM.HeaderText = "UOM";
            this.txtUOM.Name = "txtUOM";
            this.txtUOM.ReadOnly = true;
            this.txtUOM.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(12, 505);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(87, 38);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefresh.Location = new System.Drawing.Point(125, 505);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(124, 38);
            this.btnRefresh.TabIndex = 4;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnSaveAndNew_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(283, 505);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(87, 38);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dgvAddRecipe);
            this.groupBox2.Location = new System.Drawing.Point(12, 135);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(999, 364);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Recipe plan details";
            // 
            // lblMessage
            // 
            this.lblMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage.Location = new System.Drawing.Point(8, 565);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(1077, 20);
            this.lblMessage.TabIndex = 7;
            // 
            // recipePlanDetailIdDataGridViewTextBoxColumn
            // 
            this.recipePlanDetailIdDataGridViewTextBoxColumn.DataPropertyName = "RecipePlanDetailId";
            this.recipePlanDetailIdDataGridViewTextBoxColumn.HeaderText = "RecipePlanDetailId";
            this.recipePlanDetailIdDataGridViewTextBoxColumn.Name = "recipePlanDetailIdDataGridViewTextBoxColumn";
            this.recipePlanDetailIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // recipePlanHeaderIdDataGridViewTextBoxColumn
            // 
            this.recipePlanHeaderIdDataGridViewTextBoxColumn.DataPropertyName = "RecipePlanHeaderId";
            this.recipePlanHeaderIdDataGridViewTextBoxColumn.HeaderText = "RecipePlanHeaderId";
            this.recipePlanHeaderIdDataGridViewTextBoxColumn.Name = "recipePlanHeaderIdDataGridViewTextBoxColumn";
            this.recipePlanHeaderIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // productIdDataGridViewTextBoxColumn
            // 
            this.productIdDataGridViewTextBoxColumn.DataPropertyName = "ProductId";
            this.productIdDataGridViewTextBoxColumn.HeaderText = "ProductId";
            this.productIdDataGridViewTextBoxColumn.Name = "productIdDataGridViewTextBoxColumn";
            this.productIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // plannedQtyDataGridViewTextBoxColumn
            // 
            this.plannedQtyDataGridViewTextBoxColumn.DataPropertyName = "PlannedQty";
            this.plannedQtyDataGridViewTextBoxColumn.HeaderText = "PlannedQty";
            this.plannedQtyDataGridViewTextBoxColumn.Name = "plannedQtyDataGridViewTextBoxColumn";
            this.plannedQtyDataGridViewTextBoxColumn.Visible = false;
            // 
            // incrementalQtyDataGridViewTextBoxColumn
            // 
            this.incrementalQtyDataGridViewTextBoxColumn.DataPropertyName = "IncrementalQty";
            this.incrementalQtyDataGridViewTextBoxColumn.HeaderText = "IncrementalQty";
            this.incrementalQtyDataGridViewTextBoxColumn.Name = "incrementalQtyDataGridViewTextBoxColumn";
            this.incrementalQtyDataGridViewTextBoxColumn.Visible = false;
            // 
            // finalQtyDataGridViewTextBoxColumn
            // 
            this.finalQtyDataGridViewTextBoxColumn.DataPropertyName = "FinalQty";
            this.finalQtyDataGridViewTextBoxColumn.HeaderText = "Quantity";
            this.finalQtyDataGridViewTextBoxColumn.Name = "finalQtyDataGridViewTextBoxColumn";
            // 
            // uOMIdDataGridViewTextBoxColumn
            // 
            this.uOMIdDataGridViewTextBoxColumn.DataPropertyName = "UOMId";
            this.uOMIdDataGridViewTextBoxColumn.HeaderText = "UOMId";
            this.uOMIdDataGridViewTextBoxColumn.Name = "uOMIdDataGridViewTextBoxColumn";
            this.uOMIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // recipeEstimationDetailIdDataGridViewTextBoxColumn
            // 
            this.recipeEstimationDetailIdDataGridViewTextBoxColumn.DataPropertyName = "RecipeEstimationDetailId";
            this.recipeEstimationDetailIdDataGridViewTextBoxColumn.HeaderText = "RecipeEstimationDetailId";
            this.recipeEstimationDetailIdDataGridViewTextBoxColumn.Name = "recipeEstimationDetailIdDataGridViewTextBoxColumn";
            this.recipeEstimationDetailIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // qtyModifiedDateDataGridViewTextBoxColumn
            // 
            this.qtyModifiedDateDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.qtyModifiedDateDataGridViewTextBoxColumn.DataPropertyName = "QtyModifiedDate";
            this.qtyModifiedDateDataGridViewTextBoxColumn.HeaderText = "Quantity Modified Date";
            this.qtyModifiedDateDataGridViewTextBoxColumn.Name = "qtyModifiedDateDataGridViewTextBoxColumn";
            this.qtyModifiedDateDataGridViewTextBoxColumn.ReadOnly = true;
            this.qtyModifiedDateDataGridViewTextBoxColumn.Width = 107;
            // 
            // isActiveDataGridViewCheckBoxColumn
            // 
            this.isActiveDataGridViewCheckBoxColumn.DataPropertyName = "IsActive";
            this.isActiveDataGridViewCheckBoxColumn.HeaderText = "IsActive";
            this.isActiveDataGridViewCheckBoxColumn.Name = "isActiveDataGridViewCheckBoxColumn";
            // 
            // createdByDataGridViewTextBoxColumn
            // 
            this.createdByDataGridViewTextBoxColumn.DataPropertyName = "CreatedBy";
            this.createdByDataGridViewTextBoxColumn.HeaderText = "Created By";
            this.createdByDataGridViewTextBoxColumn.Name = "createdByDataGridViewTextBoxColumn";
            this.createdByDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // creationDateDataGridViewTextBoxColumn
            // 
            this.creationDateDataGridViewTextBoxColumn.DataPropertyName = "CreationDate";
            this.creationDateDataGridViewTextBoxColumn.HeaderText = "Creation Date";
            this.creationDateDataGridViewTextBoxColumn.Name = "creationDateDataGridViewTextBoxColumn";
            this.creationDateDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // lastUpdatedByDataGridViewTextBoxColumn
            // 
            this.lastUpdatedByDataGridViewTextBoxColumn.DataPropertyName = "LastUpdatedBy";
            this.lastUpdatedByDataGridViewTextBoxColumn.HeaderText = "Last Updated By";
            this.lastUpdatedByDataGridViewTextBoxColumn.Name = "lastUpdatedByDataGridViewTextBoxColumn";
            this.lastUpdatedByDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // lastUpdateDateDataGridViewTextBoxColumn
            // 
            this.lastUpdateDateDataGridViewTextBoxColumn.DataPropertyName = "LastUpdateDate";
            this.lastUpdateDateDataGridViewTextBoxColumn.HeaderText = "Last Update Date";
            this.lastUpdateDateDataGridViewTextBoxColumn.Name = "lastUpdateDateDataGridViewTextBoxColumn";
            this.lastUpdateDateDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // siteIdDataGridViewTextBoxColumn
            // 
            this.siteIdDataGridViewTextBoxColumn.DataPropertyName = "SiteId";
            this.siteIdDataGridViewTextBoxColumn.HeaderText = "SiteId";
            this.siteIdDataGridViewTextBoxColumn.Name = "siteIdDataGridViewTextBoxColumn";
            this.siteIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // masterEntityIdDataGridViewTextBoxColumn
            // 
            this.masterEntityIdDataGridViewTextBoxColumn.DataPropertyName = "MasterEntityId";
            this.masterEntityIdDataGridViewTextBoxColumn.HeaderText = "MasterEntityId";
            this.masterEntityIdDataGridViewTextBoxColumn.Name = "masterEntityIdDataGridViewTextBoxColumn";
            this.masterEntityIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // synchStatusDataGridViewCheckBoxColumn
            // 
            this.synchStatusDataGridViewCheckBoxColumn.DataPropertyName = "SynchStatus";
            this.synchStatusDataGridViewCheckBoxColumn.HeaderText = "SynchStatus";
            this.synchStatusDataGridViewCheckBoxColumn.Name = "synchStatusDataGridViewCheckBoxColumn";
            this.synchStatusDataGridViewCheckBoxColumn.Visible = false;
            // 
            // guidDataGridViewTextBoxColumn
            // 
            this.guidDataGridViewTextBoxColumn.DataPropertyName = "Guid";
            this.guidDataGridViewTextBoxColumn.HeaderText = "Guid";
            this.guidDataGridViewTextBoxColumn.Name = "guidDataGridViewTextBoxColumn";
            this.guidDataGridViewTextBoxColumn.Visible = false;
            // 
            // isChangedDataGridViewCheckBoxColumn
            // 
            this.isChangedDataGridViewCheckBoxColumn.DataPropertyName = "IsChanged";
            this.isChangedDataGridViewCheckBoxColumn.HeaderText = "IsChanged";
            this.isChangedDataGridViewCheckBoxColumn.Name = "isChangedDataGridViewCheckBoxColumn";
            this.isChangedDataGridViewCheckBoxColumn.Visible = false;
            // 
            // recipePlanDetailsDTOBindingSource
            // 
            this.recipePlanDetailsDTOBindingSource.DataSource = typeof(Semnox.Parafait.Inventory.Recipe.RecipePlanDetailsDTO);
            // 
            // frmAddRecipeUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1023, 594);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MinimizeBox = false;
            this.Name = "frmAddRecipeUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add Recipe";
            this.Load += new System.EventHandler(this.frmAddRecipeUI_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.grpReccur.ResumeLayout(false);
            this.grpReccur.PerformLayout();
            this.pnlMonth.ResumeLayout(false);
            this.pnlMonth.PerformLayout();
            this.pnlDays.ResumeLayout(false);
            this.pnlDays.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAddRecipe)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.recipePlanDetailsDTOBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton rdMonthly;
        private System.Windows.Forms.RadioButton rdWeekly;
        private System.Windows.Forms.CheckBox cbxRecur;
        private System.Windows.Forms.RadioButton rdDaily;
        private System.Windows.Forms.CheckBox cbxSat;
        private System.Windows.Forms.CheckBox cbxFri;
        private System.Windows.Forms.CheckBox cbxThu;
        private System.Windows.Forms.CheckBox cbxWen;
        private System.Windows.Forms.CheckBox cbxTue;
        private System.Windows.Forms.CheckBox cbxMon;
        private System.Windows.Forms.CheckBox cbxSun;
        private System.Windows.Forms.DataGridView dgvAddRecipe;
        private System.Windows.Forms.BindingSource recipePlanDetailsDTOBindingSource;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Panel pnlDays;
        private System.Windows.Forms.Panel pnlMonth;
        private System.Windows.Forms.RadioButton rdRecurMonthlyByWeekDay;
        private System.Windows.Forms.RadioButton rdRecurMonthlyByDate;
        private System.Windows.Forms.GroupBox grpReccur;
        private System.Windows.Forms.DataGridViewTextBoxColumn recipePlanDetailIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewLinkColumn RecipeName;
        private System.Windows.Forms.DataGridViewTextBoxColumn recipePlanHeaderIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn productIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn plannedQtyDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn incrementalQtyDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn finalQtyDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn uOMIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn recipeEstimationDetailIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn qtyModifiedDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn txtUOM;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isActiveDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn createdByDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn creationDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastUpdatedByDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastUpdateDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn siteIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn masterEntityIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn synchStatusDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn guidDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isChangedDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DateTimePicker dtpStartDate;
        private System.Windows.Forms.DateTimePicker dtpEndDate;
    }
}