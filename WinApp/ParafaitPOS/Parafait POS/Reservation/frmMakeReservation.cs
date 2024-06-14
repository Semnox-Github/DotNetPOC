/********************************************************************************************
 * Project Name - Reservations
 * Description  - UI for booking
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 ********************************************************************************************* 
 *2.50.0      28-Nov-2018      Guru S A       UI enhancements
 * 2.70        1-Jul-2019      Lakshmi        Modified to add support for ULC cards 
 *2.70.0      26-Mar-2019     Guru S A        Booking phase 2 enhancement changes - Decommissioned
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.Net.Mail;
using System.Net.Mime;
using System.IO;
using System.Drawing.Imaging;
using Semnox.Parafait.Device;
using Semnox.Parafait.Discounts;
using Semnox.Core.Utilities;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.Customer;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Booking;
using Semnox.Parafait.Product;
using Semnox.Parafait.Report.Reports;
using Semnox.Parafait.Reports;
using iTextSharp.text.pdf;
using System.Diagnostics;
using Semnox.Parafait.Printer;
using Telerik.WinControls.UI;

namespace Parafait_POS.Reservation
{
    public partial class frmMakeReservation : Form
    {
        public int BookingId = -1;
        //Added 2 parameters to frmMakeReservation to Enable Edit feature after confirming the Reservation with Payment on May 18, 2015//
        int editedBookingId = -1;
        bool isEditedBooking = false;
        public int packageIdSelected;
        bool userAction = false;
        int productQuantity;
        string productType;
        int attractionScheduleId = -1;
        double bookingProductPrice = 0.0;
        public int facilityId = -1;//Added to send facility Id as parameter to get check availablity on Dec-07-2015//
        DateTime selectedFromTime = DateTime.MaxValue;
        DateTime selectedToTime = DateTime.MaxValue;
        string loginId = "";
        string contentID = "";
        List<string> cardList;
        List<string> cardFinalList;
        List<KeyValuePair<int, int>> CategoryProductList;
        List<KeyValuePair<int, int>> CategoryProuctFinalList;//Added to add the card list 
        List<Semnox.Parafait.Transaction.Reservation.clsSelectedCategoryProducts> CategoryProductSelectedList;
        public List<Semnox.Parafait.Transaction.Reservation.clsSelectedCategoryProducts> CategoryProuctFinalSelectedList;//Added to add the selected category products list 
        public List<DiscountsDTO> reservationTransactionDiscounts;
        List<Semnox.Parafait.Transaction.Reservation.clsPackageList> lstPacakgeIdList;
        Semnox.Parafait.Transaction.Reservation.clsPackageList packageIdList;
        string excludeAttractionSchedule = "-1";
        int exattractionScheduleId;
        DataTable dtBookingSchedule;
        private readonly TagNumberParser tagNumberParser;

        Utilities Utilities = POSStatic.Utilities;
        //Added TransactionUtils on 20-Jan-2016
        TransactionUtils trxUtils = new TransactionUtils(POSStatic.Utilities);
        Semnox.Core.Utilities.EventLog audit;//Added to access logEvent method from ParafaitUtils
        MessageUtils MessageUtils = POSStatic.MessageUtils;
        TaskProcs TaskProcs = POSStatic.TaskProcs;
        ParafaitEnv ParafaitEnv = POSStatic.ParafaitEnv;
        Semnox.Parafait.Transaction.Reservation Reservation;
        Card currentCard;
        DataTable dtBookingProduct = new DataTable();
        DataTable dtPackageList;
        //decimal fromTime;
        DataTable dtProducts;
        DataTable dtFromTime = new DataTable();
        DataTable dtToTime = new DataTable();
        BindingSource bsFromTime = new BindingSource();
        BindingSource bsToTime = new BindingSource();
        int guestCount = -99999;
        //AttractionBooking atb;//Added to Add atb object details to a list when attraction products are selected on Jan-20-2016//
        List<KeyValuePair<AttractionBooking, int>> lstAttractionProductslist;//Added to Add atb object details to a list when attraction products are selected on Jan-20-2016//
        List<KeyValuePair<AttractionBooking, int>> lstAdditionalAttractionProductsList;//Added to Add atb object details to a list when attraction products are selected on Jan-20-2016//
        //Begin: Modified Added for logger function on 08-Mar-2016
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //End: Modified Added for logger function on 08-Mar-2016

        //Starts - Modification for F&B Restructuring of modifiers on 17-Oct-2018
        //List<PurchasedProducts> purchasedProductSelected = new List<PurchasedProducts>();
        //List<PurchasedProducts> purchasedAdditionalProduct = new List<PurchasedProducts>();
        List<KeyValuePair<PurchasedProducts, int>> lstPurchasedModifierProducts = new List<KeyValuePair<PurchasedProducts, int>>();
        List<KeyValuePair<PurchasedProducts, int>> lstPurchasedAdditionalModifierProducts = new List<KeyValuePair<PurchasedProducts, int>>();
        //Ends - Modification for F&B Restructuring of modifiers on 17-Oct-2018

        CustomerDetailUI customerDetailUI;

        CustomerDTO customerDTO;
        int newLineId = 99999;
        ReservationDTO reservationDTO;
        List<Semnox.Parafait.Transaction.Reservation.clsProductList> additionalProductInformationLst = new List<Semnox.Parafait.Transaction.Reservation.clsProductList>();
        public frmMakeReservation(int pBookingId)
        {
            log.LogMethodEntry(pBookingId);
            //Begin: Added to Configure the logger root LogLevel using App.config on 08-March-2016           
            //log = Logger.setLogFileAppenderName(log);
            userAction = false;
            Semnox.Core.Utilities.Logger.setRootLogLevel(log);
            //End: Added to Configure the logger root LogLevel using App.config on 08-March-2016 
            BookingId = pBookingId;
            Init();
            ParafaitEnv.Initialize();
            userAction = true;
            tagNumberParser = new TagNumberParser(Utilities.ExecutionContext);
            //LoadSchedules();
            log.LogMethodExit();
        }

        //Added 2 parameters to frmMakeReservation to Enable Edit feature after confirming the Reservation with Payment on May 18, 2015//
        public frmMakeReservation(int pBookingId, bool pisEditedBooking, int peditedBookingId)
        {
            log.LogMethodEntry(pBookingId, pisEditedBooking, peditedBookingId);
            userAction = false;
            Semnox.Core.Utilities.Logger.setRootLogLevel(log);
            BookingId = pBookingId;
            editedBookingId = peditedBookingId;
            isEditedBooking = pisEditedBooking;
            Init();
            btnBook.Text = MessageContainer.GetMessage(Utilities.ExecutionContext, "Save");
            userAction = true;
            LoadSchedules();
            tagNumberParser = new TagNumberParser(Utilities.ExecutionContext);
            log.LogMethodExit();
        }

        public frmMakeReservation(DateTime pDate, decimal pfromTime)
        {
            log.LogMethodEntry(pDate, pfromTime);
            tagNumberParser = new TagNumberParser(Utilities.ExecutionContext);
            userAction = false;
            Semnox.Core.Utilities.Logger.setRootLogLevel(log);
            Init();
            dtpforDate.Value = pDate;  
            if (pfromTime > (decimal)23.45 && pfromTime <= (decimal)24)
                pfromTime = (decimal)23.45;
            cmbFromTimeForSearch.SelectedValue = pfromTime;
            //Begin Added to load fro time + 2 hrs to  totime
            decimal fromTime = Convert.ToDecimal(cmbFromTimeForSearch.SelectedValue);
            int totToMins = (int)(Math.Floor(fromTime) * 60 + (fromTime - Math.Floor(fromTime)) * 100 + 120);
            decimal toTime = totToMins / 60 + (decimal)((totToMins % 60) / 100.0);
            if (toTime > (decimal)23.45)
                toTime = (decimal)23.55;
            //G cmbToTime.SelectedValue = toTime;
            cmbToTimeForSearch.SelectedValue = toTime;
            //Added to load fro time + 2 hrs to  totime
            fromTime = pfromTime;
            lblStatus.Text = ReservationDTO.ReservationStatus.NEW.ToString();
            lblReservationCode.Text = "";
            lblExpires.Text = "";
            lblAppliedTrxProfileName.Text = "";
            lblAppliedTrxProfileName.Tag = -1;
            //btnPrint.Enabled = btnEmail.Enabled = btnAttendeeDetails.Enabled = btnConfirm.Enabled =
            //btnCancelBooking.Enabled = btnAttendeeDetails.Enabled = btnEditBooking.Enabled = false;
            cmbRecurFrequency.SelectedIndex = 0;
            cmbChannel.SelectedItem = "Phone";
            //cmbRecurFrequency.Enabled = false;
            //dtpRecurUntil.Enabled = false;
            userAction = true;
            LoadSchedules();
            log.LogMethodExit();
        }

        private void Init()
        {
            log.LogMethodEntry();
            //Utilities.setLanguage();

            //InitializeComponent();

            //LoadCustomerUIPanel();
            //DateTime selectedFromTime = DateTime.MaxValue;
            //DateTime selectedToTime = DateTime.MaxValue;
            //Reservation = new Semnox.Parafait.Transaction.Reservation(BookingId, Utilities);
            //lstAttractionProductslist = new List<KeyValuePair<AttractionBooking, int>>();//Added to Add atb object details to a list when attraction products are selected on Jan-20-2016//
            //lstAdditionalAttractionProductsList = new List<KeyValuePair<AttractionBooking, int>>();//Added to Add atb object details to a list when attraction products are selected on Jan-20-2016//
            ////atb = new AttractionBooking(Utilities);//Added to Add atb object details to a list when attraction products are selected on Jan-20-2016//
            //CategoryProuctFinalList = new List<KeyValuePair<int, int>>();
            //CategoryProuctFinalSelectedList = new List<Semnox.Parafait.Transaction.Reservation.clsSelectedCategoryProducts>();
            //reservationTransactionDiscounts = new List<DiscountsDTO>();
            //cardFinalList = new List<string>();
            //currentCard = new Card(Utilities);
            ////btnFind.Enabled = false;

            //LoadBookingProductDropDownList();

            //LoadTimeDropdownLists();

            //SetStyleForDGVBookingProducts();

            //Common.Devices.RegisterCardReaders(new EventHandler(CardScanCompleteEventHandle)); //Added as per Devices logic
            //packageName.DataPropertyName = "product_name";
            //loginId = ParafaitEnv.LoginID;

            //LoadFacility();

            //GetTransactionProfiles();

            log.LogMethodExit();
        }


        private void LoadCustomerUIPanel()
        {
            log.LogMethodEntry();
            customerDetailUI = new CustomerDetailUI(Utilities, POSUtils.ParafaitMessageBox, ParafaitDefaultContainer.GetParafaitDefault<bool>(Utilities.ExecutionContext, "AUTO_POPUP_ONSCREEN_KEYBOARD"));
            customerDetailUI.FirstNameLeave += new EventHandler(CustomerDetailUI_FirstNameLeave);
            customerDetailUI.CustomerContactInfoEntered += new CustomerContactInfoEnteredEventHandler(CustomerDetailUI_CustomerContactInfoEntered);
            customerDetailUI.UniqueIdentifierValidating += new CancelEventHandler(txtUniqueId_Validating);
            customerDetailUI.Width = 885;
            customerDetailUI.SetBackGroundColor(tpCustomer.BackColor);
            customerDetailUI.Location = new Point((pnlCustomerDetail.Width - 1106) / 2, 0);
            pnlCustomerDetail.Controls.Add(customerDetailUI);
            log.LogMethodExit();
        }

        private void LoadBookingProductDropDownList()
        {
            log.LogMethodEntry();
            //ProductsList productsListBL = new ProductsList(Utilities.ExecutionContext);
            //List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>> searchParams = new List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>>();
            //searchParams.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.PRODUCT_TYPE_NAME, "BOOKINGS"));
            //searchParams.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.ISACTIVE, "Y"));
            //searchParams.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.SITEID, Utilities.ExecutionContext.GetSiteId().ToString()));
            //List<ProductsDTO> bookinProductsDTOLIst = productsListBL.GetProductsDTOList(searchParams);
            //if (bookinProductsDTOLIst == null)
            //{
            //    bookinProductsDTOLIst = new List<ProductsDTO>();
            //}
            //searchParams.Clear();
            //if (BookingId != -1 && Reservation != null && Reservation.GetReservationDTO != null )
            //{
            //    int bookedProductId = Reservation.GetReservationDTO.BookingProductId;
            //    searchParams.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.PRODUCT_TYPE_NAME, "BOOKINGS"));
            //    searchParams.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.ISACTIVE, "N"));
            //    searchParams.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.PRODUCT_ID, bookedProductId.ToString()));
            //    searchParams.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.SITEID, Utilities.ExecutionContext.GetSiteId().ToString()));
            //    List<ProductsDTO> inActivebookinProductsDTOLIst = productsListBL.GetProductsDTOList(searchParams);
            //    if (inActivebookinProductsDTOLIst != null && inActivebookinProductsDTOLIst.Count > 0)
            //    {
            //        foreach (ProductsDTO inactiveBooking in inActivebookinProductsDTOLIst)
            //        {
            //            bookinProductsDTOLIst.Add(inactiveBooking); 
            //        }
            //    }
            //}

            //bookinProductsDTOLIst = bookinProductsDTOLIst.OrderBy(prod => prod.ProductName).ToList(); 

            //ProductsDTO productsALLDTO = new ProductsDTO
            //{
            //    ProductName = "- All -"
            //};
            //bookinProductsDTOLIst.Insert(0, productsALLDTO);
            //cmbBookingClass.DisplayMember = "ProductName";
            //cmbBookingClass.ValueMember = "ProductId";
            //cmbBookingClass.DataSource = bookinProductsDTOLIst;
            //cmbBookingClass.SelectedIndex = 0;
            //this.cmbBookingClass.SelectedIndexChanged += new System.EventHandler(this.cmbBookingClass_SelectedIndexChanged);
            log.LogMethodExit();
        }

        private void LoadTimeDropdownLists()
        {
            log.LogMethodEntry();
            dtFromTime.Columns.Add("Display");
            dtFromTime.Columns.Add("Value");
            dtFromTime.Columns.Add(new DataColumn("Compare", typeof(DateTime)));//Added a Additional Column to comapre the dates with schedule

            dtToTime.Columns.Add("Display");
            dtToTime.Columns.Add("Value");

            DateTime startTime = Utilities.getServerTime().Date;
            while (startTime < Utilities.getServerTime().Date.AddDays(1))
            {
                dtFromTime.Rows.Add(startTime.ToString("h:mm tt"), startTime.Hour + Math.Round(startTime.Minute / 100.0, 2), startTime);
                dtToTime.Rows.Add(startTime.ToString("h:mm tt"), startTime.Hour + Math.Round(startTime.Minute / 100.0, 2));
                startTime = startTime.AddMinutes(5);
            }
            cmbFromTime.DisplayMember = "Display";
            cmbFromTime.ValueMember = "Value";
            bsFromTime.DataSource = dtFromTime;
            cmbFromTime.DataSource = bsFromTime;
            cmbToTime.DisplayMember = "Display";
            cmbToTime.ValueMember = "Value";
            bsToTime.DataSource = dtToTime;
            cmbToTime.DataSource = bsToTime;
            cmbFromTime.Enabled = cmbToTime.Enabled = false;
            cmbFromTimeForSearch.DisplayMember = "Display";
            cmbFromTimeForSearch.ValueMember = "Value";
            cmbFromTimeForSearch.DataSource = dtFromTime;
            cmbToTimeForSearch.DisplayMember = "Display";
            cmbToTimeForSearch.ValueMember = "Value";
            cmbToTimeForSearch.DataSource = dtToTime;
            log.LogMethodExit();
        }

        private void SetStyleForDGVBookingProducts()
        {
            log.LogMethodEntry();
            Utilities.setupDataGridProperties(ref dgvBookingProducts);
            dgvBookingProducts.Columns["dcQuantity"].DefaultCellStyle = Utilities.gridViewNumericCellStyle();
            dgvBookingProducts.Columns["dcPrice"].DefaultCellStyle = Utilities.gridViewAmountCellStyle();
            dgvBookingProducts.BorderStyle = BorderStyle.FixedSingle;
            dgvBookingProducts.Columns["dcProduct"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvBookingProducts.Columns["AdditionalDiscountName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            SetDGVCellFont(dgvBookingProducts);
            log.LogMethodExit();
        }

        private void CustomerDetailUI_CustomerContactInfoEntered(object source, CustomerContactInfoEnteredEventArgs e)
        {
            log.LogMethodEntry(source, e);
            CustomerListBL customerListBL = new CustomerListBL(Utilities.ExecutionContext);
            CustomerSearchCriteria customerSearchCriteria = new CustomerSearchCriteria(CustomerSearchByParameters.CONTACT_CONTACT_TYPE, Operator.EQUAL_TO, e.ContactType.ToString());
            customerSearchCriteria.And(CustomerSearchByParameters.CONTACT_ATTRIBUTE1, Operator.EQUAL_TO, e.ContactValue)
                                  .OrderBy(CustomerSearchByParameters.CUSTOMER_ID)
                                  .Paginate(0, 20);
            List<CustomerDTO> customerDTOList = customerListBL.GetCustomerDTOList(customerSearchCriteria, true, true);
            if (customerDTOList != null && customerDTOList.Count > 0)
            {
                if (customerDTOList.Count == 1 && customerDTOList[0].Id == customerDetailUI.CustomerDTO.Id)
                {
                    log.LogMethodExit("customerDTOList.Count == 1 && customerDTOList[0].Id == customerDetailUI.CustomerDTO.Id");
                    return;
                }
                CustomerLookupUI customerLookupUI = new CustomerLookupUI(Utilities, "", "", "",
                                                                                     e.ContactType == ContactType.EMAIL ? e.ContactValue : "",
                                                                                     e.ContactType == ContactType.PHONE ? e.ContactValue : "",
                                                                                     "");
                if (customerLookupUI.ShowDialog() == DialogResult.OK)
                {
                    customerDetailUI.CustomerDTO = customerLookupUI.SelectedCustomerDTO;
                }
            }
            log.LogMethodExit();
        }

        private void txtUniqueId_Validating(object sender, CancelEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            txtMessage.Text = "";

            if ((sender as TextBox).Text.Trim() == "")
            {
                log.LogMethodExit("Text.Trim() == ''");
                return;
            }

            List<CustomerDTO> customerDTOList = null;
            CustomerListBL customerListBL = new CustomerListBL(Utilities.ExecutionContext);
            CustomerSearchCriteria customerSearchCriteria = new CustomerSearchCriteria(CustomerSearchByParameters.PROFILE_UNIQUE_IDENTIFIER, Operator.EQUAL_TO, (sender as TextBox).Text.Trim());
            customerSearchCriteria.OrderBy(CustomerSearchByParameters.CUSTOMER_ID)
                                  .Paginate(0, 20);
            customerDTOList = customerListBL.GetCustomerDTOList(customerSearchCriteria, false, false);
            if (customerDTOList != null && customerDTOList.Count > 0)
            {

                if (POSUtils.ParafaitMessageBox(MessageUtils.getMessage(212, customerDTOList[0].FirstName + (customerDTOList[0].LastName == "" ? "" : " " + customerDTOList[0].LastName)), MessageContainer.GetMessage(Utilities.ExecutionContext, "Customer Registration"), MessageBoxButtons.OK) == System.Windows.Forms.DialogResult.OK)
                {
                    if (ParafaitEnv.ALLOW_DUPLICATE_UNIQUE_ID == "N")
                        e.Cancel = true;
                }
                else
                {
                    txtMessage.Text = MessageUtils.getMessage(290);
                    if (ParafaitEnv.ALLOW_DUPLICATE_UNIQUE_ID == "N")
                    {
                        e.Cancel = true;
                    }
                }
            }
            log.LogMethodExit();
        }

        private void CustomerDetailUI_FirstNameLeave(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            CustomerListBL customerListBL = new CustomerListBL(Utilities.ExecutionContext);
            CustomerSearchCriteria customerSearchCriteria = new CustomerSearchCriteria();
            customerSearchCriteria.And(CustomerSearchByParameters.PROFILE_FIRST_NAME, Operator.LIKE, customerDetailUI.FirstName)
                                  .OrderBy(CustomerSearchByParameters.CUSTOMER_ID)
                                  .Paginate(0, 20);
            List<CustomerDTO> customerDTOList = customerListBL.GetCustomerDTOList(customerSearchCriteria, true, true);
            if (customerDTOList != null && customerDTOList.Count > 0)
            {
                if (customerDTOList.Count == 1 && customerDTOList[0].Id == customerDetailUI.CustomerDTO.Id)
                {
                    log.LogMethodExit("customerDTOList.Count == 1 && customerDTOList[0].Id == customerDetailUI.CustomerDTO.Id");
                    return;
                }
                CustomerLookupUI customerLookupUI = new CustomerLookupUI(Utilities, customerDetailUI.FirstName);
                if (customerLookupUI.ShowDialog() == DialogResult.OK)
                {
                    customerDetailUI.CustomerDTO = customerLookupUI.SelectedCustomerDTO;
                }
            }
            log.LogMethodExit();
        }

        private void CardScanCompleteEventHandle(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e is DeviceScannedEventArgs)
            {
                DeviceScannedEventArgs checkScannedEvent = e as DeviceScannedEventArgs;
                TagNumber tagNumber;
                if (tagNumberParser.TryParse(checkScannedEvent.Message, out tagNumber) == false)
                {
                    string message = tagNumberParser.Validate(checkScannedEvent.Message);
                    txtMessage.Text = message;
                    log.LogMethodExit(null, "Invalid Tag Number. " + message);
                    return;
                }

                string CardNumber = tagNumber.Value;
                CardSwiped(CardNumber);
            }
            log.LogMethodExit();
        }

        private void CardSwiped(string CardNumber)
        {
            log.LogMethodEntry(CardNumber);
            if (BookingId == -1 || lblStatus.Text.ToUpper() == ReservationDTO.ReservationStatus.BLOCKED.ToString())
            {
                txtCardNumber.Text = CardNumber;
                ValidateCard();
            }
            log.LogMethodExit();
        }

        //Begin: Modified the method to get Booking Details
        private void GetBookingDetails()
        {
            log.LogMethodEntry();
            //userAction = false;

            //if (Reservation != null)
            //{
            //    ReservationDTO reservationDTO = Reservation.GetReservationDTO;
            //    //DataTable dt = Utilities.executeDataTable("select * from Bookings where BookingId = @id",
            //    //                                          new SqlParameter("@id", BookingId));
            //    if (reservationDTO != null)
            //    {
            //        int customerId = reservationDTO.CustomerId; //(dt.Rows[0]["CustomerId"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["CustomerId"].ToString()) : -1);
            //        if (customerId != -1)
            //        {
            //            CustomerDTO customerData = (new CustomerBL(Utilities.ExecutionContext, customerId)).CustomerDTO;
            //            customerDetailUI.CustomerDTO = customerData;
            //        }

            //        dtpforDate.Value = reservationDTO.FromDate.Date;//Convert.ToDateTime(dt.Rows[0]["FromDate"]).Date;
            //        txtBookingName.Text = reservationDTO.BookingName;//dt.Rows[0]["BookingName"].ToString();
            //        txtCardNumber.Text = reservationDTO.CardNumber;//dt.Rows[0]["CardNumber"].ToString();
            //        txtRemarks.Text = reservationDTO.Remarks;// dt.Rows[0]["Remarks"].ToString();
            //        cmbChannel.SelectedItem = reservationDTO.Channel;// dt.Rows[0]["Channel"];
            //        nudQuantity.Value = reservationDTO.Quantity;// Convert.ToInt32(dt.Rows[0]["Quantity"]);
            //                                                    //g nudGuestCount.Value = Convert.ToInt32(dt.Rows[0]["Quantity"]);
            //        txtGuests.Text = reservationDTO.Quantity.ToString();// dt.Rows[0]["Quantity"].ToString();
            //        guestCount = reservationDTO.Quantity; //Convert.ToInt32(dt.Rows[0]["Quantity"]);
            //        lblExpires.Text = (reservationDTO.ExpiryTime == null ? "" : Convert.ToDateTime(reservationDTO.ExpiryTime).ToString(ParafaitEnv.DATETIME_FORMAT));//(dt.Rows[0]["ExpiryTime"] == DBNull.Value ? "" : Convert.ToDateTime(dt.Rows[0]["ExpiryTime"]).ToString(ParafaitEnv.DATETIME_FORMAT));
            //        Reservation.ReservationCode = lblReservationCode.Text = reservationDTO.ReservationCode;//dt.Rows[0]["ReservationCode"].ToString();
            //        Reservation.Status = lblStatus.Text = reservationDTO.Status;// dt.Rows[0]["Status"].ToString();
            //        if (lblStatus.Text.ToUpper() == ReservationDTO.ReservationStatus.WIP.ToString())
            //        {
            //            isEditedBooking = true;
            //        }
            //        cbxRecur.Checked = reservationDTO.RecurFlag == "Y";// dt.Rows[0]["recur_flag"].ToString() == "Y";
            //        cmbRecurFrequency.SelectedItem = (reservationDTO.RecurFrequency == "W" ? "Weekly" : "Daily");// (dt.Rows[0]["recur_frequency"].ToString() == "W" ? "Weekly" : "Daily");
            //        dtpRecurUntil.Value = (reservationDTO.RecurEndDate == null ? Utilities.getServerTime() : Convert.ToDateTime(reservationDTO.RecurEndDate));//dt.Rows[0]["recur_end_date"] == DBNull.Value ? Utilities.getServerTime() : Convert.ToDateTime(dt.Rows[0]["recur_end_date"]);
            //        cbxEmailSent.Checked = (reservationDTO.IsEmailSent > 0);//((dt.Rows[0]["isEmailSent"] == DBNull.Value ? -1 : Convert.ToInt32(dt.Rows[0]["isEmailSent"])) > 0);

            //        //txtAddress.Text = dt.Rows[0]["PostalAddress"].ToString();//  dt.Rows[0]["PostalAddress"].ToString(); 
            //        cmbBookingClass.SelectedValue = reservationDTO.BookingProductId; // dt.Rows[0]["BookingProductId"];
            //        facilityId = reservationDTO.FacilityId;
            //        attractionScheduleId = reservationDTO.AttractionScheduleId;
            //        SchedulesBL schedulesBL = new SchedulesBL(Utilities.ExecutionContext, attractionScheduleId); 
            //        cmbFromTime.SelectedValue = reservationDTO.FromDate.Hour + reservationDTO.FromDate.Minute / 100.0; //Convert.ToDateTime(dt.Rows[0]["fromDate"]).Hour + Convert.ToDateTime(dt.Rows[0]["fromDate"]).Minute / 100.0;
            //        cmbToTime.SelectedValue = reservationDTO.ToDate.Hour + reservationDTO.ToDate.Minute / 100.0;//Convert.ToDateTime(dt.Rows[0]["ToDate"]).Hour + Convert.ToDateTime(dt.Rows[0]["ToDate"]).Minute / 100.0;

            //        cmbFromTimeForSearch.SelectedValue = Convert.ToDouble(schedulesBL.ScheduleDTO.ScheduleTime); 
            //        cmbToTimeForSearch.SelectedValue = Convert.ToDouble(schedulesBL.ScheduleDTO.ScheduleToTime);
            //        }
            //}
            //GetBookingPackage();

            //GetAdditionalBookingProducts();

            ////get the attraction schedule id selected if Bookingid !=-1
            ////GetAttractionSchedule(BookingId, Convert.ToInt32(cmbBookingClass.SelectedValue));
            //cmbFacility.SelectedValue = facilityId;
            //UpdateUIStatus();
            //selectedFromTime = SelectedFromDateTime();
            //selectedToTime = SelectedToDateTime();
            ////txtBookingEstimate.Text = Reservation.Transaction.Net_Transaction_Amount.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
            //txtAdvanceAmount.Text = Reservation.AdvanceRequired.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
            //lblAppliedTrxProfileName.Text = "";
            //lblAppliedTrxProfileName.Tag = -1;
            //if (Reservation.Transaction != null)
            //{
            //    txtBookingEstimate.Text = Reservation.Transaction.Net_Transaction_Amount.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
            //    txtPaidAmount.Text = Reservation.Transaction.TotalPaidAmount.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
            //    //Begin Modification Dec-24-2015 - Added to display the Balance Amount 
            //    txtBalanceAmount.Text = Math.Round(Reservation.Transaction.Net_Transaction_Amount - Reservation.Transaction.TotalPaidAmount, ParafaitEnv.RoundingPrecision, MidpointRounding.AwayFromZero).ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
            //    //End Modificaton Dec-24-2015//
            //    txtDiscount.Text = Reservation.Transaction.Discount_Amount.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);//Added txtDiscount & txtTransactionAmount to display Discount and Transaction Amount
            //    txtTransactionAmount.Text = Reservation.Transaction.Transaction_Amount.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
            //    if (!(Reservation.Transaction.TotalPaidAmount == 0.0) && (BookingId != -1))//Added to check if Payment is done, if True then enable edit Booking button
            //    {
            //        btnEditBooking.Enabled = true;
            //    }
            //    if (Reservation.Transaction.TrxProfileId != -1)
            //    {
            //        SetTransactionProfileName(Reservation.Transaction.TrxProfileId);
            //    }
            //}

            //if (isEditedBooking)//Added to change Button text to "Discounts"//
            //{
            //    lblStatus.Text = ReservationDTO.ReservationStatus.WIP.ToString();
            //    // btnExecute.Text = "Discounts";
            //    lblReservationCode.Text = "";
            //}
            ////if (lblStatus.Text == "NEW")//Added to change Button text to "Discounts"//
            ////   btnExecute.Text = "Discounts";
            //if (lblStatus.Text.ToUpper() == ReservationDTO.ReservationStatus.CONFIRMED.ToString()
            //    || lblStatus.Text.ToUpper() == ReservationDTO.ReservationStatus.BOOKED.ToString())//Added to Enable EditBooking button//
            //{
            //    btnEditBooking.Enabled = true;
            //}

            //userAction = true;
            //LoadSchedules();
            log.LogMethodExit();
        }

        private void SetTransactionProfileName(int trxProfileId)
        {
            log.LogMethodEntry(trxProfileId);
            lblAppliedTrxProfileName.Text = "";
            lblAppliedTrxProfileName.Tag = -1;
            TransactionProfileListBL transactionProfileListBL = new TransactionProfileListBL();
            List<KeyValuePair<TransactionProfileDTO.SearchByParameters, string>> searchParam = new List<KeyValuePair<TransactionProfileDTO.SearchByParameters, string>>
            {
                new KeyValuePair<TransactionProfileDTO.SearchByParameters, string>(TransactionProfileDTO.SearchByParameters.IS_ACTIVE, "1"),
                new KeyValuePair<TransactionProfileDTO.SearchByParameters, string>(TransactionProfileDTO.SearchByParameters.SITE_ID, Utilities.ExecutionContext.GetSiteId().ToString()),
                new KeyValuePair<TransactionProfileDTO.SearchByParameters, string>(TransactionProfileDTO.SearchByParameters.TRANSACTION_PROFILE_ID, trxProfileId.ToString())
            };
            List<TransactionProfileDTO> transactionProfileDTOList = transactionProfileListBL.GetTransactionProfileDTOList(searchParam);
            if (transactionProfileDTOList != null && transactionProfileDTOList.Count > 0)
            {
                lblAppliedTrxProfileName.Text = transactionProfileDTOList[0].ProfileName;
                lblAppliedTrxProfileName.Tag = trxProfileId;
            }
            log.LogMethodExit();
        }

        //End

        ////Begin: Get the Attraction Schedule Id after Boooking, While editing the reservation on Dec-7-2015//
        //private void GetAttractionSchedule(int BookingId, int bookingproductId)
        //{
        //    log.LogMethodEntry(BookingId, bookingproductId);
        //    try
        //    {
        //        double lclTime = Convert.ToDouble(cmbFromTime.SelectedValue);
        //        DateTime fromDateTime = dtpforDate.Value.Date.AddMinutes(Math.Floor(lclTime) * 60).AddMinutes((lclTime - Math.Floor(lclTime)) * 100.0);

        //        object o;
        //        o = Utilities.executeScalar(@"select AttractionScheduleId from Bookings where BookingId = @BookingId ", new SqlParameter("@BookingId", BookingId));
        //        if (o != DBNull.Value)
        //        {
        //            attractionScheduleId = Convert.ToInt32(o);
        //        }
        //        else
        //        {
        //            o = Utilities.executeScalar(@"SELECT top 1 AttractionScheduleId
        //                                                                   FROM AttractionSchedules
        //                                                                   INNER JOIN products p ON p.product_id = @BookingProductId
        //                                                                   AND p.AttractionMasterScheduleId = AttractionSchedules.AttractionMasterScheduleId
        //                                                                   WHERE
        //                                                                   @reservation >= DATEADD([mi], CONVERT(int, ScheduleTime) * 60 + ScheduleTime % 1 * 110, @fromDate) 
        //                                                                   AND  @reservation < DATEADD([mi], CONVERT(int, isnull(ScheduleToTime,23.59)) * 60 + isnull(ScheduleToTime,23.59) % 1 * 110, @fromDate)",
        //                                                                       new SqlParameter("@reservation", fromDateTime),
        //                                                                       new SqlParameter("@BookingProductId", bookingproductId),
        //                                                                       new SqlParameter("@fromDate", fromDateTime.Date));
        //            if (o != DBNull.Value)
        //            {
        //                attractionScheduleId = Convert.ToInt32(o);
        //            }
        //        }

        //        object id = Utilities.executeScalar(@"Select facilityId from AttractionSchedules where AttractionScheduleId = @attractionScheduleId ", new SqlParameter("@attractionScheduleId", attractionScheduleId));
        //        if (id != DBNull.Value)
        //        {
        //            facilityId = Convert.ToInt32(id);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(ex);
        //    }
        //    log.LogMethodExit();
        //}
        ////End:Get the Attraction Schedule Id after Boooking, While editing the reservation on Dec-7-2015//

        private void UpdateUIStatus()
        {
            log.LogMethodEntry();
            //dgvBookingProducts.Enabled =
            //btnConfirm.Enabled =
            //btnCancelBooking.Enabled =
            //cmbPackage.Enabled =
            //btnBook.Enabled = true;
            //nudQuantity.Enabled = true;
            //cmbFromTime.Enabled = cmbToTime.Enabled = false;
            //dgvPackageList.Enabled = true;
            //dgvAttractionSchedules.Enabled = true;
            ////btnExecute.Enabled = true;
            ////g nudGuestCount.Enabled = true;
            //txtGuests.Enabled = true;
            //btnReduceGuestCount.Enabled = true;
            //btnIncreaseGuestCount.Enabled = true;
            //dtpforDate.Enabled = true;
            //cbxEarlyMorning.Enabled = cbxMorning.Enabled = cbxAfternoon.Enabled = cbxNight.Enabled = true;
            //txtRemarks.Enabled = true;
            //txtBookingName.Enabled = true;
            //cmbChannel.Enabled = true;
            //cmbFacility.Enabled = true;
            //btnBlockSchedule.Enabled = true;
            //btnDiscounts.Enabled = false;
            //btnTrxProfiles.Enabled = false;
            //btnAttendeeDetails.Enabled = false;
            //btnAddServiceCharge.Enabled = false;
            //btnAddAttendeesInSummary.Enabled = false;
            //btnExecute.Enabled = false;
            //pnlCustomerDetail.Enabled = false;
            //btnPrint.Enabled = btnEmail.Enabled =  btnConfirm.Enabled = btnCancelBooking.Enabled =  btnEditBooking.Enabled = false;
            //cmbRecurFrequency.Enabled = false;
            //dtpRecurUntil.Enabled = false;
            //btnFind.Enabled = true;

            //cmbFromTimeForSearch.Enabled = cmbToTimeForSearch.Enabled = true;
            //if (lblStatus.Text.ToUpper() == ReservationDTO.ReservationStatus.NEW.ToString()
            //    || lblStatus.Text.ToUpper() == ReservationDTO.ReservationStatus.BLOCKED.ToString()
            //    || lblStatus.Text.ToUpper() == ReservationDTO.ReservationStatus.WIP.ToString())
            //{
            //    //btnExecute.Text = "Discounts";
            //    btnDiscounts.Enabled = true;
            //    //btnExecute.Enabled = true;
            //    btnTrxProfiles.Enabled = true;
            //    btnAddServiceCharge.Enabled = true;
            //    btnBook.Enabled = true;
            //    pnlCustomerDetail.Enabled = true;
            //}

            //if(  lblStatus.Text.ToUpper() == ReservationDTO.ReservationStatus.BLOCKED.ToString()
            //    || lblStatus.Text.ToUpper() == ReservationDTO.ReservationStatus.WIP.ToString())
            //{
            //    btnAddAttendeesInSummary.Enabled = true;
            //    btnAttendeeDetails.Enabled = true;
            //}

            //if (lblStatus.Text.ToUpper() != ReservationDTO.ReservationStatus.NEW.ToString() && lblStatus.Text.ToUpper() != ReservationDTO.ReservationStatus.BLOCKED.ToString())
            //{
            //    btnBlockSchedule.Enabled = false;
            //}

            //if (BookingId > 0 && lblStatus.Text.ToUpper() != ReservationDTO.ReservationStatus.BLOCKED.ToString())
            //{
            //    txtCardNumber.Enabled = false;
            //    //lnkUpdate.Visible = true;
            //}

            //if (lblStatus.Text.ToUpper() == ReservationDTO.ReservationStatus.BOOKED.ToString())
            //{
            //    btnConfirm.Enabled = true;
            //    btnPayment.Enabled = false;
            //    btnEditBooking.Enabled = true;
            //    btnCancelBooking.Enabled = true;
            //    cmbPackage.Enabled = false;
            //    btnBook.Enabled = false;
            //    //btnAttendeeDetails.Enabled = true;
            //    nudQuantity.Enabled = false;
            //    cmbFromTime.Enabled = cmbToTime.Enabled = false;
            //    dgvPackageList.Enabled = false;
            //    dgvAttractionSchedules.Enabled = false;
            //    //g nudGuestCount.Enabled = false;
            //    txtGuests.Enabled = false;
            //    btnReduceGuestCount.Enabled = false;
            //    btnIncreaseGuestCount.Enabled = false;
            //    //btnChooseSchedules.Enabled = false;
            //    dgvBookingProducts.Enabled = false;
            //    btnAddProducts.Enabled = false;
            //    btnPrevConfirm.Enabled = false;
            //    cmbBookingClass.Enabled = false;
            //    dtpforDate.Enabled = false;
            //    cbxEarlyMorning.Enabled = cbxMorning.Enabled = cbxAfternoon.Enabled = cbxNight.Enabled = false;
            //    //btnExecute.Text = "Add Attendee Details";
            //    //btnExecute.Enabled = true;
            //    txtRemarks.Enabled = false;
            //    txtBookingName.Enabled = false;
            //    cmbChannel.Enabled = false;
            //    cmbFacility.Enabled = false;
            //    cmbFromTimeForSearch.Enabled = cmbToTimeForSearch.Enabled = false;
            //    btnPrint.Enabled = btnEmail.Enabled = true;
            //}
            //else if (lblStatus.Text.ToUpper() == ReservationDTO.ReservationStatus.CONFIRMED.ToString())
            //{
            //    btnConfirm.Enabled = false;
            //    btnPayment.Enabled = true;
            //    btnExecute.Enabled = true;
            //    btnCancelBooking.Enabled = true;
            //    btnEditBooking.Enabled = true;
            //    cmbPackage.Enabled = false;
            //    btnBook.Enabled = false;
            //    //btnAttendeeDetails.Enabled = false;//Commented for enabling add attendee details button on 12-Jul-2016
            //    nudQuantity.Enabled = false;
            //    cmbFromTime.Enabled = cmbToTime.Enabled = false;
            //    dgvPackageList.Enabled = false;
            //    dgvAttractionSchedules.Enabled = false;
            //    //g nudGuestCount.Enabled = false;
            //    txtGuests.Enabled = false;
            //    btnReduceGuestCount.Enabled = false;
            //    btnIncreaseGuestCount.Enabled = false;
            //    //btnChooseSchedules.Enabled = false;
            //    dgvBookingProducts.Enabled = false;
            //    btnAddProducts.Enabled = false;
            //    dtpforDate.Enabled = false;
            //    cbxEarlyMorning.Enabled = cbxMorning.Enabled = cbxAfternoon.Enabled = cbxNight.Enabled = false;
            //    txtRemarks.Enabled = false;
            //    txtBookingName.Enabled = false;
            //    cmbChannel.Enabled = false;
            //    cmbFacility.Enabled = false;
            //    cmbFromTimeForSearch.Enabled = cmbToTimeForSearch.Enabled = false;
            //    btnPrint.Enabled = btnEmail.Enabled = true;
            //}
            //else if (lblStatus.Text.ToUpper() == ReservationDTO.ReservationStatus.CANCELLED.ToString()
            //    || lblStatus.Text.ToUpper() == ReservationDTO.ReservationStatus.COMPLETE.ToString())
            //{
            //    btnConfirm.Enabled = false;
            //    btnCancelBooking.Enabled = false;
            //    cmbPackage.Enabled = false;
            //    btnPayment.Enabled = false;
            //    btnBook.Enabled = false;
            //    btnAttendeeDetails.Enabled = false;
            //    btnAddProducts.Enabled = false;
            //    btnEditBooking.Enabled = false;
            //    nudQuantity.Enabled = false;
            //    cmbFromTime.Enabled = cmbToTime.Enabled = false;
            //    dgvPackageList.Enabled = false;
            //    dgvAttractionSchedules.Enabled = false;
            //    //g nudGuestCount.Enabled = false;
            //    txtGuests.Enabled = false;
            //    btnReduceGuestCount.Enabled = false;
            //    btnIncreaseGuestCount.Enabled = false;
            //    //btnChooseSchedules.Enabled = false;//Added to disable the schedule button when the reservation is complete/cancelled
            //    dtpforDate.Enabled = false;
            //    cbxEarlyMorning.Enabled = cbxMorning.Enabled = cbxAfternoon.Enabled = cbxNight.Enabled = false;
            //    dgvBookingProducts.Enabled = false;
            //    txtRemarks.Enabled = false;
            //    dgvBookingProducts.Enabled = false;
            //    txtRemarks.Enabled = false;
            //    txtBookingName.Enabled = false;
            //    cmbChannel.Enabled = false;
            //    cmbFacility.Enabled = false;
            //    cmbFromTimeForSearch.Enabled = cmbToTimeForSearch.Enabled = false;
            //    btnPrint.Enabled = btnEmail.Enabled = true;
            //}

            //if (Reservation.Status != ReservationDTO.ReservationStatus.BOOKED.ToString()
            //    && Reservation.Status != ReservationDTO.ReservationStatus.WIP.ToString())
            //{
            //    cmbBookingClass.Enabled = false;
            //}

            //if (Reservation.Transaction != null && Reservation.Transaction.TotalPaidAmount > 0)
            //{
            //    btnBook.Enabled = false;
            //    dgvBookingProducts.Enabled = false;
            //    btnConfirm.Enabled = false;
            //    cmbPackage.Enabled = false;
            //    nudQuantity.Enabled = false;
            //    dtpforDate.Enabled = false;
            //    cmbFromTime.Enabled = cmbToTime.Enabled = false;
            //    dgvPackageList.Enabled = false;
            //    dgvAttractionSchedules.Enabled = false;
            //    //g nudGuestCount.Enabled = false;
            //    txtGuests.Enabled = false;
            //    btnReduceGuestCount.Enabled = false;
            //    btnIncreaseGuestCount.Enabled = false;
            //    //btnChooseSchedules.Enabled = false;
            //    txtBookingName.Enabled = false;
            //    cmbChannel.Enabled = false;
            //    cmbFacility.Enabled = false;
            //    cmbFromTimeForSearch.Enabled = cmbToTimeForSearch.Enabled = false;
            //}
            ////Added to enable the controls to modify reservation on May 18, 2015//
            //if (isEditedBooking)
            //{
            //    btnBook.Enabled = true;
            //    dgvBookingProducts.Enabled = true;
            //    //btnConfirm.Enabled = true;
            //    cmbPackage.Enabled = true;
            //    nudQuantity.Enabled = true;
            //    cmbFromTime.Enabled = cmbToTime.Enabled = true;
            //    if (attractionScheduleId != -1)
            //    {
            //        SchedulesBL schedulesBL = new SchedulesBL(Utilities.ExecutionContext, attractionScheduleId);
            //        if(schedulesBL != null && schedulesBL.ScheduleDTO != null && schedulesBL.ScheduleDTO.FixedSchedule == true)
            //        {
            //            cmbFromTime.Enabled = cmbToTime.Enabled = false;
            //        }
            //    } 
            //    cmbBookingClass.Enabled = true;
            //    dgvPackageList.Enabled = true;
            //    dgvAttractionSchedules.Enabled = true;
            //    btnDiscounts.Enabled = true;
            //    btnTrxProfiles.Enabled = true;
            //    btnAddAttendeesInSummary.Enabled = true;
            //    pnlCustomerDetail.Enabled = true;
            //    //g nudGuestCount.Enabled = true;
            //    txtGuests.Enabled = true;
            //    btnReduceGuestCount.Enabled = true;
            //    btnIncreaseGuestCount.Enabled = true;
            //    //btnChooseSchedules.Enabled = true;
            //    txtBookingName.Enabled = true;
            //    cmbChannel.Enabled = true;
            //    cmbFacility.Enabled = true;
            //    cmbFromTimeForSearch.Enabled = cmbToTimeForSearch.Enabled = true;
            //    txtCardNumber.Enabled = true;
            //    dtpforDate.Enabled = true;
            //}
            log.LogMethodExit();
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            //log.LogVariableState("cmbBookingClass.SelectedValue", cmbBookingClass.SelectedValue);
            //txtMessage.Clear();
            //if (cmbBookingClass != null && cmbBookingClass.SelectedValue != null && Convert.ToInt32(cmbBookingClass.SelectedValue) > -1)
            //{
            //    DataGridView dgvResources = new DataGridView
            //    {
            //        Name = "dgvResources",
            //        Width = 900,
            //        Height = 300
            //    };

            //    double lclTime = Convert.ToDouble(cmbFromTime.SelectedValue);
            //    DateTime ReservationFromDateTime = dtpforDate.Value.Date.AddMinutes(Math.Floor(lclTime) * 60).AddMinutes((lclTime - Math.Floor(lclTime)) * 100.0);
            //    //Added to send facility Id as parameter to get check availablity on Dec-07-2015//
            //    dgvResources.DataSource = Reservation.getResourceAvailability(ReservationFromDateTime, ReservationFromDateTime, null, null, Convert.ToInt32(cmbBookingClass.SelectedValue), facilityId);
            //    dgvResources.AllowUserToDeleteRows = dgvResources.AllowUserToAddRows = false;
            //    dgvResources.ReadOnly = true;

            //    Button btnCls = new Button()
            //    {
            //        FlatStyle = FlatStyle.Flat,
            //        BackColor = Color.Transparent,
            //        BackgroundImage = Properties.Resources.normal2,
            //        BackgroundImageLayout = ImageLayout.Stretch,
            //        ForeColor = Color.White
            //    };
            //    btnCls.FlatAppearance.MouseOverBackColor = btnCls.FlatAppearance.MouseDownBackColor = Color.Transparent;
            //    btnCls.FlatAppearance.BorderSize = 0;

            //    using (Form fAvail = new Form())
            //    {
            //        fAvail.Width = dgvResources.Width + 5;
            //        fAvail.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            //        btnCls.Text = MessageContainer.GetMessage(Utilities.ExecutionContext, "Close");
            //        btnCls.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            //        fAvail.CancelButton = btnCls;
            //        fAvail.ShowInTaskbar = false;
            //        fAvail.Height = dgvResources.Height + 80;
            //        btnCls.Location = new Point(fAvail.Width / 2 - btnCls.Width / 2, dgvResources.Height + 10);
            //        fAvail.Controls.Add(dgvResources);
            //        fAvail.Controls.Add(btnCls);
            //        fAvail.StartPosition = FormStartPosition.CenterScreen;

            //        fAvail.Load += fAvail_Load;

            //        fAvail.ShowDialog();
            //    }
            //}
            log.LogMethodExit();
        }
        ////Begin:get the attraction schedule id selected if Bookingid !=-1
        //private void GetAttractionScheduleSelcted(int bookingproductId, DateTime reservationDate)
        //{
        //    log.LogMethodEntry(bookingproductId, reservationDate);
        //    object o = Utilities.executeScalar(@"SELECT AttractionScheduleId, FacilityId
        //                                                                   FROM AttractionSchedules
        //                                                                   INNER JOIN products p ON p.product_id = @BookingProductId
        //                                                                   AND p.AttractionMasterScheduleId = AttractionSchedules.AttractionMasterScheduleId
        //                                                                   WHERE @reservation >= DATEADD([mi], CONVERT(int, ScheduleTime) * 60 + ScheduleTime % 1 * 110, @fromDate) 
        //                                                                   AND  @reservation < DATEADD([mi], CONVERT(int, isnull(ScheduleToTime,23.59)) * 60 + isnull(ScheduleToTime,23.59) % 1 * 110, @fromDate)",
        //                                                                    new SqlParameter("@reservation", reservationDate),
        //                                                                    new SqlParameter("@BookingProductId", bookingproductId),
        //                                                                    new SqlParameter("@fromDate", reservationDate.Date));

        //    attractionScheduleId = Convert.ToInt32(o);
        //    log.LogMethodExit(attractionScheduleId);
        //}
        ////End


        void fAvail_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            DataGridView dgvResources = (sender as Form).Controls["dgvResources"] as DataGridView;
            Utilities.setupDataGridProperties(ref dgvResources);
            dgvResources.Columns["TimeFrom"].DefaultCellStyle =
            dgvResources.Columns["TimeTo"].DefaultCellStyle = POSStatic.Utilities.gridViewDateCellStyle();
            dgvResources.Columns["TimeTo"].DefaultCellStyle.Format =
            dgvResources.Columns["TimeFrom"].DefaultCellStyle.Format = "h:mm tt";
            log.LogMethodExit();
        }

        private void FrmMakeReservation_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (BookingId != -1)
            {
                GetBookingDetails();
                //Added to Get the audit trail Details and load it to audit trail grid // 
                GetAuditTrail(BookingId);
                if (lblStatus.Text.ToUpper() != ReservationDTO.ReservationStatus.BLOCKED.ToString())
                {
                    btnBook.Text = MessageContainer.GetMessage(Utilities.ExecutionContext, "Save");
                }
            }
            else
            {
                if (cmbBookingClass.Items.Count > 0)
                {
                    cmbBookingClass.SelectedIndex = 0;
                }
                else
                {
                    POSUtils.ParafaitMessageBox(MessageContainer.GetMessage(Utilities.ExecutionContext, 1788)); //"No Booking products are available or defined"
                }
            }
            LoadReservationDTOForBlockedSchedule();
            UpdateUIStatus();
            Utilities.setLanguage(this);
            if (lblStatus.Text.ToUpper() == ReservationDTO.ReservationStatus.BOOKED.ToString()
                || lblStatus.Text.ToUpper() == ReservationDTO.ReservationStatus.CANCELLED.ToString()
                || lblStatus.Text.ToUpper() == ReservationDTO.ReservationStatus.CONFIRMED.ToString()
                || lblStatus.Text.ToUpper() == ReservationDTO.ReservationStatus.COMPLETE.ToString())
            {
                tcBooking.SelectedTab = tpConfirm;
            }
            log.LogMethodExit();
        }

        private void LoadReservationDTOForBlockedSchedule()
        {
            log.LogMethodEntry();
            if (lblStatus.Text.ToUpper() == ReservationDTO.ReservationStatus.BLOCKED.ToString())
            {
                if (Reservation != null)
                {
                  //  reservationDTO = Reservation.GetReservationDTO;
                }
            }
            log.LogMethodExit();
        }

        //Begin: This method is used to get the  booked additional products
        private void GetAdditionalBookingProducts()
        {
            log.LogMethodEntry();
            //dgvBookingProducts.Rows.Clear();
            //additionalProductInformationLst.Clear();
            //try
            //{
            //    if (Reservation != null)
            //    {
            //        DataTable dtAdditionalProducts = Reservation.GetSelectedAdditionalBookingProducts(BookingId);
            //        foreach (DataRow dr in dtAdditionalProducts.Rows)
            //        {
            //            //GetDiscounts(Convert.ToInt32(dr["product_id"]), i);
            //            //  dgvBookingProducts.Rows.Add(delete btn , productId, qty, price, prod type, bookingProdId, discountName, trxprofId, remarks
            //            dgvBookingProducts.Rows.Add(null, dr["product_id"], dr["quantity"], dr["price"], dr["product_type"], dr["BookingProductId"], dr["discountId"], dr["TrxProfileId"], dr["Remarks"], dr["LineId"]);
            //        }
            //        GetServiceChargeProductDetails();
            //        BuildProducInformationLst();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    log.Error(ex);
            //}
            log.LogMethodExit();
        }

        private void BuildProducInformationLst()
        {
            log.LogMethodEntry();
            //List<Semnox.Parafait.Transaction.Reservation.clsProductList> adInfoList = new List<Semnox.Parafait.Transaction.Reservation.clsProductList>();
            //foreach (DataGridViewRow dr in dgvBookingProducts.Rows)
            //{
            //    if (dr.IsNewRow || dr.Cells["dcProduct"].Value == null)
            //    {
            //        break;
            //    }
            //    if (dr.Cells["dcProduct"].Value != null && dr.Cells["type"].Value != null && dr.Cells["type"].Value.ToString() == "SERVICECHARGE")
            //    {
            //        continue;
            //    }
            //    Semnox.Parafait.Transaction.Reservation.clsProductList apInfo = new Semnox.Parafait.Transaction.Reservation.clsProductList();
            //    apInfo.ProductId = Convert.ToInt32(dr.Cells["dcProduct"].Value);
            //    apInfo.Quantity = Convert.ToInt32(dr.Cells["dcQuantity"].Value);
            //    apInfo.Price = Convert.ToDouble(dr.Cells["dcPrice"].Value);
            //    apInfo.productType = dr.Cells["type"].Value.ToString();
            //    apInfo.discountId = (dr.Cells["AdditionalDiscountName"].Value != DBNull.Value ? Convert.ToInt32(dr.Cells["AdditionalDiscountName"].Value) : -1);
            //    apInfo.transactionProfileId = (dr.Cells["additionalTransactionProfileId"].Value != DBNull.Value ? Convert.ToInt32(dr.Cells["additionalTransactionProfileId"].Value) : -1);
            //    apInfo.Remarks = dr.Cells["dcRemarks"].Value;
            //    apInfo.lineId = Convert.ToInt32(dr.Cells["LineId"].Value);
            //    Semnox.Parafait.Transaction.Reservation.clsProductList existingEntry = additionalProductInformationLst.Find(entry => entry.ProductId == apInfo.ProductId && entry.lineId == apInfo.lineId);
            //    if (BookingId != -1 && lblStatus.Text.ToUpper() != ReservationDTO.ReservationStatus.BLOCKED.ToString())
            //    {
            //        if (existingEntry != null)
            //        {
            //            apInfo.purchasedModifierLst = existingEntry.purchasedModifierLst;
            //            apInfo.attractionProductLst = existingEntry.attractionProductLst;
            //        }
            //        if (apInfo.purchasedModifierLst == null || apInfo.purchasedModifierLst.Count == 0)
            //        {
            //            apInfo.purchasedModifierLst = Reservation.GeneratePurchasedProductlist(BookingId, false, apInfo.ProductId, apInfo.lineId);
            //        }
            //        if (apInfo.attractionProductLst == null || apInfo.attractionProductLst.Count == 0)
            //        {
            //            apInfo.attractionProductLst = Reservation.GenerateAttractionProductlist(false, BookingId, apInfo.ProductId, apInfo.lineId);
            //        }
            //    }
            //    else
            //    {
            //        if (existingEntry != null)
            //        {
            //            apInfo.purchasedModifierLst = existingEntry.purchasedModifierLst;
            //            apInfo.attractionProductLst = existingEntry.attractionProductLst;
            //        }
            //    }
            //    adInfoList.Add(apInfo);
            //}
            //additionalProductInformationLst = adInfoList;
            log.LogMethodExit();
        }


        private void BuildModifierLstFromProductInformationLst()
        {
            log.LogMethodEntry();
            lstPurchasedAdditionalModifierProducts.Clear();
            foreach (Semnox.Parafait.Transaction.Reservation.clsProductList item in additionalProductInformationLst)
            {
                if (item.purchasedModifierLst != null && item.purchasedModifierLst.Count > 0)
                {
                    foreach (KeyValuePair<PurchasedProducts, int> purchasedItem in item.purchasedModifierLst)
                    {
                        lstPurchasedAdditionalModifierProducts.Add(purchasedItem);
                    }
                }
            }
            log.LogMethodExit();
        }
        private void BuildAttractionSchedulesLstFromProductInformationLst()
        {
            log.LogMethodEntry();
            lstAdditionalAttractionProductsList.Clear();
            foreach (Semnox.Parafait.Transaction.Reservation.clsProductList item in additionalProductInformationLst)
            {
                if (item.attractionProductLst != null && item.attractionProductLst.Count > 0)
                {
                    foreach (KeyValuePair<Semnox.Parafait.Transaction.AttractionBooking, int> purchasedATS in item.attractionProductLst)
                    {
                        lstAdditionalAttractionProductsList.Add(purchasedATS);
                    }
                }
            }
            log.LogMethodExit();
        }
        private List<Semnox.Parafait.Transaction.Reservation.clsProductList> GetProducInformationLst(int productId, int lineId)
        {
            log.LogMethodEntry(productId, lineId);
            List<Semnox.Parafait.Transaction.Reservation.clsProductList> adInfoLst = additionalProductInformationLst.FindAll(adInfo => adInfo.ProductId == productId && adInfo.lineId == lineId);
            log.LogMethodExit(adInfoLst);
            return adInfoLst;
        }

        private void UpdateProducInformationLst(Semnox.Parafait.Transaction.Reservation.clsProductList clsProductList)
        {
            log.LogMethodEntry(clsProductList);
            bool newEntry = true;
            for (int i = 0; i < additionalProductInformationLst.Count; i++)
            {
                if (additionalProductInformationLst[i].ProductId == clsProductList.ProductId && additionalProductInformationLst[i].lineId == clsProductList.lineId)
                {
                    additionalProductInformationLst.RemoveAt(i);
                    additionalProductInformationLst.Insert(i, clsProductList);
                    newEntry = false;
                }
            }
            if (newEntry)
            {
                additionalProductInformationLst.Add(clsProductList);
            }
            log.LogMethodExit();
        }

        private void GetServiceChargeProductDetails()
        {
            log.LogMethodEntry();
            //if (Reservation != null)
            //{
            //    DataTable dtServiceChargeProduct = Reservation.GetServiceChargeProductDetails(BookingId);
            //    foreach (DataRow dr in dtServiceChargeProduct.Rows)
            //    {
            //        //GetDiscounts(Convert.ToInt32(dr["product_id"]), i);
            //        //  dgvBookingProducts.Rows.Add(delete btn , productId, qty, price, prod type, bookingProdId, discountName, trxprofId, remarks
            //        dgvBookingProducts.Rows.Add(null, dr["product_id"], dr["quantity"], dr["price"], dr["product_type"], dr["BookingProductId"], dr["discountId"], dr["TrxProfileId"], dr["Remarks"], dr["LineId"]);//, dr["discountId"]
            //    }
            //}
            log.LogMethodExit();
        }

        //end

        //Begin :Added to get the Products that are part of the booking product selected//
        private void GetBookingProductsDefinition()
        {
            log.LogMethodEntry();
            try
            {
                //if (cmbBookingClass.SelectedIndex >= 0)
                //{
                //    int loadedBookingProductId = GetLoadedBookingProductIdFromDGV();
                //    int selectedBookingProductId = Convert.ToInt32(cmbBookingClass.SelectedValue);
                //    int selectedQty = string.IsNullOrEmpty(txtGuests.Text) == true ? -1 : Convert.ToInt32(txtGuests.Text);
                //    if ((loadedBookingProductId != selectedBookingProductId) || (selectedQty != guestCount))
                //    {
                //        dtPackageList = Reservation.GetBookingProductContents(selectedBookingProductId);

                //        BindingSource bs = new BindingSource();
                //        bs.DataSource = dtPackageList;
                //        dgvPackageList.DataSource = bs;
                //        foreach (DataGridViewRow dr in dgvPackageList.Rows)
                //        {
                //            dr.Cells["SelectedStatus"].Value = "false";
                //            if (dr.Cells["ChildId"].Value != null && (lblStatus.Text.ToUpper() == ReservationDTO.ReservationStatus.NEW.ToString() 
                //                || lblStatus.Text.ToUpper() == ReservationDTO.ReservationStatus.BLOCKED.ToString()))
                //            {
                //                DataGridViewCheckBoxCell chkPackageproductId = (DataGridViewCheckBoxCell)dr.Cells["SelectProduct"];
                //                chkPackageproductId.Value = "Y";
                //                dr.Cells["SelectedStatus"].Value = "true";
                //            }
                //            dr.Cells["discountName"].Value = -1;
                //            dr.Cells["transactionProfileId"].Value = -1;
                //        }
                //        dgvPackageList.Columns["ParentId"].Visible = false;
                //        dgvPackageList.Columns["ChildId"].Visible = false;
                //        dgvPackageList.Columns["PriceInclusive"].Visible = false;
                //        dgvPackageList.Columns["CategoryId"].Visible =
                //        dgvPackageList.Columns["ProductType"].Visible =
                //        dgvPackageList.Columns["price"].Visible =
                //        dgvPackageList.Columns["ProductImage"].Visible = false;
                //        dgvPackageList.Columns["Quantity"].DisplayIndex = 3;
                //        Utilities.setupDataGridProperties(ref dgvPackageList);
                //        dgvPackageList.Columns["Quantity"].DefaultCellStyle = Utilities.gridViewNumericCellStyle();
                //        dgvPackageList.BorderStyle = BorderStyle.FixedSingle;
                //        SetDGVCellFont(dgvPackageList);

                //        for (int productRow = 0; productRow < dtPackageList.Rows.Count; productRow++)
                //        {
                //            if (Convert.ToInt32(dtPackageList.Rows[productRow]["Quantity"].ToString()) <= 0)
                //            {
                //                dgvPackageList.Rows[productRow].Cells["Quantity"].Value = txtGuests.Text;//g nudGuestCount.Value;
                //                                                                                         // dgvPackageList.Rows[productRow].Cells["Quantity"].ReadOnly = false;//Commented on Dec-07-2015 to remove quantity field from bieng read only
                //                if (selectedQty != guestCount && dgvPackageList.Rows[productRow].Cells["ChildId"] != null
                //                    && (lblStatus.Text.ToUpper() == ReservationDTO.ReservationStatus.NEW.ToString() 
                //                    || lblStatus.Text.ToUpper() == ReservationDTO.ReservationStatus.BLOCKED.ToString()))
                //                {
                //                    RemovePackageFromList(Convert.ToInt32(dgvPackageList.Rows[productRow].Cells["ChildId"].Value));
                //                }
                //            }

                //        }
                //        guestCount = selectedQty;
                //    }
                //}
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

            log.LogMethodExit();
        }

        private int GetLoadedGuestQtyFromDGV()
        {
            log.LogMethodEntry();
            int loadedQty = -999999;
            if (dgvPackageList.Rows.Count > 0)
            {
                try
                {
                    foreach (DataGridViewRow item in dgvPackageList.Rows)
                    {
                        if (item.Cells["Quantity"] != null && item.Cells["Quantity"].Value != DBNull.Value
                            && item.Cells["SelectedStatus"].Value != DBNull.Value && item.Cells["SelectedStatus"].Value.ToString() == "true")
                        {
                            loadedQty = Convert.ToInt32(item.Cells["Quantity"].Value);
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
            }
            log.LogMethodExit(loadedQty);
            return loadedQty;
        }

        private int GetLoadedBookingProductIdFromDGV()
        {
            log.LogMethodEntry();
            int loadedBookingProduct = -999999;
            if (dgvPackageList.Rows.Count > 0)
            {
                try
                {
                    foreach (DataGridViewRow item in dgvPackageList.Rows)
                    {
                        if (item.Cells["ParentId"] != null && item.Cells["ParentId"].Value != DBNull.Value && item.Cells["SelectedStatus"].Value != null)
                        {
                            loadedBookingProduct = Convert.ToInt32(item.Cells["ParentId"].Value);
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
            }
            log.LogMethodExit(loadedBookingProduct);
            return loadedBookingProduct;
        }

        //end//

        //Begin: Get the contents of the booking product selected
        private void GetBookingPackage()
        {
            log.LogMethodEntry();
            try
            {
                //if (cmbBookingClass.SelectedIndex >= 0)
                //{
                //    GetBookingProductsDefinition();//Added to get the booking products, product list (packages and  individual items)
                //    dtProducts = Reservation.GetAdditionalProducts(Convert.ToInt32(cmbBookingClass.SelectedValue));// Get the additional Products
                //    DataTable dtServiceChargeProduct = Reservation.GetServiceChargeProduct(Convert.ToInt32(cmbBookingClass.SelectedValue));
                //    dtProducts.ImportRow(dtServiceChargeProduct.Rows[0]);
                //    dcProduct.DataSource = dtProducts;
                //    dcProduct.DisplayMember = "product_name";
                //    dcProduct.ValueMember = "ChildId";

                //    List<DiscountsDTO> discountDTOList;
                //    DiscountsListBL discountsListBL = new DiscountsListBL();
                //    List<KeyValuePair<DiscountsDTO.SearchByParameters, string>> searchDiscountsParams = new List<KeyValuePair<DiscountsDTO.SearchByParameters, string>>();
                //    searchDiscountsParams.Add(new KeyValuePair<DiscountsDTO.SearchByParameters, string>(DiscountsDTO.SearchByParameters.ACTIVE_FLAG, "Y"));
                //    searchDiscountsParams.Add(new KeyValuePair<DiscountsDTO.SearchByParameters, string>(DiscountsDTO.SearchByParameters.SITE_ID, (Utilities.ParafaitEnv.IsCorporate ? Utilities.ParafaitEnv.SiteId : -1).ToString()));
                //    discountDTOList = discountsListBL.GetDiscountsDTOList(searchDiscountsParams);
                //    if (discountDTOList == null)
                //    {
                //        discountDTOList = new List<DiscountsDTO>();
                //    }
                //    discountDTOList.Insert(0, new DiscountsDTO());
                //    discountDTOList[0].DiscountName = "<SELECT>";

                //    discountName.DataSource = AdditionalDiscountName.DataSource = discountDTOList;
                //    discountName.DisplayMember = AdditionalDiscountName.DisplayMember = "DiscountName";
                //    discountName.ValueMember = AdditionalDiscountName.ValueMember = "DiscountId";
                //    if (Utilities.getParafaitDefaults("ENABLE_DISCOUNTS_IN_POS") != "Y")
                //    {
                //        discountName.ReadOnly = true;
                //        AdditionalDiscountName.ReadOnly = true;
                //    }
                //    if (BookingId == -1)
                //    {
                //        Utilities.setupDataGridProperties(ref dgvPackageList);
                //    }
                //    if (BookingId != -1)
                //    {
                //        GetBookedBookingProductItems();
                //    }
                //}
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        //End//

        private void GetPackageContents(bool allowGet = false)
        {
            log.LogMethodEntry();
            //DataTable dt = Reservation.GetPackageContents(packageIdSelected);
            //dgvPackageDetails.DataSource = dt;

            //Utilities.setupDataGridProperties(ref dgvPackageDetails);
            //dgvPackageDetails.Columns["Quantity"].DefaultCellStyle = Utilities.gridViewNumericCellStyle();
            //dgvPackageDetails.Columns["CategoryId"].Visible = false;
            //dgvPackageDetails.BorderStyle = BorderStyle.FixedSingle;
            //dgvPackageDetails.ScrollBars = ScrollBars.Both;
            //dgvPackageDetails.Columns["product_name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            //SetDGVCellFont(dgvPackageDetails);

            ////Begin: Add category products if required //
            //if (productType == "COMBO")
            //{
            //    CategoryProductList = new List<KeyValuePair<int, int>>();
            //    CategoryProductSelectedList = new List<Semnox.Parafait.Transaction.Reservation.clsSelectedCategoryProducts>();

            //    cardList = new List<string>();
            //    if (productQuantity > 0)
            //    {
            //        //Begin Modification - Jan- 20- 2016- Added to Get the Attraction Products//
            //        DataTable dtAttractionChild = Utilities.executeDataTable(@"select ChildProductId, cp.Quantity, cp.id
            //                                                             from ComboProduct cp, products p, product_type pt
            //                                                             where cp.Product_id = @productId
            //                                                             and p.product_id = ChildProductId
            //                                                             and p.product_type_id = pt.product_type_id
            //                                                             and cp.Quantity > 0
            //                                                             and pt.product_type = 'ATTRACTION'",
            //                                                                    new SqlParameter("@productId", packageIdSelected));
            //        int ComboQuantity = productQuantity;
            //        if (dtAttractionChild.Rows.Count > 0)
            //        {
            //            ComboQuantity = (int)productQuantity;
            //            productQuantity = 0;
            //        }
            //        //End Modification- Jan- 20- 2016- Added to Get the Attraction Products//
            //        DataTable dtCategory = Utilities.executeDataTable(@"select p.CategoryId, cp.Quantity, p.Name 
            //                                                             from ComboProduct cp, Category p
            //                                                             where cp.Product_id = @productId
            //                                                             and p.CategoryId = cp.CategoryId
            //                                                             and cp.Quantity > 0",
            //                                                         new SqlParameter("@productId", packageIdSelected));

            //        //if (BookingId == -1 || isEditedBooking || lblStatus.Text == "BLOCKED")
            //        if (BookingId == -1 || lblStatus.Text.ToUpper() == ReservationDTO.ReservationStatus.BLOCKED.ToString() || allowGet == true)
            //        {
            //            if (dtCategory.Rows.Count > 0)
            //            {
            //                foreach (DataRow dr in dtCategory.Rows)
            //                {
            //                    //Open the form to Add products from the category product list
            //                    using (frmProductList fpl = new frmProductList(dr["name"], dr["CategoryId"], ComboQuantity * ((int)dr["Quantity"]), packageIdSelected, BookingId, CategoryProuctFinalSelectedList, isEditedBooking))
            //                    {
            //                        if (fpl.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //                        {
            //                            if (fpl.SelectedProductList.Count > 0)
            //                            {
            //                                CategoryProductList.AddRange(fpl.SelectedProductList);
            //                                CategoryProuctFinalList.AddRange(CategoryProductList);
            //                            }
            //                        }
            //                        else
            //                        {
            //                            log.LogMethodExit("Dialog was cancelled");
            //                            return;
            //                        }
            //                    }
            //                    //End//
            //                }
            //            }
            //            //Begin Modification - Jan-20-2016-Attraction product within combo//
            //            if (dtAttractionChild.Rows.Count > 0)
            //            {
            //                int lclChildProduct = -1;
            //                excludeAttractionSchedule = "-1";
            //                bool alreadyAdded = PresentInAttractionProductList(packageIdSelected);
            //                if (alreadyAdded == false || allowGet == true)
            //                {
            //                    foreach (DataRow dr in dtAttractionChild.Rows)
            //                    {
            //                        bool excludeSchedule = false;
            //                        if (lclChildProduct == Convert.ToInt32(dr["ChildProductId"]))
            //                            excludeSchedule = true;
            //                        lclChildProduct = Convert.ToInt32(dr["ChildProductId"]);

            //                        if (CreateAttractionProduct(Convert.ToInt32(dr["Id"]), lclChildProduct, 0, ComboQuantity * Convert.ToInt32(dr["Quantity"]), packageIdSelected, lstAttractionProductslist, excludeSchedule) == false)
            //                        {
            //                            throw new Exception(Utilities.MessageUtils.getMessage("Attraction schedule product not created"));
            //                        }
            //                    }
            //                }
            //            }
            //            //End Modification - Jan-20-2016-Attraction product within combo//
            //        }
            //    }
            //    else
            //        POSUtils.ParafaitMessageBox(MessageUtils.getMessage(944));

            //    log.LogMethodExit("Selected Combo Product has quantity 0.");
            //    return;
            //}
            //end: Adding Category products//
            log.LogMethodExit();
        }

        //Get the Booking audit details
        private void GetAuditTrail(int bookingId)
        {
            log.LogMethodEntry(bookingId);
            DataTable auditTrail = Utilities.executeDataTable(@"select Data,Timestamp,
                                                                Computer, Description from EventLog
                                                                where Source ='Reservation'
                                                                and value = @bookingId
                                                                order by EventLogId desc",
                                                               new SqlParameter("@bookingId", bookingId.ToString()));
            dgvAuditTrail.DataSource = auditTrail;
            Utilities.setupDataGridProperties(ref dgvAuditTrail);
            dgvAuditTrail.BorderStyle = BorderStyle.FixedSingle;
            dgvAuditTrail.Columns["Description"].Width = 500;
            dgvAuditTrail.Columns["Description"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvAuditTrail.Columns["Data"].HeaderText = "UserName";
            SetDGVCellFont(dgvAuditTrail);
            log.LogMethodExit();
        }

        private void btnBook_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            txtMessage.Clear();
            if (attractionScheduleId == -1)
            {
                log.LogMethodExit("AttractionScheduleId == -1");
                return;
            }
            if (Save() == true)
            {
                isEditedBooking = false;
            };
            UpdateUIStatus();
            log.LogMethodExit();
        }

        private bool ValidateCustomer()
        {
            log.LogMethodEntry();
            bool valid = true;
            List<ValidationError> validationErrorList = customerDetailUI.UpdateCustomerDTO();
            CustomerBL customerBL = new CustomerBL(Utilities.ExecutionContext, customerDetailUI.CustomerDTO);
            validationErrorList.AddRange(customerBL.Validate());
            if (string.IsNullOrWhiteSpace(customerDetailUI.CustomerDTO.FirstName))
            {
                ValidationError validationError = new ValidationError(MessageContainer.GetMessage(Utilities.ExecutionContext, "Customer"), MessageContainer.GetMessage(Utilities.ExecutionContext, "First Name"), MessageUtils.getMessage(303));
                validationErrorList.Add(validationError);
            }
            if (validationErrorList.Count > 0)
            {
                customerDetailUI.ShowValidationError(validationErrorList);
                tcBooking.SelectedTab = tpCustomer;
                txtMessage.Text = validationErrorList[0].Message;
                valid = false;
            }
            log.LogMethodExit(valid);
            return valid;
        }

        //Begin: Modified the method to implement the new Booking Related Changes//
        private bool Save()
        {
            log.LogMethodEntry();
            bool status = false;
            return status;
            //audit = new Semnox.Core.Utilities.EventLog(Utilities);
            //ParafaitEnv = POSStatic.ParafaitEnv;
            //txtMessage.Text = "";
            //if (ValidateCustomer() == false)
            //{
            //    log.LogMethodExit("ValidateCustomer() == false");
            //    return false;
            //}

            //if (txtBookingName.Text.Trim() == "")
            //{
            //    txtMessage.Text = MessageUtils.getMessage(302);
            //    this.ActiveControl = txtBookingName;
            //    tcBooking.SelectedTab = tpDateTime;
            //    log.LogMethodExit("Booking Name was not entered");
            //    return false;
            //}

            //if (string.IsNullOrWhiteSpace(customerDetailUI.CustomerDTO.FirstName))
            //{
            //    txtMessage.Text = MessageUtils.getMessage(303);
            //    tcBooking.SelectedTab = tpCustomer;
            //    log.LogMethodExit("Customer Name was not entered");
            //    return false;
            //}

            //if (cmbFromTime.SelectedValue == null)
            //{
            //    txtMessage.Text = MessageUtils.getMessage(1820); // "Please enter From Time for the booking schedule"
            //    tcBooking.SelectedTab = tpDateTime;
            //    log.LogMethodExit("From Time is not set");
            //    return false;
            //}

            //if (cmbToTime.SelectedValue == null)
            //{
            //    txtMessage.Text = MessageUtils.getMessage(1821); // "Please enter To Time for the booking schedule"
            //    tcBooking.SelectedTab = tpDateTime;
            //    log.LogMethodExit("To Time is not set");
            //    return false;
            //}

            //decimal toTime = Convert.ToDecimal(cmbToTime.SelectedValue);
            //decimal fromTime = Convert.ToDecimal(cmbFromTime.SelectedValue);
            //if (toTime < 6 && fromTime > 6)
            //{
            //}
            //else if (toTime <= fromTime)
            //{
            //    txtMessage.Text = MessageUtils.getMessage(305);
            //    this.ActiveControl = cmbToTime;
            //    tcBooking.SelectedTab = tpDateTime;
            //    log.LogMethodExit("To Time should be greater than From Time");
            //    return false;
            //}

            //if (cmbChannel.SelectedItem == null || cmbChannel.SelectedIndex == -1)
            //{
            //    txtMessage.Text = MessageUtils.getMessage(306);
            //    this.ActiveControl = cmbChannel;
            //    tcBooking.SelectedTab = tpDateTime;
            //    log.LogMethodExit("Channel was not selected");
            //    return false;
            //}

            //if (cmbBookingClass.SelectedValue == null)
            //{
            //    txtMessage.Text = MessageUtils.getMessage(1796);
            //    this.ActiveControl = cmbBookingClass;
            //    tcBooking.SelectedTab = tpDateTime;
            //    log.LogMethodExit("Booking product is not selected");
            //    return false;
            //}

            //if (cmbFacility.SelectedValue == null)
            //{
            //    txtMessage.Text = MessageUtils.getMessage(695);
            //    this.ActiveControl = cmbFacility;
            //    tcBooking.SelectedTab = tpDateTime;
            //    log.LogMethodExit("Facility is not selected");
            //    return false;
            //}

            ////End Modification Dec-24-2015//

            //string message = "";

            //try
            //{
            //    int quantity = 0;
            //    bool isNew = false;
            //    bool selected = false;
            //    if (BookingId == -1 || lblStatus.Text.ToUpper() == ReservationDTO.ReservationStatus.BLOCKED.ToString())
            //    {
            //        isNew = true;
            //    }
            //    double lclTime = Convert.ToDouble(cmbFromTime.SelectedValue);
            //    DateTime fromDateTime = dtpforDate.Value.Date.AddMinutes(Math.Floor(lclTime) * 60).AddMinutes((lclTime - Math.Floor(lclTime)) * 100.0);

            //    double tolclTime = Convert.ToDouble(cmbToTime.SelectedValue);

            //    DateTime toDateTime;
            //    if (tolclTime > lclTime)
            //        toDateTime = dtpforDate.Value.Date.AddMinutes(Math.Floor(tolclTime) * 60).AddMinutes((tolclTime - Math.Floor(tolclTime)) * 100.0);
            //    else
            //        toDateTime = dtpforDate.Value.Date.AddDays(1).AddMinutes(Math.Floor(tolclTime) * 60).AddMinutes((tolclTime - Math.Floor(tolclTime)) * 100.0);

            //    int Duration = (int)(toDateTime - fromDateTime).TotalMinutes;

            //    List<Semnox.Parafait.Transaction.Reservation.clsProductList> lstProducts = new List<Semnox.Parafait.Transaction.Reservation.clsProductList>();
            //    foreach (DataGridViewRow dr in dgvBookingProducts.Rows)
            //    {
            //        if (dr.Cells["dcProduct"].Value != null)
            //        {
            //            Semnox.Parafait.Transaction.Reservation.clsProductList cp = new Semnox.Parafait.Transaction.Reservation.clsProductList();
            //            cp.ProductId = Convert.ToInt32(dr.Cells["dcProduct"].Value);
            //            cp.Quantity = dr.Cells["dcQuantity"].Value;
            //            cp.Price = dr.Cells["dcPrice"].Value == DBNull.Value ? -1 : dr.Cells["dcPrice"].Value;
            //            cp.Remarks = dr.Cells["dcRemarks"].Value;
            //            cp.Id = dr.Cells["dcBookingProductId"].Value;
            //            cp.productType = dr.Cells["type"].Value;
            //            if (dr.Cells["AdditionalDiscountName"].Value == DBNull.Value)
            //                cp.discountId = -1;
            //            else if (Convert.ToInt32(dr.Cells["AdditionalDiscountName"].Value) < 0)
            //                cp.discountId = -1;
            //            else
            //                cp.discountId = Convert.ToInt32(dr.Cells["AdditionalDiscountName"].Value);

            //            if (dr.Cells["additionalTransactionProfileId"].Value == DBNull.Value)
            //                cp.transactionProfileId = -1;
            //            else if (Convert.ToInt32(dr.Cells["additionalTransactionProfileId"].Value) < 0)
            //                cp.transactionProfileId = -1;
            //            else
            //                cp.transactionProfileId = Convert.ToInt32(dr.Cells["additionalTransactionProfileId"].Value);

            //            lstProducts.Add(cp);
            //        }
            //    }
            //    /*Added to enable multiple product selection for reservation on June 6, 2015*/
            //    if (lstPacakgeIdList == null)
            //    {
            //        lstPacakgeIdList = new List<Semnox.Parafait.Transaction.Reservation.clsPackageList>();
            //        foreach (DataGridViewRow packageRow in dgvPackageList.Rows)
            //        {
            //            if (packageRow.Cells["packageName"].Value != DBNull.Value)
            //            {
            //                if ((packageRow.Cells["packageName"].Value != null) && packageRow.Cells["SelectProduct"].Value != null && (packageRow.Cells["Quantity"].Value != null) && packageRow.Cells["SelectProduct"].Value.ToString() == "Y")
            //                {
            //                    packageIdList = new Semnox.Parafait.Transaction.Reservation.clsPackageList();
            //                    string packageName = packageRow.Cells["packageName"].FormattedValue.ToString();
            //                    packageIdList.productId = Convert.ToInt32(packageRow.Cells["ChildId"].Value);
            //                    packageIdList.bookingProductId = Convert.ToInt32(cmbBookingClass.SelectedValue);
            //                    packageIdList.guestQuantity = Convert.ToInt32(packageRow.Cells["Quantity"].Value);
            //                    if (packageRow.Cells["discountName"].Value == DBNull.Value)
            //                        packageIdList.discountId = -1;
            //                    else if (Convert.ToInt32(packageRow.Cells["discountName"].Value) < 0)
            //                        packageIdList.discountId = -1;
            //                    else
            //                        packageIdList.discountId = Convert.ToInt32(packageRow.Cells["discountName"].Value);

            //                    if (packageRow.Cells["transactionProfileId"].Value == DBNull.Value)
            //                        packageIdList.transactionProfileId = -1;
            //                    else if (Convert.ToInt32(packageRow.Cells["transactionProfileId"].Value) < 0)
            //                        packageIdList.transactionProfileId = -1;
            //                    else
            //                        packageIdList.transactionProfileId = Convert.ToInt32(packageRow.Cells["transactionProfileId"].Value);

            //                    packageIdList.productType = packageRow.Cells["ProductType"].Value.ToString();
            //                    packageIdList.priceInclusive = packageRow.Cells["PriceInclusive"].Value.ToString();
            //                    packageIdList.productPrice = packageRow.Cells["price"].Value == DBNull.Value ? 0.0 : Convert.ToDouble(packageRow.Cells["price"].Value);
            //                    packageIdList.remarks = packageRow.Cells["Remarks"].Value == null ? null : packageRow.Cells["Remarks"].Value.ToString();
            //                    lstPacakgeIdList.Add(packageIdList);
            //                    //Begin -Jan-27-2016- Removed qty check//
            //                    //if (Convert.ToInt32(packageRow.Cells["Quantity"].Value) > Convert.ToInt32(nudGuestCount.Value))
            //                    //{
            //                    //    POSUtils.ParafaitMessageBox(MessageUtils.getMessage(945) + packageName);
            //                    //    return false;
            //                    //}
            //                    //End -Jan-27-2016- Removed qty check//
            //                    //Begin:If products in the list has product of type combo and combo has products of type "'NEW', 'CARDSALE', 'RECHARGE', 'GAMETIME'" then open the form to add the new cards
            //                    if (packageRow.Cells["ProductType"].Value.ToString() == "COMBO")
            //                    {
            //                        //load the booked category products
            //                        if (CategoryProuctFinalSelectedList.Count == 0 && BookingId != -1)
            //                        {
            //                            Semnox.Parafait.Transaction.Reservation.clsSelectedCategoryProducts selectedProducts;
            //                            DataTable selectedCategoryProducts = POSStatic.Utilities.executeDataTable(@"Select tt.product_id,tt.quantity from trx_lines tt,  
            //                                                                            (select LineId,tl.TrxId from trx_lines tl, bookings b where tl.trxid  = b.TrxId
            //                                                                            and b.BookingId = @bookingId
            //                                                                            and tl.product_id = @ComboProductId)a
            //                                                                            where a.LineId = tt.ParentLineId and tt.TrxId = a.TrxId 
            //                                                                            and 
            //                                                                            not exists(Select cp.Product_Id from ComboProduct cp where 
            //                                                                            cp.ChildProductId = tt.product_id and cp.Product_Id = @ComboProductId)",
            //                                                                                                new SqlParameter("@bookingId", BookingId),
            //                                                                                                new SqlParameter("@ComboProductId", packageIdList.productId));
            //                            if (selectedCategoryProducts.Rows.Count > 0)
            //                            {
            //                                foreach (DataRow categoryProduct in selectedCategoryProducts.Rows)
            //                                {
            //                                    selectedProducts = new Semnox.Parafait.Transaction.Reservation.clsSelectedCategoryProducts();
            //                                    selectedProducts.productId = Convert.ToInt32(categoryProduct["product_id"].ToString());
            //                                    selectedProducts.parentComboProductId = packageIdList.productId;
            //                                    selectedProducts.quantity = Convert.ToInt32(categoryProduct["quantity"]);
            //                                    CategoryProuctFinalSelectedList.Add(selectedProducts);
            //                                }
            //                            }
            //                        }
            //                    }
            //                    //End//
            //                }

            //                if (packageRow.Cells["packageName"].Value != null && packageRow.Cells["SelectProduct"].Value != null && packageRow.Cells["SelectProduct"].Value.ToString() == "Y")
            //                {
            //                    selected = true;
            //                }

            //            }
            //        }
            //        //Begin:If none of the products are selected then displays the message "Select package"//
            //        if (!selected)
            //        {
            //            txtMessage.Text = MessageUtils.getMessage(304);
            //            this.ActiveControl = dgvPackageList;
            //            tcBooking.SelectedTab = tpPackage;
            //            lstPacakgeIdList = null;
            //            log.LogMethodExit("Package was not selected");
            //            return false;
            //        }
            //        //End//
            //    }
            //    else
            //    {
            //        for (int i = 0; i < lstPacakgeIdList.Count; i++)
            //        {
            //            quantity += lstPacakgeIdList[i].guestQuantity;
            //        }
            //    }

            //    try
            //    {
            //        MoreThanOneServiceChargeEntry();
            //    }
            //    catch (ValidationException ex)
            //    {
            //        log.Error(ex);
            //        txtMessage.Text = ex.GetAllValidationErrorMessages();
            //        this.ActiveControl = dgvBookingProducts;
            //        tcBooking.SelectedTab = tpAdditional;
            //        return false;
            //    }
            //    GeneratePackageAttractionProductlist();
            //    GenerateAdditionalAttractionProductlist();
            //    GenerateAdditionalProductModifiers();
            //    GeneratePackageProductModifiers();

            //    //Begin: Load discount peviously selected while booking if any.If discount were selected while booking, load the discounts//
            //    if (reservationTransactionDiscounts.Count == 0 && BookingId != -1)
            //    {
            //        ReservationDiscountsListBL reservationDiscountsListBL = new ReservationDiscountsListBL();
            //        List<KeyValuePair<ReservationDiscountsDTO.SearchByParameters, string>> searchReservationDiscountsParams = new List<KeyValuePair<ReservationDiscountsDTO.SearchByParameters, string>>();
            //        searchReservationDiscountsParams.Add(new KeyValuePair<ReservationDiscountsDTO.SearchByParameters, string>(ReservationDiscountsDTO.SearchByParameters.BOOKING_ID, BookingId.ToString()));
            //        searchReservationDiscountsParams.Add(new KeyValuePair<ReservationDiscountsDTO.SearchByParameters, string>(ReservationDiscountsDTO.SearchByParameters.SITE_ID, (Utilities.ParafaitEnv.IsCorporate ? Utilities.ParafaitEnv.SiteId : -1).ToString()));
            //        List<ReservationDiscountsDTO> reservationDiscountsDTOList = reservationDiscountsListBL.GetReservationDiscountsDTOList(searchReservationDiscountsParams);
            //        if (reservationDiscountsDTOList != null && reservationDiscountsDTOList.Count > 0)
            //        {
            //            foreach (var reservationDiscountsDTO in reservationDiscountsDTOList)
            //            {
            //                DiscountsBL discountsBL = new DiscountsBL(reservationDiscountsDTO.ReservationDiscountId);
            //                reservationTransactionDiscounts.Add(discountsBL.DiscountsDTO);
            //            }

            //        }
            //    }
            //    //End:Discounts//

            //    //Begin: Populate the list. In the below  code added all the booking details to a list//
            //    List<Semnox.Parafait.Transaction.Reservation.clsBookingDetails> lstBookingDetails = new List<Semnox.Parafait.Transaction.Reservation.clsBookingDetails>();
            //    Semnox.Parafait.Transaction.Reservation.clsBookingDetails bookingDetails = new Semnox.Parafait.Transaction.Reservation.clsBookingDetails();
            //    bookingDetails.BookingId = (reservationDTO != null ? reservationDTO.BookingId : BookingId);
            //    bookingDetails.Status = (reservationDTO != null ? reservationDTO.Status : ReservationDTO.ReservationStatus.NEW.ToString());
            //    bookingDetails.ReservationCode = (reservationDTO != null ? reservationDTO.ReservationCode : "");
            //    bookingDetails.BookingName = txtBookingName.Text;
            //    bookingDetails.bookingProductId = Convert.ToInt32(cmbBookingClass.SelectedValue);
            //    bookingDetails.CustomerDTO = customerDetailUI.CustomerDTO;
            //    bookingDetails.reservationFromDateTime = fromDateTime;
            //    bookingDetails.Quantity = Convert.ToInt32(txtGuests.Text); //g Convert.ToInt32(nudGuestCount.Value);
            //    bookingDetails.CardNumber = txtCardNumber.Text;
            //    bookingDetails.CustomerId = Reservation.CustomerId;
            //    bookingDetails.Remarks = txtRemarks.Text;
            //    bookingDetails.User = ParafaitEnv.LoginID;
            //    bookingDetails.ExpiryTime = DBNull.Value;
            //    bookingDetails.recurFlag = cbxRecur.Checked;
            //    bookingDetails.RecurFrequency = cmbRecurFrequency.SelectedItem.ToString()[0];
            //    bookingDetails.RecurUntil = dtpRecurUntil.Value.Date;
            //    bookingDetails.Channel = cmbChannel.SelectedItem.ToString();

            //    //End Modification Dec-24-2015 - Added additional parameters to save customer Details//
            //    bookingDetails.facilityId = facilityId;
            //    bookingDetails.attractionScheduleId = attractionScheduleId;
            //    lstBookingDetails.Add(bookingDetails);
            //    //end: Populate list//

            //    //Begin-Jan-21-2016- Added Additional Parameters
            //    status = Reservation.MakeReservation(lstBookingDetails,
            //                                                bookingProductPrice,
            //                                                isEditedBooking, editedBookingId,
            //                                                lstPacakgeIdList,
            //                                                Duration,
            //                                                lstProducts, cardFinalList,
            //                                                CategoryProuctFinalSelectedList,
            //                                                reservationTransactionDiscounts,
            //                                                lstAttractionProductslist,
            //                                                lstAdditionalAttractionProductsList,
            //                                                ref message, //lstProductModifiersSelectedList, lstAdditionalProductModifiersSelectedList,
            //                                                lstPurchasedModifierProducts, lstPurchasedAdditionalModifierProducts);//Modified for Adding lstProductModifiersSelectedList and lstAdditionalProductModifiersSelectedList on 04-Oct-2016
            //    //Added atb on Jan-20-2016//
            //    if (!status)
            //    {
            //        btnBook.Enabled = false;
            //        txtMessage.Text = message + " " + MessageContainer.GetMessage(Utilities.ExecutionContext, 1789);// "Review the Booking Details"
            //        log.LogMethodExit(MessageContainer.GetMessage(Utilities.ExecutionContext, 1789));
            //        return false;
            //    }
            //    else
            //    {
            //        if (reservationDTO != null)
            //        {
            //            reservationDTO = null;
            //        }
            //        lblStatus.Text = Reservation.Status;
            //        if (Reservation.Status != ReservationDTO.ReservationStatus.WIP.ToString()
            //            && Reservation.Status != ReservationDTO.ReservationStatus.BLOCKED.ToString()) //clear off expiry time label once edit booking is saved
            //        {
            //            lblExpires.Text = "";
            //        }
            //        try
            //        {
            //            ApplyTrxProfile(Reservation.Transaction, lstPacakgeIdList, lstProducts);
            //            SetTransactionProfileName(Reservation.Transaction.TrxProfileId);
            //        }
            //        catch (Exception ex)
            //        {
            //            log.Error(ex);
            //            POSUtils.ParafaitMessageBox(MessageContainer.GetMessage(Utilities.ExecutionContext, 1790, ex.Message)); //'Error while applying Transaction Profiles to the transaction. Error message: &1 '
            //        }
            //        BookingId = Reservation.BookingId;
            //        btnEditBooking.Enabled = true;
            //        btnConfirm.Enabled = true;
            //        //btnExecute.Text = "Add Attendee Details";
            //        btnBook.Enabled = false;
            //        txtMessage.Text = MessageUtils.getMessage(308, Reservation.ReservationCode);
            //        log.Info("save() - Booking " + Reservation.ReservationCode.ToString() + "saved successfully");
            //        this.BringToFront();//Added to prevent the booking from from hidind behind//
            //        if (isNew)
            //        {
            //            //log.Info("save() - New Booking BookingId: " + BookingId.ToString() + "");
            //            loginId = ParafaitEnv.LoginID;
            //            lblReservationCode.Text = Reservation.ReservationCode;
            //            btnPrint.Enabled = btnEmail.Enabled = btnConfirm.Enabled = btnCancelBooking.Enabled = btnAttendeeDetails.Enabled = true;
            //            //Added to create audit trail when booking is saved successfully on June24, 2015//
            //            audit.logEvent("Reservation", 'D', loginId, "Booking is saved successfully, BookingId is " + BookingId, "ConfirmationScreen", 0, "", BookingId.ToString(), null);
            //        }
            //        if (isEditedBooking)
            //            lblReservationCode.Text = Reservation.ReservationCode;
            //        txtBookingEstimate.Text = Reservation.Transaction.Net_Transaction_Amount.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
            //        txtAdvanceAmount.Text = Reservation.AdvanceRequired.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
            //        txtDiscount.Text = Reservation.Transaction.Discount_Amount.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
            //        txtTransactionAmount.Text = Reservation.Transaction.Transaction_Amount.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);//Added txtDiscount & txtTransactionAmount to display Discount and Transaction Amount

            //        txtBalanceAmount.Text = Math.Round(Reservation.Transaction.Net_Transaction_Amount - Reservation.Transaction.TotalPaidAmount, ParafaitEnv.RoundingPrecision, MidpointRounding.AwayFromZero).ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);//Begin Modification 19-Jul-2016 - Added to display the Balance Amount 

            //        GetAdditionalBookingProducts();
            //        //Begin Modification Dec-22-2015- Display Booking Summary and Confirm the reservation if user has clicked Yes//
            //        if (MessageBox.Show((POSStatic.MessageUtils.getMessage(1791, Environment.NewLine, txtBookingName.Text, dtpforDate.Value.ToString("MMM-dd-yyyy"), cmbFromTime.Text) +
            //                            POSStatic.MessageUtils.getMessage(1792, Environment.NewLine, cmbToTime.Text, txtGuests.Text, cmbBookingClass.Text) +
            //                            POSStatic.MessageUtils.getMessage(1793, Environment.NewLine, Reservation.ReservationCode, Reservation.Transaction.Trx_id)), MessageContainer.GetMessage(Utilities.ExecutionContext, "Booking Summary"), MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            //        {
            //            btnConfirm.Enabled = false;
            //            ConfirmReservation();
            //        }
            //        //End Modification Dec-22-2015//
            //        log.LogMethodExit(true);
            //        return true;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    POSUtils.ParafaitMessageBox(ex.Message);
            //    MessageBox.Show(ex.StackTrace);
            //    log.Error(ex);
            //    log.LogMethodExit(false);
            //    return false;
            //}
            
        }
        //End : Save

        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            txtMessage.Clear();
            this.Close();
            log.LogMethodExit();
        }

        private void cmbBookingClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            //if (cmbBookingClass.SelectedValue != null && cmbBookingClass.SelectedIndex == 0)
            //{
            if (userAction)
            {
                attractionScheduleId = -1;
            }
            //}
            cmbFromTime.Enabled = cmbToTime.Enabled = false;
            //  btnFind.Enabled = false;
            packageIdSelected = 0;
            GetBookingPackage();
            log.LogMethodExit();
        }

        //Begin Modification Dec-22-2015- Confirm the reservation//
        private void ConfirmReservation()
        {
            log.LogMethodEntry();
            //string message = "";
            //btnExecute.Text = MessageContainer.GetMessage(Utilities.ExecutionContext, "Execute");
            //btnEditBooking.Enabled = true;
            ///*Added 2 parameters to frmMakeReservation to Enable Edit feature after confirming the Reservation with Payment on May 18, 2015
            // if edited, isEditedBooking is set to true and editedBookingId is the Id of the Booking which was selected to Modify */
            //if (Reservation.ConfirmReservation(ref message, isEditedBooking, editedBookingId))
            //{
            //    txtBookingEstimate.Text = Reservation.Transaction.Net_Transaction_Amount.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
            //    txtAdvanceAmount.Text = Reservation.AdvanceRequired.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
            //    txtDiscount.Text = Reservation.Transaction.Discount_Amount.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
            //    txtTransactionAmount.Text = Reservation.Transaction.Transaction_Amount.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);//Added txtDiscount & txtTransactionAmount to display Discount and Transaction Amount

            //    //Added to create audit trail for when an edited booking is saved successfully on June24, 2015//
            //    if (isEditedBooking)
            //    {
            //        Utilities = POSStatic.Utilities;
            //        audit = new Semnox.Core.Utilities.EventLog(Utilities);
            //        ParafaitEnv = POSStatic.ParafaitEnv;
            //    }
            //    //Added the below method to create audit trail for every modification done to a reservation on June24, 2015//
            //    LogEvent(isEditedBooking, editedBookingId);
            //    txtMessage.Text = lblStatus.Text = ReservationDTO.ReservationStatus.CONFIRMED.ToString();
            //    log.Info("ConfirmReservation() - Reservation is CONFIRMED");
            //}
            //else
            //{
            //    log.Info("ConfirmReservation() - unable to Confirm reservation error: " + message);
            //}
            //txtMessage.Text = message;

            //UpdateUIStatus();

            log.LogMethodExit();
        }
        //End Modification Dec-22-2015 //

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                txtMessage.Clear();
                if (POSUtils.ParafaitMessageBox(MessageUtils.getMessage(309), MessageUtils.getMessage("Confirm"), MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    ConfirmReservation();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(ex.Message);
            }
            log.LogMethodExit();
        }

        private void btnCancelBooking_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            txtMessage.Clear();
            //if (POSUtils.ParafaitMessageBox(MessageUtils.getMessage(310), MessageUtils.getMessage("Confirm"), MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            //{
            //    if (Reservation.Transaction != null && Reservation.Transaction.TotalPaidAmount > 0)
            //    {
            //        POSUtils.ParafaitMessageBox(MessageUtils.getMessage(525), MessageUtils.getMessage("Payment Validation"));
            //        log.LogMethodExit("Delete payments before booking cancellation");
            //        btnPayment.PerformClick();
            //        return;
            //    }

            //    string message = "";
            //    if (Reservation.CancelReservation(lblReservationCode.Text, ref message))
            //    {
            //        txtMessage.Text = lblStatus.Text = "CANCELLED";
            //        log.Info("btnCancelBooking_Click() - Reservation is Cancelled ");
            //    }
            //    else
            //    {
            //        txtMessage.Text = message;
            //        log.Info("btnCancelBooking_Click() - unable to Cancel Reservation error: " + message + "");
            //    }
            //    UpdateUIStatus();
            //}
            log.LogMethodExit();
        }

        private void cbxRecur_CheckedChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (cbxRecur.Checked)
                cmbRecurFrequency.Enabled = dtpRecurUntil.Enabled = true;
            else
                cmbRecurFrequency.Enabled = dtpRecurUntil.Enabled = false;

            log.LogMethodExit();
        }

        private void btnEmail_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            txtMessage.Clear();
            try
            {
                Semnox.Parafait.Communication.SendEmailUI semail;
                string message = "";
                string template = "";
                string AttachFile = null;
                string reportFileName = GetBookingQuoteReport();
                if (ParafaitEnv.CompanyLogo != null)
                {
                    contentID = "ParafaitBookingLogo" + Guid.NewGuid().ToString() + ".jpg";//Content Id is the identifier for the image
                    AttachFile = System.IO.Path.GetTempPath() + contentID;
                    ParafaitEnv.CompanyLogo.Save(AttachFile, ImageFormat.Jpeg);// Save the logo to the folder as a jpeg file
                }

                //Get the email template selected by the booking//
                DataTable dt = Utilities.executeDataTable(@"select EmailTemplate from EmailTemplate where EmailTemplateId = (select EmailTemplateId from products where product_id  = @BookingProductId)", new SqlParameter("@BookingProductId", Convert.ToInt32(cmbBookingClass.SelectedValue)));
                if (dt.Rows.Count > 0)
                {
                    message = dt.Rows[0]["EmailTemplate"].ToString();
                    if (message.ToLower().Contains("<html") == false)
                    {
                        template = message.Replace("\r\n", "\n");
                        // template = message.Replace("\n\n", "<br /> <br /> ").Replace("\n", "<br />");
                    }
                    else
                        template = message;
                    string body = GetBookingDetailsForEmail(template);

                    //Newly added constructor in ParafaitUtils , SendEmailUI class. To display sito logo inline with Email Body. 2 additional parameters attachimage and contentid are addded//
                    semail = new Semnox.Parafait.Communication.SendEmailUI(customerDetailUI.CustomerDTO.Email,
                                                        "", "", "POS Reservation", body, reportFileName, "", AttachFile, contentID, false, Utilities, true);
                }
                else
                {
                    semail = new Semnox.Parafait.Communication.SendEmailUI(customerDetailUI.CustomerDTO.Email,
                                                         "", "",
                                                          Utilities.MessageUtils.getMessage(1835,lblReservationCode.Text,ParafaitEnv.SiteName,lblStatus.Text.ToUpper()),//"Your Booking (" + lblReservationCode.Text + ") at " + ParafaitEnv.SiteName + " " + lblStatus.Text)
                                                         "Dear " + customerDetailUI.CustomerDTO.FirstName + "," + Environment.NewLine + Environment.NewLine +
                                                       //  GetBookingInfoForEmailPrint(), --"Thank you very much for choosing us. Please find your booking details attached. We look forward to welcoming you soon!"
                                                       Utilities.MessageUtils.getMessage(1833) + Environment.NewLine +null,
                                                         reportFileName, "", false, Utilities, true);
                }
                semail.ShowDialog();
                if (semail.EmailSentSuccessfully)
                {
                    log.Info("Email Send Successfully");
                    Utilities.executeScalar("update bookings set isEmailSent = isnull(isEmailSent, 0) + 1 where BookingId = @BookingId", new SqlParameter[] { new SqlParameter("@BookingId", BookingId) });
                    cbxEmailSent.Checked = true;
                }

                //Delete the image created in the image folder once Email is sent successfully//
                FileInfo file = new FileInfo(AttachFile);
                if (file.Exists)
                {
                    file.Delete();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(MessageUtils.getMessage(1824, ex.Message));
            }

            log.LogMethodExit();
        }
        private void btnAttendeeDetails_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            txtMessage.Clear();
            using (frmBookingAttendees frb = new frmBookingAttendees(BookingId, txtBookingName.Text + "-" + lblReservationCode.Text, customerDetailUI.CustomerDTO.FirstName, Utilities.getServerTime().Date.AddHours(Convert.ToDouble(cmbFromTime.SelectedValue)), Utilities.getServerTime().Date.AddHours(Convert.ToDouble(cmbToTime.SelectedValue))))
            {
                frb.ShowDialog();
            }
            log.LogMethodExit();
        }

        private void cmbFromTime_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (dtBookingProduct != null && dtBookingProduct.Rows.Count > 0)
            {
                foreach (DataRow dr in dtBookingProduct.Rows)
                {
                    int duration = Convert.ToInt32(dr["MinimumTime"].ToString());
                    if (duration > 0)
                    {
                        GetToTime(duration);
                    }
                }
            }
            log.LogMethodExit();
        }

        private void ResetTimeSlots()
        {
            log.LogMethodEntry();
            if (userAction)
            {
                userAction = false;
                try
                {
                    if (cbxEarlyMorning.Checked)
                        cbxEarlyMorning.Checked = false;
                    if (cbxMorning.Checked)
                        cbxMorning.Checked = false;
                    if (cbxAfternoon.Checked)
                        cbxAfternoon.Checked = false;
                    if (cbxNight.Checked)
                        cbxNight.Checked = false;
                }
                finally
                {
                    userAction = true;
                }
            }
            log.LogMethodExit();
        }

        private void GetToTime(int duration)
        {
            log.LogMethodEntry(duration);
            decimal fromTime = Convert.ToDecimal(cmbFromTime.SelectedValue);
            int totToMins = (int)(Math.Floor(fromTime) * 60 + (fromTime - Math.Floor(fromTime)) * 100 + duration);
            decimal toTime = totToMins / 60 + (decimal)((totToMins % 60) / 100.0);
            if (toTime > (decimal)23.45)
            {
                toTime = toTime - 24;
            }
            cmbToTime.SelectedValue = toTime;
            log.LogMethodExit();
        }

        private void dgvBookingProducts_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            txtMessage.Clear();
            if (e.ColumnIndex < 0 || e.RowIndex < 0)
            {
                log.LogMethodExit("e.RowIndex < 0 || e.ColumnIndex < 0");
                return;
            }

            if (dgvBookingProducts.Columns[e.ColumnIndex].Name == "dcDelete")
            {
                //if (dgvBookingProducts["dcProduct", e.RowIndex].Value != null)
                //{
                //    Utilities.executeNonQuery("delete from BookingProducts where BookingId = @bookingId and ProductId = @productId",
                //                                       new SqlParameter("@bookingId", BookingId),
                //                                       new SqlParameter("@productId", dgvBookingProducts["dcProduct", e.RowIndex].Value));
                //}

                //begin: Added To delete the addons after booking
                //if (BookingId != -1 && dgvBookingProducts["dcProduct", e.RowIndex].Value != null)
                //{

                //    Utilities.executeNonQuery("delete from trx_lines  " +
                //                                " where trxid = (Select trxid from bookings where BookingId = @bookingId ) and product_id = @productId",
                //                                                   new SqlParameter("@bookingId", BookingId),
                //                                                   new SqlParameter("@productId", dgvBookingProducts["dcProduct", e.RowIndex].Value));
                //}
                //End

                try
                {
                    if (dgvBookingProducts["dcProduct", e.RowIndex].Value != null)
                    {
                        int productId = Convert.ToInt32(dgvBookingProducts["dcProduct", e.RowIndex].Value);
                        int quantity = Convert.ToInt32(dgvBookingProducts["dcQuantity", e.RowIndex].Value);
                        int lineId = Convert.ToInt32(dgvBookingProducts["LineId", e.RowIndex].Value);
                        dgvBookingProducts.Rows.Remove(dgvBookingProducts.Rows[e.RowIndex]);
                        BuildProducInformationLst();
                        BuildModifierLstFromProductInformationLst();
                        BuildAttractionSchedulesLstFromProductInformationLst();
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
            }
            log.LogMethodExit();
        }

        private void MoreThanOneServiceChargeEntry()
        {
            log.LogMethodEntry();
            int lineCount = 0;
            foreach (DataGridViewRow item in dgvBookingProducts.Rows)
            {
                if (item.Cells["dcProduct"].Value != null && item.Cells["type"].Value.ToString() == "SERVICECHARGE")
                {
                    lineCount++;
                }
                if (lineCount > 1)
                {
                    throw new ValidationException(Utilities.MessageUtils.getMessage(1828));// "Can not enter multiple entries for Service charge")
                }
            }
            log.LogMethodExit();
        }
        private void dgvBookingProducts_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.ColumnIndex < 0 || e.RowIndex < 0)
            {
                log.LogMethodExit("e.ColumnIndex < 0 || e.RowIndex < 0");
                return;
            }

            if (dgvBookingProducts.Columns[e.ColumnIndex].Name == "dcProduct")
            {
                if (dgvBookingProducts["dcProduct", e.RowIndex].Value != null)
                {
                    if (dgvBookingProducts["dcQuantity", e.RowIndex].Value == null
                    || dgvBookingProducts["dcQuantity", e.RowIndex].Value == DBNull.Value)
                    {
                        dgvBookingProducts["dcQuantity", e.RowIndex].Value = nudQuantity.Value;
                    }

                    if (dgvBookingProducts["dcPrice", e.RowIndex].Value == null
                    || dgvBookingProducts["dcPrice", e.RowIndex].Value == DBNull.Value)
                    {
                        dgvBookingProducts["dcPrice", e.RowIndex].Value = Utilities.executeScalar(@"select isnull(price,0)price from products where product_id = @productId",
                                                                           new SqlParameter("@productId", dgvBookingProducts["dcProduct", e.RowIndex].Value));
                    }
                    //Begin Modification-Jan-14-2016- Added to load the product Type on grid cell click//
                    DataTable productType = Utilities.executeDataTable(@"select p.price,pt.product_type from products p, product_type pt where product_id = @productId and                                                          p.product_type_id = pt.product_type_id",
                                                                    new SqlParameter("@productId", dgvBookingProducts["dcProduct", e.RowIndex].Value));
                    dgvBookingProducts["type", e.RowIndex].Value = productType.Rows[0]["product_type"];
                    //End Modification-Jan-14-2016- Added to load the product Type on grid cell click//

                    if (dgvBookingProducts["AdditionalDiscountName", e.RowIndex].Value == null
                   || dgvBookingProducts["AdditionalDiscountName", e.RowIndex].Value == DBNull.Value)
                    {
                        dgvBookingProducts["AdditionalDiscountName", e.RowIndex].Value = -1;
                    }

                    if (dgvBookingProducts["additionalTransactionProfileId", e.RowIndex].Value == null
                   || dgvBookingProducts["additionalTransactionProfileId", e.RowIndex].Value == DBNull.Value)
                    {
                        dgvBookingProducts["additionalTransactionProfileId", e.RowIndex].Value = -1;
                    }
                    //Begin: Added to Show Attraction Schedule PopUp on 17-Jun-2016                     
                    if (dgvBookingProducts["type", e.RowIndex].Value.ToString() == "ATTRACTION")
                    {
                        Card card = null;
                        DataTable attractionProductDet = trxUtils.getProductDetails(Convert.ToInt32(dgvBookingProducts["dcProduct", e.RowIndex].Value), card);

                        if (attractionProductDet.Rows.Count > 0)
                        {
                            //if (attractionProductDet.Rows[0]["QuantityPrompt"].ToString() == "Y")
                            //{
                            //    int attractionQuantity = productQuantity;
                            //    while (attractionQuantity > 0)
                            //    {
                            //        createAttractionProduct(Convert.ToInt32(dgvBookingProducts["dcProduct", e.RowIndex].Value), 0, 1, -1, lstAttractionProductslist);//Combo product Id is sent -1, since its not part of Combo
                            //        attractionQuantity--;
                            //    }
                            //}
                            //else
                            {
                                CreateAttractionProduct(-1, Convert.ToInt32(dgvBookingProducts["dcProduct", e.RowIndex].Value), 0, Convert.ToInt32(dgvBookingProducts["dcQuantity", e.RowIndex].Value), -1, lstAdditionalAttractionProductsList);
                            }
                        }
                    }
                    //End: Added to Show Attraction Schedule PopUp on 17-Jun-2016
                }
            }
            log.LogMethodExit();
        }

        private void btnPayment_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            //txtMessage.Clear();
            //Semnox.Core.Utilities.EventLog audit = new Semnox.Core.Utilities.EventLog(Utilities);
            //btnEditBooking.Enabled = true;
            //if (Reservation.Status == ReservationDTO.ReservationStatus.BOOKED.ToString())
            //{
            //    txtMessage.Text = MessageContainer.GetMessage(Utilities.ExecutionContext, 1794); // "Please confirm the reservation before payment";
            //    log.LogMethodExit(txtMessage.Text);
            //    return;
            //}
            //else if (Reservation.Status == ReservationDTO.ReservationStatus.CONFIRMED.ToString())
            //{
            //    Reservation.Transaction = trxUtils.CreateTransactionFromDB(Reservation.Transaction.Trx_id, Reservation.Transaction.Utilities);

            //    Card savCard = null;
            //    if (Reservation.Transaction.PrimaryCard != null)
            //    {
            //        savCard = Reservation.Transaction.PrimaryCard;
            //        Reservation.Transaction.PrimaryCard = null; // otherwise card payment will be shown
            //    }
            //    Reservation.Transaction.getTotalPaidAmount(null);//Added the method here to calculate total paid amount before calling the payments form
            //    double amountPaid = Reservation.Transaction.TotalPaidAmount;
            //    PaymentDetails frmPayment = new PaymentDetails(Reservation.Transaction);
            //    DialogResult dr = frmPayment.ShowDialog();
            //    if (savCard != null)
            //        Reservation.Transaction.PrimaryCard = savCard;
            //    Reservation.Transaction.PaymentCreditCardSurchargeAmount = frmPayment.PaymentCreditCardSurchargeAmount;
            //    Reservation.Transaction.getTotalPaidAmount(null);

            //    txtPaidAmount.Text = Reservation.Transaction.TotalPaidAmount.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
            //    //Begin Modification Dec-24-2015 - Added to display the Balance Amount 
            //    txtBalanceAmount.Text = Math.Round(Reservation.Transaction.Net_Transaction_Amount - Reservation.Transaction.TotalPaidAmount, ParafaitEnv.RoundingPrecision, MidpointRounding.AwayFromZero).ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
            //    //if (Reservation.Transaction.TotalPaidAmount >= 0 && Reservation.Transaction.TotalPaidAmount > Reservation.AdvanceRequired)
            //    //    txtAdvanceAmount.Text = 0.0.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);  
            //    //End Modificaton Dec-24-2015//

            //    if (amountPaid != Reservation.Transaction.TotalPaidAmount)
            //        audit.logEvent("Reservation", 'D', Reservation.Transaction.LoginID, " Total Amount paid till Date: " + txtPaidAmount.Text, "PaymentScreen", 0, "", BookingId.ToString(), null);
            //    UpdateUIStatus();
            //}
            log.LogMethodExit();
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            txtMessage.Clear();

            //if (Reservation.Transaction != null)
            //{
            //    using (frmInputPhysicalCards fin = new frmInputPhysicalCards(Reservation.Transaction))
            //    {
            //        Dictionary<string, string> tempCardList = new Dictionary<string, string>();
            //        if (fin.cardList.Count > 0) // card lines found
            //        {
            //            foreach (string key in fin.cardList.Keys)
            //            {

            //                if (key.Contains("T"))
            //                {
            //                    tempCardList.Add(key, "");
            //                }
            //            }
            //        }

            //        if (tempCardList.Count > 0)
            //        {
            //            fin.cardList = tempCardList;
            //            if (fin.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
            //            {
            //                log.LogMethodExit("frmInputPhysicalCards was cancelled");
            //                return;
            //            }
            //        }

            //        string message = "";
            //        if (Reservation.ExecuteReservation(fin.cardList, ref message))
            //        {
            //            audit = new Semnox.Core.Utilities.EventLog(Utilities);
            //            txtMessage.Text = MessageContainer.GetMessage(Utilities.ExecutionContext, 1795);// "Executed successfully. Open the Transaction in POS to continue.";
            //            lblStatus.Text = ReservationDTO.ReservationStatus.COMPLETE.ToString();
            //            UpdateUIStatus();
            //            audit.logEvent("Reservation", 'D', loginId, "Reservation is Executed", "Confirmation Screen", 0, "", BookingId.ToString(), null);
            //            log.Info("btnExecute_Click() - Reservation is Executed successfully with BookingId :" + BookingId.ToString());

            //        }
            //        else
            //        {
            //            txtMessage.Text = message;
            //            log.Error("Ends-btnExecute_Click() as unable to Reservation Error: " + message);
            //        }
            //    }
            //}
            //else
            //{
            //    log.Error("Reservation transaction is null");
            //}
            log.LogMethodExit();
        }

        private string GetBookingDetailsForEmail(string template)
        {
            log.LogMethodEntry(template);
            try
            {
                //double fromtime = Convert.ToDouble(cmbFromTime.SelectedValue);
                //double min = (fromtime - Convert.ToInt32(fromtime)) * 100;
                //fromtime = Convert.ToInt32(fromtime) * 60 + min;

                //double totime = Convert.ToDouble(cmbToTime.SelectedValue);
                //min = (totime - Convert.ToInt32(totime)) * 100;
                //totime = Convert.ToInt32(totime) * 60 + min;
                //string bookingProductContents = "";
                //string additionalproducts = "";
                //string facilityName = "";
                //string advancePaidDate = "";

                //DataTable dtBookingInformation = Utilities.executeDataTable(@"SELECT CF.FacilityName, TP.PaymentDate
                //                                                              FROM BOOKINGS B LEFT OUTER JOIN TRXPAYMENTS TP ON B.TRXID = TP.TRXID, 
                //                                                                   AttractionSchedules ATS, CheckInFacility CF
                //                                                             WHERE B.AttractionScheduleId = ATS.AttractionScheduleId
                //                                                               AND ATS.FacilityID = CF.FacilityId
                //                                                               AND B.BookingId = @BookingId",
                //                                                            new SqlParameter("@BookingId", BookingId));
                //if (dtBookingInformation.Rows.Count > 0)
                //{
                //    if (dtBookingInformation.Rows[0]["FacilityName"] != DBNull.Value)
                //        facilityName = dtBookingInformation.Rows[0]["FacilityName"].ToString();

                //    if (dtBookingInformation.Rows[0]["PaymentDate"] != DBNull.Value)
                //        advancePaidDate = Convert.ToDateTime(dtBookingInformation.Rows[0]["PaymentDate"]).ToString(ParafaitEnv.DATETIME_FORMAT);
                //}

                //DataTable dtBookedBookingProductDetails = Utilities.executeDataTable(@"SELECT b.BookingProductId,
                //                                                   tl.product_id,
                //                                                   p.product_name,
                //                                                   tl.Quantity,
                //                                                   tl.Remarks,
                //                                                   rd.ReservationDiscountId discountId,p.price
                //                                            FROM 
                //                                                 Bookings b
                //                                            INNER JOIN
                //                                              (SELECT TRXID,
                //                                                      PRODUCT_ID,
                //                                                      remarks,
                //                                                      SUM(TL.QUANTITY) Quantity
                //                                               FROM trx_lines tl
                //                                               WHERE tl.ParentLineId IS NULL
                //                                               GROUP BY TRXID,
                //                                                        PRODUCT_ID,
                //                                                        Remarks) tl ON b.TrxId = tl.TrxId
                //                                            INNER JOIN products p ON p.product_id = tl.product_id
                //                                            INNER JOIN ComboProduct cb ON cb.ChildProductId = tl.product_id
                //                                            LEFT OUTER JOIN ReservationDiscounts rd ON rd.productId = tl.product_id
                //                                            AND rd.BookingId = @BookingId
                //                                            WHERE cb.AdditionalProduct ='N'
                //                                              AND b.BookingId = @BookingId
                //                                              AND cb.Product_Id = b.BookingProductId",
                //                                                            new SqlParameter("@BookingId", BookingId));

                //DataTable dtadditionalProductDetails = Utilities.executeDataTable(@"select p.product_name, sum(tl.quantity) quantity,
                //                                                                        cast(sum(tl.price) as numeric(18,2)) price, 
                //                                                                        tl.Remarks
                //                                                                        --,rd.ReservationDiscountId discountId ,cb.AdditionalProduct
                //                                                                     from 
                //                                                                        Bookings b 
                //                                                                        inner join trx_lines tl
                //                                                                        on b.TrxId = tl.TrxId
                //                                                                        inner join products p
                //                                                                        on p.product_id = tl.product_id
                //                                                                        inner join ComboProduct cb
                //                                                                        on cb.ChildProductId = tl.product_id
                //                                                                        left outer join ReservationDiscounts rd 
                //                                                                        on rd.productId = tl.product_id and rd.BookingId = @BookingId
                //                                                                        where
                //                                                                        cb.AdditionalProduct ='Y' and ParentLineId is null
                //                                                                        and 
                //                                                                        b.BookingId =  @BookingId and cb.Product_Id = b.BookingProductId
                //                                                                    group by product_name, tl.remarks", new SqlParameter("@BookingId", BookingId));

                //foreach (DataRow dr in dtBookedBookingProductDetails.Rows)
                //{
                //    //bookingProductContents += "\t " + "Product Name: " + "[" + dr["product_name"].ToString() + "]" + "\t" + "Product Price: [" + dr["price"].ToString() + "]\t " + "Product Quantity: [" + Convert.ToDouble(dr["quantity"].ToString()) + "]" + Environment.NewLine;
                //    bookingProductContents += "\t\t " + dr["product_name"].ToString() + " [Qty.: " + dr["quantity"].ToString() + "]" + Environment.NewLine;
                //    if (!string.IsNullOrEmpty(dr["Remarks"].ToString()))
                //        bookingProductContents += " " + dr["Remarks"].ToString();
                //    bookingProductContents += Environment.NewLine;
                //}

                //foreach (DataRow additional in dtadditionalProductDetails.Rows)
                //{
                //    try
                //    {
                //        //additionalproducts += "\t Product Name: [" + additional["product_name"].ToString() + "]\t Product Price: [" + Math.Round(Convert.ToDouble(additional["price"].ToString())) + "]\t \t Product Quantity: [" + Convert.ToDouble(additional["quantity"].ToString()) + "]" + Environment.NewLine;
                //        additionalproducts += "\t " + additional["product_name"].ToString() + " [Price: " + additional["price"].ToString() + "]" + " [QTY.:" + Convert.ToDouble(additional["quantity"].ToString()) + "]" + Environment.NewLine;
                //        if (!string.IsNullOrEmpty(additional["dcRemarks"].ToString()))
                //            additionalproducts += " " + additional["dcRemarks"].ToString();
                //        additionalproducts += Environment.NewLine;
                //    }
                //    catch (Exception ex)
                //    {
                //        log.Error(ex);
                //    }
                //}
                string bookingDetails = string.Empty;
                //string address = string.Empty;
                //if (customerDetailUI.CustomerDTO.AddressDTOList.Count > 0)
                //{
                //    address = customerDetailUI.CustomerDTO.AddressDTOList[0].Line1;
                //}
                //bookingDetails = template.Replace("@customerName", customerDetailUI.FirstName).Replace("@address", address).Replace("@phoneNumber", customerDetailUI.CustomerDTO.PhoneNumber).Replace("@bookingName", txtBookingName.Text)
                //                                .Replace("@emailAddress", customerDetailUI.CustomerDTO.Email).Replace("@reservationCode", lblReservationCode.Text).Replace("@fromDate", dtpforDate.Value.ToString(ParafaitEnv.DATE_FORMAT))
                //                                .Replace("@fromTime", Utilities.getServerTime().Date.AddMinutes(fromtime).ToString("h:mm tt")).Replace("@toTime", Utilities.getServerTime().Date.AddMinutes(totime).ToString("h:mm tt"))
                //                                .Replace("@bookingProduct", cmbBookingClass.Text).Replace("@guestCount", txtGuests.Text)//g nudGuestCount.Value.ToString())
                //                                .Replace("@ProductName", bookingProductContents).Replace("@estimateAmount", txtBookingEstimate.Text).Replace("@transactionAmount", txtTransactionAmount.Text)
                //                                .Replace("@advancePaid", txtPaidAmount.Text).Replace("@discountAmount", txtDiscount.Text)
                //                                .Replace("@balanceDue", txtBalanceAmount.Text).Replace("@advancePaidDate", advancePaidDate).Replace("@partyRoom", facilityName)
                //                                .Replace("@additionalItems", additionalproducts).Replace("@taxAmount", Reservation.Transaction.Transaction_Amount.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL))
                //                                .Replace("@remarks", txtRemarks.Text).Replace("@siteName", ParafaitEnv.SiteName).Replace("@siteAddress", ParafaitEnv.SiteAddress)
                //                                .Replace("@status", lblStatus.Text.ToUpper()).Replace("@sitelogo", "<img src=\"cid:" + contentID + "\">");

                //log.LogMethodExit(bookingDetails);
                return bookingDetails;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null);
                return null;
            }
        }


        //private string GetBookingInfoForEmailPrint()
        //{
        //    log.LogMethodEntry();
        //    double fromtime = Convert.ToDouble(cmbFromTime.SelectedValue);
        //    double min = (fromtime - Convert.ToInt32(fromtime)) * 100;
        //    fromtime = Convert.ToInt32(fromtime) * 60 + min;

        //    double totime = Convert.ToDouble(cmbToTime.SelectedValue);
        //    min = (totime - Convert.ToInt32(totime)) * 100;
        //    totime = Convert.ToInt32(totime) * 60 + min;

        //    string bookingProductContents = "";
        //    //string additionalproducts = "";

        //    DataTable dtBookedBookingProductDetails = Utilities.executeDataTable(@"SELECT b.BookingProductId,
        //                                                                                   tl.product_id,
        //                                                                                   p.product_name,
        //                                                                                   tl.Quantity,
        //                                                                                   tl.Remarks,
        //                                                                                   rd.ReservationDiscountId discountId,p.price
        //                                                                            FROM 
        //                                                                                 Bookings b
        //                                                                            INNER JOIN
        //                                                                              (SELECT TRXID,
        //                                                                                      PRODUCT_ID,
        //                                                                                      remarks,
        //                                                                                      SUM(TL.QUANTITY) Quantity
        //                                                                               FROM trx_lines tl
        //                                                                               WHERE tl.ParentLineId IS NULL
        //                                                                               GROUP BY TRXID,
        //                                                                                        PRODUCT_ID,
        //                                                                                        Remarks) tl ON b.TrxId = tl.TrxId
        //                                                                            INNER JOIN products p ON p.product_id = tl.product_id
        //                                                                            INNER JOIN ComboProduct cb ON cb.ChildProductId = tl.product_id
        //                                                                            LEFT OUTER JOIN ReservationDiscounts rd ON rd.productId = tl.product_id
        //                                                                            AND rd.BookingId = @BookingId
        //                                                                            WHERE cb.AdditionalProduct ='N'
        //                                                                              AND b.BookingId = @BookingId
        //                                                                              AND cb.Product_Id = b.BookingProductId",
        //                                                               new SqlParameter("@BookingId", BookingId));
        //    foreach (DataRow dr in dtBookedBookingProductDetails.Rows)
        //    {
        //        //bookingProductContents += "\t " + "Product Name: " + "[" + dr["product_name"].ToString() + "]" + "\t" + "Product Price: [" + dr["price"].ToString() + "]\t " + "Product Quantity: [" + dr["quantity"].ToString() + "]" + Environment.NewLine;
        //        bookingProductContents += "\t\t " + dr["product_name"].ToString() + " [Qty.: " + dr["quantity"].ToString() + "]" + Environment.NewLine;//Added for new format on 07-Mar-2017
        //        if (!string.IsNullOrEmpty(dr["Remarks"].ToString()))
        //        {
        //            bookingProductContents += " " + dr["Remarks"].ToString();
        //        }
        //        bookingProductContents += Environment.NewLine;
        //    }

        //    //foreach (DataGridViewRow dr in dgvBookingProducts.Rows)
        //    //{
        //    //    if (dr.IsNewRow)
        //    //        break;
        //    //    try
        //    //    {
        //    //        //additionalproducts += "\t Product Name: [" + dr.Cells["dcProduct"].FormattedValue.ToString() + "]\t Product Price: [" + dr.Cells["dcPrice"].Value.ToString() + "]\t \t Product Quantity: [" + dr.Cells["dcQuantity"].Value.ToString() + "]" + Environment.NewLine;
        //    //        additionalproducts += "\t " + dr.Cells["dcProduct"].FormattedValue.ToString() + " [Qty.: " + dr.Cells["dcQuantity"].Value.ToString() + "]" + Environment.NewLine;
        //    //        if (!string.IsNullOrEmpty(dr.Cells["dcRemarks"].FormattedValue.ToString()))
        //    //            additionalproducts += " " + dr.Cells["dcRemarks"].FormattedValue.ToString();
        //    //        additionalproducts += Environment.NewLine;
        //    //    }
        //    //    catch
        //    //    {
        //    //        log.Fatal("Ends-getBookingInfoForEmailPrint() due to exception in additionalproducts ");
        //    //    }
        //    //}



        //    //Added to get the Package Name if mutiple products are selected and Sum of Guest Count//

        //    string phoneNumber = string.Empty;
        //    string alternativePhoneNumber = string.Empty;
        //    if (customerDetailUI.CustomerDTO.ContactDTOList.Count > 0)
        //    {
        //        List<ContactDTO> phoneContactDTOList = (new List<ContactDTO>(customerDetailUI.CustomerDTO.ContactDTOList)).FindAll((x) => x.ContactType == ContactType.PHONE);
        //        if (phoneContactDTOList != null && phoneContactDTOList.Count > 0)
        //        {
        //            phoneNumber = phoneContactDTOList[0].Attribute1;
        //        }
        //        if (phoneContactDTOList != null && phoneContactDTOList.Count > 1)
        //        {
        //            alternativePhoneNumber = phoneContactDTOList[1].Attribute1;
        //        }
        //    }
        //    string address = string.Empty;
        //    if (customerDetailUI.CustomerDTO.AddressDTOList.Count > 0)
        //    {
        //        address = customerDetailUI.CustomerDTO.AddressDTOList[0].Line1;
        //    }
        //    string printText = "Booking (" + lblReservationCode.Text + ") at " + ParafaitEnv.SiteName + " " + lblStatus.Text + Environment.NewLine + Environment.NewLine +
        //                                        "Customer: " + customerDetailUI.FirstName + Environment.NewLine +
        //                                        "Phone: " + phoneNumber + (string.IsNullOrEmpty(alternativePhoneNumber) ? "" : "/" + alternativePhoneNumber) + Environment.NewLine +
        //                                        "Email: " + customerDetailUI.CustomerDTO.Email + Environment.NewLine +
        //                                        "Address: " + address + Environment.NewLine + Environment.NewLine +
        //                                        "Booking Details:" + Environment.NewLine +
        //                                        "\t Booking Code: " + lblReservationCode.Text + Environment.NewLine +
        //                                        "\t Booking Name: " + txtBookingName.Text + Environment.NewLine +
        //                                        "\t For Date: " + dtpforDate.Value.ToString(ParafaitEnv.DATE_FORMAT) + Environment.NewLine +
        //                                        "\t From Time: " + Utilities.getServerTime().Date.AddMinutes(fromtime).ToString("h:mm tt") + Environment.NewLine +
        //                                        "\t To Time: " + Utilities.getServerTime().Date.AddMinutes(totime).ToString("h:mm tt") + Environment.NewLine +
        //                                        "\t Package's: " + Environment.NewLine + bookingProductContents + Environment.NewLine +
        //                                        //g "\t Booking for: " + nudGuestCount.Value + Environment.NewLine + Environment.NewLine +
        //                                        "\t Booking for: " + txtGuests.Text + Environment.NewLine + Environment.NewLine +
        //                                        "Estimate Amount: " + txtBookingEstimate.Text + Environment.NewLine +
        //                                        "Advance Paid: " + txtPaidAmount.Text + Environment.NewLine +
        //                                        "Booking is " + lblStatus.Text + Environment.NewLine + Environment.NewLine;
        //    //nudQuantity.Value.ToString()
        //    //printText += "Package Details" + Environment.NewLine;
        //    //foreach (DataGridViewRow dr in dgvPackageDetails.Rows)
        //    //{
        //    //    printText += "\t " + dr.Cells["product_name"].Value.ToString() + Environment.NewLine;
        //    //}

        //    printText += Environment.NewLine + "Additional Items" + Environment.NewLine;
        //    foreach (DataGridViewRow dr in dgvBookingProducts.Rows)
        //    {
        //        if (dr.IsNewRow)
        //        {
        //            break;
        //        }
        //        try
        //        {
        //            printText += "\t " + dr.Cells["dcProduct"].FormattedValue.ToString() + " [Qty.: " + dr.Cells["dcQuantity"].Value.ToString() + "]";
        //            if (!string.IsNullOrEmpty(dr.Cells["dcRemarks"].FormattedValue.ToString()))
        //                printText += " " + dr.Cells["dcRemarks"].FormattedValue.ToString();
        //            printText += Environment.NewLine;
        //        }
        //        catch (Exception ex)
        //        {
        //            log.Error(ex);
        //        }
        //    }

        //    printText += Environment.NewLine + "Remarks" + Environment.NewLine;
        //    printText += "\t " + txtRemarks.Text;

        //    log.LogMethodExit(printText);
        //    return printText;
        //}

        private void btnPrint_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            txtMessage.Clear();
            if (BookingId != -1)
            {
                string reportFileName = GetBookingQuoteReport();
                using (PrintDocument pd = new PrintDocument())
                {
                    if (SetupThePrinting(pd))
                    {
                        try
                        {
                            PDFFilePrinter pdfFilePrinter = new PDFFilePrinter(pd, reportFileName);
                            pdfFilePrinter.SendPDFToPrinter();
                            pdfFilePrinter = null;

                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                            POSUtils.ParafaitMessageBox(MessageContainer.GetMessage(Utilities.ExecutionContext, 1819, ex.Message)); //Error while printing the document. Error message: &1
                        }
                    }
                }
            }
            log.LogMethodExit();
        }

        private string GetBookingQuoteReport()
        {
            log.LogMethodEntry();
            RunPOSReports runPOSReports = new RunPOSReports(Utilities);
            List<clsReportParameters.SelectedParameterValue> reportParam = new List<clsReportParameters.SelectedParameterValue>();
            reportParam.Add(new clsReportParameters.SelectedParameterValue("BookingId", BookingId));
            runPOSReports.GenearateBackgroundReport("BookingReceipt", "", DateTime.MinValue, DateTime.MinValue, reportParam);
            string reportFolderPath = Path.GetTempPath();
            string reportFileName = reportFolderPath +  "BookingDetails.pdf";
            log.LogMethodExit(reportFileName);
            return reportFileName;
        }


        private bool SetupThePrinting(PrintDocument MyPrintDocument)
        {
            log.LogMethodEntry(MyPrintDocument);
            //PrintDialog MyPrintDialog = new PrintDialog();
            //MyPrintDialog.AllowCurrentPage = false;
            //MyPrintDialog.AllowPrintToFile = false;
            //MyPrintDialog.AllowSelection = false;
            //MyPrintDialog.AllowSomePages = false;
            //MyPrintDialog.PrintToFile = false;
            //MyPrintDialog.ShowHelp = false;
            //MyPrintDialog.ShowNetwork = false;
            //MyPrintDialog.PrinterSettings.DefaultPageSettings.Landscape = false;
            //MyPrintDocument.DocumentName = MessageUtils.getMessage("Booking Details") + " " + Reservation.ReservationCode;
            //MyPrintDialog.UseEXDialog = true;

            //if (MyPrintDialog.ShowDialog() != DialogResult.OK)
            //{
            //    log.LogMethodExit("Print dialog was cancelled");
            //    return false;
            //}

            //MyPrintDocument.DocumentName = MessageUtils.getMessage("Booking Details") + " " + Reservation.ReservationCode;
            //MyPrintDocument.PrinterSettings = MyPrintDialog.PrinterSettings;
            //MyPrintDocument.DefaultPageSettings = MyPrintDialog.PrinterSettings.DefaultPageSettings;
            //MyPrintDocument.DefaultPageSettings.Margins =
            //                 new Margins(10, 10, 20, 20);

            log.LogMethodExit(true);
            return true;
        }

        private void CheckScheduleAvailablity()
        {
            log.LogMethodEntry();
            if (cmbBookingClass.SelectedValue == null || cmbFromTime.SelectedValue == null || cmbToTime.SelectedValue == null || cmbFacility.SelectedValue == null)
            {
                log.LogMethodExit("No value is selected: cmbBookingClass.SelectedValue == null || cmbFromTime.SelectedValue == null || cmbToTime.SelectedValue == null || cmbFacility.SelectedValue == null");
                return;
            }
            double lclTime = Convert.ToDouble(cmbFromTime.SelectedValue);
            DateTime ReservationFromDateTime = dtpforDate.Value.Date.AddMinutes(Math.Floor(lclTime) * 60).AddMinutes((lclTime - Math.Floor(lclTime)) * 100.0);

            lclTime = Convert.ToDouble(cmbToTime.SelectedValue);
            DateTime toDateTime = dtpforDate.Value.Date.AddMinutes(Math.Floor(lclTime) * 60).AddMinutes((lclTime - Math.Floor(lclTime)) * 100.0);

            int Duration = (int)(toDateTime - ReservationFromDateTime).TotalMinutes;

            DateTime ReservationToDateTime = ReservationFromDateTime.AddMinutes(Duration);
            //Added to send facility Id as parameter to get check availablity on Dec-07-2015//
            DataTable dt = new DataTable();// Reservation.getResourceAvailability(ReservationFromDateTime, ReservationFromDateTime, null, null, (int)cmbBookingClass.SelectedValue, facilityId);
            if (dt.Rows.Count == 0)
            {
                POSUtils.ParafaitMessageBox(MessageUtils.getMessage(327));
                log.LogMethodExit("Unable to find appropriate time slot for the given Date and Time");
                return;
            }

            int resQty = Convert.ToInt32(Utilities.executeScalar(@"select isnull(sum(quantity), 0) quantityBooked
                                                                    from Bookings
                                                                    where BookingProductId = @BookingProductId
                                                                    and BookingId != @BookingId
                                                                    and ((@TimeFrom < FromDate and @TimeTo > ToDate)
                                                                        or (@TimeFrom >= FromDate and @TimeFrom < ToDate)
                                                                        or (@TimeTo > FromDate and @TimeTo <= ToDate))
                                                                    and ((status in ('BOOKED', 'BLOCKED','WIP') and (ExpiryTime is null or ExpiryTime > getdate()))
                                                                        or status in ('CONFIRMED', 'COMPLETE'))",
                                                                new SqlParameter[] {new SqlParameter("@BookingProductId", cmbBookingClass.SelectedValue),
                                                                                    new SqlParameter("@BookingId", BookingId),
                                                                                    new SqlParameter("@TimeFrom", ReservationFromDateTime),
                                                                                    new SqlParameter("@TimeTo", ReservationToDateTime)}));

            DataRow dr = dt.Rows[0];
            int maxQty = Convert.ToInt32(dr["Quantity"]);

            double fromTime, toTime;
            if (dr["TimeFrom"] != DBNull.Value)
            {
                fromTime = Convert.ToDateTime(dr["TimeFrom"]).Hour * 60 + Convert.ToDateTime(dr["TimeFrom"]).Minute;
            }
            else
                fromTime = 0;

            if (dr["TimeTo"] != DBNull.Value)
            {
                toTime = Convert.ToDateTime(dr["TimeTo"]).Hour * 60 + Convert.ToDateTime(dr["TimeTo"]).Minute;
            }
            else
            {
                toTime = 0;
            }

            int minTime = 0;
            if (dr["MinimumTime"] != DBNull.Value)
            {
                minTime = Convert.ToInt32(dr["MinimumTime"]);
            }

            int maxTime = 0;
            if (dr["MaximumTime"] != DBNull.Value)
            {
                maxTime = Convert.ToInt32(dr["MaximumTime"]);
            }

            MessageBox.Show(cmbBookingClass.Text
                            + Environment.NewLine
                            //+ Environment.NewLine
                            //+ MessageUtils.getMessage("Booking Allowed from:") + " " + Utilities.getServerTime().Date.AddMinutes(fromTime).ToString("h:mm tt").Replace('.', ':') + " to " + Utilities.getServerTime().Date.AddMinutes(toTime).ToString("h:mm tt")
                            + Environment.NewLine
                            + MessageUtils.getMessage("Max Allowed Bookings:") + " " + maxQty.ToString()
                            + Environment.NewLine
                            + MessageUtils.getMessage("Already Booked:") + " " + resQty.ToString()
                            + Environment.NewLine
                            + MessageUtils.getMessage("Minimum booking duration (Mins):") + " " + minTime.ToString()
                            + Environment.NewLine
                            + MessageUtils.getMessage("Maximum booking duration (Mins):") + " " + maxTime.ToString());
            log.LogMethodExit();
        }

        private void txtCardNumber_Validating(object sender, CancelEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ValidateCard();
            log.LogMethodExit();
        }

        private void ValidateCard()
        {
            log.LogMethodEntry();
            txtMessage.Text = "";
            if ((BookingId == -1 || lblStatus.Text.ToUpper() == ReservationDTO.ReservationStatus.BLOCKED.ToString()) && !string.IsNullOrEmpty(txtCardNumber.Text.Trim()))
            {
                Card card = new Card(txtCardNumber.Text.Trim(), "", Utilities);
                if (card.CardStatus.Equals("NEW"))
                {
                    txtMessage.Text = MessageUtils.getMessage(172);
                    txtCardNumber.Text = "";
                    log.LogMethodExit("Card is Invalid");
                    return;
                }
                else if (card.technician_card.Equals('N') == false)
                {
                    txtMessage.Text = MessageUtils.getMessage(197, txtCardNumber.Text);
                    txtCardNumber.Text = "";
                    log.LogMethodExit("Tech card used");
                    return;
                }
                else if (card.customer_id > 0)
                {
                    CustomerDTO customerDTO = (new CustomerBL(Utilities.ExecutionContext, card.customer_id)).CustomerDTO;
                    customerDetailUI.CustomerDTO = customerDTO;
                }
            }
            log.LogMethodExit();
            return;
        }

        private void frmMakeReservation_FormClosing(object sender, FormClosingEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Common.Devices.UnregisterCardReaders(); //Unregister the card reader so that main application takes over 
            log.LogMethodExit();
        }

        //Begin: open the Additional products form to add Additional products//
        private void OpenAdditionalProductsForm()
        {
            log.LogMethodEntry();
            if (cmbBookingClass.SelectedValue != null)
            {
                using (frmAdditionalProducts fap = new frmAdditionalProducts(Convert.ToInt32(cmbBookingClass.SelectedValue), Utilities))
                {
                    fap.ReturnData = new List<frmAdditionalProducts.clsReturnData>();
                    foreach (DataGridViewRow dr in dgvBookingProducts.Rows)
                    {
                        if (dr.IsNewRow || dr.Cells["dcProduct"].Value == null)
                        {
                            break;
                        }
                        if (dr.Cells["dcProduct"].Value != null && dr.Cells["type"].Value != null && dr.Cells["type"].Value.ToString() == "SERVICECHARGE")
                        {
                            continue;
                        }
                        frmAdditionalProducts.clsReturnData prod = new frmAdditionalProducts.clsReturnData();
                        prod.ProductId = dr.Cells["dcProduct"].Value;
                        prod.productName = dr.Cells["dcProduct"].FormattedValue.ToString();
                        prod.quantity = Convert.ToDecimal(dr.Cells["dcQuantity"].Value);
                        prod.productType = dr.Cells["type"].Value.ToString();
                        if (dr.Cells["dcPrice"].Value != null)
                            prod.Price = Convert.ToDecimal(dr.Cells["dcPrice"].Value);
                        else
                            prod.Price = 0;

                        //Starts Modification for multiple Display group enhanacement 
                        prod.displayGroup = Utilities.executeScalar(@"select DISTINCT  CASE when cp.DisplaygroupId is null and pd.DisplayGroupId is null then
			                                                  'Other' else pdgf.DisplayGroup end 
			                                                  from products p inner join  ComboProduct cp on p.product_id = cp.ChildProductId
			                                                  Left join (SELECT * from (SELECT *, DENSE_RANK() over (partition by ProductId order by CreatedDate ASC) as D from ProductsDisplayGroup)T
							                                                WHERE T.D = 1) pd  
			                                                on p.product_id = pd.ProductId		
			                                                left  join ProductDisplayGroupFormat pdgf
			                                                on pdgf.Id = ISNULL(cp.DisplaygroupId, isnull(pd.DisplayGroupId, (Select top 1 Id from ProductDisplayGroupFormat)))										
			                                                  where cp.Product_Id = @BookingProductId									 
				                                                and cp.AdditionalProduct ='Y'
				                                                and p.active_flag = 'Y'
				                                                and p.product_id = @productid",
                                                       new SqlParameter("@productId", prod.ProductId),
                                                       new SqlParameter("@BookingProductId", cmbBookingClass.SelectedValue)).ToString();
                        fap.ReturnData.Add(prod);
                        //End Modification for multiple Display group enhanacement 
                    }

                    if (fap.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        BuildProducInformationLst();
                        DataGridViewRow serviceChargeRow = GetServiceChargeRow();
                        List<DataGridViewRow> newDataRowsLst = new List<DataGridViewRow>();
                        //dgvBookingProducts.Rows.Clear();
                        lstPurchasedAdditionalModifierProducts.Clear();
                        lstAdditionalAttractionProductsList.Clear();
                        //int newLineId = 99999;
                        foreach (frmAdditionalProducts.clsReturnData prod in fap.ReturnData)
                        {
                            bool found = false;
                            List<Semnox.Parafait.Transaction.Reservation.clsProductList> adInfoLst = null;
                            foreach (DataGridViewRow dr in dgvBookingProducts.Rows)
                            {
                                adInfoLst = null;
                                if (dr.IsNewRow || dr.Cells["dcProduct"].Value == null)
                                { break; }
                                if (dr.Cells["dcProduct"].Value != null && dr.Cells["type"].Value != null && dr.Cells["type"].Value.ToString() == "SERVICECHARGE")
                                {
                                    continue;
                                }
                                if (prod.ProductId.Equals(dr.Cells["dcProduct"].Value))
                                {
                                    //found = true;
                                    adInfoLst = GetProducInformationLst(Convert.ToInt32(dr.Cells["dcProduct"].Value), Convert.ToInt32(dr.Cells["LineId"].Value));
                                    dgvBookingProducts.Rows.RemoveAt(dr.Index);
                                    break;
                                }
                            }

                            if (!found)
                            {
                                // dgvBookingProducts.Rows.Add(null, prod.ProductId, nudQuantity.Value, prod.Price);
                                //Commented above line and added the below code to accept the quantity value from Numberpad on May 13, 2015//
                                //  dgvBookingProducts.Rows.Add(delete btn , productId, qty, price, prod type, bookingProdId, discountName, trxprofId, remarks
                                //dgvBookingProducts.Rows.Add(null, prod.ProductId, prod.quantity, prod.Price, prod.productType, cmbBookingClass.SelectedValue, -1, -1, "");
                                DataGridViewRow dgvNewRow = (DataGridViewRow)dgvBookingProducts.Rows[0].Clone();
                                int currentLineId;
                                dgvNewRow.Cells[0].Value = null;
                                dgvNewRow.Cells[1].Value = prod.ProductId;
                                dgvNewRow.Cells[2].Value = prod.quantity;
                                dgvNewRow.Cells[3].Value = prod.Price;
                                dgvNewRow.Cells[4].Value = prod.productType;
                                dgvNewRow.Cells[5].Value = cmbBookingClass.SelectedValue;
                                if (adInfoLst == null)
                                {
                                    dgvNewRow.Cells[6].Value = -1;
                                    dgvNewRow.Cells[7].Value = -1;
                                    dgvNewRow.Cells[8].Value = "";
                                    dgvNewRow.Cells[9].Value = newLineId;
                                    currentLineId = newLineId;
                                    newLineId--;
                                }
                                else
                                {
                                    dgvNewRow.Cells[6].Value = adInfoLst[0].discountId;
                                    dgvNewRow.Cells[7].Value = adInfoLst[0].transactionProfileId;
                                    dgvNewRow.Cells[8].Value = adInfoLst[0].Remarks;
                                    dgvNewRow.Cells[9].Value = adInfoLst[0].lineId;
                                    currentLineId = adInfoLst[0].lineId;
                                }
                                List<KeyValuePair<PurchasedProducts, int>> productModifierLst = new List<KeyValuePair<PurchasedProducts, int>>();
                                List<KeyValuePair<AttractionBooking, int>> attractionProductsList = new List<KeyValuePair<AttractionBooking, int>>();
                                bool alreadyAdded = true;
                                //Begin: Added to Load Attraction Schedule PopUp on 17-Jun-2016
                                if (prod.productType == "ATTRACTION")
                                {
                                    alreadyAdded = PresentInAdditionalProductATSList(Convert.ToInt32(prod.ProductId), Convert.ToInt32(prod.quantity), currentLineId);
                                    if (alreadyAdded == false)
                                    {
                                        Card card = null;
                                        DataTable attractionProductDet = trxUtils.getProductDetails(Convert.ToInt32(prod.ProductId), card);

                                        if (attractionProductDet.Rows.Count > 0)
                                        {
                                            //if (attractionProductDet.Rows[0]["QuantityPrompt"].ToString() == "Y")
                                            //{
                                            //    int attractionQuantity = productQuantity;
                                            //    while (attractionQuantity > 0)
                                            //    {
                                            //        createAttractionProduct(Convert.ToInt32(prod.ProductId), 0, 1, -1, lstAdditionalAttractionProductsList);//Combo product Id is sent -1, since its not part of Combo
                                            //        attractionQuantity--;
                                            //    }
                                            //}
                                            //else
                                            {
                                                // RemoveFromAdditionalProductLists(Convert.ToInt32(prod.ProductId), Convert.ToInt32(prod.quantity),currentLineId);
                                                CreateAttractionProduct(-1, Convert.ToInt32(prod.ProductId), 0, Convert.ToInt32(prod.quantity), -1, attractionProductsList);

                                            }
                                        }
                                    }
                                }
                                //End: Added to Load Attraction Schedule PopUp on 17-Jun-2016
                                //Begin: Modified for adding the Product Modifier part on 04-Oct-2016
                                else if (prod.productType == "MANUAL")
                                {
                                    alreadyAdded = PresentInAdditionalProductModifierList(Convert.ToInt32(prod.ProductId), Convert.ToInt32(prod.quantity), currentLineId);
                                    if (alreadyAdded == false)
                                    {
                                        DataTable dt = Utilities.executeDataTable(@"SELECT pm.ModifierSetId, SetName, pm.AutoShowInPos 
                                                                                from ProductModifiers pm, ModifierSet ms 
                                                                                where ms.ModifierSetId = pm.ModifierSetId 
                                                                                and pm.ProductId = @ProductId", new SqlParameter("@ProductId", prod.ProductId));
                                        //bool modifiersSelected = purchasedAdditionalProduct.Exists(prod => prod.Value == Convert.ToInt32(prod.ProductId));
                                        if (dt.Rows.Count != 0)//&& purchasedAdditionalProduct.Count == 0)
                                        {
                                            int ProductQuantity = Convert.ToInt32(prod.quantity);
                                            //RemoveFromAdditionalProductLists(Convert.ToInt32(prod.ProductId), ProductQuantity);                                       
                                            while (ProductQuantity > 0)
                                            {
                                                //Modification for F&B Restructuring of modifiers on 17-Oct-2018
                                                using (FrmProductModifier frmProductModifier = new FrmProductModifier(Convert.ToInt32(prod.ProductId), prod.productType, Utilities, null))
                                                {
                                                    frmProductModifier.ShowDialog();
                                                    Transaction trx = new Transaction();
                                                    frmProductModifier.transactionModifier.purchasedProducts.PurchasedModifierSetDTOList = trx.LoadSelectedModifiers(frmProductModifier.transactionModifier.ModifierSetDTO);
                                                    //lstPurchasedAdditionalModifierProducts.Add(new KeyValuePair<PurchasedProducts, int>(frmProductModifier.transactionModifier.purchasedProducts, Convert.ToInt32(prod.ProductId)));
                                                    productModifierLst.Add(new KeyValuePair<PurchasedProducts, int>(frmProductModifier.transactionModifier.purchasedProducts, Convert.ToInt32(prod.ProductId)));
                                                }
                                                ProductQuantity--;
                                            }
                                        }
                                    }
                                }
                                if (adInfoLst == null)
                                {
                                    adInfoLst = new List<Semnox.Parafait.Transaction.Reservation.clsProductList>();
                                    adInfoLst.Add(new Semnox.Parafait.Transaction.Reservation.clsProductList());
                                    adInfoLst[0].ProductId = Convert.ToInt32(dgvNewRow.Cells[1].Value);
                                    adInfoLst[0].Quantity = Convert.ToInt32(dgvNewRow.Cells[2].Value);
                                    adInfoLst[0].Price = Convert.ToDecimal(dgvNewRow.Cells[3].Value);
                                    adInfoLst[0].productType = dgvNewRow.Cells[4].Value;
                                    adInfoLst[0].discountId = (dgvNewRow.Cells[6].Value == null) ? -1 : Convert.ToInt32(dgvNewRow.Cells[6].Value);
                                    adInfoLst[0].transactionProfileId = (dgvNewRow.Cells[7].Value == null) ? -1 : Convert.ToInt32(dgvNewRow.Cells[7].Value);
                                    adInfoLst[0].Remarks = dgvNewRow.Cells[8].Value;
                                    adInfoLst[0].lineId = currentLineId;
                                    adInfoLst[0].purchasedModifierLst = productModifierLst;
                                    adInfoLst[0].attractionProductLst = attractionProductsList;
                                    UpdateProducInformationLst(adInfoLst[0]);
                                }
                                else
                                {
                                    adInfoLst[0].Quantity = Convert.ToInt32(dgvNewRow.Cells[2].Value);
                                    adInfoLst[0].Price = Convert.ToDecimal(dgvNewRow.Cells[3].Value);
                                    if (prod.productType == "MANUAL" && alreadyAdded == false)
                                    {
                                        adInfoLst[0].purchasedModifierLst = productModifierLst;
                                    }
                                    if (prod.productType == "ATTRACTION" && alreadyAdded == false)
                                    {
                                        adInfoLst[0].attractionProductLst = attractionProductsList;
                                    }
                                    UpdateProducInformationLst(adInfoLst[0]);
                                }
                                //End: Modified for adding the Product Modifier part on 04-Oct-2016
                                newDataRowsLst.Add(dgvNewRow);
                            }
                        }
                        dgvBookingProducts.Rows.Clear();
                        if (newDataRowsLst != null && newDataRowsLst.Count > 0)
                        {
                            dgvBookingProducts.Rows.AddRange(newDataRowsLst.ToArray());
                        }
                        if (serviceChargeRow != null)
                        {
                            for (int i = 0; i < dgvBookingProducts.Rows.Count; i++)
                            {
                                if (dgvBookingProducts.Rows[i].IsNewRow || dgvBookingProducts.Rows[i].Cells["dcProduct"].Value == null)
                                {
                                    dgvBookingProducts.Rows.Insert(i, serviceChargeRow);
                                    break;
                                }
                            }
                        }
                        //remove entries for unselected products.
                        RemoveFromAdditionalProductInformationLst();
                        BuildModifierLstFromProductInformationLst();
                        BuildAttractionSchedulesLstFromProductInformationLst();
                    }
                }
            }
            else
            {
                txtMessage.Text = MessageContainer.GetMessage(Utilities.ExecutionContext, 1796); //"Please select a booking product"
                log.Info("Booking product is not Selected");
            }
            log.LogMethodExit();
        }



        private DataGridViewRow GetServiceChargeRow()
        {
            log.LogMethodEntry();
            DataGridViewRow serviceChargeRow = null;
            foreach (DataGridViewRow item in dgvBookingProducts.Rows)
            {
                if (item.Cells["dcProduct"].Value != null && item.Cells["type"].Value != null && item.Cells["type"].Value.ToString() == "SERVICECHARGE")
                {
                    serviceChargeRow = item;
                    break;
                }
            }
            log.LogMethodExit(serviceChargeRow);
            return serviceChargeRow;
        }

        //End

        private void btnAddProducts_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            txtMessage.Clear();
            //open the Additional products form to add Additional products
            if (dtProducts.Rows.Count > 1)
            {
                OpenAdditionalProductsForm();//open the Additional products form to add Additional products
            }
            log.LogMethodExit();
        }

        private void btnNextDateTime_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            txtMessage.Clear();
            try
            {
                ValidateDateAndTimePageData();
                if (tcBooking.TabPages.Contains(tpCustomer))
                    tcBooking.SelectedTab = tpCustomer;
                else
                    tcBooking.TabPages.Add(tpCustomer);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void ValidateDateAndTimePageData()
        {
            log.LogMethodEntry();
            txtMessage.Clear();
            int bookedquantity = 0;
            decimal toTime = Convert.ToDecimal(cmbToTime.SelectedValue);
            decimal fromTime = Convert.ToDecimal(cmbFromTime.SelectedValue);

            double lclTime = Convert.ToDouble(cmbFromTime.SelectedValue);
            double tolclTime = Convert.ToDouble(cmbToTime.SelectedValue);
            DateTime fromDateTime = dtpforDate.Value.Date.AddMinutes(Math.Floor(lclTime) * 60).AddMinutes((lclTime - Math.Floor(lclTime)) * 100.0);
            DateTime toDateTime;
            if (txtBookingName.Text.Trim() == "")
            {
                txtMessage.Text = MessageUtils.getMessage(302);
                log.Info("Booking Name was not Entered");
                this.ActiveControl = txtBookingName;
                throw new Exception(txtMessage.Text);
            }
            if (cmbChannel.SelectedItem == null || cmbChannel.SelectedIndex == -1)
            {
                txtMessage.Text = MessageUtils.getMessage(306);
                log.Info("Channel was not Selected");
                this.ActiveControl = cmbChannel;
                throw new Exception(txtMessage.Text);
            }
            if (cmbBookingClass.SelectedValue == null || cmbBookingClass.SelectedIndex == 0)
            {
                txtMessage.Text = MessageContainer.GetMessage(Utilities.ExecutionContext, 1796); //"Please select a booking product"
                log.Info("Booking product is not Selected");
                this.ActiveControl = cmbBookingClass;
                throw new Exception(txtMessage.Text);
            }

            if (MoreThanOneScheduleSelected())
            {
                txtMessage.Text = MessageContainer.GetMessage(Utilities.ExecutionContext, 1797); // "Please select one schedule entry"
                this.ActiveControl = dgvAttractionSchedules;
                log.Info("More than one schedule selected");
                throw new Exception(txtMessage.Text); ;
            }

            if (tolclTime > lclTime)
            {
                toDateTime = dtpforDate.Value.Date.AddMinutes(Math.Floor(tolclTime) * 60).AddMinutes((tolclTime - Math.Floor(tolclTime)) * 100.0);
            }
            else
            {
                toDateTime = dtpforDate.Value.Date.AddDays(1).AddMinutes(Math.Floor(tolclTime) * 60).AddMinutes((tolclTime - Math.Floor(tolclTime)) * 100.0);
            }
            if (BookingId == -1 || isEditedBooking || lblStatus.Text.ToUpper() == ReservationDTO.ReservationStatus.BLOCKED.ToString())
            {
                //Checks if  schedule is selected
                if (attractionScheduleId == -1)
                {
                    txtMessage.Text = MessageUtils.getMessage(943);
                    POSUtils.ParafaitMessageBox(txtMessage.Text);
                    log.LogMethodExit("Schedule was not selected");
                    throw new Exception(txtMessage.Text);
                }
                //end
                GetBookingProductsDefinition();

                //Begin:validate request  to check for Availablity
                //Begin Modification 18-Dec-2015 -Added the below code to dispaly message with Booking details like quantity booked, From date, To date//
                DataTable bookingDt = Utilities.executeDataTable(@"select ReservationCode,  Quantity, FromDate, ToDate
                                                                     from Bookings
                                                                                where BookingProductId = @bookingProductId
                                                                                and BookingId != @BookingId
                                                                                and ((@TimeFrom < FromDate and @TimeTo > ToDate)
                                                                                    or (@TimeFrom >= FromDate and @TimeFrom < ToDate)
                                                                                    or (@TimeTo > FromDate and @TimeTo <= ToDate))
                                                                                and ((status in ('BOOKED','BLOCKED') and (ExpiryTime is null or ExpiryTime > getdate()))
                                                                                or (status = 'WIP' and (ExpiryTime is null or ExpiryTime > getdate()))
                                                                                    or status in ('COMPLETE', 'CONFIRMED')) and AttractionScheduleId = @attractionScheduleId",
                                                                   new SqlParameter[] {new SqlParameter("@bookingProductId", (int)cmbBookingClass.SelectedValue),
                                                                                    new SqlParameter("@BookingId", BookingId),
                                                                                    new SqlParameter("@TimeFrom", fromDateTime),
                                                                                    new SqlParameter("@TimeTo", toDateTime),
                                                                                    new SqlParameter("@attractionScheduleId",attractionScheduleId)
                                                                                    });
                if (bookingDt.Rows.Count > 0)
                {
                    if (Utilities.getParafaitDefaults("ALLOW_MULTIPLE_BOOKINGS_WITHIN_SCHEDULE").Equals("Y"))
                    {
                        foreach (DataRow dr in bookingDt.Rows)
                        {
                            bookedquantity += Convert.ToInt32(dr["Quantity"]);
                        }
                        if (MessageBox.Show(MessageContainer.GetMessage(Utilities.ExecutionContext, 1798, Environment.NewLine, bookingDt.Rows.Count, bookedquantity,
                                                                        Convert.ToDateTime(bookingDt.Rows[0]["FromDate"]).ToString("MMM-dd-yyyy"),
                                                                        Convert.ToDateTime(bookingDt.Rows[0]["FromDate"]).ToShortTimeString(),
                                                                        Convert.ToDateTime(bookingDt.Rows[0]["ToDate"]).ToShortTimeString()), "Edit Booking", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                        {
                            if (!ValidateRequest(Convert.ToInt32(cmbBookingClass.SelectedValue), Convert.ToInt32(txtGuests.Text), cmbBookingClass.Text))
                            {
                                log.Info("Unable to validateRequest");
                                throw new Exception(txtMessage.Text);
                            }
                        }
                        else
                        {
                            txtMessage.Text = MessageUtils.getMessage(1799); // "User does not want to continue"
                            log.Info("Edit booking ,no was clicked ");
                            throw new Exception(txtMessage.Text);
                        }
                    }
                    else
                    {
                        if (!ValidateRequest(Convert.ToInt32(cmbBookingClass.SelectedValue), Convert.ToInt32(txtGuests.Text), cmbBookingClass.Text))
                        {
                            log.Info("Unable to validateRequest");
                            throw new Exception(txtMessage.Text);
                        }
                    }
                }
                else
                {
                    if (!ValidateRequest(Convert.ToInt32(cmbBookingClass.SelectedValue), Convert.ToInt32(txtGuests.Text), cmbBookingClass.Text))
                    {
                        log.Info("Unable to validateRequest");
                        throw new Exception(txtMessage.Text);
                    }
                }
                //End Modification 18-Dec-2015//
                //end
            }
            if (toTime < 6 && fromTime > 6)
            {
            }
            else if (toTime <= fromTime)
            {
                txtMessage.Text = MessageUtils.getMessage(305);
                this.ActiveControl = cmbToTime;
                log.LogMethodExit("To Time should be greater than From Time");
                throw new Exception(txtMessage.Text);
            }
            if (fromTime == 0)
            {
                txtMessage.Text = MessageUtils.getMessage("Select From Time");
                this.ActiveControl = cmbFromTime;
                log.LogMethodExit("From Time was not selected");
                throw new Exception(txtMessage.Text);
            }
            else if (toTime == 0)
            {
                txtMessage.Text = MessageUtils.getMessage("Select To Time");
                this.ActiveControl = cmbToTime;
                log.LogMethodExit("To Time was not selected");
                throw new Exception(txtMessage.Text);
            }
            log.LogMethodExit();
        }

        private bool MoreThanOneScheduleSelected()
        {
            log.LogMethodEntry();
            bool moreThanOneScheduleSelected = true;
            int selectedRows = 0;
            foreach (DataGridViewRow selectedRow in dgvAttractionSchedules.Rows)
            {
                if (selectedRow.Cells["Selected"] != null && selectedRow.Cells["Selected"].Value.ToString() == "1")
                {
                    selectedRows++;
                }
                if (selectedRows > 1)
                    break;
            }
            if (selectedRows == 1)
            {
                moreThanOneScheduleSelected = false;
            }
            log.LogMethodExit(moreThanOneScheduleSelected);
            return moreThanOneScheduleSelected;
        }

        private void btnNextCustomer_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            txtMessage.Clear();
            if (ValidateCustomer() == false)
            {
                log.LogMethodExit("ValidateCustomer() == false");
                return;
            }
            // GetBookedBookingProductItems();//Gets the product details while editing a reservation
            if (BookingId != -1 && (lblStatus.Text.ToUpper() == ReservationDTO.ReservationStatus.BLOCKED.ToString()
                                    || lblStatus.Text.ToUpper() == ReservationDTO.ReservationStatus.WIP.ToString()))
            {
                //Reservation.UpdateReservation(txtBookingName.Text,
                //                                txtRemarks.Text,
                //                                ParafaitEnv.LoginID,
                //                                cmbChannel.SelectedItem.ToString(),
                //                                customerDetailUI.CustomerDTO);
            }
            tcBooking.SelectedTab = tpPackage;
            log.LogMethodExit();
        }

        //Begin: Gets the products details while editing a reservation, ie Booked product details based on the booking id
        private void GetBookedBookingProductItems()
        {
            log.LogMethodEntry();
            //Begin: To populate the products after reservation is booked/confirmed
            if (BookingId != -1 && lblStatus.Text.ToUpper() != ReservationDTO.ReservationStatus.BLOCKED.ToString())
            {

                object Price = Utilities.executeScalar(@"select amount from trx_lines tl, bookings b where tl.TrxId  = b.trxid and b.BookingId = @id and b.BookingProductId =tl.product_id", new SqlParameter("@id", BookingId));
                bookingProductPrice = Convert.ToDouble(Price);
                DataTable dt = Utilities.executeDataTable(@"SELECT b.BookingProductId,
                                                                   tl.product_id,
                                                                   p.product_name,
                                                                   tl.Quantity,
                                                                   tl.Remarks,
                                                                   rd.ReservationDiscountId discountId,
                                                                   tl.TrxProfileId 
                                                            FROM 
                                                                 Bookings b
                                                            INNER JOIN
                                                              (SELECT TRXID,
                                                                      PRODUCT_ID,
                                                                      remarks,
                                                                      TrxProfileId,
                                                                      SUM(TL.QUANTITY) Quantity
                                                               FROM trx_lines tl
                                                               WHERE tl.ParentLineId IS NULL
                                                               GROUP BY TRXID,
                                                                        PRODUCT_ID,
                                                                        Remarks,
                                                                        TrxProfileId) tl ON b.TrxId = tl.TrxId
                                                            INNER JOIN products p ON p.product_id = tl.product_id
                                                            INNER JOIN ComboProduct cb ON cb.ChildProductId = tl.product_id
                                                            LEFT OUTER JOIN ReservationDiscounts rd ON rd.productId = tl.product_id
                                                            AND rd.BookingId = @id
                                                            WHERE ISNULL(cb.AdditionalProduct,'N') ='N'
                                                              AND b.BookingId = @id
                                                              AND cb.Product_Id = b.BookingProductId",
                                                           new SqlParameter[] { new SqlParameter("@id", BookingId) });
                //Added the below code to get the package details while editing the reservation on June 6,2015//

                try
                {
                    //if(dt.Rows.Count > 0)
                    //{
                    //    for (int k = 0; k < dgvPackageList.Rows.Count; k++)
                    //    {
                    //        DataGridViewCheckBoxCell chkPackageproductId = (DataGridViewCheckBoxCell)dgvPackageList.Rows[k].Cells["SelectProduct"];
                    //        chkPackageproductId.Value = "N";
                    //        dgvPackageList.Rows[k].Cells["SelectedStatus"].Value = "false";
                    //    }
                    //}
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int k = 0; k < dgvPackageList.Rows.Count; k++)
                        {
                            if (cmbBookingClass.SelectedValue != null && Convert.ToInt32(cmbBookingClass.SelectedValue.ToString()).CompareTo(dt.Rows[i]["BookingProductId"]) == 0)
                            {
                                if (Convert.ToInt32(dt.Rows[i]["product_id"].ToString()).CompareTo(Convert.ToInt32(dgvPackageList.Rows[k].Cells["ChildId"].Value)) == 0)
                                {
                                    GetDiscounts(Convert.ToInt32(dgvPackageList.Rows[k].Cells["ChildId"].Value), k);//get the discounts and load it to the drop down
                                    DataGridViewCheckBoxCell chkPackageproductId = (DataGridViewCheckBoxCell)dgvPackageList.Rows[k].Cells["SelectProduct"];
                                    chkPackageproductId.Value = "Y";
                                    dgvPackageList.Rows[k].Cells["SelectedStatus"].Value = "true";
                                    dgvPackageList.Rows[k].Cells["discountName"].Value = dt.Rows[i]["discountId"];
                                    dgvPackageList.Rows[k].Cells["transactionProfileId"].Value = dt.Rows[i]["TrxProfileId"];
                                    dgvPackageList.Rows[k].Cells["Quantity"].Value = dt.Rows[i]["quantity"];
                                    dgvPackageList.Rows[k].Cells["Remarks"].Value = dt.Rows[i]["Remarks"];
                                    dgvPackageList.CurrentCell = dgvPackageList.Rows[k].Cells["packageName"];
                                    dgvPackageDetails.Focus();
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
                //getPackageContents();//get the contents of the products selected
            }
            //end: populate the products
            log.LogMethodExit();
        }


        private void btnPrevCustomer_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            txtMessage.Clear();
            tcBooking.SelectedTab = tpDateTime;
            log.LogMethodExit();
        }

        private void btnNextPackage_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            txtMessage.Clear();
            int quantity = 0;
            string packageName = "";
            bool selected = false;
            if(cmbBookingClass.SelectedValue == null)
            {
                txtMessage.Text = MessageUtils.getMessage(1796);
                return;
            }

            /*Added to enable multiple package selection for reservation on June 6, 2015*/
            if (lstPacakgeIdList != null)
            {
                lstPacakgeIdList.Clear();
                lstPacakgeIdList = null;
            }
            //Begin:If Confirm tab is selected after selecting the products without selecting Additional products
            if (lstPacakgeIdList == null)
            {
                lstPacakgeIdList = new List<Semnox.Parafait.Transaction.Reservation.clsPackageList>();
                foreach (DataGridViewRow packageRow in dgvPackageList.Rows)
                {
                    if (packageRow.Cells["packageName"].Value != DBNull.Value)
                    {
                        if ((packageRow.Cells["packageName"].Value != null) && (packageRow.Cells["Quantity"].Value != null)
                            && packageRow.Cells["SelectProduct"].Value != null && packageRow.Cells["SelectProduct"].Value.ToString() == "Y")
                        {
                            packageIdList = new Semnox.Parafait.Transaction.Reservation.clsPackageList();
                            packageName = packageRow.Cells["packageName"].FormattedValue.ToString();
                            packageIdList.productId = Convert.ToInt32(packageRow.Cells["ChildId"].Value);
                            packageIdList.bookingProductId = Convert.ToInt32(cmbBookingClass.SelectedValue);
                            packageIdList.guestQuantity = Convert.ToInt32(packageRow.Cells["Quantity"].Value);
                            if (packageRow.Cells["discountName"].Value == DBNull.Value)
                                packageIdList.discountId = -1;
                            else if (Convert.ToInt32(packageRow.Cells["discountName"].Value) < 0)
                                packageIdList.discountId = -1;
                            else
                                packageIdList.discountId = Convert.ToInt32(packageRow.Cells["discountName"].Value);

                            if (packageRow.Cells["transactionProfileId"].Value == DBNull.Value)
                                packageIdList.transactionProfileId = -1;
                            else if (Convert.ToInt32(packageRow.Cells["transactionProfileId"].Value) < 0)
                                packageIdList.transactionProfileId = -1;
                            else
                                packageIdList.transactionProfileId = Convert.ToInt32(packageRow.Cells["transactionProfileId"].Value);

                            packageIdList.productType = packageRow.Cells["ProductType"].Value.ToString();
                            packageIdList.priceInclusive = packageRow.Cells["PriceInclusive"].Value.ToString();
                            packageIdList.productPrice = packageRow.Cells["price"].Value == DBNull.Value ? 0.0 : Convert.ToDouble(packageRow.Cells["price"].Value);
                            packageIdList.remarks = packageRow.Cells["Remarks"].Value == null ? null : packageRow.Cells["Remarks"].Value.ToString();
                            lstPacakgeIdList.Add(packageIdList);
                            quantity += Convert.ToInt32(packageRow.Cells["Quantity"].Value);
                            //Begin -Jan-27-2016- Removed qty check//
                            //if (Convert.ToInt32(packageRow.Cells["Quantity"].Value) > Convert.ToInt32(nudGuestCount.Value))
                            //{
                            //    POSUtils.ParafaitMessageBox(MessageUtils.getMessage(945) + packageName);
                            //    return;
                            //}
                            //End -Jan-27-2016- Removed qty check//
                            //Add physical cards if combo product has products of type 'NEW', 'CARDSALE', 'RECHARGE', 'GAMETIME'//
                            if (packageRow.Cells["ProductType"].Value.ToString() == "COMBO")
                            {
                                //load the booked category products
                                if (CategoryProuctFinalSelectedList.Count == 0 && BookingId != -1)
                                {
                                    Semnox.Parafait.Transaction.Reservation.clsSelectedCategoryProducts selectedProducts = new Semnox.Parafait.Transaction.Reservation.clsSelectedCategoryProducts();
                                    DataTable selectedCategoryProducts = POSStatic.Utilities.executeDataTable(@"Select tt.product_id,tt.quantity from trx_lines tt,  
                                                                                        (select LineId,tl.TrxId from trx_lines tl, bookings b where tl.trxid  = b.TrxId
                                                                                        and b.BookingId = @bookingId
                                                                                        and tl.product_id = @ComboProductId)a
                                                                                        where a.LineId = tt.ParentLineId and tt.TrxId = a.TrxId 
                                                                                        and 
                                                                                        not exists(Select cp.Product_Id from ComboProduct cp where 
                                                                                        cp.ChildProductId = tt.product_id)",
                                                                                                        new SqlParameter("@bookingId", BookingId),
                                                                                                        new SqlParameter("@ComboProductId", packageIdList.productId));
                                    if (selectedCategoryProducts.Rows.Count > 0)
                                    {
                                        foreach (DataRow categoryProduct in selectedCategoryProducts.Rows)
                                        {
                                            selectedProducts.productId = Convert.ToInt32(categoryProduct["product_id"].ToString());
                                            selectedProducts.parentComboProductId = packageIdList.productId;
                                            selectedProducts.quantity = Convert.ToInt32(categoryProduct["quantity"]);
                                            CategoryProuctFinalSelectedList.Add(selectedProducts);
                                        }
                                    }
                                }
                                //end:load the booked category products
                            }
                            //Begin: Modified for adding the Product Modifier part on 04-Oct-2016
                            else if (packageRow.Cells["ProductType"].Value.ToString() == "MANUAL")
                            {
                                DataTable dt = Utilities.executeDataTable(@"SELECT pm.ModifierSetId, SetName, pm.AutoShowInPos 
                                                                                from ProductModifiers pm, ModifierSet ms 
                                                                                where ms.ModifierSetId = pm.ModifierSetId 
                                                                                and pm.ProductId = @ProductId", new SqlParameter("@ProductId", packageIdSelected));

                                bool modifiersSelected = PresentInModifierProductList(lstPurchasedModifierProducts, packageIdSelected);
                                if (dt.Rows.Count != 0 && modifiersSelected == false)//lstProductModifiersSelectedList == null
                                {
                                    int ProductQuantity = Convert.ToInt32(packageRow.Cells["Quantity"].Value);
                                    while (ProductQuantity > 0)
                                    {
                                        //Modification for F&B Restructuring of modifiers on 17-Oct-2018
                                        FrmProductModifier frmProductModifier = new FrmProductModifier(packageIdSelected, packageRow.Cells["ProductType"].Value.ToString(), Utilities, null);
                                        frmProductModifier.ShowDialog();

                                        Transaction trx = new Transaction();
                                        //g frmProductModifier.transactionModifier.ModifierSetDTO = trx.LoadSelectedModifiers(frmProductModifier.transactionModifier.purchasedProducts.PurchasedModifierSetDTOList);
                                        frmProductModifier.transactionModifier.purchasedProducts.PurchasedModifierSetDTOList = trx.LoadSelectedModifiers(frmProductModifier.transactionModifier.ModifierSetDTO);
                                        lstPurchasedModifierProducts.Add(new KeyValuePair<PurchasedProducts, int>(frmProductModifier.transactionModifier.purchasedProducts, packageIdSelected));

                                        ProductQuantity--;
                                    }
                                }
                            }
                            //End: Modified for adding the Product Modifier part on 04-Oct-2016
                        }

                        if (packageRow.Cells["packageName"].Value != null && packageRow.Cells["SelectProduct"].Value != null && packageRow.Cells["SelectProduct"].Value.ToString() == "Y")
                        {
                            selected = true;
                        }
                    }
                }
                //if (quantity > Convert.ToInt32(nudGuestCount.Value))
                //{
                //    POSUtils.ParafaitMessageBox(MessageUtils.getMessage(945));
                //    return;
                //}
                if (!selected)
                {
                    txtMessage.Text = MessageUtils.getMessage(304);
                    this.ActiveControl = dgvPackageList;
                    tcBooking.SelectedTab = tpPackage;
                    lstPacakgeIdList = null;
                    log.LogMethodExit("Package was not selected");
                    return;
                }
            }
            //end//
            tcBooking.SelectedTab = tpAdditional;

            log.LogMethodExit();
        }

        private void btnPrevPackage_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            txtMessage.Clear();
            tcBooking.SelectedTab = tpCustomer;
            log.LogMethodExit();
        }

        private void btnNextAdditionalProducts_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            txtMessage.Clear();
            try
            {
                MoreThanOneServiceChargeEntry();
                tcBooking.SelectedTab = tpConfirm;
            }
            catch (ValidationException ex)
            {
                txtMessage.Text = ex.GetAllValidationErrorMessages();
            }
            log.LogMethodExit();
        }

        private void btnPrevAdditionalProducts_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            txtMessage.Clear();
            tcBooking.SelectedTab = tpPackage;
            log.Debug("Ends-btnPrevAdditionalProducts_Click()");
        }

        private void btnPrevConfirm_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            txtMessage.Clear();
            tcBooking.SelectedTab = tpAdditional;
            log.LogMethodExit();
        }

        private void cmbPackage_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            //if (cmbPackage.SelectedIndex < 0)
            //{
            //    if (userAction)
            //    {
            //        attractionScheduleId = -1;
            //        LoadSchedules();
            //    }
            //    log.LogMethodExit("CmbPackage.SelectedIndex < 0");
            //    return;
            //}

            //if (Reservation.Status != ReservationDTO.ReservationStatus.BOOKED.ToString())
            //{
            //    LoadSchedules();
            //    log.LogMethodExit("Reservation Status is not BOOKED");
            //    return;
            //}

            //DataRowView drv = cmbPackage.SelectedItem as DataRowView;
            //if (drv != null)
            //{
            //    int duration = Convert.ToInt32(drv.Row["Duration"]);
            //    if (duration <= 0)
            //    {
            //        duration = 0;
            //    }
            //    int quantity = Convert.ToInt32(drv.Row["Quantity"]);
            //    int minQuantity = Convert.ToInt32(drv.Row["MinQty"]);
            //    if (minQuantity > 0)
            //    {
            //        nudQuantity.Minimum = minQuantity;
            //    }
            //    if (quantity > 0)
            //    {
            //        nudQuantity.Value = Math.Max(minQuantity, quantity);
            //    }

            //    if (Reservation.BookingId <= 0)
            //    {
            //        GetToTime(duration);
            //    }
            //    try
            //    {
            //        GetPackageContents();
            //    }
            //    catch (Exception ex)
            //    {
            //        log.Error(ex);
            //    }
            //}
            log.LogMethodExit();
        }

        private void lnkUpdate_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            //if (ValidateCustomer() == false)
            //{
            //    log.LogMethodExit("ValidateCustomer() == false");
            //    return;
            //}
            //try
            //{
            //    Reservation.UpdateReservation(txtBookingName.Text,
            //                                    txtRemarks.Text,
            //                                    ParafaitEnv.LoginID,
            //                                    cmbChannel.SelectedItem.ToString(),
            //                                    customerDetailUI.CustomerDTO);

            //    txtMessage.Text = "Customer details updated";
            //    log.Info("Customer details updated");
            //}
            //catch (Exception ex)
            //{
            //    POSUtils.ParafaitMessageBox(ex.Message);
            //    log.Error(ex);
            //}
            log.LogMethodExit();
        }

        private void PopulateCustomerDetails(CustomerDTO customerDTO)
        {
            log.LogMethodEntry(customerDTO);
            customerDetailUI.CustomerDTO = customerDTO;
            log.LogMethodExit();
        }

        private void btnCustomerLookup_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            txtMessage.Clear();
            try
            {
                CustomerLookupUI customerLookupUI = new CustomerLookupUI(Utilities,
                                                                         customerDetailUI.FirstName,
                                                                         customerDetailUI.MiddleName,
                                                                         customerDetailUI.LastName,
                                                                         customerDetailUI.PhoneNumber,
                                                                         customerDetailUI.Email,
                                                                         customerDetailUI.UniqueIdentifier);
                if (customerLookupUI.ShowDialog() == DialogResult.OK)
                {
                    customerDTO = customerLookupUI.SelectedCustomerDTO;
                    customerDetailUI.CustomerDTO = customerLookupUI.SelectedCustomerDTO;
                }
            }
            catch (Exception ex)
            {
                POSUtils.ParafaitMessageBox(ex.Message);
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void txtCustomerName_Leave(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (string.IsNullOrWhiteSpace((sender as TextBox).Text) == false)
            {
                CustomerListBL customerListBL = new CustomerListBL(Utilities.ExecutionContext);
                CustomerSearchCriteria customerSearchCriteria = new CustomerSearchCriteria(CustomerSearchByParameters.PROFILE_FIRST_NAME, Operator.EQUAL_TO, (sender as TextBox).Text);
                customerSearchCriteria.OrderBy(CustomerSearchByParameters.CUSTOMER_ID);
                customerSearchCriteria.Paginate(0, 20);
                List<CustomerDTO> customerDTOList = customerListBL.GetCustomerDTOList(customerSearchCriteria, true, true);
                if (customerDTOList != null && customerDTOList.Count > 0)
                {
                    if (customerDTOList.Count > 1)
                    {
                        CustomerLookupUI customerLookupUI = new CustomerLookupUI(Utilities, (sender as TextBox).Text);
                        if (customerLookupUI.ShowDialog() == DialogResult.OK)
                        {
                            customerDetailUI.CustomerDTO = customerLookupUI.SelectedCustomerDTO;
                        }
                        else
                        {
                            customerDetailUI.CustomerDTO = customerDTOList[0];
                        }
                    }
                    else
                    {
                        if (customerDetailUI.CustomerDTO.Id != customerDTOList[0].Id)
                        {
                            customerDetailUI.CustomerDTO = customerDTOList[0];
                        }
                    }
                }
            }
            log.LogMethodExit();
        }

        private void customerDetailUI_CustomerContactInfoEntered(object sender, CustomerContactInfoEnteredEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            CustomerListBL customerListBL = new CustomerListBL(Utilities.ExecutionContext);
            CustomerSearchCriteria customerSearchCriteria = new CustomerSearchCriteria(CustomerSearchByParameters.CONTACT_CONTACT_TYPE, Operator.EQUAL_TO, e.ContactType.ToString());
            customerSearchCriteria.And(CustomerSearchByParameters.CONTACT_ATTRIBUTE1, Operator.EQUAL_TO, e.ContactValue)
                                  .OrderBy(CustomerSearchByParameters.CUSTOMER_ID)
                                  .Paginate(0, 20);
            List<CustomerDTO> customerDTOList = customerListBL.GetCustomerDTOList(customerSearchCriteria, true, true);
            if (customerDTOList != null && customerDTOList.Count > 0)
            {
                if (customerDTOList.Count > 1)
                {
                    CustomerLookupUI customerLookupUI = new CustomerLookupUI(Utilities, "", "", "",
                                                                             e.ContactType == ContactType.EMAIL ? e.ContactValue : "",
                                                                             e.ContactType == ContactType.PHONE ? e.ContactValue : "",
                                                                             "");
                    if (customerLookupUI.ShowDialog() == DialogResult.OK)
                    {
                        customerDetailUI.CustomerDTO = customerLookupUI.SelectedCustomerDTO;
                    }
                    else
                    {
                        customerDetailUI.CustomerDTO = customerDTOList[0];
                    }
                }
                else
                {
                    if (customerDetailUI.CustomerDTO.Id != customerDTOList[0].Id)
                    {
                        customerDetailUI.CustomerDTO = customerDTOList[0];
                    }
                }
            }
            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            txtMessage.Clear();
            this.Close();
            log.LogMethodExit();
        }


        //Begin Modification - Jan-19-2016- Added to create Attraction Product Object//
        public bool CreateAttractionProduct(int comboChildRecordId, int product_id, double price, int quantity, int parentProductId, List<KeyValuePair<AttractionBooking, int>> lstAtsProduct, bool excludeSchedule = false)
        {
            log.LogMethodEntry(product_id, price, quantity, parentProductId);
            //Get the Reservation date and time. Use this for Attraction Schedule booking product 
            double lclTime = Convert.ToDouble(cmbFromTime.SelectedValue);
            DateTime fromDateTime = dtpforDate.Value.Date.AddMinutes(Math.Floor(lclTime) * 60).AddMinutes((lclTime - Math.Floor(lclTime)) * 100.0);
            bool retVal = false;
            if (excludeSchedule)
                excludeAttractionSchedule += ", " + exattractionScheduleId.ToString();
            AttractionSchedules ats = new AttractionSchedules(null, product_id, quantity, fromDateTime, excludeAttractionSchedule);
            List<AttractionBookingDTO> scheduleList = new List<AttractionBookingDTO>();

            if (ats.ScheduleExist == false) // schedules not specified.
            {
            }
            else if (ats.ShowSchedules(scheduleList) == true)
            {

                int prodIdToAdd;
                if (parentProductId == -1)
                {
                    //When Attraction product is a Individual Product/Additional Product within Booking Product
                    prodIdToAdd = product_id;
                }
                else
                {    //When Attraction product is part of combo add the combo product id
                    prodIdToAdd = parentProductId;
                }

                // remove existing schedules, if any
                List<KeyValuePair<AttractionBooking, int>> lstCopy = new List<KeyValuePair<AttractionBooking, int>>(lstAtsProduct);
                foreach (KeyValuePair<AttractionBooking, int> keyval in lstCopy)
                {
                    KeyValuePair<AttractionBooking, int> keyvalOrig = lstAtsProduct.Find(x => x.Key == keyval.Key);
                    if (keyval.Value == prodIdToAdd && comboChildRecordId == keyval.Key.AttractionBookingDTO.Identifier)
                    {
                        lstAtsProduct.Remove(keyvalOrig);
                        //  keyvalOrig.Key.Expire();
                    }
                }

                List<AttractionBooking> atbList = new List<AttractionBooking>();
                foreach (AttractionBookingDTO attractionBookingDTO in scheduleList)
                {
                    //if (dgv["Desired Units", i].Value != null && dgv["Desired Units", i].Value != DBNull.Value)
                    //{
                    //    int qty = Convert.ToInt32(dgv["Desired Units", i].Value);
                    //    if (qty <= 0)
                    //        continue;
                    //    if (qty > 999)
                    //        qty = 999;
                    //    atb = new AttractionBooking(Utilities);
                    //    atb.AttractionPlayId = Convert.ToInt32(dgv["AttractionPlayId", i].Value);
                    //    atb.AttractionPlayName = dgv["Play Name", i].Value.ToString();
                    //    atb.AttractionScheduleId = exattractionScheduleId = Convert.ToInt32(dgv["AttractionScheduleId", i].Value);
                    //    atb.ScheduleTime = Convert.ToDateTime(dgv["Schedule Time", i].Value);
                    //    atb.ScheduleToTime = Decimal.Round(Convert.ToDecimal(dgv["ScheduleToTime", i].Value), 2, MidpointRounding.AwayFromZero);
                    //    atb.BookedUnits = qty;
                    //    if (dgv["Total Units", i].Value != DBNull.Value)
                    //        atb.AvailableUnits = Convert.ToInt32(dgv["Total Units", i].Value);
                    //    atb.Price = Convert.ToDouble(dgv["Price", i].Value == DBNull.Value ? 0 : dgv["Price", i].Value);
                    //    if (dgv["Expiry Date", i].Value != DBNull.Value)
                    //        atb.ExpiryDate = Convert.ToDateTime(dgv["Expiry Date", i].Value);
                    //    atb.PromotionId = Convert.ToInt32(dgv["PromotionId", i].Value);
                    //    atb.SelectedSeats = (dgv["Seats", i].Tag == null ? null : dgv["Seats", i].Tag as List<int>);
                    //    atb.SelectedSeatNames = (dgv["PickSeats", i].Tag == null ? null : dgv["PickSeats", i].Tag as List<string>);

                    //AttractionBooking atb = new AttractionBooking(Utilities);
                    //atb.AttractionPlayId = schedule.AttractionPlayId;
                    //atb.AttractionPlayName = schedule.AttractionPlayName;
                    //atb.AttractionScheduleId = exattractionScheduleId = schedule.AttractionScheduleId;
                    //atb.ScheduleTime = schedule.ScheduleTime;
                    //atb.BookedUnits = schedule.BookedUnits;
                    //atb.AvailableUnits = schedule.AvailableUnits;
                    //atb.Price = schedule.Price;
                    //atb.ExpiryDate = schedule.ExpiryDate;
                    //atb.PromotionId = schedule.PromotionId;
                    //atb.SelectedSeats = schedule.SelectedSeats;
                    //atb.SelectedSeatNames = schedule.SelectedSeatNames;
                    //atb.ScheduleFromTime = schedule.ScheduleFromTime;
                    //atb.ScheduleToTime = schedule.ScheduleToTime;

                    attractionBookingDTO.Identifier = comboChildRecordId;

                    AttractionBooking atb = new AttractionBooking(Utilities.ExecutionContext, attractionBookingDTO);
                    //string message = "";
                    //if (!atb.Save(-1, null, ref message))
                    //{
                    //    throw new ApplicationException(message);
                    //}

                    lstAtsProduct.Add(new KeyValuePair<AttractionBooking, int>(atb, prodIdToAdd));
                    atbList.Add(atb);
                }

                retVal = true;

                if (parentProductId != -1) // combo
                {
                    List<frmInputCards.clsCard> lstAtbCards = new List<frmInputCards.clsCard>();

                    // get a list of all cards already used
                    foreach (KeyValuePair<AttractionBooking, int> keyval in lstAtsProduct)
                    {
                        foreach (string cardNumber in keyval.Key.cardNumberList)
                        {
                            frmInputCards.clsCard card = new frmInputCards.clsCard(cardNumber);
                            card.products.Add(keyval.Key.AttractionBookingDTO.AttractionScheduleName + " " + keyval.Key.AttractionBookingDTO.ScheduleTime.ToString(POSStatic.ParafaitEnv.DATETIME_FORMAT));
                            lstAtbCards.Add(card);
                        }
                    }

                    DataRow dr = trxUtils.getProductDetails(product_id, null).Rows[0];
                    if (lstAtbCards.Count == 0) // no cards allocated which means this is the first set of schedules
                    {
                        foreach (KeyValuePair<AttractionBooking, int> keyval in lstAtsProduct)
                        {
                            for (int i = 0; i < keyval.Key.AttractionBookingDTO.BookedUnits; i++)
                            {
                                string cardNumber = new RandomTagNumber(Utilities.ExecutionContext).Value;
                                if (dr["AutoGenerateCardNumber"].ToString() != "Y")
                                    cardNumber = "T" + cardNumber.Substring(1);
                                keyval.Key.cardNumberList.Add(cardNumber);
                            }
                        }
                    }
                    else
                    {
                        using (frmInputCards fip = new frmInputCards(dr["Product_Name"].ToString(), lstAtbCards, atbList))
                        {
                            if (fip.ShowDialog() != DialogResult.OK)
                                retVal = false;
                        }
                    }
                }
            }
            else
                retVal = false;

            ats.Dispose();
            log.LogMethodExit(retVal);
            return retVal;
        }
        //End Modification - Jan-19-2016- Added to create Attraction Product Object//


        /* Added to enable edit reservation after confirming booking with payment on May 18, 2015
        Here the a new BookingId is created with old booking Details which will be further updated with edited information */
        private void btnEditBooking_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            txtMessage.Clear();
            frmMakeReservation f;
            if (POSUtils.ParafaitMessageBox(MessageUtils.getMessage(925), "Edit Booking", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                //log.Info("edit booking enabled");
                //string message = "";
                //// lblStatus.Text = "WIP";
                //isEditedBooking = true;// Here isEditedBooking is set to true , if booking is edited 
                //btnEditBooking.Enabled = false;
                //btnConfirm.Enabled = false;
                //editedBookingId = Reservation.BookingId;//editedBookingId is the Id of the Booking that was selected to edit.
                //bool status = Reservation.EditReservation(ref message);//Edit Reservation is a method written in ParafaitUtils, to Insert a new record with new bookingId and old booking data
                //if (isEditedBooking)
                //{
                //    Semnox.Core.Utilities.EventLog audit = new Semnox.Core.Utilities.EventLog(Utilities);
                //    loginId = POSStatic.ParafaitEnv.LoginID;
                //    Reservation.UpdateBookingId(editedBookingId, Reservation.BookingId);
                //    audit.logEvent("Reservation", 'D', loginId, "Booking is Edited with Id:" + editedBookingId.ToString() + " Reservation Code is " + Reservation.ReservationCode, "ConfirmationScreen", 0, "", Reservation.BookingId.ToString(), null);
                //}
                //this.Close();
                //using (f = new frmMakeReservation(Convert.ToInt32(Reservation.BookingId), isEditedBooking, editedBookingId))
                //{
                //    f.ShowDialog();
                //}
            }
            log.LogMethodExit();
        }

        private void dgvPackageList_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if ((this.dgvPackageList.IsCurrentCellDirty))//&& (dgvPackageList.Columns[dgvPackageList.CurrentCell.ColumnIndex].Name != "discounts")
            {
                // This fires the cell value changed handler , above event
                dgvPackageList.CommitEdit(DataGridViewDataErrorContexts.Commit);
                dgvPackageList.EndEdit();
            }
            log.LogMethodExit();
        }

        private void dgvPackageList_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            e.Control.KeyPress -= new KeyPressEventHandler(dgvPackageList_KeyPress);
            //To check if Non Numeric characters are entered for guest count
            if (dgvPackageList.Columns[dgvPackageList.CurrentCell.ColumnIndex].Name == "Quantity")
            {
                TextBox tb = e.Control as TextBox;
                if (tb != null)
                {
                    tb.MaxLength = 3;
                    tb.KeyPress += new KeyPressEventHandler(dgvPackageList_KeyPress);
                }
            }

            log.LogMethodExit();
        }

        //Added  to delete the selected row and change the package contents based on package selction on June 6, 2015//
        private void dgvPackageList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);

            //if (dgvPackageList.Columns[e.ColumnIndex].Name == "Quantity")
            //    dgvPackageList.KeyPress += new KeyPressEventHandler(dgvPackageList_KeyPress);
            try
            {
                if ((e.RowIndex < 0) || (e.ColumnIndex == -1))
                {
                    log.LogMethodExit("e.RowIndex < 0) || (e.ColumnIndex == -1)");
                    return;
                }
                if (dgvPackageList.Rows[e.RowIndex].Cells["ChildId"].Value == DBNull.Value)
                {
                    log.LogMethodExit("dgvPackageList.Rows[e.RowIndex].Cells[ChildId].Value == DBNull.Value");
                    return;
                }

                if (dgvPackageList.Rows[e.RowIndex].Cells["ChildId"].Value != DBNull.Value)
                {
                    if (dgvPackageList.Columns[e.ColumnIndex].Name == "SelectProduct")
                    {
                        if (dgvPackageList.Rows[e.RowIndex].Cells["ChildId"].Value != null)
                        {
                            packageIdSelected = Convert.ToInt32(dgvPackageList.Rows[e.RowIndex].Cells["ChildId"].Value);
                            productQuantity = Convert.ToInt32(dgvPackageList.Rows[e.RowIndex].Cells["Quantity"].Value);
                            productType = Convert.ToString(dgvPackageList.Rows[e.RowIndex].Cells["ProductType"].Value);

                            DataGridViewCheckBoxCell chkPackageproductId = (DataGridViewCheckBoxCell)dgvPackageList.Rows[e.RowIndex].Cells["SelectProduct"];
                            if (dgvPackageList.Rows[e.RowIndex].Cells["SelectedStatus"].Value.ToString() == "false")
                            {
                                dgvPackageList.Rows[e.RowIndex].Cells["SelectedStatus"].Value = "true";
                                chkPackageproductId.Value = "Y";
                                try
                                {
                                    GetAttractionNManualProdsForSelectedPackage(e.RowIndex, productType, packageIdSelected, productQuantity, true);
                                }
                                catch (Exception ex)
                                {
                                    log.Error(ex);
                                    dgvPackageList.Rows[e.RowIndex].Cells["SelectedStatus"].Value = "false";
                                    chkPackageproductId.Value = "N";
                                    RemovePackageFromList(packageIdSelected);
                                }
                            }
                            else
                            {
                                dgvPackageList.Rows[e.RowIndex].Cells["SelectedStatus"].Value = "false";
                                chkPackageproductId.Value = "N";
                                if (productType == "COMBO")
                                {
                                    log.Info("Selected productType COMBO ");
                                    GetComboProductCategoryProducts();//If product is unselected then if category products where seleted then list will be cleared here
                                }
                                RemovePackageFromList(packageIdSelected);
                            }

                            if (dgvPackageList.Rows[e.RowIndex].Cells["SelectedStatus"].Value.ToString() == "true")
                            {
                                try
                                {
                                    GetPackageContents(true);//If product is selectd then displays the product contents
                                }
                                catch (Exception ex)
                                {
                                    log.Error(ex);
                                    dgvPackageList.Rows[e.RowIndex].Cells["SelectedStatus"].Value = "false";
                                    chkPackageproductId.Value = "N";
                                    RemovePackageFromList(packageIdSelected);
                                }

                            }

                            GetDiscounts(packageIdSelected, e.RowIndex);//Loads the discounts corresponding to the product selected
                            dgvPackageList.CurrentCellDirtyStateChanged += new EventHandler(dgvPackageList_CurrentCellDirtyStateChanged);
                        }
                    }
                }
                else
                {
                    log.LogMethodExit("dgvPackageList.Rows[e.RowIndex].Cells[ChildId].Value is null");
                    return;
                }

                //If discounts are seleted before selcting the product
                if (dgvPackageList.Columns[e.ColumnIndex].Name == "discountName" && dgvPackageList.Columns[e.ColumnIndex].ReadOnly == false && dgvPackageList.Rows[e.RowIndex].Cells["SelectedStatus"].Value.ToString() == "false")
                {
                    POSUtils.ParafaitMessageBox(MessageUtils.getMessage(940));
                    log.LogMethodExit("Products are not selected or discountName ");
                    return;
                }
                if (dgvPackageList.Columns[e.ColumnIndex].Name == "transactionProfileId" && dgvPackageList.Columns[e.ColumnIndex].ReadOnly == false && dgvPackageList.Rows[e.RowIndex].Cells["SelectedStatus"].Value.ToString() == "false")
                {
                    POSUtils.ParafaitMessageBox(MessageUtils.getMessage(940));
                    log.LogMethodExit("Products are not selected");
                    return;
                }
                //Added - jan-27-2016-After selecting the product if user wants to Change the quantity of the product. This done to ensure proper combo quantity is sent to the form that displays category products
                if (dgvPackageList.Columns[e.ColumnIndex].Name == "Quantity" && dgvPackageList.Rows[e.RowIndex].Cells["SelectedStatus"].Value.ToString() == "true" && Convert.ToString(dgvPackageList.Rows[e.RowIndex].Cells["ProductType"].Value) == "COMBO" && CategoryProuctFinalSelectedList != null && CategoryProuctFinalSelectedList.Count > 0 || Convert.ToString(dgvPackageList.Rows[e.RowIndex].Cells["ProductType"].Value) == "ATTRACTION")
                {
                    packageIdSelected = Convert.ToInt32(dgvPackageList.Rows[e.RowIndex].Cells["ChildId"].Value);
                    foreach (Semnox.Parafait.Transaction.Reservation.clsSelectedCategoryProducts catList in CategoryProuctFinalSelectedList)
                    {
                        if (catList.parentComboProductId == packageIdSelected)
                        {
                            POSUtils.ParafaitMessageBox(MessageUtils.getMessage(946));
                            log.LogMethodExit("The booking package has category products. Please uncheck the package before changing quantity.");
                            return;
                        }
                    }

                }
                //End - jan-27-2016-
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void RemovePackageFromList(int selectedPackageId)
        {
            log.LogMethodEntry(selectedPackageId);
            if (lstAttractionProductslist != null && lstAttractionProductslist.Count > 0)
            {
                for (int j = lstAttractionProductslist.Count - 1; j >= 0; j--)
                {
                    if (selectedPackageId == lstAttractionProductslist[j].Value)
                    {
                        lstAttractionProductslist.RemoveAt(j);
                    }
                }
            }

            if (lstPurchasedModifierProducts != null && lstPurchasedModifierProducts.Count > 0)
            {
                for (int j = lstPurchasedModifierProducts.Count - 1; j >= 0; j--)
                {
                    if (selectedPackageId == lstPurchasedModifierProducts[j].Value)
                    {
                        lstPurchasedModifierProducts.RemoveAt(j);
                    }
                }
            }
            log.LogMethodExit();
        }

        private bool PresentInAttractionProductList(int selectedPackageId)
        {
            log.LogMethodEntry(selectedPackageId);
            bool foundInList = false;
            if (lstAttractionProductslist != null && lstAttractionProductslist.Count > 0)
            {
                for (int j = lstAttractionProductslist.Count - 1; j >= 0; j--)
                {
                    if (selectedPackageId == lstAttractionProductslist[j].Value)
                    {
                        foundInList = true;
                        break;
                    }
                }
            }
            log.LogMethodExit(foundInList);
            return foundInList;
        }

        private bool PresentInAdditionalProductATSList(int selectedPackageId, int quantity, int lineId)
        {
            log.LogMethodEntry(selectedPackageId, quantity, lineId);

            bool foundInList = false;
            foundInList = additionalProductInformationLst.Exists(adInfo => adInfo.ProductId == selectedPackageId && Convert.ToInt32(adInfo.Quantity) == quantity && adInfo.lineId == lineId);
            log.LogMethodExit(foundInList);
            return foundInList;
        }

        private bool PresentInModifierProductList(List<KeyValuePair<PurchasedProducts, int>> purchasedModifierProductList, int selectedPackageId)
        {
            log.LogMethodEntry();
            bool foundInList = false;
            if (purchasedModifierProductList != null && purchasedModifierProductList.Count > 0)
            {
                for (int j = purchasedModifierProductList.Count - 1; j >= 0; j--)
                {
                    if (selectedPackageId == purchasedModifierProductList[j].Value)
                    {
                        foundInList = true;
                        break;
                    }
                }
            }
            log.LogMethodExit(foundInList);
            return foundInList;
        }

        private bool PresentInAdditionalProductModifierList(int selectedPackageId, int quantity, int lineId)
        {
            log.LogMethodEntry(selectedPackageId, quantity, lineId);
            bool foundInList = false;
            foundInList = additionalProductInformationLst.Exists(adInfo => adInfo.ProductId == selectedPackageId && Convert.ToInt32(adInfo.Quantity) == quantity && adInfo.lineId == lineId);
            log.LogMethodExit(foundInList);
            return foundInList;
        }

        private int GetLoadedQuantityForAdditionProductModifier(int selectedProductId)
        {
            log.LogMethodEntry(selectedProductId);
            int loadedQty = 0;
            if (lstPurchasedAdditionalModifierProducts != null && lstPurchasedAdditionalModifierProducts.Count > 0)
            {
                for (int j = lstPurchasedAdditionalModifierProducts.Count - 1; j >= 0; j--)
                {
                    if (selectedProductId == lstPurchasedAdditionalModifierProducts[j].Value)
                    {
                        loadedQty = lstPurchasedAdditionalModifierProducts.FindAll(lst => lst.Value == selectedProductId).Count;
                        break;
                    }
                }
            }
            log.LogMethodExit(loadedQty);
            return loadedQty;
        }

        private int GetLoadedQuantityForAdditionProductATS(int selectedProductId)
        {
            log.LogMethodEntry(selectedProductId);
            int loadedQty = 0;
            if (lstAdditionalAttractionProductsList != null && lstAdditionalAttractionProductsList.Count > 0)
            {
                for (int j = lstAdditionalAttractionProductsList.Count - 1; j >= 0; j--)
                {
                    if (selectedProductId == lstPurchasedAdditionalModifierProducts[j].Value)
                    {
                        loadedQty = lstAdditionalAttractionProductsList.FindAll(lst => lst.Value == selectedProductId).Count;
                        break;
                    }
                }
            }
            log.LogMethodExit(loadedQty);
            return loadedQty;
        }
        private void RemoveFromAdditionalProductInformationLst()
        {
            log.LogMethodEntry();
            if (additionalProductInformationLst != null && additionalProductInformationLst.Count > 0)
            {
                for (int j = additionalProductInformationLst.Count - 1; j >= 0; j--)
                {
                    bool presentInGrid = false;
                    for (int i = 0; i < dgvBookingProducts.Rows.Count; i++)
                    {
                        if (dgvBookingProducts.Rows[i].Cells["dcProduct"].Value != null
                            && additionalProductInformationLst[j].ProductId == Convert.ToInt32(dgvBookingProducts.Rows[i].Cells["dcProduct"].Value)
                            && Convert.ToInt32(additionalProductInformationLst[j].Quantity) == Convert.ToInt32(dgvBookingProducts.Rows[i].Cells["dcQuantity"].Value)
                            && additionalProductInformationLst[j].lineId == Convert.ToInt32(dgvBookingProducts.Rows[i].Cells["LineId"].Value)
                            )
                        {
                            presentInGrid = true;
                            break;
                        }
                    }
                    if (presentInGrid == false)
                    {
                        additionalProductInformationLst.RemoveAt(j);
                    }
                }
            }
            log.LogMethodExit();
        }


        private void GetAttractionNManualProdsForSelectedPackage(int dgvPackageListRowIndex, string productTypeName, int selectedPackageId, int prodQty, bool allowGet = false)
        {
            log.LogMethodEntry(dgvPackageListRowIndex, productTypeName, selectedPackageId, prodQty, allowGet);
            //if (BookingId == -1 || isEditedBooking || lblStatus.Text == "BLOCKED")
            if (BookingId == -1 || lblStatus.Text.ToUpper() == ReservationDTO.ReservationStatus.BLOCKED.ToString() || allowGet == true)
            {
                if (productTypeName == "ATTRACTION")
                {
                    log.Info("Selected productType ATTRACTION ");
                    bool alreadyLoaded = PresentInAttractionProductList(selectedPackageId);
                    if (allowGet || alreadyLoaded == false)
                    {
                        Card card = null;
                        DataTable attractionProductDet = trxUtils.getProductDetails(selectedPackageId, card);
                        if (attractionProductDet.Rows.Count > 0)
                        {
                            //if (attractionProductDet.Rows[0]["QuantityPrompt"].ToString() == "Y")
                            //{
                            //    int attractionQuantity = productQuantity;
                            //    while (attractionQuantity > 0)
                            //    {
                            //        createAttractionProduct(packageIdSelected, 0, 1, -1, lstAttractionProductslist);//Combo product Id is sent -1, since its not part of Combo
                            //        attractionQuantity--;

                            //    }
                            //}
                            //else
                            if (CreateAttractionProduct(-1, selectedPackageId, 0, prodQty, -1, lstAttractionProductslist) == false)
                            {
                                throw new Exception(Utilities.MessageUtils.getMessage("Attraction schedule product not created"));
                            }
                        }
                    }
                }
                else if (productTypeName == "MANUAL")
                {
                    //if (purchasedProductSelected != null)
                    //{
                    //    purchasedProductSelected.Clear();
                    //}

                    DataTable dt = Utilities.executeDataTable(@"SELECT pm.ModifierSetId, SetName, pm.AutoShowInPos 
                                                                                from ProductModifiers pm, ModifierSet ms 
                                                                                where ms.ModifierSetId = pm.ModifierSetId 
                                                                                and pm.ProductId = @ProductId", new SqlParameter("@ProductId", selectedPackageId));
                    if (dt.Rows.Count != 0)//&& purchasedProductSelected.Count == 0)
                    {

                        bool modifiersSelected = PresentInModifierProductList(lstPurchasedModifierProducts, selectedPackageId);
                        if (allowGet || modifiersSelected == false)
                        {
                            int ProductQuantity = Convert.ToInt32(dgvPackageList.Rows[dgvPackageListRowIndex].Cells["Quantity"].Value);
                            while (ProductQuantity > 0)
                            {
                                //Modification for F&B Restructuring of modifiers on 17-Oct-2018
                                using (FrmProductModifier frmProductModifier = new FrmProductModifier(selectedPackageId, dgvPackageList.Rows[dgvPackageListRowIndex].Cells["ProductType"].Value.ToString(), Utilities, null))
                                {
                                    if (frmProductModifier.ShowDialog() == DialogResult.OK)
                                    {
                                        Transaction trx = new Transaction();
                                        frmProductModifier.transactionModifier.purchasedProducts.PurchasedModifierSetDTOList = trx.LoadSelectedModifiers(frmProductModifier.transactionModifier.ModifierSetDTO);
                                        //purchasedProductSelected.Add(frmProductModifier.transactionModifier.purchasedProducts);
                                        lstPurchasedModifierProducts.Add(new KeyValuePair<PurchasedProducts, int>(frmProductModifier.transactionModifier.purchasedProducts, selectedPackageId));
                                    }
                                    else
                                    {
                                        throw new Exception(Utilities.MessageUtils.getMessage("Modifier is not selected"));
                                    }

                                }

                                ProductQuantity--;
                            }
                        }
                    }
                }
            }
            log.LogMethodExit();
        }

        //Begin :Loads the discounts based on the product selected
        private void GetDiscounts(int selectedProductId, int rowIndex)
        {
            log.LogMethodEntry(selectedProductId, rowIndex);
            DataGridViewComboBoxCell cmbPackageProductDiscount = null;
            DataGridViewComboBoxCell cmbAdditionalProductDiscount = null;
            packageIdSelected = selectedProductId;
            if (rowIndex <= -1)
            {
                log.LogMethodExit("RowIndex <= -1 ");
                return;
            }

            if (dgvPackageList.Rows.Count > 1 && rowIndex < dgvPackageList.Rows.Count)
                cmbPackageProductDiscount = (DataGridViewComboBoxCell)dgvPackageList.Rows[rowIndex].Cells["discountName"];
            if (dgvBookingProducts.Rows.Count > 1 && rowIndex < dgvBookingProducts.Rows.Count)
                cmbAdditionalProductDiscount = (DataGridViewComboBoxCell)dgvBookingProducts.Rows[rowIndex].Cells["AdditionalDiscountName"];
            try
            {
                List<DiscountsDTO> discountDTOList;
                DiscountsListBL discountsListBL = new DiscountsListBL();
                List<KeyValuePair<DiscountsDTO.SearchByParameters, string>> searchDiscountsParams = new List<KeyValuePair<DiscountsDTO.SearchByParameters, string>>();
                searchDiscountsParams.Add(new KeyValuePair<DiscountsDTO.SearchByParameters, string>(DiscountsDTO.SearchByParameters.DISCOUNT_TYPE, DiscountsBL.DISCOUNT_TYPE_TRANSACTION));
                searchDiscountsParams.Add(new KeyValuePair<DiscountsDTO.SearchByParameters, string>(DiscountsDTO.SearchByParameters.ACTIVE_FLAG, "Y"));
                searchDiscountsParams.Add(new KeyValuePair<DiscountsDTO.SearchByParameters, string>(DiscountsDTO.SearchByParameters.AUTOMATIC_APPLY, "N"));
                searchDiscountsParams.Add(new KeyValuePair<DiscountsDTO.SearchByParameters, string>(DiscountsDTO.SearchByParameters.SITE_ID, (Utilities.ParafaitEnv.IsCorporate ? Utilities.ParafaitEnv.SiteId : -1).ToString()));
                searchDiscountsParams.Add(new KeyValuePair<DiscountsDTO.SearchByParameters, string>(DiscountsDTO.SearchByParameters.DISCOUNTED_PRODUCT_ID, packageIdSelected.ToString()));
                discountDTOList = discountsListBL.GetDiscountsDTOList(searchDiscountsParams, false, true);
                if (discountDTOList == null)
                {
                    discountDTOList = new List<DiscountsDTO>();
                }
                else
                {
                    //discountDTOList.RemoveAll(x => (x.DiscountedProductsDTOList == null || x.DiscountedProductsDTOList.Count == 0));
                }
                discountDTOList.Insert(0, new DiscountsDTO());
                discountDTOList[0].DiscountName = "<SELECT>";

                if (dgvPackageList.Rows.Count > 1 && tcBooking.SelectedTab != tpAdditional && cmbPackageProductDiscount != null)//populate  discount only if grid contains any product selected
                {
                    cmbPackageProductDiscount.DataSource = discountDTOList;
                }
                if (dgvBookingProducts.Rows.Count > 1 && tcBooking.SelectedTab != tpPackage && cmbAdditionalProductDiscount != null)//populate  discount only if grid contains any Addon product selected
                {
                    cmbAdditionalProductDiscount.DataSource = discountDTOList;
                }
                discountName.DisplayMember = AdditionalDiscountName.DisplayMember = "DiscountName";
                discountName.ValueMember = AdditionalDiscountName.ValueMember = "DiscountId";
            }
            catch (Exception ex)
            {
                POSUtils.ParafaitMessageBox(ex.Message);
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        //End//

        //To delete the selcted category products incase the combo product was unselected
        private void GetComboProductCategoryProducts()
        {
            log.LogMethodEntry();
            DataTable dtCategory = Utilities.executeDataTable(@"select p.CategoryId, cp.Quantity, p.Name 
                                                                         from ComboProduct cp, Category p
                                                                         where cp.Product_id = @productId
                                                                         and p.CategoryId = cp.CategoryId
                                                                         and cp.Quantity > 0",
                                                                       new SqlParameter("@productId", packageIdSelected));
            try
            {
                if (dtCategory.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCategory.Rows)
                    {
                        DataTable dt = POSStatic.Utilities.executeDataTable(@"select product_name, product_id 
                                                                        from products 
                                                                        where active_flag = 'Y' 
                                                                        and categoryId in (select CategoryId from getCategoryList(@categoryId))
                                                                        order by sort_order, product_name", new SqlParameter("@categoryId", dr["CategoryId"]));
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {

                            for (int j = CategoryProuctFinalSelectedList.Count - 1; j >= 0; j--)
                            {
                                if (packageIdSelected == CategoryProuctFinalSelectedList[j].parentComboProductId)
                                {
                                    //CategoryProuctFinalList.Remove(new KeyValuePair<int, int>(CategoryProuctFinalList[j].Key, CategoryProuctFinalList[j].Value));
                                    CategoryProuctFinalSelectedList.RemoveAt(j);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        //end//

        private void dgvPackageList_KeyPress(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if ((e.KeyChar != (char)Keys.Back) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
            if (char.IsControl(e.KeyChar) == false)
            {
                if (e.KeyChar < '0' || e.KeyChar > '9')
                {
                    e.Handled = true;
                }
            }
            log.LogMethodExit();
        }

        ////Added the below event to Load the Package Contents based on Package selection when clicked on any row on June 6, 2015//
        //private void dgvPackage_SelectionChanged(object sender, EventArgs e)
        //{
        //    log.LogMethodEntry(sender, e);
        //    GetPackageContents();
        //    log.LogMethodExit();
        //}

        //Added to validate the Reservation details for individual packages on June 6, 2015//
        bool ValidateRequest(int packageId, int guestcount, string packageName)
        {
            log.LogMethodEntry(packageId, guestcount, packageName);
            string message = "";

            try
            {
                double lclTime = Convert.ToDouble(cmbFromTime.SelectedValue);
                DateTime fromDateTime = dtpforDate.Value.Date.AddMinutes(Math.Floor(lclTime) * 60).AddMinutes((lclTime - Math.Floor(lclTime)) * 100.0);

                double tolclTime = Convert.ToDouble(cmbToTime.SelectedValue);

                DateTime toDateTime;
                if (tolclTime > lclTime)
                    toDateTime = dtpforDate.Value.Date.AddMinutes(Math.Floor(tolclTime) * 60).AddMinutes((tolclTime - Math.Floor(tolclTime)) * 100.0);
                else
                    toDateTime = dtpforDate.Value.Date.AddDays(1).AddMinutes(Math.Floor(tolclTime) * 60).AddMinutes((tolclTime - Math.Floor(tolclTime)) * 100.0);

                int Duration = (int)(toDateTime - fromDateTime).TotalMinutes;
                //Added 


                ////Added to send facility Id as parameter to get check availablity on Dec-07-2015//
                //if (!(Reservation.ValidateRequest(packageId, fromDateTime, Duration, guestcount, facilityId, ref message)))
                //{
                //    txtMessage.Text = message;
                //    log.LogMethodExit("Unable to ValidateRequest");
                //    return false;
                //}

                log.LogMethodExit(true);
                return true;
            }
            catch (Exception ex)
            {
                POSUtils.ParafaitMessageBox(ex.Message);
                MessageBox.Show(ex.StackTrace);
                log.Error(ex);
                log.LogMethodExit(false);
                return false;
            }
        }

        //Added to create audit trail for every modification done to a reservation on June24, 2015//
        private void LogEvent(bool editedBooking, int editedBookingId)
        {
            log.LogMethodEntry(editedBooking, editedBookingId);
            try
            {
                //audit = new Semnox.Core.Utilities.EventLog(Utilities);
                //string fromTime = dtpforDate.Value.Date.AddMinutes(Math.Floor(Convert.ToDouble(cmbFromTime.SelectedValue)) * 60).AddMinutes((Convert.ToDouble(cmbFromTime.SelectedValue) - Math.Floor(Convert.ToDouble(cmbFromTime.SelectedValue))) * 100.0).ToString("hh:mm:ss tt");
                //string toTime = dtpforDate.Value.Date.AddMinutes(Math.Floor(Convert.ToDouble(cmbToTime.SelectedValue)) * 60).AddMinutes((Convert.ToDouble(cmbToTime.SelectedValue) - Math.Floor(Convert.ToDouble(cmbToTime.SelectedValue))) * 100.0).ToString("hh:mm:ss tt");
                //string gender = customerDetailUI.CustomerDTO.Gender;

                //if (editedBooking)
                //{
                //    log.Info("LogEvent(" + editedBooking + "," + editedBookingId + ") - Edited Booking is saved successfully, BookingId is " + BookingId + "");
                //    audit.logEvent("Reservation", 'D', loginId, "Edited Booking is saved successfully, BookingId is " + BookingId, "ConfirmationScreen", 0, "", BookingId.ToString(), null);
                //    DataTable dtBookingDetails = Utilities.executeDataTable(@"select b.BookingId,b.BookingProductId,b.Quantity,
                //                                                                        b.BookingName,b.CardNumber,b.CardId,b.Age,
                //                                                                        b.FromDate,b.ToDate,
                //                                                                        b.Remarks,b.CustomerId 
                //                                                                        from  Bookings b
                //                                                                        where 
                //                                                                        b.BookingId = @editedBookingId",
                //                                                            new SqlParameter("@editedBookingId", editedBookingId), new SqlParameter("@PassPhrase", ParafaitDefaultContainer.GetDecryptedParafaitDefault(Utilities.ExecutionContext, "CUSTOMER_ENCRYPTION_PASS_PHRASE")));
                //    foreach (DataRow dr in dtBookingDetails.Rows)
                //    {
                //        if (!(dr["BookingProductId"].ToString().CompareTo(cmbBookingClass.SelectedValue.ToString()) == 0))
                //        {
                //            audit.logEvent("Reservation", 'D', ParafaitEnv.LoginID, "Booking Product is changed to " + cmbBookingClass.Text, "DateTime & BookingProduct Screen", 0, "", BookingId.ToString(), null);
                //        }
                //        if (!(dr["Quantity"].ToString().CompareTo(txtGuests.Text) == 0))
                //        {
                //            audit.logEvent("Reservation", 'D', ParafaitEnv.LoginID, "Guest Count is changed", "DateTime & BookingProduct Screen", 0, "", BookingId.ToString(), null);
                //        }
                //        if (!(Convert.ToDateTime(dr["FromDate"]).ToShortDateString().CompareTo(dtpforDate.Value.ToShortDateString()) == 0))
                //            audit.logEvent("Reservation", 'D', ParafaitEnv.LoginID, "Reservation Date is changed", "DateTime&BookingProduct Screen", 0, "", BookingId.ToString(), null);
                //        if (!(Convert.ToDateTime(dr["FromDate"]).ToString("hh:mm:ss tt").CompareTo(fromTime) == 0))
                //            audit.logEvent("Reservation", 'D', ParafaitEnv.LoginID, "Reservation FromTime is changed", "DateTime&BookingProduct Screen", 0, "", BookingId.ToString(), null);
                //        if (!(Convert.ToDateTime(dr["ToDate"]).ToString("hh:mm:ss tt").CompareTo(toTime) == 0))
                //            audit.logEvent("Reservation", 'D', ParafaitEnv.LoginID, "Reservation ToTime is changed", "DateTime&BookingProduct Screen", 0, "", BookingId.ToString(), null);

                //    }
                //    //To check if package and additional products have changed//
                //    DataTable dtBookedBookingProductDetails = Utilities.executeDataTable(@"select b.BookingProductId, tl.product_id,p.product_name, LineId, ParentLineId,tl.quantity,
                //                                                    tl.price, tl.Remarks
                //                                                    ,rd.ReservationDiscountId discountId ,cb.AdditionalProduct
                //                                                    from 
                //                                                    Bookings b 
                //                                                    inner join trx_lines tl
                //                                                    on b.TrxId = tl.TrxId
                //                                                    inner join products p
                //                                                    on p.product_id = tl.product_id
                //                                                    inner join ComboProduct cb
                //                                                    on cb.ChildProductId = tl.product_id
                //                                                    left outer join ReservationDiscounts rd 
                //                                                    on rd.productId = tl.product_id and rd.BookingId = @BookingId
                //                                                    where
                //                                                    cb.AdditionalProduct ='N' and ParentLineId is null
                //                                                    and 
                //                                                    b.BookingId =  @BookingId and cb.Product_Id = b.BookingProductId",
                //                                                            new SqlParameter("@BookingId", editedBookingId));

                //    DataTable dtAdditionalProductsDetails = Utilities.executeDataTable(@"select distinct b.BookingProductId, tl.product_id,p.product_name, LineId, ParentLineId,tl.quantity,
                //                                                    tl.amount, tl.Remarks
                //                                                    ,rd.ReservationDiscountId discountId ,cb.AdditionalProduct
                //                                                    from 
                //                                                    Bookings b 
                //                                                    inner join trx_lines tl
                //                                                    on b.TrxId = tl.TrxId
                //                                                    inner join products p
                //                                                    on p.product_id = tl.product_id
                //                                                    inner join ComboProduct cb
                //                                                    on cb.ChildProductId = tl.product_id
                //                                                    left outer join ReservationDiscounts rd 
                //                                                    on rd.productId = tl.product_id and rd.BookingId = @BookingId
                //                                                    where
                //                                                    cb.AdditionalProduct ='Y' and ParentLineId is null
                //                                                    and 
                //                                                    b.BookingId =  @BookingId and cb.Product_Id = b.BookingProductId",
                //                                                    new SqlParameter("@BookingId", editedBookingId));


                //    foreach (DataRow packageDetails in dtBookedBookingProductDetails.Rows)
                //    {
                //        if (packageDetails["AdditionalProduct"].ToString() == "N")
                //        {
                //            foreach (DataGridViewRow packageRow in dgvPackageList.Rows)
                //            {
                //                if ((packageRow.Cells["ChildId"].Value != null) && (!(Convert.ToInt32(packageDetails["product_id"].ToString()).CompareTo(packageRow.Cells["ChildId"].Value) == 0)) && packageRow.Cells["SelectedStatus"].Value.ToString() == "true")
                //                {
                //                    audit.logEvent("Reservation", 'D', ParafaitEnv.LoginID, "Package selected:" + packageRow.Cells["packageName"].FormattedValue.ToString(),
                //                                    "Package Selection Screen", 0, "", BookingId.ToString(), null);
                //                }
                //                if ((packageRow.Cells["ChildId"].Value != null) && (Convert.ToInt32(packageDetails["product_id"].ToString()).CompareTo(packageRow.Cells["ChildId"].Value) == 0) && packageRow.Cells["SelectedStatus"].Value.ToString() == "true"
                //                                   && (!(Convert.ToInt32(packageDetails["quantity"]).CompareTo(packageRow.Cells["Quantity"].Value) == 0)))
                //                {
                //                    audit.logEvent("Reservation", 'D', ParafaitEnv.LoginID, "Quantity changed to:" + packageRow.Cells["Quantity"].Value + "for Package" + packageRow.Cells["packageName"].FormattedValue.ToString(),
                //                                                        "Package Selection Screen", 0, "", BookingId.ToString(), null);
                //                }
                //                if ((packageRow.Cells["ChildId"].Value != null) && (!(Convert.ToInt32(packageDetails["product_id"].ToString()).CompareTo(packageRow.Cells["ChildId"].Value) == 0)) && packageRow.Cells["SelectedStatus"].Value.ToString() == "true" &&
                //                                     (!(Convert.ToInt32(packageDetails["quantity"]).CompareTo(packageRow.Cells["Quantity"].Value) == 0)))
                //                {
                //                    audit.logEvent("Reservation", 'D', ParafaitEnv.LoginID, "Quantity changed to:" + packageRow.Cells["Quantity"].Value,
                //                                    "Package Selection Screen", 0, "", BookingId.ToString(), null);
                //                }
                //                //if ((!(Convert.ToInt32(packageDetails["product_id"].ToString()).CompareTo(packageRow.Cells["ChildId"].Value) == 0)) &&
                //                //                      (!(packageDetails["discountId"].ToString().CompareTo(packageRow.Cells["discountName"].Value) == 0)) &&
                //                //                      (packageRow.Cells["ChildId"].Value != null))
                //                //{
                //                //    audit.logEvent("Reservation", 'D', "logged in" + ParafaitEnv.LoginID, "Product level discount has changed",
                //                //                    "Package Selection Screen", 0, "", BookingId.ToString(), null);
                //                //}

                //                DataTable selectedCategoryProducts = POSStatic.Utilities.executeDataTable(@"Select tt.product_id,tt.quantity from trx_lines tt,  
                //                                                                            (select LineId,tl.TrxId from trx_lines tl, bookings b where tl.trxid  = b.TrxId
                //                                                                            and b.BookingId = @bookingId
                //                                                                            and tl.product_id = @ComboProductId)a
                //                                                                            where a.LineId = tt.ParentLineId and tt.TrxId = a.TrxId 
                //                                                                            and 
                //                                                                            not exists(Select cp.Product_Id from ComboProduct cp where 
                //                                                                            cp.ChildProductId = tt.product_id and cp.Product_Id = @ComboProductId)",
                //                                                                           new SqlParameter("@bookingId", editedBookingId),
                //                                                                           new SqlParameter("@ComboProductId", Convert.ToInt32(packageDetails["product_id"].ToString())));
                //                foreach (DataRow selectedProducts in selectedCategoryProducts.Rows)
                //                {
                //                    foreach (Semnox.Parafait.Transaction.Reservation.clsSelectedCategoryProducts item in CategoryProuctFinalSelectedList)
                //                    {
                //                        if (!(Convert.ToInt32(selectedProducts["product_id"].ToString()).CompareTo(item.productId) == 0) && !(Convert.ToInt32(packageDetails["product_id"].ToString()).CompareTo(item.parentComboProductId) == 0) && packageRow.Cells["SelectedStatus"].Value.ToString() == "true")
                //                        {
                //                            audit.logEvent("Reservation", 'D', ParafaitEnv.LoginID, "Category Product has changed",
                //                                            "Package Selection Screen", 0, "", BookingId.ToString(), null);
                //                        }
                //                    }
                //                }
                //            }
                //        }
                //    }

                //    foreach (DataRow AddonDetails in dtAdditionalProductsDetails.Rows)
                //    {
                //        if (AddonDetails["AdditionalProduct"].ToString() == "Y")
                //        {
                //            foreach (DataGridViewRow AdditionalRow in dgvBookingProducts.Rows)
                //            {
                //                if ((AdditionalRow.Cells["dcProduct"].Value) != null && !(Convert.ToInt32(AddonDetails["product_id"].ToString()).CompareTo(AdditionalRow.Cells["dcProduct"].Value) == 0))
                //                {
                //                    audit.logEvent("Reservation", 'D', ParafaitEnv.LoginID, "Additional Products Selected: " + AdditionalRow.Cells["dcProduct"].FormattedValue.ToString(),
                //                        "Additional Products Screen", 0, "", BookingId.ToString(), null);
                //                }
                //                if ((AdditionalRow.Cells["dcProduct"].Value) != null && (Convert.ToInt32(AddonDetails["product_id"].ToString()).CompareTo(AdditionalRow.Cells["dcProduct"].Value) == 0) &&
                //                                    (!(Convert.ToInt32(AddonDetails["quantity"]).CompareTo(Convert.ToInt32(AdditionalRow.Cells["dcQuantity"].Value)) == 0)))
                //                {
                //                    audit.logEvent("Reservation", 'D', ParafaitEnv.LoginID, "Additional Products Quantity has changed: " + AdditionalRow.Cells["dcQuantity"].Value,
                //                        "Additional Products Screen", 0, "", BookingId.ToString(), null);
                //                }
                //                if ((AdditionalRow.Cells["dcProduct"].Value) != null && !(Convert.ToInt32(AddonDetails["product_id"].ToString()).CompareTo(AdditionalRow.Cells["dcProduct"].Value) == 0) &&
                //                                     (!(Convert.ToInt32(AddonDetails["quantity"]).CompareTo(Convert.ToInt32(AdditionalRow.Cells["dcQuantity"].Value)) == 0)))
                //                {
                //                    audit.logEvent("Reservation", 'D', ParafaitEnv.LoginID, "Quantity Changed to: " + AdditionalRow.Cells["dcQuantity"].Value,
                //                        "Package Selection Screen", 0, "", BookingId.ToString(), null);
                //                }
                //            }
                //        }

                //    }
                //}
                //else
                //    audit.logEvent("Reservation", 'D', ParafaitEnv.LoginID, "Booking is Confirmed: Reservation Code is" + Reservation.ReservationCode, "ConfirmationScreen", 0, "", BookingId.ToString(), null);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
            log.LogMethodExit();
        }


        private void tcBooking_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            txtMessage.Clear();
            if (tcBooking.SelectedTab == tpAuditTrail)
            {
                log.Info("SelectedTab is tpAuditTrail");
                GetAuditTrail(BookingId);
            }
            if (BookingId == -1 || isEditedBooking || lblStatus.Text.ToUpper() == ReservationDTO.ReservationStatus.BLOCKED.ToString())
            {
                if (attractionScheduleId == -1)
                {
                    POSUtils.ParafaitMessageBox(MessageUtils.getMessage(943));
                    log.LogMethodExit("Schedule has not been selected");
                    return;
                }

                if(cmbBookingClass.SelectedValue == null)
                {
                    txtMessage.Text = MessageUtils.getMessage(1796);
                    return;
                }
                //Begin: validate Availablity here 
                if (!ValidateRequest(Convert.ToInt32(cmbBookingClass.SelectedValue), Convert.ToInt32(txtGuests.Text), cmbBookingClass.Text))
                {
                    log.LogMethodExit("Error in ValidateRequest ");
                    return;
                }
                //End
            }
            if (tcBooking.SelectedTab == tpPackage)
            {
                log.Info("SelectedTab is tpPackage");
                GetBookingProductsDefinition();//get the products listed within booking product
                GetBookedBookingProductItems();//get the Products booked if booking id is != -1
                //Begin:Check status of the quantity in the setup, if quantity was mentioned then cell becomes read only

                //if (isEditedBooking)
                //{
                //    foreach (DataGridViewRow dr in dgvPackageList.Rows)
                //    {
                //        //Commneted on Dec-07-2015 to remove quantity field from bieng read only//
                //        //! dr.Cells["Quantity"].ReadOnly && 
                //        if (guestCount != Convert.ToInt32(txtGuests.Text)) //g nudGuestCount.Value)
                //            dr.Cells["Quantity"].Value = txtGuests.Text; //g nudGuestCount.Value;
                //    }
                //}
                //end

                if (lstPacakgeIdList != null)
                {
                    for (int j = 0; j < lstPacakgeIdList.Count; j++)
                    {
                        for (int k = 0; k < dgvPackageList.Rows.Count; k++)
                        {
                            if (lstPacakgeIdList[j].productId.CompareTo(dgvPackageList.Rows[k].Cells["ChildId"].Value) == 0)
                            {
                                GetDiscounts(lstPacakgeIdList[j].productId, j);
                                dgvPackageList.Rows[k].Cells["SelectProduct"].Value = "Y";
                                dgvPackageList.Rows[k].Cells["SelectedStatus"].Value = "true";
                                dgvPackageList.Rows[k].Cells["Quantity"].Value = lstPacakgeIdList[j].guestQuantity;
                                dgvPackageList.Rows[k].Cells["discountName"].Value = lstPacakgeIdList[j].discountId;
                                dgvPackageList.Rows[k].Cells["transactionProfileId"].Value = lstPacakgeIdList[j].transactionProfileId;
                            }
                        }
                    }
                }

                GeneratePackageAttractionProductlist();
                GeneratePackageProductModifiers();

                //Begin:To get the Combo Category products selected//
                for (int j = 0; j < dgvPackageList.Rows.Count; j++)
                {
                    if (dgvPackageList.Rows[j].Cells["SelectedStatus"].Value != null && dgvPackageList.Rows[j].Cells["ChildId"].Value != null 
                        && dgvPackageList.Rows[j].Cells["SelectedStatus"].Value.ToString() == "true")
                    {
                        packageIdSelected = Convert.ToInt32(dgvPackageList.Rows[j].Cells["ChildId"].Value);
                        productQuantity = Convert.ToInt32(dgvPackageList.Rows[j].Cells["Quantity"].Value);
                        productType = dgvPackageList.Rows[j].Cells["ProductType"].Value.ToString();
                        try
                        {
                            GetAttractionNManualProdsForSelectedPackage(j, productType, packageIdSelected, productQuantity);
                            GetPackageContents();//get Package Contents
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                            dgvPackageList.Rows[j].Cells["SelectedStatus"].Value = "false";
                            RemovePackageFromList(packageIdSelected);
                            DataGridViewCheckBoxCell chkPackageproductId = (DataGridViewCheckBoxCell)dgvPackageList.Rows[j].Cells["SelectProduct"];
                            chkPackageproductId.Value = "N";
                        }
                        dgvPackageList.CurrentCell = dgvPackageList.Rows[j].Cells["packageName"];
                    }
                }
                GeneratePackageAttractionProductlist();
                GeneratePackageProductModifiers();
                //End
            }

            if (tcBooking.SelectedTab == tpAdditional)
            {
                log.Info("SelectedTab is tpAdditional");

                GenerateAdditionalAttractionProductlist();
                GenerateAdditionalProductModifiers();
                if (BookingId == -1 || isEditedBooking || lblStatus.Text.ToUpper() == ReservationDTO.ReservationStatus.BLOCKED.ToString())
                {
                    if (dtProducts.Rows.Count > 1)
                    {
                        OpenAdditionalProductsForm();//open the Additional products form to add Additional products
                    }
                }

            }
            //Begin: Added to load the product type for Additional products on changing the Tab to confirm
            if (tcBooking.SelectedTab == tpConfirm)
            {
                log.Info(" SelectedTab is tpConfirm");
                for (int i = 0; i < dgvBookingProducts.Rows.Count; i++)
                {
                    if (dgvBookingProducts["dcProduct", i].Value != null && dgvBookingProducts["type", i].Value == null)
                    {
                        DataTable productType = Utilities.executeDataTable(@"select pt.product_type from products p, product_type pt where product_id = @productId and p.product_type_id = pt.product_type_id",
                                                                            new SqlParameter("@productId", dgvBookingProducts["dcProduct", i].Value));
                        dgvBookingProducts["type", i].Value = productType.Rows[0]["product_type"];
                    }
                }
            }
            //End
            //Begin: If status is "new" then Enable Book and Execute button with text "Discounts"
            if (lblStatus.Text.ToUpper() == ReservationDTO.ReservationStatus.NEW.ToString() 
                || lblStatus.Text.ToUpper() == ReservationDTO.ReservationStatus.BLOCKED.ToString())
            {
                //btnExecute.Enabled = true;
                //btnExecute.Text = "Discounts";
                btnDiscounts.Enabled = true;
                btnTrxProfiles.Enabled = true; 
                btnBook.Enabled = true;
            }
            //End
            //Begin: If status is "WIP" then Enable Book button,disable confirm,payment and edit booking button
            if (lblStatus.Text.ToUpper() == ReservationDTO.ReservationStatus.WIP.ToString())
            {
                btnConfirm.Enabled = false;
                btnPayment.Enabled = false;
                btnEditBooking.Enabled = false;
                btnBook.Enabled = true;
            }
            //End
            log.LogMethodExit();
        }


        //Begin:To load the discounts for Addon products
        private void dgvBookingProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            //Begin Modification-Jan-14-2016-Added handle grid header click
            if (e.ColumnIndex < 0 || e.RowIndex < 0)
            {
                log.LogMethodExit("e.ColumnIndex < 0 || e.RowIndex < 0");
                return;
            }
            //End Modification-Jan-14-2016-Added handle grid header click

            if (dgvBookingProducts["dcProduct", e.RowIndex].Value != null && dgvBookingProducts.Columns[e.ColumnIndex].Name == "AdditionalDiscountName")
            {
                GetDiscounts(Convert.ToInt32(dgvBookingProducts["dcProduct", e.RowIndex].Value), e.RowIndex);
            }
            if (dgvBookingProducts["dcProduct", e.RowIndex].Value != null && dgvBookingProducts.Columns[e.ColumnIndex].Name == "dcProduct")
            {
                //GetDiscounts(Convert.ToInt32(dgvBookingProducts["dcProduct", e.RowIndex].Value), e.RowIndex);
                DataTable productType = Utilities.executeDataTable(@"select p.price,pt.product_type from products p, product_type pt where product_id = @productId and p.product_type_id = pt.product_type_id",
                                                                     new SqlParameter("@productId", dgvBookingProducts["dcProduct", e.RowIndex].Value));
                dgvBookingProducts["type", e.RowIndex].Value = productType.Rows[0]["product_type"];
                // dgvBookingProducts["dcPrice", e.RowIndex].Value = productType.Rows[0]["price"];
            }
            log.LogMethodExit();
        }
        // End

        private void dgvBookingProducts_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            //Written to handle DataGridViewComboBoxCell data error
            if (e.Exception.Message == "DataGridViewComboBoxCell value is not valid.")
            {
                object value = dgvBookingProducts.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
            }
            log.LogMethodExit();
        }

        private void txtContactNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (char.IsControl(e.KeyChar) == false)
            {
                if (e.KeyChar < '0' || e.KeyChar > '9')
                {
                    e.Handled = true;
                }
            }
            log.LogMethodExit();
        }

        private void BlackButtonMouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Button selectedButton = (Button)sender;
            selectedButton.BackgroundImage = Properties.Resources.pressed1;
            log.LogMethodExit();

        }

        private void BlackButtonMouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Button selectedButton = (Button)sender;
            selectedButton.BackgroundImage = Properties.Resources.normal1;
            log.LogMethodExit();
        }

        private void BlueButtonMouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Button selectedButton = (Button)sender;
            selectedButton.BackgroundImage = Properties.Resources.normal2;
            log.LogMethodExit();
        }

        private void BlueButtonMouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Button selectedButton = (Button)sender;
            selectedButton.BackgroundImage = Properties.Resources.pressed2;
            log.LogMethodExit();
        }

        private void dtpforDate_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            System.Windows.Forms.SendKeys.Send("%{DOWN}");
            log.LogMethodExit();
        }

        private void dtpRecurUntil_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            System.Windows.Forms.SendKeys.Send("%{DOWN}");
            log.LogMethodExit();
        }

        private void SetDGVCellFont(DataGridView dgvInput)
        {
            log.LogMethodEntry();

            System.Drawing.Font font;
            try
            {
                font = new Font(Utilities.ParafaitEnv.DEFAULT_GRID_FONT, 15, FontStyle.Regular);
            }
            catch (Exception ex)
            {
                log.Error("Error occured while applying new font", ex);
                font = new Font("Tahoma", 15, FontStyle.Regular);
            }
            foreach (DataGridViewColumn c in dgvInput.Columns)
            {
                c.DefaultCellStyle.Font = new Font(font.FontFamily, 15F, FontStyle.Regular);
            }
            log.LogMethodExit();
        }


        private List<SqlParameter> PrepareSearchParameters()
        {
            log.LogMethodEntry();
            List<SqlParameter> searchParamList = new List<SqlParameter>();
            //DateTime fromDate = dtpforDate.Value;
            //DateTime toDate = dtpforDate.Value;
            //List<ValidationError> validationErrorList = new List<ValidationError>();
            //if (cmbBookingClass.SelectedValue == null && BookingId == -1)
            //{
            //    validationErrorList.Add(new ValidationError(MessageContainer.GetMessage(Utilities.ExecutionContext, "Validation Error"),
            //                                                MessageContainer.GetMessage(Utilities.ExecutionContext, "Booking Product"),
            //                                                MessageContainer.GetMessage(Utilities.ExecutionContext, 1796)));
            //}
            //if (cmbFacility.SelectedValue == null && BookingId == -1)
            //{
            //    validationErrorList.Add(new ValidationError(MessageContainer.GetMessage(Utilities.ExecutionContext, "Validation Error"),
            //                                                MessageContainer.GetMessage(Utilities.ExecutionContext, "Facility"),
            //                                                MessageContainer.GetMessage(Utilities.ExecutionContext, 694)));
            //}
            //if (cmbFromTimeForSearch.SelectedValue == null)
            //{
            //    validationErrorList.Add(new ValidationError(MessageContainer.GetMessage(Utilities.ExecutionContext, "Validation Error"),
            //                                                 MessageContainer.GetMessage(Utilities.ExecutionContext, "From Time"),
            //                                                 MessageContainer.GetMessage(Utilities.ExecutionContext, "Select From Time")));
            //}
            //if (cmbToTimeForSearch.SelectedValue == null)
            //{
            //    validationErrorList.Add(new ValidationError(MessageContainer.GetMessage(Utilities.ExecutionContext, "Validation Error"),
            //                                                 MessageContainer.GetMessage(Utilities.ExecutionContext, "To Time"),
            //                                                 MessageContainer.GetMessage(Utilities.ExecutionContext, "Select To Time")));
            //}
            //if (validationErrorList != null && validationErrorList.Count > 0)
            //{
            //    throw new ValidationException(MessageContainer.GetMessage(Utilities.ExecutionContext, "Validation Error"), validationErrorList);
            //}
            //if (BookingId == -1 || lblStatus.Text.ToUpper() == ReservationDTO.ReservationStatus.BLOCKED.ToString())
            //{
            //    bool fromDateIsSet = false;
            //    if (cbxEarlyMorning.Checked)
            //    {
            //        fromDateIsSet = true;
            //        fromDate = dtpforDate.Value.Date;
            //        toDate = dtpforDate.Value.Date.AddHours(6);
            //    }
            //    if (cbxMorning.Checked)
            //    {
            //        if (fromDateIsSet == false)
            //        {
            //            fromDateIsSet = true;
            //            fromDate = dtpforDate.Value.Date.AddHours(6);
            //        }
            //        toDate = dtpforDate.Value.Date.AddHours(12);
            //    }
            //    if (cbxAfternoon.Checked)
            //    {
            //        if (fromDateIsSet == false)
            //        {
            //            fromDateIsSet = true;
            //            fromDate = dtpforDate.Value.Date.AddHours(12);
            //        }
            //        toDate = dtpforDate.Value.Date.AddHours(18);
            //    }
            //    if (cbxNight.Checked)
            //    {
            //        if (fromDateIsSet == false)
            //        {
            //            fromDateIsSet = true;
            //            fromDate = dtpforDate.Value.Date.AddHours(18);
            //        }
            //        toDate = dtpforDate.Value.Date.AddHours(24);
            //    }
            //    if (fromDateIsSet == false)
            //    {
            //        if (cmbFromTimeForSearch.SelectedIndex != 0)
            //        {
            //            fromDateIsSet = true;
            //            fromDate = dtpforDate.Value.Date.AddHours(Convert.ToDouble(cmbFromTimeForSearch.SelectedValue));

            //            if (Convert.ToDouble(cmbToTimeForSearch.SelectedValue) > Convert.ToDouble(cmbFromTimeForSearch.SelectedValue))
            //            {
            //                toDate = dtpforDate.Value.Date.AddHours(Convert.ToDouble(cmbToTimeForSearch.SelectedValue));
            //            }
            //            else
            //            {
            //                //ValidationError validationError = new ValidationError("Booking", "To Time", "From Time can not be greater than To Time");
            //                //List<ValidationError> validationErrorList = new List<ValidationError>();
            //                //validationErrorList.Add(validationError);
            //                throw new ValidationException(MessageContainer.GetMessage(Utilities.ExecutionContext, 305),
            //                                              MessageContainer.GetMessage(Utilities.ExecutionContext, "Validation Error"),
            //                                              MessageContainer.GetMessage(Utilities.ExecutionContext, "From Time"),
            //                                              MessageContainer.GetMessage(Utilities.ExecutionContext, 305));
            //            }
            //        }
            //    }
            //    if (fromDateIsSet == false)
            //    {
            //        fromDate = Utilities.getServerTime();
            //        toDate = Utilities.getServerTime().Date.AddDays(1);
            //    }
            //    searchParamList.Add(new SqlParameter("@Fromdate", fromDate.Date));
            //    searchParamList.Add(new SqlParameter("@FromdateTime", fromDate));
            //    searchParamList.Add(new SqlParameter("@TodateTime", toDate));
            //    searchParamList.Add(new SqlParameter("@FacilityId", cmbFacility.SelectedValue));
            //    // searchParamList.Add(new SqlParameter("@NoOfGuests", txtGuests.Text));
            //    searchParamList.Add(new SqlParameter("@BookingProductId", cmbBookingClass.SelectedValue));
            //    searchParamList.Add(new SqlParameter("@BookingId", (BookingId == -1 ? -1 : BookingId)));
            //}
            //else
            //{
            //    //already booked entry
            //    searchParamList.Add(new SqlParameter("@Fromdate", dtpforDate.Value.Date));
            //    if (cmbFromTimeForSearch.SelectedIndex != 0)
            //    {
            //        fromDate = dtpforDate.Value.Date.AddHours(Convert.ToDouble(cmbFromTimeForSearch.SelectedValue));
            //    }
            //    else
            //    {
            //        fromDate = dtpforDate.Value.Date;
            //    }
            //    searchParamList.Add(new SqlParameter("@FromdateTime", fromDate));
            //    searchParamList.Add(new SqlParameter("@TodateTime", dtpforDate.Value.Date.AddDays(1)));
            //    searchParamList.Add(new SqlParameter("@FacilityId", (cmbFacility.SelectedValue ?? ((Reservation != null && Reservation.GetReservationDTO != null) ? Reservation.GetReservationDTO.FacilityId : -1))));
            //    //searchParamList.Add(new SqlParameter("@NoOfGuests", txtGuests.Text));
            //    searchParamList.Add(new SqlParameter("@BookingProductId", (cmbBookingClass.SelectedValue ?? ((Reservation != null && Reservation.GetReservationDTO != null  )?  Reservation.GetReservationDTO.BookingProductId: -1))));
            //    searchParamList.Add(new SqlParameter("@BookingId", BookingId));
            //}
            //searchParamList.Add(new SqlParameter("@SiteId", Utilities.ExecutionContext.GetSiteId()));
            //searchParamList.Add(new SqlParameter("@LoginId", Utilities.ExecutionContext.GetUserId()));
            //searchParamList.Add(new SqlParameter("@POSMachineId", Utilities.ExecutionContext.GetMachineId()));

            log.LogMethodExit(searchParamList);
            return searchParamList;
        }
        void LoadSchedules()
        {
            log.LogMethodEntry();
            log.LogVariableState("userAction", userAction);
            if (userAction)
            {
                try
                {
                    List<SqlParameter> sqlSearchParams = PrepareSearchParameters();
                    SchedulesListBL scheduleList = new SchedulesListBL(Utilities.ExecutionContext);
                    dtBookingSchedule = new DataTable();// scheduleList.GetBookingScheduleList(sqlSearchParams);
                    dgvAttractionSchedules.DataSource = dtBookingSchedule;
                    //dgvAttractionSchedules.Columns["Selected"].ValueType()
                    dgvAttractionSchedules.Columns.RemoveAt(0);

                    DataGridViewCheckBoxColumn chk = new DataGridViewCheckBoxColumn
                    {
                        HeaderText = "Selected",
                        Name = "Selected",
                        DataPropertyName = "Selected"
                    };
                    dgvAttractionSchedules.Columns.Insert(0, chk);
                    Utilities.setupDataGridProperties(ref dgvAttractionSchedules);
                    //dgvAttractionSchedules.Columns["Selected"].ReadOnly = true;
                    dgvAttractionSchedules.Columns["product_id"].Visible =
                    // dgvAttractionSchedules.Columns["product_name"].Visible =
                    dgvAttractionSchedules.Columns["Price"].Visible =
                    dgvAttractionSchedules.Columns["MinimumTime"].Visible =
                    dgvAttractionSchedules.Columns["MasterScheduleName"].Visible =
                    dgvAttractionSchedules.Columns["AttractionScheduleId"].Visible =
                    dgvAttractionSchedules.Columns["Total_Units"].Visible =
                    dgvAttractionSchedules.Columns["Booked_Units"].Visible =
                    dgvAttractionSchedules.Columns["FacilityId"].Visible = false;
                    dgvAttractionSchedules.Columns["Fixed"].MinimumWidth = 40;
                    dgvAttractionSchedules.Columns["Fixed"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                    // dgvSchedules.BorderStyle = BorderStyle.FixedSingle;
                    DataGridViewCellStyle Expirestyle = new DataGridViewCellStyle();
                    Expirestyle.BackColor = Color.LightGray;
                    DataGridViewCellStyle style = new DataGridViewCellStyle();
                    style.BackColor = Color.LightCyan;
                    dgvAttractionSchedules.RowTemplate.DefaultCellStyle = style;
                    Utilities.setupDataGridProperties(ref dgvAttractionSchedules);
                    POSStatic.CommonFuncs.setupDataGridProperties(dgvAttractionSchedules);
                    dgvAttractionSchedules.BackgroundColor = this.BackColor;
                    DataGridViewCellStyle Style = new DataGridViewCellStyle();
                    Style.Font = new System.Drawing.Font(dgvAttractionSchedules.DefaultCellStyle.Font.Name, 15.0F, FontStyle.Regular);
                    dgvAttractionSchedules.DefaultCellStyle = Style;
                    if (dgvAttractionSchedules.Rows.Count > 0)
                    {
                        foreach (DataGridViewRow attractionScheduleRow in dgvAttractionSchedules.Rows)
                        {

                            if (attractionScheduleRow.Cells["From_Time"].Value != DBNull.Value && Convert.ToDateTime(attractionScheduleRow.Cells["From_Time"].Value) < Utilities.getServerTime().AddHours(-1))
                            {
                                attractionScheduleRow.ReadOnly = true;
                                attractionScheduleRow.DefaultCellStyle.BackColor = Color.Gray;
                            }
                            if (attractionScheduleRow.Cells["Available_Units"].Value != DBNull.Value
                                && (Convert.ToInt32(attractionScheduleRow.Cells["Available_Units"].Value) == 0
                                || Convert.ToInt32(attractionScheduleRow.Cells["Available_Units"].Value) < Convert.ToInt32(txtGuests.Text))
                                )
                            {
                                attractionScheduleRow.ReadOnly = true;
                                attractionScheduleRow.DefaultCellStyle.BackColor = Color.Gray;
                            }

                            if ((attractionScheduleRow.Cells[0].Value.ToString() == "1" && attractionScheduleId.ToString() == attractionScheduleRow.Cells["AttractionScheduleId"].Value.ToString())
                                || (attractionScheduleId.ToString() == attractionScheduleRow.Cells["AttractionScheduleId"].Value.ToString()
                                   && facilityId.ToString() == attractionScheduleRow.Cells["FacilityId"].Value.ToString()
                                   && cmbBookingClass.SelectedValue != null && cmbBookingClass.SelectedValue.ToString() == attractionScheduleRow.Cells["product_id"].Value.ToString()
                                   && attractionScheduleRow.Cells["From_Time"].Value != DBNull.Value && Convert.ToDateTime(attractionScheduleRow.Cells["From_Time"].Value) >= selectedFromTime
                                   && attractionScheduleRow.Cells["From_Time"].Value != DBNull.Value && Convert.ToDateTime(attractionScheduleRow.Cells["From_Time"].Value) <= selectedToTime
                                   )
                                )
                            {
                                attractionScheduleRow.Selected = true;
                                attractionScheduleRow.Cells[0].Selected = true;
                                if (attractionScheduleId.ToString() != attractionScheduleRow.Cells["AttractionScheduleId"].Value.ToString())
                                {
                                    attractionScheduleId = Convert.ToInt32(attractionScheduleRow.Cells["AttractionScheduleId"].Value);
                                }
                                if (facilityId.ToString() != attractionScheduleRow.Cells["FacilityId"].Value.ToString())
                                {
                                    facilityId = Convert.ToInt32(attractionScheduleRow.Cells["FacilityId"].Value);
                                }
                                if (attractionScheduleRow.Cells[0].Value.ToString() != "1")
                                {
                                    userAction = false;
                                    attractionScheduleRow.Cells[0].Value = "1";

                                    userAction = true;
                                }
                                this.dgvAttractionSchedules.CurrentCell = attractionScheduleRow.Cells[0];
                                // break;
                            }
                            else
                            {
                                if(attractionScheduleRow.Cells[0].Value.ToString() == "1" && attractionScheduleId.ToString() != attractionScheduleRow.Cells["AttractionScheduleId"].Value.ToString())
                                {
                                    userAction = false;
                                    attractionScheduleRow.Cells[0].Selected = false;
                                    attractionScheduleRow.Cells[0].Value = "0";
                                    userAction = true;
                                }
                            }
                        }
                    }
                }
                catch (ValidationException ex)
                {
                    string errMsg = ex.GetAllValidationErrorMessages();
                    POSUtils.ParafaitMessageBox(String.IsNullOrEmpty(errMsg) == true ? ex.Message : errMsg);

                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    POSUtils.ParafaitMessageBox(ex.Message);
                }
            }
            log.LogMethodExit();
        }

        private DateTime SelectedFromDateTime()
        {
            log.LogMethodEntry();
            double lclTime = Convert.ToDouble(cmbFromTime.SelectedValue);
            DateTime reservationFromDateTime = dtpforDate.Value.Date.AddMinutes(Math.Floor(lclTime) * 60).AddMinutes((lclTime - Math.Floor(lclTime)) * 100.0);
            log.LogMethodExit(reservationFromDateTime);
            return reservationFromDateTime;
        }

        private DateTime SelectedToDateTime()
        {
            log.LogMethodEntry();
            double lclTime = Convert.ToDouble(cmbToTime.SelectedValue);
            DateTime reservationFromDateTime = dtpforDate.Value.Date.AddMinutes(Math.Floor(lclTime) * 60).AddMinutes((lclTime - Math.Floor(lclTime)) * 100.0);
            log.LogMethodExit(reservationFromDateTime);
            return reservationFromDateTime;
        }

        private void LoadFacility()
        {
            log.LogMethodEntry();
            FacilityList facilityListBL = new FacilityList(Utilities.ExecutionContext);
            List<KeyValuePair<FacilityDTO.SearchByParameters, string>> facilitySearcParm = new List<KeyValuePair<FacilityDTO.SearchByParameters, string>>();
            facilitySearcParm.Add(new KeyValuePair<FacilityDTO.SearchByParameters, string>(FacilityDTO.SearchByParameters.SITE_ID, Utilities.ExecutionContext.GetSiteId().ToString()));
            facilitySearcParm.Add(new KeyValuePair<FacilityDTO.SearchByParameters, string>(FacilityDTO.SearchByParameters.ACTIVE_FLAG, "1"));
            List<FacilityDTO> facilityDTOList = facilityListBL.GetFacilityDTOList(facilitySearcParm);
            FacilityDTO firstfacilityDTO = new FacilityDTO
            {
                FacilityName = "- All -"
            };
            if (facilityDTOList != null && facilityDTOList.Count > 0)
            {
                facilityDTOList.Insert(0, firstfacilityDTO);
            }
            else
            {
                facilityDTOList = new List<FacilityDTO>();
                facilityDTOList.Add(firstfacilityDTO);
            }
            cmbFacility.DataSource = facilityDTOList;
            cmbFacility.DisplayMember = "FacilityName";
            cmbFacility.ValueMember = "FacilityId";
            cmbFacility.SelectedIndex = 0;
            log.LogMethodExit();
        }

        private void dgvAttractionSchedules_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (dgvAttractionSchedules.CurrentCell != null)// && dgvAttractionSchedules.Columns[e.ColumnIndex].Name != "Selected")
            {
                dgvAttractionSchedules.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly = true;
            }
            log.LogMethodExit();
        }

        private void cbxTimeSlots_CheckedChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            LoadSchedules();
            log.LogMethodExit();
        }

        private void cmbFacility_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (userAction)
            {
                attractionScheduleId = -1;
                cmbFromTime.Enabled = cmbToTime.Enabled = false;
                LoadSchedules();
            }
            log.LogMethodExit();
        }

        private void txtGuests_Validating(object sender, CancelEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ValidateGuestNoEntry();
            txtGuests.Focus();
            log.LogMethodExit();
        }

        private int ValidateGuestNoEntry()
        {
            log.LogMethodEntry();
            int numberValidation;
            if (Int32.TryParse(txtGuests.Text, out numberValidation) == false)
            {
                POSUtils.ParafaitMessageBox(MessageContainer.GetMessage(Utilities.ExecutionContext, "Please enter valid number"));
                txtGuests.Text = "1";
            }
            log.LogMethodExit(numberValidation);
            return numberValidation;

        }

        private void btnRedueceGuestCount_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            txtMessage.Clear();
            int guestCount = ValidateGuestNoEntry();
            if (guestCount <= 1)
            {
                return;
            }
            else
            {
                txtGuests.Text = (guestCount - 1).ToString();
            }
            log.LogMethodExit();
        }

        private void btnIncreaseGuestCount_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            txtMessage.Clear();
            int guestCount = ValidateGuestNoEntry();
            if (guestCount >= 9999)
            {
                return;
            }
            else
            {
                txtGuests.Text = (guestCount + 1).ToString();
            }
            log.LogMethodExit();
        }

        private void txtGuests_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            int guestNo = (int)NumberPadForm.ShowNumberPadForm(MessageUtils.getMessage(1834), txtGuests.Text, Utilities);//"Please enter Guest count"
            if (guestNo > 0 && guestNo < 10000)
            {
                txtGuests.Text = guestNo.ToString(); 
            }
            this.ActiveControl = btnIncreaseGuestCount;
            log.LogMethodExit();

        }

        private void dgvAttractionSchedules_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (dgvAttractionSchedules.CurrentCell != null && e != null && e.RowIndex > -1 
                && dgvAttractionSchedules.Columns[e.ColumnIndex].Name == "Selected")
            {
                if (dgvAttractionSchedules["From_Time", e.RowIndex].Value != DBNull.Value
                    && Convert.ToDateTime(dgvAttractionSchedules["From_Time", e.RowIndex].Value) >= Utilities.getServerTime().AddHours(-1) )
                {
                    if (lblStatus.Text.ToUpper() == ReservationDTO.ReservationStatus.NEW.ToString()
                        || lblStatus.Text.ToUpper() == ReservationDTO.ReservationStatus.BLOCKED.ToString()
                        || lblStatus.Text.ToUpper() == ReservationDTO.ReservationStatus.WIP.ToString())
                    {
                        DataGridViewCheckBoxCell checkBox = (dgvAttractionSchedules["Selected", e.RowIndex] as DataGridViewCheckBoxCell);
                        bool rowSelected = false;
                        if (Convert.ToBoolean(checkBox.Value))
                        {
                            checkBox.Value = false;
                            attractionScheduleId = -1;
                            selectedFromTime = selectedToTime = DateTime.MaxValue;
                            //cmbFromTime.Enabled = cmbToTime.Enabled = false;
                            //btnFind.Enabled = false;
                        }
                        else
                        {
                            rowSelected = true;
                            checkBox.Value = true;
                            UnCheckOtherSelectedSchedules(e.RowIndex);
                            SetSelectedScheduleTime(e.RowIndex);
                        }
                        dgvAttractionSchedules.EndEdit();
                        dgvAttractionSchedules.CurrentCell = dgvAttractionSchedules["Facility_Name", e.RowIndex];
                        if (rowSelected)
                        {
                            CheckScheduleAvailablity();
                        }
                        LoadSchedules();
                        dgvAttractionSchedules.RefreshEdit();
                    }
                }
            }
            log.LogMethodExit();
        }

        private void UnCheckOtherSelectedSchedules(int rowIndex)
        {
            log.LogMethodEntry(rowIndex);
            int currentRowIndex = 0;
            foreach (DataGridViewRow selectedRow in dgvAttractionSchedules.Rows)
            {
                if (selectedRow.Cells["Selected"] != null && selectedRow.Cells["Selected"].Value.ToString() == "1" && currentRowIndex != rowIndex)
                {
                    DataGridViewCheckBoxCell checkBox = (dgvAttractionSchedules["Selected", currentRowIndex] as DataGridViewCheckBoxCell);
                    if (Convert.ToBoolean(checkBox.Value))
                    {
                        checkBox.Value = false;
                    }
                }
                currentRowIndex++;
            }
            log.LogMethodExit();
        }

        private void SetSelectedScheduleTime(int rowIndex)
        {
            log.LogMethodEntry(rowIndex);
            if (dgvAttractionSchedules.Rows[rowIndex] != null)
            {
                userAction = false;
                lstAttractionProductslist.Clear();
                lstPurchasedModifierProducts.Clear();
                lstAdditionalAttractionProductsList.Clear();
                lstPurchasedAdditionalModifierProducts.Clear();
                guestCount = -99999;
                DataGridViewRow selectedRow = dgvAttractionSchedules.Rows[rowIndex];
                DateTime fromScheduletime = Convert.ToDateTime(selectedRow.Cells["From_Time"].Value);
                DateTime toScheduleTime = fromScheduletime.Date.AddDays(1);
                facilityId = Convert.ToInt32(selectedRow.Cells["FacilityId"].Value);
                cmbFacility.SelectedValue = selectedRow.Cells["FacilityId"].Value;
                cmbBookingClass.SelectedValue = selectedRow.Cells["product_id"].Value;
                selectedFromTime = fromScheduletime;
                selectedToTime = toScheduleTime;
                DateTime startTime = fromScheduletime;
                DataTable dtFromTimeCopy = dtFromTime.Copy();
                for (int i = 0; i < dtFromTimeCopy.Rows.Count; i++)
                {
                    //startTime.ToString("h:mm tt"), startTime.Hour + Math.Round(startTime.Minute / 100.0, 2), startTime
                    dtFromTimeCopy.Rows[i]["Display"] = startTime.ToString("h:mm tt");
                    dtFromTimeCopy.Rows[i]["Value"] = startTime.Hour + Math.Round(startTime.Minute / 100.0, 2);
                    dtFromTimeCopy.Rows[i]["Compare"] = startTime;
                    startTime = startTime.AddMinutes(5);
                }

                


                //End: Future Date is selected//
                var result = from r in dtFromTimeCopy.AsEnumerable()
                             where r.Field<DateTime>("Compare") >= Convert.ToDateTime(selectedRow.Cells["From_Time"].Value) &&
                             r.Field<DateTime>("Compare") <= Convert.ToDateTime(selectedRow.Cells["To_Time"].Value)
                             select r;

                if (Convert.ToDateTime(selectedRow.Cells["From_Time"].Value).Hour > Convert.ToDateTime(selectedRow.Cells["To_Time"].Value).Hour)
                {
                    bsFromTime.DataSource = dtFromTimeCopy;
                    cmbFromTime.DataSource = bsFromTime;
                }
                else
                {
                    DataTable dtFromResult = result.CopyToDataTable();
                    DataTable dtToResult = result.CopyToDataTable();
                    bsFromTime.DataSource = dtFromResult;
                    cmbFromTime.DataSource = bsFromTime;
                    bsToTime.DataSource = dtToResult;
                    cmbToTime.DataSource = bsToTime;
                }

                DateTime currentTime = Utilities.getServerTime();
                attractionScheduleId = Convert.ToInt32(selectedRow.Cells["attractionScheduleId"].Value);
                bookingProductPrice = Convert.ToDouble((selectedRow.Cells["Price"].Value == DBNull.Value ? 0: selectedRow.Cells["Price"].Value));
                cmbFromTime.SelectedValue = Convert.ToDouble(Convert.ToDateTime(selectedRow.Cells["From_Time"].Value).Hour + "." + Convert.ToDateTime(selectedRow.Cells["From_Time"].Value).Minute);
                cmbToTime.SelectedValue = Convert.ToDouble(Convert.ToDateTime(selectedRow.Cells["To_Time"].Value).Hour + "." + Convert.ToDateTime(selectedRow.Cells["To_Time"].Value).Minute);
                //End
                //If schedule is fixed then disable from and to time
                if (selectedRow.Cells["Fixed"].Value.ToString() == "Y")
                {
                    //cmbToTime.SelectedValue = Convert.ToDouble(Convert.ToDateTime(reservationSchedule.toTime).Hour + "." + Convert.ToDateTime(reservationSchedule.toTime).Minute);
                    cmbFromTime.Enabled = cmbToTime.Enabled = false;
                    btnFind.Enabled = true;
                }
                else
                {
                    cmbFromTime.Enabled = false;
                    cmbFromTime.Enabled = cmbToTime.Enabled = true;
                    btnFind.Enabled = true;
                }
                userAction = true;
            }

            log.LogMethodExit();
        }

        private void dtpforDate_ValueChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            LoadSchedules();
            log.LogMethodExit();
        }

        private void txtGuests_TextChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            LoadSchedules();
            log.LogMethodExit();
        }

        private void cmbFromOrTOTimeForSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ResetTimeSlots();
            LoadSchedules();
            log.LogMethodExit();
        }

        private void txtCardNumber_TextChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            //if(userAction)
            //{
            //    ValidateCard();
            //}
            log.LogMethodExit();
        }

        private void GetTransactionProfiles()
        {
            log.LogMethodEntry();
            TransactionProfileListBL transactionProfileListBL = new TransactionProfileListBL();
            List<KeyValuePair<TransactionProfileDTO.SearchByParameters, string>> searchParam = new List<KeyValuePair<TransactionProfileDTO.SearchByParameters, string>>();
            searchParam.Add(new KeyValuePair<TransactionProfileDTO.SearchByParameters, string>(TransactionProfileDTO.SearchByParameters.IS_ACTIVE, "1"));
            searchParam.Add(new KeyValuePair<TransactionProfileDTO.SearchByParameters, string>(TransactionProfileDTO.SearchByParameters.SITE_ID, Utilities.ExecutionContext.GetSiteId().ToString()));
            List<TransactionProfileDTO> transactionProfileDTOList = transactionProfileListBL.GetTransactionProfileDTOList(searchParam);
            if (transactionProfileDTOList == null || transactionProfileDTOList.Count == 0)
            {
                transactionProfileDTOList = new List<TransactionProfileDTO>();
            }
            TransactionProfileDTO transactionProfileDTO = new TransactionProfileDTO
            {
                ProfileName = "<SELECT>"
            };
            transactionProfileDTOList.Insert(0, transactionProfileDTO);
            //trxProfilesBindingSource.DataMember = "TransactionProfileId";
            this.transactionProfileId.DataSource = this.additionalTransactionProfileId.DataSource = transactionProfileDTOList;
            this.transactionProfileId.DisplayMember = this.additionalTransactionProfileId.DisplayMember = "ProfileName";
            this.transactionProfileId.ValueMember = this.additionalTransactionProfileId.ValueMember = "TransactionProfileId";
            log.LogMethodExit();
        }

        private void dgvPackageList_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (dgvPackageList.Columns[e.ColumnIndex].Name == "discountName"
                || dgvPackageList.Columns[e.ColumnIndex].Name == "trasactionProfileId")
            {
                return;
            }
            log.LogMethodExit();
        }

        private void btnBlockSchedule_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            txtMessage.Clear();
            try
            {
                txtMessage.Text = "";
                ValidateDateAndTimePageData();
                decimal toTime = Convert.ToDecimal(cmbToTime.SelectedValue);
                decimal fromTime = Convert.ToDecimal(cmbFromTime.SelectedValue);
                double lclTime = Convert.ToDouble(cmbFromTime.SelectedValue);
                double tolclTime = Convert.ToDouble(cmbToTime.SelectedValue);
                DateTime fromDateTime = dtpforDate.Value.Date.AddMinutes(Math.Floor(lclTime) * 60).AddMinutes((lclTime - Math.Floor(lclTime)) * 100.0);
                DateTime toDateTime;
                if (tolclTime > lclTime)
                {
                    toDateTime = dtpforDate.Value.Date.AddMinutes(Math.Floor(tolclTime) * 60).AddMinutes((tolclTime - Math.Floor(tolclTime)) * 100.0);
                }
                else
                {
                    toDateTime = dtpforDate.Value.Date.AddDays(1).AddMinutes(Math.Floor(tolclTime) * 60).AddMinutes((tolclTime - Math.Floor(tolclTime)) * 100.0);
                }
                int cardId = -1;
                string cardNumber = "";
                int customerId = -1;
                string customerName = "";
                if (String.IsNullOrEmpty(txtCardNumber.Text) == false)
                {
                    Card custCard = new Card(txtCardNumber.Text, Utilities.ExecutionContext.GetUserId(), Utilities);
                    if (custCard != null && custCard.CardStatus != "NEW" && custCard.technician_card.Equals("N"))
                    {
                        cardId = custCard.card_id;
                        cardNumber = custCard.CardNumber;
                    }
                }
                if (customerDTO != null)
                {
                    customerId = customerDTO.Id;
                    customerName = customerDTO.FirstName;
                }
                if (BookingId == -1 || lblStatus.Text.ToUpper() == ReservationDTO.ReservationStatus.BLOCKED.ToString())
                {
                    if (reservationDTO == null)
                    {
                        //reservationDTO = new ReservationDTO(-1, -1, txtBookingName.Text, fromDateTime, "N", "", DateTime.MinValue, Convert.ToInt32(txtGuests.Text), "", ReservationDTO.ReservationStatus.BLOCKED.ToString(), cardId,
                        //                                  cardNumber, customerId, customerName, Utilities.getServerTime().AddMinutes(15), cmbChannel.SelectedItem.ToString(), txtRemarks.Text,
                        //                                  Utilities.ExecutionContext.GetUserId(), DateTime.Now, Utilities.ExecutionContext.GetUserId(), DateTime.Now, "", false, Utilities.ExecutionContext.GetSiteId(),
                        //                                  "", "", "", -1, toDateTime, -1, -1, "", "", Convert.ToInt32(cmbBookingClass.SelectedValue), attractionScheduleId, -1, -1, "", "", null, -1, "", "");
                    }
                    else
                    {
                        reservationDTO.FromDate = fromDateTime;
                        reservationDTO.Quantity = Convert.ToInt32(txtGuests.Text);
                        reservationDTO.CardId = cardId;
                        reservationDTO.CardNumber = cardNumber;
                        reservationDTO.CustomerId = customerId;
                        reservationDTO.CustomerName = customerName;
                        reservationDTO.ExpiryTime = Utilities.getServerTime().AddMinutes(15);
                        reservationDTO.Channel = cmbChannel.SelectedItem.ToString();
                        reservationDTO.Remarks = txtRemarks.Text;
                        reservationDTO.ToDate = toDateTime;
                        reservationDTO.BookingProductId = Convert.ToInt32(cmbBookingClass.SelectedValue);
                        reservationDTO.AttractionScheduleId = attractionScheduleId;
                    }
                   // Semnox.Parafait.Transaction.Reservation reservation = new Semnox.Parafait.Transaction.Reservation(reservationDTO, Utilities);
                   // reservation.Save();
                    audit = new Semnox.Core.Utilities.EventLog(Utilities);
                    audit.logEvent("Reservation", 'D', loginId, "Schedule is blocked successfully, reservation code  is " + reservationDTO.ReservationCode, "DateTime Screen", 0, "", reservationDTO.BookingId.ToString(), null);
                    POSUtils.ParafaitMessageBox(MessageUtils.getMessage(1810, reservationDTO.ReservationCode, (reservationDTO.ExpiryTime != null ? Convert.ToDateTime(reservationDTO.ExpiryTime).ToString(Utilities.getDateTimeFormat()) : "")));
                    //Scheduke is blocked with reservation code " + reservationDTO.ReservationCode + " and it will expire by " + reservationDTO.ExpiryTime.ToString(Utilities.getDateTimeFormat()
                    txtMessage.Text = MessageUtils.getMessage(1810, reservationDTO.ReservationCode, (reservationDTO.ExpiryTime != null ? Convert.ToDateTime(reservationDTO.ExpiryTime).ToString(Utilities.getDateTimeFormat()) : ""));
                    BookingId = reservationDTO.BookingId;
                    lblStatus.Text = reservationDTO.Status;
                    lblReservationCode.Text = reservationDTO.ReservationCode;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void ApplyTrxProfile(Transaction trx, List<Semnox.Parafait.Transaction.Reservation.clsPackageList> lstPackageList, List<Semnox.Parafait.Transaction.Reservation.clsProductList> lstAddonProducts)
        {
            log.LogMethodEntry(lstPackageList, lstAddonProducts);
            if (lblAppliedTrxProfileName.Tag != null && lblAppliedTrxProfileName.Tag.ToString() != "-1")
            {
                GetTrxProfileInfo(Convert.ToInt32(lblAppliedTrxProfileName.Tag), trx);
                trx.ApplyProfile(Convert.ToInt32(lblAppliedTrxProfileName.Tag), null, true);
            }
            else
            {
                List<Semnox.Parafait.Transaction.Reservation.clsPackageList> lstPackageListWithTrxPrfile = lstPackageList.FindAll(packProduct => packProduct.transactionProfileId != -1);
                log.LogVariableState("lstPackageListWithTrxPrfile", lstPackageListWithTrxPrfile);
                List<Tuple<int, int, string, string, string>> trxProfileIdInfo = new List<Tuple<int, int, string, string, string>>();
                //transactionProfileId, productId, VerfificationRequired?, trxProfileVerificationData.Item1, trxProfileVerificationData.Item2
                if (lstPackageListWithTrxPrfile != null)
                {
                    DataTable dtSelectedPackageProducts = new DataTable(); // Reservation.GetSelectedPackageBookingProducts(Reservation.BookingId);
                    foreach (Semnox.Parafait.Transaction.Reservation.clsPackageList packageProduct in lstPackageListWithTrxPrfile)
                    {
                        string sqlWhereClause = "product_id = " + packageProduct.productId.ToString();
                        DataRow[] selectedRows = dtSelectedPackageProducts.Select(sqlWhereClause);
                        foreach (DataRow selectedProductRow in selectedRows)
                        {
                            int productId = Convert.ToInt32(selectedProductRow["product_id"]);
                            int lineId = Convert.ToInt32(selectedProductRow["lineId"]);
                            List<Transaction.TransactionLine> prodctTrxLines = trx.TrxLines.FindAll(trxLine => trxLine.ProductID == productId && trxLine.DBLineId == lineId);
                            log.LogVariableState("prodctTrxLines", prodctTrxLines);
                            ApplyTrxProfileToTrxLine(trx, trxProfileIdInfo, prodctTrxLines, packageProduct.productId, packageProduct.transactionProfileId);
                        }

                    }
                }
                List<Semnox.Parafait.Transaction.Reservation.clsProductList> lstAddonProductsWithTrxPrfile = lstAddonProducts.FindAll(addOnProduct => addOnProduct.transactionProfileId != -1);
                log.LogVariableState("lstAddonProductsWithTrxPrfile", lstAddonProductsWithTrxPrfile);
                if (lstAddonProductsWithTrxPrfile != null)
                {
                    DataTable dtSelectedAdditionalProducts = new DataTable(); // Reservation.GetSelectedAdditionalBookingProducts(Reservation.BookingId);
                    foreach (Semnox.Parafait.Transaction.Reservation.clsProductList addOnProduct in lstAddonProductsWithTrxPrfile)
                    {
                        string sqlWhereClause = "product_id = " + addOnProduct.ProductId.ToString();
                        DataRow[] selectedRows = dtSelectedAdditionalProducts.Select(sqlWhereClause);
                        foreach (DataRow selectedProductRow in selectedRows)
                        {
                            int productId = Convert.ToInt32(selectedProductRow["product_id"]);
                            int lineId = Convert.ToInt32(selectedProductRow["lineId"]);
                            List<Transaction.TransactionLine> addOnprodctTrxLines = trx.TrxLines.FindAll(trxLine => trxLine.ProductID == productId && trxLine.DBLineId == lineId);
                            log.LogVariableState("addOnprodctTrxLines", addOnprodctTrxLines);
                            ApplyTrxProfileToTrxLine(trx, trxProfileIdInfo, addOnprodctTrxLines, addOnProduct.ProductId, addOnProduct.transactionProfileId);
                        }
                    }
                }
            }
            log.LogMethodExit();
        }

        private void GetTrxProfileInfo(int transactionProfileId, Transaction trx)
        {
            log.LogMethodEntry(transactionProfileId, trx);
            TransactionProfileBL transactionProfileBL = new TransactionProfileBL(transactionProfileId); 
            if (transactionProfileBL.TransactionProfileDTO.VerificationRequired)
            {
                bool dataReceived = false;
                do
                {
                    try
                    {
                        //userVerificationId, userVerificationName
                        Tuple<string, string> trxProfileVerificationData = InvokeTrxProfileUserVerification(transactionProfileId);
                        dataReceived = (String.IsNullOrEmpty(trxProfileVerificationData.Item1) == false);
                        //trxProfileData = new Tuple<int, int, string, string, string>(transactionProfileId, productId, "Y", trxProfileVerificationData.Item1, trxProfileVerificationData.Item2);
                        if (trx.TrxLines[0] != null)
                        {
                            trx.TrxLines[0].userVerificationId = trxProfileVerificationData.Item1;
                            trx.TrxLines[0].userVerificationName = trxProfileVerificationData.Item2;
                        }
                        else
                        {
                            log.LogVariableState("trx.TrxLines[0] == null", trx);
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        dataReceived = false;
                    }
                }
                while (dataReceived == false);
            } 
            log.LogMethodExit();
        }

        private void ApplyTrxProfileToTrxLine(Transaction trx, List<Tuple<int, int, string, string, string>> trxProfileIdInfo, List<Transaction.TransactionLine> prodctTrxLines, int productId, int transactionProfileId)
        {
            log.LogMethodEntry(trxProfileIdInfo, prodctTrxLines, productId, transactionProfileId);
            if (prodctTrxLines != null)
            {
                foreach (Transaction.TransactionLine trxLine in prodctTrxLines)
                {
                    if (trxLine.TrxProfileId == -1 || trxLine.TrxProfileId != transactionProfileId)
                    {
                        Tuple<int, int, string, string, string> trxProfileData = trxProfileIdInfo.Find(t => t.Item1 == transactionProfileId);
                        if (trxProfileData == null)
                        {
                            trxProfileData = GetTrxProfileInfo(transactionProfileId, productId);
                            trxProfileIdInfo.Add(trxProfileData);
                        }
                        if (trxProfileData.Item3 == "Y")
                        {
                            trxLine.userVerificationId = trxProfileData.Item4;
                            trxLine.userVerificationName = trxProfileData.Item5;
                        }
                        trx.ApplyProfile(transactionProfileId, trxLine, true);
                    }
                }
            }
            log.LogMethodExit();
        }

        private Tuple<int, int, string, string, string> GetTrxProfileInfo(int transactionProfileId, int productId)
        {
            log.LogMethodEntry(transactionProfileId, productId);
            TransactionProfileBL transactionProfileBL = new TransactionProfileBL(transactionProfileId);
            Tuple<int, int, string, string, string> trxProfileData = null;
            if (transactionProfileBL.TransactionProfileDTO.VerificationRequired)
            {
                bool dataReceived = false;
                do
                {
                    try
                    {
                        //userVerificationId, userVerificationName
                        Tuple<string, string> trxProfileVerificationData = InvokeTrxProfileUserVerification(transactionProfileId);
                        dataReceived = (String.IsNullOrEmpty(trxProfileVerificationData.Item1) == false);
                        trxProfileData = new Tuple<int, int, string, string, string>(transactionProfileId, productId, "Y", trxProfileVerificationData.Item1, trxProfileVerificationData.Item2);
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        dataReceived = false;
                    }
                }
                while (dataReceived == false);
            }
            else
            {
                trxProfileData = new Tuple<int, int, string, string, string>(transactionProfileId, productId, "N", "", "");
            }
            log.LogMethodExit(trxProfileData);
            return trxProfileData;
        }

        private Tuple<string, string> InvokeTrxProfileUserVerification(int trxProfileId)
        {
            log.LogMethodEntry(trxProfileId);
            Tuple<string, string> verificationData = new Tuple<string, string>("", "");//userVerificationId, userVerificationName
            using (GenericDataEntry trxProfileVerify = new GenericDataEntry(2))
            {
                trxProfileVerify.Text = MessageUtils.getMessage(1823);// "Transaction Profile Verification";
                trxProfileVerify.DataEntryObjects[0].mandatory = true;
                trxProfileVerify.DataEntryObjects[0].label = MessageUtils.getMessage("TIN Number");
                trxProfileVerify.DataEntryObjects[0].dataType = GenericDataEntry.DataTypes.String;
                trxProfileVerify.DataEntryObjects[1].mandatory = true;
                trxProfileVerify.DataEntryObjects[1].label = MessageUtils.getMessage("Name");
                trxProfileVerify.DataEntryObjects[1].dataType = GenericDataEntry.DataTypes.String;

                if (trxProfileVerify.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string userVerificationId = trxProfileVerify.DataEntryObjects[0].data;
                    string userVerificationName = trxProfileVerify.DataEntryObjects[1].data;
                    if (string.Empty.Equals(userVerificationId) || string.Empty.Equals(userVerificationName))
                    {
                        POSUtils.ParafaitMessageBox(MessageUtils.getMessage(1822));//TIN Number or Name is not entered. Both are Mandatory
                        log.LogVariableState("VerificationId", userVerificationId);
                    }
                    else
                    {
                        verificationData = new Tuple<string, string>(userVerificationId, userVerificationName);
                    }
                }
            }
            log.LogMethodExit(verificationData);
            return verificationData;
        }

        private void btnAddAttendeesInSummary_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            txtMessage.Clear();
            using (frmBookingAttendees frb = new frmBookingAttendees(BookingId, txtBookingName.Text + "-" + lblReservationCode.Text, customerDetailUI.FirstName, Utilities.getServerTime().Date.AddHours(Convert.ToDouble(cmbFromTime.SelectedValue)), Utilities.getServerTime().Date.AddHours(Convert.ToDouble(cmbToTime.SelectedValue))))
            {
                frb.ShowDialog();
            }
            log.LogMethodExit();
        }

        private void btnDiscounts_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            txtMessage.Clear();
            //using (ReservationDiscountsListUI reservationDiscountsListUI = new ReservationDiscountsListUI(Utilities, BookingId))
            //{
            //    reservationDiscountsListUI.ShowDialog();
            //    reservationTransactionDiscounts = new List<DiscountsDTO>(reservationDiscountsListUI.SelectedDiscountsDTOList);
            //}
            log.LogMethodExit();
        }

        private void btnTrxProfiles_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            txtMessage.Clear();
            if (TrxProfileAlreadyApplied(dgvPackageList) || TrxProfileAlreadyApplied(dgvBookingProducts))
            {
                POSUtils.ParafaitMessageBox(MessageUtils.getMessage(1809));
                //"You have already applied Transaction Profiles on Package products or Additional products. Can not proceed without clearing them"
                return;
            }
            using (frmChooseTrxProfile fct = new frmChooseTrxProfile())
            {
                try
                {
                    fct.Text = MessageContainer.GetMessage(Utilities.ExecutionContext, "Transaction Profiles");
                    if (fct.DialogResult == System.Windows.Forms.DialogResult.OK)
                    {
                        lblAppliedTrxProfileName.Tag = fct.TrxProfileId;
                        lblAppliedTrxProfileName.Text = fct.TrxProfileName.ToString();
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
            }
            log.LogMethodExit();
        }

        private bool TrxProfileAlreadyApplied(DataGridView productsDGV)
        {
            log.LogMethodEntry(productsDGV);
            bool trxProfileApplied = false;

            if (productsDGV.Rows.Count > 0)
            {
                foreach (DataGridViewRow dataRow in productsDGV.Rows)
                {

                    if (productsDGV.Name == "dgvPackageList")
                    {
                        if ((dataRow.Cells["transactionProfileId"].Value != null && dataRow.Cells["transactionProfileId"].Value != DBNull.Value)
                            && Convert.ToInt32(dataRow.Cells["transactionProfileId"].Value) > -1)
                        {
                            trxProfileApplied = true;
                            break;
                        }
                    }
                    else if (productsDGV.Name == "dgvBookingProducts")
                    {
                        if ((dataRow.Cells["additionalTransactionProfileId"].Value != null && dataRow.Cells["additionalTransactionProfileId"].Value != DBNull.Value)
                            && Convert.ToInt32(dataRow.Cells["additionalTransactionProfileId"].Value) > -1)
                        {
                            trxProfileApplied = true;
                            break;
                        }
                    }
                }
            }

            log.LogMethodExit(trxProfileApplied);
            return trxProfileApplied;
        }

        private void btnAddServiceCharge_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            txtMessage.Clear();
            try
            {
                ServiceChargeIsAlreadyAdded();
                AddServiceChargeProduct();
            }
            catch (ValidationException ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(ex.GetAllValidationErrorMessages());
            }
            catch (Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(MessageUtils.getMessage(1824, ex.Message));// "Unexpected error while performing the action. " + ex.Message;
            }
            log.LogMethodExit();
        }

        private void AddServiceChargeProduct()
        {
            log.LogMethodEntry();
            txtMessage.Text = "";
            if (cmbBookingClass.SelectedValue != null)
            {
                ProductsList productsListBL = new ProductsList(Utilities.ExecutionContext);
                List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>> searchParams = new List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>>();
                searchParams.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.PRODUCT_TYPE_NAME, "SERVICECHARGE"));
                searchParams.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.SITEID, Utilities.ExecutionContext.GetSiteId().ToString()));
                List<ProductsDTO> productsDTOLIst = productsListBL.GetProductsDTOList(searchParams);
                if (productsDTOLIst != null && productsDTOLIst.Count > 0)
                {
                    //  dgvBookingProducts.Rows.Add(delete btn , productId, qty, price, prod type, bookingProdId, discountName, trxprofId, remarks
                    dgvBookingProducts.Rows.Add(null, productsDTOLIst[0].ProductId, 1, productsDTOLIst[0].Price, productsDTOLIst[0].ProductType, (cmbBookingClass.SelectedValue), -1, -1, "");
                }
                else
                {
                    //"Service Charge Product setup is missing"
                    throw new ValidationException(MessageUtils.getMessage(1825));
                }
            }
            else
            {
                txtMessage.Text = MessageUtils.getMessage(1796);
            }
            log.LogMethodExit();
        }

        private void ServiceChargeIsAlreadyAdded()
        {
            log.LogMethodEntry();
            foreach (DataGridViewRow additionalProductRow in dgvBookingProducts.Rows)
            {
                if (additionalProductRow.Cells["type"].Value != null && additionalProductRow.Cells["type"].Value.ToString() == "SERVICECHARGE")
                {
                    throw new ValidationException(MessageUtils.getMessage(1826));// "Service Charge is already added"
                }
            }
            log.LogMethodExit();
        }

        private void dgvBookingProducts_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (dgvBookingProducts.CurrentCell != null && dgvBookingProducts.Rows[e.RowIndex] != null && dgvBookingProducts.Rows[e.RowIndex].Cells[e.ColumnIndex] != null)
            {
                if (dgvBookingProducts.Rows[e.RowIndex].Cells["type"].Value != null &&
                    dgvBookingProducts.Rows[e.RowIndex].Cells["type"].Value.ToString() == "SERVICECHARGE")
                {
                    if (dgvBookingProducts.Columns[e.ColumnIndex].Name != "dcPrice" && dgvBookingProducts.Columns[e.ColumnIndex].Name != "dcRemarks")
                    {
                        dgvBookingProducts.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly = true;
                    }
                }
                else if (dgvBookingProducts.CurrentCell != null && dgvBookingProducts.Rows[e.RowIndex] != null && dgvBookingProducts.Columns[e.ColumnIndex].Name == "dcQuantity")
                {
                    bool canEdit = CanEditAddProductRowQuantity(dgvBookingProducts.Rows[e.RowIndex]);

                    if (canEdit == false)
                    {
                        dgvBookingProducts.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly = true;
                        if (tcBooking.SelectedTab == tpAdditional)
                        {
                            txtMessage.Text = Utilities.MessageUtils.getMessage(1827);// "Quantity edit is not allowed for selected product"
                        }
                    }
                    else
                    {
                        dgvBookingProducts.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly = false;
                        txtMessage.Clear();
                    }
                }
                else if (dgvBookingProducts.CurrentCell != null && dgvBookingProducts.Rows[e.RowIndex] != null && dgvBookingProducts.Columns[e.ColumnIndex].Name == "type")
                {
                    dgvBookingProducts.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly = true;
                }
            }
            log.LogMethodExit();
        }

        private void GeneratePackageAttractionProductlist()
        {
            log.LogMethodEntry();
            bool loadPackageAttractions = (lstAttractionProductslist.Count == 0);
            if (BookingId != -1 && loadPackageAttractions == true)
            {
                //lstAttractionProductslist = Reservation.GenerateAttractionProductlist(true, BookingId);
            }
            log.LogMethodExit();
        }

        private void GenerateAdditionalAttractionProductlist()
        {
            log.LogMethodEntry();
            bool loadAdditionalAttractions = (lstAdditionalAttractionProductsList.Count == 0);
            if (BookingId != -1 && loadAdditionalAttractions == true)
            {
                //lstAdditionalAttractionProductsList = Reservation.GenerateAttractionProductlist(false, BookingId);
            }
            log.LogMethodExit();
        }

        private void GeneratePackageProductModifiers()
        {
            log.LogMethodEntry();
            bool loadModifierProducts = (lstPurchasedModifierProducts.Count == 0);
            if (BookingId != -1 && loadModifierProducts)
            {
                //lstPurchasedModifierProducts = Reservation.GeneratePurchasedProductlist(BookingId, true);
            }
            log.LogMethodExit();
        }

        private void GenerateAdditionalProductModifiers()
        {
            log.LogMethodEntry();
            bool loadModifierProducts = (lstPurchasedAdditionalModifierProducts.Count == 0);
            if (BookingId != -1 && loadModifierProducts)
            {
               // lstPurchasedAdditionalModifierProducts = Reservation.GeneratePurchasedProductlist(BookingId, false);
            }
            log.LogMethodExit();
        }
        private void dgvPackageList_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);

            if (dgvPackageList.CurrentCell != null && dgvPackageList.Rows[e.RowIndex] != null && dgvPackageList.Columns[e.ColumnIndex].Name == "SelectProduct")
            {
                dgvPackageList.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly = true;
            }
            else if (dgvPackageList.CurrentCell != null && dgvPackageList.Rows[e.RowIndex] != null && dgvPackageList.Columns[e.ColumnIndex].Name == "Quantity")
            {
                bool canEdit = CanEditPackageRowQuantity(dgvPackageList.Rows[e.RowIndex]);

                if (canEdit == false)
                {
                    dgvPackageList.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly = true;
                    if (tcBooking.SelectedTab == tpPackage)
                    {
                        txtMessage.Text = Utilities.MessageUtils.getMessage(1827);// "Quantity edit is not allowed for selected product"
                    }
                }
                else
                {
                    dgvPackageList.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly = false;
                    txtMessage.Clear();
                }
            }
            log.LogMethodExit();
        }

        private bool CanEditPackageRowQuantity(DataGridViewRow dataGridViewRow)
        {
            log.LogMethodEntry(dataGridViewRow);
            bool canEdit = true;
            if (dataGridViewRow.Cells["SelectProduct"].Value != null && dataGridViewRow.Cells["SelectProduct"].Value.ToString() == "Y")
            {
                if (dataGridViewRow.Cells["ProductType"].Value.ToString() == "ATTRACTION" || dataGridViewRow.Cells["ProductType"].Value.ToString() == "COMBO")
                {
                    canEdit = false;
                }
                else if (dataGridViewRow.Cells["ProductType"].Value.ToString() == "MANUAL")
                {
                    int productId = Convert.ToInt32(dataGridViewRow.Cells["ChildId"].Value);
                    bool presentInModifierList = PresentInModifierProductList(lstPurchasedModifierProducts, productId);
                    if (presentInModifierList)
                    {
                        canEdit = false;
                    }
                }
            }
            log.LogMethodExit(canEdit);
            return canEdit;
        }

        private bool CanEditAddProductRowQuantity(DataGridViewRow dataGridViewRow)
        {
            log.LogMethodEntry(dataGridViewRow);
            bool canEdit = true;
            if (dataGridViewRow.Cells["dcProduct"].Value != null)
            {
                if (dataGridViewRow.Cells["type"].Value.ToString() == "ATTRACTION" || dataGridViewRow.Cells["type"].Value.ToString() == "COMBO")
                {
                    canEdit = false;
                }
                else if (dataGridViewRow.Cells["type"].Value.ToString() == "MANUAL")
                {
                    int productId = Convert.ToInt32(dataGridViewRow.Cells["dcProduct"].Value);
                    int quantity = Convert.ToInt32(dataGridViewRow.Cells["dcQuantity"].Value);
                    int lineId = Convert.ToInt32(dataGridViewRow.Cells["LineId"].Value);
                    bool presentInModifierList = PresentInAdditionalProductModifierList(productId, quantity, lineId);
                    if (presentInModifierList)
                    {
                        canEdit = false;
                    }
                }
            }
            log.LogMethodExit(canEdit);
            return canEdit;
        }

        private void frmMakeReservation_Deactivate(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Parafait_POS.POSUtils.AttachFormEvents();
            log.LogMethodExit();
        }

        private void btnIncreaseGuestMouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Button selectedButton = (Button)sender;
            selectedButton.BackgroundImage = Properties.Resources.Plus_Btn_Hover;
            log.LogMethodExit();

        }

        private void btnIncreaseGuestMouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Button selectedButton = (Button)sender;
            selectedButton.BackgroundImage = Properties.Resources.Plus_Btn;
            log.LogMethodExit();
        }

        private void btnReduceGuestMouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Button selectedButton = (Button)sender;
            selectedButton.BackgroundImage = Properties.Resources.Minus_Btn_Hover;
            log.LogMethodExit();
        }

        private void btnReduceGuestMouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Button selectedButton = (Button)sender;
            selectedButton.BackgroundImage = Properties.Resources.Minus_Btn;
            log.LogMethodExit();
        }
    }
}
