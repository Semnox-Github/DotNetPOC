/********************************************************************************************
 * Project Name - RequisitionTemplates
 * Description  - user interface to create templates
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        08-Aug-2016   Suneetha.S     Created 
 *2.70.2      22-Nov-2019   Deeksha        Inventory Next Rel Enhancement changes
 *2.100.0     07-Aug-2020   Deeksha        Modified :Added UOM drop down field to change related UOM.
 *2.100.1     10-Aug-2021   Deeksha        Modified :Issue Fix: Template screen disabled to perform actions.
 *2.110.00    16-12-2020    Abhishek       Modified : added dto reference
 *********************************************************************************************/
using System;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Category;
using Semnox.Parafait.Product;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;

namespace Semnox.Parafait.Inventory
{
    /// <summary>
    /// Parafait Inventory Requisition Template
    /// </summary>
    public partial class frmRequisitionTemplate : Form
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int dgvselectedindex;
        private bool fireCellValueChange = true;
        private PrintDocument MyPrintDocument;
        private DataGridViewPrinter MyDataGridViewPrinter;
        private DataGridView printdgv = new DataGridView();
        private BindingSource inventoryProductListBS;
        private BindingSource inventoryReqTemplatesListBS;
        private Utilities utilities = new Utilities();
        private List<CategoryDTO> categoryListOnDisplay;
        private List<ProductDTO> inventoryProductListOnDisplay;
        private ExecutionContext machineUserContext = null;
        private string POFileName;

        /// <summary>
        /// start form initialization
        /// </summary>
        public frmRequisitionTemplate()
        {
            log.LogMethodEntry();
            InitializeComponent();
            machineUserContext = ExecutionContext.GetExecutionContext();
            utilities.setLanguage(this); //added on 26-Jul-2017
            log.LogMethodExit();
        }

        /// <summary>
        /// Requisition Template Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmRequisitionTemplate_Load(object sender, EventArgs e)
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

            cmbTemplateStatus.SelectedItem = "Open";

            LoadRequisitionTypes();
            LoadProductCategories();
            PopulateTemplateGrid();

