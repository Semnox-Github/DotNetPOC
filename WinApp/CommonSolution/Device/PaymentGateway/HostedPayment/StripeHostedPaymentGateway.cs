/********************************************************************************************
 * Project Name -  Stripe Hosted Payment Gateway                                                                     
 * Description  - Class to handle the payment of Stripe Payment Gateway
 *
 **************
 **Version Log
  *Version     Date          Modified By                     Remarks          
 *********************************************************************************************
 *2.110        25-Feb-2021    Jinto                        Created for Website 
 *2.130.2.1    29-Feb-2022    Muaaz Musthafa               Fix on RefundAmount() method and in CCTrxPGW, Authorize amount
 ********************************************************************************************/
using Newtonsoft.Json;
using Semnox.Core.Utilities;
using Stripe;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Semnox.Parafait.Device.PaymentGateway
{
    class StripeHostedPaymentGateway : HostedPaymentGateway
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        string secretKey;
        string publishableKey;
        string successUrl;
        string cancelUrl;
        string currencyCode;
        string sessionid;
        private string post_url;
        private HostedGatewayDTO hostedGatewayDTO;

        public StripeHostedPaymentGateway(Utilities utilities, bool isUnattended, ShowMessageDelegate showMessageDelegate, WriteToLogDelegate writeToLogDelegate)
            : base(utilities, isUnattended, showMessageDelegate, writeToLogDelegate)
        {
            log.LogMethodEntry(utilities, isUnattended, showMessageDelegate, writeToLogDelegate);

            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
            System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(delegate { return true; });//certificate validation procedure for the SSL/TLS secure channel
            this.hostedGatewayDTO = new HostedGatewayDTO();
            this.Initialize();
            log.LogMethodExit(null);
        }

        public override void Initialize()
        {
            log.LogMethodEntry();
            secretKey = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "HOSTED_PAYMENT_GATEWAY_SECRET_KEY");
            publishableKey = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "HOSTED_PAYMENT_GATEWAY_PUBLISHABLE_KEY");
            currencyCode = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "CURRENCY_CODE");
            post_url = "/account/Stripe";

            string errMsgFormat = "Please enter {0} value in configuration. Site : " + utilities.ParafaitEnv.SiteId;
            string errMsg = "";

            if (string.IsNullOrWhiteSpace(secretKey))
            {
                errMsg = String.Format(errMsgFormat, "HOSTED_PAYMENT_GATEWAY_SECRET_KEY");
            }
            else if (string.IsNullOrWhiteSpace(publishableKey))
            {
                errMsg = String.Format(errMsgFormat, "HOSTED_PAYMENT_GATEWAY_PUBLISHABLE_KEY");
            }

            if (!string.IsNullOrWhiteSpace(errMsg))
            {
                log.Error(errMsg);
                throw new Exception(utilities.MessageUtils.getMessage(errMsg));
            }

            LookupValuesList lookupValuesList = new LookupValuesList(utilities.ExecutionContext);
            List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
            searchParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "WEB_SITE_CONFIGURATION"));
            searchParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));

            List<LookupValuesDTO> lookupValuesDTOlist = lookupValuesList.GetAllLookupValues(searchParameters);

            if (lookupValuesDTOlist.Where(x => x.LookupValue == "SUCCESS_URL").Count() == 1)
            {
                this.hostedGatewayDTO.SuccessURL = lookupValuesDTOlist.Where(x => x.LookupValue == "SUCCESS_URL").First().Description.ToLower().Replace("@gateway", PaymentGateways.StripeHostedPayment.ToString());
                this.successUrl = this.hostedGatewayDTO.SuccessURL;
                Uri NewUri;
                if (Uri.TryCreate(this.hostedGatewayDTO.SuccessURL, UriKind.Absolute, out NewUri))
                {
                    this.post_url = NewUri.GetLeftPart(UriPartial.Authority) + post_url;
                }
            }


            if (lookupValuesDTOlist.Where(x => x.LookupValue == "FAILED_URL").Count() == 1)
            {
                this.hostedGatewayDTO.FailureURL = lookupValuesDTOlist.Where(x => x.LookupValue == "FAILED_URL").First().Description.ToLower().Replace("@gateway", PaymentGateways.StripeHostedPayment.ToString());
                this.cancelUrl = this.hostedGatewayDTO.FailureURL;
            }



            if (string.IsNullOrWhiteSpace(this.hostedGatewayDTO.SuccessURL) || string.IsNullOrWhiteSpace(this.hostedGatewayDTO.FailureURL))
            {
                log.Error("Please enter WEB_SITE_CONFIGURATION LookUpValues description for  SuccessURL/FailureURL.");
                throw new Exception(utilities.MessageUtils.getMessage("Please enter WEB_SITE_CONFIGURATION LookUpValues description for  SuccessURL/FailureURL."));
            }

            log.LogMethodExit();
        }


        private IDictionary<string, string> SetPostParameters(TransactionPaymentsDTO transactionPaymentsDTO, CCRequestPGWDTO cCRequestPGWDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO, cCRequestPGWDTO);
            IDictionary<string, string> postparamslist = new Dictionary<string, string>();

            postparamslist.Clear();
            postparamslist.Add("sessionId", sessionid);
            postparamslist.Add("publishableKey", publishableKey);
            log.LogMethodExit(postparamslist);
            return postparamslist;
        }

        private string GetSubmitFormKeyValueList(IDictionary<string, string> postparamslist, string URL, string FormName, string submitMethod = "POST")
        {
            log.LogMethodEntry(postparamslist, URL, FormName, submitMethod);
            string Method = submitMethod;
            StringBuilder builder = new StringBuilder();
            builder.Clear();
            builder.Append("<html>");

            builder.Append(string.Format("<body onload=\"document.{0}.submit()\">", FormName));
            builder.Append(string.Format("<form name=\"{0}\" method=\"{1}\" action=\"{2}\" >", FormName, Method, URL));

            foreach (KeyValuePair<string, string> param in postparamslist)
            {
                builder.Append(string.Format("<input name=\"{0}\" type=\"hidden\" value=\"{1}\" />", param.Key, param.Value));
            }

            builder.Append("</form>");
            builder.Append("</body></html>");
            log.LogMethodExit(builder.ToString());
            return builder.ToString();
        }
        //private string loadStripeCheckOut(string sessionId)
        //{
        //    log.LogMethodEntry(sessionId);

        //    StringBuilder builder = new StringBuilder();
        //    builder.Clear();
        //    builder.Append("<html><head><script src =\"https://js.stripe.com/v3/\"></script></head>");
        //    builder.Append("<body onload =\"document.getElementById('btnPay').click()\">");
        //    builder.Append("<form><button id =\"btnPay\" onclick=\"checkout()\">Checkout</button></form>");
        //    builder.Append("<script>");
        //    builder.Append("var stripe = Stripe('"+ publishableKey + "');");
        //    builder.Append("var element = document.getElementById('btnPay');");
        //    builder.Append("function checkout(){");
        //    builder.Append("stripe.redirectToCheckout({");
        //    builder.Append("sessionId:'"+ sessionid+"'" );
        //    builder.Append("}).then(function (result) { }); } </script></body></html>");

        //    log.LogMethodExit(builder.ToString());
        //    return builder.ToString();
        //}
        public override HostedGatewayDTO CreateGatewayPaymentRequest(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);

            try
            {
                CCRequestPGWDTO cCRequestPGWDTO = this.CreateCCRequestPGW(transactionPaymentsDTO, TransactionType.SALE.ToString());
                StripeConfiguration.ApiKey = secretKey;
                StripeConfiguration.MaxNetworkRetries = 2;
                List<string> paymentOption = new List<string>();
                List<LookupValuesDTO> lookupValuesDTOList1;
                LookupValuesList lookupValuesList = new LookupValuesList(utilities.ExecutionContext);
                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookUpValueSearchParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                lookUpValueSearchParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "STRIPE_PAYMENT_MODES"));
                lookUpValueSearchParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));
                lookupValuesDTOList1 = lookupValuesList.GetAllLookupValues(lookUpValueSearchParameters);
                if (lookupValuesDTOList1 != null && lookupValuesDTOList1.Any())
                {
                    foreach (LookupValuesDTO element in lookupValuesDTOList1)
                    {
                        paymentOption.Add(element.Description);
                    }
                }


                var options = new SessionCreateOptions
                {
                    CustomerEmail = transactionPaymentsDTO.NameOnCreditCard,
                    PaymentMethodTypes = paymentOption,
                    LineItems = new List<SessionLineItemOptions> {
                                    new SessionLineItemOptions {
                                        PriceData = new SessionLineItemPriceDataOptions
                                        {
                                            UnitAmount = Convert.ToInt64(transactionPaymentsDTO.Amount * 100),
                                            Currency = currencyCode.ToLower(),
                                            ProductData = new SessionLineItemPriceDataProductDataOptions
                                            {
                                                Name = "Online Purchase",
                                            },
                                        },
                                        Quantity = 1,
                                    },
                                },
                    Metadata = new Dictionary<string, string>{
                              { "trxId",  transactionPaymentsDTO.TransactionId.ToString()},
                              { "customerName", transactionPaymentsDTO.CreditCardName },
                              { "paymentModeId", transactionPaymentsDTO.PaymentModeId.ToString() },
                           },
                    Mode = "payment",
                    SuccessUrl = successUrl + "?session_id={CHECKOUT_SESSION_ID}",
                    CancelUrl = cancelUrl,
                };
                var sessionService = new SessionService();
                var service = new SessionService();
                Session session = service.Create(options);
                log.Debug("seesion: " + session);

                sessionid = session.Id;
                cCRequestPGWDTO.ReferenceNo = sessionid;
                CCRequestPGWBL cCRequestPGWBL = new CCRequestPGWBL(utilities.ExecutionContext, cCRequestPGWDTO);
                cCRequestPGWBL.Save();

                //this.hostedGatewayDTO.PrivateKey = sessionid;
                this.hostedGatewayDTO.GatewayRequestString = GetSubmitFormKeyValueList(SetPostParameters(transactionPaymentsDTO, cCRequestPGWDTO), this.post_url, "StripeWebPaymentsForm");
                //this.hostedGatewayDTO.GatewayRequestString = loadStripeCheckOut(sessionid);
                log.Debug(this.hostedGatewayDTO.GatewayRequestString);

                this.hostedGatewayDTO.FailureURL = "/account/checkouterror";
                this.hostedGatewayDTO.SuccessURL = "/account/receipt";
                this.hostedGatewayDTO.CancelURL = "/account/checkoutstatus";
                LookupsList lookupList = new LookupsList(utilities.ExecutionContext);
                List<KeyValuePair<LookupsDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<LookupsDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<LookupsDTO.SearchByParameters, string>(LookupsDTO.SearchByParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));
                searchParameters.Add(new KeyValuePair<LookupsDTO.SearchByParameters, string>(LookupsDTO.SearchByParameters.LOOKUP_NAME, "WEB_SITE_CONFIGURATION"));
                List<LookupsDTO> lookups = lookupList.GetAllLookups(searchParameters, true);
                if (lookups != null && lookups.Any())
                {
                    List<LookupValuesDTO> lookupValuesDTOList = lookups[0].LookupValuesDTOList;
                    LookupValuesDTO temp = lookupValuesDTOList.FirstOrDefault(x => x.LookupValue.Equals("HOSTED_PAYMENT_FAILURE_URL"));
                    if (temp != null && !String.IsNullOrWhiteSpace(temp.Description))
                        this.hostedGatewayDTO.FailureURL = temp.Description;

                    temp = lookupValuesDTOList.FirstOrDefault(x => x.LookupValue.Equals("HOSTED_PAYMENT_SUCCESS_URL"));
                    if (temp != null && !String.IsNullOrWhiteSpace(temp.Description))
                        this.hostedGatewayDTO.SuccessURL = temp.Description;

                    temp = lookupValuesDTOList.FirstOrDefault(x => x.LookupValue.Equals("HOSTED_PAYMENT_CANCEL_URL"));
                    if (temp != null && !String.IsNullOrWhiteSpace(temp.Description))
                        this.hostedGatewayDTO.CancelURL = temp.Description;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw;
            }
            log.LogMethodExit(this.hostedGatewayDTO);
            return this.hostedGatewayDTO;
        }

        public override HostedGatewayDTO ProcessGatewayResponse(string gatewayResponse)
        {
            log.LogMethodEntry(gatewayResponse);
            this.hostedGatewayDTO.CCTransactionsPGWDTO = null;
            hostedGatewayDTO.TransactionPaymentsDTO = new TransactionPaymentsDTO();

            dynamic response = JsonConvert.DeserializeObject(gatewayResponse);
            try
            {
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
                log.Debug("Session Data");
                log.Debug(resData);
                log.Debug("End Session Data");

                if (resData["payment_status"] == "paid")
                {
                    var service = new PaymentIntentService();
                    var intent = service.Get(session.PaymentIntentId);
                    dynamic intentData = Newtonsoft.Json.JsonConvert.DeserializeObject(intent.StripeResponse.Content);
                    log.Debug("PaymentIntent Data");
                    log.Debug(intentData);
                    log.Debug("End PaymentIntent Data");
                    intentData["amount_received"] = (Convert.ToDouble(intentData["amount_received"]) / 100).ToString();
                    hostedGatewayDTO.TransactionPaymentsDTO.CreditCardAuthorization = intentData["id"];
                    hostedGatewayDTO.TransactionPaymentsDTO.Amount = intentData["amount_received"];
                    hostedGatewayDTO.TransactionPaymentsDTO.CreditCardNumber = intent.Charges.Data[0].PaymentMethodDetails.Card.Last4;
                    hostedGatewayDTO.TransactionPaymentsDTO.PaymentModeId = resData["metadata"]["paymentModeId"];
                    hostedGatewayDTO.TransactionPaymentsDTO.TransactionId = resData["metadata"]["trxId"];
                    hostedGatewayDTO.TransactionPaymentsDTO.CurrencyCode = resData["currency"];
                    hostedGatewayDTO.TransactionPaymentsDTO.Reference = intentData["id"];
                    hostedGatewayDTO.PaymentStatus = PaymentStatusType.SUCCESS; ;
                }
                else
                {
                    hostedGatewayDTO.PaymentStatus = PaymentStatusType.FAILED;
                    hostedGatewayDTO.TransactionPaymentsDTO.CreditCardAuthorization = resData["metadata"]["trxId"];
                }
                CCRequestPGWListBL cCRequestPGWListBL = new CCRequestPGWListBL();
                List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>> searchParametersPGW = new List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>>();
                searchParametersPGW.Add(new KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>(CCRequestPGWDTO.SearchByParameters.INVOICE_NUMBER, hostedGatewayDTO.TransactionPaymentsDTO.TransactionId.ToString()));
                CCRequestPGWDTO cCRequestsPGWDTO = cCRequestPGWListBL.GetLastestCCRequestPGWDTO(searchParametersPGW);
                this.TransactionSiteId = cCRequestsPGWDTO.SiteId;

                CCTransactionsPGWListBL cCTransactionsPGWListBL = new CCTransactionsPGWListBL();
                List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.REF_NO, hostedGatewayDTO.TransactionPaymentsDTO.Reference.ToString()));

                List<CCTransactionsPGWDTO> cCTransactionsPGWDTOList = cCTransactionsPGWListBL.GetNonReversedCCTransactionsPGWDTOList(searchParameters);
                if (cCTransactionsPGWDTOList == null)
                {
                    log.Debug("No CC Transactions found");
                    CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                    cCTransactionsPGWDTO.InvoiceNo = hostedGatewayDTO.TransactionPaymentsDTO.TransactionId.ToString();
                    cCTransactionsPGWDTO.AuthCode = hostedGatewayDTO.TransactionPaymentsDTO.CreditCardAuthorization;
                    cCTransactionsPGWDTO.Authorize = hostedGatewayDTO.TransactionPaymentsDTO.Amount.ToString();
                    cCTransactionsPGWDTO.Purchase = hostedGatewayDTO.TransactionPaymentsDTO.Amount.ToString();
                    cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                    cCTransactionsPGWDTO.RefNo = resData["id"];
                    cCTransactionsPGWDTO.AcctNo = hostedGatewayDTO.TransactionPaymentsDTO.CreditCardNumber;
                    cCTransactionsPGWDTO.RefNo = resData["metadata"]["trxId"];
                    cCTransactionsPGWDTO.RecordNo = hostedGatewayDTO.TransactionPaymentsDTO.CreditCardAuthorization;
                    cCTransactionsPGWDTO.TextResponse = hostedGatewayDTO.PaymentStatus.ToString();

                    this.hostedGatewayDTO.CCTransactionsPGWDTO = cCTransactionsPGWDTO;
                }
            }
            catch (Exception ex)
            {
                log.Error("Last transaction check failed", ex);
                throw;
            }


            log.Debug("Final hostedGatewayDTO " + hostedGatewayDTO.ToString());
            log.LogMethodExit(hostedGatewayDTO);

            if (hostedGatewayDTO.PaymentStatus != PaymentStatusType.SUCCESS)
            {
                hostedGatewayDTO.TransactionPaymentsDTO.PaymentModeId = -1;
                hostedGatewayDTO.TransactionPaymentsDTO.TransactionId = -1;
            }

            return hostedGatewayDTO;

        }
        public override TransactionPaymentsDTO RefundAmount(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            StripeConfiguration.ApiKey = secretKey;

            if (transactionPaymentsDTO != null)
            {
                try
                {
                    CCTransactionsPGWDTO ccOrigTransactionsPGWDTO = null;
                    CCRequestPGWDTO cCRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, CREDIT_CARD_REFUND);

                    if (transactionPaymentsDTO.CCResponseId > -1)
                    {
                        CCTransactionsPGWListBL cCTransactionsPGWListBL = new CCTransactionsPGWListBL();
                        List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>>();
                        searchParameters.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.RESPONSE_ID, transactionPaymentsDTO.CCResponseId.ToString()));

                        List<CCTransactionsPGWDTO> cCTransactionsPGWDTOList = cCTransactionsPGWListBL.GetCCTransactionsPGWDTOList(searchParameters);

                        ccOrigTransactionsPGWDTO = cCTransactionsPGWDTOList[0];
                    }

                    var options = new RefundCreateOptions
                    {
                        PaymentIntent = transactionPaymentsDTO.Reference,
                        Amount = Convert.ToInt64(transactionPaymentsDTO.Amount * 100),
                    };
                    var service = new RefundService();
                    var refund = service.Create(options);

                    dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject(refund.StripeResponse.Content);

                    log.Debug("data: " + data);

                    if (data["status"].ToString().ToLower() == "succeeded")
                    {
                        CCTransactionsPGWDTO ccTransactionsPGWDTO = new CCTransactionsPGWDTO();
                        ccTransactionsPGWDTO.InvoiceNo = cCRequestPGWDTO.RequestID.ToString();
                        ccTransactionsPGWDTO.TranCode = PaymentGatewayTransactionType.REFUND.ToString();
                        //ccTransactionsPGWDTO.CardType = ccOrigTransactionsPGWDTO.CardType;
                        ccTransactionsPGWDTO.AcctNo = transactionPaymentsDTO.CreditCardNumber;
                        ccTransactionsPGWDTO.ResponseOrigin = ccOrigTransactionsPGWDTO != null ? ccOrigTransactionsPGWDTO.ResponseID.ToString() : null;
                        ccTransactionsPGWDTO.Authorize = transactionPaymentsDTO.Amount.ToString();
                        ccTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                        ccTransactionsPGWDTO.RecordNo = "A";
                        ccTransactionsPGWDTO.TextResponse = data["status"];
                        ccTransactionsPGWDTO.AuthCode = data["payment_intent"];
                        ccTransactionsPGWDTO.AcqRefData = data["id"];
                        CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(ccTransactionsPGWDTO);
                        ccTransactionsPGWBL.Save();
                        transactionPaymentsDTO.CCResponseId = ccTransactionsPGWBL.CCTransactionsPGWDTO.ResponseID;
                        log.Debug("Refund Success");
                    }
                    else
                    {
                        log.Error("refund failed");
                        throw new Exception(utilities.MessageUtils.getMessage(2203));
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    throw;
                }
            }

            log.LogMethodExit(transactionPaymentsDTO);
            return transactionPaymentsDTO;
        }

        public override string GetTransactionStatus(string trxId)
        {
            log.LogMethodEntry();

            string sessionId;
            Dictionary<string, Object> dict = new Dictionary<string, Object>();
            dynamic resData;
            try
            {
                //ExecutionContext executionContext = ExecutionContext.GetExecutionContext();
                //StripeConfiguration.ApiKey = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "HOSTED_PAYMENT_GATEWAY_SECRET_KEY");

                StripeConfiguration.ApiKey = secretKey;

                CCRequestPGWListBL cCRequestPGWListBL = new CCRequestPGWListBL();
                List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>> searchParametersPGW = new List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>>();
                searchParametersPGW.Add(new KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>(CCRequestPGWDTO.SearchByParameters.INVOICE_NUMBER, trxId));
                CCRequestPGWDTO cCRequestsPGWDTO = cCRequestPGWListBL.GetLastestCCRequestPGWDTO(searchParametersPGW);

                sessionId = cCRequestsPGWDTO.ReferenceNo;

                var sessionService = new SessionService();
                Session session = sessionService.Get(sessionId);
                resData = Newtonsoft.Json.JsonConvert.DeserializeObject(session.StripeResponse.Content);
                log.Debug("Session Data");
                log.Debug(resData);
                log.Debug("End Session Data");

                if (resData["payment_status"] == "paid")
                {
                    var service = new PaymentIntentService();
                    var intent = service.Get(session.PaymentIntentId);
                    dynamic intentData = Newtonsoft.Json.JsonConvert.DeserializeObject(intent.StripeResponse.Content);
                    log.Debug("PaymentIntent Data");
                    log.Debug(intentData);
                    log.Debug("End PaymentIntent Data");

                    intentData["amount_received"] = (Convert.ToDouble(intentData["amount_received"]) / 100).ToString();

                    CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                    cCTransactionsPGWDTO.InvoiceNo = resData["metadata"]["trxId"].ToString();
                    cCTransactionsPGWDTO.AuthCode = intentData["id"];
                    cCTransactionsPGWDTO.Authorize = resData["amount"];
                    cCTransactionsPGWDTO.Purchase = resData["amount"];
                    cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                    cCTransactionsPGWDTO.RefNo = resData["id"];
                    cCTransactionsPGWDTO.AcctNo = intent.Charges.Data[0].PaymentMethodDetails.Card.Last4;
                    cCTransactionsPGWDTO.RefNo = resData["metadata"]["trxId"];
                    cCTransactionsPGWDTO.RecordNo = intentData["id"];
                    cCTransactionsPGWDTO.TextResponse = PaymentStatusType.SUCCESS.ToString();
                    cCTransactionsPGWDTO.TranCode = PaymentGatewayTransactionType.STATUSCHECK.ToString();

                    CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(cCTransactionsPGWDTO);
                    ccTransactionsPGWBL.Save();

                    dict.Add("status", "1");
                    dict.Add("message", "success");
                    dict.Add("retref", intentData["id"]);
                    dict.Add("amount", intentData["amount_received"]);
                    dict.Add("orderId", resData["metadata"]["trxId"]);
                    dict.Add("acctNo", intent.Charges.Data[0].PaymentMethodDetails.Card.Last4);

                    resData = JsonConvert.SerializeObject(dict, Newtonsoft.Json.Formatting.Indented);
                }
                else
                {
                    dict.Add("status", "0");
                    dict.Add("message", "no transaction found");
                    dict.Add("orderId", cCRequestsPGWDTO.InvoiceNo);
                    resData = JsonConvert.SerializeObject(dict, Newtonsoft.Json.Formatting.Indented);
                }

                log.LogMethodExit();
                return resData;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }
}
