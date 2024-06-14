/********************************************************************************************
* Project Name - Reports - Report module
* Description  - API for the ReceiptReports.
* 
**************
**Version Log
**************
*Version     Date            Modified By         Remarks          
*********************************************************************************************
*2.110.0     30-Dec-2020     Girish Kundar       Created
*2.110.0     02-Feb-2020     Laster Menezes      Modified method GenerateAndPrintReport to return filepath
                                                 Modified method SetupThePrinting to assign the printdocument name with report name
*2.120       03-May-2021     Laster Menezes      Modified GenerateReceiptReport method to handle 'file not found' and 'null rportsource' exceptions
*2.140.1     07-Apr-2022     Laster Menezes      Issue Fix: Print issue in receipt reports. Receipt Report is printed at the end of the receipt resulting in lengthy paper print.
*2.140.5     28-Apr-2023     Rakshith Shetty     Added new method PrintReceiptReport which lets the user select the printer and then sends the reportSource object for printing.  
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO;
using System.Windows.Forms;
using Telerik.Reporting;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Printer;
using Semnox.Parafait.User;
using Semnox.Parafait.Site;
using System.Linq;
using Semnox.Parafait.Languages;


namespace Semnox.Parafait.Reports
{
    public class ReceiptReports
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private string reportKey;
        private string timeStamp;
        private DateTime? fromDate;
        private DateTime? toDate;
        private List<clsReportParameters.SelectedParameterValue> backgroundParameters;
        private string outputFormat;
        private string reportName;
        private bool isPortrait = false;
        private bool isReceipt = false;
        private bool printContinuous = false;

        public ReceiptReports(ExecutionContext executionContext)
        {
            log.LogMethodEntry();
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        public ReceiptReports(ExecutionContext executionContext, string reportKey, string timeStamp, DateTime? fromDate,
                              DateTime? toDate, List<clsReportParameters.SelectedParameterValue> backgroundParameters,
                              string outputFormat)
            : this(executionContext)
        {
            log.LogMethodEntry();
            this.reportKey = reportKey;
            this.timeStamp = timeStamp;
            this.fromDate = fromDate;
            this.toDate = toDate;
            this.backgroundParameters = backgroundParameters;
            this.outputFormat = outputFormat;
            List<ReportsDTO> reportsDTOsList;
            ReportsList reportsList = new ReportsList(executionContext);
            reportsDTOsList = reportsList.GetReportDTOWithCustomKey(reportKey);
            if (reportsDTOsList == null || reportsDTOsList.Any() == false)
            {
                throw new Exception(MessageContainerList.GetMessage(executionContext, 1308));
            }
            else
            {
                //Report key is Unique
                this.reportName = reportsDTOsList[0].ReportName;
                this.reportKey = reportsDTOsList[0].ReportKey;
                this.isPortrait = reportsDTOsList[0].IsPortrait;
                this.isReceipt = reportsDTOsList[0].IsReceipt;
                this.printContinuous = reportsDTOsList[0].PrintContinuous.ToUpper() == "Y" ? true : false;
            }            
            Common.Utilities = GetUtility();
            log.LogMethodExit();
        }


        /// <summary>
        /// GetUtility()
        /// </summary>
        /// <returns></returns>
        private Utilities GetUtility()
        {
            log.LogMethodEntry();
            Utilities Utilities = new Utilities();
            Utilities.ParafaitEnv.IsCorporate = executionContext.GetIsCorporate();
            Utilities.ParafaitEnv.User_Id = executionContext.GetUserPKId();
            Utilities.ParafaitEnv.LoginID = executionContext.GetUserId();
            Utilities.ParafaitEnv.SetPOSMachine("", Environment.MachineName);
            Utilities.ParafaitEnv.IsCorporate = executionContext.GetIsCorporate();
            Utilities.ParafaitEnv.SiteId = executionContext.GetSiteId();
            Utilities.ExecutionContext.SetIsCorporate(executionContext.GetIsCorporate());
            Utilities.ExecutionContext.SetSiteId(executionContext.GetSiteId());
            Utilities.ExecutionContext.SetUserId(executionContext.GetUserId());
            Utilities.ParafaitEnv.Initialize();
            log.LogMethodEntry();
            return Utilities;
        }


        /// <summary>
        /// Generate and Print Report 
        /// </summary>
        /// <param name="backgroundMode"></param>
        /// <returns>File Path</returns>
        public string GenerateAndPrintReport(bool backgroundMode)
        {
            log.LogMethodEntry(backgroundMode);
            string reportFileName = string.Empty;
            try
            {
                //Update the telerik configs at runtime
                Common.UpdateTelerikConfigs();
                reportFileName = GenerateReceiptReport();
                PrintReceiptReport(reportFileName, backgroundMode);
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit();
                throw new Exception(ex.Message);
            }
            log.LogMethodExit(reportFileName);
            return reportFileName;
        }

        
        /// <summary>
        /// This method returns the MemoryStream which will be converted to File in the API response
        /// </summary>
        /// <returns></returns>
        public MemoryStream GenerateReport()
        {
            log.LogMethodEntry();
            string result = string.Empty;
            try
            {
                string reportFileName = GenerateReceiptReport();
                var dataBytes = File.ReadAllBytes(reportFileName);
                MemoryStream dataStream = new MemoryStream(dataBytes);
                log.LogMethodExit(dataStream);
                return dataStream;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit();
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// SetUp Document for Print
        /// </summary>
        /// <param name="MyPrintDocument"></param>
        /// <param name="backgroundMode"></param>
        /// <returns></returns>
        private bool SetupThePrinting(PrintDocument MyPrintDocument, bool backgroundMode, string reportFileName)
        {
            log.LogMethodEntry(MyPrintDocument);
            try
            {
                PrintDialog MyPrintDialog = new PrintDialog();
                MyPrintDialog.AllowCurrentPage = false;
                MyPrintDialog.AllowPrintToFile = false;
                MyPrintDialog.AllowSelection = false;
                MyPrintDialog.AllowSomePages = false;
                MyPrintDialog.PrintToFile = false;
                MyPrintDialog.ShowHelp = false;
                MyPrintDialog.ShowNetwork = false;
                MyPrintDialog.PrinterSettings.DefaultPageSettings.Landscape = isPortrait ? false :true;
                MyPrintDocument.DocumentName = MessageContainerList.GetMessage(executionContext, reportName);
                MyPrintDialog.UseEXDialog = true;
                if (backgroundMode)
                {
                    log.Debug("Email Mode");
                }
                else if (MyPrintDialog.ShowDialog() != DialogResult.OK)
                {
                    log.LogMethodExit(false);
                    return false;
                }
                MyPrintDocument.PrinterSettings = MyPrintDialog.PrinterSettings;
                MyPrintDocument.DefaultPageSettings = MyPrintDialog.PrinterSettings.DefaultPageSettings;

                if (File.Exists(reportFileName) && isReceipt && printContinuous)
                {
                    //Get the physical size of the PDF file
                    iTextSharp.text.pdf.PdfReader pdfReader = new iTextSharp.text.pdf.PdfReader(reportFileName);
                    iTextSharp.text.Rectangle pSize = pdfReader.GetPageSize(1);
                    //convert the point size to hundredth of an inch
                    MyPrintDocument.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("Custom", (Convert.ToInt32(Math.Round(pSize.Width / 72)) + 1) * 100, (Convert.ToInt32(Math.Round(pSize.Height / 72))) * 100);
                }

                MyPrintDocument.DefaultPageSettings.Margins = new Margins(10, 10, 20, 20);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit(true);
            return true;
        }


        /// <summary>
        /// Print report File 
        /// </summary>
        /// <param name="reportFileName"></param>
        /// <param name="backgroundMode"></param>
        public void PrintReceiptReport(string reportFileName, bool backgroundMode)
        {
            log.LogMethodEntry(reportFileName, backgroundMode);
            try
            {
                using (PrintDocument pd = new PrintDocument())
                {
                    if (SetupThePrinting(pd, backgroundMode, reportFileName))
                    {
                        if (!backgroundMode)
                        {
                            try
                            {
                                PDFFilePrinter pdfFilePrinter = new PDFFilePrinter(pd, reportFileName);
                                pdfFilePrinter.SendPDFToPrinter();
                                pdfFilePrinter = null;
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex);
                                MessageBox.Show(MessageContainerList.GetMessage(executionContext, 1819, ex.Message));//Error while printing the document. Error message: &1
                            }
                        }
                        else
                        {
                            backgroundMode = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(MessageContainerList.GetMessage(executionContext, 1819, ex.Message));//Error while printing the document. Error message: &1
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Generate Report and export into file format
        /// </summary>
        /// <returns></returns>
        private string GenerateReceiptReport()
        {
            log.LogMethodEntry();
            try
            {
                string format = "PDF";
                string extension = "pdf";
                if (outputFormat == "E")
                {
                    format = "XLSX";
                    extension = "xlsx";
                }
                else if (outputFormat == "V")
                {
                    extension = "csv";
                    format = "CSV";
                }
                else if (outputFormat == "H")
                {
                    format = "HTML";
                    extension = "html";
                }

                string path = AppDomain.CurrentDomain.BaseDirectory;
                string reportFilePath = path + "\\Reports\\" + reportKey + ".trdx";
                if(!File.Exists(reportFilePath))
                {
                    throw new Exception(MessageContainerList.GetMessage(executionContext, 3023));
                }

                string filePath = (backgroundParameters != null ? Path.GetTempPath() : ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "PDF_OUTPUT_DIR"));
                string fileURL = filePath + "\\" + reportName + timeStamp + "." + extension;
                ReportSource source = ShowData();
                if(source== null)
                {
                    throw new Exception(MessageContainerList.GetMessage(executionContext, 3024, reportName));
                }
                if (Common.ExportReportData(source, format, fileURL) == false)
                {
                    throw new Exception("Unable to create file");
                }
                log.LogMethodExit();
                return fileURL;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        /// <summary>
        /// Print Report Source
        /// </summary>
        /// <returns>Returns true if Print is successful</returns>

        public bool PrintReceiptReport()
        {
            log.LogMethodEntry();
            try
            {

                Common.UpdateTelerikConfigs();
                ReportSource source = ShowData();
                PrintDialog MyPrintDialog = new PrintDialog();
                MyPrintDialog.AllowCurrentPage = false;
                MyPrintDialog.AllowPrintToFile = false;
                MyPrintDialog.AllowSelection = false;
                MyPrintDialog.AllowSomePages = false;
                MyPrintDialog.PrintToFile = false;
                MyPrintDialog.ShowHelp = false;
                MyPrintDialog.ShowNetwork = false;
                MyPrintDialog.PrinterSettings.DefaultPageSettings.Landscape = isPortrait ? false : true;
                MyPrintDialog.UseEXDialog = true;
                if (source != null)
                {
                    if (MyPrintDialog.ShowDialog() != DialogResult.OK)
                    {
                        log.LogMethodExit(false);
                        return false;
                    }
                    if (Common.PrintReportData(source, MyPrintDialog.PrinterSettings.PrinterName) == false)
                    {

                        throw new Exception("Unable to print report");

                    };
                    log.LogMethodExit();
                    return true;
                }
                else
                {
                    throw new Exception("Report Source Not Found");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;

            }
        }


        /// <summary>
        /// Generate report source 
        /// </summary>
        /// <returns>telerik report source of the report</returns>
        private Telerik.Reporting.ReportSource ShowData()
        {
            log.LogMethodEntry();
            LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
            try
            {
                DateTime startTime = lookupValuesList.GetServerDateTime();
                List<clsReportParameters.SelectedParameterValue> reportParameters = new List<clsReportParameters.SelectedParameterValue>();
                reportParameters.Add(new clsReportParameters.SelectedParameterValue("user", executionContext.GetUserId()));
                reportParameters.Add(new clsReportParameters.SelectedParameterValue("site", executionContext.GetSiteId()));
                UserContainerDTO userContainerDTO = UserContainerList.GetUserContainerDTOOrDefault(executionContext.GetUserId(), "", executionContext.GetSiteId());
                int roleId = -1;
                if (userContainerDTO != null && userContainerDTO.ManagerId != -1)
                {
                    roleId = userContainerDTO.RoleId;
                }
                reportParameters.Add(new clsReportParameters.SelectedParameterValue("role", roleId));
                reportParameters.Add(new clsReportParameters.SelectedParameterValue("type", 1));
                reportParameters.Add(new clsReportParameters.SelectedParameterValue("offset", 0));
                reportParameters.Add(new clsReportParameters.SelectedParameterValue("loggedInUserId", executionContext.GetUserPKId()));
                reportParameters.Add(new clsReportParameters.SelectedParameterValue("isCorporate", executionContext.IsCorporate));
                reportParameters.Add(new clsReportParameters.SelectedParameterValue("LangID", executionContext.LanguageId));
                string siteName = null;
                if (executionContext.GetSiteId() == -1)
                {
                    SiteList siteList = new SiteList(executionContext);
                    List<KeyValuePair<SiteDTO.SearchBySiteParameters, string>> searchParameters = new List<KeyValuePair<SiteDTO.SearchBySiteParameters, string>>();
                    searchParameters.Add(new KeyValuePair<SiteDTO.SearchBySiteParameters, string>(SiteDTO.SearchBySiteParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                    List<SiteDTO> siteDTOList = siteList.GetAllSites(searchParameters);
                    if (siteDTOList != null && siteDTOList.Any())
                    {
                        siteName = siteDTOList[0].SiteName;
                    }
                }
                else
                {
                    Semnox.Parafait.Site.Site site = new Semnox.Parafait.Site.Site(executionContext.GetSiteId(), null);
                    siteName = site.getSitedTO.SiteName;
                }
                reportParameters.Add(new clsReportParameters.SelectedParameterValue("SiteName", siteName));               
                reportParameters.Add(new clsReportParameters.SelectedParameterValue("mode", "F"));
                reportParameters.Add(new clsReportParameters.SelectedParameterValue("NumericCellFormat", "{0:" + ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "NUMBER_FORMAT") + "}"));
                reportParameters.Add(new clsReportParameters.SelectedParameterValue("AmountCellFormat", "{0:" + ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "AMOUNT_FORMAT") + "}"));
                reportParameters.Add(new clsReportParameters.SelectedParameterValue("DateTimeCellFormat", "{0:" + ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "DATETIME_FORMAT") + "}"));
                reportParameters.Add(new clsReportParameters.SelectedParameterValue("AmountWithCurSymbolCellFormat", "{0:" + ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "AMOUNT_WITH_CURRENCY_SYMBOL") + "}"));
                reportParameters.Add(new clsReportParameters.SelectedParameterValue("ENABLE_POS_FILTER_IN_TRX_REPORT", ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ENABLE_POS_FILTER_IN_TRX_REPORT")));
                reportParameters.Add(new clsReportParameters.SelectedParameterValue("EXCLUDE_PRODUCT_BREAKDOWN_IN_TRX_REPORT", ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "EXCLUDE_PRODUCT_BREAKDOWN_IN_TRX_REPORT")));
                reportParameters.Add(new clsReportParameters.SelectedParameterValue("EXCLUDE_ZERO_PRICE_SALE_IN_TRX_REPORT", ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "EXCLUDE_ZERO_PRICE_SALE_IN_TRX_REPORT")));
                reportParameters.Add(new clsReportParameters.SelectedParameterValue("EXCLUDE_SPECIAL_PRICING_IN_TRX_REPORT", ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "EXCLUDE_SPECIAL_PRICING_IN_TRX_REPORT")));
                reportParameters.Add(new clsReportParameters.SelectedParameterValue("INVENTORY_QUANTITY_FORMAT", ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "INVENTORY_QUANTITY_FORMAT")));
                reportParameters.Add(new clsReportParameters.SelectedParameterValue("INVENTORY_COST_FORMAT", ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "INVENTORY_COST_FORMAT")));

                List<clsReportParameters.SelectedParameterValue> lstAuditReportParameters = new List<clsReportParameters.SelectedParameterValue>();
                lstAuditReportParameters.Add(new clsReportParameters.SelectedParameterValue("Fromdate", fromDate));
                lstAuditReportParameters.Add(new clsReportParameters.SelectedParameterValue("Todate", toDate));
                lstAuditReportParameters.Add(new clsReportParameters.SelectedParameterValue("Offset", Common.Offset));
                lstAuditReportParameters.Add(new clsReportParameters.SelectedParameterValue("Site", executionContext.GetSiteId()));
                reportParameters.Add(new clsReportParameters.SelectedParameterValue("loginUserId", executionContext.GetUserId()));
                string selectedUser = "";

                string passPhrase = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CUSTOMER_ENCRYPTION_PASS_PHRASE");
                reportParameters.Add(new clsReportParameters.SelectedParameterValue("CustomerPassPhrase", passPhrase));

                if (reportKey.ToLower() == "bookingreceipt" || reportKey.ToLower() == "bookingreceiptcustom")
                {
                    List<LookupValuesDTO> lookupValuesDTOList = new List<LookupValuesDTO>();
                    List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchlookupParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                    searchlookupParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "BOOKING_RECEIPT_TEMPLATE_FIELDS"));
                    searchlookupParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, (Common.Utilities.ParafaitEnv.IsCorporate ? Common.Utilities.ParafaitEnv.SiteId : -1).ToString()));
                    lookupValuesDTOList = new LookupValuesList(executionContext).GetAllLookupValues(searchlookupParameters);
                    if (lookupValuesDTOList != null)
                    {
                        foreach (LookupValuesDTO lookup in lookupValuesDTOList)
                        {
                            reportParameters.Add(new clsReportParameters.SelectedParameterValue(lookup.LookupValue, lookup.Description));
                        }
                    }
                    else
                    {
                        log.Error("BOOKING_RECEIPT_TEMPLATE_FIELDS lookup values are not set to check the message send status");
                    }
                }

                if (backgroundParameters != null)
                {
                    foreach (clsReportParameters.SelectedParameterValue param in backgroundParameters)
                    {
                        reportParameters.Add(new clsReportParameters.SelectedParameterValue(param.parameterName, param.parameterValue));
                        lstAuditReportParameters.Add(new clsReportParameters.SelectedParameterValue(param.parameterName, param.parameterValue[0]));
                    }
                }

                string message = "";
                ReportSource reportSource = Common.GetReportSource(reportKey, reportName, Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), new System.Collections.ArrayList { executionContext.GetSiteId() }, ref message, reportParameters, true);

                string strParameters = string.Empty;
                foreach (clsReportParameters.SelectedParameterValue AuditParam in lstAuditReportParameters)
                {
                    if(AuditParam.parameterValue[0] != null)
                    {
                        //fetching the string parameter value for the parameters list
                        strParameters += "@" + AuditParam.parameterName + "='" + AuditParam.parameterValue[0].ToString() + "';";
                    }                    
                }

                DateTime endTime = DateTime.Now;
                RunReportAuditDTO runReportAuditDTO = new RunReportAuditDTO();
                runReportAuditDTO.StartTime = startTime;
                runReportAuditDTO.EndTime = endTime;
                runReportAuditDTO.ReportKey = reportKey;
                runReportAuditDTO.ReportId = Common.GetReportId(reportKey, executionContext.GetSiteId());
                runReportAuditDTO.CreationDate = lookupValuesList.GetServerDateTime();
                runReportAuditDTO.LastUpdateDate = lookupValuesList.GetServerDateTime();
                runReportAuditDTO.ParameterList = strParameters;
                runReportAuditDTO.LastUpdatedBy = executionContext.GetUserId();
                runReportAuditDTO.SiteId = executionContext.GetSiteId();
                runReportAuditDTO.Message = message;
                runReportAuditDTO.Source = "R";
                RunReportAudit runReportAudit = new RunReportAudit(runReportAuditDTO);
                runReportAudit.Save();
                log.LogMethodExit();
                return reportSource;
            }
            catch (Exception ex)
            {
                log.Error("Ends-ShowData() method with exception: " + ex.ToString());
                throw ex;
            }
        }
        
    }
}
