/********************************************************************************************
 * Project Name - DPL Processor
 * Description  - DPL class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *1.20.1      23-May-2021   Deeksha        Modified for Concurrent Program AWS changes 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Semnox.Core.Utilities;
using Semnox.Parafait.JobUtils;
using Semnox.Parafait.Communication;

namespace Semnox.Parafait.Inventory
{
    public class DPLProcessor
    {
        string filePath;
        string backupFolderPath;
        string errorFolderPath;
        string dplFileName;
        //bool moreFilesToProcess;
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Utilities utilities; 
        private const string EMAILMESSAGINGCHANELTYPE = "E";
        private const string DPL_SUMMARY = "DPL Program Summary";


        public DPLProcessor(Utilities utilities)
        {
            log.LogMethodEntry();
            //Debugger.Launch();
            this.utilities = utilities; 
            filePath = GetFilePath();
            backupFolderPath = filePath + @"\BackupFolder";
            errorFolderPath = filePath + @"\ErrorFolder"; 
            log.LogMethodExit();
        }

        private void SetContextSiteId()
        {
            log.LogMethodEntry(); 
            if (utilities.ParafaitEnv.IsCorporate)
            {
                utilities.ExecutionContext.SetSiteId(utilities.ParafaitEnv.SiteId);
            }
            else
            {
                utilities.ExecutionContext.SetSiteId(-1);
            }
            utilities.ExecutionContext.SetUserId(utilities.ParafaitEnv.LoginID);
            log.LogMethodExit();
        }
        private string GetFilePath()
        {
            log.LogMethodEntry();
            string fileFolderPath = "";
            LookupValuesList lookupValuesList = new LookupValuesList(utilities.ExecutionContext);
            List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchLVParameters;
            searchLVParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
            searchLVParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "DPL_FILE_FOLDER_PATH"));
            searchLVParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, "DPL_FILE_FOLDER"));
            if (utilities.ExecutionContext.IsCorporate)
            {
                searchLVParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, utilities.ExecutionContext.SiteId.ToString()));
            }
            List<LookupValuesDTO> lookupValuesListDTO = lookupValuesList.GetAllLookupValues(searchLVParameters);
            if (lookupValuesListDTO != null)
            {
                fileFolderPath = lookupValuesListDTO[0].Description;
            }
            log.LogMethodExit(fileFolderPath);
            return fileFolderPath;
        }

        private void VerifyFolderPaths()
        { 
            log.LogMethodEntry();
            StringBuilder errorMsg = new StringBuilder();
            if (!System.IO.Directory.Exists(this.filePath))
                errorMsg.Append("Folder " + this.filePath.ToString() + " not found. ");
            if (!System.IO.Directory.Exists(this.backupFolderPath))
                errorMsg.Append("Folder " + this.backupFolderPath.ToString() + " not found. ");
            if (!System.IO.Directory.Exists(this.errorFolderPath))
                errorMsg.Append("Folder " + this.errorFolderPath.ToString() + " not found. ");
            if (errorMsg.ToString().Length > 0)
                throw new Exception(errorMsg.ToString());
            log.LogMethodExit(); 
        }

        private bool FindFileToProcess()
        {
            log.LogMethodEntry();
            bool retVal = true;
            //moreFilesToProcess = false;
            if (System.IO.Directory.Exists(this.filePath))
            {
                // string[] filesInPath = Directory.GetFiles(this.filePath, "*.csv");
                string[] filesInPath = Directory.GetFiles(this.filePath, "*.*", SearchOption.TopDirectoryOnly).Where(s => s.ToLower().EndsWith(".csv") || s.ToLower().EndsWith(".dpl")).ToArray();
                DirectoryInfo directoryInfo = new DirectoryInfo(this.filePath);
                DateTime lastUpdateTime;
                string fileName="";
                //var csvFile = directoryInfo.GetFiles("*.csv", SearchOption.TopDirectoryOnly).OrderBy(t => t.LastWriteTime).ToList();
                var csvFile = directoryInfo.GetFiles().Where(f => f.Extension.ToLower() == ".csv" || f.Extension.ToLower() == ".dpl").OrderBy(t => t.LastWriteTime).ToList();
                if (csvFile.Count == 0)
                    retVal = false;
                else
                {
                    fileName = csvFile[0].FullName;
                    lastUpdateTime = csvFile[0].LastWriteTime;
                    int loopCount = 0;
                    do
                    {
                        //Loop 3 times ensure file being selected is not being updated while it is being picked for processing
                        //csvFile = directoryInfo.GetFiles("*.csv", SearchOption.TopDirectoryOnly).OrderBy(t => t.LastWriteTime).ToList();
                        csvFile = directoryInfo.GetFiles().Where(f => f.Extension.ToLower() == ".csv" || f.Extension.ToLower() == ".dpl").OrderBy(t => t.LastWriteTime).ToList();
                        if (csvFile.Count == 0)
                        {
                            retVal = false;
                            loopCount = 3;
                        }
                        else
                        {
                            if (csvFile[0].FullName == fileName && csvFile[0].LastWriteTime == lastUpdateTime)
                            {
                                System.Threading.Thread.Sleep(1000);
                                loopCount += 1;
                            }
                            else
                            {
                                fileName = csvFile[0].FullName;
                                lastUpdateTime = csvFile[0].LastWriteTime;
                                System.Threading.Thread.Sleep(1000);
                                loopCount = 0;
                            }
                        }
                    } while (loopCount < 3);
                    if (csvFile.Count > 0)
                    {
                        dplFileName = fileName;
                        retVal = true;
                        //if (csvFile.Count > 1)
                        //    moreFilesToProcess = true;
                    }
                }
            }
            else
            {
                retVal = false;
            }
            log.LogMethodExit(retVal);
            return retVal;
        }

        private string GetDPLFileFormatCode(StreamReader dplFile)
        {
            log.LogMethodEntry(dplFile);
            string retVal = "";
            if (dplFile != null)
            {
                var fileRow = dplFile.ReadLine();
                //var rowData = fileRow.Split(',');
                var rowData = Regex.Split(fileRow, ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
                if (rowData != null)
                {
                    retVal = rowData[0];
                }
                dplFile.BaseStream.Position = 0;
                dplFile.DiscardBufferedData();
                dplFile.BaseStream.Seek(0, System.IO.SeekOrigin.Begin);
            }
            log.LogMethodExit(retVal);
            return retVal;
        }

        public void DPLProcessFile()
        {
            log.LogMethodEntry();
            utilities.EventLog.logEvent("DPL", 'E', "Begins - DPLProcessFile.", "DPLGenericLog", "DPL", 3);
            try
            {
                VerifyFolderPaths();
            }
            catch (Exception ex)
            {
                log.Error("Error-VerifyFolderPaths. Error Message : " + ex.Message.ToString(), ex);//log fatal
                utilities.EventLog.logEvent("DPL", 'E', "Error-VerifyFolderPaths. Error Message : " + ex.Message.ToString(), "DPLErrorLog", "DPL", 0);
                string bodyText;
                string subjectText;
                string sendEmailTo = utilities.getParafaitDefaults("FAILURE_EMAIL_LIST");
                subjectText = "DPL File Process setup is incomplete";
                bodyText = "Hi, <BR> Please check DPL folder setup. DPL processor is unable to fetch folder details. <BR> Regards,<BR> " + utilities.ParafaitEnv.Username;
                SendMail(sendEmailTo, subjectText, bodyText, "", "");
                log.LogMethodExit();
                utilities.EventLog.logEvent("DPL", 'E', "Ends - DPLProcessFile.", "DPLGenericLog", "DPL", 3);
                throw;
                //return;
            }

            try
            {
                while (FindFileToProcess())
                {
                    StreamReader dplNewFile = new StreamReader(dplFileName);
                    if (dplNewFile != null)
                    {
                        string dplFileFormatCode = GetDPLFileFormatCode(dplNewFile);
                        if (dplFileFormatCode != "")
                        {
                            DPLFactory dplFactoryObject = new DPLFactory(utilities);
                            if (dplFactoryObject.ValidFileFormat(dplFileFormatCode))
                            {
                                DPLFile dplFileObj = dplFactoryObject.GetDPLFileObject(dplFileFormatCode, dplNewFile);
                                int masteSiteId = utilities.ParafaitEnv.SiteId;
                                utilities.ParafaitEnv.SiteId = dplFileObj.dplHeader.poSiteId;
                                SetContextSiteId();
                                dplFileObj.Process();
                                dplNewFile.Close();
                                utilities.ParafaitEnv.SiteId = masteSiteId;
                                SetContextSiteId();
                                ArchiveFile(dplFileName, dplFileObj);
                            }
                            else
                            {
                                dplNewFile.Close();
                                ArchiveFile(dplFileName);
                            }
                        }
                        else
                        {   //Unable to get file format
                            dplNewFile.Close();
                            ArchiveFile(dplFileName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Error-DPLProcessFile. Error Message : " + ex.Message.ToString(), ex);//log fatal
                utilities.EventLog.logEvent("DPL", 'E', "Error-DPLProcessFile. Error Message : " + ex.Message.ToString(), "DPLErrorLog", "DPL", 0);
                throw;
            }
             
            utilities.EventLog.logEvent("DPL", 'E', "Ends - DPLProcessFile.", "DPLGenericLog", "DPL", 3);
            log.LogMethodExit();
        }        

        private void ReportProcessStatus(string fileName, bool successStatus)
        {
            log.LogMethodEntry(fileName, successStatus);
            string bodyText;
            string subjectText;
            string fileNameOnly = Path.GetFileName(fileName);
            string sendEmailTo = "";
            if (successStatus)
            {
                subjectText = "DPL file "+ fileNameOnly + " is successfully processed";
                bodyText = "Hi, <BR> DPL file - "+ fileNameOnly + " is processed successfully. <BR> Regards,<BR> "+utilities.ParafaitEnv.Username;
                sendEmailTo = utilities.getParafaitDefaults("SUCCESS_EMAIL_LIST");
            }
            else
            {
                subjectText = "Unable to process DPL file " + fileNameOnly + " ";
                bodyText = "Hi, <BR> Please check the file format for DPL file - " + fileNameOnly + ". <BR> Regards,<BR> " + utilities.ParafaitEnv.Username;
                sendEmailTo = utilities.getParafaitDefaults("FAILURE_EMAIL_LIST");
            }
            SendMail(sendEmailTo, subjectText, bodyText, "", "");
            log.LogMethodExit();
        }

        private void ReportProcessStatus(DPLFile dplFileObj, string fileName)
        {
            log.LogMethodEntry(dplFileObj, fileName);
            if (dplFileObj != null)
            {
                string bodyText = "";
                string bodyTextEnd;
                string subjectText = "";
                string sendEmailTo = "";
                bool hasContent = false;
                string fileNameOnly = Path.GetFileName(fileName);
                StringBuilder bodySubText = new StringBuilder("<Table border=1 cellpadding=5 ><TR><Th> Section </Th><Th> Invoice/Product </Th><Th> Description </Th></TR>");
                if (!dplFileObj.GetDPLFileStatus())
                {
                    sendEmailTo = utilities.getParafaitDefaults("FAILURE_EMAIL_LIST");
                    subjectText = "Unable to process DPL file " + fileNameOnly + " ";
                    bodyText = "Hi, <BR> Error while processing DPL file - " + fileNameOnly + ". <BR>";

                    if (dplFileObj.dplHeader.hasError)
                    {
                        bodySubText.Append("<TR><TD> Header </ TD ><TD >" + dplFileObj.dplHeader.vendorInvoiceNumber + "</TD ><TD> " + dplFileObj.dplHeader.errorMessage + " </TD ></TR >");
                        hasContent = true;
                    }
                    if (dplFileObj.dplLineList != null)
                    {
                        List<DPLFile.DPLLine> dplLineListWithError = dplFileObj.dplLineList.Where(dplLine => dplLine.hasError == true).ToList();
                        if (dplLineListWithError != null && dplLineListWithError.Count > 0)
                        {
                            foreach (DPLFile.DPLLine dplLineWithError in dplLineListWithError)
                            {
                                bodySubText.Append("<TR><TD> Line </ TD ><TD >" + dplLineWithError.prodDescription + " - " + dplLineWithError.ProductCode + "</TD ><TD> " + dplLineWithError.errorMessage + " </TD ></TR >");
                                hasContent = true;
                            }
                        }
                        List<DPLFile.DPLLine> dplLinesWithNewProducts = dplFileObj.dplLineList.Where(dplLine => string.IsNullOrEmpty(dplLine.otherMessage) == false).ToList();
                        if (dplLinesWithNewProducts != null && dplLinesWithNewProducts.Count > 0)
                        {
                            bodySubText.Append("<TR><TD colspan=\"3\"> </TD></TR >");
                            bodySubText.Append("<TR><TH> </TH><TH> List of Products Created by DPL </ TH ><TH>Description</TH></TR >");
                            foreach (DPLFile.DPLLine dplLineWithNewProduct in dplLinesWithNewProducts)
                            {
                                bodySubText.Append("<TR><TD> Line </ TD ><TD >" + dplLineWithNewProduct.prodDescription + " - " + dplLineWithNewProduct.ProductCode + "</TD ><TD> " + dplLineWithNewProduct.otherMessage + " </TD ></TR >");
                                hasContent = true;
                            }
                        }
                    }
                    bodySubText.Append("</Table>");
                    bodyTextEnd = "<BR> Regards,<BR> " + utilities.ParafaitEnv.Username;
                    if (hasContent)
                        bodyText = bodyText + bodySubText.ToString() + "<BR> Please check the log for more details." + bodyTextEnd;
                    else
                        bodyText = bodyText + "Please check the log for more details." + bodyTextEnd;
                }
                else
                {
                    sendEmailTo = utilities.getParafaitDefaults("SUCCESS_EMAIL_LIST");
                    subjectText = "DPL file " + fileNameOnly + " is successfully processed";
                    bodyText = "Hi, <BR> DPL file - " + fileNameOnly + " is processed successfully. ";
                    if (dplFileObj.dplLineList != null)
                    {
                        List<DPLFile.DPLLine> dplLinesWithNewProducts = dplFileObj.dplLineList.Where(dplLine => string.IsNullOrEmpty(dplLine.otherMessage) == false).ToList();
                        if (dplLinesWithNewProducts != null && dplLinesWithNewProducts.Count > 0)
                        {
                            foreach (DPLFile.DPLLine dplLineWithNewProduct in dplLinesWithNewProducts)
                            {
                                bodySubText.Append("<TR><TD> Line </ TD ><TD >" + dplLineWithNewProduct.prodDescription + " - " + dplLineWithNewProduct.ProductCode + "</TD ><TD> " + dplLineWithNewProduct.otherMessage + " </TD ></TR >");
                                hasContent = true;
                            }
                        }
                    }
                    bodySubText.Append("</Table>");
                    bodyTextEnd = "<BR> Regards,<BR> " + utilities.ParafaitEnv.Username;
                    if (hasContent)
                        bodyText = bodyText + bodySubText.ToString() + bodyTextEnd;
                    else
                        bodyText = bodyText + bodyTextEnd;
                }

                SendMail(sendEmailTo, subjectText, bodyText, "", "");
            }
            log.LogMethodExit();
        }

        private void SendMail(string sendEmailTo, string subjectText, string bodytext, string fileName, string fileFolder)
        {
            log.LogMethodEntry(sendEmailTo, subjectText, bodytext, fileName, fileFolder);
            if (sendEmailTo != "")
            {
                try
                {

                    MessagingRequestDTO messagingRequestDTO = new MessagingRequestDTO(-1, -1, DPL_SUMMARY, EMAILMESSAGINGCHANELTYPE, sendEmailTo, null, null, null, null, null, null, subjectText,
                                                                              bodytext, -1, null, null, true, null, null, -1, false, null, false);

                    MessagingRequestBL messagingRequestBL = new MessagingRequestBL(utilities.ExecutionContext, messagingRequestDTO);
                    messagingRequestBL.Save();
                }
                catch (Exception ex)
                {
                    log.Error(ex);//log fatal
                    utilities.EventLog.logEvent("DPL", 'E', ex.Message, "DPLErrorLog", "DPL", 0);
                }
            }
            else
            {
                log.Error("Missing email ids, please set Inventory Success and Failure email list in Management Studio.");//log fatal
                utilities.EventLog.logEvent("DPL", 'E', "Missing email ids, please set Inventory Success and Failure email list in Management Studio.", "DPLErrorLog", "DPL", 0);
            }

            log.LogMethodExit();
        }

        private void ArchiveFile(string fileName, DPLFile dplFileObj = null)
        {
            log.LogMethodEntry(fileName, dplFileObj);
            string fileNameWithOutPath = Path.GetFileNameWithoutExtension(fileName);           
            string stringTimestamp = System.DateTime.Now.ToString(utilities.ParafaitEnv.DATETIME_FORMAT).Replace("-", "").Replace(" ", "").Replace(":", "");
            string fileExtension = Path.GetExtension(fileName);
            if (String.IsNullOrEmpty(fileExtension))
            {
                fileExtension = ".csv";
            }
            if (dplFileObj == null)
            {
                if (System.IO.Directory.Exists(this.errorFolderPath))
                {
                    System.IO.File.Move(fileName, this.errorFolderPath + @"\" + fileNameWithOutPath + stringTimestamp + fileExtension); 
                }
                ReportProcessStatus(fileName, false);
            }
            else
            {
                if (dplFileObj.GetDPLFileStatus())
                {
                    if (System.IO.Directory.Exists(this.backupFolderPath))
                    {
                        System.IO.File.Move(fileName, this.backupFolderPath + @"\" + fileNameWithOutPath+ stringTimestamp+ fileExtension);
                    } 
                }
                else
                {
                    if (System.IO.Directory.Exists(this.errorFolderPath))
                    {
                        System.IO.File.Move(fileName, this.errorFolderPath + @"\" + fileNameWithOutPath + stringTimestamp + fileExtension);
                    } 
                }
                ReportProcessStatus(dplFileObj, fileName);
            }
            log.LogMethodExit();
        }
    }
}
