/********************************************************************************************
 * Project Name - Requisition  UI
 * Description  -UI of Requisition 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.2       20-Nov-2019   Deeksha         Inventory Next Rel Enhancement changes
 *2.70.3       20-Mar-2020   Girish Kundar   Modified : Validation for creating requisition without any products
 *2.100.0      07-Aug-2020   Deeksha         Modified : Added UOM drop down field to change related UOM. Removed unused methods
 *2.110.00    16-12-2020    Abhishek         Modified : child save()
 *2.110.0      20-Feb-2020   Dakshakh Raj    Modified: Get Sequence method changes
 ********************************************************************************************/
using System;
using System.Data;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using Semnox.Core.Utilities;
using System.Data.SqlClient;
using Semnox.Parafait.Product;
using System.Drawing.Printing;
using Semnox.Parafait.Category;
using System.Collections.Generic;
using Semnox.Core.GenericUtilities;

namespace Semnox.Parafait.Inventory
{
    /// <summary>
    /// frmRequisition
    /// </summary>
    public partial class frmRequisition : Form
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int dgvselectedindex = -1;
        private bool fireCellValueChange = true;
        private PrintDocument MyPrintDocument;
        private DataGridViewPrinter MyDataGridViewPrinter;
        private DataGridView printdgv = new DataGridView();
        private BindingSource inventoryProductListBS;
        private BindingSource inventoryRequisitionListBS;
        private Utilities utilities = new Utilities();
        private string reqSelectedStatus = string.Empty;
        private DateTime requiredByDateTime;
        private List<ProductDTO> inventoryProductListOnDisplay;
        private List<CategoryDTO> categoryListOnDisplay;
        private ExecutionContext executionContext = null;
        private bool fireTemplateSelectionChange = true;
        private bool fireSelectionChanged = true;


        /// <summary>
        /// Constructor 
        /// </summary>
        public frmRequisition(Utilities _Utilities)
        {
            log.LogMethodEntry(_Utilities);
            InitializeComponent();
            utilities = _Utilities;
            executionContext = _Utilities.ExecutionContext;
            utilities.setupDataGridProperties(ref dgvProductList);
            utilities.setupDataGridProperties(ref dgvSearchRequisitions);
            this.dtpRequiredDate.Value = DateTime.Today;
            CommonFuncs.Utilities = utilities;
            CommonFuncs.ParafaitEnv = utilities.ParafaitEnv;
            CommonFuncs.LoadToSite(cmbTosite, false, true);
            log.LogMethodExit();
        }
        /// <summary>
        /// form load
        /// </summary>
        private void frmRequisition_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (utilities.ParafaitEnv.IsCorporate)
            {
                executionContext.SetSiteId(utilities.ParafaitEnv.SiteId);
            }
            else
            {
                executionContext.SetSiteId(-1);
            }
            executionContext.SetUserId(utilities.ParafaitEnv.LoginID);

            LoadTemplates();
            LoadRequisitionTypes();
            LoadProductCategories();
            PopulateRequisitionGrid();

            if (cmbRequisitionStatus.SelectedIndex == -1)
                cmbRequisitionStatus.SelectedIndex = 0;

