using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Device.PaymentGateway
{
    internal class CloverRequestDTO
    {
    }
    public class CancelCurrentOperationDTO
    {
    }

    public class CheckConnecionDTO
    {
    }

    public class WelcomeMessageDTO
    {
    }

    public class ResetDeviceDTO
    {
    }

    public class PaymentRequestDTO
    {
        public bool? capture { get; set; }
        public bool? final { get; set; }
        public double? amount { get; set; }
        public string receipt_email { get; set; }
        public string externalPaymentId { get; set; }
        public double? tipAmount { get; set; }
    }
    public class IncrementalAuthRequestDTO
    {
        public double? amount { get; set; }
    }

    public class CaptureRequestDTO
    {
        public double? amount { get; set; }
        public double? tipAmount { get; set; }
    }

    public class RefundRequestDTO
    {
        public double? amount { get; set; }
        public bool? fullRefund { get; set; }
    }

    public class VoidRequestDTO
    {
        public string voidReason { get; set; }
    }

    public class TipRequestDTO
    {
        public double? tipAmount { get; set; }
        public double? baseAmount { get; set; }
        public List<TipSuggestionsDTO> tipSuggestions { get; set; }
    }

    public class TipSuggestionsDTO
    {
        public string name { get; set; }

        public double? amount { get; set; }

        public int? percentage { get; set; }
    }

    public class PrintReceiptDTO
    {
        public DeliveryOptionDTO deliveryOption;
    }

    public class DeliveryOptionDTO
    {
        public string method = "PRINT";
    }

}