            txtTemplateName.Focus();
            cmbReqType.Focus();
            dgvProductList.Columns["Code"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgvProductList.Columns["Code"].Width = 120;
            dgvProductList.Columns["Description"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dgvProductList.Columns["Description"].ReadOnly = true;
            dgvProductList.Columns["Description"].Width = 506;
            dgvProductList.Columns["cmbUOM"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgvProductList.Columns["cmbUOM"].Width = 80;
            dgvProductList.Columns["Category"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgvProductList.Columns["Category"].Width = 120;
            if (!string.IsNullOrEmpty(txtReqTemplateId.Text))
                lnkLblAddRemarks.Enabled = true;
            else
                lnkLblAddRemarks.Enabled = false;

            log.LogMethodExit();
        }

        /// <summary>
        /// start form initialization
        /// </summary>
        /// <param name="_Utilities">Utilities</param>
        public frmRequisitionTemplate(Utilities _Utilities)
        {
            log.LogMethodEntry(_Utilities);
            InitializeComponent();
            machineUserContext = _Utilities.ExecutionContext;
            utilities = _Utilities;
            utilities.setLanguage(this);
            utilities.setupDataGridProperties(ref dgvProductList);
            utilities.setupDataGridProperties(ref dgvSearchRequisitions);
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
                InventoryDocumentTypeList inventoryDocumentTypeList = new InventoryDocumentTypeList(machineUserContext);
                List<KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>> inventoryDocumentTypeSearchParams = new List<KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>>();
                inventoryDocumentTypeSearchParams.Add(new KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>(InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters.ACTIVE_FLAG, "1"));
                inventoryDocumentTypeSearchParams.Add(new KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>(InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters.APPLICABILITY, "RQ"));
                inventoryDocumentTypeSearchParams.Add(new KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>(InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
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

                cmbReqTypeSearch.DataSource = bindingSourceSearchReqTypes;
                cmbReqTypeSearch.ValueMember = "DocumentTypeId";
                cmbReqTypeSearch.DisplayMember = "Name";
            }
            catch (Exception ex)
            {
                log.Error("Error while loading document types  in LoadRequisitionTypes() method", ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Loading Inventory Locations
        /// </summary>
        private void LoadInventoryLocations()
        {
            log.LogMethodEntry();
            try
            {
                LocationList inventoryLocationList = new LocationList(machineUserContext);
                List<KeyValuePair<LocationDTO.SearchByLocationParameters, string>> inventoryLocationSearchParams = new List<KeyValuePair<LocationDTO.SearchByLocationParameters, string>>();

                inventoryLocationSearchParams.Add(new KeyValuePair<LocationDTO.SearchByLocationParameters, string>(LocationDTO.SearchByLocationParameters.IS_ACTIVE, "Y"));
                inventoryLocationSearchParams.Add(new KeyValuePair<LocationDTO.SearchByLocationParameters, string>(LocationDTO.SearchByLocationParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));

                List<LocationDTO> inventoryLocationListOnDisplay = inventoryLocationList.GetAllLocations(inventoryLocationSearchParams);

                BindingSource inventoryReqDeptListBS = new BindingSource();
                BindingSource inventoryFromDeptListBS = new BindingSource();
                BindingSource inventoryToDeptListBS = new BindingSource();
                if (inventoryLocationListOnDisplay != null)
                {
                    inventoryLocationListOnDisplay.Insert(0, new LocationDTO());
                    SortableBindingList<LocationDTO> inventoryLocationDTOSortList = new SortableBindingList<LocationDTO>(inventoryLocationListOnDisplay);
                    inventoryReqDeptListBS.DataSource = inventoryLocationDTOSortList;
                    inventoryFromDeptListBS.DataSource = inventoryLocationDTOSortList;
                    inventoryToDeptListBS.DataSource = inventoryLocationDTOSortList;
                }
                else
                {
                    inventoryReqDeptListBS.DataSource = new SortableBindingList<LocationDTO>();
                    inventoryFromDeptListBS.DataSource = new SortableBindingList<LocationDTO>();
                    inventoryToDeptListBS.DataSource = new SortableBindingList<LocationDTO>();
                }

                cmbRequestingDept.DataSource = inventoryReqDeptListBS;
                cmbRequestingDept.ValueMember = "LocationId";
                cmbRequestingDept.DisplayMember = "Name";

                cmbFromDept.DataSource = inventoryFromDeptListBS;
                cmbFromDept.ValueMember = "LocationId";
                cmbFromDept.DisplayMember = "Name";

                cmbToDept.DataSource = inventoryToDeptListBS;
                cmbToDept.ValueMember = "LocationId";
                cmbToDept.DisplayMember = "Name";
            }
            catch (Exception ex)
            {
                log.Error("Error while loading Inventory Locations in LoadInventoryLocations() method", ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// populate products into grid
        /// </summary>
        private void PopulateGrid()
        {
            log.LogMethodEntry();
            try
            {
                ProductList inventoryProductList = new ProductList();
                List<KeyValuePair<ProductDTO.SearchByProductParameters, string>> inventoryProductSearchParams = new List<KeyValuePair<ProductDTO.SearchByProductParameters, string>>();

                inventoryProductSearchParams.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.IS_ACTIVE, "Y"));
                inventoryProductSearchParams.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));

                if (!string.IsNullOrEmpty(txtProdBarcode.Text))
                { 
                    RequisitionList requisitionList = new RequisitionList(machineUserContext);
                    int ProductId = requisitionList.GetProductId(txtProdBarcode.Text.ToString().Trim());
                    inventoryProductSearchParams.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.PRODUCT_ID, ProductId.ToString()));
                }

                if (!string.IsNullOrEmpty(txtProdcode.Text))
                    inventoryProductSearchParams.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.CODE_EXACT_MATCH, txtProdcode.Text.ToString()));

                if (cmbProdCategory.SelectedValue != null)
                {
                    if (!string.IsNullOrEmpty(cmbProdCategory.SelectedValue.ToString()) && cmbProdCategory.SelectedValue.ToString() != "-1")
                        inventoryProductSearchParams.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.CATEGORY, cmbProdCategory.SelectedValue.ToString()));
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
                    }
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
                        RequisitionTemplateLinesDTO lines;
                        BindingSource source = (BindingSource)dgvProductList.DataSource;
                        for (int i = 0; i < inventoryProductListOnDisplay.Count; i++)
                        {
                            lines = new RequisitionTemplateLinesDTO(-1,-1, inventoryProductListOnDisplay[i].ProductId, inventoryProductListOnDisplay[i].Code,
                                inventoryProductListOnDisplay[i].Description , 0, DateTime.MinValue, inventoryProductListOnDisplay[i].IsActive ,
                                string.Empty ,string.Empty , inventoryProductListOnDisplay[i].UOMValue , string.Empty , inventoryProductListOnDisplay[i].UomId);
                            source.Add(lines);
                        }
                        dgvProductList.DataSource = source;
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
                        inventoryProductListBS.DataSource = new SortableBindingList<ProductDTO>();

                    dgvProductList.DataSource = inventoryProductListBS;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error while populating products into grid - in PopulateGrid() method", ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// populate Templates into grid
        /// </summary>
        private void PopulateTemplateGrid()
        {
            log.LogMethodEntry();
            try
            {
                RequisitionTemplateList inventoryReqTemplatesList = new RequisitionTemplateList(machineUserContext);
                List<KeyValuePair<RequisitionTemplatesDTO.SearchByReqTemplatesTypeParameters, string>> inventoryReqTemplatesSearchParams = new List<KeyValuePair<RequisitionTemplatesDTO.SearchByReqTemplatesTypeParameters, string>>();
                inventoryReqTemplatesSearchParams.Add(new KeyValuePair<RequisitionTemplatesDTO.SearchByReqTemplatesTypeParameters, string>(RequisitionTemplatesDTO.SearchByReqTemplatesTypeParameters.SITEID, machineUserContext.GetSiteId().ToString()));
                if (chkSearchIsActive.Checked)
                    inventoryReqTemplatesSearchParams.Add(new KeyValuePair<RequisitionTemplatesDTO.SearchByReqTemplatesTypeParameters, string>(RequisitionTemplatesDTO.SearchByReqTemplatesTypeParameters.ACTIVE_FLAG, "1"));
                else
                    inventoryReqTemplatesSearchParams.Add(new KeyValuePair<RequisitionTemplatesDTO.SearchByReqTemplatesTypeParameters, string>(RequisitionTemplatesDTO.SearchByReqTemplatesTypeParameters.ACTIVE_FLAG, "0"));


                if (!string.IsNullOrEmpty(txtTemplateNamesearch.Text))
                    inventoryReqTemplatesSearchParams.Add(new KeyValuePair<RequisitionTemplatesDTO.SearchByReqTemplatesTypeParameters, string>(RequisitionTemplatesDTO.SearchByReqTemplatesTypeParameters.TEMPLATE_NAME, txtTemplateNamesearch.Text.ToString()));

                if (!string.IsNullOrEmpty(txtTemplateId.Text))
                    inventoryReqTemplatesSearchParams.Add(new KeyValuePair<RequisitionTemplatesDTO.SearchByReqTemplatesTypeParameters, string>(RequisitionTemplatesDTO.SearchByReqTemplatesTypeParameters.TEMPLATE_ID, txtTemplateId.Text.ToString()));

                if (cmbSearchStatus.SelectedItem != null)
                {
                    if (!string.IsNullOrEmpty(cmbSearchStatus.SelectedItem.ToString()) && cmbSearchStatus.SelectedItem.ToString() != "-1")
                        inventoryReqTemplatesSearchParams.Add(new KeyValuePair<RequisitionTemplatesDTO.SearchByReqTemplatesTypeParameters, string>(RequisitionTemplatesDTO.SearchByReqTemplatesTypeParameters.STATUS, cmbSearchStatus.SelectedItem.ToString()));
                }

                if (cmbReqTypeSearch.SelectedValue != null)
                {
                    if (!string.IsNullOrEmpty(cmbReqTypeSearch.SelectedValue.ToString()) && cmbReqTypeSearch.SelectedValue.ToString() != "-1")
                        inventoryReqTemplatesSearchParams.Add(new KeyValuePair<RequisitionTemplatesDTO.SearchByReqTemplatesTypeParameters, string>(RequisitionTemplatesDTO.SearchByReqTemplatesTypeParameters.REQUISITION_TYPE, cmbReqTypeSearch.SelectedValue.ToString()));
                }

                List<RequisitionTemplatesDTO> inventoryProductListOnDisplay = inventoryReqTemplatesList.GetAllTemplates(inventoryReqTemplatesSearchParams);

                inventoryReqTemplatesListBS = new BindingSource();
                if (inventoryProductListOnDisplay != null)
                {
                    SortableBindingList<RequisitionTemplatesDTO> inventoryProductDTOSortList = new SortableBindingList<RequisitionTemplatesDTO>(inventoryProductListOnDisplay);
                    inventoryReqTemplatesListBS.DataSource = inventoryProductDTOSortList;
                }
                else
                {
                    inventoryReqTemplatesListBS.DataSource = new SortableBindingList<RequisitionTemplatesDTO>();
                    ClearTemplateHeaderAndLines();
                    ClearSearchFields();
                }
                inventoryReqTemplatesListBS.AddingNew += dgvReqtemplatesList_BindingSourceAddNew;
                dgvSearchRequisitions.DataSource = inventoryReqTemplatesListBS;
            }
            catch (Exception ex)
            {
                log.Error("Error while loading templates into grid - in PopulateTemplateGrid() method", ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Populates product Category.
        /// </summary>
        private void LoadProductCategories()
        {
            log.LogMethodEntry();
            try
            {
                log.LogMethodEntry();
                CategoryList categoryList = new CategoryList(machineUserContext);
                List<KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>> categoryDTOSearchParams = new List<KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>>();
                categoryDTOSearchParams.Add(new KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>(CategoryDTO.SearchByCategoryParameters.IS_ACTIVE, "Y"));
                categoryListOnDisplay = categoryList.GetAllCategory(categoryDTOSearchParams);
                if (categoryListOnDisplay == null)
                {
                    categoryListOnDisplay = new List<CategoryDTO>();
                }
                categoryListOnDisplay = categoryListOnDisplay.OrderBy(o => o.Name).ToList();

                categoryListOnDisplay.Insert(0, new CategoryDTO());

                categoryListOnDisplay[0].Name = "<ALL>";
                cmbCategory.DataSource = categoryListOnDisplay;
                cmbCategory.ValueMember = "CategoryId";
                cmbCategory.DisplayMember = "Name";
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while populating Category", ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// insert template lines into to table
        /// </summary>
        private void InsertTemplateLines(RequisitionTemplatesDTO requisitionTemplatesDTO, bool isNewTemplate)
        {
            log.LogMethodEntry(requisitionTemplatesDTO, isNewTemplate);
            try
            {
                string status = string.Empty;
                RequisitionTemplateLinesDTO requisitionTemplateLinesDTO = null;
                BindingSource productsListBS = (BindingSource)dgvProductList.DataSource;
                var productListOnDisplay = (SortableBindingList<RequisitionTemplateLinesDTO>)productsListBS.DataSource;
                for (int i = 0; i< productListOnDisplay.Count; i++)
                {
                    requisitionTemplateLinesDTO = productListOnDisplay[i];
                    if (requisitionTemplateLinesDTO.ProductId > 0)
                    {
                        if (cmbTemplateStatus.SelectedIndex != -1)
                            status = cmbTemplateStatus.SelectedItem.ToString();

                        requisitionTemplateLinesDTO.UOMId = Convert.ToInt32(dgvProductList.Rows[i].Cells["cmbUOM"].Value);
                        requisitionTemplateLinesDTO.RequiredByDate = ServerDateTime.Now.Date;

                        //add template Id number
                        if (!string.IsNullOrEmpty(txtReqTemplateId.Text))
                            requisitionTemplateLinesDTO.TemplateId = Convert.ToInt32(txtReqTemplateId.Text);
                        else
                        {
                            log.LogMethodExit();
                            return;
                        }

                        requisitionTemplateLinesDTO.Status = status;
                        if (requisitionTemplatesDTO.RequisitionTemplateLinesListDTO == null)
                        {
                            requisitionTemplatesDTO.RequisitionTemplateLinesListDTO = new List<RequisitionTemplateLinesDTO>();
                        }
                        requisitionTemplatesDTO.RequisitionTemplateLinesListDTO.Add(requisitionTemplateLinesDTO);
                    }
                }
                RequisitionTemplatesBL requisitionLines = new RequisitionTemplatesBL(machineUserContext, requisitionTemplatesDTO);
                requisitionLines.Save(null);
            }
            catch (Exception ex)
            {
                log.Error("Error while inserting template lines InsertTemplateLines() method", ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// populate template lines (template products details) into grid on template index change
        /// </summary>
        private void PopulateTemplateLines(int templateId)
        {
            log.LogMethodEntry(templateId);
            try
            {
                RequisitionTemplateLinesList inventoryReqTemplateLineList = new RequisitionTemplateLinesList(machineUserContext);
                List<KeyValuePair<RequisitionTemplateLinesDTO.SearchByTemplateLinesParameters, string>> reqTemplateLinesSearchParams = new List<KeyValuePair<RequisitionTemplateLinesDTO.SearchByTemplateLinesParameters, string>>();

                reqTemplateLinesSearchParams.Add(new KeyValuePair<RequisitionTemplateLinesDTO.SearchByTemplateLinesParameters, string>(RequisitionTemplateLinesDTO.SearchByTemplateLinesParameters.ACTIVE_FLAG, "1"));

                reqTemplateLinesSearchParams.Add(new KeyValuePair<RequisitionTemplateLinesDTO.SearchByTemplateLinesParameters, string>(RequisitionTemplateLinesDTO.SearchByTemplateLinesParameters.TEMPLATE_ID, templateId.ToString()));

                List<RequisitionTemplateLinesDTO> reqTemplateLinesOnDisplay = inventoryReqTemplateLineList.GetAllTemplateLines(reqTemplateLinesSearchParams);

                BindingSource reqTemplateLinesBS = new BindingSource();
                if (reqTemplateLinesOnDisplay != null)
                {
                    SortableBindingList<RequisitionTemplateLinesDTO> inventoryProductDTOSortList = new SortableBindingList<RequisitionTemplateLinesDTO>(reqTemplateLinesOnDisplay);
                    reqTemplateLinesBS.DataSource = inventoryProductDTOSortList;
                }
                else
                    reqTemplateLinesBS.DataSource = new SortableBindingList<RequisitionTemplateLinesDTO>();
                dgvProductList.DataSource = reqTemplateLinesBS;
                for (int i = 0; i < reqTemplateLinesOnDisplay.Count; i++)
                {
                    int uomId = -1;
                    if (reqTemplateLinesOnDisplay[i].UOMId > -1)
                    {
                        uomId = reqTemplateLinesOnDisplay[i].UOMId;
                    }
                    else
                    {
                        uomId = ProductContainer.productDTOList.Find(x => x.ProductId == reqTemplateLinesOnDisplay[i].ProductId).UomId;
                    }
                    CommonFuncs.GetUOMComboboxForSelectedRows(dgvProductList, i, uomId);
                }
            }
            catch (Exception ex)
            {
                log.Error("Error while loading template lines PopulateTemplateLines() method", ex);
            }
            log.LogMethodExit();
        }
        private void ShowOrHideControls(bool showHide)
        {
            log.LogMethodEntry();
            dgvProductList.Enabled =
            gb_Requisition.Enabled =
            gbItemDetails.Enabled =
            cmbTemplateStatus.Enabled =
            cmbReqType.Enabled =
            cmbFromDept.Enabled =
            cmbToDept.Enabled =
            cmbRequestingDept.Enabled =
            cmbReqType.Enabled =
            chkIsActive.Enabled =
            txtTemplateName.Enabled =
            txtReqTemplateId.Enabled = showHide;
            log.LogMethodExit();
        }



        /// <summary>
        /// Displaying template details for selected template in grid
        /// </summary>
        private void DisplayTemplateDetails(int templateId)
        {
            log.LogMethodEntry(templateId);
            try
            {
                RequisitionTemplateList reqTemplateDetails = new RequisitionTemplateList(machineUserContext);
                RequisitionTemplatesDTO reqTemplateHeaderDetails = reqTemplateDetails.GetTemplate(templateId);
                if (reqTemplateHeaderDetails != null)
                {
                    txtReqTemplateId.Text = reqTemplateHeaderDetails.TemplateId.ToString();
                    txtTemplateName.Text = reqTemplateHeaderDetails.TemplateName.ToString();
                    chkIsActive.Checked = Convert.ToBoolean(reqTemplateHeaderDetails.IsActive);
                    cmbReqType.SelectedItem = (InventoryDocumentTypeDTO)GetComboBoxItemIndex(cmbReqType, Convert.ToInt32(reqTemplateHeaderDetails.RequisitionType));
                    cmbRequestingDept.SelectedItem = (LocationDTO)GetLocationComboBoxItemIndex(cmbRequestingDept, Convert.ToInt32(reqTemplateHeaderDetails.RequestingDept));
                    cmbFromDept.SelectedItem = (LocationDTO)GetLocationComboBoxItemIndex(cmbFromDept, Convert.ToInt32(reqTemplateHeaderDetails.FromDepartment));
                    cmbToDept.SelectedItem = (LocationDTO)GetLocationComboBoxItemIndex(cmbToDept, Convert.ToInt32(reqTemplateHeaderDetails.ToDepartment));
                    cmbTemplateStatus.SelectedItem = Convert.ToString(reqTemplateHeaderDetails.Status);
                    if(reqTemplateHeaderDetails.Status == "Closed")
                    {
                        ShowOrHideControls(false);
                    }
                    else
                    {
                        ShowOrHideControls(true);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Error while displaying template details in PopulateTemplateLines() method", ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// to get combo box selected index id
        /// </summary>
        private InventoryDocumentTypeDTO GetComboBoxItemIndex(ComboBox combobox, int selectedId)
        {
            log.LogMethodEntry(combobox, selectedId);
            InventoryDocumentTypeDTO typeObject = null;
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
                            typeObject = new InventoryDocumentTypeDTO();
                            typeObject = item;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Error while getting the combo box selected index id  in GetComboBoxItemIndex() method", ex);
            }
            log.LogMethodExit(typeObject);
            return typeObject;
        }

        /// <summary>
        /// to get combo box selected index id
        /// </summary>
        private LocationDTO GetLocationComboBoxItemIndex(ComboBox combobox, int selectedId)
        {
            log.LogMethodEntry(combobox, selectedId);
            LocationDTO locationObj = null;
            try
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
            catch (Exception ex)
            {
                log.Error("Error while getting the Location combo box selected index id  in GetLocationComboBoxItemIndex() method", ex);
            }
            log.LogMethodExit(locationObj);
            return locationObj;
        }

        /// <summary>
        /// 
        /// </summary>
        private void dgvProductList_BindingSourceAddNew(object sender, AddingNewEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (dgvProductList.Rows.Count == inventoryProductListBS.Count)
                {
                    inventoryProductListBS.RemoveAt(inventoryProductListBS.Count - 1);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// to get combo box selected index id
        /// </summary>
        private void dgvReqtemplatesList_BindingSourceAddNew(object sender, AddingNewEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (dgvSearchRequisitions.Rows.Count == inventoryReqTemplatesListBS.Count)
                {
                    inventoryReqTemplatesListBS.RemoveAt(inventoryReqTemplatesListBS.Count - 1);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// form close
        /// </summary>
        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                Close();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// click on product search button
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
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// save template clicked
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            bool isNewTemplate = false;
            lblMessage.Text = "";
            try
            {
                RequisitionTemplatesBL reqTemplateBL;
                RequisitionTemplatesDTO reqTemplateDTO = new RequisitionTemplatesDTO();

                if (string.IsNullOrEmpty(txtReqTemplateId.Text))
                {
                    isNewTemplate = true;
                    reqTemplateDTO.TemplateId = -1;
                }
                else
                {
                    reqTemplateDTO.TemplateId = Convert.ToInt32(txtReqTemplateId.Text);
                }


                if (!string.IsNullOrEmpty(txtTemplateName.Text))
                    reqTemplateDTO.TemplateName = txtTemplateName.Text;
                else
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(1072));
                    log.LogMethodExit();
                    return;
                }

                if (cmbReqType.SelectedIndex > 0)
                    reqTemplateDTO.RequisitionType = Convert.ToInt32(cmbReqType.SelectedValue);
                else
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(1065));
                    log.LogMethodExit();
                    return;
                }

                if (cmbRequestingDept.SelectedIndex == -1 || cmbRequestingDept.SelectedIndex == 0)
                {
                    if (cmbReqType.SelectedItem != null)
                    {
                        InventoryDocumentTypeDTO documentType = (InventoryDocumentTypeDTO)cmbReqType.SelectedItem;
                        if (documentType.Code == "MLRQ")
                        {
                            MessageBox.Show(utilities.MessageUtils.getMessage(1066));
                            log.LogMethodExit();
                            return;
                        }
                    }
                    else
                    {
                        reqTemplateDTO.RequestingDept = -1;
                    }

                }
                else
                {
                    reqTemplateDTO.RequestingDept = Convert.ToInt32(cmbRequestingDept.SelectedValue);
                }


                //from date
                LocationDTO locationFromObj = (LocationDTO)cmbFromDept.SelectedItem;
                string fromDeptName = string.Empty;
                string todeptname = string.Empty;
                if (locationFromObj != null)
                {
                    if (cmbFromDept.SelectedIndex > 0)
                    {
                        reqTemplateDTO.FromDepartment = Convert.ToInt32(cmbFromDept.SelectedValue);
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
                        reqTemplateDTO.ToDepartment = Convert.ToInt32(cmbToDept.SelectedValue);
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

                if (!string.IsNullOrEmpty(fromDeptName) && !string.IsNullOrEmpty(todeptname))
                {
                    if (fromDeptName.Trim() == todeptname.Trim())
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage(1071));
                        log.LogMethodExit();
                        return;
                    }
                }

                if (cmbTemplateStatus.SelectedIndex != -1)
                    reqTemplateDTO.Status = cmbTemplateStatus.SelectedItem.ToString();
                else
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(1073));
                    log.LogMethodExit();
                    return;
                }

                reqTemplateDTO.Remarks = string.Empty;

                reqTemplateDTO.RequiredByDate = ServerDateTime.Now.Date;

                if (chkIsActive.Checked)
                    reqTemplateDTO.IsActive = true;
                else
                    reqTemplateDTO.IsActive = false;

                reqTemplateBL = new RequisitionTemplatesBL(machineUserContext,reqTemplateDTO);
                reqTemplateBL.Save(null);

                if (reqTemplateDTO.TemplateId != -1)
                {
                    txtReqTemplateId.Text = reqTemplateDTO.TemplateId.ToString();
                    InsertTemplateLines(reqTemplateDTO, isNewTemplate);

                    PopulateTemplateGrid();
                    if (dgvSearchRequisitions != null && dgvSearchRequisitions.Rows.Count > 0)
                    {
                        DataGridViewRow row = dgvSearchRequisitions.Rows.Cast<DataGridViewRow>().Where(r => r.Cells["TemplateId"].Value.Equals(reqTemplateDTO.TemplateId)).First();
                        if (row != null)
                        {
                            dgvSearchRequisitions.Rows[row.Index].Selected = true;
                        }
                    }
                    DisplayTemplateDetails(reqTemplateDTO.TemplateId);
                }
                lblMessage.Text = utilities.MessageUtils.getMessage(1096);
            }
            catch (Exception ex)
            {
                log.Error("Error while saving template in Starts-btnSave_Click() event.", ex);
            }
            log.LogMethodExit();

        }

        /// <summary>
        /// On selected index changed in product grid
        /// </summary>
        private void dgvProductList_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            if (e.ColumnIndex < 0 || e.RowIndex < 0)
            {
                log.LogMethodExit(null, "Invalid cell");
                return;
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
                        query = "(Isnull(code,'') like " + "N'%" + dgvProductList[e.ColumnIndex, e.RowIndex].Value.ToString() + "%'" + " or Isnull(Description,'') like " + "N'%" + dgvProductList[e.ColumnIndex, e.RowIndex].Value.ToString() + "%')";
                    }
                    if (cmbReqType.SelectedIndex > 0)
                    {
                        InventoryDocumentTypeDTO documentTypeDTO = (InventoryDocumentTypeDTO)cmbReqType.SelectedItem;
                        if (documentTypeDTO != null)
                        {
                            if (documentTypeDTO.Code == "MLRQ")
                            {
                                query += " and Isnull(MarketListItem, 0) = 1";
                            }
                            else
                            {
                                query += " and Isnull(MarketListItem, 0) = 0";
                            }
                        }
                    }

                    query += " and isnull(IsActive, '')='Y'";

                    int categoryId = -1;
                    categoryId = Convert.ToInt32(cmbCategory.SelectedValue);
                    if (categoryId != -1)
                    {
                        query += " and p.CategoryId =@categoryId";
                        parameters.Add(new SqlParameter("@categoryId", categoryId));
                    }

                    RequisitionLineGenerics generics = new RequisitionLineGenerics();
                    inventoryProductListOnDisplay = inventoryProductList.GetProductList(query, parameters);

                    if (inventoryProductListOnDisplay == null || inventoryProductListOnDisplay.Any() == false)
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
                        dgvProductList["Product_ID", dgvselectedindex].Value = inventoryProductListOnDisplay[0].ProductId;
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
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// multiple_dgv_LostFocus
        /// </summary>
        void multiple_dgv_LostFocus(object sender, EventArgs e)
        {
            log.LogMethodEntry();
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
        /// grid clicked
        /// </summary>
        void multiple_dgv_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                DataGridView dg = (DataGridView)sender;
                fireCellValueChange = false;
                int categoryId = Convert.ToInt32(dg.Rows[dg.CurrentRow.Index].Cells["CategoryId"].Value);
                dgvProductList["Code", dgvselectedindex].Value = dg.Rows[dg.CurrentRow.Index].Cells["Code"].Value;
                dgvProductList["Description", dgvselectedindex].Value = dg.Rows[dg.CurrentRow.Index].Cells["Description"].Value;
                int uomId = Convert.ToInt32(dg.Rows[dg.CurrentRow.Index].Cells["UOMId"].Value);
                dgvProductList["UOMId", dgvselectedindex].Value = uomId;
                CommonFuncs.GetUOMComboboxForSelectedRows(dgvProductList, dgvselectedindex, uomId);
                dgvProductList["Product_ID", dgvselectedindex].Value = dg.Rows[dg.CurrentRow.Index].Cells["ProductId"].Value;
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
            log.LogMethodEntry();
            try
            {
                pnlMultiple_dgv.Size = new Size(300, (dgvProductList.Rows[0].Cells[0].Size.Height * 7) - 3); //Changed the function so that the grid is in a panel and scroll bar can be added if list is long
                pnlMultiple_dgv.AutoScroll = true;
                pnlMultiple_dgv.Location = new Point(155 + gb_Requisition.Location.X + dgvProductList.RowHeadersWidth + dgvProductList.CurrentRow.Cells["Code"].ContentBounds.Location.X, 197 + dgvProductList.Location.Y + dgvProductList.ColumnHeadersHeight);
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

                multiple_dgv.Columns["Description"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;

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

        /// <summary>
        /// button template search clicked
        /// </summary>
        private void btnSearchTemplates_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            { 
                PopulateTemplateGrid();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// template grid selected index changed
        /// </summary>
        private void dgvSearchRequisitions_SelectionChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                lblMessage.Text = string.Empty;
                if (dgvSearchRequisitions != null && dgvSearchRequisitions.Rows.Count > 0)
                {
                    int index;
                    try
                    {
                        index = dgvSearchRequisitions.SelectedRows[0].Index;
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        log.LogMethodExit();
                        return;
                    }
                    if (index < 0) //Header clicked
                    {
                        log.LogMethodExit();
                        return;
                    }

                    if (dgvSearchRequisitions["TemplateId", index].Value.ToString() == txtTemplateId.Text)
                    {
                        log.LogMethodExit();
                        return;
                    }

                    //ClearFields();
                    int templateId = Convert.ToInt32(dgvSearchRequisitions["TemplateId", index].Value);

                    if (templateId != -1)
                        lnkLblAddRemarks.Enabled = true;
                    else
                        lnkLblAddRemarks.Enabled = false;

                    DisplayTemplateDetails(templateId);
                    PopulateTemplateLines(templateId);
                    dgvSearchRequisitions.Rows[index].Selected = true;
                }
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// for loading bar code scanner
        /// </summary>
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
        /// for bar code scanner
        /// </summary>
        private void frmRequisitionTemplate_Activated(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            BarcodeReader.setReceiveAction = serialbarcodeDataReceived;
            log.LogMethodExit();
        }

        /// <summary>
        ///send email button clicked
        /// </summary>
        private void btnSendEmail_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            lblMessage.Text = "";

            POFileName = utilities.ParafaitEnv.SiteName + utilities.MessageUtils.getMessage("Requisition template number and name") + txtTemplateId.Text + " " + txtTemplateName.Text;
            string subject = POFileName + ".pdf";

            string body = "Hi " + "" + "," + Environment.NewLine + Environment.NewLine;
            body += "Please find attached Requisition " + txtTemplateName.Text + " for your immediate processing." + Environment.NewLine + Environment.NewLine;
            body += "Regards," + Environment.NewLine + utilities.ParafaitEnv.Username;
            string reportDir = utilities.getParafaitDefaults("PDF_OUTPUT_DIR") + "\\";

            SendEmailUI emailF = new SendEmailUI("", "", "", subject, body, POFileName + ".pdf", reportDir, false, utilities);
            emailF.ShowDialog();

            System.IO.FileInfo fi = new System.IO.FileInfo(reportDir + POFileName + ".pdf");
            try
            {
                fi.Delete();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// call print file 
        /// </summary>
        private void PrintFile()
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

        /// <summary>
        /// for allowing file print
        /// </summary>
        private void MyPrintDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            log.LogMethodEntry();
            bool more = MyDataGridViewPrinter.DrawDataGridView(e.Graphics);
            if (more == true)
                e.HasMorePages = true;
            log.LogMethodExit();
        }

        /// <summary>
        /// print setup
        /// </summary>
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
            PrintUtils.SetOutputFileName(utilities.getParafaitDefaults("PDF_OUTPUT_DIR") + "\\" + POFileName + " -." + "pdf");

            MyPrintDocument.DefaultPageSettings =
            MyPrintDialog.PrinterSettings.DefaultPageSettings;
            MyPrintDocument.DefaultPageSettings.Margins =
                             new Margins(20, 20, 20, 20);
            MyPrintDocument.PrinterSettings =
                                MyPrintDialog.PrinterSettings;
            MyPrintDocument.DocumentName = utilities.MessageUtils.getMessage("Requisition Template No ") + txtReqTemplateId.Text;
            string reportTitle = utilities.MessageUtils.getMessage("Requisition Template No") +
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

        /// <summary>
        /// format print setup
        /// </summary>
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
            printdgv.Columns.Add("col7", "col7");
            printdgv.ColumnHeadersVisible = false;
            printdgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            printdgv.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
            printdgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;

            index = printdgv.Rows.Add(utilities.MessageUtils.getMessage("Requisition template number: "), txtReqTemplateId.Text, "", "", "");
            printdgv.Rows[index].DefaultCellStyle.Font = new Font(this.Font, FontStyle.Bold);

            index = printdgv.Rows.Add(utilities.MessageUtils.getMessage("Requisition template Name: "), txtTemplateName.Text, "", "", "");
            printdgv.Rows[index].DefaultCellStyle.Font = new Font(this.Font, FontStyle.Bold);
            printdgv.Rows.Add("", "", "", "", "");

            printdgv.Rows.Add("", "", "", "", "");
            printdgv.Rows.Add("", "", "", "", "");

            bool PrintPrice = true;
            index = printdgv.Rows.Add(utilities.MessageUtils.getMessage("Product Code"), utilities.MessageUtils.getMessage("Description"), utilities.MessageUtils.getMessage("Quantity"), utilities.MessageUtils.getMessage("Required By"), (PrintPrice ? utilities.MessageUtils.getMessage("Unit Price") : ""));
            printdgv.Rows[index].DefaultCellStyle.BackColor = Color.Gray;
            printdgv.Rows[index].DefaultCellStyle.Font = new Font(this.Font, FontStyle.Bold);

            printdgv.Columns[2].DefaultCellStyle = utilities.gridViewAmountCellStyle();
            printdgv.Columns[3].DefaultCellStyle = utilities.gridViewDateCellStyle();
            printdgv.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            printdgv.Columns[4].DefaultCellStyle =
            printdgv.Columns[5].DefaultCellStyle =
            printdgv.Columns[6].DefaultCellStyle = utilities.gridViewAmountCellStyle();

            printdgv.Rows.Add("", "", "", "", "");
            for (int i = 0; i < dgvProductList.RowCount - 1; i++)
            {
                int j = printdgv.Rows.Add(dgvProductList["Code", i].Value.ToString(),
                                        dgvProductList["Description", i].Value.ToString().PadRight(30, ' ').Substring(0, 30),
                                        string.Format("{0:N0}", dgvProductList["RequsetedQnty", i].Value),
                                        dgvProductList["RequiredByDate", i].Value);

            }
            printdgv.Rows.Add("", "", "", "", "", "");

            string txt = string.Empty;
            int ind = 0;
            while (true)
            {
                string t = txt.PadRight(31, ' ').Substring(ind, 30);
                if (t.Trim() == "")
                    break;
                printdgv.Rows.Add("", t);
                txt = txt.PadRight(31, ' ').Substring(ind + 30);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// raised on row data error
        /// </summary>
        private void dgvProductList_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                MessageBox.Show("Error in ProductList grid data at row " + (e.RowIndex + 1).ToString() + ", Column " + dgvProductList.Columns[e.ColumnIndex].DataPropertyName +
                   ": " + e.Exception.Message);
                e.Cancel = true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// new button clicked to allow to add new template
        /// </summary>
        private void btnNew_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                ClearFields();
                ShowOrHideControls(true);
                cmbCategory.SelectedIndex = 0;
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// clear all existing fields value
        /// </summary>
        private void ClearFields()
        {
            log.LogMethodEntry();
            lblMessage.Text = string.Empty;
            ClearTemplateHeaderAndLines();
            ClearSearchFields();
            ClearTemplateSearchFields();
            log.LogMethodExit();
        }

        private void ClearTemplateSearchFields()
        {
            log.LogMethodEntry();
            txtTemplateId.Text = string.Empty;
            txtTemplateNamesearch.Text = string.Empty;
            cmbSearchStatus.SelectedIndex = -1;
            cmbReqTypeSearch.SelectedIndex = -1;
            log.LogMethodExit();
        }

        private void ClearSearchFields()
        {
            log.LogMethodEntry();
            txtProdcode.Text = string.Empty;
            txtProdBarcode.Text = string.Empty;
            cmbProdCategory.SelectedIndex = -1;
            log.LogMethodExit();
        }

        private void ClearTemplateHeaderAndLines()
        {
            log.LogMethodEntry();
            txtTemplateName.Text = string.Empty;
            txtReqTemplateId.Text = string.Empty;
            cmbReqType.SelectedIndex = -1;
            cmbRequestingDept.Enabled = true;
            cmbRequestingDept.SelectedIndex = -1;
            BindingSource cmbRequisitionRequestingDeptBS = new BindingSource();
            cmbRequisitionRequestingDeptBS.DataSource = new SortableBindingList<LocationDTO>();
            cmbRequestingDept.DataSource = cmbRequisitionRequestingDeptBS;

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

            cmbTemplateStatus.SelectedItem = "Open";

            BindingSource productBS = new BindingSource();
            productBS.DataSource = new SortableBindingList<RequisitionTemplateLinesDTO>();
            dgvProductList.DataSource = productBS;
            log.LogMethodExit();
        }


        /// <summary>
        /// clicked refresh button for remove unsaved data
        /// </summary>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                lblMessage.Text = string.Empty;
                RefreshPage();
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// refresh page, reloading previous data
        /// </summary>
        private void RefreshPage()
        {
            log.LogMethodEntry();
            try
            {
                if (!string.IsNullOrEmpty(txtReqTemplateId.Text))
                {
                    if (dgvSearchRequisitions != null && dgvSearchRequisitions.Rows.Count > 0)
                    {
                        DisplayTemplateDetails(Convert.ToInt32(txtReqTemplateId.Text));
                        PopulateTemplateLines(Convert.ToInt32(txtReqTemplateId.Text));
                    }
                    else
                    {
                        ClearTemplateHeaderAndLines();
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
                        id = dgvSearchRequisitions["TemplateId", reqgridSelectedIndex].Value.ToString();

                        DisplayTemplateDetails(Convert.ToInt32(id));
                        PopulateTemplateLines(Convert.ToInt32(id));
                    }
                    else
                    {
                        ClearTemplateHeaderAndLines();
                        ClearSearchFields();
                    }
                }

                txtTemplateId.Text = string.Empty;
                txtTemplateNamesearch.Text = string.Empty;
                txtProdcode.Text = string.Empty;
                txtProdBarcode.Text = string.Empty;
                cmbReqTypeSearch.SelectedIndex = -1;
                cmbSearchStatus.SelectedIndex = -1;
                chkSearchIsActive.Checked = true;
                cmbProdCategory.SelectedIndex = -1;
                txtTemplateName.Focus();
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// to make grid rows default values selected
        /// </summary>
        private void dgvProductList_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            log.LogMethodEntry();
            e.Row.Cells["RemoveLine"].Value = true;
            log.LogMethodExit();
        }

        /// <summary>
        /// document type on index selection changed
        /// </summary>
        private void cmbReqType_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                int typeId = Convert.ToInt32(cmbReqType.SelectedIndex);
                lblMessage.Text = string.Empty;
                if (typeId > 0)
                {
                    cmbReqType.Enabled = false;
                    LoadLocationsOnType();
                }
                else
                {
                    cmbReqType.Enabled = true;
                }
            }
            catch(Exception ex)
            {
                log.Error(ex);
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
                LocationList inventoryLocationList = new LocationList(machineUserContext);

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
                                fromLocationList = inventoryLocationList.GetLocationsOnLocationType("'" + "Store" + "'", machineUserContext.GetSiteId());
                                toLocationList = inventoryLocationList.GetLocationsOnLocationType("'" + "Purchase" + "'", machineUserContext.GetSiteId());
                            }
                            catch (Exception ex) { log.Error("Error in LoadLocationsOnType() event. load MLRG list", ex); }
                            break;

                        case "PURQ":
                            try
                            {
                                fromLocationList = inventoryLocationList.GetLocationsOnLocationType("'" + "Store" + "'", machineUserContext.GetSiteId());
                                toLocationList = inventoryLocationList.GetLocationsOnLocationType("'Purchase" + "','" + "Store'", machineUserContext.GetSiteId());
                            }
                            catch (Exception ex) { log.Error("Error in LoadLocationsOnType() event. load PURQ list", ex); }
                            break;
                        case "ISRQ":
                            try
                            {
                                fromLocationList = inventoryLocationList.GetLocationsOnLocationType("'Purchase" + "','" + "Store'", machineUserContext.GetSiteId());
                                toLocationList = inventoryLocationList.GetLocationsOnLocationType("'" + "Store" + "'", machineUserContext.GetSiteId());
                            }
                            catch (Exception ex) { log.Error("Error in LoadLocationsOnType() event. load ISRQ list", ex); }
                            break;
                    }
                    requestingLocationList = inventoryLocationList.GetLocationsOnLocationType("'Department'");
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
                cmbFromDept.DataSource = inventoryFromDeptListBS;
                cmbFromDept.ValueMember = "LocationId";
                cmbFromDept.DisplayMember = "Name";

                cmbToDept.DataSource = inventoryToDeptListBS;
                cmbToDept.ValueMember = "LocationId";
                cmbToDept.DisplayMember = "Name";

                cmbRequestingDept.DataSource = inventoryReqDeptListBS;
                cmbRequestingDept.ValueMember = "LocationId";
                cmbRequestingDept.DisplayMember = "Name";
            }
            catch (Exception ex)
            {
                log.Error("Error while loading locations into combo box", ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// when remarks label clicked to add Remarks
        /// </summary>
        private void lnkLblAddRemarks_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (!string.IsNullOrEmpty(txtReqTemplateId.Text))
                {
                    frmInventoryNotes frmInventoryNotes = new frmInventoryNotes(Convert.ToInt32(txtReqTemplateId.Text), "RequisitionTemplate", utilities);
                    frmInventoryNotes.StartPosition = FormStartPosition.CenterScreen;//Added to show at center on 25-Aug-2016
                    frmInventoryNotes.ShowDialog();
                }
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// advance search button clicked
        /// </summary>
        AdvancedSearch frmAdvancedSearch;
        private void btnAdvSearch_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
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
                    MessageBox.Show(ex.Message);
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Clears the screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_clear_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            cmbCategory.SelectedValue = -1;
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

        private void dgvProductList_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (dgvProductList.Rows.Count == 1 && dgvProductList.Rows[0].Cells["Code"].Value == null)
                {
                    inventoryReqTemplatesListBS = new BindingSource();
                    inventoryReqTemplatesListBS.DataSource = new SortableBindingList<RequisitionTemplateLinesDTO>();
                    dgvProductList.DataSource = inventoryReqTemplatesListBS;
                    dgvProductList.EndEdit();
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
            try
            {
                BindingSource productsListBS = (BindingSource)dgvProductList.DataSource;
                var requisitionTemplateOnDisplay = (SortableBindingList<RequisitionTemplateLinesDTO>)productsListBS.DataSource;
                for (int i = 0; i < requisitionTemplateOnDisplay.Count; i++)
                {
                    int uomId = -1;
                    if (requisitionTemplateOnDisplay[i].UOMId > -1)
                    {
                        uomId = requisitionTemplateOnDisplay[i].UOMId;
                    }
                    else
                    {
                        uomId = ProductContainer.productDTOList.Find(x => x.ProductId == requisitionTemplateOnDisplay[i].ProductId).UomId;
                    }
                    CommonFuncs.GetUOMComboboxForSelectedRows(dgvProductList, i, uomId);
                }
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
    }
}
