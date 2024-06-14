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
 *2.80       08-Apr-2020    Nitin Pai      Cobra changes for Waiver, Customer Registration and Online Sales
 *2.100       20-Nov-2020   Nitin Pai      Fix: If site is not passed, the master site should be considered 
 *2.110.0     11-Nov-2020   Girish Kundar           Upload : Tokanization process implementation
 *2.110.0     28-Dec-2020   Gururaja Kanjan         Updated for service request.  to get data access rules.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Customer;
using Semnox.Parafait.POS;
using Semnox.Parafait.Site;
using Semnox.Parafait.User;

namespace Semnox.CommonAPI.Login
{
    public class LoginController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        [HttpPost]
        [Route("api/Login/AuthenticateUsers")]
        [AllowAnonymous]
        public HttpResponseMessage Post([FromBody] LoginRequest login)
        {
            SecurityTokenDTO securityTokenDTO = null;
            try
            {
                log.LogMethodEntry(login);
                Utilities utilities = new Utilities();
                Security security = new Security(utilities);
                Security.User user = null;
                int lifeTime = -1;
                bool isCorporate = Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]);

                if (string.IsNullOrEmpty(login.LoginToken))
                {
                    int siteId = -1;
                    try
                    {
                        if (string.IsNullOrEmpty(login.SiteId) == false && Convert.ToInt32(login.SiteId) > -1)
                        {
                            siteId = Convert.ToInt32(login.SiteId);
                        }
                        SiteList siteList = new SiteList(null);
                        SiteDTO masterSiteDTO = siteList.GetMasterSiteFromHQ();
                        if (siteId == -1 && masterSiteDTO !=null)
                        {
                            siteId = Convert.ToInt32(masterSiteDTO.SiteId);
                        }
                        user = security.Login(login.LoginId, login.Password, siteId);
                    }
                    catch (Security.SecurityException se)
                    {

                        if (System.Runtime.InteropServices.Marshal.GetHRForException(se) == Security.SecurityException.ExChangePassword)
                        {
                            Users users = new Users(utilities.ExecutionContext, login.LoginId, siteId);
                            if (users != null && users.UserDTO.UserId > 0)
                            {
                                ExecutionContext executionContext = new ExecutionContext(users.UserDTO.UserName.ToString(), users.UserDTO.SiteId, -1, users.UserDTO.UserId, SiteContainerList.IsCorporate(), -1);
                                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                                lifeTime = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "JWT_TOKEN_LIFE_TIME", 0);
                                securityTokenBL.GenerateNewJWTToken(login.LoginId, users.UserDTO.Guid, users.UserDTO.SiteId.ToString(), "-1", users.UserDTO.RoleId.ToString(), "User", "-1", Guid.NewGuid().ToString(), lifeTime);
                                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                                Request.Headers.Authorization = new AuthenticationHeaderValue(securityTokenDTO.Token);
                                return Request.CreateResponse(HttpStatusCode.OK, new { userDTO = users.UserDTO });
                            }
                        }
                        else
                        {
                            Request.Headers.Authorization = new AuthenticationHeaderValue(new Guid().ToString());
                            return Request.CreateResponse(HttpStatusCode.NotFound, new { data = se.Message });
                        }
                    }
                    if (user != null && user.UserId != 0)
                    {
                        SiteList siteList = new SiteList(null);
                        var content = siteList.GetAllSitesUserLogedIn(user.LoginId);

                        List<KeyValuePair<SiteDTO.SearchBySiteParameters, string>> searchParam = new List<KeyValuePair<SiteDTO.SearchBySiteParameters, string>>();
                        searchParam.Add(new KeyValuePair<SiteDTO.SearchBySiteParameters, string>(SiteDTO.SearchBySiteParameters.IS_ACTIVE, "Y"));
                        List<SiteDTO> siteDTOList = siteList.GetAllSites(searchParam);
                        if (siteDTOList != null && siteDTOList.Count > 1)
                        {
                            isCorporate = true;
                            HttpContext.Current.Application["IsCorporate"] = "true";
                        }
                        else
                        {
                            isCorporate = false;
                            HttpContext.Current.Application["IsCorporate"] = "False";
                        }
                        //Upload download tokenization changes
                        // The token is created for the local user for the respective site
                        // Site will pass the siteId and login credentials and validated at the HQ for this site
                        //If user exists the create unique token generated using the this siteId
                        if (user.SiteId == -1 && string.IsNullOrEmpty(login.SiteId) ==false &&  Convert.ToInt32(login.SiteId) > -1 )
                        {
                            user.SiteId = Convert.ToInt32(login.SiteId);
                        }
                        else if (isCorporate == false && content != null && content.Count <= 1)
                        {
                           user.SiteId = -1; 
                        }
                        else
                        {
                            isCorporate = true;
                        }
                    }
                }
                else
                {
                    Dictionary<string, string> tokenKeyValuePairs = security.AnonymousLogin(login.LoginId, login.LoginToken);

                    string origin = tokenKeyValuePairs["origin"];
                    Guid originIdentifier = new Guid(tokenKeyValuePairs["identifier"]);
                    DateTime issuedAt = DateTime.UtcNow;
                    if (tokenKeyValuePairs.ContainsKey("issuedAt"))
                    {
                        issuedAt = DateTime.Parse(tokenKeyValuePairs["issuedAt"]).ToUniversalTime();
                    }
                    DateTime expiresAt = DateTime.UtcNow;
                    if (tokenKeyValuePairs.ContainsKey("expiresAt"))
                    {
                        expiresAt = DateTime.Parse(tokenKeyValuePairs["expiresAt"]).ToUniversalTime();
                    }
                    else
                    {
                        // override expires at and set it as 15 minutes from issued time
                        expiresAt = issuedAt.AddMinutes(15).ToUniversalTime();
                    }

                    if (String.IsNullOrEmpty(origin) || String.IsNullOrEmpty(originIdentifier.ToString()))
                    {
                        log.LogVariableState("origin or device identifier is invalid", origin + ":" + originIdentifier);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "Unauthorized"});
                    }

                    // create the anonymous user
                    Users anonymousUser = null;
                    SiteList siteList = new SiteList(ExecutionContext.GetExecutionContext());
                    List<KeyValuePair<SiteDTO.SearchBySiteParameters, string>> searchParam = new List<KeyValuePair<SiteDTO.SearchBySiteParameters, string>>();
                    searchParam.Add(new KeyValuePair<SiteDTO.SearchBySiteParameters, string>(SiteDTO.SearchBySiteParameters.IS_ACTIVE, "Y"));
                    List<SiteDTO> siteDTOList = siteList.GetAllSites(searchParam);
                    if (siteDTOList != null && siteDTOList.Count > 1)
                    {
                        isCorporate = true;
                    }
                    int siteId = 0;
                    if (!string.IsNullOrWhiteSpace(login.SiteId) && 
                        int.TryParse(login.SiteId, out siteId) && siteId > 0)
                    {
                        anonymousUser = new Users(utilities.ExecutionContext, login.LoginId, siteId);
                    }
                    else
                    {
                        // if site if is not sent in, default to HQ site
                        siteId = -1;
                        SiteDTO HQSite = siteList.GetMasterSiteFromHQ();
                        if (HQSite != null && HQSite.SiteId != -1)
                        {
                            siteId = HQSite.SiteId;
                        }
                        anonymousUser = new Users(utilities.ExecutionContext, login.LoginId, siteId);
                    }

                    

                    if (String.IsNullOrEmpty(anonymousUser.UserDTO.LoginId))
                    {
                        String message = MessageContainerList.GetMessage(ExecutionContext.GetExecutionContext(), 735, "Users", anonymousUser.UserDTO.LoginId);
                        log.Error(message);
                        throw new Exception(message);
                    }

                    user = new Security.User();
                    user.CardNumber = "";
                    user.EmpNumber = anonymousUser.UserDTO.EmpNumber;
                    user.LastName = "";
                    user.LoginId = anonymousUser.UserDTO.LoginId;
                    user.ManagerFlag = false;
                    user.RoleId = anonymousUser.UserDTO.RoleId;
                    user.UserId = anonymousUser.UserDTO.UserId;
                    user.SiteId = anonymousUser.UserDTO.SiteId;
                    user.UserName = anonymousUser.UserDTO.UserName;
                    user.UserSessionTimeOut = 300;
                    //Use the device UUID instead of user GUID as the same external POS user will be used across all devices and the device UUID will be the distinguishing factor
                    user.GUID = originIdentifier.ToString();

                    // perform additional validations is any
                    switch (login.LoginId)
                    {
                        case "CustomerApp":
                            {
                                if (user.UserId != 0)
                                {
                                    //ExecutionContext executionContext = new ExecutionContext(user.LoginId, user.SiteId, -1, user.UserId, isCorporate, -1);
                                    //CustomerListBL customerListBL = new CustomerListBL(executionContext);
                                    //try
                                    //{
                                    //    CustomerDTO customer = customerListBL.ValidateCustomerDevice(origin, originIdentifier.ToString());
                                    //    if (customer == null)
                                    //    {
                                    //        String message = MessageContainerList.GetMessage(executionContext, 2369);
                                    //        log.Error("origin or device identifier is invalid " + origin + ":" + originIdentifier);
                                    //        return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = message });
                                    //    }
                                    //}
                                    //catch(EntityNotFoundException)
                                    //{
                                    //    // do nothing as this is a new device
                                    //}
                                    //catch(Exception ex)
                                    //{
                                    //    String message = MessageContainerList.GetMessage(executionContext, 2369);
                                    //    log.Error("origin or device identifier is invalid", ex);
                                    //    return Request.CreateResponse(HttpStatusCode.NotFound, new { data = message });
                                    //}
                                }
                            }
                            break;
                        default:
                            {
                                // No specific validation is performed for time being
                                break;
                            }
                    }
                }

                if (user.UserId != 0)
                {
                    // check if the site has a default language and set it in context
                    int defaultLanguageId = -1;
                    int posMachineId = -1;
                    log.Debug("isCorporate value for execution context : " + isCorporate);

                    ExecutionContext executionContext = new ExecutionContext(user.LoginId, user.SiteId, -1, user.UserId, isCorporate, -1);
                    List<KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>>();
                    searchParameters.Add(new KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>(ParafaitDefaultsDTO.SearchByParameters.SITE_ID, Convert.ToString(user.SiteId)));
                    searchParameters.Add(new KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>(ParafaitDefaultsDTO.SearchByParameters.DEFAULT_VALUE_NAME, "DEFAULT_LANGUAGE"));

                    ParafaitDefaultsListBL parafaitDefaultsListBL = new ParafaitDefaultsListBL(executionContext);
                    List<ParafaitDefaultsDTO>  parafaitDefaultsDTOList = parafaitDefaultsListBL.GetParafaitDefaultsDTOList(searchParameters);
                    if(parafaitDefaultsDTOList != null && parafaitDefaultsDTOList.Count != 0)
                    {
                        defaultLanguageId = Convert.ToInt32(parafaitDefaultsDTOList[0].DefaultValue);
                    }
                    POSMachineContainerDTO posMachineContainerDTO = POSMachineContainerList.GetPOSMachineContainerDTOOrDefault(SiteContainerList.IsCorporate() ? user.SiteId : -1, login.MachineName, login.IPAddress,-1);
                    string posMachineGuid = string.Empty;
                    string posMachineName = string.IsNullOrWhiteSpace(login.MachineName) ? string.Empty : login.MachineName;
                    if (posMachineContainerDTO != null)
                    {
                        posMachineId = posMachineContainerDTO.POSMachineId;
                        posMachineGuid = posMachineContainerDTO.Guid;
                        posMachineName = posMachineContainerDTO.POSName;
                    }
                    SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                    log.Error("Generating token");
                    lifeTime = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "JWT_TOKEN_LIFE_TIME", 0);
                    securityTokenBL.GenerateNewJWTToken(user.LoginId, user.GUID, user.SiteId.ToString(), defaultLanguageId.ToString(), user.RoleId.ToString(),"User", posMachineId.ToString(), Guid.NewGuid().ToString(), lifeTime);
                    log.Error("Token generated");
                    securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                    log.LogMethodExit(securityTokenDTO);
                    Request.Headers.Authorization = new AuthenticationHeaderValue(securityTokenDTO.Token);
                    Users users = new Users(executionContext, user.UserId,true,true); 
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, new { userDTO = users.UserDTO});
                    return response;
                }
                else
                {
                    String message = MessageContainerList.GetMessage(ExecutionContext.GetExecutionContext(), 735);
                    log.Error(message);
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { data = message});
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ex.Message });
            }
        }
    }
}

