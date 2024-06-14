/********************************************************************************************
* Project Name - Inventory Lot
* Description  - Bussiness logic of Inventory Lot
* 
**************
**Version Log
**************
    *Version     Date          Modified By           Remarks          
*********************************************************************************************
*2.70.2       27-Nov-2019    Girish kundar       Modified: Adjustment quantity update issue fix 
*2.70.2       18-Dec-2019    Jinto Thomas        added parameter execution context for userrolrbl declaration with userid
*2.70.2       26-Dec-2019    Deeksha             Inventory Next-Rel Enhancement changes.
*2.80         01-Apr-2020    jinto Thomas        Added new search filters
*2.100.0      14-Aug-2020    Deeksha             Modified for Recipe Management enhancement.
*2.110.0      05-Feb-2021    Girish Kundar       Modified : UI fixes for resolution/messages/refresh UI 
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;
using Semnox.Parafait.User;

namespace Semnox.Parafait.Inventory
{
    public partial class frmInventoryAdjustments : Form
    {
        private Utilities utilities;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<EntityExclusionDetailDTO> entityExclusionDetailDTOList;
        private List<InventoryAdjustmentsSummaryDTO> inventoryAdjustmentsSummaryDTOList;

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="_Utilities"></param>
        public frmInventoryAdjustments(Utilities _Utilities)
        {
            log.LogMethodEntry(_Utilities);
            InitializeComponent();
            utilities = _Utilities;
            //ShowWaitScreen(); //Added this line to see that a message is shown when the form loads
            if (utilities.ParafaitEnv.IsCorporate)
            {
                executionContext.SetSiteId(utilities.ParafaitEnv.SiteId);
            }
            else
            {
                executionContext.SetSiteId(-1);
            }
            executionContext.SetUserId(utilities.ParafaitEnv.LoginID);
            utilities.setLanguage(this);
            UserRoles userRoles = new UserRoles(executionContext, Semnox.Parafait.Inventory.CommonFuncs.Utilities.ParafaitEnv.RoleId);
            entityExclusionDetailDTOList = userRoles.GetUIFieldsToHide("InventoryAdjustment");
            log.LogMethodExit();
        }

        Inventory inventoryBl;
        InventoryList inventoryList;
        InventoryAdjustmentsBL inventoryAdjustmentsBl;
        InventoryAdjustmentsDTO inventoryAdjustmentsDTO;
        Core.Utilities.ExecutionContext executionContext = Core.Utilities.ExecutionContext.GetExecutionContext();

        void populateLocation()
        {
            log.LogMethodEntry();
            LocationList locationList = new LocationList(executionContext);
            List<LocationDTO> excludeDepartmentLocationDTOList = new List<LocationDTO>();
            excludeDepartmentLocationDTOList = locationList.GetLocationsOnLocationType("'Store','Receive','Purchase','Adjustment'", executionContext.GetSiteId());

            if (excludeDepartmentLocationDTOList == null || excludeDepartmentLocationDTOList.Any() == false)
            {
                excludeDepartmentLocationDTOList = new List<LocationDTO>();
            }
            else
            {
                excludeDepartmentLocationDTOList = excludeDepartmentLocationDTOList.OrderBy(loc => loc.Name).ToList();
            }
            excludeDepartmentLocationDTOList.Insert(0, new LocationDTO());
            excludeDepartmentLocationDTOList[0].Name = "<Select>";
            BindingSource bindingSource = new BindingSource();
            bindingSource.DataSource = excludeDepartmentLocationDTOList;

            cbInvLocation.DataSource = bindingSource;
            cbInvLocation.DisplayMember = "Name";
            cbInvLocation.ValueMember = "LocationId";

            populateTransferLocation();
            log.LogMethodExit();
        }

        void populateTransferLocation()
        {
            log.LogMethodEntry();
            LocationList locationList = new LocationList(executionContext);
            List<LocationDTO> allLocationDTOList = new List<LocationDTO>();
            List<LocationDTO> excludeDepartmentLocationDTOList = new List<LocationDTO>();
            allLocationDTOList = locationList.GetLocationsOnLocationType("'Department','Store','Receive','Purchase','Adjustment'", executionContext.GetSiteId());
            allLocationDTOList = allLocationDTOList.OrderBy(loc => loc.Name).ToList();
            excludeDepartmentLocationDTOList = locationList.GetLocationsOnLocationType("'Store','Receive','Purchase','Adjustment'", executionContext.GetSiteId());
            excludeDepartmentLocationDTOList = excludeDepartmentLocationDTOList.OrderBy(loc => loc.Name).ToList();

            if (allLocationDTOList == null)
            {
                allLocationDTOList = new List<LocationDTO>();
            }
            allLocationDTOList.Insert(0, new LocationDTO());
            allLocationDTOList[0].Name = "<Select>";
            BindingSource allbindingSource = new BindingSource();
            allbindingSource.DataSource = allLocationDTOList;

            if (excludeDepartmentLocationDTOList == null)
            {
                excludeDepartmentLocationDTOList = new List<LocationDTO>();
            }
            excludeDepartmentLocationDTOList.Insert(0, new LocationDTO());
            excludeDepartmentLocationDTOList[0].Name = "<Select>";
            BindingSource bindingSource = new BindingSource();
            bindingSource.DataSource = excludeDepartmentLocationDTOList;

            if (rbFromLocation.Checked)
            {
                cbToLocation.DataSource = bindingSource;
                cbToLocation.DisplayMember = "Name";
                cbToLocation.ValueMember = "LocationId";
            }
            else
            {
                cbToLocation.DataSource = allbindingSource;
                cbToLocation.DisplayMember = "Name";
                cbToLocation.ValueMember = "LocationId";
            }
            log.LogMethodExit();
        }

        private void frmInventoryAdjustments_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                populateLocation();
                bindAdjustmentType();
                ProdSearch();
                chkSelect.DisplayIndex = 0;
                TransferQuantity.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                int pos1 = utilities.ParafaitEnv.INVENTORY_QUANTITY_FORMAT.IndexOf('.');
                if (pos1 >= 0)
                {
                    int pos2 = utilities.ParafaitEnv.INVENTORY_QUANTITY_FORMAT.Substring(pos1).Length - 1;
                    UpDownAdjQty.DecimalPlaces = pos2;
                }
                else
                    UpDownAdjQty.DecimalPlaces = 0;

                onLoadActions();
                utilities.setLanguage(this);
                dgvProducts.EditMode = DataGridViewEditMode.EditOnEnter;
                if (utilities.ParafaitEnv.IsCorporate)
                {
                    executionContext.SetSiteId(utilities.ParafaitEnv.SiteId);
                }
                else
                {
                    executionContext.SetSiteId(-1);
                }
                executionContext.SetUserId(utilities.ParafaitEnv.Username);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }


        void bindAdjustmentType()
        {
            log.LogMethodEntry();
            try
            {
                LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> LookupValuesSearchParameter = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                LookupValuesSearchParameter.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "INVENTORY_ADJUSTMENT_TYPE"));
                LookupValuesSearchParameter.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                List<LookupValuesDTO> lookupValueList = lookupValuesList.GetAllLookupValues(LookupValuesSearchParameter);
                if (lookupValueList == null)
                {
                    lookupValueList = new List<LookupValuesDTO>();
                }
                //To remove adjustment Type 'Wastage' from the list
                if (lookupValueList.Exists(x => x.LookupValue == "Wastage"))
                {
                    int index = lookupValueList.FindIndex(x => x.LookupValue == "Wastage");
                    lookupValueList.RemoveAt(index);
                }
                cbAdjustmentType.ValueMember = "LookupValueId";
                cbAdjustmentType.DisplayMember = "LookupValue";
                cbAdjustmentType.Text = "Adjustment";
                cbAdjustmentType.DataSource = lookupValueList;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void txtQuantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry(sender, e);

            if (!char.IsControl(e.KeyChar) && !char.IsNumber(e.KeyChar))
                e.Handled = true;

            log.LogMethodExit();
        }
        /// <summary>
        /// Validate if no product is selected while clicking adjust button
        /// </summary>
        /// <returns></returns>
        public bool ValidateCheckbox()
        {
            log.LogMethodEntry();
            bool isChecked = false;
            CheckBox checkBox = new CheckBox();
            foreach (DataGridViewRow dataGridRow in dgvProducts.Rows)
            {
                if (dataGridRow.Cells["chkSelect"].Value != null && dataGridRow.Cells["chkSelect"].Value.ToString() == "Y")
                {
                    isChecked = true;
                    break;
                }
            }
            log.LogMethodExit(isChecked);
            return isChecked;
        }

        private void btnAdjust_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);



            if (UpDownAdjQty.Value == 0)
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(2360), utilities.MessageUtils.getMessage("Validation Error"));//validate
                log.LogMethodExit("UpDownAdjQty.Value == 0");
                return;
            }
            try
            {
                bool valid = ValidateCheckbox();
                if (!valid)
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(806), utilities.MessageUtils.getMessage("Validation Error"));//Choose Product 
                    log.LogMethodExit();
                    return;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

            SqlTransaction SQLTrx = null;
            SqlConnection TrxCnn = null;

            if (SQLTrx == null)
            {
                TrxCnn = utilities.createConnection();
                SQLTrx = TrxCnn.BeginTransaction();
            }

            try
            {
                double CurrentQty, AdjQty = 0;
                double userAdjQty = Convert.ToDouble(UpDownAdjQty.Value);
                for (int i = 0; i < dgvProducts.RowCount; i++)
                {
                    if (dgvProducts["chkSelect", i].Value != null && dgvProducts["chkSelect", i].Value.ToString() == "Y")
                    {
                        int lotId = -1;
                        if (Convert.ToInt32(dgvProducts[locationIdDataGridViewTextBoxColumn.Index, i].Value) != -1)
                        {
                            CurrentQty = Convert.ToDouble(dgvProducts[avlQuantityDataGridViewTextBoxColumn.Index, i].Value);
                            AdjQty = userAdjQty;

                            if (CurrentQty + userAdjQty < 0)
                            {
                                MessageBox.Show(utilities.MessageUtils.getMessage("Adjustment Error"), utilities.MessageUtils.getMessage("Validation Error"));
                                return;
                            }
                            if (AdjQty == 0)
                                continue;

                            if (dgvProducts[lotIdDataGridViewTextBoxColumn.Index, i].Value != DBNull.Value)
                            {
                                lotId = Convert.ToInt32(dgvProducts[lotIdDataGridViewTextBoxColumn.Index, i].Value);
                            }

                            int productId = Convert.ToInt32(dgvProducts[productIdDataGridViewTextBoxColumn.Index, i].Value);
                            int locationId = Convert.ToInt32(dgvProducts[locationIdDataGridViewTextBoxColumn.Index, i].Value);
                            InventoryList inventoryList = new InventoryList();
                            InventoryDTO inventoryDTO = inventoryList.GetInventory(productId, locationId, lotId, SQLTrx);
                            double currentqtyinDTO = 0;
                            if (inventoryDTO != null)
                            {
                                currentqtyinDTO = inventoryDTO.Quantity;
                                inventoryDTO.Quantity = AdjQty + currentqtyinDTO;
                            }
                            else
                            {
                                inventoryDTO = new InventoryDTO();
                                inventoryDTO.ProductId = productId;
                                inventoryDTO.LocationId = locationId;
                                inventoryDTO.LotId = lotId;
                                inventoryDTO.Quantity = AdjQty;
                            }
                            inventoryDTO.UOMId = Convert.ToInt32(dgvProducts[UOMId.Index, i].Value);
                            inventoryBl = new Inventory(inventoryDTO, executionContext);
                            inventoryBl.Save(SQLTrx);

                            InventoryDocumentTypeList inventoryDocumentTypeList = new InventoryDocumentTypeList(executionContext);
                            List<InventoryDocumentTypeDTO> inventoryDocumentTypeDTOList = new List<InventoryDocumentTypeDTO>();
                            List<KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>> documentTypeSearchParams = new List<KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>>();
                            documentTypeSearchParams.Add(new KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>(InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters.NAME, "Adjustment Issue"));
                            documentTypeSearchParams.Add(new KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>(InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                            inventoryDocumentTypeDTOList = inventoryDocumentTypeList.GetAllInventoryDocumentTypes(documentTypeSearchParams);



                            //Updated code to insert Adjustment of the type selected
                            int fromLocId = Convert.ToInt32(dgvProducts[locationIdDataGridViewTextBoxColumn.Index, i].Value);
                            int adjTypeId = cbAdjustmentType.SelectedValue == null ? -1 : Convert.ToInt32(cbAdjustmentType.SelectedValue);
                            int docTypeId = -1;
                            if (inventoryDocumentTypeDTOList != null && inventoryDocumentTypeDTOList.Count > 0)
                            {
                                docTypeId = inventoryDocumentTypeDTOList[0].DocumentTypeId;
                            }
                            LookupValuesList serverTimeObject = new LookupValuesList(executionContext);
                            DateTime currentDatetime = serverTimeObject.GetServerDateTime();
                            inventoryAdjustmentsDTO = new InventoryAdjustmentsDTO(-1, "Adjustment", AdjQty, fromLocId, -1 , txtAdjRemarks.Text,
                                                                        Convert.ToInt32(dgvProducts[productIdDataGridViewTextBoxColumn.Index, i].Value), currentDatetime, null,null,
                                                                        adjTypeId , lotId, 0 , docTypeId, false ,-1, Convert.ToInt32(dgvProducts[UOMId.Index, i].Value), -1);


                            //Insert DTO
                            inventoryAdjustmentsBl = new InventoryAdjustmentsBL(inventoryAdjustmentsDTO, executionContext);
                            inventoryAdjustmentsBl.Save(SQLTrx);
                        }
                        else
                        {
                            log.Debug("Product is not available in the location"); // case when product do not have inventory record. Not allotted to any location
                            MessageBox.Show(utilities.MessageUtils.getMessage(2695, dgvProducts[codeDataGridViewTextBoxColumn.Index, i].Value), utilities.MessageUtils.getMessage("Validation Error"));
                            return;
                        }
                    }
                }
                SQLTrx.Commit();
                MessageBox.Show(utilities.MessageUtils.getMessage(2904, userAdjQty), utilities.MessageUtils.getMessage("Adjustment Status"));//Adjustment Complete
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, utilities.MessageUtils.getMessage("Adj Error"));
                SQLTrx.Rollback();
                log.Error(ex);
            }

            ProdSearch();
            log.LogMethodExit();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                ProdSearch();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void ProdSearch()
        {
            log.LogMethodEntry();
            try
            {
                ProductContainer productContainer = new ProductContainer(executionContext);
                SqlCommand cmd = utilities.getCommand();
                string advancedSearch = "";

                if (AdvancedSearch != null)
                {
                    if (!string.IsNullOrEmpty(AdvancedSearch.searchCriteria))
                    {
                        advancedSearch = " where (" + AdvancedSearch.searchCriteria + ") ";
                    }
                }
                UOMContainer uOMContainer = CommonFuncs.GetUOMContainer();
                //Updated query to search barcode from product barcode
                SegmentDefinitionList segmentDefinitionList = new SegmentDefinitionList(executionContext);
                List<SegmentDefinitionDTO> segmentDefinitionDTOList = new List<SegmentDefinitionDTO>();
                List<KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>> segmentDefinitionSearchParams = new List<KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>>();
                segmentDefinitionSearchParams = new List<KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>>();
                segmentDefinitionSearchParams.Add(new KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>(SegmentDefinitionDTO.SearchBySegmentDefinitionParameters.IS_ACTIVE, "Y"));
                segmentDefinitionSearchParams.Add(new KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>(SegmentDefinitionDTO.SearchBySegmentDefinitionParameters.APPLICABLE_ENTITY, "PRODUCT"));
                segmentDefinitionSearchParams.Add(new KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>(SegmentDefinitionDTO.SearchBySegmentDefinitionParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                segmentDefinitionDTOList = segmentDefinitionList.GetAllSegmentDefinitions(segmentDefinitionSearchParams, null);

                if (segmentDefinitionDTOList != null)
                {
                    string pivotColumns = "";
                    foreach (SegmentDefinitionDTO sd in segmentDefinitionDTOList)
                    {
                        pivotColumns += ", [" + sd.SegmentName + "]";
                    }

                    InventoryAdjustmentsList inventoryAdjustmentsList = new InventoryAdjustmentsList(executionContext);
                    inventoryAdjustmentsSummaryDTOList = new List<InventoryAdjustmentsSummaryDTO>();
                    List<KeyValuePair<InventoryAdjustmentsSummaryDTO.SearchByInventoryAdjustmentsSummaryParameters, string>> searchParameters = new List<KeyValuePair<InventoryAdjustmentsSummaryDTO.SearchByInventoryAdjustmentsSummaryParameters, string>>();
                    if (cbInvLocation.Text.Trim() == "" || (cbInvLocation.SelectedValue != null && cbInvLocation.SelectedValue.Equals(-1)))
                        searchParameters.Add(new KeyValuePair<InventoryAdjustmentsSummaryDTO.SearchByInventoryAdjustmentsSummaryParameters, string>(InventoryAdjustmentsSummaryDTO.SearchByInventoryAdjustmentsSummaryParameters.LOCATION_ID, "-1"));
                    else
                        searchParameters.Add(new KeyValuePair<InventoryAdjustmentsSummaryDTO.SearchByInventoryAdjustmentsSummaryParameters, string>(InventoryAdjustmentsSummaryDTO.SearchByInventoryAdjustmentsSummaryParameters.LOCATION_ID, cbInvLocation.SelectedValue.ToString()));
                    searchParameters.Add(new KeyValuePair<InventoryAdjustmentsSummaryDTO.SearchByInventoryAdjustmentsSummaryParameters, string>(InventoryAdjustmentsSummaryDTO.SearchByInventoryAdjustmentsSummaryParameters.DESCRIPTION, txtDescription.Text));
                    searchParameters.Add(new KeyValuePair<InventoryAdjustmentsSummaryDTO.SearchByInventoryAdjustmentsSummaryParameters, string>(InventoryAdjustmentsSummaryDTO.SearchByInventoryAdjustmentsSummaryParameters.PRODUCT_CODE, txtProdCode.Text));
                    searchParameters.Add(new KeyValuePair<InventoryAdjustmentsSummaryDTO.SearchByInventoryAdjustmentsSummaryParameters, string>(InventoryAdjustmentsSummaryDTO.SearchByInventoryAdjustmentsSummaryParameters.PURCHASEABLE, chkPurchaseable.Checked ? "Y" : "N"));
                    searchParameters.Add(new KeyValuePair<InventoryAdjustmentsSummaryDTO.SearchByInventoryAdjustmentsSummaryParameters, string>(InventoryAdjustmentsSummaryDTO.SearchByInventoryAdjustmentsSummaryParameters.BARCODE, txtProdBarcode.Text));
                    searchParameters.Add(new KeyValuePair<InventoryAdjustmentsSummaryDTO.SearchByInventoryAdjustmentsSummaryParameters, string>(InventoryAdjustmentsSummaryDTO.SearchByInventoryAdjustmentsSummaryParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                    inventoryAdjustmentsSummaryDTOList = inventoryAdjustmentsList.GetInventoryAdjustmentsSummaryDTO(searchParameters, advancedSearch, pivotColumns);
                    if (inventoryAdjustmentsSummaryDTOList != null)
                    {
                        SortableBindingList<InventoryAdjustmentsSummaryDTO> inventoryAdjustmentsSummaryDTOSortList = new SortableBindingList<InventoryAdjustmentsSummaryDTO>(inventoryAdjustmentsSummaryDTOList);
                        inventoryAdjustmentsSummaryBindingSource.DataSource = inventoryAdjustmentsSummaryDTOSortList;
                    }
                    else
                    {
                        inventoryAdjustmentsSummaryBindingSource.DataSource = new SortableBindingList<InventoryAdjustmentsSummaryDTO>();
                    }
                    dgvProducts.DataSource = inventoryAdjustmentsSummaryBindingSource;
                }
                else
                {
                    InventoryAdjustmentsList inventoryAdjustmentsList = new InventoryAdjustmentsList(executionContext);
                    List<InventoryAdjustmentsSummaryDTO> inventoryAdjustmentsSummaryDTOList = new List<InventoryAdjustmentsSummaryDTO>();
                    List<KeyValuePair<InventoryAdjustmentsSummaryDTO.SearchByInventoryAdjustmentsSummaryParameters, string>> searchParameters = new List<KeyValuePair<InventoryAdjustmentsSummaryDTO.SearchByInventoryAdjustmentsSummaryParameters, string>>();
                    if (cbInvLocation.Text.Trim() == "" || cbInvLocation.SelectedValue.Equals(-1))
                        searchParameters.Add(new KeyValuePair<InventoryAdjustmentsSummaryDTO.SearchByInventoryAdjustmentsSummaryParameters, string>(InventoryAdjustmentsSummaryDTO.SearchByInventoryAdjustmentsSummaryParameters.LOCATION_ID, "null"));
                    else if (cbInvLocation.SelectedValue != null)
                    {
                        searchParameters.Add(new KeyValuePair<InventoryAdjustmentsSummaryDTO.SearchByInventoryAdjustmentsSummaryParameters, string>(InventoryAdjustmentsSummaryDTO.SearchByInventoryAdjustmentsSummaryParameters.LOCATION_ID, cbInvLocation.SelectedValue.ToString()));
                    }
                    searchParameters.Add(new KeyValuePair<InventoryAdjustmentsSummaryDTO.SearchByInventoryAdjustmentsSummaryParameters, string>(InventoryAdjustmentsSummaryDTO.SearchByInventoryAdjustmentsSummaryParameters.DESCRIPTION, txtDescription.Text));
                    searchParameters.Add(new KeyValuePair<InventoryAdjustmentsSummaryDTO.SearchByInventoryAdjustmentsSummaryParameters, string>(InventoryAdjustmentsSummaryDTO.SearchByInventoryAdjustmentsSummaryParameters.PRODUCT_CODE, txtProdCode.Text));
                    searchParameters.Add(new KeyValuePair<InventoryAdjustmentsSummaryDTO.SearchByInventoryAdjustmentsSummaryParameters, string>(InventoryAdjustmentsSummaryDTO.SearchByInventoryAdjustmentsSummaryParameters.PURCHASEABLE, chkPurchaseable.Checked ? "Y" : "N"));
                    searchParameters.Add(new KeyValuePair<InventoryAdjustmentsSummaryDTO.SearchByInventoryAdjustmentsSummaryParameters, string>(InventoryAdjustmentsSummaryDTO.SearchByInventoryAdjustmentsSummaryParameters.BARCODE, txtProdBarcode.Text));
                    searchParameters.Add(new KeyValuePair<InventoryAdjustmentsSummaryDTO.SearchByInventoryAdjustmentsSummaryParameters, string>(InventoryAdjustmentsSummaryDTO.SearchByInventoryAdjustmentsSummaryParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                    inventoryAdjustmentsSummaryDTOList = inventoryAdjustmentsList.GetInventoryAdjustmentsSummaryDTO(searchParameters, null, null);
                    if (inventoryAdjustmentsSummaryDTOList != null)
                    {
                        SortableBindingList<InventoryAdjustmentsSummaryDTO> inventoryAdjustmentsSummaryDTOSortList = new SortableBindingList<InventoryAdjustmentsSummaryDTO>(inventoryAdjustmentsSummaryDTOList);
                        inventoryAdjustmentsSummaryBindingSource.DataSource = inventoryAdjustmentsSummaryDTOSortList;
                    }
                    else
                    {
                        inventoryAdjustmentsSummaryBindingSource.DataSource = new SortableBindingList<InventoryAdjustmentsSummaryDTO>();
                    }
                    dgvProducts.DataSource = inventoryAdjustmentsSummaryBindingSource;
                }

                dgvProducts.Columns["ProductBarcode"].Width = 30;
                dgvProducts.Columns["LotDetails"].Width = 70;
                dgvProducts.Columns["descriptionDataGridViewTextBoxColumn"].Width = 200;
                dgvProducts.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
                dgvProducts.EnableHeadersVisualStyles = false;

                dgvProducts.BackgroundColor = this.BackColor;

                dgvProducts.Columns[totalCostDataGridViewTextBoxColumn.Index].DefaultCellStyle = utilities.gridViewAmountCellStyle();
                dgvProducts.Columns[avlQuantityDataGridViewTextBoxColumn.Index].DefaultCellStyle =
                dgvProducts.Columns[reorderPointDataGridViewTextBoxColumn.Index].DefaultCellStyle =
                dgvProducts.Columns[reorderQuantityDataGridViewTextBoxColumn.Index].DefaultCellStyle = utilities.gridViewAmountCellStyle();

                dgvProducts.Columns[reorderQuantityDataGridViewTextBoxColumn.Index].DefaultCellStyle.Format =
                dgvProducts.Columns[reorderPointDataGridViewTextBoxColumn.Index].DefaultCellStyle.Format = utilities.ParafaitEnv.INVENTORY_QUANTITY_FORMAT;

                dgvProducts.Columns[avlQuantityDataGridViewTextBoxColumn.Index].DefaultCellStyle = new DataGridViewCellStyle(dgvProducts.Columns[avlQuantityDataGridViewTextBoxColumn.Index].DefaultCellStyle);
                dgvProducts.Columns[allocatedDataGridViewTextBoxColumn.Index].DefaultCellStyle = new DataGridViewCellStyle(dgvProducts.Columns[avlQuantityDataGridViewTextBoxColumn.Index].DefaultCellStyle);
                dgvProducts.Columns[avlQuantityDataGridViewTextBoxColumn.Index].DefaultCellStyle.Format = utilities.ParafaitEnv.INVENTORY_QUANTITY_FORMAT;
                dgvProducts.Columns[allocatedDataGridViewTextBoxColumn.Index].DefaultCellStyle.Format = utilities.ParafaitEnv.INVENTORY_QUANTITY_FORMAT;
                foreach (DataGridViewColumn dc in dgvProducts.Columns)
                {
                    if (dc.Index > 2 && dc.Name != "TransferQuantity") //Updated condition, Transfer quantity column appeared to be readonly 
                        dc.ReadOnly = true;


                }
                AllSelected = false;
                dgvProducts.Columns[avlQuantityDataGridViewTextBoxColumn.Index].DefaultCellStyle.ForeColor = Color.Blue;
                decimal totalCost = 0;
                for (int i = 0; i < dgvProducts.Rows.Count; i++)
                {
                    if (dgvProducts[avlQuantityDataGridViewTextBoxColumn.Index, i].Value != DBNull.Value && Convert.ToDouble(dgvProducts[avlQuantityDataGridViewTextBoxColumn.Index, i].Value) < 0)
                        dgvProducts[avlQuantityDataGridViewTextBoxColumn.Index, i].Style.BackColor = Color.LightCoral;
                    else
                        dgvProducts[avlQuantityDataGridViewTextBoxColumn.Index, i].Style.BackColor = Color.White;

                    if (dgvProducts[totalCostDataGridViewTextBoxColumn.Index, i].Value != DBNull.Value)
                        totalCost += Convert.ToDecimal(dgvProducts[totalCostDataGridViewTextBoxColumn.Index, i].Value);
                    int uomId = Convert.ToInt32(dgvProducts[UOMId.Index, i].Value);
                    if (uomId == -1 && ProductContainer.productDTOList != null && ProductContainer.productDTOList.Count > 0)
                    {
                        uomId = ProductContainer.productDTOList.Find(x => x.ProductId == (Convert.ToInt32(dgvProducts[productIdDataGridViewTextBoxColumn.Index, i].Value))).InventoryUOMId;
                    }
                    dgvProducts[txtUOM.Index, i].Value = UOMContainer.uomDTOList.Find(x => x.UOMId == uomId).UOM;
                }
                txtTotalCost.Text = totalCost.ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        bool AllSelected = false;
        private void dgvProducts_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (e.ColumnIndex == 0)
                {
                    try
                    {
                        dgvProducts.CurrentCell = dgvProducts.CurrentRow.Cells[1];
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                    }
                    for (int i = 0; i < dgvProducts.RowCount; i++)
                    {
                        if (dgvProducts[locationIdDataGridViewTextBoxColumn.Index, i].Value != DBNull.Value)
                        {
                            if (!AllSelected)
                                dgvProducts["chkSelect", i].Value = "Y";
                            else
                                dgvProducts["chkSelect", i].Value = "N";
                        }
                    }
                    AllSelected = !AllSelected;
                }
                for (int i = 0; i < dgvProducts.RowCount; i++)
                {
                    int uomId = Convert.ToInt32(dgvProducts["UOMId", i].Value);
                    CommonFuncs.GetUOMComboboxForSelectedRows(dgvProducts, i, uomId);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void dgvProducts_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (dgvProducts.Columns[e.ColumnIndex].DataPropertyName == "AvlQuantity" && dgvProducts[avlQuantityDataGridViewTextBoxColumn.Index, e.RowIndex].Value.ToString() == "NaN")
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

        private void dgvProducts_CellFormat(object sender, DataGridViewCellFormattingEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (dgvProducts.Columns[e.ColumnIndex].DataPropertyName == "AvlQuantity")
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
            if (entityExclusionDetailDTOList != null && entityExclusionDetailDTOList.Count > 0)
            {
                foreach (EntityExclusionDetailDTO entityExclusionDetailDTO in entityExclusionDetailDTOList)
                {
                    if (entityExclusionDetailDTO.FieldName == dgvProducts.Columns[e.ColumnIndex].DataPropertyName)
                    {
                        e.Value = new String('\u25CF', 6);
                    }
                }
            }
            log.LogMethodExit();
        }

        private void dgvProducts_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.Row.Index >= 0)
            {
                if (dgvProducts[locationIdDataGridViewTextBoxColumn.Name, e.Row.Index].Value == DBNull.Value)
                {
                    dgvProducts.Rows[e.Row.Index].Cells["chkSelect"].ReadOnly = true;
                }

                //Added condition to see that the record is not available for adjustment or transfer 
                //in case product is lot controlled  but inventory record has lotID set to null
                else if (dgvProducts[lotIdDataGridViewTextBoxColumn.Name, e.Row.Index].Value == DBNull.Value && Convert.ToBoolean(dgvProducts[lotControlledDataGridViewCheckBoxColumn.Name, e.Row.Index].Value) == true)
                {
                    dgvProducts.Rows[e.Row.Index].Cells["chkSelect"].ReadOnly = true;
                }

                else
                {
                    dgvProducts.Rows[e.Row.Index].Cells["chkSelect"].ReadOnly = false;
                }
            }
            log.LogMethodExit();
        }

        int CurrentRowIndex = 0;
        private void btnTransfer_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            double userTransferQty = 0;

            this.ValidateChildren();
            this.Validate();

            if (cbToLocation.SelectedIndex < 0 || (int)cbToLocation.SelectedValue < 0)
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(838), utilities.MessageUtils.getMessage("Select Location"));
                cbToLocation.Focus();
                log.LogMethodExit("cbToLocation.SelectedIndex < 0 || (int)cbToLocation.SelectedValue < 0");
                return;
            }

            if (dgvProducts.RowCount == 0)
            {
                log.LogMethodExit("dgvProducts.RowCount == 0");
                return;
            }

            CurrentRowIndex = dgvProducts.CurrentRow.Index;

            int TransferLocationId = Convert.ToInt32(cbToLocation.SelectedValue);

            SqlTransaction SQLTrx = null;
            SqlConnection TrxCnn = null;

            if (SQLTrx == null)
            {
                TrxCnn = utilities.createConnection();
                SQLTrx = TrxCnn.BeginTransaction();
            }

            try
            {
                //updated site_id condition
                double CurrentQty, TransferQty = 0;

                InventoryDTO inventoryUpdateDTO = new InventoryDTO();
                inventoryUpdateDTO.Quantity = 0;
                inventoryUpdateDTO.LocationId = 0;
                inventoryUpdateDTO.ProductId = 0;

                InventoryDTO inventoryInsertDTO = new InventoryDTO();
                inventoryInsertDTO.ProductId = 0;
                inventoryInsertDTO.LocationId = 0;
                inventoryInsertDTO.Quantity = 0;

                //Start update 2-Mar-2017 
                InventoryDocumentTypeList inventoryDocumentTypeList = new InventoryDocumentTypeList(executionContext);
                List<InventoryDocumentTypeDTO> inventoryDocumentTypeDTOList = new List<InventoryDocumentTypeDTO>();
                List<KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>> documentTypeSearchParams = new List<KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>>();
                documentTypeSearchParams.Add(new KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>(InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters.NAME, "Stock Transfer Issue"));
                documentTypeSearchParams.Add(new KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>(InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                inventoryDocumentTypeDTOList = inventoryDocumentTypeList.GetAllInventoryDocumentTypes(documentTypeSearchParams);
                //End update 2-Mar-2017
                int inventoryDocumentTypeId = -1;
                if (inventoryDocumentTypeDTOList != null && inventoryDocumentTypeDTOList.Count > 0)
                {
                    inventoryDocumentTypeId = inventoryDocumentTypeDTOList[0].DocumentTypeId;
                }


                for (int i = 0; i < dgvProducts.RowCount; i++)
                {
                    int lotId = -1;
                    if (dgvProducts["chkSelect", i].Value != null && dgvProducts["chkSelect", i].Value.ToString() == "Y")
                    {
                        if (Convert.ToInt32(dgvProducts[locationIdDataGridViewTextBoxColumn.Index, i].Value) != -1)
                        {
                            if (TransferLocationId == Convert.ToInt32(dgvProducts[locationIdDataGridViewTextBoxColumn.Index, i].Value))
                            {
                                MessageBox.Show(utilities.MessageUtils.getMessage(1085));
                                continue;
                            }

                            try
                            {
                                userTransferQty = Convert.ToDouble(dgvProducts[TransferQuantity.Index, i].Value);
                                if (userTransferQty == 0)
                                {
                                    MessageBox.Show(utilities.MessageUtils.getMessage(747), utilities.MessageUtils.getMessage("Validation Error"));//validate
                                    log.LogMethodExit("userTransferQty == 0");
                                    return;

                                }
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex);
                            }
                            if (userTransferQty == 0)
                                continue;

                            if (dgvProducts[lotIdDataGridViewTextBoxColumn.Index, i].Value != DBNull.Value)
                            {
                                lotId = Convert.ToInt32(dgvProducts[lotIdDataGridViewTextBoxColumn.Index, i].Value);
                            }

                            CurrentQty = Convert.ToDouble(dgvProducts[avlQuantityDataGridViewTextBoxColumn.Index, i].Value);
                            TransferQty = userTransferQty;

                            InventoryList inventoryList = new InventoryList();
                            //Begin:Added on 06-Sep-2017
                            inventoryUpdateDTO.ProductId = Convert.ToInt32(dgvProducts[productIdDataGridViewTextBoxColumn.Index, i].Value);
                            inventoryUpdateDTO.LotId = lotId;
                            //End:Added on 06-Sep-2017


                            int transferToLocationId = (rbToLocation.Checked ? Convert.ToInt32(dgvProducts[locationIdDataGridViewTextBoxColumn.Index, i].Value) : TransferLocationId);

                            BindingSource bindingSource = (BindingSource)dgvProducts.DataSource;
                            SortableBindingList<InventoryAdjustmentsSummaryDTO> summaryDTOList = (SortableBindingList<InventoryAdjustmentsSummaryDTO>)bindingSource.DataSource;
                            if (summaryDTOList != null)
                            {
                                InventoryAdjustmentsSummaryDTO checkQtyDTO = summaryDTOList.Where(x => x.ProductId == inventoryUpdateDTO.ProductId && x.LocationId == transferToLocationId && x.LotId == inventoryUpdateDTO.LotId).FirstOrDefault();
                                if (rbFromLocation.Checked && checkQtyDTO == null)
                                {
                                    MessageBox.Show(utilities.MessageUtils.getMessage(918), utilities.MessageUtils.getMessage("Validation Error"));
                                    return;
                                }
                                //Validate if there is no products in From location
                                if (checkQtyDTO != null && checkQtyDTO.AvlQuantity < userTransferQty)
                                {
                                    MessageBox.Show(utilities.MessageUtils.getMessage(2445), utilities.MessageUtils.getMessage("Validation Error"));
                                    return;
                                }
                            }

                            //Check if the location is department
                            //In case location is department, no inventory record should be there
                            if (!isDepartment(rbToLocation.Checked ? TransferLocationId : Convert.ToInt32(dgvProducts[locationIdDataGridViewTextBoxColumn.Index, i].Value)))
                            {
                                if (TransferLocationId == -1)
                                {
                                    MessageBox.Show(utilities.MessageUtils.getMessage(12186));
                                    log.LogMethodExit("TransferLocationId == -1");
                                    return;
                                }

                                inventoryUpdateDTO.LocationId = (rbToLocation.Checked ? TransferLocationId : Convert.ToInt32(dgvProducts[locationIdDataGridViewTextBoxColumn.Index, i].Value));
                                InventoryDTO tempDTO = inventoryList.GetInventory(inventoryUpdateDTO.ProductId, inventoryUpdateDTO.LocationId, inventoryUpdateDTO.LotId, SQLTrx);
                                double currentTolocationQty = 0;

                                if (tempDTO != null)
                                {
                                    inventoryUpdateDTO = tempDTO;
                                    currentTolocationQty = TransferQty + inventoryUpdateDTO.Quantity;
                                    inventoryUpdateDTO.Quantity = currentTolocationQty;
                                }
                                else
                                {
                                    inventoryUpdateDTO.Quantity = TransferQty + currentTolocationQty;
                                }

                                inventoryBl = new Inventory(inventoryUpdateDTO, executionContext);
                                if (inventoryBl.Save(SQLTrx) <= 0) // Location not found in inventory
                                {
                                    inventoryInsertDTO.ProductId = Convert.ToInt32(dgvProducts[productIdDataGridViewTextBoxColumn.Index, i].Value);
                                    inventoryInsertDTO.LocationId = (rbToLocation.Checked ? TransferLocationId : Convert.ToInt32(dgvProducts[locationIdDataGridViewTextBoxColumn.Index, i].Value));
                                    inventoryInsertDTO.Quantity = TransferQty;
                                    inventoryInsertDTO.LotId = lotId;
                                    inventoryInsertDTO.UOMId = Convert.ToInt32(dgvProducts[UOMId.Index, i].Value);
                                    inventoryBl = new Inventory(inventoryInsertDTO, executionContext);
                                    inventoryBl.Save(SQLTrx);
                                }
                            }
                            int fromLocationId = (rbToLocation.Checked ? Convert.ToInt32(dgvProducts[locationIdDataGridViewTextBoxColumn.Index, i].Value) : TransferLocationId);
                            InventoryDTO tempFromLocationDTO = inventoryList.GetInventory(inventoryUpdateDTO.ProductId, fromLocationId, inventoryUpdateDTO.LotId, SQLTrx);
                            double currentFromlocationQty = 0;

                            if (tempFromLocationDTO != null)
                            {
                                currentFromlocationQty = tempFromLocationDTO.Quantity;
                            }
                            else
                            {
                                tempFromLocationDTO = new InventoryDTO();
                                tempFromLocationDTO.ProductId = Convert.ToInt32(dgvProducts[productIdDataGridViewTextBoxColumn.Index, i].Value);
                                tempFromLocationDTO.LotId = lotId;
                                tempFromLocationDTO.LocationId = fromLocationId;
                            }

                            //Check if the location is department
                            //In case location is department, no inventory record should be there
                            if (!isDepartment(rbToLocation.Checked ? Convert.ToInt32(dgvProducts[locationIdDataGridViewTextBoxColumn.Index, i].Value) : TransferLocationId))
                            {
                                if (TransferLocationId == -1)
                                {
                                    MessageBox.Show(utilities.MessageUtils.getMessage(12186));
                                    return;
                                }
                                tempFromLocationDTO.Quantity = currentFromlocationQty + TransferQty * -1;
                                inventoryBl = new Inventory(tempFromLocationDTO, executionContext);

                                if (inventoryBl.Save(SQLTrx) <= 0) // Location not found in inventory
                                {
                                    inventoryInsertDTO.ProductId = Convert.ToInt32(dgvProducts[productIdDataGridViewTextBoxColumn.Index, i].Value);
                                    inventoryInsertDTO.LocationId = (rbToLocation.Checked ? Convert.ToInt32(dgvProducts[locationIdDataGridViewTextBoxColumn.Index, i].Value) : TransferLocationId);
                                    inventoryInsertDTO.Quantity = TransferQty * -1;
                                    inventoryInsertDTO.LotId = lotId;
                                    inventoryInsertDTO.UOMId = Convert.ToInt32(dgvProducts[UOMId.Index, i].Value);

                                    inventoryBl = new Inventory(inventoryInsertDTO, executionContext);
                                    inventoryBl.Save(SQLTrx);
                                }
                            }
                            LookupValuesList serverTimeObject = new LookupValuesList(executionContext);
                            DateTime currentDatetime = serverTimeObject.GetServerDateTime();
                            int fromLocId = (rbFromLocation.Checked ? TransferLocationId : Convert.ToInt32(dgvProducts[locationIdDataGridViewTextBoxColumn.Index, i].Value));
                            int toLocId = (rbToLocation.Checked ? TransferLocationId : Convert.ToInt32(dgvProducts[locationIdDataGridViewTextBoxColumn.Index, i].Value));
                            inventoryAdjustmentsDTO = new InventoryAdjustmentsDTO(-1, "Transfer" , TransferQty , fromLocId, toLocId, txtTransferRemarks.Text,
                                                                    Convert.ToInt32(dgvProducts[productIdDataGridViewTextBoxColumn.Index, i].Value), currentDatetime,
                                                                    null , null,-1 ,lotId , 0 , inventoryDocumentTypeId ,false ,-1,
                                                                    Convert.ToInt32(dgvProducts[UOMId.Index, i].Value), -1
                                                                    );
                            inventoryAdjustmentsBl = new InventoryAdjustmentsBL(inventoryAdjustmentsDTO, executionContext);
                            inventoryAdjustmentsBl.Save(SQLTrx);
                        }
                        else
                        {
                            log.Debug("Sorry unable to proceed, product &1 has no stock in inventory location"); //Sorry unable to proceed, product &1 has no stock in inventory location
                            MessageBox.Show(utilities.MessageUtils.getMessage(2695, dgvProducts[codeDataGridViewTextBoxColumn.Index, i].Value), utilities.MessageUtils.getMessage("Validation Error"));
                            return;
                        }
                    }
                }
                SQLTrx.Commit();
                MessageBox.Show(utilities.MessageUtils.getMessage(880), utilities.MessageUtils.getMessage("Transfer Status"));//Transfer Complete
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, utilities.MessageUtils.getMessage("Transfer Error"));
                SQLTrx.Rollback();
                log.Error(ex);
            }

            ProdSearch();

            try
            {
                dgvProducts.CurrentCell = dgvProducts[TransferQuantity.Index, CurrentRowIndex];
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }


        //Start update 9-Nov-2016
        //Added method
        private bool isDepartment(int locationID)
        {
            log.LogMethodEntry(locationID);
            SqlCommand cmd = utilities.getCommand();
            cmd.CommandText = @"SELECT isnull(LocationType, '') LocationType
                                FROM Location l, LocationType lt
                                WHERE l.LocationTypeID = lt.LocationTypeId
                                    and locationid = @locationid";
            cmd.Parameters.AddWithValue("@locationid", locationID);
            object o = cmd.ExecuteScalar();
            if (o != null)
            {
                string locationType = o.ToString();
                if (locationType == "Department")
                {
                    log.LogMethodExit(true);
                    return true;
                }
            }
            log.LogMethodExit(false);
            return false;
        }
        //End update 9-Nov-2016

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            this.Close();
            log.LogMethodExit();
        }

        private void dgvProducts_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);

            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                log.LogMethodExit();
                return;
            }

            if (dgvProducts.Columns[e.ColumnIndex].Name == "TransferQuantity")
            {
                if (dgvProducts[TransferQuantity.Index, e.RowIndex].Value == null || dgvProducts[TransferQuantity.Index, e.RowIndex].Value == DBNull.Value || dgvProducts[locationIdDataGridViewTextBoxColumn.Index, e.RowIndex].Value == DBNull.Value)
                {
                    dgvProducts["chkSelect", e.RowIndex].Value = "N";
                    log.LogMethodExit();
                    return;
                }

                try
                {
                    double userTransferQty = Convert.ToDouble(dgvProducts[TransferQuantity.Index, e.RowIndex].Value);
                    if (userTransferQty > 0)
                    {
                        dgvProducts["chkSelect", e.RowIndex].Value = "Y";
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
            }
            log.LogMethodExit();
        }

        private void dgvProducts_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);

            if (dgvProducts.Columns[e.ColumnIndex].Name == avlQuantityDataGridViewTextBoxColumn.Name)
            {
                try
                {
                    this.Cursor = Cursors.WaitCursor;
                    using (frmProductActivity f = new frmProductActivity(dgvProducts[productIdDataGridViewTextBoxColumn.Name, e.RowIndex].Value, dgvProducts[locationIdDataGridViewTextBoxColumn.Name, e.RowIndex].Value, utilities))
                    {
                        f.Text = "Activities for Product: " + dgvProducts[codeDataGridViewTextBoxColumn.Name, e.RowIndex].FormattedValue.ToString() +
                               " and Location: " + dgvProducts[locationNameDataGridViewTextBoxColumn.Name, e.RowIndex].FormattedValue.ToString();
                        Semnox.Parafait.Inventory.CommonUIDisplay.setupVisuals(f);//Added for GUI Design style 
                        f.StartPosition = FormStartPosition.CenterScreen;//Added to show at center on 
                        f.ShowDialog();
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
            }
            if (dgvProducts.Columns[e.ColumnIndex].Name == "ProductBarcode") //Updated column name from Barcode to ProductBarcode 
            {
                try
                {
                    //Updated barCodes constructor call to see that product description is passed
                    using (frmlistBarCodes f = new frmlistBarCodes(Convert.ToInt32(dgvProducts[productIdDataGridViewTextBoxColumn.Name, e.RowIndex].Value), dgvProducts[codeDataGridViewTextBoxColumn.Name, e.RowIndex].Value.ToString(), dgvProducts[descriptionDataGridViewTextBoxColumn.Name, e.RowIndex].Value.ToString(), utilities))
                    {
                        Semnox.Parafait.Inventory.CommonUIDisplay.setupVisuals(f);//Added for GUI Design style 
                        f.StartPosition = FormStartPosition.CenterScreen;//Added to show at center 
                        f.ShowDialog();
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
            }

            if (dgvProducts.Columns[e.ColumnIndex].Name == "LotDetails")
            {
                try
                {
                    //Starts: Modification for converting non lotcontrolled stock to lotcontrolled
                    if (e.RowIndex > -1 && (utilities.executeScalar("select isnull(LotControlled,0) from Product WHERE productId = @productId",
                                               new SqlParameter("@productId", dgvProducts[productIdDataGridViewTextBoxColumn.Index, e.RowIndex].Value))).Equals(true))
                    {
                        if (dgvProducts[lotIdDataGridViewTextBoxColumn.Index, e.RowIndex].Value != DBNull.Value)
                        {
                            ProductList product = new ProductList();
                            ProductDTO ProductDTO = new ProductDTO();
                            ProductDTO = product.GetProduct(Convert.ToInt32(dgvProducts[productIdDataGridViewTextBoxColumn.Index, e.RowIndex].Value));
                            InventoryLotUI frmInventoryLotUI = new InventoryLotUI(utilities, Convert.ToInt32(dgvProducts[lotIdDataGridViewTextBoxColumn.Index, e.RowIndex].Value), -1, ProductDTO.ExpiryType);
                            Semnox.Parafait.Inventory.CommonUIDisplay.setupVisuals(frmInventoryLotUI);//Added for GUI Design style 
                            frmInventoryLotUI.StartPosition = FormStartPosition.CenterScreen;//Added to show at center 
                            frmInventoryLotUI.ShowDialog();
                            frmInventoryLotUI.Dispose();
                        }
                        else
                        {
                            DataTable dTable = utilities.executeDataTable("SELECT ExpiryType, Isnull(ExpiryDays,0) as ExpiryDays, isnull(IssuingApproach, 'None') IssuingApproach from Product WHERE productId = @productId ",
                                                new SqlParameter("@productId", dgvProducts[productIdDataGridViewTextBoxColumn.Index, e.RowIndex].Value));
                            if (dTable != null && dTable.Rows.Count > 0)
                            {

                                if ((dTable.Rows[0]["ExpiryType"].ToString() != "E" && dTable.Rows[0]["ExpiryType"].ToString() != "D") && dTable.Rows[0]["IssuingApproach"].ToString() == "FIFO")
                                {
                                    if (dgvProducts[avlQuantityDataGridViewTextBoxColumn.Index, e.RowIndex].Value != DBNull.Value && Convert.ToDouble(dgvProducts[avlQuantityDataGridViewTextBoxColumn.Index, e.RowIndex].Value) > 0)
                                    {

                                        Semnox.Parafait.Inventory.InventoryLotBL inventoryLot = new Semnox.Parafait.Inventory.InventoryLotBL(executionContext);
                                        inventoryLot.UpdateNonLotableToLotable(Convert.ToInt32(dgvProducts[productIdDataGridViewTextBoxColumn.Index, e.RowIndex].Value), null);

                                    }
                                }
                                else
                                {
                                    MessageBox.Show(utilities.MessageUtils.getMessage("Cannot create lot for products that expire or issuingapproach is not FIFO."));
                                    return;
                                }
                            }
                        }
                    }
                    //Ends: Modification on for converting non lotcontrolled stock to lotcontrolled
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
            }
            log.LogMethodExit();
        }

        private void dgvProducts_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);

            if (dgvProducts.Columns[e.ColumnIndex].Name == "avlQuantityDataGridViewTextBoxColumn")
            {
                try
                {
                    dgvProducts.Cursor = Cursors.Hand;
                    dgvProducts[e.ColumnIndex, e.RowIndex].Style.Font = new Font(dgvProducts.DefaultCellStyle.Font, FontStyle.Underline);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
            }
            log.LogMethodExit();
        }

        private void dgvProducts_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);

            if (dgvProducts.Columns[e.ColumnIndex].Name == "avlQuantityDataGridViewTextBoxColumn")
            {
                try
                {
                    dgvProducts.Cursor = Cursors.Default;
                    dgvProducts[e.ColumnIndex, e.RowIndex].Style.Font = new Font(dgvProducts.DefaultCellStyle.Font, FontStyle.Regular);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
            }
            log.LogMethodExit();
        }

        void onLoadActions()
        {
            log.LogMethodEntry();

            LocationList locationlist = new LocationList(executionContext);
            List<KeyValuePair<LocationDTO.SearchByLocationParameters, string>> searchParameter = new List<KeyValuePair<LocationDTO.SearchByLocationParameters, string>>();
            searchParameter.Add(new KeyValuePair<LocationDTO.SearchByLocationParameters, string>(LocationDTO.SearchByLocationParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            List<LocationDTO> LocationDTOList = locationlist.GetAllLocations(searchParameter);
            var SortedLocationDTOList = LocationDTOList.OrderBy(t => t.Name); //Sorts the Name column in as ascending order
            locationDTOBindingSource.DataSource = SortedLocationDTOList;
            if (SortedLocationDTOList != null)
            {
                cmbScannedLocation.SelectedIndex = -1;
            }
            btnScanLocation_Click(null, null);
            log.LogMethodExit();
        }

        private void btnScanLocation_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            btnScanLocation.BackColor = btnScanLocation.FlatAppearance.MouseDownBackColor;
            btnScanProduct.BackColor = Color.CadetBlue;
            btnScanLocation.FlatAppearance.MouseOverBackColor = btnScanLocation.BackColor;
            btnScanProduct.FlatAppearance.MouseOverBackColor = Color.CadetBlue;

            grpScanType.Tag = "Location";
            log.LogMethodExit();
        }

        private void btnScanProduct_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            btnScanLocation.BackColor = Color.CadetBlue;
            btnScanProduct.BackColor = btnScanLocation.FlatAppearance.MouseDownBackColor;
            btnScanProduct.FlatAppearance.MouseOverBackColor = btnScanProduct.BackColor;
            btnScanLocation.FlatAppearance.MouseOverBackColor = Color.CadetBlue;

            grpScanType.Tag = "Product";
            log.LogMethodExit();
        }

        private void cmbScannedLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (cmbScannedLocation.SelectedItem == null)
            {
                log.LogMethodExit("cmbScannedLocation.SelectedItem == null)");
                return;
            }
            DataRowView dr = cmbScannedLocation.SelectedItem as DataRowView;
            lblLocationId.Text = dr["LocationId"].ToString();
            lblBarCode.Text = dr["BarCode"].ToString();
            lblActive.Text = (dr["IsActive"].ToString() == "Y" ? "Yes" : "No");
            lblAvlblToSell.Text = (dr["IsAvailableToSell"].ToString() == "Y" ? "Yes" : "No");
            log.LogMethodExit();
        }

        private void tcInvAdjTransfer_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            BarcodeReader.setReceiveAction = BarCodeScanned;
            log.LogMethodExit();
        }

        private void frmInventoryAdjustments_Activated(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            BarcodeReader.setReceiveAction = BarCodeScanned;
            txtProdBarcode.Focus();
            log.LogMethodExit();
        }

        void BarCodeScanned()
        {
            log.LogMethodEntry();
            string barCode = BarcodeReader.Barcode.Trim();
            if (barCode != "")
            {
                if (tcInvAdjTransfer.SelectedIndex == 1)
                {
                    if (grpScanType.Tag.ToString() == "Location")
                        getScannedLocation(barCode);
                    else
                        getScannedProduct(barCode);
                }
                else if (tcInvAdjTransfer.SelectedIndex == 0)
                {
                    txtProdBarcode.Text = barCode;
                }
            }
            log.LogMethodExit(barCode);
        }

        void getScannedLocation(string BarCode)
        {
            log.LogMethodEntry(BarCode);
            LocationList locationlist = new LocationList(executionContext);

            List<KeyValuePair<LocationDTO.SearchByLocationParameters, string>> searchParameter = new List<KeyValuePair<LocationDTO.SearchByLocationParameters, string>>();
            searchParameter.Add(new KeyValuePair<LocationDTO.SearchByLocationParameters, string>(LocationDTO.SearchByLocationParameters.BARCODE, BarCode));
            searchParameter.Add(new KeyValuePair<LocationDTO.SearchByLocationParameters, string>(LocationDTO.SearchByLocationParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            List<LocationDTO> LocationDTOList = locationlist.GetAllLocations(searchParameter);
            if (locationlist == null)
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(878, BarCode), utilities.MessageUtils.getMessage("Location Error"));
                log.LogMethodExit(utilities.MessageUtils.getMessage(878, BarCode));
                return;
            }
            cmbScannedLocation.SelectedValue = LocationDTOList[0].LocationId;
            log.LogMethodExit();
        }

        void getScannedProduct(string BarCode)
        {
            log.LogMethodEntry(BarCode);
            BindingSource bs = new BindingSource();
            InventoryAdjustmentsList inventoryAdjustmentsList = new InventoryAdjustmentsList(executionContext);
            List<BarcodeScanSummaryDTO> barcodeScanSummaryDTOList = new List<BarcodeScanSummaryDTO>();
            barcodeScanSummaryDTOList = inventoryAdjustmentsList.GetBarcodeScanSummaryDTO(BarCode, cmbScannedLocation.SelectedValue.ToString());
            if (barcodeScanSummaryDTOList != null)
            {
                SortableBindingList<BarcodeScanSummaryDTO> inventoryAdjustmentsSummaryDTOSortList = new SortableBindingList<BarcodeScanSummaryDTO>(barcodeScanSummaryDTOList);
                bs.DataSource = inventoryAdjustmentsSummaryDTOSortList;
            }
            else
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(879, BarCode), utilities.MessageUtils.getMessage("Product Error"));
                log.LogMethodExit(utilities.MessageUtils.getMessage(879, BarCode));
                return;
            }

            if (dgvTransferProducts.Columns.Count == 1)
            {
                dgvProducts.DataSource = bs;
                btnRemoveProduct.DisplayIndex = 0;
                BarCodeLotDetails.DisplayIndex = 3;
                dgvTransferProducts.AllowUserToAddRows = false;
                dgvTransferProducts.AllowUserToDeleteRows = false;
                utilities.setupDataGridProperties(ref dgvTransferProducts);
                foreach (DataGridViewColumn dc in dgvTransferProducts.Columns)
                    dc.ReadOnly = true;
                dgvTransferProducts.Columns["Trx_qty"].ReadOnly = false;
                dgvTransferProducts.Columns["Trx_qty"].DefaultCellStyle = utilities.gridViewNumericCellStyle();
                dgvTransferProducts.Columns["current_stock"].DefaultCellStyle = utilities.gridViewNumericCellStyle();
                dgvTransferProducts.Columns["Trx_qty"].DefaultCellStyle.Format =
                    dgvTransferProducts.Columns["current_stock"].DefaultCellStyle.Format = utilities.ParafaitEnv.INVENTORY_QUANTITY_FORMAT;

                dgvTransferProducts.Columns["ProductId"].Visible = false;
                dgvTransferProducts.Columns["LocationId"].Visible = false;
                dgvTransferProducts.Columns["LotId"].Visible = false;

                dgvTransferProducts.BorderStyle = BorderStyle.FixedSingle;
                dgvTransferProducts.BackgroundColor = this.BackColor;

            }
            log.LogMethodExit();
        }

        private void dgvTransferProdcuts_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);

            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                log.LogMethodExit();
                return;
            }
            if (dgvTransferProducts.Columns[e.ColumnIndex].Name == "btnRemoveProduct")
            {
                try
                {
                    dgvTransferProducts.Rows.RemoveAt(e.RowIndex);
                }
                catch (Exception ex) { log.Error(ex); }
            }
            if (dgvTransferProducts.Columns[e.ColumnIndex].Name == "BarCodeLotDetails")
            {
                try
                {
                    if (e.RowIndex > -1 && dgvProducts[lotIdDataGridViewTextBoxColumn.Index, e.RowIndex].Value != DBNull.Value)
                    {
                        InventoryLotUI frmInventoryLotUI = new InventoryLotUI(utilities, Convert.ToInt32(dgvProducts[lotIdDataGridViewTextBoxColumn.Index, e.RowIndex].Value));
                        frmInventoryLotUI.ShowDialog();
                        frmInventoryLotUI.Dispose();
                    }
                }
                catch (Exception ex) { log.Error(ex); }
            }
            log.LogMethodExit();
        }

        private void btnExitForm_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            this.Close();
            log.LogMethodExit();
        }

        private void btnClearProducts_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            for (int i = 0; i < dgvTransferProducts.Rows.Count; i++)
            {
                dgvTransferProducts.Rows.RemoveAt(i);
                i = -1;
            }
            log.LogMethodExit();
        }

        private void btnSaveTransfers_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            double userTransferQty = 0;

            if (cmbScannedLocation.SelectedIndex < 0)
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(838), utilities.MessageUtils.getMessage("Select Location"));
                cmbScannedLocation.Focus();
                log.LogMethodExit(utilities.MessageUtils.getMessage(838));
                return;
            }

            int TransferLocationId = Convert.ToInt32(cmbScannedLocation.SelectedValue);
            SqlTransaction SQLTrx = null;
            SqlConnection TrxCnn = null;

            if (SQLTrx == null)
            {
                TrxCnn = utilities.createConnection();
                SQLTrx = TrxCnn.BeginTransaction();
            }

            try
            {
                double TransferQty = 0;

                InventoryDTO inventoryInsertDTO = new InventoryDTO();
                inventoryInsertDTO.ProductId = 0;
                inventoryInsertDTO.LocationId = 0;
                inventoryInsertDTO.Quantity = 0;

                inventoryAdjustmentsDTO = new InventoryAdjustmentsDTO();
                inventoryAdjustmentsDTO.AdjustmentType = "Transfer";
                inventoryAdjustmentsDTO.AdjustmentQuantity = 0;
                inventoryAdjustmentsDTO.FromLocationId = 0;
                inventoryAdjustmentsDTO.ToLocationId = 0;
                inventoryAdjustmentsDTO.Remarks = txtTransferRemarks.Text;
                inventoryAdjustmentsDTO.ProductId = 0;

                for (int i = 0; i < dgvTransferProducts.RowCount; i++)
                {
                    int lotId = -1;
                    if (TransferLocationId == Convert.ToInt32(dgvTransferProducts[locationIdDataGridViewTextBoxColumn.Index, i].Value))
                        continue;

                    try
                    {
                        userTransferQty = Convert.ToDouble(dgvTransferProducts[TransferQuantity.Index, i].Value);
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                    }
                    if (userTransferQty == 0)
                        continue;

                    if (dgvTransferProducts[lotIdDataGridViewTextBoxColumn.Index, i].Value != DBNull.Value)
                    {
                        lotId = Convert.ToInt32(dgvTransferProducts[lotIdDataGridViewTextBoxColumn.Index, i].Value);
                    }
                    TransferQty = userTransferQty;

                    int productId = Convert.ToInt32(dgvTransferProducts[productIdDataGridViewTextBoxColumn.Index, i].Value);
                    int locationId = TransferLocationId;
                    inventoryList = new InventoryList();
                    InventoryDTO inventoryUpdateDTO = inventoryList.GetInventory(productId, locationId, lotId);
                    double currentQtyInDTO = 0;
                    if (inventoryUpdateDTO != null)
                    {
                        currentQtyInDTO = TransferQty + inventoryUpdateDTO.Quantity;
                        inventoryUpdateDTO.Quantity = currentQtyInDTO;
                    }
                    else
                    {
                        inventoryUpdateDTO = new InventoryDTO();
                        inventoryUpdateDTO.ProductId = productId;
                        inventoryUpdateDTO.LocationId = locationId;
                        inventoryUpdateDTO.Quantity = TransferQty;
                        inventoryUpdateDTO.LotId = lotId;
                    }
                    inventoryBl = new Inventory(inventoryUpdateDTO, executionContext);
                    if (inventoryBl.Save(SQLTrx) <= 0) // Location not found in inventory
                    {
                        inventoryInsertDTO.ProductId = productId;
                        inventoryInsertDTO.LocationId = TransferLocationId;
                        inventoryInsertDTO.Quantity = TransferQty;
                        inventoryInsertDTO.LotId = lotId;
                        inventoryInsertDTO.UOMId =  Convert.ToInt32(dgvProducts[UOMId.Index, i].Value);


                        inventoryBl = new Inventory(inventoryInsertDTO, executionContext);
                        inventoryBl.Save(SQLTrx);
                    }

                    inventoryUpdateDTO.LocationId = Convert.ToInt32(dgvTransferProducts[locationIdDataGridViewTextBoxColumn.Index, i].Value);

                    inventoryList = new InventoryList();
                    InventoryDTO tempInventoryDTO = inventoryList.GetInventory(inventoryUpdateDTO.ProductId, inventoryUpdateDTO.LocationId, inventoryUpdateDTO.LotId);
                    double currentQtyinInventoryDTO = 0;

                    if (tempInventoryDTO != null)
                    {
                        currentQtyinInventoryDTO = tempInventoryDTO.Quantity;
                    }

                    inventoryUpdateDTO.Quantity = TransferQty * -1 + currentQtyinInventoryDTO;
                    inventoryBl = new Inventory(inventoryUpdateDTO, executionContext);

                    if (inventoryBl.Save(SQLTrx) <= 0) // Location not found in inventory
                    {
                        inventoryInsertDTO.ProductId = Convert.ToInt32(dgvProducts[productIdDataGridViewTextBoxColumn.Index, i].Value); //Check
                        inventoryInsertDTO.LocationId = Convert.ToInt32(dgvTransferProducts[locationIdDataGridViewTextBoxColumn.Index, i].Value);
                        inventoryInsertDTO.Quantity = TransferQty * -1;
                        inventoryInsertDTO.LotId = lotId;
                        inventoryInsertDTO.UOMId = Convert.ToInt32(dgvProducts[UOMId.Index, i].Value);

                        inventoryBl = new Inventory(inventoryInsertDTO, executionContext);
                        inventoryBl.Save(SQLTrx);
                    }

                    inventoryAdjustmentsDTO.AdjustmentQuantity = TransferQty;
                    inventoryAdjustmentsDTO.FromLocationId = Convert.ToInt32(dgvTransferProducts[locationIdDataGridViewTextBoxColumn.Index, i].Value);
                    inventoryAdjustmentsDTO.ToLocationId = TransferLocationId;
                    inventoryAdjustmentsDTO.UOMId = Convert.ToInt32(dgvProducts[UOMId.Index, i].Value);
                    inventoryAdjustmentsDTO.ProductId = Convert.ToInt32(dgvTransferProducts[productIdDataGridViewTextBoxColumn.Index, i].Value);
                    inventoryAdjustmentsDTO.LotID = lotId;
                    inventoryAdjustmentsBl = new InventoryAdjustmentsBL(inventoryAdjustmentsDTO, executionContext);
                    inventoryAdjustmentsBl.Save(SQLTrx);
                }
                SQLTrx.Commit();

                MessageBox.Show(utilities.MessageUtils.getMessage(880), utilities.MessageUtils.getMessage("Transfer"));
                btnClearProducts.PerformClick();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(ex.Message, utilities.MessageUtils.getMessage("Transfer Error"));
                SQLTrx.Rollback();
            }
            log.LogMethodExit();
        }

        private void txtProdCode_TextChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ProdSearch();
            log.LogMethodExit();
        }

        private void txtProdCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.KeyChar == 13)
                btnSearch.PerformClick();

            log.LogMethodExit();
        }

        private void txtDescription_KeyPress(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.KeyChar == 13)
                btnSearch.PerformClick();

            log.LogMethodExit();
        }

        private void txtProdBarcode_KeyPress(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.KeyChar == 13)
                btnSearch.PerformClick();

            log.LogMethodExit();
        }

        bool segmentsExist()
        {
            log.LogMethodEntry();
            try
            {
                SegmentDefinitionList segmentDefinitionList = new SegmentDefinitionList(executionContext);
                List<SegmentDefinitionDTO> segmentDefinitionDTOList = new List<SegmentDefinitionDTO>();
                List<KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>> segmentDefinitionSearchParams = new List<KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>>();
                segmentDefinitionSearchParams = new List<KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>>();
                segmentDefinitionSearchParams.Add(new KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>(SegmentDefinitionDTO.SearchBySegmentDefinitionParameters.IS_ACTIVE, "Y"));
                segmentDefinitionSearchParams.Add(new KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>(SegmentDefinitionDTO.SearchBySegmentDefinitionParameters.SITE_ID, executionContext.GetSiteId().ToString()));
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
                MessageBox.Show(ex.Message);
                log.Error(ex);
                return false;
            }

        }

        //Start update to enable advanced search
        AdvancedSearch AdvancedSearch;
        private void btnAdvancedSearch_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (AdvancedSearch == null)
            {
                if (segmentsExist())
                {
                    AdvancedSearch = new AdvancedSearch(utilities, "Product", "p");
                    Semnox.Parafait.Inventory.CommonUIDisplay.setupVisuals(AdvancedSearch);
                    AdvancedSearch.StartPosition = FormStartPosition.CenterScreen;
                }
                else
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(1000));
                    log.LogMethodExit(utilities.MessageUtils.getMessage(1000));
                    return;
                }
            }
            if (AdvancedSearch.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    ProdSearch();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                    log.Error(ex);
                }
            }
            log.LogMethodExit();
        }
        //End update to enable advanced search


        private void rbFromLocation_CheckedChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            populateTransferLocation();
            log.LogMethodExit();
        }

        private void rbToLocation_CheckedChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            populateTransferLocation();
            log.LogMethodExit();
        }

        private void cbToLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            int locationId = Convert.ToInt32(cbToLocation.SelectedValue);
            LocationBL locationBL = new LocationBL(executionContext, locationId);
            if (locationBL.GetLocationDTO != null && locationBL.GetLocationDTO.LocationId > -1 && locationBL.GetLocationDTO.IsActive == false)
            {
                MessageBox.Show(utilities.MessageUtils.getMessage("Location is inactive."));
                cbToLocation.SelectedValue = -1;
            }
            log.LogMethodExit();
        }

        private void chkPurchaseable_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
