using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Device.PaymentGateway.HostedPayment.PayFort
{
    public class PayFortPaymentResponseDTO
    {
        public string amount { get; set; }
        public string response_code { get; set; }
        public string card_number { get; set; }
        public string card_holder_name { get; set; }
        public string signature { get; set; }
        public string merchant_identifier { get; set; }
        public string access_code { get; set; }
        public string payment_option { get; set; }
        public string expiry_date { get; set; }
        public string customer_ip { get; set; }
        public string language { get; set; }
        public string eci { get; set; }
        public string fort_id { get; set; }
        public string command { get; set; }
        public string merchant_extra { get; set; }
        public string response_message { get; set; }
        public string merchant_reference { get; set; }
        public string authorization_code { get; set; }
        public string customer_email { get; set; }
        public string merchant_extra1 { get; set; }
        public string currency { get; set; }
        public string status { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class PayFortTxSearchResponseDTO
    {
        public string query_command { get; set; }
        public string access_code { get; set; }
        public string merchant_identifier { get; set; }
        public string merchant_reference { get; set; }
        public string language { get; set; }
        public string signature { get; set; }
        public string fort_id { get; set; }
        public string response_message { get; set; }
        public string response_code { get; set; }
        public string status { get; set; }
        public string transaction_status { get; set; }
        public string transaction_code { get; set; }
        public string transaction_message { get; set; }
        public string refunded_amount { get; set; }
        public string captured_amount { get; set; }
        public string authorized_amount { get; set; }
        public string authorization_code { get; set; }
        public string processor_response_code { get; set; }
        public string acquirer_response_code { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this); ;
        }
    }

}
