using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Device.PaymentGateway
{
    /// <summary>
    /// 
    /// </summary>
    public enum eWicMode : int
    {
        /// <summary>
        /// 
        /// </summary>
        NORMAL = 0,
        /// <summary>
        /// 
        /// </summary>
        TRAINING = 1,
        /// <summary>
        /// 
        /// </summary>
        CERTIFICATION = 2,
        /// <summary>
        /// 
        /// </summary>
        CERT_OR_TRAINING = 3,
    }
}
