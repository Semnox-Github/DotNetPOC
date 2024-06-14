/********************************************************************************************
 * Project Name - Redemption Kiosk
 * Description  - Kiosk Customer Registration form
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.4.0       3-Sep-2018       Archana            Created 
 *2.4.0       25-Nov-2018      Raghuveera         Added new fields for Optin romotions, Promotion mode and terms and condition
 *2.90.0      23-Jul-2020       Jinto Thomas      Modified: Verification Code generation by using customerverificationBL
 *                                                   and sending code to user by MessagingRequest  
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Customer.Accounts;

namespace Semnox.Parafait.Customer
{
    public partial class FrmKioskCustomer: FrmKioskBaseForm
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ExecutionContext machineUserContext;
        Utilities utilities;
        private string cardNumber;
        string countryId = null;
        string imageDirectory;
        DateTime birthDate = DateTime.MinValue;
        CustomAttributes customAttributes;
        protected AlphaNumericKeyPad keypad;
        protected TextBox currentActiveTextBox;
        protected CustomerDTO customerDTO;

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="utils">Utilities</param>
        /// <param name="cardNumber">string</param>
        /// <param name="BirthDate">object</param>
        public FrmKioskCustomer(ExecutionContext executionContext, Utilities utils, string cardNumber = null, object BirthDate = null)
        {
            log.LogMethodEntry(executionContext, utils, cardNumber, BirthDate);
            InitializeComponent();
            lblTimeRemaining.Text = "";
            txtMessageLine.Text = "";
            txtCardNumber.Text = "";
            machineUserContext = executionContext;
            utilities = utils;
            utilities.setLanguage(this);
            this.cardNumber = cardNumber;
            if (birthDate != null)
                this.birthDate = Convert.ToDateTime(BirthDate);
            if (string.IsNullOrEmpty(cardNumber) == false)
            {
                AccountBL accountBL = new AccountBL(machineUserContext, cardNumber, true, true);
                txtCardNumber.Text = cardNumber;
                if (accountBL.AccountDTO != null && accountBL.AccountDTO.CustomerId > -1)
                {
                    CustomerBL customerBL = new CustomerBL(machineUserContext, accountBL.AccountDTO.CustomerId);
                    customerDTO = customerBL.CustomerDTO;
                }
                else
                {
                    customerDTO = null;
                }
            }
            else
            {
                txtCardNumber.Parent.Visible = lblCardNumber.Visible = false;
            }
            this.ShowInTaskbar = false;
            InitializeCustomerInfo();
            log.LogMethodExit();
        }
        /// <summary>
        /// Method to arrange fields in the form
        /// </summary>
        void Arrange()
        {
            log.LogMethodEntry();
            for (int i = 0; i < flpCustomer.Controls.Count; i++)
            {
                if (flpCustomer.Controls[i].Visible == false)
                {
                    flpCustomer.Controls.Remove(flpCustomer.Controls[i]);
                    i = 0;
                }
            }

            flpCustomAttributes.Visible = flpCustomer.Visible = false;
            int fieldCount = 0;
            foreach (Control c in flpCustomer.Controls)
            {
                string type = c.GetType().ToString().ToLower();
                if (c.Equals(flpCustomAttributes))
                    continue;

                if (!type.Contains("label"))
                {
                    fieldCount++;
                }
            }
            flpCustomAttributes.Visible = flpCustomer.Visible = true;
            log.LogMethodExit();
        }


        /// <summary>
        /// Method to initialize customer information.
        /// </summary>
        protected void InitializeCustomerInfo()
        {
            log.LogMethodEntry();
            customAttributes = new CustomAttributes(CustomAttributes.Applicability.CUSTOMER, utilities);
            customAttributes.createUI(flpCustomAttributes);
            LoadPromotionModeDropDown();
            lblOptinPromotions.Text = utilities.MessageUtils.getMessage(1739);
            lblPromotionMode.Text = utilities.MessageUtils.getMessage(1740);
            lblTermsAndConditions.Text = utilities.MessageUtils.getMessage(1741);
            pnlOptinPromotions.Tag = pnlTermsandConditions.Tag= false;
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
                        pbCheckBox.Image = Properties.Resources.tick_box_checked;
                    else
                        pbCheckBox.Image = Properties.Resources.T_C_tick_box;
                    pbCheckBox.Size = pbCheckBox.Image.Size;
                    pbCheckBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Normal;

                    pbCheckBox.Tag = cb;
                    pbCheckBox.Click += new System.EventHandler(pbCheckBox_Click);

                    addedControls.Add(pbCheckBox);
                    addAtIndex.Add(flpCustomAttributes.Controls.GetChildIndex(cb));

                    Button lblCheckBox = new Button();
                    lblCheckBox.Text = cb.Text;
                    lblCheckBox.Text = utilities.MessageUtils.getMessage(954);//"Opt in to receive special deals and offers";
                    lblCheckBox.Font = lblEmail.Font;//Modification on 17-Dec-2015 for introducing new theme
                    lblCheckBox.AutoSize = false;
                    lblCheckBox.Width = flpCustomer.Width - lbl.Width - pbCheckBox.Width - 50;
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

            foreach (DataRow dr in utilities.executeDataTable(@"select default_value_name, isnull(pos.optionvalue, pd.default_value) value, cu.Name
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
                                                               new SqlParameter("POSMachineId", utilities.ParafaitEnv.POSMachineId)).Rows)
            {
                try
                {
                    log.LogVariableState("default_value_name", dr["default_value_name"]);
                    log.LogVariableState("value", dr["value"]);
                    string val = dr["value"].ToString();
                    bool visible = val == "N" ? false : true;
                    string defValName = dr["default_value_name"].ToString();
                    switch (dr["default_value_name"].ToString())
                    {
                        case "LAST_NAME": txtLastName.Parent.Visible = lblLastName.Visible = visible; txtLastName.Tag = val; if (val.Equals("M")) lblLastName.Text += "*"; break;
                        case "ADDRESS1": lblAddres1.Visible = visible; txtAddress1.Parent.Visible = visible; txtAddress1.Tag = val; if (val.Equals("M")) lblAddres1.Text += "*"; break;
                        case "ADDRESS2": lblAddress2.Visible = txtAddress2.Parent.Visible = visible; txtAddress2.Tag = val; if (val.Equals("M")) lblAddress2.Text += "*"; break;
                        case "CITY": lblCity.Visible = txtCity.Parent.Visible = visible; txtCity.Tag = val; if (val.Equals("M")) lblCity.Text += "*"; break;
                        case "STATE":
                            {
                                if (!visible)
                                {
                                    cmbState.Visible = lblStateCombo.Visible = pnl9.Visible = visible;
                                }
                                else
                                {
                                    countryId = utilities.getParafaitDefaults("STATE_LOOKUP_FOR_COUNTRY");
                                    List<StateDTO> stateList = null;
                                    StateDTOList stateDTOList = new StateDTOList(utilities.ExecutionContext);
                                    List<KeyValuePair<StateDTO.SearchByParameters, string>> searchStateParams = new List<KeyValuePair<StateDTO.SearchByParameters, string>>();
                                    searchStateParams.Add(new KeyValuePair<StateDTO.SearchByParameters, string>(StateDTO.SearchByParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));
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
                                    cmbTitle.DataSource = utilities.executeDataTable("select LookupValue title from LookupView where LookupName = 'CUSTOMER_TITLES' union all select '' order by 1");
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
                                    cmbCountry.Visible = lblCountry.Visible = pnl5.Visible = visible;
                                }
                                else
                                {
                                    lblCountry.Visible = cmbCountry.Parent.Visible = visible;
                                    List<CountryDTO> countryList = null;
                                    CountryDTOList countryDTOList = new CountryDTOList(utilities.ExecutionContext);
                                    List<KeyValuePair<CountryDTO.SearchByParameters, string>> searchCountryParams = new List<KeyValuePair<CountryDTO.SearchByParameters, string>>();
                                    searchCountryParams.Add(new KeyValuePair<CountryDTO.SearchByParameters, string>(CountryDTO.SearchByParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));
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
                        case "PIN": lblPostalCode.Visible = txtPinCode.Parent.Visible = visible; txtPinCode.Tag = val; if (val.Equals("M")) lblPostalCode.Text += "*"; break;
                        case "EMAIL": lblEmail.Visible = txtEmail.Parent.Visible = visible; txtEmail.Tag = val; if (val.Equals("M")) lblEmail.Text += "*"; break;
                        case "BIRTH_DATE": txtBirthDate.Parent.Visible = lblBirthDate.Visible = visible; txtBirthDate.Tag = val; if (val.Equals("M")) lblBirthDate.Text += "*"; break;
                        case "ANNIVERSARY": txtAnniversary.Parent.Visible = lblAnniversary.Visible = visible; txtAnniversary.Tag = val; if (val.Equals("M")) lblAnniversary.Text += "*"; break;
                        case "UNIQUE_ID": txtUniqueId.Parent.Visible = lblUniqueId.Visible = visible; txtUniqueId.Tag = val; if (val.Equals("M")) lblUniqueId.Text += "*"; break;
                        case "GENDER": lblGender.Visible = comboBoxGender.Parent.Visible = visible; comboBoxGender.Tag = val; if (val.Equals("M")) lblGender.Text += "*"; break;
                        case "CONTACT_PHONE": txtContactPhone1.Parent.Visible = lblPhone.Visible = visible; txtContactPhone1.Tag = val; if (val.Equals("M")) lblPhone.Text += "*"; break;
                        case "CUSTOMER_PHOTO": pbCapture.Visible = lblPhoto.Visible = visible; pbCapture.Name = val; if (val.Equals("M")) lblPhoto.Text += "*"; break;
                        case "OPT_IN_PROMOTIONS": pnlOptinPromotions.Visible = visible; btnOpinPromotions.Tag = val; break;
                        case "OPT_IN_PROMOTIONS_MODE": lblPromotionMode.Visible = pnlPromotionMode.Visible = cmbPromotionMode.Visible = visible; cmbPromotionMode.Tag = val; if (val.Equals("M")) lblPromotionMode.Text += "*"; break;
                        case "TERMS_AND_CONDITIONS": pnlTermsandConditions.Visible = visible; btnTermsandConditions.Tag = val; break;
                        default:
                            {
                                log.LogVariableState("name", dr["Name"]);
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
            flpCustomer.Visible = true;

            foreach (Control c in flpCustomAttributes.Controls)
            {
                if (c.GetType().ToString().Contains("TextBox"))
                {
                    (c as TextBox).Enter += new EventHandler(textBox_Enter);
                }
                else if (c.GetType().ToString().Contains("Check"))
                    c.Visible = false;
            }

            utilities.setLanguage(this);

            int width = 0;
            int.TryParse(utilities.getParafaitDefaults("CUSTOMER_PHONE_NUMBER_WIDTH"), out width);
            if (width > 0)
                txtContactPhone1.MaxLength = width;
            else
                txtContactPhone1.MaxLength = 20;

            imageDirectory = utilities.getParafaitDefaults("IMAGE_DIRECTORY") + "\\";
            log.LogMethodExit();
        }

        private void ShowHideKeypad(char mode = 'N') // T for toggle, S for show and H for hide, 'N' for nothing
        {
            log.LogMethodEntry();
            if (keypad == null || keypad.IsDisposed)
            {
                if (currentActiveTextBox == null)
                    currentActiveTextBox = new TextBox();
                keypad = new AlphaNumericKeyPad(this, currentActiveTextBox);
                keypad.Location = new Point((this.Width - keypad.Width) / 2, txtMessageLine.Top - keypad.Height);
            }

            if (mode == 'T')
            {
                if (keypad.Visible)
                    keypad.Hide();
                else
                {
                    keypad.Location = new Point((this.Width - keypad.Width) / 2, txtMessageLine.Top - keypad.Height);
                    keypad.Show();
                }
            }
            else if (mode == 'S')
            {
                keypad.Location = new Point((this.Width - keypad.Width) / 2, txtMessageLine.Top - keypad.Height);
                keypad.Show();
            }
            else if (mode == 'H')
                keypad.Hide();
            log.LogMethodExit();
        }
        private void cb_CheckStateChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            foreach (Control c in flpCustomAttributes.Controls)
            {
                if (sender.Equals(c.Tag) && c.GetType().ToString().Contains("Picture"))
                {
                    if ((sender as CheckBox).Checked)
                        (c as PictureBox).Image = Properties.Resources.tick_box_checked;
                    else
                        (c as PictureBox).Image = Properties.Resources.T_C_tick_box;
                }
            }
            log.LogMethodExit();
        }

        private void rb_CheckedChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            RadioButton rb = sender as RadioButton;
            if (rb.Checked)
                rb.BackgroundImage = Properties.Resources.RadioChecked;
            else
                rb.BackgroundImage = Properties.Resources.RadioUnChecked;
            log.LogMethodExit();
        }

        private void textBox_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if ((sender as TextBox).Enabled)
                currentActiveTextBox = sender as TextBox;
            if (keypad != null && !keypad.IsDisposed)
            {
                keypad.currentTextBox = currentActiveTextBox;
                if (currentActiveTextBox.Text == "")
                {
                    if (currentActiveTextBox == txtEmail)
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
        protected void DisplayMessageLine(string message)
        {
            log.LogMethodEntry(message);
            txtMessageLine.Text = message;
            log.LogMethodExit();
        }
        
        private void pbCheckBox_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ((sender as Control).Tag as CheckBox).Checked = !((sender as Control).Tag as CheckBox).Checked;
            log.LogMethodExit();
        }
        protected virtual void btnStartOver_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            log.LogMethodExit();
        }

        /// <summary>
        /// Method to validate email address
        /// </summary>
        /// <param name="emailAddress">string</param>
        /// <returns></returns>
        protected bool ValidateEmail(string emailAddress)
        {
            log.LogMethodEntry(emailAddress);
            bool functionReturnValue = false;
            string pattern = "^[a-zA-Z0-9][\\w\\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\\w\\.-]" + "*[a-zA-Z0-9]\\.[a-zA-Z][a-zA-Z\\.]*[a-zA-Z]$";
            Match emailAddressMatch = Regex.Match(emailAddress, pattern);
            if (emailAddressMatch.Success)
            {
                functionReturnValue = true;
            }
            else
            {
                functionReturnValue = false;
            }
            log.LogMethodExit(functionReturnValue);
            return functionReturnValue;

        }
        protected virtual void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            bool newCustomer = false;
            txtFirstName.BackColor = txtCardNumber.BackColor;
            txtLastName.BackColor = txtCardNumber.BackColor;
            txtContactPhone1.BackColor = txtEmail.BackColor =  txtCardNumber.BackColor;
            pnlPromotionMode.BackColor = Color.Transparent;
            if (string.IsNullOrEmpty(txtFirstName.Text.Trim()))
            {
                DisplayMessageLine(MessageContainerList.GetMessage(machineUserContext, 448));
                txtFirstName.BackColor = Color.OrangeRed;
                txtFirstName.Focus();
                return;
            }
            if (ParafaitDefaultContainerList.GetParafaitDefault(machineUserContext, "CUSTOMER_NAME_VALIDATION").Equals("Y"))
            {
                if (txtFirstName.Text.Length < 3)
                {
                    DisplayMessageLine(MessageContainerList.GetMessage(machineUserContext, 448, 3));
                    this.ActiveControl = txtFirstName;
                    txtFirstName.BackColor = Color.OrangeRed;
                    return;
                }

                if (!System.Text.RegularExpressions.Regex.IsMatch(txtFirstName.Text, @"^[a-zA-Z]+$"))
                {
                    DisplayMessageLine(MessageContainerList.GetMessage(machineUserContext, 821));
                    this.ActiveControl = txtFirstName;
                    txtFirstName.BackColor = Color.OrangeRed;
                    return;
                }
                if (txtLastName.Text.Length > 0)
                {
                    if (!System.Text.RegularExpressions.Regex.IsMatch(txtLastName.Text, @"^[a-zA-Z]+$"))
                    {
                        DisplayMessageLine(MessageContainerList.GetMessage(machineUserContext, 821));
                        this.ActiveControl = txtLastName;
                        txtLastName.BackColor = Color.OrangeRed;
                        return;
                    }
                }
            }
            if (customerDTO == null)
            {
                newCustomer = true;
                customerDTO = new CustomerDTO();
            }

            if (!string.IsNullOrEmpty(txtEmail.Text.Trim()))
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(txtEmail.Text.Trim(), @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
                {
                    DisplayMessageLine(MessageContainerList.GetMessage(machineUserContext, 572));
                    this.ActiveControl = txtEmail;
                    txtEmail.BackColor = Color.OrangeRed;
                    return;
                }
            }

            txtContactPhone1.Text = txtContactPhone1.Text.Trim();
            if (!string.IsNullOrEmpty(txtContactPhone1.Text))
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(txtContactPhone1.Text, @"^[0-9]+$"))
                {
                    DisplayMessageLine(MessageContainerList.GetMessage(machineUserContext, 785));
                    this.ActiveControl = txtContactPhone1;
                    txtContactPhone1.BackColor = Color.OrangeRed;
                    return;
                }

                int width = txtContactPhone1.MaxLength;

                if (width != 20)
                {
                    if (txtContactPhone1.Text.Length != width)
                    {
                        DisplayMessageLine(MessageContainerList.GetMessage(machineUserContext, 785));
                        this.ActiveControl = txtContactPhone1;
                        txtContactPhone1.BackColor = Color.OrangeRed;
                        return;
                    }

                    for (int i = 0; i < 10; i++)
                    {
                        string match = new string(i.ToString()[0], width);
                        if (match.Equals(txtContactPhone1.Text))
                        {
                            DisplayMessageLine(MessageContainerList.GetMessage(machineUserContext, 785));
                            this.ActiveControl = txtContactPhone1;
                            txtContactPhone1.BackColor = Color.OrangeRed;
                            return;
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(txtCardNumber.Text.Trim()))
            {
                customerDTO.CardNumber = txtCardNumber.Text;
            }
            else
                customerDTO.CardNumber = null;

            txtBirthDate.BackColor = txtCardNumber.BackColor;
            if (!string.IsNullOrEmpty(txtBirthDate.Text.Replace(" ", "").Replace("-", "").Replace("/", "")))
            {
                string dateMonthformat = GetDateMonthFormat();
                System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;
                try
                {
					DateTime datofBirthValue;
                    if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(utilities.ExecutionContext, "IGNORE_CUSTOMER_BIRTH_YEAR"))
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
					
                    if (DateTime.Compare(customerDTO.DateOfBirth.Value, utilities.getServerTime()) > 0)
                    {
                        DisplayMessageLine(MessageContainerList.GetMessage(machineUserContext, 449, dateMonthformat.ToString(), Convert.ToDateTime("23-Feb-1982").ToString(dateMonthformat)));
                        txtBirthDate.Focus();
                        txtBirthDate.BackColor = Color.OrangeRed;
                        return;
                    }
                }
                catch
                {
                    try
                    {
                        System.Globalization.CultureInfo currentcultureprovider = System.Globalization.CultureInfo.CurrentCulture;
                        customerDTO.DateOfBirth = Convert.ToDateTime(txtBirthDate.Text, currentcultureprovider);
                        if (DateTime.Compare(customerDTO.DateOfBirth.Value, utilities.getServerTime()) > 0)
                        {
                            DisplayMessageLine(MessageContainerList.GetMessage(machineUserContext, 449, ParafaitDefaultContainerList.GetParafaitDefault(machineUserContext, "DATE_FORMAT"), Convert.ToDateTime("23-Feb-1982").ToString(ParafaitDefaultContainerList.GetParafaitDefault(machineUserContext, "DATE_FORMAT"))));
                            txtBirthDate.Focus();
                            txtBirthDate.BackColor = Color.OrangeRed;
                            return;
                        }
                    }
                    catch
                    {
                        DisplayMessageLine(MessageContainerList.GetMessage(machineUserContext, 449, ParafaitDefaultContainerList.GetParafaitDefault(machineUserContext, "DATE_FORMAT"), Convert.ToDateTime("23-Feb-1982").ToString(ParafaitDefaultContainerList.GetParafaitDefault(machineUserContext, "DATE_FORMAT"))));
                        txtBirthDate.Focus();
                        txtBirthDate.BackColor = Color.OrangeRed;
                        return;
                    }
                }
                txtBirthDate.Text = customerDTO.DateOfBirth.Value.ToString(ParafaitDefaultContainerList.GetParafaitDefault(machineUserContext, "DATE_FORMAT"));
            }
            else
                customerDTO.DateOfBirth = null;

            txtEmail.BackColor = txtCardNumber.BackColor;
            if (!string.IsNullOrEmpty(txtEmail.Text))
            {
                if (!ValidateEmail(txtEmail.Text.Trim()))
                {
                    DisplayMessageLine(MessageContainerList.GetMessage(machineUserContext, 450));
                    txtEmail.Focus();
                    txtEmail.BackColor = Color.OrangeRed;
                    return;
                }
            }
            if (!string.IsNullOrEmpty(txtUniqueId.Text.ToString()))
            {
                customerDTO.UniqueIdentifier = txtUniqueId.Text.ToString();
            }
            Label label = new Label();
            label.Text = "Field";
            foreach (Control cp in flpCustomer.Controls)
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
                    if (c.Name == "comboBoxGender")
                    {
                        if ((c as ComboBox).SelectedIndex > 1)
                        {
                            DisplayMessageLine(MessageContainerList.GetMessage(machineUserContext, 250));
                            this.ActiveControl = c;
                            return;
                        }
                    }
                    else if (c.Name == "cmbState")
                    {
                        if ((c as ComboBox).SelectedIndex <= 0)
                        {
                            DisplayMessageLine(MessageContainerList.GetMessage(machineUserContext, 249, cmbState.Text.TrimEnd(':', '*')));
                            this.ActiveControl = c;
                            c.BackColor = Color.OrangeRed;
                            return;
                        }
                    }
                    else if (c.Name == "cmbTitle")
                    {
                        if ((c as ComboBox).SelectedIndex <= 0)
                        {
                            DisplayMessageLine(MessageContainerList.GetMessage(machineUserContext, 249, lblTitle.Text.TrimEnd(':', '*')));
                            this.ActiveControl = c;
                            c.BackColor = Color.OrangeRed;
                            return;
                        }
                    }
                    else if (c.Name == "btnOpinPromotions")
                    {
                        if (pnlOptinPromotions.Tag.Equals(false))
                        {
                            DisplayMessageLine(MessageContainerList.GetMessage(machineUserContext,249, lblOptinPromotions.Text.TrimEnd(':', '*')));
                            this.ActiveControl = c;
                            c.BackColor = Color.OrangeRed;
                            return;
                        }
                    }
                    else if (c.Name == "btnTermsandConditions")
                    {
                        if (ParafaitDefaultContainerList.GetParafaitDefault(machineUserContext, "TERMS_AND_CONDITIONS").Equals("M") && pnlTermsandConditions.Tag.Equals(false))
                        {
                            DisplayMessageLine(MessageContainerList.GetMessage(machineUserContext, 249, lblTermsAndConditions.Text.TrimEnd(':', '*')));
                            this.ActiveControl = c;
                            c.BackColor = Color.OrangeRed;
                            return;
                        }
                    }
                    else if (string.IsNullOrEmpty(c.Text.Trim()))
                    {
                        DisplayMessageLine(MessageContainerList.GetMessage(machineUserContext, 249, label.Text.TrimEnd(':', '*')));
                        this.ActiveControl = c;
                        c.BackColor = Color.OrangeRed;
                        return;
                    }
                    

                }
            }            
            customerDTO.OptInPromotions = Convert.ToBoolean(pnlOptinPromotions.Tag);
            customerDTO.PolicyTermsAccepted = Convert.ToBoolean(pnlTermsandConditions.Tag);
            customerDTO.OptInPromotionsMode = cmbPromotionMode.SelectedValue.ToString();
            foreach (Control c in flpCustomAttributes.Controls)
            {
                if (c.Tag == null)
                    continue;
                if (customAttributes.FieldDisplayOption(c.Tag) == "M")
                {
                    c.BackColor = txtCardNumber.BackColor;
                    string type = c.GetType().ToString();
                    if (type.Contains("TextBox") && string.IsNullOrEmpty(c.Text.Trim()))
                    {
                        DisplayMessageLine(MessageContainerList.GetMessage(machineUserContext, 249, flpCustomAttributes.GetNextControl(c, false).Text.TrimEnd(':', '*')));
                        this.ActiveControl = c;
                        c.BackColor = Color.OrangeRed;
                        return;
                    }
                    else if (type.Contains("ComboBox") && (c as ComboBox).SelectedValue.Equals(DBNull.Value))
                    {
                        DisplayMessageLine(MessageContainerList.GetMessage(machineUserContext, 249, flpCustomAttributes.GetNextControl(c, false).Text.TrimEnd(':', '*')));
                        this.ActiveControl = c;
                        c.BackColor = Color.OrangeRed;
                        return;
                    }
                }
            }

            
            if (pbCapture.Name == "M" && (pbCapture.Tag == null || pbCapture.Tag.ToString() == ""))
            {
                DisplayMessageLine(MessageContainerList.GetMessage(machineUserContext, 17));
                return;
            }

            AddressDTO addressDTO = null;
            if (!string.IsNullOrEmpty(txtAddress1.Text) || !string.IsNullOrEmpty(txtAddress2.Text)
                || !string.IsNullOrEmpty(cmbCountry.Text) || !string.IsNullOrEmpty(cmbState.Text)
                || !string.IsNullOrEmpty(txtPinCode.Text) || !string.IsNullOrEmpty(txtCity.Text))
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
            if (!string.IsNullOrEmpty(txtEmail.Text))
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
            if (!string.IsNullOrEmpty(txtContactPhone1.Text))
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

            if (!string.IsNullOrEmpty(txtAddress1.Text))
                addressDTO.Line1 = txtAddress1.Text;
            if (!string.IsNullOrEmpty(txtAddress2.Text))
                addressDTO.Line2 = txtAddress2.Text;
            customerDTO.FirstName = txtFirstName.Text;
            customerDTO.Title = (cmbTitle.SelectedIndex > -1 ? cmbTitle.SelectedValue.ToString() : "");
            customerDTO.LastName = txtLastName.Text;
            if (!string.IsNullOrEmpty(txtCity.Text))
                addressDTO.City = txtCity.Text;
            if ((cmbState.SelectedIndex) > -1&&(cmbState.SelectedValue!=null&& Convert.ToInt32(cmbState.SelectedValue)>-1))
                addressDTO.StateId = Convert.ToInt32(cmbState.SelectedValue);
            if ((cmbCountry.SelectedIndex) > -1)
                addressDTO.CountryId = Convert.ToInt32(cmbCountry.SelectedValue);
            if (!string.IsNullOrEmpty(txtPinCode.Text))
                addressDTO.PostalCode = txtPinCode.Text;
            if (!string.IsNullOrEmpty(txtContactPhone1.Text))
                contactPhone1DTO.Attribute1 = txtContactPhone1.Text;
            if (!string.IsNullOrEmpty(txtEmail.Text))
                contactEmailDTO.Attribute1 = txtEmail.Text.Trim();
            if (!string.IsNullOrEmpty(txtAnniversary.Text))
            {
                ValidateAnnivarsaryDate();
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
                ShowHideKeypad('H');
                bool firstTimeRegistration = false;
                if (customerDTO.Id == -1 && customerDTO != null)
                {
                    int CustomDataSetId = customerDTO.CustomDataSetId;
                    if (customAttributes.Save((int)customerDTO.Id, ref CustomDataSetId))
                    {
                        customerDTO.CustomDataSetId = CustomDataSetId;
                        CustomDataSetDTO customDataSetDTO = (new CustomDataSetBL(machineUserContext, CustomDataSetId)).CustomDataSetDTO;
                        customerDTO.CustomDataSetDTO = customDataSetDTO;
                    }
                    SqlConnection sqlConnection = utilities.createConnection();
                    SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
                    try
                    {
                        CustomerBL customerBL = new CustomerBL(machineUserContext, customerDTO);
                        customerBL.Save(sqlTransaction);
                        if (customerDTO.CardNumber != null)
                        {
                            AccountBL accountBL = new AccountBL(machineUserContext, customerDTO.CardNumber, true, true);
                            accountBL.AccountDTO.CustomerId = customerDTO.Id;
                            accountBL.Save(sqlTransaction);
                        }
                        else
                        {
                        //    AccountDTO accountDTO = new AccountDTO();
                        //    accountDTO.CustomerId = customerDTO.Id;
                        //    AccountBL accountBL = new AccountBL(machineUserContext, accountDTO);
                        //    accountBL.Save(sqlTransaction);
                        }
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
                    
                    firstTimeRegistration = true;
                    if (ParafaitDefaultContainerList.GetParafaitDefault(machineUserContext, "ENABLE_KIOSK_CUSTOMER_VERIFICATION").Equals("Y"))//Utilities.getParafaitDefaults("ENABLE_KIOSK_CUSTOMER_VERIFICATION") == "Y")//Changes to add the customer verification
                    {
                        SendVerificationCode();
                    } 
                }
                else 
                {
                    int CustomDataSetId = customerDTO.CustomDataSetId;
                    if (customAttributes.Save((int)customerDTO.Id, ref CustomDataSetId))
                    {
                        customerDTO.CustomDataSetId = CustomDataSetId;
                        CustomDataSetDTO customDataSetDTO = (new CustomDataSetBL(machineUserContext, CustomDataSetId)).CustomDataSetDTO;
                        customerDTO.CustomDataSetDTO = customDataSetDTO;
                    }
                    SqlConnection sqlConnection = utilities.createConnection();
                    SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
                    try
                    {
                        CustomerBL customerBL = new CustomerBL(machineUserContext, customerDTO);
                        customerBL.Save(sqlTransaction);
                        if (customerDTO.CardNumber != null)
                        {
                            AccountBL accountBL = new AccountBL(machineUserContext, customerDTO.CardNumber, true, true);
                            accountBL.AccountDTO.CustomerId = customerDTO.Id;
                            accountBL.Save(sqlTransaction);
                        }
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
                
                DisplayMessageLine(MessageContainerList.GetMessage(machineUserContext, 452));
                log.LogMethodExit();
            }
            catch(ValidationException vx)
            {
                if(vx.ValidationErrorList!=null && vx.ValidationErrorList.Count>0)
                {
                    if (newCustomer)
                    { customerDTO = new CustomerDTO(); }
                    DisplayMessageLine(MessageContainerList.GetMessage(machineUserContext, 255, vx.ValidationErrorList[0].Message));

                    foreach (Control c in flpCustomer.Controls)
                    {
                        foreach (ValidationError vr in vx.ValidationErrorList)
                        {
                            if (c.Tag!=null && c.Tag.Equals(vr.FieldName))
                            {
                                c.BackColor = Color.OrangeRed;
                                continue;
                            }
                        }
                    }
                    throw new Exception(vx.ValidationErrorList[0].Message);
                }
            }
            catch (Exception ex)
            {
                if(newCustomer)
                { customerDTO = new CustomerDTO(); }
                DisplayMessageLine(MessageContainerList.GetMessage(machineUserContext, 255, ex.Message));
                throw new Exception(ex.Message);
            }
        }

        private string GetDateMonthFormat()
        {
            log.LogMethodEntry();
            string dateFormat = ParafaitDefaultContainerList.GetParafaitDefault(machineUserContext, "DATE_FORMAT");
            if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(machineUserContext, "IGNORE_CUSTOMER_BIRTH_YEAR"))
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
        private void SendVerificationCode()
        {
            log.LogMethodEntry();
            try
            {
                this.Cursor = Cursors.WaitCursor;
                string Code;
                if (!string.IsNullOrEmpty(customerDTO.Email) || !string.IsNullOrEmpty(customerDTO.PhoneNumber))
                {
                    CustomerVerificationBL customerVerificationBL = new CustomerVerificationBL(utilities.ExecutionContext);
                    customerVerificationBL.GenerateVerificationRecord(customerDTO.Id, utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.POSMachine, true);
                    Code = customerVerificationBL.CustomerVerificationDTO.VerificationCode;
                }                   
                else
                {
                    return;
                }

                //if (!string.IsNullOrEmpty(customerDTO.Email))
                //{
                //    Messaging msg = new Messaging(utilities);
                //    string body = "Dear " + customerDTO.FirstName + ",";
                //    body += Environment.NewLine + Environment.NewLine;
                //    body += "Your registration verification code is " + Code + ".";
                //    body += Environment.NewLine + Environment.NewLine;
                //    body += "Thank you";
                //    body += Environment.NewLine;
                //    body += utilities.ParafaitEnv.SiteName;

                //    msg.SendEMailSynchronous(customerDTO.Email, "", utilities.ParafaitEnv.SiteName + " - customer registration verification", body);
                //}

                //if (!string.IsNullOrEmpty(customerDTO.PhoneNumber))
                //{
                //    Messaging msg = new Messaging(utilities);
                //    string body = "Dear " + customerDTO.FirstName + ", ";
                //    body += "Your registration verification code is " + Code + ". ";
                //    body += "Thank you. ";
                //    body += utilities.ParafaitEnv.SiteName;

                //    msg.sendSMSSynchronous(customerDTO.PhoneNumber, body);
                //}
                log.LogMethodExit();
            }
            catch
            {

            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

        }
        private string generateCode()
        {
            log.LogMethodEntry();
            string code = utilities.GenerateRandomNumber(6, Utilities.RandomNumberType.Numeric);
            CustomerVerificationDTO customerVerificationDTO = new CustomerVerificationDTO();
            customerVerificationDTO.Source = "POS:" + utilities.ParafaitEnv.POSMachine;
            customerVerificationDTO.VerificationCode = code;
            customerVerificationDTO.ProfileDTO = customerDTO.ProfileDTO;
            CustomerVerificationBL customerVerificationBL = new CustomerVerificationBL(machineUserContext, customerVerificationDTO);
            customerVerificationBL.Save();
            log.LogMethodExit(code);
            return code;
        }
        protected virtual void btnBack_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            log.LogMethodExit();
        }
        private void btnShowKeyPad_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ShowHideKeypad('T');
            log.LogMethodExit();
        }
        private void textBoxContactPhone1_KeyPress(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (!char.IsNumber(e.KeyChar) && !char.IsControl(e.KeyChar) && !(e.KeyChar == '+'))
                e.Handled = true;
            log.LogMethodExit();
        }
        protected virtual void frmKioskCustomer_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ResetKioskTimer();
            Arrange();
            ShowHideKeypad();
            DisplayCustomerDetails();
            MaskDisplay(false);
            log.LogMethodExit();
        }
        private void DisplayCustomerDetails()
        {
            log.LogMethodEntry();
            if (customerDTO == null || customerDTO.Id < 0)
            {
                txtFirstName.Clear();
                txtLastName.Clear();
                txtAddress1.Clear();
                txtAddress2.Clear();
                txtCity.Clear();
                txtPinCode.Clear();
                if (countryId != null && countryId != "-1")
                {
                    cmbCountry.SelectedValue = Convert.ToInt32(countryId);
                    cmbCountry.Enabled = false;
                }
                txtContactPhone1.Clear();
                txtEmail.Clear(); ;
                if (birthDate != DateTime.MinValue)
                    txtBirthDate.Text = birthDate.ToString(utilities.getDateFormat());
                else
                    txtBirthDate.Clear();
                comboBoxGender.Text = "";
                if (cmbTitle.Visible)
                    cmbTitle.SelectedIndex = 0;
                pbCapture.Tag = "";
                pbCapture.Image = Properties.Resources.profile_picture_placeholder;
            }
            else
            {

                txtFirstName.Text = customerDTO.FirstName;
                if (cmbTitle.Visible)
                {
                    try
                    {
                        if (string.IsNullOrEmpty(customerDTO.Title))
                            cmbTitle.SelectedIndex = 0;
                        else
                            cmbTitle.SelectedValue = customerDTO.Title;
                    }
                    catch { cmbTitle.SelectedIndex = 0; }
                }
                txtLastName.Text = customerDTO.LastName;
                if (customerDTO.AddressDTOList != null)
                {
                    txtAddress1.Text = customerDTO.AddressDTOList[0].Line1;
                    txtAddress2.Text = customerDTO.AddressDTOList[0].Line2;
                    txtCity.Text = customerDTO.AddressDTOList[0].City;
                    cmbState.SelectedValue = customerDTO.AddressDTOList[0].StateId;
                    txtPinCode.Text = customerDTO.AddressDTOList[0].PostalCode;
                }

                try
                {
                    if (countryId != null && countryId != "-1")
                    {
                        cmbCountry.SelectedValue = Convert.ToInt32(countryId);
                    }
                    else
                    {
                        cmbCountry.SelectedValue = customerDTO.AddressDTOList[0].CountryId;
                    }
                }
                catch
                {

                }
                txtContactPhone1.Text = customerDTO.PhoneNumber;
                txtEmail.Text = customerDTO.Email;
                txtUniqueId.Text = customerDTO.UniqueIdentifier;
                if (customerDTO.Anniversary != null && customerDTO.Anniversary != DateTime.MinValue)
                {
                    txtAnniversary.Text = customerDTO.Anniversary.Value.ToString(ParafaitDefaultContainerList.GetParafaitDefault(machineUserContext, "DATE_FORMAT"));
                    lblDateTimeFormat.Visible = false;
                }
                else
                {
                    txtAnniversary.Text = "";
                    lblDateTimeFormat.Visible = true;
                }
                if (customerDTO.DateOfBirth != null && customerDTO.DateOfBirth != DateTime.MinValue)
                    txtBirthDate.Text = customerDTO.DateOfBirth.Value.ToString(ParafaitDefaultContainerList.GetParafaitDefault(machineUserContext, "DATE_FORMAT"));
                else
                    txtBirthDate.Text = "";

                switch (customerDTO.Gender)
                {
                    case "M": comboBoxGender.Text = "Male"; break;
                    case "F": comboBoxGender.Text = "Female"; break;
                    case "N": comboBoxGender.Text = "Not Set"; break;
                    default: comboBoxGender.Text = "Not Set"; break;
                }

                pbCapture.Tag = customerDTO.PhotoURL;

                if (customerDTO.PhotoURL.Trim() != "")
                {
                    try
                    {
                        CustomerBL customerBL = new CustomerBL(machineUserContext, customerDTO);
                        pbCapture.Image = customerBL.GetCustomerImage();
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
                customAttributes.PopulateData(customerDTO.Id, customerDTO.CustomDataSetId);
            }

            this.ActiveControl = txtFirstName;
            log.LogMethodExit();
        }
        /// <summary>
        /// Method to mask display
        /// </summary>
        /// <param name="inAllowEdit">bool</param>
        protected void MaskDisplay(bool inAllowEdit)
        {
            log.LogMethodEntry(inAllowEdit);
            if (customerDTO != null)
            {
                inAllowEdit &= ParafaitDefaultContainerList.GetParafaitDefault(machineUserContext, "ALLOW_CUSTOMER_INFO_EDIT").Equals("Y");
                if (inAllowEdit)
                {
                    txtFirstName.ReadOnly = false;

                    foreach (Control c in flpCustomer.Controls)
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
                    txtEmail.PasswordChar =
                    txtAnniversary.PasswordChar =
                    txtUniqueId.PasswordChar =
                    txtContactPhone1.PasswordChar = '\0';

                    if (countryId != null && countryId != "-1")
                        cmbCountry.Enabled = false;
                    btnSave.Enabled = true;
                    ShowHideKeypad('S');
                }
                else
                {
                    txtFirstName.ReadOnly = true;

                    foreach (Control c in flpCustomer.Controls)
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
                    txtEmail.PasswordChar =
                    txtAnniversary.PasswordChar =
                    txtUniqueId.PasswordChar =
                    txtContactPhone1.PasswordChar = 'X';

                    btnSave.Enabled = false;
                    ShowHideKeypad('H');
                }

                if (ParafaitDefaultContainerList.GetParafaitDefault(machineUserContext, "ALLOW_CUSTOMER_INFO_EDIT").Equals("N"))
                {
                    DisplayMessageLine(MessageContainerList.GetMessage(machineUserContext, 447));
                }
                else
                {
                    if (inAllowEdit)
                    {
                        DisplayMessageLine(MessageContainerList.GetMessage(machineUserContext, 501));
                    }
                    else
                    {
                        if (utilities.ParafaitEnv.MIFARE_CARD)
                            DisplayMessageLine(MessageContainerList.GetMessage(machineUserContext, 929));
                        else
                            DisplayMessageLine(MessageContainerList.GetMessage(machineUserContext, 928));
                    }
                }
            }
            else
            {
                ShowHideKeypad('S');
            }
            txtFirstName.Focus();
            log.LogMethodExit();
        }

        private void frmKioskCustomer_KeyPress(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ResetKioskTimer();
            log.LogMethodExit();
        }
        private void pnlOptinPromotions_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender,e);
            ResetKioskTimer();
            if (pnlOptinPromotions.Tag.Equals(true))
            {
                pnlOptinPromotions.Tag = false;
                btnOpinPromotions.Image = Properties.Resources.T_C_tick_box;
            }
            else
            {
                pnlOptinPromotions.Tag = true;
                btnOpinPromotions.Image = Properties.Resources.tick_box_checked;
            }
            log.LogMethodExit();
        }

        private void pnlTermsandConditions_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ResetKioskTimer();
            if (pnlTermsandConditions.Tag.Equals(false))
            {
                ShowHideKeypad('H');
                using (CustomerTermsandConditionsUI frmRegisterTnC = new CustomerTermsandConditionsUI(utilities,"KIOSK"))
                {
                    if (frmRegisterTnC.ShowDialog() == DialogResult.OK)
                    {
                        pnlTermsandConditions.Tag = true;
                        btnTermsandConditions.Image = Properties.Resources.tick_box_checked;
                    }
                }
                ShowHideKeypad('S');
            }
            log.LogMethodExit();
        }
        private List<LookupValuesDTO> LoadPromotionModeDropDown()
        {
            log.LogMethodEntry();
            List<LookupValuesDTO> lookupValuesDTOList = null;
            try
            {
                LookupValuesList lookupValuesList = new LookupValuesList(utilities.ExecutionContext);
                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchMemberParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                searchMemberParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "PROMOTION_MODES"));
                searchMemberParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));
                lookupValuesDTOList = lookupValuesList.GetAllLookupValues(searchMemberParams);
                if (lookupValuesDTOList == null)
                {
                    lookupValuesDTOList = new List<LookupValuesDTO>();
                }
                lookupValuesDTOList.Insert(0, new LookupValuesDTO());
                lookupValuesDTOList[0].LookupValue = string.Empty;
                lookupValuesDTOList[0].Description = "None";
                cmbPromotionMode.DataSource = lookupValuesDTOList;
                cmbPromotionMode.ValueMember = "LookupValue";
                cmbPromotionMode.DisplayMember = "Description";

            }
            catch (Exception ex)
            {
                log.Error("Error occured while loading lookupValues list", ex);
            }
            log.LogMethodExit(lookupValuesDTOList);
            return lookupValuesDTOList;
        }

        private void txtBirthDate_Leave(object sender, EventArgs e)
        {
            ResetKioskTimer();
            if (string.IsNullOrEmpty(txtBirthDate.Text))
            {
                lblDateFormat.Visible = true;
            }
            else
            {
                lblDateFormat.Visible = false;
            }
        }

        private void txtAnnivarsaryDate_Leave(object sender, EventArgs e)
        {
            ResetKioskTimer();
            if (string.IsNullOrEmpty(txtAnniversary.Text))
            {
                lblDateTimeFormat.Visible = true;
            }
            else
            {
                lblDateTimeFormat.Visible = false;
            }
        }

        private void txtUniqueId_Validating(object sender, CancelEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            //frmOKMsg frmOK;
            if (customerDTO == null)
                return;

            if ((sender as TextBox).Text.Trim() == "" && utilities.ParafaitEnv.UNIQUE_ID_MANDATORY_FOR_VIP == "Y")
            {
                e.Cancel = true;
                DisplayMessageLine(utilities.MessageUtils.getMessage(289));
                return;
            }
            DisplayMessageLine("");

            if ((sender as TextBox).Text.Trim() == "")
                return;

            List<CustomerDTO> customerDTOList = null;
            CustomerListBL customerListBL = new CustomerListBL(utilities.ExecutionContext);
            CustomerSearchCriteria customerSearchCriteria = new CustomerSearchCriteria(CustomerSearchByParameters.PROFILE_UNIQUE_IDENTIFIER, Operator.EQUAL_TO, (sender as TextBox).Text.Trim());
            if (customerDTO != null && customerDTO.Id >= 0)
            {
                customerSearchCriteria.And(CustomerSearchByParameters.CUSTOMER_ID, Operator.NOT_EQUAL_TO, customerDTO.Id);
            }
            customerSearchCriteria.OrderBy(CustomerSearchByParameters.CUSTOMER_ID)
                                  .Paginate(0, 20);
            customerDTOList = customerListBL.GetCustomerDTOList(customerSearchCriteria, false, false);
            if (customerDTOList != null && customerDTOList.Count > 0)
            {
                
                DisplayMessageLine(utilities.MessageUtils.getMessage(290));
               
            }
            log.LogMethodExit();
        }

        private void lblDateFormat_Click(object sender, EventArgs e)
        {
            ResetKioskTimer();
            lblDateFormat.Visible = false;
            txtBirthDate.Focus();
        }

        private void lblDateTimeFormat_Click(object sender, EventArgs e)
        {
            ResetKioskTimer();
            lblDateTimeFormat.Visible = false;
            txtBirthDate.Focus();
        }

        void ValidateAnnivarsaryDate()
        {
            log.LogMethodEntry();
            string dateMonthformat = GetDateMonthFormat();
            if (!string.IsNullOrEmpty(txtAnniversary.Text.Replace(" ", "").Replace("-", "").Replace("/", "")))
            {
                try
                {
                    System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;
                    DateTime AnniversaryValue;
                    if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(utilities.ExecutionContext, "IGNORE_CUSTOMER_BIRTH_YEAR"))
                    {
                        AnniversaryValue = DateTime.ParseExact(txtAnniversary.Text + "/1904", dateMonthformat + "/yyyy", provider);
                    }
                    else
                    {
                        AnniversaryValue = DateTime.ParseExact(txtAnniversary.Text, dateMonthformat, provider);
                    }
                    if (AnniversaryValue != customerDTO.Anniversary)
                    {
                        customerDTO.Anniversary = AnniversaryValue;
                    }
                }
                catch
                {
                    try
                    {
                        System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.CurrentCulture;
                        DateTime AnniversaryValue = Convert.ToDateTime(txtAnniversary.Text, provider);
                        if (AnniversaryValue != customerDTO.Anniversary)
                        {
                            customerDTO.Anniversary = AnniversaryValue;
                        }
                    }
                    catch
                    {
                        DisplayMessageLine(MessageContainerList.GetMessage(utilities.ExecutionContext, 449, dateMonthformat, dateMonthformat, Convert.ToDateTime("23-Feb-1982").ToString(dateMonthformat)));//validationErrorList.Add(new ValidationError("Customer", "Anniversary", ));
                    }
                }
            }
            else
            {
                if (customerDTO.Anniversary != null)
                {
                    customerDTO.Anniversary = null;
                }
            }
        }
    }
}
