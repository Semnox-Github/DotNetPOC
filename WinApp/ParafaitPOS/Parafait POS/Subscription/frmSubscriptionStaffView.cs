/********************************************************************************************
 * Project Name - frmSubscriptionStaffView 
 * Description  - form class to show Customer Subscriptions in staff view
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************   
 *2.110.0     21-Dec-2020    Guru S A             Created for Subscription changes                                                                               
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;
using Semnox.Parafait.SubscriptionBillingProgram;
using Semnox.Parafait.Transaction;

namespace Parafait_POS.Subscription
{
    /// <summary>
    /// frmSubscriptionStaffView
    /// </summary>
    public partial class frmSubscriptionStaffView : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private Utilities utilities;
        private const int DATE_OPTION_TODAY = 0;
        private const int DATE_OPTION_ONE_WEEK = 1;
        private const int DATE_OPTION_30_DAYS = 2;
        private const int DATE_OPTION_90_DAYS = 3;
        private const string SEARCH_DATE_FORMAT = "MM-dd-yyyy HH:mm:ss";
        private string NUMBER_FORMAT;
        private string AMOUNT_FORMAT;
        private string DATETIME_FORMAT;
        private string DATE_FORMAT;
        private const string LOG_TYPE_PAYMENT_ERROR = "PaymentError";

        private const string SUBSCRIPTION_FEATURE_SETUP = "SUBSCRIPTION_FEATURE_SETUP";
        private const string DefaultForDueForExpiryIn = "DefaultForDueForExpiryIn";
        private int defaultForDueForExpiryIn = 7;

        private List<SubscriptionJobProcessDTO> subscriptionJobProcessDTOList = null;
        private List<SubscriptionHeaderDTO> expiryingSubscriptionsDTOList = null;
        private List<CustomerCreditCardsDTO> expiringCustomerCreditCardsDTOList = null;
        private usrCtlSubscriptions usrCtlSubscriptionsSV;
        private bool fireEvent = true;
        /// <summary>
        /// frmSubscriptionStaffView
        /// </summary>
        /// <param name="utilities"></param>
        public frmSubscriptionStaffView(Utilities utilities)
        {
            log.LogMethodEntry();
            this.utilities = utilities;
            this.executionContext = utilities.ExecutionContext;
            InitializeComponent();
            LoadSubscriptionSetupDetails();
            SetFormats();
            SetFormElements();
            LoadStats();
            System.Drawing.Rectangle workingRectangle = Screen.PrimaryScreen.WorkingArea;
            log.LogVariableState("workingRectangle.Width", workingRectangle.Width);
            log.LogVariableState("this.Width", this.Width);
            log.LogVariableState("tcDashboard.Size- Before", this.tcDashboard.Size);
            if (this.Width > workingRectangle.Width - 100)
            {
                this.tcDashboard.Size = new System.Drawing.Size(workingRectangle.Width - 20, tcDashboard.Height);
                this.tpDashboardView.AutoScroll = true;
                this.tpSubscriptions.AutoScroll = true;
            }
            log.LogVariableState("tcDashboard.Size- After", this.tcDashboard.Size);
            this.utilities.setLanguage(this);
            log.LogMethodExit();
        }

        private void SetFormats()
        {
            log.LogMethodEntry();
            NUMBER_FORMAT = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "NUMBER_FORMAT");
            AMOUNT_FORMAT = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "AMOUNT_FORMAT");
            DATETIME_FORMAT = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "DATETIME_FORMAT");
            DATE_FORMAT = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "DATE_FORMAT");
            log.LogMethodExit();
        }

        private void SetFormElements()
        {
            log.LogMethodEntry();
            LoadSearchDateOptionList();
            dtpFromDate.CustomFormat = dtpToDate.CustomFormat = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, DATE_FORMAT);
            SetDatePickerDates(DATE_OPTION_TODAY);
            LoadDayOptionLists();
            log.LogMethodExit();
        }

        private void LoadSearchDateOptionList()
        {
            log.LogMethodEntry();
            try
            {
                fireEvent = false;
                List<KeyValuePair<int, string>> searchDateOptionList = new List<KeyValuePair<int, string>>();
                searchDateOptionList.Add(new KeyValuePair<int, string>(0, MessageContainerList.GetMessage(executionContext, "Today")));
                searchDateOptionList.Add(new KeyValuePair<int, string>(1, MessageContainerList.GetMessage(executionContext, "Last 1 Week")));
                searchDateOptionList.Add(new KeyValuePair<int, string>(2, MessageContainerList.GetMessage(executionContext, "Last 30 Days")));
                searchDateOptionList.Add(new KeyValuePair<int, string>(3, MessageContainerList.GetMessage(executionContext, "Last 90 Days")));
                cmbSearchDateOption.DataSource = searchDateOptionList;
                cmbSearchDateOption.ValueMember = "Key";
                cmbSearchDateOption.DisplayMember = "Value";
            }
            finally
            {
                fireEvent = true;
            }
            log.LogMethodExit();
        }


        private void LoadDayOptionLists()
        {
            log.LogMethodEntry();
            int[] searchDateOptionArray = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 60, 90 };
            int[] searchDateOptionArrayTwo = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 60, 90 };
            int[] searchDateOptionArrayThree = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 60, 90 };

            try
            {
                fireEvent = false;
                cmbCardExpiryInDays.DataSource = searchDateOptionArray;
                cmbSubscriptionExpiryInDays.DataSource = searchDateOptionArrayTwo;
                cmbBillingFailureInDays.DataSource = searchDateOptionArrayThree;
                SetDefaultForDueForExpiryIn(searchDateOptionArray);
            }
            finally
            {
                fireEvent = true;
            }
            log.LogMethodExit();
        }

        private void SetDefaultForDueForExpiryIn(int[] searchDateOptionArray)
        {
            log.LogMethodEntry(searchDateOptionArray);
            try
            {
                int indexValue = Array.FindIndex(searchDateOptionArray, dateOption => dateOption == defaultForDueForExpiryIn);
                if (indexValue > -1)
                {
                    cmbCardExpiryInDays.SelectedIndex = indexValue;
                    cmbSubscriptionExpiryInDays.SelectedIndex = indexValue;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void SetDatePickerDates(int searchDateOption)
        {
            log.LogMethodEntry();
            try
            {
                switch (searchDateOption)
                {
                    case DATE_OPTION_ONE_WEEK:
                        dtpFromDate.Value = DateTime.Now.Date.AddDays(-7);
                        dtpToDate.Value = DateTime.Now.Date.AddDays(1);
                        break;
                    case DATE_OPTION_30_DAYS:
                        dtpFromDate.Value = DateTime.Now.Date.AddDays(-30);
                        dtpToDate.Value = DateTime.Now.Date.AddDays(1);
                        break;
                    case DATE_OPTION_90_DAYS:
                        dtpFromDate.Value = DateTime.Now.Date.AddDays(-90);
                        dtpToDate.Value = DateTime.Now.Date.AddDays(1);
                        break;
                    default:
                        dtpFromDate.Value = DateTime.Now.Date;
                        dtpToDate.Value = DateTime.Now.Date.AddDays(1);
                        break;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        private void LoadStats()
        {
            log.LogMethodEntry();
            LoadDashboardTab();
            log.LogMethodExit();
        }

        private void LoadDashboardTab()
        {
            log.LogMethodEntry();
            try
            {
                LoadActivationCancellationData();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            try
            {
                LoadActiveCustomerSubscriptionData();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            try
            {
                LoadMonthlyRecurringRevenueData();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            try
            {
                LoadMonthlyChurnData();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            try
            {
                LoadSubscriptionExpiryData();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            try
            {
                LoadSubscriptionCardExpiryData();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            try
            {
                LoadPaymentFailureTransactionData();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void LoadActivationCancellationData()
        {
            log.LogMethodEntry();
            ValidateDTP();
            SubscriptionHeaderListBL subscriptionHeaderListBL = new SubscriptionHeaderListBL(executionContext);
            List<KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>(SubscriptionHeaderDTO.SearchByParameters.STATUS, SubscriptionStatus.ACTIVE));
            searchParameters.Add(new KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>(SubscriptionHeaderDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            searchParameters.Add(new KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>(SubscriptionHeaderDTO.SearchByParameters.CREATION_DATE_GREATER_EQUAL_TO, dtpFromDate.Value.ToString(SEARCH_DATE_FORMAT, CultureInfo.InvariantCulture)));
            searchParameters.Add(new KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>(SubscriptionHeaderDTO.SearchByParameters.CREATION_DATE_LESS_THAN, dtpToDate.Value.ToString(SEARCH_DATE_FORMAT, CultureInfo.InvariantCulture)));
            List<SubscriptionHeaderDTO> subscriptionHeaderDTOList = subscriptionHeaderListBL.GetSubscriptionHeaderDTOList(searchParameters, utilities);
            txtActivations.Text = (subscriptionHeaderDTOList == null) ? "0" : subscriptionHeaderDTOList.Count.ToString(NUMBER_FORMAT);
            searchParameters.Clear();
            subscriptionHeaderDTOList = null;
            //searchParameters.Add(new KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>(SubscriptionHeaderDTO.SearchByParameters.STATUS, SubscriptionStatus.ACTIVE));
            searchParameters.Add(new KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>(SubscriptionHeaderDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            searchParameters.Add(new KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>(SubscriptionHeaderDTO.SearchByParameters.CANCELLATION_DATE_GREATER_EQUAL_TO, dtpFromDate.Value.ToString(SEARCH_DATE_FORMAT, CultureInfo.InvariantCulture)));
            searchParameters.Add(new KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>(SubscriptionHeaderDTO.SearchByParameters.CANCELLATION_DATE_LESS_THAN, dtpToDate.Value.ToString(SEARCH_DATE_FORMAT, CultureInfo.InvariantCulture)));
            subscriptionHeaderDTOList = subscriptionHeaderListBL.GetSubscriptionHeaderDTOList(searchParameters, utilities);
            txtCancellations.Text = (subscriptionHeaderDTOList == null) ? "0" : subscriptionHeaderDTOList.Count.ToString(NUMBER_FORMAT);
            log.LogMethodExit();
        }

        private void ValidateDTP()
        {
            log.LogMethodEntry();
            DateTime fromDateValue;
            DateTime toDateValue;
            if (DateTime.TryParse(dtpFromDate.Value.ToString(), out fromDateValue) == false)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Please Select From Date"));
            }
            if (DateTime.TryParse(dtpToDate.Value.ToString(), out toDateValue) == false)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Please Select To Date"));
            }
            if (fromDateValue >= toDateValue)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 724));//To Date should be greater than From Date
            }
            log.LogMethodExit();
        }

        private void LoadActiveCustomerSubscriptionData()
        {
            log.LogMethodEntry();
            SubscriptionHeaderListBL subscriptionHeaderListBL = new SubscriptionHeaderListBL(executionContext);
            List<KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>(SubscriptionHeaderDTO.SearchByParameters.STATUS, SubscriptionStatus.ACTIVE));
            searchParameters.Add(new KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>(SubscriptionHeaderDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            List<SubscriptionHeaderDTO> subscriptionHeaderDTOList = subscriptionHeaderListBL.GetSubscriptionHeaderDTOList(searchParameters, utilities);
            txtTotalActiveCustomers.Text = (subscriptionHeaderDTOList == null) ? "0"
                                            : subscriptionHeaderDTOList.Select(sh => sh.CustomerId).Distinct().ToList().Count.ToString(NUMBER_FORMAT);
            txtTotalActiveSubscriptions.Text = (subscriptionHeaderDTOList == null) ? "0"
                                           : subscriptionHeaderDTOList.Select(sh => sh.SubscriptionHeaderId).Distinct().ToList().Count.ToString(NUMBER_FORMAT);
            log.LogMethodExit();
        }

        private void LoadMonthlyRecurringRevenueData()
        {
            log.LogMethodEntry();
            DateTime refDate = DateTime.Now.Day != 1 ? DateTime.Now : DateTime.Now.AddDays(-1);
            DateTime monthStartDate = new DateTime(refDate.Year, refDate.Month, 01);
            DateTime dateValue = refDate.Date.AddMonths(1);
            DateTime nextMonthStartDate = new DateTime(dateValue.Year, dateValue.Month, 01);
            SubscriptionBillingScheduleListBL subscriptionBillingScheduleListBL = new SubscriptionBillingScheduleListBL(executionContext);
            List<KeyValuePair<SubscriptionBillingScheduleDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<SubscriptionBillingScheduleDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<SubscriptionBillingScheduleDTO.SearchByParameters, string>(SubscriptionBillingScheduleDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            searchParameters.Add(new KeyValuePair<SubscriptionBillingScheduleDTO.SearchByParameters, string>(SubscriptionBillingScheduleDTO.SearchByParameters.BILL_ON_GREATER_THAN_OR_EQUAL_TO, monthStartDate.ToString(SEARCH_DATE_FORMAT, CultureInfo.InvariantCulture)));
            searchParameters.Add(new KeyValuePair<SubscriptionBillingScheduleDTO.SearchByParameters, string>(SubscriptionBillingScheduleDTO.SearchByParameters.BILL_ON_LESS_THAN, nextMonthStartDate.ToString(SEARCH_DATE_FORMAT, CultureInfo.InvariantCulture)));
            searchParameters.Add(new KeyValuePair<SubscriptionBillingScheduleDTO.SearchByParameters, string>(SubscriptionBillingScheduleDTO.SearchByParameters.STATUS, SubscriptionStatus.ACTIVE));
            List<SubscriptionBillingScheduleDTO> subscriptionBillingScheduleDTOList = subscriptionBillingScheduleListBL.GetSubscriptionBillingScheduleDTOList(searchParameters);
            decimal revenueAmount = 0;
            if (subscriptionBillingScheduleDTOList != null && subscriptionBillingScheduleDTOList.Any())
            {
                revenueAmount = subscriptionBillingScheduleDTOList.Sum(sbs => (sbs.OverridedBillAmount != null ? (decimal)sbs.OverridedBillAmount : sbs.BillAmount));
            }
            txtMonthlyRecurringRevenue.Text = (revenueAmount == 0) ? "0" : revenueAmount.ToString(AMOUNT_FORMAT);
            log.LogMethodExit();
        }

        private void LoadMonthlyChurnData()
        {
            log.LogMethodEntry();
            DateTime monthStartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 01);
            DateTime dateValue = DateTime.Now.Date.AddMonths(-1);
            DateTime previousMonthStartDate = new DateTime(dateValue.Year, dateValue.Month, 01);
            SubscriptionHeaderListBL subscriptionHeaderListBL = new SubscriptionHeaderListBL(executionContext);
            List<KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>(SubscriptionHeaderDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            searchParameters.Add(new KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>(SubscriptionHeaderDTO.SearchByParameters.CREATION_DATE_LESS_THAN, previousMonthStartDate.ToString(SEARCH_DATE_FORMAT, CultureInfo.InvariantCulture)));
            List<SubscriptionHeaderDTO> previousMonthStarList = subscriptionHeaderListBL.GetSubscriptionHeaderDTOList(searchParameters, utilities);
            searchParameters.Clear();
            searchParameters.Add(new KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>(SubscriptionHeaderDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            searchParameters.Add(new KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>(SubscriptionHeaderDTO.SearchByParameters.CREATION_DATE_LESS_THAN, monthStartDate.ToString(SEARCH_DATE_FORMAT, CultureInfo.InvariantCulture)));
            List<SubscriptionHeaderDTO> thisMonthStarList = subscriptionHeaderListBL.GetSubscriptionHeaderDTOList(searchParameters, utilities);
            int customerCountOnBeginingOfPrevMonth = (previousMonthStarList == null ? 0 : previousMonthStarList.Select(sh => sh.CustomerId).Distinct().ToList().Count);
            int customerDivserValue = customerCountOnBeginingOfPrevMonth;
            if (customerCountOnBeginingOfPrevMonth == 0)
            {
                customerDivserValue = 1;
            }
            int customerCountOnEndOfPrevMonth = (thisMonthStarList == null ? 0 : thisMonthStarList.Select(sh => sh.CustomerId).Distinct().ToList().Count);
            decimal customerChurn = (customerCountOnBeginingOfPrevMonth - customerCountOnEndOfPrevMonth) / customerDivserValue;
            customerChurn = customerChurn > -1 ? customerChurn : 0;
            txtMonthlyCustomerChurnRate.Text = (customerChurn == 0) ? "0" : customerChurn.ToString(NUMBER_FORMAT);

            int subscriptionCountOnBeginingOfPrevMonth = (previousMonthStarList == null ? 0 : previousMonthStarList.Select(sh => sh.SubscriptionHeaderId).Distinct().ToList().Count);
            int subscriptionDivserValue = subscriptionCountOnBeginingOfPrevMonth;
            if (customerCountOnBeginingOfPrevMonth == 0)
            {
                subscriptionDivserValue = 1;
            }
            int subscriptionCountOnEndOfPrevMonth = (thisMonthStarList == null ? 0 : thisMonthStarList.Select(sh => sh.SubscriptionHeaderId).Distinct().ToList().Count);
            decimal subscriptionChurn = (subscriptionCountOnBeginingOfPrevMonth - subscriptionCountOnEndOfPrevMonth) / subscriptionDivserValue;
            subscriptionChurn = subscriptionChurn > -1 ? subscriptionChurn : 0;
            txtSubscriptionChurnRate.Text = (subscriptionChurn == 0) ? "0" : subscriptionChurn.ToString(NUMBER_FORMAT);
            log.LogMethodExit();
        }

        private void LoadSubscriptionExpiryData()
        {
            log.LogMethodEntry();
            ValidateSubscriptionExpiryDays();
            SubscriptionHeaderListBL subscriptionHeaderListBL = new SubscriptionHeaderListBL(executionContext);
            List<KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>(SubscriptionHeaderDTO.SearchByParameters.STATUS, SubscriptionStatus.ACTIVE));
            searchParameters.Add(new KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>(SubscriptionHeaderDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            searchParameters.Add(new KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>(SubscriptionHeaderDTO.SearchByParameters.SUBSCRIPTION_EXPIRES_IN_XDAYS, cmbSubscriptionExpiryInDays.SelectedValue.ToString()));
            expiryingSubscriptionsDTOList = subscriptionHeaderListBL.GetSubscriptionHeaderDTOList(searchParameters, utilities, true);
            btnExpiryingSubscriptions.Text = (expiryingSubscriptionsDTOList == null) ? "0" : expiryingSubscriptionsDTOList.Count.ToString(NUMBER_FORMAT);
            log.LogMethodExit();
        }

        private void ValidateSubscriptionExpiryDays()
        {
            log.LogMethodEntry();
            int xDays;
            if (cmbSubscriptionExpiryInDays.SelectedValue == null ||
                int.TryParse(cmbSubscriptionExpiryInDays.SelectedValue.ToString(), out xDays) == false)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Expiry Days")));
            }
            log.LogMethodExit();
        }

        private void LoadSubscriptionCardExpiryData()
        {
            log.LogMethodEntry();
            ValidateCardExpiryDays();
            CustomerCreditCardsListBL customerCreditCardsListBL = new CustomerCreditCardsListBL(executionContext);
            List<KeyValuePair<CustomerCreditCardsDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CustomerCreditCardsDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<CustomerCreditCardsDTO.SearchByParameters, string>(CustomerCreditCardsDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            searchParameters.Add(new KeyValuePair<CustomerCreditCardsDTO.SearchByParameters, string>(CustomerCreditCardsDTO.SearchByParameters.LINKED_WITH_ACTIVE_SUBSCRIPTIONS, "1"));
            searchParameters.Add(new KeyValuePair<CustomerCreditCardsDTO.SearchByParameters, string>(CustomerCreditCardsDTO.SearchByParameters.CARDS_EXPIRING_IN_X_DAYS, cmbCardExpiryInDays.SelectedValue.ToString()));
            expiringCustomerCreditCardsDTOList = customerCreditCardsListBL.GetCustomerCreditCardsDTOList(searchParameters, utilities);
            btnExpiryingCreditCards.Text = (expiringCustomerCreditCardsDTOList == null) ? "0" : expiringCustomerCreditCardsDTOList.Count.ToString(NUMBER_FORMAT);
            log.LogMethodExit();
        }

        private void ValidateCardExpiryDays()
        {
            log.LogMethodEntry();
            int xDays;
            if (cmbCardExpiryInDays.SelectedValue == null ||
                int.TryParse(cmbCardExpiryInDays.SelectedValue.ToString(), out xDays) == false)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Expiry Days")));
            }
        }

        private void LoadPaymentFailureTransactionData()
        {
            log.LogMethodEntry();
            ValidateBillingFailureDays();
            subscriptionJobProcessDTOList = null;
            SubscriptionJobProcessListBL subscriptionJobProcessListBL = new SubscriptionJobProcessListBL(executionContext);
            List<KeyValuePair<SubscriptionJobProcessDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<SubscriptionJobProcessDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<SubscriptionJobProcessDTO.SearchByParameters, string>(SubscriptionJobProcessDTO.SearchByParameters.REQUEST_RUN_DATE_IN_LAST_X_DAYS, cmbBillingFailureInDays.SelectedValue.ToString()));
            searchParameters.Add(new KeyValuePair<SubscriptionJobProcessDTO.SearchByParameters, string>(SubscriptionJobProcessDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            subscriptionJobProcessDTOList = subscriptionJobProcessListBL.GetAllSubscriptionJobProcessDTOList(searchParameters, true);
            int subScriptionsWithPaymentFailure = 0;
            if (subscriptionJobProcessDTOList != null && subscriptionJobProcessDTOList.Any())
            {
                List<SubscriptionJobProcessDTO> eligibleRecords = subscriptionJobProcessDTOList.Where(sjp => sjp.SubscriptionJobProcessLogDTOList != null
                                                                               && sjp.SubscriptionJobProcessLogDTOList.Any()
                                                                               && sjp.SubscriptionJobProcessLogDTOList.Exists(sjpl => sjpl.LogType == LOG_TYPE_PAYMENT_ERROR)).ToList();
                subScriptionsWithPaymentFailure = (eligibleRecords != null && eligibleRecords.Any() ? eligibleRecords.Select(sjp => sjp.SubscriptionHeaderId).Distinct().Count() : 0);
            }
            //Count of subscription billings with Payment failurs for today: &1
            btnPaymentErrorCount.Text = (subScriptionsWithPaymentFailure == 0 ? "0" : subScriptionsWithPaymentFailure.ToString(NUMBER_FORMAT));
            log.LogMethodExit();
        }

        private void ValidateBillingFailureDays()
        {
            log.LogMethodEntry();
            int xDays;
            if (cmbBillingFailureInDays.SelectedValue == null ||
                int.TryParse(cmbBillingFailureInDays.SelectedValue.ToString(), out xDays) == false)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Billing Failure")));
            }
        }

        private void LoadSubscriptionTab()
        {
            log.LogMethodEntry();
            try
            {
                this.Cursor = Cursors.WaitCursor;
                LoadSubscriptionUserControl(null);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 1824, ex.Message));
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }
        private void LoadSubscriptionUserControl(List<SubscriptionHeaderDTO> subscriptionHeaderDTOList)
        {
            log.LogMethodEntry(subscriptionHeaderDTOList);
            if (this.tpSubscriptions.Controls.Contains(this.usrCtlSubscriptionsSV))
            {
                this.tpSubscriptions.Controls.Remove(this.usrCtlSubscriptionsSV);
                this.usrCtlSubscriptionsSV = null;
            }
            this.usrCtlSubscriptionsSV = new usrCtlSubscriptions(utilities, -1, subscriptionHeaderDTOList);
            this.usrCtlSubscriptionsSV.BackColor = System.Drawing.Color.White;
            this.usrCtlSubscriptionsSV.Font = new System.Drawing.Font("Arial", 9F);
            this.usrCtlSubscriptionsSV.Location = new System.Drawing.Point(7, 7);
            this.usrCtlSubscriptionsSV.Name = "usrCtlSubscriptionsSV";
            this.usrCtlSubscriptionsSV.Size = new System.Drawing.Size(1213, 560);
            this.usrCtlSubscriptionsSV.TabIndex = 0;
            this.tpSubscriptions.SuspendLayout();
            this.tpSubscriptions.Controls.Add(this.usrCtlSubscriptionsSV);
            this.tpSubscriptions.ResumeLayout(true);
            log.LogMethodExit();
        }

        private void btnPaymentErrorCount_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (subscriptionJobProcessDTOList != null && subscriptionJobProcessDTOList.Any())
                {
                    List<SubscriptionJobProcessDTO> eligibleRecords = subscriptionJobProcessDTOList.Where(sjp => sjp.SubscriptionJobProcessLogDTOList != null
                                                                               && sjp.SubscriptionJobProcessLogDTOList.Any()
                                                                               && sjp.SubscriptionJobProcessLogDTOList.Exists(sjpl => sjpl.LogType == LOG_TYPE_PAYMENT_ERROR)).ToList();

                    if (eligibleRecords != null && eligibleRecords.Any())
                    {
                        List<int> subscriptionHeaderIdList = eligibleRecords.Select(sjp => sjp.SubscriptionHeaderId).Distinct().ToList();
                        if (subscriptionHeaderIdList != null && subscriptionHeaderIdList.Any())
                        {
                            SubscriptionHeaderListBL subscriptionHeaderListBL = new SubscriptionHeaderListBL(executionContext);
                            List<SubscriptionHeaderDTO> subscriptionHeaderDTOList = subscriptionHeaderListBL.GetSubscriptionHeaderDTOList(subscriptionHeaderIdList, true);
                            if (subscriptionHeaderDTOList != null && subscriptionHeaderDTOList.Any())
                            {
                                LoadSubscriptionUserControl(subscriptionHeaderDTOList);
                                tcDashboard.SelectedTab = tpSubscriptions;
                                this.Cursor = Cursors.Default;
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
            finally
            {
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }

        private void lightBlueBtn_MouseHover(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.Cursor = Cursors.Hand;
            log.LogMethodExit();
        }

        private void lightBlueBtn_MouseLeave(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.Cursor = Cursors.Default;
            log.LogMethodExit();

        }

        private void btnExpiryingSubscriptions_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (expiryingSubscriptionsDTOList != null && expiryingSubscriptionsDTOList.Any())
                {
                    LoadSubscriptionUserControl(expiryingSubscriptionsDTOList);
                    tcDashboard.SelectedTab = tpSubscriptions;
                    this.Cursor = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 1824, ex.Message));
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }

        private void btnExpiryingCreditCards_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (expiringCustomerCreditCardsDTOList != null && expiringCustomerCreditCardsDTOList.Any())
                {
                    List<int> cccIdList = expiringCustomerCreditCardsDTOList.Select(ccc => ccc.CustomerCreditCardsId).Distinct().ToList();
                    if (cccIdList != null && cccIdList.Any())
                    {
                        SubscriptionHeaderListBL subscriptionHeaderListBL = new SubscriptionHeaderListBL(executionContext);
                        List<SubscriptionHeaderDTO> subscriptionHeaderDTOList = subscriptionHeaderListBL.GetSubscriptionHeaderListByCreditCards(cccIdList, true);
                        if (subscriptionHeaderDTOList != null && subscriptionHeaderDTOList.Any())
                        {
                            LoadSubscriptionUserControl(subscriptionHeaderDTOList);
                            tcDashboard.SelectedTab = tpSubscriptions;
                            this.Cursor = Cursors.Default;
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
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                this.Cursor = Cursors.WaitCursor;
                LoadActivationCancellationData();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 1824, ex.Message));
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }
        private void cmbSearchDateOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                int selectedValue = -1;
                if (fireEvent)
                {
                    this.Cursor = Cursors.WaitCursor;
                    if (cmbSearchDateOption.SelectedValue != null && int.TryParse(cmbSearchDateOption.SelectedValue.ToString(), out selectedValue))
                    {
                        SetDatePickerDates(selectedValue);
                    }
                    else
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1787));
                        //Please select a valid option from the list
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
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit();

        }
        private void cmbSubscriptionExpiryInDays_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (fireEvent)
                {
                    this.Cursor = Cursors.WaitCursor;
                    LoadSubscriptionExpiryData();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 1824, ex.Message));
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }
        private void cmbCardExpiryInDays_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (fireEvent)
                {
                    LoadSubscriptionCardExpiryData();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 1824, ex.Message));
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
        private void cmbBillingFailureInDays_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (fireEvent)
                {
                    LoadPaymentFailureTransactionData();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 1824, ex.Message));
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
        private void frmSubscriptionStaffView_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            LoadSubscriptionTab();
            log.LogMethodExit();
        }
        private void blueButton_MouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                Button senderBtn = (Button)sender;
                senderBtn.BackgroundImage = global::Parafait_POS.Properties.Resources.pressed2;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void blueButton_MouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                Button senderBtn = (Button)sender;
                senderBtn.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void LoadSubscriptionSetupDetails()
        {
            log.LogMethodEntry();
            try
            {
                LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                searchParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, SUBSCRIPTION_FEATURE_SETUP));
                List<LookupValuesDTO> lookupValuesDTOList = lookupValuesList.GetAllLookupValues(searchParameters);
                if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
                {
                    for (int i = 0; i < lookupValuesDTOList.Count(); i++)
                    {
                        int value = 0;
                        switch (lookupValuesDTOList[i].LookupValue)
                        {
                            case DefaultForDueForExpiryIn:
                                if (int.TryParse(lookupValuesDTOList[i].Description, out value))
                                {
                                    defaultForDueForExpiryIn = value;
                                }
                                break;
                            default:
                                break;
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

        private void lightBlueBtn_MouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                Button senderBtn = (Button)sender;
                senderBtn.BackgroundImage = global::Parafait_POS.Properties.Resources.LightBlueBtn648X83_Normal;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();

        }

        private void lighBlueBtn_MouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                Button senderBtn = (Button)sender;
                senderBtn.BackgroundImage = global::Parafait_POS.Properties.Resources.LightBlueBtn648X83_Pressed;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();

        }

    }
}
