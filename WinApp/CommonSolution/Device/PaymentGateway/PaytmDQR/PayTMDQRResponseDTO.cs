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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Device.PaymentGateway
{
    internal class PayTMDQRResponseDTO
    {
        public Head head { get; set; }
        public Body body { get; set; }
    }

    public class Head
    {
        public string responseTimestamp { get; set; }
        public string version { get; set; }
        public string clientId { get; set; }
        public string signature { get; set; }
    }

    public class Body
    {
        public Resultinfo resultInfo { get; set; }

        public bool overallTimeout { get; set; }

        public int noOfRequestsSent { get; set; }
        public string qrCodeId { get; set; }
        public string qrData { get; set; }
        public string image { get; set; }

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
        public string authRefId { get; set; }
        public string txnTimestamp { get; set; }
        public string refId { get; set; }
        public string refundId { get; set; }
        public string refundAmount { get; set; }

        // Check Refund Status
        public List<RefundDetail> refundDetailInfoList { get; set; }
        public string userCreditInitiateStatus { get; set; }
        public DateTime merchantRefundRequestTimestamp { get; set; }
        public string source { get; set; }
        public DateTime acceptRefundTimestamp { get; set; }
        public string acceptRefundStatus { get; set; }
        public string totalRefundAmount { get; set; }
    }

    public class RefundDetail
    {
        public string refundType { get; set; }
        public string payMethod { get; set; }
        public string maskedVpa { get; set; }
        public string userMobileNo { get; set; }
        public string refundAmount { get; set; }
    }

    public class Resultinfo
    {
        public string resultStatus { get; set; }
        public string resultCode { get; set; }
        public string resultMsg { get; set; }
    }



}
