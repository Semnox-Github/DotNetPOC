using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using Semnox.Parafait.Device.PaymentGateway.HostedPayment.WPCyberSource;

namespace Semnox.Parafait.Device.PaymentGateway.HostedPayment.CyberSource
{
    public class CyberSourceCommandHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public String getSignature(string transaction_type, string transaction_uuid, string signed_field_names, string signed_date_time, string ccRequestId, decimal amount, string currency, string ignore_avs, string payer_authentication_transaction_mode, string payer_authentication_challenge_code, string access_key, string profile_id, string locale, string secret_key, string merchant_defined_data1, string merchant_defined_data2)
        {
            log.LogMethodEntry();
            try
            {
                Dictionary<string, string> signatureParams = new Dictionary<string, string>();
                signatureParams.Add("access_key", access_key);
                signatureParams.Add("profile_id", profile_id);
                signatureParams.Add("transaction_uuid", transaction_uuid);
                signatureParams.Add("signed_field_names", signed_field_names);
                signatureParams.Add("unsigned_field_names", "");
                signatureParams.Add("signed_date_time", signed_date_time);
                signatureParams.Add("locale", locale);
                //signatureParams.Add("ignore_avs", ignore_avs);
                signatureParams.Add("transaction_type", transaction_type);
                signatureParams.Add("reference_number", ccRequestId);
                signatureParams.Add("amount", Convert.ToString(amount));
                signatureParams.Add("currency", currency);

                signatureParams.Add("payer_authentication_transaction_mode", payer_authentication_transaction_mode);
                signatureParams.Add("payer_authentication_challenge_code", payer_authentication_challenge_code.ToString());
                signatureParams.Add("merchant_defined_data1", merchant_defined_data1);
                signatureParams.Add("merchant_defined_data2", merchant_defined_data2);

                log.LogMethodExit();
                return WPCybersourceSecurity.sign(signatureParams, secret_key);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw new Exception(ex.Message);
            }

        }

