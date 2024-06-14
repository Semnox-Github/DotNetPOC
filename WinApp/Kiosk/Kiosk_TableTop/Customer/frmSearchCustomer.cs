/********************************************************************************************
 * Project Name - Portrait Kiosk
 * Description  - Created for Customer Registration Lookup while purchsing the products
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By     Remarks          
 *********************************************************************************************
 *2.150.6     10-Nov-2023      Sathyavathi     Created for Customer Lookup Enhancement
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
    public partial class frmSearchCustomer : BaseFormKiosk
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private TagNumberParser tagNumberParser;
        private Card card;
        private Utilities utilities;
        private CustomerDTO customerDTO;
        private string defaultMsg;
        private ConcurrentQueue<KeyValuePair<int, string>> statusProgressMsgQueue = new ConcurrentQueue<KeyValuePair<int, string>>();
        private VirtualWindowsKeyboardController virtualKeyboardController;
        private VirtualKeyboardController customKeyboardController;
        private int guestCustomerId;

        public CustomerDTO GetCustomerDTO { get { return customerDTO; } }

        public frmSearchCustomer(bool isCustomerMandatory, bool showGreeting1)
        {
            log.LogMethodEntry(isCustomerMandatory, showGreeting1);
            utilities = KioskStatic.Utilities;
            utilities.setLanguage();
            InitializeComponent();
            
            KioskStatic.setDefaultFont(this);
            txtEmailId.CharacterCasing = CharacterCasing.Lower;
            KioskStatic.formatMessageLine(txtMessage, 26, ThemeManager.CurrentThemeImages.BottomMessageLineImage);
            KioskStatic.logToFile("Loading customer registration lookup form");
            DisplaybtnCancel(false);
            DisplaybtnPrev(true);

            try
            {
                InitializeKeyboard();

                txtEmailId.Text = txtPhoneNum.Text = string.Empty;
                string searchFilter = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "KIOSK_CUSTOMER_LOOKUP_SEARCH_FILTER");
                if (!string.IsNullOrWhiteSpace(searchFilter))
                {
                    if (searchFilter.ToLower().Equals("phone"))
                    {
                        pnlSearchViaPhone.Visible = true;
                        pnlSearchViaEmail.Visible = false;
                        lblORField.Visible = false;
                    }
                    else if (searchFilter.ToLower().Equals("email"))
                    {
                        pnlSearchViaEmail.Location = pnlSearchViaPhone.Location;
                        pnlSearchViaPhone.Visible = false;
                        pnlSearchViaEmail.Visible = true;
                        lblORField.Visible = false;
                    }
                    else
                    {
                        pnlSearchViaPhone.Visible = true;
                        pnlSearchViaEmail.Visible = true;
                        lblORField.Visible = true;
                    }
                }
                SetPhoneNumberFieldWidth();
                //5251 - Selected product requires customer association
                lblGreeting.Text = defaultMsg = MessageContainerList.GetMessage(utilities.ExecutionContext, 5251);

                lblNewCustomerHeader.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, "New Customer?");//Literal
                //5334 - Press New Registration
                lblNewCustomerMsg.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 5334);

                lblExistingCustomerHeader.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, "Existing Customer?");//Literal
                //5327 - Tap your Card or Search
                lblExistingCustomerMsg.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 5327);
                txtMessage.Text = defaultMsg;

                if (isCustomerMandatory == false)
                {
                    btnPrev.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, "Skip");//Literal
                    //5302 - Registration is encouraged. Click Skip to continue without registering.
                    lblGreeting.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 5302);
                    txtMessage.Text = defaultMsg = string.Empty;
                }
                if (showGreeting1 == false)
                {
                    lblGreeting.Visible = showGreeting1;
                    txtMessage.Text = string.Empty;
                }
                SetCustomiImages();
                SetCustomizedFontColors();
                utilities.setLanguage(this);
            }
            catch (Exception ex)
            {
                log.Error("Error in Search Csutomer screen", ex);
                KioskStatic.logToFile("Error in Search Cutomer screen" + ex.Message);
            }
            log.LogMethodExit();
        }

        private void frmSearchCustomer_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            this.TransparencyKey = Color.Empty;
            tagNumberParser = new TagNumberParser(utilities.ExecutionContext);
            RegisterCardReader();
            SetFocus();
            HideKeyBoard();
            log.LogMethodExit();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ResetKioskTimer();
            try
            {
                txtMessage.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 1448);//Loading... Please wait...
                DisableButtons();
                KioskStatic.logToFile("Searching Customer..");
                this.customerDTO = null;           
                bool status = ValidateEmailIdField();
                if (status == false)
                {
                    log.LogMethodExit();
                    return;
                }

                status = ValidatePhoneNumberField();
                if (status == false)
                {
                    log.LogMethodExit();
                    return;
                }
                List<CustomerDTO> customerDTOList = GetCustomersFromDB();
                if (customerDTOList != null && customerDTOList.Count == 1)
                {
                    SetCustomer(customerDTOList);
                }
                else if (customerDTOList != null && customerDTOList.Count > 1)
                {
                    SelectACustomer(customerDTOList);
                }
                else
                {
                    throw new ValidationException(MessageContainerList.GetMessage(utilities.ExecutionContext, 5256)); //No customer found
                }
            }
            catch (CustomerStatic.TimeoutOccurred ex)
            {
                HideKeyBoard();
                log.Error(ex);
                KioskStatic.logToFile(ex.Message);
                throw ex;
            }
            catch (Exception ex)
            {
                HideKeyBoard();
                log.Error(ex);
                KioskStatic.logToFile(ex.Message);
                frmOKMsg.ShowUserMessage(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
                txtMessage.Text = defaultMsg;
                EnableButtons();
                SetFocus(); //show keypad again
            }
            log.LogMethodExit();
        }

        private List<CustomerDTO> GetCustomersFromDB()
        {
            log.LogMethodEntry();
            CustomerListBL customerListBL = new CustomerListBL(utilities.ExecutionContext);
            CustomerSearchCriteria customerSearchCriteria = new CustomerSearchCriteria();
            if (!string.IsNullOrWhiteSpace(txtEmailId.Text))
            {
                customerSearchCriteria.And(new CustomerSearchCriteria(CustomerSearchByParameters.CONTACT_CONTACT_TYPE, Operator.EQUAL_TO, "EMAIL"));
                customerSearchCriteria.And(new CustomerSearchCriteria(CustomerSearchByParameters.CONTACT_ATTRIBUTE1, Operator.EQUAL_TO, txtEmailId.Text));
                customerSearchCriteria.And(new CustomerSearchCriteria(CustomerSearchByParameters.ISACTIVE, Operator.EQUAL_TO, 1));
            }
            if (!string.IsNullOrWhiteSpace(txtPhoneNum.Text))
            {
                customerSearchCriteria.And(new CustomerSearchCriteria(CustomerSearchByParameters.CONTACT_CONTACT_TYPE, Operator.EQUAL_TO, "PHONE"));
                customerSearchCriteria.And(new CustomerSearchCriteria(CustomerSearchByParameters.CONTACT_ATTRIBUTE1, Operator.EQUAL_TO, txtPhoneNum.Text));
                customerSearchCriteria.And(new CustomerSearchCriteria(CustomerSearchByParameters.ISACTIVE, Operator.EQUAL_TO, 1));
            }
            if (customerSearchCriteria.ContainsCondition == false)
            {
                //5299 - Please enter your details to search the record
                throw new ValidationException(MessageContainerList.GetMessage(utilities.ExecutionContext, 5299));
            }
            List<CustomerDTO> customerDTOList = customerListBL.GetCustomerDTOList(customerSearchCriteria, true, true);
            if (customerDTOList != null && customerDTOList.Any())
            {
                foreach (CustomerDTO customer in customerDTOList.ToList())
                {
                    if (customer.Id == guestCustomerId)
                    {
                        customerDTOList.Remove(customer);                   
                    }
                }
            }
            log.LogMethodExit(customerDTOList);
            return customerDTOList;
        }
        private void SetCustomer(List<CustomerDTO> customerDTOList)
        {
            log.LogMethodEntry(customerDTOList);
            KioskStatic.logToFile("Found one customer record for the search: " + customerDTOList[0].FirstName);
            log.Info("Found one customer record for the search: " + customerDTOList[0].FirstName);
            bool isCustomerConfirmed = GetCustomerConfirmationForTheFoundResult(customerDTOList[0]);
            if (isCustomerConfirmed == true)
            {
                this.customerDTO = customerDTOList[0];
            }
            log.LogMethodExit(this.customerDTO);
        }

        private void SelectACustomer(List<CustomerDTO> customerDTOList)
        {
            log.LogMethodEntry(customerDTOList);
            List<CustomerDTO> foundCustomersList = new List<CustomerDTO>();
            for (int i = 0; i < customerDTOList.Count; i++)
            {
                foundCustomersList.Add(customerDTOList[i]);
            }
            KioskStatic.logToFile("Found more than one customer record");
            log.Info("Found more than one customer record");
            HideKeyBoard();
            using (frmSelectCustomer frmSelect = new frmSelectCustomer(utilities.ExecutionContext, foundCustomersList))
            {
                DialogResult dr = frmSelect.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    this.DialogResult = DialogResult.OK;
                    this.customerDTO = frmSelect.GetCustomerDTO;
                    KioskStatic.logToFile("Received customer record: " + customerDTO.FirstName);
                    log.Info("Received customer record: " + customerDTO.FirstName);
                    Close();
                }
            }
            log.LogMethodExit(this.customerDTO);
        }

        private void btnNewRegistration_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ResetKioskTimer();
            try
            {
                HideKeyBoard();
                this.customerDTO = CustomerStatic.ShowCustomerScreen();
                if (customerDTO != null && customerDTO.Id > -1)
                {
                    this.DialogResult = DialogResult.OK;
                    Close();
                }
            }
            catch (CustomerStatic.TimeoutOccurred ex)
            {
                log.Error(ex);
                PerformTimeoutAbortAction(null, null);
                this.DialogResult = DialogResult.Cancel;
                Close();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in SetFocus() of customer search form: " + ex.Message);
            }
            finally
            {
                SetFocus();
            }
            log.LogMethodExit();
        }

        public override void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                KioskStatic.logToFile("Cancel Button Pressed : Triggering Home Button Action ");
                base.btnHome_Click(sender, e);
            }
            catch (Exception ex)
            {
                log.Error("Error in btnCancel_Click", ex);
            }
            log.LogMethodExit();
        }

        public override void btnPrev_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            DialogResult = System.Windows.Forms.DialogResult.No;
            Close();
            log.LogMethodExit();
        }

        private void btnShowKeyPad_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            log.LogMethodExit();
        }

        private void textBox_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            log.LogMethodExit();
        }

        private void txtPhoneNum_KeyPress(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            if (!char.IsNumber(e.KeyChar) && !char.IsControl(e.KeyChar) && !(e.KeyChar == '+'))
                e.Handled = true;
            log.LogMethodExit();
        }


        private bool GetCustomerConfirmationForTheFoundResult(CustomerDTO foundCustomerDTO)
        {
            log.LogMethodEntry(foundCustomerDTO);
            ResetKioskTimer();
            bool isCusomerConfirmed = false;
            try
            {
                HideKeyBoard();
                string customerName = foundCustomerDTO.FirstName;
                if (!string.IsNullOrWhiteSpace(foundCustomerDTO.LastName))
                {
                    customerName += " " + foundCustomerDTO.LastName;
                }
                using (frmCustomerFound frmCustomerFound = new frmCustomerFound(customerName))
                {
                    DialogResult dr = frmCustomerFound.ShowDialog();
                    //kioskTransaction = frmCustomerFound.GetKioskTransaction;
                    if (dr == DialogResult.OK)
                    {
                        this.DialogResult = DialogResult.OK;
                        isCusomerConfirmed = true;
                        KioskStatic.logToFile("Found customer record: " + foundCustomerDTO.FirstName);
                        log.Info("Found customer record: " + foundCustomerDTO.FirstName);
                        Close();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Info(ex);
                KioskStatic.logToFile("Error in GetConfirmationForTheFoundCostumer() in search customer: " + ex.Message);
            }
            log.LogMethodExit(isCusomerConfirmed);
            return isCusomerConfirmed;
        }

        private void CardScanCompleteEventHandle(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            if (e is DeviceScannedEventArgs)
            {
                try
                {
                    DisableButtons();
                    txtMessage.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 1448);//Loading... Please wait...
                    Application.DoEvents();
                    this.customerDTO = null;
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

                    try
                    {
                        HandleCardRead(CardNumber, sender as DeviceClass);
                        if (card != null)
                        {
                            AccountBL accountBL = new AccountBL(utilities.ExecutionContext, card.card_id, true, true, null);
                            if (accountBL.AccountDTO.CustomerId == -1)
                            {
                                //2324 - Card is not linked to a customer record
                                string errMsg = MessageContainerList.GetMessage(utilities.ExecutionContext, 2324);
                                KioskStatic.logToFile(errMsg);
                                log.Info(errMsg);

                                throw new ValidationException(errMsg);
                            }
                            CustomerBL customerBL = new CustomerBL(utilities.ExecutionContext, accountBL.AccountDTO.CustomerId, true, true);
                            HideKeyBoard();
                            try
                            {
                                UnregisterCardReader();
                                if (customerBL.CustomerDTO != null)
                                {
                                    if (customerBL.CustomerDTO.Id == guestCustomerId)
                                    {
                                        string warning = "Warning: Guest Customer Found with id" + guestCustomerId + "skipping to add to the list";
                                        KioskStatic.logToFile(warning);
                                        log.Info(warning);
                                    }
                                    else
                                    {
                                        bool isCustomerConfirmed = GetCustomerConfirmationForTheFoundResult(customerBL.CustomerDTO);
                                        if (isCustomerConfirmed)
                                        {
                                            this.customerDTO = customerBL.CustomerDTO;
                                        }
                                    }
                                }
                            }
                            finally
                            {
                                RegisterCardReader();
                            }
                        }
                        else
                        {
                            //89 - Error: Card reading failed
                            string errMsg = MessageContainerList.GetMessage(utilities.ExecutionContext, 89);
                            KioskStatic.logToFile(errMsg);
                            log.Info(errMsg);
                            throw new ValidationException(errMsg);
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        txtMessage.Text = ex.Message;
                        KioskStatic.logToFile(txtMessage.Text);
                        HideKeyBoard();
                        frmOKMsg.ShowUserMessage(ex.Message);
                    }
                    finally
                    {
                        txtMessage.Text = defaultMsg;
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

        private void RegisterCardReader()
        {
            log.LogMethodEntry();
            if (KioskStatic.TopUpReaderDevice != null)
            {
                KioskStatic.TopUpReaderDevice.Register(new EventHandler(CardScanCompleteEventHandle));
                List<EventHandler> eventList = (KioskStatic.TopUpReaderDevice != null ? KioskStatic.TopUpReaderDevice.GetCallBackList : new List<EventHandler>());
                int listCount = eventList != null && eventList.Any() ? eventList.Count : 0;
                KioskStatic.logToFile(this.Name + ": Register topup card reader device. Call back list count: " + listCount.ToString());
                log.Info(this.Name + ": Register topup card reader device. Call back list count: " + listCount.ToString());
            }
            log.LogMethodExit();
        }

        private void UnregisterCardReader()
        {
            log.LogMethodEntry();
            if (KioskStatic.TopUpReaderDevice != null)
            {
                KioskStatic.TopUpReaderDevice.UnRegister();
                List<EventHandler> eventList = (KioskStatic.TopUpReaderDevice != null ? KioskStatic.TopUpReaderDevice.GetCallBackList : new List<EventHandler>());
                int listCount = eventList != null && eventList.Any() ? eventList.Count : 0;
                KioskStatic.logToFile(this.Name + ": UnRegister topup card reader device. Call back list count: " + listCount.ToString());
                log.Info(this.Name + ": UnRegister topup card reader device. Call back list count: " + listCount.ToString());
            }
            log.LogMethodExit();
        }

        private void HandleCardRead(string inCardNumber, DeviceClass readerDevice)
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
                //197 - Technician Card (&1) not allowed for Transaction
                txtMessage.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 197, lclCard.CardNumber);
                throw new ValidationException(MessageContainerList.GetMessage(utilities.ExecutionContext, txtMessage.Text));
            }

            if (lclCard.CardStatus == "NEW")
            {
                if (utilities.ParafaitEnv.MIFARE_CARD)
                    message = MessageContainerList.GetMessage(utilities.ExecutionContext, 528);//Please insert an Issued Card...
                else
                    message = MessageContainerList.GetMessage(utilities.ExecutionContext, 459);//Please Tap an Issued Card...

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

        private void InitializeKeyboard()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
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
                KioskStatic.logToFile("Error Initializing keyboard in Customer Lookup screen: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void SetVirtualKeyboard()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            try
            {
                virtualKeyboardController = new VirtualWindowsKeyboardController(flpSearchOptions.Top);
                bool popupOnScreenKeyBoard = true;
                virtualKeyboardController.Initialize(this, new List<Control>() { btnShowKeyPad }, popupOnScreenKeyBoard);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error Initializing Windows Keyboard in Customer Lookup screen: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void SetCustomKeyboard()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            try
            {
                customKeyboardController = new VirtualKeyboardController(flpSearchOptions.Top);
                bool showKeyboardOnTextboxEntry = true;
                customKeyboardController.Initialize(this, new List<Control>() { btnShowKeyPad }, showKeyboardOnTextboxEntry, null, lblGreeting.Font.FontFamily.Name);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error Initializing Custom Keyboard in Customer Lookup screen: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void DisposeKeyboardObject()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
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
                KioskStatic.logToFile("Error Disposing keyboard in Customer Lookup screen: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void HideKeyBoard()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
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
                KioskStatic.logToFile("Error Disposing keyboard in Customer Lookup screen: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void EnableButtons()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            try
            {
                btnPrev.Enabled = true;
                btnSearch.Enabled = true;
                btnNewRegistration.Enabled = true;
                btnShowKeyPad.Enabled = true;
                lblGreeting.Enabled =
                     lblNewCustomerHeader.Enabled =
                     lblExistingCustomerHeader.Enabled =
                     lblNewCustomerMsg.Enabled =
                     lblExistingCustomerMsg.Enabled = true;
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
            ResetKioskTimer();
            try
            {
                btnPrev.Enabled = false;
                btnSearch.Enabled = false;
                btnNewRegistration.Enabled = false;
                btnShowKeyPad.Enabled = false;
                lblGreeting.Enabled =
                     lblNewCustomerHeader.Enabled =
                     lblExistingCustomerHeader.Enabled =
                     lblNewCustomerMsg.Enabled =
                     lblExistingCustomerMsg.Enabled = false;
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
            ResetKioskTimer();
            try
            {
                Control activeControl = this.ActiveControl;
                if (ActiveControl == null)
                {
                    this.ActiveControl = (pnlSearchViaPhone.Visible) ? txtPhoneNum as Control : txtEmailId as Control;
                }
                else
                {
                    ActiveControl = null;
                    ActiveControl = activeControl;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in SetFocus() in customer lookup screen: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void SetPhoneNumberFieldWidth()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            try
            {
                int width = 0;
                int.TryParse(ParafaitDefaultContainerList.GetParafaitDefault(KioskStatic.Utilities.ExecutionContext, "CUSTOMER_PHONE_NUMBER_WIDTH"), out width);
                if (width > 0)
                    txtPhoneNum.MaxLength = width;
                else
                    txtPhoneNum.MaxLength = 20;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in SetPhoneNumberFieldWidth(): in Search Customer Screen" + ex.Message);
            }
            log.LogMethodExit();
        }

        private bool ValidatePhoneNumberField()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            bool status = true;
            try
            {
                if (pnlSearchViaPhone.Visible == true && !string.IsNullOrWhiteSpace(txtPhoneNum.Text))
                {
                    int phoneNumberWidth = ParafaitDefaultContainerList.GetParafaitDefault<int>(utilities.ExecutionContext, "CUSTOMER_PHONE_NUMBER_WIDTH");
                    if (!System.Text.RegularExpressions.Regex.IsMatch(txtPhoneNum.Text, @"^[0-9]+$"))
                    {
                        status = false;
                    }
                    else if (phoneNumberWidth > 0)
                    {
                        if (txtPhoneNum.Text.Length != phoneNumberWidth)
                        {
                            status = false;
                        }

                        //validation for same number in all the places until phoneNumberWidth. Ex: 1111111111, 2222222222 et
                        for (int i = 0; i < 10; i++)
                        {
                            string match = new string(i.ToString()[0], phoneNumberWidth);
                            if (match.Equals(txtPhoneNum.Text))
                            {
                                status = false;
                            }
                        }
                    }

                    if(status == false)
                    {
                            HideKeyBoard();
                            string errMsg = MessageContainerList.GetMessage(utilities.ExecutionContext, 785); //Enter valid Phone No
                            txtMessage.Text = errMsg;
                            this.ActiveControl = txtPhoneNum;
                            ValidationException ve = new ValidationException(errMsg);
                            throw ve;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in IsValidPhoneNumber(): " + ex.Message);
                throw;
            }
            finally
            {
                SetFocus();
            }
            log.LogMethodExit(status);
            return status;
        }

        private bool ValidateEmailIdField()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            bool status = true;
            try
            {
                if (pnlSearchViaEmail.Visible == true && !string.IsNullOrWhiteSpace(txtEmailId.Text))
                {
                    if (!System.Text.RegularExpressions.Regex.IsMatch(txtEmailId.Text.Trim(), @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,})+)$"))// Changes made for the domain name size like .com ,.comm .ukcom etc
                    {
                        HideKeyBoard();
                        //572 - Enter a valid email id
                        string errMsg = MessageContainerList.GetMessage(utilities.ExecutionContext, 572);
                        status = false;
                        txtMessage.Text = errMsg;
                        this.ActiveControl = txtEmailId;
                        ValidationException ve = new ValidationException(errMsg);
                        throw ve;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in IsValidPhoneNumber(): " + ex.Message);
                throw;
            }
            log.LogMethodExit(status);
            return status;
        }

        private void frmFetchCustomer_Closed(object sender, FormClosedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (KioskStatic.TopUpReaderDevice != null)
            {
                UnregisterCardReader();
                StopKioskTimer();
            }
            DisposeKeyboardObject();
            KioskStatic.logToFile(this.Name + ": Form closed");
            log.LogMethodExit();
        }

        private void SetCustomiImages()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            try
            {
                this.BackgroundImage = ThemeManager.GetBackgroundImage(ThemeManager.CurrentThemeImages.DefaultBackgroundImage);
                btnNewRegistration.BackgroundImage =
                    btnSearch.BackgroundImage =
                    btnPrev.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage;
            }
            catch (Exception ex)
            {
                log.Error("Error while Setting Customized Fore Colors in Customer Lookup screen", ex);
                KioskStatic.logToFile("Error setting Custom Images in Customer Lookup screen: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            try
            {
                btnSearch.ForeColor = KioskStatic.CurrentTheme.SearchCustomerSearchBtnTextForeColor;
                btnNewRegistration.ForeColor = KioskStatic.CurrentTheme.SearchCustomerNewRegistrationBtnTextForeColor;
                btnPrev.ForeColor = KioskStatic.CurrentTheme.SearchCustomerPrevBtnTextForeColor;
                this.txtMessage.ForeColor = KioskStatic.CurrentTheme.SearchCustomerFooterTextForeColor;
                this.lblGreeting.ForeColor = KioskStatic.CurrentTheme.SearchCustomerGreetingForeColor;
                this.lblNewCustomerHeader.ForeColor = KioskStatic.CurrentTheme.SearchCustomerLblNewCustomerHeaderForeColor;
                this.lblNewCustomerMsg.ForeColor = KioskStatic.CurrentTheme.SearchCustomerLblNewCustomerForeColor;
                this.lblExistingCustomerHeader.ForeColor = KioskStatic.CurrentTheme.SearchCustomerExistingCustomerHeaderForeColor;
                this.lblExistingCustomerMsg.ForeColor = KioskStatic.CurrentTheme.SearchCustomerExistingCustomerForeColor;
                this.lblEnterPhoneHeader.ForeColor = KioskStatic.CurrentTheme.SearchCustomerLblEnterPhoneHeaderForeColor;
                this.lblEnterEmailHeader.ForeColor = KioskStatic.CurrentTheme.SearchCustomerLblEnterEmailHeaderForeColor;
                this.txtPhoneNum.ForeColor = KioskStatic.CurrentTheme.SearchCustomerTxtPhoneNumForeColor;
                this.txtEmailId.ForeColor = KioskStatic.CurrentTheme.SearchCustomerTxtEmailIdForeColor;
            }
            catch (Exception ex)
            {
                log.Error("Error while Setting Customized Fore Colors in customer lookup screen", ex);
                KioskStatic.logToFile("Error setting customized font colors in Customer Lookup screen: " + ex.Message);
            }
            log.LogMethodExit();
        }
    }
}
