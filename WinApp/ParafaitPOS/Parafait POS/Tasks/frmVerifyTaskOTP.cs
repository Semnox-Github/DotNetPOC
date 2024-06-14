/********************************************************************************************
 * Project Name - Parafait POS
 * Description  - POS application
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By    Remarks          
 *********************************************************************************************
 *2.150.3     29-Apr-2024     Prajwal S      Created for OTP enhancement for Transfer Card
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
using Semnox.Parafait.Customer;
using Semnox.Parafait.Customer.Accounts;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Semnox.Parafait.ViewContainer;
using System.Runtime.ExceptionServices;
using Semnox.Parafait.Product;

namespace Parafait_POS
{
    /// <summary>
    /// frmVerifygenericOTP class
    /// </summary>
    public partial class frmVerifyTaskOTP : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static List<MessagingClientFunctionLookUpDTO> genericOTPEventMsgClientFunctionDTOList;
        private static bool genericOTPEventMsgClientFunctionLoaded = false;

        private readonly ExecutionContext executionContext;
        private Semnox.Core.Utilities.NumberPad numPad;
        private GenericOTPDTO newOTPValue;
        private TaskTypesContainerDTO taskTypesContainerDTO;
        private string NUMBERFORMAT = string.Empty;
        private const int RoundingPrecision = 0;
        private Timer resendOTPTimer;
        private int timerTickValue = 300;
        private Dictionary<string, ApprovalAction> otpApprovals = new Dictionary<string, ApprovalAction>();
        private string phoneNumber = string.Empty;
        private string emailId = string.Empty;
        private string countryCode = string.Empty;
        private Utilities utilities;
        private bool otpExpired = false;
        private bool allowedToOverride = false;
        private int genericOTPThresholdTimeinMin = 300;
        public Dictionary<string, ApprovalAction> OtpApprovals { get { return otpApprovals; } }
        public GenericOTPDTO NewOTPValue { get { return newOTPValue; } }
        public bool AllowedToOverride { get { return allowedToOverride; } }
        private GenericOTPDTO generatedGenericOTPDTO;
        /// <summary>
        /// frmVerifygenericOTP constructor
        /// </summary> 
        public frmVerifyTaskOTP(Utilities utilities, TaskTypesContainerDTO taskTypesContainerDTO, GenericOTPDTO genericOTPValue, string phoneNumber, string emailId, string countryCode)
        {
            log.LogMethodEntry("utilities", phoneNumber, emailId);
            this.executionContext = utilities.ExecutionContext;
            this.utilities = utilities;
            this.phoneNumber = phoneNumber;
            this.newOTPValue = genericOTPValue;
            this.emailId = emailId;
            this.countryCode = countryCode;
            this.taskTypesContainerDTO = taskTypesContainerDTO;
            this.otpExpired = false;
            InitializeComponent();
            NUMBERFORMAT = "";
            GetOTPSetUp();
            timerTickValue = genericOTPThresholdTimeinMin;
            SetTimerLabelValue();
            EnableOverrideOTPButton();
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
        private void frmVerifyGenericOTP_Load(object sender, EventArgs e)
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
            timerTickValue = genericOTPThresholdTimeinMin;
            //timerTickValue = 60;
            resendOTPTimer.Start();
            log.LogMethodExit();
        }

        private void GetOTPSetUp()
        {
            log.LogMethodEntry();
            genericOTPThresholdTimeinMin = 300;
            try
            {
                //log.Debug("Getting overridden values");
                // Check if the defaults have been overridden for specific sources
                LookupsContainerDTO lookupsContainerDTO = LookupsViewContainerList.GetLookupsContainerDTO(executionContext.GetSiteId(), "GENERIC_OTP");
                if (lookupsContainerDTO != null && lookupsContainerDTO.LookupValuesContainerDTOList != null)
                {
                    string descriptionValue = "";
                    foreach (LookupValuesContainerDTO lookupValuesContainerDTO in lookupsContainerDTO.LookupValuesContainerDTOList)
                    {
                        if (lookupValuesContainerDTO.LookupValue.Equals("TRANSFER_CARD_OTP_EVENT", StringComparison.InvariantCultureIgnoreCase))
                        {
                            descriptionValue = lookupValuesContainerDTO.Description;
                            break;
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(descriptionValue) && descriptionValue.Contains("|"))
                    {
                        string[] values = descriptionValue.Split('|');
                        foreach (var lookup in values)
                        {
                            string[] innervalues = lookup.Split(':');
                            if (innervalues[0].Equals("EXP", StringComparison.InvariantCultureIgnoreCase))
                            {
                                genericOTPThresholdTimeinMin = int.Parse(innervalues[1]);
                                genericOTPThresholdTimeinMin = genericOTPThresholdTimeinMin * 60;
                                break;
                                //log.Debug("Overridden value for expiry minutes " + otpValidatityMins);
                            }
                            //else if (innervalues[0].Equals("LEN", StringComparison.InvariantCultureIgnoreCase))
                            //{
                            //    otpLength = int.Parse(innervalues[1]);
                            //    log.Debug("Overridden value for otp length " + otpLength);
                            //}
                            //else if (innervalues[0].Equals("ATTEMPTS", StringComparison.InvariantCultureIgnoreCase))
                            //{
                            //    numberOfAttempts = int.Parse(innervalues[1]);
                            //    log.Debug("Overridden value for attempts " + numberOfAttempts);
                            //}
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Caught an error while getting defaults for OTP generation " + ex);
            }
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
                    txtMessage.Text = MessageContainerList.GetMessage(executionContext, 5609);
                    EnableResendOTPButton();

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
        private void EnableResendOTPButton()
        {
            log.LogMethodEntry();
            btnResend.Enabled = true;
            log.LogMethodExit();
        }

        private void EnableOverrideOTPButton()
        {
            log.LogMethodEntry();
            btnOverrideOTP.Enabled = false;
            bool allowOverride = taskTypesContainerDTO.EnableOverrideOTP;
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
                        otpApprovals.Add(ApprovalAction.ApprovalActionType.OVERRIDE_TRANSFER_CARD_OTP.ToString(),
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
                    timerTickValue = genericOTPThresholdTimeinMin;
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
            GenericOTPDTO newNewOTP = SendOTP(executionContext, utilities, phoneNumber, emailId, countryCode);
            newOTPValue = newNewOTP;
            log.LogMethodExit();
        }

        private void DisableButtons()
        {
            log.LogMethodEntry();
            btnResend.Enabled = false;
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
            if (otpExpired == true || (otpExpired == false && string.IsNullOrWhiteSpace(receivedText) == true))
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 4824));
            }
            else
            {
                newOTPValue.Code = receivedText;
                try
                {
                    //generic otp usecase
                    IGenericOTPUseCases genericOTPUseCases = GenericOTPUseCaseFactory.GetGenericOTPUseCases(executionContext);
                    using (NoSynchronizationContextScope.Enter())
                    {
                        Task task = genericOTPUseCases.ValidateGenericOTP(newOTPValue);
                        task.Wait();
                    }
                }
                catch(Exception ex)
                {
                    ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                }
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
        public static GenericOTPDTO SendOTP(ExecutionContext executionContext, Utilities utilities, string selectedPhoneNumber, string selectedEmailId, string selectedCountryCode)
        {
            log.LogMethodEntry(executionContext, "utilities", "transaction", selectedPhoneNumber, selectedEmailId);
            string msg = MessageContainerList.GetMessage(executionContext, "Sending OTP.") + " " +
                          MessageContainerList.GetMessage(executionContext, 684);// "Please wait..."                       
            GenericOTPDTO newOTPValue = BackgroundProcessRunner.Run<GenericOTPDTO>(() =>
            {
                return InvokeSendOTP(executionContext, utilities, selectedPhoneNumber, selectedEmailId, selectedCountryCode);
            },
                                                                   msg, BackgroundProcessRunner.LaunchWaitScreenAfterXSeconds);
            log.LogMethodExit("newOTPValue");
            return newOTPValue;
        }

        private static GenericOTPDTO InvokeSendOTP(ExecutionContext executionContext, Utilities utilities, string selectedPhoneNumber, string selectedEmailId, string selectedCountryCode)
        {
            log.LogMethodEntry(executionContext, "utilities", "transaction", selectedPhoneNumber, selectedEmailId);
            GenericOTPDTO result = null;
            if (string.IsNullOrWhiteSpace(selectedEmailId) == false || string.IsNullOrWhiteSpace(selectedPhoneNumber) == false)
            {
                try
                {
                    //generic otp usecase
                    IGenericOTPUseCases genericOTPUseCases = GenericOTPUseCaseFactory.GetGenericOTPUseCases(executionContext);
                    GenericOTPDTO genericOTPDTO = new GenericOTPDTO(-1, string.Empty, selectedPhoneNumber, selectedCountryCode, selectedEmailId, "TRANSFER_CARD_OTP_EVENT", false, 0, DateTime.MinValue, true);
                    using (NoSynchronizationContextScope.Enter())
                    {
                        Task<GenericOTPDTO> task = genericOTPUseCases.GenerateGenericOTP(genericOTPDTO);
                        task.Wait();
                        result = task.Result;
                    }
                }
                catch (AggregateException ex)
                {
                    ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                }
            }
            else
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 4391));
                // "Sorry, please provide contact details to proceed"
            }
            log.LogMethodExit("newOTPValue");
            return result;
        }

        public static KeyValuePair<GenericOTPDTO, ApprovalAction> PerformGenericOTPValidation(Utilities utls, TaskTypesContainerDTO taskTypesContainerDTO, int cardId)
        {
            log.LogMethodEntry("utls", taskTypesContainerDTO, "trx", "dgvGameCards");
            string genericOTPValue = string.Empty;
            KeyValuePair<GenericOTPDTO, ApprovalAction> keyValuePair = new KeyValuePair<GenericOTPDTO, ApprovalAction>();
            //this.Cursor = Cursors.WaitCursor;
            if (taskTypesContainerDTO.EnableOTPValidation)
            {
                LoadGenericOTPEventSetup(utls.ExecutionContext);

                string selectedPhoneNumber = string.Empty;
                string selectedEmailId = string.Empty;
                string countryCode = string.Empty;
                selectedPhoneNumber = GetCardCustomerPhoneNumber(utls.ExecutionContext, cardId, out countryCode);
                selectedEmailId = GetCardCustomerEmailId(utls.ExecutionContext, cardId);
                Dictionary<string, bool> clientSetup = GetGenericOTPClientSetup();
                int elementCount = GetElementCount(clientSetup);
                if (elementCount == 0)
                {
                    string eventName = MessageContainerList.GetMessage(utls.ExecutionContext, "Transfer Card OTP");
                    throw new ValidationException(MessageContainerList.GetMessage(utls.ExecutionContext, 4392, eventName));
                    // "Messaging client setup for &1 event is missing"
                }
                ValidateClientSetup(utls.ExecutionContext, clientSetup, selectedPhoneNumber, selectedEmailId);
                //this.Cursor = Cursors.WaitCursor;
                GenericOTPDTO newOTPValue = SendOTP(utls.ExecutionContext, utls, selectedPhoneNumber, selectedEmailId, countryCode);
                //his.Cursor = Cursors.WaitCursor;
                keyValuePair = ValidateOTP(utls, taskTypesContainerDTO, newOTPValue, selectedPhoneNumber, selectedEmailId, countryCode);
            }
            log.LogMethodExit(keyValuePair, "keyValuePair");
            return keyValuePair;
        }

        private static void LoadGenericOTPEventSetup(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            if (genericOTPEventMsgClientFunctionLoaded == false)
            {
                MessagingClientFunctionLookUpListBL eventMessageClientLookupListBL = new MessagingClientFunctionLookUpListBL(executionContext);
                List<KeyValuePair<MessagingClientFunctionLookUpDTO.SearchByParameters, string>> searchParam = new List<KeyValuePair<MessagingClientFunctionLookUpDTO.SearchByParameters, string>>();
                searchParam.Add(new KeyValuePair<MessagingClientFunctionLookUpDTO.SearchByParameters, string>(MessagingClientFunctionLookUpDTO.SearchByParameters.SITE_ID,
                                                                                                       executionContext.GetSiteId().ToString()));
                searchParam.Add(new KeyValuePair<MessagingClientFunctionLookUpDTO.SearchByParameters, string>(MessagingClientFunctionLookUpDTO.SearchByParameters.IS_ACTIVE, "1"));
                searchParam.Add(new KeyValuePair<MessagingClientFunctionLookUpDTO.SearchByParameters, string>(MessagingClientFunctionLookUpDTO.SearchByParameters.PARAFAIT_FUNCTION_EVENT_NAME,
                                                                                                            ParafaitFunctionEvents.TRANSFER_CARD_OTP_EVENT.ToString()));
                genericOTPEventMsgClientFunctionDTOList = eventMessageClientLookupListBL.GetAllMessagingClientFunctionLookUpList(searchParam);
                genericOTPEventMsgClientFunctionLoaded = true;
            }
            log.LogMethodExit();
        }

        private static string GetCardCustomerPhoneNumber(ExecutionContext executionContext, int cardId, out string countryCode)
        {
            log.LogMethodEntry(executionContext, cardId);
            string phoneNumber = string.Empty;
            CustomerDTO customerDTO = null;
            GetCustomerInfo(executionContext, cardId, out customerDTO);
            phoneNumber = GetContactList(customerDTO, ContactType.PHONE, out countryCode);
            log.LogMethodExit(phoneNumber);
            return phoneNumber;
        }
        private static void GetCustomerInfo(ExecutionContext executionContext, int cardId, out CustomerDTO customerDTO)
        {
            log.LogMethodEntry(executionContext, cardId);
            customerDTO = null;
            AccountDTO accountDTO = GetGameCardCustomreDetails(executionContext, cardId);
            if (accountDTO == null || accountDTO.AccountId < 0)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 4393));
                //Game card needs to be linked with a Customer to perform OTP validation
            }
            else
            {
                if (accountDTO.CustomerId < 0)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 4393));
                    //Game card needs to be linked with a Customer to perform OTP validation
                }
                else
                {
                    CustomerBL customerBL = new CustomerBL(executionContext, accountDTO.CustomerId, true, true);
                    customerDTO = customerBL.CustomerDTO;
                }
            }
            log.LogVariableState("Game card customerDTOList", customerDTO);
            log.LogMethodExit();
        }
        private static AccountDTO GetGameCardCustomreDetails(ExecutionContext executionContext, int cardId)
        {
            log.LogMethodEntry(executionContext, cardId);
            AccountDTO accountDTO = null;
            if (cardId > -1)
            {

                AccountBL accountBL = new AccountBL(executionContext, cardId, true, true);
                accountDTO = accountBL.AccountDTO;
            }
            log.LogMethodExit(cardId, "cardIdList");
            return accountDTO;
        }

        private static string GetContactList(CustomerDTO customerDTO, ContactType contactType, out string countryCode)
        {
            log.LogMethodEntry("customerDTOList", "customerIdentifier", contactType);
            string contact = string.Empty;
            countryCode = string.Empty;
            if (customerDTO != null)
            {
                if (contactType == ContactType.PHONE)
                {
                    ContactDTO contactDTO = customerDTO.PhoneContactDTO;
                    if (contactDTO != null)
                    {
                        if (contactDTO.CountryId > -1)
                        {
                            countryCode = CountryContainerList.GetCountryCode(contactDTO.SiteId, contactDTO.CountryId);
                        }
                        contact = contactDTO.Attribute1;
                    }
                }
                else if (contactType == ContactType.EMAIL)
                {
                    contact = customerDTO.Email;
                }
                else
                {
                    contact = string.Empty;
                }
            }
            log.LogMethodExit();
            return contact;
        }
        private static string GetCardCustomerEmailId(ExecutionContext executionContext, int cardId)
        {
            log.LogMethodEntry(executionContext, cardId);
            string emailId = string.Empty;
            string countryCode = string.Empty;
            CustomerDTO customerDTO;
            GetCustomerInfo(executionContext, cardId, out customerDTO);
            emailId = GetContactList(customerDTO, ContactType.EMAIL, out countryCode);
            log.LogMethodExit(emailId);
            return emailId;
        }
        private static Dictionary<string, bool> GetGenericOTPClientSetup()
        {
            log.LogMethodEntry();
            Dictionary<string, bool> clientSetupDictonary = new Dictionary<string, bool>();
            bool phoneClientFOund = false;
            bool emailClientFOund = false;
            if (genericOTPEventMsgClientFunctionDTOList != null)
            {
                for (int i = 0; i < genericOTPEventMsgClientFunctionDTOList.Count; i++)
                {
                    if (MessagingClientDTO.SourceEnumFromString(genericOTPEventMsgClientFunctionDTOList[i].MessageType) == MessagingClientDTO.MessagingChanelType.EMAIL)
                    {
                        emailClientFOund = true;
                    }
                    if (MessagingClientDTO.SourceEnumFromString(genericOTPEventMsgClientFunctionDTOList[i].MessageType) == MessagingClientDTO.MessagingChanelType.SMS)
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
        private static void ValidateClientSetup(ExecutionContext executionContext, Dictionary<string, bool> clientSetup, string selectedPhoneNumber, string selectedEmailId)
        {
            log.LogMethodEntry();
            if (string.IsNullOrEmpty(selectedPhoneNumber) && string.IsNullOrEmpty(selectedEmailId))
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 4393));
                // "Sorry, please provide contact details to proceed"
            }
            if (clientSetup[MessagingClientDTO.MessagingChanelType.SMS.ToString()] == true)
            {
                if (string.IsNullOrEmpty(selectedPhoneNumber) && clientSetup[MessagingClientDTO.MessagingChanelType.EMAIL.ToString()] == false)
                {
                    ThrowNoContactError(executionContext, selectedPhoneNumber);
                }
            }
            if (clientSetup[MessagingClientDTO.MessagingChanelType.EMAIL.ToString()] == true)
            {
                if (string.IsNullOrEmpty(selectedEmailId) && clientSetup[MessagingClientDTO.MessagingChanelType.SMS.ToString()] == false)
                {
                    ThrowNoContactError(executionContext, selectedEmailId);
                }
            }
            log.LogMethodExit();
        }

        private static void ThrowNoContactError(ExecutionContext executionContext, string contact)
        {
            log.LogMethodEntry(executionContext, contact);
            if (string.IsNullOrEmpty(contact))
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 4393));
                // 'Game card needs to be linked with a Customer to perform OTP validation'
            }
            log.LogMethodExit();
        }

        private static KeyValuePair<GenericOTPDTO, ApprovalAction> ValidateOTP(Utilities utls, TaskTypesContainerDTO taskTypesContainerDTO, GenericOTPDTO genericOTPValue, string selectedPhoneNumber, string selectedEmailId, string countryCode)
        {
            log.LogMethodEntry("genericOTPValue", "selectedPhoneNumber", "selectedEmailId", countryCode);
            GenericOTPDTO validatedOTP;
            KeyValuePair<GenericOTPDTO, ApprovalAction> genericOTPApprovals = new KeyValuePair<GenericOTPDTO, ApprovalAction>();
            //genericOTPApprovals = new Dictionary<string, ApprovalAction>();
            using (frmVerifyTaskOTP frmVerifygenericOTP = new frmVerifyTaskOTP(utls, taskTypesContainerDTO, genericOTPValue, selectedPhoneNumber, selectedEmailId, countryCode))
            {
                if (frmVerifygenericOTP.ShowDialog() == DialogResult.OK)
                {
                    validatedOTP = frmVerifygenericOTP.NewOTPValue;
                    if (frmVerifygenericOTP.AllowedToOverride)
                    {
                        Dictionary<string, ApprovalAction> valuePair = frmVerifygenericOTP.OtpApprovals;
                        foreach (var item in valuePair)
                        {
                            ApprovalAction approvalAction = new ApprovalAction(item.Value.ApproverId, item.Value.ApprovalTime);
                            // string keyValue = validatedOTP + "-" + DateTime.Now.ToString("yyyyMMddHHmmss");
                            genericOTPApprovals = new KeyValuePair<GenericOTPDTO, ApprovalAction>(validatedOTP, approvalAction);
                            break;
                        }
                    }
                    else
                    {
                        genericOTPApprovals = new KeyValuePair<GenericOTPDTO, ApprovalAction>(validatedOTP, null);
                    }
                }
                else
                {
                    throw new ValidationException(MessageContainerList.GetMessage(utls.ExecutionContext, 4398));
                    //"Sorry, OTP validation is required to proceed"
                }
            }
            log.LogMethodExit("genericOTPApprovals");
            return genericOTPApprovals;
        }

        public static void CreateTrxUsrLogEntryForGenricOTPValidationOverride(Dictionary<string, ApprovalAction> genericOTPApprovals, Transaction trx, string loginId, ExecutionContext executionContext, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(genericOTPApprovals, (trx != null ? trx.Trx_id : -1), loginId, sqlTrx);
            try
            {
                if (genericOTPApprovals != null && genericOTPApprovals.Count() > 0 && trx.Trx_id > 0)
                {
                    foreach (var item in genericOTPApprovals)
                    {

                        if (trx.TrxLines.Count > 1)
                        {
                            for (int i = 0; i < trx.TrxLines.Count; i++)
                            {
                                for (int j = 0; j < trx.TrxLines.Count; j++)
                                {
                                    if (trx.TrxLines[j].ParentLine != null && trx.TrxLines[i].DBLineId == trx.TrxLines[j].ParentLine.DBLineId && trx.TrxLines[i].ProductID == trx.TrxLines[j].ProductID
                                        && trx.TrxLines[i].ProductTypeCode == "CARDSALE" && trx.TrxLines[j].CardNumber != trx.TrxLines[i].CardNumber)
                                    {
                                        ProductsContainerDTO productsContainerDTO = ProductsContainerList.GetProductsContainerDTO(executionContext, trx.TrxLines[i].ProductID);
                                        if (productsContainerDTO.IsTransferCard)
                                        {
                                            List<KeyValuePair<TrxUserLogsDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<TrxUserLogsDTO.SearchByParameters, string>>();
                                            searchParameters.Add(new KeyValuePair<TrxUserLogsDTO.SearchByParameters, string>(TrxUserLogsDTO.SearchByParameters.TRX_ID, trx.Trx_id.ToString()));
                                            TrxUserLogsList trxUserLogsList = new TrxUserLogsList(executionContext);
                                            List<TrxUserLogsDTO> trxUserLogsDTOList = trxUserLogsList.GetAllTrxUserLogs(searchParameters, sqlTrx);
                                            if (trxUserLogsDTOList != null && trxUserLogsDTOList.Any(x => x.TrxId == trx.Trx_id && x.LineId == trx.TrxLines[i].DBLineId && x.Action == "OVERRIDE TRANSFER CARD OTP") == false)
                                            {
                                                trx.InsertTrxLogs(trx.Trx_id, trx.TrxLines[i].DBLineId, loginId, "OVERRIDE TRANSFER CARD OTP",
                                                "Staff overrides transfer card OTP", sqlTrx, item.Value.ApproverId, item.Value.ApprovalTime);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    genericOTPApprovals = new Dictionary<string, ApprovalAction>();
                }
            }
            catch(Exception ex)
            {
                log.Error("Error creating Transfer card userlogs.");
            }
            log.LogMethodExit();
        }
    }
}
