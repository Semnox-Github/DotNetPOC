/********************************************************************************************
* Project Name - Inventory 
* Description  - Inventory form for Waste Management 
* 
**************
**Version Log
**************
*Version     Date           Modified By         Remarks          
*********************************************************************************************
*2.70.2      27-Nov-2019    Girish kundar       Created 
*2.100.0     14-Aug-2020    Deeksha             Modified : Added UOM & Wastage type dropdown fields.
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Product;
using Semnox.Parafait.User;

namespace Semnox.Parafait.Inventory
{
    public partial class frmWasteManagementUI : Form
    {
        private Utilities utilities;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext = null;
        private bool fireCellValueChange = true;
        private int dgvselectedindex = -1;
        private int wastageLocationId = -1;
        private int wastageDocumentTypeId = -1;
        private int wastageAdjustmentTypeId = -1;
        private List<InventoryWastageSummaryDTO> inventoryWastageSummaryDTOList = new List<InventoryWastageSummaryDTO>();
        private InventoryDTO inventoryDTO = new InventoryDTO();
        private InventoryList inventoryList = null;
        private InventoryWastageSummaryDTO inventoryWastageSummaryDTO = null;
        private List<InventoryDTO> inventoryDTOList = null;
        private List<ValidationError> validationErrorList = new List<ValidationError>();
        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="_Utilities"></param>
        public frmWasteManagementUI(Utilities _Utilities)
        {
            log.LogMethodEntry(_Utilities);
            InitializeComponent();
            utilities = _Utilities;
            utilities.setLanguage(this);
            executionContext = _Utilities.ExecutionContext;
            if (utilities.ParafaitEnv.IsCorporate)
            {
                executionContext.SetSiteId(utilities.ParafaitEnv.SiteId);
            }
            else
            {
                executionContext.SetSiteId(-1);
            }
            executionContext.SetUserId(utilities.ParafaitEnv.LoginID);
            LoadAll();
            PopulateAdjustmentType();
            log.LogMethodExit();
        }

        private void LoadAll()
        {
            log.LogMethodEntry();
            try
            {
                GetWastageAdjustmentTypeID();
                GetWastageLocationtId();
                GetWastageDocumentTypeId();
                SetStyleAndLanguage();
                LoadGrid();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(ex.Message);
            }
            this.Cursor = Cursors.Default;
            log.LogMethodExit();
        }

        /// <summary>
        /// This method gets the adjustment type Id for the type "Wastage" 
        /// </summary>
        private void GetWastageAdjustmentTypeID()
        {
            try
            {
                log.LogMethodEntry();
                List<LookupValuesDTO> lookupValuesDTOList;
                LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                BindingSource lookupValuesBS = new BindingSource();
                lookupValuesDTOList = lookupValuesList.GetInventoryLookupValuesByValueName("INVENTORY_ADJUSTMENT_TYPE", executionContext.GetSiteId());
                log.LogVariableState("lookupValuesDTOList :", lookupValuesDTOList);
                if (lookupValuesDTOList != null)
                {
                    var lookupValuesDTO = lookupValuesDTOList.Where(x => x.LookupValue == "Wastage").FirstOrDefault();
                    wastageAdjustmentTypeId = lookupValuesDTO != null ? wastageAdjustmentTypeId = lookupValuesDTO.LookupValueId : -1;
                }
                log.Debug("wastageAdjustmentTypeId : " + wastageAdjustmentTypeId);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// This method gets the location id for the inventory location wastage of type department 
        /// </summary>
        private void GetWastageLocationtId()
        {
            try
            {
                log.LogMethodEntry();
                LocationList locationlist = new LocationList(executionContext);
                LocationDTO wastageLocationDTO = locationlist.GetWastageLocationDTO();
                if (wastageLocationDTO != null)
                {
                    wastageLocationId = wastageLocationDTO.LocationId;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                log.Error(ex);
            }
            log.LogMethodExit(wastageLocationId);
        }

        /// <summary>
        /// This method gets the document type id for Wastage type
        /// </summary>
        private void GetWastageDocumentTypeId()
        {
            try
            {
                log.LogMethodEntry();
                InventoryDocumentTypeList inventoryDocumentTypeList = new InventoryDocumentTypeList(executionContext);
                InventoryDocumentTypeDTO inventoryDocumentTypeDTO = inventoryDocumentTypeList.GetInventoryDocumentType("Wastage Adjustment");
                log.LogVariableState("inventoryDocumentTypeDTO :", inventoryDocumentTypeDTO);
                if (inventoryDocumentTypeDTO != null)
                {
                    wastageDocumentTypeId = inventoryDocumentTypeDTO.DocumentTypeId;
                }
                log.Debug("wastageDocumentTypeId : " + wastageDocumentTypeId);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// This method sets the loads the dgvInventoryWastage grid.
        /// </summary>
        private void LoadGrid()
        {
            try
            {
                log.LogMethodEntry();
                // inventoryWastageSummaryDTOList.Clear();
                dgvInventoryWastage.Refresh();
                DateTime currentDateTime = GetCurrentDateWithBusinessStartTime();
                InventoryAdjustmentsList inventoryAdjustmentsList = new InventoryAdjustmentsList(executionContext);
                List<InventoryWastageSummaryDTO> tempInventoryWastageSummaryDTOList = new List<InventoryWastageSummaryDTO>();
                List<InventoryWastageSummaryDTO> wastageSummaryDTOList = new List<InventoryWastageSummaryDTO>();
                List<KeyValuePair<InventoryWastageSummaryDTO.SearchByInventoryWastageParameters, string>> searchParameters = new List<KeyValuePair<InventoryWastageSummaryDTO.SearchByInventoryWastageParameters, string>>();
                searchParameters.Add(new KeyValuePair<InventoryWastageSummaryDTO.SearchByInventoryWastageParameters, string>(InventoryWastageSummaryDTO.SearchByInventoryWastageParameters.WASTAGE_FROM_DATE, currentDateTime.ToString("yyyy-MM-dd hh:mm:ss", CultureInfo.InvariantCulture)));
                searchParameters.Add(new KeyValuePair<InventoryWastageSummaryDTO.SearchByInventoryWastageParameters, string>(InventoryWastageSummaryDTO.SearchByInventoryWastageParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                tempInventoryWastageSummaryDTOList = inventoryAdjustmentsList.GetAllInventoryWastageSummaryDTOList(searchParameters);
                if (tempInventoryWastageSummaryDTOList != null && tempInventoryWastageSummaryDTOList.Count > 0)
                {
                    foreach (InventoryWastageSummaryDTO tempDTO in tempInventoryWastageSummaryDTOList)
                    {
                        if (wastageSummaryDTOList.Exists(x => x.InventoryId != -1 && x.InventoryId == tempDTO.InventoryId))
                        {
                            InventoryWastageSummaryDTO inventoryWastageSummaryDTO = wastageSummaryDTOList.Where(x => x.InventoryId == tempDTO.InventoryId).FirstOrDefault();
                            inventoryWastageSummaryDTO.TodaysWastageQuantity = inventoryWastageSummaryDTO.TodaysWastageQuantity + (tempDTO.TodaysWastageQuantity * -1);  // To display quantity as positive 
                            inventoryWastageSummaryDTO.Remarks = string.Empty;
                        }
                        else
                        {
                            tempDTO.Remarks = string.Empty;
                            tempDTO.TodaysWastageQuantity = tempDTO.TodaysWastageQuantity * -1;
                            wastageSummaryDTOList.Add(tempDTO);
                        }
                    }
                    inventoryWastageSummaryDTOList = wastageSummaryDTOList;
                }
                inventoryWastageSummaryDTOBindingSource.DataSource = inventoryWastageSummaryDTOList;
                dgvInventoryWastage.RefreshEdit();
                LoadUOMComboBox();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private DateTime GetCurrentDateWithBusinessStartTime()
        {
            log.LogMethodEntry();
            double businessStartTime = 6;
            DateTime currentDateTime = utilities.getServerTime();
            try
            {
                businessStartTime = ParafaitDefaultContainerList.GetParafaitDefault<double>(utilities.ExecutionContext, "BUSINESS_DAY_START_TIME", 6);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                businessStartTime = 6;
            }
            if (currentDateTime.Hour >= 0 && currentDateTime.Hour < businessStartTime)
            {
                currentDateTime = currentDateTime.AddDays(-1);
            }
            currentDateTime = currentDateTime.Date.AddHours(businessStartTime);
            log.LogMethodExit(currentDateTime);
            return currentDateTime;
        }

        private void SetStyleAndLanguage()
        {
            try
            {
                log.LogMethodEntry();
                utilities.setupDataGridProperties(ref dgvInventoryWastage);
                availableQuantityDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewNumericCellStyle();
                wastageQuantityDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewNumericCellStyle();
                TodaysWastageQuantityDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewNumericCellStyle();
                availableQuantityDataGridViewTextBoxColumn.DefaultCellStyle.Format = utilities.ParafaitEnv.INVENTORY_QUANTITY_FORMAT;
                wastageQuantityDataGridViewTextBoxColumn.DefaultCellStyle.Format = utilities.ParafaitEnv.INVENTORY_QUANTITY_FORMAT;
                TodaysWastageQuantityDataGridViewTextBoxColumn.DefaultCellStyle.Format = utilities.ParafaitEnv.INVENTORY_QUANTITY_FORMAT;
                productCodeDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewTextCellStyle();
                productDescriptionDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewTextCellStyle();
                //productDescriptionDataGridViewTextBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                ThemeUtils.SetupVisuals(this);
                utilities.setLanguage(this);
                fireCellValueChange = true;
                lblTitle.Text = lblTitle.Text + " " + DateTime.Now.DayOfWeek.ToString() + " ," + (DateTime.Now.Date.ToString("dd-MM-yyyy"));
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private List<InventoryDTO> GetProductDetails(string description)
        {
            try
            {
                log.LogMethodEntry(description);
                inventoryDTOList = new List<InventoryDTO>();
                inventoryList = new InventoryList();
                List<KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>> inventorySearchParams = new List<KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>>();
                inventorySearchParams.Add(new KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>(InventoryDTO.SearchByInventoryParameters.CODE, description));
                inventorySearchParams.Add(new KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>(InventoryDTO.SearchByInventoryParameters.DESCRIPTION, description));
                inventorySearchParams.Add(new KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>(InventoryDTO.SearchByInventoryParameters.BARCODE, description));
                inventoryDTOList = inventoryList.GetAllInventory(inventorySearchParams, true, null, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                log.Error(ex);
            }
            log.LogMethodExit(inventoryDTOList);
            return inventoryDTOList;
        }

        private void dgvInventoryWastage_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (e.ColumnIndex > -1 && e.RowIndex > -1)
                {
                    if (dgvInventoryWastage.Columns[e.ColumnIndex].Name == "productCodeDataGridViewTextBoxColumn")
                    {
                        if (inventoryWastageSummaryDTOList != null && inventoryWastageSummaryDTOList.Any())
                        {
                            InventoryWastageSummaryDTO inventoryWastageSummaryDTO = inventoryWastageSummaryDTOBindingSource.Current as InventoryWastageSummaryDTO;
                            if (inventoryWastageSummaryDTO.TodaysWastageQuantity > 0)
                            {
                                fireCellValueChange = false;
                            }
                        }
                        if (fireCellValueChange)
                        {
                            List<InventoryDTO> inventoryDTOList = new List<InventoryDTO>();
                            inventoryDTOList = GetProductDetails(dgvInventoryWastage[e.ColumnIndex, e.RowIndex].Value.ToString() + "%");
                            if (inventoryDTOList == null || inventoryDTOList.Any() == false)
                            {
                                MessageBox.Show(Semnox.Parafait.Inventory.CommonFuncs.Utilities.MessageUtils.getMessage(846), Semnox.Parafait.Inventory.CommonFuncs.Utilities.MessageUtils.getMessage("Find Products"));
                                dgvselectedindex = e.RowIndex;
                                dgvInventoryWastage.Rows.Remove(dgvInventoryWastage.Rows[dgvselectedindex]);
                            }

                            else if (inventoryDTOList.Count == 1)
                            {
                                dgvselectedindex = e.RowIndex;
                                InventoryDTO inventoryDTO = inventoryDTOList[0];
                                fireCellValueChange = false;
                                InventoryWastageSummaryDTO inventoryWastageSummaryDTO = inventoryWastageSummaryDTOBindingSource.Current as InventoryWastageSummaryDTO;
                                inventoryWastageSummaryDTO.InventoryId = inventoryDTO.InventoryId;
                                inventoryWastageSummaryDTO.ProductId = inventoryDTO.ProductId;
                                inventoryWastageSummaryDTO.LotId = inventoryDTO.LotId;
                                inventoryWastageSummaryDTO.AdjustmentId = -1;
                                inventoryWastageSummaryDTO.AvailableQuantity = Convert.ToDecimal(inventoryDTO.Quantity);
                                inventoryWastageSummaryDTO.OriginalQuantity = 0;
                                inventoryWastageSummaryDTO.WastageQuantity = 0;
                                inventoryWastageSummaryDTO.Category = inventoryDTO.CategoryName;
                                inventoryWastageSummaryDTO.UOM = inventoryDTO.UOM;
                                int uomId = inventoryDTO.UOMId;
                                ProductContainer productContainer = new ProductContainer(executionContext);
                                if (uomId == -1 && ProductContainer.productDTOList != null && ProductContainer.productDTOList.Count > 0)
                                {
                                    uomId = ProductContainer.productDTOList.Find(x => x.ProductId == inventoryDTO.ProductId).InventoryUOMId;
                                }
                                CommonFuncs.GetUOMComboboxForSelectedRows(dgvInventoryWastage, dgvselectedindex, uomId);
                                inventoryWastageSummaryDTO.UomId = inventoryDTO.UOMId;
                                inventoryWastageSummaryDTO.LocationName = inventoryDTO.LocationName;
                                inventoryWastageSummaryDTO.LocationId = inventoryDTO.LocationId;
                                inventoryWastageSummaryDTO.ProductCode = inventoryDTO.Code;
                                inventoryWastageSummaryDTO.ProductDescription = inventoryDTO.Description;
                                inventoryWastageSummaryDTO.Remarks = string.Empty;
                                inventoryWastageSummaryDTO.SiteId = inventoryDTO.SiteId;
                                inventoryWastageSummaryDTO.AdjustmentTypeId = wastageAdjustmentTypeId;
                                dgvInventoryWastage.Rows[dgvselectedindex].Cells["cmbAdjustmentType"].Value = inventoryWastageSummaryDTOList[dgvselectedindex].AdjustmentTypeId;
                                dgvInventoryWastage.Refresh();
                                dgvInventoryWastage.CurrentCell = dgvInventoryWastage.Rows[dgvselectedindex].Cells["wastageQuantityDataGridViewTextBoxColumn"];
                                fireCellValueChange = true;
                            }
                            else
                            {
                                dgvselectedindex = e.RowIndex;
                                Panel pnlMultiple_dgv = new Panel();
                                this.Controls.Add(pnlMultiple_dgv);
                                DataGridView multiple_dgv = new DataGridView();
                                pnlMultiple_dgv.Controls.Add(multiple_dgv);
                                multiple_dgv.LostFocus += new EventHandler(multiple_dgv_LostFocus);
                                multiple_dgv.Click += new EventHandler(multiple_dgv_Click);
                                multiple_dgv.Focus();
                                multiple_dgv.DataSource = inventoryDTOList;
                                multiple_dgv.Refresh();
                                multiple_dgv_Format(ref pnlMultiple_dgv, ref multiple_dgv); //Changed the function so that the grid is in a panel and scroll bar can be added if list is long
                            }
                            dgvInventoryWastage.Refresh();
                        }
                        fireCellValueChange = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        void multiple_dgv_LostFocus(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                DataGridView dg = (DataGridView)sender;
                if (dg.SelectedRows.Count == 0)
                {
                    dgvInventoryWastage.Rows.Remove(dgvInventoryWastage.Rows[dgvselectedindex]);
                    fireCellValueChange = true;
                }
                dg.Visible = false;
                dg.Parent.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        void multiple_dgv_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                fireCellValueChange = false;
                DataGridView dg = (DataGridView)sender;
                inventoryDTO = new InventoryDTO();
                inventoryWastageSummaryDTO = inventoryWastageSummaryDTOBindingSource.Current as InventoryWastageSummaryDTO;
                inventoryDTO = inventoryDTOList[dg.CurrentRow.Index];
                inventoryWastageSummaryDTO.InventoryId = inventoryDTO.InventoryId;
                inventoryWastageSummaryDTO.ProductId = inventoryDTO.ProductId;
                inventoryWastageSummaryDTO.LotId = inventoryDTO.InventoryLotDTO == null ? -1 : inventoryDTO.InventoryLotDTO.LotId;
                inventoryWastageSummaryDTO.AdjustmentId = -1;
                inventoryWastageSummaryDTO.AvailableQuantity = Convert.ToDecimal(inventoryDTO.Quantity);
                inventoryWastageSummaryDTO.WastageQuantity = 0;
                inventoryWastageSummaryDTO.OriginalQuantity = 0;
                inventoryWastageSummaryDTO.Category = inventoryDTO.CategoryName;
                int uomId = inventoryDTO.UOMId;
                ProductContainer productContainer = new ProductContainer(executionContext);
                if (uomId == -1 && ProductContainer.productDTOList != null && ProductContainer.productDTOList.Count > 0)
                {
                    uomId = ProductContainer.productDTOList.Find(x => x.ProductId == inventoryDTO.ProductId).InventoryUOMId;
                }
                inventoryWastageSummaryDTO.UomId = uomId;
                CommonFuncs.GetUOMComboboxForSelectedRows(dgvInventoryWastage, dgvselectedindex, uomId);
                inventoryWastageSummaryDTO.UOM = inventoryDTO.UOM;
                inventoryWastageSummaryDTO.LocationName = inventoryDTO.LocationName;
                inventoryWastageSummaryDTO.LocationId = inventoryDTO.LocationId;
                inventoryWastageSummaryDTO.ProductCode = inventoryDTO.Code;
                inventoryWastageSummaryDTO.ProductDescription = inventoryDTO.Description;
                inventoryWastageSummaryDTO.Remarks = string.Empty;
                inventoryWastageSummaryDTO.SiteId = inventoryDTO.SiteId;
                inventoryWastageSummaryDTO.AdjustmentTypeId = wastageAdjustmentTypeId;
                dgvInventoryWastage.Rows[dgvselectedindex].Cells["cmbAdjustmentType"].Value = inventoryWastageSummaryDTOList[dgvselectedindex].AdjustmentTypeId;
                dg.Parent.Visible = false;
                log.LogVariableState("inventoryWastageSummaryDTO", inventoryWastageSummaryDTO);
                dgvInventoryWastage.Refresh();
                dgvInventoryWastage.CurrentCell = dgvInventoryWastage.Rows[dgvselectedindex].Cells["wastageQuantityDataGridViewTextBoxColumn"];
                fireCellValueChange = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        void multiple_dgv_Format(ref Panel pnlMultiple_dgv, ref DataGridView multiple_dgv)
        {
            log.LogMethodEntry(pnlMultiple_dgv, multiple_dgv);
            try
            {
                pnlMultiple_dgv.Size = new Size(300, (dgvInventoryWastage.Rows[0].Cells[0].Size.Height * 10) - 3); //Changed the function so that the grid is in a panel and scroll bar can be added if list is long
                pnlMultiple_dgv.AutoScroll = true;
                pnlMultiple_dgv.Location = new Point(150 + dgvInventoryWastage.RowHeadersWidth + dgvInventoryWastage.CurrentRow.Cells["productCodeDataGridViewTextBoxColumn"].ContentBounds.Location.X, dgvInventoryWastage.Location.Y + dgvInventoryWastage.ColumnHeadersHeight);
                pnlMultiple_dgv.BringToFront();
                pnlMultiple_dgv.BorderStyle = BorderStyle.None;
                pnlMultiple_dgv.BackColor = Color.White;
                multiple_dgv.Width = 300;
                multiple_dgv.BorderStyle = BorderStyle.None;
                multiple_dgv.AllowUserToAddRows = false;
                multiple_dgv.BackgroundColor = Color.White;
                multiple_dgv.AllowUserToOrderColumns = true;

                for (int i = 0; i < multiple_dgv.Columns.Count; i++)
                {
                    if (multiple_dgv.Columns[i].HeaderText == "Description"
                       || multiple_dgv.Columns[i].HeaderText == "Location Name"
                       || multiple_dgv.Columns[i].HeaderText == "Code"
                       || multiple_dgv.Columns[i].HeaderText == "Lot#")
                    {
                        continue;
                    }
                    multiple_dgv.Columns[i].Visible = false;
                }
                multiple_dgv.Columns["Code"].DisplayIndex = 0;
                multiple_dgv.Columns["Description"].DisplayIndex = 1;
                multiple_dgv.Columns["LocationName"].DisplayIndex = 2;
                multiple_dgv.Columns["LotNumber"].DisplayIndex = 3;
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                log.Error(ex);
            }
            log.LogMethodExit();
        }

            private bool HasChanges()
            {
                log.LogMethodEntry();
                bool result = false;
                try
                {
                    List<InventoryWastageSummaryDTO> inventoryWastageSummaryDTOList = new List<InventoryWastageSummaryDTO>();
                    inventoryWastageSummaryDTOList = (List<InventoryWastageSummaryDTO>)inventoryWastageSummaryDTOBindingSource.DataSource;
                    log.LogVariableState("inventoryWastageSummaryDTOList", inventoryWastageSummaryDTOList);
                    if (inventoryWastageSummaryDTOList != null && inventoryWastageSummaryDTOList.Count > 0)
                    {
                        foreach (InventoryWastageSummaryDTO inventoryWastageSummaryDTO in inventoryWastageSummaryDTOList)
                        {
                            if (inventoryWastageSummaryDTO.IsChanged)
                            {
                                result = true;
                                break;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    log.Error(ex);
                }
                log.LogMethodExit(result);
                return result;
            }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                LoadGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (HasChanges())
                {
                    if (MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 127), MessageContainerList.GetMessage(utilities.ExecutionContext, "Unsaved Data"), MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        log.LogMethodExit();
                        return;
                    }
                }
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// This method is used to Save the inventory wastage details in to the table
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction();
            List<InventoryWastageSummaryDTO> inventoryWastageSummaryDTOList = new List<InventoryWastageSummaryDTO>();
            InventoryAdjustmentsDTO inventoryAdjustmentsDTO = null;
            Inventory inventoryBL = new Inventory(executionContext);
            bool rowWithZeroAvailableQty = false;
            if (!HasChanges())
            {
                MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 371));
                log.LogMethodExit("No changes to save");
                dgvInventoryWastage.Refresh();
                return;
            }

            try
            {
                dgvInventoryWastage.EndEdit();
                parafaitDBTrx.BeginTransaction();
                inventoryWastageSummaryDTOList = (List<InventoryWastageSummaryDTO>)inventoryWastageSummaryDTOBindingSource.DataSource;
                for (int i = 0; i < inventoryWastageSummaryDTOList.Count; i++)
                {
                    if (inventoryWastageSummaryDTOList[i].WastageQuantity == 0 && inventoryWastageSummaryDTOList[i].AvailableQuantity == 0 &&
                        inventoryWastageSummaryDTOList[i].TodaysWastageQuantity == 0)
                    {
                        rowWithZeroAvailableQty = true;
                    }
                }
                if (rowWithZeroAvailableQty)
                {
                    MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 2452), "Validation", MessageBoxButtons.OK);
                }

                log.LogVariableState("inventoryWastageSummaryDTOList", inventoryWastageSummaryDTOList);
                for (int i = 0; i < inventoryWastageSummaryDTOList.Count; i++)
                {
                    int inventoryUOMId = inventoryWastageSummaryDTOList[i].UomId;
                    int UserEntereduomId = Convert.ToInt32(dgvInventoryWastage.Rows[i].Cells["cmbUOM"].Value);
                    if (inventoryUOMId != UserEntereduomId)
                    {
                        decimal factor = Convert.ToDecimal(UOMContainer.GetConversionFactor(UserEntereduomId, inventoryUOMId));
                        inventoryWastageSummaryDTOList[i].WastageQuantity = inventoryWastageSummaryDTOList[i].WastageQuantity * factor;
                    }
                }
                validationErrorList = Validate(inventoryWastageSummaryDTOList);
                if (validationErrorList != null && validationErrorList.Any())
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Validation Error"), validationErrorList);
                }
                if (inventoryWastageSummaryDTOList != null && inventoryWastageSummaryDTOList.Count > 0)
                {
                    for (int i = 0; i < inventoryWastageSummaryDTOList.Count; i++)
                    {
                        inventoryWastageSummaryDTO = inventoryWastageSummaryDTOList[i];
                        if (inventoryWastageSummaryDTO.IsChanged)
                        {
                            if (inventoryWastageSummaryDTO.WastageQuantity == 0 && inventoryWastageSummaryDTO.AvailableQuantity == 0 &&
                                 inventoryWastageSummaryDTO.TodaysWastageQuantity == 0)
                            {
                                continue;
                            }
                            try
                            {
                                inventoryList = new InventoryList();
                                decimal currentQtyInDTO = Convert.ToDecimal(inventoryWastageSummaryDTO.AvailableQuantity) - inventoryWastageSummaryDTO.WastageQuantity;
                                if (inventoryWastageSummaryDTO.LotId != -1)
                                {
                                    List<KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>> inventorySearchParams = new List<KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>>();
                                    inventorySearchParams.Add(new KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>(InventoryDTO.SearchByInventoryParameters.PRODUCT_ID, inventoryWastageSummaryDTO.ProductId.ToString()));
                                    inventorySearchParams.Add(new KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>(InventoryDTO.SearchByInventoryParameters.LOCATION_ID, inventoryWastageSummaryDTO.LocationId.ToString()));
                                    inventorySearchParams.Add(new KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>(InventoryDTO.SearchByInventoryParameters.LOT_ID, inventoryWastageSummaryDTO.LotId.ToString()));
                                    inventoryDTOList = inventoryList.GetAllInventory(inventorySearchParams, true, parafaitDBTrx.SQLTrx);
                                    if (inventoryDTOList != null && inventoryDTOList.Count > 0)
                                    {
                                        inventoryDTO = inventoryDTOList[0];
                                    }
                                }
                                else
                                {
                                    inventoryDTO = inventoryList.GetInventory(inventoryWastageSummaryDTO.ProductId, inventoryWastageSummaryDTO.LocationId, inventoryWastageSummaryDTO.LotId);
                                }

                                if (inventoryWastageSummaryDTO.AdjustmentId == -1)
                                {
                                    inventoryAdjustmentsDTO = new InventoryAdjustmentsDTO();
                                    if (inventoryDTO != null)
                                    {
                                        inventoryDTO.Quantity = Convert.ToDouble(currentQtyInDTO);
                                        inventoryDTO.UOMId = inventoryWastageSummaryDTO.UomId;
                                        inventoryBL = new Inventory(inventoryDTO, executionContext);
                                        if (inventoryBL.Save(parafaitDBTrx.SQLTrx) <= 0)
                                        {
                                            log.Error("inventoryBL.Save() : Inventory Quantity update failed");
                                            throw new Exception("Inventory Quantity update failed");
                                        }
                                    }
                                    if (inventoryDTO.LotId != -1)
                                    {
                                        double lotQuantity = inventoryDTO.Quantity;
                                        Semnox.Parafait.Inventory.InventoryList inventoryList1 = new InventoryList();
                                        List<KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>> searchParams = new List<KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>>();
                                        searchParams.Add(new KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>(InventoryDTO.SearchByInventoryParameters.LOT_ID, inventoryDTO.LotId.ToString()));
                                        List<InventoryDTO> inventoryLotDTOList = inventoryList1.GetAllInventory(searchParams);
                                        if (inventoryLotDTOList != null)
                                        {
                                            foreach (InventoryDTO tempInventoryDTO in inventoryLotDTOList)
                                            {
                                                // Skip the quantity from the wastage location while calculating lot quantity from other location
                                                if (tempInventoryDTO.LocationId != inventoryDTO.LocationId && tempInventoryDTO.LocationId != wastageLocationId)
                                                    lotQuantity += tempInventoryDTO.Quantity;
                                            }
                                        }
                                        inventoryDTO.InventoryLotDTO.BalanceQuantity = lotQuantity;
                                        inventoryDTO.InventoryLotDTO.UOMId = inventoryDTO.UOMId;
                                        InventoryLotBL inventoryLotBL = new InventoryLotBL(inventoryDTO.InventoryLotDTO, executionContext);
                                        inventoryLotBL.Save(parafaitDBTrx.SQLTrx);
                                    }

                                    // Saving inventory to wastage location : If same product exists then update the wastage quantity eg: previous day added same product to wastage location
                                    inventoryDTO = inventoryList.GetInventory(inventoryWastageSummaryDTO.ProductId, wastageLocationId, inventoryWastageSummaryDTO.LotId);
                                    if (inventoryDTO != null)
                                    {
                                        inventoryDTO.Quantity = inventoryDTO.Quantity + Convert.ToDouble(inventoryWastageSummaryDTO.WastageQuantity);
                                    }
                                    else  // add new record for wastage location
                                    {
                                        inventoryDTO = new InventoryDTO(inventoryWastageSummaryDTO.ProductId, wastageLocationId, Convert.ToDouble(inventoryWastageSummaryDTO.WastageQuantity), DateTime.Now,
                                        0, inventoryWastageSummaryDTO.Remarks, inventoryWastageSummaryDTO.LotId, 0, -1, "", inventoryWastageSummaryDTO.ProductCode,
                                        inventoryWastageSummaryDTO.ProductDescription, "Y", null, "", "", "", "", 0, 0, "", inventoryWastageSummaryDTO.LocationName, "", "", "", inventoryWastageSummaryDTO.UomId );
                                    }

                                    inventoryBL = new Inventory(inventoryDTO, executionContext);
                                    inventoryBL.Save(parafaitDBTrx.SQLTrx);
                                    inventoryAdjustmentsDTO.AdjustmentQuantity = Convert.ToDouble(inventoryWastageSummaryDTO.WastageQuantity) * -1; // Save as negative quantity
                                    string adjustmentType = dgvInventoryWastage.Rows[i].Cells["cmbAdjustmentType"].FormattedValue.ToString();
                                    inventoryAdjustmentsDTO.AdjustmentType = adjustmentType;
                                    inventoryAdjustmentsDTO.ProductId = inventoryWastageSummaryDTO.ProductId;
                                    inventoryAdjustmentsDTO.Remarks = inventoryWastageSummaryDTO.Remarks;
                                    inventoryAdjustmentsDTO.Timestamp = DateTime.Now;
                                    inventoryAdjustmentsDTO.FromLocationId = inventoryWastageSummaryDTO.LocationId;
                                    inventoryAdjustmentsDTO.UserId = executionContext.GetUserId();
                                    inventoryAdjustmentsDTO.UOMId = inventoryWastageSummaryDTO.UomId;
                                    inventoryAdjustmentsDTO.DocumentTypeID = wastageDocumentTypeId; // Applicability - WADJ - Wastage Adjustment
                                    inventoryAdjustmentsDTO.ToLocationId = wastageLocationId; // Always go to the location wastage 
                                    int adjustmentTypeId = Convert.ToInt32(dgvInventoryWastage.Rows[i].Cells["cmbAdjustmentType"].Value);
                                    if(adjustmentTypeId != -1)
                                    {
                                        inventoryAdjustmentsDTO.AdjustmentTypeId = adjustmentTypeId;  
                                    }
                                    else
                                    { 
                                        inventoryAdjustmentsDTO.AdjustmentTypeId = wastageAdjustmentTypeId;
                                    }
                                    inventoryAdjustmentsDTO.LotID = inventoryWastageSummaryDTO.LotId;
                                }

                                InventoryAdjustmentsBL inventoryAdjustmentsBL = new InventoryAdjustmentsBL(inventoryAdjustmentsDTO, executionContext);
                                inventoryAdjustmentsBL.Save(parafaitDBTrx.SQLTrx);
                            }
                            catch (Exception ex)
                            {
                                parafaitDBTrx.RollBack();
                                MessageBox.Show(ex.Message, utilities.MessageUtils.getMessage("Save Error"));
                                return;
                            }
                        }
                    }
                    parafaitDBTrx.EndTransaction();
                    LoadGrid();
                    MessageBox.Show(utilities.MessageUtils.getMessage(122), utilities.MessageUtils.getMessage("Save"));
                }
                else
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(371));
                }
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                parafaitDBTrx.RollBack();
                MessageBox.Show(utilities.MessageUtils.getMessage("Save Error") + " " + ex.Message, "Save Error");
                log.Error("Error in save. Exception:" + ex.ToString());
            }
        }

        /// <summary>
        /// This method  the validate the each InventoryWastageSummaryDTO in the inventoryWastageSummaryDTOList
        /// </summary>
        /// <param name="inventoryWastageSummaryDTOList">inventoryWastageSummaryDTOList</param>
        /// <returns>bool </returns>
        private List<ValidationError> Validate(List<InventoryWastageSummaryDTO> inventoryWastageSummaryDTOList)
        {
            log.LogMethodEntry(inventoryWastageSummaryDTOList);
            validationErrorList.Clear();
            var query = inventoryWastageSummaryDTOList.GroupBy(x => new { x.ProductId, x.LocationId })
               .Where(g => g.Count() > 1)
               .Select(y => y.Key)
               .ToList();
            if (query.Count > 0)
            {
                log.Debug("Duplicate entries detail : " + query[0]);
                string errorMessage = MessageContainerList.GetMessage(utilities.ExecutionContext, 2441);
                validationErrorList.Add(new ValidationError("Wastage", "Product to Location", errorMessage));
                return validationErrorList;
            }
            for (int i = 0; i < inventoryWastageSummaryDTOList.Count; i++)
            {
                if (inventoryWastageSummaryDTOList[i].IsChanged)
                {
                    if (inventoryWastageSummaryDTOList[i].WastageQuantity == 0 && inventoryWastageSummaryDTOList[i].AvailableQuantity == 0
                        && inventoryWastageSummaryDTOList[i].TodaysWastageQuantity == 0)
                    {
                        continue;
                    }
                    if (Convert.ToDecimal(inventoryWastageSummaryDTOList[i].AvailableQuantity.ToString()) <= 0
                         && inventoryWastageSummaryDTOList[i].AdjustmentId == -1)
                    {
                        string errorMessage = MessageContainerList.GetMessage(utilities.ExecutionContext, 2453, i + 1);
                        validationErrorList.Add(new ValidationError("Wastage", "Available Quantity", errorMessage));
                        log.Debug(utilities.MessageUtils.getMessage(2453, utilities.MessageUtils.getMessage("Available Quantity")));

                    }

                    if (inventoryWastageSummaryDTOList[i].WastageQuantity <= 0)
                    {
                        string errorMessage = MessageContainerList.GetMessage(utilities.ExecutionContext, 2467, i + 1);
                        log.Debug(utilities.MessageUtils.getMessage(2467, utilities.MessageUtils.getMessage("Wastage Quantity")));
                        validationErrorList.Add(new ValidationError("Wastage", "ValidationError", errorMessage));
                    }

                    decimal currentQtyInDTO = Convert.ToDecimal(inventoryWastageSummaryDTOList[i].AvailableQuantity) - inventoryWastageSummaryDTOList[i].WastageQuantity;
                    if (currentQtyInDTO < 0)
                    {
                        log.Debug(utilities.MessageUtils.getMessage(2477, utilities.MessageUtils.getMessage("Wastage Quantity")));
                        string errorMessage = MessageContainerList.GetMessage(utilities.ExecutionContext, 2477, i + 1);
                        validationErrorList.Add(new ValidationError("Wastage", "Wastage Quantity", errorMessage));
                    }
                }
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }


        private void dgvInventoryWastage_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                e.Row.Cells["availableQuantityDataGridViewTextBoxColumn"].Value = "0.0";
                e.Row.Cells["wastageQuantityDataGridViewTextBoxColumn"].Value = "0.0";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                this.Cursor = Cursors.WaitCursor;
                StringBuilder duplicateEntryMessage = new StringBuilder(string.Empty);
                dgvInventoryWastage.EndEdit();
                List<InventoryWastageSummaryDTO> tempInventoryWastageSummaryDTOList = new List<InventoryWastageSummaryDTO>(inventoryWastageSummaryDTOList);
                using (frmInventoryWastageHistoryUI frmInventoryWastageHistoryUI = new frmInventoryWastageHistoryUI(utilities))
                {
                    if (frmInventoryWastageHistoryUI.ShowDialog() == DialogResult.OK)
                    {
                        if (frmInventoryWastageHistoryUI.SelectedInventoryWastageSummaryList != null && frmInventoryWastageHistoryUI.SelectedInventoryWastageSummaryList.Any())
                        {
                            foreach (InventoryWastageSummaryDTO inventoryWastageSummaryDTO in frmInventoryWastageHistoryUI.SelectedInventoryWastageSummaryList)
                            {
                                if (tempInventoryWastageSummaryDTOList.Exists(x => x.ProductId == inventoryWastageSummaryDTO.ProductId && x.LocationId == inventoryWastageSummaryDTO.LocationId) == false)
                                {
                                    tempInventoryWastageSummaryDTOList.Add(inventoryWastageSummaryDTO);
                                }
                                else
                                {
                                    if (duplicateEntryMessage.ToString().Contains("Product Code: " + inventoryWastageSummaryDTO.ProductCode + " To Location:" + inventoryWastageSummaryDTO.LocationName) == false)
                                    {
                                        duplicateEntryMessage.Append("Product Code: " + inventoryWastageSummaryDTO.ProductCode + " To Location:" + inventoryWastageSummaryDTO.LocationName);
                                        duplicateEntryMessage.AppendLine();
                                    }
                                }
                            }
                            inventoryWastageSummaryDTOBindingSource.DataSource = tempInventoryWastageSummaryDTOList;
                            LoadUOMComboBox();
                        }
                    }
                }
                if (string.IsNullOrEmpty(duplicateEntryMessage.ToString()) == false)
                {
                    //Wastage details for the following records are already exists. Records are not added'
                    duplicateEntryMessage.Insert(0, MessageContainerList.GetMessage(executionContext, 2468) + " \n");
                    MessageBox.Show(MessageContainerList.GetMessage(executionContext, duplicateEntryMessage.ToString()), MessageContainerList.GetMessage(executionContext, "Validation"));
                }
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void dgvInventoryWastage_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if (e.Exception != null)
            {
                log.Error(e);
            }
            e.Cancel = true;
        }

        private void dgvInventoryWastage_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (dgvInventoryWastage.Rows.Count > 0 && e.RowIndex > -1)
                {
                    dgvInventoryWastage.Rows[e.RowIndex].Cells["wastageQuantityDataGridViewTextBoxColumn"].Style.BackColor = Color.PowderBlue;
                    dgvInventoryWastage.Rows[e.RowIndex].Cells["remarksDataGridViewTextBoxColumn"].Style.BackColor = Color.PowderBlue;
                }

                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        private void dgvInventoryWastage_Paint(object sender, PaintEventArgs e)
        {
            log.LogMethodEntry();
            if (inventoryWastageSummaryDTOList != null && inventoryWastageSummaryDTOList.Any())
            {
                for (int i = 0; i < inventoryWastageSummaryDTOList.Count; i++)
                {
                    if (inventoryWastageSummaryDTOList[i].InventoryId != -1 && inventoryWastageSummaryDTOList[i].TodaysWastageQuantity > 0)
                        dgvInventoryWastage.Rows[i].Cells["productCodeDataGridViewTextBoxColumn"].ReadOnly = true;
                }
            }
            log.LogMethodExit();
        }

        private void LoadUOMComboBox()
        {
            log.LogMethodEntry();
            try
            {
                BindingSource bindingSource = (BindingSource)dgvInventoryWastage.DataSource;
                List<InventoryWastageSummaryDTO> inventoryWastageSummaryDTOList = (List<InventoryWastageSummaryDTO>)bindingSource.DataSource;

                for (int i = 0; i < inventoryWastageSummaryDTOList.Count; i++)
                {
                    int uomId = inventoryWastageSummaryDTOList[i].UomId;
                    CommonFuncs.GetUOMComboboxForSelectedRows(dgvInventoryWastage, i, uomId);
                    dgvInventoryWastage.Rows[i].Cells["cmbAdjustmentType"].Value = inventoryWastageSummaryDTOList[i].AdjustmentTypeId;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }


        private void PopulateAdjustmentType()
        {
            log.LogMethodEntry();
            try
            {
                List<LookupValuesDTO> lookupValuesDTOList;
                BindingSource wastageAdjustmentTypeBS = new BindingSource();
                LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                lookupValuesDTOList = lookupValuesList.GetInventoryLookupValuesByValueName("INVENTORY_ADJUSTMENT_TYPE", executionContext.GetSiteId());
                if (lookupValuesDTOList != null)
                {
                    lookupValuesDTOList = lookupValuesDTOList.OrderBy(x => x.LookupName).ToList();
                    lookupValuesDTOList.Insert(0, new LookupValuesDTO());
                    lookupValuesDTOList[0].LookupName = "-ALL-";
                    wastageAdjustmentTypeBS.DataSource = lookupValuesDTOList;
                }
                else
                {
                    wastageAdjustmentTypeBS.DataSource = lookupValuesDTOList;
                }
                cmbAdjustmentType.DataSource = wastageAdjustmentTypeBS;
                cmbAdjustmentType.DisplayMember = "LookupValue";
                cmbAdjustmentType.ValueMember = "LookupValueId";
                dgvInventoryWastage.Rows[0].Cells["cmbAdjustmentType"].Value = wastageAdjustmentTypeId;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void frmWasteManagementUI_Activated(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            LoadUOMComboBox();
            log.LogMethodExit();
        }
    }
}