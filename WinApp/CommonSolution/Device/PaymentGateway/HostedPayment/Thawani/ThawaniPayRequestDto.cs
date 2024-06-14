using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Semnox.Parafait.Device.PaymentGateway.HostedPayment.Thawani
{
    public class ThawaniPayRequestDto
    {
    }

    public class CreateCheckoutSessionRequestDto
    {
        public string client_reference_id { get; set; }
        public string mode { get; set; }
        public List<Product> products { get; set; }
        public string success_url { get; set; }
        public string cancel_url { get; set; }
        public string customer_id { get; set; }
        public Metadata metadata { get; set; }
    }

    public class Metadata
    {
        public string Customer_name { get; set; }
        public string Customer_email { get; set; }
        public string Customer_phonenumber{ get; set; }
        public int orderid { get; set; }
        public int PaymentModeId { get; set; }
    }

    public class Product
    {
        public string name { get; set; }
        public int quantity { get; set; }
        public int unit_amount { get; set; }
    }

    public class GetCheckoutSessionRequestDto
    {
        public string client_reference_id { get; set; }
    }

    public class GetPaymentListRequestDto
    {
        public int limit { get; set; }
        public int skip { get; set; }
        public string checkout_invoice { get; set; }
        public string payment_intent { get; set; }

    }

    public class CreateRefundRequestDto
    {
        public string payment_id { get; set; }
        public string reason { get; set; }
        public Metadata metadata { get; set; }
    }
    
}