/********************************************************************************************
 * Project Name - Security Token Controller
 * Description  - Controller for sending customer password reset link
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 * 2.80       08-Apr-2020      Nitin Pai      Cobra changes for Waiver, Customer Registration and Online Sales
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;
using Semnox.Parafait.POS;
using Semnox.Parafait.Site;
using Semnox.Parafait.User;
namespace Semnox.CommonAPI.Controllers.Security
{
    public class SecurityTokensController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        [HttpGet]
        [Route("api/Security/SecurityTokens")]
        [Authorize]
        public HttpResponseMessage Get(bool activeRecordsOnly = false, int tokenId = -1, string token = null, string tableObject = null, string objectGuid = null,
                                                DateTime? fromDate = null, DateTime? toDate = null,string usersessionId =null)
        {
            SecurityTokenDTO securityTokenDTO = null;
            try
            {
                log.LogMethodEntry(activeRecordsOnly, tokenId, token, tableObject, objectGuid, fromDate, toDate);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<SecurityTokenDTO.SearchByParameters, string>> searchSecurityParameters = new List<KeyValuePair<SecurityTokenDTO.SearchByParameters, string>>();
             // searchSecurityParameters.Add(new KeyValuePair<SecurityTokenDTO.SearchByParameters, string>(SecurityTokenDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));

                DateTime startDate = DateTime.Now;
                DateTime endDate = DateTime.Now.AddDays(1);
                if (fromDate != null)
                {
                    startDate = Convert.ToDateTime(fromDate.ToString());
                    if (startDate == DateTime.MinValue)
                    {
                        string customException = "Invalid date format, expected format is yyyy-mm-dd hh:mm:ss";
                        log.Error(customException);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
                    }
                }

                if (toDate != null)
                {
                    endDate = Convert.ToDateTime(toDate.ToString());
                    if (endDate == DateTime.MinValue)
                    {
                        string customException = "Invalid date format, expected format is yyyy-mm-dd hh:mm:ss";
                        log.Error(customException);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
                    }
                }

                if (fromDate != null || toDate != null)
                {
                    searchSecurityParameters.Add(new KeyValuePair<SecurityTokenDTO.SearchByParameters, string>(SecurityTokenDTO.SearchByParameters.START_TIME, startDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                    searchSecurityParameters.Add(new KeyValuePair<SecurityTokenDTO.SearchByParameters, string>(SecurityTokenDTO.SearchByParameters.EXPIRY_TIME, endDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                }

                if (tokenId > -1)
                {
                    searchSecurityParameters.Add(new KeyValuePair<SecurityTokenDTO.SearchByParameters, string>(SecurityTokenDTO.SearchByParameters.TOKENID, tokenId.ToString()));
                }
                if (!string.IsNullOrEmpty(token))
                {
                    searchSecurityParameters.Add(new KeyValuePair<SecurityTokenDTO.SearchByParameters, string>(SecurityTokenDTO.SearchByParameters.TOKEN, token.ToString()));
                }
                if (!string.IsNullOrEmpty(tableObject))
                {
                    searchSecurityParameters.Add(new KeyValuePair<SecurityTokenDTO.SearchByParameters, string>(SecurityTokenDTO.SearchByParameters.OBJECT, tableObject.ToString()));
                }
                if (activeRecordsOnly)
                {
                    searchSecurityParameters.Add(new KeyValuePair<SecurityTokenDTO.SearchByParameters, string>(SecurityTokenDTO.SearchByParameters.ACTIVE_FLAG, "Y"));
                }
                if (!string.IsNullOrEmpty(objectGuid))
                {
                    searchSecurityParameters.Add(new KeyValuePair<SecurityTokenDTO.SearchByParameters, string>(SecurityTokenDTO.SearchByParameters.OBJECT_GUID, objectGuid.ToString()));
                }
                if (!string.IsNullOrEmpty(usersessionId))
                {
                    searchSecurityParameters.Add(new KeyValuePair<SecurityTokenDTO.SearchByParameters, string>(SecurityTokenDTO.SearchByParameters.USER_SESSION_ID, usersessionId.ToString()));
                }
                SecurityTokenListBL securityTokenListBL = new SecurityTokenListBL();
                List<SecurityTokenDTO> securityTokenDTOList = securityTokenListBL.GetSecurityTokenDTOList(searchSecurityParameters);

                log.LogMethodExit(securityTokenDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = securityTokenDTOList });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        [HttpPost]
        [Route("api/Security/SecurityTokens")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] SecurityTokenDTO inputTokenDTO)
        {

            SecurityTokenDTO securityTokenDTO = null;
            try
            {
                log.LogMethodEntry(inputTokenDTO);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                try
                {
                    if (inputTokenDTO != null)
                    {
                        int tokenLifeTime = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "JWT_TOKEN_LIFE_TIME", 0);
                        SecurityTokenBL inputTokenBL = new SecurityTokenBL(executionContext);
                        if (!inputTokenBL.ValidateAndUpdateToken(inputTokenDTO.Token, "", false, null, null, tokenLifeTime))
                        {
                            return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = MessageContainerList.GetMessage(executionContext, 1924) });
                        }

                        SecurityTokenListBL securityTokenListBL = new SecurityTokenListBL();
                        List<KeyValuePair<SecurityTokenDTO.SearchByParameters, string>> searchSecurityParameters = new List<KeyValuePair<SecurityTokenDTO.SearchByParameters, string>>();
                        searchSecurityParameters.Add(new KeyValuePair<SecurityTokenDTO.SearchByParameters, string>(SecurityTokenDTO.SearchByParameters.TOKEN, inputTokenBL.GetSecurityTokenDTO.Token));
                        List<SecurityTokenDTO> securityTokenDTOList = securityTokenListBL.GetSecurityTokenDTOList(searchSecurityParameters);

                        if (securityTokenDTOList != null && securityTokenDTOList.Count != 0)
                        {
                            securityTokenDTOList = securityTokenDTOList.OrderByDescending(x => x.LastActivityTime).ToList();
                            return Request.CreateResponse(HttpStatusCode.OK, new { data = securityTokenDTOList[0] });
                        }
                        else
                        {
                            log.Error("Token not found");
                            return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "Token Not Found" });
                        }
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "" });
                    }
                }
                catch (ValidationException valEx)
                {
                    log.Error(valEx);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = valEx.Message });
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = ex.Message });
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = ex.Message });
            }
            //SecurityTokenDTO securityTokenDTO = null;
            //try
            //{
            //    log.LogMethodEntry(inputTokenDTO);
            //    SecurityTokenBL securityTokenBL = new SecurityTokenBL();
            //    var identity = (ClaimsPrincipal)System.Threading.Thread.CurrentPrincipal;
            //    string siteId = identity.FindFirst(ClaimTypes.Sid).Value;
            //    string loginid = identity.FindFirst(ClaimTypes.Name).Value;
            //    string guid = identity.FindFirst(ClaimTypes.UserData).Value;
            //    string language = identity.FindFirst(ClaimTypes.Locality).Value;
            //    string roleid = identity.FindFirst(ClaimTypes.Role).Value;
            //    string machine = identity.FindFirst(ClaimTypes.System).Value;

            //    if (executionContext != null && executionContext.GetSiteId() > 0)
            //        siteId = executionContext.GetSiteId().ToString();


            //    bool isCorporate = true;
            //    if (object.ReferenceEquals(null, HttpContext.Current.Application["IsCorporate"]))
            //    {
            //        // For Web and Guest app, the site selection is not mandatory every time. The site list comes up only if the user has not selected a default site.
            //        // Check and set the IsCorporateFlag once again
            //        SiteList siteList = new SiteList(null);
            //        var content = siteList.GetAllSites(-1, -1, -1);
            //        if (content != null && content.Count > 1)
            //        {
            //            HttpContext.Current.Application["IsCorporate"] = "True";
            //            isCorporate = true;
            //        }
            //        else
            //        {
            //            HttpContext.Current.Application["IsCorporate"] = "False";
            //            isCorporate = false;
            //        }
            //    }
            //    else
            //    {
            //        isCorporate = HttpContext.Current.Application["IsCorporate"].Equals("True");
            //    }

            //    List<KeyValuePair<UsersDTO.SearchByUserParameters, string>> searchByParameters = new List<KeyValuePair<UsersDTO.SearchByUserParameters, string>>();
            //    searchByParameters.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.SITE_ID, siteId));
            //    searchByParameters.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.LOGIN_ID, loginid));

            //    UsersList usersList = new UsersList(null);
            //    List<UsersDTO> usersDTOs = usersList.GetAllUsers(searchByParameters);
            //    if (usersDTOs == null || !usersDTOs.Any())
            //        return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "User not found" });

            //    UsersDTO user = usersDTOs.Find(x => x.LoginId == loginid);

            //    if (user == null)
            //        return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "User not found" });

            //    // for guest app, the user guid is set as the device uuid, set this to the user guid to distinguish the device
            //    //if (user.LoginId.Equals("External POS") || user.LoginId.Equals("CustomerApp"))
            //    if (!String.IsNullOrEmpty(guid))
            //    {
            //        user.Guid = guid;
            //    }

            //    ExecutionContext tempContext = new ExecutionContext(user.LoginId, user.SiteId, -1, user.UserId, isCorporate, -1);
            //    if (inputTokenDTO != null && inputTokenDTO.LanguageId > -1)
            //    {
            //        List<KeyValuePair<LanguagesDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<LanguagesDTO.SearchByParameters, string>>();
            //        searchParameters.Add(new KeyValuePair<LanguagesDTO.SearchByParameters, string>(LanguagesDTO.SearchByParameters.SITE_ID, siteId));
            //        searchParameters.Add(new KeyValuePair<LanguagesDTO.SearchByParameters, string>(LanguagesDTO.SearchByParameters.LANGUAGE_ID, inputTokenDTO.LanguageId.ToString()));
            //        List<LanguagesDTO> languageList = new Semnox.Parafait.Languages.Languages(tempContext).GetAllLanguagesList(searchParameters);
            //        if (languageList == null || !languageList.Any())
            //        {
            //            String message = "Language is not set up.";
            //            log.Error(message);
            //            throw new ValidationException(message);
            //        }
            //        language = languageList.FirstOrDefault().LanguageId.ToString();
            //    }

            //    if (inputTokenDTO != null && inputTokenDTO.MachineId > -1) // Id  wont be there in dto 
            //    {
            //        List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>> searchParameters = new List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>>();
            //        searchParameters.Add(new KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>(POSMachineDTO.SearchByPOSMachineParameters.SITE_ID, siteId));
            //        searchParameters.Add(new KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>(POSMachineDTO.SearchByPOSMachineParameters.POS_MACHINE_ID, inputTokenDTO.MachineId.ToString()));
            //        searchParameters.Add(new KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>(POSMachineDTO.SearchByPOSMachineParameters.ISACTIVE, "1"));
            //        POSMachineList pOSMachineList = new POSMachineList(tempContext);
            //        List<POSMachineDTO> content = pOSMachineList.GetAllPOSMachines(searchParameters, false, false);
            //        if (content == null || !content.Any())
            //        {
            //            String message = "POS Machine is not set up.";
            //            log.Error(message);
            //            throw new ValidationException(message);
            //        }
            //        machine = content.FirstOrDefault().POSMachineId.ToString();
            //    }

            //    int defaultLanguageId = -1;
            //    if (!String.IsNullOrEmpty(language))
            //        int.TryParse(language, out defaultLanguageId);

            //    int machineId = -1;
            //    if (!String.IsNullOrEmpty(machine))
            //        int.TryParse(machine, out machineId);

            //    if (defaultLanguageId == -1)
            //    {
            //        defaultLanguageId = ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "DEFAULT_LANGUAGE");
            //    }

            //    if (isCorporate)
            //    {
            //        securityTokenBL.GenerateNewJWTToken(user.LoginId, user.Guid, user.SiteId.ToString(), Convert.ToString(defaultLanguageId), Convert.ToString(user.RoleId), machineid: machineId.ToString());
            //    }
            //    else
            //    {
            //        securityTokenBL.GenerateNewJWTToken(user.LoginId, user.Guid, "-1", Convert.ToString(defaultLanguageId), Convert.ToString(user.RoleId), machineid: machineId.ToString());
            //    }
            //    securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
            //    executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, machineId, user.UserId, isCorporate, Convert.ToInt32(securityTokenDTO.LanguageId));

            //    log.Debug("Site in context is " + executionContext.GetSiteId());
            //    log.Debug("Corporate flag in context is " + executionContext.GetIsCorporate());
            //    log.Debug("Language is " + executionContext.GetLanguageId());
            //    log.Debug("POSMachine is " + executionContext.GetMachineId());

            //    Request.Headers.Authorization = new AuthenticationHeaderValue(securityTokenDTO.Token);
            //    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, new { securityTokenDTO.Token });
            //    return response;
            //}
            //catch (Exception ex)
            //{
            //    log.LogMethodExit(ex.Message);
            //    Request.Headers.Authorization = new AuthenticationHeaderValue(securityTokenDTO.Token);
            //    return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = ex.Message });
            //}
        }

    }
}
