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
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Semnox.Core.Utilities
{
    public abstract class RemoteDataService
    {
        private static readonly Parafait.logging.Logger log = new Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string WEB_API_ORIGIN_KEY = "ParafaitPOS";//These will be moved to configuration
        private const string WEB_API_URL = "https://localhost/";//These will be moved to configuration
        protected readonly Core.Utilities.ExecutionContext executionContext;
        private string parafaitLoginToken;
        private readonly string origin;
        private readonly string webApiUrl;
        private static string anonymousLoginKeyParafaitPOS;
        public RemoteDataService(Core.Utilities.ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            origin = ConfigurationManager.AppSettings["WEB_API_ORIGIN_KEY"];
            webApiUrl = ConfigurationManager.AppSettings["WEB_API_URL"];
            anonymousLoginKeyParafaitPOS = Encryption.Decrypt(ConfigurationManager.AppSettings["AnonymousLoginKeyParafaitPOS"]);
            anonymousLoginKeyParafaitPOS = "jXn2r5u8x/A?D(G+KaPdSgVkYp3s6v9y";//ConfigurationManager.AppSettings["AnonymousLoginKeyParafaitPOS"];Encryption.GetParafaitKeys("AnonymousLoginKeyParafaitPOS");
            log.LogMethodExit();
        }
        public WebApiResponse GetData(string url, List<KeyValuePair<string, string>> headers)
        {
            log.LogMethodEntry(url, headers);
            WebApiResponse result;
            try
            {
                HttpClient client = new HttpClient();
                //amitha
                client.Timeout = Timeout.InfiniteTimeSpan;
                using (NoSynchronizationContextScope.Enter())
                {

                    foreach (KeyValuePair<string, string> header in headers)
                    {
                        if (string.IsNullOrWhiteSpace(header.Value) || string.IsNullOrWhiteSpace(header.Key))
                        {
                            continue;
                        }
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }

                    Task<HttpResponseMessage> httpResponseMessageTask = client.GetAsync(url);
                    httpResponseMessageTask.Wait();
                    HttpResponseMessage httpResponseMessage = httpResponseMessageTask.Result;
                    Task<string> responseTask = httpResponseMessage.Content.ReadAsStringAsync();
                    responseTask.Wait();
                    result = new WebApiResponse(httpResponseMessage.StatusCode, responseTask.Result, httpResponseMessage.Headers);
                }
            }
            catch (UnauthorizedException ex)
            {
                log.Error(ex);
                throw;
            }
            catch (Exception ex)
            {
                string errorMessage = "Error occured while communicating with Web-Api";
                log.Error(errorMessage, ex);
                result = new WebApiResponse(errorMessage, ex);
            }
            log.LogMethodExit(result);
            return result;
        }

        private WebApiResponse PostData(string url, List<KeyValuePair<string, string>> headers, string content)
        {
            log.LogMethodEntry(url, headers);
            WebApiResponse result;
            try
            {
                HttpClient client = new HttpClient();
                //amitha
                client.Timeout = Timeout.InfiniteTimeSpan;

                using (NoSynchronizationContextScope.Enter())
                {
                    foreach (KeyValuePair<string, string> header in headers)
                    {
                        if (string.IsNullOrWhiteSpace(header.Value) || string.IsNullOrWhiteSpace(header.Key))
                        {
                            continue;
                        }
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                    byte[] buffer = Encoding.UTF8.GetBytes(content);
                    ByteArrayContent byteContent = new ByteArrayContent(buffer);
                    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    //amitha starts
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12;
                    //ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
                    // amitha ends
                    Task<HttpResponseMessage> httpResponseMessageTask = client.PostAsync(url, byteContent);
                    httpResponseMessageTask.Wait();
                    HttpResponseMessage httpResponseMessage = httpResponseMessageTask.Result;
                    Task<string> responseTask = httpResponseMessage.Content.ReadAsStringAsync();
                    responseTask.Wait();
                    result = new WebApiResponse(httpResponseMessage.StatusCode, responseTask.Result, httpResponseMessage.Headers);
                }
            }
            catch (UnauthorizedException ex)
            {
                log.Error(ex);
                throw;
            }
            catch (Exception ex)
            {
                string errorMessage = "Error occured while communicating with Web-Api";
                log.Error(errorMessage, ex);
                result = new WebApiResponse(errorMessage, ex);
            }
            log.LogMethodExit(result);
            return result;
        }

        private WebApiResponse DeleteData(string url, List<KeyValuePair<string, string>> headers, string content)
        {
            log.LogMethodEntry(url, headers);
            WebApiResponse result;
            try
            {
                HttpClient client = new HttpClient();
                client.Timeout = Timeout.InfiniteTimeSpan;
                using (NoSynchronizationContextScope.Enter())
                {
                    foreach (KeyValuePair<string, string> header in headers)
                    {
                        if (string.IsNullOrWhiteSpace(header.Value) || string.IsNullOrWhiteSpace(header.Key))
                        {
                            continue;
                        }
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                    byte[] buffer = Encoding.UTF8.GetBytes(content);
                    ByteArrayContent byteContent = new ByteArrayContent(buffer);
                    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12;
                    Task<HttpResponseMessage> httpResponseMessageTask = client.SendAsync(new HttpRequestMessage(HttpMethod.Delete, url) { Content = byteContent });
                    httpResponseMessageTask.Wait();
                    HttpResponseMessage httpResponseMessage = httpResponseMessageTask.Result;
                    Task<string> responseTask = httpResponseMessage.Content.ReadAsStringAsync();
                    responseTask.Wait();
                    result = new WebApiResponse(httpResponseMessage.StatusCode, responseTask.Result, httpResponseMessage.Headers);
                }
            }
            catch (UnauthorizedException ex)
            {
                log.Error(ex);
                throw;
            }
            catch (Exception ex)
            {
                string errorMessage = "Error occured while communicating with Web-Api";
                log.Error(errorMessage, ex);
                result = new WebApiResponse(errorMessage, ex);
            }
            log.LogMethodExit(result);
            return result;
        }
        public string Get(string urlPart, List<KeyValuePair<string, string>> parameters = null)
        {
            log.LogMethodEntry(urlPart, parameters);
            string url = GetUrl(urlPart, parameters);
            WebApiResponse result = GetImpl(url);
            if (result.Success)
            {
                if (result.StatusCode == HttpStatusCode.Unauthorized)
                {
                    executionContext.SetWebApiToken(string.Empty);
                    result = GetImpl(url);
                }
            }

            ThrowIfInvalidWebResponse(result);

            log.LogMethodExit(result.Response);
            return result.Response;
        }

        private WebApiResponse GetImpl(string url)
        {
            log.LogMethodEntry(url);

            List<KeyValuePair<string, string>> headers = GetHeaders();

            WebApiResponse result = GetData(webApiUrl + url, headers); ;
            log.LogMethodExit(result);
            return result;
        }

        public string Post(string url, string content)
        {
            log.LogMethodEntry(url);
            WebApiResponse result = PostImpl(url, content);
            if (result.Success)
            {
                if (result.StatusCode == HttpStatusCode.Unauthorized)
                {
                    executionContext.SetWebApiToken(string.Empty);
                    result = PostImpl(url, content);
                }
            }

            ThrowIfInvalidWebResponse(result);

            log.LogMethodExit(result.Response);
            return result.Response;
        }

        public string Delete(string url, string content)
        {
            log.LogMethodEntry(url);
            WebApiResponse result = DeleteImpl(url, content);
            if (result.Success)
            {
                if (result.StatusCode == HttpStatusCode.Unauthorized)
                {
                    executionContext.SetWebApiToken(string.Empty);
                    result = DeleteImpl(url, content);
                }
            }
            ThrowIfInvalidWebResponse(result);

            log.LogMethodExit(result.Response);
            return result.Response;
        }

        private static void ThrowIfInvalidWebResponse(WebApiResponse result)
        {
            log.LogMethodEntry(result);
            if (result.Success == false)
            {
                log.LogMethodExit(null, "Throwing WebApiException - " + result.ErrorMessage);
                throw new WebApiException(result.ErrorMessage, result);
            }
            if (result.StatusCode == HttpStatusCode.Unauthorized)
            {
                string message = "Unauthorized : " + result.GetServerErrorMessage();
                log.LogMethodExit(null, "Throwing UnauthorizedException - " + message);
                throw new UnauthorizedException(message);
            }
            if (result.StatusCode != HttpStatusCode.OK)
            {
                string message = result.GetServerErrorMessage();
                log.LogMethodExit(null, "Throwing WebApiException - " + message);
                throw new WebApiException(message, result);
            }
            log.LogMethodExit();
        }

        private WebApiResponse PostImpl(string url, string content)
        {
            log.LogMethodEntry(url, content);
            List<KeyValuePair<string, string>> headers = GetHeaders();
            WebApiResponse result = PostData(webApiUrl + url, headers, content); ;
            log.LogMethodExit(result);
            return result;
        }
        private WebApiResponse DeleteImpl(string url, string content)
        {
            log.LogMethodEntry(url, content);
            List<KeyValuePair<string, string>> headers = GetHeaders();
            WebApiResponse result = DeleteData(webApiUrl + url, headers, content); ;
            log.LogMethodExit(result);
            return result;
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

        private KeyValuePair<string, string> GetAuthorizationHeader()
        {
            log.LogMethodEntry();
            string authorization = GetAuthorizationToken();
            KeyValuePair<string, string> authorizationHeader = new KeyValuePair<string, string>("Authorization", authorization);
            log.LogMethodExit(authorizationHeader);
            return authorizationHeader;
        }

        private KeyValuePair<string, string> GetOriginHeader()
        {
            KeyValuePair<string, string> originHeader = new KeyValuePair<string, string>("Origin", origin);
            return originHeader;
        }

        private string GetAuthorizationToken()
        {
            log.LogMethodEntry();
            string token = executionContext.GetWebApiToken();
            if (string.IsNullOrWhiteSpace(token) == false)
            {
                log.LogMethodExit(token);
                return token;
            }
            //LoginRequest loginRequest = CreateLoginRequest();
            //string requestUrl = webApiUrl + "/api/Login/AuthenticateUsers";
            //string content = JsonConvert.SerializeObject(loginRequest);
            //List<KeyValuePair<string, string>> headers = new List<KeyValuePair<string, string>> { GetOriginHeader() };
            //WebApiResponse result = PostData(requestUrl, headers, content); ;
            //ThrowIfInvalidWebResponse(result);
            //token = result.GetAuthenticationToken();
            //// amitha starts
            ////string requestUrl2 = webApiUrl + "/api/Home/Sites";
            ////string content2 = executionContext.GetSiteId().ToString();
            ////string responseString = Post(requestUrl2, content2);
            ////List<KeyValuePair<string, string>> headers2 = new List<KeyValuePair<string, string>> { GetOriginHeader(), new KeyValuePair<string, string>("Authorization", token)   };
            ////WebApiResponse result2 = PostData(requestUrl, headers, content); ;
            ////token = result2.GetAuthenticationToken();
            //// amitha ends
            //executionContext.SetWebApiToken(token);

            //log.LogMethodExit(token);

            return token;
        }

        //private LoginRequest CreateLoginRequest()
        //{
        //    log.LogMethodEntry();
        //    string loginId = "ParafaitPOS";
        //    string loginToken = GetParafaitPOSLoginToken();
        //    string siteId = executionContext.GetSiteId().ToString();
        //    LoginRequest loginRequest = new LoginRequest { LoginId = loginId, LoginToken = loginToken, SiteId = siteId };
        //    log.LogMethodExit(loginRequest);
        //    return loginRequest;
        //}

        //private string GetParafaitPOSLoginToken()
        //{
        //    log.LogMethodEntry();
        //    if (string.IsNullOrWhiteSpace(parafaitLoginToken))
        //    {
        //        parafaitLoginToken = GenerateParafaitLoginToken(executionContext.GetPosMachineGuid());

        //    }
        //    log.LogMethodExit(parafaitLoginToken);
        //    return parafaitLoginToken;
        //}
        //private string GenerateParafaitLoginToken(string posMachineGuid)
        //{
        //    //This key will be generated and given by Parafait Support
        //    string key = GetLoginKey();

        //    SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(key));
        //    SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
        //    //create the claims            
        //    ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[]
        //    {
        //        // Integrator Name
        //        new Claim("name", "ParafaitPOS"),
        //        // Unique Id of the whiteliested device
        //       // new Claim("userdata", posMachineGuid),
        //       //amitha
        //        new Claim("userdata", "B37A3481-31D8-42EF-8EFD-E2396EFAC061"),
        //          // Token generation time in UTC
        //        new Claim("issuedat", DateTime.UtcNow.ToString(CultureInfo.InvariantCulture))

        //    });

        //    JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        //    JwtSecurityToken token = tokenHandler.CreateJwtSecurityToken(
        //        signingCredentials: signingCredentials,
        //        subject: claimsIdentity);
        //    string result = tokenHandler.WriteToken(token);
        //    return result;
        //}

        //private string GetLoginKey()
        //{
        //    log.LogMethodEntry();
        //    log.LogMethodExit("anonymousLoginKeyParafaitPOS");
        //    return anonymousLoginKeyParafaitPOS;
        //}

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

        public string GetUtcDateTimeString(string dateTimeString)
        {
            log.LogMethodEntry(dateTimeString);

            string result = DateTime
                .ParseExact(dateTimeString, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
                .ToUniversalTime().ToString("o");
            log.LogMethodExit(result);
            return result;
        }
    }
}
