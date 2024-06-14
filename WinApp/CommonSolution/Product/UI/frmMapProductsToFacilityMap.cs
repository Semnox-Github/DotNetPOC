/********************************************************************************************
 * Project Name - frmMapProductsToFacilityMap
 * Description  - UI for Mapping Products in facility map allowed product list
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 ********************************************************************************************* 
 *2.70        08-Mar-2019   Guru S A       Created
* 2.80        25-Jun-2020   Deeksha        Modified to Make Product module 
*                                          read only in Windows Management Studio.
 ********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic; 
using System.Data; 
using System.Linq; 
using System.Windows.Forms;

namespace Semnox.Parafait.Product.UI
{
    public partial class frmMapProductsToFacilityMap : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int productId = -1;
        private ExecutionContext machineUserContext;
        private Utilities utilities;
        private List<ProductsAllowedInFacilityMapDTO> productsAllowedInFacilityMapDTOList;
        private List<FacilityMapDTO> facilityMapDTOList;
        private ManagementStudioSwitch managementStudioSwitch;
        public frmMapProductsToFacilityMap(Utilities utilities, int productId)
        {
            log.LogMethodEntry(productId, utilities);
            this.utilities = utilities;
            this.productId = productId;
            SetMachineuserContext();
            InitializeComponent();
            managementStudioSwitch = new ManagementStudioSwitch(utilities.ExecutionContext);
            UpdateUIElements();
            log.LogMethodExit();
        }

        private void SetMachineuserContext()
        {
            log.LogMethodEntry();
            machineUserContext = ExecutionContext.GetExecutionContext();
            if (utilities.ParafaitEnv.IsCorporate)
            {
                machineUserContext.SetSiteId(utilities.ParafaitEnv.SiteId);
            }
            else
            {
                machineUserContext.SetSiteId(-1);
            }
            machineUserContext.SetUserId(utilities.ParafaitEnv.LoginID);
            log.LogMethodExit();
        }

        private void frmAllowedProducts_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            LoadFacilityMap();
            LoadMappedFacilityDetails();
            log.LogMethodExit();
        }

        private void LoadFacilityMap()
        {
            log.LogMethodEntry();
            if (facilityMapDTOList != null)
            {
                facilityMapDTOList.Clear();
            }
            FacilityMapListBL facilityMapListBL = new FacilityMapListBL(machineUserContext);
            List<KeyValuePair<FacilityMapDTO.SearchByParameters, string>> searchParam = new List<KeyValuePair<FacilityMapDTO.SearchByParameters, string>>(); 
            searchParam.Add(new KeyValuePair<FacilityMapDTO.SearchByParameters, string>(FacilityMapDTO.SearchByParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            facilityMapDTOList = facilityMapListBL.GetFacilityMapDTOList(searchParam);
            if(facilityMapDTOList == null)
            {
                facilityMapDTOList = new List<FacilityMapDTO>();
            }
            facilityMapDTOList = facilityMapDTOList.OrderBy(fm => fm.FacilityMapName).ToList();
            facilityMapDTOList.Insert(0, new FacilityMapDTO());
            facilityMapIdDGV.DataSource = facilityMapDTOList;
            facilityMapIdDGV.DisplayMember = "FacilityMapName";
            facilityMapIdDGV.ValueMember = "FacilityMapId";
            log.LogMethodExit();
        }
         

        private void LoadMappedFacilityDetails()
        {
            log.LogMethodEntry();
            ProductsAllowedInFacilityMapListBL productsAllowedInFacilityListBL = new ProductsAllowedInFacilityMapListBL(machineUserContext);
            List<KeyValuePair<ProductsAllowedInFacilityMapDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ProductsAllowedInFacilityMapDTO.SearchByParameters, string>>
            {
                new KeyValuePair<ProductsAllowedInFacilityMapDTO.SearchByParameters, string>(ProductsAllowedInFacilityMapDTO.SearchByParameters.PRODUCTS_ID, productId.ToString()),
                new KeyValuePair<ProductsAllowedInFacilityMapDTO.SearchByParameters, string>(ProductsAllowedInFacilityMapDTO.SearchByParameters.SITE_ID, machineUserContext.GetSiteId().ToString())
            };
            List<ProductsAllowedInFacilityMapDTO> productsAllowedInFacilityDTOList = productsAllowedInFacilityListBL.GetProductsAllowedInFacilityMapDTOList(searchParameters);
            BindingSource productsAllowedInFacilityBS = new BindingSource();
            if (productsAllowedInFacilityDTOList != null)
            {
                SortableBindingList<ProductsAllowedInFacilityMapDTO> productsAllowedInFacilityDTOSortList = new SortableBindingList<ProductsAllowedInFacilityMapDTO>(productsAllowedInFacilityDTOList);
                productsAllowedInFacilityBS.DataSource = productsAllowedInFacilityDTOSortList;
            }
            else
                productsAllowedInFacilityBS.DataSource = new SortableBindingList<ProductsAllowedInFacilityMapDTO>();

            this.dgvAllowedProducts.DataSource = productsAllowedInFacilityBS;
            utilities.setupDataGridProperties(ref dgvAllowedProducts);
            this.CreationDate.DefaultCellStyle = this.LastUpdateDate.DefaultCellStyle = utilities.gridViewDateTimeCellStyle();
            log.LogMethodExit();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                productsAllowedInFacilityDTOBindingSource = (BindingSource)dgvAllowedProducts.DataSource;
                SortableBindingList<ProductsAllowedInFacilityMapDTO> productsAllowedInFacilityRecords = (SortableBindingList<ProductsAllowedInFacilityMapDTO>)productsAllowedInFacilityDTOBindingSource.DataSource;

                if (productsAllowedInFacilityRecords != null && productsAllowedInFacilityRecords.Count > 0)
                {
                    foreach (ProductsAllowedInFacilityMapDTO productsAllowedInFacilityDTO in productsAllowedInFacilityRecords)
                    {
                        if (productsAllowedInFacilityDTO.ProductsId == -1)
                        {
                            continue;
                        }

                        if (productsAllowedInFacilityDTO.IsChanged)
                        {
                            try
                            { 
                                IsExistInAllowedProducts(productsAllowedInFacilityDTO.ProductsAllowedInFacilityMapId, productsAllowedInFacilityDTO.FacilityMapId, productsAllowedInFacilityDTO.ProductsId);
                                if (productsAllowedInFacilityDTO.DefaultRentalProduct)
                                {
                                    CheckForDuplicateDefaultRentalProduct(productsAllowedInFacilityDTO.ProductsAllowedInFacilityMapId, productsAllowedInFacilityDTO.FacilityMapId);
                                }
                            }
                            catch (ValidationException ex)
                            {
                                MessageBox.Show(ex.GetAllValidationErrorMessages());
                                continue;
                            }
                            ProductsAllowedInFacilityMapBL productsAllowedInFacilityBL = new ProductsAllowedInFacilityMapBL(machineUserContext, productsAllowedInFacilityDTO);
                            productsAllowedInFacilityBL.Save();
                        }
                    }
                    LoadMappedFacilityDetails();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit();
        }

        

        private void CheckForDuplicateDefaultRentalProduct(int allowedProductId, int facilityMapId)
        {
            log.LogMethodEntry(allowedProductId, productId);
            ProductsAllowedInFacilityMapListBL productsAllowedInFacilityListBL = new ProductsAllowedInFacilityMapListBL(machineUserContext);
            List<KeyValuePair<ProductsAllowedInFacilityMapDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ProductsAllowedInFacilityMapDTO.SearchByParameters, string>>
            {
                new KeyValuePair<ProductsAllowedInFacilityMapDTO.SearchByParameters, string>(ProductsAllowedInFacilityMapDTO.SearchByParameters.FACILITY_MAP_ID, facilityMapId.ToString()),
                new KeyValuePair<ProductsAllowedInFacilityMapDTO.SearchByParameters, string>(ProductsAllowedInFacilityMapDTO.SearchByParameters.SITE_ID, machineUserContext.GetSiteId().ToString()),
                new KeyValuePair<ProductsAllowedInFacilityMapDTO.SearchByParameters, string>(ProductsAllowedInFacilityMapDTO.SearchByParameters.DEFAULT_RENTAL_PRODUCT, "1"),
                new KeyValuePair<ProductsAllowedInFacilityMapDTO.SearchByParameters, string>(ProductsAllowedInFacilityMapDTO.SearchByParameters.IS_ACTIVE, "1")
            };
            List<ProductsAllowedInFacilityMapDTO> productsAllowedInFacilityDTOList = productsAllowedInFacilityListBL.GetProductsAllowedInFacilityMapDTOList(searchParameters);
             
            if (allowedProductId != -1 && productsAllowedInFacilityDTOList != null && productsAllowedInFacilityDTOList.Count > 0)
            {
                productsAllowedInFacilityDTOList = productsAllowedInFacilityDTOList.Where(prod => prod.ProductsAllowedInFacilityMapId != allowedProductId ).ToList(); 
            }      
            if (productsAllowedInFacilityDTOList != null && productsAllowedInFacilityDTOList.Count > 0)
            {
                throw new ValidationException(MessageContainerList.GetMessage(machineUserContext, 2129));// "There can be only one default rental product. Uncheck the existing option and save the changes before picking new default rental product"
            }
            log.LogMethodExit();
        }

        private void IsExistInAllowedProducts(int allowedProductId, int facilityMapId, int productId)
        {
            log.LogMethodEntry(allowedProductId, facilityMapId, productId);
            ProductsAllowedInFacilityMapListBL productsAllowedInFacilityListBL = new ProductsAllowedInFacilityMapListBL(machineUserContext);
            List<KeyValuePair<ProductsAllowedInFacilityMapDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ProductsAllowedInFacilityMapDTO.SearchByParameters, string>>
            {
                new KeyValuePair<ProductsAllowedInFacilityMapDTO.SearchByParameters, string>(ProductsAllowedInFacilityMapDTO.SearchByParameters.FACILITY_MAP_ID, facilityMapId.ToString()),
                new KeyValuePair<ProductsAllowedInFacilityMapDTO.SearchByParameters, string>(ProductsAllowedInFacilityMapDTO.SearchByParameters.SITE_ID, machineUserContext.GetSiteId().ToString()),
                new KeyValuePair<ProductsAllowedInFacilityMapDTO.SearchByParameters, string>(ProductsAllowedInFacilityMapDTO.SearchByParameters.PRODUCTS_ID, productId.ToString())
            };
            List<ProductsAllowedInFacilityMapDTO> productsAllowedInFacilityDTOList = productsAllowedInFacilityListBL.GetProductsAllowedInFacilityMapDTOList(searchParameters);

            if (allowedProductId != -1 && productsAllowedInFacilityDTOList != null && productsAllowedInFacilityDTOList.Count > 0)
            {
                productsAllowedInFacilityDTOList = productsAllowedInFacilityDTOList.Where(prod => prod.ProductsAllowedInFacilityMapId != allowedProductId).ToList();
            }
            if (productsAllowedInFacilityDTOList != null && productsAllowedInFacilityDTOList.Count > 0)
            {
                throw new ValidationException(MessageContainerList.GetMessage(machineUserContext, 2130));//Product is already mapped to the facility. Edit the record instead of adding it as new record
            }
            log.LogMethodExit();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            LoadMappedFacilityDetails();
            log.LogMethodExit();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (dgvAllowedProducts.CurrentRow == null || dgvAllowedProducts.CurrentRow.IsNewRow || dgvAllowedProducts.CurrentRow.Cells[0].Value == DBNull.Value)
                {
                    log.LogMethodExit("Nothing to delete");
                    return;
                }
                else
                {
                    DialogResult result1 = MessageBox.Show(MessageContainerList.GetMessage(machineUserContext, 1766),
                        MessageContainerList.GetMessage(machineUserContext, "Allowed Products"), MessageBoxButtons.YesNo);
                    if (result1 == DialogResult.Yes)
                    {
                        int id = Convert.ToInt32(dgvAllowedProducts.CurrentRow.Cells["productsAllowedInFacilityMapId"].Value);

                        if (id > -1)
                        {
                            ProductsAllowedInFacilityMapBL productsAllowedInFacilityBL = new ProductsAllowedInFacilityMapBL(machineUserContext, id);
                            productsAllowedInFacilityBL.ProductsAllowedInFacilityMapDTO.IsActive = false;
                            productsAllowedInFacilityBL.Save();
                        }
                        LoadMappedFacilityDetails();
                    }
                }
            }
            catch (Exception expn)
            {
                log.Error(expn);
                MessageBox.Show(expn.Message.ToString());
            }
            log.LogMethodExit();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            this.Close();
            log.LogMethodExit();
        }

        private void dgvAllowedProducts_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            e.Row.Cells["isActiveDataGridViewCheckBoxColumn"].Value = "True";
            e.Row.Cells["defaultRentalProductDataGridViewCheckBoxColumn"].Value = "False";
            e.Row.Cells["productsIdDGV"].Value = productId;
            log.LogMethodExit();
        }

        private void dgvAllowedProducts_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (dgvAllowedProducts.CurrentCell != null && dgvAllowedProducts.Columns[e.ColumnIndex].Name == "facilityMapIdDGV")
                {
                    SetProductType(e.RowIndex);
                    UpdateDefaultRentalFlag(e.RowIndex);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(MessageContainerList.GetMessage(machineUserContext, 1824, ex.Message));
            }
            log.LogMethodExit();
        }

        private void UpdateDefaultRentalFlag(int rowIndex)
        {
            log.LogMethodEntry(rowIndex);
            if (dgvAllowedProducts.CurrentCell != null && dgvAllowedProducts.Rows[rowIndex].Cells["productsIdDGV"] != null
                && Convert.ToInt32(dgvAllowedProducts.Rows[rowIndex].Cells["productsIdDGV"].Value) != -1
                && dgvAllowedProducts.Rows[rowIndex].Cells["productTypeDataGridViewTextBoxColumn"] != null)
            { 
                BindingSource dgvAllowedProductsBS = (BindingSource)dgvAllowedProducts.DataSource;
                var dgvAllowedProductsDTOList = (SortableBindingList<ProductsAllowedInFacilityMapDTO>)dgvAllowedProductsBS.DataSource;
                ProductsAllowedInFacilityMapDTO productsAllowedInFacilityDTO = dgvAllowedProductsDTOList[rowIndex];
                if(productsAllowedInFacilityDTO.ProductType != ProductTypeValues.RENTAL)
                {
                    productsAllowedInFacilityDTO.DefaultRentalProduct = false;
                }
            }
             log.LogMethodExit();
        }

        private void SetProductType(int rowIndex)
        {
            log.LogMethodEntry(rowIndex);
            if (dgvAllowedProducts.CurrentCell != null && dgvAllowedProducts.Rows[rowIndex].Cells["productsIdDGV"] != null
                && Convert.ToInt32(dgvAllowedProducts.Rows[rowIndex].Cells["productsIdDGV"].Value) != -1)
            {
                Products products = new Products(Convert.ToInt32(dgvAllowedProducts.Rows[rowIndex].Cells["productsIdDGV"].Value));
                BindingSource dgvAllowedProductsBS = (BindingSource)dgvAllowedProducts.DataSource;
                var dgvAllowedProductsDTOList = (SortableBindingList<ProductsAllowedInFacilityMapDTO>)dgvAllowedProductsBS.DataSource;
                ProductsAllowedInFacilityMapDTO productsAllowedInFacilityDTO = dgvAllowedProductsDTOList[rowIndex];
                productsAllowedInFacilityDTO.ProductType = products.GetProductsDTO.ProductType;
            }
            log.LogMethodExit();
        }

        private void dgvAllowedProducts_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (dgvAllowedProducts.CurrentCell != null && dgvAllowedProducts.Columns[e.ColumnIndex].Name == "defaultRentalProductDataGridViewCheckBoxColumn")
            {
                dgvAllowedProducts.Rows[e.RowIndex].Cells["defaultRentalProductDataGridViewCheckBoxColumn"].ReadOnly = true;
                if (dgvAllowedProducts.Rows[e.RowIndex].Cells["productTypeDataGridViewTextBoxColumn"].Value != null
                    && dgvAllowedProducts.Rows[e.RowIndex].Cells["productsIdDGV"].Value != null
                    && dgvAllowedProducts.Rows[e.RowIndex].Cells["productsIdDGV"].Value.ToString() != "-1")
                {
                    BindingSource dgvAllowedProductsListDTOBS = (BindingSource)dgvAllowedProducts.DataSource;
                    var dgvAllowedProductsDTOList = (SortableBindingList<ProductsAllowedInFacilityMapDTO>)dgvAllowedProductsListDTOBS.DataSource;
                    ProductsAllowedInFacilityMapDTO productsAllowedInFacilityDTO = dgvAllowedProductsDTOList[e.RowIndex];
                    if (productsAllowedInFacilityDTO.ProductType == ProductTypeValues.RENTAL)
                    {
                        dgvAllowedProducts.Rows[e.RowIndex].Cells["defaultRentalProductDataGridViewCheckBoxColumn"].ReadOnly = false;
                    }
                }
            }
            log.LogMethodExit();
        }

        private void dgvAllowedProducts_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if(dgvAllowedProducts.CurrentCell != null && dgvAllowedProducts.Columns[e.ColumnIndex].Name == "facilityMapIdDGV")
            {
                if(dgvAllowedProducts.Rows[e.RowIndex] != null && dgvAllowedProducts.Rows[e.RowIndex].Cells[e.ColumnIndex] != null 
                    && dgvAllowedProducts.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null && Convert.ToInt32(dgvAllowedProducts.Rows[e.RowIndex].Cells[e.ColumnIndex].Value) != -1)
                {
                    log.Info("Invalid product at " + e.RowIndex.ToString() + " row"); 
                }
            }
            log.LogMethodExit();
        }
        private void UpdateUIElements()
        {
            log.LogMethodEntry();
            if (managementStudioSwitch.EnablProductModule)
            {
                dgvAllowedProducts.AllowUserToAddRows = true;
                dgvAllowedProducts.ReadOnly = false;
                btnSave.Enabled = true;
                btnDelete.Enabled = true;
            }
            else
            {
                dgvAllowedProducts.AllowUserToAddRows = false;
                dgvAllowedProducts.ReadOnly = true;
                btnSave.Enabled = false;
                btnDelete.Enabled = false;
            }
            log.LogMethodExit();
        }
    }
}
