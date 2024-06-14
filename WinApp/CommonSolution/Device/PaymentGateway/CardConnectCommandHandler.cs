using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
//using Semnox.Parafait.TransactionPayments;
//using Semnox.Parafait.PaymentGateway;
using Semnox.Core.Utilities;
namespace Semnox.Parafait.Device.PaymentGateway
{
    public abstract class CardConnectCommandHandler
    {
        protected string url;
        protected string method;
        protected string contenttype;
        protected HttpWebRequest webRequest;
        protected HttpWebResponse webResponse;
        protected NameValueCollection requestCollection;
        protected NameValueCollection responseCollection;
        protected WebRequestHandler webRequestHandler;
        protected Utilities utilities;
        protected string merchantId;
        protected TransactionPaymentsDTO transactionPaymentsDTO;
        
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public CardConnectCommandHandler(Utilities utilities, TransactionPaymentsDTO transactionPaymentsDTO,string merchantId)
        {
            log.LogMethodEntry(utilities, transactionPaymentsDTO);
            this.utilities = utilities;            
            this.transactionPaymentsDTO = transactionPaymentsDTO;
            this.merchantId = merchantId;
            this.contenttype = "application/json";
            log.LogVariableState("merchantId", merchantId);
            log.LogMethodExit(null);
        }

        public virtual void CreateCommand(object data)
        {
            log.LogMethodEntry(data);
            log.LogMethodExit(null);
        }
        public virtual HttpWebResponse Sendcommand()
        {
            log.LogMethodEntry();
            log.LogMethodExit(null);
            return null;
        }

        public virtual object GetResponse(HttpWebResponse webResponse)
        {
            log.LogMethodEntry();
            log.LogMethodExit(null);
            return null;
        }

        public virtual CCTransactionsPGWDTO ParseResponse(object response)
        {
            log.LogMethodEntry();
            log.LogMethodExit(null);
            return null;
        }

    }

    public enum Methods
    {
        GET,
        POST,
        PUT,
        DELETE
    }
}
