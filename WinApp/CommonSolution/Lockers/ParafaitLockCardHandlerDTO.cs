/********************************************************************************************
 * Project Name - Parafait Locker Lock DTO
 * Description  - Data object of parafait locker lock DTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        17-Apr-2017   Raghuveera    Created 
 *2.130.00    31-Aug-2018   Dakshakh raj  Modified as part of Metra locker integration
 *2.150.3     30-Jun-2023   Abhishek      Modified as part of Hecere locker integration
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Device.Lockers
{
    /// <summary>
    /// Data object of parafait locker lock
    /// </summary>
    public class ParafaitLockCardHandlerDTO
    {
        /// <summary>
        /// Locker types 
        /// </summary>
        public enum LockerMake
        {
            /// <summary>
            /// Cocy locker
            /// </summary>
            COCY,
            /// <summary>
            /// Innovate locker
            /// </summary>
            INNOVATE,
            /// <summary>
            /// Passtech locker
            /// </summary>
            PASSTECH,
            /// <summary>
            /// Metra ELS locker
            /// </summary>
            METRA_ELS,
            /// <summary>
            /// Metra ELS NET locker
            /// </summary>
            METRA_ELS_NET,
            /// <summary>
            /// HECERE locker
            /// </summary>
            HECERE,
            /// <summary>
            /// None of the locker type in the list
            /// </summary>
            NONE
        }

        /// <summary>
        /// Locker selection mode
        /// </summary>
        public enum LockerSelectionMode
        {
            /// <summary>
            /// Fixed or assigned mode
            /// </summary>
            FIXED,
            /// <summary>
            /// Free mode.
            /// </summary>
            FREE
        }              
    }
}
