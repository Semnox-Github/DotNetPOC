/********************************************************************************************
 * Project Name - Inventory
 * Description  - UI for Add Recipe UI
 * 
 **************
 **Version Log
 **************
 *Version       Date          Modified By         Remarks          
 *********************************************************************************************
 *2.100.0       07-Sep-2020   Deeksha             Created for Recipe Management enhancement.
 *********************************************************************************************/
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;
using System.Collections.Generic;
using System.Globalization;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Inventory.Recipe
{
    public partial class frmAddRecipeUI : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext = ExecutionContext.GetExecutionContext();
        private Utilities utilities;
        private List<ProductDTO> productDTOList = null;
        private List<RecipePlanHeaderDTO> recipePlanHeaderDTOList = new List<RecipePlanHeaderDTO>();
        private List<RecipePlanDetailsDTO> recipePlanDetailsDTOListOnDisplay = new List<RecipePlanDetailsDTO>();

        public frmAddRecipeUI(Utilities utilities, DateTime planDate)
        {
            log.LogMethodEntry(utilities, planDate);
            InitializeComponent();
            this.utilities = utilities;
            ProductContainer productContainer = new ProductContainer(executionContext);
            UOMContainer uOMContainer = CommonFuncs.GetUOMContainer();
            dtpStartDate.Value = planDate;
            dtpEndDate.Value = planDate.AddDays(1);
            log.LogMethodExit();
        }


        private void frmAddRecipeUI_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            utilities.setupDataGridProperties(ref dgvAddRecipe);
            finalQtyDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewNumericCellStyle();
            finalQtyDataGridViewTextBoxColumn.DefaultCellStyle.Format = utilities.ParafaitEnv.INVENTORY_QUANTITY_FORMAT;
            qtyModifiedDateDataGridViewTextBoxColumn.DefaultCellStyle.Format = utilities.getDateFormat();
            creationDateDataGridViewTextBoxColumn.DefaultCellStyle.Format = utilities.getDateFormat();
            lastUpdateDateDataGridViewTextBoxColumn.DefaultCellStyle.Format = utilities.getDateFormat();
            btnSave.Text = MessageContainerList.GetMessage(executionContext, "Save");
            btnRefresh.Text = MessageContainerList.GetMessage(executionContext, "Refresh");
            btnCancel.Text = MessageContainerList.GetMessage(executionContext, "Cancel");
            grpReccur.Text = MessageContainerList.GetMessage(executionContext, "Recurrence");
            rdWeekly.Text = MessageContainerList.GetMessage(executionContext, "Weekly");
            rdMonthly.Text = MessageContainerList.GetMessage(executionContext, "Monthly");
            rdDaily.Text = MessageContainerList.GetMessage(executionContext, "Daily");
            cbxSun.Text = MessageContainerList.GetMessage(executionContext, "Sun");
            cbxMon.Text = MessageContainerList.GetMessage(executionContext, "Mon");
            cbxWen.Text = MessageContainerList.GetMessage(executionContext, "Wed");
            cbxThu.Text = MessageContainerList.GetMessage(executionContext, "Thu");
            cbxFri.Text = MessageContainerList.GetMessage(executionContext, "Fri");
            cbxSat.Text = MessageContainerList.GetMessage(executionContext, "Sat");
            LoadGrid();
            log.LogMethodExit();
        }

        /// <summary>
        /// Method to load Plan details on the grid
        /// </summary>
        private void LoadGrid()
        {
            try
            {
                log.LogMethodEntry();
                recipePlanDetailsDTOListOnDisplay = new List<RecipePlanDetailsDTO>();
                recipePlanHeaderDTOList = new List<RecipePlanHeaderDTO>();
                RecipePlanHeaderListBL recipePlanHeaderListBL = new RecipePlanHeaderListBL(executionContext);
                List<KeyValuePair<RecipePlanHeaderDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<RecipePlanHeaderDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<RecipePlanHeaderDTO.SearchByParameters, string>(RecipePlanHeaderDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                DateTime date = dtpStartDate.Value;
                searchParameters.Add(new KeyValuePair<RecipePlanHeaderDTO.SearchByParameters, string>(RecipePlanHeaderDTO.SearchByParameters.FROM_DATE, date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)));
                searchParameters.Add(new KeyValuePair<RecipePlanHeaderDTO.SearchByParameters, string>(RecipePlanHeaderDTO.SearchByParameters.TO_DATE, date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)));
                recipePlanHeaderDTOList = recipePlanHeaderListBL.GetAllRecipePlanHeaderDTOList(searchParameters, true, true);
                if (recipePlanHeaderDTOList != null && recipePlanHeaderDTOList.Count > 0)
                {
                    dtpStartDate.Value = recipePlanHeaderDTOList[0].PlanDateTime.Date;
                    if (recipePlanHeaderDTOList[0].RecurFlag == true)
                    {
                        cbxRecur.Checked = true;
                        dtpEndDate.Enabled = true;
                        rdDaily.Enabled = true;
                        rdWeekly.Enabled = true;
                        rdMonthly.Enabled = true;
                        if (recipePlanHeaderDTOList[0].RecurFrequency.Equals('D'))
                        {
                            rdDaily.Checked = true;
                        }
                        if (recipePlanHeaderDTOList[0].RecurFrequency.Equals('W'))
                        {
                            pnlDays.Enabled = true;
                            rdWeekly.Checked = true;
                        }
                        if (recipePlanHeaderDTOList[0].RecurFrequency.Equals('M'))
                        {
                            pnlMonth.Enabled = true;
                            rdMonthly.Checked = true;
                        }

                        if (recipePlanHeaderDTOList[0].Sunday == true)
                        {
                            cbxSun.Checked = true;
                        }
                        if (recipePlanHeaderDTOList[0].Monday == true)
                        {
                            cbxMon.Checked = true;
                        }
                        if (recipePlanHeaderDTOList[0].Tuesday == true)
                        {
                            cbxTue.Checked = true;
                        }
                        if (recipePlanHeaderDTOList[0].Wednesday == true)
                        {
                            cbxWen.Checked = true;
                        }
                        if (recipePlanHeaderDTOList[0].Thursday == true)
                        {
                            cbxThu.Checked = true;
                        }
                        if (recipePlanHeaderDTOList[0].Friday == true)
                        {
                            cbxFri.Checked = true;
                        }
                        if (recipePlanHeaderDTOList[0].Saturday == true)
                        {
                            cbxSat.Checked = true;
                        }
                        dtpEndDate.Value = Convert.ToDateTime(recipePlanHeaderDTOList[0].RecurEndDate);
                    }
                    recipePlanDetailsDTOListOnDisplay = recipePlanHeaderDTOList[0].RecipePlanDetailsDTOList;
                    foreach (RecipePlanDetailsDTO recipePlanDTO in recipePlanDetailsDTOListOnDisplay)
                    {
                        if (ProductContainer.productDTOList != null && ProductContainer.productDTOList.Count > 0)
                        {
                            ProductDTO productDTO = ProductContainer.productDTOList.Find(x => x.ProductId == recipePlanDTO.ProductId);
                            if (productDTO != null)
                            {
                                recipePlanDTO.RecipeName = productDTO.Description;
                                recipePlanDTO.UOM = UOMContainer.uomDTOList.Find(x => x.UOMId == recipePlanDTO.UOMId).UOM;
                            }
                            recipePlanDTO.AcceptChanges();
                        }
                        recipePlanHeaderDTOList[0].AcceptChanges();
                    }
                }
                else
                {
                    rdDaily.Enabled = false;
                    rdWeekly.Enabled = false;
                    rdMonthly.Enabled = false;
                    pnlDays.Enabled = false;
                    pnlMonth.Enabled = false;
                    dtpEndDate.Enabled = false;
                    dtpEndDate.Value = dtpStartDate.Value.AddDays(1);
                }
                recipePlanDetailsDTOBindingSource.DataSource = recipePlanDetailsDTOListOnDisplay;
                dgvAddRecipe.RefreshEdit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.Close();
            log.LogMethodExit();
        }

        private void dgvAddRecipe_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            if (e.ColumnIndex > -1 && e.RowIndex > -1)
            {
                if (dgvAddRecipe.Columns[e.ColumnIndex].Name == "RecipeName")
                {
                    try
                    {
                        ProductList inventoryProductList = new ProductList();
                        string description = dgvAddRecipe[e.ColumnIndex, e.RowIndex].Value.ToString();
                        if (description.Contains("%"))
                        {
                            description = string.Empty;
                        }
                        if (ProductContainer.productDTOList != null && ProductContainer.productDTOList.Count > 0)
                        {
                            productDTOList = ProductContainer.productDTOList.Where(x => x.Description.IndexOf(description, StringComparison.OrdinalIgnoreCase) != -1 |
                                                                                   x.Code.IndexOf(description, StringComparison.OrdinalIgnoreCase) != -1).ToList();
                            if (productDTOList != null && productDTOList.Count > 0)
                            {
                                productDTOList = productDTOList.FindAll(x => x.IncludeInPlan == true);
                            }
                        }
                        if (productDTOList == null || productDTOList.Count < 1)
                        {
                            MessageBox.Show(utilities.MessageUtils.getMessage(846), utilities.MessageUtils.getMessage("Find Products"));
                            BeginInvoke((MethodInvoker)delegate
                            {
                                RemoveDGVRow(e.RowIndex);
                            });
                        }
                        else if (productDTOList.Count == 1)
                        {
                            ProductDTO productDTO = productDTOList[0];
                            RecipePlanDetailsDTO recipePlanDetailsDTO = recipePlanDetailsDTOBindingSource.Current as RecipePlanDetailsDTO;
                            recipePlanDetailsDTO.ProductId = productDTO.ProductId;
                            recipePlanDetailsDTO.PlannedQty = 0;
                            recipePlanDetailsDTO.FinalQty = 0;
                            recipePlanDetailsDTO.IncrementalQty = 0;
                            recipePlanDetailsDTO.UOMId = productDTO.InventoryUOMId;
                            recipePlanDetailsDTO.RecipeName = productDTO.Description;
                            recipePlanDetailsDTO.UOM = UOMContainer.uomDTOList.Find(x => x.UOMId == productDTO.InventoryUOMId).UOM;
                            recipePlanDetailsDTO.QtyModifiedDate = null;
                            recipePlanDetailsDTO.SiteId = productDTO.SiteId;
                            dgvAddRecipe.Refresh();
                        }
                        else if (productDTOList.Count > 1)
                        {
                            Panel pnlMultiple_dgv = new Panel();
                            this.Controls.Add(pnlMultiple_dgv);
                            DataGridView multiple_dgv = new DataGridView();
                            pnlMultiple_dgv.Controls.Add(multiple_dgv);
                            multiple_dgv.LostFocus += new EventHandler(multiple_dgv_LostFocus);
                            multiple_dgv.Click += new EventHandler(multiple_dgv_Click);
                            multiple_dgv.Focus();
                            productDTOList = productDTOList.OrderBy(x => x.Description).ToList();
                            multiple_dgv.DataSource = productDTOList;
                            multiple_dgv.Refresh();
                            multiple_dgv_Format(ref pnlMultiple_dgv, ref multiple_dgv);
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        MessageBox.Show(ex.Message);
                    }
                }
                if (dgvAddRecipe.Columns[e.ColumnIndex].Name == "finalQtyDataGridViewTextBoxColumn")
                {
                    dgvAddRecipe.Rows[e.RowIndex].Cells["qtyModifiedDateDataGridViewTextBoxColumn"].Value = utilities.getServerTime();
                    dgvAddRecipe.RefreshEdit();
                }
                log.LogMethodExit();
            }
            log.LogMethodExit();
        }

        void multiple_dgv_Format(ref Panel pnlMultiple_dgv, ref DataGridView multiple_dgv)
        {
            log.LogMethodEntry(pnlMultiple_dgv, multiple_dgv);
            try
            {
                pnlMultiple_dgv.Size = new Size(300, (dgvAddRecipe.Rows[0].Cells[0].Size.Height * 10) - 3);
                pnlMultiple_dgv.AutoScroll = true;
                pnlMultiple_dgv.Location = new Point(150 + dgvAddRecipe.RowHeadersWidth + dgvAddRecipe.CurrentRow.Cells["RecipeName"].ContentBounds.Location.X, dgvAddRecipe.Location.Y + dgvAddRecipe.ColumnHeadersHeight);
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
                       || multiple_dgv.Columns[i].HeaderText == "Code")
                    {
                        continue;
                    }
                    multiple_dgv.Columns[i].Visible = false;
                }
                multiple_dgv.Columns["Code"].DisplayIndex = 0;
                multiple_dgv.Columns["Description"].DisplayIndex = 1;
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

        private void RemoveDGVRow(int index)
        {
            log.LogMethodEntry();
            try
            {
                if (index > -1)
                {
                    dgvAddRecipe.Rows.RemoveAt(index);
                }
            }
            catch (Exception ex)
            {
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
                    dgvAddRecipe.Rows.Remove(dgvAddRecipe.Rows[dgvAddRecipe.CurrentRow.Index]);
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
                RecipeName.ReadOnly = false;
                DataGridView dg = (DataGridView)sender;
                ProductDTO productDTO = productDTOList[dg.CurrentRow.Index];
                RecipePlanDetailsDTO recipePlanDetailsDTO = recipePlanDetailsDTOBindingSource.Current as RecipePlanDetailsDTO;
                recipePlanDetailsDTO.ProductId = productDTO.ProductId;
                recipePlanDetailsDTO.PlannedQty = 0;
                recipePlanDetailsDTO.FinalQty = 0;
                recipePlanDetailsDTO.IncrementalQty = 0;
                recipePlanDetailsDTO.UOMId = productDTO.InventoryUOMId;
                recipePlanDetailsDTO.RecipeName = productDTO.Description;
                recipePlanDetailsDTO.UOM = UOMContainer.uomDTOList.Find(x => x.UOMId == productDTO.InventoryUOMId).UOM;
                recipePlanDetailsDTO.QtyModifiedDate = null;
                recipePlanDetailsDTO.SiteId = productDTO.SiteId;
                dg.Parent.Visible = false;
                dgvAddRecipe.Refresh();
                dgvAddRecipe.RefreshEdit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Save Recipe's
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            SavePlan();
            log.LogMethodExit();
        }

        /// <summary>
        /// Refresh Grid's
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveAndNew_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            lblMessage.Text = string.Empty;
            LoadGrid();
            log.LogMethodExit();
        }

        /// <summary>
        /// Method to Add or Update Recipe's
        /// </summary>
        private void SavePlan()
        {
            log.LogMethodEntry();
            lblMessage.Text = string.Empty;
            using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
            {
                try
                {
                    this.Cursor = Cursors.WaitCursor;
                    parafaitDBTrx.BeginTransaction();
                    bool valid = ValidatePlan();
                    if (valid && recipePlanDetailsDTOListOnDisplay != null &&
                        recipePlanDetailsDTOListOnDisplay.Count > 0)
                    {
                        RecipePlanHeaderDTO recipePlanHeaderDTO = new RecipePlanHeaderDTO();
                        if (recipePlanHeaderDTOList != null && recipePlanHeaderDTOList.Any())
                        {
                            recipePlanHeaderDTO.RecipePlanHeaderId = recipePlanHeaderDTOList[0].RecipePlanHeaderId;

                        }
                        recipePlanHeaderDTO.PlanDateTime = dtpStartDate.Value.Date;
                        if (cbxRecur.Checked)
                        {
                            recipePlanHeaderDTO.RecurFlag = true;

                            if (rdDaily.Checked)
                            {
                                recipePlanHeaderDTO.RecurFrequency = RecipePlanHeaderDTO.RecurFrequencyEnumToString(RecipePlanHeaderDTO.RecurFrequencyEnum.DAILY);
                            }
                            else if (rdWeekly.Checked)
                            {
                                recipePlanHeaderDTO.RecurFrequency = RecipePlanHeaderDTO.RecurFrequencyEnumToString(RecipePlanHeaderDTO.RecurFrequencyEnum.WEEKLY);
                            }
                            else if (rdMonthly.Checked)
                            {
                                recipePlanHeaderDTO.RecurFrequency = RecipePlanHeaderDTO.RecurFrequencyEnumToString(RecipePlanHeaderDTO.RecurFrequencyEnum.MONTHLY);
                            }
                            else
                            {
                                recipePlanHeaderDTO.RecurFrequency = RecipePlanHeaderDTO.RecurFrequencyEnumToString(RecipePlanHeaderDTO.RecurFrequencyEnum.DAILY);
                            }
                            recipePlanHeaderDTO.RecurEndDate = dtpEndDate.Value;
                        }
                        if (rdRecurMonthlyByDate.Checked)
                        {
                            recipePlanHeaderDTO.RecurType = RecipePlanHeaderDTO.RecurTypeEnumToString(RecipePlanHeaderDTO.RecurTypeEnum.DATE);
                        }
                        else if (rdRecurMonthlyByWeekDay.Checked)
                        {
                            recipePlanHeaderDTO.RecurType = RecipePlanHeaderDTO.RecurTypeEnumToString(RecipePlanHeaderDTO.RecurTypeEnum.WEEKDAY);
                        }
                        if (cbxSun.Checked)
                        {
                            recipePlanHeaderDTO.Sunday = true;
                        }
                        if (cbxMon.Checked)
                        {
                            recipePlanHeaderDTO.Monday = true;
                        }
                        if (cbxTue.Checked)
                        {
                            recipePlanHeaderDTO.Tuesday = true;
                        }
                        if (cbxWen.Checked)
                        {
                            recipePlanHeaderDTO.Wednesday = true;
                        }
                        if (cbxThu.Checked)
                        {
                            recipePlanHeaderDTO.Thursday = true;
                        }
                        if (cbxFri.Checked)
                        {
                            recipePlanHeaderDTO.Friday = true;
                        }
                        if (cbxSat.Checked)
                        {
                            recipePlanHeaderDTO.Saturday = true;
                        }
                        List<RecipePlanDetailsDTO> recipePlanDetailsDTOList = new List<RecipePlanDetailsDTO>();
                        recipePlanDetailsDTOList = (List<RecipePlanDetailsDTO>)recipePlanDetailsDTOBindingSource.DataSource;
                        foreach (RecipePlanDetailsDTO recipeDetails in recipePlanDetailsDTOList)
                        {
                            recipeDetails.PlannedQty = recipeDetails.FinalQty;
                            if (recipeDetails.RecipePlanDetailId == -1)
                            {
                                recipeDetails.QtyModifiedDate = null;
                            }
                        }
                        recipePlanHeaderDTO.RecipePlanDetailsDTOList = recipePlanDetailsDTOList;
                        RecipePlanHeaderBL recipePlanHeaderBL = new RecipePlanHeaderBL(executionContext, recipePlanHeaderDTO);
                        recipePlanHeaderBL.Save(parafaitDBTrx.SQLTrx);
                        lblMessage.Text = MessageContainerList.GetMessage(executionContext, 122); //"Save Successful";
                        lblMessage.ForeColor = Color.Blue;
                        parafaitDBTrx.EndTransaction();
                        LoadGrid();
                    }
                }
                catch (Exception ex)
                {
                    parafaitDBTrx.RollBack();
                    lblMessage.Text = ex.Message;
                    lblMessage.ForeColor = Color.Red;
                    log.Error(ex);
                }
                finally
                {
                    this.Cursor = Cursors.Default;
                }
                log.LogMethodExit();
            }
        }


        /// <summary>
        /// Has Changes
        /// </summary>
        /// <returns></returns>
        private bool HasChanges()
        {
            log.LogMethodEntry();
            bool result = false;
            if (recipePlanHeaderDTOList != null && recipePlanHeaderDTOList.Count > 0)
            {
                if (recipePlanHeaderDTOList[0].IsChanged
                 || recipePlanHeaderDTOList[0].IsChangedRecursive)
                    result = true;
            }
            else if (recipePlanDetailsDTOListOnDisplay != null && recipePlanDetailsDTOListOnDisplay.Count > 0)
            {
                result = true;
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Method to Validate the recipe
        /// </summary>
        /// <returns></returns>
        private bool ValidatePlan()
        {
            log.LogMethodEntry();
            bool hasChanges = HasChanges();
            if (!hasChanges)
            {
                lblMessage.Text = MessageContainerList.GetMessage(executionContext, "Nothing to Save");
                log.LogMethodExit(false);
                return false;
            }
            List<RecipePlanDetailsDTO> recipePlanDetailsDTOList = new List<RecipePlanDetailsDTO>();
            recipePlanDetailsDTOList = (List<RecipePlanDetailsDTO>)recipePlanDetailsDTOBindingSource.DataSource;
            List<RecipePlanDetailsDTO> tempDTOList = new List<RecipePlanDetailsDTO>();
            var duplicates = recipePlanDetailsDTOList.GroupBy(s => s.ProductId)
                             .Where(g => g.Count() > 1)
                             .Select(g => g.Key);
            if (duplicates.Any())
            {
                lblMessage.Text = MessageContainerList.GetMessage(executionContext, 1872); //"Cannot Add Duplicate product " 
                lblMessage.ForeColor = Color.Red;
                tempDTOList = recipePlanDetailsDTOList;
                tempDTOList.RemoveAt(tempDTOList.Count - 1);
                recipePlanDetailsDTOList = tempDTOList;
                recipePlanDetailsDTOBindingSource.DataSource = null;
                recipePlanDetailsDTOBindingSource.DataSource = recipePlanDetailsDTOList;
                dgvAddRecipe.Refresh();
                return false;
            }
            if (dtpEndDate.Value.Date < dtpStartDate.Value.Date)
            {
                lblMessage.Text = MessageContainerList.GetMessage(executionContext, 2846); // "Recur end date should be greater than the Plan date"
                lblMessage.ForeColor = Color.Red;
                log.LogMethodExit(false);
                return false;
            }

            log.LogMethodExit(true);
            return true;
        }

        private void dgvAddRecipe_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex > -1 && e.RowIndex > -1)
                {
                    if (Convert.ToInt32(dgvAddRecipe["productIdDataGridViewTextBoxColumn", dgvAddRecipe.CurrentRow.Index].Value) != -1
                        && dgvAddRecipe["productIdDataGridViewTextBoxColumn", dgvAddRecipe.CurrentRow.Index].Value != null
                        && dgvAddRecipe.Columns[e.ColumnIndex].Name == "RecipeName")
                    {
                        using (frmBOM frmBOM = new frmBOM(Convert.ToInt32(dgvAddRecipe["productIdDataGridViewTextBoxColumn", dgvAddRecipe.CurrentRow.Index].Value), utilities, false))
                        {
                            CommonUIDisplay.setupVisuals(frmBOM);
                            frmBOM.ShowDialog();
                        }
                    }
                    else if (dgvAddRecipe.Columns[e.ColumnIndex].Name == "RecipeName")
                    {
                        dgvAddRecipe.Rows[e.RowIndex].Cells["RecipeName"] = new DataGridViewTextBoxCell();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        private void dgvAddRecipe_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            log.LogMethodEntry();
            dgvAddRecipe.Rows[e.RowIndex].Cells["finalQtyDataGridViewTextBoxColumn"].Style.BackColor = Color.PowderBlue;
            log.LogMethodExit();
        }

        /// <summary>
        /// Event to fire when Recur Type Monthly is checked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdMonthly_CheckedChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            lblMessage.Text = string.Empty;
            if (rdMonthly.Checked)
            {
                pnlDays.Enabled = true;
                rdRecurMonthlyByDate.Enabled = true;
                rdRecurMonthlyByDate.Checked = true;
                rdRecurMonthlyByWeekDay.Enabled = true;
                rdDaily.Checked = false;
                rdWeekly.Checked = false;
                pnlDays.Enabled = false;
                if (recipePlanHeaderDTOList != null && recipePlanHeaderDTOList.Count > 0)
                {
                    recipePlanHeaderDTOList[0].RecurFrequency = RecipePlanHeaderDTO.RecurFrequencyEnumToString(RecipePlanHeaderDTO.RecurFrequencyEnum.MONTHLY);
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Event to fire when Recur Type Daily is checked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdDaily_CheckedChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            lblMessage.Text = string.Empty;
            if (rdDaily.Checked)
            {
                pnlDays.Enabled = false;
                rdRecurMonthlyByDate.Enabled = false;
                rdRecurMonthlyByWeekDay.Enabled = false;
                rdMonthly.Checked = false;
                rdWeekly.Checked = false;
                if (recipePlanHeaderDTOList != null && recipePlanHeaderDTOList.Count > 0)
                {
                    recipePlanHeaderDTOList[0].RecurFrequency = RecipePlanHeaderDTO.RecurFrequencyEnumToString(RecipePlanHeaderDTO.RecurFrequencyEnum.DAILY);
                }
            }
            if (rdWeekly.Checked)
            {
                pnlDays.Enabled = true;
                rdRecurMonthlyByDate.Enabled = false;
                rdRecurMonthlyByWeekDay.Enabled = false;
                rdMonthly.Checked = false;
                rdDaily.Checked = false;
                if (recipePlanHeaderDTOList != null && recipePlanHeaderDTOList.Count > 0)
                {
                    recipePlanHeaderDTOList[0].RecurFrequency = RecipePlanHeaderDTO.RecurFrequencyEnumToString(RecipePlanHeaderDTO.RecurFrequencyEnum.WEEKLY);
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Event to fire when Recur Flag Daily is checked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRecur_CheckedChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            lblMessage.Text = string.Empty;
            if (cbxRecur.Checked)
            {
                rdWeekly.Enabled = true;
                rdDaily.Enabled = true;
                rdMonthly.Enabled = true;
                dtpEndDate.Enabled = true;
                if (recipePlanHeaderDTOList != null && recipePlanHeaderDTOList.Count > 0)
                {
                    recipePlanHeaderDTOList[0].RecurFlag = true;
                    if (recipePlanHeaderDTOList[0].RecurFrequency == null)
                    {
                        rdDaily.Checked = true;
                    }
                }
                else
                {
                    if (!rdWeekly.Checked && !rdMonthly.Checked)
                        rdDaily.Checked = true;
                }
                pnlMonth.Enabled = true;
            }
            else
            {
                if (recipePlanHeaderDTOList != null && recipePlanHeaderDTOList.Count > 0)
                {
                    recipePlanHeaderDTOList[0].RecurFlag = false;
                }
                rdWeekly.Enabled = false;
                rdDaily.Enabled = false;
                dtpEndDate.Enabled = false;
                rdMonthly.Enabled = false;
                pnlMonth.Enabled = false;
            }
            log.LogMethodExit();
        }

        private void dtpEndDate_ValueChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            lblMessage.Text = string.Empty;
            if (recipePlanHeaderDTOList != null && recipePlanHeaderDTOList.Count > 0)
            {
                recipePlanHeaderDTOList[0].RecurEndDate = dtpEndDate.Value;
            }
            else
            {
                dtpEndDate.Value = dtpStartDate.Value.AddDays(1);
            }
            log.LogMethodExit();
        }

        private void rdRecurMonthlyByDate_CheckedChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (recipePlanHeaderDTOList != null && recipePlanHeaderDTOList.Count > 0)
            {
                if (rdRecurMonthlyByDate.Checked)
                {
                    recipePlanHeaderDTOList[0].RecurType = RecipePlanHeaderDTO.RecurTypeEnumToString(RecipePlanHeaderDTO.RecurTypeEnum.DATE);
                }
                else if (rdRecurMonthlyByWeekDay.Checked)
                {
                    recipePlanHeaderDTOList[0].RecurType = RecipePlanHeaderDTO.RecurTypeEnumToString(RecipePlanHeaderDTO.RecurTypeEnum.WEEKDAY);
                }
            }
            log.LogMethodExit();
        }

        private void weekday_CheckedChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (recipePlanHeaderDTOList != null && recipePlanHeaderDTOList.Count > 0)
            {
                if (cbxSun.Checked)
                {
                    recipePlanHeaderDTOList[0].Sunday = true;
                }
                if (cbxMon.Checked)
                {
                    recipePlanHeaderDTOList[0].Monday = true;
                }
                if (cbxTue.Checked)
                {
                    recipePlanHeaderDTOList[0].Tuesday = true;
                }
                if (cbxWen.Checked)
                {
                    recipePlanHeaderDTOList[0].Wednesday = true;
                }
                if (cbxThu.Checked)
                {
                    recipePlanHeaderDTOList[0].Thursday = true;
                }
                if (cbxFri.Checked)
                {
                    recipePlanHeaderDTOList[0].Friday = true;
                }
                if (cbxSat.Checked)
                {
                    recipePlanHeaderDTOList[0].Saturday = true;
                }
            }
            log.LogMethodExit();
        }

        private void dtpStartDate_ValueChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                dtpEndDate.Value = dtpStartDate.Value.AddDays(1);
                LoadGrid();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void dgvAddRecipe_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                lblMessage.Text = "Error in Add Recipe grid data at row " + (e.RowIndex + 1).ToString() + ", Column "
                    + dgvAddRecipe.Columns[e.ColumnIndex].DataPropertyName +
                    ": " + e.Exception.Message;
                e.Cancel = true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
    }
}
