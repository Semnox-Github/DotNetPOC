/********************************************************************************************
 * Project Name - Customer
 * Description  - user interface
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By          Remarks          
 *********************************************************************************************
 *1.00                                             Created 
 *2.4.0       25-Nov-2018      Raghuveera          Added new fields for Optin romotions ,Promotion mode and terms and condition
 *2.70         1-Jul-2019      Lakshminarayana     Modified to add support for ULC cards
 *2.80         3-Sep-2019      Deeksha             Added logger methods.
 *2.80        12-Dec-2019      Girish Kundar       Modified :for Domain name validation changes  
 *2.70.2      18-Feb-2020      Girish Kundar       Modified : Date format issue fix
 *2.80.1      02-Feb-2021      Deeksha             Theme changes to support customized Images/Font
 *2.130.0     30-Jun-2021      Dakshakh            Theme changes to support customized Font ForeColor
 *2.150.0.0   13-Oct-2022      Sathyavathi         Mask card number
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer;
using Semnox.Parafait.KioskCore;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.Product;
namespace Parafait_Kiosk
{
    public partial class Customer : BaseFormKiosk
    {
        Card CurrentCard;
        public CustomerDTO customerDTO;

        Utilities Utilities = KioskStatic.Utilities;
        MessageUtils MessageUtils = KioskStatic.Utilities.MessageUtils;
        ParafaitEnv ParafaitEnv = KioskStatic.Utilities.ParafaitEnv;

        const string WARNING = "WARNING";
        const string ERROR = "ERROR";
        const string MESSAGE = "MESSAGE";

        string ImageDirectory;
        DateTime _birthDate = DateTime.MinValue;

        Font savTimeOutFont;
        Font TimeOutFont;

        string countryId = null;
        private readonly TagNumberParser tagNumberParser;

        public Customer(string CardNumber, object BirthDate = null)
        {
            log.LogMethodEntry(CardNumber, BirthDate);
            Utilities.setLanguage();
            InitializeComponent();
            lblTimeRemaining.Text = "";

            tagNumberParser = new TagNumberParser(Utilities.ExecutionContext);

            KioskStatic.formatMessageLine(textBoxMessageLine, 21, Properties.Resources.bottom_bar);
            KioskStatic.setDefaultFont(this);

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

            this.ShowInTaskbar = false;

            lblSiteName.Text = KioskStatic.SiteHeading;
            //lblRegistrationMessage.Text = KioskStatic.CustomerScreenMessage2;
            lblRegistrationMessage.Text = Utilities.MessageUtils.getMessage(1204);
            lblRegistrationMessage2.Text = Utilities.MessageUtils.getMessage(1205);

            //textBoxMessageLine.ForeColor = lblRegistrationMessage.ForeColor = KioskStatic.CurrentTheme.ScreenHeadingForeColor;
            //textBoxMessageLine.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(117)))), ((int)(((byte)(47)))), ((int)(((byte)(138)))));
            //textBoxMessageLine.ForeColor = KioskStatic.CurrentTheme.TextForeColor;
            //KioskStatic.setFieldLabelForeColor(flpCustomerMain);
            //KioskStatic.setFieldLabelForeColor(flpCustomAttributes);

            this.BackgroundImage = KioskStatic.CurrentTheme.RegistrationBackgroundImage;
            lblRegistrationMessage.Visible = false;//Ends:Modification on 17-Dec-2015 for introducing new theme
            KioskStatic.LoadPromotionModeDropDown(cmbPromotionMode);
            initializeCustomerInfo();
            lblOptinPromotions.Text = Utilities.MessageUtils.getMessage(1739);
            lblPromotionMode.Text = Utilities.MessageUtils.getMessage(1740);
            pnlOptinPromotions.Tag = false;
            lblDateFormat.Text = KioskStatic.Utilities.ParafaitEnv.DATE_FORMAT.ToUpper();
            flpCustomerMain.AutoScroll = true;
            flpCustomerMain.HorizontalScroll.Visible = false;
            if (BirthDate != null)
                _birthDate = Convert.ToDateTime(BirthDate);

            SetBackGroundColorAndImage();
            SetCustomizedFontColors();
            log.LogMethodExit();
        }

        private void SetBackGroundColorAndImage()
        {
            log.LogMethodEntry();
            try
            {
                foreach (Control c in flpCustomerMain.Controls)
                {
                    string type = c.GetType().ToString().ToLower();
                    if (type.Contains("panel"))
                    {
                        foreach (Control lbl in c.Controls)
                        {
                            lbl.BackColor = KioskStatic.CurrentTheme.TextBackGroundColor;//Products buttons 
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                foreach (Control c in flpCustomerMain.Controls)
                {
                    string type = c.GetType().ToString().ToLower();
                    if (type.Contains("panel"))
                    {
                        foreach (Control lbl in c.Controls)
                        {
                            lbl.BackColor = Color.White;//Products buttons 
                        }
                    }
                }
                log.Error(ex);
            }
            foreach (Control c in flpCustomerMain.Controls)
            {
                string type = c.GetType().ToString().ToLower();
                if (type.Contains("panel"))
                {
                    c.BackgroundImage = KioskStatic.CurrentTheme.TextBoxBackgroundImage;
                }
            }
            log.LogMethodExit();
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
                textBoxPin.Clear();
                if (countryId != null && countryId != "-1")
                {
                    cmbCountry.SelectedValue = Convert.ToInt32(countryId);
                    cmbCountry.Enabled = false;
                }
                textBoxContactPhone1.Clear();
                textBoxEmail.Clear();
                if (_birthDate != DateTime.MinValue)
                    txtBirthDate.Text = _birthDate.ToString(Utilities.getDateFormat());
                else
                    txtBirthDate.Clear();
                comboBoxGender.Text = "";
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
                    catch { cmbTitle.SelectedIndex = 0; }
                }
                txtLastName.Text = CurrentCard.customerDTO.LastName;
                if (CurrentCard.customerDTO.AddressDTOList != null)
                {
                    try
                    {
                        cmbState.SelectedValue = CurrentCard.customerDTO.AddressDTOList[0].StateId;
                        textBoxPin.Text = CurrentCard.customerDTO.AddressDTOList[0].PostalCode;
                    }
                    catch { cmbState.SelectedIndex = 0; }
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
                catch
                {

                }
                textBoxContactPhone1.Text = CurrentCard.customerDTO.PhoneNumber;
                textBoxEmail.Text = CurrentCard.customerDTO.Email;
                pnlOptinPromotions.Tag = CurrentCard.customerDTO.OptInPromotions;
                if (CurrentCard.customerDTO.OptInPromotions)
                {
                    pbOpinPromotions.Image = Properties.Resources.tick_box_checked;
                }
                else
                {
                    pbOpinPromotions.Image = Properties.Resources.tick_box_unchecked;
                }
                cmbPromotionMode.SelectedValue = CurrentCard.customerDTO.OptInPromotionsMode;

                if (CurrentCard.customerDTO.DateOfBirth != null && CurrentCard.customerDTO.DateOfBirth != DateTime.MinValue)
                {
                    txtBirthDate.Text = CurrentCard.customerDTO.DateOfBirth.Value.ToString(Utilities.getDateFormat());
                }
                else
                {
                    txtBirthDate.Text = "";
                }
                if (CurrentCard.customerDTO.AddressDTOList != null && CurrentCard.customerDTO.AddressDTOList.Count > 0)
                {
                    AddressDTO addressDTO = CurrentCard.customerDTO.AddressDTOList.Where((x) => x.IsActive).OrderByDescending((x) => x.LastUpdateDate).FirstOrDefault();
                    if (addressDTO != null)
                    {
                        textBoxCity.Text = addressDTO.City;
                    }
                }

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

                        Control cc = null;
                        if (type.Contains("panel") && c.Controls.Count > 0)
                        {
                            cc = c.Controls[0];

                            if (cc != null)
                                cc.Enabled = true;
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
                    textBoxEmail.PasswordChar =
                    textBoxContactPhone1.PasswordChar = '\0';
                    pbTermsCheckBox.Enabled = true;
                    chkReadPrivacyConfirm.Enabled = true;
                    lnkTermsAndPrivacy.Enabled = true;

                    if (countryId != null && countryId != "-1")
                        cmbCountry.Enabled = false;
                    buttonCustomerSave.Enabled = true;
                    pnlOptinPromotions.Enabled = inAllowEdit;
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
                        Control cc = null;
                        if (type.Contains("panel") && c.Controls.Count > 0)
                            cc = c.Controls[0];
                        {
                            if (cc != null)
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
                    textBoxContactPhone1.PasswordChar = 'X';
                    pnlOptinPromotions.Enabled = inAllowEdit;
                    pbTermsCheckBox.Enabled = false;
                    chkReadPrivacyConfirm.Enabled = false;
                    lnkTermsAndPrivacy.Enabled = false;

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
            else
            {
                showhideKeypad('S');
            }

            txtFirstName.Focus();
            log.LogMethodExit();
        }

        void displayMessageLine(string message, string msgType)
        {
            //switch (msgType)
            //{
            //    case "WARNING": textBoxMessageLine.BackColor = Color.Yellow; textBoxMessageLine.ForeColor = Color.Black; break;
            //    case "ERROR": textBoxMessageLine.BackColor = Color.Red; textBoxMessageLine.ForeColor = Color.White; break;
            //    case "MESSAGE": textBoxMessageLine.BackColor = KioskStatic.MessageLineBackColor; textBoxMessageLine.ForeColor = KioskStatic.CurrentTheme.ScreenHeadingForeColor; break;
            //    default: textBoxMessageLine.ForeColor = Color.Black; break;
            //}
            log.LogMethodEntry(message, msgType);
            textBoxMessageLine.Text = message;
            //textBoxMessageLine.ForeColor = KioskStatic.CurrentTheme.TextForeColor;
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
            ResetKioskTimer();
            showhideKeypad('H');
            bool displayErrorInPopup = true;
            frmOKMsg frmOK;
            txtFirstName.BackColor = txtCardNumber.BackColor;
            txtLastName.BackColor = txtCardNumber.BackColor; //11-Jun-2015:: Make color as default background for Last name
            textBoxContactPhone1.BackColor = textBoxEmail.BackColor = cmbPromotionMode.BackColor = txtCardNumber.BackColor;
            pbOpinPromotions.BackColor = pbTermsCheckBox.BackColor = Color.Transparent;
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
                    txtFirstName.Focus();
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
                    txtFirstName.Focus();
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
                        txtLastName.Focus();
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
                    textBoxEmail.BackColor = Color.OrangeRed;
                    textBoxEmail.Focus();
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
                    textBoxContactPhone1.Focus();
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
                        textBoxContactPhone1.Focus();
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
                            textBoxContactPhone1.BackColor = Color.OrangeRed;
                            textBoxContactPhone1.Focus();
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
                    DateTime datofBirthValue;
                    if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(Utilities.ExecutionContext, "IGNORE_CUSTOMER_BIRTH_YEAR"))
                    {
                        datofBirthValue = DateTime.ParseExact(txtBirthDate.Text + "/1904", dateMonthformat + "/yyyy", provider);
                    }
                    else
                    {
                        datofBirthValue = DateTime.ParseExact(txtBirthDate.Text, dateMonthformat, provider);
                    }
                    if (datofBirthValue != customerDTO.DateOfBirth)
                    {
                        customerDTO.DateOfBirth = datofBirthValue;
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
                        txtBirthDate.BackColor = Color.OrangeRed;
                        txtBirthDate.Focus();
                        log.LogMethodExit();
                        return;
                    }
                }
                catch
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
                            txtBirthDate.BackColor = Color.OrangeRed;
                            txtBirthDate.Focus();
                            log.LogMethodExit();
                            return;
                        }
                        //End Modification - 24 Apr 2015 - Birth Date validation
                    }
                    catch
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
                        txtBirthDate.BackColor = Color.OrangeRed;
                        txtBirthDate.Focus();
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
            customerDTO.OptInPromotions = Convert.ToBoolean(pnlOptinPromotions.Tag);
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
                        using (frmOK = new frmOKMsg(MessageUtils.getMessage(450), true))
                        {
                            frmOK.ShowDialog();
                        }
                        ResetKioskTimer();
                        StartKioskTimer();
                    }
                    textBoxEmail.Focus();
                    textBoxEmail.BackColor = Color.OrangeRed;
                    textBoxEmail.Focus();
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
                            displayMessageLine(MessageUtils.getMessage(249, cmbState.Text.TrimEnd(':', '*')), ERROR);
                            if (displayErrorInPopup)
                            {
                                StopKioskTimer();
                                using (frmOK = new frmOKMsg(MessageUtils.getMessage(249, cmbState.Text.TrimEnd(':', '*')), true))
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
            if (pnlOptinPromotions.Tag.Equals(false) && KioskStatic.Utilities.getParafaitDefaults("OPT_IN_PROMOTIONS").Equals("M"))
            {
                displayMessageLine(MessageUtils.getMessage(249, lblOptinPromotions.Text.TrimEnd(':', '*')), ERROR);
                this.ActiveControl = pbOpinPromotions;
                pbOpinPromotions.BackColor = Color.OrangeRed;
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
                    addressDTO.AddressType = AddressType.HOME;
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
                contactEmailDTO.Attribute1 = textBoxEmail.Text.Trim();

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
                    if (chkReadPrivacyConfirm.Checked)
                    {
                        customerDTO.PolicyTermsAccepted = true;
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
                    }
                    else
                    {
                        displayMessageLine(MessageUtils.getMessage(1206), ERROR);
                        pbTermsCheckBox.BackColor = Color.OrangeRed;
                        return;
                    }
                    int CustomDataSetId = customerDTO.CustomDataSetId;
                    if (CustomAttributes.Save((int)customerDTO.Id, ref CustomDataSetId))
                    {
                        customerDTO.CustomDataSetId = CustomDataSetId;
                        CustomDataSetDTO customDataSetDTO = (new CustomDataSetBL(Utilities.ExecutionContext, CustomDataSetId)).CustomDataSetDTO;
                        customerDTO.CustomDataSetDTO = customDataSetDTO;
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
                                SqlTransaction sqlTransaction = connection.BeginTransaction();
                                try
                                {
                                    CurrentCard.customerDTO = customerDTO;
                                    CurrentCard.createCard(sqlTransaction);
                                    sqlTransaction.Commit();
                                }
                                catch (Exception ex)
                                {
                                    sqlTransaction.Rollback();
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
                    if (chkReadPrivacyConfirm.Checked)
                    {
                        customerDTO.PolicyTermsAccepted = true;
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
                    }
                    else
                    {

                        displayMessageLine(MessageUtils.getMessage(1206), ERROR);
                        pbTermsCheckBox.BackColor = Color.OrangeRed;
                        return;
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

                //displayMessageLine(MessageUtils.getMessage(452), MESSAGE);

                //12-Jun-2015 - Custom message for first time registration
                frmOKMsg frm;
                if (firstTimeRegistration)
                {
                    if (KioskStatic.RegistrationBonusAmount > 0 && CurrentCard != null &&
                        ((ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "REGISTRATION_BONUS_ON_VERIFICATION").Equals("N"))))
                    {
                        frm = new frmOKMsg(MessageContainerList.GetMessage(Utilities.ExecutionContext, 927, KioskStatic.RegistrationBonusAmount) + ". " + MessageUtils.getMessage(452));
                    }
                    else if (KioskStatic.RegistrationBonusAmount > 0 && CurrentCard != null &&
                        ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "REGISTRATION_BONUS_ON_VERIFICATION").Equals("Y"))
                    {
                        frm = new frmOKMsg(MessageContainerList.GetMessage(Utilities.ExecutionContext, 1746, KioskStatic.RegistrationBonusAmount) + ". " + MessageUtils.getMessage(452));
                    }
                    else
                    {
                        frm = new frmOKMsg(MessageContainerList.GetMessage(Utilities.ExecutionContext, MessageUtils.getMessage(926) + ". " + MessageUtils.getMessage(452)));
                    }
                    firstTimeRegistration = false;
                }
                else
                {
                    frm = new frmOKMsg(MessageUtils.getMessage(452));
                }
                frm.ShowDialog();
                Close();
                //if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                //{
                //    Close();
                //}
                //else
                //{
                //    maskDisplay(false);
                //    if (KioskStatic.Utilities.ParafaitEnv.MIFARE_CARD && KioskStatic.cardAcceptor != null)
                //    {
                //        KioskStatic.cardAcceptor.AllowAllCards();
                //    }
                //}
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
            ResetKioskTimer();
            log.LogMethodExit();
        }

        void SendVerificationCode() //17-Jun-2015:Method for sending verification code
        {
            //sends mail and/or SMS to the customer
            try
            {
                ResetKioskTimer();
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
            catch
            {

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
            CheckBox chb = null;
            if (sender.GetType().ToString().ToLower().Contains("picturebox"))
            {
                chb = (sender as PictureBox).Tag as CheckBox;
            }
            else if (sender.GetType().ToString().ToLower().Contains("button"))
            {
                chb = (sender as Button).Tag as CheckBox;
            }
            if (chb != null)
            {
                if (chb.Checked)
                {
                    chb.Checked = false;
                }
                else
                {
                    chb.Checked = true;
                }
            }
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
                    c.ForeColor = txtFirstName.ForeColor;
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
                    c.ForeColor = txtFirstName.ForeColor;
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
                    // lblCheckBox.Text = "Opt in to Chuck E-Club® to receive special deals and offers";
                    lblCheckBox.Text = "I am a coach, teacher, or help in a school or youth organization";
                    lblCheckBox.AutoSize = false;
                    lblCheckBox.Width = flpCustomerMain.Width - lbl.Width - pbCheckBox.Width - 50;
                    lblCheckBox.Height = lblCardNumber.Height + 40;
                    lblCheckBox.TextAlign = ContentAlignment.MiddleLeft;
                    lblCheckBox.Tag = cb;
                    lblCheckBox.Font = new System.Drawing.Font("Bango Pro", 32F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
                                    cmbState.Visible = lblStateCombo.Visible = panel9.Visible = visible;
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
                                    cmbCountry.Visible = lblCountry.Visible = panel5.Visible = visible;
                                }
                                else
                                {
                                    lblCountry.Visible = cmbCountry.Parent.Visible = visible;
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
                                    if (val.Equals("M")) lblCountry.Text += "*";
                                }
                            }
                            break;
                        case "PIN": lblPostalCode.Visible = textBoxPin.Parent.Visible = visible; textBoxPin.Tag = val; if (val.Equals("M")) lblPostalCode.Text += "*"; break;
                        case "EMAIL": lblEmail.Visible = textBoxEmail.Parent.Visible = visible; textBoxEmail.Tag = val; if (val.Equals("M")) lblEmail.Text += "*"; break;
                        case "BIRTH_DATE": txtBirthDate.Parent.Visible = lblBirthDate.Visible = visible; txtBirthDate.Tag = val; if (val.Equals("M")) lblBirthDate.Text += "*"; break;
                        case "GENDER": lblGender.Visible = comboBoxGender.Parent.Visible = visible; comboBoxGender.Tag = val; if (val.Equals("M")) lblGender.Text += "*"; break;
                        case "CONTACT_PHONE": textBoxContactPhone1.Parent.Visible = lblPhone.Visible = visible; textBoxContactPhone1.Tag = val; if (val.Equals("M")) lblPhone.Text += "*"; break;
                        case "CUSTOMER_PHOTO": pbCapture.Visible = lblPhoto.Visible = visible; pbCapture.Name = val; if (val.Equals("M")) lblPhoto.Text += "*"; break;
                        case "OPT_IN_PROMOTIONS": pnlOptinPromotions.Visible = visible; pbOpinPromotions.Tag = val; break;
                        case "OPT_IN_PROMOTIONS_MODE": lblPromotionMode.Visible = pnlPromotionMode.Visible = cmbPromotionMode.Visible = visible; cmbPromotionMode.Tag = val; break;

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
                catch { }
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

            Utilities.setLanguage(this);

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
            btnCancel.Visible = false;
            //secondsRemaining = timeOutSecs;

            //TimeOutTimer = new Timer();
            //TimeOutTimer.Interval = 1000;
            //TimeOutTimer.Tick += new EventHandler(TimeOutTimer_Tick);
            //TimeOutTimer.Start();

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
            displayCustomerDetails();
            maskDisplay(false);
            if (KioskStatic.Utilities.getParafaitDefaults("AUTO_POPUP_ONSCREEN_KEYBOARD").Equals("Y"))
            {
                AddTextBoxClickEvent(flpCustomerMain);
            }

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
        private void AddTextBoxClickEvent(Control control)
        {
            log.LogMethodEntry(control);
            foreach (Control c in control.Controls)
            {
                if (c.HasChildren)
                {
                    AddTextBoxClickEvent(c);
                }
                else
                {
                    if (c.GetType().ToString().Contains("TextBox"))
                    {
                        c.Click += Textbox_Click;
                    }
                }
            }
            log.LogMethodExit();
        }
        private void Textbox_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            TextBox txt = CurrentActiveTextBox = (TextBox)sender;
            if (keypad != null)
            {
                if (CurrentActiveTextBox.ReadOnly)
                {
                    log.LogMethodExit();
                    return;
                }
                else
                {
                    keypad.currentTextBox = CurrentActiveTextBox;
                }
            }
            if (keypad.IsDisposed)
            {
                showhideKeypad('T');
            }
            log.LogMethodExit();
        }
        private void CardScanCompleteEventHandle(object sender, EventArgs e)
        {
            log.LogMethodExit();
            if (e is DeviceScannedEventArgs)
            {
                DeviceScannedEventArgs checkScannedEvent = e as DeviceScannedEventArgs;

                TagNumber tagNumber;
                if (tagNumberParser.TryParse(checkScannedEvent.Message, out tagNumber) == false)
                {
                    string message = tagNumberParser.Validate(checkScannedEvent.Message);
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
        //Begin Timer Cleanup
        //protected override CreateParams CreateParams 
        //{
        //    //this method is used to avoid the table layout flickering.
        //    get
        //    {
        //        CreateParams CP = base.CreateParams;
        //        CP.ExStyle = CP.ExStyle | 0x02000000;
        //        return CP;
        //    }
        //}  
        //End Timer Cleanup

        void arrange()//playpass:Starts
        {
            log.LogMethodEntry();
            lblSiteName.Top = this.Top + 50;

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
            if ((CurrentCard == null) || (CurrentCard == null && CurrentCard.customerDTO == null))
            {
                if (pnlOptinPromotions.Visible)
                {
                    pnlOptinPromotions.Top = flpCustomerMain.Bottom + 20;
                    panel16.Top = pnlOptinPromotions.Bottom + 10;//flpCustomerMain.Height + 180;
                    panelButtons.Top = panel16.Top + 200;
                }
                else
                {
                    panel16.Top = flpCustomerMain.Bottom + 20;
                    panelButtons.Top = panel16.Top + 200;
                }
            }
            log.LogMethodExit();
        }//playpass:Ends

        //void TimeOutTimer_Tick(object sender, EventArgs e)
        //{
        //    if (secondsRemaining <= 60)
        //    {
        //        lblTimeRemaining.Font = TimeOutFont;
        //        secondsRemaining = secondsRemaining - 1;
        //        lblTimeRemaining.Text = secondsRemaining.ToString("#0");                       
        //    }
        //    else
        //    {
        //        lblTimeRemaining.Font = savTimeOutFont;
        //        secondsRemaining = secondsRemaining - 1;
        //        lblTimeRemaining.Text = (secondsRemaining / 60).ToString() + ":" + (secondsRemaining % 60).ToString().PadLeft(2, '0');
        //    }

        //    if (secondsRemaining == 10)
        //    {
        //        if (TimeOut.AbortTimeOut(this))
        //        {
        //            secondsRemaining = timeOutSecs;
        //        }
        //        else
        //            secondsRemaining = 0;
        //    }

        //    if (secondsRemaining <= 0)
        //    {
        //        TimeOutTimer.Stop();
        //        displayMessageLine(MessageUtils.getMessage(457), WARNING);
        //        Application.DoEvents();
        //        this.Close();
        //        Dispose();
        //    }
        //}
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
            ResetKioskTimer();
            if (!char.IsNumber(e.KeyChar) && !char.IsControl(e.KeyChar) && !(e.KeyChar == '+'))
                e.Handled = true;
            log.LogMethodExit();
        }

        private void Customer_KeyPress(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry();
            //secondsRemaining = timeOutSecs;
            ResetKioskTimer();
            log.LogMethodExit();
        }

        //private void btnClose_Click(object sender, EventArgs e)
        //{
        //    Close();
        //    Dispose();
        //}

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

            // TimeOutTimer.Stop();
            try
            {
                keypad.Close();
            }
            catch { }
            Cursor.Hide();

            Audio.Stop();
            log.LogMethodExit();
        }

        private void textBox_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
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
            ResetKioskTimer();
            if (keypad == null || keypad.IsDisposed)
            {
                if (CurrentActiveTextBox == null)
                    CurrentActiveTextBox = new TextBox();
                keypad = new AlphaNumericKeyPad(this, CurrentActiveTextBox, KioskStatic.CurrentTheme.KeypadSizePercentage);
                // keypad.Location = new Point((this.Width - keypad.Width) / 2, (this.Height - keypad.Height) / 2);
            }

            if (mode == 'T')
            {
                if (keypad.Visible)
                    keypad.Hide();
                else
                {
                    //keypad.Location = new Point((this.Width - keypad.Width) / 2, (this.Height - keypad.Height) / 2);
                    keypad.Show();
                }
            }
            else if (mode == 'S')
            {
                //keypad.Location = new Point((this.Width - keypad.Width) / 2, (this.Height - keypad.Height)/2);
                keypad.Show();
            }
            else if (mode == 'H')
                keypad.Hide();
            log.LogMethodExit();
        }


        private void pbCapture_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            //secondsRemaining = timeOutSecs;
            //TimeOutTimer.Stop();
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
            // (sender as Button).BackgroundImage = Properties.Resources.save_cancel_btn_pressed;
        }

        private void lnkTerms_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            //TimeOutTimer.Stop();
            StopKioskTimer();
            if (keypad.IsDisposed == false)
                keypad.Hide();
            using (frmRegisterTnC fok = new frmRegisterTnC())
            {
                fok.TopMost = true;
                if (fok.ShowDialog() == System.Windows.Forms.DialogResult.Yes)
                {
                    chkReadPrivacyConfirm.Checked = true;

                }
            }
            //TimeOutTimer.Start();            
            StartKioskTimer();
            ResetKioskTimer();
            log.LogMethodExit();
        }
        private void chkReadPrivacyConfirm_CheckedChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (chkReadPrivacyConfirm.Checked)
                pbTermsCheckBox.Image = Properties.Resources.tick_box_checked;
            else
                pbTermsCheckBox.Image = Properties.Resources.tick_box_unchecked;
            ResetKioskTimer();
            log.LogMethodExit();
        }

        private void pbTermsCheckBox_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            chkReadPrivacyConfirm.Checked = !chkReadPrivacyConfirm.Checked;
            log.LogMethodExit();
        }
        private void pnlOptinPromotions_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            log.LogMethodEntry(sender, e);
            if (pnlOptinPromotions.Tag.Equals(true))
            {
                pnlOptinPromotions.Tag = false;
                pbOpinPromotions.Image = Properties.Resources.tick_box_unchecked;
            }
            else
            {
                pnlOptinPromotions.Tag = true;
                pbOpinPromotions.Image = Properties.Resources.tick_box_checked;
            }
            ResetKioskTimer();
            log.LogMethodExit();
        }

        private void verticalScrollBarView1_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            log.LogMethodExit();
        }
        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("Setting customized font colors for the UI elements");
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

                this.lblRegistrationMessage.ForeColor = KioskStatic.CurrentTheme.CustomerScreenHeader1TextForeColor;//(Join the chuck e - club) -
                this.lblRegistrationMessage2.ForeColor = KioskStatic.CurrentTheme.CustomerScreenHeader2TextForeColor;//(Get our best deals)
                this.lblOptinPromotions.ForeColor = KioskStatic.CurrentTheme.CustomerScreenOptInTextForeColor; //(Opt in promotion)
                this.chkReadPrivacyConfirm.ForeColor = KioskStatic.CurrentTheme.CustomerScreenPrivacyTextForeColor; //(I have Read and Agree to)
                this.lnkTermsAndPrivacy.LinkColor = KioskStatic.CurrentTheme.CustomerScreenInkTermsTextForeColor; //(Terms and Privacy Conditions)
                this.btnPrev.ForeColor = KioskStatic.CurrentTheme.CustomerScreenBtnPrevTextForeColor; //(Back button)
                this.buttonCustomerSave.ForeColor = KioskStatic.CurrentTheme.CustomerScreenBtnSaveTextForeColor; //(Save button) 
                this.textBoxMessageLine.ForeColor = KioskStatic.CurrentTheme.CustomerScreenFooterTextForeColor; //(Footer message)
                this.lblSiteName.ForeColor = KioskStatic.CurrentTheme.SiteHeadingForeColor;
            }
            catch (Exception ex)
            {
                log.Error("Errow while Setting Customized Fore Colors", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements: " + ex.Message);
            }
            log.LogMethodExit();
        }
    }
}
