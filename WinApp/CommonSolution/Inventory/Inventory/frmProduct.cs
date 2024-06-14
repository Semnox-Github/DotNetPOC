/********************************************************************************************
*Project Name -  frmProducts                                                                         
*Description  -
*************
**Version Log
*************
*Version     Date                   Modified By                 Remarks          
*********************************************************************************************
*1.00        11-Feb-2019            Archana                     Redemption gift search and inventory
*                                                               UI changes
*2.60        11-Apr-2019            Archana                     Include/Exclude pos products from
*                                                               Redemption 
 *******************************************************************************************
*2.60        11-Apr-2019            Girish                      Modified : Replacing purchaseTax 3 tier with Tax 3 tier
 *****************************************************************************************
 *2.60       28-Jun-2019            Archana                     Modified : Added stock link to show item stock information
 *  *******************************************************************************************
*2.70.2      13-Aug-2019            Deeksha                     Modified : Added logger methods.
*2.80.0      21-Jan-2020            Deeksha                     Modified : Re enabling New button in the inventory Module
*2.80.0      04-Jun-2020            Deeksha                     Modified : Adding display group option in the inventory product UI
*2.80.0      25-Jun-2020            Deeksha                     Modified to Make Product module read only in Windows Management Studio.
*2.90.0      02-Jul-2020            Deeksha                     Inventory process : Weighted Avg Costing changes.
*2.100.0     23-Aug-2020            Deeksha                     Modified for Recipe Management Enhancement.
*2.110.0     05-Feb-2021            Girish Kundar               Modified : UI fixes for resolution/messages/refresh UI 
*2.120.0     06-May-2021            Mushahid Faizan             Modified : Removed Literals number to Literals name.
*2.130.0     06-May-2021            Deeksha                     Modified : Issue Fix: Unable to create products when there is no products in Inventory.
 ******************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Category;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Product;
using Semnox.Parafait.Publish;
using Semnox.Parafait.Vendor;

namespace Semnox.Parafait.Inventory
{
    public partial class frmProduct : Form
    {
        private string scannedBarcode = "";
        private Utilities utilities;
        private SegmentCategorizationValueUI segmentCategorizationValueUI;
        private LocationUI LocationUI;
        private CategoryUI CategoryUI;
        private PurchaseTaxUI PurchaseTaxUI;
        private UOMUI UOMUI;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notInitCase = true;
        private int pProductId;
        private int manualProductId;
        private string imageFolder;
        private ProductDTO productDTO = null;
        private ProductsDTO productsDTO = null;
        private bool isInventory;
        private int manualProductTypeId = -1;
        private const string manualproductDisplaygroup = "Parafait Inventory Products";
        private Semnox.Core.Utilities.ExecutionContext machineUserContext = Semnox.Core.Utilities.ExecutionContext.GetExecutionContext();
        private ManagementStudioSwitch managementStudioSwitch;
        private AdvancedSearch AdvancedSearch;
        private int duplicateFromManualProductId = -1;

        public frmProduct(int manualProductId, Utilities _Utilities, bool isInventory)
        {
            log.LogMethodEntry(manualProductId, _Utilities, isInventory);
            InitializeComponent();
            PopulateProductDTO(manualProductId);
            if (productDTO != null)
            {
                pProductId = productDTO.ProductId;
            }
            else
            {
                pProductId = -1;
                productDTO = new ProductDTO();
            }
            this.manualProductId = manualProductId;
            utilities = _Utilities;
            if (Semnox.Parafait.Inventory.CommonFuncs.Utilities == null)
            {
                Semnox.Parafait.Inventory.CommonFuncs.Utilities = this.utilities;
                Semnox.Parafait.Inventory.CommonFuncs.ParafaitEnv = this.utilities.ParafaitEnv;
            }
            this.isInventory = isInventory;
            imageFolder = utilities.getParafaitDefaults("IMAGE_DIRECTORY");

            AdjustDisplayComponents(isInventory);

            InitializeVariables();

            if (utilities.ParafaitEnv.IsCorporate)
            {
                machineUserContext.SetSiteId(utilities.ParafaitEnv.SiteId);
            }
            else
            {
                machineUserContext.SetSiteId(-1);
            }
            machineUserContext.SetUserId(utilities.ParafaitEnv.LoginID);
            if (utilities.ParafaitEnv.IsCorporate && utilities.ParafaitEnv.IsMasterSite)
            {
                lnkPublishToSite.Visible = true;
            }
            else
            {
                lnkPublishToSite.Visible = false;
            }
            utilities.setLanguage(this);

            if (NoMasterAccess())
            {
                btnAddcategory.Enabled = btnAddlocation.Enabled = btnAddvendor.Enabled = btnAddUOM.Enabled = btnAddTax.Enabled = false;
            }
            HideDisplayGroupForNonInventoryCall();
            txtProductName.Focus();
            SetManualProductTypeId();
            if (isInventory == false)
            {
                managementStudioSwitch = new ManagementStudioSwitch(machineUserContext);
                UpdateUIElements();
            }
            log.LogMethodExit();
        }

        private void PopulateProductDTO(int manualProductId)
        {
            log.LogMethodEntry(manualProductId);
            productsDTO = new ProductsDTO();
            productDTO = new ProductDTO();
            Products products = new Products(machineUserContext, manualProductId, true, true);
            productsDTO = products.GetProductsDTO;
            if (productsDTO != null)
            {
                log.LogVariableState("productsDTO.InventoryItemDTO", productsDTO.InventoryItemDTO);
                if (productsDTO.InventoryItemDTO == null)
                {
                    List<ProductDTO> listProductDTO = null;
                    ProductList productList = new ProductList();
                    List<KeyValuePair<ProductDTO.SearchByProductParameters, string>> productSearchParams = new List<KeyValuePair<ProductDTO.SearchByProductParameters, string>>
                    {
                        new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.MANUAL_PRODUCT_ID, productsDTO.ProductId.ToString()),
                        new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.SITE_ID, machineUserContext.GetSiteId().ToString())
                    };
                    listProductDTO = productList.GetAllProducts(productSearchParams);
                    if (listProductDTO != null && listProductDTO.Count > 0)
                    {
                        productsDTO.InventoryItemDTO = listProductDTO[0];
                        productDTO = productsDTO.InventoryItemDTO;
                    }
                }
                else
                {
                    productDTO = productsDTO.InventoryItemDTO;
                }
            }
            log.LogMethodExit();
        }
        private Boolean NoMasterAccess()
        {
            log.LogMethodEntry();
            SqlCommand cmd = utilities.getCommand();
            cmd.CommandText = "select form_name from ManagementFormAccess " +
                        "where role_id = (select role_id from users where user_id = @user_id) " +
                        " and isnull(access_allowed, 'N') = 'N' " +
                        " and ISNULL(IsActive,1) = 1 " +
                        " and FunctionGroup ='Inventory Management' ";
            cmd.Parameters.AddWithValue("@user_id", utilities.ParafaitEnv.User_Id);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            for (int i = 0; i < dt.Rows.Count; i++)
            {

                if (dt.Rows[i]["form_name"].ToString() == "Master Data Setup")
                {
                    log.LogMethodExit(true);
                    return true;
                }
            }
            log.LogMethodExit(false);
            return false;

        }

        private void InitializeVariables()
        {
            log.LogMethodEntry();
            utilities.setupDataGridProperties(ref view_dgv);
            view_dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            productCodeToolStripLabel.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, "ProductCode:").ToString();
            toolStripLabel3.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, "Desc.:").ToString();
            toolStripLabel2.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, "Category:").ToString();
            toolStripLabel1.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, "Product Type:").ToString();
            toolStripLabel4.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, "Product Barcode:").ToString();
            toolStripLabel5.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, "Active:").ToString();
            fillByToolStripButton.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, "Search").ToString();
            tbsAdvancedSearched.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, "Adv Search").ToString();
            tsbClear.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, "Clear").ToString();
            log.LogMethodExit();
        }

        private void PopulateCategory()
        {
            log.LogMethodEntry();
            List<CategoryDTO> categoryDTOList;
            CategoryList CategoryList = new CategoryList(machineUserContext);
            List<KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>> categoryDTOSearchParams;
            categoryDTOSearchParams = new List<KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>>();
            BindingSource categoryBS = new BindingSource();
            categoryDTOSearchParams.Add(new KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>(CategoryDTO.SearchByCategoryParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            categoryDTOList = CategoryList.GetAllCategory(categoryDTOSearchParams);
            if (categoryDTOList == null)
            {
                categoryDTOList = new List<CategoryDTO>();
            }
            categoryDTOList.Insert(0, new CategoryDTO());
            categoryBS.DataSource = categoryDTOList.OrderBy(category => category.Name);
            ddlCategory.DataSource = categoryBS;
            ddlCategory.ValueMember = "CategoryId";
            ddlCategory.DisplayMember = "Name";

            for (int i = 0; i < categoryDTOList.Count; i++)
                cmbCategory.Items.Add(string.IsNullOrEmpty(categoryDTOList[i].Name) ? " - All -" : categoryDTOList[i].Name);
            log.LogMethodExit();
        }

        private void PopulateUOM()
        {
            log.LogMethodEntry();
            List<UOMDTO> uomDTOList;
            UOMList UOMList = new UOMList(machineUserContext);
            List<KeyValuePair<UOMDTO.SearchByUOMParameters, string>> uomDTOSearchParams;
            uomDTOSearchParams = new List<KeyValuePair<UOMDTO.SearchByUOMParameters, string>>();
            uomDTOSearchParams.Add(new KeyValuePair<UOMDTO.SearchByUOMParameters, string>(UOMDTO.SearchByUOMParameters.SITEID, machineUserContext.GetSiteId().ToString()));
            BindingSource uomBS = new BindingSource();
            uomDTOList = UOMList.GetAllUOMs(uomDTOSearchParams);
            if (uomDTOList == null)
            {
                uomDTOList = new List<UOMDTO>();
            }
            uomDTOList.Insert(0, new UOMDTO());
            ddlUOM.DataSource = uomDTOList;
            ddlUOM.ValueMember = "UOMId";
            ddlUOM.DisplayMember = "UOM";
            cmbInventoryUOM.DataSource = uomDTOList;
            cmbInventoryUOM.ValueMember = "UOMId";
            cmbInventoryUOM.DisplayMember = "UOM";
            log.LogMethodExit();
        }

        private void PopulateVendor()
        {
            log.LogMethodEntry();
            List<VendorDTO> vendorDTOList;
            VendorList vendorList = new VendorList(machineUserContext);
            List<KeyValuePair<VendorDTO.SearchByVendorParameters, string>> vendorDTOSearchParams;
            vendorDTOSearchParams = new List<KeyValuePair<VendorDTO.SearchByVendorParameters, string>>();
            vendorDTOSearchParams.Add(new KeyValuePair<VendorDTO.SearchByVendorParameters, string>(VendorDTO.SearchByVendorParameters.SITEID, machineUserContext.GetSiteId().ToString()));
            BindingSource vendorBS = new BindingSource();
            vendorDTOList = vendorList.GetAllVendors(vendorDTOSearchParams);
            if (vendorDTOList == null)
            {
                vendorDTOList = new List<VendorDTO>();
            }
            vendorDTOList.Insert(0, new VendorDTO());
            vendorBS.DataSource = vendorDTOList.OrderBy(vendor => vendor.Name);
            ddlPreferredvendor.DataSource = vendorBS;
            ddlPreferredvendor.ValueMember = "VendorId";
            ddlPreferredvendor.DisplayMember = "Name";
            log.LogMethodExit();
        }

        private void PopulateTax()
        {
            log.LogMethodEntry();
            List<TaxDTO> taxDTOList;
            TaxList taxList = new TaxList(machineUserContext);
            List<KeyValuePair<TaxDTO.SearchByTaxParameters, string>> taxDTOSearchParams;
            taxDTOSearchParams = new List<KeyValuePair<TaxDTO.SearchByTaxParameters, string>>();
            taxDTOSearchParams.Add(new KeyValuePair<TaxDTO.SearchByTaxParameters, string>(TaxDTO.SearchByTaxParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            taxDTOSearchParams.Add(new KeyValuePair<TaxDTO.SearchByTaxParameters, string>(TaxDTO.SearchByTaxParameters.ACTIVE_FLAG, "1"));
            taxDTOList = taxList.GetAllTaxes(taxDTOSearchParams);
            if (taxDTOList == null)
            {
                taxDTOList = new List<TaxDTO>();
            }
            taxDTOList.Insert(0, new TaxDTO());
            ddlTax.DataSource = taxDTOList;
            ddlTax.ValueMember = "TaxId";
            ddlTax.DisplayMember = "TaxName";
            log.LogMethodExit();
        }

        private void PopulateLocation()
        {
            log.LogMethodEntry();
            List<LocationDTO> outBoundLocationDTOList;
            List<LocationDTO> inBoundLocationDTOList;
            LocationList locationList = new LocationList(machineUserContext);

            BindingSource inLocationBS = new BindingSource();
            BindingSource OutLocationBS = new BindingSource();
            inBoundLocationDTOList = locationList.GetAllLocations("Store");
            if (inBoundLocationDTOList == null)
            {
                inBoundLocationDTOList = new List<LocationDTO>();
            }

            inBoundLocationDTOList.Insert(0, new LocationDTO());
            inLocationBS.DataSource = inBoundLocationDTOList.OrderBy(location => location.Name);
            ddlDefaultlocation.DataSource = inLocationBS;
            ddlDefaultlocation.ValueMember = "LocationId";
            ddlDefaultlocation.DisplayMember = "Name";

            string outboundLocationString = "Store" + "," + "Department";
            outBoundLocationDTOList = locationList.GetAllLocations(outboundLocationString);
            if (outBoundLocationDTOList == null)
            {
                outBoundLocationDTOList = new List<LocationDTO>();
            }

            outBoundLocationDTOList.Insert(0, new LocationDTO());
            OutLocationBS.DataSource = outBoundLocationDTOList.OrderBy(location => location.Name);
            ddlOutboundLocation.DataSource = OutLocationBS;
            ddlOutboundLocation.ValueMember = "LocationId";
            ddlOutboundLocation.DisplayMember = "Name";
            log.LogMethodExit();
        }

        private string GetDefaultStoreName()
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
            log.LogMethodExit(indexNameValue);
            return indexNameValue;
        }
        private void frmProduct_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            PopulateCategory();
            PopulateUOM();
            PopulateVendor();
            PopulateTax();
            PopulateLocation();
            PopulateItemType();
            PopulateProducts(null);

            ddlCategory.SelectedIndex = -1;
            ddlDefaultlocation.SelectedIndex = -1;
            ddlPreferredvendor.SelectedIndex = -1;
            ddlOutboundLocation.SelectedIndex = -1;
            ddlTax.SelectedIndex = -1;
            ddlUOM.SelectedIndex = -1;
            btnSKUSegments.Tag = -1;
            btnCustom.Tag = -1;

            if (isInventory)
            {
                EmptyAllFields();
                if (view_dgv.Rows.Count != 0)
                {
                    LoadProductFromDGV(0);
                    BOM_Load(Convert.ToInt32(view_dgv.SelectedRows[0].Cells["dataGridViewTextBoxColumn1"].Value));
                }
                else
                {
                    RestrictProductCreation();
                }
            }
            else
            {
                if (pProductId != -1)
                {
                    InventoryList inventoryList = new InventoryList(machineUserContext);
                    decimal stock = inventoryList.GetProductStockQuantity(pProductId);
                    lnkStock.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, "Stock") + " (" + stock + ")";
                    txtCode.Enabled = false;
                    if (stock > 0)
                    {
                        cmbInventoryUOM.Enabled = false;
                    }
                }
                ResizeButtons();
                BuildProductDTO();
                BOM_Load(pProductId);
                int ourScreenWidth = Screen.FromControl(this).WorkingArea.Width;
                int ourScreenHeight = Screen.FromControl(this).WorkingArea.Height;
                float scaleFactorWidth = (float)ourScreenWidth / 1366f;
                float scaleFactorHeigth = (float)ourScreenHeight / 768f;
                SizeF scaleFactor = new SizeF(scaleFactorWidth, scaleFactorHeigth);
                Scale(scaleFactor);
            }
            log.LogMethodExit();
        }

        private void PopulateProducts(List<ProductDTO> prodDTOList)
        {
            log.LogMethodEntry(prodDTOList);
            ProductList productList = new ProductList();
            List<ProductDTO> productListOnDisplay;
            BindingSource productBS = new BindingSource();
            if (prodDTOList == null)
            {
                List<KeyValuePair<ProductDTO.SearchByProductParameters, string>> SearchParameter = new List<KeyValuePair<ProductDTO.SearchByProductParameters, string>>();
                SearchParameter.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                if (cmbActive.SelectedIndex == 1)
                {
                    SearchParameter.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.IS_ACTIVE, "Y"));
                }
                else if (cmbActive.SelectedIndex == 2)
                {
                    SearchParameter.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.IS_ACTIVE, "N"));
                }
                productListOnDisplay = productList.GetAllProducts(SearchParameter);

            }
            else
            {
                productListOnDisplay = prodDTOList;
            }

            SortableBindingList<ProductDTO> sortProductDTOList;
            if (productListOnDisplay == null)
            {
                sortProductDTOList = new SortableBindingList<ProductDTO>();
            }
            else
            {
                sortProductDTOList = new SortableBindingList<ProductDTO>(productListOnDisplay);
            }
            productBS = new BindingSource();
            productBS.DataSource = sortProductDTOList;
            view_dgv.DataSource = productBS;
            txtPit.BackColor = Color.White;
            log.LogMethodExit();
        }

        private void EmptyAllFields()
        {
            log.LogMethodEntry();
            try
            {
                notInitCase = false;
                txtCode.Text = string.Empty;
                txtProductName.Text = string.Empty;
                txtCost.Text = string.Empty;
                txtLowerCostLimit.Text = txtUpperCostLimit.Text = txtCostVariancePer.Text = string.Empty;
                txtSalePrice.Text = string.Empty;
                txtBarcode.Text = string.Empty;
                txtDescription.Text = string.Empty;
                txtLastpurchaseprice.Text = string.Empty;
                txtInnerPackQty.Text = "1";
                txtPit.Text = string.Empty;
                txtTurnInPIT.Text = string.Empty;
                txtRemarks.Text = string.Empty;
                txtReorderpoint.Text = string.Empty;
                txtReorderquantity.Text = string.Empty;
                lblProductid.Text = string.Empty;
                this.categoryDTOBindingSource.ResetBindings(false);
                ddlCategory.SelectedIndex = 0;
                string defaultStoreName = GetDefaultStoreName();
                int defaultInBoundLocationIndex = ddlDefaultlocation.FindStringExact(defaultStoreName);
                int defaultOutBoundLocationIndex = ddlOutboundLocation.FindStringExact(defaultStoreName);
                ddlDefaultlocation.SelectedIndex = (defaultInBoundLocationIndex > 0 ? defaultInBoundLocationIndex : 0);
                ddlPreferredvendor.SelectedIndex = 0;
                ddlOutboundLocation.SelectedIndex = (defaultOutBoundLocationIndex > 0 ? defaultOutBoundLocationIndex : 0);
                ddlExpiryType.Text = ddlIssueApproach.Text = "None";

                txtIndays.Text = string.Empty;
                txtInnerPackQty.Text = string.Empty;
                ddlTax.SelectedIndex = 0;
                ddlUOM.SelectedIndex = 0;
                cbxActive.Checked = true;
                cbxIsSellable.Checked = false;
                cbxIsRedeemable.Checked = false;
                ddlExpiryType.Text = string.Empty;
                ddlIssueApproach.Text = string.Empty;
                cbxPurchaseable.Checked = true;
                cbxTaxInclusiveCost.Checked = true;
                pbProductImage.Image = null;
                lblImageFileName.Text = string.Empty;
                pbProductImage.Tag = DBNull.Value;
                lblProductid.Tag = null;
                tvBOM.Nodes.Clear();
                cbxLotControlled.Enabled = true;
                cbxLotControlled.Checked = false;
                btnSKUSegments.Tag = -1;
                AdvancedSearch = null;
                cbxAutoUpdateMarkup.Enabled = false;
                txtItemMarkupPercent.Text = string.Empty;
                cbxAutoUpdateMarkup.Checked = false;
                txtItemMarkupPercent.Enabled = false;
                notInitCase = true;
                cmbInventoryUOM.SelectedIndex = -1;
                txtYieldPer.Text = string.Empty;
                txtPreparationTime.Text = string.Empty;
                cbxIncludeInPlan.Checked = false;
                ClearDisplayGroupText();
            }
            catch (Exception ex)
            {
                notInitCase = true;
                log.Error(ex);
            }
            log.LogMethodExit();
        }


        private void btnSave_Click(object sender, EventArgs e)
        {

            log.LogMethodEntry();
            try
            {
                bool return_val = true;
                if (return_val)
                {
                    String currentCode = txtCode.Text;
                    String mode = "";
                    int productIdModified = -1;
                    if (lblProductid.Text != "")
                    {
                        mode = "U";
                    }
                    else
                    {
                        mode = "I";
                    }
                    try
                    {
                        if (!ProductSave())//saveProduct()
                        {
                            ResetCustomerDataSet(mode);
                            return;
                        }
                    }
                    catch (ValidationException ex)
                    {
                        log.Error(ex);
                        ResetCustomerDataSet(mode);
                        MessageBox.Show(ex.GetAllValidationErrorMessages(), MessageContainerList.GetMessage(utilities.ExecutionContext, "Database Save"));
                        return;
                    }
                    catch (SqlException ex)
                    {
                        ResetCustomerDataSet(mode);
                        if (ex.Number == 2627 || ex.Number == 2601)
                        {
                            MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 290), MessageContainerList.GetMessage(utilities.ExecutionContext, "Database Save"));
                            return;
                        }
                        MessageBox.Show(ex.Data.ToString() + ex.Message, MessageContainerList.GetMessage(utilities.ExecutionContext, "Database Save"));
                        return;
                    }
                    catch (Exception ex)
                    {
                        ResetCustomerDataSet(mode);
                        log.Error(ex);
                        MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 1824, ex.Message));
                        return;
                    }
                    productIdModified = productDTO.ProductId;
                    if (mode == "I")
                    {
                        MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 892), MessageContainerList.GetMessage(utilities.ExecutionContext, "Save Product"));
                        SetDisplayGroupText();
                    }
                    int selectedIndexVal = 0;
                    if (isInventory)
                    {
                        EmptyAllFields();
                        PopulateProducts(null);
                        try
                        {
                            DataGridViewRow row = view_dgv.Rows.Cast<DataGridViewRow>()
                                             .Where(r => r.Cells["dataGridViewTextBoxColumn1"].Value.ToString().Equals(productIdModified.ToString()))
                                              .First();
                            selectedIndexVal = row.Index;
                        }
                        catch { selectedIndexVal = 0; }

                        if (selectedIndexVal < 0)
                            selectedIndexVal = 0;

                        if (selectedIndexVal > 0)
                        {
                            view_dgv.Rows[selectedIndexVal].Selected = true;
                            LoadProductFromDGV(selectedIndexVal);

                        }
                        txtCode.Enabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 1824, ex.Message));
            }
            log.LogMethodExit();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                tsbClear.PerformClick();
                btnSave.Enabled = true;
                productDTO = new ProductDTO();
                productsDTO = new ProductsDTO();
                EmptyAllFields();
                txtCode.Enabled = true;
                lnkStock.Text = "";
                txtCode.ReadOnly = false;
                txtDescription.ReadOnly = false;
                ddlCategory.Enabled = true;
                txtSalePrice.ReadOnly = false;
                manualProductId = -1;
                cbxMarketListItem.Checked = false;
                txtCode.Focus();
                ddlOutboundLocation.Enabled = true;
                PopulateItemType();
                cmbInventoryUOM.Enabled = true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 1824, ex.Message));
            }
            log.LogMethodExit();
        }

        private bool ProductSave()
        {
            log.LogMethodEntry();
            if (string.IsNullOrEmpty(txtCode.Text))
            {
                MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 893), MessageContainerList.GetMessage(utilities.ExecutionContext, "Insert / Update Product"));
                txtCode.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtProductName.Text))
            {
                txtProductName.Text = txtCode.Text;
            }

            if (string.IsNullOrEmpty(txtDescription.Text))
            {
                MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 1361), MessageContainerList.GetMessage(utilities.ExecutionContext, "Insert / Update Product"));
                txtDescription.Focus();
                log.LogMethodExit(false);
                return false;
            }

            //Added to check if category is selected while saving product
            if (ddlCategory.SelectedValue == null || (int)ddlCategory.SelectedValue == -1)
            {
                MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 1829, lblCategory.Tag), MessageContainerList.GetMessage(utilities.ExecutionContext, "Insert / Update Product"));
                ddlCategory.Focus();
                log.LogMethodExit(false);
                return false;
            }

            if (ddlUOM.SelectedValue == null || (int)ddlUOM.SelectedValue == -1)
            {
                MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 888), MessageContainerList.GetMessage(utilities.ExecutionContext, "Insert / Update Product"));
                ddlUOM.Focus();
                log.LogMethodExit(false);
                return false;
            }

            if (ddlDefaultlocation.SelectedValue == null || (int)ddlDefaultlocation.SelectedValue == -1)
            {
                MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 1829, lblInboundLocation.Tag), MessageContainerList.GetMessage(utilities.ExecutionContext, "Insert / Update Product"));
                ddlDefaultlocation.Focus();
                log.LogMethodExit(false);
                return false;
            }
            if (ddlOutboundLocation.SelectedValue == null || (int)ddlOutboundLocation.SelectedValue == -1)
            {
                MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 1829, lblOutboundLocation.Tag), MessageContainerList.GetMessage(utilities.ExecutionContext, "Insert / Update Product"));
                ddlOutboundLocation.Focus();
                log.LogMethodExit(false);
                return false;
            }

            if (!string.IsNullOrEmpty(txtYieldPer.Text))
            {
                try
                {
                    decimal yieldPerc = Convert.ToDecimal(txtYieldPer.Text);
                    if (yieldPerc < 0)
                    {
                        MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 2857));
                        //Please enter valid yield percentage value.
                        log.LogMethodExit(false);
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 2857));
                    //Please enter valid yield percentage value.
                    log.Error(ex);
                    log.LogMethodExit(false);
                    return false;
                }
            }
            using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
            {
                parafaitDBTrx.BeginTransaction();
                if (!isInventory)
                {
                    productDTO.ManualProductId = -1;//pProductId;
                }
                productDTO.Code = txtCode.Text;
                productDTO.ProductName = txtProductName.Text;
                productDTO.UomId = Convert.ToInt32(ddlUOM.SelectedValue);
                productDTO.PurchaseTaxId = ddlTax.SelectedValue == null ? -1 : Convert.ToInt32(ddlTax.SelectedValue);
                productDTO.DefaultLocationId = Convert.ToInt32(ddlDefaultlocation.SelectedValue);
                productDTO.OutboundLocationId = Convert.ToInt32(ddlOutboundLocation.SelectedValue);
                productDTO.DefaultVendorId = Convert.ToInt32(ddlPreferredvendor.SelectedValue);
                if (cbxActive.Checked)
                    productDTO.IsActive = true;
                else
                    productDTO.IsActive = false;
                if (cbxIsRedeemable.Checked)
                    productDTO.IsRedeemable = "Y";
                else
                    productDTO.IsRedeemable = "N";

                if (cbxIsSellable.Checked)
                    productDTO.IsSellable = "Y";
                else
                    productDTO.IsSellable = "N";

                if (cbxPurchaseable.Checked)
                    productDTO.IsPurchaseable = "Y";
                else
                    productDTO.IsPurchaseable = "N";

                if (!string.IsNullOrEmpty(txtReorderpoint.Text))
                    productDTO.ReorderPoint = Convert.ToDouble(txtReorderpoint.Text);
                if (!string.IsNullOrEmpty(txtReorderquantity.Text))
                    productDTO.ReorderQuantity = Convert.ToDouble(txtReorderquantity.Text);
                if (!string.IsNullOrEmpty(txtCost.Text))
                    productDTO.Cost = Convert.ToDouble(txtCost.Text);
                if (!string.IsNullOrEmpty(txtSalePrice.Text))
                    productDTO.SalePrice = Convert.ToDouble(txtSalePrice.Text);
                if (!string.IsNullOrEmpty(txtInnerPackQty.Text))
                    productDTO.InnerPackQty = Convert.ToDouble(txtInnerPackQty.Text);
                productDTO.SegmentCategoryId = Convert.ToInt32(btnSKUSegments.Tag.ToString());
                productDTO.CustomDataSetId = Convert.ToInt32(btnCustom.Tag.ToString());
                if (cbxLotControlled.Checked)
                    productDTO.LotControlled = true;
                else
                    productDTO.LotControlled = false;
                if (cbxCostHasTax.Checked)
                    productDTO.CostIncludesTax = true;
                else
                    productDTO.CostIncludesTax = false;
                if (cbxMarketListItem.Checked)
                    productDTO.MarketListItem = true;
                else
                    productDTO.MarketListItem = false;
                productDTO.ExpiryDays = 0;
                if (ddlExpiryType.Text == "None")
                    productDTO.ExpiryType = "N";
                else if (ddlExpiryType.Text == "In Days")
                {
                    productDTO.ExpiryType = "D";
                    productDTO.LotControlled = true;
                    if (string.IsNullOrEmpty(txtIndays.Text))
                    {
                        MessageBox.Show("Please enter expiry days.");
                    }
                    else
                    {
                        productDTO.ExpiryDays = Convert.ToInt32(txtIndays.Text);
                    }
                }
                else if (ddlExpiryType.Text == "Expiry Date")
                {
                    productDTO.ExpiryType = "E";
                    productDTO.LotControlled = true;
                }

                if (cbxLotControlled.Checked == true && ddlIssueApproach.Text == "None")
                {
                    if (productDTO.ExpiryType == "E" || productDTO.ExpiryType == "D")
                        productDTO.IssuingApproach = "FEFO";
                    else
                        productDTO.IssuingApproach = "FIFO";
                }
                else
                    productDTO.IssuingApproach = ddlIssueApproach.Text;
                productDTO.Description = txtDescription.Text;
                productDTO.Remarks = txtRemarks.Text;
                productDTO.CategoryId = (int)ddlCategory.SelectedValue;
                if (!string.IsNullOrEmpty(txtLowerCostLimit.Text))
                {
                    productDTO.LowerLimitCost = Convert.ToDouble(txtLowerCostLimit.Text);
                }

                if (!string.IsNullOrEmpty(txtUpperCostLimit.Text))
                {
                    productDTO.UpperLimitCost = Convert.ToDouble(txtUpperCostLimit.Text);
                }
                if (!string.IsNullOrEmpty(txtCostVariancePer.Text))
                {
                    productDTO.CostVariancePercentage = Convert.ToDouble(txtCostVariancePer.Text);
                }

                if (!string.IsNullOrEmpty(txtLastpurchaseprice.Text))
                {
                    productDTO.LastPurchasePrice = Convert.ToDouble(txtLastpurchaseprice.Text);
                }
                else
                    productDTO.LastPurchasePrice = 0;

                if (!string.IsNullOrEmpty(txtPit.Text))
                {
                    productDTO.PriceInTickets = Convert.ToDouble(txtPit.Text);
                }
                if (!string.IsNullOrEmpty(txtTurnInPIT.Text))
                {
                    productDTO.TurnInPriceInTickets = Convert.ToInt32(txtTurnInPIT.Text);
                }
                if (cbxAutoUpdateMarkup.Checked)
                    productDTO.AutoUpdateMarkup = true;
                else
                    productDTO.AutoUpdateMarkup = false;
                if (!string.IsNullOrEmpty(txtItemMarkupPercent.Text))
                {
                    productDTO.ItemMarkupPercent = Convert.ToDouble(txtItemMarkupPercent.Text);
                }
                else
                    productDTO.ItemMarkupPercent = double.NaN;

                productDTO.TaxInclusiveCost = (cbxTaxInclusiveCost.Checked ? "Y" : "N");
                productDTO.ImageFileName = (pbProductImage.Tag == null) ? "" : pbProductImage.Tag.ToString();

                if (cbxIncludeInPlan.Checked)
                    productDTO.IncludeInPlan = true;
                else
                    productDTO.IncludeInPlan = false;
                if (cmbInventoryUOM.SelectedValue == null)
                {
                    productDTO.InventoryUOMId = Convert.ToInt32(ddlUOM.SelectedValue);
                }
                else
                {
                    productDTO.InventoryUOMId = Convert.ToInt32(cmbInventoryUOM.SelectedValue);
                }
                productDTO.ItemType = Convert.ToInt32(cmbItemType.SelectedValue);
                if (!string.IsNullOrEmpty(txtYieldPer.Text))
                {
                    productDTO.YieldPercentage = Convert.ToDecimal(txtYieldPer.Text);
                }
                if (!string.IsNullOrEmpty(txtPreparationTime.Text))
                {
                    productDTO.PreparationTime = Convert.ToInt32(txtPreparationTime.Text);
                }
                productsDTO.InventoryItemDTO = productDTO;

                UpdatePOSProductInformation();

                Products products = new Products(machineUserContext, productsDTO);
                products.Save(parafaitDBTrx.SQLTrx);

                if (products.GetProductsDTO.ProductId < 0)
                {
                    parafaitDBTrx.RollBack();
                    log.LogMethodExit(false);
                    return false;
                }
                else
                {
                    lblProductid.Text = productDTO.ProductId.ToString();
                }
                try
                {
                    CopyFromDuplicateFromManualProductId(parafaitDBTrx.SQLTrx);
                    if (cbxLotControlled.Checked)
                    {
                        if (productDTO.LotControlled && productDTO.IssuingApproach == "FIFO")
                        {
                            InventoryLotBL inventoryLot = new InventoryLotBL(machineUserContext);
                            inventoryLot.UpdateNonLotableToLotable(productDTO.ProductId, parafaitDBTrx.SQLTrx);
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    parafaitDBTrx.RollBack();
                    log.LogMethodExit(false);
                    return false;
                }
                parafaitDBTrx.EndTransaction();
                if (duplicateFromManualProductId > -1)
                {
                    duplicateFromManualProductId = -1;
                }
            }
            log.LogMethodExit(true);
            return true;
        }


        private void txtCost_Validating(object sender, CancelEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                double val;
                if (txtCost.Text != "")
                    val = Convert.ToDouble(txtCost.Text);
                txtCost.BackColor = Color.White;
                if (!CalculateMarkup())
                    e.Cancel = true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                txtCost.BackColor = Color.Yellow;
                MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 891), MessageContainerList.GetMessage(utilities.ExecutionContext, "Cost Validation"));
                e.Cancel = true;
            }
            log.LogMethodExit();
        }

        private void TxtItemMarkupPercent_Validating(object sender, CancelEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                double val;
                if (txtItemMarkupPercent.Text != "")
                    val = Convert.ToDouble(txtCost.Text);
                txtItemMarkupPercent.BackColor = Color.White;
                if (!CalculateMarkup())
                    e.Cancel = true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                txtItemMarkupPercent.BackColor = Color.Yellow;
                MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 891), MessageContainerList.GetMessage(utilities.ExecutionContext, "Cost Validation"));
                e.Cancel = true;
            }
            log.LogMethodExit();
        }

        private void TxtCode_Leave(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (txtProductName.Text.Trim() == "")
            {
                txtProductName.Text = txtCode.Text.Trim();
            }
            txtProductName.Focus();
            log.LogMethodExit();
        }


        private void CbxIsRedeemable_CheckedChanged(object sender, System.EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (notInitCase)
                {
                    if (!cbxIsRedeemable.Checked)
                    {
                        txtItemMarkupPercent.Enabled = false;
                        if (cbxAutoUpdateMarkup.Checked)
                            cbxAutoUpdateMarkup.Checked = false;
                        cbxAutoUpdateMarkup.Enabled = false;
                    }
                    else
                    {
                        txtItemMarkupPercent.Enabled = true;
                        cbxAutoUpdateMarkup.Enabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void CbxAutoUpdateMarkup_CheckedChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (notInitCase)
                {
                    if (!CalculateMarkup())
                    {
                        notInitCase = false;
                        cbxAutoUpdateMarkup.Checked = false;
                        notInitCase = true;
                        cbxAutoUpdateMarkup.Focus();
                    }
                    else
                        UISettingsForAutoMarkUp();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private bool CalculateMarkup()
        {
            log.LogMethodEntry();
            bool returnVal = true;
            if (cbxAutoUpdateMarkup.Checked && notInitCase && cbxIsRedeemable.Checked)
            {
                try
                {
                    double productCost = (txtCost.Text.Trim() != "" ? Convert.ToDouble(txtCost.Text) : 0);
                    double itemMarkUpPercent = (txtItemMarkupPercent.Text.Trim() != "" ? Convert.ToDouble(txtItemMarkupPercent.Text) : double.NaN);
                    int vendorId = Convert.ToInt32(ddlPreferredvendor.SelectedValue);
                    ProductList productBL = new ProductList();
                    try
                    {
                        double newPITValue = productBL.calculatePITByMarkUp(productCost, itemMarkUpPercent, vendorId);
                        string pitValueStr = txtPit.Text.Trim();
                        if (pitValueStr == "")
                        {
                            txtPit.Text = Convert.ToString(newPITValue);
                            txtPit.BackColor = Color.Orange;
                        }
                        else if (Convert.ToDouble(pitValueStr) != newPITValue)
                        {
                            txtPit.Text = Convert.ToString(newPITValue);
                            txtPit.BackColor = Color.Orange;
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        MessageBox.Show(ex.Message, MessageContainerList.GetMessage(utilities.ExecutionContext, "Price in Tickets"));
                        returnVal = false;
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 1228), MessageContainerList.GetMessage(utilities.ExecutionContext, "Price in Tickets"));
                    returnVal = false;
                }
            }
            log.LogMethodExit(returnVal);
            return returnVal;

        }

        private void UISettingsForAutoMarkUp()
        {
            log.LogMethodEntry();
            if (!cbxIsRedeemable.Checked)
            {
                txtItemMarkupPercent.Enabled = false;
                cbxAutoUpdateMarkup.Enabled = false;
            }
            else
            {
                txtItemMarkupPercent.Enabled = true;
                cbxAutoUpdateMarkup.Enabled = true;
            }

            if (cbxAutoUpdateMarkup.Checked && cbxIsRedeemable.Checked)
                txtPit.Enabled = false;
            else
                txtPit.Enabled = true;
            log.LogMethodExit();
        }



        private void frmProduct_Activated(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            BarcodeReader.setReceiveAction = serialbarcodeDataReceived;
            BarcodetoolStripTextBox.Focus();
            log.LogMethodExit();
        }

        //For Bar code
        private void serialbarcodeDataReceived()
        {
            log.LogMethodEntry();
            scannedBarcode = BarcodeReader.Barcode;

            if (scannedBarcode != "")
            {
                //If BarcodetoolStripTextBox text box is focused product should be populated by bar code entered
                if (BarcodetoolStripTextBox.Focused)
                {
                    BarcodetoolStripTextBox.Text = scannedBarcode;
                    fillByToolStripButton.PerformClick();
                }
            }
            log.LogMethodExit();
        }

        private void LoadProductFromDGV(int index)
        {
            log.LogMethodEntry(index);
            try
            {
                BindingSource productbs = (BindingSource)view_dgv.DataSource;
                var productDTOListOnDisplay = (SortableBindingList<ProductDTO>)productbs.DataSource;
                manualProductId = productDTOListOnDisplay[index].ManualProductId;
                if (manualProductId == -1)
                {
                    ProductDTO itemproductDTO = productDTOListOnDisplay[index];
                    CreateManualProduct(itemproductDTO);
                    manualProductId = itemproductDTO.ManualProductId;
                }
                PopulateProductDTO(manualProductId);

                if (productDTO != null)
                {
                    try
                    {
                        notInitCase = false;
                        lblProductid.Text = productDTO.ProductId.ToString();
                        cmbInventoryUOM.Enabled = true;
                        txtCode.Text = productDTO.Code;
                        txtProductName.Text = productDTO.ProductName;
                        txtCode.ReadOnly = true;
                        ddlUOM.SelectedValue = productDTO.UomId;
                        ddlTax.SelectedValue = productDTO.PurchaseTaxId;
                        ddlDefaultlocation.SelectedValue = productDTO.DefaultLocationId;
                        ddlOutboundLocation.SelectedValue = productDTO.OutboundLocationId;
                        ddlPreferredvendor.SelectedValue = productDTO.DefaultVendorId;
                        cbxActive.Checked = productDTO.IsActive;
                        cbxIsRedeemable.Checked = productDTO.IsRedeemable.Equals("Y");
                        cbxIsSellable.Checked = productDTO.IsSellable.Equals("Y");
                        cbxLotControlled.Checked = productDTO.LotControlled;
                        cbxLotControlled.Enabled = !productDTO.LotControlled;
                        ddlExpiryType.Enabled = productDTO.LotControlled;
                        ddlIssueApproach.Enabled = productDTO.LotControlled;
                        cbxMarketListItem.Checked = productDTO.MarketListItem;
                        cbxPurchaseable.Checked = productDTO.IsPurchaseable.Equals("Y");
                        cbxTaxInclusiveCost.Checked = productDTO.TaxInclusiveCost != null ? ((productDTO.TaxInclusiveCost.Equals("Y")) ? true : false) : false;
                        txtReorderpoint.Text = productDTO.ReorderPoint.ToString();
                        txtReorderquantity.Text = productDTO.ReorderQuantity.ToString();
                        txtCost.Text = productDTO.Cost.ToString();
                        txtSalePrice.Text = productDTO.SalePrice.ToString(utilities.ParafaitEnv.AMOUNT_FORMAT);
                        txtSalePrice.ReadOnly = true;
                        txtInnerPackQty.Text = productDTO.InnerPackQty.ToString();
                        btnSKUSegments.Tag = productDTO.SegmentCategoryId;
                        btnCustom.Tag = productDTO.CustomDataSetId;
                        if (productDTO.ExpiryType.Equals("N"))
                            ddlExpiryType.Text = "None";
                        else if (productDTO.ExpiryType.Equals("D"))
                        {
                            ddlExpiryType.Text = "In Days";
                            txtIndays.Text = productDTO.ExpiryDays.ToString();
                            txtIndays.Visible = true;
                        }
                        else if (productDTO.ExpiryType.Equals("E"))
                            ddlExpiryType.Text = "Expiry Date";

                        ddlIssueApproach.Text = productDTO.IssuingApproach;
                        txtDescription.Text = productDTO.Description;
                        txtRemarks.Text = productDTO.Remarks;
                        ddlCategory.SelectedValue = productDTO.CategoryId;
                        ddlCategory.Enabled = true;
                        txtLowerCostLimit.Text = productDTO.LowerLimitCost.ToString();
                        txtUpperCostLimit.Text = productDTO.UpperLimitCost.ToString();
                        txtCostVariancePer.Text = productDTO.CostVariancePercentage.ToString();
                        txtLastpurchaseprice.Text = productDTO.LastPurchasePrice.ToString();
                        txtPit.Text = productDTO.PriceInTickets.ToString();
                        txtPit.BackColor = Color.White;
                        txtTurnInPIT.Text = productDTO.TurnInPriceInTickets.ToString();
                        txtBarcode.Text = productDTO.BarCode;
                        cbxCostHasTax.Checked = productDTO.CostIncludesTax.Equals(true);
                        txtItemMarkupPercent.Text = (!Double.IsNaN(productDTO.ItemMarkupPercent) ? productDTO.ItemMarkupPercent.ToString() : "");
                        cbxAutoUpdateMarkup.Checked = productDTO.AutoUpdateMarkup;
                        pbProductImage.Tag = productDTO.ImageFileName;
                        lblImageFileName.Text = pbProductImage.Tag != null ? pbProductImage.Tag.ToString() : "";
                        if (pbProductImage.Tag != null && pbProductImage.Tag != DBNull.Value && !string.IsNullOrEmpty(pbProductImage.Tag.ToString()))
                        {
                            SqlCommand cmdImage = utilities.getCommand();
                            cmdImage.CommandText = "exec ReadBinaryDataFromFile @FileName";

                            string fileName = pbProductImage.Tag.ToString();
                            lblImageFileName.Text = fileName;
                            cmdImage.Parameters.AddWithValue("@FileName", imageFolder + "\\" + fileName);
                            try
                            {
                                object o = cmdImage.ExecuteScalar();
                                pbProductImage.Image = utilities.ConvertToImage(o);
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex);
                                pbProductImage.Image = null;
                            }
                        }
                        else
                            pbProductImage.Image = null;

                        notInitCase = true;
                        UISettingsForAutoMarkUp();

                        if (productDTO.ProductId > -1)
                        {
                            InventoryList inventoryList = new InventoryList(machineUserContext);
                            decimal stock = inventoryList.GetProductStockQuantity(productDTO.ProductId);
                            lnkStock.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, "Stock") + " (" + stock + ")";
                            if (stock > 0)
                            {
                                cmbInventoryUOM.Enabled = false;
                            }
                        }
                        SetDisplayGroupText();
                        int uomId = Convert.ToInt32(ddlUOM.SelectedValue);
                        GetUOMComboxValue(uomId);
                        if (productDTO.InventoryUOMId == -1)
                        {
                            cmbInventoryUOM.SelectedValue = productDTO.UomId;
                        }
                        else
                        {
                            cmbInventoryUOM.SelectedValue = productDTO.InventoryUOMId;
                        }
                        if (productDTO.ItemType != -1)
                        {
                            cmbItemType.SelectedValue = productDTO.ItemType;
                        }
                        else
                        {
                            PopulateItemType();
                        }
                        if (cmbInventoryUOM.SelectedValue == null)
                        {
                            GetUOMComboxValue(Convert.ToInt32(ddlUOM.SelectedValue));
                        }
                        cbxIncludeInPlan.Checked = productDTO.IncludeInPlan != null ? ((productDTO.IncludeInPlan.Equals(true)) ? true : false) : false;
                        txtYieldPer.Text = productDTO.YieldPercentage.ToString();
                        txtPreparationTime.Text = productDTO.PreparationTime.ToString();
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        notInitCase = true;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }


        private void btnAddcategory_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                this.Cursor = Cursors.WaitCursor;
                CategoryUI = new CategoryUI(utilities);
                CommonUIDisplay.setupVisuals(CategoryUI);//Added for GUI Design style on 23-Aug-2016
                CategoryUI.StartPosition = FormStartPosition.CenterScreen;//Added for showing in center on 23-Aug-2016
                CategoryUI.ShowDialog();
                PopulateCategory();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 1824, ex.Message));
            }
            finally { this.Cursor = Cursors.Default; }
            log.LogMethodExit();
        }

        private void btnAddUOM_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                this.Cursor = Cursors.WaitCursor; UOMUI = new UOMUI(utilities);
                CommonUIDisplay.setupVisuals(UOMUI);//Added for GUI Design style on 23-Aug-2016
                UOMUI.StartPosition = FormStartPosition.CenterScreen;//Added for showing in center on 23-Aug-2016
                UOMUI.ShowDialog();
                PopulateUOM();
                ddlUOM.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 1824, ex.Message));
            }
            finally { this.Cursor = Cursors.Default; }
            log.LogMethodExit();
        }

        private void btnGenerateBarCode_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (lblProductid.Text != "")
                {
                    //Updated frm_addBarcode constructor call to see that product description is passed 19-Apr-2016
                    //Updated frm_addBarcode constructor call to see that product price is passed 03-Feb-2017
                    frmAddBarcode f = new frmAddBarcode(Convert.ToInt32(lblProductid.Text), txtCode.Text, txtDescription.Text, txtCost.Text == "" ? -1 : Convert.ToDouble(txtCost.Text), utilities);
                    CommonUIDisplay.setupVisuals(f);//Added for GUI Design style on 23-Aug-2016
                    f.StartPosition = FormStartPosition.CenterScreen;//Added for showing in center on 23-Aug-2016
                    f.ShowDialog();
                    txtBarcode.Text = f.productBarcodeDTO != null && f.productBarcodeDTO.BarCode != null ?
                                                        f.productBarcodeDTO.BarCode : null;
                    //btnRefresh.PerformClick();
                }
                else
                {
                    //02-Jun-2016 Updated to add Message No.
                    MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 1018), MessageContainerList.GetMessage(utilities.ExecutionContext, "Add Bar code"));
                }
                BarcodeReader.setReceiveAction = serialbarcodeDataReceived;//Added line to fix Bar code scan related issue 31-Mar-2016
                //BarcodetoolStripTextBox.Focus();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 1824, ex.Message));
            }
            finally { this.Cursor = Cursors.Default; }
            log.LogMethodExit();
        }

        private void btnAddlocation_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                this.Cursor = Cursors.WaitCursor;
                LocationUI = new LocationUI(utilities);
                CommonUIDisplay.setupVisuals(LocationUI);//Added for GUI Design style on 23-Aug-2016
                LocationUI.StartPosition = FormStartPosition.CenterScreen;//Added for showing in center on 23-Aug-2016
                LocationUI.ShowDialog();
                PopulateLocation();
                ddlDefaultlocation.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 1824, ex.Message));
            }
            finally { this.Cursor = Cursors.Default; }
            log.LogMethodExit();
        }

        private void btnAddvendor_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                this.Cursor = Cursors.WaitCursor;
                frmVendor f = new frmVendor(utilities);
                CommonUIDisplay.setupVisuals(f);//Added for GUI Design style on 23-Aug-2016
                f.StartPosition = FormStartPosition.CenterScreen;//Added for showing in center on 23-Aug-2016
                f.ShowDialog();
                PopulateVendor();
                ddlPreferredvendor.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 1824, ex.Message));
            }
            finally { this.Cursor = Cursors.Default; }
            log.LogMethodExit();
        }

        private void btnUPCBarcode_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();

            try
            {
                if (lblProductid.Text == "")
                {
                    //02-Jun-2016 Updated code to add Message No.
                    MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 1009), MessageContainerList.GetMessage(utilities.ExecutionContext, "Generate UPC Bar code"));
                }
                else
                {
                    string productCode = "";
                    long preferredvendor = -1;
                    string vendorCode = "";
                    string typeChar = "";
                    int checkBit = 0;
                    string UPCCode = "";

                    try
                    {
                        //Get the 1st digit the type digit 
                        typeChar = utilities.getParafaitDefaults("UPC_TYPE");
                        if (string.IsNullOrEmpty(typeChar))
                        {
                            //02-Jun-2016 Updated code to add Message No.
                            MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 1011), MessageContainerList.GetMessage(utilities.ExecutionContext, "Generate UPC Bar code"));
                            log.LogMethodExit();
                            return;
                        }
                        //Check if type character has all numeric characters and length is greater than 1
                        else if (typeChar.Length > 1 || !typeChar.All(char.IsDigit))
                        {
                            //02-Jun-2016 Updated code to add Message No.
                            MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 1012), MessageContainerList.GetMessage(utilities.ExecutionContext, "Generate UPC Bar code"));
                            log.LogMethodExit();
                            return;
                        }

                        //Check if Product code has all numeric characters
                        if (!txtCode.Text.All(char.IsDigit) || txtCode.Text.Length > 5)
                        {
                            //02-Jun-2016 Updated code to add Message No.
                            MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 1013), MessageContainerList.GetMessage(utilities.ExecutionContext, "Generate UPC Bar code"));
                            log.LogMethodExit();
                            return;
                        }

                        //Left pad product code with 0
                        productCode = txtCode.Text.PadLeft(5, '0');

                        if (ddlPreferredvendor.Text.Trim() != "")
                        {
                            preferredvendor = Convert.ToInt32(ddlPreferredvendor.SelectedValue);
                            //Get vendorcode
                            VendorList vendorList = new VendorList(machineUserContext);
                            List<KeyValuePair<VendorDTO.SearchByVendorParameters, string>> SearchVendorListParameter = new List<KeyValuePair<VendorDTO.SearchByVendorParameters, string>>();
                            SearchVendorListParameter.Add(new KeyValuePair<VendorDTO.SearchByVendorParameters, string>(VendorDTO.SearchByVendorParameters.VENDOR_ID, preferredvendor.ToString()));
                            List<VendorDTO> vendorListOnDisplay = vendorList.GetAllVendors(SearchVendorListParameter);

                            if (vendorListOnDisplay != null && vendorListOnDisplay.Count > 0)
                            {
                                if (vendorListOnDisplay[0].VendorCode != null)
                                {
                                    //Check if vendor code is numeric and has length greater than 5
                                    if (!vendorListOnDisplay[0].VendorCode.ToString().All(char.IsDigit) || vendorListOnDisplay[0].VendorCode.ToString().Length > 5)
                                    {
                                        //02-Jun-2016 Updated to add Messsage No.
                                        MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 1014), MessageContainerList.GetMessage(utilities.ExecutionContext, "Generate UPC Bar code"));
                                        log.LogMethodExit();
                                        return;
                                    }
                                    else
                                    {
                                        //Left pad vendor code with 0
                                        vendorCode = vendorListOnDisplay[0].VendorCode.ToString().PadLeft(5, '0');
                                    }
                                }
                                else
                                {
                                    MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 1014), MessageContainerList.GetMessage(utilities.ExecutionContext, "Generate UPC Bar code"));
                                    log.LogMethodExit();
                                    return;
                                }

                            }
                            else
                            {
                                //02-Jun-2016 Updated to add Messsage No.
                                MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 1014), MessageContainerList.GetMessage(utilities.ExecutionContext, "Generate UPC Bar code"));
                                log.LogMethodExit();
                                return;
                            }
                        }
                        else
                        {
                            //02-Jun-2016 Updated to add Messsage No.
                            MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 1015), MessageContainerList.GetMessage(utilities.ExecutionContext, "Generate UPC Bar code"));
                            log.LogMethodExit();
                            return;
                        }

                        //Get check bit (Last digit in bar code)
                        checkBit = getCheckBit(typeChar, vendorCode, productCode);
                        UPCCode = typeChar + vendorCode + productCode + checkBit;

                        //Check for duplicate bar code
                        //cmd.CommandText = "select ID from productbarcode where BarCode = @BarCode and isactive = 'Y' " + condition; ;
                        //cmd.Parameters.AddWithValue("@BarCode", UPCCode);


                        //Check for duplicate bar code
                        ProductBarcodeListBL productBarcodeListBL = new ProductBarcodeListBL(machineUserContext);
                        List<KeyValuePair<ProductBarcodeDTO.SearchByParameters, string>> searchParameters;
                        searchParameters = new List<KeyValuePair<ProductBarcodeDTO.SearchByParameters, string>>();
                        searchParameters.Add(new KeyValuePair<ProductBarcodeDTO.SearchByParameters, string>(ProductBarcodeDTO.SearchByParameters.BARCODE, UPCCode));
                        searchParameters.Add(new KeyValuePair<ProductBarcodeDTO.SearchByParameters, string>(ProductBarcodeDTO.SearchByParameters.IS_ACTIVE, "Y"));
                        searchParameters.Add(new KeyValuePair<ProductBarcodeDTO.SearchByParameters, string>(ProductBarcodeDTO.SearchByParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                        List<ProductBarcodeDTO> productBarcodeListDTO = productBarcodeListBL.GetProductBarcodeDTOList(searchParameters);

                        if (productBarcodeListDTO != null)
                        {
                            MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 904), MessageContainerList.GetMessage(utilities.ExecutionContext, "Generate UPC Bar code"));
                            log.LogMethodExit();
                            return;
                        }



                        //Insert bar code into database
                        ProductBarcodeDTO productBarcodeDTO = new ProductBarcodeDTO();
                        productBarcodeDTO.BarCode = UPCCode;
                        productBarcodeDTO.Product_Id = productDTO.ProductId;
                        productBarcodeDTO.IsActive = true;
                        //productDTO.BarCode = UPCCode;
                        //productDTO.ProductId =Convert.ToInt32(lblProductid.Text);
                        //productDTO.IsActive = "Y";
                        ProductBarcodeBL productBarcodeBL = new ProductBarcodeBL(machineUserContext, productBarcodeDTO);
                        productBarcodeBL.Save();
                        txtBarcode.Text = UPCCode;

                        //02-Jun-2016 Updated code to add Message No.
                        MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 1016), MessageContainerList.GetMessage(utilities.ExecutionContext, "Add Barcode"));
                        //PopulateProducts(null);
                        //refreshDgv();//02-Jun-2016 Changed place where the function is called 
                        //The screen would get refreshed when the button would be clicked without saving product
                    }
                    catch
                    {
                        log.LogMethodExit();
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 1824, ex.Message));
            }
            log.LogMethodExit();
        }

        private int getCheckBit(string typeChar, string vendorCode, string productCode)
        {
            log.LogMethodEntry(typeChar, vendorCode, productCode);
            int CheckBit = 0;
            int sumOddDigits = 0;
            int sumEvenDigits = 0;

            string UPCCode = typeChar + vendorCode + productCode;
            for (int i = 0; i < UPCCode.Length; i++)
            {
                if ((i + 1) % 2 == 0)
                {
                    sumEvenDigits += Convert.ToInt32(UPCCode[i].ToString());
                }
                else
                {
                    sumOddDigits += Convert.ToInt32(UPCCode[i].ToString());
                }
            }
            int digitSum = (sumOddDigits * 3) + sumEvenDigits;
            if (digitSum % 10 != 0)
                CheckBit = 10 - (digitSum % 10);
            else
                CheckBit = digitSum % 10;
            log.LogMethodExit(CheckBit);
            return CheckBit;
        }//End update 03-May-2016


        private void transferValues(int rowindex)
        {
            log.LogMethodEntry(rowindex);
            try
            {
                notInitCase = false;
                lblProductid.Tag = rowindex;
                txtCode.Enabled = false;
                lblProductid.Text = view_dgv.Rows[rowindex].Cells["productIdDataGridViewTextBoxColumn"].Value.ToString();
                txtCode.Text = view_dgv.Rows[rowindex].Cells["codeDataGridViewTextBoxColumn"].Value.ToString();//Added 28-Apr-2016
                txtProductName.Text = view_dgv.Rows[rowindex].Cells["productNameDataGridViewTextBoxColumn"].Value.ToString();
                txtDescription.Text = view_dgv.Rows[rowindex].Cells["descriptionDataGridViewTextBoxColumn"].Value.ToString();
                ddlCategory.SelectedValue = view_dgv.Rows[rowindex].Cells["categoryIdDataGridViewTextBoxColumn"].Value;
                //Starts modification on 22-Apr-2016 for adding segments.
                btnSKUSegments.Tag = (view_dgv.Rows[rowindex].Cells["SegmentCategoryId"].Value == DBNull.Value) ? -1 : Convert.ToInt32(view_dgv.Rows[rowindex].Cells["SegmentCategoryId"].Value);
                //Ends modification on 22-Apr-2016 for adding segments.
                txtBarcode.Text = view_dgv.Rows[rowindex].Cells["barCodeDataGridViewTextBoxColumn"].Value.ToString();

                if ((String)view_dgv.Rows[rowindex].Cells["isActiveDataGridViewTextBoxColumn"].Value == "Y")
                {
                    cbxActive.Checked = true;
                    lblActive.BackColor = this.BackColor;
                }
                else
                {
                    cbxActive.Checked = false;
                    lblActive.BackColor = Color.LightGray;
                }

                if ((String)view_dgv.Rows[rowindex].Cells["IsRedeemable"].Value == "Y")
                    cbxIsRedeemable.Checked = true;
                else
                    cbxIsRedeemable.Checked = false;

                if ((String)view_dgv.Rows[rowindex].Cells["isSellableDataGridViewTextBoxColumn"].Value == "Y")
                    cbxIsSellable.Checked = true;
                else
                    cbxIsSellable.Checked = false;

                if (view_dgv.Rows[rowindex].Cells["LowerLimitCost"].Value != DBNull.Value)
                    txtLowerCostLimit.Text = Convert.ToDouble(view_dgv.Rows[rowindex].Cells["LowerLimitCost"].Value).ToString(utilities.ParafaitEnv.INVENTORY_COST_FORMAT);
                else
                    txtLowerCostLimit.Text = "";

                if (view_dgv.Rows[rowindex].Cells["UpperLimitCost"].Value != DBNull.Value)
                    txtUpperCostLimit.Text = Convert.ToDouble(view_dgv.Rows[rowindex].Cells["UpperLimitCost"].Value).ToString(utilities.ParafaitEnv.INVENTORY_COST_FORMAT);
                else
                    txtUpperCostLimit.Text = "";

                if (view_dgv.Rows[rowindex].Cells["CostVariancePercentage"].Value != DBNull.Value)
                    txtCostVariancePer.Text = Convert.ToDouble(view_dgv.Rows[rowindex].Cells["CostVariancePercentage"].Value).ToString(utilities.ParafaitEnv.AMOUNT_FORMAT);
                else
                    txtCostVariancePer.Text = "";

                if (view_dgv.Rows[rowindex].Cells["lastPurchasePriceDataGridViewTextBoxColumn"].Value != DBNull.Value)
                    txtLastpurchaseprice.Text = Convert.ToDouble(view_dgv.Rows[rowindex].Cells["lastPurchasePriceDataGridViewTextBoxColumn"].Value).ToString(utilities.ParafaitEnv.INVENTORY_COST_FORMAT);
                else
                    txtLastpurchaseprice.Text = "";
                //in the following section conditional operator was introduced to check whether the value is dbnull (it was throwing error while trying to convert nulls)-29-04-2015
                txtInnerPackQty.Text = Convert.ToDouble(view_dgv.Rows[rowindex].Cells["innerPackQtyDataGridViewTextBoxColumn"].Value == DBNull.Value ? "" : view_dgv.Rows[rowindex].Cells["innerPackQtyDataGridViewTextBoxColumn"].Value).ToString(utilities.ParafaitEnv.INVENTORY_QUANTITY_FORMAT);
                txtCost.Text = Convert.ToDouble(view_dgv.Rows[rowindex].Cells["costDataGridViewTextBoxColumn"].Value == DBNull.Value ? "" : view_dgv.Rows[rowindex].Cells["costDataGridViewTextBoxColumn"].Value).ToString(utilities.ParafaitEnv.INVENTORY_COST_FORMAT);
                txtSalePrice.Text = Convert.ToDouble(view_dgv.Rows[rowindex].Cells["SalePrice"].Value == DBNull.Value ? "" : view_dgv.Rows[rowindex].Cells["SalePrice"].Value).ToString(utilities.ParafaitEnv.AMOUNT_FORMAT);
                txtReorderpoint.Text = Convert.ToDouble(view_dgv.Rows[rowindex].Cells["reorderPointDataGridViewTextBoxColumn"].Value == DBNull.Value ? 0 : view_dgv.Rows[rowindex].Cells["reorderPointDataGridViewTextBoxColumn"].Value).ToString(utilities.ParafaitEnv.INVENTORY_QUANTITY_FORMAT);
                txtReorderquantity.Text = Convert.ToDouble(view_dgv.Rows[rowindex].Cells["reorderQuantityDataGridViewTextBoxColumn"].Value == DBNull.Value ? "" : view_dgv.Rows[rowindex].Cells["reorderQuantityDataGridViewTextBoxColumn"].Value).ToString(utilities.ParafaitEnv.INVENTORY_QUANTITY_FORMAT);
                ddlDefaultlocation.SelectedValue = view_dgv.Rows[rowindex].Cells["defaultLocationIdDataGridViewTextBoxColumn"].Value;
                ddlOutboundLocation.SelectedValue = view_dgv.Rows[rowindex].Cells["OutboundLocationId"].Value;
                ddlPreferredvendor.SelectedValue = view_dgv.Rows[rowindex].Cells["defaultVendorIdDataGridViewTextBoxColumn"].Value;
                txtRemarks.Text = view_dgv.Rows[rowindex].Cells["remarksDataGridViewTextBoxColumn"].Value.ToString();
                txtPit.Text = string.Format("{0:n2}", view_dgv.Rows[rowindex].Cells["priceInTicketsDataGridViewTextBoxColumn"].Value);
                txtTurnInPIT.Text = string.Format("{0:n0}", view_dgv.Rows[rowindex].Cells["TurnInPriceInTickets"].Value);

                if (view_dgv.Rows[rowindex].Cells["taxInclusiveCost"].Value.ToString() == "Y")
                    cbxTaxInclusiveCost.Checked = true;
                else
                    cbxTaxInclusiveCost.Checked = false;

                if (view_dgv.Rows[rowindex].Cells["TaxId"].Value == DBNull.Value)
                    ddlTax.SelectedIndex = -1;
                else
                    ddlTax.SelectedValue = Convert.ToInt32(view_dgv.Rows[rowindex].Cells["TaxId"].Value);

                if (view_dgv.Rows[rowindex].Cells["uomIdDataGridViewTextBoxColumn"].Value == DBNull.Value)
                    ddlUOM.SelectedIndex = -1;
                else
                    ddlUOM.SelectedValue = Convert.ToInt32(view_dgv.Rows[rowindex].Cells["uomIdDataGridViewTextBoxColumn"].Value);

                if (view_dgv.Rows[rowindex].Cells["isPurchaseableDataGridViewTextBoxColumn"].Value.ToString() == "Y")
                    cbxPurchaseable.Checked = true;
                else
                    cbxPurchaseable.Checked = false;

                pbProductImage.Tag = view_dgv.Rows[rowindex].Cells["ImageFileName"].Value;
                lblImageFileName.Text = pbProductImage.Tag.ToString();
                if (pbProductImage.Tag != DBNull.Value)
                {
                    SqlCommand cmdImage = utilities.getCommand();
                    cmdImage.CommandText = "exec ReadBinaryDataFromFile @FileName";

                    string fileName = pbProductImage.Tag.ToString();
                    lblImageFileName.Text = fileName;
                    cmdImage.Parameters.AddWithValue("@FileName", imageFolder + "\\" + fileName);
                    try
                    {
                        object o = cmdImage.ExecuteScalar();
                        pbProductImage.Image = utilities.ConvertToImage(o);
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        pbProductImage.Image = null;
                    }
                }
                else
                    pbProductImage.Image = null;

                if (view_dgv.Rows[rowindex].Cells["lotControlledDataGridViewCheckBoxColumn"].Value == DBNull.Value)
                {
                    cbxLotControlled.Checked = false;
                }
                else if ((Boolean)view_dgv.Rows[rowindex].Cells["lotControlledDataGridViewCheckBoxColumn"].Value)
                {
                    cbxLotControlled.Checked = true;
                }
                else
                {
                    cbxLotControlled.Checked = false;
                }

                if (view_dgv.Rows[rowindex].Cells["marketListItemDataGridViewCheckBoxColumn"].Value == DBNull.Value)
                {
                    cbxMarketListItem.Checked = false;
                }
                else if ((Boolean)view_dgv.Rows[rowindex].Cells["marketListItemDataGridViewCheckBoxColumn"].Value)
                {
                    cbxMarketListItem.Checked = true;
                }
                else
                {
                    cbxMarketListItem.Checked = false;
                }

                if (view_dgv.Rows[rowindex].Cells["expiryTypeDataGridViewTextBoxColumn"].Value == DBNull.Value)
                {
                    ddlExpiryType.SelectedText = "None";
                }
                else if ((String)view_dgv.Rows[rowindex].Cells["expiryTypeDataGridViewTextBoxColumn"].Value == "N")
                {
                    ddlExpiryType.SelectedText = "None";
                }
                else if ((String)view_dgv.Rows[rowindex].Cells["expiryTypeDataGridViewTextBoxColumn"].Value == "D")
                {
                    ddlExpiryType.SelectedText = "In Days";
                }
                else if ((String)view_dgv.Rows[rowindex].Cells["expiryTypeDataGridViewTextBoxColumn"].Value == "E")
                {
                    ddlExpiryType.SelectedText = "Expiry Date";
                }

                if (view_dgv.Rows[rowindex].Cells["issuingApproachDataGridViewTextBoxColumn"].Value == DBNull.Value)
                {
                    ddlIssueApproach.Text = "None";
                }
                else
                {
                    ddlIssueApproach.Text = view_dgv.Rows[rowindex].Cells["issuingApproachDataGridViewTextBoxColumn"].Value.ToString();
                }

                if (view_dgv.Rows[rowindex].Cells["autoUpdateMarkupDataGridViewCheckBoxColumn"].Value == DBNull.Value)
                {
                    cbxAutoUpdateMarkup.Checked = false;
                }
                else if ((Boolean)view_dgv.Rows[rowindex].Cells["autoUpdateMarkupDataGridViewCheckBoxColumn"].Value)
                {
                    cbxAutoUpdateMarkup.Checked = true;
                }
                else
                {
                    cbxAutoUpdateMarkup.Checked = false;
                }
                if (view_dgv.Rows[rowindex].Cells["itemMarkupPercentDataGridViewTextBoxColumn"].Value != DBNull.Value
                    || view_dgv.Rows[rowindex].Cells["itemMarkupPercentDataGridViewTextBoxColumn"].Value.ToString() != "NaN")
                    txtItemMarkupPercent.Text = Convert.ToDouble(view_dgv.Rows[rowindex].Cells["itemMarkupPercentDataGridViewTextBoxColumn"].Value).ToString(utilities.ParafaitEnv.AMOUNT_FORMAT);
                else
                    txtItemMarkupPercent.Text = "";

                notInitCase = true;
                UISettingsForAutoMarkUp();
            }
            catch
            { notInitCase = true; }
            log.LogMethodExit();
        }


        private void btnTabularView_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                this.Cursor = Cursors.WaitCursor;
                BindingSource productbs = (BindingSource)view_dgv.DataSource;
                var productDTOListOnDisplay = (SortableBindingList<ProductDTO>)productbs.DataSource;
                List<ProductDTO> productDTOList = new List<ProductDTO>();
                foreach (ProductDTO prodDTO in productDTOListOnDisplay)
                {
                    productDTOList.Add(prodDTO);
                }

                int productTabular_form_active = 0;
                foreach (Form child_form in Application.OpenForms)
                {
                    if (child_form.Name == "frmProductTabular")
                    {
                        child_form.Activate();
                        child_form.Focus();
                        productTabular_form_active = 1;
                        child_form.BringToFront();
                    }
                }
                if (productTabular_form_active == 0)
                {
                    frmProductTabular frm = new frmProductTabular(utilities);
                    CommonUIDisplay.setupVisuals(frm);//Setup GUI Design Style Added on 23-Aug-2016  
                    frm.MdiParent = this.MdiParent;
                    frm.Location = new Point(this.Location.X, this.Location.Y);
                    frm.Show();
                    frm.BringToFront();
                    productTabular_form_active = 0;
                }
                //End: Modification added 

                PopulateProducts(null);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 1824, ex.Message));
            }
            finally { this.Cursor = Cursors.Default; }
            log.LogMethodExit();
        }

        private void cbxLotControlled_CheckedChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (cbxLotControlled.Checked)
            {
                ddlExpiryType.Enabled = true;
                ddlIssueApproach.Enabled = true;
            }
            else
            {
                ddlExpiryType.Enabled = false;
                ddlExpiryType.Text = "None";
                ddlIssueApproach.Enabled = false;
                ddlIssueApproach.Text = "None";
            }
            log.LogMethodExit();
        }

        private void cbxMarketListItem_CheckedChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (cbxMarketListItem.Checked)
            {
                cbxLotControlled.Checked = true;
                ddlExpiryType.Enabled = true;
                ddlIssueApproach.Enabled = true;
            }
            log.LogMethodExit();
        }

        //Begin: Modification done for removing side Add Products button on Form Close on 22-Aug-2016
        private void frmProduct_FormClosed(object sender, FormClosedEventArgs e)
        {
            //Begin: Modification done for closing Product Tabular Form if Product Form is closed on 01-Sep-2016
            log.LogMethodEntry();
            foreach (Form child_form in Application.OpenForms)
            {
                if (child_form.Name == "frmProductTabular")
                {
                    child_form.Dispose();
                    break;
                }
            }
            //End: Modification done for closing Product Tabular Form if Product Form is closed on 01-Sep-2016             
            // this.Close();
            log.LogMethodExit();
        }
        //End: Modification done for removing side Add Products button on Form Close on 22-Aug-2016

        private void ddlExpiryType_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (ddlExpiryType.Text == "In Days")
            {
                txtIndays.Visible = true;
            }
            else
            {
                txtIndays.Visible = false;
            }

            ComboBox cmb = (ComboBox)sender;
            if (cmb.Focused)
            {
                string expiryType;
                //if (view_dgv.SelectedRows[0] != null)
                //    expiryType = view_dgv.SelectedRows[0].Cells["dataGridViewTextBoxColumn20"].Value.ToString();
                //else
                {
                    if (ddlExpiryType.SelectedItem.ToString() == "Expiry Date")
                        expiryType = "E";
                    else if (ddlExpiryType.SelectedItem.ToString() == "In Days")
                        expiryType = "D";
                    else
                        expiryType = "N";
                }
                string selectedValue = ddlExpiryType.SelectedItem.ToString();

                if (lblProductid.Text != "")
                {
                    if ((expiryType == "E" || expiryType == "D") && selectedValue == "None")
                    {
                        ddlExpiryType.Text = (expiryType == "E" ? "Expiry Date" : "In Days");
                    }
                    else if ((expiryType == "N" && ddlIssueApproach.SelectedItem.ToString() == "FIFO") && (selectedValue == "In Days" || selectedValue == "Expiry Date"))
                        ddlExpiryType.Text = "None";
                }
                else
                {
                    if (selectedValue == "In Days" || selectedValue == "Expiry Date")
                    {
                        cbxLotControlled.Checked = true;
                        ddlIssueApproach.SelectedItem = "FEFO";
                    }
                }
            }
            log.LogMethodExit();
        }

        //Added method 21-Feb-2017
        private void ddlIssueApproach_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ComboBox cmb = (ComboBox)sender;
            if (cmb.Focused)
            {
                string issueApproach;
                if (view_dgv.SelectedRows[0] != null)
                    issueApproach = view_dgv.SelectedRows[0].Cells["issuingApproachDataGridViewTextBoxColumn"].Value.ToString();
                else
                    issueApproach = ddlIssueApproach.SelectedItem.ToString();
                string selectedValue = ddlIssueApproach.SelectedItem.ToString();

                if (lblProductid.Text != "")
                {
                    if ((issueApproach != "None") && selectedValue == "None")
                    {
                        ddlIssueApproach.Text = issueApproach;
                    }
                }
                else
                {
                    if (selectedValue != "None")
                    {
                        cbxLotControlled.Checked = true;
                    }
                }
            }
            log.LogMethodExit();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.Close();
            log.LogMethodExit();
        }

        private void txtPit_KeyPress(object sender, KeyPressEventArgs e)
        {
            // This is to accept only numbers. 
            log.LogMethodEntry();
            if (!Char.IsNumber(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
            log.LogMethodExit();
        }

        private void txtReorderpoint_KeyPress(object sender, KeyPressEventArgs e)
        {
            // This is to accept only numbers. 
            log.LogMethodEntry();
            if (!Char.IsNumber(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
            log.LogMethodExit();
        }

        private void txtReorderquantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry();
            // This to accept only numbers. 
            if (!Char.IsNumber(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
            log.LogMethodExit();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (lblProductid.Text == "")
            {
                try
                {
                    LoadProductFromDGV(0);
                }
                catch (Exception ex)
                {
                    EmptyAllFields();
                    log.Error(ex);
                    MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 1824, ex.Message));
                }
            }
            else if (MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 889), MessageContainerList.GetMessage(utilities.ExecutionContext, "Delete Product"), MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    Products products = new Products(machineUserContext, manualProductId, true, true);
                    if (products.GetProductsDTO != null && products.GetProductsDTO.InventoryItemDTO != null)
                    {
                        products.GetProductsDTO.ActiveFlag = false;
                        products.Save();
                    }
                    PopulateProducts(null);
                }
                catch (ValidationException ex)
                {
                    log.Error(ex);
                    MessageBox.Show(ex.GetAllValidationErrorMessages(), MessageContainerList.GetMessage(utilities.ExecutionContext, "Delete Product"));
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 890), MessageContainerList.GetMessage(utilities.ExecutionContext, "Delete Product"));
                }
            }
            log.LogMethodExit();
        }

        private void refreshDgv()
        {
            log.LogMethodEntry();
            view_dgv.Refresh();
            log.LogMethodExit();
        }

        private void btnAddTax_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                this.Cursor = Cursors.WaitCursor;
                PurchaseTaxUI = new PurchaseTaxUI(utilities);
                CommonUIDisplay.setupVisuals(PurchaseTaxUI);//Added for GUI Design style on 23-Aug-2016
                PurchaseTaxUI.StartPosition = FormStartPosition.CenterScreen;//Added for showing in center on 23-Aug-2016
                PurchaseTaxUI.ShowDialog();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 1824, ex.Message));
            }
            finally { this.Cursor = Cursors.Default; }
            log.LogMethodExit();
        }

        bool FireSelectionChanged = true;
        private void view_dgv_SelectionChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (FireSelectionChanged)
                {
                    if (view_dgv.SelectedRows.Count > 0)
                    {
                        if (view_dgv.SelectedRows[0].Cells["dataGridViewTextBoxColumn2"].Value != DBNull.Value)
                        {

                            LoadProductFromDGV(view_dgv.SelectedRows[0].Index);
                            BOM_Load(Convert.ToInt32(view_dgv.SelectedRows[0].Cells["dataGridViewTextBoxColumn1"].Value));
                        }
                    }
                    else
                    {
                        EmptyAllFields();
                        BOM_Load(-1);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }


        private void BOM_Load(int productId)
        {
            log.LogMethodEntry(productId);
            if (productId != -1)
            {
                populateTree(productId);
                if (tvBOM.Nodes.Count > 0)
                {
                    tvBOM.Nodes[0].NodeFont = new System.Drawing.Font(tvBOM.Font, FontStyle.Bold);
                    tvBOM.Nodes[0].ExpandAll();
                    tvBOM.Nodes[0].Text = tvBOM.Nodes[0].Text; // reassign to set proper width for text
                }
            }
            else
                tvBOM.Nodes.Clear();
            log.LogMethodExit();
        }

        private List<ProductDTO> getProducts(int LocalProductId)
        {
            log.LogMethodEntry(LocalProductId);
            ProductList productList = new ProductList();
            List<KeyValuePair<ProductDTO.SearchByProductParameters, string>> SearchParameter = new List<KeyValuePair<ProductDTO.SearchByProductParameters, string>>();

            if (LocalProductId != -1)
            {
                SearchParameter.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.PRODUCT_ID, LocalProductId.ToString()));
            }
            List<ProductDTO> productListOnDisplay = productList.GetAdancedAllProducts(SearchParameter);
            log.LogMethodExit(productListOnDisplay);
            return (productListOnDisplay);
        }

        private void populateTree(int LocalProductId)
        {
            log.LogMethodEntry(LocalProductId);
            try
            {
                tvBOM.Nodes.Clear();
                List<ProductDTO> productDTOList = getProducts(LocalProductId);
                if (productDTOList != null)
                {
                    for (int i = 0; i < productDTOList.Count; i++)
                    {
                        if (productDTO.Code != null && productDTO.Description != null)
                        {
                            TreeNode node = new TreeNode(productDTO.Code.ToString() + productDTO.Description);
                            node.Tag = productDTO.ProductId;
                            tvBOM.Nodes.Add(node);
                            getNodes(node);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 1824, ex.Message));
            }
            log.LogMethodExit();
        }

        private TreeNode[] getChildren(int LocalProductId)
        {
            log.LogMethodEntry(LocalProductId);
            ProductList productList = new ProductList();
            TreeNode[] tnCollection = productList.getChildren(LocalProductId);
            log.LogMethodExit(tnCollection);
            return tnCollection;
        }

        private TreeNode getNodes(TreeNode rootNode)
        {
            log.LogMethodEntry(rootNode);
            TreeNode[] tn = getChildren(Convert.ToInt32(rootNode.Tag));
            if (tn == null)
            {
                log.LogMethodExit();
                return null;
            }
            else
            {
                foreach (TreeNode tnode in tn)
                {
                    if (Convert.ToInt32(tnode.Tag) == productDTO.ProductId)
                    {
                        MessageBox.Show(MessageContainerList.GetMessage(machineUserContext, 2872)); // Incorrect product setup for Product BOM.
                        break;
                    }
                    TreeNode node = getNodes(tnode);
                    if (node == null)
                        rootNode.Nodes.Add(tnode);
                    else
                        rootNode.Nodes.Add(node);
                }

                log.LogMethodExit(rootNode);
                return (rootNode);
            }
        }

        private void tvBOM_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            log.LogMethodEntry();
            //if (e.Button == System.Windows.Forms.MouseButtons.Right)
            //tvBOM.SelectedNode = e.Node;
            log.LogMethodExit();
        }

        private void refreshNode(TreeNode node)
        {
            log.LogMethodEntry(node);
            node.Nodes.Clear();
            getNodes(node);
            node.ExpandAll();
            log.LogMethodExit();
        }

        private void refreshSelectedNode()
        {
            log.LogMethodEntry();
            if (tvBOM.SelectedNode != null)
                refreshNode(tvBOM.SelectedNode);
            log.LogMethodExit();
        }

        int selectedProductId;
        private void tvBOM_AfterSelect(object sender, TreeViewEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            selectedProductId = Convert.ToInt32(e.Node.Tag);
            log.LogMethodExit();
        }

        private void Remove_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                TreeNode tn = tvBOM.SelectedNode;
                if (tn == null)
                    return;
                if (tn.Parent == null)
                    return;
                TreeNode tnParent = tn.Parent;

                if (MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 898), MessageContainerList.GetMessage(utilities.ExecutionContext, "Remove Node"), MessageBoxButtons.YesNoCancel) == System.Windows.Forms.DialogResult.Yes)
                {
                    int localProductId = Convert.ToInt32(tn.Tag);
                    int localParentProductId = Convert.ToInt32(tnParent.Tag);
                    BOMList bOMList = new BOMList(utilities.ExecutionContext);
                    List<KeyValuePair<BOMDTO.SearchByBOMParameters, string>> searchBOMParameters = new List<KeyValuePair<BOMDTO.SearchByBOMParameters, string>>();
                    searchBOMParameters.Add(new KeyValuePair<BOMDTO.SearchByBOMParameters, string>(BOMDTO.SearchByBOMParameters.PRODUCT_ID, localParentProductId.ToString()));
                    searchBOMParameters.Add(new KeyValuePair<BOMDTO.SearchByBOMParameters, string>(BOMDTO.SearchByBOMParameters.CHILDPRODUCT_ID, localProductId.ToString()));
                    List<BOMDTO> bOMDTOList = bOMList.GetAllBOMs(searchBOMParameters);
                    if (bOMDTOList != null)
                    {
                        foreach (BOMDTO bOMDTO in bOMDTOList)
                        {
                            bOMDTO.Isactive = false;
                            BOMBL bomBL = new BOMBL(machineUserContext, bOMDTO);
                            bomBL.Save();
                        }
                    }
                    tvBOM.SelectedNode = tnParent;
                    refreshNode(tnParent);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 1824, ex.Message));
            }
            log.LogMethodExit();
        }

        private void BOMEditMenu_Opening(object sender, CancelEventArgs e)
        {
            log.LogMethodEntry();
            if (tvBOM.SelectedNode == null)
                return;
            if (tvBOM.SelectedNode.Level == 0)
            {
                EditBOM.Enabled = false;
                Remove.Enabled = false;
                Add.Enabled = false;
            }
            log.LogMethodExit();
        }

        private void Add_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                this.Cursor = Cursors.WaitCursor;
                TreeNode tn = tvBOM.SelectedNode;
                //if (tn != null)
                //{
                //    int localProductId;
                //    if (tn.Parent == null)
                //    {
                //        localProductId = Convert.ToInt32(tn.Tag);
                //    }
                //    else
                //    {
                //        localProductId = Convert.ToInt32(tn.Parent.Tag);
                //    }

                //    //frmBOM frm = new frmBOM(localProductId, tn.Text, utilities);
                //    frmBOM frm = new frmBOM(localProductId, tn.Text, utilities ,true);
                //    //frm.StartPosition = FormStartPosition.CenterScreen;
                //    CommonUIDisplay.setupVisuals(frm);
                //    try
                //    {
                //        frm.ShowDialog();
                //    }
                //    catch (Exception ex)
                //    {
                //        log.Error(ex);
                //        MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 1824, ex.Message));
                //    }
                //    refreshNode(tn);
                //}

                //{
                frmBOM frm = new frmBOM(productDTO.ProductId, utilities, true);
                CommonUIDisplay.setupVisuals(frm);
                try
                {
                    frm.ShowDialog();
                    // btnRefresh.PerformClick();
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 1824, ex.Message));
                }
            }
            finally { this.Cursor = Cursors.Default; }
            log.LogMethodExit();
        }

        private void menuRefresh_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            refreshSelectedNode();
            log.LogMethodExit();
        }

        private void tvBOM_KeyUp(object sender, KeyEventArgs e)
        {
            log.LogMethodEntry();
            if (e.KeyValue == (int)Keys.Delete)
                Remove.PerformClick();
            log.LogMethodExit();
        }


        private void tvBOM_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            log.LogMethodEntry();
            EditBOM.PerformClick();
            log.LogMethodExit();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            EditBOM.PerformClick();
            this.ActiveControl = tvBOM;
            log.LogMethodExit();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            btnAdd.Enabled = true;
            Add.PerformClick();
            this.ActiveControl = tvBOM;
            log.LogMethodExit();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            try
            {
                log.LogMethodEntry();
                TreeNode tn = tvBOM.SelectedNode;
                if (tn == null)
                    return;
                if (tn.Parent == null)
                    return;
                TreeNode tnParent = tn.Parent;

                if (MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 898), MessageContainerList.GetMessage(utilities.ExecutionContext, "Remove Node"), MessageBoxButtons.YesNoCancel) == System.Windows.Forms.DialogResult.Yes)
                {
                    int localProductId = Convert.ToInt32(tn.Tag);
                    int localParentProductId = Convert.ToInt32(tnParent.Tag);
                    BOMList bOMList = new BOMList(machineUserContext);
                    List<KeyValuePair<BOMDTO.SearchByBOMParameters, string>> searchBOMParameters = new List<KeyValuePair<BOMDTO.SearchByBOMParameters, string>>();
                    searchBOMParameters.Add(new KeyValuePair<BOMDTO.SearchByBOMParameters, string>(BOMDTO.SearchByBOMParameters.PRODUCT_ID, localParentProductId.ToString()));
                    searchBOMParameters.Add(new KeyValuePair<BOMDTO.SearchByBOMParameters, string>(BOMDTO.SearchByBOMParameters.CHILDPRODUCT_ID, localProductId.ToString()));
                    List<BOMDTO> bOMDTOList = bOMList.GetAllBOMs(searchBOMParameters);
                    if (bOMDTOList != null)
                    {
                        foreach (BOMDTO bOMDTO in bOMDTOList)
                        {
                            bOMDTO.Isactive = false;
                            BOMBL bomBL = new BOMBL(machineUserContext, bOMDTO);
                            bomBL.Save();
                        }
                    }
                    tvBOM.SelectedNode = tnParent;
                    refreshNode(tnParent);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 1824, ex.Message));
            }
            log.LogMethodExit();
        }

        private void tsbClear_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            btnSave.Enabled = true;
            productCodeToolStripTextBox.Text = "";
            DescriptiontoolStripTextBox.Text = "";
            cmbCategory.SelectedIndex =
            cmbProductType.SelectedIndex =
            cmbActive.SelectedIndex = 0;
            BarcodetoolStripTextBox.Text = "";
            AdvancedSearch = null; //Added 27-Apr-2016
            fillByToolStripButton.PerformClick();
            log.LogMethodExit();
        }

        private void productCodeToolStripTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry();
            if (e.KeyChar == 13)
                fillByToolStripButton.PerformClick();
            log.LogMethodExit();
        }

        private void DescriptiontoolStripTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry();
            if (e.KeyChar == 13)
                fillByToolStripButton.PerformClick();
            log.LogMethodExit();
        }

        private void lnkProductImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry();
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Title = "Upload Product Image";
            fileDialog.Filter = "Image Files (*.gif, *.jpg, *.jpeg, *.wmf, *.png, *.ico, *.bmp)|*.gif; *.jpg; *.jpeg; *.wmf; *.png; *.ico; *.bmp|All Files(*.*)|*.*";
            if (fileDialog.ShowDialog() == DialogResult.Cancel)
                return;

            try
            {
                pbProductImage.Image = Image.FromFile(fileDialog.FileName);
                pbProductImage.Tag = (new System.IO.FileInfo(fileDialog.FileName)).Name;
                lblImageFileName.Text = pbProductImage.Tag.ToString();
                SqlCommand cmd = utilities.getCommand();
                cmd.CommandText = "exec SaveBinaryDataToFile @Image, @FileName";
                cmd.Parameters.AddWithValue("@Image", System.IO.File.ReadAllBytes(fileDialog.FileName));
                cmd.Parameters.AddWithValue("@FileName", imageFolder + "\\" + pbProductImage.Tag.ToString());
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 601), MessageContainerList.GetMessage(utilities.ExecutionContext, "Image File Error"));
            }
            log.LogMethodExit();
        }

        private void lnkClearImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry();
            pbProductImage.Image = null;
            lblImageFileName.Text = "";
            pbProductImage.Tag = DBNull.Value;
            log.LogMethodExit();
        }

        private void lnkBuildCostfromBOM_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry();
            ProductList productList = new ProductList();
            List<KeyValuePair<ProductDTO.SearchByProductParameters, string>> SearchParameter = new List<KeyValuePair<ProductDTO.SearchByProductParameters, string>>();
            if (!string.IsNullOrEmpty(lblProductid.Text))
                SearchParameter.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.PRODUCT_ID, lblProductid.Text));
            List<ProductDTO> productListOnDisplay = productList.GetAdancedAllProducts(SearchParameter);
            if (productListOnDisplay != null)
            {
                try
                {
                    object response = productList.GetBOMProductCost(productDTO.ProductId);
                    if (response != DBNull.Value)
                    {
                        txtCost.Text = Convert.ToDecimal(response).ToString(utilities.ParafaitEnv.INVENTORY_COST_FORMAT);
                        txtCost.Focus();
                        txtInnerPackQty.Focus();
                        txtCost.Focus();
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 1250), MessageContainerList.GetMessage(utilities.ExecutionContext, "Cost Validation"));
                }
            }
            log.LogMethodExit();
        }

        private void txtInnerPackQty_Validating(object sender, CancelEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                double val;
                if (txtInnerPackQty.Text != "")
                    val = Convert.ToDouble(txtInnerPackQty.Text);
                txtInnerPackQty.BackColor = Color.White;
            }
            catch
            {
                txtInnerPackQty.BackColor = Color.Yellow;
                MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 900), MessageContainerList.GetMessage(utilities.ExecutionContext, "Cost Unit Validation"));
                e.Cancel = true;
            }
            log.LogMethodExit();
        }

        private void EditBOM_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                this.Cursor = Cursors.WaitCursor;
                TreeNode tn = tvBOM.SelectedNode;
                if (tn != null)
                {
                    if (tn.Parent == null)
                        return;
                    int localProductId = Convert.ToInt32(tn.Tag);
                    frmBOM frm = new frmBOM(localProductId, utilities, false);
                    frm.StartPosition = FormStartPosition.CenterScreen;
                    frm.ShowDialog();
                    refreshNode(tn.Parent);
                }
                else
                {
                    if (tvBOM.Nodes.Count > 0)
                        tvBOM.SelectedNode = tvBOM.Nodes[0];
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 1824, ex.Message));
            }
            finally { this.Cursor = Cursors.Default; }
            log.LogMethodExit();
        }

        private void EditProduct_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                TreeNode tn = tvBOM.SelectedNode;
                if (tn != null)
                {
                    int localProductId = Convert.ToInt32(tn.Tag);

                    tsbClear.PerformClick();

                    fillByToolStripButton.PerformClick();
                    for (int i = 0; i < view_dgv.Rows.Count; i++)
                    {
                        if (Convert.ToInt32(view_dgv[dataGridViewCheckBoxColumn1.Index, i].Value) == localProductId)
                        {
                            view_dgv.CurrentCell = view_dgv[dataGridViewTextBoxColumn2.Index, i];
                            break;
                        }
                    }
                }
                else
                {
                    if (tvBOM.Nodes.Count > 0)
                        tvBOM.SelectedNode = tvBOM.Nodes[0];
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 1824, ex.Message));
            }
            log.LogMethodExit();
        }

        private void txtLowerCostLimit_Validating(object sender, CancelEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                double val;
                if (txtLowerCostLimit.Text != "")
                    val = Convert.ToDouble(txtLowerCostLimit.Text);
                txtLowerCostLimit.BackColor = Color.White;
            }
            catch
            {
                txtLowerCostLimit.BackColor = Color.Yellow;
                MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 901), MessageContainerList.GetMessage(utilities.ExecutionContext, "Cost Validation"));
                e.Cancel = true;
            }
            log.LogMethodExit();
        }

        private void txtUpperCostLimit_Validating(object sender, CancelEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                double val;
                if (txtUpperCostLimit.Text != "")
                    val = Convert.ToDouble(txtUpperCostLimit.Text);
                txtUpperCostLimit.BackColor = Color.White;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                txtUpperCostLimit.BackColor = Color.Yellow;
                MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 902), MessageContainerList.GetMessage(utilities.ExecutionContext, "Cost Validation"));
                e.Cancel = true;
            }
            log.LogMethodExit();
        }

        private void txtCostVariancePer_Validating(object sender, CancelEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                double val;
                if (txtCostVariancePer.Text != "")
                    val = Convert.ToDouble(txtCostVariancePer.Text);
                txtCostVariancePer.BackColor = Color.White;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                txtCostVariancePer.BackColor = Color.Yellow;
                MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 903), MessageContainerList.GetMessage(utilities.ExecutionContext, "Cost Validation"));
                e.Cancel = true;
            }
            log.LogMethodExit();
        }

        private void ProductSearch()
        {
            log.LogMethodEntry();
            btnSave.Enabled = true;
            FireSelectionChanged = false;
            try
            {
                ProductList productList = new ProductList(machineUserContext);
                List<KeyValuePair<ProductDTO.SearchByProductParameters, string>> SearchParameter = new List<KeyValuePair<ProductDTO.SearchByProductParameters, string>>();
                SearchParameter.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.CODE, productCodeToolStripTextBox.Text));
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
                if (cmbCategory.SelectedIndex > 0)
                {
                    CategoryList categoryList = new CategoryList(machineUserContext);
                    List<KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>> SearchCategoryParameter = new List<KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>>();
                    SearchCategoryParameter.Add(new KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>(CategoryDTO.SearchByCategoryParameters.NAME, cmbCategory.SelectedItem.ToString()));
                    List<CategoryDTO> categoryListDTO = categoryList.GetAllCategory(SearchCategoryParameter);
                    if (categoryListDTO != null)
                    {
                        SearchParameter.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.CATEGORY, categoryListDTO[0].CategoryId.ToString()));
                    }
                }
                if (DescriptiontoolStripTextBox.Text.Trim() != "")
                {
                    SearchParameter.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.DESCRIPTION, DescriptiontoolStripTextBox.Text));
                }

                if (BarcodetoolStripTextBox.Text.Trim() != "")
                {
                    SearchParameter.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.BARCODE, BarcodetoolStripTextBox.Text));
                }
                //Updated to include search by segmentcategoryid (Advanced search)
                if (AdvancedSearch != null)
                {
                    if (!string.IsNullOrEmpty(AdvancedSearch.searchCriteria))
                    {
                        FireSelectionChanged = true;
                        PopulateProducts(productList.GetSearchCriteriaAllProducts(SearchParameter, AdvancedSearch));
                    }
                }
                else
                {
                    FireSelectionChanged = true;
                    SearchParameter.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                    PopulateProducts(productList.GetSearchCriteriaAllProducts(SearchParameter, null));
                }

                if (view_dgv.Rows.Count <= 0)
                {
                    EmptyAllFields();
                    btnSave.Enabled = false;
                    txtCode.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 1824, ex.Message));
            }
            log.LogMethodExit();
        }
        private void fillByToolStripButton_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ProductSearch();
            log.LogMethodExit();
        }

        private void btnPrintBOM_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (tvBOM.Nodes.Count == 0)
                {
                    log.LogMethodExit();
                    return;
                }
                Semnox.Parafait.Product.PrintHelper print = new PrintHelper();
                print.PrintTree(this.tvBOM, "Product Bill of Material");
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 1824, ex.Message));
            }
            log.LogMethodExit();
        }

        private void txtTurnInPIT_KeyPress(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry();
            if (!Char.IsNumber(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
            log.LogMethodExit();
        }

        private void BarcodetoolStripTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry();
            if (e.KeyChar == 13)
                fillByToolStripButton.PerformClick();
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

        private void tbsAdvancedSearched_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
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
                        MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 1000));
                        log.LogMethodExit();
                        return;
                    }
                }
                if (AdvancedSearch.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        fillByToolStripButton.PerformClick();
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 1824, ex.Message));
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 1824, ex.Message));
            }
            log.LogMethodExit();
        }
        //for advanced search

        private void btnSKUSegments_Click(object sender, EventArgs e)//for adding segments.
        {
            log.LogMethodEntry();
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (lblProductid.Text != "")
                {
                    segmentCategorizationValueUI = segmentCategorizationValueUI = new SegmentCategorizationValueUI(utilities, utilities.ParafaitEnv, "Product", Convert.ToInt32(lblProductid.Text), (btnSKUSegments.Tag == null) ? -1 : (int)btnSKUSegments.Tag, txtDescription.Text);
                    CommonUIDisplay.setupVisuals(segmentCategorizationValueUI);//Added for GUI Design style 
                    segmentCategorizationValueUI.StartPosition = FormStartPosition.CenterScreen;//Added for showing in center
                    segmentCategorizationValueUI.ShowDialog();
                    PopulateProducts(null);
                }
                else
                {
                    // to add Message No.
                    MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 1017), MessageContainerList.GetMessage(utilities.ExecutionContext, "Add Segment"));
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 1824, ex.Message));
            }
            finally { this.Cursor = Cursors.Default; }
            log.LogMethodExit();
        }
        // for adding segments.

        //Added method to see that top 1 bar code is shown in the screen
        private void view_dgv_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (e.Row.Index > -1)
                {
                    if (e.Row.Cells["dataGridViewTextBoxColumn1"].Value != null && e.Row.Cells["dataGridViewTextBoxColumn2"].Value != null)
                    {
                        if (e.Row.Cells["dataGridViewTextBoxColumn23"].Value == null)
                        {
                            ProductBarcodeListBL productBarcodeListBL = new ProductBarcodeListBL(machineUserContext);
                            List<KeyValuePair<ProductBarcodeDTO.SearchByParameters, string>> SearchParameter = new List<KeyValuePair<ProductBarcodeDTO.SearchByParameters, string>>();
                            SearchParameter.Add(new KeyValuePair<ProductBarcodeDTO.SearchByParameters, string>(ProductBarcodeDTO.SearchByParameters.PRODUCT_ID, e.Row.Cells["productIdDataGridViewTextBoxColumn"].Value.ToString()));
                            List<ProductBarcodeDTO> productBarcodeListOnDisplay = productBarcodeListBL.GetProductBarcodeDTOList(SearchParameter);

                            if (productBarcodeListOnDisplay != null)
                            {
                                ProductBarcodeDTO productBarcodeDTO = new ProductBarcodeDTO();
                                e.Row.Cells["dataGridViewTextBoxColumn23"].Value = productBarcodeDTO.BarCode;
                            }
                            else
                                e.Row.Cells["dataGridViewTextBoxColumn23"].Value = "";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 1824, ex.Message));
            }
            log.LogMethodExit();
        }


        //Added function to enable editing custom attributes
        private void btnCustom_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (lblProductid.Text == "")
                {
                    //code to add Message No.
                    MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 1010), MessageContainerList.GetMessage(utilities.ExecutionContext, "Add Custom Attributes"));
                }
                else
                {
                    CustomDataUI cd;
                    try
                    {
                        cd = new CustomDataUI("INVPRODUCT", Convert.ToInt32(lblProductid.Text), null, utilities);
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        log.LogMethodExit();
                        return;
                    }
                    cd.ShowDialog();
                    PopulateProducts(null);
                }
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
            int selectedIndexVal = 0;
            try
            {
                selectedIndexVal = view_dgv.SelectedRows[0].Index;
                if (selectedIndexVal < 0) selectedIndexVal = 0;
            }
            catch
            { selectedIndexVal = 0; }
            PopulateProducts(null);
            if (selectedIndexVal > 0)
            {
                try
                {
                    view_dgv.Rows[selectedIndexVal].Selected = true;
                    LoadProductFromDGV(selectedIndexVal);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 1824, ex.Message));
                }
            }
            cmbCategory.SelectedIndex = 0;
            cmbProductType.SelectedIndex = 0;
            cmbActive.SelectedIndex = 0;

            log.LogMethodExit();
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                this.Cursor = Cursors.WaitCursor;
                using (frmUploadProduct frm = new frmUploadProduct(utilities))
                {
                    CommonUIDisplay.setupVisuals(frm);
                    frm.StartPosition = FormStartPosition.CenterScreen;
                    frm.ShowDialog();
                }
                PopulateProducts(null);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 1824, ex.Message));
            }
            finally { this.Cursor = Cursors.Default; }
            log.LogMethodExit();
        }

        private void lnkPublishToSite_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry();

            PublishUI publishUI;

            if (!string.IsNullOrEmpty(lblProductid.Text) && Convert.ToInt32(lblProductid.Text) >= 0)
            {
                publishUI = new PublishUI(utilities, Convert.ToInt32(lblProductid.Text), "Product", txtCode.Text);
                publishUI.ShowDialog();
            }
            log.LogMethodExit();

        }

        /// <summary>
        /// Method to adjust display controls in the UI form
        /// </summary>
        /// <param name="isInventory"></param>
        private void AdjustDisplayComponents(bool isInventory)
        {
            log.LogMethodEntry(isInventory);
            if (!isInventory)
            {
                fillByToolStrip.Visible = false;
                gb_view.Visible = false;
                tvBOM.Visible = true;
                lblImageFileName.Text = "";
                btnUpload.Visible = false;
                btnDuplicate.Visible = false;
                btnNew.Visible = false;
                btnDelete.Visible = false;
                gb_add.Width += 60;
                groupBox3.Width += 40;
                groupBox2.Width += 40;
                groupBox1.Width += 40;
                gb_add.Location = new Point(50, 30);
                btnTabularView.Visible = false;

                btnExit.Location = new Point(btnExit.Location.X - 105, btnExit.Location.Y);
                btnSave.Location = new Point(btnExit.Location.X + btnExit.Size.Width + 50, btnExit.Location.Y);
                btnRefresh.Location = new Point(btnSave.Location.X + btnSave.Size.Width + 40, btnExit.Location.Y);
                btnTabularView.Location = new Point(btnRefresh.Location.X + btnRefresh.Size.Width + 40, btnRefresh.Location.Y);
                btnEdit.Location = new Point(btnTabularView.Location.X + btnTabularView.Size.Width + 100, btnTabularView.Location.Y - 5);
                btnAdd.Location = new Point(btnEdit.Location.X + btnEdit.Size.Width + 50, btnEdit.Location.Y);
                btnRemove.Location = new Point(btnAdd.Location.X + btnAdd.Size.Width + 50, btnAdd.Location.Y);
                btnPrintBOM.Location = new Point(btnRemove.Location.X + btnRemove.Size.Width + 50, btnRemove.Location.Y);

                tvBOM.Location = new Point(gb_add.Location.X + gb_add.Width + 10, 47);
                tvBOM.Width += 100;
                lnkPublishToSite.Location = new Point(lnkPublishToSite.Location.X, lnkPublishToSite.Location.Y - 5);
                this.StartPosition = FormStartPosition.CenterScreen;
            }
            else
            {
                if (ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "ALLOW_PRODUCT_UPLOAD_IN_INVENTORY").Equals("Y"))
                {
                    btnUpload.Visible = true;
                }
                else
                {
                    btnUpload.Visible = false;
                }
                //btnDuplicate.Visible = false;

                //btnExit.Location = new Point(btnUpload.Location.X + btnUpload.Size.Width + 40, btnExit.Location.Y);
                //btnNew.Location = new Point(btnExit.Location.X + btnExit.Size.Width + 40, btnExit.Location.Y);
                //btnDelete.Location = new Point(btnNew.Location.X + btnNew.Size.Width + 40, btnExit.Location.Y);
                //btnSave.Location = new Point(btnDelete.Location.X + btnDelete.Size.Width + 40, btnExit.Location.Y);
                //btnRefresh.Location = new Point(btnSave.Location.X + btnSave.Size.Width + 40, btnExit.Location.Y);

            }
            log.LogMethodExit();
        }

        private void BuildProductDTO()
        {
            log.LogMethodEntry();
            if (productsDTO != null && productsDTO.InventoryItemDTO != null)
            {
                try
                {
                    lblProductid.Text = productsDTO.InventoryItemDTO.ProductId.ToString();
                    txtCode.Text = productsDTO.InventoryItemDTO.Code.ToString();
                    txtProductName.Text = productsDTO.ProductName.ToString();
                    txtDescription.Text = productsDTO.Description.ToString();
                    //txtDescription.ReadOnly = true;
                    //txtProductName.ReadOnly = true;
                    txtSalePrice.ReadOnly = true;
                    //txtCode.ReadOnly = true;
                    ddlUOM.SelectedValue = productDTO.UomId;
                    cmbInventoryUOM.SelectedValue = productDTO.InventoryUOMId;
                    txtYieldPer.Text = productDTO.YieldPercentage.ToString();
                    txtPreparationTime.Text = productDTO.PreparationTime.ToString();
                    cbxIncludeInPlan.Checked = productDTO.IncludeInPlan.Equals(true);
                    cmbItemType.SelectedValue = productDTO.ItemType;
                    ddlTax.SelectedValue = productDTO.PurchaseTaxId;
                    ddlDefaultlocation.SelectedValue = productDTO.DefaultLocationId;
                    ddlOutboundLocation.SelectedValue = productDTO.OutboundLocationId;
                    ddlPreferredvendor.SelectedValue = productDTO.DefaultVendorId;
                    cbxActive.Checked = productsDTO.ActiveFlag;//IsActive.Equals("Y");
                                                               //cbxActive.Enabled = false;
                    cbxIsRedeemable.Checked = productDTO.IsRedeemable.Equals("Y");
                    cbxIsSellable.Checked = productDTO.IsSellable.Equals("Y");
                    cbxLotControlled.Checked = productDTO.LotControlled;
                    cbxLotControlled.Enabled = !productDTO.LotControlled;
                    ddlExpiryType.Enabled = productDTO.LotControlled;
                    ddlIssueApproach.Enabled = productDTO.LotControlled; //Added 21-Feb-2017
                    cbxMarketListItem.Checked = productDTO.MarketListItem;
                    cbxPurchaseable.Checked = productDTO.IsPurchaseable.Equals("Y");
                    cbxTaxInclusiveCost.Checked = productDTO.TaxInclusiveCost != null ? ((productDTO.TaxInclusiveCost.Equals("Y")) ? true : false) : false;
                    txtReorderpoint.Text = productDTO.ReorderPoint.ToString();
                    txtReorderquantity.Text = productDTO.ReorderQuantity.ToString();
                    txtCost.Text = productDTO.Cost.ToString();
                    if (productsDTO.Price == -1)
                    {
                        txtSalePrice.Text = (0).ToString(utilities.ParafaitEnv.AMOUNT_FORMAT);
                    }
                    else
                    {
                        txtSalePrice.Text = productsDTO.Price.ToString(utilities.ParafaitEnv.AMOUNT_FORMAT);
                    }
                    txtInnerPackQty.Text = productDTO.InnerPackQty.ToString();
                    btnSKUSegments.Tag = productDTO.SegmentCategoryId;
                    btnCustom.Tag = productDTO.CustomDataSetId;
                    if (productDTO.ExpiryType.Equals("N"))
                        ddlExpiryType.Text = "None";
                    else if (productDTO.ExpiryType.Equals("D"))
                    {
                        ddlExpiryType.Text = "In Days";
                        txtIndays.Text = productDTO.ExpiryDays.ToString();
                        txtIndays.Visible = true;
                    }
                    else if (productDTO.ExpiryType.Equals("E"))
                        ddlExpiryType.Text = "Expiry Date";

                    ddlIssueApproach.Text = productDTO.IssuingApproach;
                    txtRemarks.Text = productDTO.Remarks;
                    ddlCategory.SelectedValue = productsDTO.CategoryId;
                    ddlCategory.Enabled = true;
                    txtLowerCostLimit.Text = productDTO.LowerLimitCost.ToString();
                    txtUpperCostLimit.Text = productDTO.UpperLimitCost.ToString();
                    txtCostVariancePer.Text = productDTO.CostVariancePercentage.ToString();
                    txtLastpurchaseprice.Text = productDTO.LastPurchasePrice.ToString();
                    txtPit.Text = productDTO.PriceInTickets.ToString();
                    txtPit.BackColor = Color.White;
                    txtTurnInPIT.Text = productDTO.TurnInPriceInTickets.ToString();
                    txtBarcode.Text = productDTO.BarCode;
                    txtItemMarkupPercent.Text = (!Double.IsNaN(productDTO.ItemMarkupPercent) ? productDTO.ItemMarkupPercent.ToString() : "");
                    cbxAutoUpdateMarkup.Checked = productDTO.AutoUpdateMarkup;
                    pbProductImage.Tag = productDTO.ImageFileName;
                    lblImageFileName.Text = pbProductImage.Tag != null ? pbProductImage.Tag.ToString() : "";
                    if (pbProductImage.Tag != null && pbProductImage.Tag != DBNull.Value && !string.IsNullOrEmpty(pbProductImage.Tag.ToString()))
                    {
                        //pbProductImage.Image = utilities.ConvertToImage(DT.Rows[0]["picture"]);
                        SqlCommand cmdImage = utilities.getCommand();
                        cmdImage.CommandText = "exec ReadBinaryDataFromFile @FileName";

                        string fileName = pbProductImage.Tag.ToString();
                        lblImageFileName.Text = fileName;
                        cmdImage.Parameters.AddWithValue("@FileName", imageFolder + "\\" + fileName);
                        try
                        {
                            object o = cmdImage.ExecuteScalar();
                            pbProductImage.Image = utilities.ConvertToImage(o);
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                            pbProductImage.Image = null;
                        }
                    }
                    else
                        pbProductImage.Image = null;

                    notInitCase = true;
                    UISettingsForAutoMarkUp();
                }
                catch
                { notInitCase = true; }
            }
            else
            {
                EmptyAllFields();
                btnNew.PerformClick();
                txtProductName.Text = productsDTO.ProductName.ToString();
                txtDescription.Text = productsDTO.Description.ToString();
                //txtDescription.ReadOnly = true;
                //txtProductName.ReadOnly = true;
                txtSalePrice.ReadOnly = true;
                cbxActive.Checked = productsDTO.ActiveFlag;//IsActive.Equals("Y");
                                                           //cbxActive.Enabled = false;
                if (productsDTO.Price == -1)
                {
                    txtSalePrice.Text = (0).ToString(utilities.ParafaitEnv.AMOUNT_FORMAT);
                }
                else
                {
                    txtSalePrice.Text = productsDTO.Price.ToString(utilities.ParafaitEnv.AMOUNT_FORMAT);
                }
                ddlCategory.SelectedValue = productsDTO.CategoryId;
                ddlCategory.Enabled = true;
                UISettingsForAutoMarkUp();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Method to set BOM button size
        /// </summary>
        private void ResizeButtons()
        {
            log.LogMethodEntry();
            btnEdit.Size = new Size(25, 30);
            btnAdd.Size = new Size(25, 30);
            btnPrintBOM.Size = new Size(25, 30);
            btnRemove.Size = new Size(25, 30);
            btnAddTax.Size = new Size(63, 23);
            log.LogMethodExit();
        }

        private void RestrictProductCreation()
        {
            log.LogMethodEntry();
            txtDescription.ReadOnly = true;
            ddlCategory.Enabled = false;
            ddlOutboundLocation.Enabled = false;
            txtCode.ReadOnly = true;
            ddlExpiryType.Enabled = false;
            ddlIssueApproach.Enabled = false;
            log.LogMethodExit();
        }

        private void lnkStock_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            using (Semnox.Parafait.Inventory.frmInventoryStockDetails frmStockDetails = new Semnox.Parafait.Inventory.frmInventoryStockDetails(machineUserContext, productDTO.ProductId))
            {
                Semnox.Parafait.Inventory.CommonUIDisplay.setupVisuals(frmStockDetails);
                frmStockDetails.ShowDialog();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Method to set Manual Product Type
        /// </summary>
        private void SetManualProductTypeId()
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
        /// Update POS Product Information
        /// </summary>
        private void UpdatePOSProductInformation()
        {
            log.LogMethodEntry();
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

                if (isInventory && productsDTO.InventoryItemDTO.ProductId == -1)
                {
                    log.Info("isInventory && mode == I");
                    productsDTO.InventoryProductCode = txtCode.Text;
                    productsDTO.ProductTypeId = manualProductTypeId;
                    productsDTO.DisplayInPOS = "N";
                    if (productsDTO.ProductsDisplayGroupDTOList == null || productsDTO.ProductsDisplayGroupDTOList.Any() == false)
                    {
                        productsDTO.MapedDisplayGroup = manualproductDisplaygroup;
                    }
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

        /// <summary>
        /// Map products with product table
        /// </summary>
        /// <param name="productDTO"></param>
        private void CreateManualProduct(ProductDTO productDTO)
        {
            log.LogMethodEntry(productDTO);

            using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
            {
                try
                {
                    parafaitDBTrx.BeginTransaction();
                    productsDTO = new ProductsDTO();
                    productDTO.ItemType = Convert.ToInt32(cmbItemType.SelectedValue);
                    productsDTO.InventoryItemDTO = productDTO;
                    productsDTO.ProductName = productDTO.ProductName;
                    if (string.IsNullOrEmpty(productsDTO.ProductName))
                    {
                        productsDTO.ProductName = productDTO.Code;
                    }
                    productsDTO.Description = productDTO.Description;
                    productsDTO.CategoryId = productDTO.CategoryId;
                    productsDTO.InventoryProductCode = productDTO.Code;
                    if (productDTO.IsActive == true)
                        productsDTO.ActiveFlag = true;
                    else
                        productsDTO.ActiveFlag = false;
                    productsDTO.Price = Convert.ToDecimal(productDTO.SalePrice);
                    productsDTO.ProductTypeId = manualProductTypeId;
                    productsDTO.DisplayInPOS = "N";
                    productsDTO.MapedDisplayGroup = manualproductDisplaygroup;
                    Products products = new Products(machineUserContext, productsDTO);
                    products.Save(parafaitDBTrx.SQLTrx);
                    parafaitDBTrx.EndTransaction();
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    MessageBox.Show(ex.Message);
                }
            }
            log.LogMethodExit();
        }

        private void btnShowDisplayGroups_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (manualProductId > -1)
                {
                    LoadDisplayGroup(manualProductId);
                    SetDisplayGroupText();
                }
                else
                {
                    MessageBox.Show(MessageContainerList.GetMessage(machineUserContext, 1134)); // Please save the record.
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(MessageContainerList.GetMessage(machineUserContext, 1824, ex.Message));
            }
            log.LogMethodExit();
        }

        private void LoadDisplayGroup(int productId)
        {
            log.LogMethodEntry(productId);
            if (productId > -1)
            {
                frmProductsDisplayGroup frmDisplayGroup = new frmProductsDisplayGroup(utilities, productId, "PRODUCT");
                CommonUIDisplay.setupVisuals(frmDisplayGroup);
                frmDisplayGroup.StartPosition = FormStartPosition.CenterScreen;
                this.Cursor = Cursors.Default;
                frmDisplayGroup.ShowDialog();
            }
            else
            {
                throw new ValidationException(MessageContainerList.GetMessage(machineUserContext, 1134));
            }
            log.LogMethodExit();
        }


        private void SetDisplayGroupText()
        {
            log.LogMethodEntry();
            try
            {
                ClearDisplayGroupText();
                List<ProductsDisplayGroupDTO> displayGroups = new List<ProductsDisplayGroupDTO>();
                ProductsDisplayGroupList productsDisplayGroup = new ProductsDisplayGroupList(machineUserContext);
                List<KeyValuePair<ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters, string>> searchparameters = new List<KeyValuePair<ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters, string>>();
                searchparameters.Add(new KeyValuePair<ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters, string>(ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters.PRODUCT_ID, manualProductId.ToString()));
                searchparameters.Add(new KeyValuePair<ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters, string>(ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                displayGroups = productsDisplayGroup.GetAllProductsDisplayGroup(searchparameters);
                txtDisplayGroup.Text = displayGroups[0].DisplayGroupName.ToString();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void ClearDisplayGroupText()
        {
            log.LogMethodEntry();
            txtDisplayGroup.Clear();
            log.LogMethodExit();
        }

        private void HideDisplayGroupForNonInventoryCall()
        {
            log.LogMethodEntry(isInventory);
            if (isInventory == false)
            {
                lblDisplayGrp.Visible = false;
                txtDisplayGroup.Visible = false;
                btnShowDisplayGroups.Visible = false;
            }
            log.LogMethodExit();
        }

        private void UpdateUIElements()
        {
            log.LogMethodEntry();
            if (managementStudioSwitch.EnablProductModule)
            {
                btnSave.Enabled = true;
                btnDelete.Enabled = true;
                btnAddcategory.Enabled = true;
                btnAddUOM.Enabled = true;
                btnGenerateBarCode.Enabled = true;
                btnUPCBarcode.Enabled = true;
                btnAddlocation.Enabled = true;
                btnAddvendor.Enabled = true;
                btnAddTax.Enabled = true;
                btnCustom.Enabled = true;
                btnSKUSegments.Enabled = true;
                lnkStock.Enabled = true;
                lnkProductImage.Enabled = true;
                lnkBuildCostfromBOM.Enabled = true;
                //btnEdit.Enabled = true;
                btnAdd.Enabled = true;
                //btnRemove.Enabled = true;
                btnPrintBOM.Enabled = true;
                lnkPublishToSite.Enabled = true;
                btnDuplicate.Enabled = true;
            }
            else
            {
                btnSave.Enabled = false;
                btnDelete.Enabled = false;
                btnAddcategory.Enabled = false;
                btnAddUOM.Enabled = false;
                btnGenerateBarCode.Enabled = false;
                btnUPCBarcode.Enabled = false;
                btnAddlocation.Enabled = false;
                btnAddvendor.Enabled = false;
                btnAddTax.Enabled = false;
                btnCustom.Enabled = false;
                btnSKUSegments.Enabled = false;
                lnkStock.Enabled = false;
                lnkProductImage.Enabled = false;
                lnkBuildCostfromBOM.Enabled = false;
                btnEdit.Enabled = false;
                btnAdd.Enabled = false;
                btnRemove.Enabled = false;
                btnPrintBOM.Enabled = false;
                lnkPublishToSite.Enabled = false;
                btnDuplicate.Enabled = false;
            }
            log.LogMethodExit();
        }

        private void lnkrecipeDescription_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                int productId = productDTO.ProductId;
                string productGuid = productDTO.Guid;
                if (productId > -1)
                {
                    string tableObject = "PRODUCT";
                    string tableElement = "RECIPEDESCRIPTION";

                    HtmlEditorUI htmlEditorUI = new HtmlEditorUI(utilities, tableObject, productId, tableElement, productGuid, true, "PRODUCT");
                    htmlEditorUI.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                log.Error(ex.Message);
            }
            log.LogMethodExit();
        }

        private void GetUOMComboxValue(int uomId)
        {
            log.LogMethodEntry(uomId);
            UOMContainer uomcontainer = CommonFuncs.GetUOMContainer();
            List<UOMDTO> uomDTOList = uomcontainer.relatedUOMDTOList.FirstOrDefault(x => x.Key == uomId).Value;
            if (uomDTOList != null && uomDTOList.Any())
            {
                cmbInventoryUOM.DataSource = uomDTOList;
                cmbInventoryUOM.ValueMember = "UOMId";
                cmbInventoryUOM.DisplayMember = "UOM";
            }
            log.LogMethodExit();
        }

        private void PopulateItemType()
        {
            log.LogMethodEntry();
            int defaultItemTypeId = -1;
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
            cmbItemType.SelectedValue = defaultItemTypeId;
            log.LogMethodExit();
        }

        private void productCodeToolStrip_TextLeave(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ProductSearch();
            log.LogMethodExit();
        }

        private void ddlUOM_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            int productId = lblProductid.Text != string.Empty ? Convert.ToInt32(lblProductid.Text) : -1;
            if (ddlUOM.SelectedValue != null && productId == -1)
            {
                int uomId = Convert.ToInt32(ddlUOM.SelectedValue);
                if (uomId > -1)
                {
                    GetUOMComboxValue(uomId);
                    cmbInventoryUOM.SelectedValue = ddlUOM.SelectedValue;
                }
            }
            log.LogMethodExit();
        }

        private void btnDuplicate_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                duplicateFromManualProductId = -1;
                if (productDTO != null)
                {
                    if (productDTO.ProductId > -1 && productDTO.ManualProductId > -1)
                    {
                        //Reload productsDTO
                        ProductsList manualInvProductList = new ProductsList(machineUserContext);
                        duplicateFromManualProductId = productDTO.ManualProductId;
                        productsDTO = manualInvProductList.GeneateDuplicateCopy(productDTO.ManualProductId);
                        productDTO = productsDTO.InventoryItemDTO;
                        SetUIForDuplicateEntry();
                    }
                    else
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(machineUserContext, 897));
                    }
                }
                else
                {
                    throw new ValidationException(MessageContainerList.GetMessage(machineUserContext, 897));
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(MessageContainerList.GetMessage(machineUserContext, 1824, ex.Message), MessageContainerList.GetMessage(machineUserContext, "Duplicate Product"));
            }
            log.LogMethodExit();
        }

        private void SetUIForDuplicateEntry()
        {
            log.LogMethodEntry();
            lblProductid.Text = "";
            lblProductid.Tag = null;
            txtCode.Text = "";
            tvBOM.Nodes.Clear();
            txtCode.Enabled = true;
            txtBarcode.Text = "";
            txtLastpurchaseprice.Text = "";
            txtCode.ReadOnly = false;
            txtCode.Focus();
            lnkStock.Text = string.Empty;
            btnSKUSegments.Tag = -1;
            btnCustom.Tag = -1;
            log.LogMethodExit();
        }

        private void CopyFromDuplicateFromManualProductId(SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(sqlTrx, duplicateFromManualProductId);
            if (duplicateFromManualProductId > -1)
            {
                CopyProductDiscounts(sqlTrx);
                CopyProductCalendar(sqlTrx);
                CopyProductSpecialPricing(sqlTrx);
                CopyUpsellOffers(sqlTrx);
                //CopyProductsAllowedInFacility(sqlTrx);
            }
            log.LogMethodExit();
        }

        private void CopyProductDiscounts(SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(sqlTrx);
            ProductDiscountsListBL productDiscountsListBL = new ProductDiscountsListBL(machineUserContext);
            List<KeyValuePair<ProductDiscountsDTO.SearchByParameters, string>> searchProductDiscountsParams = new List<KeyValuePair<ProductDiscountsDTO.SearchByParameters, string>>();
            searchProductDiscountsParams.Add(new KeyValuePair<ProductDiscountsDTO.SearchByParameters, string>(ProductDiscountsDTO.SearchByParameters.PRODUCT_ID, duplicateFromManualProductId.ToString()));
            searchProductDiscountsParams.Add(new KeyValuePair<ProductDiscountsDTO.SearchByParameters, string>(ProductDiscountsDTO.SearchByParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            searchProductDiscountsParams.Add(new KeyValuePair<ProductDiscountsDTO.SearchByParameters, string>(ProductDiscountsDTO.SearchByParameters.IS_ACTIVE, "Y"));
            List<ProductDiscountsDTO> productDiscountsDTOList = productDiscountsListBL.GetProductDiscountsDTOList(searchProductDiscountsParams, sqlTrx);
            if (productDiscountsDTOList != null && productDiscountsDTOList.Any())
            {
                foreach (var productDiscountsDTO in productDiscountsDTOList)
                {
                    ProductDiscountsDTO duplicateProductDiscountsDTO = new ProductDiscountsDTO(-1, productDTO.ManualProductId, productDiscountsDTO.DiscountId,
                        productDiscountsDTO.ExpiryDate, string.Empty, ServerDateTime.Now, string.Empty,
                                   ServerDateTime.Now, productDiscountsDTO.InternetKey, productDiscountsDTO.ValidFor, productDiscountsDTO.ValidForDaysMonths,
                                   productDiscountsDTO.IsActive, productDTO.SiteId, -1, false, string.Empty);
                    ProductDiscountsBL productDiscountsBL = new ProductDiscountsBL(machineUserContext, duplicateProductDiscountsDTO);
                    productDiscountsBL.Save(sqlTrx);
                }
            }
            log.LogMethodExit();
        }

        private void CopyProductCalendar(SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(sqlTrx);
            ProductsCalenderList listClassBL = new ProductsCalenderList(machineUserContext);
            List<KeyValuePair<ProductsCalenderDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<ProductsCalenderDTO.SearchByParameters, string>>();
            searchParams.Add(new KeyValuePair<ProductsCalenderDTO.SearchByParameters, string>(ProductsCalenderDTO.SearchByParameters.PRODUCT_ID, duplicateFromManualProductId.ToString()));
            searchParams.Add(new KeyValuePair<ProductsCalenderDTO.SearchByParameters, string>(ProductsCalenderDTO.SearchByParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            //searchParams.Add(new KeyValuePair<ProductsCalenderDTO.SearchByParameters, string>(ProductsCalenderDTO.SearchByParameters.IS_ACTIVE, "Y"));
            List<ProductsCalenderDTO> dtoList = listClassBL.GetAllProductCalenderList(searchParams, sqlTrx);
            if (dtoList != null && dtoList.Any())
            {
                foreach (ProductsCalenderDTO dtoItem in dtoList)
                {
                    ProductsCalenderDTO duplicateDTO = new ProductsCalenderDTO(-1, productDTO.ManualProductId, dtoItem.Day, dtoItem.Date, dtoItem.FromTime,
                                    dtoItem.ToTime, dtoItem.ShowHide, productDTO.SiteId, string.Empty, false, -1);
                    ProductsCalender blClass = new ProductsCalender(machineUserContext, duplicateDTO);
                    blClass.Save(sqlTrx);
                }
            }
            log.LogMethodExit();
        }

        private void CopyProductSpecialPricing(SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(sqlTrx);
            ProductsSpecialPricingListBL listClassBL = new ProductsSpecialPricingListBL(machineUserContext);
            List<KeyValuePair<ProductsSpecialPricingDTO.SearchByProductsSpecialPricingParameters, string>> searchParams = new List<KeyValuePair<ProductsSpecialPricingDTO.SearchByProductsSpecialPricingParameters, string>>();
            searchParams.Add(new KeyValuePair<ProductsSpecialPricingDTO.SearchByProductsSpecialPricingParameters, string>(ProductsSpecialPricingDTO.SearchByProductsSpecialPricingParameters.PRODUCT_ID, duplicateFromManualProductId.ToString()));
            searchParams.Add(new KeyValuePair<ProductsSpecialPricingDTO.SearchByProductsSpecialPricingParameters, string>(ProductsSpecialPricingDTO.SearchByProductsSpecialPricingParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            searchParams.Add(new KeyValuePair<ProductsSpecialPricingDTO.SearchByProductsSpecialPricingParameters, string>(ProductsSpecialPricingDTO.SearchByProductsSpecialPricingParameters.ACTIVE_FLAG, "Y"));
            List<ProductsSpecialPricingDTO> dtoList = listClassBL.GetAllProductsSpecialPricing(searchParams, sqlTrx);
            if (dtoList != null && dtoList.Any())
            {
                foreach (ProductsSpecialPricingDTO dtoItem in dtoList)
                {
                    ProductsSpecialPricingDTO duplicateDTO = new ProductsSpecialPricingDTO(-1, productDTO.ManualProductId, dtoItem.PricingId, dtoItem.Price, dtoItem.ActiveFlag,
                                    string.Empty, productDTO.SiteId, false, -1, string.Empty, ServerDateTime.Now, string.Empty, ServerDateTime.Now);
                    ProductsSpecialPricingBL blClass = new ProductsSpecialPricingBL(machineUserContext, duplicateDTO);
                    blClass.Save(sqlTrx);
                }
            }
            log.LogMethodExit();
        }
        private void CopyUpsellOffers(SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(sqlTrx);
            UpsellOffersList listClassBL = new UpsellOffersList(machineUserContext);
            List<KeyValuePair<UpsellOffersDTO.SearchByUpsellOffersParameters, string>> searchParams = new List<KeyValuePair<UpsellOffersDTO.SearchByUpsellOffersParameters, string>>();
            searchParams.Add(new KeyValuePair<UpsellOffersDTO.SearchByUpsellOffersParameters, string>(UpsellOffersDTO.SearchByUpsellOffersParameters.PRODUCT_ID, duplicateFromManualProductId.ToString()));
            searchParams.Add(new KeyValuePair<UpsellOffersDTO.SearchByUpsellOffersParameters, string>(UpsellOffersDTO.SearchByUpsellOffersParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            searchParams.Add(new KeyValuePair<UpsellOffersDTO.SearchByUpsellOffersParameters, string>(UpsellOffersDTO.SearchByUpsellOffersParameters.ACTIVE_FLAG, "Y"));
            List<UpsellOffersDTO> dtoList = listClassBL.GetAllUpsellOffers(searchParams, sqlTrx);
            if (dtoList != null && dtoList.Any())
            {
                foreach (UpsellOffersDTO dtoItem in dtoList)
                {
                    UpsellOffersDTO duplicateDTO = new UpsellOffersDTO(-1, productDTO.ManualProductId, dtoItem.OfferProductId, dtoItem.OfferMessage, dtoItem.EffectiveDate,
                        string.Empty, dtoItem.IsActive, productDTO.SiteId, false, string.Empty, string.Empty, ServerDateTime.Now, -1, dtoItem.SaleGroupId);
                    UpsellOffer blClass = new UpsellOffer(machineUserContext, duplicateDTO);
                    blClass.Save(sqlTrx);
                }
            }
            log.LogMethodExit();
        }
        //private void CopyProductsAllowedInFacility(SqlTransaction sqlTrx)
        //{
        //    log.LogMethodEntry(sqlTrx);
        //    ProductsAllowedInFacilityMapListBL listClassBL = new ProductsAllowedInFacilityMapListBL(machineUserContext);
        //    List<KeyValuePair<ProductsAllowedInFacilityMapDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<ProductsAllowedInFacilityMapDTO.SearchByParameters, string>>();
        //    searchParams.Add(new KeyValuePair<ProductsAllowedInFacilityMapDTO.SearchByParameters, string>(ProductsAllowedInFacilityMapDTO.SearchByParameters.PRODUCTS_ID, duplicateFromManualProductId.ToString()));
        //    searchParams.Add(new KeyValuePair<ProductsAllowedInFacilityMapDTO.SearchByParameters, string>(ProductsAllowedInFacilityMapDTO.SearchByParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
        //    searchParams.Add(new KeyValuePair<ProductsAllowedInFacilityMapDTO.SearchByParameters, string>(ProductsAllowedInFacilityMapDTO.SearchByParameters.IS_ACTIVE, "Y"));
        //    List<ProductsAllowedInFacilityMapDTO> dtoList = listClassBL.GetProductsAllowedInFacilityMapDTOList(searchParams,false, sqlTrx);
        //    if (dtoList != null && dtoList.Any())
        //    {
        //        List<ProductsAllowedInFacilityMapDTO> duplicateDTOList = new List<ProductsAllowedInFacilityMapDTO>();
        //        foreach (ProductsAllowedInFacilityMapDTO dtoItem in dtoList)
        //        {
        //            ProductsAllowedInFacilityMapDTO duplicateDTO = new ProductsAllowedInFacilityMapDTO(-1,  dtoItem.FacilityMapId,
        //                        dtoItem.FacilityMapName, productDTO.ManualProductId, dtoItem.ProductType, dtoItem.DefaultRentalProduct, dtoItem.IsActive, string.Empty, string.Empty,
        //                        DateTime.Now, string.Empty, DateTime.Now, productDTO.SiteId, false, -1);
        //            duplicateDTOList.Add(duplicateDTO);
        //        }
        //        ProductsAllowedInFacilityMapListBL blClass = new ProductsAllowedInFacilityMapListBL(machineUserContext, duplicateDTOList);
        //        blClass.Save(sqlTrx);
        //    }
        //    log.LogMethodExit();
        //}

        private void ResetCustomerDataSet(string modeValue)
        {
            log.LogMethodEntry(modeValue);
            if (modeValue == "I")
            {
                if (productsDTO != null && productsDTO.CustomDataSetDTO != null)
                {
                    productsDTO.CustomDataSetId = -1;
                    productsDTO.CustomDataSetDTO.CustomDataSetId = -1;
                    if (productsDTO.CustomDataSetDTO.CustomDataDTOList != null && productsDTO.CustomDataSetDTO.CustomDataDTOList.Any())
                    {
                        for (int i = 0; i < productsDTO.CustomDataSetDTO.CustomDataDTOList.Count; i++)
                        {
                            productsDTO.CustomDataSetDTO.CustomDataDTOList[i].CustomDataId = -1;
                            productsDTO.CustomDataSetDTO.CustomDataDTOList[i].CustomDataSetId = -1;
                        }
                    }
                }
                if (productDTO != null && productDTO.CustomDataSetDTO != null)
                {
                    productDTO.CustomDataSetId = -1;
                    productDTO.CustomDataSetDTO.CustomDataSetId = -1;
                    if (productDTO.CustomDataSetDTO.CustomDataDTOList != null && productDTO.CustomDataSetDTO.CustomDataDTOList.Any())
                    {
                        for (int i = 0; i < productDTO.CustomDataSetDTO.CustomDataDTOList.Count; i++)
                        {
                            productDTO.CustomDataSetDTO.CustomDataDTOList[i].CustomDataId = -1;
                            productDTO.CustomDataSetDTO.CustomDataDTOList[i].CustomDataSetId = -1;
                        }
                    }
                }
            }
            log.LogMethodExit();
        }

        private void ddlPreferredvendor_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                this.BeginInvoke(new Action(() => { ddlPreferredvendor.Select(0, 0); }));
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
    }
}

