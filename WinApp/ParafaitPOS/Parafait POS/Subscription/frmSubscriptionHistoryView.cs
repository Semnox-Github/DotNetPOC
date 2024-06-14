/********************************************************************************************
 * Project Name - frmSubscriptionHistoryView 
 * Description  - form class to show Subscription history details
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************   
 *2.110.0     21-Dec-2020    Guru S A             Created for Subscription changes
 *2.120.0     18-Mar-2021    Guru S A             For Subscription phase 2 changes
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Product;
using Semnox.Parafait.Transaction;
using System;
using System.Collections.Generic; 
using System.Data;
using System.Drawing;
using System.Linq; 
using System.Windows.Forms;

namespace Parafait_POS.Subscription
{
    /// <summary>
    /// frmSubscriptionHistoryView
    /// </summary>
    public partial class frmSubscriptionHistoryView : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private Utilities utilities;
        private int subscriptionHeaderId;
        private SubscriptionHeaderDTO subscriptionHeaderDTO;
        private List<SubscriptionHeaderHistoryDTO> subscriptionHeaderHistoryDTOList;
        private List<SubscriptionBillingScheduleHistoryDTO> subscriptionBillingScheduleHistoryDTOList;  
        private bool fireCmbEvent = true;
        private string UOMDAYS_DESC = "";
        private string UOMMONTH_DESC = "";
        private string UOMYEAR_DESC = "";
        private string CYCLE_PAYMODE_DESC = "";
        private string FULL_PAYMODE_DESC = "";
        private string CHOICE_PAYMODE_DESC = "";
        private string CANCEL_UNBILLED_DESC = "";
        private string CANCEL_AUTO_REN_DESC = "";
        private string BILLING_LINE_DESC = "";
        private string GRACE_LINE_DESC = "";
        /// <summary>
        /// frmSubscriptionHistoryView
        /// </summary>
        /// <param name="utilities"></param>
        /// <param name="subscriptionHeaderId"></param>
        public frmSubscriptionHistoryView(Utilities utilities, int subscriptionHeaderId)
        {
            log.LogMethodEntry(subscriptionHeaderId);
            this.subscriptionHeaderId = subscriptionHeaderId;
            this.utilities = utilities;
            this.executionContext = utilities.ExecutionContext;
            InitializeComponent();
            System.Drawing.Rectangle workingRectangle = Screen.PrimaryScreen.WorkingArea;
            if (this.Width > workingRectangle.Width - 100)
            {
                this.SuspendLayout();
                this.Size = new System.Drawing.Size(workingRectangle.Width - 20, this.Height);
                this.pnlPage.Size = new System.Drawing.Size(this.Width - 10, pnlPage.Height);
                this.pnlPage.AutoScroll = true;
                this.ResumeLayout(true);
            }
            SetCodeDescriptions();
            LoadSubsscriptionDTO();
            LoadSubscriptionHisotryDTO();
            LoadSubscriptionBillingScheuleHisotryDTO();
            LoadTransactionIds();
            LoadCustomerIds();
            LoadCreditCards();
            LoadCustomerContacts();
            LoadLinkedTransactionIds();
            LoadCmbBillingCyclesList();
            SetDGVFormat();
            LoadHeaderHistoryDGV();            
            this.utilities.setLanguage(this);
            log.LogMethodExit();
        }
        private void SetCodeDescriptions()
        {
            log.LogMethodEntry();
            UOMDAYS_DESC = UnitOfSubscriptionCycle.GetUnitOfSubscriptionCycleDescription(executionContext, UnitOfSubscriptionCycle.DAYS);
            UOMMONTH_DESC = UnitOfSubscriptionCycle.GetUnitOfSubscriptionCycleDescription(executionContext, UnitOfSubscriptionCycle.MONTHS);
            UOMYEAR_DESC = UnitOfSubscriptionCycle.GetUnitOfSubscriptionCycleDescription(executionContext, UnitOfSubscriptionCycle.YEARS);
            CYCLE_PAYMODE_DESC = SubscriptionPaymentCollectionMode.GetPaymentDescription(SubscriptionPaymentCollectionMode.SUBSCRIPTION_CYCLE);
            FULL_PAYMODE_DESC = SubscriptionPaymentCollectionMode.GetPaymentDescription(SubscriptionPaymentCollectionMode.FULL);
            CHOICE_PAYMODE_DESC = SubscriptionPaymentCollectionMode.GetPaymentDescription(SubscriptionPaymentCollectionMode.CUSTOMER_CHOICE);
            CANCEL_UNBILLED_DESC = SubscriptionCancellationOption.GetCancellationOptionDescription(SubscriptionCancellationOption.CANCELL_UNBILLED_CYCLES);
            CANCEL_AUTO_REN_DESC = SubscriptionCancellationOption.GetCancellationOptionDescription(SubscriptionCancellationOption.CANCEL_AUTO_RENEWAL_ONLY);
            BILLING_LINE_DESC = SubscriptionLineType.GetSubscriptionLineTypeDescription(SubscriptionLineType.BILLING_LINE);
            GRACE_LINE_DESC = SubscriptionLineType.GetSubscriptionLineTypeDescription(SubscriptionLineType.GRACE_LINE);
            log.LogMethodExit();
        }
        private void LoadSubsscriptionDTO()
        {
            log.LogMethodEntry();
            SubscriptionHeaderBL subscriptionHeaderBL = new SubscriptionHeaderBL(executionContext, this.subscriptionHeaderId,true);
            subscriptionHeaderDTO = subscriptionHeaderBL.SubscriptionHeaderDTO;
            log.LogMethodExit();
        }

        private void LoadSubscriptionHisotryDTO()
        {
            log.LogMethodEntry();
            SubscriptionHeaderHistoryListBL subscriptionHeaderHistoryListBL = new SubscriptionHeaderHistoryListBL(executionContext);
            List<KeyValuePair<SubscriptionHeaderHistoryDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<SubscriptionHeaderHistoryDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<SubscriptionHeaderHistoryDTO.SearchByParameters, string>(SubscriptionHeaderHistoryDTO.SearchByParameters.SUBSCRIPTION_HEADER_ID, this.subscriptionHeaderId.ToString()));
            searchParameters.Add(new KeyValuePair<SubscriptionHeaderHistoryDTO.SearchByParameters, string>(SubscriptionHeaderHistoryDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            subscriptionHeaderHistoryDTOList = subscriptionHeaderHistoryListBL.GetAllSubscriptionHeaderHistoryDTOList(searchParameters,true);
            if (subscriptionHeaderHistoryDTOList != null && subscriptionHeaderHistoryDTOList.Any())
            {
                subscriptionHeaderHistoryDTOList = subscriptionHeaderHistoryDTOList.OrderByDescending(shh => shh.CreationDate).ToList();
            }
            else
            {
                subscriptionHeaderHistoryDTOList = new List<SubscriptionHeaderHistoryDTO>(); 
            }
            SubscriptionHeaderHistoryDTO currentDTO = new SubscriptionHeaderHistoryDTO(-1, subscriptionHeaderDTO.SubscriptionHeaderId, subscriptionHeaderDTO.TransactionId, subscriptionHeaderDTO.TransactionLineId,
                                                                 subscriptionHeaderDTO.CustomerId, subscriptionHeaderDTO.CustomerContactId, subscriptionHeaderDTO.CustomerCreditCardsId, subscriptionHeaderDTO.ProductSubscriptionId,
                                                                 subscriptionHeaderDTO.ProductSubscriptionName, subscriptionHeaderDTO.ProductSubscriptionDescription, subscriptionHeaderDTO.SubscriptionPrice,
                                                                 subscriptionHeaderDTO.SubscriptionCycle, subscriptionHeaderDTO.UnitOfSubscriptionCycle, subscriptionHeaderDTO.SubscriptionCycleValidity,
                                                                 //subscriptionHeaderDTO.SeasonalSubscription, 
                                                                 subscriptionHeaderDTO.SeasonStartDate, 
                                                                 //subscriptionHeaderDTO.SeasonEndDate, 
                                                                 subscriptionHeaderDTO.FreeTrialPeriodCycle,
                                                                 subscriptionHeaderDTO.AllowPause, subscriptionHeaderDTO.BillInAdvance, subscriptionHeaderDTO.SubscriptionPaymentCollectionMode, subscriptionHeaderDTO.SelectedPaymentCollectionMode,
                                                                 subscriptionHeaderDTO.AutoRenew, subscriptionHeaderDTO.AutoRenewalMarkupPercent, subscriptionHeaderDTO.RenewalGracePeriodCycle, subscriptionHeaderDTO.NoOfRenewalReminders,
                                                                 subscriptionHeaderDTO.ReminderFrequencyInDays, subscriptionHeaderDTO.SendFirstReminderBeforeXDays, subscriptionHeaderDTO.Status, 
                                                                 subscriptionHeaderDTO.TaxInclusivePrice, subscriptionHeaderDTO.ProductsId, subscriptionHeaderDTO.LastRenewalReminderSentOn, subscriptionHeaderDTO.RenewalReminderCount,
                                                                 subscriptionHeaderDTO.LastPaymentRetryLimitReminderSentOn, subscriptionHeaderDTO.PaymentRetryLimitReminderCount, 
                                                                 subscriptionHeaderDTO.SourceSubscriptionHeaderId, subscriptionHeaderDTO.SubscriptionStartDate, subscriptionHeaderDTO.SubscriptionEndDate,
                                                                 subscriptionHeaderDTO.PausedBy, subscriptionHeaderDTO.PauseApprovedBy, subscriptionHeaderDTO.UnPausedBy, subscriptionHeaderDTO.UnPauseApprovedBy,
                                                                 subscriptionHeaderDTO.CancellationOption, subscriptionHeaderDTO.CancelledBy, subscriptionHeaderDTO.CancellationApprovedBy, 
                                                                 subscriptionHeaderDTO.IsActive, 
                                                                 subscriptionHeaderDTO.CreatedBy, subscriptionHeaderDTO.CreationDate, subscriptionHeaderDTO.LastUpdatedBy, subscriptionHeaderDTO.LastUpdateDate, 
                                                                 subscriptionHeaderDTO.SiteId, subscriptionHeaderDTO.MasterEntityId, subscriptionHeaderDTO.SynchStatus, subscriptionHeaderDTO.Guid.ToString(), subscriptionHeaderDTO.SubscriptionNumber
                                                                 );
            subscriptionHeaderHistoryDTOList.Insert(0, currentDTO); 
            log.LogMethodExit();
        }

        private void LoadSubscriptionBillingScheuleHisotryDTO()
        {
            log.LogMethodEntry();
            SubscriptionBillingScheduleHistoryListBL subscriptionBillingScheduleHistoryListBL = new SubscriptionBillingScheduleHistoryListBL(executionContext);
            List<KeyValuePair<SubscriptionBillingScheduleHistoryDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<SubscriptionBillingScheduleHistoryDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<SubscriptionBillingScheduleHistoryDTO.SearchByParameters, string>(SubscriptionBillingScheduleHistoryDTO.SearchByParameters.SUBSCRIPTION_HEADER_ID, this.subscriptionHeaderId.ToString()));
            searchParameters.Add(new KeyValuePair<SubscriptionBillingScheduleHistoryDTO.SearchByParameters, string>(SubscriptionBillingScheduleHistoryDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            subscriptionBillingScheduleHistoryDTOList = subscriptionBillingScheduleHistoryListBL.GetAllSubscriptionBillingScheduleHistoryDTOList(searchParameters);
            if (subscriptionBillingScheduleHistoryDTOList != null && subscriptionBillingScheduleHistoryDTOList.Any())
            {
                subscriptionBillingScheduleHistoryDTOList = subscriptionBillingScheduleHistoryDTOList.OrderByDescending(shh => shh.CreationDate).ToList();
            }
            else
            {
                subscriptionBillingScheduleHistoryDTOList = new List<SubscriptionBillingScheduleHistoryDTO>();
            } 
            log.LogMethodExit();
        }

        private void LoadTransactionIds()
        {
            log.LogMethodEntry(); 
            dgvHeaderTransactionId = SubscriptionUIHelper.LoadTransactionId(utilities, subscriptionHeaderDTO.TransactionId, dgvHeaderTransactionId);
            log.LogMethodExit();
        }
        private void LoadCustomerIds()
        {
            log.LogMethodEntry();
            dgvHeaderCustomerId = SubscriptionUIHelper.LoadCustomerId(executionContext, subscriptionHeaderDTO.CustomerId, dgvHeaderCustomerId);
            log.LogMethodExit();
        }
        private void LoadCreditCards()
        {
            log.LogMethodEntry();
            dgvHeaderCustomerCreditCardsID = SubscriptionUIHelper.LoadCreditCards(utilities, executionContext, subscriptionHeaderDTO.CustomerId, dgvHeaderCustomerCreditCardsID);
            log.LogMethodExit();
        }
        private void LoadCustomerContacts()
        {
            log.LogMethodEntry();
            dgvHeaderCustomerContactId = SubscriptionUIHelper.LoadCustomerContacts(executionContext, subscriptionHeaderDTO.CustomerId, dgvHeaderCustomerContactId);
            log.LogMethodExit();
        }

        private void LoadLinkedTransactionIds()
        {
            log.LogMethodEntry();
            dgvBillingCycleTransactionId = SubscriptionUIHelper.LoadLinkedTransactionId(utilities, subscriptionHeaderDTO.SubscriptionHeaderId, dgvBillingCycleTransactionId);
            log.LogMethodExit();
        }
        private void LoadCmbBillingCyclesList()
        {
            log.LogMethodEntry();
            try
            {
                fireCmbEvent = false;
                List<SubscriptionBillingScheduleDTO> subscriptionBillingScheduleDTOList = new List<SubscriptionBillingScheduleDTO>(this.subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList);
                subscriptionBillingScheduleDTOList = subscriptionBillingScheduleDTOList.OrderBy(sbs => sbs.BillFromDate).ToList();
                cmbBillingCycles.DataSource = subscriptionBillingScheduleDTOList;
                cmbBillingCycles.ValueMember = "SubscriptionBillingScheduleId";
                cmbBillingCycles.DisplayMember = "BillFromDate";
                cmbBillingCycles.SelectedIndex = 0;
            }
            finally
            {
                fireCmbEvent = true;
            }
            log.LogMethodExit();
        }

        private void SetDGVFormat()
        {
            log.LogMethodEntry();
            utilities.setupDataGridProperties(ref dgvSubscriptionHeaderHistory);
            utilities.setupDataGridProperties(ref dgvBillCycleHistory);
            subscriptionPriceDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewAmountCellStyle();
            SeasonStartDate.DefaultCellStyle = utilities.gridViewDateCellStyle();
            SubscriptionStartDate.DefaultCellStyle = utilities.gridViewDateCellStyle();
            SubscriptionEndDate.DefaultCellStyle = utilities.gridViewDateCellStyle();
            lastRenewalReminderSentOnDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewDateTimeCellStyle();
            lastPaymentRetryLimitReminderSentOnDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewDateTimeCellStyle();
            paymentRetryLimitReminderCountDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewNumericCellStyle();
            FreeTrialPeriodCycle.DefaultCellStyle = utilities.gridViewNumericCellStyle();
            RenewalGracePeriodCycle.DefaultCellStyle = utilities.gridViewNumericCellStyle();
            creationDateDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewDateTimeCellStyle();
            lastUpdateDateDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewDateTimeCellStyle();

            billFromDateDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewDateCellStyle();
            billToDateDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewDateCellStyle();
            billOnDateDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewDateCellStyle();
            creationDateDataGridViewTextBoxColumn1.DefaultCellStyle = utilities.gridViewDateTimeCellStyle();
            lastUpdateDateDataGridViewTextBoxColumn1.DefaultCellStyle = utilities.gridViewDateTimeCellStyle();
            dgvSubscriptionHeaderHistory = SubscriptionUIHelper.SetDGVCellFont(executionContext, dgvSubscriptionHeaderHistory);
            dgvBillCycleHistory = SubscriptionUIHelper.SetDGVCellFont(executionContext, dgvBillCycleHistory);
            log.LogMethodExit();
        }
        private void LoadHeaderHistoryDGV()
        {
            log.LogMethodEntry(); 
            dgvSubscriptionHeaderHistory.DataSource = subscriptionHeaderHistoryDTOList;
            if (dgvSubscriptionHeaderHistory != null && dgvSubscriptionHeaderHistory.Rows.Count > 0)
            {
                dgvSubscriptionHeaderHistory.Rows[0].DefaultCellStyle.BackColor = Color.LawnGreen;
            }
            log.LogMethodExit();
        }

        private void LoadBillingCycleHistoryDGV(int billingScheduleId)
        {
            log.LogMethodEntry();
            if (billingScheduleId > -1)
            {
                List<SubscriptionBillingScheduleHistoryDTO> lineHistoryDTOList = GetBillingScheduleIdHistory(billingScheduleId);
                dgvBillCycleHistory.DataSource = lineHistoryDTOList;
                dgvBillCycleHistory.Rows[0].DefaultCellStyle.BackColor = Color.LawnGreen;
            }            
            log.LogMethodExit();
        }

        private List<SubscriptionBillingScheduleHistoryDTO> GetBillingScheduleIdHistory(int billingScheduleId)
        {
            log.LogMethodEntry();
            List<SubscriptionBillingScheduleHistoryDTO> lineHistoryDTOList = subscriptionBillingScheduleHistoryDTOList.Where(sbs => sbs.SubscriptionBillingScheduleId == billingScheduleId).ToList();
            if (lineHistoryDTOList == null)
            {
                lineHistoryDTOList = new List<SubscriptionBillingScheduleHistoryDTO>();
            }
            SubscriptionBillingScheduleDTO selectedDTO = this.subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList.Find(sbs => sbs.SubscriptionBillingScheduleId == billingScheduleId);
            SubscriptionBillingScheduleHistoryDTO currentLineDTO = new SubscriptionBillingScheduleHistoryDTO(-1, selectedDTO.SubscriptionBillingScheduleId,
                                                    selectedDTO.SubscriptionHeaderId, selectedDTO.TransactionId, selectedDTO.TransactionLineId, selectedDTO.BillFromDate,
                                                    selectedDTO.BillToDate, selectedDTO.BillOnDate, selectedDTO.BillAmount, selectedDTO.OverridedBillAmount, selectedDTO.OverrideReason,
                                                    selectedDTO.OverrideBy, selectedDTO.OverrideApprovedBy, selectedDTO.PaymentProcessingFailureCount, selectedDTO.Status,
                                                    selectedDTO.CancelledBy, selectedDTO.CancellationApprovedBy, selectedDTO.LineType, selectedDTO.IsActive,
                                                    selectedDTO.CreatedBy, selectedDTO.CreationDate, selectedDTO.LastUpdatedBy, selectedDTO.LastUpdateDate, selectedDTO.SiteId,
                                                    selectedDTO.MasterEntityId, selectedDTO.SynchStatus, selectedDTO.Guid);
            lineHistoryDTOList.Insert(0, currentLineDTO);
            log.LogMethodExit(lineHistoryDTOList);
            return lineHistoryDTOList;
        }

        private void cmbBillingCycles_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (fireCmbEvent && cmbBillingCycles.SelectedValue != null)
                {
                    LoadBillingCycleHistoryDGV(Convert.ToInt32(cmbBillingCycles.SelectedValue));
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

        private void frmSubscriptionHistoryView_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            LoadHeaderHistoryDGV();
            if (this.subscriptionHeaderDTO != null && this.subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList != null
                && this.subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList.Any())
            {
                List<SubscriptionBillingScheduleDTO> subscriptionBillingScheduleDTOList = new List<SubscriptionBillingScheduleDTO>(this.subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList);
                subscriptionBillingScheduleDTOList = subscriptionBillingScheduleDTOList.OrderBy(sbs => sbs.BillFromDate).ToList();
                LoadBillingCycleHistoryDGV(subscriptionBillingScheduleDTOList[0].SubscriptionBillingScheduleId);
            }
            log.LogMethodExit();
        }

        private void dgvBillCycleHistory_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        private void dgvSubscriptionHeaderHistory_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (e.ColumnIndex > -1 && e.RowIndex > -1)
                {
                    if (dgvSubscriptionHeaderHistory.Columns[e.ColumnIndex].Name == "transactionLineIdDataGridViewTextBoxColumn"
                         || dgvSubscriptionHeaderHistory.Columns[e.ColumnIndex].Name == "SourceSubscriptionHeaderId")
                    {
                        SetEmptyString(e);
                    }
                    else if (dgvSubscriptionHeaderHistory.Columns[e.ColumnIndex].Name == "unitOfSubscriptionCycleDataGridViewTextBoxColumn")
                    {
                        SetUOMDescription(e);
                    }
                    else if (dgvSubscriptionHeaderHistory.Columns[e.ColumnIndex].Name == "selectedPaymentCollectionModeDataGridViewTextBoxColumn"
                             || dgvSubscriptionHeaderHistory.Columns[e.ColumnIndex].Name == "subscriptionPaymentCollectionModeDataGridViewTextBoxColumn")
                    {
                        SetPaymodeDescription(e);
                    }
                    else if (dgvSubscriptionHeaderHistory.Columns[e.ColumnIndex].Name == "CancellationOption" )
                    {
                        SetCancellationOptionDescription(e);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }        
        private static void SetEmptyString(DataGridViewCellFormattingEventArgs e)
        {
            log.LogMethodExit(e);
            if (e.Value != null && e.Value.ToString() == "-1")
            {
                e.Value = string.Empty;
            }
            log.LogMethodExit();
        }
        private void SetUOMDescription(DataGridViewCellFormattingEventArgs e)
        {
            log.LogMethodExit(e);
            if (e.Value != null)
            {
                if (e.Value.ToString() == UnitOfSubscriptionCycle.DAYS)
                {
                    e.Value = UOMDAYS_DESC;
                }
                else if (e.Value.ToString() == UnitOfSubscriptionCycle.MONTHS)
                {
                    e.Value = UOMMONTH_DESC;
                }
                else if (e.Value.ToString() == UnitOfSubscriptionCycle.YEARS)
                {
                    e.Value = UOMYEAR_DESC;
                }
            }
            log.LogMethodExit();
        }

        private void SetPaymodeDescription(DataGridViewCellFormattingEventArgs e)
        {
            log.LogMethodExit(e);
            if (e.Value != null)
            {
                if (e.Value.ToString() == SubscriptionPaymentCollectionMode.SUBSCRIPTION_CYCLE)
                {
                    e.Value = CYCLE_PAYMODE_DESC;
                }
                else if (e.Value.ToString() == SubscriptionPaymentCollectionMode.FULL)
                {
                    e.Value = FULL_PAYMODE_DESC;
                }
                else if (e.Value.ToString() == SubscriptionPaymentCollectionMode.CUSTOMER_CHOICE)
                {
                    e.Value = CHOICE_PAYMODE_DESC;
                }
            }
            log.LogMethodExit();
        }
        private void SetCancellationOptionDescription(DataGridViewCellFormattingEventArgs e)
        {
            log.LogMethodExit(e);
            if (e.Value != null)
            {
                if (e.Value.ToString() == SubscriptionCancellationOption.CANCELL_UNBILLED_CYCLES)
                {
                    e.Value = CANCEL_UNBILLED_DESC;
                }
                else if (e.Value.ToString() == SubscriptionCancellationOption.CANCEL_AUTO_RENEWAL_ONLY)
                {
                    e.Value = CANCEL_AUTO_REN_DESC;
                } 
            }
            log.LogMethodExit();
        }
        private void dgvBillCycleHistory_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            log.LogMethodExit(e);
            try
            {
                if (e.ColumnIndex > -1 && e.RowIndex > -1)
                {
                    if (dgvBillCycleHistory.Columns[e.ColumnIndex].Name == "transactionLineIdDataGridViewTextBoxColumn1")
                    {
                        SetEmptyString(e);
                    }
                    else if (dgvBillCycleHistory.Columns[e.ColumnIndex].Name == "LineType")
                    {
                        SetLineTypeDescription(e);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        private void SetLineTypeDescription(DataGridViewCellFormattingEventArgs e)
        {
            log.LogMethodExit(e);
            if (e.Value != null)
            {
                if (e.Value.ToString() == SubscriptionLineType.BILLING_LINE)
                {
                    e.Value = BILLING_LINE_DESC;
                }
                else if (e.Value.ToString() == SubscriptionLineType.GRACE_LINE)
                {
                    e.Value = GRACE_LINE_DESC;
                }
            }
            log.LogMethodExit();
        }
        private void dgvSubscriptionHeaderHistory_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }
    }
}
