/********************************************************************************************
* Project Name - Generic Utilities
* Description  - Represents a response from web api call
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.70.2        14-Nov-2019   Lakshminarayana           Created 
********************************************************************************************/
using System;
using System.Net;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using System.Linq;
namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// Represents a response from web api call
    /// </summary>
    [DataContract]
    public class WebApiResponse
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool success;
        private HttpStatusCode statusCode;
        private string response;
        private string errorMessage;
        private string exceptionType;
        private string exceptionStackTrace;
        private string token;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="response"></param>
        public WebApiResponse(HttpStatusCode statusCode, string response)
        {
            log.LogMethodEntry(statusCode, response);
            success = true;
            this.statusCode = statusCode;
            this.response = response;
            errorMessage = string.Empty;
            exceptionType = string.Empty;
            exceptionStackTrace = string.Empty;
            token = string.Empty;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="response"></param>
        public WebApiResponse(HttpStatusCode statusCode, string response, HttpResponseHeaders httpResponseHeaders)
        {
            log.LogMethodEntry(statusCode, response);
            success = true;
            this.statusCode = statusCode;
            this.response = response;
            errorMessage = string.Empty;
            exceptionType = string.Empty;
            exceptionStackTrace = string.Empty;
            foreach (var responseHeader in httpResponseHeaders)
            {
                if(responseHeader.Key.ToLower() == "authorization")
                {
                    token = responseHeader.Value.FirstOrDefault();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="ex"></param>
        public WebApiResponse(Exception ex)
        {
            log.LogMethodEntry(ex);
            success = false;
            errorMessage = ex.Message;
            exceptionType = ex.GetType().FullName;
            exceptionStackTrace = ex.StackTrace;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public WebApiResponse(string message, Exception ex)
        {
            log.LogMethodEntry(message, ex);
            success = false;
            errorMessage = message + Environment.NewLine + ex.Message;
            exceptionType = ex.GetType().FullName;
            exceptionStackTrace = ex.StackTrace;
            token = string.Empty;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get method of success field
        /// </summary>
        [DataMember]
        public bool Success
        {
            get { return success; }
            set { success = value; }
        }

        /// <summary>
        /// Get/Set method of statusCode field
        /// </summary>
        [DataMember]
        public HttpStatusCode StatusCode
        {
            get { return statusCode; }
            set { statusCode = value; }
        }

        /// <summary>
        /// Get/Set method of response field
        /// </summary>
        [DataMember]
        public string Response
        {
            get { return response; }
            set { response = value; }
        }

        /// <summary>
        /// Get/Set method of errorMessage field
        /// </summary>
        [DataMember]
        public string ErrorMessage
        {
            get { return errorMessage; }
            set { errorMessage = value; }
        }

        /// <summary>
        /// Get/Set method of exceptionType field
        /// </summary>
        [DataMember]
        public string ExceptionType
        {
            get { return exceptionType; }
            set { exceptionType = value; }
        }

        /// <summary>
        /// Get/Set method of exceptionStackTrace field
        /// </summary>
        [DataMember]
        public string ExceptionStackTrace
        {
            get { return exceptionStackTrace; }
            set { exceptionStackTrace = value; }
        }

        /// <summary>
        /// Get/Set method of token field
        /// </summary>
        [DataMember]
        public string Token
        {
            get { return token; }
            set { token = value; }
        }

        /// <summary>
        /// Returns the server error message
        /// </summary>
        /// <returns></returns>
        public string GetServerErrorMessage()
        {
            log.LogMethodEntry();
            string message = string.Empty;
            if (string.IsNullOrWhiteSpace(response))
            {
                message = "Unknown server error. server response is empty.";
                log.LogMethodExit(message);
                return message;
            }

            try
            {
                dynamic obj = JsonConvert.DeserializeObject(response);
                if (obj != null)
                {
                    message = obj["data"];
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occured while retrieving server error message from the response.", ex);
            }

            if (string.IsNullOrWhiteSpace(message))
            {
                message = "Error occured while retrieving server error message from the response.";
            }
            log.LogMethodExit(message);
            return message;
        }

        public string GetAuthenticationToken()
        {
            log.LogMethodEntry();
            string result = token;
            if(string.IsNullOrWhiteSpace(result) == false)
            {
                log.LogMethodExit(result, "token from header");
                return result;
            }
            if (string.IsNullOrWhiteSpace(response))
            {
                log.LogMethodExit(result, "Response is empty.");
                return result;
            }

            //try
            //{
            //    var groups = Regex.Match(response, "\"token\":\"(?<token>.*)\"").Groups;
            //    if (groups.Count > 0 && groups["token"].Success &&
            //        string.IsNullOrWhiteSpace(groups["token"].Value) == false)
            //    {
            //        token = groups["token"].Value;
            //    }
            //    else
            //    {
            //        log.Error("Unable to find the token is the server response.");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    log.Error("Error occured while parsing the token from the server response", ex);
            //}
            try
            {
                dynamic obj = JsonConvert.DeserializeObject(response);
                if (obj != null)
                {
                    result = obj["token"];
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occured while retrieving token from the response.", ex);
            }
            log.LogMethodExit(result);
            return result;
        }
    }
}
