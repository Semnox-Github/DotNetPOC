/********************************************************************************************
 * Project Name - Customer
 * Description  - user interface
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
*2.150.3.0   28-Apr-2023      Vignesh Bhat        Created: TableTop Kiosk Changes
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Customer;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.Product;

namespace Parafait_Kiosk
{
    public partial class Customer : BaseFormKiosk
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Card CurrentCard;
        public CustomerDTO customerDTO;
        private CustomerDTO inputCustomerDTO;
        Utilities Utilities = KioskStatic.Utilities;
        MessageUtils MessageUtils = KioskStatic.Utilities.MessageUtils;
        ParafaitEnv ParafaitEnv = KioskStatic.Utilities.ParafaitEnv;

        private readonly TagNumberParser tagNumberParser;

        const string WARNING = "WARNING";
        const string ERROR = "ERROR";
        const string MESSAGE = "MESSAGE";

        string ImageDirectory;
        DateTime _birthDate = DateTime.MinValue;

        Font savTimeOutFont;
        Font TimeOutFont;
        bool termsAndConditions;

        string countryId = null;
        bool relatedCustomer;
        bool maskLinkedRelations = true;
        private VirtualWindowsKeyboardController virtualKeyboardController;
        private VirtualKeyboardController customKeyboardController;
        private DateTime customerDOBValue;
        private DateTime customerAnniversaryValue;
        private System.Windows.Forms.Timer refreshTimer;

        public Customer(string CardNumber, object BirthDate = null, bool termsAndConditionsAgreed = false, CustomerDTO selectedCustomerDTO = null)
        {
            log.LogMethodEntry(CardNumber, BirthDate, termsAndConditionsAgreed, selectedCustomerDTO);
            Utilities.setLanguage();
            InitializeComponent();
            refreshTimer = new System.Windows.Forms.Timer();
            refreshTimer.Tick += new System.EventHandler(this.RefreshTimerTick);
            refreshTimer.Interval = 50;
            refreshTimer.Enabled = true;
            refreshTimer.Stop();
            lblTimeRemaining.Text = "";
            termsAndConditions = termsAndConditionsAgreed;

            KioskStatic.formatMessageLine(textBoxMessageLine, 21, ThemeManager.CurrentThemeImages.BottomMessageLineImage);  //Starts:Modification on 17-Dec-2015 for introducing new theme          
            KioskStatic.setDefaultFont(this);//Ends:Modification on 17-Dec-2015 for introducing new theme          

            savTimeOutFont = lblTimeRemaining.Font;
            TimeOutFont = lblTimeRemaining.Font = new System.Drawing.Font(lblTimeRemaining.Font.FontFamily, 50, FontStyle.Bold);
            SetKioskTimerTickValue(20);
            ResetKioskTimer();
            lblTimeRemaining.Text = GetKioskTimerTickValue().ToString("#0");

            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);

            if (string.IsNullOrEmpty(CardNumber) == false)
            {
                CurrentCard = new Card(CardNumber, "Kiosk", Utilities);
                txtCardNumber.Text = KioskHelper.GetMaskedCardNumber(CardNumber);
            }
            else
            {
                txtCardNumber.Parent.Visible = lblCardNumber.Visible = false;
            }
            if (selectedCustomerDTO != null)
            {
                this.inputCustomerDTO = selectedCustomerDTO;
                relatedCustomer = true;
            }

            //New Customer Registration without Card
            if (CardNumber == null && selectedCustomerDTO == null)
            {
                relatedCustomer = false;
            }

            //this.ShowInTaskbar = false;

            lblRegistrationMessage.Text = Utilities.MessageUtils.getMessage(803);
            textBoxMessageLine.Text = "";

            if (ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "SHOW_ADD_RELATION_IN_CUSTOMER_SCREEN") == "N")
            {
                btnAddRelation.Visible = false;
                PanelLinkedRelations.Visible = false;
            }
            else
            {
                if (relatedCustomer == false && !string.IsNullOrEmpty(CardNumber) && CurrentCard.customer_id != -1) //Parent customer
                {
                    btnAddRelation.Visible = true;
                    btnAddRelation.Enabled = true;
                    PopulateLinkedCustomerRelations();

                }
                else if (relatedCustomer == true) //adding related customer - the screen is invoked from frmAddCustomerRelation - More button
                {
                    btnAddRelation.Visible = false;
                    PanelLinkedRelations.Visible = false;
                }
                else
                {
                    btnAddRelation.Visible = true;
                    btnAddRelation.Enabled = false;
                    PanelLinkedRelations.Visible = false;
                }
            }

            //For new customer do not show Add Relation button until customer details are saved first
            if (CurrentCard != null && CurrentCard.customer_id == -1 && relatedCustomer == false)
            {
                btnAddRelation.Enabled = false;
            }

            this.BackgroundImage = ThemeManager.GetBackgroundImage(ThemeManager.CurrentThemeImages.CustomerBackgroundImage);
            btnHome.BackgroundImage = ThemeManager.CurrentThemeImages.HomeButton;
            PanelLinkedRelations.BackgroundImage = ThemeManager.CurrentThemeImages.KioskActivityTableImage;
            panelCardNumber.BackgroundImage = panelPin.BackgroundImage =
                panelEmail.BackgroundImage = panelContactPhone1.BackgroundImage = panelBirthDate.BackgroundImage =
                panelGender.BackgroundImage = panelTitle.BackgroundImage = panelFirstName.BackgroundImage =
                panelLastName.BackgroundImage = panelCountry.BackgroundImage = panelAddress1.BackgroundImage =
                panelAddress2.BackgroundImage = panelCity.BackgroundImage = panelState.BackgroundImage = ThemeManager.CurrentThemeImages.TextEntryBox;
            btnPrev.BackgroundImage = buttonCustomerSave.BackgroundImage = btnAddRelation.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage;
            lblTimeRemaining.BackgroundImage = ThemeManager.CurrentThemeImages.TimerBoxSmall;
            textBoxMessageLine.BackgroundImage = ThemeManager.CurrentThemeImages.BottomMessageLineImage;
            lblRegistrationMessage.Visible = false;//Ends:Modification on 17-Dec-2015 for introducing new theme
            KioskStatic.LoadPromotionModeDropDown(cmbPromotionMode);
            initializeCustomerInfo();
            lblOptinPromotions.Text = Utilities.MessageUtils.getMessage(1739);
            lblPromotionMode.Text = Utilities.MessageUtils.getMessage(1740);
            lblTermsAndConditions.Text = Utilities.MessageUtils.getMessage(1741) + "*";
            pnlOptinPromotions.Tag = false;
            pnlWhatsAppOptOut.Tag = false;
            //lblDateFormat.Text = KioskStatic.Utilities.getDateFormat().ToUpper();
            //lblDateFormat.Text = KioskStatic.DateFormat;
            //lblDateFormat.BringToFront();
            //lblDateTimeFormat.Text = KioskStatic.Utilities.getDateFormat().ToUpper();
            //lblDateTimeFormat.BringToFront();
            bigVerticalScrollCustomer.DownButtonBackgroundImage = bigVerticalScrollCustomer.DownButtonDisabledBackgroundImage = ThemeManager.CurrentThemeImages.ScrollDownButton;
            bigVerticalScrollCustomer.UpButtonBackgroundImage = bigVerticalScrollCustomer.UpButtonDisabledBackgroundImage = ThemeManager.CurrentThemeImages.ScrollUpButtonImage;
            //lblDateFormat.ForeColor = txtCardNumber.ForeColor;
            //lblDateTimeFormat.ForeColor = txtCardNumber.ForeColor;
            if (BirthDate != null)
                _birthDate = Convert.ToDateTime(BirthDate);

            tagNumberParser = new TagNumberParser(Utilities.ExecutionContext);
            SetDateTimeFormat();
            InitializeKeyboard();
            SetCustomizedFontColors();
            DisplaybtnPrev(true);
            log.LogMethodExit();
        }

        private void SetDateTimeFormat()
        {
            log.LogMethodEntry();
            lblBirthDateFormat.Text = lblAnniversaryDateFormat.Text = String.Empty;
            string dateMonthFormat = GetDateMonthFormat();
            txtBirthDate.Mask = dateMonthFormat.Replace("d", "0").Replace("MMM", ">LLL").Replace("MM", "00").Replace("y", "0");
            txtAnniversary.Mask = dateMonthFormat.Replace("d", "0").Replace("MMM", ">LLL").Replace("MM", "00").Replace("y", "0");
            lblBirthDateFormat.Text = "[" + DateTime.Now.ToString(dateMonthFormat) + "]";
            lblAnniversaryDateFormat.Text = "[" + DateTime.Now.ToString(dateMonthFormat) + "]";
            log.LogMethodExit();
        }

        private string GetDateMonthFormat()
        {
            log.LogMethodEntry();
            string dateFormat = ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "DATE_FORMAT");
            bool ignoreYear = ParafaitDefaultContainerList.GetParafaitDefault<bool>(Utilities.ExecutionContext, "IGNORE_CUSTOMER_BIRTH_YEAR");
            log.LogVariableState("ignoreYear", ignoreYear);
            if (ignoreYear)
            {
                if (dateFormat.StartsWith("Y", StringComparison.CurrentCultureIgnoreCase))
                {
                    dateFormat = dateFormat.TrimStart('y', 'Y');
                    dateFormat = dateFormat.Substring(1);
                }
                else
                {
                    int pos = dateFormat.IndexOf("Y", StringComparison.CurrentCultureIgnoreCase);
                    if (pos > 0)
                        dateFormat = dateFormat.Substring(0, pos - 1);
                }
            }
            log.LogMethodExit(dateFormat);
            return dateFormat;
        }

        private void displayCustomerDetails()
        {
            log.LogMethodEntry();
            System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;
            if ((CurrentCard == null || CurrentCard.customerDTO == null) && inputCustomerDTO == null)
            {
                txtFirstName.Clear();
                txtLastName.Clear();
                textBoxAddress1.Clear();
                textBoxAddress2.Clear();
                textBoxCity.Clear();
                textBoxPin.Clear();
                if (countryId != null && countryId != "-1")
                {
                    cmbCountry.SelectedValue = Convert.ToInt32(countryId);
                    cmbCountry.Enabled = false;
                }
                textBoxContactPhone1.Clear();
                textBoxEmail.Clear();
                if (_birthDate != DateTime.MinValue)
                {
                    txtBirthDate.Text = _birthDate.ToString(KioskStatic.DateMonthFormat, provider);
                    customerDOBValue = _birthDate;
                }
                else
                {
                    txtBirthDate.Clear();
                    customerDOBValue = DateTime.MinValue;
                }
                txtUniqueId.Clear();
                //lblDateTimeFormat.Visible = true;
                comboBoxGender.Text = "";
                if (cmbTitle.Visible)
                    cmbTitle.SelectedIndex = 0;
                pbCapture.Tag = "";
                pbCapture.Image = Properties.Resources.profile_picture_placeholder;

                displayMessageLine(MessageUtils.getMessage(446), MESSAGE);
            }
            else
            {
                CustomerDTO editCustomerDTO = null;
                if (CurrentCard != null && CurrentCard.customerDTO != null)
                {
                    editCustomerDTO = CurrentCard.customerDTO;
                }
                else if (inputCustomerDTO != null)
                {
                    editCustomerDTO = inputCustomerDTO;
                }
                txtFirstName.Text = editCustomerDTO.FirstName;
                if (cmbTitle.Visible)
                {
                    try
                    {
                        if (string.IsNullOrEmpty(editCustomerDTO.Title))
                            cmbTitle.SelectedIndex = 0;
                        else
                            cmbTitle.SelectedValue = editCustomerDTO.Title;
                    }
                    catch (Exception ex)
                    {
                        log.Error("Error occurred while executing displayCustomerDetails()", ex);
                        cmbTitle.SelectedIndex = 0;
                    }
                }
                txtLastName.Text = editCustomerDTO.LastName;
                if (editCustomerDTO.AddressDTOList != null && editCustomerDTO.AddressDTOList.Any())
                {
                    textBoxAddress1.Text = editCustomerDTO.AddressDTOList[0].Line1;
                    textBoxAddress2.Text = editCustomerDTO.AddressDTOList[0].Line2;
                    textBoxCity.Text = editCustomerDTO.AddressDTOList[0].City;
                    cmbState.SelectedValue = editCustomerDTO.AddressDTOList[0].StateId;
                    textBoxPin.Text = editCustomerDTO.AddressDTOList[0].PostalCode;
                }

                try
                {
                    if (countryId != null && countryId != "-1")
                    {
                        cmbCountry.SelectedValue = Convert.ToInt32(countryId);
                    }
                    else
                    {
                        cmbCountry.SelectedValue = editCustomerDTO.AddressDTOList[0].CountryId;
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while executing displayCustomerDetails()" + ex.Message);
                }
                textBoxContactPhone1.Text = editCustomerDTO.PhoneNumber;
                textBoxEmail.Text = editCustomerDTO.Email;
                pnlOptinPromotions.Tag = editCustomerDTO.OptInPromotions;
                pnlWhatsAppOptOut.Tag = editCustomerDTO.ProfileDTO.OptOutWhatsApp;
                txtUniqueId.Text = editCustomerDTO.UniqueIdentifier;
                if (editCustomerDTO.Anniversary != null && editCustomerDTO.Anniversary != DateTime.MinValue)
                {
                    txtAnniversary.Text = editCustomerDTO.Anniversary.Value.ToString(KioskStatic.DateMonthFormat, provider);
                    customerAnniversaryValue = (DateTime)editCustomerDTO.Anniversary;
                    //lblDateTimeFormat.Visible = false;
                }
                else
                {
                    txtAnniversary.Text = "";
                    customerAnniversaryValue = DateTime.MinValue;
                    //lblDateTimeFormat.Visible = true;
                }
                if (editCustomerDTO.OptInPromotions)
                {
                    btnOpinPromotions.Image = Properties.Resources.tick_box_checked;
                }
                else
                {
                    btnOpinPromotions.Image = Properties.Resources.tick_box_unchecked;
                }
                if (editCustomerDTO.OptInPromotionsMode == null)
                {
                    cmbPromotionMode.SelectedValue = string.Empty;
                }
                else
                {
                    cmbPromotionMode.SelectedValue = editCustomerDTO.OptInPromotionsMode;
                }
                termsAndConditions = editCustomerDTO.PolicyTermsAccepted;
                if (!termsAndConditions && Utilities.getParafaitDefaults("TERMS_AND_CONDITIONS").Equals("M"))
                {
                    pnlTermsandConditions.Visible = true;
                    flpCustomerMain.Controls.Add(pnlTermsandConditions);
                }
                else
                {
                    pnlTermsandConditions.Visible = false;
                }
                if (editCustomerDTO.DateOfBirth != null && editCustomerDTO.DateOfBirth != DateTime.MinValue)
                {
                    txtBirthDate.Text = editCustomerDTO.DateOfBirth.Value.ToString(KioskStatic.DateMonthFormat, provider);
                    customerDOBValue = (DateTime)editCustomerDTO.DateOfBirth;
                }
                else
                {
                    txtBirthDate.Text = "";
                    customerDOBValue = DateTime.MinValue;
                }

                switch (editCustomerDTO.Gender)
                {
                    case "M": comboBoxGender.Text = "Male"; break;
                    case "F": comboBoxGender.Text = "Female"; break;
                    case "N": comboBoxGender.Text = "Not Set"; break;
                    default: comboBoxGender.Text = "Not Set"; break;
                }

                pbCapture.Tag = editCustomerDTO.PhotoURL;

                if (editCustomerDTO.PhotoURL != null && editCustomerDTO.PhotoURL.Trim() != "")
                {
                    SqlCommand cmdImage = Utilities.getCommand();
                    cmdImage.CommandText = "exec ReadBinaryDataFromFile @FileName";

                    cmdImage.Parameters.AddWithValue("@FileName", ImageDirectory + editCustomerDTO.PhotoURL);
                    try
                    {
                        object o = cmdImage.ExecuteScalar();
                        pbCapture.Image = Utilities.ConvertToImage(o);
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        pbCapture.Image = null;
                    }
                }
                else
                {
                    pbCapture.Image = Properties.Resources.profile_picture_placeholder;
                }

                CustomAttributes.PopulateData(editCustomerDTO.Id, editCustomerDTO.CustomDataSetId);
            }
            //flpCustomerMain.VerticalScroll.Value = 0;
            flpCustomerMain.HorizontalScroll.Visible = false;
            flpCustomerMain.Refresh();
            ResetKioskTimer();
            log.LogMethodExit();
        }

        void maskDisplay(bool inAllowEdit)
        {
            log.LogMethodEntry(inAllowEdit);
            if ((CurrentCard != null && CurrentCard.customerDTO != null) || inputCustomerDTO != null)// already registered customer
            {
                bool allowedit = Utilities.getParafaitDefaults("ALLOW_CUSTOMER_INFO_EDIT").Equals("Y");

                inAllowEdit &= allowedit;
                if (inAllowEdit)
                {
                    maskLinkedRelations = false;
                    txtFirstName.ReadOnly = false;

                    foreach (Control c in flpCustomerMain.Controls)
                    {
                        string type = c.GetType().ToString().ToLower();

                        Control cc = null;
                        if (type.Contains("panel") && c.Controls.Count > 0)
                        {
                            cc = c.Controls[0];
                            if (cc != null && (cc.Name.Equals("lblDateFormat") || cc.Name.Equals("lblDateTimeFormat")))
                            {
                                cc = c.Controls[1];
                            }
                            if (cc != null)
                            {
                                cc.Enabled = true;
                            }
                        }
                    }

                    foreach (Control c in flpCustomAttributes.Controls)
                    {
                        string type = c.GetType().ToString().ToLower();
                        if (type.Contains("label"))
                            continue;

                        c.Enabled = true;
                    }

                    txtBirthDate.PasswordChar =
                    txtAnniversary.PasswordChar =
                    txtUniqueId.PasswordChar =
                    textBoxEmail.PasswordChar =
                    textBoxContactPhone1.PasswordChar = '\0';

                    if (string.IsNullOrEmpty(txtAnniversary.Text.Replace(" ", "").Replace("-", "").Replace("/", "")))
                        //lblDateTimeFormat.Visible = true;
                        if (countryId != null && countryId != "-1")
                            cmbCountry.Enabled = false;
                    buttonCustomerSave.Enabled = true;
                    pnlOptinPromotions.Enabled = pnlTermsandConditions.Enabled = pnlWhatsAppOptOut.Enabled = inAllowEdit;
                    ResetKioskTimer();
                }
                else
                {
                    maskLinkedRelations = true;
                    txtFirstName.ReadOnly = true;

                    foreach (Control c in flpCustomerMain.Controls)
                    {
                        string type = c.GetType().ToString().ToLower();
                        Control cc = null;
                        if (type.Contains("panel") && c.Controls.Count > 0)
                            cc = c.Controls[0];
                        if (cc != null && (cc.Name.Equals("lblDateFormat") || cc.Name.Equals("lblDateTimeFormat") || (cc.Name.Equals("dOBDataGridViewTextBoxColumn"))))
                        {
                            cc = c.Controls[1];
                        }
                        if (cc != null)
                        {
                            cc.Enabled = false;
                        }
                    }

                    foreach (Control c in flpCustomAttributes.Controls)
                    {
                        string type = c.GetType().ToString().ToLower();
                        if (type.Contains("label"))
                            continue;

                        c.Enabled = false;
                    }

                    txtBirthDate.PasswordChar =
                    textBoxEmail.PasswordChar =
                    txtAnniversary.PasswordChar =
                    txtUniqueId.PasswordChar =
                    textBoxContactPhone1.PasswordChar = 'X';
                    //lblDateTimeFormat.Visible = false;
                    pnlOptinPromotions.Enabled = pnlTermsandConditions.Enabled = pnlWhatsAppOptOut.Enabled = inAllowEdit;
                    buttonCustomerSave.Enabled = false;
                    ResetKioskTimer();
                }

                if (allowedit == false)
                {
                    displayMessageLine(MessageUtils.getMessage(447), WARNING);
                }
                else
                {
                    if (inAllowEdit)
                    {
                        displayMessageLine(MessageUtils.getMessage(501), MESSAGE);
                    }
                    else
                    {
                        if (KioskStatic.Utilities.ParafaitEnv.MIFARE_CARD)
                            displayMessageLine(MessageUtils.getMessage(929), WARNING);
                        else
                            displayMessageLine(MessageUtils.getMessage(928), WARNING);
                    }
                }
            }
            log.LogMethodExit();
        }

        void displayMessageLine(string message, string msgType)
        {
            log.LogMethodEntry(message, msgType);
            textBoxMessageLine.Text = message;
            ResetKioskTimer();
            log.LogMethodExit();
        }

        private void buttonCustomerSave_Click(object sender, EventArgs e)
        {
            //secondsRemaining = timeOutSecs;
            log.LogMethodEntry();
            bool displayErrorInPopup = true;
            frmOKMsg frmOK;
            txtFirstName.BackColor = txtCardNumber.BackColor;
            txtLastName.BackColor = txtCardNumber.BackColor; //11-Jun-2015:: Make color as default background for Last name
            textBoxContactPhone1.BackColor = textBoxEmail.BackColor = cmbPromotionMode.BackColor = txtCardNumber.BackColor;

            if (string.IsNullOrEmpty(txtFirstName.Text.Trim()))
            {
                displayMessageLine(MessageUtils.getMessage(448), ERROR);
                if (displayErrorInPopup)
                {
                    StopKioskTimer();
                    using (frmOK = new frmOKMsg(MessageUtils.getMessage(448), true))
                    {
                        frmOK.ShowDialog();
                    }
                    ResetKioskTimer();
                    StartKioskTimer();
                }
                txtFirstName.BackColor = Color.OrangeRed;
                txtFirstName.Focus();
                log.LogMethodExit();
                return;
            }

            // Begin Modification - 24-Apr-2015
            // Added customer validation if validation configuration is set to Y. Name should be alphabets and more than 3 chars
            if (Utilities.getParafaitDefaults("CUSTOMER_NAME_VALIDATION") == "Y")
            {
                if (txtFirstName.Text.Length < 3)
                {
                    displayMessageLine(MessageUtils.getMessage(820, 3), ERROR);
                    if (displayErrorInPopup)
                    {
                        StopKioskTimer();
                        using (frmOK = new frmOKMsg(MessageUtils.getMessage(820, 3), true))
                        {
                            frmOK.ShowDialog();
                        }
                        ResetKioskTimer();
                        StartKioskTimer();
                    }
                    this.ActiveControl = txtFirstName;
                    txtFirstName.BackColor = Color.OrangeRed;
                    log.LogMethodExit();
                    return;
                }

                if (!System.Text.RegularExpressions.Regex.IsMatch(txtFirstName.Text, @"^[a-zA-Z]+$"))
                {
                    displayMessageLine(MessageUtils.getMessage(821), ERROR);
                    if (displayErrorInPopup)
                    {
                        StopKioskTimer();
                        using (frmOK = new frmOKMsg(MessageUtils.getMessage(821), true))
                        {
                            frmOK.ShowDialog();
                        }
                        ResetKioskTimer();
                        StartKioskTimer();
                    }
                    this.ActiveControl = txtFirstName;
                    txtFirstName.BackColor = Color.OrangeRed;
                    log.LogMethodExit();
                    return;
                }
                if (txtLastName.Text.Length > 0)//11-Jun-2015::Last Name validation if its filled
                {//Last Name should contain alphabets 
                    if (!System.Text.RegularExpressions.Regex.IsMatch(txtLastName.Text, @"^[a-zA-Z]+$"))
                    {
                        displayMessageLine(MessageUtils.getMessage(821), ERROR);
                        if (displayErrorInPopup)
                        {
                            StopKioskTimer();
                            using (frmOK = new frmOKMsg(MessageUtils.getMessage(821), true))
                            {
                                frmOK.ShowDialog();
                            }
                            ResetKioskTimer();
                            StartKioskTimer();
                        }
                        this.ActiveControl = txtLastName;
                        txtLastName.BackColor = Color.OrangeRed;
                        log.LogMethodExit();
                        return;
                    }
                }
            }
            //End Modification 24-Apr-2015

            if (CurrentCard == null)
            {
                if (inputCustomerDTO != null)
                {
                    customerDTO = inputCustomerDTO;
                }

                if (customerDTO == null) //editing once saved
                    customerDTO = new CustomerDTO();
            }
            else if (CurrentCard.customerDTO == null)
            {
                CurrentCard.customerDTO = customerDTO = new CustomerDTO();
            }
            else
            {
                customerDTO = CurrentCard.customerDTO;
            }
            //Begin Modification - 24 Apr 2015 - Email and Phone Number validations

            if (!string.IsNullOrEmpty(textBoxEmail.Text.Trim()))
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(textBoxEmail.Text.Trim(), @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,})+)$"))// Changes made for the domain name size like .com ,.comm .ukcom etc
                {
                    displayMessageLine(MessageUtils.getMessage(572), ERROR);
                    if (displayErrorInPopup)
                    {
                        StopKioskTimer();
                        using (frmOK = new frmOKMsg(MessageUtils.getMessage(572), true))
                        {
                            frmOK.ShowDialog();
                        }
                        ResetKioskTimer();
                        StartKioskTimer();
                    }
                    this.ActiveControl = textBoxEmail;
                    textBoxEmail.BackColor = Color.OrangeRed;
                    log.LogMethodExit();
                    return;
                }
            }

            textBoxContactPhone1.Text = textBoxContactPhone1.Text.Trim();
            if (!string.IsNullOrEmpty(textBoxContactPhone1.Text))
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(textBoxContactPhone1.Text, @"^[0-9]+$"))
                {
                    displayMessageLine(MessageUtils.getMessage(785), ERROR);
                    if (displayErrorInPopup)
                    {
                        StopKioskTimer();
                        using (frmOK = new frmOKMsg(MessageUtils.getMessage(785), true))
                        {
                            frmOK.ShowDialog();
                        }
                        ResetKioskTimer();
                        StartKioskTimer();
                    }
                    this.ActiveControl = textBoxContactPhone1;
                    textBoxContactPhone1.BackColor = Color.OrangeRed;
                    log.LogMethodExit();
                    return;
                }

                int width = textBoxContactPhone1.MaxLength;

                if (width != 20) // specific width specified
                {
                    if (textBoxContactPhone1.Text.Length != width)
                    {
                        displayMessageLine(MessageUtils.getMessage(785), ERROR);
                        if (displayErrorInPopup)
                        {
                            StopKioskTimer();
                            using (frmOK = new frmOKMsg(MessageUtils.getMessage(785), true))
                            {
                                frmOK.ShowDialog();
                            }
                            ResetKioskTimer();
                            StartKioskTimer();
                        }
                        this.ActiveControl = textBoxContactPhone1;
                        textBoxContactPhone1.BackColor = Color.OrangeRed;
                        log.LogMethodExit();
                        return;
                    }

                    for (int i = 0; i < 10; i++)
                    {
                        string match = new string(i.ToString()[0], width);
                        if (match.Equals(textBoxContactPhone1.Text))
                        {
                            displayMessageLine(MessageUtils.getMessage(785), ERROR);
                            if (displayErrorInPopup)
                            {
                                StopKioskTimer();
                                frmOKMsg.ShowUserMessage(MessageUtils.getMessage(785));
                                ResetKioskTimer();
                                StartKioskTimer();
                            }
                            this.ActiveControl = textBoxContactPhone1;
                            textBoxContactPhone1.BackColor = Color.OrangeRed;
                            log.LogMethodExit();
                            return;
                        }
                    }
                }
            }

            //End Modification 24-Apr-2015. Phone and Email validations
            txtBirthDate.BackColor = txtCardNumber.BackColor;
            if (!string.IsNullOrEmpty(txtBirthDate.Text.Replace(" ", "").Replace("-", "").Replace("/", "")))
            {
                string dateMonthformat = KioskStatic.DateMonthFormat;
                System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;
                try
                {
                    DateTime datofBirthValue = customerDOBValue;
                    if (datofBirthValue != customerDTO.DateOfBirth)
                    {
                        customerDTO.DateOfBirth = datofBirthValue;
                    }
                    if (customerDTO.Id <= -1)
                    {
                        if (datofBirthValue != null && datofBirthValue != DateTime.MinValue)
                        {
                            decimal age = KioskHelper.GetAge(datofBirthValue.ToString());
                            decimal thresholdAgeOfChild = ParafaitDefaultContainerList.GetParafaitDefault<decimal>(KioskStatic.Utilities.ExecutionContext, "THRESHOLD_AGE_CHECK_IN_CHILD_SCREEN", -1);

                            if (age <= thresholdAgeOfChild)
                            {
                                customerDTO.CustomerType = CustomerType.UNREGISTERED;
                            }
                        }
                    }
                    DateTime dateofBirthValue = DateTime.MinValue;
                    System.Globalization.CultureInfo providerValue = System.Globalization.CultureInfo.CurrentCulture;
                    string dateMonthformatValue = KioskStatic.DateMonthFormat;
                    DateTime newDOB = DateTime.ParseExact(txtBirthDate.Text, dateMonthformatValue, providerValue);
                    try
                    {
                        dateofBirthValue = KioskHelper.GetFormatedDateValue(newDOB);
                        if (dateofBirthValue != customerDTO.DateOfBirth)
                        {
                            customerDTO.DateOfBirth = dateofBirthValue;
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        try
                        {
                            dateofBirthValue = Convert.ToDateTime(newDOB, providerValue);
                            if (dateofBirthValue != customerDTO.DateOfBirth)
                            {
                                customerDTO.DateOfBirth = dateofBirthValue;
                            }
                        }
                        catch (Exception exp)
                        {
                            displayMessageLine(MessageContainerList.GetMessage(Utilities.ExecutionContext, 449, dateMonthformatValue, Convert.ToDateTime("23-Feb-1982").ToString(dateMonthformatValue)), ERROR);
                            log.Error(exp);
                            log.Error(MessageContainerList.GetMessage(Utilities.ExecutionContext, 449, dateMonthformatValue, Convert.ToDateTime("23-Feb-1982").ToString(dateMonthformatValue)));
                        }
                    }
                    if (DateTime.Compare(customerDTO.DateOfBirth.Value, Utilities.getServerTime()) > 0)
                    {
                        string errMsg = MessageContainerList.GetMessage(Utilities.ExecutionContext, 4809); //ERROR: Invalid date of birth
                        displayMessageLine(errMsg, ERROR);
                        if (displayErrorInPopup)
                        {
                            StopKioskTimer();
                            frmOKMsg.ShowUserMessage(errMsg);
                            ResetKioskTimer();
                            StartKioskTimer();
                        }
                        txtBirthDate.Focus();
                        txtBirthDate.BackColor = Color.OrangeRed;
                        log.LogMethodExit();
                        return;
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    string errMsg = MessageContainerList.GetMessage(Utilities.ExecutionContext, 4809); //ERROR: Invalid date of birth
                    displayMessageLine(errMsg, ERROR);
                    if (displayErrorInPopup)
                    {
                        StopKioskTimer();
                        frmOKMsg.ShowUserMessage(errMsg);
                        ResetKioskTimer();
                        StartKioskTimer();
                    }
                    txtBirthDate.Focus();
                    txtBirthDate.BackColor = Color.OrangeRed;
                    log.LogMethodExit();
                    return;
                }
            }
            else
                customerDTO.DateOfBirth = null;

            if (!string.IsNullOrEmpty(txtUniqueId.Text.ToString()))
            {
                customerDTO.UniqueIdentifier = txtUniqueId.Text.ToString();
            }
            customerDTO.PolicyTermsAccepted = termsAndConditions;
            customerDTO.OptInPromotions = Convert.ToBoolean(pnlOptinPromotions.Tag);
            customerDTO.ProfileDTO.OptOutWhatsApp = Convert.ToBoolean(pnlWhatsAppOptOut.Tag);
            customerDTO.OptInPromotionsMode = cmbPromotionMode.SelectedValue.ToString();

            textBoxEmail.BackColor = txtCardNumber.BackColor;
            if (!string.IsNullOrEmpty(textBoxEmail.Text))
            {
                if (!KioskStatic.check_mail(textBoxEmail.Text.Trim()))
                {
                    displayMessageLine(MessageUtils.getMessage(450), ERROR);
                    if (displayErrorInPopup)
                    {
                        StopKioskTimer();
                        frmOKMsg.ShowUserMessage(MessageUtils.getMessage(450));
                        ResetKioskTimer();
                        StartKioskTimer();
                    }
                    textBoxEmail.Focus();
                    textBoxEmail.BackColor = Color.OrangeRed;
                    log.LogMethodExit();
                    return;
                }
            }

            Label label = new Label();
            label.Text = "Field";
            foreach (Control cp in flpCustomerMain.Controls)
            {
                Control c;
                if (cp.GetType().ToString().Contains("Panel"))
                    c = cp.Controls[0];
                else
                {
                    label = cp as Label;
                    continue;
                }

                if (c.Tag == null)
                    continue;

                if (c.Tag.ToString() == "M")
                {
                    c.BackColor = txtCardNumber.BackColor;
                    if (c.Name == "cmbTitle")
                    {
                        if ((c as ComboBox).SelectedIndex <= 0)
                        {
                            displayMessageLine(MessageUtils.getMessage(249, lblTitle.Text.TrimEnd(':', '*')), ERROR);
                            if (displayErrorInPopup)
                            {
                                StopKioskTimer();
                                using (frmOK = new frmOKMsg(MessageUtils.getMessage(249, lblTitle.Text.TrimEnd(':', '*')), true))
                                {
                                    frmOK.ShowDialog();
                                }
                                ResetKioskTimer();
                                StartKioskTimer();
                            }
                            this.ActiveControl = c;
                            c.BackColor = Color.OrangeRed;
                            log.LogMethodExit();
                            return;
                        }
                    }
                    else if (c.Name == "comboBoxGender")
                    {
                        if ((c as ComboBox).SelectedIndex > 1)
                        {
                            displayMessageLine(MessageUtils.getMessage(250), ERROR);
                            if (displayErrorInPopup)
                            {
                                StopKioskTimer();
                                using (frmOK = new frmOKMsg(MessageUtils.getMessage(250), true))
                                {
                                    frmOK.ShowDialog();
                                }
                                ResetKioskTimer();
                                StartKioskTimer();
                            }
                            this.ActiveControl = c;
                            log.LogMethodExit();
                            return;
                        }
                    }

                    else if (c.Name == "cmbState")
                    {
                        if ((c as ComboBox).SelectedIndex <= 0)
                        {
                            displayMessageLine(MessageUtils.getMessage(249, lblStateCombo.Text.TrimEnd(':', '*')), ERROR);
                            if (displayErrorInPopup)
                            {
                                StopKioskTimer();
                                using (frmOK = new frmOKMsg(MessageUtils.getMessage(249, lblStateCombo.Text.TrimEnd(':', '*')), true))
                                {
                                    frmOK.ShowDialog();
                                }
                                ResetKioskTimer();
                                StartKioskTimer();
                            }
                            this.ActiveControl = c;
                            c.BackColor = Color.OrangeRed;
                            log.LogMethodExit();
                            return;
                        }
                    }
                    else if (c.Name == "cmbCountry")
                    {
                        if ((c as ComboBox).SelectedIndex <= 0)
                        {
                            displayMessageLine(MessageUtils.getMessage(249, lblCountry.Text.TrimEnd(':', '*')), ERROR);
                            if (displayErrorInPopup)
                            {
                                StopKioskTimer();
                                using (frmOK = new frmOKMsg(MessageUtils.getMessage(249, lblCountry.Text.TrimEnd(':', '*')), true))
                                {
                                    frmOK.ShowDialog();
                                }
                                ResetKioskTimer();
                                StartKioskTimer();
                            }
                            this.ActiveControl = c;
                            c.BackColor = Color.OrangeRed;
                            log.LogMethodExit();
                            return;
                        }
                    }
                    else if (c.Name == "btnOpinPromotions")
                    {
                        if (pnlOptinPromotions.Tag.Equals(false))
                        {
                            displayMessageLine(MessageUtils.getMessage(249, lblOptinPromotions.Text.TrimEnd(':', '*')), ERROR);
                            if (displayErrorInPopup)
                            {
                                StopKioskTimer();
                                using (frmOK = new frmOKMsg(MessageUtils.getMessage(249, lblOptinPromotions.Text.TrimEnd(':', '*')), true))
                                {
                                    frmOK.ShowDialog();
                                }
                                ResetKioskTimer();
                                StartKioskTimer();
                            }
                            this.ActiveControl = c;
                            c.BackColor = Color.OrangeRed;
                            log.LogMethodExit();
                            return;
                        }
                    }

                    else if (c.Name == "pnlWhatsAppOptin")
                    {
                        if (pnlWhatsAppOptOut.Tag.Equals(false))
                        {
                            displayMessageLine(MessageUtils.getMessage(249, pnlWhatsAppOptOut.Text.TrimEnd(':', '*')), ERROR);
                            if (displayErrorInPopup)
                            {
                                StopKioskTimer();
                                using (frmOK = new frmOKMsg(MessageUtils.getMessage(249, pnlWhatsAppOptOut.Text.TrimEnd(':', '*')), true))
                                {
                                    frmOK.ShowDialog();
                                }
                                ResetKioskTimer();
                                StartKioskTimer();
                            }
                            this.ActiveControl = c;
                            c.BackColor = Color.OrangeRed;
                            log.LogMethodExit();
                            return;
                        }
                    }
                    else if ((string.IsNullOrEmpty(c.Text.Trim())) && !(c.Name.Equals("flpEditDateOfBirth")))
                    {
                        displayMessageLine(MessageUtils.getMessage(249, label.Text.TrimEnd(':', '*')), ERROR);
                        if (displayErrorInPopup)
                        {
                            StopKioskTimer();
                            using (frmOK = new frmOKMsg(MessageUtils.getMessage(249, label.Text.TrimEnd(':', '*')), true))
                            {
                                frmOK.ShowDialog();
                            }
                            ResetKioskTimer();
                            StartKioskTimer();
                        }
                        this.ActiveControl = c;
                        c.BackColor = Color.OrangeRed;
                        log.LogMethodExit();
                        return;
                    }
                }
            }

            foreach (Control c in flpCustomAttributes.Controls)
            {
                if (c.Tag == null)
                    continue;
                if (CustomAttributes.FieldDisplayOption(c.Tag) == "M")
                {
                    c.BackColor = txtCardNumber.BackColor;
                    string type = c.GetType().ToString();
                    if (type.Contains("TextBox") && string.IsNullOrEmpty(c.Text.Trim()))
                    {
                        displayMessageLine(MessageUtils.getMessage(249, flpCustomAttributes.GetNextControl(c, false).Text.TrimEnd(':', '*')), ERROR);
                        if (displayErrorInPopup)
                        {
                            StopKioskTimer();
                            using (frmOK = new frmOKMsg(MessageUtils.getMessage(249, flpCustomAttributes.GetNextControl(c, false).Text.TrimEnd(':', '*')), true))
                            {
                                frmOK.ShowDialog();
                            }
                            ResetKioskTimer();
                            StartKioskTimer();
                        }
                        this.ActiveControl = c;
                        c.BackColor = Color.OrangeRed;
                        log.LogMethodExit();
                        return;
                    }
                    else if (type.Contains("ComboBox") && (c as ComboBox).SelectedValue.Equals(DBNull.Value))
                    {
                        displayMessageLine(MessageUtils.getMessage(249, flpCustomAttributes.GetNextControl(c, false).Text.TrimEnd(':', '*')), ERROR);
                        if (displayErrorInPopup)
                        {
                            StopKioskTimer();
                            using (frmOK = new frmOKMsg(MessageUtils.getMessage(249, flpCustomAttributes.GetNextControl(c, false).Text.TrimEnd(':', '*')), true))
                            {
                                frmOK.ShowDialog();
                            }
                            ResetKioskTimer();
                            StartKioskTimer();
                        }
                        this.ActiveControl = c;
                        c.BackColor = Color.OrangeRed;
                        log.LogMethodExit();
                        return;
                    }
                }
            }

            if (pbCapture.Name == "M" && (pbCapture.Tag == null || pbCapture.Tag.ToString() == ""))
            {
                displayMessageLine(MessageUtils.getMessage(17), ERROR);
                if (displayErrorInPopup)
                {
                    StopKioskTimer();
                    using (frmOK = new frmOKMsg(MessageUtils.getMessage(17), true))
                    {
                        frmOK.ShowDialog();
                    }
                    ResetKioskTimer();
                    StartKioskTimer();
                }
                log.LogMethodExit();
                return;
            }


            AddressDTO addressDTO = null;
            if (!string.IsNullOrEmpty(textBoxAddress1.Text) || !string.IsNullOrEmpty(textBoxAddress2.Text)
                || !string.IsNullOrEmpty(cmbCountry.Text) || !string.IsNullOrEmpty(cmbState.Text)
                || !string.IsNullOrEmpty(textBoxPin.Text) || !string.IsNullOrEmpty(textBoxCity.Text))
            {
                if (customerDTO.AddressDTOList != null && customerDTO.AddressDTOList.Count > 0)
                {
                    var orderedAddressList = customerDTO.AddressDTOList.OrderByDescending((x) => x.LastUpdateDate);
                    addressDTO = orderedAddressList.First();
                }
                else
                {
                    addressDTO = new AddressDTO();
                    if (customerDTO.AddressDTOList == null)
                    {
                        customerDTO.AddressDTOList = new List<AddressDTO>();
                    }
                    customerDTO.AddressDTOList.Add(addressDTO);
                }
            }

            ContactDTO contactEmailDTO = null;
            if (!string.IsNullOrEmpty(textBoxEmail.Text))
            {
                if (customerDTO.ContactDTOList != null && customerDTO.ContactDTOList.Count > 0)
                {
                    var orderedContactList = customerDTO.ContactDTOList.OrderByDescending((x) => x.LastUpdateDate);
                    var orderedContactEmail = orderedContactList.Where(s => s.ContactType == ContactType.EMAIL);
                    if (orderedContactEmail.Count() > 0)
                    {
                        contactEmailDTO = orderedContactEmail.First();
                    }
                }
                if (contactEmailDTO == null)
                {
                    contactEmailDTO = new ContactDTO();
                    if (customerDTO.ContactDTOList == null)
                    {
                        customerDTO.ContactDTOList = new List<ContactDTO>();
                    }
                    contactEmailDTO.ContactType = ContactType.EMAIL;
                    customerDTO.ContactDTOList.Add(contactEmailDTO);
                }
            }

            ContactDTO contactPhone1DTO = null;
            if (!string.IsNullOrEmpty(textBoxContactPhone1.Text))
            {
                if (customerDTO.ContactDTOList != null && customerDTO.ContactDTOList.Count > 0)
                {
                    var orderedContactList = customerDTO.ContactDTOList.OrderByDescending((x) => x.LastUpdateDate);
                    var orderedContactPhone = orderedContactList.Where(s => s.ContactType == ContactType.PHONE);
                    if (orderedContactPhone.Count() > 0)
                    {
                        contactPhone1DTO = orderedContactPhone.First();
                    }
                }
                if (contactPhone1DTO == null)
                {
                    contactPhone1DTO = new ContactDTO();
                    if (customerDTO.ContactDTOList == null)
                    {
                        customerDTO.ContactDTOList = new List<ContactDTO>();
                    }
                    contactPhone1DTO.ContactType = ContactType.PHONE;
                    customerDTO.ContactDTOList.Add(contactPhone1DTO);
                }
            }

            if (!string.IsNullOrEmpty(textBoxAddress1.Text))
                addressDTO.Line1 = textBoxAddress1.Text;
            if (!string.IsNullOrEmpty(textBoxAddress2.Text))
                addressDTO.Line2 = textBoxAddress2.Text;
            customerDTO.FirstName = txtFirstName.Text;
            customerDTO.Title = (cmbTitle.SelectedIndex > -1 ? cmbTitle.SelectedValue.ToString() : "");
            customerDTO.LastName = txtLastName.Text;
            if (!string.IsNullOrEmpty(textBoxCity.Text))
                addressDTO.City = textBoxCity.Text;
            if ((cmbState.SelectedIndex) > -1 && (cmbState.SelectedValue != null && Convert.ToInt32(cmbState.SelectedValue) > -1))
                addressDTO.StateId = Convert.ToInt32(cmbState.SelectedValue);
            if ((cmbCountry.SelectedIndex) > -1)
                addressDTO.CountryId = Convert.ToInt32(cmbCountry.SelectedValue);
            if (!string.IsNullOrEmpty(textBoxPin.Text))
                addressDTO.PostalCode = textBoxPin.Text;
            if (!string.IsNullOrEmpty(textBoxContactPhone1.Text))
                contactPhone1DTO.Attribute1 = textBoxContactPhone1.Text;
            if (!string.IsNullOrEmpty(textBoxEmail.Text))
                contactEmailDTO.Attribute1 = textBoxEmail.Text.Trim().ToLower();
            if (!string.IsNullOrEmpty(txtAnniversary.Text.Replace(" ", "").Replace("-", "").Replace("/", "")))
            {
                DateTime anniversaryDate = DateTime.MinValue;
                string dateMonthformatValue = KioskStatic.DateMonthFormat;
                System.Globalization.CultureInfo providerValue = System.Globalization.CultureInfo.CurrentCulture;
                DateTime newDOB = DateTime.ParseExact(txtAnniversary.Text, dateMonthformatValue, providerValue);
                try
                {
                    anniversaryDate = KioskHelper.GetFormatedDateValue(newDOB);
                    if (anniversaryDate != customerDTO.Anniversary)
                    {
                        customerDTO.Anniversary = anniversaryDate;
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    try
                    {
                        anniversaryDate = Convert.ToDateTime(newDOB, providerValue);
                        if (anniversaryDate != customerDTO.Anniversary)
                        {
                            customerDTO.Anniversary = anniversaryDate;
                        }
                    }
                    catch (Exception exp)
                    {
                        displayMessageLine(MessageContainerList.GetMessage(Utilities.ExecutionContext, 449, dateMonthformatValue, Convert.ToDateTime("23-Feb-1982").ToString(dateMonthformatValue)), ERROR);
                        log.Error(exp);
                        log.Error(MessageContainerList.GetMessage(Utilities.ExecutionContext, 449, dateMonthformatValue, Convert.ToDateTime("23-Feb-1982").ToString(dateMonthformatValue)));
                    }
                }
            }
            if (customerDTO.PhotoURL != null && customerDTO.PhotoURL != string.Empty)
            {
                customerDTO.PhotoURL = pbCapture.Tag.ToString();
            }

            switch (comboBoxGender.Text)
            {
                case "Male": customerDTO.Gender = "M"; break;
                case "Female": customerDTO.Gender = "F"; break;
                case "Not Set": customerDTO.Gender = "N"; break;
                default: customerDTO.Gender = "N"; break;
            }
            if (KioskStatic.Utilities.getParafaitDefaults("TERMS_AND_CONDITIONS").Equals("M") && !termsAndConditions && pnlTermsandConditions.Tag.Equals("false"))
            {
                displayMessageLine(MessageUtils.getMessage(1206), ERROR);
                if (displayErrorInPopup)
                {
                    StopKioskTimer();
                    using (frmOK = new frmOKMsg(MessageUtils.getMessage(1206), true))
                    {
                        frmOK.ShowDialog();
                    }
                    ResetKioskTimer();
                    StartKioskTimer();
                    btnTermsandConditions.Focus();
                }
                log.LogMethodExit();
                return;
            }
            else if (KioskStatic.Utilities.getParafaitDefaults("TERMS_AND_CONDITIONS").Equals("M") && !termsAndConditions && pnlTermsandConditions.Tag.Equals(true))
            {
                customerDTO.PolicyTermsAccepted = true;
            }
            try
            {
                bool firstTimeRegistration = false;//12-Jun-2015 added for specific message for first time registration
                if (customerDTO.Id == -1 && customerDTO != null)
                {
                    int CustomDataSetId = customerDTO.CustomDataSetId;
                    if (CustomAttributes.Save((int)customerDTO.Id, ref CustomDataSetId))
                    {
                        customerDTO.CustomDataSetId = CustomDataSetId;
                        CustomDataSetDTO customDataSetDTO = (new CustomDataSetBL(Utilities.ExecutionContext, CustomDataSetId)).CustomDataSetDTO;
                        customerDTO.CustomDataSetDTO = customDataSetDTO;
                    }
                    SqlConnection sqlConnection = Utilities.createConnection();
                    SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
                    try
                    {
                        CustomerBL customerBL = new CustomerBL(Utilities.ExecutionContext, customerDTO);
                        customerBL.Save(sqlTransaction);
                        sqlTransaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        sqlTransaction.Rollback();
                        log.Error(ex);
                        throw;
                    }
                    finally
                    {
                        sqlConnection.Close();
                    }

                    if (CurrentCard != null)
                    {
                        CurrentCard.updateCustomer();
                    }
                    else
                    {
                        int loadRegProduct = -1;
                        Int32.TryParse(KioskStatic.Utilities.getParafaitDefaults("LOAD_PRODUCT_ON_REGISTRATION"), out loadRegProduct);
                        if (loadRegProduct != -1)
                        {
                            Products regProductBL = new Products(loadRegProduct);
                            ProductsDTO regProductDTO = regProductBL.GetProductsDTO;
                            if (regProductDTO.AutoGenerateCardNumber.Equals("Y"))
                            {
                                RandomTagNumber randomCardNumber = new RandomTagNumber(Utilities.ExecutionContext);

                                CurrentCard = new Semnox.Parafait.Transaction.Card(randomCardNumber.Value, Utilities.ParafaitEnv.LoginID, Utilities);
                                CurrentCard.primaryCard = "Y"; //Assign auto gen card as primary card
                                SqlConnection connection = Utilities.getConnection();
                                SqlTransaction pSqlTransaction = connection.BeginTransaction();
                                try
                                {
                                    CurrentCard.customerDTO = customerDTO;
                                    CurrentCard.createCard(pSqlTransaction);
                                    pSqlTransaction.Commit();
                                }
                                catch (Exception ex)
                                {
                                    pSqlTransaction.Rollback();
                                    log.Error(ex);
                                    displayMessageLine(MessageUtils.getMessage(253), WARNING);
                                    return;
                                }
                                finally
                                {
                                    connection.Close();
                                }
                            }
                        }
                    }
                    firstTimeRegistration = true;//12-Jun-2015 setting flag for displaying registration message
                    if (Utilities.getParafaitDefaults("ENABLE_KIOSK_CUSTOMER_VERIFICATION") == "Y")//Changes to add the customer verification
                    {
                        SendVerificationCode();
                    } //11-06-2015:starts
                }
                else // customer already exists. no need to update card
                {
                    int CustomDataSetId = customerDTO.CustomDataSetId;
                    if (CustomAttributes.Save((int)customerDTO.Id, ref CustomDataSetId))
                    {
                        customerDTO.CustomDataSetId = CustomDataSetId;
                        CustomDataSetDTO customDataSetDTO = (new CustomDataSetBL(Utilities.ExecutionContext, CustomDataSetId)).CustomDataSetDTO;
                        customerDTO.CustomDataSetDTO = customDataSetDTO;
                    }
                    SqlConnection sqlConnection = Utilities.createConnection();
                    SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
                    try
                    {
                        CustomerBL customerBL = new CustomerBL(Utilities.ExecutionContext, customerDTO);
                        customerBL.Save(sqlTransaction);
                        sqlTransaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        sqlTransaction.Rollback();
                        log.Error(ex);
                        log.LogMethodExit(null, "Throwing Exception " + ex.Message);
                        throw;
                    }
                    finally
                    {
                        sqlConnection.Close();
                    }
                    if (CurrentCard != null)
                        CurrentCard.updateCustomer();
                }

                if (CurrentCard != null && CurrentCard.vip_customer == 'N')
                {
                    CurrentCard.getTotalRechargeAmount();
                    if ((CurrentCard.credits_played >= ParafaitEnv.MINIMUM_SPEND_FOR_VIP_STATUS && ParafaitEnv.MINIMUM_SPEND_FOR_VIP_STATUS > 0)
                         || (CurrentCard.TotalRechargeAmount >= ParafaitEnv.MINIMUM_RECHARGE_FOR_VIP_STATUS && ParafaitEnv.MINIMUM_RECHARGE_FOR_VIP_STATUS > 0))
                    {
                        using (frmOKMsg fok = new frmOKMsg(MessageUtils.getMessage(451, KioskStatic.VIPTerm)))
                        {
                            fok.ShowDialog();
                        }
                    }
                }

                displayMessageLine(MessageUtils.getMessage(452), MESSAGE);

                if (relatedCustomer == false)
                {
                    //12-Jun-2015 - Custom message for first time registration
                    bool showAddRelationScreen = ParafaitDefaultContainerList.GetParafaitDefault<bool>(Utilities.ExecutionContext, "SHOW_ADD_RELATION_IN_CUSTOMER_SCREEN");
                    //12-Jun-2015 - Custom message for first time registration
                    frmYesNo frm = null;
                    if (firstTimeRegistration)
                    {
                        if (KioskStatic.RegistrationBonusAmount > 0 && CurrentCard != null &&
                            ((ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "REGISTRATION_BONUS_ON_VERIFICATION").Equals("N"))))
                        {
                            if (showAddRelationScreen)
                            {
                                //Congratulations! You have been awarded &1 Free Credits for registering. Your details saved. Do you want to continue editing?  Or Add Family?
                                frm = new frmYesNo(MessageContainerList.GetMessage(Utilities.ExecutionContext, 927, KioskStatic.RegistrationBonusAmount) + " " + MessageUtils.getMessage(453) + " Or "
                                                                                   + MessageUtils.getMessage(4123) + "?");
                            }
                            else
                            {
                                //Congratulations! You have been awarded &1 Free Credits for registering. Your details are saved.
                                frmOKMsg.ShowUserMessage(MessageContainerList.GetMessage(Utilities.ExecutionContext, 927, KioskStatic.RegistrationBonusAmount) + " " + MessageUtils.getMessage(5461));
                            }
                        }
                        else if (KioskStatic.RegistrationBonusAmount > 0 && CurrentCard != null &&
                            ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "REGISTRATION_BONUS_ON_VERIFICATION").Equals("Y"))
                        {
                            if (showAddRelationScreen)
                            {
                                //Please go to the nearest POS Counter and verify customer to get the registration credits. Your details saved. Do you want to continue editing?  Or Add Family?
                                frm = new frmYesNo(MessageContainerList.GetMessage(Utilities.ExecutionContext, 1746, KioskStatic.RegistrationBonusAmount) + " " + MessageUtils.getMessage(453) + " Or "
                                                                                   + MessageUtils.getMessage(4123) + "?");
                            }
                            else
                            {
                                //Please go to the nearest POS Counter and verify customer to get the registration credits. Your details are saved.
                                frmOKMsg.ShowUserMessage(MessageContainerList.GetMessage(Utilities.ExecutionContext, 1746, KioskStatic.RegistrationBonusAmount) + " " + MessageUtils.getMessage(5461));
                            }
                        }
                        else
                        {
                            if (showAddRelationScreen)
                            {
                                //Thank you for registering. Your details saved. Do you want to continue editing? Or Add Family?
                                frm = new frmYesNo(MessageContainerList.GetMessage(Utilities.ExecutionContext, MessageUtils.getMessage(926)
                                                                                       + " " + MessageUtils.getMessage(453) + " Or "
                                                                                       + MessageUtils.getMessage(4123) + "?"));
                            }
                            else
                            {
                                //Thank you for registering. Your details are saved.
                                frmOKMsg.ShowUserMessage(MessageContainerList.GetMessage(Utilities.ExecutionContext, MessageUtils.getMessage(926)
                                                                                        + " " + MessageUtils.getMessage(5461)));
                            }
                        }
                        firstTimeRegistration = false;
                    }
                    else
                    {
                        if (showAddRelationScreen)
                        {
                            //Your details saved. Do you want to continue editing? Or Add Family?
                            frm = new frmYesNo(MessageContainerList.GetMessage(Utilities.ExecutionContext, MessageUtils.getMessage(453) + " Or "
                                                                                   + MessageUtils.getMessage(4123) + "?"));
                        }
                        else
                        {
                            //Your details are saved. 
                            frmOKMsg.ShowUserMessage(MessageUtils.getMessage(5461));
                        }
                    }
                    DialogResult dr = (frm != null) ? frm.ShowDialog() : DialogResult.No;
                    if (dr == System.Windows.Forms.DialogResult.No)
                    {
                        DialogResult = DialogResult.OK;
                        Close();
                    }
                    else
                    {
                        maskDisplay(false);

                        if (KioskStatic.Utilities.ParafaitEnv.MIFARE_CARD && KioskStatic.cardAcceptor != null)
                        {
                            KioskStatic.cardAcceptor.AllowAllCards();
                        }
                        btnAddRelation.Enabled = true;
                        btnAddRelation.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage;
                    }
                    if (frm != null)
                    {
                        frm.Dispose();
                        frm = null;
                    }
                }
                else
                {
                    DialogResult = DialogResult.OK;
                    Close();
                }
                flpCustomerMain.VerticalScroll.Value = 0;
            }
            catch (ValidationException vx)
            {
                log.Error(vx);
                if (vx.ValidationErrorList != null && vx.ValidationErrorList.Count > 0)
                {
                    displayMessageLine(MessageContainerList.GetMessage(Utilities.ExecutionContext, 255, vx.ValidationErrorList[0].Message), ERROR);
                    if (displayErrorInPopup)
                    {
                        StopKioskTimer();
                        using (frmOK = new frmOKMsg(MessageContainerList.GetMessage(Utilities.ExecutionContext, 255, vx.ValidationErrorList[0].Message), true))
                        {
                            frmOK.ShowDialog();
                        }
                        ResetKioskTimer();
                        StartKioskTimer();
                    }
                    foreach (Control c in flpCustomerMain.Controls)
                    {
                        foreach (ValidationError vr in vx.ValidationErrorList)
                        {
                            if (c.Tag != null && c.Tag.Equals(vr.FieldName))
                            {
                                if (c.HasChildren)
                                {
                                    c.Controls[0].BackColor = Color.OrangeRed;
                                }
                                else
                                {
                                    c.BackColor = Color.OrangeRed;
                                }
                                continue;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                displayMessageLine(MessageContainerList.GetMessage(Utilities.ExecutionContext, 255, ex.Message), ERROR);
                if (displayErrorInPopup)
                {
                    StopKioskTimer();
                    using (frmOK = new frmOKMsg(MessageContainerList.GetMessage(Utilities.ExecutionContext, 255, ex.Message), true))
                    {
                        frmOK.ShowDialog();
                    }
                    ResetKioskTimer();
                    StartKioskTimer();
                }
            }
            log.LogMethodExit();
        }


        void SendVerificationCode() //17-Jun-2015:Method for sending verification code
        {
            //sends mail and/or SMS to the customer
            log.LogMethodEntry();
            try
            {
                this.Cursor = Cursors.WaitCursor;
                string Code;
                if (!string.IsNullOrEmpty(customerDTO.Email) || !string.IsNullOrEmpty(customerDTO.PhoneNumber))
                {
                    CustomerVerificationBL customerVerificationBL = new CustomerVerificationBL(Utilities.ExecutionContext);
                    customerVerificationBL.GenerateVerificationRecord(customerDTO.Id, Utilities.ParafaitEnv.LoginID, Utilities.ParafaitEnv.POSMachine, true);
                    Code = customerVerificationBL.CustomerVerificationDTO.VerificationCode;
                }

                else
                {
                    log.LogMethodExit();
                    return;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing SendVerificationCode()", ex);
            }
            finally
            {
                this.Cursor = Cursors.Default;
                ResetKioskTimer();
            }

            log.LogMethodExit();
        } //11-06-2015:Ends

        string generateCode() //11-06-2015:starts
        {
            log.LogMethodEntry();
            //Gererates the code and save into customerVerification Table
            string code = Utilities.GenerateRandomNumber(6, Utilities.RandomNumberType.Numeric);
            CustomerVerificationDTO customerVerificationDTO = new CustomerVerificationDTO();
            customerVerificationDTO.Source = "POS:" + Utilities.ParafaitEnv.POSMachine;
            customerVerificationDTO.VerificationCode = code;
            customerVerificationDTO.ProfileDTO = customerDTO.ProfileDTO;
            CustomerVerificationBL customerVerificationBL = new CustomerVerificationBL(Utilities.ExecutionContext, customerVerificationDTO);
            customerVerificationBL.Save();
            log.LogMethodExit(code);
            return code;
        } //11-06-2015:Ends

        private void pbCheckBox_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            ((sender as Control).Tag as CheckBox).Checked = !((sender as Control).Tag as CheckBox).Checked;
            log.LogMethodExit();
        }

        CustomAttributes CustomAttributes;
        void initializeCustomerInfo()
        {
            log.LogMethodEntry();
            CustomAttributes = new CustomAttributes(CustomAttributes.Applicability.CUSTOMER, Utilities);
            CustomAttributes.createUI(flpCustomAttributes);
            flpCustomAttributes.MinimumSize = new Size(0, 0);
            flpCustomAttributes.Height = 0;
            List<Control> addedControls = new List<Control>();
            List<int> addAtIndex = new List<int>();
            foreach (Control c in flpCustomAttributes.Controls)
            {
                string type = c.GetType().ToString();
                if (type.Contains("Label"))
                {
                    (c as Label).AutoSize = false;
                    c.Size = lblCardNumber.Size;
                    (c as Label).TextAlign = ContentAlignment.MiddleRight;
                    c.Margin = lblEmail.Margin;
                    c.Font = lblEmail.Font;
                }
                else if (type.Contains("TextBox") || type.Contains("Combo"))
                {
                    c.Font = txtFirstName.Font;
                    if (type.Contains("Combo"))
                        c.Font = new System.Drawing.Font(c.Font.FontFamily, c.Font.Size - 3, c.Font.Style);
                    c.Width = txtFirstName.Width;
                    c.Margin = txtFirstName.Margin;
                    c.BackColor = txtFirstName.BackColor;
                    c.ForeColor = System.Drawing.Color.DarkOrchid;
                    if (type.Contains("TextBox"))
                        (c as TextBox).BorderStyle = txtFirstName.BorderStyle;
                }
                else if (type.Contains("Radio"))// radio button
                {
                    RadioButton rb = (c as RadioButton);
                    rb.AutoSize = false;
                    rb.TextAlign = ContentAlignment.MiddleCenter;
                    rb.Appearance = Appearance.Button;
                    rb.FlatAppearance.BorderSize = 0;
                    rb.FlatAppearance.CheckedBackColor = Color.Transparent;
                    rb.FlatAppearance.MouseDownBackColor = Color.Transparent;
                    rb.FlatAppearance.MouseOverBackColor = Color.Transparent;
                    rb.FlatStyle = FlatStyle.Flat;
                    rb.BackgroundImage = Properties.Resources.RadioUnChecked;
                    rb.BackgroundImageLayout = ImageLayout.Zoom;
                    rb.CheckedChanged += rb_CheckedChanged;
                    rb_CheckedChanged(rb, null);

                    c.Font = txtFirstName.Font;
                    c.Font = new System.Drawing.Font(c.Font.FontFamily, c.Font.Size - 6, c.Font.Style);
                    c.ForeColor = lblEmail.ForeColor;
                    c.Height = (int)c.CreateGraphics().MeasureString(c.Text, txtFirstName.Font).Height;
                    c.Width = txtFirstName.Width / 2 - c.Margin.Right;
                    flpCustomAttributes.Height += c.Height / 2;
                }
                else if (type.Contains("Check"))// checkbox
                {
                    CheckBox cb = c as CheckBox;
                    cb.AutoSize = false;
                    cb.Width = txtFirstName.Width;
                    cb.Height = (int)c.CreateGraphics().MeasureString(c.Text, txtFirstName.Font).Height;
                    cb.CheckStateChanged += cb_CheckStateChanged;
                    Control lbl = flpCustomAttributes.GetNextControl(cb, false);
                    lbl.Text = "   ";
                    lbl.Width = 80;

                    PictureBox pbCheckBox = new PictureBox();
                    if (cb.Checked)
                        pbCheckBox.Image = global::Parafait_Kiosk.Properties.Resources.tick_box_checked;
                    else
                        pbCheckBox.Image = global::Parafait_Kiosk.Properties.Resources.tick_box_unchecked;
                    pbCheckBox.Size = pbCheckBox.Image.Size;
                    pbCheckBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Normal;

                    pbCheckBox.Tag = cb;
                    pbCheckBox.Click += new System.EventHandler(pbCheckBox_Click);

                    addedControls.Add(pbCheckBox);
                    addAtIndex.Add(flpCustomAttributes.Controls.GetChildIndex(cb));

                    Button lblCheckBox = new Button();
                    lblCheckBox.Text = cb.Text;
                    lblCheckBox.Text = MessageUtils.getMessage(954);//"Opt in to receive special deals and offers";
                    lblCheckBox.Font = lblEmail.Font;//Modification on 17-Dec-2015 for introducing new theme
                    lblCheckBox.AutoSize = false;
                    lblCheckBox.Width = flpCustomerMain.Width - lbl.Width - pbCheckBox.Width - 54;
                    lblCheckBox.Height = lblCardNumber.Height + 40;
                    lblCheckBox.TextAlign = ContentAlignment.MiddleLeft;
                    lblCheckBox.Tag = cb;
                    lblCheckBox.FlatStyle = FlatStyle.Flat;
                    lblCheckBox.FlatAppearance.BorderSize = 0;
                    lblCheckBox.FlatAppearance.CheckedBackColor =
                        lblCheckBox.FlatAppearance.MouseDownBackColor =
                        lblCheckBox.FlatAppearance.MouseOverBackColor = Color.Transparent;
                    lblCheckBox.BackColor = Color.Transparent;
                    lblCheckBox.ForeColor = lblEmail.ForeColor;//Modification on 17-Dec-2015 for introducing new theme
                    lblCheckBox.Click += new System.EventHandler(pbCheckBox_Click);

                    addedControls.Add(lblCheckBox);
                    addAtIndex.Add(flpCustomAttributes.Controls.GetChildIndex(cb));

                    flpCustomAttributes.Height += lblCheckBox.Height * 2 + lblCheckBox.Margin.Top + lblCheckBox.Margin.Bottom;
                }
            }

            int i = 0;
            foreach (Control c in addedControls)
            {
                flpCustomAttributes.Controls.Add(c);
                flpCustomAttributes.Controls.SetChildIndex(c, addAtIndex[i] + i);
                i++;
            }

            bool flpCustomVisible = false;

            foreach (DataRow dr in Utilities.executeDataTable(@"select default_value_name, isnull(pos.optionvalue, pd.default_value) value, cu.Name
                                                                from parafait_defaults pd
                                                                left outer join ParafaitOptionValues pos 
                                                                    on pd.default_value_id = pos.optionId 
                                                                    and POSMachineId = @POSMachineId
                                                                    and pos.activeFlag = 'Y'
                                                                left outer join CustomAttributes cu
                                                                    on cu.Name = pd.default_value_name
                                                                    and cu.Applicability = 'CUSTOMER'
                                                                where pd.active_flag = 'Y'
                                                                and screen_group = 'customer'",
                                                               new SqlParameter("POSMachineId", ParafaitEnv.POSMachineId)).Rows)
            {
                try
                {
                    string val = dr["value"].ToString();
                    bool visible = val == "N" ? false : true;
                    switch (dr["default_value_name"].ToString())
                    {
                        case "LAST_NAME": txtLastName.Parent.Visible = lblLastName.Visible = visible; txtLastName.Tag = val; if (val.Equals("M")) lblLastName.Text += "*"; break;
                        case "ADDRESS1": lblAddres1.Visible = visible; textBoxAddress1.Parent.Visible = visible; textBoxAddress1.Tag = val; if (val.Equals("M")) lblAddres1.Text += "*"; break;
                        case "ADDRESS2": lblAddress2.Visible = textBoxAddress2.Parent.Visible = visible; textBoxAddress2.Tag = val; if (val.Equals("M")) lblAddress2.Text += "*"; break;
                        case "CITY": lblCity.Visible = textBoxCity.Parent.Visible = visible; textBoxCity.Tag = val; if (val.Equals("M")) lblCity.Text += "*"; break;
                        case "STATE":
                            {
                                if (!visible)
                                {
                                    cmbState.Visible = lblStateCombo.Visible = panelState.Visible = visible;
                                }
                                else
                                {
                                    countryId = Utilities.getParafaitDefaults("STATE_LOOKUP_FOR_COUNTRY");
                                    List<StateDTO> stateList = null;
                                    StateDTOList stateDTOList = new StateDTOList(Utilities.ExecutionContext);
                                    List<KeyValuePair<StateDTO.SearchByParameters, string>> searchStateParams = new List<KeyValuePair<StateDTO.SearchByParameters, string>>();
                                    searchStateParams.Add(new KeyValuePair<StateDTO.SearchByParameters, string>(StateDTO.SearchByParameters.SITE_ID, Utilities.ExecutionContext.SiteId.ToString()));
                                    if (!countryId.Equals("-1"))
                                    {
                                        searchStateParams.Add(new KeyValuePair<StateDTO.SearchByParameters, string>(StateDTO.SearchByParameters.COUNTRY_ID, countryId));
                                    }
                                    stateList = stateDTOList.GetStateDTOList(searchStateParams);
                                    if (stateList == null)
                                    {
                                        stateList = new List<StateDTO>();
                                    }
                                    stateList.Insert(0, new StateDTO());
                                    stateList[0].StateId = -1;
                                    stateList[0].State = "SELECT";
                                    cmbState.DataSource = stateList;
                                    cmbState.DisplayMember = "Description";
                                    cmbState.ValueMember = "StateId";
                                    cmbState.Tag = val;

                                    if (val.Equals("M"))
                                    {
                                        lblStateCombo.Text += "*";
                                    }
                                }

                            }
                            break;
                        case "TITLE":
                            {
                                if (!visible)
                                {
                                    lblTitle.Visible = cmbTitle.Parent.Visible = visible;
                                }
                                else
                                {
                                    cmbTitle.DataSource = Utilities.executeDataTable("select LookupValue title from LookupView where LookupName = 'CUSTOMER_TITLES' union all select '' order by 1");
                                    cmbTitle.ValueMember =
                                    cmbTitle.DisplayMember = "title";
                                    cmbTitle.Tag = val;
                                    if (val.Equals("M")) lblTitle.Text += "*";
                                }
                            }
                            break;
                        case "COUNTRY":
                            {
                                if (!visible)
                                {
                                    cmbCountry.Visible = lblCountry.Visible = panelCountry.Visible = visible;
                                }
                                else
                                {
                                    lblCountry.Visible = cmbCountry.Parent.Visible = visible;
                                    List<CountryDTO> countryList = null;
                                    CountryDTOList countryDTOList = new CountryDTOList(Utilities.ExecutionContext);
                                    List<KeyValuePair<CountryDTO.SearchByParameters, string>> searchCountryParams = new List<KeyValuePair<CountryDTO.SearchByParameters, string>>();
                                    searchCountryParams.Add(new KeyValuePair<CountryDTO.SearchByParameters, string>(CountryDTO.SearchByParameters.SITE_ID, Utilities.ExecutionContext.SiteId.ToString()));
                                    countryList = countryDTOList.GetCountryDTOList(searchCountryParams);
                                    if (countryList == null)
                                    {
                                        countryList = new List<CountryDTO>();
                                    }
                                    countryList.Insert(0, new CountryDTO());
                                    countryList[0].CountryId = -1;
                                    countryList[0].CountryName = "SELECT";
                                    cmbCountry.DataSource = countryList;
                                    cmbCountry.DisplayMember = "CountryName";
                                    cmbCountry.ValueMember = "CountryId";
                                    cmbCountry.Tag = val;
                                    if (val.Equals("M")) lblCountry.Text += "*";
                                }
                            }
                            break;
                        case "PIN": lblPostalCode.Visible = textBoxPin.Parent.Visible = visible; textBoxPin.Tag = val; if (val.Equals("M")) lblPostalCode.Text += "*"; break;
                        case "EMAIL": lblEmail.Visible = textBoxEmail.Parent.Visible = visible; textBoxEmail.Tag = val; if (val.Equals("M")) lblEmail.Text += "*"; break;
                        case "BIRTH_DATE": lblBirthDate.Visible = txtBirthDate.Parent.Visible = visible; txtBirthDate.Tag = val; if (val.Equals("M")) lblBirthDate.Text += "*"; break;
                        case "ANNIVERSARY": txtAnniversary.Parent.Visible = lblAnniversary.Visible = visible; txtAnniversary.Tag = val; if (val.Equals("M")) lblAnniversary.Text += "*"; break;
                        case "UNIQUE_ID": txtUniqueId.Parent.Visible = lblUniqueId.Visible = visible; txtUniqueId.Tag = val; if (val.Equals("M")) lblUniqueId.Text += "*"; break;
                        case "GENDER": lblGender.Visible = comboBoxGender.Parent.Visible = visible; comboBoxGender.Tag = val; if (val.Equals("M")) lblGender.Text += "*"; break;
                        case "CONTACT_PHONE": textBoxContactPhone1.Parent.Visible = lblPhone.Visible = visible; textBoxContactPhone1.Tag = val; if (val.Equals("M")) lblPhone.Text += "*"; break;
                        case "CUSTOMER_PHOTO": pbCapture.Visible = lblPhoto.Visible = visible; pbCapture.Name = val; if (val.Equals("M")) lblPhoto.Text += "*"; break;
                        case "OPT_IN_PROMOTIONS": pnlOptinPromotions.Visible = visible; btnOpinPromotions.Tag = val; break;
                        case "OPT_OUT_WHATSAPP_MESSAGE": pnlWhatsAppOptOut.Visible = visible; pnlWhatsAppOptOut.Tag = val; break;
                        case "OPT_IN_PROMOTIONS_MODE": lblPromotionMode.Visible = panelOptInPromotionsMode.Visible = cmbPromotionMode.Visible = visible; cmbPromotionMode.Tag = val; break;
                        default:
                            {
                                if (dr["Name"] != DBNull.Value)
                                {
                                    foreach (Control c in flpCustomAttributes.Controls)
                                    {
                                        if ((c.Tag != null && c.Tag.Equals(dr["Name"]))
                                         || (c.Text.ToLower().Equals(dr["default_value_name"].ToString().ToLower() + ":")))
                                        {
                                            c.Visible = visible;
                                            if (visible)
                                                flpCustomVisible = true;
                                        }
                                    }
                                }
                                break;
                            }
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
            }

            flpCustomAttributes.Visible = flpCustomVisible;
            flpCustomerMain.Visible = true;

            foreach (Control c in flpCustomAttributes.Controls)
            {
                if (c.GetType().ToString().Contains("TextBox"))
                {
                    (c as TextBox).Enter += new EventHandler(textBox_Enter);
                }
                else if (c.GetType().ToString().Contains("Check"))
                    c.Visible = false;
            }


            //Begin Modification 24-Apr-2015 :: Customer Phone width
            int width = 0;
            int.TryParse(Utilities.getParafaitDefaults("CUSTOMER_PHONE_NUMBER_WIDTH"), out width);
            if (width > 0)
                textBoxContactPhone1.MaxLength = width;
            else
                textBoxContactPhone1.MaxLength = 20;
            //End Modification 24-Apr-2015 :: Customer Phone width

            ImageDirectory = Utilities.getParafaitDefaults("IMAGE_DIRECTORY") + "\\";
            Utilities.setLanguage(this);

            log.LogMethodExit();
        }

        void cb_CheckStateChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            foreach (Control c in flpCustomAttributes.Controls)
            {
                if (sender.Equals(c.Tag) && c.GetType().ToString().Contains("Picture"))
                {
                    if ((sender as CheckBox).Checked)
                        (c as PictureBox).Image = Properties.Resources.tick_box_checked;
                    else
                        (c as PictureBox).Image = Properties.Resources.tick_box_unchecked;
                }
            }
            log.LogMethodExit();
        }

        void rb_CheckedChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            RadioButton rb = sender as RadioButton;
            if (rb.Checked)
                rb.BackgroundImage = Properties.Resources.RadioChecked;
            else
                rb.BackgroundImage = Properties.Resources.RadioUnChecked;
            log.LogMethodExit();
        }

        //int timeOutSecs = 20;
        //int secondsRemaining;
        //Timer TimeOutTimer;
        private void Customer_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            arrange();

            if (PanelLinkedRelations.Visible == true)
            {
                dgvLinkedRelations.DataSource = customCustomerDTOBindingSource;  // control's DataSource.
            }
            if (CurrentCard != null && CurrentCard.card_id == -1)
            {
                displayMessageLine(MessageUtils.getMessage(454), ERROR);
                log.LogMethodExit();
                return;
            }
            if (CurrentCard != null && CurrentCard.technician_card.Equals('Y'))
            {
                try
                {
                    string msg = MessageUtils.getMessage(197, CurrentCard.CardNumber);
                    KioskStatic.logToFile(msg);
                    displayMessageLine(msg, ERROR);
                    StopKioskTimer();
                    frmOKMsg frmOK;
                    using (frmOK = new frmOKMsg(msg, true))
                    {
                        frmOK.ShowDialog();
                    }
                    ResetKioskTimer();
                    StartKioskTimer();
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
                finally
                {
                    DialogResult = DialogResult.No;
                    Close();
                }
                log.LogMethodExit();
                return;
            }

            if (!termsAndConditions && Utilities.getParafaitDefaults("TERMS_AND_CONDITIONS").Equals("M"))
            {
                pnlTermsandConditions.Visible = true;
                flpCustomerMain.Controls.Add(pnlTermsandConditions);
            }
            else
            {
                pnlTermsandConditions.Visible = false;
            }
            displayCustomerDetails();
            if (inputCustomerDTO != null)
            {
                maskDisplay(true);
            }
            else
            {
                maskDisplay(false);
            }

            if (KioskStatic.Utilities.ParafaitEnv.MIFARE_CARD && KioskStatic.TopUpReaderDevice != null)
            {
                if (KioskStatic.cardAcceptor != null)
                    KioskStatic.cardAcceptor.AllowAllCards();
            }

            if (KioskStatic.TopUpReaderDevice != null)
            {
                KioskStatic.TopUpReaderDevice.Register(new EventHandler(CardScanCompleteEventHandle));
                List<EventHandler> eventList = (KioskStatic.TopUpReaderDevice != null ? KioskStatic.TopUpReaderDevice.GetCallBackList : new List<EventHandler>());
                int listCount = eventList != null && eventList.Any() ? eventList.Count : 0;
                KioskStatic.logToFile(this.Name + ": Register topup card reader device. Call back list count: " + listCount.ToString());
                log.Info(this.Name + ": Register topup card reader device. Call back list count: " + listCount.ToString());
            }

            Cursor.Show();
            btnAddRelation.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage;
            Audio.PlayAudio(Audio.EnterDetailsAndSave);
            log.LogMethodExit();
        }

        private void CardScanCompleteEventHandle(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (e is DeviceScannedEventArgs)
            {
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
                        displayMessageLine(ex.Message, ERROR);
                        return;
                    }
                    try
                    {
                        scannedTagNumber = tagNumberParser.ValidateDecryptedTag(decryptedTagNumber, Utilities.ParafaitEnv.SiteId);
                    }
                    catch (ValidationException ex)
                    {
                        log.LogVariableState("Decrypted Tag Number validation: ", ex);
                        displayMessageLine(ex.Message, ERROR);
                        return;
                    }
                    catch (Exception ex)
                    {
                        log.LogVariableState("Decrypted Tag Number validation: ", ex);
                        displayMessageLine(ex.Message, ERROR);
                        return;
                    }
                }
                if (tagNumberParser.TryParse(scannedTagNumber, out tagNumber) == false)
                {
                    string message = tagNumberParser.Validate(scannedTagNumber);
                    displayMessageLine(message, ERROR);
                    log.LogMethodExit(null, "Invalid Tag Number. " + message);
                    return;
                }

                string CardNumber = tagNumber.Value;
                CardNumber = KioskStatic.ReverseTopupCardNumber(CardNumber);
                try
                {
                    handleCardRead(CardNumber, sender as DeviceClass);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while executing CardScanCompleteEventHandle()", ex); ;
                    displayMessageLine(ex.Message, ERROR);
                }
            }
            log.LogMethodExit();
        }

        void handleCardRead(string inCardNumber, DeviceClass readerDevice)
        {
            log.LogMethodEntry(inCardNumber, readerDevice);
            if (KioskStatic.Utilities.ParafaitEnv.MIFARE_CARD && KioskStatic.cardAcceptor != null)
            {
                KioskStatic.cardAcceptor.EjectCardFront();
                KioskStatic.cardAcceptor.BlockAllCards();
            }

            if (CurrentCard != null)
            {
                log.LogVariableState("CurrentCard", CurrentCard);
                ResetKioskTimer();
                string cardNumber = inCardNumber;
                log.LogVariableState("cardNumber", cardNumber);
                if (CurrentCard.CardNumber == cardNumber)
                {
                    maskDisplay(true);
                }
                else
                {
                    displayMessageLine(MessageContainerList.GetMessage(Utilities.ExecutionContext, 5199), ERROR); //Sorry, tapped card does not belong the customer
                    if (KioskStatic.Utilities.ParafaitEnv.MIFARE_CARD && KioskStatic.cardAcceptor != null)
                    {
                        KioskStatic.cardAcceptor.AllowAllCards();
                    }
                }
            }
            log.LogMethodExit();
        }

        void arrange()//playpass:Starts
        {
            log.LogMethodEntry();

            for (int i = 0; i < flpCustomerMain.Controls.Count; i++)
            {
                if (flpCustomerMain.Controls[i].Visible == false)
                {
                    flpCustomerMain.Controls.Remove(flpCustomerMain.Controls[i]);
                    i = 0;
                }
            }

            flpCustomAttributes.Visible = flpCustomerMain.Visible = false;
            int fieldCount = 0;
            //int flpCustomHeight = 0;
            foreach (Control c in flpCustomerMain.Controls)
            {
                string type = c.GetType().ToString().ToLower();
                if (c.Equals(flpCustomAttributes))
                    continue;

                //c.Margin = new Padding(c.Margin.Left, c.Margin.Top + 12, c.Margin.Right, c.Margin.Bottom);

                if (!type.Contains("label"))
                {
                    // c.Width += 160;
                    fieldCount++;
                }
                else
                {
                    // c.Width += 80;
                }
            }

            flpCustomAttributes.Visible = flpCustomerMain.Visible = true;
            log.LogMethodExit();
        }//playpass:Ends


        public override void KioskTimer_Tick(object sender, EventArgs e)
        {

            log.LogMethodEntry();
            int tickSecondsRemaining = GetKioskTimerSecondsValue();
            tickSecondsRemaining--;
            setKioskTimerSecondsValue(tickSecondsRemaining);
            if (tickSecondsRemaining <= 60)
            {
                lblTimeRemaining.Font = TimeOutFont;
                lblTimeRemaining.Text = tickSecondsRemaining.ToString("#0");
            }
            else
            {
                lblTimeRemaining.Font = savTimeOutFont;
                lblTimeRemaining.Text = (tickSecondsRemaining / 60).ToString() + ":" + (tickSecondsRemaining % 60).ToString().PadLeft(2, '0');
            }

            if (tickSecondsRemaining == 10)
            {
                if (TimeOut.AbortTimeOut(this))
                {
                    ResetKioskTimer();
                }
                else
                    tickSecondsRemaining = 0;
            }

            if (tickSecondsRemaining <= 0)
            {
                displayMessageLine(MessageUtils.getMessage(457), WARNING);
                Application.DoEvents();
                DialogResult = DialogResult.Cancel;
                this.Close();
                Dispose();
            }
            log.LogMethodExit();
        }

        private void textBoxContactPhone1_KeyPress(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            if (!char.IsNumber(e.KeyChar) && !char.IsControl(e.KeyChar) && !(e.KeyChar == '+'))
                e.Handled = true;
            log.LogMethodExit();
        }


        public override void btnPrev_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            DialogResult = DialogResult.OK;
            Close();
            Dispose();
            log.LogMethodExit();
        }

        private void Customer_FormClosed(object sender, FormClosedEventArgs e)
        {
            log.LogMethodEntry();
            if (KioskStatic.Utilities.ParafaitEnv.MIFARE_CARD && KioskStatic.TopUpReaderDevice != null)
            {
                if (KioskStatic.cardAcceptor != null)
                {
                    KioskStatic.cardAcceptor.EjectCardFront();
                    KioskStatic.cardAcceptor.BlockAllCards();
                }
            }

            if (KioskStatic.TopUpReaderDevice != null)
            {
                KioskStatic.TopUpReaderDevice.UnRegister();
                List<EventHandler> eventList = (KioskStatic.TopUpReaderDevice != null ? KioskStatic.TopUpReaderDevice.GetCallBackList : new List<EventHandler>());
                int listCount = eventList != null && eventList.Any() ? eventList.Count : 0;
                KioskStatic.logToFile(this.Name + ": UnRegister topup card reader device. Call back list count: " + listCount.ToString());
                log.Info(this.Name + ": UnRegister topup card reader device. Call back list count: " + listCount.ToString());
            }

            // TimeOutTimer.Stop();
            try
            {
                StopKioskTimer();
                DisposeKeyboardObject();
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing Customer_FormClosed()", ex);
            }
            //Cursor.Hide();

            Audio.Stop();
            log.Info(this.Name + ": Form closed");
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

        private void btnShowKeyPad_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            log.LogMethodExit();
        }

        private void pbCapture_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            StopKioskTimer();
            try
            {
                using (CustomerPhoto cp = new CustomerPhoto(ImageDirectory, pbCapture.Image, Utilities))
                {
                    cp.StartPosition = FormStartPosition.Manual;
                    cp.Location = new Point(this.Width / 2 - cp.Width / 2, button1.Bottom - 23);
                    if (cp.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        pbCapture.Image = cp.CustomerImage;
                        pbCapture.Tag = cp.PhotoFileName;
                        pbCapture.Height = (int)((decimal)pbCapture.Width / ((decimal)pbCapture.Image.Width / (decimal)pbCapture.Image.Height));
                    }
                }
            }
            catch (Exception ex)
            {
                displayMessageLine(ex.Message, ERROR);
            }
            finally
            {
                //TimeOutTimer.Start();
                StartKioskTimer();
            }
            log.LogMethodExit();
        }

        private void buttonCustomerSave_MouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            // (sender as Button).BackgroundImage = Properties.Resources.save_cancel_btn;//Modification on 17-Dec-2015 for introducing new theme
            log.LogMethodExit();
        }

        private void buttonCustomerSave_MouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            // (sender as Button).BackgroundImage = Properties.Resources.save_cancel_btn_pressed;//Modification on 17-Dec-2015 for introducing new theme
            log.LogMethodExit();
        }

        private void cmbTitle_SelectedIndexChanged(object sender, EventArgs e)
        {
            //secondsRemaining = timeOutSecs;   
            log.LogMethodEntry();
            ResetKioskTimer();
            log.LogMethodExit();
        }//Ends:Modification on 17-Dec-2015 for introducing new theme

        private void pnlOptinPromotions_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                ResetKioskTimer();
                if (pnlOptinPromotions.Tag.Equals(true))
                {
                    pnlOptinPromotions.Tag = false;
                    btnOpinPromotions.Image = Properties.Resources.tick_box_unchecked;
                }
                else
                {
                    pnlOptinPromotions.Tag = true;
                    btnOpinPromotions.Image = Properties.Resources.tick_box_checked;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void pnlWhatsAppOptin_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                ResetKioskTimer();
                if (pnlWhatsAppOptOut.Tag.Equals(true))
                {
                    pnlWhatsAppOptOut.Tag = false;
                    btnWhatsAppOptOut.Image = Properties.Resources.tick_box_unchecked;
                }
                else
                {
                    pnlWhatsAppOptOut.Tag = true;
                    btnWhatsAppOptOut.Image = Properties.Resources.tick_box_checked;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void pnlTermsandConditions_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (pnlTermsandConditions.Tag.Equals("false"))
                {
                    ResetKioskTimer();
                    StopKioskTimer();
                    // keypad.Hide();
                    using (frmTermsAndConditions frmRegisterTnC = new frmTermsAndConditions(KioskStatic.ApplicationContentModule.REGISTRATION))
                    {
                        if (frmRegisterTnC.ShowDialog() == DialogResult.Yes)
                        {
                            pnlTermsandConditions.Tag = true;
                            btnTermsandConditions.Image = Properties.Resources.tick_box_checked;
                        }
                    }
                    //keypad.Show();
                    StartKioskTimer();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        private void lblDateTimeFormat_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        private void txtAnnivarsaryDate_Leave(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                ResetKioskTimer();
                //if (string.IsNullOrEmpty(txtAnniversary.Text))
                //{
                //    lblDateTimeFormat.Visible = true;
                //}
                //else
                //{
                //    lblDateTimeFormat.Visible = false;
                //}
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        //private void verticalScrollBarView1_Click(object sender, EventArgs e)
        //{
        //    log.LogMethodEntry();
        //    ResetKioskTimer();
        //    log.LogMethodExit();
        //}
        //private void verticalScrolldgvLinkedRelations_Click(object sender, EventArgs e)
        //{
        //    log.LogMethodEntry();
        //    ResetKioskTimer();
        //    log.LogMethodExit();
        //}

        private void DownButtonClick(object sender, EventArgs e)
        {
            try
            {
                ResetKioskTimer();
                refreshTimer.Start();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        private void UpButtonClick(object sender, EventArgs e)
        {
            try
            {
                ResetKioskTimer();
                refreshTimer.Start();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }
        private void txtUniqueId_Validating(object sender, CancelEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                ResetKioskTimer();
                frmOKMsg frmOK;
                if (CurrentCard == null && inputCustomerDTO == null)
                {
                    log.LogMethodExit();
                    return;
                }

                if (CurrentCard != null)
                {
                    if ((sender as TextBox).Text.Trim() == "" && ParafaitEnv.UNIQUE_ID_MANDATORY_FOR_VIP == "Y" && CurrentCard.vip_customer == 'Y')
                    {
                        e.Cancel = true;
                        displayMessageLine(MessageUtils.getMessage(289), WARNING);
                        log.LogMethodExit();
                        return;
                    }
                }
                displayMessageLine("", MESSAGE);

                if ((sender as TextBox).Text.Trim() == "")
                {
                    log.LogMethodExit();
                    return;
                }
                ResetKioskTimer();
                List<CustomerDTO> customerDTOList = null;
                CustomerListBL customerListBL = new CustomerListBL(Utilities.ExecutionContext);
                CustomerSearchCriteria customerSearchCriteria = new CustomerSearchCriteria(CustomerSearchByParameters.PROFILE_UNIQUE_IDENTIFIER, Operator.EQUAL_TO, (sender as TextBox).Text.Trim());
                if (CurrentCard != null && CurrentCard.customerDTO != null && CurrentCard.customerDTO.Id >= 0)
                {
                    customerSearchCriteria.And(CustomerSearchByParameters.CUSTOMER_ID, Operator.NOT_EQUAL_TO, CurrentCard.customerDTO.Id);
                }
                else if (inputCustomerDTO != null && inputCustomerDTO.Id > -1)
                {
                    customerSearchCriteria.And(CustomerSearchByParameters.CUSTOMER_ID, Operator.NOT_EQUAL_TO, inputCustomerDTO.Id);
                }
                customerSearchCriteria.OrderBy(CustomerSearchByParameters.CUSTOMER_ID)
                                      .Paginate(0, 20);
                customerDTOList = customerListBL.GetCustomerDTOList(customerSearchCriteria, false, false);
                if (customerDTOList != null && customerDTOList.Count > 0)
                {
                    ResetKioskTimer();
                    using (frmOK = new frmOKMsg(MessageUtils.getMessage(290), true))
                    {
                        frmOK.ShowDialog();
                        if (frmOK.DialogResult == System.Windows.Forms.DialogResult.OK)
                        {
                            if (ParafaitEnv.ALLOW_DUPLICATE_UNIQUE_ID == "N")
                                e.Cancel = true;
                        }
                        else
                        {
                            displayMessageLine(MessageUtils.getMessage(290), WARNING);
                            if (ParafaitEnv.ALLOW_DUPLICATE_UNIQUE_ID == "N")
                            {
                                e.Cancel = true;
                            }
                        }
                        ResetKioskTimer();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            //KioskStatic.logToFile("Setting customized font colors for the UI elements in frmCustomer");
            try
            {
                foreach (Control c in flpCustomerMain.Controls)
                {
                    string type = c.GetType().ToString().ToLower();
                    c.ForeColor = KioskStatic.CurrentTheme.CustomerScreenDetailsHeaderTextForeColor;
                    if (type.Contains("panel"))
                    {
                        foreach (Control lbl in c.Controls)
                        {
                            lbl.ForeColor = KioskStatic.CurrentTheme.CustomerScreenInfoTextForeColor;
                        }
                    }
                }
                if (PanelLinkedRelations.Visible == true)
                {
                    foreach (Control c in dgvLinkedRelations.Controls)
                    {
                        c.ForeColor = KioskStatic.CurrentTheme.CustomerScreenDgvLinkedRelationsTextForeColor;
                    }
                    this.lblLinkedRelations.ForeColor = KioskStatic.CurrentTheme.CustomerScreenLblLinkedRelationsTextForeColor;
                }
                this.lblRegistrationMessage.ForeColor = KioskStatic.CurrentTheme.CustomerScreenHeader1TextForeColor;
                this.button1.ForeColor = KioskStatic.CurrentTheme.CustomerScreenHeader2TextForeColor;
                this.lblOptinPromotions.ForeColor = KioskStatic.CurrentTheme.CustomerScreenOptInTextForeColor; //(Opt in promotion)
                this.lblTermsAndConditions.ForeColor = KioskStatic.CurrentTheme.CustomerScreenTermsAndConditionsTextForeColor;
                this.lblWhatsAppOptOut.ForeColor = KioskStatic.CurrentTheme.CustomerScreenWhatsAppOptOutTextForeColor;
                this.lblPhoto.ForeColor = KioskStatic.CurrentTheme.CustomerScreenPhotoTextForeColor;
                this.buttonCustomerSave.ForeColor = KioskStatic.CurrentTheme.CustomerScreenBtnSaveTextForeColor; //(Save button) 
                this.textBoxMessageLine.ForeColor = KioskStatic.CurrentTheme.CustomerScreenFooterTextForeColor; //(Footer message)
                this.btnPrev.ForeColor = KioskStatic.CurrentTheme.CustomerScreenBtnPrevTextForeColor;
                this.lblTimeRemaining.ForeColor = KioskStatic.CurrentTheme.CustomerScreenLblTimeRemainingTextForeColor;
                this.btnHome.ForeColor = KioskStatic.CurrentTheme.CustomerScreenBtnHomeTextForeColor;
                this.btnAddRelation.ForeColor = KioskStatic.CurrentTheme.CustomerScreenBtnAddRelationTextForeColor;
                lblLinkedRelations.ForeColor = KioskStatic.CurrentTheme.CustomerScreenLblLinkedRelationsTextForeColor;
                this.bigVerticalScrollCustomer.InitializeScrollBar(ThemeManager.CurrentThemeImages.ScrollDownEnabled, ThemeManager.CurrentThemeImages.ScrollDownDisabled, ThemeManager.CurrentThemeImages.ScrollUpEnabled, ThemeManager.CurrentThemeImages.ScrollUpDisabled);
                this.bigVerticalScrollLinkedRelations.InitializeScrollBar(ThemeManager.CurrentThemeImages.ScrollDownEnabled, ThemeManager.CurrentThemeImages.ScrollDownDisabled, ThemeManager.CurrentThemeImages.ScrollUpEnabled, ThemeManager.CurrentThemeImages.ScrollUpDisabled);
                this.dgvLinkedRelations.ColumnHeadersDefaultCellStyle.ForeColor = KioskStatic.CurrentTheme.CustomerScreenDgvLinkedRelationsHeaderTextForeColor;
                this.dgvLinkedRelations.RowTemplate.DefaultCellStyle.ForeColor = KioskStatic.CurrentTheme.CustomerScreenDgvLinkedRelationsInfoTextForeColor;
            }
            catch (Exception ex)
            {
                log.Error("Errow while Setting Customized Fore Colors", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements in frmCustomer: " + ex.Message);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Button Add Relation Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddRelation_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            KioskStatic.logToFile("Add Relation Button Click");

            try
            {
                StopKioskTimer();

                int parentCustomerId;
                CustomerDTO parentCustomerDTO;
                if (CurrentCard != null)
                {
                    parentCustomerId = CurrentCard.customer_id;
                    parentCustomerDTO = CurrentCard.customerDTO;
                }
                else
                {
                    parentCustomerId = customerDTO.Id;
                    parentCustomerDTO = customerDTO;
                }
                using (frmAddCustomerRelation AddRelation = new frmAddCustomerRelation(parentCustomerId
                                                                 , (parentCustomerDTO.FirstName + parentCustomerDTO.LastName)
                                                                 , (CurrentCard == null) ? null : CurrentCard.CardNumber))
                {
                    AddRelation.ShowDialog();
                }
                KioskStatic.logToFile("btnAddRelation_Click() - Exit");
                StartKioskTimer();
                PopulateLinkedCustomerRelations();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                KioskStatic.logToFile("Error while btnAddRelation_Click : " + ex.Message);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Show Customer Linked Relations if Any.
        /// </summary>
        private void PopulateLinkedCustomerRelations()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            KioskStatic.logToFile("PopulateLinkedCustomerRelations(): Getting Customer Linked Relations");

            try
            {
                List<CustomCustomerDTO> customCustomerDTOList = new List<CustomCustomerDTO>();
                int customerId = (CurrentCard != null) ? CurrentCard.customerDTO.Id : customerDTO.Id;
                List<CustomerRelationshipDTO> customerRelationshipDTOList = KioskHelper.GetAllLinkedRelations(customerId, true, true);
                if ((customerRelationshipDTOList != null) && customerRelationshipDTOList.Any())
                {
                    Initialize_dgvLinkedRelations();
                    foreach (CustomerRelationshipDTO customerRelationshipDTO in customerRelationshipDTOList)
                    {
                        try
                        {
                            CustomCustomerDTO customCustomerDTO = new CustomCustomerDTO(customerRelationshipDTO);
                            customCustomerDTOList.Add(customCustomerDTO);
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                            KioskStatic.logToFile("Error Getting Customer Linked Relations:" + customerRelationshipDTO.RelatedCustomerDTO.FirstName + " , " + ex.Message);
                        }
                    }
                }
                else
                {
                    PanelLinkedRelations.Visible = false;
                    bigVerticalScrollLinkedRelations.Visible = false;
                }
                dOBDataGridViewTextBoxColumn.DefaultCellStyle.Format = KioskStatic.DateMonthFormat;
                anniversaryDataGridViewTextBoxColumn.DefaultCellStyle.Format = KioskStatic.DateMonthFormat;
                customCustomerDTOBindingSource.DataSource = customCustomerDTOList;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error Getting Customer Linked Relations");
            }
            KioskStatic.logToFile("PopulateLinkedCustomerRelations() - Exit");
            log.LogMethodExit();
        }
        /// <summary>
        /// Initializes the grid to show Customer Linked Relations
        /// </summary>
        private void Initialize_dgvLinkedRelations()
        {
            log.LogMethodEntry();
            try
            {
                ResetKioskTimer();

                PanelLinkedRelations.Visible = true;

                string fontFamName = (KioskStatic.CurrentTheme.DefaultFont != null ? KioskStatic.CurrentTheme.DefaultFont.FontFamily.Name : "Gotham Rounded Bold");
                this.dgvLinkedRelations.ColumnHeadersDefaultCellStyle.Font = new Font(fontFamName, 21);
                this.dgvLinkedRelations.ForeColor = KioskStatic.CurrentTheme.AddCustomerRelationGridTextForeColor;
                //bigVerticalScrollLinkedRelations.DownButtonBackgroundImage = bigVerticalScrollLinkedRelations.DownButtonDisabledBackgroundImage = ThemeManager.CurrentThemeImages.ScrollDownButton;
                //bigVerticalScrollLinkedRelations.UpButtonBackgroundImage = bigVerticalScrollLinkedRelations.UpButtonDisabledBackgroundImage = ThemeManager.CurrentThemeImages.ScrollUpButtonImage;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                KioskStatic.logToFile("Error while Initialize_dgvLinkedRelations() : " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void dgvLinkedRelations_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (maskLinkedRelations == true)
                {
                    if (dgvLinkedRelations.Columns[e.ColumnIndex].Name == "dOBDataGridViewTextBoxColumn")
                    {
                        if (e.Value != null)
                        {
                            e.Value = new string('X', e.Value.ToString().Length);
                            e.FormattingApplied = true;
                        }
                    }
                }
                else
                {
                    dgvLinkedRelations.Refresh();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                KioskStatic.logToFile("Error while dgvLinkedRelations_CellFormatting : " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void cmbbox_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            log.LogMethodExit();
        }
        private void KeyPressEvent(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                ResetKioskTimer();
                TextBox currentTextxBox = sender as TextBox;
                if (currentTextxBox != null)
                {
                    if (currentTextxBox.Name == "textBoxEmail")
                    {
                        SetTextToLowerCase(currentTextxBox);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        private static void SetTextToLowerCase(TextBox currentTextxBox)
        {
            log.LogMethodEntry();
            if (string.IsNullOrWhiteSpace(currentTextxBox.Text) == false)
            {
                String txtValue = currentTextxBox.Text.ToLower();
                currentTextxBox.Text = txtValue;
                currentTextxBox.Focus();
                currentTextxBox.SelectionStart = txtValue.Length;
                currentTextxBox.SelectionLength = 0;
            }
            log.LogMethodExit();
        }

        private void FormOnKeyDown(object sender, KeyEventArgs e)
        {
            log.LogMethodEntry(e);
            try
            {
                ResetKioskTimer();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            log.LogMethodExit();
        }
        private void FormOnKeyPress(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                ResetKioskTimer();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            log.LogMethodExit();
        }
        private void FormOnKeyUp(object sender, KeyEventArgs e)
        {
            log.LogMethodEntry(e);
            try
            {
                ResetKioskTimer();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            log.LogMethodExit();
        }
        private void FormOnMouseClick(Object sender, MouseEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                ResetKioskTimer();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            log.LogMethodExit();
        }
        private void SetVirtualKeyboard()
        {
            log.LogMethodEntry();
            try
            {
                virtualKeyboardController = new VirtualWindowsKeyboardController(flpCustomerMain.Bottom + 345);
                bool popupOnScreenKeyBoard = true;
                virtualKeyboardController.Initialize(this, new List<Control>() { btnShowKeyPad }, popupOnScreenKeyBoard, new List<Control>() { txtBirthDate, txtAnniversary });
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error Initializing Windows Keyboard in  Add Relation screen: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void SetCustomKeyboard()
        {
            log.LogMethodEntry();
            try
            {
                customKeyboardController = new VirtualKeyboardController(flpCustomerMain.Bottom + 345);
                bool showKeyboardOnTextboxEntry = true;
                customKeyboardController.Initialize(this, new List<Control>() { btnShowKeyPad }, showKeyboardOnTextboxEntry, new List<Control>() { txtBirthDate, txtAnniversary }, lblCardNumber.Font.FontFamily.Name);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error Initializing Custom Keyboard in  Add Relation screen: " + ex.Message);
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
                KioskStatic.logToFile("Error Initializing keyboard in  Add Relation screen: " + ex.Message);
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
                KioskStatic.logToFile("Error Disposing keyboard in  Add Relation screen: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void txtBirthDate_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                DateTime doB = string.IsNullOrEmpty(txtBirthDate.Text.Replace(" ", "").Replace("-", "").Replace("/", "")) ? DateTime.MinValue : customerDOBValue;
                DateTime pickedDate = KioskHelper.LaunchCalendar(defaultDateTimeToShow: doB, enableDaySelection: KioskStatic.enableDaySelection, enableMonthSelection: KioskStatic.enableMonthSelection,
                    enableYearSelection: KioskStatic.enableYearSelection, disableTill: DateTime.MinValue, showTimePicker: false, popupAlerts: frmOKMsg.ShowUserMessage);
                if (pickedDate != DateTime.MinValue)
                {
                    txtBirthDate.Text = pickedDate.ToString(KioskStatic.DateMonthFormat);
                    customerDOBValue = pickedDate;
                }
            }
            catch (Exception ex)
            {
                displayMessageLine(ex.Message, ERROR);
                KioskStatic.logToFile("Error in txtBirthDate_Click of Customer Registration Screen");
                frmOKMsg.ShowUserMessage(ex.Message);
            }
            log.LogMethodExit();
        }

        private void txtAnniversary_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                DateTime anniversary = string.IsNullOrEmpty(txtAnniversary.Text.Replace(" ", "").Replace("-", "").Replace("/", ""))
                                                      ? DateTime.MinValue
                                                      : customerAnniversaryValue;
                DateTime pickedDate = KioskHelper.LaunchCalendar(defaultDateTimeToShow: anniversary, enableDaySelection: KioskStatic.enableDaySelection, enableMonthSelection: KioskStatic.enableMonthSelection,
                    enableYearSelection: KioskStatic.enableYearSelection, disableTill: DateTime.MinValue, showTimePicker: false, popupAlerts: frmOKMsg.ShowUserMessage);
                if (pickedDate != DateTime.MinValue)
                {
                    txtAnniversary.Text = pickedDate.ToString(KioskStatic.DateMonthFormat);
                    customerAnniversaryValue = pickedDate;
                }
            }
            catch (Exception ex)
            {
                displayMessageLine(ex.Message, ERROR);
                KioskStatic.logToFile("Error in txtAnniversary_Click of Customer Registration Screen");
                frmOKMsg.ShowUserMessage(ex.Message);
            }
            log.LogMethodExit();
        }
        private void RefreshTimerTick(System.Object sender, System.EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                refreshTimer.Stop();
                //this.flpCustomerMain.SuspendLayout();
                //this.SuspendLayout();
                //this.flpCustomerMain.ResumeLayout(true);
                //this.ResumeLayout(true);
                this.Refresh();
                //Application.DoEvents();
            }
            catch { }
            log.LogMethodExit();
        }
    }
}
