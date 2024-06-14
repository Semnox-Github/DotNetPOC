/********************************************************************************************
* Project Name - Utilities
* Description  - Data object of NetworkException
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.130.0     14-Jul-2021  Lakshminarayana          Created 
********************************************************************************************/

using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Semnox.Core.Utilities
{
    /// <summary>
    /// Represents Network web-api access error that occur during application execution. 
    /// </summary>
    [Serializable]
    public class NetworkException : ParafaitApplicationException, ISerializable
    {
        /// <summary>
        /// Parameterized constructor of NetworkException.
        /// </summary>
        public NetworkException(string message) : base(message)
        {
        }
        /// <summary>
        /// Parameterized constructor of NetworkException.
        /// </summary>
        public NetworkException(string message, Exception innerException) : base(message, innerException)
        {

        }

        /// <summary>
        /// Serialization Constructor
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        protected NetworkException(SerializationInfo info, StreamingContext context) 
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