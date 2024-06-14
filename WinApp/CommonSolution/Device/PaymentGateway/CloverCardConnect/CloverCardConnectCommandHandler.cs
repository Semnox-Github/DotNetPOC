using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Device.PaymentGateway
{
    public class CloverCardConnectCommandHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //private string baseUrl = "https://sandbox.dev.clover.com/connect/v1/";
        private string baseUrl;
        private string authToken;

        public CloverCardConnectCommandHandler(string gatewayHostUrl, string authToken)
        {
            baseUrl = gatewayHostUrl;
            this.authToken = authToken;
        }
        public string CreateIdempotencyKey()
        {
            string idempotencyKey = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 13);
            return idempotencyKey;
        }

        private string CreateAuthToken()
        {
            //string authToken = "Bearer 0f1734ba-40e6-34b3-d214-88d754749774";
            return authToken;
        }

        public int CancelCurrentOpeartion(string cloverDeviceId, string posId)
        {
            string responseFromServer;
            string statusFromServer;
            try
            {
                WebRequest request = WebRequest.Create(baseUrl + "device/cancel");
                request.Method = "POST";

                object requestObject = new CancelCurrentOperationDTO { };
                string jsonRequest = JsonConvert.SerializeObject(requestObject, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                byte[] byteArray = Encoding.UTF8.GetBytes(jsonRequest);

                request.ContentType = "application/json";
                request.Headers.Add("X-Clover-Device-Id", cloverDeviceId);
                request.Headers.Add("X-POS-ID", posId);
                request.Headers.Add("Authorization", CreateAuthToken());

                request.ContentLength = byteArray.Length;

                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                WebResponse response = request.GetResponse();
                statusFromServer = ((HttpWebResponse)response).StatusDescription;

                using (dataStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(dataStream);
                    responseFromServer = reader.ReadToEnd();
                }

                response.Close();

                if (statusFromServer == "OK")
                {
                    return 1;
                }
                else
                {
                    return 0;
                }

            }
            catch (Exception ex)
            {
                return 0;
            }

        }

        public int CheckConnection(string cloverDeviceId, string posId)
        {
            log.LogMethodEntry(cloverDeviceId, posId);
            string responseFromServer;
            string statusFromServer;
            try
            {
                WebRequest request = WebRequest.Create(baseUrl + "device/ping");
                request.Method = "POST";

                object requestObject = new CheckConnecionDTO { };
                string jsonRequest = JsonConvert.SerializeObject(requestObject, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                byte[] byteArray = Encoding.UTF8.GetBytes(jsonRequest);

                request.ContentType = "application/json";
                request.Headers.Add("X-Clover-Device-Id", cloverDeviceId);
                request.Headers.Add("X-POS-ID", posId);
                request.Headers.Add("Authorization", CreateAuthToken());
                request.ContentLength = byteArray.Length;

                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                WebResponse response = request.GetResponse();
                statusFromServer = (((HttpWebResponse)response).StatusDescription);

                using (dataStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(dataStream);
                    responseFromServer = reader.ReadToEnd();
                    Console.WriteLine(responseFromServer);
                }

                response.Close();

                JObject obj = JObject.Parse(responseFromServer);
                if (obj.Value<string>("connected") == "True")
                {
                    log.LogMethodExit(1);
                    return 1;
                }
                else
                {
                    log.LogMethodExit(0);
                    return 0;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return 0;
            }

        }

        public string DisplayWelcomeScreen(string cloverDeviceId, string posId)
        {
            log.LogMethodEntry(cloverDeviceId, posId);
            string responseFromServer;
            string statusFromServer;
            try
            {
                WebRequest request = WebRequest.Create(baseUrl + "device/welcome");
                request.Method = "POST";

                object requestObject = new WelcomeMessageDTO { };
                string jsonRequest = JsonConvert.SerializeObject(requestObject, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                byte[] byteArray = Encoding.UTF8.GetBytes(jsonRequest);

                request.ContentType = "application/json";
                request.Headers.Add("X-Clover-Device-Id", cloverDeviceId);
                request.Headers.Add("X-POS-ID", posId);
                request.Headers.Add("Authorization", CreateAuthToken());

                request.ContentLength = byteArray.Length;

                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                WebResponse response = request.GetResponse();

                statusFromServer = ((HttpWebResponse)response).StatusDescription;

                using (dataStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(dataStream);
                    responseFromServer = reader.ReadToEnd();
                }

                response.Close();
                log.LogMethodExit(statusFromServer);
                return statusFromServer;

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(ex.Message);
                return "Exception";
            }
        }

        public string GetPaymentByExternalPaymentId(string externalPaymentId, string cloverDeviceId, string posId)
        {
            log.LogMethodEntry(externalPaymentId, cloverDeviceId, posId);
            string responseFromServer;
            string statusFromServer;

            try
            {
                WebRequest request = WebRequest.Create(baseUrl + "payments/external/" + externalPaymentId);

                request.Headers.Add("X-Clover-Device-Id", cloverDeviceId);
                request.Headers.Add("X-POS-ID", posId);
                request.Headers.Add("Authorization", CreateAuthToken());

                WebResponse response = request.GetResponse();
                statusFromServer = (((HttpWebResponse)response).StatusDescription);

                using (Stream dataStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(dataStream);
                    responseFromServer = reader.ReadToEnd();
                }

                response.Close();
                log.LogMethodExit(responseFromServer);
                return responseFromServer;

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(ex.Message);
                return "Exception";
            }

        }

        public string GetPaymentByPaymentId(string paymentId, string cloverDeviceId, string posId)
        {
            log.LogMethodEntry(paymentId, cloverDeviceId, posId);
            string responseFromServer;
            string statusFromServer;

            try
            {
                WebRequest request = WebRequest.Create(baseUrl + "payments/" + paymentId);

                request.Headers.Add("X-Clover-Device-Id", cloverDeviceId);
                request.Headers.Add("X-POS-ID", posId);
                request.Headers.Add("Authorization", CreateAuthToken());

                WebResponse response = request.GetResponse();
                statusFromServer = ((HttpWebResponse)response).StatusDescription;

                using (Stream dataStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(dataStream);
                    responseFromServer = reader.ReadToEnd();

                }

                response.Close();
                log.LogMethodExit(responseFromServer);
                return responseFromServer;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(ex.Message);
                return "Exception";

            }
        }

        public string CreateReadTip(double amount, string cloverDeviceId, string posId)
        {
            try
            {
                string responseFromServer;
                WebRequest request = WebRequest.Create(baseUrl + "device/read-tip");
                request.Method = "POST";

                var listTipSuggestions = new List<TipSuggestionsDTO>
                {
                    new TipSuggestionsDTO { name = "One dollar", amount = 100 },
                    new TipSuggestionsDTO { name = "Five dollars", amount = 500 },
                    new TipSuggestionsDTO { name = "Fifteen percent", percentage = 15 },
                    new TipSuggestionsDTO { name = "Twenty percent", percentage = 20 },
                };

                var requestObject = new TipRequestDTO
                {
                    baseAmount = amount,
                    tipSuggestions = listTipSuggestions

                };
                string jsonRequest = JsonConvert.SerializeObject(requestObject, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                byte[] byteArray = Encoding.UTF8.GetBytes(jsonRequest);

                request.ContentType = "application/json";
                request.Headers.Add("X-Clover-Device-Id", cloverDeviceId);
                request.Headers.Add("X-POS-ID", posId);
                request.Headers.Add("Idempotency-Key", CreateIdempotencyKey());
                request.Headers.Add("Authorization", CreateAuthToken());


                request.ContentLength = byteArray.Length;

                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                WebResponse response = request.GetResponse();
                Console.WriteLine(((HttpWebResponse)response).StatusDescription);

                using (dataStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(dataStream);
                    responseFromServer = reader.ReadToEnd();
                    Console.WriteLine(responseFromServer);
                }

                response.Close();
                log.LogMethodExit(responseFromServer);
                return responseFromServer;

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(ex.Message);
                return "Exception";
            }
        }

        public string CreateDirectSale(double amount, string cloverDeviceId, string posId, string externalPaymentId)
        {
            log.LogMethodEntry(amount, cloverDeviceId, posId, externalPaymentId);
            string responseFromServer;
            string statusFromServer;

            try
            {
                WebRequest request = WebRequest.Create(baseUrl + "payments");
                request.Method = "POST";

                object requestObject = new PaymentRequestDTO
                {
                    amount = amount,
                    capture = true,
                    final = true,
                    externalPaymentId = externalPaymentId,
                };

                string jsonRequest = JsonConvert.SerializeObject(requestObject, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                byte[] byteArray = Encoding.UTF8.GetBytes(jsonRequest);

                request.ContentType = "application/json";
                request.Headers.Add("X-Clover-Device-Id", cloverDeviceId);
                request.Headers.Add("X-POS-ID", posId);
                request.Headers.Add("Idempotency-Key", CreateIdempotencyKey());
                request.Headers.Add("Authorization", CreateAuthToken());
                request.ContentLength = byteArray.Length;

                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                WebResponse response = request.GetResponse();
                statusFromServer = ((HttpWebResponse)response).StatusDescription;

                using (dataStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(dataStream);
                    responseFromServer = reader.ReadToEnd();
                }

                response.Close();
                dynamic responseObject = JsonConvert.DeserializeObject(responseFromServer);

                if (responseObject.code != null)
                {
                    throw new Exception(responseObject.message.ToString());
                }
                log.LogMethodExit(responseFromServer);
                return responseFromServer;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public string CreatePaymentWithEmailedReceipt(double amount, string cloverDeviceId, string posId, string externalPaymentId, string emailAddress)
        {
            log.LogMethodEntry(amount, cloverDeviceId, posId, externalPaymentId);
            string responseFromServer;
            string statusFromServer;

            try
            {
                WebRequest request = WebRequest.Create(baseUrl + "payments");
                request.Method = "POST";

                object requestObject = new PaymentRequestDTO
                {
                    amount = amount,
                    receipt_email = emailAddress,
                    externalPaymentId = externalPaymentId,
                };

                string jsonRequest = JsonConvert.SerializeObject(requestObject, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                byte[] byteArray = Encoding.UTF8.GetBytes(jsonRequest);

                request.ContentType = "application/json";
                request.Headers.Add("X-Clover-Device-Id", cloverDeviceId);
                request.Headers.Add("X-POS-ID", posId);
                request.Headers.Add("Idempotency-Key", CreateIdempotencyKey());
                request.Headers.Add("Authorization", CreateAuthToken());
                request.ContentLength = byteArray.Length;

                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                WebResponse response = request.GetResponse();
                statusFromServer = ((HttpWebResponse)response).StatusDescription;

                using (dataStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(dataStream);
                    responseFromServer = reader.ReadToEnd();
                }

                response.Close();
                log.LogMethodExit(responseFromServer);
                return responseFromServer;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(ex.Message);
                return "Exception";
            }
        }

        public string CreateFullRefund(string paymentId, string cloverDevideId, string posId)
        {
            string responseFromServer;
            string statusFromServer;

            try
            {
                WebRequest request = WebRequest.Create(baseUrl + "payments/" + paymentId + "/refunds");
                request.Method = "POST";

                object requestObject = new RefundRequestDTO
                {
                    fullRefund = true
                };
                string jsonRequest = JsonConvert.SerializeObject(requestObject, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                byte[] byteArray = Encoding.UTF8.GetBytes(jsonRequest);

                request.ContentType = "application/json";
                request.Headers.Add("X-Clover-Device-Id", cloverDevideId);
                request.Headers.Add("X-POS-ID", posId);
                request.Headers.Add("Idempotency-Key", CreateIdempotencyKey());
                request.Headers.Add("Authorization", CreateAuthToken());
                request.ContentLength = byteArray.Length;

                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                statusFromServer = (((HttpWebResponse)response).StatusDescription);
                using (dataStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(dataStream);
                    responseFromServer = reader.ReadToEnd();
                }

                response.Close();
                return responseFromServer;

            }
            catch (Exception ex)
            {
                return "Exception";
            }
        }

        public string CreatePartialRefund(string paymentId, double amount, string cloverDevideId, string posId)
        {
            log.LogMethodEntry(paymentId, amount, cloverDevideId, posId);
            string responseFromServer;
            string statusFromServer;

            try
            {
                WebRequest request = WebRequest.Create(baseUrl + paymentId + "/refunds");
                request.Method = "POST";

                object requestObject = new RefundRequestDTO
                {
                    amount = amount
                };
                string jsonRequest = JsonConvert.SerializeObject(requestObject, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                byte[] byteArray = Encoding.UTF8.GetBytes(jsonRequest);

                request.ContentType = "application/json";
                request.Headers.Add("X-Clover-Device-Id", cloverDevideId);
                request.Headers.Add("X-POS-ID", posId);
                request.Headers.Add("Idempotency-Key", CreateIdempotencyKey());
                request.Headers.Add("Authorization", CreateAuthToken());
                request.ContentLength = byteArray.Length;

                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                statusFromServer = (((HttpWebResponse)response).StatusDescription);
                using (dataStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(dataStream);
                    responseFromServer = reader.ReadToEnd();
                }

                response.Close();
                log.LogMethodExit(responseFromServer);
                return responseFromServer;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(ex.Message);
                return "Exception";
            }
        }

        public string CreateVoid(string paymentId, string cloverDevideId, string posId)
        {
            log.LogMethodEntry(paymentId, cloverDevideId, posId);
            string responseFromServer;
            string statusFromServer;

            try
            {
                WebRequest request = WebRequest.Create(baseUrl + "payments/" + paymentId + "/void");
                request.Method = "POST";

                object requestObject = new VoidRequestDTO
                {
                    voidReason = "USER_CANCEL"
                };
                string jsonRequest = JsonConvert.SerializeObject(requestObject, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                byte[] byteArray = Encoding.UTF8.GetBytes(jsonRequest);

                request.ContentType = "application/json";
                request.Headers.Add("X-Clover-Device-Id", cloverDevideId);
                request.Headers.Add("X-POS-ID", posId);
                request.Headers.Add("Idempotency-Key", CreateIdempotencyKey());
                request.Headers.Add("Authorization", CreateAuthToken());
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                statusFromServer = (((HttpWebResponse)response).StatusDescription);
                using (dataStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(dataStream);
                    responseFromServer = reader.ReadToEnd();
                    Console.WriteLine(responseFromServer);
                }

                response.Close();
                log.LogMethodExit(responseFromServer);
                return responseFromServer;

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(ex.Message);
                return "Exception";
            }
        }

        public string CreateDirectAuth(double amount, string cloverDevideId, string posId, string externalPaymentId)
        {
            log.LogMethodEntry(amount, cloverDevideId, posId, externalPaymentId);
            try
            {
                string responseFromServer;
                string statusFromServer;

                WebRequest request = WebRequest.Create(baseUrl + "payments");
                request.Method = "POST";

                object requestObject = new PaymentRequestDTO
                {
                    amount = amount,
                    capture = false,
                    externalPaymentId = externalPaymentId,

                };
                string jsonRequest = JsonConvert.SerializeObject(requestObject, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                byte[] byteArray = Encoding.UTF8.GetBytes(jsonRequest);

                request.ContentType = "application/json";
                request.Headers.Add("X-Clover-Device-Id", cloverDevideId);
                request.Headers.Add("X-POS-ID", posId);
                request.Headers.Add("Idempotency-Key", CreateIdempotencyKey());
                request.Headers.Add("Authorization", CreateAuthToken());
                request.ContentLength = byteArray.Length;

                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                statusFromServer = ((HttpWebResponse)response).StatusDescription;

                using (dataStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(dataStream);
                    responseFromServer = reader.ReadToEnd();
                }
                log.LogVariableState("responseFromServer", responseFromServer);
                response.Close();
                dynamic responseObject = JsonConvert.DeserializeObject(responseFromServer);

                if (responseObject.code != null)
                {
                    throw new Exception(responseObject.message.ToString());
                }
                log.LogMethodExit(responseFromServer);
                return responseFromServer;

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(ex.Message);
                throw new Exception(ex.Message);
            }
        }
        public string CreateAuth(double amount, string cloverDevideId, string posId, string PaymentId)
        {
            log.LogMethodEntry(amount, cloverDevideId, posId, PaymentId);
            try
            {
                string responseFromServer;
                string statusFromServer;

                WebRequest request = WebRequest.Create(baseUrl + "payments/" + PaymentId + "/increment");
                request.Method = "POST";

                object requestObject = new IncrementalAuthRequestDTO
                {
                    amount = amount
                };
                string jsonRequest = JsonConvert.SerializeObject(requestObject, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                byte[] byteArray = Encoding.UTF8.GetBytes(jsonRequest);

                request.ContentType = "application/json";
                request.Headers.Add("X-Clover-Device-Id", cloverDevideId);
                request.Headers.Add("X-POS-ID", posId);
                request.Headers.Add("Idempotency-Key", CreateIdempotencyKey());
                request.Headers.Add("Authorization", CreateAuthToken());
                request.ContentLength = byteArray.Length;

                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                statusFromServer = ((HttpWebResponse)response).StatusDescription;

                using (dataStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(dataStream);
                    responseFromServer = reader.ReadToEnd();
                }
                log.LogVariableState("responseFromServer", responseFromServer);
                response.Close();
                dynamic responseObject = JsonConvert.DeserializeObject(responseFromServer);

                if (responseObject.code != null)
                {
                    throw new Exception(responseObject.message.ToString());
                }
                log.LogMethodExit(responseFromServer);
                return responseFromServer;

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public string CreatePreAuth(double amount, string cloverDevideId, string posId, string externalPaymentId)
        {
            log.LogMethodEntry(amount, cloverDevideId, posId, externalPaymentId);
            try
            {
                string responseFromServer;
                string statusFromServer;

                WebRequest request = WebRequest.Create(baseUrl + "payments");
                request.Method = "POST";

                object requestObject = new PaymentRequestDTO
                {
                    amount = amount,
                    capture = false,
                    externalPaymentId = externalPaymentId,
                };

                string jsonRequest = JsonConvert.SerializeObject(requestObject, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                byte[] byteArray = Encoding.UTF8.GetBytes(jsonRequest);

                request.ContentType = "application/json";
                request.Headers.Add("X-Clover-Device-Id", cloverDevideId);
                request.Headers.Add("X-POS-ID", posId);
                request.Headers.Add("Idempotency-Key", CreateIdempotencyKey());
                request.Headers.Add("Authorization", CreateAuthToken());
                request.ContentLength = byteArray.Length;

                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                statusFromServer = ((HttpWebResponse)response).StatusDescription;

                using (dataStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(dataStream);
                    responseFromServer = reader.ReadToEnd();
                }

                response.Close();
                log.LogMethodExit(responseFromServer);
                return responseFromServer;

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public string CreateCapture(string paymentId, double amount, string cloverDevideId, string posId, double tip = 0)
        {
            log.LogMethodEntry(paymentId, amount, cloverDevideId, posId, tip);
            string statusFromServer;
            string responseFromServer;

            try
            {
                WebRequest request = WebRequest.Create(baseUrl + "payments/" + paymentId + "/capture");
                request.Method = "POST";

                object captureObject = new CaptureRequestDTO
                {
                    amount = amount,
                    tipAmount = tip,
                };

                string json = JsonConvert.SerializeObject(captureObject, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                byte[] byteArray = Encoding.UTF8.GetBytes(json);

                request.ContentType = "application/json";
                request.Headers.Add("X-Clover-Device-Id", cloverDevideId);
                request.Headers.Add("X-POS-ID", posId);
                request.Headers.Add("Idempotency-Key", CreateIdempotencyKey());
                request.Headers.Add("Authorization", CreateAuthToken());
                request.ContentLength = byteArray.Length;

                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                statusFromServer = (((HttpWebResponse)response).StatusDescription);

                using (dataStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(dataStream);
                    responseFromServer = reader.ReadToEnd();
                }

                response.Close();
                dynamic responseObject = JsonConvert.DeserializeObject(responseFromServer);

                if (responseObject.code != null)
                {
                    throw new Exception(responseObject.message.ToString());
                }
                log.LogMethodExit(responseFromServer);
                return responseFromServer;

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public string CreateTipAdjustment(string paymentId, double tipAmount, string cloverDevideId, string posId)
        {
            log.LogMethodEntry(paymentId, tipAmount, cloverDevideId, posId);
            string responseFromServer;
            string statusFromServer;
            try
            {
                WebRequest request = WebRequest.Create(baseUrl + "payments/" + paymentId + "/tip-adjust");
                request.Method = "POST";

                object requestObject = new TipRequestDTO
                {
                    tipAmount = tipAmount,
                };

                string jsonRequest = JsonConvert.SerializeObject(requestObject, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                byte[] byteArray = Encoding.UTF8.GetBytes(jsonRequest);

                request.ContentType = "application/json";
                request.Headers.Add("X-Clover-Device-Id", cloverDevideId);
                request.Headers.Add("X-POS-ID", posId);
                request.Headers.Add("Idempotency-Key", CreateIdempotencyKey());
                request.Headers.Add("Authorization", CreateAuthToken());
                request.ContentLength = byteArray.Length;

                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                statusFromServer = (((HttpWebResponse)response).StatusDescription);
                using (dataStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(dataStream);
                    responseFromServer = reader.ReadToEnd();
                }

                response.Close();
                log.LogMethodExit(responseFromServer);
                return responseFromServer;

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(ex.Message);
                return "Exception";
            }
        }

        public string CreatePrintPaymentReceipt(string paymentId, string cloverDevideId, string posId)
        {
            log.LogMethodEntry(paymentId, cloverDevideId, posId);
            string responseFromServer;
            string statusFromServer;
            try
            {
                WebRequest request = WebRequest.Create(baseUrl + "payments/" + paymentId + "/receipt");
                request.Method = "POST";

                object requestObject = new PrintReceiptDTO { deliveryOption = new DeliveryOptionDTO { } };

                string jsonRequest = JsonConvert.SerializeObject(requestObject, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                byte[] byteArray = Encoding.UTF8.GetBytes(jsonRequest);

                request.ContentType = "application/json";
                request.Headers.Add("X-Clover-Device-Id", cloverDevideId);
                request.Headers.Add("X-POS-ID", posId);
                request.Headers.Add("Idempotency-Key", CreateIdempotencyKey());
                request.Headers.Add("Authorization", CreateAuthToken());
                request.ContentLength = byteArray.Length;

                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                statusFromServer = (((HttpWebResponse)response).StatusDescription);
                using (dataStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(dataStream);
                    responseFromServer = reader.ReadToEnd();
                }

                response.Close();

                //Console.WriteLine(responseFromServer);

                if (statusFromServer == "OK")
                {
                    log.LogMethodExit(responseFromServer);
                    return "1";
                }
                else
                {
                    log.LogMethodExit(responseFromServer);
                    return "0";
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(ex.Message);
                return "Exception";
            }
        }

        public string CreatePrintVoidReceipt(string paymentId, string cloverDevideId, string posId)
        {
            log.LogMethodEntry(paymentId, cloverDevideId, posId);
            string responseFromServer;
            string statusFromServer;
            try
            {
                WebRequest request = WebRequest.Create(baseUrl + "payments/" + paymentId + "/void/receipt");
                request.Method = "POST";

                object requestObject = new PrintReceiptDTO { deliveryOption = new DeliveryOptionDTO { } };

                string jsonRequest = JsonConvert.SerializeObject(requestObject, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                byte[] byteArray = Encoding.UTF8.GetBytes(jsonRequest);

                request.ContentType = "application/json";
                request.Headers.Add("X-Clover-Device-Id", cloverDevideId);
                request.Headers.Add("X-POS-ID", posId);
                request.Headers.Add("Idempotency-Key", CreateIdempotencyKey());
                request.Headers.Add("Authorization", CreateAuthToken());
                request.ContentLength = byteArray.Length;

                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                statusFromServer = (((HttpWebResponse)response).StatusDescription);
                using (dataStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(dataStream);
                    responseFromServer = reader.ReadToEnd();
                }

                response.Close();
                if (statusFromServer == "OK")
                {
                    log.LogMethodExit(responseFromServer);
                    return "1";
                }
                else
                {
                    log.LogMethodExit(responseFromServer);
                    return "0";
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(ex.Message);
                return "Exception";
            }
        }

        public string CreateRefundReceipt(string paymentId, string cloverDevideId, string posId)
        {
            string responseFromServer;
            string statusFromServer;
            try
            {
                WebRequest request = WebRequest.Create(baseUrl + "refunds/" + paymentId + "/receipt");
                request.Method = "POST";

                object requestObject = new PrintReceiptDTO { deliveryOption = new DeliveryOptionDTO { } };

                string jsonRequest = JsonConvert.SerializeObject(requestObject, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                byte[] byteArray = Encoding.UTF8.GetBytes(jsonRequest);

                request.ContentType = "application/json";
                request.Headers.Add("X-Clover-Device-Id", cloverDevideId);
                request.Headers.Add("X-POS-ID", posId);
                request.Headers.Add("Idempotency-Key", CreateIdempotencyKey());
                request.Headers.Add("Authorization", CreateAuthToken());
                request.ContentLength = byteArray.Length;

                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                statusFromServer = (((HttpWebResponse)response).StatusDescription);
                using (dataStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(dataStream);
                    responseFromServer = reader.ReadToEnd();
                }

                response.Close();
                if (statusFromServer == "OK")
                {
                    return "1";
                }
                else
                {
                    return "0";
                }

            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
                //Console.ReadLine();
                return "Exception";
            }
        }

    }
}
