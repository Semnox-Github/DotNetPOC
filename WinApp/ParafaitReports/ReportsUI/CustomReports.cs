/********************************************************************************************
*Project Name -                                                                           
*Description  -
*************
**Version Log
*************
*Version     Date                   Modified By                 Remarks          
*********************************************************************************************
*.00        09-Dec-2016            Soumya                      Updated design and queries to
*                                                             include ShowGrandTotal flag.
*2.80       23-Aug-2019            Jinto Thomas                 Added logger into methods                                                             
*2.80       18-Sep-2019            Dakshakh raj                Modified : Added logs       
*2.80       15-Jun-2020            Laster Menezes              Added new checkbox control for Repeat BreakColumn
*2.130      01-Jul-2021            Laster Menezes              handling IsActive field of the reports while creating and editing custom reports
*********************************************************************************************/
using System;
using Semnox.Core.Utilities;
using Semnox.Parafait.Reports;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Semnox.Parafait.Report.Reports
{

    /// <summary>
    ///  CustomReports class
    /// </summary>
    public partial class CustomReports : Form
    {
        int reportId;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        private ReportsDTO ReportsDTO;

        /// <summary>
        /// CustomReports with params
        /// </summary>
        public CustomReports(int P_ReportId)
        {
            log.LogMethodEntry(P_ReportId);
            try
            {
                InitializeComponent();
                reportId = P_ReportId;
                Common.setupVisuals(this);
                if (Common.ParafaitEnv.IsCorporate)
                {
                    machineUserContext.SetSiteId(Common.ParafaitEnv.SiteId);
                }
                else
                {
                    machineUserContext.SetSiteId(-1);
                }
                Common.Utilities.setLanguage(this);
                machineUserContext.SetUserId(Common.ParafaitEnv.LoginID);                
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// CustomReports_Load
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void CustomReports_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (reportId != -1)
                {
                    ReportsList reportsList = new ReportsList();
                    List<KeyValuePair<ReportsDTO.SearchByReportsParameters, string>> reportsSearchParams = new List<KeyValuePair<ReportsDTO.SearchByReportsParameters, string>>();
                    reportsSearchParams.Add(new KeyValuePair<ReportsDTO.SearchByReportsParameters, string>(ReportsDTO.SearchByReportsParameters.REPORT_ID, reportId.ToString()));
                    List<ReportsDTO> reportsListOnDisplay = reportsList.GetAllReports(reportsSearchParams);
                    this.ReportsDTO = reportsListOnDisplay[0];
                    if (reportsListOnDisplay != null)
                    {
                        txtReportName.Text = reportsListOnDisplay[0].ReportName;
                        txtReportGroup.Text = reportsListOnDisplay[0].ReportGroup;
                        txtDBQuery.Text = reportsListOnDisplay[0].DBQuery;
                        cmbOutputFormat.SelectedItem = reportsListOnDisplay[0].DisplayOutputFormat;
                        if (reportsListOnDisplay[0].BreakColumn != "")
                            txtBreakColumn.Text = reportsListOnDisplay[0].BreakColumn.ToString();
                        else
                            txtBreakColumn.Text = "";
                        if (reportsListOnDisplay[0].HideBreakColumn == "Y")
                            chkHideBreakColumn.Checked = true;
                        else
                            chkHideBreakColumn.Checked = false;

                        if (reportsListOnDisplay[0].ShowGrandTotal == "Y")
                            chkShowGrandTotal.Checked = true;
                        else
                            chkShowGrandTotal.Checked = false;

                        if (reportsListOnDisplay[0].HideGridLines == "Y")
                            chkHideGridLines.Checked = true;
                        else
                            chkHideGridLines.Checked = false;

                        if (reportsListOnDisplay[0].PrintContinuous == "Y")
                            chkPrintContinuous.Checked = true;
                        else
                            chkPrintContinuous.Checked = false;

                        if (reportsListOnDisplay[0].RepeatBreakColumns == "Y")
                            chkRepeatBreakColumns.Checked = true;
                        else
                            chkRepeatBreakColumns.Checked = false;

                        txtAggregateColumns.Text = reportsListOnDisplay[0].AggregateColumns;
                        if(reportsListOnDisplay[0].MaxDateRange == -1)
                        {
                            txtMaxDateRange.Text = string.Empty;
                        }
                        else
                        {
                            txtMaxDateRange.Text = reportsListOnDisplay[0].MaxDateRange.ToString();
                        }                        
                    }
                }
                else
                {
                    cmbOutputFormat.SelectedIndex = 0;
                    chkHideBreakColumn.Checked = false;
                }
                txtReportName.Focus();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// btnSave_Click
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (txtReportName.Text.Trim() == "")
                {                    
                    MessageBox.Show(Common.MessageUtils.getMessage(766), Common.MessageUtils.getMessage(12641));
                    return;
                }

                if (cmbOutputFormat.SelectedItem == null)
                {
                    MessageBox.Show(Common.MessageUtils.getMessage(1320), Common.MessageUtils.getMessage(12641));
                    return;
                }

                ReportsDTO reportsDTO = new ReportsDTO();
                if (reportId == -1)
                {
                    reportsDTO.ReportId = -1;
                    reportsDTO.IsActive = true;
                }
                else
                {
                    //handles the unmodified field in the report UI
                    reportsDTO = this.ReportsDTO;
                    reportsDTO.ReportId = reportId;
                }
                reportsDTO.ReportName = txtReportName.Text;
                reportsDTO.ReportGroup = txtReportGroup.Text;
                reportsDTO.ReportKey = txtReportName.Text.Replace(" ", "");
                reportsDTO.DBQuery = txtDBQuery.Text;
                reportsDTO.OutputFormat = cmbOutputFormat.SelectedItem.ToString()[0].ToString();
                if (txtBreakColumn.Text.Trim() == "")
                    reportsDTO.BreakColumn = "";
                else
                    reportsDTO.BreakColumn = (txtBreakColumn.Text);

                if (chkHideBreakColumn.Checked)
                    reportsDTO.HideBreakColumn = "Y";
                else
                    reportsDTO.HideBreakColumn = "N";

                if (chkHideGridLines.Checked)
                    reportsDTO.HideGridLines = "Y";
                else
                    reportsDTO.HideGridLines = "N";

                if (chkShowGrandTotal.Checked)
                {
                    reportsDTO.ShowGrandTotal = "Y";
                }
                else
                {
                    reportsDTO.ShowGrandTotal = "N";
                    chkShowGrandTotal.Checked = false;
                }
                reportsDTO.CustomFlag = "Y";
                reportsDTO.AggregateColumns = txtAggregateColumns.Text;

                if (chkPrintContinuous.Checked)
                    reportsDTO.PrintContinuous = "Y";
                else
                    reportsDTO.PrintContinuous = "N";

                if (chkRepeatBreakColumns.Checked)
                    reportsDTO.RepeatBreakColumns = "Y";
                else
                    reportsDTO.RepeatBreakColumns = "N";

                if(!string.IsNullOrEmpty(txtMaxDateRange.Text.Trim()))
                {
                    reportsDTO.MaxDateRange = Convert.ToInt32(txtMaxDateRange.Text);
                }
                else
                {
                    reportsDTO.MaxDateRange = -1;
                }
                
                reportsDTO.SiteId = machineUserContext.GetSiteId();
                if (reportId == -1)
                {
                    Semnox.Parafait.Reports.Reports reports = new Semnox.Parafait.Reports.Reports(reportsDTO);
                    reports.Save();
                }
                else
                {
                    Semnox.Parafait.Reports.Reports reports = new Semnox.Parafait.Reports.Reports(reportsDTO);
                    reports.Save();
                }                
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// btnCancel_Click
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.Close();
            log.LogMethodExit();
        }

        /// <summary>
        /// btnDelete_Click
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (reportId == -1)
            {
                log.LogMethodExit();
                return;
            }

            if (MessageBox.Show(Common.MessageUtils.getMessage(1322), Common.MessageUtils.getMessage(12078), MessageBoxButtons.YesNo) == DialogResult.No)
            {
                log.LogMethodExit();
                return;
            }

            ReportsList reportsList = new ReportsList();
            ReportsDTO reportsDTO = reportsList.GetReports(reportId);

            if (reportsDTO != null)
            {
                try
                {
                    reportsList = new ReportsList();
                    reportsList.DeleteReport(reportsDTO);
                    log.LogMethodExit();
                    this.Close();
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    MessageBox.Show(Common.MessageUtils.getMessage(1321), Common.MessageUtils.getMessage(12641));
                }
            }
        }

        /// <summary>
        /// btnPreview_Click
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void btnPreview_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                btnSave.PerformClick();
                string timestamp = DateTime.Now.ToString("dd-MMM-yyyy");
                if (reportId != -1)
                {           
                    ReportsCommon.openForm(null, "CustomReportviewer", new object[] { false, txtReportName.Text, txtReportName.Text, timestamp, reportId, DateTime.Now.Date.AddHours(6).AddDays(-1), DateTime.Now.Date.AddHours(6), -1, null, "P" }, true, false);
                }
                else
                {
                    MessageBox.Show(Common.MessageUtils.getMessage(1237), Common.MessageUtils.getMessage(12641));
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// btnParameters_Click
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void btnParameters_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (reportId != -1)
                {
                    frmReportParameters frp = new frmReportParameters((int)reportId);
                    log.LogMethodExit();
                    frp.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
    }
}
