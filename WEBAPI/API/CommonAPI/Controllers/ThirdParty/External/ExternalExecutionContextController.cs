/********************************************************************************************
 * Project Name - CommonAPI
 * Description  - Created to set the context.
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.130.7    11-Apr-2022   Abhishek                 Created - External  REST API
 ***************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.POS;
using Semnox.Parafait.Site;
using Semnox.Parafait.User;
using System.Security.Claims;
using System.Linq;

namespace Semnox.CommonAPI.Controllers.ThirdParty.External
{
    public class ExternalExecutionContextController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Gets the JSON Object ExecutionContext
        /// </summary>
        /// <param name="languageCode">languageCode</param>
        /// <param name="siteId">siteId</param>
        /// <param name="posMachineName">posMachineName</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Route("api/External/Environment/ExecutionContext")]
        [Authorize]
        public HttpResponseMessage Get(string languageCode = null, int siteId = -1, string posMachineName = null)
        {
            try
            {
                log.LogMethodEntry(languageCode, siteId, posMachineName);
                SecurityTokenDTO securityTokenDTO = null;
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

                if (isCorporate)
                {
                    securityTokenBL.GenerateNewJWTToken(user.LoginId, user.Guid, user.SiteId.ToString(), Convert.ToString(defaultLanguageId), Convert.ToString(user.RoleId), machineid: machineId.ToString(), userSessionId: Guid.NewGuid().ToString());
                }
                else
                {
                    securityTokenBL.GenerateNewJWTToken(user.LoginId, user.Guid, "-1", Convert.ToString(defaultLanguageId), Convert.ToString(user.RoleId), machineid: machineId.ToString(), userSessionId: Guid.NewGuid().ToString());
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
                return Request.CreateResponse(HttpStatusCode.OK, new { token = securityTokenDTO.Token });
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ex.Message });
            }
        }
    }
}