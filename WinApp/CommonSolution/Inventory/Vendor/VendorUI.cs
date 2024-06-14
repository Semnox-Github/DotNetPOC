using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Parafait.Inventory;
using Semnox.Parafait.logging;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Product;
using Semnox.Parafait.Vendor;

namespace Semnox.Parafait.Inventory
{
    public partial class VendorUI : Form
    {
        Parafait.logging.Logger log = new Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        Utilities utilities;
        BindingSource vendorListBS;

        public VendorUI()
        {

        }

        public VendorUI(Utilities _Utilities)
        {
            log.Debug("Starts-VendorUI(_Utilities) constructor.");
            try
            {
                InitializeComponent();
                utilities = _Utilities;
                utilities.setupDataGridProperties(ref dgvVendor);
                if (utilities.ParafaitEnv.IsCorporate)
                {
                    machineUserContext.SetSiteId(utilities.ParafaitEnv.SiteId);
                }
                else
                {
                    machineUserContext.SetSiteId(-1);
                }
                machineUserContext.SetUserId(utilities.ParafaitEnv.LoginID);
                log.Debug("Ends-VendorUI(_Utilities) constructor.");
            }
            catch (Exception ex)
            {
                log.Error("Ends-VendorUI(_Utilities) constructor with exception: " + ex.ToString());
                MessageBox.Show(ex.Message);
            }
        }

        private void VendorUI_Load(object sender, EventArgs e)
        {
            log.Debug("Starts-VendorUI_Load() method.");
            PopulateVendor();
            log.Debug("Starts-VendorUI_Load() method.");
        }

