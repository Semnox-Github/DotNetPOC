namespace Parafait_POS.Reservation
{
    partial class frmRescheduleReservationUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRescheduleReservationUI));
            this.gpbSearchOptions = new System.Windows.Forms.GroupBox();
            this.gpbFacilityProducts = new System.Windows.Forms.GroupBox();
            this.searchedScheduleVScrollBar = new Semnox.Core.GenericUtilities.VerticalScrollBarView();
            this.dgvSearchSchedules = new System.Windows.Forms.DataGridView();
            this.SelectSchedule = new System.Windows.Forms.DataGridViewButtonColumn();
            this.facilityMapIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.facilityMapNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmbFromTime = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.cmbToTime = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.masterScheduleIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.masterScheduleNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.scheduleIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ScheduleFromDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ScheduleToDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ScheduleFromTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.scheduleToTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FixedSchedule = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.attractionPlayIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.attractionPlayNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.attractionPlayPriceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.productIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.productNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.priceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.facilityCapacityDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ruleUnitsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.totalUnitsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bookedUnitsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.availableUnitsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.scheduleNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.desiredUnitsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.expiryDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.categoryIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.promotionIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.seatsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.facilityMapDTODataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.scheduleDetailsDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.searchedScheduleHScrollBar = new Semnox.Core.GenericUtilities.HorizontalScrollBarView();
            this.cmbSearchFacility = new Semnox.Core.GenericUtilities.AutoCompleteComboBox();
            this.lblFacility = new System.Windows.Forms.Label();
            this.lblTimeSlots = new System.Windows.Forms.Label();
            this.cbxNightSlot = new Semnox.Core.GenericUtilities.CustomCheckBox();
            this.cbxAfternoonSlot = new Semnox.Core.GenericUtilities.CustomCheckBox();
            this.cbxMorningSlot = new Semnox.Core.GenericUtilities.CustomCheckBox();
            this.cbxEarlyMorningSlot = new Semnox.Core.GenericUtilities.CustomCheckBox();
            this.cmbSearchToTime = new Semnox.Core.GenericUtilities.AutoCompleteComboBox();
            this.cmbSearchFromTime = new Semnox.Core.GenericUtilities.AutoCompleteComboBox();
            this.lblFromTimeSearch = new System.Windows.Forms.Label();
            this.lblToTimeSearch = new System.Windows.Forms.Label();
            this.dtpSearchDate = new System.Windows.Forms.DateTimePicker();
            this.lblSelectedDate = new System.Windows.Forms.Label();
            this.lblBookingProductLabel = new System.Windows.Forms.Label();
            this.lblScheduleLabel = new System.Windows.Forms.Label();
            this.lblBookingProductName = new System.Windows.Forms.Label();
            this.lblBookedScheduleDetails = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblQty = new System.Windows.Forms.Label();
            this.lblQtyVallue = new System.Windows.Forms.Label();
            this.lblOldFacilityMap = new System.Windows.Forms.Label();
            this.lblOldFacilityMapValue = new System.Windows.Forms.Label();
            this.gpbSearchOptions.SuspendLayout();
            this.gpbFacilityProducts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSearchSchedules)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.scheduleDetailsDTOBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // gpbSearchOptions
            // 
            this.gpbSearchOptions.AutoSize = true;
            this.gpbSearchOptions.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.gpbSearchOptions.Controls.Add(this.gpbFacilityProducts);
            this.gpbSearchOptions.Controls.Add(this.cmbSearchFacility);
            this.gpbSearchOptions.Controls.Add(this.lblFacility);
            this.gpbSearchOptions.Controls.Add(this.lblTimeSlots);
            this.gpbSearchOptions.Controls.Add(this.cbxNightSlot);
            this.gpbSearchOptions.Controls.Add(this.cbxAfternoonSlot);
            this.gpbSearchOptions.Controls.Add(this.cbxMorningSlot);
            this.gpbSearchOptions.Controls.Add(this.cbxEarlyMorningSlot);
            this.gpbSearchOptions.Controls.Add(this.cmbSearchToTime);
            this.gpbSearchOptions.Controls.Add(this.cmbSearchFromTime);
            this.gpbSearchOptions.Controls.Add(this.lblFromTimeSearch);
            this.gpbSearchOptions.Controls.Add(this.lblToTimeSearch);
            this.gpbSearchOptions.Controls.Add(this.dtpSearchDate);
            this.gpbSearchOptions.Controls.Add(this.lblSelectedDate);
            this.gpbSearchOptions.Font = new System.Drawing.Font("Arial", 9F);
            this.gpbSearchOptions.Location = new System.Drawing.Point(14, 71);
            this.gpbSearchOptions.Margin = new System.Windows.Forms.Padding(0);
            this.gpbSearchOptions.Name = "gpbSearchOptions";
            this.gpbSearchOptions.Padding = new System.Windows.Forms.Padding(3, 0, 4, 0);
            this.gpbSearchOptions.Size = new System.Drawing.Size(968, 510);
            this.gpbSearchOptions.TabIndex = 1;
            this.gpbSearchOptions.TabStop = false;
            this.gpbSearchOptions.Text = "Search";
            // 
            // gpbFacilityProducts
            // 
            this.gpbFacilityProducts.AutoSize = true;
            this.gpbFacilityProducts.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.gpbFacilityProducts.Controls.Add(this.searchedScheduleVScrollBar);
            this.gpbFacilityProducts.Controls.Add(this.dgvSearchSchedules);
            this.gpbFacilityProducts.Controls.Add(this.searchedScheduleHScrollBar);
            this.gpbFacilityProducts.Location = new System.Drawing.Point(4, 93);
            this.gpbFacilityProducts.Margin = new System.Windows.Forms.Padding(0);
            this.gpbFacilityProducts.Name = "gpbFacilityProducts";
            this.gpbFacilityProducts.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.gpbFacilityProducts.Size = new System.Drawing.Size(960, 403);
            this.gpbFacilityProducts.TabIndex = 116;
            this.gpbFacilityProducts.TabStop = false;
            this.gpbFacilityProducts.Text = "Pick Schedule";
            // 
            // searchedScheduleVScrollBar
            // 
            this.searchedScheduleVScrollBar.AutoHide = false;
            this.searchedScheduleVScrollBar.DataGridView = this.dgvSearchSchedules;
            this.searchedScheduleVScrollBar.DownButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("searchedScheduleVScrollBar.DownButtonBackgroundImage")));
            this.searchedScheduleVScrollBar.DownButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("searchedScheduleVScrollBar.DownButtonDisabledBackgroundImage")));
            this.searchedScheduleVScrollBar.Location = new System.Drawing.Point(910, 15);
            this.searchedScheduleVScrollBar.Margin = new System.Windows.Forms.Padding(0);
            this.searchedScheduleVScrollBar.Name = "searchedScheduleVScrollBar";
            this.searchedScheduleVScrollBar.ScrollableControl = null;
            this.searchedScheduleVScrollBar.ScrollViewer = null;
            this.searchedScheduleVScrollBar.Size = new System.Drawing.Size(47, 325);
            this.searchedScheduleVScrollBar.TabIndex = 114;
            this.searchedScheduleVScrollBar.UpButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("searchedScheduleVScrollBar.UpButtonBackgroundImage")));
            this.searchedScheduleVScrollBar.UpButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("searchedScheduleVScrollBar.UpButtonDisabledBackgroundImage")));
            this.searchedScheduleVScrollBar.UpButtonClick += new System.EventHandler(this.Scroll_ButtonClick);
            this.searchedScheduleVScrollBar.DownButtonClick += new System.EventHandler(this.Scroll_ButtonClick);
            // 
            // dgvSearchSchedules
            // 
            this.dgvSearchSchedules.AllowUserToAddRows = false;
            this.dgvSearchSchedules.AllowUserToDeleteRows = false;
            this.dgvSearchSchedules.AutoGenerateColumns = false;
            this.dgvSearchSchedules.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSearchSchedules.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SelectSchedule,
            this.facilityMapIdDataGridViewTextBoxColumn,
            this.facilityMapNameDataGridViewTextBoxColumn,
            this.cmbFromTime,
            this.cmbToTime,
            this.masterScheduleIdDataGridViewTextBoxColumn,
            this.masterScheduleNameDataGridViewTextBoxColumn,
            this.scheduleIdDataGridViewTextBoxColumn,
            this.ScheduleFromDate,
            this.ScheduleToDate,
            this.ScheduleFromTime,
            this.scheduleToTime,
            this.FixedSchedule,
            this.attractionPlayIdDataGridViewTextBoxColumn,
            this.attractionPlayNameDataGridViewTextBoxColumn,
            this.attractionPlayPriceDataGridViewTextBoxColumn,
            this.productIdDataGridViewTextBoxColumn,
            this.productNameDataGridViewTextBoxColumn,
            this.priceDataGridViewTextBoxColumn,
            this.facilityCapacityDataGridViewTextBoxColumn,
            this.ruleUnitsDataGridViewTextBoxColumn,
            this.totalUnitsDataGridViewTextBoxColumn,
            this.bookedUnitsDataGridViewTextBoxColumn,
            this.availableUnitsDataGridViewTextBoxColumn,
            this.scheduleNameDataGridViewTextBoxColumn,
            this.desiredUnitsDataGridViewTextBoxColumn,
            this.expiryDateDataGridViewTextBoxColumn,
            this.categoryIdDataGridViewTextBoxColumn,
            this.promotionIdDataGridViewTextBoxColumn,
            this.seatsDataGridViewTextBoxColumn,
            this.facilityMapDTODataGridViewTextBoxColumn});
            this.dgvSearchSchedules.DataSource = this.scheduleDetailsDTOBindingSource;
            this.dgvSearchSchedules.Location = new System.Drawing.Point(15, 17);
            this.dgvSearchSchedules.MultiSelect = false;
            this.dgvSearchSchedules.Name = "dgvSearchSchedules";
            this.dgvSearchSchedules.RowHeadersVisible = false;
            this.dgvSearchSchedules.RowTemplate.Height = 30;
            this.dgvSearchSchedules.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvSearchSchedules.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSearchSchedules.Size = new System.Drawing.Size(894, 323);
            this.dgvSearchSchedules.TabIndex = 113;
            this.dgvSearchSchedules.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSearchSchedules_CellClick);
            this.dgvSearchSchedules.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvSearchSchedules_CellFormatting);
            this.dgvSearchSchedules.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSearchSchedules_CellValueChanged);
            this.dgvSearchSchedules.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvSearchSchedules_DataError);
            // 
            // SelectSchedule
            // 
            this.SelectSchedule.Frozen = true;
            this.SelectSchedule.HeaderText = "Select";
            this.SelectSchedule.MinimumWidth = 60;
            this.SelectSchedule.Name = "SelectSchedule";
            this.SelectSchedule.ReadOnly = true;
            this.SelectSchedule.Text = "...";
            this.SelectSchedule.UseColumnTextForButtonValue = true;
            this.SelectSchedule.Width = 60;
            // 
            // facilityMapIdDataGridViewTextBoxColumn
            // 
            this.facilityMapIdDataGridViewTextBoxColumn.HeaderText = "facilityMapId";
            this.facilityMapIdDataGridViewTextBoxColumn.Name = "facilityMapIdDataGridViewTextBoxColumn";
            this.facilityMapIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.facilityMapIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // facilityMapNameDataGridViewTextBoxColumn
            // 
            this.facilityMapNameDataGridViewTextBoxColumn.DataPropertyName = "FacilityMapName";
            this.facilityMapNameDataGridViewTextBoxColumn.HeaderText = "Facility Map";
            this.facilityMapNameDataGridViewTextBoxColumn.MinimumWidth = 230;
            this.facilityMapNameDataGridViewTextBoxColumn.Name = "facilityMapNameDataGridViewTextBoxColumn";
            this.facilityMapNameDataGridViewTextBoxColumn.ReadOnly = true;
            this.facilityMapNameDataGridViewTextBoxColumn.Width = 230;
            // 
            // cmbFromTime
            // 
            this.cmbFromTime.HeaderText = "From Time";
            this.cmbFromTime.MinimumWidth = 100;
            this.cmbFromTime.Name = "cmbFromTime";
            this.cmbFromTime.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.cmbFromTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // cmbToTime
            // 
            this.cmbToTime.HeaderText = "To Time";
            this.cmbToTime.MinimumWidth = 100;
            this.cmbToTime.Name = "cmbToTime";
            this.cmbToTime.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.cmbToTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // masterScheduleIdDataGridViewTextBoxColumn
            // 
            this.masterScheduleIdDataGridViewTextBoxColumn.DataPropertyName = "MasterScheduleId";
            this.masterScheduleIdDataGridViewTextBoxColumn.HeaderText = "Master Schedule Id";
            this.masterScheduleIdDataGridViewTextBoxColumn.Name = "masterScheduleIdDataGridViewTextBoxColumn";
            this.masterScheduleIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // masterScheduleNameDataGridViewTextBoxColumn
            // 
            this.masterScheduleNameDataGridViewTextBoxColumn.DataPropertyName = "MasterScheduleName";
            this.masterScheduleNameDataGridViewTextBoxColumn.HeaderText = "Master Schedule Name";
            this.masterScheduleNameDataGridViewTextBoxColumn.Name = "masterScheduleNameDataGridViewTextBoxColumn";
            this.masterScheduleNameDataGridViewTextBoxColumn.Visible = false;
            // 
            // scheduleIdDataGridViewTextBoxColumn
            // 
            this.scheduleIdDataGridViewTextBoxColumn.DataPropertyName = "ScheduleId";
            this.scheduleIdDataGridViewTextBoxColumn.HeaderText = "Schedule Id";
            this.scheduleIdDataGridViewTextBoxColumn.Name = "scheduleIdDataGridViewTextBoxColumn";
            this.scheduleIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // ScheduleFromDate
            // 
            this.ScheduleFromDate.DataPropertyName = "ScheduleFromDate";
            this.ScheduleFromDate.HeaderText = "Schedule From Date";
            this.ScheduleFromDate.Name = "ScheduleFromDate";
            this.ScheduleFromDate.ReadOnly = true;
            this.ScheduleFromDate.Visible = false;
            // 
            // ScheduleToDate
            // 
            this.ScheduleToDate.DataPropertyName = "ScheduleToDate";
            this.ScheduleToDate.HeaderText = "Schedule To Date";
            this.ScheduleToDate.Name = "ScheduleToDate";
            this.ScheduleToDate.ReadOnly = true;
            this.ScheduleToDate.Visible = false;
            // 
            // ScheduleFromTime
            // 
            this.ScheduleFromTime.DataPropertyName = "ScheduleFromTime";
            this.ScheduleFromTime.HeaderText = "ScheduleFromTime";
            this.ScheduleFromTime.Name = "ScheduleFromTime";
            this.ScheduleFromTime.ReadOnly = true;
            this.ScheduleFromTime.Visible = false;
            // 
            // scheduleToTime
            // 
            this.scheduleToTime.DataPropertyName = "ScheduleToTime";
            this.scheduleToTime.HeaderText = "ScheduleToTime";
            this.scheduleToTime.Name = "scheduleToTime";
            this.scheduleToTime.ReadOnly = true;
            this.scheduleToTime.Visible = false;
            // 
            // FixedSchedule
            // 
            this.FixedSchedule.DataPropertyName = "FixedSchedule";
            this.FixedSchedule.HeaderText = "Fixed";
            this.FixedSchedule.MinimumWidth = 50;
            this.FixedSchedule.Name = "FixedSchedule";
            this.FixedSchedule.ReadOnly = true;
            this.FixedSchedule.Width = 50;
            // 
            // attractionPlayIdDataGridViewTextBoxColumn
            // 
            this.attractionPlayIdDataGridViewTextBoxColumn.DataPropertyName = "AttractionPlayId";
            this.attractionPlayIdDataGridViewTextBoxColumn.HeaderText = "Attraction Play Id";
            this.attractionPlayIdDataGridViewTextBoxColumn.Name = "attractionPlayIdDataGridViewTextBoxColumn";
            this.attractionPlayIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.attractionPlayIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // attractionPlayNameDataGridViewTextBoxColumn
            // 
            this.attractionPlayNameDataGridViewTextBoxColumn.DataPropertyName = "AttractionPlayName";
            this.attractionPlayNameDataGridViewTextBoxColumn.HeaderText = "Play Name";
            this.attractionPlayNameDataGridViewTextBoxColumn.Name = "attractionPlayNameDataGridViewTextBoxColumn";
            this.attractionPlayNameDataGridViewTextBoxColumn.ReadOnly = true;
            this.attractionPlayNameDataGridViewTextBoxColumn.Visible = false;
            // 
            // attractionPlayPriceDataGridViewTextBoxColumn
            // 
            this.attractionPlayPriceDataGridViewTextBoxColumn.DataPropertyName = "AttractionPlayPrice";
            this.attractionPlayPriceDataGridViewTextBoxColumn.HeaderText = "AttractionPlayPrice";
            this.attractionPlayPriceDataGridViewTextBoxColumn.Name = "attractionPlayPriceDataGridViewTextBoxColumn";
            this.attractionPlayPriceDataGridViewTextBoxColumn.ReadOnly = true;
            this.attractionPlayPriceDataGridViewTextBoxColumn.Visible = false;
            // 
            // productIdDataGridViewTextBoxColumn
            // 
            this.productIdDataGridViewTextBoxColumn.DataPropertyName = "ProductId";
            this.productIdDataGridViewTextBoxColumn.HeaderText = "Product Id";
            this.productIdDataGridViewTextBoxColumn.Name = "productIdDataGridViewTextBoxColumn";
            this.productIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // productNameDataGridViewTextBoxColumn
            // 
            this.productNameDataGridViewTextBoxColumn.DataPropertyName = "ProductName";
            this.productNameDataGridViewTextBoxColumn.HeaderText = "Product Name";
            this.productNameDataGridViewTextBoxColumn.Name = "productNameDataGridViewTextBoxColumn";
            this.productNameDataGridViewTextBoxColumn.ReadOnly = true;
            this.productNameDataGridViewTextBoxColumn.Visible = false;
            // 
            // priceDataGridViewTextBoxColumn
            // 
            this.priceDataGridViewTextBoxColumn.DataPropertyName = "Price";
            this.priceDataGridViewTextBoxColumn.HeaderText = "Price";
            this.priceDataGridViewTextBoxColumn.Name = "priceDataGridViewTextBoxColumn";
            this.priceDataGridViewTextBoxColumn.ReadOnly = true;
            this.priceDataGridViewTextBoxColumn.Visible = false;
            // 
            // facilityCapacityDataGridViewTextBoxColumn
            // 
            this.facilityCapacityDataGridViewTextBoxColumn.DataPropertyName = "FacilityCapacity";
            this.facilityCapacityDataGridViewTextBoxColumn.HeaderText = "Facility Capacity";
            this.facilityCapacityDataGridViewTextBoxColumn.Name = "facilityCapacityDataGridViewTextBoxColumn";
            this.facilityCapacityDataGridViewTextBoxColumn.ReadOnly = true;
            this.facilityCapacityDataGridViewTextBoxColumn.Visible = false;
            // 
            // ruleUnitsDataGridViewTextBoxColumn
            // 
            this.ruleUnitsDataGridViewTextBoxColumn.DataPropertyName = "RuleUnits";
            this.ruleUnitsDataGridViewTextBoxColumn.HeaderText = "Rule Units";
            this.ruleUnitsDataGridViewTextBoxColumn.Name = "ruleUnitsDataGridViewTextBoxColumn";
            this.ruleUnitsDataGridViewTextBoxColumn.ReadOnly = true;
            this.ruleUnitsDataGridViewTextBoxColumn.Visible = false;
            // 
            // totalUnitsDataGridViewTextBoxColumn
            // 
            this.totalUnitsDataGridViewTextBoxColumn.DataPropertyName = "TotalUnits";
            this.totalUnitsDataGridViewTextBoxColumn.HeaderText = "Total Units";
            this.totalUnitsDataGridViewTextBoxColumn.Name = "totalUnitsDataGridViewTextBoxColumn";
            this.totalUnitsDataGridViewTextBoxColumn.ReadOnly = true;
            this.totalUnitsDataGridViewTextBoxColumn.Visible = false;
            // 
            // bookedUnitsDataGridViewTextBoxColumn
            // 
            this.bookedUnitsDataGridViewTextBoxColumn.DataPropertyName = "BookedUnits";
            this.bookedUnitsDataGridViewTextBoxColumn.HeaderText = "Booked Units";
            this.bookedUnitsDataGridViewTextBoxColumn.Name = "bookedUnitsDataGridViewTextBoxColumn";
            this.bookedUnitsDataGridViewTextBoxColumn.ReadOnly = true;
            this.bookedUnitsDataGridViewTextBoxColumn.Visible = false;
            // 
            // availableUnitsDataGridViewTextBoxColumn
            // 
            this.availableUnitsDataGridViewTextBoxColumn.DataPropertyName = "AvailableUnits";
            this.availableUnitsDataGridViewTextBoxColumn.HeaderText = "Available Units";
            this.availableUnitsDataGridViewTextBoxColumn.MinimumWidth = 70;
            this.availableUnitsDataGridViewTextBoxColumn.Name = "availableUnitsDataGridViewTextBoxColumn";
            this.availableUnitsDataGridViewTextBoxColumn.ReadOnly = true;
            this.availableUnitsDataGridViewTextBoxColumn.Width = 70;
            // 
            // scheduleNameDataGridViewTextBoxColumn
            // 
            this.scheduleNameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.scheduleNameDataGridViewTextBoxColumn.DataPropertyName = "ScheduleName";
            this.scheduleNameDataGridViewTextBoxColumn.HeaderText = "Schedule Name";
            this.scheduleNameDataGridViewTextBoxColumn.MinimumWidth = 230;
            this.scheduleNameDataGridViewTextBoxColumn.Name = "scheduleNameDataGridViewTextBoxColumn";
            this.scheduleNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // desiredUnitsDataGridViewTextBoxColumn
            // 
            this.desiredUnitsDataGridViewTextBoxColumn.DataPropertyName = "DesiredUnits";
            this.desiredUnitsDataGridViewTextBoxColumn.HeaderText = "Desired Units";
            this.desiredUnitsDataGridViewTextBoxColumn.Name = "desiredUnitsDataGridViewTextBoxColumn";
            this.desiredUnitsDataGridViewTextBoxColumn.ReadOnly = true;
            this.desiredUnitsDataGridViewTextBoxColumn.Visible = false;
            // 
            // expiryDateDataGridViewTextBoxColumn
            // 
            this.expiryDateDataGridViewTextBoxColumn.DataPropertyName = "ExpiryDate";
            this.expiryDateDataGridViewTextBoxColumn.HeaderText = "ExpiryDate";
            this.expiryDateDataGridViewTextBoxColumn.Name = "expiryDateDataGridViewTextBoxColumn";
            this.expiryDateDataGridViewTextBoxColumn.ReadOnly = true;
            this.expiryDateDataGridViewTextBoxColumn.Visible = false;
            // 
            // categoryIdDataGridViewTextBoxColumn
            // 
            this.categoryIdDataGridViewTextBoxColumn.DataPropertyName = "CategoryId";
            this.categoryIdDataGridViewTextBoxColumn.HeaderText = "Category Id";
            this.categoryIdDataGridViewTextBoxColumn.Name = "categoryIdDataGridViewTextBoxColumn";
            this.categoryIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.categoryIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // promotionIdDataGridViewTextBoxColumn
            // 
            this.promotionIdDataGridViewTextBoxColumn.DataPropertyName = "PromotionId";
            this.promotionIdDataGridViewTextBoxColumn.HeaderText = "Promotion Id";
            this.promotionIdDataGridViewTextBoxColumn.Name = "promotionIdDataGridViewTextBoxColumn";
            this.promotionIdDataGridViewTextBoxColumn.ReadOnly = true;
            this.promotionIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // seatsDataGridViewTextBoxColumn
            // 
            this.seatsDataGridViewTextBoxColumn.DataPropertyName = "Seats";
            this.seatsDataGridViewTextBoxColumn.HeaderText = "Seats";
            this.seatsDataGridViewTextBoxColumn.Name = "seatsDataGridViewTextBoxColumn";
            this.seatsDataGridViewTextBoxColumn.ReadOnly = true;
            this.seatsDataGridViewTextBoxColumn.Visible = false;
            // 
            // facilityMapDTODataGridViewTextBoxColumn
            // 
            this.facilityMapDTODataGridViewTextBoxColumn.DataPropertyName = "FacilityMapDTO";
            this.facilityMapDTODataGridViewTextBoxColumn.HeaderText = "FacilityMapDTO";
            this.facilityMapDTODataGridViewTextBoxColumn.Name = "facilityMapDTODataGridViewTextBoxColumn";
            this.facilityMapDTODataGridViewTextBoxColumn.ReadOnly = true;
            this.facilityMapDTODataGridViewTextBoxColumn.Visible = false;
            // 
            // scheduleDetailsDTOBindingSource
            // 
            this.scheduleDetailsDTOBindingSource.DataSource = typeof(Semnox.Parafait.Product.ScheduleDetailsDTO);
            // 
            // searchedScheduleHScrollBar
            // 
            this.searchedScheduleHScrollBar.AutoHide = false;
            this.searchedScheduleHScrollBar.DataGridView = this.dgvSearchSchedules;
            this.searchedScheduleHScrollBar.LeftButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("searchedScheduleHScrollBar.LeftButtonBackgroundImage")));
            this.searchedScheduleHScrollBar.LeftButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("searchedScheduleHScrollBar.LeftButtonDisabledBackgroundImage")));
            this.searchedScheduleHScrollBar.Location = new System.Drawing.Point(16, 343);
            this.searchedScheduleHScrollBar.Margin = new System.Windows.Forms.Padding(0);
            this.searchedScheduleHScrollBar.Name = "searchedScheduleHScrollBar";
            this.searchedScheduleHScrollBar.RightButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("searchedScheduleHScrollBar.RightButtonBackgroundImage")));
            this.searchedScheduleHScrollBar.RightButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("searchedScheduleHScrollBar.RightButtonDisabledBackgroundImage")));
            this.searchedScheduleHScrollBar.ScrollableControl = null;
            this.searchedScheduleHScrollBar.ScrollViewer = null;
            this.searchedScheduleHScrollBar.Size = new System.Drawing.Size(893, 46);
            this.searchedScheduleHScrollBar.TabIndex = 115;
            this.searchedScheduleHScrollBar.LeftButtonClick += new System.EventHandler(this.Scroll_ButtonClick);
            this.searchedScheduleHScrollBar.RightButtonClick += new System.EventHandler(this.Scroll_ButtonClick);
            // 
            // cmbSearchFacility
            // 
            this.cmbSearchFacility.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbSearchFacility.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbSearchFacility.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbSearchFacility.FormattingEnabled = true;
            this.cmbSearchFacility.Location = new System.Drawing.Point(87, 58);
            this.cmbSearchFacility.Name = "cmbSearchFacility";
            this.cmbSearchFacility.Size = new System.Drawing.Size(208, 31);
            this.cmbSearchFacility.TabIndex = 107;
            this.cmbSearchFacility.SelectedIndexChanged += new System.EventHandler(this.cmbSearchFacilityProd_SelectedIndexChanged);
            // 
            // lblFacility
            // 
            this.lblFacility.Font = new System.Drawing.Font("Arial", 9F);
            this.lblFacility.Location = new System.Drawing.Point(7, 58);
            this.lblFacility.Name = "lblFacility";
            this.lblFacility.Size = new System.Drawing.Size(77, 35);
            this.lblFacility.TabIndex = 110;
            this.lblFacility.Text = "Facility Map:";
            this.lblFacility.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTimeSlots
            // 
            this.lblTimeSlots.Font = new System.Drawing.Font("Arial", 9F);
            this.lblTimeSlots.Location = new System.Drawing.Point(355, 16);
            this.lblTimeSlots.Name = "lblTimeSlots";
            this.lblTimeSlots.Size = new System.Drawing.Size(153, 35);
            this.lblTimeSlots.TabIndex = 102;
            this.lblTimeSlots.Text = "Preferred Time Slots:";
            this.lblTimeSlots.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbxNightSlot
            // 
            this.cbxNightSlot.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbxNightSlot.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.cbxNightSlot.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbxNightSlot.FlatAppearance.BorderSize = 0;
            this.cbxNightSlot.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.cbxNightSlot.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.cbxNightSlot.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.cbxNightSlot.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbxNightSlot.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbxNightSlot.ImageIndex = 1;
            this.cbxNightSlot.Location = new System.Drawing.Point(817, 13);
            this.cbxNightSlot.Name = "cbxNightSlot";
            this.cbxNightSlot.Size = new System.Drawing.Size(93, 42);
            this.cbxNightSlot.TabIndex = 101;
            this.cbxNightSlot.Text = "18 - 00";
            this.cbxNightSlot.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbxNightSlot.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.cbxNightSlot.UseVisualStyleBackColor = true;
            this.cbxNightSlot.CheckedChanged += new System.EventHandler(this.cbxTimeSlots_CheckedChanged);
            // 
            // cbxAfternoonSlot
            // 
            this.cbxAfternoonSlot.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbxAfternoonSlot.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.cbxAfternoonSlot.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbxAfternoonSlot.FlatAppearance.BorderSize = 0;
            this.cbxAfternoonSlot.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.cbxAfternoonSlot.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.cbxAfternoonSlot.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.cbxAfternoonSlot.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbxAfternoonSlot.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbxAfternoonSlot.ImageIndex = 1;
            this.cbxAfternoonSlot.Location = new System.Drawing.Point(716, 13);
            this.cbxAfternoonSlot.Name = "cbxAfternoonSlot";
            this.cbxAfternoonSlot.Size = new System.Drawing.Size(93, 42);
            this.cbxAfternoonSlot.TabIndex = 100;
            this.cbxAfternoonSlot.Text = "12 - 18";
            this.cbxAfternoonSlot.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbxAfternoonSlot.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.cbxAfternoonSlot.UseVisualStyleBackColor = true;
            this.cbxAfternoonSlot.CheckedChanged += new System.EventHandler(this.cbxTimeSlots_CheckedChanged);
            // 
            // cbxMorningSlot
            // 
            this.cbxMorningSlot.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbxMorningSlot.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.cbxMorningSlot.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbxMorningSlot.FlatAppearance.BorderSize = 0;
            this.cbxMorningSlot.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.cbxMorningSlot.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.cbxMorningSlot.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.cbxMorningSlot.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbxMorningSlot.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbxMorningSlot.ImageIndex = 1;
            this.cbxMorningSlot.Location = new System.Drawing.Point(616, 13);
            this.cbxMorningSlot.Name = "cbxMorningSlot";
            this.cbxMorningSlot.Size = new System.Drawing.Size(93, 42);
            this.cbxMorningSlot.TabIndex = 99;
            this.cbxMorningSlot.Text = "06 - 12";
            this.cbxMorningSlot.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbxMorningSlot.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.cbxMorningSlot.UseVisualStyleBackColor = true;
            this.cbxMorningSlot.CheckedChanged += new System.EventHandler(this.cbxTimeSlots_CheckedChanged);
            // 
            // cbxEarlyMorningSlot
            // 
            this.cbxEarlyMorningSlot.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbxEarlyMorningSlot.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.cbxEarlyMorningSlot.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbxEarlyMorningSlot.FlatAppearance.BorderSize = 0;
            this.cbxEarlyMorningSlot.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.cbxEarlyMorningSlot.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.cbxEarlyMorningSlot.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.cbxEarlyMorningSlot.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbxEarlyMorningSlot.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbxEarlyMorningSlot.ImageIndex = 1;
            this.cbxEarlyMorningSlot.Location = new System.Drawing.Point(516, 13);
            this.cbxEarlyMorningSlot.Name = "cbxEarlyMorningSlot";
            this.cbxEarlyMorningSlot.Size = new System.Drawing.Size(93, 42);
            this.cbxEarlyMorningSlot.TabIndex = 98;
            this.cbxEarlyMorningSlot.Text = "00 - 06";
            this.cbxEarlyMorningSlot.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbxEarlyMorningSlot.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.cbxEarlyMorningSlot.UseVisualStyleBackColor = true;
            this.cbxEarlyMorningSlot.CheckedChanged += new System.EventHandler(this.cbxTimeSlots_CheckedChanged);
            // 
            // cmbSearchToTime
            // 
            this.cmbSearchToTime.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbSearchToTime.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbSearchToTime.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbSearchToTime.FormattingEnabled = true;
            this.cmbSearchToTime.Location = new System.Drawing.Point(772, 56);
            this.cmbSearchToTime.Name = "cmbSearchToTime";
            this.cmbSearchToTime.Size = new System.Drawing.Size(135, 31);
            this.cmbSearchToTime.TabIndex = 95;
            this.cmbSearchToTime.SelectedIndexChanged += new System.EventHandler(this.cmbSearchFromTimeORToTime_SelectedIndexChanged);
            // 
            // cmbSearchFromTime
            // 
            this.cmbSearchFromTime.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbSearchFromTime.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbSearchFromTime.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbSearchFromTime.FormattingEnabled = true;
            this.cmbSearchFromTime.Location = new System.Drawing.Point(513, 56);
            this.cmbSearchFromTime.Name = "cmbSearchFromTime";
            this.cmbSearchFromTime.Size = new System.Drawing.Size(135, 31);
            this.cmbSearchFromTime.TabIndex = 93;
            this.cmbSearchFromTime.SelectedIndexChanged += new System.EventHandler(this.cmbSearchFromTimeORToTime_SelectedIndexChanged);
            // 
            // lblFromTimeSearch
            // 
            this.lblFromTimeSearch.Font = new System.Drawing.Font("Arial", 9F);
            this.lblFromTimeSearch.Location = new System.Drawing.Point(358, 56);
            this.lblFromTimeSearch.Name = "lblFromTimeSearch";
            this.lblFromTimeSearch.Size = new System.Drawing.Size(150, 35);
            this.lblFromTimeSearch.TabIndex = 96;
            this.lblFromTimeSearch.Text = "From Time:";
            this.lblFromTimeSearch.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblToTimeSearch
            // 
            this.lblToTimeSearch.Font = new System.Drawing.Font("Arial", 9F);
            this.lblToTimeSearch.Location = new System.Drawing.Point(654, 56);
            this.lblToTimeSearch.Name = "lblToTimeSearch";
            this.lblToTimeSearch.Size = new System.Drawing.Size(114, 35);
            this.lblToTimeSearch.TabIndex = 97;
            this.lblToTimeSearch.Text = "To Time:";
            this.lblToTimeSearch.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpSearchDate
            // 
            this.dtpSearchDate.CustomFormat = "ddd, dd-MMM-yyyy";
            this.dtpSearchDate.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpSearchDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpSearchDate.Location = new System.Drawing.Point(87, 16);
            this.dtpSearchDate.Name = "dtpSearchDate";
            this.dtpSearchDate.Size = new System.Drawing.Size(249, 30);
            this.dtpSearchDate.TabIndex = 92;
            this.dtpSearchDate.ValueChanged += new System.EventHandler(this.dtpSearchDate_ValueChanged);
            this.dtpSearchDate.Enter += new System.EventHandler(this.dtpSearchDate_Enter);
            // 
            // lblSelectedDate
            // 
            this.lblSelectedDate.Font = new System.Drawing.Font("Arial", 9F);
            this.lblSelectedDate.Location = new System.Drawing.Point(17, 16);
            this.lblSelectedDate.Name = "lblSelectedDate";
            this.lblSelectedDate.Size = new System.Drawing.Size(66, 35);
            this.lblSelectedDate.TabIndex = 94;
            this.lblSelectedDate.Text = "For Date:";
            this.lblSelectedDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblBookingProductLabel
            // 
            this.lblBookingProductLabel.Font = new System.Drawing.Font("Arial", 9F);
            this.lblBookingProductLabel.Location = new System.Drawing.Point(10, 0);
            this.lblBookingProductLabel.Name = "lblBookingProductLabel";
            this.lblBookingProductLabel.Size = new System.Drawing.Size(117, 35);
            this.lblBookingProductLabel.TabIndex = 108;
            this.lblBookingProductLabel.Text = "Booking Product:";
            this.lblBookingProductLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblScheduleLabel
            // 
            this.lblScheduleLabel.Font = new System.Drawing.Font("Arial", 9F);
            this.lblScheduleLabel.Location = new System.Drawing.Point(10, 33);
            this.lblScheduleLabel.Name = "lblScheduleLabel";
            this.lblScheduleLabel.Size = new System.Drawing.Size(117, 35);
            this.lblScheduleLabel.TabIndex = 109;
            this.lblScheduleLabel.Text = "Booked Schedule:";
            this.lblScheduleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblBookingProductName
            // 
            this.lblBookingProductName.AutoSize = true;
            this.lblBookingProductName.Location = new System.Drawing.Point(126, 11);
            this.lblBookingProductName.Name = "lblBookingProductName";
            this.lblBookingProductName.Size = new System.Drawing.Size(97, 15);
            this.lblBookingProductName.TabIndex = 110;
            this.lblBookingProductName.Text = "Booking Product";
            this.lblBookingProductName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblBookedScheduleDetails
            // 
            this.lblBookedScheduleDetails.AutoSize = true;
            this.lblBookedScheduleDetails.Location = new System.Drawing.Point(126, 43);
            this.lblBookedScheduleDetails.Name = "lblBookedScheduleDetails";
            this.lblBookedScheduleDetails.Size = new System.Drawing.Size(107, 15);
            this.lblBookedScheduleDetails.TabIndex = 111;
            this.lblBookedScheduleDetails.Text = "Booking Schedule";
            this.lblBookedScheduleDetails.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(421, 591);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(116, 34);
            this.btnCancel.TabIndex = 121;
            this.btnCancel.Text = "Close";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            this.btnCancel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnCancel_MouseDown);
            this.btnCancel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnCancel_MouseUp);
            // 
            // lblQty
            // 
            this.lblQty.Font = new System.Drawing.Font("Arial", 9F);
            this.lblQty.Location = new System.Drawing.Point(435, 1);
            this.lblQty.Name = "lblQty";
            this.lblQty.Size = new System.Drawing.Size(117, 35);
            this.lblQty.TabIndex = 122;
            this.lblQty.Text = "Guest Quantity:";
            this.lblQty.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblQtyVallue
            // 
            this.lblQtyVallue.AutoSize = true;
            this.lblQtyVallue.Location = new System.Drawing.Point(551, 12);
            this.lblQtyVallue.Name = "lblQtyVallue";
            this.lblQtyVallue.Size = new System.Drawing.Size(14, 15);
            this.lblQtyVallue.TabIndex = 123;
            this.lblQtyVallue.Text = "0";
            this.lblQtyVallue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblOldFacilityMap
            // 
            this.lblOldFacilityMap.Font = new System.Drawing.Font("Arial", 9F);
            this.lblOldFacilityMap.Location = new System.Drawing.Point(435, 33);
            this.lblOldFacilityMap.Name = "lblOldFacilityMap";
            this.lblOldFacilityMap.Size = new System.Drawing.Size(117, 35);
            this.lblOldFacilityMap.TabIndex = 122;
            this.lblOldFacilityMap.Text = "Facility Map:";
            this.lblOldFacilityMap.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblOldFacilityMapValue
            // 
            this.lblOldFacilityMapValue.AutoSize = true;
            this.lblOldFacilityMapValue.Location = new System.Drawing.Point(551, 43);
            this.lblOldFacilityMapValue.Name = "lblOldFacilityMapValue";
            this.lblOldFacilityMapValue.Size = new System.Drawing.Size(44, 15);
            this.lblOldFacilityMapValue.TabIndex = 123;
            this.lblOldFacilityMapValue.Text = "Facility";
            this.lblOldFacilityMapValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // frmRescheduleReservationUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(996, 637);
            this.Controls.Add(this.lblQty);
            this.Controls.Add(this.lblQtyVallue);
            this.Controls.Add(this.lblOldFacilityMap);
            this.Controls.Add(this.lblOldFacilityMapValue);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblBookingProductLabel);
            this.Controls.Add(this.lblBookedScheduleDetails);
            this.Controls.Add(this.lblBookingProductName);
            this.Controls.Add(this.lblScheduleLabel);
            this.Controls.Add(this.gpbSearchOptions);
            this.Font = new System.Drawing.Font("Arial", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmRescheduleReservationUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Reschedule Reservation";
            this.Deactivate += new System.EventHandler(this.frmRescheduleReservationUI_Deactivate);
            this.Load += new System.EventHandler(this.frmRescheduleReservationUI_Load);
            this.gpbSearchOptions.ResumeLayout(false);
            this.gpbSearchOptions.PerformLayout();
            this.gpbFacilityProducts.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSearchSchedules)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.scheduleDetailsDTOBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gpbSearchOptions;
        private System.Windows.Forms.GroupBox gpbFacilityProducts;
        private System.Windows.Forms.DataGridView dgvSearchSchedules;
        private Semnox.Core.GenericUtilities.HorizontalScrollBarView searchedScheduleHScrollBar;
        private Semnox.Core.GenericUtilities.VerticalScrollBarView searchedScheduleVScrollBar;
        private Semnox.Core.GenericUtilities.AutoCompleteComboBox cmbSearchFacility;
        private System.Windows.Forms.Label lblFacility;
        private System.Windows.Forms.Label lblTimeSlots;
        private Semnox.Core.GenericUtilities.CustomCheckBox cbxNightSlot;
        private Semnox.Core.GenericUtilities.CustomCheckBox cbxAfternoonSlot;
        private Semnox.Core.GenericUtilities.CustomCheckBox cbxMorningSlot;
        private Semnox.Core.GenericUtilities.CustomCheckBox cbxEarlyMorningSlot;
        private Semnox.Core.GenericUtilities.AutoCompleteComboBox cmbSearchToTime;
        private Semnox.Core.GenericUtilities.AutoCompleteComboBox cmbSearchFromTime;
        private System.Windows.Forms.Label lblFromTimeSearch;
        private System.Windows.Forms.Label lblToTimeSearch;
        private System.Windows.Forms.DateTimePicker dtpSearchDate;
        private System.Windows.Forms.Label lblSelectedDate;
        private System.Windows.Forms.Label lblBookingProductLabel;
        private System.Windows.Forms.Label lblScheduleLabel;
        private System.Windows.Forms.Label lblBookingProductName;
        private System.Windows.Forms.Label lblBookedScheduleDetails;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.BindingSource scheduleDetailsDTOBindingSource; 
        private System.Windows.Forms.DataGridViewButtonColumn SelectSchedule;     
        private System.Windows.Forms.DataGridViewComboBoxColumn cmbFromTime;
        private System.Windows.Forms.DataGridViewComboBoxColumn cmbToTime; 
        private System.Windows.Forms.DataGridViewTextBoxColumn facilityMapIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn facilityMapNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn masterScheduleIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn masterScheduleNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn scheduleIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ScheduleFromDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn ScheduleToDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn scheduleFromTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn scheduleToTime;
        private System.Windows.Forms.DataGridViewCheckBoxColumn fixedSchedule;
        private System.Windows.Forms.DataGridViewTextBoxColumn attractionPlayIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn attractionPlayNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn attractionPlayPriceDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn productIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn productNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn priceDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn facilityCapacityDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ruleUnitsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn totalUnitsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn bookedUnitsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn availableUnitsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn scheduleNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn desiredUnitsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn expiryDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn categoryIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn promotionIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn seatsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn facilityMapDTODataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ScheduleFromTime;
        private System.Windows.Forms.DataGridViewCheckBoxColumn FixedSchedule;
        private System.Windows.Forms.Label lblQty;
        private System.Windows.Forms.Label lblQtyVallue;
        private System.Windows.Forms.Label lblOldFacilityMap;
        private System.Windows.Forms.Label lblOldFacilityMapValue;
    }
}