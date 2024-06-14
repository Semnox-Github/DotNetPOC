/********************************************************************************************
 * Project Name - Utitlities
 * Description  - Validate token and authorization 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.40        23-Sep-2018   Manoj          Created
 *2.60        14-Mar-2019   Jagan Mohan    Implemented Roles Authorization for Form Access : ValidateTokenAndAccessPermissions
 *2.70.0      19-Jun-2019   Jagan Mohan    Added the new GetUserRoleId() method for user selected site roleId
 *2.80        02-Apr-2020   Nitin Pai      Changed token handler for Customer Registration Changes, sending authorization token in header,      
 *                                         refresh token only if it is close to expiry and not on every call
 *2.100.0     18-Nov-2020   Nitin Pai      Removed Automatic refresh of token                                           
 *2.110       09-Feb-2021   Girish Kundar  Modified: Concurrent login changes
 ********************************************************************************************/


using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
//using Semnox.Parafait.User;

namespace Semnox.Core.Utilities
{
    public class TokenValidationHandler : DelegatingHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string Origin = "Origin";
        private const string Authorization = "Authorization";
        private const string AccessControlRequestMethod = "Access-Control-Request-Method";
        private const string AccessControlRequestHeaders = "Access-Control-Request-Headers";
        private const string AccessControlAllowOrigin = "Access-Control-Allow-Origin";
        private const string AccessControlAllowMethods = "Access-Control-Allow-Methods";
        private const string AccessControlAllowHeaders = "Access-Control-Allow-Headers";
        private const string AccessControlExposeHeaders = "Access-Control-Expose-Headers";
        private const string AccessControlAllowCredentials = "Access-Control-Allow-Credentials";
        private const string AccessJwtTokenHeaders = "Authorization";
        private const string StrictTransportSecurity = "Strict-Transport-Security";
        private const string ApiKey = "APIKey";
        private const string CheckSum = "CheckSum";
        private const string Signature = "Signature";

        private string jwtKey = "";
        private string validIssuer = "";
        private string validAudience = "";

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            log.LogMethodEntry(request.Headers.Contains(Origin));
            HttpStatusCode statusCode = HttpStatusCode.OK;
            string token;
            var isCorsRequest = request.Headers.Contains(Origin);
            var containsAPIKey = request.Headers.Contains(ApiKey);
            var containsCheckSum = request.Headers.Contains(CheckSum);
            var containsSignature = request.Headers.Contains(Signature);
            var isPreflightRequest = request.Method == HttpMethod.Options;
            string originHeader = string.Empty;
            string apiKeyHeader = string.Empty;
            string checkSumHeader = string.Empty;
            string signature = string.Empty;

            String url = HttpContext.Current.Request.Url.AbsoluteUri;
            String sessionId = "";
            DateTime sessionStartTime = DateTime.UtcNow;
            if (request.Headers.Contains("SESSION_ID") && !String.IsNullOrWhiteSpace(request.Headers.GetValues("SESSION_ID").First()))
            {
                log.Debug("Got session :" + url + ":" + sessionId + ":" + sessionStartTime);
            }
            else
            {
                sessionId = Guid.NewGuid().ToString();
                log.Debug("Created session :" + url + ":" + sessionId + ":" + sessionStartTime);
            }

            sessionId = url + ":" + sessionId;

