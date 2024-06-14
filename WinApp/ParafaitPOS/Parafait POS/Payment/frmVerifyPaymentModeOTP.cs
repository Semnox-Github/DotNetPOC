/********************************************************************************************
 * Project Name - Parafait POS
 * Description  - POS application
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By    Remarks          
 *********************************************************************************************
 *2.130.7.0   4-Jul -2019     Guru S A        Created for Payment Mode OTP enhancement
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.User;
using Semnox.Parafait.Communication;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Device.PaymentGateway;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Customer.Accounts;
using System.Text.RegularExpressions;
using System.Data.SqlClient;

namespace Parafait_POS
{
    /// <summary>
    /// frmVerifyPaymentModeOTP class
    /// </summary>
    public partial class frmVerifyPaymentModeOTP : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static List<MessagingClientFunctionLookUpDTO> paymentModeOTPEventMsgClientFunctionDTOList;
        private static bool paymentModeOTPEventMsgClientFunctionLoaded = false;

        private readonly ExecutionContext executionContext;
        private Semnox.Core.Utilities.NumberPad numPad;
        private string newOTPValue = string.Empty;
        private string NUMBERFORMAT = string.Empty;
        private const int RoundingPrecision = 0;
        private Timer resendOTPTimer;
        private int timerTickValue = 300;
        private Dictionary<string, ApprovalAction> otpApprovals = new Dictionary<string, ApprovalAction>();
        private string phoneNumber = string.Empty;
        private string emailId = string.Empty;
        private string gameCardNumber = string.Empty;
        private Transaction transaction;
        private Utilities utilities;
        private bool otpExpired = false;
        private bool allowedToOverride = false;
        private int TIMER_THRESHHOLD = 300;
        public Dictionary<string, ApprovalAction> OtpApprovals { get { return otpApprovals; } }
        public string NewOTPValue { get { return newOTPValue; } }
        public bool AllowedToOverride { get { return allowedToOverride; } }
        /// <summary>
        /// frmVerifyPaymentModeOTP constructor
        /// </summary> 
        public frmVerifyPaymentModeOTP(Utilities utilities, Transaction transaction, string newOTP, string phoneNumber, string emailId, string gameCardNumber)
        {
            log.LogMethodEntry("utilities", "transaction", "newOTP", phoneNumber, emailId, gameCardNumber);
            this.executionContext = utilities.ExecutionContext;
            this.utilities = utilities;
            this.newOTPValue = newOTP;
            this.phoneNumber = phoneNumber;
            this.emailId = emailId;
            this.gameCardNumber = gameCardNumber;
            this.transaction = transaction;
            this.otpExpired = false;
            InitializeComponent();
            NUMBERFORMAT = "N0";
            TIMER_THRESHHOLD = ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "PAYMENT_MODE_OTP_THRESHOLD_TIME", 300);
            timerTickValue = TIMER_THRESHHOLD;
            SetTimerLabelValue();
            resendOTPTimer = new Timer();
            resendOTPTimer.Interval = 1000; //1 second
            resendOTPTimer.Tick += new EventHandler(ResendOTPTimerTick);

            numPad = new NumberPad(NUMBERFORMAT, RoundingPrecision);
            numPad.handleaction("");
            numPad.NewEntry = true;

            Panel NumberPadVarPanel = numPad.NumPadPanel();
            NumberPadVarPanel.Location = new System.Drawing.Point(txtUserEnteredOTP.Location.X, txtUserEnteredOTP.Location.Y);
            this.Controls.Add(NumberPadVarPanel);
            numPad.setReceiveAction = EventnumPadOKReceived;
            numPad.setKeyAction = EventnumPadKeyPressReceived;
            this.KeyPreview = true;
            this.KeyPress += new KeyPressEventHandler(FormNumPad_KeyPress);
            this.FormClosing += new FormClosingEventHandler(FormNumPad_FormClosing);
            utilities.setLanguage(this);
            log.LogMethodExit();
        }
        private void SetTimerLabelValue()
        {
            log.LogMethodEntry();
            var span = new TimeSpan(0, 0, timerTickValue);
            string timerValueString = string.Format("{0}:{1:00}",
                                        (int)span.TotalMinutes,
                                        span.Seconds);
            lblTimeValue.Text = timerValueString;
            log.LogMethodExit();
        }
        private void frmVerifyPaymentModeOTP_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                txtMessage.Clear();
                if (string.IsNullOrWhiteSpace(phoneNumber) == false && string.IsNullOrWhiteSpace(emailId) == false)
                {
                    lblContactMsg.Text = MessageContainerList.GetMessage(executionContext, 4387, phoneNumber, emailId);
                    //"A OTP code has been sent to customer contact &1 and &2";
                }
                else
                {
                    string contactNo = string.IsNullOrWhiteSpace(phoneNumber) ? emailId : phoneNumber;
                    lblContactMsg.Text = MessageContainerList.GetMessage(executionContext, 4388, contactNo);
                    //"A OTP code has been sent to customer contact &1";
                }
                LaunchTimer();
                DisableButtons();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                txtMessage.Text = ex.Message;
            }
            log.LogMethodExit();
        }

        private void LaunchTimer()
        {
            log.LogMethodEntry();
            timerTickValue = TIMER_THRESHHOLD;
            //timerTickValue = 60;
            resendOTPTimer.Start();
            log.LogMethodExit();
        }

        private void ResendOTPTimerTick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                resendOTPTimer.Stop();
                timerTickValue--;
                if (timerTickValue < 0)
                {
                    otpExpired = true;
                    EnableButtons();
                }
                else
                {
                    SetTimerLabelValue();
                    resendOTPTimer.Start();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                resendOTPTimer.Start();
            }
            log.LogMethodExit();
        }
        private void EnableButtons()
        {
            log.LogMethodEntry();
            btnResend.Enabled = true;
            bool allowOverride = ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "PAYMENT_MODE_OTP_OVERRIDE_ALLOWED", false);
            if (allowOverride)
            {
                btnOverrideOTP.Enabled = true;
            }
            log.LogMethodExit();
        }
        private void btnOverrideOTP_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                txtMessage.Clear();
                string msg = MessageContainerList.GetMessage(executionContext, 4389);
                //"Do you want to proceed without entering OTP?"
                string title = MessageContainerList.GetMessage(executionContext, "Override OTP Validation");
                if (POSUtils.ParafaitMessageBox(msg, title, MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    int mgrId = -1;
                    if (!Authenticate.Manager(ref mgrId))
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Manager Approval Required"));
                    }
                    else
                    {
                        UserContainerDTO userContainerDTO = UserContainerList.GetUserContainerDTO(executionContext.GetSiteId(), mgrId);
                        otpApprovals.Add(ApprovalAction.ApprovalActionType.OVERRIDE_PAYMENT_MODE_OTP.ToString(),
                                         new ApprovalAction(userContainerDTO.LoginId, ServerDateTime.Now));
                        this.allowedToOverride = true;
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                txtMessage.Text = ex.Message;
            }
            log.LogMethodExit();
        }

        private void btnResend_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                txtMessage.Clear();
                string msg = MessageContainerList.GetMessage(executionContext, 4390); // "Do you want to resend OTP?"
                string title = MessageContainerList.GetMessage(executionContext, "Resend OTP");
                if (POSUtils.ParafaitMessageBox(msg, title, MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    ResendOTP();
                    DisableButtons();
                    otpExpired = false;
                    timerTickValue = TIMER_THRESHHOLD;
                    resendOTPTimer.Start();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                txtMessage.Text = ex.Message;
            }
            log.LogMethodExit();
        }

        private void ResendOTP()
        {
            log.LogMethodEntry();
            string newNewOTP = SendOTP(executionContext, utilities, transaction, phoneNumber, emailId, gameCardNumber);
            newOTPValue = newNewOTP;
            log.LogMethodExit();
        }

        private void DisableButtons()
        {
            log.LogMethodEntry();
            btnResend.Enabled = false;
            btnOverrideOTP.Enabled = false;
            log.LogMethodExit();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                txtMessage.Clear();
                otpApprovals.Clear();
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                txtMessage.Text = ex.Message;
            }
            log.LogMethodExit();
        }

        private void FormNumPad_FormClosing(object sender, FormClosingEventArgs e)
        {
            log.LogMethodEntry();
            if (this.DialogResult == DialogResult.Cancel)
            {
                otpApprovals.Clear();
            }
            log.LogMethodExit();
        }

        private void FormNumPad_KeyPress(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (e.KeyChar == (char)Keys.Escape)
                {
                    this.DialogResult = DialogResult.Cancel;
                    this.Close();
                }
                else
                {
                    numPad.GetKey(e.KeyChar);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void EventnumPadOKReceived()
        {
            log.LogMethodEntry();
            try
            {
                txtMessage.Clear();
                string receivedText = numPad.ReturnNumberString;
                txtUserEnteredOTP.Text = receivedText;
                ValidOTP(receivedText);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                txtMessage.Text = ex.Message;
                numPad.handleaction("");
            }
            log.LogMethodExit();
        }

        private void ValidOTP(string receivedText)
        {
            log.LogMethodEntry();
            if (otpExpired == true || (otpExpired == false && string.IsNullOrWhiteSpace(receivedText) == true)
               || (otpExpired == false && string.IsNullOrWhiteSpace(receivedText) == false && receivedText != newOTPValue))
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Please enter valid OTP. Resend OTP if time has expired"));
            }
            log.LogMethodExit();
        }

        private void EventnumPadKeyPressReceived()
        {
            log.LogMethodEntry();
            try
            {
                string receivedText = numPad.ReturnNumberString;
                txtUserEnteredOTP.Text = receivedText;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void BlueBtn_MouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                Button selectedButton = (Button)sender;
                selectedButton.BackgroundImage = Properties.Resources.pressed2;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        private void BlueBtn_MouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                Button selectedButton = (Button)sender;
                selectedButton.BackgroundImage = Properties.Resources.normal2;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// SendOTP
        /// </summary> 
        /// <returns></returns>
        public static string SendOTP(ExecutionContext executionContext, Utilities utilities, Transaction transaction, string selectedPhoneNumber, string selectedEmailId, string gameCardNumber)
        {
            log.LogMethodEntry(executionContext, "utilities", "transaction", selectedPhoneNumber, selectedEmailId, gameCardNumber);
            string msg = MessageContainerList.GetMessage(executionContext, "Sending OTP.") + " "+
                          MessageContainerList.GetMessage(executionContext, 684);// "Please wait..."                       
            string newOTPValue = BackgroundProcessRunner.Run<string>(() =>
            {
                return InvokeSendOTP(executionContext, utilities, transaction, selectedPhoneNumber, selectedEmailId, gameCardNumber);
            },
                                                                   msg, BackgroundProcessRunner.LaunchWaitScreenAfterXSeconds);
            log.LogMethodExit("newOTPValue");
            return newOTPValue;
        }

        private static string InvokeSendOTP(ExecutionContext executionContext, Utilities utilities, Transaction transaction, string selectedPhoneNumber, string selectedEmailId, 
                                     string GameCardNumber)
        {
            log.LogMethodEntry(executionContext, "utilities", "transaction", selectedPhoneNumber, selectedEmailId, GameCardNumber);
            string newOTPValue = string.Empty;
            if (string.IsNullOrWhiteSpace(selectedEmailId) == false || string.IsNullOrWhiteSpace(selectedPhoneNumber) == false)
            {
                newOTPValue = utilities.GenerateRandomNumber(6, Utilities.RandomNumberType.Numeric);
                if (string.IsNullOrWhiteSpace(selectedPhoneNumber) == false)
                {
                    TransactionEventContactsDTO transactionEventContactsDTO = new TransactionEventContactsDTO();
                    transactionEventContactsDTO.PhoneNumber = selectedPhoneNumber;
                    transactionEventContactsDTO.MessageChannel = TransactionPaymentLink.MessageChannel.SMS;
                    transactionEventContactsDTO.TransactionId = (transaction != null && transaction.Trx_id > 0) ? transaction.Trx_id : -1;
                    transactionEventContactsDTO.OTPValue = newOTPValue;
                    transactionEventContactsDTO.OTPGameCard = GameCardNumber;
                    TransactionEventsBL transactionEventsBL = new TransactionEventsBL(executionContext, utilities, ParafaitFunctionEvents.PAYMENT_MODE_OTP_EVENT,
                                                                                       transaction, transactionEventContactsDTO);
                    transactionEventsBL.SendMessage(MessagingClientDTO.MessagingChanelType.SMS);
                }
                if (string.IsNullOrWhiteSpace(selectedEmailId) == false)
                {
                    TransactionEventContactsDTO transactionEventContactsDTO = new TransactionEventContactsDTO();
                    transactionEventContactsDTO.EmailId = selectedEmailId;
                    transactionEventContactsDTO.MessageChannel = TransactionPaymentLink.MessageChannel.EMAIL;
                    transactionEventContactsDTO.TransactionId = (transaction != null && transaction.Trx_id > 0) ? transaction.Trx_id : -1;
                    transactionEventContactsDTO.OTPValue = newOTPValue;
                    transactionEventContactsDTO.OTPGameCard = GameCardNumber;
                    TransactionEventsBL transactionEventsBL = new TransactionEventsBL(executionContext, utilities, ParafaitFunctionEvents.PAYMENT_MODE_OTP_EVENT,
                                                                                       transaction, transactionEventContactsDTO);
                    transactionEventsBL.SendMessage(MessagingClientDTO.MessagingChanelType.EMAIL);
                }
            }
            else
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 4391));
                // "Sorry, please provide contact details to proceed"
            }
            log.LogMethodExit("newOTPValue");
            return newOTPValue;
        }

        public static KeyValuePair<string, ApprovalAction> PerformPaymentModeOTPValidation(Utilities utls, PaymentModeDTO paymentModeDTO, Transaction trx,
                                                           DataGridView dgvGameCards)
        {
            log.LogMethodEntry("utls", paymentModeDTO, "trx","dgvGameCards");
            string paymentModeOTPValue = string.Empty;
            KeyValuePair<string, ApprovalAction> keyValuePair = new KeyValuePair<string, ApprovalAction>();
            //this.Cursor = Cursors.WaitCursor;
            if (paymentModeDTO.OTPValidation)
            {
                LoadPaymentModeOTPEventSetup(utls.ExecutionContext);
                List<string> phoneNumberList = GetTrxCustomerPhoneNumber(utls.ExecutionContext, trx, dgvGameCards, paymentModeDTO.IsDebitCard);
                List<string> emailIdList = GetTrxCustomerEmailId(utls.ExecutionContext, trx, dgvGameCards, paymentModeDTO.IsDebitCard);
                string selectedPhoneNumber = string.Empty;
                string selectedEmailId = string.Empty;
                Dictionary<string, bool> clientSetup = GetPaymentModeOTPClientSetup();
                int elementCount = GetElementCount(clientSetup);
                if (elementCount == 0)
                {
                    string eventName = MessageContainerList.GetMessage(utls.ExecutionContext, "Payment Mode OTP");
                    throw new ValidationException(MessageContainerList.GetMessage(utls.ExecutionContext, 4392, eventName));
                    // "Messaging client setup for &1 event is missing"
                }
                GetSelectedContactDetails(utls.ExecutionContext, paymentModeDTO.IsDebitCard, phoneNumberList, emailIdList, clientSetup, elementCount, out selectedPhoneNumber, out selectedEmailId);
                //this.Cursor = Cursors.WaitCursor;
                string gameCardNumber = (dgvGameCards != null && dgvGameCards.Rows.Count > 0 ? dgvGameCards.Rows[0].Cells["CardNumber"].Value.ToString() : null);
                string newOTPValue = SendOTP(utls.ExecutionContext, utls, trx, selectedPhoneNumber, selectedEmailId, gameCardNumber);
                //his.Cursor = Cursors.WaitCursor;
                keyValuePair = ValidateOTP(utls, trx, newOTPValue, selectedPhoneNumber, selectedEmailId, gameCardNumber);
            }
            log.LogMethodExit(keyValuePair, "keyValuePair");
            return keyValuePair;
        }

        private static void LoadPaymentModeOTPEventSetup(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            if (paymentModeOTPEventMsgClientFunctionLoaded == false)
            {
                MessagingClientFunctionLookUpListBL eventMessageClientLookupListBL = new MessagingClientFunctionLookUpListBL(executionContext);
                List<KeyValuePair<MessagingClientFunctionLookUpDTO.SearchByParameters, string>> searchParam = new List<KeyValuePair<MessagingClientFunctionLookUpDTO.SearchByParameters, string>>();
                searchParam.Add(new KeyValuePair<MessagingClientFunctionLookUpDTO.SearchByParameters, string>(MessagingClientFunctionLookUpDTO.SearchByParameters.SITE_ID,
                                                                                                       executionContext.GetSiteId().ToString()));
                searchParam.Add(new KeyValuePair<MessagingClientFunctionLookUpDTO.SearchByParameters, string>(MessagingClientFunctionLookUpDTO.SearchByParameters.IS_ACTIVE, "1"));
                searchParam.Add(new KeyValuePair<MessagingClientFunctionLookUpDTO.SearchByParameters, string>(MessagingClientFunctionLookUpDTO.SearchByParameters.PARAFAIT_FUNCTION_EVENT_NAME,
                                                                                                            ParafaitFunctionEvents.PAYMENT_MODE_OTP_EVENT.ToString()));
                paymentModeOTPEventMsgClientFunctionDTOList = eventMessageClientLookupListBL.GetAllMessagingClientFunctionLookUpList(searchParam);
                paymentModeOTPEventMsgClientFunctionLoaded = true;
            }
            log.LogMethodExit();
        }

        private static List<string> GetTrxCustomerPhoneNumber(ExecutionContext executionContext, Transaction trx, DataGridView dgvGameCards, bool isDebitCard)
        {
            log.LogMethodEntry(executionContext, "trx", "dgvGameCards", isDebitCard);
            List<string> phoneNumberList = new List<string>();
            List<CustomerDTO> customerDTOList;
            string customerIdentifier;
            GetCustomerInfo(executionContext, trx, dgvGameCards, isDebitCard, out customerDTOList, out customerIdentifier);
            phoneNumberList = GetContactList(customerDTOList, customerIdentifier, ContactType.PHONE);
            log.LogMethodExit(phoneNumberList);
            return phoneNumberList;
        }
        private static void GetCustomerInfo(ExecutionContext executionContext, Transaction trx, DataGridView dgvGameCards, bool isDebitCard, out List<CustomerDTO> customerDTOList, out string customerIdentifier)
        {
            log.LogMethodEntry(executionContext, "trx", "dgvGameCards", isDebitCard);
            customerDTOList = new List<CustomerDTO>();
            if (isDebitCard) //get game card customer info
            {
                List<AccountDTO> accountDTOList = GetGameCardCustomreDetails(executionContext, dgvGameCards);
                if (accountDTOList == null || accountDTOList.Any() == false)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 4393));
                    //Game card needs to be linked with a Customer to perform OTP validation
                }
                else
                {
                    List<AccountDTO> accountWithCustList = accountDTOList.Where(act => act.CustomerId != -1).ToList();
                    if (accountWithCustList == null || accountWithCustList.Any() == false)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 4393));
                        //Game card needs to be linked with a Customer to perform OTP validation
                    }
                    else
                    {
                        List<int> custIdList = accountWithCustList.Select(act => act.CustomerId).ToList();
                        CustomerListBL customerListBL = new CustomerListBL(executionContext);
                        customerDTOList = customerListBL.GetCustomerDTOList(custIdList, true);
                        customerIdentifier = string.Empty;
                    }
                }
                log.LogVariableState("Game card customerDTOList", customerDTOList);
                log.LogVariableState("Game card customerIdentifier", customerIdentifier);
            }
            else
            {  // else get trasaction customer info
                CustomerDTO customerDTO = GetTrxCustomerDTO(trx);
                if (customerDTO != null)
                {
                    customerDTOList.Add(customerDTO);
                }
                customerIdentifier = trx.customerIdentifier;
                log.LogVariableState("Trx customerDTOList", customerDTOList);
                log.LogVariableState("Trx customerIdentifier", customerIdentifier);
            }
            log.LogMethodExit();
        }
        private static List<AccountDTO> GetGameCardCustomreDetails(ExecutionContext executionContext, DataGridView dgvGameCards)
        {
            log.LogMethodEntry(executionContext, dgvGameCards);
            List<AccountDTO> accountDTOList = new List<AccountDTO>();
            List<int> cardIdList = new List<int>();
            if (dgvGameCards != null && dgvGameCards.Rows.Count > 0)
            {
                for (int i = 0; i < dgvGameCards.Rows.Count; i++)
                {
                    int cardId = Convert.ToInt32(dgvGameCards.Rows[i].Cells["CardId"].Value);
                    cardIdList.Add(cardId);
                }
                if (cardIdList != null && cardIdList.Any())
                {
                    AccountListBL accountListBL = new AccountListBL(executionContext);
                    accountDTOList = accountListBL.GetAccountDTOList(cardIdList);
                }
            }
            log.LogMethodExit(cardIdList, "cardIdList");
            return accountDTOList;
        }
        private static CustomerDTO GetTrxCustomerDTO(Transaction trx)
        {
            log.LogMethodEntry("trx");
            CustomerDTO customerDTO = ((trx != null && trx.customerDTO != null) ? trx.customerDTO
                                           : ((trx != null && trx.PrimaryCard != null && trx.PrimaryCard.customerDTO != null)
                                                                         ? trx.PrimaryCard.customerDTO : null));
            log.LogMethodExit();
            return customerDTO;
        }
        private static List<string> GetContactList(List<CustomerDTO> customerDTOList, string customerIdentifier, ContactType contactType)
        {
            log.LogMethodEntry("customerDTOList", "customerIdentifier", contactType);
            List<string> contactList = new List<string>();
            if (customerDTOList != null && customerDTOList.Any())
            {
                for (int i = 0; i < customerDTOList.Count; i++)
                {
                    CustomerDTO customerDTO = customerDTOList[i];
                    string contactNo = ((contactType == ContactType.PHONE) ? customerDTO.PhoneNumber
                                                                           : (contactType == ContactType.EMAIL) ? customerDTO.Email : string.Empty);
                    if (!string.IsNullOrWhiteSpace(contactNo))
                    {
                        contactList.Add(contactNo);
                    }

                    if (customerDTO.ContactDTOList != null && customerDTO.ContactDTOList.Any(x => x.ContactType == contactType))
                    {
                        List<ContactDTO> contactDTOList = customerDTO.ContactDTOList.Where(x => x.ContactType == contactType && x.IsActive).OrderByDescending(x => x.LastUpdateDate).ToList();
                        if (contactDTOList != null && contactDTOList.Any())
                        {
                            contactList.AddRange(contactDTOList.Select(pcd => pcd.Attribute1).ToList());
                        }
                    }
                }
            }
            else if (!string.IsNullOrEmpty(customerIdentifier))
            {
                string decryptedCustomerReference = Encryption.Decrypt(customerIdentifier);
                string[] customerIdentifierStringArray = decryptedCustomerReference.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < customerIdentifierStringArray.Length; i++)
                {
                    if (contactType == ContactType.PHONE && Regex.IsMatch(customerIdentifierStringArray[i], @"^\d+$"))
                    {
                        contactList.Add(customerIdentifierStringArray[i]);
                    }
                    else if (contactType == ContactType.EMAIL && Regex.IsMatch(customerIdentifierStringArray[i], @"^((([\w]+\.[\w]+)+)|([\w]+))@(([\w]+\.)+)([A-Za-z]{1,3})$"))
                    {
                        contactList.Add(customerIdentifierStringArray[i]);
                    }
                }
            }
            if (contactList != null && contactList.Count > 1)
            {
                contactList = contactList.Distinct().ToList();
            }
            log.LogMethodExit();
            return contactList;
        }
        private static List<string> GetTrxCustomerEmailId(ExecutionContext executionContext, Transaction trx, DataGridView dgvGameCards, bool isDebitCard)
        {
            log.LogMethodEntry(executionContext, "trx", "dgvGameCards", isDebitCard);
            List<string> emailIdList = new List<string>();
            List<CustomerDTO> customerDTOList;
            string customerIdentifier;
            GetCustomerInfo(executionContext, trx, dgvGameCards, isDebitCard, out customerDTOList, out customerIdentifier);
            emailIdList = GetContactList(customerDTOList, customerIdentifier, ContactType.EMAIL);
            log.LogMethodExit(emailIdList);
            return emailIdList;
        }
        private static Dictionary<string, bool> GetPaymentModeOTPClientSetup()
        {
            log.LogMethodEntry();
            Dictionary<string, bool> clientSetupDictonary = new Dictionary<string, bool>();
            bool phoneClientFOund = false;
            bool emailClientFOund = false;
            if (paymentModeOTPEventMsgClientFunctionDTOList != null)
            {
                for (int i = 0; i < paymentModeOTPEventMsgClientFunctionDTOList.Count; i++)
                {
                    if (MessagingClientDTO.SourceEnumFromString(paymentModeOTPEventMsgClientFunctionDTOList[i].MessageType) == MessagingClientDTO.MessagingChanelType.EMAIL)
                    {
                        emailClientFOund = true;
                    }
                    if (MessagingClientDTO.SourceEnumFromString(paymentModeOTPEventMsgClientFunctionDTOList[i].MessageType) == MessagingClientDTO.MessagingChanelType.SMS)
                    {
                        phoneClientFOund = true;
                    }
                }
            }
            clientSetupDictonary.Add(MessagingClientDTO.MessagingChanelType.EMAIL.ToString(), emailClientFOund);
            clientSetupDictonary.Add(MessagingClientDTO.MessagingChanelType.SMS.ToString(), phoneClientFOund);
            log.LogMethodExit(clientSetupDictonary);
            return clientSetupDictonary;
        }

        private static int GetElementCount(Dictionary<string, bool> clientSetup)
        {
            log.LogMethodEntry();
            int elementCount = 0;
            if (clientSetup != null)
            {
                foreach (var item in clientSetup)
                {
                    if (item.Value)
                    {
                        elementCount = elementCount + 1;
                    }
                }
            }
            log.LogMethodExit(elementCount);
            return elementCount;
        }
        private static void GetSelectedContactDetails(ExecutionContext executionContext, bool isDebitCard, List<string> phoneNumberList, List<string> emailIdList, Dictionary<string,
            bool> clientSetup, int elementCount, out string selectedPhoneNumber, out string selectedEmailId)
        {
            log.LogMethodEntry();
            selectedPhoneNumber = string.Empty;
            selectedEmailId = string.Empty;
            bool mandatory = false;
            bool readOnly = true;

            if (phoneNumberList != null && phoneNumberList.Count == 0 && emailIdList != null && emailIdList.Count == 0)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 4396));
                // "Sorry, please provide contact details to proceed"
            }
            using (GenericDataEntry genericDataEntry = new GenericDataEntry(elementCount))
            {
                bool contactIsLoaded = false;
                int valueIndex = 0;
                string emailDataTag = MessagingClientDTO.MessagingChanelType.EMAIL.ToString();
                string smsDataTag = MessagingClientDTO.MessagingChanelType.SMS.ToString();
                if (clientSetup[MessagingClientDTO.MessagingChanelType.SMS.ToString()] == true)
                {
                    int recordCount = 0;
                    string labelMsg = MessageContainerList.GetMessage(executionContext, "Phone Number");
                    string dataTag = MessagingClientDTO.MessagingChanelType.SMS.ToString();
                    contactIsLoaded = (phoneNumberList != null && phoneNumberList.Any());
                    if (elementCount == 1)
                    {
                        ThrowNoContactError(executionContext, phoneNumberList, isDebitCard);
                    }
                    readOnly = (phoneNumberList == null || phoneNumberList.Count <2) ? true : false;
                    SetDataEntryElement(genericDataEntry, labelMsg, valueIndex, recordCount, phoneNumberList, mandatory, readOnly, smsDataTag);
                }
                readOnly = true;
                if (clientSetup[MessagingClientDTO.MessagingChanelType.EMAIL.ToString()] == true)
                {
                    int recordCount = (elementCount == 2 ? 1 : 0);
                    string labelMsg = MessageContainerList.GetMessage(executionContext, "Email Id");
                    contactIsLoaded = (emailIdList != null && emailIdList.Any());
                    if (elementCount == 1)
                    {
                        ThrowNoContactError(executionContext, emailIdList, isDebitCard);
                    }
                    readOnly = (emailIdList == null || emailIdList.Count < 2) ? true : false;
                    SetDataEntryElement(genericDataEntry, labelMsg, valueIndex, recordCount, emailIdList, mandatory, readOnly, emailDataTag);
                }

                bool doNowShowUI = false;
                doNowShowUI = NeedToShowUI(phoneNumberList, emailIdList, clientSetup);

                string formTitleMsg = (contactIsLoaded ? MessageContainerList.GetMessage(executionContext, 4394) //OTP will be sent to selected contact(s)
                                                       : MessageContainerList.GetMessage(executionContext, 4395));//"Please provide contact details to send the OTP"
                genericDataEntry.Text = formTitleMsg;
                if (doNowShowUI == true || genericDataEntry.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    for (int i = 0; i < elementCount; i++)
                    {
                        if (genericDataEntry.DataEntryObjects[i].tagValue != null
                            && genericDataEntry.DataEntryObjects[i].tagValue.ToString() == smsDataTag)
                        {
                            selectedPhoneNumber = genericDataEntry.DataEntryObjects[i].data;
                        }
                        if (genericDataEntry.DataEntryObjects[i].tagValue != null
                            && genericDataEntry.DataEntryObjects[i].tagValue.ToString() == emailDataTag)
                        {
                            selectedEmailId = genericDataEntry.DataEntryObjects[i].data;
                        }
                    }
                    if (string.IsNullOrWhiteSpace(selectedEmailId) == false || string.IsNullOrWhiteSpace(selectedPhoneNumber) == false)
                    {
                        log.Info("User has selected contacts");
                    }
                    else
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 4396));
                        // "Sorry, please provide contact details to proceed"
                    }
                }
                else
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 4396));
                    // "Sorry, please provide contact details to proceed"
                }
            }
            log.LogMethodExit();
        }

        private static bool NeedToShowUI(List<string> phoneNumberList, List<string> emailIdList, Dictionary<string, bool> clientSetup)
        {
            log.LogMethodEntry(phoneNumberList, emailIdList, clientSetup);
            bool doNowShowUI = false;
            if (clientSetup[MessagingClientDTO.MessagingChanelType.SMS.ToString()] && phoneNumberList != null && phoneNumberList.Count == 1 &&
                                clientSetup[MessagingClientDTO.MessagingChanelType.EMAIL.ToString()] && emailIdList != null && emailIdList.Count == 1)
            {
                doNowShowUI = true;
            }
            else if (clientSetup[MessagingClientDTO.MessagingChanelType.SMS.ToString()] && phoneNumberList != null && phoneNumberList.Count == 1
                     && clientSetup[MessagingClientDTO.MessagingChanelType.EMAIL.ToString()] == false)
            {
                doNowShowUI = true;
            }
            else if (clientSetup[MessagingClientDTO.MessagingChanelType.SMS.ToString()] == false &&
                     clientSetup[MessagingClientDTO.MessagingChanelType.EMAIL.ToString()] && emailIdList != null && emailIdList.Count == 1)
            {
                doNowShowUI = true;
            }
            log.LogMethodExit(doNowShowUI);
            return doNowShowUI;
        }

        private static void ThrowNoContactError(ExecutionContext executionContext, List<string> contactList, bool isDebitCard)
        {
            log.LogMethodEntry(executionContext, contactList, isDebitCard);
            if (contactList == null || contactList.Any() == false)
            {
                if (isDebitCard)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 4393));
                    // 'Game card needs to be linked with a Customer to perform OTP validation'
                }
                else
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 4397));
                    //Sorry, Transaction needs to have customer details to perform OTP validation
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// SetDataEntryElement
        /// </summary> 
        public static void SetDataEntryElement(GenericDataEntry genericDataEntry, string labelMsg, int valueIndex, int recordCount, List<string> dataListSource, bool mandatory,
                                              bool readOnly, string dataTag = null, int maxLength = -1)
        {
            log.LogMethodEntry(labelMsg, valueIndex, recordCount, dataListSource, mandatory, readOnly, dataTag, maxLength);

            genericDataEntry.DataEntryObjects[recordCount].mandatory = mandatory;
            genericDataEntry.DataEntryObjects[recordCount].label = labelMsg;
            genericDataEntry.DataEntryObjects[recordCount].tagValue = dataTag;
            if (maxLength != -1)
            {
                genericDataEntry.DataEntryObjects[recordCount].maxlength = maxLength;
            }
            int valueCount = (dataListSource != null ? dataListSource.Count : 0);
            if (valueCount > 0)
            {
                genericDataEntry.DataEntryObjects[recordCount].dataType = GenericDataEntry.DataTypes.StringList;
                genericDataEntry.DataEntryObjects[recordCount].listDataSource = GenericDataEntry.GenerateListDataSource(dataListSource);
                genericDataEntry.DataEntryObjects[recordCount].data = dataListSource[valueIndex];
            }
            else
            {
                genericDataEntry.DataEntryObjects[recordCount].dataType = GenericDataEntry.DataTypes.String;
            }
            genericDataEntry.DataEntryObjects[recordCount].readOnly = readOnly;

            log.LogMethodExit();
        }

        private static KeyValuePair<string, ApprovalAction> ValidateOTP(Utilities utls, Transaction trx, string paymentModeOTPValue, string selectedPhoneNumber, string selectedEmailId, 
                                                                          string gameCardNumber)
        {
            log.LogMethodEntry("utls", "trx", "paymentModeOTPValue", selectedPhoneNumber, selectedEmailId, gameCardNumber);
            string validatedOTP = string.Empty;
            KeyValuePair<string, ApprovalAction> paymentModeOTPApprovals = new KeyValuePair<string, ApprovalAction>();
            //paymentModeOTPApprovals = new Dictionary<string, ApprovalAction>();
            using (frmVerifyPaymentModeOTP frmVerifyPaymentModeOTP = new frmVerifyPaymentModeOTP(utls, trx, paymentModeOTPValue, selectedPhoneNumber, selectedEmailId, gameCardNumber))
            {
                if (frmVerifyPaymentModeOTP.ShowDialog() == DialogResult.OK)
                {
                    validatedOTP = frmVerifyPaymentModeOTP.NewOTPValue;
                    if (frmVerifyPaymentModeOTP.AllowedToOverride)
                    {
                        Dictionary<string, ApprovalAction> valuePair = frmVerifyPaymentModeOTP.OtpApprovals;
                        foreach (var item in valuePair)
                        {
                            ApprovalAction approvalAction = new ApprovalAction(item.Value.ApproverId, item.Value.ApprovalTime);
                            // string keyValue = validatedOTP + "-" + DateTime.Now.ToString("yyyyMMddHHmmss");
                            paymentModeOTPApprovals = new KeyValuePair<string, ApprovalAction>(validatedOTP, approvalAction);
                            break;
                        }
                    }
                    else
                    {
                        paymentModeOTPApprovals = new KeyValuePair<string, ApprovalAction>(validatedOTP, null);
                    }
                }
                else
                {
                    throw new ValidationException(MessageContainerList.GetMessage(utls.ExecutionContext, 4398));
                    //"Sorry, OTP validation is required to proceed"
                }
            }
            log.LogMethodExit("paymentModeOTPApprovals");
            return paymentModeOTPApprovals;
        }

        public static void CreateTrxUsrLogEntryForPaymentOTPValidationOverride(Dictionary<string, ApprovalAction> paymentModeOTPApprovals, Transaction trx, string loginId, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(paymentModeOTPApprovals, (trx != null? trx.Trx_id: -1), loginId, sqlTrx);
            if (paymentModeOTPApprovals != null && paymentModeOTPApprovals.Count() > 0 && trx.Trx_id > 0)
            {
                foreach (var item in paymentModeOTPApprovals)
                {
                    string[] otpValue = item.Key.Split('-');//Utilities.ParafaitEnv
                    trx.InsertTrxLogs(trx.Trx_id, -1, loginId, "OVERRIDE PAYMENT MODE OTP",
                        "Staff overrides payment mode OTP ( " + otpValue[0] + " )", sqlTrx, item.Value.ApproverId, item.Value.ApprovalTime);
                }
                paymentModeOTPApprovals = new Dictionary<string, ApprovalAction>();
            }
            log.LogMethodExit();
        }
    }
}
