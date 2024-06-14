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
using Semnox.Parafait.Device.PaymentGateway;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Communication;
using System.Net;
using System.Configuration;
using Semnox.Parafait.Customer;
using Newtonsoft.Json;
using Semnox.Parafait.Discounts;
using Semnox.Parafait.Site;
using System.Net.Http;
using System.Xml;

namespace Semnox.Parafait.Fiscalization
{
    public class ChileFiscalization : ParafaitFiscalization
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        bool isReversal = false;
        string invoiceLineElement = "\"DETALLE\"" + ": [";
        string invoicePaymentElement = "\"SUBTOTIINFO\"" + ": [";
        string invoiceResultLine = string.Empty;

        private string invoiceType = string.Empty;
        private string invoiceTypeCode = string.Empty;
        private string boletaDocType = string.Empty;
        private string creditNoteDocType = string.Empty;
        private string fiscalNumber = string.Empty;
        private string serviceIndicator = string.Empty;

        private string customerName = string.Empty;
        private string posIdSiteName = string.Empty;
        private string waiterName = string.Empty;
        private string customerCount = string.Empty;
        private string cIUDADPOSTA = string.Empty;
        private string genericCustomerRUTId = string.Empty;
        private string cashierName = string.Empty;
        private string orderDescription = string.Empty;
        private string posDecription = string.Empty;
        private string transactionId = string.Empty;
        private string transactionNo = string.Empty;
        private string supplierRUT = string.Empty;
        private string customerRUTId = string.Empty;

        private string boletaHeaderTemplate = string.Empty;
        //private string creditNoteHeaderTemplate = string.Empty;
        private string invoiceOrderDetailsTemplate = string.Empty;
        //private string creditNoteCustomerLineTemplate = string.Empty;
        private string invoiceLineTemplate = string.Empty;
        //private string creditInvoiceLineVendorTemplate = string.Empty; 
        private string invoiceAmountDetailsTemplate = string.Empty;
        //private string creditNoteToatlDetailsTemplate = string.Empty;
        private string invoicePaymentTemplate = string.Empty;

        private string timeFormat = string.Empty;
        private string dateFormat = string.Empty;
        private string amountFormat = string.Empty;
        private string integerFormat = string.Empty;
        private int amountPrecision = 2;
        private string cultureInfo = string.Empty;
        private string currencyCode = string.Empty;
        private bool useCulture = false;
        private System.Globalization.CultureInfo invC = null;

        private string boleta_File_Upload_Path = string.Empty;
        private string factura_File_Upload_Path = string.Empty;
        private string creditNote_File_Upload_Path = string.Empty;
        private string fileUploadPath = string.Empty;
        private string recordType = string.Empty;
        string externalSystemReference = string.Empty;

        private const string FISCAL_INVOICE_SETUP = "FISCAL_INVOICE_SETUP";
        private const string FISCAL_INVOICE_ATTRIBUTES = "FISCAL_INVOICE_ATTRIBUTES";
        private const string ELIGIBLEOVERRIDEOPTIONS = "EligibleOverrideOptions";

        private const string BOLETA_HEADER = "BOLETA_HEADER";
        private const string INVOICE_DETAILS_LINE = "INVOICE_DETAILS_LINE";
        private const string INVOICE_TRANSACTION_LINE = "INVOICE_TRANSACTION_LINE";
        private const string INVOICE_AMOUNT_DETAILS = "INVOICE_AMOUNT_DETAILS";
        private const string INVOICE_PAYMENT_LINE = "INVOICE_PAYMENT_LINE";

        private const string POSTURLFORINVOICELOOKUPVALUENAME = "Post_URL_For_Invoice";
        private const string OUT_PATH_PARTIAL_STRING = "File_Upload_Path";
        private const string POST_FILE_EXTENSION = ".json";
        private const string POST_FILE_FORMAT = "JSON";
        private const string STORETAXIDNUMBER = "TAX_IDENTIFICATION_NUMBER";
        private const string EX_SYS_NAME = "ChileFiscalization";
        private const string TRANSACTION = "Transaction";
        private const string POST_INVOICE = "PostInvoice";
        private const string PRODUCT_UNIT_VALUE = "ITEM";
        private const string ACTION_TYPE_ISSUE = "ISSUE";
        private const string CASH_PAYMENT = "Cash";
        private const string ZERO_PRICE_TRX = "ZEROPRICETRX";
        private const string HUNDRED_PERC_DISCOUNT_TRX = "100PERCDISCOUNT_TRX";

        private string postURLForInvoice;
        private string apiAuthId;
        private string moveFolderPath;
        private List<LookupValuesContainerDTO> invoiceSetupLookupValueDTOList;
        private List<LookupValuesContainerDTO> invoiceAttributesValuesDTOList;

