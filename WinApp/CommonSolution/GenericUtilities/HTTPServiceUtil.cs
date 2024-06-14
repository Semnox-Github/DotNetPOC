/********************************************************************************************
 * Project Name - GenericUtilities
 * Description  - Helper class for calling REST API
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By            Remarks          
 *********************************************************************************************
 *2.90.0           14-Jul-2020      Gururaja Kanjan    Helper class for calling REST API
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Core.GenericUtilities
{

	public class HTTPServiceUtil
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// parameterized constructor with routeUri
        /// </summary>
        /// <param name="executionContext"></param>
        public HTTPServiceUtil(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// This method is for getting a records from a WEB API using GET method<para/>
        /// This will fetch the records based on the query filter provided in requestUri
        /// </summary>
        /// <param name="requestUri"></param>
        /// <param name="sessionToken"></param>
        /// <param name="host"></param>
        /// <param name="readWriteTimeout"></param>
        /// <returns></returns>
        public string Get(string requestUri, string sessionToken = "", string host = "", int readWriteTimeout = 10)
        {
            log.LogMethodEntry(requestUri, sessionToken, host, readWriteTimeout);
            string jsonResponse = string.Empty;
            try
            {
                //Creates the web request
                HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(requestUri);
                //Setting the type to GET method
                httpWebRequest.Method = "GET";
                if (!string.IsNullOrEmpty(sessionToken))
                {
                    httpWebRequest.PreAuthenticate = true;
                    // Adding the Header of the refreshed session token
                    log.LogVariableState("Session Token for Authorization", sessionToken);
                    httpWebRequest.Headers.Add("Authorization", "Bearer " + sessionToken);
                }
                if (!string.IsNullOrEmpty(host))
                {
                    log.LogVariableState("Host", host);
                    httpWebRequest.Host = host;
                }
                // Determines the content type as application/x-www-form-urlencoded
                httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                // Covert readWriteTimeout FROM seconds to milliseconds
                httpWebRequest.ReadWriteTimeout = readWriteTimeout * 1000;
                //Gets the webResponse
                using (HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    if (HttpStatusCode.OK == httpWebResponse.StatusCode)
                    {
                        Encoding encoding = Encoding.GetEncoding("utf-8");
                        using (StreamReader responseStream = new StreamReader(httpWebResponse.GetResponseStream(), encoding))
                        {
                            jsonResponse = responseStream.ReadToEnd();
                        }
                    }
                    else
                    {
                        // 2564 - Response for the &1, Json Response - &2 and HTTP Status Description - &3 and HTTP Status Code - &4, for the Request URI &5
                        string responseMessage = MessageContainerList.GetMessage(executionContext, 2564, "GET", jsonResponse, httpWebResponse.StatusDescription, httpWebResponse.StatusCode.ToString(), requestUri);
                        log.Error(responseMessage);
                        log.LogMethodExit(jsonResponse);
                        throw new Exception(responseMessage);
                    }
                }
            }
            catch (WebException webException)
            {
                if (webException.Response != null)
                {
                    using (HttpWebResponse errorResponse = (HttpWebResponse)webException.Response)
                    {
                        using (StreamReader streamReader = new StreamReader(errorResponse.GetResponseStream()))
                        {
                            log.Error(webException);
                            log.LogMethodExit(jsonResponse, "Throwing Exception : " + webException.Message);
                            throw new Exception(streamReader.ReadToEnd());
                        }
                    }
                }
                else
                {
                    log.Error(webException.Message, webException);
                    log.LogMethodExit(jsonResponse);
                    throw;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                log.LogMethodExit(jsonResponse);
                throw;
            }
            log.LogMethodExit(jsonResponse);
            return jsonResponse;
        }

        /// <summary>
        /// This method is for inserting a new record over a Web API using POST method
        /// </summary>
        /// <param name="jsonString"></param>
        /// <param name="requestUri"></param>
        /// <param name="sessionToken"></param>
        /// <param name="host"></param>
        /// <param name="readWriteTimeout"></param>
        /// <returns></returns>
        public Dictionary<string, string> Post(string jsonString, string requestUri, string sessionToken = "", string host = "", int readWriteTimeout = 10)
        {
            log.LogMethodEntry(jsonString, requestUri, sessionToken, host, readWriteTimeout);
            string jsonResponse = string.Empty;
            Dictionary<string, string> responseDictionary = new Dictionary<string, string>();
            HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(requestUri);
            UTF8Encoding utf8Encoding = new UTF8Encoding();
            byte[] byteArray = utf8Encoding.GetBytes(jsonString);
            httpWebRequest.Method = "POST";
            if (!string.IsNullOrEmpty(sessionToken))
            {
                httpWebRequest.PreAuthenticate = true;
                // Adding the Header of the refreshed session token
                log.LogVariableState("Session Token for Authorization", sessionToken);
                httpWebRequest.Headers.Add("Authorization", "Bearer " + sessionToken);
            }
            if (!string.IsNullOrEmpty(host))
            {
                log.LogVariableState("Host", host);
                httpWebRequest.Host = host;
            }
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.ContentLength = byteArray.Length;
            httpWebRequest.ReadWriteTimeout = readWriteTimeout * 1000; // Covert readWriteTimeout FROM seconds to milliseconds
            using (Stream requestedStreamData = httpWebRequest.GetRequestStream())
            {
                requestedStreamData.Write(byteArray, 0, byteArray.Length);
            }
            try
            {
                using (HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    Encoding encoding = Encoding.GetEncoding("utf-8");
                    if (HttpStatusCode.OK == httpWebResponse.StatusCode)
                    {
                        using (StreamReader responseStream = new StreamReader(httpWebResponse.GetResponseStream(), encoding))
                        {
                            jsonResponse = responseStream.ReadToEnd();
                            log.LogVariableState("Json Response ", jsonResponse);
                            // 2564 - Response for the &1, Json Response - &2 and HTTP Status Description - &3 and HTTP Status Code - &4, for the Request URI &5
                            string responseMessage = MessageContainerList.GetMessage(executionContext, 2564, "POST", jsonResponse, httpWebResponse.StatusDescription, httpWebResponse.StatusCode.ToString(), requestUri);
                            responseDictionary.Add(httpWebResponse.StatusCode.ToString(), jsonResponse);
                            log.LogVariableState("Response Message - ", responseMessage);
                            log.Debug(responseMessage);
                        }
                    }
                    else
                    {
                        using (StreamReader responseStream = new StreamReader(httpWebResponse.GetResponseStream(), encoding))
                        {
                            jsonResponse = responseStream.ReadToEnd();
                            log.LogVariableState("Json Response ", jsonResponse);
                            // 2564 - Response for the &1, Json Response - &2 and HTTP Status Description - &3 and HTTP Status Code - &4, for the Request URI &5
                            string responseMessage = MessageContainerList.GetMessage(executionContext, 2564, "POST", jsonResponse, httpWebResponse.StatusDescription, httpWebResponse.StatusCode.ToString(), requestUri);
                            log.Error(responseMessage);
                            log.LogVariableState("Forbidden Response - ", responseMessage);
                            responseDictionary.Add(httpWebResponse.StatusCode.ToString(), responseMessage);
                        }
                    }
                }
            }
            catch (WebException webException)
            {
                if (webException.Response != null)
                {
                    using (HttpWebResponse errorResponse = (HttpWebResponse)webException.Response)
                    {
                        using (StreamReader streamReader = new StreamReader(errorResponse.GetResponseStream()))
                        {
                            log.Error(webException);
                            log.LogMethodExit(streamReader.ReadToEnd(), "Throwing Exception : " + webException.Message);
                            throw new Exception(streamReader.ReadToEnd());
                        }
                    }
                }
                else
                {
                    log.Error(webException.Message, webException);
                    log.LogMethodExit(responseDictionary, "Throwing Exception : " + webException.Message);
                    throw webException;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                log.LogMethodExit(responseDictionary, "Throwing Exception : " + ex.Message);
                throw ex;
            }
            log.LogMethodExit(responseDictionary);
            return responseDictionary;
        }

        /// <summary>
        /// This method is for updating an existing record over a Web API using PUT
        /// </summary>
        /// <param name="jsonString"></param>
        /// <param name="requestUri"></param>
        /// <param name="sessionToken"></param>
        /// <param name="host"></param>
        /// <param name="readWriteTimeout"></param>
        /// <returns></returns>
        public string PUT(string jsonString, string requestUri, string sessionToken = "", string host = "", int readWriteTimeout = 10)
        {
            log.LogMethodEntry(jsonString, requestUri, sessionToken, host, readWriteTimeout);
            string jsonResponse = string.Empty;
            HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(requestUri);
            UTF8Encoding utf8Encoding = new UTF8Encoding();
            byte[] byteArray = utf8Encoding.GetBytes(jsonString);
            httpWebRequest.Method = "PUT";
            if (!string.IsNullOrEmpty(sessionToken))
            {
                httpWebRequest.PreAuthenticate = true;
                // Adding the Header of the refreshed session token
                log.LogVariableState("Session Token for Authorization", sessionToken);
                httpWebRequest.Headers.Add("Authorization", "Bearer " + sessionToken);
            }
            if (!string.IsNullOrEmpty(host))
            {
                log.LogVariableState("Host", host);
                httpWebRequest.Host = host;
            }
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.ContentLength = byteArray.Length;
            httpWebRequest.ReadWriteTimeout = readWriteTimeout * 1000; // Covert readWriteTimeout FROM seconds to milliseconds
            //  httpWebRequest.KeepAlive = false; 
            try
            {
                using (Stream requestedStreamData = httpWebRequest.GetRequestStream())
                {
                    requestedStreamData.Write(byteArray, 0, byteArray.Length);
                }
                using (HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    if (HttpStatusCode.OK == httpWebResponse.StatusCode)
                    {
                        Encoding encoding = Encoding.GetEncoding("utf-8");
                        using (StreamReader responseStream = new StreamReader(httpWebResponse.GetResponseStream(), encoding))
                        {
                            jsonResponse = responseStream.ReadToEnd();
                            if (string.IsNullOrEmpty(jsonResponse))
                            {
                                jsonResponse = httpWebResponse.StatusCode.ToString();
                            }
                        }
                    }
                    else
                    {
                        // 2564 - Response for the &1, Json Response - &2 and HTTP Status Description - &3 and HTTP Status Code - &4, for the Request URI &5
                        string responseMessage = MessageContainerList.GetMessage(executionContext, 2564, "PUT", jsonResponse, httpWebResponse.StatusDescription, httpWebResponse.StatusCode.ToString(), requestUri);
                        log.Error(responseMessage);
                        throw new Exception(responseMessage);
                    }
                }
            }
            catch (WebException webException)
            {
                if (webException.Response != null)
                {
                    using (HttpWebResponse errorResponse = (HttpWebResponse)webException.Response)
                    {
                        using (StreamReader streamReader = new StreamReader(errorResponse.GetResponseStream()))
                        {
                            log.Error(webException);
                            log.LogMethodExit(streamReader.ReadToEnd(), "Throwing Exception : " + webException.Message);
                            throw new Exception(streamReader.ReadToEnd());
                        }
                    }
                }
                else
                {
                    log.Error(webException.Message, webException);
                    log.LogMethodExit(webException, "Throwing Exception : " + webException.Message);
                    throw webException;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                log.LogMethodExit(ex, "Throwing Exception : " + ex.Message);
                throw ex;
            }
            log.LogMethodExit(jsonResponse);
            return jsonResponse;
        }

        /// <summary>
        /// This method is for deleting an existing record over a Web API using DELETE method
        /// </summary>
        /// <param name="requestUri"></param>
        /// <param name="sessionToken"></param>
        /// <param name="host"></param>
        /// <param name="readWriteTimeout"></param>
        /// <returns></returns>
        public string DELETE(string requestUri, string sessionToken = "", string host = "", int readWriteTimeout = 10)
        {
            log.LogMethodEntry(requestUri, sessionToken, host, readWriteTimeout);
            string jsonResponse = string.Empty;
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUri);
            httpWebRequest.Method = "DELETE";
            if (!string.IsNullOrEmpty(sessionToken))
            {
                httpWebRequest.PreAuthenticate = true;
                // Adding the Header of the refreshed session token
                log.LogVariableState("Session Token for Authorization", sessionToken);
                httpWebRequest.Headers.Add("Authorization", "Bearer " + sessionToken);
            }
            if (!string.IsNullOrEmpty(host))
            {
                log.LogVariableState("Host", host);
                httpWebRequest.Host = host;
            }

            httpWebRequest.ContentType = "application/json";
            httpWebRequest.ReadWriteTimeout = readWriteTimeout * 1000; // Covert readWriteTimeout FROM seconds to milliseconds
            try
            {
                using (HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    if (HttpStatusCode.OK == httpWebResponse.StatusCode)
                    {
                        Encoding encoding = Encoding.GetEncoding("utf-8");
                        using (StreamReader responseStream = new StreamReader(httpWebResponse.GetResponseStream(), encoding))
                        {
                            jsonResponse = responseStream.ReadToEnd();
                            if (string.IsNullOrEmpty(jsonResponse))
                            {
                                jsonResponse = httpWebResponse.StatusCode.ToString();
                            }
                        }
                    }
                    else
                    {
                        // 2564 - Response for the &1, Json Response - &2 and HTTP Status Description - &3 and HTTP Status Code - &4, for the Request URI &5
                        string responseMessage = MessageContainerList.GetMessage(executionContext, 2564, "DELETE", jsonResponse, httpWebResponse.StatusDescription, httpWebResponse.StatusCode.ToString(), requestUri);
                        log.Error(responseMessage);
                        log.LogMethodExit(jsonResponse);
                        throw new Exception(responseMessage);
                    }
                }
            }
            catch (WebException webException)
            {
                if (webException.Response != null)
                {
                    using (HttpWebResponse errorResponse = (HttpWebResponse)webException.Response)
                    {
                        using (StreamReader streamReader = new StreamReader(errorResponse.GetResponseStream()))
                        {
                            log.Error(webException);
                            log.LogMethodExit(streamReader.ReadToEnd(), "Throwing Exception : " + webException.Message);
                            throw new Exception(streamReader.ReadToEnd());
                        }
                    }
                }
                else
                {
                    log.Error(webException.Message, webException);
                    log.LogMethodExit(jsonResponse);
                    throw;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                log.LogMethodExit(jsonResponse, "Throwing Exception : " + ex.Message);
                throw;
            }
            log.LogMethodExit(jsonResponse);
            return jsonResponse;
        }
    }
}