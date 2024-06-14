/********************************************************************************************
 * Project Name - CustomerDetailUI
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By          Remarks          
 *********************************************************************************************
 *1.00        06-Feb-2017      Lakshminarayana     Created 
 *2.4.0       25-Nov-2018      Raghuveera           Modified to Include changes related to GDPR     
 *                                                  GetPromotionMode(), UpdateCustomerDetailsUI, 
 *                                                  , UpdateCustomerDTO, cchbTermsandConditions_Click events
 *2.70.2       01-Sept-2019      Girish kundar      Modified : UI Changes for Customer.                                                  
 *2.70.2       1-Feb-2020       Girish Kundar       Modified: BirthDate and AniversaryDate validation
 *2.70.3       14-Feb-2020     Lakshminarayana      Modified: Creating unregistered customer during check-in process
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.User;
using Semnox.Parafait.Customer.Membership;
using Semnox.Parafait.Communication;
using System.Text.RegularExpressions;
using Semnox.Parafait.Customer.UI;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Customer
{
    /// <summary>
    /// Customer Details UI
    /// </summary>
    public partial class CustomerDetailUI : UserControl
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private CustomerDTO customerDTO;
        private CustomDataTableLayoutPanel customDataTableLayoutPanel;
        private Utilities utilities;
        private Dictionary<string, Control> attributeControlDictionary;
        private MessageBoxDelegate messageBoxDelegate;
        private List<StateDTO> stateDTOList;
        private VirtualKeyboardController virtualKeyboardController;

        /// <summary>
        /// Event generated when unique identifier value changes
        /// </summary>
        public event CancelEventHandler UniqueIdentifierValidating;

        private bool preventContactInfoEnteredEvent = false;
        /// <summary>
        /// Event generated when customer contact information is entered
        /// </summary>
        public event CustomerContactInfoEnteredEventHandler CustomerContactInfoEntered;

        /// <summary>
        /// Event generated when customer first name is entered
        /// </summary>
        public event EventHandler FirstNameLeave;
        /// <summary>
        /// Parameterized Constructor Customer details UI
        /// </summary>
        /// <param name="utilities">Parafait Utilities</param>
        /// <param name="messageBoxDelegate">message box delegate</param>
        /// <param name="showKeyboardOnTextboxEntry">show virtual keyboard</param>
        public CustomerDetailUI(Utilities utilities, MessageBoxDelegate messageBoxDelegate, bool showKeyboardOnTextboxEntry = true)
        {
            log.LogMethodEntry(utilities, messageBoxDelegate);

            InitializeComponent();
            this.utilities = utilities;
            this.addressListView.SetExecutionContext = utilities.ExecutionContext;
            System.Drawing.Font font;
            try
            {
                font = new Font(utilities.ParafaitEnv.DEFAULT_GRID_FONT, 15, FontStyle.Regular);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while applying new font", ex);
                font = new Font("Tahoma", 15, FontStyle.Regular);
            }
            utilities.setLanguage(this);
            this.messageBoxDelegate = messageBoxDelegate;
            attributeControlDictionary = new Dictionary<string, Control>();
            UpdateCustomerDetailsUI();
            SetDataSources();
            SetCustomerNameCue();
            CreateCustomAttributeUI();
            SetDateTimeFormat();
            CustomerDTO = new CustomerDTO();
            SetCustomerMaxInputLenghts();
            virtualKeyboardController = new VirtualKeyboardController();
            virtualKeyboardController.Initialize(this, new List<Control>() { btnShowKeyPad }, showKeyboardOnTextboxEntry);
            contactListView.CustomerContactInfoEntered += CustomerContactInfo_OnEnter;
            addressListView.ShowKeyboardOnTextboxEntry = showKeyboardOnTextboxEntry;
            addressListView.RefreshAddressDTOList += chbShowActiveAddressEntries_CheckedChanged;
            contactListView.RefreshVirtualKeyBoard += VirtualKeyBoard_OnRefresh;
            contactListView.RefreshContactDTOList += chbShowActiveContactEntries_CheckedChanged;
            SetUserControlHeight();
            log.LogMethodExit();
        }
        private void SetUserControlHeight()
        {
            log.LogMethodEntry();
            addressListView.Height = (flpCustomer.Height -(flpPersonalInfo.Location.Y+ flpPersonalInfo.Height + 10));
            contactListView.Height = addressListView.Height;
            log.LogMethodExit();
        }

        private void VirtualKeyBoard_OnRefresh(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            virtualKeyboardController.Refresh();
            log.LogMethodExit();
        }

        private void General_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Control tt = (Control)sender;
                Control nextCtl = this.GetNextControl(tt, true);
                while (nextCtl != null && nextCtl is TextBox == false && nextCtl is MaskedTextBox == false)
                {
                    nextCtl = this.GetNextControl(nextCtl, true);
                }
                this.ActiveControl = nextCtl;
            }
        }

        private void SetDataSources()
        {
            log.LogMethodEntry();
            SetTitleDataSource();
            SetGenderDataSource();
            SetCustomerTypeDataSource();
            contactListView.ContactTypeDTOList = GetContactTypeDTOList();
            contactListView.ExecutionContext = utilities.ExecutionContext;
            stateDTOList = GetStateDTOList();
            cmbMembership.DisplayMember = "MembershipName";
            cmbMembership.ValueMember = "MembershipID";
            cmbMembership.DataSource = GetMembershipDTOList();
            cmbPromotionMode.DataSource = GetPromotionMode();
            cmbPromotionMode.ValueMember = "LookupValue";
            cmbPromotionMode.DisplayMember = "Description";
            log.LogMethodExit();
        }

        private List<LookupValuesDTO> GetPromotionMode()
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
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while loading lookupValues list", ex);
            }
            log.LogMethodExit(lookupValuesDTOList);
            return lookupValuesDTOList;
        }
        private List<MembershipDTO> GetMembershipDTOList()
        {
            log.LogMethodEntry();
            List<MembershipDTO> membershipDTOList = null;
            try
            {
                MembershipsList membershipsList = new MembershipsList(utilities.ExecutionContext);
                List<KeyValuePair<MembershipDTO.SearchByParameters, string>> searchMemberParams = new List<KeyValuePair<MembershipDTO.SearchByParameters, string>>();
                //searchMemberParams.Add(new KeyValuePair<MembershipDTO.SearchByParameters, string>(MembershipDTO.SearchByParameters.IS_ACTIVE, "1"));
                //searchMemberParams.Add(new KeyValuePair<MembershipDTO.SearchByParameters, string>(MembershipDTO.SearchByParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));
                membershipDTOList = membershipsList.GetAllMembership(searchMemberParams, utilities.ExecutionContext.GetSiteId());
                if (membershipDTOList == null)
                {
                    membershipDTOList = new List<MembershipDTO>();
                }
                membershipDTOList.Insert(0, new MembershipDTO());
                membershipDTOList[0].MembershipName = "";
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while loading membership list", ex);
            }
            log.LogMethodExit(membershipDTOList);
            return membershipDTOList;
        }

        private List<AddressTypeDTO> GetAddressTypeDTOList()
        {
            log.LogMethodEntry();
            List<AddressTypeDTO> addressTypeDTOList = null;
            AddressTypeListBL addressTypeListBL = new AddressTypeListBL(utilities.ExecutionContext);
            List<KeyValuePair<AddressTypeDTO.SearchByParameters, string>> searchAddressTypeParams = new List<KeyValuePair<AddressTypeDTO.SearchByParameters, string>>();
            searchAddressTypeParams.Add(new KeyValuePair<AddressTypeDTO.SearchByParameters, string>(AddressTypeDTO.SearchByParameters.IS_ACTIVE, "1"));
            addressTypeDTOList = addressTypeListBL.GetAddressTypeDTOList(searchAddressTypeParams);
            if (addressTypeDTOList == null)
            {
                addressTypeDTOList = new List<AddressTypeDTO>();
            }
            addressTypeDTOList.Insert(0, new AddressTypeDTO());
            addressTypeDTOList[0].Description = "SELECT";
            log.LogMethodExit(addressTypeDTOList);
            return addressTypeDTOList;
        }

        private List<ContactTypeDTO> GetContactTypeDTOList()
        {
            log.LogMethodEntry();
            List<ContactTypeDTO> contactTypeDTOList = null;
            ContactTypeListBL contactTypeListBL = new ContactTypeListBL(utilities.ExecutionContext);
            List<KeyValuePair<ContactTypeDTO.SearchByParameters, string>> searchContactTypeParams = new List<KeyValuePair<ContactTypeDTO.SearchByParameters, string>>();
            searchContactTypeParams.Add(new KeyValuePair<ContactTypeDTO.SearchByParameters, string>(ContactTypeDTO.SearchByParameters.IS_ACTIVE, "1"));
            contactTypeDTOList = contactTypeListBL.GetContactTypeDTOList(searchContactTypeParams);
            if (contactTypeDTOList == null)
            {
                contactTypeDTOList = new List<ContactTypeDTO>();
            }
            contactTypeDTOList.Insert(0, new ContactTypeDTO());
            contactTypeDTOList[0].Description = "SELECT";
            log.LogMethodExit(contactTypeDTOList);
            return contactTypeDTOList;
        }

        private List<CountryDTO> GetCountryDTOList()
        {
            log.LogMethodEntry();
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
            log.LogMethodExit(countryList);
            return countryList;
        }

        private List<StateDTO> GetStateDTOList()
        {
            log.LogMethodEntry();
            List<StateDTO> stateList = null;
            StateDTOList stateDTOList = new StateDTOList(utilities.ExecutionContext);
            List<KeyValuePair<StateDTO.SearchByParameters, string>> searchStateParams = new List<KeyValuePair<StateDTO.SearchByParameters, string>>();
            searchStateParams.Add(new KeyValuePair<StateDTO.SearchByParameters, string>(StateDTO.SearchByParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));
            stateList = stateDTOList.GetStateDTOList(searchStateParams);
            if (stateList == null)
            {
                stateList = new List<StateDTO>();
            }
            stateList.Insert(0, new StateDTO());
            stateList[0].StateId = -1;
            stateList[0].State = "SELECT";
            log.LogMethodExit(stateList);
            return stateList;
        }

        private void SetDateTimeFormat()
        {
            log.LogMethodEntry();
            string dateMonthFormat = GetDateMonthFormat();
            txtBirthDate.Mask = dateMonthFormat.Replace("d", "0").Replace("MMM", ">LLL").Replace("MM", "00").Replace("y", "0");
            txtAnniversaryDate.Mask = dateMonthFormat.Replace("d", "0").Replace("MMM", ">LLL").Replace("MM", "00").Replace("y", "0");
            lblBirthDateFormat.Text = DateTime.Now.ToString(dateMonthFormat);
            lblAnniversaryDateFormat.Text = DateTime.Now.ToString(dateMonthFormat);
            log.LogMethodExit();
        }

        private void SetCustomerNameCue()
        {
            log.LogMethodEntry();
            txtFirstName.Cue = MessageContainerList.GetMessage(utilities.ExecutionContext, "First Name");
            txtMiddleName.Cue = MessageContainerList.GetMessage(utilities.ExecutionContext, "Middle Name");
            txtLastName.Cue = MessageContainerList.GetMessage(utilities.ExecutionContext, "Last Name");
            log.LogMethodExit();
        }

        private void CreateCustomAttributeUI()
        {
            log.LogMethodEntry();
            customDataTableLayoutPanel = new CustomDataTableLayoutPanel(utilities.ExecutionContext, Applicability.CUSTOMER);
            customDataTableLayoutPanel.SetFont(new Font("Arial", 15F, FontStyle.Regular));
            for (int i = 0; i < customDataTableLayoutPanel.RowCount - 1; i++)
            {
                FlowLayoutPanel flowLayoutPanelForCustomData = new FlowLayoutPanel();
                flowLayoutPanelForCustomData.AutoSize = true;
                flowLayoutPanelForCustomData.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                flowLayoutPanelForCustomData.Margin = new Padding(0);
                flowLayoutPanelForCustomData.FlowDirection = FlowDirection.LeftToRight;
                flowLayoutPanelForCustomData.WrapContents = true;
                for (int j = 0; j < customDataTableLayoutPanel.ColumnCount; j++)
                {
                    Control control = new Control();
                    control = customDataTableLayoutPanel.GetControlFromPosition(j, i);
                    if (control != null)
                    {
                        if (control.GetType() == typeof(Label))
                        {
                            Label label = (Label)control;
                            label.TextAlign = ContentAlignment.MiddleRight;
                            label.AutoSize = false;
                            label.Height = 32;
                            label.Width = 85;
                            label.Margin = new Padding(2);
                            label.Anchor = AnchorStyles.None;
                            flowLayoutPanelForCustomData.Controls.Add(label);
                        }
                        else if (control.GetType() == typeof(CustomDataListComboBoxControl))
                        {
                            CustomDataListComboBoxControl comboBox = (CustomDataListComboBoxControl)control;
                            comboBox.Margin = new Padding(2);
                            comboBox.AutoSize = false;
                            comboBox.Width = 120;
                            comboBox.Height = 31;
                            comboBox.Anchor = AnchorStyles.Right;
                            flowLayoutPanelForCustomData.Controls.Add(comboBox);
                        }
                        else if (control.GetType() == typeof(CustomDataListCheckBoxControl))
                        {
                            CustomDataListCheckBoxControl checkBox = (CustomDataListCheckBoxControl)control;
                            checkBox.Margin = new Padding(2);
                            checkBox.AutoSize = false;
                            checkBox.Width = 120;
                            checkBox.Height = 25;
                            checkBox.Anchor = AnchorStyles.Right;
                            flowLayoutPanelForCustomData.Controls.Add(checkBox);
                        }
                        else if (control.GetType() == typeof(CustomDataListRadioButtonControl))
                        {
                            CustomDataListRadioButtonControl radioButton = (CustomDataListRadioButtonControl)control;
                            radioButton.Margin = new Padding(2);
                            radioButton.AutoSize = false;
                            radioButton.Height = 30;
                            if (radioButton.RadioButtonCount > 2)
                            {
                                radioButton.Width = 335;
                            }
                            else
                            {
                                radioButton.Width = 120;
                                radioButton.AutoSize = true;
                            }
                            flowLayoutPanelForCustomData.Controls.Add(radioButton);
                        }
                        else
                        {
                            control.Margin = new Padding(2);
                            control.Anchor = AnchorStyles.None;
                            control.Width = 120;
                            control.Height = 30;
                            flowLayoutPanelForCustomData.Controls.Add(control);
                        }
                    }
                }
                if (flowLayoutPanelForCustomData.Controls.Count > 0)
                {
                    flpPersonalInfo.Controls.Add(flowLayoutPanelForCustomData);
                }
            }
            log.LogMethodExit();
        }

        private void SetTitleDataSource()
        {
            log.LogMethodEntry();
            List<KeyValuePair<string, string>> titleList = new List<KeyValuePair<string, string>>();
            titleList.Add(new KeyValuePair<string, string>("", MessageContainerList.GetMessage(utilities.ExecutionContext, "Select")));
            titleList.Add(new KeyValuePair<string, string>("Mr.", MessageContainerList.GetMessage(utilities.ExecutionContext, "Mr")));
            titleList.Add(new KeyValuePair<string, string>("Mrs.", MessageContainerList.GetMessage(utilities.ExecutionContext, "Mrs")));
            titleList.Add(new KeyValuePair<string, string>("Ms.", MessageContainerList.GetMessage(utilities.ExecutionContext, "Ms")));
            cmbTitle.ValueMember = "Key";
            cmbTitle.DisplayMember = "Value";
            cmbTitle.DataSource = titleList;
            log.LogMethodExit();
        }

        private void SetGenderDataSource()
        {
            log.LogMethodEntry();
            List<KeyValuePair<string, string>> genderList = new List<KeyValuePair<string, string>>();
            genderList.Add(new KeyValuePair<string, string>("M", MessageContainerList.GetMessage(utilities.ExecutionContext, "Male")));
            genderList.Add(new KeyValuePair<string, string>("F", MessageContainerList.GetMessage(utilities.ExecutionContext, "Female")));
            genderList.Add(new KeyValuePair<string, string>("N", MessageContainerList.GetMessage(utilities.ExecutionContext, "Not Set")));
            cmbGender.ValueMember = "Key";
            cmbGender.DisplayMember = "Value";
            cmbGender.DataSource = genderList;
            log.LogMethodExit();
        }

        private void SetCustomerTypeDataSource()
        {
            log.LogMethodEntry();
            List<KeyValuePair<CustomerType, string>> customerTypeList = new List<KeyValuePair<CustomerType, string>>
            {
                new KeyValuePair<CustomerType, string>(CustomerType.REGISTERED,
                    MessageContainerList.GetMessage(utilities.ExecutionContext, "Registered")),
                new KeyValuePair<CustomerType, string>(CustomerType.UNREGISTERED,
                    MessageContainerList.GetMessage(utilities.ExecutionContext, "Unregistered"))
            };
            cmbCustomerType.ValueMember = "Key";
            cmbCustomerType.DisplayMember = "Value";
            cmbCustomerType.DataSource = customerTypeList;
            log.LogMethodExit();
        }

        private string GetDateMonthFormat()
        {
            string dateFormat = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "DATE_FORMAT");
            if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(utilities.ExecutionContext, "IGNORE_CUSTOMER_BIRTH_YEAR"))
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

            return dateFormat;
        }

        /// <summary>
        /// Get method for the customer DTO
        /// </summary>
        public CustomerDTO CustomerDTO
        {
            get
            {
                return customerDTO;
            }
            set
            {
                try
                {
                    addressListView.ShowActiveAddressEntries = true;
                    contactListView.ShowActiveContactEntries = true;
                    customerDTO = value;
                    if (customerDTO == null)
                    {
                        customerDTO = new CustomerDTO();
                    }

                    ClearValdationErrors();
                    if (customerDTO.AddressDTOList == null)
                    {
                        customerDTO.AddressDTOList = new List<AddressDTO>();
                    }
                    else
                    {
                        customerDTO.AddressDTOList = new List<AddressDTO>(customerDTO.AddressDTOList);
                        if (customerDTO.AddressDTOList.FirstOrDefault((x) => x.IsActive == false) != null)
                        {
                           // addressListView.ShowActiveAddressEntries = false;
                        }
                    }

                    if (customerDTO.ContactDTOList == null)
                    {
                        customerDTO.ContactDTOList = new List<ContactDTO>();
                    }
                    else
                    {
                        customerDTO.ContactDTOList = new List<ContactDTO>(customerDTO.ContactDTOList);
                        if (customerDTO.ContactDTOList.FirstOrDefault((x) => x.IsActive == false) != null)
                        {
                            //  contactListView.ShowActiveContactEntries = false;
                        }
                    }

                    addressListView.AddressDTOList = customerDTO.AddressDTOList;
                    preventContactInfoEnteredEvent = true;
                    contactListView.ContactDTOList = customerDTO.ContactDTOList;
                    customDataTableLayoutPanel.SetCustomDataSetDTO(customerDTO.CustomDataSetDTO);
                    string datMonthFormat = GetDateMonthFormat();
                    if (customerDTO.DateOfBirth != null)
                    {
                        txtBirthDate.Text = customerDTO.DateOfBirth.Value.ToString(datMonthFormat);
                    }
                    else
                    {
                        txtBirthDate.Clear();
                    }

                    if (customerDTO.Anniversary != null)
                    {
                        txtAnniversaryDate.Text = customerDTO.Anniversary.Value.ToString(datMonthFormat);
                    }
                    else
                    {
                        txtAnniversaryDate.Clear();
                    }

                    if (string.IsNullOrWhiteSpace(customerDTO.PhotoURL) == false)
                    {
                        try
                        {
                            pbCustomerImage.Image =
                                (new CustomerBL(utilities.ExecutionContext, customerDTO)).GetCustomerImage();
                        }
                        catch (Exception ex)
                        {
                            log.Error("Error occurred while displaying customer image", ex);
                            pbCustomerImage.Image = Properties.Resources.camera_icon_normal;
                        }

                    }
                    else
                    {
                        pbCustomerImage.Image = Properties.Resources.camera_icon_normal;
                    }

                    txtFirstName.Text = customerDTO.FirstName;
                    txtMiddleName.Text = customerDTO.MiddleName;
                    txtLastName.Text = customerDTO.LastName;
                    cmbTitle.SelectedValue = customerDTO.Title == null ? "" : customerDTO.Title;
                    cmbGender.SelectedValue = customerDTO.Gender == null ? "" : customerDTO.Gender;
                    cmbCustomerType.SelectedValue = customerDTO.CustomerType;
                    txtUniqueIdentifier.Text = customerDTO.UniqueIdentifier;
                    txtTaxCode.Text = customerDTO.TaxCode;
                    txtCompany.Text = customerDTO.Company;
                    txtDesignation.Text = customerDTO.Designation;
                    chbTeamUser.Checked = customerDTO.TeamUser;
                    chbRightHanded.Checked = customerDTO.RightHanded;
                    txtUserName.Text = customerDTO.UserName;
                    txtExternalSystemReference.Text = customerDTO.ExternalSystemReference;
                    txtChannel.Text = customerDTO.Channel == null ? "" : customerDTO.Channel;
                    txtNotes.Text = customerDTO.Notes;
                    chbVerified.Checked = customerDTO.Verified;
                    cmbMembership.SelectedValue = customerDTO.MembershipId;
                    cchbPromotionOptin.Checked = customerDTO.OptInPromotions;
                    cchbWhatsAppOptOut.Checked = customerDTO.ProfileDTO.OptOutWhatsApp;
                    cmbPromotionMode.SelectedValue =
                        customerDTO.OptInPromotionsMode == null ? "" : customerDTO.OptInPromotionsMode;
                    cchbTermsandConditions.Checked = customerDTO.PolicyTermsAccepted;
                    if(virtualKeyboardController != null)
                    {
                        virtualKeyboardController.Refresh();
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while displaying customer record", ex);
                    throw;
                }
                finally
                {
                    preventContactInfoEnteredEvent = false;
                }
            }
        }

        private void UpdateCustomerDetailsUI()
        {
            log.LogMethodEntry();
            attributeControlDictionary.Add("DateOfBirth", txtBirthDate);
            attributeControlDictionary.Add("Gender", pnlGender);
            attributeControlDictionary.Add("Anniversary", txtAnniversaryDate);
            attributeControlDictionary.Add("Notes", txtNotes);
            attributeControlDictionary.Add("FirstName", txtFirstName);
            attributeControlDictionary.Add("MiddleName", txtMiddleName);
            attributeControlDictionary.Add("LastName", txtLastName);
            attributeControlDictionary.Add("Title", pnlTitle);
            attributeControlDictionary.Add("UniqueIdentifier", txtUniqueIdentifier);
            attributeControlDictionary.Add("TaxCode", txtTaxCode);
            attributeControlDictionary.Add("Company", txtCompany);
            attributeControlDictionary.Add("Designation", txtDesignation);
            attributeControlDictionary.Add("UserName", txtUserName);
            attributeControlDictionary.Add("MembershipId", cmbMembership);
            attributeControlDictionary.Add("ExternalSystemReference", txtExternalSystemReference);
            attributeControlDictionary.Add("Channel", txtChannel);
            attributeControlDictionary.Add("OptInPromotionsMode", pnlPromotionMode);
            attributeControlDictionary.Add("CustomerType", pnlCustomerType);
            if (ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "BIRTH_DATE") == "N")
            {
                flpBirthDate.Visible = false;
            }
            else
            {
                UpdateManadtoryLabel(lblBirthDate, "BIRTH_DATE");
            }
            if (ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "GENDER") == "N")
            {
                flpGender.Visible = false;
            }
            else
            {
                UpdateManadtoryLabel(lblGender, "GENDER");
            }
            if (ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "ANNIVERSARY") == "N")
            {
                flpAnniversary.Visible = false;
            }
            else
            {
                UpdateManadtoryLabel(lblAnniversaryDate, "ANNIVERSARY");
            }
            if (ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "NOTES") == "N")
            {
                flpNotes.Visible = false;
            }
            else
            {
                UpdateManadtoryLabel(lblNotes, "NOTES");
            }
            if (ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "COMPANY") == "N")
            {
                flpCompany.Visible = false;
            }
            else
            {
                UpdateManadtoryLabel(lblCompany, "COMPANY");
            }
            if (ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "DESIGNATION") == "N")
            {
                flpDesignation.Visible = false;
            }
            else
            {
                UpdateManadtoryLabel(lblDesignation, "DESIGNATION");
            }
            if (ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "CUSTOMERTYPE") == "N")
            {
                flpCustomerType.Visible = false;
            }
            else
            {
                UpdateManadtoryLabel(lblCustomerType, "CUSTOMERTYPE");
            }
            if (ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "UNIQUE_ID") == "N")
            {
                flpUniqueIdentifier.Visible = false;
            }
            else
            {
                UpdateManadtoryLabel(lblUniqueIdentifier, "UNIQUE_ID");
            }
            if (ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "USERNAME") == "N")
            {
                flpUserName.Visible = false;
            }
            else
            {
                UpdateManadtoryLabel(lblUsername, "USERNAME");
            }
            if (ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "RIGHTHANDED") == "N")
            {
                flpRightHanded.Visible = false;
            }
            else
            {
                UpdateManadtoryLabel(lblRightHanded, "RIGHTHANDED");
            }
            if (ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "TEAMUSER") == "N")
            {
                flpTeamUser.Visible = false;
            }
            else
            {
                UpdateManadtoryLabel(lblTeamUser, "TEAMUSER");
            }
            if (ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "VERIFIED") == "N")
            {
                flpVerified.Visible = false;
            }
            else
            {
                UpdateManadtoryLabel(lblVerified, "VERIFIED");
            }
            if (ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "OPT_IN_PROMOTIONS") == "N")
            {
                flpPromotionOptin.Visible = false;
            }
            else
            {
                UpdateManadtoryLabel(lblPromotionOptin, "OPT_IN_PROMOTIONS");
            }
            if (ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "OPT_IN_PROMOTIONS_MODE") == "N")
            {
                flpPromotionMode.Visible = false;
            }
            else
            {
                UpdateManadtoryLabel(lblPromotionMode, "OPT_IN_PROMOTIONS_MODE");
            }

            if (ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "TERMS_AND_CONDITIONS") == "N")
            {
                flpTermsandConditions.Visible = false;
            }
            else
            {
                UpdateManadtoryLabel(lblTermsandConditions, "TERMS_AND_CONDITIONS");
            }
            if (ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "EXTERNALSYSTEMREF") == "N")
            {
                flpExternalSystemReference.Visible = false;
            }
            else
            {
                UpdateManadtoryLabel(lblExternalSystemReference, "EXTERNALSYSTEMREF");
            }
            if (ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "LAST_NAME") == "N")
            {
                txtLastName.Visible = false;
            }
            else
            {
                UpdateManadtoryLabel(lblName, "LAST_NAME");
            }
            if (ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "TITLE") == "N")
            {
                flpTitle.Visible = false;
            }
            else
            {
                UpdateManadtoryLabel(lblTitle, "TITLE");
            }
            if (ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "CHANNEL") == "N")
            {
                flpChannel.Visible = false;
            }
            else
            {
                UpdateManadtoryLabel(lblChannel, "CHANNEL");
            }
            if (ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "TAXCODE") == "N")
            {
                flpTaxCode.Visible = false;
            }
            else
            {
                UpdateManadtoryLabel(lblTaxCode, "TAXCODE");
            }
            if (ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "CUSTOMER_PHOTO") == "N")
            {
                pbCustomerImage.Visible = false;
            }
            if (ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "CUSTOMER_NAME") == "N")
            {
                txtFirstName.Visible = false;
                lblName.Visible = false;
            }
            else
            {
                UpdateManadtoryLabel(lblName, "CUSTOMER_NAME");
            }
            if (ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "MIDDLE_NAME") == "N")
            {
                txtMiddleName.Visible = false;
            }
            else
            {
                UpdateManadtoryLabel(lblName, "MIDDLE_NAME");
            }
            if (ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "OPT_OUT_WHATSAPP_MESSAGE") == "N")
            {
                flpWhatsAppOptOut.Visible = false;
            }
            else
            {
                UpdateManadtoryLabel(lblWhatsAppOptOut, "OPT_OUT_WHATSAPP_MESSAGE");
            }
            int addressInvisibleColumnCount = 0;
            if (ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "CITY") == "N")
            {
                addressInvisibleColumnCount++;
            }
            if (ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "STATE") == "N")
            {
                addressInvisibleColumnCount++;
            }
            if (ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "COUNTRY") == "N")
            {
                addressInvisibleColumnCount++;
            }
            if (ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "PIN") == "N")
            {
                addressInvisibleColumnCount++;
            }
            if (ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "ADDRESS1") == "N")
            {
                addressInvisibleColumnCount++;
            }
            if (ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "ADDRESS2") == "N")
            {
                addressInvisibleColumnCount++;
            }
            if (ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "ADDRESS3") == "N")
            {
                addressInvisibleColumnCount++;
            }
            if (ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "ADDRESS_TYPE") == "N")
            {
                addressInvisibleColumnCount++;
            }

            if (addressInvisibleColumnCount == 8)
            {
                // grpAddress.Visible = false;
            }
            if (ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "CONTACT_PHONE") == "N" &&
                ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "EMAIL") == "N" &&
                ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "FBUSERID") == "N" &&
                ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "FBACCESSTOKEN") == "N" &&
                ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "TWACCESSTOKEN") == "N" &&
                ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "TWACCESSSECRET") == "N" &&
                ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "WECHAT_ACCESS_TOKEN") == "N")
            {
                //  grpContacts.Visible = false;
            }
            log.LogMethodExit();
        }

        private void SetCustomerMaxInputLenghts()
        {
            log.LogMethodEntry();
            txtFirstName.MaxLength = 50;
            txtMiddleName.MaxLength = 50;
            txtLastName.MaxLength = 50;
            txtUniqueIdentifier.MaxLength = 100;
            txtTaxCode.MaxLength = 100;
            txtCompany.MaxLength = 200;
            txtDesignation.MaxLength = 200;
            txtExternalSystemReference.MaxLength = 50;
            txtNotes.MaxLength = 200;
            txtUserName.MaxLength = 50;
            log.LogMethodExit();
        }


        /// <summary>
        /// Update Mandatory Label
        /// </summary>
        /// <param name="label"></param>
        /// <param name="defaultValue"></param>
        private void UpdateManadtoryLabel(Label label, string defaultValue)
        {
            log.LogMethodEntry(label, defaultValue);
            if (label != null &&
                label.Text != null &&
                label.Text.Contains("*") == false &&
                ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, defaultValue) == "M")
            {
                label.Text += "*";
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Refreshes the bound control
        /// </summary>
        public void RefreshBindings()
        {
            log.LogMethodEntry();
            CustomerDTO = customerDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Clears the validation errors
        /// </summary>
        public void ClearValdationErrors()
        {
            log.LogMethodEntry();
            foreach (var item in attributeControlDictionary)
            {
                if (item.Value is Panel)
                {
                    item.Value.BackColor = Color.SlateGray;
                }
                else
                {
                    item.Value.BackColor = Color.White;
                }
            }
            customDataTableLayoutPanel.ClearErrorState();
            contactListView.ClearValdationErrors();
            addressListView.ClearValdationErrors();
            log.LogMethodExit();
        }

        /// <summary>
        /// show validation errors
        /// </summary>
        /// <param name="validationErrorList"></param>
        public void ShowValidationError(List<ValidationError> validationErrorList)
        {
            log.LogMethodEntry(validationErrorList);
            Control c = GetControlByTag(this, validationErrorList[0].FieldName);
            if (c != null)
            {
                c.Focus();
            }
            foreach (var validationError in validationErrorList)
            {
                if (attributeControlDictionary.ContainsKey(validationError.FieldName))
                {
                    attributeControlDictionary[validationError.FieldName].BackColor = Color.OrangeRed;
                }
            }
            customDataTableLayoutPanel.HandleValidationErrors(validationErrorList);
            List<ValidationError> contactValidationErrors = validationErrorList.Where(x => x.EntityName == "Contact").ToList();
            contactListView.ShowValidationErrors(contactValidationErrors);
            List<ValidationError> addressValidationErrors = validationErrorList.Where(x => x.EntityName == "Address").ToList();
            addressListView.ShowValidationErrors(addressValidationErrors);
            log.LogMethodExit();
        }

        private Control GetControlByTag(Control parentControl, string tag)
        {
            log.LogMethodEntry(tag);
            Control c = null;
            if (parentControl.Tag != null && parentControl.Tag.ToString() == tag)
            {
                c = parentControl;
            }
            else
            {
                if (parentControl.Controls != null && parentControl.Controls.Count > 0)
                {
                    foreach (Control childControl in parentControl.Controls)
                    {
                        Control matchingControl = GetControlByTag(childControl, tag);
                        if (matchingControl != null)
                        {
                            c = matchingControl;
                            break;
                        }
                    }
                }
            }
            return c;
        }

        /// <summary>
        /// Updates customer dto with new values
        /// </summary>
        public List<ValidationError> UpdateCustomerDTO()
        {
            log.LogMethodEntry();

            List<ValidationError> validationErrorList = new List<ValidationError>();
            if (string.IsNullOrWhiteSpace(txtFirstName.Text) && ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "CUSTOMER_NAME") != "M")
            {
                txtFirstName.Text = "Default";
                customerDTO.FirstName = "Default";
            }
            if (customerDTO.FirstName != txtFirstName.Text)
            {
                customerDTO.FirstName = txtFirstName.Text;
            }
            if (customerDTO.MiddleName != txtMiddleName.Text)
            {
                customerDTO.MiddleName = txtMiddleName.Text;
            }
            if (customerDTO.LastName != txtLastName.Text)
            {
                customerDTO.LastName = txtLastName.Text;
            }
            if (customerDTO.Title != ((cmbTitle.SelectedValue != null) ? cmbTitle.SelectedValue.ToString() : ""))
            {
                customerDTO.Title = (cmbTitle.SelectedValue != null) ? cmbTitle.SelectedValue.ToString() : "";
            }
            if (customerDTO.Gender != ((cmbGender.SelectedValue != null) ? cmbGender.SelectedValue.ToString() : ""))
            {
                customerDTO.Gender = (cmbGender.SelectedValue != null) ? cmbGender.SelectedValue.ToString() : "";
            }

            customerDTO.CustomerType = (CustomerType)cmbCustomerType.SelectedValue;
            if (customerDTO.UniqueIdentifier != txtUniqueIdentifier.Text)
            {
                customerDTO.UniqueIdentifier = txtUniqueIdentifier.Text;
            }
            if (customerDTO.TaxCode != txtTaxCode.Text)
            {
                customerDTO.TaxCode = txtTaxCode.Text;
            }
            if (customerDTO.Company != txtCompany.Text)
            {
                customerDTO.Company = txtCompany.Text;
            }
            if (customerDTO.Designation != txtDesignation.Text)
            {
                customerDTO.Designation = txtDesignation.Text;
            }
            if (customerDTO.TeamUser != chbTeamUser.Checked)
            {
                customerDTO.TeamUser = chbTeamUser.Checked;
            }
            if (customerDTO.RightHanded != chbRightHanded.Checked)
            {
                customerDTO.RightHanded = chbRightHanded.Checked;
            }
            if (customerDTO.UserName != txtUserName.Text)
            {
                customerDTO.UserName = txtUserName.Text;
            }
            if (customerDTO.ExternalSystemReference != txtExternalSystemReference.Text)
            {
                customerDTO.ExternalSystemReference = txtExternalSystemReference.Text;
            }
            if (customerDTO.Channel != txtChannel.Text)
            {
                customerDTO.Channel = txtChannel.Text;
            }
            if (customerDTO.Notes != txtNotes.Text)
            {
                customerDTO.Notes = txtNotes.Text;
            }

            if (customerDTO.FirstName != null && customerDTO.FirstName != customerDTO.FirstName.Trim())
            {
                customerDTO.FirstName = customerDTO.FirstName.Trim();
            }
            if (customerDTO.MiddleName != null && customerDTO.MiddleName != customerDTO.MiddleName.Trim())
            {

                customerDTO.MiddleName = customerDTO.MiddleName.Trim();
            }
            if (customerDTO.LastName != null && customerDTO.LastName != customerDTO.LastName.Trim())
            {
                customerDTO.LastName = customerDTO.LastName.Trim();
            }
            if (customerDTO.OptInPromotions != cchbPromotionOptin.Checked)
            {
                customerDTO.OptInPromotions = cchbPromotionOptin.Checked;
            }
            if (customerDTO.OptInPromotionsMode != cmbPromotionMode.SelectedValue.ToString())
            {
                customerDTO.OptInPromotionsMode = cmbPromotionMode.SelectedValue.ToString();
            }
            if (customerDTO.ProfileDTO.OptOutWhatsApp != cchbWhatsAppOptOut.Checked)
            {
                customerDTO.ProfileDTO.OptOutWhatsApp = cchbWhatsAppOptOut.Checked;
            }

            string dateMonthformat = GetDateMonthFormat();
            if (!string.IsNullOrEmpty(txtBirthDate.Text.Replace(" ", "").Replace("-", "").Replace("/", "")))
            {
                try
                {
                    System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;
                    DateTime datofBirthValue;
                    if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(utilities.ExecutionContext, "IGNORE_CUSTOMER_BIRTH_YEAR"))
                    {
                        datofBirthValue = DateTime.ParseExact(txtBirthDate.Text + "/1904", dateMonthformat + "/yyyy", provider);
                    }
                    else
                    {
                        datofBirthValue = DateTime.ParseExact(txtBirthDate.Text, dateMonthformat, provider);
                    }
                    //DateTime datofBirthValue = DateTime.ParseExact(txtBirthDate.Text + "/1900", dateMonthformat + "/yyyy", provider);
                    if (datofBirthValue != customerDTO.DateOfBirth)
                    {
                        customerDTO.DateOfBirth = datofBirthValue;
                    }
                }
                catch
                {
                    try
                    {
                        System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.CurrentCulture;
                        DateTime datofBirthValue = Convert.ToDateTime(txtBirthDate.Text, provider);
                        if (datofBirthValue != customerDTO.DateOfBirth)
                        {
                            customerDTO.DateOfBirth = datofBirthValue;
                        }
                    }
                    catch
                    {
                        validationErrorList.Add(new ValidationError("Customer", "DateOfBirth", MessageContainerList.GetMessage(utilities.ExecutionContext, 449, dateMonthformat, dateMonthformat, Convert.ToDateTime("23-Feb-1982").ToString(dateMonthformat))));
                    }
                }
            }
            else
            {
                if (customerDTO.DateOfBirth != null)
                {
                    customerDTO.DateOfBirth = null;
                }
            }


            if (!string.IsNullOrEmpty(txtAnniversaryDate.Text.Replace(" ", "").Replace("-", "").Replace("/", "")))
            {
                try
                {
                    System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;
                    DateTime AnniversaryValue;
                    if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(utilities.ExecutionContext, "IGNORE_CUSTOMER_BIRTH_YEAR"))
                    {
                        AnniversaryValue = DateTime.ParseExact(txtBirthDate.Text + "/1904", dateMonthformat + "/yyyy", provider);
                    }
                    else
                    {
                        AnniversaryValue = DateTime.ParseExact(txtBirthDate.Text, dateMonthformat, provider);
                    }
                    // DateTime AnniversaryValue = DateTime.ParseExact(txtAnniversaryDate.Text + "/1900", dateMonthformat + "/yyyy", provider);
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
                        DateTime AnniversaryValue = Convert.ToDateTime(txtAnniversaryDate.Text, provider);
                        if (AnniversaryValue != customerDTO.Anniversary)
                        {
                            customerDTO.Anniversary = AnniversaryValue;
                        }
                    }
                    catch
                    {
                        validationErrorList.Add(new ValidationError("Customer", "Anniversary", MessageContainerList.GetMessage(utilities.ExecutionContext, 449, dateMonthformat, dateMonthformat, Convert.ToDateTime("23-Feb-1982").ToString(dateMonthformat))));
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
            customerDTO.AddressDTOList = addressListView.AddressDTOList;
            contactListView.UpdateContactDTOList();
            customerDTO.ContactDTOList = contactListView.ContactDTOList;
            customDataTableLayoutPanel.Save();
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        private void txtUniqueIdentifier_Validating(object sender, CancelEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (UniqueIdentifierValidating != null)
            {
                UniqueIdentifierValidating(sender, e);
            }
            log.LogMethodExit();
        }

        private void btnUploadIDProof_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            IdProofUI idProofUI = new IdProofUI(utilities, customerDTO.ProfileDTO, messageBoxDelegate);
            idProofUI.ShowDialog();
            log.LogMethodExit();
        }

        private void pbCustomerImage_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ProfilePhotoUI profilePhotoUI = new ProfilePhotoUI(utilities, customerDTO.ProfileDTO, messageBoxDelegate);
            if (profilePhotoUI.ShowDialog() == DialogResult.OK)
            {
                pbCustomerImage.Image = profilePhotoUI.SelectedImage;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the customer first name
        /// </summary>
        public string FirstName
        {
            get
            {
                string firstName = string.Empty;
                if (string.IsNullOrWhiteSpace(txtFirstName.Text) == false)
                {
                    firstName = txtFirstName.Text.Trim();
                }
                return firstName;
            }
        }

        /// <summary>
        /// Returns the customer middle name
        /// </summary>
        public string MiddleName
        {
            get
            {
                string middleName = string.Empty;
                if (string.IsNullOrWhiteSpace(txtMiddleName.Text) == false)
                {
                    middleName = txtMiddleName.Text.Trim();
                }
                return middleName;
            }
        }

        /// <summary>
        /// Returns the customer last name
        /// </summary>
        public string LastName
        {
            get
            {
                string lastName = string.Empty;
                if (string.IsNullOrWhiteSpace(txtLastName.Text) == false)
                {
                    lastName = txtLastName.Text.Trim();
                }
                return lastName;
            }
        }

        /// <summary>
        /// Returns the uniqueidentifier entered by the user
        /// </summary>
        public string UniqueIdentifier
        {
            get
            {
                string uniqueIdentifier = string.Empty;
                if (string.IsNullOrWhiteSpace(txtUniqueIdentifier.Text) == false)
                {
                    uniqueIdentifier = txtUniqueIdentifier.Text.Trim();
                }
                return uniqueIdentifier;
            }
        }

        /// <summary>
        /// Returns the email id entered by the customer
        /// </summary>
        public string Email
        {
            get
            {
                string email = string.Empty;
                // dgvContactDTOList.EndEdit();
                if (customerDTO != null)
                {
                    if (customerDTO.ContactDTOList.Count > 0)
                    {
                        foreach (var contactDTO in customerDTO.ContactDTOList)
                        {
                            if (contactDTO.ContactType == ContactType.EMAIL)
                            {
                                email = contactDTO.Attribute1;
                                break;
                            }
                        }
                    }
                }
                return email;
            }
        }

        /// <summary>
        /// Returns the phone entered by the customer
        /// </summary>
        public string PhoneNumber
        {
            get
            {
                string phoneNumber = string.Empty;
                //   dgvContactDTOList.EndEdit();
                if (customerDTO != null)
                {
                    if (customerDTO.ContactDTOList.Count > 0)
                    {
                        foreach (var contactDTO in customerDTO.ContactDTOList)
                        {
                            if (contactDTO.ContactType == ContactType.PHONE)
                            {
                                phoneNumber = contactDTO.Attribute1;
                                break;
                            }
                        }
                    }
                }
                return phoneNumber;
            }
        }

        private void txtFirstName_Leave(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (string.IsNullOrWhiteSpace(txtFirstName.Text) == false)
            {
                if (FirstNameLeave != null)
                {
                    FirstNameLeave(sender, e);
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Changes the background color of the customer detail UI
        /// </summary>
        /// <param name="backgroundColor">background color</param>
        public void SetBackGroundColor(Color backgroundColor)
        {
            log.LogMethodEntry(backgroundColor);
            this.BackColor = backgroundColor;
            panelCustomer.BackColor = backgroundColor;
            if (backgroundColor != Color.Transparent)
            {
                //  dgvContactDTOList.BackgroundColor = backgroundColor;
                //  dgvAddressDTOList.BackgroundColor = backgroundColor;
            }
            else
            {
                //  dgvContactDTOList.BackgroundColor = Color.White;
                // dgvAddressDTOList.BackgroundColor = Color.White;
            }
            chbRightHanded.SetBackGroundColor(backgroundColor);
            //  chbShowActiveAddressEntries.SetBackGroundColor(backgroundColor);
            //   chbShowActiveContactEntries.SetBackGroundColor(backgroundColor);
            chbTeamUser.SetBackGroundColor(backgroundColor);
            chbVerified.SetBackGroundColor(backgroundColor);
            //   grpAddress.BackColor = backgroundColor;
            //   grpContacts.BackColor = backgroundColor;
            addressListView.BackColor = backgroundColor;
            contactListView.BackColor = backgroundColor;
            pnlTitle.BackColor = backgroundColor;
            pnlGender.BackColor = backgroundColor;
            cchbPromotionOptin.SetBackGroundColor(backgroundColor);
            cchbWhatsAppOptOut.SetBackGroundColor(backgroundColor);
            log.LogMethodExit();
        }

        /// <summary>
        /// Changes whether the UI is readonly
        /// </summary>
        /// <param name="value"></param>
        public void SetControlsEnabled(bool value)
        {
            log.LogMethodEntry(value);
            txtFirstName.Enabled = value;
            txtMiddleName.Enabled = value;
            txtLastName.Enabled = value;
            cmbTitle.Enabled = value;
            cmbGender.Enabled = value;
            txtUniqueIdentifier.Enabled = value;
            txtTaxCode.Enabled = value;
            txtCompany.Enabled = value;
            txtDesignation.Enabled = value;
            chbTeamUser.Enabled = value;
            chbRightHanded.Enabled = value;
            txtUserName.Enabled = value;
            txtExternalSystemReference.Enabled = value;
            txtChannel.Enabled = value;
            txtNotes.Enabled = value;
            txtBirthDate.Enabled = value;
            txtBirthDate.Enabled = value;
            txtAnniversaryDate.Enabled = value;
            customDataTableLayoutPanel.SetControlsEnabled(value);
            pbCustomerImage.Enabled = value;
            btnUploadIDProof.Enabled = value;
            btnShowKeyPad.Enabled = value;
            cchbPromotionOptin.Enabled = value;
            addressListView.SetControlsEnabled(value);
            contactListView.SetControlsEnabled(value);
            cchbWhatsAppOptOut.Enabled = value;
            log.LogMethodExit();
        }



        private int GetCountryIdForState(int stateId)
        {
            int countryId = -1;
            log.LogMethodEntry(stateId);
            if (stateDTOList != null)
            {
                foreach (var stateDTO in stateDTOList)
                {
                    if (stateDTO.StateId == stateId)
                    {
                        countryId = stateDTO.CountryId;
                    }
                }
            }
            log.LogMethodExit(countryId);
            return countryId;

        }


        private List<StateDTO> GetStateSourceByCountryId(int countryId)
        {
            log.LogMethodEntry(countryId);
            List<StateDTO> stateDTOByCountryIdList = new List<StateDTO>();
            if (countryId >= 0)
            {
                stateDTOByCountryIdList.Add(new StateDTO());
                stateDTOByCountryIdList[0].StateId = -1;
                stateDTOByCountryIdList[0].Description = "SELECT";
                if (stateDTOList != null)
                {
                    foreach (var stateDTO in stateDTOList)
                    {
                        if (stateDTO.CountryId == countryId)
                        {
                            stateDTOByCountryIdList.Add(stateDTO);
                        }
                    }
                }
            }
            else
            {
                stateDTOByCountryIdList = this.stateDTOList;
            }
            return stateDTOByCountryIdList;
        }

        private void RefreshAddressDTOList()
        {
            log.LogMethodEntry();
            if (customerDTO.ProfileId > -1)
            {
                List<AddressDTO> addressDTOList = null;
                AddressListBL addressListBL = new AddressListBL(utilities.ExecutionContext);
                List<KeyValuePair<AddressDTO.SearchByParameters, string>> searchByParameters = new List<KeyValuePair<AddressDTO.SearchByParameters, string>>();
                searchByParameters.Add(new KeyValuePair<AddressDTO.SearchByParameters, string>(AddressDTO.SearchByParameters.PROFILE_ID, customerDTO.ProfileId.ToString()));
                if (addressListView.ShowActiveAddressEntries)
                {
                    searchByParameters.Add(new KeyValuePair<AddressDTO.SearchByParameters, string>(AddressDTO.SearchByParameters.IS_ACTIVE, "1"));
                }
                addressDTOList = addressListBL.GetAddressDTOList(searchByParameters);
                if (addressDTOList == null)
                {
                    addressDTOList = new List<AddressDTO>();
                }
                customerDTO.AddressDTOList = addressDTOList;
                // This is to keep the inactive records from the UI. 
                foreach(AddressDTO addressDTO in addressListView.AddressDTOList)
                {
                    if(addressDTO.IsActive == false)
                    {
                        AddressDTO inactiveAddressDTO = customerDTO.AddressDTOList.Where(x => x.Id == addressDTO.Id).FirstOrDefault();
                        inactiveAddressDTO.IsActive = false;
                    }
                }
                addressListView.AddressDTOList = customerDTO.AddressDTOList;
            }
            log.LogMethodExit();
        }

        private void chbShowActiveAddressEntries_CheckedChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            RefreshAddressDTOList();
            log.LogMethodExit();
        }

        private void RefreshContactDTOList()
        {
            log.LogMethodEntry();
            if (customerDTO.ProfileId > -1)
            {
                List<ContactDTO> contactDTOList = null;
                ContactListBL contactListBL = new ContactListBL(utilities.ExecutionContext);
                List<KeyValuePair<ContactDTO.SearchByParameters, string>> searchByParameters = new List<KeyValuePair<ContactDTO.SearchByParameters, string>>();
                searchByParameters.Add(new KeyValuePair<ContactDTO.SearchByParameters, string>(ContactDTO.SearchByParameters.PROFILE_ID, customerDTO.ProfileId.ToString()));
                if (contactListView.ShowActiveContactEntries)
                {
                    searchByParameters.Add(new KeyValuePair<ContactDTO.SearchByParameters, string>(ContactDTO.SearchByParameters.IS_ACTIVE, "1"));
                }
                contactDTOList = contactListBL.GetContactDTOList(searchByParameters);
                if (contactDTOList == null)
                {
                    contactDTOList = new List<ContactDTO>();
                }
                customerDTO.ContactDTOList = contactDTOList;
                contactListView.ContactDTOList = customerDTO.ContactDTOList;
            }
            log.LogMethodExit();
        }

        private void chbShowActiveContactEntries_CheckedChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            RefreshContactDTOList();
            log.LogMethodExit();
        }



        private void contactDTOListBS_AddingNew(object sender, AddingNewEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ContactDTO contactDTO = new ContactDTO();
            if (ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "CONTACT_PHONE") != "N")
            {
                contactDTO.ContactType = ContactType.PHONE;
            }
            else if (ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "EMAIL") != "N")
            {
                contactDTO.ContactType = ContactType.EMAIL;
            }
            e.NewObject = contactDTO;
            log.LogMethodExit();
        }

        private void cchbPromotionOptin_CheckedChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (!cchbPromotionOptin.Checked)
            {
                cmbPromotionMode.SelectedValue = string.Empty;
            }
            cmbPromotionMode.Enabled = cchbPromotionOptin.Checked;
            log.LogMethodExit();
        }

        private void cchbWhatsAppOptin_CheckedChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (!cchbWhatsAppOptOut.Checked)
            {
                cmbPromotionMode.SelectedValue = string.Empty;
            }
            cmbPromotionMode.Enabled = cchbWhatsAppOptOut.Checked;
            log.LogMethodExit();
        }

        private void cchbTermsandConditions_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (cchbTermsandConditions.Checked)
            {
                RichContentDTO richContentDTO;
                CustomerTermsandConditionsUI customerTermsandConditionsUI = new CustomerTermsandConditionsUI(utilities, "POS");
                if (customerTermsandConditionsUI.ShowDialog() == DialogResult.OK && customerTermsandConditionsUI.Tag != null)
                {
                    richContentDTO = (RichContentDTO)customerTermsandConditionsUI.Tag;
                    cchbTermsandConditions.Checked = true;
                    if (customerDTO != null && customerDTO.ProfileDTO != null)
                    {
                        cchbTermsandConditions.Checked = true;
                        customerDTO.ProfileDTO.PolicyTermsAccepted = true;
                        if (customerDTO.ProfileDTO.ProfileContentHistoryDTOList == null)
                        {
                            customerDTO.ProfileDTO.ProfileContentHistoryDTOList = new List<ProfileContentHistoryDTO>();
                        }
                        if (!customerDTO.ProfileDTO.ProfileContentHistoryDTOList.Exists(x => (bool)(x.RichContentId == richContentDTO.Id)))
                        {
                            DateTime dateTime = utilities.getServerTime();
                            ProfileContentHistoryDTO profileContentHistoryDTO
                                = new ProfileContentHistoryDTO(-1, customerDTO.ProfileDTO.Id, richContentDTO.Id, dateTime, true, utilities.ExecutionContext.GetUserId(),
                                dateTime, utilities.ExecutionContext.GetUserId(), dateTime,
                                utilities.ExecutionContext.GetSiteId(), false, null, -1);
                            profileContentHistoryDTO.IsChanged = true;
                            customerDTO.ProfileDTO.ProfileContentHistoryDTOList.Add(profileContentHistoryDTO);
                        }
                    }
                }
                else
                {
                    cchbTermsandConditions.Checked = false;
                }
            }
            else
            {
                cchbTermsandConditions.Checked = true;
                log.LogMethodExit();
                return;
            }
            log.LogMethodExit();
        }


        private void CustomerContactInfo_OnEnter(object sender, CustomerContactInfoEnteredEventArgs e)
        {
            CustomerContactInfoEnteredEventArgs customerContactInfoEnteredEventArgs = new CustomerContactInfoEnteredEventArgs(e.ContactType, e.ContactValue);
            if (CustomerContactInfoEntered != null && preventContactInfoEnteredEvent == false)
            {
                CustomerContactInfoEntered(this, customerContactInfoEnteredEventArgs);
            }
            //preventContactInfoEnteredEvent = true;
        }

        private void btnAssociatedCards_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if(customerDTO != null && !string.IsNullOrEmpty(customerDTO.CardNumber))
            {
                using (CustomerAssociatedCardsUI customerLinkedCardsUI = new CustomerAssociatedCardsUI(utilities))
                {
                    customerLinkedCardsUI.CustomerDTO = customerDTO;
                    customerLinkedCardsUI.ShowDialog();
                }
            }
            log.LogMethodExit();
        }
    }
}
