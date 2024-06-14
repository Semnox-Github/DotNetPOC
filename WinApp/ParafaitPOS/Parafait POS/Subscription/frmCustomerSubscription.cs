/********************************************************************************************
 * Project Name - frmCustomerSubscription 
 * Description  - form class to show Customer Subscriptions
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************   
 *2.110.0     21-Dec-2020    Guru S A             Created for Subscription changes
 *2.140       14-Oct-2021     Guru S A             Modified for Payment Settlements 
 ********************************************************************************************/
using ParafaitPOS;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Device.PaymentGateway;
using Semnox.Parafait.Languages;
using Semnox.Parafait.TableAttributeSetupUI;
using Semnox.Parafait.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Parafait_POS.Subscription
{
    public partial class frmCustomerSubscription : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private Utilities utilities;
        private List<SubscriptionHeaderDTO> subscriptionHeaderDTOList;
        private int customerId = -1;
        private usrCtlSubscriptions usrCtlSubscriptionsCC;
        private PaymentModeDTO creditCardPaymentModeDTO;
        /// <summary>
        /// frmCustomerSubscription
        /// </summary>
        /// <param name="utilities"></param>
        /// <param name="customerId"></param>
        /// <param name="subscriptionHeaderDTOList"></param>
        public frmCustomerSubscription(Utilities utilities, int customerId, List<SubscriptionHeaderDTO> subscriptionHeaderDTOList)
        {
            log.LogMethodEntry(customerId, subscriptionHeaderDTOList);
            this.customerId = customerId;
            this.utilities = utilities;
            this.executionContext = utilities.ExecutionContext;
            this.subscriptionHeaderDTOList = subscriptionHeaderDTOList;
            //this.selectedCustomerDTO = null;
            //this.serverDateTime = new LookupValuesList(executionContext);
            InitializeComponent();
            LoadSubscriptionUserControl();
            SetDGVFormats();
            LoadCreditCardPaymentModeDTO();
            log.LogMethodExit();
        }

        private void LoadSubscriptionUserControl()
        {
            log.LogMethodEntry();
            this.usrCtlSubscriptionsCC = new usrCtlSubscriptions(utilities, this.customerId, this.subscriptionHeaderDTOList);
            this.usrCtlSubscriptionsCC.BackColor = System.Drawing.Color.White;
            this.usrCtlSubscriptionsCC.Font = new System.Drawing.Font("Arial", 9F);
            this.usrCtlSubscriptionsCC.Location = new System.Drawing.Point(7, 7);
            this.usrCtlSubscriptionsCC.Name = "usrCtlSubscriptionsCC";
            //this.usrCtlSubscriptionsCC.Size = new System.Drawing.Size(1213, 562);
            this.usrCtlSubscriptionsCC.TabIndex = 0; 
            this.tabSubscriptions.Controls.Add(this.usrCtlSubscriptionsCC);
            log.LogMethodExit();
        }

        private void SetDGVFormats()
        {
            log.LogMethodEntry();
            utilities.setupDataGridProperties(ref dgvCreditCards);
            lastCreditCardExpiryReminderSentOnDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewDateTimeCellStyle();
            dgvCreditCards = SubscriptionUIHelper.SetDGVCellFont(executionContext, dgvCreditCards);
            log.LogMethodExit();
        }

        private void LoadDGVCustomerCreditCards()
        {
            log.LogMethodEntry();
            if (this.customerId > -1)
            {
                LoadPaymentModeList();
                CustomerCreditCardsListBL customerCreditCardsListBL = new CustomerCreditCardsListBL(executionContext);
                List<KeyValuePair<CustomerCreditCardsDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CustomerCreditCardsDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<CustomerCreditCardsDTO.SearchByParameters, string>(CustomerCreditCardsDTO.SearchByParameters.CUSTOMER_ID, this.customerId.ToString()));
                searchParameters.Add(new KeyValuePair<CustomerCreditCardsDTO.SearchByParameters, string>(CustomerCreditCardsDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                List<CustomerCreditCardsDTO> customerCreditCardsDTOList = customerCreditCardsListBL.GetCustomerCreditCardsDTOList(searchParameters, utilities);
                dgvCreditCards.DataSource = customerCreditCardsDTOList;
            }
            log.LogMethodExit();
        }

        private void LoadPaymentModeList()
        {
            log.LogMethodEntry();
            List<PaymentModeDTO> paymentModeDTOList = SubscriptionUIHelper.GetCreditCardPaymentModeDTO(executionContext);
            dgvColumnPaymentModeId.DataSource = paymentModeDTOList;
            dgvColumnPaymentModeId.ValueMember = "PaymentModeId";
            dgvColumnPaymentModeId.DisplayMember = "PaymentMode";
            log.LogMethodExit();
        }

        private void frmCustomerSubscription_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();

            System.Drawing.Rectangle workingRectangle = Screen.PrimaryScreen.WorkingArea;
            log.LogVariableState("workingRectangle.Width", workingRectangle.Width);
            log.LogVariableState("this.Width", this.Width);
            log.LogVariableState("tabCtrlSubscriptions.Size- Before", this.tabCtrlSubscriptions.Size);
            if (this.Width > workingRectangle.Width - 100)
            {
                this.SuspendLayout();
                this.Size = new System.Drawing.Size(workingRectangle.Width - 20, this.Height);
                this.tabCtrlSubscriptions.Size = new System.Drawing.Size(this.Width-10, tabCtrlSubscriptions.Height);
                this.tabCustomerCards.AutoScroll = true;
                this.tabSubscriptions.AutoScroll = true;
                this.ResumeLayout(true);
            }
            log.LogVariableState("tabCtrlSubscriptions.Size- After", this.tabCtrlSubscriptions.Size);
            LoadDGVCustomerCreditCards();
            log.LogMethodExit();
        }


        private void dgvCreditCards_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (e.RowIndex > -1 && e.ColumnIndex > -1)
                {

                    if (dgvCreditCards.Columns[e.ColumnIndex].Name == "dgvCardColumnIsActive")
                    {
                        DataGridViewCheckBoxCell checkBox = (dgvCreditCards["dgvCardColumnIsActive", e.RowIndex] as DataGridViewCheckBoxCell);
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
        private void LoadCreditCardPaymentModeDTO()
        {
            log.LogMethodEntry();
            creditCardPaymentModeDTO = null;
            PaymentModeList paymentModeList = new PaymentModeList(executionContext);
            List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.ISCREDITCARD, "Y"));
            searchParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            List<PaymentModeDTO> tempList = paymentModeList.GetAllPaymentModeList(searchParameters);
            if (tempList != null && tempList.Any())
            {
                for (int i = 0; i < tempList.Count; i++)
                {
                    if (tempList[i].PaymentGateway != null
                        && tempList[i].PaymentGateway.LookupValue == PaymentGateways.CardConnect.ToString())
                    {
                        creditCardPaymentModeDTO = new PaymentMode(executionContext, tempList[i].PaymentModeId).GetPaymentModeDTO;
                    }
                }
            }
            log.LogMethodExit(creditCardPaymentModeDTO);
        }
        private void btnAddCustomerCreditCard_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (customerId > -1)
                {
                    double creditCardAmount = 1;
                    if (creditCardPaymentModeDTO == null)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2985));
                        //"Supporting payment gateway for subscription billing is not enabled or has incorrect setup"
                    }
                    //if (creditCardAmount != 0)
                    // {
                    TransactionPaymentsDTO creditTransactionPaymentsDTO = new TransactionPaymentsDTO();
                    creditTransactionPaymentsDTO.PaymentModeId = creditCardPaymentModeDTO.PaymentModeId;
                    creditTransactionPaymentsDTO.paymentModeDTO = creditCardPaymentModeDTO;
                    creditTransactionPaymentsDTO.TransactionId = -1;
                    creditTransactionPaymentsDTO.Amount = creditCardAmount;
                    creditTransactionPaymentsDTO.paymentModeDTO.GatewayLookUp = creditCardPaymentModeDTO.GatewayLookUp;
                    creditTransactionPaymentsDTO.SubscriptionAuthorizationMode = SubscriptionAuthorizationMode.I;
                    App.machineUserContext = executionContext;
                    App.EnsureApplicationResources();
                    creditTransactionPaymentsDTO = TableAttributesUIHelper.GetEnabledAttributeDataForPaymentMode(executionContext, creditTransactionPaymentsDTO);
                    if (creditTransactionPaymentsDTO != null)
                    {
                        string lmessage = string.Empty;
                        try
                        {
                            DisableButtons();
                            if (!CreditCardPaymentGateway.MakePayment(creditTransactionPaymentsDTO, utilities, ref lmessage))
                            {
                                log.Info("Ends-apply() as unable to Make CC payment " + lmessage);
                                throw new ValidationException(MessageContainerList.GetMessage(executionContext, lmessage));
                            }
                            else
                            {
                                CreateNewCustomerCreditCardRecord(creditTransactionPaymentsDTO);
                                lmessage = string.Empty;
                                if (!CreditCardPaymentGateway.RefundAmount(creditTransactionPaymentsDTO, utilities, ref lmessage))
                                {
                                    log.Error("Cannot reverse payment as Transaction save failed: " + lmessage);
                                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, lmessage));
                                }
                            }
                        }
                        finally
                        {
                            EnableButtons();
                        }

                    }
                }
                else
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2157));
                    //Valid Customer is required
                }
                //}
            }
            catch (Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 1824, ex.Message));
            }
            finally
            {
                try
                {
                    LoadDGVCustomerCreditCards();
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 1824, ex.Message));
                }
            }
            log.LogMethodExit();
        }
        private void CreateNewCustomerCreditCardRecord(TransactionPaymentsDTO creditTransactionPaymentsDTO)
        {
            log.LogMethodEntry(creditTransactionPaymentsDTO);
            if (creditTransactionPaymentsDTO != null)
            {
                CCTransactionsPGWListBL cCTransactionsPGWListBL = new CCTransactionsPGWListBL();
                List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>> searchParamCCT = new List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>>();
                searchParamCCT.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.RESPONSE_ID, creditTransactionPaymentsDTO.CCResponseId.ToString()));
                searchParamCCT.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                List<CCTransactionsPGWDTO> cCTransactionsPGWDTOList = cCTransactionsPGWListBL.GetCCTransactionsPGWDTOList(searchParamCCT, null);
                if (cCTransactionsPGWDTOList != null && cCTransactionsPGWDTOList.Any())
                {                    
                    CustomerCreditCardsDTO customerCreditCardsDTO = new CustomerCreditCardsDTO(-1,this.customerId, cCTransactionsPGWDTOList[0].CustomerCardProfileId, creditTransactionPaymentsDTO.PaymentModeId, cCTransactionsPGWDTOList[0].TokenID,
                        creditTransactionPaymentsDTO.NameOnCreditCard, creditTransactionPaymentsDTO.CreditCardNumber, creditTransactionPaymentsDTO.CreditCardExpiry, null, null);
                    CustomerCreditCardsListBL customerCreditCardsListBL = new CustomerCreditCardsListBL(executionContext);
                    List<KeyValuePair<CustomerCreditCardsDTO.SearchByParameters, string>> searchParamsCCC = new List<KeyValuePair<CustomerCreditCardsDTO.SearchByParameters, string>>();
                    searchParamsCCC.Add(new KeyValuePair<CustomerCreditCardsDTO.SearchByParameters, string>(CustomerCreditCardsDTO.SearchByParameters.CUSTOMER_ID, customerCreditCardsDTO.CustomerId.ToString()));
                    //searchParamsCCC.Add(new KeyValuePair<CustomerCreditCardsDTO.SearchByParameters, string>(CustomerCreditCardsDTO.SearchByParameters.CARD_PROFILE_ID, customerCreditCardsDTO.CardProfileId));
                    searchParamsCCC.Add(new KeyValuePair<CustomerCreditCardsDTO.SearchByParameters, string>(CustomerCreditCardsDTO.SearchByParameters.TOKEN_ID, customerCreditCardsDTO.TokenId));
                    List<CustomerCreditCardsDTO> customerCreditCardsDTOList = customerCreditCardsListBL.GetCustomerCreditCardsDTOList(searchParamsCCC, utilities, null);
                    if (customerCreditCardsDTOList != null && customerCreditCardsDTOList.Any())
                    {
                        //if record exists then use the same
                        customerCreditCardsDTO = customerCreditCardsDTOList[0];
                        customerCreditCardsDTO.CardProfileId = cCTransactionsPGWDTOList[0].CustomerCardProfileId; //retain latest profile instead of creating new record
                        customerCreditCardsDTO.CardExpiry = creditTransactionPaymentsDTO.CreditCardExpiry;
                    }
                    CustomerCreditCardsBL customerCreditCardsBL = new CustomerCreditCardsBL(executionContext, customerCreditCardsDTO);
                    customerCreditCardsBL.Save(null); 
                }
                else
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2880));
                    //Unable to fetch credit card payment gateway transaction details
                }
            }
            log.LogMethodExit();
        }
        private void DisableButtons()
        {
            log.LogMethodEntry();
            this.Enabled = false;
            Application.DoEvents();
            log.LogMethodExit();
        }
        private void EnableButtons()
        {
            log.LogMethodEntry();
            Application.DoEvents();
            this.Enabled = true;
            Application.DoEvents();
            log.LogMethodExit();
        }
    }
}
