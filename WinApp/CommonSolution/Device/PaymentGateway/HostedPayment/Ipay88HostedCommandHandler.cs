using Newtonsoft.Json;
using Semnox.Core.HttpUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Device.PaymentGateway
{
    class Ipay88HostedCommandHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public string merchantKey { get; set; }
        public string merchantCode { get; set; }
        public string currencyCode { get; set; }

        public Ipay88HostedCommandHandler(string merchantKey, string merchantCode, string currencyCode)
        {
            this.merchantKey = merchantKey;
            this.merchantCode = merchantCode;
            this.currencyCode = currencyCode;
        }

        public TxDetailsInquiryCardInfoResponse RequeryPayment(string trxId, string amount, string txSearchApiUrl)
        {
            log.LogMethodEntry(trxId, amount, txSearchApiUrl);
            string receivedResponse = null;
            string serializedTxSearchRequest = null;

            //Serialize TxSearch Request data
            TxDetailsInquiryCardInfo txDetailsInquiryCardInfo = new TxDetailsInquiryCardInfo
            {
                MerchantCode = merchantCode,
                ReferenceNo = trxId,
                Amount = amount,
                Version = ""
            };
            XMLSerializeDeserialize<TxDetailsInquiryCardInfo> xMLSerializeDeserialize;
            xMLSerializeDeserialize = new XMLSerializeDeserialize<TxDetailsInquiryCardInfo>();

            serializedTxSearchRequest = xMLSerializeDeserialize.SerializeData(txDetailsInquiryCardInfo);
            log.Debug("serializedTxSearchRequest: " + serializedTxSearchRequest);

            //Make TxSearch API Call
            //receivedResponse = MakeXMLRequest(url, serializedTxSearchRequest);
            WebRequestClient txSearchClient = new WebRequestClient(txSearchApiUrl, HttpVerb.POST, serializedTxSearchRequest);
            receivedResponse = txSearchClient.MakeXMLRequest();
            log.Debug("TxSearch receivedResponse: " + receivedResponse);

            //Deserialize TxSearch Repsonse
            XMLSerializeDeserialize<TxDetailsInquiryCardInfoResponse> xmlTxSearchResp;
            xmlTxSearchResp = new XMLSerializeDeserialize<TxDetailsInquiryCardInfoResponse>();
            TxDetailsInquiryCardInfoResponse deserialiedTxSearch = xmlTxSearchResp.DeserializeData(receivedResponse);
            log.LogMethodExit(deserialiedTxSearch.ToString());

            return deserialiedTxSearch;
        }

        public string GetVoidSignature(string merchantKey, string merchantCode, string CCtransId, double voidAmount, string currencyCode)
        {
            //create VoidTransactionSignatureString
            string voidSignatureString = string.Empty;
            voidSignatureString += merchantKey + merchantCode + CCtransId;
            string amount = Convert.ToString(voidAmount * 100);
            voidSignatureString += amount + currencyCode;

            //create VoidSignature 
            string VoidSignature = GetTransactionSignature(voidSignatureString);

            log.Debug("Void Signature: " + VoidSignature);
            return VoidSignature;
        }

        public string VoidTransaction(string ccTransId, double voidAmount, string voidApiUrl)
        {
            string receivedVoidResponse = null;

            //Create void signature
            string voidSignature = GetVoidSignature(merchantKey, merchantCode, ccTransId, voidAmount, currencyCode);
            log.Debug("Void Signature: " + voidSignature);

            //Void Trx Request DTO
            VoidTransaction voidTransactionDTO = new VoidTransaction
            {
                merchantcode = merchantCode,
                cctransid = ccTransId,
                amount = Convert.ToString(voidAmount),
                currency = currencyCode,
                signature = voidSignature,
            };

            XMLSerializeDeserialize<VoidTransaction> xmlVoidRequest;
            xmlVoidRequest = new XMLSerializeDeserialize<VoidTransaction>();
            string serializedVoidTransaction = xmlVoidRequest.SerializeData(voidTransactionDTO);
            log.Debug("serializedTxSearchRequest: " + serializedVoidTransaction);

            //Make Void API Call
            //string receivedVoidResponse = MakeXMLRequest(voidUrl, serializedVoidTransaction);
            WebRequestClient voidClient = new WebRequestClient(voidApiUrl, HttpVerb.POST, serializedVoidTransaction);
            receivedVoidResponse = voidClient.MakeXMLRequest();
            log.Debug("Void receivedResponse: " + receivedVoidResponse);

            //Deserialize Void Response
            XMLSerializeDeserialize<VoidTransactionResponse> xmlVoidTrxResponse;
            xmlVoidTrxResponse = new XMLSerializeDeserialize<VoidTransactionResponse>();
            VoidTransactionResponse deserialiedVoid = xmlVoidTrxResponse.DeserializeData(receivedVoidResponse);

            log.LogMethodExit(deserialiedVoid.VoidTransactionResult);
            return deserialiedVoid.VoidTransactionResult;
        }

        public string GetTransactionSignature(string toEncrypt)
        {
            SHA1CryptoServiceProvider objSHA1 = new SHA1CryptoServiceProvider();
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            objSHA1.ComputeHash(toEncryptArray);

            byte[] buffer = objSHA1.Hash;

            string HashValue = System.Convert.ToBase64String(buffer);

            return HashValue;
        }

        private string GetRefundSignature(string transId, double refund_cash_amount, string payeeACNo = "")
        {
            log.LogMethodEntry(transId, refund_cash_amount, payeeACNo);

            string refundSignatureString = string.Empty;
            refundSignatureString += merchantCode + merchantKey + transId;
            string amount = Convert.ToString(refund_cash_amount * 100);
            refundSignatureString += amount + payeeACNo;
            log.Debug("refundSignatureString: " + refundSignatureString);

            //create RefundSignature 
            string RefundSignature = GetTransactionSignature(refundSignatureString);

            log.LogMethodExit(RefundSignature);
            return RefundSignature;
        }

        public IPay88RefundResponseDTO RefundTransaction(string ccTransId, double amount, string refundApiUrl)
        {
            log.LogMethodEntry(ccTransId, amount, refundApiUrl);
            string resultJson;
            try
            {
                //create Signature for refund
                string refundSignature = GetRefundSignature(ccTransId, amount);
                log.Debug("Refund Signature: " + refundSignature);

                IPay88RefundRequestDTO iPay88refundRequestDTO = null;
                iPay88refundRequestDTO = new IPay88RefundRequestDTO
                {
                    MerchantCode = merchantCode,
                    REFUND_CASH_AMOUNT = string.Format("{0:0.00}", amount),
                    PayeeBank = "",
                    PayeeName = "",
                    PayeeACNo = "",
                    Signature = refundSignature,
                    TransId = ccTransId
                };

                string postData = JsonConvert.SerializeObject(iPay88refundRequestDTO);
                log.Debug("refundPayload: " + postData);

            
                WebRequestClient refundClient = new WebRequestClient(refundApiUrl, HttpVerb.POST, postData);
                resultJson = refundClient.MakeRequest();

                log.Debug("resultJson from Refund: " + resultJson);

                IPay88RefundResponseDTO iPay88RefundResponseDTO;
                iPay88RefundResponseDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<IPay88RefundResponseDTO>(resultJson);

                log.LogMethodExit("iPay88RefundResponseDTO: " + iPay88RefundResponseDTO.ToString());
                return iPay88RefundResponseDTO;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw new Exception(ex.Message);
            }
        }

    }

}
