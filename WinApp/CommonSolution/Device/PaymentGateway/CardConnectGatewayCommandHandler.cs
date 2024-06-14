/********************************************************************************************
 * Project Name - Device.PaymentGateway
 * Description  - AuthorizationCommandHandler Class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70.2      10-Aug-2019   Girish kundar            Modified : Removed Unused namespace's.         
 *2.110.0     08-Dec-2020   Guru S A                 Subscription changes
 *2.110.0     18-Mar-2021   Guru S A                 For Subscription phase one changes
 ********************************************************************************************/
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Device.PaymentGateway
{
    public class AuthorizationCommandHandler : CardConnectGatewayHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        bool isSaleRequest;
        private TransactionPaymentsDTO transactionPaymentsDTO;
        public AuthorizationCommandHandler(Utilities utilities, TransactionPaymentsDTO transactionPaymentsDTO, string url, string username, string password, string merchantId, bool isSaleRequest)
            : base(utilities, transactionPaymentsDTO, url, username, password, merchantId)
        {
            log.LogMethodEntry(utilities, transactionPaymentsDTO, url, username, password, merchantId);
            this.url = url + "auth";
            method = Methods.PUT.ToString();
            this.isSaleRequest = isSaleRequest;
            this.transactionPaymentsDTO = transactionPaymentsDTO;
            log.LogVariableState("username", username);
            log.LogVariableState("password", password);
            log.LogVariableState("method", method);
            log.LogVariableState("url", this.url);
            log.LogMethodExit();
        }

        public override void CreateCommand(object data)
        {
            try
            {
                log.LogMethodEntry(data);
                ReadCardResponse readCardResponse = data as ReadCardResponse;
                requestCollection = new NameValueCollection();
                requestCollection.Add("merchid", merchantId);
                requestCollection.Add("account", readCardResponse.TokenId);
                requestCollection.Add("orderid", readCardResponse.orderId);
                requestCollection.Add("expiry", readCardResponse.ExpDate);
                requestCollection.Add("currency", utilities.getParafaitDefaults("CURRENCY_CODE"));
                requestCollection.Add("amount", (transactionPaymentsDTO.Amount * 100).ToString());
                requestCollection.Add("name", readCardResponse.Name);
                if (!string.IsNullOrEmpty(readCardResponse.Signature))
                {
                    requestCollection.Add("signature", readCardResponse.Signature);
                }
                if (!string.IsNullOrEmpty(readCardResponse.ZipCode))
                {
                    requestCollection.Add("postal", readCardResponse.ZipCode);
                }
                if (isSaleRequest || transactionPaymentsDTO.SubscriptionAuthorizationMode != SubscriptionAuthorizationMode.N)
                {
                    requestCollection.Add("capture", "Y");
                }
                requestCollection.Add("includePIN", readCardResponse.includePIN ? "true" : "false");
                requestCollection.Add("aid", readCardResponse.aid);
                //if (transactionPaymentsDTO.SubscriptionAuthorizationMode == SubscriptionAuthorizationMode.N)
                //{
                //    requestCollection.Add("cof", "C");
                //    requestCollection.Add("cofscheduled", "N");
                //}
                if (transactionPaymentsDTO.SubscriptionAuthorizationMode == SubscriptionAuthorizationMode.I)
                {
                    requestCollection.Add("ecomind", "R");
                    requestCollection.Add("cof", "M");
                    if (transactionPaymentsDTO != null && transactionPaymentsDTO.Amount != 0)
                    {
                        requestCollection.Add("cofscheduled", "Y");
                    }
                    else
                    {
                        requestCollection.Add("cofscheduled", "N");
                    }
                    requestCollection.Add("profile", "Y");
                }
                if (transactionPaymentsDTO.SubscriptionAuthorizationMode == SubscriptionAuthorizationMode.P)
                {
                    requestCollection.Add("ecomind", "R");
                    requestCollection.Add("cof", "M");
                    if (transactionPaymentsDTO != null && transactionPaymentsDTO.Amount != 0)
                    {
                        requestCollection.Add("cofscheduled", "Y");
                    }
                    else
                    {
                        requestCollection.Add("cofscheduled", "N");
                    }
                    requestCollection.Add("profile", transactionPaymentsDTO.CustomerCardProfileId); 
                }
                    webRequest = webRequestHandler.CreateRequest(url, contenttype, method, username, password);
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Fatal("CreateCommand() Exception:" + ex.Message);
            }
        }
        public override HttpWebResponse Sendcommand()
        {
            try
            {
                log.LogMethodEntry();
                string data = Json.BuildJsonFromNVC(requestCollection);
                webResponse = webRequestHandler.SendRequest(webRequest, data);
                log.LogMethodExit();
                return webResponse;
            }
            catch (Exception ex)
            {
                log.Fatal("Sendcommand() Exception:" + ex.ToString());
                throw new Exception("Error occurred: " + ex.Message);
            }
        }

        public override object GetResponse(HttpWebResponse webResponse)
        {
            try
            {
                log.LogMethodEntry();
                AuthorizationResponse authorizationResponse = new AuthorizationResponse();
                EMVTagData emvTagData = new EMVTagData();
                string data = webRequestHandler.GetJsonData(webResponse);
                NameValueCollection emvTag = new NameValueCollection();
                responseCollection = Json.BuildNVCFromJson(data);
                authorizationResponse.Merchid = responseCollection["merchid"];
                authorizationResponse.Amount = responseCollection["amount"];
                authorizationResponse.Resptext = responseCollection["resptext"];
                authorizationResponse.CVVresp = responseCollection["cvvresp"];
                authorizationResponse.AVSresp = responseCollection["avsresp"];
                authorizationResponse.Respcode = responseCollection["respcode"];
                authorizationResponse.Token = responseCollection["token"];
                authorizationResponse.Authcode = responseCollection["authcode"];
                authorizationResponse.Respproc = responseCollection["respproc"];
                authorizationResponse.Retref = responseCollection["retref"];
                authorizationResponse.Respstat = responseCollection["respstat"];
                authorizationResponse.Account = responseCollection["account"];
                if (!string.IsNullOrEmpty(responseCollection["emvTagData"]))
                {
                    emvTag = Json.BuildNVCFromJson(responseCollection["emvTagData"]);
                    emvTagData.TVR = emvTag["TVR"];
                    emvTagData.ARC = emvTag["ARC"];
                    emvTagData.TSI = emvTag["TSI"];
                    emvTagData.AID = emvTag["AID"];
                    emvTagData.IAD = emvTag["IAD"];
                    emvTagData.EntryMethod = emvTag["Entry method"];
                }
                authorizationResponse.EmvTagData = emvTagData;
                if (string.IsNullOrWhiteSpace(responseCollection["profileId"]) == false)
                {
                    authorizationResponse.ProfileId = responseCollection["profileId"];
                }
                return authorizationResponse;
            }
            catch (Exception ex)
            {
                log.Fatal("GetResponse() Exception:" + ex.Message);
                return new AuthorizationResponse();
            }
        }

        public override CCTransactionsPGWDTO ParseResponse(object response)
        {
            log.LogMethodEntry();
            CCTransactionsPGWDTO ccTransactionsPGWDTO = new CCTransactionsPGWDTO();
            AuthorizationResponse authorizationResponse = response as AuthorizationResponse;
            string maskedAccount = (new String('X', 12) + ((authorizationResponse.Account.Length > 4)
                                       ? authorizationResponse.Account.Substring(authorizationResponse.Account.Length - 4)
                                       : authorizationResponse.Account));
            ccTransactionsPGWDTO.AcctNo = maskedAccount; //authorizationResponse.Account;
            ccTransactionsPGWDTO.TextResponse = authorizationResponse.Resptext;
            ccTransactionsPGWDTO.TokenID = authorizationResponse.Token;
            //ccTransactionsPGWDTO.TranCode = authorizationResponse.;
            ccTransactionsPGWDTO.DSIXReturnCode = authorizationResponse.Respcode;
            ccTransactionsPGWDTO.Authorize = authorizationResponse.Amount;
            //ccTransactionsPGWDTO.ResponseOrigin = authorizationResponse;
            ccTransactionsPGWDTO.RefNo = authorizationResponse.Retref;
            ccTransactionsPGWDTO.AuthCode = authorizationResponse.Authcode;
            ccTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
            ccTransactionsPGWDTO.RecordNo = authorizationResponse.Respstat;
            if (authorizationResponse.EmvTagData != null && !string.IsNullOrEmpty(authorizationResponse.EmvTagData.EntryMethod))
            {
                ccTransactionsPGWDTO.CaptureStatus = authorizationResponse.EmvTagData.EntryMethod;//.Contains("Contactless") ? "TAPPED" : authorizationResponse.EmvTagData.EntryMethod.Contains("Chip Read") ? "INSERTED" : "";
            }
            ccTransactionsPGWDTO.AcqRefData = "AID:" + authorizationResponse.EmvTagData.AID + "|ARC:" + authorizationResponse.EmvTagData.ARC + "|IAD:"
                + authorizationResponse.EmvTagData.IAD + "|TSI:" + authorizationResponse.EmvTagData.TSI + "|TVR:" + authorizationResponse.EmvTagData.TVR + "|EntryMethod:" + authorizationResponse.EmvTagData.EntryMethod;

            ccTransactionsPGWDTO.CustomerCardProfileId = authorizationResponse.ProfileId;

            log.LogMethodExit();
            return ccTransactionsPGWDTO;
        }
    }
    public class BinCommandHandler : CardConnectGatewayHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public BinCommandHandler(Utilities utilities, TransactionPaymentsDTO transactionPaymentsDTO, string url, string username, string password, string merchantId)
            : base(utilities, transactionPaymentsDTO, url, username, password, merchantId)
        {
            log.LogMethodEntry(utilities, transactionPaymentsDTO, url, username, password, merchantId);
            this.url = url + "bin/" + merchantId + "/";
            method = Methods.GET.ToString();
            log.LogVariableState("username", username);
            log.LogVariableState("password", password);
            log.LogVariableState("method", method);
            log.LogVariableState("url", this.url);
            log.LogMethodExit();
        }

        public override void CreateCommand(object data)
        {
            try
            {
                log.LogMethodEntry(data);
                ReadCardResponse readCardResponse = data as ReadCardResponse;
                if (data != DBNull.Value && !string.IsNullOrEmpty(data.ToString()))
                {
                    url = url + data.ToString();
                    webRequest = webRequestHandler.CreateRequest(url, contenttype, method, username, password);
                }
                else
                {
                    log.Error("Invalid card token.");
                    throw new Exception("Invalid card token.");
                }
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Fatal("CreateCommand() Exception:" + ex.Message);
            }
        }
        public override HttpWebResponse Sendcommand()
        {
            try
            {
                log.LogMethodEntry();
                //string data = Json.BuildJsonFromNVC(requestCollection);
                webResponse = webRequestHandler.SendRequest(webRequest, null);
                log.LogMethodExit();
                return webResponse;
            }
            catch (Exception ex)
            {
                log.Fatal("Sendcommand() Exception:" + ex.ToString());
                throw new Exception("Error occurred: " + ex.Message);
            }
        }

        public override object GetResponse(HttpWebResponse webResponse)
        {
            try
            {
                log.LogMethodEntry();
                BinData binData = new BinData();
                string data = webRequestHandler.GetJsonData(webResponse);
                responseCollection = Json.BuildNVCFromJson(data);
                if (responseCollection.AllKeys.Contains("errormsg"))
                {
                    log.Error("Error Message" + responseCollection["errormsg"]);
                    throw new Exception(responseCollection["errormsg"]);
                }
                else
                {
                    binData.Country = responseCollection["country"];
                    binData.Product = responseCollection["product"];
                    binData.CardUseString = responseCollection["cardusestring"];
                    binData.Gsa = Convert.ToBoolean(responseCollection["gsa"]);
                    binData.Corporate = responseCollection["corporate"];
                    binData.Fsa = Convert.ToBoolean(responseCollection["fsa"]);
                    binData.SubType = responseCollection["subtype"];
                    binData.Purchase = Convert.ToBoolean(responseCollection["purchase"]);
                    binData.Prepaid = responseCollection["prepaid"];
                    binData.CardNo = responseCollection["binlo"];
                    binData.Issuer = responseCollection["issuer"];
                    binData.AccountNo = responseCollection["binhi"];
                }
                log.LogMethodExit("returning binData ");
                return binData;
            }
            catch (Exception ex)
            {
                log.Fatal("GetResponse() Exception:" + ex.Message);
                return new AuthorizationResponse();
            }
        }


    }

    public class CaptureCommandHandler : CardConnectGatewayHandler
    {
        NameValueCollection totalDetails = new NameValueCollection();
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public CaptureCommandHandler(Utilities utilities, TransactionPaymentsDTO transactionPaymentsDTO, string url, string username, string password, string merchantId)
            : base(utilities, transactionPaymentsDTO, url, username, password, merchantId)
        {
            log.LogMethodEntry(utilities, transactionPaymentsDTO, url, username, password, merchantId);
            this.url = url + "capture";
            method = Methods.PUT.ToString();
            log.LogVariableState("username", username);
            log.LogVariableState("password", password);
            log.LogVariableState("method", method);
            log.LogVariableState("url", this.url);
            log.LogMethodExit();
        }

        public override void CreateCommand(object data)
        {
            try
            {
                log.LogMethodEntry(data);
                CCTransactionsPGWDTO readCardResponse = data as CCTransactionsPGWDTO;
                requestCollection = new NameValueCollection();
                requestCollection.Add("merchid", merchantId);
                requestCollection.Add("retref", readCardResponse.RefNo);
                requestCollection.Add("invoiceid", readCardResponse.InvoiceNo);
                requestCollection.Add("authcode", readCardResponse.AuthCode);
                requestCollection.Add("currency", utilities.getParafaitDefaults("CURRENCY_CODE"));
                requestCollection.Add("amount", (Math.Round((transactionPaymentsDTO.Amount + transactionPaymentsDTO.TipAmount), utilities.ParafaitEnv.RoundingPrecision, MidpointRounding.AwayFromZero) * 100).ToString());
                requestCollection.Add("capture", "Y");
                totalDetails = new NameValueCollection();
                totalDetails.Add("currency", utilities.getParafaitDefaults("CURRENCY_CODE"));
                totalDetails.Add("total", (transactionPaymentsDTO.Amount + transactionPaymentsDTO.TipAmount).ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL));
                totalDetails.Add("subTotal", null);
                totalDetails.Add("tax", "0.00");
                totalDetails.Add("tipAdjust", transactionPaymentsDTO.TipAmount.ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL));
                webRequest = webRequestHandler.CreateRequest(url, contenttype, method, username, password);
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Fatal("CreateCommand() Exception:" + ex.Message);
            }
        }
        public override HttpWebResponse Sendcommand()
        {
            try
            {
                log.LogMethodEntry();
                string data = Json.BuildJsonFromNVC(requestCollection);
                data = data.Replace("}", "");
                string totalDetail = Json.BuildJsonFromNVC(totalDetails);
                data = data + ",\"userfields\":{\"customFields\":[],\"totalDetails\":" + totalDetail + ",\"tip_adjust\":\"" + totalDetails["tipAdjust"] + "\"}}";
                webResponse = webRequestHandler.SendRequest(webRequest, data);
                log.LogMethodExit();
                return webResponse;
            }
            catch (Exception ex)
            {
                log.Fatal("Sendcommand() Exception:" + ex.ToString());
                throw new Exception("Error occurred: " + ex.Message);
            }
        }

        public override object GetResponse(HttpWebResponse webResponse)
        {
            try
            {
                log.LogMethodEntry();
                CaptureData captureData = new CaptureData();
                string data = webRequestHandler.GetJsonData(webResponse);
                responseCollection = Json.BuildNVCFromJson(data);
                captureData.MerchantID = responseCollection["merchid"];
                captureData.Amount = responseCollection["amount"];
                captureData.Resptext = responseCollection["resptext"];
                captureData.BatchId = responseCollection["batchid"];
                captureData.Respcode = responseCollection["respcode"];
                captureData.Token = responseCollection["token"];
                captureData.Authcode = responseCollection["authcode"];
                captureData.Respproc = responseCollection["respproc"];
                captureData.Retref = responseCollection["retref"];
                captureData.Setlstat = responseCollection["setlstat"];
                captureData.Account = responseCollection["account"];
                captureData.Respstat = responseCollection["respstat"];
                log.LogMethodExit();
                return captureData;
            }
            catch (Exception ex)
            {
                log.Fatal("GetResponse() Exception:" + ex.Message);
                return new AuthorizationResponse();
            }
        }

        public override CCTransactionsPGWDTO ParseResponse(object response)
        {
            log.LogMethodEntry();
            CCTransactionsPGWDTO ccTransactionsPGWDTO = new CCTransactionsPGWDTO();
            CaptureData captureData = response as CaptureData;
            string maskedAccount = (new String('X', 12) + ((captureData.Account.Length > 4)
                                       ? captureData.Account.Substring(captureData.Account.Length - 4)
                                       : captureData.Account));
            ccTransactionsPGWDTO.AcctNo = maskedAccount; // captureData.Account;
            ccTransactionsPGWDTO.TextResponse = captureData.Resptext;
            ccTransactionsPGWDTO.TokenID = captureData.Token;
            ccTransactionsPGWDTO.DSIXReturnCode = captureData.Respcode;
            ccTransactionsPGWDTO.Authorize = captureData.Amount;
            ccTransactionsPGWDTO.RefNo = captureData.Retref;
            ccTransactionsPGWDTO.AuthCode = captureData.Authcode;
            ccTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
            ccTransactionsPGWDTO.RecordNo = captureData.Respstat;
            ccTransactionsPGWDTO.ProcessData = captureData.BatchId;
            //ccTransactionsPGWDTO.AcqRefData = "AID:" + authorizationResponse.EmvTagData.AID + "|ARC:" + authorizationResponse.EmvTagData.ARC + "|IAD"
            //    + authorizationResponse.EmvTagData.IAD + "|TSI:" + authorizationResponse.EmvTagData.TSI + "|TVR:" + authorizationResponse.EmvTagData.TVR;
            log.LogMethodExit("ccTransactionsPGWDTO");
            return ccTransactionsPGWDTO;
        }
    }
    public class VoidCommandHandler : CardConnectGatewayHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public VoidCommandHandler(Utilities utilities, TransactionPaymentsDTO transactionPaymentsDTO, string url, string username, string password, string merchantId)
            : base(utilities, transactionPaymentsDTO, url, username, password, merchantId)
        {
            log.LogMethodEntry(utilities, transactionPaymentsDTO, url, username, password, merchantId);
            this.url = url + "void";
            method = Methods.PUT.ToString();
            log.LogVariableState("username", username);
            log.LogVariableState("password", password);
            log.LogVariableState("method", method);
            log.LogVariableState("url", this.url);
            log.LogMethodExit();
        }

        public override void CreateCommand(object data)
        {
            try
            {
                log.LogMethodEntry(data);
                CCTransactionsPGWDTO readCardResponse = data as CCTransactionsPGWDTO;
                requestCollection = new NameValueCollection();
                requestCollection.Add("merchid", merchantId);
                requestCollection.Add("retref", readCardResponse.RefNo);
                requestCollection.Add("currency", utilities.getParafaitDefaults("CURRENCY_CODE"));
                requestCollection.Add("amount", "0");
                webRequest = webRequestHandler.CreateRequest(url, contenttype, method, username, password);
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Fatal("CreateCommand() Exception:" + ex.Message);
            }
        }
        public override HttpWebResponse Sendcommand()
        {
            try
            {
                log.LogMethodEntry();
                string data = Json.BuildJsonFromNVC(requestCollection);
                webResponse = webRequestHandler.SendRequest(webRequest, data);
                log.LogMethodExit();
                return webResponse;
            }
            catch (Exception ex)
            {
                log.Fatal("Sendcommand() Exception:" + ex.ToString());
                throw new Exception("Error occurred: " + ex.Message);
            }
        }

        public override object GetResponse(HttpWebResponse webResponse)
        {
            try
            {
                log.LogMethodEntry();
                VoidResponse voidResponse = new VoidResponse();
                string data = webRequestHandler.GetJsonData(webResponse);
                responseCollection = Json.BuildNVCFromJson(data);
                //voidResponse.MerchantID = responseCollection["merchid"];
                voidResponse.Amount = responseCollection["amount"];
                voidResponse.Resptext = responseCollection["resptext"];
                //voidResponse.BatchId = responseCollection["batchid"];
                voidResponse.Respcode = responseCollection["respcode"];
                //voidResponse.Token = responseCollection["token"];
                voidResponse.Authcode = responseCollection["authcode"];
                voidResponse.Respproc = responseCollection["respproc"];
                voidResponse.Retref = responseCollection["retref"];
                //voidResponse.Setlstat = responseCollection["setlstat"];
                // voidResponse.Account = responseCollection["account"];
                voidResponse.Respstat = responseCollection["respstat"];
                log.LogMethodExit("returning voidResponse");
                return voidResponse;
            }
            catch (Exception ex)
            {
                log.Fatal("GetResponse() Exception:" + ex.Message);
                return new AuthorizationResponse();
            }
        }

        public override CCTransactionsPGWDTO ParseResponse(object response)
        {
            log.LogMethodEntry();
            CCTransactionsPGWDTO ccTransactionsPGWDTO = new CCTransactionsPGWDTO();
            VoidResponse voidResponse = response as VoidResponse;
            //ccTransactionsPGWDTO.AcctNo = captureData.Account;
            ccTransactionsPGWDTO.TextResponse = voidResponse.Resptext;
            //ccTransactionsPGWDTO.TokenID = captureData.Token;
            ccTransactionsPGWDTO.DSIXReturnCode = voidResponse.Respcode;
            ccTransactionsPGWDTO.Authorize = voidResponse.Amount;
            ccTransactionsPGWDTO.RefNo = voidResponse.Retref;
            ccTransactionsPGWDTO.AuthCode = voidResponse.Authcode;
            ccTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
            ccTransactionsPGWDTO.RecordNo = voidResponse.Respstat;
            //ccTransactionsPGWDTO.ProcessData = captureData.BatchId;
            //ccTransactionsPGWDTO.AcqRefData = "AID:" + authorizationResponse.EmvTagData.AID + "|ARC:" + authorizationResponse.EmvTagData.ARC + "|IAD"
            //    + authorizationResponse.EmvTagData.IAD + "|TSI:" + authorizationResponse.EmvTagData.TSI + "|TVR:" + authorizationResponse.EmvTagData.TVR;
            log.LogMethodExit("returning ccTransactionsPGWDTO");
            return ccTransactionsPGWDTO;
        }
    }
    public class RefundCommandHandler : CardConnectGatewayHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public RefundCommandHandler(Utilities utilities, TransactionPaymentsDTO transactionPaymentsDTO, string url, string username, string password, string merchantId)
            : base(utilities, transactionPaymentsDTO, url, username, password, merchantId)
        {
            log.LogMethodEntry(utilities, transactionPaymentsDTO, url, username, password, merchantId);
            this.url = url + "refund";
            method = Methods.PUT.ToString();
            log.LogVariableState("username", username);
            log.LogVariableState("password", password);
            log.LogVariableState("method", method);
            log.LogVariableState("url", this.url);
            log.LogMethodExit();
        }

        public override void CreateCommand(object data)
        {
            try
            {
                log.LogMethodEntry(data);
                CCTransactionsPGWDTO readCardResponse = data as CCTransactionsPGWDTO;
                requestCollection = new NameValueCollection();
                requestCollection.Add("merchid", merchantId);
                requestCollection.Add("retref", readCardResponse.RefNo);
                requestCollection.Add("currency", utilities.getParafaitDefaults("CURRENCY_CODE"));
                requestCollection.Add("amount", (Math.Round((transactionPaymentsDTO.Amount + transactionPaymentsDTO.TipAmount), utilities.ParafaitEnv.RoundingPrecision, MidpointRounding.AwayFromZero) * 100).ToString());
                webRequest = webRequestHandler.CreateRequest(url, contenttype, method, username, password);
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Fatal("CreateCommand() Exception:" + ex.Message);
            }
        }
        public override HttpWebResponse Sendcommand()
        {
            try
            {
                log.LogMethodEntry();
                string data = Json.BuildJsonFromNVC(requestCollection);
                webResponse = webRequestHandler.SendRequest(webRequest, data);
                log.LogMethodExit();
                return webResponse;
            }
            catch (Exception ex)
            {
                log.Fatal("Sendcommand() Exception:" + ex.ToString());
                throw new Exception("Error occurred: " + ex.Message);
            }
        }

        public override object GetResponse(HttpWebResponse webResponse)
        {
            try
            {
                log.LogMethodEntry();
                RefundResponse refundResponse = new RefundResponse();
                string data = webRequestHandler.GetJsonData(webResponse);
                responseCollection = Json.BuildNVCFromJson(data);
                //voidResponse.MerchantID = responseCollection["merchid"];
                refundResponse.Amount = responseCollection["amount"];
                refundResponse.Resptext = responseCollection["resptext"];
                //voidResponse.BatchId = responseCollection["batchid"];
                refundResponse.Respcode = responseCollection["respcode"];
                //voidResponse.Token = responseCollection["token"];
                refundResponse.Authcode = responseCollection["authcode"];
                refundResponse.Respproc = responseCollection["respproc"];
                refundResponse.Retref = responseCollection["retref"];
                //voidResponse.Setlstat = responseCollection["setlstat"];
                // voidResponse.Account = responseCollection["account"];
                refundResponse.Respstat = responseCollection["respstat"];
                log.LogMethodExit("returning refundResponse");
                return refundResponse;
            }
            catch (Exception ex)
            {
                log.Fatal("GetResponse() Exception:" + ex.Message);
                return new AuthorizationResponse();
            }
        }

        public override CCTransactionsPGWDTO ParseResponse(object response)
        {
            log.LogMethodEntry();
            CCTransactionsPGWDTO ccTransactionsPGWDTO = new CCTransactionsPGWDTO();
            RefundResponse refundResponse = response as RefundResponse;
            //ccTransactionsPGWDTO.AcctNo = captureData.Account;
            ccTransactionsPGWDTO.TextResponse = refundResponse.Resptext;
            //ccTransactionsPGWDTO.TokenID = captureData.Token;
            ccTransactionsPGWDTO.DSIXReturnCode = refundResponse.Respcode;
            ccTransactionsPGWDTO.Authorize = refundResponse.Amount;
            ccTransactionsPGWDTO.RefNo = refundResponse.Retref;
            ccTransactionsPGWDTO.AuthCode = refundResponse.Authcode;
            ccTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
            ccTransactionsPGWDTO.RecordNo = refundResponse.Respstat;
            //ccTransactionsPGWDTO.ProcessData = captureData.BatchId;
            //ccTransactionsPGWDTO.AcqRefData = "AID:" + authorizationResponse.EmvTagData.AID + "|ARC:" + authorizationResponse.EmvTagData.ARC + "|IAD"
            //    + authorizationResponse.EmvTagData.IAD + "|TSI:" + authorizationResponse.EmvTagData.TSI + "|TVR:" + authorizationResponse.EmvTagData.TVR;
            log.LogMethodExit("returning ccTransactionsPGWDTO");
            return ccTransactionsPGWDTO;
        }
    }

    public class InquireStatusCommandHandler : CardConnectGatewayHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public InquireStatusCommandHandler(Utilities utilities, string url, string username, string password, string merchantId)
            : base(utilities, null, url, username, password, merchantId)
        {
            log.LogMethodEntry(utilities, null, url, username, password, merchantId);
            this.url = url;
            method = Methods.GET.ToString();
            log.LogVariableState("username", username);
            log.LogVariableState("password", password);
            log.LogVariableState("method", method);
            log.LogVariableState("url", this.url);
            log.LogMethodExit();
        }

        public override void CreateCommand(object data)
        {
            try
            {
                log.LogMethodEntry(data);
                CCRequestPGWDTO ccRequestPGWDTO;
                CCTransactionsPGWDTO cCTransactionsPGWDTO;
                if (data != DBNull.Value)
                {
                    try
                    {
                        ccRequestPGWDTO = data as CCRequestPGWDTO;
                        url = url + "inquireByOrderid/" + ccRequestPGWDTO.RequestID + "/" + merchantId + "/1";//Specify 1 for the set parameter to restrict the inquiry to the specified MID. 
                    }
                    catch
                    {
                        cCTransactionsPGWDTO = data as CCTransactionsPGWDTO;
                        url = url + "inquire/" + cCTransactionsPGWDTO.RefNo + "/" + merchantId;
                    }
                    webRequest = webRequestHandler.CreateRequest(url, contenttype, method, username, password);
                }
                else
                {
                    log.Error("Error in creating Inquire Command");
                    throw new Exception("Status check command failed");
                }
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Fatal("CreateCommand() Exception:" + ex.Message);
            }
        }
        public override HttpWebResponse Sendcommand()
        {
            try
            {
                log.LogMethodEntry();
                //string data = Json.BuildJsonFromNVC(requestCollection);
                webResponse = webRequestHandler.SendRequest(webRequest, null);
                log.LogMethodExit();
                return webResponse;
            }
            catch (Exception ex)
            {
                log.Fatal("Sendcommand() Exception:" + ex.ToString());
                throw new Exception("Error occurred: " + ex.Message);
            }
        }

        public override object GetResponse(HttpWebResponse webResponse)
        {
            InquireResponse inquireResponse = new InquireResponse();
            try
            {
                log.LogMethodEntry();
                EMVTagData emvTagData = new EMVTagData();
                string data = webRequestHandler.GetJsonData(webResponse);
                NameValueCollection emvTag = new NameValueCollection();
                NameValueCollection userFields = new NameValueCollection();
                responseCollection = Json.BuildNVCFromJson(data);
                inquireResponse.Merchid = responseCollection["merchid"];
                inquireResponse.Amount = responseCollection["amount"];
                inquireResponse.Resptext = responseCollection["resptext"];
                inquireResponse.OrderId = responseCollection["orderId"];
                inquireResponse.Setlstat = responseCollection["setlstat"];
                inquireResponse.Batchid = responseCollection["batchid"];
                inquireResponse.Voidable = responseCollection["voidable"];
                inquireResponse.Refundable = responseCollection["refundable"];
                inquireResponse.Capturedate = responseCollection["capturedate"];
                inquireResponse.Entrymode = responseCollection["entrymode"];
                inquireResponse.Respcode = responseCollection["respcode"];
                inquireResponse.Token = responseCollection["token"];
                inquireResponse.Authcode = responseCollection["authcode"];
                inquireResponse.Respproc = responseCollection["respproc"];
                inquireResponse.Retref = responseCollection["retref"];
                inquireResponse.Respstat = responseCollection["respstat"];
                inquireResponse.Account = responseCollection["account"];
                if (responseCollection.GetValues("emvTagData") != null)
                {
                    if (!string.IsNullOrEmpty(responseCollection["emvTagData"]))
                    {
                        emvTag = Json.BuildNVCFromJson(responseCollection["emvTagData"]);
                        emvTagData.TVR = emvTag["TVR"];
                        emvTagData.ARC = emvTag["ARC"];
                        emvTagData.TSI = emvTag["TSI"];
                        emvTagData.AID = emvTag["AID"];
                        emvTagData.IAD = emvTag["IAD"];
                        emvTagData.EntryMethod = emvTag["Entry method"];
                    }
                    inquireResponse.EmvTagData = emvTagData;
                }
                else
                {
                    inquireResponse.EmvTagData = new EMVTagData();
                }
                //try
                //{
                //    if (!string.IsNullOrEmpty(responseCollection["userfields"]))
                //    {
                //        userFields = Json.BuildNVCFromJson(responseCollection["userfields"]);
                //        inquireResponse.Customphone = userFields["customphone"];
                //        inquireResponse.Custommerchant = userFields["custommerchant"];
                //    }
                //}
                //catch(Exception ex)
                //{
                //    log.Error("Error ignorable: Status check inquire error-", ex);
                //}
                log.LogMethodExit(inquireResponse);
                return inquireResponse;
            }
            catch (Exception ex)
            {
                log.Fatal("GetResponse() Exception:" + ex.Message);
                if (inquireResponse != null && string.IsNullOrEmpty(inquireResponse.Setlstat))
                {
                    return inquireResponse;
                }
                else
                {
                    return new InquireResponse();
                }
            }
        }

        public override CCTransactionsPGWDTO ParseResponse(object response)
        {
            log.LogMethodEntry(response);
            CCTransactionsPGWDTO ccTransactionsPGWDTO = new CCTransactionsPGWDTO();
            InquireResponse inquireResponse = response as InquireResponse;
            string maskedAccount = (new String('X', 12) + ((inquireResponse.Account.Length > 4)
                                       ? inquireResponse.Account.Substring(inquireResponse.Account.Length - 4)
                                       : inquireResponse.Account));
            ccTransactionsPGWDTO.AcctNo = maskedAccount; // inquireResponse.Account;
            ccTransactionsPGWDTO.TextResponse = inquireResponse.Resptext;
            ccTransactionsPGWDTO.TokenID = inquireResponse.Token;
            //ccTransactionsPGWDTO.TranCode = authorizationResponse.;
            ccTransactionsPGWDTO.DSIXReturnCode = inquireResponse.Respcode;
            ccTransactionsPGWDTO.Authorize = inquireResponse.Amount;
            //ccTransactionsPGWDTO.ResponseOrigin = authorizationResponse;
            ccTransactionsPGWDTO.RefNo = inquireResponse.Retref;
            ccTransactionsPGWDTO.AuthCode = inquireResponse.Authcode;
            ccTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
            ccTransactionsPGWDTO.RecordNo = inquireResponse.Respstat;
            if (inquireResponse.EmvTagData != null && !string.IsNullOrEmpty(inquireResponse.EmvTagData.EntryMethod))
            {
                ccTransactionsPGWDTO.CaptureStatus = inquireResponse.EmvTagData.EntryMethod;//.Contains("Contactless") ? "TAPPED" : authorizationResponse.EmvTagData.EntryMethod.Contains("Chip Read") ? "INSERTED" : "";
            }
            ccTransactionsPGWDTO.AcqRefData = "AID:" + inquireResponse.EmvTagData.AID + "|ARC:" + inquireResponse.EmvTagData.ARC + "|IAD:"
                + inquireResponse.EmvTagData.IAD + "|TSI:" + inquireResponse.EmvTagData.TSI + "|TVR:" + inquireResponse.EmvTagData.TVR + "|EntryMethod:" + inquireResponse.EmvTagData.EntryMethod;
            log.LogMethodExit();
            return ccTransactionsPGWDTO;
        }
    }
}
