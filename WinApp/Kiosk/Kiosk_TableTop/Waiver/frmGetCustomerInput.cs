/********************************************************************************************
 * Project Name - Portrait Kiosk
 * Description  - user interface -frmGetCustomerInput
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
*2.150.3.0   28-Apr-2023      Vignesh Bhat        Created: TableTop Kiosk Changes
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.Transaction;
using System.Collections.Concurrent;

namespace Parafait_Kiosk
{
    public partial class frmGetCustomerInput : BaseForm
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private TagNumberParser tagNumberParser;
        private Card card;
        private Utilities utilities;
        private CustomerDTO customerDTO;
        private string defaultMsg;
        private bool validationEx = false;
        private ConcurrentQueue<KeyValuePair<int, string>> statusProgressMsgQueue = new ConcurrentQueue<KeyValuePair<int, string>>();
        private VirtualWindowsKeyboardController virtualKeyboardController;
        private VirtualKeyboardController customKeyboardController;

        public CustomerDTO GetCustomerDTO { get { return customerDTO; } }
        public frmGetCustomerInput()
        {
            log.LogMethodEntry();
            utilities = KioskStatic.Utilities;
            utilities.setLanguage();
            InitializeComponent();
            KioskStatic.setDefaultFont(this);
            KioskStatic.formatMessageLine(txtMessage, 26, ThemeManager.CurrentThemeImages.BottomMessageLineImage);
            KioskStatic.logToFile("Loading customer input form");
            defaultMsg = MessageContainerList.GetMessage(utilities.ExecutionContext, 572);
            txtMessage.Text = defaultMsg;
            //SetTextBoxFontColors();
            InitializeKeyboard();
            SetCustomizedFontColors();
            utilities.setLanguage(this);
            log.LogMethodExit();
        }

        private void frmGetCustomerInput_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            SetFocus();
            HideKeyBoard();
            tagNumberParser = new TagNumberParser(utilities.ExecutionContext);
            if (KioskStatic.TopUpReaderDevice != null)
            {
                KioskStatic.logToFile("Registering topup card reader device");
                KioskStatic.TopUpReaderDevice.Register(new EventHandler(CardScanCompleteEventHandle));
                List<EventHandler> eventList = (KioskStatic.TopUpReaderDevice != null ? KioskStatic.TopUpReaderDevice.GetCallBackList : new List<EventHandler>());
                int listCount = eventList != null && eventList.Any() ? eventList.Count : 0;
                KioskStatic.logToFile(this.Name + ": Register topup card reader device. Call back list count: " + listCount.ToString());
                log.Info(this.Name + ": Register topup card reader device. Call back list count: " + listCount.ToString());
            }
            this.ActiveControl = txtEmail;
            ResetKioskTimer();
            log.LogMethodExit();
        }
        private void CardScanCompleteEventHandle(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (e is DeviceScannedEventArgs)
            {
                try
                {
                    DisableButtons();
                    txtMessage.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 1448);//Loading... Please wait...
                    Application.DoEvents();
                    this.customerDTO = null;
                    //showhideKeypad('H');
                    DeviceScannedEventArgs checkScannedEvent = e as DeviceScannedEventArgs;
                    TagNumber tagNumber;
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
                            KioskStatic.logToFile(ex.Message);
                            return;
                        }
                        try
                        {
                            scannedTagNumber = tagNumberParser.ValidateDecryptedTag(decryptedTagNumber, KioskStatic.Utilities.ParafaitEnv.SiteId);
                        }
                        catch (ValidationException ex)
                        {
                            log.LogVariableState("Decrypted Tag Number validation: ", ex);
                            KioskStatic.logToFile(ex.Message);
                            return;
                        }
                        catch (Exception ex)
                        {
                            log.LogVariableState("Decrypted Tag Number validation: ", ex);
                            KioskStatic.logToFile(ex.Message);
                            return;
                        }
                    }
                    if (tagNumberParser.TryParse(scannedTagNumber, out tagNumber) == false)
                    {
                        string message = tagNumberParser.Validate(scannedTagNumber);
                        KioskStatic.logToFile("Invalid Tag Number. " + message);
                        log.LogMethodExit(null, "Invalid Tag Number. " + message);
                        return;
                    }

                    string CardNumber = tagNumber.Value;
                    CardNumber = KioskStatic.ReverseTopupCardNumber(CardNumber);

                    //ApplicationContext ap = new ApplicationContext();
                    try
                    {

                        //KioskHelper.ShowPreloader(ap, utilities, Properties.Resources.Back_button_box, Properties.Resources.PreLoader, statusProgressMsgQueue);
                        handleCardRead(CardNumber, sender as DeviceClass);
                        if (card != null)
                        {
                            AccountBL accountBL = new AccountBL(utilities.ExecutionContext, card.card_id, true, true, null);
                            //List<KeyValuePair<AccountDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<AccountDTO.SearchByParameters, string>>();
                            //searchParameters.Add(new KeyValuePair<AccountDTO.SearchByParameters, string>(AccountDTO.SearchByParameters.TAG_NUMBER, CardNumber));
                            //AccountListBL accountListBL = new AccountListBL(utilities.ExecutionContext);
                            //List<AccountDTO> accountDTOs = accountListBL.GetAccountDTOList(searchParameters,false,true,null);
                            if (accountBL.AccountDTO.CustomerId == -1)
                            {
                                KioskStatic.logToFile("Card is not linked with a customer");
                                log.Info("Card is not linked with a customer");

                                throw new ValidationException(MessageContainerList.GetMessage(utilities.ExecutionContext, 2324));
                            }
                            CustomerBL customerBL = new CustomerBL(utilities.ExecutionContext, accountBL.AccountDTO.CustomerId, true, true);
                            //KioskHelper.ClosePreLoaderForm(statusProgressMsgQueue);
                            if (ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "OTP_CHECK_REQUIRED_FOR_WAIVER_REGISTRATION") == "Y")
                            {
                                KioskStatic.logToFile("OTP check required for waiver registration");
                                log.Info("OTP check required for waiver registration");
                                if (customerBL.CustomerDTO != null)
                                {
                                    //this.Hide();
                                    if (!string.IsNullOrEmpty(customerBL.CustomerDTO.Email))
                                    {
                                        txtEmail.Text = customerBL.CustomerDTO.Email; 
                                        ResetKioskTimer();
                                        HideKeyBoard();
                                        using (frmCustomerOTP otp = new frmCustomerOTP(customerBL.CustomerDTO))
                                        {
                                            if (otp.ShowDialog() == DialogResult.OK)
                                            {
                                                log.Info("otp.ShowDialog() == DialogResult.OK");
                                                this.DialogResult = DialogResult.OK;
                                                this.customerDTO = customerBL.CustomerDTO;
                                                KioskStatic.logToFile("Received cutomer DTO, closing form");
                                                log.Info("Received cutomer DTO, closing form");
                                                this.Close();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        KioskStatic.logToFile("Cutomer Email id is missing");
                                        log.Info("Cutomer Email id is missing");

                                        throw new ValidationException(MessageContainerList.GetMessage(utilities.ExecutionContext, "No")
                                                                   + " " + MessageContainerList.GetMessage(utilities.ExecutionContext, "EmailId"));
                                        //txtMessage.Text = "Email Id is not registered";
                                        //KioskStatic.logToFile(txtMessage.Text);
                                        //log.LogMethodExit();
                                        //return;
                                    }
                                }
                                else
                                {
                                    KioskStatic.logToFile("Didnt receive customerDTO");
                                    log.Info("Didnt receive customerDTO");
                                    throw new ValidationException(MessageContainerList.GetMessage(utilities.ExecutionContext, 2324));
                                    //KioskStatic.logToFile(txtMessage.Text);
                                    //log.LogMethodExit();
                                    //return;
                                }
                            }
                            else
                            {
                                this.DialogResult = DialogResult.OK;
                                this.customerDTO = customerBL.CustomerDTO;
                                KioskStatic.logToFile("Received cutomer DTO, closing form");
                                log.Info("Received cutomer DTO, closing form");
                                this.Close();
                            }
                        }
                        else
                        {
                            KioskStatic.logToFile("Error: Card reading failed");
                            log.Info("Error: Card reading failed");
                            throw new ValidationException(MessageContainerList.GetMessage(utilities.ExecutionContext, 89));//Error: Card reading failed
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        txtMessage.Text = ex.Message;
                        KioskStatic.logToFile(txtMessage.Text);
                        HideKeyBoard();
                        using (frmOKMsg frmOK = new frmOKMsg(ex.Message))
                        {
                            frmOK.ShowDialog();
                        }
                    }
                    finally
                    {
                        txtMessage.Text = defaultMsg;
                        //int errorRecoveryCounter = 0;
                        //KioskHelper.CloseAPObject(ap, errorRecoveryCounter);
                    }
                }
                finally
                {
                    EnableButtons();
                }
            }
            ResetKioskTimer();
            log.LogMethodExit();
        }
        void handleCardRead(string inCardNumber, DeviceClass readerDevice)
        {
            log.LogMethodEntry(inCardNumber, readerDevice);
            ResetKioskTimer();
            txtMessage.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 1448);//Loading... Please wait...
            Card lclCard = null;
            string message;
            if (utilities.ParafaitEnv.MIFARE_CARD)
            {
                try
                {
                    if (KioskStatic.cardAcceptor != null)
                    {
                        KioskStatic.cardAcceptor.EjectCardFront();
                        KioskStatic.cardAcceptor.BlockAllCards();
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    txtMessage.Text = ex.Message;
                    KioskStatic.logToFile(txtMessage.Text);
                    throw;
                }
            }
            else
            {
                lclCard = new Card(readerDevice, inCardNumber, "External POS", utilities);
                log.LogVariableState("lclCard", lclCard);
            }
            if (lclCard.technician_card.Equals('Y'))
            {
                txtMessage.Text = KioskStatic.Utilities.MessageUtils.getMessage(197, lclCard.CardNumber);
                throw new ValidationException(MessageContainerList.GetMessage(utilities.ExecutionContext, txtMessage.Text)); 
            }

            if (lclCard.CardStatus == "NEW")
            {
                if (utilities.ParafaitEnv.MIFARE_CARD)
                    message = MessageContainerList.GetMessage(utilities.ExecutionContext, 528);
                else
                    message = MessageContainerList.GetMessage(utilities.ExecutionContext, 459);

                if (utilities.ParafaitEnv.MIFARE_CARD && KioskStatic.cardAcceptor != null)
                {
                    KioskStatic.cardAcceptor.EjectCardFront();
                    KioskStatic.cardAcceptor.BlockAllCards();
                }

                KioskStatic.logToFile("NEW card tapped. Rejected.");
                log.Info("message " + message);
                throw new ValidationException(message);
            }
            card = lclCard;
            log.LogMethodExit();
        }


        private void btnOkay_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            try
            {
                DisableButtons();
                HideKeyBoard();
                txtMessage.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 1448);//Loading... Please wait...
                Application.DoEvents();
                //showhideKeypad('H');
                KioskStatic.logToFile("User clicked Okay button");
                if (string.IsNullOrEmpty(txtEmail.Text))
                {
                    txtMessage.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 572);//Enter a valid email id
                    KioskStatic.logToFile(txtMessage.Text);
                    log.Info(txtMessage.Text);
                }
                else
                {
                    GetCustomerByEmailId();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                if (validationEx == true)
                {
                    txtMessage.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 2325, MessageContainerList.GetMessage(utilities.ExecutionContext, "Email Id")) + ". ";
                    validationEx = false;
                }
                else
                {
                    txtMessage.Text = ex.Message;
                }
                KioskStatic.logToFile(txtMessage.Text);
                using (frmOKMsg frmOK = new frmOKMsg(ex.Message))
                {
                    frmOK.ShowDialog();
                }
            }
            finally
            {
                EnableButtons();
                txtMessage.Text = defaultMsg;
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }

        private void GetCustomerByEmailId()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            //ApplicationContext ap = new ApplicationContext();
            try
            {

                //KioskHelper.ShowPreloader(ap, utilities, Properties.Resources.Back_button_box, Properties.Resources.PreLoader, statusProgressMsgQueue);

                KioskStatic.logToFile("Geting Customer by Email Id");
                this.customerDTO = null;
                List<CustomerDTO> finalList = new List<CustomerDTO>();
                CustomerListBL customerListBL = new CustomerListBL(utilities.ExecutionContext);
                CustomerSearchCriteria customerSearchCriteria = new CustomerSearchCriteria();
                customerSearchCriteria.And(new CustomerSearchCriteria(CustomerSearchByParameters.CONTACT_CONTACT_TYPE, Operator.EQUAL_TO, "EMAIL"));
                customerSearchCriteria.And(new CustomerSearchCriteria(CustomerSearchByParameters.CONTACT_ATTRIBUTE1, Operator.EQUAL_TO, txtEmail.Text));
                customerSearchCriteria.And(new CustomerSearchCriteria(CustomerSearchByParameters.ISACTIVE, Operator.EQUAL_TO, 1));
                List<CustomerDTO> customerDTOList = customerListBL.GetCustomerDTOList(customerSearchCriteria, true, true);
                if (customerDTOList != null && customerDTOList.Count == 1)
                {
                    KioskStatic.logToFile("Found one matching record");
                    log.Info("Found one matching record");
                    finalList.Add(customerDTOList[0]);
                }
                else if (customerDTOList != null && customerDTOList.Count > 0)
                {
                    KioskStatic.logToFile("Found more than one matching record");
                    log.Info("Found more than one matching record");
                    for (int i = 0; i < customerDTOList.Count; i++)
                    {
                        CustomerBL customerBL = null;
                        try
                        {
                            customerBL = new CustomerBL(utilities.ExecutionContext, customerDTOList[i]);

                            if (customerBL.IsAdult())
                            {
                                finalList.Add(customerDTOList[i]);
                            }
                        }
                        catch (Exception ex)
                        {
                            //KioskHelper.ClosePreLoaderForm(statusProgressMsgQueue);
                            log.Error(ex);
                            txtMessage.Text = ex.Message;
                            KioskStatic.logToFile(txtMessage.Text);
                            if (customerBL != null && customerBL.CustomerDTO != null)
                            {
                                log.LogVariableState("CustomerDTO", customerBL.CustomerDTO);
                            }
                        }

                    }
                    if (finalList != null && finalList.Count > 1)
                    {
                        validationEx = true;
                        KioskStatic.logToFile("More than one matching record, even after ignoring minors");
                        log.Info("More than one matching record, even after ignoring minors");
                        throw new ValidationException(MessageContainerList.GetMessage(utilities.ExecutionContext, 2325, MessageContainerList.GetMessage(utilities.ExecutionContext, "Email Id"))
                                             + ". " + MessageContainerList.GetMessage(utilities.ExecutionContext, 441));
                    }
                }
                else
                {
                    throw new ValidationException(MessageContainerList.GetMessage(utilities.ExecutionContext, 2486));
                    //Email ID is not registered, Use New Customer option to register
                }
                //KioskHelper.ClosePreLoaderForm(statusProgressMsgQueue);
                if (finalList != null && finalList.Count == 1)
                {
                    if (ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "OTP_CHECK_REQUIRED_FOR_WAIVER_REGISTRATION") == "Y")
                    {
                        HideKeyBoard();
                        using (frmCustomerOTP otp = new frmCustomerOTP(finalList[0]))
                        {
                            if (otp.ShowDialog() == DialogResult.OK)
                            {
                                log.Info("otp.ShowDialog() == DialogResult.OK");
                                this.DialogResult = DialogResult.OK;
                                this.customerDTO = finalList[0];
                                KioskStatic.logToFile("Received cutomer DTO, closing form");
                                log.Info("Received cutomer DTO, closing form");
                                this.Close();
                            }
                        }
                    }
                    else
                    {
                        this.DialogResult = DialogResult.OK;
                        this.customerDTO = finalList[0];
                        KioskStatic.logToFile("Received cutomer DTO, closing form");
                        log.Info("Received cutomer DTO, closing form");
                        this.Close();
                    }
                }
            }
            finally
            {
                //int errorRecoveryCounter = 0;
                //KioskHelper.CloseAPObject(ap, errorRecoveryCounter);
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                DisableButtons();
                //showhideKeypad('H');
                KioskStatic.logToFile("Cancel button is clicked");
                this.DialogResult = System.Windows.Forms.DialogResult.No;
                Close();
            }
            finally
            {
                EnableButtons();
            }
            log.LogMethodExit();
        }

        private void InitializeKeyboard()
        {
            log.LogMethodEntry();
            try
            {
                bool isWindowsKeyboardEnabled = ParafaitDefaultContainerList.GetParafaitDefault<bool>(KioskStatic.Utilities.ExecutionContext, "ENABLE_WINDOWS_KEYBOARD", false);
                if (isWindowsKeyboardEnabled)
                {
                    SetVirtualKeyboard();
                }
                else
                {
                    SetCustomKeyboard();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error Initializing keyboard in Customer OTP screen: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void SetVirtualKeyboard()
        {
            log.LogMethodEntry();
            try
            {
                virtualKeyboardController = new VirtualWindowsKeyboardController(panel1.Top);
                bool popupOnScreenKeyBoard = true;
                virtualKeyboardController.Initialize(this, new List<Control>() { btnShowKeyPad }, popupOnScreenKeyBoard);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error Initializing Windows Keyboard in Get Customer Input screen: " + ex.Message);
            }
            log.LogMethodExit();
        }


        private void SetCustomKeyboard()
        {
            log.LogMethodEntry();
            try
            {
                customKeyboardController = new VirtualKeyboardController(panel1.Top);
                bool showKeyboardOnTextboxEntry = true;
                customKeyboardController.Initialize(this, new List<Control>() { btnShowKeyPad }, showKeyboardOnTextboxEntry, null, lblmsg.Font.FontFamily.Name);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error Initializing Custom Keyboard in Get Customer Input screen: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void DisposeKeyboardObject()
        {
            log.LogMethodEntry();
            try
            {
                bool isWindowsKeyboardEnabled = ParafaitDefaultContainerList.GetParafaitDefault<bool>(KioskStatic.Utilities.ExecutionContext, "ENABLE_WINDOWS_KEYBOARD", false);
                if (isWindowsKeyboardEnabled)
                {
                    virtualKeyboardController.Dispose();
                }
                else
                {
                    customKeyboardController.Dispose();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error Disposing keyboard in Get Customer Input screen: " + ex.Message);
            }
            log.LogMethodExit();
        }
        private void HideKeyBoard()
        {
            log.LogMethodEntry();
            try
            {
                bool isWindowsKeyboardEnabled = ParafaitDefaultContainerList.GetParafaitDefault<bool>(KioskStatic.Utilities.ExecutionContext, "ENABLE_WINDOWS_KEYBOARD", false);
                if (isWindowsKeyboardEnabled)
                {
                    virtualKeyboardController.HideKeyboard();
                }
                else
                {
                    customKeyboardController.HideKeyboard();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error Disposing keyboard in Get Customer Input screen: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void textBox_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                ResetKioskTimer();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        //AlphaNumericKeyPad keypad;
        //public TextBox currentActiveTextBox;
        private void btnShowKeyPad_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            log.LogMethodExit();
        }
        private void frmGetCustomerInput_Closed(object sender, FormClosedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (KioskStatic.TopUpReaderDevice != null)
            {
                KioskStatic.TopUpReaderDevice.UnRegister();
                List<EventHandler> eventList = (KioskStatic.TopUpReaderDevice != null ? KioskStatic.TopUpReaderDevice.GetCallBackList : new List<EventHandler>());
                int listCount = eventList != null && eventList.Any() ? eventList.Count : 0;
                KioskStatic.logToFile(this.Name + ": UnRegister topup card reader device. Call back list count: " + listCount.ToString());
                log.Info(this.Name + ": UnRegister topup card reader device. Call back list count: " + listCount.ToString());
            }
            DisposeKeyboardObject();
            KioskStatic.logToFile(this.Name + ": Form closed");
            log.LogMethodExit();
        }
        private void SetTextBoxFontColors()
        {
            log.LogMethodEntry();
            if (KioskStatic.CurrentTheme == null ||
               (KioskStatic.CurrentTheme != null && KioskStatic.CurrentTheme.TextForeColor == Color.White))
            {
                txtEmail.ForeColor = Color.Black;
            }
            else
            {
                txtEmail.ForeColor = KioskStatic.CurrentTheme.TextForeColor;
            }
            log.LogMethodExit();
        }

        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            //KioskStatic.logToFile("Setting customized font colors for the UI elements in frmGetCustomerInput");
            try
            {
                this.label1.ForeColor = KioskStatic.CurrentTheme.FrmGetCustInputLabel1TextForeColor;
                this.label2.ForeColor = KioskStatic.CurrentTheme.FrmGetCustInputLabel2TextForeColor;
                this.label3.ForeColor = KioskStatic.CurrentTheme.FrmGetCustInputLabel3TextForeColor;
                this.lblmsg.ForeColor = KioskStatic.CurrentTheme.FrmGetCustInputLblmsgTextForeColor;
                this.lmlEmail.ForeColor = KioskStatic.CurrentTheme.FrmGetCustInputLmlEmailTextForeColor;
                this.txtEmail.ForeColor = KioskStatic.CurrentTheme.FrmGetCustInputTxtEmailTextForeColor;
                this.btnOk.ForeColor = KioskStatic.CurrentTheme.FrmGetCustInputBtnOkTextForeColor;
                this.btnCancel.ForeColor = KioskStatic.CurrentTheme.FrmGetCustInputBtnCancelTextForeColor;
                this.txtMessage.ForeColor = KioskStatic.CurrentTheme.FrmGetCustInputTxtMessageTextForeColor;
                this.BackgroundImage = ThemeManager.GetBackgroundImage(ThemeManager.CurrentThemeImages.GetCustomerInputBackgroundImage);
                btnOk.BackgroundImage =
                    btnCancel.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage;
            }
            catch (Exception ex)
            {
                log.Error("Errow while Setting Customized Fore Colors", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements in frmGetCustomerInput: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void EnableButtons()
        {
            log.LogMethodEntry();
            try
            {
                btnCancel.Enabled = true;
                btnShowKeyPad.Enabled = true;
                btnOk.Enabled = true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        private void DisableButtons()
        {
            log.LogMethodEntry();
            try
            {
                btnCancel.Enabled = false;
                btnOk.Enabled = false;
                btnShowKeyPad.Enabled = false;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        
        private void SetFocus()
        {
            log.LogMethodEntry();
            try
            {
                txtEmail.Focus(); 
            }
            catch(Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in SetFocus(): " + ex.Message);
            }
            log.LogMethodExit();
        }
    }
}
