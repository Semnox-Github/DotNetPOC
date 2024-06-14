/********************************************************************************************
* Project Name - JobUtils
* Description  - 
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.160.0     17-Jul-2022   Deeksha                 Created 
********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Semnox.Parafait.JobUtils
{
    [Serializable]
    /// <summary>
    /// Represents OrderStatusIdNotFoundException error that occur during parafait application execution. 
    /// </summary>
    public class AnotherInstanceOfProgramIsRunningException : ParafaitApplicationException, ISerializable
    {
        /// <summary>
        /// Parameterized constructor of OrderStatusIdNotFoundException.
        /// </summary>
        public AnotherInstanceOfProgramIsRunningException(string message) : base(message)
        {
        }
        /// <summary>
        /// Parameterized constructor of OrderStatusIdNotFoundException.
        /// </summary>
        public AnotherInstanceOfProgramIsRunningException(string message, Exception innerException) : base(message, innerException)
        {

        }

        /// <summary>
        /// Serialization Constructor
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        protected AnotherInstanceOfProgramIsRunningException(SerializationInfo info, StreamingContext context)
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
