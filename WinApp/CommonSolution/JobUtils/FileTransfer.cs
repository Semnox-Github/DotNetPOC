/********************************************************************************************
 * Project Name - JobUtils
 * Description  - File Tranfer
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        13-Aug-2019      Deeksha        Added logger methods.
 ********************************************************************************************/
using ICSharpCode.SharpZipLib.Zip;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace Semnox.Parafait.JobUtils
{
    /// <summary>
    /// Class for handling File transfer activities
    /// </summary>
    public class FileTransfer
    {
        Semnox.Parafait.logging.Logger log;
        Utilities Utilities;
        SendEmailUI SendEmailUI;//Used to send emails
        Initialize init;
        /// <summary>
        /// enum class is defined with types of file templates are there
        /// </summary>
        public enum FileTemplates
        {
            /// <summary>
            /// Orderline file template has order related transaction details
            /// </summary>
            OrderLine,
            /// <summary>
            /// Template to send members transaction details 
            /// </summary>
            Members,
            /// <summary>
            /// Template to send Product and Category transaction details
            /// </summary>
            ProductCategory,
            /// <summary>
            /// Template to send visiting details
            /// </summary>
            VisitingDetails,
            /// <summary>
            /// Template to send Coupon codes details
            /// </summary>
            CouponCode,
            /// <summary>
            /// Template to send Inventory details
            /// </summary>
            InventoryDetails
        }

        //public enum FileTemplates
        //{

        //    /// <summary>
        //    /// Template to send Product and Category transaction details
        //    /// </summary>
        //    ProductCategory,
        //}

        /// <summary>
        /// Classs constructorsetLanguage
        /// </summary>
        /// <param name="_Utilities">parafait utilities</param>
        public FileTransfer(Utilities _Utilities)
        {
            log.LogMethodEntry(_Utilities);
            try
            {
                Utilities = _Utilities;
                log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                //Initializing the public variables
                init = new Initialize(Utilities);
            }
            catch(Exception ex)
            { Utilities.EventLog.logEvent("ParafaitDataTransfer", 'E', "Constructor called FileTransfer()", "Error while initializing variables : "+ex.Message, "MerkleFileTransfer", 1, "", "", Utilities.ParafaitEnv.LoginID, Utilities.ParafaitEnv.POSMachine, null); }
            log.LogMethodExit();
        }
        /// <summary>
        /// Start method
        /// </summary>
        public void StartProcess(string scheduledHour, int refreshFrequency)
        {
            log.LogMethodEntry(scheduledHour, refreshFrequency);
  
            //Merkle File Transfer File Transfer 
            if (init != null && init.IsMerkleFileTransferEnabled)
            {
                Utilities.EventLog.logEvent("ParafaitDataTransfer", 'I', "StartProcess Called ", "", "MerkleFileTransfer", 1, "", "", Utilities.ParafaitEnv.LoginID, Utilities.ParafaitEnv.POSMachine, null);
                try
                {
                    log.Debug("Start Merkle File Transfer Process called...");
                    bool status = StartFileTransfer(scheduledHour, refreshFrequency);

                    if (!status)
                        log.Debug("Failed Merkle File transfer...");

                }
                catch (Exception ex)
                {
                    Utilities.EventLog.logEvent("ParafaitDataTransfer", 'E', "Error in StartFileTransfer " + ex.ToString(), "", "MerkleFileTransfer", 1, "", "", Utilities.ParafaitEnv.LoginID, Utilities.ParafaitEnv.POSMachine, null);
                }                

                log.Debug("Start File clean operation...");               
                try
                {
                    FileBackupOperation();
                }
                catch (Exception ex)
                {
                    Utilities.EventLog.logEvent("ParafaitDataTransfer", 'E', "Error in file backup " + ex.ToString(), "", "MerkleFileTransfer", 1, "", "", Utilities.ParafaitEnv.LoginID, Utilities.ParafaitEnv.POSMachine, null);
                }

                log.Debug("End File clean operation...");

                log.Debug("End Merkle File Transfer Process called...");
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Method will be called by service when on start, this is the start where file upload process begin
        /// </summary>
        /// <returns>Return true on successfull upload, false on failure</returns>
        public bool StartFileTransfer(string runAt, int frequency)
        {
            log.LogMethodEntry(runAt, frequency);
            CreateTextFile createTextFile = new CreateTextFile(Utilities);
            GPGFileEncryption gpgEncryption = new GPGFileEncryption(Utilities, init.EncryptionKey, init.GPGFileLocation);
            UploadFiles uploadFiles = new UploadFiles(Utilities);

            bool FileTransderStatus = false;
            string fileStorePath = string.Empty;
            if (!string.IsNullOrEmpty(init.FileStorePath))
                fileStorePath = init.FileStorePath;
                        
            if (!string.IsNullOrEmpty(fileStorePath))
            {
                var myEnumMemberCount = Enum.GetNames(typeof(FileTemplates)).Length;
                int fileCount = 0;
                foreach (FileTemplates enumValue in Enum.GetValues(typeof(FileTemplates)))
                {           
                    IQuery dataQuery = null;
                    bool isEncryptSuccess = false;
                    int creationStatus = 0;
                    string message = string.Empty;
                    fileCount++;
                    //Get from date
                    object successfullRunTime = GetLastRunDateTime(enumValue);
                    Utilities.EventLog.logEvent("ParafaitDataTransfer", 'I', "successfullRunTime: "+ successfullRunTime, "", "MerkleFileTransfer", 1, "", "", Utilities.ParafaitEnv.LoginID, Utilities.ParafaitEnv.POSMachine, null);
                  
                    if (successfullRunTime != null)
                    {
                        TimeSpan diff = DateTime.Now - Convert.ToDateTime(successfullRunTime);
                        double hours = diff.TotalHours;
                        DateTime fromDate = Convert.ToDateTime(successfullRunTime);

                        DateTime toDate = DateTime.Now;
                        if (enumValue == FileTemplates.ProductCategory)
                        {
                            fromDate = Convert.ToDateTime("2016-06-17 06:00:00");
                        }
                        string currentTime = DateTime.Now.Hour.ToString().PadLeft(2, '0') + ":" + DateTime.Now.Minute.ToString().PadLeft(2, '0');// +":" + DateTime.Now.Second;
                        Utilities.EventLog.logEvent("ParafaitDataTransfer", 'I', "enumValue: " + enumValue + " huorsr:" + hours + " currentTime:" + currentTime + " runAt:" + runAt, "", "MerkleFileTransfer", 1, "", "", Utilities.ParafaitEnv.LoginID, Utilities.ParafaitEnv.POSMachine, null);
                        if (hours > frequency && Convert.ToDateTime(currentTime) >= Convert.ToDateTime(runAt))
                        {
                            Utilities.EventLog.logEvent("ParafaitDataTransfer", 'I', "Running time reached " + "enumValue: " + enumValue + " huorsr:" + hours + " currentTime:" + currentTime + " runAt:" + runAt, "", "MerkleFileTransfer", 1, "", "", Utilities.ParafaitEnv.LoginID, Utilities.ParafaitEnv.POSMachine, null);
                            #region Creating Text Files
                            try
                            {
                                log.Debug("Preparing data query for  Template : " + enumValue + ", FromDate:" + fromDate + ", ToDate: " + toDate);
                                dataQuery = GetDataQuery(enumValue, Convert.ToDateTime(fromDate), toDate);
                                Utilities.EventLog.logEvent("ParafaitDataTransfer", 'I', "Preparing data query for  Template : " + enumValue + ", FromDate:" + fromDate + ", ToDate: " + toDate, "", "MerkleFileTransfer", 1, "", "", Utilities.ParafaitEnv.LoginID, Utilities.ParafaitEnv.POSMachine, null);
                                log.Debug("Start Creating Text files...");
                                creationStatus = createTextFile.CreateFile(dataQuery, ref fileStorePath);
                                log.Debug("End Creating Text files...");
                            }
                            catch (Exception ex)
                            {
                                Utilities.EventLog.logEvent("ParafaitDataTransfer", 'E', "Error while creating file " + ex.ToString(), "", "MerkleFileTransfer", 1, "", "", Utilities.ParafaitEnv.LoginID, Utilities.ParafaitEnv.POSMachine, null);
                                message = "Error while Creating Text File: " + ex.Message;
                                log.Error(message);
                            }
                            #endregion

                            #region Encrypt Files Using GpG encryption
                            try
                            {
                                if (creationStatus == 1)
                                {
                                    log.Debug("Start File Encryption...");

                                    isEncryptSuccess = gpgEncryption.EncryptFile(fileStorePath);
                                    log.Debug("End File Encryption...");
                                }
                            }
                            catch (Exception ex)
                            {
                                Utilities.EventLog.logEvent("ParafaitDataTransfer", 'E', "Error while Encrypting file " + ex.ToString() + " creationStatus:" + creationStatus, "", "MerkleFileTransfer", 1, "", "", Utilities.ParafaitEnv.LoginID, Utilities.ParafaitEnv.POSMachine, null);
                                //EventLog
                                message = "Error while Encrypting Text File: " + ex.Message;
                                log.Error(message);
                            }
                            #endregion

                            #region Upload Files
                            try
                            {
                                if ((creationStatus == 1) && isEncryptSuccess)
                                {
                                    log.Debug("Start Uploading file");
                                    string inputFileName = System.IO.Path.GetFileNameWithoutExtension(fileStorePath);
                                    var files = Directory.GetFiles(init.FileStorePath).Where(name => name.Contains(inputFileName));
                                    foreach (string file in files)
                                    {
                                        if (!file.EndsWith(".txt"))
                                            isEncryptSuccess = uploadFiles.SFTPFileTransfer(file);
                                    }
                                    log.Debug("End Uploading file");
                                }
                            }
                            catch (Exception ex)
                            {
                                Utilities.EventLog.logEvent("ParafaitDataTransfer", 'E', "Error while uploading file " + ex.ToString(), "", "MerkleFileTransfer", 1, "", "", Utilities.ParafaitEnv.LoginID, Utilities.ParafaitEnv.POSMachine, null);
                                isEncryptSuccess = false;
                                message = "Error while Uploading files : " + ex.Message;
                                log.Error(message);
                            }
                            #endregion

                            #region Exsynch Log Update
                            try
                            {
                                if ((creationStatus == 0) && !isEncryptSuccess)
                                {
                                    UpdateExSysSynchLog(enumValue, fileCount, false, "Failed", DateTime.Now, fileStorePath, message);
                                    SendStatusEmail(enumValue.ToString(), init.RecipientEmail, ref message);
                                }
                                else if ((creationStatus == 1) && !isEncryptSuccess)
                                {
                                    UpdateExSysSynchLog(enumValue, fileCount, false, "Failed", DateTime.Now, fileStorePath, message);
                                    SendStatusEmail(enumValue.ToString(), init.RecipientEmail, ref message);
                                }
                                else if ((creationStatus == 1) && isEncryptSuccess)
                                {
                                    FileTransderStatus = true;
                                    UpdateExSysSynchLog(enumValue, fileCount, true, "Success", DateTime.Now, fileStorePath, message);
                                }
                                if ((creationStatus == -1) && !isEncryptSuccess)
                                {
                                    Utilities.EventLog.logEvent("ParafaitDataTransfer", 'I', "End StartFileTransfer() No Record for the FromDate:" + fromDate + ", ToDate: " + toDate, "",
                                        "MerkleFileTransfer", 1, "", "", Utilities.ParafaitEnv.LoginID, Utilities.ParafaitEnv.POSMachine, null);
                                }
                            }
                            catch (Exception ex)
                            {
                                Utilities.EventLog.logEvent("ParafaitDataTransfer", 'E', "Error in UpdateExSysSynchLog , creationStatus:  " + creationStatus + " isEncryptSuccess: " + isEncryptSuccess + " enumValue :" + enumValue + " " + ex.ToString(), "", "MerkleFileTransfer", 1, "", "", Utilities.ParafaitEnv.LoginID, Utilities.ParafaitEnv.POSMachine, null);
                            }
                            #endregion
                        }
                    }
                    fileStorePath = init.FileStorePath;
                }
            }
            //Utilities.EventLog.logEvent("ParafaitDataTransfer", 'D', "End StartFileTransfer()", "", "MerkleFileTransfer", 1, "", "", Utilities.ParafaitEnv.LoginID, Utilities.ParafaitEnv.POSMachine, null);
            log.LogMethodExit(FileTransderStatus);
            return FileTransderStatus;
        }

        /// <summary>
        /// Method to upload monthly inventory data
        /// </summary>
        /// <param name="runDate">next run date</param>
        /// <param name="templateType">Type of template uploading</param>
        /// <param name="createTextFile">new object defined to create text files</param>
        /// <param name="gpgEncryption">Object for encrypting data</param>
        /// <param name="uploadFiles">object to upload files</param>
        /// <param name="fileStorePath">path to store temp files</param>
        /// <param name="fileCount">file index</param>
        public void UploadMonthlyData(DateTime runDate, FileTemplates templateType, CreateTextFile createTextFile, GPGFileEncryption gpgEncryption, UploadFiles uploadFiles, string fileStorePath, int fileCount)
        {
            log.LogMethodEntry(runDate, templateType, createTextFile, gpgEncryption, uploadFiles, fileStorePath, fileCount);
            IQuery dataQuery = null;
            int creationStatus = 0;
            string message = string.Empty;
            bool isEncryptSuccess = false;
            try
            {
                dataQuery = GetDataQuery(templateType, runDate, DateTime.MinValue);
                if(createTextFile.CreateFile(dataQuery, ref fileStorePath) == 1)
                {
                    isEncryptSuccess = gpgEncryption.EncryptFile(fileStorePath);

                    if(isEncryptSuccess)
                    {
                        string inputFileName = System.IO.Path.GetFileNameWithoutExtension(fileStorePath);
                        var files = Directory.GetFiles(init.FileStorePath).Where(name => name.Contains(inputFileName));
                        foreach (string file in files)
                        {
                            if (!file.EndsWith(".txt"))
                                isEncryptSuccess = uploadFiles.SFTPFileTransfer(file);
                        }

                        if ((creationStatus == 1) && isEncryptSuccess)
                        {
                            UpdateExSysSynchLog(templateType, fileCount, true, "Success", DateTime.Now, fileStorePath, message);
                        }
                        else
                        {
                            UpdateExSysSynchLog(templateType, fileCount, true, "Failed", DateTime.Now, fileStorePath, message);
                            SendStatusEmail(templateType.ToString(), init.RecipientEmail, ref message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.EventLog.logEvent("ParafaitDataTransfer", 'E', "Error while creating file " + ex.ToString() + " RunDate: "+ runDate, "", "MerkleFileTransfer", 1, "", "", Utilities.ParafaitEnv.LoginID, Utilities.ParafaitEnv.POSMachine, null);
                message = "Error while Creating Text File: " + ex.Message;
                log.Error(message);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Method to clean temp folder, move all files from temp folder to history folder 
        /// </summary>
        public void FileBackupOperation()
        {
            log.LogMethodEntry();
            if (!string.IsNullOrEmpty(init.HistoryFolderPath))
            {
                String backupFilesFolderName = init.HistoryFolderPath;
                DirectoryInfo dirInfo = new DirectoryInfo(backupFilesFolderName);
                if (dirInfo.Exists == false)
                    Directory.CreateDirectory(backupFilesFolderName);

                string[] filenames = Directory.GetFiles(init.FileStorePath);

                if (filenames.Length > 0)
                {
                    try
                    {
                        // Zip up the files - From SharpZipLib Demo Code
                        using (ZipOutputStream stream = new ZipOutputStream(File.Create(backupFilesFolderName + "Backup_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".zip")))
                        {
                            stream.SetLevel(9); // 0-9, 9 being the highest compression

                            byte[] buffer = new byte[4096];

                            foreach (string file in filenames)
                            {

                                ZipEntry entry = new
                                ZipEntry(Path.GetFileName(file));

                                entry.DateTime = DateTime.Now;
                                stream.PutNextEntry(entry);

                                using (FileStream fs = File.OpenRead(file))
                                {
                                    int sourceBytes;
                                    do
                                    {
                                        sourceBytes = fs.Read(buffer, 0,
                                        buffer.Length);

                                        stream.Write(buffer, 0, sourceBytes);

                                    } while (sourceBytes > 0);
                                }
                            }
                            stream.Finish();
                            stream.Close();
                        }
                    }
                    catch(Exception ex)
                    {
                        Utilities.EventLog.logEvent("ParafaitDataTransfer", 'E', "Start FileBackupOperation() Error while files cleaning :"+ex.Message, "", "MerkleFileTransfer", 1, "", "", Utilities.ParafaitEnv.LoginID, Utilities.ParafaitEnv.POSMachine, null);
                    }

                    //delete files after backup
                    Array.ForEach(filenames,delegate (string path) { File.Delete(path); });
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Method to initialize the Query execution
        /// </summary>
        /// <param name="templates">to which template query to execute</param>
        /// <param name="frmDate">select data from  this date</param>
        /// <param name="todate">select data till this date</param>
        /// <returns></returns>
        public IQuery GetDataQuery(FileTemplates templates, DateTime frmDate, DateTime todate)
        {
            log.LogMethodEntry(templates, frmDate, todate);
            string query = string.Empty;
            IQuery qryResult = null;

            switch (templates)
            {
                case FileTemplates.OrderLine:
                    qryResult = new OrderLineQuery(Utilities, frmDate, todate);
                    break;
                case FileTemplates.Members:
                    qryResult = new MembersQuery(Utilities, frmDate, todate);
                    break;
                case FileTemplates.ProductCategory:
                    qryResult = new ProductCategoryQuery(Utilities, frmDate, todate);
                    break;
                case FileTemplates.VisitingDetails:
                    qryResult = new VisitingDetailsQuery(Utilities, frmDate, todate);
                    break;
                case FileTemplates.CouponCode:
                    qryResult = new CouponCodeQuery(Utilities, frmDate, todate);
                    break;
                case FileTemplates.InventoryDetails:
                    qryResult = new InventoryQuery(Utilities);
                    break;
            }
            log.LogMethodExit(qryResult);
            return qryResult;
        }

        /// <summary>
        /// Method to get Template last successfull run date and time
        /// </summary>
        /// <param name="template">type of template</param>
        /// <returns>Returns successfull run date and time</returns>
        public object GetLastRunDateTime(FileTemplates template)
        {
            log.LogMethodEntry(template);
            object dtTimStamp = Utilities.executeScalar(@"select max(timestamp) from ExSysSynchLog es where es.ExSysName = 'ParafaitDataTransfer' and es.IsSuccessFul = 1 
                                                                 and es.ParafaitObject = @ObjectName",
                                     new SqlParameter("@ObjectName", Convert.ToString(template)));

            //object dtTimStamp = "2016-06-17 06:00:00";
            ////Modified to send full data for product category template
            //if (template == FileTemplates.ProductCategory)
            //{
            //    dtTimStamp = "2016-06-17 06:00:00";
            //}
            if (DBNull.Value.Equals(dtTimStamp))
            {
                dtTimStamp = Convert.ToDateTime("2016-06-17 06:00:00");
            }
            log.LogMethodExit(dtTimStamp);
            return dtTimStamp;
        }

        /// <summary>
        /// Method to Insert File Transfer status to ExSynchLog Table
        /// </summary>
        /// <param name="ParafaitObject">Process name</param>
        /// <param name="ParafaitObjectId">type of template</param>
        /// <param name="success">1 on success, 0 on Failure</param>
        /// <param name="status">Success, Failure</param>
        /// <param name="last_updated_date">current datetime</param>
        /// <param name="data">Validation data</param>
        /// <param name="remarks">Warning message</param>
        void UpdateExSysSynchLog(object ParafaitObject, object ParafaitObjectId, bool success, object status, object last_updated_date, object data, object remarks)
        {
            log.LogMethodEntry(ParafaitObject, ParafaitObjectId, success, status, last_updated_date, data, remarks);
            try
            {
                Utilities.executeNonQuery(@"insert into ExSysSynchLog
                                                    (TimeStamp, ExSysName,
                                                    ParafaitObject, ParafaitObjectId,
                                                    ParafaitObjectGuid, IsSuccessFul,
                                                    Status, Data,
                                                    Remarks)
                                               values 
                                                    (getdate(), 'ParafaitDataTransfer',
                                                    @ParafaitObject, @ParafaitObjectId,
                                                    @guid, @success,
                                                    @status, @data,
                                                    substring(@remarks, 1, 500))",
                                             new SqlParameter("@ParafaitObject", Convert.ToString(ParafaitObject)),
                                             new SqlParameter("@ParafaitObjectId", ParafaitObjectId),
                                             new SqlParameter("@guid", DBNull.Value),//reportScheduleGuid
                                             new SqlParameter("@success", success),
                                             new SqlParameter("@status", status),
                                             new SqlParameter("@data", data),
                                             new SqlParameter("@remarks", remarks));
            }
            catch (Exception ex)
            {
                Utilities.EventLog.logEvent("ParafaitDataTransfer", 'E', "Error while Inserting into ExsynchLog, object Name: "+ ParafaitObjectId, ex.ToString(), "MerkleFileTransfer", 1, "", "", Utilities.ParafaitEnv.LoginID, Utilities.ParafaitEnv.POSMachine, null);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// to send File transfer status to support
        /// </summary>
        /// <param name="templateType">template type failed</param>
        /// <param name="emailRecipientList">to email id</param>
        /// <param name="message">error message to find</param>
        public void SendStatusEmail(string templateType, string emailRecipientList, ref string message)
        {
            log.LogMethodEntry(templateType, emailRecipientList, message);
            try
            {
                log.Debug("Start Sending failed status email to " + emailRecipientList);
                SendEmailUI = new SendEmailUI(emailRecipientList, 
                        "", "", 
                        "Merkle File Transfer Failure - " + DateTime.Now.ToString("dd-MM-yyyy h:mm tt"), "Hi, <br/><br/> Error occured while File Transfer, file template type : " + templateType + " and error details is as follow.<br/>"+ message +"<br/>" + " please check.", "", "", true, Utilities);
            }
            catch (Exception ex)
            {
                Utilities.EventLog.logEvent("ParafaitDataTransfer", 'E', "Error: Sending Summary email: " + ex.Message, "", "MerkleFileTransfer", 1, "", "", Utilities.ParafaitEnv.LoginID, Utilities.ParafaitEnv.POSMachine, null);
            }
            log.LogMethodExit();
        }
    }
}
