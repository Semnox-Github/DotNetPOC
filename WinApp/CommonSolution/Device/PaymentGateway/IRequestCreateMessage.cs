using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Semnox.Parafait.Device.PaymentGateway
{
    /// <summary>
    ///Request Create Message 
    /// </summary>
    public interface IRequestCreateMessage
    {
        /// <summary>
        /// Create Request Message
        /// </summary>
        /// <param name="clsrequestattributes"></param>
        /// <returns></returns>
        string CreaterequestMessage(ClsRequestMessageAttributes clsrequestattributes);
    }
}
