/********************************************************************************************
* Project Name - Parafait_Kiosk -frmAddCustomerRelation.cs
* Description  - frmAddCustomerRelation.cs 
* 
**************
**Version Log
**************
*Version       Date            Modified By        Remarks          
*********************************************************************************************
 *2.140.0      18-Oct-2021     Sathyavathi        Created
 *2.150.0      28-Jan-2022     Deeksha            Modified to handle Customer UI impacts
 *2.150.0.0    21-Jun-2022     Vignesh Bhat       Back and Cancel button changes 
 *2.130.8      26-Jun-2022     Guru S A           OSK usage in Kiosk
 *2.150.0.0    23-Sep-2022     Sathyavathi        Check-In feature Phase-2
 *2.150.0.0    13-Oct-2022     Sathyavathi        Mask card number
 *2.150.0.0    02-Dec-2022     Sathyavathi        Check-In feature Phase-2 Additional features
 *2.150.1      22-Feb-2023     Sathyavathi        Kiosk Cart Enhancements
 *2.155.0      22-Jun-2023     Sathyavathi        Attraction Sale in Kiosk - Calendar changes
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.Customer;
using Semnox.Core.GenericUtilities;
using System.ComponentModel;
using System.Linq;
using Semnox.Parafait.Languages;
using System.Globalization;

namespace Parafait_Kiosk
{
    public partial class frmAddCustomerRelation : BaseFormKiosk
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Utilities utilities = KioskStatic.Utilities;
        private SortableBindingList<CustomCustomerDTO> customCustomerDTOList = new SortableBindingList<CustomCustomerDTO>();
        private int parentCustomerId;
        private Font savTimeOutFont;
        private Font TimeOutFont;

        private const string WARNING = "WARNING";
        private const string ERROR = "ERROR";
        private const string MESSAGE = "MESSAGE";
        private string countryId = null;
        private VirtualWindowsKeyboardController virtualKeyboardController;
        private VirtualKeyboardController customKeyboardController;
        private int relatedCustomerId;
        private string relatedCustomerName;
        private string relatedDateOfBirth;

        internal int RelatedCustomerId { get { return relatedCustomerId; }}
        internal CustomCustomerDTO LinkedCustomerDTO { get { return (CustomCustomerDTO)customCustomerDTOList[0]; } }

        public frmAddCustomerRelation(int parentCustId, string parentCustName, string cardNumber = null, string relativeName = "", string relativeDOB = "")
        {
            log.LogMethodEntry();
            InitializeComponent();

            parentCustomerId = parentCustId;

            if (!string.IsNullOrEmpty(cardNumber))
            {
                txtCardNumber.Text = KioskHelper.GetMaskedCardNumber(cardNumber);
                txtCardNumber.Enabled = false;
            }
            else
            {
                lblCardNumber.Visible = false;
                txtCardNumber.Parent.Visible = false;
            }

            txtCustomerName.Text = parentCustName;
            txtCustomerName.Enabled = false;

            if (!string.IsNullOrEmpty(relativeName))
                this.relatedCustomerName = relativeName;

            if (!string.IsNullOrEmpty(relativeDOB))
                this.relatedDateOfBirth = relativeDOB;

            lblTimeRemaining.Text = "";

            KioskStatic.formatMessageLine(textBoxMessageLine, 21, ThemeManager.CurrentThemeImages.BottomMessageLineImage);
            KioskStatic.setDefaultFont(this);

            savTimeOutFont = lblTimeRemaining.Font;
            TimeOutFont = lblTimeRemaining.Font = new System.Drawing.Font(lblTimeRemaining.Font.FontFamily, 50, FontStyle.Bold);
            SetKioskTimerTickValue(30);
            ResetKioskTimer();
            lblTimeRemaining.Text = GetKioskTimerTickValue().ToString("#0");

            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);

            textBoxMessageLine.Text = "";

            try
            {
                this.BackgroundImage = ThemeManager.GetBackgroundImage(ThemeManager.CurrentThemeImages.AddCustomerRelationBackgroundImage);
                btnHome.BackgroundImage = ThemeManager.CurrentThemeImages.HomeButton;
                foreach (Control c in dgvAddCustomerRelation.Controls)
                {
                    c.ForeColor = KioskStatic.CurrentTheme.AddCustomerRelationGridTextForeColor;
                }
                panelAddRelations.BackgroundImage = ThemeManager.CurrentThemeImages.KioskActivityTableImage;
                btnPrev.BackgroundImage = btnSave.BackgroundImage = btnComplete.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage;
                panelAddRelations.BackgroundImage = ThemeManager.CurrentThemeImages.KioskActivityTableImage;
                textBoxMessageLine.BackgroundImage = ThemeManager.CurrentThemeImages.BottomMessageLineImage;
                lblTimeRemaining.BackgroundImage = ThemeManager.CurrentThemeImages.TimerBoxSmall;
                lblGreeting.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, lblGreeting.Text);
                lblGridFooterMsg.Text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 4350); //(*) Marks field are mandatory to fill up

                bigVerticalScrollCustomerRelation.DownButtonBackgroundImage = bigVerticalScrollCustomerRelation.DownButtonDisabledBackgroundImage = ThemeManager.CurrentThemeImages.ScrollDownButton;
                bigVerticalScrollCustomerRelation.UpButtonBackgroundImage = bigVerticalScrollCustomerRelation.UpButtonDisabledBackgroundImage = ThemeManager.CurrentThemeImages.ScrollUpButtonImage;
                SetCustomizedFontColors();
                DisplaybtnPrev(true);
                SetFont();
                utilities.setLanguage(this);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            log.LogMethodExit();
        }

        private void frmAddCustomerRelation_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ResetKioskTimer();
            lblSiteName.Text = KioskStatic.SiteHeading;
            try
            {
                InitializeDGVAddCustomerRelationsDataGrid();
                customCustomerDTOBindingSource.AddNew();
                dgvAddCustomerRelation.DataSource = customCustomerDTOBindingSource; // control's DataSource.
                customCustomerDTOBindingSource.DataSource = customCustomerDTOList;
                dOBDGVTextBoxColumn.DefaultCellStyle.Format = KioskStatic.DateMonthFormat;
                dOBDGVTextBoxColumn.DefaultCellStyle.NullValue = KioskStatic.DateMonthFormat;
                anniversaryDataGridViewComboBoxColumn.DefaultCellStyle.Format = KioskStatic.DateMonthFormat;
                anniversaryDataGridViewComboBoxColumn.DefaultCellStyle.NullValue = KioskStatic.DateMonthFormat;
                InitializeKeyboard();
                this.dgvAddCustomerRelation.ClearSelection();
                this.ActiveControl = txtCustomerName;
                HideKeyBoard();
                if (countryId != null && countryId != "-1")
                    countryDataGridViewComboBoxColumn.Visible = false;


                if (!string.IsNullOrEmpty(relatedCustomerName))
                {//the screen is invoked from child/adult enter checkin details link relation button
                    customCustomerDTOBindingSource.AllowNew = false;
                    InsertNewCustomCustomerDTO();
                    btnSave.Enabled = true;
                    btnComplete.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                StopKioskTimer();
                frmOKMsg.ShowUserMessage(ex.Message);
                ResetKioskTimer();
                StartKioskTimer();
                KioskStatic.logToFile("Error while loading Add Relation screen :" + ex.Message);
            }
            log.LogMethodExit();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                HideKeyBoard();
                KioskStatic.logToFile("Add Relation btnSave Click");
                dgvAddCustomerRelation.EndEdit();
                SaveData();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in btnSave_Click() in Add Customer Relation screen: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void SaveData()
        {
            log.LogMethodEntry();
            int i = 0;
            try
            {
                DisableButtons();
                StopKioskTimer();
                if (customCustomerDTOList == null || customCustomerDTOList.Count == 0)
                {
                    string message = MessageContainerList.GetMessage(utilities.ExecutionContext, 2790); //Please enter required fields
                    ValidationException ve = new ValidationException(message);
                    throw ve;
                }
                bool hasPartialData = HasPartialData();
                if (hasPartialData == true)
                {
                    string msg = MessageContainerList.GetMessage(utilities.ExecutionContext, 1008); //Processing..Please wait...
                    textBoxMessageLine.Text = msg;
                    Application.DoEvents();
                    foreach (CustomCustomerDTO customCustomerDTO in customCustomerDTOList)
                    {
                        if (customCustomerDTO.CustomerRelationshipTypeId == -1)
                        {
                            string errMsg = MessageContainerList.GetMessage(utilities.ExecutionContext, 249, dgvAddCustomerRelation.Columns["customerRelationshipTypeIdDataGridViewComboBoxColumn"].HeaderText); //&1 is mandatory. Please enter a value.
                            ValidationException ve = new ValidationException(errMsg);
                            throw ve;
                        }

                        if (!string.IsNullOrWhiteSpace(customCustomerDTO.EmailAddress))
                        {
                            if (!System.Text.RegularExpressions.Regex.IsMatch(customCustomerDTO.EmailAddress, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,})+)$"))
                            {
                                string errMsg = MessageContainerList.GetMessage(utilities.ExecutionContext, 450);
                                ValidationException ve = new ValidationException(errMsg);
                                throw ve;
                            }

                            if (!KioskStatic.check_mail(customCustomerDTO.EmailAddress))
                            {
                                string errMsg = MessageContainerList.GetMessage(utilities.ExecutionContext, 450);
                                ValidationException ve = new ValidationException(errMsg);
                                throw ve;
                            }
                        }

                        if (string.IsNullOrEmpty(customCustomerDTO.RelatedCustomerName))
                        {
                            string errMsg = MessageContainerList.GetMessage(utilities.ExecutionContext, 249, dgvAddCustomerRelation.Columns["relatedCustomerNameDataGridViewTextBoxColumn"].HeaderText); //&1 is mandatory. Please enter a value.
                            ValidationException ve = new ValidationException(errMsg);
                            throw ve;
                        }

                        textBoxMessageLine.Text = "";
                        int existingCustomerId = customCustomerDTO.CustomerRelationshipDTO.RelatedCustomerDTO.Id;
                        int parentCustId = customCustomerDTO.CustomerRelationshipDTO.CustomerId;
                        CustomerBL customerBL = new CustomerBL(utilities.ExecutionContext, customCustomerDTO.CustomerRelationshipDTO.RelatedCustomerDTO);

                        //If minor is being registered, make the customer type as Unregistered. Part of Check-In feature requirements
                        if (existingCustomerId <= -1)
                        {
                            if (customCustomerDTO.DOB != null && customCustomerDTO.DOB > DateTime.MinValue)
                            {
                                decimal age = KioskHelper.GetAge(customCustomerDTO.DOB.ToString());
                                decimal thresholdAgeOfChild = ParafaitDefaultContainerList.GetParafaitDefault<decimal>(KioskStatic.Utilities.ExecutionContext, "THRESHOLD_AGE_CHECK_IN_CHILD_SCREEN", -1);

                                if (age <= thresholdAgeOfChild)
                                {
                                    customerBL.CustomerDTO.CustomerType = CustomerType.UNREGISTERED;
                                }
                            }
                        }
                        customerBL.Save(null);
                        if (existingCustomerId == -1 || parentCustId == -1)
                        {
                            log.LogVariableState("Customer Id: ", customerBL.CustomerDTO.Id);
                            KioskStatic.logToFile("Customer with Id " + customerBL.CustomerDTO.Id + " is saved");
                            customCustomerDTO.CustomerRelationshipDTO.CustomerId = parentCustomerId;
                            customCustomerDTO.RelatedCustomerId = customerBL.CustomerDTO.Id;
                            CustomerRelationshipBL customerRelationshipBL = new CustomerRelationshipBL(utilities.ExecutionContext, customCustomerDTO.CustomerRelationshipDTO);
                            customerRelationshipBL.Save();
                            log.LogVariableState("Customer Relationship Id: ", customerRelationshipBL.CustomerRelationshipDTO.CustomerRelationshipTypeId);
                            KioskStatic.logToFile("Customer relationhip saved with CustomerRelationshipTypeId: " + customerRelationshipBL.CustomerRelationshipDTO.CustomerRelationshipTypeId);
                            relatedCustomerId = customCustomerDTO.RelatedCustomerId;
                        }
                        else
                        {
                            CustomerRelationshipBL customerRelationshipBL = new CustomerRelationshipBL(utilities.ExecutionContext, customCustomerDTO.CustomerRelationshipDTO);
                            customerRelationshipBL.Save();
                        }
                        dgvAddCustomerRelation.Rows[i].DefaultCellStyle.SelectionBackColor = Color.Green;
                        dgvAddCustomerRelation.Rows[i].DefaultCellStyle.SelectionForeColor = Color.White;
                        dgvAddCustomerRelation.Rows[i].DefaultCellStyle.BackColor = Color.Green;
                        dgvAddCustomerRelation.Rows[i].DefaultCellStyle.ForeColor = Color.White;
                        i++;
                    }
                    string message = MessageContainerList.GetMessage(utilities.ExecutionContext, 452);
                    textBoxMessageLine.Text = message;
                    Application.DoEvents();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                textBoxMessageLine.Text = ex.Message;
                HighLightErrorRecord(i);
                frmOKMsg.ShowUserMessage(ex.Message);
                KioskStatic.logToFile("Error Saving customer/relation - btnSave_Click in addRelation screen: " + ex.Message);
                Application.DoEvents();
                throw;
            }
            finally
            {
                EnableButtons();
                ResetKioskTimer();
                StartKioskTimer();
            }
            log.LogMethodExit();
        }

        private void HighLightErrorRecord(int i)
        {
            log.LogMethodEntry(i);
            try
            {
                if (customCustomerDTOList != null && customCustomerDTOList.Any())
                {
                    if (i >= 0 && i <= customCustomerDTOList.Count - 1)
                    {
                        dgvAddCustomerRelation.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                    }
                    else
                    {
                        dgvAddCustomerRelation.Rows[customCustomerDTOList.Count - 1].DefaultCellStyle.BackColor = Color.Red;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void btnComplete_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            KioskStatic.logToFile("Add Relation btnComplete Click");
            try
            {
                HideKeyBoard();
                ResetKioskTimer();
                SaveData();
                DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private bool HasPartialData()
        {
            log.LogMethodEntry();
            bool hasPartialData = false;
            if (customCustomerDTOList != null && customCustomerDTOList.Count != 0)
            {
                foreach (CustomCustomerDTO dto in customCustomerDTOList)
                {
                    if (dto.RelatedCustomerId == -1 || dto.ParentCustomerId == -1)
                    {
                        hasPartialData = true;
                        break;
                    }
                    else if (dto.RelatedCustomerId > -1
                        && (dto.CustomerRelationshipDTO != null
                             && (dto.CustomerRelationshipDTO.IsChanged
                                || (dto.CustomerRelationshipDTO.RelatedCustomerDTO != null
                                    && (dto.CustomerRelationshipDTO.RelatedCustomerDTO.IsChanged ||
                                         dto.CustomerRelationshipDTO.RelatedCustomerDTO.IsChangedRecursive)))))
                    {
                        hasPartialData = true;
                        break;
                    }
                }
            }
            log.LogMethodExit(hasPartialData);
            return hasPartialData;
        }

        public override void btnPrev_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                DisableButtons();
                HideKeyBoard();
                bool hasPartialData = HasPartialData();
                if (hasPartialData)
                {
                    string msg = MessageContainerList.GetMessage(utilities.ExecutionContext, 5171); //Do you want to go back without saving?;
                    using (frmYesNo yesNo = new frmYesNo(msg))
                    {
                        DialogResult dr = yesNo.ShowDialog();
                        if (dr == DialogResult.Yes)
                        {
                            StopKioskTimer();
                            Close();
                            Dispose();
                        }
                    }
                }
                else
                {
                    StopKioskTimer();
                    Close();
                    Dispose();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                KioskStatic.logToFile("Error while btnPrev_Click :" + ex.Message);
            }
            finally
            {
                EnableButtons();
            }
            log.LogMethodExit();
        }

        private void InitializeDGVAddCustomerRelationsDataGrid()
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("Initializing the Add Relation data grid Columns");
            try
            {
                ResetKioskTimer();

                List<CustomerRelationshipTypeDTO> customerRelationshipTypeDTOList = null;
                CustomerRelationshipTypeListBL customerRelationshipTypeListBL = new CustomerRelationshipTypeListBL(utilities.ExecutionContext);
                List<KeyValuePair<CustomerRelationshipTypeDTO.SearchByParameters, string>> searchCustomerRelationshipTypeParams = new List<KeyValuePair<CustomerRelationshipTypeDTO.SearchByParameters, string>>();
                searchCustomerRelationshipTypeParams.Add(new KeyValuePair<CustomerRelationshipTypeDTO.SearchByParameters, string>(CustomerRelationshipTypeDTO.SearchByParameters.IS_ACTIVE, "1"));
                searchCustomerRelationshipTypeParams.Add(new KeyValuePair<CustomerRelationshipTypeDTO.SearchByParameters, string>(CustomerRelationshipTypeDTO.SearchByParameters.SITE_ID, utilities.ExecutionContext.SiteId.ToString()));
                customerRelationshipTypeDTOList = customerRelationshipTypeListBL.GetCustomerRelationshipTypeDTOList(searchCustomerRelationshipTypeParams);
                if (customerRelationshipTypeDTOList == null)
                {
                    customerRelationshipTypeDTOList = new List<CustomerRelationshipTypeDTO>();
                }
                customerRelationshipTypeDTOList.Insert(0, new CustomerRelationshipTypeDTO() { Id = -1, Description = "<SELECT>" });
                customerRelationshipTypeIdDataGridViewComboBoxColumn.DataSource = customerRelationshipTypeDTOList;
                customerRelationshipTypeIdDataGridViewComboBoxColumn.DisplayMember = "Description";
                customerRelationshipTypeIdDataGridViewComboBoxColumn.ValueMember = "Id";
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                KioskStatic.logToFile("Error while getting relationship types :" + ex.Message);
            }
            try
            {
                address1DataGridViewTextBoxColumn.Visible = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "ADDRESS1").Equals("M") ? true : false;
                address2DataGridViewTextBoxColumn.Visible = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "ADDRESS2").Equals("M") ? true : false;
                address3DataGridViewTextBoxColumn.Visible = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "ADDRESS3").Equals("M") ? true : false;
                cityDataGridViewTextBoxColumn.Visible = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "CITY").Equals("M") ? true : false;
                postalCodeDataGridViewTextBoxColumn.Visible = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "PIN").Equals("M") ? true : false;
                contactPhoneDataGridViewTextBoxColumn.Visible = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "CONTACT_PHONE").Equals("M") ? true : false;
                emailAddressDataGridViewTextBoxColumn.Visible = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "EMAIL").Equals("M") ? true : false;
                dOBDGVTextBoxColumn.Visible = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "BIRTH_DATE").Equals("M") ? true : false;
                anniversaryDataGridViewComboBoxColumn.Visible = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "ANNIVERSARY").Equals("M") ? true : false;
                addressTypeDataGridViewTextBoxColumn.Visible = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "ADDRESS_TYPE").Equals("M") ? true : false;
                uniqueIdentifierDataGridViewTextBoxColumn.Visible = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "UNIQUE_ID").Equals("M") ? true : false;
                lastNameDataGridViewTextBoxColumn.Visible = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "LAST_NAME").Equals("M") ? true : false;
                membershipIdDataGridViewTextBoxColumn.Visible = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "MEMBERSHIPID").Equals("M") ? true : false;
                middleNameDataGridViewTextBoxColumn.Visible = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "MIDDLE_NAME").Equals("M") ? true : false;
                OptInPromotionsModeDataGridViewTextBoxColumn.Visible = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "OPT_IN_PROMOTIONS_MODE").Equals("M") ? true : false;
                PhotoURLDataGridViewTextBoxColumn.Visible = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "CUSTOMER_PHOTO").Equals("M") ? true : false;
                genderDataGridViewComboBoxColumn.Visible = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "GENDER").Equals("M") ? true : false;
                if (genderDataGridViewComboBoxColumn.Visible == true)
                {
                    genderDataGridViewComboBoxColumn.Items.AddRange(new object[] {
                                                                                    "Female",
                                                                                    "Male",
                                                                                    "Not Set"
                                                                                 });
                }

                titleDataGridViewComboBoxColumn.Visible = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "TITLE").Equals("M") ? true : false;
                if (titleDataGridViewComboBoxColumn.Visible == true)
                {
                    titleDataGridViewComboBoxColumn.DisplayIndex = 0;
                    LookupsContainerDTO customerTitleList = LookupsContainerList.GetLookupsContainerDTO(-1, "CUSTOMER_TITLES");

                    if (customerTitleList != null)
                    {
                        titleDataGridViewComboBoxColumn.DataSource = customerTitleList.LookupValuesContainerDTOList;
                        titleDataGridViewComboBoxColumn.ValueMember = titleDataGridViewComboBoxColumn.DisplayMember = "LookupValue";
                    }
                }

                stateDataGridViewComboBoxColumn.Visible = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "STATE").Equals("M") ? true : false;
                if (stateDataGridViewComboBoxColumn.Visible == true)
                {
                    List<CountryContainerDTO> countryContainerList = CountryContainerList.GetCountryContainerDTOList(utilities.ExecutionContext.SiteId);
                    countryContainerList.Insert(0, new CountryContainerDTO() { CountryId = -1, CountryName = "<SELECT>" });
                    string selectedCountryId = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "STATE_LOOKUP_FOR_COUNTRY");

                    if (!string.IsNullOrWhiteSpace(selectedCountryId) && selectedCountryId != "-1")
                        countryId = selectedCountryId;

                    if (!string.IsNullOrWhiteSpace(countryId) && countryId != "-1")
                    {
                        int stateCountryId = -1;
                        if (int.TryParse(countryId, out stateCountryId) == false)
                        {
                            string message = "Invalid country Id: Please check the set up";
                            log.Error(message);
                            KioskStatic.logToFile(message);
                            textBoxMessageLine.Text = message;
                            return;
                        }
                        if (countryContainerList.Exists(c => c.CountryId == stateCountryId) == false)
                        {
                            string message = "Invalid country Id: Please check the set up";
                            log.Error(message);
                            KioskStatic.logToFile(message);
                            textBoxMessageLine.Text = message;
                            return;
                        }
                        List<StateContainerDTO> StateContainerDTOList = countryContainerList.Where(x => x.CountryId == stateCountryId).FirstOrDefault().StateContainerDTOList;
                        stateDataGridViewComboBoxColumn.DataSource = StateContainerDTOList;
                        stateDataGridViewComboBoxColumn.DisplayMember = "Description";
                        stateDataGridViewComboBoxColumn.ValueMember = "StateId";
                    }
                }

                countryDataGridViewComboBoxColumn.Visible = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "COUNTRY").Equals("M") ? true : false;
                if (countryDataGridViewComboBoxColumn.Visible == true)
                {
                    List<CountryContainerDTO> countryContainerList = CountryContainerList.GetCountryContainerDTOList(utilities.ExecutionContext.SiteId);
                    if (countryContainerList != null && countryContainerList.Any())
                    {
                        countryContainerList.Insert(0, new CountryContainerDTO() { CountryId = -1, CountryName = "<SELECT>" });
                        countryDataGridViewComboBoxColumn.DataSource = countryContainerList;
                        countryDataGridViewComboBoxColumn.DisplayMember = "CountryName";
                        countryDataGridViewComboBoxColumn.ValueMember = "CountryId";
                    }
                }

                foreach (DataGridViewColumn col in dgvAddCustomerRelation.Columns)
                {
                    if (col.Visible == true && !string.IsNullOrWhiteSpace(col.HeaderText))
                        col.HeaderText += "*";
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                displayMessageLine(ex.Message, ERROR);
                StopKioskTimer();
                using (frmOKMsg frmOK = new frmOKMsg(ex.Message, true))
                {
                    frmOK.ShowDialog();
                }
                ResetKioskTimer();
                StartKioskTimer();
                KioskStatic.logToFile("Error while initializing the Add Relation data grid Columns: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void DownButtonClick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            log.LogMethodExit();
        }

        private void UpButtonClick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            log.LogMethodExit();
        }

        private void LeftButtonClick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            log.LogMethodExit();
        }

        private void RightButtonClick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            log.LogMethodExit();
        }

        public TextBox CurrentActiveTextBox;

        private void btnShowKeypad_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ResetKioskTimer();
            log.LogMethodExit();
        }

        void displayMessageLine(string message, string msgType)
        {
            log.LogMethodEntry(message, msgType);
            ResetKioskTimer();
            textBoxMessageLine.Text = message;
            log.LogMethodExit();
        }
        
        void customCustomerDTOBindingSource_AddingNew(object sender, AddingNewEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                ResetKioskTimer();
                e.NewObject = new CustomCustomerDTO(parentCustomerId);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            log.LogMethodExit();
        }

        private void customCustomerDTOBindingSource_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                ResetKioskTimer();
                e.Cancel = true;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            log.LogMethodExit(null);
        }

        private void dgvCustomCustomerDTOList_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ResetKioskTimer();
            if (e.Control is TextBox)
            {
                CurrentActiveTextBox = e.Control as TextBox;
                try
                {
                    Font font = new Font(e.CellStyle.Font.FontFamily, e.CellStyle.Font.Size);
                    e.CellStyle = dgvAddCustomerRelation.DefaultCellStyle;
                    e.CellStyle.Font = font;
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    KioskStatic.logToFile("Error while setting cell style in dgvCustomCustomerDTOList_EditingControlShowing() in add relation screen: " + ex.Message);
                }
            }
            else if (e.Control is ComboBox)
            {
                ComboBox cb = e.Control as ComboBox;
                if (cb != null)
                {
                    cb.IntegralHeight = false;
                    cb.MaxDropDownItems = 10;
                }
            }
            log.LogMethodExit();
        }

        private void dgvAddCustomerRelation_RowLeave(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                ResetKioskTimer();
                btnSave.Enabled = btnComplete.Enabled = (customCustomerDTOList == null || customCustomerDTOList.Count == 0) ? false : true;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            log.LogMethodExit();
        }

        private void dgvAddCustomerRelation_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ResetKioskTimer();

            if (e.RowIndex < 0)
                return;

            dgvAddCustomerRelation.EndEdit();
            try
            {
                dgvAddCustomerRelation.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor = SystemColors.Highlight;
                dgvAddCustomerRelation.Rows[e.RowIndex].DefaultCellStyle.SelectionForeColor = KioskStatic.CurrentTheme.AddCustomerRelationGridTextForeColor;
                dgvAddCustomerRelation.Rows[e.RowIndex].DefaultCellStyle.BackColor = SystemColors.Window;
                dgvAddCustomerRelation.Rows[e.RowIndex].DefaultCellStyle.ForeColor = KioskStatic.CurrentTheme.AddCustomerRelationGridTextForeColor;

                if (dgvAddCustomerRelation.Columns[e.ColumnIndex].Name == "countryDataGridViewComboBoxColumn")
                {
                    string value = dgvAddCustomerRelation.CurrentCell.Value.ToString();
                    if (!string.IsNullOrEmpty(value))
                    {
                        List<CountryContainerDTO> countryContainerList = CountryContainerList.GetCountryContainerDTOList(utilities.ExecutionContext.SiteId);
                        List<StateContainerDTO> StateContainerDTOList = countryContainerList.Where(x => x.CountryName == value).FirstOrDefault().StateContainerDTOList;
                        if (StateContainerDTOList != null && StateContainerDTOList.Any())
                        {
                            countryContainerList.Insert(0, new CountryContainerDTO() { CountryId = -1, CountryName = "<SELECT>" });
                            stateDataGridViewComboBoxColumn.DataSource = StateContainerDTOList;
                            stateDataGridViewComboBoxColumn.DisplayMember = "Description";
                            stateDataGridViewComboBoxColumn.ValueMember = "StateId";
                        }
                    }
                }
                else if (dgvAddCustomerRelation.Columns[e.ColumnIndex].Name == "anniversaryDataGridViewComboBoxColumn")
                {
                    StopKioskTimer();
                    HideKeyBoard();
                    dgvAddCustomerRelation.CurrentRow.DefaultCellStyle.SelectionBackColor = SystemColors.Highlight;
                    dgvAddCustomerRelation.CurrentRow.DefaultCellStyle.SelectionForeColor = KioskStatic.CurrentTheme.AddCustomerRelationGridTextForeColor;
                    DateTime doB = (dgvAddCustomerRelation.CurrentCell.Value == null) ?
                                           DateTime.MinValue : Convert.ToDateTime(dgvAddCustomerRelation.CurrentCell.Value);
                    DateTime newDOB = KioskHelper.LaunchCalendar(defaultDateTimeToShow: doB, enableDaySelection: KioskStatic.enableDaySelection, enableMonthSelection: KioskStatic.enableMonthSelection,
                        enableYearSelection: KioskStatic.enableYearSelection, disableTill: DateTime.MinValue, showTimePicker: false, popupAlerts: frmOKMsg.ShowUserMessage);
                    string dateMonthformat = KioskStatic.DateMonthFormat;
                    DateTime anniversaryDate = DateTime.MinValue;
                    try
                    {
                        anniversaryDate = KioskHelper.GetFormatedDateValue(newDOB);
                        if (anniversaryDate != Convert.ToDateTime(customCustomerDTOList[dgvAddCustomerRelation.CurrentRow.Index].Anniversary))
                        {
                            dgvAddCustomerRelation.Rows[dgvAddCustomerRelation.CurrentRow.Index].Cells["anniversaryDataGridViewComboBoxColumn"].Value = anniversaryDate.Date.ToString(KioskStatic.DateMonthFormat);
                            customCustomerDTOList[dgvAddCustomerRelation.CurrentRow.Index].Anniversary = anniversaryDate.Date.ToString();
                        }
                    }
                    catch (Exception ex)
                    {
                        try
                        {
                            System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.CurrentCulture;
                            anniversaryDate = Convert.ToDateTime(newDOB, provider);
                            if (anniversaryDate != Convert.ToDateTime(customCustomerDTOList[dgvAddCustomerRelation.CurrentRow.Index].Anniversary))
                            {
                                dgvAddCustomerRelation.Rows[dgvAddCustomerRelation.CurrentRow.Index].Cells["anniversaryDataGridViewComboBoxColumn"].Value = anniversaryDate.Date.ToString(KioskStatic.DateMonthFormat);
                                customCustomerDTOList[dgvAddCustomerRelation.CurrentRow.Index].Anniversary = anniversaryDate.Date.ToString();
                            }
                        }
                        catch (Exception exp)
                        {
                            displayMessageLine(MessageContainerList.GetMessage(utilities.ExecutionContext, 449, dateMonthformat, Convert.ToDateTime("23-Feb-1982").ToString(dateMonthformat)), ERROR);
                            log.Error(exp);
                            log.Error(MessageContainerList.GetMessage(utilities.ExecutionContext, 449, dateMonthformat, Convert.ToDateTime("23-Feb-1982").ToString(dateMonthformat)));
                        }
                        log.Error(ex);
                    }
                    dgvAddCustomerRelation.RefreshEdit();
                    dgvAddCustomerRelation_UpdateBindDataSource();
                    UpdateBindDataSource();
                    StartKioskTimer();
                }
                
                else if (dgvAddCustomerRelation.Columns[e.ColumnIndex].Name == "dgvAddCustomerRelation_btnMore")
                {
                    if (Convert.ToInt32(dgvAddCustomerRelation.CurrentRow.Cells["customerRelationshipTypeIdDataGridViewComboBoxColumn"].Value) > 0)
                    {
                        StopKioskTimer();
                        HideKeyBoard();
                        dgvAddCustomerRelation.EndEdit();
                        //dgvAddCustomerRelation_UpdateBindDataSource();
                        //UpdateBindDataSource();

                        int index = dgvAddCustomerRelation.CurrentRow.Index;

                        CustomerDTO editRelatedCustomerDTO = new CustomerDTO();

                        editRelatedCustomerDTO = customCustomerDTOList[index].CustomerRelationshipDTO.RelatedCustomerDTO;

                        KioskStatic.logToFile("Invoking Customer Screen from Add Relation More Click");
                        using (Customer customer = new Customer(string.Empty, null, false, editRelatedCustomerDTO))
                        {
                            DialogResult dr = customer.ShowDialog();
                            if (dr == DialogResult.Cancel)
                            {
                                string msg = MessageContainerList.GetMessage(utilities.ExecutionContext, "Timeout");
                                throw new CustomerStatic.TimeoutOccurred(msg);
                            }
                            KioskStatic.logToFile("Returning back to Add Relation screen on btnSave Click");
                            if (customer.customerDTO != null)
                            {
                                customCustomerDTOList[index].RelatedCustomerId = customer.customerDTO.Id;
                                displayMessageLine(MessageContainerList.GetMessage(utilities.ExecutionContext, 254), MESSAGE);
                            }
                            customer.Close();
                        }
                        dgvAddCustomerRelation_UpdateBindDataSource();
                        StartKioskTimer();
                    }
                    else
                    {
                        string message = MessageContainerList.GetMessage(utilities.ExecutionContext, 249, dgvAddCustomerRelation.Columns["customerRelationshipTypeIdDataGridViewComboBoxColumn"].HeaderText);
                        displayMessageLine(message, ERROR);
                        log.Debug(message);
                        StopKioskTimer();
                        frmOKMsg.ShowUserMessage(message);
                        ResetKioskTimer();
                        StartKioskTimer();
                    }
                }
                else if (dgvAddCustomerRelation.Columns[e.ColumnIndex].Name == "dOBDGVTextBoxColumn")
                {
                    StopKioskTimer();
                    HideKeyBoard();
                    dgvAddCustomerRelation.CurrentRow.DefaultCellStyle.SelectionBackColor = SystemColors.Highlight;
                    dgvAddCustomerRelation.CurrentRow.DefaultCellStyle.SelectionForeColor = KioskStatic.CurrentTheme.AddCustomerRelationGridTextForeColor;
                    DateTime doB = (dgvAddCustomerRelation.CurrentCell.Value == null) ?
                                           DateTime.MinValue : Convert.ToDateTime(dgvAddCustomerRelation.CurrentCell.Value);
                    DateTime newDOB = KioskHelper.LaunchCalendar(defaultDateTimeToShow: doB, enableDaySelection: KioskStatic.enableDaySelection, enableMonthSelection: KioskStatic.enableMonthSelection,
                        enableYearSelection: KioskStatic.enableYearSelection, disableTill: DateTime.MinValue, showTimePicker: false, popupAlerts: frmOKMsg.ShowUserMessage);
                    string dateMonthformat = KioskStatic.DateMonthFormat;
                    DateTime dateofBirthValue = DateTime.MinValue;
                    try
                    {
                        dateofBirthValue = KioskHelper.GetFormatedDateValue(newDOB);
                        if (dateofBirthValue != Convert.ToDateTime(customCustomerDTOList[dgvAddCustomerRelation.CurrentRow.Index].DOB))
                        {
                            dgvAddCustomerRelation.Rows[dgvAddCustomerRelation.CurrentRow.Index].Cells["dOBDGVTextBoxColumn"].Value = dateofBirthValue.Date.ToString(KioskStatic.DateMonthFormat);
                            customCustomerDTOList[dgvAddCustomerRelation.CurrentRow.Index].DOB = dateofBirthValue;
                        }
                    }
                    catch (Exception ex)
                    {
                        try
                        {
                            System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.CurrentCulture;
                            dateofBirthValue = Convert.ToDateTime(newDOB, provider);
                            if (dateofBirthValue != Convert.ToDateTime(customCustomerDTOList[dgvAddCustomerRelation.CurrentRow.Index].DOB))
                            {
                                dgvAddCustomerRelation.Rows[dgvAddCustomerRelation.CurrentRow.Index].Cells["dOBDGVTextBoxColumn"].Value = dateofBirthValue.Date.ToString(KioskStatic.DateMonthFormat);
                                customCustomerDTOList[dgvAddCustomerRelation.CurrentRow.Index].DOB = dateofBirthValue;
                            }
                        }
                        catch (Exception exp)
                        {
                            displayMessageLine(MessageContainerList.GetMessage(utilities.ExecutionContext, 449, dateMonthformat, Convert.ToDateTime("23-Feb-1982").ToString(dateMonthformat)), ERROR);
                            log.Error(exp);
                            log.Error(MessageContainerList.GetMessage(utilities.ExecutionContext, 449, dateMonthformat, Convert.ToDateTime("23-Feb-1982").ToString(dateMonthformat)));
                        }
                        log.Error(ex);
                    }
                    //customCustomerDTOList[e.RowIndex].DOB = Convert.ToDateTime(newDOB);
                    if (customCustomerDTOList[e.RowIndex].RelatedCustomerId <= -1 &&
                        dateofBirthValue != null &&
                        dateofBirthValue.Date != null)
                    {
                        if (!string.IsNullOrWhiteSpace(dateofBirthValue.Date.ToString()))
                        {
                            decimal age = KioskHelper.GetAge(dateofBirthValue.Date.ToString());
                            decimal ageOfMajority = ParafaitDefaultContainerList.GetParafaitDefault<decimal>(KioskStatic.Utilities.ExecutionContext, "AGE_OF_MAJORITY", -1);

                            if (age <= ageOfMajority)
                            {
                                CustomerDTO parentCustomerDTO = customCustomerDTOList[0].GetCustomerDTO(parentCustomerId);
                                customCustomerDTOList[e.RowIndex].ContactPhone = parentCustomerDTO.PhoneNumber;
                                customCustomerDTOList[e.RowIndex].EmailAddress = parentCustomerDTO.Email;
                                customCustomerDTOList[e.RowIndex].CustomerType = CustomerType.UNREGISTERED;
                            }
                        }
                    }
                    dgvAddCustomerRelation.RefreshEdit();
                    dgvAddCustomerRelation_UpdateBindDataSource();
                    UpdateBindDataSource();
                    StartKioskTimer();
                }
                else
                {
                    //nothing to do
                }
            }
            catch (CustomerStatic.TimeoutOccurred ex)
            {
                KioskStatic.logToFile("Timeout occured");
                log.Error(ex);
                PerformTimeoutAbortAction(kioskTransaction, kioskAttractionDTO);
                this.DialogResult = DialogResult.Cancel;
                log.LogMethodExit();
                return;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                KioskStatic.logToFile("Error while dgvAddCustomerRelation_CellClick() : " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void UpdateBindDataSource()
        {
            log.LogMethodEntry();

            if (dgvAddCustomerRelation.Rows.Count <= 0)
                return;

            try
            {
                ResetKioskTimer();
                dOBDGVTextBoxColumn.DefaultCellStyle.NullValue = KioskStatic.DateMonthFormat;
                dgvAddCustomerRelation.Rows[customCustomerDTOList.Count - 1].Selected = true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error while UpdateBindDataSource() - Add Customer Relation Screen: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void dgvAddCustomerRelation_UpdateBindDataSource()
        {
            log.LogMethodEntry();
            try
            {
                ResetKioskTimer();
                this.dgvAddCustomerRelation.ClearSelection();
                customCustomerDTOBindingSource.DataSource = customCustomerDTOList;
                this.dgvAddCustomerRelation.DataSource = customCustomerDTOBindingSource;
                this.dgvAddCustomerRelation.Refresh();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            log.LogMethodExit();
        }

        private void dgvAddCustomerRelation_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                ResetKioskTimer();
                if (e.Row.Index < 0)
                    return;

                if (e.Row.Cells["contactPhoneDataGridViewTextBoxColumn"].Value != null)
                {
                    if (e.Row.Cells["contactPhoneDataGridViewTextBoxColumn"].Value.ToString() == "")
                    {
                        e.Row.Cells["contactPhoneDataGridViewTextBoxColumn"].Value = null;
                    }
                }
                if (e.Row.Cells["emailAddressDataGridViewTextBoxColumn"].Value != null)
                {
                    if (e.Row.Cells["emailAddressDataGridViewTextBoxColumn"].Value.ToString() == "")
                    {
                        e.Row.Cells["emailAddressDataGridViewTextBoxColumn"].Value = null;
                    }
                }
                if (e.Row.Cells["dOBDGVTextBoxColumn"].Value != null)
                {
                    if (e.Row.Cells["dOBDGVTextBoxColumn"].Value.ToString() == "")
                    {
                        e.Row.Cells["dOBDGVTextBoxColumn"].Value = null;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            log.LogMethodExit();
        }

        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            //KioskStatic.logToFile("Setting customized font colors for the UI elements in add customer relations");
            try
            {
                foreach (Control c in dgvAddCustomerRelation.Controls)
                {
                    c.ForeColor = KioskStatic.CurrentTheme.AddCustomerRelationGridTextForeColor;
                }
                this.lblSiteName.ForeColor = KioskStatic.CurrentTheme.SiteHeadingForeColor;
                this.lblGreeting.ForeColor = KioskStatic.CurrentTheme.AddCustomerRelationLblGreetingTextForeColor;
                this.lblAddParticipants.ForeColor = KioskStatic.CurrentTheme.AddCustomerRelationLblAddParticipantsTextForeColor;
                this.textBoxMessageLine.ForeColor = KioskStatic.CurrentTheme.AddCustomerRelationTxtboxMessageLineTextForeColor;//Footer text message
                this.txtCustomerName.ForeColor = KioskStatic.CurrentTheme.AddCustomerRelationTxtCustomerNameTextForeColor;
                this.txtCardNumber.ForeColor = KioskStatic.CurrentTheme.AddCustomerRelationTxtCardNumberTextForeColor;
                this.btnPrev.ForeColor = this.btnSave.ForeColor = this.btnComplete.ForeColor = KioskStatic.CurrentTheme.AddCustomerRelationBtnBackTextForeColor;
                this.btnHome.ForeColor = KioskStatic.CurrentTheme.AddCustomerRelationBtnHomeTextForeColor;
                this.lblTimeRemaining.ForeColor = KioskStatic.CurrentTheme.AddCustomerRelationLblTimeRemainingTextForeColor;
                this.lblCardNumber.ForeColor = KioskStatic.CurrentTheme.AddCustomerRelationLblCardNumberForeColor;
                this.lblCustomerName.ForeColor = KioskStatic.CurrentTheme.AddCustomerRelationLblCustomerNameForeColor;
                this.lblGridFooterMsg.ForeColor = KioskStatic.CurrentTheme.AddCustomerRelationLblGridFooterTextForeColor;
                this.bigVerticalScrollCustomerRelation.InitializeScrollBar(ThemeManager.CurrentThemeImages.ScrollDownEnabled, ThemeManager.CurrentThemeImages.ScrollDownDisabled, ThemeManager.CurrentThemeImages.ScrollUpEnabled, ThemeManager.CurrentThemeImages.ScrollUpDisabled);
                this.bigHorizontalScrollCustomerRelation.InitializeScrollBar(ThemeManager.CurrentThemeImages.ScrollLeftEnabled, ThemeManager.CurrentThemeImages.ScrollLeftDisabled, ThemeManager.CurrentThemeImages.ScrollRightEnabled, ThemeManager.CurrentThemeImages.ScrollRightDisabled);
                this.dgvAddCustomerRelation.ColumnHeadersDefaultCellStyle.ForeColor = KioskStatic.CurrentTheme.AddCustomerRelationGridHeaderTextForeColor;
                this.dgvAddCustomerRelation.ForeColor = KioskStatic.CurrentTheme.AddCustomerRelationGridTextForeColor;
                this.dgvAddCustomerRelation.DefaultCellStyle.ForeColor = KioskStatic.CurrentTheme.AddCustomerRelationGridTextForeColor;
                this.dgvAddCustomerRelation.DefaultCellStyle.SelectionForeColor = KioskStatic.CurrentTheme.AddCustomerRelationGridTextForeColor;
                this.dgvAddCustomerRelation.RowsDefaultCellStyle.ForeColor = KioskStatic.CurrentTheme.AddCustomerRelationGridTextForeColor;
                this.dgvAddCustomerRelation.RowsDefaultCellStyle.SelectionForeColor = KioskStatic.CurrentTheme.AddCustomerRelationGridTextForeColor;
            }
            catch (Exception ex)
            {
                log.Error("Error while Setting Customized Fore Colors", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements in add customer relations: " + ex.Message);
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
                displayMessageLine(MessageContainerList.GetMessage(utilities.ExecutionContext, 457), WARNING);
                Application.DoEvents();
                base.CloseForms();
                Dispose();
            }
            log.LogMethodExit();
        }

        private void frmAddCustomerRelation_FormClosed(object sender, FormClosedEventArgs e)
        {
            log.LogMethodEntry();
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

            log.Info(this.Name + ": Form closed");
            log.LogMethodExit();
        }

        private void dgvAddCustomerRelation_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            log.LogMethodEntry(sender, e);
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

        private void FormOnKeyDown(object sender, KeyEventArgs e)
        {
            log.LogMethodEntry(e);
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
        private void FormOnKeyPress(object sender, KeyPressEventArgs e)
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
        private void FormOnKeyUp(object sender, KeyEventArgs e)
        {
            log.LogMethodEntry(e);
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
        private void FormOnMouseClick(Object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(); try
            {
                ResetKioskTimer();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void InsertNewCustomCustomerDTO()
        {
            log.LogMethodEntry();
            ResetKioskTimer();

            try
            {
                if (customCustomerDTOList == null || customCustomerDTOList.Count == 0)
                {
                    CustomCustomerDTO relationToBeLinkedDTO = new CustomCustomerDTO(parentCustomerId);
                    customCustomerDTOList = new SortableBindingList<CustomCustomerDTO>();
                    customCustomerDTOList.Add(relationToBeLinkedDTO);
                }
                if (customCustomerDTOList.Count > 0)
                {
                    customCustomerDTOList[0].RelatedCustomerName = relatedCustomerName;
                    customCustomerDTOList[0].DOB = Convert.ToDateTime(relatedDateOfBirth);
                    customCustomerDTOList[0].ParentCustomerId = parentCustomerId;
                    if (customCustomerDTOList[0].RelatedCustomerId <= -1)
                    {
                        if (!string.IsNullOrWhiteSpace(relatedDateOfBirth))
                        {
                            decimal age = KioskHelper.GetAge(relatedDateOfBirth);
                            decimal ageOfMajority = ParafaitDefaultContainerList.GetParafaitDefault<decimal>(KioskStatic.Utilities.ExecutionContext, "AGE_OF_MAJORITY", -1);

                            if (age <= ageOfMajority)
                            {
                                CustomerDTO parentCustomerDTO = customCustomerDTOList[0].GetCustomerDTO(parentCustomerId);
                                customCustomerDTOList[0].ContactPhone = parentCustomerDTO.PhoneNumber;
                                customCustomerDTOList[0].EmailAddress = parentCustomerDTO.Email;
                                customCustomerDTOList[0].CustomerType = CustomerType.UNREGISTERED;
                            }
                        }
                    }
                    UpdateBindDataSource();
                    dgvAddCustomerRelation.EndEdit();
                    dgvAddCustomerRelation.Refresh();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error while InsertNewCustomCheckInDetailDTO() in frmEnterChildDetails: " + ex);
            }
            log.LogMethodExit();
        }

        private void SetFont()
        {
            log.LogMethodEntry();
            ResetKioskTimer();

            try
            {
                dgvAddCustomerRelation.Font = new System.Drawing.Font(lblGreeting.Font.FontFamily, 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                dgvAddCustomerRelation.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font(lblGreeting.Font.FontFamily, 21F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

                dgvAddCustomerRelation.DefaultCellStyle.Font =
                    dgvAddCustomerRelation.RowTemplate.DefaultCellStyle.Font =
                    dgvAddCustomerRelation.RowHeadersDefaultCellStyle.Font =
                    dgvAddCustomerRelation.RowsDefaultCellStyle.Font = new System.Drawing.Font(lblGreeting.Font.FontFamily, 21F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

                lblGreeting.Font = new Font(lblGreeting.Font.FontFamily, 38F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                lblAddParticipants.Font = new Font(lblGreeting.Font.FontFamily, 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

                //dgvAddCustomerRelation.ColumnHeadersDefaultCellStyle.BackColor = SystemColors.Highlight;
                //dgvAddCustomerRelation.ColumnHeadersDefaultCellStyle.ForeColor = SystemColors.HighlightText;
                dgvAddCustomerRelation.DefaultCellStyle.SelectionBackColor = this.dgvAddCustomerRelation.DefaultCellStyle.BackColor;
                dgvAddCustomerRelation.DefaultCellStyle.SelectionForeColor = this.dgvAddCustomerRelation.DefaultCellStyle.ForeColor;
            }
            catch (Exception ex)
            {
                string msg = "Unexpected error while Setting Customized background images for dgvEnterAdultDetails: ";
                log.Error(msg, ex);
                KioskStatic.logToFile(msg + ex);
            }
            log.LogMethodExit();
        }

        private void SetVirtualKeyboard()
        {
            log.LogMethodEntry();
            try
            {
                virtualKeyboardController = new VirtualWindowsKeyboardController(panelAddRelations.Top);
                bool popupOnScreenKeyBoard = true;
                virtualKeyboardController.Initialize(this, new List<Control>() { btnShowKeyPad }, popupOnScreenKeyBoard);
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
                customKeyboardController = new VirtualKeyboardController(panelAddRelations.Top);
                bool showKeyboardOnTextboxEntry = true;
                customKeyboardController.Initialize(this, new List<Control>() { btnShowKeyPad }, showKeyboardOnTextboxEntry, null, lblAddParticipants.Font.FontFamily.Name);

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
        private void EnableButtons()
        {
            log.LogMethodEntry();
            try
            {
                this.btnSave.Enabled = true;
                this.btnComplete.Enabled = true;
                this.btnPrev.Enabled = true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in EnableButtons() in Add Relation screen: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void DisableButtons()
        {
            log.LogMethodEntry();
            try
            {
                this.btnSave.Enabled = false;
                this.btnComplete.Enabled = false;
                this.btnPrev.Enabled = false;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in DisableButtons() in Add Relation screen: " + ex.Message);
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
                KioskStatic.logToFile("Error Hiding keyboard in Get Customer Input screen: " + ex.Message);
            }
            log.LogMethodExit();
        }
    }
}


