            if (isCorsRequest)
            {
                log.Debug("isCorsRequest :" + isCorsRequest);
                originHeader = request.Headers.GetValues(Origin).First();
                String referrer = request.Headers.Contains("Referrer") ? request.Headers.GetValues("Referrer").First() : "";
                if (!string.IsNullOrWhiteSpace(originHeader) && !IsAllowedOriginal(originHeader))
                {
                    if (containsAPIKey)
                    {
                        log.Debug("containsAPIKey :" + containsAPIKey);
                        apiKeyHeader = request.Headers.GetValues(ApiKey).First();
                        if (!string.IsNullOrWhiteSpace(apiKeyHeader) && !IsAllowedOriginal(apiKeyHeader))
                        {
                            log.Debug("apiKeyHeader not empty && !IsAllowedOriginal(apiKeyHeader)");
                            HttpResponseMessage httpResponseMessage1 = await Task<HttpResponseMessage>.Factory.StartNew(() => new HttpResponseMessage(HttpStatusCode.Unauthorized) { });
                            AddHeaders(ref httpResponseMessage1, ref request);
                            log.LogMethodExit(httpResponseMessage1);
                            return httpResponseMessage1;
                        }
                    }

                    log.Debug("apiKeyHeader not empty && !IsAllowedOriginal(apiKeyHeader)");
                    HttpResponseMessage httpResponseMessage = await Task<HttpResponseMessage>.Factory.StartNew(() => new HttpResponseMessage(HttpStatusCode.Unauthorized) { });
                    AddHeaders(ref httpResponseMessage, ref request);
                    log.LogMethodExit(httpResponseMessage);
                    return httpResponseMessage;
                }

                if (string.IsNullOrWhiteSpace(originHeader))
                {
                    log.Debug("isCorsRequest :false");
                    isCorsRequest = false;
                }

                if (isPreflightRequest)
                {
                    log.Debug("isPreflightRequest :" + isPreflightRequest);
                    var response = new HttpResponseMessage(HttpStatusCode.OK);
                    AddPreFlightHeaders(ref response, ref request);
                    var tcs = new TaskCompletionSource<HttpResponseMessage>();
                    tcs.SetResult(response);
                    log.LogMethodExit(response.StatusCode);
                    log.Debug("returning isPreflightRequest ");
                    return response;
                }
            }

            if (!TryRetrieveToken(request, out token)) //Check token jwt exists or not 
            {
                try
                {
                    log.Debug("Token jwt Not exists ");
                    OverrideDeletePutMethods(ref request);
                    log.Debug(" If Login details are encrypted . Need to verify the credentials");
                    ValidateLoginRequest(containsSignature, request);
                    var response = await base.SendAsync(request, cancellationToken);
                    log.Debug("completed login");
                    AddHeaders(ref response, ref request);
                    log.Debug("added headers");
                    log.LogMethodExit(response);
                    log.Debug("returning token handler ");
                    return response;
                }
                catch (Microsoft.IdentityModel.Tokens.SecurityTokenValidationException)
                {
                    log.Debug("statusCode : " + HttpStatusCode.Unauthorized);
                    statusCode = HttpStatusCode.Unauthorized;
                }
                catch (Exception ex)
                {
                    log.LogMethodExit(ex.Message);
                    statusCode = HttpStatusCode.InternalServerError;
                }
                log.LogMethodExit(statusCode);
                var errorResponseMessage = await Task<HttpResponseMessage>.Factory.StartNew(() => new HttpResponseMessage(statusCode) { });
                AddHeaders(ref errorResponseMessage, ref request);
                log.LogMethodExit(errorResponseMessage);
                return errorResponseMessage;
            }

            try
            {
                log.Debug("Calling OverrideDeletePutMethods");
                OverrideDeletePutMethods(ref request);
                if (String.IsNullOrEmpty(jwtKey))
                    jwtKey = Encryption.GetParafaitKeys("JWTKey");

                if (String.IsNullOrEmpty(validIssuer))
                    validIssuer = Encryption.GetParafaitKeys("JWTIssuer");

                if (String.IsNullOrEmpty(validAudience))
                    validAudience = Encryption.GetParafaitKeys("JWTAudience");

                var now = DateTime.UtcNow;
                var securityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(jwtKey));

