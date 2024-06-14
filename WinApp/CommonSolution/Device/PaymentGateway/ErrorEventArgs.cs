using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Semnox.Parafait.Device.PaymentGateway
{
    /// <summary>
    /// ErrorResponseEventArgs Class
    /// </summary>
    public class ErrorResponseEventArgs : Exception
    {
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string statusmessage;
        private ClsResponseMessageAttributes responseMessageatt;
        /// <summary>
        /// ErrorResponseEventArgs Method
        /// </summary>
        /// <param name="Statusmessage"></param>
        /// <param name="responseMsgAttributes"></param>
        public ErrorResponseEventArgs(string Statusmessage, ClsResponseMessageAttributes responseMsgAttributes) //, Exception ex, ClsResponseMessageAttributes responseMsgAttributes, List<ClsRequestMessageAttributes> listReqAttributes)
        {
            log.LogMethodEntry(Statusmessage, responseMsgAttributes);

           statusmessage = Statusmessage;
           responseMessageatt = responseMsgAttributes;

            log.LogMethodExit(null);
        }

        /// <summary>
        /// Get Property for Status Message
        /// </summary>
        public String StatusMessage
        {
            get { return statusmessage; }
        }

        /// <summary>
        /// Get Property for ResponseMessage
        /// </summary>
        public ClsResponseMessageAttributes ResponseMessage
        {
            get { return responseMessageatt; }
        }
    }
}
