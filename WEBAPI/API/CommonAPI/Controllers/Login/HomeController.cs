/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - API for the Home Landing Page for Access the forms for current user and site id
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.40        23-Oct-2018   Jagan          Created 
 *2.60        08-May-2019   Nitin          Added [FromBody] tag for post method for guest app
 *2.80        15-Oct-2019   Nitin          Guest App Phase 2 changes
 *2.80       08-Apr-2020      Nitin Pai      Cobra changes for Waiver, Customer Registration and Online Sales
 *2.150.3     27-APR-2023   Sweedol             Modified: Management form access new architecture changes
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using Semnox.Core.Utilities;
using Semnox.Parafait.Site;
using Semnox.Parafait.User;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Semnox.Parafait.POS;

namespace Semnox.CommonAPI.Games.Controllers.Login
{
    public class HomeController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Get the JSON Object List of Sites
        /// </summary>        
        /// <returns>HttpMessgae</returns>
        [Route("api/Home/Sites/")]
        [Authorize]
        [HttpGet]
        public HttpResponseMessage Get()
        {
            SecurityTokenDTO securityTokenDTO = null;
            try
            {
                log.LogMethodEntry();
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                SiteList siteList = new SiteList(null);
                var content = siteList.GetAllSitesUserLogedIn(securityTokenDTO.LoginId);
                //if (content != null && content.Count > 1)
                //{
                //    HttpContext.Current.Application["IsCorporate"] = "True";
                //}
                //else
                //{
                //    HttpContext.Current.Application["IsCorporate"] = "False";
                //}

                log.LogMethodExit(content);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content, token = securityTokenDTO.Token });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ex.Message, token = securityTokenDTO.Token });
            }
        }
        /// <summary>
        /// Post the JSON Object List of forms access for current site id
        /// </summary>        
        /// <returns>HttpMessgae</returns>
        [Route("api/Home/Sites/")]
        [Authorize]
        [HttpPost]
        public HttpResponseMessage Post([FromBody] string siteId)
        {
            SecurityTokenDTO securityTokenDTO = null;
            try
            {
                log.LogMethodEntry(siteId);

                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                var identity = (ClaimsPrincipal)System.Threading.Thread.CurrentPrincipal;
                string loginid = identity.FindFirst(ClaimTypes.Name).Value;
                string guid = identity.FindFirst(ClaimTypes.UserData).Value;

                if (object.ReferenceEquals(null,HttpContext.Current.Application["IsCorporate"]))
                {
                    // For Web and Guest app, the site selection is not madatory every time. The site list comes up only if the user has not selected a default site.
                    // Check and set the IsCorporateFlag once again
                    SiteList siteList = new SiteList(null);
                    var content = siteList.GetAllSites(-1,-1,-1);

                    if (content != null && content.Count > 1)
                        HttpContext.Current.Application["IsCorporate"] = "True";
                    else
                        HttpContext.Current.Application["IsCorporate"] = "False";
                }


                List<KeyValuePair<UsersDTO.SearchByUserParameters, string>> searchByParameters = new List<KeyValuePair<UsersDTO.SearchByUserParameters, string>>();
                // Commenting this as the guest app user will be created in HQ and not in site
                //if (!loginid.Equals("External POS")  || user.LoginId.Equals("CustomerApp"))
                //{
                searchByParameters.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.SITE_ID, siteId.ToString()));
                //}
                searchByParameters.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.LOGIN_ID, loginid));

                UsersList usersList = new UsersList(null);
                List<UsersDTO> usersDTOs = usersList.GetAllUsers(searchByParameters);
                if(usersDTOs == null)
                {
                    throw new ValidationException("No user found in this site");
                }
                UsersDTO user = usersDTOs.Find(x => x.LoginId == loginid);

                // for guest app, the user guid is set as the device uuid, set this to the user guid to distinguish the device
                //if (user.LoginId.Equals("External POS") || user.LoginId.Equals("CustomerApp"))
                if(!String.IsNullOrEmpty(guid))
                {
                    user.Guid = guid;
                }

                ExecutionContext executionContext = new ExecutionContext(user.LoginId, user.SiteId, -1, user.UserId, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), -1);
                //int defaultLanguageId = ParafaitDefaultContainer.GetParafaitDefault<int>(executionContext, "DEFAULT_LANGUAGE");
                List<KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>> searchParafaitDefaultsParameters = new List<KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>>
                {
                    new KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>(ParafaitDefaultsDTO.SearchByParameters.SITE_ID, Convert.ToString(user.SiteId))
                };
                //int defaultLanguageId = ParafaitDefaultContainer.GetParafaitDefault<int>(executionContext, "DEFAULT_LANGUAGE");
                int defaultLanguageId = -1;
                int posMachineId = -1;
                POSMachineContainerDTO pOSMachineContainerDTO = POSMachineContainerList.GetPOSMachineContainerDTOOrDefault(SiteContainerList.IsCorporate() ? user.SiteId : -1,executionContext.POSMachineName, "", -1);
                if (pOSMachineContainerDTO != null)
                {
                    posMachineId = pOSMachineContainerDTO.POSMachineId;
                }
                searchParafaitDefaultsParameters.Add(new KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>(ParafaitDefaultsDTO.SearchByParameters.DEFAULT_VALUE_NAME, "DEFAULT_LANGUAGE"));
                ParafaitDefaultsListBL parafaitDefaultsListBL = new ParafaitDefaultsListBL(executionContext);
                List<ParafaitDefaultsDTO> parafaitDefaultsDTOList = parafaitDefaultsListBL.GetParafaitDefaultsDTOList(searchParafaitDefaultsParameters);
                if (parafaitDefaultsDTOList != null && parafaitDefaultsDTOList.Count != 0)
                {
                    defaultLanguageId = Convert.ToInt32(parafaitDefaultsDTOList[0].DefaultValue);
                }
                int tokenLifeTime = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "JWT_TOKEN_LIFE_TIME", 0);
                if (Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]))
                {
                    securityTokenBL.GenerateNewJWTToken(user.LoginId, user.Guid, user.SiteId.ToString(), Convert.ToString(defaultLanguageId), Convert.ToString(user.RoleId),"User", posMachineId.ToString(), Guid.NewGuid().ToString(), tokenLifeTime);
                }
                else
                {
                    securityTokenBL.GenerateNewJWTToken(user.LoginId, user.Guid, "-1", Convert.ToString(defaultLanguageId), Convert.ToString(user.RoleId), "User", posMachineId.ToString(), Guid.NewGuid().ToString(), tokenLifeTime);
                }
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                log.Debug("Site in context is " + executionContext.GetSiteId());
                log.Debug("Corporate flag in context is " + executionContext.GetIsCorporate());

                List<KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>(ManagementFormAccessDTO.SearchByParameters.SITE_ID, Convert.ToString(securityTokenDTO.SiteId)));
                searchParameters.Add(new KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>(ManagementFormAccessDTO.SearchByParameters.ROLE_ID, Convert.ToString(user.RoleId)));
                searchParameters.Add(new KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>(ManagementFormAccessDTO.SearchByParameters.ISACTIVE, "1"));
                ManagementFormAccessListBL managementFormAccessListBL = new ManagementFormAccessListBL(executionContext);
                List<ManagementFormAccessDTO> managementFormAccessDTO = managementFormAccessListBL.GetManagementFormAccessDTOList(searchParameters);
                /// if managementFormAccessDTO is null, All the links in the UI should not be enabled.
                log.LogMethodExit(managementFormAccessDTO);
                Users users = new Users(executionContext, user.UserId, true, true);
                Request.Headers.Authorization = new AuthenticationHeaderValue(securityTokenDTO.Token);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, new { firstname = user.UserName, userDTO = users.UserDTO, managementFormAccess = managementFormAccessDTO, token = securityTokenDTO.Token });
                return response;
            }
            catch (Exception ex)
            {
                log.LogMethodExit(ex.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ex.Message });
            }
        }
    }
}