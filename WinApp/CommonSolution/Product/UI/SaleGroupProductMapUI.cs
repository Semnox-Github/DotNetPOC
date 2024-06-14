/********************************************************************************************
 * Project Name - Sale Group Product Map UI
 * Description  - UI for Sale Group Product Map
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 ********************************************************************************************* 
* 2.80        28-Jun-2020   Deeksha        Modified to Make Product module read only in Windows Management Studio.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Core.GenericUtilities;

namespace Semnox.Parafait.Product
{
    public partial class SaleGroupProductMapUI : Form
    {
        Utilities utilities;
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        BindingSource saleGroupProductMapListBS;
        List<ProductsDTO> productsDTOList;
        List<SalesOfferGroupDTO> salesOfferGroupListOnDisplay;
        bool loadSearch = true;
        private ManagementStudioSwitch managementStudioSwitch;
        public SaleGroupProductMapUI(Utilities _Utilities)
        {
            log.LogMethodEntry(_Utilities);
            InitializeComponent();
            RegisterKeyDownHandlers(this);//Modification on 22-Feb-2016 to hide the search grid control
            utilities = _Utilities;
            utilities.setupDataGridProperties(ref dgvDisplayData);

            if (utilities.ParafaitEnv.IsCorporate)
            {
                machineUserContext.SetSiteId(utilities.ParafaitEnv.SiteId);
            }
            else
            {
                machineUserContext.SetSiteId(-1);
            }
            machineUserContext.SetUserId(utilities.ParafaitEnv.LoginID);
            utilities.setLanguage(this);
            LoadSalesOfferGroup();
            LoadSalesOfferGroupSearch();
            LoadProducts();
            LoadProductSearch();
            PopulateSaleGroupProductMapsGrid();
            managementStudioSwitch = new ManagementStudioSwitch(machineUserContext);
            UpdateUIElements();
            log.LogMethodEntry();
        }

        private void LoadSalesOfferGroup()
        {
            try
            {
                log.LogMethodEntry();
                SalesOfferGroupList salesOfferGroupList = new SalesOfferGroupList(machineUserContext);
                List<KeyValuePair<SalesOfferGroupDTO.SearchBySalesOfferGroupParameters, string>> salesOfferGroupSearchParams = new List<KeyValuePair<SalesOfferGroupDTO.SearchBySalesOfferGroupParameters, string>>();
                salesOfferGroupSearchParams.Add(new KeyValuePair<SalesOfferGroupDTO.SearchBySalesOfferGroupParameters, string>(SalesOfferGroupDTO.SearchBySalesOfferGroupParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                salesOfferGroupListOnDisplay = salesOfferGroupList.GetAllSalesOfferGroups(salesOfferGroupSearchParams);
                if (salesOfferGroupListOnDisplay == null)
                {
                    salesOfferGroupListOnDisplay = new List<SalesOfferGroupDTO>();
                }
                salesOfferGroupListOnDisplay.Insert(0, new SalesOfferGroupDTO());
                salesOfferGroupListOnDisplay[0].Name = "<SELECT>";
                BindingSource bs = new BindingSource();
                bs.DataSource = salesOfferGroupListOnDisplay;
                cmbGroup.DataSource = salesOfferGroupListOnDisplay;
                cmbGroup.DisplayMember = "Name";
                cmbGroup.ValueMember = "SaleGroupId";

                saleGroupIdDataGridViewTextBoxColumn.DataSource = bs;
                saleGroupIdDataGridViewTextBoxColumn.DisplayMember = "Name";
                saleGroupIdDataGridViewTextBoxColumn.ValueMember = "SaleGroupId";
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in loading sales group " + ex.Message);
            }
        }

        private void LoadProducts()
        {
            try
            {
                log.LogMethodEntry();
                Products products = new Products();
                ProductsFilterParams productsFilterParams = new ProductsFilterParams();
                productsFilterParams.SiteId = machineUserContext.GetSiteId();
                productsFilterParams.POSMachineId = utilities.ParafaitEnv.POSMachineId;
                productsDTOList = products.GetProductDTOList(productsFilterParams);
                if (productsDTOList == null)
                {
                    productsDTOList = new List<ProductsDTO>();
                }
                productsDTOList.Insert(0, new ProductsDTO());
                productsDTOList[0].ProductName = "<SELECT>";
                BindingSource bs = new BindingSource();
                bs.DataSource = productsDTOList;
                cmbProduct.DataSource = productsDTOList;
                cmbProduct.DisplayMember = "ProductName";
                cmbProduct.ValueMember = "ProductId";

                productIdDataGridViewTextBoxColumn.DataSource = bs;
                productIdDataGridViewTextBoxColumn.DisplayMember = "ProductName";
                productIdDataGridViewTextBoxColumn.ValueMember = "ProductId";
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in loading products " + ex.Message);
            }
        }
        private void PopulateSaleGroupProductMapsGrid()
        {
            log.LogMethodEntry();
            SaleGroupProductMapList saleGroupProductMapList = new SaleGroupProductMapList(machineUserContext);
            List<KeyValuePair<SaleGroupProductMapDTO.SearchBySaleGroupProductMapParameters, string>> saleGroupProductMapSearchParams = new List<KeyValuePair<SaleGroupProductMapDTO.SearchBySaleGroupProductMapParameters, string>>();
            if (chbShowActiveEntries.Checked)
            {
                saleGroupProductMapSearchParams.Add(new KeyValuePair<SaleGroupProductMapDTO.SearchBySaleGroupProductMapParameters, string>(SaleGroupProductMapDTO.SearchBySaleGroupProductMapParameters.IS_ACTIVE, "1"));
            }
            saleGroupProductMapSearchParams.Add(new KeyValuePair<SaleGroupProductMapDTO.SearchBySaleGroupProductMapParameters, string>(SaleGroupProductMapDTO.SearchBySaleGroupProductMapParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            if (cmbGroup.SelectedValue != null && !cmbGroup.SelectedValue.ToString().Equals("-1"))
            {
                saleGroupProductMapSearchParams.Add(new KeyValuePair<SaleGroupProductMapDTO.SearchBySaleGroupProductMapParameters, string>(SaleGroupProductMapDTO.SearchBySaleGroupProductMapParameters.SALE_GROUP_ID, cmbGroup.SelectedValue.ToString()));
            }
            if (cmbProduct.SelectedValue != null && !cmbProduct.SelectedValue.ToString().Equals("-1"))
            {
                saleGroupProductMapSearchParams.Add(new KeyValuePair<SaleGroupProductMapDTO.SearchBySaleGroupProductMapParameters, string>(SaleGroupProductMapDTO.SearchBySaleGroupProductMapParameters.PRODUCT_ID, cmbProduct.SelectedValue.ToString()));
            }
            List<SaleGroupProductMapDTO> saleGroupProductMapListOnDisplay = saleGroupProductMapList.GetAllSaleGroupProductMaps(saleGroupProductMapSearchParams);
            saleGroupProductMapListBS = new BindingSource();
            if (saleGroupProductMapListOnDisplay != null)
            {
                SortableBindingList<SaleGroupProductMapDTO> saleGroupProductMapDTOSortList = new SortableBindingList<SaleGroupProductMapDTO>(saleGroupProductMapListOnDisplay);
                saleGroupProductMapListBS.DataSource = saleGroupProductMapDTOSortList;
            }
            else
                saleGroupProductMapListBS.DataSource = new SortableBindingList<SaleGroupProductMapDTO>();
            saleGroupProductMapListBS.AddingNew += dgvDisplayData_BindingSourceAddNew;
            dgvDisplayData.DataSource = saleGroupProductMapListBS;
            dgvDisplayData.DataError += new DataGridViewDataErrorEventHandler(dgvDisplayData_ComboDataError);
            log.LogMethodExit();
        }

        void dgvDisplayData_BindingSourceAddNew(object sender, AddingNewEventArgs e)
        {
            log.LogMethodEntry();
            if (dgvDisplayData.Rows.Count == saleGroupProductMapListBS.Count)
            {
                saleGroupProductMapListBS.RemoveAt(saleGroupProductMapListBS.Count - 1);
            }
            log.LogMethodExit();
        }

        void dgvDisplayData_ComboDataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry();

            if (e.ColumnIndex == dgvDisplayData.Columns["productIdDataGridViewTextBoxColumn"].Index)
            {
                if (productsDTOList != null)
                    dgvDisplayData.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = productsDTOList[0].ProductId;
            }
            else if (e.ColumnIndex == dgvDisplayData.Columns["saleGroupIdDataGridViewTextBoxColumn"].Index)
            {
                if (salesOfferGroupListOnDisplay != null)
                    dgvDisplayData.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = salesOfferGroupListOnDisplay[0].SaleGroupId;
            }
            log.LogMethodExit();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            BindingSource saleGroupProductMapListBS = (BindingSource)dgvDisplayData.DataSource;
            var saleGroupProductMapListOnDisplay = (SortableBindingList<SaleGroupProductMapDTO>)saleGroupProductMapListBS.DataSource;
            if (saleGroupProductMapListOnDisplay.Count > 0)
            {
                foreach (SaleGroupProductMapDTO saleGroupProductMapDTO in saleGroupProductMapListOnDisplay)
                {
                    if (saleGroupProductMapDTO.ProductId == -1)
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage(940));
                        return;
                    }
                    if (saleGroupProductMapDTO.SaleGroupId == -1)
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage(1251));
                        return;
                    }
                    if (saleGroupProductMapDTO.SequenceId == -1)
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage(1252));
                        return;
                    }
                    SaleGroupProductMap saleGroupProductMap = new SaleGroupProductMap(machineUserContext, saleGroupProductMapDTO);
                    saleGroupProductMap.Save(null);
                }
                PopulateSaleGroupProductMapsGrid();
            }
            else
                MessageBox.Show(utilities.MessageUtils.getMessage(371));
            log.LogMethodExit();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            cmbProduct.SelectedValue = cmbGroup.SelectedValue = -1;
            chbShowActiveEntries.Checked = true;
            PopulateSaleGroupProductMapsGrid();
            log.LogMethodExit();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (this.dgvDisplayData.SelectedRows.Count <= 0 && this.dgvDisplayData.SelectedCells.Count <= 0)
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(959));
                log.Debug("Ends-saleGroupProductMapDeleteBtn_Click() event by showing \"No rows selected. Please select the rows you want to delete and press delete..\" message");
                return;
            }
            bool rowsDeleted = false;
            bool confirmDelete = false;
            if (this.dgvDisplayData.SelectedCells.Count > 0)
            {
                foreach (DataGridViewCell cell in this.dgvDisplayData.SelectedCells)
                {
                    dgvDisplayData.Rows[cell.RowIndex].Selected = true;
                }
            }
            foreach (DataGridViewRow saleGroupProductMapRow in this.dgvDisplayData.SelectedRows)
            {
                if (saleGroupProductMapRow.Cells[0].Value == null)
                {
                    return;
                }
                if (Convert.ToInt32(saleGroupProductMapRow.Cells[0].Value.ToString()) <= 0)
                {
                    dgvDisplayData.Rows.RemoveAt(saleGroupProductMapRow.Index);
                    rowsDeleted = true;
                }
                else
                {
                    if (confirmDelete || (MessageBox.Show(utilities.MessageUtils.getMessage(958), "Confirm Inactvation.", MessageBoxButtons.YesNo) == DialogResult.Yes))
                    {
                        confirmDelete = true;
                        BindingSource saleGroupProductMapDTOListDTOBS = (BindingSource)dgvDisplayData.DataSource;
                        var saleGroupProductMapDTOList = (SortableBindingList<SaleGroupProductMapDTO>)saleGroupProductMapDTOListDTOBS.DataSource;
                        SaleGroupProductMapDTO saleGroupProductMapDTO = saleGroupProductMapDTOList[saleGroupProductMapRow.Index];
                        saleGroupProductMapDTO.IsActive = false;
                        SaleGroupProductMap saleGroupProductMap = new SaleGroupProductMap(machineUserContext, saleGroupProductMapDTO);
                        saleGroupProductMap.Save(null);
                    }
                }
            }
            if (rowsDeleted == true)
                MessageBox.Show(utilities.MessageUtils.getMessage(957));
            PopulateSaleGroupProductMapsGrid();
            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.Close();
            log.LogMethodExit();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            txtGroupSearch.Text = cmbGroup.Text;
            txtProductSearch.Text = cmbProduct.Text;
            PopulateSaleGroupProductMapsGrid();
            log.LogMethodExit();
        }

        private void txtGroupSearch_TextChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (loadSearch)
            {
                if (txtGroupSearch.Text.Length > 0)
                {
                    if (salesOfferGroupListOnDisplay != null)
                    {
                        List<SalesOfferGroupDTO> salesOfferGroupDTOList = salesOfferGroupListOnDisplay.Where(x => (bool)((string.IsNullOrEmpty(x.Name) ? "" : x.Name.ToLower()).Contains(txtGroupSearch.Text.ToLower()))).ToList<SalesOfferGroupDTO>();
                        if (salesOfferGroupDTOList.Count > 0)
                        {
                            dgvGroupSearch.Visible = true;
                            dgvGroupSearch.DataSource = salesOfferGroupDTOList;
                        }
                        else
                        {
                            dgvGroupSearch.Visible = false;
                        }
                    }
                }
                else
                {
                    cmbGroup.Text = "";
                    dgvGroupSearch.Visible = false;
                }
            }
            else
            {
                loadSearch = true;
            }
            log.LogMethodExit();
        }


        private void cmbGroup_SelectedValueChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (cmbGroup.Text.Equals("Semnox.Parafait.Product.SalesOfferGroupDTO"))
            {
                txtGroupSearch.Text = "";
            }
            else
            {
                txtGroupSearch.Text = cmbGroup.Text;
            }
            loadSearch = false;
            log.LogMethodExit();
        }

        private void dgvGroupSearch_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                txtGroupSearch.Text = dgvGroupSearch.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                cmbGroup.Text = txtGroupSearch.Text;
                dgvGroupSearch.Visible = false;
            }
            catch { }
            log.LogMethodExit();
        }

        /// <summary>
        /// To fix the issue hiding the List on click of any controls in the form
        /// </summary>
        /// <param name="control"></param>
        private void RegisterKeyDownHandlers(Control control)//Modification on 22-Feb-2016 to hide the search grid control
        {
            log.LogMethodEntry(control);
            foreach (Control ctl in control.Controls)
            {
                ctl.Click += MyKeyPressEventHandler;
                RegisterKeyDownHandlers(ctl);
            }
            log.LogMethodExit();
        }//Ends modification on 22-Feb-2016 to hide the search grid control

        public void MyKeyPressEventHandler(Object sender, EventArgs e)//Modification on 22-Feb-2016 to hide the search grid control
        {
            log.LogMethodEntry();
            dgvGroupSearch.Visible = dgvProductSearch.Visible = false;
            loadSearch = false;
            log.LogMethodExit();
        }//Ends modification on 22-Feb-2016 to hide the search grid control

        /// <summary>
        /// Loads Search Records
        /// </summary>
        private void LoadSalesOfferGroupSearch()
        {
            log.LogMethodEntry();

            BindingSource bindingSource = new BindingSource();
            bindingSource.DataSource = salesOfferGroupListOnDisplay;
            dgvGroupSearch.DataSource = bindingSource;
            for (int i = 0; i < dgvGroupSearch.Columns.Count; i++)
            {
                if (!dgvGroupSearch.Columns[i].Name.Equals("Name"))
                {
                    dgvGroupSearch.Columns[i].Visible = false;
                }
                else
                {
                    dgvGroupSearch.Columns[i].Width = dgvGroupSearch.Width;
                }
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Loads Search Records
        /// </summary>
        private void LoadProductSearch()
        {
            log.LogMethodEntry();

            BindingSource bindingSource = new BindingSource();
            bindingSource.DataSource = productsDTOList;
            dgvProductSearch.DataSource = bindingSource;
            for (int i = 0; i < dgvProductSearch.Columns.Count; i++)
            {
                if (!dgvProductSearch.Columns[i].Name.Equals("ProductName"))
                {
                    dgvProductSearch.Columns[i].Visible = false;
                }
                else
                {
                    dgvProductSearch.Columns[i].Width = dgvProductSearch.Width;
                }
            }
            log.LogMethodExit();
        }

        private void txtProductSearch_TextChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (loadSearch)
            {
                if (txtProductSearch.Text.Length > 0)
                {
                    if (productsDTOList != null)
                    {
                        List<ProductsDTO> productsDTOFilterList = productsDTOList.Where(x => (bool)((string.IsNullOrEmpty(x.ProductName) ? "" : x.ProductName.ToLower()).Contains(txtProductSearch.Text.ToLower()))).ToList<ProductsDTO>();
                        if (productsDTOFilterList.Count > 0)
                        {
                            dgvProductSearch.Visible = true;
                            dgvProductSearch.DataSource = productsDTOFilterList;
                        }
                        else
                        {
                            dgvProductSearch.Visible = false;
                        }
                    }
                }
                else
                {
                    cmbProduct.Text = "";
                    dgvProductSearch.Visible = false;
                }
            }
            else
            {
                loadSearch = true;
            }
            log.LogMethodExit();
        }

        private void cmbProduct_SelectedValueChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (cmbProduct.Text.Equals("Semnox.Parafait.Product.ProductsDTO"))
            {
                txtProductSearch.Text = "";
            }
            else
            {
                txtProductSearch.Text = cmbProduct.Text;
            }
            loadSearch = false;
            log.LogMethodExit();
        }

        private void dgvProductSearch_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                txtProductSearch.Text = dgvProductSearch.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                cmbProduct.Text = txtProductSearch.Text;
                dgvProductSearch.Visible = false;
            }
            catch { }
            log.LogMethodExit();
        }

        private void UpdateUIElements()
        {
            log.LogMethodEntry();
            if (managementStudioSwitch.EnablProductModule)
            {
                dgvDisplayData.AllowUserToAddRows = true;
                dgvDisplayData.ReadOnly = false;
                btnSave.Enabled = true;
                btnDelete.Enabled = true;
            }
            else
            {
                dgvDisplayData.AllowUserToAddRows = false;
                dgvDisplayData.ReadOnly = true;
                btnSave.Enabled = false;
                btnDelete.Enabled = false;
            }
            log.LogMethodExit();
        }
    }
}