        private void PopulateVendor()
        {
            log.Debug("Starts-PopulateVendor() method.");
            try
            {
                PopulatePurchaseTax();
                VendorList vendorList = new VendorList();
                List<KeyValuePair<VendorDTO.SearchByVendorParameters, string>> vendorSearchParams = new List<KeyValuePair<VendorDTO.SearchByVendorParameters, string>>();
                List<VendorDTO> vendorListOnDisplay = vendorList.GetAllVendors(vendorSearchParams);
                vendorListBS = new BindingSource();
                if (vendorListOnDisplay != null)
                    vendorListBS.DataSource = new SortableBindingList<VendorDTO>(vendorListOnDisplay);
                else
                    vendorListBS.DataSource = new SortableBindingList<VendorDTO>();
                vendorListBS.AddingNew += dgvVendor_BindingSourceAddNew;
                dgvVendor.DataSource = vendorListBS;
                log.Debug("Ends-PopulateVendor() method.");
            }
            catch (Exception ex)
            {
                log.Debug("Ends-PopulateVendor() method.");
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        ///  2.60        08-04-2019   Girish K       Replaced PurchaseTax 3 tier with Tax 3 Tier
        /// </summary>
        private void PopulatePurchaseTax()
        {
            log.LogMethodEntry("Starts-PopulatePurchaseTax() method.");
            try
            {
                TaxList purchaseTaxList = new TaxList();
                List<TaxDTO> purchaseTaxDTOList = new List<TaxDTO>();
                List<KeyValuePair<TaxDTO.SearchByTaxParameters, string>> purchaseTaxSearchParams = new List<KeyValuePair<TaxDTO.SearchByTaxParameters, string>>();
                purchaseTaxDTOList = purchaseTaxList.GetAllTaxes(purchaseTaxSearchParams);

                purchaseTaxDTOList.Insert(0, new TaxDTO());
                purchaseTaxDTOList[0].TaxName = "<Select>";

                taxIdDataGridViewTextBoxColumn.DataSource = purchaseTaxDTOList;
                taxIdDataGridViewTextBoxColumn.ValueMember = "TaxId";
                taxIdDataGridViewTextBoxColumn.DisplayMember = "TaxName";
                log.LogMethodExit("Ends-PopulatePurchaseTax() method.");
            }
            catch(Exception ex)
            {
                log.LogMethodExit("Ends-PopulatePurchaseTax() method.");
                MessageBox.Show(ex.Message);
            }
        }

        protected void dgvVendor_BindingSourceAddNew(object sender, AddingNewEventArgs e)
        {
            log.Debug("Starts-dgvVendor_BindingSourceAddNew() Event.");
            try
            {
                if (dgvVendor.Rows.Count == vendorListBS.Count)
                {
                    vendorListBS.RemoveAt(vendorListBS.Count - 1);
                }
                log.Debug("Ends-dgvVendor_BindingSourceAddNew() Event.");
            }
            catch (Exception ex)
            {
                log.Debug("Ends-dgvVendor_BindingSourceAddNew() Event.");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnSave_Click() Event.");
            try
            {
                BindingSource vendorListBS = (BindingSource)dgvVendor.DataSource;
                var vendorListOnDisplay = (SortableBindingList<VendorDTO>)vendorListBS.DataSource;
                if (vendorListOnDisplay.Count > 0)
                {
                    foreach (VendorDTO vendorDTO in vendorListOnDisplay)
                    {
                        if (vendorDTO.IsChanged)
                        {
                            if (string.IsNullOrEmpty(vendorDTO.Name))
                            {
                                MessageBox.Show("Please enter the name.");
                                return;
                            }
                        }

                        Vendor.Vendor vendor = new Vendor.Vendor(vendorDTO);
                        vendor.Save();
                    }
                    PopulateVendor();
                }
                else
                    MessageBox.Show(utilities.MessageUtils.getMessage(371));
                log.Debug("Ends-btnSave_Click() Event.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                log.Debug("Ends-btnSave_Click() Event.");
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            PopulateVendor();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnDelete_Click() event.");
            try
            {
                if (this.dgvVendor.SelectedRows.Count <= 0 && this.dgvVendor.SelectedCells.Count <= 0)
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(959));
                    log.Debug("Ends-btnDelete_Click() event by \"No rows selected. Please select the rows you want to delete and press delete..\" message ");
                    return;
                }
                bool rowsDeleted = false;
                bool confirmDelete = false;
                if (this.dgvVendor.SelectedCells.Count > 0)
                {
                    foreach (DataGridViewCell cell in this.dgvVendor.SelectedCells)
                    {
                        dgvVendor.Rows[cell.RowIndex].Selected = true;
                    }
                }
                foreach (DataGridViewRow vendorRow in this.dgvVendor.SelectedRows)
                {
                    if (vendorRow.Cells[0].Value != null)
                    {
                        if (Convert.ToInt32(vendorRow.Cells[0].Value) <= 0)
                        {
                            dgvVendor.Rows.RemoveAt(vendorRow.Index);
                            rowsDeleted = true;
                        }
                        else
                        {
                            if (confirmDelete || (MessageBox.Show(utilities.MessageUtils.getMessage(958), "Confirm Inactvation.", MessageBoxButtons.YesNo) == DialogResult.Yes))
                            {
                                confirmDelete = true;
                                BindingSource vendorDTOListDTOBS = (BindingSource)dgvVendor.DataSource;
                                var vendorDTOList = (SortableBindingList<VendorDTO>)vendorDTOListDTOBS.DataSource;
                                VendorDTO vendorDTO = vendorDTOList[vendorRow.Index];
                                vendorDTO.IsActive = "N";
                                Vendor.Vendor vendor = new Vendor.Vendor(vendorDTO);
                                vendor.Save();
                            }
                        }
                    }
                }
                if (rowsDeleted == true)
                    MessageBox.Show(utilities.MessageUtils.getMessage(957));
                PopulateVendor();
                log.Debug("Ends-btnDelete_Click() event.");
            }
            catch (Exception ex)
            {
                log.Error("Ends-btnDelete_Click() event with exception: " + ex.ToString());
                MessageBox.Show("Delete failed!!!.\n Error: " + ex.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
