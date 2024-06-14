/********************************************************************************************
* Project Name - PeruFiscalization
* Description  - Class for PeruFiscalization 
* 
**************
**Version Log
**************
*Version     Date              Modified By        Remarks          
*********************************************************************************************
*2.150.5.0     20-Jun-2023       Guru S A           Created 
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
using Semnox.Parafait.POS;
using System.Xml;
using Semnox.Parafait.Discounts;

namespace Semnox.Parafait.Fiscalization
{
    public class PeruFiscalization : ParafaitFiscalization
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string peruResultString = string.Empty;
        private string boletaHeaderTemplate = string.Empty;
        private string facturaHeaderTemplate = string.Empty;
        private string invoiceLineTemplate = string.Empty;
        private string invoiceLegalTemplate = string.Empty;
        private string invoiceTaxTotalSubTotalTemplate = string.Empty;
        private string invoiceTaxTotalTemplate = string.Empty;
        private string invoiceDiscountTemplate = string.Empty;
        private string invoiceDiscountTotalTemplate = string.Empty;

        private string creditNoteHeaderTemplate = string.Empty;
        private string creditNoteLineTemplate = string.Empty;
        private string creditNoteLegalTemplate = string.Empty;
        private string creditNoteTaxTotalTemplate = string.Empty;

        private string uBLVersionID = string.Empty;
        private string customizationID = string.Empty;
        private string customizationAgencyName = string.Empty;
        private string boletaValue = string.Empty;
        private string facturaValue = string.Empty;
        private string creditNoteValue = string.Empty;
        private string dniValue = string.Empty;
        private string rucValue = string.Empty;

        private string dateFormat = string.Empty;
        private string timeFormat = string.Empty;
        private string amountFormat = string.Empty;
        private string integerFormat = string.Empty;
        private string cultureInfo = string.Empty;
        private bool useCulture = false;
        private System.Globalization.CultureInfo invC = null;

        private string invoiceTypeCode = string.Empty;
        private string currencyCode = string.Empty;
        private int trxLineCount = 1;
        private string signatureId = string.Empty;
        private string signatureIDSchmeId = string.Empty;
        private string signatureIDSchmeName = string.Empty;
        private string signatureIDSchmeAgency = string.Empty;
        private string signatureIDSchmeURI = string.Empty;
        private string rucNumber = string.Empty;
        private string externalReferenceUri = string.Empty;
        private string accountingSPName = string.Empty;
        private string accountingSPEmailId = string.Empty;
        private string accountingSPRegistrationName = string.Empty;
        private string accountingSPRegistrationAddressId = string.Empty;
        private string accountingSPRegistrationIDSchemeAgency = string.Empty;
        private string accountingSPRegistrationIDSchemeName = string.Empty;
        private string accountingSPLine = string.Empty;
        private string accountingSPIdentificationCode = string.Empty;
        private string accountingSPCountryCodeListId = string.Empty;
        private string accountingSPCountryCodeListAgency = string.Empty;
        private string accountingSPCountryCodeListName = string.Empty;
        private string accountingSPId = string.Empty;
        private string accountingSPDistrict = string.Empty;
        private string accountingSPCountrySubentity = string.Empty;
        private string accountingSPCitySubdivisionName = string.Empty;
        private string accountingSPCityName = string.Empty;
        private string addressTypeCode = string.Empty;
        private string siteName = string.Empty;
        private string accountingCPId = string.Empty;
        private string acpIdentifierContent = string.Empty;

        private string acpIdentificationSchemeID = string.Empty;
        private string accountingCPName = string.Empty;
        private string accountingCPRegistraionId = string.Empty;
        private string accountingCPCitySubdivisionName = string.Empty;
        private string accountingCPCityName = string.Empty;
        private string accountingCPCountrySubentity = string.Empty;
        private string accountingCPDistrict = string.Empty;
        private string accountingCPAddressLine = string.Empty;
        private string accountingCPIdentificationCode = string.Empty;
        private string accountingCPCountryCodeListId = string.Empty;
        private string accountingCPCountryCodeListAgency = string.Empty;
        private string accountingCPCountryCodeListName = string.Empty;
        private string accountingCPMailId = string.Empty;
        private string accountingCPDNI = string.Empty;
        private string accountingCPRUC = string.Empty;
        private string discrepancyResponseCode = string.Empty;
        private string taxSchemeId = string.Empty;
        private string taxTypeCode = string.Empty;
        private string taxSchemeName = string.Empty;
        private string taxExemptionReasonCodeValue = string.Empty;
        private string zeroTaxSchemeId = string.Empty;
        private string zeroTaxTypeCode = string.Empty;
        private string zeroTaxSchemeName = string.Empty;
        private string billingReferenceID = string.Empty;
        private string billingReferenceIssueDate = string.Empty;
        private string billingReferenceDocumentTypeCode = string.Empty;
        private string discrepancyResponseCodeDescription = string.Empty;
        private string pricingReferencePriceTypeCodeCodeContent = string.Empty;

        private string boleta_File_Upload_Path = string.Empty;
        private string factura_File_Upload_Path = string.Empty;
        private string creditNote_File_Upload_Path = string.Empty;
        private string fileUploadPath = string.Empty;

        private string allowanceChargeIndicator = string.Empty;
        private string allowanceChargeReasonCodeValue = string.Empty;

        private decimal taxableAmountBeforeDiscountValue;
        private decimal taxableAmountAfterDiscountValue;
        private decimal taxAmountValue;

        private Dictionary<string, string> invoiceTypeCodeAttributeList = new Dictionary<string, string>();
        private Dictionary<string, string> documentCurrencyCodeAttributeList = new Dictionary<string, string>();
        private Dictionary<string, string> accountingCPList = new Dictionary<string, string>();
        private Dictionary<string, string> accountingSPList = new Dictionary<string, string>();
        private Dictionary<string, string> accountingSPAddressTypeCodeList = new Dictionary<string, string>();
        private Dictionary<string, string> allowanceChargeReasonCodeList = new Dictionary<string, string>();
        private Dictionary<string, string> documentCurrencyCodeList = new Dictionary<string, string>();
        private Dictionary<string, string> invoicedQuantityList = new Dictionary<string, string>();
        private Dictionary<string, string> invoiceTypeCodeList = new Dictionary<string, string>();
        private Dictionary<string, string> itemClassificationCodeList = new Dictionary<string, string>();
        private Dictionary<string, string> priceTypeCodeList = new Dictionary<string, string>();
        private Dictionary<string, string> taxExemptionReasonCodeList = new Dictionary<string, string>();
        private Dictionary<string, string> taxSchemeList = new Dictionary<string, string>();
        private Dictionary<string, string> zeroTaxSchemeList = new Dictionary<string, string>();
        private Dictionary<string, string> documentTypeCodeList = new Dictionary<string, string>();
        private Dictionary<string, string> DiscrepancyResponseCodeList = new Dictionary<string, string>();

        private const string PERU_BOLETA_HEADER = "PERU_BOLETA_HEADER";
        private const string PERU_FACTURA_HEADER = "PERU_FACTURA_HEADER";
        private const string PERU_INVOICE_LINE = "PERU_INVOICE_LINE";
        private const string PERU_INVOICE_TAX_TOTAL = "PERU_INVOICE_TAX_TOTAL";
        private const string PERU_TAX_TOTAL_SUBTOTAL = "PERU_TAX_TOTAL_SUBTOTAL";
        private const string PERU_INVOICE_LEGAL = "PERU_INVOICE_LEGAL";
        private const string PERU_INVOICE_DISCOUNT = "PERU_INVOICE_DISCOUNT";
        private const string PERU_INVOICE_DISCOUNT_TOTAL = "PERU_INVOICE_DISCOUNT_TOTAL";

        private const string ELIGIBLEOVERRIDEOPTIONS = "EligibleOverrideOptions";

        private const string PERU_CREDIT_NOTE_HEADER = "PERU_CREDIT_NOTE_HEADER";
        private const string PERU_CREDIT_NOTE_LINE = "PERU_CREDIT_NOTE_LINE";
        private const string PERU_CREDIT_NOTE_TAX_TOTAL = "PERU_CREDIT_NOTE_TAX_TOTAL";
        private const string PERU_CREDIT_NOTE_LEGAL = "PERU_CREDIT_NOTE_LEGAL";
        private const string JSON_FILE_EXTENSION = ".json";
        private const string XML_FILE_EXTENSION = ".xml";
        private const string TXT_FILE_EXTENSION = ".txt";

        private const string BOLETA = "Boleta";
        private const string FACTURA = "Factura";
        private const string CreditNOTES = "CreditNotes";

        private const string FISCAL_INVOICE_SETUP = "FISCAL_INVOICE_SETUP";
        private const string FISCAL_INVOICE_ATTRIBUTES = "FISCAL_INVOICE_ATTRIBUTES";
        private const string EX_SYS_NAME = "PeruFiscalization";
        private const string TRANSACTION = "Transaction";
        private const string POST_INVOICE = "PostInvoice";
        private const string ZERO_PRICE_TRX = "ZEROPRICETRX";
        private const string HUNDRED_PERC_DISCOUNT_TRX = "100PERCDISCOUNTTRX";


        private const string OUT_PATH_PARTIAL_STRING = "Out_File_Path";
        private const string MOVE_PATH_STRING = "Move_Posted_Files_Path";

        private const string USERNAMELOOKUPVALUENAME = "TAX_IDENTIFICATION_NUMBER";
        private const string POSTURLFORINVOICELOOKUPVALUENAME = "Post_URL_For_Invoice";
        private const string USERPASSWORDLOOKUPVALUENAME = "UserPassword";
        private const string POSTURLFORTOKENLOOKUPVALUENAME = "Post_URL_For_Token";
        private const string INVOICEAUTHKEYLOOKUPVALUENAME = "Invoice_Authentication_Key";

        private string userName;
        private string password;
        private string postURLForInvoice;
        private string peruAuthKey;
        private string postURLForToken;
        private string moveFolderPath;
        private List<LookupValuesContainerDTO> invoiceSetupLookupValueDTOList;
        private List<LookupValuesContainerDTO> invoiceAttributesValuesDTOList;

        /// <summary>
        /// Peru Fiscalization
        /// </summary>
        /// <param name="executionContext"></param>
        public PeruFiscalization(ExecutionContext executionContext, Utilities utilities)
            : base(executionContext, utilities)
        {
            log.LogMethodEntry(executionContext, "utilities");
            this.exSysName = EX_SYS_NAME;
            this.postFileFormat = XMLFORMAT;
            SetLookUpValueList();
            moveFolderPath = GetMoveFolderPath(invoiceSetupLookupValueDTOList);
            BuildPeruAttributes();
            userName = GetUserName();
            password = GetUserPassword();
            peruAuthKey = GetPeruAuthenticationKey();
            postURLForToken = GetPostURLForToken();
            postURLForInvoice = GetPostURLForInvoice();
            log.LogMethodExit();
        }

        /// <summary>
        /// Create Invoice Files
        /// </summary>
        public override List<ExSysSynchLogDTO> CreateInvoice(DateTime currentTime, DateTime lastRunTime)
        {
            log.LogMethodEntry(currentTime, lastRunTime);
            List<ExSysSynchLogDTO> logDataList = new List<ExSysSynchLogDTO>();
            try
            {
                List<TransactionDTO> transactionDTOList = null;
                SetInvoiceTemplates();
                SetCreditNoteTemplates();
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
                                logDataList.AddRange(trxLogList);
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Error(MessageContainerList.GetMessage(executionContext, 4206), ex); //Error in Process InvoiceJson Records 
                            string msg = "Unexpected error in CreateInvoice: ";
                            ExSysSynchLogDTO logDTO = LogError(ex, msg, TRANSACTION, transactionDTO.TransactionId, transactionDTO.Guid);
                            if (logDTO != null)
                            {
                                logDataList.Add(logDTO);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(MessageContainerList.GetMessage(executionContext, 4206), ex); //Error in Process InvoiceJson Records
                string msg = "Unexpected error in CreateInvoice: ";
                ExSysSynchLogDTO logDTO = LogError(ex, msg, TRANSACTION);
                if (logDTO != null)
                {
                    logDataList.Add(logDTO);
                }
                //throw new Exception(msg);
            }
            log.LogMethodExit(logDataList);
            return logDataList;
        }

        /// <summary>
        /// Create Invoice File
        /// </summary> 
        public override List<ExSysSynchLogDTO> CreateInvoice(int trxId, out string fileName)
        {
            log.LogMethodEntry(trxId);
            List<ExSysSynchLogDTO> logList = new List<ExSysSynchLogDTO>();
            string trxGuid = string.Empty;
            fileName = string.Empty;
            TransactionDTO trxDTO = null;
            try
            {
                TransactionUtils transactionUtils = new TransactionUtils(utilities);
                Transaction.Transaction trx = transactionUtils.CreateTransactionFromDB(trxId, utilities);
                trxGuid = trx.TrxGuid;
                string result = string.Empty;
                string invoiceType = GetInvoiceType(trx);

                List<ValidationError> validationErrorList = ValidateTransactionData(trx, invoiceType);
                if (validationErrorList != null && validationErrorList.Any())
                {
                    ValidationException vEx = new ValidationException("Transaction Validation Failed", validationErrorList);
                    log.Error("Validation failed for " + trx.Trx_id, vEx);
                    throw vEx;
                }
                BuildProfileDTO(trx);
                bool isReversal = false;
                decimal discountTotal = 0;
                try
                {
                    if (invoiceType == BOLETA || invoiceType == FACTURA)
                    {
                        SetInvoiceTypeAttributes(invoiceType);
                        if (trx.Transaction_Amount == 0)
                        {   //skip zero price transaction
                            trxDTO = trx.TransactionDTO;
                            string errMsg = ZERO_PRICE_TRX + "_" + trxId;
                            ValidationException ve = new ValidationException(errMsg);
                            throw ve;
                        }
                        CalculateTaxDetails(trx);
                        if (taxableAmountAfterDiscountValue == 0)
                        {   //skip zero price transaction
                            trxDTO = trx.TransactionDTO;
                            string errMsg = HUNDRED_PERC_DISCOUNT_TRX + "_" + trxId;
                            ValidationException ve = new ValidationException(errMsg);
                            throw ve;
                        }
                        string headerString = BuildHeaderSection(trx, invoiceType, isReversal);
                        string lineDetailsElement = BuildTaxAndLegalSummaryDetailSection(trx, invoiceType, isReversal);
                        result = headerString + lineDetailsElement;
                        string ivoiceLineElement = "\"InvoiceLine\"" + " : [";
                        string invoiceResultLine = " "; 
                        foreach (Transaction.Transaction.TransactionLine trxLine in trx.TransactionLineList)
                        {
                            if (trxLine.LineAmount != 0) //skip line if amount is zero
                            {
                                decimal lineAmountAfterDiscount = Convert.ToDecimal(trxLine.Price); ;
                                if (trxLine.TransactionDiscountsDTOList != null && trxLine.TransactionDiscountsDTOList.Any())
                                {
                                    lineAmountAfterDiscount = GetLinePrice(trxLine);
                                }
                                if (lineAmountAfterDiscount != 0) //skip 100% discount line
                                {
                                    invoiceResultLine = invoiceResultLine + BuildInvoiceLineSection(trxLine);
                                    invoiceResultLine = invoiceResultLine.Replace("@DiscountSection", BuildLineDiscountValues(trxLine));
                                    if (trxLine.TransactionDiscountsDTOList != null && trxLine.TransactionDiscountsDTOList.Any())
                                    {
                                        decimal lineDiscountAmount = GetLineDiscountAmount(trxLine);
                                        discountTotal = discountTotal + lineDiscountAmount;
                                    }
                                    invoiceResultLine = invoiceResultLine + ",";
                                }
                            }
                        }
                        invoiceResultLine = invoiceResultLine.Substring(0, invoiceResultLine.Length - 1);
                        result = result.Replace("@DiscountTotalSection", BuildLineDiscountTotalValues(discountTotal));
                        result = result + ivoiceLineElement + invoiceResultLine;
                        result = result + "]}]}";
                        fileName = fileUploadPath + rucNumber + "-" + invoiceTypeCode + "-" + trx.Trx_No + JSON_FILE_EXTENSION;
                        System.IO.File.WriteAllText(fileName, result);
                        log.LogVariableState("Json save completed", fileName);
                    }
                    else
                    {
                        isReversal = true;
                        Transaction.Transaction originalTrx = GetOriginalTrx(trx);
                        if (originalTrx.Transaction_Amount == 0)
                        {
                            //skip zero price transaction
                            trxDTO = trx.TransactionDTO;
                            string errMsg = ZERO_PRICE_TRX + "_" + trxId;
                            ValidationException ve = new ValidationException(errMsg);
                            throw ve;
                        }
                        CalculateTaxDetails(originalTrx);
                        if (taxableAmountAfterDiscountValue == 0)
                        {   //skip zero price transaction
                            trxDTO = trx.TransactionDTO;
                            string errMsg = HUNDRED_PERC_DISCOUNT_TRX + "_" + trxId;
                            ValidationException ve = new ValidationException(errMsg);
                            throw ve;
                        }
                        invoiceType = GetInvoiceType(trx.OriginalTrxId);
                        SetInvoiceTypeAttributes(invoiceType);
                        fileUploadPath = creditNote_File_Upload_Path;
                        string headerString = BuildHeaderSection(trx, invoiceType, isReversal);
                        string lineDetailsElement = BuildTaxAndLegalSummaryDetailSection(trx, invoiceType, isReversal);
                        result = headerString + lineDetailsElement;
                        string creditNoteElement = "\"CreditNoteLine\"" + " : [";
                        string creditResultLine = " "; 
                        if (originalTrx != null && originalTrx.TransactionLineList != null && originalTrx.TransactionLineList.Any())
                        {
                            foreach (Transaction.Transaction.TransactionLine trxLine in originalTrx.TransactionLineList)
                            {
                                if (trxLine.LineAmount != 0) //skip line if amount is zero
                                {
                                    decimal lineAmountAfterDiscount = Convert.ToDecimal(trxLine.Price);
                                    if (trxLine.TransactionDiscountsDTOList != null && trxLine.TransactionDiscountsDTOList.Any())
                                    {
                                        lineAmountAfterDiscount = GetLinePrice(trxLine);
                                    }
                                    if (lineAmountAfterDiscount != 0) //skip 100% discount line
                                    {
                                        creditResultLine = creditResultLine + BuildCreditNoteLineSection(trxLine);
                                        creditResultLine = creditResultLine + ",";
                                    }
                                }
                            }
                        }
                        creditResultLine = creditResultLine.Substring(0, creditResultLine.Length - 1);
                        result = result + creditNoteElement + creditResultLine;
                        result = result + "]}]}";
                        fileName = fileUploadPath + rucNumber + "-" + creditNoteValue + "-" + trx.Trx_No + JSON_FILE_EXTENSION;
                        System.IO.File.WriteAllText(fileName, result);
                        log.LogVariableState("Json save completed", fileName);
                    }
                    ExSysSynchLogDTO exSysSynchLogDTO = new ExSysSynchLogDTO(logId: -1, timestamp: ServerDateTime.Now, exSysName: EX_SYS_NAME,
                                   parafaitObject: TRANSACTION, parafaitObjectId: trx.Trx_id, parafaitObjectGuid: trx.TrxGuid,
                                  isSuccessFul: true, status: SUCCESS_STATUS, data: "File created: " + fileName, remarks: this.programName, concurrentRequestId: this.concurrentRequestId);
                    ExSysSynchLogBL exSysSynchLogBL = new ExSysSynchLogBL(executionContext, exSysSynchLogDTO);
                    exSysSynchLogBL.Save();
                    logList.Add(exSysSynchLogBL.ExSysSynchLogDTO);
                }
                catch (Exception ex)
                {
                    string zeroTrxExternalRef = ZERO_PRICE_TRX + "_" + trxId;
                    string hunPerTrxExternalRef = HUNDRED_PERC_DISCOUNT_TRX + "_" + trxId;
                    if (ex.Message != null
                        && (ex.Message.Trim() == zeroTrxExternalRef.Trim()
                          || ex.Message.Trim() == hunPerTrxExternalRef.Trim()) && trxDTO != null)
                    {
                        //zero price transaction. Update external ref and record info
                        //string zeroTrxExternalRef = ZERO_PRICE_TRX + "_" + trxId;
                        UpdateTrxExternalSystemRefNumber(trxDTO, (ex.Message.Trim() == zeroTrxExternalRef.Trim() ? zeroTrxExternalRef : hunPerTrxExternalRef));
                        ExSysSynchLogDTO exSysSynchLogDTO = new ExSysSynchLogDTO(logId: -1, timestamp: ServerDateTime.Now, exSysName: EX_SYS_NAME,
                                   parafaitObject: TRANSACTION, parafaitObjectId: trx.Trx_id, parafaitObjectGuid: trx.TrxGuid,
                                  isSuccessFul: true, status: SUCCESS_STATUS, data: "Zero Price Transaction, skipping file generation", remarks: this.programName, concurrentRequestId: this.concurrentRequestId);
                        ExSysSynchLogBL exSysSynchLogBL = new ExSysSynchLogBL(executionContext, exSysSynchLogDTO);
                        exSysSynchLogBL.Save();
                        logList.Add(exSysSynchLogBL.ExSysSynchLogDTO);
                    }
                    else
                    {
                        throw;
                    }
                }
            } 
            catch (Exception ex)
            {
                log.Error("Create Invoice Error", ex);
                string msg = "Unexpected error in CreateInvoice: ";
                ExSysSynchLogDTO logDTO = LogError(ex, msg, TRANSACTION, trxId, trxGuid);
                if (logDTO != null)
                {
                    logList.Add(logDTO);
                }
            }
            log.LogMethodExit(logList);
            return logList;
        }

        /// <summary>
        /// Recreate Invoice Files
        /// </summary>  
        /// <returns></returns>
        public override List<ExSysSynchLogDTO> ReprocessInvoice(List<int> trxIdList)
        {
            log.LogMethodEntry(trxIdList);
            List<ExSysSynchLogDTO> logList = new List<ExSysSynchLogDTO>();
            try
            {
                List<TransactionDTO> transactionDTOList = null;
                SetInvoiceTemplates();
                SetCreditNoteTemplates();
                transactionDTOList = GetEligibleInvoiceRecords(trxIdList);
                if (transactionDTOList != null && transactionDTOList.Any())
                {
                    foreach (TransactionDTO transactionDTO in transactionDTOList)
                    {
                        try
                        {
                            string fileName = string.Empty;
                            List<ExSysSynchLogDTO> trxLogList = CreateInvoice(transactionDTO.TransactionId, out fileName);
                            if (trxLogList != null)
                            {
                                logList.AddRange(trxLogList);
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Error(MessageContainerList.GetMessage(executionContext, 4206), ex); //Error in Process InvoiceJson Records 
                            string msg = "Unexpected error in ReCreateInvoice: ";
                            ExSysSynchLogDTO logDTO = LogError(ex, msg, TRANSACTION, transactionDTO.TransactionId, transactionDTO.Guid);
                            if (logDTO != null)
                            {
                                logList.Add(logDTO);
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
                    logList.Add(logDTO);
                }
            }
            log.LogMethodExit(logList);
            return logList;
        }
        //<summary>
        //Post Invoice Files
        //</summary>
        public override List<ExSysSynchLogDTO> PostInvoice(DateTime currentTime, DateTime lastRunTime)
        {
            log.LogMethodEntry(currentTime, lastRunTime);
            List<ExSysSynchLogDTO> logList = new List<ExSysSynchLogDTO>();
            try
            {
                setMoveFolderPath();
                List<LookupValuesContainerDTO> lookupValuesDTOList = GetOutFolderPathInfo();
                if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
                {
                    for (int i = 0; i < lookupValuesDTOList.Count; i++)
                    {
                        if (string.IsNullOrWhiteSpace(lookupValuesDTOList[i].Description) == false)
                        {
                            VerifyFolderPath(lookupValuesDTOList[i].Description);
                            List<ExSysSynchLogDTO> folderFileLogList = PostFiles(lookupValuesDTOList[i].Description);
                            if (folderFileLogList != null && folderFileLogList.Any())
                            {
                                logList.AddRange(folderFileLogList);
                            }
                        }
                        else
                        {
                            string msg = MessageContainerList.GetMessage(executionContext, 4407, lookupValuesDTOList[i].LookupValue);
                            //Skipping xml posting for &1 as path is not defined in the setup
                            log.Error(msg);
                            ExSysSynchLogDTO exSysSynchLogDTO = new ExSysSynchLogDTO(logId: -1, timestamp: ServerDateTime.Now, exSysName: EX_SYS_NAME,
                              parafaitObject: POST_INVOICE, parafaitObjectId: -1, parafaitObjectGuid: null,
                             isSuccessFul: true, status: SUCCESS_STATUS, data: msg, remarks: this.programName, concurrentRequestId: this.concurrentRequestId);
                            ExSysSynchLogBL exSysSynchLogBL = new ExSysSynchLogBL(executionContext, exSysSynchLogDTO);
                            exSysSynchLogBL.Save();
                            logList.Add(exSysSynchLogBL.ExSysSynchLogDTO);
                        }
                    }
                }
                else
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2932, OUT_PATH_PARTIAL_STRING, FISCAL_INVOICE_SETUP));
                    //&1 details are missing in &2 setup 
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                string msg = "Error in PostInvoice: ";
                ExSysSynchLogDTO logDTO = LogError(ex, msg, POST_INVOICE);
                if (logDTO != null)
                {
                    logList.Add(logDTO);
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
                    logList.Add(logDTO);
                }
            }
            log.LogMethodExit(logList);
            return logList;
        }
        private List<ValidationError> ValidateTransactionData(Transaction.Transaction trx, string invoiceType)
        {
            log.LogMethodEntry("trx", invoiceType);
            List<ValidationError> errorList = new List<ValidationError>();
            bool isReversalTrx = trx.OriginalTrxId > -1 ? true : false;

            if (string.IsNullOrWhiteSpace(invoiceType) && isReversalTrx == false)
            {
                string msg = MessageContainerList.GetMessage(executionContext, "Invoice type is not set for the transaction. Cannot proceed.");
                ValidationError validationError = new ValidationError("Invoice", null, msg);
                errorList.Add(validationError);
            }
            log.LogVariableState("trx.customerDTO.Id", trx.customerDTO != null ? trx.customerDTO.Id : -2);
            log.LogVariableState("trx.customerDTO.ProfileDTO.Id", (trx.customerDTO != null && trx.customerDTO.ProfileDTO != null ? trx.customerDTO.ProfileDTO.Id : -3));
            if ((trx.customerDTO != null && trx.customerDTO.Id > -1 && trx.customerDTO.ProfileDTO != null && trx.customerDTO.ProfileDTO.Id > -1)
                == false)
            {
                ValidationError validationError = new ValidationError("Customer", null, MessageContainerList.GetMessage(executionContext, 1934));
                //Customer details are not passed
                errorList.Add(validationError);
            }

            log.LogMethodExit(errorList);
            return errorList;
        }

        private string BuildInvoiceLineSection(Transaction.Transaction.TransactionLine trxLine)
        {
            log.LogMethodEntry(trxLine);
            string str = invoiceLineTemplate;
            decimal lineExtensionAmountBeforeDiscount;
            decimal lineExtensionAmountAfterDiscount;
            decimal PricingReferenceAmountContent;
            decimal taxAmountAmount;
            decimal taxSubtotalTaxAmount;
            try
            {
                str = str.Replace("@ILID", (trxLine.DBLineId).ToString());
                str = str.Replace("@ILNote", "");//pending

                str = str.Replace("@ILInvoicedQuantityQuantityContent", (Convert.ToInt32(trxLine.quantity)).ToString(integerFormat, invC));
                if (invoicedQuantityList != null && invoicedQuantityList.Any())
                {
                    foreach (KeyValuePair<string, string> entry in invoicedQuantityList)
                    {
                        str = str.Replace(entry.Key.ToString(), entry.Value);
                    }
                }
                if (trxLine.TransactionDiscountsDTOList != null && trxLine.TransactionDiscountsDTOList.Any())
                {
                    lineExtensionAmountAfterDiscount = GetLinePrice(trxLine);
                }
                else
                {
                    lineExtensionAmountAfterDiscount = Convert.ToDecimal(trxLine.Price);
                }
                lineExtensionAmountBeforeDiscount = Convert.ToDecimal(trxLine.Price);
                str = str.Replace("@ILLineExtensionAmountAmountContent", lineExtensionAmountAfterDiscount.ToString(amountFormat, invC));
                str = str.Replace("@ILLineExtensionAmountAmountCurrencyID", currencyCode);
                PricingReferenceAmountContent = (Convert.ToDecimal(lineExtensionAmountAfterDiscount, invC) + ((Convert.ToDecimal(lineExtensionAmountAfterDiscount, invC) * Convert.ToDecimal((((decimal)trxLine.tax_percentage)), invC) / 100)));
                str = str.Replace("@ILPricingReferenceAlternativeConditionPricePriceAmountAmountContent", PricingReferenceAmountContent.ToString(amountFormat, invC));
                str = str.Replace("@ILPricingReferenceAlternativeConditionPricePriceAmountAmountCurrencyID", currencyCode);

                //string priceRefTypeCode = Math.Abs(lineExtensionAmountAfterDiscount) != 0 ? pricingReferencePriceTypeCodeCodeContent : "02";
                str = str.Replace("@ILPricingReferenceAlternativeConditionPricePriceTypeCodeCodeContent", pricingReferencePriceTypeCodeCodeContent);
                if (priceTypeCodeList != null && priceTypeCodeList.Any())
                {
                    foreach (KeyValuePair<string, string> entry in priceTypeCodeList)
                    {
                        str = str.Replace(entry.Key.ToString(), entry.Value);
                    }
                }
                //TaxTotal
                taxAmountAmount = (Convert.ToDecimal(lineExtensionAmountAfterDiscount, invC) * Convert.ToDecimal(((decimal)trxLine.tax_percentage).ToString(), invC) / 100);
                str = str.Replace("@ILTaxTotalTaxAmountAmountContent", taxAmountAmount.ToString(amountFormat, invC));
                str = str.Replace("@ILTaxTotalTaxAmountAmountCurrencyID", currencyCode);

                str = str.Replace("@ILTaxTotalTaxSubtotalTaxableAmountAmountContent", lineExtensionAmountAfterDiscount.ToString(amountFormat, invC));//(trxLine.Price).ToString());
                str = str.Replace("@ILTaxTotalTaxSubtotalTaxableAmountAmountCurrencyID", currencyCode);

                taxSubtotalTaxAmount = (Convert.ToDecimal(lineExtensionAmountAfterDiscount, invC) * Convert.ToDecimal(((decimal)trxLine.tax_percentage).ToString(amountFormat, invC), invC)) / 100;
                str = str.Replace("@ILTaxTotalTaxSubtotalTaxAmountAmountContent", taxSubtotalTaxAmount.ToString(amountFormat, invC));
                str = str.Replace("@ILTaxTotalTaxSubtotalTaxAmountAmountCurrencyID", currencyCode);

                str = str.Replace("@ILTaxTotalTaxSubtotalTaxCategoryPercentNumericContent", (trxLine.tax_percentage).ToString(amountFormat, invC));
                Dictionary<string, string> taxSchemeListValue = taxSchemeList;
                string taxSchemeIdValue = taxSchemeId;
                string taxSchemeNameValue = taxSchemeName;
                string taxTypeCodeValue = taxTypeCode;
                log.LogVariableState("lineExtensionAmountAfterDiscount", lineExtensionAmountAfterDiscount);
                
                log.LogVariableState("taxSchemeIdValue", taxSchemeIdValue);
                log.LogVariableState("taxSchemeNameValue", taxSchemeNameValue);
                log.LogVariableState("taxTypeCodeValue", taxTypeCodeValue);

                str = str.Replace("@TaxTotalTaxSubtotalTaxCategoryID", taxSchemeIdValue);

                if (taxSchemeListValue != null && taxSchemeListValue.Any())
                {
                    foreach (KeyValuePair<string, string> entry in taxSchemeListValue)
                    {
                        str = str.Replace(entry.Key.ToString(), entry.Value);
                    }
                }
                //string taxExmpReasonCode = Math.Abs(lineExtensionAmountBeforeDiscount) != 0 ? taxExemptionReasonCodeValue : "15";
                str = str.Replace("@ILTaxExemptionReasonCodeValue", taxExemptionReasonCodeValue);
                if (taxExemptionReasonCodeList != null && taxExemptionReasonCodeList.Any())
                {
                    foreach (KeyValuePair<string, string> entry in taxExemptionReasonCodeList)
                    {
                        str = str.Replace(entry.Key.ToString(), entry.Value);
                    }
                }

                str = str.Replace("@TaxTotalTaxSubtotalTaxCategoryName", taxSchemeNameValue);
                str = str.Replace("@TaxTotalTaxSubtotalTaxCategoryTaxTypeCode", taxTypeCodeValue);

                str = str.Replace("@ItemDescription", trxLine.ProductName);

                str = str.Replace("@pricePriceAmount", (trxLine.OriginalPrice).ToString(amountFormat, invC));
                str = str.Replace("@PricePriceAmountAmountCurrencyID", currencyCode);
            }
            catch (Exception ex)
            {
                log.Error("Build InvoiceLine Section Error", ex);
                log.LogMethodExit(null, "Throwing Build InvoiceLine Section Exception-" + ex.Message);
                throw;
            }
            log.LogMethodExit(str);
            return str;
        }

        private void SetInvoiceTemplates()
        {
            log.LogMethodEntry();
            boletaHeaderTemplate = LoadTemplateData(PERU_BOLETA_HEADER);
            facturaHeaderTemplate = LoadTemplateData(PERU_FACTURA_HEADER);
            invoiceLineTemplate = LoadTemplateData(PERU_INVOICE_LINE);
            invoiceTaxTotalTemplate = LoadTemplateData(PERU_INVOICE_TAX_TOTAL);
            invoiceTaxTotalSubTotalTemplate = LoadTemplateData(PERU_TAX_TOTAL_SUBTOTAL);
            invoiceLegalTemplate = LoadTemplateData(PERU_INVOICE_LEGAL);
            invoiceDiscountTemplate = LoadTemplateData(PERU_INVOICE_DISCOUNT);
            invoiceDiscountTotalTemplate = LoadTemplateData(PERU_INVOICE_DISCOUNT_TOTAL);
            log.LogMethodExit();
        }

        private void SetCreditNoteTemplates()
        {
            log.LogMethodEntry();
            creditNoteHeaderTemplate = LoadTemplateData(PERU_CREDIT_NOTE_HEADER);
            creditNoteLineTemplate = LoadTemplateData(PERU_CREDIT_NOTE_LINE);
            creditNoteTaxTotalTemplate = LoadTemplateData(PERU_CREDIT_NOTE_TAX_TOTAL);
            creditNoteLegalTemplate = LoadTemplateData(PERU_CREDIT_NOTE_LEGAL);
            log.LogMethodExit();
        }

        private string LoadTemplateData(string templateSection)
        {
            log.LogMethodEntry(templateSection);
            string template = string.Empty;
            try
            {
                string strExeFilePath = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                strExeFilePath += "\\Resources\\PeruTemplates\\";

                if (Directory.Exists(strExeFilePath))
                {
                    switch (templateSection)
                    {
                        case PERU_BOLETA_HEADER:
                            {
                                template = File.ReadAllText(strExeFilePath + PERU_BOLETA_HEADER + TXT_FILE_EXTENSION);
                                break;
                            }
                        case PERU_FACTURA_HEADER:
                            {
                                template = File.ReadAllText(strExeFilePath + PERU_FACTURA_HEADER + TXT_FILE_EXTENSION);
                                break;
                            }
                        case PERU_INVOICE_LINE:
                            {
                                template = File.ReadAllText(strExeFilePath + PERU_INVOICE_LINE + TXT_FILE_EXTENSION);
                                break;
                            }
                        case PERU_INVOICE_TAX_TOTAL:
                            {
                                template = File.ReadAllText(strExeFilePath + PERU_INVOICE_TAX_TOTAL + TXT_FILE_EXTENSION);
                                break;
                            }
                        case PERU_TAX_TOTAL_SUBTOTAL:
                            {
                                template = File.ReadAllText(strExeFilePath + PERU_TAX_TOTAL_SUBTOTAL + TXT_FILE_EXTENSION);
                                break;
                            }
                        case PERU_INVOICE_LEGAL:
                            {
                                template = File.ReadAllText(strExeFilePath + PERU_INVOICE_LEGAL + TXT_FILE_EXTENSION);
                                break;
                            }
                        case PERU_INVOICE_DISCOUNT:
                            {
                                template = File.ReadAllText(strExeFilePath + PERU_INVOICE_DISCOUNT + TXT_FILE_EXTENSION);
                                break;
                            }
                        case PERU_INVOICE_DISCOUNT_TOTAL:
                            {
                                template = File.ReadAllText(strExeFilePath + PERU_INVOICE_DISCOUNT_TOTAL + TXT_FILE_EXTENSION);
                                break;
                            }
                        case PERU_CREDIT_NOTE_HEADER:
                            {
                                template = File.ReadAllText(strExeFilePath + PERU_CREDIT_NOTE_HEADER + TXT_FILE_EXTENSION);
                                break;
                            }
                        case PERU_CREDIT_NOTE_LINE:
                            {
                                template = File.ReadAllText(strExeFilePath + PERU_CREDIT_NOTE_LINE + TXT_FILE_EXTENSION);
                                break;
                            }
                        case PERU_CREDIT_NOTE_TAX_TOTAL:
                            {
                                template = File.ReadAllText(strExeFilePath + PERU_CREDIT_NOTE_TAX_TOTAL + TXT_FILE_EXTENSION);
                                break;
                            }
                        case PERU_CREDIT_NOTE_LEGAL:
                            {
                                template = File.ReadAllText(strExeFilePath + PERU_CREDIT_NOTE_LEGAL + TXT_FILE_EXTENSION);
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

        private List<TransactionDTO> GetEligibleInvoiceRecords(DateTime currentTime, DateTime LastRunTime)
        {
            log.LogMethodEntry(currentTime, LastRunTime);
            List<TransactionDTO> transactionDTOList = new List<TransactionDTO>();
            try
            {
                if (utilities != null && utilities.ParafaitEnv != null)
                {
                    siteName = utilities.ParafaitEnv.SiteName;
                }
                List<LookupValuesContainerDTO> lookupValuesDTOList = GetEligibleOverrideOptions();
                if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
                {
                    string overrideOptions = lookupValuesDTOList[0].Description;
                    TransactionListBL transactionListBL = new TransactionListBL(executionContext);
                    List<KeyValuePair<TransactionDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<TransactionDTO.SearchByParameters, string>>();
                    searchParams.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                    searchParams.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.POS_OVERRIDE_OPTION_NAME_LIST, overrideOptions));
                    searchParams.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.LAST_UPDATE_FROM_TIME, LastRunTime.ToString("yyyy-MM-dd HH:mm:ss")));
                    searchParams.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.LAST_UPDATE_TO_TIME, currentTime.ToString("yyyy-MM-dd HH:mm:ss")));
                    searchParams.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.STATUS, Transaction.Transaction.TrxStatus.CLOSED.ToString()));
                    transactionDTOList = transactionListBL.GetTransactionDTOList(searchParams, utilities, null, 0, 5000);
                    if (transactionDTOList != null && transactionDTOList.Any())
                    {
                        //filter procecssed records
                        transactionDTOList = transactionDTOList.Where(t => string.IsNullOrWhiteSpace(t.ExternalSystemReference) == true
                                                                            || t.OriginalTransactionId > 0).ToList();
                        transactionDTOList = CheckForProcessedReversalTrx(transactionDTOList);
                    }
                }
                else
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2933, "Peru Invoice"));//&1 setup details are missing
                }
            }
            catch (Exception ex)
            {
                log.Error("Get Eligible Invoice Records Error", ex);
                throw;
            }
            log.LogMethodExit(transactionDTOList);
            return transactionDTOList;
        }

        private List<LookupValuesContainerDTO> GetEligibleOverrideOptions()
        {
            log.LogMethodEntry();
            List<LookupValuesContainerDTO> lookupValuesContainerDTOList = LookupsContainerList.GetLookupsContainerDTO(executionContext.GetSiteId(), FISCAL_INVOICE_SETUP, executionContext).LookupValuesContainerDTOList;
            if (lookupValuesContainerDTOList != null && lookupValuesContainerDTOList.Any())
            {
                lookupValuesContainerDTOList = lookupValuesContainerDTOList.Where(lv => lv.LookupValue == ELIGIBLEOVERRIDEOPTIONS).ToList();
            }
            log.LogMethodExit(lookupValuesContainerDTOList);
            return lookupValuesContainerDTOList;
        }

        private string GetInvoiceType(Transaction.Transaction trx)
        {
            log.LogMethodEntry();
            string invoicType = trx.GetInvoiceType();
            log.LogMethodExit(invoicType);
            return invoicType;
        }

        private string GetInvoiceType(int transactionId)
        {
            log.LogMethodEntry();
            string invoicType = string.Empty;
            try
            {
                if (transactionId > -1)
                {
                    TrxPOSPrinterOverrideRulesListBL trxPOSPrinterOverrideRulesListBL = new TrxPOSPrinterOverrideRulesListBL(executionContext);
                    List<KeyValuePair<TrxPOSPrinterOverrideRulesDTO.SearchByParameters, string>> searchParam = new List<KeyValuePair<TrxPOSPrinterOverrideRulesDTO.SearchByParameters, string>>();
                    searchParam.Add(new KeyValuePair<TrxPOSPrinterOverrideRulesDTO.SearchByParameters, string>(TrxPOSPrinterOverrideRulesDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                    searchParam.Add(new KeyValuePair<TrxPOSPrinterOverrideRulesDTO.SearchByParameters, string>(TrxPOSPrinterOverrideRulesDTO.SearchByParameters.TRANSACTION_ID, transactionId.ToString()));
                    List<TrxPOSPrinterOverrideRulesDTO> trxPOSPrinterOverrideRulesDTOList = trxPOSPrinterOverrideRulesListBL.GetTrxPOSPrinterOverrideRulesDTOList(searchParam);
                    if (trxPOSPrinterOverrideRulesDTOList != null && trxPOSPrinterOverrideRulesDTOList.Any())
                    {
                        POSPrinterOverrideOptionsBL pOSPrinterOverrideOptionsBL = new POSPrinterOverrideOptionsBL(executionContext, trxPOSPrinterOverrideRulesDTOList[0].POSPrinterOverrideOptionId);
                        POSPrinterOverrideOptionsDTO pOSPrinterOverrideOptionsDTO = pOSPrinterOverrideOptionsBL.POSPrinterOverrideOptionsDTO;
                        invoicType = pOSPrinterOverrideOptionsDTO.OptionName;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Get Invoice Type Error", ex);
                log.LogMethodExit(null, "Throwing Exception in Get Invoice Type-" + ex.Message);
                throw;
            }
            log.LogMethodExit(invoicType);
            return invoicType;
        }

        private void SetInvoiceTypeAttributes(string invoicType)
        {
            log.LogMethodEntry(invoicType);
            if (invoicType == "Boleta")
            {
                invoiceTypeCode = boletaValue;
                accountingCPId = accountingCPDNI;
                acpIdentificationSchemeID = dniValue;
                fileUploadPath = boleta_File_Upload_Path;
            }
            else if (invoicType == "Factura")
            {
                invoiceTypeCode = facturaValue;
                accountingCPId = accountingCPRUC;
                acpIdentificationSchemeID = rucValue;
                fileUploadPath = factura_File_Upload_Path;
            }
            log.LogMethodExit();
        }

        private void BuildProfileDTO(Transaction.Transaction trx)
        {
            log.LogMethodEntry();
            try
            {
                if (trx.customerDTO != null && trx.customerDTO.Id > -1 && trx.customerDTO.ProfileDTO != null && trx.customerDTO.ProfileDTO.Id > -1)
                {
                    accountingCPName = (string.IsNullOrWhiteSpace(trx.customerDTO.ProfileDTO.FirstName) ? string.Empty : trx.customerDTO.ProfileDTO.FirstName) +
                                       (string.IsNullOrWhiteSpace(trx.customerDTO.ProfileDTO.MiddleName) ? string.Empty : " " + trx.customerDTO.ProfileDTO.MiddleName) +
                                       (string.IsNullOrWhiteSpace(trx.customerDTO.ProfileDTO.LastName) ? string.Empty : " " + trx.customerDTO.ProfileDTO.LastName);
                    accountingCPRegistraionId = trx.customerDTO.Id.ToString();
                    if (trx.customerDTO.ProfileDTO.AddressDTOList != null && trx.customerDTO.ProfileDTO.AddressDTOList.Any())
                    {
                        acpIdentifierContent = trx.customerDTO.ProfileDTO.AddressDTOList[0].PostalCode;
                        accountingCPCitySubdivisionName = string.Empty;
                        accountingCPCityName = trx.customerDTO.ProfileDTO.AddressDTOList[0].City;
                        accountingCPCountrySubentity = string.Empty;
                        accountingCPDistrict = string.Empty;
                        accountingCPAddressLine = trx.customerDTO.ProfileDTO.AddressDTOList[0].Line1;
                        accountingCPMailId = trx.customerDTO.Email;
                        accountingCPDNI = trx.customerDTO.TaxCode;
                        accountingCPRUC = trx.customerDTO.UniqueIdentifier;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Build ProfileDTO Error", ex);
                log.LogMethodExit(null, "Throwing Exception in Build ProfileDTO" + ex.Message);
                throw;
            }
            log.LogMethodExit();
        }

        private string BuildLineDiscountTotalValues(decimal discountTotal)
        {
            log.LogMethodEntry(discountTotal);
            string discountTotalSection = string.Empty;
            try
            {
                if (discountTotal > 0)
                {
                    discountTotalSection = invoiceDiscountTotalTemplate;
                    discountTotalSection = discountTotalSection.Replace("@discountTotal", discountTotal.ToString(amountFormat, invC));
                }
            }
            catch (Exception ex)
            {
                log.Error("Build Line Discount Total Values Error", ex);
                log.LogMethodExit(null, "Throwing Build Line Discount Total Values Exception-" + ex.Message);
                throw;
            }
            log.LogMethodExit(discountTotalSection);
            return discountTotalSection;
        }

        private Transaction.Transaction GetOriginalTrx(Transaction.Transaction trx)
        {
            log.LogMethodEntry(trx);
            Transaction.Transaction originalTrx = null;
            try
            {
                if (trx != null && trx.OriginalTrxId > -1)
                {
                    List<TransactionDTO> transactionDTOList = new List<TransactionDTO>();
                    TransactionListBL transactionListBL = new TransactionListBL(executionContext);
                    List<KeyValuePair<TransactionDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<TransactionDTO.SearchByParameters, string>>();
                    searchParams.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                    searchParams.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.TRANSACTION_ID, (trx.OriginalTrxId).ToString()));
                    transactionDTOList = transactionListBL.GetTransactionDTOList(searchParams, null, null, 0, 5000);
                    if (transactionDTOList != null && transactionDTOList.Any())
                    {
                        TransactionUtils transactionUtils = new TransactionUtils(utilities);
                        originalTrx = transactionUtils.CreateTransactionFromDB(transactionDTOList[0].TransactionId, utilities);

                        billingReferenceID = transactionDTOList[0].TransactionNumber;

                        billingReferenceIssueDate = transactionDTOList[0].TransactionDate.ToString(dateFormat);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Get Original Trx Data Error", ex);
                log.LogMethodExit(null, "Throwing Exception in Getting Original Trx Data" + ex.Message);
                throw;
            }
            log.LogMethodExit();
            return originalTrx;
        }

        private string BuildHeaderSection(Transaction.Transaction trx, string invoiceType, bool isReversal)
        {
            log.LogMethodEntry();
            string headerSection = string.Empty;
            try
            {
                if (isReversal)
                {
                    headerSection = BuildCreditNoteHeadervalues(trx);
                }
                else if (invoiceType == "Boleta")
                {
                    headerSection = BuildInvoiceHeadervalues(trx, boletaHeaderTemplate);
                }
                else if (invoiceType == "Factura")
                {
                    headerSection = BuildInvoiceHeadervalues(trx, facturaHeaderTemplate);
                }
            }
            catch (Exception ex)
            {
                log.Error("Build Header Section Error", ex);
                log.LogMethodExit(null, "Throwing Exception in Build Header Section" + ex.Message);
                throw new Exception(ex.Message);
            }
            log.LogMethodExit(headerSection);
            return headerSection;
        }

        private string BuildInvoiceHeadervalues(Transaction.Transaction transaction, string headerSection)
        {
            log.LogMethodEntry(transaction);
            try
            {
                headerSection = headerSection.Replace("@UBLVersionId", uBLVersionID);
                headerSection = headerSection.Replace("@CustomizationId", customizationID);
                headerSection = headerSection.Replace("@CustomizationAgencyName", customizationAgencyName);
                headerSection = headerSection.Replace("@InvoiceId", transaction.Trx_No);

                headerSection = headerSection.Replace("@IssueTime", (transaction.TrxDate).ToString(timeFormat, invC));
                headerSection = headerSection.Replace("@IssueDate", (transaction.TrxDate.ToString(dateFormat, invC)));
                headerSection = headerSection.Replace("@DueDate", (transaction.TrxDate.ToString(dateFormat, invC)));
                headerSection = headerSection.Replace("@InvoiceTypeCodeCodeContent", invoiceTypeCode);//pending
                if (invoiceTypeCodeAttributeList != null && invoiceTypeCodeAttributeList.Any())
                {
                    foreach (KeyValuePair<string, string> entry in invoiceTypeCodeAttributeList)
                    {
                        headerSection = headerSection.Replace(entry.Key.ToString(), entry.Value);
                    }
                }
                headerSection = headerSection.Replace("@DocumentCurrencyCodeCodeContent", currencyCode);//pending
                if (documentCurrencyCodeAttributeList != null && documentCurrencyCodeAttributeList.Any())
                {
                    foreach (KeyValuePair<string, string> entry in documentCurrencyCodeAttributeList)
                    {
                        headerSection = headerSection.Replace(entry.Key, entry.Value);
                    }
                }
                if (documentCurrencyCodeAttributeList != null && documentCurrencyCodeAttributeList.Any())
                {
                    foreach (KeyValuePair<string, string> entry in documentCurrencyCodeAttributeList)
                    {
                        headerSection = headerSection.Replace(entry.Key, entry.Value);
                    }
                }
                trxLineCount = GetTrxLineCount(transaction);
                headerSection = headerSection.Replace("@TrxLineCount", trxLineCount.ToString());

                headerSection = headerSection.Replace("@SignatureIDContent", signatureId);
                headerSection = headerSection.Replace("@SignatureIDSchmeId", signatureIDSchmeId);
                headerSection = headerSection.Replace("@SignatureIDSchmeName", signatureIDSchmeName);
                headerSection = headerSection.Replace("@SignatureIDSchmeAgency", signatureIDSchmeAgency);
                headerSection = headerSection.Replace("@SignatureIDSchmeURI", signatureIDSchmeURI);

                headerSection = headerSection.Replace("@SignatoryPartyPartyIdentificationID", rucNumber);
                headerSection = headerSection.Replace("@PartyNameName", siteName);
                headerSection = headerSection.Replace("@DigitalSignatureAttachmentExternalReferenceURI", externalReferenceUri);
                headerSection = headerSection.Replace("@ASPID", rucNumber);

                if (accountingSPList != null && accountingSPList.Any())
                {
                    foreach (KeyValuePair<string, string> entry in accountingSPList)
                    {
                        headerSection = headerSection.Replace(entry.Key, entry.Value);
                    }
                }
                headerSection = headerSection.Replace("@ASPPartyName", accountingSPName);
                headerSection = headerSection.Replace("@ASPRegistrationName", accountingSPRegistrationName);
                headerSection = headerSection.Replace("@ASPRegistrationAddressID", accountingSPRegistrationAddressId);
                headerSection = headerSection.Replace("@ASPRegistrationIDSchemeAgency", accountingSPRegistrationIDSchemeAgency);
                headerSection = headerSection.Replace("@ASPRegistrationIDSchemeName", accountingSPRegistrationIDSchemeName);
                headerSection = headerSection.Replace("@ASPRegistrationAddressCodeContent", addressTypeCode);
                if (accountingSPAddressTypeCodeList != null && accountingSPAddressTypeCodeList.Any())
                {
                    foreach (KeyValuePair<string, string> entry in accountingSPAddressTypeCodeList)
                    {
                        headerSection = headerSection.Replace(entry.Key, entry.Value);
                    }
                }
                headerSection = headerSection.Replace("@ASPCity", accountingSPCityName);
                headerSection = headerSection.Replace("@ASPCountrySubentity", accountingSPCountrySubentity);
                headerSection = headerSection.Replace("@ASPDistrict", accountingSPDistrict);
                headerSection = headerSection.Replace("@ASPAddressLine", accountingSPLine);
                headerSection = headerSection.Replace("@ASPCountryCodeContent", accountingSPIdentificationCode);
                headerSection = headerSection.Replace("@ASPCountryCodeListId", accountingSPCountryCodeListId);
                headerSection = headerSection.Replace("@ASPCountryCodeListAgency", accountingSPCountryCodeListAgency);
                headerSection = headerSection.Replace("@ASPCountryCodeListName", accountingSPCountryCodeListName);
                headerSection = headerSection.Replace("@ASPCountryCodeListName", accountingSPCountryCodeListName);
                headerSection = headerSection.Replace("@ASPEmail", accountingSPEmailId);

                headerSection = headerSection.Replace("@ACPID", accountingCPId);
                headerSection = headerSection.Replace("@ACPIdentificationSchemeID", acpIdentificationSchemeID);
                if (accountingCPList != null && accountingCPList.Any())
                {
                    foreach (KeyValuePair<string, string> entry in accountingCPList)
                    {
                        headerSection = headerSection.Replace(entry.Key, entry.Value);
                    }
                }
                headerSection = headerSection.Replace("@ACPPartyName", accountingCPName);
                headerSection = headerSection.Replace("@ACPPartyLegalEntityRegistrationName", accountingCPName);
                headerSection = headerSection.Replace("@ACPIdentifierContent", acpIdentifierContent);
                headerSection = headerSection.Replace("@ACPCityName", accountingCPCityName);
                headerSection = headerSection.Replace("@ACPCountrySubentity", accountingCPCitySubdivisionName);
                headerSection = headerSection.Replace("@ACPDistrict", accountingCPDistrict);
                headerSection = headerSection.Replace("@ACPAddressLine", accountingCPAddressLine);
                headerSection = headerSection.Replace("@ACPCountryID", accountingCPIdentificationCode);
                headerSection = headerSection.Replace("@ACPCountryCodeListId", accountingCPCountryCodeListId);
                headerSection = headerSection.Replace("@ACPCountryCodeListAgency", accountingCPCountryCodeListAgency);
                headerSection = headerSection.Replace("@ACPCountryCodeListName", accountingCPCountryCodeListName);

                headerSection = headerSection.Replace("@ACPMailID", accountingCPMailId);
            }
            catch (Exception ex)
            {
                log.Error("Build Invoice Header values Error", ex);
                log.LogMethodExit(null, "Throwing Exception in Build Invoice Header values" + ex.Message);
                throw;
            }
            log.LogMethodExit(headerSection);
            return headerSection;
        }

        private string BuildCreditNoteHeadervalues(Transaction.Transaction transaction)
        {
            log.LogMethodEntry();
            string headerSection = creditNoteHeaderTemplate;
            try
            {
                headerSection = headerSection.Replace("@UBLVersionId", uBLVersionID);
                headerSection = headerSection.Replace("@CustomizationId", customizationID);
                headerSection = headerSection.Replace("@CustomizationAgencyName", customizationAgencyName);
                headerSection = headerSection.Replace("@InvoiceId", transaction.Trx_No);
                headerSection = headerSection.Replace("@IssueDate", (transaction.TrxDate.ToString(dateFormat, invC)));
                headerSection = headerSection.Replace("@IssueTime", (transaction.TrxDate).ToString(timeFormat, invC));
                headerSection = headerSection.Replace("@InvoiceTypeCodeCodeContent", invoiceTypeCode);
                headerSection = headerSection.Replace("@DocumentCurrencyCodeCodeContent", currencyCode);
                if (documentCurrencyCodeAttributeList != null && documentCurrencyCodeAttributeList.Any())
                {
                    foreach (KeyValuePair<string, string> entry in documentCurrencyCodeAttributeList)
                    {
                        headerSection = headerSection.Replace(entry.Key, entry.Value);
                    }
                }
                headerSection = headerSection.Replace("@DiscrepancyResponseCode", discrepancyResponseCode);
                if (DiscrepancyResponseCodeList != null && DiscrepancyResponseCodeList.Any())
                {
                    foreach (KeyValuePair<string, string> entry in DiscrepancyResponseCodeList)
                    {
                        headerSection = headerSection.Replace(entry.Key, entry.Value);
                    }
                }

                headerSection = headerSection.Replace("@DiscrepancyResponseCodeDescription", billingReferenceID);//pending - Original invoice number

                headerSection = headerSection.Replace("@BillingReferenceID", billingReferenceID);//pending - Original invoice number
                headerSection = headerSection.Replace("@BillingReferenceIssueDate", billingReferenceIssueDate);//pending - Original invoice Date
                headerSection = headerSection.Replace("@BillingReferenceDocumentTypeCode", invoiceTypeCode);//pending - Original invoice Date

                headerSection = headerSection.Replace("@BRDocumentTypeCode", invoiceTypeCode);//pending - Original invoice Date
                if (documentTypeCodeList != null && documentTypeCodeList.Any())
                {
                    foreach (KeyValuePair<string, string> entry in documentTypeCodeList)
                    {
                        headerSection = headerSection.Replace(entry.Key, entry.Value);
                    }
                }
                trxLineCount = GetTrxLineCount(transaction);
                headerSection = headerSection.Replace("@TrxLineCount", trxLineCount.ToString());

                headerSection = headerSection.Replace("@SignatureIDContent", signatureId);
                headerSection = headerSection.Replace("@SignatureIDSchmeId", signatureIDSchmeId);
                headerSection = headerSection.Replace("@SignatureIDSchmeName", signatureIDSchmeName);
                headerSection = headerSection.Replace("@SignatureIDSchmeAgency", signatureIDSchmeAgency);
                headerSection = headerSection.Replace("@SignatureIDSchmeURI", signatureIDSchmeURI);

                headerSection = headerSection.Replace("@SignatoryPartyPartyIdentificationID", rucNumber);
                headerSection = headerSection.Replace("@PartyNameName", siteName);
                headerSection = headerSection.Replace("@DigitalSignatureAttachmentExternalReferenceURI", externalReferenceUri);
                headerSection = headerSection.Replace("@ASPID", rucNumber);

                if (accountingSPList != null && accountingSPList.Any())
                {
                    foreach (KeyValuePair<string, string> entry in accountingSPList)
                    {
                        headerSection = headerSection.Replace(entry.Key, entry.Value);
                    }
                }
                headerSection = headerSection.Replace("@ASPPartyName", accountingSPName);
                headerSection = headerSection.Replace("@ASPRegistrationName", accountingSPRegistrationName);
                headerSection = headerSection.Replace("@ASPRegistrationAddressID", accountingSPRegistrationAddressId);
                headerSection = headerSection.Replace("@ASPRegistrationIDSchemeAgency", accountingSPRegistrationIDSchemeAgency);
                headerSection = headerSection.Replace("@ASPRegistrationIDSchemeName", accountingSPRegistrationIDSchemeName);
                headerSection = headerSection.Replace("@ASPRegistrationAddressCodeContent", addressTypeCode);
                if (accountingSPAddressTypeCodeList != null && accountingSPAddressTypeCodeList.Any())
                {
                    foreach (KeyValuePair<string, string> entry in accountingSPAddressTypeCodeList)
                    {
                        headerSection = headerSection.Replace(entry.Key, entry.Value);
                    }
                }
                headerSection = headerSection.Replace("@ASPCity", accountingSPCityName);
                headerSection = headerSection.Replace("@ASPCountrySubentity", accountingSPCountrySubentity);
                headerSection = headerSection.Replace("@ASPDistrict", accountingSPDistrict);
                headerSection = headerSection.Replace("@ASPAddressLine", accountingSPLine);
                headerSection = headerSection.Replace("@ASPCountryCodeContent", accountingSPIdentificationCode);
                headerSection = headerSection.Replace("@ASPCountryCodeListId", accountingSPCountryCodeListId);
                headerSection = headerSection.Replace("@ASPCountryCodeListAgency", accountingSPCountryCodeListAgency);
                headerSection = headerSection.Replace("@ASPCountryCodeListName", accountingSPCountryCodeListName);
                headerSection = headerSection.Replace("@ASPEmail", accountingSPEmailId);

                headerSection = headerSection.Replace("@ACPID", accountingCPId);
                headerSection = headerSection.Replace("@ACPIdentificationSchemeID", acpIdentificationSchemeID);
                if (accountingCPList != null && accountingCPList.Any())
                {
                    foreach (KeyValuePair<string, string> entry in accountingCPList)
                    {
                        headerSection = headerSection.Replace(entry.Key, entry.Value);
                    }
                }
                headerSection = headerSection.Replace("@ACPPartyName", accountingCPName);
                headerSection = headerSection.Replace("@ACPPartyLegalEntityRegistrationName", accountingCPName);
                headerSection = headerSection.Replace("@ACPIdentifierContent", acpIdentifierContent);
                headerSection = headerSection.Replace("@ACPCityName", accountingCPCityName);
                headerSection = headerSection.Replace("@ACPCountrySubentity", accountingCPCitySubdivisionName);
                headerSection = headerSection.Replace("@ACPDistrict", accountingCPDistrict);
                headerSection = headerSection.Replace("@ACPAddressLine", accountingCPAddressLine);
                headerSection = headerSection.Replace("@ACPCountryID", accountingCPIdentificationCode);
                headerSection = headerSection.Replace("@ACPCountryCodeListId", accountingCPCountryCodeListId);
                headerSection = headerSection.Replace("@ACPCountryCodeListAgency", accountingCPCountryCodeListAgency);
                headerSection = headerSection.Replace("@ACPCountryCodeListName", accountingCPCountryCodeListName);
                headerSection = headerSection.Replace("@ACPMailID", accountingCPMailId);
            }
            catch (Exception ex)
            {
                log.Error("Build Credit Note Header values Error", ex);
                log.LogMethodExit(null, "Throwing Exception in Build Credit Note Header values" + ex.Message);
                throw;
            }
            log.LogMethodExit(headerSection);
            return headerSection;
        }

        private string BuildTaxAndLegalSummaryDetailSection(Transaction.Transaction trx, string invoiceType, bool isReversal = false)
        {
            log.LogMethodEntry();
            string taxAndLegalMonetarySection = string.Empty;
            try
            {
                string taxTotalSection = string.Empty;
                string legalMonetarySection = string.Empty;
                if (isReversal)
                {
                    taxTotalSection = BuildCreditNoteTaxTotalValues(trx);
                    legalMonetarySection = BuildCreditNoteLegalMonetaryValues(trx);
                }
                else if (invoiceType == "Boleta" || invoiceType == "Factura")
                {
                    taxTotalSection = BuildInvoiceTaxTotalValues(trx);
                    legalMonetarySection = BuildInvoiceLegalMonetaryValues(trx);
                }
                taxAndLegalMonetarySection = taxTotalSection + legalMonetarySection;
            }
            catch (Exception ex)
            {
                log.Error("Build Lines Detail Section Error", ex);
                log.LogMethodExit(null, "Throwing Build Lines Detail Section Exception-" + ex.Message);
                throw;
            }
            log.LogMethodExit(taxAndLegalMonetarySection);
            return taxAndLegalMonetarySection;
        }

        private string BuildInvoiceTaxTotalValues(Transaction.Transaction trx)
        {
            log.LogMethodEntry();
            string taxTotalSection = invoiceTaxTotalTemplate;
            try
            {
                taxTotalSection = taxTotalSection.Replace("@TaxTotalAmountContent", taxAmountValue.ToString(amountFormat, invC));//(trx.Tax_Amount).ToString())//pending
                taxTotalSection = taxTotalSection.Replace("@TaxTotalAmountCurrencyID", currencyCode);
                string taxTotalSubTotal = BuildInvoiceTaxTotalSubTotalValues();
                taxTotalSection = taxTotalSection.Replace("@TaxTotalSubTotal", taxTotalSubTotal);
            }
            catch (Exception ex)
            {
                log.Error("Build Invoice TaxTotal Values Error", ex);
                log.LogMethodExit(null, "Throwing Build Invoice TaxTotal Values Exception-" + ex.Message);
                throw;
            }
            log.LogMethodExit(taxTotalSection);
            return taxTotalSection;
        }

        private string BuildInvoiceTaxTotalSubTotalValues()
        {
            log.LogMethodEntry();
            string taxTotalSubTotalSection = "";
            try
            {
                if (taxableAmountBeforeDiscountValue != 0)
                {
                    taxTotalSubTotalSection = invoiceTaxTotalSubTotalTemplate;
                    taxTotalSubTotalSection = taxTotalSubTotalSection.Replace("@TaxTotalAmountTaxableAmountContent", taxableAmountAfterDiscountValue.ToString(amountFormat, invC));// (trx.Net_Transaction_Amount - trx.Tax_Amount).ToString());
                    taxTotalSubTotalSection = taxTotalSubTotalSection.Replace("@TaxTotalAmountCurrencyID", currencyCode);
                    taxTotalSubTotalSection = taxTotalSubTotalSection.Replace("@TaxTotalTaxSubtotalAmountContent", taxAmountValue.ToString(amountFormat, invC));//(trx.Tax_Amount).ToString());
                    taxTotalSubTotalSection = taxTotalSubTotalSection.Replace("@TaxTotalTaxSubtotalAmountCurrencyID", currencyCode);

                    Dictionary<string, string> taxSchemeListValue = taxSchemeList;
                    //string taxSchemeIdValue = Math.Abs(taxableAmountBeforeDiscountValue) != 0 ? taxSchemeId : zeroTaxSchemeId;
                    //string taxSchemeNameValue = Math.Abs(taxableAmountBeforeDiscountValue) != 0 ? taxSchemeName : zeroTaxSchemeName;
                    //string taxTypeCodeValue = Math.Abs(taxableAmountBeforeDiscountValue) != 0 ? taxTypeCode : zeroTaxTypeCode; 
                    string taxSchemeIdValue = taxSchemeId;
                    string taxSchemeNameValue = taxSchemeName;
                    string taxTypeCodeValue = taxTypeCode;
                    log.LogVariableState("taxSchemeIdValue", taxSchemeIdValue);
                    log.LogVariableState("taxSchemeNameValue", taxSchemeNameValue);
                    log.LogVariableState("taxTypeCodeValue", taxTypeCodeValue);

                    taxTotalSubTotalSection = taxTotalSubTotalSection.Replace("@TaxTotalTaxSubtotalTaxCategoryID", taxSchemeIdValue);

                    if (taxSchemeListValue != null && taxSchemeListValue.Any())
                    {
                        foreach (KeyValuePair<string, string> entry in taxSchemeListValue)
                        {
                            taxTotalSubTotalSection = taxTotalSubTotalSection.Replace(entry.Key.ToString(), entry.Value);
                        }
                    }

                    taxTotalSubTotalSection = taxTotalSubTotalSection.Replace("@TaxTotalTaxSubtotalTaxCategoryName", taxSchemeNameValue);
                    taxTotalSubTotalSection = taxTotalSubTotalSection.Replace("@TaxTotalTaxSubtotalTaxCategoryTaxTypeCode", taxTypeCodeValue);
                    taxTotalSubTotalSection = taxTotalSubTotalSection.Replace("@CurrencyID", currencyCode);
                }
            }
            catch (Exception ex)
            {
                log.Error("Build Invoice Tax Total Subtotal Values Error", ex);
                log.LogMethodExit(null, "Throwing Build Invoice Tax Total Subtotal Values Exception-" + ex.Message);
                throw;
            }
            log.LogMethodExit(taxTotalSubTotalSection);
            return taxTotalSubTotalSection;
        }

        private void CalculateTaxDetails(Transaction.Transaction trx)
        {
            log.LogMethodEntry(trx);
            decimal taxableAmountBD = 0;
            decimal taxableAmountAD = 0;
            decimal taxAmount = 0;
            try
            {
                if (trx != null && trx.TransactionLineList != null && trx.TransactionLineList.Any())
                {
                    foreach (Transaction.Transaction.TransactionLine trxLine in trx.TransactionLineList)
                    {
                        decimal amount = 0;
                        decimal amountBeforeDiscount = 0;
                        amount = GetLinePrice(trxLine);
                        amountBeforeDiscount = Convert.ToDecimal(((decimal)trxLine.Price), invC);
                        taxableAmountBD = taxableAmountBD + amountBeforeDiscount;
                        taxableAmountAD = taxableAmountAD + amount;
                        if (trxLine.tax_percentage > 0)
                        {
                            taxAmount = taxAmount + (amount * ((Convert.ToDecimal(((decimal)trxLine.tax_percentage), invC) / 100)));
                        }
                    }
                }
                taxableAmountBeforeDiscountValue = taxableAmountBD;
                taxableAmountAfterDiscountValue = taxableAmountAD;
                taxAmountValue = taxAmount;
            }
            catch (Exception ex)
            {
                log.Error("Build Invoice Get taxable amount", ex);
                log.LogMethodExit(null, "Throwing Get taxable amount Exception-" + ex.Message);
                throw;
            }
            log.LogMethodExit();
        }

        private decimal GetLinePrice(Transaction.Transaction.TransactionLine trxLine)
        {
            log.LogMethodEntry();
            decimal amount = GetDiscountedLinePrice(trxLine);
            amount = Convert.ToDecimal((amount), invC);
            log.LogMethodExit(amount);
            return amount;
        }

        private string BuildInvoiceLegalMonetaryValues(Transaction.Transaction trx)
        {
            log.LogMethodEntry();
            string legalMonetarySection = invoiceLegalTemplate;
            try
            {
                legalMonetarySection = legalMonetarySection.Replace("@LegalMonetaryTotalLineExtensionAmountAmountContent", taxableAmountAfterDiscountValue.ToString(amountFormat, invC));
                legalMonetarySection = legalMonetarySection.Replace("@CurrencyID", currencyCode);
                decimal totalTaxInclusiveAmount = taxableAmountAfterDiscountValue + taxAmountValue;
                legalMonetarySection = legalMonetarySection.Replace("@LegalMonetaryTotalLineExtensionAmountTaxInclusiveAmountAmountContent", (totalTaxInclusiveAmount).ToString(amountFormat, invC));
                legalMonetarySection = legalMonetarySection.Replace("@LegalMonetaryTotalLineExtensionAmountTaxInclusiveAmountPayableAmountAmountContent", (totalTaxInclusiveAmount).ToString(amountFormat, invC));
                legalMonetarySection = legalMonetarySection.Replace("@LegalMonetaryTotalAllowanceTotalAmountContent", "0.00"); //(0).ToString(amountFormat, invC)); 

            }
            catch (Exception ex)
            {
                log.Error("Build Invoice Legal Monetary Values Error", ex);
                log.LogMethodExit(null, "Throwing Build Invoice Legal Monetary Va Exception-" + ex.Message);
                throw;
            }
            log.LogMethodExit(legalMonetarySection);
            return legalMonetarySection;
        }

        private string BuildCreditNoteTaxTotalValues(Transaction.Transaction trx)
        {
            log.LogMethodEntry();
            string creditNoteTaxSection = creditNoteTaxTotalTemplate;
            try
            {
                creditNoteTaxSection = creditNoteTaxSection.Replace("@TaxTotalAmountContent", taxAmountValue.ToString(amountFormat, invC));
                creditNoteTaxSection = creditNoteTaxSection.Replace("@TaxTotalAmountCurrencyID", currencyCode);
                string taxTotalSubTotal = BuildInvoiceTaxTotalSubTotalValues();
                creditNoteTaxSection = creditNoteTaxSection.Replace("@TaxTotalSubTotal", taxTotalSubTotal);
            }
            catch (Exception ex)
            {
                log.Error("Build CreditNote TaxTotal Values Error", ex);
                log.LogMethodExit(null, "Throwing Build CreditNote TaxTotal Values Exception-" + ex.Message);
                throw;
            }
            log.LogMethodExit(creditNoteTaxSection);
            return creditNoteTaxSection;
        }

        private string BuildCreditNoteLegalMonetaryValues(Transaction.Transaction trx)
        {
            log.LogMethodEntry();
            string legalMonetarySection = creditNoteLegalTemplate;
            try
            {
                legalMonetarySection = legalMonetarySection.Replace("@LegalMonetaryTotalLineExtensionAmountAmountContent", taxableAmountAfterDiscountValue.ToString(amountFormat, invC));
                legalMonetarySection = legalMonetarySection.Replace("@CurrencyID", currencyCode);
                decimal totalTaxInclusiveAmount = taxableAmountAfterDiscountValue + taxAmountValue;
                legalMonetarySection = legalMonetarySection.Replace("@LegalMonetaryTotalLineExtensionAmountTaxInclusiveAmountAmountContent", Math.Abs(totalTaxInclusiveAmount).ToString(amountFormat, invC));
                legalMonetarySection = legalMonetarySection.Replace("@LegalMonetaryTotalLineExtensionAmountTaxInclusiveAmountPayableAmountAmountContent", Math.Abs(totalTaxInclusiveAmount).ToString(amountFormat, invC));
                legalMonetarySection = legalMonetarySection.Replace("@LegalMonetaryTotalAllowanceTotalAmountContent", "0.00"); //Math.Abs(0).ToString(amountFormat, invC)); 
            }
            catch (Exception ex)
            {
                log.Error("Build Build CreditNote Legal Monetary Values Error", ex);
                log.LogMethodExit(null, "Throwing Build CreditNote Legal Monetary Values Exception-" + ex.Message);
                throw;
            }
            log.LogMethodExit(legalMonetarySection);
            return legalMonetarySection;
        }

        private string BuildLineDiscountValues(Transaction.Transaction.TransactionLine trxLine)
        {
            log.LogMethodEntry(trxLine);
            string linesDiscountSection = string.Empty;
            try
            {
                if (trxLine.TransactionDiscountsDTOList != null && trxLine.TransactionDiscountsDTOList.Any())
                {
                    linesDiscountSection = invoiceDiscountTemplate;
                    linesDiscountSection = linesDiscountSection.Replace("@AllowanceChargeIndicator", allowanceChargeIndicator);
                    linesDiscountSection = linesDiscountSection.Replace("@AllowanceChargeReasonCodeValue", allowanceChargeReasonCodeValue);
                    if (allowanceChargeReasonCodeList != null && allowanceChargeReasonCodeList.Any())
                    {
                        foreach (KeyValuePair<string, string> entry in allowanceChargeReasonCodeList)
                        {
                            linesDiscountSection = linesDiscountSection.Replace(entry.Key.ToString(), entry.Value);
                        }
                    }

                    linesDiscountSection = linesDiscountSection.Replace("@AllowanceChargeAmountCurrencyID", currencyCode);
                    decimal lineDiscountAmount = GetLineDiscountAmount(trxLine);
                    linesDiscountSection = linesDiscountSection.Replace("@AllowanceChargeDiscountAmount", (lineDiscountAmount).ToString(amountFormat, invC));
                    linesDiscountSection = linesDiscountSection.Replace("@AllowanceChargeBaseAmount", Math.Abs(trxLine.OriginalPrice).ToString(amountFormat, invC));
                    decimal multiplierFactorNumeric = lineDiscountAmount / (decimal)Math.Abs(trxLine.OriginalPrice);
                    linesDiscountSection = linesDiscountSection.Replace("@MultiplierFactorNumeric", Math.Abs(multiplierFactorNumeric).ToString(amountFormat, invC));
                }
            }
            catch (Exception ex)
            {
                log.Error("Build Build CreditNote Legal Monetary Values Error", ex);
                log.LogMethodExit(null, "Throwing Build CreditNote Legal Monetary Values Exception-" + ex.Message);
                throw;
            }
            log.LogMethodExit(linesDiscountSection);
            return linesDiscountSection;
        }

        private decimal GetLineDiscountAmount(Transaction.Transaction.TransactionLine trxLine)
        {
            log.LogMethodEntry();
            decimal lineDiscountAmount = 0;
            if (trxLine.TransactionDiscountsDTOList != null && trxLine.TransactionDiscountsDTOList.Any())
            {
                decimal discountAmount = 0;
                decimal discountPercentage = 0;
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
                lineDiscountAmount = discountAmount + ((Convert.ToDecimal((trxLine.Price).ToString(amountFormat, invC), invC) * (Convert.ToDecimal((discountPercentage).ToString(amountFormat, invC), invC)) / 100));
            }
            log.LogMethodExit(lineDiscountAmount);
            return lineDiscountAmount;
        }

        private string BuildCreditNoteLineSection(Transaction.Transaction.TransactionLine trxLine)
        {
            log.LogMethodEntry();
            string str = creditNoteLineTemplate;
            decimal lineExtensionAmountBeforeDiscount;
            decimal lineExtensionAmountAfterDiscount;
            decimal PricingReferenceAmountContent;
            decimal taxAmountAmount;
            decimal taxSubtotalTaxAmount;
            try
            {
                str = str.Replace("@CLID", (trxLine.DBLineId).ToString());
                str = str.Replace("@CLNote", "");//pending

                str = str.Replace("@CLInvoicedQuantityQuantityContent", Math.Abs(Convert.ToInt32(trxLine.quantity)).ToString(integerFormat, invC));
                if (invoicedQuantityList != null && invoicedQuantityList.Any())
                {
                    foreach (KeyValuePair<string, string> entry in invoicedQuantityList)
                    {
                        str = str.Replace(entry.Key.ToString(), entry.Value);
                    }
                }
                if (trxLine.TransactionDiscountsDTOList != null && trxLine.TransactionDiscountsDTOList.Any())
                {
                    lineExtensionAmountAfterDiscount = GetLinePrice(trxLine);
                }
                else
                {
                    lineExtensionAmountAfterDiscount = Convert.ToDecimal(trxLine.Price);
                }
                lineExtensionAmountBeforeDiscount = Convert.ToDecimal(trxLine.Price);
                str = str.Replace("@CLLineExtensionAmountAmountContent", lineExtensionAmountAfterDiscount.ToString(amountFormat, invC));
                str = str.Replace("@CLLineExtensionAmountAmountCurrencyID", currencyCode);

                PricingReferenceAmountContent = (Convert.ToDecimal(lineExtensionAmountAfterDiscount, invC) + ((Convert.ToDecimal(lineExtensionAmountAfterDiscount, invC) * Convert.ToDecimal((((decimal)trxLine.tax_percentage)), invC) / 100)));
                str = str.Replace("@CLPricingReferenceAlternativeConditionPricePriceAmountAmountContent", PricingReferenceAmountContent.ToString(amountFormat, invC));
                str = str.Replace("@CLPricingReferenceAlternativeConditionPricePriceAmountAmountCurrencyID", currencyCode);

                //string priceRefTypeCode = Math.Abs(lineExtensionAmountAfterDiscount) != 0 ? pricingReferencePriceTypeCodeCodeContent : "02";
                str = str.Replace("@CLPricingReferenceAlternativeConditionPricePriceTypeCodeCodeContent", pricingReferencePriceTypeCodeCodeContent);
                if (priceTypeCodeList != null && priceTypeCodeList.Any())
                {
                    foreach (KeyValuePair<string, string> entry in priceTypeCodeList)
                    {
                        str = str.Replace(entry.Key.ToString(), entry.Value);
                    }
                }
                //TaxTotal
                taxAmountAmount = (Convert.ToDecimal(lineExtensionAmountAfterDiscount, invC) * Convert.ToDecimal(((decimal)trxLine.tax_percentage).ToString(), invC) / 100);
                str = str.Replace("@CLTaxTotalTaxAmountAmountContent", taxAmountAmount.ToString(amountFormat, invC));
                str = str.Replace("@CLTaxTotalTaxAmountAmountCurrencyID", currencyCode);

                str = str.Replace("@CLTaxTotalTaxSubtotalTaxableAmountAmountContent", lineExtensionAmountAfterDiscount.ToString(amountFormat, invC));//(trxLine.Price).ToString());
                str = str.Replace("@CLTaxTotalTaxSubtotalTaxableAmountAmountCurrencyID", currencyCode);

                taxSubtotalTaxAmount = (Convert.ToDecimal(lineExtensionAmountAfterDiscount, invC) * Convert.ToDecimal(((decimal)trxLine.tax_percentage).ToString(amountFormat, invC), invC)) / 100;
                str = str.Replace("@CLTaxTotalTaxSubtotalTaxAmountAmountContent", taxSubtotalTaxAmount.ToString(amountFormat, invC));
                str = str.Replace("@CLTaxTotalTaxSubtotalTaxAmountAmountCurrencyID", currencyCode);
                str = str.Replace("@CLTaxTotalTaxSubtotalTaxCategoryPercentNumericContent", (trxLine.tax_percentage).ToString(amountFormat, invC));

                Dictionary<string, string> taxSchemeListValue = taxSchemeList;
                //string taxSchemeIdValue = Math.Abs(lineExtensionAmountBeforeDiscount) != 0 ? taxSchemeId : zeroTaxSchemeId;
                //string taxSchemeNameValue = Math.Abs(lineExtensionAmountBeforeDiscount) != 0 ? taxSchemeName : zeroTaxSchemeName;
                //string taxTypeCodeValue = Math.Abs(lineExtensionAmountBeforeDiscount) != 0 ? taxTypeCode : zeroTaxTypeCode; 
                string taxSchemeIdValue = taxSchemeId;
                string taxSchemeNameValue = taxSchemeName;
                string taxTypeCodeValue = taxTypeCode;
                log.LogVariableState("taxSchemeIdValue", taxSchemeIdValue);
                log.LogVariableState("taxSchemeNameValue", taxSchemeNameValue);
                log.LogVariableState("taxTypeCodeValue", taxTypeCodeValue);

                str = str.Replace("@TaxTotalTaxSubtotalTaxCategoryID", taxSchemeIdValue);
                if (taxSchemeListValue != null && taxSchemeListValue.Any())
                {
                    foreach (KeyValuePair<string, string> entry in taxSchemeListValue)
                    {
                        str = str.Replace(entry.Key.ToString(), entry.Value);
                    }
                }
                //string taxExmpReasonCode = Math.Abs(lineExtensionAmountBeforeDiscount) != 0 ? taxExemptionReasonCodeValue : "15";
                str = str.Replace("@ILTaxTotalTaxSubtotalTaxCategoryTERCCodeContent", taxExemptionReasonCodeValue);
                if (taxExemptionReasonCodeList != null && taxExemptionReasonCodeList.Any())
                {
                    foreach (KeyValuePair<string, string> entry in taxExemptionReasonCodeList)
                    {
                        str = str.Replace(entry.Key.ToString(), entry.Value);
                    }
                }
                str = str.Replace("@TaxTotalTaxSubtotalTaxCategoryName", taxSchemeNameValue);
                str = str.Replace("@TaxTotalTaxSubtotalTaxCategoryTaxTypeCode", taxTypeCodeValue);

                str = str.Replace("@ItemDescription", trxLine.ProductName);

                str = str.Replace("@pricePriceAmount", (lineExtensionAmountAfterDiscount).ToString(amountFormat, invC));
                str = str.Replace("@PricePriceAmountAmountCurrencyID", currencyCode);
            }
            catch (Exception ex)
            {
                log.Error("Build Credit Note Line Error", ex);
                log.LogMethodExit(null, "Throwing Build Credit Note Line Exception-" + ex.Message);
                throw;
            }
            log.LogMethodExit(str);
            return str;
        }

        private Dictionary<string, Dictionary<string, string>> GeneratePeruAttributesList(LookupValuesDTO lookupValuesDTO)
        {
            log.LogMethodEntry();
            Dictionary<string, Dictionary<string, string>> xAttributeDictionary = null;
            try
            {
                Dictionary<string, string> attribute = new Dictionary<string, string>();
                xAttributeDictionary = new Dictionary<string, Dictionary<string, string>>();
                if (lookupValuesDTO != null)
                {
                    string description = lookupValuesDTO.Description;
                    string[] xAttributeValue = null;
                    string[] xAttributes = description.Split(new string[] { "||" }, StringSplitOptions.None);
                    foreach (string attr in xAttributes)
                    {
                        xAttributeValue = attr.Split('|');
                        attribute.Add(xAttributeValue[0].ToString().Replace(" ", String.Empty), xAttributeValue[1]);
                    }
                }
                if (!xAttributeDictionary.ContainsKey(lookupValuesDTO.LookupValue))
                {
                    xAttributeDictionary.Add(lookupValuesDTO.LookupValue, attribute);
                }
            }
            catch (Exception ex)
            {
                log.Error("Generating Peru Attributes Error", ex);
                log.LogMethodExit(null, "Throwing Generate Peru Attributes Exception-" + ex.Message);
                throw new Exception(ex.Message);
            }
            log.LogMethodExit(xAttributeDictionary);
            return xAttributeDictionary;
        }

        private void MapPeruAttributes(Dictionary<string, Dictionary<string, string>> peruAttributesDictionary)
        {
            log.LogMethodEntry();
            try
            {
                foreach (var peruAttributes in peruAttributesDictionary)
                {
                    switch (peruAttributes.Key)
                    {
                        case "DocumentCurrencyCode":
                            {
                                documentCurrencyCodeAttributeList = peruAttributes.Value;
                                break;
                            }
                        case "InvoiceTypeCode":
                            {
                                invoiceTypeCodeAttributeList = peruAttributes.Value;
                                break;
                            }
                        case "AccountingSP":
                            {
                                accountingSPList = peruAttributes.Value;
                                break;
                            }
                        case "AccountingCP":
                            {
                                accountingCPList = peruAttributes.Value;
                                break;
                            }
                        case "AccountingSPAddressTypeCode":
                            {
                                accountingSPAddressTypeCodeList = peruAttributes.Value;
                                break;
                            }
                        case "AllowanceChargeReasonCode":
                            {
                                allowanceChargeReasonCodeList = peruAttributes.Value;
                                break;
                            }
                        case "InvoicedQuantity":
                            {
                                invoicedQuantityList = peruAttributes.Value;
                                break;
                            }
                        case "ItemClassificationCode":
                            {
                                itemClassificationCodeList = peruAttributes.Value;
                                break;
                            }
                        case "PriceTypeCode":
                            {
                                priceTypeCodeList = peruAttributes.Value;
                                break;
                            }
                        case "TaxExemptionReasonCode":
                            {
                                taxExemptionReasonCodeList = peruAttributes.Value;
                                break;
                            }
                        case "TaxScheme":
                            {
                                taxSchemeList = peruAttributes.Value;
                                break;
                            }
                        case "ZeroTaxScheme":
                            {
                                zeroTaxSchemeList = peruAttributes.Value;
                                break;
                            }
                        case "DocumentTypeCode":
                            {
                                documentTypeCodeList = peruAttributes.Value;
                                break;
                            }
                        case "DiscrepancyResponse":
                            {
                                DiscrepancyResponseCodeList = peruAttributes.Value;
                                break;
                            }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Map Peru Attributes Error", ex);
                log.LogMethodExit(null, "Throwing Map Peru Attributes Exception-" + ex.Message);
                throw;
            }
            log.LogMethodExit();
        }

        private void BuildPeruAttributes()
        {
            log.LogMethodEntry();
            try
            {
                List<LookupValuesDTO> invoiceAttributesValuesDTOList;
                List<LookupValuesDTO> invoiceSetupValuesDTOList;
                LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                Dictionary<string, Dictionary<string, string>> PeruAttributesDictionary = new Dictionary<string, Dictionary<string, string>>();

                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> invoiceSetupSearchParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                invoiceSetupSearchParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "FISCAL_INVOICE_SETUP"));
                invoiceSetupValuesDTOList = lookupValuesList.GetAllLookupValues(invoiceSetupSearchParameters);

                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> invoiceAttributesSearchParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                invoiceAttributesSearchParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "FISCAL_INVOICE_ATTRIBUTES"));
                invoiceAttributesValuesDTOList = lookupValuesList.GetAllLookupValues(invoiceAttributesSearchParameters);

                foreach (LookupValuesDTO lookupValuesDTO in invoiceAttributesValuesDTOList)
                {
                    if (lookupValuesDTO.Description.Contains("|"))
                    {
                        PeruAttributesDictionary = GeneratePeruAttributesList(lookupValuesDTO);
                    }
                    if (PeruAttributesDictionary != null && PeruAttributesDictionary.Any())
                    {
                        MapPeruAttributes(PeruAttributesDictionary);
                    }
                }
                //CurrencyCode
                //currencyCode = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CURRENCY_CODE");
                //currencyId = new XAttribute("currencyID", currencyCode);

                //RUCOfIssuer
                rucNumber = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "TAX_IDENTIFICATION_NUMBER");
                log.LogVariableState("rucNumber", rucNumber);
                //UBLVersionID
                LookupValuesDTO lookupValuesUBLVersionIDDTO = invoiceAttributesValuesDTOList.FirstOrDefault(p => p.LookupValue == "UBLVersionID");
                if (lookupValuesUBLVersionIDDTO != null)
                {
                    uBLVersionID = lookupValuesUBLVersionIDDTO.Description;
                }
                log.LogVariableState("uBLVersionID", uBLVersionID);
                //CustomizationID
                LookupValuesDTO lookupValuesCustomizationIDDTO = invoiceAttributesValuesDTOList.FirstOrDefault(p => p.LookupValue == "CustomizationID");
                if (lookupValuesCustomizationIDDTO != null)
                {
                    customizationID = lookupValuesCustomizationIDDTO.Description;
                }
                log.LogVariableState("customizationID", customizationID);
                //CustomizationAgencyName
                LookupValuesDTO lookupValuesCustomizationAgencyNameDTO = invoiceAttributesValuesDTOList.FirstOrDefault(p => p.LookupValue == "CustomizationAgencyName");
                if (lookupValuesCustomizationAgencyNameDTO != null)
                {
                    customizationAgencyName = lookupValuesCustomizationAgencyNameDTO.Description;
                }
                log.LogVariableState("customizationAgencyName", customizationAgencyName);
                //Boleta
                LookupValuesDTO lookupValuesBoletaAndDNIDTO = invoiceSetupValuesDTOList.FirstOrDefault(p => p.LookupValue == "BoletaAndDNIValues");
                if (lookupValuesBoletaAndDNIDTO != null)
                {
                    string[] values = lookupValuesBoletaAndDNIDTO.Description.Split('|');
                    boletaValue = values[0];
                    dniValue = values[1];
                }
                log.LogVariableState("boletaValue", boletaValue);
                log.LogVariableState("dniValue", dniValue);
                //Factura
                LookupValuesDTO lookupValuesFacturaAndRUCDTO = invoiceSetupValuesDTOList.FirstOrDefault(p => p.LookupValue == "FacturaAndRUCValues");
                if (lookupValuesFacturaAndRUCDTO != null)
                {
                    string[] values = lookupValuesFacturaAndRUCDTO.Description.Split('|');
                    facturaValue = values[0];
                    rucValue = values[1];
                }
                log.LogVariableState("facturaValue", facturaValue);
                log.LogVariableState("rucValue", rucValue);

                //CreditNote
                LookupValuesDTO lookupValuesCreditNoteDTO = invoiceSetupValuesDTOList.FirstOrDefault(p => p.LookupValue == "CreditNote");
                if (lookupValuesCreditNoteDTO != null)
                {
                    creditNoteValue = lookupValuesCreditNoteDTO.Description;
                }
                log.LogVariableState("creditNoteValue", creditNoteValue);

                //Date Format
                LookupValuesDTO lookupValuesDateFormatDTO = invoiceSetupValuesDTOList.FirstOrDefault(p => p.LookupValue == "InvoiceDate_Format");
                if (lookupValuesDateFormatDTO != null)
                {
                    dateFormat = lookupValuesDateFormatDTO.Description;
                    if (string.IsNullOrWhiteSpace(dateFormat))
                    {
                        dateFormat = "yyyy-MM-dd";
                    }
                }
                log.LogVariableState("dateFormat", dateFormat);
                //Time Format
                LookupValuesDTO lookupValuesTimeFormatDTO = invoiceSetupValuesDTOList.FirstOrDefault(p => p.LookupValue == "InvoiceTime_Format");
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
                LookupValuesDTO lookupValuesIntegerFormatDTO = invoiceSetupValuesDTOList.FirstOrDefault(p => p.LookupValue == "InvoiceInteger_Format");
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
                LookupValuesDTO lookupValuesamountFormatDTO = invoiceSetupValuesDTOList.FirstOrDefault(p => p.LookupValue == "InvoiceAmount_Format");
                if (lookupValuesamountFormatDTO != null)
                {
                    amountFormat = lookupValuesamountFormatDTO.Description;
                    if (string.IsNullOrWhiteSpace(amountFormat))
                    {
                        amountFormat = "####.00";
                    }
                }
                log.LogVariableState("amountFormat", amountFormat);
                //CultureInfo for amount
                LookupValuesDTO lookupValuesCultureInfoDTO = invoiceSetupValuesDTOList.FirstOrDefault(p => p.LookupValue == "CultureInfo");
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
                //Currency_Code
                LookupValuesDTO lookupValuesCurrencyCodeDTO = invoiceSetupValuesDTOList.FirstOrDefault(p => p.LookupValue == "Currency_Code");
                if (lookupValuesCurrencyCodeDTO != null)
                {
                    currencyCode = lookupValuesCurrencyCodeDTO.Description;
                    if (string.IsNullOrWhiteSpace(currencyCode))
                    {
                        currencyCode = "PEN";
                    }
                }
                log.LogVariableState("currencyCode", currencyCode);
                //Boleta_File_Upload_Path
                LookupValuesDTO lookupValuesBoletaFileUploadPathDTO = invoiceSetupValuesDTOList.FirstOrDefault(p => p.LookupValue == "Boleta_File_Upload_Path");
                if (lookupValuesBoletaFileUploadPathDTO != null)
                {
                    boleta_File_Upload_Path = lookupValuesBoletaFileUploadPathDTO.Description;
                }
                log.LogVariableState("boleta_File_Upload_Path", boleta_File_Upload_Path);

                //Factura_File_Upload_Path
                LookupValuesDTO lookupValuesFacturaFileUploadPathDTO = invoiceSetupValuesDTOList.FirstOrDefault(p => p.LookupValue == "Factura_File_Upload_Path");
                if (lookupValuesFacturaFileUploadPathDTO != null)
                {
                    factura_File_Upload_Path = lookupValuesFacturaFileUploadPathDTO.Description;
                }
                log.LogVariableState("factura_File_Upload_Path", factura_File_Upload_Path);

                //CreditNote_File_Upload_Path
                LookupValuesDTO lookupValuesCreditNoteFileUploadPathDTO = invoiceSetupValuesDTOList.FirstOrDefault(p => p.LookupValue == "CreditNote_File_Upload_Path");
                if (lookupValuesCreditNoteFileUploadPathDTO != null)
                {
                    creditNote_File_Upload_Path = lookupValuesCreditNoteFileUploadPathDTO.Description;
                }
                log.LogVariableState("creditNote_File_Upload_Path", creditNote_File_Upload_Path);
                //SignatureId
                LookupValuesDTO lookupValuesSignatureIdDTO = invoiceAttributesValuesDTOList.FirstOrDefault(p => p.LookupValue == "SignatureId");
                if (lookupValuesSignatureIdDTO != null)
                {
                    signatureId = lookupValuesSignatureIdDTO.Description;
                }
                log.LogVariableState("signatureId", signatureId);
                LookupValuesDTO lookupValuesSignatureIDSchmeIdDTO = invoiceAttributesValuesDTOList.FirstOrDefault(p => p.LookupValue == "SignatureIDSchmeId");
                if (lookupValuesSignatureIDSchmeIdDTO != null)
                {
                    signatureIDSchmeId = lookupValuesSignatureIDSchmeIdDTO.Description;
                }
                log.LogVariableState("signatureIDSchmeId", signatureIDSchmeId);
                LookupValuesDTO lookupValuesSignatureIDSchmeNameDTO = invoiceAttributesValuesDTOList.FirstOrDefault(p => p.LookupValue == "SignatureIDSchmeName");
                if (lookupValuesSignatureIDSchmeNameDTO != null)
                {
                    signatureIDSchmeName = lookupValuesSignatureIDSchmeNameDTO.Description;
                }
                log.LogVariableState("signatureIDSchmeName", signatureIDSchmeName);
                LookupValuesDTO lookupValuesSignatureIDSchmeAgencyDTO = invoiceAttributesValuesDTOList.FirstOrDefault(p => p.LookupValue == "SignatureIDSchmeAgency");
                if (lookupValuesSignatureIDSchmeAgencyDTO != null)
                {
                    signatureIDSchmeAgency = lookupValuesSignatureIDSchmeAgencyDTO.Description;
                }
                log.LogVariableState("signatureIDSchmeAgency", signatureIDSchmeAgency);
                LookupValuesDTO lookupValuesSignatureIDSchmeURIDTO = invoiceAttributesValuesDTOList.FirstOrDefault(p => p.LookupValue == "SignatureIDSchmeURI");
                if (lookupValuesSignatureIDSchmeURIDTO != null)
                {
                    signatureIDSchmeURI = lookupValuesSignatureIDSchmeURIDTO.Description;
                }
                log.LogVariableState("signatureIDSchmeURI", signatureIDSchmeURI);
                //TaxSchemeId 
                LookupValuesDTO lookupValuesTaxSchemeIdDTO = invoiceAttributesValuesDTOList.FirstOrDefault(p => p.LookupValue == "TaxSchemeId");
                if (lookupValuesTaxSchemeIdDTO != null)
                {
                    taxSchemeId = lookupValuesTaxSchemeIdDTO.Description;
                }
                log.LogVariableState("taxSchemeId", taxSchemeId);

                //zeroTaxSchemeId 
                LookupValuesDTO lookupValuesZeroTaxSchemeIdDTO = invoiceAttributesValuesDTOList.FirstOrDefault(p => p.LookupValue == "ZeroTaxSchemeId");
                if (lookupValuesZeroTaxSchemeIdDTO != null)
                {
                    zeroTaxSchemeId = lookupValuesZeroTaxSchemeIdDTO.Description;
                }
                log.LogVariableState("zeroTaxSchemeId", zeroTaxSchemeId);
                //DiscrepancyResponseCode 
                LookupValuesDTO lookupValuesDiscrepancyResponseCodeDTO = invoiceAttributesValuesDTOList.FirstOrDefault(p => p.LookupValue == "DiscrepancyResponseCode");
                if (lookupValuesDiscrepancyResponseCodeDTO != null)
                {
                    discrepancyResponseCode = lookupValuesDiscrepancyResponseCodeDTO.Description;
                }
                log.LogVariableState("discrepancyResponseCode", discrepancyResponseCode);
                //DiscrepancyResponseCode 
                LookupValuesDTO lookupValuesdiscrepancyResponseCodeDescriptionDTO = invoiceAttributesValuesDTOList.FirstOrDefault(p => p.LookupValue == "DiscrepancyResponseCodeDescription");
                if (lookupValuesdiscrepancyResponseCodeDescriptionDTO != null)
                {
                    discrepancyResponseCodeDescription = lookupValuesdiscrepancyResponseCodeDescriptionDTO.Description;
                }
                log.LogVariableState("discrepancyResponseCodeDescription", discrepancyResponseCodeDescription);
                //PricingReferencePriceTypeCodeCodeContent 
                LookupValuesDTO lookupValuesPricingReferencePriceTypeCodeCodeContentDTO = invoiceAttributesValuesDTOList.FirstOrDefault(p => p.LookupValue == "PricingReferenceAlternativeConditionPricePriceTypeCodeCodeContent");
                if (lookupValuesPricingReferencePriceTypeCodeCodeContentDTO != null)
                {
                    pricingReferencePriceTypeCodeCodeContent = lookupValuesPricingReferencePriceTypeCodeCodeContentDTO.Description;
                }
                log.LogVariableState("pricingReferencePriceTypeCodeCodeContent", pricingReferencePriceTypeCodeCodeContent);
                //TaxTypeCode 
                LookupValuesDTO lookupValuesTaxTypeCodeDTO = invoiceAttributesValuesDTOList.FirstOrDefault(p => p.LookupValue == "TaxTypeCode");
                if (lookupValuesTaxTypeCodeDTO != null)
                {
                    taxTypeCode = lookupValuesTaxTypeCodeDTO.Description;
                }
                log.LogVariableState("taxTypeCode", taxTypeCode);
                //ZeroTaxTypeCode 
                LookupValuesDTO lookupValuesZeroTaxTypeCodeDTO = invoiceAttributesValuesDTOList.FirstOrDefault(p => p.LookupValue == "ZeroTaxTypeCode");
                if (lookupValuesZeroTaxTypeCodeDTO != null)
                {
                    zeroTaxTypeCode = lookupValuesZeroTaxTypeCodeDTO.Description;
                }
                log.LogVariableState("zeroTaxTypeCode", zeroTaxTypeCode);
                //TaxSchemeName 
                LookupValuesDTO lookupValuesTaxSchemeNameDTO = invoiceAttributesValuesDTOList.FirstOrDefault(p => p.LookupValue == "TaxSchemeName");
                if (lookupValuesTaxSchemeNameDTO != null)
                {
                    taxSchemeName = lookupValuesTaxSchemeNameDTO.Description;
                }
                log.LogVariableState("taxSchemeName", taxSchemeName);
                //ZeroTaxSchemeName 
                LookupValuesDTO lookupValuesZeroTaxSchemeNameDTO = invoiceAttributesValuesDTOList.FirstOrDefault(p => p.LookupValue == "ZeroTaxSchemeName");
                if (lookupValuesZeroTaxSchemeNameDTO != null)
                {
                    zeroTaxSchemeName = lookupValuesZeroTaxSchemeNameDTO.Description;
                }
                log.LogVariableState("zeroTaxSchemeName", zeroTaxSchemeName);
                //TaxExemptionReasonCodeValue 
                LookupValuesDTO lookupValuesTaxExemptionReasonCodeValueDTO = invoiceAttributesValuesDTOList.FirstOrDefault(p => p.LookupValue == "TaxExemptionReasonCodeValue");
                if (lookupValuesTaxExemptionReasonCodeValueDTO != null)
                {
                    taxExemptionReasonCodeValue = lookupValuesTaxExemptionReasonCodeValueDTO.Description;
                }
                log.LogVariableState("taxExemptionReasonCodeValue", taxExemptionReasonCodeValue);
                //ExternalReferenceUri
                LookupValuesDTO lookupValuesExternalReferenceUriDTO = invoiceAttributesValuesDTOList.FirstOrDefault(p => p.LookupValue == "ExternalReferenceUri");
                if (lookupValuesExternalReferenceUriDTO != null)
                {
                    externalReferenceUri = lookupValuesExternalReferenceUriDTO.Description;
                }
                log.LogVariableState("externalReferenceUri", externalReferenceUri);
                //AccountingSPName
                LookupValuesDTO lookupValuesAccountingSPNameDTO = invoiceAttributesValuesDTOList.FirstOrDefault(p => p.LookupValue == "AccountingSPName");
                if (lookupValuesAccountingSPNameDTO != null)
                {
                    accountingSPName = lookupValuesAccountingSPNameDTO.Description;
                }
                log.LogVariableState("accountingSPName", accountingSPName);
                //AccountingSPRegistrationName 
                LookupValuesDTO lookupValuesAccountingSPRegistrationNameDTO = invoiceAttributesValuesDTOList.FirstOrDefault(p => p.LookupValue == "AccountingSPRegistrationName");
                if (lookupValuesAccountingSPRegistrationNameDTO != null)
                {
                    accountingSPRegistrationName = lookupValuesAccountingSPRegistrationNameDTO.Description;
                }
                log.LogVariableState("accountingSPRegistrationName", accountingSPRegistrationName);
                //AccountingSPRegistrationAddressId
                LookupValuesDTO lookupValuesAccountingSPRegistrationAddressIdDTO = invoiceAttributesValuesDTOList.FirstOrDefault(p => p.LookupValue == "AccountingSPRegistrationAddressId");
                if (lookupValuesAccountingSPRegistrationAddressIdDTO != null)
                {
                    accountingSPRegistrationAddressId = lookupValuesAccountingSPRegistrationAddressIdDTO.Description;
                }
                log.LogVariableState("accountingSPRegistrationAddressId", accountingSPRegistrationAddressId);
                //AccountingSPRegistrationIDSchemeAgency
                LookupValuesDTO lookupValuesAccountingSPRegistrationIDSchemeAgencyDTO = invoiceAttributesValuesDTOList.FirstOrDefault(p => p.LookupValue == "AccountingSPRegistrationIDSchemeAgency");
                if (lookupValuesAccountingSPRegistrationIDSchemeAgencyDTO != null)
                {
                    accountingSPRegistrationIDSchemeAgency = lookupValuesAccountingSPRegistrationIDSchemeAgencyDTO.Description;
                }
                log.LogVariableState("accountingSPRegistrationIDSchemeAgency", accountingSPRegistrationIDSchemeAgency);
                //AccountingSPRegistrationIDSchemeName
                LookupValuesDTO lookupValuesAccountingSPRegistrationIDSchemeNameDTO = invoiceAttributesValuesDTOList.FirstOrDefault(p => p.LookupValue == "AccountingSPRegistrationIDSchemeName");
                if (lookupValuesAccountingSPRegistrationIDSchemeNameDTO != null)
                {
                    accountingSPRegistrationIDSchemeName = lookupValuesAccountingSPRegistrationIDSchemeNameDTO.Description;
                }
                log.LogVariableState("accountingSPRegistrationIDSchemeName", accountingSPRegistrationIDSchemeName);
                //AccountingSPLine
                LookupValuesDTO lookupValuesAccountingSPLineDTO = invoiceAttributesValuesDTOList.FirstOrDefault(p => p.LookupValue == "AccountingSPLine");
                if (lookupValuesAccountingSPLineDTO != null)
                {
                    accountingSPLine = lookupValuesAccountingSPLineDTO.Description;
                }
                log.LogVariableState("accountingSPLine", accountingSPLine);
                //AccountingSPId
                LookupValuesDTO lookupValuesaccountingSPIdDTO = invoiceAttributesValuesDTOList.FirstOrDefault(p => p.LookupValue == "AccountingSPIdentificationCode");
                if (lookupValuesaccountingSPIdDTO != null)
                {
                    accountingSPIdentificationCode = lookupValuesaccountingSPIdDTO.Description;
                }
                log.LogVariableState("accountingSPIdentificationCode", accountingSPIdentificationCode);
                //AccountingSPCountryCodeListId
                LookupValuesDTO lookupValuesAccountingSPCountryCodeListIdDTO = invoiceAttributesValuesDTOList.FirstOrDefault(p => p.LookupValue == "AccountingSPCountryCodeListId");
                if (lookupValuesAccountingSPCountryCodeListIdDTO != null)
                {
                    accountingSPCountryCodeListId = lookupValuesAccountingSPCountryCodeListIdDTO.Description;
                }
                log.LogVariableState("accountingSPCountryCodeListId", accountingSPCountryCodeListId);
                //AccountingSPCountryCodeListAgency 
                LookupValuesDTO lookupValuesAccountingSPCountryCodeListAgencyDTO = invoiceAttributesValuesDTOList.FirstOrDefault(p => p.LookupValue == "AccountingSPCountryCodeListAgency");
                if (lookupValuesAccountingSPCountryCodeListAgencyDTO != null)
                {
                    accountingSPCountryCodeListAgency = lookupValuesAccountingSPCountryCodeListAgencyDTO.Description;
                }
                log.LogVariableState("accountingSPCountryCodeListAgency", accountingSPCountryCodeListAgency);
                //AccountingSPCountryCodeListName  
                LookupValuesDTO lookupValuesAccountingSPCountryCodeListNameDTO = invoiceAttributesValuesDTOList.FirstOrDefault(p => p.LookupValue == "AccountingSPCountryCodeListName");
                if (lookupValuesAccountingSPCountryCodeListNameDTO != null)
                {
                    accountingSPCountryCodeListName = lookupValuesAccountingSPCountryCodeListNameDTO.Description;
                }
                log.LogVariableState("accountingSPCountryCodeListName", accountingSPCountryCodeListName);
                //AccountingSPDistrict
                LookupValuesDTO lookupValuesAccountingSPDistrictDTO = invoiceAttributesValuesDTOList.FirstOrDefault(p => p.LookupValue == "AccountingSPDistrict");
                if (lookupValuesAccountingSPDistrictDTO != null)
                {
                    accountingSPDistrict = lookupValuesAccountingSPDistrictDTO.Description;
                }
                log.LogVariableState("accountingSPDistrict", accountingSPDistrict);
                //AccountingSPCountrySubentity
                LookupValuesDTO lookupValuesAccountingSPCountrySubentityDTO = invoiceAttributesValuesDTOList.FirstOrDefault(p => p.LookupValue == "AccountingSPCountrySubentity");
                if (lookupValuesAccountingSPCountrySubentityDTO != null)
                {
                    accountingSPCountrySubentity = lookupValuesAccountingSPCountrySubentityDTO.Description;
                }
                log.LogVariableState("accountingSPCountrySubentity", accountingSPCountrySubentity);
                //AccountingSPCitySubdivisionName
                LookupValuesDTO lookupValuesAccountingSPCitySubdivisionNameDTO = invoiceAttributesValuesDTOList.FirstOrDefault(p => p.LookupValue == "AccountingSPCitySubdivisionName");
                if (lookupValuesAccountingSPCitySubdivisionNameDTO != null)
                {
                    accountingSPCitySubdivisionName = lookupValuesAccountingSPCitySubdivisionNameDTO.Description;
                }
                log.LogVariableState("accountingSPCitySubdivisionName", accountingSPCitySubdivisionName);
                //AccountingSPCityName
                LookupValuesDTO lookupValuesAccountingSPCityNameDTO = invoiceAttributesValuesDTOList.FirstOrDefault(p => p.LookupValue == "AccountingSPCityName");
                if (lookupValuesAccountingSPCityNameDTO != null)
                {
                    accountingSPCityName = lookupValuesAccountingSPCityNameDTO.Description;
                }
                log.LogVariableState("accountingSPCityName", accountingSPCityName);
                //AccountingSPEmailId
                LookupValuesDTO lookupValuesAccountingSPEmailIdDTO = invoiceAttributesValuesDTOList.FirstOrDefault(p => p.LookupValue == "AccountingSPEmailId");
                if (lookupValuesAccountingSPEmailIdDTO != null)
                {
                    accountingSPEmailId = lookupValuesAccountingSPEmailIdDTO.Description;
                }
                log.LogVariableState("accountingSPEmailId", accountingSPEmailId);
                //AddressTypeCode
                LookupValuesDTO lookupValuesaddressTypeCodeDTO = invoiceAttributesValuesDTOList.FirstOrDefault(p => p.LookupValue == "AddressTypeCode");
                if (lookupValuesaddressTypeCodeDTO != null)
                {
                    addressTypeCode = lookupValuesaddressTypeCodeDTO.Description;
                }
                log.LogVariableState("addressTypeCode", addressTypeCode);
                ////AccountingCPId
                //LookupValuesDTO lookupValuesAccountingCPIdDTO = invoiceAttributesValuesDTOList.FirstOrDefault(p => p.LookupValue == "AccountingCPId");
                //if (lookupValuesAccountingCPIdDTO != null)
                //{
                //    accountingCPId = lookupValuesAccountingCPIdDTO.Description;
                //}
                //AccountingCPIdentificationCode
                LookupValuesDTO lookupValuesAccountingCPIdentificationCodeDTO = invoiceAttributesValuesDTOList.FirstOrDefault(p => p.LookupValue == "AccountingCPIdentificationCode");
                if (lookupValuesAccountingCPIdentificationCodeDTO != null)
                {
                    accountingCPIdentificationCode = lookupValuesAccountingCPIdentificationCodeDTO.Description;
                }
                log.LogVariableState("accountingCPIdentificationCode", accountingCPIdentificationCode);
                //AccountingCPCountryCodeListId
                LookupValuesDTO lookupValuesAccountingCPCountryCodeListIdDTO = invoiceAttributesValuesDTOList.FirstOrDefault(p => p.LookupValue == "AccountingCPCountryCodeListId");
                if (lookupValuesAccountingCPCountryCodeListIdDTO != null)
                {
                    accountingCPCountryCodeListId = lookupValuesAccountingCPCountryCodeListIdDTO.Description;
                }
                log.LogVariableState("accountingCPCountryCodeListId", accountingCPCountryCodeListId);
                //AccountingCPCountryCodeListAgency
                LookupValuesDTO lookupValuesAccountingCPCountryCodeListAgencyDTO = invoiceAttributesValuesDTOList.FirstOrDefault(p => p.LookupValue == "AccountingCPCountryCodeListAgency");
                if (lookupValuesAccountingCPCountryCodeListAgencyDTO != null)
                {
                    accountingCPCountryCodeListAgency = lookupValuesAccountingCPCountryCodeListAgencyDTO.Description;
                }
                log.LogVariableState("accountingCPCountryCodeListAgency", accountingCPCountryCodeListAgency);
                //AccountingCPCountryCodeListName
                LookupValuesDTO lookupValuesAccountingCPCountryCodeListNameDTO = invoiceAttributesValuesDTOList.FirstOrDefault(p => p.LookupValue == "AccountingCPCountryCodeListName");
                if (lookupValuesAccountingCPCountryCodeListNameDTO != null)
                {
                    accountingCPCountryCodeListName = lookupValuesAccountingCPCountryCodeListNameDTO.Description;
                }
                log.LogVariableState("accountingCPCountryCodeListName", accountingCPCountryCodeListName);
                //AllowanceChargeIndicator
                LookupValuesDTO lookupValuesAllowanceChargeIndicatorDTO = invoiceAttributesValuesDTOList.FirstOrDefault(p => p.LookupValue == "AllowanceChargeIndicator");
                if (lookupValuesAllowanceChargeIndicatorDTO != null)
                {
                    allowanceChargeIndicator = lookupValuesAllowanceChargeIndicatorDTO.Description;
                }
                log.LogVariableState("allowanceChargeIndicator", allowanceChargeIndicator);
                //AllowanceChargeReasonCodeValue
                LookupValuesDTO lookupValuesAllowanceChargeReasonCodeValueDTO = invoiceAttributesValuesDTOList.FirstOrDefault(p => p.LookupValue == "AllowanceChargeReasonCodeValue");
                if (lookupValuesAllowanceChargeReasonCodeValueDTO != null)
                {
                    allowanceChargeReasonCodeValue = lookupValuesAllowanceChargeReasonCodeValueDTO.Description;
                }
                log.LogVariableState("allowanceChargeReasonCodeValue", allowanceChargeReasonCodeValue);
            }
            catch (Exception ex)
            {
                log.Error("Build Peru Attributes Error", ex);
                log.LogMethodExit(null, "Throwing Build Peru Attributes Exception-" + ex.Message);
                throw new Exception(ex.Message);
            }
            log.LogMethodExit();
        }

        private List<TransactionDTO> GetEligibleInvoiceRecords(List<int> trxIdList)
        {
            log.LogMethodEntry(trxIdList);
            List<TransactionDTO> transactionDTOList = new List<TransactionDTO>();
            try
            {
                string trxIdValues = GetIDString(trxIdList);
                if (utilities != null && utilities.ParafaitEnv != null)
                {
                    siteName = utilities.ParafaitEnv.SiteName;
                }
                List<LookupValuesContainerDTO> lookupValuesDTOList = GetEligibleOverrideOptions();
                if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
                {
                    string overrideOptions = lookupValuesDTOList[0].Description;
                    TransactionListBL transactionListBL = new TransactionListBL(executionContext);
                    List<KeyValuePair<TransactionDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<TransactionDTO.SearchByParameters, string>>();
                    searchParams.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                    searchParams.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.POS_OVERRIDE_OPTION_NAME_LIST, overrideOptions));
                    searchParams.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.TRANSACTION_ID_LIST, trxIdValues));
                    searchParams.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.STATUS, Transaction.Transaction.TrxStatus.CLOSED.ToString()));
                    transactionDTOList = transactionListBL.GetTransactionDTOList(searchParams, utilities, null, 0, 5000);
                }
                else
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2933, "Peru Invoice"));//&1 setup details are missing
                }
            }
            catch (Exception ex)
            {
                log.Error("Get Eligible Invoice Records Error", ex);
                throw;
            }
            log.LogMethodExit(transactionDTOList);
            return transactionDTOList;
        }

        private static string GetIDString(List<int> trxIdList)
        {
            log.LogMethodEntry();
            StringBuilder stringBuilder = new StringBuilder("");
            string trxIdValues = string.Empty;
            if (trxIdList != null && trxIdList.Any())
            {
                for (int i = 0; i < trxIdList.Count; i++)
                {
                    if (i == trxIdList.Count - 1)
                    {
                        stringBuilder.Append(trxIdList[i]);
                    }
                    else
                    {
                        stringBuilder.Append(trxIdList[i]);
                        stringBuilder.Append(",");
                    }
                }
                trxIdValues = stringBuilder.ToString();
            }
            log.LogMethodExit(trxIdValues);
            return trxIdValues;
        }

        private List<ExSysSynchLogDTO> PostFiles(string folderPath)
        {
            log.LogMethodEntry(folderPath);
            List<ExSysSynchLogDTO> folderFileLogList = new List<ExSysSynchLogDTO>();
            if (System.IO.Directory.Exists(folderPath))
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);
                var xmlFiles = directoryInfo.GetFiles().Where(f => f.Extension.ToLower() == XML_FILE_EXTENSION).OrderBy(t => t.LastWriteTime).ToList();
                if (xmlFiles != null)
                {
                    log.LogVariableState("xmlFiles.Count", xmlFiles.Count);
                    for (int i = 0; i < xmlFiles.Count; i++)
                    {
                        string fileName = xmlFiles[i].FullName;
                        try
                        {
                            log.Info("Processing file no: " + (i + 1));
                            DateTime lastUpdateTime = xmlFiles[i].LastWriteTime;
                            fileName = VerifyFileForUpdates(directoryInfo, fileName, lastUpdateTime);
                            if (string.IsNullOrWhiteSpace(fileName))
                            {
                                string msg = "Skipping " + xmlFiles[0].FullName + " as VerifyFileForUpdates() failed ";
                                string msg1 = "Error processing " + xmlFiles[0].FullName;
                                ValidationException validationException = new ValidationException(msg);
                                log.Info(msg);
                                ExSysSynchLogDTO logDTO = LogError(validationException, msg1, POST_INVOICE);
                                if (logDTO != null)
                                {
                                    folderFileLogList.Add(logDTO);
                                }
                            }
                            else
                            {

                                List<ExSysSynchLogDTO> trxLogList = PostInvoice(fileName);
                                if (trxLogList != null && trxLogList.Any())
                                {
                                    folderFileLogList.AddRange(trxLogList);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                            string msg = "Error while posting " + fileName + " :";
                            ExSysSynchLogDTO logDTO = LogError(ex, msg, POST_INVOICE);
                            if (logDTO != null)
                            {
                                folderFileLogList.Add(logDTO);
                            }
                        }
                    }
                }
            }
            log.LogMethodExit(folderFileLogList);
            return folderFileLogList;
        }

        public override List<ExSysSynchLogDTO> PostInvoice(string invoicefileNamewithPath)
        {
            log.LogMethodEntry(invoicefileNamewithPath);
            List<ExSysSynchLogDTO> trxLogList = new List<ExSysSynchLogDTO>();
            using (NoSynchronizationContextScope.Enter())
            {
                string fileName = Path.GetFileName(invoicefileNamewithPath);
                Task<List<ExSysSynchLogDTO>> task = PostXML(invoicefileNamewithPath, fileName);
                task.Wait();
                trxLogList = task.Result;
            }
            log.LogMethodExit(trxLogList);
            return trxLogList;
        }

        private async Task<List<ExSysSynchLogDTO>> PostXML(string fileNameWithPath, string fileName)
        {
            log.LogMethodEntry(fileNameWithPath, fileName);
            List<ExSysSynchLogDTO> trxLogList = new List<ExSysSynchLogDTO>();
            int trxId = -1;
            string trxGuid = string.Empty;
            TransactionDTO transactionDTO = null;
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12;
                string xmlFileContent = File.ReadAllText(fileNameWithPath);
                transactionDTO = GetTransactionDTOfromRequest(xmlFileContent);
                if (transactionDTO != null)
                {
                    trxId = transactionDTO.TransactionId;
                    trxGuid = transactionDTO.Guid;
                }
                log.LogVariableState("xmlFileContent", xmlFileContent);
                log.LogVariableState("postURLForInvoice", postURLForInvoice);

                using (HttpClient client = new HttpClient())
                {
                    client.Timeout = System.Threading.Timeout.InfiniteTimeSpan;
                    var nvc = new List<KeyValuePair<string, string>>();
                    nvc.Add(new KeyValuePair<string, string>("username", userName));
                    nvc.Add(new KeyValuePair<string, string>("password", password));
                    nvc.Add(new KeyValuePair<string, string>("grant_type", "password"));
                    var req = new HttpRequestMessage(HttpMethod.Post, postURLForToken) { Content = new FormUrlEncodedContent(nvc) };
                    req.Headers.Add("Authorization", peruAuthKey);
                    HttpResponseMessage res = await client.SendAsync(req);
                    if (res != null && res.IsSuccessStatusCode)
                    {
                        string response = await res.Content.ReadAsStringAsync();
                        JObject jObject = (JObject)JsonConvert.DeserializeObject(response);

                        if (jObject["access_token"] != null)
                        {
                            string token = jObject["access_token"].ToString();
                            string result = "";
                            using (var formContent = new MultipartFormDataContent())
                            {
                                Stream fileStream = System.IO.File.OpenRead(fileNameWithPath);
                                formContent.Add(new StreamContent(fileStream), "file", fileName);
                                using (var clients = new HttpClient())
                                {
                                    clients.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                                    {
                                        var message = await clients.PostAsync(postURLForInvoice, formContent);
                                        log.LogVariableState("message", message);
                                        result = await message.Content.ReadAsStringAsync();
                                        log.LogVariableState("result", result);
                                        if (message != null && message.IsSuccessStatusCode)
                                        {
                                            //result = await message.Content.ReadAsStringAsync();
                                            string trxExternalSystemRefNo = UpdateExternalSystemReference(result, transactionDTO);
                                            log.LogVariableState("XML post completed for", fileName);
                                            MovePostedFiles(fileNameWithPath, fileName);


                                            ExSysSynchLogDTO exSysSynchLogDTO = new ExSysSynchLogDTO(logId: -1, timestamp: ServerDateTime.Now, exSysName: EX_SYS_NAME,
                                              parafaitObject: TRANSACTION, parafaitObjectId: trxId, parafaitObjectGuid: trxGuid,
                                             isSuccessFul: true, status: SUCCESS_STATUS, data: trxExternalSystemRefNo, remarks: this.programName, concurrentRequestId: this.concurrentRequestId);
                                            ExSysSynchLogBL exSysSynchLogBL = new ExSysSynchLogBL(executionContext, exSysSynchLogDTO);
                                            exSysSynchLogBL.Save();
                                            trxLogList.Add(exSysSynchLogBL.ExSysSynchLogDTO);
                                        }
                                        else
                                        {
                                            MovePostedFiles(fileNameWithPath, fileName, true);
                                            string errorMsg = "StatusCode: " + message.StatusCode + " Error: " + result;
                                            log.Info("Skipping " + fileName + " as PostXML() failed " + errorMsg);
                                            throw new Exception(errorMsg);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        string result = await res.Content.ReadAsStringAsync();
                        log.LogVariableState("Authorization failure", res);
                        string errorMsg = "StatusCode: " + res.StatusCode + " Error: " + result;
                        log.Info("Skipping " + fileName + " as PostXML() failed " + errorMsg);
                        throw new Exception(errorMsg);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                string msg = "Error while posting " + trxId + " :";
                ExSysSynchLogDTO logDTO = LogError(ex, msg, TRANSACTION, trxId, trxGuid);
                if (logDTO != null)
                {
                    trxLogList.Add(logDTO);
                }
            }
            log.LogMethodExit(trxLogList);
            return trxLogList;
        }

        private void setMoveFolderPath()
        {
            log.LogMethodEntry();
            List<LookupValuesContainerDTO> lookupValuesDTOList = new List<LookupValuesContainerDTO>(invoiceSetupLookupValueDTOList);
            LookupValuesContainerDTO lookupValuesDTO = null;
            try
            {
                if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
                {
                    lookupValuesDTO = lookupValuesDTOList.Where(lv => string.IsNullOrWhiteSpace(lv.LookupValue) == false
                                                                       && lv.LookupValue == (MOVE_PATH_STRING)).FirstOrDefault();
                    moveFolderPath = lookupValuesDTO.Description;
                    if (string.IsNullOrWhiteSpace(moveFolderPath))
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2933, "Peru Invoice Move Folder Path"));//&1 setup details are missing
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Set Move Folder Path Error", ex);
                throw;
            }
            log.LogMethodExit(moveFolderPath);
        }

        private List<LookupValuesContainerDTO> GetOutFolderPathInfo()
        {
            log.LogMethodEntry();
            List<LookupValuesContainerDTO> lookupValuesDTOList = new List<LookupValuesContainerDTO>(invoiceSetupLookupValueDTOList);
            if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
            {
                lookupValuesDTOList = lookupValuesDTOList.Where(lv => string.IsNullOrWhiteSpace(lv.LookupValue) == false
                                                                    && lv.LookupValue.Contains(OUT_PATH_PARTIAL_STRING)).ToList();
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
                var xmlFileTemp = directoryInfo.GetFiles().Where(f => f.FullName == fileName).OrderBy(t => t.LastWriteTime).ToList();
                if (xmlFileTemp.Count == 0)
                {
                    unexpectedIssue++;
                }
                else
                {
                    if (xmlFileTemp[0].FullName == fileName && xmlFileTemp[0].LastWriteTime == lastUpdateTime)
                    {
                        processFile = true;
                    }
                    else
                    {
                        lastUpdateTime = xmlFileTemp[0].LastWriteTime;
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

        private TransactionDTO GetTransactionDTOfromRequest(string xmlContent)
        {
            log.LogMethodEntry(xmlContent);
            TransactionDTO transactionDTO = null;
            try
            {
                string trxNumber = string.Empty;
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlContent);
                XmlNamespaceManager nsManager = new XmlNamespaceManager(xmlDoc.NameTable);
                nsManager.AddNamespace("cbc", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2");

                string tagName = "cbc:ID";
                XmlNode xmlNode = xmlDoc.SelectSingleNode("//" + tagName, nsManager);
                if (xmlNode != null)
                {
                    trxNumber = xmlNode.InnerText;
                }
                else
                {
                    XmlNodeList dataElements = xmlDoc.GetElementsByTagName("cbc:ID");
                    if (dataElements.Count > 0)
                    {
                        trxNumber = dataElements[0].InnerText;
                    }
                }
                if (string.IsNullOrWhiteSpace(trxNumber) == false)
                {
                    transactionDTO = GetTransactionDTO(trxNumber);
                }
                else
                {
                    log.Info("Unable to fetch trx no from xml content");
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
            string externalSystemReferenceNumber = string.Empty;
            try
            {
                //Fetching externalSystemReferenceNumber
                if (result != null && result.ToLower().Contains("description"))
                {
                    var jo = JObject.Parse(result);
                    if (jo != null && jo.Count > 1)
                    {
                        externalSystemReferenceNumber = jo["description"].ToString();
                    }
                }
                log.LogVariableState("externalSystemReferenceNumber", externalSystemReferenceNumber);
                //Fetching Transaction number & updating the transactionDTO with externalSystemReferenceNumber
                UpdateTrxExternalSystemRefNumber(transactionDTO, externalSystemReferenceNumber);
            }
            catch (Exception ex)
            {
                log.Error("Error while Updating External System Reference", ex);
                throw new Exception(MessageContainerList.GetMessage(executionContext, 4223) + " : " + ex.Message);//Error while Updating External System Reference
            }
            log.LogMethodExit(externalSystemReferenceNumber);
            return externalSystemReferenceNumber;
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

        private TransactionDTO GetTransactionDTO(string trxNumber)
        {
            log.LogMethodEntry(trxNumber);
            TransactionDTO transactionDTO = null;
            try
            {
                TransactionUtils transactionUtils = new TransactionUtils(utilities);
                List<TransactionDTO> transactionDTOList;
                TransactionListBL transactionListBL = new TransactionListBL(executionContext);
                List<KeyValuePair<TransactionDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<TransactionDTO.SearchByParameters, string>>();
                searchParams.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                searchParams.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.TRANSACTION_NUMBER, trxNumber));
                searchParams.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.STATUS, Transaction.Transaction.TrxStatus.CLOSED.ToString()));
                transactionDTOList = transactionListBL.GetTransactionDTOList(searchParams, utilities, null, 0, 5000);
                if (transactionDTOList != null && transactionDTOList.Any())
                {
                    transactionDTOList = transactionDTOList.OrderByDescending(t => t.TransactionId).ToList();
                    transactionDTO = transactionDTOList[0];
                }
                else
                {
                    throw new ValidationException("No Transaction Data found for transaction number " + trxNumber);
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

        private void MovePostedFiles(string fileNameWithPath, string fileName, bool postFailed = false)
        {
            log.LogMethodEntry(fileNameWithPath, fileName);
            if (!Directory.Exists(moveFolderPath))
            {
                Directory.CreateDirectory(moveFolderPath);
            }
            string directoryPath = Path.GetDirectoryName(fileNameWithPath) + @"\";
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileNameWithPath);

            string stringTimestamp = System.DateTime.Now.ToString(utilities.ParafaitEnv.DATETIME_FORMAT).Replace("-", "").Replace(" ", "").Replace(":", "").Replace(".", "").Replace(",", "");
            string xmlFileName = (postFailed ? "Errored_" : "") + stringTimestamp + "_" + fileName;
            try
            {
                File.Move(fileNameWithPath, moveFolderPath + xmlFileName);
            }
            catch (Exception ex)
            {
                log.Error(fileName, ex);
            }
            string textFileName = fileNameWithoutExtension + TXT_FILE_EXTENSION;
            string destinationTextFileName = (postFailed ? "Errored_" : "") + stringTimestamp + "_" + textFileName;
            try
            {
                File.Move(directoryPath + textFileName, moveFolderPath + destinationTextFileName);
            }
            catch (Exception ex)
            {
                log.Error(textFileName, ex);
            }
            string codBarTextFileName = fileNameWithoutExtension + "CodBar.txt";
            string destinationCodBarFileName = (postFailed ? "Errored_" : "") + stringTimestamp + "_" + codBarTextFileName;
            try
            {
                File.Move(directoryPath + codBarTextFileName, moveFolderPath + destinationCodBarFileName);
            }
            catch (Exception ex)
            {
                log.Error(codBarTextFileName, ex);
            }
            string codBarPngFileName = fileNameWithoutExtension + "CodBar.png";
            string destinationCodBarPngFileName = (postFailed ? "Errored_" : "") + stringTimestamp + "_" + codBarPngFileName;
            try
            {
                File.Move(directoryPath + codBarPngFileName, moveFolderPath + destinationCodBarPngFileName);
            }
            catch (Exception ex)
            {
                log.Error(codBarPngFileName, ex);
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

        private string GetUserName()
        {
            log.LogMethodEntry();
            string value = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, USERNAMELOOKUPVALUENAME);
            log.LogMethodExit(value);
            return value;
        }

        private string GetUserPassword()
        {
            log.LogMethodEntry();
            string value = GetLookupValueDescription(USERPASSWORDLOOKUPVALUENAME);
            log.LogMethodExit(value);
            return value;
        }

        private string GetPeruAuthenticationKey()
        {
            log.LogMethodEntry();
            string value = GetLookupValueDescription(INVOICEAUTHKEYLOOKUPVALUENAME);
            log.LogMethodExit(value);
            return value;
        }

        private string GetPostURLForToken()
        {
            log.LogMethodEntry();
            string value = GetLookupValueDescription(POSTURLFORTOKENLOOKUPVALUENAME);
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

        private int GetTrxLineCount(Transaction.Transaction transaction)
        {
            log.LogMethodEntry("transaction");
            int countValue = 1;
            try
            {
                if (transaction.TransactionLineList != null && transaction.TransactionLineList.Any())
                {
                    List<Semnox.Parafait.Transaction.Transaction.TransactionLine> trxLines = transaction.TransactionLineList.Where(tl => tl.LineAmount != 0).ToList();
                    if (trxLines != null && trxLines.Any()) //ignore zero price lines
                    {
                        countValue = 0;
                        foreach (Transaction.Transaction.TransactionLine item in trxLines)
                        {
                            decimal lineAmount = GetDiscountedLineAmount(item);
                            if (lineAmount != 0)
                            {
                                countValue = countValue + 1;
                            }
                        }
                        //countValue = trxLines.Count();
                    }
                }
            }
            catch (Exception exx)
            {
                log.Error(exx);
            }
            log.LogMethodExit(countValue);
            return countValue;
        }


        private List<TransactionDTO> CheckForProcessedReversalTrx(List<TransactionDTO> transactionDTOList)
        {
            log.LogMethodEntry("transactionDTOList");
            List<TransactionDTO> trxList = new List<TransactionDTO>();
            List<int> reversalTrxIds = new List<int>();
            if (transactionDTOList != null && transactionDTOList.Any())
            {
                foreach (TransactionDTO item in transactionDTOList)
                {
                    if (item.OriginalTransactionId > 0)
                    {
                        string zeroTrxCode = ZERO_PRICE_TRX + "_" + item.TransactionId;
                        string hundDisxTrxCode = HUNDRED_PERC_DISCOUNT_TRX + "_" + item.TransactionId;
                        if (zeroTrxCode != item.ExternalSystemReference
                                    && hundDisxTrxCode != item.ExternalSystemReference)
                        {
                            reversalTrxIds.Add(item.TransactionId);
                        }
                    }
                    else
                    {
                        trxList.Add(item);
                    }
                }
                if (reversalTrxIds != null && reversalTrxIds.Any())
                {
                    ExSysSynchLogListBL exSysSynchLogListBL = new ExSysSynchLogListBL(executionContext);
                    string parafaitObjectName = TRANSACTION;
                    string status = SUCCESS_STATUS;
                    string remarks = "InvoicePostXMLProgram";
                    List<ExSysSynchLogDTO> logDTOList = exSysSynchLogListBL.GetExSysSynchLogDTOList(parafaitObjectName, status, remarks, reversalTrxIds);

                    foreach (TransactionDTO item in transactionDTOList)
                    {
                        if (item.OriginalTransactionId > 0 && reversalTrxIds.Exists(t => t == item.TransactionId))
                        {
                            if (logDTOList == null || logDTOList.Any() == false ||
                                logDTOList.Exists(l => l.ParafaitObject == TRANSACTION && l.ParafaitObjectId == item.TransactionId && l.ParafaitObjectGuid == item.Guid
                                                       && l.Status == status && l.Remarks == remarks) == false)
                            {
                                trxList.Add(item);
                            }
                        }
                    }
                }
            }
            log.LogMethodExit(trxList);
            return trxList;
        }
    }
}
