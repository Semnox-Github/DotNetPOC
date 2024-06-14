/********************************************************************************************
 * Project Name - Inventory
 * Description  - UI for REcipe Calendar View
 * 
 **************
 **Version Log
 **************
 *Version       Date          Modified By         Remarks          
 *********************************************************************************************
 *2.100.0       27-Sep-2020   Deeksha             Created for Recipe Management enhancement.
 *********************************************************************************************/
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Globalization;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;
using System.Collections.Generic;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Languages;
using System.Reflection;

namespace Semnox.Parafait.Inventory.Recipe
{
    public partial class frmRecipeCalendar : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executioncontex = ExecutionContext.GetExecutionContext();
        private Utilities utilities;
        private readonly Dictionary<int, Point> calendarDays;
        private DateTime calendarDate;
        private List<RecipePlanHeaderDTO> recipePlanHeaderDTOListOnDisplay = new List<RecipePlanHeaderDTO>();
        private List<RecipePlanDetailsDTO> recipePlanDetailsDTOListOnDisplay = new List<RecipePlanDetailsDTO>();
        private int planHeaderId;
        private CustomCheckBox cbxSelect;
        private bool renderCalendar = true;

        public frmRecipeCalendar(Utilities utilities)
        {
            log.LogMethodEntry(utilities);
            InitializeComponent();
            this.utilities = utilities;
            calendarDate = DateTime.Now;
            calendarDays = new Dictionary<int, Point>();
            SetMonthName(calendarDate.Month);
            ProductContainer productContainer = new ProductContainer(executioncontex);
            CreateHeaderCheckBox();
            log.LogMethodExit();
        }

        protected override CreateParams CreateParams
        {
            //this method is used to avoid the table layout flickering.
            get
            {
                CreateParams CP = base.CreateParams;
                CP.ExStyle = CP.ExStyle | 0x02000000;
                return CP;
            }
        }

        /// <summary>
        /// Method to display month Name in the Month View
        /// </summary>
        /// <param name="currentMonth"></param>
        private void SetMonthName(int currentMonth)
        {
            log.LogMethodEntry(currentMonth);
            string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(currentMonth);
            grpMonth.Text = monthName + " - " + calendarDate.Year;
            log.LogMethodExit();
        }

        private void frmRecipeCalendar_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.BackColor = CommonUIDisplay.SkinColor;
            setupDay();
            setupWeek();
            tabCalendar.SelectedIndex = 1;
            tabCalendar.SelectedIndex = 0;
            tabCalendar.SelectedIndex = 1;
            dgvDay.Width = dgvWeek.Width;
            RefreshDayWeek();
            btnPrev.Width = btnNext.Width = 30;
            btnPrev.Font = btnNext.Font = new Font(btnPrev.Font, FontStyle.Bold);

            utilities.setupDataGridProperties(ref dgvAll);
            dgvPlanDetails.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
            dgvPlanDetails.EnableHeadersVisualStyles = false;

            dgvPlanDetails.BackgroundColor = this.BackColor;
            dgvPlanDetails.EditMode = DataGridViewEditMode.EditOnEnter;
            planDateTimeDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewDateCellStyle();
            plannedQtyDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewAmountCellStyle();
            plannedQtyDataGridViewTextBoxColumn.DefaultCellStyle.Format = utilities.ParafaitEnv.INVENTORY_QUANTITY_FORMAT;
            finalQtyDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewAmountCellStyle();
            finalQtyDataGridViewTextBoxColumn.DefaultCellStyle.Format = utilities.ParafaitEnv.INVENTORY_QUANTITY_FORMAT;
            incrementalQtyDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewAmountCellStyle();
            incrementalQtyDataGridViewTextBoxColumn.DefaultCellStyle.Format = utilities.ParafaitEnv.INVENTORY_QUANTITY_FORMAT;
            creationDateDataGridViewTextBoxColumn.DefaultCellStyle.Format = utilities.getDateTimeFormat();
            lastUpdateDateDataGridViewTextBoxColumn.DefaultCellStyle.Format = utilities.getDateTimeFormat();
            planDateTimeDataGridViewTextBoxColumn.DefaultCellStyle.Format = utilities.getDateFormat();
            recurEndDateDataGridViewTextBoxColumn.DefaultCellStyle.Format = utilities.getDateFormat();
            
