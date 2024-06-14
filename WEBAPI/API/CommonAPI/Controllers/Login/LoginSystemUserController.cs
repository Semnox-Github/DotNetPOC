/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - API for the Login
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.40        23-Sep-2018   Manoj          Created
 *2.60        14-Mar-2019   Jagan Mohan    Implemented Roles Authorization for Form Access
 *2.70        27-Jul-2019   Nitin Pai      Implemented Anonymous Login for non userid\pwd loging
 *2.80        05-Apr-2020   Girish Kundar  Modified: API path changes and token removed form the response body
 * 2.80       08-Apr-2020      Nitin Pai      Cobra changes for Waiver, Customer Registration and Online Sales
 ********************************************************************************************/
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Semnox.Core.Utilities;
using Semnox.Parafait.Authentication;
using System.Threading.Tasks;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;

namespace Semnox.CommonAPI.Games
{
    public class LoginSystemController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        [HttpPost]
        [Route("api/Login/AuthenticateSystemUsers")]
        [AllowAnonymous]
        public async Task<HttpResponseMessage> Post([FromBody] LoginRequest login)
        {
            ExecutionContext executionContext = null;
            try
            {
                IAuthenticationUseCases authenticationUseCases = AuthenticationUseCaseFactory.GetAuthenticationUseCases();
                executionContext = await authenticationUseCases.LoginSystemUser(login);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = executionContext });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            } 
            //SecurityTokenDTO securityTokenDTO = null;
            //try
            //{
            //    log.LogMethodEntry(login);
            //    Utilities utilities = new Utilities();
            //    Security security = new Security(utilities);
            //    Security.User user = null;
            //    bool isCorporate = Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]);

            //    if (string.IsNullOrEmpty(login.LoginToken))
            //    {
            //        try
            //        {
            //            user = security.Login(login.LoginId, login.Password);
            //        }
            //        catch (Security.SecurityException se)
            //        {

            //            if (System.Runtime.InteropServices.Marshal.GetHRForException(se) == Security.SecurityException.ExChangePassword)
            //            {
            //                Users users = new Users(login.LoginId);
            //                if (users.UserDTO.UserId > 0)
            //                {
            //                    SecurityTokenBL securityTokenBL = new SecurityTokenBL();
            //                    securityTokenBL.GenerateNewJWTToken(login.LoginId, users.UserDTO.Guid, users.UserDTO.SiteId.ToString(), "-1", users.UserDTO.RoleId.ToString());
            //                    securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
            //                    Request.Headers.Authorization = new AuthenticationHeaderValue(securityTokenDTO.Token);
            //                    return Request.CreateResponse(HttpStatusCode.OK, new { userDTO = users.UserDTO});
            //                }
            //            }
            //            else
            //            {
            //                Request.Headers.Authorization = new AuthenticationHeaderValue(new Guid().ToString());
            //                return Request.CreateResponse(HttpStatusCode.NotFound, new { data = se.Message });
            //            }
            //        }
            //        if (user.UserId != 0)
            //        {
            //            SiteList siteList = new SiteList(null);
            //            var content = siteList.GetAllSitesUserLogedIn(user.LoginId);
            //            if (content != null && content.Count <= 1)
            //            {
            //               user.SiteId = -1; // comment this line for HQ 
            //            }
            //            else
            //            {
            //                isCorporate = true;
            //            }
            //        }
            //    }
            //    //else
            //    //{
            //    //    Dictionary<string, string> tokenKeyValuePairs = security.AnonymousLogin(login.LoginId, login.LoginToken);

            //    //    string origin = tokenKeyValuePairs["origin"];
            //    //    Guid originIdentifier = new Guid(tokenKeyValuePairs["identifier"]);
            //    //    DateTime issuedAt = DateTime.UtcNow;
            //    //    if (tokenKeyValuePairs.ContainsKey("issuedAt"))
            //    //    {
            //    //        issuedAt = DateTime.Parse(tokenKeyValuePairs["issuedAt"]).ToUniversalTime();
            //    //    }
            //    //    DateTime expiresAt = DateTime.UtcNow;
            //    //    if (tokenKeyValuePairs.ContainsKey("expiresAt"))
            //    //    {
            //    //        expiresAt = DateTime.Parse(tokenKeyValuePairs["expiresAt"]).ToUniversalTime();
            //    //    }
            //    //    else
            //    //    {
            //    //        // override expires at and set it as 15 minutes from issued time
            //    //        expiresAt = issuedAt.AddMinutes(15).ToUniversalTime();
            //    //    }

            //    //    //if (issuedAt < DateTime.UtcNow.AddMinutes(-15))
            //    //    //{
            //    //    //    log.LogVariableState("Token has expired", issuedAt + " current " + DateTime.UtcNow.AddMinutes(-15));
            //    //    //    return Request.CreateResponse(HttpStatusCode.NotFound, new { data = "Unauthorized", token = securityTokenDTO.Token });
            //    //    //}

            //    //    if (String.IsNullOrEmpty(origin) || String.IsNullOrEmpty(originIdentifier.ToString()))
            //    //    {
            //    //        log.LogVariableState("origin or device identifier is invalid", origin + ":" + originIdentifier);
            //    //        return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "Unauthorized"});
            //    //    }

            //    //    // create the anonymous user
            //    //    Users anonymousUser = null;
            //    //    int siteId = 0;
            //    //    if (!string.IsNullOrWhiteSpace(login.SiteId))
            //    //        int.TryParse(login.SiteId, out siteId);

            //    //    if (siteId > 0)
            //    //    {
            //    //        anonymousUser = new Users(login.LoginId, siteId);
            //    //        isCorporate = true;
            //    //    }
            //    //    else
            //    //        anonymousUser = new Users(login.LoginId);

            //    //    if (String.IsNullOrEmpty(anonymousUser.GetUserDTO.LoginId))
            //    //    {
            //    //        String message = MessageContainer.GetMessage(ExecutionContext.GetExecutionContext(), 735, "Users", anonymousUser.GetUserDTO.LoginId);
            //    //        log.Error(message);
            //    //        throw new Exception(message);
            //    //    }

            //    //    user = new Security.User();
            //    //    user.CardNumber = "";
            //    //    user.EmpNumber = anonymousUser.GetUserDTO.EmpNumber;
            //    //    user.LastName = "";
            //    //    user.LoginId = anonymousUser.GetUserDTO.LoginId;
            //    //    user.ManagerFlag = false;
            //    //    user.RoleId = anonymousUser.GetUserDTO.RoleId;
            //    //    user.UserId = anonymousUser.GetUserDTO.UserId;
            //    //    user.SiteId = anonymousUser.GetUserDTO.SiteId;
            //    //    user.UserName = anonymousUser.GetUserDTO.UserName;
            //    //    user.UserSessionTimeOut = 300;
            //    //    //Use the device UUID instead of user GUID as the same external POS user will be used across all devices and the device UUID will be the distinguishing factor
            //    //    user.GUID = originIdentifier.ToString();

            //    //    // perform additional validations is any
            //    //    switch (login.LoginId)
            //    //    {
            //    //        case "CustomerApp":
            //    //            {
            //    //                if (user.UserId != 0)
            //    //                {
            //    //                    ExecutionContext executionContext = new ExecutionContext(user.LoginId, user.SiteId, -1, user.UserId, isCorporate, -1);
            //    //                    CustomerListBL customerListBL = new CustomerListBL(executionContext);
            //    //                    try
            //    //                    {
            //    //                        CustomerDTO customer = customerListBL.ValidateCustomerDevice(origin, originIdentifier.ToString());
            //    //                        if (customer == null)
            //    //                        {
            //    //                            String message = MessageContainer.GetMessage(executionContext, 2369);
            //    //                            log.Error("origin or device identifier is invalid " + origin + ":" + originIdentifier);
            //    //                            return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = message });
            //    //                        }
            //    //                    }
            //    //                    catch(EntityNotFoundException)
            //    //                    {
            //    //                        // do nothing as this is a new device
            //    //                    }
            //    //                    catch(Exception ex)
            //    //                    {
            //    //                        String message = MessageContainer.GetMessage(executionContext, 2369);
            //    //                        log.Error("origin or device identifier is invalid", ex);
            //    //                        return Request.CreateResponse(HttpStatusCode.NotFound, new { data = message });
            //    //                    }
            //    //                }
            //    //            }
            //    //            break;
            //    //        default:
            //    //            {
            //    //                // No specific validation is performed for time being
            //    //                break;
            //    //            }
            //    //    }
            //    //}

            //    if (user.UserId != 0)
            //    {
            //        // check if the site has a default language and set it in context
            //        int defaultLanguageId = -1;
            //        ExecutionContext executionContext = new ExecutionContext(user.LoginId, user.SiteId, -1, user.UserId, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), -1);
            //        List<KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>>();
            //        searchParameters.Add(new KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>(ParafaitDefaultsDTO.SearchByParameters.SITE_ID, Convert.ToString(user.SiteId)));
            //        searchParameters.Add(new KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>(ParafaitDefaultsDTO.SearchByParameters.DEFAULT_VALUE_NAME, "DEFAULT_LANGUAGE"));

            //        ParafaitDefaultsListBL parafaitDefaultsListBL = new ParafaitDefaultsListBL(executionContext);
            //        List<ParafaitDefaultsDTO>  parafaitDefaultsDTOList = parafaitDefaultsListBL.GetParafaitDefaultsDTOList(searchParameters);
            //        if(parafaitDefaultsDTOList != null && parafaitDefaultsDTOList.Count != 0)
            //        {
            //            defaultLanguageId = Convert.ToInt32(parafaitDefaultsDTOList[0].DefaultValue);
            //        }

            //        SecurityTokenBL securityTokenBL = new SecurityTokenBL();
            //        log.Error("Generating token");
            //        securityTokenBL.GenerateNewJWTToken(user.LoginId, user.GUID, user.SiteId.ToString(), defaultLanguageId.ToString(), user.RoleId.ToString());
            //        log.Error("Token generated");
            //        securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
            //        log.LogMethodExit(securityTokenDTO);
            //        Request.Headers.Authorization = new AuthenticationHeaderValue(securityTokenDTO.Token);
            //        Users users = new Users(executionContext, user.UserId);
            //        HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, new { userDTO = users.UserDTO});
            //        return response;
            //    }
            //    else
            //    {
            //        String message = MessageContainer.GetMessage(ExecutionContext.GetExecutionContext(), 735);
            //        log.Error(message);
            //        return Request.CreateResponse(HttpStatusCode.NotFound, new { data = message});
            //    }
            //}
            //catch (Exception ex)
            //{
            //    log.Error(ex);
            //    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ex.Message });
            //}
        }
    }
}

