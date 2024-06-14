/********************************************************************************************
* Project Name - EcuadorFiscalization
* Description  - Class for EcuadorFiscalization 
* 
**************
**Version Log
**************
*Version     Date              Modified By        Remarks          
*********************************************************************************************
*2.130.2.0     07-Jan-2022       Dakshakh           Created 
*2.155.0       28-May-2023       Guru S A           Modified for reprocessing improvements    
********************************************************************************************/
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Reflection;
using System.Globalization;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;
using Semnox.Parafait.JobUtils;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Transaction;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Communication;
using System.Net;
using System.Configuration;
using Semnox.Parafait.Customer;
using Newtonsoft.Json;
using Semnox.Parafait.Discounts;

namespace Semnox.Parafait.Fiscalization
{
    public class EcuadorFiscalization : ParafaitFiscalization
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        bool isReversal = false;
        string ivoiceLineElement = "\"detalles\"" + ": [";
        string invoiceResultLine = string.Empty;

        private string posApiToken = string.Empty;
        private string invoiceType = string.Empty;
        private string invoiceTypeCode = string.Empty;
        private string docIssueDate = string.Empty;
        private string docType = string.Empty;
        private string authorizationNumber = string.Empty;
        private string docNumber = string.Empty;
        private string facturaDocType = string.Empty;
        private string creditNoteDocType = string.Empty;
        private string timeFormat = string.Empty;


        private string accountingCPName = string.Empty;
        private string accountingCPContactNumber = string.Empty;
        private string accountingCPAddressLine = string.Empty;
        private string accountingCPMailId = string.Empty;
        private string accountingCPCedula = string.Empty;
        private string accountingCPRUC = string.Empty;
        private string accountingCPIDNumber = string.Empty;
        private string accountingCPType = string.Empty;
        private string accountingCPIsForeigner = string.Empty;
        private string accountingCPDNI = string.Empty;

        private string accountingSPRUC = string.Empty;
        private string accountingSPName = string.Empty;
        private string accountingSPIdentificationCode = string.Empty;
        private string accountingSPEmailID = string.Empty;
        private string accountingSPAddress = string.Empty;
        private string accountingSPContactNumber = string.Empty;
        private string accountingSPIsForeigner = string.Empty;
        private string accountingSPType = string.Empty;

        private string facturaHeaderTemplate = string.Empty;
        private string creditNoteHeaderTemplate = string.Empty;

        private string facturaCustomerLineTemplate = string.Empty;
        private string creditNoteCustomerLineTemplate = string.Empty;

        private string facturaVendorTemplate = string.Empty;
        private string creditNoteVendorTemplate = string.Empty;

        private string facturaChargesTemplate = string.Empty;
        private string creditNoteChargesTemplate = string.Empty;

        private string facturaDetailsTemplate = string.Empty;
        private string creditNoteDetailsTemplate = string.Empty;

        private string invoiceProductTemplate = string.Empty;

        private string dateFormat = string.Empty;
        private string amountFormat = string.Empty;
        private string integerFormat = string.Empty;
        private int amountPrecision = 2;
        private string cultureInfo = string.Empty;
        private bool useCulture = false;
        private System.Globalization.CultureInfo invC = null;

        private string boleta_File_Upload_Path = string.Empty;
        private string factura_File_Upload_Path = string.Empty;
        private string creditNote_File_Upload_Path = string.Empty;
        private string fileUploadPath = string.Empty;
        private string recordType = string.Empty;
        string externalSystemReference = string.Empty;

        private const string FACTURA = "Factura";

        private const string FACTURA_HEADER = "FACTURA_HEADER";
        private const string CREDITNOTE_HEADER = "CREDITNOTE_HEADER";

        private const string FACTURA_CUSTOMER_LINE = "INVOICE_CUSTOMER_LINE";
        private const string CREDITNOTE_CUSTOMER_LINE = "CREDITNOTE_CUSTOMER_LINE";

        private const string FACTURA_VENDOR_LINE = "INVOICE_VENDOR_LINE";
        private const string CREDITNOTE_VENDOR_LINE = "CREDITNOTE_VENDOR_LINE";

        private const string FACTURA_DETAILS_LINE = "INVOICE_DETAILS_LINE";
        private const string CREDITNOTE_DETAILS_LINE = "CREDITNOTE_DETAILS_LINE";

        private const string INVOICE_PRODUCT_LINE = "INVOICE_PRODUCT_LINE";

        private const string FISCAL_INVOICE_SETUP = "FISCAL_INVOICE_SETUP";
        private const string FISCAL_INVOICE_ATTRIBUTES = "FISCAL_INVOICE_ATTRIBUTES";
        private const string OUT_PATH_PARTIAL_STRING = "File_Upload_Path";
        private const string POST_FILE_EXTENSION = ".json";

        private const string USERNAMELOOKUPVALUENAME = "TAX_IDENTIFICATION_NUMBER";
        private const string POSTURLFORINVOICELOOKUPVALUENAME = "Post_URL_For_Invoice";
        private const string INVOICEAUTHKEYLOOKUPVALUENAME = "Invoice_Authentication_Key";
        private const string INVOICEAPITOKEN = "INVOICE_API_TOKEN";

        private const string EX_SYS_NAME = "EcuadorFiscalization";
        private const string TRANSACTION = "Transaction";
        private const string POST_INVOICE = "PostInvoice";

        private string postURLForInvoice;
        private string authKey;
        private string moveFolderPath;
        private List<LookupValuesContainerDTO> invoiceSetupLookupValueDTOList;
        private List<LookupValuesContainerDTO> invoiceAttributesValuesDTOList;

        /// <summary>
        /// Ecuador Fiscalization
        /// </summary>
        /// <param name="executionContext"></param>
        public EcuadorFiscalization(ExecutionContext executionContext, Utilities utilities)
            : base(executionContext, utilities)
        {
            log.LogMethodEntry(executionContext, "utilities");
            this.exSysName = EX_SYS_NAME;
            this.postFileFormat = JSONFORMAT;
            SetLookUpValueList();
            moveFolderPath = GetMoveFolderPath(invoiceSetupLookupValueDTOList);
            BuildInvoiceAttributes();
            authKey = GetAuthenticationKey();
            posApiToken = GetPosAPIToken();
            postURLForInvoice = GetPostURLForInvoice();
            log.LogMethodExit();
        }

        /// <summary>
        ///Create Invoice file
        /// </summary>
        public override List<ExSysSynchLogDTO> CreateInvoice(DateTime currentTime, DateTime LastRunTime)
        {
            log.LogMethodEntry(currentTime, LastRunTime);
            List<ExSysSynchLogDTO> logDTOList = new List<ExSysSynchLogDTO>();
            try
            {
                string msg = MessageContainerList.GetMessage(executionContext, "Post invoice process takes care of creation and post tasks for Ecuador. No need to run Create Invoice process seperately");
                ExSysSynchLogDTO exSysSynchLogDTO = new ExSysSynchLogDTO(logId: -1, timestamp: ServerDateTime.Now, exSysName: EX_SYS_NAME,
                              parafaitObject: TRANSACTION, parafaitObjectId: -1, parafaitObjectGuid: string.Empty,
                             isSuccessFul: true, status: SUCCESS_STATUS, data: msg, remarks: this.programName, concurrentRequestId: this.concurrentRequestId);
                ExSysSynchLogBL exSysSynchLogBL = new ExSysSynchLogBL(executionContext, exSysSynchLogDTO);
                exSysSynchLogBL.Save();
                logDTOList.Add(exSysSynchLogBL.ExSysSynchLogDTO);
            }
            catch (Exception ex)
            {
                log.Error(MessageContainerList.GetMessage(executionContext, 4206), ex); //Error in Process InvoiceJson Records
                string msg = "Unexpected error in CreateInvoice: ";
                ExSysSynchLogDTO logDTO = LogError(ex, msg, TRANSACTION);
                if (logDTO != null)
                {
                    logDTOList.Add(logDTO);
                }
            }
            log.LogMethodExit(logDTOList);
            return logDTOList;
        }

