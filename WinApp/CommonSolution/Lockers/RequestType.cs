using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Device.Lockers
{
    public enum RequestType
    {
        /// <summary>
        /// Opening the lock
        /// </summary>
        OPEN_LOCK,
        /// <summary>
        /// Open All Lock
        /// </summary>
        OPEN_ALL_LOCK,
        /// <summary>
        /// Block the lock
        /// </summary>
        BLOCK_LOCK,
        /// <summary>
        /// Block card
        /// </summary>
        BLOCK_CARD,
        /// <summary>
        /// Unblock the lock
        /// </summary>
        UNBLOCK_LOCK,
        /// <summary>
        /// Unblock card
        /// </summary>
        UNBLOCK_CARD
    }
}
