/********************************************************************************************
 * Project Name - GenericUtilities                                                                       
 * Description  - MessageQueueStatus for message status update
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 ********************************************************************************************* 
  *2.110.0     01-Feb-2021   Girish Kundar     Created : Urban Piper changes
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.GenericUtilities
{
    public enum MessageQueueStatus
    {
        /// <summary>
        /// Read
        /// </summary>
        Read,
        /// <summary>
        /// UnRead
        /// </summary>
        UnRead,
        /// <summary>
        /// Success
        /// </summary>
        Success,
        /// <summary>
        /// Error
        /// </summary>
        Error,
        /// <summary>
        /// Processing
        /// </summary>
        Processing,
        /// <summary>
        /// Pending
        /// </summary>
        Pending,
        /// <summary>
        /// Failed
        /// </summary>
        Failed,
        /// <summary>
        /// Processing
        /// </summary>
        HQProcessing,
        /// <summary>
        /// Pending
        /// </summary>
        HQPending,
        /// <summary>
        /// Failed
        /// </summary>
        HQFailed
    }
}
