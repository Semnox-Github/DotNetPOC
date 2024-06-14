/********************************************************************************************
 * Project Name - Customer
 * Description  - user interface
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *1.00                                              Created 
 *2.4.0       25-Nov-2018      Raghuveera           Added new fields for Optin romotions ,Promotion mode and terms and condition
 *2.70        1-Jul-2019       Lakshminarayana      Modified to add support for ULC cards 
 *2.80        4-Sep-2019       Deeksha              Added logger methods.
 *2.80        12-Dec-2019      Girish Kundar        Modified :for Domain name validation changes  
 *2.70.2      18-Feb-2020      Girish Kundar        Modified : Date format issue fix
 *2.90.0      23-Jul-2020      Jinto Thomas         Modified: Verification Code generation by using customerverificationBL
 *                                                   and sending code to user by MessagingRequest  
 *2.150.1     22-Feb-2023      Guru S A             Kiosk Cart Enhancements
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Customer;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.Product;
using Semnox.Parafait.Languages;

namespace Parafait_Kiosk
{
    public partial class Customer : BaseFormKiosk
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Card CurrentCard;
        public CustomerDTO customerDTO;

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

        public Customer(string CardNumber, object BirthDate = null, bool termsAndConditionsAgreed = false)
        {
            log.LogMethodEntry(CardNumber, BirthDate, termsAndConditionsAgreed);
            Utilities.setLanguage();
            InitializeComponent();

            try
            {
                TagNumberLengthList tagNumberLengthList = new TagNumberLengthList(KioskStatic.Utilities.ExecutionContext);
                if (tagNumberLengthList.MaximumValidTagNumberLength > 10)
                {
                    txtCardNumber.Font = new Font(txtCardNumber.Font.FontFamily, txtCardNumber.Font.Size - 9, txtCardNumber.Font.Style);
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occured while changing the font size of tag number", ex);
            }

            lblTimeRemaining.Text = "";
            termsAndConditions = termsAndConditionsAgreed;

            tagNumberParser = new TagNumberParser(Utilities.ExecutionContext);

            //flpCustomerMain.AutoScroll = false;
            KioskStatic.formatMessageLine(textBoxMessageLine);
            KioskStatic.setDefaultFont(this);

            savTimeOutFont = lblTimeRemaining.Font;
            TimeOutFont = lblTimeRemaining.Font = new System.Drawing.Font(lblTimeRemaining.Font.FontFamily, 50, FontStyle.Bold);
            //lblTimeRemaining.Text = timeOutSecs.ToString("#0");
            SetKioskTimerTickValue(20);
            ResetKioskTimer();
            lblTimeRemaining.Text = GetKioskTimerTickValue().ToString("#0");

            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);

            if (string.IsNullOrEmpty(CardNumber) == false)
            {
                CurrentCard = new Card(CardNumber, "Kiosk", Utilities);
                txtCardNumber.Text = CardNumber;
            }
            else
            {
                txtCardNumber.Visible = lblCardNumber.Visible = false;
            }
            
            this.ShowInTaskbar = false;
            
            lblSiteName.Text = KioskStatic.SiteHeading;
            lblMessage1.Text = KioskStatic.CustomerScreenMessage1;
            lblMessage2.Text = KioskStatic.CustomerScreenMessage2;
            //Starts:Modification on 17-Dec-2015 for introducing new theme
            //lblSiteName.ForeColor = KioskStatic.CurrentTheme.SiteHeadingForeColor;
            //textBoxMessageLine.ForeColor = lblMessage1.ForeColor = lblMessage2.ForeColor = lblRegistration.ForeColor = KioskStatic.CurrentTheme.ScreenHeadingForeColor;
            //KioskStatic.setFieldLabelForeColor(flpCustomerMain);
            //KioskStatic.setFieldLabelForeColor(flpCustomAttributes);

            this.BackgroundImage = ThemeManager.CurrentThemeImages.HomeScreenBackgroundImage;//NewTheme102015 //KioskStatic.CurrentTheme.RegistrationBackgroundImage;
            
            btnHome.BackgroundImage = ThemeManager.CurrentThemeImages.HomeButton;
            btnHome.Size = btnHome.BackgroundImage.Size;
            //panel1.BackgroundImage = panel10.BackgroundImage = panel11.BackgroundImage =
            //    panel12.BackgroundImage = panel13.BackgroundImage = panel14.BackgroundImage =
            //    panel15.BackgroundImage = panel2.BackgroundImage = panel3.BackgroundImage =
            //    panel4.BackgroundImage = panel5.BackgroundImage = panel6.BackgroundImage =
            //    panel7.BackgroundImage = panel8.BackgroundImage = panel9.BackgroundImage = ThemeManager.CurrentThemeImages.TextEntryBox;
            btnClose.BackgroundImage = buttonCustomerSave.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage;
            lblTimeRemaining.BackgroundImage = ThemeManager.CurrentThemeImages.TimerBoxSmall;
            textBoxMessageLine.BackgroundImage = ThemeManager.CurrentThemeImages.BottomMessageLineImage;
            //lblRegistrationMessage.Visible = false;//Ends:Modification on 17-Dec-2015 for introducing new theme
            KioskStatic.LoadPromotionModeDropDown(cmbPromotionMode);
            initializeCustomerInfo();
            lblOptinPromotions.Text = Utilities.MessageUtils.getMessage(1739);
            lblPromotionMode.Text = Utilities.MessageUtils.getMessage(1740);
            lblTermsAndConditions.Text = Utilities.MessageUtils.getMessage(1741);
            pnlOptinPromotions.Tag = false;
            //Starts:Modification on 17-Dec-2015 for introducing new theme
            if (KioskStatic.CurrentTheme.TextForeColor != Color.White)
            {
                txtBirthDate.ForeColor = txtCardNumber.ForeColor = txtFirstName.ForeColor = txtLastName.ForeColor =
                    textBoxAddress1.ForeColor = textBoxAddress2.ForeColor = textBoxCity.ForeColor = textBoxContactPhone1.ForeColor =
                    cmbCountry.ForeColor = textBoxEmail.ForeColor = textBoxPin.ForeColor = comboBoxGender.ForeColor =
                    cmbState.ForeColor = cmbTitle.ForeColor = textBoxAnniversaryDate.ForeColor = txtFBAccessToken.ForeColor = cmbPromotionMode.ForeColor=
                    txtFBUserId.ForeColor = txtUniqueId.ForeColor = KioskStatic.CurrentTheme.TextForeColor;
            }
            else
            {
                txtBirthDate.ForeColor = txtCardNumber.ForeColor = txtFirstName.ForeColor = txtLastName.ForeColor =
                    textBoxAddress1.ForeColor = textBoxAddress2.ForeColor = textBoxCity.ForeColor = textBoxContactPhone1.ForeColor =
                    cmbCountry.ForeColor = textBoxEmail.ForeColor = textBoxPin.ForeColor = comboBoxGender.ForeColor =
                    cmbState.ForeColor = cmbTitle.ForeColor = textBoxAnniversaryDate.ForeColor = txtFBAccessToken.ForeColor = cmbPromotionMode.ForeColor =
                    txtFBUserId.ForeColor = txtUniqueId.ForeColor = Color.DarkOrchid;
            }
            //Ends:Modification on 17-Dec-2015 for introducing new theme
            

            if (BirthDate != null)
                _birthDate = Convert.ToDateTime(BirthDate);
            log.LogMethodExit();
            // lblTimeRemaining.Top = lblSiteName.Bottom + 20;//playpass:Starts//Modification on 17-Dec-2015 for introducing new theme
        }

        private void displayCustomerDetails()
        {
            log.LogMethodEntry();
            if (CurrentCard == null || CurrentCard.customerDTO == null)
            {
                txtFirstName.Clear();
                txtLastName.Clear();
                textBoxAddress1.Clear();
                textBoxAddress2.Clear();
                textBoxCity.Clear();
                cmbState.Text = "" ;
                textBoxPin.Clear();
                if (countryId != null && countryId != "-1")
                {
                    cmbCountry.SelectedValue = Convert.ToInt32(countryId);
                    cmbCountry.Enabled = false;
                }
                
                textBoxContactPhone1.Clear();
                textBoxEmail.Clear(); ;
                if (_birthDate != DateTime.MinValue)
                    txtBirthDate.Text = _birthDate.ToString(Utilities.getDateFormat());
                else
                    txtBirthDate.Clear();
                textBoxAnniversaryDate.Clear();
                comboBoxGender.Text = "";
                textBoxNotes.Clear();
                if (cmbTitle.Visible)
                    cmbTitle.SelectedIndex = 0;
                pbCapture.Tag = "";
                pbCapture.Image = Properties.Resources.profile_picture_placeholder;

                displayMessageLine(MessageUtils.getMessage(446), MESSAGE);
            }
            else
            {
                txtFirstName.Text = CurrentCard.customerDTO.FirstName;
                if (cmbTitle.Visible)
                {
                    try
                    {
                        if (string.IsNullOrEmpty(CurrentCard.customerDTO.Title))
                            cmbTitle.SelectedIndex = 0;
                        else
                            cmbTitle.SelectedValue = CurrentCard.customerDTO.Title;
                    }
                    catch(Exception ex)
                    {
                        log.Error(ex.Message);
                        cmbTitle.SelectedIndex = 0;
                    }
                }
                txtLastName.Text = CurrentCard.customerDTO.LastName;
                if(CurrentCard.customerDTO.AddressDTOList != null)
                {
                    textBoxAddress1.Text = CurrentCard.customerDTO.AddressDTOList[0].Line1;
                    textBoxAddress2.Text = CurrentCard.customerDTO.AddressDTOList[0].Line2;
                    textBoxCity.Text = CurrentCard.customerDTO.AddressDTOList[0].City;
                    cmbState.SelectedValue = CurrentCard.customerDTO.AddressDTOList[0].StateId;
                    textBoxPin.Text = CurrentCard.customerDTO.AddressDTOList[0].PostalCode;
                }
                
                try
                {
                    if (countryId != null && countryId != "-1")
                    {
                        cmbCountry.SelectedValue = Convert.ToInt32(countryId);
                    }
                    else
                    {
                        cmbCountry.SelectedValue = CurrentCard.customerDTO.AddressDTOList[0].CountryId;
                    }
                    
                }
                catch(Exception ex)
                {
                    log.Error(ex.Message);
                }
                if (CurrentCard.customerDTO.AddressDTOList != null)
                {
                    try
                    {
                        cmbState.SelectedValue = CurrentCard.customerDTO.AddressDTOList[0].StateId;
                    }
                    catch(Exception ex)
                    {
                        log.Error(ex.Message);
                        cmbState.SelectedIndex = 0;
                    }
                }
                textBoxContactPhone1.Text = CurrentCard.customerDTO.PhoneNumber;
                textBoxEmail.Text = CurrentCard.customerDTO.Email;
                textBoxNotes.Text = CurrentCard.customerDTO.Notes;
                txtUniqueId.Text = CurrentCard.customerDTO.UniqueIdentifier;
                pnlOptinPromotions.Tag = CurrentCard.customerDTO.OptInPromotions;
                if (CurrentCard.customerDTO.OptInPromotions)
                {
                    btnOptinPromotions.Image = Properties.Resources.checkbox_checked;
                }
                else
                {
                    btnOptinPromotions.Image = Properties.Resources.checkbox_unchecked;
                }
                cmbPromotionMode.SelectedValue = CurrentCard.customerDTO.OptInPromotionsMode;
                termsAndConditions = CurrentCard.customerDTO.PolicyTermsAccepted;
                if(!termsAndConditions && Utilities.getParafaitDefaults("TERMS_AND_CONDITIONS").Equals("M"))
                {
                    pnlTermsAndConditions.Visible = true;
                }
                else
                {
                    pnlTermsAndConditions.Visible = false;
                }
                if (CurrentCard.customerDTO.ProfileDTO.ContactDTOList != null && CurrentCard.customerDTO.ProfileDTO.ContactDTOList.Count > 0)
                {
                    foreach (var contactDTO in CurrentCard.customerDTO.ProfileDTO.ContactDTOList)
                    {
                        if (contactDTO.ContactType == ContactType.FACEBOOK)
                        {
                            txtFBUserId.Text = contactDTO.Attribute1;
                            txtFBAccessToken.Text = contactDTO.Attribute2;
                            break;
                        }
                    }
                }

                if (CurrentCard.customerDTO.DateOfBirth != DateTime.MinValue && CurrentCard.customerDTO.DateOfBirth != null )
                    txtBirthDate.Text = CurrentCard.customerDTO.DateOfBirth.Value.ToString(Utilities.getDateFormat());
                else
                    txtBirthDate.Text = "";

                if (CurrentCard.customerDTO.Anniversary != DateTime.MinValue && CurrentCard.customerDTO.Anniversary != null)
                    textBoxAnniversaryDate.Text = CurrentCard.customerDTO.Anniversary.Value.ToString(Utilities.getDateFormat());
                else
                    textBoxAnniversaryDate.Text = "";

                switch (CurrentCard.customerDTO.Gender)
                {
                    case "M": comboBoxGender.Text = "Male"; break;
                    case "F": comboBoxGender.Text = "Female"; break;
                    case "N": comboBoxGender.Text = "Not Set"; break;
                    default: comboBoxGender.Text = "Not Set"; break;
                }

                pbCapture.Tag = CurrentCard.customerDTO.PhotoURL;

                if (CurrentCard.customerDTO.PhotoURL.Trim() != "")
                {
                    SqlCommand cmdImage = Utilities.getCommand();
                    cmdImage.CommandText = "exec ReadBinaryDataFromFile @FileName";

                    cmdImage.Parameters.AddWithValue("@FileName", ImageDirectory + CurrentCard.customerDTO.PhotoURL);
                    try
                    {
                        object o = cmdImage.ExecuteScalar();
                        pbCapture.Image = Utilities.ConvertToImage(o);
                    }
                    catch
                    {
                        pbCapture.Image = null;
                    }
                }
                else
                {
                    pbCapture.Image = Properties.Resources.profile_picture_placeholder;
                }

                CustomAttributes.PopulateData(CurrentCard.customerDTO.Id, CurrentCard.customerDTO.CustomDataSetId);                
            }

            this.ActiveControl = txtFirstName;
            log.LogMethodExit();
        }

        void maskDisplay(bool inAllowEdit)
        {
            log.LogMethodEntry(inAllowEdit);
            if (CurrentCard != null && CurrentCard.customerDTO != null)// already registered customer
            {
                bool allowedit = Utilities.getParafaitDefaults("ALLOW_CUSTOMER_INFO_EDIT").Equals("Y");

                inAllowEdit &= allowedit;
                if (inAllowEdit)
                {
                    txtFirstName.ReadOnly = false;

                    foreach (Control c in flpCustomerMain.Controls)
                    {
                        string type = c.GetType().ToString().ToLower();
                        if (c.Equals(txtFirstName)
                            || type.Contains("label")
                            || type.Contains("panel"))
                            continue;

                        c.Enabled = true;
                    }

                    foreach (Control c in flpCustomAttributes.Controls)
                    {
                        string type = c.GetType().ToString().ToLower();
                        //if (type.Contains("label"))
                        //    continue;

                        c.Enabled = true;
                    }

                    txtBirthDate.PasswordChar =
                    textBoxEmail.PasswordChar =
                    textBoxContactPhone1.PasswordChar = '\0';

                    if (countryId != null && countryId != "-1")
                        cmbCountry.Enabled = false;
                    buttonCustomerSave.Enabled = true;
                    //secondsRemaining = timeOutSecs;
                    ResetKioskTimer();
                    showhideKeypad('S');
                }
                else
                {
                    txtFirstName.ReadOnly = true;

                    foreach (Control c in flpCustomerMain.Controls)
                    {
                        string type = c.GetType().ToString().ToLower();
                        if (c.Equals(txtFirstName)
                            || type.Contains("label")
                            || type.Contains("panel"))
                            continue;

                        c.Enabled = false;
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
                    textBoxContactPhone1.PasswordChar = 'X';

                    buttonCustomerSave.Enabled = false;
                    //secondsRemaining = timeOutSecs;
                    ResetKioskTimer();
                    showhideKeypad('H');
                }

                if (allowedit == false)
                {
                    displayMessageLine(MessageUtils.getMessage(447), WARNING);
                }
                else
                {
                    if (inAllowEdit)
                        displayMessageLine(MessageUtils.getMessage(501), MESSAGE);
                    else
                    {
                        if (KioskStatic.Utilities.ParafaitEnv.MIFARE_CARD)
                            displayMessageLine(MessageUtils.getMessage(929), WARNING);
                        else
                            displayMessageLine(MessageUtils.getMessage(928), WARNING);
                    }
                }
            }
            else
            {
                showhideKeypad('S');
            }

            txtFirstName.Focus();
            log.LogMethodExit();
        }

        void displayMessageLine(string message, string msgType)
        {
            log.LogMethodEntry(message, msgType);
            
            textBoxMessageLine.Text = message;
            log.LogMethodExit();
        }

        private string GetDateMonthFormat()
        {
            log.LogMethodEntry();
            string dateFormat = ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "DATE_FORMAT");
            if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(Utilities.ExecutionContext, "IGNORE_CUSTOMER_BIRTH_YEAR"))
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

        private void buttonCustomerSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            //secondsRemaining = timeOutSecs;
            bool displayErrorInPopup = true;
            frmOKMsg frmOK;            
            ResetKioskTimer();
            txtFirstName.BackColor = txtCardNumber.BackColor;
            txtLastName.BackColor =  txtCardNumber.BackColor; //11-Jun-2015:: Make color as default background for Last name
            pnlPromotionMode.BackColor = Color.Transparent;
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
                    if(displayErrorInPopup)
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
                if (customerDTO == null) //editing once saved
                    customerDTO = new CustomerDTO();
            }
            else if (CurrentCard.customerDTO == null)
                CurrentCard.customerDTO = customerDTO = new CustomerDTO();
            else
                customerDTO = CurrentCard.customerDTO;

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
                    textBoxEmail.BackColor = Color.Goldenrod;
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
                        textBoxContactPhone1.BackColor = Color.Goldenrod;
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
                                using (frmOK = new frmOKMsg(MessageUtils.getMessage(785), true))
                                {
                                    frmOK.ShowDialog();
                                }
                                ResetKioskTimer();
                                StartKioskTimer();
                            }
                            this.ActiveControl = textBoxContactPhone1;
                            textBoxContactPhone1.BackColor = Color.Goldenrod;
                            log.LogMethodExit();
                            return;
                        }
                    }
                }
            }

            //End Modification 24-Apr-2015. Phone and Email validations

            if (!string.IsNullOrEmpty(txtBirthDate.Text.Trim()))
            {
                //string dateformat = Utilities.getDateFormat(); //Added on 24-Apr-2015 for Birth Date validation
                string dateformat = GetDateMonthFormat();
                System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;
                try
                {
                    if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(Utilities.ExecutionContext, "IGNORE_CUSTOMER_BIRTH_YEAR"))
                    {
                        try
                        {
                            customerDTO.DateOfBirth = DateTime.ParseExact(txtBirthDate.Text + "/1900", dateformat + "/yyyy", provider);
                        }
                        catch (Exception ex)
                        {
                            log.Debug("txtBirthDate : " + txtBirthDate.Text);
                            log.Debug(" Birth year is ignored is enabled but Year entered ");
                            dateformat = ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "DATE_FORMAT");
                            customerDTO.DateOfBirth = DateTime.ParseExact(txtBirthDate.Text, dateformat, provider);
                        }
                    }
                    else
                    {
                        customerDTO.DateOfBirth = DateTime.ParseExact(txtBirthDate.Text, dateformat, provider);
                    }

                    if (DateTime.Compare(customerDTO.DateOfBirth.Value, Utilities.getServerTime()) > 0)
                    {
                        displayMessageLine(MessageUtils.getMessage(449, Utilities.getDateFormat(), Convert.ToDateTime("23-Feb-1982").ToString(Utilities.getDateFormat())), ERROR);
                        if (displayErrorInPopup)
                        {
                            StopKioskTimer();
                            using (frmOK = new frmOKMsg(MessageUtils.getMessage(449, Utilities.getDateFormat(), Convert.ToDateTime("23-Feb-1982").ToString(Utilities.getDateFormat())), true))
                            {
                                frmOK.ShowDialog();
                            }
                            ResetKioskTimer();
                            StartKioskTimer();
                        }
                        txtBirthDate.Focus();
                        log.LogMethodExit();
                        return;
                    }
                }
                catch(Exception ex)
                {
                    try
                    {
                        //Begin Modification - 24 Apr 2015 - Birth Date validation
                        //customer.birth_date = Convert.ToDateTime(txtBirthDate.Text);
                        System.Globalization.CultureInfo currentcultureprovider = System.Globalization.CultureInfo.CurrentCulture;
                        customerDTO.DateOfBirth = Convert.ToDateTime(txtBirthDate.Text, currentcultureprovider);
                        if (DateTime.Compare(customerDTO.DateOfBirth.Value, Utilities.getServerTime()) > 0)
                        {
                            displayMessageLine(MessageUtils.getMessage(449, Utilities.getDateFormat(), Convert.ToDateTime("23-Feb-1982").ToString(Utilities.getDateFormat())), ERROR);
                            if (displayErrorInPopup)
                            {
                                StopKioskTimer();
                                using (frmOK = new frmOKMsg(MessageUtils.getMessage(449, Utilities.getDateFormat(), Convert.ToDateTime("23-Feb-1982").ToString(Utilities.getDateFormat())), true))
                                {
                                    frmOK.ShowDialog();
                                }
                                ResetKioskTimer();
                                StartKioskTimer();
                            }
                            txtBirthDate.Focus();
                            log.LogMethodExit();
                            return;
                        }
                        log.Error(ex.Message);
                        //End Modification - 24 Apr 2015 - Birth Date validation
                    }
                    catch(Exception exp)
                    {
                        displayMessageLine(MessageUtils.getMessage(449, Utilities.getDateFormat(), Convert.ToDateTime("23-Feb-1982").ToString(Utilities.getDateFormat())), ERROR);
                        if (displayErrorInPopup)
                        {
                            StopKioskTimer();
                            using (frmOK = new frmOKMsg(MessageUtils.getMessage(449, Utilities.getDateFormat(), Convert.ToDateTime("23-Feb-1982").ToString(Utilities.getDateFormat())), true))
                            {
                                frmOK.ShowDialog();
                            }
                            ResetKioskTimer();
                            StartKioskTimer();
                        }
                        txtBirthDate.Focus();
                        log.Error(exp.Message);
                        log.LogMethodExit();
                        return;
                    }
                }
                txtBirthDate.Text = customerDTO.DateOfBirth.Value.ToString(Utilities.getDateFormat());
            }
            else
            {
                customerDTO.DateOfBirth = null;
            }
            customerDTO.PolicyTermsAccepted = termsAndConditions;
            customerDTO.OptInPromotions = Convert.ToBoolean(pnlOptinPromotions.Tag);
            customerDTO.OptInPromotionsMode = cmbPromotionMode.SelectedValue.ToString();
           
            if (!string.IsNullOrEmpty(textBoxAnniversaryDate.Text.Trim()))
            {
                try
                {
                    customerDTO.Anniversary = Convert.ToDateTime(textBoxAnniversaryDate.Text);
                }
                catch(Exception expn)
                {
                    displayMessageLine(MessageUtils.getMessage(449, Utilities.getDateFormat(), Convert.ToDateTime("10-Jun-2003").ToString(Utilities.getDateFormat())), ERROR);
                    textBoxAnniversaryDate.Focus();
                    log.Error(expn.Message);
                    log.LogMethodExit();
                    return;
                }
            }
            else
            {
                customerDTO.Anniversary = null;
            }

            textBoxEmail.BackColor = txtCardNumber.BackColor;
            if (!string.IsNullOrEmpty(textBoxEmail.Text))
            {
                if (!KioskStatic.check_mail(textBoxEmail.Text.Trim()))
                {
                    displayMessageLine(MessageUtils.getMessage(450), ERROR);
                    if (displayErrorInPopup)
                    {
                        StopKioskTimer();
                        using (frmOK = new frmOKMsg(MessageUtils.getMessage(450), true))
                        {
                            frmOK.ShowDialog();
                        }
                        ResetKioskTimer();
                        StartKioskTimer();
                    }
                    textBoxEmail.Focus();
                    textBoxEmail.BackColor = Color.OrangeRed;
                    log.LogMethodExit();
                    return;
                }
            }

            foreach (Control c in flpCustomerMain.Controls)
            {
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
                            c.BackColor = Color.OrangeRed;
                            log.LogMethodExit();
                            return;
                        }
                    }
                    else if (c.Name == "cmbState")
                    {
                        if ((c as ComboBox).SelectedIndex <= 0)
                        {
                            displayMessageLine(MessageUtils.getMessage(249, lblState.Text.TrimEnd(':', '*')), ERROR);
                            if (displayErrorInPopup)
                            {
                                StopKioskTimer();
                                using (frmOK = new frmOKMsg(MessageUtils.getMessage(249, lblState.Text.TrimEnd(':', '*')), true))
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
                    else if (string.IsNullOrEmpty(c.Text.Trim()))
                    {
                        displayMessageLine(MessageUtils.getMessage(249, flpCustomerMain.GetNextControl(c, false).Text.TrimEnd(':', '*')), ERROR);
                        if (displayErrorInPopup)
                        {
                            StopKioskTimer();
                            using (frmOK = new frmOKMsg(MessageUtils.getMessage(249, flpCustomerMain.GetNextControl(c, false).Text.TrimEnd(':', '*')), true))
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
            if (KioskStatic.Utilities.getParafaitDefaults("TERMS_AND_CONDITIONS").Equals("M") && !termsAndConditions && pnlTermsAndConditions.Tag.Equals("false"))
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
                }
                log.LogMethodExit();
                return;
            }
            else if (KioskStatic.Utilities.getParafaitDefaults("TERMS_AND_CONDITIONS").Equals("M") && !termsAndConditions && pnlTermsAndConditions.Tag.Equals(true))
            {
                customerDTO.PolicyTermsAccepted = true;
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
                    var orderedContactEmail = orderedContactList.Where(s => s.ContactType == ContactType.PHONE);
                    if (orderedContactEmail.Count() > 0)
                    {
                        contactPhone1DTO = orderedContactEmail.First();
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

            ContactDTO contactFacebookDTO = null;
            if (customerDTO.ContactDTOList != null && customerDTO.ContactDTOList.Count > 0 && (!string.IsNullOrEmpty(txtFBUserId.Text) || !string.IsNullOrEmpty(txtFBAccessToken.Text)))
            {
                var orderedContactList = customerDTO.ContactDTOList.OrderByDescending((x) => x.LastUpdateDate);
                var orderedContactFacebook = orderedContactList.Where(s => s.ContactType == ContactType.FACEBOOK);
                if (orderedContactFacebook.Count() > 0)
                {
                    contactFacebookDTO = orderedContactFacebook.First();
                }
            }
            if (contactFacebookDTO == null && (!string.IsNullOrEmpty(txtFBUserId.Text) || !string.IsNullOrEmpty(txtFBAccessToken.Text)))
            {
                contactFacebookDTO = new ContactDTO();
                if (customerDTO.ContactDTOList == null)
                {
                    customerDTO.ContactDTOList = new List<ContactDTO>();
                }
                contactFacebookDTO.ContactType = ContactType.FACEBOOK;
                customerDTO.ContactDTOList.Add(contactFacebookDTO);
            }

            customerDTO.FirstName = txtFirstName.Text;
            customerDTO.Title = (cmbTitle.SelectedIndex > -1 ? cmbTitle.SelectedValue.ToString() : "");
            customerDTO.LastName = txtLastName.Text;
            if (!string.IsNullOrEmpty(textBoxAddress1.Text))
                addressDTO.Line1 = textBoxAddress1.Text;
            if (!string.IsNullOrEmpty(textBoxAddress2.Text))
                addressDTO.Line2 = textBoxAddress2.Text;
            if (!string.IsNullOrEmpty(textBoxCity.Text))
                addressDTO.City = textBoxCity.Text;
            if ((cmbState.SelectedIndex) > -1&&(cmbState.SelectedValue!=null && Convert.ToInt32(cmbState.SelectedValue)>-1))
                addressDTO.StateId =Convert.ToInt32(cmbState.SelectedValue);
            if ((cmbCountry.SelectedIndex) > -1)
                addressDTO.CountryId = Convert.ToInt32(cmbCountry.SelectedValue);
            if (!string.IsNullOrEmpty(textBoxPin.Text))
                addressDTO.PostalCode = textBoxPin.Text;
            if (!string.IsNullOrEmpty(textBoxContactPhone1.Text))
                contactPhone1DTO.Attribute1 = textBoxContactPhone1.Text;
            if (!string.IsNullOrEmpty(textBoxEmail.Text))
                contactEmailDTO.Attribute1 = textBoxEmail.Text.Trim();
            customerDTO.Notes = textBoxNotes.Text;
            customerDTO.UniqueIdentifier = txtUniqueId.Text;
            if(!string.IsNullOrEmpty(txtFBUserId.Text) || !string.IsNullOrEmpty(txtFBAccessToken.Text))
            {
                contactFacebookDTO.Attribute1 = txtFBUserId.Text;
                contactFacebookDTO.Attribute2 = txtFBAccessToken.Text;
            }
            customerDTO.PhotoURL = pbCapture.Tag.ToString();

            switch (comboBoxGender.Text)
            {
                case "Male": customerDTO.Gender = "M"; break;
                case "Female": customerDTO.Gender = "F"; break;
                case "Not Set": customerDTO.Gender = "N"; break;
                default: customerDTO.Gender = "N"; break;
            }

            try
            {
                showhideKeypad('H');
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
                        throw ex;
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
                    if (Utilities.getParafaitDefaults("ENABLE_KIOSK_CUSTOMER_VERIFICATION") == "Y")//Changes to add the customerDTO verification
                    {
                        SendVerificationCode();
                    } //11-06-2015:starts
                }
                else // customerDTO already exists. no need to update card
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
                        throw ex;
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
                        frmOKMsg fok = new frmOKMsg(MessageUtils.getMessage(451, KioskStatic.VIPTerm));
                        fok.ShowDialog();
                    }
                }

                displayMessageLine(MessageUtils.getMessage(452), MESSAGE);

                //12-Jun-2015 - Custom message for first time registration
                frmYesNo frm;
                if (firstTimeRegistration)
                {
                    if (KioskStatic.RegistrationBonusAmount > 0 && CurrentCard != null &&
                        ((ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "REGISTRATION_BONUS_ON_VERIFICATION").Equals("N"))))
                    {
                        frm = new frmYesNo(MessageContainerList.GetMessage(Utilities.ExecutionContext, 927, KioskStatic.RegistrationBonusAmount) + ". " + MessageUtils.getMessage(452));
                    }
                    else if (KioskStatic.RegistrationBonusAmount > 0 && CurrentCard != null &&
                        ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "REGISTRATION_BONUS_ON_VERIFICATION").Equals("Y"))
                    {
                        frm = new frmYesNo(MessageContainerList.GetMessage(Utilities.ExecutionContext, 1746, KioskStatic.RegistrationBonusAmount) + ". " + MessageUtils.getMessage(452));
                    }
                    else
                    {
                        frm = new frmYesNo(MessageContainerList.GetMessage(Utilities.ExecutionContext, MessageUtils.getMessage(926) + ". " + MessageUtils.getMessage(452)));
                    }
                    firstTimeRegistration = false;
                }
                else
                {
                    frm = new frmYesNo(MessageUtils.getMessage(453));
                }

                if (frm.ShowDialog() == System.Windows.Forms.DialogResult.No)
                {
                    Close();
                }
                else
                {
                    maskDisplay(false);
                    if (KioskStatic.Utilities.ParafaitEnv.MIFARE_CARD && KioskStatic.cardAcceptor != null)
                    {
                        KioskStatic.cardAcceptor.AllowAllCards();
                    }
                }
            }
            catch (ValidationException vx)
            {
                if (vx.ValidationErrorList != null && vx.ValidationErrorList.Count > 0)
                {
                    displayMessageLine(MessageUtils.getMessage(255, vx.ValidationErrorList[0].Message), ERROR);
                    if (displayErrorInPopup)
                    {
                        StopKioskTimer();
                        using (frmOK = new frmOKMsg(MessageUtils.getMessage(255, vx.ValidationErrorList[0].Message), true))
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
                                c.BackColor = Color.OrangeRed;
                                continue;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                displayMessageLine(MessageUtils.getMessage(255, ex.Message), ERROR);
                if (displayErrorInPopup)
                {
                    StopKioskTimer();
                    using (frmOK = new frmOKMsg(MessageUtils.getMessage(255, ex.Message), true))
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
            log.LogMethodEntry();
            //sends mail and/or SMS to the customer
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

                //if (!string.IsNullOrEmpty(customerDTO.Email))
                //{
                //    Messaging msg = new Messaging(Utilities);
                //    string body = "Dear " + customerDTO.FirstName + ",";
                //    body += Environment.NewLine + Environment.NewLine;
                //    body += "Your registration verification code is " + Code + ".";
                //    body += Environment.NewLine + Environment.NewLine;
                //    body += "Thank you";
                //    body += Environment.NewLine;
                //    body += ParafaitEnv.SiteName;

                //    msg.SendEMailSynchronous(customerDTO.Email, "", ParafaitEnv.SiteName + " - customer registration verification", body);
                //}

                //if (!string.IsNullOrEmpty(customerDTO.PhoneNumber))
                //{
                //    Messaging msg = new Messaging(Utilities);
                //    string body = "Dear " + customerDTO.FirstName + ", ";
                //    body += "Your registration verification code is " + Code + ". ";
                //    body += "Thank you. ";
                //    body += ParafaitEnv.SiteName;

                //    msg.sendSMSSynchronous(customerDTO.PhoneNumber, body);
                //}
            }
            catch(Exception ex)
            {
                log.Error("Error occurred while executing SendVerificationCode()" + ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
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
            List<Control> addedControls = new List<Control>();
            List<int> addAtIndex = new List<int>();
            foreach (Control c in flpCustomAttributes.Controls)
            {
                string type = c.GetType().ToString();
                if (type.Contains("Label"))
                {
                    (c as Label).AutoSize = false;
                    c.Size = lblFBToken.Size;
                    (c as Label).TextAlign = ContentAlignment.MiddleRight;
                    c.Margin = lblFBToken.Margin;
                    c.Font = lblFBToken.Font;
                }
                else if (type.Contains("TextBox") || type.Contains("Combo"))
                {
                    c.Font = txtFBUserId.Font;
                    if (type.Contains("Combo"))
                        c.Font = new System.Drawing.Font(c.Font.FontFamily, c.Font.Size - 3, c.Font.Style);
                    c.Width = txtFBUserId.Width;
                    c.Margin = txtFBUserId.Margin;
                    c.BackColor = txtFBUserId.BackColor;
                    c.ForeColor = System.Drawing.Color.DarkOrchid;
                    if (type.Contains("TextBox"))
                        (c as TextBox).BorderStyle = txtFBUserId.BorderStyle;
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

                    c.Font = txtFBAccessToken.Font;
                    c.Font = new System.Drawing.Font(c.Font.FontFamily, c.Font.Size - 6, c.Font.Style);
                    c.ForeColor = txtFBAccessToken.ForeColor;
                    c.Height = (int)c.CreateGraphics().MeasureString(c.Text, txtFBAccessToken.Font).Height;
                    c.Width = txtFBAccessToken.Width / 2 - c.Margin.Right;
                }
                else if (type.Contains("Check"))// checkbox
                {
                    CheckBox cb = c as CheckBox;
                    cb.AutoSize = false;
                    cb.Width = txtFirstName.Width;
                    cb.Height = (int)c.CreateGraphics().MeasureString(c.Text, txtFBAccessToken.Font).Height;
                    cb.CheckStateChanged += cb_CheckStateChanged;
                    flpCustomAttributes.GetNextControl(cb, false).Text = "     ";

                    PictureBox pbCheckBox = new PictureBox();
                    if (cb.Checked)
                        pbCheckBox.Image = global::Parafait_Kiosk.Properties.Resources.tick_box;
                    else
                        pbCheckBox.Image = global::Parafait_Kiosk.Properties.Resources.tick_box_blank;
                    pbCheckBox.Size = new System.Drawing.Size(36, cb.Height);
                    pbCheckBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
                    pbCheckBox.Tag = cb;
                    pbCheckBox.Click += new System.EventHandler(pbCheckBox_Click);

                    addedControls.Add(pbCheckBox);
                    addAtIndex.Add(flpCustomAttributes.Controls.GetChildIndex(cb));

                    Button lblCheckBox = new Button();
                    lblCheckBox.Text = MessageUtils.getMessage(cb.Text);
                    lblCheckBox.AutoSize = false;
                    lblCheckBox.Width = txtFirstName.Width - pbCheckBox.Width + lblBirthDate.Width + txtFirstName.Width;
                    lblCheckBox.Height = cb.Height;
                    lblCheckBox.TextAlign = ContentAlignment.MiddleLeft;
                    lblCheckBox.Tag = cb;
                    lblCheckBox.FlatStyle = FlatStyle.Flat;
                    lblCheckBox.FlatAppearance.BorderSize = 0;
                    lblCheckBox.FlatAppearance.CheckedBackColor = 
                        lblCheckBox.FlatAppearance.MouseDownBackColor = 
                        lblCheckBox.FlatAppearance.MouseOverBackColor = Color.Transparent;
                    lblCheckBox.BackColor = Color.Transparent;
                    lblCheckBox.ForeColor = txtCardNumber.ForeColor;
                    lblCheckBox.Click += new System.EventHandler(pbCheckBox_Click);

                    addedControls.Add(lblCheckBox);
                    addAtIndex.Add(flpCustomAttributes.Controls.GetChildIndex(cb));
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
                        case "LAST_NAME": txtLastName.Visible = flpCustomerMain.GetNextControl(txtLastName, false).Visible = visible; txtLastName.Tag = val; break;
                        case "ADDRESS1": flpCustomerMain.GetNextControl(textBoxAddress1, false).Visible = visible; textBoxAddress1.Visible = visible; textBoxAddress1.Tag = val; break;
                        case "ADDRESS2": flpCustomerMain.GetNextControl(textBoxAddress2, false).Visible = textBoxAddress2.Visible = visible; textBoxAddress2.Tag = val; break;
                        case "CITY": flpCustomerMain.GetNextControl(textBoxCity, false).Visible = textBoxCity.Visible = visible; textBoxCity.Tag = val; break;
                        case "STATE":
                            {
                                if (!visible)
                                {
                                    flpCustomerMain.GetNextControl(cmbState, false).Visible = cmbState.Visible = visible;
                                }
                                else
                                {
                                    countryId = Utilities.getParafaitDefaults("STATE_LOOKUP_FOR_COUNTRY");
                                    List<StateDTO> stateList = null;
                                    StateDTOList stateDTOList = new StateDTOList(Utilities.ExecutionContext);
                                    List<KeyValuePair<StateDTO.SearchByParameters, string>> searchStateParams = new List<KeyValuePair<StateDTO.SearchByParameters, string>>();
                                    searchStateParams.Add(new KeyValuePair<StateDTO.SearchByParameters, string>(StateDTO.SearchByParameters.SITE_ID, Utilities.ExecutionContext.GetSiteId().ToString()));
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
                                    cmbState.Visible = visible;
                                }
                            } break;
                        case "TITLE":
                            {
                                if (!visible)
                                {
                                    flpCustomerMain.GetNextControl(cmbTitle, false).Visible = cmbTitle.Visible = visible;
                                }
                                else
                                {
                                    cmbTitle.DataSource = Utilities.executeDataTable("select LookupValue title from LookupView where LookupName = 'CUSTOMER_TITLES' union all select '' order by 1");
                                    cmbTitle.ValueMember =
                                    cmbTitle.DisplayMember = "title";
                                    cmbTitle.Tag = val;
                                    cmbTitle.Visible = visible;
                                }
                            } break;
                        case "COUNTRY":
                            {
                                if (!visible)
                                {
                                    flpCustomerMain.GetNextControl(cmbCountry, false).Visible = cmbCountry.Visible = lblCountry.Visible = visible;
                                }
                                else
                                {
                                    List<CountryDTO> countryList = null;
                                    CountryDTOList countryDTOList = new CountryDTOList(Utilities.ExecutionContext);
                                    List<KeyValuePair<CountryDTO.SearchByParameters, string>> searchCountryParams = new List<KeyValuePair<CountryDTO.SearchByParameters, string>>();
                                    searchCountryParams.Add(new KeyValuePair<CountryDTO.SearchByParameters, string>(CountryDTO.SearchByParameters.SITE_ID, Utilities.ExecutionContext.GetSiteId().ToString()));
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
                                }
                            }
                            break;
                        case "PIN": flpCustomerMain.GetNextControl(textBoxPin, false).Visible = textBoxPin.Visible = visible; textBoxPin.Tag = val; break;
                        case "EMAIL": lblEmail.Visible = textBoxEmail.Visible = visible; textBoxEmail.Tag = val; break;
                        case "BIRTH_DATE": txtBirthDate.Visible = lblBirthDate.Visible = visible; txtBirthDate.Tag = val; break;
                        case "GENDER": flpCustomerMain.GetNextControl(comboBoxGender, false).Visible = comboBoxGender.Visible = visible; comboBoxGender.Tag = val; break;
                        case "ANNIVERSARY": flpCustomerMain.GetNextControl(textBoxAnniversaryDate, false).Visible = textBoxAnniversaryDate.Visible = visible; textBoxAnniversaryDate.Tag = val; break;
                        case "CONTACT_PHONE": textBoxContactPhone1.Visible = lblPhone.Visible = visible; textBoxContactPhone1.Tag = val; break;
                        case "NOTES": flpCustomerMain.GetNextControl(textBoxNotes, false).Visible = textBoxNotes.Visible = visible; textBoxNotes.Tag = val; break;
                        case "UNIQUE_ID": flpCustomerMain.GetNextControl(txtUniqueId, false).Visible = txtUniqueId.Visible = visible; txtUniqueId.Tag = val; break;
                        case "CUSTOMER_PHOTO": pbCapture.Visible = lblPhoto.Visible = visible; pbCapture.Name = val; break;
                        case "FBUSERID": flpCustomerMain.GetNextControl(txtFBUserId, false).Visible = txtFBUserId.Visible = visible;; txtFBUserId.Tag = val; break;
                        case "FBACCESSTOKEN": flpCustomerMain.GetNextControl(txtFBAccessToken, false).Visible = txtFBAccessToken.Visible = visible; txtFBAccessToken.Tag = val; break;
                        case "OPT_IN_PROMOTIONS": pnlOptinPromotions.Visible = visible; btnOptinPromotions.Tag = val; break;
                        case "OPT_IN_PROMOTIONS_MODE": lblPromotionMode.Visible = cmbPromotionMode.Visible = visible; cmbPromotionMode.Tag = val; break;
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
                catch(Exception ex)
                {
                    log.Error(ex.Message);
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

            if (flpCustomVisible)
            {
                flpCustomAttributes.Height = (flpCustomAttributes.Controls.Count / 2) * (flpCustomAttributes.Controls[1].Height + flpCustomAttributes.Controls[1].Margin.Top + flpCustomAttributes.Controls[1].Margin.Bottom);
            }

            Utilities.setLanguage(this);

            foreach (Control c in flpCustomerMain.Controls)
            {
                if (c.Tag != null && c.Tag.ToString().Equals("M"))
                {
                    flpCustomerMain.GetNextControl(c, false).Text += "*";
                }
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
            log.LogMethodExit();
        }

        void cb_CheckStateChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            foreach (Control c in flpCustomAttributes.Controls)
            {
                if (sender.Equals(c.Tag) && c.GetType().ToString().Contains("Picture"))
                {
                    if ((sender as CheckBox).Checked)
                        (c as PictureBox).Image = Properties.Resources.tick_box;
                    else
                        (c as PictureBox).Image = Properties.Resources.tick_box_blank;
                }
            }
            log.LogMethodExit();
        }

        void rb_CheckedChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
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
                flpCustomerMain.Location = new Point((this.Width - flpCustomerMain.Width - panelButtons.Width) / 2, flpCustomerMain.Location.Y);
                panelButtons.Location = new Point(flpCustomerMain.Location.X + flpCustomerMain.Width, panelButtons.Location.Y);

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
                    Close();
                }
                log.LogMethodExit();
                return;
            }

            showhideKeypad(); // initialize
            if (!termsAndConditions && Utilities.getParafaitDefaults("TERMS_AND_CONDITIONS").Equals("M"))
            {
                pnlTermsAndConditions.Visible = true;
            }
            else
            {
                pnlTermsAndConditions.Visible = false;
            }
            displayCustomerDetails();
            maskDisplay(false);

            if (KioskStatic.Utilities.ParafaitEnv.MIFARE_CARD && KioskStatic.TopUpReaderDevice != null)
            {
                if (KioskStatic.cardAcceptor != null)
                    KioskStatic.cardAcceptor.AllowAllCards();
            }

            if (KioskStatic.TopUpReaderDevice != null)
                KioskStatic.TopUpReaderDevice.Register(new EventHandler(CardScanCompleteEventHandle));

            
            Cursor.Show();
                       
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
                if (tagNumberParser.TryParse(checkScannedEvent.Message, out tagNumber) == false)
                {
                    string message = tagNumberParser.Validate(checkScannedEvent.Message);
                    displayMessageLine(message, ERROR);
                    log.LogMethodExit(null, "Invalid Tag Number. " + message);
                    log.LogMethodExit();
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
                    log.Error("Error occurred while executing CardScanCompleteEventHandle" + ex.Message);
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
                //secondsRemaining = timeOutSecs;
                ResetKioskTimer();
                string cardNumber = inCardNumber;
                if (CurrentCard.CardNumber == cardNumber)
                    maskDisplay(true);
                else
                {
                    displayMessageLine(Utilities.MessageUtils.getMessage(172), ERROR);
                    if (KioskStatic.Utilities.ParafaitEnv.MIFARE_CARD && KioskStatic.cardAcceptor != null)
                    {
                        KioskStatic.cardAcceptor.AllowAllCards();
                    }
                }
            }
            log.LogMethodExit();
        }


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
                this.Close();
                Dispose();
            }
            log.LogMethodExit();
        }

        private void textBoxContactPhone1_KeyPress(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry();
            if (!char.IsNumber(e.KeyChar) && !char.IsControl(e.KeyChar) && !(e.KeyChar == '+'))
                e.Handled = true;
            log.LogMethodExit();
        }

        private void Customer_KeyPress(object sender, KeyPressEventArgs e)
        {
            //secondsRemaining = timeOutSecs;
            log.LogMethodEntry();
            ResetKioskTimer();
            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            Close();
            Dispose();
            log.LogMethodExit();
        }

        private void Customer_FormClosing(object sender, FormClosingEventArgs e)
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
                KioskStatic.logToFile(this.Name + ": TopUp Reader unregistered");
            }

            //TimeOutTimer.Stop();
            try
            {
                keypad.Close();
            }
            catch(Exception ex)
            {
                log.Error("Error occurred while executing Customer_FormClosing()" + ex.Message);
            }
            Cursor.Hide();

            Audio.Stop();
            log.LogMethodExit();
        }

        private void textBox_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if ((sender as TextBox).Enabled)
                CurrentActiveTextBox = sender as TextBox;
            if (keypad != null && !keypad.IsDisposed)
            {
                keypad.currentTextBox = CurrentActiveTextBox;
                //12-Jun-2015:: Modified to make first alphabet of text in Upper case            
                if (CurrentActiveTextBox.Text == "")
                {
                    if (CurrentActiveTextBox == textBoxEmail)
                    {
                        keypad.LowerCase();
                    }
                    else
                    {
                        keypad.FirstKeyPressed = false;
                        keypad.UpperCase();
                    }
                }
            }
            log.LogMethodExit();
        }

        AlphaNumericKeyPad keypad;
        public TextBox CurrentActiveTextBox;
        private void btnShowKeyPad_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            showhideKeypad('T'); // toggle
            log.LogMethodExit();
        }

        void showhideKeypad(char mode = 'N') // T for toggle, S for show and H for hide, 'N' for nothing
        {
            log.LogMethodEntry(mode);
            if (keypad == null || keypad.IsDisposed)
            {
                if (CurrentActiveTextBox == null)
                    CurrentActiveTextBox = new TextBox();
                keypad = new AlphaNumericKeyPad(this, CurrentActiveTextBox, KioskStatic.CurrentTheme.KeypadSizePercentage);
                keypad.Location = new Point((this.Width - keypad.Width) / 2, this.Height - keypad.Height-50 );                
            }

            if (mode == 'T')
            {
                if (keypad.Visible)
                    keypad.Hide();
                else
                {
                    keypad.Location = new Point((this.Width - keypad.Width) / 2, this.Height - keypad.Height - 50);
                    keypad.Show();
                }
            }
            else if (mode == 'S')
            {
                keypad.Location = new Point((this.Width - keypad.Width) / 2, this.Height - keypad.Height - 50);
                keypad.Show();
            }
            else if (mode == 'H')
                keypad.Hide();
            log.LogMethodExit();
        }

        private void pbCapture_Click(object sender, EventArgs e)
        {
            //secondsRemaining = timeOutSecs;
            //TimeOutTimer.Stop();
            log.LogMethodEntry();
            ResetKioskTimer();
            StopKioskTimer();
            try
            {
                CustomerPhoto cp = new CustomerPhoto(ImageDirectory, pbCapture.Image, Utilities);
                cp.StartPosition = FormStartPosition.Manual;
                cp.Location = new Point(this.Width / 2 - cp.Width / 2, lblSiteName.Bottom - 23);
                if (cp.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    pbCapture.Image = cp.CustomerImage;
                    pbCapture.Tag = cp.PhotoFileName;
                    pbCapture.Height = (int)((decimal)pbCapture.Width / ((decimal)pbCapture.Image.Width / (decimal)pbCapture.Image.Height));
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
           // (sender as Button).BackgroundImage = Properties.Resources.save_cancel_btn;
        }

        private void buttonCustomerSave_MouseDown(object sender, MouseEventArgs e)
        {
            //(sender as Button).BackgroundImage = Properties.Resources.save_cancel_btn_pressed;
        }
        

        private void cmbTitle_SelectedIndexChanged(object sender, EventArgs e)//starts:Modification on 17-Dec-2015 for introducing new theme
        {
            log.LogMethodEntry();
            //secondsRemaining = timeOutSecs;
            ResetKioskTimer();
            log.LogMethodExit();
        }//Ends:Modification on 17-Dec-2015 for introducing new theme

        private void pnlOptinPromotions_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (pnlOptinPromotions.Tag.Equals(true))
            {
                pnlOptinPromotions.Tag = false;
                btnOptinPromotions.Image = Properties.Resources.checkbox_unchecked;
            }
            else
            {
                pnlOptinPromotions.Tag = true;
                btnOptinPromotions.Image = Properties.Resources.checkbox_checked;
            }
            log.LogMethodExit();
        }

        private void pnlTermsAndConditions_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (pnlTermsAndConditions.Tag.Equals("false"))
            {
                StopKioskTimer();
                showhideKeypad('H');
                using (frmRegisterTnC frmRegisterTnC = new frmRegisterTnC())
                {
                    if (frmRegisterTnC.ShowDialog() == DialogResult.Yes)
                    {
                        pnlTermsAndConditions.Tag = true;
                        btnTermsAndCondition.Image = Properties.Resources.checkbox_checked;
                    }
                }
                showhideKeypad('S');
            }
            StartKioskTimer();
            ResetKioskTimer();
            log.LogMethodExit();
        }
    }
}
