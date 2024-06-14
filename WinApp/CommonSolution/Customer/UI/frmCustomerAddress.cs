/********************************************************************************************
 * Project Name - frmCustAddress
 * Description  -from to add customer address details
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 2.70.2        18-Aug-2019       Girish Kundar   Created
 2.110.0       09-Jan-2021       Girish Kundar   Modified: Issue fix - Selected index fix for loading form
 ********************************************************************************************/
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Customer;

namespace Semnox.Parafait.Customer
{
    public partial class frmCustomerAddress : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext = ExecutionContext.GetExecutionContext();
        public AddressDTO addressDTO;
        private Dictionary<string, Control> attributeControlDictionary;
        private Utilities utilities = new Utilities();
        private List<CountryDTO> countryList = null;
        private List<StateDTO> stateList = null;
        private VirtualKeyboardController virtualKeyboardController;

        /// <summary>
        /// Constructor for frmCustomerAddress
        /// </summary>
        /// <param name="addressDTO">addressDTO</param>
        /// <param name="showKeyboardOnTextboxEntry">showKeyboardOnTextboxEntry</param>
        public frmCustomerAddress(AddressDTO addressDTO, bool showKeyboardOnTextboxEntry = true)
        {
            log.LogMethodEntry();
            InitializeComponent();
            LoadAddressTypes();
            LoadState();
            LoadCountry();
            SetCustomerMaxInputLenghts();
            attributeControlDictionary = new Dictionary<string, Control>();
            EnableDisablePanels();
            if (addressDTO == null)
            {
                throw new Exception("AddressDTO is Not Bind to the User Control ");
            }
            else
            {
                this.addressDTO = addressDTO;
                UpdateAddressUI();
            }
            utilities.setLanguage(this);
            Rectangle rcScreen = Screen.PrimaryScreen.WorkingArea;
            this.Location = new Point((rcScreen.Left + rcScreen.Right) / 2 - (this.Width / 2), 15);
            virtualKeyboardController = new VirtualKeyboardController();
            virtualKeyboardController.Initialize(this, new List<Control>() { btnShowKeyPad }, showKeyboardOnTextboxEntry);// ParafaitDefaultContainerList.GetParafaitDefault<bool>(utilities.ExecutionContext, "AUTO_POPUP_ONSCREEN_KEYBOARD"));
            SetInitialFocus();
            log.LogMethodExit();
        }

        private void SetInitialFocus()
        {
            log.LogMethodEntry();
            txtLine1.Focus();
            log.LogMethodExit();
        }

        /// <summary>
        /// Setting the Maximum character lengths for TextBoxes
        /// </summary>
        private void SetCustomerMaxInputLenghts()
        {
            log.LogMethodEntry();
            txtLine1.MaxLength = 100;
            txtLine2.MaxLength = 100;
            txtLine3.MaxLength = 100;
            txtCity.MaxLength = 100;
            txtPostalCode.MaxLength = 100;
            log.LogMethodExit();
        }

