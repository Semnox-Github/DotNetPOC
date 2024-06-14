using System;

namespace Parafait_POS.Reservation
{
    partial class frmReservationUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmReservationUI));
            this.tcBooking = new System.Windows.Forms.TabControl();
            this.tpDateTime = new System.Windows.Forms.TabPage();
            this.gpbSelectedOptions = new System.Windows.Forms.GroupBox();
            this.txtGuestQty = new System.Windows.Forms.TextBox();
            this.lblGuestQty = new System.Windows.Forms.Label();
            this.btnShowKeyPad = new System.Windows.Forms.Button();
            this.selectedScheduleVScrollBar = new Semnox.Core.GenericUtilities.VerticalScrollBarView();
            this.dgvSelectedBookingSchedule = new System.Windows.Forms.DataGridView();
            this.cancelledDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.isChangedDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ReservationDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.trxReservationScheduleIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.trxIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lineIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.trxLineProductIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.scheduleNameDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.scheduleFromDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.scheduleToDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.guestQuantityDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.facilityMapNameDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.trxLineProductNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.schedulesIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.facilityMapIdDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cancelledByDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmbFromTime = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.cmbToTime = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.RemoveLine = new System.Windows.Forms.DataGridViewButtonColumn();
            this.BookingProductName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BookingProductId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BookingProductTrxLine = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FacilityTrxLine = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.guestQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Reschedule = new System.Windows.Forms.DataGridViewImageColumn();
            this.transactionReservationScheduleDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnNextDateTime = new System.Windows.Forms.Button();
            this.btnClearBooking = new System.Windows.Forms.Button();
            this.btnBlockSchedule = new System.Windows.Forms.Button();
            this.cmbChannel = new Semnox.Core.GenericUtilities.AutoCompleteComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtBookingName = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.gpbSearchOptions = new System.Windows.Forms.GroupBox();
            this.cbxHideBookedSlots = new Semnox.Core.GenericUtilities.CustomCheckBox();
            this.gpbFacilityProducts = new System.Windows.Forms.GroupBox();
            this.dgvSearchSchedules = new System.Windows.Forms.DataGridView();
            this.facilityMapIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ScheduleFromDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ScheduleToDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.facilityMapNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.masterScheduleIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.scheduleIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.scheduleFromTimeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.scheduleToTimeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fixedScheduleDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.availableUnitsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.scheduleNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.masterScheduleNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.desiredUnitsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.expiryDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.categoryIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.promotionIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.seatsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.facilityMapDTODataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.scheduleDetailsDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.searchedScheduleHScrollBar = new Semnox.Core.GenericUtilities.HorizontalScrollBarView();
            this.searchedScheduleVScrollBar = new Semnox.Core.GenericUtilities.VerticalScrollBarView();
            this.facProdHScrollBar = new Semnox.Core.GenericUtilities.HorizontalScrollBarView();
            this.dgvSearchFacilityProducts = new System.Windows.Forms.DataGridView();
            this.productNameDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProductType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.productIdDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.activeFlagDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.productTypeIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SelectedRecord = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.productInformation = new System.Windows.Forms.DataGridViewButtonColumn();
            this.productsDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.btnAddToBooking = new System.Windows.Forms.Button();
            this.facProdVScrollBar = new Semnox.Core.GenericUtilities.VerticalScrollBarView();
            this.cmbSearchFacility = new Semnox.Core.GenericUtilities.AutoCompleteComboBox();
            this.lblFacility = new System.Windows.Forms.Label();
            this.cmbSearchBookingProduct = new Semnox.Core.GenericUtilities.AutoCompleteComboBox();
            this.label3 = new System.Windows.Forms.Label();
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
            this.tpCustomer = new System.Windows.Forms.TabPage();
            this.custPanelVScrollBar = new Semnox.Core.GenericUtilities.VerticalScrollBarView();
            this.pnlCustomerDetail = new System.Windows.Forms.Panel();
            this.btnCustomerLookup = new System.Windows.Forms.Button();
            this.btnPrevCustomer = new System.Windows.Forms.Button();
            this.btnNextCustomer = new System.Windows.Forms.Button();
            this.txtCardNumber = new System.Windows.Forms.TextBox();
            this.lblCardNumber = new System.Windows.Forms.Label();
            this.tpPackageProducts = new System.Windows.Forms.TabPage();
            this.btnPkgTabShowKeyPad = new System.Windows.Forms.Button();
            this.packageListVScrollBar = new Semnox.Core.GenericUtilities.VerticalScrollBarView();
            this.pnlPackageProducts = new System.Windows.Forms.Panel();
            this.usrCtrlPkgProductDetails1 = new Parafait_POS.Reservation.usrCtrlProductDetails();
            this.btnPrevPackage = new System.Windows.Forms.Button();
            this.btnNextPackage = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtRemarks = new System.Windows.Forms.TextBox();
            this.lblPkgQty = new System.Windows.Forms.Label();
            this.lblPkgProductName = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tpAdditionalProducts = new System.Windows.Forms.TabPage();
            this.verticalScrollBarView3 = new Semnox.Core.GenericUtilities.VerticalScrollBarView();
            this.pnlAdditionalProducts = new System.Windows.Forms.Panel();
            this.usrCtrlAdditionProductDetails1 = new Parafait_POS.Reservation.usrCtrlProductDetails();
            this.btnPrevAdditionalProducts = new System.Windows.Forms.Button();
            this.btnNextAdditionalProducts = new System.Windows.Forms.Button();
            this.lblAdditionalProdQty = new System.Windows.Forms.Label();
            this.lblAdditionalProdPrice = new System.Windows.Forms.Label();
            this.lblAdditionalProdName = new System.Windows.Forms.Label();
            this.tpSummary = new System.Windows.Forms.TabPage();
            this.btnBookingCheckList = new System.Windows.Forms.Button();
            this.btnMapWaivers = new System.Windows.Forms.Button();
            this.btnNextSummary = new System.Windows.Forms.Button();
            this.btnSendEmail = new System.Windows.Forms.Button();
            this.btnPrintBooking = new System.Windows.Forms.Button();
            this.btnSendPaymentLink = new System.Windows.Forms.Button();
            this.btnAddTransactionProfile = new System.Windows.Forms.Button();
            this.btnApplyDiscount = new System.Windows.Forms.Button();
            this.btnApplyDiscCoupon = new System.Windows.Forms.Button();
            this.btnEditBooking = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnPrevSummary = new System.Windows.Forms.Button();
            this.btnBook = new System.Windows.Forms.Button();
            this.btnCancelBooking = new System.Windows.Forms.Button();
            this.btnExecute = new System.Windows.Forms.Button();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.btnPayment = new System.Windows.Forms.Button();
            this.btnAddAttendees = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.gbxCharges = new System.Windows.Forms.GroupBox();
            this.pcbRemoveGratuityAmount = new System.Windows.Forms.PictureBox();
            this.btnApplyGratuityAmount = new System.Windows.Forms.Button();
            this.txtGratuityAmount = new System.Windows.Forms.TextBox();
            this.txtServiceChrageAmount = new System.Windows.Forms.TextBox();
            this.pcbRemoveServiceCharge = new System.Windows.Forms.PictureBox();
            this.lblGratuityAmount = new System.Windows.Forms.Label();
            this.lblServiceChargeAmount = new System.Windows.Forms.Label();
            this.btnApplyServiceCharge = new System.Windows.Forms.Button();
            this.gbxSvcServiceCharge = new System.Windows.Forms.GroupBox();
            this.txtSvcServiceChargePercentage = new System.Windows.Forms.TextBox();
            this.txtSvcServiceChargeAmount = new System.Windows.Forms.TextBox();
            this.pbcSvcRemoveServiceCharge = new System.Windows.Forms.PictureBox();
            this.lblSvcServiceChargePercentage = new System.Windows.Forms.Label();
            this.lblSvcServiceChargeAmount = new System.Windows.Forms.Label();
            this.pcbRemoveDiscCoupon = new System.Windows.Forms.PictureBox();
            this.txtAppliedDiscountCoupon = new System.Windows.Forms.TextBox();
            this.lblAppliedDiscount = new System.Windows.Forms.Label();
            this.lblAppliedDiscountCoupon = new System.Windows.Forms.Label();
            this.txtAppliedDiscount = new System.Windows.Forms.TextBox();
            this.pcbRemoveDiscount = new System.Windows.Forms.PictureBox();
            this.lblAppliedTransactionProfile = new System.Windows.Forms.Label();
            this.txtAppliedTransactionProfile = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtSvcTrxServiceCharges = new System.Windows.Forms.TextBox();
            this.lblSvcServiceChargeSummary = new System.Windows.Forms.Label();
            this.txtDiscountAmount = new System.Windows.Forms.TextBox();
            this.lblDiscountAmount = new System.Windows.Forms.Label();
            this.txtBalanceAmount = new System.Windows.Forms.TextBox();
            this.lblBalanceAmount = new System.Windows.Forms.Label();
            this.txtEstimateAmount = new System.Windows.Forms.TextBox();
            this.lblEstimateAmount = new System.Windows.Forms.Label();
            this.txtAdvancePaid = new System.Windows.Forms.TextBox();
            this.lblPaid = new System.Windows.Forms.Label();
            this.txtMinimumAdvanceAmount = new System.Windows.Forms.TextBox();
            this.lblMinimumAdvAmount = new System.Windows.Forms.Label();
            this.txtTransactionAmount = new System.Windows.Forms.TextBox();
            this.lblTransactionAmount = new System.Windows.Forms.Label();
            this.txtReservationStatus = new System.Windows.Forms.TextBox();
            this.lblReservationStatus = new System.Windows.Forms.Label();
            this.txtExpiryDate = new System.Windows.Forms.TextBox();
            this.lblExpiryDate = new System.Windows.Forms.Label();
            this.txtReservationCode = new System.Windows.Forms.TextBox();
            this.lblReservationCode = new System.Windows.Forms.Label();
            this.cbxEmailSent = new Semnox.Core.GenericUtilities.CustomCheckBox();
            this.tpCheckList = new System.Windows.Forms.TabPage();
            this.btnSaveCheckList = new System.Windows.Forms.Button();
            this.btnPrevCheckList = new System.Windows.Forms.Button();
            this.btnNextCheckList = new System.Windows.Forms.Button();
            this.grpCheckList = new System.Windows.Forms.GroupBox();
            this.cmbEventCheckList = new System.Windows.Forms.ComboBox();
            this.chkListHScrollBar = new Semnox.Core.GenericUtilities.HorizontalScrollBarView();
            this.dgvUserJobTaskList = new System.Windows.Forms.DataGridView();
            this.remarksMandatoryDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.chklistValue = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.validateTagDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.isActiveDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.assignedUserId = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.checkListStatus = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.maintChklstdetId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.maintJobNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.taskNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chklstScheduleTimeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.jobTaskIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.maintJobTypeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.assignedToDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.departmentIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chklistRemarks = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.checklistCloseDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cardIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cardNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.taskCardNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.assetIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.assetNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.assetTypeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.assetGroupNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sourceSystemIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.durationToCompleteDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.requestTypeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.requestDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.priorityDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.requestDetailDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.imageNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.requestedByDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contactPhoneDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contactEmailIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.resolutionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.commentsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.repairCostDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.docFileNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.attribute1DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chkListVScroolBar = new Semnox.Core.GenericUtilities.VerticalScrollBarView();
            this.btnPrintCheckList = new System.Windows.Forms.Button();
            this.lblCheckListName = new System.Windows.Forms.Label();
            this.tpAuditDetails = new System.Windows.Forms.TabPage();
            this.btnPrevAuditTrail = new System.Windows.Forms.Button();
            this.lblAuditTrail = new System.Windows.Forms.Label();
            this.dgvAuditTrail = new System.Windows.Forms.DataGridView();
            this.auditRailHScrollBar = new Semnox.Core.GenericUtilities.HorizontalScrollBarView();
            this.auditTrailVScrollBar = new Semnox.Core.GenericUtilities.VerticalScrollBarView();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.userJobItemsDTOBindingSource = new System.Windows.Forms.BindingSource(this.components);
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
            this.dataGridViewTextBoxColumn18 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn19 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn20 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn21 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn22 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn23 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn24 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn25 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn26 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn27 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn28 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn29 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn30 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn31 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn32 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn33 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn34 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn35 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn36 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn37 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn38 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn39 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn40 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn41 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn42 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn43 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn44 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn45 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn46 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn47 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn48 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn49 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn50 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn51 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn52 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn53 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn54 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn55 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn56 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn57 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn58 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn59 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn60 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn61 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn62 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn63 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn64 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn65 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn66 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn67 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn68 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn69 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn70 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn71 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn72 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn73 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn74 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn75 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn76 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn77 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn78 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn79 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn80 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn81 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn82 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn83 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn84 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn85 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn86 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn87 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn88 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn89 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn90 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn91 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn92 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn93 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn94 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn95 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn96 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn97 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn98 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn99 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn100 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn101 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn102 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn103 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn104 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn105 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn106 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn107 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn108 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn109 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn110 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn111 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn112 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn113 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn114 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn115 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn116 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn117 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn118 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn119 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn120 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn121 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn122 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn123 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn124 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn125 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn126 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn127 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn128 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn129 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn130 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn131 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn132 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn133 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn134 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn135 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn136 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn137 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn138 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn139 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn140 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn141 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn142 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn143 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn144 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn145 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn146 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn147 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn148 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn149 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn150 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn151 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn152 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn153 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn154 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn155 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn156 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn157 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn158 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn159 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn160 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn161 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn162 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn163 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn164 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn165 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn166 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn167 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn168 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn169 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn170 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn171 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn172 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn173 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.inActivityTimerClock = new System.Windows.Forms.Timer(this.components);
            this.tcBooking.SuspendLayout();
            this.tpDateTime.SuspendLayout();
            this.gpbSelectedOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSelectedBookingSchedule)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.transactionReservationScheduleDTOBindingSource)).BeginInit();
            this.gpbSearchOptions.SuspendLayout();
            this.gpbFacilityProducts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSearchSchedules)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.scheduleDetailsDTOBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSearchFacilityProducts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.productsDTOBindingSource)).BeginInit();
            this.tpCustomer.SuspendLayout();
            this.tpPackageProducts.SuspendLayout();
            this.pnlPackageProducts.SuspendLayout();
            this.tpAdditionalProducts.SuspendLayout();
            this.pnlAdditionalProducts.SuspendLayout();
            this.tpSummary.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.gbxCharges.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pcbRemoveGratuityAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcbRemoveServiceCharge)).BeginInit();
            this.gbxSvcServiceCharge.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbcSvcRemoveServiceCharge)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcbRemoveDiscCoupon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcbRemoveDiscount)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.tpCheckList.SuspendLayout();
            this.grpCheckList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUserJobTaskList)).BeginInit();
            this.tpAuditDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAuditTrail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.userJobItemsDTOBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // tcBooking
            // 
            this.tcBooking.Controls.Add(this.tpDateTime);
            this.tcBooking.Controls.Add(this.tpCustomer);
            this.tcBooking.Controls.Add(this.tpPackageProducts);
            this.tcBooking.Controls.Add(this.tpAdditionalProducts);
            this.tcBooking.Controls.Add(this.tpSummary);
            this.tcBooking.Controls.Add(this.tpCheckList);
            this.tcBooking.Controls.Add(this.tpAuditDetails);
            this.tcBooking.ItemSize = new System.Drawing.Size(93, 40);
            this.tcBooking.Location = new System.Drawing.Point(1, -10);
            this.tcBooking.Name = "tcBooking";
            this.tcBooking.SelectedIndex = 0;
            this.tcBooking.Size = new System.Drawing.Size(1304, 680);
            this.tcBooking.TabIndex = 1;
            // 
            // tpDateTime
            // 
            this.tpDateTime.Controls.Add(this.gpbSelectedOptions);
            this.tpDateTime.Controls.Add(this.gpbSearchOptions);
            this.tpDateTime.Location = new System.Drawing.Point(4, 44);
            this.tpDateTime.Name = "tpDateTime";
            this.tpDateTime.Padding = new System.Windows.Forms.Padding(3);
            this.tpDateTime.Size = new System.Drawing.Size(1296, 632);
            this.tpDateTime.TabIndex = 0;
            this.tpDateTime.Text = "Date and Time";
            this.tpDateTime.UseVisualStyleBackColor = true;
            this.tpDateTime.Enter += new System.EventHandler(this.tpDateTime_Enter);
            // 
            // gpbSelectedOptions
            // 
            this.gpbSelectedOptions.Controls.Add(this.txtGuestQty);
            this.gpbSelectedOptions.Controls.Add(this.lblGuestQty);
            this.gpbSelectedOptions.Controls.Add(this.btnShowKeyPad);
            this.gpbSelectedOptions.Controls.Add(this.selectedScheduleVScrollBar);
            this.gpbSelectedOptions.Controls.Add(this.btnCancel);
            this.gpbSelectedOptions.Controls.Add(this.btnNextDateTime);
            this.gpbSelectedOptions.Controls.Add(this.dgvSelectedBookingSchedule);
            this.gpbSelectedOptions.Controls.Add(this.btnClearBooking);
            this.gpbSelectedOptions.Controls.Add(this.btnBlockSchedule);
            this.gpbSelectedOptions.Controls.Add(this.cmbChannel);
            this.gpbSelectedOptions.Controls.Add(this.label11);
            this.gpbSelectedOptions.Controls.Add(this.txtBookingName);
            this.gpbSelectedOptions.Controls.Add(this.label10);
            this.gpbSelectedOptions.Location = new System.Drawing.Point(6, 428);
            this.gpbSelectedOptions.Name = "gpbSelectedOptions";
            this.gpbSelectedOptions.Size = new System.Drawing.Size(1283, 204);
            this.gpbSelectedOptions.TabIndex = 1;
            this.gpbSelectedOptions.TabStop = false;
            this.gpbSelectedOptions.Text = "Selected Option";
            // 
            // txtGuestQty
            // 
            this.txtGuestQty.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtGuestQty.Location = new System.Drawing.Point(649, 16);
            this.txtGuestQty.MaxLength = 5;
            this.txtGuestQty.Name = "txtGuestQty";
            this.txtGuestQty.ReadOnly = true;
            this.txtGuestQty.Size = new System.Drawing.Size(80, 30);
            this.txtGuestQty.TabIndex = 123;
            // 
            // lblGuestQty
            // 
            this.lblGuestQty.Location = new System.Drawing.Point(545, 16);
            this.lblGuestQty.Name = "lblGuestQty";
            this.lblGuestQty.Size = new System.Drawing.Size(102, 30);
            this.lblGuestQty.TabIndex = 122;
            this.lblGuestQty.Text = "Guest Quantity:";
            this.lblGuestQty.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnShowKeyPad
            // 
            this.btnShowKeyPad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnShowKeyPad.BackColor = System.Drawing.Color.Transparent;
            this.btnShowKeyPad.BackgroundImage = global::Parafait_POS.Properties.Resources.keyboard;
            this.btnShowKeyPad.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnShowKeyPad.CausesValidation = false;
            this.btnShowKeyPad.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnShowKeyPad.FlatAppearance.BorderSize = 0;
            this.btnShowKeyPad.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnShowKeyPad.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnShowKeyPad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShowKeyPad.Font = new System.Drawing.Font("Arial", 8.25F);
            this.btnShowKeyPad.ForeColor = System.Drawing.Color.Black;
            this.btnShowKeyPad.Location = new System.Drawing.Point(1119, 160);
            this.btnShowKeyPad.Name = "btnShowKeyPad";
            this.btnShowKeyPad.Size = new System.Drawing.Size(36, 41);
            this.btnShowKeyPad.TabIndex = 25;
            this.btnShowKeyPad.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnShowKeyPad.UseVisualStyleBackColor = false;
            // 
            // selectedScheduleVScrollBar
            // 
            this.selectedScheduleVScrollBar.AutoHide = false;
            this.selectedScheduleVScrollBar.DataGridView = this.dgvSelectedBookingSchedule;
            this.selectedScheduleVScrollBar.DownButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("selectedScheduleVScrollBar.DownButtonBackgroundImage")));
            this.selectedScheduleVScrollBar.DownButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("selectedScheduleVScrollBar.DownButtonDisabledBackgroundImage")));
            this.selectedScheduleVScrollBar.Location = new System.Drawing.Point(1230, 53);
            this.selectedScheduleVScrollBar.Margin = new System.Windows.Forms.Padding(0);
            this.selectedScheduleVScrollBar.Name = "selectedScheduleVScrollBar";
            this.selectedScheduleVScrollBar.ScrollableControl = null;
            this.selectedScheduleVScrollBar.ScrollViewer = null;
            this.selectedScheduleVScrollBar.Size = new System.Drawing.Size(40, 101);
            this.selectedScheduleVScrollBar.TabIndex = 117;
            this.selectedScheduleVScrollBar.UpButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("selectedScheduleVScrollBar.UpButtonBackgroundImage")));
            this.selectedScheduleVScrollBar.UpButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("selectedScheduleVScrollBar.UpButtonDisabledBackgroundImage")));
            this.selectedScheduleVScrollBar.UpButtonClick += new System.EventHandler(this.ScrollBar_Click);
            this.selectedScheduleVScrollBar.DownButtonClick += new System.EventHandler(this.ScrollBar_Click);
            // 
            // dgvSelectedBookingSchedule
            // 
            this.dgvSelectedBookingSchedule.AllowUserToAddRows = false;
            this.dgvSelectedBookingSchedule.AllowUserToDeleteRows = false;
            this.dgvSelectedBookingSchedule.AutoGenerateColumns = false;
            this.dgvSelectedBookingSchedule.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSelectedBookingSchedule.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cancelledDataGridViewCheckBoxColumn,
            this.isChangedDataGridViewCheckBoxColumn,
            this.ReservationDate,
            this.trxReservationScheduleIdDataGridViewTextBoxColumn,
            this.trxIdDataGridViewTextBoxColumn,
            this.lineIdDataGridViewTextBoxColumn,
            this.trxLineProductIdDataGridViewTextBoxColumn,
            this.scheduleNameDataGridViewTextBoxColumn1,
            this.scheduleFromDateDataGridViewTextBoxColumn,
            this.scheduleToDateDataGridViewTextBoxColumn,
            this.guestQuantityDataGridViewTextBoxColumn,
            this.facilityMapNameDataGridViewTextBoxColumn1,
            this.trxLineProductNameDataGridViewTextBoxColumn,
            this.schedulesIdDataGridViewTextBoxColumn,
            this.facilityMapIdDataGridViewTextBoxColumn1,
            this.cancelledByDataGridViewTextBoxColumn,
            this.cmbFromTime,
            this.cmbToTime,
            this.RemoveLine,
            this.BookingProductName,
            this.BookingProductId,
            this.BookingProductTrxLine,
            this.FacilityTrxLine,
            this.guestQty,
            this.Reschedule});
            this.dgvSelectedBookingSchedule.DataSource = this.transactionReservationScheduleDTOBindingSource;
            this.dgvSelectedBookingSchedule.Location = new System.Drawing.Point(12, 53);
            this.dgvSelectedBookingSchedule.MultiSelect = false;
            this.dgvSelectedBookingSchedule.Name = "dgvSelectedBookingSchedule";
            this.dgvSelectedBookingSchedule.RowHeadersVisible = false;
            this.dgvSelectedBookingSchedule.RowTemplate.Height = 30;
            this.dgvSelectedBookingSchedule.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvSelectedBookingSchedule.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSelectedBookingSchedule.Size = new System.Drawing.Size(1215, 101);
            this.dgvSelectedBookingSchedule.TabIndex = 116;
            this.dgvSelectedBookingSchedule.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSelectedBookingSchedule_CellClick);
            this.dgvSelectedBookingSchedule.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSelectedBookingSchedule_CellEnter);
            this.dgvSelectedBookingSchedule.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvSelectedBookingSchedule_CellFormatting);
            this.dgvSelectedBookingSchedule.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSelectedBookingSchedule_CellValueChanged);
            this.dgvSelectedBookingSchedule.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvSelectedBookingSchedule_DataError);
            // 
            // cancelledDataGridViewCheckBoxColumn
            // 
            this.cancelledDataGridViewCheckBoxColumn.DataPropertyName = "Cancelled";
            this.cancelledDataGridViewCheckBoxColumn.HeaderText = "Cancelled";
            this.cancelledDataGridViewCheckBoxColumn.Name = "cancelledDataGridViewCheckBoxColumn";
            this.cancelledDataGridViewCheckBoxColumn.Visible = false;
            // 
            // isChangedDataGridViewCheckBoxColumn
            // 
            this.isChangedDataGridViewCheckBoxColumn.DataPropertyName = "IsChanged";
            this.isChangedDataGridViewCheckBoxColumn.HeaderText = "IsChanged";
            this.isChangedDataGridViewCheckBoxColumn.Name = "isChangedDataGridViewCheckBoxColumn";
            this.isChangedDataGridViewCheckBoxColumn.Visible = false;
            // 
            // ReservationDate
            // 
            this.ReservationDate.DataPropertyName = "ScheduleFromDate";
            this.ReservationDate.HeaderText = "Reservation Date";
            this.ReservationDate.MinimumWidth = 130;
            this.ReservationDate.Name = "ReservationDate";
            this.ReservationDate.ReadOnly = true;
            this.ReservationDate.Width = 130;
            // 
            // trxReservationScheduleIdDataGridViewTextBoxColumn
            // 
            this.trxReservationScheduleIdDataGridViewTextBoxColumn.DataPropertyName = "TrxReservationScheduleId";
            this.trxReservationScheduleIdDataGridViewTextBoxColumn.HeaderText = "Trx Reservation Schedule Id";
            this.trxReservationScheduleIdDataGridViewTextBoxColumn.Name = "trxReservationScheduleIdDataGridViewTextBoxColumn";
            this.trxReservationScheduleIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // trxIdDataGridViewTextBoxColumn
            // 
            this.trxIdDataGridViewTextBoxColumn.DataPropertyName = "TrxId";
            this.trxIdDataGridViewTextBoxColumn.HeaderText = "Trx Id";
            this.trxIdDataGridViewTextBoxColumn.Name = "trxIdDataGridViewTextBoxColumn";
            this.trxIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // lineIdDataGridViewTextBoxColumn
            // 
            this.lineIdDataGridViewTextBoxColumn.DataPropertyName = "LineId";
            this.lineIdDataGridViewTextBoxColumn.HeaderText = "Line Id";
            this.lineIdDataGridViewTextBoxColumn.Name = "lineIdDataGridViewTextBoxColumn";
            this.lineIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // trxLineProductIdDataGridViewTextBoxColumn
            // 
            this.trxLineProductIdDataGridViewTextBoxColumn.DataPropertyName = "TrxLineProductId";
            this.trxLineProductIdDataGridViewTextBoxColumn.HeaderText = "Product Id";
            this.trxLineProductIdDataGridViewTextBoxColumn.Name = "trxLineProductIdDataGridViewTextBoxColumn";
            this.trxLineProductIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // scheduleNameDataGridViewTextBoxColumn1
            // 
            this.scheduleNameDataGridViewTextBoxColumn1.DataPropertyName = "ScheduleName";
            this.scheduleNameDataGridViewTextBoxColumn1.HeaderText = "Schedule Name";
            this.scheduleNameDataGridViewTextBoxColumn1.MinimumWidth = 150;
            this.scheduleNameDataGridViewTextBoxColumn1.Name = "scheduleNameDataGridViewTextBoxColumn1";
            this.scheduleNameDataGridViewTextBoxColumn1.Visible = false;
            this.scheduleNameDataGridViewTextBoxColumn1.Width = 150;
            // 
            // scheduleFromDateDataGridViewTextBoxColumn
            // 
            this.scheduleFromDateDataGridViewTextBoxColumn.DataPropertyName = "ScheduleFromDate";
            this.scheduleFromDateDataGridViewTextBoxColumn.HeaderText = "ScheduleFromDate";
            this.scheduleFromDateDataGridViewTextBoxColumn.MinimumWidth = 100;
            this.scheduleFromDateDataGridViewTextBoxColumn.Name = "scheduleFromDateDataGridViewTextBoxColumn";
            this.scheduleFromDateDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.scheduleFromDateDataGridViewTextBoxColumn.Visible = false;
            // 
            // scheduleToDateDataGridViewTextBoxColumn
            // 
            this.scheduleToDateDataGridViewTextBoxColumn.DataPropertyName = "ScheduleToDate";
            this.scheduleToDateDataGridViewTextBoxColumn.HeaderText = "ScheduleToDate";
            this.scheduleToDateDataGridViewTextBoxColumn.MinimumWidth = 100;
            this.scheduleToDateDataGridViewTextBoxColumn.Name = "scheduleToDateDataGridViewTextBoxColumn";
            this.scheduleToDateDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.scheduleToDateDataGridViewTextBoxColumn.Visible = false;
            // 
            // guestQuantityDataGridViewTextBoxColumn
            // 
            this.guestQuantityDataGridViewTextBoxColumn.DataPropertyName = "GuestQuantity";
            this.guestQuantityDataGridViewTextBoxColumn.HeaderText = "Guests";
            this.guestQuantityDataGridViewTextBoxColumn.MinimumWidth = 80;
            this.guestQuantityDataGridViewTextBoxColumn.Name = "guestQuantityDataGridViewTextBoxColumn";
            this.guestQuantityDataGridViewTextBoxColumn.Visible = false;
            // 
            // facilityMapNameDataGridViewTextBoxColumn1
            // 
            this.facilityMapNameDataGridViewTextBoxColumn1.DataPropertyName = "FacilityMapName";
            this.facilityMapNameDataGridViewTextBoxColumn1.HeaderText = "Facility";
            this.facilityMapNameDataGridViewTextBoxColumn1.MinimumWidth = 150;
            this.facilityMapNameDataGridViewTextBoxColumn1.Name = "facilityMapNameDataGridViewTextBoxColumn1";
            this.facilityMapNameDataGridViewTextBoxColumn1.ReadOnly = true;
            this.facilityMapNameDataGridViewTextBoxColumn1.Width = 150;
            // 
            // trxLineProductNameDataGridViewTextBoxColumn
            // 
            this.trxLineProductNameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.trxLineProductNameDataGridViewTextBoxColumn.DataPropertyName = "TrxLineProductName";
            this.trxLineProductNameDataGridViewTextBoxColumn.HeaderText = "Facility Rental";
            this.trxLineProductNameDataGridViewTextBoxColumn.MinimumWidth = 150;
            this.trxLineProductNameDataGridViewTextBoxColumn.Name = "trxLineProductNameDataGridViewTextBoxColumn";
            this.trxLineProductNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // schedulesIdDataGridViewTextBoxColumn
            // 
            this.schedulesIdDataGridViewTextBoxColumn.DataPropertyName = "SchedulesId";
            this.schedulesIdDataGridViewTextBoxColumn.HeaderText = "Schedules Id";
            this.schedulesIdDataGridViewTextBoxColumn.Name = "schedulesIdDataGridViewTextBoxColumn";
            this.schedulesIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // facilityMapIdDataGridViewTextBoxColumn1
            // 
            this.facilityMapIdDataGridViewTextBoxColumn1.DataPropertyName = "FacilityMapId";
            this.facilityMapIdDataGridViewTextBoxColumn1.HeaderText = "Facility Map Id";
            this.facilityMapIdDataGridViewTextBoxColumn1.Name = "facilityMapIdDataGridViewTextBoxColumn1";
            this.facilityMapIdDataGridViewTextBoxColumn1.Visible = false;
            // 
            // cancelledByDataGridViewTextBoxColumn
            // 
            this.cancelledByDataGridViewTextBoxColumn.DataPropertyName = "CancelledBy";
            this.cancelledByDataGridViewTextBoxColumn.HeaderText = "cancelled By";
            this.cancelledByDataGridViewTextBoxColumn.Name = "cancelledByDataGridViewTextBoxColumn";
            this.cancelledByDataGridViewTextBoxColumn.Visible = false;
            // 
            // cmbFromTime
            // 
            this.cmbFromTime.HeaderText = "From Time";
            this.cmbFromTime.MinimumWidth = 100;
            this.cmbFromTime.Name = "cmbFromTime";
            // 
            // cmbToTime
            // 
            this.cmbToTime.HeaderText = "To Time";
            this.cmbToTime.MinimumWidth = 100;
            this.cmbToTime.Name = "cmbToTime";
            // 
            // RemoveLine
            // 
            this.RemoveLine.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RemoveLine.HeaderText = "";
            this.RemoveLine.MinimumWidth = 40;
            this.RemoveLine.Name = "RemoveLine";
            this.RemoveLine.ReadOnly = true;
            this.RemoveLine.Text = "X";
            this.RemoveLine.Width = 40;
            // 
            // BookingProductName
            // 
            this.BookingProductName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.BookingProductName.HeaderText = "Booking Product";
            this.BookingProductName.MinimumWidth = 190;
            this.BookingProductName.Name = "BookingProductName";
            this.BookingProductName.ReadOnly = true;
            // 
            // BookingProductId
            // 
            this.BookingProductId.HeaderText = "BookingProductId";
            this.BookingProductId.Name = "BookingProductId";
            this.BookingProductId.Visible = false;
            // 
            // BookingProductTrxLine
            // 
            this.BookingProductTrxLine.HeaderText = "BookingProductTrxLine";
            this.BookingProductTrxLine.Name = "BookingProductTrxLine";
            this.BookingProductTrxLine.Visible = false;
            // 
            // FacilityTrxLine
            // 
            this.FacilityTrxLine.HeaderText = "FacilityTrxLine";
            this.FacilityTrxLine.Name = "FacilityTrxLine";
            this.FacilityTrxLine.Visible = false;
            // 
            // guestQty
            // 
            this.guestQty.HeaderText = "Guests";
            this.guestQty.Name = "guestQty";
            // 
            // Reschedule
            // 
            this.Reschedule.HeaderText = "";
            this.Reschedule.Image = global::Parafait_POS.Properties.Resources.RescheduleReservation;
            this.Reschedule.MinimumWidth = 40;
            this.Reschedule.Name = "Reschedule";
            this.Reschedule.ReadOnly = true;
            this.Reschedule.ToolTipText = "Reschedule";
            this.Reschedule.Width = 40;
            // 
            // transactionReservationScheduleDTOBindingSource
            // 
            this.transactionReservationScheduleDTOBindingSource.DataSource = typeof(Semnox.Parafait.Transaction.TransactionReservationScheduleDTO);
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
            this.btnCancel.Location = new System.Drawing.Point(452, 163);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(116, 34);
            this.btnCancel.TabIndex = 120;
            this.btnCancel.Text = "Close";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnClose_Click);
            this.btnCancel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BlueBtn_MouseDown);
            this.btnCancel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BlueBtn_MouseUp);
            // 
            // btnNextDateTime
            // 
            this.btnNextDateTime.BackColor = System.Drawing.Color.Transparent;
            this.btnNextDateTime.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnNextDateTime.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnNextDateTime.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnNextDateTime.FlatAppearance.BorderSize = 0;
            this.btnNextDateTime.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnNextDateTime.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnNextDateTime.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnNextDateTime.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNextDateTime.ForeColor = System.Drawing.Color.White;
            this.btnNextDateTime.Location = new System.Drawing.Point(872, 163);
            this.btnNextDateTime.Name = "btnNextDateTime";
            this.btnNextDateTime.Size = new System.Drawing.Size(116, 34);
            this.btnNextDateTime.TabIndex = 121;
            this.btnNextDateTime.Text = "Next";
            this.btnNextDateTime.UseVisualStyleBackColor = false;
            this.btnNextDateTime.Click += new System.EventHandler(this.btnNextDateTime_Click);
            this.btnNextDateTime.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BlueBtn_MouseDown);
            this.btnNextDateTime.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BlueBtn_MouseUp);
            // 
            // btnClearBooking
            // 
            this.btnClearBooking.BackColor = System.Drawing.Color.Transparent;
            this.btnClearBooking.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnClearBooking.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnClearBooking.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnClearBooking.FlatAppearance.BorderSize = 0;
            this.btnClearBooking.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnClearBooking.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnClearBooking.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnClearBooking.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearBooking.ForeColor = System.Drawing.Color.White;
            this.btnClearBooking.Location = new System.Drawing.Point(248, 162);
            this.btnClearBooking.Name = "btnClearBooking";
            this.btnClearBooking.Size = new System.Drawing.Size(104, 36);
            this.btnClearBooking.TabIndex = 115;
            this.btnClearBooking.Text = "Clear";
            this.btnClearBooking.UseVisualStyleBackColor = false;
            this.btnClearBooking.Click += new System.EventHandler(this.btnClearBooking_Click);
            this.btnClearBooking.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BlueBtn_MouseDown);
            this.btnClearBooking.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BlueBtn_MouseUp);
            // 
            // btnBlockSchedule
            // 
            this.btnBlockSchedule.BackColor = System.Drawing.Color.Transparent;
            this.btnBlockSchedule.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnBlockSchedule.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnBlockSchedule.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnBlockSchedule.FlatAppearance.BorderSize = 0;
            this.btnBlockSchedule.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnBlockSchedule.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnBlockSchedule.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnBlockSchedule.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBlockSchedule.ForeColor = System.Drawing.Color.White;
            this.btnBlockSchedule.Location = new System.Drawing.Point(668, 162);
            this.btnBlockSchedule.Name = "btnBlockSchedule";
            this.btnBlockSchedule.Size = new System.Drawing.Size(104, 36);
            this.btnBlockSchedule.TabIndex = 115;
            this.btnBlockSchedule.Text = "Block Schedule";
            this.btnBlockSchedule.UseVisualStyleBackColor = false;
            this.btnBlockSchedule.Click += new System.EventHandler(this.btnBlockSchedule_Click);
            this.btnBlockSchedule.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BlueBtn_MouseDown);
            this.btnBlockSchedule.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BlueBtn_MouseUp);
            // 
            // cmbChannel
            // 
            this.cmbChannel.AutoCompleteCustomSource.AddRange(new string[] {
            "InPerson",
            "Phone",
            "Email",
            "Web",
            "Gateway"});
            this.cmbChannel.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbChannel.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbChannel.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbChannel.FormattingEnabled = true;
            this.cmbChannel.Items.AddRange(new object[] {
            "InPerson",
            "Phone",
            "Email",
            "Web",
            "Gateway"});
            this.cmbChannel.Location = new System.Drawing.Point(418, 16);
            this.cmbChannel.Name = "cmbChannel";
            this.cmbChannel.Size = new System.Drawing.Size(116, 31);
            this.cmbChannel.TabIndex = 69;
            this.cmbChannel.SelectedValueChanged += new System.EventHandler(this.cmbChannel_SelectedValueChanged);
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(340, 16);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(80, 30);
            this.label11.TabIndex = 71;
            this.label11.Text = "Channel:*";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBookingName
            // 
            this.txtBookingName.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBookingName.Location = new System.Drawing.Point(105, 16);
            this.txtBookingName.MaxLength = 50;
            this.txtBookingName.Name = "txtBookingName";
            this.txtBookingName.Size = new System.Drawing.Size(226, 30);
            this.txtBookingName.TabIndex = 68;
            this.txtBookingName.Validating += new System.ComponentModel.CancelEventHandler(this.txtBookingName_Validating);
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(4, 16);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(102, 30);
            this.label10.TabIndex = 70;
            this.label10.Text = "Booking Name:*";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gpbSearchOptions
            // 
            this.gpbSearchOptions.Controls.Add(this.cbxHideBookedSlots);
            this.gpbSearchOptions.Controls.Add(this.gpbFacilityProducts);
            this.gpbSearchOptions.Controls.Add(this.cmbSearchFacility);
            this.gpbSearchOptions.Controls.Add(this.lblFacility);
            this.gpbSearchOptions.Controls.Add(this.cmbSearchBookingProduct);
            this.gpbSearchOptions.Controls.Add(this.label3);
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
            this.gpbSearchOptions.Location = new System.Drawing.Point(7, -2);
            this.gpbSearchOptions.Name = "gpbSearchOptions";
            this.gpbSearchOptions.Size = new System.Drawing.Size(1282, 430);
            this.gpbSearchOptions.TabIndex = 0;
            this.gpbSearchOptions.TabStop = false;
            this.gpbSearchOptions.Text = "Search";
            // 
            // cbxHideBookedSlots
            // 
            this.cbxHideBookedSlots.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbxHideBookedSlots.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.cbxHideBookedSlots.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbxHideBookedSlots.Checked = true;
            this.cbxHideBookedSlots.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbxHideBookedSlots.FlatAppearance.BorderSize = 0;
            this.cbxHideBookedSlots.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbxHideBookedSlots.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbxHideBookedSlots.ImageIndex = 0;
            this.cbxHideBookedSlots.Location = new System.Drawing.Point(1046, 53);
            this.cbxHideBookedSlots.Name = "cbxHideBookedSlots";
            this.cbxHideBookedSlots.Size = new System.Drawing.Size(180, 36);
            this.cbxHideBookedSlots.TabIndex = 117;
            this.cbxHideBookedSlots.Text = "Hide Booked Slots:";
            this.cbxHideBookedSlots.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbxHideBookedSlots.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.cbxHideBookedSlots.UseVisualStyleBackColor = true;
            this.cbxHideBookedSlots.Visible = false;
            this.cbxHideBookedSlots.CheckedChanged += new System.EventHandler(this.cbxHideBookedSlots_CheckedChanged);
            // 
            // gpbFacilityProducts
            // 
            this.gpbFacilityProducts.Controls.Add(this.dgvSearchSchedules);
            this.gpbFacilityProducts.Controls.Add(this.searchedScheduleHScrollBar);
            this.gpbFacilityProducts.Controls.Add(this.searchedScheduleVScrollBar);
            this.gpbFacilityProducts.Controls.Add(this.facProdHScrollBar);
            this.gpbFacilityProducts.Controls.Add(this.btnAddToBooking);
            this.gpbFacilityProducts.Controls.Add(this.facProdVScrollBar);
            this.gpbFacilityProducts.Controls.Add(this.dgvSearchFacilityProducts);
            this.gpbFacilityProducts.Location = new System.Drawing.Point(0, 86);
            this.gpbFacilityProducts.Name = "gpbFacilityProducts";
            this.gpbFacilityProducts.Size = new System.Drawing.Size(1280, 354);
            this.gpbFacilityProducts.TabIndex = 116;
            this.gpbFacilityProducts.TabStop = false;
            this.gpbFacilityProducts.Text = "Pick Schedule";
            // 
            // dgvSearchSchedules
            // 
            this.dgvSearchSchedules.AllowUserToAddRows = false;
            this.dgvSearchSchedules.AllowUserToDeleteRows = false;
            this.dgvSearchSchedules.AutoGenerateColumns = false;
            this.dgvSearchSchedules.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSearchSchedules.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.facilityMapIdDataGridViewTextBoxColumn,
            this.ScheduleFromDate,
            this.ScheduleToDate,
            this.facilityMapNameDataGridViewTextBoxColumn,
            this.masterScheduleIdDataGridViewTextBoxColumn,
            this.scheduleIdDataGridViewTextBoxColumn,
            this.scheduleFromTimeDataGridViewTextBoxColumn,
            this.scheduleToTimeDataGridViewTextBoxColumn,
            this.fixedScheduleDataGridViewCheckBoxColumn,
            this.availableUnitsDataGridViewTextBoxColumn,
            this.scheduleNameDataGridViewTextBoxColumn,
            this.masterScheduleNameDataGridViewTextBoxColumn,
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
            this.desiredUnitsDataGridViewTextBoxColumn,
            this.expiryDateDataGridViewTextBoxColumn,
            this.categoryIdDataGridViewTextBoxColumn,
            this.promotionIdDataGridViewTextBoxColumn,
            this.seatsDataGridViewTextBoxColumn,
            this.facilityMapDTODataGridViewTextBoxColumn});
            this.dgvSearchSchedules.DataSource = this.scheduleDetailsDTOBindingSource;
            this.dgvSearchSchedules.Location = new System.Drawing.Point(13, 15);
            this.dgvSearchSchedules.MultiSelect = false;
            this.dgvSearchSchedules.Name = "dgvSearchSchedules";
            this.dgvSearchSchedules.RowHeadersVisible = false;
            this.dgvSearchSchedules.RowTemplate.Height = 30;
            this.dgvSearchSchedules.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvSearchSchedules.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSearchSchedules.Size = new System.Drawing.Size(766, 280);
            this.dgvSearchSchedules.TabIndex = 113;
            this.dgvSearchSchedules.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSearchSchedules_CellEnter);
            this.dgvSearchSchedules.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvSearchSchedules_CellFormatting);
            this.dgvSearchSchedules.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSearchSchedules_RowEnter);
            // 
            // facilityMapIdDataGridViewTextBoxColumn
            // 
            this.facilityMapIdDataGridViewTextBoxColumn.DataPropertyName = "FacilityMapId";
            this.facilityMapIdDataGridViewTextBoxColumn.HeaderText = "Facility Map Id";
            this.facilityMapIdDataGridViewTextBoxColumn.Name = "facilityMapIdDataGridViewTextBoxColumn";
            this.facilityMapIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // ScheduleFromDate
            // 
            this.ScheduleFromDate.DataPropertyName = "ScheduleFromDate";
            this.ScheduleFromDate.HeaderText = "ScheduleFromDate";
            this.ScheduleFromDate.Name = "ScheduleFromDate";
            this.ScheduleFromDate.ReadOnly = true;
            this.ScheduleFromDate.Visible = false;
            // 
            // ScheduleToDate
            // 
            this.ScheduleToDate.DataPropertyName = "ScheduleToDate";
            this.ScheduleToDate.HeaderText = "ScheduleToDate";
            this.ScheduleToDate.Name = "ScheduleToDate";
            this.ScheduleToDate.ReadOnly = true;
            this.ScheduleToDate.Visible = false;
            // 
            // facilityMapNameDataGridViewTextBoxColumn
            // 
            this.facilityMapNameDataGridViewTextBoxColumn.DataPropertyName = "FacilityMapName";
            this.facilityMapNameDataGridViewTextBoxColumn.HeaderText = "Facility Map";
            this.facilityMapNameDataGridViewTextBoxColumn.MinimumWidth = 150;
            this.facilityMapNameDataGridViewTextBoxColumn.Name = "facilityMapNameDataGridViewTextBoxColumn";
            this.facilityMapNameDataGridViewTextBoxColumn.Width = 150;
            // 
            // masterScheduleIdDataGridViewTextBoxColumn
            // 
            this.masterScheduleIdDataGridViewTextBoxColumn.DataPropertyName = "MasterScheduleId";
            this.masterScheduleIdDataGridViewTextBoxColumn.HeaderText = "Master Schedule Id";
            this.masterScheduleIdDataGridViewTextBoxColumn.Name = "masterScheduleIdDataGridViewTextBoxColumn";
            this.masterScheduleIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // scheduleIdDataGridViewTextBoxColumn
            // 
            this.scheduleIdDataGridViewTextBoxColumn.DataPropertyName = "ScheduleId";
            this.scheduleIdDataGridViewTextBoxColumn.HeaderText = "Schedule Id";
            this.scheduleIdDataGridViewTextBoxColumn.Name = "scheduleIdDataGridViewTextBoxColumn";
            this.scheduleIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // scheduleFromTimeDataGridViewTextBoxColumn
            // 
            this.scheduleFromTimeDataGridViewTextBoxColumn.DataPropertyName = "ScheduleFromTime";
            this.scheduleFromTimeDataGridViewTextBoxColumn.HeaderText = "From Time";
            this.scheduleFromTimeDataGridViewTextBoxColumn.MinimumWidth = 100;
            this.scheduleFromTimeDataGridViewTextBoxColumn.Name = "scheduleFromTimeDataGridViewTextBoxColumn";
            // 
            // scheduleToTimeDataGridViewTextBoxColumn
            // 
            this.scheduleToTimeDataGridViewTextBoxColumn.DataPropertyName = "ScheduleToTime";
            this.scheduleToTimeDataGridViewTextBoxColumn.HeaderText = "To Time";
            this.scheduleToTimeDataGridViewTextBoxColumn.MinimumWidth = 100;
            this.scheduleToTimeDataGridViewTextBoxColumn.Name = "scheduleToTimeDataGridViewTextBoxColumn";
            // 
            // fixedScheduleDataGridViewCheckBoxColumn
            // 
            this.fixedScheduleDataGridViewCheckBoxColumn.DataPropertyName = "FixedSchedule";
            this.fixedScheduleDataGridViewCheckBoxColumn.HeaderText = "Fixed";
            this.fixedScheduleDataGridViewCheckBoxColumn.MinimumWidth = 50;
            this.fixedScheduleDataGridViewCheckBoxColumn.Name = "fixedScheduleDataGridViewCheckBoxColumn";
            this.fixedScheduleDataGridViewCheckBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.fixedScheduleDataGridViewCheckBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.fixedScheduleDataGridViewCheckBoxColumn.Width = 50;
            // 
            // availableUnitsDataGridViewTextBoxColumn
            // 
            this.availableUnitsDataGridViewTextBoxColumn.DataPropertyName = "AvailableUnits";
            this.availableUnitsDataGridViewTextBoxColumn.HeaderText = "Available Units";
            this.availableUnitsDataGridViewTextBoxColumn.Name = "availableUnitsDataGridViewTextBoxColumn";
            // 
            // scheduleNameDataGridViewTextBoxColumn
            // 
            this.scheduleNameDataGridViewTextBoxColumn.DataPropertyName = "ScheduleName";
            this.scheduleNameDataGridViewTextBoxColumn.HeaderText = "Schedule Name";
            this.scheduleNameDataGridViewTextBoxColumn.MinimumWidth = 150;
            this.scheduleNameDataGridViewTextBoxColumn.Name = "scheduleNameDataGridViewTextBoxColumn";
            this.scheduleNameDataGridViewTextBoxColumn.Width = 150;
            // 
            // masterScheduleNameDataGridViewTextBoxColumn
            // 
            this.masterScheduleNameDataGridViewTextBoxColumn.DataPropertyName = "MasterScheduleName";
            this.masterScheduleNameDataGridViewTextBoxColumn.HeaderText = "Schedule Group";
            this.masterScheduleNameDataGridViewTextBoxColumn.MinimumWidth = 150;
            this.masterScheduleNameDataGridViewTextBoxColumn.Name = "masterScheduleNameDataGridViewTextBoxColumn";
            this.masterScheduleNameDataGridViewTextBoxColumn.Visible = false;
            this.masterScheduleNameDataGridViewTextBoxColumn.Width = 150;
            // 
            // attractionPlayIdDataGridViewTextBoxColumn
            // 
            this.attractionPlayIdDataGridViewTextBoxColumn.DataPropertyName = "AttractionPlayId";
            this.attractionPlayIdDataGridViewTextBoxColumn.HeaderText = "Attraction Play Id";
            this.attractionPlayIdDataGridViewTextBoxColumn.Name = "attractionPlayIdDataGridViewTextBoxColumn";
            this.attractionPlayIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // attractionPlayNameDataGridViewTextBoxColumn
            // 
            this.attractionPlayNameDataGridViewTextBoxColumn.DataPropertyName = "AttractionPlayName";
            this.attractionPlayNameDataGridViewTextBoxColumn.HeaderText = "Play Name";
            this.attractionPlayNameDataGridViewTextBoxColumn.Name = "attractionPlayNameDataGridViewTextBoxColumn";
            this.attractionPlayNameDataGridViewTextBoxColumn.Visible = false;
            // 
            // attractionPlayPriceDataGridViewTextBoxColumn
            // 
            this.attractionPlayPriceDataGridViewTextBoxColumn.DataPropertyName = "AttractionPlayPrice";
            this.attractionPlayPriceDataGridViewTextBoxColumn.HeaderText = "AttractionPlayPrice";
            this.attractionPlayPriceDataGridViewTextBoxColumn.Name = "attractionPlayPriceDataGridViewTextBoxColumn";
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
            this.productNameDataGridViewTextBoxColumn.Visible = false;
            // 
            // priceDataGridViewTextBoxColumn
            // 
            this.priceDataGridViewTextBoxColumn.DataPropertyName = "Price";
            this.priceDataGridViewTextBoxColumn.HeaderText = "Price";
            this.priceDataGridViewTextBoxColumn.Name = "priceDataGridViewTextBoxColumn";
            this.priceDataGridViewTextBoxColumn.Visible = false;
            // 
            // facilityCapacityDataGridViewTextBoxColumn
            // 
            this.facilityCapacityDataGridViewTextBoxColumn.DataPropertyName = "FacilityCapacity";
            this.facilityCapacityDataGridViewTextBoxColumn.HeaderText = "Facility Capacity";
            this.facilityCapacityDataGridViewTextBoxColumn.Name = "facilityCapacityDataGridViewTextBoxColumn";
            this.facilityCapacityDataGridViewTextBoxColumn.Visible = false;
            // 
            // ruleUnitsDataGridViewTextBoxColumn
            // 
            this.ruleUnitsDataGridViewTextBoxColumn.DataPropertyName = "RuleUnits";
            this.ruleUnitsDataGridViewTextBoxColumn.HeaderText = "Rule Units";
            this.ruleUnitsDataGridViewTextBoxColumn.Name = "ruleUnitsDataGridViewTextBoxColumn";
            this.ruleUnitsDataGridViewTextBoxColumn.Visible = false;
            // 
            // totalUnitsDataGridViewTextBoxColumn
            // 
            this.totalUnitsDataGridViewTextBoxColumn.DataPropertyName = "TotalUnits";
            this.totalUnitsDataGridViewTextBoxColumn.HeaderText = "Total Units";
            this.totalUnitsDataGridViewTextBoxColumn.Name = "totalUnitsDataGridViewTextBoxColumn";
            this.totalUnitsDataGridViewTextBoxColumn.Visible = false;
            // 
            // bookedUnitsDataGridViewTextBoxColumn
            // 
            this.bookedUnitsDataGridViewTextBoxColumn.DataPropertyName = "BookedUnits";
            this.bookedUnitsDataGridViewTextBoxColumn.HeaderText = "Booked Units";
            this.bookedUnitsDataGridViewTextBoxColumn.Name = "bookedUnitsDataGridViewTextBoxColumn";
            this.bookedUnitsDataGridViewTextBoxColumn.Visible = false;
            // 
            // desiredUnitsDataGridViewTextBoxColumn
            // 
            this.desiredUnitsDataGridViewTextBoxColumn.DataPropertyName = "DesiredUnits";
            this.desiredUnitsDataGridViewTextBoxColumn.HeaderText = "Desired Units";
            this.desiredUnitsDataGridViewTextBoxColumn.Name = "desiredUnitsDataGridViewTextBoxColumn";
            this.desiredUnitsDataGridViewTextBoxColumn.Visible = false;
            // 
            // expiryDateDataGridViewTextBoxColumn
            // 
            this.expiryDateDataGridViewTextBoxColumn.DataPropertyName = "ExpiryDate";
            this.expiryDateDataGridViewTextBoxColumn.HeaderText = "ExpiryDate";
            this.expiryDateDataGridViewTextBoxColumn.Name = "expiryDateDataGridViewTextBoxColumn";
            this.expiryDateDataGridViewTextBoxColumn.Visible = false;
            // 
            // categoryIdDataGridViewTextBoxColumn
            // 
            this.categoryIdDataGridViewTextBoxColumn.DataPropertyName = "CategoryId";
            this.categoryIdDataGridViewTextBoxColumn.HeaderText = "Category Id";
            this.categoryIdDataGridViewTextBoxColumn.Name = "categoryIdDataGridViewTextBoxColumn";
            this.categoryIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // promotionIdDataGridViewTextBoxColumn
            // 
            this.promotionIdDataGridViewTextBoxColumn.DataPropertyName = "PromotionId";
            this.promotionIdDataGridViewTextBoxColumn.HeaderText = "Promotion Id";
            this.promotionIdDataGridViewTextBoxColumn.Name = "promotionIdDataGridViewTextBoxColumn";
            this.promotionIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // seatsDataGridViewTextBoxColumn
            // 
            this.seatsDataGridViewTextBoxColumn.DataPropertyName = "Seats";
            this.seatsDataGridViewTextBoxColumn.HeaderText = "Seats";
            this.seatsDataGridViewTextBoxColumn.Name = "seatsDataGridViewTextBoxColumn";
            this.seatsDataGridViewTextBoxColumn.Visible = false;
            // 
            // facilityMapDTODataGridViewTextBoxColumn
            // 
            this.facilityMapDTODataGridViewTextBoxColumn.DataPropertyName = "FacilityMapDTO";
            this.facilityMapDTODataGridViewTextBoxColumn.HeaderText = "FacilityMapDTO";
            this.facilityMapDTODataGridViewTextBoxColumn.Name = "facilityMapDTODataGridViewTextBoxColumn";
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
            this.searchedScheduleHScrollBar.Location = new System.Drawing.Point(14, 297);
            this.searchedScheduleHScrollBar.Margin = new System.Windows.Forms.Padding(0);
            this.searchedScheduleHScrollBar.Name = "searchedScheduleHScrollBar";
            this.searchedScheduleHScrollBar.RightButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("searchedScheduleHScrollBar.RightButtonBackgroundImage")));
            this.searchedScheduleHScrollBar.RightButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("searchedScheduleHScrollBar.RightButtonDisabledBackgroundImage")));
            this.searchedScheduleHScrollBar.ScrollableControl = null;
            this.searchedScheduleHScrollBar.ScrollViewer = null;
            this.searchedScheduleHScrollBar.Size = new System.Drawing.Size(764, 40);
            this.searchedScheduleHScrollBar.TabIndex = 115;
            this.searchedScheduleHScrollBar.LeftButtonClick += new System.EventHandler(this.ScrollBar_Click);
            this.searchedScheduleHScrollBar.RightButtonClick += new System.EventHandler(this.ScrollBar_Click);
            // 
            // searchedScheduleVScrollBar
            // 
            this.searchedScheduleVScrollBar.AutoHide = false;
            this.searchedScheduleVScrollBar.DataGridView = this.dgvSearchSchedules;
            this.searchedScheduleVScrollBar.DownButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("searchedScheduleVScrollBar.DownButtonBackgroundImage")));
            this.searchedScheduleVScrollBar.DownButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("searchedScheduleVScrollBar.DownButtonDisabledBackgroundImage")));
            this.searchedScheduleVScrollBar.Location = new System.Drawing.Point(782, 12);
            this.searchedScheduleVScrollBar.Margin = new System.Windows.Forms.Padding(0);
            this.searchedScheduleVScrollBar.Name = "searchedScheduleVScrollBar";
            this.searchedScheduleVScrollBar.ScrollableControl = null;
            this.searchedScheduleVScrollBar.ScrollViewer = null;
            this.searchedScheduleVScrollBar.Size = new System.Drawing.Size(40, 282);
            this.searchedScheduleVScrollBar.TabIndex = 114;
            this.searchedScheduleVScrollBar.UpButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("searchedScheduleVScrollBar.UpButtonBackgroundImage")));
            this.searchedScheduleVScrollBar.UpButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("searchedScheduleVScrollBar.UpButtonDisabledBackgroundImage")));
            this.searchedScheduleVScrollBar.UpButtonClick += new System.EventHandler(this.ScrollBar_Click);
            this.searchedScheduleVScrollBar.DownButtonClick += new System.EventHandler(this.ScrollBar_Click);
            // 
            // facProdHScrollBar
            // 
            this.facProdHScrollBar.AutoHide = false;
            this.facProdHScrollBar.DataGridView = this.dgvSearchFacilityProducts;
            this.facProdHScrollBar.LeftButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("facProdHScrollBar.LeftButtonBackgroundImage")));
            this.facProdHScrollBar.LeftButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("facProdHScrollBar.LeftButtonDisabledBackgroundImage")));
            this.facProdHScrollBar.Location = new System.Drawing.Point(826, 254);
            this.facProdHScrollBar.Margin = new System.Windows.Forms.Padding(0);
            this.facProdHScrollBar.Name = "facProdHScrollBar";
            this.facProdHScrollBar.RightButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("facProdHScrollBar.RightButtonBackgroundImage")));
            this.facProdHScrollBar.RightButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("facProdHScrollBar.RightButtonDisabledBackgroundImage")));
            this.facProdHScrollBar.ScrollableControl = null;
            this.facProdHScrollBar.ScrollViewer = null;
            this.facProdHScrollBar.Size = new System.Drawing.Size(400, 40);
            this.facProdHScrollBar.TabIndex = 119;
            this.facProdHScrollBar.LeftButtonClick += new System.EventHandler(this.ScrollBar_Click);
            this.facProdHScrollBar.RightButtonClick += new System.EventHandler(this.ScrollBar_Click);
            // 
            // dgvSearchFacilityProducts
            // 
            this.dgvSearchFacilityProducts.AllowUserToAddRows = false;
            this.dgvSearchFacilityProducts.AllowUserToDeleteRows = false;
            this.dgvSearchFacilityProducts.AutoGenerateColumns = false;
            this.dgvSearchFacilityProducts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSearchFacilityProducts.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.productNameDataGridViewTextBoxColumn1,
            this.ProductType,
            this.productIdDataGridViewTextBoxColumn1,
            this.descriptionDataGridViewTextBoxColumn,
            this.activeFlagDataGridViewTextBoxColumn,
            this.productTypeIdDataGridViewTextBoxColumn,
            this.SelectedRecord,
            this.productInformation});
            this.dgvSearchFacilityProducts.DataSource = this.productsDTOBindingSource;
            this.dgvSearchFacilityProducts.Location = new System.Drawing.Point(826, 15);
            this.dgvSearchFacilityProducts.MultiSelect = false;
            this.dgvSearchFacilityProducts.Name = "dgvSearchFacilityProducts";
            this.dgvSearchFacilityProducts.RowHeadersVisible = false;
            this.dgvSearchFacilityProducts.RowTemplate.Height = 30;
            this.dgvSearchFacilityProducts.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvSearchFacilityProducts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSearchFacilityProducts.Size = new System.Drawing.Size(400, 238);
            this.dgvSearchFacilityProducts.TabIndex = 0;
            this.dgvSearchFacilityProducts.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSearchFacilityProducts_CellClick);
            this.dgvSearchFacilityProducts.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSearchFacilityProducts_RowEnter);
            // 
            // productNameDataGridViewTextBoxColumn1
            // 
            this.productNameDataGridViewTextBoxColumn1.DataPropertyName = "ProductName";
            this.productNameDataGridViewTextBoxColumn1.HeaderText = "Product Name";
            this.productNameDataGridViewTextBoxColumn1.MinimumWidth = 200;
            this.productNameDataGridViewTextBoxColumn1.Name = "productNameDataGridViewTextBoxColumn1";
            this.productNameDataGridViewTextBoxColumn1.ReadOnly = true;
            this.productNameDataGridViewTextBoxColumn1.Width = 200;
            // 
            // ProductType
            // 
            this.ProductType.DataPropertyName = "ProductType";
            this.ProductType.HeaderText = "Product Type";
            this.ProductType.MinimumWidth = 100;
            this.ProductType.Name = "ProductType";
            this.ProductType.ReadOnly = true;
            this.ProductType.Width = 120;
            // 
            // productIdDataGridViewTextBoxColumn1
            // 
            this.productIdDataGridViewTextBoxColumn1.DataPropertyName = "ProductId";
            this.productIdDataGridViewTextBoxColumn1.HeaderText = "ProductId";
            this.productIdDataGridViewTextBoxColumn1.Name = "productIdDataGridViewTextBoxColumn1";
            this.productIdDataGridViewTextBoxColumn1.Visible = false;
            // 
            // descriptionDataGridViewTextBoxColumn
            // 
            this.descriptionDataGridViewTextBoxColumn.DataPropertyName = "Description";
            this.descriptionDataGridViewTextBoxColumn.HeaderText = "Description";
            this.descriptionDataGridViewTextBoxColumn.Name = "descriptionDataGridViewTextBoxColumn";
            this.descriptionDataGridViewTextBoxColumn.Visible = false;
            // 
            // activeFlagDataGridViewTextBoxColumn
            // 
            this.activeFlagDataGridViewTextBoxColumn.DataPropertyName = "ActiveFlag";
            this.activeFlagDataGridViewTextBoxColumn.HeaderText = "ActiveFlag";
            this.activeFlagDataGridViewTextBoxColumn.Name = "activeFlagDataGridViewTextBoxColumn";
            this.activeFlagDataGridViewTextBoxColumn.Visible = false;
            // 
            // productTypeIdDataGridViewTextBoxColumn
            // 
            this.productTypeIdDataGridViewTextBoxColumn.DataPropertyName = "ProductTypeId";
            this.productTypeIdDataGridViewTextBoxColumn.HeaderText = "ProductTypeId";
            this.productTypeIdDataGridViewTextBoxColumn.Name = "productTypeIdDataGridViewTextBoxColumn";
            this.productTypeIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // SelectedRecord
            // 
            this.SelectedRecord.FalseValue = "False";
            this.SelectedRecord.HeaderText = "Add";
            this.SelectedRecord.MinimumWidth = 50;
            this.SelectedRecord.Name = "SelectedRecord";
            this.SelectedRecord.ReadOnly = true;
            this.SelectedRecord.TrueValue = "True";
            this.SelectedRecord.Width = 50;
            // 
            // productInformation
            // 
            this.productInformation.HeaderText = "\r\n...";
            this.productInformation.MinimumWidth = 30;
            this.productInformation.Name = "productInformation";
            this.productInformation.ReadOnly = true;
            this.productInformation.Text = "...";
            this.productInformation.UseColumnTextForButtonValue = true;
            this.productInformation.Width = 30;
            // 
            // productsDTOBindingSource
            // 
            this.productsDTOBindingSource.DataSource = typeof(Semnox.Parafait.Product.ProductsDTO);
            // 
            // btnAddToBooking
            // 
            this.btnAddToBooking.BackColor = System.Drawing.Color.Transparent;
            this.btnAddToBooking.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnAddToBooking.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnAddToBooking.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnAddToBooking.FlatAppearance.BorderSize = 0;
            this.btnAddToBooking.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnAddToBooking.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnAddToBooking.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnAddToBooking.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddToBooking.ForeColor = System.Drawing.Color.White;
            this.btnAddToBooking.Location = new System.Drawing.Point(933, 302);
            this.btnAddToBooking.Name = "btnAddToBooking";
            this.btnAddToBooking.Size = new System.Drawing.Size(200, 36);
            this.btnAddToBooking.TabIndex = 116;
            this.btnAddToBooking.Text = "Add To Booking";
            this.btnAddToBooking.UseVisualStyleBackColor = false;
            this.btnAddToBooking.Click += new System.EventHandler(this.btnAddToBooking_Click);
            this.btnAddToBooking.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BlueBtn_MouseDown);
            this.btnAddToBooking.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BlueBtn_MouseUp);
            // 
            // facProdVScrollBar
            // 
            this.facProdVScrollBar.AutoHide = false;
            this.facProdVScrollBar.DataGridView = this.dgvSearchFacilityProducts;
            this.facProdVScrollBar.DownButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("facProdVScrollBar.DownButtonBackgroundImage")));
            this.facProdVScrollBar.DownButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("facProdVScrollBar.DownButtonDisabledBackgroundImage")));
            this.facProdVScrollBar.Location = new System.Drawing.Point(1229, 12);
            this.facProdVScrollBar.Margin = new System.Windows.Forms.Padding(0);
            this.facProdVScrollBar.Name = "facProdVScrollBar";
            this.facProdVScrollBar.ScrollableControl = null;
            this.facProdVScrollBar.ScrollViewer = null;
            this.facProdVScrollBar.Size = new System.Drawing.Size(40, 241);
            this.facProdVScrollBar.TabIndex = 115;
            this.facProdVScrollBar.UpButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("facProdVScrollBar.UpButtonBackgroundImage")));
            this.facProdVScrollBar.UpButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("facProdVScrollBar.UpButtonDisabledBackgroundImage")));
            this.facProdVScrollBar.UpButtonClick += new System.EventHandler(this.ScrollBar_Click);
            this.facProdVScrollBar.DownButtonClick += new System.EventHandler(this.ScrollBar_Click);
            // 
            // cmbSearchFacility
            // 
            this.cmbSearchFacility.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbSearchFacility.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbSearchFacility.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbSearchFacility.FormattingEnabled = true;
            this.cmbSearchFacility.Location = new System.Drawing.Point(75, 54);
            this.cmbSearchFacility.Name = "cmbSearchFacility";
            this.cmbSearchFacility.Size = new System.Drawing.Size(179, 31);
            this.cmbSearchFacility.TabIndex = 107;
            this.cmbSearchFacility.SelectedIndexChanged += new System.EventHandler(this.cmbSearchFacilityProdOrMasterSchedule_SelectedIndexChanged);
            // 
            // lblFacility
            // 
            this.lblFacility.Location = new System.Drawing.Point(6, 54);
            this.lblFacility.Name = "lblFacility";
            this.lblFacility.Size = new System.Drawing.Size(66, 30);
            this.lblFacility.TabIndex = 110;
            this.lblFacility.Text = "Facility Map:";
            this.lblFacility.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbSearchBookingProduct
            // 
            this.cmbSearchBookingProduct.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbSearchBookingProduct.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbSearchBookingProduct.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbSearchBookingProduct.FormattingEnabled = true;
            this.cmbSearchBookingProduct.Items.AddRange(new object[] {
            "Facility",
            "Game"});
            this.cmbSearchBookingProduct.Location = new System.Drawing.Point(438, 53);
            this.cmbSearchBookingProduct.Name = "cmbSearchBookingProduct";
            this.cmbSearchBookingProduct.Size = new System.Drawing.Size(179, 31);
            this.cmbSearchBookingProduct.TabIndex = 109;
            this.cmbSearchBookingProduct.SelectedIndexChanged += new System.EventHandler(this.cmbSearchBookingProduct_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(335, 53);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 30);
            this.label3.TabIndex = 108;
            this.label3.Text = "Booking Product:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTimeSlots
            // 
            this.lblTimeSlots.Location = new System.Drawing.Point(304, 14);
            this.lblTimeSlots.Name = "lblTimeSlots";
            this.lblTimeSlots.Size = new System.Drawing.Size(131, 30);
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
            this.cbxNightSlot.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbxNightSlot.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbxNightSlot.ImageIndex = 1;
            this.cbxNightSlot.Location = new System.Drawing.Point(700, 11);
            this.cbxNightSlot.Name = "cbxNightSlot";
            this.cbxNightSlot.Size = new System.Drawing.Size(80, 36);
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
            this.cbxAfternoonSlot.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbxAfternoonSlot.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbxAfternoonSlot.ImageIndex = 1;
            this.cbxAfternoonSlot.Location = new System.Drawing.Point(614, 11);
            this.cbxAfternoonSlot.Name = "cbxAfternoonSlot";
            this.cbxAfternoonSlot.Size = new System.Drawing.Size(80, 36);
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
            this.cbxMorningSlot.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbxMorningSlot.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbxMorningSlot.ImageIndex = 1;
            this.cbxMorningSlot.Location = new System.Drawing.Point(528, 11);
            this.cbxMorningSlot.Name = "cbxMorningSlot";
            this.cbxMorningSlot.Size = new System.Drawing.Size(80, 36);
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
            this.cbxEarlyMorningSlot.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbxEarlyMorningSlot.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbxEarlyMorningSlot.ImageIndex = 1;
            this.cbxEarlyMorningSlot.Location = new System.Drawing.Point(442, 11);
            this.cbxEarlyMorningSlot.Name = "cbxEarlyMorningSlot";
            this.cbxEarlyMorningSlot.Size = new System.Drawing.Size(80, 36);
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
            this.cmbSearchToTime.Location = new System.Drawing.Point(1108, 14);
            this.cmbSearchToTime.Name = "cmbSearchToTime";
            this.cmbSearchToTime.Size = new System.Drawing.Size(116, 31);
            this.cmbSearchToTime.TabIndex = 95;
            this.cmbSearchToTime.SelectedIndexChanged += new System.EventHandler(this.cmbSearchFromTimeORToTime_SelectedIndexChanged);
            // 
            // cmbSearchFromTime
            // 
            this.cmbSearchFromTime.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbSearchFromTime.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbSearchFromTime.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbSearchFromTime.FormattingEnabled = true;
            this.cmbSearchFromTime.Location = new System.Drawing.Point(893, 14);
            this.cmbSearchFromTime.Name = "cmbSearchFromTime";
            this.cmbSearchFromTime.Size = new System.Drawing.Size(116, 31);
            this.cmbSearchFromTime.TabIndex = 93;
            this.cmbSearchFromTime.SelectedIndexChanged += new System.EventHandler(this.cmbSearchFromTimeORToTime_SelectedIndexChanged);
            // 
            // lblFromTimeSearch
            // 
            this.lblFromTimeSearch.Location = new System.Drawing.Point(818, 14);
            this.lblFromTimeSearch.Name = "lblFromTimeSearch";
            this.lblFromTimeSearch.Size = new System.Drawing.Size(70, 30);
            this.lblFromTimeSearch.TabIndex = 96;
            this.lblFromTimeSearch.Text = "From Time:";
            this.lblFromTimeSearch.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblToTimeSearch
            // 
            this.lblToTimeSearch.Location = new System.Drawing.Point(1051, 14);
            this.lblToTimeSearch.Name = "lblToTimeSearch";
            this.lblToTimeSearch.Size = new System.Drawing.Size(54, 30);
            this.lblToTimeSearch.TabIndex = 97;
            this.lblToTimeSearch.Text = "To Time:";
            this.lblToTimeSearch.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpSearchDate
            // 
            this.dtpSearchDate.CustomFormat = "ddd, dd-MMM-yyyy";
            this.dtpSearchDate.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpSearchDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpSearchDate.Location = new System.Drawing.Point(75, 14);
            this.dtpSearchDate.Name = "dtpSearchDate";
            this.dtpSearchDate.Size = new System.Drawing.Size(214, 30);
            this.dtpSearchDate.TabIndex = 92;
            this.dtpSearchDate.ValueChanged += new System.EventHandler(this.dtpSearchDate_ValueChanged);
            this.dtpSearchDate.Enter += new System.EventHandler(this.dtpSearchDate_Enter);
            // 
            // lblSelectedDate
            // 
            this.lblSelectedDate.Location = new System.Drawing.Point(15, 14);
            this.lblSelectedDate.Name = "lblSelectedDate";
            this.lblSelectedDate.Size = new System.Drawing.Size(57, 30);
            this.lblSelectedDate.TabIndex = 94;
            this.lblSelectedDate.Text = "For Date:";
            this.lblSelectedDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tpCustomer
            // 
            this.tpCustomer.Controls.Add(this.custPanelVScrollBar);
            this.tpCustomer.Controls.Add(this.btnCustomerLookup);
            this.tpCustomer.Controls.Add(this.btnPrevCustomer);
            this.tpCustomer.Controls.Add(this.btnNextCustomer);
            this.tpCustomer.Controls.Add(this.pnlCustomerDetail);
            this.tpCustomer.Controls.Add(this.txtCardNumber);
            this.tpCustomer.Controls.Add(this.lblCardNumber);
            this.tpCustomer.Location = new System.Drawing.Point(4, 44);
            this.tpCustomer.Name = "tpCustomer";
            this.tpCustomer.Padding = new System.Windows.Forms.Padding(3);
            this.tpCustomer.Size = new System.Drawing.Size(1296, 632);
            this.tpCustomer.TabIndex = 1;
            this.tpCustomer.Text = "Customer";
            this.tpCustomer.UseVisualStyleBackColor = true;
            this.tpCustomer.Enter += new System.EventHandler(this.tpCustomer_Enter);
            // 
            // custPanelVScrollBar
            // 
            this.custPanelVScrollBar.AutoHide = false;
            this.custPanelVScrollBar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.custPanelVScrollBar.DataGridView = null;
            this.custPanelVScrollBar.DownButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("custPanelVScrollBar.DownButtonBackgroundImage")));
            this.custPanelVScrollBar.DownButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("custPanelVScrollBar.DownButtonDisabledBackgroundImage")));
            this.custPanelVScrollBar.Location = new System.Drawing.Point(1251, 42);
            this.custPanelVScrollBar.Margin = new System.Windows.Forms.Padding(0);
            this.custPanelVScrollBar.Name = "custPanelVScrollBar";
            this.custPanelVScrollBar.ScrollableControl = this.pnlCustomerDetail;
            this.custPanelVScrollBar.ScrollViewer = null;
            this.custPanelVScrollBar.Size = new System.Drawing.Size(40, 542);
            this.custPanelVScrollBar.TabIndex = 74;
            this.custPanelVScrollBar.UpButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("custPanelVScrollBar.UpButtonBackgroundImage")));
            this.custPanelVScrollBar.UpButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("custPanelVScrollBar.UpButtonDisabledBackgroundImage")));
            this.custPanelVScrollBar.UpButtonClick += new System.EventHandler(this.ScrollBar_Click);
            this.custPanelVScrollBar.DownButtonClick += new System.EventHandler(this.ScrollBar_Click);
            // 
            // pnlCustomerDetail
            // 
            this.pnlCustomerDetail.AutoScroll = true;
            this.pnlCustomerDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlCustomerDetail.Location = new System.Drawing.Point(7, 42);
            this.pnlCustomerDetail.Name = "pnlCustomerDetail";
            this.pnlCustomerDetail.Size = new System.Drawing.Size(1267, 542);
            this.pnlCustomerDetail.TabIndex = 73;
            // 
            // btnCustomerLookup
            // 
            this.btnCustomerLookup.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnCustomerLookup.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCustomerLookup.FlatAppearance.BorderSize = 0;
            this.btnCustomerLookup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCustomerLookup.ForeColor = System.Drawing.Color.White;
            this.btnCustomerLookup.Location = new System.Drawing.Point(400, 590);
            this.btnCustomerLookup.Name = "btnCustomerLookup";
            this.btnCustomerLookup.Size = new System.Drawing.Size(150, 34);
            this.btnCustomerLookup.TabIndex = 75;
            this.btnCustomerLookup.Text = "Customer Lookup";
            this.btnCustomerLookup.UseVisualStyleBackColor = true;
            this.btnCustomerLookup.Click += new System.EventHandler(this.btnCustomerLookup_Click);
            // 
            // btnPrevCustomer
            // 
            this.btnPrevCustomer.BackColor = System.Drawing.Color.Transparent;
            this.btnPrevCustomer.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnPrevCustomer.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPrevCustomer.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnPrevCustomer.FlatAppearance.BorderSize = 0;
            this.btnPrevCustomer.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnPrevCustomer.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPrevCustomer.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPrevCustomer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrevCustomer.ForeColor = System.Drawing.Color.White;
            this.btnPrevCustomer.Location = new System.Drawing.Point(556, 590);
            this.btnPrevCustomer.Name = "btnPrevCustomer";
            this.btnPrevCustomer.Size = new System.Drawing.Size(116, 34);
            this.btnPrevCustomer.TabIndex = 76;
            this.btnPrevCustomer.Text = "Prev";
            this.btnPrevCustomer.UseVisualStyleBackColor = false;
            this.btnPrevCustomer.Click += new System.EventHandler(this.btnPrevCustomer_Click);
            // 
            // btnNextCustomer
            // 
            this.btnNextCustomer.BackColor = System.Drawing.Color.Transparent;
            this.btnNextCustomer.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnNextCustomer.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnNextCustomer.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnNextCustomer.FlatAppearance.BorderSize = 0;
            this.btnNextCustomer.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnNextCustomer.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnNextCustomer.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnNextCustomer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNextCustomer.ForeColor = System.Drawing.Color.White;
            this.btnNextCustomer.Location = new System.Drawing.Point(678, 590);
            this.btnNextCustomer.Name = "btnNextCustomer";
            this.btnNextCustomer.Size = new System.Drawing.Size(116, 34);
            this.btnNextCustomer.TabIndex = 77;
            this.btnNextCustomer.Text = "Next";
            this.btnNextCustomer.UseVisualStyleBackColor = false;
            this.btnNextCustomer.Click += new System.EventHandler(this.btnNextCustomer_Click);
            // 
            // txtCardNumber
            // 
            this.txtCardNumber.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCardNumber.Location = new System.Drawing.Point(91, 6);
            this.txtCardNumber.MaxLength = 12;
            this.txtCardNumber.Name = "txtCardNumber";
            this.txtCardNumber.Size = new System.Drawing.Size(145, 30);
            this.txtCardNumber.TabIndex = 71;
            this.txtCardNumber.Validating += new System.ComponentModel.CancelEventHandler(this.txtCardNumber_Validating);
            // 
            // lblCardNumber
            // 
            this.lblCardNumber.Location = new System.Drawing.Point(22, 5);
            this.lblCardNumber.Name = "lblCardNumber";
            this.lblCardNumber.Size = new System.Drawing.Size(67, 30);
            this.lblCardNumber.TabIndex = 72;
            this.lblCardNumber.Text = "Card:";
            this.lblCardNumber.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tpPackageProducts
            // 
            this.tpPackageProducts.Controls.Add(this.btnPkgTabShowKeyPad);
            this.tpPackageProducts.Controls.Add(this.packageListVScrollBar);
            this.tpPackageProducts.Controls.Add(this.pnlPackageProducts);
            this.tpPackageProducts.Controls.Add(this.btnPrevPackage);
            this.tpPackageProducts.Controls.Add(this.btnNextPackage);
            this.tpPackageProducts.Controls.Add(this.label1);
            this.tpPackageProducts.Controls.Add(this.txtRemarks);
            this.tpPackageProducts.Controls.Add(this.lblPkgQty);
            this.tpPackageProducts.Controls.Add(this.lblPkgProductName);
            this.tpPackageProducts.Controls.Add(this.label4);
            this.tpPackageProducts.Location = new System.Drawing.Point(4, 44);
            this.tpPackageProducts.Name = "tpPackageProducts";
            this.tpPackageProducts.Padding = new System.Windows.Forms.Padding(3);
            this.tpPackageProducts.Size = new System.Drawing.Size(1296, 632);
            this.tpPackageProducts.TabIndex = 2;
            this.tpPackageProducts.Text = "Package";
            this.tpPackageProducts.UseVisualStyleBackColor = true;
            this.tpPackageProducts.Enter += new System.EventHandler(this.tpPackageProducts_Enter);
            this.tpPackageProducts.Leave += new System.EventHandler(this.tpPackageProducts_Leave);
            // 
            // btnPkgTabShowKeyPad
            // 
            this.btnPkgTabShowKeyPad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPkgTabShowKeyPad.BackColor = System.Drawing.Color.Transparent;
            this.btnPkgTabShowKeyPad.BackgroundImage = global::Parafait_POS.Properties.Resources.keyboard;
            this.btnPkgTabShowKeyPad.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPkgTabShowKeyPad.CausesValidation = false;
            this.btnPkgTabShowKeyPad.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnPkgTabShowKeyPad.FlatAppearance.BorderSize = 0;
            this.btnPkgTabShowKeyPad.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPkgTabShowKeyPad.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPkgTabShowKeyPad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPkgTabShowKeyPad.Font = new System.Drawing.Font("Arial", 8.25F);
            this.btnPkgTabShowKeyPad.ForeColor = System.Drawing.Color.Black;
            this.btnPkgTabShowKeyPad.Location = new System.Drawing.Point(459, 463);
            this.btnPkgTabShowKeyPad.Name = "btnPkgTabShowKeyPad";
            this.btnPkgTabShowKeyPad.Size = new System.Drawing.Size(36, 41);
            this.btnPkgTabShowKeyPad.TabIndex = 121;
            this.btnPkgTabShowKeyPad.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnPkgTabShowKeyPad.UseVisualStyleBackColor = false;
            // 
            // packageListVScrollBar
            // 
            this.packageListVScrollBar.AutoHide = false;
            this.packageListVScrollBar.DataGridView = null;
            this.packageListVScrollBar.DownButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("packageListVScrollBar.DownButtonBackgroundImage")));
            this.packageListVScrollBar.DownButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("packageListVScrollBar.DownButtonDisabledBackgroundImage")));
            this.packageListVScrollBar.Location = new System.Drawing.Point(789, 34);
            this.packageListVScrollBar.Margin = new System.Windows.Forms.Padding(0);
            this.packageListVScrollBar.Name = "packageListVScrollBar";
            this.packageListVScrollBar.ScrollableControl = this.pnlPackageProducts;
            this.packageListVScrollBar.ScrollViewer = null;
            this.packageListVScrollBar.Size = new System.Drawing.Size(40, 309);
            this.packageListVScrollBar.TabIndex = 118;
            this.packageListVScrollBar.UpButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("packageListVScrollBar.UpButtonBackgroundImage")));
            this.packageListVScrollBar.UpButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("packageListVScrollBar.UpButtonDisabledBackgroundImage")));
            this.packageListVScrollBar.UpButtonClick += new System.EventHandler(this.ScrollBar_Click);
            this.packageListVScrollBar.DownButtonClick += new System.EventHandler(this.ScrollBar_Click);
            // 
            // pnlPackageProducts
            // 
            this.pnlPackageProducts.AutoScroll = true;
            this.pnlPackageProducts.Controls.Add(this.usrCtrlPkgProductDetails1);
            this.pnlPackageProducts.Location = new System.Drawing.Point(6, 34);
            this.pnlPackageProducts.Name = "pnlPackageProducts";
            this.pnlPackageProducts.Size = new System.Drawing.Size(807, 309);
            this.pnlPackageProducts.TabIndex = 120;
            // 
            // usrCtrlPkgProductDetails1
            // 
            this.usrCtrlPkgProductDetails1.BackColor = System.Drawing.Color.White;
            this.usrCtrlPkgProductDetails1.CbxSelectedProductChecked = false;
            this.usrCtrlPkgProductDetails1.Cursor = System.Windows.Forms.Cursors.Default;
            this.usrCtrlPkgProductDetails1.Location = new System.Drawing.Point(15, 3);
            this.usrCtrlPkgProductDetails1.Name = "usrCtrlPkgProductDetails1";
            this.usrCtrlPkgProductDetails1.Size = new System.Drawing.Size(792, 60);
            this.usrCtrlPkgProductDetails1.TabIndex = 0;
            // 
            // btnPrevPackage
            // 
            this.btnPrevPackage.BackColor = System.Drawing.Color.Transparent;
            this.btnPrevPackage.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnPrevPackage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPrevPackage.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnPrevPackage.FlatAppearance.BorderSize = 0;
            this.btnPrevPackage.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnPrevPackage.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPrevPackage.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPrevPackage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrevPackage.ForeColor = System.Drawing.Color.White;
            this.btnPrevPackage.Location = new System.Drawing.Point(280, 585);
            this.btnPrevPackage.Name = "btnPrevPackage";
            this.btnPrevPackage.Size = new System.Drawing.Size(116, 34);
            this.btnPrevPackage.TabIndex = 116;
            this.btnPrevPackage.Text = "Prev";
            this.btnPrevPackage.UseVisualStyleBackColor = false;
            this.btnPrevPackage.Click += new System.EventHandler(this.btnPrevPackage_Click);
            // 
            // btnNextPackage
            // 
            this.btnNextPackage.BackColor = System.Drawing.Color.Transparent;
            this.btnNextPackage.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnNextPackage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnNextPackage.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnNextPackage.FlatAppearance.BorderSize = 0;
            this.btnNextPackage.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnNextPackage.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnNextPackage.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnNextPackage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNextPackage.ForeColor = System.Drawing.Color.White;
            this.btnNextPackage.Location = new System.Drawing.Point(477, 585);
            this.btnNextPackage.Name = "btnNextPackage";
            this.btnNextPackage.Size = new System.Drawing.Size(116, 34);
            this.btnNextPackage.TabIndex = 117;
            this.btnNextPackage.Text = "Next";
            this.btnNextPackage.UseVisualStyleBackColor = false;
            this.btnNextPackage.Click += new System.EventHandler(this.btnNextPackage_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(10, 415);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 30);
            this.label1.TabIndex = 115;
            this.label1.Text = "Remarks";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtRemarks
            // 
            this.txtRemarks.Location = new System.Drawing.Point(8, 448);
            this.txtRemarks.Multiline = true;
            this.txtRemarks.Name = "txtRemarks";
            this.txtRemarks.Size = new System.Drawing.Size(414, 56);
            this.txtRemarks.TabIndex = 114;
            this.txtRemarks.Enter += new System.EventHandler(this.ScrollBar_Click);
            this.txtRemarks.Leave += new System.EventHandler(this.txtRemarks_Leave);
            // 
            // lblPkgQty
            // 
            this.lblPkgQty.Location = new System.Drawing.Point(495, 8);
            this.lblPkgQty.Name = "lblPkgQty";
            this.lblPkgQty.Size = new System.Drawing.Size(109, 30);
            this.lblPkgQty.TabIndex = 111;
            this.lblPkgQty.Text = "Quantity";
            this.lblPkgQty.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblPkgProductName
            // 
            this.lblPkgProductName.Location = new System.Drawing.Point(93, 8);
            this.lblPkgProductName.Name = "lblPkgProductName";
            this.lblPkgProductName.Size = new System.Drawing.Size(190, 30);
            this.lblPkgProductName.TabIndex = 111;
            this.lblPkgProductName.Text = "Product Name";
            this.lblPkgProductName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(307, 8);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(158, 30);
            this.label4.TabIndex = 122;
            this.label4.Text = "Price Inclusive Quantity";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tpAdditionalProducts
            // 
            this.tpAdditionalProducts.Controls.Add(this.verticalScrollBarView3);
            this.tpAdditionalProducts.Controls.Add(this.btnPrevAdditionalProducts);
            this.tpAdditionalProducts.Controls.Add(this.btnNextAdditionalProducts);
            this.tpAdditionalProducts.Controls.Add(this.pnlAdditionalProducts);
            this.tpAdditionalProducts.Controls.Add(this.lblAdditionalProdQty);
            this.tpAdditionalProducts.Controls.Add(this.lblAdditionalProdPrice);
            this.tpAdditionalProducts.Controls.Add(this.lblAdditionalProdName);
            this.tpAdditionalProducts.Location = new System.Drawing.Point(4, 44);
            this.tpAdditionalProducts.Name = "tpAdditionalProducts";
            this.tpAdditionalProducts.Padding = new System.Windows.Forms.Padding(3);
            this.tpAdditionalProducts.Size = new System.Drawing.Size(1296, 632);
            this.tpAdditionalProducts.TabIndex = 3;
            this.tpAdditionalProducts.Text = "Additional Products";
            this.tpAdditionalProducts.UseVisualStyleBackColor = true;
            this.tpAdditionalProducts.Enter += new System.EventHandler(this.tpAdditionalProducts_Enter);
            this.tpAdditionalProducts.Leave += new System.EventHandler(this.tpAdditionalProducts_Leave);
            // 
            // verticalScrollBarView3
            // 
            this.verticalScrollBarView3.AutoHide = false;
            this.verticalScrollBarView3.DataGridView = null;
            this.verticalScrollBarView3.DownButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("verticalScrollBarView3.DownButtonBackgroundImage")));
            this.verticalScrollBarView3.DownButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("verticalScrollBarView3.DownButtonDisabledBackgroundImage")));
            this.verticalScrollBarView3.Location = new System.Drawing.Point(826, 34);
            this.verticalScrollBarView3.Margin = new System.Windows.Forms.Padding(0);
            this.verticalScrollBarView3.Name = "verticalScrollBarView3";
            this.verticalScrollBarView3.ScrollableControl = this.pnlAdditionalProducts;
            this.verticalScrollBarView3.ScrollViewer = null;
            this.verticalScrollBarView3.Size = new System.Drawing.Size(40, 530);
            this.verticalScrollBarView3.TabIndex = 124;
            this.verticalScrollBarView3.UpButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("verticalScrollBarView3.UpButtonBackgroundImage")));
            this.verticalScrollBarView3.UpButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("verticalScrollBarView3.UpButtonDisabledBackgroundImage")));
            this.verticalScrollBarView3.UpButtonClick += new System.EventHandler(this.ScrollBar_Click);
            this.verticalScrollBarView3.DownButtonClick += new System.EventHandler(this.ScrollBar_Click);
            // 
            // pnlAdditionalProducts
            // 
            this.pnlAdditionalProducts.AutoScroll = true;
            this.pnlAdditionalProducts.Controls.Add(this.usrCtrlAdditionProductDetails1);
            this.pnlAdditionalProducts.Location = new System.Drawing.Point(6, 34);
            this.pnlAdditionalProducts.Name = "pnlAdditionalProducts";
            this.pnlAdditionalProducts.Size = new System.Drawing.Size(838, 531);
            this.pnlAdditionalProducts.TabIndex = 123;
            // 
            // usrCtrlAdditionProductDetails1
            // 
            this.usrCtrlAdditionProductDetails1.BackColor = System.Drawing.Color.White;
            this.usrCtrlAdditionProductDetails1.CbxSelectedProductChecked = false;
            this.usrCtrlAdditionProductDetails1.Cursor = System.Windows.Forms.Cursors.Default;
            this.usrCtrlAdditionProductDetails1.Location = new System.Drawing.Point(15, 3);
            this.usrCtrlAdditionProductDetails1.Name = "usrCtrlAdditionProductDetails1";
            this.usrCtrlAdditionProductDetails1.Size = new System.Drawing.Size(813, 63);
            this.usrCtrlAdditionProductDetails1.TabIndex = 0;
            // 
            // btnPrevAdditionalProducts
            // 
            this.btnPrevAdditionalProducts.BackColor = System.Drawing.Color.Transparent;
            this.btnPrevAdditionalProducts.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnPrevAdditionalProducts.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPrevAdditionalProducts.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnPrevAdditionalProducts.FlatAppearance.BorderSize = 0;
            this.btnPrevAdditionalProducts.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnPrevAdditionalProducts.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPrevAdditionalProducts.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPrevAdditionalProducts.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrevAdditionalProducts.ForeColor = System.Drawing.Color.White;
            this.btnPrevAdditionalProducts.Location = new System.Drawing.Point(277, 585);
            this.btnPrevAdditionalProducts.Name = "btnPrevAdditionalProducts";
            this.btnPrevAdditionalProducts.Size = new System.Drawing.Size(116, 34);
            this.btnPrevAdditionalProducts.TabIndex = 127;
            this.btnPrevAdditionalProducts.Text = "Prev";
            this.btnPrevAdditionalProducts.UseVisualStyleBackColor = false;
            this.btnPrevAdditionalProducts.Click += new System.EventHandler(this.btnPrevAdditionalProducts_Click);
            // 
            // btnNextAdditionalProducts
            // 
            this.btnNextAdditionalProducts.BackColor = System.Drawing.Color.Transparent;
            this.btnNextAdditionalProducts.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnNextAdditionalProducts.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnNextAdditionalProducts.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnNextAdditionalProducts.FlatAppearance.BorderSize = 0;
            this.btnNextAdditionalProducts.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnNextAdditionalProducts.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnNextAdditionalProducts.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnNextAdditionalProducts.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNextAdditionalProducts.ForeColor = System.Drawing.Color.White;
            this.btnNextAdditionalProducts.Location = new System.Drawing.Point(486, 585);
            this.btnNextAdditionalProducts.Name = "btnNextAdditionalProducts";
            this.btnNextAdditionalProducts.Size = new System.Drawing.Size(116, 34);
            this.btnNextAdditionalProducts.TabIndex = 128;
            this.btnNextAdditionalProducts.Text = "Next";
            this.btnNextAdditionalProducts.UseVisualStyleBackColor = false;
            this.btnNextAdditionalProducts.Click += new System.EventHandler(this.btnNextAdditionalProducts_Click);
            // 
            // lblAdditionalProdQty
            // 
            this.lblAdditionalProdQty.Location = new System.Drawing.Point(495, 8);
            this.lblAdditionalProdQty.Name = "lblAdditionalProdQty";
            this.lblAdditionalProdQty.Size = new System.Drawing.Size(109, 30);
            this.lblAdditionalProdQty.TabIndex = 120;
            this.lblAdditionalProdQty.Text = "Quantity";
            this.lblAdditionalProdQty.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblAdditionalProdPrice
            // 
            this.lblAdditionalProdPrice.Location = new System.Drawing.Point(321, 8);
            this.lblAdditionalProdPrice.Name = "lblAdditionalProdPrice";
            this.lblAdditionalProdPrice.Size = new System.Drawing.Size(109, 30);
            this.lblAdditionalProdPrice.TabIndex = 121;
            this.lblAdditionalProdPrice.Text = "Price";
            this.lblAdditionalProdPrice.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblAdditionalProdName
            // 
            this.lblAdditionalProdName.Location = new System.Drawing.Point(134, 8);
            this.lblAdditionalProdName.Name = "lblAdditionalProdName";
            this.lblAdditionalProdName.Size = new System.Drawing.Size(109, 30);
            this.lblAdditionalProdName.TabIndex = 122;
            this.lblAdditionalProdName.Text = "Product Name";
            this.lblAdditionalProdName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tpSummary
            // 
            this.tpSummary.Controls.Add(this.btnBookingCheckList);
            this.tpSummary.Controls.Add(this.btnMapWaivers);
            this.tpSummary.Controls.Add(this.btnNextSummary);
            this.tpSummary.Controls.Add(this.btnSendEmail);
            this.tpSummary.Controls.Add(this.btnPrintBooking);
            this.tpSummary.Controls.Add(this.btnSendPaymentLink);
            this.tpSummary.Controls.Add(this.btnAddTransactionProfile);
            this.tpSummary.Controls.Add(this.btnApplyDiscount);
            this.tpSummary.Controls.Add(this.btnApplyDiscCoupon);
            this.tpSummary.Controls.Add(this.btnEditBooking);
            this.tpSummary.Controls.Add(this.btnClose);
            this.tpSummary.Controls.Add(this.btnPrevSummary);
            this.tpSummary.Controls.Add(this.btnBook);
            this.tpSummary.Controls.Add(this.btnCancelBooking);
            this.tpSummary.Controls.Add(this.btnExecute);
            this.tpSummary.Controls.Add(this.btnConfirm);
            this.tpSummary.Controls.Add(this.btnPayment);
            this.tpSummary.Controls.Add(this.btnAddAttendees);
            this.tpSummary.Controls.Add(this.groupBox3);
            this.tpSummary.Controls.Add(this.label14);
            this.tpSummary.Controls.Add(this.groupBox2);
            this.tpSummary.Controls.Add(this.cbxEmailSent);
            this.tpSummary.Location = new System.Drawing.Point(4, 44);
            this.tpSummary.Name = "tpSummary";
            this.tpSummary.Padding = new System.Windows.Forms.Padding(3);
            this.tpSummary.Size = new System.Drawing.Size(1296, 632);
            this.tpSummary.TabIndex = 4;
            this.tpSummary.Text = "Summary";
            this.tpSummary.UseVisualStyleBackColor = true;
            this.tpSummary.Enter += new System.EventHandler(this.tpSummary_Enter);
            // 
            // btnBookingCheckList
            // 
            this.btnBookingCheckList.BackColor = System.Drawing.Color.Transparent;
            this.btnBookingCheckList.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnBookingCheckList.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnBookingCheckList.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnBookingCheckList.FlatAppearance.BorderSize = 0;
            this.btnBookingCheckList.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnBookingCheckList.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnBookingCheckList.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnBookingCheckList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBookingCheckList.ForeColor = System.Drawing.Color.White;
            this.btnBookingCheckList.Location = new System.Drawing.Point(669, 424);
            this.btnBookingCheckList.Name = "btnBookingCheckList";
            this.btnBookingCheckList.Size = new System.Drawing.Size(119, 34);
            this.btnBookingCheckList.TabIndex = 136;
            this.btnBookingCheckList.Text = "Check List";
            this.btnBookingCheckList.UseVisualStyleBackColor = false;
            this.btnBookingCheckList.Click += new System.EventHandler(this.btnBookingCheckList_Click);
            // 
            // btnMapWaivers
            // 
            this.btnMapWaivers.BackColor = System.Drawing.Color.Transparent;
            this.btnMapWaivers.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnMapWaivers.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnMapWaivers.Enabled = false;
            this.btnMapWaivers.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnMapWaivers.FlatAppearance.BorderSize = 0;
            this.btnMapWaivers.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnMapWaivers.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnMapWaivers.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnMapWaivers.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMapWaivers.ForeColor = System.Drawing.Color.White;
            this.btnMapWaivers.Location = new System.Drawing.Point(538, 476);
            this.btnMapWaivers.Name = "btnMapWaivers";
            this.btnMapWaivers.Size = new System.Drawing.Size(118, 34);
            this.btnMapWaivers.TabIndex = 135;
            this.btnMapWaivers.Text = "Map Waivers";
            this.btnMapWaivers.UseVisualStyleBackColor = false;
            this.btnMapWaivers.Click += new System.EventHandler(this.btnMapWaivers_Click);
            this.btnMapWaivers.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BlueBtn_MouseDown);
            this.btnMapWaivers.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BlueBtn_MouseUp);
            // 
            // btnNextSummary
            // 
            this.btnNextSummary.BackColor = System.Drawing.Color.Transparent;
            this.btnNextSummary.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnNextSummary.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnNextSummary.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnNextSummary.FlatAppearance.BorderSize = 0;
            this.btnNextSummary.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnNextSummary.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnNextSummary.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnNextSummary.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNextSummary.ForeColor = System.Drawing.Color.White;
            this.btnNextSummary.Location = new System.Drawing.Point(687, 585);
            this.btnNextSummary.Name = "btnNextSummary";
            this.btnNextSummary.Size = new System.Drawing.Size(116, 34);
            this.btnNextSummary.TabIndex = 134;
            this.btnNextSummary.Text = "Next";
            this.btnNextSummary.UseVisualStyleBackColor = false;
            this.btnNextSummary.Click += new System.EventHandler(this.btnNextSummary_Click);
            // 
            // btnSendEmail
            // 
            this.btnSendEmail.BackColor = System.Drawing.Color.Transparent;
            this.btnSendEmail.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnSendEmail.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSendEmail.Enabled = false;
            this.btnSendEmail.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnSendEmail.FlatAppearance.BorderSize = 0;
            this.btnSendEmail.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnSendEmail.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSendEmail.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSendEmail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSendEmail.ForeColor = System.Drawing.Color.White;
            this.btnSendEmail.Location = new System.Drawing.Point(538, 424);
            this.btnSendEmail.Name = "btnSendEmail";
            this.btnSendEmail.Size = new System.Drawing.Size(118, 34);
            this.btnSendEmail.TabIndex = 133;
            this.btnSendEmail.Text = "Email Booking";
            this.btnSendEmail.UseVisualStyleBackColor = false;
            this.btnSendEmail.Click += new System.EventHandler(this.btnSendEmail_Click);
            this.btnSendEmail.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BlueBtn_MouseDown);
            this.btnSendEmail.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BlueBtn_MouseUp);
            // 
            // btnPrintBooking
            // 
            this.btnPrintBooking.BackColor = System.Drawing.Color.Transparent;
            this.btnPrintBooking.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnPrintBooking.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPrintBooking.Enabled = false;
            this.btnPrintBooking.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnPrintBooking.FlatAppearance.BorderSize = 0;
            this.btnPrintBooking.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnPrintBooking.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPrintBooking.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPrintBooking.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrintBooking.ForeColor = System.Drawing.Color.White;
            this.btnPrintBooking.Location = new System.Drawing.Point(409, 424);
            this.btnPrintBooking.Name = "btnPrintBooking";
            this.btnPrintBooking.Size = new System.Drawing.Size(118, 34);
            this.btnPrintBooking.TabIndex = 133;
            this.btnPrintBooking.Text = "Print Booking";
            this.btnPrintBooking.UseVisualStyleBackColor = false;
            this.btnPrintBooking.Click += new System.EventHandler(this.btnPrintBooking_Click);
            this.btnPrintBooking.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BlueBtn_MouseDown);
            this.btnPrintBooking.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BlueBtn_MouseUp);
            // 
            // btnSendPaymentLink
            // 
            this.btnSendPaymentLink.BackColor = System.Drawing.Color.Transparent;
            this.btnSendPaymentLink.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnSendPaymentLink.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSendPaymentLink.Enabled = false;
            this.btnSendPaymentLink.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnSendPaymentLink.FlatAppearance.BorderSize = 0;
            this.btnSendPaymentLink.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnSendPaymentLink.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSendPaymentLink.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSendPaymentLink.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSendPaymentLink.ForeColor = System.Drawing.Color.White;
            this.btnSendPaymentLink.Location = new System.Drawing.Point(669, 476);
            this.btnSendPaymentLink.Name = "btnSendPaymentLink";
            this.btnSendPaymentLink.Size = new System.Drawing.Size(118, 34);
            this.btnSendPaymentLink.TabIndex = 133;
            this.btnSendPaymentLink.Text = "Payment Link";
            this.btnSendPaymentLink.UseVisualStyleBackColor = false;
            this.btnSendPaymentLink.Click += new System.EventHandler(this.btnSendPaymentLink_Click);
            this.btnSendPaymentLink.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BlueBtn_MouseDown);
            this.btnSendPaymentLink.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BlueBtn_MouseUp);
            // 
            // btnAddTransactionProfile
            // 
            this.btnAddTransactionProfile.BackColor = System.Drawing.Color.Transparent;
            this.btnAddTransactionProfile.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnAddTransactionProfile.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnAddTransactionProfile.Enabled = false;
            this.btnAddTransactionProfile.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnAddTransactionProfile.FlatAppearance.BorderSize = 0;
            this.btnAddTransactionProfile.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnAddTransactionProfile.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnAddTransactionProfile.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnAddTransactionProfile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddTransactionProfile.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddTransactionProfile.ForeColor = System.Drawing.Color.White;
            this.btnAddTransactionProfile.Location = new System.Drawing.Point(280, 424);
            this.btnAddTransactionProfile.Name = "btnAddTransactionProfile";
            this.btnAddTransactionProfile.Size = new System.Drawing.Size(118, 34);
            this.btnAddTransactionProfile.TabIndex = 133;
            this.btnAddTransactionProfile.Text = "Transaction Profile";
            this.btnAddTransactionProfile.UseVisualStyleBackColor = false;
            this.btnAddTransactionProfile.Click += new System.EventHandler(this.btnAddTransactionProfile_Click);
            this.btnAddTransactionProfile.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BlueBtn_MouseDown);
            this.btnAddTransactionProfile.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BlueBtn_MouseUp);
            // 
            // btnApplyDiscount
            // 
            this.btnApplyDiscount.BackColor = System.Drawing.Color.Transparent;
            this.btnApplyDiscount.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnApplyDiscount.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnApplyDiscount.Enabled = false;
            this.btnApplyDiscount.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnApplyDiscount.FlatAppearance.BorderSize = 0;
            this.btnApplyDiscount.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnApplyDiscount.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnApplyDiscount.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnApplyDiscount.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnApplyDiscount.ForeColor = System.Drawing.Color.White;
            this.btnApplyDiscount.Location = new System.Drawing.Point(151, 424);
            this.btnApplyDiscount.Name = "btnApplyDiscount";
            this.btnApplyDiscount.Size = new System.Drawing.Size(118, 34);
            this.btnApplyDiscount.TabIndex = 133;
            this.btnApplyDiscount.Text = "Apply Discount";
            this.btnApplyDiscount.UseVisualStyleBackColor = false;
            this.btnApplyDiscount.Click += new System.EventHandler(this.btnApplyDiscount_Click);
            this.btnApplyDiscount.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BlueBtn_MouseDown);
            this.btnApplyDiscount.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BlueBtn_MouseUp);
            // 
            // btnApplyDiscCoupon
            // 
            this.btnApplyDiscCoupon.BackColor = System.Drawing.Color.Transparent;
            this.btnApplyDiscCoupon.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnApplyDiscCoupon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnApplyDiscCoupon.Enabled = false;
            this.btnApplyDiscCoupon.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnApplyDiscCoupon.FlatAppearance.BorderSize = 0;
            this.btnApplyDiscCoupon.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnApplyDiscCoupon.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnApplyDiscCoupon.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnApplyDiscCoupon.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnApplyDiscCoupon.ForeColor = System.Drawing.Color.White;
            this.btnApplyDiscCoupon.Location = new System.Drawing.Point(22, 424);
            this.btnApplyDiscCoupon.Name = "btnApplyDiscCoupon";
            this.btnApplyDiscCoupon.Size = new System.Drawing.Size(118, 34);
            this.btnApplyDiscCoupon.TabIndex = 132;
            this.btnApplyDiscCoupon.Text = "Apply Coupon";
            this.btnApplyDiscCoupon.UseVisualStyleBackColor = false;
            this.btnApplyDiscCoupon.Click += new System.EventHandler(this.btnApplyDiscCoupon_Click);
            this.btnApplyDiscCoupon.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BlueBtn_MouseDown);
            this.btnApplyDiscCoupon.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BlueBtn_MouseUp);
            // 
            // btnEditBooking
            // 
            this.btnEditBooking.BackColor = System.Drawing.Color.Transparent;
            this.btnEditBooking.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnEditBooking.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnEditBooking.Enabled = false;
            this.btnEditBooking.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnEditBooking.FlatAppearance.BorderSize = 0;
            this.btnEditBooking.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnEditBooking.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnEditBooking.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnEditBooking.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEditBooking.ForeColor = System.Drawing.Color.White;
            this.btnEditBooking.Location = new System.Drawing.Point(294, 585);
            this.btnEditBooking.Name = "btnEditBooking";
            this.btnEditBooking.Size = new System.Drawing.Size(116, 34);
            this.btnEditBooking.TabIndex = 128;
            this.btnEditBooking.Text = "Edit Booking";
            this.btnEditBooking.UseVisualStyleBackColor = false;
            this.btnEditBooking.Click += new System.EventHandler(this.btnEditBooking_Click);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnClose.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(556, 585);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(116, 34);
            this.btnClose.TabIndex = 130;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnPrevSummary
            // 
            this.btnPrevSummary.BackColor = System.Drawing.Color.Transparent;
            this.btnPrevSummary.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnPrevSummary.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPrevSummary.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnPrevSummary.FlatAppearance.BorderSize = 0;
            this.btnPrevSummary.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnPrevSummary.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPrevSummary.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPrevSummary.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrevSummary.ForeColor = System.Drawing.Color.White;
            this.btnPrevSummary.Location = new System.Drawing.Point(32, 585);
            this.btnPrevSummary.Name = "btnPrevSummary";
            this.btnPrevSummary.Size = new System.Drawing.Size(116, 34);
            this.btnPrevSummary.TabIndex = 126;
            this.btnPrevSummary.Text = "Prev";
            this.btnPrevSummary.UseVisualStyleBackColor = false;
            this.btnPrevSummary.Click += new System.EventHandler(this.btnPrevConfirm_Click);
            // 
            // btnBook
            // 
            this.btnBook.BackColor = System.Drawing.Color.Transparent;
            this.btnBook.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnBook.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnBook.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnBook.FlatAppearance.BorderSize = 0;
            this.btnBook.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnBook.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnBook.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnBook.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBook.ForeColor = System.Drawing.Color.White;
            this.btnBook.Location = new System.Drawing.Point(163, 585);
            this.btnBook.Name = "btnBook";
            this.btnBook.Size = new System.Drawing.Size(116, 34);
            this.btnBook.TabIndex = 127;
            this.btnBook.Text = "Book";
            this.btnBook.UseVisualStyleBackColor = false;
            this.btnBook.Click += new System.EventHandler(this.btnBook_Click);
            // 
            // btnCancelBooking
            // 
            this.btnCancelBooking.BackColor = System.Drawing.Color.Transparent;
            this.btnCancelBooking.BackgroundImage = global::Parafait_POS.Properties.Resources.discount_button;
            this.btnCancelBooking.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCancelBooking.FlatAppearance.BorderColor = System.Drawing.Color.LightSalmon;
            this.btnCancelBooking.FlatAppearance.BorderSize = 0;
            this.btnCancelBooking.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnCancelBooking.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCancelBooking.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCancelBooking.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelBooking.ForeColor = System.Drawing.Color.Black;
            this.btnCancelBooking.Location = new System.Drawing.Point(425, 585);
            this.btnCancelBooking.Name = "btnCancelBooking";
            this.btnCancelBooking.Size = new System.Drawing.Size(116, 34);
            this.btnCancelBooking.TabIndex = 129;
            this.btnCancelBooking.Text = "Cancel Booking";
            this.btnCancelBooking.UseVisualStyleBackColor = false;
            this.btnCancelBooking.Click += new System.EventHandler(this.btnCancelBooking_Click);
            // 
            // btnExecute
            // 
            this.btnExecute.BackColor = System.Drawing.Color.Transparent;
            this.btnExecute.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnExecute.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnExecute.Enabled = false;
            this.btnExecute.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnExecute.FlatAppearance.BorderSize = 0;
            this.btnExecute.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnExecute.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnExecute.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnExecute.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExecute.ForeColor = System.Drawing.Color.White;
            this.btnExecute.Location = new System.Drawing.Point(410, 476);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(118, 34);
            this.btnExecute.TabIndex = 125;
            this.btnExecute.Text = "Execute Booking";
            this.btnExecute.UseVisualStyleBackColor = false;
            this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
            this.btnExecute.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BlueBtn_MouseDown);
            this.btnExecute.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BlueBtn_MouseUp);
            // 
            // btnConfirm
            // 
            this.btnConfirm.BackColor = System.Drawing.Color.Transparent;
            this.btnConfirm.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnConfirm.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnConfirm.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnConfirm.FlatAppearance.BorderSize = 0;
            this.btnConfirm.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnConfirm.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnConfirm.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnConfirm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConfirm.ForeColor = System.Drawing.Color.White;
            this.btnConfirm.Location = new System.Drawing.Point(281, 476);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(118, 34);
            this.btnConfirm.TabIndex = 124;
            this.btnConfirm.Text = "Confirm Booking";
            this.btnConfirm.UseVisualStyleBackColor = false;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            this.btnConfirm.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BlueBtn_MouseDown);
            this.btnConfirm.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BlueBtn_MouseUp);
            // 
            // btnPayment
            // 
            this.btnPayment.BackColor = System.Drawing.Color.Transparent;
            this.btnPayment.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnPayment.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPayment.Enabled = false;
            this.btnPayment.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnPayment.FlatAppearance.BorderSize = 0;
            this.btnPayment.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnPayment.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPayment.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPayment.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPayment.ForeColor = System.Drawing.Color.White;
            this.btnPayment.Location = new System.Drawing.Point(152, 476);
            this.btnPayment.Name = "btnPayment";
            this.btnPayment.Size = new System.Drawing.Size(118, 34);
            this.btnPayment.TabIndex = 123;
            this.btnPayment.Text = "Payment";
            this.btnPayment.UseVisualStyleBackColor = false;
            this.btnPayment.Click += new System.EventHandler(this.btnPayment_Click);
            this.btnPayment.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BlueBtn_MouseDown);
            this.btnPayment.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BlueBtn_MouseUp);
            // 
            // btnAddAttendees
            // 
            this.btnAddAttendees.BackColor = System.Drawing.Color.Transparent;
            this.btnAddAttendees.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnAddAttendees.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnAddAttendees.Enabled = false;
            this.btnAddAttendees.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnAddAttendees.FlatAppearance.BorderSize = 0;
            this.btnAddAttendees.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnAddAttendees.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnAddAttendees.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnAddAttendees.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddAttendees.ForeColor = System.Drawing.Color.White;
            this.btnAddAttendees.Location = new System.Drawing.Point(23, 476);
            this.btnAddAttendees.Name = "btnAddAttendees";
            this.btnAddAttendees.Size = new System.Drawing.Size(118, 34);
            this.btnAddAttendees.TabIndex = 122;
            this.btnAddAttendees.Text = "Attendees";
            this.btnAddAttendees.UseVisualStyleBackColor = false;
            this.btnAddAttendees.Click += new System.EventHandler(this.btnAddAttendees_Click);
            this.btnAddAttendees.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BlueBtn_MouseDown);
            this.btnAddAttendees.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BlueBtn_MouseUp);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.gbxCharges);
            this.groupBox3.Controls.Add(this.gbxSvcServiceCharge);
            this.groupBox3.Controls.Add(this.pcbRemoveDiscCoupon);
            this.groupBox3.Controls.Add(this.txtAppliedDiscountCoupon);
            this.groupBox3.Controls.Add(this.lblAppliedDiscount);
            this.groupBox3.Controls.Add(this.lblAppliedDiscountCoupon);
            this.groupBox3.Controls.Add(this.txtAppliedDiscount);
            this.groupBox3.Controls.Add(this.pcbRemoveDiscount);
            this.groupBox3.Controls.Add(this.lblAppliedTransactionProfile);
            this.groupBox3.Controls.Add(this.txtAppliedTransactionProfile);
            this.groupBox3.Location = new System.Drawing.Point(8, 241);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(798, 136);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Applied to Reservation";
            // 
            // gbxCharges
            // 
            this.gbxCharges.Controls.Add(this.pcbRemoveGratuityAmount);
            this.gbxCharges.Controls.Add(this.btnApplyGratuityAmount);
            this.gbxCharges.Controls.Add(this.txtGratuityAmount);
            this.gbxCharges.Controls.Add(this.txtServiceChrageAmount);
            this.gbxCharges.Controls.Add(this.pcbRemoveServiceCharge);
            this.gbxCharges.Controls.Add(this.lblGratuityAmount);
            this.gbxCharges.Controls.Add(this.lblServiceChargeAmount);
            this.gbxCharges.Controls.Add(this.btnApplyServiceCharge);
            this.gbxCharges.Location = new System.Drawing.Point(450, 13);
            this.gbxCharges.Name = "gbxCharges";
            this.gbxCharges.Size = new System.Drawing.Size(326, 102);
            this.gbxCharges.TabIndex = 126;
            this.gbxCharges.TabStop = false;
            this.gbxCharges.Text = "Charges";
            // 
            // pcbRemoveGratuityAmount
            // 
            this.pcbRemoveGratuityAmount.Image = global::Parafait_POS.Properties.Resources.R_Remove_Btn_Normal;
            this.pcbRemoveGratuityAmount.Location = new System.Drawing.Point(265, 59);
            this.pcbRemoveGratuityAmount.Name = "pcbRemoveGratuityAmount";
            this.pcbRemoveGratuityAmount.Size = new System.Drawing.Size(30, 30);
            this.pcbRemoveGratuityAmount.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pcbRemoveGratuityAmount.TabIndex = 139;
            this.pcbRemoveGratuityAmount.TabStop = false;
            this.pcbRemoveGratuityAmount.Click += new System.EventHandler(this.pcbRemoveGratuityAmount_Click);
            // 
            // btnApplyGratuityAmount
            // 
            this.btnApplyGratuityAmount.BackColor = System.Drawing.Color.Transparent;
            this.btnApplyGratuityAmount.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnApplyGratuityAmount.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnApplyGratuityAmount.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnApplyGratuityAmount.FlatAppearance.BorderSize = 0;
            this.btnApplyGratuityAmount.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnApplyGratuityAmount.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnApplyGratuityAmount.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnApplyGratuityAmount.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnApplyGratuityAmount.ForeColor = System.Drawing.Color.White;
            this.btnApplyGratuityAmount.Location = new System.Drawing.Point(263, 57);
            this.btnApplyGratuityAmount.Name = "btnApplyGratuityAmount";
            this.btnApplyGratuityAmount.Size = new System.Drawing.Size(52, 34);
            this.btnApplyGratuityAmount.TabIndex = 137;
            this.btnApplyGratuityAmount.Text = "Apply";
            this.btnApplyGratuityAmount.UseVisualStyleBackColor = false;
            this.btnApplyGratuityAmount.Click += new System.EventHandler(this.btnApplyGratuityAmount_Click);
            // 
            // txtGratuityAmount
            // 
            this.txtGratuityAmount.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtGratuityAmount.Location = new System.Drawing.Point(136, 59);
            this.txtGratuityAmount.MaxLength = 50;
            this.txtGratuityAmount.Name = "txtGratuityAmount";
            this.txtGratuityAmount.ReadOnly = true;
            this.txtGratuityAmount.Size = new System.Drawing.Size(121, 30);
            this.txtGratuityAmount.TabIndex = 123;
            // 
            // txtServiceChrageAmount
            // 
            this.txtServiceChrageAmount.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtServiceChrageAmount.Location = new System.Drawing.Point(136, 20);
            this.txtServiceChrageAmount.MaxLength = 50;
            this.txtServiceChrageAmount.Name = "txtServiceChrageAmount";
            this.txtServiceChrageAmount.ReadOnly = true;
            this.txtServiceChrageAmount.Size = new System.Drawing.Size(121, 30);
            this.txtServiceChrageAmount.TabIndex = 122;
            // 
            // pcbRemoveServiceCharge
            // 
            this.pcbRemoveServiceCharge.Image = global::Parafait_POS.Properties.Resources.R_Remove_Btn_Normal;
            this.pcbRemoveServiceCharge.Location = new System.Drawing.Point(265, 20);
            this.pcbRemoveServiceCharge.Name = "pcbRemoveServiceCharge";
            this.pcbRemoveServiceCharge.Size = new System.Drawing.Size(30, 30);
            this.pcbRemoveServiceCharge.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pcbRemoveServiceCharge.TabIndex = 0;
            this.pcbRemoveServiceCharge.TabStop = false;
            this.pcbRemoveServiceCharge.Click += new System.EventHandler(this.pcbRemoveServiceCharge_Click);
            // 
            // lblGratuityAmount
            // 
            this.lblGratuityAmount.Location = new System.Drawing.Point(20, 59);
            this.lblGratuityAmount.Name = "lblGratuityAmount";
            this.lblGratuityAmount.Size = new System.Drawing.Size(109, 30);
            this.lblGratuityAmount.TabIndex = 121;
            this.lblGratuityAmount.Text = "Gratuity Amount:";
            this.lblGratuityAmount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblServiceChargeAmount
            // 
            this.lblServiceChargeAmount.Location = new System.Drawing.Point(20, 20);
            this.lblServiceChargeAmount.Name = "lblServiceChargeAmount";
            this.lblServiceChargeAmount.Size = new System.Drawing.Size(109, 30);
            this.lblServiceChargeAmount.TabIndex = 121;
            this.lblServiceChargeAmount.Text = "Service Charges:";
            this.lblServiceChargeAmount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnApplyServiceCharge
            // 
            this.btnApplyServiceCharge.BackColor = System.Drawing.Color.Transparent;
            this.btnApplyServiceCharge.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnApplyServiceCharge.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnApplyServiceCharge.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnApplyServiceCharge.FlatAppearance.BorderSize = 0;
            this.btnApplyServiceCharge.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnApplyServiceCharge.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnApplyServiceCharge.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnApplyServiceCharge.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnApplyServiceCharge.ForeColor = System.Drawing.Color.White;
            this.btnApplyServiceCharge.Location = new System.Drawing.Point(263, 18);
            this.btnApplyServiceCharge.Name = "btnApplyServiceCharge";
            this.btnApplyServiceCharge.Size = new System.Drawing.Size(52, 34);
            this.btnApplyServiceCharge.TabIndex = 138;
            this.btnApplyServiceCharge.Text = "Apply";
            this.btnApplyServiceCharge.UseVisualStyleBackColor = false;
            this.btnApplyServiceCharge.Click += new System.EventHandler(this.btnApplyServiceCharge_Click);
            // 
            // gbxSvcServiceCharge
            // 
            this.gbxSvcServiceCharge.Controls.Add(this.txtSvcServiceChargePercentage);
            this.gbxSvcServiceCharge.Controls.Add(this.txtSvcServiceChargeAmount);
            this.gbxSvcServiceCharge.Controls.Add(this.pbcSvcRemoveServiceCharge);
            this.gbxSvcServiceCharge.Controls.Add(this.lblSvcServiceChargePercentage);
            this.gbxSvcServiceCharge.Controls.Add(this.lblSvcServiceChargeAmount);
            this.gbxSvcServiceCharge.Location = new System.Drawing.Point(450, 13);
            this.gbxSvcServiceCharge.Name = "gbxSvcServiceCharge";
            this.gbxSvcServiceCharge.Size = new System.Drawing.Size(303, 102);
            this.gbxSvcServiceCharge.TabIndex = 127;
            this.gbxSvcServiceCharge.TabStop = false;
            this.gbxSvcServiceCharge.Text = "Service Charge";
            // 
            // txtSvcServiceChargePercentage
            // 
            this.txtSvcServiceChargePercentage.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSvcServiceChargePercentage.Location = new System.Drawing.Point(150, 43);
            this.txtSvcServiceChargePercentage.MaxLength = 50;
            this.txtSvcServiceChargePercentage.Name = "txtSvcServiceChargePercentage";
            this.txtSvcServiceChargePercentage.Size = new System.Drawing.Size(102, 30);
            this.txtSvcServiceChargePercentage.TabIndex = 72;
            this.txtSvcServiceChargePercentage.TextChanged += new System.EventHandler(this.txtSvcServiceCharge_TextChanged);
            this.txtSvcServiceChargePercentage.Enter += new System.EventHandler(this.txtSvcServiceCharge_Enter);
            // 
            // txtSvcServiceChargeAmount
            // 
            this.txtSvcServiceChargeAmount.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSvcServiceChargeAmount.Location = new System.Drawing.Point(42, 43);
            this.txtSvcServiceChargeAmount.MaxLength = 50;
            this.txtSvcServiceChargeAmount.Name = "txtSvcServiceChargeAmount";
            this.txtSvcServiceChargeAmount.Size = new System.Drawing.Size(102, 30);
            this.txtSvcServiceChargeAmount.TabIndex = 71;
            this.txtSvcServiceChargeAmount.TextChanged += new System.EventHandler(this.txtSvcServiceCharge_TextChanged);
            this.txtSvcServiceChargeAmount.Enter += new System.EventHandler(this.txtSvcServiceCharge_Enter);
            // 
            // pbcSvcRemoveServiceCharge
            // 
            this.pbcSvcRemoveServiceCharge.Image = global::Parafait_POS.Properties.Resources.R_Remove_Btn_Normal;
            this.pbcSvcRemoveServiceCharge.Location = new System.Drawing.Point(6, 43);
            this.pbcSvcRemoveServiceCharge.Name = "pbcSvcRemoveServiceCharge";
            this.pbcSvcRemoveServiceCharge.Size = new System.Drawing.Size(30, 30);
            this.pbcSvcRemoveServiceCharge.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbcSvcRemoveServiceCharge.TabIndex = 0;
            this.pbcSvcRemoveServiceCharge.TabStop = false;
            this.pbcSvcRemoveServiceCharge.Click += new System.EventHandler(this.pcbSvcRemoveServiceCharge_Click);
            // 
            // lblSvcServiceChargePercentage
            // 
            this.lblSvcServiceChargePercentage.Location = new System.Drawing.Point(147, 17);
            this.lblSvcServiceChargePercentage.Name = "lblSvcServiceChargePercentage";
            this.lblSvcServiceChargePercentage.Size = new System.Drawing.Size(109, 30);
            this.lblSvcServiceChargePercentage.TabIndex = 121;
            this.lblSvcServiceChargePercentage.Text = "Percentage";
            this.lblSvcServiceChargePercentage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSvcServiceChargeAmount
            // 
            this.lblSvcServiceChargeAmount.Location = new System.Drawing.Point(39, 17);
            this.lblSvcServiceChargeAmount.Name = "lblSvcServiceChargeAmount";
            this.lblSvcServiceChargeAmount.Size = new System.Drawing.Size(109, 30);
            this.lblSvcServiceChargeAmount.TabIndex = 121;
            this.lblSvcServiceChargeAmount.Text = "Amount";
            this.lblSvcServiceChargeAmount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pcbRemoveDiscCoupon
            // 
            this.pcbRemoveDiscCoupon.Image = global::Parafait_POS.Properties.Resources.R_Remove_Btn_Normal;
            this.pcbRemoveDiscCoupon.Location = new System.Drawing.Point(386, 20);
            this.pcbRemoveDiscCoupon.Name = "pcbRemoveDiscCoupon";
            this.pcbRemoveDiscCoupon.Size = new System.Drawing.Size(30, 30);
            this.pcbRemoveDiscCoupon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pcbRemoveDiscCoupon.TabIndex = 121;
            this.pcbRemoveDiscCoupon.TabStop = false;
            this.pcbRemoveDiscCoupon.Click += new System.EventHandler(this.pcbRemoveDiscCoupon_Click);
            this.pcbRemoveDiscCoupon.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pcbRemove_MouseDown);
            this.pcbRemoveDiscCoupon.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pcbRemove_MouseUp);
            // 
            // txtAppliedDiscountCoupon
            // 
            this.txtAppliedDiscountCoupon.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAppliedDiscountCoupon.Location = new System.Drawing.Point(154, 20);
            this.txtAppliedDiscountCoupon.MaxLength = 50;
            this.txtAppliedDiscountCoupon.Name = "txtAppliedDiscountCoupon";
            this.txtAppliedDiscountCoupon.ReadOnly = true;
            this.txtAppliedDiscountCoupon.Size = new System.Drawing.Size(226, 30);
            this.txtAppliedDiscountCoupon.TabIndex = 120;
            // 
            // lblAppliedDiscount
            // 
            this.lblAppliedDiscount.Location = new System.Drawing.Point(35, 58);
            this.lblAppliedDiscount.Name = "lblAppliedDiscount";
            this.lblAppliedDiscount.Size = new System.Drawing.Size(113, 30);
            this.lblAppliedDiscount.TabIndex = 119;
            this.lblAppliedDiscount.Text = "Discount:";
            this.lblAppliedDiscount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblAppliedDiscountCoupon
            // 
            this.lblAppliedDiscountCoupon.Location = new System.Drawing.Point(38, 20);
            this.lblAppliedDiscountCoupon.Name = "lblAppliedDiscountCoupon";
            this.lblAppliedDiscountCoupon.Size = new System.Drawing.Size(113, 30);
            this.lblAppliedDiscountCoupon.TabIndex = 119;
            this.lblAppliedDiscountCoupon.Text = "Discount Coupon:";
            this.lblAppliedDiscountCoupon.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtAppliedDiscount
            // 
            this.txtAppliedDiscount.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAppliedDiscount.Location = new System.Drawing.Point(154, 58);
            this.txtAppliedDiscount.MaxLength = 50;
            this.txtAppliedDiscount.Name = "txtAppliedDiscount";
            this.txtAppliedDiscount.ReadOnly = true;
            this.txtAppliedDiscount.Size = new System.Drawing.Size(226, 30);
            this.txtAppliedDiscount.TabIndex = 120;
            // 
            // pcbRemoveDiscount
            // 
            this.pcbRemoveDiscount.Image = global::Parafait_POS.Properties.Resources.R_Remove_Btn_Normal;
            this.pcbRemoveDiscount.Location = new System.Drawing.Point(386, 56);
            this.pcbRemoveDiscount.Name = "pcbRemoveDiscount";
            this.pcbRemoveDiscount.Size = new System.Drawing.Size(30, 30);
            this.pcbRemoveDiscount.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pcbRemoveDiscount.TabIndex = 121;
            this.pcbRemoveDiscount.TabStop = false;
            this.pcbRemoveDiscount.Click += new System.EventHandler(this.pcbRemoveDiscount_Click);
            this.pcbRemoveDiscount.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pcbRemove_MouseDown);
            this.pcbRemoveDiscount.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pcbRemove_MouseUp);
            // 
            // lblAppliedTransactionProfile
            // 
            this.lblAppliedTransactionProfile.Location = new System.Drawing.Point(38, 96);
            this.lblAppliedTransactionProfile.Name = "lblAppliedTransactionProfile";
            this.lblAppliedTransactionProfile.Size = new System.Drawing.Size(113, 30);
            this.lblAppliedTransactionProfile.TabIndex = 119;
            this.lblAppliedTransactionProfile.Text = "Transaction Profile:";
            this.lblAppliedTransactionProfile.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtAppliedTransactionProfile
            // 
            this.txtAppliedTransactionProfile.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAppliedTransactionProfile.Location = new System.Drawing.Point(154, 96);
            this.txtAppliedTransactionProfile.MaxLength = 50;
            this.txtAppliedTransactionProfile.Name = "txtAppliedTransactionProfile";
            this.txtAppliedTransactionProfile.ReadOnly = true;
            this.txtAppliedTransactionProfile.Size = new System.Drawing.Size(226, 30);
            this.txtAppliedTransactionProfile.TabIndex = 120;
            // 
            // label14
            // 
            this.label14.Location = new System.Drawing.Point(417, 261);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(113, 30);
            this.label14.TabIndex = 119;
            this.label14.Text = "Reservation Code:";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtSvcTrxServiceCharges);
            this.groupBox2.Controls.Add(this.lblSvcServiceChargeSummary);
            this.groupBox2.Controls.Add(this.txtDiscountAmount);
            this.groupBox2.Controls.Add(this.lblDiscountAmount);
            this.groupBox2.Controls.Add(this.txtBalanceAmount);
            this.groupBox2.Controls.Add(this.lblBalanceAmount);
            this.groupBox2.Controls.Add(this.txtEstimateAmount);
            this.groupBox2.Controls.Add(this.lblEstimateAmount);
            this.groupBox2.Controls.Add(this.txtAdvancePaid);
            this.groupBox2.Controls.Add(this.lblPaid);
            this.groupBox2.Controls.Add(this.txtMinimumAdvanceAmount);
            this.groupBox2.Controls.Add(this.lblMinimumAdvAmount);
            this.groupBox2.Controls.Add(this.txtTransactionAmount);
            this.groupBox2.Controls.Add(this.lblTransactionAmount);
            this.groupBox2.Controls.Add(this.txtReservationStatus);
            this.groupBox2.Controls.Add(this.lblReservationStatus);
            this.groupBox2.Controls.Add(this.txtExpiryDate);
            this.groupBox2.Controls.Add(this.lblExpiryDate);
            this.groupBox2.Controls.Add(this.txtReservationCode);
            this.groupBox2.Controls.Add(this.lblReservationCode);
            this.groupBox2.Location = new System.Drawing.Point(30, 7);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(754, 214);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            // 
            // txtSvcTrxServiceCharges
            // 
            this.txtSvcTrxServiceCharges.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSvcTrxServiceCharges.Location = new System.Drawing.Point(530, 164);
            this.txtSvcTrxServiceCharges.MaxLength = 50;
            this.txtSvcTrxServiceCharges.Name = "txtSvcTrxServiceCharges";
            this.txtSvcTrxServiceCharges.ReadOnly = true;
            this.txtSvcTrxServiceCharges.Size = new System.Drawing.Size(121, 30);
            this.txtSvcTrxServiceCharges.TabIndex = 122;
            // 
            // lblSvcServiceChargeSummary
            // 
            this.lblSvcServiceChargeSummary.Location = new System.Drawing.Point(414, 164);
            this.lblSvcServiceChargeSummary.Name = "lblSvcServiceChargeSummary";
            this.lblSvcServiceChargeSummary.Size = new System.Drawing.Size(113, 30);
            this.lblSvcServiceChargeSummary.TabIndex = 121;
            this.lblSvcServiceChargeSummary.Text = "Service Charge:";
            this.lblSvcServiceChargeSummary.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtDiscountAmount
            // 
            this.txtDiscountAmount.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDiscountAmount.Location = new System.Drawing.Point(530, 56);
            this.txtDiscountAmount.MaxLength = 50;
            this.txtDiscountAmount.Name = "txtDiscountAmount";
            this.txtDiscountAmount.ReadOnly = true;
            this.txtDiscountAmount.Size = new System.Drawing.Size(121, 30);
            this.txtDiscountAmount.TabIndex = 120;
            // 
            // lblDiscountAmount
            // 
            this.lblDiscountAmount.Location = new System.Drawing.Point(414, 56);
            this.lblDiscountAmount.Name = "lblDiscountAmount";
            this.lblDiscountAmount.Size = new System.Drawing.Size(113, 30);
            this.lblDiscountAmount.TabIndex = 119;
            this.lblDiscountAmount.Text = "Discount Amount:";
            this.lblDiscountAmount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtBalanceAmount
            // 
            this.txtBalanceAmount.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBalanceAmount.Location = new System.Drawing.Point(530, 128);
            this.txtBalanceAmount.MaxLength = 50;
            this.txtBalanceAmount.Name = "txtBalanceAmount";
            this.txtBalanceAmount.ReadOnly = true;
            this.txtBalanceAmount.Size = new System.Drawing.Size(121, 30);
            this.txtBalanceAmount.TabIndex = 120;
            // 
            // lblBalanceAmount
            // 
            this.lblBalanceAmount.Location = new System.Drawing.Point(414, 128);
            this.lblBalanceAmount.Name = "lblBalanceAmount";
            this.lblBalanceAmount.Size = new System.Drawing.Size(113, 30);
            this.lblBalanceAmount.TabIndex = 119;
            this.lblBalanceAmount.Text = "Balance Amount:";
            this.lblBalanceAmount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtEstimateAmount
            // 
            this.txtEstimateAmount.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEstimateAmount.Location = new System.Drawing.Point(168, 128);
            this.txtEstimateAmount.MaxLength = 50;
            this.txtEstimateAmount.Name = "txtEstimateAmount";
            this.txtEstimateAmount.ReadOnly = true;
            this.txtEstimateAmount.Size = new System.Drawing.Size(121, 30);
            this.txtEstimateAmount.TabIndex = 120;
            // 
            // lblEstimateAmount
            // 
            this.lblEstimateAmount.Location = new System.Drawing.Point(52, 128);
            this.lblEstimateAmount.Name = "lblEstimateAmount";
            this.lblEstimateAmount.Size = new System.Drawing.Size(113, 30);
            this.lblEstimateAmount.TabIndex = 119;
            this.lblEstimateAmount.Text = "Estimate Amount:";
            this.lblEstimateAmount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtAdvancePaid
            // 
            this.txtAdvancePaid.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAdvancePaid.Location = new System.Drawing.Point(530, 92);
            this.txtAdvancePaid.MaxLength = 50;
            this.txtAdvancePaid.Name = "txtAdvancePaid";
            this.txtAdvancePaid.ReadOnly = true;
            this.txtAdvancePaid.Size = new System.Drawing.Size(121, 30);
            this.txtAdvancePaid.TabIndex = 120;
            // 
            // lblPaid
            // 
            this.lblPaid.Location = new System.Drawing.Point(414, 92);
            this.lblPaid.Name = "lblPaid";
            this.lblPaid.Size = new System.Drawing.Size(113, 30);
            this.lblPaid.TabIndex = 119;
            this.lblPaid.Text = "Paid:";
            this.lblPaid.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtMinimumAdvanceAmount
            // 
            this.txtMinimumAdvanceAmount.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMinimumAdvanceAmount.Location = new System.Drawing.Point(168, 92);
            this.txtMinimumAdvanceAmount.MaxLength = 50;
            this.txtMinimumAdvanceAmount.Name = "txtMinimumAdvanceAmount";
            this.txtMinimumAdvanceAmount.ReadOnly = true;
            this.txtMinimumAdvanceAmount.Size = new System.Drawing.Size(121, 30);
            this.txtMinimumAdvanceAmount.TabIndex = 120;
            // 
            // lblMinimumAdvAmount
            // 
            this.lblMinimumAdvAmount.Location = new System.Drawing.Point(6, 92);
            this.lblMinimumAdvAmount.Name = "lblMinimumAdvAmount";
            this.lblMinimumAdvAmount.Size = new System.Drawing.Size(159, 30);
            this.lblMinimumAdvAmount.TabIndex = 119;
            this.lblMinimumAdvAmount.Text = "Minimum Advance Amount:";
            this.lblMinimumAdvAmount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtTransactionAmount
            // 
            this.txtTransactionAmount.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTransactionAmount.Location = new System.Drawing.Point(168, 56);
            this.txtTransactionAmount.MaxLength = 50;
            this.txtTransactionAmount.Name = "txtTransactionAmount";
            this.txtTransactionAmount.ReadOnly = true;
            this.txtTransactionAmount.Size = new System.Drawing.Size(121, 30);
            this.txtTransactionAmount.TabIndex = 120;
            // 
            // lblTransactionAmount
            // 
            this.lblTransactionAmount.Location = new System.Drawing.Point(32, 56);
            this.lblTransactionAmount.Name = "lblTransactionAmount";
            this.lblTransactionAmount.Size = new System.Drawing.Size(133, 30);
            this.lblTransactionAmount.TabIndex = 119;
            this.lblTransactionAmount.Text = "Transaction Amount:";
            this.lblTransactionAmount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtReservationStatus
            // 
            this.txtReservationStatus.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtReservationStatus.Location = new System.Drawing.Point(530, 20);
            this.txtReservationStatus.MaxLength = 50;
            this.txtReservationStatus.Name = "txtReservationStatus";
            this.txtReservationStatus.ReadOnly = true;
            this.txtReservationStatus.Size = new System.Drawing.Size(164, 30);
            this.txtReservationStatus.TabIndex = 120;
            // 
            // lblReservationStatus
            // 
            this.lblReservationStatus.Location = new System.Drawing.Point(414, 20);
            this.lblReservationStatus.Name = "lblReservationStatus";
            this.lblReservationStatus.Size = new System.Drawing.Size(113, 30);
            this.lblReservationStatus.TabIndex = 119;
            this.lblReservationStatus.Text = "Status:";
            this.lblReservationStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtExpiryDate
            // 
            this.txtExpiryDate.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtExpiryDate.Location = new System.Drawing.Point(168, 164);
            this.txtExpiryDate.MaxLength = 50;
            this.txtExpiryDate.Name = "txtExpiryDate";
            this.txtExpiryDate.ReadOnly = true;
            this.txtExpiryDate.Size = new System.Drawing.Size(226, 30);
            this.txtExpiryDate.TabIndex = 120;
            // 
            // lblExpiryDate
            // 
            this.lblExpiryDate.Location = new System.Drawing.Point(52, 164);
            this.lblExpiryDate.Name = "lblExpiryDate";
            this.lblExpiryDate.Size = new System.Drawing.Size(113, 30);
            this.lblExpiryDate.TabIndex = 119;
            this.lblExpiryDate.Text = "Expiry Date:";
            this.lblExpiryDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtReservationCode
            // 
            this.txtReservationCode.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtReservationCode.Location = new System.Drawing.Point(168, 20);
            this.txtReservationCode.MaxLength = 50;
            this.txtReservationCode.Name = "txtReservationCode";
            this.txtReservationCode.ReadOnly = true;
            this.txtReservationCode.Size = new System.Drawing.Size(226, 30);
            this.txtReservationCode.TabIndex = 120;
            // 
            // lblReservationCode
            // 
            this.lblReservationCode.Location = new System.Drawing.Point(52, 20);
            this.lblReservationCode.Name = "lblReservationCode";
            this.lblReservationCode.Size = new System.Drawing.Size(113, 30);
            this.lblReservationCode.TabIndex = 119;
            this.lblReservationCode.Text = "Reservation Code:";
            this.lblReservationCode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbxEmailSent
            // 
            this.cbxEmailSent.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbxEmailSent.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.cbxEmailSent.Enabled = false;
            this.cbxEmailSent.FlatAppearance.BorderSize = 0;
            this.cbxEmailSent.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbxEmailSent.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbxEmailSent.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cbxEmailSent.ImageIndex = 1;
            this.cbxEmailSent.Location = new System.Drawing.Point(18, 385);
            this.cbxEmailSent.Name = "cbxEmailSent";
            this.cbxEmailSent.Size = new System.Drawing.Size(141, 30);
            this.cbxEmailSent.TabIndex = 131;
            this.cbxEmailSent.Text = "Email Sent";
            this.cbxEmailSent.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbxEmailSent.UseVisualStyleBackColor = true;
            // 
            // tpCheckList
            // 
            this.tpCheckList.Controls.Add(this.btnSaveCheckList);
            this.tpCheckList.Controls.Add(this.btnPrevCheckList);
            this.tpCheckList.Controls.Add(this.btnNextCheckList);
            this.tpCheckList.Controls.Add(this.grpCheckList);
            this.tpCheckList.Location = new System.Drawing.Point(4, 44);
            this.tpCheckList.Name = "tpCheckList";
            this.tpCheckList.Padding = new System.Windows.Forms.Padding(3);
            this.tpCheckList.Size = new System.Drawing.Size(1296, 632);
            this.tpCheckList.TabIndex = 5;
            this.tpCheckList.Text = "Check List";
            this.tpCheckList.UseVisualStyleBackColor = true;
            this.tpCheckList.Enter += new System.EventHandler(this.tpCheckList_Enter);
            // 
            // btnSaveCheckList
            // 
            this.btnSaveCheckList.BackColor = System.Drawing.Color.Transparent;
            this.btnSaveCheckList.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnSaveCheckList.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSaveCheckList.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnSaveCheckList.FlatAppearance.BorderSize = 0;
            this.btnSaveCheckList.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnSaveCheckList.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSaveCheckList.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSaveCheckList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveCheckList.ForeColor = System.Drawing.Color.White;
            this.btnSaveCheckList.Location = new System.Drawing.Point(408, 585);
            this.btnSaveCheckList.Name = "btnSaveCheckList";
            this.btnSaveCheckList.Size = new System.Drawing.Size(116, 34);
            this.btnSaveCheckList.TabIndex = 128;
            this.btnSaveCheckList.Text = "Save Check List";
            this.btnSaveCheckList.UseVisualStyleBackColor = false;
            this.btnSaveCheckList.Click += new System.EventHandler(this.btnSaveCheckList_Click);
            // 
            // btnPrevCheckList
            // 
            this.btnPrevCheckList.BackColor = System.Drawing.Color.Transparent;
            this.btnPrevCheckList.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnPrevCheckList.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPrevCheckList.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnPrevCheckList.FlatAppearance.BorderSize = 0;
            this.btnPrevCheckList.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnPrevCheckList.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPrevCheckList.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPrevCheckList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrevCheckList.ForeColor = System.Drawing.Color.White;
            this.btnPrevCheckList.Location = new System.Drawing.Point(255, 585);
            this.btnPrevCheckList.Name = "btnPrevCheckList";
            this.btnPrevCheckList.Size = new System.Drawing.Size(116, 34);
            this.btnPrevCheckList.TabIndex = 118;
            this.btnPrevCheckList.Text = "Prev";
            this.btnPrevCheckList.UseVisualStyleBackColor = false;
            this.btnPrevCheckList.Click += new System.EventHandler(this.btnPrevCheckList_Click);
            // 
            // btnNextCheckList
            // 
            this.btnNextCheckList.BackColor = System.Drawing.Color.Transparent;
            this.btnNextCheckList.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnNextCheckList.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnNextCheckList.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnNextCheckList.FlatAppearance.BorderSize = 0;
            this.btnNextCheckList.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnNextCheckList.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnNextCheckList.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnNextCheckList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNextCheckList.ForeColor = System.Drawing.Color.White;
            this.btnNextCheckList.Location = new System.Drawing.Point(557, 585);
            this.btnNextCheckList.Name = "btnNextCheckList";
            this.btnNextCheckList.Size = new System.Drawing.Size(116, 34);
            this.btnNextCheckList.TabIndex = 119;
            this.btnNextCheckList.Text = "Next";
            this.btnNextCheckList.UseVisualStyleBackColor = false;
            this.btnNextCheckList.Click += new System.EventHandler(this.btnNextCheckList_Click);
            // 
            // grpCheckList
            // 
            this.grpCheckList.Controls.Add(this.cmbEventCheckList);
            this.grpCheckList.Controls.Add(this.chkListHScrollBar);
            this.grpCheckList.Controls.Add(this.chkListVScroolBar);
            this.grpCheckList.Controls.Add(this.dgvUserJobTaskList);
            this.grpCheckList.Controls.Add(this.btnPrintCheckList);
            this.grpCheckList.Controls.Add(this.lblCheckListName);
            this.grpCheckList.Location = new System.Drawing.Point(7, 6);
            this.grpCheckList.Name = "grpCheckList";
            this.grpCheckList.Size = new System.Drawing.Size(1241, 489);
            this.grpCheckList.TabIndex = 0;
            this.grpCheckList.TabStop = false;
            // 
            // cmbEventCheckList
            // 
            this.cmbEventCheckList.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbEventCheckList.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbEventCheckList.FormattingEnabled = true;
            this.cmbEventCheckList.Location = new System.Drawing.Point(93, 17);
            this.cmbEventCheckList.Name = "cmbEventCheckList";
            this.cmbEventCheckList.Size = new System.Drawing.Size(179, 31);
            this.cmbEventCheckList.TabIndex = 126;
            this.cmbEventCheckList.SelectedIndexChanged += new System.EventHandler(this.cmbEventCheckList_SelectedIndexChanged);
            // 
            // chkListHScrollBar
            // 
            this.chkListHScrollBar.AutoHide = false;
            this.chkListHScrollBar.DataGridView = this.dgvUserJobTaskList;
            this.chkListHScrollBar.LeftButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("chkListHScrollBar.LeftButtonBackgroundImage")));
            this.chkListHScrollBar.LeftButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("chkListHScrollBar.LeftButtonDisabledBackgroundImage")));
            this.chkListHScrollBar.Location = new System.Drawing.Point(18, 422);
            this.chkListHScrollBar.Margin = new System.Windows.Forms.Padding(0);
            this.chkListHScrollBar.Name = "chkListHScrollBar";
            this.chkListHScrollBar.RightButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("chkListHScrollBar.RightButtonBackgroundImage")));
            this.chkListHScrollBar.RightButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("chkListHScrollBar.RightButtonDisabledBackgroundImage")));
            this.chkListHScrollBar.ScrollableControl = null;
            this.chkListHScrollBar.ScrollViewer = null;
            this.chkListHScrollBar.Size = new System.Drawing.Size(1176, 40);
            this.chkListHScrollBar.TabIndex = 125;
            this.chkListHScrollBar.LeftButtonClick += new System.EventHandler(this.ScrollBar_Click);
            this.chkListHScrollBar.RightButtonClick += new System.EventHandler(this.ScrollBar_Click);
            // 
            // dgvUserJobTaskList
            // 
            this.dgvUserJobTaskList.AllowUserToAddRows = false;
            this.dgvUserJobTaskList.AllowUserToDeleteRows = false;
            this.dgvUserJobTaskList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUserJobTaskList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.remarksMandatoryDataGridViewCheckBoxColumn,
            this.chklistValue,
            this.validateTagDataGridViewCheckBoxColumn,
            this.isActiveDataGridViewCheckBoxColumn,
            this.assignedUserId,
            this.checkListStatus,
            this.maintChklstdetId,
            this.maintJobNameDataGridViewTextBoxColumn,
            this.taskNameDataGridViewTextBoxColumn,
            this.chklstScheduleTimeDataGridViewTextBoxColumn,
            this.jobTaskIdDataGridViewTextBoxColumn,
            this.maintJobTypeDataGridViewTextBoxColumn,
            this.assignedToDataGridViewTextBoxColumn,
            this.departmentIdDataGridViewTextBoxColumn,
            this.chklistRemarks,
            this.checklistCloseDate,
            this.cardIdDataGridViewTextBoxColumn,
            this.cardNumberDataGridViewTextBoxColumn,
            this.taskCardNumberDataGridViewTextBoxColumn,
            this.assetIdDataGridViewTextBoxColumn,
            this.assetNameDataGridViewTextBoxColumn,
            this.assetTypeDataGridViewTextBoxColumn,
            this.assetGroupNameDataGridViewTextBoxColumn,
            this.sourceSystemIdDataGridViewTextBoxColumn,
            this.durationToCompleteDataGridViewTextBoxColumn,
            this.requestTypeDataGridViewTextBoxColumn,
            this.requestDateDataGridViewTextBoxColumn,
            this.priorityDataGridViewTextBoxColumn,
            this.requestDetailDataGridViewTextBoxColumn,
            this.imageNameDataGridViewTextBoxColumn,
            this.requestedByDataGridViewTextBoxColumn,
            this.contactPhoneDataGridViewTextBoxColumn,
            this.contactEmailIdDataGridViewTextBoxColumn,
            this.resolutionDataGridViewTextBoxColumn,
            this.commentsDataGridViewTextBoxColumn,
            this.repairCostDataGridViewTextBoxColumn,
            this.docFileNameDataGridViewTextBoxColumn,
            this.attribute1DataGridViewTextBoxColumn});
            this.dgvUserJobTaskList.Location = new System.Drawing.Point(18, 62);
            this.dgvUserJobTaskList.MultiSelect = false;
            this.dgvUserJobTaskList.Name = "dgvUserJobTaskList";
            this.dgvUserJobTaskList.RowHeadersVisible = false;
            this.dgvUserJobTaskList.RowTemplate.Height = 30;
            this.dgvUserJobTaskList.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvUserJobTaskList.Size = new System.Drawing.Size(1176, 357);
            this.dgvUserJobTaskList.TabIndex = 123;
            this.dgvUserJobTaskList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvUserJobTaskList_CellClick);
            this.dgvUserJobTaskList.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvUserJobTaskList_CellEnter);
            this.dgvUserJobTaskList.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvUserJobTaskList_CellFormatting);
            // 
            // remarksMandatoryDataGridViewCheckBoxColumn
            // 
            this.remarksMandatoryDataGridViewCheckBoxColumn.DataPropertyName = "RemarksMandatory";
            this.remarksMandatoryDataGridViewCheckBoxColumn.HeaderText = "Remarks Mandatory?";
            this.remarksMandatoryDataGridViewCheckBoxColumn.Name = "remarksMandatoryDataGridViewCheckBoxColumn";
            this.remarksMandatoryDataGridViewCheckBoxColumn.Visible = false;
            // 
            // chklistValue
            // 
            this.chklistValue.DataPropertyName = "ChklistValue";
            this.chklistValue.FalseValue = "false";
            this.chklistValue.HeaderText = "Checked?";
            this.chklistValue.MinimumWidth = 80;
            this.chklistValue.Name = "chklistValue";
            this.chklistValue.ReadOnly = true;
            this.chklistValue.TrueValue = "true";
            this.chklistValue.Width = 80;
            // 
            // validateTagDataGridViewCheckBoxColumn
            // 
            this.validateTagDataGridViewCheckBoxColumn.DataPropertyName = "ValidateTag";
            this.validateTagDataGridViewCheckBoxColumn.HeaderText = "Validate Tag?";
            this.validateTagDataGridViewCheckBoxColumn.Name = "validateTagDataGridViewCheckBoxColumn";
            this.validateTagDataGridViewCheckBoxColumn.Visible = false;
            // 
            // isActiveDataGridViewCheckBoxColumn
            // 
            this.isActiveDataGridViewCheckBoxColumn.DataPropertyName = "IsActive";
            this.isActiveDataGridViewCheckBoxColumn.HeaderText = "Active?";
            this.isActiveDataGridViewCheckBoxColumn.Name = "isActiveDataGridViewCheckBoxColumn";
            this.isActiveDataGridViewCheckBoxColumn.Visible = false;
            // 
            // assignedUserId
            // 
            this.assignedUserId.DataPropertyName = "AssignedUserId";
            this.assignedUserId.HeaderText = "Assigned User";
            this.assignedUserId.MinimumWidth = 150;
            this.assignedUserId.Name = "assignedUserId";
            this.assignedUserId.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.assignedUserId.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.assignedUserId.Width = 150;
            // 
            // checkListStatus
            // 
            this.checkListStatus.DataPropertyName = "Status";
            this.checkListStatus.HeaderText = "Status";
            this.checkListStatus.MinimumWidth = 100;
            this.checkListStatus.Name = "checkListStatus";
            this.checkListStatus.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.checkListStatus.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // maintChklstdetId
            // 
            this.maintChklstdetId.DataPropertyName = "MaintChklstdetId";
            this.maintChklstdetId.HeaderText = "Job Item ID";
            this.maintChklstdetId.Name = "maintChklstdetId";
            this.maintChklstdetId.ReadOnly = true;
            this.maintChklstdetId.Visible = false;
            // 
            // maintJobNameDataGridViewTextBoxColumn
            // 
            this.maintJobNameDataGridViewTextBoxColumn.DataPropertyName = "MaintJobName";
            this.maintJobNameDataGridViewTextBoxColumn.HeaderText = "Job Name";
            this.maintJobNameDataGridViewTextBoxColumn.MinimumWidth = 150;
            this.maintJobNameDataGridViewTextBoxColumn.Name = "maintJobNameDataGridViewTextBoxColumn";
            this.maintJobNameDataGridViewTextBoxColumn.Visible = false;
            this.maintJobNameDataGridViewTextBoxColumn.Width = 150;
            // 
            // taskNameDataGridViewTextBoxColumn
            // 
            this.taskNameDataGridViewTextBoxColumn.DataPropertyName = "TaskName";
            this.taskNameDataGridViewTextBoxColumn.HeaderText = "Task Name";
            this.taskNameDataGridViewTextBoxColumn.Name = "taskNameDataGridViewTextBoxColumn";
            this.taskNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // chklstScheduleTimeDataGridViewTextBoxColumn
            // 
            this.chklstScheduleTimeDataGridViewTextBoxColumn.DataPropertyName = "ChklstScheduleTime";
            this.chklstScheduleTimeDataGridViewTextBoxColumn.HeaderText = "Schedule Date";
            this.chklstScheduleTimeDataGridViewTextBoxColumn.MinimumWidth = 120;
            this.chklstScheduleTimeDataGridViewTextBoxColumn.Name = "chklstScheduleTimeDataGridViewTextBoxColumn";
            this.chklstScheduleTimeDataGridViewTextBoxColumn.ReadOnly = true;
            this.chklstScheduleTimeDataGridViewTextBoxColumn.Width = 120;
            // 
            // jobTaskIdDataGridViewTextBoxColumn
            // 
            this.jobTaskIdDataGridViewTextBoxColumn.DataPropertyName = "JobTaskId";
            this.jobTaskIdDataGridViewTextBoxColumn.HeaderText = "JOb Task Id";
            this.jobTaskIdDataGridViewTextBoxColumn.Name = "jobTaskIdDataGridViewTextBoxColumn";
            this.jobTaskIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // maintJobTypeDataGridViewTextBoxColumn
            // 
            this.maintJobTypeDataGridViewTextBoxColumn.DataPropertyName = "MaintJobType";
            this.maintJobTypeDataGridViewTextBoxColumn.HeaderText = "Job Type";
            this.maintJobTypeDataGridViewTextBoxColumn.Name = "maintJobTypeDataGridViewTextBoxColumn";
            this.maintJobTypeDataGridViewTextBoxColumn.Visible = false;
            // 
            // assignedToDataGridViewTextBoxColumn
            // 
            this.assignedToDataGridViewTextBoxColumn.DataPropertyName = "AssignedTo";
            this.assignedToDataGridViewTextBoxColumn.HeaderText = "Assigned To";
            this.assignedToDataGridViewTextBoxColumn.Name = "assignedToDataGridViewTextBoxColumn";
            this.assignedToDataGridViewTextBoxColumn.Visible = false;
            // 
            // departmentIdDataGridViewTextBoxColumn
            // 
            this.departmentIdDataGridViewTextBoxColumn.DataPropertyName = "DepartmentId";
            this.departmentIdDataGridViewTextBoxColumn.HeaderText = "Department";
            this.departmentIdDataGridViewTextBoxColumn.Name = "departmentIdDataGridViewTextBoxColumn";
            this.departmentIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // chklistRemarks
            // 
            this.chklistRemarks.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.chklistRemarks.DataPropertyName = "ChklistRemarks";
            this.chklistRemarks.HeaderText = "Remarks";
            this.chklistRemarks.MinimumWidth = 200;
            this.chklistRemarks.Name = "chklistRemarks";
            // 
            // checklistCloseDate
            // 
            this.checklistCloseDate.DataPropertyName = "ChecklistCloseDate";
            this.checklistCloseDate.HeaderText = "Close Date";
            this.checklistCloseDate.MinimumWidth = 120;
            this.checklistCloseDate.Name = "checklistCloseDate";
            this.checklistCloseDate.ReadOnly = true;
            this.checklistCloseDate.Width = 120;
            // 
            // cardIdDataGridViewTextBoxColumn
            // 
            this.cardIdDataGridViewTextBoxColumn.DataPropertyName = "CardId";
            this.cardIdDataGridViewTextBoxColumn.HeaderText = "Card Id";
            this.cardIdDataGridViewTextBoxColumn.Name = "cardIdDataGridViewTextBoxColumn";
            this.cardIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // cardNumberDataGridViewTextBoxColumn
            // 
            this.cardNumberDataGridViewTextBoxColumn.DataPropertyName = "CardNumber";
            this.cardNumberDataGridViewTextBoxColumn.HeaderText = "Card Number";
            this.cardNumberDataGridViewTextBoxColumn.Name = "cardNumberDataGridViewTextBoxColumn";
            this.cardNumberDataGridViewTextBoxColumn.Visible = false;
            // 
            // taskCardNumberDataGridViewTextBoxColumn
            // 
            this.taskCardNumberDataGridViewTextBoxColumn.DataPropertyName = "TaskCardNumber";
            this.taskCardNumberDataGridViewTextBoxColumn.HeaderText = "Task Card Number";
            this.taskCardNumberDataGridViewTextBoxColumn.Name = "taskCardNumberDataGridViewTextBoxColumn";
            this.taskCardNumberDataGridViewTextBoxColumn.Visible = false;
            // 
            // assetIdDataGridViewTextBoxColumn
            // 
            this.assetIdDataGridViewTextBoxColumn.DataPropertyName = "AssetId";
            this.assetIdDataGridViewTextBoxColumn.HeaderText = "Asset";
            this.assetIdDataGridViewTextBoxColumn.Name = "assetIdDataGridViewTextBoxColumn";
            this.assetIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // assetNameDataGridViewTextBoxColumn
            // 
            this.assetNameDataGridViewTextBoxColumn.DataPropertyName = "AssetName";
            this.assetNameDataGridViewTextBoxColumn.HeaderText = "Asset Name";
            this.assetNameDataGridViewTextBoxColumn.Name = "assetNameDataGridViewTextBoxColumn";
            this.assetNameDataGridViewTextBoxColumn.Visible = false;
            // 
            // assetTypeDataGridViewTextBoxColumn
            // 
            this.assetTypeDataGridViewTextBoxColumn.DataPropertyName = "AssetType";
            this.assetTypeDataGridViewTextBoxColumn.HeaderText = "Asset Type";
            this.assetTypeDataGridViewTextBoxColumn.Name = "assetTypeDataGridViewTextBoxColumn";
            this.assetTypeDataGridViewTextBoxColumn.Visible = false;
            // 
            // assetGroupNameDataGridViewTextBoxColumn
            // 
            this.assetGroupNameDataGridViewTextBoxColumn.DataPropertyName = "AssetGroupName";
            this.assetGroupNameDataGridViewTextBoxColumn.HeaderText = "Asset Group Name";
            this.assetGroupNameDataGridViewTextBoxColumn.Name = "assetGroupNameDataGridViewTextBoxColumn";
            this.assetGroupNameDataGridViewTextBoxColumn.Visible = false;
            // 
            // sourceSystemIdDataGridViewTextBoxColumn
            // 
            this.sourceSystemIdDataGridViewTextBoxColumn.DataPropertyName = "SourceSystemId";
            this.sourceSystemIdDataGridViewTextBoxColumn.HeaderText = "Source System Id";
            this.sourceSystemIdDataGridViewTextBoxColumn.Name = "sourceSystemIdDataGridViewTextBoxColumn";
            this.sourceSystemIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // durationToCompleteDataGridViewTextBoxColumn
            // 
            this.durationToCompleteDataGridViewTextBoxColumn.DataPropertyName = "DurationToComplete";
            this.durationToCompleteDataGridViewTextBoxColumn.HeaderText = "Duration To Complete";
            this.durationToCompleteDataGridViewTextBoxColumn.Name = "durationToCompleteDataGridViewTextBoxColumn";
            this.durationToCompleteDataGridViewTextBoxColumn.Visible = false;
            // 
            // requestTypeDataGridViewTextBoxColumn
            // 
            this.requestTypeDataGridViewTextBoxColumn.DataPropertyName = "RequestType";
            this.requestTypeDataGridViewTextBoxColumn.HeaderText = "Request Type";
            this.requestTypeDataGridViewTextBoxColumn.Name = "requestTypeDataGridViewTextBoxColumn";
            this.requestTypeDataGridViewTextBoxColumn.Visible = false;
            // 
            // requestDateDataGridViewTextBoxColumn
            // 
            this.requestDateDataGridViewTextBoxColumn.DataPropertyName = "RequestDate";
            this.requestDateDataGridViewTextBoxColumn.HeaderText = "Request Date";
            this.requestDateDataGridViewTextBoxColumn.Name = "requestDateDataGridViewTextBoxColumn";
            this.requestDateDataGridViewTextBoxColumn.Visible = false;
            // 
            // priorityDataGridViewTextBoxColumn
            // 
            this.priorityDataGridViewTextBoxColumn.DataPropertyName = "Priority";
            this.priorityDataGridViewTextBoxColumn.HeaderText = "Priority";
            this.priorityDataGridViewTextBoxColumn.Name = "priorityDataGridViewTextBoxColumn";
            this.priorityDataGridViewTextBoxColumn.Visible = false;
            // 
            // requestDetailDataGridViewTextBoxColumn
            // 
            this.requestDetailDataGridViewTextBoxColumn.DataPropertyName = "RequestDetail";
            this.requestDetailDataGridViewTextBoxColumn.HeaderText = "Request Detail";
            this.requestDetailDataGridViewTextBoxColumn.Name = "requestDetailDataGridViewTextBoxColumn";
            this.requestDetailDataGridViewTextBoxColumn.Visible = false;
            // 
            // imageNameDataGridViewTextBoxColumn
            // 
            this.imageNameDataGridViewTextBoxColumn.DataPropertyName = "ImageName";
            this.imageNameDataGridViewTextBoxColumn.HeaderText = "Image Name";
            this.imageNameDataGridViewTextBoxColumn.Name = "imageNameDataGridViewTextBoxColumn";
            this.imageNameDataGridViewTextBoxColumn.Visible = false;
            // 
            // requestedByDataGridViewTextBoxColumn
            // 
            this.requestedByDataGridViewTextBoxColumn.DataPropertyName = "RequestedBy";
            this.requestedByDataGridViewTextBoxColumn.HeaderText = "Requested By";
            this.requestedByDataGridViewTextBoxColumn.Name = "requestedByDataGridViewTextBoxColumn";
            this.requestedByDataGridViewTextBoxColumn.Visible = false;
            // 
            // contactPhoneDataGridViewTextBoxColumn
            // 
            this.contactPhoneDataGridViewTextBoxColumn.DataPropertyName = "ContactPhone";
            this.contactPhoneDataGridViewTextBoxColumn.HeaderText = "Contact Phone";
            this.contactPhoneDataGridViewTextBoxColumn.Name = "contactPhoneDataGridViewTextBoxColumn";
            this.contactPhoneDataGridViewTextBoxColumn.Visible = false;
            // 
            // contactEmailIdDataGridViewTextBoxColumn
            // 
            this.contactEmailIdDataGridViewTextBoxColumn.DataPropertyName = "ContactEmailId";
            this.contactEmailIdDataGridViewTextBoxColumn.HeaderText = "Contact EmailId";
            this.contactEmailIdDataGridViewTextBoxColumn.Name = "contactEmailIdDataGridViewTextBoxColumn";
            this.contactEmailIdDataGridViewTextBoxColumn.Visible = false;
            // 
            // resolutionDataGridViewTextBoxColumn
            // 
            this.resolutionDataGridViewTextBoxColumn.DataPropertyName = "Resolution";
            this.resolutionDataGridViewTextBoxColumn.HeaderText = "Resolution";
            this.resolutionDataGridViewTextBoxColumn.Name = "resolutionDataGridViewTextBoxColumn";
            this.resolutionDataGridViewTextBoxColumn.Visible = false;
            // 
            // commentsDataGridViewTextBoxColumn
            // 
            this.commentsDataGridViewTextBoxColumn.DataPropertyName = "Comments";
            this.commentsDataGridViewTextBoxColumn.HeaderText = "Comments";
            this.commentsDataGridViewTextBoxColumn.Name = "commentsDataGridViewTextBoxColumn";
            this.commentsDataGridViewTextBoxColumn.Visible = false;
            // 
            // repairCostDataGridViewTextBoxColumn
            // 
            this.repairCostDataGridViewTextBoxColumn.DataPropertyName = "RepairCost";
            this.repairCostDataGridViewTextBoxColumn.HeaderText = "Repair Cost";
            this.repairCostDataGridViewTextBoxColumn.Name = "repairCostDataGridViewTextBoxColumn";
            this.repairCostDataGridViewTextBoxColumn.Visible = false;
            // 
            // docFileNameDataGridViewTextBoxColumn
            // 
            this.docFileNameDataGridViewTextBoxColumn.DataPropertyName = "DocFileName";
            this.docFileNameDataGridViewTextBoxColumn.HeaderText = "Doc File Name";
            this.docFileNameDataGridViewTextBoxColumn.Name = "docFileNameDataGridViewTextBoxColumn";
            this.docFileNameDataGridViewTextBoxColumn.Visible = false;
            // 
            // attribute1DataGridViewTextBoxColumn
            // 
            this.attribute1DataGridViewTextBoxColumn.DataPropertyName = "Attribute1";
            this.attribute1DataGridViewTextBoxColumn.HeaderText = "Attribute";
            this.attribute1DataGridViewTextBoxColumn.Name = "attribute1DataGridViewTextBoxColumn";
            this.attribute1DataGridViewTextBoxColumn.Visible = false;
            // 
            // chkListVScroolBar
            // 
            this.chkListVScroolBar.AutoHide = false;
            this.chkListVScroolBar.DataGridView = this.dgvUserJobTaskList;
            this.chkListVScroolBar.DownButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("chkListVScroolBar.DownButtonBackgroundImage")));
            this.chkListVScroolBar.DownButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("chkListVScroolBar.DownButtonDisabledBackgroundImage")));
            this.chkListVScroolBar.Location = new System.Drawing.Point(1196, 62);
            this.chkListVScroolBar.Margin = new System.Windows.Forms.Padding(0);
            this.chkListVScroolBar.Name = "chkListVScroolBar";
            this.chkListVScroolBar.ScrollableControl = null;
            this.chkListVScroolBar.ScrollViewer = null;
            this.chkListVScroolBar.Size = new System.Drawing.Size(40, 357);
            this.chkListVScroolBar.TabIndex = 124;
            this.chkListVScroolBar.UpButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("chkListVScroolBar.UpButtonBackgroundImage")));
            this.chkListVScroolBar.UpButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("chkListVScroolBar.UpButtonDisabledBackgroundImage")));
            this.chkListVScroolBar.UpButtonClick += new System.EventHandler(this.ScrollBar_Click);
            this.chkListVScroolBar.DownButtonClick += new System.EventHandler(this.ScrollBar_Click);
            // 
            // btnPrintCheckList
            // 
            this.btnPrintCheckList.BackColor = System.Drawing.Color.Transparent;
            this.btnPrintCheckList.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnPrintCheckList.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPrintCheckList.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnPrintCheckList.FlatAppearance.BorderSize = 0;
            this.btnPrintCheckList.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnPrintCheckList.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPrintCheckList.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPrintCheckList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrintCheckList.ForeColor = System.Drawing.Color.White;
            this.btnPrintCheckList.Location = new System.Drawing.Point(1089, 20);
            this.btnPrintCheckList.Name = "btnPrintCheckList";
            this.btnPrintCheckList.Size = new System.Drawing.Size(104, 36);
            this.btnPrintCheckList.TabIndex = 122;
            this.btnPrintCheckList.Text = "Print";
            this.btnPrintCheckList.UseVisualStyleBackColor = false;
            this.btnPrintCheckList.Click += new System.EventHandler(this.btnPrintCheckList_Click);
            this.btnPrintCheckList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BlueBtn_MouseDown);
            this.btnPrintCheckList.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BlueBtn_MouseUp);
            // 
            // lblCheckListName
            // 
            this.lblCheckListName.Location = new System.Drawing.Point(6, 17);
            this.lblCheckListName.Name = "lblCheckListName";
            this.lblCheckListName.Size = new System.Drawing.Size(84, 30);
            this.lblCheckListName.TabIndex = 120;
            this.lblCheckListName.Text = "Check List:";
            this.lblCheckListName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tpAuditDetails
            // 
            this.tpAuditDetails.Controls.Add(this.btnPrevAuditTrail);
            this.tpAuditDetails.Controls.Add(this.lblAuditTrail);
            this.tpAuditDetails.Controls.Add(this.dgvAuditTrail);
            this.tpAuditDetails.Controls.Add(this.auditRailHScrollBar);
            this.tpAuditDetails.Controls.Add(this.auditTrailVScrollBar);
            this.tpAuditDetails.Location = new System.Drawing.Point(4, 44);
            this.tpAuditDetails.Name = "tpAuditDetails";
            this.tpAuditDetails.Padding = new System.Windows.Forms.Padding(3);
            this.tpAuditDetails.Size = new System.Drawing.Size(1296, 632);
            this.tpAuditDetails.TabIndex = 6;
            this.tpAuditDetails.Text = "Audit Trail";
            this.tpAuditDetails.UseVisualStyleBackColor = true;
            this.tpAuditDetails.Enter += new System.EventHandler(this.tpAuditDetails_Enter);
            // 
            // btnPrevAuditTrail
            // 
            this.btnPrevAuditTrail.BackColor = System.Drawing.Color.Transparent;
            this.btnPrevAuditTrail.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            this.btnPrevAuditTrail.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPrevAuditTrail.FlatAppearance.BorderColor = System.Drawing.Color.Turquoise;
            this.btnPrevAuditTrail.FlatAppearance.BorderSize = 0;
            this.btnPrevAuditTrail.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnPrevAuditTrail.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPrevAuditTrail.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPrevAuditTrail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrevAuditTrail.ForeColor = System.Drawing.Color.White;
            this.btnPrevAuditTrail.Location = new System.Drawing.Point(255, 585);
            this.btnPrevAuditTrail.Name = "btnPrevAuditTrail";
            this.btnPrevAuditTrail.Size = new System.Drawing.Size(116, 34);
            this.btnPrevAuditTrail.TabIndex = 127;
            this.btnPrevAuditTrail.Text = "Prev";
            this.btnPrevAuditTrail.UseVisualStyleBackColor = false;
            this.btnPrevAuditTrail.Click += new System.EventHandler(this.btnPrevAuditTrail_Click);
            // 
            // lblAuditTrail
            // 
            this.lblAuditTrail.Location = new System.Drawing.Point(15, 5);
            this.lblAuditTrail.Name = "lblAuditTrail";
            this.lblAuditTrail.Size = new System.Drawing.Size(84, 30);
            this.lblAuditTrail.TabIndex = 121;
            this.lblAuditTrail.Text = "Booking Audit:";
            this.lblAuditTrail.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dgvAuditTrail
            // 
            this.dgvAuditTrail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAuditTrail.Location = new System.Drawing.Point(18, 38);
            this.dgvAuditTrail.Name = "dgvAuditTrail";
            this.dgvAuditTrail.ReadOnly = true;
            this.dgvAuditTrail.RowTemplate.Height = 30;
            this.dgvAuditTrail.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvAuditTrail.Size = new System.Drawing.Size(1175, 428);
            this.dgvAuditTrail.TabIndex = 61;
            // 
            // auditRailHScrollBar
            // 
            this.auditRailHScrollBar.AutoHide = false;
            this.auditRailHScrollBar.DataGridView = this.dgvAuditTrail;
            this.auditRailHScrollBar.LeftButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("auditRailHScrollBar.LeftButtonBackgroundImage")));
            this.auditRailHScrollBar.LeftButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("auditRailHScrollBar.LeftButtonDisabledBackgroundImage")));
            this.auditRailHScrollBar.Location = new System.Drawing.Point(18, 469);
            this.auditRailHScrollBar.Margin = new System.Windows.Forms.Padding(0);
            this.auditRailHScrollBar.Name = "auditRailHScrollBar";
            this.auditRailHScrollBar.RightButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("auditRailHScrollBar.RightButtonBackgroundImage")));
            this.auditRailHScrollBar.RightButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("auditRailHScrollBar.RightButtonDisabledBackgroundImage")));
            this.auditRailHScrollBar.ScrollableControl = null;
            this.auditRailHScrollBar.ScrollViewer = null;
            this.auditRailHScrollBar.Size = new System.Drawing.Size(1175, 40);
            this.auditRailHScrollBar.TabIndex = 126;
            this.auditRailHScrollBar.LeftButtonClick += new System.EventHandler(this.ScrollBar_Click);
            this.auditRailHScrollBar.RightButtonClick += new System.EventHandler(this.ScrollBar_Click);
            // 
            // auditTrailVScrollBar
            // 
            this.auditTrailVScrollBar.AutoHide = false;
            this.auditTrailVScrollBar.DataGridView = this.dgvAuditTrail;
            this.auditTrailVScrollBar.DownButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("auditTrailVScrollBar.DownButtonBackgroundImage")));
            this.auditTrailVScrollBar.DownButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("auditTrailVScrollBar.DownButtonDisabledBackgroundImage")));
            this.auditTrailVScrollBar.Location = new System.Drawing.Point(1195, 37);
            this.auditTrailVScrollBar.Margin = new System.Windows.Forms.Padding(0);
            this.auditTrailVScrollBar.Name = "auditTrailVScrollBar";
            this.auditTrailVScrollBar.ScrollableControl = null;
            this.auditTrailVScrollBar.ScrollViewer = null;
            this.auditTrailVScrollBar.Size = new System.Drawing.Size(40, 428);
            this.auditTrailVScrollBar.TabIndex = 60;
            this.auditTrailVScrollBar.UpButtonBackgroundImage = ((System.Drawing.Image)(resources.GetObject("auditTrailVScrollBar.UpButtonBackgroundImage")));
            this.auditTrailVScrollBar.UpButtonDisabledBackgroundImage = ((System.Drawing.Image)(resources.GetObject("auditTrailVScrollBar.UpButtonDisabledBackgroundImage")));
            this.auditTrailVScrollBar.UpButtonClick += new System.EventHandler(this.ScrollBar_Click);
            this.auditTrailVScrollBar.DownButtonClick += new System.EventHandler(this.ScrollBar_Click);
            // 
            // txtMessage
            // 
            this.txtMessage.BackColor = System.Drawing.Color.MistyRose;
            this.txtMessage.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtMessage.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMessage.Location = new System.Drawing.Point(0, 670);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.ReadOnly = true;
            this.txtMessage.Size = new System.Drawing.Size(1310, 22);
            this.txtMessage.TabIndex = 21;
            // 
            // userJobItemsDTOBindingSource
            // 
            this.userJobItemsDTOBindingSource.DataSource = typeof(Semnox.Parafait.Maintenance.UserJobItemsDTO);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn1.DataPropertyName = "TrxReservationScheduleId";
            this.dataGridViewTextBoxColumn1.HeaderText = "Trx Reservation Schedule Id";
            this.dataGridViewTextBoxColumn1.MinimumWidth = 130;
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Visible = false;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.DataPropertyName = "TrxId";
            this.dataGridViewTextBoxColumn2.HeaderText = "Trx Id";
            this.dataGridViewTextBoxColumn2.MinimumWidth = 150;
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Visible = false;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "LineId";
            this.dataGridViewTextBoxColumn3.HeaderText = "Line Id";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.Visible = false;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.DataPropertyName = "GuestQuantity";
            this.dataGridViewTextBoxColumn4.HeaderText = "Guest Quantity";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.Visible = false;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.DataPropertyName = "SchedulesId";
            this.dataGridViewTextBoxColumn5.HeaderText = "Schedules Id";
            this.dataGridViewTextBoxColumn5.MinimumWidth = 150;
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.Visible = false;
            this.dataGridViewTextBoxColumn5.Width = 150;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.DataPropertyName = "ScheduleFromDate";
            this.dataGridViewTextBoxColumn6.HeaderText = "Schedule From Date";
            this.dataGridViewTextBoxColumn6.MinimumWidth = 130;
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            this.dataGridViewTextBoxColumn6.Visible = false;
            this.dataGridViewTextBoxColumn6.Width = 130;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.DataPropertyName = "ScheduleToDate";
            this.dataGridViewTextBoxColumn7.HeaderText = "Schedule To Date";
            this.dataGridViewTextBoxColumn7.MinimumWidth = 100;
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.ReadOnly = true;
            this.dataGridViewTextBoxColumn7.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn7.Visible = false;
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.DataPropertyName = "FacilityId";
            this.dataGridViewTextBoxColumn8.HeaderText = "Facility Id";
            this.dataGridViewTextBoxColumn8.MinimumWidth = 100;
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.ReadOnly = true;
            this.dataGridViewTextBoxColumn8.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn8.Visible = false;
            // 
            // dataGridViewTextBoxColumn9
            // 
            this.dataGridViewTextBoxColumn9.DataPropertyName = "CancelledBy";
            this.dataGridViewTextBoxColumn9.HeaderText = "cancelled By";
            this.dataGridViewTextBoxColumn9.MinimumWidth = 100;
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            this.dataGridViewTextBoxColumn9.Visible = false;
            this.dataGridViewTextBoxColumn9.Width = 150;
            // 
            // dataGridViewTextBoxColumn10
            // 
            this.dataGridViewTextBoxColumn10.DataPropertyName = "Guid";
            this.dataGridViewTextBoxColumn10.HeaderText = "Guid";
            this.dataGridViewTextBoxColumn10.MinimumWidth = 150;
            this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            this.dataGridViewTextBoxColumn10.ReadOnly = true;
            this.dataGridViewTextBoxColumn10.Visible = false;
            this.dataGridViewTextBoxColumn10.Width = 150;
            // 
            // dataGridViewTextBoxColumn11
            // 
            this.dataGridViewTextBoxColumn11.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn11.DataPropertyName = "SiteId";
            this.dataGridViewTextBoxColumn11.HeaderText = "SiteId";
            this.dataGridViewTextBoxColumn11.MinimumWidth = 150;
            this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
            this.dataGridViewTextBoxColumn11.ReadOnly = true;
            this.dataGridViewTextBoxColumn11.Visible = false;
            // 
            // dataGridViewTextBoxColumn12
            // 
            this.dataGridViewTextBoxColumn12.DataPropertyName = "MasterEntityId";
            this.dataGridViewTextBoxColumn12.HeaderText = "MasterEntityId";
            this.dataGridViewTextBoxColumn12.MinimumWidth = 100;
            this.dataGridViewTextBoxColumn12.Name = "dataGridViewTextBoxColumn12";
            this.dataGridViewTextBoxColumn12.Visible = false;
            // 
            // dataGridViewTextBoxColumn13
            // 
            this.dataGridViewTextBoxColumn13.DataPropertyName = "CreatedBy";
            this.dataGridViewTextBoxColumn13.HeaderText = "Created By";
            this.dataGridViewTextBoxColumn13.MinimumWidth = 100;
            this.dataGridViewTextBoxColumn13.Name = "dataGridViewTextBoxColumn13";
            this.dataGridViewTextBoxColumn13.Visible = false;
            // 
            // dataGridViewTextBoxColumn14
            // 
            this.dataGridViewTextBoxColumn14.DataPropertyName = "CreationDate";
            this.dataGridViewTextBoxColumn14.HeaderText = "Created Date";
            this.dataGridViewTextBoxColumn14.MinimumWidth = 50;
            this.dataGridViewTextBoxColumn14.Name = "dataGridViewTextBoxColumn14";
            this.dataGridViewTextBoxColumn14.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn14.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn14.Visible = false;
            this.dataGridViewTextBoxColumn14.Width = 50;
            // 
            // dataGridViewTextBoxColumn15
            // 
            this.dataGridViewTextBoxColumn15.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn15.DataPropertyName = "LastUpdateDate";
            this.dataGridViewTextBoxColumn15.HeaderText = "Last Update Date";
            this.dataGridViewTextBoxColumn15.MinimumWidth = 190;
            this.dataGridViewTextBoxColumn15.Name = "dataGridViewTextBoxColumn15";
            this.dataGridViewTextBoxColumn15.ReadOnly = true;
            this.dataGridViewTextBoxColumn15.Visible = false;
            // 
            // dataGridViewTextBoxColumn16
            // 
            this.dataGridViewTextBoxColumn16.DataPropertyName = "ProductId";
            this.dataGridViewTextBoxColumn16.HeaderText = "ProductId";
            this.dataGridViewTextBoxColumn16.MinimumWidth = 150;
            this.dataGridViewTextBoxColumn16.Name = "dataGridViewTextBoxColumn16";
            this.dataGridViewTextBoxColumn16.Visible = false;
            this.dataGridViewTextBoxColumn16.Width = 150;
            // 
            // dataGridViewTextBoxColumn17
            // 
            this.dataGridViewTextBoxColumn17.DataPropertyName = "ProductName";
            this.dataGridViewTextBoxColumn17.HeaderText = "Product Name";
            this.dataGridViewTextBoxColumn17.MinimumWidth = 150;
            this.dataGridViewTextBoxColumn17.Name = "dataGridViewTextBoxColumn17";
            this.dataGridViewTextBoxColumn17.ReadOnly = true;
            this.dataGridViewTextBoxColumn17.Visible = false;
            this.dataGridViewTextBoxColumn17.Width = 150;
            // 
            // dataGridViewTextBoxColumn18
            // 
            this.dataGridViewTextBoxColumn18.DataPropertyName = "Description";
            this.dataGridViewTextBoxColumn18.HeaderText = "Description";
            this.dataGridViewTextBoxColumn18.Name = "dataGridViewTextBoxColumn18";
            this.dataGridViewTextBoxColumn18.Visible = false;
            // 
            // dataGridViewTextBoxColumn19
            // 
            this.dataGridViewTextBoxColumn19.DataPropertyName = "ActiveFlag";
            this.dataGridViewTextBoxColumn19.HeaderText = "ActiveFlag";
            this.dataGridViewTextBoxColumn19.Name = "dataGridViewTextBoxColumn19";
            this.dataGridViewTextBoxColumn19.Visible = false;
            // 
            // dataGridViewTextBoxColumn20
            // 
            this.dataGridViewTextBoxColumn20.DataPropertyName = "ProductTypeId";
            this.dataGridViewTextBoxColumn20.HeaderText = "ProductTypeId";
            this.dataGridViewTextBoxColumn20.Name = "dataGridViewTextBoxColumn20";
            this.dataGridViewTextBoxColumn20.Visible = false;
            // 
            // dataGridViewTextBoxColumn21
            // 
            this.dataGridViewTextBoxColumn21.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn21.DataPropertyName = "Price";
            this.dataGridViewTextBoxColumn21.HeaderText = "Price";
            this.dataGridViewTextBoxColumn21.MinimumWidth = 150;
            this.dataGridViewTextBoxColumn21.Name = "dataGridViewTextBoxColumn21";
            this.dataGridViewTextBoxColumn21.ReadOnly = true;
            this.dataGridViewTextBoxColumn21.Visible = false;
            // 
            // dataGridViewTextBoxColumn22
            // 
            this.dataGridViewTextBoxColumn22.DataPropertyName = "Credits";
            this.dataGridViewTextBoxColumn22.HeaderText = "Credits";
            this.dataGridViewTextBoxColumn22.Name = "dataGridViewTextBoxColumn22";
            this.dataGridViewTextBoxColumn22.ReadOnly = true;
            this.dataGridViewTextBoxColumn22.Visible = false;
            // 
            // dataGridViewTextBoxColumn23
            // 
            this.dataGridViewTextBoxColumn23.DataPropertyName = "Courtesy";
            this.dataGridViewTextBoxColumn23.HeaderText = "Courtesy";
            this.dataGridViewTextBoxColumn23.MinimumWidth = 150;
            this.dataGridViewTextBoxColumn23.Name = "dataGridViewTextBoxColumn23";
            this.dataGridViewTextBoxColumn23.Visible = false;
            this.dataGridViewTextBoxColumn23.Width = 150;
            // 
            // dataGridViewTextBoxColumn24
            // 
            this.dataGridViewTextBoxColumn24.DataPropertyName = "Bonus";
            this.dataGridViewTextBoxColumn24.HeaderText = "Bonus";
            this.dataGridViewTextBoxColumn24.Name = "dataGridViewTextBoxColumn24";
            this.dataGridViewTextBoxColumn24.Visible = false;
            // 
            // dataGridViewTextBoxColumn25
            // 
            this.dataGridViewTextBoxColumn25.DataPropertyName = "Time";
            this.dataGridViewTextBoxColumn25.HeaderText = "Time";
            this.dataGridViewTextBoxColumn25.Name = "dataGridViewTextBoxColumn25";
            this.dataGridViewTextBoxColumn25.Visible = false;
            // 
            // dataGridViewTextBoxColumn26
            // 
            this.dataGridViewTextBoxColumn26.DataPropertyName = "SortOrder";
            this.dataGridViewTextBoxColumn26.HeaderText = "SortOrder";
            this.dataGridViewTextBoxColumn26.MinimumWidth = 100;
            this.dataGridViewTextBoxColumn26.Name = "dataGridViewTextBoxColumn26";
            this.dataGridViewTextBoxColumn26.Visible = false;
            // 
            // dataGridViewTextBoxColumn27
            // 
            this.dataGridViewTextBoxColumn27.DataPropertyName = "Tax_id";
            this.dataGridViewTextBoxColumn27.HeaderText = "Tax_id";
            this.dataGridViewTextBoxColumn27.MinimumWidth = 100;
            this.dataGridViewTextBoxColumn27.Name = "dataGridViewTextBoxColumn27";
            this.dataGridViewTextBoxColumn27.Visible = false;
            // 
            // dataGridViewTextBoxColumn28
            // 
            this.dataGridViewTextBoxColumn28.DataPropertyName = "Tickets";
            this.dataGridViewTextBoxColumn28.HeaderText = "Tickets";
            this.dataGridViewTextBoxColumn28.MinimumWidth = 50;
            this.dataGridViewTextBoxColumn28.Name = "dataGridViewTextBoxColumn28";
            this.dataGridViewTextBoxColumn28.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn28.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn28.Visible = false;
            this.dataGridViewTextBoxColumn28.Width = 50;
            // 
            // dataGridViewTextBoxColumn29
            // 
            this.dataGridViewTextBoxColumn29.DataPropertyName = "FaceValue";
            this.dataGridViewTextBoxColumn29.HeaderText = "FaceValue";
            this.dataGridViewTextBoxColumn29.Name = "dataGridViewTextBoxColumn29";
            this.dataGridViewTextBoxColumn29.Visible = false;
            // 
            // dataGridViewTextBoxColumn30
            // 
            this.dataGridViewTextBoxColumn30.DataPropertyName = "DisplayGroup";
            this.dataGridViewTextBoxColumn30.HeaderText = "DisplayGroup";
            this.dataGridViewTextBoxColumn30.MinimumWidth = 150;
            this.dataGridViewTextBoxColumn30.Name = "dataGridViewTextBoxColumn30";
            this.dataGridViewTextBoxColumn30.Visible = false;
            this.dataGridViewTextBoxColumn30.Width = 150;
            // 
            // dataGridViewTextBoxColumn31
            // 
            this.dataGridViewTextBoxColumn31.DataPropertyName = "TicketAllowed";
            this.dataGridViewTextBoxColumn31.HeaderText = "TicketAllowed";
            this.dataGridViewTextBoxColumn31.MinimumWidth = 150;
            this.dataGridViewTextBoxColumn31.Name = "dataGridViewTextBoxColumn31";
            this.dataGridViewTextBoxColumn31.Visible = false;
            this.dataGridViewTextBoxColumn31.Width = 150;
            // 
            // dataGridViewTextBoxColumn32
            // 
            this.dataGridViewTextBoxColumn32.DataPropertyName = "VipCard";
            this.dataGridViewTextBoxColumn32.HeaderText = "VipCard";
            this.dataGridViewTextBoxColumn32.Name = "dataGridViewTextBoxColumn32";
            this.dataGridViewTextBoxColumn32.Visible = false;
            // 
            // dataGridViewTextBoxColumn33
            // 
            this.dataGridViewTextBoxColumn33.DataPropertyName = "LastUpdatedDate";
            this.dataGridViewTextBoxColumn33.HeaderText = "LastUpdatedDate";
            this.dataGridViewTextBoxColumn33.Name = "dataGridViewTextBoxColumn33";
            this.dataGridViewTextBoxColumn33.Visible = false;
            // 
            // dataGridViewTextBoxColumn34
            // 
            this.dataGridViewTextBoxColumn34.DataPropertyName = "LastUpdatedUser";
            this.dataGridViewTextBoxColumn34.HeaderText = "LastUpdatedUser";
            this.dataGridViewTextBoxColumn34.MinimumWidth = 200;
            this.dataGridViewTextBoxColumn34.Name = "dataGridViewTextBoxColumn34";
            this.dataGridViewTextBoxColumn34.ReadOnly = true;
            this.dataGridViewTextBoxColumn34.Visible = false;
            this.dataGridViewTextBoxColumn34.Width = 200;
            // 
            // dataGridViewTextBoxColumn35
            // 
            this.dataGridViewTextBoxColumn35.DataPropertyName = "InternetKey";
            this.dataGridViewTextBoxColumn35.HeaderText = "InternetKey";
            this.dataGridViewTextBoxColumn35.MinimumWidth = 100;
            this.dataGridViewTextBoxColumn35.Name = "dataGridViewTextBoxColumn35";
            this.dataGridViewTextBoxColumn35.ReadOnly = true;
            this.dataGridViewTextBoxColumn35.Visible = false;
            this.dataGridViewTextBoxColumn35.Width = 120;
            // 
            // dataGridViewTextBoxColumn36
            // 
            this.dataGridViewTextBoxColumn36.DataPropertyName = "TaxInclusivePrice";
            this.dataGridViewTextBoxColumn36.HeaderText = "TaxInclusivePrice";
            this.dataGridViewTextBoxColumn36.Name = "dataGridViewTextBoxColumn36";
            this.dataGridViewTextBoxColumn36.Visible = false;
            // 
            // dataGridViewTextBoxColumn37
            // 
            this.dataGridViewTextBoxColumn37.DataPropertyName = "InventoryProductCode";
            this.dataGridViewTextBoxColumn37.HeaderText = "InventoryProductCode";
            this.dataGridViewTextBoxColumn37.Name = "dataGridViewTextBoxColumn37";
            this.dataGridViewTextBoxColumn37.Visible = false;
            // 
            // dataGridViewTextBoxColumn38
            // 
            this.dataGridViewTextBoxColumn38.DataPropertyName = "ExpiryDate";
            this.dataGridViewTextBoxColumn38.HeaderText = "ExpiryDate";
            this.dataGridViewTextBoxColumn38.Name = "dataGridViewTextBoxColumn38";
            this.dataGridViewTextBoxColumn38.Visible = false;
            // 
            // dataGridViewTextBoxColumn39
            // 
            this.dataGridViewTextBoxColumn39.DataPropertyName = "AutoCheckOut";
            this.dataGridViewTextBoxColumn39.HeaderText = "AutoCheckOut";
            this.dataGridViewTextBoxColumn39.Name = "dataGridViewTextBoxColumn39";
            this.dataGridViewTextBoxColumn39.Visible = false;
            // 
            // dataGridViewTextBoxColumn40
            // 
            this.dataGridViewTextBoxColumn40.DataPropertyName = "CheckInFacilityId";
            this.dataGridViewTextBoxColumn40.HeaderText = "CheckInFacilityId";
            this.dataGridViewTextBoxColumn40.Name = "dataGridViewTextBoxColumn40";
            this.dataGridViewTextBoxColumn40.ReadOnly = true;
            this.dataGridViewTextBoxColumn40.Visible = false;
            // 
            // dataGridViewTextBoxColumn41
            // 
            this.dataGridViewTextBoxColumn41.DataPropertyName = "MaxCheckOutAmount";
            this.dataGridViewTextBoxColumn41.HeaderText = "MaxCheckOutAmount";
            this.dataGridViewTextBoxColumn41.MinimumWidth = 150;
            this.dataGridViewTextBoxColumn41.Name = "dataGridViewTextBoxColumn41";
            this.dataGridViewTextBoxColumn41.Visible = false;
            this.dataGridViewTextBoxColumn41.Width = 150;
            // 
            // dataGridViewTextBoxColumn42
            // 
            this.dataGridViewTextBoxColumn42.DataPropertyName = "POSTypeId";
            this.dataGridViewTextBoxColumn42.HeaderText = "POSTypeId";
            this.dataGridViewTextBoxColumn42.Name = "dataGridViewTextBoxColumn42";
            this.dataGridViewTextBoxColumn42.Visible = false;
            // 
            // dataGridViewTextBoxColumn43
            // 
            this.dataGridViewTextBoxColumn43.DataPropertyName = "CustomDataSetId";
            this.dataGridViewTextBoxColumn43.HeaderText = "CustomDataSetId";
            this.dataGridViewTextBoxColumn43.MinimumWidth = 120;
            this.dataGridViewTextBoxColumn43.Name = "dataGridViewTextBoxColumn43";
            this.dataGridViewTextBoxColumn43.Visible = false;
            this.dataGridViewTextBoxColumn43.Width = 120;
            // 
            // dataGridViewTextBoxColumn44
            // 
            this.dataGridViewTextBoxColumn44.DataPropertyName = "Guid";
            this.dataGridViewTextBoxColumn44.HeaderText = "Guid";
            this.dataGridViewTextBoxColumn44.Name = "dataGridViewTextBoxColumn44";
            this.dataGridViewTextBoxColumn44.Visible = false;
            // 
            // dataGridViewTextBoxColumn45
            // 
            this.dataGridViewTextBoxColumn45.DataPropertyName = "CardTypeId";
            this.dataGridViewTextBoxColumn45.HeaderText = "CardTypeId";
            this.dataGridViewTextBoxColumn45.Name = "dataGridViewTextBoxColumn45";
            this.dataGridViewTextBoxColumn45.Visible = false;
            // 
            // dataGridViewTextBoxColumn46
            // 
            this.dataGridViewTextBoxColumn46.DataPropertyName = "SiteId";
            this.dataGridViewTextBoxColumn46.HeaderText = "SiteId";
            this.dataGridViewTextBoxColumn46.Name = "dataGridViewTextBoxColumn46";
            this.dataGridViewTextBoxColumn46.Visible = false;
            // 
            // dataGridViewTextBoxColumn47
            // 
            this.dataGridViewTextBoxColumn47.DataPropertyName = "TrxRemarksMandatory";
            this.dataGridViewTextBoxColumn47.HeaderText = "TrxRemarksMandatory";
            this.dataGridViewTextBoxColumn47.Name = "dataGridViewTextBoxColumn47";
            this.dataGridViewTextBoxColumn47.Visible = false;
            // 
            // dataGridViewTextBoxColumn48
            // 
            this.dataGridViewTextBoxColumn48.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn48.DataPropertyName = "CategoryId";
            this.dataGridViewTextBoxColumn48.HeaderText = "CategoryId";
            this.dataGridViewTextBoxColumn48.MinimumWidth = 200;
            this.dataGridViewTextBoxColumn48.Name = "dataGridViewTextBoxColumn48";
            this.dataGridViewTextBoxColumn48.ReadOnly = true;
            this.dataGridViewTextBoxColumn48.Visible = false;
            // 
            // dataGridViewTextBoxColumn49
            // 
            this.dataGridViewTextBoxColumn49.DataPropertyName = "OverridePrintTemplateId";
            this.dataGridViewTextBoxColumn49.HeaderText = "OverridePrintTemplateId";
            this.dataGridViewTextBoxColumn49.MinimumWidth = 120;
            this.dataGridViewTextBoxColumn49.Name = "dataGridViewTextBoxColumn49";
            this.dataGridViewTextBoxColumn49.ReadOnly = true;
            this.dataGridViewTextBoxColumn49.Visible = false;
            this.dataGridViewTextBoxColumn49.Width = 120;
            // 
            // dataGridViewTextBoxColumn50
            // 
            this.dataGridViewTextBoxColumn50.DataPropertyName = "StartDate";
            this.dataGridViewTextBoxColumn50.HeaderText = "StartDate";
            this.dataGridViewTextBoxColumn50.Name = "dataGridViewTextBoxColumn50";
            this.dataGridViewTextBoxColumn50.Visible = false;
            // 
            // dataGridViewTextBoxColumn51
            // 
            this.dataGridViewTextBoxColumn51.DataPropertyName = "ButtonColor";
            this.dataGridViewTextBoxColumn51.HeaderText = "ButtonColor";
            this.dataGridViewTextBoxColumn51.Name = "dataGridViewTextBoxColumn51";
            this.dataGridViewTextBoxColumn51.Visible = false;
            // 
            // dataGridViewTextBoxColumn52
            // 
            this.dataGridViewTextBoxColumn52.DataPropertyName = "AutoGenerateCardNumber";
            this.dataGridViewTextBoxColumn52.HeaderText = "AutoGenerateCardNumber";
            this.dataGridViewTextBoxColumn52.Name = "dataGridViewTextBoxColumn52";
            this.dataGridViewTextBoxColumn52.Visible = false;
            // 
            // dataGridViewTextBoxColumn53
            // 
            this.dataGridViewTextBoxColumn53.DataPropertyName = "QuantityPrompt";
            this.dataGridViewTextBoxColumn53.HeaderText = "QuantityPrompt";
            this.dataGridViewTextBoxColumn53.Name = "dataGridViewTextBoxColumn53";
            this.dataGridViewTextBoxColumn53.Visible = false;
            // 
            // dataGridViewTextBoxColumn54
            // 
            this.dataGridViewTextBoxColumn54.DataPropertyName = "OnlyForVIP";
            this.dataGridViewTextBoxColumn54.HeaderText = "OnlyForVIP";
            this.dataGridViewTextBoxColumn54.Name = "dataGridViewTextBoxColumn54";
            this.dataGridViewTextBoxColumn54.ReadOnly = true;
            this.dataGridViewTextBoxColumn54.Visible = false;
            // 
            // dataGridViewTextBoxColumn55
            // 
            this.dataGridViewTextBoxColumn55.DataPropertyName = "AllowPriceOverride";
            this.dataGridViewTextBoxColumn55.HeaderText = "AllowPriceOverride";
            this.dataGridViewTextBoxColumn55.MinimumWidth = 150;
            this.dataGridViewTextBoxColumn55.Name = "dataGridViewTextBoxColumn55";
            this.dataGridViewTextBoxColumn55.Visible = false;
            this.dataGridViewTextBoxColumn55.Width = 150;
            // 
            // dataGridViewTextBoxColumn56
            // 
            this.dataGridViewTextBoxColumn56.DataPropertyName = "RegisteredCustomerOnly";
            this.dataGridViewTextBoxColumn56.HeaderText = "RegisteredCustomerOnly";
            this.dataGridViewTextBoxColumn56.Name = "dataGridViewTextBoxColumn56";
            this.dataGridViewTextBoxColumn56.Visible = false;
            // 
            // dataGridViewTextBoxColumn57
            // 
            this.dataGridViewTextBoxColumn57.DataPropertyName = "ManagerApprovalRequired";
            this.dataGridViewTextBoxColumn57.HeaderText = "ManagerApprovalRequired";
            this.dataGridViewTextBoxColumn57.MinimumWidth = 120;
            this.dataGridViewTextBoxColumn57.Name = "dataGridViewTextBoxColumn57";
            this.dataGridViewTextBoxColumn57.Visible = false;
            this.dataGridViewTextBoxColumn57.Width = 120;
            // 
            // dataGridViewTextBoxColumn58
            // 
            this.dataGridViewTextBoxColumn58.DataPropertyName = "MinimumUserPrice";
            this.dataGridViewTextBoxColumn58.HeaderText = "MinimumUserPrice";
            this.dataGridViewTextBoxColumn58.Name = "dataGridViewTextBoxColumn58";
            this.dataGridViewTextBoxColumn58.Visible = false;
            // 
            // dataGridViewTextBoxColumn59
            // 
            this.dataGridViewTextBoxColumn59.DataPropertyName = "TextColor";
            this.dataGridViewTextBoxColumn59.HeaderText = "TextColor";
            this.dataGridViewTextBoxColumn59.Name = "dataGridViewTextBoxColumn59";
            this.dataGridViewTextBoxColumn59.Visible = false;
            // 
            // dataGridViewTextBoxColumn60
            // 
            this.dataGridViewTextBoxColumn60.DataPropertyName = "Font";
            this.dataGridViewTextBoxColumn60.HeaderText = "Font";
            this.dataGridViewTextBoxColumn60.Name = "dataGridViewTextBoxColumn60";
            this.dataGridViewTextBoxColumn60.Visible = false;
            // 
            // dataGridViewTextBoxColumn61
            // 
            this.dataGridViewTextBoxColumn61.DataPropertyName = "VerifiedCustomerOnly";
            this.dataGridViewTextBoxColumn61.HeaderText = "VerifiedCustomerOnly";
            this.dataGridViewTextBoxColumn61.Name = "dataGridViewTextBoxColumn61";
            this.dataGridViewTextBoxColumn61.Visible = false;
            // 
            // dataGridViewTextBoxColumn62
            // 
            this.dataGridViewTextBoxColumn62.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn62.DataPropertyName = "Modifier";
            this.dataGridViewTextBoxColumn62.HeaderText = "Modifier";
            this.dataGridViewTextBoxColumn62.MinimumWidth = 200;
            this.dataGridViewTextBoxColumn62.Name = "dataGridViewTextBoxColumn62";
            this.dataGridViewTextBoxColumn62.Visible = false;
            // 
            // dataGridViewTextBoxColumn63
            // 
            this.dataGridViewTextBoxColumn63.DataPropertyName = "MinimumQuantity";
            this.dataGridViewTextBoxColumn63.HeaderText = "MinimumQuantity";
            this.dataGridViewTextBoxColumn63.MinimumWidth = 120;
            this.dataGridViewTextBoxColumn63.Name = "dataGridViewTextBoxColumn63";
            this.dataGridViewTextBoxColumn63.Visible = false;
            this.dataGridViewTextBoxColumn63.Width = 120;
            // 
            // dataGridViewTextBoxColumn64
            // 
            this.dataGridViewTextBoxColumn64.DataPropertyName = "DisplayInPOS";
            this.dataGridViewTextBoxColumn64.HeaderText = "DisplayInPOS";
            this.dataGridViewTextBoxColumn64.Name = "dataGridViewTextBoxColumn64";
            this.dataGridViewTextBoxColumn64.Visible = false;
            // 
            // dataGridViewTextBoxColumn65
            // 
            this.dataGridViewTextBoxColumn65.DataPropertyName = "CardCount";
            this.dataGridViewTextBoxColumn65.HeaderText = "CardCount";
            this.dataGridViewTextBoxColumn65.Name = "dataGridViewTextBoxColumn65";
            this.dataGridViewTextBoxColumn65.Visible = false;
            // 
            // dataGridViewTextBoxColumn66
            // 
            this.dataGridViewTextBoxColumn66.DataPropertyName = "ImageFileName";
            this.dataGridViewTextBoxColumn66.HeaderText = "ImageFileName";
            this.dataGridViewTextBoxColumn66.Name = "dataGridViewTextBoxColumn66";
            this.dataGridViewTextBoxColumn66.Visible = false;
            // 
            // dataGridViewTextBoxColumn67
            // 
            this.dataGridViewTextBoxColumn67.DataPropertyName = "AdvancePercentage";
            this.dataGridViewTextBoxColumn67.HeaderText = "AdvancePercentage";
            this.dataGridViewTextBoxColumn67.Name = "dataGridViewTextBoxColumn67";
            this.dataGridViewTextBoxColumn67.Visible = false;
            // 
            // dataGridViewTextBoxColumn68
            // 
            this.dataGridViewTextBoxColumn68.DataPropertyName = "AdvanceAmount";
            this.dataGridViewTextBoxColumn68.HeaderText = "AdvanceAmount";
            this.dataGridViewTextBoxColumn68.Name = "dataGridViewTextBoxColumn68";
            this.dataGridViewTextBoxColumn68.Visible = false;
            // 
            // dataGridViewTextBoxColumn69
            // 
            this.dataGridViewTextBoxColumn69.DataPropertyName = "EmailTemplateId";
            this.dataGridViewTextBoxColumn69.HeaderText = "EmailTemplateId";
            this.dataGridViewTextBoxColumn69.Name = "dataGridViewTextBoxColumn69";
            this.dataGridViewTextBoxColumn69.Visible = false;
            // 
            // dataGridViewTextBoxColumn70
            // 
            this.dataGridViewTextBoxColumn70.DataPropertyName = "MaximumTime";
            this.dataGridViewTextBoxColumn70.HeaderText = "MaximumTime";
            this.dataGridViewTextBoxColumn70.Name = "dataGridViewTextBoxColumn70";
            this.dataGridViewTextBoxColumn70.Visible = false;
            // 
            // dataGridViewTextBoxColumn71
            // 
            this.dataGridViewTextBoxColumn71.DataPropertyName = "MinimumTime";
            this.dataGridViewTextBoxColumn71.HeaderText = "MinimumTime";
            this.dataGridViewTextBoxColumn71.Name = "dataGridViewTextBoxColumn71";
            this.dataGridViewTextBoxColumn71.Visible = false;
            // 
            // dataGridViewTextBoxColumn72
            // 
            this.dataGridViewTextBoxColumn72.DataPropertyName = "CardValidFor";
            this.dataGridViewTextBoxColumn72.HeaderText = "CardValidFor";
            this.dataGridViewTextBoxColumn72.Name = "dataGridViewTextBoxColumn72";
            this.dataGridViewTextBoxColumn72.Visible = false;
            // 
            // dataGridViewTextBoxColumn73
            // 
            this.dataGridViewTextBoxColumn73.DataPropertyName = "AdditionalTaxInclusive";
            this.dataGridViewTextBoxColumn73.HeaderText = "AdditionalTaxInclusive";
            this.dataGridViewTextBoxColumn73.Name = "dataGridViewTextBoxColumn73";
            this.dataGridViewTextBoxColumn73.Visible = false;
            // 
            // dataGridViewTextBoxColumn74
            // 
            this.dataGridViewTextBoxColumn74.DataPropertyName = "AdditionalPrice";
            this.dataGridViewTextBoxColumn74.HeaderText = "AdditionalPrice";
            this.dataGridViewTextBoxColumn74.Name = "dataGridViewTextBoxColumn74";
            this.dataGridViewTextBoxColumn74.Visible = false;
            // 
            // dataGridViewTextBoxColumn75
            // 
            this.dataGridViewTextBoxColumn75.DataPropertyName = "AdditionalTaxId";
            this.dataGridViewTextBoxColumn75.HeaderText = "AdditionalTaxId";
            this.dataGridViewTextBoxColumn75.Name = "dataGridViewTextBoxColumn75";
            this.dataGridViewTextBoxColumn75.Visible = false;
            // 
            // dataGridViewTextBoxColumn76
            // 
            this.dataGridViewTextBoxColumn76.DataPropertyName = "MasterEntityId";
            this.dataGridViewTextBoxColumn76.HeaderText = "MasterEntityId";
            this.dataGridViewTextBoxColumn76.Name = "dataGridViewTextBoxColumn76";
            this.dataGridViewTextBoxColumn76.Visible = false;
            // 
            // dataGridViewTextBoxColumn77
            // 
            this.dataGridViewTextBoxColumn77.DataPropertyName = "WaiverRequired";
            this.dataGridViewTextBoxColumn77.HeaderText = "WaiverRequired";
            this.dataGridViewTextBoxColumn77.Name = "dataGridViewTextBoxColumn77";
            this.dataGridViewTextBoxColumn77.Visible = false;
            // 
            // dataGridViewTextBoxColumn78
            // 
            this.dataGridViewTextBoxColumn78.DataPropertyName = "SegmentCategoryId";
            this.dataGridViewTextBoxColumn78.HeaderText = "SegmentCategoryId";
            this.dataGridViewTextBoxColumn78.Name = "dataGridViewTextBoxColumn78";
            this.dataGridViewTextBoxColumn78.Visible = false;
            // 
            // dataGridViewTextBoxColumn79
            // 
            this.dataGridViewTextBoxColumn79.DataPropertyName = "CardExpiryDate";
            this.dataGridViewTextBoxColumn79.HeaderText = "CardExpiryDate";
            this.dataGridViewTextBoxColumn79.Name = "dataGridViewTextBoxColumn79";
            this.dataGridViewTextBoxColumn79.Visible = false;
            // 
            // dataGridViewTextBoxColumn80
            // 
            this.dataGridViewTextBoxColumn80.DataPropertyName = "ProductDisplayGroupFormatId";
            this.dataGridViewTextBoxColumn80.HeaderText = "ProductDisplayGroupFormatId";
            this.dataGridViewTextBoxColumn80.Name = "dataGridViewTextBoxColumn80";
            this.dataGridViewTextBoxColumn80.Visible = false;
            // 
            // dataGridViewTextBoxColumn81
            // 
            this.dataGridViewTextBoxColumn81.DataPropertyName = "TranslatedProductName";
            this.dataGridViewTextBoxColumn81.HeaderText = "TranslatedProductName";
            this.dataGridViewTextBoxColumn81.Name = "dataGridViewTextBoxColumn81";
            this.dataGridViewTextBoxColumn81.Visible = false;
            // 
            // dataGridViewTextBoxColumn82
            // 
            this.dataGridViewTextBoxColumn82.DataPropertyName = "TranslatedProductDescription";
            this.dataGridViewTextBoxColumn82.HeaderText = "TranslatedProductDescription";
            this.dataGridViewTextBoxColumn82.Name = "dataGridViewTextBoxColumn82";
            this.dataGridViewTextBoxColumn82.Visible = false;
            // 
            // dataGridViewTextBoxColumn83
            // 
            this.dataGridViewTextBoxColumn83.DataPropertyName = "CategoryName";
            this.dataGridViewTextBoxColumn83.HeaderText = "CategoryName";
            this.dataGridViewTextBoxColumn83.Name = "dataGridViewTextBoxColumn83";
            this.dataGridViewTextBoxColumn83.Visible = false;
            // 
            // dataGridViewTextBoxColumn84
            // 
            this.dataGridViewTextBoxColumn84.DataPropertyName = "TaxPercentage";
            this.dataGridViewTextBoxColumn84.HeaderText = "TaxPercentage";
            this.dataGridViewTextBoxColumn84.Name = "dataGridViewTextBoxColumn84";
            this.dataGridViewTextBoxColumn84.Visible = false;
            // 
            // dataGridViewTextBoxColumn85
            // 
            this.dataGridViewTextBoxColumn85.DataPropertyName = "ZoneId";
            this.dataGridViewTextBoxColumn85.HeaderText = "ZoneId";
            this.dataGridViewTextBoxColumn85.Name = "dataGridViewTextBoxColumn85";
            this.dataGridViewTextBoxColumn85.Visible = false;
            // 
            // dataGridViewTextBoxColumn86
            // 
            this.dataGridViewTextBoxColumn86.DataPropertyName = "LockerExpiryInHours";
            this.dataGridViewTextBoxColumn86.HeaderText = "LockerExpiryInHours";
            this.dataGridViewTextBoxColumn86.Name = "dataGridViewTextBoxColumn86";
            this.dataGridViewTextBoxColumn86.Visible = false;
            // 
            // dataGridViewTextBoxColumn87
            // 
            this.dataGridViewTextBoxColumn87.DataPropertyName = "LockerExpiryDate";
            this.dataGridViewTextBoxColumn87.HeaderText = "LockerExpiryDate";
            this.dataGridViewTextBoxColumn87.Name = "dataGridViewTextBoxColumn87";
            this.dataGridViewTextBoxColumn87.Visible = false;
            // 
            // dataGridViewTextBoxColumn88
            // 
            this.dataGridViewTextBoxColumn88.DataPropertyName = "WaiverSetId";
            this.dataGridViewTextBoxColumn88.HeaderText = "WaiverSetId";
            this.dataGridViewTextBoxColumn88.MinimumWidth = 150;
            this.dataGridViewTextBoxColumn88.Name = "dataGridViewTextBoxColumn88";
            this.dataGridViewTextBoxColumn88.ReadOnly = true;
            this.dataGridViewTextBoxColumn88.Visible = false;
            this.dataGridViewTextBoxColumn88.Width = 150;
            // 
            // dataGridViewTextBoxColumn89
            // 
            this.dataGridViewTextBoxColumn89.DataPropertyName = "MaxQtyPerDay";
            this.dataGridViewTextBoxColumn89.HeaderText = "MaxQtyPerDay";
            this.dataGridViewTextBoxColumn89.MinimumWidth = 120;
            this.dataGridViewTextBoxColumn89.Name = "dataGridViewTextBoxColumn89";
            this.dataGridViewTextBoxColumn89.ReadOnly = true;
            this.dataGridViewTextBoxColumn89.Visible = false;
            this.dataGridViewTextBoxColumn89.Width = 120;
            // 
            // dataGridViewTextBoxColumn90
            // 
            this.dataGridViewTextBoxColumn90.DataPropertyName = "HsnSacCode";
            this.dataGridViewTextBoxColumn90.HeaderText = "HsnSacCode";
            this.dataGridViewTextBoxColumn90.MinimumWidth = 70;
            this.dataGridViewTextBoxColumn90.Name = "dataGridViewTextBoxColumn90";
            this.dataGridViewTextBoxColumn90.ReadOnly = true;
            this.dataGridViewTextBoxColumn90.Visible = false;
            this.dataGridViewTextBoxColumn90.Width = 70;
            // 
            // dataGridViewTextBoxColumn91
            // 
            this.dataGridViewTextBoxColumn91.DataPropertyName = "WebDescription";
            this.dataGridViewTextBoxColumn91.HeaderText = "WebDescription";
            this.dataGridViewTextBoxColumn91.MinimumWidth = 120;
            this.dataGridViewTextBoxColumn91.Name = "dataGridViewTextBoxColumn91";
            this.dataGridViewTextBoxColumn91.ReadOnly = true;
            this.dataGridViewTextBoxColumn91.Visible = false;
            this.dataGridViewTextBoxColumn91.Width = 120;
            // 
            // dataGridViewTextBoxColumn92
            // 
            this.dataGridViewTextBoxColumn92.DataPropertyName = "OrderTypeId";
            this.dataGridViewTextBoxColumn92.HeaderText = "OrderTypeId";
            this.dataGridViewTextBoxColumn92.MinimumWidth = 150;
            this.dataGridViewTextBoxColumn92.Name = "dataGridViewTextBoxColumn92";
            this.dataGridViewTextBoxColumn92.ReadOnly = true;
            this.dataGridViewTextBoxColumn92.Visible = false;
            this.dataGridViewTextBoxColumn92.Width = 150;
            // 
            // dataGridViewTextBoxColumn93
            // 
            this.dataGridViewTextBoxColumn93.DataPropertyName = "IsGroupMeal";
            this.dataGridViewTextBoxColumn93.HeaderText = "IsGroupMeal";
            this.dataGridViewTextBoxColumn93.MinimumWidth = 150;
            this.dataGridViewTextBoxColumn93.Name = "dataGridViewTextBoxColumn93";
            this.dataGridViewTextBoxColumn93.Visible = false;
            this.dataGridViewTextBoxColumn93.Width = 150;
            // 
            // dataGridViewTextBoxColumn94
            // 
            this.dataGridViewTextBoxColumn94.DataPropertyName = "MembershipId";
            this.dataGridViewTextBoxColumn94.HeaderText = "MembershipId";
            this.dataGridViewTextBoxColumn94.MinimumWidth = 150;
            this.dataGridViewTextBoxColumn94.Name = "dataGridViewTextBoxColumn94";
            this.dataGridViewTextBoxColumn94.Visible = false;
            this.dataGridViewTextBoxColumn94.Width = 150;
            // 
            // dataGridViewTextBoxColumn95
            // 
            this.dataGridViewTextBoxColumn95.DataPropertyName = "CardSale";
            this.dataGridViewTextBoxColumn95.HeaderText = "CardSale";
            this.dataGridViewTextBoxColumn95.Name = "dataGridViewTextBoxColumn95";
            this.dataGridViewTextBoxColumn95.Visible = false;
            // 
            // dataGridViewTextBoxColumn96
            // 
            this.dataGridViewTextBoxColumn96.DataPropertyName = "ZoneCode";
            this.dataGridViewTextBoxColumn96.HeaderText = "ZoneCode";
            this.dataGridViewTextBoxColumn96.MinimumWidth = 150;
            this.dataGridViewTextBoxColumn96.Name = "dataGridViewTextBoxColumn96";
            this.dataGridViewTextBoxColumn96.Visible = false;
            this.dataGridViewTextBoxColumn96.Width = 150;
            // 
            // dataGridViewTextBoxColumn97
            // 
            this.dataGridViewTextBoxColumn97.DataPropertyName = "LockerMode";
            this.dataGridViewTextBoxColumn97.HeaderText = "LockerMode";
            this.dataGridViewTextBoxColumn97.MinimumWidth = 150;
            this.dataGridViewTextBoxColumn97.Name = "dataGridViewTextBoxColumn97";
            this.dataGridViewTextBoxColumn97.Visible = false;
            this.dataGridViewTextBoxColumn97.Width = 150;
            // 
            // dataGridViewTextBoxColumn98
            // 
            this.dataGridViewTextBoxColumn98.DataPropertyName = "TaxName";
            this.dataGridViewTextBoxColumn98.HeaderText = "TaxName";
            this.dataGridViewTextBoxColumn98.MinimumWidth = 50;
            this.dataGridViewTextBoxColumn98.Name = "dataGridViewTextBoxColumn98";
            this.dataGridViewTextBoxColumn98.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn98.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn98.Visible = false;
            this.dataGridViewTextBoxColumn98.Width = 50;
            // 
            // dataGridViewTextBoxColumn99
            // 
            this.dataGridViewTextBoxColumn99.DataPropertyName = "UsedInDiscounts";
            this.dataGridViewTextBoxColumn99.HeaderText = "UsedInDiscounts";
            this.dataGridViewTextBoxColumn99.MinimumWidth = 150;
            this.dataGridViewTextBoxColumn99.Name = "dataGridViewTextBoxColumn99";
            this.dataGridViewTextBoxColumn99.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn99.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn99.Visible = false;
            this.dataGridViewTextBoxColumn99.Width = 150;
            // 
            // dataGridViewTextBoxColumn100
            // 
            this.dataGridViewTextBoxColumn100.DataPropertyName = "CreditPlusConsumptionId";
            this.dataGridViewTextBoxColumn100.HeaderText = "CreditPlusConsumptionId";
            this.dataGridViewTextBoxColumn100.MinimumWidth = 150;
            this.dataGridViewTextBoxColumn100.Name = "dataGridViewTextBoxColumn100";
            this.dataGridViewTextBoxColumn100.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn100.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn100.Visible = false;
            this.dataGridViewTextBoxColumn100.Width = 150;
            // 
            // dataGridViewTextBoxColumn101
            // 
            this.dataGridViewTextBoxColumn101.DataPropertyName = "ProductType";
            this.dataGridViewTextBoxColumn101.HeaderText = "Product Type";
            this.dataGridViewTextBoxColumn101.MinimumWidth = 120;
            this.dataGridViewTextBoxColumn101.Name = "dataGridViewTextBoxColumn101";
            this.dataGridViewTextBoxColumn101.ReadOnly = true;
            this.dataGridViewTextBoxColumn101.Visible = false;
            this.dataGridViewTextBoxColumn101.Width = 120;
            // 
            // dataGridViewTextBoxColumn102
            // 
            this.dataGridViewTextBoxColumn102.DataPropertyName = "AvailableUnits";
            this.dataGridViewTextBoxColumn102.HeaderText = "Available Units";
            this.dataGridViewTextBoxColumn102.MinimumWidth = 70;
            this.dataGridViewTextBoxColumn102.Name = "dataGridViewTextBoxColumn102";
            this.dataGridViewTextBoxColumn102.ReadOnly = true;
            this.dataGridViewTextBoxColumn102.Visible = false;
            this.dataGridViewTextBoxColumn102.Width = 70;
            // 
            // dataGridViewTextBoxColumn103
            // 
            this.dataGridViewTextBoxColumn103.DataPropertyName = "FacilityId";
            this.dataGridViewTextBoxColumn103.HeaderText = "Facility Id";
            this.dataGridViewTextBoxColumn103.Name = "dataGridViewTextBoxColumn103";
            this.dataGridViewTextBoxColumn103.Visible = false;
            // 
            // dataGridViewTextBoxColumn104
            // 
            this.dataGridViewTextBoxColumn104.DataPropertyName = "FacilityName";
            this.dataGridViewTextBoxColumn104.HeaderText = "Facility";
            this.dataGridViewTextBoxColumn104.MinimumWidth = 150;
            this.dataGridViewTextBoxColumn104.Name = "dataGridViewTextBoxColumn104";
            this.dataGridViewTextBoxColumn104.Visible = false;
            this.dataGridViewTextBoxColumn104.Width = 150;
            // 
            // dataGridViewTextBoxColumn105
            // 
            this.dataGridViewTextBoxColumn105.DataPropertyName = "MasterScheduleId";
            this.dataGridViewTextBoxColumn105.HeaderText = "Master Schedule Id";
            this.dataGridViewTextBoxColumn105.Name = "dataGridViewTextBoxColumn105";
            this.dataGridViewTextBoxColumn105.Visible = false;
            // 
            // dataGridViewTextBoxColumn106
            // 
            this.dataGridViewTextBoxColumn106.DataPropertyName = "ScheduleId";
            this.dataGridViewTextBoxColumn106.HeaderText = "Schedule Id";
            this.dataGridViewTextBoxColumn106.Name = "dataGridViewTextBoxColumn106";
            this.dataGridViewTextBoxColumn106.Visible = false;
            // 
            // dataGridViewTextBoxColumn107
            // 
            this.dataGridViewTextBoxColumn107.DataPropertyName = "ScheduleTime";
            this.dataGridViewTextBoxColumn107.HeaderText = "Schedule Time";
            this.dataGridViewTextBoxColumn107.MinimumWidth = 150;
            this.dataGridViewTextBoxColumn107.Name = "dataGridViewTextBoxColumn107";
            this.dataGridViewTextBoxColumn107.ReadOnly = true;
            this.dataGridViewTextBoxColumn107.Visible = false;
            this.dataGridViewTextBoxColumn107.Width = 150;
            // 
            // dataGridViewTextBoxColumn108
            // 
            this.dataGridViewTextBoxColumn108.DataPropertyName = "ScheduleFromTime";
            this.dataGridViewTextBoxColumn108.HeaderText = "From Time";
            this.dataGridViewTextBoxColumn108.MinimumWidth = 150;
            this.dataGridViewTextBoxColumn108.Name = "dataGridViewTextBoxColumn108";
            this.dataGridViewTextBoxColumn108.ReadOnly = true;
            this.dataGridViewTextBoxColumn108.Visible = false;
            this.dataGridViewTextBoxColumn108.Width = 150;
            // 
            // dataGridViewTextBoxColumn109
            // 
            this.dataGridViewTextBoxColumn109.DataPropertyName = "ScheduleToTime";
            this.dataGridViewTextBoxColumn109.HeaderText = "To Time";
            this.dataGridViewTextBoxColumn109.MinimumWidth = 150;
            this.dataGridViewTextBoxColumn109.Name = "dataGridViewTextBoxColumn109";
            this.dataGridViewTextBoxColumn109.ReadOnly = true;
            this.dataGridViewTextBoxColumn109.Visible = false;
            this.dataGridViewTextBoxColumn109.Width = 150;
            // 
            // dataGridViewTextBoxColumn110
            // 
            this.dataGridViewTextBoxColumn110.DataPropertyName = "ScheduleName";
            this.dataGridViewTextBoxColumn110.HeaderText = "Schedule Name";
            this.dataGridViewTextBoxColumn110.MinimumWidth = 150;
            this.dataGridViewTextBoxColumn110.Name = "dataGridViewTextBoxColumn110";
            this.dataGridViewTextBoxColumn110.ReadOnly = true;
            this.dataGridViewTextBoxColumn110.Visible = false;
            this.dataGridViewTextBoxColumn110.Width = 150;
            // 
            // dataGridViewTextBoxColumn111
            // 
            this.dataGridViewTextBoxColumn111.DataPropertyName = "MasterScheduleName";
            this.dataGridViewTextBoxColumn111.HeaderText = "Schedule Group";
            this.dataGridViewTextBoxColumn111.MinimumWidth = 150;
            this.dataGridViewTextBoxColumn111.Name = "dataGridViewTextBoxColumn111";
            this.dataGridViewTextBoxColumn111.ReadOnly = true;
            this.dataGridViewTextBoxColumn111.Visible = false;
            this.dataGridViewTextBoxColumn111.Width = 150;
            // 
            // dataGridViewTextBoxColumn112
            // 
            this.dataGridViewTextBoxColumn112.DataPropertyName = "AttractionPlayId";
            this.dataGridViewTextBoxColumn112.HeaderText = "Attraction Play Id";
            this.dataGridViewTextBoxColumn112.MinimumWidth = 70;
            this.dataGridViewTextBoxColumn112.Name = "dataGridViewTextBoxColumn112";
            this.dataGridViewTextBoxColumn112.ReadOnly = true;
            this.dataGridViewTextBoxColumn112.Visible = false;
            this.dataGridViewTextBoxColumn112.Width = 70;
            // 
            // dataGridViewTextBoxColumn113
            // 
            this.dataGridViewTextBoxColumn113.DataPropertyName = "AttractionPlayName";
            this.dataGridViewTextBoxColumn113.HeaderText = "Play Name";
            this.dataGridViewTextBoxColumn113.MinimumWidth = 150;
            this.dataGridViewTextBoxColumn113.Name = "dataGridViewTextBoxColumn113";
            this.dataGridViewTextBoxColumn113.Visible = false;
            this.dataGridViewTextBoxColumn113.Width = 150;
            // 
            // dataGridViewTextBoxColumn114
            // 
            this.dataGridViewTextBoxColumn114.DataPropertyName = "AttractionPlayPrice";
            this.dataGridViewTextBoxColumn114.HeaderText = "AttractionPlayPrice";
            this.dataGridViewTextBoxColumn114.MinimumWidth = 150;
            this.dataGridViewTextBoxColumn114.Name = "dataGridViewTextBoxColumn114";
            this.dataGridViewTextBoxColumn114.Visible = false;
            this.dataGridViewTextBoxColumn114.Width = 150;
            // 
            // dataGridViewTextBoxColumn115
            // 
            this.dataGridViewTextBoxColumn115.DataPropertyName = "ProductId";
            this.dataGridViewTextBoxColumn115.HeaderText = "Product Id";
            this.dataGridViewTextBoxColumn115.MinimumWidth = 150;
            this.dataGridViewTextBoxColumn115.Name = "dataGridViewTextBoxColumn115";
            this.dataGridViewTextBoxColumn115.Visible = false;
            this.dataGridViewTextBoxColumn115.Width = 150;
            // 
            // dataGridViewTextBoxColumn116
            // 
            this.dataGridViewTextBoxColumn116.DataPropertyName = "ProductName";
            this.dataGridViewTextBoxColumn116.HeaderText = "Product Name";
            this.dataGridViewTextBoxColumn116.MinimumWidth = 150;
            this.dataGridViewTextBoxColumn116.Name = "dataGridViewTextBoxColumn116";
            this.dataGridViewTextBoxColumn116.Visible = false;
            this.dataGridViewTextBoxColumn116.Width = 150;
            // 
            // dataGridViewTextBoxColumn117
            // 
            this.dataGridViewTextBoxColumn117.DataPropertyName = "Price";
            this.dataGridViewTextBoxColumn117.HeaderText = "Price";
            this.dataGridViewTextBoxColumn117.MinimumWidth = 50;
            this.dataGridViewTextBoxColumn117.Name = "dataGridViewTextBoxColumn117";
            this.dataGridViewTextBoxColumn117.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn117.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn117.Visible = false;
            this.dataGridViewTextBoxColumn117.Width = 50;
            // 
            // dataGridViewTextBoxColumn118
            // 
            this.dataGridViewTextBoxColumn118.DataPropertyName = "FacilityCapacity";
            this.dataGridViewTextBoxColumn118.HeaderText = "Facility Capacity";
            this.dataGridViewTextBoxColumn118.MinimumWidth = 150;
            this.dataGridViewTextBoxColumn118.Name = "dataGridViewTextBoxColumn118";
            this.dataGridViewTextBoxColumn118.Visible = false;
            this.dataGridViewTextBoxColumn118.Width = 150;
            // 
            // dataGridViewTextBoxColumn119
            // 
            this.dataGridViewTextBoxColumn119.DataPropertyName = "ProductLevelUnits";
            this.dataGridViewTextBoxColumn119.HeaderText = "Product Level Units";
            this.dataGridViewTextBoxColumn119.MinimumWidth = 150;
            this.dataGridViewTextBoxColumn119.Name = "dataGridViewTextBoxColumn119";
            this.dataGridViewTextBoxColumn119.ReadOnly = true;
            this.dataGridViewTextBoxColumn119.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn119.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn119.Visible = false;
            this.dataGridViewTextBoxColumn119.Width = 150;
            // 
            // dataGridViewTextBoxColumn120
            // 
            this.dataGridViewTextBoxColumn120.DataPropertyName = "RuleUnits";
            this.dataGridViewTextBoxColumn120.HeaderText = "Rule Units";
            this.dataGridViewTextBoxColumn120.MinimumWidth = 150;
            this.dataGridViewTextBoxColumn120.Name = "dataGridViewTextBoxColumn120";
            this.dataGridViewTextBoxColumn120.ReadOnly = true;
            this.dataGridViewTextBoxColumn120.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn120.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn120.Visible = false;
            this.dataGridViewTextBoxColumn120.Width = 150;
            // 
            // dataGridViewTextBoxColumn121
            // 
            this.dataGridViewTextBoxColumn121.DataPropertyName = "TotalUnits";
            this.dataGridViewTextBoxColumn121.HeaderText = "Total Units";
            this.dataGridViewTextBoxColumn121.MinimumWidth = 120;
            this.dataGridViewTextBoxColumn121.Name = "dataGridViewTextBoxColumn121";
            this.dataGridViewTextBoxColumn121.ReadOnly = true;
            this.dataGridViewTextBoxColumn121.Visible = false;
            this.dataGridViewTextBoxColumn121.Width = 120;
            // 
            // dataGridViewTextBoxColumn122
            // 
            this.dataGridViewTextBoxColumn122.DataPropertyName = "BookedUnits";
            this.dataGridViewTextBoxColumn122.HeaderText = "Booked Units";
            this.dataGridViewTextBoxColumn122.MinimumWidth = 150;
            this.dataGridViewTextBoxColumn122.Name = "dataGridViewTextBoxColumn122";
            this.dataGridViewTextBoxColumn122.Visible = false;
            this.dataGridViewTextBoxColumn122.Width = 150;
            // 
            // dataGridViewTextBoxColumn123
            // 
            this.dataGridViewTextBoxColumn123.DataPropertyName = "AvailableUnits";
            this.dataGridViewTextBoxColumn123.HeaderText = "Available Units";
            this.dataGridViewTextBoxColumn123.MinimumWidth = 120;
            this.dataGridViewTextBoxColumn123.Name = "dataGridViewTextBoxColumn123";
            this.dataGridViewTextBoxColumn123.Visible = false;
            this.dataGridViewTextBoxColumn123.Width = 120;
            // 
            // dataGridViewTextBoxColumn124
            // 
            this.dataGridViewTextBoxColumn124.DataPropertyName = "DesiredUnits";
            this.dataGridViewTextBoxColumn124.HeaderText = "Desired Units";
            this.dataGridViewTextBoxColumn124.Name = "dataGridViewTextBoxColumn124";
            this.dataGridViewTextBoxColumn124.Visible = false;
            // 
            // dataGridViewTextBoxColumn125
            // 
            this.dataGridViewTextBoxColumn125.DataPropertyName = "ExpiryDate";
            this.dataGridViewTextBoxColumn125.HeaderText = "ExpiryDate";
            this.dataGridViewTextBoxColumn125.MinimumWidth = 100;
            this.dataGridViewTextBoxColumn125.Name = "dataGridViewTextBoxColumn125";
            this.dataGridViewTextBoxColumn125.Visible = false;
            // 
            // dataGridViewTextBoxColumn126
            // 
            this.dataGridViewTextBoxColumn126.DataPropertyName = "CategoryId";
            this.dataGridViewTextBoxColumn126.HeaderText = "Category Id";
            this.dataGridViewTextBoxColumn126.MinimumWidth = 150;
            this.dataGridViewTextBoxColumn126.Name = "dataGridViewTextBoxColumn126";
            this.dataGridViewTextBoxColumn126.Visible = false;
            this.dataGridViewTextBoxColumn126.Width = 150;
            // 
            // dataGridViewTextBoxColumn127
            // 
            this.dataGridViewTextBoxColumn127.DataPropertyName = "PromotionId";
            this.dataGridViewTextBoxColumn127.HeaderText = "Promotion Id";
            this.dataGridViewTextBoxColumn127.MinimumWidth = 100;
            this.dataGridViewTextBoxColumn127.Name = "dataGridViewTextBoxColumn127";
            this.dataGridViewTextBoxColumn127.Visible = false;
            this.dataGridViewTextBoxColumn127.Width = 150;
            // 
            // dataGridViewTextBoxColumn128
            // 
            this.dataGridViewTextBoxColumn128.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn128.DataPropertyName = "Seats";
            this.dataGridViewTextBoxColumn128.HeaderText = "Seats";
            this.dataGridViewTextBoxColumn128.MinimumWidth = 120;
            this.dataGridViewTextBoxColumn128.Name = "dataGridViewTextBoxColumn128";
            this.dataGridViewTextBoxColumn128.Visible = false;
            // 
            // dataGridViewTextBoxColumn129
            // 
            this.dataGridViewTextBoxColumn129.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn129.DataPropertyName = "FacilityDTO";
            this.dataGridViewTextBoxColumn129.HeaderText = "FacilityDTO";
            this.dataGridViewTextBoxColumn129.MinimumWidth = 200;
            this.dataGridViewTextBoxColumn129.Name = "dataGridViewTextBoxColumn129";
            this.dataGridViewTextBoxColumn129.Visible = false;
            // 
            // dataGridViewTextBoxColumn130
            // 
            this.dataGridViewTextBoxColumn130.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn130.DataPropertyName = "BookedUnits";
            this.dataGridViewTextBoxColumn130.HeaderText = "Booked Units";
            this.dataGridViewTextBoxColumn130.MinimumWidth = 120;
            this.dataGridViewTextBoxColumn130.Name = "dataGridViewTextBoxColumn130";
            this.dataGridViewTextBoxColumn130.Visible = false;
            // 
            // dataGridViewTextBoxColumn131
            // 
            this.dataGridViewTextBoxColumn131.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn131.DataPropertyName = "AvailableUnits";
            this.dataGridViewTextBoxColumn131.HeaderText = "Available Units";
            this.dataGridViewTextBoxColumn131.MinimumWidth = 200;
            this.dataGridViewTextBoxColumn131.Name = "dataGridViewTextBoxColumn131";
            this.dataGridViewTextBoxColumn131.Visible = false;
            // 
            // dataGridViewTextBoxColumn132
            // 
            this.dataGridViewTextBoxColumn132.DataPropertyName = "DesiredUnits";
            this.dataGridViewTextBoxColumn132.HeaderText = "Desired Units";
            this.dataGridViewTextBoxColumn132.Name = "dataGridViewTextBoxColumn132";
            this.dataGridViewTextBoxColumn132.Visible = false;
            // 
            // dataGridViewTextBoxColumn133
            // 
            this.dataGridViewTextBoxColumn133.DataPropertyName = "ExpiryDate";
            this.dataGridViewTextBoxColumn133.HeaderText = "ExpiryDate";
            this.dataGridViewTextBoxColumn133.Name = "dataGridViewTextBoxColumn133";
            this.dataGridViewTextBoxColumn133.Visible = false;
            // 
            // dataGridViewTextBoxColumn134
            // 
            this.dataGridViewTextBoxColumn134.DataPropertyName = "CategoryId";
            this.dataGridViewTextBoxColumn134.HeaderText = "Category Id";
            this.dataGridViewTextBoxColumn134.Name = "dataGridViewTextBoxColumn134";
            this.dataGridViewTextBoxColumn134.Visible = false;
            // 
            // dataGridViewTextBoxColumn135
            // 
            this.dataGridViewTextBoxColumn135.DataPropertyName = "PromotionId";
            this.dataGridViewTextBoxColumn135.HeaderText = "Promotion Id";
            this.dataGridViewTextBoxColumn135.Name = "dataGridViewTextBoxColumn135";
            this.dataGridViewTextBoxColumn135.Visible = false;
            // 
            // dataGridViewTextBoxColumn136
            // 
            this.dataGridViewTextBoxColumn136.DataPropertyName = "Seats";
            this.dataGridViewTextBoxColumn136.HeaderText = "Seats";
            this.dataGridViewTextBoxColumn136.Name = "dataGridViewTextBoxColumn136";
            this.dataGridViewTextBoxColumn136.Visible = false;
            // 
            // dataGridViewTextBoxColumn137
            // 
            this.dataGridViewTextBoxColumn137.DataPropertyName = "FacilityDTO";
            this.dataGridViewTextBoxColumn137.HeaderText = "FacilityDTO";
            this.dataGridViewTextBoxColumn137.Name = "dataGridViewTextBoxColumn137";
            this.dataGridViewTextBoxColumn137.Visible = false;
            // 
            // dataGridViewTextBoxColumn138
            // 
            this.dataGridViewTextBoxColumn138.DataPropertyName = "MaintChklstdetId";
            this.dataGridViewTextBoxColumn138.HeaderText = "Job Item ID";
            this.dataGridViewTextBoxColumn138.Name = "dataGridViewTextBoxColumn138";
            this.dataGridViewTextBoxColumn138.ReadOnly = true;
            this.dataGridViewTextBoxColumn138.Visible = false;
            // 
            // dataGridViewTextBoxColumn139
            // 
            this.dataGridViewTextBoxColumn139.DataPropertyName = "MaintJobName";
            this.dataGridViewTextBoxColumn139.HeaderText = "Job Name";
            this.dataGridViewTextBoxColumn139.MinimumWidth = 150;
            this.dataGridViewTextBoxColumn139.Name = "dataGridViewTextBoxColumn139";
            this.dataGridViewTextBoxColumn139.Visible = false;
            this.dataGridViewTextBoxColumn139.Width = 150;
            // 
            // dataGridViewTextBoxColumn140
            // 
            this.dataGridViewTextBoxColumn140.DataPropertyName = "ChklstScheduleTime";
            this.dataGridViewTextBoxColumn140.HeaderText = "Schedule Date";
            this.dataGridViewTextBoxColumn140.MinimumWidth = 120;
            this.dataGridViewTextBoxColumn140.Name = "dataGridViewTextBoxColumn140";
            this.dataGridViewTextBoxColumn140.ReadOnly = true;
            this.dataGridViewTextBoxColumn140.Visible = false;
            this.dataGridViewTextBoxColumn140.Width = 120;
            // 
            // dataGridViewTextBoxColumn141
            // 
            this.dataGridViewTextBoxColumn141.DataPropertyName = "JobTaskId";
            this.dataGridViewTextBoxColumn141.HeaderText = "JOb Task Id";
            this.dataGridViewTextBoxColumn141.MinimumWidth = 150;
            this.dataGridViewTextBoxColumn141.Name = "dataGridViewTextBoxColumn141";
            this.dataGridViewTextBoxColumn141.ReadOnly = true;
            this.dataGridViewTextBoxColumn141.Visible = false;
            this.dataGridViewTextBoxColumn141.Width = 150;
            // 
            // dataGridViewTextBoxColumn142
            // 
            this.dataGridViewTextBoxColumn142.DataPropertyName = "MaintJobType";
            this.dataGridViewTextBoxColumn142.HeaderText = "Job Type";
            this.dataGridViewTextBoxColumn142.MinimumWidth = 120;
            this.dataGridViewTextBoxColumn142.Name = "dataGridViewTextBoxColumn142";
            this.dataGridViewTextBoxColumn142.Visible = false;
            this.dataGridViewTextBoxColumn142.Width = 120;
            // 
            // dataGridViewTextBoxColumn143
            // 
            this.dataGridViewTextBoxColumn143.DataPropertyName = "AssignedTo";
            this.dataGridViewTextBoxColumn143.HeaderText = "Assigned To";
            this.dataGridViewTextBoxColumn143.Name = "dataGridViewTextBoxColumn143";
            this.dataGridViewTextBoxColumn143.Visible = false;
            // 
            // dataGridViewTextBoxColumn144
            // 
            this.dataGridViewTextBoxColumn144.DataPropertyName = "Status";
            this.dataGridViewTextBoxColumn144.HeaderText = "Status";
            this.dataGridViewTextBoxColumn144.MinimumWidth = 100;
            this.dataGridViewTextBoxColumn144.Name = "dataGridViewTextBoxColumn144";
            this.dataGridViewTextBoxColumn144.Visible = false;
            this.dataGridViewTextBoxColumn144.Width = 120;
            // 
            // dataGridViewTextBoxColumn145
            // 
            this.dataGridViewTextBoxColumn145.DataPropertyName = "AssignedUserId";
            this.dataGridViewTextBoxColumn145.HeaderText = "Assigned User";
            this.dataGridViewTextBoxColumn145.MinimumWidth = 150;
            this.dataGridViewTextBoxColumn145.Name = "dataGridViewTextBoxColumn145";
            this.dataGridViewTextBoxColumn145.Visible = false;
            this.dataGridViewTextBoxColumn145.Width = 150;
            // 
            // dataGridViewTextBoxColumn146
            // 
            this.dataGridViewTextBoxColumn146.DataPropertyName = "DepartmentId";
            this.dataGridViewTextBoxColumn146.HeaderText = "Department";
            this.dataGridViewTextBoxColumn146.MinimumWidth = 100;
            this.dataGridViewTextBoxColumn146.Name = "dataGridViewTextBoxColumn146";
            this.dataGridViewTextBoxColumn146.Visible = false;
            // 
            // dataGridViewTextBoxColumn147
            // 
            this.dataGridViewTextBoxColumn147.DataPropertyName = "ChecklistCloseDate";
            this.dataGridViewTextBoxColumn147.HeaderText = "Close Date";
            this.dataGridViewTextBoxColumn147.MinimumWidth = 120;
            this.dataGridViewTextBoxColumn147.Name = "dataGridViewTextBoxColumn147";
            this.dataGridViewTextBoxColumn147.Visible = false;
            this.dataGridViewTextBoxColumn147.Width = 120;
            // 
            // dataGridViewTextBoxColumn148
            // 
            this.dataGridViewTextBoxColumn148.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn148.DataPropertyName = "ChklistRemarks";
            this.dataGridViewTextBoxColumn148.HeaderText = "Remarks";
            this.dataGridViewTextBoxColumn148.MinimumWidth = 200;
            this.dataGridViewTextBoxColumn148.Name = "dataGridViewTextBoxColumn148";
            this.dataGridViewTextBoxColumn148.Visible = false;
            // 
            // dataGridViewTextBoxColumn149
            // 
            this.dataGridViewTextBoxColumn149.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn149.DataPropertyName = "TaskName";
            this.dataGridViewTextBoxColumn149.HeaderText = "Task Name";
            this.dataGridViewTextBoxColumn149.MinimumWidth = 120;
            this.dataGridViewTextBoxColumn149.Name = "dataGridViewTextBoxColumn149";
            this.dataGridViewTextBoxColumn149.Visible = false;
            // 
            // dataGridViewTextBoxColumn150
            // 
            this.dataGridViewTextBoxColumn150.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn150.DataPropertyName = "CardId";
            this.dataGridViewTextBoxColumn150.HeaderText = "Card Id";
            this.dataGridViewTextBoxColumn150.MinimumWidth = 200;
            this.dataGridViewTextBoxColumn150.Name = "dataGridViewTextBoxColumn150";
            this.dataGridViewTextBoxColumn150.Visible = false;
            // 
            // dataGridViewTextBoxColumn151
            // 
            this.dataGridViewTextBoxColumn151.DataPropertyName = "CardNumber";
            this.dataGridViewTextBoxColumn151.HeaderText = "Card Number";
            this.dataGridViewTextBoxColumn151.Name = "dataGridViewTextBoxColumn151";
            this.dataGridViewTextBoxColumn151.Visible = false;
            // 
            // dataGridViewTextBoxColumn152
            // 
            this.dataGridViewTextBoxColumn152.DataPropertyName = "TaskCardNumber";
            this.dataGridViewTextBoxColumn152.HeaderText = "Task Card Number";
            this.dataGridViewTextBoxColumn152.Name = "dataGridViewTextBoxColumn152";
            this.dataGridViewTextBoxColumn152.Visible = false;
            // 
            // dataGridViewTextBoxColumn153
            // 
            this.dataGridViewTextBoxColumn153.DataPropertyName = "AssetId";
            this.dataGridViewTextBoxColumn153.HeaderText = "Asset";
            this.dataGridViewTextBoxColumn153.Name = "dataGridViewTextBoxColumn153";
            this.dataGridViewTextBoxColumn153.Visible = false;
            // 
            // dataGridViewTextBoxColumn154
            // 
            this.dataGridViewTextBoxColumn154.DataPropertyName = "AssetName";
            this.dataGridViewTextBoxColumn154.HeaderText = "Asset Name";
            this.dataGridViewTextBoxColumn154.Name = "dataGridViewTextBoxColumn154";
            this.dataGridViewTextBoxColumn154.Visible = false;
            // 
            // dataGridViewTextBoxColumn155
            // 
            this.dataGridViewTextBoxColumn155.DataPropertyName = "AssetType";
            this.dataGridViewTextBoxColumn155.HeaderText = "Asset Type";
            this.dataGridViewTextBoxColumn155.Name = "dataGridViewTextBoxColumn155";
            this.dataGridViewTextBoxColumn155.Visible = false;
            // 
            // dataGridViewTextBoxColumn156
            // 
            this.dataGridViewTextBoxColumn156.DataPropertyName = "AssetGroupName";
            this.dataGridViewTextBoxColumn156.HeaderText = "Asset Group Name";
            this.dataGridViewTextBoxColumn156.Name = "dataGridViewTextBoxColumn156";
            this.dataGridViewTextBoxColumn156.Visible = false;
            // 
            // dataGridViewTextBoxColumn157
            // 
            this.dataGridViewTextBoxColumn157.DataPropertyName = "SourceSystemId";
            this.dataGridViewTextBoxColumn157.HeaderText = "Source System Id";
            this.dataGridViewTextBoxColumn157.Name = "dataGridViewTextBoxColumn157";
            this.dataGridViewTextBoxColumn157.Visible = false;
            // 
            // dataGridViewTextBoxColumn158
            // 
            this.dataGridViewTextBoxColumn158.DataPropertyName = "DurationToComplete";
            this.dataGridViewTextBoxColumn158.HeaderText = "Duration To Complete";
            this.dataGridViewTextBoxColumn158.Name = "dataGridViewTextBoxColumn158";
            this.dataGridViewTextBoxColumn158.Visible = false;
            // 
            // dataGridViewTextBoxColumn159
            // 
            this.dataGridViewTextBoxColumn159.DataPropertyName = "RequestType";
            this.dataGridViewTextBoxColumn159.HeaderText = "Request Type";
            this.dataGridViewTextBoxColumn159.Name = "dataGridViewTextBoxColumn159";
            this.dataGridViewTextBoxColumn159.Visible = false;
            // 
            // dataGridViewTextBoxColumn160
            // 
            this.dataGridViewTextBoxColumn160.DataPropertyName = "RequestDate";
            this.dataGridViewTextBoxColumn160.HeaderText = "Request Date";
            this.dataGridViewTextBoxColumn160.Name = "dataGridViewTextBoxColumn160";
            this.dataGridViewTextBoxColumn160.Visible = false;
            // 
            // dataGridViewTextBoxColumn161
            // 
            this.dataGridViewTextBoxColumn161.DataPropertyName = "Priority";
            this.dataGridViewTextBoxColumn161.HeaderText = "Priority";
            this.dataGridViewTextBoxColumn161.Name = "dataGridViewTextBoxColumn161";
            this.dataGridViewTextBoxColumn161.Visible = false;
            // 
            // dataGridViewTextBoxColumn162
            // 
            this.dataGridViewTextBoxColumn162.DataPropertyName = "RequestDetail";
            this.dataGridViewTextBoxColumn162.HeaderText = "Request Detail";
            this.dataGridViewTextBoxColumn162.Name = "dataGridViewTextBoxColumn162";
            this.dataGridViewTextBoxColumn162.Visible = false;
            // 
            // dataGridViewTextBoxColumn163
            // 
            this.dataGridViewTextBoxColumn163.DataPropertyName = "ImageName";
            this.dataGridViewTextBoxColumn163.HeaderText = "Image Name";
            this.dataGridViewTextBoxColumn163.Name = "dataGridViewTextBoxColumn163";
            this.dataGridViewTextBoxColumn163.Visible = false;
            // 
            // dataGridViewTextBoxColumn164
            // 
            this.dataGridViewTextBoxColumn164.DataPropertyName = "RequestedBy";
            this.dataGridViewTextBoxColumn164.HeaderText = "Requested By";
            this.dataGridViewTextBoxColumn164.Name = "dataGridViewTextBoxColumn164";
            this.dataGridViewTextBoxColumn164.Visible = false;
            // 
            // dataGridViewTextBoxColumn165
            // 
            this.dataGridViewTextBoxColumn165.DataPropertyName = "ContactPhone";
            this.dataGridViewTextBoxColumn165.HeaderText = "Contact Phone";
            this.dataGridViewTextBoxColumn165.Name = "dataGridViewTextBoxColumn165";
            this.dataGridViewTextBoxColumn165.Visible = false;
            // 
            // dataGridViewTextBoxColumn166
            // 
            this.dataGridViewTextBoxColumn166.DataPropertyName = "ContactEmailId";
            this.dataGridViewTextBoxColumn166.HeaderText = "Contact EmailId";
            this.dataGridViewTextBoxColumn166.Name = "dataGridViewTextBoxColumn166";
            this.dataGridViewTextBoxColumn166.Visible = false;
            // 
            // dataGridViewTextBoxColumn167
            // 
            this.dataGridViewTextBoxColumn167.DataPropertyName = "Resolution";
            this.dataGridViewTextBoxColumn167.HeaderText = "Resolution";
            this.dataGridViewTextBoxColumn167.Name = "dataGridViewTextBoxColumn167";
            this.dataGridViewTextBoxColumn167.Visible = false;
            // 
            // dataGridViewTextBoxColumn168
            // 
            this.dataGridViewTextBoxColumn168.DataPropertyName = "Comments";
            this.dataGridViewTextBoxColumn168.HeaderText = "Comments";
            this.dataGridViewTextBoxColumn168.Name = "dataGridViewTextBoxColumn168";
            this.dataGridViewTextBoxColumn168.Visible = false;
            // 
            // dataGridViewTextBoxColumn169
            // 
            this.dataGridViewTextBoxColumn169.DataPropertyName = "RepairCost";
            this.dataGridViewTextBoxColumn169.HeaderText = "Repair Cost";
            this.dataGridViewTextBoxColumn169.Name = "dataGridViewTextBoxColumn169";
            this.dataGridViewTextBoxColumn169.Visible = false;
            // 
            // dataGridViewTextBoxColumn170
            // 
            this.dataGridViewTextBoxColumn170.DataPropertyName = "DocFileName";
            this.dataGridViewTextBoxColumn170.HeaderText = "Doc File Name";
            this.dataGridViewTextBoxColumn170.Name = "dataGridViewTextBoxColumn170";
            this.dataGridViewTextBoxColumn170.Visible = false;
            // 
            // dataGridViewTextBoxColumn171
            // 
            this.dataGridViewTextBoxColumn171.DataPropertyName = "Attribute1";
            this.dataGridViewTextBoxColumn171.HeaderText = "Attribute";
            this.dataGridViewTextBoxColumn171.Name = "dataGridViewTextBoxColumn171";
            this.dataGridViewTextBoxColumn171.Visible = false;
            // 
            // dataGridViewTextBoxColumn172
            // 
            this.dataGridViewTextBoxColumn172.DataPropertyName = "DocFileName";
            this.dataGridViewTextBoxColumn172.HeaderText = "Doc File Name";
            this.dataGridViewTextBoxColumn172.Name = "dataGridViewTextBoxColumn172";
            this.dataGridViewTextBoxColumn172.Visible = false;
            // 
            // dataGridViewTextBoxColumn173
            // 
            this.dataGridViewTextBoxColumn173.DataPropertyName = "Attribute1";
            this.dataGridViewTextBoxColumn173.HeaderText = "Attribute";
            this.dataGridViewTextBoxColumn173.Name = "dataGridViewTextBoxColumn173";
            this.dataGridViewTextBoxColumn173.Visible = false;
            // 
            // inActivityTimerClock
            // 
            this.inActivityTimerClock.Interval = 1000;
            this.inActivityTimerClock.Tick += new System.EventHandler(this.inActivityTimerClock_Tick);
            // 
            // frmReservationUI
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoScroll = true;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(1310, 692);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.tcBooking);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmReservationUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Make a Booking";
            this.Deactivate += new System.EventHandler(this.frmReservationUI_Deactivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmReservationUI_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmReservationUI_FormClosed);
            this.Load += new System.EventHandler(this.frmReservationUI_Load);
            this.tcBooking.ResumeLayout(false);
            this.tpDateTime.ResumeLayout(false);
            this.gpbSelectedOptions.ResumeLayout(false);
            this.gpbSelectedOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSelectedBookingSchedule)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.transactionReservationScheduleDTOBindingSource)).EndInit();
            this.gpbSearchOptions.ResumeLayout(false);
            this.gpbFacilityProducts.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSearchSchedules)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.scheduleDetailsDTOBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSearchFacilityProducts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.productsDTOBindingSource)).EndInit();
            this.tpCustomer.ResumeLayout(false);
            this.tpCustomer.PerformLayout();
            this.tpPackageProducts.ResumeLayout(false);
            this.tpPackageProducts.PerformLayout();
            this.pnlPackageProducts.ResumeLayout(false);
            this.tpAdditionalProducts.ResumeLayout(false);
            this.pnlAdditionalProducts.ResumeLayout(false);
            this.tpSummary.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.gbxCharges.ResumeLayout(false);
            this.gbxCharges.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pcbRemoveGratuityAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcbRemoveServiceCharge)).EndInit();
            this.gbxSvcServiceCharge.ResumeLayout(false);
            this.gbxSvcServiceCharge.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbcSvcRemoveServiceCharge)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcbRemoveDiscCoupon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcbRemoveDiscount)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tpCheckList.ResumeLayout(false);
            this.grpCheckList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvUserJobTaskList)).EndInit();
            this.tpAuditDetails.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAuditTrail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.userJobItemsDTOBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #endregion
        private System.Windows.Forms.BindingSource scheduleDetailsDTOBindingSource;
        private System.Windows.Forms.BindingSource productsDTOBindingSource;
        private System.Windows.Forms.BindingSource transactionReservationScheduleDTOBindingSource;
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
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn18;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn19;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn20;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn21;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn22;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn23;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn24;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn25;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn26;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn27;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn28;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn29;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn30;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn31;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn32;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn33;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn34;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn35;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn36;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn37;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn38;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn39;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn40;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn41;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn42;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn43;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn44;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn45;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn46;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn47;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn48;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn49;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn50;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn51;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn52;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn53;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn54;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn55;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn56;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn57;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn58;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn59;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn60;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn61;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn62;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn63;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn64;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn65;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn66;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn67;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn68;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn69;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn70;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn71;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn72;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn73;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn74;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn75;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn76;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn77;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn78;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn79;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn80;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn81;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn82;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn83;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn84;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn85;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn86;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn87;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn88;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn89;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn90;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn91;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn92;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn93;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn94;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn95;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn96;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn97;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn98;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn99;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn100;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn101;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn102;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn103;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn104;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn105;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn106;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn107;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn108;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn109;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn110;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn111;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn112;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn113;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn114;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn115;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn116;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn117;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn118;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn119;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn120;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn121;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn122;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn123;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn124;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn125;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn126;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn127;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn128;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn129;
        private System.Windows.Forms.TabControl tcBooking;
        private System.Windows.Forms.TabPage tpDateTime;
        private Semnox.Core.GenericUtilities.VerticalScrollBarView selectedScheduleVScrollBar;
        private System.Windows.Forms.DataGridView dgvSearchSchedules;
        private System.Windows.Forms.GroupBox gpbSelectedOptions;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnNextDateTime;
        private System.Windows.Forms.DataGridView dgvSelectedBookingSchedule;
        private System.Windows.Forms.Button btnBlockSchedule;
        private Semnox.Core.GenericUtilities.AutoCompleteComboBox cmbChannel;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtBookingName;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.GroupBox gpbSearchOptions;
        private System.Windows.Forms.GroupBox gpbFacilityProducts;
        private Semnox.Core.GenericUtilities.HorizontalScrollBarView facProdHScrollBar;
        private System.Windows.Forms.Button btnAddToBooking;
        private Semnox.Core.GenericUtilities.VerticalScrollBarView facProdVScrollBar;
        private System.Windows.Forms.DataGridView dgvSearchFacilityProducts;
        private Semnox.Core.GenericUtilities.HorizontalScrollBarView searchedScheduleHScrollBar;
        private Semnox.Core.GenericUtilities.VerticalScrollBarView searchedScheduleVScrollBar;
        private Semnox.Core.GenericUtilities.AutoCompleteComboBox cmbSearchFacility;
        private System.Windows.Forms.Label lblFacility;
        private Semnox.Core.GenericUtilities.AutoCompleteComboBox cmbSearchBookingProduct;
        private System.Windows.Forms.Label label3;
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
        private System.Windows.Forms.TabPage tpCustomer;
        private Semnox.Core.GenericUtilities.VerticalScrollBarView custPanelVScrollBar;
        private System.Windows.Forms.Panel pnlCustomerDetail;
        private System.Windows.Forms.TextBox txtCardNumber;
        private System.Windows.Forms.Label lblCardNumber;
        private System.Windows.Forms.TabPage tpPackageProducts;
        private System.Windows.Forms.TabPage tpAdditionalProducts;
        private System.Windows.Forms.TabPage tpSummary;
        private System.Windows.Forms.TabPage tpCheckList;
        private System.Windows.Forms.TabPage tpAuditDetails;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.Button btnCustomerLookup;
        private System.Windows.Forms.Button btnPrevCustomer;
        private System.Windows.Forms.Button btnNextCustomer;
        private System.Windows.Forms.Label lblPkgQty;
        private System.Windows.Forms.Label lblPkgProductName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtRemarks;
        private System.Windows.Forms.Button btnPrevPackage;
        private System.Windows.Forms.Button btnNextPackage;
        private Semnox.Core.GenericUtilities.VerticalScrollBarView packageListVScrollBar; 
        private Semnox.Core.GenericUtilities.VerticalScrollBarView verticalScrollBarView3;
        private System.Windows.Forms.Panel pnlAdditionalProducts;
        private usrCtrlProductDetails usrCtrlAdditionProductDetails1;
        private usrCtrlProductDetails usrCtrlPkgProductDetails1;
        private System.Windows.Forms.Label lblAdditionalProdQty;
        private System.Windows.Forms.Label lblAdditionalProdPrice;
        private System.Windows.Forms.Label lblAdditionalProdName;
        private System.Windows.Forms.Button btnPrevAdditionalProducts;
        private System.Windows.Forms.Button btnNextAdditionalProducts;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtDiscountAmount;
        private System.Windows.Forms.Label lblDiscountAmount;
        private System.Windows.Forms.TextBox txtBalanceAmount;
        private System.Windows.Forms.Label lblBalanceAmount;
        private System.Windows.Forms.TextBox txtEstimateAmount;
        private System.Windows.Forms.Label lblEstimateAmount;
        private System.Windows.Forms.TextBox txtAdvancePaid;
        private System.Windows.Forms.Label lblPaid;
        private System.Windows.Forms.TextBox txtMinimumAdvanceAmount;
        private System.Windows.Forms.Label lblMinimumAdvAmount;
        private System.Windows.Forms.TextBox txtTransactionAmount;
        private System.Windows.Forms.Label lblTransactionAmount;
        private System.Windows.Forms.TextBox txtReservationStatus;
        private System.Windows.Forms.Label lblReservationStatus;
        private System.Windows.Forms.TextBox txtExpiryDate;
        private System.Windows.Forms.Label lblExpiryDate;
        private System.Windows.Forms.TextBox txtReservationCode;
        private System.Windows.Forms.Label lblReservationCode;
        private System.Windows.Forms.PictureBox pcbRemoveDiscount;
        private System.Windows.Forms.TextBox txtAppliedDiscount;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.PictureBox pcbRemoveDiscCoupon;
        private System.Windows.Forms.TextBox txtAppliedTransactionProfile;
        private System.Windows.Forms.Label lblAppliedTransactionProfile;
        private System.Windows.Forms.TextBox txtAppliedDiscountCoupon;
        private System.Windows.Forms.Label lblAppliedDiscountCoupon;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label lblAppliedDiscount;
        private System.Windows.Forms.Button btnEditBooking;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnPrevSummary;
        private System.Windows.Forms.Button btnBook;
        private System.Windows.Forms.Button btnCancelBooking;
        private System.Windows.Forms.Button btnExecute;
        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.Button btnPayment;
        private System.Windows.Forms.Button btnAddAttendees;
        private System.Windows.Forms.Button btnSendEmail;
        private System.Windows.Forms.Button btnSendPaymentLink;
        private System.Windows.Forms.Button btnAddTransactionProfile;
        private System.Windows.Forms.Button btnApplyDiscount;
        private System.Windows.Forms.Button btnApplyDiscCoupon;
        private Semnox.Core.GenericUtilities.CustomCheckBox cbxEmailSent;
        private System.Windows.Forms.GroupBox grpCheckList;
        private System.Windows.Forms.Label lblCheckListName;
        private System.Windows.Forms.Button btnPrintCheckList;
        private System.Windows.Forms.Button btnPrevCheckList;
        private System.Windows.Forms.Button btnNextCheckList;
        private System.Windows.Forms.DataGridView dgvUserJobTaskList;
        private Semnox.Core.GenericUtilities.HorizontalScrollBarView chkListHScrollBar;
        private Semnox.Core.GenericUtilities.VerticalScrollBarView chkListVScroolBar;
        private Semnox.Core.GenericUtilities.HorizontalScrollBarView auditRailHScrollBar;
        private System.Windows.Forms.DataGridView dgvAuditTrail;
        private System.Windows.Forms.Label lblAuditTrail;
        private Semnox.Core.GenericUtilities.VerticalScrollBarView auditTrailVScrollBar;
        private System.Windows.Forms.Button btnSaveCheckList;
        private System.Windows.Forms.Button btnPrevAuditTrail;
        private System.Windows.Forms.Button btnNextSummary;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn130;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn131;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn132;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn133;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn134;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn135;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn136;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn137;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn138;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn139;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn140;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn141;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn142;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn143;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn144;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn145;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn146;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn147;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn148;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn149;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn150;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn151;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn152;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn153;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn154;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn155;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn156;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn157;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn158;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn159;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn160;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn161;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn162;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn163;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn164;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn165;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn166;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn167;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn168;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn169;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn170;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn171;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn172;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn173;
        private System.Windows.Forms.Panel pnlPackageProducts;
        private System.Windows.Forms.Button btnClearBooking;
        private System.Windows.Forms.Button btnPrintBooking; 
        private System.Windows.Forms.Button btnShowKeyPad;
        private System.Windows.Forms.Button btnPkgTabShowKeyPad; 
        private System.Windows.Forms.DataGridViewTextBoxColumn facilityMapIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ScheduleFromDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn ScheduleToDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn facilityMapNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn masterScheduleIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn scheduleIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn scheduleFromTimeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn scheduleToTimeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn fixedScheduleDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn availableUnitsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn scheduleNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn masterScheduleNameDataGridViewTextBoxColumn;
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
        private System.Windows.Forms.DataGridViewTextBoxColumn desiredUnitsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn expiryDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn categoryIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn promotionIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn seatsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn facilityMapDTODataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn productNameDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProductType;
        private System.Windows.Forms.DataGridViewTextBoxColumn productIdDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn activeFlagDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn productTypeIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn SelectedRecord;
        private System.Windows.Forms.DataGridViewButtonColumn productInformation;
        private System.Windows.Forms.TextBox txtGuestQty;
        private System.Windows.Forms.Label lblGuestQty;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox gbxCharges; 
        private System.Windows.Forms.PictureBox pcbRemoveServiceCharge;
        private System.Windows.Forms.Label lblGratuityAmount;
        private System.Windows.Forms.Label lblServiceChargeAmount;
        private System.Windows.Forms.Button btnMapWaivers;
        private System.Windows.Forms.ComboBox cmbEventCheckList;
        private System.Windows.Forms.BindingSource userJobItemsDTOBindingSource;
        private System.Windows.Forms.Button btnBookingCheckList;
        private System.Windows.Forms.DataGridViewCheckBoxColumn remarksMandatoryDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn chklistValue;
        private System.Windows.Forms.DataGridViewCheckBoxColumn validateTagDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isActiveDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn assignedUserId;
        private System.Windows.Forms.DataGridViewComboBoxColumn checkListStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn maintChklstdetId;
        private System.Windows.Forms.DataGridViewTextBoxColumn maintJobNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn taskNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn chklstScheduleTimeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn jobTaskIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn maintJobTypeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn assignedToDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn departmentIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn chklistRemarks;
        private System.Windows.Forms.DataGridViewTextBoxColumn checklistCloseDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn cardIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cardNumberDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn taskCardNumberDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn assetIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn assetNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn assetTypeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn assetGroupNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn sourceSystemIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn durationToCompleteDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn requestTypeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn requestDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn priorityDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn requestDetailDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn imageNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn requestedByDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn contactPhoneDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn contactEmailIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn resolutionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn commentsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn repairCostDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn docFileNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn attribute1DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn cancelledDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isChangedDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ReservationDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn trxReservationScheduleIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn trxIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lineIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn trxLineProductIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn scheduleNameDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn scheduleFromDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn scheduleToDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn guestQuantityDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn facilityMapNameDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn trxLineProductNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn schedulesIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn facilityMapIdDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn cancelledByDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn cmbFromTime;
        private System.Windows.Forms.DataGridViewComboBoxColumn cmbToTime;
        private System.Windows.Forms.DataGridViewButtonColumn RemoveLine;
        private System.Windows.Forms.DataGridViewTextBoxColumn BookingProductName;
        private System.Windows.Forms.DataGridViewTextBoxColumn BookingProductId;
        private System.Windows.Forms.DataGridViewTextBoxColumn BookingProductTrxLine;
        private System.Windows.Forms.DataGridViewTextBoxColumn FacilityTrxLine;
        private System.Windows.Forms.DataGridViewTextBoxColumn guestQty;
        private System.Windows.Forms.DataGridViewImageColumn Reschedule;
        private Semnox.Core.GenericUtilities.CustomCheckBox cbxHideBookedSlots;
        private System.Windows.Forms.PictureBox pcbRemoveGratuityAmount;
        private System.Windows.Forms.Button btnApplyGratuityAmount;
        private System.Windows.Forms.TextBox txtGratuityAmount;
        private System.Windows.Forms.TextBox txtServiceChrageAmount;
        private System.Windows.Forms.Button btnApplyServiceCharge;
        private System.Windows.Forms.GroupBox gbxSvcServiceCharge;
        private System.Windows.Forms.TextBox txtSvcServiceChargePercentage;
        private System.Windows.Forms.TextBox txtSvcServiceChargeAmount;
        private System.Windows.Forms.PictureBox pbcSvcRemoveServiceCharge;
        private System.Windows.Forms.Label lblSvcServiceChargePercentage;
        private System.Windows.Forms.Label lblSvcServiceChargeAmount;
        private System.Windows.Forms.TextBox txtSvcTrxServiceCharges;
        private System.Windows.Forms.Label lblSvcServiceChargeSummary;
        private System.Windows.Forms.Timer inActivityTimerClock;
    }
}