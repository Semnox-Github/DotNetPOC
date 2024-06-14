
using System.Collections.Generic;
using Semnox.Parafait.Device.PaymentGateway;

//This cs holds all the DTO class definitons required for Payment, refund, void

namespace Semnox.Parafait.Device.PaymentGateway
{
    public class Payment
    {
        public int amount { get; set; }
        public Cardtransaction cardTransaction { get; set; }
        public long createdTime { get; set; }
        public Device device { get; set; }
        public Employee employee { get; set; }
        public string externalPaymentId { get; set; }
        public string id { get; set; }
        public long modifiedTime { get; set; }
        public bool offline { get; set; }
        public Order order { get; set; }
        public object[] refunds { get; set; }
        public string result { get; set; }
        public int taxAmount { get; set; }
        public int tipAmount { get; set; }
    }

    public class Cardtransaction
    {
        public string authCode { get; set; }
        public bool captured { get; set; }
        public string cardType { get; set; }
        public string cardholderName { get; set; }
        public string currency { get; set; }
        public string entryType { get; set; }
        public Extra extra { get; set; }
        public string first6 { get; set; }
        public string last4 { get; set; }
        public string referenceId { get; set; }
        public string state { get; set; }
        public string transactionNo { get; set; }
        public string type { get; set; }
    }

    public class Extra
    {
        public string iccContainer { get; set; }
        public string common { get; set; }
        public string func { get; set; }
        public string authorizingNetworkName { get; set; }
        public string athNtwkId { get; set; }
        public string routingIndicator { get; set; }
        public string exp { get; set; }
        public string cvmResult { get; set; }
        public string applicationIdentifier { get; set; }
        public string card { get; set; }
        public string tkntype { get; set; }
    }

    public class Device
    {
        public string id { get; set; }
    }

    public class Employee
    {
        public string id { get; set; }
    }

    public class Order
    {
        public string id { get; set; }
    }

}
