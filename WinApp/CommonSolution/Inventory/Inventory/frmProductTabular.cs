/********************************************************************************************
*Project Name -  frmProducts                                                                         
*Description  -
/*****************************************************************************************************************
 * **Version Log*
** Version     Date                Modified By                 Remarks
/*********************************************************************************************************************
 *2.60       11-Apr-2019            Girish                     Modified : Replacing purchaseTax 3 tier with Tax 3 tier 
 *2.70.2     13-Aug-2019            Deeksha                    Added logger methods.
 *2.80.0     11-Feb-2020            Deeksha                    Modified :Allowing User to create New Records.
 *2.90.0     20-Jun-2020            Deeksha                    Modified :Bulk product publish for inventory products & weighted average costing changes.
 *2.100.0    23-Aug-2020            Deeksha                    Modified :Added RecipeDescription , ItemType , Include in plan , Yield percentage fields
*********************************************************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Category;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Product;
using Semnox.Parafait.Publish;
using Semnox.Parafait.Vendor;

namespace Semnox.Parafait.Inventory
{
    public partial class frmProductTabular : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Utilities utilities;
        private SegmentCategorizationValueUI segmentCategorizationValueUI;
        private ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        private int manualProductTypeId = -1;
        private const string manualproductDisplaygroup = "Parafait Inventory Products";
        private CustomCheckBox cbxHeaderSelect;
        private AdvancedSearch AdvancedSearch;
        private List<ProductDTO> dtoProductDetails = new List<ProductDTO>();
        private SortableBindingList<ProductDTO> dtoProductLoaded;
        private int dgvProductPageNo = 2;
        private const int dgvProductPageSize = 5000;
        private SortOrder dgvProductSortOrder = SortOrder.None;
        private DataGridViewColumn oldSortColumn = null;
        private int defaultItemTypeId = -1;

        public frmProductTabular(Utilities _Utilities)
        {
            log.LogMethodEntry(_Utilities);
            utilities = _Utilities;
            InitializeComponent();
            utilities.setLanguage(this);
            LoadCategoryCombobox();
            LoadUOMCombobox();
            LoadInOutBoundLocationCombobox();
            LoadVendorCombobox();
            LoadTaxCombobox();
            LoadExpiryType();
            SetManualProductTypeId();
            PopulateCategory();
            PopulateVendor();
            PopulateItemType();
            PopulateDisplayGroup();
            CreateHeaderCheckBox();
            dgvProductPageNo = 2;
            log.LogMethodExit();
        }

        private void frmProductTabular_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (utilities.ParafaitEnv.IsCorporate)
            {
                machineUserContext.SetSiteId(utilities.ParafaitEnv.SiteId);
            }
            else
            {
                machineUserContext.SetSiteId(-1);
            }
            machineUserContext.SetUserId(utilities.ParafaitEnv.LoginID);

            dgvProducts.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
            dgvProducts.EnableHeadersVisualStyles = false;
            dgvProducts.EditMode = DataGridViewEditMode.EditOnEnter;
            dgvProducts.BackgroundColor = this.BackColor;
            dgvProducts.ColumnHeadersDefaultCellStyle.Font = new Font(utilities.ParafaitEnv.DEFAULT_FONT, 9F, FontStyle.Bold);

            costDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewAmountCellStyle();
            lastPurchasePriceDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewAmountCellStyle();
            salePriceDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewAmountCellStyle();
            reorderPointDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewAmountCellStyle();
            innerPackQtyDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewAmountCellStyle();
            reorderQuantityDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewAmountCellStyle();
            TurnInPriceInTickets.DefaultCellStyle =
                priceInTicketsDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewNumericCellStyle();

            LowerLimitCost.DefaultCellStyle =
                UpperLimitCost.DefaultCellStyle =
                CostVariancePercentage.DefaultCellStyle = utilities.gridViewNumericCellStyle();

            lastPurchasePriceDataGridViewTextBoxColumn.DefaultCellStyle.Format =
                LowerLimitCost.DefaultCellStyle.Format =
            costDataGridViewTextBoxColumn.DefaultCellStyle.Format = utilities.ParafaitEnv.INVENTORY_COST_FORMAT;
            reorderPointDataGridViewTextBoxColumn.DefaultCellStyle.Format =
            innerPackQtyDataGridViewTextBoxColumn.DefaultCellStyle.Format =
            reorderQuantityDataGridViewTextBoxColumn.DefaultCellStyle.Format = utilities.ParafaitEnv.INVENTORY_QUANTITY_FORMAT;
            cmbActive.SelectedIndex = 1;
            cmbProductType.SelectedIndex = 0;
            int navFormWidth = 0;
            foreach (Form f in Application.OpenForms)
            {
                if (f.Name == "Navigation")
                {
                    navFormWidth = f.Width;
                    break;
                }
            }
            this.Width = this.ParentForm.Width - navFormWidth - 30;// - this.ParentForm.Controls["splitContainerRedemption"].Width - 30;
            this.Height = this.ParentForm.Height - 120;
            PopulateProductGrid();
            if (utilities.ParafaitEnv.IsCorporate && utilities.ParafaitEnv.IsMasterSite)
            {
                lnkPublishToSite.Visible = true;
            }
            else
            {
                lnkPublishToSite.Visible = false;
            }
            log.LogMethodExit();
        }

        private void PopulateProductGrid(List<ProductDTO> productDTOLists = null)
        {
            log.LogMethodEntry(productDTOLists);
            dgvProductPageNo = 2;
            dtoProductLoaded = new SortableBindingList<ProductDTO>();
            BindingSource productListBS = new BindingSource();
            ProductList productList = new ProductList();
            List<ProductDTO> productListOnDisplay;
            if (productDTOLists == null)
            { // during form load
                List<KeyValuePair<ProductDTO.SearchByProductParameters, string>> productListSearchParams = new List<KeyValuePair<ProductDTO.SearchByProductParameters, string>>();
                productListSearchParams.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.IS_ACTIVE, "Y"));
                productListSearchParams.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                productListOnDisplay = productList.GetAllProducts(productListSearchParams);
            }
            else
            {
                productListOnDisplay = productDTOLists;
            }
            if(productListOnDisplay != null && productListOnDisplay.Any())
            {
                dtoProductDetails = new List<ProductDTO>(productListOnDisplay);
                IEnumerable<ProductDTO> firstSetOfRows = productListOnDisplay.AsEnumerable().Skip(0).Take(dgvProductPageSize);
                if (firstSetOfRows.Any())
                {
                    dtoProductLoaded = new SortableBindingList<ProductDTO>(firstSetOfRows.ToList());
                }
                else
                {
                    dtoProductLoaded = new SortableBindingList<ProductDTO>(dtoProductDetails);
                }
                dgvProducts.DataSource = dtoProductLoaded;
                for(int i = 0; i < dtoProductLoaded.Count; i++)
                {
                    CommonFuncs.GetUOMComboboxForSelectedRows(dgvProducts, i, dtoProductLoaded[i].InventoryUOMId);
                    if (dtoProductLoaded[i].ItemType == -1)
                    {
                        dtoProductLoaded[i].ItemType = defaultItemTypeId;
                    }
                }
            }
            log.LogMethodExit();
        }

        private void LoadCategoryCombobox()
        {
            log.LogMethodEntry();
            List<CategoryDTO> categoryListOnDisplay = null;
            categoryListOnDisplay = GetAllCategory(categoryListOnDisplay);

            categoryListOnDisplay.Insert(0, new CategoryDTO());
            categoryListOnDisplay[0].Name = "None";
            categoryListOnDisplay[0].CategoryId = -1;

            categoryIdDataGridViewTextBoxColumn.DataSource = categoryListOnDisplay;
            categoryIdDataGridViewTextBoxColumn.DisplayMember = "Name";
            categoryIdDataGridViewTextBoxColumn.ValueMember = "CategoryId";
            log.LogMethodExit();
        }

        private void PopulateCategory()
        {
            log.LogMethodEntry();
            BindingSource categoryBS = new BindingSource();
            List<CategoryDTO> categoryDTOList = null;
            categoryDTOList = GetAllCategory(categoryDTOList);
            categoryDTOList.Insert(0, new CategoryDTO());
            categoryDTOList[0].Name = "<All>";
            categoryBS.DataSource = categoryDTOList.OrderBy(category => category.Name);
            cmbProductCategory.DataSource = categoryBS;
            cmbProductCategory.ValueMember = "CategoryId";
            cmbProductCategory.DisplayMember = "Name";
            log.LogMethodExit();
        }

        private List<CategoryDTO> GetAllCategory(List<CategoryDTO> categoryDTOList)
        {
            log.LogMethodEntry(categoryDTOList);
            CategoryList CategoryList = new CategoryList(machineUserContext);
            List<KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>> categoryDTOSearchParams;
            categoryDTOSearchParams = new List<KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>>();
            categoryDTOSearchParams.Add(new KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>(CategoryDTO.SearchByCategoryParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            categoryDTOList = CategoryList.GetAllCategory(categoryDTOSearchParams);
            if (categoryDTOList == null)
            {
                categoryDTOList = new List<CategoryDTO>();
            }
            log.LogMethodExit(categoryDTOList);
            return categoryDTOList;
        }

        private void LoadUOMCombobox()
        {
            log.LogMethodEntry();
            List<KeyValuePair<UOMDTO.SearchByUOMParameters, string>> uomListSearchParams = new List<KeyValuePair<UOMDTO.SearchByUOMParameters, string>>();
            uomListSearchParams.Add(new KeyValuePair<UOMDTO.SearchByUOMParameters, string>(UOMDTO.SearchByUOMParameters.SITEID, machineUserContext.GetSiteId().ToString()));

            UOMDataHandler uomDataHandler = new UOMDataHandler();
            List<UOMDTO> uomListOnDisplay = uomDataHandler.GetUOMList(uomListSearchParams);

            if (uomListOnDisplay == null)
                uomListOnDisplay = new List<UOMDTO>();

            uomListOnDisplay.Insert(0, new UOMDTO());
            uomListOnDisplay[0].UOM = "None";
            uomListOnDisplay[0].UOMId = -1;
            uomIdDataGridViewTextBoxColumn.DataSource = uomListOnDisplay;
            uomIdDataGridViewTextBoxColumn.DisplayMember = "UOM";
            uomIdDataGridViewTextBoxColumn.ValueMember = "UOMId";
            log.LogMethodExit();
        }

        private void LoadInOutBoundLocationCombobox()
        {
            log.LogMethodEntry();
            List<LocationDTO> inBoundLocationDTOList;
            List<LocationDTO> outBoundLocationDTOList;
            LocationList locationList = new LocationList(machineUserContext);
            List<KeyValuePair<LocationDTO.SearchByLocationParameters, string>> locationDTOSearchParams;
            locationDTOSearchParams = new List<KeyValuePair<LocationDTO.SearchByLocationParameters, string>>();
            BindingSource locationBS = new BindingSource();
            inBoundLocationDTOList = locationList.GetAllLocations("Store");
            if (inBoundLocationDTOList == null)
            {
                inBoundLocationDTOList = new List<LocationDTO>();
            }

            inBoundLocationDTOList.Insert(0, new LocationDTO());
            inBoundLocationDTOList[0].Name = "None";
            defaultLocationIdDataGridViewTextBoxColumn.DataSource = inBoundLocationDTOList;
            defaultLocationIdDataGridViewTextBoxColumn.DisplayMember = "Name";
            defaultLocationIdDataGridViewTextBoxColumn.ValueMember = "LocationId";

            outBoundLocationDTOList = locationList.GetAllLocations(locationDTOSearchParams);
            if (outBoundLocationDTOList == null)
            {
                outBoundLocationDTOList = new List<LocationDTO>();
            }
            outBoundLocationDTOList.Insert(0, new LocationDTO());
            outBoundLocationDTOList[0].Name = "None";
            outboundLocationIdDataGridViewTextBoxColumn.DataSource = outBoundLocationDTOList;
            outboundLocationIdDataGridViewTextBoxColumn.DisplayMember = "Name";
            outboundLocationIdDataGridViewTextBoxColumn.ValueMember = "LocationId";
            log.LogMethodExit();
        }

        private void LoadVendorCombobox()
        {
            log.LogMethodEntry();
            List<VendorDTO> vendorListOnDisplay = null;
            vendorListOnDisplay = GetAllVendor(vendorListOnDisplay);

            vendorListOnDisplay.Insert(0, new VendorDTO());
            vendorListOnDisplay[0].Name = "None";
            vendorListOnDisplay[0].VendorId = -1;

            defaultVendorIdDataGridViewTextBoxColumn.DataSource = vendorListOnDisplay;
            defaultVendorIdDataGridViewTextBoxColumn.DisplayMember = "Name";
            defaultVendorIdDataGridViewTextBoxColumn.ValueMember = "VendorId";
            log.LogMethodExit();
        }

        private void PopulateVendor()
        {
            log.LogMethodEntry();
            BindingSource vendorBS = new BindingSource();
            List<VendorDTO> vendorDTOList = null;
            vendorDTOList = GetAllVendor(vendorDTOList);
            vendorDTOList.Insert(0, new VendorDTO());
            vendorDTOList[0].Name = "<All>";
            vendorBS.DataSource = vendorDTOList.OrderBy(Vendor => Vendor.Name);
            cmbVendor.DataSource = vendorBS;
            cmbVendor.ValueMember = "VendorId";
            cmbVendor.DisplayMember = "Name";
            log.LogMethodExit();
        }

        private List<VendorDTO> GetAllVendor(List<VendorDTO> vendorDTOList)
        {
            log.LogMethodEntry(vendorDTOList);
            VendorList venodrList = new VendorList(machineUserContext);
            List<KeyValuePair<VendorDTO.SearchByVendorParameters, string>> vendorDTOSearchParams;
            vendorDTOSearchParams = new List<KeyValuePair<VendorDTO.SearchByVendorParameters, string>>();
            vendorDTOSearchParams.Add(new KeyValuePair<VendorDTO.SearchByVendorParameters, string>(VendorDTO.SearchByVendorParameters.SITEID, machineUserContext.GetSiteId().ToString()));
            vendorDTOList = venodrList.GetAllVendors(vendorDTOSearchParams);
            if (vendorDTOList == null)
            {
                vendorDTOList = new List<VendorDTO>();
            }
            log.LogMethodExit(vendorDTOList);
            return vendorDTOList;
        }

        private void LoadTaxCombobox()
        {
            log.LogMethodEntry();
            List<TaxDTO> taxDTOList;
            TaxList taxList = new TaxList(machineUserContext);
            List<KeyValuePair<TaxDTO.SearchByTaxParameters, string>> taxDTOSearchParams;
            taxDTOSearchParams = new List<KeyValuePair<TaxDTO.SearchByTaxParameters, string>>();
            taxDTOSearchParams.Add(new KeyValuePair<TaxDTO.SearchByTaxParameters, string>(TaxDTO.SearchByTaxParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            BindingSource taxBS = new BindingSource();
            taxDTOList = taxList.GetAllTaxes(taxDTOSearchParams);

            if (taxDTOList == null)
            {
                taxDTOList = new List<TaxDTO>();
            }

            taxDTOList.Insert(0, new TaxDTO());
            taxDTOList[0].TaxName = "None";

            taxIdDataGridViewTextBoxColumn.DataSource = taxDTOList;
            taxIdDataGridViewTextBoxColumn.ValueMember = "TaxId";
            taxIdDataGridViewTextBoxColumn.DisplayMember = "TaxName";
            log.LogMethodExit();
        }

        private void LoadExpiryType()
        {
            log.LogMethodEntry();
            DataTable dt = new DataTable();
            dt.Columns.Add("Type");
            dt.Columns.Add("ExpiryName");
            dt.Rows.Add("N", "None");
            dt.Rows.Add("D", "In Days");
            dt.Rows.Add("E", "Expiry Date");
            expiryTypeDataGridViewTextBoxColumn.DataSource = dt;
            expiryTypeDataGridViewTextBoxColumn.ValueMember = "Type";
            expiryTypeDataGridViewTextBoxColumn.DisplayMember = "ExpiryName";
            log.LogMethodExit();
        }

        private void dgvProducts_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                e.Row.Cells["isSellableDataGridViewTextBoxColumn"].Value = "Y";
                e.Row.Cells["isRedeemableDataGridViewTextBoxColumn"].Value = "Y";
                e.Row.Cells["isActiveDataGridViewTextBoxColumn"].Value = "true";
                e.Row.Cells["isPurchaseableDataGridViewTextBoxColumn"].Value = "Y";
                e.Row.Cells["innerPackQtyDataGridViewTextBoxColumn"].Value = 1;
                e.Row.Cells["costDataGridViewTextBoxColumn"].Value = 0;
                e.Row.Cells["salePriceDataGridViewTextBoxColumn"].Value = 0;
                e.Row.Cells["reorderPointDataGridViewTextBoxColumn"].Value = 0;
                e.Row.Cells["reorderQuantityDataGridViewTextBoxColumn"].Value = 1;

                e.Row.Cells["expiryTypeDataGridViewTextBoxColumn"].Value = "N";
                e.Row.Cells["issuingApproachDataGridViewTextBoxColumn"].Value = "None";
                e.Row.Cells["taxInclusiveCostDataGridViewTextBoxColumn"].Value = "N";
                e.Row.Cells["autoUpdateMarkupDataGridViewCheckBoxColumn"].Value = false;
                int defaultStoreId = GetDefaultStoreId();
                if (defaultStoreId > 0)
                    e.Row.Cells["defaultLocationIdDataGridViewTextBoxColumn"].Value = defaultStoreId;
                if (defaultStoreId > 0)
                    e.Row.Cells["outboundLocationIdDataGridViewTextBoxColumn"].Value = defaultStoreId;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void dgvProducts_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (dgvProducts.Columns[e.ColumnIndex].DataPropertyName == "ItemMarkupPercent" 
                    && dgvProducts["itemMarkupPercentDataGridViewTextBoxColumn", e.RowIndex].Value.ToString() == "NaN")
                { }
                else
                {
                    MessageBox.Show("Error in Product grid data at row " + (e.RowIndex + 1).ToString() + ", Column " + dgvProducts.Columns[e.ColumnIndex].DataPropertyName +
                    ": " + e.Exception.Message);
                    e.Cancel = true;
                }

            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            Products productsBL = null;//create products

            try
            {
                this.Cursor = Cursors.WaitCursor;
                ProductsDTO productsDTO = null;
                foreach (ProductDTO d in dtoProductLoaded)
                {
                    if (d.IsChanged)
                    {
                        if (d.LotControlled && d.IssuingApproach == "None")
                        {
                            if (d.ExpiryType == "E" || d.ExpiryType == "D")
                                d.IssuingApproach = "FEFO";
                            else
                                d.IssuingApproach = "FIFO";
                        }
                        if (d.ManualProductId == -1)
                        {
                            productsDTO = new ProductsDTO();
                        }
                        else
                        {
                            productsBL = new Products(d.ManualProductId);
                            productsDTO = productsBL.GetProductsDTO;
                        }
                        if (d.ProductName == null)
                            d.ProductName = d.Code;
                        productsDTO.InventoryItemDTO = d;
                        UpdatePOSProductInformation(productsDTO);
                        productsBL = new Products(machineUserContext, productsDTO);
                        productsBL.Save();


                        if (d.LotControlled && d.IssuingApproach == "FIFO")
                        {
                            InventoryLotBL inventoryLot = new InventoryLotBL(machineUserContext);
                            inventoryLot.UpdateNonLotableToLotable(d.ProductId, null);
                        }

                    }
                }
                ProductSearch();
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                if (ex.Number == 2627)
                {
                    MessageBox.Show(MessageContainerList.GetMessage(machineUserContext, 290), MessageContainerList.GetMessage(machineUserContext, "Database Save"));
                    return;
                }
                MessageBox.Show(ex.Data.ToString() + ex.Message, MessageContainerList.GetMessage(machineUserContext, "Database Save"));
                return;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(MessageContainerList.GetMessage(machineUserContext, 1824, ex.Message));
            }
            finally { this.Cursor = Cursors.Default; }
            log.LogMethodExit();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                cbxHeaderSelect.Checked = false;
                bool valid = ValidateSetProductsCheckbox();
                if (!valid)
                {
                    MessageBox.Show(MessageContainerList.GetMessage(machineUserContext, 218), MessageContainerList.GetMessage(machineUserContext, "Validation Error"));//Choose Product 
                    log.LogMethodExit();
                    return;
                }
                for (int rowIndex = 0; rowIndex < dgvProducts.Rows.Count - 1; rowIndex++)
                {
                    if (dgvProducts.Rows[rowIndex].Cells["SetProducts"].Value != null && (bool)dgvProducts.Rows[rowIndex].Cells["SetProducts"].Value)
                    {
                        dgvProducts.Rows[rowIndex].Cells["isActiveDataGridViewTextBoxColumn"].Value = false;
                        dgvProducts.Rows[rowIndex].Cells["SetProducts"].Value = false;
                    }
                }
                MessageBox.Show(MessageContainerList.GetMessage(machineUserContext, 2731)); // "Products will be made as Inactive after save"
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            bool saverequired = false;
            bool isChanged = false;

            try
            {
                this.Cursor = Cursors.WaitCursor;
                foreach (ProductDTO d in dtoProductLoaded)
                {
                    if (d.IsChanged == true)
                    {
                        isChanged = true;
                        break;
                    }
                }
            }

            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(ex.Message);
            }
            try
            {
                if (isChanged)
                {
                    DialogResult DR = MessageBox.Show(MessageContainerList.GetMessage(machineUserContext, 566), MessageContainerList.GetMessage(machineUserContext, "Save Changes"), MessageBoxButtons.YesNoCancel);
                    switch (DR)
                    {
                        case DialogResult.Yes: saverequired = true; break;
                        case DialogResult.No: break;
                        case DialogResult.Cancel: return;
                        default: break;
                    }
                }

                if (saverequired)
                {
                    this.Cursor = Cursors.WaitCursor;
                    if (!ValidateChildren())
                    {
                        log.LogMethodExit();
                        return;
                    }
                    btnSave.PerformClick();
                }
                ProductSearch();
            }

            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(ex.Message);
            }
            finally { this.Cursor = Cursors.Default; }
            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            bool saverequired = false;
            bool isChanged = false;
            try
            {
                this.Cursor = Cursors.WaitCursor;
                foreach (ProductDTO d in dtoProductLoaded)
                {
                    if (d.IsChanged == true)
                    {
                        isChanged = true;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (isChanged)
                {
                    DialogResult DR = MessageBox.Show(MessageContainerList.GetMessage(machineUserContext, 566), MessageContainerList.GetMessage(machineUserContext, "Save Changes"), MessageBoxButtons.YesNoCancel);
                    switch (DR)
                    {
                        case DialogResult.Yes: saverequired = true; break;
                        case DialogResult.No: break;
                        case DialogResult.Cancel: return;
                        default: break;
                    }
                }

                if (saverequired)
                {
                    if (!ValidateChildren())
                        return;
                    btnSave.PerformClick();
                }
                this.Close();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }

        private void dgvProducts_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            if (dgvProducts.Columns[e.ColumnIndex].DataPropertyName == "Code")
            {
                CurrencyManager xCM = (CurrencyManager)dgvProducts.BindingContext[dgvProducts.DataSource, dgvProducts.DataMember];
                if (xCM.Count <= 0) return;

                try
                {
                    if (dgvProducts[codeDataGridViewTextBoxColumn.Index, e.RowIndex].Value == null ||
                        Convert.ToInt32(dgvProducts[productIdDataGridViewTextBoxColumn.Index, e.RowIndex].Value) == -1)
                    {
                        dgvProducts[codeDataGridViewTextBoxColumn.Index, e.RowIndex].ReadOnly = false;
                    }
                    else
                    {
                        dgvProducts[codeDataGridViewTextBoxColumn.Index, e.RowIndex].ReadOnly = true;
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }

            }
            if (dgvProducts.Columns[e.ColumnIndex].DataPropertyName == "SalePrice")
            {
                CurrencyManager xCM = (CurrencyManager)dgvProducts.BindingContext[dgvProducts.DataSource, dgvProducts.DataMember];
                if (xCM.Count <= 0) return;
                try
                {
                    if (Convert.ToInt32(dgvProducts[productIdDataGridViewTextBoxColumn.Index, e.RowIndex].Value) == -1
                        && Convert.ToInt32(dgvProducts[salePriceDataGridViewTextBoxColumn.Index, e.RowIndex].Value) == 0)
                    {
                        dgvProducts[salePriceDataGridViewTextBoxColumn.Index, e.RowIndex].ReadOnly = false;
                    }
                    else
                    {
                        dgvProducts[salePriceDataGridViewTextBoxColumn.Index, e.RowIndex].ReadOnly = true;
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
            }

            if (dgvProducts.Columns[e.ColumnIndex].DataPropertyName == "ProductName")
            {
                CurrencyManager xCM = (CurrencyManager)dgvProducts.BindingContext[dgvProducts.DataSource, dgvProducts.DataMember];
                if (xCM.Count <= 0) return;
                try
                {
                    if (dgvProducts["codeDataGridViewTextBoxColumn", e.RowIndex].Value != null && dgvProducts["productNameDataGridViewTextBoxColumn", e.RowIndex].Value.ToString() == "")
                        dgvProducts["productNameDataGridViewTextBoxColumn", e.RowIndex].Value = dgvProducts["codeDataGridViewTextBoxColumn", e.RowIndex].Value;
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
            }
            if (dgvProducts.Columns[e.ColumnIndex].DataPropertyName == "PriceInTickets")
            {
                CurrencyManager xCM = (CurrencyManager)dgvProducts.BindingContext[dgvProducts.DataSource, dgvProducts.DataMember];
                if (xCM.Count <= 0) return;

                try
                {
                    if (dgvProducts["isRedeemableDataGridViewTextBoxColumn", e.RowIndex].Value.ToString() == "Y" && dgvProducts["autoUpdateMarkupDataGridViewCheckBoxColumn", e.RowIndex].Value.ToString() == "True")
                    {
                        dgvProducts["priceInTicketsDataGridViewTextBoxColumn", e.RowIndex].ReadOnly = true;
                    }
                    else
                    {
                        dgvProducts["priceInTicketsDataGridViewTextBoxColumn", e.RowIndex].ReadOnly = false;
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
            }
            if (dgvProducts.Columns[e.ColumnIndex].DataPropertyName == "ItemMarkupPercent")
            {
                CurrencyManager xCM = (CurrencyManager)dgvProducts.BindingContext[dgvProducts.DataSource, dgvProducts.DataMember];
                if (xCM.Count <= 0) return;

                try
                {
                    if (dgvProducts["isRedeemableDataGridViewTextBoxColumn", e.RowIndex].Value.ToString() == "N")
                    {
                        dgvProducts["itemMarkupPercentDataGridViewTextBoxColumn", e.RowIndex].ReadOnly = true;
                    }
                    else
                    {
                        dgvProducts["itemMarkupPercentDataGridViewTextBoxColumn", e.RowIndex].ReadOnly = false;
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
            }
            if (dgvProducts.Columns[e.ColumnIndex].DataPropertyName == "AutoUpdateMarkup")
            {
                CurrencyManager xCM = (CurrencyManager)dgvProducts.BindingContext[dgvProducts.DataSource, dgvProducts.DataMember];
                if (xCM.Count <= 0) return;

                try
                {
                    if (dgvProducts["isRedeemableDataGridViewTextBoxColumn", e.RowIndex].Value.ToString() == "N")
                    {
                        dgvProducts["autoUpdateMarkupDataGridViewCheckBoxColumn", e.RowIndex].ReadOnly = true;
                    }
                    else
                    {
                        dgvProducts["autoUpdateMarkupDataGridViewCheckBoxColumn", e.RowIndex].ReadOnly = false;
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
            }
            log.LogMethodExit();
        }

        private void dgvProducts_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (!dgvProducts.Rows[e.RowIndex].IsNewRow && dgvProducts["codeDataGridViewTextBoxColumn", e.RowIndex].Value == null || dgvProducts["codeDataGridViewTextBoxColumn", e.RowIndex].Value == DBNull.Value)
                {
                    MessageBox.Show(MessageContainerList.GetMessage(machineUserContext, 1087), MessageContainerList.GetMessage(machineUserContext, "Code Validation"));
                    e.Cancel = true;
                }

                else if (!dgvProducts.Rows[e.RowIndex].IsNewRow && dgvProducts["categoryIdDataGridViewTextBoxColumn", e.RowIndex].Value.ToString() == "-1")
                {
                    MessageBox.Show(MessageContainerList.GetMessage(machineUserContext, 1088), MessageContainerList.GetMessage(machineUserContext, "Category Validation"));
                    e.Cancel = true;
                }

                else if ((!dgvProducts.Rows[e.RowIndex].IsNewRow && dgvProducts["uomIdDataGridViewTextBoxColumn", e.RowIndex].Value.ToString() == "-1"))
                {
                    MessageBox.Show(MessageContainerList.GetMessage(machineUserContext, 888), MessageContainerList.GetMessage(machineUserContext, "UOM Validation"));
                    e.Cancel = true;
                }

                else if (!dgvProducts.Rows[e.RowIndex].IsNewRow && dgvProducts["defaultLocationIdDataGridViewTextBoxColumn", e.RowIndex].Value.ToString() == "-1")
                {
                    MessageBox.Show(MessageContainerList.GetMessage(machineUserContext, 1089), MessageContainerList.GetMessage(machineUserContext, "Location Validation"));
                    e.Cancel = true;
                }

                else if (!dgvProducts.Rows[e.RowIndex].IsNewRow && dgvProducts["outboundLocationIdDataGridViewTextBoxColumn", e.RowIndex].Value.ToString() == "-1")
                {
                    MessageBox.Show(MessageContainerList.GetMessage(machineUserContext, 1090), MessageContainerList.GetMessage(machineUserContext, "Location Validation"));
                    e.Cancel = true;
                }
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void dgvProducts_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            if (dgvProducts.Columns[e.ColumnIndex].DataPropertyName == "ItemMarkupPercent"
                || dgvProducts.Columns[e.ColumnIndex].DataPropertyName == "AutoUpdateMarkup"
                || dgvProducts.Columns[e.ColumnIndex].DataPropertyName == "Cost")
            {
                if (dgvProducts["isRedeemableDataGridViewTextBoxColumn", e.RowIndex].Value.ToString() == "Y"
                     && dgvProducts["autoUpdateMarkupDataGridViewCheckBoxColumn", e.RowIndex].Value.ToString() == "True")
                {
                    double productCost;
                    double itemMarkUpPercent;
                    int vendorId;
                    double priceInTicket;
                    try { productCost = Convert.ToDouble(dgvProducts["costDataGridViewTextBoxColumn", e.RowIndex].Value.ToString()); }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        MessageBox.Show(MessageContainerList.GetMessage(machineUserContext, 891), MessageContainerList.GetMessage(machineUserContext, "Cost Validation"));
                        log.LogMethodExit();
                        return;
                    }
                    try
                    {
                        if (dgvProducts["itemMarkupPercentDataGridViewTextBoxColumn", e.RowIndex].Value.ToString() == ""
                            || dgvProducts["itemMarkupPercentDataGridViewTextBoxColumn", e.RowIndex].Value.ToString() == "NaN")
                            itemMarkUpPercent = double.NaN;
                        else
                            itemMarkUpPercent = Convert.ToDouble(dgvProducts["itemMarkupPercentDataGridViewTextBoxColumn", e.RowIndex].Value.ToString());
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        MessageBox.Show(MessageContainerList.GetMessage(machineUserContext, 1229), MessageContainerList.GetMessage(machineUserContext, "Price in Tickets"));
                        log.LogMethodExit();
                        return;
                    }
                    try { vendorId = Convert.ToInt32(dgvProducts["defaultVendorIdDataGridViewTextBoxColumn", e.RowIndex].Value.ToString()); }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        MessageBox.Show(MessageContainerList.GetMessage(machineUserContext, 850), MessageContainerList.GetMessage(machineUserContext, "Vendor Id"));
                        log.LogMethodExit();
                        return;
                    }
                    try { priceInTicket = Convert.ToDouble(dgvProducts["priceInTicketsDataGridViewTextBoxColumn", e.RowIndex].Value.ToString()); }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        MessageBox.Show(MessageContainerList.GetMessage(machineUserContext, 1230), MessageContainerList.GetMessage(machineUserContext, "Price in Tickets"));
                        log.LogMethodExit();
                        return;
                    }
                    try
                    {
                        ProductList productBL = new ProductList();
                        double newPITValue = productBL.calculatePITByMarkUp(productCost, itemMarkUpPercent, vendorId);
                        if (newPITValue != priceInTicket)
                        {
                            dgvProducts["priceInTicketsDataGridViewTextBoxColumn", e.RowIndex].Value = newPITValue;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, MessageContainerList.GetMessage(machineUserContext, "Price in Tickets"));
                        if (dgvProducts.Columns[e.ColumnIndex].DataPropertyName == "AutoUpdateMarkup" && dgvProducts["autoUpdateMarkupDataGridViewCheckBoxColumn", e.RowIndex].Value.ToString() == "True")
                            dgvProducts["autoUpdateMarkupDataGridViewCheckBoxColumn", e.RowIndex].Value = false;
                        log.LogMethodExit();
                        return;
                    }
                }
            }
            log.LogMethodExit();
        }

        private void dgvProducts_CellFormat(object sender, DataGridViewCellFormattingEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (dgvProducts.Columns[e.ColumnIndex].DataPropertyName == "ItemMarkupPercent")
                {
                    if (e.Value != null)
                    {
                        string value = e.Value.ToString();
                        if (value.ToString() == "NaN")
                        {
                            e.Value = null;
                            e.FormattingApplied = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void dgvProducts_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            if (dgvProducts.Columns[e.ColumnIndex].DataPropertyName == "ItemMarkupPercent")
            {
                //to avoid null value error
            }
            log.LogMethodExit();
        }

        private void dgvProducts_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (dgvProducts.Columns[e.ColumnIndex].Name == "ProductBarCode")
                {
                    if (dgvProducts["productIdDataGridViewTextBoxColumn", e.RowIndex].Value != null && dgvProducts["codeDataGridViewTextBoxColumn", e.RowIndex].Value != null)
                    {
                        //Updated frm_addBarcode constructor call to see that product description is passed
                        //Updated frm_addBarcode constructor call to see that product price is passed
                        frmAddBarcode f = new frmAddBarcode(Convert.ToInt32(dgvProducts["productIdDataGridViewTextBoxColumn", e.RowIndex].Value),
                            dgvProducts["codeDataGridViewTextBoxColumn", e.RowIndex].Value.ToString(), dgvProducts["descriptionDataGridViewTextBoxColumn", e.RowIndex].Value.ToString(),
                            (dgvProducts["productIdDataGridViewTextBoxColumn", e.RowIndex].Value == DBNull.Value
                            || dgvProducts["productIdDataGridViewTextBoxColumn", e.RowIndex].Value == null) ? -1 : Convert.ToDouble(dgvProducts["costDataGridViewTextBoxColumn", e.RowIndex].Value), utilities);
                        //    Semnox.Parafait.Inventory.CommonUIDisplay.setupVisuals(f);//Added to style GUI 

                        f.StartPosition = FormStartPosition.CenterScreen;//Added to show at Center
                        f.ShowDialog();
                        ProductSearch();
                    }
                    else
                    {

                        MessageBox.Show(MessageContainerList.GetMessage(machineUserContext, 1018), MessageContainerList.GetMessage(machineUserContext, "Add Barcode"));
                    }
                }
                // to add a button for SKU segement
                if (dgvProducts.Columns[e.ColumnIndex].Name == "SegmentCategoryId")
                {
                    if (dgvProducts["productIdDataGridViewTextBoxColumn", e.RowIndex].Value != null && dgvProducts["codeDataGridViewTextBoxColumn", e.RowIndex].Value != null)
                    {
                        segmentCategorizationValueUI = new SegmentCategorizationValueUI(utilities, utilities.ParafaitEnv, "Product", Convert.ToInt32(dgvProducts["productIdDataGridViewTextBoxColumn", e.RowIndex].Value), ((dgvProducts["SegmentCategoryId", e.RowIndex].Value) == null || dgvProducts["SegmentCategoryId", e.RowIndex].Value == DBNull.Value) ? -1 : (int)Convert.ToInt32(dgvProducts["SegmentCategoryId", e.RowIndex].Value), dgvProducts["descriptionDataGridViewTextBoxColumn", e.RowIndex].Value.ToString());
                        segmentCategorizationValueUI.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show(MessageContainerList.GetMessage(machineUserContext, 1017), MessageContainerList.GetMessage(machineUserContext, "Add Segment"));
                    }
                }//to add a button for SKU segement
                //to add a button for updating custom attributes
                if (dgvProducts.Columns[e.ColumnIndex].Name == "CustomDataSetId")
                {
                    if (dgvProducts["productIdDataGridViewTextBoxColumn", e.RowIndex].Value != null && dgvProducts["codeDataGridViewTextBoxColumn", e.RowIndex].Value != null)
                    {
                        CustomDataUI cd;
                        try
                        {
                            cd = new CustomDataUI("INVPRODUCT", Convert.ToInt32(dgvProducts["productIdDataGridViewTextBoxColumn", e.RowIndex].Value), null, utilities);
                            cd.ShowDialog();
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                            log.LogMethodExit();
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show(MessageContainerList.GetMessage(machineUserContext, 1010), MessageContainerList.GetMessage(machineUserContext, "Add Segment"));
                    }
                }//to add a button for updating custom attributes
                if(dgvProducts.Columns[e.ColumnIndex].Name == "btnRecipeDescription")
                {
                    try
                    {
                        int productId = dtoProductLoaded[e.RowIndex].ProductId;
                        string productGuid = dtoProductLoaded[e.RowIndex].Guid;
                        if (productId > -1)
                        {
                            string tableObject = "PRODUCT";
                            string tableElement = "RECIPEDESCRIPTION";

                            HtmlEditorUI htmlEditorUI = new HtmlEditorUI(utilities, tableObject, productId, tableElement, productGuid, true, "PRODUCT");
                            htmlEditorUI.ShowDialog();
                        }
                        else
                        {
                            MessageBox.Show(utilities.MessageUtils.getMessage("Please save Product before adding Recipe description."), utilities.MessageUtils.getMessage("Add Recipe Description"));
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        log.Error(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit();
        }

        //Added method to see that top 1 barcode is shown in the screen
        private void dgvProducts_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (e.Row.Index > -1)
                {
                    if (e.Row.Cells["productIdDataGridViewTextBoxColumn"].Value != null && e.Row.Cells["codeDataGridViewTextBoxColumn"].Value != null)
                    {
                        if (e.Row.Cells["BarCode"].Value == null)
                        {
                            ProductBarcodeListBL productBarcodeListBL = new ProductBarcodeListBL(machineUserContext);
                            List<KeyValuePair<ProductBarcodeDTO.SearchByParameters, string>> SearchParameter = new List<KeyValuePair<ProductBarcodeDTO.SearchByParameters, string>>();
                            SearchParameter.Add(new KeyValuePair<ProductBarcodeDTO.SearchByParameters, string>(ProductBarcodeDTO.SearchByParameters.PRODUCT_ID, e.Row.Cells["productIdDataGridViewTextBoxColumn"].Value.ToString()));
                            List<ProductBarcodeDTO> productBarcodeListOnDisplay = productBarcodeListBL.GetProductBarcodeDTOList(SearchParameter);

                            if (productBarcodeListOnDisplay != null)
                            {
                                e.Row.Cells["BarCode"].Value = productBarcodeListOnDisplay[0].BarCode;
                            }
                            else
                                e.Row.Cells["BarCode"].Value = "";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        int GetDefaultStoreId()
        {
            log.LogMethodEntry();
            List<LocationDTO> storeLocationDTOList;
            LocationList locationList = new LocationList(machineUserContext);
            int indexValue = -1;
            string indexNameValue = "";
            storeLocationDTOList = locationList.GetAllLocations("Store");
            if (storeLocationDTOList != null)
            {
                foreach (LocationDTO storeLocationDTO in storeLocationDTOList)
                {
                    if (storeLocationDTO.IsStore == "Y")
                    {
                        indexValue = storeLocationDTO.LocationId;
                        indexNameValue = storeLocationDTO.Name;
                        break;
                    }
                }
            }
            log.LogMethodExit(indexValue);
            return indexValue;
        }

        /// <summary>
        /// Set Manual Product Type 
        /// </summary>
        void SetManualProductTypeId()
        {
            log.LogMethodEntry();
            ProductTypeListBL productTypeListBL = new ProductTypeListBL(machineUserContext);
            List<KeyValuePair<ProductTypeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ProductTypeDTO.SearchByParameters, string>>
            {
                new KeyValuePair<ProductTypeDTO.SearchByParameters, string>(ProductTypeDTO.SearchByParameters.PRODUCT_TYPE, "MANUAL"),
                new KeyValuePair<ProductTypeDTO.SearchByParameters, string>(ProductTypeDTO.SearchByParameters.SITE_ID, machineUserContext.GetSiteId().ToString())};
            List<ProductTypeDTO> listProductTypeDTOs = productTypeListBL.GetProductTypeDTOList(searchParameters);
            if (listProductTypeDTOs != null && listProductTypeDTOs.Count > 0)
            {
                manualProductTypeId = listProductTypeDTOs[0].ProductTypeId;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Method to Update POSProductionInformation
        /// </summary>
        /// <param name="productsDTO"></param>
        private void UpdatePOSProductInformation(ProductsDTO productsDTO)
        {
            log.LogMethodEntry(productsDTO);
            if (productsDTO.InventoryItemDTO != null)
            {
                if (productsDTO.CategoryId != productsDTO.InventoryItemDTO.CategoryId)
                {
                    productsDTO.CategoryId = productsDTO.InventoryItemDTO.CategoryId;
                }
                if (productsDTO.ProductName != productsDTO.InventoryItemDTO.ProductName)
                {
                    productsDTO.ProductName = productsDTO.InventoryItemDTO.ProductName;
                }
                if (productsDTO.Description != productsDTO.InventoryItemDTO.Description)
                {
                    productsDTO.Description = productsDTO.InventoryItemDTO.Description;
                }

                if (productsDTO.ActiveFlag != productsDTO.InventoryItemDTO.IsActive)
                {
                    productsDTO.ActiveFlag = productsDTO.InventoryItemDTO.IsActive;
                }
                if (productsDTO.InventoryItemDTO.ProductId == -1)
                {
                    productsDTO.ProductTypeId = manualProductTypeId;
                    productsDTO.DisplayInPOS = "N";
                    productsDTO.InventoryProductCode = productsDTO.InventoryItemDTO.Code;
                    productsDTO.MapedDisplayGroup = manualproductDisplaygroup;
                    if (productsDTO.Price != (decimal)productsDTO.InventoryItemDTO.SalePrice)
                    {
                        if (productsDTO.InventoryItemDTO.SalePrice == -1)
                        {
                            productsDTO.Price = 0;
                        }
                        else
                        {
                            productsDTO.Price = (decimal)productsDTO.InventoryItemDTO.SalePrice;
                        }
                    }
                }
            }
            log.LogMethodExit();
        }


        private void PopulateDisplayGroup()
        {
            log.LogMethodEntry();
            List<ProductDisplayGroupFormatDTO> displayGroupDTOList;
            ProductDisplayGroupList displayGroupList = new ProductDisplayGroupList(machineUserContext);
            List<KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>> displayGroupDTOSearchParams;
            displayGroupDTOSearchParams = new List<KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>>();
            BindingSource displayGroupBS = new BindingSource();
            displayGroupDTOSearchParams.Add(new KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>(ProductDisplayGroupFormatDTO.SearchByDisplayParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            displayGroupDTOList = displayGroupList.GetAllProductDisplayGroup(displayGroupDTOSearchParams, false, false, null);
            if (displayGroupDTOList == null)
            {
                displayGroupDTOList = new List<ProductDisplayGroupFormatDTO>();
            }
            displayGroupDTOList.Insert(0, new ProductDisplayGroupFormatDTO());
            displayGroupDTOList[0].DisplayGroup = "<All>";
            displayGroupBS.DataSource = displayGroupDTOList.OrderBy(x => x.DisplayGroup);
            cmbDisplayGroup.DataSource = displayGroupBS;
            cmbDisplayGroup.ValueMember = "Id";
            cmbDisplayGroup.DisplayMember = "DisplayGroup";
            log.LogMethodExit();
        }

        private void CreateHeaderCheckBox()
        {
            log.LogMethodEntry();
            cbxHeaderSelect = new CustomCheckBox();
            cbxHeaderSelect.FlatAppearance.BorderSize = 0;
            cbxHeaderSelect.ImageAlign = ContentAlignment.MiddleCenter;
            cbxHeaderSelect.FlatAppearance.MouseDownBackColor = Color.Transparent;
            cbxHeaderSelect.FlatAppearance.MouseOverBackColor = Color.Transparent;
            cbxHeaderSelect.FlatAppearance.CheckedBackColor = Color.Transparent;
            cbxHeaderSelect.Text = string.Empty;
            cbxHeaderSelect.Font = dgvProducts.Font;
            cbxHeaderSelect.Location = new Point(dgvProducts.Columns["SetProducts"].HeaderCell.ContentBounds.X + 40,
                                                    dgvProducts.Columns["SetProducts"].HeaderCell.ContentBounds.Y + 13);
            cbxHeaderSelect.BackColor = Color.Transparent;
            cbxHeaderSelect.Size = new Size(60, 28);
            cbxHeaderSelect.Click += new EventHandler(HeaderCheckBox_Clicked);
            dgvProducts.Controls.Add(cbxHeaderSelect);
            log.LogMethodExit();
        }

        private void HeaderCheckBox_Clicked(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                this.Cursor = Cursors.WaitCursor;
                dgvProducts.EndEdit();
                CheckBox headerCheckBox = (sender as CheckBox);
                int bulkPublishLimit = GetBulkPublishLimit();
                if (headerCheckBox.Checked)
                {
                    if (dtoProductDetails.Count > bulkPublishLimit)
                    {
                        if (MessageBox.Show(MessageContainerList.GetMessage(machineUserContext, 2733, dtoProductDetails.Count), "Bulk Publish Process", MessageBoxButtons.YesNo) == DialogResult.No)  //"Do you want to select &1 records for publish?"
                        {
                            headerCheckBox.Checked = false;
                            log.LogMethodExit();
                            return;
                        }
                    }
                    LoadRemainingDGVProductList();
                }
                for (int rowIndex = 0; rowIndex < dgvProducts.Rows.Count - 1; rowIndex++)
                {
                    DataGridViewCheckBoxCell checkBox = (dgvProducts["SetProducts", rowIndex] as DataGridViewCheckBoxCell);
                    checkBox.Value = headerCheckBox.Checked;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(MessageContainerList.GetMessage(machineUserContext, 1824, ex.Message));
            }
            finally
            {
                this.Cursor = Cursors.Default;

            }
            log.LogMethodExit();
        }

        private int GetBulkPublishLimit()
        {
            log.LogMethodEntry();
            List<LookupValuesDTO> lookupValuesDTOList;
            int bulkPublishLimit = 0;
            try
            {
                LookupValuesList lookupValuesList = new LookupValuesList(machineUserContext);
                lookupValuesDTOList = lookupValuesList.GetInventoryLookupValuesByValueName("PUBLISH_SETUP", machineUserContext.GetSiteId());
                if (lookupValuesDTOList != null)
                {
                    LookupValuesDTO lookupValuesDTO = lookupValuesDTOList.Where(x => x.Description == "LIMIT_FOR_BULK_PUBLISH").FirstOrDefault();
                    if (lookupValuesDTO != null)
                    {
                        bulkPublishLimit = Convert.ToInt32(lookupValuesDTO.LookupValue);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(MessageContainerList.GetMessage(machineUserContext, 1824, ex.Message));
            }
            log.LogMethodExit(bulkPublishLimit);
            return bulkPublishLimit;

        }

        private void dgvProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (e.ColumnIndex > -1 && e.RowIndex > -1)
                {
                    if (e.ColumnIndex == 0 && e.RowIndex >= 0)
                    {
                        if (dgvProducts.Rows[e.RowIndex].Cells["SetProducts"].Value != null && (bool)dgvProducts.Rows[e.RowIndex].Cells["SetProducts"].Value)
                        {
                            dgvProducts.Rows[e.RowIndex].Cells["SetProducts"].Value = false;
                        }
                        else
                        {
                            dgvProducts.Rows[e.RowIndex].Cells["SetProducts"].Value = true;
                        }
                        dgvProducts.RefreshEdit();
                    }
                    if (dgvProducts.Columns[e.ColumnIndex].Name == "marketListItemDataGridViewCheckBoxColumn"
                            || dgvProducts.Columns[e.ColumnIndex].Name == "CostIncludeTaxDataGridViewCheckBoxColumn"
                            || dgvProducts.Columns[e.ColumnIndex].Name == "autoUpdateMarkupDataGridViewCheckBoxColumn"
                            || dgvProducts.Columns[e.ColumnIndex].Name == "isActiveDataGridViewTextBoxColumn"
                            || dgvProducts.Columns[e.ColumnIndex].Name == "lotControlledDataGridViewCheckBoxColumn")
                    {
                        DataGridViewCheckBoxCell checkBox = dgvProducts.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewCheckBoxCell;
                        if (Convert.ToBoolean(checkBox.Value))
                        {
                            checkBox.Value = false;
                        }
                        else
                        {
                            checkBox.Value = true;
                        }
                    }
                    if (dgvProducts.Columns[e.ColumnIndex].Name == "taxInclusiveCostDataGridViewTextBoxColumn"
                            || dgvProducts.Columns[e.ColumnIndex].Name == "isSellableDataGridViewTextBoxColumn"
                            || dgvProducts.Columns[e.ColumnIndex].Name == "isRedeemableDataGridViewTextBoxColumn"
                            || dgvProducts.Columns[e.ColumnIndex].Name == "isPurchaseableDataGridViewTextBoxColumn")
                    {
                        DataGridViewCheckBoxCell checkBox = dgvProducts.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewCheckBoxCell;
                        if (checkBox.Value.ToString() == "Y")
                        {
                            checkBox.Value = "N";
                        }
                        else
                        {
                            checkBox.Value = "Y";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        private void lnkPublishToSite_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                this.Cursor = Cursors.WaitCursor;
                List<KeyValuePair<int, string>> productListToPublish = new List<KeyValuePair<int, string>>();
                bool valid = ValidateSetProductsCheckbox();
                if (!valid)
                {
                    MessageBox.Show(MessageContainerList.GetMessage(machineUserContext, 218), MessageContainerList.GetMessage(machineUserContext, "Validation Error")); //Choose Product 
                    log.LogMethodExit();
                    return;
                }
                int selectedProductCount = GetSelectedProductCount();
                int bulkPublishLimit = GetBulkPublishLimit();
                for (int rowIndex = 0; rowIndex < dtoProductLoaded.Count; rowIndex++)
                {
                    if (dgvProducts.Rows[rowIndex].Cells["SetProducts"].Value != null && (bool)dgvProducts.Rows[rowIndex].Cells["SetProducts"].Value)
                    {
                        int id = Convert.ToInt32(dgvProducts.Rows[rowIndex].Cells["productIdDataGridViewTextBoxColumn"].Value);
                        string code = Convert.ToString(dgvProducts.Rows[rowIndex].Cells["codeDataGridViewTextBoxColumn"].Value);
                        productListToPublish.Add(new KeyValuePair<int, string>(id, code));
                        dgvProducts.Rows[rowIndex].Cells["SetProducts"].Value = false;
                    }

                }
                if (productListToPublish.Count > 0)
                {
                    //You have selected &1 records .Do you want to publish them in batches? (Batch size is &2)
                    if (productListToPublish.Count > bulkPublishLimit
                        && MessageBox.Show(MessageContainerList.GetMessage(machineUserContext, 2748, productListToPublish.Count, bulkPublishLimit),
                                       MessageContainerList.GetMessage(machineUserContext, "Bulk Publish Process"), MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        log.LogMethodExit();
                        return;
                    }
                    int index = 0;
                    int publishedProducts = 0;
                    int remainingProductsToPublish = productListToPublish.Count;
                    while (publishedProducts < productListToPublish.Count)
                    {
                        this.Cursor = Cursors.WaitCursor;
                        int rangeEnd = 0;
                        index = publishedProducts;
                        if (productListToPublish.Count > bulkPublishLimit)
                        {
                            rangeEnd = (remainingProductsToPublish > bulkPublishLimit ? bulkPublishLimit : remainingProductsToPublish);
                        }
                        else
                        {
                            rangeEnd = productListToPublish.Count;
                        }
                        CallPublishUI(productListToPublish.GetRange(index, rangeEnd).ToList());

                        publishedProducts += rangeEnd;
                        remainingProductsToPublish = productListToPublish.Count - publishedProducts;
                        if (publishedProducts < productListToPublish.Count //Do you want to publish next &1 records? (Remaining Products to publish - &)
                            && (MessageBox.Show(MessageContainerList.GetMessage(machineUserContext, 2732, bulkPublishLimit, remainingProductsToPublish),
                                                MessageContainerList.GetMessage(machineUserContext, "Bulk Publish Process"), MessageBoxButtons.YesNo) == DialogResult.No)) 
                        {
                            break;
                        }
                        this.Cursor = Cursors.WaitCursor;
                    }
                    cbxHeaderSelect.Checked = false;
                }

            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(MessageContainerList.GetMessage(machineUserContext, 1824, ex.Message));
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit();

        }
        private void CallPublishUI(List<KeyValuePair<int, string>> productListToPublish)
        {
            log.LogMethodEntry();
            using (PublishUI publishUI = new PublishUI(utilities, productListToPublish, "Product"))
            {
                publishUI.ShowDialog();
                productListToPublish.Clear();
            }
            log.LogMethodExit();
        }
        private int GetSelectedProductCount()
        {
            log.LogMethodEntry();
            int count = 0;
            foreach (DataGridViewRow dataGridRow in dgvProducts.Rows)
            {
                if (dataGridRow.Cells["SetProducts"].Value != null && (bool)dataGridRow.Cells["SetProducts"].Value)
                {
                    count++;
                }
                else
                {
                    continue;
                }
            }
            log.LogMethodExit(count);
            return count;
        }

        private bool ValidateSetProductsCheckbox()
        {
            log.LogMethodEntry();
            bool isChecked = false;
            foreach (DataGridViewRow dataGridRow in dgvProducts.Rows)
            {
                if (dataGridRow.Cells["SetProducts"].Value != null && (bool)dataGridRow.Cells["SetProducts"].Value)
                {
                    isChecked = true;
                    break;
                }
                else
                {
                    continue;
                }
            }
            log.LogMethodExit(isChecked);
            return isChecked;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                this.Cursor = Cursors.WaitCursor;
                ProductSearch();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(MessageContainerList.GetMessage(machineUserContext, 1824, ex.Message));
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }

        private void ProductSearch()
        {
            log.LogMethodEntry();
            try
            {
                cbxHeaderSelect.Checked = false;
                ProductList productList = new ProductList(machineUserContext);
                List<KeyValuePair<ProductDTO.SearchByProductParameters, string>> SearchParameter = new List<KeyValuePair<ProductDTO.SearchByProductParameters, string>>();
                SearchParameter.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.CODE, txtCode.Text));
                if (cmbProductType.SelectedIndex == 1)
                {
                    SearchParameter.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.ISREDEEMABLE, "Y"));
                }
                else if (cmbProductType.SelectedIndex == 2)
                {
                    SearchParameter.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.ISSELLABLE, "Y"));
                }
                if (cmbActive.SelectedIndex == 1)
                {
                    SearchParameter.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.IS_ACTIVE, "Y"));
                }
                else if (cmbActive.SelectedIndex == 2)
                {
                    SearchParameter.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.IS_ACTIVE, "N"));
                }
                if (cmbProductCategory.SelectedIndex > 0)
                {
                    CategoryList categoryList = new CategoryList(machineUserContext);
                    List<KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>> SearchCategoryParameter = new List<KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>>();
                    SearchCategoryParameter.Add(new KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>(CategoryDTO.SearchByCategoryParameters.CATEGORY_ID, cmbProductCategory.SelectedValue.ToString()));
                    List<CategoryDTO> categoryListDTO = categoryList.GetAllCategory(SearchCategoryParameter);
                    if (categoryListDTO != null)
                    {
                        SearchParameter.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.CATEGORY, categoryListDTO[0].CategoryId.ToString()));
                    }
                }
                if (cmbVendor.SelectedIndex > 0)
                {
                    VendorList vendorList = new VendorList(machineUserContext);
                    List<KeyValuePair<VendorDTO.SearchByVendorParameters, string>> SearchVendorParameter = new List<KeyValuePair<VendorDTO.SearchByVendorParameters, string>>();
                    SearchVendorParameter.Add(new KeyValuePair<VendorDTO.SearchByVendorParameters, string>(VendorDTO.SearchByVendorParameters.VENDOR_ID, cmbVendor.SelectedValue.ToString()));
                    List<VendorDTO> vendorListDTO = vendorList.GetAllVendors(SearchVendorParameter);
                    if (vendorListDTO != null)
                    {
                        SearchParameter.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.VENDOR_ID, vendorListDTO[0].VendorId.ToString()));
                    }
                }
                if (cmbDisplayGroup.SelectedIndex > 0)
                {
                    ProductDisplayGroupList displaygroupList = new ProductDisplayGroupList(machineUserContext);
                    List<KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>> SearchVendorParameter = new List<KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>>();
                    SearchVendorParameter.Add(new KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>(ProductDisplayGroupFormatDTO.SearchByDisplayParameters.DISPLAY_GROUP_ID, cmbDisplayGroup.SelectedValue.ToString()));
                    List<ProductDisplayGroupFormatDTO> productDisplayGroupFormatDTO = displaygroupList.GetAllProductDisplayGroup(SearchVendorParameter);
                    if (productDisplayGroupFormatDTO != null)
                    {
                        SearchParameter.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.DISPLAY_GROUP_NAME, productDisplayGroupFormatDTO[0].DisplayGroup.ToString()));
                    }
                }
                if (txtDescription.Text.Trim() != string.Empty)
                {
                    SearchParameter.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.DESCRIPTION, txtDescription.Text));
                }
                if (txtProductName.Text.Trim() != string.Empty)
                {
                    SearchParameter.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.PRODUCT_NAME, txtProductName.Text));
                }
                if (txtBarcode.Text.Trim() != string.Empty)
                {
                    SearchParameter.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.BARCODE, txtBarcode.Text));
                }
                if (cbxLot.Checked)
                {
                    SearchParameter.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.LOT_CONTROLLABLE, "1"));
                }
                else
                {
                    SearchParameter.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.LOT_CONTROLLABLE, "0"));
                }
                if (AdvancedSearch != null)
                {
                    if (!string.IsNullOrEmpty(AdvancedSearch.searchCriteria))
                    {
                        SearchParameter.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                        List<ProductDTO> productLists = productList.GetSearchCriteriaAllProducts(SearchParameter, AdvancedSearch);
                        if (productLists == null)
                        {
                            productLists = new List<ProductDTO>();
                            PopulateProductGrid(productLists);
                        }
                        else
                        {
                            PopulateProductGrid(productLists);
                        }
                    }
                }
                else
                {
                    SearchParameter.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                    List<ProductDTO> productLists = productList.GetSearchCriteriaAllProducts(SearchParameter, null);
                    if (productLists == null)
                    {
                        productLists = new List<ProductDTO>();
                        PopulateProductGrid(productLists);
                    }
                    else
                    {
                        PopulateProductGrid(productLists);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(MessageContainerList.GetMessage(machineUserContext, 1824, ex.Message));
            }
            log.LogMethodExit();
        }


        private void btnAdvancedSearch_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (AdvancedSearch == null)
                {
                    if (segmentsExist())
                    {
                        AdvancedSearch = new AdvancedSearch(utilities, "Product", "p");
                        CommonUIDisplay.setupVisuals(AdvancedSearch);
                        AdvancedSearch.StartPosition = FormStartPosition.CenterScreen;
                    }
                    else
                    {
                        MessageBox.Show(MessageContainerList.GetMessage(machineUserContext, 1000));
                        log.LogMethodExit();
                        return;
                    }
                }
                if (AdvancedSearch.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        ProductSearch();

                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        MessageBox.Show(MessageContainerList.GetMessage(machineUserContext, 1824, ex.Message));
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(MessageContainerList.GetMessage(machineUserContext, 1824, ex.Message));
            }
            finally
            {
                AdvancedSearch = null;
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }


        private bool segmentsExist()
        {
            log.LogMethodEntry();
            try
            {
                SegmentDefinitionList segmentDefinitionList = new SegmentDefinitionList(machineUserContext);
                List<SegmentDefinitionDTO> segmentDefinitionDTOList = new List<SegmentDefinitionDTO>();
                List<KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>> segmentDefinitionSearchParams = new List<KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>>();
                segmentDefinitionSearchParams = new List<KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>>();
                segmentDefinitionSearchParams.Add(new KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>(SegmentDefinitionDTO.SearchBySegmentDefinitionParameters.IS_ACTIVE, "Y"));
                segmentDefinitionDTOList = segmentDefinitionList.GetAllSegmentDefinitions(segmentDefinitionSearchParams);
                if (segmentDefinitionDTOList == null)
                {
                    log.LogMethodExit(false);
                    return false;
                }
                else
                {
                    log.LogMethodExit(true);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(ex.Message);
                log.LogMethodExit(false);
                return false;
            }
        }


        private void btnClear_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                this.Cursor = Cursors.WaitCursor;
                ClearSearchFields();
                ProductSearch();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(MessageContainerList.GetMessage(machineUserContext, 1824, ex.Message));
            }
            finally
            {
                AdvancedSearch = null;
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }

        private void ClearSearchFields()
        {
            log.LogMethodEntry();
            cbxHeaderSelect.Checked = false;
            txtBarcode.Text = string.Empty;
            txtCode.Text = string.Empty;
            txtDescription.Text = string.Empty;
            txtProductName.Text = string.Empty;
            cmbActive.SelectedIndex = 1;
            cmbVendor.SelectedIndex = 0;
            cmbProductType.SelectedIndex = 0;
            cmbProductCategory.SelectedIndex = 0;
            cmbDisplayGroup.SelectedIndex = 0;
            cbxLot.Checked = false;
            log.LogMethodExit();
        }


        private void dataGridViewProduct_Scroll(object sender, ScrollEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                int display = dgvProducts.Rows.Count - dgvProducts.DisplayedRowCount(false);
                if (e.ScrollOrientation.Equals(System.Windows.Forms.ScrollOrientation.VerticalScroll) &&
                    (e.Type == ScrollEventType.SmallIncrement || e.Type == ScrollEventType.LargeIncrement))
                {
                    if (e.NewValue >= dgvProducts.Rows.Count - GetProductDGVDisplayedRowsCount(dgvProducts))
                    {
                        log.LogVariableState("e.NewValue", e.NewValue);
                        dgvProducts.DataSource = LoadNextPageForDGVProduct();
                        dgvProducts.ClearSelection();
                        dgvProducts.FirstDisplayedScrollingRowIndex = display;
                        dgvProducts.Refresh();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(MessageContainerList.GetMessage(machineUserContext, 1824, ex.Message));
            }
            log.LogMethodExit();
        }

        private int GetProductDGVDisplayedRowsCount(DataGridView dvgObject)
        {
            log.LogMethodEntry(dvgObject);
            int displayedRowsCount = dvgObject.Rows[dvgObject.FirstDisplayedScrollingRowIndex].Height;
            displayedRowsCount = dvgObject.Height / displayedRowsCount;
            log.LogMethodExit(displayedRowsCount);
            return displayedRowsCount;
        }

        private SortableBindingList<ProductDTO> LoadNextPageForDGVProduct()
        {
            log.LogMethodEntry();
            if (dtoProductLoaded.Count < dtoProductDetails.Count)
            {
                IEnumerable<ProductDTO> nextSetOfRows = dtoProductDetails.AsEnumerable().Skip((dgvProductPageNo - 1) * dgvProductPageSize).Take(dgvProductPageSize);
                if (nextSetOfRows.Any())
                {
                    log.LogVariableState("Do  dgvProductPageNo.Add", nextSetOfRows.Count());
                    foreach (ProductDTO row in nextSetOfRows)
                    {
                        dtoProductLoaded.Add(row);
                    }
                    IncrementDGVProductPageNo();
                }
            }
            log.LogMethodExit(dtoProductLoaded);
            return dtoProductLoaded;
        }

        private void IncrementDGVProductPageNo()
        {
            log.LogMethodEntry();
            dgvProductPageNo++;
            log.LogMethodExit();
        }

        private void LoadRemainingDGVProductList()
        {
            log.LogMethodEntry();
            this.Cursor = Cursors.WaitCursor;
            if (dtoProductLoaded.Count < dtoProductDetails.Count)
            {
                dgvProducts.DataSource = FullLoadForDGVGamePlay();
            }
            log.LogMethodExit();
        }

        private SortableBindingList<ProductDTO> FullLoadForDGVGamePlay()
        {
            log.LogMethodEntry();
            if (dtoProductLoaded.Count < dtoProductDetails.Count)
            {
                dtoProductLoaded = new SortableBindingList<ProductDTO>(dtoProductDetails);
                dgvProductPageNo = Convert.ToInt32(dtoProductDetails.Count / dgvProductPageSize);
            }
            log.LogMethodExit(dtoProductLoaded);
            return dtoProductLoaded;
        }

        private void dgvProducts_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (e.ColumnIndex > -1)
                {
                    if (dgvProducts.Columns[e.ColumnIndex].SortMode == DataGridViewColumnSortMode.Automatic)
                    {
                        LoadRemainingDGVProductList();
                        SortDGVByColumnIndex(e.ColumnIndex);
                    }
                    for(int i = 0; i < dtoProductLoaded.Count;i++)
                    {
                        int uomId = dtoProductLoaded[i].InventoryUOMId;
                        CommonFuncs.GetUOMComboboxForSelectedRows(dgvProducts, i, uomId);
                    }
                    
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }

        private void SortDGVByColumnIndex(int columnIndex)
        {
            log.LogMethodEntry(columnIndex);
            ListSortDirection direction;
            SortOrder oldSortOrder = dgvProductSortOrder;
            DataGridViewColumn newColumn = dgvProducts.Columns[columnIndex];
            // If oldColumn is null, then the DataGridView is not currently sorted.
            if (oldSortColumn != null)
            {
                // Sort the same column again, reversing the SortOrder.
                if (oldSortColumn == newColumn &&
                    oldSortOrder == SortOrder.Ascending)
                {
                    direction = ListSortDirection.Descending;
                    dgvProductSortOrder = SortOrder.Descending;
                    oldSortColumn = newColumn;
                }
                else
                {
                    // Sort a new column and remove the old SortGlyph.
                    direction = ListSortDirection.Ascending;
                    oldSortColumn.HeaderCell.SortGlyphDirection = SortOrder.None;
                    dgvProductSortOrder = SortOrder.Ascending;
                    oldSortColumn = newColumn;
                }
            }
            else
            {
                dgvProductSortOrder = SortOrder.Ascending;
                oldSortColumn = newColumn;
                direction = ListSortDirection.Ascending;
            }

            // If no column has been selected, display an error dialog  box.
            if (newColumn != null)
            {

                dgvProducts.Sort(newColumn, direction);
                newColumn.HeaderCell.SortGlyphDirection =
                    direction == ListSortDirection.Ascending ?
                    SortOrder.Ascending : SortOrder.Descending;
            }
            log.LogMethodExit();
        }

        private void PopulateItemType()
        {
            log.LogMethodEntry();            
            List<LookupValuesDTO> lookupValuesDTOList = new List<LookupValuesDTO>();
            LookupValuesList lookupValuesList = new LookupValuesList(machineUserContext);
            lookupValuesDTOList = lookupValuesList.GetInventoryLookupValuesByValueName("PRODUCT_ITEM_TYPE", machineUserContext.GetSiteId());
            LookupValuesDTO lookUpValueDTO = lookupValuesDTOList.Find(x => x.LookupValue == "STANDARD_ITEM");
            if (lookUpValueDTO != null)
            {
                defaultItemTypeId = lookUpValueDTO.LookupValueId;
            }
            if (lookupValuesDTOList == null)
            {
                lookupValuesDTOList = new List<LookupValuesDTO>();
            }
            lookupValuesDTOList.Insert(0, new LookupValuesDTO());
            cmbItemType.DataSource = lookupValuesDTOList;
            cmbItemType.ValueMember = "LookupValueId";
            cmbItemType.DisplayMember = "LookUpValue";
            log.LogMethodExit();
        }
    }
}
