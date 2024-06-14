/********************************************************************************************
*Project Name - Parafait Report                                                                          
*Description  - RunBackground
*************
**Version Log
*************
*Version     Date                   Modified By                 Remarks          
*********************************************************************************************
*2.80       18-Sep-2019             Dakshakh raj                Modified : Added logs      
*2.110      22-Dec-2020             Laster Menezes              New report 'TransactionByCollectionDate' changes. Modified methods runSchedule,CreateBackgroundReport
*2.110      09-Mar-2021             Laster Menezes              Adding new Sales, Reconciliation TRDX reports
*2.120      23-Mar-2021             Laster Menezes              Modified runSchedule, CreateBackgroundReport - Added new Report key InventoryWithCategorySearch
*********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Reports;

namespace Semnox.Parafait.Report.Reports
{

    /// <summary>
    /// RunBackground class
    /// </summary>
    public static class RunBackground
    {

        /// <summary>
        ///  Semnox.Core.Logger
        /// </summary>
        public static Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// ExecutionContext
        /// </summary>
        public static ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();

        /// <summary>
        /// runSchedule method
        /// </summary>
        /// <returns>returns bool</returns>
        public static bool runSchedule(int schedule_id, string frequency, decimal run_at, double include_data_for, ref string message)
        {
            log.LogMethodEntry(schedule_id, frequency, run_at, include_data_for, message);
            try
            {
                Common.GlobalScheduleId = schedule_id;
                log.Info("Schedule ID:" + Common.GlobalScheduleId.ToString());
                ReportScheduleReportsList reportScheduleReportsList = new ReportScheduleReportsList();
                DataTable dt = new DataTable();
                log.Info("Getting list of report schedule reports.");
                dt = reportScheduleReportsList.GetReportScheduleReportsByScheduleIDAll(schedule_id);

                DateTime toDate;
                DateTime fromDate;
                double businessDayStartTime = 6;
                double.TryParse(Common.Utilities.getParafaitDefaults("BUSINESS_DAY_START_TIME"), out businessDayStartTime);

                if (frequency == "100") // monthly
                {
                    toDate = DateTime.Now.Date.AddHours(businessDayStartTime); // run until 6AM of first day of this month
                    fromDate = DateTime.Now.Date.AddMonths(-1).AddHours(businessDayStartTime); // since first day of last month
                }
                else
                    if (include_data_for <= 0)
                {
                    int wholeNumber = (int)run_at;
                    int decimalPortion = (int)((run_at - (int)run_at) * 100);


                    toDate = DateTime.Now.Date.AddHours(wholeNumber);

                    toDate = toDate.AddMinutes(decimalPortion);

                    //toDate = DateTime.Now.Date.AddHours(run_at); // run until schedule run time since 6AM
                    fromDate = DateTime.Now.Date.AddHours(businessDayStartTime);
                }
                else if (frequency == "1001") // first day of month
                {
                    toDate = DateTime.Now.Date.AddHours(businessDayStartTime); // run until 6AM today
                    int prevDay = toDate.AddDays(-1).Day;
                    fromDate = toDate.AddDays(31 - prevDay - include_data_for);
                }
                else
                {
                    //toDate = DateTime.Now.Date.AddHours(businessDayStartTime); // run until 6AM today
                    //fromDate = toDate.AddDays(-1 * include_data_for);
                    toDate = DateTime.Now.Date.AddHours(businessDayStartTime); // run until 6AM today
                    fromDate = toDate.AddDays(-1 * include_data_for);
                }

                appendMessage("From Date: " + fromDate.ToString(Common.ParafaitEnv.DATETIME_FORMAT), ref message);
                log.Info("From Date: " + fromDate.ToString(Common.ParafaitEnv.DATETIME_FORMAT));
                appendMessage("To Date: " + toDate.ToString(Common.ParafaitEnv.DATETIME_FORMAT), ref message);
                log.Info("To Date: " + toDate.ToString(Common.ParafaitEnv.DATETIME_FORMAT));

                if (dt == null)
                {
                    appendMessage("No reports specified in this schedule", ref message);
                    log.Info("No reports specified in this schedule");
                    return false;
                }

                DateTime time = DateTime.Now;
                string TimeStamp = DateTime.Now.ToString("yyyy-MMM-dd Hmm") + "-" + schedule_id.ToString();
                TimeStamp = CentralTimeZone.getLocalTime(time, 0).ToString("yyyy-MMM-dd Hmm") + "-" + schedule_id.ToString();
                string CSVTimestamp = "";
                try
                {
                    if (Common.Utilities.getParafaitDefaults("CSV_REPORTNAME_TIMESTAMP_FORMAT") != null && Common.Utilities.getParafaitDefaults("CSV_REPORTNAME_TIMESTAMP_FORMAT") != "")
                        CSVTimestamp = time.ToString(Common.Utilities.getParafaitDefaults("CSV_REPORTNAME_TIMESTAMP_FORMAT"));
                }
                catch
                {
                    CSVTimestamp = "";
                }

                string scheduleName = "";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Common.GlobalReportId = Convert.ToInt32(dt.Rows[i]["report_id"]);
                    Common.GlobalReportScheduleReportId = Convert.ToInt32(dt.Rows[i]["report_schedule_report_id"]);
                    string reportKey = dt.Rows[i]["report_key"].ToString();
                    string reportName = dt.Rows[i]["report_name"].ToString();
                    log.Info("Report Name:" + reportName);
                    string customFlag = dt.Rows[i]["CustomFlag"].ToString();
                    string soutputformat = dt.Rows[i]["soutputformat"].ToString();
                    string routputformat = dt.Rows[i]["routputformat"].ToString();
                    int reportID = Convert.ToInt32(dt.Rows[i]["report_id"]);
                    scheduleName = dt.Rows[i]["schedule_name"].ToString();

                    log.Info("Before calling form:  GlobalReportId" + Common.GlobalReportId + "  GlobalReportScheduleReportId " + Common.GlobalReportScheduleReportId + " reportKey " + reportKey);

                    log.Info("Before calling form:  routputformat" + routputformat + "  reportName " + reportName);
                    log.Info("from date " + fromDate + "  toDate " + toDate + "  TimeStamp " + TimeStamp);
                    log.Info("from date " + fromDate + "  toDate " + toDate + "  TimeStamp " + TimeStamp);
                    if (dt.Rows[i]["customFlag"].ToString() != "Y")
                    {
                        string formName = "";

                        switch (reportKey)
                        {
                            case "Transaction":
                            case "CollectionChart":
                            case "TrxSummary":
                            case "PaymentModeBreakdownTransaction":
                            case "CashierBreakdownTransaction":
                            case "ProductBreakdownTransaction":
                            case "DiscountBreakdownTransaction":
                            case "SpecialPricingTransaction":
                            case "TaxBreakdownTransaction":
                            case "POSMachineBreakdownTransaction":
                            case "CardTransfersTransaction":
                            case "CardActivitiesTransaction":
                            case "DisplayGroupBreakdownTransaction":
                            case "TransactionWithConceptTypesAndArea":
                            case "SalesSummary":
                            case "TransactionByCollectionDate":
                            case "Sales":
                            case "Reconciliation":
                                formName = "TransactionReportviewer"; break;
                            case "GameMetric":
                            case "GamePerformance":
                            case "GameLevelRevenueSummary":
                            case "TrafficChart":
                                formName = "GamePlayReportviewer"; break;
                            case "Inventory":
                            case "PurchaseOrder":
                            case "ReceivedInventory":
                            case "InvAdj":
                            case "InventoryStatus":
                            case "InventoryWithCategorySearch":
                                formName = "SKUSearchReportviewer"; break;
                            case "OpenToBuy":
                            case "Top15WeeklyUnitSales":
                            case "DepartmentSelling_MTD_YTD":
                            case "InventoryAgingReport":
                                formName = "RetailReportviewer"; break;
                            default: formName = "GenericReportviewer"; break;
                        }
                        log.Info("Before calling ReportsCommon.openForm ");
                        log.Info("formName " + formName);
                        ReportsCommon.openForm(null, formName, new object[] { true, reportID, reportKey, reportName, TimeStamp, fromDate, toDate, null, "P" }, true, true);
                        log.Info("After calling ReportsCommon.openForm ");
                    }
                    else
                    {
                        //         public CustomReportviewer(bool BackgroundMode, string ReportName, string TimeStamp, int ReportID, DateTime FromDate, DateTime ToDate, int ReportScheduleReportId)
                        log.Info("Before calling CustomReportviewer");
                        ReportsCommon.openForm(null, "CustomReportviewer", new object[] { true, reportKey, reportName, TimeStamp.ToString(), reportID, fromDate, toDate, Common.GlobalReportScheduleReportId, null, "P" }, true, true);
                        // ReportsCommon.openForm(null, "CustomReportviewer", new object[] { true,(long)Common.GlobalReportId, fromDate, toDate, true, TimeStamp }, true, true);
                        appendMessage("Run Custom Report: " + dt.Rows[i]["report_name"].ToString(), ref message);
                        log.Info("Run Custom Report: " + dt.Rows[i]["report_name"].ToString());
                    }
                }
                string reportDirectory = Common.Utilities.getParafaitDefaults("PDF_OUTPUT_DIR") + "\\";
                DirectoryInfo di = new DirectoryInfo(reportDirectory);
                string pattern = "*" + TimeStamp + "*";
                FileInfo[] pdfFiles = di.GetFiles(pattern);

                if (pdfFiles.Count() == 0)
                {

                    message += "No files Created Or Empty Data.";
                    return false;
                }
                else
                {
                    appendMessage("Done creating reports", ref message);
                    log.Info("Done creating reports");
                }


                Application.DoEvents();
                ReportScheduleEmailList reportScheduleEmailList = new ReportScheduleEmailList();
                dt = new DataTable();
                dt = reportScheduleEmailList.GetReportScheduleEmailListByScheduleID(schedule_id);

                if (dt == null || dt.Rows.Count == 0)
                {
                    appendMessage("No email ids specified to send mail", ref message);
                    log.Info("No email ids specified to send mail");
                    return true;
                }

                appendMessage("Emailing reports...", ref message);
                log.Info("Emailing reports...");

                string ToEmails = "";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ToEmails += ", " + dt.Rows[i]["emailid"].ToString();
                }
                string subject = Common.GlobalReportScheduleReportId > 0 ? "Parafait Schedule Report " : " Background Email";
                subject += " -" + scheduleName;
                ToEmails = ToEmails.Substring(2);
                bool retVal = false;
                try
                {
                    retVal = Common.sendEmail(TimeStamp, ToEmails, Common.Utilities, subject, ref message);

                }
                catch (Exception ex)
                {
                    appendMessage("Error: emailing reports", ref message);
                    log.Error("Error1:   emailing reports ");
                    appendMessage(ex.Message, ref message);
                    log.Info(ex.Message);
                    retVal = false;
                }
                log.LogMethodExit(retVal);
                return retVal;
            }
            catch (Exception ex)
            {
                appendMessage(ex.Message, ref message);
                log.Info(ex.Message); 
                log.Error("Error2: Exception reports", ex);
                log.Info(ex.StackTrace);
                log.LogMethodExit(false);
                return false;
            }
        }

        static void appendMessage(string appendMessage, ref string message)
        {
            log.LogMethodEntry(appendMessage, message);
            message += appendMessage + Environment.NewLine;
            log.LogMethodExit();
        }



        /// <summary>
        /// CreateBackgroundReport method
        /// </summary>
        /// <param name="reportKey"></param>
        /// <param name="timeStamp"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="lstBackgroundParams"></param>
        /// <param name="outputFormat"></param>
        public static void CreateBackgroundReport(string reportKey, string timeStamp, DateTime fromDate, DateTime toDate, List<clsReportParameters.SelectedParameterValue> lstBackgroundParams, string outputFormat = "P")
        {
            log.LogMethodEntry(reportKey, timeStamp, fromDate, toDate, lstBackgroundParams, outputFormat);
            try
            {

                ReportsDTO reportsDTO = new ReportsDTO();
                reportsDTO = GetReportDTO(reportKey);
                if (reportsDTO == null || reportsDTO.ReportId == -1)
                {
                    throw new Exception(MessageContainerList.GetMessage(Common.Utilities.ExecutionContext, 1308));
                }
                reportKey = reportsDTO.ReportKey;
                string reportName = reportsDTO.ReportName;
                int reportId = reportsDTO.ReportId;
                Common.GlobalReportId = reportsDTO.ReportId;
                double businessDayStartTime = 6;
                double.TryParse(Common.Utilities.getParafaitDefaults("BUSINESS_DAY_START_TIME"), out businessDayStartTime);

                fromDate = ((fromDate == null || fromDate == DateTime.MinValue) ? DateTime.Now.Date.AddHours(businessDayStartTime) : fromDate); // run until 6AM today
                toDate = ((toDate == null || toDate == DateTime.MinValue) ? DateTime.Now : toDate);

                string formName = "";
                if (reportsDTO != null && reportsDTO.CustomFlag != null)
                    if (reportsDTO.CustomFlag == "N")
                    {
                        switch (reportKey)
                        {
                            case "Transaction":
                            case "CollectionChart":
                            case "TrxSummary":
                            case "PaymentModeBreakdownTransaction":
                            case "CashierBreakdownTransaction":
                            case "ProductBreakdownTransaction":
                            case "DiscountBreakdownTransaction":
                            case "SpecialPricingTransaction":
                            case "TaxBreakdownTransaction":
                            case "POSMachineBreakdownTransaction":
                            case "CardTransfersTransaction":
                            case "CardActivitiesTransaction":
                            case "DisplayGroupBreakdownTransaction":
                            case "TransactionWithConceptTypesAndArea":
                            case "SalesSummary":
                            case "TransactionByCollectionDate":
                            case "Sales":
                            case "Reconciliation":
                                formName = "TransactionReportviewer"; break;
                            case "GameMetric":
                            case "GamePerformance":
                            case "GameLevelRevenueSummary":
                            case "TrafficChart":
                                formName = "GamePlayReportviewer"; break;
                            case "Inventory":
                            case "PurchaseOrder":
                            case "ReceivedInventory":
                            case "InvAdj":
                            case "InventoryStatus":
                            case "InventoryWithCategorySearch":
                                formName = "SKUSearchReportviewer"; break;
                            case "OpenToBuy":
                            case "Top15WeeklyUnitSales":
                            case "DepartmentSelling_MTD_YTD":
                            case "InventoryAgingReport":
                                formName = "RetailReportviewer"; break;
                            default: formName = "GenericReportviewer"; break;
                        }
                        log.Info("Before calling ReportsCommon.openForm ");
                        log.Info("formName " + formName);
                        ReportsCommon.openForm(null, formName, new object[] { true, reportId, reportKey, reportName, timeStamp, fromDate, toDate, lstBackgroundParams, outputFormat }, true, true);
                        log.Info("After calling ReportsCommon.openForm ");
                    }
                    else
                    {
                        log.Info("Before calling CustomReportviewer");
                        ReportsCommon.openForm(null, "CustomReportviewer", new object[] { true, reportKey, reportName, timeStamp, reportId, fromDate, toDate, -1, lstBackgroundParams, outputFormat }, true, true);
                        log.Info("Run Custom Report: " + reportName);
                    }
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit();
                throw new Exception(ex.Message);
            }
        }


        /// <summary>
        /// GetReportDTO
        /// </summary>
        /// <param name="reportKey"></param>
        /// <returns></returns>
        private static ReportsDTO GetReportDTO(string reportKey)
        {
            log.LogMethodEntry(reportKey);
            string customReportKey = reportKey + "Custom"; 
            ReportsDTO reportsDTO = new ReportsDTO();
            ReportsList reportsList = new ReportsList();
            reportsDTO = reportsList.GetReportsByReportKey(customReportKey);
            if (reportsDTO == null)
            {
                reportsDTO = reportsList.GetReportsByReportKey(reportKey);
            }
            log.LogMethodExit(reportsDTO);
            return reportsDTO;
        }

    }
}
