/********************************************************************************************
 * Project Name - Hosted Payments                                                                     
 * Description  - Class to manager hosted payment gateways
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.150.2      08-Jan-2023   Nitin Pai            Modified to handle Angular payment
 *2.150.4      15-Jun-2023   Muaaz Musthafa       Updated the PaymentProcessStatus for PRE-AUTH payments  
 ********************************************************************************************/
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Semnox.Core.Utilities;
using Stripe;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Device.PaymentGateway
{

    public enum PaymentStatusType
    {
        /// <summary>
        /// Transaction None
        /// </summary>
        NONE,
        /// <summary>
        /// Transaction Success
        /// </summary>
        SUCCESS,
        /// <summary>
        /// Transaction Failed
        /// </summary>
        FAILED,
        /// <summary>
        /// Transaction Cancelled
        /// </summary>
        CANCELLED,
        /// <summary>
        /// Transaction Error
        /// </summary>
        ERROR,
        /// <summary>
        /// Transaction PreAuth
        /// </summary>
        PRE_AUTH
    }

    public enum PaymentProcessStatusType
    {
        /// <summary>
        /// 
        /// </summary>
        PAYMENT_INITIATED,
        /// <summary>
        /// 
        /// </summary>
        PAYMENT_PROCESSING,
        /// <summary>
        /// 
        /// </summary>
        THREE_DS_INITIATED,
        /// <summary>
        /// 
        /// </summary>
        THREE_DS_PROCESSED,
        /// <summary>
        /// 
        /// </summary>
        PAYMENT_COMPLETED,
        /// <summary>
        /// 
        /// </summary>
        PAYMENT_FAILED,
        /// <summary>
        /// 
        /// </summary>
        PAYMENT_CANCELLED
    }

    public class PaymentProcessStatus
    {
        public List<String> paymentInitiated = new List<string>();
        public List<String> threeDSInitiated = new List<string>();
        public List<String> paymentProcessing = new List<string>();
        public List<String> threeDSProcessed = new List<string>();

        public PaymentProcessStatus()
        {
            this.paymentInitiated.Add(PaymentProcessStatusType.THREE_DS_INITIATED.ToString());
            this.paymentInitiated.Add(PaymentProcessStatusType.PAYMENT_PROCESSING.ToString());
            this.threeDSInitiated.Add(PaymentProcessStatusType.THREE_DS_PROCESSED.ToString());
            this.paymentProcessing.Add(PaymentProcessStatusType.PAYMENT_COMPLETED.ToString());
            this.paymentProcessing.Add(PaymentProcessStatusType.PAYMENT_FAILED.ToString());
            this.threeDSProcessed.Add(PaymentProcessStatusType.PAYMENT_COMPLETED.ToString());
            this.threeDSProcessed.Add(PaymentProcessStatusType.PAYMENT_FAILED.ToString());
        }
    }

    public class HostedGatewayDTO
    {

        /// <summary>
        /// Payment gateways supported in parafait. 
        /// </summary>

        public TransactionPaymentsDTO TransactionPaymentsDTO
        {
            get; set;
        }

        public CCTransactionsPGWDTO CCTransactionsPGWDTO
        {
            get; set;
        }

        public PaymentStatusType PaymentStatus
        {
            get; set;
        }

        public string PaymentStatusMessage
        {
            get; set;
        }


        public string MerchantId
        {
            get; set;
        }

        public string ClientId
        {
            get; set;
        }

        public string APIKey
        {
            get; set;
        }

        public string RequestURL
        {
            get; set;
        }

        public string SuccessURL
        {
            get; set;
        }

        public string CallBackURL
        {
            get; set;
        }

        public string FailureURL
        {
            get; set;
        }

        public string CancelURL
        {
            get; set;
        }

        public string PublicKey
        {
            get; set;
        }

        public string PrivateKey
        {
            get; set;
        }

        public string GatewayRequestString
        {
            get; set;
        }

        public string GatewayRequestStringContentType
        {
            get; set;
        }

        public string GatewayResponseString
        {
            get; set;
        }
        public string GatewayRequestFormString
        {
            get; set;
        }

        public PaymentGateways GatewayLookUp
        {
            get; set;
        }
        public PaymentProcessStatusType PaymentProcessStatus
        {
            get; set;
        }

        public string PGSuccessResponseMessage
        {
            get; set;
        }

        public string PGFailedResponseMessage
        {
            get; set;
        }

        public int SiteId
        {
            get; set;
        }

        public int TrxId
        {
            get; set;
        }

        public string TrxGuid
        {
            get; set;
        }

        public string GatewayReferenceNumber
        {
            get; set;
        }

        public Boolean IsHostedGateway
        {
            get; set;
        }

        public HostedGatewayDTO()
        {
            // Initiating the payment status to None
            this.PaymentStatus = PaymentStatusType.NONE;
            this.TrxId = -1;
            this.IsHostedGateway = true;
        }
    }
    public class HostedPaymentGateway : PaymentGateway
    {
        public enum RequestContentType
        {
            /// <summary>
            /// FORM
            /// </summary>
            FORM,
            /// <summary>
            /// JSON
            /// </summary>
            JSON,
        }

        public static string RequestContentTypeToString(RequestContentType RequestContentType)
        {
            if (RequestContentType == RequestContentType.FORM)
                return "FORM";
            else
                return "JSON";
        }

        public static RequestContentType RequestContentTypeFromString(String contentType)
        {
            if (contentType.ToUpper().Equals("FORM"))
                return RequestContentType.FORM;
            else
                return RequestContentType.JSON;
        }


        public int TransactionSiteId
        {
            get
            {
                return transactionSiteId;
            }

            set
            {
                transactionSiteId = value;
            }
        }

        private int transactionSiteId;

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Constructor of payment gateway class.
        /// </summary>
        /// <param name="utilities">Parafait utilities.</param>
        /// <param name="isUnattended">Whether the payment process is supervised by an attendant.</param>
        /// <param name="showMessageDelegate"> Delegate instance to display message.</param>
        /// <param name="writeToLogDelegate">Delegate instance for writing the Log to File</param>
        public HostedPaymentGateway(Utilities utilities, bool isUnattended, ShowMessageDelegate showMessageDelegate, WriteToLogDelegate writeToLogDelegate)
             : base(utilities, isUnattended, showMessageDelegate, writeToLogDelegate)
        {
            log.LogMethodEntry(utilities, isUnattended, showMessageDelegate, writeToLogDelegate);
            this.isHostedPayment = true;
            this.transactionSiteId = -1;
            log.LogMethodExit(null);

        }

        public virtual HostedGatewayDTO CreateGatewayPaymentRequest(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry();
            HostedGatewayDTO HostedGatewayConfigurationDTO = new HostedGatewayDTO();
            log.LogMethodExit();
            return HostedGatewayConfigurationDTO;
        }


        public virtual HostedGatewayDTO ProcessGatewayResponse(string gatewayResponse)
        {
            log.LogMethodEntry();
            HostedGatewayDTO hostedGatewayDTO = new HostedGatewayDTO();
            log.LogMethodExit();
            return hostedGatewayDTO;
        }

        public virtual HostedGatewayDTO InitiatePaymentProcessing(string gatewayResponse)
        {
            log.LogMethodEntry(gatewayResponse);
            HostedGatewayDTO hostedGatewayDTO = null;
            log.LogMethodExit(hostedGatewayDTO);
            return hostedGatewayDTO;
        }

        /// <summary>
        ///  GetTransactionSiteIdFromResponse - generic method to retrive the TransactionSiteId from the gateway response order Id
        /// </summary>
        /// <param name="respGateway"></param>
        /// <param name="paymentGateway"></param>
        /// <param name="contextSiteId"></param>
        /// <returns></returns>
        public static int GetTransactionSiteIdFromResponse(string respGateway, string paymentGateway, int contextSiteId)
        {
            log.LogMethodEntry(respGateway, paymentGateway, contextSiteId);
            if (contextSiteId == -1 || paymentGateway.ToLower() == PaymentGateways.CCAvenueHostedPayment.ToString().ToLower())
            {
                log.LogMethodExit(contextSiteId);
                return contextSiteId;
            }
            else
            {
                int trxId = -1;

                int trxSiteId = -1;

                dynamic response = Newtonsoft.Json.JsonConvert.DeserializeObject(respGateway);
                log.LogVariableState("response", response);
                PaymentGateways paymentGateways = (PaymentGateways)Enum.Parse(typeof(PaymentGateways), paymentGateway, true);
                log.LogVariableState("paymentGateways", paymentGateways.ToString());
                switch (paymentGateways)
                {
                    case PaymentGateways.AdyenHostedPayment:
                        {
                            trxId = response["merchantReference"] != null ? Convert.ToInt32(response["merchantReference"]) : -1;
                            if (trxId == -1)
                            {
                                JObject paymentResp = JObject.Parse(respGateway);
                                string merchantReference = paymentResp["notificationItems"]?
                                                .Select(item => item["NotificationRequestItem"]["merchantReference"]?.ToString())
                                                .FirstOrDefault();
                                trxId = int.TryParse(merchantReference, out trxId) ? trxId : -1;
                            }
                            break;
                        }
                    case PaymentGateways.BamboraHostedPayment:
                        {
                            trxId = response["trnOrderNumber"] != null ? Convert.ToInt32(response["trnOrderNumber"]) : -1;
                            break;
                        }
                    case PaymentGateways.CardConnectHostedPayment:
                        {
                            try
                            {
                                string trxIdString = string.Empty;
                                if (response["encOrderNo"] != null) // For encrypted GatewayResponse
                                {
                                    trxIdString = response["encOrderNo"];
                                }
                                else
                                {
                                    string userfeild = (response["userfields"]);
                                    dynamic postdata = JsonConvert.DeserializeObject(userfeild);

                                    trxIdString = postdata.transactionId;
                                }

                                if (string.IsNullOrEmpty(trxIdString))
                                {
                                    throw new Exception("No orderNo found!");
                                }
                                trxIdString = Encryption.Decrypt(Encoding.UTF8.GetString(Convert.FromBase64String(trxIdString)));
                                if (!String.IsNullOrWhiteSpace(trxIdString))
                                {
                                    trxId = Convert.ToInt32(trxIdString.Substring(0, trxIdString.IndexOf(":")));
                                }

                            }
                            catch (Exception ex)
                            {
                                log.Error("Error while getting trxId" + ex);
                            }

                            break;
                        }
                    case PaymentGateways.CorvusPayHostedPayment:
                        {
                            trxId = response["order_number"] != null ? Convert.ToInt32(response["order_number"]) : -1;
                            break;
                        }

                    case PaymentGateways.FirstDataPayeezyHostedPayment:
                        {
                            trxId = response["Reference_No"] != null ? Convert.ToInt32(response["Reference_No"]) : -1;
                            break;
                        }
                    case PaymentGateways.WorldPayHostedPayment:
                        {

                            if (response["MD"] != null)
                            {
                                string[] result = new string[] { };
                                result = response["MD"].ToString().Split('|');
                                trxId = Convert.ToInt32(result[1]);
                            }
                            else
                            {
                                //incase of non-3DS
                                trxId = response["orderReference"] != null ? Convert.ToInt32(response["orderReference"]) : -1;
                            }

                            break;
                        }
                    case PaymentGateways.EcommpayHostedPayment:
                        {
                            string responseData;
                            if (response["payment_id"] != null)
                            {
                                trxId = (response["payment_id"]);
                            }
                            else
                            {
                                //Incase of call back 
                                responseData = response["payment"];
                                dynamic postdata = JsonConvert.DeserializeObject(responseData);
                                trxId = postdata.id;
                            }
                            break;
                        }
                    case PaymentGateways.ipay88HostedPayment:
                        {
                            trxId = response["RefNo"] != null ? Convert.ToInt32(response["RefNo"]) : -1;
                            break;
                        }
                    case PaymentGateways.StripeHostedPayment:
                        {
                            ExecutionContext executionContext = ExecutionContext.GetExecutionContext();
                            StripeConfiguration.ApiKey = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "HOSTED_PAYMENT_GATEWAY_SECRET_KEY");
                            log.Debug("stripe secret key: " + StripeConfiguration.ApiKey);
                            string sessionId = string.Empty;
                            if (response["id"] != null)
                            {
                                log.Debug("callback:");
                                dynamic data = response["data"]["object"];
                                sessionId = data["id"];
                            }
                            else
                            {
                                log.Debug("callsuccess:");
                                sessionId = response["session_id"].ToString();
                            }
                            log.Debug("sessionid: " + sessionId);
                            var sessionService = new SessionService();
                            Session session = sessionService.Get(sessionId);
                            dynamic resData = Newtonsoft.Json.JsonConvert.DeserializeObject(session.StripeResponse.Content);
                            trxId = resData["metadata"]["trxId"] != null ? Convert.ToInt32(resData["metadata"]["trxId"]) : -1;
                            break;
                        }
                    case PaymentGateways.CreditCallHostedPayment:
                        {
                            trxId = response["ekashu_reference"] != null ? Convert.ToInt32(response["ekashu_reference"]) : -1;
                            break;
                        }
                    case PaymentGateways.MidtransHostedPayment:
                        {
                            trxId = response["order_id"] != null ? Convert.ToInt32(response["order_id"]) : -1;
                            break;
                        }
                    case PaymentGateways.CyberSourceHostedPayment:
                        {
                            trxId = response["req_reference_number"] != null ? Convert.ToInt32(response["req_reference_number"]) : -1;
                            break;
                        }
                    case PaymentGateways.WPCyberSourceHostedPayment:
                        {
                            trxId = response["req_reference_number"] != null ? Convert.ToInt32(response["req_reference_number"]) : -1;
                            break;
                        }
                    case PaymentGateways.CommonWebHostedPayment:
                        {
                            trxId = response["OrderId"] != null ? Convert.ToInt32(response["OrderId"]) : -1;
                            break;
                        }
                    case PaymentGateways.ThawaniHostedPayment:
                        {
                            trxId = response["client_reference_id"] != null ? Convert.ToInt32(response["client_reference_id"]) : -1;
                            break;
                        }
                    case PaymentGateways.VisaNetsHostedPayment:
                        {
                            trxId = response["OrderId"] != null ? Convert.ToInt32(response["OrderId"]) : -1;
                            break;
                        }
                    case PaymentGateways.PaytmHostedPayment:
                        {
                            trxId = response["ORDERID"] != null ? Convert.ToInt32(response["ORDERID"]) : -1;
                            break;
                        }
                    case PaymentGateways.PayFortHostedPayment:
                        {
                            trxId = response["merchant_reference"] != null ? Convert.ToInt32(response["merchant_reference"]) : -1;
                            break;
                        }
                    case PaymentGateways.MonerisHostedPayment:
                        {
                            trxId = response["order_no"] != null ? Convert.ToInt32(response["order_no"]) : -1;
                            break;
                        }
                }


                if (trxId != -1)
                {
                    CCRequestPGWListBL cCRequestPGWListBL = new CCRequestPGWListBL();
                    List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>> searchParametersPGW = new List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>>();
                    searchParametersPGW.Add(new KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>(CCRequestPGWDTO.SearchByParameters.INVOICE_NUMBER, trxId.ToString()));
                    CCRequestPGWDTO cCRequestsPGWDTO = cCRequestPGWListBL.GetLastestCCRequestPGWDTO(searchParametersPGW);
                    log.LogMethodExit(cCRequestsPGWDTO.SiteId);
                    trxSiteId = cCRequestsPGWDTO.SiteId;
                }

                if (trxSiteId == -1)
                {
                    throw new Exception("Order site Id Could Not be fetched - " + trxId);
                }

                log.LogMethodExit("Order site Id fetched for " + trxId + " - " + trxSiteId.ToString());
                return trxSiteId;
            }
        }

        public virtual bool UpdatePaymentProcessingStatus(HostedGatewayDTO hostedGatewayDTO)
        {
            log.LogMethodEntry(hostedGatewayDTO);

            PaymentProcessStatus paymentProcessStatus = new PaymentProcessStatus();
            CCRequestPGWListBL cCRequestPGWListBL = new CCRequestPGWListBL();
            List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>> searchParametersPGW = new List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>>();
            int trxId = hostedGatewayDTO.TrxId > -1 ? hostedGatewayDTO.TrxId : hostedGatewayDTO.TransactionPaymentsDTO.TransactionId;
            searchParametersPGW.Add(new KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>(CCRequestPGWDTO.SearchByParameters.INVOICE_NUMBER, trxId.ToString()));
            CCRequestPGWDTO cCRequestsPGWDTO = cCRequestPGWListBL.GetLastestCCRequestPGWDTO(searchParametersPGW);
            string curStatus = cCRequestsPGWDTO.PaymentProcessStatus;

            if (curStatus.Equals(PaymentProcessStatusType.PAYMENT_INITIATED.ToString()))
            {
                if (paymentProcessStatus.paymentInitiated.Contains(hostedGatewayDTO.PaymentProcessStatus.ToString()))
                {
                    CCRequestPGWBL cCRequestPGWBL = new CCRequestPGWBL(utilities.ExecutionContext, cCRequestsPGWDTO);
                    int rowsUpdated = cCRequestPGWBL.ChangePaymentProcessingStatus(curStatus, hostedGatewayDTO.PaymentProcessStatus.ToString());
                    if (rowsUpdated == 1)
                    {
                        log.Debug("Status updated successfully");
                        return true;
                    }
                    else
                    {
                        log.Debug("Status updated not successful");
                        return false;
                    }
                }
                else
                {
                    return false;
                }

            }
            else if (curStatus.Equals(PaymentProcessStatusType.THREE_DS_INITIATED.ToString()))
            {
                if (paymentProcessStatus.threeDSInitiated.Contains(hostedGatewayDTO.PaymentProcessStatus.ToString()))
                {
                    CCRequestPGWBL cCRequestPGWBL = new CCRequestPGWBL(utilities.ExecutionContext, cCRequestsPGWDTO);
                    int rowsUpdated = cCRequestPGWBL.ChangePaymentProcessingStatus(curStatus, hostedGatewayDTO.PaymentProcessStatus.ToString());
                    if (rowsUpdated == 1)
                    {
                        log.Debug("Status updated successfully");
                        return true;
                    }
                    else
                    {
                        log.Debug("Status updated not successful");
                        return false;
                    }
                }
                else
                {
                    return false;
                }

            }
            else if (curStatus.Equals(PaymentProcessStatusType.THREE_DS_PROCESSED.ToString()))
            {
                if (paymentProcessStatus.threeDSProcessed.Contains(hostedGatewayDTO.PaymentProcessStatus.ToString()))
                {
                    CCRequestPGWBL cCRequestPGWBL = new CCRequestPGWBL(utilities.ExecutionContext, cCRequestsPGWDTO);
                    int rowsUpdated = cCRequestPGWBL.ChangePaymentProcessingStatus(curStatus, hostedGatewayDTO.PaymentProcessStatus.ToString());
                    if (rowsUpdated == 1)
                    {
                        log.Debug("Status updated successfully");
                        return true;
                    }
                    else
                    {
                        log.Debug("Status updated not successful");
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (hostedGatewayDTO.PaymentProcessStatus.ToString().Equals(PaymentProcessStatusType.PAYMENT_COMPLETED.ToString()) || hostedGatewayDTO.PaymentProcessStatus.ToString().Equals(PaymentProcessStatusType.PAYMENT_FAILED.ToString()))
                {
                    CCRequestPGWBL cCRequestPGWBL = new CCRequestPGWBL(utilities.ExecutionContext, cCRequestsPGWDTO);
                    int rowsUpdated = cCRequestPGWBL.ChangePaymentProcessingStatus(curStatus, hostedGatewayDTO.PaymentProcessStatus.ToString());
                    if (rowsUpdated == 1)
                    {
                        log.Debug("Status updated successfully");
                        return true;
                    }
                    else
                    {
                        log.Debug("Status updated not successful");
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

        }
        public static int GetTransactionIdFromResponse(string respGateway, string paymentGateway)
        {
            log.LogMethodEntry(respGateway, paymentGateway);

            int trxId = -1;

            int trxSiteId = -1;

            dynamic response = Newtonsoft.Json.JsonConvert.DeserializeObject(respGateway);
            log.LogVariableState("response", response);
            PaymentGateways paymentGateways = (PaymentGateways)Enum.Parse(typeof(PaymentGateways), paymentGateway, true);
            log.LogVariableState("paymentGateways", paymentGateways.ToString());
            switch (paymentGateways)
            {
                case PaymentGateways.AdyenHostedPayment:
                    {
                        trxId = response["merchantReference"] != null ? Convert.ToInt32(response["merchantReference"]) : -1;
                        if (trxId == -1)
                        {
                            JObject paymentResp = JObject.Parse(respGateway);
                            string merchantReference = paymentResp["notificationItems"]?
                                            .Select(item => item["NotificationRequestItem"]["merchantReference"]?.ToString())
                                            .FirstOrDefault();
                            trxId = int.TryParse(merchantReference, out trxId) ? trxId : -1;
                        }
                        break;
                    }
                case PaymentGateways.BamboraHostedPayment:
                    {
                        trxId = response["trnOrderNumber"] != null ? Convert.ToInt32(response["trnOrderNumber"]) : -1;
                        break;
                    }
                case PaymentGateways.CardConnectHostedPayment:
                    {
                        try
                        {
                            string trxIdString = string.Empty;
                            if (response["encOrderNo"] != null) // For encrypted GatewayResponse
                            {
                                trxIdString = response["encOrderNo"];
                            }
                            else
                            {
                                string userfeild = (response["userfields"]);
                                dynamic postdata = JsonConvert.DeserializeObject(userfeild);

                                trxIdString = postdata.transactionId;
                            }

                            if (string.IsNullOrEmpty(trxIdString))
                            {
                                throw new Exception("No orderNo found!");
                            }
                            trxIdString = Encryption.Decrypt(Encoding.UTF8.GetString(Convert.FromBase64String(trxIdString)));
                            if (!String.IsNullOrWhiteSpace(trxIdString))
                            {
                                trxId = Convert.ToInt32(trxIdString.Substring(0, trxIdString.IndexOf(":")));
                            }

                        }
                        catch (Exception ex)
                        {
                            log.Error("Error while getting trxId" + ex);
                        }
                        break;
                    }
                case PaymentGateways.CorvusPayHostedPayment:
                    {
                        trxId = response["order_number"] != null ? Convert.ToInt32(response["order_number"]) : -1;
                        break;
                    }

                case PaymentGateways.FirstDataPayeezyHostedPayment:
                    {
                        trxId = response["Reference_No"] != null ? Convert.ToInt32(response["Reference_No"]) : -1;
                        break;
                    }
                case PaymentGateways.WorldPayHostedPayment:
                    {

                        if (response["MD"] != null)
                        {
                            string[] result = new string[] { };
                            result = response["MD"].ToString().Split('|');
                            trxId = Convert.ToInt32(result[1]);
                        }
                        else
                        {
                            //incase of non-3DS
                            trxId = response["orderReference"] != null ? Convert.ToInt32(response["orderReference"]) : -1;
                        }

                        break;
                    }
                case PaymentGateways.EcommpayHostedPayment:
                    {
                        string responseData;
                        if (response["payment_id"] != null)
                        {
                            trxId = (response["payment_id"]);
                        }
                        else
                        {
                            //Incase of call back 
                            responseData = response["payment"];
                            dynamic postdata = JsonConvert.DeserializeObject(responseData);
                            trxId = postdata.id;
                        }
                        break;
                    }
                case PaymentGateways.ipay88HostedPayment:
                    {
                        trxId = response["RefNo"] != null ? Convert.ToInt32(response["RefNo"]) : -1;
                        break;
                    }
                case PaymentGateways.StripeHostedPayment:
                    {
                        ExecutionContext executionContext = ExecutionContext.GetExecutionContext();
                        StripeConfiguration.ApiKey = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "HOSTED_PAYMENT_GATEWAY_SECRET_KEY");
                        log.Debug("stripe secret key: " + StripeConfiguration.ApiKey);
                        string sessionId = string.Empty;
                        if (response["id"] != null)
                        {
                            log.Debug("callback:");
                            dynamic data = response["data"]["object"];
                            sessionId = data["id"];
                        }
                        else
                        {
                            log.Debug("callsuccess:");
                            sessionId = response["session_id"].ToString();
                        }
                        log.Debug("sessionid: " + sessionId);
                        var sessionService = new SessionService();
                        Session session = sessionService.Get(sessionId);
                        dynamic resData = Newtonsoft.Json.JsonConvert.DeserializeObject(session.StripeResponse.Content);
                        trxId = resData["metadata"]["trxId"] != null ? Convert.ToInt32(resData["metadata"]["trxId"]) : -1;
                        break;
                    }
                case PaymentGateways.CreditCallHostedPayment:
                    {
                        trxId = response["ekashu_reference"] != null ? Convert.ToInt32(response["ekashu_reference"]) : -1;
                        break;
                    }
                case PaymentGateways.CyberSourceHostedPayment:
                    {
                        trxId = response["req_reference_number"] != null ? Convert.ToInt32(response["req_reference_number"]) : -1;
                        break;
                    }
                case PaymentGateways.WPCyberSourceHostedPayment:
                    {
                        trxId = response["req_reference_number"] != null ? Convert.ToInt32(response["req_reference_number"]) : -1;
                        break;
                    }
                case PaymentGateways.CommonWebHostedPayment:
                    {
                        trxId = response["OrderId"] != null ? Convert.ToInt32(response["OrderId"]) : -1;
                        break;
                    }
                case PaymentGateways.ThawaniHostedPayment:
                    {
                        trxId = response["client_reference_id"] != null ? Convert.ToInt32(response["client_reference_id"]) : -1;
                        break;
                    }
                case PaymentGateways.VisaNetsHostedPayment:
                    {
                        trxId = response["OrderId"] != null ? Convert.ToInt32(response["OrderId"]) : -1;
                        break;
                    }
                case PaymentGateways.PaytmHostedPayment:
                    {
                        trxId = response["ORDERID"] != null ? Convert.ToInt32(response["ORDERID"]) : -1;
                        break;
                    }
                case PaymentGateways.PayFortHostedPayment:
                    {
                        trxId = response["merchant_reference"] != null ? Convert.ToInt32(response["merchant_reference"]) : -1;
                        break;
                    }
                case PaymentGateways.MonerisHostedPayment:
                    {
                        trxId = response["order_no"] != null ? Convert.ToInt32(response["order_no"]) : -1;
                        break;
                    }
            }

            log.LogMethodExit("Order site Id fetched for " + trxId + " - " + trxSiteId.ToString());
            return trxId;

        }
        public virtual string GetTransactionStatus(string trxId)
        {
            log.LogMethodEntry();
            Dictionary<string, Object> dict = new Dictionary<string, Object>();
            string getTransactionStatus = string.Empty;
            dict.Add("status", "-1");
            dict.Add("message", "No Implementation Found");
            getTransactionStatus = JsonConvert.SerializeObject(dict, Newtonsoft.Json.Formatting.Indented);
            log.LogMethodExit();
            return getTransactionStatus;
        }

        public virtual CCTransactionsPGWDTO GetPaymentResponseValue(string gatewayResponse)
        {
            log.LogMethodEntry();
            CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
            log.LogMethodExit();
            return cCTransactionsPGWDTO;
        }
    }
}
