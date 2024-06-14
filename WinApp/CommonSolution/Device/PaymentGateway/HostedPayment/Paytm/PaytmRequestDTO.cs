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

namespace Semnox.Parafait.Device.PaymentGateway.HostedPayment.Paytm
{
    #region [ Common ]
    public class RequestHead
    {
        /// <summary>
        /// v1
        /// </summary>
        public string version { get; set; }
        /// <summary>
        /// WEB , WAP
        /// </summary>
        public string channelId { get; set; }
        /// <summary>
        /// EPOCH timestamp
        /// </summary>
        public string requestTimestamp { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string signature { get; set; }
    }
    public class RequestBody
    {
        public string mid { get; set; }
        public string orderId { get; set; }
    }
    public class AgentInfo
    {
        public string employeeId { get; set; }
        public string name { get; set; }
        public string phoneNo { get; set; }
        public string email { get; set; }
    }

    #endregion [ Common ]

    #region [ Initiate Transaction]
    public class InitiateTransactionRequestDto
    {
        public InitiateTransactionRequestHead head { get; set; }
        public InitiateTransactionRequestBody body { get; set; }
    }

    public class InitiateTransactionRequestBody : RequestBody
    {
        public string requestType { get; set; }
        public string websiteName { get; set; }
        public Txnamount txnAmount { get; set; }
        public Userinfo userInfo { get; set; }
        public string callbackUrl { get; set; }
        public string paytmSsoToken { get; set; }
        public PaymentMode[] enablePaymentMode { get; set; }
        public PaymentMode[] disablePaymentMode { get; set; }
        public string promoCode { get; set; }
        public ExtendInfo extendInfo { get; set; }

    }

    public class ExtendInfo
    {
        public string udf1 { get; set; }
        public string udf2 { get; set; }
        public string udf3 { get; set; }
        public string mercUnqRef { get; set; }
        public string comments { get; set; }
        public string subwalletAmount { get; set; }
    }

    public class Txnamount
    {
        public string value { get; set; }
        public string currency { get; set; }
    }

    public class Userinfo
    {
        public string custId { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
    }

    public class PaymentMode
    {
        public ModeOfPayment mode { get; set; }
        public string[] channels { get; set; }
    }

    public enum ModeOfPayment
    {
        BALANCE, PPBL, UPI, CREDIT_CARD, DEBIT_CARD, NET_BANKING, EMI, PAYTM_DIGITAL_CREDIT
    }


    public class InitiateTransactionRequestHead : RequestHead
    {
    }
    #endregion [ Initiate Transaction]

    #region [ Transaction Status]

    public class GetPaymentStatusRequestDto
    {
        public TransactionStatusRequestHead head { get; set; }
        public TransactionStatusRequestBody body { get; set; }
    }

    public class TransactionStatusRequestHead : RequestHead
    {
        public string clientId { get; set; }
    }

    public class TransactionStatusRequestBody
    {
        public string mid { get; set; }
        public string orderId { get; set; }
        public TransactionType txnType { get; set; }
    }

    public enum TransactionType
    {
        PREAUTH, RELEASE, CAPTURE, WITHDRAW
    }

    #endregion [ Transaction Status] 

    #region [ Refund ]

    public class RefundRequestDto
    {
        public RefundRequestHead head { get; set; }
        public RefundRequestBody body { get; set; }
    }

    public class RefundRequestHead : RequestHead
    {
        public string clientId { get; set; }
    }

    public class RefundRequestBody : RequestBody
    {
        public string refId { get; set; }
        public string txnId { get; set; }
        public string txnType { get { return "REFUND"; } }
        public string refundAmount { get; set; }
        public string preferredDestination { get; set; }
        public string comments { get; set; }
        public AgentInfo agentInfo { get; set; }
        public bool disableMerchantDebitRetry { get; set; }
        public string token { get; set; }
    }

    #endregion [ Refund]

    #region [ Refund Status ]

    public class RefundStatusRequestDto
    {
        public RefundStatusRequestHead head { get; set; }
        public RefundStatusRequestBody body { get; set; }
    }

    public class RefundStatusRequestHead : RequestHead
    {
        public string clientId { get; set; }
    }
    public class RefundStatusRequestBody : RequestBody
    {
        public string refId { get; set; }
    }

    #endregion [ Refund Status ]
}