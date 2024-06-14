/********************************************************************************************
* Project Name - Utilities
* Description  - Base class of Remote use cases. implements helper methods for calling parafait WEB API 
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.110.0        12-Nov-2019   Lakshminarayana           Created 
********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Semnox.Core.Utilities
{
    public class RemoteUseCases
    {
        private static readonly Parafait.logging.Logger log = new Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected readonly ExecutionContext executionContext;
        private readonly string origin;
        private readonly string webApiUrl;
        private static readonly HttpClient client;
        private readonly string requestGuid;
        private int retryCount = 0;
        private static readonly int MAX_API_REQUEST_RETRIES = 3;
        static RemoteUseCases()
        {
            log.LogMethodEntry();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12;
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            client = new HttpClient()
            {
                Timeout = Timeout.InfiniteTimeSpan
            };
            string maxRetiesConfig = ConfigurationManager.AppSettings["MAX_API_REQUEST_RETRIES"];
            int maxRetriesValue;
            if (int.TryParse(maxRetiesConfig, out maxRetriesValue))
            {
                MAX_API_REQUEST_RETRIES = maxRetriesValue;
            }
            log.LogMethodExit();
        }

        public RemoteUseCases()
            : this(CreateRequestGuid())
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        public RemoteUseCases(string requestGuid)
        {
            log.LogMethodEntry(requestGuid);
            origin = ConfigurationManager.AppSettings["WEB_API_ORIGIN_KEY"];
            webApiUrl = ConfigurationManager.AppSettings["WEB_API_URL"];
            this.requestGuid = requestGuid;
            log.LogMethodExit();
        }

        public RemoteUseCases(ExecutionContext executionContext)
            : this(CreateRequestGuid())
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        public RemoteUseCases(ExecutionContext executionContext, string requestGuid)
            : this(requestGuid)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        private static string CreateRequestGuid()
        {
            log.LogMethodEntry();
            string result = Guid.NewGuid().ToString();
            log.LogMethodExit(result);
            return result;
        }

        private async Task<WebApiResponse> SendData(HttpMethod httpMethod, string url, List<KeyValuePair<string, string>> headers, string content = null)
        {
            log.LogMethodEntry();
            log.Debug("SendData start: " + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss fff"));
            WebApiResponse result = null;
            while (true)
            {
                try
                {
                    result = await SendDataImpl(httpMethod, url, headers, content);
                    break;
                }
                catch (Exception ex)
                {
                    log.Error("Error occured while sending the request", ex);
                    if (retryCount == MAX_API_REQUEST_RETRIES || string.IsNullOrWhiteSpace(requestGuid))
                    {
                        string errorMessage = "Error occurred while communicating with Web-Api";
                        log.Error(errorMessage, ex);
                        result = new WebApiResponse(errorMessage, ex, true);
                        break;
                    }
                    retryCount++;
                }
            }
            log.Debug("SendData end: " + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss fff"));
            log.LogMethodExit(result);
            return result;
        }

        private async Task<WebApiResponse> SendDataImpl(HttpMethod httpMethod, string url, List<KeyValuePair<string, string>> headers, string content)
        {
            log.LogMethodEntry(httpMethod, url, headers, content);
            WebApiResponse result;
            using (var requestMessage = new HttpRequestMessage(httpMethod, url))
            {
                foreach (KeyValuePair<string, string> header in headers)
                {
                    if (string.IsNullOrWhiteSpace(header.Value) || string.IsNullOrWhiteSpace(header.Key))
                    {
                        continue;
                    }
                    requestMessage.Headers.Add(header.Key, header.Value);
                }
                log.Debug("SendData Headers added: " + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss fff"));
                if (string.IsNullOrWhiteSpace(content) == false)
                {
                    StringContent stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                    requestMessage.Content = stringContent;
                }
                log.Debug("SendData content set: " + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss fff"));
                using (HttpResponseMessage httpResponseMessage = await client.SendAsync(requestMessage))
                {
                    log.Debug("SendData Got Response : " + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss fff"));
                    string response = await httpResponseMessage.Content.ReadAsStringAsync();
                    log.Debug("SendData read Response Content: " + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss fff"));
                    result = new WebApiResponse(httpResponseMessage.StatusCode, response, httpResponseMessage.Headers);
                }
            }
            log.LogMethodExit(result);
            return result;
        }
        public async Task<string> Get(string urlPart, List<KeyValuePair<string, string>> parameters = null)
        {
            log.LogMethodEntry(urlPart, parameters);
            string url = GetUrl(urlPart, parameters);
            List<KeyValuePair<string, string>> headers = GetHeaders();
            WebApiResponse result = await SendData(HttpMethod.Get, webApiUrl + url, headers);
            ThrowIfInvalidWebResponse(result);
            log.LogMethodExit(result.Response);
            return result.Response;
        }

        private void SetTokenToExecutionContext(string token)
        {
            log.LogMethodEntry(token);
            if (executionContext != null)
            {
                executionContext.SetWebApiToken(token);
            }
            log.LogMethodExit();
        }

        private string GetTokenFromExecutionContext()
        {
            log.LogMethodEntry();
            string result = string.Empty;
            if (executionContext != null)
            {
                result = executionContext.GetWebApiToken();
            }
            log.LogMethodExit(result);
            return result;
        }

        public async Task<T> Get<T>(string urlPart, List<KeyValuePair<string, string>> parameters = null, string responseAttribute = "data")
        {
            log.LogMethodEntry(urlPart, parameters);
            RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
            string responseString = await Get(urlPart, parameters);
            T result = await DeserializeObject<T>(responseString, responseAttribute);
            log.LogMethodExit(result);
            return result;
        }

        public async Task<string> Post(string url, string content)
        {
            log.LogMethodEntry(url);
            List<KeyValuePair<string, string>> headers = GetHeaders();
            WebApiResponse result = await SendData(HttpMethod.Post, webApiUrl + url, headers, content);
            ThrowIfInvalidWebResponse(result);
            log.LogMethodExit(result.Response);
            return result.Response;
        }

        public async Task<string> Put(string url, string content)
        {
            log.LogMethodEntry(url);
            List<KeyValuePair<string, string>> headers = GetHeaders();
            WebApiResponse result = await SendData(HttpMethod.Put, webApiUrl + url, headers, content);
            ThrowIfInvalidWebResponse(result);
            log.LogMethodExit(result.Response);
            return result.Response;
        }

        public async Task<string> Delete(string url, string content)
        {
            log.LogMethodEntry(url);
            List<KeyValuePair<string, string>> headers = GetHeaders();
            WebApiResponse result = await SendData(HttpMethod.Delete, webApiUrl + url, headers, content);
            ThrowIfInvalidWebResponse(result);

            log.LogMethodExit(result.Response);
            return result.Response;
        }

        public async Task<T> Post<T>(string url, object postData, string responseAttribute = "data")
        {
            log.LogMethodEntry(url, postData);
            string content = JsonConvert.SerializeObject(postData);
            T result = await Post<T>(url, content, responseAttribute);
            return result;
        }

        public async Task<T> Post<T>(string url, string content, string responseAttribute = "data")
        {
            log.LogMethodEntry(url, content);
            RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
            string responseString = await Post(url, content);
            T result = await DeserializeObject<T>(responseString, responseAttribute);
            log.LogMethodExit(result);
            return result;
        }

        public async Task<T> Put<T>(string url, object putData, string responseAttribute = "data")
        {
            log.LogMethodEntry(url, putData, responseAttribute);
            string content = JsonConvert.SerializeObject(putData);
            T result = await Put<T>(url, content, responseAttribute);
            log.LogMethodExit(result);
            return result;
        }

        public async Task<T> Put<T>(string url, string content, string responseAttribute = "data")
        {
            log.LogMethodEntry(url, content);
            RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
            string responseString = await Put(url, content);
            T result = await DeserializeObject<T>(responseString, responseAttribute);
            log.LogMethodExit(result);
            return result;
        }

        public async Task<T> Delete<T>(string url, string content, string responseAttribute = "data")
        {
            log.LogMethodEntry(url, content);
            RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
            string responseString = await Delete(url, content);
            T result = await DeserializeObject<T>(responseString, responseAttribute);
            log.LogMethodExit(result);
            return result;
        }

        public async Task<T> Delete<T>(string url, object deleteData, string responseAttribute = "data")
        {
            log.LogMethodEntry(url, deleteData, responseAttribute);
            string content = JsonConvert.SerializeObject(deleteData);
            T result = await Delete<T>(url, content, responseAttribute);
            log.LogMethodExit(result);
            return result;
        }

        protected async Task<T> DeserializeObject<T>(string responseString, string responseAttribute)
        {
            return await Task<T>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(responseString);
                T result;
                try
                {
                    JObject responseObject = (JObject)JsonConvert.DeserializeObject(responseString);
                    if (string.IsNullOrWhiteSpace(responseAttribute))
                    {
                        result = responseObject.ToObject<T>();
                    }
                    else
                    {
                        result = responseObject[responseAttribute].ToObject<T>();
                    }

                }
                catch (Exception ex)
                {
                    string errorMessage = "Invalid Response. Unable to convert the response to " + typeof(T) + ".";
                    log.LogMethodExit("Throwing Exception -" + errorMessage);
                    throw new InvalidResponseException(errorMessage, ex);
                }

                log.LogMethodExit(result);
                return result;
            });
        }

        private void ThrowIfInvalidWebResponse(WebApiResponse result)
        {
            log.LogMethodEntry(result);
            if (result.Success == false)
            {
                if (result.IsNetworkError)
                {
                    SetTokenToExecutionContext(string.Empty);
                    string message = "Unable to connect to server : " + result.GetServerErrorMessage();
                    log.LogMethodExit(null, "Throwing NetworkException - " + message);
                    throw new NetworkException(message);
                }
                log.LogMethodExit(null, "Throwing WebApiException - " + result.ErrorMessage);
                throw new WebApiException(result.ErrorMessage, result);
            }
            if (result.StatusCode == HttpStatusCode.Unauthorized)
            {
                SetTokenToExecutionContext(string.Empty);
                string message = "Unauthorized : " + result.GetServerErrorMessage();
                log.LogMethodExit(null, "Throwing UnauthorizedException - " + message);
                throw new UnauthorizedException(message);
            }
            if (result.StatusCode == HttpStatusCode.BadRequest ||
                result.StatusCode == HttpStatusCode.InternalServerError)
            {
                Exception serverException;
                try
                {
                    JObject responseObject = (JObject)JsonConvert.DeserializeObject(result.Response);
                    if (responseObject.ContainsKey("exception"))
                    {
                        string serializedException = responseObject["exception"].ToString();
                        string className = ((JObject)JsonConvert.DeserializeObject(serializedException))["ClassName"].ToString();
                        serverException = (Exception)JsonConvert.DeserializeObject(serializedException, TypeResolver.GetType(className, typeof(Exception)));
                    }
                    else if (responseObject.ContainsKey("data"))
                    {
                        string errorMessage = responseObject["data"].ToString();
                        serverException = new WebApiException(errorMessage, result);
                    }
                    else
                    {
                        serverException = new WebApiException(result.Response, result);
                    }
                }
                catch (Exception ex)
                {
                    string errorMessage;
                    try
                    {
                        JObject responseObject = (JObject)JsonConvert.DeserializeObject(result.Response);
                        errorMessage = responseObject["data"].ToString();
                    }
                    catch (Exception)
                    {
                        errorMessage = result.Response;
                    }

                    log.Error(errorMessage, ex);
                    log.LogMethodExit(null, "Throwing WebApiException - " + errorMessage);
                    throw new WebApiException(errorMessage, result);
                }
                log.LogMethodExit(null, "BadRequest Throwing Exception - " + serverException.Message);
                if (serverException is NetworkException)
                {
                    SetTokenToExecutionContext(string.Empty);
                }
                throw serverException;
            }
            if (result.StatusCode != HttpStatusCode.OK)
            {
                string message = result.GetServerErrorMessage();
                log.LogMethodExit(null, "Throwing WebApiException - " + message);
                throw new WebApiException(message, result);
            }
            log.LogMethodExit();
        }

        private List<KeyValuePair<string, string>> GetHeaders()
        {
            log.LogMethodEntry();
            List<KeyValuePair<string, string>> headers = new List<KeyValuePair<string, string>>
            {
                GetOriginHeader(), GetAuthorizationHeader(), GetRequestGuidHeader()
            };
            log.LogMethodExit(headers);
            return headers;
        }

        private KeyValuePair<string, string> GetRequestGuidHeader()
        {
            log.LogMethodEntry();
            KeyValuePair<string, string> authorizationHeader = new KeyValuePair<string, string>("RequestIdentifier", requestGuid);
            log.LogMethodExit(authorizationHeader);
            return authorizationHeader;
        }

        private KeyValuePair<string, string> GetAuthorizationHeader()
        {
            log.LogMethodEntry();
            string authorization = GetTokenFromExecutionContext();
            KeyValuePair<string, string> authorizationHeader = new KeyValuePair<string, string>("Authorization", authorization);
            log.LogMethodExit(authorizationHeader);
            return authorizationHeader;
        }

        private KeyValuePair<string, string> GetOriginHeader()
        {
            KeyValuePair<string, string> originHeader = new KeyValuePair<string, string>("Origin", origin);
            return originHeader;
        }

        private string GetUrl(string url, List<KeyValuePair<string, string>> parameters)
        {
            log.LogMethodEntry(url, parameters);

            string result = url;

            if (parameters != null && parameters.Any())
            {
                result += "?" + string.Join("&", parameters.Select(x => string.Format("{0}={1}", HttpUtility.UrlEncode(x.Key), HttpUtility.UrlEncode(x.Value))));
            }

            log.LogMethodExit(result);
            return result;
        }
    }
}
