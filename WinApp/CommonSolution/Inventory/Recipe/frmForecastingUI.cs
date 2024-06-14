/********************************************************************************************
 * Project Name - Inventory
 * Description  - UI for Forecasting Data
 * 
 **************
 **Version Log
 **************
 *Version       Date          Modified By         Remarks          
 *********************************************************************************************
 *2.100.0       28-Sep-2020   Deeksha             Created for Recipe Management enhancement.
 *********************************************************************************************/
using System;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using System.Globalization;
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Communication;
using System.Collections.Generic;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Inventory.Recipe
{
    public partial class frmForecastingUI : Form
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext = ExecutionContext.GetExecutionContext();
        private Utilities utilities;
        private List<RecipeEstimationHeaderDTO> recipeEstimationHeaderDTOList = new List<RecipeEstimationHeaderDTO>();

        public frmForecastingUI(Utilities utilities)
        {
            log.LogMethodEntry(utilities);
            InitializeComponent();
            this.utilities = utilities;
            if (utilities.ParafaitEnv.IsCorporate)
            {
                executionContext.SetSiteId(utilities.ParafaitEnv.SiteId);
            }
            else
            {
                executionContext.SetSiteId(-1);
            }
            ForecastingTypePointListBL forecastingTypePointBL = new ForecastingTypePointListBL(executionContext);
            ProductContainer productContainer = new ProductContainer(executionContext);
            log.LogMethodExit();
        }

        private void frmForecastingUI_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            btnCreatePlan.Enabled = false;
            SetStyleAndDefaultValues();
            log.LogMethodExit();
        }

        private void SetStyleAndDefaultValues()
        {
            log.LogMethodEntry();
            try
            {
                utilities.setupDataGridProperties(ref dgvEstimationDetails);
                stockDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewAmountCellStyle();
                PlannedQuantity.DefaultCellStyle = utilities.gridViewAmountCellStyle();
                dgvEstimationDetails.Columns["EstimatedWeeklyQuantity"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvEstimationDetails.Columns["EstimatedMonthlyQuantity"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                stockDataGridViewTextBoxColumn.DefaultCellStyle.Format = utilities.ParafaitEnv.INVENTORY_QUANTITY_FORMAT;
                EstimatedWeeklyQuantity.DefaultCellStyle.Format = utilities.ParafaitEnv.INVENTORY_QUANTITY_FORMAT;
                EstimatedMonthlyQuantity.DefaultCellStyle.Format = utilities.ParafaitEnv.INVENTORY_QUANTITY_FORMAT;
                PlannedQuantity.DefaultCellStyle.Format = utilities.ParafaitEnv.INVENTORY_QUANTITY_FORMAT;
                ThemeUtils.SetupVisuals(this);
                utilities.setLanguage(this);
                btnSave.Text = MessageContainerList.GetMessage(executionContext, "Save");
                btnCancel.Text = MessageContainerList.GetMessage(executionContext, "Cancel");
                btnClearData.Text = MessageContainerList.GetMessage(executionContext, "Clear Data");
                btnCreatePlan.Text = MessageContainerList.GetMessage(executionContext, "Create Plan");
                btnGetData.Text = MessageContainerList.GetMessage(executionContext, "Get Data");
                lblAspirational.Text = MessageContainerList.GetMessage(executionContext, "Aspirational %:");
                lblSeasonal.Text = MessageContainerList.GetMessage(executionContext, "Seasonal %:");
                lblEventOffset.Text = MessageContainerList.GetMessage(executionContext, "Event Offset(Hrs):");
                lblHistoricalDays.Text = MessageContainerList.GetMessage(executionContext, "Historical Days:");
                cbxFinished.Text = MessageContainerList.GetMessage(executionContext, "Finished");
                cbxSemiFinished.Text = MessageContainerList.GetMessage(executionContext, "Semi Finished");
                cbxEvents.Text = MessageContainerList.GetMessage(executionContext, "Events and Promotions");
                dtpFromDate.Value = utilities.getServerTime();
                dtpToDate.Value = dtpFromDate.Value.AddDays(1);
                txtAspirationalPerc.Text = ParafaitDefaultContainerList.GetParafaitDefault<decimal>(executionContext, "ASPIRATIONAL_PERCENTAGE").ToString();
                txtSeasonalPerc.Text = ParafaitDefaultContainerList.GetParafaitDefault<decimal>(executionContext, "SEASONAL_PERCENTAGE").ToString();
                txtHistoricalDays.Text = ParafaitDefaultContainerList.GetParafaitDefault<decimal>(executionContext, "HISTORICAL_DAYS").ToString();
                txtEventOffset.Text = ParafaitDefaultContainerList.GetParafaitDefault<decimal>(executionContext, "EVENT_OFFSET").ToString();
                lblMessage.Text = MessageContainerList.GetMessage(executionContext, 2771); //Please choose the Date and click on Get Data to generate the Forecast
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

        private void GetData()
        {
            log.LogMethodEntry();

            btnGetData.Enabled = false;
            LookupValuesList serverTimeObject = new LookupValuesList(executionContext);
            string currentDatetime = serverTimeObject.GetServerDateTime().ToString("yyyy-MM-dd");
            TimeSpan ts = new TimeSpan(6, 00, 0);
            currentDatetime = currentDatetime + " " + ts + " " + "AM";
            DateTime date = Convert.ToDateTime(currentDatetime);
            RecipeEstimationHeaderListBL recipeEstimationHeaderListBL = new RecipeEstimationHeaderListBL(executionContext);
            List<KeyValuePair<RecipeEstimationHeaderDTO.SearchByParameters, string>> searchEstimationParameter = new List<KeyValuePair<RecipeEstimationHeaderDTO.SearchByParameters, string>>();
            searchEstimationParameter.Add(new KeyValuePair<RecipeEstimationHeaderDTO.SearchByParameters, string>(RecipeEstimationHeaderDTO.SearchByParameters.CURRENT_FROM_DATE, date.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
            searchEstimationParameter.Add(new KeyValuePair<RecipeEstimationHeaderDTO.SearchByParameters, string>(RecipeEstimationHeaderDTO.SearchByParameters.CURRENT_TO_DATE, date.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
            searchEstimationParameter.Add(new KeyValuePair<RecipeEstimationHeaderDTO.SearchByParameters, string>(RecipeEstimationHeaderDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            recipeEstimationHeaderDTOList = recipeEstimationHeaderListBL.GetAllRecipeEstimationHeaderDTOList(searchEstimationParameter, true, true);
            if (recipeEstimationHeaderDTOList == null || recipeEstimationHeaderDTOList.Any() == false)
            {
                if (MessageBox.Show(MessageContainerList.GetMessage(executionContext, 2806), "Recipe Forecasting", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    BuildHistoricalData(date);
                }
                else
                {
                    lblMessage.Text = MessageContainerList.GetMessage(executionContext, 2771); //Please choose the Date and click on Get Data to generate the Forecast
                    btnGetData.Enabled = true;
                    return;
                }
            }
            else
            {
                Application.DoEvents();
                ApplicationContext ap = new ApplicationContext();
                System.Threading.ThreadStart thr = delegate
                {
                    Form f = SetFormStyle();
                    try
                    {
                        ap.MainForm = f;
                        Application.Run(ap);
                    }
                    catch { }
                };
                System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(thr));
                thread.Start();
                try
                {
                    GetForecastForGivenDateRange(date);
                }
                finally
                {
                    ap.ExitThread();
                }
            }
            if (recipeEstimationHeaderDTOList.Exists(x => x.RecipeEstimationHeaderId <= -1))
            {
                btnCreatePlan.Enabled = false;
                btnSave.Enabled = true;
                btnDelete.Enabled = true;
            }
            else
            {
                btnSave.Enabled = false;
                btnDelete.Enabled = false;
                btnCreatePlan.Enabled = true;
            }
            log.LogMethodExit();
        }

        private void GetForecastForGivenDateRange(DateTime date)
        {
            log.LogMethodEntry(date);
            using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
            {
                try
                {
                    this.Cursor = Cursors.WaitCursor;
                    parafaitDBTrx.BeginTransaction();
                    RecipeEstimationHeaderListBL recipeEstimationHeaderListBL = new RecipeEstimationHeaderListBL(executionContext);
                    DateTime fromdate = dtpFromDate.Value.Date;
                    DateTime todate = dtpToDate.Value.Date;
                    List<KeyValuePair<RecipeEstimationHeaderDTO.SearchByParameters, string>> searchEstimationParameter = new List<KeyValuePair<RecipeEstimationHeaderDTO.SearchByParameters, string>>();
                    searchEstimationParameter.Add(new KeyValuePair<RecipeEstimationHeaderDTO.SearchByParameters, string>(RecipeEstimationHeaderDTO.SearchByParameters.FROM_DATE, fromdate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                    searchEstimationParameter.Add(new KeyValuePair<RecipeEstimationHeaderDTO.SearchByParameters, string>(RecipeEstimationHeaderDTO.SearchByParameters.TO_DATE, todate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                    searchEstimationParameter.Add(new KeyValuePair<RecipeEstimationHeaderDTO.SearchByParameters, string>(RecipeEstimationHeaderDTO.SearchByParameters.DATE_NOT_IN_JOB_DATA, date.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                    searchEstimationParameter.Add(new KeyValuePair<RecipeEstimationHeaderDTO.SearchByParameters, string>(RecipeEstimationHeaderDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                    recipeEstimationHeaderDTOList = recipeEstimationHeaderListBL.GetAllRecipeEstimationHeaderDTOList(searchEstimationParameter, true, true);
                    if (recipeEstimationHeaderDTOList.Exists(x => x.FromDate.Hour == 6 & x.FromDate.Minute == 0))
                    {
                        recipeEstimationHeaderDTOList = recipeEstimationHeaderDTOList.FindAll(x => x.FromDate.Hour != 6).ToList();
                    }
                    if(recipeEstimationHeaderDTOList != null && recipeEstimationHeaderDTOList.Count > 0)
                    {
                        lblMessage.Text = MessageContainerList.GetMessage(executionContext, 2879);
                        lblMessage.ForeColor = Color.Black;
                        //Displaying Saved forecast information. Please click on clear & Get Data to regenerate the forecast
                    }
                    else
                    {
                        lblMessage.Text = string.Empty;
                        RecipeEstimationHeaderDTO recipeEstimationHeaderDTO = new RecipeEstimationHeaderDTO(-1, fromdate, todate,
                                                                                                             txtAspirationalPerc.Text == string.Empty ? 0 : Convert.ToDecimal(txtAspirationalPerc.Text),
                                                                                                             txtSeasonalPerc.Text == string.Empty ? 0 : Convert.ToDecimal(txtSeasonalPerc.Text),
                                                                                                             cbxEvents.Checked, txtHistoricalDays.Text == string.Empty ? 0 : Convert.ToInt32(txtHistoricalDays.Text),
                                                                                                             txtEventOffset.Text == string.Empty ? 0 : Convert.ToInt32(txtEventOffset.Text), cbxFinished.Checked,
                                                                                                             cbxSemiFinished.Checked, true);
                        RecipeEstimationHeaderBL recipeEstimationHeaderBL = new RecipeEstimationHeaderBL(executionContext, recipeEstimationHeaderDTO);
                        recipeEstimationHeaderDTOList = recipeEstimationHeaderBL.BuildForecastData(parafaitDBTrx.SQLTrx);
                    }
                    if (recipeEstimationHeaderDTOList != null && recipeEstimationHeaderDTOList.Count > 0)
                    {
                        ForecastingSummaryListBL forecastingSummaryBL = new ForecastingSummaryListBL(executionContext, recipeEstimationHeaderDTOList);
                        List<ForecastingSummaryDTO> forecastingSummaryDTOList = forecastingSummaryBL.GetForecastingSummary(fromdate, todate);
                        if (forecastingSummaryDTOList != null && forecastingSummaryDTOList.Any())
                        {
                            forecastingSummaryDTOBindingSource.DataSource = new List<ForecastingSummaryDTO>(forecastingSummaryDTOList);
                        }
                        else
                        {
                            forecastingSummaryDTOBindingSource.DataSource = new List<ForecastingSummaryDTO>();
                        }
                    }
                    else
                    {
                        forecastingSummaryDTOBindingSource.DataSource = new List<ForecastingSummaryDTO>();
                    }
                    parafaitDBTrx.EndTransaction();
                    dgvEstimationDetails.RefreshEdit();
                }
                catch (Exception ex)
                {
                    parafaitDBTrx.RollBack();
                    log.Error(ex);
                }
                finally
                {
                    this.Cursor = Cursors.Default;
                    btnGetData.Enabled = true;
                }
                log.LogMethodExit();
            }
        }

        private void btnGetData_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                bool valid = ValidateForecast();
                recipeEstimationHeaderDTOList = new List<RecipeEstimationHeaderDTO>();
                if (valid)
                {
                    lblMessage.Text = MessageContainerList.GetMessage(executionContext, "Please Wait...");
                    if (recipeEstimationHeaderDTOList.Exists(x => x.RecipeEstimationHeaderId <= -1))
                    {
                        forecastingSummaryDTOBindingSource.DataSource = null;
                        dgvEstimationDetails.RefreshEdit();
                    }
                    GetData();
                    ValidateSavedForecastData();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void ValidateSavedForecastData()
        {
            log.LogMethodEntry();
            int datediff = Convert.ToInt32(Math.Round((dtpToDate.Value.Date - dtpFromDate.Value.Date).TotalDays));
            if (datediff == 0)
            {
                datediff = 1;
            }
            datediff++;
            DateTime toDate = dtpToDate.Value.Date;
            if (recipeEstimationHeaderDTOList.Exists(x => x.RecipeEstimationHeaderId > -1)
                && recipeEstimationHeaderDTOList[recipeEstimationHeaderDTOList.Count - 1].ToDate.Date < dtpToDate.Value.Date)
            {
                lblMessage.Text = MessageContainerList.GetMessage(executionContext, 2804, dtpFromDate.Value.ToString(utilities.getDateFormat()),
                        recipeEstimationHeaderDTOList[recipeEstimationHeaderDTOList.Count - 1].ToDate.ToString(utilities.getDateFormat()));
                //Displaying Forecast from &1 to &2. Please click on Clear Data to regenerate the Forecast information
                dtpToDate.Value = recipeEstimationHeaderDTOList[recipeEstimationHeaderDTOList.Count - 1].ToDate;
            }
            log.LogMethodExit();
        }

        private void BuildHistoricalData(DateTime date)
        {
            log.LogMethodEntry(date);
            Application.DoEvents();
            ApplicationContext ap = new ApplicationContext();
            System.Threading.ThreadStart thr = delegate
            {
                Form f = SetFormStyle();
                try
                {
                    ap.MainForm = f;
                    Application.Run(ap);
                }
                catch { }
            };
            System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(thr));
            thread.Start();
            using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
            {
                try
                {
                    parafaitDBTrx.BeginTransaction();
                    RecipeEstimationHeaderDTO recipeEstimationHeaderDTO = new RecipeEstimationHeaderDTO(-1, date, date,
                                                                                        0, 0, false, 365,
                                                                                       -1, null, null, true);
                    RecipeEstimationHeaderBL recipeEstimationHeaderBL = new RecipeEstimationHeaderBL(executionContext, recipeEstimationHeaderDTO);
                    recipeEstimationHeaderBL.BuildForecastData(parafaitDBTrx.SQLTrx);
                    parafaitDBTrx.EndTransaction();
                    GetForecastForGivenDateRange(date);
                }
                catch (Exception ex)
                {
                    parafaitDBTrx.RollBack();
                    log.Error(ex);
                }

                finally
                {
                    btnGetData.Enabled = true;
                    ap.ExitThread();
                }
            }
            log.LogMethodExit();
        }

        private static Form SetFormStyle()
        {
            log.LogMethodEntry();
            Form f = new Form();
            System.Windows.Forms.Button btnWait;
            btnWait = new System.Windows.Forms.Button();
            btnWait.BackColor = System.Drawing.Color.Transparent;
            btnWait.BackgroundImage = Properties.Resources.pressed1;
            btnWait.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            btnWait.Dock = System.Windows.Forms.DockStyle.Fill;
            btnWait.FlatAppearance.BorderSize = 0;
            btnWait.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            btnWait.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            btnWait.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnWait.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            btnWait.ForeColor = System.Drawing.Color.White;
            btnWait.Image = Properties.Resources.PreLoader;
            btnWait.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            btnWait.Location = new System.Drawing.Point(0, 0);
            btnWait.Name = "btnWait";
            btnWait.Size = new System.Drawing.Size(346, 89);
            btnWait.TabIndex = 2;
            btnWait.Text = Semnox.Parafait.Inventory.CommonFuncs.Utilities.MessageUtils.getMessage(2807); //Please wait Generating Forecasting data
            btnWait.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            btnWait.UseVisualStyleBackColor = false;
            // 
            // Form
            // 
            f.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            f.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            f.BackColor = System.Drawing.Color.White;
            f.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            f.ClientSize = new System.Drawing.Size(346, 89);
            f.Controls.Add(btnWait);
            f.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            f.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            f.TransparencyKey = System.Drawing.Color.White;

            f.ShowInTaskbar = false;
            f.TopLevel = f.TopMost = true;
            log.LogMethodExit(f);
            return f;
        }

        private bool ValidateForecast()
        {
            log.LogMethodEntry();
            bool valid = true;
            if (dtpToDate.Value.Date < dtpFromDate.Value.Date)
            {
                lblMessage.Text = MessageContainerList.GetMessage(executionContext, 724); //To Date should be greater than From Date
                lblMessage.ForeColor = Color.Red;
                log.LogMethodExit(false);
                return false;
            }
            if ((dtpToDate.Value.Date - dtpFromDate.Value.Date).TotalDays > 30)
            {
                dtpToDate.Value = dtpFromDate.Value.Date.AddDays(30);
                MessageBox.Show(MessageContainerList.GetMessage(executionContext, 2789)); //Date range should not be more than 30 Days
                log.LogMethodExit(false);
                return false;
            }
            int historicalDataInDays = 0;
            if (!string.IsNullOrEmpty(txtHistoricalDays.Text))
            {
                historicalDataInDays = Convert.ToInt32(txtHistoricalDays.Text);
            }
            if(historicalDataInDays < 0 || historicalDataInDays > 365)
            {
                MessageBox.Show(MessageContainerList.GetMessage(executionContext, 2871)); //Historical Days cannot be less than a year.
                log.LogMethodExit(false);
                return false;
            }
            try
            {
                decimal aspirationalPerc = txtAspirationalPerc.Text == string.Empty ? 0 : Convert.ToDecimal(txtAspirationalPerc.Text);
                decimal seasonalnalPerc = txtSeasonalPerc.Text == string.Empty ? 0 : Convert.ToDecimal(txtSeasonalPerc.Text);
                if (aspirationalPerc < -100 || seasonalnalPerc > 100
                    || seasonalnalPerc < -100 || aspirationalPerc > 100)
                {
                    MessageBox.Show(MessageContainerList.GetMessage(executionContext, 2855)); //Percentage Range should be between 100%. Please enter valid percentage
                    log.LogMethodExit(false);
                    return false;
                }
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit(valid);
            return valid;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
            {
                try
                {
                    if (recipeEstimationHeaderDTOList != null && recipeEstimationHeaderDTOList.Count > 0)
                    {
                        this.Cursor = Cursors.WaitCursor;
                        parafaitDBTrx.BeginTransaction();
                        RecipeEstimationHeaderListBL recipeEstimationHeaderListBL = new RecipeEstimationHeaderListBL(executionContext, recipeEstimationHeaderDTOList);
                        recipeEstimationHeaderListBL.Save(parafaitDBTrx.SQLTrx);
                        lblMessage.Text = MessageContainerList.GetMessage(executionContext, 122); // Save Successful
                        parafaitDBTrx.EndTransaction();
                        btnCreatePlan.Enabled = true;
                        btnSave.Enabled = false;
                        btnDelete.Enabled = false;
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

        private void dgvEstimationDetails_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                lblMessage.Text = string.Empty;
                if (e.ColumnIndex > -1 && e.RowIndex > -1)
                {
                    if (dgvEstimationDetails.Columns[e.ColumnIndex].Name == "cbxDel")
                    {
                        DataGridViewCheckBoxCell checkBox = dgvEstimationDetails.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewCheckBoxCell;
                        if (Convert.ToBoolean(checkBox.Value))
                        {
                            checkBox.Value = false;
                        }
                        else
                        {
                            checkBox.Value = true;
                        }
                    }
                    if (dgvEstimationDetails.Columns[e.ColumnIndex].Name == "RecipeName")
                    {
                        if (Convert.ToInt32(dgvEstimationDetails["ProductId", dgvEstimationDetails.CurrentRow.Index].Value) != -1
                        && dgvEstimationDetails["ProductId", dgvEstimationDetails.CurrentRow.Index].Value != null)
                        {
                            using (frmBOM frmBOM = new frmBOM(Convert.ToInt32(dgvEstimationDetails["ProductId", dgvEstimationDetails.CurrentRow.Index].Value), utilities, false))
                            {
                                CommonUIDisplay.setupVisuals(frmBOM);
                                frmBOM.ShowDialog();
                            }
                        }
                    }
                    if (dgvEstimationDetails.Columns[e.ColumnIndex].Name == "DailyEstimate")
                    {
                        if (Convert.ToInt32(dgvEstimationDetails["ProductId", dgvEstimationDetails.CurrentRow.Index].Value) != -1
                        && dgvEstimationDetails["ProductId", dgvEstimationDetails.CurrentRow.Index].Value != null)
                        {
                            using (frmRecipeDailyEstimationDetails frmEstDetails = new frmRecipeDailyEstimationDetails(utilities, recipeEstimationHeaderDTOList, Convert.ToInt32(dgvEstimationDetails["ProductId", dgvEstimationDetails.CurrentRow.Index].Value)))
                            {
                                CommonUIDisplay.setupVisuals(frmEstDetails);
                                frmEstDetails.ShowDialog();
                                frmEstDetails.StartPosition = FormStartPosition.CenterParent;
                            }
                        }
                    }
                }
                ValidateSavedForecastData();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void btnClearData_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                lblMessage.Text = string.Empty;
                this.Cursor = Cursors.WaitCursor;
                ClearData();
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }

        private void ClearData()
        {
            if (recipeEstimationHeaderDTOList != null && recipeEstimationHeaderDTOList.Exists(x => x.RecipeEstimationHeaderId <= -1))
            {
                forecastingSummaryDTOBindingSource.DataSource = null;
                dgvEstimationDetails.RefreshEdit();
                recipeEstimationHeaderDTOList = new List<RecipeEstimationHeaderDTO>();
            }
            else if (recipeEstimationHeaderDTOList != null && recipeEstimationHeaderDTOList.Count > 0 && MessageBox.Show(MessageContainerList.GetMessage(executionContext, 2808), MessageContainerList.GetMessage(executionContext, "Recipe Forecasting"),
                        MessageBoxButtons.YesNo) == DialogResult.Yes)
            { // Existing data will be purged and forecasting engine will have to be Re-run to generate the Data.
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    try
                    {
                        parafaitDBTrx.BeginTransaction();
                        RecipeEstimationHeaderListBL recipeEstimationHeaderListBL = new RecipeEstimationHeaderListBL(executionContext, recipeEstimationHeaderDTOList);
                        recipeEstimationHeaderListBL.Delete(parafaitDBTrx.SQLTrx);
                        parafaitDBTrx.EndTransaction();
                        recipeEstimationHeaderDTOList = new List<RecipeEstimationHeaderDTO>();
                        forecastingSummaryDTOBindingSource.DataSource = null;
                        dgvEstimationDetails.RefreshEdit();
                    }
                    catch (Exception ex)
                    {
                        lblMessage.Text = ex.Message;
                        parafaitDBTrx.RollBack();
                        log.Error(ex);
                    }
                }
            }
        }

        private void btnCreatePlan_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
            {
                try
                {
                    if (recipeEstimationHeaderDTOList.Count > 0)
                    {
                        this.Cursor = Cursors.WaitCursor;
                        parafaitDBTrx.BeginTransaction();
                        List<RecipePlanHeaderDTO> recipePlanHeaderList = new List<RecipePlanHeaderDTO>();
                        RecipePlanHeaderListBL recipePlanHeaderListBL = null;
                        foreach (RecipeEstimationHeaderDTO headerDTO in recipeEstimationHeaderDTOList)
                        {
                            if (headerDTO.RecipeEstimationDetailsDTOList.Count > 0)
                            {
                                RecipePlanHeaderDTO recipePlanHeaderDTO = null;
                                recipePlanHeaderListBL = new RecipePlanHeaderListBL(executionContext);
                                List<KeyValuePair<RecipePlanHeaderDTO.SearchByParameters, string>> searchPlanParams = new List<KeyValuePair<RecipePlanHeaderDTO.SearchByParameters, string>>();
                                searchPlanParams.Add(new KeyValuePair<RecipePlanHeaderDTO.SearchByParameters, string>(RecipePlanHeaderDTO.SearchByParameters.PLAN_FROM_DATE, headerDTO.FromDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)));
                                searchPlanParams.Add(new KeyValuePair<RecipePlanHeaderDTO.SearchByParameters, string>(RecipePlanHeaderDTO.SearchByParameters.PLAN_TO_DATE, headerDTO.ToDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)));
                                List<RecipePlanHeaderDTO> recipePlanHeaderDTOList = recipePlanHeaderListBL.GetAllRecipePlanHeaderDTOList(searchPlanParams, true);
                                if (recipePlanHeaderDTOList != null && recipePlanHeaderDTOList.Any())
                                {
                                    recipePlanHeaderDTO = recipePlanHeaderDTOList[0];
                                    recipePlanHeaderDTO.RecipeEstimationHeaderId = headerDTO.RecipeEstimationHeaderId;
                                }
                                else
                                {
                                    recipePlanHeaderDTO = new RecipePlanHeaderDTO(-1, headerDTO.FromDate, null, null, null, null, headerDTO.RecipeEstimationHeaderId,
                                                                                    null, null, null, null, null, null, null, true);
                                }
                                foreach (RecipeEstimationDetailsDTO estimationDetailsDTO in headerDTO.RecipeEstimationDetailsDTOList)
                                {
                                    if (recipePlanHeaderDTO.RecipePlanDetailsDTOList.Exists(x => x.ProductId == estimationDetailsDTO.ProductId))
                                    {
                                        recipePlanHeaderDTO.RecipePlanDetailsDTOList.Find(x => x.ProductId == estimationDetailsDTO.ProductId).PlannedQty = estimationDetailsDTO.TotalEstimatedQty;
                                        recipePlanHeaderDTO.RecipePlanDetailsDTOList.Find(x => x.ProductId == estimationDetailsDTO.ProductId).QtyModifiedDate = utilities.getServerTime();
                                    }
                                    else
                                    {
                                        RecipePlanDetailsDTO recipePlanDetailsDTO = new RecipePlanDetailsDTO(-1, -1, estimationDetailsDTO.ProductId, estimationDetailsDTO.TotalEstimatedQty, null, estimationDetailsDTO.TotalEstimatedQty,
                                            estimationDetailsDTO.UOMId, estimationDetailsDTO.RecipeEstimationDetailId, null, true);
                                        recipePlanHeaderDTO.RecipePlanDetailsDTOList.Add(recipePlanDetailsDTO);
                                    }
                                }
                                recipePlanHeaderList.Add(recipePlanHeaderDTO);
                            }
                        }
                        if (recipePlanHeaderList.Count > 0)
                        {
                            recipePlanHeaderListBL = new RecipePlanHeaderListBL(executionContext, recipePlanHeaderList);
                            recipePlanHeaderListBL.Save(parafaitDBTrx.SQLTrx);
                            lblMessage.Text = MessageContainerList.GetMessage(executionContext, 2842); // Plan Created Successfully"
                            parafaitDBTrx.EndTransaction();
                        }
                    }
                }
                catch (Exception ex)
                {
                    parafaitDBTrx.RollBack();
                    log.Error(ex);
                    lblMessage.Text = ex.Message;
                }
                finally
                {
                    this.Cursor = Cursors.Default;
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Method which deletes selected row and the estimation details record
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                bool valid = ValidateSetQuantityCheckbox();
                if (!valid && recipeEstimationHeaderDTOList != null && recipeEstimationHeaderDTOList.Count > 0)
                {
                    MessageBox.Show(MessageContainerList.GetMessage(executionContext, 218), MessageContainerList.GetMessage(executionContext,
                                    MessageContainerList.GetMessage(executionContext, "Validation Error")));//Choose Product 
                    log.LogMethodExit();
                    return;
                }
                if (recipeEstimationHeaderDTOList != null && recipeEstimationHeaderDTOList.Count > 0 &&
                    MessageBox.Show(MessageContainerList.GetMessage(executionContext, 889), //Are you sure you want to delete this product?
                                    MessageContainerList.GetMessage(executionContext, "Recipe Forecasting"),
                           MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    foreach (DataGridViewRow dataGridRow in dgvEstimationDetails.Rows)
                    {
                        if (dataGridRow.Cells["cbxDel"].Value != null && (bool)dataGridRow.Cells["cbxDel"].Value)
                        {
                            BeginInvoke((MethodInvoker)delegate
                            {
                                RemoveDGVRow(dataGridRow.Index);
                            });
                            int productId = Convert.ToInt32(dgvEstimationDetails.Rows[dataGridRow.Index].Cells["productId"].Value);
                            foreach (RecipeEstimationHeaderDTO headerDTO in recipeEstimationHeaderDTOList)
                            {
                                RecipeEstimationDetailsDTO dtoList = headerDTO.RecipeEstimationDetailsDTOList.Find(x => x.ProductId == productId);
                                headerDTO.RecipeEstimationDetailsDTOList.Remove(dtoList);
                            }
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
        /// Method to remove Row at specified index
        /// </summary>
        private void RemoveDGVRow(int index)
        {
            log.LogMethodEntry();
            try
            {
                if (index > -1)
                {
                    dgvEstimationDetails.Rows.RemoveAt(index);
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
            foreach (DataGridViewRow dataGridRow in dgvEstimationDetails.Rows)
            {
                if (dataGridRow.Cells["cbxDel"].Value != null && (bool)dataGridRow.Cells["cbxDel"].Value)
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

        /// <summary>
        /// Validate UI fields
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPerc_TextChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            lblMessage.Text = string.Empty;
            txtSeasonalPerc.BackColor = Color.White;
            txtAspirationalPerc.BackColor = Color.White;
            txtHistoricalDays.BackColor = Color.White;
            txtEventOffset.BackColor = Color.White;
            try
            {
                decimal aspirationalPerc = txtAspirationalPerc.Text == string.Empty ? 0 : Convert.ToDecimal(txtAspirationalPerc.Text);
                if (aspirationalPerc < -100 || aspirationalPerc > 100)
                {
                    lblMessage.Text = MessageContainerList.GetMessage(executionContext, 2855);
                    txtAspirationalPerc.BackColor = Color.Red;
                    //Percentage Range should be between +100% and -100%. Please enter valid percentage
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = MessageContainerList.GetMessage(executionContext, 1144, "Aspirational Percentage"); // Please enter valid value for &1.
                txtAspirationalPerc.BackColor = Color.Red;
                log.Error(ex);
            }
            try
            {
                decimal seasonalnalPerc = txtSeasonalPerc.Text == string.Empty ? 0 : Convert.ToDecimal(txtSeasonalPerc.Text);
                if (seasonalnalPerc < -100 || seasonalnalPerc > 100)
                {
                    lblMessage.Text = MessageContainerList.GetMessage(executionContext, 2855);
                    txtSeasonalPerc.BackColor = Color.Red;
                    //Percentage Range should be between 100%. Please enter valid percentage
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = MessageContainerList.GetMessage(executionContext, 1144, "Seasonal Percentage"); // Please enter valid value for &1.
                txtSeasonalPerc.BackColor = Color.Red;
                log.Error(ex);
            }
            try
            {
                decimal historicalDays = txtHistoricalDays.Text == string.Empty ? 0 : Convert.ToDecimal(txtHistoricalDays.Text);
                if(historicalDays < 0)
                {
                    lblMessage.Text = MessageContainerList.GetMessage(executionContext, 1144, "Historical Days"); // Please enter valid value for &1
                    txtHistoricalDays.BackColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = MessageContainerList.GetMessage(executionContext, 1144 , "Historical Days"); // Please enter valid value for &1
                txtHistoricalDays.BackColor = Color.Red;
                log.Error(ex);
            }
            try
            {
                decimal eventOffset = txtEventOffset.Text == string.Empty ? 0 : Convert.ToDecimal(txtEventOffset.Text);
                if(eventOffset < 0)
                {
                    lblMessage.Text = MessageContainerList.GetMessage(executionContext, 1144, "Event Offset"); // Please enter valid value for &1
                    txtEventOffset.BackColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = MessageContainerList.GetMessage(executionContext, 1144 , "Event Offset"); // Please enter valid value for &1
                txtEventOffset.BackColor = Color.Red;
                log.Error(ex);
            }
            lblMessage.ForeColor = Color.Red;
            log.LogMethodExit();
        }
    }
}
