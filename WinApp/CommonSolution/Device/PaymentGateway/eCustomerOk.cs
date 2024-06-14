using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Semnox.Parafait.Device.PaymentGateway
{
    /// <summary>
    /// 
    /// </summary>
    public enum eCustomerOK : int
    { 
        /// <summary>
        /// 
        /// </summary>
        LOOP = 0,
        /// <summary>
        /// 
        /// </summary>
        YES = 1,
        /// <summary>
        /// 
        /// </summary>
        NO = 2,
        /// <summary>
        /// 
        /// </summary>
        CANCEL = 3,
        /// <summary>
        /// 
        /// </summary>
        ERROR = 4,
    }
}
