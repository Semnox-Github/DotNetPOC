/********************************************************************************************
 * Project Name - Customer
 * Description  - CustomerVerificationUI
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.70.2        10-Aug-2019      Girish kundar      Modified :Removed Unused namespace's.
 **2.70.3        04-Feb-2020      Nitin Pai         Fix, verification was not working if the config is disabled
 *2.90.0        23-Jul-2020       Jinto Thomas      Modified: Verification Code generation by using customerverificationBL
 *                                                   and sending code to user by MessagingRequest   
 *2.140         14-Oct-2021       Guru              Modified for Payment Settlements  
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.Device.PaymentGateway;
using Semnox.Parafait.User;

namespace Semnox.Parafait.Customer
{
    /// <summary>
    /// Customer verification UI class
    /// </summary>
    public partial class CustomerVerificationUI : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Utilities utilities;
        private CustomerDTO customerDTO;
        private CustomerVerificationDTO customerVerificationDTO;
        private MessageBoxDelegate messageBoxDelegate;
        private static object staticDataExchange;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="utilities">parafait utilities</param>
        /// <param name="customerDTO">Customer DTO</param>
        /// <param name="messageBoxDelegate">MessageBox delegate</param>
        public CustomerVerificationUI(Utilities utilities, CustomerDTO customerDTO, MessageBoxDelegate messageBoxDelegate)
        {
            log.LogMethodEntry(utilities, customerDTO, messageBoxDelegate);
            InitializeComponent();
            this.utilities = utilities;
            utilities.setLanguage(this);
            this.messageBoxDelegate = messageBoxDelegate;
            this.customerDTO = customerDTO;
            log.LogMethodExit();
        }
        private void CustomerVerificationUI_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            btnOK.Enabled = false;
            log.LogMethodExit();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            this.Close();
            log.LogMethodExit();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (customerVerificationDTO != null)
            {
                customerDTO.CustomerVerificationDTO = customerVerificationDTO;
                customerDTO.ProfileDTO = customerVerificationDTO.ProfileDTO;
            }
            if (customerDTO.Id > 0)
            {
                SqlConnection sqlConnection = utilities.createConnection();
                SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
                object retValue =  0; // set the value as 0 so that the customer save is committed when registration bonus is disabled or not set up correctly

                try
                {
                    CustomerBL customerBL = new CustomerBL(utilities.ExecutionContext, customerDTO);
                    customerBL.Save(sqlTransaction);

                    //Starts Modification for registration bonus on verification on 27-Nov-2018
                    if (!(ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "REGISTRATION_BONUS_ON_VERIFICATION").Equals("N")))
                    {
                        AccountListBL accountBL = new AccountListBL(utilities.ExecutionContext);
                        List<KeyValuePair<AccountDTO.SearchByParameters, string>> accountSearchParameters = new List<KeyValuePair<AccountDTO.SearchByParameters, string>>();
                        accountSearchParameters.Add(new KeyValuePair<AccountDTO.SearchByParameters, string>(AccountDTO.SearchByParameters.CUSTOMER_ID, customerDTO.Id.ToString()));
                        List<AccountDTO> accountListDTO = accountBL.GetAccountDTOList(accountSearchParameters, true, true);

                        if (accountListDTO != null || accountListDTO.Count == 1)
                        {
                            string strProdId = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "LOAD_PRODUCT_ON_REGISTRATION");
                            int productId = -1;
                            string message = "";
                            if (int.TryParse(strProdId, out productId) == true && productId != -1)
                            {

                                Type cardType = Type.GetType("Semnox.Parafait.Transaction.Card,Transaction");
                                object card = null;
                                if (cardType != null)
                                {
                                    ConstructorInfo constructorN = cardType.GetConstructor(new Type[] { typeof(int), utilities.ParafaitEnv.LoginID.GetType(), typeof(Utilities), sqlTransaction.GetType() });
                                    card = constructorN.Invoke(new object[] { -1, utilities.ParafaitEnv.LoginID, utilities, sqlTransaction });
                                }
                                else
                                {
                                    throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, 1479, "Card"));
                                }


                                AccountListBL accountListBL = new AccountListBL(utilities.ExecutionContext);
                                List<KeyValuePair<AccountDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<AccountDTO.SearchByParameters, string>>();
                                searchParameters.Add(new KeyValuePair<AccountDTO.SearchByParameters, string>(AccountDTO.SearchByParameters.CUSTOMER_ID, customerDTO.Id.ToString()));
                                List<AccountDTO> accountDTOList = accountListBL.GetAccountDTOList(searchParameters, true, true);
                                AccountDTO accountDTO = null;
                                if (accountDTOList != null && accountDTOList.Count > 0)
                                {
                                    accountDTO = accountDTOList.Where(x => x.PrimaryAccount == true).FirstOrDefault();
                                    if (accountDTO == null)
                                        accountDTO = accountDTOList[0];
                                }

                                MethodInfo cardMethodType = cardType.GetMethod("getCardDetails", new[] { accountDTO.AccountId.GetType(), sqlTransaction.GetType() });
                                cardMethodType.Invoke(card, new object[] { accountDTO.AccountId, sqlTransaction });

                                Type transactionType = Type.GetType("Semnox.Parafait.Transaction.Transaction,Transaction");
                                object transaction = null;
                                if (transactionType != null)
                                {
                                    ConstructorInfo constructorN = transactionType.GetConstructor(new Type[] { utilities.GetType() });
                                    transaction = constructorN.Invoke(new object[] { utilities });
                                }
                                else
                                {
                                    throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, 1479, "Transaction"));
                                }

                                decimal i = 1;
                                MethodInfo transactionMethodInfo = transactionType.GetMethod("createTransactionLine", new[] { card.GetType(), productId.GetType(), typeof(decimal), message.GetType().MakeByRefType() });
                                transactionMethodInfo.Invoke(transaction, new object[] { card, productId, i, message });

                                FieldInfo transactionFieldInfo = transactionType.GetField("Net_Transaction_Amount");

                                PaymentModeList paymentModeListBL = new PaymentModeList(utilities.ExecutionContext);
                                List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>> searchPaymentModeParameters = new List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>>();
                                searchPaymentModeParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.SITE_ID, (utilities.ParafaitEnv.IsCorporate ? utilities.ParafaitEnv.SiteId : -1).ToString()));
                                searchPaymentModeParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.ISCASH, "Y"));
                                List<PaymentModeDTO> paymentModeDTOList = paymentModeListBL.GetPaymentModeList(searchPaymentModeParameters);
                                if (paymentModeDTOList != null)
                                {
                                    TransactionPaymentsDTO trxPaymentDTO = new TransactionPaymentsDTO(-1, -1, paymentModeDTOList[0].PaymentModeId, Convert.ToInt32(transactionFieldInfo.GetValue(transaction)),
                                                                                                      "", "", "", "", "", -1, "", -1, 0, -1, "", "", false, -1, -1, "", utilities.getServerTime(),
                                                                                                      utilities.ParafaitEnv.LoginID, -1, null, 0, -1, utilities.ParafaitEnv.POSMachine, -1, "", null);
                                    trxPaymentDTO.paymentModeDTO = paymentModeDTOList[0];

                                    transactionFieldInfo = transactionType.GetField("TransactionPaymentsDTOList");
                                    List<TransactionPaymentsDTO> transactionPaymentsDTOList = transactionFieldInfo.GetValue(transaction) as List<TransactionPaymentsDTO>;

                                    Type tableAttributeUIHelper = Type.GetType("Semnox.Parafait.TableAttributeSetupUI.TableAttributesUIHelper, TableAttributeSetupUI");

                                    try
                                    {
                                        bool canSkip = false;
                                        bool readOnly = false;
                                        object parentWindow = null; 
                                            MethodInfo tableAttributeUIHelperInfo = tableAttributeUIHelper.GetMethod("GetEnabledAttributeDataForPaymentMode",
                                                                                    new[] { utilities.ExecutionContext.GetType(), trxPaymentDTO.GetType(),
                                                                                    typeof(bool), typeof(bool), typeof(object) });
                                        trxPaymentDTO = (TransactionPaymentsDTO)tableAttributeUIHelperInfo.Invoke(transactionFieldInfo,
                                                                                       new object[] { utilities.ExecutionContext, trxPaymentDTO, canSkip, readOnly, parentWindow });
                                    }
                                    catch (Exception ex)
                                    {
                                        log.Error(ex);
                                        if (ex.InnerException != null && string.IsNullOrWhiteSpace(ex.InnerException.Message) == false)
                                        {
                                            throw new ValidationException(ex.InnerException.Message);
                                        }
                                        else
                                        {
                                            throw;
                                        }
                                    }
                                    transactionPaymentsDTOList.Add(trxPaymentDTO);
                                }
                                transactionMethodInfo = transactionType.GetMethod("SaveTransacation", new[] { sqlTransaction.GetType(), message.GetType().MakeByRefType() });
                                retValue = transactionMethodInfo.Invoke(transaction, new object[] { sqlTransaction, message });
                            }
                        }
                        else
                        {
                            log.Debug("Registration bonus is not given as the customer is already registered against a card");
                        }
                    }
                    //Ends Modification for registration bonus on verification on 27-Nov-2018

                    if (Convert.ToInt32(retValue) == 0)
                        sqlTransaction.Commit();
                }
                catch (ValidationException ex)
                {
                    sqlTransaction.Rollback();
                    log.Error("validation failed", ex);
                    StringBuilder errorMessageBuilder = new StringBuilder("");
                    foreach (var validationError in ex.ValidationErrorList)
                    {
                        errorMessageBuilder.Append(validationError.Message);
                        errorMessageBuilder.Append(Environment.NewLine);
                    }
                    messageBoxDelegate(errorMessageBuilder.ToString(), MessageContainerList.GetMessage(utilities.ExecutionContext, "Verify Customer"));
                }
                catch (Exception ex)
                {
                    sqlTransaction.Rollback();
                    log.Error("Exception occurred while saving discount", ex);
                    messageBoxDelegate(MessageContainerList.GetMessage(utilities.ExecutionContext, 718), MessageContainerList.GetMessage(utilities.ExecutionContext, "Verify Customer"));
                }
                finally
                {
                    sqlConnection.Close();
                }
            }
            this.Close();
            log.LogMethodExit();
        }

        private void btnVerify_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (string.IsNullOrWhiteSpace(txtVerificationCode.Text))
            {
                messageBoxDelegate(MessageContainerList.GetMessage(utilities.ExecutionContext, 1144, lblVerficationCode.Text.Replace(":", "")), this.Text);
                log.LogMethodExit(null, "Customer verification code is empty");
                return;
            }
            CustomerVerificationListBL customerVerificationListBL = new CustomerVerificationListBL(utilities.ExecutionContext);
            List<KeyValuePair<CustomerVerificationDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CustomerVerificationDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<CustomerVerificationDTO.SearchByParameters, string>(CustomerVerificationDTO.SearchByParameters.VERIFICATION_CODE, txtVerificationCode.Text.Trim()));
            List<CustomerVerificationDTO> customerVerificationDTOList = customerVerificationListBL.GetCustomerVerificationDTOList(searchParameters, true, true);
            txtVerificationCode.BackColor = Color.White;
            lblVerifiedMessage.Visible = false;
            btnOK.Enabled = false;
            bool validVerificationCodeFound = false;
            if (customerVerificationDTOList != null && customerVerificationDTOList.Count > 0)
            {
                foreach (var customerVerificationDTOItem in customerVerificationDTOList)
                {
                    if (customerVerificationDTOItem.CustomerId == -1)
                    {
                        customerVerificationDTO = customerVerificationDTOItem;
                        validVerificationCodeFound = true;
                        break;
                    }
                }
            }
            if (validVerificationCodeFound)
            {
                txtName.Text = customerVerificationDTO.ProfileDTO.FirstName;
                string contactPhone = "";
                string contactEmail = "";

                if (customerVerificationDTO.ProfileDTO.ContactDTOList != null)
                {
                    foreach (var contactDTO in customerVerificationDTO.ProfileDTO.ContactDTOList)
                    {
                        if (contactDTO.ContactType == ContactType.PHONE)
                        {
                            contactPhone = contactDTO.Attribute1;
                        }
                        if (contactDTO.ContactType == ContactType.EMAIL)
                        {
                            contactEmail = contactDTO.Attribute1;
                        }
                    }
                }

                txtPhone.Text = contactPhone;
                txtEmail.Text = contactEmail;
                customerDTO.ProfileDTO = customerVerificationDTO.ProfileDTO;
                btnOK.Enabled = true;
                txtVerificationCode.BackColor = Color.LightGreen;
                lblVerifiedMessage.Visible = true;
            }
            else
            {
                txtVerificationCode.BackColor = Color.Red;
                messageBoxDelegate(MessageContainerList.GetMessage(utilities.ExecutionContext, 1450), MessageContainerList.GetMessage(utilities.ExecutionContext, "Code Verify"));
            }
            log.LogMethodExit();
        }

        private void lnkSendVerificationCode_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                this.Cursor = Cursors.WaitCursor;
                string Code;
                string contactPhone = null;
                string contactEmail = null;
                if (customerDTO.ContactDTOList != null)
                {
                    foreach (var contactDTO in customerDTO.ContactDTOList)
                    {
                        if (contactDTO.ContactType == ContactType.PHONE)
                        {
                            contactPhone = contactDTO.Attribute1;
                        }
                        if (contactDTO.ContactType == ContactType.EMAIL)
                        {
                            contactEmail = contactDTO.Attribute1;
                        }
                    }
                }
                if (!string.IsNullOrEmpty(contactPhone) || !string.IsNullOrEmpty(contactEmail))
                {

                    CustomerVerificationBL customerVerificationBL = new CustomerVerificationBL(utilities.ExecutionContext);
                    customerVerificationBL.GenerateVerificationRecord(customerDTO.Id, utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.POSMachine, true);
                    Code = customerVerificationBL.CustomerVerificationDTO.VerificationCode;
                    //string SMSGateway = utilities.getParafaitDefaults("SMS_GATEWAY");
                    //Code = GenerateVeficationCode();
                    //if (!string.IsNullOrEmpty(contactEmail))
                    //{
                    //    Messaging msg = new Messaging(utilities);
                    //    string body = "Dear " + customerDTO.FirstName + ",";
                    //    body += Environment.NewLine + Environment.NewLine;
                    //    body += "Your registration verification code is " + Code + ".";
                    //    body += Environment.NewLine + Environment.NewLine;
                    //    body += "Thank you";
                    //    body += Environment.NewLine;
                    //    body += utilities.ParafaitEnv.SiteName;

                    //    msg.SendEMailSynchronous(contactEmail, "", utilities.ParafaitEnv.SiteName + " - customer registration verification", body);
                    //}

                    //if (!string.IsNullOrEmpty(contactPhone) && (SMSGateway != SMSGateways.None.ToString()))
                    //{
                    //    string Template = string.Empty;
                    //    List<KeyValuePair<string, string>> paramList = new List<KeyValuePair<string, string>>();
                    //    CustomerVerificationBL customerVerificationBL = new CustomerVerificationBL(utilities.ExecutionContext);
                    //    Template = "Dear @customername, ";
                    //    Template += "Your registration verification code is @code.\n ";
                    //    Template += "Thank you \n @sitename ";

                    //    if(SMSGateway == SMSGateways.Aliyun.ToString())
                    //    {
                    //        //Aliyun Doesn't use the above template therefore we pass only code to the template defined in Aliyun dashboard
                    //        if (Template.ToLower().Contains("@code"))
                    //            paramList.Add(new KeyValuePair<string, string>("@code", Code));
                    //    }                             
                    //    else
                    //    {
                    //        if (Template.ToLower().Contains("@customername"))
                    //            paramList.Add(new KeyValuePair<string, string>("@customername", customerDTO.FirstName));
                    //        if (Template.ToLower().Contains("@code"))
                    //            paramList.Add(new KeyValuePair<string, string>("@code", Code));
                    //        if (Template.ToLower().Contains("@sitename"))
                    //            paramList.Add(new KeyValuePair<string, string>("@sitename", utilities.ParafaitEnv.SiteName));
                    //    }

                    //    SendSMS sendSMS = new SendSMS();
                    //    sendSMS.Initialize(utilities);
                    //    sendSMS.sendSMSSynchronous(contactPhone, Template, paramList);
                    //}
                }
                else
                {
                    messageBoxDelegate(MessageContainerList.GetMessage(utilities.ExecutionContext, 1451), MessageContainerList.GetMessage(utilities.ExecutionContext, "Customer Verification"));
                    log.Error("Both email and phone is empty");
                }
            }
            catch (ValidationException ex)
            {
                log.Error("Error occurred while generating the verification code", ex);
                StringBuilder sb = new StringBuilder();
                foreach (var validationError in ex.ValidationErrorList)
                {
                    sb.Append(validationError.Message);
                    sb.Append(Environment.NewLine);
                }
                messageBoxDelegate(sb.ToString(), MessageContainerList.GetMessage(utilities.ExecutionContext, "Customer Verification"));
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while sending the verification code", ex);
                messageBoxDelegate(ex.Message, MessageContainerList.GetMessage(utilities.ExecutionContext, "Customer Verification"));
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }

        string GenerateVeficationCode()
        {
            log.LogMethodEntry();
            string code = utilities.GenerateRandomNumber(6, Utilities.RandomNumberType.Numeric);
            CustomerVerificationDTO customerVerificationDTO = new CustomerVerificationDTO();
            customerVerificationDTO.Source = "POS:" + utilities.ParafaitEnv.POSMachine;
            customerVerificationDTO.VerificationCode = code;
            customerVerificationDTO.ProfileDTO = customerDTO.ProfileDTO;
            CustomerVerificationBL customerVerificationBL = new CustomerVerificationBL(utilities.ExecutionContext, customerVerificationDTO);
            customerVerificationBL.Save();
            return code;
        }
    }
}
