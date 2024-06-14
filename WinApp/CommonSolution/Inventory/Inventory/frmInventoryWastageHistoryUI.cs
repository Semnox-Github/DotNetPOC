/********************************************************************************************
* Project Name - Inventory 
* Description  - Inventory form for Inventory Wastage History 
* 
**************
**Version Log
**************
*Version     Date          Modified By         Remarks          
*********************************************************************************************
*2.70.2     27-Nov-2019    Girish kundar       Created 
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Category;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Product;

namespace Semnox.Parafait.Inventory
{
    public partial class frmInventoryWastageHistoryUI : Form
    {
        private Utilities utilities;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private SortableBindingList<InventoryWastageSummaryDTO> inventoryWastageSummaryDTOList;
        private List<InventoryWastageSummaryDTO> selectedInventoryWastageSummaryDTOList;
        private CustomCheckBox cbxHeaderSelectRecord = null;

        /// <summary>
        /// This is the parameterized constructor for frmInventoryWastageHistoryUI
        /// </summary>
        /// <param name="_Utilities">_Utilities</param>
        public frmInventoryWastageHistoryUI(Utilities _Utilities)
        {
            log.LogMethodEntry(_Utilities);
            InitializeComponent();
            utilities = _Utilities;
            executionContext = _Utilities.ExecutionContext;
            utilities.setLanguage(this);
            SetStyleAndLanguage();
            PopulateCategory();
            LoadGrid();
            CreateHeaderCheckBox();
            SetDateForDTP();
            log.LogMethodExit();
        }

        /// <summary>
        /// Get method for sortableBindingInventoryWastageSummaryDTOList
        /// </summary>
        public List<InventoryWastageSummaryDTO> SelectedInventoryWastageSummaryList
        {
            get { return selectedInventoryWastageSummaryDTOList; }
        }

        private void SetDateForDTP()
        {
            log.LogMethodEntry();
            dtpfromDate.Value = utilities.getServerTime().AddDays(-1);
            dtpToDate.Value = utilities.getServerTime();
            log.LogMethodExit();
        }
        /// <summary>
        /// Sets the cell style for the grid view columns
        /// </summary>
        private void SetStyleAndLanguage()
        {
            try
            {
                log.LogMethodEntry();
                utilities.setupDataGridProperties(ref dgvInventoryWastageHist);
                wastageQuantityDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewNumericCellStyle();
                wastageQuantityDataGridViewTextBoxColumn.DefaultCellStyle.Format = utilities.ParafaitEnv.INVENTORY_QUANTITY_FORMAT;
                productCodeDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewTextCellStyle();
                productDescriptionDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewTextCellStyle();
                categoryDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewTextCellStyle();
                uomDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewTextCellStyle();
                productDescriptionDataGridViewTextBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                lastUpdateDateDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewDateTimeCellStyle();
                WastageAdjustedDateDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewDateTimeCellStyle();
                ThemeUtils.SetupVisuals(this);
                utilities.setLanguage(this);
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void HeaderCheckBox_Clicked(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            CheckBox headerCheckBox = (sender as CheckBox);
            foreach (DataGridViewRow dataGridRow in dgvInventoryWastageHist.Rows)
            {
                dataGridRow.Cells["selectRecord"].Value = headerCheckBox.Checked;
            }
            dgvInventoryWastageHist.EndEdit();
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
        private void CreateHeaderCheckBox()
        {
            try
            {
                log.LogMethodEntry();
                cbxHeaderSelectRecord = new CustomCheckBox();
                cbxHeaderSelectRecord.FlatAppearance.BorderSize = 0;
                cbxHeaderSelectRecord.ImageAlign = ContentAlignment.MiddleCenter;
                cbxHeaderSelectRecord.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
                cbxHeaderSelectRecord.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
                cbxHeaderSelectRecord.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
                cbxHeaderSelectRecord.Text = string.Empty;
                cbxHeaderSelectRecord.Font = dgvInventoryWastageHist.Font;
                cbxHeaderSelectRecord.Location = new Point(dgvInventoryWastageHist.Columns["selectRecord"].HeaderCell.ContentBounds.X + 10, dgvInventoryWastageHist.Columns["selectRecord"].HeaderCell.ContentBounds.Y + 1);
                cbxHeaderSelectRecord.BackColor = Color.Transparent;
                cbxHeaderSelectRecord.Size = new Size(60, 28);
                cbxHeaderSelectRecord.Click += new EventHandler(HeaderCheckBox_Clicked);
                dgvInventoryWastageHist.Controls.Add(cbxHeaderSelectRecord);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// This method loads the History to the grid
        /// </summary>
        private void LoadGrid()
        {
            try
            {
                log.LogMethodEntry();
                DateTime currentDateTime = GetCurrentDateWithBusinessStartTime();
                InventoryAdjustmentsList inventoryAdjustmentsList = new InventoryAdjustmentsList(executionContext);
                List<KeyValuePair<InventoryWastageSummaryDTO.SearchByInventoryWastageParameters, string>> searchParameters = new List<KeyValuePair<InventoryWastageSummaryDTO.SearchByInventoryWastageParameters, string>>();
                searchParameters.Add(new KeyValuePair<InventoryWastageSummaryDTO.SearchByInventoryWastageParameters, string>(InventoryWastageSummaryDTO.SearchByInventoryWastageParameters.WASTAGE_FROM_DATE, currentDateTime.Date.AddDays(-1).ToString("yyyy-MM-dd hh:mm:ss", CultureInfo.InvariantCulture)));
                searchParameters.Add(new KeyValuePair<InventoryWastageSummaryDTO.SearchByInventoryWastageParameters, string>(InventoryWastageSummaryDTO.SearchByInventoryWastageParameters.WASTAGE_TO_DATE, currentDateTime.ToString("yyyy-MM-dd hh:mm:ss", CultureInfo.InvariantCulture)));
                searchParameters.Add(new KeyValuePair<InventoryWastageSummaryDTO.SearchByInventoryWastageParameters, string>(InventoryWastageSummaryDTO.SearchByInventoryWastageParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                List<InventoryWastageSummaryDTO> inventoryWastageSummaryList = inventoryAdjustmentsList.GetAllInventoryWastageSummaryDTOList(searchParameters);
                inventoryWastageSummaryDTOList = new SortableBindingList<InventoryWastageSummaryDTO>(inventoryWastageSummaryList);
                inventoryWastageSummaryDTOBindingSource.DataSource = inventoryWastageSummaryDTOList;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// This method handles the click event for the button load.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                log.LogMethodEntry();
                bool rowSelected = false;
                selectedInventoryWastageSummaryDTOList = new List<InventoryWastageSummaryDTO>();
                BindingSource focussedGridDataListDTOBS = (BindingSource)dgvInventoryWastageHist.DataSource;
                var inventoryWastageSummaryDTOList = (SortableBindingList<InventoryWastageSummaryDTO>)focussedGridDataListDTOBS.DataSource;
                if (inventoryWastageSummaryDTOList == null || inventoryWastageSummaryDTOList.Count == 0)
                {
                    log.Debug("Nothing to Add");
                    return;
                }
                log.LogVariableState("inventoryWastageSummaryDTOList", inventoryWastageSummaryDTOList);
                foreach (DataGridViewRow dataGridRow in dgvInventoryWastageHist.Rows)
                {
                    if (dataGridRow.Cells["selectRecord"].Value != null && (bool)dataGridRow.Cells["selectRecord"].Value)
                    {
                        rowSelected = true;
                        InventoryWastageSummaryDTO inventoryWastageSummaryDTO = inventoryWastageSummaryDTOList[dataGridRow.Index];
                        inventoryWastageSummaryDTO.AdjustmentId = -1;
                        inventoryWastageSummaryDTO.Remarks = string.Empty;
                        inventoryWastageSummaryDTO.TodaysWastageQuantity = 0;
                        selectedInventoryWastageSummaryDTOList.Add(inventoryWastageSummaryDTO);
                    }
                }
                if (!rowSelected)
                {
                    MessageBox.Show(Semnox.Parafait.Inventory.CommonFuncs.Utilities.MessageUtils.getMessage(2457), utilities.MessageUtils.getMessage("Validation Error"));
                    log.Debug("No rows selected. Please select the rows before clicking Add.");
                    return;
                }
                this.DialogResult = DialogResult.OK;
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
        /// This method handles the click event for the button Close  
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                log.LogMethodEntry();
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void PopulateCategory()
        {
            try
            {
                log.LogMethodEntry();
                CategoryList categoryList = new CategoryList(executionContext);
                List<KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>> categoryListSearchParams = new List<KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>>();
                categoryListSearchParams.Add(new KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>(CategoryDTO.SearchByCategoryParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                categoryListSearchParams.Add(new KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>(CategoryDTO.SearchByCategoryParameters.IS_ACTIVE, "Y"));
                List<CategoryDTO> categoryListOnDisplay = categoryList.GetAllCategory(categoryListSearchParams);

                if (categoryListOnDisplay == null)
                {
                    categoryListOnDisplay = new List<CategoryDTO>();
                }
                BindingSource categoryBS = new BindingSource();
                if (categoryListOnDisplay.Count > 0)
                {
                    categoryListOnDisplay = categoryListOnDisplay.OrderBy(x => x.Name).ToList();
                    categoryListOnDisplay.Insert(0, new CategoryDTO());
                    categoryListOnDisplay[0].Name = "-ALL-";
                    categoryBS.DataSource = categoryListOnDisplay;
                }
                else
                {
                    categoryBS.DataSource = categoryListOnDisplay;
                }
               
                cmbCategory.DataSource = categoryBS;
                cmbCategory.DisplayMember = "Name";
                cmbCategory.ValueMember = "CategoryId";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                log.LogMethodEntry();
                txtProductDescription.Text = string.Empty;
                cmbCategory.SelectedValue = -1;
                dtpfromDate.Value = utilities.getServerTime().AddDays(-1);
                dtpToDate.Value = utilities.getServerTime();
                inventoryWastageSummaryDTOList.Clear();
                cbxHeaderSelectRecord.Checked = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                log.LogMethodEntry();
                cbxHeaderSelectRecord.Checked = false;
                double totalDays = dtpToDate.Value.Subtract(dtpfromDate.Value).TotalDays;
                if (totalDays > 30)
                {
                    log.Debug("Date range for search should not be more than 30 days");
                    MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 2451), "Validation Error");
                    return;
                }
                if (totalDays < 0)
                {
                    //To date cannot be less than from date.
                    MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 1093), "Validation Error");
                    return;
                }
                double businessStartTime = ParafaitDefaultContainerList.GetParafaitDefault<double>(utilities.ExecutionContext, "BUSINESS_DAY_START_TIME", 6);
                DateTime fromDate = dtpfromDate.Value.Date.AddHours(businessStartTime);
                DateTime toDate = dtpToDate.Value.Date.AddDays(1).AddHours(businessStartTime);
                string categoryName = ((CategoryDTO)(cmbCategory.SelectedItem)).Name;
                string productDescription = txtProductDescription.Text.Trim();
                inventoryWastageSummaryDTOList = new SortableBindingList<InventoryWastageSummaryDTO>();
                InventoryAdjustmentsList inventoryAdjustmentsList = new InventoryAdjustmentsList(executionContext);
                List<KeyValuePair<InventoryWastageSummaryDTO.SearchByInventoryWastageParameters, string>> searchParameters = new List<KeyValuePair<InventoryWastageSummaryDTO.SearchByInventoryWastageParameters, string>>();
                if (Convert.ToInt32(cmbCategory.SelectedValue) != -1)
                {
                    searchParameters.Add(new KeyValuePair<InventoryWastageSummaryDTO.SearchByInventoryWastageParameters, string>(InventoryWastageSummaryDTO.SearchByInventoryWastageParameters.CATEGORY, categoryName.ToString()));
                }
                if (!string.IsNullOrEmpty(productDescription))
                {
                    searchParameters.Add(new KeyValuePair<InventoryWastageSummaryDTO.SearchByInventoryWastageParameters, string>(InventoryWastageSummaryDTO.SearchByInventoryWastageParameters.PRODUCT_DESCRIPTION, productDescription.ToString()));
                }
                searchParameters.Add(new KeyValuePair<InventoryWastageSummaryDTO.SearchByInventoryWastageParameters, string>(InventoryWastageSummaryDTO.SearchByInventoryWastageParameters.WASTAGE_FROM_DATE, fromDate.ToString("yyyy-MM-dd hh:mm:ss", CultureInfo.InvariantCulture)));
                searchParameters.Add(new KeyValuePair<InventoryWastageSummaryDTO.SearchByInventoryWastageParameters, string>(InventoryWastageSummaryDTO.SearchByInventoryWastageParameters.WASTAGE_TO_DATE, toDate.ToString("yyyy-MM-dd hh:mm:ss", CultureInfo.InvariantCulture)));
                searchParameters.Add(new KeyValuePair<InventoryWastageSummaryDTO.SearchByInventoryWastageParameters, string>(InventoryWastageSummaryDTO.SearchByInventoryWastageParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                List<InventoryWastageSummaryDTO> inventoryWastageSummaryList = inventoryAdjustmentsList.GetAllInventoryWastageSummaryDTOList(searchParameters);
                inventoryWastageSummaryDTOList = new SortableBindingList<InventoryWastageSummaryDTO>(inventoryWastageSummaryList);
                inventoryWastageSummaryDTOBindingSource.DataSource = inventoryWastageSummaryDTOList;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void dgvInventoryWastageHist_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                log.LogMethodEntry();
                dgvInventoryWastageHist.EndEdit();
                dgvInventoryWastageHist.CurrentCell.Selected = false;
                if (e.ColumnIndex == 0 && e.RowIndex >= 0)
                {
                    if (dgvInventoryWastageHist.Rows[e.RowIndex].Cells["selectRecord"].Value != null && (bool)dgvInventoryWastageHist.Rows[e.RowIndex].Cells["selectRecord"].Value)
                    {
                        dgvInventoryWastageHist.Rows[e.RowIndex].Cells["selectRecord"].Value = false;
                    }
                    else
                    {
                        dgvInventoryWastageHist.Rows[e.RowIndex].Cells["selectRecord"].Value = true;
                    }
                    dgvInventoryWastageHist.RefreshEdit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                log.Error(ex);
            }

            log.LogMethodExit();
        }
    }
}
