using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Semnox.Parafait.Device.PaymentGateway
{
    /// <summary>
    /// 
    /// </summary>
    public enum eSCATStatus : int
    {
        /// <summary>
        /// 
        /// </summary>
        WAIT = 0,
        /// <summary>
        /// 
        /// </summary>
        DONE = 1,
        /// <summary>
        /// 
        /// </summary>
        MANUAL = 2,
        /// <summary>
        /// 
        /// </summary>
        CANCEL = 3,
        /// <summary>
        /// 
        /// </summary>
        FATAL_ERROR = 4,
        /// <summary>
        /// 
        /// </summary>
        CHECK_MICR_STATUS = 5,
        /// <summary>
        /// 
        /// </summary>
        MANUAL_NOT_ALLOWED = 6
    }
}
