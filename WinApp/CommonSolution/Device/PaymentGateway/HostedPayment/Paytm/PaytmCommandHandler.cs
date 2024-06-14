/********************************************************************************************
 * Project Name -  PayTM Hosted Payment Gateway                                                                     
 * Description  -  Class to handle the payment of PayTM Payment Gateway
 *
 **************
 **Version Log
  *Version     Date          Modified By                Remarks          
 *********************************************************************************************
 *2.150.1      8-Mar-2023   Muaaz Musthafa             Created for Website
 ********************************************************************************************/

using Newtonsoft.Json;
using Paytm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Semnox.Parafait.Device.PaymentGateway.HostedPayment.Paytm
{
    public class PaytmCommandHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        string _baseUrl, _merchantid, _merchantkey;
        bool shouldChangeFailure = false;
        public PaytmCommandHandler(string baseurl, string merchantid, string merchantkey)
        {
            log.LogMethodEntry(baseurl, merchantid, merchantkey);

            _baseUrl = baseurl;
            _merchantid = merchantid;
            _merchantkey = merchantkey;

            log.LogMethodExit();
        }
        /// <summary>
        /// Create token to sent to Paytm
        /// </summary>
        /// <param name="initiateTransactionRequestDto"></param>
        /// <returns></returns>
        public CreatePaymentTokenResponseDto CreatePaymentToken(InitiateTransactionRequestDto initiateTransactionRequestDto)
        {
            log.LogMethodEntry(initiateTransactionRequestDto);
            CreatePaymentTokenResponseDto result = null;
            try
            {
                log.Debug($"Initiate Transaction API method Inititated");
                if (initiateTransactionRequestDto == null)
                {
                    log.Error("initiateTransactionRequestDto was null");
                    throw new Exception("Payment failed");
                }
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(_baseUrl + $"/theia/api/v1/initiateTransaction?mid={_merchantid}&orderId={initiateTransactionRequestDto.body.orderId}");

                log.Debug($"OrderId - {initiateTransactionRequestDto.body.orderId}");
                log.Debug($"Amount - {initiateTransactionRequestDto.body.txnAmount.value}");
                log.Debug($"CustomerId - {initiateTransactionRequestDto.body.txnAmount.currency}");
                log.Debug($"Currency - {initiateTransactionRequestDto.body.userInfo.custId}");

                JsonSerializerSettings serializerSettings = GetJsonSerializerSettings();
                string paytmChecksum = Checksum.generateSignature(JsonConvert.SerializeObject(initiateTransactionRequestDto.body, serializerSettings), _merchantkey);

                initiateTransactionRequestDto.head.signature = paytmChecksum;

                string post_data = JsonConvert.SerializeObject(initiateTransactionRequestDto, serializerSettings);

                log.Debug($"Final- Init payment Request - {post_data}");

                webRequest.Method = "POST";
                webRequest.ContentType = "application/json";
                webRequest.ContentLength = post_data.Length;

                using (StreamWriter requestWriter = new StreamWriter(webRequest.GetRequestStream()))
                {
                    requestWriter.Write(post_data);
                }

                using (StreamReader responseReader = new StreamReader(webRequest.GetResponse().GetResponseStream()))
                {
                    string responseData = string.Empty;
                    responseData = responseReader.ReadToEnd();

                    log.Debug($"Init payment responseData={responseData}");

                    result = JsonConvert.DeserializeObject<CreatePaymentTokenResponseDto>(responseData);
                    log.Debug($"Init payment result={JsonConvert.SerializeObject(result)}");

                    // verify checksum
                    if (!VerifyChecksum(responseData))
                    {
                        log.Error("Init payment Checksum Mismatched");
                        throw new Exception("Error: Payment failed");
                    }
                    log.Info("Init payment Checksum Matched");

                    if (result == null || result.body == null)
                    {
                        log.Error("Init payment response was null");
                        throw new Exception("Error: Payment failed");
                    }


                    if (FailureResponseMessage.Select(x => x.code).ToList().Contains(result.body.resultInfo.resultCode))
                    {
                        log.Error($"Init payment Payment Failed with error - {result.body.resultInfo.resultMsg}");
                        throw new Exception("Payment Failed");
                    }

                    log.Info($"Initiate Transaction API method Completed");

                    result.body.customerId = initiateTransactionRequestDto.body.userInfo.custId;
                    result.body.orderId = initiateTransactionRequestDto.body.orderId;
                    result.body.reqAmount = initiateTransactionRequestDto.body.txnAmount.value;
                }

            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            log.LogMethodExit(result);
            return result;
        }

        private bool VerifyChecksum(string responseData)
        {
            log.LogMethodEntry(responseData);
            try
            {
                dynamic responseObj = JsonConvert.DeserializeObject(responseData);

                if (responseObj == null || responseObj.body == null)
                {
                    log.Error("VerifyChecksum() response was null");
                    throw new Exception("Payment failed");
                }

                if (responseObj.head.signature == null)
                {
                    log.Error("VerifyChecksum() response signature was null");
                    throw new Exception("Payment failed");
                }
                var resultBody = responseObj.body;
                string responseSignature = responseObj.head.signature;
                string paytmParams = JsonConvert.SerializeObject(resultBody, new JsonSerializerSettings
                {
                    DefaultValueHandling = DefaultValueHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore,
                    Formatting = Formatting.None,
                });
                log.Debug($"VerifyChecksum() paytmParams={paytmParams}");

                log.LogMethodExit();
                return Checksum.verifySignature(paytmParams, _merchantkey, responseSignature);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }

        }

        /// <summary>
        /// Check the status of the transaction
        /// </summary>
        /// <param name="transactionStatusRequestDto"></param>
        /// <returns></returns>
        public GetPaymentStatusResponseDto GetPaymentStatus(GetPaymentStatusRequestDto transactionStatusRequestDto)
        {
            log.LogMethodEntry(transactionStatusRequestDto);
            GetPaymentStatusResponseDto result = null;
            try
            {
                log.Debug($"Payment Status API method Inititated");
                if (transactionStatusRequestDto == null)
                {
                    log.Error("transactionStatusRequestDto was null");
                    throw new Exception("Payment failed");
                }
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(_baseUrl + $"/v3/order/status");

                log.Debug($"Order Id - {transactionStatusRequestDto.body.orderId}");

                JsonSerializerSettings serializerSettings = GetJsonSerializerSettings();
                string paytmChecksum = Checksum.generateSignature(JsonConvert.SerializeObject(transactionStatusRequestDto.body, serializerSettings), _merchantkey);

                transactionStatusRequestDto.head.signature = paytmChecksum;
                string post_data = JsonConvert.SerializeObject(transactionStatusRequestDto, serializerSettings);

                log.Info($"Request - {post_data}");

                webRequest.Method = "POST";
                webRequest.ContentType = "application/json";
                webRequest.ContentLength = post_data.Length;

                using (StreamWriter requestWriter = new StreamWriter(webRequest.GetRequestStream()))
                {
                    requestWriter.Write(post_data);
                }

                using (StreamReader responseReader = new StreamReader(webRequest.GetResponse().GetResponseStream()))
                {
                    string responseData = string.Empty;
                    responseData = responseReader.ReadToEnd();
                    log.Debug($"GetPaymentStatus() responseData={responseData}");

                    result = JsonConvert.DeserializeObject<GetPaymentStatusResponseDto>(responseData);
                    log.Debug($"GetPaymentStatus() result={JsonConvert.SerializeObject(result)}");

                    // verify checksum
                    if (!VerifyChecksum(responseData))
                    {
                        log.Error("GetPaymentStatus() Checksum Mismatched");
                        throw new Exception("Error: Payment failed");
                    }
                    log.Info("GetPaymentStatus() Checksum Matched");

                    log.Info($"Payment Status API method Completed");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Initiate the refund 
        /// </summary>
        /// <param name="refundRequestDto"></param>
        /// <returns></returns>
        public RefundResponseDto CreateRefund(RefundRequestDto refundRequestDto)
        {
            log.LogMethodEntry(refundRequestDto);
            log.Info($"Refund Payment API method Inititated");

            try
            {
                if (refundRequestDto == null)
                {
                    log.Error("CreateRefund() refundRequestDto was null");
                    throw new Exception("Refund Failed");
                }
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(_baseUrl + $"/refund/apply");

                log.Debug($"Order - {refundRequestDto.body.orderId}");
                log.Debug($"Transaction ID - {refundRequestDto.body.txnId}");
                log.Debug($"Reference ID - {refundRequestDto.body.refId}");
                log.Debug($"Refund Amount - {refundRequestDto.body.refundAmount}");

                JsonSerializerSettings serializerSettings = GetJsonSerializerSettings();
                string paytmChecksum = Checksum.generateSignature(JsonConvert.SerializeObject(refundRequestDto.body, serializerSettings), _merchantkey);

                refundRequestDto.head.signature = paytmChecksum;
                string post_data = JsonConvert.SerializeObject(refundRequestDto, serializerSettings);

                log.Debug($"Final- refund Request - {post_data}");

                webRequest.Method = "POST";
                webRequest.ContentType = "application/json";
                webRequest.ContentLength = post_data.Length;

                using (StreamWriter requestWriter = new StreamWriter(webRequest.GetRequestStream()))
                {
                    requestWriter.Write(post_data);
                }

                using (StreamReader responseReader = new StreamReader(webRequest.GetResponse().GetResponseStream()))
                {
                    string responseData = string.Empty;
                    responseData = responseReader.ReadToEnd();
                    log.Debug($"Refund responseData={responseData}");
                    var result = JsonConvert.DeserializeObject<RefundResponseDto>(responseData);
                    log.Debug($"Refund result={JsonConvert.SerializeObject(result)}");
                    // verify checksum
                    if (!VerifyChecksum(responseData))
                    {
                        log.Error("Refund: Checksum Mismatched");
                        throw new Exception("Error: Payment failed");
                    }
                    log.Info("Refund: Checksum Matched");

                    log.Info($"Refund Payment API method Completed");
                    log.LogMethodExit(result);
                    return result;


                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }

        }

        /// <summary>
        /// Check the refund status
        /// </summary>
        /// <param name="refundStatusRequestDto"></param>
        /// <returns></returns>
        public RefundStatusResponseDto GetRefundStatus(RefundStatusRequestDto refundStatusRequestDto)
        {
            log.LogMethodEntry(refundStatusRequestDto);
            log.Info($"Refund Payment status API method Inititated");
            try
            {
                if (refundStatusRequestDto == null)
                {
                    log.Error("GetRefundStatus() refundStatusRequestDto was null");
                    throw new Exception("Refund Failed");
                }
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(_baseUrl + $"/v2/refund/status");

                log.Debug($"Order - {refundStatusRequestDto.body.orderId}");
                log.Debug($"Reference ID - {refundStatusRequestDto.body.refId}");

                JsonSerializerSettings serializerSettings = GetJsonSerializerSettings();
                string paytmChecksum = Checksum.generateSignature(JsonConvert.SerializeObject(refundStatusRequestDto.body, serializerSettings), _merchantkey);
                refundStatusRequestDto.head.signature = paytmChecksum;

                string post_data = JsonConvert.SerializeObject(refundStatusRequestDto, serializerSettings);

                log.Debug($"GetRefundStatus() Request - {post_data}");

                webRequest.Method = "POST";
                webRequest.ContentType = "application/json";
                webRequest.ContentLength = post_data.Length;

                using (StreamWriter requestWriter = new StreamWriter(webRequest.GetRequestStream()))
                {
                    requestWriter.Write(post_data);
                }

                using (StreamReader responseReader = new StreamReader(webRequest.GetResponse().GetResponseStream()))
                {
                    string responseData = string.Empty;
                    responseData = responseReader.ReadToEnd();
                    log.Debug($"GetRefundStatus() responseData={responseData}");
                    if (string.IsNullOrWhiteSpace(responseData))
                    {
                        log.Error("GetRefundStatus() response was null");
                        throw new Exception("Refund check failed");
                    }

                    var result = JsonConvert.DeserializeObject<RefundStatusResponseDto>(responseData);
                    log.Debug($"GetRefundStatus() result={JsonConvert.SerializeObject(result)}");

                    // verify checksum
                    if (!VerifyChecksum(responseData))
                    {
                        log.Error("GetRefundStatus() Checksum Mismatched");
                        throw new Exception("RefundStatus Error: Payment failed");
                    }
                    log.Info("GetRefundStatus() Checksum Matched");

                    log.Info($"Refund status API method Completed");
                    log.LogMethodExit(result);
                    return result;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
        }

        /// <summary>
        /// Validate the callbackurl response
        /// </summary>
        /// <param name="callBackResponseDto"></param>
        /// <returns></returns>
        public GetPaymentStatusResponseDto ValidateResponse(CallBackResponseDto callBackResponseDto)
        {
            log.LogMethodEntry(callBackResponseDto);
            GetPaymentStatusResponseDto transactionResponse = null;
            try
            {
                if (callBackResponseDto == null)
                {
                    log.Error("Callback response was null");
                    throw new Exception($"Response was null");
                }

                GetPaymentStatusRequestDto requestDto = new GetPaymentStatusRequestDto
                {
                    head = new TransactionStatusRequestHead
                    {
                        signature = ""
                    },
                    body = new TransactionStatusRequestBody
                    {
                        mid = _merchantid,
                        orderId = callBackResponseDto.ORDERID
                    }
                };
                log.Debug($"requestDto={JsonConvert.SerializeObject(requestDto)}");

                transactionResponse = GetPaymentStatus(requestDto);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            log.LogMethodExit(transactionResponse);
            return transactionResponse;
        }

        /// <summary>
        /// Success responses
        /// </summary>
        public static List<PaytmResponseMessage> SuccessResponseMessage
        {
            get
            {
                return ResponseMessage.Where(x => x.isSuccess == true).ToList();
            }
        }

        /// <summary>
        /// Failure responses
        /// </summary>
        public static List<PaytmResponseMessage> FailureResponseMessage
        {
            get
            {
                return ResponseMessage.Where(x => x.isSuccess == false).ToList();
            }
        }

        /// <summary>
        /// Code,Status,Message,isSuccess
        /// </summary>
        private static List<PaytmResponseMessage> ResponseMessage
        {
            get
            {
                var message = new List<PaytmResponseMessage>
                {
                    new PaytmResponseMessage("0000","Success","Success",true), // Success for CreateToken
                    new PaytmResponseMessage("01","TXN_SUCCESS","Txn Success",true),
                    new PaytmResponseMessage("1","TXN_SUCCESS","Txn Success",true),
                    new PaytmResponseMessage("0002","Success","Success Idempotent",true),
                    new PaytmResponseMessage("10","TXN_SUCCESS","Refund successful",true),//sucess for refund
                    new PaytmResponseMessage("601","PENDING","Refund request was raised for this transaction. But it is pending state.",true),//sucess for refund

                    new PaytmResponseMessage("196","F","Payment failed as amount entered exceeds the allowed limit. Please enter a lower amount and try again or reach out to the merchant for further assistance.",false),
                    new PaytmResponseMessage("227","TXN_FAILURE","Your payment has been declined by your bank. Please contact your bank for any queries. If money has been deducted from your account, your bank will inform us within 48 hrs and we will refund the same.",false),
                    new PaytmResponseMessage("235","TXN_FAILURE","Wallet balance Insufficient, bankName=WALLET",false),
                    new PaytmResponseMessage("295","TXN_FAILURE","Your payment failed as the UPI ID entered is incorrect. Please try again by entering a valid VPA or use a different method to complete the payment.",false),
                    new PaytmResponseMessage("330","TXN_FAILURE","Checksum provided is invalid",false),
                    new PaytmResponseMessage("331","NO_RECORD_FOUND","No Record Found",false),
                    new PaytmResponseMessage("334","TXN_FAILURE","Invalid Order ID",false),
                    new PaytmResponseMessage("335","TXN_FAILURE","Mid is invalid",false),
                    new PaytmResponseMessage("400","PENDING","Transaction status not confirmed yet.",false),
                    new PaytmResponseMessage("401","TXN_FAILURE","Your payment has been declined by your bank. Please contact your bank for any queries. If money has been deducted from your account, your bank will inform us within 48 hrs and we will refund the same.",false),//recheck
                    new PaytmResponseMessage("402","PENDING","Looks like the payment is not complete. Please wait while we confirm the status with your bank.",false),
                    new PaytmResponseMessage("501","TXN_FAILURE","Server Down",false),
                    new PaytmResponseMessage("600","TXN_FAILURE","Invalid refund request or restricted by bank.",false),

                    new PaytmResponseMessage("607","TXN_FAILURE","Refund can not be initiated for a cancelled transaction.",false),
                    new PaytmResponseMessage("617","TXN_FAILURE","Refund Already Raised (If merchant repeated their request within the 10 minutes after initiate the first refund request)",false),
                    new PaytmResponseMessage("619","TXN_FAILURE","Invalid refund amount",false),
                    new PaytmResponseMessage("620","TXN_FAILURE","BALANCE_NOT_ENOUGH",false),
                    new PaytmResponseMessage("626","TXN_FAILURE","Another Refund on same order is already in Progress, please retry after few minutes",false),
                    new PaytmResponseMessage("627","TXN_FAILURE","Order Details Mismatch",false),
                    new PaytmResponseMessage("628","TXN_FAILURE","Refund request was raised to respective bank. But it is in pending state from bank side.",false),
                    new PaytmResponseMessage("629","TXN_FAILURE","Refund is already Successful",false),
                    new PaytmResponseMessage("631","TXN_FAILURE","Record not found",false),
                    new PaytmResponseMessage("635","TXN_FAILURE","Partial Refund under Rupee 1 is not allowed",false),
                    new PaytmResponseMessage("810","TXN_FAILURE","Txn Failed",false),
                    new PaytmResponseMessage("900","U","System error",false),
                    new PaytmResponseMessage("1006","F","Your Session has expired",false),
                    new PaytmResponseMessage("1007","F","Missing mandatory element",false),
                    new PaytmResponseMessage("1008","F","Pipe character is not allowed",false),
                    new PaytmResponseMessage("1011","F","Invalid Promo Param",false),
                    new PaytmResponseMessage("1012","F","Promo amount cannot be more than transaction amount",false),
                    new PaytmResponseMessage("2004","F","SSO Token is invalid",false),
                    new PaytmResponseMessage("2005","F","Checksum provided is invalid",false),
                    new PaytmResponseMessage("2007","F","Txn amount is invalid",false),
                    new PaytmResponseMessage("2013","F","Mid in the query param doesn’t match with the Mid sent in the request",false),
                    new PaytmResponseMessage("2014","F","OrderId in the query param doesn’t match with the OrderId sent in the request",false),
                    new PaytmResponseMessage("2023","F","Repeat Request Inconsistent",false)
                };
                return message;
            }
        }

        public string GetMaskedCardNumber(string lastFourDigit)
        {
            string cardNumber = string.Empty;
            try
            {
                if (string.IsNullOrWhiteSpace(lastFourDigit))
                {
                    log.Error("Card number was empty");
                    return cardNumber;
                }

                return "**** **** **** " + lastFourDigit;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
        }

        private static JsonSerializerSettings GetJsonSerializerSettings()
        {
            return new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.None
            };
        }
    }
}
