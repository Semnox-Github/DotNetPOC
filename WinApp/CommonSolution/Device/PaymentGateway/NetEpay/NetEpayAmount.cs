using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Device.PaymentGateway
{
    [Serializable]
    public class NetEpayAmount
    {
        private string purchase;
        private string authorize;
        private string gratuity;
        private string surchargeWithLookup;
        private string originalAuthorized;

        public NetEpayAmount()
        {
            this.purchase = null;
            this.authorize = null;
            this.gratuity = null;
            this.surchargeWithLookup = null;
        }
        public NetEpayAmount(string purchase, string authorize, string gratuity, string surchargeWithLookup, string originalAuthorized)
        {
            this.purchase = purchase;
            this.authorize = authorize;
            this.gratuity = gratuity;
            this.surchargeWithLookup = surchargeWithLookup;
            this.originalAuthorized = originalAuthorized;
        }

        public string Purchase
        {
            get
            {
                return purchase;
            }
            set
            {
                purchase = value;
            }
        }

        public string Authorize
        {
            get
            {
                return authorize;
            }
            set
            {
                authorize = value;
            }
        }

        public string Gratuity
        {
            get
            {
                return gratuity;
            }
            set
            {
                gratuity = value;
            }
        }

        public string SurchargeWithLookup
        {
            get
            {
                return surchargeWithLookup;
            }
            set
            {
                surchargeWithLookup = value;
            }
        }
        public string OriginalAuthorized
        {
            get
            {
                return originalAuthorized;
            }
            set
            {
                originalAuthorized = value;
            }
        }
    }
}