        /// <summary>
        ///Create Invoice file
        /// </summary> 
        /// <returns></returns>
        public override List<ExSysSynchLogDTO> CreateInvoice(int trxId, out string fileName)
        {
            log.LogMethodEntry(trxId);
            List<ExSysSynchLogDTO> logDTOList = new List<ExSysSynchLogDTO>();
            string trxGuid = string.Empty;
            fileName = string.Empty;
            try
            {
                TransactionUtils transactionUtils = new TransactionUtils(utilities);
                Transaction.Transaction transaction = transactionUtils.CreateTransactionFromDB(trxId, utilities);
                trxGuid = transaction.TrxGuid;
                string invoiceType = GetInvoiceType(transaction);
                List<ValidationError> validationErrorList = ValidateTransactionData(transaction, invoiceType);
                if (validationErrorList != null && validationErrorList.Any())
                {
                    ValidationException vEx = new ValidationException("Transaction Validation Failed", validationErrorList);
                    log.Error("Validation failed for " + transaction.Trx_id, vEx);
                    throw vEx;
                }
                if (transaction.OriginalTrxId > 0)
                {
                    isReversal = true;
                }
                else
                {
                    isReversal = false;
                }
                SetInvoiceTypeAttributes(invoiceType, isReversal);
                BuildProfileDTO(transaction);
                
                invoiceResultLine = BuildHeaderSection(transaction, invoiceType, isReversal);
                invoiceResultLine += BuildInvoiceCustomerLineSection(transaction, invoiceType, isReversal);
                invoiceResultLine += BuildInvoiceVendorLineSection(transaction, invoiceType, isReversal);
                invoiceResultLine += BuildInvoiceDetailsSection(transaction, invoiceType, isReversal);
                invoiceResultLine += ivoiceLineElement;

                int loopCount = 0;
                externalSystemReference = string.Empty;
                foreach (Transaction.Transaction.TransactionLine trxLine in transaction.TransactionLineList)
                {
                    loopCount++;
                    externalSystemReference = GetexternalSystemReference(trxLine);
                    invoiceResultLine = invoiceResultLine + BuildInvoiceProductSection(trxLine);
                    if (transaction.TransactionLineList.Count > loopCount && transaction.TransactionLineList.Count != 1)
                    {
                        invoiceResultLine = invoiceResultLine + ",";
                    }
                }
                invoiceResultLine += "]}";

                fileName = fileUploadPath + invoiceTypeCode + "-" + transaction.Trx_No + POST_FILE_EXTENSION;
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }
                System.IO.File.WriteAllText(fileName, invoiceResultLine);
                log.LogVariableState("Json save completed", fileName);
                ExSysSynchLogDTO exSysSynchLogDTO = new ExSysSynchLogDTO(logId: -1, timestamp: ServerDateTime.Now, exSysName: EX_SYS_NAME,
                               parafaitObject: TRANSACTION, parafaitObjectId: transaction.Trx_id, parafaitObjectGuid: transaction.TrxGuid,
                              isSuccessFul: true, status: SUCCESS_STATUS, data: "File created: " + fileName, remarks: this.programName, concurrentRequestId: this.concurrentRequestId);
                ExSysSynchLogBL exSysSynchLogBL = new ExSysSynchLogBL(executionContext, exSysSynchLogDTO);
                exSysSynchLogBL.Save();
                logDTOList.Add(exSysSynchLogBL.ExSysSynchLogDTO);
            }
            catch (Exception ex)
            {
                log.Error("Error in CreateInvoice", ex);
                string msg = "Unexpected error in CreateInvoice: ";
                ExSysSynchLogDTO logDTO = LogError(ex, msg, TRANSACTION, trxId, trxGuid);
                if (logDTO != null)
                {
                    logDTOList.Add(logDTO);
                }
            }
            log.LogMethodExit(logDTOList);
            return logDTOList;
        }

        /// <summary>
        /// Recreate Invoice File
        /// </summary>  
        /// <returns></returns>
        public override List<ExSysSynchLogDTO> ReprocessInvoice(List<int> trxIdList)
        {
            log.LogMethodEntry(trxIdList);
            List<ExSysSynchLogDTO> logDTOList = new List<ExSysSynchLogDTO>();
            try
            {
                SetInvoiceTemplates();
                List<TransactionDTO> transactionDTOList = null;
                transactionDTOList = GetEligibleInvoiceRecords(trxIdList);
                if (transactionDTOList != null && transactionDTOList.Any())
                {
                    foreach (TransactionDTO transactionDTO in transactionDTOList)
                    {
                        try
                        {
                            string fileName = string.Empty;
                            List<ExSysSynchLogDTO> trxLogList = CreateInvoice(transactionDTO.TransactionId, out fileName);
                            if (trxLogList != null && trxLogList.Any())
                            {
                                logDTOList.AddRange(trxLogList);
                            }
                            List<ExSysSynchLogDTO> posttrxLogList = PostInvoice(fileName);
                            if (posttrxLogList != null && posttrxLogList.Any())
                            {
                                logDTOList.AddRange(posttrxLogList);
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Error(MessageContainerList.GetMessage(executionContext, 4206), ex); //Error in Process InvoiceJson Records 
                            string msg = "Unexpected error in ReCreateInvoice: ";
                            ExSysSynchLogDTO logDTO = LogError(ex, msg, TRANSACTION, transactionDTO.TransactionId, transactionDTO.Guid);
                            if (logDTO != null)
                            {
                                logDTOList.Add(logDTO);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(MessageContainerList.GetMessage(executionContext, 4206), ex); //Error in Process InvoiceJson Records

                string msg = "Unexpected error in ReCreateInvoice: ";
                ExSysSynchLogDTO logDTO = LogError(ex, msg, TRANSACTION);
                if (logDTO != null)
                {
                    logDTOList.Add(logDTO);
                }
            }
            log.LogMethodExit(logDTOList);
            return logDTOList;
        }

        //<summary>
        //Post Invoice Files
        //</summary>
        public override List<ExSysSynchLogDTO> PostInvoice(DateTime currentTime, DateTime lastRunTime)
        {
            log.LogMethodEntry(currentTime, lastRunTime);
            List<ExSysSynchLogDTO> logDTOList = new List<ExSysSynchLogDTO>();
            try
            {
                List<TransactionDTO> transactionDTOList = null;
                SetInvoiceTemplates();
                transactionDTOList = GetEligibleInvoiceRecords(currentTime, lastRunTime);
                if (transactionDTOList != null && transactionDTOList.Any())
                {
                    foreach (TransactionDTO transactionDTO in transactionDTOList)
                    {
                        try
                        {
                            string fileName = string.Empty;
                            List<ExSysSynchLogDTO> trxLogList = CreateInvoice(transactionDTO.TransactionId, out fileName);
                            if (trxLogList != null && trxLogList.Any())
                            {
                                logDTOList.AddRange(trxLogList);
                            }
                            List<ExSysSynchLogDTO> postTrxLogList = PostInvoice(fileName);
                            if (postTrxLogList != null && postTrxLogList.Any())
                            {
                                logDTOList.AddRange(postTrxLogList);
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Error(MessageContainerList.GetMessage(executionContext, 4206), ex); //Error in Process InvoiceJson Records 
                            string msg = "Unexpected error in PostInvoice: ";
                            ExSysSynchLogDTO logDTO = LogError(ex, msg, TRANSACTION, transactionDTO.TransactionId, transactionDTO.Guid);
                            if (logDTO != null)
                            {
                                logDTOList.Add(logDTO);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                string msg = "Error in PostInvoice: ";
                ExSysSynchLogDTO logDTO = LogError(ex, msg, POST_INVOICE);
                if (logDTO != null)
                {
                    logDTOList.Add(logDTO);
                }
            }
            try
            {
                PurgeOldFiles();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                string msg = "Error in PurgeOldFiles: ";
                ExSysSynchLogDTO logDTO = LogError(ex, msg, POST_INVOICE);
                if (logDTO != null)
                {
                    logDTOList.Add(logDTO);
                }
            }
            log.LogMethodExit(logDTOList);
            return logDTOList;
        }
        private void BuildInvoiceAttributes()
        {
            log.LogMethodEntry();
            try
            {
                //RUCOfIssuer
                accountingSPRUC = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "TAX_IDENTIFICATION_NUMBER");
                //Factura
                LookupValuesContainerDTO lookupValuesDocTypeDTO = invoiceSetupLookupValueDTOList.FirstOrDefault(p => p.LookupValue == "DocumentType");
                if (lookupValuesDocTypeDTO != null)
                {
                    string[] values = lookupValuesDocTypeDTO.Description.Split('|');
                    facturaDocType = values[0];
                    creditNoteDocType = values[1];
                }
                log.LogVariableState("facturaDocType", facturaDocType);
                log.LogVariableState("creditNoteDocType", creditNoteDocType);
                LookupValuesContainerDTO lookupValuesAuthNumDTO = invoiceSetupLookupValueDTOList.FirstOrDefault(p => p.LookupValue == "Authorization_Number");
                if (lookupValuesAuthNumDTO != null)
                {
                    authorizationNumber = lookupValuesAuthNumDTO.Description;
                }
                log.LogVariableState("authorizationNumber", authorizationNumber);
                //Factura
                LookupValuesContainerDTO lookupValuesRecordTypeDTO = invoiceSetupLookupValueDTOList.FirstOrDefault(p => p.LookupValue == "RecordType");
                if (lookupValuesRecordTypeDTO != null)
                {
                    recordType = lookupValuesRecordTypeDTO.Description;
                }
                log.LogVariableState("recordType", recordType);
                //Date Format
                LookupValuesContainerDTO lookupValuesDateFormatDTO = invoiceSetupLookupValueDTOList.FirstOrDefault(p => p.LookupValue == "InvoiceDate_Format");
                if (lookupValuesDateFormatDTO != null)
                {
                    dateFormat = lookupValuesDateFormatDTO.Description;
                    if (string.IsNullOrWhiteSpace(dateFormat))
                    {
                        dateFormat = "dd/MM/yyyy";
                    }
                }
                log.LogVariableState("dateFormat", dateFormat);
                //Time Format
                LookupValuesContainerDTO lookupValuesTimeFormatDTO = invoiceSetupLookupValueDTOList.FirstOrDefault(p => p.LookupValue == "InvoiceTime_Format");
                if (lookupValuesTimeFormatDTO != null)
                {
                    timeFormat = lookupValuesTimeFormatDTO.Description;
                    if (string.IsNullOrWhiteSpace(timeFormat))
                    {
                        timeFormat = "HH:mm:ss";
                    }
                }
                log.LogVariableState("timeFormat", timeFormat);
                //Integer Format
                LookupValuesContainerDTO lookupValuesIntegerFormatDTO = invoiceSetupLookupValueDTOList.FirstOrDefault(p => p.LookupValue == "InvoiceInteger_Format");
                if (lookupValuesIntegerFormatDTO != null)
                {
                    integerFormat = lookupValuesIntegerFormatDTO.Description;
                    if (string.IsNullOrWhiteSpace(integerFormat))
                    {
                        integerFormat = "###0";
                    }
                }
                log.LogVariableState("integerFormat", integerFormat);
                //Amount Format
                LookupValuesContainerDTO lookupValuesamountFormatDTO = invoiceSetupLookupValueDTOList.FirstOrDefault(p => p.LookupValue == "InvoiceAmount_Format");
                if (lookupValuesamountFormatDTO != null)
                {
                    amountFormat = lookupValuesamountFormatDTO.Description;
                    if (string.IsNullOrWhiteSpace(amountFormat))
                    {
                        amountFormat = "####.00";
                    }
                }
                log.LogVariableState("amountFormat", amountFormat);
                //Amount Precision
                LookupValuesContainerDTO lookupValuesamountPrecisionDTO = invoiceSetupLookupValueDTOList.FirstOrDefault(p => p.LookupValue == "InvoiceAmountPrecision");
                if (lookupValuesamountPrecisionDTO != null)
                {
                    string amountPrecisionVal = lookupValuesamountPrecisionDTO.Description;
                    if (string.IsNullOrWhiteSpace(amountPrecisionVal))
                    {
                        amountPrecision = 2;
                    }
                    if (Int32.TryParse(amountPrecisionVal, out amountPrecision) == false)
                    {
                        amountPrecision = 2;
                    }
                }
                log.LogVariableState("amountPrecision", amountPrecision);
                //CultureInfo for amount
                LookupValuesContainerDTO lookupValuesCultureInfoDTO = invoiceSetupLookupValueDTOList.FirstOrDefault(p => p.LookupValue == "CultureInfo");
                if (lookupValuesCultureInfoDTO != null)
                {
                    cultureInfo = lookupValuesCultureInfoDTO.Description;
                    if (string.IsNullOrWhiteSpace(cultureInfo) == false)
                    {
                        useCulture = true;
                    }
                }
                log.LogVariableState("cultureInfo", cultureInfo);
                log.LogVariableState("useCulture", useCulture);
                invC = (useCulture ? new System.Globalization.CultureInfo(cultureInfo) : CultureInfo.InvariantCulture);
                //Boleta_File_Upload_Path
                LookupValuesContainerDTO lookupValuesBoletaFileUploadPathDTO = invoiceSetupLookupValueDTOList.FirstOrDefault(p => p.LookupValue == "Boleta_File_Upload_Path");
                if (lookupValuesBoletaFileUploadPathDTO != null)
                {
                    boleta_File_Upload_Path = lookupValuesBoletaFileUploadPathDTO.Description;
                }
                log.LogVariableState("boleta_File_Upload_Path", boleta_File_Upload_Path);
                //Factura_File_Upload_Path
                LookupValuesContainerDTO lookupValuesFacturaFileUploadPathDTO = invoiceSetupLookupValueDTOList.FirstOrDefault(p => p.LookupValue == "Factura_File_Upload_Path");
                if (lookupValuesFacturaFileUploadPathDTO != null)
                {
                    factura_File_Upload_Path = lookupValuesFacturaFileUploadPathDTO.Description;
                }
                log.LogVariableState("factura_File_Upload_Path", factura_File_Upload_Path);
                //CreditNote_File_Upload_Path
                LookupValuesContainerDTO lookupValuesCreditNoteFileUploadPathDTO = invoiceSetupLookupValueDTOList.FirstOrDefault(p => p.LookupValue == "CreditNote_File_Upload_Path");
                if (lookupValuesCreditNoteFileUploadPathDTO != null)
                {
                    creditNote_File_Upload_Path = lookupValuesCreditNoteFileUploadPathDTO.Description;
                }
                log.LogVariableState("creditNote_File_Upload_Path", creditNote_File_Upload_Path);
                //AccountingSPIdentificationCode
                LookupValuesContainerDTO lookupValuesaccountingSPIdDTO = invoiceAttributesValuesDTOList.FirstOrDefault(p => p.LookupValue == "AccountingSPIdentificationCode");
                if (lookupValuesaccountingSPIdDTO != null)
                {
                    accountingSPIdentificationCode = lookupValuesaccountingSPIdDTO.Description;
                }
                log.LogVariableState("accountingSPIdentificationCode", accountingSPIdentificationCode);
                //AccountingSPName
                LookupValuesContainerDTO lookupValuesAccountingSPNameDTO = invoiceAttributesValuesDTOList.FirstOrDefault(p => p.LookupValue == "AccountingSPName");
                if (lookupValuesAccountingSPNameDTO != null)
                {
                    accountingSPName = lookupValuesAccountingSPNameDTO.Description;
                }
                log.LogVariableState("accountingSPName", accountingSPName);
                //accountingSPContactNumber
                LookupValuesContainerDTO lookupValuesAccountingSPContactNumberDTO = invoiceAttributesValuesDTOList.FirstOrDefault(p => p.LookupValue == "accountingSPContactNumber");
                if (lookupValuesAccountingSPContactNumberDTO != null)
                {
                    accountingSPContactNumber = lookupValuesAccountingSPContactNumberDTO.Description;
                }
                log.LogVariableState("accountingSPContactNumber", accountingSPContactNumber);
                //accountingSPType
                LookupValuesContainerDTO lookupValuesAccountingSPTypeDTO = invoiceAttributesValuesDTOList.FirstOrDefault(p => p.LookupValue == "accountingSPType");
                if (lookupValuesAccountingSPTypeDTO != null)
                {
                    accountingSPType = lookupValuesAccountingSPTypeDTO.Description;
                }
                log.LogVariableState("accountingSPType", accountingSPType);
                //accountingSPType
                LookupValuesContainerDTO lookupValuesAccountingSPEmailIDDTO = invoiceAttributesValuesDTOList.FirstOrDefault(p => p.LookupValue == "accountingSPEmailID");
                if (lookupValuesAccountingSPEmailIDDTO != null)
                {
                    accountingSPEmailID = lookupValuesAccountingSPEmailIDDTO.Description;
                }
                log.LogVariableState("accountingSPEmailID", accountingSPEmailID);
            }
            catch (Exception ex)
            {
                log.Error("Error in Build Invoice Attributes", ex);
                throw new Exception(MessageContainerList.GetMessage(executionContext, 4218) + " : " + ex.Message);//Error in Build Invoice Attributes
            }
            log.LogMethodExit();
        }
        private void SetInvoiceTemplates()
        {
            log.LogMethodEntry();
            try
            {
                facturaHeaderTemplate = LoadTemplateData(FACTURA_HEADER);
                creditNoteHeaderTemplate = LoadTemplateData(CREDITNOTE_HEADER);

                facturaCustomerLineTemplate = LoadTemplateData(FACTURA_CUSTOMER_LINE);
                creditNoteCustomerLineTemplate = LoadTemplateData(CREDITNOTE_CUSTOMER_LINE);

                facturaVendorTemplate = LoadTemplateData(FACTURA_VENDOR_LINE);
                creditNoteVendorTemplate = LoadTemplateData(CREDITNOTE_VENDOR_LINE);

                facturaDetailsTemplate = LoadTemplateData(FACTURA_DETAILS_LINE);
                creditNoteDetailsTemplate = LoadTemplateData(CREDITNOTE_DETAILS_LINE);

                invoiceProductTemplate = LoadTemplateData(INVOICE_PRODUCT_LINE);
            }
            catch (Exception ex)
            {
                log.Error("Set Invoice Templates Error", ex);
                throw new Exception(MessageContainerList.GetMessage(executionContext, 4219) + " : " + ex.Message); //Set Invoice Templates Error
            }
            log.LogMethodExit();
        }
        private string LoadTemplateData(string templateSection)
        {
            log.LogMethodEntry(templateSection);
            string template = string.Empty;
            try
            {
                string strExeFilePath = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                strExeFilePath += "\\Resources\\";

                if (Directory.Exists(strExeFilePath))
                {
                    switch (templateSection)
                    {
                        case "FACTURA_HEADER":
                            {
                                template = File.ReadAllText(strExeFilePath + "FacturaHeader.txt");
                                break;
                            }
                        case "CREDITNOTE_HEADER":
                            {
                                template = File.ReadAllText(strExeFilePath + "CNHeader.txt");
                                break;
                            }
                        case "INVOICE_CUSTOMER_LINE":
                            {
                                template = File.ReadAllText(strExeFilePath + "FacturaClient.txt");
                                break;
                            }
                        case "CREDITNOTE_CUSTOMER_LINE":
                            {
                                template = File.ReadAllText(strExeFilePath + "CNClient.txt");
                                break;
                            }
                        case "INVOICE_VENDOR_LINE":
                            {
                                template = File.ReadAllText(strExeFilePath + "FacturaVendor.txt");
                                break;
                            }
                        case "CREDITNOTE_VENDOR_LINE":
                            {
                                template = File.ReadAllText(strExeFilePath + "CNVendor.txt");
                                break;
                            }
                        case "INVOICE_DETAILS_LINE":
                            {
                                template = File.ReadAllText(strExeFilePath + "FacturaDetails.txt");
                                break;
                            }
                        case "CREDITNOTE_DETAILS_LINE":
                            {
                                template = File.ReadAllText(strExeFilePath + "CNDetails.txt");
                                break;
                            }
                        case "INVOICE_PRODUCT_LINE":
                            {
                                template = File.ReadAllText(strExeFilePath + "InvoiceProductLineDetails.txt");
                                break;
                            }
                    }
                }
                else
                {
                    string msg1 = MessageContainerList.GetMessage(executionContext, "Unable to locate folder &1", strExeFilePath);
                    ValidationException validationException = new ValidationException(msg1);
                    throw validationException;
                }
            }
            catch (Exception ex)
            {
                log.Error(MessageContainerList.GetMessage(executionContext, 4220), ex);//Load Template Data Error
                throw;
            }
            log.LogMethodExit(template);
            return template;
        }

        private List<ValidationError> ValidateTransactionData(Transaction.Transaction trx, string invoiceType)
        {
            log.LogMethodEntry("trx", invoiceType);
            List<ValidationError> errorList = new List<ValidationError>();

            bool reversalTrx = (trx.OriginalTrxId > 0 ? true : false);
            if (string.IsNullOrWhiteSpace(invoiceType) && reversalTrx == false)
            {
                string msg = MessageContainerList.GetMessage(executionContext, "Invoice type is not set for the transaction. Cannot proceed.");
                ValidationError validationError = new ValidationError("Invoice", null, msg);
                errorList.Add(validationError);
            }
            log.LogVariableState("trx.customerDTO.Id", (trx.customerDTO != null ? trx.customerDTO.Id : -2));
            log.LogVariableState("trx.customerDTO.ProfileDTO.Id", (trx.customerDTO != null && trx.customerDTO.ProfileDTO != null ? trx.customerDTO.ProfileDTO.Id : -3));
            if ((trx.customerDTO != null && trx.customerDTO.Id > -1 && trx.customerDTO.ProfileDTO != null && trx.customerDTO.ProfileDTO.Id > -1)
                == false)
            {
                log.Error("Error in Build ProfileDTO. CUstomer DTO not found or not saved yet.");
                ValidationError validationError = new ValidationError("Customer", null, MessageContainerList.GetMessage(executionContext, 1934));
                //Customer details are not passed
                errorList.Add(validationError);
            }

            foreach (Transaction.Transaction.TransactionLine trxLine in trx.TransactionLineList)
            {
                try
                {
                    externalSystemReference = GetexternalSystemReference(trxLine);
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                    if (ex.InnerException != null)
                    {
                        msg = msg + ": " + ex.InnerException.Message;
                    }
                    ValidationError validationError = new ValidationError("Product", null, MessageContainerList.GetMessage(executionContext, msg));
                    errorList.Add(validationError);
                }
            }

            log.LogMethodExit(errorList);
            return errorList;
        }

        private List<TransactionDTO> GetEligibleInvoiceRecords(DateTime currentTime, DateTime lastRunTime)
        {
            log.LogMethodEntry(currentTime, lastRunTime);
            List<TransactionDTO> transactionDTOList = new List<TransactionDTO>();
            try
            {
                TransactionListBL transactionListBL = new TransactionListBL(executionContext);
                List<KeyValuePair<TransactionDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<TransactionDTO.SearchByParameters, string>>();
                searchParams.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                searchParams.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.LAST_UPDATE_FROM_TIME, lastRunTime.ToString("yyyy-MM-dd HH:mm:ss")));
                searchParams.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.LAST_UPDATE_TO_TIME, currentTime.ToString("yyyy-MM-dd HH:mm:ss")));
                searchParams.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.STATUS, Transaction.Transaction.TrxStatus.CLOSED.ToString()));
                transactionDTOList = transactionListBL.GetTransactionDTOList(searchParams, utilities, null, 0, 5000);
                if (transactionDTOList != null && transactionDTOList.Any())
                {
                    List<TransactionDTO> reversedTransactionDTOList = new List<TransactionDTO>();
                    List<TransactionDTO> originalTransactionDTOList = new List<TransactionDTO>();

                    reversedTransactionDTOList = transactionDTOList.FindAll(pc => pc.OriginalTransactionId > -1);
                    transactionDTOList = transactionDTOList.FindAll(pc => string.IsNullOrWhiteSpace(pc.ExternalSystemReference) == true && pc.OriginalTransactionId <= -1);

                    if (reversedTransactionDTOList != null && reversedTransactionDTOList.Any())
                    {
                        StringBuilder idListStringBuilder = new StringBuilder("");
                        string idList;
                        for (int i = 0; i < reversedTransactionDTOList.Count; i++)
                        {
                            if (i != 0)
                            {
                                idListStringBuilder.Append(",");
                            }

                            idListStringBuilder.Append(reversedTransactionDTOList[i].OriginalTransactionId.ToString());
                        }
                        idList = idListStringBuilder.ToString();
                        List<KeyValuePair<TransactionDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<TransactionDTO.SearchByParameters, string>>();
                        searchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        searchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.TRANSACTION_ID_LIST, idList.ToString()));
                        originalTransactionDTOList = transactionListBL.GetTransactionDTOList(searchParameters, null, null, 0, 5000);

                        if (originalTransactionDTOList != null && originalTransactionDTOList.Any())
                        {
                            foreach (TransactionDTO originalTransactionDTO in originalTransactionDTOList)
                            {
                                TransactionDTO transactionDTO = reversedTransactionDTOList.FirstOrDefault(rt => rt.OriginalTransactionId == originalTransactionDTO.TransactionId);
                                if (transactionDTO != null)
                                {
                                    //external ref not set. process it
                                    if (string.IsNullOrWhiteSpace(transactionDTO.ExternalSystemReference))
                                    {
                                        transactionDTOList.Add(transactionDTO);
                                    }
                                    else if (!string.IsNullOrWhiteSpace(originalTransactionDTO.ExternalSystemReference))
                                    {
                                        if (transactionDTO.ExternalSystemReference.Equals(originalTransactionDTO.ExternalSystemReference))
                                        {
                                            transactionDTOList.Add(transactionDTO);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(MessageContainerList.GetMessage(executionContext, 4207), ex);//Error in Get Eligible Invoice Records
                throw;
            }
            log.LogMethodExit(transactionDTOList);
            return transactionDTOList;
        }

        private void SetInvoiceTypeAttributes(string invoicType, bool isReversal)
        {
            log.LogMethodEntry(invoicType, isReversal);
            if (string.IsNullOrWhiteSpace(invoicType) && isReversal == false)
            {
                string msg = MessageContainerList.GetMessage(executionContext, "Invoice type is not set for the transaction. Cannot proceed.");
                throw new ValidationException(msg);
            }
            if (invoicType == "Factura")
            {
                invoiceTypeCode = "FAC";
                fileUploadPath = factura_File_Upload_Path;
            }
            else
            {
                invoiceTypeCode = "NCT";
                fileUploadPath = creditNote_File_Upload_Path;
            }
            log.LogMethodExit();
        }

        private string GetInvoiceType(Transaction.Transaction trx)
        {
            log.LogMethodEntry();
            string invoicType = trx.GetInvoiceType();
            log.LogMethodExit(invoicType);
            return invoicType;
        }

        private void BuildProfileDTO(Transaction.Transaction trx)
        {
            log.LogMethodEntry(trx.Trx_id);
            try
            {
                log.LogVariableState("BuildProfileDTO trx.customerDTO.Id", (trx.customerDTO != null ? trx.customerDTO.Id : -2));
                log.LogVariableState("BuildProfileDTO trx.customerDTO.ProfileDTO.Id", (trx.customerDTO != null && trx.customerDTO.ProfileDTO != null ? trx.customerDTO.ProfileDTO.Id : -3));
                if (trx.customerDTO != null && trx.customerDTO.Id > -1 && trx.customerDTO.ProfileDTO != null && trx.customerDTO.ProfileDTO.Id > -1)
                {
                    accountingCPName = (string.IsNullOrWhiteSpace(trx.customerDTO.ProfileDTO.FirstName) ? string.Empty : trx.customerDTO.ProfileDTO.FirstName) +
                                       (string.IsNullOrWhiteSpace(trx.customerDTO.ProfileDTO.MiddleName) ? string.Empty : " " + trx.customerDTO.ProfileDTO.MiddleName) +
                                       (string.IsNullOrWhiteSpace(trx.customerDTO.ProfileDTO.LastName) ? string.Empty : " " + trx.customerDTO.ProfileDTO.LastName);
                    if (trx.customerDTO.ProfileDTO.AddressDTOList != null && trx.customerDTO.ProfileDTO.AddressDTOList.Any())
                    {
                        accountingCPContactNumber = trx.customerDTO.PhoneNumber;
                        accountingCPAddressLine = trx.customerDTO.ProfileDTO.AddressDTOList[0].Line1;
                        accountingCPMailId = trx.customerDTO.Email;
                        accountingCPCedula = trx.customerDTO.Id.ToString();
                        accountingCPRUC = trx.customerDTO.UniqueIdentifier;
                        accountingCPType = "N";
                        accountingCPIsForeigner = "false";
                        accountingCPDNI = trx.customerDTO.TaxCode;
                    }
                }
                else
                {
                    log.Error("Error in Build ProfileDTO. CUstomer DTO not found or not saved yet.");
                    throw new Exception(MessageContainerList.GetMessage(executionContext, 4208));//Error in Build ProfileDTO
                }
            }
            catch (Exception ex)
            {
                log.Error("Error for Build ProfileDTO", ex);
                throw new Exception(MessageContainerList.GetMessage(executionContext, 4208) + " : " + ex.Message);
            }
            log.LogMethodExit();
        }

        private string BuildHeaderSection(Transaction.Transaction trx, string invoiceType, bool isReversal)
        {
            log.LogMethodEntry("trx", invoiceType, isReversal);
            string headerSection = string.Empty;
            try
            {
                if (isReversal)
                {
                    headerSection = BuildCreditNoteHeadervalues(trx);
                }
                else if (invoiceType == "Factura")
                {
                    headerSection = BuildInvoiceHeadervalues(trx, facturaHeaderTemplate);
                }
                else
                {
                    string msg = MessageContainerList.GetMessage(executionContext, "Invoice type is not set for the transaction. Cannot proceed.");
                    throw new ValidationException(msg);
                }
            }
            catch (Exception ex)
            {
                log.Error("Error in Build Header Section", ex);
                throw new Exception(MessageContainerList.GetMessage(executionContext, 4209) + " : " + ex.Message);//Error in Build Header Section
            }
            log.LogMethodExit(headerSection);
            return headerSection;
        }

        private string BuildInvoiceHeadervalues(Transaction.Transaction transaction, string headerSection)
        {
            log.LogMethodEntry("transaction", headerSection);
            try
            {
                headerSection = headerSection.Replace("@ApiTokenOfPOS", "\"" + posApiToken + "\"");
                headerSection = headerSection.Replace("@DocIssueDate", "\"" + (transaction.TrxDate).ToString(dateFormat, invC) + "\"");
                headerSection = headerSection.Replace("@DocType", "\"" + facturaDocType + "\"");
                headerSection = headerSection.Replace("@DocNumber", "\"" + transaction.TransactionDTO.TransactionNumber.ToString() + "\"");
                headerSection = headerSection.Replace("@Authorization", "");//Should be empty for electronics
            }
            catch (Exception ex)
            {
                log.Error("Error in Build Invoice Header values", ex);
                throw new Exception(MessageContainerList.GetMessage(executionContext, 4210) + " : " + ex.Message);//Error in Build Invoice Header values
            }
            log.LogMethodExit(headerSection);
            return headerSection;
        }

        private string BuildCreditNoteHeadervalues(Transaction.Transaction transaction)
        {
            log.LogMethodEntry("transaction");
            string headerSection = creditNoteHeaderTemplate;
            try
            {
                log.LogVariableState("transaction.TransactionDTO.ExternalSystemReference", transaction.TransactionDTO.ExternalSystemReference.ToString());
                headerSection = headerSection.Replace("@ApiTokenOfPOS", "\"" + posApiToken + "\"");
                headerSection = headerSection.Replace("@DocIssueDate", "\"" + (transaction.TrxDate).ToString(dateFormat, invC) + "\"");
                headerSection = headerSection.Replace("@DocType", "\"" + creditNoteDocType + "\"");
                if (string.IsNullOrWhiteSpace(transaction.TransactionDTO.ExternalSystemReference))
                {
                    string externalSystemReferenceVal = string.Empty;
                    TransactionDTO orginalTrxDto = GetOrginalTrxDTO(transaction.TransactionDTO);
                    if (orginalTrxDto != null)
                    {
                        externalSystemReferenceVal = orginalTrxDto.ExternalSystemReference.ToString();
                    }
                    log.LogVariableState(externalSystemReferenceVal, externalSystemReferenceVal);
                    headerSection = headerSection.Replace("@DocRelationID", "\"" + externalSystemReferenceVal + "\"");
                }
                else
                {
                    headerSection = headerSection.Replace("@DocRelationID", "\"" + transaction.TransactionDTO.ExternalSystemReference.ToString() + "\"");
                }
                //Indicates the document to which the credit note applies.
                headerSection = headerSection.Replace("@RecordType", "\"" + recordType + "\"");
                //Indicates the type of document record (CLI: client, PRO: provider).
                headerSection = headerSection.Replace("@DocNumber", "\"" + transaction.TransactionDTO.TransactionNumber.ToString() + "\"");
                headerSection = headerSection.Replace("@Authorization", "\"" + authorizationNumber + "\"");
            }
            catch (Exception ex)
            {
                log.Error("Error in Build Credit Note Header values", ex);
                throw new Exception(MessageContainerList.GetMessage(executionContext, 4211) + " : " + ex.Message);//Error in Build Credit Note Header values
            }
            log.LogMethodExit(headerSection);
            return headerSection;
        }

        private string BuildInvoiceCustomerLineSection(Transaction.Transaction transaction, string invoiceType, bool isReversal)
        {
            log.LogMethodEntry("transaction", invoiceType, isReversal);
            string invoiceCustomerLineSection = string.Empty;
            try
            {
                invoiceCustomerLineSection = (isReversal == false ? facturaCustomerLineTemplate : creditNoteCustomerLineTemplate);
                invoiceCustomerLineSection = invoiceCustomerLineSection.Replace("@ClientRUC", "\"" + accountingCPRUC + "\"");
                invoiceCustomerLineSection = invoiceCustomerLineSection.Replace("@ClientIDNumber", "\"" + accountingCPDNI + "\"");
                invoiceCustomerLineSection = invoiceCustomerLineSection.Replace("@ClientName", "\"" + accountingCPName + "\"");
                invoiceCustomerLineSection = invoiceCustomerLineSection.Replace("@ClientContactNumber", "\"" + accountingCPContactNumber + "\"");
                invoiceCustomerLineSection = invoiceCustomerLineSection.Replace("@ClientAddress", "\"" + accountingCPAddressLine + "\"");
                invoiceCustomerLineSection = invoiceCustomerLineSection.Replace("@ClientType", "\"" + accountingCPType + "\"");
                invoiceCustomerLineSection = invoiceCustomerLineSection.Replace("@ClientEmail", "\"" + accountingCPMailId + "\"");
                invoiceCustomerLineSection = invoiceCustomerLineSection.Replace("@ClientIsForeigner", accountingCPIsForeigner);
            }
            catch (Exception ex)
            {
                log.Error("Error in Build Invoice Customer Line Section", ex);
                throw new Exception(MessageContainerList.GetMessage(executionContext, 4213) + " : " + ex.Message);//Error in Build Invoice Customer Line Section
            }
            log.LogMethodExit();
            return invoiceCustomerLineSection;
        }

        private string BuildInvoiceDetailsSection(Transaction.Transaction transaction, string invoiceType, bool isReversal)
        {
            log.LogMethodEntry();
            string invoiceDetailsSection = string.Empty;
            try
            {
                decimal preTaxAmount = Convert.ToDecimal(transaction.Pre_TaxAmount); 
                decimal taxAmount = Convert.ToDecimal(transaction.Tax_Amount); 
                decimal totalAmount = Convert.ToDecimal(transaction.Transaction_Amount);
                decimal trxTotalAmount = totalAmount; ;
                if (transaction.Discount_Amount > 0)
                {
                    decimal discountedPreTaxAmount = GetDiscountedTotalPreTaxAmount(transaction);
                    decimal discountedtaxAmount = GetDiscountedTotalTaxAmount(transaction);
                    decimal discountedtotalAmount = GetDiscountedTotalAmount(transaction);
                    trxTotalAmount = discountedtotalAmount;
                    preTaxAmount = discountedPreTaxAmount;
                    taxAmount = discountedtaxAmount;
                }
                if (trxTotalAmount <= 0 && isReversal == false) // to handle 100% discount.
                {
                    preTaxAmount = 0;
                    taxAmount = 0;
                } 
                preTaxAmount = TruncateDecimal(preTaxAmount);
                taxAmount = TruncateDecimal(taxAmount);
                trxTotalAmount = TruncateDecimal(trxTotalAmount);

                invoiceDetailsSection = (isReversal == false ? facturaDetailsTemplate : creditNoteDetailsTemplate);
                string docType = (isReversal == false ? facturaDocType : creditNoteDocType);
                invoiceDetailsSection = invoiceDetailsSection.Replace("@DocType", "\"" + docType + "\"");
                invoiceDetailsSection = invoiceDetailsSection.Replace("@SubAmount", "0.00");
                invoiceDetailsSection = invoiceDetailsSection.Replace("@SubTotal", Math.Abs(preTaxAmount).ToString(amountFormat, invC));
                invoiceDetailsSection = invoiceDetailsSection.Replace("@TaxAmount", Math.Abs(taxAmount).ToString(amountFormat, invC));
                //invoiceDetailsSection = invoiceDetailsSection.Replace("@ServiceCharge", serviceCharge);
                invoiceDetailsSection = invoiceDetailsSection.Replace("@TotalAmount", Math.Abs(trxTotalAmount).ToString(amountFormat, invC));
                invoiceDetailsSection = invoiceDetailsSection.Replace("@TransactionId", "\"" + transaction.TrxGuid.ToString() + "\"");
            }
            catch (Exception ex)
            {
                log.Error("Error in Build Invoice Customer Line Section", ex);
                throw new Exception(MessageContainerList.GetMessage(executionContext, 4214) + " : " + ex.Message);//Error in Build Invoice Customer Line Section
            }
            log.LogMethodExit(invoiceDetailsSection);
            return invoiceDetailsSection;
        }

        private string BuildInvoiceProductSection(Transaction.Transaction.TransactionLine trxLine)
        {
            log.LogMethodEntry();
            string invoiceProductSection = invoiceProductTemplate;
            try
            {
                string zeroBase = "0.00";
                string taxBase = "0.00";
                string noTax = "0.00";
                string taxPercentage = string.Empty;
                decimal discountFactor = (trxLine.Discount_Percentage == 0 ? 0 : (decimal)(trxLine.Discount_Percentage / 100));
                if (trxLine.tax_percentage == 0 && trxLine.tax_id < 0)
                {
                    decimal noTaxAmount = (decimal)trxLine.Price - discountFactor * (decimal)trxLine.Price;
                    noTaxAmount = TruncateDecimal(noTaxAmount);
                    noTax = noTaxAmount.ToString(amountFormat, invC);
                    taxPercentage = "\"" + string.Empty + "\"";
                }
                else if (trxLine.tax_percentage == 0 && trxLine.tax_id >= 0)
                {
                    decimal zeroBaseAmount = (decimal)trxLine.Price - discountFactor * (decimal)trxLine.Price;
                    zeroBaseAmount = TruncateDecimal(zeroBaseAmount);
                    zeroBase = zeroBaseAmount.ToString(amountFormat, invC);
                    taxPercentage = trxLine.tax_percentage.ToString(amountFormat, invC);
                }
                else
                {
                    decimal taxBaseAmount = (decimal)trxLine.Price - discountFactor * (decimal)trxLine.Price;
                    taxBaseAmount = TruncateDecimal(taxBaseAmount);
                    taxBase = taxBaseAmount.ToString(amountFormat, invC);
                    taxPercentage = trxLine.tax_percentage.ToString(amountFormat, invC);
                }
                invoiceProductSection = invoiceProductSection.Replace("@ProductId", "\"" + externalSystemReference + "\"");
                invoiceProductSection = invoiceProductSection.Replace("@Quantity", Math.Abs(trxLine.quantity).ToString(integerFormat, invC));
                invoiceProductSection = invoiceProductSection.Replace("@Price", trxLine.Price.ToString(amountFormat, invC));
                invoiceProductSection = invoiceProductSection.Replace("@ProductTaxPercentage", taxPercentage);
                invoiceProductSection = invoiceProductSection.Replace("@ProductDiscountPercentage", trxLine.Discount_Percentage.ToString(amountFormat, invC));
                invoiceProductSection = invoiceProductSection.Replace("@ZeroBase", zeroBase);//Represents the value of the product if it taxes 0% VAT (8 int, 2 decimal)
                invoiceProductSection = invoiceProductSection.Replace("@TaxBase", taxBase);//Represents the value of the product if it taxes 12% VAT (8 int, 2 decimal).
                invoiceProductSection = invoiceProductSection.Replace("@NoTax", noTax);//Represents the value of the product if it does not tax VAT (8 int, 2 decimal).
            }
            catch (Exception ex)
            {
                log.Error("Error in Build Invoice Customer Line Section", ex);
                throw new Exception(MessageContainerList.GetMessage(executionContext, 4215) + " : " + ex.Message);//Error in Build Invoice Customer Line Section
            }
            log.LogMethodExit(invoiceProductSection);
            return invoiceProductSection;
        }

        private string GetexternalSystemReference(Transaction.Transaction.TransactionLine trxLine)
        {
            log.LogMethodEntry();
            string prodExternalSystemReference = string.Empty;
            try
            {
                ProductsContainerDTO parentProductContainerDTO = null;
                parentProductContainerDTO = ProductsContainerList.GetProductsContainerDTO(executionContext, trxLine.ProductID);
                prodExternalSystemReference = parentProductContainerDTO.ExternalSystemReference;
                if (string.IsNullOrEmpty(prodExternalSystemReference))
                {
                    log.Error("External reference of the product is empty Please check the product setup");
                    log.LogMethodExit(parentProductContainerDTO.ProductId, "External reference of the product is empty Please check the product setup");
                    throw new Exception(MessageContainerList.GetMessage(executionContext, 4216, trxLine.ProductName)); //External reference of the product is empty Please check the product setup
                }
            }
            catch (Exception ex)
            {
                log.Error("Error while getting external reference of the product", ex);
                throw;
            }
            log.LogMethodExit(prodExternalSystemReference);
            return prodExternalSystemReference;
        }

        private string BuildInvoiceVendorLineSection(Transaction.Transaction transaction, string invoiceType, bool isReversal)
        {
            log.LogMethodEntry(transaction, invoiceType, isReversal);
            string invoiceVendorLineSection = string.Empty;
            try
            {
                if (!isReversal)
                {
                    invoiceVendorLineSection = facturaVendorTemplate;
                    invoiceVendorLineSection = invoiceVendorLineSection.Replace("@VendorRUC", "\"" + accountingSPRUC + "\"");
                    invoiceVendorLineSection = invoiceVendorLineSection.Replace("@VendorIDNumber", "\"" + accountingSPIdentificationCode + "\"");
                    invoiceVendorLineSection = invoiceVendorLineSection.Replace("@VendorName", "\"" + accountingSPName + "\"");
                    invoiceVendorLineSection = invoiceVendorLineSection.Replace("@VendorContactNumber", "\"" + accountingSPContactNumber + "\"");
                    invoiceVendorLineSection = invoiceVendorLineSection.Replace("@VendorType", "\"" + accountingSPType + "\"");
                    invoiceVendorLineSection = invoiceVendorLineSection.Replace("@VendorEmail", "\"" + accountingSPEmailID + "\"");
                }
                else
                {
                    invoiceVendorLineSection = creditNoteVendorTemplate;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error in Build Invoice Customer Line Section", ex);
                throw new Exception(MessageContainerList.GetMessage(executionContext, 4217) + " : " + ex.Message);//Error in Build Invoice Customer Line Section
            }
            log.LogMethodExit(invoiceVendorLineSection);
            return invoiceVendorLineSection;
        }


        public override List<ExSysSynchLogDTO> PostInvoice(string invoicefileNamewithPath)
        {
            log.LogMethodEntry(invoicefileNamewithPath);
            List<ExSysSynchLogDTO> trxLogList = new List<ExSysSynchLogDTO>();
            if (string.IsNullOrWhiteSpace(invoicefileNamewithPath) == false)
            {
                using (NoSynchronizationContextScope.Enter())
                {
                    string fileName = Path.GetFileName(invoicefileNamewithPath);
                    Task<List<ExSysSynchLogDTO>> task = PostJson(invoicefileNamewithPath, fileName);
                    task.Wait();
                    trxLogList = task.Result;
                }
            }
            else
            {
                log.Error("invoicefileNamewithPath is not provided.");
            }
            log.LogMethodExit(trxLogList);
            return trxLogList;

        }

        private List<LookupValuesContainerDTO> GetOutFolderPathInfo()
        {
            log.LogMethodEntry();
            List<LookupValuesContainerDTO> lookupValuesDTOList = new List<LookupValuesContainerDTO>(invoiceSetupLookupValueDTOList);
            try
            {
                if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
                {
                    lookupValuesDTOList = lookupValuesDTOList.Where(lv => string.IsNullOrWhiteSpace(lv.LookupValue) == false
                                                                        && lv.LookupValue.Contains(OUT_PATH_PARTIAL_STRING)).ToList();
                }
            }
            catch (Exception ex)
            {
                log.Error("Get Out Folder Path Info Error", ex);
                throw new Exception(MessageContainerList.GetMessage(executionContext, 4222) + " : " + ex.Message);//Get Out Folder Path Info Error
            }
            log.LogMethodExit(lookupValuesDTOList);
            return lookupValuesDTOList;
        }

        private void VerifyFolderPath(string filePath)
        {
            log.LogMethodEntry(filePath);
            if (!System.IO.Directory.Exists(filePath))
            {
                throw new Exception(MessageContainerList.GetMessage(executionContext, "Folder") + " " + filePath.ToString() + " "
                                                                     + MessageContainerList.GetMessage(executionContext, "not found."));
            }
            log.LogMethodExit();
        }

        private string VerifyFileForUpdates(DirectoryInfo directoryInfo, string fileName, DateTime lastUpdateTime)
        {
            log.LogMethodEntry(fileName, lastUpdateTime);
            bool processFile = false;
            int unexpectedIssue = 0;
            do
            {
                //Loop 3 times ensure file being selected is not being updated while it is being picked for processing 
                var jsonFileTemp = directoryInfo.GetFiles().Where(f => f.FullName == fileName).OrderBy(t => t.LastWriteTime).ToList();
                if (jsonFileTemp.Count == 0)
                {
                    unexpectedIssue++;
                }
                else
                {
                    if (jsonFileTemp[0].FullName == fileName && jsonFileTemp[0].LastWriteTime == lastUpdateTime)
                    {
                        processFile = true;
                    }
                    else
                    {
                        lastUpdateTime = jsonFileTemp[0].LastWriteTime;
                        System.Threading.Thread.Sleep(500);
                        unexpectedIssue++;
                    }
                }
            } while (processFile == false && unexpectedIssue < 10);
            if (unexpectedIssue > 9)
            {
                fileName = string.Empty;
            }
            log.LogMethodExit(fileName);
            return fileName;
        }

        private async Task<List<ExSysSynchLogDTO>> PostJson(string fileNameWithPath, string fileName)
        {
            log.LogMethodEntry(fileNameWithPath, fileName);
            List<ExSysSynchLogDTO> trxLogList = new List<ExSysSynchLogDTO>();
            TransactionDTO transactionDTO = null;
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12;
                HttpClient client = new HttpClient();
                client.Timeout = System.Threading.Timeout.InfiniteTimeSpan;
                string result = "";
                int trx_id = -1;
                string jsonRequestContent = File.ReadAllText(fileNameWithPath);
                transactionDTO = GetTransactionDTOfromRequest(jsonRequestContent);
                if (transactionDTO != null)
                {
                    trx_id = transactionDTO.TransactionId;
                }
                log.LogVariableState("jsonRequestContent", jsonRequestContent);
                log.LogVariableState("postURLForInvoice", postURLForInvoice);
                using (var clients = new HttpClient())
                {
                    clients.DefaultRequestHeaders.Add("Authorization", authKey);
                    {
                        var message = await clients.PostAsync(postURLForInvoice, new StringContent(jsonRequestContent, Encoding.UTF8, "application/json"));
                        log.LogVariableState("message", message);
                        result = await message.Content.ReadAsStringAsync();
                        log.LogVariableState("result", result);
                        if (message != null && message.IsSuccessStatusCode)
                        {
                            result = await message.Content.ReadAsStringAsync();
                            string trxExternalSystemRefNo = UpdateExternalSystemReference(result, transactionDTO);
                            log.LogVariableState("Json post completed for", fileName);

                            //Move files from In folders to Moved folder
                            MoveFiles(fileNameWithPath, fileName, result);

                            ExSysSynchLogDTO exSysSynchLogDTO = new ExSysSynchLogDTO(logId: -1, timestamp: ServerDateTime.Now, exSysName: EX_SYS_NAME,
                              parafaitObject: TRANSACTION, parafaitObjectId: transactionDTO.TransactionId, parafaitObjectGuid: transactionDTO.Guid,
                             isSuccessFul: true, status: SUCCESS_STATUS, data: trxExternalSystemRefNo, remarks: this.programName, concurrentRequestId: this.concurrentRequestId);
                            ExSysSynchLogBL exSysSynchLogBL = new ExSysSynchLogBL(executionContext, exSysSynchLogDTO);
                            exSysSynchLogBL.Save();
                            trxLogList.Add(exSysSynchLogBL.ExSysSynchLogDTO);
                        }
                        else
                        {
                            //Move files from In folders to Moved folder
                            MoveFiles(fileNameWithPath, fileName, result, true);
                            string errorMsg = "StatusCode: " + message.StatusCode + " Error: " + result;
                            log.Info("Skipping " + fileName + " as PostXML() failed " + errorMsg);
                            throw new Exception(errorMsg);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                int trxId = (transactionDTO != null ? transactionDTO.TransactionId : -1);
                string guidValue = (transactionDTO != null ? transactionDTO.Guid : string.Empty);
                string msg = "Error while posting " + trxId + " :";
                ExSysSynchLogDTO logDTO = LogError(ex, msg, TRANSACTION, trxId, guidValue);
                if (logDTO != null)
                {
                    trxLogList.Add(logDTO);
                }
            }
            log.LogMethodExit(trxLogList);
            return trxLogList;
        }

        private string UpdateExternalSystemReference(string result, TransactionDTO transactionDTO)
        {
            log.LogMethodEntry(result, transactionDTO);
            string trxExternalSystemReferenceNumber = string.Empty;
            try
            {
                if (result.Contains("id"))
                {
                    var jo = JObject.Parse(result);
                    trxExternalSystemReferenceNumber = jo["id"].ToString();
                }
                if (transactionDTO != null)
                {
                    transactionDTO.ExternalSystemReference = trxExternalSystemReferenceNumber;
                    TransactionBL transactionBL = new TransactionBL(executionContext, transactionDTO);
                    transactionBL.Save();
                }
            }
            catch (Exception ex)
            {
                log.Error("Error while Updating External System Reference", ex);
                throw new Exception(MessageContainerList.GetMessage(executionContext, 4223) + " : " + ex.Message);//Error while Updating External System Reference
            }
            log.LogMethodExit(trxExternalSystemReferenceNumber);
            return trxExternalSystemReferenceNumber;
        }

        private TransactionDTO GetTransactionDTOfromRequest(string jsonRequestContent)
        {
            log.LogMethodEntry(jsonRequestContent);
            TransactionDTO transactionDTO = null;
            try
            {
                if (!string.IsNullOrEmpty(jsonRequestContent) && jsonRequestContent.Contains("adicional1"))
                {
                    var jo = JObject.Parse(jsonRequestContent);
                    string trxGuid = jo["adicional1"].ToString();
                    transactionDTO = GetTransactionDTO(trxGuid);
                }
                else
                {
                    log.Info("adicional1 is not found in json");
                }
            }
            catch (Exception ex)
            {
                log.Error("Error while Getting Transaction DTO", ex);
                throw new Exception(MessageContainerList.GetMessage(executionContext, 4224) + " : " + ex.Message);//Error while Getting Transaction DTO
            }
            log.LogMethodExit(transactionDTO);
            return transactionDTO;
        }

        private TransactionDTO GetTransactionDTO(string trxGuid)
        {
            log.LogMethodEntry(trxGuid);
            TransactionDTO transactionDTO = null;
            try
            {
                TransactionUtils transactionUtils = new TransactionUtils(utilities);
                List<TransactionDTO> transactionDTOList;
                TransactionListBL transactionListBL = new TransactionListBL(executionContext);
                List<KeyValuePair<TransactionDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<TransactionDTO.SearchByParameters, string>>();
                searchParams.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                searchParams.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.GUID, trxGuid));
                transactionDTOList = transactionListBL.GetTransactionDTOList(searchParams, utilities, null, 0, 5000);
                if (transactionDTOList != null && transactionDTOList.Any())
                {
                    transactionDTO = transactionDTOList[0];
                }
                else
                {
                    throw new ValidationException("No Transaction Data found for transaction " + trxGuid);
                }
            }
            catch (Exception ex)
            {
                log.Error("Error while Getting Transaction DTO", ex);
                throw new Exception(MessageContainerList.GetMessage(executionContext, 4224) + " : " + ex.Message);//Error while Getting Transaction DTO
            }
            log.LogMethodExit(transactionDTO);
            return transactionDTO;
        }

        private void MoveFiles(string fileNameWithPath, string fileName, string response, bool postFailed = false)
        {
            log.LogMethodEntry(fileNameWithPath, fileName, response);
            try
            {
                if (moveFolderPath != null)
                {
                    if (!Directory.Exists(moveFolderPath))
                    {
                        Directory.CreateDirectory(moveFolderPath);
                    }
                    string stringTimestamp = ServerDateTime.Now.ToString().Replace("/", "").Replace("-", "").Replace(" ", "").Replace(":", "").Replace(".", "").Replace(",", "");
                    string fileNameWithDatetime = stringTimestamp + "_" + "Response Message" + "_" + fileName;
                    File.WriteAllText(moveFolderPath + fileNameWithDatetime, response);
                    try
                    {
                        if (File.Exists(Path.Combine(moveFolderPath + fileName)))
                        {
                            File.Delete(Path.Combine(moveFolderPath + fileName));
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(fileName, ex);
                    }
                    string fileNameValue = string.Empty;
                    try
                    {
                        fileNameValue = (postFailed ? "Errored_" + stringTimestamp + "_" : "") + fileName;
                        File.Move(fileNameWithPath, moveFolderPath + fileNameValue);
                    }
                    catch (Exception ex)
                    {
                        log.Error(fileNameValue, ex);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Error while Moving Files", ex);
            }
            log.LogMethodExit();
        }

        private void SetLookUpValueList()
        {
            log.LogMethodEntry();
            try
            {
                invoiceSetupLookupValueDTOList = LookupsContainerList.GetLookupsContainerDTO(executionContext.GetSiteId(), FISCAL_INVOICE_SETUP, executionContext).LookupValuesContainerDTOList;
                invoiceAttributesValuesDTOList = LookupsContainerList.GetLookupsContainerDTO(executionContext.GetSiteId(), FISCAL_INVOICE_ATTRIBUTES, executionContext).LookupValuesContainerDTOList;

                log.LogVariableState("invoiceSetupLookupValueDTOList", invoiceSetupLookupValueDTOList);
                log.LogVariableState("invoiceAttributesValuesDTOList", invoiceAttributesValuesDTOList);
            }
            catch (Exception ex)
            {
                log.Error("Error while Settting LookUp Value List", ex);
                throw new Exception(MessageContainerList.GetMessage(executionContext, 4226) + " : " + ex.Message);//Error while Getting Invoice Setup LookUp
            }
            log.LogMethodExit();
        }

        private string GetAuthenticationKey()
        {
            log.LogMethodEntry();
            string value = GetLookupValueDescription(INVOICEAUTHKEYLOOKUPVALUENAME);
            log.LogMethodExit(value);
            return value;
        }

        private string GetPosAPIToken()
        {
            log.LogMethodEntry();
            string value = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, INVOICEAPITOKEN);
            log.LogMethodExit(value);
            return value;
        }

        private string GetPostURLForInvoice()
        {
            log.LogMethodEntry();
            string value = GetLookupValueDescription(POSTURLFORINVOICELOOKUPVALUENAME);
            log.LogMethodExit(value);
            return value;
        }

        private string GetLookupValueDescription(string lookupValueName)
        {
            log.LogMethodEntry(lookupValueName);
            string description = string.Empty;
            try
            {
                if (invoiceSetupLookupValueDTOList != null && invoiceSetupLookupValueDTOList.Any())
                {
                    LookupValuesContainerDTO lookupValueDTO = invoiceSetupLookupValueDTOList.Find(lv => lv.LookupValue == lookupValueName);
                    if (lookupValueDTO == null)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2932, lookupValueName, FISCAL_INVOICE_SETUP));
                    }
                    else
                    {
                        description = lookupValueDTO.Description;
                    }
                }
                else
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2933, FISCAL_INVOICE_SETUP));

                }
            }
            catch (Exception ex)
            {
                log.Error("Error while Getting Lookup Value Description", ex);
                throw new Exception(MessageContainerList.GetMessage(executionContext, 4227) + " : " + ex.Message);//Error while Getting Lookup Value Description
            }
            log.LogMethodExit(description);
            return description;
        }

        private decimal TruncateDecimal(decimal value)
        {
            log.LogMethodEntry(value, amountPrecision);
            decimal step = (decimal)Math.Pow(10, amountPrecision);
            decimal tmp = Math.Truncate(step * value);
            decimal outVal = tmp / step;
            log.LogMethodExit(outVal);
            return outVal;
        }

        private decimal GetDiscountAmount(Transaction.Transaction transaction)
        {
            log.LogMethodEntry();
            decimal outVal = 0;
            if (transaction.TransactionLineList != null && transaction.TransactionLineList.Any())
            {
                foreach (Transaction.Transaction.TransactionLine trxLine in transaction.TransactionLineList)
                {
                    decimal discountFactor = (trxLine.Discount_Percentage == 0 ? 0 : (decimal)(trxLine.Discount_Percentage / 100));
                    if (discountFactor != 0)
                    {
                        decimal taxFactor = (decimal)(trxLine.tax_percentage == 0 ? 0 : trxLine.tax_percentage / 100);
                        decimal lineAmount = (decimal)trxLine.Price + ((decimal)trxLine.Price * taxFactor);
                        outVal = outVal + (discountFactor * lineAmount);
                    }
                }
            }
            log.LogMethodExit(outVal);
            return outVal;
        } 

        private decimal GetDiscountedTotalAmount(Transaction.Transaction transaction)
        {
            log.LogMethodEntry();
            decimal outVal = 0;
            if (transaction.TransactionLineList != null && transaction.TransactionLineList.Any())
            {
                foreach (Transaction.Transaction.TransactionLine trxLine in transaction.TransactionLineList)
                {
                    decimal lineAmount = GetDiscountedLineAmount(trxLine);
                    outVal = outVal + lineAmount;
                }
            }
            log.LogMethodExit(outVal);
            return outVal;
        }
        
        private decimal GetDiscountedTotalPreTaxAmount(Transaction.Transaction transaction)
        {
            log.LogMethodEntry();
            decimal outVal = 0;
            if (transaction.TransactionLineList != null && transaction.TransactionLineList.Any())
            {
                foreach (Transaction.Transaction.TransactionLine trxLine in transaction.TransactionLineList)
                {
                    decimal linePrice = GetDiscountedLinePrice(trxLine);
                    outVal = outVal + linePrice;
                }
            }
            log.LogMethodExit(outVal);
            return outVal;
        }

        private decimal GetDiscountedTotalTaxAmount(Transaction.Transaction transaction)
        {
            log.LogMethodEntry();

            decimal outVal = 0;
            if (transaction.TransactionLineList != null && transaction.TransactionLineList.Any())
            {
                foreach (Transaction.Transaction.TransactionLine trxLine in transaction.TransactionLineList)
                {
                    decimal lineTaxAmount = GetDiscountedLineTaxAmount(trxLine);
                    outVal = outVal + lineTaxAmount;
                }
            }
            log.LogMethodExit(outVal);
            return outVal;
        }

        protected override List<string> GetFolderPath()
        {
            log.LogMethodEntry();
            List<string> folderList = GetAllErrorFolderPath(invoiceSetupLookupValueDTOList);
            if (folderList == null)
            {
                folderList = new List<string>();
            }
            List<string> sentFolderList = GetAllSentFolderPath(invoiceSetupLookupValueDTOList);
            if (sentFolderList != null && sentFolderList.Any())
            {
                folderList.AddRange(sentFolderList);
            }
            folderList.Add(moveFolderPath);
            log.LogMethodExit(folderList);
            return folderList;
        }

        private List<TransactionDTO> GetEligibleInvoiceRecords(List<int> trxIdList)
        {
            log.LogMethodEntry(trxIdList);
            List<TransactionDTO> transactionDTOList = new List<TransactionDTO>();
            try
            {
                if (trxIdList == null || trxIdList.Any() == false)
                {
                    string msg1 = MessageContainerList.GetMessage(executionContext, 1831) + " [TransactionIdList]"; //The Parameter should not be empty
                    ValidationException validationException = new ValidationException(msg1);
                    throw validationException;
                }
                else if (trxIdList.Count > 200)
                {
                    string msg2 = MessageContainerList.GetMessage(executionContext, 5142, "[TransactionIdList]", 200);
                    //"Parameter &1 cannot have more than &&2 values"
                    ValidationException validationException = new ValidationException(msg2);
                    throw validationException;
                }
                string trxIdValues = FiscalizationHelper.GetIDString(trxIdList);
                TransactionListBL transactionListBL = new TransactionListBL(executionContext);
                List<KeyValuePair<TransactionDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<TransactionDTO.SearchByParameters, string>>();
                searchParams.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                searchParams.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.TRANSACTION_ID_LIST, trxIdValues));
                searchParams.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.STATUS, Transaction.Transaction.TrxStatus.CLOSED.ToString()));
                transactionDTOList = transactionListBL.GetTransactionDTOList(searchParams, utilities, null, 0, 5000);
                if (transactionDTOList != null && transactionDTOList.Any())
                {
                    List<TransactionDTO> reversedTransactionDTOList = transactionDTOList.FindAll(pc => pc.OriginalTransactionId > -1);
                    transactionDTOList = transactionDTOList.FindAll(pc => string.IsNullOrWhiteSpace(pc.ExternalSystemReference) == true && pc.OriginalTransactionId <= -1);

                    if (reversedTransactionDTOList != null && reversedTransactionDTOList.Any())
                    {
                        StringBuilder idListStringBuilder = new StringBuilder("");
                        string idList;
                        for (int i = 0; i < reversedTransactionDTOList.Count; i++)
                        {
                            if (i != 0)
                            {
                                idListStringBuilder.Append(",");
                            }

                            idListStringBuilder.Append(reversedTransactionDTOList[i].OriginalTransactionId.ToString());
                        }
                        idList = idListStringBuilder.ToString();
                        List<KeyValuePair<TransactionDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<TransactionDTO.SearchByParameters, string>>();
                        searchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        searchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.TRANSACTION_ID_LIST, idList.ToString()));
                        List<TransactionDTO> originalTransactionDTOList = transactionListBL.GetTransactionDTOList(searchParameters, null, null, 0, 5000);
                        if (originalTransactionDTOList != null && originalTransactionDTOList.Any())
                        {
                            foreach (TransactionDTO originalTransactionDTO in originalTransactionDTOList)
                            {
                                TransactionDTO transactionDTO = reversedTransactionDTOList.FirstOrDefault(rt => rt.OriginalTransactionId == originalTransactionDTO.TransactionId);
                                if (transactionDTO != null)
                                {
                                    //external ref not set. process it
                                    if (string.IsNullOrWhiteSpace(transactionDTO.ExternalSystemReference))
                                    {
                                        transactionDTOList.Add(transactionDTO);
                                    }
                                    else if (!string.IsNullOrWhiteSpace(originalTransactionDTO.ExternalSystemReference))
                                    {
                                        if (transactionDTO.ExternalSystemReference.Equals(originalTransactionDTO.ExternalSystemReference))
                                        {
                                            transactionDTOList.Add(transactionDTO);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(MessageContainerList.GetMessage(executionContext, 4207), ex);//Error in Get Eligible Invoice Records
                throw;
            }
            log.LogMethodExit(transactionDTOList);
            return transactionDTOList;
        }

        private ExSysSynchLogDTO LogError(Exception ex, string msg, string parafaitObject = TRANSACTION, int parafaitObjectId = -1, string parafaitObjectGuid = null)
        {
            log.LogMethodEntry(ex, msg, parafaitObject, parafaitObjectId, parafaitObjectGuid);
            ExSysSynchLogDTO exSysSynchLogDTO = null;
            string message = msg + ex.Message;
            if (ex.InnerException != null)
            {
                message = message + " " + ex.InnerException.Message;
            }
            try
            {
                exSysSynchLogDTO = new ExSysSynchLogDTO(logId: -1, timestamp: ServerDateTime.Now, exSysName: EX_SYS_NAME,
                  parafaitObject: parafaitObject, parafaitObjectId: parafaitObjectId, parafaitObjectGuid: parafaitObjectGuid,
                 isSuccessFul: false, status: ERROR_STATUS, data: message, remarks: this.programName, concurrentRequestId: this.concurrentRequestId);
                ExSysSynchLogBL exSysSynchLogBL = new ExSysSynchLogBL(executionContext, exSysSynchLogDTO);
                exSysSynchLogBL.Save();
                exSysSynchLogDTO = exSysSynchLogBL.ExSysSynchLogDTO;
            }
            catch (Exception exx)
            {
                log.Error(exx);
            }
            log.LogMethodExit(exSysSynchLogDTO);
            return exSysSynchLogDTO;
        }

        private TransactionDTO GetOrginalTrxDTO(TransactionDTO transactionDTO)
        {
            log.LogMethodEntry((transactionDTO != null ? transactionDTO.TransactionId : -1), (transactionDTO != null ? transactionDTO.OriginalTransactionId : -1));
            TransactionDTO originalTrxDTO = null;
            string idList = transactionDTO.OriginalTransactionId.ToString();
            TransactionListBL transactionListBL = new TransactionListBL(executionContext);
            List<KeyValuePair<TransactionDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<TransactionDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            searchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.TRANSACTION_ID_LIST, idList));
            List<TransactionDTO> originalTransactionDTOList = transactionListBL.GetTransactionDTOList(searchParameters, null, null, 0, 5000);
            if (originalTransactionDTOList != null && originalTransactionDTOList.Any())
            {
                originalTrxDTO = originalTransactionDTOList[0];
            }
            log.LogMethodExit((originalTrxDTO != null ? originalTrxDTO.TransactionId : -1));
            return originalTrxDTO;
        }
    }
}
