/********************************************************************************************
* Project Name - Generic Utilities
* Description  - Generic Web Api Data Access Handler
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.70.2        12-Nov-2019   Lakshminarayana           Created 
********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Web;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// Generic Web-Api Data Access Handler
    /// </summary>
    public class WebDataAccessHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private readonly ParafaitRemotingClient client;
        private static string serverMachineName;
        private static string anonymousLoginKeyParafaitPOS;
        private string parafaitLoginToken;
        private readonly string origin;
        private readonly int overridingSiteId;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="overridingSiteId">overriding site id</param>
        public WebDataAccessHandler(ExecutionContext executionContext, int overridingSiteId = -1)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.overridingSiteId = overridingSiteId;
            this.origin = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "WEB_API_ORIGIN_KEY");
            client = GetParafaitRemotingClient();
            log.LogMethodExit();
        }

        public string Get(string url, List<KeyValuePair<string, string>> parameterHeaders = null)
        {
            log.LogMethodEntry(url, parameterHeaders);
            WebApiResponse result = GetImpl(url, parameterHeaders);
            if (result.Success)
            {
                if (result.StatusCode == HttpStatusCode.Unauthorized)
                {
                    executionContext.SetWebApiToken(string.Empty);
                    result = GetImpl(url, parameterHeaders);
                }
            }
            executionContext.SetWebApiToken(result.GetAuthenticationToken());

            ThrowIfInvalidWebResponse(result);
            
            log.LogMethodExit(result.Response);
            return result.Response;
        }

        private WebApiResponse GetImpl(string url, List<KeyValuePair<string, string>> parameterHeaders = null)
        {
            log.LogMethodEntry(url, parameterHeaders);
            WebApiResponse result;
            List<KeyValuePair<string, string>> headers = GetHeaders(parameterHeaders);
            string webApiUrl = GetWebApiUrl();
            try
            {
                result = client.Get(webApiUrl + url, headers);
            }
            catch (Exception ex)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 2418);// "Error occured while communicating with on-demand service";
                log.Error(errorMessage, ex);
                result = new WebApiResponse(errorMessage, ex);
            }
            log.LogMethodExit(result);
            return result;
        }

        public string Post(string url, string content)
        {
            return Post(url, null, content);
        }

        public string Post(string url, List<KeyValuePair<string, string>> parameterHeaders, string content)
        {
            log.LogMethodEntry(url, parameterHeaders);
            WebApiResponse result = PostImpl(url, parameterHeaders, content);
            if (result.Success)
            {
                if (result.StatusCode == HttpStatusCode.Unauthorized)
                {
                    executionContext.SetWebApiToken(string.Empty);
                    result = PostImpl(url, parameterHeaders, content);
                }
            }
            executionContext.SetWebApiToken(result.GetAuthenticationToken());

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

            if (result.StatusCode != HttpStatusCode.OK)
            {
                string message = result.GetServerErrorMessage();
                log.LogMethodExit(null, "Throwing WebApiException - " + message);
                throw new WebApiException(message, result);
            }
            log.LogMethodExit();
        }

        private WebApiResponse PostImpl(string url, List<KeyValuePair<string, string>> parameterHeaders, string content)
        {
            log.LogMethodEntry(url, parameterHeaders, content);
            WebApiResponse result;
            string webApiUrl = GetWebApiUrl();
            List<KeyValuePair<string, string>> headers = GetHeaders(parameterHeaders);
            log.LogVariableState("url", webApiUrl + url);
            log.LogVariableState("content", content);
            try
            {
                result = client.Post(webApiUrl + url, headers, content, "application/json");
            }
            catch (Exception ex)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 2418);// "Error occured while communicating with on-demand service";
                log.Error(errorMessage, ex);
                result = new WebApiResponse(errorMessage, ex);
            }
            log.LogMethodExit(result);
            return result;
        }

        private List<KeyValuePair<string, string>> GetHeaders(List<KeyValuePair<string, string>> parameterHeaders)
        {
            log.LogMethodEntry(parameterHeaders);
            List<KeyValuePair<string, string>> headers = new List<KeyValuePair<string, string>>
            {
                GetOriginHeader(), GetAuthorizationHeader()
            };
            if (parameterHeaders != null && parameterHeaders.Any())
            {
                headers.AddRange(parameterHeaders);
            }
            log.LogMethodExit(headers);
            return headers;
        }

        private ParafaitRemotingClient GetParafaitRemotingClient()
        {
            string remotingUrl =
                ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ONDEMAND_REMOTING_SERVER_URL");
            if (string.IsNullOrEmpty(remotingUrl.Trim()))
            {
                if (string.IsNullOrWhiteSpace(serverMachineName))
                {
                    serverMachineName = GetServerMachineName();
                }
                remotingUrl = "http://" + serverMachineName + ":8000";
            }

            int timeoutInSeconds =
                ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ONDEMAND_REMOTING_SERVICE_TIMEOUT",
                    20);
            System.ServiceModel.WSHttpBinding binding =
                new System.ServiceModel.WSHttpBinding(System.ServiceModel.SecurityMode.None)
                {
                    SendTimeout = new TimeSpan(0, 0, timeoutInSeconds), MaxReceivedMessageSize = int.MaxValue, MaxBufferPoolSize = int.MaxValue
                };
            ParafaitRemotingClient remotingClient = new ParafaitRemotingClient(binding, new System.ServiceModel.EndpointAddress(remotingUrl + "/ParafaitRemoting/Service/ParafaitRemoting"));
            return remotingClient;
        }

        private string GetServerMachineName()
        {
            string connectionString = GetConnectionString();
            int pos1 = connectionString.IndexOf("data source", StringComparison.OrdinalIgnoreCase);
            int pos2 = connectionString.IndexOf('=', pos1) + 1;
            int pos3 = connectionString.IndexOf('\\', pos2);
            if (pos3 == -1)
                pos3 = connectionString.IndexOf(';', pos2);

            string result = pos3 == -1 ? connectionString.Substring(pos2) : connectionString.Substring(pos2, pos3 - pos2);

            result = result.Trim();
            if (result == "" || result == ".")
                result = "localhost";
            log.LogMethodExit(result);
            return result;
        }

        private string GetConnectionString()
        {
            log.LogMethodEntry();
            string connString = string.Empty;
            try
            {
                connString = ConfigurationManager.ConnectionStrings["ParafaitConnectionString"].ConnectionString;
            }
            catch(Exception ex)
            {
                log.Error("Error occured while retrieving ParafaitConnectionString", ex);
            }
            if (string.IsNullOrEmpty(connString))
                connString = ConfigurationManager.ConnectionStrings["ParafaitUtils.Properties.Settings.ParafaitConnectionString"].ConnectionString;
            string connectionString = StaticUtils.getParafaitConnectionString(connString);
            log.LogMethodExit(connectionString);
            return connectionString;
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
            string token;// = executionContext.GetWebApiToken();
            //if (string.IsNullOrWhiteSpace(token) == false)
            //{
            //    log.LogMethodExit(token);
            //    return token;
            //}
            LoginRequest loginRequest = CreateLoginRequest();
            string webApiUrl = GetWebApiUrl();
            string requestUrl = webApiUrl + "/api/Login/AuthenticateUsers";
            string content = JsonConvert.SerializeObject(loginRequest);
            List<KeyValuePair<string, string>> headers = new List<KeyValuePair<string, string>> {GetOriginHeader()};
            WebApiResponse result;
            try
            {
                result = client.Post(requestUrl, headers, content, "application/json");
            }
            catch (Exception ex)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 2418);
                log.Error(errorMessage, ex);
                result = new WebApiResponse(errorMessage, ex);
            }
            ThrowIfInvalidWebResponse(result);
            token = result.GetAuthenticationToken();
            //executionContext.SetWebApiToken(token);
            log.LogMethodExit(token);
            return token;
        }

        private LoginRequest CreateLoginRequest()
        {
            log.LogMethodEntry();
            string loginId = "ParafaitPOS";
            string loginToken = GetParafaitPOSLoginToken();
            string siteId = overridingSiteId >= 0 ? overridingSiteId.ToString() : executionContext.GetSiteId().ToString();
            LoginRequest loginRequest = new LoginRequest {LoginId = loginId, LoginToken = loginToken, SiteId = siteId};
            log.LogMethodExit(loginRequest);
            return loginRequest;
        }

        private string GetParafaitPOSLoginToken()
        {
            log.LogMethodEntry();
            if (string.IsNullOrWhiteSpace(parafaitLoginToken))
            {
                parafaitLoginToken = GenerateParafaitLoginToken(executionContext.GetPosMachineGuid());
            }
            log.LogMethodExit(parafaitLoginToken);
            return parafaitLoginToken;
        }
        private string GenerateParafaitLoginToken(string posMachineGuid)
        {
            //This key will be generated and given by Parafait Support
            string key = GetLoginKey();
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(key));
            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            //create the claims            
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[]
            {
                // Integrator Name
                new Claim("name", "ParafaitPOS"),
                // Unique Id of the whiteliested device
                new Claim("userdata", posMachineGuid),
                // Token generation time in UTC
                new Claim("issuedat", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture))

            });

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken token = tokenHandler.CreateJwtSecurityToken(
                signingCredentials: signingCredentials,
                subject: claimsIdentity);
            string result = tokenHandler.WriteToken(token);
            return result;
        }

        private string GetLoginKey()
        {
            log.LogMethodEntry();
            if (string.IsNullOrWhiteSpace(anonymousLoginKeyParafaitPOS))
            {
                anonymousLoginKeyParafaitPOS = Encryption.GetParafaitKeys("AnonymousLoginKeyParafaitPOS");
            }
            log.LogMethodExit("anonymousLoginKeyParafaitPOS");
            return anonymousLoginKeyParafaitPOS;
        }

        private string GetWebApiUrl()
        {
            log.LogMethodEntry();
            string webApiUrl = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "WEB_API_URL");
            if (string.IsNullOrWhiteSpace(webApiUrl))
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 2419);//"WEB_API_URL is not configured. Please configure the same.";
                log.LogMethodExit("Throwing Exception - " + errorMessage);
                throw new Exception(errorMessage);
            }
            log.LogMethodExit(webApiUrl);
            return webApiUrl;
        }

        public string GetUrl(string url, List<KeyValuePair<string, string>> parameters)
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

        ~WebDataAccessHandler()
        {
            log.LogMethodEntry();
            if (client != null)
            {
                try
                {
                    client.Close();
                }
                catch (Exception ex)
                {
                    log.Error("Error occured when the client tried to close", ex);
                }
            }
            log.LogMethodExit();
        }
    }
}
