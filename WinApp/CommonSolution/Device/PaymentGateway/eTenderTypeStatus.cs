using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Semnox.Parafait.Device.PaymentGateway
{
    /// <summary>
    /// 
    /// </summary>
    public enum eTenderTypeStatus : int
    {
        /// <summary>
        /// 
        /// </summary>
        LOOP = 0,
        /// <summary>
        /// 
        /// </summary>
        OK = 1,
        /// <summary>
        /// 
        /// </summary>
        MISMATCH = 2,
        /// <summary>
        /// 
        /// </summary>
        ERROR_SCAT_DEAD = 3,
        /// <summary>
        /// 
        /// </summary>
        BALANCE_INQUIRY_REQUEST = 4,
        /// <summary>
        /// 
        /// </summary>
        PIN_CHANGE_REQUEST = 5,
        /// <summary>
        /// 
        /// </summary>
        CANCEL = 6,
        /// <summary>
        /// 
        /// </summary>
        HOST_DOWN = 7,
        /// <summary>
        /// 
        /// </summary>
        RESERVED = 8 | 9
    }
}