                Microsoft.IdentityModel.Tokens.SecurityToken securityToken;
                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                TokenValidationParameters validationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = validIssuer,
                    ValidAudience = validAudience,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    LifetimeValidator = this.LifetimeValidator,
                    IssuerSigningKey = securityKey,
                };
                log.Debug("Calling handler.ValidateToken to get CurrentPrincipal");
                Thread.CurrentPrincipal = handler.ValidateToken(token, validationParameters, out securityToken);
                log.Debug("Calling handler.ValidateToken to get  HttpContext.Current.User ");
                HttpContext.Current.User = handler.ValidateToken(token, validationParameters, out securityToken);
                log.Debug("Compeleted Validation");
                var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
                string roleId = identity.FindFirst(ClaimTypes.Role).Value;
                string siteId = identity.FindFirst(ClaimTypes.Sid).Value;
                string loginId = identity.FindFirst(ClaimTypes.Name).Value;
                string guid = identity.FindFirst(ClaimTypes.UserData).Value;
                string userSessionId = identity.FindFirst(ClaimTypes.PrimarySid).Value;

                log.Debug("Compeleted assignments to ClaimTypes");
                if (!ValidateTokenAndAccessPermissions(roleId, loginId, siteId, token, request, guid, userSessionId))
                {
                    log.Debug("ValidateTokenAndAccessPermissions returning false");
                    statusCode = HttpStatusCode.Unauthorized;
                    log.LogMethodExit(statusCode);
                    var errorResponseVar = await Task<HttpResponseMessage>.Factory.StartNew(() => new HttpResponseMessage(statusCode) { });
                    AddHeaders(ref errorResponseVar, ref request);
                    log.LogMethodExit(errorResponseVar);
                    log.Debug("returning with statusCode " + statusCode);
                    return errorResponseVar;
                }

                Utilities utilities = new Utilities();
                Security security = new Security(utilities);
                int userPkId = security.GetUserPkId(siteId, loginId);
                ExecutionContext executionContext = ExecutionContext.GetExecutionContext();
                executionContext.SetIsCorporate(true);
                executionContext.SetUserPKId(userPkId); // to get user level check some config

                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                log.Debug("checking for ENABLE_CHECKSUM defualt value ");
                if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "ENABLE_CHECKSUM") &&
                                    !url.ToUpper().Contains("ASSETS"))
                {
                    log.Debug("checking for ENABLE_CHECKSUM defualt value : true");
                    log.LogMethodEntry(containsCheckSum);
                    bool failed = false;
                    try
                    {
                        if (containsCheckSum)
                        {
                            log.Debug("containsCheckSum : " + containsCheckSum);
                            apiKeyHeader = loginId;
                            checkSumHeader = request.Headers.GetValues(CheckSum).First();

                            if (!string.IsNullOrWhiteSpace(checkSumHeader) && checkSumHeader.Contains("|"))
                            {
                                string[] csvalues = checkSumHeader.Split('|');
                                if (!string.IsNullOrWhiteSpace(csvalues[0]))
                                {
                                    apiKeyHeader = csvalues[0];
                                }
                                if (!string.IsNullOrWhiteSpace(csvalues[1]))
                                {
                                    checkSumHeader = csvalues[1];
                                }
                            }
                            log.Debug("Checksum user" + apiKeyHeader);
                            log.Debug("Checksum " + checkSumHeader);

                            string requestBody;
                            string generatedCheckSum = string.Empty;
                            string urlParameters = HttpContext.Current.Request.Url.AbsoluteUri;

                            if (request.Method == HttpMethod.Get)
                            {
                                log.Debug("request.Method == HttpMethod.Get : ");
                                if (!string.IsNullOrEmpty(urlParameters))
                                {
                                    // takes the url parameter from get method, if there is not any get parameter it will take it as empty.
                                    urlParameters = urlParameters.Contains('?') ? HttpContext.Current.Request.Url.AbsoluteUri.Split('?')[1] : string.Empty;
                                    log.Debug("urlParameters " + urlParameters);
                                    generatedCheckSum = securityTokenBL.GenerateChecksum(apiKeyHeader, HttpUtility.UrlDecode(urlParameters));
                                }
                            }
                            else
                            {
                                log.Debug("request.Method == HttpMethod.POST : ");
                                Stream receiveStream = HttpContext.Current.Request.InputStream;
                                StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                                //urlParameters = urlParameters.Contains('?') ? HttpContext.Current.Request.Url.AbsoluteUri.Split('?')[1] : string.Empty;
                                requestBody = readStream.ReadToEnd().ToString()/* + urlParameters*/;
                                generatedCheckSum = securityTokenBL.GenerateChecksum(apiKeyHeader, requestBody);
                                
                            }

                            log.Debug("generatedCheckSum " + generatedCheckSum);
                            if (generatedCheckSum != checkSumHeader) // Check whether the checksum matches or not.
                            {
                                log.Debug("generatedCheckSum != checkSumHeader");
                                HttpResponseMessage httpResponseMessage = await Task<HttpResponseMessage>.Factory.StartNew(() => new HttpResponseMessage(HttpStatusCode.Unauthorized) { });
                                AddHeaders(ref httpResponseMessage, ref request);
                                log.LogMethodExit(httpResponseMessage);
                                log.Debug("Error returning generatedCheckSum != checkSumHeader");
                                return httpResponseMessage;
                            }
                        }
                        else if (containsSignature)
                        {
                            log.Debug("containsSignature : " + containsSignature);
                            apiKeyHeader = loginId;
                            signature = request.Headers.GetValues(Signature).First();
                            if (String.IsNullOrWhiteSpace(signature))
                            {
                                log.Debug("signature not found");
                                HttpResponseMessage httpResponseMessage = await Task<HttpResponseMessage>.Factory.StartNew(() => new HttpResponseMessage(HttpStatusCode.Unauthorized) { });
                                AddHeaders(ref httpResponseMessage, ref request);
                                log.LogMethodExit(httpResponseMessage);
                                log.Debug("returning signature not found");
                                return httpResponseMessage;
                            }
                            var encoding = new System.Text.ASCIIEncoding();
                            string urlParameters = HttpContext.Current.Request.Url.AbsoluteUri;
                            log.Debug("calling DecryptSignature()");
                            string clientSignature = securityTokenBL.DecryptSignature(signature);
                            string[] sessionKeyHashPair = clientSignature.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                            log.Debug("creating sessionKeyHashPair[]");
                            if (sessionKeyHashPair.Length != 2)
                            {
                                log.Debug("signature not found : sessionKeyHashPair.Length != 2");
                                HttpResponseMessage httpResponseMessage = await Task<HttpResponseMessage>.Factory.StartNew(() => new HttpResponseMessage(HttpStatusCode.Unauthorized) { });
                                AddHeaders(ref httpResponseMessage, ref request);
                                log.LogMethodExit(httpResponseMessage);
                                log.Debug("returning signature not found");
                                return httpResponseMessage;
                            }
                            string sessionKey = sessionKeyHashPair[0];
                            string clientComputedHash = sessionKeyHashPair[1];
                            byte[] securityKeyByte = encoding.GetBytes(sessionKey);

                            if (request.Method == HttpMethod.Get)
                            {
                                log.Debug("request.Method == HttpMethod.Get");
                                if (!string.IsNullOrEmpty(urlParameters))
                                {
                                    log.Debug("getting urlParameters");
                                    urlParameters = urlParameters.Contains('?') ? HttpContext.Current.Request.Url.AbsoluteUri.Split('?')[1] : string.Empty;
                                    byte[] rawData = encoding.GetBytes(urlParameters);
                                    log.Debug("created array of bytes- urlParameters");
                                    using (var hmacsha256 = new HMACSHA256(securityKeyByte))
                                    {
                                        byte[] hashmessage = hmacsha256.ComputeHash(rawData);
                                        var hashValue = Convert.ToBase64String(hashmessage);
                                        StringComparer comparer = StringComparer.InvariantCulture;
                                        if (comparer.Compare(clientComputedHash, hashValue) != 0)
                                        {
                                            log.Debug("Compare(clientComputedHash, hashValue) falied");
                                            log.Debug("signature not found");
                                            HttpResponseMessage httpResponseMessage = await Task<HttpResponseMessage>.Factory.StartNew(() => new HttpResponseMessage(HttpStatusCode.Unauthorized) { });
                                            AddHeaders(ref httpResponseMessage, ref request);
                                            log.LogMethodExit(httpResponseMessage);
                                            log.Debug("returning signature not valid");
                                            return httpResponseMessage;
                                        }
                                        log.LogMethodExit(hashValue);
                                    }
                                }
                            }
                            else
                            {
                                log.Debug("request.Method == HttpMethod.Post");
                                Stream receiveStream = HttpContext.Current.Request.InputStream;
                                StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                                string requestBody = readStream.ReadToEnd().ToString();
                                var newEncoding = new System.Text.ASCIIEncoding();
                                //requestBody = requestBody.Replace(System.Environment.NewLine,"");
                                byte[] rawData = newEncoding.GetBytes(requestBody);
                                log.Debug("created array of bytes- body");
                                using (var hmacsha256 = new HMACSHA256(securityKeyByte))
                                {
                                    byte[] hashmessage = hmacsha256.ComputeHash(rawData);
                                    var hashValue = Convert.ToBase64String(hashmessage);
                                    StringComparer comparer = StringComparer.InvariantCulture;
                                    if (comparer.Compare(clientComputedHash, hashValue) != 0)
                                    {
                                        log.Debug("Compare(clientComputedHash, hashValue) falied");
                                        HttpResponseMessage httpResponseMessage = await Task<HttpResponseMessage>.Factory.StartNew(() => new HttpResponseMessage(HttpStatusCode.Unauthorized) { });
                                        AddHeaders(ref httpResponseMessage, ref request);
                                        log.LogMethodExit(httpResponseMessage);
                                        log.Debug("returning body content is not valid");
                                        return httpResponseMessage;
                                    }
                                    log.LogMethodExit(hashValue);
                                }
                            }
                        }
                        else
                        {
                            HttpResponseMessage httpResponseMessage = await Task<HttpResponseMessage>.Factory.StartNew(() => new HttpResponseMessage(HttpStatusCode.Unauthorized) { });
                            AddHeaders(ref httpResponseMessage, ref request);
                            log.LogMethodExit(httpResponseMessage);
                            log.Debug("returningrequest is not valid :  No check sum and signature");
                            return httpResponseMessage;
                        }
                    }
                    catch (Microsoft.IdentityModel.Tokens.SecurityTokenValidationException ex)
                    {
                        log.Debug(" SecurityTokenValidationException: failed = true");
                        failed = true;
                        log.Debug(ex.Message);
                        statusCode = HttpStatusCode.Unauthorized;
                        log.Debug(" statusCode:" + statusCode);
                    }
                    catch (Exception ex)
                    {
                        log.Debug(" Exception: failed = true");
                        failed = true;
                        log.Debug(ex.Message);
                        statusCode = HttpStatusCode.InternalServerError;
                        log.Debug(" statusCode:" + statusCode);
                    }
                    if (failed)
                    {
                        log.Debug(" failed = true");
                        HttpResponseMessage httpResponseMessage = await Task<HttpResponseMessage>.Factory.StartNew(() => new HttpResponseMessage(statusCode) { });
                        AddHeaders(ref httpResponseMessage, ref request);
                        log.LogMethodExit(httpResponseMessage);
                        log.Debug(" Error:  returing statusCode " + statusCode);
                        return httpResponseMessage;

                    }
                    log.LogMethodExit();

                }
                bool generateNewToken = false;
                DateTime validTill = securityToken.ValidTo;

                if (validTill > DateTime.UtcNow)
                {
                    log.Debug("validTill > DateTime.UtcNow ,generateNewToken = false");
                    generateNewToken = false;
                }
                else
                {
                    log.Debug("validTill < DateTime.UtcNow : Unauthorized");
                    HttpResponseMessage httpResponseMessage = await Task<HttpResponseMessage>.Factory.StartNew(() => new HttpResponseMessage(HttpStatusCode.Unauthorized) { });
                    AddHeaders(ref httpResponseMessage, ref request);
                    log.LogMethodExit(httpResponseMessage);
                    log.Debug(" Error:  returing statusCode : Unauthorized");
                    return httpResponseMessage;
                }
                log.Debug("calling requested API:");
                var response = await base.SendAsync(request, cancellationToken);
                log.Debug("calling AddHeaders()");
                if (ConfigurationManager.AppSettings.AllKeys.Contains("RemoveSQLException") && ConfigurationManager.AppSettings["RemoveSQLException"].ToString().ToUpper() == "TRUE")
                {
                    //to remove sql error from response
                    CleanResponseContent(response);
                }
                AddHeaders(ref response, ref request);
                log.Debug("AddHeaders() completed");
                log.LogMethodExit();
                log.Debug("returning response");
                return response;
            }
            catch (Microsoft.IdentityModel.Tokens.SecurityTokenValidationException ex)
            {
                log.Debug("SecurityTokenValidationException : Unauthorized");
                statusCode = HttpStatusCode.Unauthorized;
            }
            catch (Exception ex)
            {
                log.Debug("Exception : InternalServerError");
                statusCode = HttpStatusCode.InternalServerError;
            }
            log.LogMethodExit(statusCode);
            var errorResponse = await Task<HttpResponseMessage>.Factory.StartNew(() => new HttpResponseMessage(statusCode) { });
            log.Debug("calling AddHeaders()");
            AddHeaders(ref errorResponse, ref request);
            log.Debug("AddHeaders() completed");
            log.LogMethodExit();
            log.Debug("returning errorResponse");
            return errorResponse;
        }
        private void AddPreFlightHeaders(ref HttpResponseMessage response, ref HttpRequestMessage request)
        {
            log.LogMethodEntry(" Calling AddPreFlightHeaders()");
            log.Debug("Adding origin to response header");
            response.Headers.Add(AccessControlAllowOrigin, request.Headers.GetValues(Origin).First());
            //response.Headers.Add(AccessControlAllowOrigin, "*");
            log.Debug("Adding origin to response header completed");
            var accessControlRequestMethod = request.Headers.GetValues(AccessControlRequestMethod).FirstOrDefault();
            log.Debug("getting accessControlRequestMethod");
            if (accessControlRequestMethod != null)
            {
                log.Debug("accessControlRequestMethod != null , add to response.Headers.");
                response.Headers.Add(AccessControlAllowMethods, accessControlRequestMethod);
            }
            var requestedHeaders = string.Join(", ", request.Headers.GetValues(AccessControlRequestHeaders));
            if (!string.IsNullOrEmpty(requestedHeaders))
            {
                log.Debug("requestedHeaders != null , add to response.Headers.");
                response.Headers.Add(AccessControlAllowHeaders, requestedHeaders);
                response.Headers.Add(AccessControlExposeHeaders, "Authorization");
            }
            response.Headers.Add(AccessControlAllowCredentials, "true");
            log.LogMethodExit();
        }
        private void AddHeaders(ref HttpResponseMessage response, ref HttpRequestMessage request)
        {
            log.LogMethodEntry("AddHeaders() starts");
            if (request.Headers.Contains(Origin))
            {
                log.LogMethodEntry("Add origin to header");
                response.Headers.Add(AccessControlAllowOrigin, request.Headers.GetValues(Origin).First());
                //response.Headers.Add(AccessControlAllowOrigin, "*"); // only for virtual arcade
            }
            if (response.Headers.Contains("Authorization"))
            {
                response.Headers.Remove(Authorization);
            }
            log.Debug("get authorization");
            if (request.Headers.Contains("Authorization"))
            {
                log.LogMethodEntry("request.Headers.Contains(Authorization)");
                String token = request.Headers.GetValues(Authorization).First();
                if (!token.Contains("Bearer"))
                {
                    token = "Bearer " + token;
                }
                response.Headers.Add(Authorization, token);
                log.Debug("added authorization");
            }
            else
            {
                response.Headers.Add(Authorization, "");
                log.Debug("added authorization");
            }
            response.Headers.Add(AccessControlAllowHeaders, "Content-Type, Authorization, x-requested-with");
            response.Headers.Add(AccessControlExposeHeaders, "Authorization");
            response.Headers.Add(AccessControlAllowCredentials, "true");
            response.Headers.Add(StrictTransportSecurity, "max-age=31536000; includeSubDomains");
            log.LogMethodExit();
        }

        private bool IsAllowedOriginal(string origin)
        {
            log.LogMethodEntry(origin);
            log.Debug("IsAllowedOriginal starts");
            var authorizedClients = System.Configuration.ConfigurationManager.AppSettings["AuthorizedClients"].ToString();
            log.LogVariableState("Authorized Cients ", authorizedClients);
            string[] clients = authorizedClients.Split(',');
            bool returnvalue = clients.Any(a => a.Trim().Equals(origin));
            log.Debug("IsAllowedOriginal ends");
            log.LogMethodExit(returnvalue);
            return returnvalue;
        }

        private void OverrideDeletePutMethods(ref HttpRequestMessage request)
        {
            log.Debug("OverrideDeletePutMethods starts");
            ///X-HTTP-Method-Override is a non-standard HTTP header. 
            ///It is designed for clients that cannot send certain HTTP request types, such as PUT or DELETE. 
            ///Instead, the client sends a POST request and sets the X-HTTP-Method-Override header to the desired method
            bool methodDelete = request.Headers.Contains("X-HTTP-Method-Override");
            // Check for HTTP POST with the X-HTTP-Method-Override header.
            if (methodDelete)
            {
                // Check if the header value is in our methods list.
                var method = "Delete";
                request.Method = new HttpMethod(method);
            }
            log.Debug("OverrideDeletePutMethods ends");
        }
        /// <summary>
        /// Token and Roles are not available then the user will be considered as unauthorized user.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        private bool ValidateTokenAndAccessPermissions(string roleId, string loginId, string siteId, string token, HttpRequestMessage request, string userGuid, string userSessionId)
        {
            log.Debug("ValidateTokenAndAccessPermissions starts");
            SecurityTokenBL securitybl = new SecurityTokenBL();
            if (securitybl.ValidateToken(userGuid, token, null, null, userSessionId))
            {
                log.Debug("securitybl.ValidateToken success");
                /// 19-Jun-2019 - Jagan Mohan
                /// Get the control name from the Request object, there is a logic to be applied for the Home controller
                /// In the Home controller, user can change the select a new Site. Upon selecting a new site, that siteId has to be set as SiteId and - 
                /// Roles have to be retrieved for the loggdin user and the newly selected siteId.
                /// If the permissions are there to view the newly selected site for the logged in user, user will be allowed to do further operations otherwise user will be logged out from the site 
                string originHeader = string.Empty;
                string apiKeyHeader = string.Empty;
                if (request.Headers.Contains("FormName"))
                {
                    log.Debug("request.Headers.Contains(FormName)");
                    Utilities utilities = new Utilities();
                    Security security = new Security(utilities);
                    IEnumerable<string> headerValues = request.Headers.GetValues("FormName");
                    string formName = headerValues.FirstOrDefault();
                    string requestURI = request.RequestUri.Segments[2];
                    string controllerName = requestURI.Replace(@"/", "");
                    if (controllerName.ToUpper().ToString() == "HOME" && request.Method.ToString() == "POST")
                    {
                        log.Debug("controllerName = Home");
                        if (Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]))
                        {
                            log.Debug("IsCorporate = true");
                            // below siteId is the new one which the user selected
                            HttpRequest httpRequest = HttpContext.Current.Request;
                            var bodyStream = new StreamReader(HttpContext.Current.Request.InputStream);
                            bodyStream.BaseStream.Seek(0, SeekOrigin.Begin);
                            string formBodyText = bodyStream.ReadToEnd().ToString();
                            siteId = formBodyText.Replace("\"", "");
                        }
                        else
                        {
                            siteId = "-1";
                        }
                        /// again passthe loginId & SiteId to know the roles for the logged in user
                        roleId = security.GetUserRoleId(loginId, siteId);
                    }
                    /// Get the form name from the Current Request method and validate the form acceess.(14-Mar-2019 - Jagan Mohan)
                    log.Debug("calling ValidateFormAccess");
                    return security.ValidateFormAccess(formName, roleId, siteId);
                }

                if (request.Headers.Contains(Origin))
                {
                    log.Debug("request.Headers.Contains(Origin)");
                    /// 02-Aug-2019 - Jagan Mohan
                    /// The below condition check for other apps whether the origin request allowed or not
                    originHeader = request.Headers.GetValues(Origin).First();
                    if (!string.IsNullOrWhiteSpace(originHeader) && IsAllowedOriginal(originHeader))
                    {
                        log.Debug("true");
                        return true;
                    }
                    else
                    {
                        log.Debug("false : no origin");
                        return false;
                    }
                }

                if (request.Headers.Contains(ApiKey))
                {
                    log.Debug("request.Headers.Contains(ApiKey)");
                    /// The below condition check for other apps whether the origin request allowed or not
                    apiKeyHeader = request.Headers.GetValues(ApiKey).First();
                    if (!string.IsNullOrWhiteSpace(apiKeyHeader) && IsAllowedOriginal(apiKeyHeader))
                    {
                        log.Debug("true");
                        return true;
                    }
                    else
                    {
                        log.Debug("false : no ApiKey");
                        return false;
                    }
                }

                log.Debug("true :  allow without origin");
                return true;
            }
            else
            {
                log.Debug("false :  do allow without origin");
                return false;
            }
        }

        private bool TryRetrieveToken(HttpRequestMessage request, out string token)
        {
            log.Debug("TryRetrieveToken() starts");
            token = null;
            IEnumerable<string> authzHeaders;
            if (!request.Headers.TryGetValues("Authorization", out authzHeaders) || authzHeaders.Count() > 1)
            {
                log.Debug("No Authorization data ,return false");
                return false;
            }
            var bearerToken = authzHeaders.ElementAt(0);
            token = bearerToken.StartsWith("Bearer ") ? bearerToken.Substring(7) : bearerToken;
            log.Debug(" Authorization data found ,return true");
            return true;
        }
        public bool LifetimeValidator(DateTime? notBefore, DateTime? expires, Microsoft.IdentityModel.Tokens.SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            log.Debug("LifetimeValidator() starts");
            if (expires != null)
            {
                if (DateTime.UtcNow < expires) return true;
            }
            log.Debug("LifetimeValidator()returns false");
            return false;
        }

        public void ValidateLoginRequest(bool containsSignature, HttpRequestMessage request)
        {
            log.LogMethodEntry(containsSignature);
            SecurityTokenBL securityTokenBL = new SecurityTokenBL();
            if (containsSignature)
            {
                string signature = request.Headers.GetValues(Signature).First();
                if (String.IsNullOrWhiteSpace(signature))
                {
                    log.Debug("signature not found");
                    throw new Exception("signature not found");
                }
                var encoding = new System.Text.ASCIIEncoding();
                string urlParameters = HttpContext.Current.Request.Url.AbsoluteUri;
                string clientSignature = securityTokenBL.DecryptSignature(signature);
                string[] sessionKeyHashPair = clientSignature.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                if (sessionKeyHashPair.Length != 2)
                {
                    log.Debug("signature not valid");
                    throw new Exception("signature not valid");
                }
                string sessionKey = sessionKeyHashPair[0];
                string clientComputedHash = sessionKeyHashPair[1];
                byte[] securityKeyByte = encoding.GetBytes(sessionKey);

                if (request.Method == HttpMethod.Post)
                {
                    Stream receiveStream = HttpContext.Current.Request.InputStream;
                    StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                    string requestBody = readStream.ReadToEnd().ToString();
                    string loginRequestString = securityTokenBL.DecryptPayLoad(sessionKey, requestBody);
                    byte[] rawData = encoding.GetBytes(loginRequestString);
                    using (var hmacsha256 = new HMACSHA256(securityKeyByte))
                    {
                        byte[] hashmessage = hmacsha256.ComputeHash(rawData);
                        var hashValue = Convert.ToBase64String(hashmessage);
                        StringComparer comparer = StringComparer.InvariantCulture;
                        if (comparer.Compare(clientComputedHash, hashValue) != 0)
                        {
                            log.Debug("Compare(clientComputedHash, hashValue) failed");
                            throw new Exception("Invalid data");
                        }
                        var json = JsonConvert.DeserializeObject(loginRequestString).ToString();
                        StringContent stringContent = new StringContent(json, Encoding.UTF8, "application/json");
                        request.Content = stringContent;
                        log.LogMethodExit(hashValue);
                    }
                }
                else
                {
                    throw new Exception("Error while validating the login request");
                }
                log.LogMethodExit();
            }
        }

        public void CleanResponseContent(HttpResponseMessage response)
        {
            log.LogMethodEntry(response);
            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                log.Error(response.Content);
                Dictionary<string, string> message = new Dictionary<string, string>();
                message.Add("data", "Something went wrong");
                var json = JsonConvert.SerializeObject(message);
                response.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }
            log.LogMethodExit();
        }
    }
}

