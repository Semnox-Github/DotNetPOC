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
    internal class PayTMDQRRequestDTO
    {
        public decimal transactionAmount { get; set; }
        public string orderId { get; set; } // payment ccRequestId
        public decimal refundAmount { get; set; }
        //public string refundId { get; set; }
        public string paytmPaymentId { get; set; }
        public string refId { get; set; } // refund ccRequestId
    }
}
