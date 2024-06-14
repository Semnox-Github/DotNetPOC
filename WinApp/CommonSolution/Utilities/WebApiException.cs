/********************************************************************************************
* Project Name - Utilities
* Description  - Data object of WebApiException
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.70.2        12-Nov-2019   Lakshminarayana           Created 
*2.100.0       12-Sept-2020  Girish Kundar             Modified : Created a copy in the Utilitiy project for POS UI redesign 
********************************************************************************************/

using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Semnox.Core.Utilities
{
    [Serializable]
    /// <summary>
    /// Represents Unauthorized web-api access error that occur during application execution. 
    /// </summary>
    public class WebApiException : ParafaitApplicationException, ISerializable
    {
        private readonly WebApiResponse webApiResponse;
        /// <summary>
        /// Parameterized constructor of WebApiException.
        /// </summary>
        public WebApiException(string message, WebApiResponse webApiResponse):base(message)
        {
            this.webApiResponse = webApiResponse;
        }

        /// <summary>
        /// Get method of webApiResponse
        /// </summary>
        public WebApiResponse WebApiResponse
        {
            get { return webApiResponse; }
        }

        /// <summary>
        /// Serialization constructor
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        protected WebApiException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
             webApiResponse = (WebApiResponse)info.GetValue("webApiResponse", typeof(WebApiResponse));
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }
            info.AddValue("webApiResponse", webApiResponse, typeof(WebApiResponse));
            base.GetObjectData(info, context);
        }
    }
}