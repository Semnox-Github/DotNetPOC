/********************************************************************************************
 * Project Name - CustomReportViewer 
 * Description  - CustomReportViewer code behind
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 * 2.100      28-Sep-2020      Laster Menezes     Modified GetCustomReportSource to pass reportid
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using System.Data.SqlClient;
using Semnox.Parafait.Reports;

namespace Semnox.Parafait.Report.Reports
{
    /// <summary>
    /// CustomReportviewer Class
    /// </summary>
    public partial class CustomReportviewer : Form
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        bool backgroundMode;
        string reportName;
        string timestamp;
        DateTime fromdate;
        DateTime todate;
        int reportId;
        int globalReportScheduleReportId;
        string[] otherParams;
        string DBQuery, outputFormat;
        //string reportFileName;
        string breakColumn;
        bool HideBreakColumn;
        string AggregateColumns;
        //, HideGridLines;
        static Telerik.Reporting.ReportSource ReportSource;
        ReportsList reportsList;
        ReportsDTO reportsDTO;
        List<clsReportParameters.SQLParameter> parameterList = new List<clsReportParameters.SQLParameter>();
        static clsReportParameters parameters;
        List<clsReportParameters.SelectedParameterValue> lstOtherParameters;
        List<clsReportParameters.SelectedParameterValue> lstBackgroundParams;
        bool sendBackgroundEmail;
        BackgroundWorker bgWorker;
        EmailReport emailReportForm;
        //ArrayList selectedSites;
        string emailList;
        string reportEmailFormat;
        List<SqlParameter> selectedParameters;
        DateTime startTime, endTime;
        string strParameters;
        bool ShowGrandTotal = false;
        bool printContinuous = false;
        bool repeatBreakColumns = false;
        int maxDateRange = -1;

        /// <summary>
        /// CustomReportviewer with params
        /// </summary>
        public CustomReportviewer(bool BackgroundMode, string ReportKey, string ReportName, string TimeStamp, int ReportID, DateTime FromDate, DateTime ToDate, int ReportScheduleReportId, List<clsReportParameters.SelectedParameterValue> ListBackgroundParams, string outputFileFormat)
        {
            log.Debug("Starts-CustomReportviewer(BackgroundMode, ReportID, ReportName, TimeStamp) method.");
            try
            {
                InitializeComponent();

                reportViewer.UpdateUI += reportViewer_UpdateUI;
                reportName = ReportName;
                outputFormat = outputFileFormat;
                lstBackgroundParams = ListBackgroundParams;
                backgroundMode = BackgroundMode;
                timestamp = TimeStamp;
                if (Common.ParafaitEnv.IsCorporate)
                {
                    machineUserContext.SetSiteId(Common.ParafaitEnv.SiteId);
                }
                else
                {
                    machineUserContext.SetSiteId(-1);
                }
                machineUserContext.SetUserId(Common.ParafaitEnv.LoginID);
                btnEmailReport.Enabled = false;
                reportId = ReportID;
                Common.Utilities.setLanguage(this);
                globalReportScheduleReportId = ReportScheduleReportId;
                if (globalReportScheduleReportId != -1)
                {
                    parameters = new clsReportParameters(reportId);
                    parameters.getScheduleParameters(globalReportScheduleReportId);
                    parameterList = parameters.getParameterList();
                }
                else
                {

                    parameters = new clsReportParameters(ReportID);
                    if (parameters.lstParameters.Count <= 0)
                    {
                        parameterList = null;
                    }
                    parameterList = parameters.getParameterList();
                    if (parameterList == null || parameterList.Count == 0)
                    {
                        groupBox5.Visible = false;
                        flpParameters1.Visible = false;
                        pnlButtons.Location = new Point(3, CalFromDate.Bottom + 50);
                        groupBox1.Height = 100;
                        reportViewer.Location = new Point(3, groupBox1.Bottom + 50);
                    }
                    else
                    {
                        groupBox5.Visible = true;
                        flpParameters1.Visible = true;
                    }
                }
                fromdate = FromDate;
                todate = ToDate;
                log.Debug("Ends-CustomReportviewer(BackgroundMode, ReportID, ReportName, TimeStamp) method.");
            }
            catch (Exception ex)
            {
                log.Error("Ends-CustomReportviewer(BackgroundMode, ReportID, ReportName, TimeStamp) constructor with exception: " + ex.ToString());
            }
        }
        void reportViewer_UpdateUI(object sender, EventArgs e)
        {
            groupBox1.Focus();
        }
        private void CustomReportviewer_Load(object sender, EventArgs e)
        {
            log.Debug("Starts-CustomReportviewer_Load() event.");
            try
            {
                this.Name = reportName;
                this.Text = reportName;


                if (!backgroundMode)
                {
                    CalFromDate.Value = fromdate;
                    dtpTimeFrom.Value = fromdate;
                    CalToDate.Value = todate;
                    dtpTimeTo.Value = todate;
                    CalFromDate.Focus();
                }
                //InitializeControls();
                LoadReportDetails();                             
                if (backgroundMode)
                {
                    try
                    {
                        outputFormat = (Common.GlobalReportScheduleReportId != -1) ? Common.getReportOutputFormat() : outputFormat;
                        string format = "PDF";
                        string extension = "pdf";

                        if (outputFormat == "E")
                        {
                            format = "XLSX";
                            extension = "xlsx";
                        }

                        else if (outputFormat == "H")
                        {
                            extension = "html";
                            format = "HTML";
                        }
                        else if (outputFormat == "V")
                        {
                            extension = "csv";
                            format = "CSV";
                        }
                        else
                        {
                            extension = "pdf";
                            format = "PDF";
                        }
                        string fileName = Common.Utilities.getParafaitDefaults("PDF_OUTPUT_DIR") + "\\" + reportName + ((Common.GlobalScheduleId != -1 && timestamp != "") ? "-" : "") + timestamp + "." + extension;
                        if (ReportSource == null)
                        {
                            RunReport();
                        }
                        Common.ExportReportData(ReportSource, format, fileName);
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                    }
                    finally
                    {
                        this.ShowInTaskbar = false;
                        this.Visible = false;
                        this.Close();
                    }
                }
                else
                {
                    bool isDateRangeValid = IsReportDateRangeValid();
                    if (!isDateRangeValid)
                    {
                        return;
                    }
                    RunReport();
                }

                log.Debug("Ends-CustomReportviewer_Load() event.");
            }
            catch (Exception ex)
            {
                log.Error("Ends-CustomReportviewer_Load() event with exception: " + ex.ToString());
            }
        }

        private void LoadReportDetails()
        {
            log.Debug("Starts-LoadReportDetails() method.");
            try
            {
                reportsList = new ReportsList();
                reportsDTO = new ReportsDTO();
                reportsDTO = reportsList.GetReports(reportId);
                if (reportsDTO == null)
                {
                    MessageBox.Show("Invalid Report Id: " + reportId.ToString(), "Run Custom Report");
                    return;
                }
                DBQuery = reportsDTO.DBQuery;
                outputFormat = reportsDTO.OutputFormat;
                breakColumn = reportsDTO.BreakColumn;
                AggregateColumns = reportsDTO.AggregateColumns;
                maxDateRange = reportsDTO.MaxDateRange;
                if (!string.IsNullOrEmpty(reportsDTO.ShowGrandTotal))
                {
                    ShowGrandTotal = reportsDTO.ShowGrandTotal == "N" ? false : true;
                }
              

                if (reportsDTO.HideBreakColumn == "Y")
                    HideBreakColumn = true;
                else
                    HideBreakColumn = false;

                if (reportsDTO.PrintContinuous == "Y")
                    printContinuous = true;
                else
                    printContinuous = false;

                if(!string.IsNullOrEmpty(reportsDTO.RepeatBreakColumns))
                {
                    if (reportsDTO.RepeatBreakColumns == "Y")
                        repeatBreakColumns = true;
                    else
                        repeatBreakColumns = false;
                }
                else
                {
                    string repeatBreakColumnsForSite = Common.Utilities.getParafaitDefaults("REPEAT_BREAKCOLUMN_DATA_IN_CUSTOM_REPORTS");
                    if (repeatBreakColumnsForSite == "Y")
                        repeatBreakColumns = true;
                    else
                        repeatBreakColumns = false;
                }

                //if (reportsDTO.HideGridLines == "Y")
                //    HideGridLines = true;
                //else
                //    HideGridLines = false;
               
                if (backgroundMode)
                {
                    string schOutputFormat = Common.getReportOutputFormat();
                    if (schOutputFormat != "D")
                        outputFormat = schOutputFormat;
                }
                LoadParameters();

                log.Debug("Ends-LoadReportDetails() method.");
            }
            catch (Exception ex)
            {
                log.Error("Ends-LoadReportDetails() event with exception: " + ex.ToString());
            }
        }

        private void LoadParameters()
        {
            log.Debug("Starts-LoadParameters() method.");
            try
            {
                if (parameterList != null)
                {
                    flpParameters1.Controls.Clear();
                    //flpParameters1.RowCount = 0;
                    //flpParameters1.ColumnCount = 0;
                    if (parameterList != null)
                    {
                        foreach (clsReportParameters.ReportParameter param in parameters.lstParameters)
                        {
                            System.Globalization.CultureInfo cultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture;
                            System.Globalization.TextInfo textInfo = cultureInfo.TextInfo;
                            string paramOperator = param.Operator.ToString();

                           FlowLayoutPanel fpParam = new FlowLayoutPanel();
                            fpParam.Width = ((flpParameters1.Width) / 3) - 20;
                            fpParam.Height = 40;
                            fpParam.FlowDirection = FlowDirection.LeftToRight;
                            string paramName = param.ParameterName;
                            string dataSource = param.DataSource.ToString();
                            System.Windows.Forms.Label lblParameter = new System.Windows.Forms.Label();
                            lblParameter.Height = 40;
                            lblParameter.Width = 100;
                            lblParameter.Padding = new Padding(0, 6, 0, 0);
                            lblParameter.Text = param.ParameterName.Replace('_', ' ');
                            lblParameter.Text = textInfo.ToTitleCase(lblParameter.Text.ToLower());
                            lblParameter.Text += ":";
                            lblParameter.AutoSize = false;
                            fpParam.Controls.Add(lblParameter);

                            Telerik.WinControls.UI.RadCheckedDropDownList cmbMultiValueParameter = new Telerik.WinControls.UI.RadCheckedDropDownList();

                            ComboBox cmbParameter = new ComboBox();
                            TextBox txtParameter = new TextBox();

                            switch (param.DataSourceType)
                            {
                                case clsReportParameters.DataSourceType.CONSTANT:
                                    {
                                        switch (param.DataType)
                                        {
                                            case clsReportParameters.DataType.TEXT:
                                                {
                                                    txtParameter = new TextBox();
                                                    txtParameter.Width = 120;
                                                    txtParameter.Tag = param.DataType;

                                                    txtParameter.Height = 30;// lblParameter.Height;
                                                    txtParameter.Name = paramName;
                                                    if (param.Value != null)
                                                        txtParameter.Text = param.Value.ToString();
                                                    else
                                                        txtParameter.Text = dataSource;
                                                    fpParam.Controls.Add(txtParameter);
                                                    flpParameters1.Controls.Add(fpParam);
                                                    param.UIControl = txtParameter;
                                                    break;
                                                }
                                            case clsReportParameters.DataType.NUMBER:
                                                {
                                                    txtParameter = new TextBox();
                                                    txtParameter.Width = 120;
                                                    txtParameter.Tag = param.DataType;
                                                    txtParameter.Name = paramName;
                                                    try
                                                    {
                                                        if (param.Value != null)
                                                            txtParameter.Text = param.Value.ToString();
                                                        else
                                                            txtParameter.Text = Convert.ToDecimal(dataSource).ToString();
                                                    }
                                                    catch { }
                                                    fpParam.Controls.Add(txtParameter);
                                                    flpParameters1.Controls.Add(fpParam);
                                                    param.UIControl = txtParameter;

                                                    txtParameter.KeyPress += new KeyPressEventHandler(delegate (object s, KeyPressEventArgs ev)
                                                    {
                                                        char decimalChar = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator[0];
                                                        if (!Char.IsNumber(ev.KeyChar) && !Char.IsControl(ev.KeyChar) && !(ev.KeyChar == decimalChar))
                                                        {
                                                            ev.Handled = true;
                                                        }
                                                    });

                                                    break;
                                                }
                                            case clsReportParameters.DataType.DATETIME:
                                                {
                                                    txtParameter = new TextBox();
                                                    txtParameter.Width = 120;
                                                    txtParameter.Tag = param.DataType;
                                                    txtParameter.Name = paramName;
                                                    if (param.Value != null && param.Value.ToString().Trim() != "")
                                                        txtParameter.Text = Convert.ToDateTime(param.Value).ToString(Common.ParafaitEnv.DATETIME_FORMAT);
                                                    else
                                                        txtParameter.Text = dataSource;
                                                    fpParam.Controls.Add(txtParameter);
                                                    flpParameters1.Controls.Add(fpParam);
                                                    param.UIControl = txtParameter;

                                                    DateTimePicker dtPicker = new DateTimePicker();
                                                    dtPicker.Width = 17;
                                                    // dtPicker.Height = txtParameter.Height;
                                                    dtPicker.Format = DateTimePickerFormat.Short;
                                                    flpParameters1.Controls.Add(dtPicker);

                                                    dtPicker.ValueChanged += new EventHandler(delegate (object o, EventArgs ev)
                                                    {
                                                        txtParameter.Text = dtPicker.Value.Date.AddHours(6).ToString(dtPicker.CustomFormat);
                                                    });
                                                    break;
                                                }

                                        }

                                        break;
                                    }
                                case clsReportParameters.DataSourceType.STATIC_LIST:
                                    {
                                        switch (param.Operator)
                                        {
                                            case clsReportParameters.Operator.INLIST:
                                                {
                                                    cmbMultiValueParameter = new Telerik.WinControls.UI.RadCheckedDropDownList();
                                                    cmbMultiValueParameter.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList;
                                                    cmbMultiValueParameter.ShowCheckAllItems = true;
                                                    cmbMultiValueParameter.Width = 150;
                                                    cmbMultiValueParameter.Height = txtParameter.Height;
                                                    cmbMultiValueParameter.Name = paramName;
                                                    cmbMultiValueParameter.Tag = param.DataType;


                                                    if (param.InListValue.InListValueList != null && param.InListValue.InListValueDT.Rows.Count > 0)
                                                    {
                                                        //InListValue

                                                        cmbMultiValueParameter.DataSource = param.InListValue.InListValueDT;
                                                        cmbMultiValueParameter.DisplayMember = "Value";   // param.InListValue.clsInListValueObject;
                                                        cmbMultiValueParameter.ValueMember = "Value";


                                                    }
                                                    else
                                                    {
                                                        if (param.ListDataSource.Rows.Count > 0)
                                                            param.ListDataSource.Rows.RemoveAt(0);
                                                        cmbMultiValueParameter.DataSource = param.ListDataSource;
                                                        cmbMultiValueParameter.DisplayMember = cmbParameter.ValueMember = param.ListDataSource.Columns[0].ColumnName;
                                                    }

                                                    fpParam.Controls.Add(cmbMultiValueParameter);
                                                    flpParameters1.Controls.Add(fpParam);

                                                    param.UIControl = cmbMultiValueParameter;
                                                    if (param.Value != null)
                                                    {
                                                        try
                                                        {
                                                            cmbMultiValueParameter.SelectedValue = param.Value;
                                                        }
                                                        catch { }
                                                    }
                                                    else
                                                    {
                                                        foreach (Telerik.WinControls.UI.RadCheckedListDataItem item in cmbMultiValueParameter.Items)
                                                        {
                                                            item.Checked = true;
                                                        }
                                                    }
                                                }
                                                break;
                                            case clsReportParameters.Operator.Default:
                                                {
                                                    cmbParameter = new ComboBox();
                                                    cmbParameter.DropDownStyle = ComboBoxStyle.DropDownList;
                                                    cmbParameter.Width = 150;
                                                    cmbParameter.Name = paramName;
                                                    cmbParameter.Tag = param.DataType;
                                                    if (param.ListDataSource.Rows.Count > 0)
                                                        param.ListDataSource.Rows.RemoveAt(0);
                                                    cmbParameter.DataSource = param.ListDataSource;
                                                    cmbParameter.DisplayMember = cmbParameter.ValueMember = param.ListDataSource.Columns[0].ColumnName;

                                                    fpParam.Controls.Add(cmbParameter);
                                                    flpParameters1.Controls.Add(fpParam);

                                                    param.UIControl = cmbParameter;
                                                    if (param.Value != null)
                                                    {
                                                        try
                                                        {
                                                            cmbParameter.SelectedValue = param.Value;
                                                        }
                                                        catch { }
                                                    }
                                                    else
                                                    {
                                                        cmbParameter.SelectedIndex = -1;
                                                    }
                                                }
                                                break;
                                        }
                                        break;
                                    }
                                case clsReportParameters.DataSourceType.QUERY:
                                    {
                                        switch (param.Operator)
                                        {
                                            case clsReportParameters.Operator.INLIST:
                                                {
                                                    cmbMultiValueParameter = new Telerik.WinControls.UI.RadCheckedDropDownList();
                                                    cmbMultiValueParameter.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList;
                                                    cmbMultiValueParameter.ShowCheckAllItems = true;
                                                    cmbMultiValueParameter.Width = 150;
                                                    cmbMultiValueParameter.Name = paramName;
                                                    cmbMultiValueParameter.Tag = param.DataType;

                                                    if (param.ListDataSource.Rows.Count > 0)
                                                        param.ListDataSource.Rows.RemoveAt(0);

                                                    if (param.InListValue.InListValueList != null && param.InListValue.InListValueDT.Rows.Count > 0)
                                                    {
                                                        //InListValue

                                                        cmbMultiValueParameter.DataSource = param.InListValue.InListValueDT;
                                                        cmbMultiValueParameter.ValueMember = "Value";
                                                        cmbMultiValueParameter.DisplayMember = "Value";   // param.InListValue.clsInListValueObject;
                                                       // cmbMultiValueParameter.SelectAll();



                                                    }
                                                    else
                                                    {
                                                        cmbMultiValueParameter.DataSource = param.ListDataSource;
                                                        cmbMultiValueParameter.ValueMember = param.ListDataSource.Columns[0].ColumnName;
                                                        if (param.ListDataSource.Columns.Count > 1)
                                                            cmbMultiValueParameter.DisplayMember = param.ListDataSource.Columns[1].ColumnName;
                                                        else
                                                            cmbMultiValueParameter.DisplayMember = cmbMultiValueParameter.ValueMember;
                                                    }

                                                    fpParam.Controls.Add(cmbMultiValueParameter);
                                                    flpParameters1.Controls.Add(fpParam);

                                                    param.UIControl = cmbMultiValueParameter;
                                                    if (param.Value != null)
                                                    {
                                                        try
                                                        {
                                                            cmbParameter.SelectedValue = param.Value;
                                                        }
                                                        catch { }
                                                    }
                                                    else
                                                    {
                                                        foreach (Telerik.WinControls.UI.RadCheckedListDataItem item in cmbMultiValueParameter.Items)
                                                        {
                                                            item.Checked = true;
                                                        }
                                                    }
                                                }
                                                break;
                                            case clsReportParameters.Operator.Default:
                                                {
                                                    cmbParameter = new ComboBox();
                                                    cmbParameter.DropDownStyle = ComboBoxStyle.DropDownList;
                                                    cmbParameter.Width = 150;
                                                    cmbParameter.Name = paramName;
                                                    cmbParameter.Tag = param.DataType;
                                                    if (param.ListDataSource.Rows.Count > 0)
                                                        param.ListDataSource.Rows.RemoveAt(0);
                                                    cmbParameter.DataSource = param.ListDataSource;
                                                    cmbParameter.ValueMember = param.ListDataSource.Columns[0].ColumnName;
                                                    if (param.ListDataSource.Columns.Count > 1)
                                                        cmbParameter.DisplayMember = param.ListDataSource.Columns[1].ColumnName;
                                                    else
                                                        cmbParameter.DisplayMember = cmbParameter.ValueMember;

                                                    fpParam.Controls.Add(cmbParameter);
                                                    flpParameters1.Controls.Add(fpParam);

                                                    param.UIControl = cmbParameter;
                                                    if (param.Value != null)
                                                    {
                                                        try
                                                        {
                                                            cmbParameter.SelectedValue = param.Value;
                                                        }
                                                        catch { }
                                                    }
                                                    else
                                                    {
                                                        cmbParameter.SelectedIndex = 0;
                                                    }
                                                }
                                                break;
                                        }
                                        break;
                                    }
                            }

                        }
                    }
                }
                log.Debug("Ends-LoadParameters() method.");
            }
            catch (Exception ex)
            {

                log.Error("Ends-LoadParameters() event with exception: " + ex.ToString());
            }
        }

        void dtp_ValueChanged(object sender, EventArgs e)
        {
            log.Debug("Starts-dtp_ValueChanged() event.");
            DateTimePicker dtp = sender as DateTimePicker;
            foreach (Control c in flpParameters1.Controls)
            {
                if (c.Tag != null && c.Tag.ToString() == dtp.Tag.ToString() && c.GetType().ToString().Contains("TextBox"))
                {
                    c.Text = dtp.Value.ToString(Common.ParafaitEnv.DATE_FORMAT);
                    break;
                }
            }
            log.Debug("Ends-dtp_ValueChanged() event.");
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnEmailReport_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnEmailReport_Click() event.");
            try
            {
                this.Cursor = Cursors.WaitCursor;

                emailReportForm = new EmailReport();
                emailReportForm.setEmailParamsCallback = new EmailReport.SendOnDemandEmailDelegate(this.SendEmail);
                log.Debug("Ends-btnEmailReport_Click() event.");
                //emailReportForm.ShowDialog();//Commented on 13-Sep-2017
                //Begin: Added on 13-Sep-2017 for changing cursor back to default
                if (emailReportForm.ShowDialog() == DialogResult.Cancel)
                {
                    this.Cursor = Cursors.Default;
                }
                //End: Added on 13-Sep-2017 for changing cursor back to default

            }
            catch (Exception ex)
            {
                log.Error("Ends-btnEmailReport_Click() event with exception: " + ex.ToString());
            }
        }

        private void SendEmail(string Format, string EmailList, Label lblEmailSendingStatus)
        {
            log.Debug("Starts-SendEmail() method.");
            try
            {
                string extension = "pdf";
                if (Format == "Excel")
                {
                    Format = "XLSX";
                    extension = "xlsx";
                }
                else if (Format == "CSV")
                {
                    extension = "csv";
                    Format = "CSV";
                }

                string TimeStamp = DateTime.Now.ToString("yyyy-MMM-dd Hmm");
                string fileName = Common.Utilities.getParafaitDefaults("PDF_OUTPUT_DIR") + "\\" + reportName + " - " + TimeStamp + "." + extension;
                // Semnox.Parafait.Report.Reports.Common.ExportReportData(ReportSource, Format, fileName);
                string message = "";
                if (sendBackgroundEmail)
                {
                    emailList = EmailList;
                    reportEmailFormat = Format;
                    bgWorker = new BackgroundWorker();
                    bgWorker.DoWork += new DoWorkEventHandler(bgWorker_DoWork);
                    bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_RunWorkerCompleted);
                    bgWorker.RunWorkerAsync();
                    emailReportForm.Close();
                    return;
                }
                if (!Common.SendReport(ReportSource, reportName, Format, TimeStamp, EmailList, Common.Utilities, ref message, ""))
                    MessageBox.Show(Common.MessageUtils.getMessage("Error") + ": " + message);
                else
                    MessageBox.Show(Common.MessageUtils.getMessage(333), Common.MessageUtils.getMessage("On Demand Email"));
                lblEmailSendingStatus.Text = "";
                this.Cursor = Cursors.Default;
                log.Debug("Ends-SendEmail() method.");
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                log.Error("Ends-SendEmail() method with exception: " + ex.ToString());
                MessageBox.Show(ex.Message);
            }
        }

        private void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            log.Debug("Starts-bgWorker_DoWork() event.");
            try
            {
                string message = "";
                string TimeStamp = DateTime.Now.ToString("yyyy-MMM-dd Hmm");
                string fileName = Common.Utilities.getParafaitDefaults("PDF_OUTPUT_DIR") + "\\" + reportName + " - " + TimeStamp + ".pdf";
                Telerik.Reporting.Report rptSrc = Common.GetCustomReportSource(reportId, DBQuery, reportName, fromdate, todate, "", "", selectedParameters, HideBreakColumn, breakColumn, ShowGrandTotal, printContinuous, AggregateColumns, repeatBreakColumns, ref message, 0,otherParams);

                RunReportAuditDTO runReportAuditDTO = new RunReportAuditDTO();
                runReportAuditDTO.StartTime = startTime;
                runReportAuditDTO.EndTime = endTime;
                runReportAuditDTO.ReportKey = reportName;
                runReportAuditDTO.ReportId = reportId;
                runReportAuditDTO.CreationDate = DateTime.Now;
                runReportAuditDTO.LastUpdateDate = DateTime.Now;
                runReportAuditDTO.ParameterList = strParameters;
                runReportAuditDTO.LastUpdatedBy = Common.ParafaitEnv.Username;
                runReportAuditDTO.SiteId = machineUserContext.GetSiteId();
                runReportAuditDTO.Message = message;
                runReportAuditDTO.Source = "R";
                RunReportAudit runReportAudit = new RunReportAudit(runReportAuditDTO);
                runReportAudit.Save();

                if (!Common.SendReport(rptSrc, "", reportEmailFormat, TimeStamp, emailList, Common.Utilities, ref message, ""))
                    MessageBox.Show(Common.MessageUtils.getMessage("Error") + ": " + message);
                else
                    MessageBox.Show(Common.MessageUtils.getMessage(333), Common.MessageUtils.getMessage("On Demand Email"));
                log.Debug("Ends-bgWorker_DoWork() event.");
            }
            catch (Exception ex)
            {
                log.Error("Ends-bgWorker_DoWork() event with exception: " + ex.ToString());
            }
        }

        private void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            log.Debug("Starts-bgWorker_RunWorkerCompleted() event.");
            try
            {
                log.Debug("Ends-bgWorker_RunWorkerCompleted() event.");
            }
            catch (Exception ex)
            {
                log.Error("Ends-bgWorker_RunWorkerCompleted() event with exception: " + ex.ToString());
            }
        }
        private void btnGo_Click(object sender, EventArgs e)
        {
            fromdate = CalFromDate.Value.Date.AddHours(dtpTimeFrom.Value.Hour).AddMinutes(dtpTimeFrom.Value.Minute);
            todate = CalToDate.Value.Date.AddHours(dtpTimeTo.Value.Hour).AddMinutes(dtpTimeTo.Value.Minute);
            if (IsReportDateRangeValid())
            {
                RunReport();
            }
        }



        void RunReport()
        {
            log.Debug("Starts-RunReport() method.");
            try
            {
                strParameters = string.Empty;
                string msgAllSelected = string.Empty;
                msgAllSelected = Common.GetMessage(2719);
                if (string.IsNullOrEmpty(msgAllSelected))
                {
                    msgAllSelected = "- All Selected -";
                }
                startTime = DateTime.Now;
                otherParams = new string[parameterList.Count];
                selectedParameters = new List<SqlParameter>();
                string tDBQuery = DBQuery;
                strParameters = "";
                if (!backgroundMode)
                {
                    fromdate = CalFromDate.Value.Date.AddHours(dtpTimeFrom.Value.Hour).AddMinutes(dtpTimeFrom.Value.Minute);
                    todate = CalToDate.Value.Date.AddHours(dtpTimeTo.Value.Hour).AddMinutes(dtpTimeTo.Value.Minute);
                }
                foreach (clsReportParameters.SQLParameter param in parameterList)
                {
                    if (param.ParameterName.ToLower() == "@fromdate") // check if @fromDate is overridden by user parameters
                    {
                        if (param.ParameterValue != DBNull.Value)
                            fromdate = Convert.ToDateTime(param.ParameterValue);
                        break;
                    }
                }

                foreach (clsReportParameters.SQLParameter param in parameterList)
                {
                    if (param.ParameterName.ToLower() == "@todate") // check if @toDate is overridden by user parameters
                    {
                        if (param.ParameterValue != DBNull.Value)
                            todate = Convert.ToDateTime(param.ParameterValue);
                        break;
                    }
                }


                List<clsReportParameters.SelectedParameterValue> lstAuditReportParameters = new List<clsReportParameters.SelectedParameterValue>();
                lstAuditReportParameters.Add(new clsReportParameters.SelectedParameterValue("Fromdate", fromdate));
                lstAuditReportParameters.Add(new clsReportParameters.SelectedParameterValue("Todate", todate));
                lstAuditReportParameters.Add(new clsReportParameters.SelectedParameterValue("Offset", Common.Offset));
                lstAuditReportParameters.Add(new clsReportParameters.SelectedParameterValue("Site", Common.ParafaitEnv.SiteName));

                //int parameterCount = 0;
                int otherParamCount = 0;
                lstOtherParameters = new List<clsReportParameters.SelectedParameterValue>();
                foreach (Control tp in flpParameters1.Controls)
                {
                    foreach (Control tpc in tp.Controls)
                    {
                        if (tpc.Tag != null)
                        {
                            if (tpc.Name.ToLower() == "@fromdate" || tpc.Name.ToLower() == "@todate")
                                continue;
                            if (tpc.GetType().ToString().Contains("Text"))
                            {
                                if (!string.IsNullOrEmpty((tpc as TextBox).Text))
                                {
                                    selectedParameters.Add(new SqlParameter("@" + tpc.Name, (tpc as TextBox).Text));
                                    //strParameters += ";" + tpc.Name + ":" + (tpc as TextBox).Text;
                                    otherParams[otherParamCount] = tpc.Name + ": " + (tpc as TextBox).Text;
                                    lstAuditReportParameters.Add(new clsReportParameters.SelectedParameterValue(tpc.Name, (tpc as TextBox).Text.ToString()));
                                    otherParamCount++;
                                }
                                else
                                {
                                    if(tpc.Tag.ToString() == clsReportParameters.DataType.TEXT.ToString())
                                    {
                                        selectedParameters.Add(new SqlParameter("@" + tpc.Name, DBNull.Value));
                                    }
                                    else if (tpc.Tag.ToString() == clsReportParameters.DataType.NUMBER.ToString())
                                    {
                                        selectedParameters.Add(new SqlParameter("@" + tpc.Name, -1));
                                    }

                                    //strParameters += ";" + tpc.Name + ":" + (tpc as TextBox).Text;
                                    otherParams[otherParamCount] = tpc.Name + ": " + (tpc as TextBox).Text;
                                    lstAuditReportParameters.Add(new clsReportParameters.SelectedParameterValue(tpc.Name, (tpc as TextBox).Text.ToString()));
                                    otherParamCount++;
                                }

                            }
                            else if (tpc.GetType().ToString().Contains("ComboBox"))
                            {
                                if ((tpc as ComboBox).SelectedIndex != -1)
                                {
                                    selectedParameters.Add(new SqlParameter("@" + tpc.Name, (tpc as ComboBox).SelectedValue));
                                    //strParameters += ";" + tpc.Name + ":" + (tpc as ComboBox).SelectedValue;
                                    otherParams[otherParamCount] = tpc.Name + ": " + (tpc as ComboBox).Text;
                                    lstAuditReportParameters.Add(new clsReportParameters.SelectedParameterValue(tpc.Name, (tpc as ComboBox).Text));
                                    otherParamCount++;
                                }
                                else
                                {
                                    if (tpc.Tag.ToString() == clsReportParameters.DataType.TEXT.ToString())
                                    {
                                        selectedParameters.Add(new SqlParameter("@" + tpc.Name, DBNull.Value));
                                    }
                                    else if (tpc.Tag.ToString() == clsReportParameters.DataType.NUMBER.ToString())
                                    {
                                        selectedParameters.Add(new SqlParameter("@" + tpc.Name, -1));
                                    }
                                   // strParameters += ";" + tpc.Name + ":" + (tpc as ComboBox).SelectedValue;
                                    otherParams[otherParamCount] = tpc.Name + ": " + (tpc as ComboBox).Text;
                                    lstAuditReportParameters.Add(new clsReportParameters.SelectedParameterValue(tpc.Name, (tpc as ComboBox).Text));
                                    otherParamCount++;
                                }
                            }
                            else
                            {
                                if ((tpc.GetType().ToString().Contains("RadCheckedDropDownList")))
                                {
                                    if ((tpc as Telerik.WinControls.UI.RadCheckedDropDownList).CheckedItems.Count != 0)
                                    {
                                        string paramSelectedValue = "";
                                        string paramSelectedText = "";

                                        foreach (Telerik.WinControls.UI.RadCheckedListDataItem item in (tpc as Telerik.WinControls.UI.RadCheckedDropDownList).CheckedItems)
                                        {
                                            if (item.Value != null && item.Value != DBNull.Value)
                                            {
                                                paramSelectedValue += ",'" + item.Text.ToString() + "'";
                                                paramSelectedText += ",'" + item.Text + "'";
                                            }
                                        }

                                        paramSelectedValue = paramSelectedValue.Substring(1);
                                        //strParameters += ";" + tpc.Name + ":" + paramSelectedValue;
                                        tDBQuery = tDBQuery.Replace("@" + tpc.Name, " " + paramSelectedValue + " "); // in list is a string replacement 
                                        if ((tpc as Telerik.WinControls.UI.RadCheckedDropDownList).CheckedItems.Count == (tpc as Telerik.WinControls.UI.RadCheckedDropDownList).Items.Count)
                                        {
                                            otherParams[otherParamCount] = tpc.Name + ": " + msgAllSelected;
                                            lstAuditReportParameters.Add(new clsReportParameters.SelectedParameterValue(tpc.Name, msgAllSelected));
                                        }
                                        else
                                        {
                                            otherParams[otherParamCount] = tpc.Name + ": " + paramSelectedText.Substring(1);
                                            lstAuditReportParameters.Add(new clsReportParameters.SelectedParameterValue(tpc.Name, paramSelectedText.Substring(1)));
                                        }

                                       
                                        otherParamCount++;
                                    }
                                }
                            }
                        }
                    }
                }

                if (tDBQuery.ToLower().Contains("@fromdate"))
                {
                    selectedParameters.Add(new SqlParameter("@FromDate", fromdate));
                    //strParameters += ";FromDate:" + fromdate.ToString();
                    //selectReportParameters[parameterCount] = new SqlParameter("@FromDate", fromdate);
                    //parameterCount++;
                }

                if (tDBQuery.ToLower().Contains("@todate"))
                {
                    selectedParameters.Add(new SqlParameter("@ToDate", todate));
                    //strParameters += ";todate:" + todate.ToString();
                    //selectReportParameters[parameterCount] = new SqlParameter("@ToDate", todate);
                    //parameterCount++;
                }

                if (lstBackgroundParams != null)
                {
                    foreach (clsReportParameters.SelectedParameterValue param in lstBackgroundParams)
                    {
                        selectedParameters.Add(new SqlParameter(param.parameterName, param.parameterValue));
                        lstAuditReportParameters.Add(new clsReportParameters.SelectedParameterValue(param.parameterName, param.parameterValue[0]));
                    }
                }

                if (Common.ParafaitEnv.IsCorporate)
                    tDBQuery = System.Text.RegularExpressions.Regex.Replace(tDBQuery, "[@][Ss][Ii][Tt][Ee][Ii][Dd]", "(" + Common.ParafaitEnv.SiteId.ToString() + ")");
                else
                    tDBQuery = System.Text.RegularExpressions.Regex.Replace(tDBQuery, "[@][Ss][Ii][Tt][Ee][Ii][Dd]", "(-1)");

                DataTable dt = new DataTable();
                this.Cursor = Cursors.WaitCursor;

                try
                {
                    if (Common.ParafaitEnv.IsCorporate && Common.ShowEmailOnload())
                    //if((todate - fromdate).TotalDays > 3)
                    {
                        sendBackgroundEmail = true;
                        btnEmailReport_Click(null, null);
                        DBQuery = tDBQuery;
                        return;
                    }
                    ReportsList reportsList = new ReportsList();
                    string message = "";
                    string schOutputFormat = Common.getReportOutputFormat();
                    Telerik.Reporting.Report report = null;


                    if (outputFormat == "C")
                    {
                        string connectionString = Common.getConnectionString();

                        report = Common.GetCustomReportBarChartSource(connectionString, reportId, tDBQuery, reportName, fromdate, todate, machineUserContext.GetUserId(), selectedParameters, false, -1, ref message, otherParams);
                    }
                    else
                    {
                        report = Common.GetCustomReportSource(reportId, tDBQuery, reportName, fromdate, todate, machineUserContext.GetUserId(), "", selectedParameters, HideBreakColumn, breakColumn, ShowGrandTotal, printContinuous, AggregateColumns, repeatBreakColumns, ref message, 0,otherParams);
                    }

                    if (report != null)
                    {
                        reportViewer.ReportSource = report;
                        ReportSource = report;//Added on 13-Sep-2017 for email part

                        reportViewer.RefreshReport();
                        btnEmailReport.Enabled = true;
                    }
                    else
                    {
                        btnEmailReport.Enabled = false;
                        reportViewer.ReportSource = null;

                    }

                    if (message.Contains("No data"))
                    {
                        ReportSource = null;
                        reportViewer.ReportSource = null;
                        reportViewer.Resources.MissingReportSource = "No data found";
                        reportViewer.RefreshReport();
                    }
                    else
                    {
                        reportViewer.Resources.MissingReportSource = message;
                        reportViewer.RefreshReport();
                    }

                    foreach (clsReportParameters.SelectedParameterValue AuditParam in lstAuditReportParameters)
                    {
                        //fetching the string parameter value for the parameters list
                        strParameters += "@" + AuditParam.parameterName + "='" + AuditParam.parameterValue[0].ToString() + "';";
                    }


                    endTime = DateTime.Now;
                    ReportsDTO reportsDTO = reportsList.GetReports(reportId);

                    RunReportAuditDTO runReportAuditDTO = new RunReportAuditDTO();
                    runReportAuditDTO.StartTime = startTime;
                    runReportAuditDTO.EndTime = endTime;
                    runReportAuditDTO.ReportKey = reportsDTO != null ? reportsDTO.ReportKey : "";
                    runReportAuditDTO.ReportId = reportId;
                    runReportAuditDTO.CreationDate = DateTime.Now;
                    runReportAuditDTO.LastUpdateDate = DateTime.Now;
                    runReportAuditDTO.ParameterList = strParameters;
                    runReportAuditDTO.LastUpdatedBy = Common.ParafaitEnv.Username;
                    runReportAuditDTO.SiteId = machineUserContext.GetSiteId();
                    runReportAuditDTO.Message = message;
                    if (backgroundMode)
                        runReportAuditDTO.Source = "S";
                    else
                        runReportAuditDTO.Source = "R";
                    RunReportAudit runReportAudit = new RunReportAudit(runReportAuditDTO);
                    runReportAudit.Save();

                }
                catch (Exception ex)
                {
                    this.Cursor = Cursors.Default;
                    MessageBox.Show("Error in query: " + ex.Message);
                    return;
                }

                this.Cursor = Cursors.Default;
                log.Debug("Ends-RunReport() method.");
            }
            catch (Exception ex)
            {
                log.Error("Ends-RunReport() method with exception: " + ex.ToString());
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }



        /// <summary>
        /// IsReportDateRangeValid() method
        /// </summary>
        /// <returns></returns>
        private bool IsReportDateRangeValid()
        {
            log.LogMethodEntry();
            bool IsDateRangeValid = false;
            ReportsList reportsList = new ReportsList();
            bool isMaxDateRangeValid = reportsList.IsReportMaxDateRangeValid(fromdate, todate, maxDateRange);
            if (isMaxDateRangeValid)
            {
                IsDateRangeValid = true;
            }
            else
            {
                MessageBox.Show(Common.GetMessage(2643, maxDateRange), Common.GetMessage(2642));
                IsDateRangeValid = false;
            }
            log.LogMethodExit(IsDateRangeValid);
            return IsDateRangeValid;
        }

    }
}
