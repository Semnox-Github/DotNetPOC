using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Device.PaymentGateway
{
    public class WebRequestHandler
    {
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int timeOut;
        public int ReadWriteTimeout { get { return timeOut; } set { timeOut = value; log.LogVariableState("ReadWriteTimeout", timeOut); } }
        public WebRequestHandler()
        {
            log.LogMethodEntry();
            timeOut = 100;
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates the web request object by using the passed parameters
        /// </summary>
        /// <param name="url">Is the Endpoint url</param>
        /// <param name="data">Data which need to be transafered with request</param>
        /// <param name="contentType"> content type like application/json or text/xml etc</param>
        /// <param name="method"> method like POST,PUT, GET etc</param>
        /// <param name="authorization">this value will be added to the header key value 'Authorization'</param>
        /// <returns> returns the request which is created using passed parameter</returns>
        public HttpWebRequest CreateRequest(string url, string contentType, string method, string authorization)
        {
            log.LogMethodEntry(url, contentType, authorization);
            log.LogVariableState("url", url);
            log.LogVariableState("contentType", contentType);
            log.LogVariableState("method", method);
            log.LogVariableState("authorization", authorization);                       
            HttpWebRequest request =
                       (HttpWebRequest)WebRequest.Create(url);
            request.Method = method;
            request.ContentType = contentType;                      
            request.ReadWriteTimeout = ReadWriteTimeout;
            request.Headers["Authorization"] = authorization;
            log.LogMethodExit(request);
            return request;
        }
        /// <summary>
        /// Creates the web request object by using the passed parameters. 
        /// The username and password will be converted in to base64 format and prefixed with the word 'Basic' and added to the header key value 'Authorization'
        /// </summary>
        /// <param name="url">Is the Endpoint url</param>
        /// <param name="contentType">content type like application/json or text/xml etc</param>
        /// <param name="method"> method like POST,PUT, GET etc</param>
        /// <param name="userName">user name</param>
        /// <param name="password"> password</param>
        /// <returns></returns>
        public HttpWebRequest CreateRequest(string url, string contentType, string method, string userName, string password)
        {
            log.LogMethodEntry();
            log.LogVariableState("url", url);
            log.LogVariableState("contentType", contentType);
            log.LogVariableState("method", method);
            log.LogVariableState("userName", userName);
            log.LogVariableState("password", password);
            var plainTextBytes = System.Text.Encoding.ASCII.GetBytes(userName+":"+ password);
            string credentials = Convert.ToBase64String(plainTextBytes);
            log.LogMethodExit();
            return CreateRequest(url, contentType, method, new AuthenticationHeaderValue("Basic", credentials).ToString());            
        }

        /// <summary>
        /// Creates the web request object by using the passed parameters. 
        /// The username and password will be converted in to base64 format and prefixed with the word 'Basic' and added to the header key value 'Authorization'
        /// </summary>
        /// <param name="url">Is the Endpoint url</param>
        /// <param name="contentType">content type like application/json or text/xml etc</param>
        /// <param name="method"> method like POST,PUT, GET etc</param>
        /// <param name="credential">user will send the credential and that will be set to the request.Credentials value</param>
        /// <returns></returns>
        public HttpWebRequest CreateRequest(string url, string contentType, string method, NetworkCredential credential)
        {
            log.LogMethodEntry();
            log.LogVariableState("url", url);
            log.LogVariableState("contentType", contentType);
            log.LogVariableState("method", method);
            log.LogVariableState("credential", credential);
            HttpWebRequest request =
                       (HttpWebRequest)WebRequest.Create(url);
            request.Method = method;
            request.ContentType = contentType;
            request.ReadWriteTimeout = ReadWriteTimeout;
            request.Credentials = credential;
            log.LogMethodExit(request);
            return request;
        }
        /// <summary>
        /// Will add the passed key and value to the header
        /// </summary>
        /// <param name="request">HttpWebRequest type object</param>
        /// <param name="key">Key name the of value should be added</param>
        /// <param name="value">Value to add to the header </param>
        public void AddToHeader(HttpWebRequest request, string key, string value)
        {
            log.LogMethodEntry();            
            request.Headers.Add(key, value);
            log.LogMethodExit();
        }

        /// <summary>
        /// Will add the passed key and value to the header
        /// </summary>
        /// <param name="response">HttpWebResponse type object</param>
        /// <param name="key">Key name the of value should be added</param>
        /// <returns>value of passed key</returns>
        public string GetHeaderKeyValue(HttpWebResponse response, string key)
        {
            log.LogMethodEntry();
            log.LogMethodExit();
            if (response.Headers.AllKeys.Contains(key))
                return response.Headers[key];
            else
                return null;
        }

        /// <summary>
        /// sends the web request for passed parameter
        /// </summary>
        /// <param name="request">HttpWebRequest type object</param>
        /// <param name="data">Data which need to be transafered with request</param>
        /// <returns>HttpWebResponse object will be returned</returns>
        public HttpWebResponse SendRequest(HttpWebRequest request, string data)
        {
            log.LogMethodEntry();
            log.LogVariableState("data", data);
            if (!string.IsNullOrEmpty(data))
            {
                byte[] byteArray = Encoding.UTF8.GetBytes(data);
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream. 
                dataStream.Write(byteArray, 0, byteArray.Length);
            }
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();               
            log.LogMethodExit(response);
            return response;
        }
        /// <summary>
        /// Accepts the web response and if the receiving data in json format
        /// </summary>
        /// <param name="response">HttpWebResponse type object</param>
        /// <returns></returns>
        public string GetJsonData(HttpWebResponse response)
        {
            log.LogMethodEntry();
            log.LogVariableState("response", response);
            MemoryStream ms = new MemoryStream();
            response.GetResponseStream().CopyTo(ms);
            string data = Encoding.ASCII.GetString(ms.ToArray());
            log.LogMethodExit(data);
            return data;
        }
    }
}
