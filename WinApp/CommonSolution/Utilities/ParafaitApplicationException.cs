/********************************************************************************************
* Project Name - Utilities
* Description  - Data object of UnauthorizedException
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.100.0     12-Nov-2020   Lakshminarayana         Created 
********************************************************************************************/

using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Semnox.Core.Utilities
{
    [Serializable]
    /// <summary>
    /// Represents ParafaitApplicationException error that occur during parafait application execution. 
    /// </summary>
    public class ParafaitApplicationException : Exception, ISerializable
    {
        /// <summary>
        /// Parameterized constructor of ParafaitApplicationException.
        /// </summary>
        public ParafaitApplicationException(string message) : base(message)
        {
        }
        /// <summary>
        /// Parameterized constructor of ParafaitApplicationException.
        /// </summary>
        public ParafaitApplicationException(string message, Exception innerException) : base(message, innerException)
        {

        }

        /// <summary>
        /// Serialization Constructor
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        protected ParafaitApplicationException(SerializationInfo info, StreamingContext context) 
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