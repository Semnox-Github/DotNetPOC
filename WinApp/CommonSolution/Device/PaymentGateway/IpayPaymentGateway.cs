/********************************************************************************************
 * Project Name - IpayPaymentGateway
 * Description  - IpayPaymentGateway class
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.100.0    19-Aug-2020      Jinto Thomas    Created              
 *2.110.0       30-Dec-2020      Girish Kundar       Modified : Added delete method = Payment link changes
 *2.130.3.1  25-Nov-2022      Muaaz Musthafa  Fixed : Corrected Original Payment date in RefundAmount() 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Semnox.Parafait.Device.PaymentGateway
{
    class IpayPaymentGateway : PaymentGateway
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        string merchantCode;
        string merchantKey;
        string serverUrl;
        string requeryUrl;
        string voidUrl;
        string signatureType;
        string currencyCode;
        private static Dictionary<string, string> voidTransactionErrCode = new Dictionary<string, string>();
        public enum RecieptPrintAlignment
        {
            Left,
            Right,
            Center
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="utilities"></param>
        /// <param name="isUnattended"></param>
        /// <param name="showMessageDelegate"></param>
        /// <param name="writeToLogDelegate"></param>
        public IpayPaymentGateway(Utilities utilities, bool isUnattended, ShowMessageDelegate showMessageDelegate, WriteToLogDelegate writeToLogDelegate)
           : base(utilities, isUnattended, showMessageDelegate, writeToLogDelegate)
        {
            log.LogMethodEntry(utilities, isUnattended, showMessageDelegate, writeToLogDelegate);

            try
            {
                merchantCode = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext,"CREDIT_CARD_STORE_ID");
                merchantKey = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "CREDIT_CARD_TOKEN_ID");
                serverUrl = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "CREDIT_CARD_HOST_URL");
                requeryUrl = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "CREDIT_CARD_REQUERY_URL");
                voidUrl = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "CREDIT_CARD_VOID_URL");
                currencyCode = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "CURRENCY_CODE");
                

                if (string.IsNullOrEmpty(merchantCode))
                {
                    throw new Exception(utilities.MessageUtils.getMessage("Merchant Code is not set"));
                }
                if (string.IsNullOrEmpty(merchantKey))
                {
                    throw new Exception(utilities.MessageUtils.getMessage("Merchant Key is not set"));
                }
                if (string.IsNullOrEmpty(serverUrl))
                {
                    throw new Exception(utilities.MessageUtils.getMessage("serverUrl is not set"));
                }
                if (string.IsNullOrEmpty(requeryUrl))
                {
                    throw new Exception(utilities.MessageUtils.getMessage("requeryUrl is not set"));
                }
                if (string.IsNullOrEmpty(voidUrl))
                {
                    throw new Exception(utilities.MessageUtils.getMessage("voidUrl is not set"));
                }
                if (string.IsNullOrEmpty(currencyCode))
                {
                    throw new Exception(utilities.MessageUtils.getMessage("serverUrl is not set"));
                }
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                

            }
            catch (Exception ex)
            {
                log.Error("Error occured while initialize gateway", ex);
                throw;
            }

            log.LogMethodExit(null);
        }
        /// <summary>
        /// 
        /// </summary>
        public override void Initialize()
        {
            log.LogMethodEntry();
            CheckLastTransactionStatus();
            InitializeVoidErrorCode();
            log.LogMethodExit();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cCRequestPGWDTO"></param>
        /// <param name="cCTransactionsPGWDTO"></param>
        public override void SendLastTransactionStatusCheckRequest(CCRequestPGWDTO cCRequestPGWDTO, CCTransactionsPGWDTO cCTransactionsPGWDTO)
        {
            log.LogMethodEntry(cCRequestPGWDTO, cCTransactionsPGWDTO);

            log.Debug(utilities.MessageUtils.getMessage("Checking the transaction status" + ((cCRequestPGWDTO != null) ? " of TrxId:" + cCRequestPGWDTO.InvoiceNo + " Amount:" + cCRequestPGWDTO.POSAmount : ".")));

            if (cCRequestPGWDTO != null)
            {
                log.Debug("cCRequestPGWDTO is not null");

                TransactionInquiry transactionInquiry = RequeryPayment(cCTransactionsPGWDTO);
                
                if (transactionInquiry.Status == "1")
                {
                    try
                    {
                        log.Debug("There is transaction exist");
                        log.LogVariableState("query response", transactionInquiry);
                        CCTransactionsPGWDTO ccTransactionsPGWDTOResponse = new CCTransactionsPGWDTO();
                        ccTransactionsPGWDTOResponse.InvoiceNo = cCRequestPGWDTO.RequestID.ToString();
                        ccTransactionsPGWDTOResponse.RefNo = transactionInquiry.RefNo;
                        ccTransactionsPGWDTOResponse.Authorize = transactionInquiry.Amount;
                        ccTransactionsPGWDTOResponse.AuthCode = transactionInquiry.TransId;
                        ccTransactionsPGWDTOResponse.TranCode = PaymentGatewayTransactionType.SALE.ToString();
                        ccTransactionsPGWDTOResponse.TransactionDatetime = utilities.getServerTime();
                        ccTransactionsPGWDTOResponse.CardType = "credit";
                        ccTransactionsPGWDTOResponse.RecordNo = "A";
                        ccTransactionsPGWDTOResponse.TextResponse = "Approved";

                        if (cCTransactionsPGWDTO == null)
                        {
                            log.Debug("Saving ccTransactionsPGWDTOResponse.");
                            CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(ccTransactionsPGWDTOResponse);
                            ccTransactionsPGWBL.Save();
                        }
                        log.LogVariableState("ccTransactionsPGWDTOResponse", ccTransactionsPGWDTOResponse);
                        if (!string.IsNullOrEmpty(ccTransactionsPGWDTOResponse.RefNo) && ccTransactionsPGWDTOResponse.RecordNo.Equals("A"))
                        {
                            TransactionPaymentsDTO transactionPaymentsDTO = new TransactionPaymentsDTO();
                            try
                            {
                                transactionPaymentsDTO.TransactionId = Convert.ToInt32(cCRequestPGWDTO.InvoiceNo);
                            }
                            catch
                            {
                                log.Debug("Transaction id conversion is failed");
                            }
                            transactionPaymentsDTO.Amount = Convert.ToDouble(ccTransactionsPGWDTOResponse.Authorize);
                            transactionPaymentsDTO.CCResponseId = (cCTransactionsPGWDTO == null) ? ccTransactionsPGWDTOResponse.ResponseID : cCTransactionsPGWDTO.ResponseID;
                            log.LogVariableState("transactionPaymentsDTO", transactionPaymentsDTO);
                            log.Debug("Calling RefundAmount()");
                            transactionPaymentsDTO = RefundAmount(transactionPaymentsDTO);
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Debug("Exception one");
                        log.Error("Last transaction check failed", ex);
                        throw;
                    }
                }
                else
                {
                    log.Debug("There is no transaction exists");
                    log.Error("Last transaction status is not available.");
                    return;
                }
            }


            log.LogMethodExit();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transactionPaymentsDTO"></param>
        /// <returns></returns>
        public override TransactionPaymentsDTO MakePayment(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            try
            {
                signatureType = "SHA256";
                CCRequestPGWDTO ccRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, CREDIT_CARD_PAYMENT);
                ClientRequestModel clientRequestModel = new ClientRequestModel();
                clientRequestModel = CreateRequestModel(transactionPaymentsDTO, ccRequestPGWDTO);

                
                BasicHttpsBinding_IGatewayService basicHttpsBinding_IGatewayService = new BasicHttpsBinding_IGatewayService();
                basicHttpsBinding_IGatewayService.Url = serverUrl;
                ClientResponseModel clientResponseModel = basicHttpsBinding_IGatewayService.EntryPageFunctionality(clientRequestModel);

                log.LogVariableState("clientRequestModel", clientRequestModel);
                log.Debug("resposne");
                log.Debug(clientResponseModel);
                log.LogVariableState("clientResponseModel", clientResponseModel);

                if (clientResponseModel.Status == 1)
                {
                    CCTransactionsPGWDTO ccTransactionsPGWDTO = new CCTransactionsPGWDTO();
                    ccTransactionsPGWDTO.RefNo = clientResponseModel.RefNo;
                    ccTransactionsPGWDTO.Authorize = clientResponseModel.Amount;
                    ccTransactionsPGWDTO.AuthCode = clientResponseModel.TransId;
                    ccTransactionsPGWDTO.InvoiceNo = ccRequestPGWDTO.RequestID.ToString();
                    ccTransactionsPGWDTO.TranCode = PaymentGatewayTransactionType.SALE.ToString();
                    ccTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                    ccTransactionsPGWDTO.CardType = "Credit";
                    ccTransactionsPGWDTO.RecordNo = "A";
                    ccTransactionsPGWDTO.TextResponse = "Approved";
                    transactionPaymentsDTO.CreditCardNumber = clientRequestModel.BarcodeNo.Substring(clientRequestModel.BarcodeNo.Length - 4).PadLeft(clientRequestModel.BarcodeNo.Length, 'X');
                    ccTransactionsPGWDTO.AcctNo = transactionPaymentsDTO.CreditCardNumber;

                    List<LookupValuesDTO> lookupValuesDTOList;
                    LookupValuesList lookupValuesList = new LookupValuesList(utilities.ExecutionContext);
                    List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookUpValueSearchParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                    lookUpValueSearchParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME,"IPAY_PAYMENT_OPTIONS"));
                    lookupValuesDTOList = lookupValuesList.GetAllLookupValues(lookUpValueSearchParameters);

                    if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
                    {
                        var lookupValuesDTO = lookupValuesDTOList.Where(x => x.LookupValue == clientResponseModel.PaymentId.ToString()).FirstOrDefault();
                        if (lookupValuesDTO != null)
                        {
                            ccTransactionsPGWDTO.CardType = lookupValuesDTO.Description;
                        }
                        else
                        {
                            ccTransactionsPGWDTO.CardType = string.Empty;
                        }
                            
                    }
                    else
                    {
                        ccTransactionsPGWDTO.CardType = string.Empty;
                    }
                    ccTransactionsPGWDTO.ProcessData = clientResponseModel.PaymentId.ToString();
                    SendPrintReceiptRequest(transactionPaymentsDTO, ccTransactionsPGWDTO);

                    CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(ccTransactionsPGWDTO);
                    ccTransactionsPGWBL.Save();
                    transactionPaymentsDTO.CCResponseId = ccTransactionsPGWBL.CCTransactionsPGWDTO.ResponseID;
                    transactionPaymentsDTO.CreditCardName = ccTransactionsPGWBL.CCTransactionsPGWDTO.CardType;
                    //transactionPaymentsDTO.CreditCardAuthorization = ccTransactionsPGWBL.CCTransactionsPGWDTO.AuthCode;
                    transactionPaymentsDTO.Reference = ccTransactionsPGWBL.CCTransactionsPGWDTO.AuthCode;
                    transactionPaymentsDTO.PosMachine = clientRequestModel.TerminalID;

                    log.Debug("Payment Success");
                }
                else
                {
                    log.Debug("Payment Failed");
                    throw new Exception(clientResponseModel.ErrDesc);
                }
                log.LogMethodExit(transactionPaymentsDTO);
                return transactionPaymentsDTO;
            }
            catch(Exception ex)
            {
                log.Error("Error occured while making payment", ex);
                log.LogMethodExit(null, "Throwing Payment Gateway Exception-" + ex.Message);
                throw new PaymentGatewayException(ex.Message);
            }
                        
        }
        /// <summary>
        /// Initializing Error code for void transactions
        /// </summary>
        private void InitializeVoidErrorCode()
        {
            voidTransactionErrCode.Add("1", "Refer to card issuer ");
            voidTransactionErrCode.Add("3", "Invalid Merchant  ");
            voidTransactionErrCode.Add("4", "Retain Card ");
            voidTransactionErrCode.Add("5", "Do not honor ");
            voidTransactionErrCode.Add("6", "System error ");
            voidTransactionErrCode.Add("7", "Pick up card (special) ");
            voidTransactionErrCode.Add("12", "Invalid transaction ");
            voidTransactionErrCode.Add("13", "Invalid Amount ");
            voidTransactionErrCode.Add("14", "Invalid card number ");
            voidTransactionErrCode.Add("15", "Invalid issuer ");
            voidTransactionErrCode.Add("19", "System timeout ");
            voidTransactionErrCode.Add("20", "Invalid response ");
            voidTransactionErrCode.Add("21", "No action taken ");
            voidTransactionErrCode.Add("22", "Suspected malfunction ");
            voidTransactionErrCode.Add("30", "Format error ");
            voidTransactionErrCode.Add("33", "Expired card ");
            voidTransactionErrCode.Add("34", "Suspected fraud ");
            voidTransactionErrCode.Add("36", "Restricted card ");
            voidTransactionErrCode.Add("41", "Pick up card (lost) ");
            voidTransactionErrCode.Add("43", "Pick up card (stolen) ");
            voidTransactionErrCode.Add("51", "Not sufficient funds ");
            voidTransactionErrCode.Add("54", "Expired card");
            voidTransactionErrCode.Add("59", "Suspected fraud ");
            voidTransactionErrCode.Add("61", "Exceeds withdrawal limit ");
            voidTransactionErrCode.Add("62", "Restricted card ");
            voidTransactionErrCode.Add("63", "Security violation ");
            voidTransactionErrCode.Add("65", "Activity count exceeded ");
            voidTransactionErrCode.Add("91", "Issuer or switch inoperative ");
            voidTransactionErrCode.Add("96", "System malfunction");
            voidTransactionErrCode.Add("1001", "Merchant Code is empty ");
            voidTransactionErrCode.Add("1002", "Transaction ID is empty ");
            voidTransactionErrCode.Add("1003", "Amount is empty ");
            voidTransactionErrCode.Add("1004", "Currency is empty ");
            voidTransactionErrCode.Add("1005", "Signature is empty ");
            voidTransactionErrCode.Add("1006", "Signature not match ");
            voidTransactionErrCode.Add("1007", "Invalid Amount ");
            voidTransactionErrCode.Add("1008", "Invalid Currency ");
            voidTransactionErrCode.Add("1009", "Invalid Merchant Code ");
            voidTransactionErrCode.Add("1010", "This transaction is not eligible for voiding ");
            voidTransactionErrCode.Add("1011", "Transaction not found ");
            voidTransactionErrCode.Add("1012", "Connection error ");
            voidTransactionErrCode.Add("9999", "Transaction already voided ");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transactionPaymentsDTO"></param>
        /// <param name="ccRequestPGWDTO"></param>
        /// <returns></returns>
        private ClientRequestModel CreateRequestModel(TransactionPaymentsDTO transactionPaymentsDTO, CCRequestPGWDTO ccRequestPGWDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO, ccRequestPGWDTO);

            ClientRequestModel clientRequestModel = new ClientRequestModel();
            clientRequestModel.Amount = transactionPaymentsDTO.Amount.ToString(utilities.ParafaitEnv.AMOUNT_FORMAT);
            clientRequestModel.BarcodeNo = transactionPaymentsDTO.CreditCardNumber;
            clientRequestModel.Currency = currencyCode;
            clientRequestModel.MerchantCode = merchantCode;
            clientRequestModel.PaymentId = 0;
            clientRequestModel.ProdDesc = PaymentGatewayTransactionType.SALE.ToString();
            clientRequestModel.RefNo = utilities.ParafaitEnv.SiteId + "_" + ccRequestPGWDTO.RequestID;
            clientRequestModel.Remark = utilities.ParafaitEnv.SiteName;
            clientRequestModel.TerminalID = utilities.ParafaitEnv.POSMachine;
            string signatureString = GetPaymentSignatureString(clientRequestModel);
            clientRequestModel.Signature = GetPaymentSignature(signatureString);
            clientRequestModel.SignatureType = signatureType;
            clientRequestModel.UserName = "Offline Payment";
            clientRequestModel.UserContact = "01234567890";
            clientRequestModel.UserEmail = "offlinePayment@ipay88.com.my";

            log.LogMethodExit(clientRequestModel);
            return clientRequestModel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transactionPaymentsDTO"></param>
        /// <returns></returns>
        public override TransactionPaymentsDTO RefundAmount(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);

            if (transactionPaymentsDTO != null)
            {
                try
                {
                    CCTransactionsPGWBL ccOrigTransactionsPGWBL = new CCTransactionsPGWBL(transactionPaymentsDTO.CCResponseId);
                    CCTransactionsPGWDTO ccOrigTransactionsPGWDTO = ccOrigTransactionsPGWBL.CCTransactionsPGWDTO;
                    DateTime originalPaymentDate = new DateTime();

                    CCRequestPGWBL cCRequestPGWBL = new CCRequestPGWBL(utilities.ExecutionContext, Convert.ToInt32(ccOrigTransactionsPGWBL.CCTransactionsPGWDTO.InvoiceNo));
                    CCRequestPGWDTO ccOrigRequestPGWDTO = cCRequestPGWBL.CCRequestPGWDTO;
                    log.Debug("ccOrigRequestPGWDTO: " + ccOrigRequestPGWDTO.ToString());

                    TransactionInquiry transactionInquiry = RequeryPayment(ccOrigTransactionsPGWDTO);

                    if (ccOrigRequestPGWDTO != null)
                    {
                        originalPaymentDate = ccOrigRequestPGWDTO.RequestDatetime;
                        log.Debug("originalPaymentDate: " + originalPaymentDate);
                    }
                    else
                    {
                        log.Error("No cCRequest details found!");
                        throw new Exception("Refund Failed!");
                    }

                    //Create refund ccRequestPGW
                    CCRequestPGWDTO cCRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, CREDIT_CARD_REFUND);

                    DateTime bussStartTime = utilities.getServerTime().Date.AddHours(Convert.ToInt32(ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "BUSINESS_DAY_START_TIME")));
                    DateTime bussEndTime = bussStartTime.AddDays(1);
                    if (utilities.getServerTime() < bussStartTime)
                    {
                        bussStartTime = bussStartTime.AddDays(-1);
                        bussEndTime = bussStartTime.AddDays(1);
                    }

                    if ((originalPaymentDate >= bussStartTime) && (originalPaymentDate <= bussEndTime))
                    {
                        // same day: VOID
                        log.Debug("Same day: Void");
                        if (Convert.ToDouble(ccOrigTransactionsPGWDTO.Authorize) == transactionPaymentsDTO.Amount)
                        {
                            VoidTransaction(ccOrigTransactionsPGWDTO, cCRequestPGWDTO, transactionPaymentsDTO);
                        }
                        else
                        {
                            throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, 2868));
                        }
                    }
                    else
                    {
                        // Next Day: Refund
                        log.Debug("Next Day: Refund");
                        if (!isUnattended)
                        {
                            // Message = "Verify that refund is done through Bank terminal!...";
                            //return true;
                            if (showMessageDelegate != null)
                            {
                                showMessageDelegate("Verify that refund is done through Bank terminal!...", "Ipay Payment Gateway", MessageBoxButtons.OK);
                            }

                            CCTransactionsPGWDTO ccTransactionsPGWDTO = new CCTransactionsPGWDTO();
                            ccTransactionsPGWDTO.InvoiceNo = cCRequestPGWDTO.RequestID.ToString();
                            ccTransactionsPGWDTO.TranCode = PaymentGatewayTransactionType.REFUND.ToString();
                            ccTransactionsPGWDTO.CardType = ccOrigTransactionsPGWDTO.CardType;
                            ccTransactionsPGWDTO.ResponseOrigin = ccOrigTransactionsPGWDTO.ResponseID.ToString();
                            ccTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                            ccTransactionsPGWDTO.RecordNo = "A";
                            ccTransactionsPGWDTO.TextResponse = "Approved";
                            CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(ccTransactionsPGWDTO);
                            SendPrintReceiptRequest(transactionPaymentsDTO, ccTransactionsPGWDTO);
                            ccTransactionsPGWBL.Save();
                            transactionPaymentsDTO.CCResponseId = ccTransactionsPGWBL.CCTransactionsPGWDTO.ResponseID;

                        }
                        else
                        {
                            throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, "Refund can not be processed, please contact our staff"));
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Error occured while Refunding the Amount", ex);
                    throw new Exception(ex.Message);
                }
            }


            log.LogMethodExit(transactionPaymentsDTO);
            return transactionPaymentsDTO;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cCRequestPGWDTO"></param>
        /// <returns></returns>
        private TransactionInquiry RequeryPayment(CCTransactionsPGWDTO cCTransactionsPGWDTO)
        {
            log.LogMethodEntry(cCTransactionsPGWDTO);

            Transaction_Inquiry_CardDetails transaction_Inquiry_CardDetails = new Transaction_Inquiry_CardDetails();
            transaction_Inquiry_CardDetails.Url = requeryUrl;
            TransactionInquiry transactionInquiry = transaction_Inquiry_CardDetails.TxDetailsInquiryCardInfo(merchantCode, cCTransactionsPGWDTO.RefNo, cCTransactionsPGWDTO.Authorize, "");

            log.LogMethodExit(transactionInquiry);
            return transactionInquiry;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ccOrigTransactionsPGWDTO"></param>
        /// <param name="cCRequestPGWDTO"></param>
        /// <param name="transactionPaymentsDTO"></param>
        /// <returns></returns>
        private TransactionPaymentsDTO VoidTransaction(CCTransactionsPGWDTO ccOrigTransactionsPGWDTO, CCRequestPGWDTO cCRequestPGWDTO, TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(ccOrigTransactionsPGWDTO, cCRequestPGWDTO, transactionPaymentsDTO);

            
            try
            {
                Service1 voidTransactionService = new Service1();
                voidTransactionService.Url = voidUrl;
                string signature = GetVoidTransactionSignatureString(ccOrigTransactionsPGWDTO);
                signature = GetVoidTransactionSignature(signature);

                string status = voidTransactionService.VoidTransaction(merchantCode, ccOrigTransactionsPGWDTO.AuthCode, ccOrigTransactionsPGWDTO.Authorize, currencyCode, signature);
                if (status == "0")
                {
                    CCTransactionsPGWDTO ccTransactionsPGWDTO = new CCTransactionsPGWDTO();
                    ccTransactionsPGWDTO.InvoiceNo = cCRequestPGWDTO.RequestID.ToString();
                    ccTransactionsPGWDTO.TranCode = PaymentGatewayTransactionType.VOID.ToString();
                    ccTransactionsPGWDTO.CardType = ccOrigTransactionsPGWDTO.CardType;
                    ccTransactionsPGWDTO.ResponseOrigin = ccOrigTransactionsPGWDTO.ResponseID.ToString();
                    ccTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                    ccTransactionsPGWDTO.RecordNo = "A";
                    ccTransactionsPGWDTO.TextResponse = "Approved";
                    CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(ccTransactionsPGWDTO, utilities.ExecutionContext);
                    SendPrintReceiptRequest(transactionPaymentsDTO, ccTransactionsPGWDTO);
                    ccTransactionsPGWBL.Save();
                    transactionPaymentsDTO.CCResponseId = ccTransactionsPGWBL.CCTransactionsPGWDTO.ResponseID;
                }
                              
                else
                {
                    string errorMessage = string.Empty;
                    if (voidTransactionErrCode.ContainsKey(status))
                    {
                        errorMessage = voidTransactionErrCode[status];
                    }
                    else
                    {
                        errorMessage = "Unknown Error";
                    }
                    throw new Exception(errorMessage);
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occured while voiding Transaction", ex);
                log.LogMethodExit(null, "Throwing Payment Gateway Exception-" + ex.Message);
                throw new PaymentGatewayException(ex.Message);
            }


            log.LogMethodExit(transactionPaymentsDTO);
            return transactionPaymentsDTO;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rawData"></param>
        /// <returns></returns>
        private string GetPaymentSignature(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientRequestModel"></param>
        /// <returns></returns>
        private string GetPaymentSignatureString(ClientRequestModel clientRequestModel)
        {
            log.LogMethodEntry(clientRequestModel);
            string signatureString = string.Empty;
            signatureString += merchantKey + merchantCode+clientRequestModel.RefNo;
            string amount = Regex.Replace(clientRequestModel.Amount, @"[^0-9]+", "");
            signatureString += amount+currencyCode;
            if (!string.IsNullOrEmpty(clientRequestModel.xfield1))
            {
                signatureString += clientRequestModel.xfield1;
            }
            signatureString += clientRequestModel.BarcodeNo + clientRequestModel.TerminalID;
            log.LogMethodExit(signatureString);
            return signatureString;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="toEncrypt"></param>
        /// <returns></returns>
        private string GetVoidTransactionSignature(string toEncrypt)
        {
            SHA1CryptoServiceProvider objSHA1 = new SHA1CryptoServiceProvider();
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            objSHA1.ComputeHash(toEncryptArray);

            byte[] buffer = objSHA1.Hash;

            string HashValue = System.Convert.ToBase64String(buffer);

            return HashValue;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cCTransactionsPGWDTO"></param>
        /// <returns></returns>
        private string GetVoidTransactionSignatureString(CCTransactionsPGWDTO cCTransactionsPGWDTO)
        {
            log.LogMethodEntry(cCTransactionsPGWDTO);

            string signatureString = string.Empty;
            signatureString += merchantKey + merchantCode + cCTransactionsPGWDTO.AuthCode;
            string amount = Regex.Replace(cCTransactionsPGWDTO.Authorize, @"[^0-9]+", "");
            signatureString += amount + currencyCode;
            
            log.LogMethodExit(signatureString);
            return signatureString;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="transactionPaymentsDTO"></param>
        /// <param name="ccTransactionsPGWDTO"></param>
        private void SendPrintReceiptRequest(TransactionPaymentsDTO transactionPaymentsDTO, CCTransactionsPGWDTO ccTransactionsPGWDTO)
        {
            if (utilities.getParafaitDefaults("PRINT_CUSTOMER_RECEIPT") == "Y")
            {
                transactionPaymentsDTO.Memo = GetReceiptText(transactionPaymentsDTO, ccTransactionsPGWDTO, false);
            }
            if (utilities.getParafaitDefaults("PRINT_MERCHANT_RECEIPT") == "Y")
            {
                transactionPaymentsDTO.Memo += GetReceiptText(transactionPaymentsDTO, ccTransactionsPGWDTO, true);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="trxPaymentsDTO"></param>
        /// <param name="ccTransactionsPGWDTO"></param>
        /// <param name="IsMerchantCopy"></param>
        /// <returns></returns>
        private string GetReceiptText(TransactionPaymentsDTO trxPaymentsDTO, CCTransactionsPGWDTO ccTransactionsPGWDTO, bool IsMerchantCopy)
        {
            log.LogMethodEntry(trxPaymentsDTO, ccTransactionsPGWDTO, IsMerchantCopy);
            try
            {
                string[] addressArray = utilities.ParafaitEnv.SiteAddress.Split(',');
                string receiptText = "";
                receiptText += AllignText(utilities.ParafaitEnv.SiteName, RecieptPrintAlignment.Center);
                if (addressArray != null && addressArray.Length > 0)
                {
                    for (int i = 0; i < addressArray.Length; i++)
                    {
                        receiptText += Environment.NewLine + AllignText(addressArray[i] + ((i != addressArray.Length - 1) ? "," : ""), RecieptPrintAlignment.Center);
                    }
                }
                receiptText += Environment.NewLine;
                receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("Order No") + "     : ".PadLeft(12) + "@invoiceNo", RecieptPrintAlignment.Left);
                receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("Transaction ID") + "     : ".PadLeft(12) + ccTransactionsPGWDTO.RefNo, RecieptPrintAlignment.Left);
                receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("Transaction Date") + ": ".PadLeft(4) + ccTransactionsPGWDTO.TransactionDatetime.ToString("MMM dd yyyy HH:mm"), RecieptPrintAlignment.Left);
                receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("Transaction Amount") + ": ".PadLeft(4) +currencyCode+ " " +ccTransactionsPGWDTO.Authorize, RecieptPrintAlignment.Left);
                receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("Payment Type") + ": ".PadLeft(4) + ccTransactionsPGWDTO.CardType, RecieptPrintAlignment.Left);
                
                //if (!string.IsNullOrEmpty(ccTransactionsPGWDTO.AuthCode))
                //    receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("Authorization") + "   : ".PadLeft(10) + ccTransactionsPGWDTO.AuthCode, Alignment.Left);
                //if (!string.IsNullOrEmpty(ccTransactionsPGWDTO.CardType))
                //    receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("Card Type") + "       : ".PadLeft(15) + ccTransactionsPGWDTO.CardType, Alignment.Left);
              
                
                receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage((ccTransactionsPGWDTO.RecordNo.Equals("A")) ? "APPROVED" : (ccTransactionsPGWDTO.RecordNo.Equals("B")) ? "RETRY" : "DECLINED") + "-" + ccTransactionsPGWDTO.DSIXReturnCode, RecieptPrintAlignment.Center);
                //if (ccTransactionsPGWDTO.RecordNo.Equals("C"))
                //{
                //    receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage(ccTransactionsPGWDTO.TextResponse), Alignment.Center);
                //}
                receiptText += Environment.NewLine;
                    
                //receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("Total") + "                           : " + ((Convert.ToDouble(ccTransactionsPGWDTO.Authorize) == 0) ? (trxPaymentsDTO.Amount + trxPaymentsDTO.TipAmount) : Convert.ToDouble(ccTransactionsPGWDTO.Authorize)).ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL), Alignment.Left);
                //receiptText += Environment.NewLine;
                
                
                receiptText += Environment.NewLine;
                if (IsMerchantCopy)
                {
                    if ((!string.IsNullOrEmpty(ccTransactionsPGWDTO.RecordNo) && ccTransactionsPGWDTO.RecordNo.Equals("A")))
                    {
                        receiptText += Environment.NewLine;
                        receiptText += Environment.NewLine;
                        receiptText += Environment.NewLine + AllignText("_______________________", RecieptPrintAlignment.Center);
                        receiptText += Environment.NewLine;
                        receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("Signature"), RecieptPrintAlignment.Center);
                        receiptText += Environment.NewLine;
                        receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage(1180), RecieptPrintAlignment.Center);
                    }
                    
                    receiptText += Environment.NewLine;
                    receiptText += Environment.NewLine + AllignText("**" + utilities.MessageUtils.getMessage("Merchant Copy") + "**", RecieptPrintAlignment.Center);
                }
                else
                {
                    receiptText += Environment.NewLine;
                    receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("IMPORTANT— retain this copy for your records"), RecieptPrintAlignment.Center);
                    receiptText += Environment.NewLine;
                    receiptText += Environment.NewLine + AllignText("**" + utilities.MessageUtils.getMessage("Cardholder Copy") + " **", RecieptPrintAlignment.Center);
                }

                receiptText += Environment.NewLine;
                receiptText += AllignText(" " + utilities.MessageUtils.getMessage("Thank You"), RecieptPrintAlignment.Center);
                
               if (!string.IsNullOrEmpty(ccTransactionsPGWDTO.RefNo) && ccTransactionsPGWDTO.RecordNo.Equals("A"))
                {
                    if (IsMerchantCopy)
                    {
                        ccTransactionsPGWDTO.MerchantCopy = receiptText;
                    }
                    else
                    {
                        ccTransactionsPGWDTO.CustomerCopy = receiptText;
                    }
                }
                else
                {
                    Print(receiptText);
                }

                log.LogMethodExit(receiptText);
                return receiptText;
            }
            catch (Exception ex)
            {
                log.Fatal("GetReceiptText() failed to print receipt exception:" + ex.ToString());
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="align"></param>
        /// <returns></returns>
        public static string AllignText(string text, RecieptPrintAlignment align)
        {
            log.LogMethodEntry(text, align);

            int pageWidth = 40;
            string res;
            if (align.Equals(RecieptPrintAlignment.Right))
            {
                string returnValueNew = text.PadLeft(pageWidth, ' ');
                log.LogMethodExit(returnValueNew);
                return returnValueNew;
            }
            else if (align.Equals(RecieptPrintAlignment.Center))
            {
                int len = (pageWidth - text.Length);
                int len2 = len / 2;
                len2 = len2 + text.Length;
                res = text.PadLeft(len2);
                if (res.Length > pageWidth && res.Length > text.Length)
                {
                    res = res.Substring(res.Length - pageWidth);
                }

                log.LogMethodExit(res);
                return res;
            }
            else
            {
                //res= text.PadLeft(5 + text.Length);  
                log.LogMethodExit(text);
                return text;
            }
        }

    }
}
