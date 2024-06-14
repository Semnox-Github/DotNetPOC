/********************************************************************************************
 * Project Name -  PayTM Hosted Payment Gateway                                                                     
 * Description  -  Class to handle the payment of PayTM Payment Gateway
 *
 **************
 **Version Log
  *Version     Date          Modified By                Remarks          
 *********************************************************************************************
 *2.150.1      8-Mar-2023    Muaaz Musthafa             Created for Website
 ********************************************************************************************/

using System.Collections.Generic;

namespace Semnox.Parafait.Device.PaymentGateway.HostedPayment.Paytm
{
    #region [Common]
    public class ResponseHead
    {
        public string responseTimestamp { get; set; }
        public string version { get; set; }
        public string signature { get; set; }
        public string clientId { get; set; }
    }
    public class Resultinfo
    {
        public string resultStatus { get; set; }
        //public ResultStatus resultStatus { get; set; }
        public string resultCode { get; set; }
        public string resultMsg { get; set; }


    }
    public enum ResultStatus
    {
        TXN_SUCCESS, TXN_FAILURE, PENDING, NO_RECORD_FOUND, F, S
    }
    public class RefundDetailInfo
    {
        public string payMethod { get; set; }
        public string refundType { get; set; }
        public string refundAmount { get; set; }
        public string issuingBankName { get; set; }
        public string maskedCardNumber { get; set; }
        public string cardScheme { get; set; }
        public string userMobileNo { get; set; }
        public string maskedVpa { get; set; }
        public string maskedBankAccountNumber { get; set; }
        public string ifscCode { get; set; }
        public string issuingBankCode { get; set; }
        public string rrn { get; set; }
        public string userCreditExpectedDate { get; set; }
    }

    #endregion [Common]

    #region [ Initiate Transaction]
    public class CreatePaymentTokenResponseDto
    {
        public InitiateTransactionResponseHead head { get; set; }
        public InitiateTransactionResponseBody body { get; set; }
    }

    public class InitiateTransactionResponseHead : ResponseHead
    {

    }

    public class InitiateTransactionResponseResultinfo : Resultinfo
    {
        public bool bankRetry { get; set; }
        public bool retry { get; set; }
        public bool isRedirect { get; set; }
    }

    public class InitiateTransactionResponseBody
    {
        public InitiateTransactionResponseResultinfo resultInfo { get; set; }
        public object extraParamsMap { get; set; }
        public bool isPromoCodeValid { get; set; }
        public string txnToken { get; set; }
        public bool authenticated { get; set; }
        public string orderId { get; set; }
        public string customerId { get; set; }
        public string reqAmount { get; set; }

    }
    #endregion [ Initiate Transaction]

    #region [ Transaction Status]

    public class GetPaymentStatusResponseDto
    {
        public TransactionStatusResponseHead head { get; set; }
        public TransactionStatusResponseBody body { get; set; }
    }

    public class TransactionStatusResponseHead : ResponseHead
    {
        public string channelId { get; set; }
    }
    public class TransactionStatusResponseBody
    {
        public Resultinfo resultInfo { get; set; }
        public string txnId { get; set; }
        public string bankTxnId { get; set; }
        public string orderId { get; set; }
        public string txnAmount { get; set; }
        public string txnType { get; set; }
        public string gatewayName { get; set; }
        public string bankName { get; set; }
        public string mid { get; set; }
        public string paymentMode { get; set; }
        public string refundAmt { get; set; }
        public string txnDate { get; set; }
        public string authCode { get; set; }
        public string cardScheme { get; set; }
        public string lastFourDigit { get; set; }
        public string merchantUniqueReference { get; set; }
        public string udf1 { get; set; }
        public string udf2 { get; set; }
    }

    #endregion [ Transaction Status]

    #region [ Refund ]

    public class RefundResponseDto
    {
        public RefundResponseHead head { get; set; }
        public RefundResponseBody body { get; set; }
    }

    public class RefundResponseHead : ResponseHead
    {
        public string channelId { get; set; }
    }

    public class RefundResponseBody
    {
        public Resultinfo resultInfo { get; set; }
        public string mid { get; set; }
        public string orderId { get; set; }
        public string refId { get; set; }
        public string refundId { get; set; }
        public string txnId { get; set; }
        public string txnTimestamp { get; set; }
        public string refundAmount { get; set; }
    }

    #endregion [ Refund ]

    #region [ Refund Status ]

    public class RefundStatusResponseDto
    {
        public RefundStatusResponseHead head { get; set; }
        public RefundStatusResponseBody body { get; set; }
    }

    public class RefundStatusResponseHead : ResponseHead
    {
        public string channelId { get; set; }
    }

    public class RefundStatusResponseBody : RefundResponseBody
    {
        public string source { get; set; }
        public string txnAmount { get; set; }
        public string totalRefundAmount { get; set; }
        public string merchantRefundRequestTimestamp { get; set; }
        public string maxRefundRetryTimeStamp { get; set; }
        public string acceptRefundStatus { get; set; }
        public string acceptRefundTimestamp { get; set; }
        public string userCreditInitiateStatus { get; set; }
        public string userCreditInitiateTimestamp { get; set; }
        public string refundReason { get; set; }
        public string gatewayInfo { get; set; }
        public AgentInfo agentInfo { get; set; }
        public List<RefundDetailInfo> refundDetailInfoList { get; set; }
    }

    #endregion [ Refund Status ]

    #region [ Call Back ]

    public class CallBackResponseDto
    {
        public string BANKNAME { get; set; }
        public string BANKTXNID { get; set; }
        public string CHECKSUMHASH { get; set; }
        public string CURRENCY { get; set; }
        public string GATEWAYNAME { get; set; }
        public string MID { get; set; }
        public string ORDERID { get; set; }
        public string PAYMENTMODE { get; set; }
        public string RESPCODE { get; set; }
        public string RESPMSG { get; set; }
        public string STATUS { get; set; }
        public string TXNAMOUNT { get; set; }
        public string TXNDATE { get; set; }
        public string TXNID { get; set; }
        public string prepaidCard { get; set; }
        public FeeRateFactors feeRateFactors { get; set; }

        // international payments
        //public string MERC_UNQ_REF { get; set; }
        //public string CUSTID { get; set; }
        //public string baseCurrency { get; set; }
    }

    public class FeeRateFactors
    {
        public bool corporateCard { get; set; }
    }

    #endregion [ Call Back ]

    #region [Success and Error Codes, Messages]
    public class PaytmResponseMessage
    {
        public string code { get; set; }
        public string status { get; set; }
        public string message { get; set; }
        public bool isSuccess { get; set; }

        public PaytmResponseMessage(string code, string status, string message, bool isSuccess)
        {
            this.code = code;
            this.status = status;
            this.message = message;
            this.isSuccess = isSuccess;
        }
    }
    #endregion

}