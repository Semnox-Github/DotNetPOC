/********************************************************************************************
 * Project Name -  Moneris Hosted Payment Gateway                                                                     
 * Description  -  Class to handle the payment of Moneris Payment Gateway
 *
 **************
 **Version Log
  *Version     Date             Modified By                Remarks          
 *********************************************************************************************
 *2.150.3     15-May-2023      Muaaz Musthafa             Created for Website
 ********************************************************************************************/

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Device.PaymentGateway.HostedPayment.Moneris
{

    public enum MonerisAction
    {
        receipt,
        preload
    }
    public class MonerisPreloadRequestDTO
    {
        public string store_id { get; set; }
        public string api_token { get; set; }
        public string checkout_id { get; set; }
        public string txn_total { get; set; }
        public string environment { get; set; }
        public string action { get; set; }
        public string ticket { get; set; }
        public string order_no { get; set; }
        public string ask_cvv = "Y";
        public string cust_id { get; set; }
        public ContactDetails contact_details { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
    public class ContactDetails
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
    }
}
