/********************************************************************************************
* Project Name - Utilities
* Description  - Data object of UnauthorizedException
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.100.0       12-Sept-2020  Lakshmi Narayan       Created 
********************************************************************************************/

using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Semnox.Core.Utilities
{
    /// <summary>
    /// Represents Unauthorized web-api access error that occur during application execution. 
    /// </summary>
    [Serializable]
    public class UnauthorizedException : ParafaitApplicationException, ISerializable
    {
        /// <summary>
        /// Parameterized constructor of UnauthorizedException.
        /// </summary>
        public UnauthorizedException(string message) : base(message)
        {
        }
        /// <summary>
        /// Parameterized constructor of UnauthorizedException.
        /// </summary>
        public UnauthorizedException(string message, Exception innerException) : base(message, innerException)
        {

        }

        /// <summary>
        /// Serialization Constructor
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        protected UnauthorizedException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
        }
        /// <summary>
        /// ISerializable interface implementation
        /// </summary>
        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }
            base.GetObjectData(info, context);
        }
    }
}