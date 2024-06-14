using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// Order Types
    /// </summary>
    public enum OrderStatus
    {
        /// <summary>
        /// Open
        /// </summary>
        OPEN,
        /// <summary>
        /// Complete
        /// </summary>
        COMPLETE,
        /// <summary>
        /// Cancelled
        /// </summary>
        CANCELLED,
    }
}

