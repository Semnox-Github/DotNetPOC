namespace Semnox.Parafait.Inventory.Recipe
{
    partial class frmForecastingUI
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
            this.cbxEvents = new System.Windows.Forms.CheckBox();
            this.cbxFinished = new System.Windows.Forms.CheckBox();
            this.cbxSemiFinished = new System.Windows.Forms.CheckBox();
            this.lblEventOffset = new System.Windows.Forms.Label();
            this.txtEventOffset = new System.Windows.Forms.TextBox();
            this.lblHistoricalDays = new System.Windows.Forms.Label();
            this.lblSeasonal = new System.Windows.Forms.Label();
            this.lblAspirational = new System.Windows.Forms.Label();
            this.txtHistoricalDays = new System.Windows.Forms.TextBox();
            this.txtSeasonalPerc = new System.Windows.Forms.TextBox();
            this.txtAspirationalPerc = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnGetData = new System.Windows.Forms.Button();
            this.btnClearData = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dgvEstimationDetails = new System.Windows.Forms.DataGridView();
            this.cbxDel = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ProductId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RecipeName = new System.Windows.Forms.DataGridViewLinkColumn();
            this.stockDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EstimatedMonthlyQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EstimatedWeeklyQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PlannedQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UOM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DailyEstimate = new System.Windows.Forms.DataGridViewLinkColumn();
            this.forecastingSummaryDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCreatePlan = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
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
            this.dataGridViewTextBoxColumn12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn13 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn14 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn15 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn16 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn17 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblMessage = new System.Windows.Forms.Label();
            this.btnDelete = new System.Windows.Forms.Button();
            this.dtpFromDate = new System.Windows.Forms.DateTimePicker();
            this.dtpToDate = new System.Windows.Forms.DateTimePicker();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEstimationDetails)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.forecastingSummaryDTOBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dtpToDate);
            this.groupBox1.Controls.Add(this.dtpFromDate);
            this.groupBox1.Controls.Add(this.cbxEvents);
            this.groupBox1.Controls.Add(this.cbxFinished);
            this.groupBox1.Controls.Add(this.cbxSemiFinished);
            this.groupBox1.Controls.Add(this.lblEventOffset);
            this.groupBox1.Controls.Add(this.txtEventOffset);
            this.groupBox1.Controls.Add(this.lblHistoricalDays);
            this.groupBox1.Controls.Add(this.lblSeasonal);
            this.groupBox1.Controls.Add(this.lblAspirational);
            this.groupBox1.Controls.Add(this.txtHistoricalDays);
            this.groupBox1.Controls.Add(this.txtSeasonalPerc);
            this.groupBox1.Controls.Add(this.txtAspirationalPerc);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btnGetData);
            this.groupBox1.Controls.Add(this.btnClearData);
            this.groupBox1.Location = new System.Drawing.Point(33, 22);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1052, 124);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Estimation Header";
            // 
            // cbxEvents
            // 
            this.cbxEvents.AutoSize = true;
            this.cbxEvents.Location = new System.Drawing.Point(520, 78);
            this.cbxEvents.Name = "cbxEvents";
            this.cbxEvents.Size = new System.Drawing.Size(135, 17);
            this.cbxEvents.TabIndex = 20;
            this.cbxEvents.Text = "Events and Promotions";
            this.cbxEvents.UseVisualStyleBackColor = true;
            // 
            // cbxFinished
            // 
            this.cbxFinished.AutoSize = true;
            this.cbxFinished.Checked = true;
            this.cbxFinished.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbxFinished.Location = new System.Drawing.Point(798, 78);
            this.cbxFinished.Name = "cbxFinished";
            this.cbxFinished.Size = new System.Drawing.Size(65, 17);
            this.cbxFinished.TabIndex = 18;
            this.cbxFinished.Text = "Finished";
            this.cbxFinished.UseVisualStyleBackColor = true;
            // 
            // cbxSemiFinished
            // 
            this.cbxSemiFinished.AutoSize = true;
            this.cbxSemiFinished.Checked = true;
            this.cbxSemiFinished.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbxSemiFinished.Location = new System.Drawing.Point(696, 78);
            this.cbxSemiFinished.Name = "cbxSemiFinished";
            this.cbxSemiFinished.Size = new System.Drawing.Size(88, 17);
            this.cbxSemiFinished.TabIndex = 17;
            this.cbxSemiFinished.Text = "Semi finished";
            this.cbxSemiFinished.UseVisualStyleBackColor = true;
            // 
            // lblEventOffset
            // 
            this.lblEventOffset.AutoSize = true;
            this.lblEventOffset.Location = new System.Drawing.Point(492, 32);
            this.lblEventOffset.Name = "lblEventOffset";
            this.lblEventOffset.Size = new System.Drawing.Size(91, 13);
            this.lblEventOffset.TabIndex = 16;
            this.lblEventOffset.Text = "Event Offset(Hrs):";
            // 
            // txtEventOffset
            // 
            this.txtEventOffset.Location = new System.Drawing.Point(589, 28);
            this.txtEventOffset.Name = "txtEventOffset";
            this.txtEventOffset.Size = new System.Drawing.Size(66, 20);
            this.txtEventOffset.TabIndex = 15;
            this.txtEventOffset.TextChanged += new System.EventHandler(this.txtPerc_TextChanged);
            // 
            // lblHistoricalDays
            // 
            this.lblHistoricalDays.AutoSize = true;
            this.lblHistoricalDays.Location = new System.Drawing.Point(704, 32);
            this.lblHistoricalDays.Name = "lblHistoricalDays";
            this.lblHistoricalDays.Size = new System.Drawing.Size(80, 13);
            this.lblHistoricalDays.TabIndex = 14;
            this.lblHistoricalDays.Text = "Historical Days:";
            // 
            // lblSeasonal
            // 
            this.lblSeasonal.AutoSize = true;
            this.lblSeasonal.Location = new System.Drawing.Point(335, 80);
            this.lblSeasonal.Name = "lblSeasonal";
            this.lblSeasonal.Size = new System.Drawing.Size(65, 13);
            this.lblSeasonal.TabIndex = 13;
            this.lblSeasonal.Text = "Seasonal %:";
            // 
            // lblAspirational
            // 
            this.lblAspirational.AutoSize = true;
            this.lblAspirational.Location = new System.Drawing.Point(325, 32);
            this.lblAspirational.Name = "lblAspirational";
            this.lblAspirational.Size = new System.Drawing.Size(75, 13);
            this.lblAspirational.TabIndex = 12;
            this.lblAspirational.Text = "Aspirational %:";
            // 
            // txtHistoricalDays
            // 
            this.txtHistoricalDays.Location = new System.Drawing.Point(790, 28);
            this.txtHistoricalDays.Name = "txtHistoricalDays";
            this.txtHistoricalDays.Size = new System.Drawing.Size(66, 20);
            this.txtHistoricalDays.TabIndex = 11;
            this.txtHistoricalDays.TextChanged += new System.EventHandler(this.txtPerc_TextChanged);
            // 
            // txtSeasonalPerc
            // 
            this.txtSeasonalPerc.Location = new System.Drawing.Point(406, 76);
            this.txtSeasonalPerc.Name = "txtSeasonalPerc";
            this.txtSeasonalPerc.Size = new System.Drawing.Size(66, 20);
            this.txtSeasonalPerc.TabIndex = 10;
            this.txtSeasonalPerc.TextChanged += new System.EventHandler(this.txtPerc_TextChanged);
            // 
            // txtAspirationalPerc
            // 
            this.txtAspirationalPerc.Location = new System.Drawing.Point(406, 28);
            this.txtAspirationalPerc.Name = "txtAspirationalPerc";
            this.txtAspirationalPerc.Size = new System.Drawing.Size(66, 20);
            this.txtAspirationalPerc.TabIndex = 9;
            this.txtAspirationalPerc.TextChanged += new System.EventHandler(this.txtPerc_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(43, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "To Date:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(33, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "From Date:";
            // 
            // btnGetData
            // 
            this.btnGetData.Location = new System.Drawing.Point(907, 67);
            this.btnGetData.Name = "btnGetData";
            this.btnGetData.Size = new System.Drawing.Size(123, 38);
            this.btnGetData.TabIndex = 4;
            this.btnGetData.Text = "Get Data";
            this.btnGetData.UseVisualStyleBackColor = true;
            this.btnGetData.Click += new System.EventHandler(this.btnGetData_Click);
            // 
            // btnClearData
            // 
            this.btnClearData.Location = new System.Drawing.Point(907, 19);
            this.btnClearData.Name = "btnClearData";
            this.btnClearData.Size = new System.Drawing.Size(123, 38);
            this.btnClearData.TabIndex = 3;
            this.btnClearData.Text = "Clear Data";
            this.btnClearData.UseVisualStyleBackColor = true;
            this.btnClearData.Click += new System.EventHandler(this.btnClearData_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dgvEstimationDetails);
            this.groupBox2.Location = new System.Drawing.Point(33, 152);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1052, 350);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Estimation Details";
            // 
            // dgvEstimationDetails
            // 
            this.dgvEstimationDetails.AutoGenerateColumns = false;
            this.dgvEstimationDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvEstimationDetails.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cbxDel,
            this.ProductId,
            this.RecipeName,
            this.stockDataGridViewTextBoxColumn,
            this.EstimatedMonthlyQuantity,
            this.EstimatedWeeklyQuantity,
            this.PlannedQuantity,
            this.UOM,
            this.DailyEstimate});
            this.dgvEstimationDetails.DataSource = this.forecastingSummaryDTOBindingSource;
            this.dgvEstimationDetails.Location = new System.Drawing.Point(19, 19);
            this.dgvEstimationDetails.Name = "dgvEstimationDetails";
            this.dgvEstimationDetails.Size = new System.Drawing.Size(1027, 262);
            this.dgvEstimationDetails.TabIndex = 0;
            this.dgvEstimationDetails.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvEstimationDetails_CellClick);
            // 
            // cbxDel
            // 
            this.cbxDel.HeaderText = "Del";
            this.cbxDel.MinimumWidth = 80;
            this.cbxDel.Name = "cbxDel";
            this.cbxDel.Width = 80;
            // 
            // ProductId
            // 
            this.ProductId.DataPropertyName = "ProductId";
            this.ProductId.HeaderText = "ProductId";
            this.ProductId.Name = "ProductId";
            this.ProductId.Visible = false;
            // 
            // RecipeName
            // 
            this.RecipeName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.RecipeName.DataPropertyName = "RecipeName";
            this.RecipeName.HeaderText = "Recipe Name";
            this.RecipeName.MinimumWidth = 120;
            this.RecipeName.Name = "RecipeName";
            this.RecipeName.ReadOnly = true;
            this.RecipeName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.RecipeName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.RecipeName.Width = 120;
            // 
            // stockDataGridViewTextBoxColumn
            // 
            this.stockDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.stockDataGridViewTextBoxColumn.DataPropertyName = "Stock";
            this.stockDataGridViewTextBoxColumn.FillWeight = 50F;
            this.stockDataGridViewTextBoxColumn.HeaderText = "Stock";
            this.stockDataGridViewTextBoxColumn.MinimumWidth = 120;
            this.stockDataGridViewTextBoxColumn.Name = "stockDataGridViewTextBoxColumn";
            this.stockDataGridViewTextBoxColumn.ReadOnly = true;
            this.stockDataGridViewTextBoxColumn.Width = 120;
            // 
            // EstimatedMonthlyQuantity
            // 
            this.EstimatedMonthlyQuantity.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.EstimatedMonthlyQuantity.DataPropertyName = "EstimatedMonthlyQuantity";
            this.EstimatedMonthlyQuantity.HeaderText = "Estimated Quantity";
            this.EstimatedMonthlyQuantity.MinimumWidth = 120;
            this.EstimatedMonthlyQuantity.Name = "EstimatedMonthlyQuantity";
            this.EstimatedMonthlyQuantity.ReadOnly = true;
            this.EstimatedMonthlyQuantity.Width = 120;
            // 
            // EstimatedWeeklyQuantity
            // 
            this.EstimatedWeeklyQuantity.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.EstimatedWeeklyQuantity.DataPropertyName = "EstimatedWeeklyQuantity";
            this.EstimatedWeeklyQuantity.HeaderText = "Estimated Weekly Quantity";
            this.EstimatedWeeklyQuantity.MinimumWidth = 120;
            this.EstimatedWeeklyQuantity.Name = "EstimatedWeeklyQuantity";
            this.EstimatedWeeklyQuantity.ReadOnly = true;
            this.EstimatedWeeklyQuantity.Width = 120;
            // 
            // PlannedQuantity
            // 
            this.PlannedQuantity.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.PlannedQuantity.DataPropertyName = "PlannedQuantity";
            this.PlannedQuantity.FillWeight = 50F;
            this.PlannedQuantity.HeaderText = "Planned Quantity";
            this.PlannedQuantity.MinimumWidth = 120;
            this.PlannedQuantity.Name = "PlannedQuantity";
            this.PlannedQuantity.ReadOnly = true;
            this.PlannedQuantity.Width = 120;
            // 
            // UOM
            // 
            this.UOM.DataPropertyName = "UOM";
            this.UOM.HeaderText = "UOM";
            this.UOM.Name = "UOM";
            this.UOM.ReadOnly = true;
            // 
            // DailyEstimate
            // 
            this.DailyEstimate.DataPropertyName = "EstimationDetails";
            this.DailyEstimate.HeaderText = "Daily Estimate";
            this.DailyEstimate.Name = "DailyEstimate";
            this.DailyEstimate.ReadOnly = true;
            // 
            // forecastingSummaryDTOBindingSource
            // 
            this.forecastingSummaryDTOBindingSource.DataSource = typeof(Semnox.Parafait.Inventory.Recipe.ForecastingSummaryDTO);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(33, 508);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(123, 38);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCreatePlan
            // 
            this.btnCreatePlan.Location = new System.Drawing.Point(163, 508);
            this.btnCreatePlan.Name = "btnCreatePlan";
            this.btnCreatePlan.Size = new System.Drawing.Size(123, 38);
            this.btnCreatePlan.TabIndex = 3;
            this.btnCreatePlan.Text = "Create Plan";
            this.btnCreatePlan.UseVisualStyleBackColor = true;
            this.btnCreatePlan.Click += new System.EventHandler(this.btnCreatePlan_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(421, 508);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(123, 38);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "RecipeEstimationDetailId";
            this.dataGridViewTextBoxColumn1.HeaderText = "RecipeEstimationDetailId";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Visible = false;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "RecipeName";
            this.dataGridViewTextBoxColumn2.HeaderText = "Recipe Name";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "StockQty";
            this.dataGridViewTextBoxColumn3.HeaderText = "Stock";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.DataPropertyName = "RecipeEstimationHeaderId";
            this.dataGridViewTextBoxColumn4.HeaderText = "RecipeEstimationHeaderId";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.Visible = false;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.DataPropertyName = "ProductId";
            this.dataGridViewTextBoxColumn5.HeaderText = "ProductId";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.Visible = false;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.DataPropertyName = "EstimatedQty";
            this.dataGridViewTextBoxColumn6.HeaderText = "Estimated Quantity";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.DataPropertyName = "TotalEstimatedQty";
            this.dataGridViewTextBoxColumn7.HeaderText = "Total Estimated Quantity";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.DataPropertyName = "PlannedQty";
            this.dataGridViewTextBoxColumn8.HeaderText = "Planned Quantity";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            // 
            // dataGridViewTextBoxColumn9
            // 
            this.dataGridViewTextBoxColumn9.DataPropertyName = "UOMId";
            this.dataGridViewTextBoxColumn9.HeaderText = "UOMId";
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            this.dataGridViewTextBoxColumn9.Visible = false;
            // 
            // dataGridViewTextBoxColumn10
            // 
            this.dataGridViewTextBoxColumn10.DataPropertyName = "AccountingCalendarMasterId";
            this.dataGridViewTextBoxColumn10.HeaderText = "AccountingCalendarMasterId";
            this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            this.dataGridViewTextBoxColumn10.Visible = false;
            // 
            // dataGridViewTextBoxColumn11
            // 
            this.dataGridViewTextBoxColumn11.DataPropertyName = "CreatedBy";
            this.dataGridViewTextBoxColumn11.HeaderText = "CreatedBy";
            this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
            this.dataGridViewTextBoxColumn11.Visible = false;
            // 
            // dataGridViewTextBoxColumn12
            // 
            this.dataGridViewTextBoxColumn12.DataPropertyName = "CreationDate";
            this.dataGridViewTextBoxColumn12.HeaderText = "CreationDate";
            this.dataGridViewTextBoxColumn12.Name = "dataGridViewTextBoxColumn12";
            this.dataGridViewTextBoxColumn12.Visible = false;
            // 
            // dataGridViewTextBoxColumn13
            // 
            this.dataGridViewTextBoxColumn13.DataPropertyName = "LastUpdatedBy";
            this.dataGridViewTextBoxColumn13.HeaderText = "LastUpdatedBy";
            this.dataGridViewTextBoxColumn13.Name = "dataGridViewTextBoxColumn13";
            this.dataGridViewTextBoxColumn13.Visible = false;
            // 
            // dataGridViewTextBoxColumn14
            // 
            this.dataGridViewTextBoxColumn14.DataPropertyName = "LastUpdateDate";
            this.dataGridViewTextBoxColumn14.HeaderText = "LastUpdateDate";
            this.dataGridViewTextBoxColumn14.Name = "dataGridViewTextBoxColumn14";
            this.dataGridViewTextBoxColumn14.Visible = false;
            // 
            // dataGridViewTextBoxColumn15
            // 
            this.dataGridViewTextBoxColumn15.DataPropertyName = "SiteId";
            this.dataGridViewTextBoxColumn15.HeaderText = "SiteId";
            this.dataGridViewTextBoxColumn15.Name = "dataGridViewTextBoxColumn15";
            this.dataGridViewTextBoxColumn15.Visible = false;
            // 
            // dataGridViewTextBoxColumn16
            // 
            this.dataGridViewTextBoxColumn16.DataPropertyName = "MasterEntityId";
            this.dataGridViewTextBoxColumn16.HeaderText = "MasterEntityId";
            this.dataGridViewTextBoxColumn16.Name = "dataGridViewTextBoxColumn16";
            this.dataGridViewTextBoxColumn16.Visible = false;
            // 
            // dataGridViewTextBoxColumn17
            // 
            this.dataGridViewTextBoxColumn17.DataPropertyName = "Guid";
            this.dataGridViewTextBoxColumn17.HeaderText = "Guid";
            this.dataGridViewTextBoxColumn17.Name = "dataGridViewTextBoxColumn17";
            this.dataGridViewTextBoxColumn17.Visible = false;
            // 
            // lblMessage
            // 
            this.lblMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage.Location = new System.Drawing.Point(1, 571);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(1119, 25);
            this.lblMessage.TabIndex = 5;
            // 
            // btnDelete
            // 
            this.btnDelete.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnDelete.Location = new System.Drawing.Point(292, 508);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(123, 38);
            this.btnDelete.TabIndex = 6;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // dtpFromDate
            // 
            this.dtpFromDate.CustomFormat = "ddd, dd-MMM-yyyy";
            this.dtpFromDate.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.dtpFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFromDate.Location = new System.Drawing.Point(98, 25);
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.Size = new System.Drawing.Size(177, 26);
            this.dtpFromDate.TabIndex = 63;
            this.dtpFromDate.Value = new System.DateTime(2020, 8, 21, 0, 0, 0, 0);
            // 
            // dtpToDate
            // 
            this.dtpToDate.CustomFormat = "ddd, dd-MMM-yyyy";
            this.dtpToDate.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.dtpToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpToDate.Location = new System.Drawing.Point(98, 73);
            this.dtpToDate.Name = "dtpToDate";
            this.dtpToDate.Size = new System.Drawing.Size(177, 26);
            this.dtpToDate.TabIndex = 64;
            this.dtpToDate.Value = new System.DateTime(2020, 8, 21, 0, 0, 0, 0);
            // 
            // frmForecastingUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1175, 591);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnCreatePlan);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmForecastingUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Recipe Production Estimate";
            this.Load += new System.EventHandler(this.frmForecastingUI_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvEstimationDetails)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.forecastingSummaryDTOBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView dgvEstimationDetails;
        private System.Windows.Forms.Button btnGetData;
        private System.Windows.Forms.Button btnClearData;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCreatePlan;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox cbxFinished;
        private System.Windows.Forms.CheckBox cbxSemiFinished;
        private System.Windows.Forms.Label lblEventOffset;
        private System.Windows.Forms.TextBox txtEventOffset;
        private System.Windows.Forms.Label lblHistoricalDays;
        private System.Windows.Forms.Label lblSeasonal;
        private System.Windows.Forms.Label lblAspirational;
        private System.Windows.Forms.TextBox txtHistoricalDays;
        private System.Windows.Forms.TextBox txtSeasonalPerc;
        private System.Windows.Forms.TextBox txtAspirationalPerc;
        private System.Windows.Forms.DataGridViewTextBoxColumn eventQtyDataGridViewTextBoxColumn;
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
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn12;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn13;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn14;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn15;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn16;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn17;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.DataGridViewTextBoxColumn estimatedMonthlyQuantityDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn estimatedWeeklyQuantityDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn plannedQuantityDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn producedQuantityDataGridViewTextBoxColumn;
        private System.Windows.Forms.BindingSource forecastingSummaryDTOBindingSource;
        private System.Windows.Forms.CheckBox cbxEvents;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.DataGridViewCheckBoxColumn cbxDel;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProductId;
        private System.Windows.Forms.DataGridViewLinkColumn RecipeName;
        private System.Windows.Forms.DataGridViewTextBoxColumn stockDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn EstimatedMonthlyQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn EstimatedWeeklyQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn PlannedQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn UOM;
        private System.Windows.Forms.DataGridViewLinkColumn DailyEstimate;
        private System.Windows.Forms.DateTimePicker dtpToDate;
        private System.Windows.Forms.DateTimePicker dtpFromDate;
    }
}