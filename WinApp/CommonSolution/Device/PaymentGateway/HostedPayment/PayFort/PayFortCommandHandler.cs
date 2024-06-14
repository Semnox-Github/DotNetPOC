using Newtonsoft.Json;
using Semnox.Core.HttpUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Device.PaymentGateway.HostedPayment.PayFort
{
    class PayFortCommandHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string _accessCode;
        //private bool _callbackEnable;
        private string _merchantIdentifier;
        private string _hashKey;
        private string _payfortRedirectionUrl;
        private string _payfortApiUrl;
        public PayFortCommandHandler(string accessCode, string merchantIdentifier, string hashKey, string payfortRedirectionUrl, string payfortApiUrl)
        {
            log.LogMethodEntry(accessCode, merchantIdentifier, hashKey, payfortRedirectionUrl, payfortApiUrl);
            _accessCode = accessCode;
            _merchantIdentifier = merchantIdentifier;
            _hashKey = hashKey;
            _payfortRedirectionUrl = payfortRedirectionUrl;
            _payfortApiUrl = payfortApiUrl;
            log.LogMethodExit();
        }

        public PayFortPaymentResponseDTO CreateRefund(PayFortRefundRequestDTO refundRequestDTO)
        {
            log.LogMethodEntry(refundRequestDTO);
            PayFortPaymentResponseDTO refundResponseDTO = null;
            try
            {
                SortedDictionary<string, string> refundReqParamsSortedDictionary = new SortedDictionary<string, string>
                {
                    { "command", "REFUND" },
                    { "access_code", _accessCode },
                    { "merchant_identifier", _merchantIdentifier },
                    { "merchant_reference", refundRequestDTO.merchant_reference },
                    { "language", "en" },
                    { "amount", (refundRequestDTO.amount * 100).ToString() },
                    { "currency", "AED" }
                };

                string genRefundSignature = GenerateHash(refundReqParamsSortedDictionary);
                refundReqParamsSortedDictionary.Add("signature", genRefundSignature);

                string postRefundData = JsonConvert.SerializeObject(refundReqParamsSortedDictionary, Formatting.None);
                log.Debug("Refund request: " + postRefundData);

                WebRequestClient webRequestClient = new WebRequestClient(_payfortApiUrl, HttpVerb.POST, postRefundData);
                webRequestClient.IsBasicAuthentication = false;
                string refundResponse = webRequestClient.MakeRequest();
                log.Debug("Response from TxSearch: " + refundResponse);

                refundResponseDTO = JsonConvert.DeserializeObject<PayFortPaymentResponseDTO>(refundResponse);

                if (refundResponseDTO == null)
                {
                    log.Error($"No refund response received for trxId: {refundRequestDTO.merchant_reference}");
                    throw new Exception("Error processing refund!");
                }

                if (!VerifySignature(refundResponseDTO))
                {
                    log.Error("Signature does not match");
                    throw new Exception("Error processing refund!");
                }

                log.LogMethodExit(refundResponseDTO);
                return refundResponseDTO;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw;
            }
        }

        public PayFortTxSearchResponseDTO CreateTxSearch(string trxId)
        {
            log.LogMethodEntry(trxId);
            PayFortTxSearchResponseDTO txSearchResponseDTO = null;
            try
            {
                SortedDictionary<string, string> txReqParamsSortedDictionary = new SortedDictionary<string, string>
                {
                    { "query_command", "CHECK_STATUS"},
                    { "access_code", _accessCode},
                    { "merchant_identifier", _merchantIdentifier},
                    { "merchant_reference", trxId},
                    { "language", "en"}
                };

                string genSignature = GenerateHash(txReqParamsSortedDictionary);
                txReqParamsSortedDictionary.Add("signature", genSignature);

                string postTxReqData = JsonConvert.SerializeObject(txReqParamsSortedDictionary, Formatting.None);
                log.Debug("TxSearch request: " + postTxReqData);

                WebRequestClient webRequestClient = new WebRequestClient(_payfortApiUrl, HttpVerb.POST, postTxReqData);
                webRequestClient.IsBasicAuthentication = false;

                string txSearchResponse = webRequestClient.MakeRequest();
                log.Debug("Response from TxSearch: " + txSearchResponse);

                txSearchResponseDTO = JsonConvert.DeserializeObject<PayFortTxSearchResponseDTO>(txSearchResponse);

                if (txSearchResponseDTO == null)
                {
                    log.Error($"No txSearch response received for trxId: {trxId}");
                    throw new Exception("Error processing txSearch!");
                }

                if (!VerifySignature(txSearchResponseDTO))
                {
                    log.Error("Signature does not match!");
                    throw new Exception("Error processing txSearch!");
                }

                log.LogMethodExit(txSearchResponseDTO);
                return txSearchResponseDTO;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw;
            }
        }

        public bool VerifySignature<T>(T objectDTO)
        {
            log.LogMethodEntry(objectDTO);
            bool isMatch = false;
            string resposneSignature = typeof(T).GetProperty("signature").GetValue(objectDTO).ToString();
            if (string.IsNullOrEmpty(resposneSignature))
            {
                log.Error("No Signature key present in response.");
                return isMatch;
            }

            //Get the values from response for signature verification
            SortedDictionary<string, string> sortedResponseDictionary = new SortedDictionary<string, string>();
            foreach (PropertyInfo property in objectDTO.GetType().GetProperties())
            {
                string key = property.Name;
                string value = property.GetValue(objectDTO) != null ? property.GetValue(objectDTO).ToString() : "";
                if (key != "signature" && !string.IsNullOrEmpty(value))
                {
                    sortedResponseDictionary.Add(key, value);
                }
            }

            string genSignature = GenerateHash(sortedResponseDictionary);
            log.Debug("Generated Signature: " + genSignature);

            if (genSignature != resposneSignature)
            {
                isMatch = false;
                log.Error("Payment Signature does not match");
            }
            else
            {
                isMatch = true;
                log.Info("Payment Signature matched!");
            }

            return isMatch;
        }

        public string GenerateHash(IDictionary<string, string> postparamslist)
        {
            log.LogMethodEntry(postparamslist);
            //List<string> mandatoryFields = new List<string>();
            //postparamslist.Sort(Compare1); //Here postparamslist is already sorted
            string paramLine = _hashKey;// "TESTSHAIN";
            foreach (KeyValuePair<string, string> item in postparamslist)
            {
                paramLine += item.Key + "=" + item.Value;
            }
            paramLine += _hashKey;// "TESTSHAIN";
            log.Info(paramLine);

            log.LogMethodExit(paramLine);
            return Generatehash512(paramLine);
        }

        public static string Generatehash512(string text)
        {
            log.LogMethodEntry(text);
            byte[] message = Encoding.UTF8.GetBytes(text);

            UnicodeEncoding UE = new UnicodeEncoding();
            byte[] hashValue;
            SHA512Managed hashString = new SHA512Managed();
            string hex = "";
            hashValue = hashString.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }
            log.LogMethodExit(hex);
            return hex;
        }

    }
}
