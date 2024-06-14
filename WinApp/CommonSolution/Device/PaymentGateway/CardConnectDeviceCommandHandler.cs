/********************************************************************************************
 * Project Name - Device.PaymentGateway
 * Description  - ConnectCommandHandler Class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70.2        10-Aug-2019   Girish kundar            Modified : Added Logger Method and Removed Unused namespace's.
 ********************************************************************************************/
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Device.PaymentGateway
{

    public class ConnectCommandHandler : CardConnectDeviceHandler
    {
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ConnectCommandHandler(Utilities utilities, TransactionPaymentsDTO transactionPaymentsDTO, string sessionKey, string merchantId, string url, string deviceId, string authorization)
            : base(utilities, transactionPaymentsDTO, sessionKey, merchantId, url, deviceId, authorization)
        {
            log.LogMethodEntry(utilities, transactionPaymentsDTO, sessionKey);
            this.url = url + "connect";
            method = Methods.POST.ToString();
            sessionKey = "";
            log.LogVariableState("sessionKey", sessionKey);
            log.LogVariableState("method", method);
            log.LogVariableState("url", this.url);
            log.LogMethodExit();
        }

        public override void CreateCommand(object data)
        {
            try
            {
                log.LogMethodEntry(data);
                requestCollection = new NameValueCollection();
                requestCollection.Add("merchantId", merchantId);
                requestCollection.Add("hsn", deviceId);
                requestCollection.Add("force", "true");
                log.Debug("Creating Web Request");
                webRequest = webRequestHandler.CreateRequest(url, contenttype, method, authorization);
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
                log.Fatal("Sendcommand() Exception:" + ex.Message);
                throw new Exception("Error occurred: "+ ex.Message);
            }
        }

        public override object GetResponse(HttpWebResponse webResponse)
        {
            try
            {
                log.LogMethodEntry();
                string data = webRequestHandler.GetHeaderKeyValue(webResponse, "X-CardConnect-SessionKey");
                if (string.IsNullOrEmpty(data))
                {
                    log.Debug("Connect process failed to get session key.");
                    throw new Exception(utilities.MessageUtils.getMessage("Connect process failed to get session key."));
                }
                string[] stringArray = data.Split(';');
                if (stringArray.Length > 1)
                {
                    if (!string.IsNullOrEmpty(stringArray[0]))
                    {
                        sessionKey = stringArray[0];
                    }
                    else
                    {
                        log.Error("Connect process failed to get session key.");
                        throw new Exception(utilities.MessageUtils.getMessage("Connect process failed to get session key."));
                    }
                }
                else
                {
                    log.Error("Connect process failed to get session key.");
                    throw new Exception(utilities.MessageUtils.getMessage("Connect process failed to get session key."));
                }
                //string data = webRequestHandler.GetJsonData(webResponse);
                //responseCollection = Json.BuildNVCFromJson(data);
                log.LogMethodExit(sessionKey);
                return sessionKey;
            }
            catch (Exception ex)
            {
                log.Fatal("GetResponse() Exception:" + ex.Message);
                return null;
            }
        }

        public override CCTransactionsPGWDTO ParseResponse(object response)
        {
            log.LogMethodEntry();
            log.LogMethodExit();
            return null;
        }
    }
    public class DisplayCommandHandler : CardConnectDeviceHandler
    {
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public DisplayCommandHandler(Utilities utilities, TransactionPaymentsDTO transactionPaymentsDTO, string sessionKey, string merchantId, string url, string deviceId, string authorization)
            : base(utilities,  transactionPaymentsDTO, sessionKey, merchantId, url, deviceId, authorization)
        {
            log.LogMethodEntry(utilities, transactionPaymentsDTO, sessionKey);
            this.url = url + "display";
            method = Methods.POST.ToString();
            this.sessionKey = sessionKey;
            log.LogVariableState("sessionKey", sessionKey);
            log.LogVariableState("method", method);
            log.LogVariableState("url", this.url);
            log.LogMethodExit();
        }

        public override void CreateCommand(object data)
        {
            try
            {
                log.LogMethodEntry(data);
                requestCollection = new NameValueCollection();
                requestCollection.Add("merchantId", merchantId);
                requestCollection.Add("hsn", deviceId);
                requestCollection.Add("text", (data == DBNull.Value) ? "" : data.ToString());
                log.Debug("Creating Web Request");
                webRequest = webRequestHandler.CreateRequest(url, contenttype, method, authorization);
                if (!string.IsNullOrEmpty(sessionKey))
                {
                    webRequestHandler.AddToHeader(webRequest, "X-CardConnect-SessionKey", sessionKey);
                }
                else
                {
                    log.Error("Invalid session.");
                    throw new Exception("Invalid session.");
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
                string data = Json.BuildJsonFromNVC(requestCollection);
                webResponse = webRequestHandler.SendRequest(webRequest, data);
                log.LogMethodExit();
                return webResponse;
            }
            catch (Exception ex)
            {
                log.Fatal("Sendcommand() Exception:" + ex.Message);
                return null;
            }
        }
    }
    public class ReadInputCommandHandler : CardConnectDeviceHandler
    {
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ReadInputCommandHandler(Utilities utilities, TransactionPaymentsDTO transactionPaymentsDTO, string sessionKey, string merchantId, string url, string deviceId, string authorization)
            : base(utilities, transactionPaymentsDTO, sessionKey, merchantId, url, deviceId, authorization)
        {
            log.LogMethodEntry(utilities, transactionPaymentsDTO, sessionKey);
            this.url = url + "readInput";
            method = Methods.POST.ToString();
            this.sessionKey = sessionKey;
            log.LogVariableState("sessionKey", sessionKey);
            log.LogVariableState("method", method);
            log.LogVariableState("url", this.url);
            log.LogMethodExit();
        }

        public override void CreateCommand(object data)
        {
            try
            {
                log.LogMethodEntry(data);
                ReadInputRequest readInputRequest = data as ReadInputRequest;
                string format = "";
                if (readInputRequest.Format.Equals(InputType.ALPHA_NUMERIC))
                {
                    format = "AN" + ((readInputRequest.MinLength <= 0) ? 5 : readInputRequest.MinLength) + ((readInputRequest.MaxLength > 0) ? "," + readInputRequest.MaxLength : "");
                }
                else if (readInputRequest.Format.Equals(InputType.NUMERIC))
                {
                    format = "N" + ((readInputRequest.MinLength <= 0) ? 5 : readInputRequest.MinLength) + ((readInputRequest.MaxLength > 0) ? "," + readInputRequest.MaxLength : "");
                }
                else if (readInputRequest.Format.Equals(InputType.MMYY))
                {
                    format = "MMYY";
                }
                else if (readInputRequest.Format.Equals(InputType.PHONE_NUMBER))
                {
                    format = "PHONE";
                }
                else if (readInputRequest.Format.Equals(InputType.AMOUNT))
                {
                    format = "AMOUNT";
                }
                else
                {
                    log.LogVariableState("format :", format);
                    log.Error("Invalid format");
                    throw new Exception("Invalid format");
                }
                requestCollection = new NameValueCollection();
                requestCollection.Add("merchantId", merchantId);
                requestCollection.Add("hsn", deviceId);
                requestCollection.Add("prompt", readInputRequest.DisplayMessage);
                requestCollection.Add("format", format);
                log.Debug("Creating Web Request");
                webRequest = webRequestHandler.CreateRequest(url, contenttype, method, authorization);
                if (!string.IsNullOrEmpty(sessionKey))
                {
                    webRequestHandler.AddToHeader(webRequest, "X-CardConnect-SessionKey", sessionKey);
                }
                else
                {
                    log.Error("Invalid session.");
                    throw new Exception("Invalid session.");
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
                string data = Json.BuildJsonFromNVC(requestCollection);
                webResponse = webRequestHandler.SendRequest(webRequest, data);
                log.LogMethodExit();
                return webResponse;
            }
            catch (Exception ex)
            {
                log.Fatal("Sendcommand() Exception:" + ex.ToString());
                throw new Exception( ex.Message);
            }
        }
        public override object GetResponse(HttpWebResponse webResponse)
        {
            try
            {
                string result;
                log.LogMethodEntry();
                string data = webRequestHandler.GetJsonData(webResponse);
                responseCollection = Json.BuildNVCFromJson(data);
                if (responseCollection.AllKeys.Contains("errorMessage"))
                {
                    log.Error("ErrorMessage : "+ responseCollection["errorMessage"]);
                    throw new Exception(responseCollection["errorMessage"]);
                }
                else
                {
                    result = responseCollection["input"];
                    log.LogMethodExit(result);
                    return result;
                }
            }
            catch (Exception ex)
            {
                log.Fatal("GetResponse() Exception:" + ex.Message);
                return null;
            }
        }
    }
    public class ReadCardCommandHandler : CardConnectDeviceHandler
    {
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ReadCardCommandHandler(Utilities utilities, TransactionPaymentsDTO transactionPaymentsDTO, string sessionKey, string merchantId, string url, string deviceId, string authorization)
            : base(utilities, transactionPaymentsDTO, sessionKey, merchantId, url, deviceId, authorization)
        {
            log.LogMethodEntry(utilities,  transactionPaymentsDTO, sessionKey);
            this.url = url + "readCard";
            method = Methods.POST.ToString();
            this.sessionKey = sessionKey;
            log.LogVariableState("sessionKey", sessionKey);
            log.LogVariableState("method", method);
            log.LogVariableState("url", this.url);
            log.LogMethodExit();
        }

        public override void CreateCommand(object data)
        {
            try
            {
                log.LogMethodEntry(data);
                ReadCardRequest readCardRequest = data as ReadCardRequest;
                requestCollection = new NameValueCollection();
                requestCollection.Add("merchantId", merchantId);
                requestCollection.Add("hsn", deviceId);
                requestCollection.Add("includeAmountDisplay", "true");
                requestCollection.Add("beep", readCardRequest.IsBeepSoundRequired.ToString());
                requestCollection.Add("includePIN", readCardRequest.includePIN ? "true" : "false");
                requestCollection.Add("aid", readCardRequest.aid);
                requestCollection.Add("amount", ((transactionPaymentsDTO.Amount < 0 ? transactionPaymentsDTO.Amount * -1 : transactionPaymentsDTO.Amount) * 100).ToString());
                requestCollection.Add("includeSignature", readCardRequest.IsSignatureRequired ? "true" : "false");
                requestCollection.Add("confirmAmount", "false");
                log.Debug("Creating Web Request");
                webRequest = webRequestHandler.CreateRequest(url, contenttype, method, authorization);
                if (!string.IsNullOrEmpty(sessionKey))
                {
                    webRequestHandler.AddToHeader(webRequest, "X-CardConnect-SessionKey", sessionKey);
                }
                else
                {
                    log.Error("Invalid session.");
                    throw new Exception("Invalid session.");
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
                string data = Json.BuildJsonFromNVC(requestCollection);
                webResponse = webRequestHandler.SendRequest(webRequest, data);
                log.LogMethodExit();
                return webResponse;
            }
            catch (Exception ex)
            {
                log.Fatal("Sendcommand() Exception:" + ex.ToString());
                throw new Exception( ex.Message);
            }
        }
        public override object GetResponse(HttpWebResponse webResponse)
        {
            try
            {
                ReadCardResponse readCardResp = new ReadCardResponse();
                log.LogMethodEntry();
                string data = webRequestHandler.GetJsonData(webResponse);
                responseCollection = Json.BuildNVCFromJson(data);
                if (responseCollection.AllKeys.Contains("errorMessage"))
                {
                    log.Error("ErrorMessage :" + responseCollection["errorMessage"]);
                    throw new Exception(responseCollection["errorMessage"]);
                }
                else
                {                    
                    readCardResp.TokenId = responseCollection["Token"];
                    readCardResp.ExpDate = responseCollection["expiry"];
                    readCardResp.Name = responseCollection["name"];                    
                    readCardResp.Signature = responseCollection["signature"];
                    log.LogMethodExit(readCardResp);
                    return readCardResp;
                }
            }
            catch (Exception ex)
            {
                log.Fatal("GetResponse() Exception:" + ex.Message);
                return new ReadCardResponse();
            }
        }
    }
    public class ReadManualCommandHandler : CardConnectDeviceHandler
    {
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ReadManualCommandHandler(Utilities utilities, TransactionPaymentsDTO transactionPaymentsDTO, string sessionKey, string merchantId, string url, string deviceId, string authorization)
            : base(utilities, transactionPaymentsDTO, sessionKey, merchantId, url, deviceId, authorization)
        {
            log.LogMethodEntry(utilities, transactionPaymentsDTO, sessionKey);
            this.url = url + "readManual";
            method = Methods.POST.ToString();
            this.sessionKey = sessionKey;
            log.LogVariableState("sessionKey", sessionKey);
            log.LogVariableState("method", method);
            log.LogVariableState("url", this.url);
            log.LogMethodExit();
        }

        public override void CreateCommand(object data)
        {
            try
            {
                log.LogMethodEntry(data);
                ReadCardRequest readCardRequest = data as ReadCardRequest;
                requestCollection = new NameValueCollection();
                requestCollection.Add("merchantId", merchantId);
                requestCollection.Add("hsn", deviceId);
                requestCollection.Add("includeAmountDisplay", "true");
                requestCollection.Add("beep", readCardRequest.IsBeepSoundRequired.ToString());
                requestCollection.Add("includePIN", readCardRequest.includePIN ? "true" : "false");
                requestCollection.Add("aid", readCardRequest.aid);
                requestCollection.Add("amount", (transactionPaymentsDTO.Amount * 100).ToString());
                requestCollection.Add("includeSignature", readCardRequest.IsSignatureRequired ? "true" : "false");
                log.Debug("Creating Web Request..");
                webRequest = webRequestHandler.CreateRequest(url, contenttype, method, authorization);
                if (!string.IsNullOrEmpty(sessionKey))
                {
                    webRequestHandler.AddToHeader(webRequest, "X-CardConnect-SessionKey", sessionKey);
                }
                else
                {
                    log.Error("Invalid session.");
                    throw new Exception("Invalid session.");
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
                string data = Json.BuildJsonFromNVC(requestCollection);
                log.Debug("Sending Web Request..");
                webResponse = webRequestHandler.SendRequest(webRequest, data);
                log.LogMethodExit();
                return webResponse;
            }
            catch (Exception ex)
            {
                log.Fatal("Sendcommand() Exception:" + ex.ToString());
                throw new Exception( ex.Message);
            }
        }
        public override object GetResponse(HttpWebResponse webResponse)
        {
            try
            {
                ReadCardResponse readCardResp = new ReadCardResponse();
                log.LogMethodEntry();
                string data = webRequestHandler.GetJsonData(webResponse);
                responseCollection = Json.BuildNVCFromJson(data);
                if (responseCollection.AllKeys.Contains("errorMessage"))
                {
                    log.Error("ErrorMessage" + responseCollection["errorMessage"]);
                    throw new Exception(responseCollection["errorMessage"]);
                }
                else
                {
                    readCardResp.TokenId = responseCollection["Token"];
                    readCardResp.ExpDate = responseCollection["expiry"];
                    readCardResp.Name = responseCollection["name"];
                    readCardResp.Signature = responseCollection["signature"];
                    log.LogMethodExit(readCardResp);
                    return readCardResp;
                }
            }
            catch (Exception ex)
            {
                log.Fatal("GetResponse() Exception:" + ex.Message);
                return new ReadCardResponse();
            }
        }
    }
    public class AuthCardCommandHandler : CardConnectDeviceHandler
    {
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public AuthCardCommandHandler(Utilities utilities, TransactionPaymentsDTO transactionPaymentsDTO, string sessionKey, string merchantId, string url, string deviceId, string authorization)
            : base(utilities, transactionPaymentsDTO, sessionKey, merchantId, url, deviceId, authorization)
        {
            log.LogMethodEntry(utilities, transactionPaymentsDTO, sessionKey);
            this.url = url + "authCard";
            method = Methods.POST.ToString();
            this.sessionKey = sessionKey;
            log.LogVariableState("sessionKey", sessionKey);
            log.LogVariableState("method", method);
            log.LogVariableState("url", this.url);
            log.LogMethodExit();
        }

        public override void CreateCommand(object data)
        {
            try
            {
                log.LogMethodEntry(data);
                ReadCardRequest readCardRequest = data as ReadCardRequest;
                requestCollection = new NameValueCollection();
                requestCollection.Add("merchantId", merchantId);
                requestCollection.Add("hsn", deviceId);
                requestCollection.Add("includeAmountDisplay", "true");
                requestCollection.Add("beep", readCardRequest.IsBeepSoundRequired.ToString());
                requestCollection.Add("includePIN", readCardRequest.includePIN ? "true" : "false");
                requestCollection.Add("aid", readCardRequest.aid);
                requestCollection.Add("amount", (transactionPaymentsDTO.Amount * 100).ToString());
                requestCollection.Add("includeSignature", readCardRequest.IsSignatureRequired ? "true" : "false");
                requestCollection.Add("includeAVS", readCardRequest.IsZipValidationRequired.ToString());
                requestCollection.Add("capture", readCardRequest.IsCaptureRequired.ToString());
                log.Debug("Creating Web Request");
                webRequest = webRequestHandler.CreateRequest(url, contenttype, method, authorization);
                if (!string.IsNullOrEmpty(sessionKey))
                {
                    webRequestHandler.AddToHeader(webRequest, "X-CardConnect-SessionKey", sessionKey);
                }
                else
                {
                    log.Error("Invalid session.");
                    throw new Exception("Invalid session.");
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
                string data = Json.BuildJsonFromNVC(requestCollection);
                webResponse = webRequestHandler.SendRequest(webRequest, data);
                log.LogMethodExit();
                return webResponse;
            }
            catch (Exception ex)
            {
                log.Fatal("Sendcommand() Exception:" + ex.ToString());
                throw new Exception( ex.Message);
            }
        }
        public override object GetResponse(HttpWebResponse webResponse)
        {
            try
            {
                AuthCardResponse authCardResp = new AuthCardResponse();
                log.LogMethodEntry();
                string data = webRequestHandler.GetJsonData(webResponse);
                responseCollection = Json.BuildNVCFromJson(data);
                if (responseCollection.AllKeys.Contains("errorMessage"))
                {
                    log.Error("ErrorMessage :" + responseCollection["errorMessage"]);
                    throw new Exception(responseCollection["errorMessage"]);
                }
                else
                {
                    authCardResp.TokenId = responseCollection["Token"];
                    authCardResp.ExpDate = responseCollection["expiry"];
                    authCardResp.Name = responseCollection["name"];
                    authCardResp.MerchentId = responseCollection["merchid"];
                    authCardResp.Amount = responseCollection["amount"];
                    authCardResp.Batchid = responseCollection["batchid"];
                    authCardResp.Retref = responseCollection["retref"];
                    authCardResp.Avsresp = responseCollection["avsresp"];
                    authCardResp.Respproc = responseCollection["respproc"];
                    authCardResp.Resptext = responseCollection["resptext"];
                    authCardResp.Authcode = responseCollection["authcode"];
                    authCardResp.Respcode = responseCollection["respcode"];
                    authCardResp.CvvResp = responseCollection["cvvresp"];
                    authCardResp.Respstat = responseCollection["respstat"];
                    log.LogMethodExit(authCardResp);
                    return authCardResp;
                }
            }
            catch (Exception ex)
            {
                log.Fatal("GetResponse() Exception:" + ex.Message);
                return new AuthCardResponse();
            }
        }
        public override CCTransactionsPGWDTO ParseResponse(object response)
        {
            log.LogMethodEntry();
            CCTransactionsPGWDTO ccTransactionsPGWDTO = new CCTransactionsPGWDTO();
            AuthCardResponse authCardResponse = response as AuthCardResponse;
            ccTransactionsPGWDTO.AcctNo = authCardResponse.TokenId;
            ccTransactionsPGWDTO.TextResponse = authCardResponse.Resptext;
            ccTransactionsPGWDTO.TokenID = authCardResponse.TokenId;
            //ccTransactionsPGWDTO.TranCode = authorizationResponse.;
            ccTransactionsPGWDTO.DSIXReturnCode = authCardResponse.Respcode;
            ccTransactionsPGWDTO.Authorize = authCardResponse.Amount;
            //ccTransactionsPGWDTO.ResponseOrigin = authorizationResponse;
            ccTransactionsPGWDTO.RefNo = authCardResponse.Retref;
            ccTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
            ccTransactionsPGWDTO.RecordNo = authCardResponse.Respstat;
            ccTransactionsPGWDTO.ProcessData = authCardResponse.Batchid;
            //ccTransactionsPGWDTO.AcqRefData = "AID:" + authorizationResponse.EmvTagData.AID + "|ARC:" + authorizationResponse.EmvTagData.ARC + "|IAD:"
            //    + authorizationResponse.EmvTagData.IAD + "|TSI:" + authorizationResponse.EmvTagData.TSI + "|TVR:" + authorizationResponse.EmvTagData.TVR;
            log.LogMethodExit();
            return ccTransactionsPGWDTO;
        }
    }
    public class AuthManualCommandHandler : CardConnectDeviceHandler
    {
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public AuthManualCommandHandler(Utilities utilities, TransactionPaymentsDTO transactionPaymentsDTO, string sessionKey, string merchantId, string url, string deviceId, string authorization)
            : base(utilities, transactionPaymentsDTO, sessionKey, merchantId, url, deviceId, authorization)
        {
            log.LogMethodEntry(utilities, transactionPaymentsDTO, sessionKey);
            this.url = url + "authManual";
            method = Methods.POST.ToString();
            this.sessionKey = sessionKey;
            log.LogVariableState("sessionKey", sessionKey);
            log.LogVariableState("method", method);
            log.LogVariableState("url", this.url);
            log.LogMethodExit();
        }

        public override void CreateCommand(object data)
        {
            try
            {
                log.LogMethodEntry(data);
                ReadCardRequest readCardRequest = data as ReadCardRequest;
                requestCollection = new NameValueCollection();
                requestCollection.Add("merchantId", merchantId);
                requestCollection.Add("hsn", deviceId);
                requestCollection.Add("includeAmountDisplay", "true");
                requestCollection.Add("beep", readCardRequest.IsBeepSoundRequired.ToString());
                requestCollection.Add("includePIN", readCardRequest.includePIN ? "true" : "false");
                requestCollection.Add("aid", readCardRequest.aid);
                requestCollection.Add("amount", (transactionPaymentsDTO.Amount * 100).ToString());
                requestCollection.Add("includeSignature", readCardRequest.IsSignatureRequired ? "true" : "false");
                requestCollection.Add("includeAVS", readCardRequest.IsZipValidationRequired.ToString());
                requestCollection.Add("capture", readCardRequest.IsCaptureRequired.ToString());
                log.Debug("Creating Web Request.");
                webRequest = webRequestHandler.CreateRequest(url, contenttype, method, authorization);
                if (!string.IsNullOrEmpty(sessionKey))
                {
                    webRequestHandler.AddToHeader(webRequest, "X-CardConnect-SessionKey", sessionKey);
                }
                else
                {
                    log.Error("Invalid session.");
                    throw new Exception("Invalid session.");
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
                string data = Json.BuildJsonFromNVC(requestCollection);
                webResponse = webRequestHandler.SendRequest(webRequest, data);
                log.LogMethodExit();
                return webResponse;
            }
            catch (Exception ex)
            {
                log.Fatal("Sendcommand() Exception:" + ex.ToString());
                throw new Exception( ex.Message);
            }
        }
        public override object GetResponse(HttpWebResponse webResponse)
        {
            try
            {
                AuthCardResponse authCardResp = new AuthCardResponse();
                log.LogMethodEntry();
                string data = webRequestHandler.GetJsonData(webResponse);
                responseCollection = Json.BuildNVCFromJson(data);
                if (responseCollection.AllKeys.Contains("errorMessage"))
                {
                    throw new Exception(responseCollection["errorMessage"]);
                }
                else
                {
                    authCardResp.TokenId = responseCollection["Token"];
                    authCardResp.ExpDate = responseCollection["expiry"];
                    authCardResp.Name = responseCollection["name"];
                    authCardResp.MerchentId = responseCollection["merchid"];
                    authCardResp.Amount = responseCollection["amount"];
                    authCardResp.Batchid = responseCollection["batchid"];
                    authCardResp.Retref = responseCollection["retref"];
                    authCardResp.Avsresp = responseCollection["avsresp"];
                    authCardResp.Respproc = responseCollection["respproc"];
                    authCardResp.Resptext = responseCollection["resptext"];
                    authCardResp.Authcode = responseCollection["authcode"];
                    authCardResp.Respcode = responseCollection["respcode"];
                    authCardResp.CvvResp = responseCollection["cvvresp"];
                    authCardResp.Respstat = responseCollection["respstat"];
                    log.LogMethodExit(authCardResp);
                    return authCardResp;
                }
            }
            catch (Exception ex)
            {
                log.Fatal("GetResponse() Exception:" + ex.Message);
                return new AuthCardResponse();
            }
        }
        public override CCTransactionsPGWDTO ParseResponse(object response)
        {
            log.LogMethodEntry();
            CCTransactionsPGWDTO ccTransactionsPGWDTO = new CCTransactionsPGWDTO();
            AuthCardResponse authCardResponse = response as AuthCardResponse;
            ccTransactionsPGWDTO.AcctNo = authCardResponse.TokenId;
            ccTransactionsPGWDTO.TextResponse = authCardResponse.Resptext;
            ccTransactionsPGWDTO.TokenID = authCardResponse.TokenId;
            //ccTransactionsPGWDTO.TranCode = authorizationResponse.;
            ccTransactionsPGWDTO.DSIXReturnCode = authCardResponse.Respcode;
            ccTransactionsPGWDTO.Authorize = authCardResponse.Amount;
            //ccTransactionsPGWDTO.ResponseOrigin = authorizationResponse;
            ccTransactionsPGWDTO.RefNo = authCardResponse.Retref;
            ccTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
            ccTransactionsPGWDTO.RecordNo = authCardResponse.Respstat;
            ccTransactionsPGWDTO.ProcessData = authCardResponse.Batchid;
            //ccTransactionsPGWDTO.AcqRefData = "AID:" + authorizationResponse.EmvTagData.AID + "|ARC:" + authorizationResponse.EmvTagData.ARC + "|IAD:"
            //    + authorizationResponse.EmvTagData.IAD + "|TSI:" + authorizationResponse.EmvTagData.TSI + "|TVR:" + authorizationResponse.EmvTagData.TVR;
            log.LogMethodExit();
            return ccTransactionsPGWDTO;
        }
    }
    public class ReadConfirmationCommandHandler : CardConnectDeviceHandler
    {
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ReadConfirmationCommandHandler(Utilities utilities, TransactionPaymentsDTO transactionPaymentsDTO, string sessionKey, string merchantId, string url, string deviceId, string authorization)
            : base(utilities, transactionPaymentsDTO, sessionKey, merchantId, url, deviceId, authorization)
        {
            log.LogMethodEntry(utilities, transactionPaymentsDTO, sessionKey);
            this.url = url + "readConfirmation";
            method = Methods.POST.ToString();
            this.sessionKey = sessionKey;
            log.LogVariableState("sessionKey", sessionKey);
            log.LogVariableState("method", method);
            log.LogVariableState("url", this.url);
            log.LogMethodExit();
        }

        public override void CreateCommand(object data)
        {
            try
            {
                log.LogMethodEntry(data);
                string text="";
                int j = 0;
                int length = data.ToString().Length;
                for (int i = 0; i+23 < length; i = i + 23)
                {
                    text += data.ToString().Substring(i, 23) + "\n";
                    j = i+23;
                }
                if (j < length - 1)
                {
                    text += data.ToString().Substring(j, length - j) + "\n";
                }
                
                requestCollection = new NameValueCollection();
                requestCollection.Add("merchantId", merchantId);
                requestCollection.Add("hsn", deviceId);
                requestCollection.Add("prompt", text);
                log.Debug("Creating Web Request");
                webRequest = webRequestHandler.CreateRequest(url, contenttype, method, authorization);
                if (!string.IsNullOrEmpty(sessionKey))
                {
                    webRequestHandler.AddToHeader(webRequest, "X-CardConnect-SessionKey", sessionKey);
                }
                else
                {
                    log.Error("Invalid session.");
                    throw new Exception("Invalid session.");
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
                string data = Json.BuildJsonFromNVC(requestCollection);
                webResponse = webRequestHandler.SendRequest(webRequest, data);
                log.LogMethodExit();
                return webResponse;
            }
            catch (Exception ex)
            {
                log.Fatal("Sendcommand() Exception:" + ex.ToString());
                throw new Exception( ex.Message);
            }
        }
        public override object GetResponse(HttpWebResponse webResponse)
        {
            try
            {                
                log.LogMethodEntry();
                string result = "";
                string data = webRequestHandler.GetJsonData(webResponse);
                responseCollection = Json.BuildNVCFromJson(data);
                if (responseCollection.AllKeys.Contains("errorMessage"))
                {
                    log.Error("ErrorMessage :" + responseCollection["errorMessage"]);
                    throw new Exception(responseCollection["errorMessage"]);
                }
                else
                {
                    result = responseCollection["confirmed"];
                    log.LogMethodExit(result);
                    return result;
                }
            }
            catch (Exception ex)
            {
                log.Fatal("GetResponse() Exception:" + ex.Message);
                return false;
            }
        }
        
    }
    public class CancelCommandHandler : CardConnectDeviceHandler
    {
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public CancelCommandHandler(Utilities utilities, TransactionPaymentsDTO transactionPaymentsDTO, string sessionKey, string merchantId, string url, string deviceId, string authorization)
            : base(utilities, transactionPaymentsDTO, sessionKey, merchantId, url, deviceId, authorization)
        {
            log.LogMethodEntry(utilities, transactionPaymentsDTO, sessionKey);
            this.url = url + "cancel";
            method = Methods.POST.ToString();
            this.sessionKey = sessionKey;
            log.LogVariableState("sessionKey", sessionKey);
            log.LogVariableState("method", method);
            log.LogVariableState("url", this.url);
            log.LogMethodExit();
        }

        public override void CreateCommand(object data)
        {
            try
            {
                log.LogMethodEntry(data);
                requestCollection = new NameValueCollection();
                requestCollection.Add("merchantId", merchantId);
                requestCollection.Add("hsn", deviceId);
                requestCollection.Add("force", "true");
                log.Debug("Creating Web Request");
                webRequest = webRequestHandler.CreateRequest(url, contenttype, method, authorization);
                if (!string.IsNullOrEmpty(sessionKey))
                {
                    webRequestHandler.AddToHeader(webRequest, "X-CardConnect-SessionKey", sessionKey);
                }
                else
                {
                    log.Error("Invalid session.");
                    throw new Exception("Invalid session.");
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
                string data = Json.BuildJsonFromNVC(requestCollection);
                log.Debug("Sending Web Request");
                webResponse = webRequestHandler.SendRequest(webRequest, data);
                log.LogMethodExit();
                return webResponse;
            }
            catch (Exception ex)
            {
                log.Fatal("Sendcommand() Exception:" + ex.Message);
                return null;
            }
        }
    }
    public class DisconnectCommandHandler : CardConnectDeviceHandler
    {
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public DisconnectCommandHandler(Utilities utilities, TransactionPaymentsDTO transactionPaymentsDTO, string sessionKey, string merchantId, string url, string deviceId, string authorization)
            : base(utilities,  transactionPaymentsDTO, sessionKey, merchantId, url, deviceId, authorization)
        {
            log.LogMethodEntry(utilities, transactionPaymentsDTO, sessionKey);
            this.url = url + "disconnect";
            method = Methods.POST.ToString();
            this.sessionKey = sessionKey;
            log.LogVariableState("sessionKey", sessionKey);
            log.LogVariableState("method", method);
            log.LogVariableState("url", this.url);
            log.LogMethodExit();
        }

        public override void CreateCommand(object data)
        {
            try
            {
                log.LogMethodEntry(data);
                requestCollection = new NameValueCollection();
                requestCollection.Add("merchantId", merchantId);
                requestCollection.Add("hsn", deviceId);
                requestCollection.Add("force", "true");
                log.Debug("Creating Web Request");
                webRequest = webRequestHandler.CreateRequest(url, contenttype, method, authorization);
                if (!string.IsNullOrEmpty(sessionKey))
                {
                    webRequestHandler.AddToHeader(webRequest, "X-CardConnect-SessionKey", sessionKey);
                }
                else
                {
                    log.Error("Invalid session.");
                    throw new Exception("Invalid session.");
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
                string data = Json.BuildJsonFromNVC(requestCollection);
                log.Debug("Sending Web Request");
                webResponse = webRequestHandler.SendRequest(webRequest, data);
                log.LogMethodExit(webResponse);
                return webResponse;
            }
            catch (Exception ex)
            {
                log.Fatal("Sendcommand() Exception:" + ex.Message);
                return null;
            }
        }
    }

}
