/********************************************************************************************
 * Project Name - Parafait_Kiosk
 * Description  - Select Slot Screen for Attraction Booking
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.155.0.0   06-Jun-2023      Sathyavathi        Created for Attraction Sale in Kiosk
 *2.150.7     10-Nov-2023      Sathyavathi        Customer Lookup Enhancement
 *2.152.0.0   12-Dec-2023      Suraj Pai          Modified for Attraction Sale in TableTop Kiosk
 ********************************************************************************************/
using System;
using System.Data;
using System.Windows.Forms;
using Semnox.Parafait.KioskCore;
using System.Collections.Generic;
using Semnox.Parafait.Product;
using Semnox.Core.Utilities;
using System.Linq;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Transaction;
using System.Drawing;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.Transaction.TransactionFunctions;
using Semnox.Core.GenericUtilities;

namespace Parafait_Kiosk
{
    public partial class frmSelectSlot : BaseFormKiosk
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext = KioskStatic.Utilities.ExecutionContext;
        private ProductsContainerDTO productsContainerDTO;
        private DateTime defaultDate;
        private DateTime selectedDate;
        private const string ERROR = "ERROR";
        private const string REFRESH = "REFRESH";
        private int productId;
        private int comboProductId;
        private bool allowChangeScheduleDate;
        private ScheduleDetailsDTO selectedScheduleDetailsDTO;
        private AttractionBookingDTO selectedAttractionBookingdDTO;
        private int quantity;
        private int businessStartHour;
        private int businessEndHour;
        private AttractionBookingSchedulesBL attractionBookingScheduleBL;
        private List<AttractionBookingDTO> selectedAttractionBookings;
        private List<DayAttractionScheduleDTO> dayAttractionSchedulesList;
        private List<MasterScheduleBL> masterScheduleBLList;
        private DateTime maxAllowedDateForBooking;
        private int allowAttractionBookingUptoXDays;
        private string KIOSK_UI_DATEFORMAT;

        public KioskTransaction GetKioskTransaction { get { return kioskTransaction; } }

        public KioskAttractionDTO KioskAttractionDTO { get { return kioskAttractionDTO; } }

        public frmSelectSlot(KioskTransaction kioskTransaction, int productId, int comboProductId, KioskAttractionDTO kioskAttrcationDTO, bool allowDateSelection, DateTime defaultDate)
        {
            log.LogMethodEntry("kioskTransaction", productId, kioskAttrcationDTO, allowDateSelection, defaultDate);
            KioskStatic.logToFile("In frmSelectSlot()");
            this.kioskTransaction = kioskTransaction;
            this.productId = productId;
            this.defaultDate = defaultDate;
            this.comboProductId = comboProductId;
            this.kioskAttractionDTO = kioskAttrcationDTO;
            this.allowChangeScheduleDate = allowDateSelection;
            InitializeComponent();
            KioskStatic.setDefaultFont(this);
            KioskStatic.formatMessageLine(txtMessage, 26, ThemeManager.CurrentThemeImages.BottomMessageLineImage);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            DisplaybtnCancel(true);
            DisplaybtnPrev(true);
            SetKioskTimerTickValue(30);
            try
            {
                this.productsContainerDTO = ProductsContainerList.GetProductsContainerDTO(executionContext.SiteId, productId);
                this.masterScheduleBLList = KioskStatic.MasterScheduleBLList;

                lblSelectSlot.Visible = KioskStatic.CurrentTheme.ShowHeaderMessage;

                KIOSK_UI_DATEFORMAT = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "DATE_FORMAT");
                allowAttractionBookingUptoXDays = ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "ALLOW_ATTRACTION_BOOKING_UPTO_X_DAYS", 30);
                //allowAttractionBookingUptoXDays = 0 means allowed only for the current date, allowAttractionBookingUptoXDays = 1 means allowed till tomorrow i.e +1day.
                maxAllowedDateForBooking = ServerDateTime.Now.AddDays(allowAttractionBookingUptoXDays);
                btnCalendar.Enabled = (allowAttractionBookingUptoXDays > 4) ? true : false;
                ShowOrHideNoSchedulesAvailableMsg(false);
                SetCustomImages();
                SetOnScreenMessages();
                selectedAttractionBookings = new List<AttractionBookingDTO>();

