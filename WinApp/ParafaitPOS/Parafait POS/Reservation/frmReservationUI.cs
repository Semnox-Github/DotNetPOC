/********************************************************************************************
 * Project Name - Reservation
 * Description  - Reservation form
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.70.0      26-Mar-2019   Guru S A                Created for Booking phase 2 enhancement changes 
 *2.70.0      27-Jul-2019   Nitin Pai               Attraction Enhancement 
 *2.70.2      11-NOv-2019   Guru S A                Waiver phase 2 enhancement
 *2.80.0      22-Apr-2020   Guru S A                Ability to reschedule attraction schedule selection as per reservation change
 *2.80        18-May-2020   Laster Menezes          Exception handling while printing Booking Receipt and BookingCheckList 
 *2.80.0      09-Jun-2020   Jinto Thomas            Enable Active flag for Comboproduct data
 *2.90        18-Aug-2020   Laster Menezes          Added method PerformFiscaliation for fiscalizing the booking payments
 *2.100.0     01-Sep-2020   Guru S A                Payment link changes
 *2.110.0     22-Dec-2020   Girish Kundar           Modified :FiscalTrust changes - Advance payment to be fiscalized
 *2.110.0     14-Feb-2021   Nitin Pai               Modified - Autobahn Event Handling Changes
 *2.110.0     14-Mar-2021   Guru S A                Subscription phase one changes
 *  *2.120.0  29-mar-2021   Sathyavathi             Bug fix - Attraction within Reservations
 *2.120       26-Apr-2021   Laster Menezes          Modified BookingReceipt generation methods to use receipt framework of reports 
 *                                                  to generate Booking receipt and booking checklist receipts on one click
 *                                                  modified PerformFiscaliation method to store the fiscal reference in ExternalSourceReference field 
 *2.140.0     12-Dec-2021   Guru S A                Booking execute process performance fixes
 *2.130.4     22-Feb-2022   Mathew Ninan            Modified DateTime to ServerDateTime 
 *2.140.5     28-Apr-2023   Rakshith Shetty         Modified method GetBookingReceiptReport to print the report source instead of generating the pdf and then printing.   
 *2.150.9     22-Mar-2024   Vignesh Bhat            Modified: Remove  Waiver validation for past transaction date
 *********************************************************************************************/
using Parafait_POS.Attraction;
using Parafait_POS.Waivers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Category;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Customer.Waivers;
using Semnox.Parafait.Device.Printer.FiscalPrint;
using Semnox.Parafait.Discounts;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Maintenance;
using Semnox.Parafait.POS;
using Semnox.Parafait.Printer;
using Semnox.Parafait.Printer.Cashdrawers;
using Semnox.Parafait.Product;
using Semnox.Parafait.Report.Reports;
using Semnox.Parafait.Reports;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.User;
using Semnox.Parafait.Waiver;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Parafait_POS.Reservation
{
    public partial class frmReservationUI : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ReservationBL reservationBL;
        private ExecutionContext executionContext = null;
        private Utilities utilities = null;
        private bool userAction = false;
        private CustomerDetailUI customerDetailUI;
        private ParafaitEnv ParafaitEnv;
        private DataTable dtFromTime = new DataTable();
        private DataTable dtToTime = new DataTable();
        private BindingSource bsFromTime = new BindingSource();
        private BindingSource bsToTime = new BindingSource();
        private List<TransactionProfileDTO> transactionProfileDTOList;
        //private List<MasterScheduleDTO> masterScheduleDTOList;
        private decimal fromTimeForSearch;
        private decimal toTimeForSearch;
        //private int masterScheduleIdForSearch;
        private int facilityIdForSearch;
        private int productIdForSearch;
        //private int guestCountForSearch;
        private List<SchedulesDTO> schedulesDTOMasterList = new List<SchedulesDTO>();
        private string excludeAttractionSchedule = "-1";
        private int exattractionScheduleId;
        private string contentID = "";
        private List<UsersDTO> usersDTOList;
        private List<LookupValuesDTO> checkListStatusList;
        private VirtualKeyboardController virtualKeyboard;
        private decimal lastTimeSlotForTheDay = (decimal)23.55;
        private ReservationDefaultSetup reservationDefaultSetup;
        private SelectedScheduleNProductDetails selectedScheduleNProductDetails;
        private List<UserJobItemsDTO> userJobItemsDTOList;
        private FiscalPrinter fiscalPrinter;
        private bool enablePaymentLinkButton = false;
        private bool hideBookedSlots = true;
        private List<FacilityMapDTO> facilityMapDTOList;
        private Dictionary<int, List<ProductModifiersDTO>> productModifierDictonary = new Dictionary<int, List<ProductModifiersDTO>>();
        private Dictionary<int, ProductsDTO> productDictonary = new Dictionary<int, ProductsDTO>();
        private Dictionary<int, List<ComboProductDTO>> comboProductDictonary = new Dictionary<int, List<ComboProductDTO>>();
        private Dictionary<int, List<CategoryDTO>> categoryDictonary = new Dictionary<int, List<CategoryDTO>>();
        private bool autoChargeOptionEnabled = false;
        public delegate void PerformActivityTimeOutChecksdelegate(int inactivityPeriodSec);
        public PerformActivityTimeOutChecksdelegate PerformActivityTimeOutChecks;
        private const string WEB_SITE_CONFIGURATION = "WEB_SITE_CONFIGURATION";
        private const string ONLINE_PARTY_BOOKINGS_EMAIL_GROUP = "ONLINE_PARTY_BOOKINGS_EMAIL_GROUP";
        public frmReservationUI(int bookingId)
        {
            log.LogMethodEntry(bookingId);
            SetUtilsAndContext();
            InitUIElements(bookingId);
            DateTime startDate = ServerDateTime.Now;
            if (this.reservationBL.GetReservationDTO != null)
            {
                startDate = this.reservationBL.GetReservationDTO.FromDate;
            }
            SetDefaultSearchDateParameters(startDate.Date, (decimal)startDate.Hour);
            autoChargeOptionEnabled = ReservationDefaultSetup.IsAutoChargeOptionEnabledForReservation(executionContext);
            log.LogMethodExit();
        }

        public frmReservationUI(DateTime pDate, decimal pfromTime)
        {
            log.LogMethodEntry(pDate, pfromTime);
            SetUtilsAndContext();
            InitUIElements(-1);
            SetDefaultSearchDateParameters(pDate, pfromTime);
            autoChargeOptionEnabled = ReservationDefaultSetup.IsAutoChargeOptionEnabledForReservation(executionContext);
            log.LogMethodExit();
        }

        private void SetUtilsAndContext()
        {
            log.LogMethodEntry();
            utilities = POSStatic.Utilities;
            ParafaitEnv = POSStatic.ParafaitEnv;
            executionContext = ExecutionContext.GetExecutionContext();
            if (utilities.ParafaitEnv.IsCorporate)//Starts:Modification on 02-jan-2017 fo customer feedback
            {
                executionContext.SetSiteId(utilities.ParafaitEnv.SiteId);
            }
            else
            {
                executionContext.SetSiteId(-1);
            }
            executionContext.SetMachineId(utilities.ParafaitEnv.POSMachineId);
            log.LogMethodExit();
        }

        private void InitUIElements(int bookingId)
        {
            log.LogMethodEntry(bookingId);
            try
            {
                userAction = false;
                POSUtils.SetLastActivityDateTime();
                this.usrCtrlPkgProductDetails1 = new usrCtrlProductDetails();
                this.usrCtrlAdditionProductDetails1 = new usrCtrlProductDetails();
                this.reservationDefaultSetup = new ReservationDefaultSetup(executionContext);
                InitializeComponent();
                LoadReservationBL(bookingId);
                utilities.setLanguage();
                SetscheduleDTOMasterList();
                LoadCustomerUIPanel();
                LoadFacility();
                LoadBookingProductDropDown();
                LoadTimeDropdownDropDown();
                SetStyleForDGV();
                Common.Devices.RegisterCardReaders(new EventHandler(CardScanCompleteEventHandle));
                GetTransactionProfiles();
                LoadCheckListStatus();
                LoadAssignedTo();
                virtualKeyboard = new VirtualKeyboardController();
                //virtualKeyboard.Initialize(this, new List<Control>() { btnShowKeyPad, btnPkgTabShowKeyPad }, ParafaitDefaultContainerList.GetParafaitDefault<bool>(utilities.ExecutionContext, "AUTO_POPUP_ONSCREEN_KEYBOARD"), new List<Control>() { txtSearchGuestCount, pnlCustomerDetail, txtServiceChargeAmount, txtServiceChargePercentage });
                virtualKeyboard.Initialize(this, new List<Control>() { btnShowKeyPad, btnPkgTabShowKeyPad }, ParafaitDefaultContainerList.GetParafaitDefault<bool>(utilities.ExecutionContext, "AUTO_POPUP_ONSCREEN_KEYBOARD"), new List<Control>() { pnlCustomerDetail, txtSvcServiceChargeAmount, txtSvcServiceChargePercentage });
                dgvSearchFacilityProducts.AutoGenerateColumns = false;
                selectedScheduleNProductDetails = null;
                enablePaymentLinkButton = TransactionPaymentLink.ISPaymentLinkEnbled(executionContext);
                SetHideBookedSlotFlag();
                if (hideBookedSlots)
                {
                    cbxHideBookedSlots.Visible = true;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                txtMessage.Text = ex.Message;
            }
            finally
            {
                POSUtils.SetLastActivityDateTime();
                userAction = true;
            }
            log.LogMethodExit();
        }



        private void LoadReservationBL(int bookingId)
        {
            log.LogMethodEntry(bookingId);
            POSUtils.SetLastActivityDateTime();
            if (bookingId == -1)
            {
                reservationBL = new ReservationBL(utilities.ExecutionContext, utilities);
            }
            else
            {
                reservationBL = new ReservationBL(utilities.ExecutionContext, utilities, bookingId);
            }
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }

        private void SetscheduleDTOMasterList()
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            List<MasterScheduleBL> masterScheduleBLList = ((Parafait_POS.POS)Application.OpenForms["POS"]).MasterScheduleBLList;
            if (masterScheduleBLList == null || masterScheduleBLList.Any() == false)
            {
                POSUtils.SetLastActivityDateTime();
                ((Parafait_POS.POS)Application.OpenForms["POS"]).LoadMasterScheduleBLList();
                masterScheduleBLList = ((Parafait_POS.POS)Application.OpenForms["POS"]).MasterScheduleBLList;
            }
            POSUtils.SetLastActivityDateTime();
            if (masterScheduleBLList != null && masterScheduleBLList.Count > 0)
            {
                for (int i = 0; i < masterScheduleBLList.Count; i++)
                {
                    if (masterScheduleBLList[i].MasterScheduleDTO != null && masterScheduleBLList[i].MasterScheduleDTO.SchedulesDTOList != null)
                    {
                        schedulesDTOMasterList.AddRange(masterScheduleBLList[i].MasterScheduleDTO.SchedulesDTOList);
                    }
                }
            }
            log.LogMethodExit();
        }

        private void SetDefaultSearchDateParameters(DateTime pDate, decimal pfromTime)
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            userAction = false;
            int minTimeSlot = this.reservationDefaultSetup.GetCalendarTimeSlotGap;
            dtpSearchDate.Value = pDate;
            if (minTimeSlot <= 15)
            {
                // decimal reduceBy = Math.Round((decimal)((minTimeSlot * 3) / 60),2);
                if (pfromTime > ((decimal)(23.60) - (decimal)(minTimeSlot * 3) / 100) && pfromTime <= (decimal)24)
                    pfromTime = ((decimal)(23.60) - (decimal)(minTimeSlot * 3) / 100);
            }
            else
            {
                if (pfromTime > ((decimal)(23.60) - (decimal)(minTimeSlot) / 100) && pfromTime <= (decimal)24)
                    pfromTime = GetLastFromHourSlot(pfromTime, minTimeSlot);
            }
            pfromTime = GetFromHourSlot(pfromTime, minTimeSlot);
            cmbSearchFromTime.SelectedValue = pfromTime;
            //Load from time + 2 hrs to  totime
            decimal fromTime = Convert.ToDecimal(cmbSearchFromTime.SelectedValue);
            int hourSlot = GetHourSlot(minTimeSlot);
            int totToMins = (int)(Math.Floor(fromTime) * 60 + (fromTime - Math.Floor(fromTime)) * 100 + hourSlot);
            decimal toTime = totToMins / 60 + (decimal)((totToMins % 60) / 100.0);
            if (toTime > ((decimal)23.60 - (decimal)(minTimeSlot) / 100))
                toTime = GetLastToHourSlot(pfromTime, minTimeSlot);

            cmbSearchToTime.SelectedValue = toTime;
            fromTime = pfromTime;
            cmbChannel.SelectedItem = this.reservationDefaultSetup.GetDefaultChannel;
            userAction = true;
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }

        private int GetHourSlot(int minTimeSlot)
        {
            log.LogMethodEntry(minTimeSlot);
            POSUtils.SetLastActivityDateTime();
            int hourSlot = 0;
            while (hourSlot < 120)
            {
                hourSlot = hourSlot + minTimeSlot;
            }
            if (hourSlot > 120)
                hourSlot = hourSlot - minTimeSlot;


            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit(hourSlot);
            return hourSlot;
        }

        private decimal GetFromHourSlot(decimal pfromTime, int minTimeSlot)
        {
            log.LogMethodEntry(pfromTime, minTimeSlot);
            POSUtils.SetLastActivityDateTime();
            decimal hourSlot = 0;

            DateTime startTime = utilities.getServerTime().Date;

            while (hourSlot < pfromTime)
            {
                //dtFromTime.Rows.Add(startTime.ToString("h:mm tt"), startTime.Hour + Math.Round(startTime.Minute / 100.0, 2), startTime);
                //dtToTime.Rows.Add(startTime.ToString("h:mm tt"), startTime.Hour + Math.Round(startTime.Minute / 100.0, 2));
                startTime = startTime.AddMinutes(this.reservationDefaultSetup.GetCalendarTimeSlotGap);
                hourSlot = (decimal)(startTime.Hour + Math.Round(startTime.Minute / 100.0, 2));
            }

            //while (hourSlot < pfromTime)
            //{
            //    hourSlot = hourSlot + (decimal)minTimeSlot/60;
            //}
            if (hourSlot > pfromTime)
            {
                startTime = startTime.AddMinutes(-this.reservationDefaultSetup.GetCalendarTimeSlotGap);
                hourSlot = (decimal)(startTime.Hour + Math.Round(startTime.Minute / 100.0, 2));
            }

            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit(hourSlot);
            return hourSlot;
        }

        private decimal GetLastToHourSlot(decimal pfromTime, int minTimeSlot)
        {
            log.LogMethodEntry(pfromTime, minTimeSlot);
            POSUtils.SetLastActivityDateTime();
            decimal hourSlot = pfromTime;

            DateTime startTime = utilities.getServerTime().Date.AddMinutes((int)pfromTime * 60 + (double)pfromTime % 1 * 100);

            while (startTime.Date == utilities.getServerTime().Date)
            {
                //dtFromTime.Rows.Add(startTime.ToString("h:mm tt"), startTime.Hour + Math.Round(startTime.Minute / 100.0, 2), startTime);
                //dtToTime.Rows.Add(startTime.ToString("h:mm tt"), startTime.Hour + Math.Round(startTime.Minute / 100.0, 2));
                startTime = startTime.AddMinutes(this.reservationDefaultSetup.GetCalendarTimeSlotGap);
                hourSlot = (decimal)(startTime.Hour + Math.Round(startTime.Minute / 100.0, 2));
            }

            //while (hourSlot < pfromTime)
            //{
            //    hourSlot = hourSlot + (decimal)minTimeSlot/60;
            //}
            if (startTime.Date > utilities.getServerTime().Date)
            {
                startTime = startTime.AddMinutes(-this.reservationDefaultSetup.GetCalendarTimeSlotGap);
                hourSlot = (decimal)(startTime.Hour + Math.Round(startTime.Minute / 100.0, 2));
            }

            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit(hourSlot);
            return hourSlot;
        }

        private decimal GetLastFromHourSlot(decimal pfromTime, int minTimeSlot)
        {
            log.LogMethodEntry(pfromTime, minTimeSlot);
            POSUtils.SetLastActivityDateTime();
            decimal hourSlot = pfromTime;

            DateTime startTime = utilities.getServerTime().Date.AddMinutes((int)pfromTime * 60 + (double)pfromTime % 1 * 100);

            while (startTime.Date == utilities.getServerTime().Date)
            {
                //dtFromTime.Rows.Add(startTime.ToString("h:mm tt"), startTime.Hour + Math.Round(startTime.Minute / 100.0, 2), startTime);
                //dtToTime.Rows.Add(startTime.ToString("h:mm tt"), startTime.Hour + Math.Round(startTime.Minute / 100.0, 2));
                startTime = startTime.AddMinutes(this.reservationDefaultSetup.GetCalendarTimeSlotGap);
                hourSlot = (decimal)(startTime.Hour + Math.Round(startTime.Minute / 100.0, 2));
            }

            //while (hourSlot < pfromTime)
            //{
            //    hourSlot = hourSlot + (decimal)minTimeSlot/60;
            //}
            if (startTime.Date > utilities.getServerTime().Date)
            {
                startTime = startTime.AddMinutes(-this.reservationDefaultSetup.GetCalendarTimeSlotGap);
                startTime = startTime.AddMinutes(-this.reservationDefaultSetup.GetCalendarTimeSlotGap);
                hourSlot = (decimal)(startTime.Hour + Math.Round(startTime.Minute / 100.0, 2));
            }

            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit(hourSlot);
            return hourSlot;
        }
        private void LoadCustomerUIPanel()
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            pnlCustomerDetail.Controls.Clear();
            customerDetailUI = new CustomerDetailUI(utilities, POSUtils.ParafaitMessageBox, ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "AUTO_POPUP_ONSCREEN_KEYBOARD"));
            customerDetailUI.FirstNameLeave += new EventHandler(CustomerDetailUI_FirstNameLeave);
            customerDetailUI.CustomerContactInfoEntered += new CustomerContactInfoEnteredEventHandler(CustomerDetailUI_CustomerContactInfoEntered);
            customerDetailUI.UniqueIdentifierValidating += new CancelEventHandler(txtUniqueId_Validating);
            customerDetailUI.Width = 885;
            customerDetailUI.SetBackGroundColor(tpCustomer.BackColor);
            customerDetailUI.Location = new Point((pnlCustomerDetail.Width - 1106) / 2, 0);
            pnlCustomerDetail.Controls.Add(customerDetailUI);
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }
        private void CustomerDetailUI_FirstNameLeave(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                POSUtils.SetLastActivityDateTime();
                this.Cursor = Cursors.WaitCursor;
                CustomerListBL customerListBL = new CustomerListBL(utilities.ExecutionContext);
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
                        this.Cursor = Cursors.Default;
                        return;
                    }
                    try
                    {
                        POSUtils.SetLastActivityDateTime();
                        StopInActivityTimeoutTimer();
                        using (CustomerLookupUI customerLookupUI = new CustomerLookupUI(utilities, customerDetailUI.FirstName))
                        {
                            if (customerLookupUI.ShowDialog() == DialogResult.OK)
                            {
                                POSUtils.SetLastActivityDateTime();
                                if (customerLookupUI.SelectedCustomerDTO != null)
                                {
                                    customerDetailUI.CustomerDTO = customerLookupUI.SelectedCustomerDTO;
                                    SetTrxCustomerDTO();
                                }
                            }
                        }
                    }
                    finally
                    {
                        POSUtils.SetLastActivityDateTime();
                        StartInActivityTimeoutTimer();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }

        private void SetTrxCustomerDTO()
        {
            log.LogMethodEntry();
            try
            {
                POSUtils.SetLastActivityDateTime();
                if (BookingIsInEditMode())
                {
                    reservationBL.AddCustomer(customerDetailUI.CustomerDTO);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                txtMessage.Text = ex.Message;
            }
            log.LogMethodExit();
        }

        private void CustomerDetailUI_CustomerContactInfoEntered(object source, CustomerContactInfoEnteredEventArgs e)
        {
            log.LogMethodEntry(source, e);
            try
            {
                POSUtils.SetLastActivityDateTime();
                this.Cursor = Cursors.WaitCursor;
                CustomerListBL customerListBL = new CustomerListBL(utilities.ExecutionContext);
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
                        this.Cursor = Cursors.Default;
                        return;
                    }
                    try
                    {
                        POSUtils.SetLastActivityDateTime();
                        StopInActivityTimeoutTimer();
                        using (CustomerLookupUI customerLookupUI = new CustomerLookupUI(utilities, "", "", "",
                                                                                             e.ContactType == ContactType.EMAIL ? e.ContactValue : "",
                                                                                             e.ContactType == ContactType.PHONE ? e.ContactValue : "",
                                                                                             ""))
                        {
                            if (customerLookupUI.ShowDialog() == DialogResult.OK)
                            {
                                POSUtils.SetLastActivityDateTime();
                                if (customerLookupUI.SelectedCustomerDTO != null)
                                {
                                    customerDetailUI.CustomerDTO = customerLookupUI.SelectedCustomerDTO;
                                    SetTrxCustomerDTO();
                                }
                            }
                        }
                    }
                    finally
                    {
                        POSUtils.SetLastActivityDateTime();
                        StartInActivityTimeoutTimer();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }


        private void txtUniqueId_Validating(object sender, CancelEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                POSUtils.SetLastActivityDateTime();
                this.Cursor = Cursors.WaitCursor;
                txtMessage.Text = "";

                if ((sender as TextBox).Text.Trim() == "")
                {
                    log.LogMethodExit("Text.Trim() == ''");
                    this.Cursor = Cursors.Default;
                    return;
                }

                List<CustomerDTO> customerDTOList = null;
                CustomerListBL customerListBL = new CustomerListBL(utilities.ExecutionContext);
                CustomerSearchCriteria customerSearchCriteria = new CustomerSearchCriteria(CustomerSearchByParameters.PROFILE_UNIQUE_IDENTIFIER, Operator.EQUAL_TO, (sender as TextBox).Text.Trim());
                customerSearchCriteria.OrderBy(CustomerSearchByParameters.CUSTOMER_ID)
                                      .Paginate(0, 20);
                POSUtils.SetLastActivityDateTime();
                customerDTOList = customerListBL.GetCustomerDTOList(customerSearchCriteria, false, false);
                POSUtils.SetLastActivityDateTime();
                if (customerDTOList != null && customerDTOList.Count > 0)
                {

                    if (POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 212, customerDTOList[0].FirstName + (customerDTOList[0].LastName == "" ? "" : " " + customerDTOList[0].LastName)), MessageContainerList.GetMessage(executionContext, "Customer Registration"), MessageBoxButtons.OK) == System.Windows.Forms.DialogResult.OK)
                    {
                        POSUtils.SetLastActivityDateTime();
                        if (ParafaitEnv.ALLOW_DUPLICATE_UNIQUE_ID == "N")
                            e.Cancel = true;
                    }
                    else
                    {
                        POSUtils.SetLastActivityDateTime();
                        txtMessage.Text = MessageContainerList.GetMessage(executionContext, 290);
                        if (ParafaitEnv.ALLOW_DUPLICATE_UNIQUE_ID == "N")
                        {
                            e.Cancel = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            finally
            {
                this.Cursor = Cursors.Default;
                POSUtils.SetLastActivityDateTime();
            }
            log.LogMethodExit();
        }

        private void LoadFacility()
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            FacilityMapListBL facilityMapListBL = new FacilityMapListBL(executionContext);
            List<KeyValuePair<FacilityMapDTO.SearchByParameters, string>> facilitySearcParm = new List<KeyValuePair<FacilityMapDTO.SearchByParameters, string>>();
            facilitySearcParm.Add(new KeyValuePair<FacilityMapDTO.SearchByParameters, string>(FacilityMapDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            facilitySearcParm.Add(new KeyValuePair<FacilityMapDTO.SearchByParameters, string>(FacilityMapDTO.SearchByParameters.IS_ACTIVE, "1"));
            facilitySearcParm.Add(new KeyValuePair<FacilityMapDTO.SearchByParameters, string>(FacilityMapDTO.SearchByParameters.HAVING_PRODUCT_TYPES_IN, ProductTypeValues.BOOKINGS));
            facilityMapDTOList = facilityMapListBL.GetFacilityMapDTOList(facilitySearcParm, true);
            FacilityMapDTO firstfacilityMapDTO = new FacilityMapDTO
            {
                FacilityMapName = "- All -"
            };
            if (facilityMapDTOList == null || facilityMapDTOList.Count == 0)
            {
                facilityMapDTOList = new List<FacilityMapDTO>();
            }
            List<FacilityMapDTO> dupNameList = facilityMapDTOList.Where(f => facilityMapDTOList.Where(ff => ff.FacilityMapName == f.FacilityMapName).Count() > 1).ToList();
            if (dupNameList != null && dupNameList.Any())
            {
                for (int i = 0; i < facilityMapDTOList.Count; i++)
                {
                    if (dupNameList.Exists(f => f.FacilityMapId == facilityMapDTOList[i].FacilityMapId))
                    {
                        facilityMapDTOList[i].FacilityMapName = facilityMapDTOList[i].FacilityMapName + " [" + facilityMapDTOList[i].FacilityMapId + "]";
                    }
                }
            }
            facilityMapDTOList.Insert(0, firstfacilityMapDTO);

            cmbSearchFacility.DataSource = facilityMapDTOList;
            cmbSearchFacility.DisplayMember = "FacilityMapName";
            cmbSearchFacility.ValueMember = "FacilityMapId";
            cmbSearchFacility.SelectedIndex = 0;
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }

        private void LoadBookingProductDropDown()
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            ProductsList productsListBL = new ProductsList(executionContext);
            List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>> searchParams = new List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>>();
            searchParams.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.PRODUCT_TYPE_NAME, "BOOKINGS"));
            searchParams.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.ISACTIVE, "Y"));
            searchParams.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.SITEID, executionContext.GetSiteId().ToString()));
            List<ProductsDTO> bookinProductsDTOLIst = productsListBL.GetProductsDTOList(searchParams);
            if (bookinProductsDTOLIst == null)
            {
                bookinProductsDTOLIst = new List<ProductsDTO>();
            }
            searchParams.Clear();
            if (reservationBL.GetReservationDTO != null)
            {
                int bookedProductId = reservationBL.GetReservationDTO.BookingProductId;
                searchParams.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.PRODUCT_TYPE_NAME, "BOOKINGS"));
                searchParams.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.ISACTIVE, "N"));
                searchParams.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.PRODUCT_ID, bookedProductId.ToString()));
                searchParams.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.SITEID, executionContext.GetSiteId().ToString()));
                List<ProductsDTO> inActivebookinProductsDTOLIst = productsListBL.GetProductsDTOList(searchParams);
                if (inActivebookinProductsDTOLIst != null && inActivebookinProductsDTOLIst.Count > 0)
                {
                    foreach (ProductsDTO inactiveBooking in inActivebookinProductsDTOLIst)
                    {
                        bookinProductsDTOLIst.Add(inactiveBooking);
                    }
                }
            }

            bookinProductsDTOLIst = bookinProductsDTOLIst.OrderBy(prod => prod.ProductName).ToList();

            ProductsDTO productsALLDTO = new ProductsDTO
            {
                ProductName = "- All -"
            };
            bookinProductsDTOLIst.Insert(0, productsALLDTO);
            cmbSearchBookingProduct.DisplayMember = "ProductName";
            cmbSearchBookingProduct.ValueMember = "ProductId";
            cmbSearchBookingProduct.DataSource = bookinProductsDTOLIst;
            cmbSearchBookingProduct.SelectedIndex = 0;
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }

        private void LoadTimeDropdownDropDown()
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            dtFromTime.Columns.Add("Display");
            dtFromTime.Columns.Add("Value");
            dtFromTime.Columns.Add(new DataColumn("Compare", typeof(DateTime)));

            dtToTime.Columns.Add("Display");
            dtToTime.Columns.Add("Value");

            DateTime startTime = utilities.getServerTime().Date;
            while (startTime < utilities.getServerTime().Date.AddDays(1))
            {
                dtFromTime.Rows.Add(startTime.ToString("h:mm tt"), startTime.Hour + Math.Round(startTime.Minute / 100.0, 2), startTime);
                dtToTime.Rows.Add(startTime.ToString("h:mm tt"), startTime.Hour + Math.Round(startTime.Minute / 100.0, 2));
                startTime = startTime.AddMinutes(this.reservationDefaultSetup.GetCalendarTimeSlotGap);
            }
            cmbSearchFromTime.DisplayMember = "Display";
            cmbSearchFromTime.ValueMember = "Value";
            cmbSearchFromTime.DataSource = dtFromTime;
            cmbSearchToTime.DisplayMember = "Display";
            cmbSearchToTime.ValueMember = "Value";
            cmbSearchToTime.DataSource = dtToTime;

            cmbFromTime.DisplayMember = "Display";
            cmbFromTime.ValueMember = "Value";
            bsFromTime.DataSource = dtFromTime;
            cmbFromTime.DataSource = bsFromTime;
            cmbToTime.DisplayMember = "Display";
            cmbToTime.ValueMember = "Value";
            bsToTime.DataSource = dtToTime;
            cmbToTime.DataSource = bsToTime;

            lastTimeSlotForTheDay = Convert.ToDecimal(dtToTime.Rows[dtToTime.Rows.Count - 1]["Value"]);
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }

        private void LoadCheckListStatus()
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            LookupValuesList lookupValuesList = new LookupValuesList(utilities.ExecutionContext);
            List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
            lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "MAINT_JOB_STATUS"));
            lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            checkListStatusList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
            if (checkListStatusList == null)
            {
                checkListStatusList = new List<LookupValuesDTO>();
            }
            LookupValuesDTO lookupValuesDTO = new LookupValuesDTO();
            checkListStatusList.Insert(0, lookupValuesDTO);
            checkListStatus.DataSource = checkListStatusList;
            checkListStatus.DisplayMember = "LookupValue";
            checkListStatus.ValueMember = "LookupValueId";
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }
        private void LoadAssignedTo()
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            UsersList usersList = new UsersList(executionContext);
            List<KeyValuePair<UsersDTO.SearchByUserParameters, string>> usersSearchParams = new List<KeyValuePair<UsersDTO.SearchByUserParameters, string>>();
            usersSearchParams.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.ACTIVE_FLAG, "Y"));
            usersSearchParams.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            usersDTOList = usersList.GetAllUsers(usersSearchParams);
            if (usersDTOList == null)
            {
                usersDTOList = new List<UsersDTO>();
            }
            usersDTOList.Insert(0, new UsersDTO());
            assignedUserId.DataSource = usersDTOList;
            assignedUserId.DisplayMember = "UserName";
            assignedUserId.ValueMember = "UserId";
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }
        private void SetStyleForDGV()
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            utilities.setupDataGridProperties(ref dgvSearchSchedules);
            utilities.setupDataGridProperties(ref dgvSearchFacilityProducts);
            utilities.setupDataGridProperties(ref dgvSelectedBookingSchedule);
            utilities.setupDataGridProperties(ref dgvUserJobTaskList);
            utilities.setupDataGridProperties(ref dgvAuditTrail);

            dgvSelectedBookingSchedule.Columns["ReservationDate"].DefaultCellStyle = utilities.gridViewDateTimeCellStyle();

            dgvSearchSchedules.Columns["scheduleNameDataGridViewTextBoxColumn"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //dgvSearchSchedules.Columns["scheduleToTimeDataGridViewTextBoxColumn"].DefaultCellStyle = utilities.gridViewDateTimeCellStyle(); 
            // dgvSearchFacilityProducts.Columns["availableUnitsDataGridViewTextBoxColumn1"].DefaultCellStyle = utilities.gridViewNumericCellStyle();
            //dgvSelectedBookingSchedule.Columns["ReservationDate"].DefaultCellStyle = utilities.gridViewDateTimeCellStyle();
            ////dgvSelectedBookingSchedule.Columns["scheduleFromDateDataGridViewTextBoxColumn"].DefaultCellStyle = gridViewNumericCellStyle();
            ////dgvSelectedBookingSchedule.Columns["scheduleToDateDataGridViewTextBoxColumn"].DefaultCellStyle = gridViewNumericCellStyle();
            //dgvSelectedBookingSchedule.Columns["guestQuantityDataGridViewTextBoxColumn"].DefaultCellStyle = gridViewNumericCellStyle();
            //dgvBookingProducts.BorderStyle = BorderStyle.FixedSingle;
            //dgvBookingProducts.Columns["dcProduct"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //dgvBookingProducts.Columns["AdditionalDiscountName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            SetDGVCellFont(dgvSearchSchedules);
            SetDGVCellFont(dgvSearchFacilityProducts);
            SetDGVCellFont(dgvSelectedBookingSchedule);
            SetDGVCellFont(dgvUserJobTaskList);
            SetDGVCellFont(dgvAuditTrail);
            SetDGVCellOrderforSearchFacilityProducts();
            SetDGVCellOrderForSelectedBookingSchedule();
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }

        private void SetDGVCellFont(DataGridView dgvInput)
        {
            log.LogMethodEntry();
            System.Drawing.Font font;
            try
            {
                font = new Font(utilities.ParafaitEnv.DEFAULT_GRID_FONT, 15, FontStyle.Regular);
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


        private void SetDGVCellOrderForSelectedBookingSchedule()
        {
            log.LogMethodEntry();
            InsertColumnAtGivenPosition(dgvSelectedBookingSchedule, "RemoveLine", 0);
            InsertColumnAtGivenPosition(dgvSelectedBookingSchedule, "ReservationDate", 1);
            InsertColumnAtGivenPosition(dgvSelectedBookingSchedule, "Reschedule", 2);
            InsertColumnAtGivenPosition(dgvSelectedBookingSchedule, "cmbFromTime", 3);
            InsertColumnAtGivenPosition(dgvSelectedBookingSchedule, "cmbToTime", 4);
            InsertColumnAtGivenPosition(dgvSelectedBookingSchedule, "guestQty", 5);
            InsertColumnAtGivenPosition(dgvSelectedBookingSchedule, "BookingProductName", 6);
            InsertColumnAtGivenPosition(dgvSelectedBookingSchedule, "facilityMapNameDataGridViewTextBoxColumn1", 7);
            InsertColumnAtGivenPosition(dgvSelectedBookingSchedule, "trxLineProductNameDataGridViewTextBoxColumn", 8);
            dgvSelectedBookingSchedule.Columns["RemoveLine"].DisplayIndex = 0;
            dgvSelectedBookingSchedule.Columns["ReservationDate"].DisplayIndex = 1;
            dgvSelectedBookingSchedule.Columns["Reschedule"].DisplayIndex = 2;
            dgvSelectedBookingSchedule.Columns["cmbFromTime"].DisplayIndex = 3;
            dgvSelectedBookingSchedule.Columns["cmbToTime"].DisplayIndex = 4;
            dgvSelectedBookingSchedule.Columns["guestQty"].DisplayIndex = 5;
            dgvSelectedBookingSchedule.Columns["BookingProductName"].DisplayIndex = 6;
            dgvSelectedBookingSchedule.Columns["facilityMapNameDataGridViewTextBoxColumn1"].DisplayIndex = 7;
            dgvSelectedBookingSchedule.Columns["trxLineProductNameDataGridViewTextBoxColumn"].DisplayIndex = 8;
            log.LogMethodExit();
        }

        private void SetDGVCellOrderforSearchFacilityProducts()
        {
            log.LogMethodEntry();
            InsertColumnAtGivenPosition(dgvSearchFacilityProducts, "SelectedRecord", 0);
            InsertColumnAtGivenPosition(dgvSearchFacilityProducts, "productNameDataGridViewTextBoxColumn1", 1);
            InsertColumnAtGivenPosition(dgvSearchFacilityProducts, "productInformation", 2);
            InsertColumnAtGivenPosition(dgvSearchFacilityProducts, "ProductType", 3);
            dgvSearchFacilityProducts.Columns["SelectedRecord"].DisplayIndex = 0;
            dgvSearchFacilityProducts.Columns["productNameDataGridViewTextBoxColumn1"].DisplayIndex = 1;
            dgvSearchFacilityProducts.Columns["productInformation"].DisplayIndex = 2;
            dgvSearchFacilityProducts.Columns["ProductType"].DisplayIndex = 3;
            dgvSearchFacilityProducts.Columns["ProductType"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            // dgvSearchFacilityProducts.Columns["availableUnitsDataGridViewTextBoxColumn1"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            //dgvSearchFacilityProducts.Columns["availableUnitsDataGridViewTextBoxColumn1"].Width = 75;
            log.LogMethodExit();
        }

        private void InsertColumnAtGivenPosition(DataGridView dgv, string dgvColumnName, int insertPosition)
        {
            log.LogMethodEntry(dgvColumnName, insertPosition);
            DataGridViewColumn addColumn = dgv.Columns[dgvColumnName];
            dgv.Columns.Remove(addColumn);
            dgv.Columns.Insert(insertPosition, addColumn);

            log.LogMethodExit();
        }
        private void CardScanCompleteEventHandle(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e is DeviceScannedEventArgs)
            {
                POSUtils.SetLastActivityDateTime();
                txtMessage.Clear();
                DeviceScannedEventArgs checkScannedEvent = e as DeviceScannedEventArgs;
                TagNumber tagNumber;
                TagNumberParser tagNumberParser = new TagNumberParser(executionContext);
                string scannedTagNumber = checkScannedEvent.Message;
                DeviceClass encryptedTagDevice = sender as DeviceClass;
                if (tagNumberParser.IsTagDecryptApplicable(encryptedTagDevice, checkScannedEvent.Message.Length))
                {
                    string decryptedTagNumber = string.Empty;
                    try
                    {
                        decryptedTagNumber = tagNumberParser.GetDecryptedTagData(encryptedTagDevice, checkScannedEvent.Message);
                    }
                    catch (Exception ex)
                    {
                        log.LogVariableState("Decrypted Tag Number result: ", ex);
                        txtMessage.Text = ex.Message;
                        return;
                    }
                    try
                    {
                        scannedTagNumber = tagNumberParser.ValidateDecryptedTag(decryptedTagNumber, utilities.ParafaitEnv.SiteId);
                    }
                    catch (ValidationException ex)
                    {
                        log.LogVariableState("Decrypted Tag Number validation: ", ex);
                        txtMessage.Text = ex.Message;
                        return;
                    }
                    catch (Exception ex)
                    {
                        log.LogVariableState("Decrypted Tag Number validation: ", ex);
                        txtMessage.Text = ex.Message;
                        return;
                    }
                }
                if (tagNumberParser.TryParse(scannedTagNumber, out tagNumber) == false)
                {
                    string message = tagNumberParser.Validate(scannedTagNumber);
                    txtMessage.Text = message;
                    log.LogMethodExit(null, "Invalid Tag Number.");
                    return;
                }
                // string CardNumber = utilities.ProcessScannedCode(checkScannedEvent.Message, utilities.ParafaitEnv.LEFT_TRIM_CARD_NUMBER, utilities.ParafaitEnv.RIGHT_TRIM_CARD_NUMBER);
                CardSwiped(tagNumber.Value);
            }
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }

        private void CardSwiped(string CardNumber)
        {
            log.LogMethodEntry(CardNumber);
            try
            {
                POSUtils.SetLastActivityDateTime();
                if (BookingIsInEditMode())
                {
                    txtCardNumber.Text = CardNumber;
                    ValidateCard();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                txtMessage.Text = ex.Message;
            }
            log.LogMethodExit();
        }

        private bool BookingIsInEditMode()
        {
            log.LogMethodEntry();
            bool inEditMode = ((reservationBL.GetReservationDTO == null || (reservationBL.GetReservationDTO != null
                                                                            && (reservationBL.GetReservationDTO.Status == ReservationDTO.ReservationStatus.NEW.ToString()
                                                                                || reservationBL.GetReservationDTO.Status == ReservationDTO.ReservationStatus.WIP.ToString()
                                                                                || reservationBL.GetReservationDTO.Status == ReservationDTO.ReservationStatus.BLOCKED.ToString())
                                                                              )));
            log.LogMethodExit(inEditMode);
            return inEditMode;
        }

        private void ValidateCard()
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            txtMessage.Clear();
            if (BookingIsInEditMode())
            {
                if (string.IsNullOrEmpty(txtCardNumber.Text.Trim()) == false)
                {
                    Card card = new Card(txtCardNumber.Text.Trim(), "", utilities);
                    POSUtils.SetLastActivityDateTime();
                    if (card.CardStatus.Equals("NEW"))
                    {
                        txtMessage.Text = MessageContainerList.GetMessage(executionContext, 172);
                        txtCardNumber.Text = "";
                        log.LogMethodExit("Card is Invalid");
                        this.Cursor = Cursors.Default;
                        return;
                    }
                    else if (card.technician_card.Equals('N') == false)
                    {
                        txtMessage.Text = MessageContainerList.GetMessage(executionContext, 197, txtCardNumber.Text);
                        txtCardNumber.Text = "";
                        log.LogMethodExit("Tech card used");
                        this.Cursor = Cursors.Default;
                        return;
                    }
                    else if (card.customer_id > 0)
                    {
                        POSUtils.SetLastActivityDateTime();
                        CustomerDTO customerDTO = (new CustomerBL(executionContext, card.customer_id)).CustomerDTO;
                        customerDetailUI.CustomerDTO = customerDTO;
                    }

                    if (string.IsNullOrEmpty(txtCardNumber.Text) == false)
                    {
                        reservationBL.SetCustomerCard(card);
                        //reservationBL.BookingTransaction.PrimaryCard = card;
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(txtCardNumber.Text))
                    {
                        reservationBL.ResetCustomerCard();
                        // reservationBL.BookingTransaction.PrimaryCard = null;
                    }
                }
                SetTrxCustomerDTO();
            }
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }
        private void GetTransactionProfiles()
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            TransactionProfileListBL transactionProfileListBL = new TransactionProfileListBL(executionContext);
            List<KeyValuePair<TransactionProfileDTO.SearchByParameters, string>> searchParam = new List<KeyValuePair<TransactionProfileDTO.SearchByParameters, string>>();
            searchParam.Add(new KeyValuePair<TransactionProfileDTO.SearchByParameters, string>(TransactionProfileDTO.SearchByParameters.IS_ACTIVE, "1"));
            searchParam.Add(new KeyValuePair<TransactionProfileDTO.SearchByParameters, string>(TransactionProfileDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            transactionProfileDTOList = transactionProfileListBL.GetTransactionProfileDTOList(searchParam);
            if (transactionProfileDTOList == null || transactionProfileDTOList.Count == 0)
            {
                transactionProfileDTOList = new List<TransactionProfileDTO>();
            }
            TransactionProfileDTO transactionProfileDTO = new TransactionProfileDTO
            {
                ProfileName = "<SELECT>"
            };
            transactionProfileDTOList.Insert(0, transactionProfileDTO);
            POSUtils.SetLastActivityDateTime();
            //trxProfilesBindingSource.DataMember = "TransactionProfileId";
            //this.transactionProfileId.DataSource = this.additionalTransactionProfileId.DataSource = transactionProfileDTOList;
            //this.transactionProfileId.DisplayMember = this.additionalTransactionProfileId.DisplayMember = "ProfileName";
            //this.transactionProfileId.ValueMember = this.additionalTransactionProfileId.ValueMember = "TransactionProfileId";
            log.LogMethodExit();
        }

        private void ValidateSearchParameters()
        {
            log.LogMethodEntry();
            List<ValidationError> validationErrorList = new List<ValidationError>();
            if (cmbSearchBookingProduct.SelectedValue == null)
            {
                validationErrorList.Add(new ValidationError(MessageContainerList.GetMessage(executionContext, "Validation Error"),
                                                            MessageContainerList.GetMessage(executionContext, "Booking Product"),
                                                            MessageContainerList.GetMessage(executionContext, 1796)));
            }
            if (cmbSearchFacility.SelectedValue == null)
            {
                validationErrorList.Add(new ValidationError(MessageContainerList.GetMessage(executionContext, "Validation Error"),
                                                            MessageContainerList.GetMessage(executionContext, "Facility"),
                                                            MessageContainerList.GetMessage(executionContext, 694)));
            }
            if (cmbSearchFromTime.SelectedValue == null)
            {
                validationErrorList.Add(new ValidationError(MessageContainerList.GetMessage(executionContext, "Validation Error"),
                                                             MessageContainerList.GetMessage(executionContext, "From Time"),
                                                             MessageContainerList.GetMessage(executionContext, "Select From Time")));
            }
            if (cmbSearchToTime.SelectedValue == null)
            {
                validationErrorList.Add(new ValidationError(MessageContainerList.GetMessage(executionContext, "Validation Error"),
                                                             MessageContainerList.GetMessage(executionContext, "To Time"),
                                                             MessageContainerList.GetMessage(executionContext, "Select To Time")));
            }
            if (validationErrorList != null && validationErrorList.Count > 0)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Validation Error"), validationErrorList);
            }
            //if (BookingId == -1 || lblStatus.Text.ToUpper() == ReservationDTO.ReservationStatus.BLOCKED.ToString())
            //{

            //searchParamList.Add(new SqlParameter("@Fromdate", fromDate.Date));
            //searchParamList.Add(new SqlParameter("@FromdateTime", fromDate));
            //searchParamList.Add(new SqlParameter("@TodateTime", toDate));
            //searchParamList.Add(new SqlParameter("@FacilityId", cmbFacility.SelectedValue));
            //// searchParamList.Add(new SqlParameter("@NoOfGuests", txtGuests.Text));
            //searchParamList.Add(new SqlParameter("@BookingProductId", cmbBookingClass.SelectedValue));
            //searchParamList.Add(new SqlParameter("@BookingId", (BookingId == -1 ? -1 : BookingId)));
            // }
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
            //    searchParamList.Add(new SqlParameter("@BookingProductId", (cmbBookingClass.SelectedValue ?? ((Reservation != null && Reservation.GetReservationDTO != null) ? Reservation.GetReservationDTO.BookingProductId : -1))));
            //    searchParamList.Add(new SqlParameter("@BookingId", BookingId));
            //}
            //searchParamList.Add(new SqlParameter("@SiteId", Utilities.ExecutionContext.GetSiteId()));
            //searchParamList.Add(new SqlParameter("@LoginId", Utilities.ExecutionContext.GetUserId()));
            //searchParamList.Add(new SqlParameter("@POSMachineId", Utilities.ExecutionContext.GetMachineId()));

            //log.LogMethodExit(searchParamList);
            //return searchParamList;
        }

        private void SetSearchDateParameters()
        {
            log.LogMethodEntry();
            bool fromDateIsSet = false;
            if (cbxEarlyMorningSlot.Checked)
            {
                fromDateIsSet = true;
                fromTimeForSearch = 0;
                toTimeForSearch = 6;
            }
            if (cbxMorningSlot.Checked)
            {
                if (fromDateIsSet == false)
                {
                    fromDateIsSet = true;
                    fromTimeForSearch = 6;
                }
                toTimeForSearch = 12;
            }
            if (cbxAfternoonSlot.Checked)
            {
                if (fromDateIsSet == false)
                {
                    fromDateIsSet = true;
                    fromTimeForSearch = 12;
                }
                toTimeForSearch = 18;
            }
            if (cbxNightSlot.Checked)
            {
                if (fromDateIsSet == false)
                {
                    fromDateIsSet = true;
                    fromTimeForSearch = 18;
                }
                toTimeForSearch = 24;
            }
            if (fromDateIsSet == false)
            {
                if (cmbSearchFromTime.SelectedValue != null && cmbSearchFromTime.SelectedIndex != 0)
                {
                    fromDateIsSet = true;
                    fromTimeForSearch = Convert.ToDecimal(cmbSearchFromTime.SelectedValue);

                    if (Convert.ToDouble(cmbSearchToTime.SelectedValue) > Convert.ToDouble(cmbSearchFromTime.SelectedValue))
                    {
                        toTimeForSearch = Convert.ToDecimal(cmbSearchToTime.SelectedValue);
                    }
                    else
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 305),
                                                      MessageContainerList.GetMessage(executionContext, "Validation Error"),
                                                      MessageContainerList.GetMessage(executionContext, "From Time"),
                                                      MessageContainerList.GetMessage(executionContext, 305));
                    }
                }
            }
            if (fromDateIsSet == false)
            {
                fromTimeForSearch = utilities.getServerTime().Hour;
                toTimeForSearch = 24;
            }
            log.LogMethodExit();
        }

        //private void GetGracePeriodForFixedScheduleBooking()
        //{
        //    log.LogMethodEntry();
        //    fixedScheduleGracePeriodForBooking = -60;
        //    LookupValuesList lookupValuesListBL = new LookupValuesList(executionContext);
        //    List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
        //    searchParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "BOOKINGS_SETUP"));
        //    searchParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, "FIXED_SCHEDULE_BOOKING_GRACE_PERIOD"));
        //    searchParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, executionContext.GetSiteId().ToString()));
        //    List<LookupValuesDTO> lookupValuesDTOList = lookupValuesListBL.GetAllLookupValues(searchParameters);
        //    if (lookupValuesDTOList != null && lookupValuesDTOList.Count > 0)
        //    {
        //        try
        //        {
        //            fixedScheduleGracePeriodForBooking = Convert.ToInt32(lookupValuesDTOList[0].Description) * -1;
        //        }
        //        catch (Exception ex)
        //        {
        //            log.Error(ex);
        //            fixedScheduleGracePeriodForBooking = -60;
        //        }
        //    }
        //    log.LogMethodExit(fixedScheduleGracePeriodForBooking);
        //}

        private void LoadBookingDetails()
        {
            log.LogMethodEntry();
            if (reservationBL != null && reservationBL.GetReservationDTO != null)
            {
                this.Cursor = Cursors.WaitCursor;
                RefreshSelectedBookingSchedule();
                LoadCustomerNCardDetails();
                LoadPackageDetails();
                LoadAdditionalProductDetails();
                LoadSummaryDetails();
                LoadEventCheckListDropDown();
                LoadCheckListDetails();
                LoadAuditTrail();
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }

        private void ReLoadBookingDetails()
        {
            log.LogMethodEntry();
            this.Cursor = Cursors.WaitCursor;
            try
            {
                POSUtils.SetLastActivityDateTime();
                RefreshSelectedBookingSchedule();
                ResetCustomerNCardDetails();
                ClearNAdjustPnlPackageProducts();
                ClearPnlAdditionalProducts();
                ResetSummaryDetails();
                LoadEventCheckListDropDown();
                LoadCheckListDetails();
                GetAuditTrail();
                POSUtils.SetLastActivityDateTime();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                txtMessage.Text = ex.Message;
            }
            this.Cursor = Cursors.Default;
            log.LogMethodExit();
        }

        private void LoadSchedules()
        {
            log.LogMethodEntry(userAction);
            try
            {
                if (userAction)
                {
                    POSUtils.SetLastActivityDateTime();
                    this.Cursor = Cursors.WaitCursor;
                    ValidateSearchParameters();
                    SetSearchDateParameters();
                    // masterScheduleIdForSearch = (int)cmbSearchSchedulesGroup.SelectedValue;
                    facilityIdForSearch = (int)cmbSearchFacility.SelectedValue;
                    productIdForSearch = (int)cmbSearchBookingProduct.SelectedValue;
                    // guestCountForSearch = (string.IsNullOrEmpty(txtSearchGuestCount.Text) ? 1 : Convert.ToInt32(txtSearchGuestCount.Text));
                    List<ScheduleDetailsDTO> scheduleDetailsDTOList = GetElligileSchedules();
                    POSUtils.SetLastActivityDateTime();
                    dgvSearchFacilityProducts.DataSource = null;
                    dgvSearchSchedules.DataSource = scheduleDetailsDTOList;
                    if (scheduleDetailsDTOList != null && scheduleDetailsDTOList.Count > 0 && dgvSearchSchedules.CurrentRow != null)
                    {
                        UpdateDGVSearchScheduleAvailableUnit();
                        int rowIndex = dgvSearchSchedules.CurrentRow.Index;
                        if (rowIndex >= 0)
                        {
                            POSUtils.SetLastActivityDateTime();
                            DoDGVSearchScheduleRowEnterOperation(rowIndex);
                            POSUtils.SetLastActivityDateTime();
                        }
                    }
                }
            }
            catch (ValidationException ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(ex.GetAllValidationErrorMessages());
            }
            catch (Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 1824, ex.Message));
            }
            this.Cursor = Cursors.Default;
            log.LogMethodExit();
        }

        private void UpdateDGVSearchScheduleAvailableUnit()
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            if (dgvSearchSchedules.Rows.Count > 0)
            {
                for (int i = 0; i < dgvSearchSchedules.Rows.Count; i++)
                {
                    POSUtils.SetLastActivityDateTime();
                    if (dgvSearchSchedules.Rows[i].Cells["facilityMapDTODataGridViewTextBoxColumn"] != null && dgvSearchSchedules.Rows[i].Cells["facilityMapDTODataGridViewTextBoxColumn"].Value != null)
                    {
                        FacilityMapDTO selectedFacilityMapDTORow = (FacilityMapDTO)dgvSearchSchedules.Rows[i].Cells["facilityMapDTODataGridViewTextBoxColumn"].Value;
                        int totalUnits = 0;
                        totalUnits = Convert.ToInt32(dgvSearchSchedules.Rows[i].Cells["totalUnitsDataGridViewTextBoxColumn"].Value);
                        //int scheduleId = Convert.ToInt32(dgvSearchSchedules.Rows[i].Cells["scheduleIdDataGridViewTextBoxColumn"].Value);
                        DateTime scheduleDate = Convert.ToDateTime(dgvSearchSchedules.Rows[i].Cells["scheduleFromDate"].Value);
                        //int ruleUnits = 0;
                        //ruleUnits = Convert.ToInt32(dgvSearchSchedules.Rows[i].Cells["ruleUnitsDataGridViewTextBoxColumn"].Value);
                        int bookedUnits = 0;
                        FacilityMapBL facilityMapBL = new FacilityMapBL(executionContext, selectedFacilityMapDTORow);
                        DateTime scheduleToDate = Convert.ToDateTime(dgvSearchSchedules.Rows[i].Cells["scheduleToDate"].Value);
                        bool fixedSchedule = Convert.ToBoolean(dgvSearchSchedules.Rows[i].Cells["fixedScheduleDataGridViewCheckBoxColumn"].Value);
                        //if (fixedSchedule == false)
                        //{
                        //    decimal fromTime;
                        //    if (cmbSearchFromTime.SelectedValue == null 
                        //        || decimal.TryParse(cmbSearchFromTime.SelectedValue.ToString(), out fromTime) == false)
                        //    {
                        //        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2091));//"Please enter valid time in Search From Time field"
                        //    }
                        //    decimal toTime;
                        //    if (cmbSearchToTime.SelectedValue == null 
                        //        || decimal.TryParse(cmbSearchToTime.SelectedValue.ToString(), out toTime) == false)
                        //    {
                        //        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2092));// "Please enter valid time in Search To Time field"
                        //    }
                        //    scheduleDate = scheduleDate.Date.AddMinutes((int)fromTime * 60 + (double)fromTime % 1 * 100);
                        //    scheduleToDate = scheduleDate.Date.AddMinutes((int)toTime * 60 + (double)toTime % 1 * 100);
                        //}
                        if (fixedSchedule)
                        {
                            POSUtils.SetLastActivityDateTime();
                            bookedUnits = facilityMapBL.GetTotalBookedUnitsForReservation(scheduleDate, scheduleToDate);
                        }
                        int availableUnits = totalUnits - bookedUnits;
                        dgvSearchSchedules.Rows[i].Cells["availableUnitsDataGridViewTextBoxColumn"].Value = availableUnits;
                        //LoadFacilityProducts(selectedFacilityMapDTORow, availableUnits, bookedUnits, ruleUnits > 0);
                    }
                }
            }
            log.LogMethodExit();
        }

        private List<ScheduleDetailsDTO> GetElligileSchedules()
        {
            //log.LogMethodEntry(masterScheduleIdForSearch, facilityIdForSearch, productIdForSearch);
            log.LogMethodEntry(facilityIdForSearch, productIdForSearch);
            POSUtils.SetLastActivityDateTime();
            List<ScheduleDetailsDTO> scheduleDetailsDTOList = new List<ScheduleDetailsDTO>();
            scheduleDetailsDTOList = ((Parafait_POS.POS)Application.OpenForms["POS"]).GetEligibleSchedules(dtpSearchDate.Value.Date, fromTimeForSearch, toTimeForSearch, facilityIdForSearch, productIdForSearch, -1);
            POSUtils.SetLastActivityDateTime();
            if (scheduleDetailsDTOList != null && scheduleDetailsDTOList.Count > 0)
            {
                for (int i = 0; i < scheduleDetailsDTOList.Count; i++)
                {
                    if (scheduleDetailsDTOList[i].ScheduleFromDate < utilities.getServerTime().AddMinutes(-this.reservationDefaultSetup.GetGracePeriodForFixedSchedule) && scheduleDetailsDTOList[i].FixedSchedule == true
                        || scheduleDetailsDTOList[i].ScheduleFromDate < utilities.getServerTime().Date && scheduleDetailsDTOList[i].FixedSchedule == false)
                    {
                        scheduleDetailsDTOList.RemoveAt(i);
                        i = -1;
                    }
                }
            }

            if (scheduleDetailsDTOList != null && scheduleDetailsDTOList.Count > 0)
            {
                for (int i = 0; i < scheduleDetailsDTOList.Count; i++)
                {
                    double Price = (scheduleDetailsDTOList[i].Price == null ? 0 : (double)scheduleDetailsDTOList[i].Price);
                    if (Price == 0)
                    {
                        Price = (scheduleDetailsDTOList[i].AttractionPlayPrice == null ? 0 : (double)scheduleDetailsDTOList[i].AttractionPlayPrice);
                    }

                    scheduleDetailsDTOList[i].Price = Price;

                    int? totalUnits = 0;
                    totalUnits = scheduleDetailsDTOList[i].TotalUnits;

                    POSUtils.SetLastActivityDateTime();
                    int bookedUnits = 0;
                    FacilityMapBL facilityMapBL = new FacilityMapBL(executionContext, scheduleDetailsDTOList[i].FacilityMapDTO);
                    FacilityMapDTO currentFMapDTO = (facilityMapDTOList != null && facilityMapDTOList.Any()
                                                     ? facilityMapDTOList.Find(fm => fm.FacilityMapId == scheduleDetailsDTOList[i].FacilityMapDTO.FacilityMapId) : null);
                    bool hideBookedSlotForFac = currentFMapDTO != null
                                                  && currentFMapDTO.FacilityMapDetailsDTOList != null
                                                  && currentFMapDTO.FacilityMapDetailsDTOList.Any()
                                                  && currentFMapDTO.FacilityMapDetailsDTOList.Exists(fmd => fmd.IsActive && fmd.FacilityDTOList != null && fmd.FacilityDTOList.Any()
                                                                                                            && fmd.FacilityDTOList.Exists(fac => fac.AllowMultipleBookings == false
                                                                                                                                               && fac.ActiveFlag));
                    if (scheduleDetailsDTOList[i].FixedSchedule)
                    {
                        POSUtils.SetLastActivityDateTime();
                        bookedUnits = facilityMapBL.GetTotalBookedUnitsForReservation(scheduleDetailsDTOList[i].ScheduleFromDate, scheduleDetailsDTOList[i].ScheduleToDate);
                        scheduleDetailsDTOList[i].AvailableUnits = totalUnits - bookedUnits;
                    }
                    else
                    {
                        //decimal fromTime;
                        //if (cmbSearchFromTime.SelectedValue == null 
                        //    || decimal.TryParse(cmbSearchFromTime.SelectedValue.ToString(), out fromTime) == false)
                        //{
                        //    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2091)); //Please enter valid time in Search From Time field
                        //}
                        //decimal toTime;
                        //if (cmbSearchToTime.SelectedValue == null 
                        //    || decimal.TryParse(cmbSearchToTime.SelectedValue.ToString(), out toTime) == false)
                        //{
                        //    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2092)); //Please enter valid time in Search To Time field
                        //}

                        //DateTime scheduleFromDate = scheduleDetailsDTOList[i].ScheduleFromDate.Date.AddMinutes((int)fromTime * 60 + (double)fromTime % 1 * 100);
                        //DateTime scheduleToDate = scheduleDetailsDTOList[i].ScheduleFromDate.Date.AddMinutes((int)toTime * 60 + (double)toTime % 1 * 100);

                        //bookedUnits = facilityMapBL.GetTotalBookedUnitsForBookings(scheduleFromDate, scheduleToDate);
                        scheduleDetailsDTOList[i].AvailableUnits = totalUnits - bookedUnits;
                    }
                    if (scheduleDetailsDTOList[i].AvailableUnits < 1 || (hideBookedSlotForFac && BookingIsInEditMode() && cbxHideBookedSlots.Checked && bookedUnits > 0))
                    {
                        scheduleDetailsDTOList.RemoveAt(i);
                        i = -1;
                    }
                }
            }
            //if (scheduleDetailsDTOList != null && scheduleDetailsDTOList.Any())
            //{
            //    scheduleDetailsDTOList = scheduleDetailsDTOList.OrderBy(sch => sch.ScheduleFromDate).ToList();
            //} 
            log.LogMethodExit(scheduleDetailsDTOList);
            return scheduleDetailsDTOList;
        }

        private void frmReservationUI_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            UpdateUIElements();
            LoadSchedules();
            LoadBookingDetails();
            if (BookingIsInEditMode() == false)
            {
                tcBooking.SelectedTab = tpSummary;
            }
            utilities.setLanguage(this);
            StartInActivityTimeoutTimer();
            log.LogMethodExit();
        }

        private void btnPrevAuditTrail_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            POSUtils.SetLastActivityDateTime();
            tcBooking.SelectedTab = tpCheckList;
            this.Cursor = Cursors.Default;
            log.LogMethodExit();
        }

        private void btnNextCheckList_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            POSUtils.SetLastActivityDateTime();
            tcBooking.SelectedTab = tpAuditDetails;
            this.Cursor = Cursors.Default;
            log.LogMethodExit();
        }

        private void btnSaveCheckList_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                POSUtils.SetLastActivityDateTime();
                this.Cursor = Cursors.WaitCursor;
                txtMessage.Clear();
                if (dgvUserJobTaskList.Rows.Count > 0)
                {
                    List<UserJobItemsDTO> modifiedCheckListItems = ((List<UserJobItemsDTO>)dgvUserJobTaskList.DataSource);
                    //if (modifiedCheckListItems != null && dgvUserJobTaskList.Rows.Count > 0)
                    {
                        string editedBy = executionContext.GetUserId();
                        string editedUser = "";
                        int editedByUserPKId = executionContext.GetUserPKId();
                        LookupValuesDTO closedStatusDTO = checkListStatusList.Find(cList => cList.LookupValue == "Closed");
                        //if (editedByUserPKId == -1)
                        {
                            Users currentUserId = new Users(executionContext, editedBy);
                            editedUser = currentUserId.UserDTO.UserName;
                            editedByUserPKId = currentUserId.UserDTO.UserId;
                            executionContext.SetUserPKId(editedByUserPKId);
                        }
                        foreach (UserJobItemsDTO currentRecord in modifiedCheckListItems)
                        {
                            POSUtils.SetLastActivityDateTime();
                            if (currentRecord != null && currentRecord.IsChanged == true && currentRecord.MaintChklstdetId > -1)
                            {

                                if (currentRecord.AssignedUserId != editedByUserPKId)
                                {
                                    currentRecord.AssignedUserId = editedByUserPKId;
                                    currentRecord.AssignedTo = editedUser;
                                }
                                if (currentRecord.Status == closedStatusDTO.LookupValueId && string.IsNullOrEmpty(currentRecord.ChecklistCloseDate))
                                {
                                    currentRecord.ChecklistCloseDate = utilities.getServerTime().ToString("yyyy-MM-dd HH:mm:ss");
                                }
                                else if (currentRecord.Status != closedStatusDTO.LookupValueId && string.IsNullOrEmpty(currentRecord.ChecklistCloseDate) == false)
                                {
                                    currentRecord.ChecklistCloseDate = null;
                                }
                                UserJobItemsBL userJobItemsBL = new UserJobItemsBL(executionContext, currentRecord);
                                userJobItemsBL.Save(null);
                            }
                        }
                        LoadCheckListDetails();
                    }
                }
            }
            catch (ValidationException ex)
            {
                log.Error(ex);
                txtMessage.Text = ex.GetAllValidationErrorMessages();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                txtMessage.Text = MessageContainerList.GetMessage(executionContext, 1824, ex.Message);
            }
            this.Cursor = Cursors.Default;
            log.LogMethodExit();
        }

        private void btnPrevCheckList_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            POSUtils.SetLastActivityDateTime();
            tcBooking.SelectedTab = tpSummary;
            this.Cursor = Cursors.Default;
            log.LogMethodExit();
        }

        private void btnPrintCheckList_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            POSUtils.SetLastActivityDateTime();
            PrintChecklist();
            this.Cursor = Cursors.Default;
            log.LogMethodExit();
        }


        private void btnPrevConfirm_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            POSUtils.SetLastActivityDateTime();
            tcBooking.SelectedTab = tpAdditionalProducts;
            this.Cursor = Cursors.Default;
            log.LogMethodExit();
        }

        private void btnBook_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            POSUtils.SetLastActivityDateTime();
            bool calledSaveMethod = false;
            bool savedSuccesfully = false;
            string reservationStatusBeforeBooking = string.Empty;
            int bookingId = -1;
            try
            {
                this.Cursor = Cursors.WaitCursor;
                txtMessage.Clear();
                if (BookingIsInEditMode())
                {
                    POSUtils.SetLastActivityDateTime();
                    CheckForInitialMandatoryFields();
                    SetRemarks();
                    SetServiceCharges();
                    bookingId = this.reservationBL.GetReservationDTO.BookingId;
                    reservationStatusBeforeBooking = this.reservationBL.GetReservationDTO.Status;
                    calledSaveMethod = true;
                    //reservationBL.BookReservation(null);
                    this.Cursor = Cursors.WaitCursor;
                    try
                    {
                        POSUtils.SetLastActivityDateTime();
                        StopInActivityTimeoutTimer();
                        string msg = MessageContainerList.GetMessage(executionContext, "Saving reservation.") + " " +
                                      MessageContainerList.GetMessage(executionContext, 684);// "Please wait..."
                        bool valid = BackgroundProcessRunner.Run<bool>(() => { return InvokeBookReservation(); }, msg, BackgroundProcessRunner.LaunchWaitScreenAfterXSeconds);
                    }
                    finally
                    {
                        StartInActivityTimeoutTimer();
                    }
                    this.Cursor = Cursors.WaitCursor;
                    savedSuccesfully = true;
                }
            }
            catch (ValidationException ex)
            {
                log.Error(ex);
                txtMessage.Text = ex.GetAllValidationErrorMessages();
                POSUtils.ParafaitMessageBox(ex.GetAllValidationErrorMessages(), MessageContainerList.GetMessage(executionContext, "Save Booking"));
                if (calledSaveMethod)
                {
                    POSUtils.SetLastActivityDateTime();
                    RefreshDataForErroredReservation(bookingId);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                txtMessage.Text = ex.Message;
                if (calledSaveMethod)
                {
                    POSUtils.SetLastActivityDateTime();
                    RefreshDataForErroredReservation(bookingId);
                }
            }
            if (bookingId == -1 && calledSaveMethod && savedSuccesfully == false)
            {
                POSUtils.SetLastActivityDateTime();
                UpdateUIElements();
                ReLoadBookingDetails();
                tcBooking.SelectedTab = tpDateTime;
            }
            else
            {
                LoadSummaryDetails();
                UpdateUIElements();
            }
            this.Cursor = Cursors.Default;
            if (savedSuccesfully)
            {
                POSUtils.SetLastActivityDateTime();
                txtMessage.Text = MessageContainerList.GetMessage(executionContext, "BOOKED");
                if (reservationStatusBeforeBooking == ReservationDTO.ReservationStatus.WIP.ToString()
                    && this.reservationBL.GetReservationDTO.Status == ReservationDTO.ReservationStatus.BOOKED.ToString())
                {
                    ConfirmBooking();
                }
            }
            SetFormOnFocus();
            log.LogMethodExit();
        }
        private bool InvokeBookReservation()
        {
            log.LogMethodEntry();
            reservationBL.BookReservation(null);
            log.LogMethodExit();
            return true;
        }

        private void RefreshDataForErroredReservation(int bookingId)
        {
            log.LogMethodEntry(bookingId);
            //Restore old data 
            if (this.reservationBL != null)
            {
                POSUtils.SetLastActivityDateTime();
                this.reservationBL.ClearUnSavedSchedules();
                if (bookingId > -1)
                {
                    //this.reservationBL = new ReservationBL(executionContext, utilities, bookingId);
                    txtMessage.Text = MessageContainerList.GetMessage(executionContext, 2136);//"Unexpected error while saving the changes. New changes are reverted. Please redo and save the changes"
                    POSUtils.ParafaitMessageBox(txtMessage.Text);
                }
                else
                {
                    //this.reservationBL = new ReservationBL()
                    txtMessage.Text = MessageContainerList.GetMessage(executionContext, "Sorry, unexpected error while saving the changes, please create new reservation to proceed");// "Unexpected error while saving the changes. Only booking product and schedule are retained. Please redo remaining sections and save the changes"
                    POSUtils.ParafaitMessageBox(txtMessage.Text);
                }
                POSUtils.SetLastActivityDateTime();
                LoadReservationBL(bookingId);
            }
            log.LogMethodExit();
        }

        private void SetRemarks()
        {
            log.LogMethodEntry();
            if (BookingIsInEditMode() && reservationBL.ReservationTransactionIsNotNull())
            {
                if (string.IsNullOrEmpty(txtRemarks.Text) == false && txtRemarks.Text != reservationBL.GetReservationDTO.Remarks)
                {
                    POSUtils.SetLastActivityDateTime();
                    ValidateBookingRemarks();
                    reservationBL.GetReservationDTO.Remarks = txtRemarks.Text;
                }
            }
            log.LogMethodExit();
        }

        private void ValidateBookingRemarks()
        {
            log.LogMethodEntry();
            string specialChars = @"[-+=@]";
            if (BookingIsInEditMode() && string.IsNullOrEmpty(txtRemarks.Text.Trim()) == false && Regex.IsMatch(txtRemarks.Text.Substring(0, 1), specialChars))
            {
                //txtRemarks.Text = string.Empty;
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2265, MessageContainerList.GetMessage(executionContext, "Booking remarks"), specialChars));
            }
            log.LogMethodExit();
        }

        private void UpdateUIElements()
        {
            log.LogMethodEntry();
            this.Cursor = Cursors.WaitCursor;
            SetUIElementToDefaultStatus();

            if (BookingIsInEditMode())
            {
                SetUIElementsToEditMode();
            }
            else if (this.reservationBL.GetReservationDTO != null
                   && (this.reservationBL.GetReservationDTO.Status == ReservationDTO.ReservationStatus.BOOKED.ToString()
                      || this.reservationBL.GetReservationDTO.Status == ReservationDTO.ReservationStatus.CONFIRMED.ToString())
                )
            {
                SetUIElementsForBookedConfirmedStatus();
            }
            else if (this.reservationBL.GetReservationDTO != null
                   && (this.reservationBL.GetReservationDTO.Status == ReservationDTO.ReservationStatus.COMPLETE.ToString()
                      || this.reservationBL.GetReservationDTO.Status == ReservationDTO.ReservationStatus.CANCELLED.ToString())
                )
            {
                btnSendPaymentLink.Enabled = false;
                btnPrintBooking.Enabled = btnSendEmail.Enabled = true;
                if (this.reservationBL.GetReservationDTO.Status == ReservationDTO.ReservationStatus.COMPLETE.ToString())
                {
                    btnSendPaymentLink.Enabled = enablePaymentLinkButton;
                    btnPayment.Enabled = true;
                    btnSaveCheckList.Enabled = btnPrintCheckList.Enabled = true;
                    dgvUserJobTaskList.Enabled = true;
                }
            }
            UpdateUserControlUIElements();
            this.Cursor = Cursors.Default;
            log.LogMethodExit();
        }

        private void UpdateUserControlUIElements()
        {
            log.LogMethodEntry();
            this.Cursor = Cursors.WaitCursor;
            ReservationDTO.ReservationStatus reservationStatus = ReservationDTO.ReservationStatus.CANCELLED;
            if (this.reservationBL.GetReservationDTO != null)
            {
                if (Enum.TryParse(this.reservationBL.GetReservationDTO.Status, out reservationStatus) == false)
                {
                    reservationStatus = ReservationDTO.ReservationStatus.CANCELLED;
                }
            }
            this.usrCtrlPkgProductDetails1.Enabled = false;
            foreach (Control controlItem in pnlPackageProducts.Controls)
            {
                if (controlItem.Name.StartsWith("usrCtrlPkgProductDetails0"))
                {
                    usrCtrlProductDetails usrCtrlProductDetails = (usrCtrlProductDetails)controlItem;
                    usrCtrlProductDetails.UpdateUIElements(reservationStatus);
                }
            }
            if (pnlPackageProducts != null)
            {
                packageListVScrollBar.Location = new Point(pnlPackageProducts.Location.X + pnlPackageProducts.Width - 20, packageListVScrollBar.Location.Y);
            }
            foreach (Control controlItem in pnlAdditionalProducts.Controls)
            {
                if (controlItem.Name.StartsWith("usrCtrlAdditionProductDetails"))
                {
                    usrCtrlProductDetails usrCtrlProductDetails = (usrCtrlProductDetails)controlItem;
                    usrCtrlProductDetails.UpdateUIElements(reservationStatus);
                }
            }
            this.Cursor = Cursors.Default;
            log.LogMethodExit();
        }

        private void SetUIElementToDefaultStatus()
        {
            log.LogMethodEntry();

            dtpSearchDate.Enabled = cbxEarlyMorningSlot.Enabled = cbxMorningSlot.Enabled = cbxAfternoonSlot.Enabled = cbxNightSlot.Enabled = false;
            cmbSearchFromTime.Enabled = cmbSearchToTime.Enabled = cmbSearchFacility.Enabled = cmbSearchBookingProduct.Enabled = false;
            //cmbSearchSchedulesGroup.Enabled = false;
            //btnReduceGuestCount.Enabled = txtSearchGuestCount.Enabled = btnIncreaseGuestCount.Enabled = false;
            dgvSearchSchedules.Enabled = dgvSearchFacilityProducts.Enabled = dgvSelectedBookingSchedule.Enabled = dgvAuditTrail.Enabled = dgvUserJobTaskList.Enabled = false;
            btnAddToBooking.Enabled = cmbChannel.Enabled = btnBlockSchedule.Enabled = false;
            pnlCustomerDetail.Enabled = btnCustomerLookup.Enabled = false;
            txtRemarks.Enabled = false;
            //pcbRemoveServiceCharge.Enabled = txtServiceChargeAmount.Enabled = txtServiceChargePercentage.Enabled = false;
            pcbRemoveDiscCoupon.Enabled = pcbRemoveDiscount.Enabled = btnApplyDiscCoupon.Enabled = btnApplyDiscount.Enabled = btnAddTransactionProfile.Enabled = false;
            btnSendEmail.Enabled = btnPrintBooking.Enabled = btnPayment.Enabled = false;
            btnSendPaymentLink.Enabled = enablePaymentLinkButton;
            btnAddAttendees.Enabled = true;
            btnConfirm.Enabled = btnExecute.Enabled = btnBook.Enabled = btnEditBooking.Enabled = btnCancelBooking.Enabled = false;
            btnMapWaivers.Enabled = true;
            btnSaveCheckList.Enabled = btnPrintCheckList.Enabled = false;
            cbxEmailSent.Enabled = false;
            txtCardNumber.Enabled = false;
            cbxHideBookedSlots.Enabled = false;
            EnableDisableChargeButtons();
            log.LogMethodExit();
        }

        private void SetUIElementsToEditMode()
        {
            log.LogMethodEntry();
            dtpSearchDate.Enabled = cbxEarlyMorningSlot.Enabled = cbxMorningSlot.Enabled = cbxAfternoonSlot.Enabled = cbxNightSlot.Enabled = true;
            cmbSearchFromTime.Enabled = cmbSearchToTime.Enabled = cmbSearchFacility.Enabled = cmbSearchBookingProduct.Enabled = true;
            dgvSearchSchedules.Enabled = dgvSearchFacilityProducts.Enabled = dgvSelectedBookingSchedule.Enabled = true;
            btnAddToBooking.Enabled = cmbChannel.Enabled = btnBlockSchedule.Enabled = true;
            pnlCustomerDetail.Enabled = btnCustomerLookup.Enabled = true;
            txtRemarks.Enabled = true;
            //pcbRemoveServiceCharge.Enabled = txtServiceChargeAmount.Enabled = txtServiceChargePercentage.Enabled = true;
            pcbRemoveDiscCoupon.Enabled = pcbRemoveDiscount.Enabled = btnApplyDiscCoupon.Enabled = btnApplyDiscount.Enabled = btnAddTransactionProfile.Enabled = true;
            btnSendPaymentLink.Enabled = false;
            btnAddAttendees.Enabled = true;
            btnBook.Enabled = true;
            if (this.reservationBL.GetReservationDTO != null
                && (this.reservationBL.GetReservationDTO.Status == ReservationDTO.ReservationStatus.BLOCKED.ToString()
                    || this.reservationBL.GetReservationDTO.Status == ReservationDTO.ReservationStatus.WIP.ToString()))
            {
                btnCancelBooking.Enabled = true;
            }
            btnMapWaivers.Enabled = false;
            cbxHideBookedSlots.Enabled = true;
            log.LogMethodExit();
        }

        private void SetUIElementsForBookedConfirmedStatus()
        {
            log.LogMethodEntry();
            btnSendPaymentLink.Enabled = enablePaymentLinkButton;
            btnSendEmail.Enabled = btnPrintBooking.Enabled = btnPayment.Enabled = true;
            btnEditBooking.Enabled = btnCancelBooking.Enabled = true;
            dgvUserJobTaskList.Enabled = true;
            btnSaveCheckList.Enabled = btnPrintCheckList.Enabled = true;
            if (this.reservationBL.GetReservationDTO.Status == ReservationDTO.ReservationStatus.CONFIRMED.ToString())
            {
                btnExecute.Enabled = true;
            }
            if (this.reservationBL.GetReservationDTO.Status == ReservationDTO.ReservationStatus.BOOKED.ToString())
            {
                btnConfirm.Enabled = true;
            }
            log.LogMethodExit();
        }
        private void btnEditBooking_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                POSUtils.SetLastActivityDateTime();
                this.Cursor = Cursors.WaitCursor;
                txtMessage.Clear();
                if (reservationBL.ReservationTransactionIsNotNull() &&
                    (reservationBL.GetReservationDTO.Status == ReservationDTO.ReservationStatus.BOOKED.ToString() ||
                    reservationBL.GetReservationDTO.Status == ReservationDTO.ReservationStatus.CONFIRMED.ToString()))
                {
                    if (POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 925), "Edit Booking", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    {
                        POSUtils.SetLastActivityDateTime();
                        this.Cursor = Cursors.WaitCursor;
                        log.Info("edit booking enabled");
                        int bookingId = reservationBL.GetReservationDTO.BookingId;
                        bool editSaveIsSuccessful = false;
                        try
                        {
                            //reservationBL.EditReservation(null);
                            this.Cursor = Cursors.WaitCursor;
                            try
                            {
                                POSUtils.SetLastActivityDateTime();
                                StopInActivityTimeoutTimer();
                                string msg = MessageContainerList.GetMessage(executionContext, "Opening reservation for editing.") + " " +
                                              MessageContainerList.GetMessage(executionContext, 684);// "Please wait..."
                                bool valid = BackgroundProcessRunner.Run<bool>(() => { return InvokeEditReservation(); }, msg, BackgroundProcessRunner.LaunchWaitScreenAfterXSeconds);
                            }
                            finally
                            {
                                StartInActivityTimeoutTimer();
                            }
                            this.Cursor = Cursors.WaitCursor;
                            UpdateUIElements();
                            tcBooking.SelectedTab = tpDateTime;
                            editSaveIsSuccessful = true;
                        }
                        catch (ValidationException ex)
                        {
                            log.Error(ex);
                            txtMessage.Text = ex.GetAllValidationErrorMessages();
                            POSUtils.ParafaitMessageBox(txtMessage.Text, MessageContainerList.GetMessage(executionContext, "Edit Booking"));
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                            txtMessage.Text = ex.Message;
                            POSUtils.ParafaitMessageBox(txtMessage.Text, MessageContainerList.GetMessage(executionContext, "Edit Booking"));
                        }
                        if (editSaveIsSuccessful == false)
                        {
                            this.Cursor = Cursors.WaitCursor;
                            LoadReservationBL(bookingId);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                txtMessage.Text = ex.Message;
                POSUtils.ParafaitMessageBox(txtMessage.Text, MessageContainerList.GetMessage(executionContext, "Edit Booking"));
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }
        private bool InvokeEditReservation()
        {
            log.LogMethodEntry();
            reservationBL.EditReservation(null);
            log.LogMethodExit();
            return true;
        }
        private void btnCancelBooking_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                POSUtils.SetLastActivityDateTime();
                this.Cursor = Cursors.WaitCursor;
                txtMessage.Clear();
                if (this.reservationBL.GetReservationDTO != null
                    && (this.reservationBL.GetReservationDTO.Status == ReservationDTO.ReservationStatus.BLOCKED.ToString()
                        || this.reservationBL.GetReservationDTO.Status == ReservationDTO.ReservationStatus.BOOKED.ToString()
                        || this.reservationBL.GetReservationDTO.Status == ReservationDTO.ReservationStatus.CONFIRMED.ToString()))

                {
                    if (POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 310), MessageContainerList.GetMessage(executionContext, "Cancel"), MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    {
                        POSUtils.SetLastActivityDateTime();
                        this.Cursor = Cursors.WaitCursor;
                        if (this.reservationBL.BookingTransaction != null && this.reservationBL.BookingTransaction.TotalPaidAmount > 0)
                        {
                            try
                            {
                                bool cancellationChargeReceived = this.reservationBL.HasCancellationChargeReceived(null);
                                if (cancellationChargeReceived == false)
                                {
                                    POSUtils.SetLastActivityDateTime();
                                    using (frmReservationCancellationCharge cancellationChargesUI = new frmReservationCancellationCharge(reservationBL))
                                    {
                                        if (cancellationChargesUI.ShowDialog() == DialogResult.OK)
                                        {
                                            this.Cursor = Cursors.WaitCursor;
                                            POSUtils.SetLastActivityDateTime();
                                            reservationBL = cancellationChargesUI.GetReservationBL;
                                            reservationBL.CancelReservation(null);
                                            POSUtils.SetLastActivityDateTime();
                                            if (!ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "DEFAULT_PAY_MODE").Equals("1"))
                                            {
                                                PaymentDetails paymentDetails = new PaymentDetails(this.reservationBL.BookingTransaction);
                                                while (this.reservationBL.BookingTransaction.Net_Transaction_Amount != this.reservationBL.BookingTransaction.TotalPaidAmount)
                                                {
                                                    paymentDetails.ShowDialog();
                                                    this.reservationBL.BookingTransaction.getTotalPaidAmount(null);
                                                    this.reservationBL.BookingTransaction.updateAmounts(true, null);
                                                }
                                            }
                                            POSUtils.SetLastActivityDateTime();
                                            ParafaitDBTransaction dBTransaction = new ParafaitDBTransaction();
                                            try
                                            {
                                                StopInActivityTimeoutTimer();
                                                dBTransaction.BeginTransaction();
                                                reservationBL.SaveCancelledReservation(dBTransaction.SQLTrx);
                                                dBTransaction.EndTransaction();
                                                dBTransaction.Dispose();
                                            }
                                            catch (Exception ex)
                                            {
                                                log.Error(ex);
                                                if (dBTransaction != null)
                                                {
                                                    dBTransaction.RollBack();
                                                    dBTransaction.Dispose();
                                                }
                                                throw;
                                            }
                                            finally
                                            {
                                                POSUtils.SetLastActivityDateTime();
                                                StartInActivityTimeoutTimer();
                                            }
                                            LoadSummaryDetails();
                                            UpdateUIElements();
                                            txtMessage.Text = MessageContainerList.GetMessage(executionContext, "CANCELLED");
                                        }
                                        else
                                        {
                                            txtMessage.Text = MessageContainerList.GetMessage(executionContext, 2093);//Please settle the Cancellation Charges to proceed with Cancellation
                                        }
                                    }
                                }
                                else
                                {
                                    POSUtils.SetLastActivityDateTime();
                                    POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 525), MessageContainerList.GetMessage(executionContext, "Payment Validation"));
                                    log.LogMethodExit("Delete payments before booking cancellation");
                                    btnPayment.PerformClick();
                                    this.Cursor = Cursors.Default;
                                    return;
                                }
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex);
                                txtMessage.Text = MessageContainerList.GetMessage(executionContext, 1824, ex.Message);
                                throw;
                            }

                        }
                        else if (this.reservationBL.BookingTransaction != null && this.reservationBL.BookingTransaction.TotalPaidAmount == 0)
                        {
                            POSUtils.SetLastActivityDateTime();
                            ParafaitDBTransaction dBTransaction = new ParafaitDBTransaction();
                            try
                            {
                                StopInActivityTimeoutTimer();
                                dBTransaction.BeginTransaction();
                                reservationBL.CancelReservation(dBTransaction.SQLTrx);
                                reservationBL.SaveCancelledReservation(dBTransaction.SQLTrx);
                                dBTransaction.EndTransaction();
                                dBTransaction.Dispose();
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex);
                                if (dBTransaction != null)
                                {
                                    dBTransaction.RollBack();
                                    dBTransaction.Dispose();
                                }
                                throw;
                            }
                            finally
                            {
                                POSUtils.SetLastActivityDateTime();
                                StartInActivityTimeoutTimer();
                            }
                            LoadSummaryDetails();
                            UpdateUIElements();
                            txtMessage.Text = MessageContainerList.GetMessage(executionContext, "CANCELLED");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                txtMessage.Text = ex.Message;
                POSUtils.ParafaitMessageBox(txtMessage.Text, MessageContainerList.GetMessage(executionContext, "Cancel Booking"));
                POSUtils.SetLastActivityDateTime();
                ReloadBooking(MessageContainerList.GetMessage(executionContext, "Cancel Booking"));
            }
            SetFormOnFocus();
            this.Cursor = Cursors.Default;
            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            POSUtils.SetLastActivityDateTime();
            this.Close();
            log.LogMethodExit();
        }

        private void btnNextSummary_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            POSUtils.SetLastActivityDateTime();
            txtMessage.Clear();
            tcBooking.SelectedTab = tpCheckList;
            this.Cursor = Cursors.Default;
            log.LogMethodExit();
        }

        private void btnPrevAdditionalProducts_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            POSUtils.SetLastActivityDateTime();
            txtMessage.Clear();
            tcBooking.SelectedTab = tpPackageProducts;
            this.Cursor = Cursors.Default;
            log.LogMethodExit();
        }

        private void btnNextAdditionalProducts_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            POSUtils.SetLastActivityDateTime();
            txtMessage.Clear();
            MoveToSummaryTab();
            log.LogMethodExit();
        }

        private void MoveToSummaryTab()
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            tcBooking.SelectedTab = tpSummary;
            this.Cursor = Cursors.Default;
            log.LogMethodExit();
        }

        private void btnPrevPackage_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            POSUtils.SetLastActivityDateTime();
            txtMessage.Clear();
            tcBooking.SelectedTab = tpCustomer;
            this.Cursor = Cursors.Default;
            log.LogMethodExit();
        }

        private void btnNextPackage_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            POSUtils.SetLastActivityDateTime();
            txtMessage.Clear();
            if (BookingIsInEditMode())
            {
                try
                {
                    ValidateBookingRemarks();
                    reservationBL.HasPackageProducts();
                    tcBooking.SelectedTab = tpAdditionalProducts;
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    txtMessage.Text = ex.Message;
                }
            }
            else
            {
                tcBooking.SelectedTab = tpAdditionalProducts;
            }
            POSUtils.SetLastActivityDateTime();
            this.Cursor = Cursors.Default;
            log.LogMethodExit();
        }

        private void btnNextCustomer_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            POSUtils.SetLastActivityDateTime();
            txtMessage.Clear();
            try
            {
                ValidateCustomerDetails();
                tcBooking.SelectedTab = tpPackageProducts;
            }
            catch (ValidationException ex)
            {
                log.Error(ex);
                txtMessage.Text = ex.GetAllValidationErrorMessages();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                txtMessage.Text = ex.Message;
            }
            POSUtils.SetLastActivityDateTime();
            this.Cursor = Cursors.Default;
            log.LogMethodExit();
        }

        private void ValidateCustomerDetails()
        {
            log.LogMethodEntry();
            if (BookingIsInEditMode())
            {
                if (ValidateCustomer())
                {
                    SetTrxCustomerDTO();
                }
                reservationBL.HasValidCustomer();
            }
            log.LogMethodExit();
        }

        private bool ValidateCustomer()
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            bool valid = true;
            List<ValidationError> validationErrorList = customerDetailUI.UpdateCustomerDTO();
            CustomerBL customerBL = new CustomerBL(executionContext, customerDetailUI.CustomerDTO);
            validationErrorList.AddRange(customerBL.Validate());
            if (string.IsNullOrWhiteSpace(customerDetailUI.CustomerDTO.FirstName))
            {
                ValidationError validationError = new ValidationError(MessageContainerList.GetMessage(executionContext, "Customer"), MessageContainerList.GetMessage(executionContext, "First Name"), MessageContainerList.GetMessage(executionContext, 303));
                validationErrorList.Add(validationError);
            }
            if (validationErrorList.Count > 0)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Customer"), validationErrorList);
            }
            log.LogMethodExit(valid);
            return valid;
        }

        private void btnCustomerLookup_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            POSUtils.SetLastActivityDateTime();
            txtMessage.Clear();
            try
            {
                StopInActivityTimeoutTimer();
                using (CustomerLookupUI customerLookupUI = new CustomerLookupUI(utilities,
                                                                         customerDetailUI.FirstName,
                                                                         customerDetailUI.MiddleName,
                                                                         customerDetailUI.LastName,
                                                                         customerDetailUI.Email,
                                                                         customerDetailUI.PhoneNumber,
                                                                         customerDetailUI.UniqueIdentifier))
                {
                    if (customerLookupUI.ShowDialog() == DialogResult.OK)
                    {
                        POSUtils.SetLastActivityDateTime();
                        if (customerLookupUI.SelectedCustomerDTO != null)
                        {
                            customerDetailUI.CustomerDTO = customerLookupUI.SelectedCustomerDTO;
                            SetTrxCustomerDTO();
                        }
                    }
                }
            }
            catch (ValidationException ex)
            {
                log.Error(ex);
                txtMessage.Text = ex.GetAllValidationErrorMessages();
                POSUtils.ParafaitMessageBox(ex.GetAllValidationErrorMessages(), MessageContainerList.GetMessage(executionContext, "Customer Lookup"));
            }
            catch (Exception ex)
            {
                log.Error(ex);
                txtMessage.Text = ex.Message;
            }
            finally
            {
                this.Cursor = Cursors.Default;
                POSUtils.SetLastActivityDateTime();
                StartInActivityTimeoutTimer();
            }
            log.LogMethodExit();
        }

        private void btnPrevCustomer_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                POSUtils.SetLastActivityDateTime();
                txtMessage.Clear();
                tcBooking.SelectedTab = tpDateTime;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            this.Cursor = Cursors.Default;
            log.LogMethodExit();
        }

        private void dtpSearchDate_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                System.Windows.Forms.SendKeys.Send("%{DOWN}");
                POSUtils.SetLastActivityDateTime();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void dtpSearchDate_ValueChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                POSUtils.SetLastActivityDateTime();
                LoadSchedules();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void cbxTimeSlots_CheckedChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                POSUtils.SetLastActivityDateTime();
                SetTimeSearchListValues();
                LoadSchedules();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void cmbSearchFromTimeORToTime_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                POSUtils.SetLastActivityDateTime();
                ResetTimeSlots();
                LoadSchedules();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void SetTimeSearchListValues()
        {
            log.LogMethodEntry();
            if (userAction)
            {
                POSUtils.SetLastActivityDateTime();
                bool fromDateIsSet = false;
                decimal fromTimeSlot = 0;
                decimal toTimeSlot = lastTimeSlotForTheDay;
                decimal currentFromTimeSlot = 0;
                decimal currentToTimeSlot = lastTimeSlotForTheDay;
                if (cmbSearchFromTime.SelectedValue != null)
                {
                    fromTimeSlot = Convert.ToDecimal(cmbSearchFromTime.SelectedValue);
                    currentFromTimeSlot = fromTimeSlot;
                    if (cmbSearchToTime.SelectedValue != null && Convert.ToDouble(cmbSearchToTime.SelectedValue) > Convert.ToDouble(cmbSearchFromTime.SelectedValue))
                    {
                        toTimeSlot = Convert.ToDecimal(cmbSearchToTime.SelectedValue);
                        currentToTimeSlot = toTimeSlot;
                    }
                }
                userAction = false;
                try
                {

                    if (cbxEarlyMorningSlot.Checked)
                    {
                        fromTimeSlot = 0;
                        fromDateIsSet = true;
                        toTimeSlot = 6;
                    }
                    if (cbxMorningSlot.Checked)
                    {
                        if (fromDateIsSet == false)
                        {
                            fromTimeSlot = 6;
                            fromDateIsSet = true;
                        }
                        toTimeSlot = 12;
                    }
                    if (cbxAfternoonSlot.Checked)
                    {
                        if (fromDateIsSet == false)
                        {
                            fromTimeSlot = 12;
                            fromDateIsSet = true;
                        }
                        toTimeSlot = 18;
                    }
                    if (cbxNightSlot.Checked)
                    {
                        if (fromDateIsSet == false)
                        {
                            fromTimeSlot = 18;
                            fromDateIsSet = true;
                        }
                        toTimeSlot = lastTimeSlotForTheDay;
                    }

                    if (fromTimeSlot != currentFromTimeSlot || toTimeSlot != currentToTimeSlot)
                    {
                        cmbSearchFromTime.SelectedValue = fromTimeSlot;
                        cmbSearchToTime.SelectedValue = toTimeSlot;
                    }
                }
                finally
                {
                    POSUtils.SetLastActivityDateTime();
                    userAction = true;
                }
            }
            log.LogMethodExit();

        }
        private void ResetTimeSlots()
        {
            log.LogMethodEntry();
            if (userAction)
            {
                POSUtils.SetLastActivityDateTime();
                userAction = false;
                try
                {
                    if (cbxEarlyMorningSlot.Checked)
                        cbxEarlyMorningSlot.Checked = false;
                    if (cbxMorningSlot.Checked)
                        cbxMorningSlot.Checked = false;
                    if (cbxAfternoonSlot.Checked)
                        cbxAfternoonSlot.Checked = false;
                    if (cbxNightSlot.Checked)
                        cbxNightSlot.Checked = false;
                }
                finally
                {
                    userAction = true;
                }
            }
            log.LogMethodExit();
        }

        private void cmbSearchFacilityProdOrMasterSchedule_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            POSUtils.SetLastActivityDateTime();
            LoadSchedules();
            this.Cursor = Cursors.Default;
            log.LogMethodExit();
        }

        private void cmbSearchBookingProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            POSUtils.SetLastActivityDateTime();
            LoadSchedules();
            this.Cursor = Cursors.Default;
            log.LogMethodExit();
        }

        //private void btnReduceGuestCount_Click(object sender, EventArgs e)
        //{
        //    log.LogMethodEntry(sender, e);
        //    txtMessage.Clear();
        //    int guestCount = ValidateGuestNoEntry();
        //    if (guestCount <= 1)
        //    {
        //        this.Cursor = Cursors.Default;
        //        return;
        //    }
        //    else
        //    {
        //        txtSearchGuestCount.Text = (guestCount - 1).ToString();
        //    }
        //    log.LogMethodExit();
        //}

        //private void btnIncreaseGuestCount_Click(object sender, EventArgs e)
        //{
        //    log.LogMethodEntry(sender, e);
        //    txtMessage.Clear();
        //    int guestCount = ValidateGuestNoEntry();
        //    if (guestCount >= 9999)
        //    {
        //        this.Cursor = Cursors.Default;
        //        return;
        //    }
        //    else
        //    {
        //        txtSearchGuestCount.Text = (guestCount + 1).ToString();
        //    }
        //    log.LogMethodExit();
        //}

        //private int ValidateGuestNoEntry()
        //{
        //    log.LogMethodEntry();
        //    int numberValidation;
        //    if (Int32.TryParse(txtSearchGuestCount.Text, out numberValidation) == false)
        //    {
        //        POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 2094)); //"Please enter valid number"
        //        txtSearchGuestCount.Text = "1";
        //    }
        //    log.LogMethodExit(numberValidation);
        //    this.Cursor = Cursors.Default;
        //    return numberValidation;

        //}

        //private void txtSearchGuestCount_Enter(object sender, EventArgs e)
        //{
        //    log.LogMethodEntry(sender, e);
        //    int guestNo = (int)NumberPadForm.ShowNumberPadForm(MessageContainerList.GetMessage(executionContext, 1834), txtSearchGuestCount.Text, utilities);//"Please enter Guest count"
        //    if (guestNo > 0 && guestNo < 10000)
        //    {
        //        txtSearchGuestCount.Text = guestNo.ToString();
        //    }
        //    this.ActiveControl = btnIncreaseGuestCount;
        //    log.LogMethodExit();
        //}

        //private void txtSearchGuestCount_TextChanged(object sender, EventArgs e)
        //{
        //    log.LogMethodEntry(sender, e);
        //    // LoadSchedules();
        //    log.LogMethodExit();
        //}

        //private void txtSearchGuestCount_Validating(object sender, CancelEventArgs e)
        //{
        //    log.LogMethodEntry(sender, e);
        //    ValidateGuestNoEntry();
        //    txtSearchGuestCount.Focus();
        //    log.LogMethodExit();
        //}

        private void dgvSearchSchedules_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            POSUtils.SetLastActivityDateTime();
            //if (dgvSearchSchedules.CurrentCell != null)
            //{
            //    dgvSearchSchedules.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly = true;
            //}
            log.LogMethodExit();
        }

        private void dgvSearchSchedules_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (dgvSearchSchedules.CurrentCell != null)
            {
                DoDGVSearchScheduleRowEnterOperation(e.RowIndex);
            }
            log.LogMethodExit();
        }

        private void DoDGVSearchScheduleRowEnterOperation(int rowIndex)
        {
            log.LogMethodEntry();
            this.Cursor = Cursors.WaitCursor;
            if (rowIndex >= 0)
            {
                dgvSearchSchedules.Rows[rowIndex].ReadOnly = true;
                if (dgvSearchSchedules.Rows[rowIndex].Cells["facilityMapDTODataGridViewTextBoxColumn"] != null && dgvSearchSchedules.Rows[rowIndex].Cells["facilityMapDTODataGridViewTextBoxColumn"].Value != null)
                {
                    FacilityMapDTO selectedFacilityMapDTORow = (FacilityMapDTO)dgvSearchSchedules.Rows[rowIndex].Cells["facilityMapDTODataGridViewTextBoxColumn"].Value;

                    LoadFacilityProducts(selectedFacilityMapDTORow);
                }
            }
            this.Cursor = Cursors.Default;
            log.LogMethodExit();
        }

        private void LoadFacilityProducts(FacilityMapDTO facilityMapDTO)//, int availableFacilityUnits, int bookedFacilityUnits, bool hasRuleUnit)
        {
            log.LogMethodEntry(facilityMapDTO);
            POSUtils.SetLastActivityDateTime();
            List<ProductsDTO> allowedProductsDTOList = null;
            ProductsDTO defaultRentalProductDTO = null;
            if (facilityMapDTO.ProductsAllowedInFacilityDTOList != null)
            {
                allowedProductsDTOList = new List<ProductsDTO>();

                foreach (ProductsAllowedInFacilityMapDTO item in facilityMapDTO.ProductsAllowedInFacilityDTOList)
                {
                    if (item.ProductsDTO != null && (item.ProductsDTO.ProductType == ProductTypeValues.BOOKINGS || item.ProductsDTO.ProductType == ProductTypeValues.RENTAL))
                    {
                        if (item.DefaultRentalProduct)
                        {
                            defaultRentalProductDTO = item.ProductsDTO;
                        }
                        allowedProductsDTOList.Add(item.ProductsDTO);
                    }
                }
            }
            dgvSearchFacilityProducts.DataSource = allowedProductsDTOList;
            facProdHScrollBar.UpdateButtonStatus();
            if (defaultRentalProductDTO != null)
            {
                foreach (DataGridViewRow productRow in dgvSearchFacilityProducts.Rows)
                {
                    if (Convert.ToInt32(productRow.Cells["productIdDataGridViewTextBoxColumn1"].Value) == defaultRentalProductDTO.ProductId)
                    {
                        productRow.Cells["SelectedRecord"].Value = true;// "True"; 
                    }
                }
            }
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }


        //private ProductsDTO GetLocalProductsDTO(ProductsDTO productsDTO)
        //{
        //    log.LogMethodEntry();
        //    ProductsDTO localProductsDTO = null;
        //    if (productsDTO != null)
        //    {
        //        //copies DTO attributes only. Not the child DTO list etc.
        //        localProductsDTO = new ProductsDTO(productsDTO.ProductId, productsDTO.ProductName, productsDTO.Description, productsDTO.ActiveFlag, productsDTO.ProductTypeId, productsDTO.Price,
        //                                                         productsDTO.Credits, productsDTO.Courtesy, productsDTO.Bonus, productsDTO.Time, productsDTO.SortOrder, productsDTO.Tax_id, productsDTO.Tickets,
        //                                                         productsDTO.FaceValue, productsDTO.DisplayGroup, productsDTO.TicketAllowed, productsDTO.VipCard, productsDTO.LastUpdatedDate, productsDTO.LastUpdatedUser,
        //                                                         productsDTO.InternetKey, productsDTO.TaxInclusivePrice, productsDTO.InventoryProductCode, productsDTO.ExpiryDate, productsDTO.AvailableUnits,
        //                                                         productsDTO.AutoCheckOut, productsDTO.CheckInFacilityId, productsDTO.MaxCheckOutAmount, productsDTO.POSTypeId, productsDTO.CustomDataSetId,
        //                                                         productsDTO.CardTypeId, productsDTO.Guid, productsDTO.SiteId, productsDTO.SynchStatus, productsDTO.TrxRemarksMandatory, productsDTO.CategoryId,
        //                                                         productsDTO.OverridePrintTemplateId, productsDTO.StartDate, productsDTO.ButtonColor, productsDTO.AutoGenerateCardNumber, productsDTO.QuantityPrompt,
        //                                                         productsDTO.OnlyForVIP, productsDTO.AllowPriceOverride, productsDTO.RegisteredCustomerOnly, productsDTO.ManagerApprovalRequired, productsDTO.MinimumUserPrice,
        //                                                         productsDTO.TextColor, productsDTO.Font, productsDTO.VerifiedCustomerOnly, productsDTO.Modifier, productsDTO.MinimumQuantity, productsDTO.DisplayInPOS,
        //                                                         productsDTO.TrxHeaderRemarksMandatory, productsDTO.CardCount, productsDTO.ImageFileName, productsDTO.AdvancePercentage, productsDTO.AdvanceAmount,
        //                                                         productsDTO.EmailTemplateId, productsDTO.MaximumTime, productsDTO.MinimumTime, productsDTO.CardValidFor, productsDTO.AdditionalTaxInclusive, productsDTO.AdditionalPrice,
        //                                                         productsDTO.AdditionalTaxId, productsDTO.MasterEntityId, productsDTO.WaiverRequired, productsDTO.SegmentCategoryId, productsDTO.CardExpiryDate,
        //                                                         productsDTO.InvokeCustomerRegistration, productsDTO.ProductDisplayGroupFormatId, productsDTO.ZoneId, productsDTO.LockerExpiryInHours, productsDTO.LockerExpiryDate,
        //                                                         productsDTO.WaiverSetId, productsDTO.ProductType, productsDTO.MaxQtyPerDay, productsDTO.HsnSacCode, productsDTO.WebDescription, productsDTO.OrderTypeId,
        //                                                         productsDTO.IsGroupMeal, productsDTO.MembershipId, productsDTO.CardSale, productsDTO.ZoneCode, productsDTO.LockerMode, productsDTO.TaxName, productsDTO.UsedInDiscounts, executionContext.GetUserId(), DateTime.Now, productsDTO.FacilityMapId);
        //    }
        //    log.LogMethodExit(localProductsDTO);
        //    return localProductsDTO;
        //}

        private void dgvSearchSchedules_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (dgvSearchSchedules.CurrentCell != null)
            {
                if ((dgvSearchSchedules.Columns[e.ColumnIndex].Name == "scheduleFromTimeDataGridViewTextBoxColumn"
                    || dgvSearchSchedules.Columns[e.ColumnIndex].Name == "scheduleToTimeDataGridViewTextBoxColumn") && e.Value != null)
                {
                    string selectCondtion = "value = " + e.Value.ToString();
                    DataRow foundRows = GetMatchingRow(e.Value);
                    if (foundRows != null)
                        e.Value = foundRows["Display"];
                }
                if (e.Value != null && dgvSearchSchedules.Columns[e.ColumnIndex].Name == "fixedScheduleDataGridViewCheckBoxColumn" && e.Value.ToString().Length > 1)
                {
                    if (e.Value.ToString().ToLower() == "false")
                        e.Value = "N";
                    else
                        e.Value = "Y";
                }
            }
            log.LogMethodExit();
        }

        private DataRow GetMatchingRow(object value)
        {
            log.LogMethodEntry(value);
            DataRow selectedRow = null;
            for (int i = 0; i < dtFromTime.Rows.Count; i++)
            {
                if (Convert.ToDecimal(value) == Convert.ToDecimal(dtFromTime.Rows[i]["value"]))
                {
                    selectedRow = dtFromTime.Rows[i];
                    break;
                }
            }
            log.LogMethodExit(selectedRow);
            return selectedRow;
        }

        //private void dgvSearchFacilityProducts_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        //{
        //    log.LogMethodEntry(sender, e);
        //    if (dgvSearchSchedules.CurrentCell != null)
        //    {
        //        if (dgvSearchFacilityProducts.Columns[e.ColumnIndex].Name == "availableUnitsDataGridViewTextBoxColumn1")
        //        {
        //            if (dgvSearchFacilityProducts.Rows[e.RowIndex].Cells["ProductType"].Value.ToString() == ProductTypeValues.RENTAL)
        //            {
        //                e.Value = null;
        //            }
        //        }
        //    }
        //    log.LogMethodExit();
        //}

        private void dgvSearchFacilityProducts_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (dgvSearchFacilityProducts.CurrentCell != null && e.RowIndex > -1)
            {
                dgvSearchFacilityProducts.Rows[e.RowIndex].ReadOnly = true;
            }
            log.LogMethodExit();
        }


        private void dgvSearchFacilityProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            POSUtils.SetLastActivityDateTime();
            if (dgvSearchFacilityProducts.CurrentCell != null && e != null && e.RowIndex > -1)
            {
                if (dgvSearchFacilityProducts.Columns[e.ColumnIndex].Name == "SelectedRecord")
                {
                    if (BookingIsInEditMode())
                    {
                        DataGridViewCheckBoxCell checkBox = (dgvSearchFacilityProducts["SelectedRecord", e.RowIndex] as DataGridViewCheckBoxCell);
                        if (Convert.ToBoolean(checkBox.Value))
                        {
                            checkBox.Value = false;
                        }
                        else
                        {
                            checkBox.Value = true;
                        }
                        // dgvSearchFacilityProducts.EndEdit();
                        //dgvSearchFacilityProducts.CurrentCell = dgvSearchFacilityProducts["productNameDataGridViewTextBoxColumn1", e.RowIndex];
                        //dgvSearchFacilityProducts.RefreshEdit();
                    }
                }
                else if (dgvSearchFacilityProducts.Columns[e.ColumnIndex].Name == "productInformation")
                {
                    if (BookingIsInEditMode())
                    {
                        if (dgvSearchFacilityProducts["productIdDataGridViewTextBoxColumn1", e.RowIndex] != null)
                        {
                            try
                            {
                                txtMessage.Clear();
                                int selectedProductId = Convert.ToInt32(dgvSearchFacilityProducts["productIdDataGridViewTextBoxColumn1", e.RowIndex].Value);
                                string productType = dgvSearchFacilityProducts["ProductType", e.RowIndex].Value.ToString();
                                if (productType == ProductTypeValues.BOOKINGS)
                                {
                                    POSUtils.SetLastActivityDateTime();
                                    using (frmBookingPackageDetails frmPkgDetails = new frmBookingPackageDetails(selectedProductId))
                                    {
                                        POSUtils.SetLastActivityDateTime();
                                        frmPkgDetails.ShowDialog();
                                    }
                                }
                                else
                                {
                                    txtMessage.Text = MessageContainerList.GetMessage(executionContext, 2114);// "Please select a booking product to view the details"
                                }
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex);
                                txtMessage.Text = MessageContainerList.GetMessage(executionContext, 2114);// "Please select a booking product to view the details"
                                POSUtils.SetLastActivityDateTime();
                            }
                        }
                    }
                }
            }
            log.LogMethodExit();

        }

        private class SelectedScheduleNProductDetails
        {
            internal class ProductDetails
            {
                internal int productId = -1;
                internal string productName = "";
                internal int quantity = 1;
                internal double price = -1;
                internal string productType = "";
            }
            internal int scheduleId = -1;
            internal string scheduleName = "";
            internal int facilityMapId = -1;
            internal string facilityMapName = "";
            internal bool fixedSchedule = false;
            internal DateTime selectedScheduleDate = DateTime.MinValue;
            internal decimal selectedFromTime = -1;
            internal decimal selectedToTime = -1;
            internal int availableQuantity = 0;
            internal int facTotalUnits = 0;
            internal List<ProductDetails> selectedProducts = new List<ProductDetails>();
            internal ProductDetails bookingProduct;
            internal ProductDetails rentalProduct;
        }
        private void btnAddToBooking_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                POSUtils.SetLastActivityDateTime();
                this.Cursor = Cursors.WaitCursor;
                txtMessage.Clear();
                selectedScheduleNProductDetails = new SelectedScheduleNProductDetails();
                GetSelectionDetails();
                ShowBookingEntryUI();
                AddSelectedSchedulesToTheBooking();
                selectedScheduleNProductDetails = null;

            }
            catch (ValidationException ex)
            {
                log.Error(ex);
                txtMessage.Text = ex.GetAllValidationErrorMessages();
                POSUtils.SetLastActivityDateTime();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                txtMessage.Text = ex.Message;
                POSUtils.SetLastActivityDateTime();
            }
            try
            {
                RefreshSelectedBookingSchedule();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                txtMessage.Text = ex.Message;
                POSUtils.SetLastActivityDateTime();
                userAction = true;
            }
            this.Cursor = Cursors.Default;
            log.LogMethodExit();
        }

        private void ShowBookingEntryUI()
        {
            log.LogMethodEntry(selectedScheduleNProductDetails);
            POSUtils.SetLastActivityDateTime();
            if (BookingIsInEditMode() && selectedScheduleNProductDetails != null)
            {
                Semnox.Parafait.Transaction.Transaction.TransactionLine activeBookingLine = null;
                if (this.reservationBL != null)
                {
                    activeBookingLine = this.reservationBL.GetBookingProductTransactionLine();
                }
                if (this.reservationBL.GetReservationDTO == null || String.IsNullOrEmpty(reservationBL.GetReservationDTO.BookingName) || reservationBL.GetReservationDTO.Quantity == 0
                    || activeBookingLine == null)
                {
                    string bookingName = string.Empty;
                    int guestQty = -1;
                    if (reservationBL.GetReservationDTO != null && String.IsNullOrEmpty(reservationBL.GetReservationDTO.BookingName) == false)
                    {
                        bookingName = reservationBL.GetReservationDTO.BookingName;
                    }

                    if (reservationBL.GetReservationDTO != null && reservationBL.GetReservationDTO.Quantity > 0)
                    {
                        guestQty = reservationBL.GetReservationDTO.Quantity;
                    }
                    POSUtils.SetLastActivityDateTime();
                    using (frmBasicReservationInputs frmBasicData = new frmBasicReservationInputs(this.executionContext, this.utilities, dtFromTime, dtToTime,
                                                                    selectedScheduleNProductDetails.fixedSchedule, selectedScheduleNProductDetails.selectedScheduleDate,
                                                                    selectedScheduleNProductDetails.selectedFromTime,
                                                                    selectedScheduleNProductDetails.selectedToTime, bookingName, guestQty, selectedScheduleNProductDetails.facilityMapId, selectedScheduleNProductDetails.facTotalUnits))
                    {
                        frmBasicData.getBasicReservationInputs += new frmBasicReservationInputs.GetBasicReservationInputs(GetBasicReservationInputs);
                        POSUtils.SetLastActivityDateTime();
                        if (frmBasicData.ShowDialog() != DialogResult.OK)
                        {
                            txtMessage.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, "Please enter Booking Name/Guest Quantity details first");
                            throw new ValidationException(txtMessage.Text);
                        }
                    }
                    //using (GenericDataEntry reservationDetails = new GenericDataEntry(2))
                    //{
                    //    reservationDetails.Text = MessageContainerList.GetMessage(executionContext, "Booking Details");
                    //    reservationDetails.DataEntryObjects[0].mandatory = true;
                    //    reservationDetails.DataEntryObjects[0].label = MessageContainerList.GetMessage(executionContext, "Booking Name");
                    //    reservationDetails.DataEntryObjects[0].dataType = GenericDataEntry.DataTypes.String;
                    //    reservationDetails.DataEntryObjects[0].maxlength = 50;
                    //    reservationDetails.DataEntryObjects[0].width = 200;
                    //    if (reservationBL.GetReservationDTO != null && String.IsNullOrEmpty(reservationBL.GetReservationDTO.BookingName) == false)
                    //    {
                    //        reservationDetails.DataEntryObjects[0].data = reservationBL.GetReservationDTO.BookingName;
                    //    }
                    //    reservationDetails.DataEntryObjects[1].mandatory = true;
                    //    reservationDetails.DataEntryObjects[1].label = MessageContainerList.GetMessage(executionContext, "Guest Quantity");
                    //    reservationDetails.DataEntryObjects[1].dataType = GenericDataEntry.DataTypes.Integer;
                    //    reservationDetails.DataEntryObjects[1].maxlength = 5;
                    //    reservationDetails.DataEntryObjects[1].width = 50;
                    //    if (reservationBL.GetReservationDTO != null && reservationBL.GetReservationDTO.Quantity > 0)
                    //    {
                    //        reservationDetails.DataEntryObjects[1].data = Convert.ToString(reservationBL.GetReservationDTO.Quantity);
                    //    }
                    //    if (reservationDetails.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    //    {
                    //        //reservationBL.GetReservationDTO.BookingName = reservationDetails.DataEntryObjects[0].data;
                    //        txtBookingName.Text = reservationDetails.DataEntryObjects[0].data;
                    //        txtGuestQty.Text = reservationDetails.DataEntryObjects[1].data;
                    //        //reservationBL.GetReservationDTO.BookingName = reservationDetails.DataEntryObjects[1].data;
                    //        log.LogVariableState("reservationDetails.DataEntryObjects[0].data", reservationDetails.DataEntryObjects[0].data);
                    //        log.LogVariableState("reservationDetails.DataEntryObjects[1].data", reservationDetails.DataEntryObjects[1].data);

                    //    }
                    //    else
                    //    {
                    //        txtMessage.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, "Please enter Booking Name/Guest Quantity details first");
                    //        throw new ValidationException(txtMessage.Text);
                    //    }
                    //}
                }
            }
            log.LogMethodExit();
        }

        private void GetBasicReservationInputs(string bookingName, int guestQty, decimal selectedFromTime, decimal selectedToTime)
        {
            log.LogMethodEntry(bookingName, guestQty, selectedFromTime, selectedToTime);
            POSUtils.SetLastActivityDateTime();
            txtBookingName.Text = bookingName;
            txtGuestQty.Text = guestQty.ToString(utilities.ParafaitEnv.NUMBER_FORMAT);
            //txtGuestQty.Text = guestQty.ToString("####");
            if (selectedScheduleNProductDetails.fixedSchedule == false)
            {
                selectedScheduleNProductDetails.selectedFromTime = selectedFromTime;
                selectedScheduleNProductDetails.selectedToTime = selectedToTime;
            }
            log.LogMethodExit();
        }

        private void CheckForInitialMandatoryFields()
        {
            log.LogMethodEntry(selectedScheduleNProductDetails);
            txtMessage.Clear();
            POSUtils.SetLastActivityDateTime();
            ValidateBookingName();
            if (cmbChannel.SelectedItem == null || cmbChannel.SelectedIndex == -1)
            {
                log.Info("Channel was not Selected");
                this.ActiveControl = cmbChannel;
                throw new Exception(MessageContainerList.GetMessage(executionContext, 306));
            }
            //int eventHostId = -1;
            //string eventHostName = "";
            //int eventCheckListId = -1;
            //string eventCheckListName = "";
            //if (cmbEventHost.SelectedItem != null && (int)cmbEventHost.SelectedValue > -1)
            //{
            //    eventHostId = (int)cmbEventHost.SelectedValue;
            //    if (cmbEventHost.SelectedItem != null)
            //    {
            //        UsersDTO usersDTO = (UsersDTO)cmbEventHost.SelectedItem;
            //        eventHostName = usersDTO.LoginId;
            //    }
            //}
            //if (cmbEventCheckList.SelectedItem != null && (int)cmbEventCheckList.SelectedValue > -1)
            //{
            //    eventCheckListId = (int)cmbEventCheckList.SelectedValue;
            //    if (cmbEventCheckList.SelectedItem != null)
            //    {
            //        JobTaskGroupDTO jobTaskGroupDTO = (JobTaskGroupDTO)cmbEventCheckList.SelectedItem;
            //        eventCheckListName = jobTaskGroupDTO.TaskGroupName;
            //    }
            //}
            //if (eventHostId > -1 && eventCheckListId == -1 || eventCheckListId > -1 && eventHostId == -1)
            //{
            //    log.Info("Host Or Checklist was not Selected");
            //    this.ActiveControl = (eventHostId == -1 ? cmbEventHost : cmbEventCheckList);
            //    throw new Exception(MessageContainerList.GetMessage(executionContext, 2095));//"Please select host and checklist"

            //}
            if (reservationBL.GetReservationDTO == null && selectedScheduleNProductDetails != null)
            {

                // int questCount = Convert.ToInt32(txtGuestQty.Text);
                ReservationDTO reservationDTO = new ReservationDTO(-1, -1, txtBookingName.Text.Trim(), selectedScheduleNProductDetails.selectedScheduleDate, "N", "", null, 0, "", ReservationDTO.ReservationStatus.NEW.ToString(), -1, ""
                                                                   , -1, "", null, cmbChannel.SelectedItem.ToString(), "", "", ServerDateTime.Now, "", ServerDateTime.Now, "", false, executionContext.GetSiteId(), "", "", "", 0,
                                                                    selectedScheduleNProductDetails.selectedScheduleDate.AddHours((double)selectedScheduleNProductDetails.selectedToTime), -1, 0, "", "", selectedScheduleNProductDetails.bookingProduct.productId, -1, 0, -1,
                                                                    "", "", null, selectedScheduleNProductDetails.bookingProduct.productName, 0, 0, -1, "", "");
                reservationBL = new ReservationBL(executionContext, utilities, reservationDTO);
            }
            else if (reservationBL.GetReservationDTO != null)
            {
                //cmbChannel.SelectedItem.ToString()
                if (reservationBL.GetReservationDTO.BookingName != txtBookingName.Text.Trim())
                    reservationBL.GetReservationDTO.BookingName = txtBookingName.Text.Trim();
                if (reservationBL.GetReservationDTO.Channel != cmbChannel.SelectedItem.ToString())
                    reservationBL.GetReservationDTO.Channel = cmbChannel.SelectedItem.ToString();
                //if (reservationBL.GetReservationDTO.EventHostId != eventHostId)
                //    reservationBL.GetReservationDTO.EventHostId = eventHostId;
                //if (reservationBL.GetReservationDTO.EventHostName != eventHostName)
                //    reservationBL.GetReservationDTO.EventHostName = eventHostName;
                //if (reservationBL.GetReservationDTO.CheckListTaskGroupId != eventCheckListId)
                //    reservationBL.GetReservationDTO.CheckListTaskGroupId = eventCheckListId;
                //if (reservationBL.GetReservationDTO.CheckListTaskGroupName != eventCheckListName)
                //    reservationBL.GetReservationDTO.CheckListTaskGroupName = eventCheckListName;

            }
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }

        private void ValidateBookingName()
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            string specialChars = @"[-+=@]";
            if (string.IsNullOrEmpty(txtBookingName.Text.Trim()))
            {
                log.Info("Booking Name was not Entered");
                this.ActiveControl = txtBookingName;
                throw new Exception(MessageContainerList.GetMessage(executionContext, 302));//Booking Name was not Entered
            }
            else if (Regex.IsMatch(txtBookingName.Text.Substring(0, 1), specialChars))
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2265, MessageContainerList.GetMessage(executionContext, "Booking Name"), specialChars));
            }
            log.LogMethodExit();
        }

        private void AddSelectedSchedulesToTheBooking()
        {
            log.LogMethodEntry(selectedScheduleNProductDetails);
            try
            {
                POSUtils.SetLastActivityDateTime();
                StopInActivityTimeoutTimer();
                this.Cursor = Cursors.WaitCursor;
                if (selectedScheduleNProductDetails != null && selectedScheduleNProductDetails.scheduleId > -1)
                {
                    CheckForInitialMandatoryFields();
                    int guestCount = 0;
                    try
                    {
                        guestCount = GenericUtils.ConvertStringToInt(executionContext, txtGuestQty.Text);
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2104));//Please enter valid guest quantity
                    }
                    DateTime selectedFromDate = selectedScheduleNProductDetails.selectedScheduleDate.Date.AddMinutes((int)selectedScheduleNProductDetails.selectedFromTime * 60 + (double)selectedScheduleNProductDetails.selectedFromTime % 1 * 100);
                    DateTime selectedToDate = selectedScheduleNProductDetails.selectedScheduleDate.Date.AddMinutes((int)selectedScheduleNProductDetails.selectedToTime * 60 + (double)selectedScheduleNProductDetails.selectedToTime % 1 * 100);
                    int totalUnits = selectedScheduleNProductDetails.facTotalUnits;
                    int bookedUnits = 0;
                    FacilityMapBL facilityMapBL = new FacilityMapBL(executionContext, selectedScheduleNProductDetails.facilityMapId);
                    //DateTime scheduleToDate = selectedScheduleNProductDetails.selectedScheduleDate.Date.AddMinutes((int)selectedScheduleNProductDetails.selectedToTime * 60 + (double)selectedScheduleNProductDetails.selectedToTime % 1 * 100);
                    bookedUnits = facilityMapBL.GetTotalBookedUnitsForReservation(selectedFromDate, selectedToDate);
                    int availableUnits = totalUnits - bookedUnits;
                    selectedScheduleNProductDetails.availableQuantity = availableUnits;

                    if (selectedScheduleNProductDetails.availableQuantity < guestCount)
                    {
                        //guestCount = selectedScheduleNProductDetails.availableQuantity;
                        txtGuestQty.Text = string.Empty;
                        txtMessage.Text = MessageContainerList.GetMessage(executionContext, 326, guestCount, selectedScheduleNProductDetails.availableQuantity);//Cannot reserve quantity &1. Available: &2
                        throw new ValidationException(txtMessage.Text);
                    }
                    TransactionReservationScheduleDTO transactionReservationScheduleDTO = new TransactionReservationScheduleDTO(-1, -1, -1, guestCount, selectedScheduleNProductDetails.scheduleId, selectedScheduleNProductDetails.scheduleName, selectedFromDate,
                                                                                                                                selectedToDate, false, "", "", "",
                                                                                                                                ServerDateTime.Now, "", ServerDateTime.Now, executionContext.GetSiteId(), false, -1, -1, "", selectedScheduleNProductDetails.facilityMapId, selectedScheduleNProductDetails.facilityMapName, null);

                    int bookingProductId = -1;
                    if (selectedScheduleNProductDetails.bookingProduct != null)
                    {
                        bookingProductId = selectedScheduleNProductDetails.bookingProduct.productId;
                    }
                    reservationBL.ValidateScheduleNQuantity(bookingProductId, transactionReservationScheduleDTO, -1);
                    Transaction.TransactionLine outTrxLine = null;
                    outTrxLine = reservationBL.AddFacilityRentalProduct(selectedScheduleNProductDetails.rentalProduct.productId, transactionReservationScheduleDTO);
                    if (bookingProductId > -1)
                    {
                        reservationBL.AddBookingProduct(selectedScheduleNProductDetails.bookingProduct.productId, outTrxLine);
                    }
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
                POSUtils.SetLastActivityDateTime();
                StartInActivityTimeoutTimer();
            }
            log.LogMethodExit();
        }


        private void RefreshSelectedBookingSchedule()
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            this.Cursor = Cursors.WaitCursor;
            if (reservationBL.GetReservationDTO != null)
            {
                txtBookingName.Text = reservationBL.GetReservationDTO.BookingName;
                cmbChannel.SelectedItem = reservationBL.GetReservationDTO.Channel;
                //cmbEventHost.SelectedValue = reservationBL.GetReservationDTO.EventHostId;
                //cmbEventCheckList.SelectedValue = reservationBL.GetReservationDTO.CheckListTaskGroupId;
                txtGuestQty.Text = reservationBL.GetReservationDTO.Quantity.ToString(utilities.ParafaitEnv.NUMBER_FORMAT);
            }
            else
            {
                txtBookingName.Text = "";
                cmbChannel.SelectedItem = this.reservationDefaultSetup.GetDefaultChannel;
                //cmbEventHost.SelectedValue = -1;
                //cmbEventCheckList.SelectedValue = -1;
                txtGuestQty.Text = string.Empty;
            }
            LoadDGVSelectedBookingSchedule();
            this.Cursor = Cursors.Default;
            log.LogMethodExit();
        }

        private void LoadDGVSelectedBookingSchedule()
        {
            log.LogMethodEntry();
            try
            {
                POSUtils.SetLastActivityDateTime();
                StopInActivityTimeoutTimer();
                this.Cursor = Cursors.WaitCursor;
                List<TransactionReservationScheduleDTO> transactionReservationScheduleDTOList = null;
                List<KeyValuePair<Transaction.TransactionLine, TransactionReservationScheduleDTO>> mapList = new List<KeyValuePair<Transaction.TransactionLine, TransactionReservationScheduleDTO>>();
                if (reservationBL.ReservationTransactionIsNotNull())
                {
                    reservationBL.SetHeaderDetails();
                    List<Transaction.TransactionLine> trxLinesList = reservationBL.GetScheduleTransactionLines();
                    if (trxLinesList != null && trxLinesList.Count > 0)
                    {
                        transactionReservationScheduleDTOList = new List<TransactionReservationScheduleDTO>();
                        foreach (Transaction.TransactionLine item in trxLinesList)
                        {
                            TransactionReservationScheduleDTO trsDTO = item.GetCurrentTransactionReservationScheduleDTO();
                            if (trsDTO != null)
                            {
                                if (trsDTO.Cancelled == false)
                                    trsDTO.Cancelled = (item.LineValid != true);
                                if (trsDTO.TrxLineProductId == -1)
                                {
                                    trsDTO.TrxLineProductId = item.ProductID;
                                    trsDTO.TrxLineProductName = item.ProductName;
                                }
                                transactionReservationScheduleDTOList.Add(trsDTO);
                                mapList.Add(new KeyValuePair<Transaction.TransactionLine, TransactionReservationScheduleDTO>(item, trsDTO));
                            }
                        }
                    }
                    Transaction.TransactionLine bookingProdtrxLine = reservationBL.GetBookingProductTransactionLine();
                    if (bookingProdtrxLine != null)
                    {
                        if (trxLinesList == null)
                        {
                            trxLinesList = new List<Transaction.TransactionLine>();
                        }
                        trxLinesList.Add(bookingProdtrxLine);
                    }

                }
                if (transactionReservationScheduleDTOList != null && transactionReservationScheduleDTOList.Count > 0)
                {
                    if (BookingIsInEditMode())
                    {
                        for (int i = 0; i < transactionReservationScheduleDTOList.Count; i++)
                        {
                            if (transactionReservationScheduleDTOList[i].Cancelled == true)
                            {
                                transactionReservationScheduleDTOList.RemoveAt(i);
                                i = -1;
                                if (i < 0)
                                {
                                    i = 0;
                                }
                            }
                        }
                    }
                }
                dgvSelectedBookingSchedule.DataSource = transactionReservationScheduleDTOList;
                // selectedSchedulHScrollBar.UpdateButtonStatus();
                if (dgvSelectedBookingSchedule.Rows.Count > 0)
                {
                    userAction = false;
                    for (int i = 0; i < dgvSelectedBookingSchedule.Rows.Count; i++)
                    {
                        //schedulesDTOMasterList
                        dgvSelectedBookingSchedule.Rows[i].Cells["RemoveLine"].Value = "X";
                        int scheduleId = Convert.ToInt32(dgvSelectedBookingSchedule.Rows[i].Cells["schedulesIdDataGridViewTextBoxColumn"].Value);
                        DateTime schFromDate = Convert.ToDateTime(dgvSelectedBookingSchedule.Rows[i].Cells["scheduleFromDateDataGridViewTextBoxColumn"].Value);
                        DateTime schToDate = Convert.ToDateTime(dgvSelectedBookingSchedule.Rows[i].Cells["scheduleToDateDataGridViewTextBoxColumn"].Value);
                        dgvSelectedBookingSchedule.Rows[i].Cells["cmbFromTime"].Value = Convert.ToDecimal(schFromDate.Hour + schFromDate.Minute / 100.0);
                        dgvSelectedBookingSchedule.Rows[i].Cells["cmbToTime"].Value = Convert.ToDecimal(schToDate.Hour + schToDate.Minute / 100.0);
                        dgvSelectedBookingSchedule.Rows[i].Cells["guestQty"].Value = dgvSelectedBookingSchedule.Rows[i].Cells["guestQuantityDataGridViewTextBoxColumn"].Value;

                        //if (schedulesDTOMasterList.Exists(sch => sch.ScheduleId == scheduleId && sch.FixedSchedule == false))
                        //{

                        //    dgvSelectedBookingSchedule.Rows[i].Cells["cmbFromTime"].ReadOnly = false;
                        //    dgvSelectedBookingSchedule.Rows[i].Cells["cmbToTime"].ReadOnly = false;
                        //}
                        //else
                        {
                            dgvSelectedBookingSchedule.Rows[i].Cells["cmbFromTime"].ReadOnly = true;
                            dgvSelectedBookingSchedule.Rows[i].Cells["cmbToTime"].ReadOnly = true;
                        }
                        KeyValuePair<Transaction.TransactionLine, TransactionReservationScheduleDTO> facilityLineInfo = mapList.Find(item => item.Value == transactionReservationScheduleDTOList[i]);
                        // if (facilityLineInfo != null)
                        // {
                        Transaction.TransactionLine bookingProdLine = reservationBL.BookingTransaction.TrxLines.Find(tl => tl.ParentLine == facilityLineInfo.Key);
                        if (bookingProdLine != null)
                        {
                            dgvSelectedBookingSchedule.Rows[i].Cells["BookingProductName"].Value = bookingProdLine.ProductName;
                            dgvSelectedBookingSchedule.Rows[i].Cells["BookingProductId"].Value = bookingProdLine.ProductID;
                            dgvSelectedBookingSchedule.Rows[i].Cells["BookingProductTrxLine"].Value = bookingProdLine;
                        }
                        else
                        {
                            DataGridViewImageCell dataGridViewImageCell = dgvSelectedBookingSchedule.Rows[i].Cells["Reschedule"] as DataGridViewImageCell;
                            dataGridViewImageCell.Value = Properties.Resources.RescheduleReservationBlankImage;
                        }
                        dgvSelectedBookingSchedule.Rows[i].Cells["FacilityTrxLine"].Value = facilityLineInfo.Key;
                        // }
                    }
                    userAction = true;
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
                POSUtils.SetLastActivityDateTime();
                StartInActivityTimeoutTimer();
            }
            log.LogMethodExit();
        }

        private void LoadCustomerNCardDetails()
        {
            log.LogMethodEntry();
            if (reservationBL.GetReservationDTO != null && reservationBL.BookingTransaction != null)
            {
                if (reservationBL.BookingTransaction.customerDTO != null)
                {
                    customerDetailUI.CustomerDTO = reservationBL.BookingTransaction.customerDTO;
                }
                if (reservationBL.GetReservationDTO != null && reservationBL.GetReservationDTO.CardId != -1)
                {
                    txtCardNumber.Text = reservationBL.GetReservationDTO.CardNumber;
                }
            }
            log.LogMethodExit();
        }
        private void ResetCustomerNCardDetails()
        {
            log.LogMethodEntry();
            customerDetailUI.CustomerDTO = null;
            txtCardNumber.Text = string.Empty;
            log.LogMethodExit();
        }
        private void ClearNAdjustPnlPackageProducts()
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            for (int i = 0; i < pnlPackageProducts.Controls.Count; i++)// Control controlItem in pnlPackageProducts.Controls)
            {
                if (pnlPackageProducts.Controls[i].Name.StartsWith("usrCtrlPkgProductDetails0"))
                {
                    pnlPackageProducts.Controls.RemoveAt(i);
                    i = i - 1;
                }
            }
            //Adjustment for hiden price field
            pnlPackageProducts.Width = (945 - 90);
            packageListVScrollBar.Location = new Point(909 - 90, packageListVScrollBar.Location.Y);
            log.LogMethodExit();
        }

        private void ClearPnlAdditionalProducts()
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            for (int i = 0; i < pnlAdditionalProducts.Controls.Count; i++) //foreach (Control controlItem in pnlAdditionalProducts.Controls)
            {
                if (pnlAdditionalProducts.Controls[i].Name.StartsWith("usrCtrlAdditionProductDetails0"))
                {
                    pnlAdditionalProducts.Controls.RemoveAt(i);
                    i = i - 1;
                }
            }
            log.LogMethodExit();
        }
        private void LoadPackageDetails()
        {
            log.LogMethodEntry();
            try
            {
                this.Cursor = Cursors.WaitCursor;
                POSUtils.SetLastActivityDateTime();
                StopInActivityTimeoutTimer();
                txtMessage.Clear();
                if (reservationBL.ReservationTransactionIsNotNull())
                {
                    Products bookingProduct = reservationBL.GetBookingProduct();
                    int guestQty = reservationBL.GetGuestQuantity();
                    if (bookingProduct != null)
                    {
                        txtRemarks.Text = reservationBL.GetReservationDTO.Remarks;
                        // Products bookingProduct = new Products(bookingProductTrxLine.ProductID);
                        List<ComboProductDTO> packageProductDTOList = bookingProduct.GetComboPackageProductSetup(false);
                        List<Transaction.TransactionLine> packageProductTrxLines = reservationBL.GetPurchasedPackageProducts();
                        if (packageProductDTOList != null && packageProductDTOList.Count > 0)
                        {
                            ClearNAdjustPnlPackageProducts();
                            bool hasActiveLines = (packageProductTrxLines != null && packageProductTrxLines.Count > 1 ? packageProductTrxLines.Exists(tl => tl.LineValid == true) : false);
                            this.usrCtrlPkgProductDetails1.Visible = false;
                            int locationX = this.usrCtrlPkgProductDetails1.Location.X;
                            int locationY = this.usrCtrlPkgProductDetails1.Location.Y;
                            int counter = 0;
                            for (int i = 0; i < packageProductDTOList.Count; i++)
                            {
                                List<Transaction.TransactionLine> productTrxLines = null;
                                if (packageProductTrxLines != null && packageProductTrxLines.Count > 0)
                                {
                                    productTrxLines = GetLinesAndLinkedChildLines(packageProductTrxLines, packageProductDTOList[i].ChildProductId, packageProductDTOList[i].ComboProductId);
                                }
                                if (packageProductDTOList[i].IsActive == false && (productTrxLines == null || productTrxLines.Any() == false))
                                {
                                    //skip inactive combo with no assigned product lines
                                    continue;
                                }
                                usrCtrlProductDetails usrCtrlProductDetailsEntry = new usrCtrlProductDetails(packageProductDTOList[i], productTrxLines, guestQty, reservationBL.GetReservationDTO.Status);
                                usrCtrlProductDetailsEntry.CancelProductLine += new usrCtrlProductDetails.CancelProductLineDelegate(CancelProductLine);
                                usrCtrlProductDetailsEntry.RefreshTransactionLines += new usrCtrlProductDetails.RefreshTransactionLinesDelegate(RefreshProductTransactionLines);
                                usrCtrlProductDetailsEntry.AddProductToTransaction += new usrCtrlProductDetails.AddProductToTransactionDelegate(AddProductToBookingTransaction);
                                usrCtrlProductDetailsEntry.GetTrxProfile += new usrCtrlProductDetails.GetTrxProfileDelegate(GetTrxProfile);
                                usrCtrlProductDetailsEntry.SetDiscounts += new usrCtrlProductDetails.SetDiscountsDelegate(SetDiscounts);
                                usrCtrlProductDetailsEntry.GetTrxLineIndex += new usrCtrlProductDetails.GetTrxLineIndexDelegate(GetTrxLineIndex);
                                usrCtrlProductDetailsEntry.EditModifiers += new usrCtrlProductDetails.EditModifiersDelegate(EditModifiers);
                                usrCtrlProductDetailsEntry.RescheduleAttraction += new usrCtrlProductDetails.RescheduleAttractionDelegate(RescheduleAttraction);
                                usrCtrlProductDetailsEntry.RescheduleAttractionGroup += new usrCtrlProductDetails.RescheduleAttractionGroupDelegate(RescheduleAttractionGroup);
                                usrCtrlProductDetailsEntry.ChangePrice += new usrCtrlProductDetails.ChangePriceDelegate(ChangePrice);
                                usrCtrlProductDetailsEntry.ResetPrice += new usrCtrlProductDetails.ResetPriceDelegate(ResetPrice);

                                if (BookingIsInEditMode() && hasActiveLines == false
                                    && (packageProductTrxLines == null || packageProductTrxLines.Count == 0)
                                    && usrCtrlProductDetailsEntry.CbxSelectedProductChecked == false)
                                {
                                    if (packageProductDTOList.Count == 1 || packageProductDTOList[i].PriceInclusive)
                                    {
                                        usrCtrlProductDetailsEntry.CbxSelectedProductChecked = true;
                                    }
                                }
                                usrCtrlProductDetailsEntry.Name = "usrCtrlPkgProductDetails0" + counter.ToString();
                                pnlPackageProducts.Controls.Add(usrCtrlProductDetailsEntry);
                                usrCtrlProductDetailsEntry.Location = new Point(locationX, locationY);
                                locationY = locationY + 44;
                                usrCtrlProductDetailsEntry.HidePriceFieldAndAdjustUI();
                                usrCtrlProductDetailsEntry.Width = 890 - 102;
                            }
                        }
                    }
                }
                UpdateUserControlUIElements();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                txtMessage.Text = ex.Message;
            }
            finally
            {
                this.Cursor = Cursors.Default;
                POSUtils.SetLastActivityDateTime();
                StartInActivityTimeoutTimer();
            }
            log.LogMethodExit();
        }

        private void CancelProductLine(Transaction.TransactionLine trxLineRecord)
        {
            log.LogMethodEntry(trxLineRecord);
            try
            {
                POSUtils.SetLastActivityDateTime();
                StopInActivityTimeoutTimer();
                this.Cursor = Cursors.WaitCursor;
                txtMessage.Clear();
                if (trxLineRecord != null)
                {
                    if (trxLineRecord != null && reservationBL != null && reservationBL.ReservationTransactionIsNotNull())
                    {
                        int lineIndex = reservationBL.BookingTransaction.TrxLines.IndexOf(trxLineRecord);
                        reservationBL.RemoveProduct(trxLineRecord.ProductID, lineIndex);
                    }
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
                StartInActivityTimeoutTimer();
            }
            log.LogMethodExit();

        }

        private List<Transaction.TransactionLine> AddProductToBookingTransaction(int productId, double price, int quantity, string productType, int comboProductId)// PurchasedProducts purchasedProduct, List<AttractionBooking> atbList, List<ReservationDTO.SelectedCategoryProducts> categoryProducts)
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            this.Cursor = Cursors.WaitCursor;
            txtMessage.Clear();
            List<Transaction.TransactionLine> trxLineList = null;
            if (BookingIsInEditMode())
            {
                try
                {
                    Transaction.TransactionLine bookingProductTrxLine = reservationBL.GetBookingProductTransactionLine();
                    if (bookingProductTrxLine != null)
                    {
                        if (productType == ProductTypeValues.ATTRACTION)
                        {
                            AddAttractionProduct(-1, productId, comboProductId, price, quantity, -1, bookingProductTrxLine);
                        }
                        else if (productType == ProductTypeValues.COMBO)
                        {
                            AddComboProduct(productId, price, quantity, productType, comboProductId, bookingProductTrxLine);
                        }
                        else if (productType == ProductTypeValues.MANUAL)
                        {
                            AddManualProduct(productId, price, quantity, productType, comboProductId, bookingProductTrxLine);
                        }
                        else
                        {
                            try
                            {
                                POSUtils.SetLastActivityDateTime();
                                StopInActivityTimeoutTimer();
                                reservationBL.AddProduct(productId, price, quantity, comboProductId, bookingProductTrxLine, null, null, null);
                            }
                            finally
                            {
                                POSUtils.SetLastActivityDateTime();
                                StartInActivityTimeoutTimer();
                            }
                        }
                    }
                }
                catch (ValidationException ex)
                {
                    log.Error(ex);
                    txtMessage.Text = ex.GetAllValidationErrorMessages();
                    throw;
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    txtMessage.Text = ex.Message;
                    throw;
                }

            }
            this.Cursor = Cursors.Default;
            SetFormOnFocus();
            log.LogMethodExit(trxLineList);
            return trxLineList;
        }

        public void AddAttractionProduct(int comboChildRecordId, int productId, int comboProductId, double price, int quantity, int parentProductId, Transaction.TransactionLine bookingProductTrxLine, bool excludeSchedule = false)
        {
            log.LogMethodEntry(comboChildRecordId, productId, comboProductId, price, quantity, parentProductId, bookingProductTrxLine, excludeSchedule);
            POSUtils.SetLastActivityDateTime();
            this.Cursor = Cursors.WaitCursor;
            List<AttractionBooking> atbList = GetAttractionProduct(null, new Dictionary<int, int>() { { productId, quantity } }, comboProductId, price, parentProductId, excludeSchedule);
            this.Cursor = Cursors.WaitCursor;
            try
            {
                POSUtils.SetLastActivityDateTime();
                StopInActivityTimeoutTimer();
                if (atbList != null && atbList.Count > 0)
                {
                    if (bookingProductTrxLine != null)
                    {
                        //foreach (AttractionBooking item in atbList)
                        //{
                        reservationBL.AddProduct(productId, price, quantity, comboProductId, bookingProductTrxLine, null, atbList, null);
                        //}
                    }
                }
                else
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2097));//"Attraction schedule is not selected"

                }
            }
            finally
            {
                POSUtils.SetLastActivityDateTime();
                this.Cursor = Cursors.Default;
                StartInActivityTimeoutTimer();
            }
            log.LogMethodExit();
        }
        public List<AttractionBooking> GetAttractionProduct(List<ComboProductDTO> comboProducts, Dictionary<int, int> quantityMap, int comboProductId, double price, int parentProductId, bool excludeSchedule = false, List<AttractionBooking> atbMasterList = null, int comboQuantity = 1)
        {
            log.LogMethodEntry(comboProducts, quantityMap, comboProductId, price, parentProductId, excludeSchedule, atbMasterList);
            POSUtils.SetLastActivityDateTime();
            this.Cursor = Cursors.WaitCursor;
            List<AttractionBooking> atbList = null;
            if (BookingIsInEditMode())
            {
                Transaction.TransactionLine bookingProductTrxLine = reservationBL.GetBookingProductTransactionLine();
                if (bookingProductTrxLine != null)
                {
                    //if (scheduleTrxLine != null && scheduleTrxLine.Count > 0)
                    //{
                    DateTime fromDateTime = reservationBL.GetReservationDTO.FromDate;
                    DateTime toDateTime = reservationBL.GetReservationDTO.ToDate;
                    if (excludeSchedule)
                        excludeAttractionSchedule += ", " + exattractionScheduleId.ToString();


                    //AttractionSchedules ats = new AttractionSchedules(null, productId, quantity, fromDateTime, excludeAttractionSchedule, -1);
                    //List<AttractionBookingDTO> scheduleList = new List<AttractionBookingDTO>();

                    List<AttractionBooking> attractionBookings = null;
                    String message = "";
                    CustomerDTO customerDTO = (reservationBL.BookingTransaction != null
                                             && reservationBL.BookingTransaction.PrimaryCard != null
                                             && reservationBL.BookingTransaction.PrimaryCard.customerDTO != null ?
                                                reservationBL.BookingTransaction.PrimaryCard.customerDTO : (reservationBL.BookingTransaction != null
                                                                                                             && reservationBL.BookingTransaction.customerDTO != null ?
                                                                                                                reservationBL.BookingTransaction.customerDTO : null));

                    POSUtils.SetLastActivityDateTime();
                    using (AttractionSchedule attractionSchedules = new AttractionSchedule(POSStatic.Utilities, POSStatic.Utilities.ExecutionContext))
                    {
                        attractionBookings = attractionSchedules.ShowSchedulesForNonPOSScreens(quantityMap, null, fromDateTime, toDateTime,
                                                        -1, true, customerDTO, -1, ref message);
                    }
                    POSUtils.SetLastActivityDateTime();

                    if (attractionBookings != null && attractionBookings.Any())
                    {
                        this.Cursor = Cursors.WaitCursor;
                        int prodIdToAdd;
                        if (parentProductId == -1)
                        {
                            //When Attraction product is a Individual Product/Additional Product within Booking Product
                            prodIdToAdd = quantityMap.Keys.ElementAt(0);
                        }
                        else
                        {    //When Attraction product is part of combo add the combo product id
                            prodIdToAdd = parentProductId;
                        }

                        atbList = new List<AttractionBooking>();
                        List<int> comboLineQuantity = new List<int>();
                        int counter = comboQuantity;
                        if (comboProducts != null)
                        {
                            while (comboQuantity > 0)
                            {
                                foreach (ComboProductDTO cmbPrd in comboProducts)
                                {
                                    for (int i = 0; i < cmbPrd.Quantity; i++)
                                    {
                                        comboLineQuantity.Add(cmbPrd.ComboProductId);
                                    }
                                }
                                comboQuantity--;
                            }
                        }
                        for (int i = 0; i < quantityMap.Keys.Count; i++)
                        {
                            int productId = quantityMap.Keys.ElementAt(i);
                            int quantity = quantityMap[productId];
                            ProductsDTO productsDTO = GetProductsDTO(productId);

                            List<AttractionBooking> tempList = attractionBookings.Where(x => x.AttractionBookingDTO.AttractionProductId == productId).ToList();
                            List<AttractionBooking> atbListForProduct = new List<AttractionBooking>();

                            foreach (AttractionBooking ats in tempList)
                            {
                                try
                                {
                                    POSUtils.SetLastActivityDateTime();
                                    int seatsBooked = ats.AttractionBookingDTO.BookedUnits;
                                    while (seatsBooked > 0)
                                    {
                                        AttractionBooking atb = new AttractionBooking(executionContext);
                                        atb.CloneObject(ats, 1);

                                        // overriding the list as the cards numbers are yet to be generated
                                        atb.cardList = new List<Card>(atb.cardList);
                                        atb.cardNumberList = new List<string>(atb.cardNumberList);

                                        exattractionScheduleId = ats.AttractionBookingDTO.AttractionScheduleId;
                                        atb.AttractionBookingDTO.AttractionProductId = ats.AttractionBookingDTO.AttractionProductId;
                                        int comboChildRecordId = -1;
                                        if (comboProducts != null)
                                        {
                                            List<ComboProductDTO> tempProduct = comboProducts.Where(x => x.ChildProductId == atb.AttractionBookingDTO.AttractionProductId).ToList();
                                            if (tempProduct.Any())
                                            {
                                                for (int j = 0; j < comboLineQuantity.Count; j++)
                                                {
                                                    if (comboLineQuantity[j] > -1)
                                                    {
                                                        foreach (ComboProductDTO cmbPrd in tempProduct)
                                                        {
                                                            if (comboLineQuantity[j] == cmbPrd.ComboProductId)
                                                            {
                                                                comboChildRecordId = cmbPrd.ComboProductId;
                                                                comboLineQuantity[j] = -1;
                                                                break;
                                                            }
                                                        }
                                                    }
                                                    if (comboChildRecordId > -1)
                                                        break;
                                                }

                                            }
                                        }

                                        //ats.AttractionBookingDTO.Identifier = comboChildRecordId;
                                        atb.AttractionBookingDTO.Identifier = comboChildRecordId;
                                        atb.Save(-1);
                                        POSUtils.SetLastActivityDateTime();
                                        atbListForProduct.Add(atb);
                                        seatsBooked--;
                                    }
                                    //lstAtsProduct.Add(new KeyValuePair<AttractionBooking, int>(atb, prodIdToAdd));
                                }
                                catch (Exception ex)
                                // if (!atb.Save(-1, null, ref message))
                                {
                                    log.Error(ex);
                                    log.LogMethodExit(ex.Message);
                                    throw;
                                }
                            }

                            //retVal = true;
                            List<Card> cardList = null;
                            this.Cursor = Cursors.WaitCursor;
                            if (GetAttractionCards(productsDTO, comboProductId, quantity, atbList, atbListForProduct, out cardList) == false)
                            {
                                if (atbListForProduct != null)
                                {
                                    foreach (AttractionBooking atb in atbListForProduct)
                                    {
                                        POSUtils.SetLastActivityDateTime();
                                        atb.Expire();
                                    }
                                }
                            }
                            else
                            {
                                this.Cursor = Cursors.WaitCursor;
                                if (atbListForProduct != null && atbListForProduct.Count > 0)
                                {
                                    foreach (AttractionBooking atb in atbListForProduct)
                                    {
                                        if (productsDTO.CardSale.Equals("Y") == false)
                                        {
                                            if (atb.cardList != null)
                                            {
                                                atb.cardList = null;
                                            }
                                        }
                                    }
                                    //
                                    // reservationBL.AddProduct(productId, price, quantity, comboProductId, bookingProductTrxLine[0], null, atbList, null);
                                }
                            }
                            atbList.AddRange(atbListForProduct);
                        }
                        //if (parentProductId != -1) // combo
                        //{
                        //    List<frmInputCards.clsCard> lstAtbCards = new List<frmInputCards.clsCard>();

                        //    // get a list of all cards already used
                        //    foreach (KeyValuePair<AttractionBooking, int> keyval in lstAtsProduct)
                        //    {
                        //        foreach (string cardNumber in keyval.Key.cardNumberList)
                        //        {
                        //            frmInputCards.clsCard card = new frmInputCards.clsCard(cardNumber);
                        //            card.products.Add(keyval.Key.AttractionScheduleName + " " + keyval.Key.ScheduleTime.ToString(POSStatic.ParafaitEnv.DATETIME_FORMAT));
                        //            lstAtbCards.Add(card);
                        //        }
                        //    }

                        //    DataRow dr = trxUtils.getProductDetails(product_id, null).Rows[0];
                        //    if (lstAtbCards.Count == 0) // no cards allocated which means this is the first set of schedules
                        //    {
                        //        foreach (KeyValuePair<AttractionBooking, int> keyval in lstAtsProduct)
                        //        {
                        //            for (int i = 0; i < keyval.Key.BookedUnits; i++)
                        //            {
                        //                string cardNumber = utilities.GenerateRandomCardNumber();
                        //                if (dr["AutoGenerateCardNumber"].ToString() != "Y")
                        //                    cardNumber = "T" + cardNumber.Substring(1);
                        //                keyval.Key.cardNumberList.Add(cardNumber);
                        //            }
                        //        }
                        //    }
                        //    else
                        //    {
                        //        using (frmInputCards fip = new frmInputCards(dr["Product_Name"].ToString(), lstAtbCards, atbList))
                        //        {
                        //            if (fip.ShowDialog() != DialogResult.OK)
                        //            {
                        //                //retVal = false;
                        //            }
                        //        }
                        //    }
                        //}
                    }
                    else
                    {
                        // retVal = false;
                    }

                    //ats.Dispose();
                    // log.LogMethodExit(retVal);
                    // return retVal;
                }
                //}
            }
            this.Cursor = Cursors.Default;
            log.LogMethodExit(atbList);
            return atbList;
        }

        bool GetAttractionCards(ProductsDTO productsDTO, int bookingComboProductId, int cardCount, List<AttractionBooking> atbMasterList, List<AttractionBooking> atbList, out List<Card> cardList)
        {
            log.LogMethodEntry(productsDTO, bookingComboProductId, cardCount, atbMasterList, atbList);
            POSUtils.SetLastActivityDateTime();
            if (productsDTO == null)
            {
                log.LogMethodExit("Attraction Product details are missing");
                throw new Exception(MessageContainerList.GetMessage(executionContext, "Attraction Product details are missing"));
            }
            string productName = productsDTO.ProductName;
            bool cardSale = productsDTO.CardSale.Equals("Y");
            bool autogenCardNumber = productsDTO.AutoGenerateCardNumber.Equals("Y");
            bool loadToSingleCard = productsDTO.CardCount.ToString().Equals("1");
            log.LogVariableState("cardSale", cardSale);
            log.LogVariableState("autogenCardNumber", autogenCardNumber);
            log.LogVariableState("loadToSingleCard", loadToSingleCard);

            this.Cursor = Cursors.WaitCursor;
            cardList = null;
            if (!cardSale)
            {
                this.Cursor = Cursors.Default;
                log.LogMethodExit("Return Value: true");
                return true;
            }

            if (loadToSingleCard == false)
            {
                if (atbList != null)
                {
                    if (autogenCardNumber)
                    {
                        int trxCardCount = 0;
                        //reservationBL.BookingTransaction.TrxLines.Count(x => (x.CardNumber != null && x.LineValid == true));
                        if (atbMasterList != null)
                        {
                            trxCardCount = atbMasterList.Where(atb => atb.cardList != null && atb.cardList.Count > 0).Sum(atb => atb.cardList.Count);
                        }
                        bool autogen = false;
                        if (trxCardCount >= cardCount)
                        {
                            List<frmInputCards.clsCard> lstAtbCards = GenerateClsCardList(atbMasterList);
                            POSUtils.SetLastActivityDateTime();
                            using (Reservation.frmInputCards fip = new Reservation.frmInputCards(productName, lstAtbCards, atbList))
                            {
                                if (fip.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                                {
                                    POSUtils.SetLastActivityDateTime();
                                    POSUtils.ParafaitMessageBox("Card numbers will be auto-generated");
                                    autogen = true;
                                }
                            }
                        }
                        else
                            autogen = true;

                        if (autogen)
                        {
                            foreach (AttractionBooking atb in atbList)
                            {
                                int cards = atb.AttractionBookingDTO.BookedUnits;
                                while (cards-- > 0)
                                {
                                    POSUtils.SetLastActivityDateTime();
                                    RandomTagNumber randomTagNumber = new RandomTagNumber(executionContext);
                                    atb.cardNumberList.Add(randomTagNumber.Value);
                                }
                            }
                        }
                    }
                    else
                    {
                        POSUtils.SetLastActivityDateTime();
                        int trxCardCount = 0;
                        //reservationBL.BookingTransaction.TrxLines.Count(x => (x.CardNumber != null && x.LineValid == true));
                        if (atbMasterList != null)
                        {
                            trxCardCount = atbMasterList.Where(atb => atb.cardList != null && atb.cardList.Count > 0).Sum(atb => atb.cardList.Count);
                        }
                        bool autogen = true;
                        if (trxCardCount >= cardCount)
                        {
                            List<frmInputCards.clsCard> lstAtbCards = GenerateClsCardList(atbMasterList);
                            POSUtils.SetLastActivityDateTime();
                            using (Reservation.frmInputCards fip = new Reservation.frmInputCards(productName, lstAtbCards, atbList))
                            {
                                if (fip.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                                {
                                    POSUtils.ParafaitMessageBox("Card numbers will be auto-generated");
                                }
                                else
                                {
                                    autogen = false;
                                }
                            }
                        }

                        if (autogen)
                        {
                            foreach (AttractionBooking atb in atbList)
                            {
                                int cards = atb.AttractionBookingDTO.BookedUnits;
                                while (cards-- > 0)
                                {
                                    POSUtils.SetLastActivityDateTime();
                                    RandomTagNumber randomTagNumber = new RandomTagNumber(executionContext);
                                    string cardNumber = randomTagNumber.Value;
                                    cardNumber = "T" + cardNumber.Substring(1);
                                    atb.cardNumberList.Add(cardNumber);
                                }
                            }
                        }
                    }

                    Card currentCard;

                    foreach (AttractionBooking atb in atbList)
                    {
                        foreach (string cardNumber in atb.cardNumberList)
                        {
                            POSUtils.SetLastActivityDateTime();
                            if (POSStatic.ParafaitEnv.MIFARE_CARD)
                            {
                                currentCard = new MifareCard(Common.Devices.PrimaryCardReader, cardNumber, ParafaitEnv.LoginID, utilities);
                            }
                            else
                            {
                                currentCard = new Card(Common.Devices.PrimaryCardReader, cardNumber, ParafaitEnv.LoginID, utilities);
                            }
                            atb.cardList.Add(currentCard);
                        }
                    }
                }
                else
                {
                    this.Cursor = Cursors.Default;
                    log.LogMethodExit(false, "atbList == null");
                    return false;
                }
            }
            else // load to single card
            {
                POSUtils.SetLastActivityDateTime();
                string cardNumber = string.Empty;
                //check whether product is already added to the transaction under current package/additional product setup
                if (bookingComboProductId > -1 && reservationBL != null && reservationBL.BookingTransaction != null && reservationBL.BookingTransaction.TrxLines != null && reservationBL.BookingTransaction.TrxLines.Any())
                {
                    List<Transaction.TransactionLine> pkgORAddProdLineList = reservationBL.BookingTransaction.TrxLines.Where(tl => tl.LineValid == true && (tl.ComboproductId == bookingComboProductId || (tl.ComboproductId == -1 && tl.ParentLine != null && tl.ParentLine.ComboproductId == bookingComboProductId))).ToList();
                    if (pkgORAddProdLineList != null && pkgORAddProdLineList.Any())
                    {
                        log.LogVariableState("pkgORAddProdLineList", pkgORAddProdLineList);
                        Transaction.TransactionLine cardNumberLine = pkgORAddProdLineList.Where(tl => tl.ProductID == productsDTO.ProductId).FirstOrDefault(cardTL => string.IsNullOrEmpty(cardTL.CardNumber) == false);
                        log.LogVariableState("cardNumberLine", cardNumberLine);
                        if (cardNumberLine != null)
                        {
                            cardNumber = cardNumberLine.CardNumber;
                        }
                        //else
                        //{
                        //    List<Transaction.TransactionLine> productLineList = new List<Transaction.TransactionLine>();
                        //    for (int i = 0; i < pkgORAddProdLineList.Count; i++)
                        //    {
                        //        List<Transaction.TransactionLine> productLineListTemp = reservationBL.BookingTransaction.TrxLines.Where(tl => tl.LineValid == true && tl.ParentLine == pkgORAddProdLineList[i]).ToList();
                        //        if(productLineListTemp != null && productLineListTemp.Any())
                        //        {
                        //            productLineList.AddRange(productLineListTemp);
                        //        }
                        //    }
                        //    if(productLineList != null && productLineList.Any())
                        //    {
                        //        cardNumberLine = productLineList.Where(tl => tl.ProductID == productsDTO.ProductId).FirstOrDefault(cardTL => string.IsNullOrEmpty(cardTL.CardNumber) == false);
                        //        if (cardNumberLine != null)
                        //        {
                        //            cardNumber = cardNumberLine.CardNumber;
                        //        }
                        //    }
                        //}
                    }
                }
                if (string.IsNullOrEmpty(cardNumber))
                {
                    POSUtils.SetLastActivityDateTime();
                    if (autogenCardNumber)
                    {
                        RandomTagNumber randomTagNumber = new RandomTagNumber(executionContext);
                        cardNumber = randomTagNumber.Value;
                    }
                    else
                    {
                        RandomTagNumber randomTagNumber = new RandomTagNumber(executionContext);
                        cardNumber = randomTagNumber.Value;
                        cardNumber = "T" + cardNumber.Substring(1);
                    }
                }

                Card currentCard;
                POSUtils.SetLastActivityDateTime();
                if (POSStatic.ParafaitEnv.MIFARE_CARD)
                {
                    currentCard = new MifareCard(Common.Devices.PrimaryCardReader, cardNumber, ParafaitEnv.LoginID, utilities);
                }
                else
                {
                    currentCard = new Card(Common.Devices.PrimaryCardReader, cardNumber, ParafaitEnv.LoginID, utilities);
                }

                if (atbList != null)
                {
                    foreach (AttractionBooking atb in atbList)
                    {
                        int cards = atb.AttractionBookingDTO.BookedUnits;
                        while (cards-- > 0)
                        {
                            atb.cardNumberList.Add(cardNumber);
                            atb.cardList.Add(currentCard);
                        }
                    }
                }
                else
                {
                    this.Cursor = Cursors.Default;
                    log.LogMethodExit(false, "atbList == null");
                    return false;
                }
            }
            this.Cursor = Cursors.Default;
            log.LogMethodExit("Return Value: true");
            return true;
        }

        private static List<frmInputCards.clsCard> GenerateClsCardList(List<AttractionBooking> atbMasterList)
        {
            log.LogMethodEntry(atbMasterList);
            List<frmInputCards.clsCard> lstAtbCards = new List<frmInputCards.clsCard>();
            if (atbMasterList != null)
            {// get a list of all cards already used
                foreach (AttractionBooking atb in atbMasterList)
                {
                    foreach (string cardNumber in atb.cardNumberList)
                    {
                        frmInputCards.clsCard card = new frmInputCards.clsCard(cardNumber);
                        card.products.Add(atb.AttractionBookingDTO.AttractionScheduleName + " " + atb.AttractionBookingDTO.ScheduleFromDate.ToString(POSStatic.ParafaitEnv.DATETIME_FORMAT));
                        lstAtbCards.Add(card);
                    }
                }
            }
            log.LogMethodExit(lstAtbCards);
            return lstAtbCards;
        }

        private void AddManualProduct(int productId, double price, int quantity, string productType, int comboProductId, Transaction.TransactionLine bookingProductTrxLine)
        {
            log.LogMethodEntry(productId, price, quantity, productType, comboProductId, bookingProductTrxLine);
            try
            {
                POSUtils.SetLastActivityDateTime();
                this.Cursor = Cursors.WaitCursor;
                int productQuantity = quantity;
                if (productQuantity > 0)
                {
                    PurchasedProducts purchasedProducts = GetModifierDetails(productId, productType);
                    try
                    {
                        POSUtils.SetLastActivityDateTime();
                        StopInActivityTimeoutTimer();
                        this.Cursor = Cursors.WaitCursor;
                        if (purchasedProducts != null)
                        {
                            while (productQuantity > 0)
                            {
                                List<KeyValuePair<int, PurchasedProducts>> purchasedProductList = new List<KeyValuePair<int, PurchasedProducts>>();
                                purchasedProductList.Add(new KeyValuePair<int, PurchasedProducts>(productId, purchasedProducts));
                                reservationBL.AddProduct(productId, price, 1, comboProductId, bookingProductTrxLine, purchasedProductList, null, null);
                                productQuantity--;
                            }
                        }
                        else
                        {
                            reservationBL.AddProduct(productId, price, quantity, comboProductId, bookingProductTrxLine, null, null, null);
                        }
                    }
                    finally
                    {
                        POSUtils.SetLastActivityDateTime();
                        StartInActivityTimeoutTimer();
                    }
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }

        private void AddComboProduct(int productId, double price, int quantity, string productType, int comboProductId, Transaction.TransactionLine bookingProductTrxLine)
        {
            log.LogMethodEntry(productId, price, quantity, productType, comboProductId, bookingProductTrxLine);
            this.Cursor = Cursors.WaitCursor;
            POSUtils.SetLastActivityDateTime();
            List<ReservationDTO.SelectedCategoryProducts> categoryProductSelectedList = new List<ReservationDTO.SelectedCategoryProducts>();

            List<string> cardList = new List<string>();
            List<AttractionBooking> atbList = null;
            if (quantity > 0 && BookingIsInEditMode())
            {
                List<ComboProductDTO> comboProductDTOList = GetComboProductList(productId);
                List<KeyValuePair<int, int>> categoryProductList = new List<KeyValuePair<int, int>>();
                List<KeyValuePair<int, PurchasedProducts>> productModifiersList = new List<KeyValuePair<int, PurchasedProducts>>();
                int comboQuantity = quantity;
                POSUtils.SetLastActivityDateTime();
                if (comboProductDTOList != null && comboProductDTOList.Count > 0)//comboProductDTOList.Exists(cb => cb.Quantity > 0)
                {
                    List<ComboProductDTO> attractionComboProductDTOList = comboProductDTOList.Where(cb => cb.ChildProductType == ProductTypeValues.ATTRACTION && cb.Quantity > 0).ToList();
                    if (attractionComboProductDTOList != null && attractionComboProductDTOList.Count > 0)
                    {
                        comboQuantity = quantity;
                        quantity = 0;
                    }
                    List<ComboProductDTO> CategoryComboProductDTOList = comboProductDTOList.Where(cb => cb.CategoryId != -1 && cb.Quantity > 0).ToList();
                    List<ComboProductDTO> manualProductDTOList = comboProductDTOList.Where(cb => cb.ChildProductType == ProductTypeValues.MANUAL).ToList();
                    int categoryComboQuantity = 0;

                    if (CategoryComboProductDTOList != null && CategoryComboProductDTOList.Count > 0)
                    {
                        foreach (ComboProductDTO comboProductDTO in CategoryComboProductDTOList)
                        {
                            POSUtils.SetLastActivityDateTime();
                            List<CategoryDTO> categoryDTOList = GetCategoryDTOList(comboProductDTO.CategoryId);
                            if (categoryDTOList != null && categoryDTOList.Count > 0)
                            {
                                foreach (CategoryDTO categoryDTO in categoryDTOList)
                                {
                                    POSUtils.SetLastActivityDateTime();
                                    this.Cursor = Cursors.WaitCursor;
                                    //Open the form to Add products from the category product list
                                    using (frmProductList fpl = new frmProductList(categoryDTO.Name, categoryDTO.CategoryId, comboQuantity * (int)comboProductDTO.Quantity, productId, reservationBL.GetReservationDTO.BookingId, categoryProductSelectedList, (reservationBL.GetReservationDTO.Status == ReservationDTO.ReservationStatus.WIP.ToString())))
                                    {
                                        POSUtils.SetLastActivityDateTime();
                                        if (fpl.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                                        {
                                            if (fpl.SelectedProductList.Count > 0)
                                            {
                                                // category level product will be 1, individual products will have the quanntity. 
                                                // Set to 1 to prevent multiplication by combo quantity
                                                categoryComboQuantity = 1;

                                                List<KeyValuePair<int, int>> nonManualProductsList = new List<KeyValuePair<int, int>>();
                                                foreach (KeyValuePair<int, int> catProduct in fpl.SelectedProductList)
                                                {
                                                    // Products comboChildProduct = new Products(catProduct.Key);
                                                    ProductsDTO comboChildProductDTO = GetProductsDTO(catProduct.Key);
                                                    if (comboChildProductDTO != null)
                                                    {
                                                        ComboProductDTO tempProduct = new ComboProductDTO(
                                                                     comboProductDTO.ComboProductId,
                                                                     productId,
                                                                     comboChildProductDTO.ProductId,
                                                                     catProduct.Value,
                                                                     comboProductDTO.CategoryId,
                                                                     "",
                                                                     comboChildProductDTO.TaxInclusivePrice.ToString() == "Y" ? true : false,
                                                                     false,
                                                                     -1,
                                                                     Convert.ToDouble(comboChildProductDTO.Price.ToString()),
                                                                     1,
                                                                     comboChildProductDTO.SiteId,
                                                                     "",
                                                                     true,
                                                                     -1,
                                                                     "",
                                                                     ServerDateTime.Now,
                                                                     "",
                                                                     ServerDateTime.Now,
                                                                     comboChildProductDTO.ProductType,
                                                                     comboChildProductDTO.ProductName,
                                                                     comboChildProductDTO.AutoGenerateCardNumber,
                                                                     true,
                                                                     string.Empty,
                                                                     comboChildProductDTO.MaximumQuantity);
                                                        comboProductDTOList.Add(tempProduct);

                                                        switch (tempProduct.ChildProductType)
                                                        {
                                                            case ProductTypeValues.ATTRACTION:
                                                                attractionComboProductDTOList.Add(tempProduct);
                                                                nonManualProductsList.Add(catProduct);
                                                                categoryProductSelectedList = categoryProductSelectedList.Where(x => x.productId != comboChildProductDTO.ProductId).ToList();
                                                                break;
                                                            default:
                                                                continue;
                                                        }
                                                    }
                                                }

                                                foreach (KeyValuePair<int, int> nonManualProduct in nonManualProductsList)
                                                {
                                                    fpl.SelectedProductList.Remove(nonManualProduct);
                                                }

                                                categoryProductList.AddRange(fpl.SelectedProductList);
                                            }
                                        }
                                        else
                                        {
                                            log.LogMethodExit("Dialog was cancelled");
                                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2098));// "Category products are not selected"
                                        }
                                    }
                                }
                            }
                            //Add create trx line code
                        }
                    }
                    this.Cursor = Cursors.WaitCursor;
                    //Begin Modification - Jan-20-2016-Attraction product within combo//
                    // if this is a category combo, then set combo quantity as 1, else the user given value
                    //int totalProductdQty = categoryComboQuantity == 0 ? comboQuantity : categoryComboQuantity;

                    POSUtils.SetLastActivityDateTime();
                    if (attractionComboProductDTOList != null && attractionComboProductDTOList.Count > 0)
                    {
                        int lclChildProduct = -1;
                        excludeAttractionSchedule = "-1";
                        List<AttractionBooking> atbListForComboChildATS = null;
                        Dictionary<int, int> quantityMap = new Dictionary<int, int>();

                        //if (alreadyAdded == false || allowGet == true)
                        {
                            bool excludeSchedule = false;
                            foreach (ComboProductDTO comboProductDTO in attractionComboProductDTOList)
                            {
                                if (lclChildProduct == comboProductDTO.ChildProductId)
                                {
                                    excludeSchedule = true;
                                }
                                lclChildProduct = comboProductDTO.ChildProductId;
                                int seats = (comboProductDTO.CategoryId > -1 ? 1 : comboQuantity) * (int)comboProductDTO.Quantity;
                                if (quantityMap.ContainsKey(lclChildProduct))
                                {
                                    quantityMap[lclChildProduct] = quantityMap[lclChildProduct] + seats;
                                }
                                else
                                {
                                    quantityMap.Add(lclChildProduct, seats);
                                }
                            }
                            POSUtils.SetLastActivityDateTime();
                            atbListForComboChildATS = GetAttractionProduct(comboProductDTOList, quantityMap, comboProductId, price, productId, excludeSchedule, atbList, comboQuantity);
                            if (atbListForComboChildATS == null || atbListForComboChildATS.Count == 0)
                            {
                                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2097));//"Attraction schedule is not selected"
                            }
                            else
                            {
                                if (atbList == null)
                                {
                                    atbList = new List<AttractionBooking>();
                                }
                                atbList.AddRange(atbListForComboChildATS);
                            }
                        }
                    }
                    this.Cursor = Cursors.WaitCursor;
                    //End Modification - Jan-20-2016-Attraction product within combo// 
                    if (manualProductDTOList != null && manualProductDTOList.Count > 0)
                    {
                        foreach (ComboProductDTO comboProductDTO in manualProductDTOList)
                        {
                            POSUtils.SetLastActivityDateTime();
                            PurchasedProducts purchasedProducts = GetModifierDetails(comboProductDTO.ChildProductId, comboProductDTO.ChildProductType);
                            if (purchasedProducts != null)
                            {
                                productModifiersList.Add(new KeyValuePair<int, PurchasedProducts>(comboProductDTO.ChildProductId, purchasedProducts));
                            }
                        }
                    }
                }
                this.Cursor = Cursors.WaitCursor;
                comboProductDTOList = comboProductDTOList.OrderBy(x => x.CategoryId).ToList();
                while (comboQuantity-- > 0)
                {
                    List<AttractionBooking> atbForCombo = new List<AttractionBooking>();
                    List<ReservationDTO.SelectedCategoryProducts> categoryProductForComboLine = new List<ReservationDTO.SelectedCategoryProducts>();
                    int currentCategory = -1;
                    int currentCategoryQty = 0;
                    int requiredCatQty = 0;

                    foreach (ComboProductDTO selectedProduct in comboProductDTOList)
                    {
                        POSUtils.SetLastActivityDateTime();
                        switch (selectedProduct.ChildProductType)
                        {
                            case ProductTypeValues.ATTRACTION:
                                List<AttractionBooking> tempList = (atbList != null ? atbList.Where(x => x.AttractionBookingDTO.AttractionProductId == selectedProduct.ChildProductId
                                        && x.AttractionBookingDTO.BookedUnits > 0).ToList() : new List<AttractionBooking>());
                                int requiredQty = selectedProduct.CategoryId == -1 ? selectedProduct.Quantity == null ? 0 : (int)selectedProduct.Quantity : requiredCatQty - currentCategoryQty;
                                foreach (AttractionBooking tempProd in tempList)
                                {
                                    if (tempProd.AttractionBookingDTO.BookedUnits > 0 && requiredQty > 0)
                                    {
                                        AttractionBooking comboATB = new AttractionBooking(this.executionContext);
                                        int AvailableUnits = Math.Min(tempProd.AttractionBookingDTO.BookedUnits, requiredQty);
                                        comboATB.CloneObject(tempProd, AvailableUnits);
                                        comboATB.AttractionBookingDTO.AttractionProductId = tempProd.AttractionBookingDTO.AttractionProductId;
                                        comboATB.AttractionBookingDTO.Identifier = tempProd.AttractionBookingDTO.Identifier;
                                        if (comboATB.cardList != null)
                                            comboATB.cardList = comboATB.cardList.Skip(0).Take(Math.Min(AvailableUnits, comboATB.cardList.Count)).ToList();
                                        if (comboATB.cardNumberList != null)
                                            comboATB.cardNumberList = comboATB.cardNumberList.Skip(0).Take(Math.Min(AvailableUnits, comboATB.cardNumberList.Count)).ToList();

                                        if (tempProd.cardList != null)
                                            tempProd.cardList.RemoveRange(0, Math.Min(AvailableUnits, comboATB.cardList.Count));
                                        if (tempProd.cardNumberList != null)
                                            tempProd.cardNumberList.RemoveRange(0, Math.Min(AvailableUnits, comboATB.cardNumberList.Count));

                                        requiredQty -= AvailableUnits;

                                        if (selectedProduct.CategoryId != -1)
                                            currentCategoryQty += AvailableUnits;
                                        atbForCombo.Add(comboATB);
                                        try
                                        {
                                            tempProd.ReduceBookedUnits(AvailableUnits);
                                            comboATB.Save(-1);
                                        }
                                        catch (Exception ex)
                                        {
                                            log.LogMethodExit("error while saving atb " + ex.Message);
                                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Unexpected error while saving the changes."));// "Category products are not selected"
                                        }
                                    }
                                }
                                break;
                            case "": // for a category, the child product type is blank
                                currentCategory = selectedProduct.CategoryId;
                                currentCategoryQty = 0;
                                requiredCatQty = selectedProduct.Quantity == null ? 0 : (int)selectedProduct.Quantity;
                                break;
                            default:
                                if (currentCategory != -1 && selectedProduct.CategoryId == currentCategory)
                                {
                                    List<ReservationDTO.SelectedCategoryProducts> tempCatList = categoryProductSelectedList.Where(x => x.productId == selectedProduct.ChildProductId).ToList();
                                    foreach (ReservationDTO.SelectedCategoryProducts tempProd in tempCatList)
                                    {
                                        if (tempProd.quantity > 0 && currentCategoryQty < requiredCatQty)
                                        {
                                            int AvailableUnits = Math.Min(tempProd.quantity, (requiredCatQty - currentCategoryQty));

                                            ReservationDTO.SelectedCategoryProducts catComboprod = new ReservationDTO.SelectedCategoryProducts()
                                            {
                                                parentComboProductId = tempProd.parentComboProductId,
                                                productId = tempProd.productId,
                                                quantity = AvailableUnits,
                                                productPrice = tempProd.productPrice
                                            };
                                            categoryProductForComboLine.Add(catComboprod);
                                            currentCategoryQty += AvailableUnits;

                                            if (tempProd.quantity == AvailableUnits)
                                            {
                                                tempProd.quantity = 0;
                                            }
                                            else
                                            {
                                                tempProd.quantity = tempProd.quantity - AvailableUnits;
                                            }

                                            if (currentCategoryQty >= requiredCatQty)
                                                currentCategory = -1;
                                        }
                                    }
                                }
                                break;
                        }
                    }


                    if (bookingProductTrxLine != null)
                    {
                        try
                        {
                            POSUtils.SetLastActivityDateTime();
                            StopInActivityTimeoutTimer();
                            reservationBL.AddProduct(productId, price, 1, comboProductId, bookingProductTrxLine, productModifiersList, atbForCombo, categoryProductForComboLine);
                        }
                        finally
                        {
                            POSUtils.SetLastActivityDateTime();
                            StartInActivityTimeoutTimer();
                        }
                    }
                }
            }
            else
            {
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 944));

                //  log.LogMethodExit("Selected Combo Product has quantity 0.");
                //return;
            }
            this.Cursor = Cursors.Default;
            log.LogMethodExit();
        }

        private PurchasedProducts GetModifierDetails(int productId, string productType)
        {
            log.LogMethodEntry(productId, productType);
            this.Cursor = Cursors.WaitCursor;
            POSUtils.SetLastActivityDateTime();
            PurchasedProducts purchasedProducts = null;
            List<ProductModifiersDTO> productModifiersDTOList = GetProductModifierList(productId);
            if (productModifiersDTOList != null && productModifiersDTOList.Count > 0)
            {
                POSUtils.SetLastActivityDateTime();
                using (FrmProductModifier frmProductModifier = new FrmProductModifier(productId, productType, utilities, null))
                {
                    POSUtils.SetLastActivityDateTime();
                    if (frmProductModifier.ShowDialog() == DialogResult.OK)
                    {
                        POSUtils.SetLastActivityDateTime();
                        this.Cursor = Cursors.WaitCursor;
                        Transaction trx = new Transaction();
                        frmProductModifier.transactionModifier.purchasedProducts.PurchasedModifierSetDTOList = trx.LoadSelectedModifiers(frmProductModifier.transactionModifier.ModifierSetDTO);
                        purchasedProducts = frmProductModifier.transactionModifier.purchasedProducts;
                    }

                }
            }
            log.LogMethodExit(purchasedProducts);
            this.Cursor = Cursors.Default;
            return purchasedProducts;
        }

        private List<ProductModifiersDTO> GetProductModifierList(int productId)
        {
            log.LogMethodEntry(productId);
            List<ProductModifiersDTO> returnProductModifiersDTOList = new List<ProductModifiersDTO>();
            try
            {
                POSUtils.SetLastActivityDateTime();
                StopInActivityTimeoutTimer();
                if (productModifierDictonary.ContainsKey(productId) == false)
                {
                    ProductModifiersList productModifiersListBL = new ProductModifiersList(executionContext);
                    List<KeyValuePair<ProductModifiersDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ProductModifiersDTO.SearchByParameters, string>>();
                    searchParameters.Add(new KeyValuePair<ProductModifiersDTO.SearchByParameters, string>(ProductModifiersDTO.SearchByParameters.PRODUCT_ID, productId.ToString()));
                    searchParameters.Add(new KeyValuePair<ProductModifiersDTO.SearchByParameters, string>(ProductModifiersDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                    List<ProductModifiersDTO> productModifiersDTOList = productModifiersListBL.GetAllProductModifiersList(searchParameters);
                    productModifierDictonary.Add(productId, productModifiersDTOList);
                }
                if (productModifierDictonary[productId] != null && productModifierDictonary[productId].Any())
                {
                    returnProductModifiersDTOList = new List<ProductModifiersDTO>(productModifierDictonary[productId]);
                }
            }
            finally
            {
                POSUtils.SetLastActivityDateTime();
                StartInActivityTimeoutTimer();
            }
            log.LogMethodExit();
            return returnProductModifiersDTOList;
        }

        private List<ComboProductDTO> GetComboProductList(int productId)
        {
            log.LogMethodEntry(productId);
            List<ComboProductDTO> returnComboProductDTOList = new List<ComboProductDTO>();
            try
            {
                POSUtils.SetLastActivityDateTime();
                StopInActivityTimeoutTimer();
                if (comboProductDictonary.ContainsKey(productId) == false)
                {
                    ComboProductList comboProductList = new ComboProductList(executionContext);
                    List<KeyValuePair<ComboProductDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ComboProductDTO.SearchByParameters, string>>();
                    searchParameters.Add(new KeyValuePair<ComboProductDTO.SearchByParameters, string>(ComboProductDTO.SearchByParameters.PRODUCT_ID, productId.ToString()));
                    searchParameters.Add(new KeyValuePair<ComboProductDTO.SearchByParameters, string>(ComboProductDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                    //searchParameters.Add(new KeyValuePair<ComboProductDTO.SearchByParameters, string>(ComboProductDTO.SearchByParameters.CHILD_PRODUCT_TYPE, ProductTypeValues.ATTRACTION));
                    List<ComboProductDTO> comboProductDTOList = comboProductList.GetComboProductDTOList(searchParameters);
                    comboProductDictonary.Add(productId, comboProductDTOList);
                }
                if (comboProductDictonary[productId] != null && comboProductDictonary[productId].Any())
                {
                    returnComboProductDTOList = new List<ComboProductDTO>(comboProductDictonary[productId]);
                }
            }
            finally
            {
                POSUtils.SetLastActivityDateTime();
                StartInActivityTimeoutTimer();
            }
            log.LogMethodExit();
            return returnComboProductDTOList;
        }

        private List<CategoryDTO> GetCategoryDTOList(int categoryId)
        {
            log.LogMethodEntry(categoryId);
            List<CategoryDTO> returnCategoryDTOList = new List<CategoryDTO>();
            try
            {
                POSUtils.SetLastActivityDateTime();
                StopInActivityTimeoutTimer();
                if (categoryDictonary.ContainsKey(categoryId) == false)
                {

                    CategoryList categoryList = new CategoryList(executionContext);
                    List<KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>> searchCatParas = new List<KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>>();
                    searchCatParas.Add(new KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>(CategoryDTO.SearchByCategoryParameters.CATEGORY_ID, categoryId.ToString()));
                    searchCatParas.Add(new KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>(CategoryDTO.SearchByCategoryParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                    searchCatParas.Add(new KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>(CategoryDTO.SearchByCategoryParameters.IS_ACTIVE, "Y"));
                    List<CategoryDTO> categoryDTOList = categoryList.GetAllCategory(searchCatParas);
                    categoryDictonary.Add(categoryId, categoryDTOList);
                }
                if (categoryDictonary[categoryId] != null && categoryDictonary[categoryId].Any())
                {
                    returnCategoryDTOList = new List<CategoryDTO>(categoryDictonary[categoryId]);
                }
            }
            finally
            {
                POSUtils.SetLastActivityDateTime();
                StartInActivityTimeoutTimer();
            }
            log.LogMethodExit();
            return returnCategoryDTOList;
        }
        private List<Transaction.TransactionLine> RefreshProductTransactionLines(int productId, int comboProductId)
        {
            log.LogMethodEntry(productId, comboProductId);
            this.Cursor = Cursors.WaitCursor;
            List<Transaction.TransactionLine> transactionLines = null;
            if (reservationBL.ReservationTransactionIsNotNull())
            {
                transactionLines = reservationBL.GetReservationProductTransactionLines(productId, comboProductId);
            }
            log.LogMethodExit(transactionLines);
            this.Cursor = Cursors.Default;
            SetFormOnFocus();
            return transactionLines;
        }

        private void LoadAdditionalProductDetails()
        {
            log.LogMethodEntry();
            try
            {
                this.Cursor = Cursors.WaitCursor;
                POSUtils.SetLastActivityDateTime();
                StopInActivityTimeoutTimer();
                txtMessage.Clear();
                if (reservationBL.ReservationTransactionIsNotNull())
                {
                    Products bookingProduct = reservationBL.GetBookingProduct();
                    int guestQty = reservationBL.GetGuestQuantity();
                    if (bookingProduct != null)
                    {
                        List<ComboProductDTO> additionalProductDTOList = bookingProduct.GetComboAdditionalProductSetup(false);
                        List<Transaction.TransactionLine> additionalProductTrxLines = reservationBL.GetPurchasedAdditionalProducts();
                        if (additionalProductDTOList != null && additionalProductDTOList.Count > 0)
                        {
                            ClearPnlAdditionalProducts();
                            this.usrCtrlAdditionProductDetails1.Visible = false;
                            int locationX = this.usrCtrlAdditionProductDetails1.Location.X;
                            int locationY = this.usrCtrlAdditionProductDetails1.Location.Y;
                            int counter = 0;
                            for (int i = 0; i < additionalProductDTOList.Count; i++)
                            {
                                List<Transaction.TransactionLine> productTrxLines = null;
                                if (additionalProductTrxLines != null && additionalProductTrxLines.Count > 0)
                                {
                                    productTrxLines = GetLinesAndLinkedChildLines(additionalProductTrxLines, additionalProductDTOList[i].ChildProductId, additionalProductDTOList[i].ComboProductId);
                                }
                                if (additionalProductDTOList[i].IsActive == false && (productTrxLines == null || productTrxLines.Any() == false))
                                {
                                    //skip inactive combo with no assigned product lines
                                    continue;
                                }
                                usrCtrlProductDetails usrCtrlProductDetailsEntry = new usrCtrlProductDetails(additionalProductDTOList[i], productTrxLines, 0, reservationBL.GetReservationDTO.Status);
                                usrCtrlProductDetailsEntry.CancelProductLine += new usrCtrlProductDetails.CancelProductLineDelegate(CancelProductLine);
                                usrCtrlProductDetailsEntry.RefreshTransactionLines += new usrCtrlProductDetails.RefreshTransactionLinesDelegate(RefreshProductTransactionLines);
                                usrCtrlProductDetailsEntry.AddProductToTransaction += new usrCtrlProductDetails.AddProductToTransactionDelegate(AddProductToBookingTransaction);
                                usrCtrlProductDetailsEntry.GetTrxProfile += new usrCtrlProductDetails.GetTrxProfileDelegate(GetTrxProfile);
                                usrCtrlProductDetailsEntry.SetDiscounts += new usrCtrlProductDetails.SetDiscountsDelegate(SetDiscounts);
                                usrCtrlProductDetailsEntry.GetTrxLineIndex += new usrCtrlProductDetails.GetTrxLineIndexDelegate(GetTrxLineIndex);
                                usrCtrlProductDetailsEntry.EditModifiers += new usrCtrlProductDetails.EditModifiersDelegate(EditModifiers);
                                usrCtrlProductDetailsEntry.RescheduleAttraction += new usrCtrlProductDetails.RescheduleAttractionDelegate(RescheduleAttraction);
                                usrCtrlProductDetailsEntry.RescheduleAttractionGroup += new usrCtrlProductDetails.RescheduleAttractionGroupDelegate(RescheduleAttractionGroup);
                                usrCtrlProductDetailsEntry.ChangePrice += new usrCtrlProductDetails.ChangePriceDelegate(ChangePrice);
                                usrCtrlProductDetailsEntry.ResetPrice += new usrCtrlProductDetails.ResetPriceDelegate(ResetPrice);

                                usrCtrlProductDetailsEntry.Name = "usrCtrlAdditionProductDetails0" + counter.ToString();
                                pnlAdditionalProducts.Controls.Add(usrCtrlProductDetailsEntry);
                                usrCtrlProductDetailsEntry.Location = new Point(locationX, locationY);
                                usrCtrlProductDetailsEntry.Width = 785;
                                locationY = locationY + 44;

                            }
                        }
                        else
                        {
                            ClearPnlAdditionalProducts();
                            this.usrCtrlAdditionProductDetails1.Enabled = false;
                        }
                    }
                }
                UpdateUserControlUIElements();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                txtMessage.Text = ex.Message;
            }
            finally
            {
                this.Cursor = Cursors.Default;
                POSUtils.SetLastActivityDateTime();
                StartInActivityTimeoutTimer();
            }
            log.LogMethodExit();
        }

        private void LoadSummaryDetails()
        {
            log.LogMethodEntry();
            this.Cursor = Cursors.WaitCursor;
            try
            {
                POSUtils.SetLastActivityDateTime();
                StopInActivityTimeoutTimer();
                txtMessage.Clear();
                if (reservationBL.ReservationTransactionIsNotNull())
                {
                    if (BookingIsInEditMode())
                    {
                        try
                        {
                            if (autoChargeOptionEnabled)
                            {
                                reservationBL.AutoApplyCharges();
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                            txtMessage.Text = ex.Message;
                        }
                    }
                    txtReservationCode.Text = reservationBL.GetReservationDTO.ReservationCode;
                    txtReservationStatus.Text = reservationBL.GetReservationDTO.Status;
                    txtExpiryDate.Text = (reservationBL.GetReservationDTO.ExpiryTime == null ? "" : Convert.ToDateTime(reservationBL.GetReservationDTO.ExpiryTime).ToString(ParafaitEnv.DATETIME_FORMAT));
                    double serviceCharge = reservationBL.GetServiceChargeAmount();
                    if (autoChargeOptionEnabled)
                    {
                        double gratuityAmount = reservationBL.GetAutoGratuityAmount();
                        txtServiceChrageAmount.Text = serviceCharge.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
                        txtGratuityAmount.Text = gratuityAmount.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
                    }
                    else
                    {
                        txtSvcTrxServiceCharges.Text = serviceCharge.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
                        txtSvcServiceChargeAmount.Text = (reservationBL.GetReservationDTO.ServiceChargeAmount == 0 ? string.Empty : reservationBL.GetReservationDTO.ServiceChargeAmount.ToString());
                        txtSvcServiceChargePercentage.Text = (reservationBL.GetReservationDTO.ServiceChargePercentage == 0 ? string.Empty : reservationBL.GetReservationDTO.ServiceChargePercentage.ToString());
                    }
                    txtTransactionAmount.Text = reservationBL.BookingTransaction.Transaction_Amount.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);

                    decimal advanceRequired = reservationBL.GetAdvanceRequired();
                    txtMinimumAdvanceAmount.Text = (advanceRequired > 0 ? advanceRequired.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL) : "");
                    txtEstimateAmount.Text = reservationBL.BookingTransaction.Net_Transaction_Amount.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
                    txtDiscountAmount.Text = reservationBL.BookingTransaction.Discount_Amount.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
                    txtAdvancePaid.Text = reservationBL.BookingTransaction.TotalPaidAmount.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
                    txtBalanceAmount.Text = Math.Round(reservationBL.BookingTransaction.Net_Transaction_Amount - reservationBL.BookingTransaction.TotalPaidAmount, ParafaitEnv.RoundingPrecision, MidpointRounding.AwayFromZero).ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
                    DiscountContainerDTO discountCouponDTO = reservationBL.GetAppliedDiscountCouponForBooking();
                    if (discountCouponDTO != null)
                    {
                        txtAppliedDiscountCoupon.Text = discountCouponDTO.DiscountName;
                        txtAppliedDiscountCoupon.Tag = discountCouponDTO.DiscountId;
                    }
                    else
                    {
                        txtAppliedDiscountCoupon.Text = string.Empty;
                        txtAppliedDiscountCoupon.Tag = "-1";
                    }
                    DiscountContainerDTO discountContainerIDDTO = reservationBL.GetAppliedDiscountForBooking();
                    if (discountContainerIDDTO != null)
                    {
                        txtAppliedDiscount.Text = discountContainerIDDTO.DiscountName;
                        txtAppliedDiscount.Tag = discountContainerIDDTO.DiscountId;
                    }
                    else
                    {
                        txtAppliedDiscount.Text = string.Empty;
                        txtAppliedDiscount.Tag = "-1";
                    }


                    cbxEmailSent.Checked = (reservationBL.GetReservationDTO.IsEmailSent > 0);
                    EnableDisableChargeButtons();
                    SetTransactionProfileName();
                    if (BookingIsInEditMode() && autoChargeOptionEnabled == false)
                    {
                        txtMessage.Text = MessageContainerList.GetMessage(executionContext, 2099);
                        // "Service charge amount details gets refreshed (if applicable) once booking changes are saved"
                    }
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
                POSUtils.SetLastActivityDateTime();
                StartInActivityTimeoutTimer();
            }
            log.LogMethodExit();
        }

        private void ResetSummaryDetails()
        {
            log.LogMethodEntry();
            this.Cursor = Cursors.WaitCursor;
            txtReservationCode.Text = string.Empty;
            txtReservationStatus.Text = ReservationDTO.ReservationStatus.NEW.ToString();
            txtExpiryDate.Text = string.Empty;
            txtSvcTrxServiceCharges.Text = string.Empty;
            txtTransactionAmount.Text = string.Empty;
            txtMinimumAdvanceAmount.Text = string.Empty;
            txtEstimateAmount.Text = string.Empty;
            txtDiscountAmount.Text = string.Empty;
            txtAdvancePaid.Text = string.Empty;
            txtBalanceAmount.Text = string.Empty;
            txtAppliedDiscountCoupon.Text = string.Empty;
            txtAppliedDiscountCoupon.Tag = "-1";
            txtAppliedDiscount.Text = string.Empty;
            txtAppliedDiscount.Tag = "-1";
            txtSvcServiceChargeAmount.Text = string.Empty;
            txtSvcServiceChargePercentage.Text = string.Empty;
            txtServiceChrageAmount.Text = string.Empty;
            txtGratuityAmount.Text = string.Empty;
            cbxEmailSent.Checked = false;
            txtAppliedTransactionProfile.Text = "";
            txtAppliedTransactionProfile.Tag = "-1";
            this.Cursor = Cursors.Default;
            log.LogMethodExit();
        }
        private void SetTransactionProfileName()
        {
            log.LogMethodEntry();
            List<TransactionProfileDTO> transactionProfileDTOList = null;
            if (reservationBL.ReservationTransactionIsNotNull() && reservationBL.BookingTransaction.TrxProfileId > -1)
            {
                TransactionProfileListBL transactionProfileListBL = new TransactionProfileListBL(utilities.ExecutionContext);
                List<KeyValuePair<TransactionProfileDTO.SearchByParameters, string>> searchParam = new List<KeyValuePair<TransactionProfileDTO.SearchByParameters, string>>
                {
                    new KeyValuePair<TransactionProfileDTO.SearchByParameters, string>(TransactionProfileDTO.SearchByParameters.IS_ACTIVE, "1"),
                    new KeyValuePair<TransactionProfileDTO.SearchByParameters, string>(TransactionProfileDTO.SearchByParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()),
                    new KeyValuePair<TransactionProfileDTO.SearchByParameters, string>(TransactionProfileDTO.SearchByParameters.TRANSACTION_PROFILE_ID, reservationBL.BookingTransaction.TrxProfileId.ToString())
                };
                transactionProfileDTOList = transactionProfileListBL.GetTransactionProfileDTOList(searchParam);
            }

            if (transactionProfileDTOList != null && transactionProfileDTOList.Count > 0)
            {
                txtAppliedTransactionProfile.Text = transactionProfileDTOList[0].ProfileName;
                txtAppliedTransactionProfile.Tag = reservationBL.BookingTransaction.TrxProfileId;
            }
            else
            {
                txtAppliedTransactionProfile.Text = "";
                txtAppliedTransactionProfile.Tag = "-1";
            }
            log.LogMethodExit();
        }

        private void LoadCheckListDetails()
        {
            log.LogMethodEntry();
            try
            {
                this.Cursor = Cursors.WaitCursor;
                POSUtils.SetLastActivityDateTime();
                StopInActivityTimeoutTimer();
                if (reservationBL.GetReservationDTO != null)
                {
                    if (reservationBL.GetReservationDTO.BookingCheckListDTOList != null && reservationBL.GetReservationDTO.BookingCheckListDTOList.Any())
                    {
                        userJobItemsDTOList = reservationBL.GetReservationCheckList();
                        LoadDgvUserJobTaskList();
                    }
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
                POSUtils.SetLastActivityDateTime();
                StartInActivityTimeoutTimer();
            }
            log.LogMethodExit();
        }

        private void LoadDgvUserJobTaskList()
        {
            log.LogMethodEntry();
            if (userJobItemsDTOList != null && userJobItemsDTOList.Any())
            {
                List<UserJobItemsDTO> selectCheckListJobs = new List<UserJobItemsDTO>();
                if (cmbEventCheckList.SelectedValue != null && (int)cmbEventCheckList.SelectedIndex > -1)
                {
                    selectCheckListJobs = userJobItemsDTOList.Where(uj => uj.BookingCheckListId == (int)cmbEventCheckList.SelectedValue && uj.BookingId == reservationBL.GetReservationDTO.BookingId).ToList();
                }
                dgvUserJobTaskList.DataSource = selectCheckListJobs;
            }
            log.LogMethodExit();
        }

        private void LoadEventCheckListDropDown()
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            List<BookingCheckListDTO> dropDownSourceList = new List<BookingCheckListDTO>();
            dropDownSourceList.Add(new BookingCheckListDTO());
            if (reservationBL.GetReservationDTO != null)
            {
                if (reservationBL.GetReservationDTO.BookingCheckListDTOList != null
                    && reservationBL.GetReservationDTO.BookingCheckListDTOList.Any())
                {
                    List<BookingCheckListDTO> activeList = reservationBL.GetReservationDTO.BookingCheckListDTOList.Where(bcl => bcl.IsActive == true).ToList();
                    if (activeList != null && activeList.Any())
                    {
                        dropDownSourceList.AddRange(new List<BookingCheckListDTO>(activeList));
                    }
                }
            }
            this.cmbEventCheckList.DataSource = dropDownSourceList;
            this.cmbEventCheckList.DisplayMember = "CheckListTaskGroupName";
            this.cmbEventCheckList.ValueMember = "BookingCheckListId";
            this.cmbEventCheckList.SelectedIndex = 0;
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }

        private void cmbEventCheckList_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            LoadDgvUserJobTaskList();
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }

        private void LoadAuditTrail()
        {
            log.LogMethodEntry();
            GetAuditTrail();
            log.LogMethodExit();
        }

        //Get the Booking audit details
        private void GetAuditTrail()
        {
            log.LogMethodEntry();
            try
            {
                this.Cursor = Cursors.WaitCursor;
                POSUtils.SetLastActivityDateTime();
                StopInActivityTimeoutTimer();
                DataTable auditTrail = null;
                if (reservationBL.GetReservationDTO != null && reservationBL.GetReservationDTO.BookingId > -1)
                {
                    auditTrail = utilities.executeDataTable(@"select Data as UserName,Timestamp,
                                                                Computer, Description from EventLog
                                                                where Source ='Reservation'
                                                                and value = @bookingGuId
                                                                order by EventLogId desc",
                                                                       new SqlParameter("@bookingGuId", reservationBL.GetReservationDTO.Guid.ToString()));
                }
                else
                {
                    auditTrail = utilities.executeDataTable(@"select * from (select null as UserName,getdate() as Timestamp, null as Computer, null as Description 
                                                                    ) as tbl where 0 = 1");
                }
                dgvAuditTrail.DataSource = auditTrail;
                dgvAuditTrail.Columns["Timestamp"].DefaultCellStyle = utilities.gridViewDateTimeCellStyle();
                SetDGVCellFont(dgvAuditTrail);
                dgvAuditTrail.RowHeadersVisible = false;
                //dgvAuditTrail.Columns["Description"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            finally
            {
                this.Cursor = Cursors.Default;
                POSUtils.SetLastActivityDateTime();
                StartInActivityTimeoutTimer();
            }
            log.LogMethodExit();
        }
        private void SetSelectedScheduleTime(List<TransactionReservationScheduleDTO> transactionReservationScheduleDTOList)
        {
            log.LogMethodEntry(transactionReservationScheduleDTOList);
            this.Cursor = Cursors.WaitCursor;
            if (transactionReservationScheduleDTOList != null && transactionReservationScheduleDTOList.Count > 0)
            {
                //ransactionReservationScheduleDTO minimumScheduleDTO = transactionReservationScheduleDTOList.Min(trs => trs.ScheduleFromDate);
                DateTime fromScheduletime = transactionReservationScheduleDTOList.Min(trs => trs.ScheduleFromDate);
                DateTime toScheduleTime = fromScheduletime.Date.AddDays(1);
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
                             where r.Field<DateTime>("Compare") >= fromScheduletime &&
                             r.Field<DateTime>("Compare") <= toScheduleTime
                             select r;

                //if (Convert.ToDateTime(selectedRow.Cells["From_Time"].Value).Hour > Convert.ToDateTime(selectedRow.Cells["To_Time"].Value).Hour)
                if (fromScheduletime.Hour > toScheduleTime.Hour)
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

                //DateTime currentTime = utilities.getServerTime(); 
                //cmbFromTime.SelectedValue = Convert.ToDouble(Convert.ToDateTime(selectedRow.Cells["From_Time"].Value).Hour + "." + Convert.ToDateTime(selectedRow.Cells["From_Time"].Value).Minute);
                //cmbToTime.SelectedValue = Convert.ToDouble(Convert.ToDateTime(selectedRow.Cells["To_Time"].Value).Hour + "." + Convert.ToDateTime(selectedRow.Cells["To_Time"].Value).Minute);
                //End
                //If schedule is fixed then disable from and to time
                // if (selectedRow.Cells["Fixed"].Value.ToString() == "Y")
                // { 
                //     cmbFromTime.Enabled = cmbToTime.Enabled = false; 
                //  }
                //  else
                // {
                //      cmbFromTime.Enabled = false;
                //      cmbFromTime.Enabled = cmbToTime.Enabled = true; 
                //  }
                //userAction = true;
            }
            this.Cursor = Cursors.Default;
            log.LogMethodExit();
        }


        private void GetSelectionDetails()
        {
            log.LogMethodEntry();
            this.Cursor = Cursors.WaitCursor;
            //SelectedScheduleNProductDetails selectedScheduleNProductDetails = new SelectedScheduleNProductDetails();
            if (dgvSearchFacilityProducts.Rows.Count > 0)
            {
                int selectedRowCount = dgvSearchSchedules.Rows.GetRowCount(DataGridViewElementStates.Selected);
                if (selectedRowCount > 0)
                {
                    if (selectedRowCount > 1)
                    {
                        this.ActiveControl = dgvSearchSchedules;
                        log.Info("More than one schedule selected");// "Please select one schedule entry"
                        throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, 1797));
                    }
                    else
                    {
                        selectedScheduleNProductDetails.scheduleId = Convert.ToInt32(dgvSearchSchedules.SelectedRows[0].Cells["scheduleIdDataGridViewTextBoxColumn"].Value);
                        selectedScheduleNProductDetails.scheduleName = dgvSearchSchedules.SelectedRows[0].Cells["scheduleNameDataGridViewTextBoxColumn"].Value.ToString();
                        selectedScheduleNProductDetails.facilityMapId = Convert.ToInt32(dgvSearchSchedules.SelectedRows[0].Cells["facilityMapIdDataGridViewTextBoxColumn"].Value);
                        selectedScheduleNProductDetails.facilityMapName = dgvSearchSchedules.SelectedRows[0].Cells["facilityMapNameDataGridViewTextBoxColumn"].Value.ToString();
                        selectedScheduleNProductDetails.fixedSchedule = Convert.ToBoolean(dgvSearchSchedules.SelectedRows[0].Cells["fixedScheduleDataGridViewCheckBoxColumn"].Value);
                        selectedScheduleNProductDetails.selectedScheduleDate = Convert.ToDateTime(dgvSearchSchedules.SelectedRows[0].Cells["ScheduleFromDate"].Value);
                        selectedScheduleNProductDetails.availableQuantity = Convert.ToInt32(dgvSearchSchedules.SelectedRows[0].Cells["availableUnitsDataGridViewTextBoxColumn"].Value);
                        //if (selectedScheduleNProductDetails.fixedSchedule)
                        //{
                        selectedScheduleNProductDetails.selectedFromTime = Convert.ToDecimal(dgvSearchSchedules.SelectedRows[0].Cells["scheduleFromTimeDataGridViewTextBoxColumn"].Value);
                        selectedScheduleNProductDetails.selectedToTime = Convert.ToDecimal(dgvSearchSchedules.SelectedRows[0].Cells["scheduleToTimeDataGridViewTextBoxColumn"].Value);
                        //}
                        //else
                        //{
                        //    decimal fromTime;
                        //    if (cmbSearchFromTime.SelectedValue == null
                        //        || decimal.TryParse(cmbSearchFromTime.SelectedValue.ToString(), out  fromTime) == false)
                        //    {
                        //        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2091));//"Please enter valid time in Search From Time field"
                        //    }
                        //    decimal toTime;
                        //    if (cmbSearchToTime.SelectedValue == null 
                        //        || decimal.TryParse(cmbSearchToTime.SelectedValue.ToString(), out toTime) == false)
                        //    {
                        //        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2092)); //"Please enter valid time in Search To Time field"
                        //    }
                        //    selectedScheduleNProductDetails.selectedFromTime = fromTime;
                        //    selectedScheduleNProductDetails.selectedToTime = toTime;
                        //}
                        selectedScheduleNProductDetails.facTotalUnits = Convert.ToInt32(dgvSearchSchedules.SelectedRows[0].Cells["totalUnitsDataGridViewTextBoxColumn"].Value);
                    }

                    if (selectedScheduleNProductDetails.scheduleId > -1)
                    {
                        if (dgvSearchFacilityProducts.Rows.Count > 0)
                        {
                            foreach (DataGridViewRow productRow in dgvSearchFacilityProducts.Rows)
                            {
                                if (productRow.Cells["SelectedRecord"].Value != null && productRow.Cells["SelectedRecord"].Value.ToString().ToLower() == "true")
                                {
                                    SelectedScheduleNProductDetails.ProductDetails productDetails = new SelectedScheduleNProductDetails.ProductDetails();
                                    productDetails.productId = Convert.ToInt32(productRow.Cells["productIdDataGridViewTextBoxColumn1"].Value);
                                    productDetails.productName = productRow.Cells["productNameDataGridViewTextBoxColumn1"].Value.ToString();
                                    productDetails.productType = productRow.Cells["ProductType"].Value.ToString();
                                    //if (productDetails.productType == ProductTypeValues.BOOKINGS)
                                    //{
                                    //    selectedScheduleNProductDetails.availableQuantity = Convert.ToInt32(productRow.Cells["availableUnitsDataGridViewTextBoxColumn1"].Value);
                                    //}
                                    selectedScheduleNProductDetails.selectedProducts.Add(productDetails);
                                }
                            }
                            if (selectedScheduleNProductDetails.selectedProducts.Count == 0)
                            {
                                this.ActiveControl = dgvSearchFacilityProducts;
                                log.Info("Products are not selected");
                                throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, 2100));//Please select a product
                            }
                            else
                            {
                                //selectedScheduleNProductDetails.selectedBookingProduct
                                List<SelectedScheduleNProductDetails.ProductDetails> bookingProductList = selectedScheduleNProductDetails.selectedProducts.Where(prod => prod.productType == ProductTypeValues.BOOKINGS).ToList();
                                List<SelectedScheduleNProductDetails.ProductDetails> rentalProductList = selectedScheduleNProductDetails.selectedProducts.Where(prod => prod.productType == ProductTypeValues.RENTAL).ToList();
                                if (rentalProductList == null || rentalProductList.Count == 0)
                                {
                                    this.ActiveControl = dgvSearchFacilityProducts;
                                    log.Info("Rental products is not selected");
                                    throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, 2101));// "Please select a rental product"
                                }
                                else if (rentalProductList.Count > 1)
                                {
                                    this.ActiveControl = dgvSearchFacilityProducts;
                                    log.Info("More than one rental products are selected");
                                    throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, 2102));//"Please select only one rental product"
                                }
                                else
                                {
                                    selectedScheduleNProductDetails.rentalProduct = rentalProductList[0];
                                }

                                if (bookingProductList != null && bookingProductList.Count > 1)
                                {
                                    this.ActiveControl = dgvSearchFacilityProducts;
                                    log.Info("More than one Booking products are selected");
                                    throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, 2103));// "Please select only one booking product"
                                }
                                else if (bookingProductList.Count == 0)
                                {
                                    try
                                    {
                                        reservationBL.HasBookingProduct();
                                    }
                                    catch (ValidationException ex)
                                    {
                                        log.Error(ex);
                                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1796));// "Please select a booking product"
                                    }
                                }
                                else
                                {
                                    selectedScheduleNProductDetails.bookingProduct = bookingProductList[0];
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                this.Cursor = Cursors.Default;
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Facility map is not having required product setup"));
            }
            this.Cursor = Cursors.Default;
            log.LogMethodExit(selectedScheduleNProductDetails);
            //return selectedScheduleNProductDetails;
        }

        private void dgvSelectedBookingSchedule_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            //cmbFromTime.SelectedValue = Convert.ToDouble(Convert.ToDateTime(selectedRow.Cells["From_Time"].Value).Hour + "." + Convert.ToDateTime(selectedRow.Cells["From_Time"].Value).Minute);
            //cmbToTime.SelectedValue = Convert.ToDouble(Convert.ToDateTime(selectedRow.Cells["To_Time"].Value).Hour + "." + Convert.ToDateTime(selectedRow.Cells["To_Time"].Value).Minute);
            if (dgvSelectedBookingSchedule.CurrentCell != null & e.Value != null)
            {
                if (dgvSelectedBookingSchedule.Columns[e.ColumnIndex].Name == "cmbFromTime" || dgvSelectedBookingSchedule.Columns[e.ColumnIndex].Name == "cmbToTime")
                {
                    decimal decimalCheckValue = 0;
                    if (decimal.TryParse(e.Value.ToString(), out decimalCheckValue))
                    {
                        string selectCondtion = "value = " + e.Value.ToString();
                        DataRow foundRows = GetMatchingRow(e.Value);
                        if (foundRows != null)
                            e.Value = foundRows["Display"];
                    }
                }
            }
            log.LogMethodExit();
        }

        private void dgvSelectedBookingSchedule_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (dgvSelectedBookingSchedule.CurrentCell != null & userAction && e.RowIndex > -1)
            {
                if (dgvSelectedBookingSchedule.Columns[e.ColumnIndex].Name == "cmbFromTime" || dgvSelectedBookingSchedule.Columns[e.ColumnIndex].Name == "cmbToTime")
                {
                    txtMessage.Clear();
                    bool fromTimeField = (dgvSelectedBookingSchedule.Columns[e.ColumnIndex].Name == "cmbFromTime");
                    if (ValidateUserSelectedTime(e, fromTimeField))
                    {
                        UpdateSchduleTime(e, fromTimeField);
                    }
                    else
                    {
                        RefreshSelectedBookingSchedule();
                        dgvSelectedBookingSchedule.CurrentCell = dgvSelectedBookingSchedule.Rows[e.RowIndex].Cells[e.ColumnIndex];

                    }
                }
                if (dgvSelectedBookingSchedule.Columns[e.ColumnIndex].Name == "guestQty")
                {
                    txtMessage.Clear();
                    UpdateScheduleQuantity(e);
                }
            }
            log.LogMethodExit();
        }

        private void UpdateScheduleQuantity(DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(e);
            this.Cursor = Cursors.WaitCursor;
            if (userAction)
            {
                //                dgvSelectedBookingSchedule.Rows[i].Cells["guestQty"].Value = dgvSelectedBookingSchedule.Rows[i].Cells["guestQuantityDataGridViewTextBoxColumn"].Value;
                int quantity = -1;
                try
                {
                    quantity = Convert.ToInt32(dgvSelectedBookingSchedule.Rows[e.RowIndex].Cells["guestQty"].Value);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
                Transaction.TransactionLine facilityTrxLine = (Transaction.TransactionLine)dgvSelectedBookingSchedule.Rows[e.RowIndex].Cells["FacilityTrxLine"].Value;
                if (quantity < 1)
                {
                    txtMessage.Text = MessageContainerList.GetMessage(executionContext, 2104);// "Please enter valid guest quantity"
                    POSUtils.ParafaitMessageBox(txtMessage.Text, MessageContainerList.GetMessage(executionContext, "Update Guest Count"));
                    if (facilityTrxLine != null && facilityTrxLine.TransactionReservationScheduleDTOList != null && facilityTrxLine.TransactionReservationScheduleDTOList.Any())
                    {
                        dgvSelectedBookingSchedule.Rows[e.RowIndex].Cells["guestQty"].Value = facilityTrxLine.GetCurrentTransactionReservationScheduleDTO().GuestQuantity;
                    }
                    else
                    {
                        dgvSelectedBookingSchedule.Rows[e.RowIndex].Cells["guestQty"].Value = 1;
                    }
                }
                else
                {
                    //Transaction.TransactionLine facilityTrxLine = (Transaction.TransactionLine)dgvSelectedBookingSchedule.Rows[e.RowIndex].Cells["FacilityTrxLine"].Value;
                    if (facilityTrxLine != null && facilityTrxLine.TransactionReservationScheduleDTOList != null && facilityTrxLine.TransactionReservationScheduleDTOList.Any())
                    {
                        if (quantity > -1 && facilityTrxLine.GetCurrentTransactionReservationScheduleDTO().GuestQuantity != quantity)
                        {
                            int lineId = reservationBL.BookingTransaction.TrxLines.IndexOf(facilityTrxLine);
                            int oldQuantity = facilityTrxLine.GetCurrentTransactionReservationScheduleDTO().GuestQuantity;
                            try
                            {
                                facilityTrxLine.GetCurrentTransactionReservationScheduleDTO().GuestQuantity = quantity;
                                reservationBL.ValidateScheduleNQuantity(-1, facilityTrxLine.GetCurrentTransactionReservationScheduleDTO(), lineId);
                                // reservationBL.BookingTransaction.TrxLines[lineId].TransactionReservationScheduleDTO.GuestQuantity = quantity;
                                txtGuestQty.Text = reservationBL.GetGuestQuantity().ToString(utilities.ParafaitEnv.NUMBER_FORMAT);
                            }
                            catch (ValidationException ex)
                            {
                                log.Error(ex);
                                POSUtils.ParafaitMessageBox(ex.GetAllValidationErrorMessages(), MessageContainerList.GetMessage(executionContext, "Update Guest Count"));
                                reservationBL.BookingTransaction.TrxLines[lineId].GetCurrentTransactionReservationScheduleDTO().GuestQuantity = oldQuantity;
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex);
                                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 1824, ex.Message), MessageContainerList.GetMessage(executionContext, "Update Guest Count"));
                                reservationBL.BookingTransaction.TrxLines[lineId].GetCurrentTransactionReservationScheduleDTO().GuestQuantity = oldQuantity;
                            }
                            LoadDGVSelectedBookingSchedule();
                            dgvSelectedBookingSchedule.CurrentCell = dgvSelectedBookingSchedule.Rows[e.RowIndex].Cells["guestQty"];
                        }
                    }
                }
            }
            this.Cursor = Cursors.Default;
            log.LogMethodExit();
        }

        private bool ValidateUserSelectedTime(DataGridViewCellEventArgs e, bool fromTimeField)
        {
            log.LogMethodEntry(e, fromTimeField);
            this.Cursor = Cursors.WaitCursor;
            bool validTimeValue = false;
            int trsScheduleId = Convert.ToInt32(dgvSelectedBookingSchedule.Rows[e.RowIndex].Cells["schedulesIdDataGridViewTextBoxColumn"].Value);
            SchedulesDTO schedulesDTO = schedulesDTOMasterList.Find(sch => sch.ScheduleId == trsScheduleId);
            decimal fromtimeValue = Convert.ToDecimal(dgvSelectedBookingSchedule.Rows[e.RowIndex].Cells["cmbFromTime"].Value);
            decimal toTimeValue = Convert.ToDecimal(dgvSelectedBookingSchedule.Rows[e.RowIndex].Cells["cmbToTime"].Value);

            if (fromTimeField)
            {
                validTimeValue = (schedulesDTO.ScheduleId == trsScheduleId && schedulesDTO.FixedSchedule == false && fromtimeValue >= schedulesDTO.ScheduleTime && fromtimeValue <= schedulesDTO.ScheduleToTime);
            }
            else
            {
                validTimeValue = (schedulesDTO.ScheduleId == trsScheduleId && schedulesDTO.FixedSchedule == false && toTimeValue >= schedulesDTO.ScheduleTime && toTimeValue <= schedulesDTO.ScheduleToTime);
            }
            if (validTimeValue)
            {
                if (fromtimeValue > toTimeValue)
                {
                    txtMessage.Text = MessageContainerList.GetMessage(executionContext, 305);
                }
            }
            else
            {
                string fromTimeString = "";
                string toTimeString = "";
                DataRow foundRows = GetMatchingRow(schedulesDTO.ScheduleTime);
                if (foundRows != null)
                    fromTimeString = foundRows["Display"].ToString();

                foundRows = GetMatchingRow(schedulesDTO.ScheduleToTime);
                if (foundRows != null)
                    toTimeString = foundRows["Display"].ToString();

                txtMessage.Text = MessageContainerList.GetMessage(executionContext, 2105, fromTimeString, toTimeString);//"Time should be with in " + fromTimeString + " and " + toTimeString
            }
            this.Cursor = Cursors.Default;
            log.LogMethodExit(validTimeValue);
            return validTimeValue;
        }

        private void UpdateSchduleTime(DataGridViewCellEventArgs e, bool fromTimeUpdate)
        {
            log.LogMethodEntry(e, fromTimeUpdate);
            this.Cursor = Cursors.WaitCursor;
            if (userAction)
            {
                string cellColumnName = "";
                if (fromTimeUpdate)
                    cellColumnName = "cmbFromTime";
                else
                    cellColumnName = "cmbToTime";

                DateTime oldDate;
                decimal timeValue = Convert.ToDecimal(dgvSelectedBookingSchedule.Rows[e.RowIndex].Cells[cellColumnName].Value);
                DateTime trsScheduleFromDate = Convert.ToDateTime(dgvSelectedBookingSchedule.Rows[e.RowIndex].Cells["scheduleFromDateDataGridViewTextBoxColumn"].Value);
                //int trsId = Convert.ToInt32(dgvSelectedBookingSchedule.Rows[e.RowIndex].Cells["trxReservationScheduleIdDataGridViewTextBoxColumn"].Value);
                //int trsTrxLineId = Convert.ToInt32(dgvSelectedBookingSchedule.Rows[e.RowIndex].Cells["lineIdDataGridViewTextBoxColumn"].Value);
                //int trsScheduleId = Convert.ToInt32(dgvSelectedBookingSchedule.Rows[e.RowIndex].Cells["schedulesIdDataGridViewTextBoxColumn"].Value);
                //int trsFacilityId = Convert.ToInt32(dgvSelectedBookingSchedule.Rows[e.RowIndex].Cells["facilityIdDataGridViewTextBoxColumn1"].Value);
                Transaction.TransactionLine facilityTrxLine = (Transaction.TransactionLine)dgvSelectedBookingSchedule.Rows[e.RowIndex].Cells["FacilityTrxLine"].Value;
                if (facilityTrxLine != null && facilityTrxLine.TransactionReservationScheduleDTOList != null && facilityTrxLine.TransactionReservationScheduleDTOList.Any())
                {
                    // if (fromTimeUpdate == true && facilityTrxLine.TransactionReservationScheduleDTO.ScheduleFromDate != facilityTrxLine.TransactionReservationScheduleDTO.ScheduleFromDate.Date.AddMinutes((int)timeValue * 60 + (double)timeValue % 1 * 100))
                    if (fromTimeUpdate == true && facilityTrxLine.GetCurrentTransactionReservationScheduleDTO().ScheduleFromDate != trsScheduleFromDate.Date.AddMinutes((int)timeValue * 60 + (double)timeValue % 1 * 100))
                    {
                        int lineId = reservationBL.BookingTransaction.TrxLines.IndexOf(facilityTrxLine);
                        //reservationBL.BookingTransaction.TrxLines[lineId].TransactionReservationScheduleDTO.ScheduleFromDate = facilityTrxLine.TransactionReservationScheduleDTO.ScheduleFromDate.Date.AddMinutes((int)timeValue * 60 + (double)timeValue % 1 * 100);
                        oldDate = reservationBL.BookingTransaction.TrxLines[lineId].GetCurrentTransactionReservationScheduleDTO().ScheduleFromDate;
                        reservationBL.BookingTransaction.TrxLines[lineId].GetCurrentTransactionReservationScheduleDTO().ScheduleFromDate = trsScheduleFromDate.Date.AddMinutes((int)timeValue * 60 + (double)timeValue % 1 * 100);
                        try
                        {
                            reservationBL.ValidateScheduleNQuantity(-1, reservationBL.BookingTransaction.TrxLines[lineId].GetCurrentTransactionReservationScheduleDTO(), lineId);
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                            txtMessage.Text = ex.Message;
                            reservationBL.BookingTransaction.TrxLines[lineId].GetCurrentTransactionReservationScheduleDTO().ScheduleFromDate = oldDate;
                        }
                        LoadDGVSelectedBookingSchedule();
                    }
                    //if (fromTimeUpdate == false && facilityTrxLine.TransactionReservationScheduleDTO.ScheduleToDate != facilityTrxLine.TransactionReservationScheduleDTO.ScheduleFromDate.Date.AddMinutes((int)timeValue * 60 + (double)timeValue % 1 * 100))
                    if (fromTimeUpdate == false && facilityTrxLine.GetCurrentTransactionReservationScheduleDTO().ScheduleToDate != trsScheduleFromDate.Date.AddMinutes((int)timeValue * 60 + (double)timeValue % 1 * 100))
                    {
                        int lineId = reservationBL.BookingTransaction.TrxLines.IndexOf(facilityTrxLine);
                        //reservationBL.BookingTransaction.TrxLines[lineId].TransactionReservationScheduleDTO.ScheduleToDate = facilityTrxLine.TransactionReservationScheduleDTO.ScheduleFromDate.Date.AddMinutes((int)timeValue * 60 + (double)timeValue % 1 * 100);
                        oldDate = reservationBL.BookingTransaction.TrxLines[lineId].GetCurrentTransactionReservationScheduleDTO().ScheduleToDate;
                        reservationBL.BookingTransaction.TrxLines[lineId].GetCurrentTransactionReservationScheduleDTO().ScheduleToDate = trsScheduleFromDate.Date.AddMinutes((int)timeValue * 60 + (double)timeValue % 1 * 100);
                        try
                        {
                            reservationBL.ValidateScheduleNQuantity(-1, reservationBL.BookingTransaction.TrxLines[lineId].GetCurrentTransactionReservationScheduleDTO(), lineId);
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                            txtMessage.Text = ex.Message;
                            reservationBL.BookingTransaction.TrxLines[lineId].GetCurrentTransactionReservationScheduleDTO().ScheduleToDate = oldDate;
                        }
                        LoadDGVSelectedBookingSchedule();
                    }
                }
            }
            this.Cursor = Cursors.Default;
            log.LogMethodExit();
        }

        private void dgvSelectedBookingSchedule_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (dgvSelectedBookingSchedule.CurrentCell != null)
            {
                if (dgvSelectedBookingSchedule.Columns[e.ColumnIndex].Name == "cmbFromTime" || dgvSelectedBookingSchedule.Columns[e.ColumnIndex].Name == "cmbToTime")
                {

                }
            }
            log.LogMethodExit();
        }

        private void dgvSelectedBookingSchedule_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                POSUtils.SetLastActivityDateTime();
                if (e.RowIndex > -1 && e.ColumnIndex > -1)
                {
                    if (dgvSelectedBookingSchedule.CurrentCell != null && dgvSelectedBookingSchedule.Columns[e.ColumnIndex].Name == "RemoveLine")
                    {
                        txtMessage.Clear();
                        if (BookingIsInEditMode())
                        {
                            // "Do you want to remove selected schedule and products?"
                            if (POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 2106), MessageContainerList.GetMessage(executionContext, "Remove"), MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                            {
                                if (dgvSelectedBookingSchedule.Rows[e.RowIndex].Cells["BookingProductTrxLine"].Value != null)
                                {
                                    Transaction.TransactionLine bookingProductTrxLine = (Transaction.TransactionLine)dgvSelectedBookingSchedule.Rows[e.RowIndex].Cells["BookingProductTrxLine"].Value;
                                    if (bookingProductTrxLine != null)
                                    {
                                        int lineId = reservationBL.BookingTransaction.TrxLines.IndexOf(bookingProductTrxLine);
                                        if (lineId == -1)
                                        {
                                            throw new ValidationException(MessageContainerList.GetMessage(this.executionContext, 2264));// "Opps something went wrong. Please close and reopen the booking"));
                                        }
                                        txtMessage.Text = MessageContainerList.GetMessage(executionContext, 1008);//Processing..Please wait...
                                        reservationBL.RemoveBookingProduct(bookingProductTrxLine.ProductID, lineId);
                                        txtMessage.Clear();
                                    }
                                }
                                if (dgvSelectedBookingSchedule.Rows[e.RowIndex].Cells["FacilityTrxLine"].Value != null)
                                {
                                    Transaction.TransactionLine facilityProductTrxLine = (Transaction.TransactionLine)dgvSelectedBookingSchedule.Rows[e.RowIndex].Cells["FacilityTrxLine"].Value;
                                    if (facilityProductTrxLine != null)
                                    {
                                        int lineId = reservationBL.BookingTransaction.TrxLines.IndexOf(facilityProductTrxLine);
                                        reservationBL.RemoveProduct(facilityProductTrxLine.ProductID, lineId);
                                    }
                                }
                                RefreshSelectedBookingSchedule();
                            }
                        }
                    }
                    if (dgvSelectedBookingSchedule.CurrentCell != null && dgvSelectedBookingSchedule.Columns[e.ColumnIndex].Name == "Reschedule")
                    {
                        txtMessage.Clear();
                        if (BookingIsInEditMode() && dgvSelectedBookingSchedule.Rows[e.RowIndex].Cells["BookingProductTrxLine"].Value != null)
                        {
                            if (reservationBL.GetReservationDTO.Status != ReservationDTO.ReservationStatus.NEW.ToString())
                            {
                                if (reservationBL.BookingTransaction != null
                                    && reservationBL.BookingTransaction.TrxLines.Exists(tl => tl.DBLineId == 0 && tl.LineValid && tl.CancelledLine == false) == false)
                                {
                                    reservationBL.BookingTransaction.ReservationRescheduleExceptionChecks();
                                    frmRescheduleReservationUI frmRescheduleReservationUI = new frmRescheduleReservationUI(executionContext, utilities, reservationBL);
                                    if (frmRescheduleReservationUI.ShowDialog() == DialogResult.OK)
                                    {
                                        this.Cursor = Cursors.WaitCursor;
                                        LoadReservationBL(reservationBL.GetReservationDTO.BookingId);
                                        if (this.reservationBL.GetReservationDTO.Status == ReservationDTO.ReservationStatus.CONFIRMED.ToString())
                                        {
                                            this.Cursor = Cursors.WaitCursor;
                                            UpdateUIElements();
                                            MoveToSummaryTab();
                                        }
                                        else
                                        {
                                            txtMessage.Text = MessageContainerList.GetMessage(executionContext, 2717);
                                            //'Please proceed with pending edits or go to summary tab to book/confirm the reservation'
                                        }
                                    }
                                    this.Cursor = Cursors.WaitCursor;
                                    RefreshSelectedBookingSchedule();
                                }
                                else
                                {
                                    txtMessage.Text = MessageContainerList.GetMessage(executionContext, 2127);
                                    //'There are unsaved records, Please save the booking first
                                }
                            }
                            else
                            {
                                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2400));
                                //Please save the booking first
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                txtMessage.Text = ex.Message;
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }


        private void dgvSelectedBookingSchedule_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            //if (dgvSelectedBookingSchedule.CurrentCell != null && dgvSelectedBookingSchedule.Columns[e.ColumnIndex].Name == "guestQty"
            //    && dgvSelectedBookingSchedule.Columns[e.ColumnIndex].ReadOnly == true)
            //{
            //    // dgvSelectedBookingSchedule.Columns[e.ColumnIndex].ReadOnly = false;
            //}
            log.LogMethodExit();
        }

        private void btnNextDateTime_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            POSUtils.SetLastActivityDateTime();
            txtMessage.Clear();
            try
            {
                ValidateDateTimePageData();
                tcBooking.SelectedTab = tpCustomer;
            }
            catch (ValidationException ex)
            {
                log.Error(ex);
                txtMessage.Text = ex.GetAllValidationErrorMessages();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                txtMessage.Text = ex.Message;
            }
            this.Cursor = Cursors.Default;
            log.LogMethodExit();
        }

        private void ValidateDateTimePageData()
        {
            log.LogMethodEntry();
            this.Cursor = Cursors.WaitCursor;
            if (BookingIsInEditMode())
            {
                CheckForInitialMandatoryFields();
                reservationBL.HasBookingProduct();
                reservationBL.HasValidSchedule();
            }
            this.Cursor = Cursors.Default;
            log.LogMethodExit();
        }

        private void txtCardNumber_Validating(object sender, CancelEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ValidateCard();
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }

        private void txtBookingName_Validating(object sender, CancelEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                txtMessage.Clear();
                if (BookingIsInEditMode() && string.IsNullOrEmpty(txtBookingName.Text.Trim()) == false && reservationBL.GetReservationDTO != null)
                {
                    if (reservationBL.GetReservationDTO.BookingName != txtBookingName.Text.Trim())
                    {
                        ValidateBookingName();
                        reservationBL.GetReservationDTO.BookingName = txtBookingName.Text.Trim();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                txtMessage.Text = ex.Message;
                txtBookingName.Focus();
            }
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }


        private void txtRemarks_Leave(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                txtMessage.Clear();
                if (BookingIsInEditMode() && string.IsNullOrEmpty(txtRemarks.Text.Trim()) == false)
                {
                    ValidateBookingRemarks();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                txtMessage.Text = ex.Message;
                txtRemarks.Focus();
            }
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }

        private void cmbChannel_SelectedValueChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (BookingIsInEditMode() && cmbChannel.SelectedItem != null && reservationBL.GetReservationDTO != null)
            {
                if (reservationBL.GetReservationDTO.Channel != cmbChannel.SelectedItem.ToString())
                    reservationBL.GetReservationDTO.Channel = cmbChannel.SelectedItem.ToString();
            }
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }

        private void tpDateTime_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                POSUtils.SetLastActivityDateTime();
                this.Cursor = Cursors.Default;
                RefreshSelectedBookingSchedule();
                POSUtils.SetLastActivityDateTime();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void tpCustomer_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                POSUtils.SetLastActivityDateTime();
                LoadCustomerNCardDetails();
                POSUtils.SetLastActivityDateTime();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void tpPackageProducts_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                POSUtils.SetLastActivityDateTime();
                this.Cursor = Cursors.WaitCursor;
                LoadPackageDetails();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            POSUtils.SetLastActivityDateTime();
            this.Cursor = Cursors.Default;
            log.LogMethodExit();
        }

        private void tpAdditionalProducts_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                POSUtils.SetLastActivityDateTime();
                this.Cursor = Cursors.WaitCursor;
                LoadAdditionalProductDetails();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            POSUtils.SetLastActivityDateTime();
            this.Cursor = Cursors.Default;
            log.LogMethodExit();
        }

        private void tpSummary_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                POSUtils.SetLastActivityDateTime();
                this.Cursor = Cursors.WaitCursor;
                LoadSummaryDetails();
                //this.ActiveControl = this.txtReservationCode;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            this.txtReservationCode.Focus();
            this.Cursor = Cursors.Default;
            log.LogMethodExit();
        }
        private void tpCheckList_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                POSUtils.SetLastActivityDateTime();
                this.Cursor = Cursors.WaitCursor;
                LoadCheckListDetails();
                if (userJobItemsDTOList != null && userJobItemsDTOList.Any())
                {
                    try
                    {
                        if (cmbEventCheckList.SelectedIndex == 0)
                        {
                            cmbEventCheckList.SelectedIndex = 1;
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            this.Cursor = Cursors.Default;
            log.LogMethodExit();
        }
        private void tpAuditDetails_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                POSUtils.SetLastActivityDateTime();
                this.Cursor = Cursors.WaitCursor;
                LoadAuditTrail();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            this.Cursor = Cursors.Default;
            log.LogMethodExit();
        }
        private void btnBlockSchedule_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                txtMessage.Clear();
                this.Cursor = Cursors.WaitCursor;
                POSUtils.SetLastActivityDateTime();
                StopInActivityTimeoutTimer();
                if (BookingIsInEditMode())
                {
                    try
                    {
                        if (reservationBL.GetReservationDTO != null
                            && (reservationBL.GetReservationDTO.Status == ReservationDTO.ReservationStatus.NEW.ToString()
                            || reservationBL.GetReservationDTO.Status == ReservationDTO.ReservationStatus.BLOCKED.ToString())
                            )
                        {
                            ValidateDateTimePageData();
                            int bookingId = reservationBL.GetReservationDTO.BookingId;
                            DateTime? expiryTime = reservationBL.GetReservationDTO.ExpiryTime;
                            string bookingStatus = reservationBL.GetReservationDTO.Status;
                            try
                            {
                                reservationBL.BlockReservationSchedule();
                                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 1810, reservationBL.GetReservationDTO.ReservationCode, (reservationBL.GetReservationDTO.ExpiryTime != null ? Convert.ToDateTime(reservationBL.GetReservationDTO.ExpiryTime).ToString(utilities.getDateTimeFormat()) : "")));
                            }
                            catch (ValidationException ex)
                            {
                                log.Error(ex);
                                txtMessage.Text = ex.GetAllValidationErrorMessages();
                                throw;
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex);
                                txtMessage.Text = ex.Message;
                                if (bookingId == -1)
                                {
                                    reservationBL.RestoreReservationDTOFromBlockedStateToNew(expiryTime, bookingStatus);
                                }
                                else
                                {
                                    LoadReservationBL(bookingId);
                                }
                                throw;
                            }
                        }
                        else
                        {
                            txtMessage.Text = MessageContainerList.GetMessage(executionContext, 2228);//Block is allowed only when booking is in New or Blocked status
                        }
                    }
                    catch (ValidationException ex)
                    {
                        log.Error(ex);
                        txtMessage.Text = ex.GetAllValidationErrorMessages();
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        txtMessage.Text = ex.Message;
                    }
                }
                else
                {
                    txtMessage.Text = MessageContainerList.GetMessage(executionContext, 2228);//Block is allowed only when booking is in New or Blocked status
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            finally
            {
                this.Cursor = Cursors.Default;
                POSUtils.SetLastActivityDateTime();
                StartInActivityTimeoutTimer();
            }
            log.LogMethodExit();
        }

        private void btnClearBooking_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                POSUtils.SetLastActivityDateTime();
                this.Cursor = Cursors.WaitCursor;
                txtMessage.Clear();
                int bookingId = -1;
                // "Are you sure you want to clear unsaved data?"
                if (reservationBL.GetReservationDTO != null && POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 2107), MessageContainerList.GetMessage(executionContext, "Clear"), MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    try
                    {
                        POSUtils.SetLastActivityDateTime();
                        StopInActivityTimeoutTimer();
                        if (reservationBL.GetReservationDTO != null)
                        {
                            bookingId = reservationBL.GetReservationDTO.BookingId;
                            LoadReservationBL(bookingId);
                            SetDefaultSearchDateParameters(ServerDateTime.Now.Date, (decimal)ServerDateTime.Now.Hour);
                            LoadSchedules();
                            LoadBookingDetails();
                            if (bookingId == -1)
                            {
                                LoadCustomerUIPanel();
                                ClearNAdjustPnlPackageProducts();
                                ClearPnlAdditionalProducts();
                            }
                        }
                    }
                    finally
                    {
                        this.Cursor = Cursors.WaitCursor;
                        StartInActivityTimeoutTimer();
                    }
                }
                RefreshSelectedBookingSchedule();
                UpdateUIElements();
            }
            catch (ValidationException ex)
            {
                log.Error(ex);
                txtMessage.Text = ex.GetAllValidationErrorMessages();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                txtMessage.Text = ex.Message;
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }



        private void btnPrintBooking_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                POSUtils.SetLastActivityDateTime();
                this.Cursor = Cursors.WaitCursor;
                txtMessage.Clear();
                if (this.reservationBL.GetReservationDTO != null && this.reservationBL.GetReservationDTO.BookingId > -1)
                {
                    PerformFiscaliation();
                    POSUtils.SetLastActivityDateTime();
                    string reportFileName = string.Empty;
                    string reportType = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "PARTY_BOOKING_PRINT_TYPE");
                    if (reportType.ToLower() == "receipt")
                    {
                        GetBookingReceiptReport(false);
                    }
                    else
                    {
                        reportFileName = GetBookingQuoteReport(false);
                        if (string.IsNullOrEmpty(reportFileName))
                        {
                            txtMessage.Text = MessageContainerList.GetMessage(executionContext, 2110);// "Error while generating report file"
                            this.Cursor = Cursors.Default;
                            log.LogMethodExit("Error while generating report file");
                            return;
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 1819, ex.Message)); //Error while printing the document. Error message: &1
            }
            SetFormOnFocus();
            this.Cursor = Cursors.Default;
            log.LogMethodExit();
        }

        private string GetBookingQuoteReport(bool backgroundMode)
        {
            log.LogMethodEntry();
            string reportFileName = string.Empty;
            try
            {
                this.Cursor = Cursors.WaitCursor;
                POSUtils.SetLastActivityDateTime();
                StopInActivityTimeoutTimer();
                if (this.reservationBL.GetReservationDTO != null && this.reservationBL.GetReservationDTO.BookingId > -1)
                {
                    List<clsReportParameters.SelectedParameterValue> reportParam = new List<clsReportParameters.SelectedParameterValue>();
                    reportParam.Add(new clsReportParameters.SelectedParameterValue("BookingId", this.reservationBL.GetReservationDTO.BookingId));
                    ReceiptReports receiptReports = new ReceiptReports(executionContext, "BookingReceipt", "", null, null, reportParam, "P");
                    reportFileName = receiptReports.GenerateAndPrintReport(backgroundMode);
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
                StartInActivityTimeoutTimer();
            }
            log.LogMethodExit(reportFileName);
            return reportFileName;
        }


        private void GetBookingReceiptReport(bool backgroundMode)
        {
            log.LogMethodEntry();
            string reportFileName = string.Empty;
            try
            {
                this.Cursor = Cursors.WaitCursor;
                POSUtils.SetLastActivityDateTime();
                StopInActivityTimeoutTimer();
                List<clsReportParameters.SelectedParameterValue> reportParam = new List<clsReportParameters.SelectedParameterValue>();
                reportParam.Add(new clsReportParameters.SelectedParameterValue("BookingId", this.reservationBL.GetReservationDTO.BookingId));
                ReceiptReports receiptReports = new ReceiptReports(executionContext, "BookingReceiptrprint", "", null, null, reportParam, "P");
                receiptReports.PrintReceiptReport();
            }
            finally
            {
                this.Cursor = Cursors.Default;
                StartInActivityTimeoutTimer();
            }
            log.LogMethodExit();
        }

        private string GetBookingPrintReport(bool backgroundMode)
        {
            log.LogMethodEntry();
            string reportFileName = string.Empty;
            try
            {
                POSUtils.SetLastActivityDateTime();
                StopInActivityTimeoutTimer();
                string reportType = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "PARTY_BOOKING_PRINT_TYPE");
                if (reportType == "RECEIPT")
                {
                    GetBookingReceiptReport(backgroundMode);
                }
                else
                {
                    reportFileName = GetBookingQuoteReport(backgroundMode);
                }
            }
            finally
            {
                POSUtils.SetLastActivityDateTime();
                StartInActivityTimeoutTimer();
            }
            log.LogMethodExit(reportFileName);
            return reportFileName;
        }

        private bool SetupThePrinting(PrintDocument MyPrintDocument, string docName)
        {
            log.LogMethodEntry(MyPrintDocument, docName);
            POSUtils.SetLastActivityDateTime();
            PrintDialog MyPrintDialog = new PrintDialog();
            MyPrintDialog.AllowCurrentPage = false;
            MyPrintDialog.AllowPrintToFile = false;
            MyPrintDialog.AllowSelection = false;
            MyPrintDialog.AllowSomePages = false;
            MyPrintDialog.PrintToFile = false;
            MyPrintDialog.ShowHelp = false;
            MyPrintDialog.ShowNetwork = false;
            MyPrintDialog.PrinterSettings.DefaultPageSettings.Landscape = false;
            string reservationCode = "";
            if (this.reservationBL.GetReservationDTO != null)
            {
                reservationCode = this.reservationBL.GetReservationDTO.ReservationCode;
            }
            MyPrintDocument.DocumentName = docName + " " + reservationCode;
            MyPrintDialog.UseEXDialog = true;
            POSUtils.SetLastActivityDateTime();
            if (MyPrintDialog.ShowDialog() != DialogResult.OK)
            {
                log.LogMethodExit("Print dialog was cancelled");
                return false;
            }

            MyPrintDocument.DocumentName = docName + " " + reservationCode;
            MyPrintDocument.PrinterSettings = MyPrintDialog.PrinterSettings;
            MyPrintDocument.DefaultPageSettings = MyPrintDialog.PrinterSettings.DefaultPageSettings;
            MyPrintDocument.DefaultPageSettings.Margins =
                             new Margins(10, 10, 20, 20);

            log.LogMethodExit(true);
            return true;
        }
        private void PrintChecklist()
        {
            log.LogMethodEntry();
            try
            {
                POSUtils.SetLastActivityDateTime();
                this.Cursor = Cursors.WaitCursor;
                txtMessage.Clear();
                if (this.reservationBL.GetReservationDTO != null
                    && this.reservationBL.GetReservationDTO.BookingId > -1
                    && this.dgvUserJobTaskList.Rows.Count > 0)
                {
                    string reportFileName = GetChecklistPrintReport(false);
                    if (string.IsNullOrEmpty(reportFileName))
                    {
                        txtMessage.Text = MessageContainerList.GetMessage(executionContext, 2110);//"Error while generating report file"
                        this.Cursor = Cursors.Default;
                        log.LogMethodExit("Error while generating report file");
                        return;
                    }
                    using (PrintDocument pd = new PrintDocument())
                    {
                        if (SetupThePrinting(pd, MessageContainerList.GetMessage(executionContext, "BookingCheckList")))
                        {
                            try
                            {
                                POSUtils.SetLastActivityDateTime();
                                StopInActivityTimeoutTimer();
                                string checkListName = cmbEventCheckList.Text;
                                if (string.IsNullOrEmpty(checkListName) == false)
                                {
                                    //checkListName = WaiverCustomerUtils.StripNonAlphaNumericExceptUnderScore(checkListName);
                                    pd.DocumentName = pd.DocumentName + "_" + checkListName;
                                }
                                pd.DocumentName = WaiverCustomerUtils.StripNonAlphaNumericExceptUnderScore(pd.DocumentName);
                                PDFFilePrinter pdfFilePrinter = new PDFFilePrinter(pd, reportFileName);
                                pdfFilePrinter.SendPDFToPrinter();
                                pdfFilePrinter = null;

                            }
                            catch (Exception ex)
                            {
                                log.Error(ex);
                                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 1819, ex.Message)); //Error while printing the document. Error message: &1
                            }
                            finally
                            {
                                POSUtils.SetLastActivityDateTime();
                                StartInActivityTimeoutTimer();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 1819, ex.Message)); //Error while printing the document. Error message: &1
            }
            SetFormOnFocus();
            this.Cursor = Cursors.Default;
            log.LogMethodExit();
        }
        private string GetChecklistPrintReport(bool backgroundMode)
        {
            log.LogMethodEntry();
            string reportFileName = string.Empty;
            try
            {
                this.Cursor = Cursors.WaitCursor;
                POSUtils.SetLastActivityDateTime();
                StopInActivityTimeoutTimer();
                if (this.reservationBL.GetReservationDTO != null
                    && this.reservationBL.GetReservationDTO.BookingId > -1
                    && cmbEventCheckList.SelectedValue != null && (int)cmbEventCheckList.SelectedValue > -1)
                {
                    List<clsReportParameters.SelectedParameterValue> reportParam = new List<clsReportParameters.SelectedParameterValue>();
                    reportParam.Add(new clsReportParameters.SelectedParameterValue("BookingId", this.reservationBL.GetReservationDTO.BookingId));
                    reportParam.Add(new clsReportParameters.SelectedParameterValue("BookingCheckListId", (int)cmbEventCheckList.SelectedValue));
                    ReceiptReports receiptReports = new ReceiptReports(executionContext, "BookingCheckList", "", null, null, reportParam, "P");
                    reportFileName = receiptReports.GenerateAndPrintReport(backgroundMode);
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
                POSUtils.SetLastActivityDateTime();
                StartInActivityTimeoutTimer();
            }
            log.LogMethodExit(reportFileName);
            return reportFileName;
        }
        private void btnSendEmail_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            this.Cursor = Cursors.WaitCursor;
            txtMessage.Clear();
            POSUtils.SetLastActivityDateTime();
            try
            {
                if (reservationBL.GetReservationDTO != null && reservationBL.GetReservationDTO.BookingId > -1)
                {
                    Products bookingProduct = reservationBL.GetBookingProduct();
                    //if (bookingProduct == null)
                    //{
                    //    bookingProduct = new Products(reservationBL.GetReservationDTO.BookingProductId);
                    //}
                    if (bookingProduct != null && bookingProduct.GetProductsDTO != null)
                    {
                        POSUtils.SetLastActivityDateTime();
                        Semnox.Parafait.Communication.SendEmailUI semail;
                        string attachFile = null;
                        string reportFileName = GetBookingQuoteReport(true);
                        if (ParafaitEnv.CompanyLogo != null)
                        {
                            contentID = "ParafaitBookingLogo" + Guid.NewGuid().ToString() + ".jpg";//Content Id is the identifier for the image
                            attachFile = POSStatic.GetCompanyLogoImageFile(contentID);
                            if (string.IsNullOrWhiteSpace(attachFile))
                            {
                                contentID = "";
                            }
                        }
                        POSUtils.SetLastActivityDateTime();
                        //Get the email template selected by the booking//
                        EmailTemplateListBL emailTemplateListBL = new EmailTemplateListBL(executionContext);
                        List<KeyValuePair<EmailTemplateDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<EmailTemplateDTO.SearchByParameters, string>>();
                        searchParameters.Add(new KeyValuePair<EmailTemplateDTO.SearchByParameters, string>(EmailTemplateDTO.SearchByParameters.EMAIL_TEMPLATE_ID, bookingProduct.GetProductsDTO.EmailTemplateId.ToString()));
                        searchParameters.Add(new KeyValuePair<EmailTemplateDTO.SearchByParameters, string>(EmailTemplateDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        //DataTable dt = utilities.executeDataTable(@"select EmailTemplate from EmailTemplate where EmailTemplateId = (select EmailTemplateId from products where product_id  = @BookingProductId)",
                        //                                          new SqlParameter("@BookingProductId", bookingProduct.ProductID));
                        List<EmailTemplateDTO> emailTemplateDTOList = emailTemplateListBL.GetEmailTemplateDTOList(searchParameters);
                        string ccEmailId = GetCCEmailId();
                        if (emailTemplateDTOList != null && emailTemplateDTOList.Count > 0 && emailTemplateDTOList[0] != null && emailTemplateDTOList[0].EmailTemplateId > 0)
                        {
                            string emailContent = string.Empty;
                            emailContent = emailTemplateDTOList[0].EmailTemplate;

                            TransactionEmailTemplatePrint transactionEmailTemplatePrint = new TransactionEmailTemplatePrint(utilities.ExecutionContext, utilities,
                                  emailTemplateDTOList[0].EmailTemplateId, reservationBL.BookingTransaction, reservationBL.GetReservationDTO);
                            emailContent = transactionEmailTemplatePrint.GenerateEmailTemplateContent();
                            if (emailContent.ToLower().Contains("<html") == false)
                            {
                                emailContent = emailContent.Replace("\r\n", "\n");
                            }
                            string emailSubject = (string.IsNullOrEmpty(emailTemplateDTOList[0].Description) ? "POS Reservation" : emailTemplateDTOList[0].Description);
                            //Newly added constructor in ParafaitUtils , SendEmailUI class. To display sito logo inline with Email Body. 2 additional parameters attachimage and contentid are addded//
                            semail = new Semnox.Parafait.Communication.SendEmailUI(customerDetailUI.CustomerDTO.Email,
                                                                ccEmailId, "", emailSubject, emailContent, reportFileName, "", attachFile, contentID, false, utilities, true);
                        }
                        else
                        {
                            semail = new Semnox.Parafait.Communication.SendEmailUI(customerDetailUI.CustomerDTO.Email,
                                                                 ccEmailId, "",
                                                                 MessageContainerList.GetMessage(executionContext, 1835, reservationBL.GetReservationDTO.ReservationCode, ParafaitEnv.SiteName, reservationBL.GetReservationDTO.Status),//"Your Booking (" + lblReservationCode.Text + ") at " + ParafaitEnv.SiteName + " " + lblStatus.Text)
                                                                 "Dear " + customerDetailUI.CustomerDTO.FirstName + "," + Environment.NewLine + Environment.NewLine +
                                                              //  GetBookingInfoForEmailPrint(), --"Thank you very much for choosing us. Please find your booking details attached. We look forward to welcoming you soon!"
                                                              MessageContainerList.GetMessage(executionContext, 1833) + Environment.NewLine + null,
                                                                 reportFileName, "", false, utilities, true);
                        }
                        POSUtils.SetLastActivityDateTime();
                        semail.ShowDialog();
                        if (semail.EmailSentSuccessfully)
                        {
                            POSUtils.SetLastActivityDateTime();
                            log.Info("Email Send Successfully");
                            // Utilities.executeScalar("update bookings set isEmailSent = isnull(isEmailSent, 0) + 1 where BookingId = @BookingId", new SqlParameter[] { new SqlParameter("@BookingId", BookingId) });
                            reservationBL.IncrementNSaveEmailSentCount();
                            cbxEmailSent.Checked = true;
                        }

                        if (string.IsNullOrEmpty(attachFile) == false)
                        {//Delete the image created in the image folder once Email is sent successfully//
                            FileInfo file = new FileInfo(attachFile);
                            if (file.Exists)
                            {
                                file.Delete();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 1824, ex.Message));
            }
            this.Activate();
            this.Cursor = Cursors.Default;
            log.LogMethodExit();

        }

        private string GetCCEmailId()
        {
            log.LogMethodEntry();
            string ccEmailId = string.Empty;
            try
            {
                LookupsContainerDTO lookupsContainerDTO = LookupsContainerList.GetLookupsContainerDTO(executionContext.GetSiteId(), WEB_SITE_CONFIGURATION, executionContext);
                if (lookupsContainerDTO != null && lookupsContainerDTO.LookupValuesContainerDTOList != null && lookupsContainerDTO.LookupValuesContainerDTOList.Any())
                {
                    LookupValuesContainerDTO containerDTO = lookupsContainerDTO.LookupValuesContainerDTOList.Find(lv => lv.LookupValue == ONLINE_PARTY_BOOKINGS_EMAIL_GROUP);
                    log.LogVariableState("LookupValuesContainerDTO", containerDTO);
                    if (containerDTO != null)
                    {
                        if (string.IsNullOrWhiteSpace(containerDTO.Description) == false)
                        {
                            ccEmailId = containerDTO.Description;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit(ccEmailId);
            return ccEmailId;
        }

        private void btnPayment_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                POSUtils.SetLastActivityDateTime();
                txtMessage.Clear();
                Semnox.Core.Utilities.EventLog audit = new Semnox.Core.Utilities.EventLog(utilities);

                if (reservationBL.GetReservationDTO != null && reservationBL.GetReservationDTO.Status == ReservationDTO.ReservationStatus.BOOKED.ToString())
                {
                    throw new Exception(MessageContainerList.GetMessage(executionContext, 1794)); // "Please confirm the reservation before payment"; 
                }
                else if (reservationBL.GetReservationDTO != null && reservationBL.GetReservationDTO.Status == ReservationDTO.ReservationStatus.CONFIRMED.ToString())
                {
                    Card savCard = null;
                    if (reservationBL.BookingTransaction.PrimaryCard != null)
                    {
                        savCard = reservationBL.BookingTransaction.PrimaryCard;
                        reservationBL.BookingTransaction.PrimaryCard = null; // otherwise card payment will be shown
                    }
                    try
                    {
                        reservationBL.BookingTransaction.getTotalPaidAmount(null);//Added the method here to calculate total paid amount before calling the payments form
                        double amountPaid = reservationBL.BookingTransaction.TotalPaidAmount;
                        if (reservationBL.BookingTransaction.Order == null || (reservationBL.BookingTransaction.Order != null && reservationBL.BookingTransaction.Order.OrderHeaderDTO == null))
                        {
                            reservationBL.BookingTransaction.Order = new OrderHeaderBL(utilities.ExecutionContext, new OrderHeaderDTO());
                        }
                        if (reservationBL.BookingTransaction.Order != null && reservationBL.BookingTransaction.Order.OrderHeaderDTO != null && reservationBL.BookingTransaction.Order.OrderHeaderDTO.TransactionOrderTypeId == -1)
                        {
                            reservationBL.BookingTransaction.Order.OrderHeaderDTO.TransactionOrderTypeId = POSStatic.transactionOrderTypes["Sale"];
                        }
                        POSUtils.SetLastActivityDateTime();
                        using (PaymentDetails frmPayment = new PaymentDetails(reservationBL.BookingTransaction))
                        {
                            DialogResult dr = frmPayment.ShowDialog();
                            this.Cursor = Cursors.WaitCursor;
                            if (savCard != null)
                            {
                                reservationBL.BookingTransaction.PrimaryCard = savCard;
                                savCard = null;
                            }
                            reservationBL.BookingTransaction.PaymentCreditCardSurchargeAmount = frmPayment.PaymentCreditCardSurchargeAmount;
                            reservationBL.BookingTransaction.getTotalPaidAmount(null);

                            if (amountPaid != reservationBL.BookingTransaction.TotalPaidAmount)
                            {
                                audit.logEvent("Reservation", 'D', reservationBL.BookingTransaction.LoginID, " Total Amount paid till Date: " + reservationBL.BookingTransaction.TotalPaidAmount.ToString(utilities.ParafaitEnv.AMOUNT_FORMAT), "BookingPayment", 0, "", reservationBL.GetReservationDTO.Guid.ToString(), null);
                                OpenCashDrawer();
                            }
                        }
                    }
                    finally
                    {
                        if (savCard != null)
                        {
                            reservationBL.BookingTransaction.PrimaryCard = savCard;
                            savCard = null;
                        }
                    }
                    LoadSummaryDetails();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                LoadSummaryDetails();
                txtMessage.Text = ex.Message;
            }
            this.Cursor = Cursors.Default;
            log.LogMethodExit();
        }
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            POSUtils.SetLastActivityDateTime();
            ConfirmBooking();
            SetFormOnFocus();
            log.LogMethodExit();
        }
        private void ConfirmBooking()
        {
            log.LogMethodEntry();
            try
            {
                POSUtils.SetLastActivityDateTime();
                if (reservationBL.ReservationTransactionIsNotNull() && reservationBL.GetReservationDTO.Status == ReservationDTO.ReservationStatus.BOOKED.ToString())
                {
                    if (POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 309), MessageContainerList.GetMessage(executionContext, "Confirm"), MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    {
                        txtMessage.Clear();
                        this.Cursor = Cursors.WaitCursor;
                        try
                        {
                            POSUtils.SetLastActivityDateTime();
                            StopInActivityTimeoutTimer();
                            string msg = MessageContainerList.GetMessage(executionContext, "Confirming the reservation.") + " " +
                                          MessageContainerList.GetMessage(executionContext, 684);//"Please wait..."
                            bool valid = BackgroundProcessRunner.Run<bool>(() => { return InvokeConfirmReservation(); }, msg, BackgroundProcessRunner.LaunchWaitScreenAfterXSeconds);
                        }
                        finally
                        {
                            this.Cursor = Cursors.WaitCursor;
                            StartInActivityTimeoutTimer();
                        }
                        //reservationBL.ConfirmReservation(null);
                        // LogEvent(isEditedBooking, editedBookingId)
                        UpdateUIElements();
                        LoadSummaryDetails();
                        txtMessage.Text = MessageContainerList.GetMessage(executionContext, "CONFIRMED");
                        log.Info("Reservation is CONFIRMED");
                    }
                }
            }
            catch (ValidationException ex)
            {
                log.Error(ex);
                txtMessage.Text = ex.GetAllValidationErrorMessages();
                ReloadBooking(MessageContainerList.GetMessage(executionContext, "Confirm"));
            }
            catch (Exception ex)
            {
                log.Error(ex);
                txtMessage.Text = ex.Message;
                ReloadBooking(MessageContainerList.GetMessage(executionContext, "Confirm"));
            }
            this.Cursor = Cursors.Default;
            log.LogMethodExit();
        }
        private bool InvokeConfirmReservation()
        {
            log.LogMethodEntry();
            reservationBL.ConfirmReservation(null);
            log.LogMethodExit();
            return true;
        }
        private void btnExecute_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            this.Cursor = Cursors.WaitCursor;
            POSUtils.SetLastActivityDateTime();
            try
            {
                txtMessage.Clear();
                this.Cursor = Cursors.WaitCursor;
                if (reservationBL.GetReservationDTO != null && reservationBL.GetReservationDTO.Status == ReservationDTO.ReservationStatus.CONFIRMED.ToString()
                    && reservationBL.BookingTransaction != null)
                {
                    POSUtils.SetLastActivityDateTime();
                    //Do you want to execute the reservation?
                    if (POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 4638),
                                                   MessageContainerList.GetMessage(executionContext, "Execute"), MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    {
                        POSUtils.SetLastActivityDateTime();
                        using (frmInputPhysicalCards fin = new frmInputPhysicalCards(reservationBL.BookingTransaction))
                        {
                            Dictionary<string, string> tempCardList = new Dictionary<string, string>();
                            if (fin.CompleteCardList.Count > 0) // card lines found
                            {
                                foreach (string key in fin.CompleteCardList.Keys)
                                {
                                    if (key.StartsWith("T"))
                                    {
                                        tempCardList.Add(key, "");
                                    }
                                }
                            }

                            if (tempCardList.Count > 0)
                            {
                                //fin.cardList = tempCardList;
                                if (fin.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                                {
                                    POSUtils.SetLastActivityDateTime();
                                    POSUtils.SetLastActivityDateTime();
                                    log.LogMethodExit("frmInputPhysicalCards was cancelled");
                                    this.Cursor = Cursors.Default;
                                    return;
                                }
                            }
                            else
                            {
                                string waiverPendingMsg = fin.WaiverPendingMsg;
                                if (string.IsNullOrEmpty(waiverPendingMsg) == false)
                                {
                                    POSUtils.SetLastActivityDateTime();
                                    waiverPendingMsg = waiverPendingMsg + MessageContainerList.GetMessage(POSStatic.Utilities.ExecutionContext, 2382);
                                    //Skipping these transaction line cards to proceed with the rest. Please complete waiver signinng formalities if you want to include them
                                    POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(POSStatic.Utilities.ExecutionContext, waiverPendingMsg));

                                    log.LogMethodExit("tempCardList.Count > 0");
                                    this.Cursor = Cursors.Default;
                                    return;
                                }
                                this.Cursor = Cursors.Default;
                            }
                            this.Cursor = Cursors.WaitCursor;
                            try
                            {
                                this.Cursor = Cursors.WaitCursor;
                                try
                                {
                                    POSUtils.SetLastActivityDateTime();
                                    StopInActivityTimeoutTimer();
                                    string msg = MessageContainerList.GetMessage(executionContext, "Execute reservation is in progress. ") +
                                                  MessageContainerList.GetMessage(executionContext, 684);// "Please wait..."                       
                                    bool valid = BackgroundProcessRunner.Run<bool>((progress) => { return InvokeExecuteReservation(fin.MappedCardList, progress); }, msg, BackgroundProcessRunner.LaunchWaitScreenAfterXSeconds);
                                }
                                finally
                                {
                                    StartInActivityTimeoutTimer();
                                }
                                this.Cursor = Cursors.WaitCursor;
                                UpdateUIElements();
                                LoadSummaryDetails();
                                if (reservationBL.IsWaiverSignaturePending())
                                {
                                    txtMessage.Text = MessageContainerList.GetMessage(executionContext, 2421);// "Booking is partially executed due to pending waivers.";  
                                    log.Info(txtMessage.Text + " :" + reservationBL.GetReservationDTO.BookingId.ToString());
                                }
                                else if (reservationBL.HasTempCards())
                                {
                                    txtMessage.Text = MessageContainerList.GetMessage(executionContext, 4146);// "Booking is partially executed due to pending temp cards"
                                    log.Info(txtMessage.Text + " :" + reservationBL.GetReservationDTO.BookingId.ToString());
                                }
                                else
                                {
                                    txtMessage.Text = MessageContainerList.GetMessage(executionContext, 1795);// "Executed successfully. Open the Transaction in POS to continue.";  
                                    log.Info("Reservation is Executed successfully with BookingId :" + reservationBL.GetReservationDTO.BookingId.ToString());
                                }
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex);
                                txtMessage.Text = ex.Message;
                                ReloadBooking(MessageContainerList.GetMessage(executionContext, "Execute Transaction"));
                            }
                        }
                    }
                }
                SetFormOnFocus();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                txtMessage.Text = ex.Message;
            }
            this.Cursor = Cursors.Default;
            log.LogMethodExit();
        }
        private bool InvokeExecuteReservation(Dictionary<string, string> cardList, IProgress<ProgressReport> progress)
        {
            log.LogMethodEntry(cardList);
            reservationBL.ExecuteReservationTransaction(cardList, progress);
            log.LogMethodExit();
            return true;
        }
        private void ReloadBooking(string activity)
        {
            log.LogMethodEntry();
            POSUtils.ParafaitMessageBox(txtMessage.Text, activity);
            this.Cursor = Cursors.WaitCursor;
            try
            {
                POSUtils.SetLastActivityDateTime();
                reservationBL = new ReservationBL(executionContext, utilities, reservationBL.GetReservationDTO.BookingId);
                UpdateUIElements();
                LoadBookingDetails();
                POSUtils.SetLastActivityDateTime();
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }
        private void btnAddAttendees_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                POSUtils.SetLastActivityDateTime();
                txtMessage.Clear();
                if (reservationBL.GetReservationDTO != null && reservationBL.GetReservationDTO.BookingId > -1
                    && reservationBL.BookingTransaction != null && reservationBL.BookingTransaction.customerDTO != null)
                {
                    using (frmBookingAttendees frb = new frmBookingAttendees(reservationBL, reservationBL.BookingTransaction.customerDTO.FirstName))
                    {
                        frb.ShowDialog();
                        reservationBL = frb.GetReservationBL;
                        POSUtils.SetLastActivityDateTime();
                    }
                }
                else
                {
                    txtMessage.Text = MessageContainerList.GetMessage(executionContext, 2400);// "Save the booking with customer details to access Attendee details"
                }
            }
            catch (ValidationException ex)
            {
                log.Error(ex);
                txtMessage.Text = ex.GetAllValidationErrorMessages();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                txtMessage.Text = MessageContainerList.GetMessage(executionContext, 1824, ex.Message);
            }
            this.Cursor = Cursors.Default;
            log.LogMethodExit();
        }
        private void btnAddTransactionProfile_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            POSUtils.SetLastActivityDateTime();
            this.Cursor = Cursors.WaitCursor;
            txtMessage.Clear();
            try
            {
                GetTrxProfile();
            }
            catch (ValidationException ex)
            {
                log.Error(ex);
                txtMessage.Text = ex.GetAllValidationErrorMessages();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                txtMessage.Text = MessageContainerList.GetMessage(executionContext, 1824, ex.Message);
            }
            this.Cursor = Cursors.Default;
            log.LogMethodExit();
        }
        private void GetTrxProfile(List<Transaction.TransactionLine> trxlineList = null)
        {
            log.LogMethodEntry(trxlineList);
            txtMessage.Clear();
            bool fromLines = false;
            this.Cursor = Cursors.WaitCursor;
            POSUtils.SetLastActivityDateTime();
            if (BookingIsInEditMode() && reservationBL != null && reservationBL.ReservationTransactionIsNotNull())
            {
                POSUtils.SetLastActivityDateTime();
                using (frmChooseTrxProfile fct = new frmChooseTrxProfile())
                {
                    try
                    {
                        if (fct.DialogResult == System.Windows.Forms.DialogResult.OK)
                        {
                            POSUtils.SetLastActivityDateTime();
                            this.Cursor = Cursors.WaitCursor;
                            if (reservationBL.BookingTransaction != null
                                && ((int)fct.TrxProfileId != reservationBL.BookingTransaction.TrxProfileId) || (trxlineList != null && trxlineList.Count > 0))
                            {
                                if (trxlineList == null || trxlineList.Count == 0)
                                {
                                    reservationBL.BookingTransaction.ApplyProfile(fct.TrxProfileId);
                                }
                                else
                                {
                                    fromLines = true;
                                    foreach (Transaction.TransactionLine trxLineItem in trxlineList)
                                    {
                                        reservationBL.BookingTransaction.ApplyProfile(fct.TrxProfileId, trxLineItem);
                                    }
                                }
                                if (fct.TrxProfileVerify.ToString() == "Y")
                                {
                                    InvokeTrxProfileUserVerification((int)fct.TrxProfileId, trxlineList);
                                    this.Cursor = Cursors.WaitCursor;
                                }
                            }
                            if (fromLines == false)
                            {
                                LoadSummaryDetails();
                            }
                        }
                    }
                    finally
                    {
                        POSUtils.SetLastActivityDateTime();
                        fct.Dispose();
                    }
                }
            }
            this.Cursor = Cursors.Default;
            log.LogMethodExit();
        }
        private void InvokeTrxProfileUserVerification(int trxProfileId, List<Transaction.TransactionLine> trxlineList)
        {
            log.LogMethodEntry(trxProfileId, trxlineList);
            this.Cursor = Cursors.WaitCursor;
            string userVerificationId = string.Empty;
            string userVerificationName = string.Empty;
            bool verificationDetailsReceived = false;
            while (verificationDetailsReceived)
            {
                POSUtils.SetLastActivityDateTime();
                using (GenericDataEntry trxProfileVerify = new GenericDataEntry(2))
                {
                    trxProfileVerify.Text = MessageContainerList.GetMessage(executionContext, 1823);// "Transaction Profile Verification"
                    trxProfileVerify.DataEntryObjects[0].mandatory = true;
                    trxProfileVerify.DataEntryObjects[0].label = MessageContainerList.GetMessage(executionContext, "TIN Number");
                    trxProfileVerify.DataEntryObjects[0].dataType = GenericDataEntry.DataTypes.String;
                    trxProfileVerify.DataEntryObjects[1].mandatory = true;
                    trxProfileVerify.DataEntryObjects[1].label = MessageContainerList.GetMessage(executionContext, "Name");
                    trxProfileVerify.DataEntryObjects[1].dataType = GenericDataEntry.DataTypes.String;
                    if (trxProfileVerify.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        POSUtils.SetLastActivityDateTime();
                        userVerificationId = trxProfileVerify.DataEntryObjects[0].data;
                        userVerificationName = trxProfileVerify.DataEntryObjects[1].data;
                        log.LogVariableState("VerificationId", userVerificationId);
                        log.LogVariableState("VerificationName", userVerificationName);
                        if (string.Empty.Equals(userVerificationId) == false && string.Empty.Equals(userVerificationName) == false)
                        {
                            verificationDetailsReceived = true;
                            if (reservationBL.BookingTransaction != null)
                            {
                                if (trxlineList != null && trxlineList.Count > 0)
                                {
                                    foreach (Transaction.TransactionLine trxLineItem in trxlineList)
                                    {
                                        reservationBL.BookingTransaction.TrxLines[reservationBL.BookingTransaction.TrxLines.IndexOf(trxLineItem)].userVerificationId = userVerificationId;
                                        reservationBL.BookingTransaction.TrxLines[reservationBL.BookingTransaction.TrxLines.IndexOf(trxLineItem)].userVerificationName = userVerificationName;
                                    }
                                }
                                else
                                {
                                    reservationBL.BookingTransaction.TrxLines[0].userVerificationId = userVerificationId;
                                    reservationBL.BookingTransaction.TrxLines[0].userVerificationName = userVerificationName;
                                }
                            }
                        }
                    }
                    if (verificationDetailsReceived == false)
                    {
                        POSUtils.SetLastActivityDateTime();
                        POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 1822));// "TIN Number or Name is not entered. Both are Mandatory."
                    }
                }
            }
            this.Cursor = Cursors.Default;
            log.LogMethodExit(trxProfileId);
        }
        private void btnApplyDiscount_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                POSUtils.SetLastActivityDateTime();
                SetDiscounts(-1, -1);
            }
            catch (ValidationException ex)
            {
                log.Error(ex);
                txtMessage.Text = ex.GetAllValidationErrorMessages();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                txtMessage.Text = ex.Message;
            }
            this.Cursor = Cursors.Default;
            log.LogMethodExit();
        }
        private void SetDiscounts(int productId, int comboProductId)
        {
            log.LogMethodEntry(productId, comboProductId);
            POSUtils.SetLastActivityDateTime();
            this.Cursor = Cursors.WaitCursor;
            bool fromLines = false;
            txtMessage.Clear();
            if (BookingIsInEditMode() && reservationBL != null && reservationBL.ReservationTransactionIsNotNull())
            {
                try
                {
                    if (productId > -1 && comboProductId > -1)
                    {
                        fromLines = true;
                    }
                    POSUtils.SetLastActivityDateTime();
                    List<DiscountContainerDTO> appliedDiscountContainerDTOList = reservationBL.GetAppliedDiscountInfo(productId, comboProductId);
                    if (appliedDiscountContainerDTOList != null && appliedDiscountContainerDTOList.Count > 0)
                    {//Get distinct records only
                        appliedDiscountContainerDTOList = appliedDiscountContainerDTOList.GroupBy(disc => new { disc.DiscountId, disc.DiscountAmount, disc.DiscountPercentage }).Select(g => g.First()).ToList();
                    }

                    List<DiscountContainerDTO> selectedDiscountDTOList = null;
                    POSUtils.SetLastActivityDateTime();
                    using (ReservationDiscountsListUI reservationDiscountsListUI = new ReservationDiscountsListUI(utilities, appliedDiscountContainerDTOList, productId, comboProductId))
                    {
                        if (reservationDiscountsListUI.ShowDialog() == DialogResult.OK)
                        {
                            try
                            {
                                POSUtils.SetLastActivityDateTime();
                                StopInActivityTimeoutTimer();
                                this.Cursor = Cursors.WaitCursor;
                                selectedDiscountDTOList = new List<DiscountContainerDTO>(reservationDiscountsListUI.SelectedDiscountsDTOList);
                                List<DiscountContainerDTO> unAppliedDiscountContainerDTOList = new List<DiscountContainerDTO>();
                                if (selectedDiscountDTOList != null && selectedDiscountDTOList.Count > 0)
                                {
                                    if (appliedDiscountContainerDTOList != null && appliedDiscountContainerDTOList.Count > 0)
                                    {
                                        for (int i = 0; i < appliedDiscountContainerDTOList.Count; i++)
                                        {
                                            if (selectedDiscountDTOList.Exists(disc => disc.DiscountId == appliedDiscountContainerDTOList[i].DiscountId) == false)
                                            {
                                                unAppliedDiscountContainerDTOList.Add(appliedDiscountContainerDTOList[i]);
                                            }
                                        }
                                    }

                                    //These are no longer applied to the transaction. Unapply
                                    if (unAppliedDiscountContainerDTOList != null && unAppliedDiscountContainerDTOList.Count > 0)
                                    {
                                        foreach (DiscountContainerDTO removedDiscountsDTO in appliedDiscountContainerDTOList)
                                        {
                                            reservationBL.UnApplyDiscounts(removedDiscountsDTO.DiscountId, productId, comboProductId);
                                        }
                                    }

                                    foreach (DiscountContainerDTO selectedDiscountItem in selectedDiscountDTOList)
                                    {
                                        bool alreadyApplied = false;
                                        if (appliedDiscountContainerDTOList != null && appliedDiscountContainerDTOList.Count > 0)
                                        {
                                            if (appliedDiscountContainerDTOList.Exists(disc => disc.DiscountId == selectedDiscountItem.DiscountId) == true)
                                            {
                                                alreadyApplied = true;
                                            }
                                        }
                                        if (alreadyApplied == false)
                                        {
                                            decimal? variableAmount = null;
                                            int mgrId = -1;
                                            if (selectedDiscountItem.VariableDiscounts == "Y")
                                            {
                                                variableAmount = POSUtils.GetVariableDiscountAmount();
                                                this.Cursor = Cursors.WaitCursor;
                                            }
                                            if (selectedDiscountItem.ManagerApprovalRequired == "Y")
                                            {
                                                if (!Authenticate.Manager(ref mgrId))
                                                {
                                                    POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, "Manager Approval Required"));
                                                    log.LogVariableState("Manager Approval Required to apply this discount", selectedDiscountItem);
                                                    continue;
                                                }
                                                this.Cursor = Cursors.WaitCursor;
                                            }
                                            reservationBL.ApplyDiscounts(selectedDiscountItem.DiscountId, productId, comboProductId, variableAmount, mgrId);
                                        }
                                    }
                                }
                                else
                                {
                                    if (appliedDiscountContainerDTOList != null && appliedDiscountContainerDTOList.Count > 0)
                                    {
                                        foreach (DiscountContainerDTO removedDiscountsDTO in appliedDiscountContainerDTOList)
                                        {
                                            reservationBL.UnApplyDiscounts(removedDiscountsDTO.DiscountId, productId, comboProductId);
                                        }
                                    }
                                }
                            }
                            finally
                            {
                                StartInActivityTimeoutTimer();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    txtMessage.Text = ex.Message;
                }
                if (fromLines == false)
                {
                    LoadSummaryDetails();
                }
            }
            this.Cursor = Cursors.Default;
            log.LogMethodExit();
        }
        private int GetTrxLineIndex(Transaction.TransactionLine trxLine)
        {
            log.LogMethodEntry(trxLine);
            int lineIndex = -1;
            if (reservationBL.ReservationTransactionIsNotNull())
            {
                lineIndex = reservationBL.BookingTransaction.TrxLines.IndexOf(trxLine);
            }
            log.LogMethodExit(lineIndex);
            return lineIndex;
        }
        void EditModifiers(List<Transaction.TransactionLine> selectedTrxLineWithModifiers)
        {
            log.LogMethodEntry(selectedTrxLineWithModifiers);
            try
            {
                this.Cursor = Cursors.WaitCursor;
                POSUtils.SetLastActivityDateTime();
                StopInActivityTimeoutTimer();
                if (BookingIsInEditMode() && selectedTrxLineWithModifiers != null && selectedTrxLineWithModifiers.Count > 0)
                {
                    List<KeyValuePair<int, PurchasedProducts>> purchasedProductList = new List<KeyValuePair<int, PurchasedProducts>>();
                    foreach (Transaction.TransactionLine trxLineItem in selectedTrxLineWithModifiers)
                    {
                        if (purchasedProductList.Exists(item => item.Key == trxLineItem.ProductID) == false)
                        {
                            PurchasedProducts purchasedProducts = GetModifierDetails(trxLineItem.ProductID, trxLineItem.ProductTypeCode);
                            this.Cursor = Cursors.WaitCursor;
                            if (purchasedProducts != null)
                            {
                                purchasedProductList.Add(new KeyValuePair<int, PurchasedProducts>(trxLineItem.ProductID, purchasedProducts));
                            }
                            else
                            {
                                throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Please select modifiers to proceed with edit operation"));
                            }
                        }
                        this.Cursor = Cursors.WaitCursor;
                        CancelProductLine(trxLineItem);
                        reservationBL.AddManualChildProduct(trxLineItem, purchasedProductList, 1);
                    }
                    //foreach (Transaction.TransactionLine trxLineItem in selectedTrxLineWithModifiers)
                    //{
                    //    CancelProductLine(trxLineItem);
                    //    //Products productsBL = new Products(trxLineItem.ProductID);
                    //    //double productPrice = (double)productsBL.GetProductsDTO.Price;
                    //    //string productType = productsBL.GetProductsDTO.ProductType;
                    //    //AddProductToBookingTransaction(trxLineItem.ProductID, productPrice, 1, productType, trxLineItem.ComboproductId);
                    //    reservationBL.AddManualChildProduct(trxLineItem, purchasedProductList, 1);
                    //}
                }
            }
            finally
            {
                StartInActivityTimeoutTimer();
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }
        private void pcbRemoveDiscCoupon_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            POSUtils.SetLastActivityDateTime();
            this.Cursor = Cursors.WaitCursor;
            if (BookingIsInEditMode() && txtAppliedDiscountCoupon.Tag != null && txtAppliedDiscountCoupon.Tag.ToString() != "-1")
            {
                int discountId = -1;
                // "Are you sure do you want to remove the Coupon applied?"
                if (POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 2112), MessageContainerList.GetMessage(executionContext, "Remove Coupon"), MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    try
                    {
                        POSUtils.SetLastActivityDateTime();
                        StopInActivityTimeoutTimer();
                        discountId = Convert.ToInt32(txtAppliedDiscountCoupon.Tag);
                        reservationBL.UnApplyDiscounts(discountId, -1, -1);
                        DiscountContainerDTO discountCouponDTO = reservationBL.GetAppliedDiscountCouponForBooking();
                        if (discountCouponDTO != null)
                        {
                            txtAppliedDiscountCoupon.Text = discountCouponDTO.DiscountName;
                            txtAppliedDiscountCoupon.Tag = discountCouponDTO.DiscountId;
                        }
                        else
                        {
                            txtAppliedDiscountCoupon.Text = string.Empty;
                            txtAppliedDiscountCoupon.Tag = "-1";
                        }
                        LoadSummaryDetails();
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        txtMessage.Text = ex.Message;
                    }
                    finally
                    {
                        StartInActivityTimeoutTimer();
                    }
                }
            }
            this.Cursor = Cursors.Default;
            log.LogMethodExit();
        }
        private void pcbRemoveDiscount_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            POSUtils.SetLastActivityDateTime();
            this.Cursor = Cursors.WaitCursor;
            if (BookingIsInEditMode() && txtAppliedDiscount.Tag != null && txtAppliedDiscount.Tag.ToString() != "-1")
            {
                int discountId = -1;
                // "Are you sure do you want to remove the discount applied?"
                if (POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 2113), MessageContainerList.GetMessage(executionContext, "Remove Discount"), MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    try
                    {
                        POSUtils.SetLastActivityDateTime();
                        StopInActivityTimeoutTimer();
                        discountId = Convert.ToInt32(txtAppliedDiscount.Tag);
                        reservationBL.UnApplyDiscounts(discountId, -1, -1);
                        DiscountContainerDTO discountContainerIDDTO = reservationBL.GetAppliedDiscountForBooking();
                        if (discountContainerIDDTO != null)
                        {
                            txtAppliedDiscount.Text = discountContainerIDDTO.DiscountName;
                            txtAppliedDiscount.Tag = discountContainerIDDTO.DiscountId;
                        }
                        else
                        {
                            txtAppliedDiscount.Text = string.Empty;
                            txtAppliedDiscount.Tag = "-1";
                        }
                        LoadSummaryDetails();
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        txtMessage.Text = ex.Message;
                    }
                    finally
                    {
                        StartInActivityTimeoutTimer();
                    }
                }
            }
            this.Cursor = Cursors.Default;
            log.LogMethodExit();
        }
        private void btnApplyDiscCoupon_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            POSUtils.SetLastActivityDateTime();
            this.Cursor = Cursors.WaitCursor;
            txtMessage.Clear();
            if (BookingIsInEditMode() && reservationBL != null && reservationBL.ReservationTransactionIsNotNull())
            {
                try
                {
                    string couponNumber = "";
                    POSUtils.SetLastActivityDateTime();
                    GenericDataEntry discountCouponEntry = new GenericDataEntry(1);
                    discountCouponEntry.Text = POSStatic.MessageUtils.getMessage("Discount Coupons");
                    discountCouponEntry.DataEntryObjects[0].mandatory = false;
                    discountCouponEntry.DataEntryObjects[0].label = "Discount Coupon";
                    discountCouponEntry.DataEntryObjects[0].data = couponNumber;
                    if (discountCouponEntry.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        POSUtils.SetLastActivityDateTime();
                        couponNumber = discountCouponEntry.DataEntryObjects[0].data;
                        if (string.Empty.Equals(couponNumber) == false)
                        {
                            POSUtils.SetLastActivityDateTime();
                            reservationBL.ApplyDiscountCoupon(couponNumber);
                            LoadSummaryDetails();
                        }
                    }
                }
                catch (ValidationException ex)
                {
                    log.Error(ex);
                    txtMessage.Text = ex.GetAllValidationErrorMessages();
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    txtMessage.Text = ex.Message;
                }
            }
            this.Cursor = Cursors.Default;
            log.LogMethodExit();
        }

        private void dgvUserJobTaskList_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            log.LogMethodExit();
        }
        private void dgvUserJobTaskList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            if (dgvUserJobTaskList.CurrentCell != null && e != null && e.RowIndex > -1
               && dgvUserJobTaskList.Columns[e.ColumnIndex].Name == "chklistValue")
            {
                if (reservationBL.GetReservationDTO != null && dgvUserJobTaskList.Enabled == true)
                {
                    DataGridViewCheckBoxCell checkBox = (dgvUserJobTaskList["chklistValue", e.RowIndex] as DataGridViewCheckBoxCell);
                    if (Convert.ToBoolean(checkBox.Value))
                    {
                        checkBox.Value = false;
                    }
                    else
                    {
                        checkBox.Value = true;
                    }
                }
            }
            log.LogMethodExit();
        }
        private void dgvUserJobTaskList_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
        }

        private void frmReservationUI_Deactivate(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            //Parafait_POS.POSUtils.AttachFormEvents();
            log.LogMethodExit();
        }
        private void frmReservationUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                this.Cursor = Cursors.WaitCursor;
                reservationBL.ClearUnSavedSchedules();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            this.Cursor = Cursors.Default;
            log.LogMethodExit();
        }
        private void BlueBtn_MouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            POSUtils.SetLastActivityDateTime();
            Button selectedButton = (Button)sender;
            selectedButton.BackgroundImage = Properties.Resources.pressed2;
            log.LogMethodExit();
        }
        private void BlueBtn_MouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            POSUtils.SetLastActivityDateTime();
            Button selectedButton = (Button)sender;
            selectedButton.BackgroundImage = Properties.Resources.normal2;
            log.LogMethodExit();

        }
        private void tpAdditionalProducts_Leave(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                txtMessage.Clear();
                SetServiceCharges();
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                txtMessage.Text = ex.Message;
                this.Cursor = Cursors.Default;
                tcBooking.SelectedTab = tpAdditionalProducts;
            }
            log.LogMethodExit();
        }
        private void tpPackageProducts_Leave(object sender, EventArgs e)
        {
            log.LogMethodEntry(); try
            {
                txtMessage.Clear();
                SetRemarks();
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                txtMessage.Text = ex.Message;
                this.Cursor = Cursors.Default;
                POSUtils.ParafaitMessageBox(ex.Message);
                tcBooking.SelectedTab = tpPackageProducts;
            }
            log.LogMethodExit();
        }
        private void pcbRemove_MouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            POSUtils.SetLastActivityDateTime();
            PictureBox selectedPCBX = (PictureBox)sender;
            selectedPCBX.Image = Properties.Resources.R_Remove_Btn_Hover;
            log.LogMethodExit();
        }
        private void pcbRemove_MouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            POSUtils.SetLastActivityDateTime();
            PictureBox selectedPCBX = (PictureBox)sender;
            selectedPCBX.Image = Properties.Resources.R_Remove_Btn_Normal;
            log.LogMethodExit();
        }
        private void SetFormOnFocus()
        {
            log.LogMethodEntry();
            try
            {
                this.Activate();
                this.Focus();
                this.TopMost = true;
                this.BringToFront();
                this.TopMost = false;
            }
            catch { }
            log.LogMethodExit();
        }
        private void btnMapWaivers_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                POSUtils.SetLastActivityDateTime();
                txtMessage.Clear();
                this.Cursor = Cursors.WaitCursor;
                if (reservationBL.GetReservationDTO != null
                    && (reservationBL.GetReservationDTO.Status == ReservationDTO.ReservationStatus.CONFIRMED.ToString() ||
                        reservationBL.GetReservationDTO.Status == ReservationDTO.ReservationStatus.COMPLETE.ToString())
                    && reservationBL.BookingTransaction != null)
                {
                    //if (reservationBL.BookingTransaction.TransactionDate < utilities.getServerTime().Date)
                    //{
                    //    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2355)); //Cannot map waivers for past date transaction 
                    //}
                    //else
                    //{
                    if (reservationBL.WaiverSignatureRequired())
                    {
                        List<WaiversDTO> trxWaiversDTOList = reservationBL.BookingTransaction.GetWaiversDTOList();
                        if (trxWaiversDTOList == null || trxWaiversDTOList.Any() == false)
                        {
                            log.LogVariableState("trxWaiversDTOList", trxWaiversDTOList);
                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2317,
                                 MessageContainerList.GetMessage(executionContext, "Transaction") + " " + MessageContainerList.GetMessage(executionContext, "Waivers")));
                        }
                        POSUtils.SetLastActivityDateTime();
                        using (frmMapWaiversToTransaction frm = new frmMapWaiversToTransaction(POSStatic.Utilities, reservationBL.BookingTransaction))
                        {
                            if (frm.Width > Application.OpenForms["POS"].Width + 28)
                            {
                                frm.Width = Application.OpenForms["POS"].Width - 30;
                            }
                            if (frm.ShowDialog() == DialogResult.OK)
                            {
                                try
                                {
                                    POSUtils.SetLastActivityDateTime();
                                    StopInActivityTimeoutTimer();
                                    string msg = string.Empty;
                                    this.Cursor = Cursors.WaitCursor;
                                    int retcode = reservationBL.BookingTransaction.SaveOrder(ref msg);
                                    POSUtils.SetLastActivityDateTime();
                                    if (retcode != 0)
                                    {
                                        // POSUtils.ParafaitMessageBox(msg);
                                        txtMessage.Text = msg;
                                        //reload transaction details from db
                                        reservationBL = new ReservationBL(executionContext, utilities, reservationBL.GetReservationDTO.BookingId);
                                        log.LogMethodExit(msg);
                                        return;
                                    }
                                }
                                finally
                                {
                                    StartInActivityTimeoutTimer();
                                }
                            }
                        }
                    }
                    else
                    {
                        txtMessage.Text = MessageContainerList.GetMessage(executionContext, 2357);//Transaction does not require waiver mapping
                    }
                    //}
                }
                else
                {
                    txtMessage.Text = MessageContainerList.GetMessage(executionContext, 2356);//Sorry, reservation status should be confirmed or complete
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                txtMessage.Text = ex.Message;
                //POSUtils.ParafaitMessageBox(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }
        private void btnBookingCheckList_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                POSUtils.SetLastActivityDateTime();
                txtMessage.Clear();
                //if (BookingIsInEditMode())
                //{
                if (this.reservationBL != null
                    && this.reservationBL.GetReservationDTO != null
                    && this.reservationBL.GetReservationDTO.BookingId > -1)
                {
                    using (frmBookingCheckList frmCheckList = new frmBookingCheckList(executionContext, utilities, this.reservationBL))
                    {
                        frmCheckList.ShowDialog();
                        POSUtils.SetLastActivityDateTime();
                    };
                }
                else
                {
                    txtMessage.Text = MessageContainerList.GetMessage(executionContext, 2400);//Please save the booking first
                }
                //}
            }
            catch (Exception ex)
            {
                log.Error(ex);
                txtMessage.Text = ex.Message;
            }
            log.LogMethodExit();
        }
        private void OpenCashDrawer()
        {
            log.LogMethodEntry();
            try
            {
                POSUtils.SetLastActivityDateTime();
                StopInActivityTimeoutTimer();
                string cashdrawerInterfaceMode = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "CASHDRAWER_INTERFACE_MODE");
                log.Debug("cashdrawerInterfaceMode :" + cashdrawerInterfaceMode);
                POSMachineContainerDTO pOSMachineContainerDTO = POSMachineContainerList.GetPOSMachineContainerDTOOrDefault(utilities.ExecutionContext.SiteId,
                                                                   ParafaitEnv.POSMachine, "", -1);
                if (cashdrawerInterfaceMode != "NONE")
                {
                    if (pOSMachineContainerDTO.POSCashdrawerContainerDTOList == null ||
                                                pOSMachineContainerDTO.POSCashdrawerContainerDTOList.Any() == false)
                    {
                        log.Error("cashdrawer is not mapped to the POS");
                        POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext, 1), MessageContainerList.GetMessage(utilities.ExecutionContext, "Validate Cashdrawer")); // New message
                        return;
                    }
                    int shiftId = POSUtils.GetOpenShiftId(ParafaitEnv.LoginID);
                    log.Debug("Open ShiftId :" + shiftId);
                    if (shiftId > -1)
                    {
                        ShiftBL shiftBL = new ShiftBL(utilities.ExecutionContext, shiftId);
                        if (shiftBL.ShiftDTO.CashdrawerId > 0)
                        {
                            log.Debug("Open CashdrawerId :" + shiftBL.ShiftDTO.CashdrawerId);
                            CashdrawerBL cashdrawerBL = new CashdrawerBL(utilities.ExecutionContext, shiftBL.ShiftDTO.CashdrawerId);
                            cashdrawerBL.OpenCashDrawer();
                        }
                    }
                }
            }
            finally
            {
                StartInActivityTimeoutTimer();
            }
            log.LogMethodExit();
        }

        internal void RescheduleAttraction(Transaction.TransactionLine productTrxLine, int lineIndex, int comboProductId)
        {
            log.LogMethodEntry(productTrxLine, lineIndex, comboProductId);
            POSUtils.SetLastActivityDateTime();
            Transaction.TransactionLine comboParent = GetComboParentLine(productTrxLine);
            List<Transaction.TransactionLine> relatedLines = GetComboAttractionRelatedLines(comboParent);
            if (relatedLines != null && relatedLines.Exists(tl => tl.LineValid && tl.DBLineId == 0))
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Sorry, cannot proceed with group reschedule. There are unsaved records"));
            }
            ProcessAttractionReschedule(productTrxLine, lineIndex, comboProductId, relatedLines);
            log.LogMethodExit();
        }
        internal void RescheduleAttractionGroup(Transaction.TransactionLine productTrxLine, int lineIndex, int comboProductId)
        {
            log.LogMethodEntry(productTrxLine, lineIndex);
            if (productTrxLine != null)
            {
                POSUtils.SetLastActivityDateTime();
                Transaction.TransactionLine comboParent = GetComboParentLine(productTrxLine);
                List<Transaction.TransactionLine> relatedLines = GetAllRelatedAttractionLines(comboParent);
                if (relatedLines != null && relatedLines.Exists(tl => tl.LineValid && tl.DBLineId == 0))
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Sorry, cannot proceed with group reschedule. There are unsaved records"));
                }
                ProcessAttractionReschedule(productTrxLine, lineIndex, comboProductId, relatedLines);
            }
            log.LogMethodExit();
        }

        private List<Transaction.TransactionLine> GetAllRelatedAttractionLines(Transaction.TransactionLine comboParent)
        {
            log.LogMethodEntry();
            List<Transaction.TransactionLine> transactionLineList = new List<Transaction.TransactionLine>();
            if (comboParent != null)
            {
                List<Transaction.TransactionLine> comboLineList = reservationBL.BookingTransaction.TrxLines.Where(tl => tl.LineValid
                                                                                                                        && tl.ProductID == comboParent.ProductID
                                                                                                                        && tl.ComboproductId == comboParent.ComboproductId).ToList();
                if (comboLineList != null && comboLineList.Any())
                {
                    for (int i = 0; i < comboLineList.Count(); i++)
                    {
                        List<Transaction.TransactionLine> comboChildLineList = GetComboAttractionRelatedLines(comboLineList[i]);
                        if (comboChildLineList != null && comboChildLineList.Any())
                        {
                            transactionLineList.AddRange(comboChildLineList);
                        }
                    }
                }
            }
            log.LogMethodExit(transactionLineList);
            return transactionLineList;
        }
        private List<Transaction.TransactionLine> GetComboAttractionRelatedLines(Transaction.TransactionLine comboParent)
        {
            log.LogMethodEntry();
            List<Transaction.TransactionLine> transactionLineList = new List<Transaction.TransactionLine>();
            if (comboParent != null)
            {
                if (comboParent.LineAtb != null)
                {
                    transactionLineList.Add(comboParent);
                }
                List<Transaction.TransactionLine> comboChildLineList = GetChildLineList(comboParent);
                if (comboChildLineList != null && comboChildLineList.Any())
                {
                    transactionLineList.AddRange(comboChildLineList);
                }
            }
            log.LogMethodExit(transactionLineList);
            return transactionLineList;
        }
        private List<Transaction.TransactionLine> GetChildLineList(Transaction.TransactionLine transactionLine)
        {
            log.LogMethodEntry();
            List<Transaction.TransactionLine> transactionLineList = new List<Transaction.TransactionLine>();
            List<Transaction.TransactionLine> comboLineList = reservationBL.BookingTransaction.TrxLines.Where(tl => tl.LineValid
                                                                                                                       && tl.ParentLine == transactionLine).ToList();
            if (comboLineList != null && comboLineList.Any())
            {
                for (int i = 0; i < comboLineList.Count; i++)
                {
                    if (comboLineList[i].LineAtb != null)
                    {
                        transactionLineList.Add(comboLineList[i]);
                    }
                    List<Transaction.TransactionLine> comboChildLineList = GetChildLineList(comboLineList[i]);
                    if (comboChildLineList != null && comboChildLineList.Any())
                    {
                        transactionLineList.AddRange(comboChildLineList);
                    }
                }
            }
            log.LogMethodExit(transactionLineList);
            return transactionLineList;
        }
        private Transaction.TransactionLine GetComboParentLine(Transaction.TransactionLine productTrxLine)
        {
            log.LogMethodEntry();
            Transaction.TransactionLine comboParentLine = null;
            if (productTrxLine.ComboproductId > -1)
            {//this is parent line
                comboParentLine = productTrxLine;
            }
            else if (productTrxLine.ComboproductId == -1 && productTrxLine.ParentLine != null)
            {
                comboParentLine = GetComboParentLine(productTrxLine.ParentLine);
            }
            log.LogMethodExit(comboParentLine);
            return comboParentLine;
        }
        private void ProcessAttractionReschedule(Transaction.TransactionLine productTrxLine, int lineIndex, int comboProductId, List<Transaction.TransactionLine> relatedLines)
        {
            log.LogMethodEntry(productTrxLine, lineIndex, comboProductId, relatedLines);
            try
            {
                POSUtils.SetLastActivityDateTime();
                this.Cursor = Cursors.WaitCursor;
                List<AttractionBooking> existingBookings = new List<AttractionBooking>();
                Dictionary<int, int> quantityMap = new Dictionary<int, int>();
                List<Products> productsList = new List<Products>();
                if (relatedLines != null && relatedLines.Any())
                {
                    foreach (Transaction.TransactionLine trxLine in relatedLines)
                    {
                        if (quantityMap.ContainsKey(trxLine.ProductID))
                        {
                            quantityMap[trxLine.ProductID] = quantityMap[trxLine.ProductID] + Decimal.ToInt32(trxLine.quantity);
                        }
                        else
                        {
                            quantityMap.Add(trxLine.ProductID, Decimal.ToInt32(trxLine.quantity));
                            productsList.Add(new Products(trxLine.ProductID));
                        }
                        if (trxLine.LineAtb != null)
                        {
                            int index = reservationBL.BookingTransaction.TrxLines.IndexOf(trxLine);
                            existingBookings.Add(trxLine.LineAtb);
                        }
                    }

                }

                DateTime fromDateTime = reservationBL.GetReservationDTO.FromDate;
                DateTime toDateTime = reservationBL.GetReservationDTO.ToDate;

                String message = "";
                POSUtils.SetLastActivityDateTime();
                POS posParent = Application.OpenForms["POS"] as POS;
                List<AttractionBooking> attractionBooking = posParent.GetAttractionBookingSchedule(quantityMap, existingBookings, fromDateTime,
                                                                -1, true, reservationBL.BookingTransaction, ref message, false, toDateTime);

                //this.NewTrx = selectedTransaction;
                if (attractionBooking != null && attractionBooking.Any())
                {
                    List<AttractionBooking> atbMasterList = new List<AttractionBooking>();
                    foreach (KeyValuePair<int, int> productMap in quantityMap)
                    {
                        Products tempProd = productsList.FirstOrDefault(x => x.GetProductsDTO.ProductId == productMap.Key);

                        if (tempProd.GetProductsDTO.CardSale.Equals("Y") && !tempProd.GetProductsDTO.AutoGenerateCardNumber.Equals("Y"))
                        {
                            List<AttractionBooking> atbList = attractionBooking.Where(x => x.AttractionBookingDTO.AttractionProductId == tempProd.GetProductsDTO.ProductId).ToList();
                            int seats = productMap.Value;
                            if (atbList.Any())
                            {
                                List<Card> cardList;
                                if (GetAttractionCards(tempProd.GetProductsDTO, comboProductId, seats, atbMasterList, atbList, out cardList) == false)
                                {
                                    return;
                                }
                                atbMasterList.AddRange(atbList);
                            }
                        }
                    }

                    int bookingTrxId = -1;
                    try
                    {
                        POSUtils.SetLastActivityDateTime();
                        StopInActivityTimeoutTimer();
                        using (ParafaitDBTransaction dBTransaction = new ParafaitDBTransaction())
                        {
                            try
                            {
                                dBTransaction.BeginTransaction();

                                foreach (Transaction.TransactionLine trxLine in reservationBL.BookingTransaction.TrxLines)
                                {
                                    Products tempProd = productsList.FirstOrDefault(x => x.GetProductsDTO.ProductId == trxLine.ProductID);

                                    DateTime originalDateTime = ServerDateTime.Now;
                                    int trxLineIndex = reservationBL.BookingTransaction.TrxLines.IndexOf(trxLine);
                                    List<AttractionBooking> tempExistingList = existingBookings.Where(x => (trxLine.DBLineId > 0 && x.AttractionBookingDTO.LineId == trxLine.DBLineId
                                                                                                            || trxLine.DBLineId == 0 && x.AttractionBookingDTO.LineId == trxLineIndex)).ToList();
                                    if (tempExistingList.Any())
                                    {
                                        AttractionBooking originalATB = tempExistingList[0];
                                        originalATB.ReduceBookedUnits(originalATB.AttractionBookingDTO.BookedUnits, dBTransaction.SQLTrx);
                                        originalDateTime = originalATB.AttractionBookingDTO.ScheduleFromDate;
                                    }

                                    List<AttractionBooking> tempList = null;
                                    tempList = attractionBooking.Where(x => (x.AttractionBookingDTO.AttractionProductId == trxLine.ProductID)).ToList();

                                    if (tempList.Any() && trxLine.card != null && !tempProd.GetProductsDTO.AutoGenerateCardNumber.Equals("Y"))
                                    {
                                        tempList = tempList.Where(y => y.cardList.Any(z => z.CardNumber == trxLine.card.CardNumber)).ToList();
                                    }

                                    if (tempList.Any())
                                    {
                                        AttractionBooking atb = tempList[0];
                                        AttractionBooking clone = new AttractionBooking(executionContext);
                                        clone.CloneObject(atb, 1);
                                        clone.AttractionBookingDTO.AttractionProductId = atb.AttractionBookingDTO.AttractionProductId;
                                        clone.AttractionBookingDTO.TrxId = reservationBL.BookingTransaction.Trx_id;
                                        clone.AttractionBookingDTO.LineId = trxLine.DBLineId;
                                        clone.AttractionBookingDTO.Source = AttractionBookingDTO.SourceEnum.RESERVATION;
                                        atb.ReduceBookedUnits(1, dBTransaction.SQLTrx);
                                        clone.Save(trxLine.card == null ? -1 : trxLine.card.card_id, dBTransaction.SQLTrx);
                                        if (bookingTrxId == -1)
                                        {
                                            bookingTrxId = clone.AttractionBookingDTO.TrxId;
                                        }

                                        if (atb.AttractionBookingDTO.BookedUnits == 0)
                                        {
                                            attractionBooking.Remove(atb);
                                        }


                                        trxLine.LineAtb = clone;
                                        double minutesToAdd = (clone.AttractionBookingDTO.ScheduleFromDate - originalDateTime).TotalMinutes;
                                        reservationBL.BookingTransaction.ReschduleCurrentLineEntitlements(reservationBL.BookingTransaction.Utilities, reservationBL.BookingTransaction.Trx_id, trxLine, minutesToAdd,
                                            clone.AttractionBookingDTO.ScheduleToDate, dBTransaction.SQLTrx);
                                    }

                                }

                                dBTransaction.EndTransaction();
                            }
                            catch (Exception ex)
                            //if (!atb.Save(card == null ? -1 : card.card_id, sqlTrx, ref message))
                            {
                                log.Error(ex);
                                dBTransaction.RollBack();
                                txtMessage.Text = message;
                                log.LogMethodExit("Ends-ctxBookingContextMenu_ItemClicked() as unable to Save AttractionBooking error:)" + message);
                                return;
                            }
                        }

                    }//displayBookings();
                    finally
                    {
                        StartInActivityTimeoutTimer();
                    }
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }
        private void btnSendPaymentLink_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            this.Cursor = Cursors.WaitCursor;
            txtMessage.Clear();
            POSUtils.SetLastActivityDateTime();
            try
            {
                if (reservationBL.GetReservationDTO != null && reservationBL.GetReservationDTO.BookingId > -1)
                {
                    btnSendPaymentLink.Enabled = false;
                    Semnox.Parafait.Communication.SendEmailUI semail;
                    string attachFile = null;
                    if (ParafaitEnv.CompanyLogo != null)
                    {
                        contentID = "ParafaitBookingLogo" + Guid.NewGuid().ToString() + ".jpg";//Content Id is the identifier for the image
                        attachFile = POSStatic.GetCompanyLogoImageFile(contentID);
                        if (string.IsNullOrWhiteSpace(attachFile))
                        {
                            contentID = "";
                        }
                    }

                    //Get the email template 
                    EmailTemplateDTO emailTemplateDTO = TransactionPaymentLink.GetPaymentLinkEmailTemplateDTO(executionContext);


                    //Generate email content
                    string body = string.Empty;
                    if (emailTemplateDTO != null && emailTemplateDTO.EmailTemplateId > 0)
                    {
                        body = emailTemplateDTO.EmailTemplate;

                        TransactionEmailTemplatePrint transactionEmailTemplatePrint = new TransactionEmailTemplatePrint(executionContext, utilities, emailTemplateDTO.EmailTemplateId, this.reservationBL.BookingTransaction, reservationBL.GetReservationDTO);
                        body = transactionEmailTemplatePrint.GenerateEmailTemplateContent();
                    }
                    else
                    {
                        throw new Exception(MessageContainerList.GetMessage(executionContext, 2744));//'Email template to send payment link is not defined'
                    }

                    string emailSubject = (string.IsNullOrEmpty(emailTemplateDTO.Description) ? MessageContainerList.GetMessage(executionContext, "Payment Link")
                                                                                              : emailTemplateDTO.Description);
                    //Newly added constructor in ParafaitUtils , SendEmailUI class. To display sito logo inline with Email Body. 2 additional parameters attachimage and contentid are addded//
                    POSUtils.SetLastActivityDateTime();
                    semail = new Semnox.Parafait.Communication.SendEmailUI(customerDetailUI.CustomerDTO.Email,
                                                        "", "", emailSubject, body, string.Empty, "", attachFile, contentID, false, utilities, true);

                    semail.ShowDialog();
                    if (semail.EmailSentSuccessfully)
                    {
                        POSUtils.SetLastActivityDateTime();
                        log.Info("Email Send Successfully");
                        Semnox.Core.Utilities.EventLog audit = new Semnox.Core.Utilities.EventLog(utilities);
                        audit.logEvent(MessageContainerList.GetMessage(executionContext, "Reservation"),
                                                                 'D', executionContext.GetUserId(),
                                                                 MessageContainerList.GetMessage(executionContext, 2745, MessageContainerList.GetMessage(executionContext, "customer")),//Payment link email is sent to &1
                                                                 MessageContainerList.GetMessage(executionContext, "Reservation"),
                                                                 0, "", reservationBL.GetReservationDTO.Guid.ToString(), null);
                    }

                    if (string.IsNullOrEmpty(attachFile) == false)
                    {//Delete the image created in the image folder once Email is sent successfully//
                        FileInfo file = new FileInfo(attachFile);
                        if (file.Exists)
                        {
                            file.Delete();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 1824, ex.Message));
            }
            finally
            {
                btnSendPaymentLink.Enabled = enablePaymentLinkButton;
                this.Activate();
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// PerformFiscaliation
        /// </summary>
        private void PerformFiscaliation()
        {
            log.LogMethodEntry();
            try
            {
                POSUtils.SetLastActivityDateTime();
                StopInActivityTimeoutTimer();
                FiscalPrinterFactory.GetInstance().Initialize(utilities);
                string fiscalPrinterType = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "FISCAL_PRINTER");
                if (string.IsNullOrEmpty(fiscalPrinterType) == false)
                {
                    fiscalPrinter = FiscalPrinterFactory.GetInstance().GetFiscalPrinter(fiscalPrinterType);
                }
                if ((fiscalPrinterType.Equals(FiscalPrinters.CroatiaFiscalization.ToString())))
                {
                    string fiscalizationResponse = string.Empty;
                    bool isFiscalized = false;
                    fiscalPrinter = FiscalPrinterFactory.GetInstance().GetFiscalPrinter(fiscalPrinterType);
                    try
                    {
                        isFiscalized = fiscalPrinter.PrintReceipt(this.reservationBL.GetReservationDTO.TrxId, ref fiscalizationResponse, null, 0);
                    }
                    catch (ValidationException ex)
                    {
                        Transaction transaction = new Transaction(utilities);
                        transaction.InsertTrxLogs(this.reservationBL.GetReservationDTO.TrxId, -1, transaction.Utilities.ParafaitEnv.LoginID, "FISCALIZATION", ex.Message);
                    }

                    //Updates the Reference string to TransactionPaymentsDTOList of BookingTransaction
                    if (this.reservationBL.BookingTransaction.TransactionPaymentsDTOList != null && this.reservationBL.BookingTransaction.TransactionPaymentsDTOList.Count > 0)
                    {
                        Semnox.Parafait.Device.PaymentGateway.TransactionPaymentsListBL transactionPaymentsListBL = new Semnox.Parafait.Device.PaymentGateway.TransactionPaymentsListBL(utilities.ExecutionContext);
                        List<KeyValuePair<Semnox.Parafait.Device.PaymentGateway.TransactionPaymentsDTO.SearchByParameters, string>> transactionsPaymentsSearchParams = new List<KeyValuePair<Semnox.Parafait.Device.PaymentGateway.TransactionPaymentsDTO.SearchByParameters, string>>();
                        transactionsPaymentsSearchParams.Add(new KeyValuePair<Semnox.Parafait.Device.PaymentGateway.TransactionPaymentsDTO.SearchByParameters, string>(Semnox.Parafait.Device.PaymentGateway.TransactionPaymentsDTO.SearchByParameters.TRANSACTION_ID, this.reservationBL.BookingTransaction.Trx_id.ToString()));

                        List<Semnox.Parafait.Device.PaymentGateway.TransactionPaymentsDTO> transactionPaymentsDTOs = transactionPaymentsListBL.GetTransactionPaymentsDTOList(transactionsPaymentsSearchParams);

                        if (transactionPaymentsDTOs != null || transactionPaymentsDTOs.Count > 0)
                        {
                            for (int i = 0; i < transactionPaymentsDTOs.Count(); i++)
                            {
                                foreach (Semnox.Parafait.Device.PaymentGateway.TransactionPaymentsDTO transactionPaymentsDTO in transactionPaymentsDTOs)
                                {
                                    if (this.reservationBL.BookingTransaction.TransactionPaymentsDTOList[i].PaymentId == transactionPaymentsDTO.PaymentId)
                                    {
                                        this.reservationBL.BookingTransaction.TransactionPaymentsDTOList[i].ExternalSourceReference = transactionPaymentsDTO.ExternalSourceReference;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                if (fiscalPrinterType.ToUpper().Equals(FiskaltrustPrinter.FISKALTRUST))
                {
                    string signature = string.Empty;
                    FiscalizationBL fiscalizationHelper = new FiscalizationBL(utilities.ExecutionContext, reservationBL.BookingTransaction);
                    bool doFiscalize = fiscalizationHelper.NeedsFiscalization(); // If there is no payment then will not be fiscalized
                    if (doFiscalize)
                    {
                        FiscalizationRequest fiscalizationRequest = fiscalizationHelper.BuildFiscalizationRequest();
                        bool isSuccess = fiscalPrinter.PrintReceipt(fiscalizationRequest, ref signature);
                        if (isSuccess)
                        {
                            fiscalizationHelper.UpdatePaymentReference(fiscalizationRequest, signature);
                            if (this.reservationBL.BookingTransaction.TransactionPaymentsDTOList != null && this.reservationBL.BookingTransaction.TransactionPaymentsDTOList.Count > 0)
                            {
                                Semnox.Parafait.Device.PaymentGateway.TransactionPaymentsListBL transactionPaymentsListBL = new Semnox.Parafait.Device.PaymentGateway.TransactionPaymentsListBL(utilities.ExecutionContext);
                                List<KeyValuePair<Semnox.Parafait.Device.PaymentGateway.TransactionPaymentsDTO.SearchByParameters, string>> transactionsPaymentsSearchParams = new List<KeyValuePair<Semnox.Parafait.Device.PaymentGateway.TransactionPaymentsDTO.SearchByParameters, string>>();
                                transactionsPaymentsSearchParams.Add(new KeyValuePair<Semnox.Parafait.Device.PaymentGateway.TransactionPaymentsDTO.SearchByParameters, string>(Semnox.Parafait.Device.PaymentGateway.TransactionPaymentsDTO.SearchByParameters.TRANSACTION_ID, reservationBL.BookingTransaction.Trx_id.ToString()));

                                List<Semnox.Parafait.Device.PaymentGateway.TransactionPaymentsDTO> transactionPaymentsDTOs = transactionPaymentsListBL.GetTransactionPaymentsDTOList(transactionsPaymentsSearchParams);

                                if (transactionPaymentsDTOs != null || transactionPaymentsDTOs.Count > 0)
                                {
                                    for (int i = 0; i < transactionPaymentsDTOs.Count(); i++)
                                    {
                                        foreach (Semnox.Parafait.Device.PaymentGateway.TransactionPaymentsDTO transactionPaymentsDTO in transactionPaymentsDTOs)
                                        {
                                            if (reservationBL.BookingTransaction.TransactionPaymentsDTOList[i].PaymentId == transactionPaymentsDTO.PaymentId)
                                            {
                                                reservationBL.BookingTransaction.TransactionPaymentsDTOList[i].ExternalSourceReference = transactionPaymentsDTO.ExternalSourceReference;
                                                break;
                                            }
                                        }
                                    }
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
            finally
            {
                StartInActivityTimeoutTimer();
            }
            log.LogMethodExit();
        }
        internal void ChangePrice(Transaction.TransactionLine productTrxLine, int lineIndex, int comboProductId)
        {
            log.LogMethodEntry(productTrxLine, lineIndex, comboProductId);
            if ((productTrxLine.ReceiptPrinted || productTrxLine.KDSSent)
               && ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "CANCEL_PRINTED_TRX_LINE") == false)
            {
                string msg = MessageContainerList.GetMessage(executionContext, "Transaction line already printed. Cannot perform the action.");
                log.Info(msg);
                throw new ValidationException(msg);
            }

            if (productTrxLine.AllowPriceOverride == false)
            {
                string msg = MessageContainerList.GetMessage(executionContext, "Price override is not allowed for the transaction line");
                log.Info(msg);
                throw new ValidationException(msg);
            }
            if (productTrxLine.AllowPriceOverride)
            {
                double Price = 0;

                Price = NumberPadForm.ShowNumberPadForm(MessageContainerList.GetMessage(executionContext, "Change Price"), '-', utilities);
                if (Price >= 0)
                {
                    if (Price == 0 && POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 485),
                         MessageContainerList.GetMessage(executionContext, "User Price"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.No)
                    {
                        log.Info("Price is 0");
                        return;
                    }
                    UpdateProductPrice(productTrxLine, Price);
                }
                else
                {
                    string msg = MessageContainerList.GetMessage(executionContext, 367, Convert.ToDouble("0").ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL));
                    log.Info(msg);
                    throw new ValidationException(msg);
                }
            }
            else
            {
                string msg = MessageContainerList.GetMessage(executionContext, "Price override is not allowed for &1 line(s)", productTrxLine.ProductName);
                log.Info(msg);
                throw new ValidationException(msg);
            }
            log.LogMethodExit();
        }
        private void UpdateProductPrice(Transaction.TransactionLine productTrxLine, double price)
        {
            log.LogMethodEntry(productTrxLine, price);
            if (price == -1)
            {
                productTrxLine.UserPrice = false;
                price = Convert.ToDouble(reservationBL.BookingTransaction.getProductDetails(productTrxLine.ProductID)["Price"]);
            }
            else
            {
                DataRow Product = reservationBL.BookingTransaction.getProductDetails(productTrxLine.ProductID);
                if (Product["MinimumUserPrice"] != DBNull.Value)
                {
                    if (price < Convert.ToDouble(Product["MinimumUserPrice"]))
                    {
                        string msg = MessageContainerList.GetMessage(executionContext, 367, Convert.ToDouble(Product["MinimumUserPrice"]).ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL));
                        log.Info(msg);
                        throw new ValidationException(msg);
                    }
                }
                if (productTrxLine.ModifierSetId != null && Convert.ToInt32(productTrxLine.ModifierSetId) > -1)
                {
                    productTrxLine.UserPrice = false;
                }
                else
                {
                    productTrxLine.UserPrice = true;
                }
            }

            if (productTrxLine.TaxInclusivePrice == "Y")
            {
                price = price / (1 + productTrxLine.tax_percentage / 100.0);
            }

            productTrxLine.Price = price;
            reservationBL.BookingTransaction.updateAmounts();
            log.LogMethodExit();
        }
        internal void ResetPrice(Transaction.TransactionLine productTrxLine, int lineIndex, int comboProductId)
        {
            log.LogMethodEntry(productTrxLine, lineIndex, comboProductId);
            if (productTrxLine.UserPrice)
            {
                UpdateProductPrice(productTrxLine, -1);
            }
            log.LogMethodExit();
        }
        private List<Transaction.TransactionLine> GetLinesAndLinkedChildLines(List<Transaction.TransactionLine> trxLineList, int productId, int comboProductId)
        {
            log.LogMethodEntry();
            try
            {
                List<Transaction.TransactionLine> selectedTrxLines = null;
                if (trxLineList != null)
                {
                    List<Transaction.TransactionLine> selectedParentTrxLines = trxLineList.Where(tl => tl.ProductID == productId
                                                                                                    && tl.ComboproductId == comboProductId
                                                                                                    && tl.LineValid == true
                                                                                                    && tl.CancelledLine == false).ToList();
                    if (selectedParentTrxLines != null && selectedParentTrxLines.Count > 0)
                    {
                        selectedTrxLines = LoadTrxLineChildDetails(trxLineList, selectedParentTrxLines);
                    }
                }

                log.LogMethodExit(selectedTrxLines);
                return selectedTrxLines;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(ex.Message);
                throw;
            }
        }
        private List<Transaction.TransactionLine> LoadTrxLineChildDetails(List<Transaction.TransactionLine> trxLineList, List<Transaction.TransactionLine> selectedParentTrxLines)
        {
            log.LogMethodEntry();
            List<Transaction.TransactionLine> selectedTrxLines = new List<Transaction.TransactionLine>();
            if (selectedParentTrxLines != null)
            {
                foreach (Transaction.TransactionLine parentTrxLine in selectedParentTrxLines)
                {
                    selectedTrxLines.Add(parentTrxLine);
                    List<Transaction.TransactionLine> selectedChildTrxLines = trxLineList.Where(tl => tl.ParentLine == parentTrxLine
                                                                                                     && tl.LineValid == true && tl.CancelledLine == false).ToList();

                    if (selectedChildTrxLines != null && selectedChildTrxLines.Count > 0)
                    {
                        List<Transaction.TransactionLine> selectedSubChildTrxLines = LoadTrxLineChildDetails(trxLineList, selectedChildTrxLines);
                        if (selectedSubChildTrxLines != null)
                            selectedTrxLines.AddRange(selectedSubChildTrxLines);
                    }
                }
            }
            log.LogMethodExit(selectedTrxLines);
            return selectedTrxLines;
        }
        private void cbxHideBookedSlots_CheckedChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                LoadSchedules();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        private void SetHideBookedSlotFlag()
        {
            log.LogMethodEntry();
            if (facilityMapDTOList != null && facilityMapDTOList.Any())
            {
                hideBookedSlots = facilityMapDTOList.Exists(fm => fm.FacilityMapDetailsDTOList != null && fm.FacilityMapDetailsDTOList.Any()
                                               && fm.FacilityMapDetailsDTOList.Exists(fmd => fmd.IsActive && fmd.FacilityDTOList != null && fmd.FacilityDTOList.Any()
                                                                                       && fmd.FacilityDTOList.Exists(fac => fac.ActiveFlag && fac.AllowMultipleBookings == false)));
            }
            log.LogMethodExit();
        }


        private void EnableDisableChargeButtons()
        {
            log.LogMethodEntry();
            bool inEditMode = BookingIsInEditMode();
            if (autoChargeOptionEnabled)
            {
                gbxCharges.Visible = true;
                gbxSvcServiceCharge.Visible = false;
                lblSvcServiceChargeSummary.Visible = false;
                txtSvcTrxServiceCharges.Visible = false;
                if (this.reservationBL.HasServiceCharges())
                {
                    pcbRemoveServiceCharge.Visible = true;
                    pcbRemoveServiceCharge.Enabled = inEditMode;
                    btnApplyServiceCharge.Visible = false;
                    btnApplyServiceCharge.Enabled = false;
                }
                else
                {
                    pcbRemoveServiceCharge.Visible = false;
                    pcbRemoveServiceCharge.Enabled = false;
                    btnApplyServiceCharge.Visible = true;
                    btnApplyServiceCharge.Enabled = inEditMode;
                }
                if (this.reservationBL.HasAutoGratuityAmount())
                {
                    pcbRemoveGratuityAmount.Visible = true;
                    pcbRemoveGratuityAmount.Enabled = inEditMode;
                    btnApplyGratuityAmount.Visible = false;
                    btnApplyGratuityAmount.Enabled = false;
                }
                else
                {
                    pcbRemoveGratuityAmount.Visible = false;
                    pcbRemoveGratuityAmount.Enabled = false;
                    btnApplyGratuityAmount.Visible = true;
                    btnApplyGratuityAmount.Enabled = inEditMode;
                }
            }
            else
            {
                gbxCharges.Visible = false;
                gbxSvcServiceCharge.Visible = true;
                lblSvcServiceChargeSummary.Visible = true;
                txtSvcTrxServiceCharges.Visible = true;
                txtSvcServiceChargeAmount.Enabled = inEditMode;
                txtSvcServiceChargePercentage.Enabled = inEditMode;
                pbcSvcRemoveServiceCharge.Enabled = inEditMode;
            }
            log.LogMethodExit();
        }
        private ProductsDTO GetProductsDTO(int productId)
        {
            log.LogMethodEntry(productId);
            if (productDictonary.ContainsKey(productId) == false)
            {
                Products productsBl = new Products(productId);
                productDictonary.Add(productId, productsBl.GetProductsDTO);
            }
            ProductsDTO productsDTO = productDictonary[productId];
            log.LogMethodExit();
            return productsDTO;
        }

        private void pcbRemoveServiceCharge_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            this.Cursor = Cursors.WaitCursor;
            try
            {
                POSUtils.SetLastActivityDateTime();
                txtMessage.Clear();
                if (BookingIsInEditMode())
                {
                    try
                    {
                        if (reservationBL.HasServiceCharges())
                        {//"Do you want to remove Service charges?"
                            if (POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 2109), MessageContainerList.GetMessage(executionContext, "Remove"), MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                this.Cursor = Cursors.WaitCursor;
                                try
                                {
                                    this.Cursor = Cursors.WaitCursor;
                                    StopInActivityTimeoutTimer();
                                    reservationBL.CancelServiceChargeLine(null);
                                    txtServiceChrageAmount.Text = string.Empty;
                                }
                                finally
                                {
                                    StartInActivityTimeoutTimer();
                                    this.Cursor = Cursors.WaitCursor;
                                }
                                LoadSummaryDetails();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        txtMessage.Text = ex.Message;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                txtMessage.Text = ex.Message;
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }

        private void pcbRemoveGratuityAmount_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.Cursor = Cursors.WaitCursor;
            try
            {
                POSUtils.SetLastActivityDateTime();
                txtMessage.Clear();
                if (BookingIsInEditMode())
                {
                    try
                    {
                        if (reservationBL.HasAutoGratuityAmount())
                        {//"Do you want to remove gratuity amount?"
                            if (POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, "Do you want to remove gratuity amount?"), MessageContainerList.GetMessage(executionContext, "Remove"), MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                this.Cursor = Cursors.WaitCursor;
                                try
                                {
                                    StopInActivityTimeoutTimer();
                                    reservationBL.CancelAutoGratuityLine(null);
                                    txtGratuityAmount.Text = string.Empty;
                                }
                                finally
                                {
                                    StartInActivityTimeoutTimer();
                                    this.Cursor = Cursors.WaitCursor;
                                }
                                LoadSummaryDetails();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        txtMessage.Text = ex.Message;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                txtMessage.Text = ex.Message;
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }

        private void btnApplyServiceCharge_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.Cursor = Cursors.WaitCursor;
            try
            {
                POSUtils.SetLastActivityDateTime();
                txtMessage.Clear();
                if (BookingIsInEditMode())
                {
                    try
                    {
                        if (reservationBL.HasServiceCharges() == false)
                        {   //"Do you want to apply service charges?"
                            if (POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 4513), MessageContainerList.GetMessage(executionContext, "Apply"), MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                try
                                {
                                    POSUtils.SetLastActivityDateTime();
                                    StopInActivityTimeoutTimer();
                                    this.Cursor = Cursors.WaitCursor;
                                    reservationBL.ApplyServiceCharges(null);
                                    LoadSummaryDetails();
                                }
                                finally
                                {
                                    StartInActivityTimeoutTimer();
                                }
                            }
                        }
                        else
                        {
                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, 4504));
                            // "Sorry, cannot apply service charges again. Transaction already has entry for the service charges"
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        txtMessage.Text = ex.Message;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                txtMessage.Text = ex.Message;
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit();

        }

        private void btnApplyGratuityAmount_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.Cursor = Cursors.WaitCursor;
            try
            {
                POSUtils.SetLastActivityDateTime();
                txtMessage.Clear();
                if (BookingIsInEditMode())
                {
                    try
                    {
                        if (reservationBL.HasAutoGratuityAmount() == false)
                        {   //Do you want to apply gratuity amount?
                            if (POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 4512), MessageContainerList.GetMessage(executionContext, "Apply"), MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                try
                                {
                                    POSUtils.SetLastActivityDateTime();
                                    StopInActivityTimeoutTimer();
                                    this.Cursor = Cursors.WaitCursor;
                                    reservationBL.ApplyAutoGratuityAmount(null);
                                    LoadSummaryDetails();
                                }
                                finally
                                {
                                    StartInActivityTimeoutTimer();
                                }
                            }
                        }
                        else
                        {
                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, 4505));
                            // "Sorry, cannot apply gratuity amount again. Transaction already has entry for the gratuity amount"
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        txtMessage.Text = ex.Message;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                txtMessage.Text = ex.Message;
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }

        private void SetServiceCharges()
        {
            log.LogMethodEntry();
            if (BookingIsInEditMode() && reservationBL.ReservationTransactionIsNotNull() && autoChargeOptionEnabled == false)
            {
                double serviceChargeAmount = 0;
                double.TryParse(txtSvcServiceChargeAmount.Text, out serviceChargeAmount);
                double serviceChargePercentage = 0;
                double.TryParse(txtSvcServiceChargePercentage.Text, out serviceChargePercentage);

                if (reservationBL.GetReservationDTO.ServiceChargeAmount != serviceChargeAmount)
                {
                    reservationBL.GetReservationDTO.ServiceChargeAmount = serviceChargeAmount;
                }
                if (reservationBL.GetReservationDTO.ServiceChargePercentage != serviceChargePercentage)
                {
                    reservationBL.GetReservationDTO.ServiceChargePercentage = serviceChargePercentage;
                }
            }
            log.LogMethodExit();
        }
        private void txtSvcServiceCharge_TextChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                POSUtils.SetLastActivityDateTime();
                txtMessage.Clear();
                TextBox selectTextBox = (TextBox)sender;
                double validValue = 0;
                if (string.IsNullOrEmpty(selectTextBox.Text) == false)
                {
                    if (double.TryParse(selectTextBox.Text, out validValue) == false)
                    {
                        txtMessage.Text = MessageContainerList.GetMessage(executionContext, 648);
                        this.ActiveControl = selectTextBox;
                        selectTextBox.Text = string.Empty;
                    }
                    else if (string.IsNullOrEmpty(txtSvcServiceChargeAmount.Text) == false && string.IsNullOrEmpty(txtSvcServiceChargePercentage.Text) == false)
                    {
                        txtMessage.Text = MessageContainerList.GetMessage(executionContext, 2108);// "Please provide service charge amount or percentage"
                        this.ActiveControl = selectTextBox;
                        selectTextBox.Text = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        private void txtSvcServiceCharge_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                POSUtils.SetLastActivityDateTime();
                txtMessage.Clear();
                TextBox textBoxObject = (TextBox)sender;

                double serviceCharge = (double)NumberPadForm.ShowNumberPadForm(MessageContainerList.GetMessage(executionContext, "Please enter Service Charge value"), (string.IsNullOrEmpty(textBoxObject.Text) == true ? "0" : textBoxObject.Text), utilities);//"Please enter Guest count"
                if (serviceCharge > 0 && serviceCharge < 10000)
                {
                    textBoxObject.Text = serviceCharge.ToString();
                }
                else if (serviceCharge <= 0)
                {
                    textBoxObject.Text = string.Empty;
                }
                this.ActiveControl = pbcSvcRemoveServiceCharge;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        private void pcbSvcRemoveServiceCharge_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            this.Cursor = Cursors.WaitCursor;
            try
            {
                POSUtils.SetLastActivityDateTime();
                txtMessage.Clear();
                if (BookingIsInEditMode())
                {
                    try
                    {
                        if (reservationBL.HasServiceCharges())
                        {//"Do you want to remove Service charges?"
                            if (POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 2109), MessageContainerList.GetMessage(executionContext, "Remove"), MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                this.Cursor = Cursors.WaitCursor; try
                                {
                                    POSUtils.SetLastActivityDateTime();
                                    StopInActivityTimeoutTimer();
                                    reservationBL.RemoveManuallyAppliedServiceCharges(null);
                                    txtSvcServiceChargeAmount.Text = txtSvcServiceChargePercentage.Text = string.Empty;
                                    LoadSummaryDetails();
                                }
                                finally
                                {
                                    StartInActivityTimeoutTimer();
                                }
                            }
                        }
                        else
                        {
                            txtSvcServiceChargeAmount.Text = txtSvcServiceChargePercentage.Text = string.Empty;
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        txtMessage.Text = ex.Message;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                txtMessage.Text = ex.Message;
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }
        private void inActivityTimerClock_Tick(object sender, EventArgs e)
        {
            inActivityTimerClock.Stop();
            try
            {
                DateTime lastActivityTimeValue = POSUtils.GetPOSLastTrxActivityTime();
                int inactivityPeriodSec = (int)(DateTime.Now - (DateTime)lastActivityTimeValue).TotalSeconds;

                if (inactivityPeriodSec > POSStatic.POS_INACTIVE_TIMEOUT)//inactivityPeriodSec > 60
                {
                    CallPerformActivityTimeOutChecks(inactivityPeriodSec);
                }
                else
                {
                    //EnableFormElements();
                    StartInActivityTimeoutTimer1();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }
        private void CallPerformActivityTimeOutChecks(int inactivityPeriodSec)
        {
            log.LogMethodEntry();
            if (POSUtils.MessageBoxIsOpen() == false)
            {
                if (PerformActivityTimeOutChecks != null)
                {
                    try
                    {
                        PerformActivityTimeOutChecks(inactivityPeriodSec);
                    }
                    finally
                    {
                        StartInActivityTimeoutTimer();
                    }
                }
                else
                {
                    POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext, "Timeout occured, closing the form"),
                                                            MessageContainerList.GetMessage(utilities.ExecutionContext, "Time Out"));
                    POSUtils.ForceCloseCurrentScreen(this);
                }
            }
            else
            {
                StartInActivityTimeoutTimer1();
            }
            log.LogMethodExit();
        }

        private void frmReservationUI_FormClosed(object sender, FormClosedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                this.inActivityTimerClock.Stop();
                this.inActivityTimerClock.Tick += null;
                this.inActivityTimerClock.Dispose();
                this.PerformActivityTimeOutChecks += null;
                this.Dispose();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void ScrollBar_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }
        private void StartInActivityTimeoutTimer()
        {
            log.LogMethodEntry();
            try
            {
                if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "RELOGIN_USER_AFTER_INACTIVE_TIMEOUT", false))
                {
                    inActivityTimerClock.Start();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        private void StartInActivityTimeoutTimer1()
        {
            log.LogMethodEntry();
            try
            {
                if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "RELOGIN_USER_AFTER_INACTIVE_TIMEOUT", false))
                {
                    inActivityTimerClock.Start();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        private void StopInActivityTimeoutTimer()
        {
            log.LogMethodEntry();
            try
            {
                POSUtils.SetLastActivityDateTime();
                inActivityTimerClock.Stop();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
    }
}
