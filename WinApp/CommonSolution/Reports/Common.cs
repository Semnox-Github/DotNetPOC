/********************************************************************************************
 * Project Name - Common
 * Description  - Common class
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *1.00        14-Dec-2018      Raghuveera         Modified for getting encrypted key value 
 *2.70        29-Jul-2019      Laster Menezes     Modified for updating the logic of grouping multiple breakcolumn, Aggregate the columns based on user requirement
 *2.80        15-Jun-2020      Laster Menezes     Modified the custom report logic to include RepeatBreakColumn implementation 
 * 2.90       24-Jul-2020      Laster Menezes     Inluded new methods MergeReportFiles, TotalPDFPageCount, GetFileCountByTimeStampAndExtention, BuildQueryString
 * 2.100      28-Sep-2020      Laster Menezes     Modified GetCustomReportSource to pass Background color and header Text color to CreateCustomReport and 
                                                  CreatePOSCustomReport templates
 *2.100       15-Oct-2020      Laster Menezes     Modified GetCustomReportSource, added new method GetGroupIndexedDatatable to implement Table group pagination in custom reports
 *2.110       28-Dec-2020      Laster Menezes     Modified GetReportSource to allow the Trdx report to print in continuous receipt format when 
                                                  PrintContinuous field of Reports is enabled
 *2.110       09-Mar-2021      Laster Menezes     Adding new method getParafaitDefaults
 *2.120       08-Apr-2021      Laster Menezes     Added method IsTrdxReportParametersVisible
 *2.120       27-Apr-2021      Laster menezes     Modified initEnv method to set the siteid and IsCorporate fields of ParafaitEnv before initializing
 *2.120       03-May-2021      Laster Menezes     Modified GetReportSource method to include Paper PageSize formatting for TRDX reports
 *2.120.1     21-Jul-2021      Laster Menezes     Modified sendEmail method to use getParafaitDefaults method of Common class instead of container class method
 *2.140.0     01-Dec-2021      Laster Menezes     Modified method GetReportFileName - Timestamp improvements
 *2.140.0     04-Jan-2022      Laster Menezes     Modified GetAggregateColumnList method to handle whitespace characters in AggregateColumns for custom reports
 *2.140.5     03-Feb-2023      Rakshith Shetty    Modified InitEnv Method to initialize execution context object.
 *2.140.5     28-Apr-2023      Rakshith Shetty    Added PrintReportData method to print the report source of receipt reports to the printer selected by the users. 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Net.Mail;
using System.IO;
using System.Net;
using System.Data;
using System.Data.OleDb;
using System.Collections;
using System.Xml;
using Telerik.Reporting;
using System.Drawing;
using System.Configuration;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using System.Collections.Specialized;
using System.Drawing.Printing;

namespace Semnox.Parafait.Reports
{
    /// <summary>
    /// Common Class
    /// </summary>
    public static class Common
    {
        /// <summary>
        /// Log
        /// </summary>
        public static logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SkinColor Property
        /// </summary>
        public static System.Drawing.Color SkinColor = System.Drawing.Color.White;


        /// <summary>
        /// GlobalScheduleId Property
        /// </summary>
        public static int GlobalScheduleId = -1;

        /// <summary>
        /// GlobalReportId Property
        /// </summary>
        public static int GlobalReportId = -1;

        /// <summary>
        /// GlobalReportScheduleReportId Property
        /// </summary>
        public static int GlobalReportScheduleReportId = -1;


        /// <summary>
        /// Utilities Property
        /// </summary>
        public static Utilities Utilities;

        /// <summary>
        /// ParafaitEnv Property
        /// </summary>
        public static ParafaitEnv ParafaitEnv;


        /// <summary>
        /// MessageUtils Property
        /// </summary>
        public static MessageUtils MessageUtils;


        /// <summary>
        /// ConnectionString Property
        /// </summary>
        public static string ConnectionString = string.Empty;


        /// <summary>
        /// CompanyName Property
        /// </summary>
        public static string CompanyName = string.Empty;

        /// <summary>
        /// BusinessDayStartTime Property
        /// </summary>
        public static double BusinessDayStartTime = 6;


        /// <summary>
        /// Offset Property
        /// </summary>
        public static int Offset = 0;

        /// <summary>
        /// DBName Property
        /// </summary>
        public static string DBName = string.Empty;

        /// <summary>
        /// LanguageID Property
        /// </summary>
        public static long LanguageID = -1;

        /// <summary>
        /// Mode Property
        /// </summary>
        public static string Mode = "F";

        /// <summary>
        /// BackgroundMode Property
        /// </summary>
        public static bool BackgroundMode = false;

        /// <summary>
        /// arrSortBy DataTable
        /// </summary>
        public static DataTable dtSortData = new DataTable();

        /// <summary>
        /// sortDirection Property
        /// </summary>
        public static string sortDirection = string.Empty;

        /// <summary>
        /// sortFileld Property
        /// </summary>
        public static string sortField = string.Empty;


        /// <summary>
        /// dataRowGrandTotal Property
        /// </summary>
        public static object[] dataRowGrandTotal = new object[150];


        /// <summary>
        /// customReportColumnCount Property
        /// </summary>
        public static int customReportColumnCount = 0;


        /// <summary>
        /// dtSortTableColumns DataTable
        /// </summary>
        public static DataTable dtSortTableColumns = new DataTable();

        /// <summary>
        /// amountFormatDecimalPoints property
        /// </summary>
        public static int amountFormatDecimalPoints = 0;

        /// <summary>
        /// lstBackgroundReportParams property
        /// </summary>
        public static List<clsReportParameters.SelectedParameterValue> lstBackgroundReportParams;

        /// <summary>
        /// prevBreakValue property
        /// </summary>
        public static string prevBreakValue = string.Empty;

        /// <summary>
        /// GrandaggrColumnValuesFirstBreakCol property
        /// </summary>
        public static double[] GrandaggrColumnValuesFirstBreakCol = new double[150];

        /// <summary>
        /// cultureCode property
        /// </summary>
        public static string cultureCode = string.Empty;

        /// <summary>
        /// initEnv Method
        /// </summary>
        public static void initEnv()
        {
            log.LogMethodEntry();
            try
            {
                Utilities = new Utilities();
                Utilities.ParafaitEnv.Initialize();
                ParafaitEnv = Utilities.ParafaitEnv;
                MessageUtils = Utilities.MessageUtils;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// LoadMessages Method
        /// </summary>
        public static void LoadMessages()
        {
            log.LogMethodEntry();
            try
            {
                LanguageID = ParafaitEnv.LanguageId;
                if (ParafaitEnv.IsCorporate)
                {
                    MessagesFunctions.CreateMessageList(ParafaitEnv.LanguageName, ParafaitEnv.SiteId);
                }
                else
                {
                    MessagesFunctions.CreateMessageList(ParafaitEnv.LanguageName, -1);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// initEnv method
        /// </summary>
        /// <param name="connectionString">ConnectionString</param>
        public static void initEnv(string connectionString)
        {
            log.LogMethodEntry(connectionString);
            try
            {
                Utilities = new Utilities(connectionString);
                ExecutionContext executionContext = ExecutionContext.GetExecutionContext();

                if (Convert.ToInt32(Utilities.executeScalar(@"select count(1) from site")) > 1)
                {
                    Utilities.ParafaitEnv.SiteId = Convert.ToInt32(Utilities.executeScalar(@"select master_site_id from company"));
                    Utilities.ParafaitEnv.IsCorporate = true;
                    executionContext.SetIsCorporate(true);
                    executionContext.SetSiteId(Utilities.ParafaitEnv.SiteId);
                    executionContext.SetSitePKId(Utilities.ParafaitEnv.SiteId);
                    executionContext.SetUserId(Convert.ToString(Utilities.ParafaitEnv.User_Id));
                    executionContext.SetMachineId(Utilities.ParafaitEnv.POSMachineId);
                }
                else
                {
                    Utilities.ParafaitEnv.SiteId = -1;
                    Utilities.ParafaitEnv.IsCorporate = false;
                }

                Utilities.ParafaitEnv.Initialize();
                ConnectionString = connectionString;
                ReportServerGenericClass reportServerGenericClass = new ReportServerGenericClass(connectionString);

                if (reportServerGenericClass.GetSiteId() != -1)
                    Utilities.ParafaitEnv.IsCorporate = true;

                ParafaitEnv = Utilities.ParafaitEnv;
                MessageUtils = Utilities.MessageUtils;
                LoadMessages();
                SetSiteCulture();

            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// getReportOutputFormat method
        /// </summary>
        /// <returns></returns>
        public static string getReportOutputFormat()
        {
            log.LogMethodEntry();
            try
            {
                ReportsDataHandler ReportsDataHandler = new ReportsDataHandler();
                log.LogMethodExit();
                return ReportsDataHandler.getReportOutputFormat(Common.GlobalReportScheduleReportId);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit();
                return "";
            }
        }


        /// <summary>
        /// WaitForFileCreation method
        /// </summary>
        /// <param name="file_name">file_name</param>
        /// <returns></returns>
        public static bool WaitForFileCreation(string file_name)
        {
            log.LogMethodEntry(file_name);
            int MaxCount = 360; // around 3 minutes
            while (MaxCount-- > 0)
            {
                if (System.IO.File.Exists(file_name)) // wait till file is created
                {
                    try
                    {
                        using (System.IO.Stream stream = new System.IO.FileStream(file_name, System.IO.FileMode.Open))
                        {
                            log.LogMethodExit();
                            return true;
                        }
                    }
                    catch { }
                }
                System.Threading.Thread.Sleep(500);
            }
            log.LogMethodExit();
            return false;
        }


        /// <summary>
        /// GetReportFileName method
        /// </summary>
        /// <param name="reportName">reportName</param>
        /// <param name="timeStamp">timeStamp</param>
        /// <param name="extension">extension</param>
        /// <returns>returns string</returns>
        public static string GetReportFileName(string reportName, string timeStamp, string extension)
        {
            log.LogMethodEntry(reportName, timeStamp, extension);
            if(GlobalScheduleId != -1 && timeStamp != "")
            {
                timeStamp = " - " + timeStamp;
            }
            log.LogMethodExit();
            return Common.Utilities.getParafaitDefaults("PDF_OUTPUT_DIR") + "\\" + reportName  + timeStamp + "." + extension;
        }


        /// <summary>
        ///  createHTMLFile method
        /// </summary>
        /// <param name="dg">dg</param>
        /// <param name="titleText">titleText</param>
        /// <param name="fileName">fileName</param>
        /// <returns>returns bool</returns>
        public static bool createHTMLFile(DataGridView dg, string titleText, string fileName)
        {
            log.LogMethodEntry(dg, titleText, fileName);
            try
            {
                System.IO.StreamWriter sw = new System.IO.StreamWriter(fileName, true, Encoding.UTF8); //creating html file
                //write datagridview contents to HTML file
                sw.Write(htmlMessageBody(dg, titleText).ToString());
                sw.Close();
                log.LogMethodExit();
                return true;
            }
            catch
            {
                log.LogMethodExit();
                return false;
            }
        }

        /// <summary>
        /// htmlMessageBody method
        /// </summary>
        /// <param name="dg">dg</param>
        /// <param name="titleText">titleText</param>
        /// <returns>returns StringBuilder</returns>
        public static StringBuilder htmlMessageBody(DataGridView dg, string titleText)
        {
            log.LogMethodEntry(dg, titleText);
            StringBuilder strB = new StringBuilder();
            string resultText = "<p>" + titleText
                                    .Replace(Environment.NewLine + Environment.NewLine, "</p><p>")
                                    .Replace(Environment.NewLine, "<br />")
                                    .Replace("</p><p>", "</p>" + Environment.NewLine + "<p>") + "</p>";

            //create html & table
            strB.AppendLine("<html><body><center><b>" + resultText + "</b><" +
                            "table style=\"font-family: arial; font-size:9pt; border-width:1px; border-spacing:0px; border-collapse:collapse; border-color: gray; border-style:solid;\" cellpadding='0' cellspacing='0'>");
            strB.AppendLine("<tr>");
            //cteate table header
            for (int i = 0; i < dg.Columns.Count; i++)
            {
                if (dg.Columns[i].Visible)
                    strB.AppendLine("<td style=\"border-width:1px; padding:0px; border-color:gray; border-style:solid;\" align='center' valign='middle' bgcolor=\"#ADD8E6\">" +
                                dg.Columns[i].HeaderText + "</td>");
            }
            //create table body
            strB.AppendLine("<tr>");
            for (int i = 0; i < dg.Rows.Count; i++)
            {
                strB.AppendLine("<tr>");
                foreach (DataGridViewCell dgvc in dg.Rows[i].Cells)
                {
                    if (dgvc.Visible)
                    {
                        string align;
                        if (dgvc.Style != null && dgvc.Style.Alignment != DataGridViewContentAlignment.NotSet)
                            align = dgvc.Style.Alignment.ToString();
                        else
                            align = dgvc.OwningColumn.DefaultCellStyle.Alignment.ToString();
                        if (align.EndsWith("Left"))
                            align = "left";
                        else if (align.EndsWith("Right"))
                            align = "right";
                        else if (align.EndsWith("Center"))
                            align = "center";
                        else align = "left";

                        string bgColor;
                        if (i % 2 == 0)
                            bgColor = "white";
                        else
                            bgColor = "azure";

                        string bold = string.Empty;
                        if (dgvc.OwningRow.DefaultCellStyle.Font != null && dgvc.OwningRow.DefaultCellStyle.Font.Bold)
                            bold = "font-weight:bold; ";
                        strB.AppendLine("<td style=\"" + bold + "border-width:1px; padding:0px; border-color:gray; border-style:solid; background-color:" + bgColor + ";\"" + "  align='" + align + "' valign='middle'>" +
                                    dgvc.FormattedValue.ToString() + "</td>");
                    }
                }
                strB.AppendLine("</tr>");

            }
            //table footer & end of html file
            strB.AppendLine("</table></center></body></html>");
            log.LogMethodExit(strB);
            return strB;
        }

        /// <summary>
        /// SendReport method
        /// </summary>
        /// <param name="ReportSource">ReportSource</param>
        /// <param name="reportName">reportName</param>
        /// <param name="Format">Format</param>
        /// <param name="TimeStamp">TimeStamp</param>
        /// <param name="ToEmails">ToEmails</param>
        /// <param name="Utilities">Utilities</param>
        /// <param name="message">message</param>
        /// <param name="subject">subject</param>
        /// <returns>returns bool</returns>
        public static bool SendReport(Telerik.Reporting.ReportSource ReportSource, string reportName, string Format, string TimeStamp, string ToEmails, Utilities Utilities, ref string message, string subject)
        {
            log.LogMethodEntry(ReportSource, reportName, Format, TimeStamp, ToEmails, Utilities, message, subject);
            try
            {

                string fileName = Common.GetReportFileName(reportName, TimeStamp, Format);
                bool exportSuccessful = Semnox.Parafait.Reports.Common.ExportReportData(ReportSource, Format, fileName.Trim());
                if (!exportSuccessful)
                {
                    log.LogMethodExit(false);
                    return false;
                }
                Common.WaitForFileCreation(fileName);
                if(!sendEmail(TimeStamp, ToEmails, Utilities, subject, ref message))
                {
                    log.LogMethodExit(false);
                    return false;
                }
                log.LogMethodExit(true);
                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(false);
                return false;
            }
        }

        /// <summary>
        /// GenerateReport method
        /// </summary>
        /// <returns>returns bool</returns>
        public static bool GenerateReport(Telerik.Reporting.ReportSource ReportSource, string reportName, string Format, string TimeStamp, Utilities Utilities, ref string message)
        {
            log.LogMethodEntry(ReportSource, reportName, Format, TimeStamp, Utilities, message);
            try
            {
                string fileName = Common.GetReportFileName(reportName, TimeStamp, Format);
                bool exportSuccessful = Semnox.Parafait.Reports.Common.ExportReportData(ReportSource, Format, fileName.Trim());
                if (!exportSuccessful)
                {
                    log.LogMethodExit(false);
                    return false;
                }
                Common.WaitForFileCreation(fileName);
                log.LogMethodExit(true);
                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(false);
                return false;
            }
        }

        /// <summary>
        /// sendEmail method
        /// </summary>
        /// <param name="TimeStamp">TimeStamp</param>
        /// <param name="ToEmails">ToEmails</param>
        /// <param name="Utilities">Utilities</param>
        /// <param name="message">message</param>
        /// <param name="subject">subject name</param>
        /// <returns></returns>
        public static bool sendEmail(string TimeStamp, string ToEmails, Utilities Utilities, string subject, ref string message, string siteVersion = null)
        {
            log.LogMethodEntry(TimeStamp, ToEmails, Utilities, subject, message);
            try
            {
                string smtpPassword = string.Empty;
                System.Net.ServicePointManager.ServerCertificateValidationCallback = SSLValidationCallback;
                ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
                appendMessage("Emailing reports...", ref message);
                log.Info("Emailing reports...");
                //System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12;                
                string smtpHost = getParafaitDefaults("SMTP_HOST");
                int smtpPort = 587;
                try
                {
                    smtpPort = Convert.ToInt32(getParafaitDefaults("SMTP_PORT")); // specify -1 to ignore, null to use 587
                }
                catch
                {
                    smtpPort = 587;
                }

                smtpHost = smtpHost == "" ? "smtp.gmail.com" : smtpHost;
                string smtpUsername = getParafaitDefaults("SMTP_NETWORK_CREDENTIAL_USERNAME");
                //string smtpPassword = ParafaitDefaultContainerList.GetDecryptedParafaitDefault(Utilities.ExecutionContext, "SMTP_NETWORK_CREDENTIAL_PASSWORD");//Utilities.getParafaitDefaults("SMTP_NETWORK_CREDENTIAL_PASSWORD");
                if (!string.IsNullOrEmpty(siteVersion))
                {
                    if (IsPasswordDecryptionRequired(siteVersion))
                    {
                        //Use internal getParafaitDefaults function of reports instead of ContainerList
                        smtpPassword = Encryption.Decrypt(getParafaitDefaults("SMTP_NETWORK_CREDENTIAL_PASSWORD"));
                    }
                    else
                    {
                        smtpPassword = getParafaitDefaults("SMTP_NETWORK_CREDENTIAL_PASSWORD");
                    }
                }
                else
                {
                    smtpPassword = Encryption.Decrypt(getParafaitDefaults("SMTP_NETWORK_CREDENTIAL_PASSWORD"));
                }

                string smtpDisplayName = getParafaitDefaults("SMTP_FROM_DISPLAY_NAME");
                string smtpMailDomain = getParafaitDefaults("SMTP_MAIL_ADDRESS_DOMAIN");
                string smtpFromMailAddress = getParafaitDefaults("SMTP_FROM_MAIL_ADDRESS");
                string replyTo = smtpFromMailAddress == "" ? (smtpUsername == "" ? "noreply@noreply.noreply" : smtpUsername) : smtpFromMailAddress;
                smtpUsername = smtpUsername == "" ? "ParafaitReports@gmail.com" : smtpUsername;
                smtpPassword = smtpPassword == "" ? "semnox!1" : smtpPassword;
                smtpDisplayName = smtpDisplayName == "" ? "Parafait Reports" : smtpDisplayName;

                SmtpClient mailClient;

                if (smtpPort == -1) // ignore port
                {
                    mailClient = new SmtpClient(smtpHost);
                }
                else
                {
                    mailClient = new SmtpClient(smtpHost, smtpPort);
                }
                mailClient.Timeout = 200000;
                if (getParafaitDefaults("ENABLE_SMTP_SSL") == "Y")
                {
                    mailClient.EnableSsl = true;
                }
                else
		        {
		            mailClient.EnableSsl = false;
		        }

                NetworkCredential credential = new NetworkCredential(smtpUsername, smtpPassword);

                mailClient.Credentials = credential;

                appendMessage("Email ids: " + ToEmails, ref message);
                log.Info("Email ids: " + ToEmails);

                string emailBody =
                    Common.MessageUtils.getMessage("Hi") + "," + Environment.NewLine + Environment.NewLine +
                    Common.MessageUtils.getMessage(772) +
                    Environment.NewLine + Environment.NewLine;

                string reportDirectory = getParafaitDefaults("PDF_OUTPUT_DIR") + "\\";

                DirectoryInfo di = new DirectoryInfo(reportDirectory);

                string pattern = "*" + TimeStamp + "*";
                FileInfo[] pdfFiles = di.GetFiles(pattern);

                if (pdfFiles.Count() == 0)
                {
                    log.Debug("Ends-sendEmail(ReportSource, FileName, Format, TimeStamp, ToEmails, Utilities, ref message) method.");
                    log.Info("No files found to be attached.");
                    message += "No files found to be attached.";
                    return false;
                }

                foreach (FileInfo fi in pdfFiles)
                {
                    if (!fi.Extension.Equals(".html", StringComparison.CurrentCultureIgnoreCase))
                        emailBody += fi.Name + Environment.NewLine;
                }

                MailMessage msg = new MailMessage();
                string mailFrom = smtpFromMailAddress == "" ? (smtpUsername.Contains("@") ? smtpUsername : smtpUsername + "@" + (String.IsNullOrEmpty(smtpMailDomain) ? smtpHost : smtpMailDomain)) : smtpFromMailAddress;
                string mailReplyTo = replyTo.Contains("@") ? replyTo : replyTo + "@" + (String.IsNullOrEmpty(smtpMailDomain) ? smtpHost : smtpMailDomain);

                try
                {
                    msg.From = new MailAddress(mailFrom, smtpDisplayName);
                }
                catch (Exception ex)
                {
                    appendMessage(mailFrom + ": " + ex.Message, ref message);
                    log.Error(mailFrom + ": " + ex.Message);
                }

                try
                {
                    msg.ReplyToList.Add(new MailAddress(mailReplyTo));
                }
                catch (Exception ex)
                {
                    appendMessage(mailReplyTo + ": " + ex.Message, ref message);
                    log.Error(mailReplyTo + ": " + ex.Message);
                }
                msg.To.Add(ToEmails);
                if (!string.IsNullOrEmpty(subject))
                    msg.Subject = subject;
                else
                    msg.Subject = Utilities.MessageUtils.getMessage("Parafait On Demand Email Report");
                msg.BodyEncoding = Encoding.UTF8;
                msg.IsBodyHtml = true;

                msg.Body = "<p " + " style=\"font-family: arial;\">" + emailBody
                                    .Replace(Environment.NewLine + Environment.NewLine, "</p><p>")
                                    .Replace(Environment.NewLine, "<br />")
                                    .Replace("</p><p>", "</p>" + Environment.NewLine + "<p>") + "</p>";

                if (pdfFiles.Count() == 0)
                {
                    appendMessage("No report file found", ref message);
                    log.Info("No report file found");
                    msg.Dispose();
                    log.LogMethodExit(false);
                    return false;
                }

                foreach (FileInfo fi in pdfFiles)
                {
                    if (fi.Extension.Equals(".html", StringComparison.CurrentCultureIgnoreCase))
                        msg.Body += File.ReadAllText(fi.FullName);
                    else
                        msg.Attachments.Add(new Attachment(fi.FullName));
                }

                bool retVal = true;
                try
                {
                    mailClient.Send(msg);
                }
                catch (Exception ex)
                {
                    appendMessage("Error: emailing reports", ref message);
                    log.Error("Error: emailing reports" + ex.Message);
                    appendMessage(ex.Message, ref message);
                    log.Info(ex.Message);
                    msg.Dispose();
                    log.LogMethodExit(false);
                    return false;
                }
                msg.Dispose();

                appendMessage("Done emailing reports", ref message);
                log.Info("Done emailing reports");
                log.LogMethodExit(retVal);
                return retVal;
            }
            catch (Exception ex)
            {
                appendMessage(ex.Message, ref message);
                log.Error(ex); 
                log.Info(ex.StackTrace);
                log.LogMethodExit();
                return false;
            }
        }

        /// <summary>
        /// GetSiteVersion method
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static string GetSiteVersion(string connectionString)
        {
            string siteVersion = string.Empty;
            log.LogMethodEntry(connectionString);
            try
            {
                int siteId = -1;
                if (Convert.ToInt32(Common.Utilities.executeScalar(@"select count(1) from site")) > 1)
                {
                    siteId = Convert.ToInt32(Common.Utilities.executeScalar(@"select master_site_id from company"));
                }
                else
                {
                    siteId = Convert.ToInt32(Common.Utilities.executeScalar(@"select top 1 site_id from site"));
                }

                if(siteId != -1)
                {
                    DataTable dtSiteVersion = Utilities.executeDataTable(@"select version from site where site_id= @siteId"
                                                                , new SqlParameter("@siteId", siteId));
                    if(dtSiteVersion != null && dtSiteVersion.Rows.Count > 0)
                    {
                        siteVersion = dtSiteVersion.Rows[0]["version"].ToString();
                    }
                }

            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
            return siteVersion;
        }


        private static string SSLValidationError { get; set; }
        private static bool SSLValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            log.LogMethodEntry(sender, certificate, chain, sslPolicyErrors);
            if (sslPolicyErrors == SslPolicyErrors.None)
            {
                // No SSL Errors
                log.LogMethodExit(true);
                return true;
            }
            // Errors in Certificate Chain
            if ((sslPolicyErrors & SslPolicyErrors.RemoteCertificateChainErrors) != 0)
            {
                // Check Certificate Chain Exists
                if (chain != null && chain.ChainStatus != null)
                {
                    foreach (X509ChainStatus chainStatus in chain.ChainStatus)
                    {
                        // Untrusted Root Authority
                        if ((certificate.Subject == certificate.Issuer) && (chainStatus.Status == X509ChainStatusFlags.UntrustedRoot))
                        {
                            SSLValidationError = string.Format("Certificate {0} has Untrusted root, Certificate Hash: {1}", certificate.Subject, certificate.GetCertHashString());
                            log.LogMethodExit(false);
                            return false;
                        }
                        // Other error in Chain
                        else if (chainStatus.Status != X509ChainStatusFlags.NoError)
                        {
                            SSLValidationError = string.Format("Error in Certificate chain for {0}: {1}, Certificate Hash {2}", certificate.Subject, chainStatus.Status.ToString(), certificate.GetCertHashString());
                            log.LogMethodExit(false);
                            return false;
                        }
                    }
                }
            }
            log.LogMethodExit(true);
            return true;
        }


        /// <summary>
        /// appendMessage method
        /// </summary>
        /// <param name="appendMessage"></param>
        /// <param name="message"></param>
        static void appendMessage(string appendMessage, ref string message)
        {
            log.LogMethodEntry(appendMessage, message);
            message += appendMessage + Environment.NewLine;
            log.LogMethodExit();
        }


        /// <summary>
        /// getReportGridTable method
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="breakColumn"></param>
        /// <param name="aggrColumnList"></param>
        /// <param name="hideBreakColumn"></param>
        /// <param name="showGrandTotal"></param>
        /// <param name="repeatBreakColumns"></param>
        /// <returns></returns>
        public static DataTable getReportGridTable(DataTable dt, ArrayList breakColumn, int[] aggrColumnList, bool hideBreakColumn,bool showGrandTotal,bool repeatBreakColumns)
        {
            log.LogMethodEntry(dt, breakColumn, aggrColumnList, hideBreakColumn);
            log.LogMethodExit();
            return getGroupedReportGridTable(dt, breakColumn, aggrColumnList, hideBreakColumn, true, repeatBreakColumns);
        }


        /// <summary>
        /// getGroupedReportGridTable
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="breakColumn"></param>
        /// <param name="aggrColumnList"></param>
        /// <param name="hideBreakColumn"></param>
        /// <param name="showGrandTotal"></param>
        /// <param name="repeatBreakColumns"></param>
        /// <returns></returns>
        public static DataTable getGroupedReportGridTable(DataTable dt, ArrayList breakColumn, int[] aggrColumnList, bool hideBreakColumn, bool showGrandTotal, bool repeatBreakColumns)
        {
            log.LogMethodEntry(dt, breakColumn, aggrColumnList, hideBreakColumn, showGrandTotal);
            prevBreakValue = string.Empty;
            dataRowGrandTotal = null;
            try
            {
                breakColumn.Sort();
                int lowestBreakColValue = Convert.ToInt32(breakColumn[0]);
                GrandaggrColumnValuesFirstBreakCol = null;

                if (breakColumn != null)
                {
                    string sortingColumns = string.Empty;
                    for (int i = 0; i < breakColumn.Count; i++)
                    {
                        int colNo = Convert.ToInt32(breakColumn[i]);
                        if (i == breakColumn.Count - 1)
                        {
                            sortingColumns = sortingColumns + dt.Columns[colNo - 1].ColumnName;
                        }
                        else
                        {
                            sortingColumns = sortingColumns + dt.Columns[colNo - 1].ColumnName + ",";
                        }
                    }

                    DataView dv = new DataView(dt);
                    dv.Sort = sortingColumns;

                    dt = dv.ToTable();

                    dt.Columns.Add("IsHeader", typeof(System.Int32));
                    foreach (DataRow row in dt.Rows)
                    {
                        row["IsHeader"] = 0;
                    }

                    GrandaggrColumnValuesFirstBreakCol = new double[dt.Columns.Count];
                    for (int k = 0; k < GrandaggrColumnValuesFirstBreakCol.Count(); k++)
                    {
                        GrandaggrColumnValuesFirstBreakCol[k] = 0.00;
                    }

                    for (int rowindex1 = 0; rowindex1 < breakColumn.Count; rowindex1++)
                    {
                        int breakColumnValue = Convert.ToInt32(breakColumn[rowindex1]);
                        dt = GetGroupedData(dt, breakColumnValue - 1, aggrColumnList, hideBreakColumn, showGrandTotal, lowestBreakColValue,repeatBreakColumns);
                    }
                }
                dt.Columns.RemoveAt(dt.Columns.Count - 1);
                log.LogMethodExit(dt);
                return dt;
            }
            catch(Exception ex)
            {
                log.Error(ex);
                //return the data without grouping
                return dt;
            }
        }


        /// <summary>
        /// GetGroupedData Method
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="breakColumnValue"></param>
        /// <param name="aggrColumnList"></param>
        /// <param name="hideBreakColumn"></param>
        /// <param name="showGrandTotal"></param>
        /// <param name="lowestBreakColValue"></param>
        /// <returns></returns>
        public static DataTable GetGroupedData(DataTable dt, int breakColumnValue, int[] aggrColumnList, bool hideBreakColumn, bool showGrandTotal, 
                                               int lowestBreakColValue, bool repeatBreakColumns)
        {
            try
            {
                log.LogMethodEntry(dt, breakColumnValue, aggrColumnList, hideBreakColumn, showGrandTotal, lowestBreakColValue);
                DataTable newDT = new DataTable();
                object[] aggrColumnValues = new object[dt.Columns.Count];
                object[] GrandaggrColumnValues = new object[dt.Columns.Count];
                object[] datarow = new object[dt.Columns.Count];
                foreach (System.Data.DataColumn dc in dt.Columns)
                {
                    newDT.Columns.Add(dc.ColumnName, dc.DataType);
                }
                for (int rowindex = 0; rowindex < dt.Rows.Count; rowindex++)
                {
                    if (dt.Rows[rowindex][breakColumnValue].ToString() != prevBreakValue && rowindex != 0) // sub-total row for break column
                    {
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            datarow[i] = null;
                            for (int j = 0; j < aggrColumnList.Length; j++)
                            {
                                if (aggrColumnList[j] == i)
                                {
                                    datarow[i] = aggrColumnValues[i];
                                }
                            }
                        }

                        if (newDT.Columns[breakColumnValue].DataType.Name == "String")
                            datarow[breakColumnValue] = prevBreakValue + " Total";
                        else
                            datarow[breakColumnValue] = prevBreakValue;

                        if (!hideBreakColumn)
                        {
                            datarow[dt.Columns.Count - 1] = 1;
                            if (Convert.ToInt32(dt.Rows[rowindex][dt.Columns.Count - 1]) != 2 && prevBreakValue != "")
                            {
                                newDT.Rows.Add(datarow);
                            }
                        }
                    }

                    if (dt.Rows[rowindex][breakColumnValue].ToString() != prevBreakValue && hideBreakColumn == false) // header row for break column
                    {
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            datarow[i] = null;
                        }

                        for (int i = 0; i < aggrColumnValues.Length; i++)
                            aggrColumnValues[i] = 0;

                        datarow[breakColumnValue] = dt.Rows[rowindex][breakColumnValue].ToString();
                        datarow[dt.Columns.Count - 1] = 2; //Header Row

                        if (!repeatBreakColumns)
                        {
                            if (datarow[breakColumnValue].ToString() != "")
                            newDT.Rows.Add(datarow);
                        }

                    }

                    for (int i = 0; i < dt.Columns.Count; i++) // each row
                    {
                        string type = dt.Columns[i].DataType.ToString().ToLower();
                        if(repeatBreakColumns)
                        {
                            datarow[i] = dt.Rows[rowindex][i];
                        }
                        else
                        {
                            if (i == breakColumnValue && type.ToLower().Contains("string"))
                               datarow[i] = DBNull.Value;
                            else
                            datarow[i] = dt.Rows[rowindex][i];
                        }
                        //if (i == breakColumnValue && type.ToLower().Contains("string"))
                        //    datarow[i] = DBNull.Value;
                        //else
                        //datarow[i] = dt.Rows[rowindex][i];
                        for (int j = 0; j < aggrColumnList.Length; j++)
                        {
                            if (aggrColumnList[j] == i)
                            {
                                double val;
                                if (dt.Rows[rowindex][i] == DBNull.Value)
                                    val = 0;
                                else
                                    val = Convert.ToDouble(dt.Rows[rowindex][i]);
                                aggrColumnValues[i] = Convert.ToDouble(aggrColumnValues[i]) + val;
                                if (lowestBreakColValue - 1 == breakColumnValue)
                                {
                                    GrandaggrColumnValuesFirstBreakCol[i] = Convert.ToDouble(GrandaggrColumnValuesFirstBreakCol[i]) + val;
                                }
                            }
                        }
                    }

                    newDT.Rows.Add(datarow);
                    prevBreakValue = dt.Rows[rowindex][breakColumnValue].ToString();

                    if (rowindex == dt.Rows.Count - 1) // last row. print sub total
                    {
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            string type = dt.Columns[i].DataType.ToString().ToLower();
                            datarow[i] = null;
                            for (int j = 0; j < aggrColumnList.Length; j++)
                            {
                                if (aggrColumnList[j] == i)
                                {
                                    datarow[i] = aggrColumnValues[i];
                                }
                            }
                        }
                        if (newDT.Columns[breakColumnValue].DataType.Name == "String" || newDT.Columns[breakColumnValue].ColumnName.EndsWith("id", StringComparison.CurrentCultureIgnoreCase))
                        {
                            if (hideBreakColumn == false)
                            {
                                datarow[breakColumnValue] = prevBreakValue + " Total";
                                datarow[dt.Columns.Count - 1] = 1;
                            }
                        }
                        else
                            datarow[breakColumnValue] = prevBreakValue;

                        if (!hideBreakColumn)
                        {
                            if (Convert.ToInt32(datarow[dt.Columns.Count - 1]) != 2 && prevBreakValue != "")
                                newDT.Rows.Add(datarow);
                        }

                        if (showGrandTotal) //Added condition 9-Dec-2016
                        {
                            // Grand Total
                            for (int i = 0; i < dt.Columns.Count; i++)
                            {
                                string type = dt.Columns[i].DataType.ToString().ToLower();

                                datarow[i] = null;
                                for (int j = 0; j < aggrColumnList.Length; j++)
                                {
                                    if (aggrColumnList[j] == i)
                                    {
                                        datarow[i] = GrandaggrColumnValuesFirstBreakCol[i];
                                    }
                                }
                            }

                            if (hideBreakColumn == false)
                            {
                                string type = newDT.Columns[lowestBreakColValue - 1].DataType.Name.ToString().ToLower();
                                if (type.Contains("string")  || type.EndsWith("id", StringComparison.CurrentCultureIgnoreCase))
                                    datarow[lowestBreakColValue - 1] = "Grand Total";
                            }
                            else
                            {
                                string type = newDT.Columns[lowestBreakColValue].DataType.Name.ToString().ToLower();
                                if (type.Contains("string")  || type.EndsWith("id", StringComparison.CurrentCultureIgnoreCase))
                                    datarow[lowestBreakColValue] = "Grand Total";
                            }
                            dataRowGrandTotal = null;
                            dataRowGrandTotal = datarow;
                        }
                    }
                }
                log.LogMethodExit(newDT);
                return newDT;
            }
            catch(Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null);
                return null;
            }
        }


        /// <summary>
        /// getReportGridTable method
        /// </summary>
        /// <param name="dt">dt</param>
        /// <param name="aggrColumnList">aggrColumnList</param>
        /// <param name="hideBreakColumn">hideBreakColumn</param>
        /// <param name="showGrandTotal">showGrandTotal</param>
        /// <returns>returns DataTable</returns>
        public static DataTable getReportGridTable(DataTable dt, int[] aggrColumnList, bool hideBreakColumn, bool showGrandTotal)
        {
            log.LogMethodEntry(dt, aggrColumnList, hideBreakColumn, showGrandTotal);
            int breakColumn = 0;
            prevBreakValue = string.Empty;
            dataRowGrandTotal = null;
            DataTable newDT = new DataTable();
            object[] aggrColumnValues = new object[dt.Columns.Count];
            object[] GrandaggrColumnValues = new object[dt.Columns.Count];
            object[] datarow = new object[dt.Columns.Count];
            foreach (System.Data.DataColumn dc in dt.Columns)
            {
                newDT.Columns.Add(dc.ColumnName, dc.DataType);
            }

            for (int rowindex = 0; rowindex < dt.Rows.Count; rowindex++)
            {
                for (int i = 0; i < dt.Columns.Count; i++) // each row
                {
                    string type = dt.Columns[i].DataType.ToString().ToLower();
                    datarow[i] = dt.Rows[rowindex][i];
                    for (int j = 0; j < aggrColumnList.Length; j++)
                    {
                        if (aggrColumnList[j] == i)
                        {
                            double val;
                            if (dt.Rows[rowindex][i] == DBNull.Value)
                                val = 0;
                            else
                                val = Convert.ToDouble(dt.Rows[rowindex][i]);
                            aggrColumnValues[i] = Convert.ToDouble(aggrColumnValues[i]) + val;
                            GrandaggrColumnValues[i] = Convert.ToDouble(GrandaggrColumnValues[i]) + val;
                        }
                    }
                }
                newDT.Rows.Add(datarow);

                if (rowindex == dt.Rows.Count - 1) // last row. print sub total
                {
                    if (showGrandTotal) //Added condition 9-Dec-2016
                    {
                        // Grand Total
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            string type = dt.Columns[i].DataType.ToString().ToLower();

                            datarow[i] = null;
                            for (int j = 0; j < aggrColumnList.Length; j++)
                            {
                                if (aggrColumnList[j] == i)
                                {
                                    datarow[i] = GrandaggrColumnValues[i];
                                }
                            }
                        }

                        if (newDT.Columns[breakColumn].DataType.Name == "String" || newDT.Columns[breakColumn].ColumnName.EndsWith("id", StringComparison.CurrentCultureIgnoreCase))
                            datarow[breakColumn] = "Grand Total";

                        dataRowGrandTotal = null;
                        dataRowGrandTotal = datarow;
                    }
                }
            }

            log.LogMethodExit(newDT);
            return newDT;
        }


        /// <summary>
        /// ExportToCSV method
        /// </summary>
        /// <param name="dtDataTable">dtDataTable</param>
        /// <param name="strFilePath">strFilePath</param>
        /// <param name="delimiter">delimiter</param>
        public static void ExportToCSV(this DataTable dtDataTable, string strFilePath, string delimiter)
        {
            log.LogMethodEntry(dtDataTable, strFilePath, delimiter);
            StreamWriter sw = new StreamWriter(strFilePath, false);
            //headers  
            for (int i = 0; i < dtDataTable.Columns.Count; i++)
            {
                sw.Write(dtDataTable.Columns[i]);
                if (i < dtDataTable.Columns.Count - 1)
                {
                    sw.Write(delimiter);
                }
            }
            sw.Write(sw.NewLine);
            foreach (DataRow dr in dtDataTable.Rows)
            {
                for (int i = 0; i < dtDataTable.Columns.Count; i++)
                {
                    if (!Convert.IsDBNull(dr[i]))
                    {
                        string value = dr[i].ToString();
                        if (value.Contains(delimiter))
                        {
                            value = String.Format("\"{0}\"", value);
                            sw.Write(value);
                        }
                        else
                        {
                            sw.Write(dr[i].ToString());
                        }
                    }
                    if (i < dtDataTable.Columns.Count - 1)
                    {
                        sw.Write(delimiter);
                    }
                }
                sw.Write(sw.NewLine);
            }
            sw.Close();
            log.LogMethodExit();
        }


        /// <summary>
        /// removeEmptyLinesFromCSV method
        /// </summary>
        /// <param name="filesPath">filesPath</param>
        public static void removeEmptyLinesFromCSV(string filesPath)
        {
            log.LogMethodEntry(filesPath);
            try
            {
                DirectoryInfo di = new DirectoryInfo(filesPath);

                string headerRow = string.Empty;
                string line;
                foreach (FileInfo f in di.GetFiles("*.csv"))
                {
                    using (TextReader reader = new StreamReader(f.FullName))
                    {
                        using (TextWriter writer = new StreamWriter(f.FullName + ".tmp"))
                        {
                            do
                            {
                                line = reader.ReadLine();
                                if (line != null)
                                    if (line != headerRow)
                                        writer.WriteLine(line);
                            } while (line != null);
                        }
                    }

                    File.Delete(f.FullName);
                    File.Move(f.FullName + ".tmp", f.FullName);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// ExportToDBF method
        /// </summary>
        /// <param name="dt">dt</param>
        /// <param name="path">path</param>
        /// <param name="fileName">fileName</param>
        public static void ExportToDBF(DataTable dt, string path, string fileName)
        {
            log.LogMethodEntry(dt, path, fileName);
            ArrayList list = new ArrayList();

            if (File.Exists(path + fileName + ".dbf"))
            {
                File.Delete(path + fileName + ".dbf");
            }

            string createSql = "create table " + fileName + "(";

            foreach (System.Data.DataColumn dc in dt.Columns)
            {
                string fieldName = dc.ColumnName;

                string type = dc.DataType.ToString();

                switch (type)
                {
                    case "System.String":
                        if (fieldName == "TRN_COA")
                            type = "varchar(15)";
                        else
                            type = "varchar(100)";
                        break;

                    case "System.Boolean":
                        type = "varchar(10)";
                        break;

                    case "System.Int32":
                        type = "int";
                        break;

                    case "System.Double":
                        type = "Double";
                        break;

                    case "System.DateTime":
                        type = "TimeStamp";
                        break;

                    case "System.Decimal":
                        type = "Decimal(14,2)";
                        break;
                }

                createSql = createSql + "[" + fieldName + "]" + " " + type + ",";
                list.Add(fieldName);
            }
            createSql = createSql.Substring(0, createSql.Length - 1) + ")";
            OleDbConnection con = new OleDbConnection(GetConnection(path));
            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = con;
            if (con.State == ConnectionState.Closed)
                con.Open();
            cmd.CommandText = createSql;
            cmd.ExecuteNonQuery();

            foreach (DataRow row in dt.Rows)
            {
                string insertSql = "insert into " + fileName + " values(";

                for (int i = 0; i < list.Count; i++)
                {
                    if (row[list[i].ToString()] == DBNull.Value)
                        insertSql = insertSql + "null,";
                    else if (dt.Columns[i].DataType.ToString() == "System.String")
                        insertSql = insertSql + "'" + ReplaceEscape(row[list[i].ToString()].ToString()) + "',";
                    else if (dt.Columns[i].DataType.ToString() == "System.DateTime")
                    {
                        string value = Convert.ToDateTime(ReplaceEscape(row[list[i].ToString()].ToString())).ToString("M/d/yyyy");
                        insertSql = insertSql + "'" + value + "',";
                    }
                    else
                        insertSql = insertSql + row[list[i].ToString()].ToString() + ",";
                }

                insertSql = insertSql.Substring(0, insertSql.Length - 1) + ")";
                cmd.CommandText = insertSql;
                cmd.ExecuteNonQuery();
            }
            con.Close();
            log.LogMethodExit();
        }


        private static string GetConnection(string path)
        {
            log.LogMethodEntry(path);
            log.LogMethodExit();
            return "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + ";Extended Properties=dBASE IV;";
        }


        /// <summary>
        /// ReplaceEscape method
        /// </summary>
        /// <param name="str">str</param>
        /// <returns> returns string</returns>
        public static string ReplaceEscape(string str)
        {
            log.LogMethodEntry(str);
            str = str.Replace("'", "''");
            log.LogMethodExit();
            return str;
        }


        /// <summary>
        /// changeConnectionString  method
        /// </summary>
        /// <param name="conStr">conStr</param>
        /// <param name="dbName">dbName</param>
        /// <returns>returns string</returns>
        public static string changeConnectionString(string conStr, string dbName)
        {
            log.LogMethodEntry(conStr, dbName);
            int pos1 = conStr.IndexOf("Database");
            if (pos1 < 0)
            {
                pos1 = conStr.IndexOf("Initial Catalog");
                int pos2 = conStr.IndexOf(";", pos1);
                if (pos2 > 0)
                    conStr = conStr.Substring(0, pos1) + "Initial Catalog=" + dbName + conStr.Substring(pos2);
                else
                    conStr = conStr.Substring(0, pos1) + "Initial Catalog=" + dbName;
            }
            else if (pos1 > 0)
            {
                int pos2 = conStr.IndexOf(";", pos1);
                if (pos2 > 0)
                    conStr = conStr.Substring(0, pos1) + "Database=" + dbName + conStr.Substring(pos2);
                else
                    conStr = conStr.Substring(0, pos1) + "Database=" + dbName;
            }
            log.LogMethodExit(conStr);
            return conStr;
        }


        /// <summary>
        /// ExportReportData method
        /// </summary>
        /// <param name="ReportSource">ReportSource</param>
        /// <param name="OutputFormat">OutputFormat</param>
        /// <param name="FileName">FileName</param>
        /// <returns>returns bool</returns>
        public static bool ExportReportData(Telerik.Reporting.ReportSource ReportSource, string OutputFormat, string FileName)
        {
            log.LogMethodEntry(ReportSource, OutputFormat, FileName);
            try
            {
                Telerik.Reporting.Processing.ReportProcessor reportProcessor = new Telerik.Reporting.Processing.ReportProcessor();
                System.Collections.Hashtable deviceInfo = new System.Collections.Hashtable();
                Telerik.Reporting.Processing.RenderingResult renderingResult = reportProcessor.RenderReport(OutputFormat, ReportSource, deviceInfo);
                if(File.Exists(FileName))
                {
                    File.Delete(FileName);
                }

                using (FileStream fs = new FileStream(FileName, FileMode.Create))
                {
                    fs.Write(renderingResult.DocumentBytes, 0, renderingResult.DocumentBytes.Length);
                }      
                log.LogMethodExit(true);
                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
                // return false;
            }
        }
        /// <summary>
        /// PrintReportData method
        /// </summary>
        /// <param name="ReportSource">ReportSource</param>
        /// <param name="printerName">printrName</param>
        /// <returns>returns bool</returns>
        public static bool PrintReportData(Telerik.Reporting.ReportSource ReportSource, string printerName)
        {
            log.LogMethodEntry(ReportSource, printerName);
            try
            {
                Telerik.Reporting.Processing.ReportProcessor reportProcessor = new Telerik.Reporting.Processing.ReportProcessor();

                System.Drawing.Printing.PrinterSettings psettings = new System.Drawing.Printing.PrinterSettings
                {
                    PrinterName = printerName
                };
                psettings.DefaultPageSettings.PaperSize = new PaperSize("Custom", 314, 39366);
                reportProcessor.PrintReport(ReportSource, psettings);
                log.LogMethodExit(true);
                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
                // return false;
            }
        }

        /// <summary>
        /// DeserializeReport method
        /// </summary>
        /// <param name="path">path</param>
        /// <returns>returns Telerik.Reporting.Report</returns>
        public static Telerik.Reporting.Report DeserializeReport(string path)
        {
            log.LogMethodEntry(path);
            try
            {
                System.Xml.XmlReaderSettings settings = new System.Xml.XmlReaderSettings();
                settings.IgnoreWhitespace = true;
                using (System.Xml.XmlReader xmlReader = System.Xml.XmlReader.Create(path, settings))
                {
                    Telerik.Reporting.XmlSerialization.ReportXmlSerializer xmlSerializer = new Telerik.Reporting.XmlSerialization.ReportXmlSerializer();
                    Telerik.Reporting.Report report = (Telerik.Reporting.Report)xmlSerializer.Deserialize(xmlReader);
                    log.LogMethodExit(report);
                    return report;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit();
                return null;
            }
        }


        /// <summary>
        /// setupVisuals method
        /// </summary>
        /// <param name="c">c</param>
        public static void setupVisuals(Control c)
        {
            log.LogMethodEntry(c);
            string type = c.GetType().ToString().ToLower();

            if (c.HasChildren)
            {
                c.BackColor = Common.SkinColor;
                foreach (Control cc in c.Controls)
                {
                    setupVisuals(cc);
                }
            }
            else
            {
                if (type.Contains("radiobutton"))
                {
                    ;
                }
                else
                    if (type.Contains("button"))
                {
                    setupButtonVisuals((Button)c);
                }
                else
                        if (type.Contains("tabpage"))
                {
                    TabPage tp = (TabPage)c;
                    tp.BackColor = Common.SkinColor;
                }
                else
                            if (type == "System.Windows.Forms.DataGridView")
                {
                    DataGridView dg = (DataGridView)c;
                    dg.BackColor = Common.SkinColor;
                }
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// setupButtonVisuals method
        /// </summary>
        /// <param name="b">Button b</param>
        public static void setupButtonVisuals(Button b)
        {
            log.LogMethodEntry(b);
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.FlatAppearance.MouseDownBackColor =
            b.FlatAppearance.MouseOverBackColor =
            b.BackColor = System.Drawing.Color.Transparent;
            b.Font = new System.Drawing.Font("arial", 8.5f);
            b.ForeColor = System.Drawing.Color.White;
            b.Size = new System.Drawing.Size(90, 25);
            b.BackgroundImageLayout = ImageLayout.Stretch;
            b.BackgroundImage = Properties.Resources.normal2;

            b.MouseDown += b_MouseDown;
            b.MouseUp += b_MouseUp;
            log.LogMethodExit();
        }

        static void b_MouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender,e);
            Button b = sender as Button;
            b.BackgroundImage = Properties.Resources.normal2;
            b.ForeColor = System.Drawing.Color.White;
            log.LogMethodExit();
        }

        static void b_MouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Button b = sender as Button;
            b.BackgroundImage = Properties.Resources.pressed3;
            b.ForeColor = System.Drawing.Color.White;
            log.LogMethodExit();
        }

        /// <summary>
        /// getConnectionString method
        /// </summary>
        /// <returns>returns string</returns>
        public static string getConnectionString()
        {
            log.LogMethodEntry();
            string constr = Common.Utilities.getConnection().ConnectionString;
            return constr;
            log.LogMethodExit();
        }


        /// <summary>
        /// GetReportSource method
        /// </summary>
        /// <param name="reportKey">reportKey</param>
        /// <param name="reportName">reportName</param>
        /// <param name="FromDate">FromDate</param>
        /// <param name="ToDate">ToDate</param>
        /// <param name="Sites">Sites</param>
        /// <param name="message">message</param>
        /// <param name="arguments">arguments</param>
        /// <param name="isBackground">isBackground</param>
        /// <param name="type">type</param>
        /// <returns>returns ReportSource</returns>
        public static ReportSource GetReportSource(string reportKey, string reportName, DateTime FromDate, DateTime ToDate, ArrayList Sites, ref string message, List<clsReportParameters.SelectedParameterValue> arguments, bool isBackground, string type = "W")
        {
            log.LogMethodEntry(reportKey, reportName, FromDate, ToDate, Sites, message, arguments, isBackground, type);
            try
            {
                Telerik.Reporting.ReportSource reportSource;
                DateTime startDate = DateTime.Now;
                string reportFilePath = string.Empty;
                bool printContinuous = false;
                List<ReportsDTO> ReportsDTOs;
                ExecutionContext executionContext = ExecutionContext.GetExecutionContext();
                bool isPortrait = false;
                double pageWidth = 0;
                double pageHeight = 0;
                int pageSize = -1;
                bool isReceipt = false;
                ReportsList reportsList = new ReportsList(executionContext);
                List<KeyValuePair<ReportsDTO.SearchByReportsParameters, string>> reportsSearchParams = new List<KeyValuePair<ReportsDTO.SearchByReportsParameters, string>>();
                reportsSearchParams.Add(new KeyValuePair<ReportsDTO.SearchByReportsParameters, string>(ReportsDTO.SearchByReportsParameters.REPORT_KEY, reportKey));
                reportsSearchParams.Add(new KeyValuePair<ReportsDTO.SearchByReportsParameters, string>(ReportsDTO.SearchByReportsParameters.SITE_ID, executionContext.GetSiteId().ToString()));                
                ReportsDTOs = reportsList.GetAllReports(reportsSearchParams);

                if (ReportsDTOs != null && ReportsDTOs.Any())
                {
                    printContinuous = ReportsDTOs[0].PrintContinuous.ToUpper() == "Y" ? true : false; //Returns one Record since Report key is unique
                    isPortrait = ReportsDTOs[0].IsPortrait;
                    pageWidth = ReportsDTOs[0].PageWidth;
                    pageHeight = ReportsDTOs[0].PageHeight;
                    pageSize = ReportsDTOs[0].PageSize;
                    isReceipt = ReportsDTOs[0].IsReceipt;
                }

                if (type == "F")
                {
                    string path = System.IO.Directory.GetCurrentDirectory();
                    reportFilePath = path + "\\Reports\\" + reportKey + ".trdx";
                }
                else
                {
                    string path = AppDomain.CurrentDomain.BaseDirectory;
                    reportFilePath = path + "\\Reports\\" + reportKey + ".trdx";
                }

                log.Info("CommonPath : " + reportFilePath);
                Telerik.Reporting.UriReportSource URI = new Telerik.Reporting.UriReportSource();
                URI.Uri = reportFilePath;
                XmlReaderSettings settings = new XmlReaderSettings();
                var connectionString = Common.getConnectionString();
                settings.IgnoreWhitespace = true;

                var connectionStringHandler = new ReportConnectionstringManager(Utilities.getConnection().ConnectionString, "");

                var report = new Telerik.Reporting.Report();
                report = Common.DeserializeReport(reportFilePath);
                report.DocumentName = ((reportName + " " + DateTime.Now.ToString("dd-MM-yyyy hh-mm")).TrimStart()).TrimEnd();

                report.PageSettings.Landscape = isPortrait ? false : true;

                if (isReceipt && printContinuous)
                {
                    report.PageSettings.Margins.Left = Telerik.Reporting.Drawing.Unit.Inch(0.15);
                    report.PageSettings.Margins.Right = Telerik.Reporting.Drawing.Unit.Inch(0.15);
                }
                else
                {
                    report.PageSettings.Margins.Left = Telerik.Reporting.Drawing.Unit.Inch(0.25);
                    report.PageSettings.Margins.Right = Telerik.Reporting.Drawing.Unit.Inch(0.25);
                }

                report.PageSettings.Margins.Top = report.PageSettings.Margins.Bottom = Telerik.Reporting.Drawing.Unit.Inch(0.25);

                if (reportKey.ToLower().EndsWith("rprint") || reportKey.ToLower().EndsWith("rprintcustom") || printContinuous) //trdx filename should end with rprint to generate the report in receipt mode
                {
                    report.PageSettings.Landscape = false;                 
                    report.PageSettings.ContinuousPaper = true;
                }
                
                if (pageSize != -1)
                {
                    LookupValuesList lookUpList = new LookupValuesList();
                    List<LookupValuesDTO> pageSizelookUpValuesList = new List<LookupValuesDTO>();
                    List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookUpValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                    lookUpValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "REPORTS_PRINTING_PAGE_SIZE"));
                    pageSizelookUpValuesList = lookUpList.GetAllLookupValues(lookUpValuesSearchParams);
                    if (pageSizelookUpValuesList != null && pageSizelookUpValuesList.Any())
                    {
                        string paperKind = pageSizelookUpValuesList.Find(m => m.LookupValueId == ReportsDTOs[0].PageSize).LookupValue;
                        if (paperKind.ToUpper() == System.Drawing.Printing.PaperKind.Custom.ToString().ToUpper())
                        {
                            report.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.Custom;
                            report.PageSettings.Landscape = isPortrait ? false : true;
                            report.PageSettings.PaperSize = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(pageHeight == 0 ? 11D : pageHeight),
                                                        Telerik.Reporting.Drawing.Unit.Inch(pageWidth == 0 ? 8.5D : pageWidth));
                        }
                        else
                        {
                            report.PageSettings.PaperKind = (System.Drawing.Printing.PaperKind)Enum.Parse(typeof(System.Drawing.Printing.PaperKind), paperKind);
                            report.PageSettings.Landscape = isPortrait ? false : true;
                        }
                    }
                }      
                
                var sourceReportSource = new InstanceReportSource { ReportDocument = report };
                reportSource = connectionStringHandler.UpdateReportSource(sourceReportSource, isBackground, type);

                reportSource.Parameters.Add("fromdate", FromDate);
                reportSource.Parameters.Add("todate", ToDate);
                reportSource.Parameters.Add("connectionstring", Utilities.getConnection().ConnectionString);

                Telerik.Reporting.SubReport subreportItem = report.Items.Find("detailSection1", true)[0] as Telerik.Reporting.SubReport;

                int cntParameters = 0;
                if (arguments != null)
                {
                    foreach (clsReportParameters.SelectedParameterValue param in arguments)
                    {
                        reportSource.Parameters.Add(param.parameterName, param.parameterValue[0]);
                        cntParameters++;
                    }
                }
                message = "Success";
                log.LogMethodExit(reportSource);
                return reportSource;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                message = "Error generating report: " + ex.Message;
                log.LogMethodExit();
                return null;
            }
        }


        /// <summary>
        /// GetCustomReportSource method
        /// </summary>
        /// <param name="DBQuery">DBQuery</param>
        /// <param name="reportName">reportName</param>
        /// <param name="FromDate">FromDate</param>
        /// <param name="ToDate">ToDate</param>
        /// <param name="UserID">UserID</param>
        /// <param name="SelectedParameters">SelectedParameters</param>
        /// <param name="HideBreakColumn">HideBreakColumn</param>
        /// <param name="breakColumn">breakColumn</param>
        /// <param name="message">message</param>
        /// <param name="otherParams">otherParams</param>
        /// <param name="showGrandTotal">showGrandTotal</param>
        /// <returns>returns  Telerik.Reporting.Report</returns>
        public static Telerik.Reporting.Report GetCustomReportSource(int reportID, string DBQuery, string reportName, DateTime FromDate, DateTime ToDate, string UserID, string SelectedSites, List<SqlParameter> SelectedParameters, bool HideBreakColumn, string breakColumn, bool showGrandTotal, bool printContinuous, string aggreagteColumns, bool repeatBreakColumns, ref string message, decimal reportPageWidth = 0, params string[] otherParams)
        {
            log.LogMethodEntry(DBQuery, reportName, FromDate, ToDate, UserID, SelectedSites, SelectedParameters, HideBreakColumn, breakColumn, showGrandTotal, message, otherParams);
            try
            {
                string headerBackgrndColor = string.Empty;
                string headerTxtColor = string.Empty;
                int rowCountPerPage = -1;

                if (DBQuery.Contains("@PassPhrase"))
                {
                    try
                    {
                        string passPhrase = Common.Utilities.getParafaitDefaults("CUSTOMER_ENCRYPTION_PASS_PHRASE");
                        passPhrase = string.IsNullOrEmpty(passPhrase) ? string.Empty : passPhrase;
                        DBQuery = DBQuery.Replace("@PassPhrase", "'"+passPhrase+"'");
                    }
                    catch(Exception ex)
                    {
                        log.Error(ex);
                    }

                }

                ReportsList reportsList = new ReportsList();
                string query = "exec dbo.SetContextInfo   " + Common.ParafaitEnv.User_Id + "  ;";
                query += " " + DBQuery;
                DataTable dt = reportsList.GetCustomReportData(query, SelectedParameters, true);

                Common.ClearSortingFields();

                if (SelectedSites == "")
                    SelectedSites = Common.ParafaitEnv.SiteName;

                if (dt == null)
                {
                    message = "No data";
                    log.LogMethodExit(message);
                    return null;
                }

                
                ReportsDTO reportsDTO = new ReportsDTO();
                reportsDTO = reportsList.GetReports(reportID);
                if (reportsDTO != null)
                {
                    headerBackgrndColor = reportsDTO.HeaderBackgroundColor;
                    headerTxtColor = reportsDTO.HeaderTextColor;
                    rowCountPerPage = reportsDTO.RowCountPerPage;
                }

                if(rowCountPerPage == -1)
                {
                    rowCountPerPage = string.IsNullOrWhiteSpace(ParafaitDefaultContainerList.GetParafaitDefault(Common.Utilities.ExecutionContext, "ROW_COUNT_PER_PAGE_IN_CUSTOM_REPORTS"))
                                                         ? 10000 : Convert.ToInt32(ParafaitDefaultContainerList.GetParafaitDefault(Common.Utilities.ExecutionContext, "ROW_COUNT_PER_PAGE_IN_CUSTOM_REPORTS"));
                }

                Telerik.Reporting.Report report = null;
                if (dt.Columns.Count == 1 || printContinuous)
                {
                    report = new CreatePOSCustomReport(dt, reportName, FromDate, ToDate, 0, SelectedSites, UserID, HideBreakColumn, breakColumn, showGrandTotal, repeatBreakColumns, reportPageWidth, headerBackgrndColor, headerTxtColor, otherParams);
                }
                else
                {
                    report = new CreateCustomReport(dt, reportName, FromDate, ToDate, 0, SelectedSites, UserID, HideBreakColumn, breakColumn, showGrandTotal, aggreagteColumns, repeatBreakColumns, headerBackgrndColor, headerTxtColor, rowCountPerPage, reportID, otherParams);
                    ListSortByFields(dt, HideBreakColumn, breakColumn);
                }
                message = "Success";
                log.LogMethodExit(report);
                return report;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                message = ex.Message;
                log.LogMethodExit();
                return null;
            }
        }


        /// <summary>
        /// Lists the Fields of the datatable and binds to an array
        /// </summary>
        /// <param name="dtsort"></param>
        /// <param name="hideBreakColumn"></param>
        /// <param name="breakColumn"></param>
        public static void ListSortByFields(DataTable dtsort, bool hideBreakColumn, string breakColumn)
        {
            log.LogMethodEntry(dtsort, hideBreakColumn, breakColumn);
            DataTable SortFieldsTable = new DataTable();
            SortFieldsTable.Columns.Add("SortFieldsText");
            SortFieldsTable.Columns.Add("SortFieldsValue");

            ArrayList breakColList = Common.GetArrayListFromCSV(breakColumn);

            int breakColNo = 0;
            if(breakColList != null && breakColList.Count == 1)
            {
                breakColNo = Convert.ToInt32(breakColList[0]);
            }
            else
            {
                //No break Column assigned
                breakColNo = 0;
            }

            for (int i = 0; i < dtsort.Columns.Count; i++)
            {
                if (breakColList == null || breakColList.Count == 1)
                {
                    if(i != (breakColNo - 1) || hideBreakColumn == false)
                    {
                        //Do not include GroupIndexer column in the sortby dropdownlist
                        if (dtsort.Columns[i].ColumnName != "GroupIndexer")
                        {
                            if (dtsort.Columns[i].ColumnName.Contains("_"))
                                SortFieldsTable.Rows.Add(dtsort.Columns[i].ColumnName.Replace("_", " "), dtsort.Columns[i].ColumnName);
                            else
                                SortFieldsTable.Rows.Add(dtsort.Columns[i].ColumnName, dtsort.Columns[i].ColumnName);
                        }                        
                    }
                }

            }
            dtSortTableColumns.Clear();
            dtSortTableColumns = SortFieldsTable;
            dtSortData = dtsort;
            customReportColumnCount = dtsort.Columns.Count;
            log.LogMethodExit();
        }


        /// <summary>
        /// ClearSortingFields method
        /// </summary>
        public static void ClearSortingFields()
        {
            log.LogMethodEntry();
            dtSortTableColumns.Clear();
            dtSortData.Clear();
            dataRowGrandTotal = new object[150];
            sortDirection = string.Empty;
            sortField = string.Empty;
            customReportColumnCount = 0;
            log.LogMethodExit();
        }

        private static void addDatagridToPrintGrid(ref DataGridView printDGV, DataGridView sourceGrid, string gridHeading)
        {
            log.LogMethodEntry(printDGV, sourceGrid, gridHeading);
            addDatagridToPrintGrid(ref printDGV, sourceGrid, 0, sourceGrid.Columns.Count - 1, gridHeading);
            log.LogMethodExit();
        }

        private static void addDatagridToPrintGrid(ref DataGridView printDGV, DataGridView sourceGrid, int fromColumn, int toColumn, string gridHeading)
        {
            log.LogMethodEntry(printDGV, sourceGrid, fromColumn, toColumn, gridHeading);
            string addChar = ".";
            if (printDGV.Rows.Count != 0)
            {
                printDGV.Rows.Add(addChar);
                printDGV.Rows.Add(addChar);
            }
            printDGV.Rows.Add(addChar);
            int printDGVCurrentRow = printDGV.Rows.Count - 1;
            printDGV.Rows[printDGVCurrentRow].Cells[0].Value = gridHeading;
            printDGV.Rows[printDGVCurrentRow].DefaultCellStyle.Font = new Font(printDGV.DefaultCellStyle.Font.FontFamily,
                                                                             printDGV.DefaultCellStyle.Font.Size,
                                                                             FontStyle.Bold);
            printDGV.Rows.Add();
            printDGVCurrentRow++;

            int colIndex = 0;
            for (int i = fromColumn; i <= toColumn; i++)
            {
                printDGV.Rows[printDGVCurrentRow].Cells[colIndex].Value = sourceGrid.Columns[i].HeaderText;
                printDGV.Rows[printDGVCurrentRow].DefaultCellStyle = sourceGrid.ColumnHeadersDefaultCellStyle;
                printDGV.Rows[printDGVCurrentRow].Cells[colIndex].Style.Alignment = sourceGrid.ColumnHeadersDefaultCellStyle.Alignment;
                colIndex++;
            }

            for (int j = colIndex; j < printDGV.Columns.Count; j++)
            {
                printDGV.Rows[printDGVCurrentRow].Cells[j].Style.BackColor = Color.White;
            }

            foreach (DataGridViewRow row in sourceGrid.Rows)
            {
                printDGV.Rows.Add();
                printDGVCurrentRow++;

                colIndex = 0;
                for (int i = fromColumn; i <= toColumn; i++)
                {
                    printDGV.Rows[printDGVCurrentRow].Cells[colIndex].Value = row.Cells[i].FormattedValue;
                    printDGV.Rows[printDGVCurrentRow].DefaultCellStyle.Font = row.DefaultCellStyle.Font;
                    printDGV.Rows[printDGVCurrentRow].DefaultCellStyle.BackColor = row.DefaultCellStyle.BackColor;
                    printDGV.Rows[printDGVCurrentRow].Cells[colIndex].Style = sourceGrid.Columns[i].DefaultCellStyle;
                    colIndex++;
                }

                for (int j = colIndex; j < printDGV.Columns.Count; j++)
                {
                    printDGV.Rows[printDGVCurrentRow].Cells[j].Style.BackColor = Color.White;
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// setupGridviews  method
        /// </summary>
        /// <param name="dgv">dgv</param>
        public static void setupGridviews(ref DataGridView dgv)
        {
            log.LogMethodEntry(dgv);
            dgv.ReadOnly = true;
            dgv.AllowUserToDeleteRows = false;
            dgv.AllowUserToAddRows = false;
            dgv.AutoSize = true;
            Common.Utilities.setupDataGridProperties(ref dgv);
            dgv.BackgroundColor = Color.White;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.None;

            for (int k = 0; k < dgv.Columns.Count; k++)
            {
                dgv.Columns[k].SortMode = DataGridViewColumnSortMode.NotSortable;
                if (dgv.Columns[k].ValueType.Name.StartsWith("Int", StringComparison.CurrentCultureIgnoreCase))
                {
                    dgv.Columns[k].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgv.Columns[k].DefaultCellStyle.Format = Common.Utilities.getParafaitDefaults("NUMBER_FORMAT");
                }
                if (dgv.Columns[k].ValueType.Name == "Decimal" || dgv.Columns[k].ValueType.Name == "Double")
                {
                    dgv.Columns[k].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgv.Columns[k].DefaultCellStyle.Format = Common.Utilities.getParafaitDefaults("AMOUNT_FORMAT");
                }
                else if (dgv.Columns[k].ValueType.Name == "DateTime")
                {
                    dgv.Columns[k].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                    dgv.Columns[k].DefaultCellStyle.Format = Common.Utilities.getParafaitDefaults("DATETIME_FORMAT");
                }
                else
                    dgv.Columns[k].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dgv.Columns[k].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

                dgv.Columns[k].HeaderText = getHeaderText(dgv.Columns[k].HeaderText);
            }

            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.RowsDefaultCellStyle.SelectionBackColor = Color.White;
            dgv.RowsDefaultCellStyle.SelectionForeColor = Color.Black;

            dgv.RowsDefaultCellStyle.Font = dgv.DefaultCellStyle.Font;

            string totalTL = Common.Utilities.MessageUtils.getMessage("Total");
            string grandTotalTL = Common.Utilities.MessageUtils.getMessage("Grand Total");

            if (dgv.Rows.Count > 0)
            {
                if (dgv.Rows[dgv.Rows.Count - 1].Cells[0].Value.ToString().Contains(grandTotalTL)
                    || dgv.Rows[dgv.Rows.Count - 1].Cells[1].Value.ToString().Contains(grandTotalTL))
                {
                    dgv.Rows[dgv.Rows.Count - 1].DefaultCellStyle.Font
                        = new Font(dgv.RowsDefaultCellStyle.Font.Name,
                                    dgv.RowsDefaultCellStyle.Font.Size,
                                    FontStyle.Bold);
                    dgv.Rows[dgv.Rows.Count - 1].DefaultCellStyle.BackColor = Color.LightGray;
                }
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// getHeaderText method
        /// </summary>
        /// <param name="header">header</param>
        /// <returns>returns string</returns>
        public static string getHeaderText(string header)
        {
            log.LogMethodEntry(header);
            header.ToString().Replace("_", " ");
            header = char.ToUpper(header[0]) + header.Substring(1);
            log.LogMethodExit(header);
            return header;
        }


        /// <summary>
        /// exportReportData method
        /// </summary>
        /// <param name="reportKey">reportKey</param>
        /// <param name="reportID">reportID</param>
        /// <param name="reportName">reportName</param>
        /// <param name="customFlag">customFlag</param>
        /// <param name="fromDate">fromDate</param>
        /// <param name="toDate">toDate</param>
        /// <param name="FileName">FileName</param>
        /// <param name="message">message</param>
        public static void exportReportData(string reportKey, long reportID, string reportName, string customFlag, DateTime fromDate, DateTime toDate, string FileName, ref string message)
        {
            log.LogMethodEntry(reportKey, reportID, reportName, customFlag, fromDate, toDate, FileName, message);
            try
            {
                string reportDirectory = Common.Utilities.getParafaitDefaults("PDF_OUTPUT_DIR") + "\\";
                string queryFilePath = Common.Utilities.getParafaitDefaults("PARAFAIT_HOME") + "\\Scripts\\";
                Telerik.Reporting.UriReportSource URI = new Telerik.Reporting.UriReportSource();
                List<SqlParameter> selectedParameters = new List<SqlParameter>();
                selectedParameters.Add(new SqlParameter("@fromDate", fromDate));
                selectedParameters.Add(new SqlParameter("@toDate", toDate));
                selectedParameters.Add(new SqlParameter("@offSet", 0));
                selectedParameters.Add(new SqlParameter("@site", -1));
                appendMessage("Run Report: " + reportName, ref message);

                ReportsList reportsList = new ReportsList();

                string query;
                DataTable dt;
                Form frm = new Form();
                DataGridView dgv = new DataGridView();
                frm.Controls.Add(dgv);
                dgv.Visible = true;

                if (reportKey == "Transaction" || reportKey == "PaymentModeBreakdownTransaction" || reportKey == "CashierBreakdownTransaction" ||
                    reportKey == "ProductBreakdownTransaction" || reportKey == "DiscountBreakdownTransaction" || reportKey == "SpecialPricingTransaction" ||
                    reportKey == "TaxBreakdownTransaction" || reportKey == "POSMachineBreakdownTransaction" || reportKey == "CardTransfersTransaction" ||
                    reportKey == "CardActivitiesTransaction" || reportKey == "DisplayGroupBreakdownTransaction" || reportKey == "SalesSummary" || reportKey == "TransactionWithConceptTypesAndArea")
                {
                    DataTable tempDt = new DataTable();
                    for (int i = 0; i < 8; i++)
                        dgv.Columns.Add("Col" + i.ToString(), "   ");

                    //Payment Mode Breakdown
                    DataGridView dgvPaymentMode = new DataGridView();
                    frm.Controls.Add(dgvPaymentMode);
                    query = System.IO.File.ReadAllText(queryFilePath + "\\PaymentModeBreakdown.sql");
                    tempDt = reportsList.GetQueryOutput(query, selectedParameters);
                    if (tempDt == null)
                    {
                        log.Info("No data found.");
                        log.Debug("Ends-exportReportData(reportKey, reportID, reportName, customFlag, fromDate, toDate, FileName, ref message) method. No data found.");
                        return;
                    }
                    DataTable newDT = Common.Utilities.getReportGridTable(tempDt, 0, new int[] { 1, 2, 3, 4, 5, 6 });

                    if (newDT.Rows.Count > 0)
                        tempDt.Rows.Add(new object[] {newDT.Rows[newDT.Rows.Count - 1][0], //grand total
                                                    newDT.Rows[newDT.Rows.Count - 1][1],
                                                    newDT.Rows[newDT.Rows.Count - 1][2],
                                                    newDT.Rows[newDT.Rows.Count - 1][3],
                                                    newDT.Rows[newDT.Rows.Count - 1][4],
                                                    newDT.Rows[newDT.Rows.Count - 1][5],
                                                    newDT.Rows[newDT.Rows.Count - 1][6],
                                                    newDT.Rows[newDT.Rows.Count - 1][7]});
                    dgvPaymentMode.DataSource = tempDt;
                    setupGridviews(ref dgvPaymentMode);
                    Common.Utilities.setLanguage(dgvPaymentMode);

                    //Cashier breakdown
                    DataGridView dgvCashier = new DataGridView();
                    frm.Controls.Add(dgvCashier);
                    query = System.IO.File.ReadAllText(queryFilePath + "\\CashierBreakdown.sql");
                    tempDt = new DataTable();
                    tempDt = reportsList.GetQueryOutput(query, selectedParameters);
                    newDT = Common.Utilities.getReportGridTable(tempDt, 0, new int[] { 1, 2, 3, 4 });

                    if (newDT.Rows.Count > 0)
                        tempDt.Rows.Add(new object[] {newDT.Rows[newDT.Rows.Count - 1][0], //grand total
                                                    newDT.Rows[newDT.Rows.Count - 1][1],
                                                    newDT.Rows[newDT.Rows.Count - 1][2],
                                                    newDT.Rows[newDT.Rows.Count - 1][3],
                                                    newDT.Rows[newDT.Rows.Count - 1][4]});
                    dgvCashier.DataSource = tempDt;
                    setupGridviews(ref dgvCashier);
                    Common.Utilities.setLanguage(dgvCashier);

                    //Product Breakdown
                    DataGridView dgvProducts = new DataGridView();
                    frm.Controls.Add(dgvProducts);
                    query = System.IO.File.ReadAllText(queryFilePath + "\\ProductBreakdown.sql");
                    tempDt = new DataTable();
                    tempDt = reportsList.GetQueryOutput(query, selectedParameters);
                    newDT = Common.Utilities.getReportGridTable(tempDt, 1, new int[] { 3, 4, 5, 6, 7 });

                    for (int i = 0; i < newDT.Rows.Count; i++)
                    {
                        if (newDT.Rows[i]["product_name"] != DBNull.Value)
                            newDT.Rows[i]["product_type"] = string.Empty;
                    }

                    dgvProducts.DataSource = newDT;
                    dgvProducts.Columns[0].Visible = false;
                    setupGridviews(ref dgvProducts);
                    Common.Utilities.setLanguage(dgvProducts);

                    //Discount Breakdown
                    DataGridView dgvDiscounts = new DataGridView();
                    frm.Controls.Add(dgvDiscounts);
                    query = System.IO.File.ReadAllText(queryFilePath + "\\DiscountBreakdown.sql");
                    tempDt = new DataTable();
                    tempDt = reportsList.GetQueryOutput(query, selectedParameters);
                    newDT = Common.Utilities.getReportGridTable(tempDt, 0, new int[] { 2, 4 });

                    if (newDT.Rows.Count > 0)
                        tempDt.Rows.Add(new object[] {newDT.Rows[newDT.Rows.Count - 1][0], //grand total
                                                    newDT.Rows[newDT.Rows.Count - 1][1],
                                                    newDT.Rows[newDT.Rows.Count - 1][2],
                                                    newDT.Rows[newDT.Rows.Count - 1][3],
                                                    newDT.Rows[newDT.Rows.Count - 1][4],
                                                    newDT.Rows[newDT.Rows.Count - 1][5]});
                    dgvDiscounts.DataSource = tempDt;
                    setupGridviews(ref dgvDiscounts);
                    Common.Utilities.setLanguage(dgvDiscounts);

                    //Special Pricing Breakdown
                    DataGridView dgvSpecialPricing = new DataGridView();
                    frm.Controls.Add(dgvSpecialPricing);
                    query = System.IO.File.ReadAllText(queryFilePath + "\\SpecialPricingBreakdown.sql");
                    tempDt = new DataTable();
                    tempDt = reportsList.GetQueryOutput(query, selectedParameters);
                    newDT = Common.Utilities.getReportGridTable(tempDt, 0, new int[] { 2, 3, 4, 5, 6 });

                    for (int i = 0; i < newDT.Rows.Count; i++)
                    {
                        if (newDT.Rows[i]["product_name"] != DBNull.Value)
                            newDT.Rows[i]["pricing_name"] = string.Empty;
                    }
                    dgvSpecialPricing.DataSource = newDT;
                    setupGridviews(ref dgvSpecialPricing);
                    Common.Utilities.setLanguage(dgvSpecialPricing);

                    //Tax Breakdown
                    DataGridView dgvTax = new DataGridView();
                    frm.Controls.Add(dgvTax);
                    query = System.IO.File.ReadAllText(queryFilePath + "\\TaxBreakdown.sql");
                    tempDt = new DataTable();
                    tempDt = reportsList.GetQueryOutput(query, selectedParameters);
                    newDT = Common.Utilities.getReportGridTable(tempDt, 0, new int[] { 1 });

                    if (newDT.Rows.Count > 0)
                        tempDt.Rows.Add(new object[] {newDT.Rows[newDT.Rows.Count - 1][0], //grand total
                                                    newDT.Rows[newDT.Rows.Count - 1][1]});
                    dgvTax.DataSource = tempDt;
                    setupGridviews(ref dgvTax);
                    Common.Utilities.setLanguage(dgvTax);

                    //POS Breakdown
                    DataGridView dgvPOSBreakDown = new DataGridView();
                    frm.Controls.Add(dgvPOSBreakDown);
                    query = System.IO.File.ReadAllText(queryFilePath + "\\POSBreakdown.sql");
                    tempDt = new DataTable();
                    tempDt = reportsList.GetQueryOutput(query, selectedParameters);
                    newDT = Common.Utilities.getReportGridTable(tempDt, 0, new int[] { 2, 3, 4, 5 });

                    if (newDT.Rows.Count > 0)
                        tempDt.Rows.Add(new object[] {newDT.Rows[newDT.Rows.Count - 1][0], //grand total
                                                    newDT.Rows[newDT.Rows.Count - 1][1],
                                                    newDT.Rows[newDT.Rows.Count - 1][2],
                                                    newDT.Rows[newDT.Rows.Count - 1][3],
                                                    newDT.Rows[newDT.Rows.Count - 1][4],
                                                    newDT.Rows[newDT.Rows.Count - 1][5]});
                    dgvPOSBreakDown.DataSource = tempDt;
                    setupGridviews(ref dgvPOSBreakDown);
                    Common.Utilities.setLanguage(dgvPOSBreakDown);

                    //Card Transfers Breakdown
                    DataGridView dgvTransfers = new DataGridView();
                    frm.Controls.Add(dgvTransfers);
                    query = System.IO.File.ReadAllText(queryFilePath + "\\TransfersBreakdown.sql");
                    tempDt = new DataTable();
                    tempDt = reportsList.GetQueryOutput(query, selectedParameters);
                    dgvTransfers.DataSource = tempDt;
                    setupGridviews(ref dgvTransfers);
                    Common.Utilities.setLanguage(dgvTransfers);

                    //Card Activities Breakdown
                    DataGridView dgvExchangesTickets = new DataGridView();
                    frm.Controls.Add(dgvExchangesTickets);
                    query = System.IO.File.ReadAllText(queryFilePath + "\\ExchangesTicketsBreakdown.sql");
                    tempDt = new DataTable();
                    tempDt = reportsList.GetQueryOutput(query, selectedParameters);
                    dgvExchangesTickets.DataSource = tempDt;
                    setupGridviews(ref dgvExchangesTickets);
                    Common.Utilities.setLanguage(dgvExchangesTickets);

                    dgv.GridColor = Color.White;
                    dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
                    dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                    dgv.AllowUserToAddRows = false;
                    dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.White;

                    addDatagridToPrintGrid(ref dgv, dgvPaymentMode, Common.MessageUtils.getMessage("Payment Breakdown"));
                    addDatagridToPrintGrid(ref dgv, dgvCashier, 0, dgvCashier.ColumnCount - 2, Common.MessageUtils.getMessage("Cashier Breakdown"));
                    addDatagridToPrintGrid(ref dgv, dgvProducts, 1, dgvProducts.Columns.Count - 1, Common.MessageUtils.getMessage("Product Breakdown"));
                    addDatagridToPrintGrid(ref dgv, dgvDiscounts, Common.MessageUtils.getMessage("Discount Breakdown"));
                    addDatagridToPrintGrid(ref dgv, dgvSpecialPricing, Common.MessageUtils.getMessage("Special Pricing"));
                    addDatagridToPrintGrid(ref dgv, dgvTax, Common.MessageUtils.getMessage("Tax Breakdown"));
                    addDatagridToPrintGrid(ref dgv, dgvPOSBreakDown, Common.MessageUtils.getMessage("POS Breakdown"));
                    addDatagridToPrintGrid(ref dgv, dgvTransfers, Common.MessageUtils.getMessage("Card Transfers"));
                    addDatagridToPrintGrid(ref dgv, dgvExchangesTickets, Common.MessageUtils.getMessage("Card Activities"));
                }
                else
                {
                    query = System.IO.File.ReadAllText(queryFilePath + "\\" + reportKey + ".sql");
                    dt = new DataTable();
                    DataTable newDT;
                    dt = reportsList.GetQueryOutput(query, selectedParameters);
                    if (dt == null)
                    {
                        log.Info("No data found.");
                        log.Debug("Ends-exportReportData(reportKey, reportID, reportName, customFlag, fromDate, toDate, FileName, ref message) method. No data found.");
                        return;
                    }
                    switch (reportKey)
                    {
                        case "GameMetric":
                            newDT = Common.Utilities.getReportGridTable(dt, 1, new int[] { 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 18, 19 });
                            dgv.DataSource = newDT;
                            break;
                        case "GameLevelRevenueSummary":
                            newDT = Common.Utilities.getReportGridTable(dt, 0, new int[] { 2, 3, 4 });
                            if (newDT.Rows.Count > 0)
                                dt.Rows.Add(new object[] {newDT.Rows[newDT.Rows.Count - 1][0], //grand total
                                                                            newDT.Rows[newDT.Rows.Count - 1][1],
                                                                            newDT.Rows[newDT.Rows.Count - 1][2],
                                                                            newDT.Rows[newDT.Rows.Count - 1][3],
                                                                            newDT.Rows[newDT.Rows.Count - 1][4]});
                            dgv.DataSource = dt;
                            break;
                        case "GamePerformance":
                            newDT = Common.Utilities.getReportGridTable(dt, 0, new int[] { 1, 2, 3, 4, 5, 6 });
                            if (newDT.Rows.Count > 0)
                                dt.Rows.Add(new object[] {newDT.Rows[newDT.Rows.Count - 1][0], //grand total
                                                                    newDT.Rows[newDT.Rows.Count - 1][1],
                                                                    newDT.Rows[newDT.Rows.Count - 1][2],
                                                                    newDT.Rows[newDT.Rows.Count - 1][3],
                                                                    newDT.Rows[newDT.Rows.Count - 1][4],
                                                                    newDT.Rows[newDT.Rows.Count - 1][5],
                                                                    newDT.Rows[newDT.Rows.Count - 1][6]});
                            dgv.DataSource = dt;
                            break;
                        case "InventoryStatus":
                            newDT = Common.Utilities.getReportGridTable(dt, 0, new int[] { 3, 5, 6, 8, 9, 11, 12, 14, 15, 17, 18, 20, 21, 23 });
                            string totalTL = Common.Utilities.MessageUtils.getMessage("Total");
                            string grandTotalTL = Common.Utilities.MessageUtils.getMessage("Grand Total");

                            for (int i = 0; i < newDT.Rows.Count; i++)
                            {
                                if (newDT.Rows[i][3] == DBNull.Value && newDT.Rows[i][0] != DBNull.Value)
                                {
                                    newDT.Rows.RemoveAt(i);
                                    i = 0;
                                    continue;
                                }

                                if (newDT.Rows[i][0] == DBNull.Value && newDT.Rows[i][2] == DBNull.Value)
                                {
                                    newDT.Rows.RemoveAt(i);
                                    i = 0;
                                    continue;
                                }

                                if (newDT.Rows[i][0].ToString() == grandTotalTL)
                                {
                                    newDT.Rows.RemoveAt(i);
                                }
                            }
                            dgv.DataSource = newDT;
                            break;
                        case "TechCard":
                            newDT = Common.Utilities.getReportGridTable(dt, 0, new int[] { 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 });
                            dgv.DataSource = newDT;
                            break;
                        case "TrxSummary":
                            newDT = Common.Utilities.getReportGridTable(dt, 0, new int[] { 1, 2, 3, 4, 5, 6, 7 });

                            if (newDT.Rows.Count > 0)
                                dt.Rows.Add(new object[] {newDT.Rows[newDT.Rows.Count - 1][0], //grand total
                                                                                    newDT.Rows[newDT.Rows.Count - 1][1],
                                                                                    newDT.Rows[newDT.Rows.Count - 1][2],
                                                                                    newDT.Rows[newDT.Rows.Count - 1][3],
                                                                                    newDT.Rows[newDT.Rows.Count - 1][4],
                                                                                    newDT.Rows[newDT.Rows.Count - 1][5],
                                                                                    newDT.Rows[newDT.Rows.Count - 1][6],
                                                                                    newDT.Rows[newDT.Rows.Count - 1][7]
                                                });
                            dgv.DataSource = dt;
                            break;
                        case "ReceiptsBySupplier":
                            newDT = Common.Utilities.getReportGridTable(dt, 0, new int[] { 3 });

                            for (int i = 0; i < newDT.Rows.Count; i++)
                            {
                                if (newDT.Rows[i][0] == DBNull.Value && newDT.Rows[i][1] == DBNull.Value)
                                {
                                    newDT.Rows.RemoveAt(i);
                                    i = 0;
                                    continue;
                                }

                                if (newDT.Rows[i][0].ToString().Contains("Total") && newDT.Rows[i][2] == DBNull.Value)
                                {
                                    newDT.Rows[i][1] = Common.Utilities.MessageUtils.getMessage("Total");
                                    continue;
                                }
                            }

                            if (newDT.Rows.Count > 0)
                                newDT.Rows.RemoveAt(newDT.Rows.Count - 1); // remove grand total row
                            newDT.Columns.RemoveAt(0);

                            newDT.Rows.Add(new object[] { null });
                            newDT.Rows.Add(new object[] { null });
                            newDT.Rows.Add(new object[] { null });

                            newDT.Rows.Add(new object[] { Common.Utilities.MessageUtils.getMessage("Stores Incharge."), "    " + Common.MessageUtils.getMessage("Manager") });
                            newDT.Rows.Add(new object[] { null });
                            newDT.Rows.Add(new object[] { null });
                            newDT.Rows.Add(new object[] { Common.Utilities.MessageUtils.getMessage("Remarks, if any") });
                            newDT.Rows.Add(new object[] { null });
                            newDT.Rows.Add(new object[] { null });
                            newDT.Rows.Add(new object[] { null, Common.Utilities.MessageUtils.getMessage("General Manager") });
                            newDT.Rows.Add(new object[] { null });

                            dgv.DataSource = newDT;
                            break;
                        default:
                            dgv.DataSource = dt;
                            break;
                    }
                    setupGridviews(ref dgv);
                    Common.Utilities.setLanguage(dgv);
                }
                string reportTitle = getReportTitle(reportName, ParafaitEnv.SiteName, fromDate, toDate);
                Common.createHTMLFile(dgv, reportTitle, FileName);
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                appendMessage(ex.Message, ref message);
                log.LogMethodExit();
            }
        }


        /// <summary>
        /// getReportTitle method
        /// </summary>
        /// <param name="title">title</param>
        /// <param name="site_name">site_name</param>
        /// <param name="fromDate">fromDate</param>
        /// <param name="toDate">toDate</param>
        /// <param name="otherParams">otherParams</param>
        /// <returns>returns string</returns>
        public static string getReportTitle(string title, string site_name, DateTime fromDate, DateTime toDate, params string[] otherParams)
        {
            log.LogMethodEntry(title, site_name, fromDate, toDate, otherParams);
            try
            {
                string otherParamValues = string.Empty;
                if (otherParams != null)
                {
                    if (otherParams.Length > 0)
                    {
                        for (int k = 0; k < otherParams.Length; k++)
                        {
                            if (otherParams[k] == null)
                                continue;

                            otherParamValues += MessageUtils.getMessage(otherParams[k]) + Environment.NewLine;
                        }
                    }
                }
                log.LogMethodExit();
                return
                    title +
                    Environment.NewLine +
                    MessageUtils.getMessage("Site") + ": " +
                    site_name +
                    Environment.NewLine +
                    MessageUtils.getMessage("Period From") + " " +
                    fromDate.ToString("dddd, dd-MMM-yyyy h:mm tt") +
                    " " + MessageUtils.getMessage("To") + " " +
                    toDate.ToString("dddd, dd-MMM-yyyy h:mm tt") +
                    Environment.NewLine +
                    otherParamValues +
                    "[" + MessageUtils.getMessage("Report Run at") + ": " + Common.Utilities.getServerTime().ToString(Common.Utilities.getDateTimeFormat()) + "]" +
                    Environment.NewLine +
                    Environment.NewLine;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit();
                return "";
            }
        }


        /// <summary>
        /// GetCustomReportBarChartSource method
        /// </summary>
        /// <param name="connectionString">connectionString</param>
        /// <param name="DBQuery">DBQuery</param>
        /// <param name="reportName">reportName</param>
        /// <param name="FromDate">FromDate</param>
        /// <param name="ToDate">ToDate</param>
        /// <param name="UserID">UserID</param>
        /// <param name="SelectedParameters">SelectedParameters</param>
        /// <param name="HideBreakColumn">HideBreakColumn</param>
        /// <param name="breakColumn">breakColumn</param>
        /// <param name="message">message</param>
        /// <param name="otherParams">otherParamss</param>
        /// <param name="reportId">reportId</param>
        /// <returns>returns Telerik.Reporting.Report</returns>
        public static Telerik.Reporting.Report GetCustomReportBarChartSource(string connectionString, int reportId, string DBQuery, string reportName, DateTime FromDate, DateTime ToDate, string UserID, List<SqlParameter> SelectedParameters, bool HideBreakColumn, int breakColumn, ref string message, params string[] otherParams)
        {
            log.LogMethodEntry(connectionString, reportId, DBQuery, reportName, FromDate, ToDate, UserID, SelectedParameters, HideBreakColumn, breakColumn, message, otherParams);
            try
            {
                ReportsList reportsList = new ReportsList();
                DataTable dt = reportsList.GetCustomReportData(DBQuery, SelectedParameters);

                var connectionStringHandler = new ReportConnectionstringManager(Utilities.getConnection().ConnectionString, "");

                if(dt == null)
                {
                    message = "No Data";
                    log.LogMethodExit();
                    return null;
                }
                Telerik.Reporting.Report report = new dynamicBarChart(dt, reportId, FromDate, ToDate, reportName, 0, Common.ParafaitEnv.User_Id, Common.ParafaitEnv.RoleId, Common.getConnectionString(), Common.ParafaitEnv.SiteName);
                message = "Success";
                log.LogMethodExit(report);
                return report;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                message = ex.Message;
                log.LogMethodExit();
                return null;
            }
        }


        /// <summary>
        /// ShowEmailOnload method
        /// </summary>
        /// <returns>returns bool</returns>
        public static bool ShowEmailOnload()
        {
            log.LogMethodEntry();
            ReportsList reportsList = new ReportsList();
            DataTable dtData = reportsList.GetTransactionTotalCount();

            bool ShowEmail = false;
            if (dtData != null)
            {
                var count = Utilities.getParafaitDefaults("REPORT_TRANSACTION_COUNT_FOR_AUTO_EMAIL");
                count = string.IsNullOrEmpty(count) ? "1000000" : count;

                if (Convert.ToInt64(dtData.Rows[0][5]) >= Convert.ToInt64(count))
                    ShowEmail = true;
                else
                    ShowEmail = false;
            }
            log.LogMethodExit(ShowEmail);
            return ShowEmail;
        }


        /// <summary>
        /// GetTimeBasedSchedulesFromToDate method
        /// </summary>
        /// <param name="date">date</param>
        /// <param name="siteId">siteId</param>
        /// <param name="scheduleId">scheduleId</param>
        /// <param name="fromDate">fromDate</param>
        /// <param name="toDate">toDate</param>
        ///  <param name="scheduleName">scheduleName</param>
        /// <returns>returns bool</returns>
        public static bool GetScheduleFromToDate(DateTime date, int siteId, int scheduleId, ref DateTime fromDate, ref DateTime toDate, ref string scheduleName)
        {
            log.LogMethodEntry(date, siteId, scheduleId, fromDate, toDate, scheduleName);
            try
            {
                ReportScheduleList reportScheduleList = new ReportScheduleList();
                ReportScheduleDTO reportScheduleDTO = reportScheduleList.GetReportSchedule(scheduleId);

                if (reportScheduleDTO == null || reportScheduleDTO.ScheduleId <= 0)
                {
                    log.LogMethodExit(false);
                    return false;
                }

                scheduleName = reportScheduleDTO.ScheduleName;
                double businessDayStartTime = 6;
                double.TryParse(Common.Utilities.getParafaitDefaults("BUSINESS_DAY_START_TIME"), out businessDayStartTime);

                if (reportScheduleDTO.Frequency.ToString() == "100") // monthly
                {
                    toDate = DateTime.Now.Date.AddHours(businessDayStartTime); // run until 6AM of first day of this month
                    fromDate = DateTime.Now.Date.AddMonths(-1).AddHours(businessDayStartTime); // since first day of last month
                }
                else
                
                if (reportScheduleDTO.IncludeDataFor <= 0)
                {
                    // toDate = DateTime.Now.Date.AddHours(run_at); // run until schedule run time since 6AM
                    int wholeNumber = (int)reportScheduleDTO.RunAt;
                    int decimalPortion = (int)((reportScheduleDTO.RunAt - (int)reportScheduleDTO.RunAt) * 100);

                    toDate = DateTime.Now.Date.AddHours(wholeNumber);
                    toDate = toDate.AddMinutes(decimalPortion);

                    fromDate = DateTime.Now.Date.AddHours(businessDayStartTime);
                }
                else if (reportScheduleDTO.Frequency.ToString() == "1001") // first day of month
                {
                    toDate = DateTime.Now.Date.AddHours(businessDayStartTime); // run u0ntil 6AM today
                    int prevDay = toDate.AddDays(-1).Day;
                    fromDate = toDate.AddDays(31 - prevDay - reportScheduleDTO.IncludeDataFor);
                }
                else
                {
                    toDate = DateTime.Now.Date.AddHours(businessDayStartTime); // run until 6AM today
                    fromDate = toDate.AddDays(-1 * reportScheduleDTO.IncludeDataFor);
                }
                log.LogMethodExit(true);
                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit();
                return false;
            }
        }


        /// <summary>
        /// GetScheduledEmailList method
        /// </summary>
        /// <param name="schedule_id">schedule_id</param>
        /// <param name="siteConnectionString"></param>
        /// <returns>returns email list</returns>
        public static string GetScheduledEmailList(int schedule_id, string siteConnectionString)
        {
            log.LogMethodEntry(schedule_id, siteConnectionString);
            DataTable dt = new DataTable();

            ReportScheduleEmailList reportScheduleEmailList = new ReportScheduleEmailList(siteConnectionString);
            dt = reportScheduleEmailList.GetReportScheduleEmailListByScheduleID(schedule_id);

            if (dt == null || dt.Rows.Count == 0)
            {
                log.Info("No email ids specified to send mail");
                return null;
            }
            string ToEmails = string.Empty;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ToEmails += ", " + dt.Rows[i]["emailid"].ToString();
            }
            ToEmails = ToEmails.Substring(2);
            log.Info("ToEmails" + ToEmails);
            log.LogMethodExit(ToEmails);
            return ToEmails;
        }


        /// <summary>
        /// DeleteOldReportFiles method
        /// </summary>
        public static void DeleteOldReportFiles()
        {
            log.LogMethodEntry();
            try
            {
                int reportsRetainDays = 3;
                string reportsPath = Utilities.getParafaitDefaults("PDF_OUTPUT_DIR");
                try
                {
                    reportsRetainDays = Convert.ToInt32(Convert.ToDouble(Utilities.getParafaitDefaults("RETAIN_REPORT_FILES_FOR_DAYS")));
                    if (reportsRetainDays <= 0)
                        reportsRetainDays = 3;
                }
                catch(Exception ex)
                {
                    log.Error(ex);
                }
                log.Info("Retains reports for days:" + reportsRetainDays.ToString());
                reportsRetainDays = -1 * reportsRetainDays;

                log.Info("Deleting old [" + reportsRetainDays.ToString() + " days] remote backup files...");
                foreach (string f in System.IO.Directory.GetFiles(reportsPath))
                {
                    if (System.IO.File.GetCreationTime(f) < DateTime.Now.AddDays(reportsRetainDays))
                    {
                        try
                        {
                            if (!f.Contains(".trdx"))
                            {
                                System.IO.File.Delete(f);
                                log.Info("Deleting report file " + f);
                            }

                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                            continue;
                        }
                    }
                }
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }


        /// <summary>
        /// GetReportId method
        /// </summary>
        /// <param name="reportKey">reportKey</param>
        /// <param name="siteId">siteId</param>
        /// <returns>returns int</returns>
        public static int GetReportId(string reportKey, int siteId)
        {
            log.LogMethodEntry(reportKey, siteId);
            int reportId = -1;
            try
            {
                ReportsList reportsList = new ReportsList();
                List<KeyValuePair<ReportsDTO.SearchByReportsParameters, string>> searchParameters = new List<KeyValuePair<ReportsDTO.SearchByReportsParameters, string>>();
                searchParameters.Add(new KeyValuePair<ReportsDTO.SearchByReportsParameters, string>(ReportsDTO.SearchByReportsParameters.REPORT_KEY, reportKey));
                searchParameters.Add(new KeyValuePair<ReportsDTO.SearchByReportsParameters, string>(ReportsDTO.SearchByReportsParameters.SITE_ID, siteId.ToString()));
                List<ReportsDTO> ReportsDTOList = reportsList.GetAllReports(searchParameters);
                if (ReportsDTOList == null || ReportsDTOList.Count == 0)
                {
                    reportId = -1;
                }
                else
                {
                    reportId = ReportsDTOList[0].ReportId;
                }

            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit(reportId);
            return reportId;
        }


        /// <summary>
        /// GetSiteId method
        /// </summary>
        /// <returns>returns int</returns>
        public static int GetSiteId()
        {
            log.LogMethodEntry();
            int siteId = -1;
            try
            {
                SqlConnection connection = Common.Utilities.DBUtilities.sqlConnection; ;
                SqlCommand cmd = new SqlCommand();
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                cmd.Connection = connection;
                cmd.CommandText = "SELECT * FROM site  ";
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    cmd.Connection = connection;
                    cmd.CommandText = "select master_site_id from Company";
                    da = new SqlDataAdapter(cmd);
                    DataTable dtcm = new DataTable();

                    da.Fill(dtcm);
                    siteId = dtcm.Rows[0][0] == DBNull.Value ? -1 : Convert.ToInt32(dtcm.Rows[0][0]);
                }
            }
            catch (Exception ex)
            {
                siteId = -1;
                log.Error(ex);
            }
            log.LogMethodExit(siteId);
            return siteId;
        }


        /// <summary>
        /// IsServer Method 
        /// </summary>
        /// <returns>
        /// returns whether report is run from POS application or Web
        /// </returns>
        public static bool IsServer(string type = "W")
        {
            log.LogMethodEntry(type);
            try
            {
                log.Info("Begin-IsServer() method  ");
                string path = string.Empty;
                if (type.ToLower() == "f")
                {
                    string cpath = System.IO.Directory.GetCurrentDirectory();
                    path = Directory.GetParent(cpath).ToString();
                }
                else
                {
                    string cpath = AppDomain.CurrentDomain.BaseDirectory;
                    cpath = Directory.GetParent(cpath).ToString();
                    path = Directory.GetParent(cpath).ToString();
                }
                path = path + "\\Server";

                log.Info("path is " + path);
                if (Directory.Exists(path))
                {
                    log.Info("Directory is exist");
                    return true;
                }
                log.LogMethodExit(false);
                return false;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(false);
                return false;
            }
        }


        /// <summary>
        /// UpdateTelerikConfigs method
        /// </summary>
        public static void UpdateTelerikConfigs()
        {
            log.LogMethodEntry();
            int configChangeCount = 0;
            bool isRuntimeConfigExist = false;
            bool isTelerikAssemblyConfigExist = false;

            string runtimeTelerikConfig = "<runtime>" +
                                           "<assemblyBinding xmlns='urn:schemas-microsoft-com:asm.v1'>" +
                                            "<dependentAssembly>" +
                                            "<assemblyIdentity name='Telerik.Reporting' publicKeyToken='a9d7983dfcc261be' culture='neutral' />" +
                                            "<bindingRedirect oldVersion='0.0.0.0-14.1.20.513' newVersion='14.1.20.513' /> " +
                                            "</dependentAssembly>" +
                                            "</assemblyBinding>" +
                                            "</runtime >";
            string telerikAssemblyConfig = @"<Telerik.Reporting>" +
                                             "<AssemblyReferences>" +
                                             "<add name='Reports' version='1.0.0.0' />" +
                                             "<add name='MessagesFunctions' version='1.0.0.0' />" +
                                             "</AssemblyReferences>" +
                                             "</Telerik.Reporting>";

            string telerikConfigSection = @"<section name='Telerik.Reporting' type='Telerik.Reporting.Configuration.ReportingConfigurationSection, Telerik.Reporting, Version=14.1.20.513, Culture = neutral, PublicKeyToken = a9d7983dfcc261be' allowLocation='true' allowDefinition='Everywhere' />";

            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                foreach (XmlElement element in xmlDoc.DocumentElement.Cast<XmlNode>().Where(n => n.NodeType != XmlNodeType.Comment))
                {
                    if (element.Name == "configSections")
                    {
                        if (!element.InnerXml.Contains("Telerik.Reporting"))
                        {
                            element.InnerXml = element.InnerXml.Insert(0, telerikConfigSection);
                            configChangeCount++;
                        }
                        else if(element.InnerXml.Contains("11.1.17.614"))
                        {
                            element.InnerXml = element.InnerXml.Replace("11.1.17.614", "14.1.20.513");
                            configChangeCount++;
                        }
                    }

                    if (element.Name == "runtime")
                    {
                        if (element.InnerXml.Contains("Telerik.Reporting"))
                        {
                            isRuntimeConfigExist = true;
                            if (element.InnerXml.Contains("11.1.17.614"))
                            {
                                element.InnerXml = element.InnerXml.Replace("11.1.17.614", "14.1.20.513");
                                configChangeCount++;                                
                            }                           
                        }
                    }
                    if (element.Name == "Telerik.Reporting")
                    {
                        if (element.InnerXml.Contains("Reports") && element.InnerXml.Contains("MessagesFunctions"))
                        {
                            isTelerikAssemblyConfigExist = true;
                        }
                    }
                }

                //create a new element if runtime config element doesn't exists
                if (!isRuntimeConfigExist)
                {
                    XmlDocumentFragment xfrag = xmlDoc.CreateDocumentFragment();
                    xfrag.InnerXml = runtimeTelerikConfig;
                    xmlDoc.DocumentElement.AppendChild(xfrag);
                }

                //create a new element if TelerikAssemblyConfig doesn't exists
                if (!isTelerikAssemblyConfigExist)
                {
                    XmlDocumentFragment xfrag = xmlDoc.CreateDocumentFragment();
                    xfrag.InnerXml = telerikAssemblyConfig;
                    xmlDoc.DocumentElement.AppendChild(xfrag);
                }

                if (!isRuntimeConfigExist || !isTelerikAssemblyConfigExist || configChangeCount >0)
                {
                    xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                    ConfigurationManager.RefreshSection("configSections");
                    ConfigurationManager.RefreshSection("runtime");
                    ConfigurationManager.RefreshSection("Telerik.Reporting");
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
        /// GetAmountFormatDecimalPoints method
        /// </summary>
        /// <param name="decimalSeperator"></param>
        /// <returns></returns>
        public static int GetAmountFormatDecimalPoints(char decimalSeperator)
        {
            log.LogMethodEntry(decimalSeperator);
            int decimalPoints = 2;
            try
            {
                string amountFormat = ParafaitEnv.AMOUNT_FORMAT;
                string[] splitAmountFormat = amountFormat.Split(new char[] { decimalSeperator });
                if (splitAmountFormat.ToList().Count > 1)
                {
                    decimalPoints = splitAmountFormat[1].Length;
                }
                else if (splitAmountFormat.ToList().Count == 1)
                {
                    decimalPoints = 0;
                }

                log.LogMethodExit(decimalPoints);
                return decimalPoints;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(decimalPoints);
                return decimalPoints;

            }
        }


        /// <summary>
        /// GetArrayListFromCSV method
        /// </summary>
        /// <param name="csvString"></param>
        /// <returns></returns>
        public static ArrayList GetArrayListFromCSV(string csvString)
        {
            log.LogMethodEntry(csvString);
            ArrayList csvArrayList = new ArrayList();
            try
            {
                if (!string.IsNullOrEmpty(csvString))
                {
                    string[] csvStringValues = csvString.Split(',');
                    foreach (string csvValue in csvStringValues)
                    {
                        csvArrayList.Add(csvValue);
                    }
                    log.LogMethodExit(csvArrayList);
                    return csvArrayList;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit();
                return null;
            }
            log.LogMethodExit();
            return null;
        }


        /// <summary>
        /// SetSiteCulture method
        /// </summary>
        public static void SetSiteCulture()
        {
            log.LogMethodEntry();
            string culture = "en-US";
            int decimalPoints = 2;
            try
            {
                string defaultLanguage = Utilities.getParafaitDefaults("DEFAULT_LANGUAGE");
                if (!string.IsNullOrEmpty(defaultLanguage))
                {
                    int defaultLangId = Convert.ToInt32(defaultLanguage);
                    List<LanguagesDTO> languagesDTOList = new List<LanguagesDTO>();
                    List<KeyValuePair<LanguagesDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<LanguagesDTO.SearchByParameters, string>>();
                    searchParameters.Add(new KeyValuePair<LanguagesDTO.SearchByParameters, string>(LanguagesDTO.SearchByParameters.LANGUAGE_ID, defaultLangId.ToString()));
                    Semnox.Parafait.Languages.Languages languages = new Semnox.Parafait.Languages.Languages(Utilities.ExecutionContext);
                    languagesDTOList = languages.GetAllLanguagesList(searchParameters);

                    if (languagesDTOList != null && languagesDTOList.Count > 0)
                    {
                        culture = languagesDTOList[0].CultureCode;
                    }
                    if (!string.IsNullOrEmpty(culture))
                    {
                        System.Globalization.CultureInfo currentCulture = System.Globalization.CultureInfo.GetCultureInfo(culture);
                        char decimalSeparator = Convert.ToChar(currentCulture.NumberFormat.NumberDecimalSeparator.ToString());
                        decimalPoints = GetAmountFormatDecimalPoints(decimalSeparator);
                    }
                }
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }
            cultureCode = culture;
            amountFormatDecimalPoints = decimalPoints;
            log.LogMethodExit();
        }


        /// <summary>
        /// GetArrayListFromDataTable method
        /// </summary>
        /// <param name="dsTable"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static ArrayList GetArrayListFromDataTable(DataTable dsTable,string columnName)
        {
            ArrayList dsArrayList = new ArrayList();
            try
            {
                if (dsTable != null)
                {
                    foreach (DataRow dr in dsTable.Rows)
                    {
                        dsArrayList.Add(Convert.ChangeType(dr[columnName], Type.GetType(dsTable.Columns[columnName].DataType.ToString())));
                    }
                    return dsArrayList;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
            return null;
        }


        /// <summary>
        /// GetCSVFromArrayList method
        /// </summary>
        /// <param name="arrayList"></param>
        /// <returns></returns>
        public static string GetCSVFromArrayList(ArrayList arrayList)
        {
            string CSVString;
            try
            {
                if (arrayList != null)
                {
                    CSVString = String.Join(",", arrayList.ToArray());
                    return CSVString;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return "";
            }
            return "";
        }



        /// <summary>
        /// GetAggregateColumnList method
        /// </summary>
        /// <param name="AggrColumns"></param>
        /// <returns></returns>
        public static ArrayList GetAggregateColumnList(string AggrColumns)
        {
            log.LogMethodEntry(AggrColumns);
            try
            {
                ArrayList AggColList = new ArrayList();
                if (!string.IsNullOrWhiteSpace(AggrColumns))
                {
                    string[] AggrColumnsList = AggrColumns.Split(',');

                    foreach (string columnNo in AggrColumnsList)
                    {
                        AggColList.Add(columnNo);
                    }
                    return AggColList;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
            return null;
        }


        /// <summary>
        /// GetEmailSubjectOnDemand method
        /// </summary>
        /// <param name="reportName"></param>
        /// <returns></returns>
        public static string GetEmailSubjectOnDemand(string reportName)
        {
            log.LogMethodEntry(reportName);
            string emailSubject = string.Empty;
            try
            {
                emailSubject = MessageContainerList.GetMessage(Semnox.Core.Utilities.ExecutionContext.GetExecutionContext(),2220, reportName);  // "Parafait On Demand Email Report - &1";         
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit(emailSubject);
            return emailSubject;
        }

        /// <summary>
        /// IsGreaterVersion Method
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        public static bool IsPasswordDecryptionRequired(string version)
        {
            log.LogMethodEntry(version);
            bool isGreaterVersion = false;
            try
            {
                string[] arr;

                string[] separators = { "." };
                arr = version.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                if (arr.Length > 0)
                {
                    if (Convert.ToInt32(arr[0]) >= 2)
                    {
                        if (Convert.ToInt32(arr[0]) == 2 && Convert.ToInt32(arr[1]) < 50)
                            isGreaterVersion = false;
                        else
                            isGreaterVersion = true;
                    }

                }
            }
            catch (Exception ex)
            {
                log.Error("Ends-IsGreaterVersion method with exception1: " + ex.Message);
            }
            log.LogMethodExit(isGreaterVersion);
            return isGreaterVersion;
        }

        public static List<KeyValuePair<string, string>> GetTableauDashboadLinks()
        {
            log.LogMethodEntry();
            List<KeyValuePair<string, string>> tableauDashboardURL = new List<KeyValuePair<string, string>>();
            try
            {
                LookupValuesList lookUpList = new LookupValuesList(Semnox.Core.Utilities.ExecutionContext.GetExecutionContext());
                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookUpValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                lookUpValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "TABLEAU_DASHBOARDS"));
                List<LookupValuesDTO> lookUpValuesList = lookUpList.GetAllLookupValues(lookUpValuesSearchParams);

                if (lookUpValuesList != null && lookUpValuesList.Count > 0)
                {
                    foreach (LookupValuesDTO valueDTO in lookUpValuesList)
                    {
                        tableauDashboardURL.Add(new KeyValuePair<string, string>(valueDTO.LookupValue, valueDTO.Description));
                    }
                }

            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

            return tableauDashboardURL;
        }

        public static string GetMessage(int messageNo, params object[] parameters)
        {
            log.LogMethodEntry(messageNo, parameters);
            string message = "";
            message = MessageContainerList.GetMessage(Semnox.Core.Utilities.ExecutionContext.GetExecutionContext(), messageNo, parameters);
            log.LogMethodExit(message);
            return message;
        }

        public static int GetTimeSpanInDays(DateTime fromDate, DateTime toDate)
        {
            log.LogMethodEntry(fromDate, toDate);
            int differenceInDays = -1;
            try
            {
                TimeSpan timeSpan = toDate - fromDate;
                differenceInDays = timeSpan.Days;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit(differenceInDays);
            return differenceInDays;
        }


        /// <summary>
        /// BuildQueryString
        /// </summary>
        /// <param name="nvc"></param>
        /// <returns></returns>
        public static string BuildQueryString(NameValueCollection nvc)
        {
            log.LogMethodEntry(nvc);
            StringBuilder sb = new StringBuilder("?");
            bool first = true;

            foreach (string key in nvc.AllKeys)
            {
                foreach (string value in nvc.GetValues(key))
                {
                    if (!first)
                    {
                        sb.Append("&");
                    }
                    sb.AppendFormat("{0}={1}", Uri.EscapeDataString(key), Uri.EscapeDataString(value));
                    first = false;
                }
            }
            log.LogMethodExit(sb.ToString());
            return sb.ToString();
        }

        public static DataTable GetGroupIndexedDatatable(DataTable dt, int rowsInGroup)
        {
            log.LogMethodEntry(dt, rowsInGroup);
            if (dt != null && dt.Columns[dt.Columns.Count - 1].ColumnName != "GroupIndexer")
            {
                dt.Columns.Add("GroupIndexer", typeof(Int32));
                int groupIndexer = 0;
                int groupCount = 0;
                foreach (DataRow row in dt.Rows)
                {
                    if (groupCount == rowsInGroup)
                    {
                        groupIndexer++;
                        groupCount = 0;
                    }
                    row["GroupIndexer"] = groupIndexer + 1;
                    groupCount++;
                }
            }
            return dt;
        }


        /// <summary>
        /// getParafaitDefaults
        /// </summary>
        /// <param name="default_value_name"></param>
        /// <returns>ParafaitDefaults vlaue</returns>
        public static string getParafaitDefaults(string default_value_name)
        {
            log.LogMethodEntry(default_value_name);

            object returnValue = Utilities.executeScalar(@"select isnull(pos.optionvalue, isnull(us.optionValue, pd.default_value)) value 
                                                        from parafait_defaults pd 
                                                        left outer join ParafaitOptionValues pos 
                                                        on pd.default_value_id = pos.optionId 
                                                        and POSMachineId = @POSMachineId 
                                                        and pos.activeFlag = 'Y' 
                                                        left outer join ParafaitOptionValues us 
                                                        on pd.default_value_id = us.optionId 
                                                        and us.UserId = @UserId 
                                                        and us.activeFlag = 'Y' 
                                                        where pd.active_flag = 'Y' 
                                                        and default_value_name = @default_value_name 
                                                        and (pd.site_id = @site_id or @site_id = -1)",
                                                        new SqlParameter("@default_value_name", default_value_name),
                                                        new SqlParameter("@POSMachineId", ParafaitEnv.POSMachineId),
                                                        new SqlParameter("@UserId", ParafaitEnv.User_Id),
                                                        new SqlParameter("@site_id", (ParafaitEnv.IsCorporate) ? ParafaitEnv.SiteId : -1)
                                                        );
            log.LogVariableState("returnValue", returnValue);
            log.LogMethodExit();
            if (returnValue == null || returnValue == DBNull.Value)
            { return ""; }
            else
            { return returnValue.ToString(); }
        }


        /// <summary>
        /// IsTrdxReportParametersVisible
        /// </summary>
        /// <param name="reportKey"></param>
        /// <param name="reportName"></param>
        /// <param name="isBackground"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsTrdxReportParametersVisible(string reportKey, string reportName, bool isBackground, string type = "W")
        {
            log.LogMethodEntry(reportKey, reportName, isBackground, type);
            bool parameterVisible = false;
            try
            {
                Telerik.Reporting.ReportSource reportSource;
                DateTime startDate = DateTime.Now;
                string reportFilePath = string.Empty;

                if (type == "F")
                {
                    string path = System.IO.Directory.GetCurrentDirectory();
                    reportFilePath = path + "\\Reports\\" + reportKey + ".trdx";
                }
                else
                {
                    string path = AppDomain.CurrentDomain.BaseDirectory;
                    reportFilePath = path + "\\Reports\\" + reportKey + ".trdx";
                }

                Telerik.Reporting.UriReportSource URI = new Telerik.Reporting.UriReportSource();
                URI.Uri = reportFilePath;
                XmlReaderSettings settings = new XmlReaderSettings();
                var connectionString = Common.getConnectionString();
                settings.IgnoreWhitespace = true;

                var connectionStringHandler = new ReportConnectionstringManager(Utilities.getConnection().ConnectionString, "");

                var report = new Telerik.Reporting.Report();
                report = Common.DeserializeReport(reportFilePath);

                report.DocumentName = ((reportName + " " + DateTime.Now.ToString("dd-MM-yyyy hh-mm")).TrimStart()).TrimEnd();

                var sourceReportSource = new InstanceReportSource { ReportDocument = report };
                reportSource = connectionStringHandler.UpdateReportSource(sourceReportSource, isBackground, type);

                if (reportSource != null)
                {
                    //Reads the built in parameters of the TRDX file
                    Telerik.Reporting.InstanceReportSource reportSourceInstance = (Telerik.Reporting.InstanceReportSource)reportSource;
                    Telerik.Reporting.IReportDocument reportDoc = reportSourceInstance.ReportDocument;
                    IEnumerable<Telerik.Reporting.IReportParameter> reportParams = reportDoc.ReportParameters;
                    foreach (Telerik.Reporting.IReportParameter reportParameter in reportParams)
                    {
                        if (reportParameter.Visible == true)
                        {
                            //When one of the TRDX Prameters is set visible
                            parameterVisible = true;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit();
            }
            return parameterVisible;
        }
    }
}
