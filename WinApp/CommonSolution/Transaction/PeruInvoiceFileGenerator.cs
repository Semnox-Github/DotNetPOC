/********************************************************************************************
 * Project Name - Peru Invoice File Generator
 * Description  - Child calss of  Generator Invoice File Generator
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By      Remarks          
 *********************************************************************************************
 *2.110.0     05-Jan-2021      Dakshakh Raj     Created for Peru Invoice Enhancement - Parafait Job changes
 ********************************************************************************************/
using Newtonsoft.Json;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Languages;
using Semnox.Parafait.POS;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// PeruInvoiceFileGenerator
    /// </summary>
    public class PeruInvoiceFileGenerator : InvoiceFileGenerator
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //private Utilities utilities;
        private SqlTransaction sqlTransaction;

        private string peruResultString = string.Empty;
        private string boletaHeaderTemplate = string.Empty;
        private string facturaHeaderTemplate = string.Empty;
        private string invoiceLineTemplate = string.Empty;
        private string invoiceLegalTemplate = string.Empty;
        private string invoiceTaxTotalTemplate = string.Empty;
        private string invoiceDiscountTemplate = string.Empty;
        private string invoiceDiscountTotalTemplate = string.Empty;

        private string creditNoteHeaderTemplate = string.Empty;
        private string creditNoteLineTemplate = string.Empty;
        private string creditNoteLegalTemplate = string.Empty;
        private string creditNoteTaxTotalTemplate = string.Empty;

        private string uBLVersionID = string.Empty;
        private string customizationID = string.Empty;
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
        private string signatureId = string.Empty;
        private string rucNumber = string.Empty;
        private string externalReferenceUri = string.Empty;
        private string accountingSPName = string.Empty;
        private string accountingSPRegistrationName = string.Empty;
        private string accountingSPRegistrationAddressId = string.Empty;
        private string accountingSPLine = string.Empty;
        private string accountingSPIdentificationCode = string.Empty;
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
        ProfileDTO profileDTO;
        private string accountingCPName = string.Empty;
        private string accountingCPRegistraionId = string.Empty;
        private string accountingCPCitySubdivisionName = string.Empty;
        private string accountingCPCityName = string.Empty;
        private string accountingCPCountrySubentity = string.Empty;
        private string accountingCPDistrict = string.Empty;
        private string accountingCPAddressLine = string.Empty;
        private string accountingCPIdentificationCode = string.Empty;
        private string accountingCPMailId = string.Empty;
        private string accountingCPDNI = string.Empty;
        private string accountingCPRUC = string.Empty;
        private string taxSchemeId = string.Empty;
        private string discrepancyResponseCode = string.Empty;
        private string taxTypeCode = string.Empty;
        private string taxSchemeName = string.Empty;
        private string taxExemptionReasonCodeValue = string.Empty;
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

        private decimal taxableAmountValue;
        private decimal taxAmountValue;

        Dictionary<string, string> invoiceTypeCodeAttributeList = new Dictionary<string, string>();
        Dictionary<string, string> documentCurrencyCodeAttributeList = new Dictionary<string, string>();
        Dictionary<string, string> accountingCPList = new Dictionary<string, string>();
        Dictionary<string, string> accountingSPList = new Dictionary<string, string>();
        Dictionary<string, string> accountingSPAddressTypeCodeList = new Dictionary<string, string>();
        Dictionary<string, string> allowanceChargeReasonCodeList = new Dictionary<string, string>();
        Dictionary<string, string> documentCurrencyCodeList = new Dictionary<string, string>();
        Dictionary<string, string> invoicedQuantityList = new Dictionary<string, string>();
        Dictionary<string, string> invoiceTypeCodeList = new Dictionary<string, string>();
        Dictionary<string, string> itemClassificationCodeList = new Dictionary<string, string>();
        Dictionary<string, string> priceTypeCodeList = new Dictionary<string, string>();
        Dictionary<string, string> taxExemptionReasonCodeList = new Dictionary<string, string>();
        Dictionary<string, string> taxSchemeList = new Dictionary<string, string>();
        Dictionary<string, string> documentTypeCodeList = new Dictionary<string, string>();
        Dictionary<string, string> DiscrepancyResponseCodeList = new Dictionary<string, string>();

        private const string PERU_BOLETA_HEADER = "PERU_BOLETA_HEADER";
        private const string PERU_FACTURA_HEADER = "PERU_FACTURA_HEADER";
        private const string PERU_INVOICE_LINE = "PERU_INVOICE_LINE";
        private const string PERU_INVOICE_TAX_TOTAL = "PERU_INVOICE_TAX_TOTAL";
        private const string PERU_INVOICE_LEGAL = "PERU_INVOICE_LEGAL";
        private const string PERU_INVOICE_DISCOUNT = "PERU_INVOICE_DISCOUNT";
        private const string PERU_INVOICE_DISCOUNT_TOTAL = "PERU_INVOICE_DISCOUNT_TOTAL";

        private const string PERU_CREDIT_NOTE_HEADER = "PERU_CREDIT_NOTE_HEADER";
        private const string PERU_CREDIT_NOTE_LINE = "PERU_CREDIT_NOTE_LINE";
        private const string PERU_CREDIT_NOTE_TAX_TOTAL = "PERU_CREDIT_NOTE_TAX_TOTAL";
        private const string PERU_CREDIT_NOTE_LEGAL = "PERU_CREDIT_NOTE_LEGAL";

        private const string BOLETA = "Boleta";
        private const string FACTURA = "Factura";
        /// <summary>
        /// PeruInvoiceFileGenerator
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="utilities"></param>
        /// <param name="sqlTransaction"></param>
        public PeruInvoiceFileGenerator(ExecutionContext executionContext, Utilities utilities, SqlTransaction sqlTransaction)
            : base(executionContext, utilities)
        {
            log.LogMethodEntry(executionContext, sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            BuildPeruAttributes();
            SetInvoiceTemplates();
            SetCreditNoteTemplates();
            log.LogMethodExit(null);
        }

        /// <summary>
        /// GetEligibleInvoiceRecords
        /// </summary> 
        /// <param name="currentTime"></param>
        /// <param name="LastRunTime"></param>
        /// <returns></returns>
        public override List<TransactionDTO> GetEligibleInvoiceRecords(DateTime currentTime, DateTime LastRunTime)
        {
            log.LogMethodEntry(currentTime, LastRunTime);
            List<TransactionDTO> transactionDTOList = new List<TransactionDTO>();
            try
            {
                if (utilities != null && utilities.ParafaitEnv != null)
                {
                    siteName = utilities.ParafaitEnv.SiteName;
                }
                List<LookupValuesDTO> lookupValuesDTOList = GetEligibleOverrideOptions();
                if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
                {
                    string overrideOptions = lookupValuesDTOList[0].Description;
                    TransactionListBL transactionListBL = new TransactionListBL(executionContext);
                    List<KeyValuePair<TransactionDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<TransactionDTO.SearchByParameters, string>>();
                    searchParams.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                    searchParams.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.POS_OVERRIDE_OPTION_NAME_LIST, overrideOptions));
                    searchParams.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.LAST_UPDATE_FROM_TIME, LastRunTime.ToString("yyyy-MM-dd HH:mm:ss")));
                    searchParams.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.LAST_UPDATE_TO_TIME, currentTime.ToString("yyyy-MM-dd HH:mm:ss")));
                    transactionDTOList = transactionListBL.GetTransactionDTOList(searchParams, utilities, sqlTransaction, 0, 5000);
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

        private List<LookupValuesDTO> GetEligibleOverrideOptions()
        {
            log.LogMethodEntry();
            LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
            List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
            searchParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            searchParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "FISCAL_INVOICE_SETUP"));
            searchParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, "EligibleOverrideOptions"));
            List<LookupValuesDTO> lookupValuesDTOList = lookupValuesList.GetAllLookupValues(searchParameters, sqlTransaction);
            log.LogMethodExit(lookupValuesDTOList);
            return lookupValuesDTOList;
        }

        private string GetInvoiceType(Transaction trx)
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

        /// <summary>
        /// build ProfileDTO
        /// </summary>
        /// <param name="trx"></param>
        private void BuildProfileDTO(Transaction trx)
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

        /// <summary>
        /// BuildInvoiceFile
        /// </summary>
        /// <param name="trx"></param>
        public override void BuildInvoiceFile(Transaction trx)
        {
            log.LogMethodEntry();
            try
            {
                BuildProfileDTO(trx);
                string result = string.Empty;
                string invoiceType = GetInvoiceType(trx);
                bool isReversal = false;
                decimal discountTotal = 0;
                if (invoiceType == BOLETA || invoiceType == FACTURA)
                {
                    SetInvoiceTypeAttributes(invoiceType);
                    string headerString = BuildHeaderSection(trx, invoiceType, isReversal);
                    string lineDetailsElement = BuildTaxAndLegalSummaryDetailSection(trx, invoiceType, isReversal);
                    result = headerString + lineDetailsElement;
                    string ivoiceLineElement = "\"InvoiceLine\"" + " : [";
                    string invoiceResultLine = string.Empty;
                    int loopCount = 0;
                    foreach (Transaction.TransactionLine trxLine in trx.TransactionLineList)
                    {
                        loopCount++;
                        invoiceResultLine = invoiceResultLine + BuildInvoiceLineSection(trxLine);
                        invoiceResultLine = invoiceResultLine.Replace("@DiscountSection", BuildLineDiscountValues(trxLine));
                        if (trxLine.TransactionDiscountsDTOList != null && trxLine.TransactionDiscountsDTOList.Any())
                        {
                            //discountTotal = discountTotal + (((decimal)trxLine.Price * (decimal)trxLine.TransactionDiscountsDTOList.Sum(td => td.DiscountPercentage) / 100));//(decimal)trxLine.TransactionDiscountsDTOList.Sum(td => td.DiscountAmount);
                            discountTotal = discountTotal + (((Convert.ToDecimal(((decimal)trxLine.Price).ToString(amountFormat, invC), invC)) * Convert.ToDecimal(((decimal)trxLine.TransactionDiscountsDTOList.Sum(td => td.DiscountPercentage)).ToString(amountFormat, invC), invC) / 100));
                        }
                        if (trx.TransactionLineList.Count > loopCount && trx.TransactionLineList.Count != 1)
                        {
                            invoiceResultLine = invoiceResultLine + ",";
                        }
                    }
                    result = result.Replace("@DiscountTotalSection", BuildLineDiscountTotalValues(discountTotal));
                    result = result + ivoiceLineElement + invoiceResultLine;
                    result = result + "]}]}";
                    System.IO.File.WriteAllText(fileUploadPath + rucNumber + "-" + invoiceTypeCode + "-" + trx.Trx_No + ".json", result);
                }
                else
                {
                    isReversal = true;
                    Transaction originalTrx = GetOriginalTrx(trx);
                    CalculateTaxDetails(originalTrx);
                    invoiceType = GetInvoiceType(trx.OriginalTrxId);
                    SetInvoiceTypeAttributes(invoiceType);
                    fileUploadPath = creditNote_File_Upload_Path;
                    string headerString = BuildHeaderSection(trx, invoiceType, isReversal);
                    string lineDetailsElement = BuildTaxAndLegalSummaryDetailSection(trx, invoiceType, isReversal);
                    result = headerString + lineDetailsElement;
                    string creditNoteElement = "\"CreditNoteLine\"" + " : [";
                    string creditResultLine = string.Empty;
                    int loopCount = 0;
                    if (originalTrx != null && originalTrx.TransactionLineList != null && originalTrx.TransactionLineList.Any())
                    {
                        foreach (Transaction.TransactionLine trxLine in originalTrx.TransactionLineList)
                        {
                            loopCount++;
                            creditResultLine = creditResultLine + BuildCreditNoteLineSection(trxLine);
                            if (originalTrx.TransactionLineList.Count > loopCount && originalTrx.TransactionLineList.Count != 1)
                            {
                                creditResultLine = creditResultLine + ",";
                            }
                        }
                    }
                    result = result + creditNoteElement + creditResultLine;
                    result = result + "]}]}";
                    System.IO.File.WriteAllText(fileUploadPath + rucNumber + "-" + creditNoteValue + "-" + trx.Trx_No + ".json", result);
                }
                //System.IO.File.WriteAllText(@"C:\Users\Public\TestFolder\WriteText.txt", result);
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error("Build Invoice Files Error", ex);
                log.LogMethodExit(null, "Throwing Build Invoice Files Exception-" + ex.Message);
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Build Line Discount Total Values
        /// </summary>
        /// <param name="discountTotal"></param>
        /// <returns></returns>
        private string BuildLineDiscountTotalValues(decimal discountTotal)
        {
            log.LogMethodEntry(discountTotal);
            try
            {
                string discountTotalSection = string.Empty;
                if (discountTotal > 0)
                {
                    discountTotalSection = invoiceDiscountTotalTemplate;
                    discountTotalSection = discountTotalSection.Replace("@discountTotal", discountTotal.ToString(amountFormat, invC));
                }
                log.LogMethodExit(discountTotalSection);
                return discountTotalSection;
            }
            catch (Exception ex)
            {
                log.Error("Build Line Discount Total Values Error", ex);
                log.LogMethodExit(null, "Throwing Build Line Discount Total Values Exception-" + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Get Original TrxData
        /// </summary>
        /// <param name="trx"></param>
        private Transaction GetOriginalTrx(Transaction trx)
        {
            log.LogMethodEntry(trx);
            try
            {
                Transaction originalTrx = null;
                if (trx != null && trx.OriginalTrxId != null)
                {
                    List<TransactionDTO> transactionDTOList = new List<TransactionDTO>();
                    TransactionListBL transactionListBL = new TransactionListBL(executionContext);
                    List<KeyValuePair<TransactionDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<TransactionDTO.SearchByParameters, string>>();
                    searchParams.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                    searchParams.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.TRANSACTION_ID, (trx.OriginalTrxId).ToString()));
                    transactionDTOList = transactionListBL.GetTransactionDTOList(searchParams, null, sqlTransaction, 0, 5000);
                    if (transactionDTOList != null && transactionDTOList.Any())
                    {
                        TransactionUtils transactionUtils = new TransactionUtils(utilities);
                        originalTrx = transactionUtils.CreateTransactionFromDB(transactionDTOList[0].TransactionId, utilities);

                        billingReferenceID = transactionDTOList[0].TransactionNumber;

                        billingReferenceIssueDate = transactionDTOList[0].TransactionDate.ToString(dateFormat);
                    }
                }
                return originalTrx;
            }
            catch (Exception ex)
            {
                log.Error("Get Original Trx Data Error", ex);
                log.LogMethodExit(null, "Throwing Exception in Getting Original Trx Data" + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Build Header Section
        /// </summary>
        /// <param name="trx">trx</param>
        /// <param name="invoiceType">invoiceType</param>
        /// <returns></returns>
        public override string BuildHeaderSection(Transaction trx, string invoiceType, bool isReversal)
        {
            log.LogMethodEntry();
            try
            {
                string headerSection = string.Empty;
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
                log.LogMethodExit(headerSection);
                return headerSection;
            }
            catch (Exception ex)
            {
                log.Error("Build Header Section Error", ex);
                log.LogMethodExit(null, "Throwing Exception in Build Header Section" + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Build Invoice Header values
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="headerSection"></param>
        private string BuildInvoiceHeadervalues(Transaction transaction, string headerSection)
        {
            log.LogMethodEntry(transaction);
            try
            {
                headerSection = headerSection.Replace("@UBLVersionId", uBLVersionID);
                headerSection = headerSection.Replace("@CustomizationId", customizationID);
                headerSection = headerSection.Replace("@InvoiceId", transaction.Trx_No);

                headerSection = headerSection.Replace("@IssueTime", (transaction.TrxDate).ToString(timeFormat));
                headerSection = headerSection.Replace("@IssueDate", (transaction.TrxDate.ToString(dateFormat)));
                headerSection = headerSection.Replace("@DueDate", (transaction.TrxDate.ToString(dateFormat)));
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
                headerSection = headerSection.Replace("@SignatureIDContent", signatureId);
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
                headerSection = headerSection.Replace("@ACPMailID", accountingCPMailId);
                log.LogMethodExit(headerSection);
                return headerSection;
            }
            catch (Exception ex)
            {
                log.Error("Build Invoice Header values Error", ex);
                log.LogMethodExit(null, "Throwing Exception in Build Invoice Header values" + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Build CreditNote Header Values
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="headerSection"></param>
        private string BuildCreditNoteHeadervalues(Transaction transaction)
        {
            log.LogMethodEntry();
            try
            {
                string headerSection = creditNoteHeaderTemplate;
                headerSection = headerSection.Replace("@UBLVersionId", uBLVersionID);
                headerSection = headerSection.Replace("@CustomizationId", customizationID);
                headerSection = headerSection.Replace("@InvoiceId", transaction.Trx_No);
                headerSection = headerSection.Replace("@IssueDate", (transaction.TrxDate.ToString(dateFormat)));
                headerSection = headerSection.Replace("@IssueTime", (transaction.TrxDate).ToString(timeFormat));
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
                headerSection = headerSection.Replace("@SignatureIDContent", signatureId);
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
                headerSection = headerSection.Replace("@ACPMailID", accountingCPMailId);
                log.LogMethodExit(headerSection);
                return headerSection;
            }
            catch (Exception ex)
            {
                log.Error("Build Credit Note Header values Error", ex);
                log.LogMethodExit(null, "Throwing Exception in Build Credit Note Header values" + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// BuildTaxAndLegalSummaryDetailSection
        /// </summary>
        /// <param name="trx"></param>
        /// <param name="invoiceType"></param>
        /// <returns></returns>
        public override string BuildTaxAndLegalSummaryDetailSection(Transaction trx, string invoiceType, bool isReversal = false)
        {
            log.LogMethodEntry();
            try
            {
                string linesDetailSection = string.Empty;
                if (isReversal)
                {
                    string taxTotalSection = string.Empty;
                    string legalMonetarySection = string.Empty;
                    taxTotalSection = BuildCreditNoteTaxTotalValues(trx);
                    legalMonetarySection = BuildCreditNoteLegalMonetaryValues(trx);
                    linesDetailSection = taxTotalSection + legalMonetarySection;
                }
                else if (invoiceType == "Boleta" || invoiceType == "Factura")
                {
                    string taxTotalSection = string.Empty;
                    string legalMonetarySection = string.Empty;
                    taxTotalSection = BuildInvoiceTaxTotalValues(trx);
                    legalMonetarySection = BuildInvoiceLegalMonetaryValues(trx);
                    linesDetailSection = taxTotalSection + legalMonetarySection;
                }
                return linesDetailSection;
            }
            catch (Exception ex)
            {
                log.Error("Build Lines Detail Section Error", ex);
                log.LogMethodExit(null, "Throwing Build Lines Detail Section Exception-" + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Build Invoice TaxTotal Values
        /// </summary>
        /// <param name="trx"></param>
        /// <param name="linesDetailSection"></param>
        /// <returns></returns>
        private string BuildInvoiceTaxTotalValues(Transaction trx)
        {
            log.LogMethodEntry();
            try
            {
                string linesDetailSection = invoiceTaxTotalTemplate;
                CalculateTaxDetails(trx);
                linesDetailSection = linesDetailSection.Replace("@TaxTotalAmountContent", taxAmountValue.ToString(amountFormat, invC));//(trx.Tax_Amount).ToString())//pending
                linesDetailSection = linesDetailSection.Replace("@TaxTotalAmountCurrencyID", currencyCode);
                linesDetailSection = linesDetailSection.Replace("@TaxTotalAmountTaxableAmountContent", taxableAmountValue.ToString(amountFormat, invC));// (trx.Net_Transaction_Amount - trx.Tax_Amount).ToString());
                linesDetailSection = linesDetailSection.Replace("@TaxTotalAmountCurrencyID", currencyCode);
                linesDetailSection = linesDetailSection.Replace("@TaxTotalTaxSubtotalAmountContent", taxAmountValue.ToString(amountFormat, invC));//(trx.Tax_Amount).ToString());
                linesDetailSection = linesDetailSection.Replace("@TaxTotalTaxSubtotalAmountCurrencyID", currencyCode);

                linesDetailSection = linesDetailSection.Replace("@TaxTotalTaxSubtotalTaxCategoryID", taxSchemeId);
                if (taxSchemeList != null && taxSchemeList.Any())
                {
                    foreach (KeyValuePair<string, string> entry in taxSchemeList)
                    {
                        linesDetailSection = linesDetailSection.Replace(entry.Key.ToString(), entry.Value);
                    }
                }
                linesDetailSection = linesDetailSection.Replace("@TaxTotalTaxSubtotalTaxCategoryName", taxSchemeName);
                linesDetailSection = linesDetailSection.Replace("@TaxTotalTaxSubtotalTaxCategoryTaxTypeCode", taxTypeCode);

                linesDetailSection = linesDetailSection.Replace("@LegalMonetaryTotalLineExtensionAmountAmountContent", taxableAmountValue.ToString(amountFormat, invC));//(trx.Transaction_Amount - trx.Tax_Amount).ToString());
                linesDetailSection = linesDetailSection.Replace("@CurrencyID", currencyCode);
                linesDetailSection = linesDetailSection.Replace("@LegalMonetaryTotalLineExtensionAmountTaxInclusiveAmountAmountContent", (trx.Net_Transaction_Amount).ToString(amountFormat, invC));
                linesDetailSection = linesDetailSection.Replace("@LegalMonetaryTotalLineExtensionAmountTaxInclusiveAmountPayableAmountAmountContent", (trx.Net_Transaction_Amount).ToString(amountFormat, invC));


                return linesDetailSection;
            }
            catch (Exception ex)
            {
                log.Error("Build Invoice TaxTotal Values Error", ex);
                log.LogMethodExit(null, "Throwing Build Invoice TaxTotal Values Exception-" + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Calculate Taxable Amount
        /// </summary>
        /// <param name="trx"></param>
        /// <returns></returns>
        private void CalculateTaxDetails(Transaction trx)
        {
            log.LogMethodEntry(trx);
            decimal taxableAmount = 0;
            decimal taxAmount = 0;
            try
            {
                if (trx != null && trx.TransactionLineList != null && trx.TransactionLineList.Any())
                {
                    foreach (Transaction.TransactionLine trxLine in trx.TransactionLineList)
                    {
                        decimal amount;
                        if (trxLine.TransactionDiscountsDTOList != null && trxLine.TransactionDiscountsDTOList.Any())
                        {
                            //amount = (decimal)trxLine.Price - (((decimal)trxLine.Price * (decimal)trxLine.TransactionDiscountsDTOList.Sum(td => td.DiscountPercentage)) / 100);
                            amount = Convert.ToDecimal(trxLine.Price) - ((Convert.ToDecimal(trxLine.Price)) * (Convert.ToDecimal(((decimal)(trxLine.TransactionDiscountsDTOList.Sum(td => td.DiscountPercentage))), invC)) / 100);
                        }
                        else
                        {
                            //amount = (decimal)trxLine.Price);
                            amount = Convert.ToDecimal(((decimal)trxLine.Price), invC);
                        }
                        taxableAmount = taxableAmount + amount;
                        if (trxLine.tax_percentage > 0)
                        {
                            //taxAmount = taxAmount + ((amount * (decimal)trxLine.tax_percentage) / 100);
                            taxAmount = taxAmount + (amount * ((Convert.ToDecimal(((decimal)trxLine.tax_percentage), invC) / 100)));

                        }
                    }
                }
                taxableAmountValue = taxableAmount;
                taxAmountValue = taxAmount;
            }
            catch (Exception ex)
            {
                log.Error("Build Invoice Get taxable amount", ex);
                log.LogMethodExit(null, "Throwing Get taxable amount Exception-" + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Build Invoice Legal Monetary Values
        /// </summary>
        /// <param name="trx"></param>
        /// <param name="linesDetailSection"></param>
        /// <returns></returns>
        private string BuildInvoiceLegalMonetaryValues(Transaction trx)
        {
            log.LogMethodEntry();
            try
            {
                string linesDetailSection = invoiceLegalTemplate;
                linesDetailSection = linesDetailSection.Replace("@LegalMonetaryTotalLineExtensionAmountAmountContent", taxableAmountValue.ToString(amountFormat, invC));
                linesDetailSection = linesDetailSection.Replace("@CurrencyID", currencyCode);
                linesDetailSection = linesDetailSection.Replace("@LegalMonetaryTotalLineExtensionAmountTaxInclusiveAmountAmountContent", (trx.Net_Transaction_Amount).ToString(amountFormat, invC));
                linesDetailSection = linesDetailSection.Replace("@LegalMonetaryTotalLineExtensionAmountTaxInclusiveAmountPayableAmountAmountContent", (trx.Net_Transaction_Amount).ToString(amountFormat, invC));
                return linesDetailSection;
            }
            catch (Exception ex)
            {
                log.Error("Build Invoice Legal Monetary Values Error", ex);
                log.LogMethodExit(null, "Throwing Build Invoice Legal Monetary Va Exception-" + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Build CreditNote TaxTotal Values
        /// </summary>
        /// <param name="trx"></param>
        /// <param name="linesDetailSection"></param>
        /// <returns></returns>
        private string BuildCreditNoteTaxTotalValues(Transaction trx)
        {
            try
            {
                string linesDetailSection = creditNoteTaxTotalTemplate;
                linesDetailSection = linesDetailSection.Replace("@TaxTotalAmountContent", taxAmountValue.ToString(amountFormat, invC));
                linesDetailSection = linesDetailSection.Replace("@TaxTotalAmountCurrencyID", currencyCode);
                linesDetailSection = linesDetailSection.Replace("@TaxSubtotalTaxableAmountAmountContent", taxableAmountValue.ToString(amountFormat, invC));
                linesDetailSection = linesDetailSection.Replace("@TaxTotalTaxSubtotalAmountContent", taxAmountValue.ToString(amountFormat, invC));
                linesDetailSection = linesDetailSection.Replace("@TaxTotalTaxSubtotalAmountCurrencyID", currencyCode);

                linesDetailSection = linesDetailSection.Replace("@TaxTotalTaxSubtotalTaxCategoryID", taxSchemeId);
                if (taxSchemeList != null && taxSchemeList.Any())
                {
                    foreach (KeyValuePair<string, string> entry in taxSchemeList)
                    {
                        linesDetailSection = linesDetailSection.Replace(entry.Key.ToString(), entry.Value);
                    }
                }
                linesDetailSection = linesDetailSection.Replace("@TaxTotalTaxSubtotalTaxCategoryName", taxSchemeName);
                linesDetailSection = linesDetailSection.Replace("@TaxTotalTaxSubtotalTaxCategoryTaxTypeCode", taxTypeCode);
                return linesDetailSection;
            }
            catch (Exception ex)
            {
                log.Error("Build CreditNote TaxTotal Values Error", ex);
                log.LogMethodExit(null, "Throwing Build CreditNote TaxTotal Values Exception-" + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Build CreditNote LegalMonetary Values
        /// </summary>
        /// <param name="trx"></param>
        /// <returns></returns>
        private string BuildCreditNoteLegalMonetaryValues(Transaction trx)
        {
            log.LogMethodEntry();
            try
            {
                string linesDetailSection = creditNoteLegalTemplate;
                linesDetailSection = linesDetailSection.Replace("@LegalMonetaryTotalLineExtensionAmountAmountContent", taxableAmountValue.ToString(amountFormat, invC));
                linesDetailSection = linesDetailSection.Replace("@CurrencyID", currencyCode);
                linesDetailSection = linesDetailSection.Replace("@LegalMonetaryTotalLineExtensionAmountTaxInclusiveAmountAmountContent", Math.Abs(trx.Net_Transaction_Amount).ToString(amountFormat, invC));
                linesDetailSection = linesDetailSection.Replace("@LegalMonetaryTotalLineExtensionAmountTaxInclusiveAmountPayableAmountAmountContent", Math.Abs(trx.Net_Transaction_Amount).ToString(amountFormat, invC));
                return linesDetailSection;
            }
            catch (Exception ex)
            {
                log.Error("Build Build CreditNote Legal Monetary Values Error", ex);
                log.LogMethodExit(null, "Throwing Build CreditNote Legal Monetary Values Exception-" + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Build InvoiceLine Section
        /// </summary>
        /// <param name="trxLine">trxLine</param>
        /// <returns></returns>
        public override string BuildInvoiceLineSection(Transaction.TransactionLine trxLine)
        {
            log.LogMethodEntry(trxLine);
            string str = invoiceLineTemplate;
            decimal lineExtensionAmount;
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
                    //lineExtensionAmount = ((decimal)trxLine.Price - (((decimal)trxLine.Price * (decimal)trxLine.TransactionDiscountsDTOList.Sum(td => td.DiscountPercentage)) / 100)).ToString(amountFormat, invC);
                    lineExtensionAmount = Convert.ToDecimal(((decimal)trxLine.Price) - (Convert.ToDecimal((decimal)trxLine.Price) * Convert.ToDecimal((decimal)trxLine.TransactionDiscountsDTOList.Sum(td => td.DiscountPercentage), invC) / 100));
                }
                else
                {
                lineExtensionAmount = Convert.ToDecimal(trxLine.Price);
            }
            str = str.Replace("@ILLineExtensionAmountAmountContent", lineExtensionAmount.ToString(amountFormat, invC));
            str = str.Replace("@ILLineExtensionAmountAmountCurrencyID", currencyCode);
            PricingReferenceAmountContent = (Convert.ToDecimal(lineExtensionAmount, invC) + ((Convert.ToDecimal(lineExtensionAmount, invC) * Convert.ToDecimal((((decimal)trxLine.tax_percentage)), invC) / 100)));
            str = str.Replace("@ILPricingReferenceAlternativeConditionPricePriceAmountAmountContent", PricingReferenceAmountContent.ToString(amountFormat, invC));
            str = str.Replace("@ILPricingReferenceAlternativeConditionPricePriceAmountAmountCurrencyID", currencyCode);

                str = str.Replace("@ILPricingReferenceAlternativeConditionPricePriceTypeCodeCodeContent", pricingReferencePriceTypeCodeCodeContent);
                if (priceTypeCodeList != null && priceTypeCodeList.Any())
                {
                    foreach (KeyValuePair<string, string> entry in priceTypeCodeList)
                    {
                        str = str.Replace(entry.Key.ToString(), entry.Value);
                    }
                }
                //TaxTotal
                taxAmountAmount = (Convert.ToDecimal(lineExtensionAmount, invC) * Convert.ToDecimal(((decimal)trxLine.tax_percentage).ToString(), invC) / 100);
                str = str.Replace("@ILTaxTotalTaxAmountAmountContent", taxAmountAmount.ToString(amountFormat, invC));
                str = str.Replace("@ILTaxTotalTaxAmountAmountCurrencyID", currencyCode);

                str = str.Replace("@ILTaxTotalTaxSubtotalTaxableAmountAmountContent", lineExtensionAmount.ToString(amountFormat, invC));//(trxLine.Price).ToString());
                str = str.Replace("@ILTaxTotalTaxSubtotalTaxableAmountAmountCurrencyID", currencyCode);

                taxSubtotalTaxAmount = (Convert.ToDecimal(lineExtensionAmount, invC) * Convert.ToDecimal(((decimal)trxLine.tax_percentage).ToString(amountFormat, invC), invC)) / 100;
                str = str.Replace("@ILTaxTotalTaxSubtotalTaxAmountAmountContent", taxSubtotalTaxAmount.ToString(amountFormat, invC));
                str = str.Replace("@ILTaxTotalTaxSubtotalTaxAmountAmountCurrencyID", currencyCode);

                str = str.Replace("@ILTaxTotalTaxSubtotalTaxCategoryPercentNumericContent", (trxLine.tax_percentage).ToString(amountFormat, invC));

                str = str.Replace("@TaxTotalTaxSubtotalTaxCategoryID", taxSchemeId);
                if (taxSchemeList != null && taxSchemeList.Any())
                {
                    foreach (KeyValuePair<string, string> entry in taxSchemeList)
                    {
                        str = str.Replace(entry.Key.ToString(), entry.Value);
                    }
                }
                str = str.Replace("@ILTaxExemptionReasonCodeValue", taxExemptionReasonCodeValue);
                if (taxExemptionReasonCodeList != null && taxExemptionReasonCodeList.Any())
                {
                    foreach (KeyValuePair<string, string> entry in taxExemptionReasonCodeList)
                    {
                        str = str.Replace(entry.Key.ToString(), entry.Value);
                    }
                }
                str = str.Replace("@TaxTotalTaxSubtotalTaxCategoryName", taxSchemeName);
                str = str.Replace("@TaxTotalTaxSubtotalTaxCategoryTaxTypeCode", taxTypeCode);

                str = str.Replace("@ItemDescription", trxLine.ProductName);

                str = str.Replace("@pricePriceAmount", (trxLine.OriginalPrice).ToString(amountFormat, invC));
                str = str.Replace("@PricePriceAmountAmountCurrencyID", currencyCode);

                return str;
            }
            catch (Exception ex)
            {
                log.Error("Build InvoiceLine Section Error", ex);
                log.LogMethodExit(null, "Throwing Build InvoiceLine Section Exception-" + ex.Message);
                throw;
            }

        }

        /// <summary>
        /// Build Discount Values
        /// </summary>
        /// <param name="trx"></param>
        /// <returns></returns>
        private string BuildLineDiscountValues(Transaction.TransactionLine trxLine)
        {
            log.LogMethodEntry();
            try
            {
                string linesDiscountSection = string.Empty;
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
                    //linesDiscountSection = linesDiscountSection.Replace("@AllowanceChargeDiscountAmount", (((decimal)trxLine.Price * (decimal)trxLine.TransactionDiscountsDTOList.Sum(td => td.DiscountPercentage) / 100)).ToString(amountFormat, invC));
                    linesDiscountSection = linesDiscountSection.Replace("@AllowanceChargeDiscountAmount", ((Convert.ToDecimal((trxLine.Price).ToString(amountFormat, invC), invC) * (Convert.ToDecimal(((decimal)trxLine.TransactionDiscountsDTOList.Sum(td => td.DiscountPercentage)).ToString(amountFormat, invC), invC)) / 100)).ToString(amountFormat, invC));
                    linesDiscountSection = linesDiscountSection.Replace("@AllowanceChargeBaseAmount", Math.Abs(trxLine.OriginalPrice).ToString(amountFormat, invC));
                }
                return linesDiscountSection;
            }
            catch (Exception ex)
            {
                log.Error("Build Build CreditNote Legal Monetary Values Error", ex);
                log.LogMethodExit(null, "Throwing Build CreditNote Legal Monetary Values Exception-" + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// BuildCreditNoteLineSection
        /// </summary>
        /// <param name="trxLine"></param>
        /// <returns></returns>
        private string BuildCreditNoteLineSection(Transaction.TransactionLine trxLine)
        {
            log.LogMethodEntry();
            string str = creditNoteLineTemplate;
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
                string lineExtensionAmount = string.Empty; 
                if (trxLine.TransactionDiscountsDTOList != null && trxLine.TransactionDiscountsDTOList.Any())
                {
                    //lineExtensionAmount = ((decimal)trxLine.Price - (((decimal)trxLine.Price * (decimal)trxLine.TransactionDiscountsDTOList.Sum(td => td.DiscountPercentage)) / 100)).ToString(amountFormat, invC);
                    lineExtensionAmount = (Convert.ToDecimal(((decimal)trxLine.Price).ToString(amountFormat, invC), invC) - (Convert.ToDecimal(((decimal)trxLine.Price).ToString(amountFormat, invC), invC) * Convert.ToDecimal((decimal)trxLine.TransactionDiscountsDTOList.Sum(td => td.DiscountPercentage), invC) / 100)).ToString(amountFormat, invC);
                }
                else
                {
                    lineExtensionAmount = (trxLine.Price).ToString(amountFormat, invC);
                }
                str = str.Replace("@CLLineExtensionAmountAmountContent", lineExtensionAmount);
                str = str.Replace("@CLLineExtensionAmountAmountCurrencyID", currencyCode);

                str = str.Replace("@CLPricingReferenceAlternativeConditionPricePriceAmountAmountContent", (Convert.ToDecimal(lineExtensionAmount, invC) + ((Convert.ToDecimal(lineExtensionAmount, invC) * (decimal)trxLine.tax_percentage) / 100)).ToString(amountFormat, invC));
                str = str.Replace("@CLPricingReferenceAlternativeConditionPricePriceAmountAmountCurrencyID", currencyCode);

                str = str.Replace("@CLPricingReferenceAlternativeConditionPricePriceTypeCodeCodeContent", pricingReferencePriceTypeCodeCodeContent);
                if (priceTypeCodeList != null && priceTypeCodeList.Any())
                {
                    foreach (KeyValuePair<string, string> entry in priceTypeCodeList)
                    {
                        str = str.Replace(entry.Key.ToString(), entry.Value);
                    }
                }
                //TaxTotal
                str = str.Replace("@CLTaxTotalTaxAmountAmountContent", ((Convert.ToDecimal(lineExtensionAmount, invC) * (decimal)trxLine.tax_percentage) / 100).ToString(amountFormat, invC));
                str = str.Replace("@CLTaxTotalTaxAmountAmountCurrencyID", currencyCode);

                str = str.Replace("@CLTaxTotalTaxSubtotalTaxableAmountAmountContent", lineExtensionAmount);
                str = str.Replace("@CLTaxTotalTaxSubtotalTaxableAmountAmountCurrencyID", currencyCode);

                str = str.Replace("@CLTaxTotalTaxSubtotalTaxAmountAmountContent", ((Convert.ToDecimal(lineExtensionAmount, invC) * (decimal)trxLine.tax_percentage) / 100).ToString(amountFormat, invC));
                str = str.Replace("@CLTaxTotalTaxSubtotalTaxAmountAmountCurrencyID", currencyCode);

                str = str.Replace("@CLTaxTotalTaxSubtotalTaxCategoryPercentNumericContent", (trxLine.tax_percentage).ToString());

                str = str.Replace("@TaxTotalTaxSubtotalTaxCategoryID", taxSchemeId);
                if (taxSchemeList != null && taxSchemeList.Any())
                {
                    foreach (KeyValuePair<string, string> entry in taxSchemeList)
                    {
                        str = str.Replace(entry.Key.ToString(), entry.Value);
                    }
                }
                str = str.Replace("@ILTaxTotalTaxSubtotalTaxCategoryTERCCodeContent", taxExemptionReasonCodeValue);
                if (taxExemptionReasonCodeList != null && taxExemptionReasonCodeList.Any())
                {
                    foreach (KeyValuePair<string, string> entry in taxExemptionReasonCodeList)
                    {
                        str = str.Replace(entry.Key.ToString(), entry.Value);
                    }
                }
                str = str.Replace("@TaxTotalTaxSubtotalTaxCategoryName", taxSchemeName);
                str = str.Replace("@TaxTotalTaxSubtotalTaxCategoryTaxTypeCode", taxTypeCode);

                str = str.Replace("@ItemDescription", trxLine.ProductName);

                str = str.Replace("@pricePriceAmount", (trxLine.OriginalPrice).ToString(amountFormat, invC));
                str = str.Replace("@PricePriceAmountAmountCurrencyID", currencyCode);

                return str;
            }
            catch (Exception ex)
            {
                log.Error("Build Credit Note Line Error", ex);
                log.LogMethodExit(null, "Throwing Build Credit Note Line Exception-" + ex.Message);
                throw;
            }

        }

        /// <summary>
        /// Generate Peru AttributesList
        /// </summary>
        /// <param name="lookupValuesDTO"></param>
        /// <returns></returns>
        private Dictionary<string, Dictionary<string, string>> GeneratePeruAttributesList(LookupValuesDTO lookupValuesDTO)
        {
            log.LogMethodEntry();
            try
            {
                Dictionary<string, string> attribute = new Dictionary<string, string>();
                Dictionary<string, Dictionary<string, string>> xAttributeDictionary = new Dictionary<string, Dictionary<string, string>>();
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
                return xAttributeDictionary;
            }
            catch (Exception ex)
            {
                log.Error("Generating Peru Attributes Error", ex);
                log.LogMethodExit(null, "Throwing Generate Peru Attributes Exception-" + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Map Peru Attributes
        /// </summary>
        /// <param name="peruAttributesDictionary"></param>
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

        /// <summary>
        /// Build Peru Attributes
        /// </summary>
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

                //UBLVersionID
                LookupValuesDTO lookupValuesUBLVersionIDDTO = invoiceAttributesValuesDTOList.FirstOrDefault(p => p.LookupValue == "UBLVersionID");
                if (lookupValuesUBLVersionIDDTO != null)
                {
                    uBLVersionID = lookupValuesUBLVersionIDDTO.Description;
                }
                //CustomizationID
                LookupValuesDTO lookupValuesCustomizationIDDTO = invoiceAttributesValuesDTOList.FirstOrDefault(p => p.LookupValue == "CustomizationID");
                if (lookupValuesCustomizationIDDTO != null)
                {
                    customizationID = lookupValuesCustomizationIDDTO.Description;
                }
                //Boleta
                LookupValuesDTO lookupValuesBoletaAndDNIDTO = invoiceSetupValuesDTOList.FirstOrDefault(p => p.LookupValue == "BoletaAndDNIValues");
                if (lookupValuesBoletaAndDNIDTO != null)
                {
                    string[] values = lookupValuesBoletaAndDNIDTO.Description.Split('|');
                    boletaValue = values[0];
                    dniValue = values[1];
                }
                //Factura
                LookupValuesDTO lookupValuesFacturaAndRUCDTO = invoiceSetupValuesDTOList.FirstOrDefault(p => p.LookupValue == "FacturaAndRUCValues");
                if (lookupValuesFacturaAndRUCDTO != null)
                {
                    string[] values = lookupValuesFacturaAndRUCDTO.Description.Split('|');
                    facturaValue = values[0];
                    rucValue = values[1];
                }

                //CreditNote
                LookupValuesDTO lookupValuesCreditNoteDTO = invoiceSetupValuesDTOList.FirstOrDefault(p => p.LookupValue == "CreditNote");
                if (lookupValuesCreditNoteDTO != null)
                {
                    creditNoteValue = lookupValuesCreditNoteDTO.Description;
                }

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
                //Boleta_File_Upload_Path
                LookupValuesDTO lookupValuesBoletaFileUploadPathDTO = invoiceSetupValuesDTOList.FirstOrDefault(p => p.LookupValue == "Boleta_File_Upload_Path");
                if (lookupValuesBoletaFileUploadPathDTO != null)
                {
                    boleta_File_Upload_Path = lookupValuesBoletaFileUploadPathDTO.Description;
                }

                //Factura_File_Upload_Path
                LookupValuesDTO lookupValuesFacturaFileUploadPathDTO = invoiceSetupValuesDTOList.FirstOrDefault(p => p.LookupValue == "Factura_File_Upload_Path");
                if (lookupValuesFacturaFileUploadPathDTO != null)
                {
                    factura_File_Upload_Path = lookupValuesFacturaFileUploadPathDTO.Description;
                }

                //CreditNote_File_Upload_Path
                LookupValuesDTO lookupValuesCreditNoteFileUploadPathDTO = invoiceSetupValuesDTOList.FirstOrDefault(p => p.LookupValue == "CreditNote_File_Upload_Path");
                if (lookupValuesCreditNoteFileUploadPathDTO != null)
                {
                    creditNote_File_Upload_Path = lookupValuesCreditNoteFileUploadPathDTO.Description;
                }
                //SignatureId
                LookupValuesDTO lookupValuesSignatureIdDTO = invoiceAttributesValuesDTOList.FirstOrDefault(p => p.LookupValue == "SignatureId");
                if (lookupValuesSignatureIdDTO != null)
                {
                    signatureId = lookupValuesSignatureIdDTO.Description;
                }
                //TaxSchemeId 
                LookupValuesDTO lookupValuesTaxSchemeIdDTO = invoiceAttributesValuesDTOList.FirstOrDefault(p => p.LookupValue == "TaxSchemeId");
                if (lookupValuesTaxSchemeIdDTO != null)
                {
                    taxSchemeId = lookupValuesTaxSchemeIdDTO.Description;
                }
                //DiscrepancyResponseCode 
                LookupValuesDTO lookupValuesDiscrepancyResponseCodeDTO = invoiceAttributesValuesDTOList.FirstOrDefault(p => p.LookupValue == "DiscrepancyResponseCode");
                if (lookupValuesDiscrepancyResponseCodeDTO != null)
                {
                    discrepancyResponseCode = lookupValuesDiscrepancyResponseCodeDTO.Description;
                }
                //DiscrepancyResponseCode 
                LookupValuesDTO lookupValuesdiscrepancyResponseCodeDescriptionDTO = invoiceAttributesValuesDTOList.FirstOrDefault(p => p.LookupValue == "DiscrepancyResponseCodeDescription");
                if (lookupValuesdiscrepancyResponseCodeDescriptionDTO != null)
                {
                    discrepancyResponseCodeDescription = lookupValuesdiscrepancyResponseCodeDescriptionDTO.Description;
                }
                //PricingReferencePriceTypeCodeCodeContent 
                LookupValuesDTO lookupValuesPricingReferencePriceTypeCodeCodeContentDTO = invoiceAttributesValuesDTOList.FirstOrDefault(p => p.LookupValue == "PricingReferenceAlternativeConditionPricePriceTypeCodeCodeContent");
                if (lookupValuesPricingReferencePriceTypeCodeCodeContentDTO != null)
                {
                    pricingReferencePriceTypeCodeCodeContent = lookupValuesPricingReferencePriceTypeCodeCodeContentDTO.Description;
                }
                //TaxTypeCode 
                LookupValuesDTO lookupValuesTaxTypeCodeDTO = invoiceAttributesValuesDTOList.FirstOrDefault(p => p.LookupValue == "TaxTypeCode");
                if (lookupValuesTaxTypeCodeDTO != null)
                {
                    taxTypeCode = lookupValuesTaxTypeCodeDTO.Description;
                }
                //TaxSchemeName 
                LookupValuesDTO lookupValuesTaxSchemeNameDTO = invoiceAttributesValuesDTOList.FirstOrDefault(p => p.LookupValue == "TaxSchemeName");
                if (lookupValuesTaxSchemeNameDTO != null)
                {
                    taxSchemeName = lookupValuesTaxSchemeNameDTO.Description;
                }
                //TaxExemptionReasonCodeValue 
                LookupValuesDTO lookupValuesTaxExemptionReasonCodeValueDTO = invoiceAttributesValuesDTOList.FirstOrDefault(p => p.LookupValue == "TaxExemptionReasonCodeValue");
                if (lookupValuesTaxExemptionReasonCodeValueDTO != null)
                {
                    taxExemptionReasonCodeValue = lookupValuesTaxExemptionReasonCodeValueDTO.Description;
                }
                //ExternalReferenceUri
                LookupValuesDTO lookupValuesExternalReferenceUriDTO = invoiceAttributesValuesDTOList.FirstOrDefault(p => p.LookupValue == "ExternalReferenceUri");
                if (lookupValuesExternalReferenceUriDTO != null)
                {
                    externalReferenceUri = lookupValuesExternalReferenceUriDTO.Description;
                }
                //AccountingSPName
                LookupValuesDTO lookupValuesAccountingSPNameDTO = invoiceAttributesValuesDTOList.FirstOrDefault(p => p.LookupValue == "AccountingSPName");
                if (lookupValuesAccountingSPNameDTO != null)
                {
                    accountingSPName = lookupValuesAccountingSPNameDTO.Description;
                }
                //AccountingSPRegistrationName 
                LookupValuesDTO lookupValuesAccountingSPRegistrationNameDTO = invoiceAttributesValuesDTOList.FirstOrDefault(p => p.LookupValue == "AccountingSPRegistrationName");
                if (lookupValuesAccountingSPRegistrationNameDTO != null)
                {
                    accountingSPRegistrationName = lookupValuesAccountingSPRegistrationNameDTO.Description;
                }
                //AccountingSPRegistrationAddressId
                LookupValuesDTO lookupValuesAccountingSPRegistrationAddressIdDTO = invoiceAttributesValuesDTOList.FirstOrDefault(p => p.LookupValue == "AccountingSPRegistrationAddressId");
                if (lookupValuesAccountingSPRegistrationAddressIdDTO != null)
                {
                    accountingSPRegistrationAddressId = lookupValuesAccountingSPRegistrationAddressIdDTO.Description;
                }
                //AccountingSPLine
                LookupValuesDTO lookupValuesAccountingSPLineDTO = invoiceAttributesValuesDTOList.FirstOrDefault(p => p.LookupValue == "AccountingSPLine");
                if (lookupValuesAccountingSPLineDTO != null)
                {
                    accountingSPLine = lookupValuesAccountingSPLineDTO.Description;
                }
                //AccountingSPId
                LookupValuesDTO lookupValuesaccountingSPIdDTO = invoiceAttributesValuesDTOList.FirstOrDefault(p => p.LookupValue == "AccountingSPIdentificationCode");
                if (lookupValuesaccountingSPIdDTO != null)
                {
                    accountingSPIdentificationCode = lookupValuesaccountingSPIdDTO.Description;
                }
                //AccountingSPDistrict
                LookupValuesDTO lookupValuesAccountingSPDistrictDTO = invoiceAttributesValuesDTOList.FirstOrDefault(p => p.LookupValue == "AccountingSPDistrict");
                if (lookupValuesAccountingSPDistrictDTO != null)
                {
                    accountingSPDistrict = lookupValuesAccountingSPDistrictDTO.Description;
                }
                //AccountingSPCountrySubentity
                LookupValuesDTO lookupValuesAccountingSPCountrySubentityDTO = invoiceAttributesValuesDTOList.FirstOrDefault(p => p.LookupValue == "AccountingSPCountrySubentity");
                if (lookupValuesAccountingSPCountrySubentityDTO != null)
                {
                    accountingSPCountrySubentity = lookupValuesAccountingSPCountrySubentityDTO.Description;
                }
                //AccountingSPCitySubdivisionName
                LookupValuesDTO lookupValuesAccountingSPCitySubdivisionNameDTO = invoiceAttributesValuesDTOList.FirstOrDefault(p => p.LookupValue == "AccountingSPCitySubdivisionName");
                if (lookupValuesAccountingSPCitySubdivisionNameDTO != null)
                {
                    accountingSPCitySubdivisionName = lookupValuesAccountingSPCitySubdivisionNameDTO.Description;
                }
                //AccountingSPCityName
                LookupValuesDTO lookupValuesAccountingSPCityNameDTO = invoiceAttributesValuesDTOList.FirstOrDefault(p => p.LookupValue == "AccountingSPCityName");
                if (lookupValuesAccountingSPCityNameDTO != null)
                {
                    accountingSPCityName = lookupValuesAccountingSPCityNameDTO.Description;
                }
                //AddressTypeCode
                LookupValuesDTO lookupValuesaddressTypeCodeDTO = invoiceAttributesValuesDTOList.FirstOrDefault(p => p.LookupValue == "AddressTypeCode");
                if (lookupValuesaddressTypeCodeDTO != null)
                {
                    addressTypeCode = lookupValuesaddressTypeCodeDTO.Description;
                }
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
                //AllowanceChargeIndicator
                LookupValuesDTO lookupValuesAllowanceChargeIndicatorDTO = invoiceAttributesValuesDTOList.FirstOrDefault(p => p.LookupValue == "AllowanceChargeIndicator");
                if (lookupValuesAllowanceChargeIndicatorDTO != null)
                {
                    allowanceChargeIndicator = lookupValuesAllowanceChargeIndicatorDTO.Description;
                }
                //AllowanceChargeReasonCodeValue
                LookupValuesDTO lookupValuesAllowanceChargeReasonCodeValueDTO = invoiceAttributesValuesDTOList.FirstOrDefault(p => p.LookupValue == "AllowanceChargeReasonCodeValue");
                if (lookupValuesAllowanceChargeReasonCodeValueDTO != null)
                {
                    allowanceChargeReasonCodeValue = lookupValuesAllowanceChargeReasonCodeValueDTO.Description;
                }
            }
            catch (Exception ex)
            {
                log.Error("Build Peru Attributes Error", ex);
                log.LogMethodExit(null, "Throwing Build Peru Attributes Exception-" + ex.Message);
                throw new Exception(ex.Message);
            }
            log.LogMethodExit();
        }
        //public override void SendFile()
        //{
        //    log.LogMethodEntry();

        //    log.LogMethodExit();
        //}

        private void SetInvoiceTemplates()
        {
            log.LogMethodEntry();
            boletaHeaderTemplate = LoadTemplateData(PERU_BOLETA_HEADER);
            facturaHeaderTemplate = LoadTemplateData(PERU_FACTURA_HEADER);
            invoiceLineTemplate = LoadTemplateData(PERU_INVOICE_LINE);
            invoiceTaxTotalTemplate = LoadTemplateData(PERU_INVOICE_TAX_TOTAL);
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
            log.LogMethodEntry();
            string template = null;
            // Get the template 
            EmailTemplateListBL emailTemplateListBL = new EmailTemplateListBL(executionContext);
            List<KeyValuePair<EmailTemplateDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<EmailTemplateDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<EmailTemplateDTO.SearchByParameters, string>(EmailTemplateDTO.SearchByParameters.NAME, templateSection));
            searchParameters.Add(new KeyValuePair<EmailTemplateDTO.SearchByParameters, string>(EmailTemplateDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            List<EmailTemplateDTO> emailTemplateDTOList = emailTemplateListBL.GetEmailTemplateDTOList(searchParameters);
            if (emailTemplateDTOList != null && emailTemplateDTOList.Any())
            {
                template = emailTemplateDTOList[0].EmailTemplate;
            }
            else
            {
                log.Info(templateSection + " is not defined");
            }
            log.LogMethodExit(template);
            return template;
        }

    }
}
