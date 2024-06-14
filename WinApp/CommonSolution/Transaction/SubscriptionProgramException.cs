/********************************************************************************************
 * Project Name - SubscriptionProgramException BL
 * Description  -Subscription program exceptions
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************    
 *2.130.14     30-Sep-2023    Guru S A           Subscription declien issue fixes
 ********************************************************************************************/

using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// SubscriptionProgramException
    /// </summary>
    public class SubscriptionProgramException : ParafaitApplicationException, ISerializable
    {
        private string errorCode;
        /// <summary>
        /// Get ErrorCode
        /// </summary>
        public string ErrorCode { get { return errorCode; } } 
        /// <summary>
        /// SubscriptionProgramException constructor
        /// </summary>
        /// <param name="message"></param>
        public SubscriptionProgramException(string message) : base(message)
        {
        }
        /// <summary>
        /// SubscriptionProgramException  constructor
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public SubscriptionProgramException(string message, Exception innerException) : base(message, innerException)
        {
        }
        /// <summary>
        /// SubscriptionProgramException  constructor
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected SubscriptionProgramException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
        /// <summary>
        /// Custom constructor SubscriptionProgramException
        /// </summary> 
        public SubscriptionProgramException(string errorCode, string message, Exception innerException) : base(message, innerException)
        {
            this.errorCode = errorCode;
        }
    }
    /// <summary>
    /// SubscriptionExceptionCode
    /// </summary>
    public class SubscriptionExceptionCode
    {
        /// <summary>
        /// ERROR_CODE_CLEAR_SESSION
        /// </summary>
        public static string ERROR_CODE_CLEAR_SESSION = "SBP_CODE_001_CLEAR_SUBSCRIPTION_BILL_TRX_SESSION";
        /// <summary>
        /// ERROR_CODE_CANCEL_TRX
        /// </summary>
        public static string ERROR_CODE_CANCEL_TRX = "SBP_CODE_002_CANCEL_SUBSCRIPTION_BILL_TRX";
        /// <summary>
        /// ERROR_CODE_REVERSE_TRX
        /// </summary>
        public static string ERROR_CODE_REVERSE_TRX = "SBP_CODE_003_REVERSE_SUBSCRIPTION_BILL_TRX";
    }
}