            LoadAllRecipe();
            btnAddRecipe.Text = MessageContainerList.GetMessage(executioncontex, "Add Recipe");
            btnCopyRecipe.Text = MessageContainerList.GetMessage(executioncontex, "Copy Recipe");
            btnCancel.Text = MessageContainerList.GetMessage(executioncontex, "Cancel");
            btnExport.Text = MessageContainerList.GetMessage(executioncontex, "Export");
            btnCreateKPN.Text = MessageContainerList.GetMessage(executioncontex, "Create KPN");
            btnSave.Text = MessageContainerList.GetMessage(executioncontex, "Save");
            btnDelete.Text = MessageContainerList.GetMessage(executioncontex, "Delete");
            btnSearch.Text = MessageContainerList.GetMessage(executioncontex, "Search");
            lblRecipe.Text = MessageContainerList.GetMessage(executioncontex, "Recipe");
            dtpPlanDate.Value = utilities.getServerTime();
            log.LogMethodExit();
        }

        /// <summary>
        /// Method to set up Day 
        /// </summary>
        private void setupDay()
        {
            log.LogMethodEntry();
            setupDayAndWeekView(ref dgvDay);
            setupDGVs(ref dgvDay);
            CommonUIDisplay.Utilities.setLanguage(dgvDay);
            log.LogMethodExit();
        }

        /// <summary>
        /// Method to set up Week
        /// </summary>
        private void setupWeek()
        {
            log.LogMethodEntry();
            setupDayAndWeekView(ref dgvWeek);
            setupDGVs(ref dgvWeek);
            updateColumnHeaders();
            CommonUIDisplay.Utilities.setLanguage(dgvWeek);
            log.LogMethodExit();
        }

        /// <summary>
        /// Method to update Column headers
        /// </summary>
        private void updateColumnHeaders()
        {
            log.LogMethodEntry();
            int daysToAdd = 0;
            DateTime FirstDayOfWeek = dtpPlanDate.Value.Date;
            switch (dtpPlanDate.Value.DayOfWeek)
            {
                case DayOfWeek.Monday: daysToAdd = 0; break;
                case DayOfWeek.Tuesday: daysToAdd = -1; break;
                case DayOfWeek.Wednesday: daysToAdd = -2; break;
                case DayOfWeek.Thursday: daysToAdd = -3; break;
                case DayOfWeek.Friday: daysToAdd = -4; break;
                case DayOfWeek.Saturday: daysToAdd = -5; break;
                case DayOfWeek.Sunday: daysToAdd = -6; break;
                default: daysToAdd = 0; break;
            }
            FirstDayOfWeek = FirstDayOfWeek.AddDays(daysToAdd);
            foreach (DataGridViewColumn dc in dgvWeek.Columns)
            {
                dc.HeaderText = FirstDayOfWeek.AddDays(dc.Index).ToString("dddd, MMM dd");
            }
            dgvDay.Columns[0].HeaderText = dtpPlanDate.Value.ToString("dddd, MMM dd");
            log.LogMethodExit();
        }

        /// <summary>
        /// Method which creates manufacturing details for the selected Recipe Plan Details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCreateKPN_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
            {
                try
                {
                    dgvPlanDetails.RefreshEdit();
                    lblMessage.Text = string.Empty;
                    if (dgvPlanDetails.Rows.Count > 0)
                    {
                        this.Cursor = Cursors.WaitCursor;
                        string recipeDate = grpPlanDetails.Text.Substring(17);
                        DateTime manufactureDate = DateTime.ParseExact(recipeDate, utilities.getDateFormat(), CultureInfo.InvariantCulture);

                        parafaitDBTrx.BeginTransaction();
                        bool valid = ValidateSetQuantityCheckbox();
                        if (!valid)
                        {
                            lblMessage.Text = MessageContainerList.GetMessage(executioncontex, 218); // Choose Product
                            lblMessage.ForeColor = Color.Red;
                            log.LogMethodExit();
                            return;
                        }
                        RecipePlanHeaderBL recipePlanHeaderBL = new RecipePlanHeaderBL(executioncontex, planHeaderId, true, true);
                        RecipePlanHeaderDTO recipePlanHeaderDTO = recipePlanHeaderBL.RecipePlanHeaderDTO;
                        recipePlanHeaderDTO.RecipePlanDetailsDTOList = new List<RecipePlanDetailsDTO>(recipePlanDetailsDTOListOnDisplay);

                        RecipeManufacturingHeaderDTO manufacturingHeaderDTO = new RecipeManufacturingHeaderDTO();

                        RecipeManufacturingHeaderListBL recipeManufacturingHeaderListBL = new RecipeManufacturingHeaderListBL(executioncontex);
                        List<KeyValuePair<RecipeManufacturingHeaderDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<RecipeManufacturingHeaderDTO.SearchByParameters, string>>();
                        searchParameters.Add(new KeyValuePair<RecipeManufacturingHeaderDTO.SearchByParameters, string>(RecipeManufacturingHeaderDTO.SearchByParameters.MFG_FROM_DATETIME, manufactureDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)));
                        searchParameters.Add(new KeyValuePair<RecipeManufacturingHeaderDTO.SearchByParameters, string>(RecipeManufacturingHeaderDTO.SearchByParameters.MFG_TO_DATETIME, manufactureDate.AddDays(1).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)));
                        searchParameters.Add(new KeyValuePair<RecipeManufacturingHeaderDTO.SearchByParameters, string>(RecipeManufacturingHeaderDTO.SearchByParameters.SITE_ID, executioncontex.GetSiteId().ToString()));
                        List<RecipeManufacturingHeaderDTO> recipeManufacturingHeaderDTOList = recipeManufacturingHeaderListBL.GetAllRecipeManufacturingHeaderDTOList(searchParameters, true, true);
                        if (recipeManufacturingHeaderDTOList.Any())
                        {
                            manufacturingHeaderDTO = recipeManufacturingHeaderDTOList[0];
                            //if (manufacturingHeaderDTO.IsComplete)
                            //{
                            //    lblMessage.Text = MessageContainerList.GetMessage(executioncontex, 2844);//Cannot create Manufacturing details. Stock is finalized for the selected date
                            //    log.LogMethodExit();
                            //    return;
                            //}
                        }
                        else
                        {
                            manufacturingHeaderDTO.RecipeManufacturingDetailsDTOList = new List<RecipeManufacturingDetailsDTO>();
                            manufacturingHeaderDTO.MFGDateTime = manufactureDate;
                        }
                        manufacturingHeaderDTO.RecipePlanHeaderId = recipePlanHeaderDTO.RecipePlanHeaderId;
                        foreach (DataGridViewRow dataGridRow in dgvPlanDetails.Rows)
                        {
                            if (dataGridRow.Cells["cbxSel"].Value != null && (bool)dataGridRow.Cells["cbxSel"].Value)
                            {
                                RecipeManufacturingDetailsDTO dto = manufacturingHeaderDTO.RecipeManufacturingDetailsDTOList.Find(x => x.RecipePlanDetailId == recipePlanHeaderDTO.RecipePlanDetailsDTOList[dataGridRow.Index].RecipePlanDetailId
                                                & x.ProductId == recipePlanHeaderDTO.RecipePlanDetailsDTOList[dataGridRow.Index].ProductId);
                                if (dto == null && ProductContainer.productDTOList != null && ProductContainer.productDTOList.Count > 0)
                                {
                                    ProductDTO productDTO = ProductContainer.productDTOList.Find(x => x.ProductId == recipePlanHeaderDTO.RecipePlanDetailsDTOList[dataGridRow.Index].ProductId);
                                    dto = new RecipeManufacturingDetailsDTO(-1, -1, -1, productDTO.ProductId, true, -1, -1, 1,
                                                                            productDTO.UomId, 1, productDTO.UomId, Convert.ToDecimal(productDTO.Cost), Convert.ToDecimal(productDTO.Cost), 
                                                                            Convert.ToDecimal(productDTO.Cost), recipePlanHeaderDTO.RecipePlanDetailsDTOList[dataGridRow.Index].RecipePlanDetailId, true,false);

                                    manufacturingHeaderDTO.RecipeManufacturingDetailsDTOList.Add(dto);
                                    if (productDTO.ProductBOMDTOList != null && productDTO.ProductBOMDTOList.Any())
                                    {
                                        int count = productDTO.ProductBOMDTOList.Count;
                                        while (count > 0)
                                        {
                                            for (int j = 0; j < productDTO.ProductBOMDTOList.Count; j++)
                                            {
                                                BOMDTO bOMDTO = productDTO.ProductBOMDTOList[j];
                                                ProductDTO childproductDTO = ProductContainer.productDTOList.Find(x => x.ProductId == bOMDTO.ChildProductId);
                                                if (bOMDTO.UOMId == -1)
                                                {
                                                    bOMDTO.UOMId = childproductDTO.InventoryUOMId;
                                                }
                                                dto = new RecipeManufacturingDetailsDTO(-1, -1, -1, childproductDTO.ProductId, false, -1, -1, bOMDTO.Quantity,
                                                                                                bOMDTO.UOMId, bOMDTO.Quantity, bOMDTO.UOMId, Convert.ToDecimal(childproductDTO.Cost), (bOMDTO.Quantity * Convert.ToDecimal(childproductDTO.Cost)),
                                                                                                Convert.ToDecimal(childproductDTO.Cost), recipePlanHeaderDTO.RecipePlanDetailsDTOList[dataGridRow.Index].RecipePlanDetailId, true,false);

                                                manufacturingHeaderDTO.RecipeManufacturingDetailsDTOList.Add(dto);
                                                count--;
                                            }
                                        }
                                    }
                                    RecipeManufacturingDetailsListBL recipeManufacturingDetailsListBL = new RecipeManufacturingDetailsListBL(executioncontex, manufacturingHeaderDTO.RecipeManufacturingDetailsDTOList);
                                    recipeManufacturingDetailsListBL.SetLineDetails();
                                }
                            }
                        }
                        List<RecipeManufacturingDetailsDTO> parentProductList = manufacturingHeaderDTO.RecipeManufacturingDetailsDTOList.Where(x => x.IsParentItem == true).ToList();
                        foreach (RecipeManufacturingDetailsDTO parentDTO in parentProductList)
                        {
                            if (parentDTO.RecipePlanDetailId > -1 && ProductContainer.productDTOList != null && ProductContainer.productDTOList.Count > 0)
                            {
                                decimal? finalQuantity = 0;
                                ProductDTO productDTO = ProductContainer.productDTOList.Find(x => x.ProductId == parentDTO.ProductId);
                                RecipePlanDetailsDTO recipePlanDetailsDTO = recipePlanHeaderDTO.RecipePlanDetailsDTOList.Find(x => x.RecipePlanDetailId == parentDTO.RecipePlanDetailId);

                                if (recipePlanDetailsDTO != null)
                                {
                                    finalQuantity = recipePlanDetailsDTO.FinalQty;
                                    int parentLineId = parentDTO.MfgLineId;
                                    List<RecipeManufacturingDetailsDTO> childProductList = manufacturingHeaderDTO.RecipeManufacturingDetailsDTOList.Where(x => x.ParentMFGLineId == parentLineId).ToList();
                                    foreach (RecipeManufacturingDetailsDTO childDTO in childProductList)
                                    {
                                        decimal quantity = productDTO.ProductBOMDTOList.Find(x => x.ChildProductId == childDTO.ProductId).Quantity;
                                        if (productDTO.YieldPercentage > 0)
                                        {
                                            childDTO.Quantity = (finalQuantity * quantity * 100) / productDTO.YieldPercentage;
                                        }
                                        else
                                        {
                                            childDTO.Quantity = (finalQuantity * quantity);
                                        }

                                        childDTO.ActualMfgQuantity = childDTO.Quantity;
                                        childDTO.ActualCost = childDTO.ActualMfgQuantity * childDTO.ItemCost;
                                    }
                                    parentDTO.Quantity = finalQuantity;
                                    parentDTO.ActualMfgQuantity = finalQuantity;
                                    parentDTO.ActualCost = parentDTO.ActualMfgQuantity * parentDTO.ItemCost;
                                }
                            }
                        }
                        RecipeManufacturingHeaderBL recipeManufacturingHeaderBL = new RecipeManufacturingHeaderBL(executioncontex, manufacturingHeaderDTO);
                        recipeManufacturingHeaderBL.Save(parafaitDBTrx.SQLTrx);
                        lblMessage.Text = MessageContainerList.GetMessage(executioncontex, 2848);// "Manufacturing details created successfully";
                        parafaitDBTrx.EndTransaction();
                    }
                }
                catch (Exception ex)
                {
                    lblMessage.Text = ex.Message;
                    parafaitDBTrx.RollBack();
                    log.Error(ex);
                }
                finally
                {
                    this.Cursor = Cursors.Default;
                }
            }
            log.LogMethodExit();
        }

        private void setupDayAndWeekView(ref DataGridView dgv)
        {
            log.LogMethodEntry(dgv);
            dgv.RowTemplate.Height = 80;

            for (int i = 0; i <= 4; i++)
            {
                if (dgv.Name.Contains("Week"))
                    dgv.Rows.Add(new object[] { "", "", "", "", "", "", "" });
                else
                    dgv.Rows.Add(new object[] { "" });
            }
            log.LogMethodExit();
        }

        private void setupDGVs(ref DataGridView dgv)
        {
            log.LogMethodEntry(dgv);
            dgv.EnableHeadersVisualStyles = false;
            dgv.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.ReadOnly = true;
            dgv.AllowUserToAddRows = false;
            dgv.BackgroundColor = this.BackColor;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("arial", 8);
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            foreach (DataGridViewColumn dc in dgv.Columns)
                dc.SortMode = DataGridViewColumnSortMode.NotSortable;

            dgv.AllowUserToResizeRows = false;
            dgv.RowHeadersDefaultCellStyle.BackColor = Color.LightGray;
            dgv.RowHeadersDefaultCellStyle.ForeColor = Color.Black;
            dgv.RowHeadersDefaultCellStyle.SelectionBackColor = Color.LightGray;
            dgv.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            dgv.RowHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;

            dgv.GridColor = dgv.DefaultCellStyle.BackColor = Color.LightYellow;
            dgv.SelectionMode = DataGridViewSelectionMode.CellSelect;
            log.LogMethodExit();
        }

        private DateTime getFirstDayOfWeek()
        {
            log.LogMethodEntry();
            int daysToAdd = 0;
            switch (dtpPlanDate.Value.DayOfWeek)
            {
                case DayOfWeek.Monday: daysToAdd = 0; break;
                case DayOfWeek.Tuesday: daysToAdd = -1; break;
                case DayOfWeek.Wednesday: daysToAdd = -2; break;
                case DayOfWeek.Thursday: daysToAdd = -3; break;
                case DayOfWeek.Friday: daysToAdd = -4; break;
                case DayOfWeek.Saturday: daysToAdd = -5; break;
                case DayOfWeek.Sunday: daysToAdd = -6; break;
                default: daysToAdd = 0; break;
            }
            DateTime ret = dtpPlanDate.Value.Date.AddDays(daysToAdd);
            log.LogMethodExit(ret);
            return ret;
        }

        private DateTime getGridCellDateTime(int rowIndex, int colIndex, bool isweek, bool isMonth = false)
        {
            log.LogMethodEntry(rowIndex, colIndex, isweek);
            DateTime CellTime;
            if (isweek)
            {
                CellTime = getFirstDayOfWeek();
            }
            else if (isMonth)
            {
                CellTime = new DateTime(calendarDate.Year, calendarDate.Month, 1);
            }
            else
                CellTime = dtpPlanDate.Value.Date;

            CellTime = CellTime.AddDays(colIndex);
            int hour = Convert.ToInt32(rowIndex * 30 / 60);
            int mins = rowIndex % 2 * 30;
            CellTime = CellTime.AddHours(hour);
            CellTime = CellTime.AddMinutes(mins);
            log.LogMethodExit(CellTime);
            return CellTime;
        }

        private void RefreshDayWeek()
        {
            log.LogMethodEntry();
            DateTime planFromDate, planToDate;
            DateTime origplanFromDate, origplanToDate;

            long plan_id;
            char? recur_frequency, recurType;
            bool? recur_flag;
            DateTime recur_end_date = DateTime.MinValue;
            bool? sun, mon, tue, wed, thu, fri, sat;
            int minRow = -1;
            int maxRow = 100;
            int numrows;
            DateTime cellTime = DateTime.MinValue;

            // scroll to top of tab page
            ScrollableControl scrCtl = tabPageWeek as ScrollableControl;
            scrCtl.ScrollControlIntoView(dgvWeek);
            scrCtl = tabPageDay as ScrollableControl;
            scrCtl.ScrollControlIntoView(dgvDay);

            for (int i = 0; i < tabPageDay.Controls.Count; i++)
            {
                Control c = tabPageDay.Controls[i];
                if (c.Name.Contains("planDisplay"))
                {
                    tabPageDay.Controls.Remove(c);
                    c.Dispose();
                    i = -1;
                }
            }

            RecipePlanHeaderListBL recipePlanHeaderListBL = new RecipePlanHeaderListBL(executioncontex);
            List<RecipePlanHeaderDTO> recipePlanHeaderDTOList = recipePlanHeaderListBL.GetRecipePlanHeaderDTOList(dtpPlanDate.Value.Date, dtpPlanDate.Value.Date.AddDays(1), utilities.ExecutionContext.GetSiteId());

            for (int rows = 0; rows < recipePlanHeaderDTOList.Count; rows++)
            {
                minRow = -1;
                maxRow = 100;

                plan_id = recipePlanHeaderDTOList[rows].RecipePlanHeaderId;
                planFromDate = recipePlanHeaderDTOList[rows].PlanDateTime;
                planToDate = recipePlanHeaderDTOList[rows].PlanDateTime;
                sun = recipePlanHeaderDTOList[rows].Sunday;
                mon = recipePlanHeaderDTOList[rows].Monday;
                tue = recipePlanHeaderDTOList[rows].Tuesday;
                wed = recipePlanHeaderDTOList[rows].Wednesday;
                thu = recipePlanHeaderDTOList[rows].Thursday;
                fri = recipePlanHeaderDTOList[rows].Friday;
                sat = recipePlanHeaderDTOList[rows].Saturday;
                if (recipePlanHeaderDTOList[rows].RecurFlag != null)
                {
                    recur_flag = recipePlanHeaderDTOList[rows].RecurFlag;
                }
                else
                {
                    recur_flag = false;
                }
                recur_frequency = recipePlanHeaderDTOList[rows].RecurFrequency;
                recurType = recipePlanHeaderDTOList[rows].RecurType;
                if (recipePlanHeaderDTOList[rows].RecurEndDate != null)
                    recur_end_date = Convert.ToDateTime(recipePlanHeaderDTOList[rows].RecurEndDate);


                origplanFromDate = planFromDate;
                origplanToDate = planToDate;

                string frequency = string.Empty;
                if (recur_flag == true)
                {
                    if (recur_frequency == 'D')
                        frequency = "day";
                    else if (recur_frequency == 'W')
                        frequency = "week";
                    else
                        frequency = "month";

                    DateTime cellTimeRecur = getGridCellDateTime(0, 0, false); // get date on day of week

                    if (recur_end_date.Date >= cellTimeRecur.Date)
                    {
                        if (recur_frequency == 'W')
                        {
                            bool recur = false;
                            string dayOfWeek = cellTimeRecur.DayOfWeek.ToString();
                            if (cellTimeRecur.DayOfWeek == origplanFromDate.DayOfWeek)
                            {
                                recur = true;
                            }
                            if (sun == true && dayOfWeek.Equals("Sunday"))
                            {
                                recur = true;
                            }
                            else if (mon == true && dayOfWeek.Equals("Monday"))
                            {
                                recur = true;
                            }
                            else if (tue == true && dayOfWeek.Equals("Tuesday"))
                            {
                                recur = true;
                            }
                            else if (wed == true && dayOfWeek.Equals("Wednesday"))
                            {
                                recur = true;
                            }
                            else if (thu == true && dayOfWeek.Equals("Thursday"))
                            {
                                recur = true;
                            }
                            else if (fri == true && dayOfWeek.Equals("Friday"))
                            {
                                recur = true;
                            }
                            else if (sat == true && dayOfWeek.Equals("Saturday"))
                            {
                                recur = true;
                            }
                            if (recur == false)
                            {
                                continue;
                            }
                        }
                        else if (recur_frequency == 'M')
                        {
                            if (recurType == 'D')
                            {
                                if (cellTimeRecur.Day != planFromDate.Day)
                                    continue;
                            }
                            else
                            {
                                DateTime monthStart = new DateTime(planFromDate.Year, planFromDate.Month, 1);
                                int promoWeekNo = 0;
                                while (monthStart <= planFromDate)
                                {
                                    if (monthStart.DayOfWeek == planFromDate.DayOfWeek)
                                        promoWeekNo++;
                                    monthStart = monthStart.AddDays(1);
                                }

                                monthStart = new DateTime(cellTimeRecur.Year, cellTimeRecur.Month, 1);
                                int gridWeekNo = 0;
                                while (monthStart <= cellTimeRecur)
                                {
                                    if (monthStart.DayOfWeek == cellTimeRecur.DayOfWeek)
                                        gridWeekNo++;
                                    monthStart = monthStart.AddDays(1);
                                }

                                if (cellTimeRecur.DayOfWeek != planFromDate.DayOfWeek || gridWeekNo != promoWeekNo)
                                    continue;
                            }
                        }

                        TimeSpan ts = planToDate.Date - planFromDate.Date; // used to get number of days the plan spans over. change plan from and to days as per week day date
                        planFromDate = cellTimeRecur.Date.AddHours(planFromDate.Hour).AddMinutes(planFromDate.Minute);
                        planToDate = cellTimeRecur.Date.AddDays(ts.Days).AddHours(planToDate.Hour).AddMinutes(planToDate.Minute);
                    }
                }

                for (int i = 0; i < dgvDay.RowCount; i++)
                {
                    cellTime = getGridCellDateTime(i, 0, false);
                    if (cellTime.Date >= planFromDate.Date && cellTime.Date < planToDate.AddDays(1).Date)
                    {
                        if (minRow == -1)
                            minRow = i;
                        maxRow = i;
                        break;
                    }
                }
                log.LogVariableState("minRow", minRow);
                log.LogVariableState("maxRow", maxRow);
                if (minRow != -1)
                {
                    Label planDisplay = new Label();
                    planDisplay.Name = "planDisplay" + "|" + cellTime.Date.ToString();
                    planDisplay.Font = new Font("arial", 8);
                    planDisplay.BorderStyle = BorderStyle.FixedSingle;

                    Color backColor;
                    switch (rows)
                    {
                        case 1: backColor = Color.LightBlue; break;
                        case 2: backColor = Color.LightCoral; break;
                        case 3: backColor = Color.LightCyan; break;
                        case 4: backColor = Color.LightGreen; break;
                        case 5: backColor = Color.LightPink; break;
                        case 6: backColor = Color.LightSalmon; break;
                        case 7: backColor = Color.LightSkyBlue; break;
                        default: backColor = Color.LightSteelBlue; break;
                    }

                    planDisplay.BackColor = backColor;
                    planDisplay.Text = recipePlanHeaderDTOList[rows].RecipePlanDetailsDTOList.Count + " Recipes";
                    if (recur_flag == true)
                    {
                        planDisplay.Text += MessageContainerList.GetMessage(executioncontex, "Recurs every") + " " + frequency;
                        planDisplay.Text += " " + MessageContainerList.GetMessage(executioncontex, "until") + " " + recur_end_date.ToString(utilities.getDateFormat());
                    }

                    planDisplay.Tag = plan_id;
                    planDisplay.DoubleClick += new EventHandler(recipePlanDisplay_DoubleClick);
                    planDisplay.Click += new EventHandler(recipePlanDisplay_Click);

                    if (maxRow == minRow)
                        numrows = 1;
                    else
                        numrows = maxRow - minRow + 1;
                    planDisplay.Size = new Size(dgvDay.Columns[0].Width - 10, dgvDay.Rows[0].Height * numrows);
                    planDisplay.Location = new Point(dgvDay.RowHeadersWidth + 1, dgvDay.Rows[0].Height * minRow + dgvDay.ColumnHeadersHeight + 1);
                    tabPageDay.Controls.Add(planDisplay);
                    planDisplay.BringToFront();
                }
            }
            tabPageDay.Refresh();


            for (int i = 0; i < tabPageWeek.Controls.Count; i++)
            {
                Control c = tabPageWeek.Controls[i];
                if (c.Name.Contains("planDisplay"))
                {
                    tabPageWeek.Controls.Remove(c);
                    c.Dispose();
                    i = -1;
                }
            }

            recipePlanHeaderDTOList = recipePlanHeaderListBL.GetRecipePlanHeaderDTOList(getFirstDayOfWeek().Date, getFirstDayOfWeek().AddDays(7).Date, utilities.ExecutionContext.GetSiteId());
            for (int rows = 0; rows < recipePlanHeaderDTOList.Count; rows++)
            {
                plan_id = recipePlanHeaderDTOList[rows].RecipePlanHeaderId;
                planFromDate = recipePlanHeaderDTOList[rows].PlanDateTime;
                planToDate = recipePlanHeaderDTOList[rows].PlanDateTime;
                sun = recipePlanHeaderDTOList[rows].Sunday;
                mon = recipePlanHeaderDTOList[rows].Monday;
                tue = recipePlanHeaderDTOList[rows].Tuesday;
                wed = recipePlanHeaderDTOList[rows].Wednesday;
                thu = recipePlanHeaderDTOList[rows].Thursday;
                fri = recipePlanHeaderDTOList[rows].Friday;
                sat = recipePlanHeaderDTOList[rows].Saturday;
                if (recipePlanHeaderDTOList[rows].RecurFlag != null)
                {
                    recur_flag = recipePlanHeaderDTOList[rows].RecurFlag;
                }
                else
                {
                    recur_flag = false;
                }
                recur_frequency = recipePlanHeaderDTOList[rows].RecurFrequency;
                recurType = recipePlanHeaderDTOList[rows].RecurType;
                if (recipePlanHeaderDTOList[rows].RecurEndDate != null)
                    recur_end_date = Convert.ToDateTime(recipePlanHeaderDTOList[rows].RecurEndDate);

                planFromDate = planFromDate.Date.AddHours(planFromDate.Hour).AddMinutes(planFromDate.Minute);
                planToDate = planToDate.Date.AddHours(planToDate.Hour).AddMinutes(planToDate.Minute);

                origplanFromDate = planFromDate;
                origplanToDate = planToDate;

                int numIterations = 0;

                if (recur_flag == true)
                    numIterations = dgvWeek.Columns.Count; // repeat for each day of week to check if recur applicable
                else
                    numIterations = 1;

                for (int recurDate = 0; recurDate < numIterations; recurDate++)
                {
                    string frequency = string.Empty;
                    if (recur_flag == true)
                    {
                        if (recur_frequency == 'D')
                            frequency = "day";
                        else if (recur_frequency == 'W')
                            frequency = "week";
                        else
                            frequency = "month";
                        DateTime cellTimeRecur = getGridCellDateTime(0, recurDate, true); // get date on day of week

                        if (recur_frequency == 'W')
                        {
                            bool recur = false;
                            string dayOfWeek = cellTimeRecur.DayOfWeek.ToString();
                            if (cellTimeRecur.DayOfWeek == origplanFromDate.DayOfWeek)
                            {
                                recur = true;
                            }
                            if (sun == true && dayOfWeek.Equals("Sunday"))
                            {
                                recur = true;
                            }
                            else if (mon == true && dayOfWeek.Equals("Monday"))
                            {
                                recur = true;
                            }
                            else if (tue == true && dayOfWeek.Equals("Tuesday"))
                            {
                                recur = true;
                            }
                            else if (wed == true && dayOfWeek.Equals("Wednesday"))
                            {
                                recur = true;
                            }
                            else if (thu == true && dayOfWeek.Equals("Thursday"))
                            {
                                recur = true;
                            }
                            else if (fri == true && dayOfWeek.Equals("Friday"))
                            {
                                recur = true;
                            }
                            else if (sat == true && dayOfWeek.Equals("Saturday"))
                            {
                                recur = true;
                            }
                            if (recur == false)
                            {
                                continue;
                            }
                        }
                        else if (recur_frequency == 'M')
                        {
                            if (recurType == 'D')
                            {
                                if (cellTimeRecur.Day != planFromDate.Day)
                                    continue;
                            }
                            else
                            {
                                DateTime monthStart = new DateTime(planFromDate.Year, planFromDate.Month, 1);
                                int promoWeekNo = 0;
                                while (monthStart <= planFromDate)
                                {
                                    if (monthStart.DayOfWeek == planFromDate.DayOfWeek)
                                        promoWeekNo++;
                                    monthStart = monthStart.AddDays(1);
                                }

                                monthStart = new DateTime(cellTimeRecur.Year, cellTimeRecur.Month, 1);
                                int gridWeekNo = 0;
                                while (monthStart <= cellTimeRecur)
                                {
                                    if (monthStart.DayOfWeek == cellTimeRecur.DayOfWeek)
                                        gridWeekNo++;
                                    monthStart = monthStart.AddDays(1);
                                }

                                if (cellTimeRecur.DayOfWeek != planFromDate.DayOfWeek || gridWeekNo != promoWeekNo)
                                    continue;
                            }
                        }
                        if ((recur_end_date.Date >= cellTimeRecur.Date) && (origplanFromDate.Date <= cellTimeRecur.Date))
                        {
                            TimeSpan ts = planToDate.Date - planFromDate.Date; // used to get number of days the plan spans over. change plan from and to days as per week day date
                            planFromDate = cellTimeRecur.Date.AddHours(planFromDate.Hour).AddMinutes(planFromDate.Minute);
                            planToDate = cellTimeRecur.Date.AddDays(ts.Days).AddHours(planToDate.Hour).AddMinutes(planToDate.Minute);
                        }
                        else
                        {
                            continue;
                        }
                    }

                    foreach (DataGridViewColumn dc in dgvWeek.Columns)
                    {
                        if (!dc.Visible)
                            continue;

                        minRow = -1;
                        maxRow = 100;
                        for (int i = 0; i < dgvWeek.RowCount; i++)
                        {
                            cellTime = getGridCellDateTime(i, dc.Index, true);
                            if (cellTime.Date >= planFromDate.Date && cellTime.Date < planToDate.AddDays(1).Date)
                            {
                                if (minRow == -1)
                                    minRow = i;
                                maxRow = i;
                                break;
                            }
                        }

                        if (minRow != -1)
                        {
                            Label planDisplay = new Label();
                            planDisplay.Name = "planDisplay" + "|" + cellTime.Date.ToString();
                            planDisplay.Font = new Font("arial", 8);
                            planDisplay.BorderStyle = BorderStyle.FixedSingle;

                            Color backColor;
                            switch (rows)
                            {
                                case 1: backColor = Color.LightBlue; break;
                                case 2: backColor = Color.LightCoral; break;
                                case 3: backColor = Color.LightCyan; break;
                                case 4: backColor = Color.LightGreen; break;
                                case 5: backColor = Color.LightPink; break;
                                case 6: backColor = Color.LightSalmon; break;
                                case 7: backColor = Color.LightSkyBlue; break;
                                default: backColor = Color.LightSteelBlue; break;
                            }

                            planDisplay.BackColor = backColor;
                            planDisplay.Text = recipePlanHeaderDTOList[rows].RecipePlanDetailsDTOList.Count + " Recipes";// + origplanFromDate.ToString("MMM dd, h:mm tt") + " to " + origplanToDate.ToString("MMM dd, h:mm tt");
                            if (recur_flag == true)
                            {
                                planDisplay.Text += MessageContainerList.GetMessage(executioncontex,"Recurs every") + " " + frequency;
                                planDisplay.Text += " " + MessageContainerList.GetMessage(executioncontex,"until") + " " + recur_end_date.ToString(utilities.getDateFormat());
                            }

                            planDisplay.Tag = plan_id;
                            planDisplay.DoubleClick += new EventHandler(recipePlanDisplay_DoubleClick);
                            planDisplay.Click += new EventHandler(recipePlanDisplay_Click);

                            if (maxRow == minRow)
                                numrows = 1;
                            else
                                numrows = maxRow - minRow + 1;
                            planDisplay.Size = new Size(dgvWeek.Columns[0].Width - 10, dgvWeek.Rows[0].Height * numrows);
                            planDisplay.Location = new Point(dgvWeek.Columns[0].Width * dc.Index + dgvWeek.RowHeadersWidth + 1, dgvDay.Rows[0].Height * minRow + dgvWeek.ColumnHeadersHeight + 1);
                            tabPageWeek.Controls.Add(planDisplay);
                            planDisplay.BringToFront();
                        }
                    }
                }
            }
            tabPageWeek.Refresh();
            log.LogMethodExit();
        }



        void recipePlanDisplay_DoubleClick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            lblMessage.Text = string.Empty;
            dgvDay_DoubleClick(sender, e);
            log.LogMethodExit();
        }

        void recipePlanDisplay_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            lblMessage.Text = string.Empty;
            dgvDay_Click(sender, e);
            log.LogMethodExit();
        }

        private void btnAddRecipe_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                frmAddRecipeUI frmAddRecipe = new frmAddRecipeUI(utilities, dtpPlanDate.Value);
                CommonUIDisplay.setupVisuals(frmAddRecipe);
                frmAddRecipe.ShowDialog();
                RefreshMonth();
                RefreshDayWeek();
                LoadRecipePlanDetails();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void dgvWeek_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            log.LogMethodEntry();
            cellPaint(sender, e);
            log.LogMethodExit();
        }

        private void cellPaint(object sender, DataGridViewCellPaintingEventArgs e)
        {
            log.LogMethodEntry();
            if (e.Value != null)
            {
                if (e.Value.ToString().Contains(":")) // header
                {
                    e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.ContentForeground);
                    string hour = e.Value.ToString();
                    string mins = hour.Substring(hour.IndexOf(':') + 1);
                    hour = hour.Substring(0, hour.IndexOf(':'));

                    StringFormat sf = new StringFormat();
                    sf.Alignment = StringAlignment.Far;

                    e.Graphics.DrawString(hour, new Font(this.Font.FontFamily, 12, FontStyle.Bold)
                        , Brushes.Black,
                        new Rectangle(e.CellBounds.X, e.CellBounds.Y, e.CellBounds.Width - 35, e.CellBounds.Y),
                        sf);

                    e.Graphics.DrawString(mins, new Font(this.Font.FontFamily, 9, FontStyle.Regular)
                        , Brushes.Black,
                        new Rectangle(e.CellBounds.X, e.CellBounds.Y, e.CellBounds.Width - 10, e.CellBounds.Y),
                        sf);

                    e.Handled = true;
                }
                else if (e.ColumnIndex >= 0 && e.RowIndex >= 0) // cells
                {
                    e.Paint(e.CellBounds, DataGridViewPaintParts.All);

                    if (e.RowIndex % 2 == 0)
                    {
                        e.Graphics.DrawLine(Pens.DarkKhaki, e.CellBounds.X, e.CellBounds.Y, e.CellBounds.Right, e.CellBounds.Y);
                        e.Graphics.DrawLine(Pens.Khaki, e.CellBounds.X, e.CellBounds.Bottom, e.CellBounds.Right, e.CellBounds.Bottom);
                    }
                    else
                    {
                        e.Graphics.DrawLine(Pens.Khaki, e.CellBounds.X, e.CellBounds.Y, e.CellBounds.Right, e.CellBounds.Y);
                        e.Graphics.DrawLine(Pens.DarkKhaki, e.CellBounds.X, e.CellBounds.Bottom, e.CellBounds.Right, e.CellBounds.Bottom);
                    }
                    e.Graphics.DrawLine(Pens.Black, e.CellBounds.X, e.CellBounds.Y, e.CellBounds.X, e.CellBounds.Bottom);

                    e.Handled = true;
                }
            }
            log.LogMethodExit();
        }

        private void dgvDay_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            log.LogMethodEntry();
            cellPaint(sender, e);
            log.LogMethodExit();
        }

        private void btnCopyRecipe_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                frmCopyRecipeUI frmCopyRecipeUI = new frmCopyRecipeUI(utilities, dtpPlanDate.Value);
                CommonUIDisplay.setupVisuals(frmCopyRecipeUI);
                frmCopyRecipeUI.ShowDialog();
                RefreshMonth();
                RefreshDayWeek();
                dgvPlanDetails.RefreshEdit();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Method to create Month view
        /// </summary>
        /// <param name="e"></param>
        private void RenderMonthCalendar(PaintEventArgs e)
        {
            log.LogMethodEntry();
            calendarDays.Clear();
            int MarginSize = 2;
            int cellWidth = 110;
            int cellHeight = 60;
            Font dayOfWeekFont = new Font("Arial", 8, FontStyle.Regular);
            Font daysFont = new Font("Arial", 8, FontStyle.Regular);
            Font todayFont = new Font("Arial", 8, FontStyle.Bold);
            Font dateHeaderFont = new Font("Arial", 10, FontStyle.Bold);
            var bmp = new Bitmap(ClientSize.Width, ClientSize.Height);
            Graphics g = Graphics.FromImage(bmp);
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            g.Clear(Color.LightYellow);
            SizeF sunSize = g.MeasureString("Sun", dayOfWeekFont);
            SizeF monSize = g.MeasureString("Mon", dayOfWeekFont);
            SizeF tueSize = g.MeasureString("Tue", dayOfWeekFont);
            SizeF wedSize = g.MeasureString("Wed", dayOfWeekFont);
            SizeF thuSize = g.MeasureString("Thu", dayOfWeekFont);
            SizeF friSize = g.MeasureString("Fri", dayOfWeekFont);
            SizeF satSize = g.MeasureString("Sat", dayOfWeekFont);
            SizeF dateHeaderSize = g.MeasureString(
                calendarDate.ToString("MMMM") + " " + calendarDate.Year.ToString(CultureInfo.InvariantCulture), dateHeaderFont);
            int headerSpacing = 15;
            //int headerSpacing = Max(sunSize.Height, monSize.Height, tueSize.Height, wedSize.Height, thuSize.Height, friSize.Height,
            //              satSize.Height) + 1;
            int controlsSpacing = 5;

            DateTime date = new DateTime(calendarDate.Year, calendarDate.Month, DateTime.DaysInMonth(calendarDate.Year, calendarDate.Month));
            var beginningOfMonth = new DateTime(date.Year, date.Month, 1);

            while (date.Date.AddDays(1).DayOfWeek != CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek)
                date = date.AddDays(1);

            log.LogMethodExit();
            int numWeeks = (int)Math.Truncate(date.Subtract(beginningOfMonth).TotalDays / 7f) + 1;

            int xStart = MarginSize;
            int yStart = MarginSize;
            DayOfWeek startWeekEnum = new DateTime(calendarDate.Year, calendarDate.Month, 1).DayOfWeek;
            int startWeek = ((int)startWeekEnum) + 1;
            int rogueDays = startWeek - 1;

            yStart += headerSpacing + controlsSpacing;

            int counter = 1;
            int counter2 = 1;

            bool first = false;
            bool first2 = false;
            for (int y = 0; y < numWeeks; y++)
            {
                for (int x = 0; x < 7; x++)
                {
                    if (rogueDays == 0 && counter <= DateTime.DaysInMonth(calendarDate.Year, calendarDate.Month))
                    {
                        if (!calendarDays.ContainsKey(counter))
                            calendarDays.Add(counter, new Point(xStart, (int)(yStart + 2f + g.MeasureString(counter.ToString(CultureInfo.InvariantCulture), daysFont).Height)));
                        if (first == false)
                        {
                            first = true;
                            if (calendarDate.Year == ServerDateTime.Now.Year && calendarDate.Month == ServerDateTime.Now.Month
                         && counter == ServerDateTime.Now.Day)
                            {
                                g.DrawString(
                                    calendarDate.ToString("MMM") + " " + counter.ToString(CultureInfo.InvariantCulture),
                                    todayFont, Brushes.Black, xStart + 1, yStart + 1);
                            }
                            else
                            {
                                g.DrawString(
                                    calendarDate.ToString("MMM") + " " + counter.ToString(CultureInfo.InvariantCulture),
                                    daysFont, Brushes.Black, xStart + 1, yStart + 1);
                            }
                        }
                        else
                        {
                            if (calendarDate.Year == ServerDateTime.Now.Year && calendarDate.Month == ServerDateTime.Now.Month
                         && counter == ServerDateTime.Now.Day)
                            {
                                g.DrawString(counter.ToString(CultureInfo.InvariantCulture), todayFont, Brushes.Black, xStart + 1, yStart + 1);
                            }
                            else
                            {
                                g.DrawString(counter.ToString(CultureInfo.InvariantCulture), daysFont, Brushes.Black, xStart + 1, yStart + 1);
                            }
                        }
                        counter++;
                    }
                    else if (rogueDays > 0)
                    {
                        int dm =
                            DateTime.DaysInMonth(calendarDate.AddMonths(-1).Year, calendarDate.AddMonths(-1).Month) -
                            rogueDays + 1;
                        g.DrawString(dm.ToString(CultureInfo.InvariantCulture), daysFont, new SolidBrush(Color.FromArgb(170, 170, 170)), xStart + 1, yStart + 1);
                        rogueDays--;
                    }

                    g.DrawRectangle(Pens.DarkGray, xStart, yStart, cellWidth, cellHeight);
                    if (rogueDays == 0 && counter > DateTime.DaysInMonth(calendarDate.Year, calendarDate.Month))
                    {
                        if (first2 == false)
                            first2 = true;
                        else
                        {
                            if (counter2 == 1)
                            {
                                g.DrawString(calendarDate.AddMonths(1).ToString("MMM") + " " + counter2.ToString(CultureInfo.InvariantCulture), daysFont,
                                             new SolidBrush(Color.FromArgb(170, 170, 170)), xStart + 1, yStart + 1);
                            }
                            else
                            {
                                g.DrawString(counter2.ToString(CultureInfo.InvariantCulture), daysFont,
                                             new SolidBrush(Color.FromArgb(170, 170, 170)), xStart + 1, yStart + 1);
                            }
                            counter2++;
                        }
                    }
                    xStart += cellWidth;
                }
                xStart = MarginSize;
                yStart += cellHeight;
            }
            xStart = MarginSize + ((cellWidth - (int)sunSize.Width) / 2);
            yStart = MarginSize + controlsSpacing;

            g.DrawString("Sun", dayOfWeekFont, Brushes.Black, xStart, yStart);
            xStart = MarginSize + ((cellWidth - (int)monSize.Width) / 2) + cellWidth;
            g.DrawString("Mon", dayOfWeekFont, Brushes.Black, xStart, yStart);

            xStart = MarginSize + ((cellWidth - (int)tueSize.Width) / 2) + cellWidth * 2;
            g.DrawString("Tue", dayOfWeekFont, Brushes.Black, xStart, yStart);

            xStart = MarginSize + ((cellWidth - (int)wedSize.Width) / 2) + cellWidth * 3;
            g.DrawString("Wed", dayOfWeekFont, Brushes.Black, xStart, yStart);

            xStart = MarginSize + ((cellWidth - (int)thuSize.Width) / 2) + cellWidth * 4;
            g.DrawString("Thu", dayOfWeekFont, Brushes.Black, xStart, yStart);

            xStart = MarginSize + ((cellWidth - (int)friSize.Width) / 2) + cellWidth * 5;
            g.DrawString("Fri", dayOfWeekFont, Brushes.Black, xStart, yStart);

            xStart = MarginSize + ((cellWidth - (int)satSize.Width) / 2) + cellWidth * 6;
            g.DrawString("Sat", dayOfWeekFont, Brushes.Black, xStart, yStart);

            //Show date in header
            g.DrawString(
                calendarDate.ToString("MMMM") + " " + calendarDate.Year.ToString(CultureInfo.InvariantCulture),
                dateHeaderFont, Brushes.Black, ClientSize.Width - MarginSize - dateHeaderSize.Width,
                MarginSize);

            g.Dispose();
            e.Graphics.DrawImage(bmp, 0, 0, ClientSize.Width, ClientSize.Height);
            bmp.Dispose();
            if (renderCalendar)
            {
                RefreshMonth();
                renderCalendar = false;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Refresh's Month view
        /// </summary>
        private void RefreshMonth()
        {
            log.LogMethodEntry();

            for (int i = 0; i < Calendar.Controls.Count; i++)
            {
                Control c = Calendar.Controls[i];
                if (c.Name.Contains("recipePlanDisplay"))
                {
                    Calendar.Controls.Remove(c);
                    c.Dispose();
                    i = -1;
                }
            }
            DateTime planFromDate, planToDate;
            DateTime origplanFromDate, origplanToDate;

            long plan_id;
            char? recur_frequency, recurType;
            bool? recur_flag;
            DateTime recur_end_date = DateTime.MinValue;
            bool? sun, mon, tue, wed, thu, fri, sat;
            DateTime cellTime;


            int cellWidth = 110;
            int cellHeight = 60;
            DateTime fromdate = new DateTime(calendarDate.Year, calendarDate.Month, 1);
            DateTime todate = fromdate.AddDays(DateTime.DaysInMonth(calendarDate.Year, calendarDate.Month));

            RecipePlanHeaderListBL recipePlanHeaderListBL = new RecipePlanHeaderListBL(executioncontex);
            List<RecipePlanHeaderDTO> recipePlanHeaderDTOList = recipePlanHeaderListBL.GetRecipePlanHeaderDTOList(fromdate.Date, todate.Date, utilities.ExecutionContext.GetSiteId());

            for (int rows = 0; rows < recipePlanHeaderDTOList.Count; rows++)
            {

                plan_id = recipePlanHeaderDTOList[rows].RecipePlanHeaderId;
                planFromDate = recipePlanHeaderDTOList[rows].PlanDateTime;
                planToDate = recipePlanHeaderDTOList[rows].PlanDateTime;
                sun = recipePlanHeaderDTOList[rows].Sunday;
                mon = recipePlanHeaderDTOList[rows].Monday;
                tue = recipePlanHeaderDTOList[rows].Tuesday;
                wed = recipePlanHeaderDTOList[rows].Wednesday;
                thu = recipePlanHeaderDTOList[rows].Thursday;
                fri = recipePlanHeaderDTOList[rows].Friday;
                sat = recipePlanHeaderDTOList[rows].Saturday;
                if (recipePlanHeaderDTOList[rows].RecurFlag != null)
                {
                    recur_flag = recipePlanHeaderDTOList[rows].RecurFlag;
                }
                else
                {
                    recur_flag = false;
                }
                recur_frequency = recipePlanHeaderDTOList[rows].RecurFrequency;
                recurType = recipePlanHeaderDTOList[rows].RecurType;
                if (recipePlanHeaderDTOList[rows].RecurEndDate != null)
                    recur_end_date = Convert.ToDateTime(recipePlanHeaderDTOList[rows].RecurEndDate);

                int numIterations = 0;
                origplanFromDate = planFromDate;
                origplanToDate = planToDate;
                if (recur_flag == true)
                    numIterations = DateTime.DaysInMonth(calendarDate.Year, calendarDate.Month); // repeat for each day of week to check if recur applicable
                else
                    numIterations = 1;

                for (int recurDate = 0; recurDate < numIterations; recurDate++)
                {
                    if (recur_flag == true)
                    {
                        DateTime cellTimeRecur = getGridCellDateTime(0, recurDate, false, true); // get date on day of week
                        bool recur = false;
                        if (recur_frequency == 'W')
                        {
                            string dayOfWeek = cellTimeRecur.DayOfWeek.ToString();
                            if (cellTimeRecur.DayOfWeek == origplanFromDate.DayOfWeek)
                            {
                                recur = true;
                            }
                            if (sun == true && dayOfWeek.Equals("Sunday"))
                            {
                                recur = true;
                            }
                            else if (mon == true && dayOfWeek.Equals("Monday"))
                            {
                                recur = true;
                            }
                            else if (tue == true && dayOfWeek.Equals("Tuesday"))
                            {
                                recur = true;
                            }
                            else if (wed == true && dayOfWeek.Equals("Wednesday"))
                            {
                                recur = true;
                            }
                            else if (thu == true && dayOfWeek.Equals("Thursday"))
                            {
                                recur = true;
                            }
                            else if (fri == true && dayOfWeek.Equals("Friday"))
                            {
                                recur = true;
                            }
                            else if (sat == true && dayOfWeek.Equals("Saturday"))
                            {
                                recur = true;
                            }
                            if (recur == false)
                            {
                                continue;
                            }
                        }
                        else if (recur_frequency == 'M')
                        {
                            if (recurType == 'D')
                            {
                                if (cellTimeRecur.Day != planFromDate.Day)
                                    continue;
                            }
                            else
                            {
                                DateTime monthStart = new DateTime(planFromDate.Year, planFromDate.Month, 1);
                                int promoWeekNo = 0;
                                while (monthStart <= planFromDate)
                                {
                                    if (monthStart.DayOfWeek == planFromDate.DayOfWeek)
                                        promoWeekNo++;
                                    monthStart = monthStart.AddDays(1);
                                }

                                monthStart = new DateTime(cellTimeRecur.Year, cellTimeRecur.Month, 1);
                                int gridWeekNo = 0;
                                while (monthStart <= cellTimeRecur)
                                {
                                    if (monthStart.DayOfWeek == cellTimeRecur.DayOfWeek)
                                        gridWeekNo++;
                                    monthStart = monthStart.AddDays(1);
                                }

                                if (cellTimeRecur.DayOfWeek != planFromDate.DayOfWeek || gridWeekNo != promoWeekNo)
                                    continue;
                            }
                        }
                        if ((recur_end_date.Date >= cellTimeRecur.Date) && (origplanFromDate.Date <= cellTimeRecur.Date))
                        {
                            TimeSpan ts = planToDate.Date - planFromDate.Date; // used to get number of days the plan spans over. change plan from and to days as per week day date
                            planFromDate = cellTimeRecur.Date.AddHours(planFromDate.Hour).AddMinutes(planFromDate.Minute);
                            planToDate = cellTimeRecur.Date.AddDays(ts.Days).AddHours(planToDate.Hour).AddMinutes(planToDate.Minute);
                        }
                        else
                        {
                            continue;
                        }
                    }
                    Point point = new Point(0, 0);
                    for (int i = 1; i <= DateTime.DaysInMonth(calendarDate.Year, calendarDate.Month); i++)
                    {
                        DateTime dt = new DateTime(calendarDate.Year, calendarDate.Month, i);
                        cellTime = dt.Date;
                        if (calendarDays.Count > 0)
                        {
                            point = calendarDays[i];
                        }
                        if (cellTime.Date == planFromDate.Date)
                        {
                            Color backColor;
                            switch (rows)
                            {
                                case 1: backColor = Color.LightBlue; break;
                                case 2: backColor = Color.LightCoral; break;
                                case 3: backColor = Color.LightCyan; break;
                                case 4: backColor = Color.LightGreen; break;
                                case 5: backColor = Color.LightPink; break;
                                case 6: backColor = Color.LightSalmon; break;
                                case 7: backColor = Color.LightSkyBlue; break;
                                default: backColor = Color.LightSteelBlue; break;
                            }
                            Label recipePlanDisplay = new Label();
                            recipePlanDisplay.Name = "recipePlanDisplay" + "|" + dt.Date.ToString();
                            recipePlanDisplay.Font = new Font("arial", 8);
                            recipePlanDisplay.BorderStyle = BorderStyle.FixedSingle;
                            recipePlanDisplay.BackColor = backColor;
                            recipePlanDisplay.Text = recipePlanHeaderDTOList[rows].RecipePlanDetailsDTOList.Count + " Recipes";
                            if (recur_flag == true)
                            {
                                string frequency = string.Empty;
                                if (recur_frequency == 'D')
                                    frequency = "day";
                                else if (recur_frequency == 'W')
                                    frequency = "week";
                                else
                                    frequency = "month";

                                recipePlanDisplay.Text += MessageContainerList.GetMessage(executioncontex, "Recurs every") + " "  + frequency;
                                recipePlanDisplay.Text += " " + MessageContainerList.GetMessage(executioncontex, "till") + " " + recur_end_date.Day;
                            }

                            recipePlanDisplay.Tag = plan_id;
                            recipePlanDisplay.DoubleClick += new EventHandler(recipePlanDisplay_DoubleClick);
                            recipePlanDisplay.Click += new EventHandler(recipePlanDisplay_Click);
                            recipePlanDisplay.Size = new Size(cellWidth - 10, cellHeight - 17);
                            recipePlanDisplay.Location = new Point(point.X + 1, point.Y + 1);
                            Calendar.Controls.Add(recipePlanDisplay);
                            recipePlanDisplay.BringToFront();
                        }
                        else
                        {
                            Label recipePlanDisplay = new Label();
                            recipePlanDisplay.Name = "recipePlanDisplay" + "|" + dt.Date.ToString();
                            recipePlanDisplay.Font = new Font("arial", 8);
                            recipePlanDisplay.BackColor = Color.LightYellow;
                            recipePlanDisplay.Tag = -1;
                            recipePlanDisplay.Text = dt.Date.ToString();
                            recipePlanDisplay.ForeColor = Color.LightYellow;
                            recipePlanDisplay.DoubleClick += new EventHandler(recipePlanDisplay_DoubleClick);
                            recipePlanDisplay.Click += new EventHandler(recipePlanDisplay_Click);
                            recipePlanDisplay.Size = new Size(cellWidth - 10, cellHeight - 20);
                            recipePlanDisplay.Location = new Point(point.X + 1, point.Y + 1);
                            Calendar.Controls.Add(recipePlanDisplay);
                        }
                    }
                }
            }
            if (recipePlanHeaderDTOList != null && recipePlanHeaderDTOList.Count == 0 && calendarDays.Count > 0)
            {
                for (int i = 1; i <= DateTime.DaysInMonth(calendarDate.Year, calendarDate.Month); i++)
                {
                    Point point = new Point(0, 0);
                    DateTime dt = new DateTime(calendarDate.Year, calendarDate.Month, i);
                    cellTime = dt.Date;
                    point = calendarDays[i];
                    Label recipePlanDisplay = new Label();
                    recipePlanDisplay.Name = "recipePlanDisplay" + "|" + dt.Date.ToString();
                    recipePlanDisplay.Font = new Font("arial", 8);
                    recipePlanDisplay.BackColor = Color.LightYellow;
                    recipePlanDisplay.Tag = -1;
                    recipePlanDisplay.Text = dt.Date.ToString();
                    recipePlanDisplay.ForeColor = Color.LightYellow;
                    recipePlanDisplay.DoubleClick += new EventHandler(recipePlanDisplay_DoubleClick);
                    recipePlanDisplay.Click += new EventHandler(recipePlanDisplay_Click);
                    recipePlanDisplay.Size = new Size(cellWidth - 10, cellHeight - 20);
                    recipePlanDisplay.Location = new Point(point.X + 1, point.Y + 1);
                    Calendar.Controls.Add(recipePlanDisplay);
                }
            }
            Calendar.Refresh();
            log.LogMethodExit();
        }


        private void Calendar_Paint_1(object sender, PaintEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                RenderMonthCalendar(e);
            }
            catch (Exception ex)
            {
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

        private void btnPrev_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                lblMessage.Text = string.Empty;
                grpPlanDetails.Visible = false;
                if (tabCalendar.SelectedIndex == 0)
                {
                    dtpPlanDate.Value = dtpPlanDate.Value.AddDays(-1);
                }
                else if (tabCalendar.SelectedIndex == 1)
                {
                    dtpPlanDate.Value = dtpPlanDate.Value.AddDays(-7);
                }
                if (tabCalendar.SelectedIndex == 2)
                {
                    calendarDate = calendarDate.AddMonths(-1);
                    SetMonthName(calendarDate.Month);
                    dtpPlanDate.Value = dtpPlanDate.Value.AddMonths(-1);
                    Refresh();
                    RefreshMonth();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                lblMessage.Text = string.Empty;
                grpPlanDetails.Visible = false;
                if (tabCalendar.SelectedIndex == 0)
                {
                    dtpPlanDate.Value = dtpPlanDate.Value.AddDays(1);
                }
                else if (tabCalendar.SelectedIndex == 1)
                {
                    dtpPlanDate.Value = dtpPlanDate.Value.AddDays(7);
                }
                if (tabCalendar.SelectedIndex == 2)
                {
                    calendarDate = calendarDate.AddMonths(1);
                    dtpPlanDate.Value = dtpPlanDate.Value.AddMonths(1);
                    SetMonthName(calendarDate.Month);
                    Refresh();
                    RefreshMonth();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void dtpPlanDate_ValueChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            grpPlanDetails.Visible = false;
            updateColumnHeaders();
            RefreshDayWeek();
            calendarDate = dtpPlanDate.Value.Date;
            SetMonthName(calendarDate.Month);
            if (calendarDays != null && calendarDays.Any())
            {
                RefreshMonth();
            }
            if (tabCalendar.SelectedTab.Name.Contains("Week"))
                dgvWeek.Focus();
            else if (tabCalendar.SelectedTab.Name.Contains("Day"))
                dgvDay.Focus();
            else
            {
                Calendar.Focus();
            }
            log.LogMethodExit();
        }

        private void tabCalendar_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                lblMessage.Text = string.Empty;
                grpPlanDetails.Visible = false;
                if (tabCalendar.SelectedIndex == 2)
                {
                    calendarDate = dtpPlanDate.Value.Date;
                    SetMonthName(calendarDate.Month);
                    if (calendarDays != null && calendarDays.Any())
                    {
                        RefreshMonth();
                    }
                }
                if (tabCalendar.SelectedIndex == 3)
                {
                    btnPrev.Enabled = false;
                    btnNext.Enabled = false;
                    LoadAllRecipe();
                }
                else
                {
                    btnPrev.Enabled = true;
                    btnNext.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void LoadAllRecipe(DateTime? fromDate = null , DateTime? todate = null)
        {
            log.LogMethodEntry();
            RecipePlanHeaderListBL recipePlanHeaderListBL = new RecipePlanHeaderListBL(executioncontex);
            List<KeyValuePair<RecipePlanHeaderDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<RecipePlanHeaderDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<RecipePlanHeaderDTO.SearchByParameters, string>(RecipePlanHeaderDTO.SearchByParameters.IS_ACTIVE, "1"));
            searchParameters.Add(new KeyValuePair<RecipePlanHeaderDTO.SearchByParameters, string>(RecipePlanHeaderDTO.SearchByParameters.SITE_ID, executioncontex.GetSiteId().ToString()));
            if(fromDate != null && todate != null)
            {
                searchParameters.Add(new KeyValuePair<RecipePlanHeaderDTO.SearchByParameters, string>(RecipePlanHeaderDTO.SearchByParameters.FROM_DATE, Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)));
                searchParameters.Add(new KeyValuePair<RecipePlanHeaderDTO.SearchByParameters, string>(RecipePlanHeaderDTO.SearchByParameters.TO_DATE, Convert.ToDateTime(todate).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)));

            }
            recipePlanHeaderDTOListOnDisplay = recipePlanHeaderListBL.GetAllRecipePlanHeaderDTOList(searchParameters, true, true);
            BindingSource recipePlanHeaderBS = new BindingSource();
            if (recipePlanHeaderDTOListOnDisplay != null && recipePlanHeaderDTOListOnDisplay.Count > 0)
            {
                foreach (RecipePlanHeaderDTO recipePlanHeaderDTO in recipePlanHeaderDTOListOnDisplay)
                {
                    recipePlanHeaderDTO.RecipeCount = recipePlanHeaderDTO.RecipePlanDetailsDTOList.Count + " Recipe";
                }
                SortableBindingList<RecipePlanHeaderDTO> recipePlanHeaderListList = new SortableBindingList<RecipePlanHeaderDTO>(recipePlanHeaderDTOListOnDisplay);
                recipePlanHeaderBS.DataSource = recipePlanHeaderListList;
            }
            else
            {
                recipePlanHeaderBS.DataSource = new SortableBindingList<RecipePlanHeaderDTO>();
            }
            dgvAll.DataSource = recipePlanHeaderBS;
            log.LogMethodExit();
        }

        private void dgvDay_DoubleClick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                cbxSelect.Checked = false;
                Label label = sender as Label;
                frmAddRecipeUI f = null;
                if (label == null)
                {
                    DateTime FromTime = DateTime.MinValue, ToTime = DateTime.MinValue;
                    DateTime RefDate = getFirstDayOfWeek();

                    if (tabCalendar.SelectedTab.Name.Contains("Week"))
                    {
                        getSelectedTimeSlotOfWeek(RefDate, ref FromTime, ref ToTime);
                        if (FromTime == DateTime.MinValue)
                        {
                            log.LogMethodExit(null);
                            return;
                        }
                        f = new frmAddRecipeUI(utilities, FromTime);
                    }
                    if (tabCalendar.SelectedTab.Name.Contains("Day"))
                    {
                        if (dgvDay.SelectedCells.Count <= 0)
                        {
                            log.LogMethodExit(null);
                            return;
                        }
                        FromTime = dtpPlanDate.Value.Date;
                        f = new frmAddRecipeUI(utilities, FromTime);

                    }

                }
                else
                {
                    List<string> recipeDateList = label.Name.Split('|').ToList();
                    DateTime planDate = Convert.ToDateTime(recipeDateList[1]).Date;
                    f = new frmAddRecipeUI(utilities, planDate);
                }
                if (f != null)
                {
                    CommonUIDisplay.setupVisuals(f);
                    f.ShowDialog();
                    RefreshDayWeek();
                    RefreshMonth();
                    LoadRecipePlanDetails();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void getSelectedTimeSlotOfWeek(DateTime RefDate, ref DateTime FromTime, ref DateTime ToTime)
        {
            log.LogMethodEntry(RefDate, FromTime, ToTime);
            int minColumn = 100, maxColumn = -1;
            int minColRow = 100, maxColRow = -1;

            if (dgvWeek.SelectedCells.Count <= 0)
            {
                log.LogMethodExit(null);
                return;
            }
            foreach (DataGridViewCell c in dgvWeek.SelectedCells)
            {
                if (c.ColumnIndex < minColumn)
                    minColumn = c.ColumnIndex;

                if (c.ColumnIndex > maxColumn)
                    maxColumn = c.ColumnIndex;
            }

            foreach (DataGridViewCell c in dgvWeek.SelectedCells)
            {
                if (c.ColumnIndex == minColumn)
                    if (c.RowIndex < minColRow)
                        minColRow = c.RowIndex;

                if (c.ColumnIndex == maxColumn)
                    if (c.RowIndex > maxColRow)
                        maxColRow = c.RowIndex;
            }

            FromTime = RefDate.Date;
            FromTime = FromTime.AddDays(minColumn);
            int hour = Convert.ToInt32(minColRow * 30 / 60);
            int mins = minColRow % 2 * 30;
            FromTime = FromTime.AddHours(hour);
            FromTime = FromTime.AddMinutes(mins);

            ToTime = RefDate.Date;
            ToTime = ToTime.AddDays(maxColumn);
            maxColRow++;
            hour = Convert.ToInt32(maxColRow * 30 / 60);
            mins = maxColRow % 2 * 30;
            ToTime = ToTime.AddHours(hour);
            ToTime = ToTime.AddMinutes(mins);
            log.LogMethodExit();
        }

        private void dgvDay_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                BindingSource recipePlanDetailsBS = new BindingSource();
                cbxSelect.Checked = false;
                grpPlanDetails.Visible = true;
                LoadAllRecipe();
                grpPlanDetails.Text = MessageContainerList.GetMessage(executioncontex, "Plan Details");
                Label label = sender as Label;
                if (label != null)
                {
                    grpPlanDetails.Font = new Font("Arial", 9, FontStyle.Regular);
                    planHeaderId = Convert.ToInt32(((Label)sender).Tag);
                    List<string> recipeDateList = label.Name.Split('|').ToList();
                    DateTime planDate = Convert.ToDateTime(recipeDateList[1]).Date;
                    grpPlanDetails.Text = MessageContainerList.GetMessage(executioncontex, "Plan Details for ")
                                            + planDate.ToString(utilities.getDateFormat());
                    LoadRecipePlanDetails();
                }
                else
                {
                    recipePlanDetailsBS.DataSource = new List<RecipePlanDetailsDTO>();
                    dgvPlanDetails.DataSource = recipePlanDetailsBS;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void LoadRecipePlanDetails()
        {
            log.LogMethodEntry();
            BindingSource recipePlanDetailsBS = new BindingSource();
            if (planHeaderId != -1)
            {
                RecipePlanDetailsListBL recipePlanDetailsListBL = new RecipePlanDetailsListBL(executioncontex);
                List<KeyValuePair<RecipePlanDetailsDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<RecipePlanDetailsDTO.SearchByParameters, string>>();
                searchParams.Add(new KeyValuePair<RecipePlanDetailsDTO.SearchByParameters, string>(RecipePlanDetailsDTO.SearchByParameters.RECIPE_PLAN_HEADER_ID, planHeaderId.ToString()));
                searchParams.Add(new KeyValuePair<RecipePlanDetailsDTO.SearchByParameters, string>(RecipePlanDetailsDTO.SearchByParameters.SITE_ID, executioncontex.GetSiteId().ToString()));
                searchParams.Add(new KeyValuePair<RecipePlanDetailsDTO.SearchByParameters, string>(RecipePlanDetailsDTO.SearchByParameters.IS_ACTIVE, "1"));
                List<RecipePlanDetailsDTO> planDetailsDTOList = recipePlanDetailsListBL.GetRecipePlanDetailsDTOList(searchParams);
                recipePlanDetailsDTOListOnDisplay = planDetailsDTOList;
                if (planDetailsDTOList != null && planDetailsDTOList.Any()
                    && ProductContainer.productDTOList != null && ProductContainer.productDTOList.Count > 0)
                {
                    foreach (RecipePlanDetailsDTO recipePlanDTO in planDetailsDTOList)
                    {
                        ProductDTO productDTO = ProductContainer.productDTOList.Find(x => x.ProductId == recipePlanDTO.ProductId);
                        recipePlanDTO.RecipeName = productDTO.Description;
                    }
                    recipePlanDetailsBS.DataSource = planDetailsDTOList;
                }
            }
            else
            {
                recipePlanDetailsBS.DataSource = new List<RecipePlanDetailsDTO>();
            }
            dgvPlanDetails.DataSource = recipePlanDetailsBS;
            log.LogMethodExit();
        }

        private void dgvAll_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                BindingSource recipePlanDetailsBS = new BindingSource();
                if (e.RowIndex > -1 && e.ColumnIndex > -1)
                {
                    int planHeaderId = Convert.ToInt32(dgvAll.Rows[e.RowIndex].Cells["recipePlanHeaderIdDataGridViewTextBoxColumn"].Value);
                    if (planHeaderId != -1)
                    {
                        RecipePlanDetailsListBL recipePlanDetailsListBL = new RecipePlanDetailsListBL(executioncontex);
                        List<KeyValuePair<RecipePlanDetailsDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<RecipePlanDetailsDTO.SearchByParameters, string>>();
                        searchParams.Add(new KeyValuePair<RecipePlanDetailsDTO.SearchByParameters, string>(RecipePlanDetailsDTO.SearchByParameters.RECIPE_PLAN_HEADER_ID, planHeaderId.ToString()));
                        searchParams.Add(new KeyValuePair<RecipePlanDetailsDTO.SearchByParameters, string>(RecipePlanDetailsDTO.SearchByParameters.IS_ACTIVE, "1"));
                        searchParams.Add(new KeyValuePair<RecipePlanDetailsDTO.SearchByParameters, string>(RecipePlanDetailsDTO.SearchByParameters.SITE_ID, executioncontex.GetSiteId().ToString()));
                        List<RecipePlanDetailsDTO> planDetailsDTOList = recipePlanDetailsListBL.GetRecipePlanDetailsDTOList(searchParams);
                        if (planDetailsDTOList != null && planDetailsDTOList.Any()
                            && ProductContainer.productDTOList != null && ProductContainer.productDTOList.Count > 0)
                        {
                            grpPlanDetails.Visible = true;
                            foreach (RecipePlanDetailsDTO recipePlanDTO in planDetailsDTOList)
                            {
                                ProductDTO productDTO = ProductContainer.productDTOList.Find(x => x.ProductId == recipePlanDTO.ProductId);
                                recipePlanDTO.RecipeName = productDTO.Description;
                            }
                            recipePlanDetailsBS.DataSource = planDetailsDTOList;
                        }
                    }
                    else
                    {
                        recipePlanDetailsBS.DataSource = new List<RecipePlanDetailsDTO>();
                    }
                    dgvPlanDetails.DataSource = recipePlanDetailsBS;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validate if the check box is not checked
        /// </summary>
        /// <returns></returns>
        bool ValidateSetQuantityCheckbox()
        {
            log.LogMethodEntry();
            bool isChecked = false;
            CheckBox checkBox = new CheckBox();
            foreach (DataGridViewRow dataGridRow in dgvPlanDetails.Rows)
            {
                if (dataGridRow.Cells["cbxSel"].Value != null && (bool)dataGridRow.Cells["cbxSel"].Value)
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


        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            lblMessage.Text = string.Empty;
            using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
            {
                try
                {
                    if (dgvPlanDetails.Rows.Count > 0)
                    {
                        this.Cursor = Cursors.WaitCursor;
                        bool valid = ValidateSetQuantityCheckbox();
                        if (!valid)
                        {
                            lblMessage.Text = MessageContainerList.GetMessage(executioncontex, 218); // Choose Product
                            lblMessage.ForeColor = Color.Red;
                            LoadRecipePlanDetails();
                            log.LogMethodExit();
                            return;
                        }
                        List<RecipePlanDetailsDTO> plandetailsDTOList = new List<RecipePlanDetailsDTO>();
                        foreach (DataGridViewRow dataGridRow in dgvPlanDetails.Rows)
                        {
                            if(recipePlanDetailsDTOListOnDisplay[dataGridRow.Index].FinalQty <= 0)
                            {
                                lblMessage.Text = MessageContainerList.GetMessage(executioncontex, 1773 ,
                                    recipePlanDetailsDTOListOnDisplay[dataGridRow.Index].RecipeName); // Please enter valid value for &1
                                lblMessage.ForeColor = Color.Red;
                                log.LogMethodExit();
                                return;
                            }
                            if (dataGridRow.Cells["cbxSel"].Value != null && (bool)dataGridRow.Cells["cbxSel"].Value
                                && recipePlanDetailsDTOListOnDisplay[dataGridRow.Index].IsChanged)
                            {
                                plandetailsDTOList.Add(recipePlanDetailsDTOListOnDisplay[dataGridRow.Index]);
                            }
                        }
                        if (plandetailsDTOList.Count > 0)
                        {
                            parafaitDBTrx.BeginTransaction();
                            RecipePlanDetailsListBL recipePlanDetailsListBL = new RecipePlanDetailsListBL(executioncontex, plandetailsDTOList);
                            recipePlanDetailsListBL.Save(parafaitDBTrx.SQLTrx);
                            parafaitDBTrx.EndTransaction();
                            LoadRecipePlanDetails();
                            lblMessage.Text = MessageContainerList.GetMessage(executioncontex, "Save Successful");
                        }
                        else
                        {
                            lblMessage.Text = MessageContainerList.GetMessage(executioncontex, "Nothing to Save");
                        }
                    }
                    
                }
                catch (Exception ex)
                {
                    parafaitDBTrx.RollBack();
                    log.Error(ex);
                }
                finally
                {
                    this.Cursor = Cursors.Default;
                }
            }
            log.LogMethodExit();
        }


        private void dgvPlanDetails_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            if (e.RowIndex > -1 & e.ColumnIndex > -1)
            {
                bool valid = ValidateSetQuantityCheckbox();
                if (!valid)
                {

                    lblMessage.Text = MessageContainerList.GetMessage(executioncontex, 2856); // 'Please select the product and click on Save button.
                    lblMessage.ForeColor = Color.Red;
                }
                if (dgvPlanDetails.Columns[e.ColumnIndex].Name == "incrementalQtyDataGridViewTextBoxColumn")
                {
                    decimal adjQty = Convert.ToDecimal(dgvPlanDetails.Rows[e.RowIndex].Cells["incrementalQtyDataGridViewTextBoxColumn"].Value);
                    decimal planQty = Convert.ToDecimal(dgvPlanDetails.Rows[e.RowIndex].Cells["plannedQtyDataGridViewTextBoxColumn"].Value);
                    dgvPlanDetails.Rows[e.RowIndex].Cells["finalQtyDataGridViewTextBoxColumn"].Value = planQty + adjQty;
                }
            }
            dgvPlanDetails.RefreshEdit();
            log.LogMethodExit();
        }

        /// <summary>
        /// Search Recipe's based on Recipe Name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                this.Cursor = Cursors.WaitCursor;
                List<RecipePlanDetailsDTO> searchResultDetails = new List<RecipePlanDetailsDTO>();
                BindingSource recipePlanDetailsBS = new BindingSource();
                searchResultDetails = recipePlanDetailsDTOListOnDisplay.Where(x => x.RecipeName.IndexOf(txtRecipeName.Text, StringComparison.OrdinalIgnoreCase) != -1).ToList();
                if (searchResultDetails != null && searchResultDetails.Any())
                {
                    recipePlanDetailsBS.DataSource = searchResultDetails;
                }
                else
                {
                    recipePlanDetailsBS.DataSource = new List<RecipePlanDetailsDTO>();
                }
                dgvPlanDetails.DataSource = recipePlanDetailsBS;
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

        /// <summary>
        /// Create Header CheckBox
        /// </summary>
        private void CreateHeaderCheckBox()
        {
            log.LogMethodEntry();
            cbxSelect = new CustomCheckBox();
            cbxSelect.FlatAppearance.BorderSize = 0;
            cbxSelect.ImageAlign = ContentAlignment.MiddleCenter;
            cbxSelect.FlatAppearance.MouseDownBackColor = Color.Transparent;
            cbxSelect.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            cbxSelect.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            cbxSelect.Text = string.Empty;
            cbxSelect.Font = dgvPlanDetails.Font;
            cbxSelect.Location = new Point(dgvPlanDetails.Columns["cbxSel"].HeaderCell.ContentBounds.X + 25,
                                    dgvPlanDetails.Columns["cbxSel"].HeaderCell.ContentBounds.Y + 5);
            cbxSelect.BackColor = Color.Transparent;
            cbxSelect.Size = new Size(60, 28);
            cbxSelect.Click += new EventHandler(HeaderCheckBox_Clicked);
            dgvPlanDetails.Controls.Add(cbxSelect);
            log.LogMethodExit();
        }

        // <summary>
        /// Header check Box clicked event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HeaderCheckBox_Clicked(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            CheckBox headerCheckBox = (sender as CheckBox);
            foreach (DataGridViewRow dataGridRow in dgvPlanDetails.Rows)
            {
                dataGridRow.Cells["cbxSel"].Value = headerCheckBox.Checked;
            }
            dgvPlanDetails.EndEdit();
            lblMessage.Text = string.Empty;
            log.LogMethodExit();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            cbxSelect.Checked = false;
            lblMessage.Text = string.Empty;
            using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
            {
                try
                {
                    this.Cursor = Cursors.WaitCursor;
                    if (dgvPlanDetails.Rows.Count > 0)
                    {
                        bool valid = ValidateSetQuantityCheckbox();
                        if (!valid)
                        {

                            lblMessage.Text = MessageContainerList.GetMessage(executioncontex, 218); // Choose Product
                            lblMessage.ForeColor = Color.Red;
                            log.LogMethodExit();
                            return;
                        }
                        parafaitDBTrx.BeginTransaction();
                        if (MessageBox.Show(MessageContainerList.GetMessage(executioncontex, 1121), "Recipe Manufacturing", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        { //Are you sure you want to delete this item?
                            foreach (DataGridViewRow dataGridRow in dgvPlanDetails.Rows)
                            {
                                if (dataGridRow.Cells["cbxSel"].Value != null && (bool)dataGridRow.Cells["cbxSel"].Value)
                                {
                                    recipePlanDetailsDTOListOnDisplay[dataGridRow.Index].IsActive = false;
                                }
                            }
                            RecipePlanDetailsListBL recipePlanDetailsListBL = new RecipePlanDetailsListBL(executioncontex, recipePlanDetailsDTOListOnDisplay);
                            recipePlanDetailsListBL.Save(parafaitDBTrx.SQLTrx);
                            parafaitDBTrx.EndTransaction();
                            List<KeyValuePair<RecipePlanDetailsDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<RecipePlanDetailsDTO.SearchByParameters, string>>();
                            searchParams.Add(new KeyValuePair<RecipePlanDetailsDTO.SearchByParameters, string>(RecipePlanDetailsDTO.SearchByParameters.IS_ACTIVE, "1"));
                            searchParams.Add(new KeyValuePair<RecipePlanDetailsDTO.SearchByParameters, string>(RecipePlanDetailsDTO.SearchByParameters.RECIPE_PLAN_HEADER_ID, planHeaderId.ToString()));
                            searchParams.Add(new KeyValuePair<RecipePlanDetailsDTO.SearchByParameters, string>(RecipePlanDetailsDTO.SearchByParameters.SITE_ID, executioncontex.GetSiteId().ToString()));
                            recipePlanDetailsDTOListOnDisplay = recipePlanDetailsListBL.GetRecipePlanDetailsDTOList(searchParams);
                            if (recipePlanDetailsDTOListOnDisplay != null && recipePlanDetailsDTOListOnDisplay.Count > 0
                                && ProductContainer.productDTOList != null && ProductContainer.productDTOList.Count > 0)
                            {
                                foreach (RecipePlanDetailsDTO detailsDTO in recipePlanDetailsDTOListOnDisplay)
                                {
                                    detailsDTO.RecipeName = ProductContainer.productDTOList.Find(x => x.ProductId == detailsDTO.ProductId).Description;
                                }
                                dgvPlanDetails.DataSource = recipePlanDetailsDTOListOnDisplay;
                            }
                            else
                            {
                                dgvPlanDetails.DataSource = new List<RecipePlanDetailsDTO>();
                            }
                            dgvPlanDetails.RefreshEdit();
                            lblMessage.Text = MessageContainerList.GetMessage(executioncontex, 957); //Rows deleted successfully
                            RefreshMonth();
                            RefreshDayWeek();
                        }
                    }
                }
                catch (Exception ex)
                {
                    parafaitDBTrx.RollBack();
                    log.Error(ex);
                }
                finally
                {
                    this.Cursor = Cursors.Default;
                }
            }
            log.LogMethodExit();
        }

        private void dgvPlanDetails_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                log.LogMethodEntry();
                dgvPlanDetails.EndEdit();
                dgvPlanDetails.CurrentCell.Selected = false;
                if (e.ColumnIndex == 0 && e.RowIndex >= 0)
                {
                    if (dgvPlanDetails.Rows[e.RowIndex].Cells["cbxSel"].Value != null && (bool)dgvPlanDetails.Rows[e.RowIndex].Cells["cbxSel"].Value)
                    {
                        dgvPlanDetails.Rows[e.RowIndex].Cells["cbxSel"].Value = false;
                    }
                    else
                    {
                        dgvPlanDetails.Rows[e.RowIndex].Cells["cbxSel"].Value = true;
                    }
                    lblMessage.Text = string.Empty;
                    dgvPlanDetails.RefreshEdit();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            BuildTemplate();
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates Excel to export Data
        /// </summary>
        public void BuildTemplate()
        {
            try
            {
                log.LogMethodEntry();
                this.Cursor = Cursors.WaitCursor;
                frmExportDateRangeUI frmExportDateRangeUI = new frmExportDateRangeUI(utilities);
                DialogResult statusResult = new System.Windows.Forms.DialogResult();
                statusResult = frmExportDateRangeUI.ShowDialog();
                if (statusResult == DialogResult.OK)
                {
                    UOMContainer uOMContainer = CommonFuncs.GetUOMContainer();
                    LoadAllRecipe(frmExportDateRangeUI.FromDate, frmExportDateRangeUI.ToDate);
                    List<RecipePlanHeaderDTO> recipePlanHeaderDTOs = recipePlanHeaderDTOListOnDisplay;
                    if(recipePlanHeaderDTOListOnDisplay != null && recipePlanHeaderDTOListOnDisplay.Count == 0)
                    {
                        lblMessage.Text = MessageContainerList.GetMessage(executioncontex, 2847);//No Recipe's available for the selected source range. Please Add Recipe's.
                        return;
                    }
                    List<RecipePlanDetailsExcel> recipePlanDetailsDTOList = new List<RecipePlanDetailsExcel>();
                    if (ProductContainer.productDTOList != null && ProductContainer.productDTOList.Count > 0)
                    {
                        foreach (RecipePlanHeaderDTO dto in recipePlanHeaderDTOListOnDisplay)
                        {
                            RecipePlanDetailsExcel recipePlanDetailsDTO = new RecipePlanDetailsExcel();
                            recipePlanDetailsDTO.PlanDate = dto.PlanDateTime;
                            recipePlanDetailsDTOList.Add(recipePlanDetailsDTO);
                            foreach (RecipePlanDetailsDTO detailsDTO in dto.RecipePlanDetailsDTOList)
                            {
                                RecipePlanDetailsExcel recipePlanDetailsExcel = new RecipePlanDetailsExcel();

                                recipePlanDetailsExcel.PlanDate = null;
                                recipePlanDetailsExcel.PlannedQty = detailsDTO.PlannedQty;
                                recipePlanDetailsExcel.FinalQty = detailsDTO.FinalQty;
                                recipePlanDetailsExcel.RecipeName = ProductContainer.productDTOList.Find(x => x.ProductId == detailsDTO.ProductId).Description;
                                recipePlanDetailsExcel.UOM = UOMContainer.uomDTOList.Find(x => x.UOMId == detailsDTO.UOMId).UOM;
                                recipePlanDetailsExcel.IncrementedQty = detailsDTO.IncrementalQty;
                                recipePlanDetailsDTOList.Add(recipePlanDetailsExcel);
                            }
                        }

                        DataTable dataTable = new DataTable(typeof(RecipePlanDetailsExcel).Name);

                        PropertyInfo[] Props = typeof(RecipePlanDetailsExcel).GetProperties(BindingFlags.Public | BindingFlags.Instance);

                        foreach (PropertyInfo prop in Props)
                        {
                            dataTable.Columns.Add(prop.Name);
                        }
                        foreach (RecipePlanDetailsExcel item in recipePlanDetailsDTOList)
                        {
                            var values = new object[Props.Length];
                            for (int i = 0; i < Props.Length; i++)
                            {
                                values[i] = Props[i].GetValue(item, null);
                            }
                            dataTable.Rows.Add(values);
                        }
                        utilities.ExportToExcelTabDelimited("PlanDetails", dataTable);
                        LoadAllRecipe();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void dgvPlanDetails_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                lblMessage.Text = "Error in Plan Details grid data at row " + (e.RowIndex + 1).ToString() + ", Column "
                    + dgvPlanDetails.Columns[e.ColumnIndex].DataPropertyName +
                    ": " + e.Exception.Message;
                e.Cancel = true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            dtpPlanDate.Value = utilities.getServerTime();
            lblMessage.Text = string.Empty;
            LoadAllRecipe();
            log.LogMethodExit();
        }
    }
}

