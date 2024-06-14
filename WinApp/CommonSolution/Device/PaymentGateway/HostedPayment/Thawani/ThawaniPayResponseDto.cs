using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Semnox.Parafait.Device.PaymentGateway.HostedPayment.Thawani
{
    public class ThawaniPayResponseDto
    {
    }

    public class ThawaniPayResponse
    {
        public CheckoutSessionResponseDto checkoutSessionResponseDto;
        public GetPaymentListResponseDto paymentListResponseDto;
    }

    public class CheckoutSessionResponseDto
    {
        public bool success { get; set; }
        public int code { get; set; }
        public string description { get; set; }
        public Data data { get; set; }
    }

    public class Data
    {
        public string session_id { get; set; }
        public string client_reference_id { get; set; }
        public string customer_id { get; set; }
        public List<Product> products { get; set; }
        public int total_amount { get; set; }
        public string currency { get; set; }
        public string success_url { get; set; }
        public string cancel_url { get; set; }
        public string payment_status { get; set; }
        public string mode { get; set; }
        public string invoice { get; set; }
        public Metadata metadata { get; set; }
        public DateTime created_at { get; set; }
        public DateTime expire_at { get; set; }
        public Subscription subscription { get; set; }

        // Get List of Payments
        public string activity { get; set; }
        public string payment_id { get; set; }
        public string masked_card { get; set; }
        public string card_type { get; set; }
        public string status { get; set; }
        public int amount { get; set; }
        public int fee { get; set; }
        public bool refunded { get; set; }
        public List<Refund> refunds { get; set; }
        public string payment_intent { get; set; }
        public string checkout_invoice { get; set; }
        //public DateTime created_at { get; set; }
        public string reason { get; set; }

        //Create Refund
        public string refund_id { get; set; }

        // Webhook - Payment
        public object customer { get; set; }
        public object card { get; set; }
        public bool save_card_on_success { get; set; }


    }

    //public class Metadata
    //{
    //}

    public class Subscription
    {
    }

    //public class Product
    //{
    //    public string name { get; set; }
    //    public int quantity { get; set; }
    //    public int unit_amount { get; set; }
    //}


    public class GetPaymentListResponseDto
    {
        public bool success { get; set; }
        public int code { get; set; }
        public string description { get; set; }
        public List<Data> data { get; set; }
    }

    //public class Data
    //{
    //    public string activity { get; set; }
    //    public string payment_id { get; set; }
    //    public string masked_card { get; set; }
    //    public string card_type { get; set; }
    //    public string status { get; set; }
    //    public int amount { get; set; }
    //    public int fee { get; set; }
    //    public bool refunded { get; set; }
    //    public List<Refund> refunds { get; set; }
    //    public string payment_intent { get; set; }
    //    public string checkout_invoice { get; set; }
    //    public DateTime created_at { get; set; }
    //    public string reason { get; set; }
    //}

    public class Refund
    {
        public int amount { get; set; }
        public string refund_id { get; set; }
        public string payment_id { get; set; }
        public string status { get; set; }
        public string reason { get; set; }
        public Metadata metadata { get; set; }
        public DateTime created_at { get; set; }
    }


    public class RefundResponseDto
    {
        public bool success { get; set; }
        public int code { get; set; }
        public string description { get; set; }
        public Data data { get; set; }
    }

    //public class Data
    //{
    //    public int amount { get; set; }
    //    public string refund_id { get; set; }
    //    public string payment_id { get; set; }
    //    public string status { get; set; }
    //    public string reason { get; set; }
    //    public Metadata metadata { get; set; }
    //    public DateTime created_at { get; set; }
    //}


    public class WebhookResponseDto
    {
        public Data data { get; set; }
        public string event_type { get; set; }
    }

    //public class Metadata
    //{
    //    public string customer { get; set; }
    //}

    //public class Product
    //{
    //    public string name { get; set; }
    //    public int unit_amount { get; set; }
    //    public int quantity { get; set; }
    //}

}