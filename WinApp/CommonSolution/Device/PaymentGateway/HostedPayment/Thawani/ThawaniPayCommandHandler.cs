using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace Semnox.Parafait.Device.PaymentGateway.HostedPayment.Thawani
{
    public class ThawaniPayCommandHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        const string PAYMENT_STATUS_PAID = "paid";
        private string _PUBLIC_KEY, _SECRET_KEY, _HOST_URL, _CHECKOUT_URL;

        public ThawaniPayCommandHandler(string PUBLIC_KEY, string SECRET_KEY, string HOST_URL, string CHECKOUT_URL)
        {
            _PUBLIC_KEY = PUBLIC_KEY;
            _SECRET_KEY = SECRET_KEY;
            _HOST_URL = HOST_URL;
            _CHECKOUT_URL = CHECKOUT_URL;
        }

        public CheckoutSessionResponseDto CreateCheckout(CreateCheckoutSessionRequestDto checkoutRequestDTO)
        {
            CheckoutSessionResponseDto checkoutResponseDto = null;
            try
            {
                if (checkoutRequestDTO == null)
                {
                    log.Error($"CreateCheckout(): Request was empty.");
                    throw new Exception("Request was empty.");
                }

                string API_URL = _HOST_URL + "/checkout/session";
                string responseFromServer;

                HttpWebRequest myHttpWebRequest = (HttpWebRequest)HttpWebRequest.Create(API_URL);
                myHttpWebRequest.Method = "POST";


                string json = JsonConvert.SerializeObject(checkoutRequestDTO, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore });
                log.Debug(json);

                byte[] data = Encoding.UTF8.GetBytes(json);

                myHttpWebRequest.ContentType = "application/json";
                myHttpWebRequest.Accept = "application/json";
                myHttpWebRequest.Headers.Add("thawani-api-key", _SECRET_KEY);
                myHttpWebRequest.ContentLength = data.Length;

                Stream requestStream = myHttpWebRequest.GetRequestStream();
                requestStream.Write(data, 0, data.Length);
                requestStream.Close();
                HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                Stream responseStream = myHttpWebResponse.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(responseStream, Encoding.Default);
                responseFromServer = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                responseStream.Close();
                myHttpWebResponse.Close();

                // deserialize the response received
                checkoutResponseDto = JsonConvert.DeserializeObject<CheckoutSessionResponseDto>(responseFromServer);
            }
            catch (Exception ex)
            {
                log.Error($"Exception in CreateCheckout(): {ex}");
                throw;
            }
            return checkoutResponseDto;
        }

        public ThawaniPayResponse GetPaymentByMerchantRequestId(GetCheckoutSessionRequestDto requestDto)
        {
            log.Info("GetPaymentByMerchantRequestId() started");
            log.Debug($"RequestDto: {JsonConvert.SerializeObject(requestDto)}");
            GetPaymentListResponseDto paymentObj = null;
            ThawaniPayResponse paymentResponseObj = new ThawaniPayResponse();

            try
            {
                if (requestDto == null)
                {
                    log.Error($"GetPaymentByMerchantRequestId(): RequestDto was empty.");
                    throw new Exception("GetPaymentByMerchantRequestId(): RequestDto was empty.");
                }

                CheckoutSessionResponseDto checkoutSessionResponse = GetPaymentSession(requestDto);
                if (checkoutSessionResponse == null)
                {
                    log.Error($"GetPaymentByMerchantRequestId(): GetPaymentSession() Response was empty.");
                    throw new Exception("Response was empty when fetching payment session.");
                }

                paymentResponseObj.checkoutSessionResponseDto = checkoutSessionResponse;

                //if (checkoutSessionResponse.data.payment_status == PAYMENT_STATUS_PAID)
                //{
                //payment was paid
                // now fetch payment details
                GetPaymentListRequestDto paymentListRequestDto = new GetPaymentListRequestDto
                {
                    limit = 5,
                    skip = 0,
                    checkout_invoice = checkoutSessionResponse.data.invoice
                };

                log.Debug($"GetPaymentByMerchantRequestId(): paymentListRequestDto:{JsonConvert.SerializeObject(paymentListRequestDto)}");
                GetPaymentListResponseDto paymentListResponseDto = GetPaymentList(paymentListRequestDto);

                if (paymentListResponseDto == null)
                {
                    log.Error($"GetPaymentByMerchantRequestId(): GetPaymentList() Response was empty.");
                    throw new Exception("Response was empty when fetching payments list.");
                }
                else
                {
                    paymentObj = paymentListResponseDto;
                    paymentResponseObj.paymentListResponseDto = paymentObj;
                }
                //}
            }
            catch (Exception ex)
            {
                log.Error($"GetPaymentByMerchantRequestId(): {ex}");
                throw;
            }

            return paymentResponseObj;
        }

        //public GetPaymentListResponseDto GetPaymentByMerchantRequestId(GetCheckoutSessionRequestDto requestDto)
        //{
        //    log.Info("GetPaymentByMerchantRequestId() started");
        //    log.Debug($"RequestDto: {JsonConvert.SerializeObject(requestDto)}");
        //    GetPaymentListResponseDto paymentObj = null;
        //    try
        //    {
        //        if (requestDto == null)
        //        {
        //            log.Error($"GetPaymentByMerchantRequestId(): RequestDto was empty.");
        //            throw new Exception("GetPaymentByMerchantRequestId(): RequestDto was empty.");
        //        }

        //        CheckoutSessionResponseDto checkoutSessionResponse = GetPaymentSession(requestDto);
        //        if (checkoutSessionResponse == null)
        //        {
        //            log.Error($"GetPaymentByMerchantRequestId(): GetPaymentSession() Response was empty.");
        //            throw new Exception("Response was empty when fetching payment session.");
        //        }

        //        if (checkoutSessionResponse.data.payment_status == PAYMENT_STATUS_PAID)
        //        {
        //            //payment was paid
        //            // now fetch payment details
        //            GetPaymentListRequestDto paymentListRequestDto = new GetPaymentListRequestDto
        //            {
        //                limit = 5,
        //                skip = 0,
        //                checkout_invoice = checkoutSessionResponse.data.invoice
        //            };

        //            log.Debug($"GetPaymentByMerchantRequestId(): paymentListRequestDto:{JsonConvert.SerializeObject(paymentListRequestDto)}");
        //            GetPaymentListResponseDto paymentListResponseDto = GetPaymentList(paymentListRequestDto);

        //            if (paymentListResponseDto == null)
        //            {
        //                log.Error($"GetPaymentByMerchantRequestId(): GetPaymentList() Response was empty.");
        //                throw new Exception("Response was empty when fetching payments list.");
        //            }
        //            else
        //            {
        //                paymentObj = paymentListResponseDto;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error($"GetPaymentByMerchantRequestId(): {ex}");
        //        throw;
        //    }

        //    return paymentObj;
        //}
        public CheckoutSessionResponseDto GetPaymentSession(GetCheckoutSessionRequestDto requestDto)
        {
            CheckoutSessionResponseDto checkoutResponseDto = null;
            try
            {
                log.Debug($"GetPaymentSession() RequestDto: {JsonConvert.SerializeObject(requestDto)}");
                if (requestDto == null)
                {
                    log.Error("GetPaymentSession() Request was empty.");
                    throw new Exception("Get Payment Request was empty. Please try again.");
                }

                if (string.IsNullOrWhiteSpace(requestDto.client_reference_id))
                {
                    log.Error("GetPaymentSession() Payment RRN was empty.");
                    throw new Exception("Payment RRN was empty. Please try again.");
                }

                string API_URL = _HOST_URL + "/checkout/reference/" + requestDto.client_reference_id;
                string responseFromServer;
                string statusFromServer;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(API_URL);
                request.Method = "GET";
                request.ContentType = "application/json";
                request.Accept = "application/json";
                request.Headers.Add("thawani-api-key", _SECRET_KEY);

                HttpWebResponse webResponse = (HttpWebResponse)request.GetResponse();
                statusFromServer = ((HttpWebResponse)webResponse).StatusDescription;
                Stream receiveStream = webResponse.GetResponseStream();
                StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                responseFromServer = readStream.ReadToEnd();

                webResponse.Close();
                readStream.Close();
                log.Debug($"GetPaymentSession(): Response {responseFromServer}");
                // deserialize the response received
                if (string.IsNullOrWhiteSpace(responseFromServer))
                {
                    log.Error("GetPaymentSession(): Response was empty.");
                }
                checkoutResponseDto = JsonConvert.DeserializeObject<CheckoutSessionResponseDto>(responseFromServer);
                log.Debug($"checkoutResponseDto: {JsonConvert.SerializeObject(checkoutResponseDto)}");
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            return checkoutResponseDto;
        }

        public GetPaymentListResponseDto GetPaymentList(GetPaymentListRequestDto requestDto)
        {
            GetPaymentListResponseDto checkoutResponseDto = null;
            try
            {
                log.Debug($"GetPaymentList() RequestDto: {JsonConvert.SerializeObject(requestDto)}");
                if (requestDto == null)
                {
                    log.Error("GetPaymentSession() Request was empty.");
                    throw new Exception("Error occurred while processing your payment");
                }

                if (requestDto.limit <= 0 || requestDto.skip < 0 || string.IsNullOrWhiteSpace(requestDto.checkout_invoice))
                {
                    log.Error($"GetPaymentList() Input Param was empty. Params received were: requestDto.limit:{requestDto.limit}, requestDto.skip:{requestDto.skip}, requestDto.checkout_invoice:{requestDto.checkout_invoice}");
                    throw new Exception("Insufficient Params Provided");
                }

                //if (string.IsNullOrWhiteSpace(configurations._host_url) || string.IsNullOrWhiteSpace(configurations._secret_key))
                //{
                //    log.Error($"GetPaymentList() Configurations were empty. Params received were: configurations._secret_key:{configurations._secret_key}, configurations._host_url:{configurations._host_url}");
                //    throw new Exception("Configurations were empty.");
                //}


                string API_URL = _HOST_URL + $"/payments?limit={requestDto.limit}&skip={requestDto.skip}&checkout_invoice={requestDto.checkout_invoice}";
                string responseFromServer;
                string statusFromServer;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(API_URL);
                request.Method = "GET";
                request.ContentType = "application/json";
                request.Accept = "application/json";
                request.Headers.Add("thawani-api-key", _SECRET_KEY);

                HttpWebResponse webResponse = (HttpWebResponse)request.GetResponse();
                statusFromServer = ((HttpWebResponse)webResponse).StatusDescription;
                Stream receiveStream = webResponse.GetResponseStream();
                StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                responseFromServer = readStream.ReadToEnd();

                webResponse.Close();
                readStream.Close();
                log.Debug($"GetPaymentSession(): Response {responseFromServer}");
                // deserialize the response received
                if (string.IsNullOrWhiteSpace(responseFromServer))
                {
                    log.Error("GetPaymentSession(): Response was empty.");
                }
                checkoutResponseDto = JsonConvert.DeserializeObject<GetPaymentListResponseDto>(responseFromServer);
                log.Debug($"checkoutResponseDto: {JsonConvert.SerializeObject(checkoutResponseDto)}");
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            return checkoutResponseDto;
        }

        public RefundResponseDto CreateRefund(CreateRefundRequestDto refundRequestDto)
        {
            RefundResponseDto refundResponseDto = null;
            try
            {
                if (refundRequestDto == null)
                {
                    log.Error($"CreateRefund(): Request was empty.");
                    throw new Exception("Request was empty.");
                }

                //if (string.IsNullOrWhiteSpace(configurations._secret_key)
                //    || string.IsNullOrWhiteSpace(configurations._host_url))
                //{
                //    log.Error($"CreateRefund(): Configurations was empty.");
                //    throw new Exception("Configurations was empty.");
                //}

                string API_URL = _HOST_URL + "/refunds";
                string responseFromServer;

                HttpWebRequest myHttpWebRequest = (HttpWebRequest)HttpWebRequest.Create(API_URL);
                myHttpWebRequest.Method = "POST";


                string json = JsonConvert.SerializeObject(refundRequestDto, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore });
                log.Debug(json);

                byte[] data = Encoding.UTF8.GetBytes(json);

                myHttpWebRequest.ContentType = "application/json";
                myHttpWebRequest.Accept = "application/json";
                myHttpWebRequest.Headers.Add("thawani-api-key", _SECRET_KEY);
                myHttpWebRequest.ContentLength = data.Length;

                Stream requestStream = myHttpWebRequest.GetRequestStream();
                requestStream.Write(data, 0, data.Length);
                requestStream.Close();
                HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                Stream responseStream = myHttpWebResponse.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(responseStream, Encoding.Default);
                responseFromServer = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                responseStream.Close();
                myHttpWebResponse.Close();

                // deserialize the response received
                refundResponseDto = JsonConvert.DeserializeObject<RefundResponseDto>(responseFromServer);
            }
            catch (Exception ex)
            {
                log.Error($"Exception in CreateRefund(): {ex}");
                throw;
            }
            return refundResponseDto;
        }

        public string PrepareGatewayRequestString(string checkoutURL)
        {
            StringBuilder strBuilder = new StringBuilder();

            strBuilder.Append("<html>");
            strBuilder.Append("<body onload=\"loadPayment()\">");
            strBuilder.Append("<script>function loadPayment() { ");
            strBuilder.Append($"window.location.replace(\"{checkoutURL}\");");
            strBuilder.Append("}</script>");

            strBuilder.Append("</body></html>");

            return strBuilder.ToString();
        }


    }
}