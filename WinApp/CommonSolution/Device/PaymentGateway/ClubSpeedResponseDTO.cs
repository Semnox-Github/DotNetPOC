using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Device.PaymentGateway
{
    internal class ClubSpeedResponseDTO
    {
        public List<Customer> Customers { get; set; }
    }

    public class Customer
    {
        public string cardId { get; set; }
        public double payAmount { get; set; }
        public string message { get; set; }
        public string referenceNo { get; set; }
        public double balance { get; set; }
        public string sessionId { get; set; }
    }

    class PaymentResponse
    {
        public string resultCode { get; set; }
    }
}
