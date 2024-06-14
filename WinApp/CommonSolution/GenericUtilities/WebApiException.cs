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
********************************************************************************************/

using System;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// Represents Unauthorized web-api access error that occur during application execution. 
    /// </summary>
    public class WebApiException : Exception
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
    }
}