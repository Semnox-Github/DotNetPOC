using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Semnox.Parafait.Device.PaymentGateway.HostedPayment.Thawani
{
    public class ThawaniPayConfigurations
    {
        public string _public_key { get; set; }
        public string _secret_key { get; set; }
        public string _host_url { get; set; }
        public string _checkout_url { get; set; }

        public ThawaniPayConfigurations(string PUBLIC_KEY, string SECRET_KEY, string HOST_URL, string CHECKOUT_URL)
        {
            _public_key = PUBLIC_KEY;
            _secret_key = SECRET_KEY;
            _host_url = HOST_URL;
            _checkout_url = CHECKOUT_URL;
        }
    }
}