                if (kioskAttrcationDTO.ChildAttractionBookingDTOList != null &&
                    kioskAttrcationDTO.ChildAttractionBookingDTOList.Any())
                {
                    KioskAttractionChildDTO kioskAttractionChildDTO = kioskAttrcationDTO.ChildAttractionBookingDTOList.Where(p => p.ChildProductId == this.productId && p.ComboProductId == comboProductId).FirstOrDefault();
                    this.quantity = kioskAttractionChildDTO.ChildProductQuantity * kioskAttrcationDTO.Quantity;
                    this.selectedAttractionBookingdDTO = kioskAttractionChildDTO.ChildAttractionBookingDTO;

                    if (kioskAttractionDTO.ChildAttractionBookingDTOList != null && kioskAttractionDTO.ChildAttractionBookingDTOList.Any())
                    {
                        foreach (KioskAttractionChildDTO kioskAttractionChild in kioskAttractionDTO.ChildAttractionBookingDTOList)
                        {
                            if (kioskAttractionChild.ChildProductType == ProductTypeValues.ATTRACTION)
                            {
                                selectedAttractionBookings.Add(selectedAttractionBookingdDTO);
                            }
                        }
                    }
                }
                else  //single attraction
                {
                    this.selectedAttractionBookingdDTO = kioskAttrcationDTO.AttractionBookingDTO;
                    this.quantity = kioskAttrcationDTO.Quantity;
                    selectedAttractionBookings.Add(kioskAttractionDTO.AttractionBookingDTO);
                }
                selectedDate = (selectedAttractionBookingdDTO.ScheduleFromDate != DateTime.MinValue) ?
                   KioskHelper.GetSlotDate(executionContext, selectedAttractionBookingdDTO.ScheduleFromDate) : ServerDateTime.Now;
                attractionBookingScheduleBL = new AttractionBookingSchedulesBL(executionContext, selectedDate, this.masterScheduleBLList);
                if ((selectedDate - defaultDate).Days > 5)
                {
                    RefreshScheduleDates(selectedDate);
                }
                else
                {
                    RefreshScheduleDates(defaultDate);
                }
                businessStartHour = ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "BUSINESS_DAY_START_TIME", 6);
                businessEndHour = ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "BUSINESS_DAY_END_TIME", 24);
                flpDates.Enabled = allowChangeScheduleDate;
                DateTime selDate = (selectedAttractionBookingdDTO.ScheduleFromDate != DateTime.MinValue) ? KioskHelper.GetSlotDate(executionContext, selectedAttractionBookingdDTO.ScheduleFromDate) : defaultDate;
                RefreshBackgroundImageForDate(selDate);
                RefreshSlots(selDate);
                RefreshYourSelections();
                txtMessage.Text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Select a slot"); //Select a slot
                SetCustomizedFontColors();
                KioskStatic.Utilities.setLanguage(this);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Unexpected error in frmSelectSlot()" + ex.Message);
            }
            log.LogMethodExit();
        }

        private void frmSelectSlot_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            log.LogMethodExit();
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED
                return cp;
            }
        }

        private void RefreshScheduleDates(DateTime defaultBookingDate)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            try
            {
                DateTime bookingDate = defaultBookingDate;
                pnlDateSection.SuspendLayout();
                foreach (Control c in flpDates.Controls)
                {
                    string type = c.GetType().ToString().ToLower();
                    if (type.Contains("button") && !(c.Name.Equals("btnCalendar")))
                    {
                        c.Tag = bookingDate;
                        string dateString = bookingDate.ToString("MMM dd");
                        c.Text = dateString.Replace(' ', '\n');
                        c.Enabled = bookingDate.Date > (Convert.ToDateTime(maxAllowedDateForBooking).Date) ? false : true;
                        bookingDate = bookingDate.AddDays(1);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in RefreshScheduleDates(): " + ex.Message);
            }
            finally
            {
                pnlDateSection.ResumeLayout(true);
            }
            log.LogMethodExit();
        }

        private void RefreshYourSelections()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            try
            {
                DateTime scheduleFromDate = (selectedScheduleDetailsDTO != null) ? selectedScheduleDetailsDTO.ScheduleFromDate : selectedAttractionBookingdDTO.ScheduleFromDate;
                decimal scheduleFromTime = (selectedScheduleDetailsDTO != null) ? selectedScheduleDetailsDTO.ScheduleFromTime : selectedAttractionBookingdDTO.ScheduleFromTime;

                if (scheduleFromDate != DateTime.MinValue && scheduleFromTime > -1)
                {
                    int hours = Decimal.ToInt32(scheduleFromTime);
                    int minutes = (int)((scheduleFromTime - hours) * 100);
                    DateTime fromTime = scheduleFromDate.Date.AddMinutes(hours * 60 + minutes);
                    lblYourSelectionsTime.Text = fromTime.ToString("hh:mm tt").TrimStart('0');

                    string dateSelected = (scheduleFromDate == DateTime.MinValue) ?
                        defaultDate.ToString("MMM dd") : scheduleFromDate.ToString("MMM dd");
                    lblYourSelectionsDate.Text = dateSelected;
                }
                else
                {
                    lblYourSelectionsTime.Text = "--";
                    lblYourSelectionsDate.Text = "--";
                }

                lblYourSelectionsQty.Text = this.quantity.ToString();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in RefreshYourSelections(): " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void RefreshSlots(DateTime scheduleFromDate)
        {
            log.LogMethodEntry(scheduleFromDate);
            ResetKioskTimer();
            try
            {
                txtMessage.Text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 1008); //Processing..Please wait...
                List<ScheduleDetailsViewDTO> schedules;
                int facilityMapId = -1;
                schedules = GetEligibleSchedules(scheduleFromDate, businessStartHour, businessEndHour, facilityMapId, productsContainerDTO.ProductId);
                if (schedules == null || schedules.Any() == false)
                {
                    string message = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 5158); //"No schedules available for the day. Please select another date";
                    ShowOrHideNoSchedulesAvailableMsg(true);
                    return;
                }
                try
                {
                    ShowOrHideNoSchedulesAvailableMsg(false);
                    flpSlots.SuspendLayout();
                    flpSlots.Controls.Clear();
                    foreach (ScheduleDetailsViewDTO scheduleDetailsViewDTO in schedules)
                    {
                        if (scheduleDetailsViewDTO.PastSchedule)
                        {
                            int gracePeriod = ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "ATTRACTION_BOOKING_GRACE_PERIOD", 0);

                            if (scheduleDetailsViewDTO.ScheduleDetailsDTO.ScheduleFromDate < ServerDateTime.Now.AddMinutes(gracePeriod))
                            {
                                continue;
                            }
                        }

                        ResetKioskTimer();
                        int totalUnits = 0;
                        totalUnits = scheduleDetailsViewDTO.ScheduleDetailsDTO.TotalUnits == null ? 0 : Convert.ToInt32(scheduleDetailsViewDTO.ScheduleDetailsDTO.TotalUnits);
                        // to avoid double counting in a reschedule scenario
                        List<AttractionBooking> existingAttractionBookings = kioskAttractionDTO.GetExistingAttractionBookings;
                        if (existingAttractionBookings != null && existingAttractionBookings.Any())
                        {
                            List<AttractionBooking> existingATB = existingAttractionBookings.Where(x => (x.AttractionBookingDTO != null)
                            && (x.AttractionBookingDTO.AttractionScheduleId == scheduleDetailsViewDTO.ScheduleDetailsDTO.ScheduleId)
                            && (x.AttractionBookingDTO.FacilityMapId == scheduleDetailsViewDTO.ScheduleDetailsDTO.FacilityMapId)
                            && (x.AttractionBookingDTO.ScheduleFromDate.Date == scheduleDetailsViewDTO.ScheduleDetailsDTO.ScheduleFromDate.Date)
                            && (x.AttractionBookingDTO.ScheduleFromTime == scheduleDetailsViewDTO.ScheduleDetailsDTO.ScheduleFromTime)
                                                                                                                ).ToList();
                            if (existingATB != null && existingATB.Any())
                            {
                                scheduleDetailsViewDTO.ScheduleDetailsDTO.BookedUnits = scheduleDetailsViewDTO.ScheduleDetailsDTO.BookedUnits;
                            }
                        }
                        //pastschedule is true for blocked schedule or reserved schedule.
                        scheduleDetailsViewDTO.ScheduleDetailsDTO.AvailableUnits = totalUnits < 0 ? 0 : totalUnits - scheduleDetailsViewDTO.ScheduleDetailsDTO.BookedUnits;
                        bool blockedSchedule = (scheduleDetailsViewDTO != null ? scheduleDetailsViewDTO.PastSchedule : false);
                        if (blockedSchedule == false
                            && (scheduleDetailsViewDTO.ScheduleDetailsDTO.AvailableUnits >= this.quantity
                            || (selectedScheduleDetailsDTO != null
                               && selectedScheduleDetailsDTO.ScheduleId == scheduleDetailsViewDTO.ScheduleDetailsDTO.ScheduleId
                               && selectedScheduleDetailsDTO.ScheduleFromDate == scheduleDetailsViewDTO.ScheduleDetailsDTO.ScheduleFromDate)
                            || (selectedAttractionBookingdDTO != null
                               && selectedAttractionBookingdDTO.DayAttractionScheduleDTO != null
                               && selectedAttractionBookingdDTO.DayAttractionScheduleDTO.AttractionScheduleId == scheduleDetailsViewDTO.ScheduleDetailsDTO.ScheduleId
                               && selectedAttractionBookingdDTO.DayAttractionScheduleDTO.ScheduleDateTime == scheduleDetailsViewDTO.ScheduleDetailsDTO.ScheduleFromDate
                               ))
                               )
                        //do not allow if it can't fully accomodate the required quantity.
                        {
                            usrCtrlSlot usrCtrl = new usrCtrlSlot(executionContext, scheduleDetailsViewDTO.ScheduleDetailsDTO, selectedAttractionBookingdDTO);
                            usrCtrl.selectedSlotMethod += new usrCtrlSlot.SelctedSlotDelegate(SelectedSlotDelegate);
                            usrCtrl.unSelctSlotMethod += new usrCtrlSlot.UnSelctSlotDelegate(UnSelectSlotDelegate);
                            flpSlots.Controls.Add(usrCtrl);
                        }
                        else
                        {
                            //5528 - 'Unable to fully accommodate the quantity for this attraction slot. There are less units available than the quantity selected.'
                            //5529 - 'Sorry, this schedule is blocked. Please pick a different time slot.
                            string msg = (blockedSchedule == false ?
                                             MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 5528)
                                            : MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 5529));
                            KioskStatic.logToFile(msg);
                            log.Error(msg);
                            log.LogVariableState("AvailableUnits: ", scheduleDetailsViewDTO.ScheduleDetailsDTO.AvailableUnits);
                            log.LogVariableState("Quantity Selected: ", quantity);
                        }
                    }
                    if ((schedules != null && schedules.Any()) && (flpSlots.Controls.Count == 0 && schedules[0].ScheduleDetailsDTO.AvailableUnits < quantity)) //case when schedules are available but can't accomodate the quantity selected together.
                    {
                        //5528 - 'Unable to fully accommodate the quantity for this attraction slot. There are less units available than the quantity selected.'
                        string msg = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 5528);
                        msg += Environment.NewLine;
                        msg += MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Available Seats: " + schedules[0].ScheduleDetailsDTO.AvailableUnits); //"No schedules available for the day. Please select another date";
                        lblNoSchedules.Text = msg;
                        ShowOrHideNoSchedulesAvailableMsg(true);
                        return;
                    }
                }
                finally
                {
                    flpSlots.ResumeLayout(true);
                    txtMessage.Text = string.Empty;
                    ResetKioskTimer();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in RefreshYourSelections(): " + ex.Message);
            }
            finally
            {
                DisplayStatusMessage(string.Empty);
                ResetKioskTimer();
            }
            log.LogMethodExit();
        }

        private int GetTotalBookedQuantityForCurrentBooking(ScheduleDetailsDTO scheduleDetailsDTO, int bookingId = -1, bool checkForProduct = false)
        {
            log.LogMethodEntry(scheduleDetailsDTO, bookingId, checkForProduct);
            int qty = -1;
            if (selectedAttractionBookings != null && selectedAttractionBookings.Any())
            {
                // Get total booked units from temp list. DB list go seperately
                List<AttractionBookingDTO> existingBooking = selectedAttractionBookings.Where(x => (x.AttractionScheduleId == scheduleDetailsDTO.ScheduleId)
                                                                                                && (x.FacilityMapId == scheduleDetailsDTO.FacilityMapId)
                                                                                                && (x.ScheduleFromDate.Date == scheduleDetailsDTO.ScheduleFromDate.Date)
                                                                                                && (x.ScheduleFromTime == scheduleDetailsDTO.ScheduleFromTime)
                                                                                                    ).ToList();
                if (existingBooking != null && existingBooking.Any())
                {
                    if (checkForProduct)
                    {
                        existingBooking = existingBooking.Where(x => (x.AttractionProductId == scheduleDetailsDTO.ProductId)).ToList();
                    }

                    if (bookingId != -1)
                    {
                        existingBooking = existingBooking.Where(x => (x.BookingId == bookingId)).ToList();
                    }
                    qty = existingBooking.Sum(x => x.BookedUnits);
                }
            }
            log.LogMethodExit(qty);
            return qty;
        }

        public override void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                DisableButtons();
                KioskStatic.logToFile("Cancel Button Pressed : Triggering Home Button Action ");
                base.btnHome_Click(sender, e);
            }
            catch (Exception ex)
            {
                log.Error("Error in btnCancel_Click", ex);
            }
            finally
            {
                EnableButtons();
            }
            log.LogMethodExit();
        }

        private void BlockSchedule()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            StopKioskTimer();
            try
            {
                if (selectedAttractionBookingdDTO != null)
                {
                    AttractionBooking atb = new AttractionBooking(executionContext, selectedAttractionBookingdDTO);
                    atb.Save(-1);
                    selectedAttractionBookingdDTO = atb.AttractionBookingDTO;
                    selectedAttractionBookingdDTO.Identifier = comboProductId;
                    SetSelectedScheduleDetails(selectedAttractionBookingdDTO);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            finally
            {
                StartKioskTimer();
            }
            log.LogMethodExit();
        }

        private void btnProceed_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ResetKioskTimer();
            StopKioskTimer();

            DisableButtons();
            try
            {
                this.txtMessage.Text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 1008); //Processing..Please wait...
                Application.DoEvents();
                if (selectedScheduleDetailsDTO == null && (selectedAttractionBookingdDTO == null || selectedAttractionBookingdDTO.AttractionScheduleId == -1))
                {
                    string warningMsg = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Select a slot");
                    frmOKMsg.ShowUserMessage(warningMsg);
                    DisplayStatusMessage(warningMsg); // "Select a slot"
                    log.LogMethodExit();
                    return;
                }

                if (selectedScheduleDetailsDTO != null)
                {
                    selectedAttractionBookingdDTO = CreateNewBookingAndReleaseOldSlot(selectedScheduleDetailsDTO, quantity, this.comboProductId);
                }
                if (kioskAttractionDTO.ChildAttractionBookingDTOList != null && kioskAttractionDTO.ChildAttractionBookingDTOList.Any())
                {
                    BlockSchedule();
                    int totalCnt = kioskAttractionDTO.ChildAttractionBookingDTOList.Count;
                    for (int i = 0; i < totalCnt; i++)
                    {
                        if (kioskAttractionDTO.ChildAttractionBookingDTOList[i].ChildProductId == this.productId
                            && kioskAttractionDTO.ChildAttractionBookingDTOList[i].ComboProductId == this.comboProductId)
                        {
                            if (i == totalCnt - 1)
                            {
                                using (frmAttractionSummary frm = new frmAttractionSummary(kioskTransaction, kioskAttractionDTO))
                                {
                                    DialogResult drSummary = frm.ShowDialog();
                                    kioskTransaction = frm.GetKioskTransaction;
                                    if (drSummary == DialogResult.No)
                                    {
                                        RefreshUI();
                                        break;
                                    }
                                    else
                                    {
                                        this.DialogResult = drSummary;
                                        this.Close();
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                int k = i + 1;
                                for (int j = k; j < totalCnt; j++)
                                {
                                    if (j == totalCnt - 1 && kioskAttractionDTO.ChildAttractionBookingDTOList[j].ChildProductType != ProductTypeValues.ATTRACTION)
                                    {
                                        using (frmAttractionSummary frm = new frmAttractionSummary(kioskTransaction, kioskAttractionDTO))
                                        {
                                            DialogResult drSummary = frm.ShowDialog();
                                            kioskTransaction = frm.GetKioskTransaction;
                                            if (drSummary == DialogResult.No)
                                            {
                                                RefreshUI();
                                                break;
                                            }
                                            else
                                            {
                                                this.DialogResult = drSummary;
                                                this.Close();
                                                break;
                                            }
                                        }
                                    }
                                    else
                                    {

                                        if (kioskAttractionDTO.ChildAttractionBookingDTOList[j].ChildProductType == ProductTypeValues.ATTRACTION)
                                        {
                                            int productId = kioskAttractionDTO.ChildAttractionBookingDTOList[j].ChildProductId;
                                            int comboProductId = kioskAttractionDTO.ChildAttractionBookingDTOList[j].ComboProductId;
                                            int itemCount = (kioskAttractionDTO.GetAttractionChildIndex(comboProductId) - 1);
                                            using (frmProcessingAttractions frm = new frmProcessingAttractions(kioskTransaction, kioskAttractionDTO, itemCount))
                                            {
                                                DialogResult dr = frm.ShowDialog();
                                                kioskTransaction = frm.GetKioskTransaction;
                                                if (dr == System.Windows.Forms.DialogResult.No) //back button pressed
                                                {
                                                    RefreshUI();
                                                    break;
                                                }
                                                else
                                                {
                                                    this.DialogResult = dr;
                                                    this.Close();
                                                    break;
                                                }

                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        }
                    }
                }
                else
                {
                    AddSingleAttractionToTransaction();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("btnProceed_Click() in CheckInCheckOutQtyScreen : " + ex.Message);
                frmOKMsg.ShowUserMessage("Sorry, Failed to save the booking information" + ex.Message); //sathya, add msg number
            }
            finally
            {
                EnableButtons();
                StartKioskTimer();
            }
            log.LogMethodExit();
        }

        private void AddSingleAttractionToTransaction()
        {
            log.LogMethodEntry();
            StopKioskTimer();
            try
            {
                bool isRegisteredCustomerOnly = productsContainerDTO.RegisteredCustomerOnly.Equals("Y") ? true : false;
                bool cardSale = KioskHelper.AttractionIsOfTypeCardSale(executionContext);
                if (cardSale)
                {
                    using (frmCardSaleOption ftc = new frmCardSaleOption())
                    {
                        DialogResult drt = ftc.ShowDialog();
                        if (drt == DialogResult.Cancel)
                        {
                            this.txtMessage.Text = string.Empty;
                            log.LogMethodExit();
                            return;
                        }

                        if (ftc.SelectedOption == frmCardSaleOption.CardSaleOption.EXISTING)
                        {
                            bool loadToSigleCard = productsContainerDTO.LoadToSingleCard;
                            int quantityLocal = kioskAttractionDTO.Quantity;
                            List<Card> cardList = new List<Card>();
                            bool isSkipLinkingCustomer = false;
                            int i = 1;
                            while (quantityLocal > 0)
                            {
                                //string msg = "Tap your card for quantity " + i + " of " + kioskAttractionDTO.Quantity;//sathya, add msg num
                                bool enableNote = false;
                                string msg;
                                msg = MessageContainerList.GetMessage(executionContext, 458); //Please Tap Your Card
                                if (kioskAttractionDTO.Quantity > 1 && loadToSigleCard == false)
                                {
                                    //override the message
                                    msg = MessageContainerList.GetMessage(executionContext, 4118, i, kioskAttractionDTO.Quantity); //"Tap your card for quantity 1/3"
                                    enableNote = true;
                                }
                                using (frmAttractionTapCard fac = new frmAttractionTapCard(kioskTransaction, msg, enableNote))
                                {
                                    fac.ShowDialog();
                                    kioskTransaction = fac.GetKioskTransaction;
                                    if (fac.Card == null)
                                    {
                                        cardList = null;
                                        log.LogMethodExit();
                                        return;
                                    }
                                    else
                                    {
                                        if (isSkipLinkingCustomer == false)
                                        {
                                            bool isCustomerMandatory = (kioskTransaction.HasCustomerRecord() == false && isRegisteredCustomerOnly == true) ? true : false;
                                            bool isLinked = false;
                                            try
                                            {
                                                isLinked = CustomerStatic.LinkCustomerToTheCard(kioskTransaction, isCustomerMandatory, fac.Card);
                                                if (isLinked == false)
                                                {
                                                    isSkipLinkingCustomer = true;
                                                }
                                                if (!CustomerStatic.AlertUserForCustomerRegistrationRequirement(kioskTransaction.HasCustomerRecord(), isCustomerMandatory, isLinked))
                                                {
                                                    log.LogMethodExit();
                                                    return;
                                                }
                                            }
                                            catch (CustomerStatic.TimeoutOccurred ex)
                                            {
                                                KioskStatic.logToFile("Timeout occured");
                                                log.Error(ex);
                                                PerformTimeoutAbortAction(kioskTransaction, kioskAttractionDTO);
                                                this.DialogResult = DialogResult.Cancel;
                                                log.LogMethodExit();
                                                return;
                                            }
                                        }
                                        if (i == 1)
                                        {
                                            kioskTransaction.SetPrimaryCard(fac.Card);
                                        }
                                        if (loadToSigleCard)
                                        {
                                            while (quantityLocal > 0)
                                            {
                                                cardList.Add(fac.Card);
                                                quantityLocal--;
                                            }
                                            break;
                                        }
                                        else
                                        {
                                            cardList.Add(fac.Card);
                                        }
                                    }
                                }
                                i++;
                                quantityLocal--;
                            }
                            if (cardList != null && cardList.Any())
                            {
                                AssignCardsToProducts(cardList);
                            }
                        }
                        else
                        {
                            bool showMsgRecommendCustomerToRegister = true;
                            bool isLinked = false;
                            try
                            {
                                isLinked = CustomerStatic.LinkCustomerToTheTransaction(kioskTransaction, executionContext, isRegisteredCustomerOnly, showMsgRecommendCustomerToRegister);
                            }
                            catch (CustomerStatic.TimeoutOccurred ex)
                            {
                                KioskStatic.logToFile("Timeout occured");
                                log.Error(ex);
                                PerformTimeoutAbortAction(kioskTransaction, kioskAttractionDTO);
                                this.DialogResult = DialogResult.Cancel;
                                log.LogMethodExit();
                                return;
                            }
                            if (!CustomerStatic.AlertUserForCustomerRegistrationRequirement(kioskTransaction.HasCustomerRecord(),
                                    isRegisteredCustomerOnly, isLinked))
                            {
                                log.LogMethodExit();
                                return;
                            }
                            kioskAttractionDTO = kioskTransaction.GenerateAttractionCards(kioskAttractionDTO);
                        }
                    }
                }
                else
                {
                    bool showMsgRecommendCustomerToRegister = true;
                    bool isLinked = false;
                    try
                    {
                        isLinked = CustomerStatic.LinkCustomerToTheTransaction(kioskTransaction, executionContext, isRegisteredCustomerOnly, showMsgRecommendCustomerToRegister);
                    }
                    catch (CustomerStatic.TimeoutOccurred ex)
                    {
                        KioskStatic.logToFile("Timeout occured");
                        log.Error(ex);
                        PerformTimeoutAbortAction(kioskTransaction, kioskAttractionDTO);
                        this.DialogResult = DialogResult.Cancel;
                        log.LogMethodExit();
                        return;
                    }
                    if (!CustomerStatic.AlertUserForCustomerRegistrationRequirement(kioskTransaction.HasCustomerRecord(),
                            isRegisteredCustomerOnly, isLinked))
                    {
                        log.LogMethodExit();
                        return;
                    }
                }
                BlockSchedule();
                List<Semnox.Parafait.Transaction.Transaction.TransactionLine> addedLines = kioskTransaction.AddAttractiontProduct(kioskAttractionDTO);

                if (kioskTransaction.ShowCartInKiosk == false)
                {
                    if (kioskTransaction != null)
                    {
                        ProceedActionImpl(kioskTransaction);
                    }
                }
                else
                {
                    frmChooseProduct.AlertUser(kioskAttractionDTO.ProductId, kioskTransaction, ProceedActionImpl);
                    DialogResult = DialogResult.OK;
                    string msg = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 4842); //adding to cart
                    KioskStatic.logToFile("frmAttractionSummary: " + msg);
                    txtMessage.Text = msg;
                    GoBackToProductSelectionScreen();
                }

            }
            catch (Exception ex)
            {
                string msg = "Unexpected error occurred while executing btnProceed_Click() in attraction summary : ";
                log.Error(msg + ex);
                KioskStatic.logToFile(msg + ex.Message);
                frmOKMsg.ShowUserMessage(ex.Message);
            }
            finally
            {
                StartKioskTimer();
            }
            log.LogMethodExit();
        }

        private void ProceedActionImpl(KioskTransaction kioskTransaction)
        {
            log.LogMethodEntry("kioskTransaction");
            try
            {
                using (frmPaymentMode frpm = new frmPaymentMode(kioskTransaction))
                {
                    DialogResult dr = frpm.ShowDialog();
                    kioskTransaction = frpm.GetKioskTransaction;
                    if (dr != System.Windows.Forms.DialogResult.No) // back button pressed
                    {
                        DialogResult = dr;
                        this.Close();
                        log.LogMethodExit();
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                this.Show();
                log.Error(ex);
                frmOKMsg.ShowUserMessage(ex.Message);
                txtMessage.Text = ex.Message;
                this.Close();
            }
            log.LogMethodExit();
        }
        private void AssignCardsToProducts(List<Card> cardList)
        {
            log.LogMethodEntry(cardList);
            if (kioskAttractionDTO.ChildAttractionBookingDTOList != null && kioskAttractionDTO.ChildAttractionBookingDTOList.Any())
            {
                foreach (Card cardItem in cardList)
                {
                    foreach (KioskAttractionChildDTO childItem in kioskAttractionDTO.ChildAttractionBookingDTOList)
                    {
                        if (childItem.ChildProductType == ProductTypeValues.ATTRACTION)
                        {
                            int childqty = childItem.ChildProductQuantity;
                            if (childItem.CardList == null)
                            {
                                childItem.CardList = new List<Card>();
                            }
                            while (childqty > 0)
                            {
                                childItem.CardList.Add(cardItem);
                                childqty--;
                            }
                        }
                    }
                }
            }
            else
            {
                kioskAttractionDTO.CardList = cardList;
            }
            log.LogMethodExit();
        }

        private void RefreshUI()
        {
            log.LogMethodEntry();
            try
            {
                StopKioskTimer();
                RefreshScheduleDates(this.defaultDate);
                DateTime date = (selectedScheduleDetailsDTO != null) ? KioskHelper.GetSlotDate(executionContext, selectedScheduleDetailsDTO.ScheduleFromDate) : selectedDate;
                RefreshBackgroundImageForDate(date);
                RefreshSlots(date);
                RefreshYourSelections();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Select slot screen - RefreshUI  : " + ex.Message);
                frmOKMsg.ShowUserMessage("Error while refreshing screen data: " + ex.Message); //sathya, add msg number
            }
            finally
            {
                ResetKioskTimer();
                StopKioskTimer();
            }
            log.LogMethodExit();
        }

        private void SetSelectedScheduleDetails(AttractionBookingDTO attractionBookingdDTO)
        {
            log.LogMethodEntry();
            if (kioskAttractionDTO.ChildAttractionBookingDTOList != null
                            && kioskAttractionDTO.ChildAttractionBookingDTOList.Any())
            {

                KioskAttractionChildDTO selectedChild = kioskAttractionDTO.ChildAttractionBookingDTOList.Where(x => x.ChildProductId == this.productId && x.ComboProductId == this.comboProductId).FirstOrDefault();
                if (selectedChild != null)
                {
                    selectedChild.ChildAttractionBookingDTO = attractionBookingdDTO;
                }
                if (allowChangeScheduleDate == true)
                {
                    foreach (KioskAttractionChildDTO item in kioskAttractionDTO.ChildAttractionBookingDTOList)
                    {
                        if (item.ChildProductType == ProductTypeValues.ATTRACTION
                            && item.ComboProductId != selectedChild.ComboProductId)
                        {
                            if (item.ChildAttractionBookingDTO != null
                                && item.ChildAttractionBookingDTO.AttractionScheduleId == -1
                                && item.ChildAttractionBookingDTO.FacilityMapId == -1)
                            {
                                item.ChildAttractionBookingDTO.ScheduleFromDate = attractionBookingdDTO.ScheduleFromDate;
                            }
                        }
                    }
                }
            }
            else
            {
                kioskAttractionDTO.AttractionBookingDTO = attractionBookingdDTO;
            }
            log.LogMethodExit();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            DisableButtons();
            try
            {
                bool isThisFirstProduct = CheckForFirstProductWithBlockedSlots();
                if (isThisFirstProduct)
                {
                    string message = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 5164);
                    // This action will clear any slots that are selected for the this product. Do you want to proceed?
                    using (frmYesNo f = new frmYesNo(message))
                    {
                        if (f.ShowDialog() != DialogResult.Yes)
                        {
                            DialogResult = DialogResult.None;
                            log.LogMethodExit();
                            return;
                        }
                    }
                }
                this.DialogResult = DialogResult.No;
                Close();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in btnBack_Click() of Select Slot Screen" + ex.Message);
            }
            finally
            {
                EnableButtons();
                ResetKioskTimer();
            }
            log.LogMethodExit();
        }

        private void bigVerticalScrollView_ButtonClick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                ResetKioskTimer();
            }
            catch (Exception ex)
            {
                log.Error("Error in bigVerticalScrollView_ButtonClick", ex);
            }
            log.LogMethodExit();
        }

        private bool CheckForFirstProductWithBlockedSlots()
        {
            log.LogMethodEntry();
            bool firstProductWithBlockedSlot = false;
            if (kioskAttractionDTO != null)
            {
                if (kioskAttractionDTO.ChildAttractionBookingDTOList != null &&
                    kioskAttractionDTO.ChildAttractionBookingDTOList.Any())
                {
                    foreach (KioskAttractionChildDTO item in kioskAttractionDTO.ChildAttractionBookingDTOList)
                    {
                        if (item.ChildProductType == ProductTypeValues.ATTRACTION)
                        {
                            if (item.ComboProductId == this.comboProductId)
                            {
                                if (item.ChildAttractionBookingDTO != null
                                    && item.ChildAttractionBookingDTO.BookingId > -1)
                                {
                                    firstProductWithBlockedSlot = true;
                                }
                                break;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
                else
                {
                    if (kioskAttractionDTO.AttractionBookingDTO != null
                        && kioskAttractionDTO.AttractionBookingDTO.BookingId > -1)
                    {
                        firstProductWithBlockedSlot = true;
                    }
                }
            }
            log.LogMethodExit(firstProductWithBlockedSlot);
            return firstProductWithBlockedSlot;

        }

        private void vScrollBarProducts_Scroll(object sender, ScrollEventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            try
            {
                if (e.NewValue > e.OldValue)
                    scrollDown(e.NewValue - e.OldValue);
                else if (e.NewValue < e.OldValue)
                    scrollUp(e.OldValue - e.NewValue);
            }
            catch { }
            log.LogMethodExit();
        }

        private void scrollDown(int value = 10)
        {
            log.LogMethodEntry(value);
            try
            {
                ResetKioskTimer();
                if (flpSlots.Top + flpSlots.Height > 3)
                {
                    flpSlots.Top = flpSlots.Top - value;
                }
            }
            catch { }
            log.LogMethodExit();
        }

        private void scrollUp(int value = 10)
        {
            log.LogMethodEntry(value);
            try
            {
                ResetKioskTimer();
                if (flpSlots.Top < 0)
                {
                    flpSlots.Top = Math.Min(0, flpSlots.Top + value);
                }
            }
            catch { }
            log.LogMethodExit();
        }

        private void SetOnScreenMessages()
        {
            log.LogMethodEntry();
            try
            {
                ProductsContainerDTO parentProdContainerDTO = ProductsContainerList.GetProductsContainerDTO(executionContext.SiteId, kioskAttractionDTO.ProductId);
                lblBookingSlot.Text = MessageContainerList.GetMessage(executionContext, "Booking tickets for"); // "Booking tickets for"
                lblNoSchedules.Text = MessageContainerList.GetMessage(executionContext, 5158); //"No schedules available for the day. Please select another date"
                if (parentProdContainerDTO.ProductType == ProductTypeValues.COMBO)
                {
                    lblBookingSlot.Text += " ";
                    lblBookingSlot.Text += MessageContainerList.GetMessage(executionContext, 4776, kioskAttractionDTO.GetAttractionChildIndex(comboProductId)
                        , kioskAttractionDTO.ChildAttractionBookingDTOList.FindAll(p => p.ChildProductType == ProductTypeValues.ATTRACTION).Count); //&1 of &2;
                }
                string productName = KioskHelper.GetProductName(productsContainerDTO.ProductId);
                lblProductName.Text = MessageContainerList.GetMessage(executionContext, productName);
                lblYourSelectionsQty.Text = this.quantity.ToString();
                lblDate.Text = MessageContainerList.GetMessage(executionContext, "Date");
                lblSelectSlot.Text = txtMessage.Text = MessageContainerList.GetMessage(executionContext, "Select a slot"); //'Select a slot'

                lblYourSelections.Text = MessageContainerList.GetMessage(executionContext, "Your Selections..."); //Your Selections...
                lblYourSelectionsQtyHeader.Text = MessageContainerList.GetMessage(executionContext, "Quantity"); //Quantity
                lblYourSelectionsDateHeader.Text = MessageContainerList.GetMessage(executionContext, "Date"); //Quantity
                lblYourSelectionsTimeHeader.Text = MessageContainerList.GetMessage(executionContext, "Time"); //Quantity

                btnProceed.Text = (parentProdContainerDTO.ProductType == ProductTypeValues.COMBO) ?
                    MessageContainerList.GetMessage(executionContext, "Proceed")
                    : MessageContainerList.GetMessage(executionContext, "Confirm Booking");
            }
            catch (Exception ex)
            {
                log.Error("Error in SetOnScreenMessages", ex);
            }
            log.LogMethodExit();
        }

        private void SelectedSlotDelegate(ScheduleDetailsDTO scheduleDetailsDTO)
        {
            log.LogMethodEntry(scheduleDetailsDTO);
            ResetKioskTimer();
            try
            {
                int atrChildIndex = kioskAttractionDTO.GetAttractionChildIndex(comboProductId);
                if (atrChildIndex == 1)
                {
                    DateTime newSlotDate = KioskHelper.GetSlotDate(executionContext, scheduleDetailsDTO.ScheduleFromDate);
                    DateTime currentSlotDate = (selectedScheduleDetailsDTO != null ? KioskHelper.GetSlotDate(executionContext, selectedScheduleDetailsDTO.ScheduleFromDate) : selectedDate);
                    if (newSlotDate.Date != currentSlotDate.Date)
                    {
                        bool slotSelectetedForRemainingATRChildEntries = kioskAttractionDTO.SecondOrOtherATRChildHasSelectedSlot();
                        if (slotSelectetedForRemainingATRChildEntries)
                        {
                            string msg = MessageContainerList.GetMessage(executionContext, 5169);
                            //"Do you want to redo the slot selection for the combo?"
                            using (frmYesNo f = new frmYesNo(msg))
                            {
                                if (f.ShowDialog() != DialogResult.Yes)
                                {
                                    //log msg hence no container call
                                    string msg1 = "Slot Selection: User does not want to redo slot selection";
                                    KioskStatic.logToFile(msg1);
                                    log.LogMethodExit(msg1);
                                    ValidationException ve = new ValidationException(REFRESH);
                                    throw ve;
                                }
                                else
                                {
                                    int productIdLocal = kioskAttractionDTO.ProductId;
                                    int quantityLocal = kioskAttractionDTO.Quantity;
                                    kioskTransaction.ClearTemporarySlots(kioskAttractionDTO);
                                    kioskAttractionDTO = new KioskAttractionDTO(productIdLocal, quantityLocal);
                                }
                            }
                        }
                    }
                }
                foreach (usrCtrlSlot usrCtrl in flpSlots.Controls)
                {
                    usrCtrl.SetBackgroungImage = ThemeManager.CurrentThemeImages.SlotBackgroundImage;
                    usrCtrl.IsSelected = false;
                }
                this.selectedScheduleDetailsDTO = scheduleDetailsDTO;
                RefreshYourSelections();
            }
            catch (ValidationException ve)
            {
                if (ve.Message.Trim() != REFRESH)
                {
                    log.Error(ve);
                    KioskStatic.logToFile("Error in SelectedSlotDeiegate() of frmSelectSlot: " + ve.Message);
                }
                else
                {
                    throw ve;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in SelectedSlotDeiegate() of frmSelectSlot: " + ex.Message);
            }
            log.LogMethodExit();
        }


        private void UnSelectSlotDelegate(ScheduleDetailsDTO scheduleDetailsDTO)
        {
            log.LogMethodEntry(scheduleDetailsDTO);
            ResetKioskTimer();
            try
            {
                foreach (usrCtrlSlot usrCtrl in flpSlots.Controls)
                {
                    usrCtrl.SetBackgroungImage = ThemeManager.CurrentThemeImages.SlotBackgroundImage;
                    usrCtrl.IsSelected = false;
                }
                this.selectedScheduleDetailsDTO = null;
                RefreshYourSelections();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in SelectedSlotDeiegate() of frmSelectSlot: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void SetCustomImages()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            try
            {
                this.BackgroundImage = ThemeManager.GetBackgroundImage(ThemeManager.CurrentThemeImages.SelectSlotBackgroundImage);
                btnHome.BackgroundImage = ThemeManager.CurrentThemeImages.HomeButton;
                btnDate1.BackgroundImage = ThemeManager.CurrentThemeImages.BigCircleSelected;
                btnDate2.BackgroundImage =
                    btnDate3.BackgroundImage = ThemeManager.CurrentThemeImages.BigCircleUnSelected;
                btnPrev.BackgroundImage = btnCancel.BackgroundImage = btnProceed.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage;
                pnlYourSelections.BackgroundImage = ThemeManager.CurrentThemeImages.PanelYourSelections;
                this.bigVerticalScrollView.InitializeScrollBar(ThemeManager.CurrentThemeImages.ScrollDownEnabled, ThemeManager.CurrentThemeImages.ScrollDownDisabled, ThemeManager.CurrentThemeImages.ScrollUpEnabled, ThemeManager.CurrentThemeImages.ScrollUpDisabled);
                pnlTimeSection.BackgroundImage = ThemeManager.CurrentThemeImages.PanelSelectTimeSection;
                pnlDateSection.BackgroundImage = ThemeManager.CurrentThemeImages.PanelDateSection;
                btnCalendar.BackgroundImage = ThemeManager.CurrentThemeImages.PickDate;
            }
            catch (Exception ex)
            {
                log.Error("Error while Setting Customized background images for frmSelectSlot", ex);
                KioskStatic.logToFile("Error while setting customized background images for frmSelectSlot: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            try
            {
                this.btnHome.ForeColor = KioskStatic.CurrentTheme.SelectSlotHomeButtonTextForeColor;
                this.lblBookingSlot.ForeColor = KioskStatic.CurrentTheme.SelectSlotLblBookingSlotTextForeColor;
                this.lblProductName.ForeColor = KioskStatic.CurrentTheme.SelectSlotLblProductNameTextForeColor;
                this.btnPrev.ForeColor = KioskStatic.CurrentTheme.SelectSlotBackButtonTextForeColor;
                this.btnCancel.ForeColor = KioskStatic.CurrentTheme.SelectSlotCancelButtonTextForeColor;
                this.btnProceed.ForeColor = KioskStatic.CurrentTheme.SelectSlotProceedButtonTextForeColor;
                this.txtMessage.ForeColor = KioskStatic.CurrentTheme.SelectSlotFooterTextForeColor;

                this.lblYourSelectionsQtyHeader.ForeColor =
                    this.lblYourSelectionsDateHeader.ForeColor =
                    this.lblYourSelectionsTimeHeader.ForeColor = KioskStatic.CurrentTheme.AttractionQtyHeadersTextForeColor;

                this.lblYourSelectionsQty.ForeColor =
                    this.lblYourSelectionsDate.ForeColor =
                    this.lblYourSelectionsTime.ForeColor = KioskStatic.CurrentTheme.AttractionQtyValuesTextForeColor;

                this.lblDate.ForeColor = KioskStatic.CurrentTheme.SelectSlotLblDateTextForeColor;
                this.lblSelectSlot.ForeColor = KioskStatic.CurrentTheme.SelectSlotLblSelectSlotTextForeColor;
                this.lblYourSelections.ForeColor = KioskStatic.CurrentTheme.AttractionQtyLblYourSelectionsTextForeColor;
                this.lblNoSchedules.ForeColor = KioskStatic.CurrentTheme.SelectSlotLblNoSchedulesTextForeColor;

                this.btnDate1.ForeColor =
                    this.btnDate2.ForeColor =
                    this.btnDate3.ForeColor = KioskStatic.CurrentTheme.SelectSlotBtnDatesTextForeColor;
            }
            catch (Exception ex)
            {
                log.Error("Error while Setting Customized Fore Colors for frmSelectSlot", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements of frmSelectSlot: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void frmSelectSlot_FormClosed(object sender, FormClosedEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                StopKioskTimer();
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing frmSelectSlot_FormClosed()", ex);
            }
            log.Info(this.Name + ": Form closed");
            log.LogMethodExit();
        }

        private void btnDate_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                ResetKioskTimer();
                if (allowChangeScheduleDate)
                {
                    DisplayStatusMessage(MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 1008)); //Processing..Please wait...
                    Application.DoEvents();
                    DisableButtons();

                    Image selImage = ThemeManager.CurrentThemeImages.BigCircleSelected;
                    Image unselImage = ThemeManager.CurrentThemeImages.BigCircleUnSelected;
                    Button b = sender as Button;
                    if (b.Tag != null)
                    {
                        selectedDate = Convert.ToDateTime(b.Tag);
                        RefreshBackgroundImageForDate(selectedDate);
                        RefreshSlots(selectedDate);
                    }
                    txtMessage.Text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Select a slot"); // "Select a slot"
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing btnDate_Click() in frmSelectSlot" + ex.Message);
            }
            finally
            {
                EnableButtons();
            }
            log.LogMethodExit();
        }

        private void RefreshBackgroundImageForDate(DateTime date)
        {
            log.LogMethodEntry(date);
            ResetKioskTimer();

            try
            {
                pnlDateSection.SuspendLayout();
                foreach (Control c in flpDates.Controls)
                {
                    string type = c.GetType().ToString().ToLower();
                    if (type.Contains("button") && !(c.Name.Equals("btnCalendar")))
                    {
                        DateTime buttonDate = (DateTime)c.Tag;
                        c.BackgroundImage = ((date.Date == buttonDate.Date)) ? ThemeManager.CurrentThemeImages.BigCircleSelected : ThemeManager.CurrentThemeImages.BigCircleUnSelected;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing SetBackgroundImageForDate() in frmSelectSlot" + ex.Message);
            }
            finally
            {
                pnlDateSection.ResumeLayout(true);
            }

            log.LogMethodExit();
        }

        private void btnCalendarIcon_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                DisableButtons();
                ResetKioskTimer();
                DisplayStatusMessage(MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 1008)); //Processing..Please wait...
                DateTime newDate = KioskHelper.LaunchCalendar(defaultDateTimeToShow: selectedDate, enableDaySelection: true, enableMonthSelection: true
                    , enableYearSelection: true, disableTill: DateTime.Now.Date.AddDays(-1), showTimePicker: false, popupAlerts: frmOKMsg.ShowUserMessage);
                if (newDate != null && newDate != DateTime.MinValue)
                {
                    if (newDate.Date != selectedDate.Date)
                    {
                        //ServerDateTime.Now.AddDays(-1) to include today as well
                        //allowAttractionBookingUptoXDays = 0 means allowed only for the current date, allowAttractionBookingUptoXDays = 1 means allowed till tomorrow.
                        if (((newDate) - (ServerDateTime.Now.AddDays(-1))).Days > allowAttractionBookingUptoXDays)
                        {
                            string validationMsg = MessageContainerList.GetMessage(executionContext, 5227, maxAllowedDateForBooking.ToString(KIOSK_UI_DATEFORMAT));
                            DisplayStatusMessage(validationMsg);
                            frmOKMsg.ShowUserMessage(validationMsg);
                            log.LogMethodExit();
                            return;
                        }
                        selectedDate = newDate;
                        this.defaultDate = newDate;
                        RefreshScheduleDates(this.defaultDate);
                        RefreshBackgroundImageForDate(selectedDate);
                        RefreshSlots(selectedDate);
                    }
                    DisplayStatusMessage(MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Select a slot")); // "Select a slot"
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing btnCalenderIcon_Click() in frmSelectSlot()", ex);
            }
            finally
            {
                EnableButtons();
            }
            log.LogMethodExit();
        }

        private List<ScheduleDetailsViewDTO> GetEligibleSchedules(DateTime scheduleDate, decimal fromTime, decimal toTime, int facilityMapId, int productId = -1, int masterScheduleId = -1)
        {
            log.LogMethodEntry(scheduleDate, fromTime, toTime, facilityMapId, productId, masterScheduleId);
            List<ScheduleDetailsViewDTO> scheduleDetailsViewDTOList = null;
            try
            {
                if (attractionBookingScheduleBL == null)
                {
                    attractionBookingScheduleBL = new AttractionBookingSchedulesBL(KioskStatic.Utilities.ExecutionContext);
                }

                this.Cursor = Cursors.WaitCursor;
                DisplayStatusMessage(MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 1008)); //Processing..Please wait...
                Application.DoEvents();
                scheduleDetailsViewDTOList = BackgroundProcessRunner.Run<List<ScheduleDetailsViewDTO>>(() =>
                {
                    return InvokeBuildScheduleDetailsForView(scheduleDate: scheduleDate,
                        productsList: productId != -1 ? productId.ToString() : "",
                        facilityMapId: facilityMapId, fixedSchedule: null, scheduleFromTime: fromTime,
                        scheduleToTime: toTime, selectedToDateTime: selectedAttractionBookingdDTO.ScheduleToDate,
                        includePast: false, filterProducts: true);
                }
                );
                this.Cursor = Cursors.WaitCursor;

                dayAttractionSchedulesList = new List<DayAttractionScheduleDTO>();
                foreach (ScheduleDetailsViewDTO scheduleDetailsViewDTO in scheduleDetailsViewDTOList)
                {
                    if (scheduleDetailsViewDTO.DayAttractionScheduleDTO != null)
                        dayAttractionSchedulesList.Add(scheduleDetailsViewDTO.DayAttractionScheduleDTO);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Unexpected error in GetEligibleSchedules()" + ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit(scheduleDetailsViewDTOList);
            return scheduleDetailsViewDTOList;
        }

        private List<ScheduleDetailsViewDTO> InvokeBuildScheduleDetailsForView(DateTime scheduleDate,
            string productsList = null, int facilityMapId = -1, bool? fixedSchedule = null,
            decimal? scheduleFromTime = null, decimal? scheduleToTime = null,
            DateTime? selectedToDateTime = null, bool includePast = false, bool filterProducts = false)
        {
            log.LogMethodEntry();
            List<ScheduleDetailsViewDTO> scheduleDetailsViewDTOList = attractionBookingScheduleBL.BuildScheduleDetailsForView(scheduleDate: scheduleDate,
                productsList: productId != -1 ? productId.ToString() : "",
                facilityMapId: facilityMapId, fixedSchedule: null, scheduleFromTime: scheduleFromTime,
                scheduleToTime: scheduleToTime, selectedToDateTime: selectedAttractionBookingdDTO.ScheduleToDate,
                includePast: false, filterProducts: true);

            log.LogMethodExit();
            return scheduleDetailsViewDTOList;
        }

        private AttractionBookingDTO CreateNewBookingAndReleaseOldSlot(ScheduleDetailsDTO scheduleDetailsDTO, int qty, int comboProductId)
        {
            log.LogMethodEntry(scheduleDetailsDTO, qty);
            ReleaseOldSlot();
            AttractionBookingDTO selectedAttrBookingDTO = new AttractionBookingDTO();
            selectedAttrBookingDTO.AttractionProductId = scheduleDetailsDTO.ProductId;
            selectedAttrBookingDTO.Identifier = comboProductId;
            selectedAttrBookingDTO.FacilityMapId = scheduleDetailsDTO.FacilityMapId;
            selectedAttrBookingDTO.AttractionPlayId = scheduleDetailsDTO.AttractionPlayId;
            selectedAttrBookingDTO.AttractionPlayName = scheduleDetailsDTO.AttractionPlayName;
            selectedAttrBookingDTO.AttractionScheduleId = scheduleDetailsDTO.ScheduleId;
            selectedAttrBookingDTO.AttractionScheduleName = scheduleDetailsDTO.ScheduleName;
            selectedAttrBookingDTO.ScheduleFromDate = scheduleDetailsDTO.ScheduleFromDate;
            selectedAttrBookingDTO.ScheduleToDate = scheduleDetailsDTO.ScheduleToDate;
            selectedAttrBookingDTO.BookedUnits = qty;
            selectedAttrBookingDTO.Price = scheduleDetailsDTO.Price == null ? 0 : Convert.ToDouble(scheduleDetailsDTO.Price.ToString());
            selectedAttrBookingDTO.PromotionId = scheduleDetailsDTO.PromotionId;
            selectedAttrBookingDTO.AvailableUnits = scheduleDetailsDTO.TotalUnits;
            selectedAttrBookingDTO.ScheduleFromTime = scheduleDetailsDTO.ScheduleFromTime;
            selectedAttrBookingDTO.ScheduleToTime = scheduleDetailsDTO.ScheduleToTime;
            if (scheduleDetailsDTO.ExpiryDate != null)
            {
                selectedAttrBookingDTO.ExpiryDate = Convert.ToDateTime(scheduleDetailsDTO.ExpiryDate.ToString());
            }
            log.LogMethodExit(selectedAttrBookingDTO);
            return selectedAttrBookingDTO;
        }

        private void ReleaseOldSlot()
        {
            log.LogMethodEntry();
            try
            {
                if (selectedAttractionBookingdDTO != null && selectedAttractionBookingdDTO.BookingId > -1)
                {

                    AttractionBooking atb = new AttractionBooking(executionContext, selectedAttractionBookingdDTO);
                    atb.Expire();
                    selectedAttractionBookingdDTO = null;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error while releaseing old blocked schedule: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void DisableButtons()
        {
            log.LogMethodEntry();
            pnlTimeSection.SuspendLayout();
            this.btnProceed.Enabled = false;
            this.btnPrev.Enabled = false;
            this.btnCancel.Enabled = false;
            flpDates.Enabled = false;
            log.LogMethodExit();
        }

        private void EnableButtons()
        {
            log.LogMethodEntry();

            flpDates.Enabled = allowChangeScheduleDate;
            pnlTimeSection.ResumeLayout();
            this.btnProceed.Enabled = true;
            this.btnPrev.Enabled = true;
            this.btnCancel.Enabled = true;
            pnlDateSection.Enabled = true;
            log.LogMethodExit();
        }

        private void DisplayStatusMessage(string msg)
        {
            log.LogMethodEntry();
            txtMessage.Text = msg;
            log.LogMethodExit();
        }

        private void ShowOrHideNoSchedulesAvailableMsg(bool show)
        {
            log.LogMethodEntry(show);
            lblNoSchedules.Visible = show;
            flpSlots.Visible = bigVerticalScrollView.Visible = !show;
            log.LogMethodExit();
        }

        protected override void CloseForms()
        {
            log.LogMethodEntry();
            int lowerLimit = 1;
            for (int i = Application.OpenForms.Count - 1; i > lowerLimit; i--)
            {
                if (attractionForms.Exists(fName => fName == Application.OpenForms[i].Name) == true)
                {
                    Application.OpenForms[i].Visible = false;
                }
            }
            base.CloseForms();
            log.LogMethodExit();
        }
        private void GoBackToProductSelectionScreen()
        {
            log.LogMethodEntry();
            int lowerLimit = 2; //frmChooseProduct 
            for (int i = Application.OpenForms.Count - 1; i > lowerLimit; i--)
            {
                if (Application.OpenForms[i].Name == "frmChooseProduct")
                {
                    break;
                }
                if (attractionForms.Exists(fName => fName == Application.OpenForms[i].Name) == true)
                {
                    Application.OpenForms[i].Visible = false;
                    Application.OpenForms[i].Close();
                }
            }
            log.LogMethodExit();
        }
    }
}
