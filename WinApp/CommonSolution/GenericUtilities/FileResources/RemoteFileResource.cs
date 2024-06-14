/********************************************************************************************
 * Project Name -GenericUtilities
 * Description  -RemoteFileResource class to get and store files 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.140.00     21-Sep-2021       Lakshminarayana           Created
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Semnox.Core.Utilities;

namespace Semnox.Core.GenericUtilities.FileResources
{
    public class RemoteFileResource : FileResource
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string FILE_RESOURCE_URL = "api/Common/FileResource";
        private const string FILE_RESOURCE_HASH_URL = "api/Common/FileResourceHash";
        private static readonly HttpClient client;
        private readonly string webApiUrl;
        private readonly string origin;

        static RemoteFileResource()
        {
            log.LogMethodEntry();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12;
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            client = new HttpClient()
            {
                Timeout = System.Threading.Timeout.InfiniteTimeSpan
            };
            log.LogMethodExit();
        }

        public RemoteFileResource(ExecutionContext executionContext, string defaultValueName, string fileName, bool secure, string webApiUrl, string origin) :
            base(executionContext, defaultValueName, fileName, secure)
        {
            log.LogMethodEntry(defaultValueName, fileName, secure, webApiUrl);
            this.webApiUrl = webApiUrl;
            this.origin = origin;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get the file
        /// </summary>
        /// <returns></returns>
        public async override Task<Stream> Get()
        {
            try
            {
                log.LogMethodEntry(defaultValueName);
                Stream result;
                WebApiGetRequestParameterCollection parameters = new WebApiGetRequestParameterCollection("defaultValueName", defaultValueName, "fileName", fileName, "secure", secure);
                string url = GetUrl(FILE_RESOURCE_URL, parameters);
                List<KeyValuePair<string, string>> headers = GetHeaders();  
                var requestMessage = new HttpRequestMessage(HttpMethod.Get, webApiUrl + url);
                foreach (KeyValuePair<string, string> header in headers)
                {
                    if (string.IsNullOrWhiteSpace(header.Value) || string.IsNullOrWhiteSpace(header.Key))
                    {
                        continue;
                    }
                    requestMessage.Headers.Add(header.Key, header.Value);
                }
                HttpResponseMessage httpResponseMessage = await client.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead);
                result = await httpResponseMessage.Content.ReadAsStreamAsync();
                if (httpResponseMessage.StatusCode == HttpStatusCode.NotFound)
                {
                    log.Debug("File Not found");
                    return null;
                }
                if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
                {
                    WebApiResponse webApiResponse = new WebApiResponse(httpResponseMessage.StatusCode, result.ToString(), httpResponseMessage.Headers);
                    ThrowIfInvalidWebResponse(webApiResponse);
                }
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
        }

        private List<KeyValuePair<string, string>> GetHeaders()
        {
            log.LogMethodEntry();
            List<KeyValuePair<string, string>> headers = new List<KeyValuePair<string, string>>
            {
                GetOriginHeader(), GetAuthorizationHeader()
            };
            log.LogMethodExit(headers);
            return headers;
        }

        private KeyValuePair<string, string> GetOriginHeader()
        {
            KeyValuePair<string, string> originHeader = new KeyValuePair<string, string>("Origin", origin);
            return originHeader;
        }

        private KeyValuePair<string, string> GetAuthorizationHeader()
        {
            log.LogMethodEntry();
            string authorization = GetTokenFromExecutionContext();
            KeyValuePair<string, string> authorizationHeader = new KeyValuePair<string, string>("Authorization", authorization);
            log.LogMethodExit(authorizationHeader);
            return authorizationHeader;
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

        public override async Task<string> GetHash()
        {
            log.LogMethodEntry();
            RemoteUseCases remoteUseCases = new RemoteUseCases(executionContext);
            string result = await remoteUseCases.Get<string>(FILE_RESOURCE_HASH_URL, new WebApiGetRequestParameterCollection("defaultValueName",
                                                                                                                defaultValueName,
                                                                                                                "fileName",
                                                                                                                fileName,
                                                                                                                "secure",
                                                                                                                secure));
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// To get the local file path
        /// </summary>
        /// <param name="inputStream"></param>
        /// <returns></returns>
        public override async Task<string> GetLocalPath()
        {
            log.LogMethodEntry();
            string result = string.Empty;
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// save the file
        /// </summary>
        /// <param name="inputStream"></param>
        /// <returns></returns>
        public override async Task<bool> Save(Stream inputStream)
        {
            try
            {
                log.LogMethodEntry("inputStream");
                bool result = false;
                WebApiGetRequestParameterCollection parameters = new WebApiGetRequestParameterCollection("defaultValueName", defaultValueName, "fileName", fileName, "secure", secure);
                string url = GetUrl(FILE_RESOURCE_URL, parameters);
                List<KeyValuePair<string, string>> headers = GetHeaders();
                using (var requestMessage = new HttpRequestMessage(HttpMethod.Post, webApiUrl + url))
                {
                    foreach (KeyValuePair<string, string> header in headers)
                    {
                        if (string.IsNullOrWhiteSpace(header.Value) || string.IsNullOrWhiteSpace(header.Key))
                        {
                            continue;
                        }
                        requestMessage.Headers.Add(header.Key, header.Value);
                    }
                    requestMessage.Content = new StreamContent(inputStream);
                    using (HttpResponseMessage httpResponseMessage = await client.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead))
                    {
                        await httpResponseMessage.Content.ReadAsStringAsync();
                        if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
                        {
                            WebApiResponse webApiResponse = new WebApiResponse(httpResponseMessage.StatusCode, result.ToString(), httpResponseMessage.Headers);
                            ThrowIfInvalidWebResponse(webApiResponse);
                        }
                        result = true;
                    }
                }
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
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

        private void ThrowIfInvalidWebResponse(WebApiResponse result)
        {
            log.LogMethodEntry(result);
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
                        serverException = (Exception)JsonConvert.DeserializeObject(serializedException, typeof(Exception));
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
