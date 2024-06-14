/********************************************************************************************
 * Project Name - frmSubscriptionDetails 
 * Description  - form class to show Subscription Details
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************   
 *2.110.0     21-Dec-2020    Guru S A             Created for Subscription changes                                                                               
 *2.120.0     21-Feb-2021    Guru S A             For Subscription phase 2 changes                                                                               
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Product;
using Semnox.Parafait.Transaction;

namespace Parafait_POS.Subscription
{
    /// <summary>
    /// frmSubscriptionDetails
    /// </summary>
    public partial class frmSubscriptionDetails : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private Utilities utilities;
        private SubscriptionHeaderDTO subscriptionHeaderDTO;
        private SubscriptionHeaderBL subscriptionHeaderBL;
        private LookupValuesList serverDateTime;
        private bool fromCustomerUI = false;
        private int subscriptionHeaderId;
        private string NUMBER_FORMAT;
        private string AMOUNT_FORMAT;
        private string DATETIME_FORMAT;
        private string DATE_FORMAT;
        private const string COLLAPSEPANEL = "C";
        private const string EXPANDPANEL = "E";
        private const int BUTTON_PADDING_SPACE = 5;
        private const int BUTTON_SIZE = 30;
        private const string MANAGER_APPROVAL_FOR_PAUSE_UNPAUSE_SUBSCRIPTION = "MANAGER_APPROVAL_FOR_PAUSE_UNPAUSE_SUBSCRIPTION";
        private const string CANCEL_SUBSCRIPTION_REQUIRES_MANAGER_APPROVAL = "CANCEL_SUBSCRIPTION_REQUIRES_MANAGER_APPROVAL";
        private string BILLING_LINE_DESC = "";
        private string GRACE_LINE_DESC = "";

        /// <summary>
        /// frmSubscriptionDetails
        /// </summary>
        /// <param name="utilities"></param>
        /// <param name="fromCustomerUI"></param>
        /// <param name="subscriptionHeaderId"></param>
        public frmSubscriptionDetails(Utilities utilities, bool fromCustomerUI, int subscriptionHeaderId)
        {
            log.LogMethodEntry(fromCustomerUI, subscriptionHeaderId);
            this.utilities = utilities;
            this.executionContext = utilities.ExecutionContext;
            this.subscriptionHeaderId = subscriptionHeaderId;
            this.fromCustomerUI = fromCustomerUI;
            LoadSubscriptionDTO();
            SetCodeDescriptions();
            serverDateTime = new LookupValuesList(executionContext);
            InitializeComponent();
            System.Drawing.Rectangle workingRectangle = Screen.PrimaryScreen.WorkingArea;
            log.LogVariableState("workingRectangle.Width", workingRectangle.Width);
            log.LogVariableState("this.Width", this.Width);
            log.LogVariableState("pnlPage.Size- Before", this.pnlPage.Size);
            if (this.Width > workingRectangle.Width - 100)
            {
                this.Size = new Size(workingRectangle.Width - 20, this.Height);
                this.pnlPage.Size = new System.Drawing.Size(this.Width - 10, pnlPage.Height);
                this.pnlPage.AutoScroll = true;
            }
            log.LogVariableState("pnlPage.Size- After", this.pnlPage.Size);
            fpnlHeaderActions.Visible = false;
            fpnlBillingCycleActions.Visible = false;
            AdjustHeaderActionSize();
            SetFormats();
            HideShowBillingCycleActionsOption();
            AdjustBillingActionSize();
            SetbtnPauseSubscriptionText();
            LoadSelectedPaymentCollectionMode();
            LoadSubscriptionStatus();
            LoadCreditCards();
            LoadCustomerContacts();
            LoadCancellationOptions();
            SetHeaderSection();
            SetSubscriptionBillCycles();
            CollapseExpandPanels(this.btnExpandCollapse1);
            this.utilities.setLanguage(this);
            log.LogMethodExit();
        }
        private void AdjustHeaderActionSize()
        {
            log.LogMethodEntry();
            fpnlHeaderActions.SuspendLayout();
            this.fpnlHeaderActions.Controls.Remove(btnReactivateSubscription);
            btnReactivateSubscription.Visible = false;
            fpnlHeaderActions.ResumeLayout(true);
            fpnlHeaderActions.Refresh();
            //int heightAdjuster = (btnReactivateSubscription.Visible ? 0 : btnReactivateSubscription.Height + BUTTON_PADDING_SPACE);
            //log.Info(heightAdjuster);
            //fpnlHeaderActions.Size = new Size(fpnlHeaderActions.Width, fpnlHeaderActions.Height - heightAdjuster);
            log.LogMethodExit();
        }
        private void LoadSubscriptionDTO()
        {
            log.LogMethodEntry();
            subscriptionHeaderBL = new SubscriptionHeaderBL(executionContext, subscriptionHeaderId, true);
            subscriptionHeaderDTO = subscriptionHeaderBL.SubscriptionHeaderDTO;
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
        private void HideShowBillingCycleActionsOption()
        {
            log.LogMethodEntry();
            //this.fpnlBillingCycleActions.Controls.Remove(btnResetPaymentErrorCount);
            //btnResetPaymentErrorCount.Visible = false;
            btnOverridePrice.Visible = true;
            if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "ENABLE_OVERRIDE_SUBSCRIPTION_PRICE_IN_UI") == false)
            {
                this.fpnlBillingCycleActions.Controls.Remove(btnOverridePrice);
                btnOverridePrice.Visible = false;
            }
            log.LogMethodExit();
        }
        private void AdjustBillingActionSize()
        {
            log.LogMethodEntry();
            fpnlBillingCycleActions.SuspendLayout();
            fpnlBillingCycleActions.ResumeLayout(true);
            fpnlBillingCycleActions.Refresh();
            log.LogMethodExit();
        }
        private void SetbtnPauseSubscriptionText()
        {
            log.LogMethodEntry();
            btnPauseSubscription.Text = subscriptionHeaderDTO.Status == SubscriptionStatus.PAUSED ? MessageContainerList.GetMessage(executionContext, "Unpause Subscription") : MessageContainerList.GetMessage(executionContext, "Pause Subscription");
            log.LogMethodExit();
        }
        private void LoadSelectedPaymentCollectionMode()
        {
            log.LogMethodEntry();
            cmbSelectedPaymentCollectionMode = SubscriptionUIHelper.LoadSelectedPaymentCollectionMode(executionContext, cmbSelectedPaymentCollectionMode);
            log.LogMethodExit();
        }
        private void LoadSubscriptionStatus()
        {
            log.LogMethodEntry();
            cmbSubscriptionStatus = SubscriptionUIHelper.LoadSubscriptionStatus(executionContext, cmbSubscriptionStatus);
            log.LogMethodExit();
        }
        private void LoadCreditCards()
        {
            log.LogMethodEntry();
            cmbCreditCardId = SubscriptionUIHelper.LoadCreditCards(utilities, executionContext, subscriptionHeaderDTO.CustomerId, cmbCreditCardId);
            log.LogMethodExit();
        }
        private void LoadCustomerContacts()
        {
            log.LogMethodEntry();
            cmbCustomerContact = SubscriptionUIHelper.LoadCustomerContacts(executionContext, subscriptionHeaderDTO.CustomerId, cmbCustomerContact);
            log.LogMethodExit();
        }
        private void LoadCancellationOptions()
        {
            log.LogMethodEntry();
            cmbCancellationOption = SubscriptionUIHelper.LoadCancellationOptions(executionContext, cmbCancellationOption);
            log.LogMethodExit();
        }
        private void SetHeaderSection()
        {
            log.LogMethodEntry();
            lblHeaderName.Text = subscriptionHeaderDTO.ProductSubscriptionName;
            txtSubscriptionNo.Text = subscriptionHeaderDTO.SubscriptionNumber;
            txtSubscriptionStartDate.Text = subscriptionHeaderDTO.SubscriptionStartDate.ToString(DATE_FORMAT);
            txtSubscriptionEndDate.Text = subscriptionHeaderDTO.SubscriptionEndDate.ToString(DATE_FORMAT);
            cbxAllowPause.Checked = subscriptionHeaderDTO.AllowPause;
            cbxAutoRenew.Checked = subscriptionHeaderDTO.AutoRenew;
            cbxBillInAdvance.Checked = subscriptionHeaderDTO.BillInAdvance;
            // cbxSeasonalSubscription.Checked = subscriptionHeaderDTO.SeasonalSubscription;

            txtAutoRenewalMarkup.Text = subscriptionHeaderDTO.AutoRenewalMarkupPercent.ToString(NUMBER_FORMAT);
            txtFirstReminderBeforeXDays.Text = (subscriptionHeaderDTO.SendFirstReminderBeforeXDays == null ? "0"
                                                          : ((int)subscriptionHeaderDTO.SendFirstReminderBeforeXDays).ToString(NUMBER_FORMAT));
            txtFreeTrialPeriod.Text = (subscriptionHeaderDTO.FreeTrialPeriodCycle == null ? "0"
                                                          : ((int)subscriptionHeaderDTO.FreeTrialPeriodCycle).ToString(NUMBER_FORMAT));
            txtLastReminderSentOn.Text = (subscriptionHeaderDTO.LastRenewalReminderSentOn == null ? string.Empty
                                                          : ((DateTime)subscriptionHeaderDTO.LastRenewalReminderSentOn).ToString(DATETIME_FORMAT));
            txtNextReminderOn.Text = GetNextReminderOn();
            txtRenewalGracePeriod.Text = (subscriptionHeaderDTO.RenewalGracePeriodCycle == null ? "0"
                                                          : ((int)subscriptionHeaderDTO.RenewalGracePeriodCycle).ToString(NUMBER_FORMAT));
            txtRenewalReminderFrequency.Text = (subscriptionHeaderDTO.ReminderFrequencyInDays == null ? "0"
                                                          : ((int)subscriptionHeaderDTO.ReminderFrequencyInDays).ToString(NUMBER_FORMAT));
            txtRenewalReminderSent.Text = (subscriptionHeaderDTO.RenewalReminderCount == null ? "0"
                                                          : ((int)subscriptionHeaderDTO.RenewalReminderCount).ToString(NUMBER_FORMAT));
            //txtSeasonEndDate.Text = (subscriptionHeaderDTO.SeasonEndDate == null ? string.Empty
            //                                              : ((DateTime)subscriptionHeaderDTO.SeasonEndDate).ToString(DATE_FORMAT));
            txtSeasonStartDate.Text = (subscriptionHeaderDTO.SeasonStartDate == null ? string.Empty
                                                          : ((DateTime)subscriptionHeaderDTO.SeasonStartDate).ToString(DATE_FORMAT));
            txtSubscriptionCycle.Text = subscriptionHeaderDTO.SubscriptionCycle.ToString(NUMBER_FORMAT);
            txtSubscriptionCycleValidity.Text = subscriptionHeaderDTO.SubscriptionCycleValidity.ToString(NUMBER_FORMAT);
            txtSubscriptionDescription.Text = subscriptionHeaderDTO.ProductSubscriptionDescription;
            txtSubscriptionPrice.Text = subscriptionHeaderDTO.SubscriptionPrice.ToString(AMOUNT_FORMAT);
            txtUnitOfSubscriptionCycle.Text = UnitOfSubscriptionCycle.GetUnitOfSubscriptionCycleDescription(executionContext, subscriptionHeaderDTO.UnitOfSubscriptionCycle);

            cmbCreditCardId.SelectedValue = subscriptionHeaderDTO.CustomerCreditCardsId;
            cmbCustomerContact.SelectedValue = subscriptionHeaderDTO.CustomerContactId;
            cmbSelectedPaymentCollectionMode.SelectedValue = subscriptionHeaderDTO.SelectedPaymentCollectionMode;
            cmbSubscriptionStatus.SelectedValue = subscriptionHeaderDTO.Status;
            cmbCancellationOption.SelectedValue = subscriptionHeaderDTO.CancellationOption;

            txtPauseApprovedBy.Text = subscriptionHeaderDTO.PauseApprovedBy;
            txtPausedBy.Text = subscriptionHeaderDTO.PausedBy;
            txtCancelledBy.Text = subscriptionHeaderDTO.CancelledBy;
            txtCancellationApprovedBy.Text = subscriptionHeaderDTO.CancellationApprovedBy;
            txtUnpauseApprovedBy.Text = subscriptionHeaderDTO.UnPauseApprovedBy;
            txtUnpausedBy.Text = subscriptionHeaderDTO.UnPausedBy;
            txtSourceSubscriptionHeaderId.Text = (subscriptionHeaderDTO.SourceSubscriptionHeaderId == -1 ? string.Empty : subscriptionHeaderDTO.SourceSubscriptionHeaderId.ToString(NUMBER_FORMAT));
            log.LogMethodExit();
        }
        private string GetNextReminderOn()
        {
            log.LogMethodEntry();
            string nextReminderOn = string.Empty;
            if (subscriptionHeaderDTO.LastRenewalReminderSentOn != null)
            {
                int frequency = (subscriptionHeaderDTO.ReminderFrequencyInDays == null ? 0 : (int)subscriptionHeaderDTO.ReminderFrequencyInDays);
                nextReminderOn = ((DateTime)subscriptionHeaderDTO.LastRenewalReminderSentOn).AddDays(frequency).ToString(DATE_FORMAT);
            }
            else
            {
                SubscriptionBillingScheduleDTO lastBillCycleDTO = subscriptionHeaderBL.GetLastBillingScheduleDTO();
                if (lastBillCycleDTO != null)
                {
                    if (serverDateTime.GetServerDateTime() > lastBillCycleDTO.BillFromDate)
                    {
                        int xDays = (subscriptionHeaderDTO.SendFirstReminderBeforeXDays == null ? 1 : (int)subscriptionHeaderDTO.SendFirstReminderBeforeXDays);
                        nextReminderOn = lastBillCycleDTO.BillToDate.AddDays(xDays * -1).ToString(DATE_FORMAT);
                    }
                }
            }
            log.LogMethodExit(nextReminderOn);
            return nextReminderOn;
        }
        private void SetSubscriptionBillCycles()
        {
            log.LogMethodEntry();
            SetDGVFormat();
            dgvColumnTransactionId = SubscriptionUIHelper.LoadLinkedTransactionId(utilities, this.subscriptionHeaderDTO.SubscriptionHeaderId, dgvColumnTransactionId);
            LoadDGVSubscriptionBillingSchedule();
            log.LogMethodExit();
        }
        private void SetDGVFormat()
        {
            log.LogMethodEntry();
            utilities.setupDataGridProperties(ref dgvSubscriptionBillingSchedule);
            dgvSubscriptionBillingSchedule.RowHeadersVisible = false;
            billFromDateDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewDateCellStyle();
            billOnDateDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewDateCellStyle();
            billToDateDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewDateCellStyle();
            dgvSubscriptionBillingSchedule = SubscriptionUIHelper.SetDGVCellFont(executionContext, dgvSubscriptionBillingSchedule);
            log.LogMethodExit();
        }
        private void LoadDGVSubscriptionBillingSchedule()
        {
            log.LogMethodEntry();
            dgvSubscriptionBillingSchedule.DataSource = this.subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList;
            log.LogMethodExit();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                this.Cursor = Cursors.WaitCursor;
                UpdateSubscriptionHeaderDTOWithUIData();
                if (subscriptionHeaderDTO.IsChanged)
                {
                    try
                    {
                        subscriptionHeaderBL = new SubscriptionHeaderBL(executionContext, this.subscriptionHeaderDTO);
                        subscriptionHeaderBL.Save();
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 1824, ex.Message));
                    }
                    RefreshSubscription();
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
        private void UpdateSubscriptionHeaderDTOWithUIData()
        {
            log.LogMethodEntry();
            int customerContactValue = -1;
            Int32.TryParse(cmbCustomerContact.SelectedValue != null ? cmbCustomerContact.SelectedValue.ToString() : "-1", out customerContactValue);
            if (customerContactValue > -1 && customerContactValue != this.subscriptionHeaderDTO.CustomerContactId)
            {
                this.subscriptionHeaderDTO.CustomerContactId = customerContactValue;
            }

            int creditCardIdValue = -1;
            Int32.TryParse(cmbCreditCardId.SelectedValue != null ? cmbCreditCardId.SelectedValue.ToString() : "-1", out creditCardIdValue);
            if (creditCardIdValue > -1 && creditCardIdValue != this.subscriptionHeaderDTO.CustomerCreditCardsId)
            {
                this.subscriptionHeaderDTO.CustomerCreditCardsId = creditCardIdValue;
            }
            if (cbxAutoRenew.Checked != this.subscriptionHeaderDTO.AutoRenew)
            {
                this.subscriptionHeaderDTO.AutoRenew = cbxAutoRenew.Checked;
            }
            decimal autoRenewalMarkupValue = 0;
            decimal.TryParse(txtAutoRenewalMarkup.Text, out autoRenewalMarkupValue);
            if (autoRenewalMarkupValue != this.subscriptionHeaderDTO.AutoRenewalMarkupPercent)
            {
                this.subscriptionHeaderDTO.AutoRenewalMarkupPercent = autoRenewalMarkupValue;
            }
            log.LogMethodExit();
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (this.subscriptionHeaderDTO.IsChanged)
                {   //There are changes that have not been saved, do you want to discard these changes?
                    if (POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 2740),
                                                    MessageContainerList.GetMessage(executionContext, "Save"), MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        log.LogMethodExit();
                        return;
                    }
                }
                RefreshSubscription();
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
        private void RefreshSubscription()
        {
            log.LogMethodEntry();
            RefresherHeaderSection();
            SetSubscriptionBillCycles();
            log.LogMethodExit();
        }
        private void RefresherHeaderSection()
        {
            log.LogMethodEntry();
            LoadSubscriptionDTO();
            SetHeaderSection();
            log.LogMethodExit();
        }
        private void btnDetails_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                SetbtnPauseSubscriptionText();
                fpnlHeaderActions.Location = new Point(btnDetails.Location.X - fpnlHeaderActions.Width / 2 - btnDetails.Width / 2, 5);
                fpnlHeaderActions.Show();
                fpnlHeaderActions.Focus();
                fpnlHeaderActions.BringToFront();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 1824, ex.Message));
            }
            log.LogMethodExit();
        }
        private void pnlHeaderActions_Leave(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                HideHeaderActionPanel();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 1824, ex.Message));
            }
            log.LogMethodExit();
        }
        private void HideHeaderActionPanel()
        {
            log.LogMethodEntry();
            if (fpnlHeaderActions != null)
            {
                fpnlHeaderActions.Visible = false;
            }
            log.LogMethodExit();
        }
        private void btnCancelSubscription_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (subscriptionHeaderBL != null && subscriptionHeaderBL.SubscriptionHeaderDTO != null &&
                    subscriptionHeaderBL.SubscriptionHeaderDTO.SubscriptionHeaderId > -1
                    && (subscriptionHeaderBL.SubscriptionHeaderDTO.IsActive == false || subscriptionHeaderBL.SubscriptionHeaderDTO.Status != SubscriptionStatus.ACTIVE))
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2877));// "Sorry, only active subscription can be cancelled" 
                }
                if (subscriptionHeaderBL != null && subscriptionHeaderBL.SubscriptionHeaderDTO != null && subscriptionHeaderBL.SubscriptionHeaderDTO.CancellationOption == SubscriptionCancellationOption.CANCELL_UNBILLED_CYCLES)
                {
                    if (POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 2987), MessageContainerList.GetMessage(executionContext, "Cancellation"), MessageBoxButtons.YesNo) == DialogResult.No)
                    { // "Do you want to cancel unbilled cycles?"
                        log.LogMethodExit("User opted No");
                        return;
                    }
                }

                if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, CANCEL_SUBSCRIPTION_REQUIRES_MANAGER_APPROVAL))
                {
                    subscriptionHeaderBL.SubscriptionHeaderDTO.CancellationApprovedBy = null; //reset any old approval info
                    string mgrLoginId = SubscriptionUIHelper.GetManagerApproval(executionContext);
                    subscriptionHeaderBL.SubscriptionHeaderDTO.CancellationApprovedBy = mgrLoginId;
                }
                subscriptionHeaderBL.CancelSubscription(utilities);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 1824, ex.Message));
            }
            finally
            {
                this.Cursor = Cursors.Default;
                CallRefreshAfterTheTask();
            }
            log.LogMethodExit();
        }
        private void btnSendManualRenewalReminder_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                this.Cursor = Cursors.WaitCursor;
                subscriptionHeaderBL.SendManualRenewalReminder(utilities);
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 2055));//Action completed successfully.
                RefresherHeaderSection();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 1824, ex.Message));
            }
            finally
            {
                this.Cursor = Cursors.Default;
                HideBillingActionPanel();
                HideHeaderActionPanel();
            }
            log.LogMethodExit();
        }
        private void btnViewHistory_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                this.Cursor = Cursors.WaitCursor;
                using (frmSubscriptionHistoryView frm = new frmSubscriptionHistoryView(utilities, this.subscriptionHeaderId))
                {
                    frm.ShowDialog();
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
                HideBillingActionPanel();
                HideHeaderActionPanel();
            }
            log.LogMethodExit();
        }

        private void btnPauseSubscription_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (this.subscriptionHeaderDTO != null && subscriptionHeaderDTO.AllowPause
                    && (subscriptionHeaderDTO.Status == SubscriptionStatus.PAUSED || subscriptionHeaderDTO.Status == SubscriptionStatus.ACTIVE))
                {
                    if (subscriptionHeaderDTO.Status == SubscriptionStatus.PAUSED)
                    {
                        UnPauseSubscription();
                    }
                    else
                    {
                        PauseSubscription();
                    }
                }
                else
                {
                    POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 2964));
                    // "Sorry, Pause operation is not allowed for this subscription"
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
                CallRefreshAfterTheTask();
            }
            log.LogMethodExit();
        }
        private void CallRefreshAfterTheTask()
        {
            log.LogMethodEntry();
            try
            {
                HideHeaderActionPanel();
                HideBillingActionPanel();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            try
            {
                this.Cursor = Cursors.WaitCursor;
                RefreshSubscription();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 1824, ex.Message));
            }
            this.Cursor = Cursors.Default;
            log.LogMethodExit();
        }
        private void PauseSubscription()
        {
            log.LogMethodEntry();
            this.Cursor = Cursors.WaitCursor;
            if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, MANAGER_APPROVAL_FOR_PAUSE_UNPAUSE_SUBSCRIPTION))
            {
                subscriptionHeaderDTO.PauseApprovedBy = null; //reset any old approval info
                string mgrLoginId = SubscriptionUIHelper.GetManagerApproval(executionContext);
                subscriptionHeaderDTO.PauseApprovedBy = mgrLoginId;
            }
            this.Cursor = Cursors.WaitCursor;
            SubscriptionHeaderBL subscriptionHeaderBL = new SubscriptionHeaderBL(executionContext, subscriptionHeaderDTO);
            subscriptionHeaderBL.PauseSubscription(utilities);
            log.LogMethodExit();
        }
        private void UnPauseSubscription()
        {
            log.LogMethodEntry();
            using (frmUnPauseSubscription unPauseSubscriptionForm = new frmUnPauseSubscription(utilities, subscriptionHeaderId))
            {
                unPauseSubscriptionForm.ShowDialog();
            }
            log.LogMethodExit();
        }
        private void btnReactivateSubscription_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                this.Cursor = Cursors.WaitCursor;

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
        private void fpnlBillingCycleActions_Leave(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                HideBillingActionPanel();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 1824, ex.Message));
            }
            log.LogMethodExit();
        }
        private void HideBillingActionPanel()
        {
            log.LogMethodEntry();
            if (fpnlBillingCycleActions != null)
            {
                fpnlBillingCycleActions.Tag = null;
                fpnlBillingCycleActions.Visible = false;
            }
            log.LogMethodExit();
        }
        private void dgvSubscriptionBillingSchedule_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (e.RowIndex > -1 && e.ColumnIndex > -1)
                {

                    if (dgvSubscriptionBillingSchedule.Columns[e.ColumnIndex].Name == "dgvColumnbillCycleActions")
                    {
                        // dgvSubscriptionBillingSchedule.Rows[e.RowIndex].Cells["dgvColumnbillCycleActions"] 
                        fpnlBillingCycleActions.Location = new Point(dgvSubscriptionBillingSchedule.Location.X + btnDetails.Width, dgvSubscriptionBillingSchedule.Location.Y + e.RowIndex * 10);
                        fpnlBillingCycleActions.Tag = dgvSubscriptionBillingSchedule.Rows[e.RowIndex];
                        fpnlBillingCycleActions.Show();
                        fpnlBillingCycleActions.Focus();
                        fpnlBillingCycleActions.BringToFront();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 1824, ex.Message));
            }
            log.LogMethodExit();
        }
        private void btnOverridePrice_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (fpnlBillingCycleActions.Tag != null)
                {
                    DataGridViewRow dataGridViewRow = (DataGridViewRow)fpnlBillingCycleActions.Tag;

                    if (dataGridViewRow != null && dataGridViewRow.Cells["subscriptionBillingScheduleId"] != null)
                    {
                        object billingScheduleIdObj = dataGridViewRow.Cells["subscriptionBillingScheduleId"].Value;
                        if (billingScheduleIdObj != null && billingScheduleIdObj != DBNull.Value)
                        {
                            int billingScheduleId = Convert.ToInt32(billingScheduleIdObj);
                            if (billingScheduleId > -1)
                            {
                                object trxIdObj = dataGridViewRow.Cells["dgvColumnTransactionId"].Value;
                                if (trxIdObj != null && trxIdObj != DBNull.Value)
                                {
                                    int trxId = Convert.ToInt32(trxIdObj);
                                    if (trxId > -1)
                                    {
                                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2981));
                                        //"Sorry cannot override price for already billed schedule"
                                    }
                                }
                                SubscriptionBillingScheduleDTO selectedSchedule = subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList.Find(sbs => sbs.SubscriptionBillingScheduleId == billingScheduleId);
                                if (selectedSchedule != null)
                                {
                                    if (selectedSchedule.Status == SubscriptionStatus.CANCELLED || selectedSchedule.Status == SubscriptionStatus.EXPIRED)
                                    {
                                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2975));
                                        //"Subscription billing schedule is in Cancelled/Inactive status, cannot override bill amount"
                                    }
                                    using (frmOverridePrice overridePriceForm = new frmOverridePrice(utilities, selectedSchedule))
                                    {
                                        overridePriceForm.ShowDialog();
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
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 1824, ex.Message));
            }
            finally
            {
                this.Cursor = Cursors.Default;
                CallRefreshAfterTheTask();
            }
            log.LogMethodExit();
        }
        private void btnResetPaymentErrorCount_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (fpnlBillingCycleActions.Tag != null)
                {
                    DataGridViewRow dataGridViewRow = (DataGridViewRow)fpnlBillingCycleActions.Tag;

                    if (dataGridViewRow != null && dataGridViewRow.Cells["subscriptionBillingScheduleId"] != null)
                    {
                        object billingScheduleIdObj = dataGridViewRow.Cells["subscriptionBillingScheduleId"].Value;
                        if (billingScheduleIdObj != null && billingScheduleIdObj != DBNull.Value)
                        {
                            int billingScheduleId = Convert.ToInt32(billingScheduleIdObj);
                            if (billingScheduleId > -1)
                            {
                                object trxIdObj = dataGridViewRow.Cells["dgvColumnTransactionId"].Value;
                                if (trxIdObj != null && trxIdObj != DBNull.Value)
                                {
                                    int trxId = Convert.ToInt32(trxIdObj);
                                    if (trxId > -1)
                                    {
                                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2989));
                                        //"Sorry, this schedule is already billed"
                                    }
                                }
                                SubscriptionBillingScheduleDTO selectedSchedule = subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList.Find(sbs => sbs.SubscriptionBillingScheduleId == billingScheduleId);
                                if (selectedSchedule != null)
                                {
                                    if (selectedSchedule.Status != SubscriptionStatus.ACTIVE)
                                    {
                                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2990, SubscriptionStatus.ACTIVE));
                                        // "Subscription billing schedule is not in &1 status"
                                    }
                                    if (selectedSchedule.PaymentProcessingFailureCount == 0)
                                    {
                                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2991));
                                        //"Invalid action, payment processing failure count is zero"
                                    }
                                    DateTime serverDate = ServerDateTime.Now.Date;
                                    bool skipSave = false;
                                    if (selectedSchedule.BillToDate < serverDate)
                                    {
                                        string msg = MessageContainerList.GetMessage(executionContext, 5234, selectedSchedule.BillFromDate.ToString(DATE_FORMAT), selectedSchedule.BillToDate.ToString(DATE_FORMAT));
                                        //"Entitlements for billing schedule [ &1 to &2 ] has expired. Do you want to cancel the line?"
                                        string msgT = MessageContainerList.GetMessage(executionContext, "Reset Payment Error Count");
                                        DialogResult dr = POSUtils.ParafaitMessageBox(msg, msgT, MessageBoxButtons.YesNo);
                                        if (dr == DialogResult.Yes)
                                        {
                                            skipSave = true;
                                            string mgrLoginId = string.Empty;
                                            if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, CANCEL_SUBSCRIPTION_REQUIRES_MANAGER_APPROVAL))
                                            { 
                                                mgrLoginId = SubscriptionUIHelper.GetManagerApproval(executionContext);
                                            }
                                            using (ParafaitDBTransaction dBTransaction = new ParafaitDBTransaction())
                                            {
                                                try
                                                {
                                                    dBTransaction.BeginTransaction();
                                                    selectedSchedule.CancellationApprovedBy = mgrLoginId;
                                                    SubscriptionBillingScheduleBL subscriptionBillingScheduleBL = new SubscriptionBillingScheduleBL(executionContext, selectedSchedule);
                                                    subscriptionBillingScheduleBL.CancelSubscriptionLine(dBTransaction.SQLTrx);
                                                    SubscriptionBillingScheduleDTO nextPastUnbilledCycle = GetNextUnbilledCycle(selectedSchedule);
                                                    while (nextPastUnbilledCycle != null)
                                                    {
                                                        string msg1 = MessageContainerList.GetMessage(executionContext, 5235, nextPastUnbilledCycle.BillFromDate.ToString(DATE_FORMAT), nextPastUnbilledCycle.BillToDate.ToString(DATE_FORMAT));
                                                        //"Entitlements for next billing schedule [ &1 to &2 ] has expired. Do you want to cancel the line?"
                                                        string msgT1 = MessageContainerList.GetMessage(executionContext, "Reset Payment Error Count");
                                                        DialogResult dr1 = POSUtils.ParafaitMessageBox(msg1, msgT1, MessageBoxButtons.YesNo);
                                                        if (dr1 == DialogResult.Yes)
                                                        {
                                                            nextPastUnbilledCycle.CancellationApprovedBy = mgrLoginId;
                                                            subscriptionBillingScheduleBL = new SubscriptionBillingScheduleBL(executionContext, nextPastUnbilledCycle);
                                                            subscriptionBillingScheduleBL.CancelSubscriptionLine(dBTransaction.SQLTrx);
                                                            nextPastUnbilledCycle = GetNextUnbilledCycle(nextPastUnbilledCycle);
                                                        }
                                                        else
                                                        {
                                                            nextPastUnbilledCycle = null;
                                                        }
                                                    }
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
                                            }
                                        }
                                    }
                                    if (skipSave == false)
                                    {
                                        selectedSchedule.PaymentProcessingFailureCount = 0;
                                        SubscriptionBillingScheduleBL subscriptionBillingScheduleBL = new SubscriptionBillingScheduleBL(executionContext, selectedSchedule);
                                        subscriptionBillingScheduleBL.Save();
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
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 1824, ex.Message));
            }
            finally
            {
                this.Cursor = Cursors.Default;
                CallRefreshAfterTheTask();
            }
            log.LogMethodExit();
        }        
        private void btnPrintReceipt_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (fpnlBillingCycleActions.Tag != null)
                {
                    DataGridViewRow dataGridViewRow = (DataGridViewRow)fpnlBillingCycleActions.Tag;

                    if (dataGridViewRow != null && dataGridViewRow.Cells["dgvColumnTransactionId"] != null)
                    {
                        object TrxId = dataGridViewRow.Cells["dgvColumnTransactionId"].Value;
                        if (TrxId != null && TrxId != DBNull.Value)
                        {
                            if ((int)TrxId > -1)
                            {
                                if (POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 490), "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                                {
                                    RePrintTransaction((int)TrxId);
                                }
                            }
                            else
                            {
                                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2988));
                                // "'Sorry, unable to proceed. Subscription billing cycle is not linked with a transaction'"
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
                HideHeaderActionPanel();
                HideBillingActionPanel();
            }
            log.LogMethodExit();
        }
        private void RePrintTransaction(int trxId)
        {
            log.LogMethodEntry(trxId);
            bool rePrint = true;
            List<int> trxIdList = new List<int>();
            trxIdList.Add(trxId);

            string message = "";

            PrintMultipleTransactions printMultipleTransactions = new PrintMultipleTransactions(utilities);
            if (!printMultipleTransactions.Print(trxIdList, rePrint, ref message))
            {
                log.Error(message);
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 1824, message));
            }
            log.LogMethodExit();
        }
        private void btnEmailReceipt_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (fpnlBillingCycleActions.Tag != null)
                {
                    DataGridViewRow dataGridViewRow = (DataGridViewRow)fpnlBillingCycleActions.Tag;

                    if (dataGridViewRow != null && dataGridViewRow.Cells["dgvColumnTransactionId"] != null)
                    {
                        object trxId = dataGridViewRow.Cells["dgvColumnTransactionId"].Value;
                        if (trxId != null && trxId != DBNull.Value)
                        {
                            if ((int)trxId > -1)
                            {
                                string contentID = string.Empty;
                                string attachFile = null;
                                string transactionAttachment = null;
                                if (utilities.ParafaitEnv.CompanyLogo != null)
                                {
                                    contentID = "ParafaitLogo" + Guid.NewGuid().ToString() + ".jpg";//Content Id is the identifier for the image
                                    attachFile = POSStatic.GetCompanyLogoImageFile(contentID);
                                    if (string.IsNullOrWhiteSpace(attachFile))
                                    {
                                        contentID = "";
                                    }
                                }
                                TransactionUtils transactionUtils = new TransactionUtils(utilities);
                                Transaction transaction = transactionUtils.CreateTransactionFromDB((int)trxId, utilities);
                                SubscriptionEventsBL subscriptionEventsBL = null;
                                subscriptionEventsBL = new SubscriptionEventsBL(utilities,
                                                                                  (transaction.Trx_id == this.subscriptionHeaderDTO.TransactionId
                                                                                   ? ParafaitFunctionEvents.SUBSCRIPTION_PURCHASE_EVENT
                                                                                   : ParafaitFunctionEvents.BILL_SUBSCRIPTION_EVENT), this.subscriptionHeaderDTO, transaction);

                                List<string> emailMsg = subscriptionEventsBL.BuildEmailMessage(null);
                                if (emailMsg != null && emailMsg.Any())
                                {
                                    SendEmailUI semail;
                                    string emailId = emailMsg[0];
                                    string ccEmailId = emailMsg[1];
                                    string bccEmailId = emailMsg[2];
                                    string emailSubject = emailMsg[3];
                                    string body = emailMsg[4];
                                    transactionAttachment = emailMsg[5];
                                    semail = new SendEmailUI(emailId,
                                                                    ccEmailId, "", emailSubject, body, transactionAttachment, "", attachFile, contentID, false, utilities, true);
                                    semail.ShowDialog();
                                    if (semail.EmailSentSuccessfully)
                                    {
                                        log.Info("Email Send Successfully");
                                    }
                                }
                                if (string.IsNullOrEmpty(attachFile) == false)
                                {//Delete the image created in the image folder once Email is sent successfully//
                                    FileInfo file = new FileInfo(attachFile);
                                    if (file.Exists)
                                    {
                                        file.Delete();
                                    }
                                }
                                if (string.IsNullOrEmpty(transactionAttachment) == false)
                                {//Delete the attachment file
                                    FileInfo file = new FileInfo(transactionAttachment);
                                    if (file.Exists)
                                    {
                                        file.Delete();
                                    }
                                }
                            }
                            else
                            {
                                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2988));
                                // "'Sorry, unable to proceed. Subscription billing cycle is not linked with a transaction'"
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
                HideHeaderActionPanel();
                HideBillingActionPanel();
            }
            log.LogMethodExit();
        }
        private void btnExpandCollapse_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                Button senderBtn = (Button)sender;
                if (senderBtn != null)
                {
                    CollapseExpandPanels(senderBtn);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 1824, ex.Message));
            }
            log.LogMethodExit();
        }

        private void CollapseExpandPanels(Button senderBtn)
        {
            log.LogMethodEntry();
            bool isExpandPanel = false;
            if (senderBtn.Tag == null || string.IsNullOrWhiteSpace(senderBtn.Tag.ToString()) || senderBtn.Tag.ToString() == COLLAPSEPANEL)
            {
                isExpandPanel = true;
            }
            if (senderBtn.Name == "btnExpandCollapse1")
            {
                if (isExpandPanel)
                {
                    this.pnlMainHeaderBody.Show();
                    this.pnlPaymentAndSeason.Hide();
                    this.pnlRenewalAndReminders.Hide();
                    this.btnExpandCollapse1.Tag = EXPANDPANEL;
                    this.btnExpandCollapse1.Image = Properties.Resources.CollapseArrow;
                    this.btnExpandCollapse2.Tag = COLLAPSEPANEL;
                    this.btnExpandCollapse2.Image = Properties.Resources.ExpandArrow;
                    this.btnExpandCollapse3.Tag = COLLAPSEPANEL;
                    this.btnExpandCollapse3.Image = Properties.Resources.ExpandArrow;
                }
                else
                {
                    this.pnlMainHeaderBody.Hide();
                    this.btnExpandCollapse1.Tag = COLLAPSEPANEL;
                    this.btnExpandCollapse1.Image = Properties.Resources.ExpandArrow;
                }
            }
            else if (senderBtn.Name == "btnExpandCollapse2")
            {
                if (isExpandPanel)
                {
                    this.pnlMainHeaderBody.Hide();
                    this.pnlPaymentAndSeason.Show();
                    this.pnlRenewalAndReminders.Hide();
                    this.btnExpandCollapse1.Tag = COLLAPSEPANEL;
                    this.btnExpandCollapse1.Image = Properties.Resources.ExpandArrow;
                    this.btnExpandCollapse2.Tag = EXPANDPANEL;
                    this.btnExpandCollapse2.Image = Properties.Resources.CollapseArrow;
                    this.btnExpandCollapse3.Tag = COLLAPSEPANEL;
                    this.btnExpandCollapse3.Image = Properties.Resources.ExpandArrow;
                }
                else
                {
                    this.pnlPaymentAndSeason.Hide();
                    this.btnExpandCollapse2.Tag = COLLAPSEPANEL;
                    this.btnExpandCollapse2.Image = Properties.Resources.ExpandArrow;
                }
            }
            else //btnExpandCollapse3
            {
                if (isExpandPanel)
                {
                    this.pnlMainHeaderBody.Hide();
                    this.pnlPaymentAndSeason.Hide();
                    this.pnlRenewalAndReminders.Show();
                    this.btnExpandCollapse1.Tag = COLLAPSEPANEL;
                    this.btnExpandCollapse1.Image = Properties.Resources.ExpandArrow;
                    this.btnExpandCollapse2.Tag = COLLAPSEPANEL;
                    this.btnExpandCollapse2.Image = Properties.Resources.ExpandArrow;
                    this.btnExpandCollapse3.Tag = isExpandPanel;
                    this.btnExpandCollapse3.Image = Properties.Resources.CollapseArrow;
                }
                else
                {
                    this.pnlRenewalAndReminders.Hide();
                    this.btnExpandCollapse3.Tag = COLLAPSEPANEL;
                    this.btnExpandCollapse3.Image = Properties.Resources.ExpandArrow;
                }
            }
            log.LogMethodExit();
        }

        private void pnlMainHeader_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                HideHeaderActionPanel();
                HideBillingActionPanel();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        private void pnlMainHeaderBody_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                HideHeaderActionPanel();
                HideBillingActionPanel();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        private void lblHeaderName_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                HideHeaderActionPanel();
                HideBillingActionPanel();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        private void pnlPaymentSeason_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                HideHeaderActionPanel();
                HideBillingActionPanel();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        private void lblPaymentSeasonHeader_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                HideHeaderActionPanel();
                HideBillingActionPanel();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        private void pnlPaymentAndSeason_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                HideHeaderActionPanel();
                HideBillingActionPanel();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        private void pnlRenewalHeader_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                HideHeaderActionPanel();
                HideBillingActionPanel();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        private void lblRenewalHeader_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                HideHeaderActionPanel();
                HideBillingActionPanel();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        private void pnlRenewalAndReminders_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                HideHeaderActionPanel();
                HideBillingActionPanel();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
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
        private void btnDetails_MouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                Button senderBtn = (Button)sender;
                senderBtn.BackgroundImage = global::Parafait_POS.Properties.Resources.More_Options_Btn_Pressed;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        private void btnDetails_MouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                Button senderBtn = (Button)sender;
                senderBtn.BackgroundImage = global::Parafait_POS.Properties.Resources.More_Options_Btn_Normal;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void SetCodeDescriptions()
        {
            log.LogMethodEntry();
            //UOMDAYS_DESC = UnitOfSubscriptionCycle.GetUnitOfSubscriptionCycleDescription(executionContext, UnitOfSubscriptionCycle.DAYS);
            //UOMMONTH_DESC = UnitOfSubscriptionCycle.GetUnitOfSubscriptionCycleDescription(executionContext, UnitOfSubscriptionCycle.MONTHS);
            //UOMYEAR_DESC = UnitOfSubscriptionCycle.GetUnitOfSubscriptionCycleDescription(executionContext, UnitOfSubscriptionCycle.YEARS);
            //CYCLE_PAYMODE_DESC = SubscriptionPaymentCollectionMode.GetPaymentDescription(SubscriptionPaymentCollectionMode.SUBSCRIPTION_CYCLE);
            //FULL_PAYMODE_DESC = SubscriptionPaymentCollectionMode.GetPaymentDescription(SubscriptionPaymentCollectionMode.FULL);
            //CHOICE_PAYMODE_DESC = SubscriptionPaymentCollectionMode.GetPaymentDescription(SubscriptionPaymentCollectionMode.CUSTOMER_CHOICE);
            //CANCEL_UNBILLED_DESC = SubscriptionCancellationOption.GetCancellationOptionDescription(SubscriptionCancellationOption.CANCELL_UNBILLED_CYCLES);
            //CANCEL_AUTO_REN_DESC = SubscriptionCancellationOption.GetCancellationOptionDescription(SubscriptionCancellationOption.CANCEL_AUTO_RENEWAL_ONLY);
            BILLING_LINE_DESC = SubscriptionLineType.GetSubscriptionLineTypeDescription(SubscriptionLineType.BILLING_LINE);
            GRACE_LINE_DESC = SubscriptionLineType.GetSubscriptionLineTypeDescription(SubscriptionLineType.GRACE_LINE);
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

        private void dgvSubscriptionBillingSchedule_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            log.LogMethodExit(e);
            try
            {
                if (e.ColumnIndex > -1 && e.RowIndex > -1)
                {
                    if (dgvSubscriptionBillingSchedule.Columns[e.ColumnIndex].Name == "LineType")
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
        private SubscriptionBillingScheduleDTO GetNextUnbilledCycle(SubscriptionBillingScheduleDTO selectedSchedule)
        {
            log.LogMethodExit(selectedSchedule);
            DateTime serverDate = ServerDateTime.Now.Date;
            SubscriptionBillingScheduleDTO retValue = null;
            List<SubscriptionBillingScheduleDTO> unbilledCycles = subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList.FindAll(sbs => sbs.SubscriptionBillingScheduleId != selectedSchedule.SubscriptionBillingScheduleId
                                 && sbs.BillOnDate > selectedSchedule.BillOnDate && sbs.IsActive && sbs.Status == SubscriptionStatus.ACTIVE
                                 && sbs.TransactionId == -1 && sbs.BillToDate < serverDate);
            if (unbilledCycles != null && unbilledCycles.Any())
            {
                retValue = unbilledCycles.OrderBy(sbs => sbs.BillOnDate).First();
            }
            log.LogMethodExit(retValue);
            return retValue;
        }
    }
}
