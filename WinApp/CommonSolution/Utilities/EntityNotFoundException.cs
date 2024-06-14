/********************************************************************************************
 * Project Name - Utilities
 * Description  - Exception class for error while searching record by using id :(used in BL) 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *2.70        03-Jul-2019   Lakshminarayan     Created: 
 ********************************************************************************************/
using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Semnox.Core.Utilities
{
    /// <summary>
    /// Represents Entity not found error that occur during application execution.
    /// </summary>
    [Serializable]
    public class EntityNotFoundException : ParafaitApplicationException, ISerializable
    {
        /// <summary>
        /// Parameterized constructor of ParafaitApplicationException.
        /// </summary>
        public EntityNotFoundException(string message):base(message)
        {

        }
        
        /// <summary>
        /// Parameterized constructor of ParafaitApplicationException.
        /// </summary>
        public EntityNotFoundException(string message, Exception innerException):base(message, innerException)
        {

        }

        /// <summary>
        /// Serialization Constructor
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        protected EntityNotFoundException(SerializationInfo info, StreamingContext context) 
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
