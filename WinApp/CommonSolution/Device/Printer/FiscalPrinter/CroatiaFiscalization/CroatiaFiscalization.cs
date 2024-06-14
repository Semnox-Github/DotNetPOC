/********************************************************************************************
 * Project Name - Croatia Fiscalization
 * Description  - Fiscalization methods for Croatia fiscalization implementation
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.90.0      18-Aug-2020      Laster Menezes     Created methods for implementing Croatia Fiscalization
 *2.110       08-Feb-2021      Laster Menezes     Modified method BuildXMLInvoiceAndSend - Updating LastUpdateDate for Trxpayments when fiscalization is successful.
 *2.120       26-Apr-2021      Laster Menezes     Modified the functionality to use ExternalSourceReference column of TrxPayments instead of Reference column
 *2.140       04-Oct-2021      Laster Menezes     Enhancement: QR code changes and fiscalization process improvements for Booking transactions
  ********************************************************************************************/
using Cis;
using Semnox.Core.Utilities;
using Semnox.Parafait.Device.Printer.FiscalPrinter;
using Semnox.Parafait.JobUtils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Semnox.Parafait.Device.Printer.FiscalPrint
{
    public class CroatiaFiscalization : Semnox.Parafait.Device.Printer.FiscalPrint.FiscalPrinter
    {
        Utilities Utilities;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        string oibNumber = string.Empty;
        bool IsVatSystem = false;
        string serviceUrl = string.Empty;
        string certificateName = string.Empty;
        int requestTimeout = 2000;
        const string dateFormatShort = "dd.MM.yyyy";
        const string dateFormatLong = "dd.MM.yyyyTHH:mm:ss";
        X509Certificate2 certificate = null;
        string accountNumberSequenceTag = string.Empty;
        string excludedPaymentModes = string.Empty;
        string reservationAdvancePaymentProduct = string.Empty;

        public const string CROATIAFISCALIZATION = "CROATIAFISCALIZATION";

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        public CroatiaFiscalization(Utilities _Utilities) : base(_Utilities)
        {
            log.LogMethodEntry(_Utilities);
            Utilities = _Utilities;
            Utilities.ParafaitEnv.Initialize();
            Initailize();
            log.LogMethodExit(null);
        }

        public enum SQLParametersVariable
        {
            /// <summary>
            /// EXCLUDED_PAYMENT_MODES
            /// </summary>
            EXCLUDED_PAYMENT_MODES,
        }

        /// <summary>
        /// SetFiscalizationService method
        /// </summary>
        /// <param name="fs"></param>
        private void SetFiscalizationService(FiskalizacijaService fs)
        {
            log.LogMethodEntry(fs);
            // Set CIS service URL
            // default = Fiscalization.SERVICE_URL_PRODUCTION
            fs.Url = serviceUrl;
            // Set request timeout in miliseconds  default = 100s
            fs.Timeout = requestTimeout;
            log.LogMethodExit();
        }

        /// <summary>
        /// PrintReceipt method
        /// </summary>
        /// <param name="TrxId"></param>
        /// <param name="Message"></param>
        /// <param name="SQLTrx"></param>
        /// <param name="tenderedCash"></param>
        /// <param name="isFiscal"></param>
        /// <param name="trxReprint"></param>
        /// <returns></returns>
        public override bool PrintReceipt(int TrxId, ref string Message, SqlTransaction SQLTrx = null, decimal tenderedCash = 0, bool isFiscal = true, bool trxReprint = false)
        {
            log.LogMethodEntry(TrxId, SQLTrx, trxReprint, Message);
            try
            {
                bool fiscalResponse = BuildXMLInvoiceAndSend(TrxId);
                if(!fiscalResponse)
                {
                    LogFiscalizationError(TrxId,string.Empty);
                }
                log.LogMethodExit(false);                
                return fiscalResponse;
            }
            catch (ValidationException ex)
            {
                log.LogMethodExit(false);
                LogFiscalizationError(TrxId, ex.GetAllValidationErrorMessages());
                throw new ValidationException("Fiscalization Failed", ex.ValidationErrorList);
            }
        }

        /// <summary>
        /// Initailize method
        /// </summary>
        private void Initailize()
        {
            log.LogMethodEntry();
            try
            {
                List<ValidationError> validationErrorList = new List<ValidationError>();
                LookupValuesList lookupValuesList = new LookupValuesList(utilities.ExecutionContext);
                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                searchParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "CROATIA_FISCALIZATION"));
                List<LookupValuesDTO> LookupValuesDTOList = lookupValuesList.GetAllLookupValues(searchParameters);
                if (LookupValuesDTOList != null)
                {
                    foreach (LookupValuesDTO lookupValueDTO in LookupValuesDTOList)
                    {
                        switch (lookupValueDTO.LookupValue)
                        {
                            case "OIB_NUMBER":
                                oibNumber = lookupValueDTO.Description;
                                break;
                            case "IS_VAT_SYSTEM":
                                if (!string.IsNullOrEmpty(lookupValueDTO.Description))
                                {
                                    if (lookupValueDTO.Description.ToUpper() == "Y")
                                    {
                                        IsVatSystem = true;
                                    }
                                }
                                break;
                            case "SERVICE_URL":
                                serviceUrl = lookupValueDTO.Description;
                                break;
                            case "CERTIFICATE_NAME":
                                certificateName = lookupValueDTO.Description;
                                break;
                            case "REQUEST_TIMEOUT":
                                requestTimeout = Convert.ToInt32(lookupValueDTO.Description);
                                break;
                            case "ACCOUNT_NUMBER_SEQUENCE_TAG":
                                accountNumberSequenceTag = lookupValueDTO.Description;
                                break;
                            case "FISCALIZATION_EXCLUDED_PAYMENT_MODES":
                                string fiscalizationExcludedPaymentModes = lookupValueDTO.Description;
                                string[] arrExcludedPaymentModes;
                                string[] separators = { "|" };
                                arrExcludedPaymentModes = fiscalizationExcludedPaymentModes.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                                excludedPaymentModes = string.Join(",", arrExcludedPaymentModes);
                                break;
                            case "RESERVATION_ADVANCE_PAYMENT_PRODUCT":
                                reservationAdvancePaymentProduct = lookupValueDTO.Description;
                                break;
                        }
                    }
                }
                try
                {
                    LoadCertificateData();
                }
                catch (Exception ex)
                {
                    validationErrorList.Add(new ValidationError("FISCALIZATION", "", "Error in Fetching Fiscal Certificate:" + ex.Message));
                }
                if (validationErrorList.Count > 0)
                {
                    throw new ValidationException("Fiscalization Error", validationErrorList);
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occured during initialization", ex);
            }
        }


        /// <summary>
        /// LoadCertificateData method
        /// </summary>
        private void LoadCertificateData()
        {
            log.LogMethodEntry();
            if (!string.IsNullOrEmpty(certificateName))
            {
                try
                {
                    // Get certificate from Trusted Root Certificates
                    X509Store store = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
                    store.Open(OpenFlags.ReadOnly);
                    //finding certificate in the store
                    X509Certificate2Collection certs = store.Certificates.Find(X509FindType.FindBySubjectName, certificateName, false);
                    if (certs.Count == 1)
                    {
                        certificate = certs[0];
                        log.LogVariableState("Certificate Name", certificate.SubjectName);
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    throw ex;
                }
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// BuildXMLInvoice method
        /// </summary>
        /// <param name="trxId"></param>
        /// <returns></returns>
        private bool BuildXMLInvoiceAndSend(int trxId)
        {
            log.LogMethodEntry(trxId);
            try
            {
                string trxStatus = GetTransactionStatus(trxId);
                if (!IsBookingTransaction(trxId))
                {
                    bool isReversal = false;
                    List<EligibleFiscalPayments> eligibleFiscalPayments = new List<EligibleFiscalPayments>();
                    if (!trxStatus.Equals("CLOSED"))
                    {
                        //No fiscalization if transaction is not closed.
                        return false;
                    }
                    else
                    {
                        bool isReversedTrx = IsReversedTransaction(trxId);
                        if (isReversedTrx)
                        {
                            isReversal = true;
                            int origTrxId = GetOriginalTrxId(trxId);
                            eligibleFiscalPayments = GetEligibleTrxPayments(origTrxId);

                            if (eligibleFiscalPayments != null && eligibleFiscalPayments.Count > 0)
                            {
                                //Skip fiscalization when parent Transaction is pending for fiscalization                                
                                return false;
                            }
                            else
                            {
                                eligibleFiscalPayments = GetEligibleTrxPayments(trxId);
                            }
                        }
                        else
                        {
                            eligibleFiscalPayments = GetEligibleTrxPayments(trxId);
                        }
                    }

                    if (eligibleFiscalPayments != null && eligibleFiscalPayments.Count > 0)
                    {
                        decimal fiscalAmount = 0;
                        string fiscalInvoiceId = string.Empty;
                        CroatiaFiscalReceipt croatiaFiscalReceipt = new CroatiaFiscalReceipt();
                        List<FiscalTaxLines> fiscalTaxLines = new List<FiscalTaxLines>();

                        bool isReversedPaymentsFiscalized = FiscalizeReversedPayments(trxId);
                        if(!isReversedPaymentsFiscalized)
                        {
                            //Could not fiscalize already fiscalized reversed payments. Do not continue.
                            return false;
                        }

                        //Get positive payment lines when transaction is non reversal.
                        if(!isReversal)
                        {
                            eligibleFiscalPayments = eligibleFiscalPayments.FindAll(m => m.paymentAmount >= 0);
                        }
                        
                        decimal trxNetAmount = GetTransactionNetAmount(trxId);                       
                        fiscalInvoiceId = eligibleFiscalPayments[0].siteId.ToString() + eligibleFiscalPayments[0].paymentId.ToString();

                        foreach (EligibleFiscalPayments eligibleFiscalPayment in eligibleFiscalPayments)
                        {
                            fiscalAmount = fiscalAmount + eligibleFiscalPayment.paymentAmount;
                        }
                        if(fiscalAmount == 0)
                        {
                            //No Payment amount. No fiscalization is needed.
                            return true;
                        }

                        fiscalTaxLines = GetFiscalizationTaxLines(trxId, fiscalAmount, trxNetAmount);
                        log.LogVariableState("Amount to be fiscalized", fiscalAmount);

                        //Populate Fiscal Receipt with Payemnt and Tax Lines
                        croatiaFiscalReceipt.eligibleFiscalPayments = eligibleFiscalPayments;
                        croatiaFiscalReceipt.fiscalTaxLines = fiscalTaxLines;
                        string jirReference = FiscalizeReceipt(trxId, croatiaFiscalReceipt, fiscalInvoiceId, fiscalAmount);

                        if (!string.IsNullOrWhiteSpace(jirReference))
                        {
                            jirReference = jirReference + "/COMPLETE/";
                            List<int> lstPaymentIds = new List<int>();
                            foreach (EligibleFiscalPayments eligibleFiscalPayment in croatiaFiscalReceipt.eligibleFiscalPayments)
                            {
                                lstPaymentIds.Add(Convert.ToInt32(eligibleFiscalPayment.paymentId.ToString()));
                            }
                            //Update TrxPayments Lines 
                            UpdateTrxPaymentLines(jirReference, lstPaymentIds, isReversal);
                        }
                    }
                }
                else
                {
                    //Booking Transaction
                    string bookingStatus = GetBookingStatus(trxId);
                    if (!bookingStatus.ToUpper().Equals("COMPLETE"))
                    {
                        decimal fiscalAmount = 0;
                        string fiscalInvoiceId = string.Empty;

                        //Process For Advance payments
                        if (!bookingStatus.ToUpper().Equals("CANCELLED"))
                        {
                            List<EligibleFiscalPayments> eligibleDepositPayments = new List<EligibleFiscalPayments>();
                            eligibleDepositPayments = GetEligibleTrxPayments(trxId);

                            if (eligibleDepositPayments != null && eligibleDepositPayments.Count > 0)
                            {
                                foreach (EligibleFiscalPayments depositPaymentLine in eligibleDepositPayments)
                                {
                                    CroatiaFiscalReceipt depositFiscalReceipt = new CroatiaFiscalReceipt();
                                    List<FiscalTaxLines> depositTaxLines = new List<FiscalTaxLines>();
                                    fiscalAmount = depositPaymentLine.paymentAmount;
                                    fiscalInvoiceId = eligibleDepositPayments[0].siteId.ToString() + depositPaymentLine.paymentId.ToString();
                                    log.LogVariableState("Deposit amount to be fiscalized", fiscalAmount);

                                    //Get Tax details for Booking deopsit product
                                    DataTable dtDepositProduct = GetReservationDepositTaxReferenceProduct();
                                    if (dtDepositProduct != null && dtDepositProduct.Rows.Count > 0)
                                    {
                                        decimal taxPercentage = Convert.ToDecimal(dtDepositProduct.Rows[0]["tax_percentage"].ToString());
                                        decimal taxAmount = (fiscalAmount * taxPercentage) / (100 + taxPercentage);
                                        decimal amountWithoutTax = fiscalAmount - taxAmount;
                                        FiscalTaxLines depositTaxLine = new FiscalTaxLines();
                                        depositTaxLine.taxPercentage = taxPercentage;
                                        depositTaxLine.taxAmount = taxAmount;
                                        depositTaxLine.amountWithoutTax = amountWithoutTax;
                                        depositTaxLines.Add(depositTaxLine);

                                        depositFiscalReceipt.eligibleFiscalPayments = eligibleDepositPayments;
                                        depositFiscalReceipt.fiscalTaxLines = depositTaxLines;

                                        string jirReference = FiscalizeReceipt(trxId, depositFiscalReceipt, fiscalInvoiceId, fiscalAmount);

                                        if (!string.IsNullOrWhiteSpace(jirReference))
                                        {
                                            jirReference = jirReference + "/DEPOSIT/" + taxPercentage + "/";
                                            List<int> lstPaymentIds = new List<int>();
                                            foreach (EligibleFiscalPayments eligibleFiscalPayment in depositFiscalReceipt.eligibleFiscalPayments)
                                            {
                                                lstPaymentIds.Add(Convert.ToInt32(eligibleFiscalPayment.paymentId.ToString()));
                                            }
                                            UpdateTrxPaymentLines(jirReference, lstPaymentIds);
                                        }
                                    }
                                    else
                                    {
                                        log.Debug("Cannot fiscalize the deposit payment. Reference Deposit Product for tax details is not set");
                                        return false;
                                    }
                                }
                            }
                        }
                        else if (bookingStatus.ToUpper().Equals("CANCELLED"))
                        {
                            List<EligibleFiscalPayments> eligibleDepositCancelledPayments = new List<EligibleFiscalPayments>();
                            eligibleDepositCancelledPayments = GetEligibleTrxPayments(trxId);
                            eligibleDepositCancelledPayments = eligibleDepositCancelledPayments.FindAll(m => m.fiscalReference != string.Empty);
                            foreach (EligibleFiscalPayments cancelledDepositPaymentLine in eligibleDepositCancelledPayments)
                            {
                                if (cancelledDepositPaymentLine.fiscalReference.Contains("DEPOSIT"))
                                {
                                    CroatiaFiscalReceipt depositCancelFiscalReceipt = new CroatiaFiscalReceipt();
                                    List<FiscalTaxLines> depositCancelTaxLines = new List<FiscalTaxLines>();
                                    fiscalInvoiceId = cancelledDepositPaymentLine.siteId.ToString() + cancelledDepositPaymentLine.paymentId.ToString();

                                    decimal prevDepositAmount = GetPreviousFiscalizedDepositAmount(cancelledDepositPaymentLine);
                                    decimal prevTaxPerc = GetPreviousFiscalizedTaxPercentage(cancelledDepositPaymentLine);
                                    decimal taxAmount = (prevDepositAmount * prevTaxPerc) / (100 + prevTaxPerc);
                                    decimal amountWithoutTax = prevDepositAmount - taxAmount;

                                    FiscalTaxLines depositCancelTaxLine = new FiscalTaxLines();
                                    depositCancelTaxLine.taxPercentage = prevTaxPerc;
                                    depositCancelTaxLine.taxAmount = -taxAmount;
                                    depositCancelTaxLine.amountWithoutTax = -amountWithoutTax;
                                    depositCancelTaxLines.Add(depositCancelTaxLine);

                                    depositCancelFiscalReceipt.eligibleFiscalPayments.Add(cancelledDepositPaymentLine);
                                    depositCancelFiscalReceipt.fiscalTaxLines = depositCancelTaxLines;

                                    string jirReference = FiscalizeReceipt(trxId, depositCancelFiscalReceipt, fiscalInvoiceId, fiscalAmount);

                                    if (!string.IsNullOrWhiteSpace(jirReference))
                                    {
                                        jirReference = jirReference + "/REVERSED/" + prevTaxPerc + "/";
                                        List<int> lstPaymentIds = new List<int>();
                                        lstPaymentIds.Add(Convert.ToInt32(cancelledDepositPaymentLine.paymentId.ToString()));
                                        UpdateTrxPaymentLines(jirReference, lstPaymentIds);
                                    }
                                }
                            }
                        }
                    }
                    if (bookingStatus.Equals("COMPLETE"))
                    {
                        if(trxStatus.ToUpper().Equals("CANCELLED"))
                        {
                            //Reverse the deposit fiscalization
                            ReverseDepositFisclization(trxId);
                        }
                        else if(trxStatus.ToUpper().Equals("CLOSED"))
                        {
                            bool areDepositsReversed = ReverseDepositFisclization(trxId);
                            //Fiscalize the entire booking Transaction
                            if (areDepositsReversed)
                            {
                                SendEntireBookingForFiscalization(trxId);
                            }
                        }                                              
                    }
                }
            }
            catch (ValidationException ex)
            {
                log.LogMethodExit(false);
                throw new ValidationException("Fiscalization Failed", ex.ValidationErrorList);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
            log.LogMethodExit(true);
            return true;
        }


        /// <summary>
        /// ReverseDepositFisclization
        /// </summary>
        /// <param name="trxId"></param>
        /// <returns>True if Deposit Fisclization is reversed</returns>
        private bool ReverseDepositFisclization(int trxId)
        {
            //Reverse the deposit amount
            log.LogMethodEntry(trxId);
            bool areDepositsReversed = false;
            List<EligibleFiscalPayments> eligibleDepositPayments = new List<EligibleFiscalPayments>();
            eligibleDepositPayments = GetFiscalizedDepositPaymentsLines(trxId);
            try
            {
                int recieptIndex = 0;
                foreach (EligibleFiscalPayments depositPaymentLine in eligibleDepositPayments)
                {
                    CroatiaFiscalReceipt depositCancelReceipt = new CroatiaFiscalReceipt();
                    List<FiscalTaxLines> depositCancelTaxLines = new List<FiscalTaxLines>();
                    recieptIndex++;
                    string fiscalInvoiceId = depositPaymentLine.siteId + "0" + recieptIndex.ToString() + depositPaymentLine.paymentId.ToString();
                    string[] depositFiscalReference = depositPaymentLine.fiscalReference.Split('/');
                    string[] depositAmountReference = depositFiscalReference[0].Split('|');

                    decimal depositAmount = Convert.ToDecimal(depositAmountReference[4].ToString());
                    decimal taxPercentage = Convert.ToDecimal(depositFiscalReference[2].ToString());
                    decimal taxAmount = Convert.ToDecimal(((depositAmount * taxPercentage) / (100 + taxPercentage)));

                    FiscalTaxLines depositCancelTaxLine = new FiscalTaxLines();
                    depositCancelTaxLine.taxPercentage = taxPercentage;
                    depositCancelTaxLine.amountWithoutTax = -(depositAmount - taxAmount);
                    depositCancelTaxLine.taxAmount = -taxAmount;
                    depositCancelTaxLines.Add(depositCancelTaxLine);

                    List<EligibleFiscalPayments> eligibleDepositPaymentsForCancellation = new List<EligibleFiscalPayments>();
                    eligibleDepositPaymentsForCancellation.Add(depositPaymentLine);
                    depositCancelReceipt.eligibleFiscalPayments = eligibleDepositPaymentsForCancellation;
                    depositCancelReceipt.fiscalTaxLines = depositCancelTaxLines;

                    string jirCancelDepositReference = FiscalizeReceipt(trxId, depositCancelReceipt, fiscalInvoiceId, -(depositAmount));

                    if (!string.IsNullOrWhiteSpace(jirCancelDepositReference))
                    {
                        jirCancelDepositReference = jirCancelDepositReference + "/REVERSED/";
                        List<int> lstPaymentIds = new List<int>();
                        lstPaymentIds.Add(Convert.ToInt32(depositPaymentLine.paymentId.ToString()));
                        UpdateTrxPaymentLines(jirCancelDepositReference, lstPaymentIds);
                    }
                }
                areDepositsReversed = true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit(areDepositsReversed);
            return areDepositsReversed;
        }


        /// <summary>
        /// FiscalizeReversedPayments
        /// </summary>
        /// <param name="trxId"></param>
        /// <returns>True if Reversed Payments are fiscalized</returns>
        private bool FiscalizeReversedPayments(int trxId)
        {
            log.LogMethodEntry(trxId);
            try
            {
                decimal fiscalAmount = 0;
                CroatiaFiscalReceipt reversePaymentReceipt = new CroatiaFiscalReceipt();
                List<FiscalTaxLines> reversePaymentTaxLines = new List<FiscalTaxLines>();
                List<EligibleFiscalPayments> eligibleReversePaymentLines = new List<EligibleFiscalPayments>();

                decimal trxNetAmount = GetTransactionNetAmount(trxId);
                eligibleReversePaymentLines = GetReversedPayments(trxId);
                if (eligibleReversePaymentLines != null && eligibleReversePaymentLines.Count > 0)
                {
                    foreach (EligibleFiscalPayments eliglibleReversePayment in eligibleReversePaymentLines)
                    {
                        fiscalAmount = fiscalAmount + eliglibleReversePayment.paymentAmount;
                    }
                    string fiscalFinalInvoiceId = eligibleReversePaymentLines[0].siteId.ToString() + eligibleReversePaymentLines[0].paymentId.ToString();
                    reversePaymentTaxLines = GetFiscalizationTaxLines(trxId, fiscalAmount, trxNetAmount);
                    reversePaymentReceipt.eligibleFiscalPayments = eligibleReversePaymentLines;
                    reversePaymentReceipt.fiscalTaxLines = reversePaymentTaxLines;

                    string jirReference = FiscalizeReceipt(trxId, reversePaymentReceipt, fiscalFinalInvoiceId, fiscalAmount);

                    if (!string.IsNullOrWhiteSpace(jirReference))
                    {
                        jirReference = jirReference + "/COMPLETE/";
                        List<int> lstPaymentIds = new List<int>();
                        foreach (EligibleFiscalPayments eliglibleReversePayment in reversePaymentReceipt.eligibleFiscalPayments)
                        {
                            lstPaymentIds.Add(Convert.ToInt32(eliglibleReversePayment.paymentId.ToString()));
                        }
                        UpdateTrxPaymentLines(jirReference, lstPaymentIds, true);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(false);
                return false;
            }
            log.LogMethodExit(true);
            return true;
        }


        /// <summary>
        /// SendEntireBookingForFiscalization
        /// </summary>
        /// <param name="trxId"></param>
        private void SendEntireBookingForFiscalization(int trxId)
        {
            log.LogMethodEntry();
            decimal fiscalAmount = 0;
            CroatiaFiscalReceipt finalBookingReceipt = new CroatiaFiscalReceipt();
            List<FiscalTaxLines> finalBookingTaxLines = new List<FiscalTaxLines>();
            List<EligibleFiscalPayments> finalBookingPaymentLines = new List<EligibleFiscalPayments>();

            decimal trxNetAmount = GetTransactionNetAmount(trxId);
            finalBookingPaymentLines = GetEligibleBookingTrxPaymentsFinal(trxId);
            if(finalBookingPaymentLines != null && finalBookingPaymentLines.Count > 0)
            {
                foreach (EligibleFiscalPayments finalBookingPaymentLine in finalBookingPaymentLines)
                {
                    fiscalAmount = fiscalAmount + finalBookingPaymentLine.paymentAmount;
                }
                log.Info("amount to be fiscalized " + fiscalAmount);

                string fiscalFinalInvoiceId = finalBookingPaymentLines[0].siteId.ToString() + finalBookingPaymentLines.FindAll(m => m.fiscalReference.Contains("DEPOSIT") == false).FirstOrDefault().paymentId.ToString();

                finalBookingTaxLines = GetFiscalizationTaxLines(trxId, fiscalAmount, trxNetAmount);
                finalBookingReceipt.eligibleFiscalPayments = finalBookingPaymentLines;
                finalBookingReceipt.fiscalTaxLines = finalBookingTaxLines;

                string jirFinalReference = FiscalizeReceipt(trxId, finalBookingReceipt, fiscalFinalInvoiceId, fiscalAmount);

                if (!string.IsNullOrWhiteSpace(jirFinalReference))
                {
                    jirFinalReference = jirFinalReference + "/COMPLETE/";
                    List<int> lstPaymentIds = new List<int>();
                    foreach (EligibleFiscalPayments finalBookingpaymentLine in finalBookingReceipt.eligibleFiscalPayments)
                    {
                        lstPaymentIds.Add(Convert.ToInt32(finalBookingpaymentLine.paymentId.ToString()));
                    }
                    UpdateTrxPaymentLines(jirFinalReference, lstPaymentIds);
                }
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// IsReversedTransaction
        /// </summary>
        /// <param name="trxId"></param>
        /// <returns>returns true if Transaction is reversed</returns>
        private bool IsReversedTransaction(int trxId)
        {
            log.LogMethodEntry(trxId);
            bool isReversalTrx = false;
            int originalTrxId = -1;
            DataTable dtTransaction = Utilities.executeDataTable(@"select isnull(OriginalTrxID,-1) OriginalTrxID from trx_header where TrxId =@trxid", new SqlParameter("@trxId", trxId));
            if (dtTransaction != null && dtTransaction.Rows.Count > 0)
            {
                originalTrxId = Convert.ToInt32(dtTransaction.Rows[0]["OriginalTrxID"].ToString());
                isReversalTrx = originalTrxId != -1 ? true : false;
            }
            log.LogMethodExit(isReversalTrx);
            return isReversalTrx;
        }


        /// <summary>
        /// GetOriginalTrxId
        /// </summary>
        /// <param name="trxId"></param>
        /// <returns>Original TrxId for Reversed Transaction</returns>
        private int GetOriginalTrxId(int trxId)
        {
            log.LogMethodEntry(trxId);
            int originalTrxId = -1;
            DataTable dtTransaction = Utilities.executeDataTable(@"select isnull(OriginalTrxID,-1) OriginalTrxID from trx_header where TrxId = @trxid", new SqlParameter("@trxId", trxId));
            if (dtTransaction != null && dtTransaction.Rows.Count > 0)
            {
                originalTrxId = Convert.ToInt32(dtTransaction.Rows[0]["OriginalTrxID"].ToString());
                log.LogVariableState("originalTrxId", originalTrxId);
            }
            log.LogMethodExit(originalTrxId);
            return originalTrxId;
        }


        /// <summary>
        /// GetReversedPayments
        /// </summary>
        /// <param name="trxId"></param>
        /// <returns>List of fiscalized Reversed Payment Lines</returns>
        private List<EligibleFiscalPayments> GetReversedPayments(int trxId)
        {
            log.LogMethodEntry(trxId);
            List<EligibleFiscalPayments> eligibleReversedPayments = new List<EligibleFiscalPayments>();
            string fiscalisedPaymentsQuery = @"select distinct s.site_id SiteId, isnull((tp.Amount),0) Amount, tp.PaymentId,paymentmode, 
                                                          isCash,isCreditCard,isDebitCard,tp.ExternalSourceReference ,
                                                          tp.PosMachine,pmch.FriendlyName POSFriendlyName
                                                          from TrxPayments tp inner join TrxPayments tp1 
                                                          on tp.ExternalSourceReference = tp1.ExternalSourceReference
                                                          and tp.PaymentId != tp1.PaymentId, trx_header th, PaymentModes pm, POSMachines pmch, site s 
                                                          where th.TrxId = tp.TrxId
													      and tp.PaymentModeId = pm.PaymentModeId 
                                                          and tp.PosMachine = pmch.POSName
                                                          and (th.site_id = s.site_id or th.site_id is null) 
                                                          and pm.isDebitCard != 'Y'
                                                          and tp.Amount < 0 
                                                          and tp.PaymentModeId = pm.PaymentModeId
                                                          and tp.ExternalSourceReference like 'ZKI%' 
                                                          and tp.TrxId = @trxId";
            List<SqlParameter> sqlParamTrxPaymentLines = new List<SqlParameter>();
            DataTable dtTrxPaymentsLines = null;
            sqlParamTrxPaymentLines.Add(new SqlParameter("@trxId", trxId));

            if (!string.IsNullOrEmpty(excludedPaymentModes))
            {
                DataAccessHandler dataAccessHandler = new DataAccessHandler();
                sqlParamTrxPaymentLines.AddRange(dataAccessHandler.GetSqlParametersForInClause(SQLParametersVariable.EXCLUDED_PAYMENT_MODES, excludedPaymentModes));
                fiscalisedPaymentsQuery = fiscalisedPaymentsQuery + @" and pm.PaymentMode not in (" + dataAccessHandler.GetInClauseParameterName(SQLParametersVariable.EXCLUDED_PAYMENT_MODES, excludedPaymentModes) + @")";                
            }
            dtTrxPaymentsLines = Utilities.executeDataTable(fiscalisedPaymentsQuery, sqlParamTrxPaymentLines.ToArray());

            if (dtTrxPaymentsLines.Rows.Count > 0)
            {
                foreach (DataRow row in dtTrxPaymentsLines.Rows)
                {
                    EligibleFiscalPayments eligibleFiscalPayment = new EligibleFiscalPayments();
                    eligibleFiscalPayment.siteId = Convert.ToInt32(row["SiteId"]);
                    eligibleFiscalPayment.paymentAmount = Convert.ToDecimal(row["Amount"]);
                    eligibleFiscalPayment.paymentId = Convert.ToInt32(row["PaymentId"]);
                    eligibleFiscalPayment.paymentMode = row["paymentmode"].ToString();
                    eligibleFiscalPayment.isCash = row["isCash"].ToString();
                    eligibleFiscalPayment.isDebitCard = row["isDebitCard"].ToString();
                    eligibleFiscalPayment.isCreditCard = row["isCreditCard"].ToString();
                    eligibleFiscalPayment.fiscalReference = row["ExternalSourceReference"].ToString();
                    eligibleFiscalPayment.posMachine = row["PosMachine"].ToString();
                    eligibleFiscalPayment.posFriendlyName = row["POSFriendlyName"].ToString();
                    eligibleReversedPayments.Add(eligibleFiscalPayment);
                }
            }
            log.LogMethodExit(eligibleReversedPayments);
            return eligibleReversedPayments;
        }


        /// <summary>
        /// GetEligibleTrxPayments
        /// </summary>
        /// <param name="trxId"></param>
        /// <returns>List of Eligible Trx Payments</returns>
        private List<EligibleFiscalPayments> GetEligibleTrxPayments(int trxId)
        {
            log.LogMethodEntry(trxId);
            List<EligibleFiscalPayments> eligibleFiscalPayments = new List<EligibleFiscalPayments>();
            string unFiscalisedPaymentsQuery = @"select s.site_id SiteId, isnull((tp.Amount),0) Amount, tp.PaymentId,paymentmode, 
                                                isCash,isCreditCard,isDebitCard,tp.ExternalSourceReference,tp.PosMachine,pmach.FriendlyName POSFriendlyName
                                                from PaymentModes pm, trx_header th,TrxPayments tp, POSMachines pmach, site s
                                                where th.TrxId = tp.TrxId
                                                and tp.PaymentModeId = pm.PaymentModeId 
												and tp.PosMachine = pmach.POSName
												and pm.isDebitCard != 'Y' 
                                                and (th.site_id = s.site_id or th.site_id is null)
                                                and (tp.ExternalSourceReference is null or tp.ExternalSourceReference not like 'ZKI%' )  
                                                and isnull(tp.ExternalSourceReference,'') not like '%COMPLETE%'
                                                and th.TrxId = @trxId ";

            string unFiscalisedDuplicatePaymentsQuery = @" union all
                                                          select distinct s.site_id SiteId, isnull((tp.Amount),0) Amount, tp.PaymentId,paymentmode, 
                                                          isCash,isCreditCard,isDebitCard,tp.ExternalSourceReference ,tp.PosMachine,pmch.FriendlyName POSFriendlyName
                                                          from TrxPayments tp inner join
                                                          TrxPayments tp1 on tp.ExternalSourceReference = tp1.ExternalSourceReference
                                                          and tp.PaymentId != tp1.PaymentId, trx_header th, PaymentModes pm, POSMachines pmch, site s 
                                                          where th.TrxId = tp.TrxId
													      and tp.PaymentModeId = pm.PaymentModeId
														  and tp.PosMachine = pmch.POSName
                                                          and (th.site_id = s.site_id or th.site_id is null)
                                                          and pm.isDebitCard != 'Y'
                                                          and tp.Amount < 0
                                                          and tp.PaymentModeId = pm.PaymentModeId
                                                          and tp.ExternalSourceReference like 'ZKI%'
                                                          and tp.TrxId = @trxId ";

            List<SqlParameter> sqlParamTrxPaymentLines = new List<SqlParameter>();
            DataTable dtTrxPaymentsLines = null;
            sqlParamTrxPaymentLines.Add(new SqlParameter("@trxId", trxId));

            if (!string.IsNullOrEmpty(excludedPaymentModes))
            {
                DataAccessHandler dataAccessHandler = new DataAccessHandler();
                sqlParamTrxPaymentLines.AddRange(dataAccessHandler.GetSqlParametersForInClause(SQLParametersVariable.EXCLUDED_PAYMENT_MODES, excludedPaymentModes));
                unFiscalisedPaymentsQuery = unFiscalisedPaymentsQuery + @" and pm.PaymentMode not in (" + dataAccessHandler.GetInClauseParameterName(SQLParametersVariable.EXCLUDED_PAYMENT_MODES, excludedPaymentModes) + @")";
                unFiscalisedDuplicatePaymentsQuery = unFiscalisedDuplicatePaymentsQuery + @" and pm.PaymentMode not in (" + dataAccessHandler.GetInClauseParameterName(SQLParametersVariable.EXCLUDED_PAYMENT_MODES, excludedPaymentModes) + @")";
            }
            dtTrxPaymentsLines = Utilities.executeDataTable(unFiscalisedPaymentsQuery + unFiscalisedDuplicatePaymentsQuery, sqlParamTrxPaymentLines.ToArray());

            if (dtTrxPaymentsLines.Rows.Count > 0)
            {
                foreach (DataRow row in dtTrxPaymentsLines.Rows)
                {
                    EligibleFiscalPayments eligibleFiscalPayment = new EligibleFiscalPayments();
                    eligibleFiscalPayment.siteId = Convert.ToInt32(row["SiteId"]);
                    eligibleFiscalPayment.paymentAmount = Convert.ToDecimal(row["Amount"]);
                    eligibleFiscalPayment.paymentId = Convert.ToInt32(row["PaymentId"]);
                    eligibleFiscalPayment.paymentMode = row["paymentmode"].ToString();
                    eligibleFiscalPayment.isCash = row["isCash"].ToString();
                    eligibleFiscalPayment.isDebitCard = row["isDebitCard"].ToString();
                    eligibleFiscalPayment.isCreditCard = row["isCreditCard"].ToString();
                    eligibleFiscalPayment.fiscalReference = row["ExternalSourceReference"].ToString();
                    eligibleFiscalPayment.posMachine = row["PosMachine"].ToString();
                    eligibleFiscalPayment.posFriendlyName = row["POSFriendlyName"].ToString();
                    eligibleFiscalPayments.Add(eligibleFiscalPayment);
                }
            }
            log.LogMethodExit(eligibleFiscalPayments);
            return eligibleFiscalPayments;
        }


        /// <summary>
        /// GetFiscalizedDepositPaymentsLines
        /// </summary>
        /// <param name="trxId"></param>
        /// <returns>List of Fiscalized Deposit Payments Lines</returns>
        private List<EligibleFiscalPayments> GetFiscalizedDepositPaymentsLines(int trxId)
        {
            log.LogMethodEntry(trxId);
            List<EligibleFiscalPayments> eligibleFiscalPayments = new List<EligibleFiscalPayments>();
            string FiscalisedPaymentsQuery = @"select distinct s.site_id SiteId, isnull((tp.Amount),0) Amount, tp.PaymentId,paymentmode,isCash,isCreditCard,isDebitCard,
                                                tp.ExternalSourceReference,tp.PosMachine,pmach.FriendlyName POSFriendlyName                                             
                                                from PaymentModes pm, trx_header th,TrxPayments tp, POSMachines pmach, bookings b,site s
                                                where th.TrxId = tp.TrxId
                                                and b.TrxId = th.TrxId and (th.site_id = s.site_id or th.site_id is null) and tp.PaymentModeId = pm.PaymentModeId 
												and tp.PosMachine = pmach.POSName and pm.isDebitCard != 'Y' and tp.ExternalSourceReference like 'ZKI%' 
                                                and tp.ExternalSourceReference like '%DEPOSIT%' and tp.ExternalSourceReference NOT like '%REVERSED%' 
                                                and th.TrxId = @trxId ";

            List<SqlParameter> sqlParamTrxPaymentLines = new List<SqlParameter>();
            DataTable dtTrxPaymentsLines = null;
            sqlParamTrxPaymentLines.Add(new SqlParameter("@trxId", trxId));

            if (!string.IsNullOrEmpty(excludedPaymentModes))
            {
                DataAccessHandler dataAccessHandler = new DataAccessHandler();
                sqlParamTrxPaymentLines.AddRange(dataAccessHandler.GetSqlParametersForInClause(SQLParametersVariable.EXCLUDED_PAYMENT_MODES, excludedPaymentModes));
                FiscalisedPaymentsQuery = FiscalisedPaymentsQuery + @" and pm.PaymentMode not in (" + dataAccessHandler.GetInClauseParameterName(SQLParametersVariable.EXCLUDED_PAYMENT_MODES, excludedPaymentModes) + @")";
            }
            dtTrxPaymentsLines = Utilities.executeDataTable(FiscalisedPaymentsQuery, sqlParamTrxPaymentLines.ToArray());

            if (dtTrxPaymentsLines.Rows.Count > 0)
            {
                foreach (DataRow row in dtTrxPaymentsLines.Rows)
                {
                    EligibleFiscalPayments eligibleFiscalPayment = new EligibleFiscalPayments();
                    eligibleFiscalPayment.siteId = Convert.ToInt32(row["SiteId"]);
                    eligibleFiscalPayment.paymentAmount = Convert.ToDecimal(row["Amount"]);
                    eligibleFiscalPayment.paymentId = Convert.ToInt32(row["PaymentId"]);
                    eligibleFiscalPayment.paymentMode = row["paymentmode"].ToString();
                    eligibleFiscalPayment.isCash = row["isCash"].ToString();
                    eligibleFiscalPayment.isDebitCard = row["isDebitCard"].ToString();
                    eligibleFiscalPayment.isCreditCard = row["isCreditCard"].ToString();
                    eligibleFiscalPayment.fiscalReference = row["ExternalSourceReference"].ToString();
                    eligibleFiscalPayment.posMachine = row["PosMachine"].ToString();
                    eligibleFiscalPayment.posFriendlyName = row["POSFriendlyName"].ToString();
                    eligibleFiscalPayments.Add(eligibleFiscalPayment);
                }
            }
            log.LogMethodExit(eligibleFiscalPayments);
            return eligibleFiscalPayments;
        }


        /// <summary>
        /// GetEligibleAdvancePaymentsLines
        /// </summary>
        /// <param name="trxId"></param>
        /// <returns>List of Eligible Advance Payments Lines</returns>
        private List<EligibleFiscalPayments> GetEligibleAdvancePaymentsLines(int trxId)
        {
            log.LogMethodEntry(trxId);
            List<EligibleFiscalPayments> eligibleAdvanceFiscalPayments = new List<EligibleFiscalPayments>();
            string FiscalisedPaymentsQuery = @"select distinct s.site_id SiteId, isnull((tp.Amount),0) Amount, tp.PaymentId,paymentmode, 
                                                isCash,isCreditCard,isDebitCard,tp.ExternalSourceReference,tp.PosMachine,pmach.FriendlyName POSFriendlyName                                             
                                                from PaymentModes pm, trx_header th,TrxPayments tp, POSMachines pmach, bookings b,site s
                                                where th.TrxId = tp.TrxId
                                                and b.TrxId = th.TrxId
												and (th.site_id = s.site_id or th.site_id is null)
                                                and tp.PaymentModeId = pm.PaymentModeId 
												and tp.PosMachine = pmach.POSName
												and pm.isDebitCard != 'Y' 
                                                and tp.ExternalSourceReference like 'ZKI%' 
                                                and tp.ExternalSourceReference like '%DEPOSIT%' 
                                                and tp.ExternalSourceReference NOT like '%REVERSED%' 
                                                and th.TrxId = @trxId ";

            List<SqlParameter> sqlParamTrxPaymentLines = new List<SqlParameter>();
            DataTable dtTrxPaymentsLines = null;
            sqlParamTrxPaymentLines.Add(new SqlParameter("@trxId", trxId));

            if (!string.IsNullOrEmpty(excludedPaymentModes))
            {
                DataAccessHandler dataAccessHandler = new DataAccessHandler();
                sqlParamTrxPaymentLines.AddRange(dataAccessHandler.GetSqlParametersForInClause(SQLParametersVariable.EXCLUDED_PAYMENT_MODES, excludedPaymentModes));
                FiscalisedPaymentsQuery = FiscalisedPaymentsQuery + @" and pm.PaymentMode not in (" + dataAccessHandler.GetInClauseParameterName(SQLParametersVariable.EXCLUDED_PAYMENT_MODES, excludedPaymentModes) + @")";
            }
            dtTrxPaymentsLines = Utilities.executeDataTable(FiscalisedPaymentsQuery, sqlParamTrxPaymentLines.ToArray());

            if (dtTrxPaymentsLines.Rows.Count > 0)
            {
                foreach (DataRow row in dtTrxPaymentsLines.Rows)
                {
                    EligibleFiscalPayments eligibleFiscalPayment = new EligibleFiscalPayments();
                    eligibleFiscalPayment.siteId = Convert.ToInt32(row["SiteId"]);
                    eligibleFiscalPayment.paymentAmount = Convert.ToDecimal(row["Amount"]);
                    eligibleFiscalPayment.paymentId = Convert.ToInt32(row["PaymentId"]);
                    eligibleFiscalPayment.paymentMode = row["paymentmode"].ToString();
                    eligibleFiscalPayment.isCash = row["isCash"].ToString();
                    eligibleFiscalPayment.isDebitCard = row["isDebitCard"].ToString();
                    eligibleFiscalPayment.isCreditCard = row["isCreditCard"].ToString();
                    eligibleFiscalPayment.fiscalReference = row["ExternalSourceReference"].ToString();
                    eligibleFiscalPayment.posMachine = row["PosMachine"].ToString();
                    eligibleFiscalPayment.posFriendlyName = row["POSFriendlyName"].ToString();
                    eligibleAdvanceFiscalPayments.Add(eligibleFiscalPayment);
                }
            }
            log.LogMethodExit(eligibleAdvanceFiscalPayments);
            return eligibleAdvanceFiscalPayments;
        }


        /// <summary>
        /// GetEligibleBookingTrxPaymentsFinal
        /// </summary>
        /// <param name="trxId"></param>
        /// <returns>List of Eligible Final Booking Trx Payments</returns>
        private List<EligibleFiscalPayments> GetEligibleBookingTrxPaymentsFinal(int trxId)
        {
            log.LogMethodEntry(trxId);
            List<EligibleFiscalPayments> eligibleFiscalPayments = new List<EligibleFiscalPayments>();
            string unFiscalisedPaymentsQuery = @"select s.site_id SiteId, isnull((tp.Amount),0) Amount, tp.PaymentId,paymentmode, 
                                                isCash,isCreditCard,isDebitCard,tp.ExternalSourceReference,tp.PosMachine,pmach.FriendlyName POSFriendlyName
                                                from PaymentModes pm, trx_header th,TrxPayments tp, POSMachines pmach, site s
                                                where th.TrxId = tp.TrxId
                                                and tp.PaymentModeId = pm.PaymentModeId 
												and tp.PosMachine = pmach.POSName
												and pm.isDebitCard != 'Y' 
                                                and (th.site_id = s.site_id or th.site_id is null)
                                                and (tp.ExternalSourceReference is null or (tp.ExternalSourceReference like 'ZKI%' and tp.ExternalSourceReference like '%DEPOSIT%' 
                                                        and tp.ExternalSourceReference like '%REVERSED%' and tp.ExternalSourceReference not like '%COMPLETE%'))                                               
                                                and th.TrxId = @trxId ";

            string unFiscalisedDuplicatePaymentsQuery = @" union all
                                                          select s.site_id SiteId, isnull((tp.Amount),0) Amount, tp.PaymentId,paymentmode, 
                                                          isCash,isCreditCard,isDebitCard,tp.ExternalSourceReference ,tp.PosMachine,pmch.FriendlyName POSFriendlyName
                                                          from TrxPayments tp inner join
                                                          TrxPayments tp1 on tp.ExternalSourceReference = tp1.ExternalSourceReference
                                                          and tp.PaymentId != tp1.PaymentId, trx_header th, PaymentModes pm, POSMachines pmch, site s 
                                                          where th.TrxId = tp.TrxId
													      and tp.PaymentModeId = pm.PaymentModeId
														  and tp.PosMachine = pmch.POSName
                                                          and (th.site_id = s.site_id or th.site_id is null)
                                                          and pm.isDebitCard != 'Y'
                                                          and tp.Amount < 0
                                                          and tp.PaymentModeId = pm.PaymentModeId
                                                          and tp.ExternalSourceReference like 'ZKI%'
                                                          and tp.TrxId = @trxId ";

            List<SqlParameter> sqlParamTrxPaymentLines = new List<SqlParameter>();
            DataTable dtTrxPaymentsLines = null;
            sqlParamTrxPaymentLines.Add(new SqlParameter("@trxId", trxId));

            if (!string.IsNullOrEmpty(excludedPaymentModes))
            {
                DataAccessHandler dataAccessHandler = new DataAccessHandler();
                sqlParamTrxPaymentLines.AddRange(dataAccessHandler.GetSqlParametersForInClause(SQLParametersVariable.EXCLUDED_PAYMENT_MODES, excludedPaymentModes));
                unFiscalisedPaymentsQuery = unFiscalisedPaymentsQuery + @" and pm.PaymentMode not in (" + dataAccessHandler.GetInClauseParameterName(SQLParametersVariable.EXCLUDED_PAYMENT_MODES, excludedPaymentModes) + @")";
                unFiscalisedDuplicatePaymentsQuery = unFiscalisedDuplicatePaymentsQuery + @" and pm.PaymentMode not in (" + dataAccessHandler.GetInClauseParameterName(SQLParametersVariable.EXCLUDED_PAYMENT_MODES, excludedPaymentModes) + @")";
            }
            dtTrxPaymentsLines = Utilities.executeDataTable(unFiscalisedPaymentsQuery + unFiscalisedDuplicatePaymentsQuery, sqlParamTrxPaymentLines.ToArray());

            if (dtTrxPaymentsLines.Rows.Count > 0)
            {
                foreach (DataRow row in dtTrxPaymentsLines.Rows)
                {
                    EligibleFiscalPayments eligibleFiscalPayment = new EligibleFiscalPayments();
                    eligibleFiscalPayment.siteId = Convert.ToInt32(row["SiteId"]);
                    eligibleFiscalPayment.paymentAmount = Convert.ToDecimal(row["Amount"]);
                    eligibleFiscalPayment.paymentId = Convert.ToInt32(row["PaymentId"]);
                    eligibleFiscalPayment.paymentMode = row["paymentmode"].ToString();
                    eligibleFiscalPayment.isCash = row["isCash"].ToString();
                    eligibleFiscalPayment.isDebitCard = row["isDebitCard"].ToString();
                    eligibleFiscalPayment.isCreditCard = row["isCreditCard"].ToString();
                    eligibleFiscalPayment.fiscalReference = row["ExternalSourceReference"].ToString();
                    eligibleFiscalPayment.posMachine = row["PosMachine"].ToString();
                    eligibleFiscalPayment.posFriendlyName = row["POSFriendlyName"].ToString();
                    eligibleFiscalPayments.Add(eligibleFiscalPayment);
                }
            }
            log.LogMethodExit(eligibleFiscalPayments);
            return eligibleFiscalPayments;
        }


        /// <summary>
        /// Send invoice
        /// </summary>
        /// <param name="invoice">Invoice to send</param>
        /// <param name="certificate">Signing certificate</param>
        /// <param name="setupService">Function to set service settings</param>
        private RacunOdgovor SendInvoice(RacunType invoice, X509Certificate2 certificate,
            Action<FiskalizacijaService> setupService = null)
        {
            log.LogMethodEntry(invoice, certificate, setupService);
            RacunOdgovor racunOdgovorResponse = null;
            RacunZahtjev request = new RacunZahtjev
            {
                Racun = invoice,
                Zaglavlje = GetRequestHeader()
            };

            racunOdgovorResponse = SignAndSendRequest<RacunZahtjev, RacunOdgovor>(request, x => x.racuni, certificate, setupService);
            log.LogMethodExit(racunOdgovorResponse);
            return racunOdgovorResponse;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="request"></param>
        /// <param name="serviceMethod"></param>
        /// <param name="certificate"></param>
        /// <param name="setupService"></param>
        /// <returns></returns>
        private TResponse SignAndSendRequest<TRequest, TResponse>(TRequest request,
            Func<FiskalizacijaService, Func<TRequest, TResponse>> serviceMethod, X509Certificate2 certificate,
            Action<FiskalizacijaService> setupService = null)
            where TRequest : ICisRequest
            where TResponse : ICisResponse
        {
            log.LogMethodEntry(request, serviceMethod, certificate, setupService);
            // Create service endpoint
            FiskalizacijaService fs = new FiskalizacijaService(serviceUrl);
            // fs.CheckResponseSignature = true;
            if (setupService != null)
            {
                setupService(fs);
            }
            Sign(request, certificate);
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12;
            var method = serviceMethod(fs);
            var result = method(request);
            result.Request = request;
            log.LogMethodExit(result);
            return result;
        }


        /// <summary>
        /// Sign method
        /// </summary>
        /// <param name="request"></param>
        /// <param name="certificate"></param>
        private static void Sign(ICisRequest request, X509Certificate2 certificate)
        {
            log.LogMethodEntry(request, certificate);
            if (request.Signature != null)
                // Already signed
                return;

            // Check if ZKI is generated
            RacunZahtjev invoiceRequest = request as RacunZahtjev;
            if (invoiceRequest != null && invoiceRequest.Racun.ZastKod == null)
                GenerateZki(invoiceRequest.Racun, certificate);

            request.Id = request.GetType().Name;

            #region Sign request XML

            SignedXml xml = null;
            string ser = Serialize(request);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(ser);
            log.LogVariableState("Fiscalization XML Header:", ser);

            xml = new SignedXml(doc);
            xml.SigningKey = certificate.PrivateKey;
            xml.SignedInfo.CanonicalizationMethod = SignedXml.XmlDsigExcC14NTransformUrl;

            KeyInfo keyInfo = new KeyInfo();
            KeyInfoX509Data keyInfoData = new KeyInfoX509Data();
            keyInfoData.AddCertificate(certificate);
            keyInfoData.AddIssuerSerial(certificate.Issuer, certificate.GetSerialNumberString());
            keyInfo.AddClause(keyInfoData);
            xml.KeyInfo = keyInfo;

            Transform[] transforms = new Transform[]
            {
                new XmlDsigEnvelopedSignatureTransform(false),
                new XmlDsigExcC14NTransform(false)
            };

            Reference reference = new Reference("#" + request.Id);
            foreach (Transform x in transforms)
                reference.AddTransform(x);
            xml.AddReference(reference);
            xml.ComputeSignature();

            #endregion

            #region Fill request with signature data

            Signature s = xml.Signature;
            X509IssuerSerial certSerial = (X509IssuerSerial)keyInfoData.IssuerSerials[0];
            request.Signature = new SignatureType
            {
                SignedInfo = new SignedInfoType
                {
                    CanonicalizationMethod = new CanonicalizationMethodType { Algorithm = s.SignedInfo.CanonicalizationMethod },
                    SignatureMethod = new SignatureMethodType { Algorithm = s.SignedInfo.SignatureMethod },
                    Reference =
                        (from x in s.SignedInfo.References.OfType<Reference>()
                         select new ReferenceType
                         {
                             URI = x.Uri,
                             Transforms =
                                 (from t in transforms
                                  select new TransformType { Algorithm = t.Algorithm }).ToArray(),
                             DigestMethod = new DigestMethodType { Algorithm = x.DigestMethod },
                             DigestValue = x.DigestValue
                         }).ToArray()
                },
                SignatureValue = new SignatureValueType { Value = s.SignatureValue },
                KeyInfo = new KeyInfoType
                {
                    ItemsElementName = new[] { ItemsChoiceType2.X509Data },
                    Items = new[]
                    {
                        new X509DataType
                        {
                            ItemsElementName = new[]
                            {
                                ItemsChoiceType.X509IssuerSerial,
                                ItemsChoiceType.X509Certificate
                            },
                            Items = new object[]
                            {
                                new X509IssuerSerialType
                                {
                                    X509IssuerName = certSerial.IssuerName,
                                    X509SerialNumber = certSerial.SerialNumber
                                },
                                certificate.RawData
                            }
                        }
                    }
                }
            };
            #endregion
            log.LogMethodExit();
        }


        /// <summary>
        /// GetResponseErrors method
        /// </summary>
        /// <param name="response"></param>
        private string GetResponseErrors(ICisResponse response)
        {
            log.LogMethodEntry(response);
            string errorMessage = "";
            GreskaType[] errors = response.Greske != null ? response.Greske : null;
            if (response is ProvjeraOdgovor)
            {
                // Remove "valid error" from response
                errors = errors.Where(x => x.SifraGreske != "v100").ToArray();
                response.Greske = errors;
            }
            if (errors != null)
            {
                foreach (GreskaType greskaType in errors)
                {
                    errorMessage = string.Join("\n", greskaType.PorukaGreske.Trim());
                }
                errorMessage = "Error :" + errorMessage;
            }
            log.LogMethodExit(errorMessage);
            log.LogVariableState("errorMessage", errorMessage);
            return errorMessage;
        }


        /// <summary>
        /// GetRequestHeader method
        /// </summary>
        /// <returns></returns>
        private ZaglavljeType GetRequestHeader()
        {
            log.LogMethodEntry();
            //Header
            ZaglavljeType zaglavljeType = new ZaglavljeType();
            zaglavljeType.IdPoruke = Guid.NewGuid().ToString();
            zaglavljeType.DatumVrijeme = Utilities.getServerTime().ToString(dateFormatLong);
            log.LogMethodExit(zaglavljeType);
            return zaglavljeType;
        }


        /// <summary>
        /// GenerateZki method
        /// </summary>
        /// <param name="invoice"></param>
        /// <param name="certificate"></param>
        private static void GenerateZki(RacunType invoice, X509Certificate2 certificate)
        {
            log.LogMethodEntry(invoice, certificate);
            StringBuilder sb = new StringBuilder();
            sb.Append(invoice.Oib);
            sb.Append(invoice.DatVrijeme);
            sb.Append(invoice.BrRac.BrOznRac);
            sb.Append(invoice.BrRac.OznPosPr);
            sb.Append(invoice.BrRac.OznNapUr);
            sb.Append(invoice.IznosUkupno);
            invoice.ZastKod = SignAndHashMD5(sb.ToString(), certificate);
            log.LogVariableState("ZastKod", invoice.ZastKod);
            log.LogMethodExit();
        }


        /// <summary>
        /// SignAndHashMD5 method
        /// </summary>
        /// <param name="value"></param>
        /// <param name="certificate"></param>
        /// <returns></returns>
        private static string SignAndHashMD5(string value, X509Certificate2 certificate)
        {
            log.LogMethodEntry(value, certificate);

            // Sign data
            byte[] b = Encoding.ASCII.GetBytes(value);
            RSACryptoServiceProvider provider = (RSACryptoServiceProvider)certificate.PrivateKey;
            byte[] signData = provider.SignData(b, new SHA1CryptoServiceProvider());

            // Compute hash
            MD5 md5 = MD5.Create();
            byte[] hash = md5.ComputeHash(signData);
            string result = new string(hash.SelectMany(x => x.ToString("x2")).ToArray());
            log.LogMethodExit(result);
            return result;
        }


        /// <summary>
        /// Serialize method
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private static string Serialize(ICisRequest request)
        {
            log.LogMethodEntry(request);
            // Fix empty arrays to null
            if (request is RacunZahtjev)
            {
                var rz = (RacunZahtjev)request;
                var r = rz.Racun;
                Action<Array, Action> fixArray = (x, y) =>
                {
                    var isEmpty = x != null && !x.OfType<object>().Any(x1 => x1 != null);
                    if (isEmpty)
                        y();
                };
                fixArray(r.Naknade, () => r.Naknade = null);
                fixArray(r.OstaliPor, () => r.OstaliPor = null);
                fixArray(r.Pdv, () => r.Pdv = null);
                fixArray(r.Pnp, () => r.Pnp = null);
            }

            using (var ms = new MemoryStream())
            {
                // Set namespace to root element
                var root = new XmlRootAttribute { Namespace = "http://www.apis-it.hr/fin/2012/types/f73", IsNullable = false };
                var ser = new XmlSerializer(request.GetType(), root);
                ser.Serialize(ms, request);
                string UTF8String = Encoding.UTF8.GetString(ms.ToArray());
                log.LogMethodExit(UTF8String);
                return UTF8String;
            }

        }


        /// <summary>
        /// PrintMultipleReceipt
        /// </summary>
        /// <param name="unfiscalTrxIds"></param>
        /// <returns></returns>
        public override bool PrintMultipleReceipt(List<int> unfiscalTrxIds)
        {
            log.LogMethodEntry(unfiscalTrxIds);
            List<ValidationError> validationErrors = new List<ValidationError>();
            foreach (int trxId in unfiscalTrxIds)
            {
                try
                {
                    bool fiscalResponse = BuildXMLInvoiceAndSend(trxId);
                }
                catch (ValidationException ex)
                {
                    foreach (ValidationError validationError in ex.ValidationErrorList)
                    {
                        validationErrors.Add(validationError);
                    }
                }
            }
            log.LogMethodExit();

            if (validationErrors.Count > 0)
            {
                throw new ValidationException("Fiscalization Error", validationErrors);
            }
            log.LogMethodExit(true);
            return true;
        }


        /// <summary>
        /// IsBookingTransaction
        /// </summary>
        /// <param name="trxId"></param>
        /// <returns>True if Transaction is of the type Booking</returns>
        private bool IsBookingTransaction(int trxId)
        {
            log.LogMethodEntry(trxId);
            bool IsBookingTransaction = false;
            DataTable dtTransaction = Utilities.executeDataTable(@"select * from trx_header th, Bookings b where th.TrxId = b.TrxId 
                                                                    and th.TrxId =@trxid", new SqlParameter("@trxId", trxId));
            if (dtTransaction != null && dtTransaction.Rows.Count > 0)
            {
                IsBookingTransaction = true;
            }
            log.LogMethodExit(IsBookingTransaction);
            return IsBookingTransaction;
        }


        /// <summary>
        /// GetTransactionStatus
        /// </summary>
        /// <param name="trxId"></param>
        /// <returns>Transaction status</returns>
        private string GetTransactionStatus(int trxId)
        {
            log.LogMethodEntry(trxId);
            string trxStatus = string.Empty;
            DataTable dtTransaction = Utilities.executeDataTable(@"select status from trx_header where TrxId = @trxid", new SqlParameter("@trxId", trxId));
            if (dtTransaction != null && dtTransaction.Rows.Count > 0)
            {
                trxStatus = dtTransaction.Rows[0]["status"].ToString();
            }
            log.LogMethodExit(trxStatus);
            return trxStatus;
        }


        /// <summary>
        /// GetBookingStatus
        /// </summary>
        /// <param name="trxId"></param>
        /// <returns>Booking Status</returns>
        private string GetBookingStatus(int trxId)
        {
            log.LogMethodEntry(trxId);
            string bookingStatus = string.Empty;
            DataTable dtTransaction = Utilities.executeDataTable(@"select status from bookings where TrxId = @trxid", new SqlParameter("@trxId", trxId));
            if (dtTransaction != null && dtTransaction.Rows.Count > 0)
            {
                bookingStatus = dtTransaction.Rows[0]["status"].ToString();
            }
            log.LogMethodExit(bookingStatus);
            return bookingStatus;
        }


        /// <summary>
        /// GetFiscalizationTaxLines
        /// </summary>
        /// <param name="trxId"></param>
        /// <returns>List of Fiscalization TaxLines</returns>
        private List<FiscalTaxLines> GetFiscalizationTaxLines(int trxId, decimal payAmount, decimal totalAmount)
        {
            log.LogMethodEntry(trxId);
            List<FiscalTaxLines> fiscalTaxLinesList = new List<FiscalTaxLines>();
            List<SqlParameter> sqlParamTrxTaxLines = new List<SqlParameter>();
            sqlParamTrxTaxLines.Add(new SqlParameter("@trxId", trxId));
            DataTable dtTrxTaxLines = new DataTable();
            dtTrxTaxLines = Utilities.executeDataTable(@"select t.trxid,t.product_id ,sum(isnull(NetAmt,0.00)) - sum(isnull((v.DiscountAmount),0.00)) AmountWithDiscount ,
                                                         isnull(t.tax_percentage,0.00) tax_percentage, sum(isnull(TaxAmount,0.00)) TaxAmount, 
                                                        (sum(isnull(NetAmt,0.00)) - sum(isnull(v.DiscountAmount,0.00)) - sum(isnull(TaxAmount,0.00))) AmountWithoutTax
                                                        from trx_lines t left outer join
                                                        (select tl.TrxId,tl.LineId, tl.product_id,p.TaxInclusivePrice,tl.tax_percentage,
                                                        isnull(p.TaxInclusivePrice,'N') IsTaxInclusivePrice, Round(sum(tl.amount),2) NetAmt,
                                                        case when isnull(p.TaxInclusivePrice,'N') = 'Y' then
                                                        (sum(tl.amount) -sum(isnull(td.DiscountAmt,0.00)))*(isnull(tl.tax_percentage,0.00))/((100 + isnull(tl.tax_percentage,0.00)))
                                                        Else 
                                                        (sum(tl.amount) -sum((isnull(td.DiscountAmt,0.00))))*(isnull(tl.tax_percentage,0.00))/(100)
                                                        End TaxAmount,
                                                        sum(isnull(td.DiscountAmt,0.00)) DiscountAmount from trx_lines tl
                                                        left outer join (select trxid,LineId, isnull(sum(td.DiscountAmount),0.00) 
                                                        DiscountAmt from TrxDiscounts td  where  td.TrxId=@trxid group by trxid,LineId) td
                                                        on tl.TrxId = td.TrxId
                                                        and tl.LineId=td.LineId,products p
                                                        where tl.TrxId=@trxId
                                                        and p.product_id =tl.product_id
														group by tl.TrxId,tl.LineId, tl.product_id, DiscountAmt,tax_percentage,p.TaxInclusivePrice) v
														on v.TrxId = t.TrxId
														where t.TrxId=@trxId
														and t.LineId = v.LineId
														group by t.product_id,t.TrxId,t.tax_percentage,TaxInclusivePrice,v.TaxAmount", sqlParamTrxTaxLines.ToArray());

            if (dtTrxTaxLines != null && dtTrxTaxLines.Rows.Count > 0)
            {
                foreach (DataRow row in dtTrxTaxLines.Rows)
                {
                    FiscalTaxLines fiscalTaxLine = new FiscalTaxLines();
                    fiscalTaxLine.taxPercentage = Convert.ToDecimal(row["tax_percentage"]);
                    fiscalTaxLine.taxAmount = Math.Round(((Convert.ToDecimal(row["TaxAmount"]) * payAmount) / Math.Round(totalAmount,2)), 2);
                    fiscalTaxLine.amountWithoutTax = Math.Round(((Convert.ToDecimal(row["AmountWithoutTax"]) * payAmount) / totalAmount), 2);
                    fiscalTaxLinesList.Add(fiscalTaxLine);
                }
            }
            log.LogMethodExit(fiscalTaxLinesList);
            return fiscalTaxLinesList;
        }


        /// <summary>
        /// GetReservationDepositTaxReferenceProduct
        /// </summary>
        /// <returns>Product details of Reservation product used for deposit</returns>
        private DataTable GetReservationDepositTaxReferenceProduct()
        {
            log.LogMethodEntry();
            DataTable dtProduct = null;
            dtProduct = Utilities.executeDataTable(@"select t.tax_percentage, TaxInclusivePrice from Products p, tax t where p.tax_id = t.tax_id
                                                     and product_name =@productname", new SqlParameter("@productname", reservationAdvancePaymentProduct));
            log.LogMethodExit(dtProduct);
            return dtProduct;
        }


        /// <summary>
        /// Fiscalize Receipt
        /// </summary>
        /// <returns>Returns ZKI|JIR response</returns>
        private string FiscalizeReceipt(int trxId, CroatiaFiscalReceipt croatiaFiscalReceipt, string fiscalInvoiceId, decimal fiscalAmount)
        {
            log.LogMethodEntry(trxId, croatiaFiscalReceipt, fiscalInvoiceId, fiscalAmount);
            RacunType invoiceData = null;
            string posLocationId = string.Empty;
            string posId = string.Empty;
            string oibCashier = string.Empty;
            string externalResponse = string.Empty;
            NacinPlacanjaType nacinPlacanjaType = NacinPlacanjaType.O;
            OznakaSlijednostiType oznakaSlijednostiType = OznakaSlijednostiType.P;
            List<ValidationError> validationErrorList = new List<ValidationError>();
            string jirReference = string.Empty;

            NumberFormatInfo numberFormatInfo = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
            numberFormatInfo.NumberGroupSeparator = "";//No thousand separator 

            if (croatiaFiscalReceipt.fiscalTaxLines != null && croatiaFiscalReceipt.fiscalTaxLines.Count > 0)
            {
                DataTable dtTrxUser = Utilities.executeDataTable(@"select users.user_id, EmpNumber from trx_header, users where trx_header.TrxId=@trxId and trx_header.user_id = users.user_id",
                                                                 new SqlParameter("@trxId", trxId));
                if (dtTrxUser != null && dtTrxUser.Rows.Count > 0)
                {
                    int userId = Convert.ToInt32(dtTrxUser.Rows[0]["user_id"].ToString());
                    if (dtTrxUser.Rows[0]["EmpNumber"] != DBNull.Value && string.IsNullOrEmpty(dtTrxUser.Rows[0]["EmpNumber"].ToString()))
                    {
                        log.Error("Employee Number is not defined for the user");
                    }
                    else
                    {
                        oibCashier = dtTrxUser.Rows[0]["EmpNumber"].ToString();
                    }
                }

                PorezType[] taxLines = new PorezType[croatiaFiscalReceipt.fiscalTaxLines.Count];
                for (int i = 0; i < croatiaFiscalReceipt.fiscalTaxLines.Count; i++)
                {
                    PorezType porezType = new PorezType();
                    porezType.Stopa = decimal.Parse(croatiaFiscalReceipt.fiscalTaxLines[i].taxPercentage.ToString()).ToString("N2", numberFormatInfo);
                    porezType.Osnovica = decimal.Parse(croatiaFiscalReceipt.fiscalTaxLines[i].amountWithoutTax.ToString()).ToString("N2", numberFormatInfo);
                    porezType.Iznos = decimal.Parse(croatiaFiscalReceipt.fiscalTaxLines[i].taxAmount.ToString()).ToString("N2", numberFormatInfo);
                    log.LogVariableState("Stopa(Tax percentage)", porezType.Stopa);
                    log.LogVariableState("Osnovica(Base Price)", porezType.Osnovica);
                    log.LogVariableState("Iznos(Tax Amount)", porezType.Iznos);
                    taxLines[i] = porezType;
                }

                if (croatiaFiscalReceipt.eligibleFiscalPayments != null && croatiaFiscalReceipt.eligibleFiscalPayments.Count > 1)
                {
                    nacinPlacanjaType = NacinPlacanjaType.O;
                    log.LogVariableState("Payment mode", NacinPlacanjaType.O);
                }
                else
                {
                    if (croatiaFiscalReceipt.eligibleFiscalPayments[0].isCash.ToString() == "Y")
                    {
                        nacinPlacanjaType = NacinPlacanjaType.G;
                        log.LogVariableState("Payment mode", NacinPlacanjaType.G);
                    }
                    else if (croatiaFiscalReceipt.eligibleFiscalPayments[0].isCreditCard.ToString() == "Y")
                    {
                        nacinPlacanjaType = NacinPlacanjaType.K;
                        log.LogVariableState("Payment mode", NacinPlacanjaType.K);
                    }
                    else
                    {
                        nacinPlacanjaType = NacinPlacanjaType.O;
                        log.LogVariableState("Payment mode", NacinPlacanjaType.O);
                    }
                }

                if (accountNumberSequenceTag.ToUpper() == "P")
                {
                    oznakaSlijednostiType = OznakaSlijednostiType.P;
                    log.LogVariableState("AccountNumberSequenceTag", OznakaSlijednostiType.P);

                }
                else if (accountNumberSequenceTag.ToUpper() == "N")
                {
                    oznakaSlijednostiType = OznakaSlijednostiType.N;
                    log.LogVariableState("AccountNumberSequenceTag", OznakaSlijednostiType.N);
                }

                string posFriendlyName = croatiaFiscalReceipt.eligibleFiscalPayments[0].posFriendlyName.ToString();
                if (!string.IsNullOrEmpty(posFriendlyName))
                {
                    string[] posFriendlyNameArray;
                    string[] separators = { "-", "/", ".", "\\" };
                    posFriendlyNameArray = posFriendlyName.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                    if (posFriendlyNameArray.Length == 2)
                    {
                        posLocationId = posFriendlyNameArray[0].Trim().ToString(); ;
                        posId = posFriendlyNameArray[1].Trim().ToString();
                        log.LogVariableState("Pos LocationId", posLocationId);
                        log.LogVariableState("posId", posId);
                    }
                }

                DateTime fiscalizationTime = Utilities.getServerTime();

                RacunType invoice = new RacunType()
                {
                    BrRac = new BrojRacunaType()
                    {
                        BrOznRac = fiscalInvoiceId.ToString(),
                        OznPosPr = posLocationId,
                        OznNapUr = posId
                    },
                    DatVrijeme = fiscalizationTime.ToString(dateFormatLong),
                    IznosUkupno = fiscalAmount.ToString("N2", numberFormatInfo),
                    NacinPlac = nacinPlacanjaType,
                    NakDost = false,
                    Oib = oibNumber,
                    OibOper = oibCashier,
                    OznSlijed = oznakaSlijednostiType,
                    Pdv = taxLines,
                    USustPdv = IsVatSystem
                };
                invoiceData = invoice;

                log.LogVariableState("BrOznRac(Account Number)", fiscalInvoiceId);
                log.LogVariableState("IznosUkupno(Fiscal Amount)", fiscalAmount);
                log.LogVariableState("NacinPlac(Payment Mode)", nacinPlacanjaType);
                log.LogVariableState("OznSlijed(Account Number Sequence Tag)", accountNumberSequenceTag);

                RacunOdgovor FiscalResult = null;

                int loopCount = 0;
                while (loopCount < 3 && FiscalResult == null) //To check for 3 iteration when IRS server cannot be connected
                {
                    try
                    {
                        FiscalResult = SendInvoice(invoiceData, certificate, SetFiscalizationService);

                        if (FiscalResult.Jir != null)
                        {
                            jirReference = "ZKI:" + ((Cis.RacunZahtjev)FiscalResult.Request).Racun.ZastKod + "|" + "JIR:" + FiscalResult.Jir + "|" +
                                                    fiscalInvoiceId + "|" + fiscalizationTime.ToString("yyyyMMdd_HHmm") + "|" + fiscalAmount.ToString("N2");
                        }
                        else
                        {
                            externalResponse = GetResponseErrors(FiscalResult);
                            validationErrorList.Add(new ValidationError("FISCALIZATION", "", "Error in Fiscalizing Payment Amount " + fiscalAmount + " : " + externalResponse, trxId));
                            log.Error(externalResponse);
                        }
                        break;
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        if (loopCount == 2)
                        {
                            validationErrorList.Add(new ValidationError("FISCALIZATION", "", "Error in Fiscalizing Payment Amount " + fiscalAmount + " : " + ex.Message, trxId));
                        }
                        loopCount++;
                    }
                }

                if (validationErrorList != null && validationErrorList.Count > 0)
                {
                    throw new ValidationException("Fiscalization Error", validationErrorList);
                }
            }
            log.LogMethodExit(jirReference);
            return jirReference;
        }


        /// <summary>
        /// Update Trx Payment Lines
        /// </summary>
        /// <param name="jirReference"></param>
        /// <param name="paymentIdList"></param>
        /// <param name="isReversal"></param>
        private void UpdateTrxPaymentLines(string jirReference, List<int> paymentIdList, bool isReversal = false)
        {
            log.LogMethodEntry(jirReference);
            try
            {
                LookupValuesList lookupValuesList = new LookupValuesList();
                //Update payment level ZKI and JIR
                foreach (int paymentId in paymentIdList)
                {
                    try
                    {
                        if (isReversal)
                        {
                            Utilities.executeScalar(@"UPDATE TrxPayments 
                                                    SET ExternalSourceReference= @externalSourceReference, LastUpdateDate = @lastUpdateDate where PaymentId = @PaymentId",
                             new SqlParameter("@externalSourceReference", jirReference),
                             new SqlParameter("@PaymentId", paymentId),
                             new SqlParameter("@lastUpdateDate", lookupValuesList.GetServerDateTime()));
                        }
                        else
                        {
                            Utilities.executeScalar(@"UPDATE TrxPayments 
                                                    SET ExternalSourceReference= isnull(ExternalSourceReference,'') + @externalSourceReference, LastUpdateDate = @lastUpdateDate
                                                    where PaymentId = @PaymentId",
                             new SqlParameter("@externalSourceReference", jirReference),
                             new SqlParameter("@PaymentId", paymentId),
                             new SqlParameter("@lastUpdateDate", lookupValuesList.GetServerDateTime()));
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error("Error while updating externalSourceReference in TrxPayments JIR: " + jirReference + " for PaymentId " + paymentId + " Error: " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Get Previous Fiscalized Deposit Amount
        /// </summary>
        /// <param name="depositPaymentLine"></param>
        /// <returns>Previously Fiscalized Deposit Amount</returns>
        private decimal GetPreviousFiscalizedDepositAmount(EligibleFiscalPayments depositPaymentLine)
        {
            log.LogMethodEntry(depositPaymentLine);
            decimal fiscalizedDeposit = 0;
            string[] fiscalReference = depositPaymentLine.fiscalReference.Split('/');
            if (!string.IsNullOrWhiteSpace(fiscalReference[0]))
            {
                string[] fiscalPaymentReference = depositPaymentLine.fiscalReference.Split('|');
                if (!string.IsNullOrWhiteSpace(fiscalReference[3]))
                {
                    fiscalizedDeposit = Convert.ToDecimal(fiscalPaymentReference[3]);
                }
            }
            log.LogMethodExit(fiscalizedDeposit);
            return fiscalizedDeposit;
        }


        /// <summary>
        /// Get Previous Fiscalized Tax Percentage
        /// </summary>
        /// <param name="depositPaymentLine"></param>
        /// <returns>Previously used TaxPercentage for deposit fiscalization</returns>
        private decimal GetPreviousFiscalizedTaxPercentage(EligibleFiscalPayments depositPaymentLine)
        {
            log.LogMethodEntry(depositPaymentLine);
            decimal fiscalizedTaxPerc = 0;
            string[] fiscalReference = depositPaymentLine.fiscalReference.Split('/');
            if (!string.IsNullOrWhiteSpace(fiscalReference[2]))
            {
                fiscalizedTaxPerc = Convert.ToDecimal(fiscalReference[2]);
            }
            log.LogMethodExit(fiscalizedTaxPerc);
            return fiscalizedTaxPerc;
        }



        /// <summary>
        /// Get Transaction Net Amount
        /// </summary>
        /// <param name="trxId"></param>
        /// <returns>Transaction Net Amount</returns>
        private decimal GetTransactionNetAmount(int trxId)
        {
            log.LogMethodEntry(trxId);
            List<SqlParameter> sqlParamTotalPayment = new List<SqlParameter>();
            DataTable dtTotalPayment = null;
            decimal TotalNetAmount = 0;
            sqlParamTotalPayment.Add(new SqlParameter("@trxId", trxId));
            dtTotalPayment = Utilities.executeDataTable(@"select sum(isnull(tl.amount,0.00)) - sum(isnull(v.DiscountAmt,0.00)) TotalAmountWithDiscount
                                                        from trx_lines tl left outer join (select trxid,LineId, isnull(sum(td.DiscountAmount),0.00) DiscountAmt 
                                                        from TrxDiscounts td  where td.TrxId=@trxId group by trxid,LineId) v on tl.TrxId = v.TrxId and tl.LineId=v.LineId
                                                        where tl.TrxId=@trxId", sqlParamTotalPayment.ToArray());
            if (dtTotalPayment != null && dtTotalPayment.Rows.Count > 0)
            {
                //TotalNetAmount is used to use for proportionate while sending Tax Lines. This is the Total Amount to be paid 
                TotalNetAmount = decimal.Parse(dtTotalPayment.Rows[0]["TotalAmountWithDiscount"].ToString());
                log.LogVariableState("Tota Transaction Amount With Discount", TotalNetAmount);
            }
            log.LogMethodExit(TotalNetAmount);
            return TotalNetAmount;
        }

        
        /// <summary>
        /// Creates Concurrent request for failed fiscalization's
        /// </summary>
        /// <param name="utilities"></param>
        /// <param name="trxId"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private void LogFiscalizationError(int trxId, string message)
        {
            log.LogMethodEntry(trxId, message);
            LookupValuesList lookupValuesList = new LookupValuesList(utilities.ExecutionContext);
            DateTime dt = lookupValuesList.GetServerDateTime();
            string currentDate = dt.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            currentDate = string.Concat(currentDate, " 00:00:00");
            int croatiaFiscalProgramId = -1;
            log.Debug("The request start date : " + currentDate);

            List<KeyValuePair<ConcurrentProgramsDTO.SearchByProgramsParameters, string>> searchByProgramsParameters =
                new List<KeyValuePair<ConcurrentProgramsDTO.SearchByProgramsParameters, string>>();
            searchByProgramsParameters.Add(new KeyValuePair<ConcurrentProgramsDTO.SearchByProgramsParameters, string>
                (ConcurrentProgramsDTO.SearchByProgramsParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));
            searchByProgramsParameters.Add(new KeyValuePair<ConcurrentProgramsDTO.SearchByProgramsParameters, string>
                (ConcurrentProgramsDTO.SearchByProgramsParameters.PROGRAM_NAME, "croatiafiscalization"));
            searchByProgramsParameters.Add(new KeyValuePair<ConcurrentProgramsDTO.SearchByProgramsParameters, string>
                (ConcurrentProgramsDTO.SearchByProgramsParameters.ACTIVE_FLAG, "1"));
            ConcurrentProgramList concurrentProgramList = new ConcurrentProgramList(utilities.ExecutionContext);
            List<ConcurrentProgramsDTO> concurrentProgramsDTOList =
                concurrentProgramList.GetAllConcurrentPrograms(searchByProgramsParameters);

            if (concurrentProgramsDTOList != null)
            {
                log.Debug("Concurrent program ID :" + concurrentProgramsDTOList.First().ProgramId);
                croatiaFiscalProgramId = concurrentProgramsDTOList.First().ProgramId;
                string parafaitObject = string.Empty;

                ConcurrentRequestDetailsListBL concurrentRequestDetailsListBL = new ConcurrentRequestDetailsListBL(utilities.ExecutionContext);
                List<KeyValuePair<ConcurrentRequestDetailsDTO.SearchByParameters, string>> rsearchParameters = new List<KeyValuePair<ConcurrentRequestDetailsDTO.SearchByParameters, string>>();
                rsearchParameters.Add(new KeyValuePair<ConcurrentRequestDetailsDTO.SearchByParameters, string>(ConcurrentRequestDetailsDTO.SearchByParameters.CONCURRENT_PROGRAM_ID, croatiaFiscalProgramId.ToString()));
                rsearchParameters.Add(new KeyValuePair<ConcurrentRequestDetailsDTO.SearchByParameters, string>(ConcurrentRequestDetailsDTO.SearchByParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));
                List<ConcurrentRequestDetailsDTO> result = concurrentRequestDetailsListBL.GetConcurrentRequestDetailsDTOList(rsearchParameters);
                if (result == null || result.Any() == false)
                {
                    ConcurrentRequestsDTO concurrentRequestsDTO = new ConcurrentRequestsDTO(-1, concurrentProgramsDTOList.First().ProgramId, -1,
                                          DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                                          utilities.ExecutionContext.GetUserId(),
                                          DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                                          DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), string.Empty,
                                          "Running", "Normal", false, string.Empty, string.Empty,
                                          string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                                          string.Empty, string.Empty, -1, -1, false);
                    ConcurrentRequests concurrentRequests = new ConcurrentRequests(utilities.ExecutionContext, concurrentRequestsDTO);
                    concurrentRequests.Save();
                    concurrentRequestsDTO = concurrentRequests.GetconcurrentRequests;
                    log.Debug("Concurrent Request ID :" + concurrentRequestsDTO.RequestId);
                    ConcurrentRequestDetailsDTO concurrentRequestDetailsDTO = new ConcurrentRequestDetailsDTO(-1, concurrentRequestsDTO.RequestId, dt,
                        concurrentRequestsDTO.ProgramId, parafaitObject, trxId, String.Empty, false, "Failed", String.Empty, String.Empty, message, true);
                    ConcurrentRequestDetailsBL concurrentRequestDetailsBL = new ConcurrentRequestDetailsBL(utilities.ExecutionContext, concurrentRequestDetailsDTO);
                    concurrentRequestDetailsBL.Save();
                }
            }
            else
            {
                log.Error("ERROR: Concurrent Program for croatia fiscalization does not exit");
            }
            log.LogMethodExit();
        }
    }
}