        /// <summary>
        /// Ecuador Fiscalization
        /// </summary>
        /// <param name="executionContext"></param>
        public ChileFiscalization(ExecutionContext executionContext, Utilities utilities)
            : base(executionContext, utilities)
        {
            log.LogMethodEntry(executionContext, "utilities");
            this.exSysName = EX_SYS_NAME;
            this.postFileFormat = JSONFORMAT;
            SetLookUpValueList();
            moveFolderPath = GetMoveFolderPath(invoiceSetupLookupValueDTOList);
            BuildInvoiceAttributes();
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
                string msg = MessageContainerList.GetMessage(executionContext, "Post invoice process takes care of creation and post tasks for Chile. No need to run Create Invoice process seperately");
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
            TransactionDTO trxDTO = null;
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
                if (transaction.Transaction_Amount == 0)
                {
                    //skip zero price transaction
                    trxDTO = transaction.TransactionDTO;
                    string zeroTrxExternalRef = ZERO_PRICE_TRX + "_" + trxId;
                    UpdateTrxExternalSystemRefNumber(trxDTO, zeroTrxExternalRef);
                    ExSysSynchLogDTO exSysSynchLogDTOZ = new ExSysSynchLogDTO(logId: -1, timestamp: ServerDateTime.Now, exSysName: EX_SYS_NAME,
                               parafaitObject: TRANSACTION, parafaitObjectId: transaction.Trx_id, parafaitObjectGuid: transaction.TrxGuid,
                              isSuccessFul: true, status: SUCCESS_STATUS, data: "Zero Price Transaction, skipping file generation", remarks: this.programName, concurrentRequestId: this.concurrentRequestId);
                    ExSysSynchLogBL exSysSynchLogBLZ = new ExSysSynchLogBL(executionContext, exSysSynchLogDTOZ);
                    exSysSynchLogBLZ.Save();
                    logDTOList.Add(exSysSynchLogBLZ.ExSysSynchLogDTO);
                    log.LogMethodExit(logDTOList);
                    return logDTOList;
                }
                SetInvoiceTypeAttributes(invoiceType);
                SetTrxOrderDetails(transaction);
                BuildProfileDTO(transaction);
                if (transaction.OriginalTrxId > 0)
                {
                    isReversal = true;
                }
                else
                {
                    isReversal = false;
                }
                invoiceResultLine = BuildHeaderSection(transaction, invoiceType, isReversal);
                invoiceResultLine += BuildOrderDetailsSection(transaction, invoiceType, isReversal);
                invoiceResultLine += BuildInvoiceAmountSection(transaction, invoiceType, isReversal);

                invoiceResultLine += invoiceLineElement;
                foreach (Transaction.Transaction.TransactionLine trxLine in transaction.TransactionLineList)
                {
                    invoiceResultLine = invoiceResultLine + BuildInvoiceLineSection(trxLine);
                    invoiceResultLine = invoiceResultLine + ",";
                }
                invoiceResultLine = invoiceResultLine.Substring(0, invoiceResultLine.Length - 1);
                invoiceResultLine += "],";


                invoiceResultLine += invoicePaymentElement;
                invoiceResultLine += BuildPaymentDetailsSection(transaction);
                invoiceResultLine += "]}";


                string pathAppender = fileUploadPath.EndsWith("\\") ? "" : "\\";
                fileName = fileUploadPath + pathAppender + invoiceTypeCode + "-" + transaction.Trx_No + POST_FILE_EXTENSION;
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

        private void UpdateTrxExternalSystemRefNumber(TransactionDTO transactionDTO, string externalSystemReferenceNumber)
        {
            log.LogMethodEntry("transactionDTO", externalSystemReferenceNumber);
            if (transactionDTO != null && transactionDTO.TransactionId > -1 && string.IsNullOrWhiteSpace(externalSystemReferenceNumber) == false)
            {
                transactionDTO.ExternalSystemReference = externalSystemReferenceNumber;
                TransactionBL transactionBL = new TransactionBL(executionContext, transactionDTO);
                transactionBL.Save();
            }
            log.LogMethodExit();
        }

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

        /// <summary>
        /// Reprocess Invoice
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
        private string BuildHeaderSection(Transaction.Transaction trx, string invoiceType, bool isReversal)
        {
            log.LogMethodEntry("trx", invoiceType, isReversal);
            string headerSection = string.Empty;
            try
            {
                if (isReversal)
                {
                    //headerSection = BuildCreditNoteHeadervalues(trx);
                }
                else if (invoiceType == "Boleta")
                {
                    headerSection = BuildInvoiceHeadervalues(trx, boletaHeaderTemplate);
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
                headerSection = headerSection.Replace("@DocType", "\"" + boletaDocType + "\"");
                headerSection = headerSection.Replace("@FiscalNum", fiscalNumber);
                headerSection = headerSection.Replace("@DocIssueDate", "\"" + (transaction.TrxDate).ToString(dateFormat, invC) + "\"");
                headerSection = headerSection.Replace("@ServiceIndicator", serviceIndicator);

            }
            catch (Exception ex)
            {
                log.Error("Error in Build Invoice Header values", ex);
                throw new Exception(MessageContainerList.GetMessage(executionContext, 4210) + " : " + ex.Message);//Error in Build Invoice Header values
            }
            log.LogMethodExit(headerSection);
            return headerSection;
        }

        private string BuildOrderDetailsSection(Transaction.Transaction transaction, string invoiceType, bool isReversal)
        {
            log.LogMethodEntry("transaction", invoiceType, isReversal);
            string invoiceCustomerLineSection = string.Empty;
            try
            {
                invoiceCustomerLineSection = (isReversal == false ? invoiceOrderDetailsTemplate : "");//creditNoteCustomerLineTemplate);
                invoiceCustomerLineSection = invoiceCustomerLineSection.Replace("@GenericStoreRut", "\"" + customerRUTId + "\"");
                invoiceCustomerLineSection = invoiceCustomerLineSection.Replace("@StorePOSIdSiteName", "\"" + posIdSiteName + "\"");
                invoiceCustomerLineSection = invoiceCustomerLineSection.Replace("@CustomerName", "\"" + customerName + "\"");
                invoiceCustomerLineSection = invoiceCustomerLineSection.Replace("@WaiterName", "\"" + waiterName + "\"");
                invoiceCustomerLineSection = invoiceCustomerLineSection.Replace("@GuestCount", "\"" + customerCount + "\"");
                invoiceCustomerLineSection = invoiceCustomerLineSection.Replace("@CashierName", "\"" + cashierName + "\"");
                invoiceCustomerLineSection = invoiceCustomerLineSection.Replace("@OrderDescr", "\"" + orderDescription + "\"");
                invoiceCustomerLineSection = invoiceCustomerLineSection.Replace("@DescrServ", "\"" + posDecription + "\"");
                invoiceCustomerLineSection = invoiceCustomerLineSection.Replace("@TransId", "\"" + transactionId + "\"");
                invoiceCustomerLineSection = invoiceCustomerLineSection.Replace("@ServiceType", "\"" + cIUDADPOSTA + "\"");//optional for restaurant
            }
            catch (Exception ex)
            {
                log.Error("Error in Build Order Details Section", ex);
                throw new Exception(MessageContainerList.GetMessage(executionContext, 4213) + " : " + ex.Message);//Error in Build Invoice Customer Line Section
            }
            log.LogMethodExit();
            return invoiceCustomerLineSection;
        }

        private string BuildInvoiceAmountSection(Transaction.Transaction transaction, string invoiceType, bool isReversal)
        {
            log.LogMethodEntry();
            string invoiceDetailsSection = string.Empty;
            try
            {
                decimal taxAmount = Convert.ToDecimal(transaction.Tax_Amount);
                decimal totalAmount = Convert.ToDecimal(transaction.Transaction_Amount);
                decimal trxTotalAmount = totalAmount;
                if (transaction.Discount_Amount > 0)
                {
                    decimal discountedtaxAmount = GetDiscountedTotalTaxAmount(transaction);
                    decimal discountedtotalAmount = GetDiscountedTotalAmount(transaction);
                    trxTotalAmount = discountedtotalAmount;
                    taxAmount = discountedtaxAmount;
                }
                if (trxTotalAmount <= 0 && isReversal == false) // to handle 100% discount.
                {
                    taxAmount = 0;
                }
                //preTaxAmount = TruncateDecimal(preTaxAmount);
                //taxAmount = TruncateDecimal(taxAmount);
                //trxTotalAmount = TruncateDecimal(trxTotalAmount);
                trxTotalAmount = ConvertToInt(trxTotalAmount);
                taxAmount = ConvertToInt(taxAmount);
                decimal netAmount = trxTotalAmount - taxAmount;

                invoiceDetailsSection = (isReversal == false ? invoiceAmountDetailsTemplate : "");//creditNoteToatlDetailsTemplate);
                string docType = (isReversal == false ? boletaDocType : creditNoteDocType);
                invoiceDetailsSection = invoiceDetailsSection.Replace("@NetAmt", Math.Abs(netAmount).ToString(integerFormat, invC));
                invoiceDetailsSection = invoiceDetailsSection.Replace("@TaxAmt", Math.Abs(taxAmount).ToString(integerFormat, invC));
                invoiceDetailsSection = invoiceDetailsSection.Replace("@TotalAmt", Math.Abs(trxTotalAmount).ToString(integerFormat, invC));
            }
            catch (Exception ex)
            {
                log.Error("Error in Build Invoice Amount Section", ex);
                throw new Exception(MessageContainerList.GetMessage(executionContext, "Error") + " : " + ex.Message);
            }
            log.LogMethodExit(invoiceDetailsSection);
            return invoiceDetailsSection;
        }

        private string BuildInvoiceLineSection(Transaction.Transaction.TransactionLine trxLine)
        {
            log.LogMethodEntry();
            string invoiceProductSection = invoiceLineTemplate;
            try
            {
                decimal finalLinePrice = GetDiscountedLinePrice(trxLine);
                decimal finalLineAmount = ConvertToInt(GetDiscountedLineAmount(trxLine));
                invoiceProductSection = invoiceProductSection.Replace("@ProdCode", "\"" + DoSubstring(trxLine.ProductID.ToString(), 35) + "\"");
                invoiceProductSection = invoiceProductSection.Replace("@ProdName", "\"" + DoSubstring(trxLine.ProductName, 80) + "\"");
                invoiceProductSection = invoiceProductSection.Replace("@Quantity", Math.Abs(trxLine.quantity).ToString(integerFormat, invC));
                invoiceProductSection = invoiceProductSection.Replace("@Unit", "\"" + PRODUCT_UNIT_VALUE + "\"");
                invoiceProductSection = invoiceProductSection.Replace("@Price", finalLinePrice.ToString(amountFormat, invC));
                invoiceProductSection = invoiceProductSection.Replace("@Amt", finalLineAmount.ToString(integerFormat, invC));
            }
            catch (Exception ex)
            {
                log.Error("Error in Build Invoice Line Section", ex);
                throw new Exception(MessageContainerList.GetMessage(executionContext, "Error") + " : " + ex.Message);
            }
            log.LogMethodExit(invoiceProductSection);
            return invoiceProductSection;
        }

        private string BuildPaymentDetailsSection(Transaction.Transaction transaction)
        {
            log.LogMethodEntry();
            string invoicePaymentDetailsSection = string.Empty;
            try
            {

                invoicePaymentDetailsSection = invoicePaymentTemplate;
                if (transaction.TransactionPaymentsDTOList != null && transaction.TransactionPaymentsDTOList.Any())
                {
                    List<PaymentModeDTO> paymentModeList = transaction.TransactionPaymentsDTOList.Select(tp => tp.PaymentModeDTO).Distinct().ToList();
                    if (paymentModeList != null && paymentModeList.Any())
                    {
                        foreach (PaymentModeDTO paymentMode in paymentModeList)
                        {
                            string paymentMethod = paymentMode.PaymentMode;
                            decimal amountPaid = (decimal)transaction.TransactionPaymentsDTOList.Where(tp => tp.PaymentModeDTO.PaymentModeId == paymentMode.PaymentModeId).Sum(tpp => tpp.Amount);
                            amountPaid = ConvertToInt(amountPaid);
                            invoicePaymentDetailsSection = invoicePaymentDetailsSection.Replace("@Method", "\"" + DoSubstring(paymentMethod, 30) + "\"");
                            invoicePaymentDetailsSection = invoicePaymentDetailsSection.Replace("@PaidAmt", Math.Abs(amountPaid).ToString(integerFormat, invC));
                            invoicePaymentDetailsSection = invoicePaymentDetailsSection + ",";
                        }
                        invoicePaymentDetailsSection = invoicePaymentDetailsSection.Substring(0, invoicePaymentDetailsSection.Length - 1);
                    }
                }
                else if (transaction.Transaction_Amount == 0 || transaction.Net_Transaction_Amount == 0)
                { //100% discount or zero price product sale
                    decimal amountPaid = 0;
                    invoicePaymentDetailsSection = invoicePaymentDetailsSection.Replace("@Method", "\"" + CASH_PAYMENT + "\"");
                    invoicePaymentDetailsSection = invoicePaymentDetailsSection.Replace("@PaidAmt", Math.Abs(amountPaid).ToString(integerFormat, invC));
                }
            }
            catch (Exception ex)
            {
                log.Error("Error in Build payment method Section", ex);
                throw new Exception(MessageContainerList.GetMessage(executionContext, "Error") + " : " + ex.Message);
            }
            log.LogMethodExit(invoicePaymentDetailsSection);
            return invoicePaymentDetailsSection;
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
                List<LookupValuesContainerDTO> lookupValuesDTOList = GetEligibleOverrideOptions();
                if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
                {
                    string overrideOptions = lookupValuesDTOList[0].Description;
                    TransactionListBL transactionListBL = new TransactionListBL(executionContext);
                    List<KeyValuePair<TransactionDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<TransactionDTO.SearchByParameters, string>>();
                    searchParams.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                    searchParams.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.TRANSACTION_ID_LIST, trxIdValues));
                    searchParams.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.POS_OVERRIDE_OPTION_NAME_LIST, overrideOptions));
                    searchParams.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.STATUS, Transaction.Transaction.TrxStatus.CLOSED.ToString()));
                    transactionDTOList = transactionListBL.GetTransactionDTOList(searchParams, utilities, null, 0, 5000);
                    if (transactionDTOList != null && transactionDTOList.Any())
                    {
                        transactionDTOList = transactionDTOList.FindAll(pc => pc.OriginalTransactionId <= -1);
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

        private async Task<List<ExSysSynchLogDTO>> PostJson(string fileNameWithPath, string fileName)
        {
            log.LogMethodEntry(fileNameWithPath, fileName);
            bool hasError = true;
            string errorDetails = string.Empty;
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
                    var content = new List<KeyValuePair<string, string>>();
                    content.Add(new KeyValuePair<string, string>("ID", apiAuthId));
                    content.Add(new KeyValuePair<string, string>("RUT", supplierRUT));
                    content.Add(new KeyValuePair<string, string>("DTE", boletaDocType));
                    content.Add(new KeyValuePair<string, string>("DataConvert", ConvertToBase64(jsonRequestContent)));
                    content.Add(new KeyValuePair<string, string>("Extension", POST_FILE_FORMAT));
                    content.Add(new KeyValuePair<string, string>("action", ACTION_TYPE_ISSUE));
                    var req = new HttpRequestMessage(HttpMethod.Post, postURLForInvoice) { Content = new FormUrlEncodedContent(content) };
                    var message = await client.SendAsync(req);
                    log.LogVariableState("message", message);
                    result = await message.Content.ReadAsStringAsync();
                    log.LogVariableState("result", result);
                    hasError = GetHasError(result);
                    if (message != null && message.IsSuccessStatusCode && hasError == false)
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
                        errorDetails = GetErrorDetails(result);
                        MoveFiles(fileNameWithPath, fileName, result, true);
                        string errorMsg = "StatusCode: " + message.StatusCode + " Error: " + errorDetails;
                        log.Info("Skipping " + fileName + " as PostXML() failed " + errorMsg);
                        throw new Exception(errorMsg);
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

        private TransactionDTO GetTransactionDTOfromRequest(string jsonRequestContent)
        {
            log.LogMethodEntry(jsonRequestContent);
            TransactionDTO transactionDTO = null;
            try
            {
                if (!string.IsNullOrEmpty(jsonRequestContent) && jsonRequestContent.Contains("CMNAPOSTAL"))
                {
                    var jo = JObject.Parse(jsonRequestContent);
                    string trxId = jo["RECEPTOR"]["CMNAPOSTAL"].ToString();
                    transactionDTO = GetTransactionDTO(trxId);
                }
                else
                {
                    log.Info("CMNAPOSTAL is not found in json");
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

        private TransactionDTO GetTransactionDTO(string trxId)
        {
            log.LogMethodEntry(trxId);
            TransactionDTO transactionDTO = null;
            try
            {
                TransactionUtils transactionUtils = new TransactionUtils(utilities);
                List<TransactionDTO> transactionDTOList;
                TransactionListBL transactionListBL = new TransactionListBL(executionContext);
                List<KeyValuePair<TransactionDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<TransactionDTO.SearchByParameters, string>>();
                searchParams.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                searchParams.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.TRANSACTION_ID, trxId));
                transactionDTOList = transactionListBL.GetTransactionDTOList(searchParams, utilities, null, 0, 5000);
                if (transactionDTOList != null && transactionDTOList.Any())
                {
                    transactionDTO = transactionDTOList[0];
                }
                else
                {
                    throw new ValidationException("No Transaction Data found for transaction " + trxId);
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
        private string UpdateExternalSystemReference(string result, TransactionDTO transactionDTO)
        {
            log.LogMethodEntry(result, transactionDTO);
            string trxExternalSystemReferenceNumber = string.Empty;
            try
            {
                if (result.Contains("FolioAsignado"))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(result);
                    XmlNodeList nList = doc.SelectNodes("iDTE/Response/FolioAsignado");
                    foreach (XmlNode node in nList)
                    {
                        trxExternalSystemReferenceNumber = node.InnerText;
                        if (transactionDTO.TransactionAmount != 0 && transactionDTO.TransactionNetAmount == 0)
                        {
                            trxExternalSystemReferenceNumber = HUNDRED_PERC_DISCOUNT_TRX + "_" + transactionDTO.TransactionId;
                        }
                        break;
                    }
                }
                if (string.IsNullOrWhiteSpace(trxExternalSystemReferenceNumber))
                {
                    string errMsg = MessageContainerList.GetMessage(executionContext, 4519, "FolioAsignado", MessageContainerList.GetMessage(executionContext, "Transaction"));
                    //Unexpected error, Unable to fetch &1 data for &2
                    ValidationException ve = new ValidationException(errMsg);
                    throw ve;
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

        private void SetInvoiceTemplates()
        {
            log.LogMethodEntry();
            try
            {
                boletaHeaderTemplate = LoadTemplateData(BOLETA_HEADER);
                //creditNoteHeaderTemplate = LoadTemplateData(CREDITNOTE_HEADER);
                invoiceOrderDetailsTemplate = LoadTemplateData(INVOICE_DETAILS_LINE);
                //creditNoteCustomerLineTemplate = LoadTemplateData(CREDITNOTE_CUSTOMER_LINE);
                invoiceLineTemplate = LoadTemplateData(INVOICE_TRANSACTION_LINE);
                //creditNoteVendorTemplate = LoadTemplateData(CREDITNOTE_VENDOR_LINE);
                invoiceAmountDetailsTemplate = LoadTemplateData(INVOICE_AMOUNT_DETAILS);
                //creditNoteDetailsTemplate = LoadTemplateData(CREDITNOTE_DETAILS_LINE);
                invoicePaymentTemplate = LoadTemplateData(INVOICE_PAYMENT_LINE);
            }
            catch (Exception ex)
            {
                log.Error("Set Invoice Templates Error", ex);
                throw new Exception(MessageContainerList.GetMessage(executionContext, 4219) + " : " + ex.Message); //Set Invoice Templates Error
            }
            log.LogMethodExit();
        }

        private List<ValidationError> ValidateTransactionData(Transaction.Transaction trx, string invoiceType)
        {
            log.LogMethodEntry("trx", invoiceType);
            //invoiceType = "Boleta";
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
            log.LogMethodExit(errorList);
            return errorList;
        }
        private string LoadTemplateData(string templateSection)
        {
            log.LogMethodEntry(templateSection);
            string template = string.Empty;
            try
            {
                string strExeFilePath = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                strExeFilePath += "\\Resources\\ChileTemplates\\";
                if (Directory.Exists(strExeFilePath))
                {
                    switch (templateSection)
                    {
                        case BOLETA_HEADER:
                            {
                                template = File.ReadAllText(strExeFilePath + "Chile_Header.txt");
                                break;
                            }
                        //case "CREDITNOTE_HEADER":
                        //    {
                        //        template = File.ReadAllText(strExeFilePath + "CNHeader.txt");
                        //        break;
                        //    }
                        case INVOICE_DETAILS_LINE:
                            {
                                template = File.ReadAllText(strExeFilePath + "Chile_Details.txt");
                                break;
                            }
                        //case "CREDITNOTE_CUSTOMER_LINE":
                        //    {
                        //        template = File.ReadAllText(strExeFilePath + "CNClient.txt");
                        //        break;
                        //    }
                        case INVOICE_TRANSACTION_LINE:
                            {
                                template = File.ReadAllText(strExeFilePath + "Chile_Line_Details.txt");
                                break;
                            }
                        //case "CREDITNOTE_VENDOR_LINE":
                        //    {
                        //        template = File.ReadAllText(strExeFilePath + "CNVendor.txt");
                        //        break;
                        //    }
                        case INVOICE_AMOUNT_DETAILS:
                            {
                                template = File.ReadAllText(strExeFilePath + "Chile_Amount_Details.txt");
                                break;
                            }
                        //case "CREDITNOTE_DETAILS_LINE":
                        //    {
                        //        template = File.ReadAllText(strExeFilePath + "CNDetails.txt");
                        //        break;
                        //    }
                        case INVOICE_PAYMENT_LINE:
                            {
                                template = File.ReadAllText(strExeFilePath + "Chile_Payment.txt");
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
        private void BuildInvoiceAttributes()
        {
            log.LogMethodEntry();
            try
            {
                //Invoice_Authentication_Key
                LookupValuesContainerDTO lookupValuesInvoiceAuthKeyDTO = invoiceSetupLookupValueDTOList.FirstOrDefault(p => p.LookupValue == "Invoice_Authentication_Key");
                if (lookupValuesInvoiceAuthKeyDTO != null)
                {
                    apiAuthId = lookupValuesInvoiceAuthKeyDTO.Description;
                }
                log.LogVariableState("apiAuthId", apiAuthId);
                //Suplier RUT
                supplierRUT = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, STORETAXIDNUMBER);
                log.LogVariableState("supplierRUT", supplierRUT);
                //Factura
                LookupValuesContainerDTO lookupValuesDocTypeDTO = invoiceSetupLookupValueDTOList.FirstOrDefault(p => p.LookupValue == "DocumentType");
                if (lookupValuesDocTypeDTO != null)
                {
                    string[] values = lookupValuesDocTypeDTO.Description.Split('|');
                    boletaDocType = values[0];
                    creditNoteDocType = values[1];
                }
                log.LogVariableState("boletaDocType", boletaDocType);
                log.LogVariableState("creditNoteDocType", creditNoteDocType);
                //FiscalNumber
                LookupValuesContainerDTO lookupValuesFiscalNumDTO = invoiceSetupLookupValueDTOList.FirstOrDefault(p => p.LookupValue == "FiscalNumber");
                if (lookupValuesFiscalNumDTO != null)
                {
                    fiscalNumber = lookupValuesFiscalNumDTO.Description;
                }
                log.LogVariableState("fiscalNumber", fiscalNumber);
                //BoletaAndDNIValues
                LookupValuesContainerDTO lookupValuesServiceIndicatorDTO = invoiceSetupLookupValueDTOList.FirstOrDefault(p => p.LookupValue == "BoletaAndDNIValues");
                if (lookupValuesServiceIndicatorDTO != null)
                {
                    string[] values = lookupValuesServiceIndicatorDTO.Description.Split('|');
                    serviceIndicator = values[0];
                }
                log.LogVariableState("serviceIndicator", serviceIndicator);
                //AccountingCPId
                LookupValuesContainerDTO lookupValuesAccountingCPId = invoiceAttributesValuesDTOList.FirstOrDefault(p => p.LookupValue == "AccountingCPId");
                if (lookupValuesAccountingCPId != null)
                {
                    genericCustomerRUTId = lookupValuesAccountingCPId.Description;
                }
                log.LogVariableState("genericCustomerRUTId", genericCustomerRUTId);
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
                //Currency_Code
                LookupValuesContainerDTO lookupValuesCurrencyInfoDTO = invoiceSetupLookupValueDTOList.FirstOrDefault(p => p.LookupValue == "Currency_Code");
                if (lookupValuesCurrencyInfoDTO != null)
                {
                    currencyCode = lookupValuesCurrencyInfoDTO.Description;
                    if (string.IsNullOrWhiteSpace(currencyCode))
                    {
                        currencyCode = "CLP";
                    }
                }
                log.LogVariableState("currencyCode", currencyCode);
                invC = (useCulture ? new System.Globalization.CultureInfo(cultureInfo) : CultureInfo.InvariantCulture);
                //Boleta_File_Upload_Path
                LookupValuesContainerDTO lookupValuesBoletaFileUploadPathDTO = invoiceSetupLookupValueDTOList.FirstOrDefault(p => p.LookupValue == "Boleta_File_Upload_Path");
                if (lookupValuesBoletaFileUploadPathDTO != null)
                {
                    boleta_File_Upload_Path = lookupValuesBoletaFileUploadPathDTO.Description;
                }
                log.LogVariableState("boleta_File_Upload_Path", boleta_File_Upload_Path);
                //CreditNote_File_Upload_Path
                LookupValuesContainerDTO lookupValuesCreditNoteFileUploadPathDTO = invoiceSetupLookupValueDTOList.FirstOrDefault(p => p.LookupValue == "CreditNote_File_Upload_Path");
                if (lookupValuesCreditNoteFileUploadPathDTO != null)
                {
                    creditNote_File_Upload_Path = lookupValuesCreditNoteFileUploadPathDTO.Description;
                }
                log.LogVariableState("creditNote_File_Upload_Path", creditNote_File_Upload_Path);
            }
            catch (Exception ex)
            {
                log.Error("Error in Build Invoice Attributes", ex);
                throw new Exception(MessageContainerList.GetMessage(executionContext, 4218) + " : " + ex.Message);//Error in Build Invoice Attributes
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
        private void SetTrxOrderDetails(Transaction.Transaction trx)
        {
            log.LogMethodEntry(trx.Trx_id);
            waiterName = string.Empty;
            orderDescription = string.Empty;
            customerCount = string.Empty;
            posIdSiteName = string.Empty;
            cashierName = string.Empty;
            posDecription = string.Empty;
            transactionId = string.Empty;
            transactionNo = string.Empty;

            if (trx.Order != null && trx.Order.OrderHeaderDTO != null)
            {
                waiterName = DoSubstring(trx.Order.OrderHeaderDTO.WaiterName, 70);
                orderDescription = DoSubstring(trx.Order.OrderHeaderDTO.TableNumber, 20);
                customerCount = (trx.Order.OrderHeaderDTO.GuestCount <= 0 ? "1" : trx.Order.OrderHeaderDTO.GuestCount.ToString());
            }
            int siteId = executionContext.GetSitePKId();
            SiteContainerDTO siteDTO = SiteContainerList.GetCurrentSiteContainerDTO(siteId);
            string siteName = string.Empty;
            if (siteDTO != null)
            {
                siteName = siteDTO.SiteName;
            }
            posIdSiteName = +utilities.ParafaitEnv.POSMachineId + "-" + siteName;
            cashierName = DoSubstring(trx.Username, 20);
            posDecription = DoSubstring(trx.POSMachine, 70);
            transactionId = trx.Trx_id.ToString();
            transactionNo = trx.Trx_No;
            log.LogMethodExit();
        }
        private void BuildProfileDTO(Transaction.Transaction trx)
        {
            log.LogMethodEntry(trx.Trx_id);
            try
            {
                customerRUTId = genericCustomerRUTId;
                customerName = string.Empty;
                log.LogVariableState("BuildProfileDTO trx.customerDTO.Id", (trx.customerDTO != null ? trx.customerDTO.Id : -2));
                log.LogVariableState("BuildProfileDTO trx.customerDTO.ProfileDTO.Id", (trx.customerDTO != null && trx.customerDTO.ProfileDTO != null ? trx.customerDTO.ProfileDTO.Id : -3));
                if (trx.customerDTO != null && trx.customerDTO.Id > -1 && trx.customerDTO.ProfileDTO != null && trx.customerDTO.ProfileDTO.Id > -1)
                {
                    customerName = (string.IsNullOrWhiteSpace(trx.customerDTO.ProfileDTO.FirstName) ? string.Empty : trx.customerDTO.ProfileDTO.FirstName) +
                                       (string.IsNullOrWhiteSpace(trx.customerDTO.ProfileDTO.MiddleName) ? string.Empty : " " + trx.customerDTO.ProfileDTO.MiddleName) +
                                       (string.IsNullOrWhiteSpace(trx.customerDTO.ProfileDTO.LastName) ? string.Empty : " " + trx.customerDTO.ProfileDTO.LastName);
                    customerName = DoSubstring(customerName, 100);
                    if (trx.customerDTO != null && string.IsNullOrWhiteSpace(trx.customerDTO.TaxCode) == false)
                    {
                        customerRUTId = trx.customerDTO.TaxCode;
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
        private void SetInvoiceTypeAttributes(string invoicType)
        {
            log.LogMethodEntry(invoicType);
            invoiceTypeCode = string.Empty;
            fileUploadPath = string.Empty;
            if (string.IsNullOrWhiteSpace(invoicType))
            {
                string msg = MessageContainerList.GetMessage(executionContext, "Invoice type is not set for the transaction. Cannot proceed.");
                throw new ValidationException(msg);
            }
            if (invoicType == "Boleta")
            {
                invoiceTypeCode = "BOL";
                fileUploadPath = boleta_File_Upload_Path;
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
        private List<TransactionDTO> GetEligibleInvoiceRecords(DateTime currentTime, DateTime lastRunTime)
        {
            log.LogMethodEntry(currentTime, lastRunTime);
            List<TransactionDTO> transactionDTOList = new List<TransactionDTO>();
            try
            {
                List<LookupValuesContainerDTO> lookupValuesDTOList = GetEligibleOverrideOptions();
                if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
                {
                    string overrideOptions = lookupValuesDTOList[0].Description;
                    //lastRunTime = DateTime.Now.AddDays(-1);
                    TransactionListBL transactionListBL = new TransactionListBL(executionContext);
                    List<KeyValuePair<TransactionDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<TransactionDTO.SearchByParameters, string>>();
                    searchParams.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                    searchParams.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.LAST_UPDATE_FROM_TIME, lastRunTime.ToString("yyyy-MM-dd HH:mm:ss")));
                    searchParams.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.LAST_UPDATE_TO_TIME, currentTime.ToString("yyyy-MM-dd HH:mm:ss")));
                    searchParams.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.POS_OVERRIDE_OPTION_NAME_LIST, overrideOptions));
                    searchParams.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.STATUS, Transaction.Transaction.TrxStatus.CLOSED.ToString()));
                    transactionDTOList = transactionListBL.GetTransactionDTOList(searchParams, utilities, null, 0, 5000);
                    if (transactionDTOList != null && transactionDTOList.Any())
                    {
                        //remove already processed or reversal transactions
                        transactionDTOList = transactionDTOList.FindAll(pc => string.IsNullOrWhiteSpace(pc.ExternalSystemReference) == true && pc.OriginalTransactionId <= -1);
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
        private string GetPostURLForInvoice()
        {
            log.LogMethodEntry();
            string value = GetLookupValueDescription(POSTURLFORINVOICELOOKUPVALUENAME);
            log.LogMethodExit(value);
            return value;
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
        private List<LookupValuesContainerDTO> GetEligibleOverrideOptions()
        {
            log.LogMethodEntry();
            List<LookupValuesContainerDTO> lookupValuesContainerDTOList = LookupsContainerList.GetLookupsContainerDTO(executionContext.GetSiteId(), FISCAL_INVOICE_SETUP, executionContext).LookupValuesContainerDTOList;
            if (lookupValuesContainerDTOList != null && lookupValuesContainerDTOList.Any())
            {
                lookupValuesContainerDTOList = lookupValuesContainerDTOList.Where(lv => lv.LookupValue == ELIGIBLEOVERRIDEOPTIONS).ToList();
            }
            else
            {
                string msg = MessageContainerList.GetMessage(executionContext, 2932, ELIGIBLEOVERRIDEOPTIONS, FISCAL_INVOICE_SETUP);
                //&1 details are missing in &2 setup.
                ValidationException ve = new ValidationException(msg);
                throw ve;
            }
            log.LogMethodExit(lookupValuesContainerDTOList);
            return lookupValuesContainerDTOList;
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
        private decimal GetDiscountedTotalAmount(Transaction.Transaction transaction)
        {
            log.LogMethodEntry("transaction");
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
        protected override decimal GetDiscountedLinePrice(Transaction.Transaction.TransactionLine trxLine)
        {
            log.LogMethodEntry();
            decimal lineAmount = trxLine.TaxInclusivePrice == "Y" ? (decimal)trxLine.LineAmount : (decimal)trxLine.Price;//sending price as per product setup
            decimal discountAmount = 0;
            decimal discountPercentage = 0;
            if (trxLine.TransactionDiscountsDTOList != null && trxLine.TransactionDiscountsDTOList.Any())
            {
                foreach (TransactionDiscountsDTO td in trxLine.TransactionDiscountsDTOList)
                {
                    if (td.DiscountId > -1)
                    {
                        DiscountContainerDTO discountContainerDTO = DiscountContainerList.GetDiscountContainerDTOOrDefault(executionContext, td.DiscountId);
                        if (discountContainerDTO != null && discountContainerDTO.DiscountAmount > 0)
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
                double linePrice = trxLine.TaxInclusivePrice == "Y" ? trxLine.LineAmount : trxLine.Price;//sending price as per product setup
                lineAmount = Convert.ToDecimal(linePrice) - ((Convert.ToDecimal(linePrice)) * (discountPercentage) / 100) - finalFlatDiscountAmount;
            }
            log.LogMethodExit(lineAmount);
            return lineAmount;
        }

        private bool GetHasError(string result)
        {
            log.LogMethodEntry(result);
            bool hasError = true;
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(result);
                XmlNodeList errList = doc.SelectNodes("iDTE/Response/HasError");
                foreach (XmlNode node in errList)
                {
                    hasError = node.InnerText.ToLower() == "true" ? true : false;
                    break;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit(hasError);
            return hasError;
        }


        private string GetErrorDetails(string result)
        {
            log.LogMethodEntry(result);
            string errorDetails = result; //pass result if unable to find error details tag
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(result);
                XmlNodeList detailList = doc.SelectNodes("iDTE/Response/DetailError");
                foreach (XmlNode node in detailList)
                {
                    errorDetails = node.InnerText;//fetching error details
                    break;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit(errorDetails);
            return errorDetails;
        }

    }
}
