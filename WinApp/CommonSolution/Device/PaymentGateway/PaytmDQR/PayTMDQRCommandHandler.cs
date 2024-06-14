/******************************************************************************************************
 * Project Name - Device
 * Description  - PayTMDQR Payment gateway
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By    Remarks          
 ******************************************************************************************************
 *2.140.5     11-Jan-2023    Prasad & Fiona   PayTMDQR Payment gateway integration
 ********************************************************************************************************/
using Newtonsoft.Json;
using Paytm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Semnox.Parafait.Device.PaymentGateway
{
    internal class PayTMDQRCommandHandler
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static string _MID, _POS_ID, _MERCHANT_KEY, _HOST_URL, _COMPORT;
        int _BAUD_RATE, _PARITY_BIT, _DATA_BIT, _STOP_BIT, _DEBUG_MODE;


        QrDisplay objds = new QrDisplay();

        const int PAYMENT_PENDING = 402;

        public PayTMDQRCommandHandler(Dictionary<string, string> configurations)
        {
            log.LogMethodEntry(configurations);
            _MID = GetConfiguration("MID", configurations);
            _POS_ID = GetConfiguration("POS_ID", configurations);
            _MERCHANT_KEY = GetConfiguration("MERCHANT_KEY", configurations);
            _HOST_URL = GetConfiguration("HOST_URL", configurations);
            _COMPORT = GetConfiguration("COMPORT", configurations);

            _BAUD_RATE = Convert.ToInt32(GetConfiguration("BAUD_RATE", configurations));
            _PARITY_BIT = Convert.ToInt32(GetConfiguration("PARITY_BIT", configurations));
            _DATA_BIT = Convert.ToInt32(GetConfiguration("DATA_BIT", configurations));
            _STOP_BIT = Convert.ToInt32(GetConfiguration("STOP_BIT", configurations));
            _DEBUG_MODE = Convert.ToInt32(GetConfiguration("DEBUG_MODE", configurations));
            log.LogMethodExit();
            //LoadErrorMessages();
        }

        public bool ShowDeviceHomeScreen()
        {
            log.LogMethodEntry();
            bool res = false;
            try
            {
                res = objds.showHomeScreen(_MID, _COMPORT, 110200, _PARITY_BIT, _DATA_BIT, _STOP_BIT, _DEBUG_MODE, _POS_ID);
            }
            catch (Exception ex)
            {
                log.Error($"Error while displaying Home Screen: {ex}");
                throw;
            }
            log.LogMethodExit();
            return res;
        }

        public bool DownloadHomeScreen()
        {
            bool res = false;
            try
            {
                res = objds.downloadHomeScreen(_MID, _COMPORT, _BAUD_RATE, _PARITY_BIT, _DATA_BIT, _STOP_BIT, _DEBUG_MODE, _POS_ID, _MERCHANT_KEY);
            }
            catch (Exception ex)
            {
                log.Error($"Error in downloadHomeScreen: {ex}");
                throw;
            }
            return res;
        }

        //public bool CheckConnection()
        //{
        //    //bool res = false;
        //    bool result = false;
        //    try
        //    {
        //        //res = DownloadHomeScreen();
        //        result = ShowDeviceHomeScreen();
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(ex);
        //        throw;
        //    }
        //    return result;
        //    //if(result)
        //    //{
        //    //    return result;
        //    //}
        //    //else
        //    //{
        //    //    return false;
        //    //}
        //}

        private static bool CheckInternetConnection()
        {
            log.LogMethodEntry();
            // log.Info("CheckInternetConnection() started");
            WebClient client = new WebClient();
            byte[] recieveddata = null;
            bool internetConnectionStatus = false;
            try
            {
                // Here we are checking the no of bytes recived 
                recieveddata = client.DownloadData("https://www.google.com");
                if (recieveddata != null && recieveddata.Length > 0)
                {
                    // If we recieve data we say connection is active
                    log.Info("Internet connection available");
                    internetConnectionStatus = true;
                }
                else
                {
                    // If we dont recieve data, connection is not active
                    log.Error("Internet not available");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            log.LogMethodExit();
            return internetConnectionStatus;
        }

        //public PayTMDQRResponseDTO CreateQrCode(PayTMDQRRequestDTO requestDTO)
        public PayTMDQRResponseDTO CreateQrCode(PayTMDQRRequestDTO requestDTO)
        {
            log.LogMethodEntry(requestDTO);
            string QR_data = string.Empty;
            string orderId = string.Empty;
            decimal amount;
            PayTMDQRResponseDTO response;
            try
            {
                if (!CheckInternetConnection())
                {
                    log.Error("Internet is not available. Please try again.");
                    throw new Exception("Internet is not available. Please try again.");
                }
                if (requestDTO == null)
                {
                    log.Error("Request params were not provided");
                    throw new Exception("Request params were not provided");
                }
                orderId = requestDTO.orderId;
                amount = requestDTO.transactionAmount;

                Dictionary<string, string> body = new Dictionary<string, string>();
                Dictionary<string, string> head = new Dictionary<string, string>();
                Dictionary<string, Dictionary<string, string>> requestBody = new Dictionary<string, Dictionary<string, string>>();
                if (amount <= 0)
                {
                    log.Error("Amount must be greater than Zero");
                    throw new Exception("Amount must be greater than Zero");
                }

                body.Add("mid", _MID);
                body.Add("orderId", orderId);
                body.Add("amount", amount.ToString());
                body.Add("businessType", "UPI_QR_CODE");
                body.Add("posId", _POS_ID);

                var requestJson = JsonConvert.SerializeObject(body);
                log.Debug("Request body: " + requestJson);

                string paytmChecksum = Paytm.Checksum.generateSignature(JsonConvert.SerializeObject(body), _MERCHANT_KEY);
                log.Debug("Request Checksum: " + paytmChecksum);

                head.Add("clientId", "C11");
                head.Add("version", "V1");
                head.Add("signature", paytmChecksum);

                requestBody.Add("body", body);
                requestBody.Add("head", head);

                string post_data = JsonConvert.SerializeObject(requestBody);
                log.Debug("Request: " + post_data);

                string url = _HOST_URL + "/paymentservices/qr/create";

                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);

                webRequest.Method = "POST";
                webRequest.ContentType = "application/json";
                webRequest.ContentLength = post_data.Length;

                using (StreamWriter requestWriter = new StreamWriter(webRequest.GetRequestStream()))
                {
                    requestWriter.Write(post_data);
                }

                string responseData = string.Empty;

                using (StreamReader responseReader = new StreamReader(webRequest.GetResponse().GetResponseStream()))
                {
                    responseData = responseReader.ReadToEnd();
                    log.Debug("Response: " + responseData);
                }


                if (string.IsNullOrWhiteSpace(responseData))
                {
                    log.Error("Create QR: Response was null");
                    throw new Exception("QR generation failed");
                }
                var responseObj = JsonConvert.DeserializeObject<PayTMDQRResponseDTO>(responseData);
                if (responseObj == null)
                {
                    log.Error("Create QR: Deserialized Response was null");
                    throw new Exception("Error: QR generation failed");
                }

                bool isChecksumVerified = false;

                var responseBody = responseObj.body;
                string responseBody_String = JsonConvert.SerializeObject(responseBody);
                // verify checksum here

                if (isChecksumVerified)// checksum failed
                {
                    log.Error("Create QR: Checksum verification failed");
                    throw new Exception("Invalid Response Received");
                }
                log.Info("Checksum is verified");

                List<string> successReponseCode = SuccessResponseMessage.Select(x => x.Code).ToList();
                log.LogVariableState("successReponseCode", successReponseCode);
                log.LogVariableState("responseObj.body.resultInfo.resultCode", responseObj.body.resultInfo.resultCode);
                if (successReponseCode.Contains(responseObj.body.resultInfo.resultCode))
                {
                    log.Info("QR Code generated");
                    log.LogMethodExit(responseObj);
                    // QR successfully generated
                    return responseObj;
                }
                else
                {
                    // QR Generation failed
                    log.Error("QR Generation failed");
                    throw new Exception("QR Generation failed");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            //log.LogMethodExit(responseObj);
            //return responseOb;
        }

        public PayTMDQRResponseDTO CheckTxStatus(PayTMDQRRequestDTO requestDTO)
        {
            log.LogMethodEntry(requestDTO);

            PayTMDQRResponseDTO responseObj = null;

            try
            {
                if (!CheckInternetConnection())
                {
                    log.Error("Internet is not available. Please try again.");
                    throw new Exception("Internet is not available. Please try again.");
                }
                if (requestDTO == null)
                {
                    log.Error("CheckTxStatus requestDTO was null");
                    throw new Exception("Request params were missing");
                }
                if (string.IsNullOrEmpty(requestDTO.orderId))
                {
                    log.Error("CheckTxStatus Order Id was null");
                    throw new Exception("Order Id was not provided");
                }
                else
                {
                    string orderId = requestDTO.orderId;
                    string API_URL = _HOST_URL + "/v3/order/status";
                    Dictionary<string, string> body = new Dictionary<string, string>();
                    Dictionary<string, string> head = new Dictionary<string, string>();
                    Dictionary<string, Dictionary<string, string>> requestBody = new Dictionary<string, Dictionary<string, string>>();


                    body.Add("mid", _MID);
                    body.Add("orderId", orderId);

                    var requestJson = JsonConvert.SerializeObject(body);
                    log.Debug("CheckTxStatus Request body: " + requestJson);

                    string paytmChecksum = Paytm.Checksum.generateSignature(JsonConvert.SerializeObject(body), _MERCHANT_KEY);
                    log.Debug("CheckTxStatus Request Checksum: " + paytmChecksum);

                    head.Add("signature", paytmChecksum);

                    requestBody.Add("body", body);
                    requestBody.Add("head", head);

                    string post_data = JsonConvert.SerializeObject(requestBody);
                    log.Debug("CheckTxStatus Request: " + post_data);

                    HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(API_URL);

                    webRequest.Method = "POST";
                    webRequest.ContentType = "application/json";
                    webRequest.ContentLength = post_data.Length;

                    using (StreamWriter requestWriter = new StreamWriter(webRequest.GetRequestStream()))
                    {
                        requestWriter.Write(post_data);
                    }

                    string responseData = string.Empty;

                    using (StreamReader responseReader = new StreamReader(webRequest.GetResponse().GetResponseStream()))
                    {
                        responseData = responseReader.ReadToEnd();
                        log.Debug("CheckTxStatus Response: " + responseData);


                        if (string.IsNullOrEmpty(responseData))
                        {
                            log.Error("Get_Payment Response was empty");
                            throw new Exception("Get Payment Response was empty");
                        }
                        responseObj = JsonConvert.DeserializeObject<PayTMDQRResponseDTO>(responseData);
                        // verify checksum

                        var successReponseCode = SuccessResponseMessage.Select(x => x.Code).ToList();
                        if (successReponseCode.Contains(responseObj.body.resultInfo.resultCode))
                        {
                            log.Debug("CheckTxStatus: Previous Payment was successful");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            return responseObj;
        }

        public PayTMDQRResponseDTO RefundAmount(PayTMDQRRequestDTO requestDTO)
        {
            log.LogMethodEntry(requestDTO);
            // orderId is ccRequestId of the sale Tx
            // txnId is Payment Id by Paytm
            // refId is ccRequestId of Refund

            PayTMDQRResponseDTO responseObj = null;
            string refId = String.Empty;
            try
            {
                string API_URL = _HOST_URL + "/refund/apply";
                if (!CheckInternetConnection())
                {
                    log.Error("Internet is not available. Please try again.");
                    throw new Exception("Internet is not available. Please try again.");
                }
                if (requestDTO == null)
                {
                    log.Error("Refund Request params were empty");
                    throw new Exception("Refund Request params were empty");
                }
                if (string.IsNullOrEmpty(requestDTO.orderId) || string.IsNullOrEmpty(requestDTO.paytmPaymentId) || requestDTO.refundAmount <= 0 || string.IsNullOrEmpty(requestDTO.refId))
                {
                    log.Error("Refund Invalid params provided");
                    throw new Exception("Refund: Invalid params provided");
                }
                else
                {
                    string orderId = requestDTO.orderId;
                    string txnId = requestDTO.paytmPaymentId;
                    decimal amount = requestDTO.refundAmount;
                    refId = requestDTO.refId;

                    Dictionary<string, string> body = new Dictionary<string, string>();
                    Dictionary<string, string> head = new Dictionary<string, string>();
                    Dictionary<string, Dictionary<string, string>> requestBody = new Dictionary<string, Dictionary<string, string>>();

                    body.Add("mid", _MID);
                    body.Add("orderId", orderId);
                    body.Add("txnId", txnId);
                    body.Add("txnType", "REFUND");
                    body.Add("refId", refId);
                    body.Add("refundAmount", amount.ToString());

                    var requestJson = JsonConvert.SerializeObject(body);
                    log.Debug("RefundAmount() Request body: " + requestJson);

                    string paytmChecksum = Paytm.Checksum.generateSignature(requestJson, _MERCHANT_KEY);
                    log.Debug("RefundAmount() Request Checksum: " + paytmChecksum);

                    head.Add("signature", paytmChecksum);

                    requestBody.Add("body", body);
                    requestBody.Add("head", head);

                    string post_data = JsonConvert.SerializeObject(requestBody);
                    log.Debug("RefundAmount() Request: " + post_data);

                    HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(API_URL);

                    webRequest.Method = "POST";
                    webRequest.ContentType = "application/json";
                    webRequest.ContentLength = post_data.Length;

                    using (StreamWriter requestWriter = new StreamWriter(webRequest.GetRequestStream()))
                    {
                        requestWriter.Write(post_data);
                    }

                    string responseData = string.Empty;

                    using (StreamReader responseReader = new StreamReader(webRequest.GetResponse().GetResponseStream()))
                    {
                        responseData = responseReader.ReadToEnd();
                        log.Debug("RefundAmount() Response: " + responseData);

                        if (string.IsNullOrEmpty(responseData))
                        {
                            log.Error("Refund Response was empty");
                            throw new Exception("Refund request failed");
                        }
                        // verify checksum

                        responseObj = JsonConvert.DeserializeObject<PayTMDQRResponseDTO>(responseData);
                        log.LogVariableState("Refund responseObj", responseObj);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            log.LogMethodExit(responseObj);
            return responseObj;
        }

        public PayTMDQRResponseDTO CheckRefundStatus(PayTMDQRRequestDTO requestDTO)
        {
            log.LogMethodEntry(requestDTO);

            PayTMDQRResponseDTO responseObj = null;

            try
            {
                if (!CheckInternetConnection())
                {
                    log.Error("Internet is not available. Please try again.");
                    throw new Exception("Internet is not available. Please try again.");
                }
                if (requestDTO == null)
                {
                    log.Error($"Request params were null");
                    throw new Exception("Request params were empty");
                }

                string orderId = requestDTO.orderId; // payment ccRequestId
                string refundId = requestDTO.refId; // refund ccRequestId; REFID from the Refund Request

                if (string.IsNullOrEmpty(orderId) || string.IsNullOrEmpty(refundId))
                {
                    log.Error($"Order Id or Refund Id was null");
                    throw new Exception("Could not get Refund Status. Invalid params provided.");
                }
                else
                {
                    string API_URL = _HOST_URL + "/v2/refund/status";
                    Dictionary<string, string> body = new Dictionary<string, string>();
                    Dictionary<string, string> head = new Dictionary<string, string>();
                    Dictionary<string, Dictionary<string, string>> requestBody = new Dictionary<string, Dictionary<string, string>>();


                    body.Add("mid", _MID);
                    body.Add("orderId", orderId);
                    body.Add("refId", refundId);

                    var requestJson = JsonConvert.SerializeObject(body);
                    log.Debug("Request body: " + requestJson);

                    string paytmChecksum = Paytm.Checksum.generateSignature(JsonConvert.SerializeObject(body), _MERCHANT_KEY);
                    log.Debug("Request Checksum: " + paytmChecksum);

                    head.Add("signature", paytmChecksum);

                    requestBody.Add("body", body);
                    requestBody.Add("head", head);

                    string post_data = JsonConvert.SerializeObject(requestBody);
                    log.Debug("Request: " + post_data);

                    HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(API_URL);

                    webRequest.Method = "POST";
                    webRequest.ContentType = "application/json";
                    webRequest.ContentLength = post_data.Length;

                    using (StreamWriter requestWriter = new StreamWriter(webRequest.GetRequestStream()))
                    {
                        requestWriter.Write(post_data);
                    }

                    string responseData = string.Empty;

                    using (StreamReader responseReader = new StreamReader(webRequest.GetResponse().GetResponseStream()))
                    {
                        responseData = responseReader.ReadToEnd();
                        log.Debug("Response: " + responseData);

                        if (string.IsNullOrEmpty(responseData))
                        {
                            log.Error("Get Refund: Response was null");
                            throw new Exception("Get Refund: Response was empty");
                        }


                        responseObj = JsonConvert.DeserializeObject<PayTMDQRResponseDTO>(responseData);
                        log.LogVariableState("Get Refund responseObj", responseObj);
                        // verify checksum
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            log.LogMethodExit(responseObj);
            return responseObj;
        }

        private bool verifyChecksum(Dictionary<string, string> signatureParams, string API_KEY, string responseChecksum)
        {
            log.LogMethodEntry(signatureParams, API_KEY, responseChecksum);
            try
            {
                log.LogMethodExit();
                return Paytm.Checksum.verifySignature(signatureParams, API_KEY, responseChecksum);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }

        }

        private string GetConfiguration(string key, Dictionary<string, string> configurations)
        {
            log.LogMethodEntry(key, configurations);
            string configValue = "";
            try
            {
                if (configurations.ContainsKey(key))
                {
                    configurations.TryGetValue(key, out configValue);
                }
                else
                {
                    log.Error("Invalid configuration key provided");
                    throw new Exception("Invalid configuration key provided");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            return configValue;
        }


        public static List<ResponseCodesDTO> SuccessResponseMessage
        {
            get
            {
                return ResponseCodesAndMessages.Where(x => x.IsSuccess == true).ToList();
            }
        }

        public static List<ResponseCodesDTO> FailureResponseMessage
        {
            get
            {
                return ResponseCodesAndMessages.Where(x => x.IsSuccess == false).ToList();
            }
        }

        /// <summary>
        /// Code,Status,Message,isSuccess
        /// </summary>
        private static List<ResponseCodesDTO> ResponseCodesAndMessages
        {
            get
            {
                var message = new List<ResponseCodesDTO>
                {
                    new ResponseCodesDTO("QR_0001","SUCCESS","SUCCESS",true),
                     new ResponseCodesDTO("QR_1001","FAILURE","Error while encrypting JSON string", false),
                     new ResponseCodesDTO("QR_1002","FAILURE","Empty MID and MerchantGuid", false),
                     new ResponseCodesDTO("QR_1003","FAILURE","Empty Industry Type", false),
                     new ResponseCodesDTO("QR_1004","FAILURE","Empty Order Id", false),
                     new ResponseCodesDTO("QR_1005","FAILURE","Invalid Amount", false),
                     new ResponseCodesDTO("QR_1006","FAILURE","Empty Product Id", false),
                     new ResponseCodesDTO("QR_1007","FAILURE","Invalid Expiry Date", false),
                     new ResponseCodesDTO("QR_1008","FAILURE","Empty Product Details", false),
                     new ResponseCodesDTO("QR_1009","FAILURE","Empty Product Type", false),
                     new ResponseCodesDTO("QR_1010","FAILURE","Empty Inventory Count", false),
                     new ResponseCodesDTO("QR_1011","FAILURE","Invalid Request Type", false),
                     new ResponseCodesDTO("QR_1012","FAILURE","Invalid Request Type, Id is specified only for Product and Order QR codes", false),
                     new ResponseCodesDTO("QR_1013","FAILURE","Error while parsing date", false),
                     new ResponseCodesDTO("QR_1014","FAILURE","Start Date cannot be after Till Date", false),
                     new ResponseCodesDTO("QR_1015","FAILURE","Both id and date range cannot be null", false),
                     new ResponseCodesDTO("QR_1016","FAILURE","QR Code not found", false),
                     new ResponseCodesDTO("QR_1017","FAILURE","Empty id for searching QR Code", false),
                     new ResponseCodesDTO("QR_1018","FAILURE","Expiry date should be in the future", false),
                     new ResponseCodesDTO("QR_1019","FAILURE","Data already exist with this posid and mid", false),
                     new ResponseCodesDTO("QR_1020","FAILURE","Data already exist for this Merchant with same Id", false),
                     new ResponseCodesDTO("QR_1021","FAILURE","Data already exist with this orderId and mid", false),
                     new ResponseCodesDTO("QR_1022","FAILURE","Data already exist with this productId and mid", false),
                     new ResponseCodesDTO("QR_1023","FAILURE","Unable to find Merchant", false),
                     new ResponseCodesDTO("QR_1024","FAILURE","Not authorized to view QR Code", false),
                     new ResponseCodesDTO("QR_1025","FAILURE","Invalid update request", false),
                     new ResponseCodesDTO("QR_1026","FAILURE","Merchant key not found", false),
                     new ResponseCodesDTO("QR_1027","FAILURE","Empty Order Details", false),
                     new ResponseCodesDTO("QR_1028","FAILURE","Error Occurred while generating QRCode", false),
                     new ResponseCodesDTO("QR_1029","FAILURE","Unable to perform database operation, txn rollbacked", false),
                     new ResponseCodesDTO("QR_1030","FAILURE","Merchant guid and mid given in the request do not belong to same merchant", false),
                     new ResponseCodesDTO("QR_1031","FAILURE","Deeplink should be present in request", false),
                     new ResponseCodesDTO("QR_1032","FAILURE","Empty merchant handle", false),
                     new ResponseCodesDTO("QR_1033","FAILURE","A QR Code with same order id already exists", false),
                     new ResponseCodesDTO("QR_1034","FAILURE","A QR Code with same order id already exists", false),
                     new ResponseCodesDTO("GE_0001","FAILURE","Oops, something went wrong!", false),
                     new ResponseCodesDTO("GE_0003","FAILURE","We could not get the requested details. Please try again.", false),
                     new ResponseCodesDTO("GE_1010","FAILURE","We could not get the requested details. Please try again.", false),
                     new ResponseCodesDTO("GE_1043","FAILURE","Merchant does not exists", false),
                     new ResponseCodesDTO("DQR_0001","FAILURE","Blank request received", false),
                     new ResponseCodesDTO("DQR_0002","FAILURE","Field mid can not be blank", false),
                     new ResponseCodesDTO("DQR_0003","FAILURE","Field displayName can not be blank", false),
                     new ResponseCodesDTO("DQR_0004","FAILURE","Merchant's VPA-address not found", false),
                     new ResponseCodesDTO("DQR_0005","FAILURE","Could not generate UPI-handle", false),
                     new ResponseCodesDTO("DQR_0006","FAILURE","Request param changed with repeated order id", false),
                     new ResponseCodesDTO("DQR_0007","FAILURE","UPI Paymode is not enabled on Merchant", false),
                     new ResponseCodesDTO("DQR_0021","FAILURE","Invalid/Blank type of QrCode", false),
                     new ResponseCodesDTO("DQRO_0001","FAILURE","Error occured while generating order", false),

                     new ResponseCodesDTO("01","TXN_SUCCESS","Txn Success", true),
                     new ResponseCodesDTO("227","TXN_FAILURE","Your payment has been declined by your bank. Please contact your bank for any queries. If money has been deducted from your account, your bank will inform us within 48 hrs and we will refund the same.", false),
                     new ResponseCodesDTO("235","TXN_FAILURE","Wallet balance Insufficient, bankName=WALLET", false),
                     new ResponseCodesDTO("295","TXN_FAILURE","Your payment failed as the UPI ID entered is incorrect. Please try again by entering a valid VPA or use a different method to complete the payment.", false),
                     new ResponseCodesDTO("331","NO_RECORD_FOUND","No Record Found", false),
                     new ResponseCodesDTO("334","TXN_FAILURE","Invalid Order ID", false),
                     new ResponseCodesDTO("335","TXN_FAILURE","Mid is invalid", false),
                     new ResponseCodesDTO("400","PENDING","Transaction status not confirmed yet.", false),
                     new ResponseCodesDTO("401","TXN_FAILURE","Your payment has been declined by your bank. Please contact your bank for any queries. If money has been deducted from your account, your bank will inform us within 48 hrs and we will refund the same.", false),
                     new ResponseCodesDTO("402","PENDING","Looks like the payment is not complete. Please wait while we confirm the status with your bank.", false),
                     new ResponseCodesDTO("501","TXN_FAILURE","Server Down", false),
                     new ResponseCodesDTO("810","TXN_FAILURE","Txn Failed", false),

                     new ResponseCodesDTO("501","PENDING","System Error", false),
                     new ResponseCodesDTO("601","PENDING","Refund request was raised for this transaction. But it is pending state.", true),
                     new ResponseCodesDTO("330","TXN_FAILURE","Checksum provided is invalid", false),
                     new ResponseCodesDTO("335","TXN_FAILURE","Mid is invalid", false),
                     new ResponseCodesDTO("600","TXN_FAILURE","Invalid refund request or restricted by bank.", false),
                     new ResponseCodesDTO("607","TXN_FAILURE","Refund can not be initiated for a cancelled transaction.", false),
                     new ResponseCodesDTO("617","TXN_FAILURE","Refund Already Raised (If merchant repeated their request within the 10 minutes after initiate the first refund request)", false),
                     new ResponseCodesDTO("619","TXN_FAILURE","Invalid refund amount", false),
                     new ResponseCodesDTO("626","TXN_FAILURE","Another Refund on same order is already in Progress, please retry after few minutes", false),
                     new ResponseCodesDTO("627","TXN_FAILURE","Order Details Mismatch", false),
                     new ResponseCodesDTO("628","TXN_FAILURE","Refund request was raised to respective bank. But it is in pending state from bank side.", false),
                     new ResponseCodesDTO("629","TXN_FAILURE","Refund is already Successful", false),
                     new ResponseCodesDTO("635","TXN_FAILURE","Partial Refund under Rupee 1 is not allowed", false),

                     new ResponseCodesDTO("10","TXN_SUCCESS","Refund successful", true),
                     new ResponseCodesDTO("501","PENDING","System error", false),
                     new ResponseCodesDTO("330","TXN_FAILURE","Checksum provided is invalid", false),
                     new ResponseCodesDTO("335","TXN_FAILURE","Mid is invalid", false),
                     new ResponseCodesDTO("600","TXN_FAILURE","Invalid refund request", false),
                     new ResponseCodesDTO("620","TXN_FAILURE","BALANCE_NOT_ENOUGH", false),
                     new ResponseCodesDTO("631","TXN_FAILURE","Record not found", false),

                };

                return message;
            }
        }
    }
}