        /// <summary>
        /// Method add the * character for Mandatory fields
        /// </summary>
        /// <param name="label">label</param>
        /// <param name="defaultValue">defaultValue</param>
        private void UpdateManadtoryLabel(Label label, string defaultValue)
        {
            log.LogMethodEntry(label, defaultValue);
            if (label != null &&
                label.Text != null &&
                label.Text.Contains("*") == false &&
                ParafaitDefaultContainerList.GetParafaitDefault(executionContext, defaultValue) == "M")
            {
                label.Text += "*";
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Method which Checks for Mandatory and Optional fields for Customer Address
        /// </summary>
        private void EnableDisablePanels()
        {
            log.LogMethodEntry();
            if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CITY") == "N")
            {
                pnlCity.Visible = false;
                //flpAddress.Controls.Remove(pnlCity);
            }
            else if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CITY") == "M")
            {
                UpdateManadtoryLabel(lblCity, "CITY");
            }
            if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "STATE") == "N")
            {
                pnlState.Visible = false;
                // flpAddress.Controls.Remove(pnlState);
            }
            else if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "STATE") == "M")
            {
                UpdateManadtoryLabel(lblState, "STATE");
            }
            if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "COUNTRY") == "N")
            {
                pnlCountry.Visible = false;
                flpAddress.Controls.Remove(pnlCountry);
            }
            else if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "COUNTRY") == "M")
            {
                UpdateManadtoryLabel(lblCountry, "COUNTRY");
            }
            if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "PIN") == "N")
            {
                pnlPostalCode.Visible = false;
                //flpAddress.Controls.Remove(pnlPostalCode);
            }
            else if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "PIN") == "M")
            {
                UpdateManadtoryLabel(lblPostalCode, "PIN");
            }
            if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ADDRESS1") == "N")
            {
                pnlLine1.Visible = false;
                // flpAddress.Controls.Remove(pnlLine1);
            }
            else if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ADDRESS1") == "M")
            {
                UpdateManadtoryLabel(lblAddress1, "ADDRESS1");
            }
            if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ADDRESS2") == "N")
            {
                pnlLine2.Visible = false;
                // flpAddress.Controls.Remove(pnlLine2);
            }
            else if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ADDRESS2") == "M")
            {
                UpdateManadtoryLabel(lblAddress2, "ADDRESS2");
            }
            if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ADDRESS3") == "N")
            {
                pnlLine3.Visible = false;
                //flpAddress.Controls.Remove(pnlLine3);
            }
            else if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ADDRESS3") == "M")
            {
                UpdateManadtoryLabel(lblAddress3, "ADDRESS3");
            }
            if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ADDRESS_TYPE") == "N")
            {
                pnlType.Visible = false;
            }
            else if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ADDRESS_TYPE") == "M")
            {
                UpdateManadtoryLabel(lblType, "ADDRESS_TYPE");
            }

            log.LogMethodExit();
        }

        /// <summary>
        /// Updates the Address Card for View.
        /// </summary>
        private void UpdateAddressUI()
        {
            log.LogMethodEntry();
            if(addressDTO.Id == -1)
            {
                int defaultCountryId = ParafaitDefaultContainerList.GetParafaitDefault<int>(utilities.ExecutionContext, "STATE_LOOKUP_FOR_COUNTRY");
                log.LogVariableState("defaultCountryId", defaultCountryId);
                if (defaultCountryId >= 0)
                {
                    addressDTO.CountryId = defaultCountryId;
                }
            }
            List<StateDTO> stateDTOList = (List<StateDTO>)cbState.DataSource;
            if(stateDTOList != null && stateDTOList.Any())
            {
                int defaultStateIndex = stateDTOList.FindIndex(x => x.StateId == addressDTO.StateId);
                cbState.SelectedIndex = (defaultStateIndex == -1 ? 0 : defaultStateIndex);
            }
            else
            {
                cbState.SelectedIndex = 0; // Default <Select>
            }
            List<CountryDTO> countryDTOList = (List<CountryDTO>)cbCountry.DataSource;
            if (countryDTOList != null && countryDTOList.Any())
            {
                int defaultCountryIndex = countryDTOList.FindIndex(x => x.CountryId == addressDTO.CountryId);
                cbCountry.SelectedIndex = (defaultCountryIndex == -1 ? 0 : defaultCountryIndex);
            }
            else
            {
                cbCountry.SelectedIndex = 0; // Default <Select>
            }
            cbType.SelectedValue = addressDTO.AddressType;
            //cbState.SelectedIndex = addressDTO.StateId;
            //cbCountry.SelectedIndex = addressDTO.CountryId;
            txtLine1.Text = string.IsNullOrEmpty(addressDTO.Line1) ? string.Empty : addressDTO.Line1;
            txtLine2.Text = string.IsNullOrEmpty(addressDTO.Line2) ? string.Empty : addressDTO.Line2;
            txtLine3.Text = string.IsNullOrEmpty(addressDTO.Line3) ? string.Empty : addressDTO.Line3;
            txtPostalCode.Text = string.IsNullOrEmpty(addressDTO.PostalCode) ? string.Empty : addressDTO.PostalCode;
            txtCity.Text = string.IsNullOrEmpty(addressDTO.City) ? string.Empty : addressDTO.City;
            chkActive.CheckState = ((addressDTO.IsActive) ? CheckState.Checked : CheckState.Unchecked);
            txtLine1.Focus();
            log.LogMethodExit();
        }

        /// <summary>
        /// Button Close Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.Dispose();
            log.LogMethodExit();
        }

        /// <summary>
        /// Method Loads the State names for the List
        /// </summary>
        private void LoadState()
        {
            log.LogMethodEntry();

            try
            {
                StateDTOList stateDTOList = new StateDTOList(executionContext);
                List<KeyValuePair<StateDTO.SearchByParameters, string>> searchStateParams = new List<KeyValuePair<StateDTO.SearchByParameters, string>>();
                searchStateParams.Add(new KeyValuePair<StateDTO.SearchByParameters, string>(StateDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
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
            cbState.DataSource = stateList;
            cbState.DisplayMember = "Description";
            cbState.ValueMember = "StateId";
        }

        /// <summary>
        /// Method Loads the Country names for the List
        /// </summary>
        private void LoadCountry()
        {
            log.LogMethodEntry();
            try
            {
                CountryDTOList countryDTOList = new CountryDTOList(executionContext);
                List<KeyValuePair<CountryDTO.SearchByParameters, string>> searchCountryParams = new List<KeyValuePair<CountryDTO.SearchByParameters, string>>();
                searchCountryParams.Add(new KeyValuePair<CountryDTO.SearchByParameters, string>(CountryDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
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
            cbCountry.DataSource = countryList;
            cbCountry.DisplayMember = "CountryName";
            cbCountry.ValueMember = "CountryId";
            log.LogMethodExit();
        }

        /// <summary>
        /// Method Loads the Address Types for the List
        /// </summary>
        private void LoadAddressTypes()
        {
            log.LogMethodEntry();
            List<AddressTypeDTO> addressTypeDTOList = null;
            try
            {
                AddressTypeListBL addressTypeListBL = new AddressTypeListBL(executionContext);
                List<KeyValuePair<AddressTypeDTO.SearchByParameters, string>> searchAddressTypeParams = new List<KeyValuePair<AddressTypeDTO.SearchByParameters, string>>();
                searchAddressTypeParams.Add(new KeyValuePair<AddressTypeDTO.SearchByParameters, string>(AddressTypeDTO.SearchByParameters.IS_ACTIVE, "1"));
                addressTypeDTOList = addressTypeListBL.GetAddressTypeDTOList(searchAddressTypeParams);
                if (addressTypeDTOList == null)
                {
                    addressTypeDTOList = new List<AddressTypeDTO>();
                }
                addressTypeDTOList.Insert(0, new AddressTypeDTO());
                addressTypeDTOList[0].Id = -1;
                addressTypeDTOList[0].Description = "SELECT";
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while loading address type", ex);
            }
            log.LogMethodExit(addressTypeDTOList);
            cbType.DataSource = addressTypeDTOList;
            cbType.DisplayMember = "Description";
            cbType.ValueMember = "AddressType";
            log.LogMethodExit();
        }

        /// <summary>
        /// Cancels the Address update and closes the form.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.Close();
            log.LogMethodExit();
        }

        /// <summary>
        /// Clears the error indication colors for the controls  
        /// </summary>
        private void ClearErrorColor()
        {
            log.LogMethodEntry();
            lblMessage.Text = string.Empty;
            if (txtLine1.BackColor == Color.OrangeRed) txtLine1.BackColor = Color.White;
            if (txtLine2.BackColor == Color.OrangeRed) txtLine2.BackColor = Color.White;
            if (txtLine3.BackColor == Color.OrangeRed) txtLine3.BackColor = Color.White;
            if (txtPostalCode.BackColor == Color.OrangeRed) txtPostalCode.BackColor = Color.White;
            if (txtCity.BackColor == Color.OrangeRed) txtCity.BackColor = Color.White;
            if (cbType.BackColor == Color.OrangeRed) cbType.BackColor = Color.White;
            if (cbCountry.BackColor == Color.OrangeRed) cbCountry.BackColor = Color.White;
            if (cbState.BackColor == Color.OrangeRed) cbState.BackColor = Color.White;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the address info to the DTO List
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ClearErrorColor();
            AddressTypeDTO addressTypeDTO = (AddressTypeDTO)cbType.SelectedItem;
            addressDTO.AddressType = addressTypeDTO.AddressType;
            if (cbState.SelectedValue != null)
            {
                addressDTO.StateId = Convert.ToInt32(cbState.SelectedValue);
            }
            if (cbCountry.SelectedValue != null)
            {
                addressDTO.CountryId = Convert.ToInt32(cbCountry.SelectedValue);
            }
            if (addressTypeDTO.AddressType == AddressType.NONE && ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ADDRESS_TYPE") == "M")
            {
                lblMessage.Text = MessageContainerList.GetMessage(executionContext, 165).Replace("&1", "Address Type");
                cbType.BackColor = Color.OrangeRed;
                cbType.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtLine1.Text) && ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ADDRESS1") == "M")
            {
                lblMessage.Text = MessageContainerList.GetMessage(executionContext, 165).Replace("&1", "Line1");
                txtLine1.BackColor = Color.OrangeRed;
                txtLine1.Focus();
                return;
            }
            else if (addressDTO.Line1 != txtLine1.Text)
            {
                addressDTO.Line1 = txtLine1.Text;
            }
            if (string.IsNullOrEmpty(txtLine2.Text) && ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ADDRESS2") == "M")
            {
                lblMessage.Text = MessageContainerList.GetMessage(executionContext, 165).Replace("&1", "Line2");
                txtLine2.BackColor = Color.OrangeRed;
                txtLine2.Focus();
                return;
            }
            else if (addressDTO.Line2 != txtLine2.Text)
            {
                addressDTO.Line2 = txtLine2.Text;
            }
            if (string.IsNullOrEmpty(txtLine3.Text) && ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ADDRESS3") == "M")
            {
                lblMessage.Text = MessageContainerList.GetMessage(executionContext, 165).Replace("&1", "Line3");
                txtLine3.BackColor = Color.OrangeRed;
                txtLine3.Focus();
                return;
            }
            else if (addressDTO.Line3 != txtLine3.Text)
            {
                addressDTO.Line3 = txtLine3.Text;
            }
            if (string.IsNullOrEmpty(txtPostalCode.Text) && ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "PIN") == "M")
            {
                lblMessage.Text = MessageContainerList.GetMessage(executionContext, 165).Replace("&1", "Postal Code");
                txtPostalCode.BackColor = Color.OrangeRed;
                txtPostalCode.Focus();
                return;
            }
            else if (addressDTO.PostalCode != txtPostalCode.Text)
            {
                addressDTO.PostalCode = txtPostalCode.Text;
            }
            if (string.IsNullOrEmpty(txtCity.Text) && ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CITY") == "M")
            {
                lblMessage.Text = MessageContainerList.GetMessage(executionContext, 165).Replace("&1", "City");
                txtCity.BackColor = Color.OrangeRed;
                txtCity.Focus();
                return;
            }
            else if (addressDTO.City != txtCity.Text)
            {
                addressDTO.City = txtCity.Text;
            }
            CountryDTO countryDTO = (CountryDTO)cbCountry.SelectedItem;
            if ((countryDTO == null || countryDTO.CountryId == -1) && ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "COUNTRY") == "M")
            {
                lblMessage.Text = MessageContainerList.GetMessage(executionContext, 165).Replace("&1", "COUNTRY");
                cbCountry.BackColor = Color.OrangeRed;
                cbCountry.Focus();
                return;
            }
            if (countryDTO == null)
            {
                countryDTO = new CountryDTO();
            }
            addressDTO.CountryName = countryDTO.CountryName.Equals("SELECT") ? string.Empty : countryDTO.CountryName;
            StateDTO stateDTO = (StateDTO)cbState.SelectedItem;
            if ((stateDTO == null || stateDTO.StateId == -1) && ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "STATE") == "M")
            {
                lblMessage.Text = MessageContainerList.GetMessage(executionContext, 165).Replace("&1", "STATE");
                cbState.BackColor = Color.OrangeRed;
                cbState.Focus();
                return;
            }
            if (stateDTO == null)
            {
                stateDTO = new StateDTO();
            }
            addressDTO.StateName = stateDTO.Description.Equals("SELECT") ? string.Empty : stateDTO.Description;
            addressDTO.IsActive = chkActive.CheckState == CheckState.Checked ? true : false;
            this.DialogResult = DialogResult.OK;
            this.Dispose();
            log.LogMethodExit();
        }

        /// <summary>
        /// Opens the GenericEntityView form for State names  with POS Background and Touch friendly
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStateView_Click(object sender, EventArgs e)
        {
            StateDTO selectedStateDTO = null;
            List<EntityPropertyDefintion> entityPropertyDefintionList = new List<EntityPropertyDefintion>()
                        {
                            new EntityPropertyDefintion("Description", MessageContainerList.GetMessage(utilities.ExecutionContext,"Name"), true ),
                            new EntityPropertyDefintion("State", MessageContainerList.GetMessage(utilities.ExecutionContext,"State"), true),
                        };
            using (GenericEntitySelectionUI<StateDTO> genericEntitySelectionUI = new GenericEntitySelectionUI<StateDTO>(utilities, MessageContainerList.GetMessage(utilities.ExecutionContext, "States"), entityPropertyDefintionList, stateList))
            {
                genericEntitySelectionUI.SetPOSBackGroundColor(this.BackColor);

                if (genericEntitySelectionUI.ShowDialog() == DialogResult.OK)
                {
                    selectedStateDTO = genericEntitySelectionUI.SelectedValue;
                }
            }
            if (selectedStateDTO != null)
            {
                List<StateDTO> stateDTOList = (List<StateDTO>)cbState.DataSource;
                if (stateDTOList != null && stateDTOList.Any())
                {
                    int defaultStateIndex = stateDTOList.FindIndex(x => x.StateId == selectedStateDTO.StateId);
                    cbState.SelectedIndex = (defaultStateIndex == -1 ? 0 : defaultStateIndex);
                }
                else
                {
                    cbState.SelectedIndex = 0; // Default <Select>
                }
            }
        }

        /// <summary>
        /// Opens the GenericEntityView form for Country names with POS Background and Touch friendly
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCountryView_Click(object sender, EventArgs e)
        {
            CountryDTO selectedCountryDTO = null;
            List<EntityPropertyDefintion> entityPropertyDefintionList = new List<EntityPropertyDefintion>()
                        {
                            new EntityPropertyDefintion("CountryName", MessageContainerList.GetMessage(utilities.ExecutionContext,"Name"), true),
                        };
            using (GenericEntitySelectionUI<CountryDTO> genericEntitySelectionUI = new GenericEntitySelectionUI<CountryDTO>(utilities, MessageContainerList.GetMessage(utilities.ExecutionContext, "Country"), entityPropertyDefintionList, countryList))
            {
                genericEntitySelectionUI.SetPOSBackGroundColor(this.BackColor);
                if (genericEntitySelectionUI.ShowDialog() == DialogResult.OK)
                {
                    selectedCountryDTO = genericEntitySelectionUI.SelectedValue;
                }
            }
            if (selectedCountryDTO != null)
            {
                List<CountryDTO> countryDTOList = (List<CountryDTO>)cbCountry.DataSource;
                if (countryDTOList != null && countryDTOList.Any())
                {
                    int defaultCountryIndex = countryDTOList.FindIndex(x => x.CountryId == selectedCountryDTO.CountryId);
                    cbCountry.SelectedIndex = (defaultCountryIndex == -1 ? 0 : defaultCountryIndex);
                }
                else
                {
                    cbCountry.SelectedIndex = 0; // Default <Select>
                }
            }
        }

        /// <summary>
        /// This is called when Size of FlowLayout panel changes 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void flpAddress_SizeChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.Height = flpAddress.Height + 10;
            log.LogMethodExit();
        }

        /// <summary>
        /// This is called when formCustomer address Controls loads adjust its height
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmCustomerAddress_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.Height = flpAddress.Height + 10;
            log.LogMethodExit();
        }
    }
}
