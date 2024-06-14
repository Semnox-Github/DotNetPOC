/******************************************************************************************************
 * Project Name - Device
 * Description  - PineLabs Payment gateway
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By    Remarks          
 ******************************************************************************************************
 *2.140.5     11-Jan-2023    Prasad & Fiona   PineLabs Payment gateway integration
 ********************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Device.PaymentGateway
{
    internal class PineLabsPlutusA920RequestDTO
    {
    }

    public class PineLabsGetPaymentRequestDto
    {
        public string TxnId { get; set; }
        public string TxnAmtPaise { get; set; }
        public string MerchantID { get; set; }
        public string TxnTypeIdentifierString { get; set; }
        public string SecurityToken { get; set; }
    }

    public class PineLabsGetUpiPaymentStatus
    {
        public string ccRequestId { get; set; }
        public double amount { get; set; }
        public string batchId { get; set; }
        public string invoiceNo { get; set; }
    }

}
