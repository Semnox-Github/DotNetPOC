/********************************************************************************************
 * Project Name - usrCtlSubscriptions 
 * Description  - user conrtrol for Subscriptions
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
using System.Linq;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;

namespace Parafait_POS.Subscription
{
    /// <summary>
    /// usrCtlSubscriptions
    /// </summary>
    public partial class usrCtlSubscriptions : UserControl
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private Utilities utilities;
        private int customerId = -1;
        private CustomerDTO selectedCustomerDTO;
        private List<SubscriptionHeaderDTO> subscriptionHeaderDTOList;
        private LookupValuesList serverDateTime;
        private VirtualKeyboardController virtualKeyboard;
        private bool initialized = false;
        private const string SUBSCRIPTION_FEATURE_SETUP = "SUBSCRIPTION_FEATURE_SETUP";
        private const string RETRYPAYMENTFAILEDBILLCYCLE = "RetryPaymentFailedBillCycle"; 
        private int retryCount = 0;
        /// <summary>
        /// usrCtlSubscriptions
        /// </summary>
        public usrCtlSubscriptions()
        {
            log.LogMethodEntry();
            if (initialized == false)
            {
                InitializeComponent();
                initialized = true;
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// usrCtlSubscriptions
        /// </summary>
        /// <param name="utilities"></param>
        /// <param name="customerId"></param>
        /// <param name="subscriptionHeaderDTOList"></param>
        public usrCtlSubscriptions(Utilities utilities, int customerId, List<SubscriptionHeaderDTO> subscriptionHeaderDTOList) : this()
        {
            log.LogMethodEntry(customerId, subscriptionHeaderDTOList);
            this.customerId = customerId;
            this.utilities = utilities;
            this.executionContext = utilities.ExecutionContext;
            this.subscriptionHeaderDTOList = subscriptionHeaderDTOList;
            this.selectedCustomerDTO = null;
            this.serverDateTime = new LookupValuesList(executionContext);
            // InitializeComponent();
            LoadSubscriptionSetupDetails();
            LoadSubscriptionProductList();
            LoadSubscriptionStatus();
            SetActiveStatus();
            SetDisplay();
            LoadDGVSubscriptionHeaderData();
            virtualKeyboard = new VirtualKeyboardController();
            virtualKeyboard.Initialize(this, new List<Control>() { btnShowKeyPad }, ParafaitDefaultContainerList.GetParafaitDefault<bool>(utilities.ExecutionContext, "AUTO_POPUP_ONSCREEN_KEYBOARD"));
            this.utilities.setLanguage(this);
            log.LogMethodExit();
        }
        private void LoadSubscriptionProductList()
        {
            log.LogMethodEntry();
            ProductSubscriptionListBL productSubscriptionListBL = new ProductSubscriptionListBL(executionContext);
            List<KeyValuePair<ProductSubscriptionDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ProductSubscriptionDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<ProductSubscriptionDTO.SearchByParameters, string>(ProductSubscriptionDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            List<ProductSubscriptionDTO> productSubscriptionDTOList = productSubscriptionListBL.GetProductSubscriptionDTOList(searchParameters);
            if (productSubscriptionDTOList == null || productSubscriptionDTOList.Any() == false)
            {
                productSubscriptionDTOList = new List<ProductSubscriptionDTO>();
            }
            productSubscriptionDTOList.Insert(0, new ProductSubscriptionDTO());
            cmbSubscriptionProducts.DataSource = productSubscriptionDTOList;
            cmbSubscriptionProducts.ValueMember = "ProductSubscriptionId";
            cmbSubscriptionProducts.DisplayMember = "ProductSubscriptionName";
            log.LogMethodExit();
        }
        private void LoadSubscriptionStatus()
        {
            log.LogMethodEntry();
            cmbSubscriptionStatus = SubscriptionUIHelper.LoadSubscriptionStatus(executionContext, cmbSubscriptionStatus);
            log.LogMethodExit();
        }
        private void SetActiveStatus()
        {
            log.LogMethodEntry();
            try
            {
                cmbSubscriptionStatus.SelectedValue = SubscriptionStatus.ACTIVE;
            }
            catch { }
            log.LogMethodExit();
        }
        private void SetDisplay()
        {
            log.LogMethodEntry();
            SetCustomerFields(this.customerId);
            SetDGVFormats();
            //System.Drawing.Rectangle workingRectangle = Screen.PrimaryScreen.WorkingArea;
            //if (this.Width > workingRectangle.Width - 100)
            //{
            //    this.pnlPage.Size = new System.Drawing.Size(workingRectangle.Width - 20, pnlPage.Height);
            //    this.pnlPage.AutoScroll = true; 
            //}
            log.LogMethodExit();
        }
        private void SetCustomerFields(int customerId)
        {
            log.LogMethodEntry(customerId);
            if (customerId > -1)
            {
                CustomerBL customerBL = new CustomerBL(executionContext, customerId);
                txtCustFirstName.ReadOnly = true;
                txtCustFirstName.Text = customerBL.CustomerDTO.FirstName;
                btnCustomerLookup.Enabled = false;
            }
            log.LogMethodExit();
        }
        private void SetDGVFormats()
        {
            log.LogMethodEntry();
            utilities.setupDataGridProperties(ref dgvSubscriptionHeader);
            LastBillOnDate.DefaultCellStyle = utilities.gridViewDateCellStyle();
            NextBillOnDate.DefaultCellStyle = utilities.gridViewDateCellStyle();
            SubscriptionExpiryDate.DefaultCellStyle = utilities.gridViewDateCellStyle();
            dgvSubscriptionHeader = SubscriptionUIHelper.SetDGVCellFont(executionContext, dgvSubscriptionHeader);
            log.LogMethodExit();
        }
        private void LoadCustomerList(List<SubscriptionHeaderDTO> subscriptionHeaderDTOList)
        {
            log.LogMethodEntry();
            List<CustomerDTO> customerDTOList = GetCustomerDTOList(subscriptionHeaderDTOList);
            dgvColumnCustomerId.DataSource = customerDTOList;
            dgvColumnCustomerId.ValueMember = "Id";
            dgvColumnCustomerId.DisplayMember = "FirstName";
            log.LogMethodExit();
        }
        private List<CustomerDTO> GetCustomerDTOList(List<SubscriptionHeaderDTO> subscriptionHeaderDTOList)
        {
            log.LogMethodEntry();
            List<CustomerDTO> customerDTOList = new List<CustomerDTO>();
            if (subscriptionHeaderDTOList != null && subscriptionHeaderDTOList.Any())
            {
                List<int> customerIdList = subscriptionHeaderDTOList.Select(sh => sh.CustomerId).Distinct().ToList();
                CustomerListBL customerListBL = new CustomerListBL(executionContext);
                customerDTOList = customerListBL.GetCustomerDTOList(customerIdList, true);
            }
            if (customerDTOList == null)
            {
                customerDTOList = new List<CustomerDTO>();
            }
            customerDTOList.Insert(0, new CustomerDTO());
            log.LogMethodExit(customerDTOList);
            return customerDTOList;
        }
        private void LoadDGVSubscriptionHeaderData()
        {
            log.LogMethodEntry();
            if (this.subscriptionHeaderDTOList != null && this.subscriptionHeaderDTOList.Any())
            {
                LoadDGVSubscriptionHeader(this.subscriptionHeaderDTOList);
            }
            else
            {
                List<SubscriptionHeaderDTO> subscriptionHeaderDTOList = SearchSubscriptionHeader();
                LoadDGVSubscriptionHeader(subscriptionHeaderDTOList);
            }
            log.LogMethodExit();
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                this.Cursor = Cursors.WaitCursor;
                List<SubscriptionHeaderDTO> subscriptionHeaderDTOList = SearchSubscriptionHeader();
                LoadDGVSubscriptionHeader(subscriptionHeaderDTOList);
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
        private List<SubscriptionHeaderDTO> SearchSubscriptionHeader()
        {
            log.LogMethodEntry();
            List<KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>> searchParam = new List<KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>>();
            searchParam.Add(new KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>(SubscriptionHeaderDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            if (cmbSubscriptionProducts.SelectedIndex > 0)
            {
                searchParam.Add(new KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>(SubscriptionHeaderDTO.SearchByParameters.PRODUCT_SUBSCRIPTION_ID, cmbSubscriptionProducts.SelectedValue.ToString()));
            }
            if (this.customerId > -1)
            {
                searchParam.Add(new KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>(SubscriptionHeaderDTO.SearchByParameters.CUSTOMER_ID, customerId.ToString()));
            }
            else
            {
                if (selectedCustomerDTO != null && selectedCustomerDTO.Id > -1)
                {
                    searchParam.Add(new KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>(SubscriptionHeaderDTO.SearchByParameters.CUSTOMER_ID, selectedCustomerDTO.Id.ToString()));
                }
                else if (string.IsNullOrEmpty(txtCustFirstName.Text) == false)
                {
                    searchParam.Add(new KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>(SubscriptionHeaderDTO.SearchByParameters.CUSTOMER_FIRST_NAME_LIKE, txtCustFirstName.Text));
                }
            }
            if (cmbSubscriptionStatus.SelectedIndex > -1)
            {
                if (cmbSubscriptionStatus.SelectedValue != null && cmbSubscriptionStatus.SelectedValue != "ALL")
                {
                    searchParam.Add(new KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>(SubscriptionHeaderDTO.SearchByParameters.STATUS, cmbSubscriptionStatus.SelectedValue.ToString()));
                }
            }
            if (cbxCardExpiresBeforeNextBilling.Checked)
            {
                searchParam.Add(new KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>(SubscriptionHeaderDTO.SearchByParameters.CREDIT_CARD_EXPIRES_BEFORE_NEXT_BILLING, "1"));
            }
            if (cbxCreditCardExpired.Checked)
            {
                searchParam.Add(new KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>(SubscriptionHeaderDTO.SearchByParameters.HAS_EXPIRED_CREDIT_CARD, "1"));
            }
            if (cbxHasPaymentFailure.Checked)
            {
                searchParam.Add(new KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>(SubscriptionHeaderDTO.SearchByParameters.LATEST_BILL_CYCLE_HAS_PAYMENT_ERROR, "1"));
            }
            if (cbxPaymentRetryLimitCrossed.Checked)
            {
                searchParam.Add(new KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>(SubscriptionHeaderDTO.SearchByParameters.REACHED_PAYMENT_RETRY_LIMIT, retryCount.ToString()));
            }
            SubscriptionHeaderListBL subscriptionHeaderListBL = new SubscriptionHeaderListBL(executionContext);
            List<SubscriptionHeaderDTO> subscriptionHeaderDTOList = subscriptionHeaderListBL.GetSubscriptionHeaderDTOList(searchParam, utilities, true);
            log.LogMethodExit();
            return subscriptionHeaderDTOList;
        }
        private void LoadDGVSubscriptionHeader(List<SubscriptionHeaderDTO> subscriptionHeaderDTOList)
        {
            log.LogMethodEntry();
            try
            {
                this.Cursor = Cursors.WaitCursor;
                dgvSubscriptionHeader.ReadOnly = false;
                dgvSubscriptionHeader.Rows.Clear();
                if (subscriptionHeaderDTOList != null && subscriptionHeaderDTOList.Any())
                {
                    LoadCustomerList(subscriptionHeaderDTOList);
                    //dgvSubscriptionHeader.DataSource = subscriptionHeaderDTOList;
                    DateTime currentServerTime = serverDateTime.GetServerDateTime();
                    List<CustomerCreditCardsDTO> customerCreditCardsDTOList;
                    List<int> customerCreditCardIdList = subscriptionHeaderDTOList.Select(sh => sh.CustomerCreditCardsId).Distinct().ToList();
                    CustomerCreditCardsListBL customerCreditCardsListBL = new CustomerCreditCardsListBL(executionContext);
                    customerCreditCardsDTOList = customerCreditCardsListBL.GetCustomerCreditCardsDTOList(customerCreditCardIdList);
                    List<KeyValuePair<int, bool>> cardsExpiryStatusList = GetCardExpiryStatusList(subscriptionHeaderDTOList, customerCreditCardsDTOList);
                    dgvSubscriptionHeader.Rows.Add(subscriptionHeaderDTOList.Count);
                    for (int i = 0; i < subscriptionHeaderDTOList.Count; i++)
                    {
                        SubscriptionHeaderDTO sDTO = subscriptionHeaderDTOList[i];

                        dgvSubscriptionHeader.Rows[i].Cells["dgvColumnSubscriptionHeaderId"].Value = sDTO.SubscriptionHeaderId;
                        dgvSubscriptionHeader.Rows[i].Cells["SubscriptionNumber"].Value = sDTO.SubscriptionNumber;
                        dgvSubscriptionHeader.Rows[i].Cells["dgvColumnCustomerId"].Value = sDTO.CustomerId;
                        dgvSubscriptionHeader.Rows[i].Cells["productSubscriptionNameDataGridViewTextBoxColumn"].Value = sDTO.ProductSubscriptionName;
                        dgvSubscriptionHeader.Rows[i].Cells["statusDataGridViewTextBoxColumn"].Value = sDTO.Status;
                        dgvSubscriptionHeader.Rows[i].Cells["LastBillOnDate"].Value = null;
                        dgvSubscriptionHeader.Rows[i].Cells["NextBillOnDate"].Value = null;
                        dgvSubscriptionHeader.Rows[i].Cells["SubscriptionExpiryDate"].Value = null;
                        (dgvSubscriptionHeader.Rows[i].Cells["LastPaymentStatus"] as DataGridViewImageCell).Value = Properties.Resources.Green;
                        (dgvSubscriptionHeader.Rows[i].Cells["CreditCardStatus"] as DataGridViewImageCell).Value = Properties.Resources.ActiveCreditCard;
                        if (sDTO.SubscriptionBillingScheduleDTOList != null && sDTO.SubscriptionBillingScheduleDTOList.Any())
                        {
                            // CreditCardStatus 
                            dgvSubscriptionHeader.Rows[i].Cells["LastBillOnDate"].Value = sDTO.SubscriptionBillingScheduleDTOList.Exists(sbs => sbs.TransactionId > -1 && sbs.IsActive && sbs.LineType == SubscriptionLineType.BILLING_LINE) ?
                                                             sDTO.SubscriptionBillingScheduleDTOList.Where(sbs => sbs.TransactionId > -1 && sbs.IsActive && sbs.LineType == SubscriptionLineType.BILLING_LINE).Max(sbs => sbs.BillOnDate) : (DateTime?)null;
                            dgvSubscriptionHeader.Rows[i].Cells["NextBillOnDate"].Value = sDTO.SubscriptionBillingScheduleDTOList.Exists(sbs => sbs.TransactionId == -1 && sbs.IsActive && sbs.LineType == SubscriptionLineType.BILLING_LINE) ?
                                                            sDTO.SubscriptionBillingScheduleDTOList.Where(sbs => sbs.TransactionId == -1 && sbs.IsActive && sbs.LineType == SubscriptionLineType.BILLING_LINE).Min(sbs => sbs.BillOnDate) : (DateTime?)null;
                            dgvSubscriptionHeader.Rows[i].Cells["SubscriptionExpiryDate"].Value = sDTO.SubscriptionBillingScheduleDTOList.Exists(sbs => sbs.IsActive && sbs.LineType == SubscriptionLineType.BILLING_LINE) ?
                                                            sDTO.SubscriptionBillingScheduleDTOList.Where(sbs => sbs.LineType == SubscriptionLineType.BILLING_LINE).Max(sbs => sbs.BillToDate) : (DateTime?)null;
                            if (sDTO.SubscriptionBillingScheduleDTOList.Exists(sbs => sbs.IsActive
                                                                                       //&& sbs.BillOnDate < currentServerTime
                                                                                       && sbs.TransactionId == -1 && sbs.PaymentProcessingFailureCount > 0))
                            {
                                (dgvSubscriptionHeader.Rows[i].Cells["LastPaymentStatus"] as DataGridViewImageCell).Value = Properties.Resources.Red;
                            }
                            if (cardsExpiryStatusList.Exists(cc => cc.Key == sDTO.CustomerCreditCardsId) == false)
                            {
                                CustomerCreditCardsDTO customerCreditCardsDTO = customerCreditCardsDTOList.Find(cc => cc.CustomerCreditCardsId == sDTO.CustomerCreditCardsId);
                                if (customerCreditCardsDTO != null)
                                {
                                    CustomerCreditCardsBL customerCreditCardsBL = new CustomerCreditCardsBL(executionContext, customerCreditCardsDTO);
                                    bool cardExpired = true;
                                    try
                                    {
                                        cardExpired = customerCreditCardsBL.CustomerCreditCardHasExpired(utilities);
                                    }
                                    catch (Exception ex)
                                    {
                                        log.Error(ex);
                                        POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 1824, ex.Message));
                                    }
                                    cardsExpiryStatusList.Add(new KeyValuePair<int, bool>(sDTO.CustomerCreditCardsId, cardExpired));
                                }
                                else
                                {
                                    cardsExpiryStatusList.Add(new KeyValuePair<int, bool>(sDTO.CustomerCreditCardsId, true));
                                }
                            }
                            if (cardsExpiryStatusList.Find(cc => cc.Key == sDTO.CustomerCreditCardsId).Value)
                            {
                                (dgvSubscriptionHeader.Rows[i].Cells["CreditCardStatus"] as DataGridViewImageCell).Value = Properties.Resources.ExpiredCreditCard;
                            }
                        }
                    }                     
                }
                vScrollDGVSubscriptionHeader.UpdateButtonStatus();
                hScrollDGVSubscriptionHeader.UpdateButtonStatus();
            }
            finally
            {
                dgvSubscriptionHeader.ReadOnly = true;
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }
        private List<KeyValuePair<int, bool>> GetCardExpiryStatusList(List<SubscriptionHeaderDTO> subscriptionHeaderDTOList, List<CustomerCreditCardsDTO> customerCreditCardsDTOList)
        {
            log.LogMethodEntry();
            List<KeyValuePair<int, bool>> cardsExpiryStatusList = new List<KeyValuePair<int, bool>>();
            if (customerCreditCardsDTOList != null && customerCreditCardsDTOList.Any())
            {
                CustomerCreditCardsBL customerCreditCardsBL = new CustomerCreditCardsBL(executionContext, customerCreditCardsDTOList[0]);
                bool cardExpired = true;
                try
                {
                    cardExpired = customerCreditCardsBL.CustomerCreditCardHasExpired(utilities);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 1824, ex.Message));
                }
                cardsExpiryStatusList.Add(new KeyValuePair<int, bool>(customerCreditCardsDTOList[0].CustomerCreditCardsId, cardExpired));
            }
            log.LogMethodExit(cardsExpiryStatusList);
            return cardsExpiryStatusList;
        }
        private void btnClearSearch_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                this.Cursor = Cursors.WaitCursor;
                cmbSubscriptionProducts.SelectedIndex = 0;
                if (this.customerId == -1)
                {
                    txtCustFirstName.Clear();
                }
                cmbSubscriptionStatus.SelectedValue = SubscriptionStatus.ACTIVE;
                cbxCardExpiresBeforeNextBilling.Checked = false;
                cbxCreditCardExpired.Checked = false;
                cbxHasPaymentFailure.Checked = false;
                cbxPaymentRetryLimitCrossed.Checked = false;
                selectedCustomerDTO = null;
                List<SubscriptionHeaderDTO> subscriptionHeaderDTOList = SearchSubscriptionHeader();
                LoadDGVSubscriptionHeader(subscriptionHeaderDTOList);
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
        private void btnCustomerLookup_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (this.customerId == -1)
                {
                    using (CustomerLookupUI customerLookupUI = new CustomerLookupUI(utilities, txtCustFirstName.Text))
                    {
                        if (customerLookupUI.ShowDialog() == DialogResult.OK)
                        {
                            selectedCustomerDTO = customerLookupUI.SelectedCustomerDTO;
                            txtCustFirstName.Text = selectedCustomerDTO.FirstName;
                        }
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
        private void dgvSubscriptionHeader_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (e.RowIndex > -1 && e.ColumnIndex > -1)
                {
                    if (dgvSubscriptionHeader.Columns[e.ColumnIndex].Name == "dgvColumnViewDetails")
                    {
                        int subscriptionHeaderId = Convert.ToInt32(dgvSubscriptionHeader["dgvColumnSubscriptionHeaderId", e.RowIndex].Value);
                        using (frmSubscriptionDetails frm = new frmSubscriptionDetails(utilities, true, subscriptionHeaderId))
                        {
                            frm.ShowDialog();
                        }
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
            log.LogMethodEntry(retryCount);
            log.Info("retryCount: " + retryCount.ToString());
            LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
            List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
            searchParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, SUBSCRIPTION_FEATURE_SETUP));
            List<LookupValuesDTO> lookupValuesDTOList = lookupValuesList.GetAllLookupValues(searchParameters);
            if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
            {
                for (int i = 0; i < lookupValuesDTOList.Count(); i++)
                {
                    switch (lookupValuesDTOList[i].LookupValue)
                    {
                        case RETRYPAYMENTFAILEDBILLCYCLE:
                            if (lookupValuesDTOList[i].Description == "Y")
                            {
                                //canRetryPaymentFailedBillCycle = true;
                                retryCount = 4;
                            }
                            else if (lookupValuesDTOList[i].Description == "N")
                            {
                                retryCount = 0;
                            }
                            else
                            {
                                if (int.TryParse(lookupValuesDTOList[i].Description, out retryCount) == false)
                                {
                                    retryCount = 0;
                                }
                            }
                            if (retryCount < 0)
                            {
                                retryCount = 0;
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            //retryCount = 2;
            log.Info("retryCount: " + retryCount.ToString());
            log.LogMethodExit(retryCount);
        }
    }
}
