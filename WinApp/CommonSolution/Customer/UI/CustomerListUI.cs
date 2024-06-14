/********************************************************************************************
 * Project Name - CustomerListUI
 * Description  - UI for CustomerList display
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.70.2       01-Sept-2019      Girish kundar    Modified : UI Changes for Customer.                                                  
 *2.70.2       05-Dec-2019      Girish kundar    Modified :  Added clear button to clear the search values.
 *2.70.3       14-Feb-2020     Lakshminarayana      Modified: Creating unregistered customer during check-in process
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.CardCore;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.Customer.Membership;
using Semnox.Parafait.Customer.UI;
using Semnox.Parafait.GenericUtilities;
using Semnox.Parafait.Site;

namespace Semnox.Parafait.Customer
{
    /// <summary>
    /// Customer UI class
    /// </summary>
    public partial class CustomerListUI : Form
    {
        /// <summary>
        /// Parafait utility 
        /// </summary>
        protected Utilities utilities;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int totalNoOfPages = 0;
        private int currentPage = 0;
        private int pageSize = 20;
        private List<StateDTO> stateDTOList;
        private CustomerSearchCriteria customerAdvancedSearchSearchCriteria;
        private VirtualKeyboardController virtualKeyboardController;
        private List<CustomAttributesDTO> customAttributesDTOList;
        protected string cardNumber = string.Empty;
        private const string EXACT_SEARCH = "E";
        private const string LIKE_SEARCH = "L";
        public string CardNumber
        {
            get { return cardNumber; }
            set { cardNumber = value; }
        }


        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="utilities"></param>
        public CustomerListUI(Utilities utilities)
        {
            log.LogMethodEntry(utilities);
            this.utilities = utilities;
            InitializeComponent();
            utilities.setupDataGridProperties(ref dgvAddressDTOList);
            utilities.setupDataGridProperties(ref dgvContactDTOList);
            utilities.setupDataGridProperties(ref dgvCustomerDTOList);
            utilities.setLanguage(this);
            selectDataGridViewButtonColumn.Visible = false;
            txtFirstName.Cue = MessageContainerList.GetMessage(utilities.ExecutionContext, "First Name");
            txtMiddleName.Cue = MessageContainerList.GetMessage(utilities.ExecutionContext, "Middle Name");
            txtLastName.Cue = MessageContainerList.GetMessage(utilities.ExecutionContext, "Last Name");
            txtPhone.Cue = MessageContainerList.GetMessage(utilities.ExecutionContext, "Phone Number");
            txtEmail.Cue = MessageContainerList.GetMessage(utilities.ExecutionContext, "Email Id");
            customDataSetDataGridViewButtonColumn.Text = "...";
            customDataSetDataGridViewButtonColumn.UseColumnTextForButtonValue = true;
            customDataSetDataGridViewButtonColumn.Width = 30;
            selectDataGridViewButtonColumn.Text = "...";
            selectDataGridViewButtonColumn.UseColumnTextForButtonValue = true;
            selectDataGridViewButtonColumn.Width = 30;

            dateOfBirthDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewDateCellStyle();
            anniversaryDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewDateCellStyle();
            lastUpdateDateDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewDateTimeCellStyle();
            log.LogMethodExit();
        }



        private async void CustomerListUI_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            UpdateDataGridViewColumnVisibility();
            lblPage.Text = "";
            SetCustomerMaxInputLenghts();
            SetAddressMaxInputLenghts();
            SetContactMaxInputLenghts();
            SetTitleDataSource();
            SetGenderDataSource();
            SetCustomerTypeDataSource();
            OnDataLoadStart();
            await RefreshData();
            OnDataLoadComplete();
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
            titleDataGridViewComboBoxColumn.ValueMember = "Key";
            titleDataGridViewComboBoxColumn.DisplayMember = "Value";
            titleDataGridViewComboBoxColumn.DataSource = titleList;
            log.LogMethodExit();
        }

        private void SetGenderDataSource()
        {
            log.LogMethodEntry();
            List<KeyValuePair<string, string>> genderList = new List<KeyValuePair<string, string>>();
            genderList.Add(new KeyValuePair<string, string>("M", MessageContainerList.GetMessage(utilities.ExecutionContext, "Male")));
            genderList.Add(new KeyValuePair<string, string>("F", MessageContainerList.GetMessage(utilities.ExecutionContext, "Female")));
            genderList.Add(new KeyValuePair<string, string>("N", MessageContainerList.GetMessage(utilities.ExecutionContext, "Not Set")));
            genderDataGridViewComboBoxColumn.ValueMember = "Key";
            genderDataGridViewComboBoxColumn.DisplayMember = "Value";
            genderDataGridViewComboBoxColumn.DataSource = genderList;
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
            customerTypeDataGridViewComboBoxColumn.ValueMember = "Key";
            customerTypeDataGridViewComboBoxColumn.DisplayMember = "Value";
            customerTypeDataGridViewComboBoxColumn.DataSource = customerTypeList;
            log.LogMethodExit();
        }

        private void SetCustomerMaxInputLenghts()
        {
            log.LogMethodEntry();
            firstNameDataGridViewTextBoxColumn.MaxInputLength = 50;
            middleNameDataGridViewTextBoxColumn.MaxInputLength = 50;
            lastNameDataGridViewTextBoxColumn.MaxInputLength = 50;
            uniqueIdentifierDataGridViewTextBoxColumn.MaxInputLength = 100;
            taxCodeDataGridViewTextBoxColumn.MaxInputLength = 100;
            companyDataGridViewTextBoxColumn.MaxInputLength = 200;
            designationDataGridViewTextBoxColumn.MaxInputLength = 200;
            externalSystemReferenceDataGridViewTextBoxColumn.MaxInputLength = 50;
            notesDataGridViewTextBoxColumn.MaxInputLength = 200;
            userNameDataGridViewTextBoxColumn.MaxInputLength = 50;
            log.LogMethodExit();
        }

        private void SetAddressMaxInputLenghts()
        {
            log.LogMethodEntry();
            line1DataGridViewTextBoxColumn.MaxInputLength = 50;
            line2DataGridViewTextBoxColumn.MaxInputLength = 50;
            line3DataGridViewTextBoxColumn.MaxInputLength = 50;
            cityDataGridViewTextBoxColumn.MaxInputLength = 50;
            postalCodeDataGridViewTextBoxColumn.MaxInputLength = 100;
            log.LogMethodExit();
        }
        private void SetContactMaxInputLenghts()
        {
            log.LogMethodEntry();
            attribute1DataGridViewTextBoxColumn.MaxInputLength = 200;
            attribute2DataGridViewTextBoxColumn.MaxInputLength = 200;
            log.LogMethodExit();
        }


        #region Column visibility based on parafait defaults
        private void UpdateDataGridViewColumnVisibility()
        {
            log.LogMethodEntry();
            if (Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "BIRTH_DATE") == "N")
            {
                dateOfBirthDataGridViewTextBoxColumn.Visible = false;
            }
            if (Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "GENDER") == "N")
            {
                genderDataGridViewComboBoxColumn.Visible = false;
            }
            if (Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "ANNIVERSARY") == "N")
            {
                anniversaryDataGridViewTextBoxColumn.Visible = false;
            }
            if (Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "NOTES") == "N")
            {
                notesDataGridViewTextBoxColumn.Visible = false;
            }
            if (Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "COMPANY") == "N")
            {
                companyDataGridViewTextBoxColumn.Visible = false;
            }
            if (Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "DESIGNATION") == "N")
            {
                designationDataGridViewTextBoxColumn.Visible = false;
            }
            if (Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "UNIQUE_ID") == "N")
            {
                uniqueIdentifierDataGridViewTextBoxColumn.Visible = false;
            }
            if (Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "USERNAME") == "N")
            {
                userNameDataGridViewTextBoxColumn.Visible = false;
            }
            if (Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "RIGHTHANDED") == "N")
            {
                rightHandedDataGridViewCheckBoxColumn.Visible = false;
            }
            if (Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "TEAMUSER") == "N")
            {
                teamUserDataGridViewCheckBoxColumn.Visible = false;
            }
            if (Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "VERIFIED") == "N")
            {
                verifiedDataGridViewCheckBoxColumn.Visible = false;
            }
            if (Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "EXTERNALSYSTEMREF") == "N")
            {
                externalSystemReferenceDataGridViewTextBoxColumn.Visible = false;
            }
            if (Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "LAST_NAME") == "N")
            {
                lastNameDataGridViewTextBoxColumn.Visible = false;
            }
            if (Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "TITLE") == "N")
            {
                titleDataGridViewComboBoxColumn.Visible = false;
            }
            if (Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "CHANNEL") == "N")
            {
                channelDataGridViewTextBoxColumn.Visible = false;
            }
            if (Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "TAXCODE") == "N")
            {
                taxCodeDataGridViewTextBoxColumn.Visible = false;
            }
            if (Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "CUSTOMER_NAME") == "N")
            {
                firstNameDataGridViewTextBoxColumn.Visible = false;
            }
            if (Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "MIDDLE_NAME") == "N")
            {
                middleNameDataGridViewTextBoxColumn.Visible = false;
            }
            int addressInvisibleColumnCount = 0;
            if (Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "CITY") == "N")
            {
                addressInvisibleColumnCount++;
                cityDataGridViewTextBoxColumn.Visible = false;
            }
            if (Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "STATE") == "N")
            {
                addressInvisibleColumnCount++;
                stateIdDataGridViewComboBoxColumn.Visible = false;
            }
            if (Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "COUNTRY") == "N")
            {
                addressInvisibleColumnCount++;
                countryIdDataGridViewComboBoxColumn.Visible = false;
            }
            if (Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "PIN") == "N")
            {
                addressInvisibleColumnCount++;
                postalCodeDataGridViewTextBoxColumn.Visible = false;
            }
            if (Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "ADDRESS1") == "N")
            {
                addressInvisibleColumnCount++;
                line1DataGridViewTextBoxColumn.Visible = false;
            }
            if (Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "ADDRESS2") == "N")
            {
                addressInvisibleColumnCount++;
                line2DataGridViewTextBoxColumn.Visible = false;
            }
            if (Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "ADDRESS3") == "N")
            {
                addressInvisibleColumnCount++;
                line3DataGridViewTextBoxColumn.Visible = false;
            }
            if (Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "ADDRESS_TYPE") == "N")
            {
                addressInvisibleColumnCount++;
                addressTypeIdDataGridViewComboBoxColumn.Visible = false;
            }
            if (addressInvisibleColumnCount == 8)
            {
                grpAddress.Visible = false;
                tlpAddressAndContact.ColumnStyles[0] = new ColumnStyle(SizeType.Absolute, 0f);
            }
            if (Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "CONTACT_PHONE") == "N" &&
                Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "EMAIL") == "N" &&
                Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "FBUSERID") == "N" &&
                Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "FBACCESSTOKEN") == "N" &&
                Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "TWACCESSTOKEN") == "N" &&
                Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "TWACCESSSECRET") == "N" &&
                Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "WECHAT_ACCESS_TOKEN") == "N")
            {
                grpContact.Visible = false;
                tlpAddressAndContact.ColumnStyles[1] = new ColumnStyle(SizeType.Absolute, 0f);
            }
            if (grpAddress.Visible == false && grpContact.Visible == false)
            {
                tlpMain.RowStyles[1] = new RowStyle(SizeType.Absolute, 0f);
            }
            log.LogMethodExit();
        }
        #endregion

        #region OnDataLoadStart and OnDataLoadComplete
        private void OnDataLoadStart()
        {
            log.LogMethodEntry();
            DisableControls();
            lblStatus.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 1448);
            log.LogMethodExit();
        }

        private void OnDataLoadComplete()
        {
            log.LogMethodEntry();
            lblStatus.Text = "";
            EnableControls();
            DisplayCurrentPageCount();
            UpdatePagingControls();
            log.LogMethodExit();
        }

        private void UpdatePagingControls()
        {
            log.LogMethodEntry();
            if (totalNoOfPages == 0)
            {
                btnFirst.Enabled = false;
                btnLast.Enabled = false;
                btnNext.Enabled = false;
                btnPrevious.Enabled = false;
            }
            else
            {
                if (currentPage == 0)
                {
                    btnPrevious.Enabled = false;
                    btnFirst.Enabled = false;
                }
                else
                {
                    btnPrevious.Enabled = true;
                    btnFirst.Enabled = true;
                }
                if (currentPage == (totalNoOfPages - 1))
                {
                    btnNext.Enabled = false;
                    btnLast.Enabled = false;
                }
                else
                {
                    btnNext.Enabled = true;
                    btnLast.Enabled = true;
                }
            }
            log.LogMethodExit();
        }
        #endregion

        #region Enable and Disable controls
        private void DisableControls()
        {
            log.LogMethodEntry();
            btnClose.Enabled = false;
            btnUpdateMembership.Enabled = false;
            btnExport.Enabled = false;
            btnImport.Enabled = false;
            btnRefresh.Enabled = false;
            btnRelationship.Enabled = false;
            btnNext.Enabled = false;
            btnPrevious.Enabled = false;
            btnFirst.Enabled = false;
            btnLast.Enabled = false;
            btnSearch.Enabled = false;
            btnAdvancedSearch.Enabled = false;
            btnSave.Enabled = false;
            txtFirstName.Enabled = false;
            txtMiddleName.Enabled = false;
            txtLastName.Enabled = false;
            cmbSite.Enabled = false;
            txtEmail.Enabled = false;
            txtUniqueIdentifier.Enabled = false;
            txtPhone.Enabled = false;
            cmbMembership.Enabled = false;
            btnAssociatedCards.Enabled = false;
            btnPhoneSearchOption.Enabled = false;
            btnEmailSearchOption.Enabled = false;
            RefreshControls();
            log.LogMethodExit();
        }

        private void RefreshControls()
        {
            log.LogMethodEntry();
            btnClose.Refresh();
            btnUpdateMembership.Refresh();
            btnExport.Refresh();
            btnImport.Refresh();
            btnRefresh.Refresh();
            btnRelationship.Refresh();
            btnNext.Refresh();
            btnPrevious.Refresh();
            btnFirst.Refresh();
            btnLast.Refresh();
            btnSearch.Refresh();
            btnAdvancedSearch.Refresh();
            btnSave.Refresh();
            txtFirstName.Refresh();
            txtMiddleName.Refresh();
            txtLastName.Refresh();
            cmbSite.Refresh();
            txtEmail.Refresh();
            txtUniqueIdentifier.Refresh();
            txtPhone.Refresh();
            cmbMembership.Refresh();
            lblStatus.Refresh();
            btnAssociatedCards.Refresh();
            btnPhoneSearchOption.Refresh();
            btnEmailSearchOption.Refresh();
            log.LogMethodExit();
        }

        private void EnableControls()
        {
            log.LogMethodEntry();
            btnClose.Enabled = true;
            btnUpdateMembership.Enabled = true;
            btnExport.Enabled = true;
            btnImport.Enabled = true;
            btnRefresh.Enabled = true;
            btnRelationship.Enabled = true;
            btnNext.Enabled = true;
            btnPrevious.Enabled = true;
            btnFirst.Enabled = true;
            btnLast.Enabled = true;
            btnSearch.Enabled = true;
            btnAdvancedSearch.Enabled = true;
            btnSave.Enabled = true;
            txtFirstName.Enabled = true;
            txtMiddleName.Enabled = true;
            txtLastName.Enabled = true;
            cmbSite.Enabled = true;
            txtEmail.Enabled = true;
            txtUniqueIdentifier.Enabled = true;
            txtPhone.Enabled = true;
            cmbMembership.Enabled = true;
            btnAssociatedCards.Enabled = true;
            btnPhoneSearchOption.Enabled = true;
            btnEmailSearchOption.Enabled = true;
            RefreshControls();
            log.LogMethodExit();
        }
        #endregion


        private async Task RefreshData()
        {
            log.LogMethodEntry();
            addressTypeIdDataGridViewComboBoxColumn.DisplayMember = "Description";
            addressTypeIdDataGridViewComboBoxColumn.ValueMember = "AddressType";
            addressTypeIdDataGridViewComboBoxColumn.DataSource = await Task<List<AddressTypeDTO>>.Factory.StartNew(() => { return GetAddressTypeDTOList(); });
            contactTypeIdDataGridViewComboBoxColumn.DisplayMember = "Description";
            contactTypeIdDataGridViewComboBoxColumn.ValueMember = "ContactType";
            contactTypeIdDataGridViewComboBoxColumn.DataSource = await Task<List<ContactTypeDTO>>.Factory.StartNew(() => { return GetContactTypeDTOList(); });
            countryIdDataGridViewComboBoxColumn.DisplayMember = "CountryName";
            countryIdDataGridViewComboBoxColumn.ValueMember = "CountryId";
            countryIdDataGridViewComboBoxColumn.DataSource = await Task<List<CountryDTO>>.Factory.StartNew(() => { return GetCountryDTOList(); });
            stateIdDataGridViewComboBoxColumn.DisplayMember = "Description";
            stateIdDataGridViewComboBoxColumn.ValueMember = "StateId";
            stateDTOList = await Task<List<StateDTO>>.Factory.StartNew(() => { return GetStateDTOList(); });
            stateIdDataGridViewComboBoxColumn.DataSource = stateDTOList;
            cmbSite.DisplayMember = "SiteName";
            cmbSite.ValueMember = "SiteId";
            cmbSite.DataSource = await Task<List<SiteDTO>>.Factory.StartNew(() => { return GetSiteDTOList(); });
            cmbMembership.DisplayMember = "MembershipName";
            cmbMembership.ValueMember = "MembershipID";
            List<MembershipDTO> membershipDTOList = await Task<List<MembershipDTO>>.Factory.StartNew(() => { return GetMembershipDTOList(); });
            BindingSource bs = new BindingSource();
            bs.DataSource = membershipDTOList;
            cmbMembership.DataSource = bs;
            membershipIdDataGridViewComboBoxColumn.DisplayMember = "MembershipName";
            membershipIdDataGridViewComboBoxColumn.ValueMember = "MembershipID";
            bs = new BindingSource();
            bs.DataSource = membershipDTOList;
            membershipIdDataGridViewComboBoxColumn.DataSource = bs;
            await RefreshCustomerDTOList();
            log.LogMethodExit();
        }

        private async Task RefreshCustomerDTOList()
        {
            log.LogMethodEntry();
            currentPage = 0;
            CustomerSearchCriteria customerSearchCriteria = GetCustomerSearchCriteria();
            int totalNoOfCustomer = await Task<int>.Factory.StartNew(() => { return GetCustomerCount(customerSearchCriteria); });
            log.LogVariableState("totalNoOfCustomer", totalNoOfCustomer);
            totalNoOfPages = (totalNoOfCustomer / pageSize) + ((totalNoOfCustomer % pageSize) > 0 ? 1 : 0);
            log.LogVariableState("totalNoOfPages", totalNoOfPages);
            await LoadCustomerDTOList();
            log.LogMethodExit();
        }

        private async Task LoadCustomerDTOList()
        {
            log.LogMethodEntry();
            CustomerSearchCriteria customerSearchCriteria = GetCustomerSearchCriteria();
            IList<CustomerDTO> customerDTOList = null;
            if (totalNoOfPages > 0)
            {
                customerSearchCriteria.OrderBy(CustomerSearchByParameters.CUSTOMER_ID);
                customerSearchCriteria.Paginate(currentPage, pageSize);
                customerDTOList = await Task<List<CustomerDTO>>.Factory.StartNew(() => { return GetCustomerDTOList(customerSearchCriteria); });
            }
            //if(customerId >=0)
            //{
            //    customerId = -1;
            //}
            if (customerDTOList == null)
            {
                customerDTOList = new SortableBindingList<CustomerDTO>();
            }
            else
            {
                customerDTOList = new SortableBindingList<CustomerDTO>(customerDTOList);
            }
            foreach (var customerDTO in customerDTOList)
            {
                if (customerDTO.AddressDTOList != null)
                {
                    customerDTO.AddressDTOList = new List<AddressDTO>(customerDTO.AddressDTOList);
                }
                else
                {
                    customerDTO.AddressDTOList = new List<AddressDTO>();
                }
                if (customerDTO.ContactDTOList != null)
                {
                    customerDTO.ContactDTOList = new List<ContactDTO>(customerDTO.ContactDTOList);
                }
                else
                {
                    customerDTO.ContactDTOList = new List<ContactDTO>();
                }
            }
            customerDTOListBS.DataSource = customerDTOList;
            log.LogMethodExit();
        }

        private List<AddressTypeDTO> GetAddressTypeDTOList()
        {
            log.LogMethodEntry();
            List<AddressTypeDTO> addressTypeDTOList = null;
            try
            {
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
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while loading address type", ex);
            }
            log.LogMethodExit(addressTypeDTOList);
            return addressTypeDTOList;
        }

        private List<SiteDTO> GetSiteDTOList()
        {
            log.LogMethodEntry();
            List<SiteDTO> siteDTOList = null;
            try
            {
                SiteList siteList = new SiteList(this.utilities.ExecutionContext);
                List<KeyValuePair<SiteDTO.SearchBySiteParameters, string>> searchSiteParams = new List<KeyValuePair<SiteDTO.SearchBySiteParameters, string>>();
                siteDTOList = siteList.GetAllSites(searchSiteParams);
                if (siteDTOList == null)
                {
                    siteDTOList = new List<SiteDTO>();
                }
                siteDTOList.Insert(0, new SiteDTO());
                siteDTOList[0].SiteName = "SELECT";
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while loading sites", ex);
            }
            log.LogMethodExit(siteDTOList);
            return siteDTOList;
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

        private List<ContactTypeDTO> GetContactTypeDTOList()
        {
            log.LogMethodEntry();
            List<ContactTypeDTO> contactTypeDTOList = null;
            try
            {
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
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while loading contact types", ex);
            }

            log.LogMethodExit(contactTypeDTOList);
            return contactTypeDTOList;
        }

        private List<CountryDTO> GetCountryDTOList()
        {
            log.LogMethodEntry();
            CountryDTOList countryDTOList = new CountryDTOList(utilities.ExecutionContext);
            List<CountryDTO> countryList = null;
            try
            {
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
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while loading country list", ex);
            }
            log.LogMethodExit(countryList);
            return countryList;
        }

        private List<StateDTO> GetStateDTOList()
        {
            log.LogMethodEntry();
            List<StateDTO> stateList = null;
            try
            {
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
                stateList[0].Description = "SELECT";
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while loading state list", ex);
            }
            log.LogMethodExit(stateList);
            return stateList;
        }

        private List<CustomerDTO> GetCustomerDTOList(CustomerSearchCriteria customerSearchCriteria)
        {
            log.LogMethodEntry(customerSearchCriteria);
            List<CustomerDTO> customerDTOList = null;
            try
            {
                CustomerListBL customerListBL = new CustomerListBL(utilities.ExecutionContext);
                customerDTOList = customerListBL.GetCustomerDTOList(customerSearchCriteria, true, true);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while loading the customers", ex);
            }
            log.LogMethodExit(customerDTOList);
            return customerDTOList;
        }

        private CustomerSearchCriteria GetCustomerSearchCriteria()
        {
            log.LogMethodEntry();
            CustomerSearchCriteria searchCriteria = null;
            if (this.customerAdvancedSearchSearchCriteria != null)
            {
                searchCriteria = customerAdvancedSearchSearchCriteria;
            }
            else
            {
                searchCriteria = new CustomerSearchCriteria();
                if (string.IsNullOrWhiteSpace(txtFirstName.Text) == false)
                {
                    searchCriteria.And(CustomerSearchByParameters.PROFILE_FIRST_NAME, Operator.LIKE, "%" + txtFirstName.Text + "%");
                }

                if (string.IsNullOrWhiteSpace(txtMiddleName.Text) == false)
                {
                    searchCriteria.And(CustomerSearchByParameters.PROFILE_MIDDLE_NAME, Operator.LIKE, "%" + txtMiddleName.Text + "%");
                }
                if (string.IsNullOrWhiteSpace(txtLastName.Text) == false)
                {
                    searchCriteria.And(CustomerSearchByParameters.PROFILE_LAST_NAME, Operator.LIKE, "%" + txtLastName.Text + "%");
                }
                if (string.IsNullOrWhiteSpace(txtUniqueIdentifier.Text) == false)
                {
                    searchCriteria.And(CustomerSearchByParameters.PROFILE_UNIQUE_IDENTIFIER, Operator.LIKE, "%" + txtUniqueIdentifier.Text + "%");
                }
                if (Convert.ToInt32(cmbSite.SelectedValue) != -1)
                {
                    searchCriteria.And(CustomerSearchByParameters.CUSTOMER_SITE_ID, Operator.EQUAL_TO, Convert.ToInt32(cmbSite.SelectedValue));
                }
                if (Convert.ToInt32(cmbMembership.SelectedValue) != -1)
                {
                    searchCriteria.And(CustomerSearchByParameters.CUSTOMER_MEMBERSHIP_ID, Operator.EQUAL_TO, Convert.ToInt32(cmbMembership.SelectedValue));
                }
                if (string.IsNullOrWhiteSpace(txtPhone.Text) == false)
                {
                    if (btnPhoneSearchOption.Tag == null || btnPhoneSearchOption.Tag.ToString() == EXACT_SEARCH)
                    {
                        searchCriteria.And(new CustomerSearchCriteria(CustomerSearchByParameters.CONTACT_CONTACT_TYPE, Operator.EQUAL_TO, "PHONE")
                                                  .And(CustomerSearchByParameters.CONTACT_ATTRIBUTE1, Operator.EQUAL_TO, txtPhone.Text));
                    }
                    else
                    {
                        searchCriteria.And(new CustomerSearchCriteria(CustomerSearchByParameters.CONTACT_CONTACT_TYPE, Operator.EQUAL_TO, "PHONE")
                                                    .And(CustomerSearchByParameters.CONTACT_ATTRIBUTE1, Operator.LIKE, "%" + txtPhone.Text + "%")); 
                    }
                }
                if (string.IsNullOrWhiteSpace(txtEmail.Text) == false)
                {
                    if (btnEmailSearchOption.Tag == null || btnEmailSearchOption.Tag.ToString() == EXACT_SEARCH)
                    {
                        searchCriteria.Or(new CustomerSearchCriteria(CustomerSearchByParameters.CONTACT_CONTACT_TYPE, Operator.EQUAL_TO, "EMAIL")
                                                .And(CustomerSearchByParameters.CONTACT_ATTRIBUTE1, Operator.EQUAL_TO, txtEmail.Text));
                    }
                    else
                    {
                        searchCriteria.Or(new CustomerSearchCriteria(CustomerSearchByParameters.CONTACT_CONTACT_TYPE, Operator.EQUAL_TO, "EMAIL")
                                                  .And(CustomerSearchByParameters.CONTACT_ATTRIBUTE1, Operator.LIKE, "%" + txtEmail.Text + "%"));
                    }
                }
            }
            log.LogMethodExit(searchCriteria);
            return searchCriteria;
        }

        private int GetCustomerCount(CustomerSearchCriteria customerSearchCriteria)
        {
            log.LogMethodEntry(customerSearchCriteria);
            int customerCount = 0;
            try
            {
                CustomerListBL customerListBL = new CustomerListBL(utilities.ExecutionContext);
                customerCount = customerListBL.GetCustomerCount(customerSearchCriteria);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while getting customer count", ex);
            }
            log.LogMethodExit(customerCount);
            return customerCount;
        }

        private async void btnFirst_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            currentPage = 0;
            OnDataLoadStart();
            await LoadCustomerDTOList();
            OnDataLoadComplete();
            log.LogMethodExit();
        }

        private async void btnLast_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            currentPage = totalNoOfPages - 1;
            OnDataLoadStart();
            await LoadCustomerDTOList();
            OnDataLoadComplete();
            log.LogMethodExit();
        }

        private async void btnPrevious_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            currentPage--;
            if (currentPage < 0)
            {
                currentPage = 0;
            }
            OnDataLoadStart();
            await LoadCustomerDTOList();
            OnDataLoadComplete();
            log.LogMethodExit();
        }

        private async void btnNext_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            currentPage++;
            if (currentPage >= totalNoOfPages)
            {
                currentPage = totalNoOfPages - 1;
            }
            OnDataLoadStart();
            await LoadCustomerDTOList();
            OnDataLoadComplete();
            log.LogMethodExit();
        }

        private void DisplayCurrentPageCount()
        {
            log.LogMethodEntry();
            if (totalNoOfPages > 0)
            {
                lblPage.Text = "Page " + (currentPage + 1) + " of " + totalNoOfPages;
            }
            else
            {
                lblPage.Text = "Page " + (currentPage) + " of " + totalNoOfPages;
            }
            log.LogMethodExit();
        }

        private IList<CardCoreDTO> GetCardCoreDTOList(int customerId)
        {
            log.LogMethodEntry(customerId);
            IList<CardCoreDTO> cardCoreDTOList = null;
            CardCoreBL cardCoreBL = new CardCoreBL();
            CardParams cardParams = new CardParams();
            if (customerId >= 0)
            {
                cardParams.CustomerId = customerId;
                cardCoreDTOList = cardCoreBL.GetAllCardsList(cardParams, false, null);
            }
            if (cardCoreDTOList == null)
            {
                cardCoreDTOList = new SortableBindingList<CardCoreDTO>();
            }
            else
            {
                cardCoreDTOList = cardCoreDTOList.OrderByDescending(x => x.PrimaryCard).ToList(); // To display Primary card on top
                cardCoreDTOList = new SortableBindingList<CardCoreDTO>(cardCoreDTOList);
            }

            log.LogMethodExit(cardCoreDTOList);
            return cardCoreDTOList;
        }

        private void customerDTOListBS_AddingNew(object sender, AddingNewEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            CustomerDTO customerDTO = new CustomerDTO();
            customerDTO.AddressDTOList = new List<AddressDTO>();
            customerDTO.ContactDTOList = new List<ContactDTO>();
            log.LogMethodExit();
        }

        private async void customerDTOListBS_CurrentChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            chbShowActiveAddressEntries.Checked = true;
            chbShowActiveContactEntries.Checked = true;
            if (customerDTOListBS.Current != null && customerDTOListBS.Current is CustomerDTO)
            {
                CustomerDTO customerDTO = customerDTOListBS.Current as CustomerDTO;
                dgvAddressDTOList.AllowUserToAddRows = true;
                addressDTOListBS.DataSource = customerDTO.AddressDTOList;
                contactDTOListBS.DataSource = customerDTO.ContactDTOList;
                dgvContactDTOList.AllowUserToAddRows = true;
                if (customerDTO.ContactDTOList.FirstOrDefault((x) => x.IsActive == false) != null)
                {
                    chbShowActiveContactEntries.Checked = false;
                }
                if (customerDTO.AddressDTOList.FirstOrDefault((x) => x.IsActive == false) != null)
                {
                    chbShowActiveAddressEntries.Checked = false;
                }
                cardCoreDTOListBS.DataSource = await Task<IList<CardCoreDTO>>.Factory.StartNew(() => { return (SortableBindingList<CardCoreDTO>)GetCardCoreDTOList(customerDTO.Id); });
            }
            else
            {
                cardCoreDTOListBS.DataSource = new List<CardCoreDTO>();
                addressDTOListBS.DataSource = new List<AddressDTO>();
                dgvAddressDTOList.AllowUserToAddRows = false;
                contactDTOListBS.DataSource = new List<ContactDTO>();
                dgvContactDTOList.AllowUserToAddRows = false;
            }
            log.LogMethodExit();
        }

        private void dgvCustomerDTOList_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.ColumnIndex != genderDataGridViewComboBoxColumn.Index)
            {
                MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 1144).Replace("&1", dgvCustomerDTOList.Columns[e.ColumnIndex].HeaderText));
            }
            e.Cancel = true;
            log.LogMethodExit();
        }



        private void dgvContactDTOList_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.ColumnIndex != contactTypeIdDataGridViewComboBoxColumn.Index)
            {
                MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 1144).Replace("&1", dgvContactDTOList.Columns[e.ColumnIndex].HeaderText));
            }
            e.Cancel = true;
            log.LogMethodExit();
        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            customerAdvancedSearchSearchCriteria = null;
            OnDataLoadStart();
            await RefreshCustomerDTOList();
            OnDataLoadComplete();
            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            this.DialogResult = DialogResult.Cancel;
            this.Close();
            log.LogMethodExit();
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            OnDataLoadStart();
            customerAdvancedSearchSearchCriteria = null;
            cmbSite.SelectedIndex = 0;
            txtFirstName.ResetText();
            txtLastName.ResetText();
            txtMiddleName.ResetText();
            await RefreshData();
            OnDataLoadComplete();
            log.LogMethodExit();
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            OnDataLoadStart();
            dgvCustomerDTOList.EndEdit();
            dgvAddressDTOList.EndEdit();
            dgvContactDTOList.EndEdit();
            SortableBindingList<CustomerDTO> customerDTOSortableList = (SortableBindingList<CustomerDTO>)customerDTOListBS.DataSource;
            CustomerBL customerBL;
            bool error = false;
            if (customerDTOSortableList != null)
            {
                for (int i = 0; i < customerDTOSortableList.Count; i++)
                {
                    if (customerDTOSortableList[i].IsChangedRecursive)
                    {
                        SqlConnection sqlConnection = utilities.createConnection();
                        SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
                        try
                        {

                            customerBL = new CustomerBL(utilities.ExecutionContext, customerDTOSortableList[i]);
                            customerBL.Save(sqlTransaction);
                            sqlTransaction.Commit();
                        }
                        catch (ValidationException ex)
                        {
                            sqlTransaction.Rollback();
                            log.Error("validation failed", ex);
                            StringBuilder errorMessageBuilder = new StringBuilder("");
                            foreach (var validationError in ex.ValidationErrorList)
                            {
                                errorMessageBuilder.Append(validationError.Message);
                                errorMessageBuilder.Append(Environment.NewLine);
                            }
                            MessageBox.Show(errorMessageBuilder.ToString());
                            error = true;
                            customerDTOListBS.Position = i;
                            dgvCustomerDTOList.Rows[i].Selected = true;
                            break;
                        }
                        catch (Exception ex)
                        {
                            sqlTransaction.Rollback();
                            error = true;
                            log.Error("Exception occurred while saving discount", ex);
                            customerDTOListBS.Position = i;
                            dgvCustomerDTOList.Rows[i].Selected = true;
                            MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 718));
                            break;
                        }
                        finally
                        {
                            sqlConnection.Close();
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 371));
            }
            if (!error)
            {
                await LoadCustomerDTOList();
            }
            else
            {
                dgvCustomerDTOList.Update();
                dgvAddressDTOList.Update();
                dgvContactDTOList.Update();
                dgvCustomerDTOList.Refresh();
                dgvAddressDTOList.Refresh();
                dgvContactDTOList.Refresh();
            }
            OnDataLoadComplete();
            log.LogMethodExit();
        }

        private async void btnImport_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ImportCustomerUI importCustomerUI = new ImportCustomerUI(utilities);
            importCustomerUI.ShowDialog();
            OnDataLoadStart();
            await RefreshCustomerDTOList();
            OnDataLoadComplete();
            log.LogMethodExit();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            CustomerExportUI customerExportUI = new CustomerExportUI(utilities, GetCustomerSearchCriteria());
            customerExportUI.ShowDialog();
            log.LogMethodExit();
        }

        private void btnRelationship_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (customerDTOListBS.Current != null && customerDTOListBS.Current is CustomerDTO)
            {
                CustomerRelationshipListUI customerRelationshipListUI = new CustomerRelationshipListUI(utilities, customerDTOListBS.Current as CustomerDTO, MessageBox.Show);
                customerRelationshipListUI.StartPosition = FormStartPosition.CenterScreen;
                customerRelationshipListUI.ShowDialog();
            }
            log.LogMethodExit();
        }

        private async void btnAdvancedSearch_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (customerAdvancedSearchSearchCriteria == null)
            {
                customerAdvancedSearchSearchCriteria = new CustomerSearchCriteria();
            }
            AdvancedSearchUI advancedSearchUI = new AdvancedSearchUI(utilities, customerAdvancedSearchSearchCriteria);
            if (advancedSearchUI.ShowDialog() == DialogResult.OK)
            {
                OnDataLoadStart();
                await RefreshCustomerDTOList();
                OnDataLoadComplete();
            }
            else
            {
                customerAdvancedSearchSearchCriteria = null;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// On CustomerDTOList Cell content click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void dgvCustomerDTOList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (e.ColumnIndex == customDataSetDataGridViewButtonColumn.Index)
                {
                    if (customAttributesDTOList == null)
                    {
                        CustomAttributesListBL customAttributesListBL = new CustomAttributesListBL(utilities.ExecutionContext);
                        List<KeyValuePair<CustomAttributesDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CustomAttributesDTO.SearchByParameters, string>>();
                        searchParameters.Add(new KeyValuePair<CustomAttributesDTO.SearchByParameters, string>(CustomAttributesDTO.SearchByParameters.APPLICABILITY, Applicability.CUSTOMER.ToString()));
                        searchParameters.Add(new KeyValuePair<CustomAttributesDTO.SearchByParameters, string>(CustomAttributesDTO.SearchByParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));
                        customAttributesDTOList = customAttributesListBL.GetCustomAttributesDTOList(searchParameters);
                        if (customAttributesDTOList == null)
                        {
                            customAttributesDTOList = new List<CustomAttributesDTO>();
                        }
                    }
                    if (customAttributesDTOList.Count == 0)
                    {
                        MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 559, Applicability.CUSTOMER.ToString()));
                        return;
                    }
                    if (e.RowIndex >= 0 &&
                        e.ColumnIndex == customDataSetDataGridViewButtonColumn.Index &&
                        customerDTOListBS.Current != null &&
                        customerDTOListBS.Current is CustomerDTO)
                    {
                        CustomerDTO customerDTO = customerDTOListBS.Current as CustomerDTO;
                        if (customerDTO.CustomDataSetDTO == null)
                        {
                            customerDTO.CustomDataSetDTO = new CustomDataSetDTO();
                        }

                        CustomDataListUI customDataListUI = new CustomDataListUI(utilities, customerDTO.CustomDataSetDTO, Applicability.CUSTOMER);
                        customDataListUI.ShowDialog();
                        if (customerDTO.CustomDataSetId != customerDTO.CustomDataSetDTO.CustomDataSetId)
                        {
                            customerDTO.CustomDataSetId = customerDTO.CustomDataSetDTO.CustomDataSetId;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while displaying custom data", ex);
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit();
        }

        private void dgvContactDTOList_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (e.RowIndex >= 0)
                {
                    if (dgvContactDTOList.Rows[e.RowIndex].Cells[contactTypeIdDataGridViewComboBoxColumn.Index].Value != null &&
                        dgvContactDTOList.Rows[e.RowIndex].Cells[contactTypeIdDataGridViewComboBoxColumn.Index].Value is ContactType)
                    {
                        ContactType contactType = (ContactType)dgvContactDTOList.Rows[e.RowIndex].Cells[contactTypeIdDataGridViewComboBoxColumn.Index].Value;
                        if (contactType == ContactType.PHONE ||
                           contactType == ContactType.EMAIL ||
                           contactType == ContactType.WECHAT)
                        {
                            dgvContactDTOList.Rows[e.RowIndex].Cells[attribute2DataGridViewTextBoxColumn.Index].ReadOnly = true;
                        }
                        else
                        {
                            dgvContactDTOList.Rows[e.RowIndex].Cells[attribute2DataGridViewTextBoxColumn.Index].ReadOnly = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                log.Error("Error occurred while enable/ disable attribute 2 column", ex);
            }


            log.LogMethodExit();
        }

        private void addressDTOListBS_AddingNew(object sender, AddingNewEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            AddressDTO addressDTO = new AddressDTO();
            int defaultCountryId = Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault<int>(utilities.ExecutionContext, "STATE_LOOKUP_FOR_COUNTRY");
            if (defaultCountryId >= 0)
            {
                addressDTO.CountryId = defaultCountryId;
            }
            e.NewObject = addressDTO;
            log.LogMethodExit();
        }

        private void dgvAddressDTOList_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.ColumnIndex == countryIdDataGridViewComboBoxColumn.Index || e.ColumnIndex == stateIdDataGridViewComboBoxColumn.Index)
            {
                List<AddressDTO> addressDTOList = null;
                if (addressDTOListBS.DataSource is List<AddressDTO>)
                {
                    addressDTOList = (List<AddressDTO>)addressDTOListBS.DataSource;
                }
                if (addressDTOList != null)
                {
                    AddressDTO addressDTO = addressDTOList[e.RowIndex];
                    DataGridViewComboBoxCell stateCell = dgvAddressDTOList.CurrentRow.Cells[stateIdDataGridViewComboBoxColumn.Index] as DataGridViewComboBoxCell;
                    DataGridViewComboBoxCell countryCell = dgvAddressDTOList.CurrentRow.Cells[countryIdDataGridViewComboBoxColumn.Index] as DataGridViewComboBoxCell;
                    if (e.ColumnIndex == countryIdDataGridViewComboBoxColumn.Index)
                    {
                        if (countryCell != null &&
                            stateCell != null)
                        {
                            int stateId = -1;
                            int countryId = -1;
                            if (stateCell.Value != null)
                            {
                                stateId = Convert.ToInt32(stateCell.Value);
                            }
                            if (countryCell.Value != null)
                            {
                                countryId = Convert.ToInt32(countryCell.Value);
                            }
                            if (GetCountryIdForState(stateId) != countryId || countryId == -1)
                            {
                                stateCell.Value = -1;
                                stateCell.DataSource = GetStateSourceByCountryId(addressDTO.CountryId);
                                stateCell.ValueMember = "StateId";
                                stateCell.DisplayMember = "Description";
                            }
                        }
                    }
                    else
                    {
                        if (countryCell != null &&
                            countryCell.Value != null &&
                            stateCell != null &&
                            stateCell.Value != null)
                        {
                            int stateId = Convert.ToInt32(stateCell.Value);
                            if (stateId > -1)
                            {
                                int countryId = GetCountryIdForState(stateId);
                                if (Convert.ToInt32(countryCell.Value) != countryId)
                                {
                                    countryCell.Value = countryId;
                                }
                            }
                        }
                    }
                }
            }
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

        private void dgvAddressDTOList_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (dgvAddressDTOList.CurrentCell.ColumnIndex == countryIdDataGridViewComboBoxColumn.Index ||
                dgvAddressDTOList.CurrentCell.ColumnIndex == stateIdDataGridViewComboBoxColumn.Index)
            {
                dgvAddressDTOList.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
            log.LogMethodExit();
        }

        private void UpdateDgvAddressDTOListRow(DataGridViewRow row, AddressDTO addressDTO)
        {
            log.LogMethodEntry(row, addressDTO);
            if (row != null && addressDTO != null)
            {
                DataGridViewComboBoxCell cell = row.Cells[stateIdDataGridViewComboBoxColumn.Index] as DataGridViewComboBoxCell;
                if (cell != null)
                {
                    cell.DataSource = GetStateSourceByCountryId(addressDTO.CountryId);
                    cell.ValueMember = "StateId";
                    cell.DisplayMember = "Description";
                }
            }
            log.LogMethodExit();
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


        private void dgvAddressDTOList_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            bool handled = false;
            if (e.ColumnIndex == stateIdDataGridViewComboBoxColumn.Index)
            {
                List<AddressDTO> addressDTOList = null;
                if (addressDTOListBS.DataSource is List<AddressDTO>)
                {
                    addressDTOList = (List<AddressDTO>)addressDTOListBS.DataSource;
                }
                if (addressDTOList != null && addressDTOList.Count > 0)
                {
                    DataGridViewComboBoxCell cell = dgvAddressDTOList[e.ColumnIndex, e.RowIndex] as DataGridViewComboBoxCell;
                    if (cell != null)
                    {
                        cell.Value = -1;
                        handled = true;
                    }
                }
            }
            if (!handled)
            {
                MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 1144).Replace("&1", dgvAddressDTOList.Columns[e.ColumnIndex].HeaderText));
            }
            e.Cancel = true;
            log.LogMethodExit();
        }

        private void addressDTOListBS_DataSourceChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            List<AddressDTO> addressDTOList = null;
            if (addressDTOListBS.DataSource is List<AddressDTO>)
            {
                addressDTOList = (List<AddressDTO>)addressDTOListBS.DataSource;
            }
            if (addressDTOList != null)
            {
                for (int i = 0; i < addressDTOList.Count; i++)
                {
                    UpdateDgvAddressDTOListRow(dgvAddressDTOList.Rows[i], addressDTOList[i]);
                }
            }
            log.LogMethodExit();
        }

        private void dgvAddressDTOList_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.RowIndex >= 0)
            {
                List<AddressDTO> addressDTOList = null;
                if (addressDTOListBS.DataSource is List<AddressDTO>)
                {
                    addressDTOList = (List<AddressDTO>)addressDTOListBS.DataSource;
                }
                if (addressDTOList != null)
                {
                    UpdateDgvAddressDTOListRow(dgvAddressDTOList.Rows[e.RowIndex], addressDTOList[e.RowIndex]);
                }
            }

            log.LogMethodExit();
        }

        private void RefreshAddressDTOList()
        {
            log.LogMethodEntry();
            if (customerDTOListBS.Current != null && customerDTOListBS.Current is CustomerDTO)
            {
                CustomerDTO customerDTO = customerDTOListBS.Current as CustomerDTO;
                if (customerDTO.ProfileId > -1)
                {
                    List<AddressDTO> addressDTOList = null;
                    AddressListBL addressListBL = new AddressListBL(utilities.ExecutionContext);
                    List<KeyValuePair<AddressDTO.SearchByParameters, string>> searchByParameters = new List<KeyValuePair<AddressDTO.SearchByParameters, string>>();
                    searchByParameters.Add(new KeyValuePair<AddressDTO.SearchByParameters, string>(AddressDTO.SearchByParameters.PROFILE_ID, customerDTO.ProfileId.ToString()));
                    if (chbShowActiveAddressEntries.Checked)
                    {
                        searchByParameters.Add(new KeyValuePair<AddressDTO.SearchByParameters, string>(AddressDTO.SearchByParameters.IS_ACTIVE, "1"));
                    }
                    addressDTOList = addressListBL.GetAddressDTOList(searchByParameters);
                    if (addressDTOList == null)
                    {
                        addressDTOList = new List<AddressDTO>();
                    }
                    customerDTO.AddressDTOList = addressDTOList;
                    addressDTOListBS.DataSource = customerDTO.AddressDTOList;
                }
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
            if (customerDTOListBS.Current != null && customerDTOListBS.Current is CustomerDTO)
            {
                CustomerDTO customerDTO = customerDTOListBS.Current as CustomerDTO;
                if (customerDTO.ProfileId > -1)
                {
                    List<ContactDTO> contactDTOList = null;
                    ContactListBL contactListBL = new ContactListBL(utilities.ExecutionContext);
                    List<KeyValuePair<ContactDTO.SearchByParameters, string>> searchByParameters = new List<KeyValuePair<ContactDTO.SearchByParameters, string>>();
                    searchByParameters.Add(new KeyValuePair<ContactDTO.SearchByParameters, string>(ContactDTO.SearchByParameters.PROFILE_ID, customerDTO.ProfileId.ToString()));
                    if (chbShowActiveContactEntries.Checked)
                    {
                        searchByParameters.Add(new KeyValuePair<ContactDTO.SearchByParameters, string>(ContactDTO.SearchByParameters.IS_ACTIVE, "1"));
                    }
                    contactDTOList = contactListBL.GetContactDTOList(searchByParameters);
                    if (contactDTOList == null)
                    {
                        contactDTOList = new List<ContactDTO>();
                    }
                    customerDTO.ContactDTOList = contactDTOList;
                    contactDTOListBS.DataSource = customerDTO.ContactDTOList;
                }
            }
            log.LogMethodExit();
        }

        private void chbShowActiveContactEntries_CheckedChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            RefreshContactDTOList();
            log.LogMethodExit();
        }

        private bool IsUnsavedChangesExist()
        {
            log.LogMethodEntry();
            bool unsavedChangesExist = false;
            if ((this is CustomerLookupUI) == false)
            {
                if (customerDTOListBS.DataSource != null && customerDTOListBS.DataSource is SortableBindingList<CustomerDTO>)
                {
                    SortableBindingList<CustomerDTO> customerDTOList = customerDTOListBS.DataSource as SortableBindingList<CustomerDTO>;
                    foreach (var customerDTO in customerDTOList)
                    {
                        if (customerDTO.IsChangedRecursive)
                        {
                            unsavedChangesExist = true;
                            break;
                        }
                    }
                }
            }
            log.LogMethodExit(unsavedChangesExist);
            return unsavedChangesExist;
        }

        private void CustomerListUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            dgvCustomerDTOList.EndEdit();
            dgvAddressDTOList.EndEdit();
            dgvContactDTOList.EndEdit();
            if (IsUnsavedChangesExist())
            {
                DialogResult DR = MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 566), MessageContainerList.GetMessage(utilities.ExecutionContext, "Close Customers"), MessageBoxButtons.YesNoCancel);
                switch (DR)
                {
                    case DialogResult.Yes:
                        {
                            btnSave.PerformClick();
                            if (IsUnsavedChangesExist())
                            {
                                e.Cancel = true;
                            }
                            break;
                        }
                    case DialogResult.No: break;
                    case DialogResult.Cancel: e.Cancel = true; break;
                    default: break;
                }
            }
            virtualKeyboardController.RemoveEventListener(this);
            log.LogMethodExit();
        }

        private async void btnUpdateMembership_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (customerDTOListBS.Current != null && customerDTOListBS.Current is CustomerDTO)
            {
                UpdateMembershipUI updateMembershipUI = new UpdateMembershipUI(utilities, customerDTOListBS.Current as CustomerDTO);
                if (updateMembershipUI.ShowDialog() == DialogResult.OK)
                {
                    OnDataLoadStart();
                    await LoadCustomerDTOList();
                    OnDataLoadComplete();
                }
            }
            log.LogMethodExit();
        }

        private void CustomerListUI_Shown(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e); 
            virtualKeyboardController = new VirtualKeyboardController();
            virtualKeyboardController.Initialize(this, new List<Control>() { btnShowKeyPad },ParafaitDefaultContainerList.GetParafaitDefault<bool>(utilities.ExecutionContext, "AUTO_POPUP_ONSCREEN_KEYBOARD"));
            log.LogMethodExit();
        }
         

        private void dgvContactDTOList_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.ColumnIndex == attribute2DataGridViewTextBoxColumn.Index && e.Value != null)
            {
                dgvContactDTOList.Rows[e.RowIndex].Tag = e.Value;
                e.Value = new String('\u25CF', e.Value.ToString().Length);
            }
            log.LogMethodExit();
        }

        private void dgvContactDTOList_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (dgvContactDTOList.CurrentCell.ColumnIndex == attribute2DataGridViewTextBoxColumn.Index)//select target column
            {
                TextBox textBox = e.Control as TextBox;
                if (textBox != null)
                {
                    textBox.UseSystemPasswordChar = true;
                }
            }
            else
            {
                TextBox textBox = e.Control as TextBox;
                if (textBox != null)
                {
                    textBox.UseSystemPasswordChar = false;
                }
            }
            log.LogMethodExit();
        }


        protected virtual void dgvCardDTOList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            log.LogMethodExit();
        }

        private void btnAssociatedCards_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (customerDTOListBS.Current != null && customerDTOListBS.Current is CustomerDTO)
            {
                CustomerDTO customerDTO = customerDTOListBS.Current as CustomerDTO;
                AccountListBL accountListBL = new AccountListBL(utilities.ExecutionContext);
                List<KeyValuePair<AccountDTO.SearchByParameters, string>> accountSearchParameters = new List<KeyValuePair<AccountDTO.SearchByParameters, string>>();
                accountSearchParameters.Add(new KeyValuePair<AccountDTO.SearchByParameters, string>(AccountDTO.SearchByParameters.CUSTOMER_ID, customerDTO.Id.ToString()));
                var accountDTOList = accountListBL.GetAccountDTOList(accountSearchParameters, true, true);
                if (accountDTOList == null)
                {
                    log.LogMethodExit(null, "No Active Cards for View ");
                    accountDTOList = new List<AccountDTO>();
                    return;
                }
                accountDTOList = accountDTOList.OrderByDescending(x => x.PrimaryAccount).ToList();
                SortableBindingList<AccountDTO> accountDTOListForDisplay = new SortableBindingList<AccountDTO>(accountDTOList);
                using (CustomerAssociatedCardsUI customerLinkedCardsUI = new CustomerAssociatedCardsUI(utilities))
                {
                    customerLinkedCardsUI.AccountDTOList = accountDTOListForDisplay;
                    customerLinkedCardsUI.ShowDialog();
                    cardNumber = customerLinkedCardsUI.CardNumber;
                }
                //this.DialogResult = System.Windows.Forms.DialogResult.No;
            }
            log.LogMethodExit();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            txtEmail.Text =
            txtFirstName.Text =
            txtLastName.Text =
            txtMiddleName.Text =
            txtPhone.Text =
            txtUniqueIdentifier.Text = string.Empty;
            cmbMembership.SelectedValue = -1;
            cmbSite.SelectedValue = -1;
            customerAdvancedSearchSearchCriteria = null;
            ResetSearchOptionButtons();
            log.LogMethodExit();
        }

        private void btnSearchOption_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                Button btnClicked = (Button)sender;
                ToggleSearchOptionButtonTag(btnClicked);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void ToggleSearchOptionButtonTag(Button btnClicked)
        {
            log.LogMethodEntry();
            if (btnClicked != null)
            {
                if (btnClicked.Tag == null || btnClicked.Tag.ToString() == "L")
                {
                    btnClicked.Tag = EXACT_SEARCH;
                    btnClicked.BackgroundImage = Properties.Resources.ToggleOn;
                    btnClicked.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, "Exact");
                    btnClicked.TextAlign = ContentAlignment.MiddleLeft;
                }
                else
                {
                    btnClicked.Tag = LIKE_SEARCH;
                    btnClicked.BackgroundImage = Properties.Resources.ToggleOff;
                    btnClicked.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, "Like");
                    btnClicked.TextAlign = ContentAlignment.MiddleRight;
                }
            }
            log.LogMethodExit();
        }

        private void ResetSearchOptionButtons()
        {
            log.LogMethodEntry();
            try
            {
                btnPhoneSearchOption.Tag = LIKE_SEARCH;
                ToggleSearchOptionButtonTag(btnPhoneSearchOption);
                btnEmailSearchOption.Tag = LIKE_SEARCH;
                ToggleSearchOptionButtonTag(btnEmailSearchOption);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }         
    }
}