        public String prepareForm(string signed_field_names, string transaction_uuid, string signed_date_time, string locale, string ignore_avs, string transaction_type, string ccRequestId, decimal amount, string currency, string signature, string payer_authentication_transaction_mode, string payer_authentication_challenge_code, string access_key, string profile_id, string checkout_url, string merchant_defined_data1, string merchant_defined_data2)
        {
            log.LogMethodEntry();
            try
            {
                string form = "<html><head>";
                form += "<META HTTP-EQUIV=\"CACHE-CONTROL\" CONTENT=\"no-store, no-cache, must-revalidate\" />";
                form += "<META HTTP-EQUIV=\"PRAGMA\" CONTENT=\"no-store, no-cache, must-revalidate\" />";
                form += $"</head><body onload =\"document.WorldPayWebPaymentsForm.submit()\">";
                form += $"<form name=\"WorldPayWebPaymentsForm\" method=\"POST\" action=\"{checkout_url}\">";
                form += $"<input type=\"hidden\" name=\"access_key\" value=\"{access_key}\">";
                form += $"<input type=\"hidden\" name=\"profile_id\" value=\"{profile_id}\">";
                form += $"<input type=\"hidden\" name=\"transaction_uuid\" value=\"{transaction_uuid}\">";
                form += $"<input type=\"hidden\" name=\"signed_field_names\" value=\"{signed_field_names}\">";
                form += $"<input type=\"hidden\" name=\"unsigned_field_names\" value=\"\">";
                form += $"<input type=\"hidden\" name=\"signed_date_time\" value=\"{signed_date_time}\">";
                form += $"<input type=\"hidden\" name=\"locale\" value=\"{locale}\">";
                //form += $"<input type=\"hidden\" name=\"ignore_avs\" value=\"{ignore_avs}\">";
                form += $"<input type=\"hidden\" name=\"transaction_type\" value=\"{transaction_type}\">";
                form += $"<input type=\"hidden\" name=\"reference_number\" value=\"{ccRequestId}\">";
                form += $"<input type=\"hidden\" name=\"amount\" value=\"{amount.ToString()}\">";
                form += $"<input type=\"hidden\" name=\"currency\" value=\"{currency}\">";

                form += $"<input type=\"hidden\" name=\"merchant_defined_data1\" value=\"{merchant_defined_data1}\">";
                form += $"<input type=\"hidden\" name=\"merchant_defined_data2\" value=\"{merchant_defined_data2}\">";
                form += $"<input type=\"hidden\" name=\"payer_authentication_transaction_mode\" value=\"{payer_authentication_transaction_mode}\">";
                form += $"<input type=\"hidden\" name=\"payer_authentication_challenge_code\" value=\"{payer_authentication_challenge_code}\">";
                form += $"<input type=\"hidden\" name=\"signature\" value=\"{signature}\">";

                form += $"</form>";
                form += "</body></html>";

                log.LogMethodExit();
                return form;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        // Create Tx Search when we dont receive timely response of SALE Tx
        public TxSearchResponseDTO CreateTxSearch(TxSearchRequestDTO searchRequestDTO, Dictionary<string, string> configParameters)
        {
            log.LogMethodEntry(searchRequestDTO, configParameters);
            TxSearchResponseDTO response = null;
            string SCHEME = GetConfigParams(configParameters, "SCHEME");
            string HOST = GetConfigParams(configParameters, "HOST_URL");
            string BASE_URL = SCHEME + HOST;
            try
            {
                if (!string.IsNullOrEmpty(BASE_URL))
                {
                    string API_URL = BASE_URL + "/tss/v2/searches";
                    if (searchRequestDTO != null)
                    {
                        response = MakeSearchRequest(searchRequestDTO, API_URL, configParameters);
                    }
                    else
                    {
                        throw new Exception("Request was null");
                    }
                }
                else
                {
                    throw new Exception("Base Url was null");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw new Exception(ex.Message);
            }
            log.LogMethodExit(response);
            return response;
        }

        private TxSearchResponseDTO MakeSearchRequest(TxSearchRequestDTO searchRequestDTO, string API_URL, Dictionary<string, string> configParameters)
        {
            log.LogMethodEntry(searchRequestDTO, API_URL, configParameters);
            try
            {
                string requestDate = getFormattedRequestDate();
                string requestTarget = "post /tss/v2/searches";

                string host = GetConfigParams(configParameters, "HOST_URL");
                //string host = GetConfigParams(configParameters, "HOST_PRODUCTION");
                string MerchantId = GetConfigParams(configParameters, "MERCHANT_ID");
                string SecretKey = GetConfigParams(configParameters, "REST_SECRET_KEY");
                string KeyId = GetConfigParams(configParameters, "PUBLIC_KEY");
                string algorithm = GetConfigParams(configParameters, "ALGORITHM");

                string JsonObj = JsonConvert.SerializeObject(searchRequestDTO, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore });

                var Digest = GenerateDigest(JsonObj);

                var signatureParams = "host: " + host + "\n(request-target): " + requestTarget + "\ndigest: " + Digest + "\nv-c-merchant-id: " + MerchantId;
                log.Debug("signatureParams: " + signatureParams);
                var SignatureHash = GenerateSignatureFromParams(signatureParams, SecretKey);

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(API_URL);
                httpWebRequest.Method = "POST";

                httpWebRequest.Headers.Add("v-c-merchant-id", MerchantId);
                httpWebRequest.Headers.Add("v-c-date", requestDate);
                httpWebRequest.Headers.Add("Digest", Digest);
                httpWebRequest.Headers.Add("Signature", "keyid=\"" + KeyId + "\", algorithm=\"" + algorithm + "\", headers=\"host (request-target) digest v-c-merchant-id\", signature=\"" + SignatureHash + "\"");
                httpWebRequest.ContentType = "application/json";
                log.Debug("TxSearch httpWebRequest headers: " + httpWebRequest.Headers);

                //var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                HttpWebResponse httpResponse;
                string responseFromServer = "";
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(JsonObj);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    responseFromServer = streamReader.ReadToEnd();
                }

                // deserialize the response received
                TxSearchResponseDTO response = deserializeTxSearchResponse(responseFromServer);
                log.Debug("TxSearch responseFromServer:" + responseFromServer);

                log.LogMethodExit(response);
                return response;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        // Tx Status Check using Cybersource Payment Id
        public TxStatusResponseDTO CreateTransactionCheck(CyberSourceRequestDTO requestDTO, Dictionary<string, string> configParameters)
        {
            log.LogMethodEntry(requestDTO, configParameters);
            TxStatusResponseDTO response = null;
            string SCHEME = GetConfigParams(configParameters, "SCHEME");
            string HOST = GetConfigParams(configParameters, "HOST_URL");
            string BASE_URL = SCHEME + HOST;
            try
            {
                if (!string.IsNullOrEmpty(BASE_URL))
                {
                    string API_URL = BASE_URL + "/tss/v2/transactions/" + requestDTO.paymentId;
                    log.Info(API_URL);
                    //string API_URL = "https://apitest.cybersource.com/tss/v2/transactions/6584950941846702404006";
                    if (requestDTO != null)
                    {
                        response = MakeTransactionStatusCheckRequest(requestDTO, API_URL, configParameters);
                    }
                    else
                    {
                        throw new Exception("Request was null");
                    }
                }
                else
                {
                    throw new Exception("Base Url was null");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw new Exception(ex.Message);
            }

            log.LogMethodExit(response);
            return response;
        }

        private TxStatusResponseDTO MakeTransactionStatusCheckRequest(CyberSourceRequestDTO requestDTO, string API_URL, Dictionary<string, string> configParameters)
        {
            log.LogMethodEntry(requestDTO, API_URL, configParameters);
            string responseFromServer;
            string statusFromServer;
            try
            {
                string requestDate = getFormattedRequestDate();
                string requestTarget = "get /tss/v2/transactions/" + requestDTO.paymentId;
                log.Debug("requestTarget: " + requestTarget);

                string host = GetConfigParams(configParameters, "HOST_URL");
                //string host = GetConfigParams(configParameters, "HOST_PRODUCTION");
                string MerchantId = GetConfigParams(configParameters, "MERCHANT_ID");
                string SecretKey = GetConfigParams(configParameters, "REST_SECRET_KEY");
                string KeyId = GetConfigParams(configParameters, "PUBLIC_KEY");
                string algorithm = GetConfigParams(configParameters, "ALGORITHM");

                var SignatureParm = "host: " + host + "\n(request-target): " + requestTarget + "\nv-c-merchant-id: " + MerchantId;
                log.Debug("SignatureParm: " + SignatureParm);
                var SignatureHash = GenerateSignatureFromParams(SignatureParm, SecretKey);
                log.Debug("SignatureHash: " + SignatureHash);

                //var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://apitest.cybersource.com/pts/v2/payments/6584950941846702404006/refunds");
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(API_URL);
                httpWebRequest.Method = "GET";

                httpWebRequest.Headers.Add("v-c-merchant-id", MerchantId);
                //httpWebRequest.Headers.Add("v-c-date", DateTime.Now.ToString("ddd, dd MMM yyy HH':'mm':'ss 'GMT'"));
                httpWebRequest.Headers.Add("v-c-date", requestDate);
                httpWebRequest.Headers.Add("Signature", "keyid=\"" + KeyId + "\", algorithm=\"" + algorithm + "\", headers=\"host (request-target) v-c-merchant-id\", signature=\"" + SignatureHash + "\"");
                httpWebRequest.ContentType = "application/json";
                log.Debug("TxSearch httpWebRequest headers: " + httpWebRequest.Headers);

                HttpWebResponse webResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                statusFromServer = ((HttpWebResponse)webResponse).StatusDescription;
                Stream receiveStream = webResponse.GetResponseStream();
                StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                responseFromServer = readStream.ReadToEnd();

                webResponse.Close();
                readStream.Close();

                log.Info(responseFromServer);

                // deserialize the response received
                TxStatusResponseDTO response = deserializeTxStatusResponse(responseFromServer);

                log.LogMethodExit(response);
                return response;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw new Exception(ex.Message);
            }
        }


        // VOID : requires Cybersource Payment Id and ccRequestId
        public VoidResponseDTO CreateVoid(CyberSourceRequestDTO requestDTO, VoidRequestDTO voidRequestDTO, Dictionary<string, string> configParameters)
        {
            log.LogMethodEntry(requestDTO, voidRequestDTO, configParameters);
            VoidResponseDTO response = null;
            string SCHEME = GetConfigParams(configParameters, "SCHEME");
            string HOST = GetConfigParams(configParameters, "HOST_URL");
            string BASE_URL = SCHEME + HOST;
            try
            {
                if (!string.IsNullOrEmpty(BASE_URL))
                {
                    string API_URL = BASE_URL + "/pts/v2/payments/" + requestDTO.paymentId + "/voids";
                    if (requestDTO != null)
                    {
                        response = MakeVoidRequest(requestDTO, voidRequestDTO, API_URL, configParameters);
                    }
                    else
                    {
                        throw new Exception("Request was null");
                    }
                }
                else
                {
                    throw new Exception("Base Url was null");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw new Exception(ex.Message);
            }
            log.LogMethodExit(response);
            return response;
        }

        private VoidResponseDTO MakeVoidRequest(CyberSourceRequestDTO requestDTO, VoidRequestDTO voidRequestDTO, string API_URL, Dictionary<string, string> configParameters)
        {
            log.LogMethodEntry(requestDTO, voidRequestDTO, API_URL, configParameters);
            try
            {
                string requestDate = getFormattedRequestDate();
                string requestTarget = "post /pts/v2/payments/" + requestDTO.paymentId + "/voids";

                string host = GetConfigParams(configParameters, "HOST_URL");
                //string host = GetConfigParams(configParameters, "HOST_PRODUCTION");
                string MerchantId = GetConfigParams(configParameters, "MERCHANT_ID");
                string SecretKey = GetConfigParams(configParameters, "REST_SECRET_KEY");
                string KeyId = GetConfigParams(configParameters, "PUBLIC_KEY");
                string algorithm = GetConfigParams(configParameters, "ALGORITHM");

                string JsonObj = JsonConvert.SerializeObject(voidRequestDTO, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore });

                var Digest = GenerateDigest(JsonObj);

                var SignatureParm = "host: " + host + "\n(request-target): " + requestTarget + "\ndigest: " + Digest + "\nv-c-merchant-id: " + MerchantId;
                log.Debug("SignatureParm: " + SignatureParm);
                var SignatureHash = GenerateSignatureFromParams(SignatureParm, SecretKey);

                //var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://apitest.cybersource.com/pts/v2/payments/6584950941846702404006/refunds");
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(API_URL);
                httpWebRequest.Method = "POST";

                httpWebRequest.Headers.Add("v-c-merchant-id", MerchantId);
                httpWebRequest.Headers.Add("v-c-date", requestDate);
                httpWebRequest.Headers.Add("Digest", Digest);
                httpWebRequest.Headers.Add("Signature", "keyid=\"" + KeyId + "\", algorithm=\"" + algorithm + "\", headers=\"host (request-target) digest v-c-merchant-id\", signature=\"" + SignatureHash + "\"");
                httpWebRequest.ContentType = "application/json";
                log.Debug("Void httpWebRequest headers: " + httpWebRequest.Headers);

                HttpWebResponse httpResponse;
                string responseFromServer = "";
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(JsonObj);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    responseFromServer = streamReader.ReadToEnd();
                }

                // deserialize the response received
                VoidResponseDTO response = deserializeVoidResponse(responseFromServer);
                log.Info(responseFromServer);

                log.LogMethodEntry(response);
                return response;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        //REFUND: requires Cybersource Payment Id, ccRequestId, totalAmount to be refunded and currency
        public RefundResponseDTO CreateRefund(CyberSourceRequestDTO requestDTO, RefundRequestDTO refundRequestDTO, Dictionary<string, string> configParameters)
        {
            log.LogMethodEntry(requestDTO, refundRequestDTO, configParameters);
            RefundResponseDTO response = null;
            string SCHEME = GetConfigParams(configParameters, "SCHEME");
            string HOST = GetConfigParams(configParameters, "HOST_URL");
            string BASE_URL = SCHEME + HOST;
            try
            {
                if (!string.IsNullOrEmpty(BASE_URL))
                {
                    string API_URL = BASE_URL + "/pts/v2/payments/" + requestDTO.paymentId + "/refunds";
                    log.Info(API_URL);
                    if (requestDTO != null)
                    {
                        response = MakeRefundRequest(requestDTO, refundRequestDTO, API_URL, configParameters);
                    }
                    else
                    {
                        throw new Exception("Request was null");
                    }
                }
                else
                {
                    throw new Exception("Base Url was null");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw new Exception(ex.Message);
            }
            log.LogMethodExit(response);
            return response;
        }

        private RefundResponseDTO MakeRefundRequest(CyberSourceRequestDTO requestDTO, RefundRequestDTO refundRequestDTO, string API_URL, Dictionary<string, string> configParameters)
        {
            log.LogMethodEntry(requestDTO, refundRequestDTO, API_URL, configParameters);
            try
            {
                string requestDate = getFormattedRequestDate();
                string requestTarget = "post /pts/v2/payments/" + requestDTO.paymentId + "/refunds";

                string host = GetConfigParams(configParameters, "HOST_URL");
                string MerchantId = GetConfigParams(configParameters, "MERCHANT_ID");
                string SecretKey = GetConfigParams(configParameters, "REST_SECRET_KEY");
                string KeyId = GetConfigParams(configParameters, "PUBLIC_KEY");
                string algorithm = GetConfigParams(configParameters, "ALGORITHM");
                //SecretKey = "Ga+dQUuctbdudg7Pzw8yvzh6yttlwF6TjKfbCj8sQ5c=";
                //string KeyId = "3d249232-bfe6-46ff-87dc-332ed320f9a9";
                //string MerchantId = "cwpdsemnoxtest001";
                //string requestTarget = "post /pts/v2/payments/6584950941846702404006/refunds";

                //string JsonObj = "{\"clientReferenceInformation\":{\"code\":\"1658494933318\"},\"orderInformation\":{\"amountDetails\":{\"totalAmount\":\"10\",\"currency\":\"GBP\"}}}";
                string JsonObj = JsonConvert.SerializeObject(refundRequestDTO, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore });

                var Digest = GenerateDigest(JsonObj);

                var SignatureParm = "host: " + host + "\n(request-target): " + requestTarget + "\ndigest: " + Digest + "\nv-c-merchant-id: " + MerchantId;
                log.Debug("SignatureParm: " + SignatureParm);
                var SignatureHash = GenerateSignatureFromParams(SignatureParm, SecretKey);

                //var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://apitest.cybersource.com/pts/v2/payments/6584950941846702404006/refunds");
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(API_URL);
                httpWebRequest.Method = "POST";

                httpWebRequest.Headers.Add("v-c-merchant-id", MerchantId);
                httpWebRequest.Headers.Add("v-c-date", requestDate);
                httpWebRequest.Headers.Add("Digest", Digest);
                httpWebRequest.Headers.Add("Signature", "keyid=\"" + KeyId + "\", algorithm=\"" + algorithm + "\", headers=\"host (request-target) digest v-c-merchant-id\", signature=\"" + SignatureHash + "\"");
                httpWebRequest.ContentType = "application/json";
                log.Debug("Refund httpWebRequest headers: " + httpWebRequest.Headers);

                //var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                HttpWebResponse httpResponse;
                string responseFromServer = "";
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(JsonObj);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    responseFromServer = streamReader.ReadToEnd();
                }

                // deserialize the response received
                RefundResponseDTO response = deserializeRefundResponse(responseFromServer);
                log.LogMethodExit(response);
                return response;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw new Exception(ex.Message);
            }
        }



        // Methods for Deserialization of the response
        private VoidResponseDTO deserializeVoidResponse(string response)
        {
            log.LogMethodEntry(response);
            try
            {
                log.LogMethodExit(response);
                return JsonConvert.DeserializeObject<VoidResponseDTO>(response);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw new Exception(ex.Message);
            }

        }
        private TxStatusResponseDTO deserializeTxStatusResponse(string response)
        {
            try
            {
                return JsonConvert.DeserializeObject<TxStatusResponseDTO>(response);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw new Exception(ex.Message);
            }
        }
        private RefundResponseDTO deserializeRefundResponse(string response)
        {
            try
            {
                return JsonConvert.DeserializeObject<RefundResponseDTO>(response);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw new Exception(ex.Message);
            }

        }
        private TxSearchResponseDTO deserializeTxSearchResponse(string response)
        {
            try
            {
                return JsonConvert.DeserializeObject<TxSearchResponseDTO>(response);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw new Exception(ex.Message);
            }

        }
        // End Deserialization


        public TxSearchRequestDTO GetTxSearchRequestDTO(string ccRequestId)
        {
            try
            {
                TxSearchRequestDTO searchRequestDTO = new TxSearchRequestDTO
                {
                    query = "clientReferenceInformation.code:" + ccRequestId,
                    sort = "id:desc",
                };
                return searchRequestDTO;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public static string GenerateDigest(string jsonRestBody)
        {
            log.LogMethodEntry();
            var digest = "";
            var bodyText = jsonRestBody;
            try
            {
                using (var sha256hash = SHA256.Create())
                {
                    byte[] payloadBytes = sha256hash
                        .ComputeHash(Encoding.UTF8.GetBytes(bodyText));
                    digest = Convert.ToBase64String(payloadBytes);
                    digest = "SHA-256=" + digest;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw new Exception(ex.Message);
            }
            log.LogMethodExit(digest);
            return digest;
        }

        private static string GenerateSignatureFromParams(string signatureParams, string secretKey)
        {
            log.LogMethodEntry();
            try
            {
                byte[] sigBytes = Encoding.UTF8.GetBytes(signatureParams);
                byte[] decodedSecret = Convert.FromBase64String(secretKey);
                HMACSHA256 hmacSha256 = new HMACSHA256(decodedSecret);
                byte[] messageHash = hmacSha256.ComputeHash(sigBytes);
                log.LogMethodExit(Convert.ToBase64String(messageHash));
                return Convert.ToBase64String(messageHash);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw new Exception(ex.Message);
            }

        }

        private string GetConfigParams(Dictionary<string, string> configParameters, string key)
        {
            string configvalue = "";
            try
            {
                if (configParameters.ContainsKey(key))
                {
                    configParameters.TryGetValue(key, out configvalue);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw new Exception(ex.Message);
            }
            return configvalue;
        }

        private string getFormattedRequestDate()
        {
            log.LogMethodEntry();
            log.LogMethodExit(DateTime.Now.ToUniversalTime().ToString("r"));
            return DateTime.Now.ToUniversalTime().ToString("r");
        }


        public CyberSourceRequestDTO getRequestDTO(string paymentId)
        {
            try
            {
                CyberSourceRequestDTO requestDTO = null;
                requestDTO = new CyberSourceRequestDTO
                {
                    paymentId = paymentId,
                };

                return requestDTO;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw new Exception(ex.Message);
            }
        }


        public void WriteToFile(string text)
        {
            try
            {
                var dir = HttpContext.Current.Server.MapPath("~/responses/");
                string fileName = DateTime.Now.ToString("d") + ".txt";
                var file = Path.Combine(dir, fileName);

                Directory.CreateDirectory(dir);
                //File.WriteAllText(file, text);
                File.AppendAllText(file, text);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw new Exception(ex.ToString());
            }

        }

        public void WriteWebhookResponseToFile(string text)
        {
            try
            {
                var dir = HttpContext.Current.Server.MapPath("~/responses/");
                string fileName = "WR_" + DateTime.Now.ToString("d") + ".txt";
                var file = Path.Combine(dir, fileName);

                Directory.CreateDirectory(dir);
                //File.WriteAllText(file, text);
                File.AppendAllText(file, text);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw new Exception(ex.ToString());
            }

        }

        public String getUUID()
        {
            return Guid.NewGuid().ToString();
        }

        public String getUTCDateTime()
        {
            DateTime time = DateTime.Now.ToUniversalTime();
            return time.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'");
        }

        public Dictionary<string, string> getUserBillingDetails(string userId)
        {
            try
            {
                // fetch user billing details from the DB
                // Assumed the details have been fetched; now proceed
                Dictionary<string, string> userBillingDetails = new Dictionary<string, string>();
                userBillingDetails.Add("bill_to_forename", "Prasad");
                userBillingDetails.Add("bill_to_surname", "Diwan");
                userBillingDetails.Add("bill_to_email", "prasadrdiwan@gmail.com");
                userBillingDetails.Add("bill_to_address_line1", "74 Shore Street");
                userBillingDetails.Add("bill_to_address_line2", "");
                userBillingDetails.Add("bill_to_address_city", "Stoke Sub Hambon");
                userBillingDetails.Add("bill_to_address_state", "");
                userBillingDetails.Add("bill_to_address_country", "GB"); // Use the two character ISO country code.
                userBillingDetails.Add("bill_to_address_postal_code", "TA14 3GD");
                userBillingDetails.Add("locale", "en");

                return userBillingDetails;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw new Exception(ex.Message);
            }

        }

        public String getCCRequestId()
        {
            return Guid.NewGuid().ToString().Substring(0, 8);
        }

        public TxStatusDTO GetTxStatusFromSearchResponse(TxSearchResponseDTO response)
        {
            log.LogMethodEntry(response);
            TxStatusDTO txStatusDTO = null;
            if (response != null)
            {
                try
                {
                    if (response.totalCount > 0)
                    {
                        List<Transactionsummary> transactionSummaries = response._embedded.transactionSummaries;
                        var transactions = from transaction in transactionSummaries
                                           from app in transaction.applicationInformation.applications
                                           where app.name == "ics_bill" && app.reasonCode == "100" // second condition added on 09 Nov 2022
                                           select transaction;
                        if (transactions.Count() > 0)
                        {
                            // check for void/refund
                            var voidTx = from transaction_void in transactionSummaries
                                         from app_void in transaction_void.applicationInformation.applications
                                         where app_void.name == "ics_void"
                                         select transaction_void;
                            var refundTx = from transaction_refund in transactionSummaries
                                           from app_refund in transaction_refund.applicationInformation.applications
                                           where app_refund.name == "ics_credit"
                                           select transaction_refund;
                            if (voidTx.Count() > 0)
                            {
                                //void found
                                log.Debug("Void already done");
                                txStatusDTO = new TxStatusDTO
                                {
                                    reasonCode = -2,
                                    status = "TX NOT FOUND",
                                    TxType = "NA"
                                };
                            }
                            else if (refundTx.Count() > 0)
                            {
                                //refund found
                                log.Debug("Refund already done");
                                txStatusDTO = new TxStatusDTO
                                {
                                    reasonCode = -2,
                                    status = "TX NOT FOUND",
                                    TxType = "NA"
                                };
                            }
                            else
                            {
                                // proceed with sale
                                foreach (var tx in transactions)
                                {
                                    if (tx.applicationInformation.reasonCode == 100)
                                    {
                                        txStatusDTO = new TxStatusDTO();
                                        txStatusDTO.reasonCode = Convert.ToInt32(tx.applicationInformation.reasonCode);
                                        txStatusDTO.paymentId = tx.id;
                                        txStatusDTO.InvoiceNo = tx.clientReferenceInformation.code.ToString();
                                        txStatusDTO.AuthCode = tx.processorInformation.approvalCode.ToString();
                                        txStatusDTO.Authorize = tx.orderInformation.amountDetails.totalAmount.ToString();
                                        txStatusDTO.Purchase = txStatusDTO.Authorize;
                                        txStatusDTO.TransactionDatetime = tx.submitTimeUtc;
                                        txStatusDTO.AcctNo = string.Concat("**** **** **** ", tx.paymentInformation.card.suffix.ToString());
                                        txStatusDTO.RecordNo = tx.id;
                                        var app = from application in tx.applicationInformation.applications
                                                  where application.name == "ics_bill"
                                                  select application;
                                        foreach (var application_obj in app)
                                        {
                                            if (application_obj.rMessage != null)
                                            {
                                                txStatusDTO.TextResponse = application_obj.rMessage.ToString();
                                            }
                                        }

                                        txStatusDTO.TxType = "SALE";
                                    }
                                    else
                                    {
                                        // reasoncode other than 100
                                        txStatusDTO = new TxStatusDTO();
                                        txStatusDTO.reasonCode = Convert.ToInt32(tx.applicationInformation.reasonCode);
                                        txStatusDTO.InvoiceNo = tx.clientReferenceInformation.code.ToString();
                                        txStatusDTO.TxType = "SALE";
                                        var app = from application in tx.applicationInformation.applications
                                                  where application.name == "ics_bill"
                                                  select application;
                                        if (app.Count() > 0)
                                        {
                                            foreach (var application_obj in app)
                                            {
                                                if (application_obj.rMessage != null)
                                                {
                                                    txStatusDTO.TextResponse = application_obj.rMessage.ToString();
                                                }
                                            }
                                        }
                                        else
                                        {
                                            txStatusDTO.TextResponse = String.Empty;

                                        }

                                    }
                                }
                            }
                        }
                        else
                        {
                            txStatusDTO = new TxStatusDTO
                            {
                                reasonCode = -2,
                                status = "TX NOT FOUND",
                                TxType = "NA"
                            };
                        }
                    }
                    else
                    {
                        txStatusDTO = new TxStatusDTO
                        {
                            reasonCode = -2,
                            status = "TX NOT FOUND",
                            TxType = "NA"
                        };
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    throw new Exception(ex.Message);
                }

            }
            else
            {
                txStatusDTO = new TxStatusDTO
                {
                    reasonCode = -2,
                    status = "TX NOT FOUND",
                    TxType = "NA"
                };
            }
            log.LogMethodExit(txStatusDTO);
            return txStatusDTO;
        }

        public bool verifySignature(string response, string secret_key)
        {
            log.LogMethodEntry(response, secret_key);
            bool result = false;
            //response = JsonConvert.SerializeObject(response);
            CyberSourceResponseDTO responseObj = JsonConvert.DeserializeObject<CyberSourceResponseDTO>(response);
            if (responseObj != null)
            {
                Dictionary<string, string> responseParams = new Dictionary<string, string>();
                Dictionary<string, string> signatureParams = new Dictionary<string, string>();
                responseParams = JsonConvert.DeserializeObject<Dictionary<string, string>>(response);
                foreach (KeyValuePair<string, string> pair in responseParams)
                {
                    signatureParams.Add(pair.Key, pair.Value);
                }
                string sign = WPCybersourceSecurity.sign(signatureParams, secret_key);
                if (sign.Equals(responseObj.signature))
                {
                    result = true;
                }
            }
            log.LogMethodExit(result);
            return result;
        }

        public bool verifySignature(CyberSourceResponseDTO responseObj, string secret_key)
        {
            log.LogMethodEntry(responseObj, secret_key);
            bool result = false;
            try
            {
                if (responseObj != null)
                {
                    Dictionary<string, string> responseParams = new Dictionary<string, string>();
                    Dictionary<string, string> signatureParams = new Dictionary<string, string>();
                    responseParams = JsonConvert.DeserializeObject<Dictionary<string, string>>(JsonConvert.SerializeObject(responseObj));
                    foreach (KeyValuePair<string, string> pair in responseParams)
                    {
                        try
                        {
                            log.Debug(pair.Key + ":" + pair.Value);
                            signatureParams.Add(pair.Key, pair.Value);
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                        }
                    }
                    log.Debug("Initiating sign process ");
                    string sign = WPCybersourceSecurity.sign(signatureParams, secret_key);
                    log.Debug("Generated Signature " + sign);
                    log.Debug("Incoming Signature " + responseObj.signature);
                    if (sign.Equals(responseObj.signature))
                    {
                        result = true;
                    }
                }
            }
            catch(Exception ex)
            {
                log.Error("Gor error in verify signature " + ex);
            }
            log.LogMethodExit(result);
            return result;
        }
    }

}