            cmbReqType.Focus();
            dgvProductList.Columns["Code"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgvProductList.Columns["Code"].Width = 100;
            dgvProductList.Columns["Description"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dgvProductList.Columns["Description"].Width = 275;
            dgvProductList.Columns["cmbUOM"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgvProductList.Columns["cmbUOM"].Width = 80;
            dgvProductList.Columns["StockAtLocation"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgvProductList.Columns["StockAtLocation"].Width = 85;
            dgvProductList.Columns["StockAtLocation"].DefaultCellStyle = utilities.gridViewNumericCellStyle();
            dgvProductList.Columns["StockAtLocation"].DefaultCellStyle.Format = utilities.ParafaitEnv.INVENTORY_QUANTITY_FORMAT;
            dgvProductList.Columns["RequestedQnty"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgvProductList.Columns["RequestedQnty"].DefaultCellStyle = utilities.gridViewNumericCellStyle();
            dgvProductList.Columns["RequestedQnty"].DefaultCellStyle.Format = utilities.ParafaitEnv.INVENTORY_QUANTITY_FORMAT;
            dgvProductList.Columns["RequestedQnty"].Width = 85;
            dgvProductList.Columns["RequiredByDate"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgvProductList.Columns["RequiredByDate"].DefaultCellStyle = utilities.gridViewDateCellStyle();
            dgvProductList.Columns["RequiredByDate"].Width = 120;
            dgvProductList.Columns["Category"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgvProductList.Columns["Category"].Width = 100;
            if (!string.IsNullOrEmpty(txtRequisitionId.Text))
                lnkLblAddRemarks.Enabled = true;
            else
                lnkLblAddRemarks.Enabled = false;
            utilities.setLanguage(this);
            ProductContainer productContainer = new ProductContainer(executionContext);
            log.LogMethodExit();
        }

        /// <summary>
        /// button close form clicked
        /// </summary>
        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            Close();
            log.LogMethodExit();
        }

        /// <summary>
        /// display template details
        /// </summary>
        private void DisplayTemplateDetails(int templateId)
        {
            log.LogMethodEntry(templateId);
            try
            {
                fireTemplateSelectionChange = false;
                RequisitionTemplateList requisitionTemplateDetails = new RequisitionTemplateList(executionContext);
                RequisitionTemplatesDTO requisitionsTemplateHeaderDetails = requisitionTemplateDetails.GetTemplate(templateId);
                if (requisitionsTemplateHeaderDetails != null)
                {
                    if (DBNull.Value.Equals(requisitionsTemplateHeaderDetails.TemplateId) || requisitionsTemplateHeaderDetails.TemplateId == -1)
                    {
                        cmbTemplate.SelectedIndex = -1;
                    }
                    else
                    {
                        cmbTemplate.SelectedItem = (RequisitionTemplatesDTO)GetTemplateComboBoxDTO(cmbTemplate, Convert.ToInt32(requisitionsTemplateHeaderDetails.TemplateId));
                    }
                    fireTemplateSelectionChange = true;
                    txtEstimatedValue.Text = requisitionsTemplateHeaderDetails.EstimatedValue.ToString();
                    cmbReqType.SelectedItem = (InventoryDocumentTypeDTO)GetComboBoxItemIndex(cmbReqType, Convert.ToInt32(requisitionsTemplateHeaderDetails.RequisitionType));
                    cmbRequisitionRequestingDept.SelectedItem = (LocationDTO)GetLocationComboBoxItemIndex(cmbRequisitionRequestingDept, Convert.ToInt32(requisitionsTemplateHeaderDetails.RequestingDept));
                    cmbFromDept.SelectedItem = (LocationDTO)GetLocationComboBoxItemIndex(cmbFromDept, Convert.ToInt32(requisitionsTemplateHeaderDetails.FromDepartment));
                    cmbToDept.SelectedItem = (LocationDTO)GetLocationComboBoxItemIndex(cmbToDept, Convert.ToInt32(requisitionsTemplateHeaderDetails.ToDepartment));
                    cmbRequisitionStatus.SelectedItem = Convert.ToString(requisitionsTemplateHeaderDetails.Status);
                    reqSelectedStatus = Convert.ToString(requisitionsTemplateHeaderDetails.Status);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// display template lines for selected template from combo box
        /// </summary>
        private void DisplayTemplateLines(int templateId)
        {
            log.LogMethodEntry(templateId);
            try
            {
                RequisitionTemplateLinesList templateProductList = new RequisitionTemplateLinesList(executionContext);
                List<KeyValuePair<RequisitionTemplateLinesDTO.SearchByTemplateLinesParameters, string>> templateProductsSearchParams = new List<KeyValuePair<RequisitionTemplateLinesDTO.SearchByTemplateLinesParameters, string>>();
                templateProductsSearchParams.Add(new KeyValuePair<RequisitionTemplateLinesDTO.SearchByTemplateLinesParameters, string>(RequisitionTemplateLinesDTO.SearchByTemplateLinesParameters.ACTIVE_FLAG, "1"));
                templateProductsSearchParams.Add(new KeyValuePair<RequisitionTemplateLinesDTO.SearchByTemplateLinesParameters, string>(RequisitionTemplateLinesDTO.SearchByTemplateLinesParameters.TEMPLATE_ID, templateId.ToString()));
                int toDeptId = -1;
                if (cmbToDept.SelectedIndex > 0)
                {
                    LocationDTO toLocation = (LocationDTO)cmbToDept.SelectedItem;
                    if (toLocation != null)
                    {
                        toDeptId = Convert.ToInt32(toLocation.LocationId);
                    }
                }
                List<RequisitionTemplateLinesDTO> inventoryProductListOnDisplay = templateProductList.GetRequisitionsTemplateListForLoad(templateId, true, toDeptId);
                if (inventoryProductListOnDisplay != null && inventoryProductListOnDisplay.Count > 0)
                {
                    RequisitionLinesDTO lines;
                    BindingSource source = (BindingSource)dgvProductList.DataSource;
                    for (int i = 0; i < inventoryProductListOnDisplay.Count; i++)
                    {
                        lines = new RequisitionLinesDTO(-1, -1, string.Empty, inventoryProductListOnDisplay[i].ProductId, inventoryProductListOnDisplay[i].Code,
                                                        inventoryProductListOnDisplay[i].Description, 0, 0, dtpRequiredDate.Value.Date, inventoryProductListOnDisplay[i].IsActive,string.Empty,string.Empty,
                                                        executionContext.GetSiteId() , inventoryProductListOnDisplay[i].UOM, inventoryProductListOnDisplay[i].StockAtLocation ,
                                                        inventoryProductListOnDisplay[i].Price , inventoryProductListOnDisplay[i].CategoryName, inventoryProductListOnDisplay[i].UOMId,0);
                        if (dtpRequiredDate.Value.Date == DateTime.MinValue)
                        {
                            RequiredByDate.DefaultCellStyle.Format = ("");
                        }
                        source.Add(lines);
                    }
                    dgvProductList.DataSource = source;
                    LoadUOMComboBox();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// load all the templates into combo box
        /// </summary>
        private void LoadTemplates()
        {
            log.LogMethodEntry();
            try
            {
                RequisitionTemplateList requisitionTemplates = new RequisitionTemplateList(executionContext);
                List<KeyValuePair<RequisitionTemplatesDTO.SearchByReqTemplatesTypeParameters, string>> requisitionTemplatesSearchParams = new List<KeyValuePair<RequisitionTemplatesDTO.SearchByReqTemplatesTypeParameters, string>>();
                requisitionTemplatesSearchParams.Add(new KeyValuePair<RequisitionTemplatesDTO.SearchByReqTemplatesTypeParameters, string>(RequisitionTemplatesDTO.SearchByReqTemplatesTypeParameters.SITEID, executionContext.GetSiteId().ToString()));
                requisitionTemplatesSearchParams.Add(new KeyValuePair<RequisitionTemplatesDTO.SearchByReqTemplatesTypeParameters, string>(RequisitionTemplatesDTO.SearchByReqTemplatesTypeParameters.ACTIVE_FLAG, "1"));

                requisitionTemplatesSearchParams.Add(new KeyValuePair<RequisitionTemplatesDTO.SearchByReqTemplatesTypeParameters, string>(RequisitionTemplatesDTO.SearchByReqTemplatesTypeParameters.STATUS, "Open"));

                List<RequisitionTemplatesDTO> inventoryLocationListOnDisplay = requisitionTemplates.GetAllTemplates(requisitionTemplatesSearchParams);

                BindingSource requisitionTemplatesBS = new BindingSource();
                if (inventoryLocationListOnDisplay != null)
                {
                    inventoryLocationListOnDisplay.Insert(0, new RequisitionTemplatesDTO());
                    SortableBindingList<RequisitionTemplatesDTO> requisitionTemplatesDTOSortList = new SortableBindingList<RequisitionTemplatesDTO>(inventoryLocationListOnDisplay);
                    requisitionTemplatesBS.DataSource = requisitionTemplatesDTOSortList;
                }
                else
                {
                    requisitionTemplatesBS.DataSource = new SortableBindingList<RequisitionTemplatesDTO>();
                }

                cmbTemplate.DataSource = requisitionTemplatesBS;
                cmbTemplate.ValueMember = "TemplateId";
                cmbTemplate.DisplayMember = "TemplateName";
            }
            catch (Exception ex)
            {
                log.Error("Error in LoadTemplates() method.", ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// to get item index 
        /// </summary>
        private InventoryDocumentTypeDTO GetComboBoxItemIndex(ComboBox combobox, int selectedId)
        {
            log.LogMethodEntry(combobox, selectedId);
            InventoryDocumentTypeDTO inventoryObj = null;
            try
            {
                BindingSource bindingSource = (BindingSource)combobox.DataSource;
                var reqTypeList = (SortableBindingList<InventoryDocumentTypeDTO>)bindingSource.DataSource;
                if (reqTypeList.Count > 0)
                {
                    foreach (InventoryDocumentTypeDTO item in reqTypeList)
                    {
                        if (item.DocumentTypeId == selectedId)
                        {
                            inventoryObj = new InventoryDocumentTypeDTO();
                            inventoryObj = item;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit(inventoryObj);
            return inventoryObj;
        }

        /// <summary>
        /// to get item index 
        /// </summary>
        private LocationDTO GetLocationComboBoxItemIndex(ComboBox combobox, int selectedId)
        {
            log.LogMethodEntry(combobox, selectedId);
            LocationDTO locationObj = null;
            try
            {
                if (combobox.DataSource != null)
                {
                    BindingSource bindingSource = (BindingSource)combobox.DataSource;
                    var reqTypeList = (SortableBindingList<LocationDTO>)bindingSource.DataSource;
                    if (reqTypeList.Count > 0)
                    {
                        foreach (LocationDTO item in reqTypeList)
                        {
                            if (item.LocationId == selectedId)
                            {
                                locationObj = new LocationDTO();
                                locationObj = item;
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit(locationObj);
            return locationObj;
        }

        /// <summary>
        /// to get item index 
        /// </summary>
        private RequisitionTemplatesDTO GetTemplateComboBoxDTO(ComboBox combobox, int selectedId)
        {
            log.LogMethodEntry(combobox, selectedId);
            RequisitionTemplatesDTO reqDTO = null;
            try
            {
                if (combobox.DataSource != null)
                {
                    BindingSource bindingSource = (BindingSource)combobox.DataSource;
                    var requisitionList = (SortableBindingList<RequisitionTemplatesDTO>)bindingSource.DataSource;
                    if (requisitionList.Count > 0)
                    {
                        foreach (RequisitionTemplatesDTO item in requisitionList)
                        {
                            if (item.TemplateId == selectedId)
                            {
                                reqDTO = new RequisitionTemplatesDTO();
                                reqDTO = item;
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit(reqDTO);
            return reqDTO;
        }

        /// <summary>
        /// Populates product Category.
        /// </summary>
        private void LoadProductCategories()
        {
            log.LogMethodEntry();
            try
            {
                CategoryList productCategoryList = new CategoryList(executionContext);
                List<KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>> categoryDTOSearchParams = new List<KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>>();
                categoryDTOSearchParams.Add(new KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>(CategoryDTO.SearchByCategoryParameters.IS_ACTIVE, "Y"));
                categoryListOnDisplay = productCategoryList.GetAllCategory(categoryDTOSearchParams);
                if (categoryListOnDisplay == null)
                {
                    categoryListOnDisplay = new List<CategoryDTO>();
                }
                categoryListOnDisplay = categoryListOnDisplay.OrderBy(c => c.Name).ToList();

                categoryListOnDisplay.Insert(0, new CategoryDTO());
                categoryListOnDisplay[0].Name = "<ALL>";
                cmbCategory.DataSource = categoryListOnDisplay;
                cmbCategory.ValueMember = "CategoryId";
                cmbCategory.DisplayMember = "Name";
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Loading Requisitions type
        /// </summary>
        private void LoadRequisitionTypes()
        {
            log.LogMethodEntry();
            try
            {
                InventoryDocumentTypeList inventoryDocumentTypeList = new InventoryDocumentTypeList(executionContext);
                List<KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>> inventoryDocumentTypeSearchParams = new List<KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>>();
                inventoryDocumentTypeSearchParams.Add(new KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>(InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters.ACTIVE_FLAG, "1"));
                inventoryDocumentTypeSearchParams.Add(new KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>(InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                inventoryDocumentTypeSearchParams.Add(new KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>(InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters.APPLICABILITY, "RQ"));
                List<InventoryDocumentTypeDTO> inventoryDocumentTypeListOnDisplay = inventoryDocumentTypeList.GetAllInventoryDocumentTypes(inventoryDocumentTypeSearchParams);
                BindingSource bindingSourceReqTypes = new BindingSource();
                BindingSource bindingSourceSearchReqTypes = new BindingSource();
                if (inventoryDocumentTypeListOnDisplay != null)
                {
                    inventoryDocumentTypeListOnDisplay.Insert(0, new InventoryDocumentTypeDTO());
                    SortableBindingList<InventoryDocumentTypeDTO> inventoryDocumentTypeDTOSortList = new SortableBindingList<InventoryDocumentTypeDTO>(inventoryDocumentTypeListOnDisplay);
                    inventoryDocumentTypeListOnDisplay.Insert(0, new InventoryDocumentTypeDTO());
                    bindingSourceReqTypes.DataSource = inventoryDocumentTypeDTOSortList;
                    bindingSourceSearchReqTypes.DataSource = inventoryDocumentTypeDTOSortList;
                }
                else
                {
                    bindingSourceReqTypes.DataSource = new SortableBindingList<InventoryDocumentTypeDTO>();
                    bindingSourceSearchReqTypes.DataSource = new SortableBindingList<InventoryDocumentTypeDTO>();
                }
                cmbReqType.DataSource = bindingSourceReqTypes;
                cmbReqType.ValueMember = "DocumentTypeId";
                cmbReqType.DisplayMember = "Name";

                cmbSearchReqType.DataSource = bindingSourceSearchReqTypes;
                cmbSearchReqType.ValueMember = "DocumentTypeId";
                cmbSearchReqType.DisplayMember = "Name";
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// load existing requisitions into grid
        /// </summary>
        private void PopulateRequisitionGrid()
        {
            log.LogMethodEntry();
            try
            {     
                RequisitionList requisitionList = new RequisitionList(executionContext);
                List<KeyValuePair<RequisitionDTO.SearchByRequisitionParameters, string>> inventoryRequisitionsSearchParams = new List<KeyValuePair<RequisitionDTO.SearchByRequisitionParameters, string>>();
                inventoryRequisitionsSearchParams.Add(new KeyValuePair<RequisitionDTO.SearchByRequisitionParameters, string>(RequisitionDTO.SearchByRequisitionParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (chkSearchIsActive.Checked)
                    inventoryRequisitionsSearchParams.Add(new KeyValuePair<RequisitionDTO.SearchByRequisitionParameters, string>(RequisitionDTO.SearchByRequisitionParameters.ACTIVE_FLAG, "1"));
                else
                    inventoryRequisitionsSearchParams.Add(new KeyValuePair<RequisitionDTO.SearchByRequisitionParameters, string>(RequisitionDTO.SearchByRequisitionParameters.ACTIVE_FLAG, "0"));


                if (!string.IsNullOrEmpty(txtReqNoSearch.Text))
                    inventoryRequisitionsSearchParams.Add(new KeyValuePair<RequisitionDTO.SearchByRequisitionParameters, string>(RequisitionDTO.SearchByRequisitionParameters.REQUISITION_NUMBER, txtReqNoSearch.Text.ToString()));


                if (cmbSearchStatus.SelectedItem != null)
                {
                    if (!string.IsNullOrEmpty(cmbSearchStatus.SelectedItem.ToString()) && cmbSearchStatus.SelectedItem.ToString() != "-1")
                        inventoryRequisitionsSearchParams.Add(new KeyValuePair<RequisitionDTO.SearchByRequisitionParameters, string>(RequisitionDTO.SearchByRequisitionParameters.STATUS, cmbSearchStatus.SelectedItem.ToString()));
                }

                if (cmbSearchReqType.SelectedValue != null)
                {
                    if (!string.IsNullOrEmpty(cmbSearchReqType.SelectedValue.ToString()) && cmbSearchReqType.SelectedValue.ToString() != "-1")
                        inventoryRequisitionsSearchParams.Add(new KeyValuePair<RequisitionDTO.SearchByRequisitionParameters, string>(RequisitionDTO.SearchByRequisitionParameters.REQUISITION_TYPE, cmbSearchReqType.SelectedValue.ToString()));
                }

                List<RequisitionDTO> inventoryProductListOnDisplay = requisitionList.GetAllRequisitions(inventoryRequisitionsSearchParams);

                inventoryRequisitionListBS = new BindingSource();
                if (inventoryProductListOnDisplay != null)
                {
                    SortableBindingList<RequisitionDTO> inventoryProductDTOSortList = new SortableBindingList<RequisitionDTO>(inventoryProductListOnDisplay);
                    inventoryRequisitionListBS.DataSource = inventoryProductDTOSortList;
                }
                else
                {
                    inventoryRequisitionListBS.DataSource = new SortableBindingList<RequisitionDTO>();
                    ClearRequisitionHeaderAndLines();
                    ClearSearchFields();
                }
                inventoryRequisitionListBS.AddingNew += dgvRequisitionsList_BindingSourceAddNew;

                dgvSearchRequisitions.DataSource = inventoryRequisitionListBS;

                // Modified on 25-Oct-2015 for clearing retained date variable when rows count are zero. 
                if (dgvSearchRequisitions.Rows.Count == 0)
                {
                    requiredByDateTime = DateTime.MinValue;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Display selected requisition details
        /// </summary>
        private void DisplayRequisitionDetails(int requisitionId)
        {
            log.LogMethodEntry(requisitionId);
            try
            {
                RequisitionList requisitionDetails = new RequisitionList(executionContext);
                RequisitionDTO requisitionsHeaderDetails = requisitionDetails.GetRequisition(requisitionId);
                if (requisitionsHeaderDetails != null)
                {
                    if (DBNull.Value.Equals(requisitionsHeaderDetails.TemplateId) || requisitionsHeaderDetails.TemplateId == -1)
                    {
                        cmbTemplate.SelectedIndex = -1;
                    }
                    else
                    {
                        fireSelectionChanged = false;
                        cmbTemplate.SelectedItem = (RequisitionTemplatesDTO)GetTemplateComboBoxDTO(cmbTemplate, Convert.ToInt32(requisitionsHeaderDetails.TemplateId));
                        fireSelectionChanged = true;
                    }

                    txtRequisitionId.Text = requisitionsHeaderDetails.RequisitionId.ToString();
                    txtReqNumber.Text = requisitionsHeaderDetails.RequisitionNo.ToString();
                    txtEstimatedValue.Text = requisitionsHeaderDetails.EstimatedValue.ToString();
                    chkIsActive.Checked = Convert.ToBoolean(requisitionsHeaderDetails.IsActive);
                    cmbReqType.SelectedItem = (InventoryDocumentTypeDTO)GetComboBoxItemIndex(cmbReqType, Convert.ToInt32(requisitionsHeaderDetails.RequisitionType));
                    cmbRequisitionRequestingDept.SelectedItem = (LocationDTO)GetLocationComboBoxItemIndex(cmbRequisitionRequestingDept, Convert.ToInt32(requisitionsHeaderDetails.RequestingDept));
                    cmbFromDept.SelectedItem = (LocationDTO)GetLocationComboBoxItemIndex(cmbFromDept, Convert.ToInt32(requisitionsHeaderDetails.FromDepartment));
                    cmbToDept.SelectedItem = (LocationDTO)GetLocationComboBoxItemIndex(cmbToDept, Convert.ToInt32(requisitionsHeaderDetails.ToDepartment));
                    if (requisitionsHeaderDetails.RequiredByDate != null && requisitionsHeaderDetails.RequiredByDate != DateTime.MinValue)
                    {
                        dtpRequiredDate.CustomFormat = utilities.getDateFormat();
                        dtpRequiredDate.Value = Convert.ToDateTime(requisitionsHeaderDetails.RequiredByDate).Date;
                    }
                    else
                    {
                        dtpRequiredDate.CustomFormat = " ";
                    }
                    cmbRequisitionStatus.SelectedItem = Convert.ToString(requisitionsHeaderDetails.Status);
                    reqSelectedStatus = Convert.ToString(requisitionsHeaderDetails.Status);
                    cmbTosite.SelectedValue = requisitionsHeaderDetails.ToSiteId;
                    if (requisitionsHeaderDetails.ToSiteId == utilities.ParafaitEnv.SiteId)
                    {
                        btnSave.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// populate products into grid based on search criteria
        /// </summary>
        private void PopulateGrid()
        {
            log.LogMethodEntry();
            try
            {
                ProductList inventoryProductList = new ProductList();
                List<KeyValuePair<ProductDTO.SearchByProductParameters, string>> inventoryProductSearchParams = new List<KeyValuePair<ProductDTO.SearchByProductParameters, string>>();

                inventoryProductSearchParams.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.IS_ACTIVE, "Y"));
                inventoryProductSearchParams.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.SITE_ID, executionContext.GetSiteId().ToString()));

                if (!string.IsNullOrEmpty(txtProdBarcode.Text))
                {
                    RequisitionList requisitionList = new RequisitionList(executionContext);
                    int ProductId = requisitionList.GetProductId(txtProdBarcode.Text.ToString().Trim());
                    inventoryProductSearchParams.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.PRODUCT_ID, ProductId.ToString()));
                }

                if (cmbReqType.SelectedIndex > 0)
                {
                    InventoryDocumentTypeDTO documentTypeDTO = (InventoryDocumentTypeDTO)cmbReqType.SelectedItem;
                    if (documentTypeDTO != null)
                    {
                        if (documentTypeDTO.Code == "MLRQ")
                        {
                            inventoryProductSearchParams.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.MARKET_LIST_ITEM, "1"));
                        }
                        else
                        {
                            inventoryProductSearchParams.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.MARKET_LIST_ITEM, "0"));
                        }
                    }
                }
                if (!string.IsNullOrEmpty(txtProdcode.Text))
                    inventoryProductSearchParams.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.CODE_EXACT_MATCH, txtProdcode.Text.ToString()));

                if (cmbProdCategory.SelectedValue != null)
                {
                    if (!string.IsNullOrEmpty(cmbProdCategory.SelectedValue.ToString()) && cmbProdCategory.SelectedValue.ToString() != "-1")
                        inventoryProductSearchParams.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.CATEGORY, cmbProdCategory.SelectedValue.ToString()));
                }

                List<ProductDTO> inventoryProductListOnDisplay;
                //added for advanced search
                if (frmAdvancedSearch != null && !string.IsNullOrEmpty(frmAdvancedSearch.searchString) && frmAdvancedSearch.searchString != "-1")
                {
                    inventoryProductSearchParams.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.SEGMENT_CATEGORY_ID, frmAdvancedSearch.searchString.ToString()));
                    inventoryProductListOnDisplay = inventoryProductList.GetAdancedAllProducts(inventoryProductSearchParams);
                    frmAdvancedSearch = null;
                }
                else
                {
                    inventoryProductListOnDisplay = inventoryProductList.GetAllProducts(inventoryProductSearchParams);
                }
                if (dgvProductList != null && dgvProductList.Rows.Count > 0)
                {
                    if (inventoryProductListOnDisplay != null)
                    {
                        RequisitionLinesDTO lines;
                        double currentstock = 0;
                        BindingSource source = (BindingSource)dgvProductList.DataSource;
                        for (int i = 0; i < inventoryProductListOnDisplay.Count; i++)
                        {
                            currentstock = GetCurrentStock(inventoryProductListOnDisplay[i].ProductId);
                            lines = new RequisitionLinesDTO(-1, -1, string.Empty, inventoryProductListOnDisplay[i].ProductId, inventoryProductListOnDisplay[i].Code,
                                                       inventoryProductListOnDisplay[i].Description, 0, 0, dtpRequiredDate.Value.Date, inventoryProductListOnDisplay[i].IsActive, string.Empty, string.Empty,
                                                       executionContext.GetSiteId(), inventoryProductListOnDisplay[i].UOMValue, currentstock,
                                                       inventoryProductListOnDisplay[i].Cost, string.Empty, inventoryProductListOnDisplay[i].UomId,0);
                            if (dtpRequiredDate.CustomFormat != " " && dtpRequiredDate.Value != DateTime.MinValue)
                            {
                                dtpRequiredDate.CustomFormat = utilities.getDateFormat();
                                lines.RequiredByDate = dtpRequiredDate.Value.Date;
                            }
                            else
                            {
                                dtpRequiredDate.CustomFormat = " ";
                            }
                            source.Add(lines);
                        }
                        dgvProductList.DataSource = source;
                    }
                    else
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage(846), utilities.MessageUtils.getMessage("Find Products"));
                    }
                }
                else
                {
                    inventoryProductListBS = new BindingSource();

                    if (inventoryProductListOnDisplay != null)
                    {
                        SortableBindingList<ProductDTO> inventoryProductDTOSortList = new SortableBindingList<ProductDTO>(inventoryProductListOnDisplay);
                        inventoryProductListBS.DataSource = inventoryProductDTOSortList;
                    }
                    else
                    {
                        inventoryProductListBS.DataSource = new SortableBindingList<ProductDTO>();
                        MessageBox.Show(utilities.MessageUtils.getMessage(846), utilities.MessageUtils.getMessage("Find Products"));
                    }

                    dgvProductList.DataSource = inventoryProductListBS;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets Current Stock
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        private double GetCurrentStock(int productId)
        {
            log.LogMethodEntry(productId);
            double stock = 0;
            try
            {
                RequisitionLineGenerics generics = new RequisitionLineGenerics();
                if (!string.IsNullOrEmpty(txtRequisitionId.Text))
                {
                    stock = generics.GetProductStock(Convert.ToInt32(productId), Convert.ToInt32(txtRequisitionId.Text));
                    dgvProductList["StockAtLocation", dgvselectedindex].Value = stock;
                }
                else
                {
                    LocationDTO locationDTO = (LocationDTO)cmbToDept.SelectedItem;
                    if (locationDTO != null && locationDTO.LocationId != -1)
                    {
                        stock = generics.GetProductStockForNew(Convert.ToInt32(productId), Convert.ToInt32(locationDTO.LocationId));
                    }
                    else
                    {
                        lblMessage.Text = utilities.MessageUtils.getMessage(1063);
                    }
                }
            }
            catch (Exception ex) { log.Error(ex); }
            log.LogMethodExit(stock);
            return stock;
        }

        /// <summary>
        /// Insert Requisition lines into table
        /// </summary>
        private void InsertRequisitionLines(int requisitionId, bool isNewTemplate)
        {
            log.LogMethodEntry(requisitionId, isNewTemplate);
            try
            {
                string status = string.Empty;
                RequisitionLinesDTO requisitionLinesDTO = null;
                BindingSource productsListBS = (BindingSource)dgvProductList.DataSource;
                var productListOnDisplay = (SortableBindingList<RequisitionLinesDTO>)productsListBS.DataSource;
                for (int i = 0; i < productListOnDisplay.Count; i++)
                {
                    requisitionLinesDTO = productListOnDisplay[i];
                    if (requisitionLinesDTO.ProductId != -1)
                    {
                        requisitionLinesDTO.UOMId = Convert.ToInt32(dgvProductList.Rows[i].Cells["cmbUOM"].Value);
                        if (Convert.ToDouble(requisitionLinesDTO.RequestedQuantity) > 0)
                        {
                            if (cmbRequisitionStatus.SelectedIndex != -1)
                                status = cmbRequisitionStatus.SelectedItem.ToString();

                            //row required by date validating
                            if (requisitionLinesDTO.RequiredByDate != null && requisitionLinesDTO.RequiredByDate != DateTime.MinValue)
                            {
                                if (dtpRequiredDate.CustomFormat != " " && dtpRequiredDate.Value != DateTime.MinValue)
                                {
                                    DateTime selectedDate = Convert.ToDateTime(dtpRequiredDate.Value.Date);
                                    DateTime cellCurrentDate = Convert.ToDateTime(requisitionLinesDTO.RequiredByDate);
                                    if (cellCurrentDate.Date < selectedDate)
                                    {
                                        MessageBox.Show(utilities.MessageUtils.getMessage(1064));
                                        log.LogMethodExit();
                                        return;
                                    }
                                }
                                else
                                    requisitionLinesDTO.RequiredByDate = DateTime.MinValue;
                            }
                            else
                                requisitionLinesDTO.RequiredByDate = DateTime.MinValue;
                            //add requisition number
                            if (!string.IsNullOrEmpty(txtReqNumber.Text))
                                requisitionLinesDTO.RequisitionNo = txtReqNumber.Text.ToString().Trim();
                            else
                            {
                                if (!string.IsNullOrEmpty(txtRequisitionId.Text))
                                {
                                    requisitionLinesDTO.RequisitionNo = string.Empty;
                                }
                                else
                                {
                                    log.LogMethodExit();
                                    return;
                                }
                            }
                            if (!string.IsNullOrEmpty(txtRequisitionId.Text))
                            {
                                requisitionLinesDTO.RequisitionId = Convert.ToInt32(txtRequisitionId.Text);
                            }
                            else
                            {
                                log.LogMethodExit();
                                return;
                            }
                            requisitionLinesDTO.Status = status;
                            RequisitionLinesBL requisitionLines = new RequisitionLinesBL(executionContext,requisitionLinesDTO);
                            requisitionLines.Save(null);
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

        /// <summary>
        /// load requisition lines into product grid for selected requisition
        /// </summary>
        private void PopulateRequisitionLines(int requisitionId)
        {
            log.LogMethodEntry(requisitionId);
            try
            {
                RequisitionLinesList inventoryReqTemplateLineList = new RequisitionLinesList(executionContext);
                int toDeptId = -1;
                if (cmbToDept.SelectedIndex > 0)
                {
                    LocationDTO toLocation = (LocationDTO)cmbToDept.SelectedItem;
                    if (toLocation != null)
                    {
                        toDeptId = Convert.ToInt32(toLocation.LocationId);
                    }
                }
                List<RequisitionLinesDTO> requisitionLinesOnDisplay = inventoryReqTemplateLineList.GetRequisitionsList(requisitionId, true, toDeptId);
                BindingSource requisitionLinesBS = new BindingSource();
                if (requisitionLinesOnDisplay != null)
                {
                    SortableBindingList<RequisitionLinesDTO> inventoryRequisitionLinesDTOSortList = new SortableBindingList<RequisitionLinesDTO>(requisitionLinesOnDisplay);
                    requisitionLinesBS.DataSource = inventoryRequisitionLinesDTOSortList;
                }
                else
                    requisitionLinesBS.DataSource = new SortableBindingList<RequisitionLinesDTO>();
                dgvProductList.DataSource = requisitionLinesBS;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// to calculate total estimated amount by fetching each product price multiplying with quantity
        /// </summary>
        private void CalculateEstimatedValue()
        {
            log.LogMethodEntry();
            double sum = 0;
            try
            {
                if (dgvProductList != null && dgvProductList.Rows.Count > 0)
                {
                    for (int i = 0; i < dgvProductList.Rows.Count; ++i)
                    {
                        if (dgvProductList.Rows[i].Cells["RequestedQnty"].Value != null && dgvProductList.Rows[i].Cells["OrigPrice"].Value != null)
                        {
                            sum += Convert.ToDouble(dgvProductList.Rows[i].Cells["RequestedQnty"].Value) * Convert.ToDouble(dgvProductList.Rows[i].Cells["OrigPrice"].Value);
                        }
                    }
                }
                txtEstimatedValue.Text = sum.ToString();
            }
            catch (Exception e)
            {
                log.Error(e);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// disable add or remove rows from requisition grid
        /// </summary>
        private void dgvRequisitionsList_BindingSourceAddNew(object sender, AddingNewEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (dgvSearchRequisitions.Rows.Count == inventoryRequisitionListBS.Count)
                {
                    inventoryRequisitionListBS.RemoveAt(inventoryRequisitionListBS.Count - 1);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// button save requisition clicked
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            bool isNewTemplate = false;
            lblMessage.Text = string.Empty;
            try
            {
                dgvProductList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                RequisitionBL requisitionBL;
                RequisitionDTO requisitionsDTO = new RequisitionDTO();

                //requisition Id validation
                if (string.IsNullOrEmpty(txtRequisitionId.Text))
                {
                    isNewTemplate = true;
                    requisitionsDTO.RequisitionId = -1;
                }
                else
                {
                    requisitionsDTO.RequisitionId = Convert.ToInt32(txtRequisitionId.Text);
                }

                //Templates
                if (cmbTemplate.SelectedIndex > 0)
                    requisitionsDTO.TemplateId = Convert.ToInt32(cmbTemplate.SelectedValue);
                else
                {
                    requisitionsDTO.TemplateId = -1;
                }

                //Inventory document type
                if (cmbReqType.SelectedIndex > 0)
                    requisitionsDTO.RequisitionType = Convert.ToInt32(cmbReqType.SelectedValue);
                else
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(1065));
                    return;
                }

                //indent department
                LocationDTO documentTypeSource = (LocationDTO)cmbRequisitionRequestingDept.SelectedItem;
                if (documentTypeSource != null)
                {
                    if (cmbRequisitionRequestingDept.SelectedIndex == -1 || cmbRequisitionRequestingDept.SelectedIndex == 0)
                    {
                        if (cmbReqType.SelectedItem != null)
                        {
                            InventoryDocumentTypeDTO documentType = (InventoryDocumentTypeDTO)cmbReqType.SelectedItem;
                            if (documentType.Code == "MLRQ")
                            {
                                MessageBox.Show(utilities.MessageUtils.getMessage(1066));
                                return;
                            }
                            else
                            {
                                requisitionsDTO.RequestingDept = -1;
                            }

                        }
                    }
                    else
                    {
                        requisitionsDTO.RequestingDept = Convert.ToInt32(cmbRequisitionRequestingDept.SelectedValue);
                    }
                }
                else
                {
                    requisitionsDTO.RequestingDept = -1;
                }

                //from date
                LocationDTO locationFromObj = (LocationDTO)cmbFromDept.SelectedItem;
                string fromDeptName = string.Empty;
                string todeptname = string.Empty;
                if (locationFromObj != null)
                {
                    if (cmbFromDept.SelectedIndex > 0)
                    {
                        requisitionsDTO.FromDepartment = Convert.ToInt32(cmbFromDept.SelectedValue);
                        fromDeptName = locationFromObj.Name.ToString();
                    }
                    else
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage(1067));
                        log.LogMethodExit();
                        return;
                    }
                }
                else
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(1067));
                    log.LogMethodExit();
                    return;
                }

                //to date
                LocationDTO locationToObj = (LocationDTO)cmbToDept.SelectedItem;
                if (locationToObj != null)
                {
                    if (cmbToDept.SelectedIndex > 0)
                    {
                        requisitionsDTO.ToDepartment = Convert.ToInt32(cmbToDept.SelectedValue);
                        todeptname = locationToObj.Name.ToString();
                    }
                    else
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage(1068));
                        log.LogMethodExit();
                        return;
                    }
                }
                else
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(1068));
                    log.LogMethodExit();
                    return;
                }
                
                //status
                if (cmbRequisitionStatus.SelectedIndex != -1)
                    requisitionsDTO.Status = cmbRequisitionStatus.SelectedItem.ToString();
                else
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(1069));
                    log.LogMethodExit();
                    return;
                }

                //Added on 25-Oct-2016 for validating   
                if (dgvProductList.Rows.Count < 2 && cmbRequisitionStatus.Text == "Open")
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(218));
                    log.LogMethodExit();
                    return;
                }
                if (cmbReqType.SelectedItem != null)
                {
                    InventoryDocumentTypeDTO documentType = (InventoryDocumentTypeDTO)cmbReqType.SelectedItem;
                    if (documentType.Code == "ITRQ")
                    {
                        if (cmbTosite.SelectedValue != null && Convert.ToInt32(cmbTosite.SelectedValue) != -1)
                        {
                            if (Convert.ToInt32(cmbTosite.SelectedValue) == utilities.ParafaitEnv.SiteId)
                            {
                                MessageBox.Show(utilities.MessageUtils.getMessage("To site can not be same site."));
                                return;
                            }
                            requisitionsDTO.ToSiteId = Convert.ToInt32(cmbTosite.SelectedValue);
                            requisitionsDTO.FromSiteId = utilities.ParafaitEnv.SiteId;
                        }
                        else
                        {
                            MessageBox.Show(utilities.MessageUtils.getMessage(1186));
                            log.LogMethodExit();
                            return;
                        }
                    }
                }

                requisitionsDTO.Remarks = string.Empty;
                if (!string.IsNullOrEmpty(txtEstimatedValue.Text))
                    requisitionsDTO.EstimatedValue = Convert.ToDouble(txtEstimatedValue.Text);
                if (dtpRequiredDate.CustomFormat != " " && dtpRequiredDate.Value != null)
                {
                    DateTime selectedDate = Convert.ToDateTime(dtpRequiredDate.Value.Date);
                    DateTime todayDate = Convert.ToDateTime(ServerDateTime.Now.Date);
                    if (selectedDate < todayDate)
                    {
                        if (requisitionsDTO.RequisitionId == -1)
                        {
                            MessageBox.Show(utilities.MessageUtils.getMessage(1070));
                            log.LogMethodExit();
                            return;
                        }
                        else
                        {
                            requisitionsDTO.RequiredByDate = dtpRequiredDate.Value.Date;
                        }
                    }
                    else
                    {
                        requisitionsDTO.RequiredByDate = dtpRequiredDate.Value.Date;
                    }
                }
                else
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(15));
                    log.LogMethodExit();
                    return;
                }
                //check is Active selected
                if (chkIsActive.Checked)
                {
                    requisitionsDTO.IsActive = true;
                }
                else
                {
                    requisitionsDTO.IsActive = false;
                }
                //requisition number
                if (!string.IsNullOrEmpty(txtReqNumber.Text))
                {
                    requisitionsDTO.RequisitionNo = txtReqNumber.Text.ToString().Trim();
                }
                else
                {
                    requisitionsDTO.RequisitionNo = string.Empty;
                }
                //from and to department validation
                if (!string.IsNullOrEmpty(fromDeptName) && !string.IsNullOrEmpty(todeptname))
                {
                    if (fromDeptName.Trim() == todeptname.Trim())
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage(1071));
                        log.LogMethodExit();
                        return;
                    }
                }
                LocationBL location = new LocationBL(executionContext, requisitionsDTO.FromDepartment);
                if (location.GetLocationDTO != null && !location.GetLocationDTO.IsActive.Equals(true))
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage("Select active from department."));
                    log.LogMethodExit();
                    return;
                }
                location = new LocationBL(executionContext, requisitionsDTO.ToDepartment);
                if (location.GetLocationDTO != null && !location.GetLocationDTO.IsActive.Equals(true))
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage("Select active to department."));
                    log.LogMethodExit();
                    return;
                }
                location = new LocationBL(executionContext, requisitionsDTO.RequestingDept);
                if (location.GetLocationDTO != null && !location.GetLocationDTO.IsActive.Equals(true))
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage("Select active requesting department."));
                    log.LogMethodExit();
                    return;
                }
                foreach (DataGridViewRow row in this.dgvProductList.Rows)
                {
                    if (row.Cells["RequestedQnty"].Value != null && Convert.ToInt32(row.Cells["RequestedQnty"].Value.ToString()) <= 0)
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage(2404), utilities.MessageUtils.getMessage("Validation Error"));
                        dgvProductList.SelectionMode = DataGridViewSelectionMode.CellSelect;
                        dgvProductList.CurrentCell = row.Cells["RequestedQnty"];
                        log.LogMethodExit();
                        return;
                    }
                }
                BindingSource productsListBS = (BindingSource)dgvProductList.DataSource;
                var productListOnDisplay = (SortableBindingList<RequisitionLinesDTO>)productsListBS.DataSource;
                if (productListOnDisplay == null || productListOnDisplay.Count == 0)
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(2050), utilities.MessageUtils.getMessage("Validation Error")); //No products selected. Please select required products
                    log.LogMethodExit();
                    return;
                }
                requisitionBL = new RequisitionBL(executionContext,requisitionsDTO);
                requisitionBL.Save(null);
                if (requisitionsDTO.RequisitionId != -1)
                {
                    txtRequisitionId.Text = requisitionsDTO.RequisitionId.ToString();
                    txtReqNumber.Text = requisitionsDTO.RequisitionNo;
                    InsertRequisitionLines(requisitionsDTO.RequisitionId, isNewTemplate);

                    PopulateRequisitionGrid();
                    if (dgvSearchRequisitions != null && dgvSearchRequisitions.Rows.Count > 0)
                    {
                        DataGridViewRow row = dgvSearchRequisitions.Rows.Cast<DataGridViewRow>().Where(r => r.Cells["RequisitionId"].Value.Equals(requisitionsDTO.RequisitionId)).First();
                        if (row != null)
                        {
                            dgvSearchRequisitions.Rows[row.Index].Selected = true;
                        }
                    }
                    DisplayRequisitionDetails(requisitionsDTO.RequisitionId);
                    CalculateEstimatedValue();
                    LoadUOMComboBox();
                }
                lblMessage.Text = utilities.MessageUtils.getMessage(1086);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// add default values into product grid
        /// </summary>
        private void dgvReqProducts_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                RequiredByDate.DefaultCellStyle.Format = ("");
                if (dtpRequiredDate.CustomFormat != " ")
                {
                    e.Row.Cells["RequiredByDate"].Value = dtpRequiredDate.Value.Date;
                }
                else
                {
                    if (e.Row.Cells["RequiredByDate"].Value != null)
                    {
                        e.Row.Cells["RequiredByDate"].Value = DateTime.MinValue;
                        if ((DateTime)e.Row.Cells["RequiredByDate"].Value == DateTime.MinValue)
                        {
                            RequiredByDate.DefaultCellStyle.Format = ("");
                        }
                        e.Row.Cells["RequiredByDate"].Value = " ";
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// button requisition search clicked
        /// </summary>
        private void btnSearchRequisitions_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                PopulateRequisitionGrid();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// On requisition grid selected index changed
        /// </summary>
        private void dgvSearchRequisitions_SelectionChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            lblMessage.Text = "";
            try
            {
                if (dgvSearchRequisitions != null && dgvSearchRequisitions.Rows.Count > 0)
                {
                    int selectedIndex;
                    try
                    {
                        selectedIndex = dgvSearchRequisitions.SelectedRows[0].Index;
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        log.LogMethodExit();
                        return;
                    }
                    if (selectedIndex < 0) //Header clicked
                    {
                        log.LogMethodExit();
                        return;
                    }

                    if (!string.IsNullOrEmpty(dgvSearchRequisitions["RequisitionId", selectedIndex].Value.ToString()))
                        cmbTemplate.Enabled = false;
                    else
                        cmbTemplate.Enabled = true;

                    if (dgvSearchRequisitions["Status", selectedIndex].Value.ToString() == "Submitted")
                    {
                        HideControls();
                        lnkLblAddRemarks.Enabled = true;
                        btnSave.Enabled = true;
                        btnEmail.Enabled = true;
                        btnPrint.Enabled = true;
                        cmbRequisitionStatus.Enabled = true;
                    }
                    else if (dgvSearchRequisitions["Status", selectedIndex].Value.ToString() == "InProgress" ||
                        dgvSearchRequisitions["Status", selectedIndex].Value.ToString() == "Closed" ||
                        dgvSearchRequisitions["Status", selectedIndex].Value.ToString() == "Cancelled")
                    {
                        HideControls();
                        lnkLblAddRemarks.Enabled = false;
                        btnSave.Enabled = false;
                        btnEmail.Enabled = false;
                        btnPrint.Enabled = false;
                        cmbRequisitionStatus.Enabled = false;
                    }
                    else
                    {
                        ShowControls();
                        lnkLblAddRemarks.Enabled = true;
                        btnSave.Enabled = true;
                        btnEmail.Enabled = true;
                        btnPrint.Enabled = true;
                        cmbRequisitionStatus.Enabled = true;
                    }

                    reqSelectedStatus = dgvSearchRequisitions["Status", selectedIndex].Value.ToString();

                    if (dgvSearchRequisitions["RequisitionNo", selectedIndex].Value.ToString() == txtReqNumber.Text.ToString() &&
                        dgvSearchRequisitions["RequisitionId", selectedIndex].Value.ToString() == txtRequisitionId.Text.ToString())
                    {
                        log.LogMethodExit();
                        return;
                    }

                    int reqId = Convert.ToInt32(dgvSearchRequisitions["RequisitionId", selectedIndex].Value);
                    string reqNumber = Convert.ToString(dgvSearchRequisitions["RequisitionNo", selectedIndex].Value);
                    DisplayRequisitionDetails(reqId);
                    PopulateRequisitionLines(reqId);
                    LoadUOMComboBox();
                    CalculateEstimatedValue();

                    //Added on 25-Oct-2016 for assigning requiredByDate 
                    if (dtpRequiredDate.Text.Trim() != string.Empty)
                        requiredByDateTime = dtpRequiredDate.Value;
                    else
                        requiredByDateTime = DateTime.MinValue;
                }
                else
                {
                    ShowControls();
                    lnkLblAddRemarks.Enabled = false;
                    btnSave.Enabled = true;
                    btnEmail.Enabled = true;
                    btnPrint.Enabled = true;
                    cmbRequisitionStatus.Enabled = true;
                }

            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// to hide the controls based on requisition status
        /// </summary>
        private void HideControls()
        {
            log.LogMethodEntry();
            gb_Requisition.Enabled = true;
            gbItemDetails.Enabled = false;
            cmbTemplate.Enabled = false;
            cmbReqType.Enabled = false;
            cmbRequisitionRequestingDept.Enabled = false;
            cmbFromDept.Enabled = false;
            cmbToDept.Enabled = false;
            dtpRequiredDate.Enabled = false;
            chkIsActive.Enabled = false;
            btnProdSearch.Enabled = false;
            cmbTosite.Enabled = false;
            log.LogMethodExit();
        }

        /// <summary>
        /// to show the controls based on requisition status
        /// </summary>
        private void ShowControls()
        {
            log.LogMethodEntry();
            gb_Requisition.Enabled = true;
            gbItemDetails.Enabled = true;

            if (!string.IsNullOrEmpty(txtReqNumber.Text))
                cmbTemplate.Enabled = false;
            else
                cmbTemplate.Enabled = true;

            cmbReqType.Enabled = true;
            cmbRequisitionRequestingDept.Enabled = true;
            cmbFromDept.Enabled = true;
            cmbToDept.Enabled = true;
            dtpRequiredDate.Enabled = true;
            btnSave.Enabled = true;
            btnEmail.Enabled = true;
            btnPrint.Enabled = true;
            btnProdSearch.Enabled = true;
            chkIsActive.Enabled = true; //Added on 25-Oct-2016 for enabling isActive checkbox
            cmbTosite.Enabled = true;
            log.LogMethodExit();
        }

        /// <summary>
        /// product search fields clear
        /// </summary>
        private void btnClear_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                txtProdcode.Text = string.Empty;
                cmbProdCategory.SelectedIndex = -1;
                txtProdBarcode.Text = string.Empty;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// product search button clicked
        /// </summary>
        private void btnProdSearch_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                PopulateGrid();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// On product grid cell value changed - it will open new popupgrid with search criteria matches
        /// </summary>
        private void dgvProductList_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            //required by date validation
            if (e.ColumnIndex < 0 || e.RowIndex < 0)
            {
                log.LogMethodExit(null, "Invalid cell");
                return;
            }
            if (dgvProductList.Columns[e.ColumnIndex].Name == "RequiredByDate" && fireCellValueChange)
            {
                DateTime selectedDate = Convert.ToDateTime(dtpRequiredDate.Value.Date);
                DateTime cellCurrentDate;
                if (dgvProductList["RequiredByDate", e.RowIndex].Value != null)
                {
                    cellCurrentDate = Convert.ToDateTime(dgvProductList["RequiredByDate", e.RowIndex].Value);
                    if (cellCurrentDate.Date < selectedDate)
                    {
                        MessageBox.Show("Selected date must be greater than or equal to header required date");

                        //Added on 25-Oct-2016 for clearing or retaing date after validating
                        fireCellValueChange = false;
                        if (dtpRequiredDate.Text.Trim() == String.Empty)
                        {
                            dgvProductList["RequiredByDate", e.RowIndex].Value = string.Empty;
                        }
                        else
                        {
                            dgvProductList["RequiredByDate", e.RowIndex].Value = dtpRequiredDate.Text;
                        }
                        fireCellValueChange = true;
                        //end
                        log.LogMethodExit();
                        return;
                    }
                }
            }

            if (dgvProductList.Columns[e.ColumnIndex].Name == "Code" && fireCellValueChange)
            {
                try
                {
                    List<SqlParameter> parameters = new List<SqlParameter>();
                    ProductList inventoryProductList = new ProductList();
                    string query = string.Empty;
                    if (dgvProductList[e.ColumnIndex, e.RowIndex].Value != null)
                    {
                        query = "(Isnull(T.code,'') like " + "N'%" + dgvProductList[e.ColumnIndex, e.RowIndex].Value.ToString() + "%'" + " or Isnull(T.Description,'') like " + "N'%" + dgvProductList[e.ColumnIndex, e.RowIndex].Value.ToString() + "%'" + " or Isnull(T.ProdBarCode,'') like " + "N'%" + dgvProductList[e.ColumnIndex, e.RowIndex].Value.ToString() + "%')";
                    }
                    if (cmbReqType.SelectedIndex > 0)
                    {
                        InventoryDocumentTypeDTO documentTypeDTO = (InventoryDocumentTypeDTO)cmbReqType.SelectedItem;
                        if (documentTypeDTO != null)
                        {
                            if (documentTypeDTO.Code == "MLRQ")
                            {
                                query += " and Isnull(T.MarketListItem, 0) = 1";
                            }
                            else
                            {
                                query += " and Isnull(T.MarketListItem, 0) = 0";
                            }
                            if (documentTypeDTO.Code == "ITRQ")
                            {
                                query += " and Isnull(T.MasterEntityId, -1) > -1";
                            }
                        }
                    }

                    query += " and isnull(T.IsActive, '')='Y'";

                    query += " and (T.Site_id = " + executionContext.GetSiteId() + " or -1= " + executionContext.GetSiteId() + ")";

                    int categoryId = -1;
                    categoryId = Convert.ToInt32(cmbCategory.SelectedValue);
                    if (categoryId != -1)
                    {
                        query += " and T.CategoryId =@categoryId";
                        parameters.Add(new SqlParameter("@categoryId", categoryId));
                    }

                    RequisitionLineGenerics generics = new RequisitionLineGenerics();
                    inventoryProductListOnDisplay = inventoryProductList.GetBarcodeSearchProducts(query, parameters);

                    if (inventoryProductListOnDisplay == null || inventoryProductListOnDisplay.Count < 1)
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage(846), utilities.MessageUtils.getMessage("Find Products"));
                        dgvselectedindex = e.RowIndex;
                        BeginInvoke(new MethodInvoker(RemoveDGVRow));
                    }
                    else if (inventoryProductListOnDisplay.Count == 1)
                    {
                        dgvselectedindex = e.RowIndex;
                        fireCellValueChange = false;
                        dgvProductList["Code", dgvselectedindex].Value = inventoryProductListOnDisplay[0].Code;
                        fireCellValueChange = true;
                        dgvProductList["Description", dgvselectedindex].Value = inventoryProductListOnDisplay[0].Description;
                        int uomId = inventoryProductListOnDisplay[0].UomId;
                        dgvProductList["UOMId", dgvselectedindex].Value = uomId;
                        CommonFuncs.GetUOMComboboxForSelectedRows(dgvProductList, dgvselectedindex, uomId);
                        dgvProductList["OrigPrice", dgvselectedindex].Value = inventoryProductListOnDisplay[0].Cost;
                        int CategoryId = inventoryProductListOnDisplay[0].CategoryId;
                        CategoryDTO categoryDTO = categoryListOnDisplay.Find(prd => prd.CategoryId == CategoryId);
                        if (categoryDTO != null)
                        {
                            dgvProductList["Category", dgvselectedindex].Value = categoryDTO.Name;
                        }
                        else
                        {
                            dgvProductList["Category", dgvselectedindex].Value = string.Empty;
                        }
                        double productQnty = 0;
                        if (!string.IsNullOrEmpty(txtRequisitionId.Text))
                        {
                            productQnty = generics.GetProductStock(inventoryProductListOnDisplay[0].ProductId, Convert.ToInt32(txtRequisitionId.Text));
                            dgvProductList["StockAtLocation", dgvselectedindex].Value = productQnty;
                        }
                        else
                        {
                            LocationDTO LocationType = (LocationDTO)cmbToDept.SelectedItem;
                            if (LocationType != null)
                            {
                                productQnty = generics.GetProductStockForNew(inventoryProductListOnDisplay[0].ProductId, Convert.ToInt32(LocationType.LocationId));
                                dgvProductList["StockAtLocation", dgvselectedindex].Value = productQnty;
                            }
                            else
                            {
                                MessageBox.Show(utilities.MessageUtils.getMessage("Select To Department"));
                            }
                        }

                        dgvProductList["ProductId", dgvselectedindex].Value = inventoryProductListOnDisplay[0].ProductId;
                    }
                    else if (inventoryProductListOnDisplay.Count > 1)
                    {
                        dgvselectedindex = e.RowIndex;
                        Panel pnlMultiple_dgv = new Panel();
                        this.Controls.Add(pnlMultiple_dgv);
                        DataGridView multiple_dgv = new DataGridView();
                        pnlMultiple_dgv.Controls.Add(multiple_dgv);
                        multiple_dgv.LostFocus += new EventHandler(multiple_dgv_LostFocus);
                        multiple_dgv.Click += new EventHandler(multiple_dgv_Click);
                        multiple_dgv.Focus();
                        inventoryProductListOnDisplay = inventoryProductListOnDisplay.OrderBy(x => x.Description).ToList();
                        multiple_dgv.DataSource = inventoryProductListOnDisplay;
                        multiple_dgv.Refresh();
                        multiple_dgv_Format(ref pnlMultiple_dgv, ref multiple_dgv); //Changed the function so that the grid is in a panel and scroll bar can be added if list is long
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    MessageBox.Show(ex.Message);
                }
                log.LogMethodExit();
            }
        }

        /// <summary>
        /// multiple_dgv_LostFocus
        /// </summary>
        void multiple_dgv_LostFocus(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                DataGridView dg = (DataGridView)sender;
                if (dg.SelectedRows.Count == 0)
                {
                    dgvProductList.Rows.Remove(dgvProductList.Rows[dgvselectedindex]);
                }
                dg.Visible = false;
                dg.Parent.Visible = false;
                dgvProductList.Controls.Remove(dg.Parent);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// multiple_dgv_Click
        /// </summary>
        void multiple_dgv_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                DataGridView dg = (DataGridView)sender;
                fireCellValueChange = false;
                RequisitionLineGenerics generics = new RequisitionLineGenerics();
                double stock = 0;
                int categoryId = Convert.ToInt32(dg.Rows[dg.CurrentRow.Index].Cells["CategoryId"].Value);
                stock = GetCurrentStock(Convert.ToInt32(dg.Rows[dg.CurrentRow.Index].Cells["ProductId"].Value));
                dgvProductList["Code", dgvselectedindex].Value = dg.Rows[dg.CurrentRow.Index].Cells["Code"].Value;
                dgvProductList["Description", dgvselectedindex].Value = dg.Rows[dg.CurrentRow.Index].Cells["Description"].Value;
                int uomId = Convert.ToInt32(dg.Rows[dg.CurrentRow.Index].Cells["UOMId"].Value);
                dgvProductList["UOMId", dgvselectedindex].Value = uomId;
                CommonFuncs.GetUOMComboboxForSelectedRows(dgvProductList, dgvselectedindex, uomId);
                dgvProductList["OrigPrice", dgvselectedindex].Value = dg.Rows[dg.CurrentRow.Index].Cells["Cost"].Value;
                dgvProductList["ProductId", dgvselectedindex].Value = dg.Rows[dg.CurrentRow.Index].Cells["ProductId"].Value;
                dgvProductList["StockAtLocation", dgvselectedindex].Value = Convert.ToDouble(stock);
                dgvProductList["Category", dgvselectedindex].Value = string.Empty;
                CategoryDTO categoryDTO = categoryListOnDisplay.Find(prd => prd.CategoryId == categoryId);
                if (categoryDTO != null)
                {
                    dgvProductList["Category", dgvselectedindex].Value = categoryDTO.Name;
                }
                else
                {
                    dgvProductList["Category", dgvselectedindex].Value = string.Empty;
                }
                fireCellValueChange = true;
                dg.Parent.Visible = false;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// multiple_dgv_Format
        /// </summary>
        void multiple_dgv_Format(ref Panel pnlMultiple_dgv, ref DataGridView multiple_dgv)
        {
            try
            {
                log.LogMethodEntry(pnlMultiple_dgv, multiple_dgv);
                pnlMultiple_dgv.Size = new Size(300, (dgvProductList.Rows[0].Cells[0].Size.Height * 10) - 3); //Changed the function so that the grid is in a panel and scroll bar can be added if list is long
                pnlMultiple_dgv.AutoScroll = true;

                pnlMultiple_dgv.Location = new Point(150 + gb_Requisition.Location.X + dgvProductList.RowHeadersWidth + dgvProductList.CurrentRow.Cells["Code"].ContentBounds.Location.X, 257 + dgvProductList.Location.Y + dgvProductList.ColumnHeadersHeight);
                pnlMultiple_dgv.BringToFront();
                pnlMultiple_dgv.BorderStyle = BorderStyle.None;
                pnlMultiple_dgv.BackColor = Color.White;
                multiple_dgv.Width = 300;
                multiple_dgv.BorderStyle = BorderStyle.None;
                multiple_dgv.AllowUserToAddRows = false;
                multiple_dgv.BackgroundColor = Color.White;
                for (int j = 0; j < inventoryProductListOnDisplay.Count; j++)
                {
                    int categoryId = Convert.ToInt32(multiple_dgv.Rows[j].Cells["CategoryId"].Value);
                    CategoryDTO categoryDTO = categoryListOnDisplay.Find(x => x.CategoryId == categoryId);
                    if (categoryDTO != null)
                    {
                        multiple_dgv.Rows[j].Cells["Remarks"].Value = categoryDTO.Name;
                    }
                    else
                        multiple_dgv.Rows[j].Cells["Remarks"].Value = string.Empty;
                }

                for (int i = 0; i < multiple_dgv.Columns.Count; i++)
                {
                    if (multiple_dgv.Columns[i].HeaderText == "Code"
                       || multiple_dgv.Columns[i].HeaderText == "Description"
                       || multiple_dgv.Columns[i].HeaderText == "Remarks")
                    {
                        multiple_dgv.Columns[i].Visible = true;
                    }
                    else
                    {
                        multiple_dgv.Columns[i].Visible = false;
                    }
                }

                multiple_dgv.Columns["Description"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                multiple_dgv.Font = new Font("Arial", 8, FontStyle.Regular);
                multiple_dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                multiple_dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                multiple_dgv.ReadOnly = true;
                multiple_dgv.BorderStyle = BorderStyle.None;
                multiple_dgv.RowHeadersVisible = false;
                multiple_dgv.ColumnHeadersVisible = false;
                multiple_dgv.AllowUserToResizeColumns = false;
                multiple_dgv.MultiSelect = false;
                multiple_dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                multiple_dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.Wheat;
                multiple_dgv.EndEdit();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void serialbarcodeDataReceived()
        {
            log.LogMethodEntry();
            string scannedBarcode = BarcodeReader.Barcode;
            if (scannedBarcode != "")
            {
                try
                {
                    txtProdBarcode.Text = scannedBarcode;
                }
                catch (Exception ex)
                {
                    log.Error(ex);

                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// To remove unsaved fields
        /// </summary>
        private void RefreshPage()
        {
            log.LogMethodEntry();
            LoadTemplates();
            if (!string.IsNullOrEmpty(txtRequisitionId.Text))
            {
                if (dgvSearchRequisitions != null && dgvSearchRequisitions.Rows.Count > 0)
                {
                    DisplayRequisitionDetails(Convert.ToInt32(txtRequisitionId.Text));
                    PopulateRequisitionLines(Convert.ToInt32(txtRequisitionId.Text));
                    CalculateEstimatedValue();
                    cmbTemplate.Enabled = false;
                }
                else
                {
                    cmbTemplate.Enabled = true;
                    ClearRequisitionHeaderAndLines();
                    ClearSearchFields();
                }
            }
            else
            {
                int reqgridSelectedIndex = 0;
                string id = string.Empty;
                if (dgvSearchRequisitions != null && dgvSearchRequisitions.Rows.Count > 0)
                {
                    reqgridSelectedIndex = dgvSearchRequisitions.SelectedRows[0].Index;
                    id = dgvSearchRequisitions["RequisitionId", reqgridSelectedIndex].Value.ToString();

                    DisplayRequisitionDetails(Convert.ToInt32(id));
                    PopulateRequisitionLines(Convert.ToInt32(id));
                    CalculateEstimatedValue();
                }

                if (!string.IsNullOrEmpty(id))
                    cmbTemplate.Enabled = false;
                else
                    cmbTemplate.Enabled = true;
            }
            lblMessage.Text = string.Empty;
            txtReqNoSearch.Text = string.Empty;
            txtProdcode.Text = string.Empty;
            cmbSearchReqType.SelectedIndex = -1;
            txtProdBarcode.Text = string.Empty;
            cmbSearchStatus.SelectedIndex = -1;
            chkSearchIsActive.Checked = true;
            cmbProdCategory.SelectedIndex = -1;
            cmbReqType.Focus();
            LoadUOMComboBox();
            log.LogMethodExit();
        }

        private void ClearRequisitionHeaderAndLines()
        {
            log.LogMethodEntry();
            //group box
            gb_Requisition.Enabled = true;
            gbItemDetails.Enabled = true;

            //Combo boxes
            cmbTemplate.Enabled = true;
            cmbTemplate.SelectedIndex = -1;

            cmbReqType.Enabled = true;
            cmbReqType.SelectedIndex = -1;

            cmbTosite.Enabled = false;
            cmbTosite.SelectedIndex = -1;

            cmbRequisitionRequestingDept.Enabled = true;
            cmbRequisitionRequestingDept.SelectedIndex = -1;
            BindingSource cmbRequisitionRequestingDeptBS = new BindingSource();
            cmbRequisitionRequestingDeptBS.DataSource = new SortableBindingList<LocationDTO>();
            cmbRequisitionRequestingDept.DataSource = cmbRequisitionRequestingDeptBS;

            cmbFromDept.Enabled = true;
            cmbFromDept.SelectedIndex = -1;
            BindingSource cmbFromDeptBS = new BindingSource();
            cmbFromDeptBS.DataSource = new SortableBindingList<LocationDTO>();
            cmbFromDept.DataSource = cmbFromDeptBS;

            cmbToDept.Enabled = true;
            cmbToDept.SelectedIndex = -1;
            BindingSource cmbToDeptBS = new BindingSource();
            cmbToDeptBS.DataSource = new SortableBindingList<LocationDTO>();
            cmbToDept.DataSource = cmbToDeptBS;

            cmbRequisitionStatus.Enabled = true;
            reqSelectedStatus = "Open";
            cmbRequisitionStatus.SelectedItem = "Open";

            dtpRequiredDate.Enabled = true;
            dtpRequiredDate.CustomFormat = " ";
            dtpRequiredDate.Value = System.DateTime.Today;
            chkIsActive.Enabled = true; //Added on 25-Oct-2016 for fixing isActive issue

            //text boxes
            txtReqNumber.Text = "";
            txtRequisitionId.Text = "";
            txtEstimatedValue.Text = "";
            lnkLblAddRemarks.Enabled = true;

            BindingSource productBS = new BindingSource();
            productBS.DataSource = new SortableBindingList<RequisitionLinesDTO>();
            dgvProductList.DataSource = productBS;
            log.LogMethodExit();
        }

        /// <summary>
        /// Clear all page values
        /// </summary>
        private void ClearFields()
        {
            log.LogMethodEntry();
            lblMessage.Text = "";
            reqSelectedStatus = string.Empty;

            ClearRequisitionHeaderAndLines();
            ClearSearchFields();
            ClearRequisitionSearchFields();

            btnSave.Enabled = true;
            btnProdSearch.Enabled = true;
            btnSave.Enabled = true;
            btnEmail.Enabled = true;
            btnPrint.Enabled = true;
            log.LogMethodExit();
        }

        private void ClearRequisitionSearchFields()
        {
            log.LogMethodEntry();
            txtReqNoSearch.Text = string.Empty;
            cmbSearchReqType.SelectedIndex = -1;
            cmbSearchStatus.SelectedIndex = -1;
            chkSearchIsActive.Checked = true;
            log.LogMethodExit();
        }

        private void ClearSearchFields()
        {
            log.LogMethodEntry();
            txtProdcode.Text = string.Empty;
            cmbProdCategory.SelectedIndex = -1;
            txtProdBarcode.Text = string.Empty;
        }

        /// <summary>
        /// To remove unsaved fields value
        /// </summary>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                RefreshPage();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// To create new requisition
        /// </summary>
        private void btnCreateNew_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                ClearFields();
                cmbTemplate.SelectedIndex = -1;
                cmbTemplate.Enabled = true;
                cmbCategory.SelectedIndex = 0;
                requiredByDateTime = DateTime.MinValue; //Added on 25-Oct-2016 for assigning requiredByDate 
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// send email button clicked
        /// </summary>
        string POFileName;
        private void btnEmail_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (txtReqNumber.Text == string.Empty)
                {
                    log.LogMethodExit();
                    return;
                }

                POFileName = utilities.MessageUtils.getMessage("_RequisitionNumber_") + txtReqNumber.Text;
                string subject = POFileName + ".pdf";
                CreatePdfFile();

                string body = "Hi " + "" + "," + Environment.NewLine + Environment.NewLine;
                body += "Please find attached Requisition " + txtReqNumber.Text + " for your immediate processing." + Environment.NewLine + Environment.NewLine;
                body += "Regards," + Environment.NewLine + utilities.ParafaitEnv.Username;
                string reportDir = utilities.getParafaitDefaults("PDF_OUTPUT_DIR") + "\\";

                SendEmailUI emailF = new SendEmailUI("", "", "", subject, body, POFileName + ".pdf", reportDir, false, utilities);
                emailF.ShowDialog();

                System.IO.FileInfo fi = new System.IO.FileInfo(reportDir + POFileName + ".pdf");
                try
                {
                    fi.Delete();
                }
                catch (Exception ex) { log.Error(ex); }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(utilities.MessageUtils.getMessage("Error while creating PDF", ex), utilities.MessageUtils.getMessage("Send Email"));
            }
            log.LogMethodExit();
        }

        private void CreatePdfFile()
        {
            log.LogMethodEntry();
            MyPrintDocument = new PrintDocument();
            MyPrintDocument.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(MyPrintDocument_PrintPage);
            formatDGVforPrint();
            if (SetupThePrinting())
            {
                MyPrintDocument.Print();
            }
            log.LogMethodExit();
        }

        private void formatDGVforPrint()
        {
            log.LogMethodEntry();
            printdgv.DefaultCellStyle.Font = new Font("Tahoma", 8);
            printdgv.Columns.Clear();
            int index;
            printdgv.GridColor = Color.White;
            printdgv.CellBorderStyle = DataGridViewCellBorderStyle.None;
            printdgv.Columns.Add("col1", "col1");
            printdgv.Columns.Add("col2", "col2");
            printdgv.Columns.Add("col3", "col3");
            printdgv.Columns.Add("col4", "col4");
            printdgv.Columns.Add("col5", "col5");
            printdgv.Columns.Add("col6", "col6");
            printdgv.ColumnHeadersVisible = false;
            printdgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            printdgv.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
            printdgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;

            index = printdgv.Rows.Add(utilities.MessageUtils.getMessage("Inventory Requisition id: "), txtRequisitionId.Text, "", "", "");
            printdgv.Rows[index].DefaultCellStyle.Font = new Font(this.Font, FontStyle.Bold);

            index = printdgv.Rows.Add(utilities.MessageUtils.getMessage("Inventory Requisition number: "), txtReqNumber.Text, "", "", "");
            printdgv.Rows[index].DefaultCellStyle.Font = new Font(this.Font, FontStyle.Bold);

            index = printdgv.Rows.Add(utilities.MessageUtils.getMessage("Inventory Requisition type: "), cmbReqType.Text, "", "", "");
            printdgv.Rows[index].DefaultCellStyle.Font = new Font(this.Font, FontStyle.Bold);
            printdgv.Rows.Add("", "", "", "", "");
            printdgv.Rows.Add("", "", "", "", "");

            index = printdgv.Rows.Add(utilities.MessageUtils.getMessage("Product Code"), utilities.MessageUtils.getMessage("Description"), utilities.MessageUtils.getMessage("Category"), utilities.MessageUtils.getMessage("Required Quantity"), utilities.MessageUtils.getMessage("Required By Date"));
            printdgv.Rows[index].DefaultCellStyle.BackColor = Color.Gray;
            printdgv.Rows[index].DefaultCellStyle.Font = new Font(this.Font, FontStyle.Bold);

            printdgv.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            printdgv.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            printdgv.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            printdgv.Columns[5].DefaultCellStyle = utilities.gridViewDateCellStyle();

            printdgv.Rows.Add("", "", "", "", "");
            for (int i = 0; i < dgvProductList.RowCount - 1; i++)
            {
                int j = printdgv.Rows.Add(dgvProductList["Code", i].Value.ToString(),
                                        dgvProductList["Description", i].Value == null ? string.Empty : dgvProductList["Description", i].Value.ToString().PadRight(30, ' ').Substring(0, 30),
                                        dgvProductList["Category", i].Value.ToString(),
                                        dgvProductList["RequestedQnty", i].Value == null ? string.Empty : string.Format("{0:N0}", dgvProductList["RequestedQnty", i].Value),
                                        dgvProductList["RequiredByDate", i].Value == null ? string.Empty : dgvProductList["RequiredByDate", i].Value);
            }
            printdgv.Rows.Add("", "", "", "", "", "");
            log.LogMethodExit();
        }

        /// <summary>
        /// Printing Setup
        /// </summary>
        /// <returns></returns>
        private bool SetupThePrinting()
        {
            log.LogMethodEntry();
            PrintDialog MyPrintDialog = new PrintDialog();
            MyPrintDialog.AllowCurrentPage = false;
            MyPrintDialog.AllowPrintToFile = false;
            MyPrintDialog.AllowSelection = false;
            MyPrintDialog.AllowSomePages = false;
            MyPrintDialog.PrintToFile = false;
            MyPrintDialog.ShowHelp = false;
            MyPrintDialog.ShowNetwork = false;
            MyPrintDialog.PrinterSettings.DefaultPageSettings.Landscape = false;

            MyPrintDialog.PrinterSettings.PrinterName = PrintUtils.GetPDFPrinterName();
            PrintUtils.SetOutputFileName(utilities.getParafaitDefaults("PDF_OUTPUT_DIR") + "\\" + POFileName + ".pdf");//CommonFuncs.GetReportFileName(POFileName, "").Replace(" - .pdf", ".pdf"));

            MyPrintDocument.DefaultPageSettings =
            MyPrintDialog.PrinterSettings.DefaultPageSettings;
            MyPrintDocument.DefaultPageSettings.Margins =
                             new Margins(20, 20, 20, 20);
            MyPrintDocument.PrinterSettings =
                                MyPrintDialog.PrinterSettings;
            MyPrintDocument.DocumentName = utilities.MessageUtils.getMessage("Inventory Requisition Number ") + txtReqNumber.Text;
            string reportTitle = utilities.MessageUtils.getMessage("Inventory Requisition") +
                                Environment.NewLine +
                                Environment.NewLine +
                                Environment.NewLine;

            if (utilities.ParafaitEnv.CompanyLogo == null)
            {
                MyDataGridViewPrinter = new DataGridViewPrinter(printdgv,
                    MyPrintDocument, true, true, reportTitle, new Font("Tahoma", 14,
                        FontStyle.Bold, GraphicsUnit.Point), Color.Black, true);
            }
            else
            {
                MyDataGridViewPrinter = new DataGridViewPrinter(printdgv,
                    MyPrintDocument, true, true, reportTitle, new Font("Tahoma", 14,
                     FontStyle.Bold, GraphicsUnit.Point), Color.Black, true, utilities.ParafaitEnv.CompanyLogo);
            }
            log.LogMethodExit(true);
            return true;
        }

        private void MyPrintDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            bool more = MyDataGridViewPrinter.DrawDataGridView(e.Graphics);
            if (more == true)
                e.HasMorePages = true;
            log.LogMethodExit();
        }


        /// <summary>
        /// Combo box Template on index selection changed
        /// </summary>
        private void cmbTemplate_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                int templateId = -1;
                lblMessage.Text = "";
                if (fireSelectionChanged)
                {
                    if (cmbTemplate.SelectedIndex != -1 && cmbTemplate.SelectedIndex != 0)
                    {
                        cmbTemplate.Enabled = false;
                        if (fireTemplateSelectionChange)
                        {
                            if (cmbTemplate.SelectedItem != null)
                            {
                                RequisitionTemplatesDTO template = (RequisitionTemplatesDTO)cmbTemplate.SelectedItem;
                                templateId = template.TemplateId;
                                DisplayTemplateDetails(templateId);
                                DisplayTemplateLines(templateId);
                                CalculateEstimatedValue();
                            }
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(txtReqNumber.Text))
                            cmbTemplate.Enabled = false;
                        else
                            cmbTemplate.Enabled = true;
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

        /// <summary>
        /// document type on index selection changed
        /// </summary>
        private void cmbReqType_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            lblMessage.Text = "";
            int typeId = Convert.ToInt32(cmbReqType.SelectedIndex);
            if (typeId > 0)
            {
                cmbReqType.Enabled = false;
                InventoryDocumentTypeDTO documentType = (InventoryDocumentTypeDTO)cmbReqType.SelectedItem;
                if (documentType != null)
                {
                    if (documentType.Code == "MLRQ")
                    {
                        dtpRequiredDate.CustomFormat = "dd-MMM-yyyy";
                        dtpRequiredDate.Value = DateTime.Now.Date.AddDays(1);
                    }
                    if (documentType.Code == "ITRQ" && btnSave.Enabled)
                    {
                        cmbTosite.Enabled = true;
                    }
                    else
                    {
                        cmbTosite.Enabled = false;
                    }

                }


                if (cmbReqType.SelectedIndex > 0)
                {
                    LoadLocationsOnType();
                }
            }
            else
            {
                cmbReqType.Enabled = true;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// load the locations on document type selected
        /// </summary>
        private void LoadLocationsOnType()
        {
            log.LogMethodEntry();

            try
            {
                LocationList inventoryLocationList = new LocationList(executionContext);

                List<LocationDTO> fromLocationList = new List<LocationDTO>();
                List<LocationDTO> toLocationList = new List<LocationDTO>();
                List<LocationDTO> requestingLocationList = new List<LocationDTO>();
                BindingSource inventoryFromDeptListBS = new BindingSource();
                BindingSource inventoryToDeptListBS = new BindingSource();
                BindingSource inventoryReqDeptListBS = new BindingSource();
                InventoryDocumentTypeDTO documentTypeDTO = (InventoryDocumentTypeDTO)cmbReqType.SelectedItem;
                if (documentTypeDTO != null)
                {
                    switch (documentTypeDTO.Code)
                    {
                        case "MLRQ":
                            try
                            {
                                fromLocationList = inventoryLocationList.GetLocationsOnLocationType("'" + "Store" + "'", executionContext.GetSiteId());
                                toLocationList = inventoryLocationList.GetLocationsOnLocationType("'" + "Purchase" + "'", executionContext.GetSiteId());
                            }
                            catch (Exception ex) { log.Error("Error in LoadLocationsOnType() event. load MLRG list", ex); }
                            break;
                        case "PURQ":
                            try
                            {
                                fromLocationList = inventoryLocationList.GetLocationsOnLocationType("'" + "Store" + "'", executionContext.GetSiteId());
                                toLocationList = inventoryLocationList.GetLocationsOnLocationType("'Purchase" + "','" + "Store'", executionContext.GetSiteId());
                            }
                            catch (Exception ex) { log.Error("Error in LoadLocationsOnType() event. load PURQ list", ex); }
                            break;
                        case "ISRQ":
                            try
                            {
                                fromLocationList = inventoryLocationList.GetLocationsOnLocationType("'Department" + "','" + "Store'", executionContext.GetSiteId());
                                toLocationList = inventoryLocationList.GetLocationsOnLocationType("'" + "Store" + "'", executionContext.GetSiteId());
                            }
                            catch (Exception ex) { log.Error("Error in LoadLocationsOnType() event. load ISRQ list", ex); }
                            break;
                        case "ITRQ":
                            try
                            {
                                fromLocationList = inventoryLocationList.GetLocationsOnLocationType("'" + "Store'", executionContext.GetSiteId(), true);
                                toLocationList = inventoryLocationList.GetLocationsOnLocationType("'" + "Store" + "'", executionContext.GetSiteId(), true);
                            }
                            catch (Exception ex) { log.Error("Error in LoadLocationsOnType() event. load ITRQ list", ex); }
                            break;
                    }
                    if (documentTypeDTO.Code.Equals("ITRQ"))
                    {
                        requestingLocationList = inventoryLocationList.GetLocationsOnLocationType("'Store'", executionContext.GetSiteId(), true);
                    }
                    else
                    {
                        requestingLocationList = inventoryLocationList.GetLocationsOnLocationType("'Department'", executionContext.GetSiteId());
                    }
                }
                if (fromLocationList != null && fromLocationList.Count > 0)
                {
                    fromLocationList.Insert(0, new LocationDTO());
                    SortableBindingList<LocationDTO> inventoryLocationFromDTOSortList = new SortableBindingList<LocationDTO>(fromLocationList);
                    inventoryFromDeptListBS.DataSource = inventoryLocationFromDTOSortList;
                }
                else
                {
                    inventoryFromDeptListBS.DataSource = new SortableBindingList<LocationDTO>();
                }

                if (requestingLocationList != null && requestingLocationList.Count > 0)
                {
                    requestingLocationList.Insert(0, new LocationDTO());
                    SortableBindingList<LocationDTO> inventoryReqLocationDTOSortList = new SortableBindingList<LocationDTO>(requestingLocationList);
                    inventoryReqDeptListBS.DataSource = inventoryReqLocationDTOSortList;
                }
                else
                {
                    inventoryReqDeptListBS.DataSource = new SortableBindingList<LocationDTO>();
                }
                if (toLocationList != null && toLocationList.Count > 0)
                {
                    toLocationList.Insert(0, new LocationDTO());
                    SortableBindingList<LocationDTO> inventoryToLocationDTOSortList = new SortableBindingList<LocationDTO>(toLocationList);
                    inventoryToDeptListBS.DataSource = inventoryToLocationDTOSortList;
                }
                else
                {
                    inventoryToDeptListBS.DataSource = new SortableBindingList<LocationDTO>();
                }

                cmbRequisitionRequestingDept.DataSource = inventoryReqDeptListBS;
                cmbRequisitionRequestingDept.ValueMember = "LocationId";
                cmbRequisitionRequestingDept.DisplayMember = "Name";

                cmbFromDept.DataSource = inventoryFromDeptListBS;
                cmbFromDept.ValueMember = "LocationId";
                cmbFromDept.DisplayMember = "Name";

                cmbToDept.DataSource = inventoryToDeptListBS;
                cmbToDept.ValueMember = "LocationId";
                cmbToDept.DisplayMember = "Name";
            }
            catch (Exception ex)
            {
                log.Error("Error in -LoadLocationsOnType() event.", ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// TO validate selected date from date picker
        /// </summary>
        private void dtpRequiredDate_CloseUp(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            // Modified on 25-Oct-2015 for assigning retained date after validating. 
            DateTime selectedDate = Convert.ToDateTime(dtpRequiredDate.Value.Date);
            DateTime todayDate = Convert.ToDateTime(DateTime.Now.Date);
            if (selectedDate < todayDate)
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(1070));
                try
                {
                    InventoryDocumentTypeDTO documentType = (InventoryDocumentTypeDTO)cmbReqType.SelectedItem;
                    if (documentType != null && documentType.Code == "MLRQ")
                    {
                        if (requiredByDateTime != DateTime.MinValue)
                            dtpRequiredDate.Value = requiredByDateTime;
                        else
                            dtpRequiredDate.Value = DateTime.Now.AddDays(1);
                    }
                    else
                    {
                        if (requiredByDateTime != DateTime.MinValue)
                            dtpRequiredDate.Value = requiredByDateTime;
                        else
                            dtpRequiredDate.CustomFormat = " ";
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
                log.LogMethodExit();
                return;
            }//end

            dtpRequiredDate.CustomFormat = utilities.getDateFormat();

            if (dtpRequiredDate.CustomFormat == null)
                dtpRequiredDate.CustomFormat = "dd-MMM-yyyy";

            dtpRequiredDate.Value = selectedDate;

            //for changing grid rows required by date on header date change
            if (dgvProductList != null && dgvProductList.Rows.Count > 0)
            {
                for (int i = 0; i < dgvProductList.Rows.Count; i++)
                {
                    if (dgvProductList.Rows[i].Cells["Code"].Value != null)
                    {
                        dgvProductList.Rows[i].Cells["RequiredByDate"].Value = dtpRequiredDate.Value.Date;
                    }
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Add document type name on Cell value changed
        /// </summary>
        private void dgvSearchRequisitions_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (e.ColumnIndex < 0 || e.RowIndex < 0)
                {
                    log.LogMethodExit(null, "Invalid cell");
                    return;
                }
                if (dgvSearchRequisitions.Columns[e.ColumnIndex].Name == "RequisitionType")
                {
                    int rowIndex = dgvSearchRequisitions.CurrentCell.RowIndex;
                    int Id = Convert.ToInt32(dgvSearchRequisitions["RequisitionType", rowIndex].Value);
                    InventoryDocumentTypeList documentType = new InventoryDocumentTypeList(executionContext);
                    InventoryDocumentTypeDTO typeObject = documentType.GetInventoryDocumentType(Id);
                    if (typeObject != null)
                    {
                        dgvSearchRequisitions["RequisitionType", rowIndex].Value = typeObject.Name;
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

        /// <summary>
        /// when remarks label clicked to add Remarks
        /// </summary>
        private void lnkLblAddRemarks_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (!string.IsNullOrEmpty(txtRequisitionId.Text) || !string.IsNullOrEmpty(txtReqNumber.Text))
            {
                frmInventoryNotes frmInventoryNotes = new frmInventoryNotes(Convert.ToInt32(txtRequisitionId.Text), "InventoryRequisition", utilities);
                frmInventoryNotes.StartPosition = FormStartPosition.CenterScreen;//Added to show at center on 25-Aug-2016
                frmInventoryNotes.ShowDialog();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// advance search button clicked
        /// </summary>
        AdvancedSearch frmAdvancedSearch;
        private void btnAdvSearch_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (frmAdvancedSearch == null)
            {
                frmAdvancedSearch = new AdvancedSearch(utilities, "Product", "P");
            }
            if (frmAdvancedSearch.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    PopulateGrid();
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    MessageBox.Show(ex.Message);
                }
            }
            log.LogMethodExit();
        }

        private void frmRequisition_Activated(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            BarcodeReader.setReceiveAction = serialbarcodeDataReceived;
            LoadUOMComboBox();
            log.LogMethodExit();
        }

        private void cmbRequisitionStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (!string.IsNullOrEmpty(reqSelectedStatus))
                {
                    string selectedValue = cmbRequisitionStatus.SelectedItem.ToString();

                    //cannot submit the requisition which expired Expected receive date
                    if (reqSelectedStatus == "Open")
                    {
                        if (selectedValue == "Submitted")
                        {
                            if (dtpRequiredDate.CustomFormat != " " && dtpRequiredDate.Value != DateTime.MinValue)
                            {
                                DateTime currentDate = DateTime.Now.Date;
                                DateTime selectedDate = dtpRequiredDate.Value.Date;
                                if (selectedDate < currentDate)
                                {
                                    cmbRequisitionStatus.SelectedItem = reqSelectedStatus;
                                    MessageBox.Show("Expected received date has been expired !!! please reset the date ");
                                    log.LogMethodExit();
                                    return;
                                }
                            }
                        }
                    }

                    //if requisition submitted then only option allow to change Canceled 
                    if (reqSelectedStatus == "Submitted")
                    {
                        if (selectedValue == "Cancelled")
                        {
                            cmbRequisitionStatus.SelectedItem = selectedValue;
                        }
                        else
                        {
                            if (selectedValue != "Submitted")
                            {
                                cmbRequisitionStatus.SelectedItem = reqSelectedStatus;
                                log.LogMethodExit();
                                return;
                            }
                        }
                    }

                    //if requisition canceled state cannot change any other states
                    if (reqSelectedStatus == "Cancelled")
                    {
                        cmbRequisitionStatus.SelectedItem = reqSelectedStatus;
                    }

                    //cannot change status In progress in requisition form, its will only changed through issue
                    if (selectedValue == "InProgress")
                    {
                        cmbRequisitionStatus.SelectedItem = reqSelectedStatus;
                        log.LogMethodExit();
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void dgvProductList_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry(sender, e);

            MessageBox.Show("Error in Product grid data at row " + (e.RowIndex + 1).ToString() + ", Column " + dgvProductList.Columns[e.ColumnIndex].DataPropertyName +
                ": " + e.Exception.Message);
            e.Cancel = true;

            log.LogMethodExit();
        }

        private void dtpRequiredDate_KeyDown(object sender, KeyEventArgs e)
        {
            log.LogMethodEntry();
            e.SuppressKeyPress = true;
            log.LogMethodExit();
        }

        private void dgvProductList_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (dgvProductList.Columns[e.ColumnIndex].Name == "RequestedQnty")
                {

                    double verifyDouble = 0;
                    if (Double.TryParse(e.FormattedValue.ToString(), out verifyDouble) == false)
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage(648));
                        e.Cancel = true;
                    }

                }
                if (dgvProductList.Rows.Count == 1 && dgvProductList.Rows[0].Cells["Code"].Value == null)
                {
                    inventoryRequisitionListBS = new BindingSource();
                    inventoryRequisitionListBS.DataSource = new SortableBindingList<RequisitionLinesDTO>();
                    dgvProductList.DataSource = inventoryRequisitionListBS;
                    dgvProductList.EndEdit();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        // Print Option been added to frmRequisition as a enhancement on 11-04-2019
        private void btnPrint_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (string.IsNullOrEmpty(txtReqNumber.Text))
                {
                    log.LogMethodExit();
                    return;
                }

                POFileName = utilities.MessageUtils.getMessage("_RequisitionNumber_") + txtReqNumber.Text;
                string subject = POFileName + ".pdf";
                CreatePdfFile();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(utilities.MessageUtils.getMessage("Error while creating PDF", ex));
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Clears category combo box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_clear_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                cmbCategory.SelectedValue = -1;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit();
        }
        private void RemoveDGVRow()
        {
            log.LogMethodEntry(dgvselectedindex);
            try
            {
                if (dgvselectedindex > -1)
                {
                    dgvProductList.Rows.RemoveAt(dgvselectedindex);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void LoadUOMComboBox()
        {
            log.LogMethodEntry();
            try
            {
                int rowIndex = dgvProductList.CurrentRow.Index;
                BindingSource bindingSource = (BindingSource)dgvProductList.DataSource;
                SortableBindingList<RequisitionLinesDTO> requisitionLinesDTOList = (SortableBindingList<RequisitionLinesDTO>)bindingSource.DataSource;
                if (dgvProductList.Rows[rowIndex].Cells["cmbUOM"].Value == null)
                {
                    for (int i = 0; i < requisitionLinesDTOList.Count; i++)
                    {
                        int uomId = -1;
                        if (requisitionLinesDTOList[i].UOMId > -1)
                        {
                            uomId = requisitionLinesDTOList[i].UOMId;
                        }
                        else
                        {
                            uomId = ProductContainer.productDTOList.Find(x => x.ProductId == requisitionLinesDTOList[i].ProductId).UomId;
                        }
                        CommonFuncs.GetUOMComboboxForSelectedRows(dgvProductList, i, uomId);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void dgvProductList_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            log.LogMethodEntry();
            LoadUOMComboBox();
            log.LogMethodExit();
        }

        private void cmbToDept_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
