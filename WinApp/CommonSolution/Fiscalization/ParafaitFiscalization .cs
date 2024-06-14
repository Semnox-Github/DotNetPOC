/********************************************************************************************
* Project Name - Fiscalization
* Description  - Class for ParafaitFiscalization 
* 
**************
**Version Log
**************
*Version     Date              Modified By        Remarks          
*********************************************************************************************
*2.130.5.0     02-Dec-2021       Dakshakh           Created 
*2.150.5       20-Jun-2023       Guru S A           Modified for reprocessing improvements 
********************************************************************************************/
using System;
using System.Linq;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using Semnox.Parafait.POS;
using Semnox.Core.Utilities;
using Semnox.Parafait.JobUtils;
using Semnox.Parafait.Transaction;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Device.Printer.FiscalPrint;
using Semnox.Parafait.Languages;
using Semnox.Parafait.User;
using System.IO;
using System.Configuration;
using Semnox.Parafait.Discounts;
using System.Security;
using System.Text;

namespace Semnox.Parafait.Fiscalization
{
    public class ParafaitFiscalization
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected ExecutionContext executionContext;
        protected Utilities utilities;
        protected int concurrentRequestId;
        protected const string ERROR_STATUS = "Error";
        protected const string SUCCESS_STATUS = "Success";
        protected const string MOVE_PATH_STRING = "MOVE_POSTED_FILES_PATH";
        protected const string ERROR_PATH_STRING = "ERROR_FILE_PATH";
        protected const string SENT_FILE_PATH = "SENT_FILE_PATH";
        protected const string JSONFORMAT = "Json";
        protected const string XMLFORMAT = "XML";
        protected string programName;
        protected string exSysName = "Fiscalization";
        protected string postFileFormat = "Fiscalization";
        public string SetProgramName { set { programName = value; } }
        public int SetConcurrentRequestId { set { concurrentRequestId = value; } }
        public string GetEXSysName { get { return exSysName; } }
        public string GetPostFileFormat { get { return postFileFormat; } }

