/********************************************************************************************
 * Project Name - Vendor  UI
 * Description  -UI of Vendor 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        26-Aug-2016   Suneetha.S          Created 
 *2.70.2        15-Jul-2019   Girish Kundar       Modified : Added LogMethdEntry() and LogMethodExit()
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;
using Semnox.Parafait.Vendor;

namespace Semnox.Parafait.Inventory
{
    public partial class frmVendor : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        private  Utilities utilities;
        private  VendorDTO vendorDTO = new VendorDTO();

        public frmVendor()
        {
            InitializeComponent();

        }
        private void InitializeVariables()
        {
            vendorFillByToolStripButton.Text = utilities.MessageUtils.getMessage(10353).ToString();
            vendorNameToolStripLabel.Text = utilities.MessageUtils.getMessage(12292).ToString();
            lblErrorEmail.Visible = false;
            lblErrorWebsite.Visible = false;
            txtWebsite.BackColor = Color.White;
            txtMail.BackColor = Color.White;
        }

        public frmVendor(Utilities _Utilities)
        {
            log.LogMethodEntry(_Utilities);
            try
            {
                InitializeComponent();
                utilities = _Utilities;
                utilities.setupDataGridProperties(ref dgvVendor);
                dgvVendor.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                if (utilities.ParafaitEnv.IsCorporate)
                {
                    machineUserContext.SetSiteId(utilities.ParafaitEnv.SiteId);
                }
                else
                {
                    machineUserContext.SetSiteId(-1);
                }
                if (utilities.ParafaitEnv.IsCorporate && utilities.ParafaitEnv.IsMasterSite)
                {
                    lnkPublishToSite.Visible = true;
                }
                else
                {
                    lnkPublishToSite.Visible = false;
                }
                machineUserContext.SetUserId(utilities.ParafaitEnv.LoginID);
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error("Ends-VendorUI(_Utilities) constructor with exception: " + ex.ToString());
                MessageBox.Show(ex.Message);
            }
        }

        private void frmVendor_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender,e);
            PopulateVendor(string.Empty);            
            dgvVendor.Refresh();
            InitializeVariables();
            txtName.Focus();
            utilities.setLanguage(this);
            log.LogMethodExit();
        }

        private void PopulateVendor(string searchBy)

        {
            log.LogMethodEntry(searchBy);
            try
            {
                PopulatePurchaseTax();
                PopulateCountry();
                PopulateState();
                VendorList vendorList = new VendorList(machineUserContext);
                List<KeyValuePair<VendorDTO.SearchByVendorParameters, string>> vendorSearchParams = new List<KeyValuePair<VendorDTO.SearchByVendorParameters, string>>();
                if (searchBy != string.Empty && searchBy == "Name")
                    vendorSearchParams.Add(new KeyValuePair<VendorDTO.SearchByVendorParameters, string>(VendorDTO.SearchByVendorParameters.NAME, vendorNameToolStripTextBox.Text));
                vendorSearchParams.Add(new KeyValuePair<VendorDTO.SearchByVendorParameters, string>(VendorDTO.SearchByVendorParameters.SITEID, machineUserContext.GetSiteId().ToString()));
                List<VendorDTO> vendorListOnDisplay = vendorList.GetAllVendors(vendorSearchParams);
                if (vendorListOnDisplay != null)
                {
                    SortableBindingList<VendorDTO> vendorDTOSortList = new SortableBindingList<VendorDTO>(vendorListOnDisplay);
                    vendorDTOBindingSource.DataSource = vendorDTOSortList;
                }
                else
                    vendorDTOBindingSource.DataSource = new SortableBindingList<VendorDTO>();

                dgvVendor.DataSource = vendorDTOBindingSource;
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.LogMethodExit(ex);
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        ///  2.60        08-04-2019   Girish K       Replaced PurchaseTax 3 tier with Tax 3 Tier
        /// </summary>
        private void PopulatePurchaseTax()
        {
            log.LogMethodEntry();
            try
            {
                TaxList taxList = new TaxList(machineUserContext);
                List<TaxDTO> purchaseTaxDTOList = new List<TaxDTO>();
                List<KeyValuePair<TaxDTO.SearchByTaxParameters, string>> taxDTOSearchParams;
                taxDTOSearchParams = new List<KeyValuePair<TaxDTO.SearchByTaxParameters, string>>();
                taxDTOSearchParams.Add(new KeyValuePair<TaxDTO.SearchByTaxParameters, string>(TaxDTO.SearchByTaxParameters.ACTIVE_FLAG, "1"));
                BindingSource purchaseTaxDTOBindingSource = new BindingSource();
                purchaseTaxDTOList = taxList.GetAllTaxes(taxDTOSearchParams);
               
                if (purchaseTaxDTOList != null)
                {
                    SortableBindingList<TaxDTO> vendorDTOSortList = new SortableBindingList<TaxDTO>(purchaseTaxDTOList);
                    purchaseTaxDTOBindingSource.DataSource = vendorDTOSortList;
                }
                else
                {
                    purchaseTaxDTOList = new List<TaxDTO>();
                    purchaseTaxDTOBindingSource.DataSource = new SortableBindingList<TaxDTO>();

                }
                TaxDTO defaultTax = new TaxDTO();
               // defaultTax.TaxId = 0;
                defaultTax.TaxName = "<SELECT>";
                purchaseTaxDTOList.Insert(0, defaultTax);
                purchaseTaxDTOBindingSource.DataSource = purchaseTaxDTOList;
                ddlPurchaseTax.DataSource = purchaseTaxDTOBindingSource;
                ddlPurchaseTax.ValueMember = "TaxId";
                ddlPurchaseTax.DisplayMember = "TaxName";

                log.LogMethodExit();
            }
            catch(Exception ex)
            {
                log.LogMethodExit(ex);
                MessageBox.Show(ex.Message);
            }
        }

        private void vendorFillByToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                PopulateVendor("Name");
            }
            catch (System.Exception ex)
            {
                log.LogMethodExit(ex);
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void refreshDgv()
        {
            log.LogMethodEntry();
            PopulateVendor(string.Empty);
            dgvVendor.Refresh();
            log.LogMethodExit();
        }

        private void emptyAllFields()
        {
            log.LogMethodEntry();
            txtName.Text = string.Empty;
            txtWebsite.Text = string.Empty;
            txtAddressline1.Text = string.Empty;
            txtaddressline2.Text = string.Empty;
            txtCity.Text = string.Empty;
            //txtState.Text = string.Empty;
            ddlState.SelectedValue = -1;
            txtZip.Text = string.Empty;
            //ddlCountry.Text = "India";
            ddlCountry.SelectedIndex = 1;
            rtbAddressremarks.Text = string.Empty;
            rtbRemarks.Text = string.Empty;
            txtContactname.Text = string.Empty;
            txtFax.Text = string.Empty;
            txtPhone.Text = string.Empty;
            txtMail.Text = string.Empty;
            txtName.Focus();
            lblVendorid.Text = string.Empty;
            txtTaxRegistrationNo.Text = string.Empty;
            txtVendorCode.Text = string.Empty;
            ddlIsactive.Text = "Y";
            ddlPurchaseTax.SelectedValue = -1;
            txtVendorMarkup.Text = string.Empty;
            InitializeVariables();
            log.LogMethodExit();
        }

        protected void dgvVendor_BindingSourceAddNew(object sender, AddingNewEventArgs e)
        {
            log.LogMethodEntry(sender,e);
            try
            {
                if (dgvVendor.Rows.Count == vendorDTOBindingSource.Count)
                {
                    vendorDTOBindingSource.RemoveAt(vendorDTOBindingSource.Count - 1);
                }
                log.Debug("Ends-dgvVendor_BindingSourceAddNew() Event.");
            }
            catch (Exception ex)
            {
                log.LogMethodExit(ex);
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender,e);
            try
            {
                if (txtName.Text == string.Empty)
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(906), utilities.MessageUtils.getMessage("Insert / Update Vendor"));
                    txtName.Focus(); 
                }
                else if (!requiredFieldsFilled())
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(907),utilities.MessageUtils.getMessage("Insert / Update Vendor"));
                    txtWebsite.Focus(); 
                }
                else
                {
		            vendorDTO.Name = txtName.Text;
		            vendorDTO.Address1 = txtAddressline1.Text;
		            vendorDTO.Address2 = txtaddressline2.Text;
		            vendorDTO.AddressRemarks = rtbAddressremarks.Text;
		            vendorDTO.City = txtCity.Text;
		            vendorDTO.ContactName = txtContactname.Text;
		            vendorDTO.Website = txtWebsite.Text;
		            vendorDTO.Phone = txtPhone.Text;
		            vendorDTO.Fax = txtFax.Text;
		            vendorDTO.Website = txtWebsite.Text;
		            vendorDTO.State = ddlState.Text;
                    vendorDTO.StateId = Convert.ToInt32(ddlState.SelectedValue);
                    vendorDTO.PostalCode = txtZip.Text;
		            vendorDTO.Country = ddlCountry.Text;
                    vendorDTO.CountryId = Convert.ToInt32(ddlCountry.SelectedValue);
                    vendorDTO.DefaultPaymentTermsId = -1;
		            vendorDTO.Remarks = rtbRemarks.Text;
		            vendorDTO.TaxRegistrationNumber = txtTaxRegistrationNo.Text;
		            vendorDTO.Email = txtMail.Text;
		            vendorDTO.IsActive = ddlIsactive.Text == "Y";
                    vendorDTO.VendorCode = txtVendorCode.Text;
		            //vendorDTO.TaxId = Convert.ToInt32(ddlPurchaseTax.SelectedValue);
		            vendorDTO.VendorMarkupPercent = (txtVendorMarkup.Text.Trim() != string.Empty ? Convert.ToDouble(txtVendorMarkup.Text) : double.NaN);
                    vendorDTO.PurchaseTaxId = Convert.ToInt32(ddlPurchaseTax.SelectedValue);
                    Vendor.Vendor vendor = new Vendor.Vendor(machineUserContext,vendorDTO);
		            vendor.Save();
                    int vendorIdModified = vendorDTO.VendorId;
                    MessageBox.Show(utilities.MessageUtils.getMessage(911),utilities.MessageUtils.getMessage("Save Vendor"));
                    emptyAllFields();
                    dgvVendor.ClearSelection();
                    refreshDgv();

                    DataGridViewRow row = dgvVendor.Rows.Cast<DataGridViewRow>()
                                         .Where(r => r.Cells["VendorId"].Value.ToString().Equals(vendorIdModified.ToString()))
                                          .First();
                    if (row.Index >= 0)
                    {
                        dgvVendor.Rows[row.Index].Selected = true;
                        dgvVendor.CurrentCell = dgvVendor.Rows[row.Index].Cells["vendorname"];
                        dgvVendor.Refresh();
                    } 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                log.LogMethodExit(ex);
            }
        }

        private bool requiredFieldsFilled()
        {
            log.LogMethodEntry();
            if (txtName.Text == string.Empty)
            {
                log.LogMethodEntry(false);
                return false;
            }
            log.LogMethodExit(true);
            return true;
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender,e);
            this.Dispose();
            log.LogMethodExit();
        }

        private void btnDuplicate_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (vendorDTOBindingSource.Current != null &&
                 vendorDTOBindingSource.Current is VendorDTO)
            {
                vendorDTO = vendorDTOBindingSource.Current as VendorDTO;
                vendorDTO.VendorId = -1;
                transferValues(vendorDTO);
                lblVendorid.Text = string.Empty;
                txtName.Text = string.Empty;
                txtName.Focus();
            }
            log.LogMethodExit();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            vendorDTO = new VendorDTO();
            emptyAllFields();
            txtName.Focus();
            log.LogMethodExit();
        }

        public void transferValues (VendorDTO vendorDTO)
        {
            log.LogMethodEntry(vendorDTO);
            lblVendorid.Text = vendorDTO.VendorId.ToString();
            txtName.Text = vendorDTO.Name;
            rtbRemarks.Text = vendorDTO.Remarks;
            txtAddressline1.Text = vendorDTO.Address1;
            txtaddressline2.Text = vendorDTO.Address2;
            txtCity.Text = vendorDTO.City;
            //txtState.Text = vendorDTO.State;
            ddlCountry.SelectedValue = vendorDTO.CountryId;
            ddlState.SelectedValue = vendorDTO.StateId;
            //ddlCountry.Text = vendorDTO.Country;           
            txtZip.Text = vendorDTO.PostalCode;
            rtbAddressremarks.Text = vendorDTO.AddressRemarks;
            txtContactname.Text = vendorDTO.ContactName;
            txtPhone.Text = vendorDTO.Phone;
            txtFax.Text = vendorDTO.Fax;
            txtMail.Text = vendorDTO.Email;
            ddlIsactive.Text = vendorDTO.IsActive ? "Y" : "N";
            txtWebsite.Text = vendorDTO.Website;
            txtTaxRegistrationNo.Text = vendorDTO.TaxRegistrationNumber;
            txtVendorCode.Text = vendorDTO.VendorCode;
            txtVendorMarkup.Text = (!Double.IsNaN(vendorDTO.VendorMarkupPercent) ? vendorDTO.VendorMarkupPercent.ToString() : string.Empty);
            ddlPurchaseTax.SelectedValue = vendorDTO.PurchaseTaxId;
            InitializeVariables();
            log.LogMethodExit();
        }

        private void vendorDTOBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (vendorDTOBindingSource.Current != null &&
                vendorDTOBindingSource.Current is VendorDTO)
            {
                vendorDTO = vendorDTOBindingSource.Current as VendorDTO;
                transferValues(vendorDTO);                
            }
            log.LogMethodExit();
        }

        private void txtWebsite_Validating(object sender, CancelEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (txtWebsite.Text != string.Empty)
            {
                try
                {
                    Exception ex = new Exception();

                    string strRegex = "^(https?://)"
                                        + "?(([0-9a-z_!~*'().&=+$%-]+: )?[0-9a-z_!~*'().&=+$%-]+@)?" //user@
                                        + @"(([0-9]{1,3}\.){3}[0-9]{1,3}" // IP- 199.194.52.184
                                        + "|" // allows either IP or domain
                                        + @"([0-9a-z_!~*'()-]+\.)*" // tertiary domain(s)- www.
                                        + @"([0-9a-z][0-9a-z-]{0,61})?[0-9a-z]\." // second level domain
                                        + "[a-z]{2,6})" // first level domain- .com or .museum
                                        + "(:[0-9]{1,4})?" // port number- :80
                                        + "((/?)|" // a slash isn't required if there is no file name
                                        + "(/[0-9a-z_!~*'().;?:@&=+$,%#-]+)+/?)$";
                    Regex re = new Regex(strRegex);
                    if (!re.IsMatch(txtWebsite.Text))
                        throw ex;

                    txtWebsite.BackColor = Color.White;
                    lblErrorWebsite.Visible = false;
                }
                catch
                {
                    txtWebsite.BackColor = Color.Yellow;
                    lblErrorWebsite.Visible = true;
                    e.Cancel = true;
                }
            }
            log.LogMethodExit();
        }

        private void txtMail_Validating(object sender, CancelEventArgs e)
        {
            log.LogMethodEntry(sender,e);
            try
            {
                Exception ex = new Exception();
                String inputEmail = txtMail.Text;
                string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                      @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                      @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
                Regex re = new Regex(strRegex);
                if (!string.IsNullOrEmpty(inputEmail) && !re.IsMatch(inputEmail))
                    throw ex;
                txtMail.BackColor = Color.White;
                lblErrorEmail.Visible = false;
            }
            catch
            {
                txtMail.BackColor = Color.Yellow;
                lblErrorEmail.Visible = true;
                e.Cancel = true;

            }
            log.LogMethodExit();
        }

        private void txtPhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry(sender,e);
            if (!Char.IsNumber(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
            log.LogMethodExit();
        }

        private void txtFax_KeyPress(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry(sender,e);
            if (!Char.IsNumber(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
            log.LogMethodExit();
        }

        private void txtZip_KeyPress(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry(sender,e);
            if (!Char.IsNumber(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
            log.LogMethodExit();
        }

        private void vendorNameToolStripTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry(sender,e);
            if (e.KeyChar == 13 && vendorNameToolStripTextBox.Focused)
                vendorFillByToolStripButton.PerformClick();
            log.LogMethodExit();
        }
        private void lnkPublishToSite_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry(sender,e);
            Publish.PublishUI publishUI;

            if (!string.IsNullOrEmpty(lblVendorid.Text) && Convert.ToInt32(lblVendorid.Text) >= 0)
            {
                publishUI = new Publish.PublishUI(utilities, Convert.ToInt32(lblVendorid.Text), "Vendor", txtName.Text);
                publishUI.ShowDialog();
            }
            log.LogMethodExit();
        }

        private void PopulateCountry()
        {
            log.LogMethodEntry();
            try
            {
                CountryDTOList countryDTOListClass = new CountryDTOList(machineUserContext);
                //List<KeyValuePair<CountryDTO.SearchByParameters, string>> searchParameters;
                //searchParameters = new List<KeyValuePair<CountryDTO.SearchByParameters, string>>();
                //searchParameters.Add(new KeyValuePair<CountryDTO.SearchByParameters, string>(CountryDTO.SearchByParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                CountryDTO countryDTOForSearch = new CountryDTO(); 
                List<CountryDTO> countryDTOList = countryDTOListClass.GetCountryDTOList(countryDTOForSearch);
                if (countryDTOList != null)
                {
                    SortableBindingList<CountryDTO> countryDTOSortList = new SortableBindingList<CountryDTO>(countryDTOList);
                    countryDTOBindingSource.DataSource = countryDTOSortList;
                }
                else
                {
                    countryDTOList = new List<CountryDTO>();
                    countryDTOBindingSource.DataSource = new SortableBindingList<CountryDTO>();

                }
                countryDTOList.Insert(0, new CountryDTO());
                countryDTOBindingSource.DataSource = countryDTOList;
                ddlCountry.DataSource = countryDTOBindingSource;
                ddlCountry.ValueMember = "CountryId";
                ddlCountry.DisplayMember = "CountryName";
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error("Ends-PopulateCountry() method.");
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit();
        }

        private void PopulateState()
        {
            log.LogMethodExit();
            try
            {
                StateDTOList stateDTOListClass = new StateDTOList(machineUserContext);
                //List<KeyValuePair<StateDTO.SearchByParameters, string>> searchParameters;
                //searchParameters = new List<KeyValuePair<StateDTO.SearchByParameters, string>>();
                //searchParameters.Add(new KeyValuePair<StateDTO.SearchByParameters, string>(StateDTO.SearchByParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                StateDTO stateDTOForSearch = new StateDTO();
                stateDTOForSearch.CountryId = Convert.ToInt32(ddlCountry.SelectedValue);
                List<StateDTO> stateDTOList = stateDTOListClass.GetStateDTOList(stateDTOForSearch);
                if (stateDTOList != null)
                {
                    SortableBindingList<StateDTO> stateDTOSortList = new SortableBindingList<StateDTO>(stateDTOList);
                    stateDTOBindingSource.DataSource = stateDTOSortList;
                }
                else
                {
                    stateDTOList = new List<StateDTO>();
                    stateDTOBindingSource.DataSource = new SortableBindingList<StateDTO>();

                }
                stateDTOList.Insert(0, new StateDTO());
                stateDTOBindingSource.DataSource = stateDTOList;
                ddlState.DataSource = stateDTOBindingSource;
                ddlState.ValueMember = "StateId";
                ddlState.DisplayMember = "State";
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.LogMethodExit(ex);
                log.Error("Ends-PopulateState() method.");
                MessageBox.Show(ex.Message);
            }
        }

        private void DdlCountry_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            log.LogMethodEntry(sender,e);
            PopulateState();
            ddlState.SelectedValue = -1;
            log.LogMethodExit();
        }        
    }
}
