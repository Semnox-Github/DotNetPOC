using System;

namespace Semnox.Parafait.Device.PaymentGateway.HostedPayment.CyberSource
{
    public class CyberSourceRequestDTO
    {
        public Clientreferenceinformation clientReferenceInformation { get; set; }
        public Processinginformation processingInformation { get; set; }
        public Paymentinformation paymentInformation { get; set; }
        public Orderinformation orderInformation { get; set; }
        public string paymentId { get; set; }
    }


    public class TxSearchRequestDTO
    {
        public string query { get; set; }
        public string sort { get; set; }

    }


    public class RefundRequestDTO
    {
        public Clientreferenceinformation clientReferenceInformation { get; set; }
        public Orderinformation orderInformation { get; set; }
    }

    public class VoidRequestDTO
    {
        public Clientreferenceinformation clientReferenceInformation { get; set; }
    }


    //public class CheckoutRequestDTO
    //{

    //}

    //public class Clientreferenceinformation
    //{
    //    public string code { get; set; }
    //}

    //public class Processinginformation
    //{
    //    public bool capture { get; set; }
    //}

    //public class Paymentinformation
    //{
    //    public Card card { get; set; }
    //}

    //public class Card
    //{
    //    public string number { get; set; }
    //    public string expirationMonth { get; set; }
    //    public string expirationYear { get; set; }
    //}

    //public class Orderinformation
    //{
    //    public Amountdetails amountDetails { get; set; }
    //    public Billto billTo { get; set; }
    //}

    //public class Amountdetails
    //{
    //    public string totalAmount { get; set; }
    //    public string currency { get; set; }
    //}

    //public class Billto
    //{
    //    public string firstName { get; set; }
    //    public string lastName { get; set; }
    //    public string address1 { get; set; }
    //    public string locality { get; set; }
    //    public string administrativeArea { get; set; }
    //    public string postalCode { get; set; }
    //    public string country { get; set; }
    //    public string email { get; set; }
    //    public string phoneNumber { get; set; }
    //}
}