        /// <summary>
        /// Parafait Fiscalization
        /// </summary> 
        public ParafaitFiscalization(ExecutionContext executionContext, Utilities utilities)
        {
            log.LogMethodEntry(executionContext, "utilities");
            this.executionContext = executionContext;
            this.utilities = utilities;
            log.LogMethodExit();
        }
        /// <summary>
        /// Create Invoice Files
        /// </summary>
        public virtual List<ExSysSynchLogDTO> CreateInvoice(DateTime currentTime, DateTime lastRunTime)
        {
            log.LogMethodEntry(currentTime, lastRunTime);
            log.LogMethodExit("Please implement CreateInvoice");
            throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Please implement CreateInvoice"));
        }
        /// <summary>
        /// Create Invoice File
        /// </summary>
        public virtual List<ExSysSynchLogDTO> CreateInvoice(int trxId, out string fileName)
        {
            log.LogMethodEntry(trxId);
            log.LogMethodExit("Please implement CreateInvoice");
            throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Please implement CreateInvoice"));
        }
        /// <summary>
        /// Post Invoice Files
        /// </summary>
        public virtual List<ExSysSynchLogDTO> PostInvoice(DateTime currentTime, DateTime lastRunTime)
        {
            log.LogMethodEntry(currentTime, lastRunTime);
            log.LogMethodExit("Please implement PostInvoice");
            throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Please implement PostInvoice"));
        }
        /// <summary>
        /// Post Invoice File
        /// </summary>
        public virtual List<ExSysSynchLogDTO> PostInvoice(string invoicefileNamewithPath)
        {
            log.LogMethodEntry(invoicefileNamewithPath);
            log.LogMethodExit("Please implement PostInvoice");
            throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Please implement PostInvoice"));
        }
        /// <summary>
        /// ReprocessInvoice Invoice Files
        /// </summary>
        public virtual List<ExSysSynchLogDTO> ReprocessInvoice(List<int> trxIdList)
        {
            log.LogMethodEntry(trxIdList);
            log.LogMethodExit("Please implement ReprocessInvoice");
            throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Please implement ReprocessInvoice"));
        }
        /// <summary>
        /// Purge Old Files
        /// </summary>
        public virtual void PurgeOldFiles()
        {
            log.LogMethodEntry();
            try
            {
                int cutOffDays = GetCutOffDateForPurge();
                // Get the current date
                DateTime currentDate = ServerDateTime.Now;
                List<string> folderPathList = GetFolderPath();
                if (folderPathList != null && folderPathList.Any())
                {
                    foreach (string folderPath in folderPathList)
                    {
                        string[] files = Directory.GetFiles(folderPath);
                        foreach (string filePath in files)
                        {
                            try
                            {
                                DateTime fileCreationDate = File.GetCreationTime(filePath);
                                TimeSpan timeDifference = currentDate - fileCreationDate;
                                if (timeDifference.TotalDays > cutOffDays)
                                {
                                    File.Delete(filePath);
                                }
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex);
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

        protected virtual List<string> GetFolderPath()
        {
            log.LogMethodEntry();
            throw new NotImplementedException();
            log.LogMethodExit();
        }

        private int GetCutOffDateForPurge()
        {
            log.LogMethodEntry();
            int cutOffDays = 30;
            try
            {
                string cutOffDaysString = ConfigurationManager.AppSettings["CUT_OFF_DAYS_FOR_FILE_PURGE"];
                log.LogVariableState("cutOffDaysString", cutOffDaysString);
                int.TryParse(cutOffDaysString, out cutOffDays);
            }
            catch (Exception ex)
            {
                log.Error("GetCutOffDateForPurge", ex);
            }
            log.LogMethodExit(cutOffDays);
            return cutOffDays;
        }

        protected List<string> GetAllErrorFolderPath(List<LookupValuesContainerDTO> invoiceSetupLookupValueDTOList)
        {
            log.LogMethodEntry();
            List<string> errFolderPathList = new List<string>();
            List<LookupValuesContainerDTO> lookupValuesDTOList = new List<LookupValuesContainerDTO>(invoiceSetupLookupValueDTOList);
            List<LookupValuesContainerDTO> pathLookupValuesDTOList = null;
            try
            {
                if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
                {
                    pathLookupValuesDTOList = lookupValuesDTOList.Where(lv => string.IsNullOrWhiteSpace(lv.LookupValue) == false
                                                                       && lv.LookupValue.ToUpper().EndsWith(ERROR_PATH_STRING)).ToList();
                    if (pathLookupValuesDTOList != null && pathLookupValuesDTOList.Any())
                    {
                        foreach (LookupValuesContainerDTO item in pathLookupValuesDTOList)
                        {
                            if (string.IsNullOrWhiteSpace(item.Description) == false)
                            {
                                errFolderPathList.Add(item.Description);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Get All Error Folder Path", ex);
            }
            log.LogMethodExit(errFolderPathList);
            return errFolderPathList;
        }

        protected List<string> GetAllSentFolderPath(List<LookupValuesContainerDTO> invoiceSetupLookupValueDTOList)
        {
            log.LogMethodEntry();
            List<string> sentFolderPathList = new List<string>();
            List<LookupValuesContainerDTO> lookupValuesDTOList = new List<LookupValuesContainerDTO>(invoiceSetupLookupValueDTOList);
            List<LookupValuesContainerDTO> pathLookupValuesDTOList = null;
            try
            {
                if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
                {
                    pathLookupValuesDTOList = lookupValuesDTOList.Where(lv => string.IsNullOrWhiteSpace(lv.LookupValue) == false
                                                                       && lv.LookupValue.ToUpper().EndsWith(SENT_FILE_PATH)).ToList();
                    if (pathLookupValuesDTOList != null && pathLookupValuesDTOList.Any())
                    {
                        foreach (LookupValuesContainerDTO item in pathLookupValuesDTOList)
                        {
                            if (string.IsNullOrWhiteSpace(item.Description) == false)
                            {
                                sentFolderPathList.Add(item.Description);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Get All sent Folder Path", ex);
            }
            log.LogMethodExit(sentFolderPathList);
            return sentFolderPathList;
        }

        protected string GetMoveFolderPath(List<LookupValuesContainerDTO> invoiceSetupLookupValueDTOList)
        {
            log.LogMethodEntry();
            List<LookupValuesContainerDTO> lookupValuesDTOList = new List<LookupValuesContainerDTO>(invoiceSetupLookupValueDTOList);
            LookupValuesContainerDTO LookupValuesContainerDTO = null;
            string moveFolderPath = string.Empty;
            try
            {

                if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
                {
                    LookupValuesContainerDTO = lookupValuesDTOList.Where(lv => string.IsNullOrWhiteSpace(lv.LookupValue) == false
                                                                       && lv.LookupValue.ToUpper() == (MOVE_PATH_STRING)).FirstOrDefault();
                    moveFolderPath = LookupValuesContainerDTO.Description;
                    if (string.IsNullOrWhiteSpace(moveFolderPath))
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2933, "Invoice Move Folder Path"));//&1 setup details are missing
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Set Move Folder Path Error", ex);
                throw new Exception(MessageContainerList.GetMessage(executionContext, 4221) + " : " + ex.Message);//Set Move Folder Path Error
            }
            log.LogMethodExit(moveFolderPath);
            return moveFolderPath;
        }

        protected decimal GetDiscountedLineAmount(Transaction.Transaction.TransactionLine trxLine)
        {
            log.LogMethodEntry(trxLine); 
            decimal discountPercentage = 0;
            decimal lineAmount = (decimal)trxLine.LineAmount;
            if (trxLine.TransactionDiscountsDTOList != null && trxLine.TransactionDiscountsDTOList.Any())
            {
                foreach (TransactionDiscountsDTO td in trxLine.TransactionDiscountsDTOList)
                {
                    discountPercentage = discountPercentage + (decimal)td.DiscountPercentage;
                }
                lineAmount = Convert.ToDecimal(trxLine.LineAmount) - ((Convert.ToDecimal(trxLine.LineAmount)) * (discountPercentage) / 100);
            } 
            log.LogMethodExit(lineAmount);
            return lineAmount;
        }

        protected virtual decimal GetDiscountedLinePrice(Transaction.Transaction.TransactionLine trxLine)
        {
            log.LogMethodEntry();
            decimal lineAmount = (decimal)trxLine.Price;
            decimal discountAmount = 0;
            decimal discountPercentage = 0;
            if (trxLine.TransactionDiscountsDTOList != null && trxLine.TransactionDiscountsDTOList.Any())
            {
                foreach (TransactionDiscountsDTO td in trxLine.TransactionDiscountsDTOList)
                {
                    if (td.DiscountId > -1)
                    {
                        DiscountContainerDTO discountDTO = DiscountContainerList.GetDiscountContainerDTOOrDefault(executionContext, td.DiscountId);
                        if (discountDTO != null && discountDTO.DiscountAmount != null)
                        {
                            discountAmount = discountAmount + (decimal)td.DiscountAmount;
                        }
                        else
                        {
                            discountPercentage = discountPercentage + (decimal)td.DiscountPercentage;
                        }
                    }
                }
                decimal finalFlatDiscountAmount = (trxLine.tax_percentage > 0 ? discountAmount / (1 + (decimal)(trxLine.tax_percentage / 100))
                                                                               : discountAmount);
                lineAmount = Convert.ToDecimal(trxLine.Price) - ((Convert.ToDecimal(trxLine.Price)) * (discountPercentage) / 100) - finalFlatDiscountAmount;
            }
            log.LogMethodExit(lineAmount);
            return lineAmount;
        }

        protected decimal GetDiscountedLineTaxAmount(Transaction.Transaction.TransactionLine trxLine)
        {
            log.LogMethodEntry(); 
            decimal discountAmount = 0;
            decimal discountPercentage = 0;
            decimal lineTaxAmount = (trxLine.tax_percentage > 0 ? (decimal)trxLine.Price * (decimal)(trxLine.tax_percentage / 100) : 0);
            if (trxLine.TransactionDiscountsDTOList != null && trxLine.TransactionDiscountsDTOList.Any())
            {
                foreach (TransactionDiscountsDTO td in trxLine.TransactionDiscountsDTOList)
                {
                    if (td.DiscountId > -1)
                    {
                        DiscountContainerDTO discountDTO = DiscountContainerList.GetDiscountContainerDTOOrDefault(executionContext, td.DiscountId);
                        if (discountDTO != null && discountDTO.DiscountAmount != null)
                        {
                            discountAmount = discountAmount + (decimal)td.DiscountAmount;
                        }
                        else
                        {
                            discountPercentage = discountPercentage + (decimal)td.DiscountPercentage;
                        }
                    }
                }
                decimal finalFlatDiscountAmount = (trxLine.tax_percentage > 0 ? (discountAmount - discountAmount / (1 + (decimal)(trxLine.tax_percentage / 100)))
                                                                              : 0);
                lineTaxAmount = lineTaxAmount - ((lineTaxAmount) * (discountPercentage) / 100) - finalFlatDiscountAmount;
            } 
            log.LogMethodExit(lineTaxAmount);
            return lineTaxAmount;
        }
        protected string ConvertToBase64(string input)
        {
            log.LogMethodEntry(input);
            byte[] bytes = Encoding.UTF8.GetBytes(input);
            string base64Value = Convert.ToBase64String(bytes);
            log.LogMethodExit(base64Value);
            return base64Value;
        }
        protected string DoSubstring(string inputString, int requiredLength)
        {
            log.LogMethodEntry(inputString, requiredLength);
            string opString = inputString;
            if (string.IsNullOrWhiteSpace(inputString) == false && requiredLength > 0)
            {
                if (inputString.Length > requiredLength) 
                {
                    opString = inputString.Substring(0, requiredLength);
                }
                
            } 
            log.LogMethodExit(opString);
            return opString;
        }
        protected int ConvertToInt(decimal inputValue)
        {
            log.LogMethodEntry(inputValue);
            int opIntValue = Convert.ToInt32(inputValue);
            log.LogMethodExit(opIntValue);
            return opIntValue;
        }
        protected int ConvertToInt(double inputValue)
        {
            log.LogMethodEntry(inputValue);
            int opIntValue = Convert.ToInt32(inputValue);
            log.LogMethodExit(opIntValue);
            return opIntValue;
        }
    }
}
