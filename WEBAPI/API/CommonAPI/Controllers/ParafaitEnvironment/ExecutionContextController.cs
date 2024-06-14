/********************************************************************************************
 * Project Name - Execution Context Controller
 * Description  - API for setting the execution context
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.80       08-Apr-2020      Nitin Pai      Cobra changes for Waiver, Customer Registration and Online Sales
 *2.80       28-May-2020      Girish Kundar  Modified : Added GET() method and removed POST() 
 ********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.POS;
using Semnox.Parafait.Site;
using Semnox.Parafait.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Web;
using System.Web.Http;

namespace Semnox.CommonAPI.Controllers.ParafaitEnvironment
{
    public class ExecutionContextController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        SecurityTokenDTO securityTokenDTO = null;
        [HttpGet]
        [Route("api/ParafaitEnvironment/ExecutionContext")]
        [Authorize]
        public HttpResponseMessage Get(string languageCode = null, int siteId = -1 ,string posMachineName = null)
        {
            try
            {
                log.LogMethodEntry(languageCode, siteId , posMachineName);
                ExecutionContext executionContext = null;
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                var identity = (ClaimsPrincipal)System.Threading.Thread.CurrentPrincipal;
                string siteid = identity.FindFirst(ClaimTypes.Sid).Value;
                string loginid = identity.FindFirst(ClaimTypes.Name).Value;
                string guid = identity.FindFirst(ClaimTypes.UserData).Value;
                string language = identity.FindFirst(ClaimTypes.Locality).Value;
                string roleid = identity.FindFirst(ClaimTypes.Role).Value;
                string machine = identity.FindFirst(ClaimTypes.System).Value;

                if (siteId > 0)
                    siteid = siteId.ToString();
                bool isCorporate = true;
                //if (object.ReferenceEquals(null, HttpContext.Current.Application["IsCorporate"]))
                {
                    // For Web and Guest app, the site selection is not madatory every time. The site list comes up only if the user has not selected a default site.
                    // Check and set the IsCorporateFlag once again
                    SiteList siteList = new SiteList(null);
                    var content = siteList.GetAllSites(-1, -1, -1);
                    if (content != null && content.Count > 1)
                    {
                        HttpContext.Current.Application["IsCorporate"] = "True";
                        isCorporate = true;
                    }
                    else
                    {
                        HttpContext.Current.Application["IsCorporate"] = "False";
                        isCorporate = false;
                    }
                }
                //else
                //{
                //    isCorporate = HttpContext.Current.Application["IsCorporate"].Equals("True");
                //}

                List<KeyValuePair<UsersDTO.SearchByUserParameters, string>> searchByParameters = new List<KeyValuePair<UsersDTO.SearchByUserParameters, string>>();
                searchByParameters.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.SITE_ID, siteid));
                searchByParameters.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.LOGIN_ID, loginid));

                UsersList usersList = new UsersList(null);
                List<UsersDTO> usersDTOs = usersList.GetAllUsers(searchByParameters);
                if (usersDTOs == null || !usersDTOs.Any())
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "User not found" });

                UsersDTO user = usersDTOs.Find(x => x.LoginId == loginid);

                if (user == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "User not found" });

                // for guest app, the user guid is set as the device uuid, set this to the user guid to distinguish the device
                //if (user.LoginId.Equals("External POS") || user.LoginId.Equals("CustomerApp"))
                if (!String.IsNullOrEmpty(guid))
                {
                    user.Guid = guid;
                }

                ExecutionContext tempContext = new ExecutionContext(user.LoginId, user.SiteId, -1, user.UserId, isCorporate, -1);
                if (!String.IsNullOrEmpty(languageCode))
                {
                    List<KeyValuePair<LanguagesDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<LanguagesDTO.SearchByParameters, string>>();
                    searchParameters.Add(new KeyValuePair<LanguagesDTO.SearchByParameters, string>(LanguagesDTO.SearchByParameters.SITE_ID, siteid));
                    searchParameters.Add(new KeyValuePair<LanguagesDTO.SearchByParameters, string>(LanguagesDTO.SearchByParameters.LANGUAGE_CODE, languageCode));
                    List<LanguagesDTO> languageList = new Semnox.Parafait.Languages.Languages(tempContext).GetAllLanguagesList(searchParameters);
                    if (languageList == null || !languageList.Any())
                    {
                        String message = "Language " + languageCode + " is not set up.";
                        log.Error(message);
                        throw new ValidationException(message);
                    }
                    language = languageList.FirstOrDefault().LanguageId.ToString();
                }
                else
                {
                    language = ParafaitDefaultContainerList.GetParafaitDefault(tempContext, "DEFAULT_LANGUAGE", "");
                }

                if (!String.IsNullOrEmpty(posMachineName))
                {
                    List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>> searchParameters = new List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>>();
                    searchParameters.Add(new KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>(POSMachineDTO.SearchByPOSMachineParameters.SITE_ID, siteid));
                    searchParameters.Add(new KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>(POSMachineDTO.SearchByPOSMachineParameters.POS_OR_COMPUTER_NAME, posMachineName));
                    searchParameters.Add(new KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>(POSMachineDTO.SearchByPOSMachineParameters.ISACTIVE, "1"));
                    POSMachineList pOSMachineList = new POSMachineList(tempContext);
                    List<POSMachineDTO> content = pOSMachineList.GetAllPOSMachines(searchParameters, false, false);
                    if (content == null || !content.Any())
                    {
                        String message = "POS Machine " + posMachineName + " is not set up.";
                        log.Error(message);
                        throw new ValidationException(message);
                    }
                    machine = content.FirstOrDefault().POSMachineId.ToString();
                }

                int defaultLanguageId = -1;
                if (!String.IsNullOrEmpty(language))
                    int.TryParse(language, out defaultLanguageId);

                int machineId = -1;
                if (!String.IsNullOrEmpty(machine))
                    int.TryParse(machine, out machineId);

                if (defaultLanguageId == -1)
                {
                    defaultLanguageId = ParafaitDefaultContainerList.GetParafaitDefault<int>(tempContext, "DEFAULT_LANGUAGE");
                }
                int tokenLifeTime = ParafaitDefaultContainerList.GetParafaitDefault(tempContext, "JWT_TOKEN_LIFE_TIME", 0);
                if (isCorporate)
                {
                    securityTokenBL.GenerateNewJWTToken(user.LoginId, user.Guid, user.SiteId.ToString(), Convert.ToString(defaultLanguageId), Convert.ToString(user.RoleId), machineid: machineId.ToString(),userSessionId:  Guid.NewGuid().ToString(), lifeTime: tokenLifeTime);
                }
                else
                {
                    securityTokenBL.GenerateNewJWTToken(user.LoginId, user.Guid, "-1", Convert.ToString(defaultLanguageId), Convert.ToString(user.RoleId), machineid: machineId.ToString(), userSessionId: Guid.NewGuid().ToString(), lifeTime: tokenLifeTime);
                }

                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, machineId, user.UserId, isCorporate, Convert.ToInt32(securityTokenDTO.LanguageId));

                log.Debug("Site in context is " + executionContext.GetSiteId());
                log.Debug("Corporate flag in context is " + executionContext.GetIsCorporate());
                log.Debug("Language is " + executionContext.GetLanguageId());
                log.Debug("POSMachine is " + executionContext.GetMachineId());
                executionContext.POSMachineName = posMachineName;
                executionContext.LanguageCode = languageCode;
                Request.Headers.Authorization = new AuthenticationHeaderValue(securityTokenDTO.Token);
                log.LogMethodExit();
                return Request.CreateResponse(HttpStatusCode.OK, new { data = executionContext });
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ex.Message });
            }
        }

        ///// <summary>
        ///// Post the JSON Object List of forms access for current site id
        ///// </summary>        
        ///// <returns>HttpMessgae</returns>
        //[Route("api/ParafaitEnvironment/ExecutionContext/")]
        //[Authorize]
        //[HttpPost]
        //public HttpResponseMessage Post([FromBody]ExecutionContext executionContext)
        //{
            
        //    try
        //    {
        //        log.LogMethodEntry(executionContext);

        //        SecurityTokenBL securityTokenBL = new SecurityTokenBL();
        //        var identity = (ClaimsPrincipal)System.Threading.Thread.CurrentPrincipal;
        //        string siteId = identity.FindFirst(ClaimTypes.Sid).Value;
        //        string loginid = identity.FindFirst(ClaimTypes.Name).Value;
        //        string guid = identity.FindFirst(ClaimTypes.UserData).Value;
        //        string language = identity.FindFirst(ClaimTypes.Locality).Value;
        //        string roleid = identity.FindFirst(ClaimTypes.Role).Value;
        //        string machine = identity.FindFirst(ClaimTypes.System).Value;

        //        if (executionContext.GetSiteId() > 0)
        //            siteId = executionContext.GetSiteId().ToString();

        //        bool isCorporate = true;
        //        if (object.ReferenceEquals(null, HttpContext.Current.Application["IsCorporate"]))
        //        {
        //            // For Web and Guest app, the site selection is not madatory every time. The site list comes up only if the user has not selected a default site.
        //            // Check and set the IsCorporateFlag once again
        //            SiteList siteList = new SiteList(null);
        //            var content = siteList.GetAllSites(-1, -1, -1);
        //            if (content != null && content.Count > 1)
        //            {
        //                HttpContext.Current.Application["IsCorporate"] = "True";
        //                isCorporate = true;
        //            }
        //            else
        //            {
        //                HttpContext.Current.Application["IsCorporate"] = "False";
        //                isCorporate = false;
        //            }
        //        }
        //        else
        //        {
        //            isCorporate = HttpContext.Current.Application["IsCorporate"].Equals("True");
        //        }

        //        List<KeyValuePair<UsersDTO.SearchByUserParameters, string>> searchByParameters = new List<KeyValuePair<UsersDTO.SearchByUserParameters, string>>();
        //        searchByParameters.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.SITE_ID, siteId));
        //        searchByParameters.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.LOGIN_ID, loginid));

        //        UsersList usersList = new UsersList(null);
        //        List<UsersDTO> usersDTOs = usersList.GetAllUsers(searchByParameters);
        //        if (usersDTOs == null || !usersDTOs.Any())
        //            return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "User not found" });

        //        UsersDTO user = usersDTOs.Find(x => x.LoginId == loginid);

        //        if (user == null)
        //            return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "User not found" });

        //        // for guest app, the user guid is set as the device uuid, set this to the user guid to distinguish the device
        //        //if (user.LoginId.Equals("External POS") || user.LoginId.Equals("CustomerApp"))
        //        if (!String.IsNullOrEmpty(guid))
        //        {
        //            user.Guid = guid;
        //        }

        //        ExecutionContext tempContext = new ExecutionContext(user.LoginId, user.SiteId, -1, user.UserId, isCorporate, -1);

        //        if (!String.IsNullOrEmpty(executionContext.LanguageCode))
        //        {
        //            List<KeyValuePair<LanguagesDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<LanguagesDTO.SearchByParameters, string>>();
        //            searchParameters.Add(new KeyValuePair<LanguagesDTO.SearchByParameters, string>(LanguagesDTO.SearchByParameters.SITE_ID, siteId));
        //            searchParameters.Add(new KeyValuePair<LanguagesDTO.SearchByParameters, string>(LanguagesDTO.SearchByParameters.LANGUAGE_CODE, executionContext.LanguageCode));
        //            List<LanguagesDTO> languageList = new Semnox.Parafait.Languages.Languages(tempContext).GetAllLanguagesList(searchParameters);
        //            if (languageList == null || !languageList.Any())
        //            {
        //                String message = "Language " + executionContext.LanguageCode + " is not set up.";
        //                log.Error(message);
        //                throw new ValidationException(message);
        //            }
        //            language = languageList.FirstOrDefault().LanguageId.ToString();
        //        }

        //        if (!String.IsNullOrEmpty(executionContext.POSMachineName))
        //        {
        //            List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>> searchParameters = new List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>>();
        //            searchParameters.Add(new KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>(POSMachineDTO.SearchByPOSMachineParameters.SITE_ID, siteId));
        //            searchParameters.Add(new KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>(POSMachineDTO.SearchByPOSMachineParameters.POS_OR_COMPUTER_NAME, executionContext.POSMachineName));
        //            searchParameters.Add(new KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>(POSMachineDTO.SearchByPOSMachineParameters.ISACTIVE, "1"));
        //            POSMachineList pOSMachineList = new POSMachineList(tempContext);
        //            List<POSMachineDTO> content = pOSMachineList.GetAllPOSMachines(searchParameters, false, false);
        //            if (content == null || !content.Any())
        //            {
        //                String message = "POS Machine " + executionContext.POSMachineName + " is not set up.";
        //                log.Error(message);
        //                throw new ValidationException(message);
        //            }
        //            machine = content.FirstOrDefault().POSMachineId.ToString();
        //        }

        //        int defaultLanguageId = -1;
        //        if (!String.IsNullOrEmpty(language))
        //            int.TryParse(language, out defaultLanguageId);

        //        int machineId = -1;
        //        if (!String.IsNullOrEmpty(machine))
        //            int.TryParse(machine, out machineId);

        //        if (defaultLanguageId == -1)
        //        {
        //            defaultLanguageId = ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "DEFAULT_LANGUAGE");
        //        }

        //        if (isCorporate)
        //        {
        //            securityTokenBL.GenerateNewJWTToken(user.LoginId, user.Guid, user.SiteId.ToString(), Convert.ToString(defaultLanguageId), Convert.ToString(user.RoleId), machineid: machineId.ToString());
        //        }
        //        else
        //        {
        //            securityTokenBL.GenerateNewJWTToken(user.LoginId, user.Guid, "-1", Convert.ToString(defaultLanguageId), Convert.ToString(user.RoleId), machineid: machineId.ToString());
        //        }
        //        securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
        //        executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, machineId, user.UserId, isCorporate, Convert.ToInt32(securityTokenDTO.LanguageId));

        //        log.Debug("Site in context is " + executionContext.GetSiteId());
        //        log.Debug("Corporate flag in context is " + executionContext.GetIsCorporate());
        //        log.Debug("Language is " + executionContext.GetLanguageId());
        //        log.Debug("POSMachine is " + executionContext.GetMachineId());

        //        Request.Headers.Authorization = new AuthenticationHeaderValue(securityTokenDTO.Token);
        //        HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, new { data = executionContext});
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        log.LogMethodExit(ex.Message);
        //        Request.Headers.Authorization = new AuthenticationHeaderValue(securityTokenDTO.Token);
        //        return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ex.Message});
        //    }
        //}
    }
}