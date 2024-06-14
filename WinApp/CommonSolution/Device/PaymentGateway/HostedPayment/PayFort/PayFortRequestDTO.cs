using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Device.PaymentGateway.HostedPayment.PayFort
{
    public class PayFortRefundRequestDTO
    {
        public string command { get; set; }
        public string access_code { get; set; }
        public string merchant_identifier { get; set; }
        public string merchant_reference { get; set; }
        public double amount { get; set; }
        public string currency { get; set; }
        public string language { get; set; }
        public string signature { get; set; }
        public string fort_id { get; set; }
        public string order_description { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
