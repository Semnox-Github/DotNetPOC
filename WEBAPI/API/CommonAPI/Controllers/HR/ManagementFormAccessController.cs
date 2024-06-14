/********************************************************************************************
 * Project Name - User Role
 * Description  -  Controller of the User Roles Management Form Access Controller class.
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By               Remarks          
 *********************************************************************************************
 *2.70.0      25-Jun-2019   Mushahid Faizan           Created
*2.90.0       14-Jun-2020   Girish Kundar             Modified : REST API phase 2 changes/standard 
 *2.110.0     08-sept-2020  Mushahid Faizan         Modified : Added Search parameter in get and Catch block in post.
*2.120.0     01-Apr-2021   Prajwal S                  Modified.
 *2.150.3     27-APR-2023   Sweedol             Modified: Management form access new architecture changes
  ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Linq;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.User;
using Semnox.CommonAPI.Helpers;
using System.Threading.Tasks;

namespace Semnox.CommonAPI.HR
{
    public class ManagementFormAccessController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
       

        /// <summary>
        /// Get the JSON Object User Roles Management Form Access List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/HR/FormAccess")]
        public async Task<HttpResponseMessage> Get(int userRoleId = -1, int mgmtFormAccessId = -1, string functionGroup = null, string formName = null, string mainMenu = null,
                                       string accessAllowed = null, bool checkManagementFormAccess = false, string isActive = null)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(userRoleId);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>>();
                if (userRoleId > -1)
                {
                    searchParameters.Add(new KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>(ManagementFormAccessDTO.SearchByParameters.ROLE_ID, userRoleId.ToString()));
                }
                if (mgmtFormAccessId > -1)
                {
                    searchParameters.Add(new KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>(ManagementFormAccessDTO.SearchByParameters.MANAGEMENT_FORM_ACCESS_ID, mgmtFormAccessId.ToString()));
                }
                if (!string.IsNullOrEmpty(functionGroup))
                {
                    searchParameters.Add(new KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>(ManagementFormAccessDTO.SearchByParameters.FUNCTION_GROUP, functionGroup.ToString()));
                }
                if (!string.IsNullOrEmpty(formName))
                {
                    searchParameters.Add(new KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>(ManagementFormAccessDTO.SearchByParameters.FORM_NAME, formName.ToString()));
                }
                if (!string.IsNullOrEmpty(mainMenu))
                {
                    searchParameters.Add(new KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>(ManagementFormAccessDTO.SearchByParameters.MAIN_MENU, mainMenu.ToString()));
                }
                if (string.IsNullOrEmpty(accessAllowed) == false)
                {
                    if (accessAllowed.ToString() == "1" || accessAllowed.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>(ManagementFormAccessDTO.SearchByParameters.ACCESS_ALLOWED, accessAllowed.ToString()));
                    }
                }
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>(ManagementFormAccessDTO.SearchByParameters.ISACTIVE, "1"));
                    }
                }
                List<ManagementFormAccessDTO> managementFormAccessDTOList = new List<ManagementFormAccessDTO>();
                // Check for the Mgmt Form Access based on the UserRoleId.
                if (userRoleId > -1 && checkManagementFormAccess)
                {
                    ManagementFormAccessService managementFormAccessServiceBL = new ManagementFormAccessService(executionContext);
                    managementFormAccessDTOList = managementFormAccessServiceBL.GetManagementFormAccessList(userRoleId);
                }
                else
                {
                    IManagementFormAccessUseCases managementFormAccessUseCases = UserUseCaseFactory.GetManagementFormAccessUseCases(executionContext);
                     managementFormAccessDTOList = await managementFormAccessUseCases.GetManagementFormAccessDTOList(searchParameters);
                }

                log.LogMethodExit(managementFormAccessDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = managementFormAccessDTOList });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }

        /// <summary>
        /// Performs a Post operation on managementFormAccessDTOList details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/HR/FormAccess")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody] List<ManagementFormAccessDTO> managementFormAccessDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(managementFormAccessDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (managementFormAccessDTOList == null || managementFormAccessDTOList.Any(a => a.ManagementFormAccessId > -1))
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                if (managementFormAccessDTOList != null && managementFormAccessDTOList.Any())
                {
                    IManagementFormAccessUseCases managementFormAccessUseCases = UserUseCaseFactory.GetManagementFormAccessUseCases(executionContext);
                    managementFormAccessDTOList = await managementFormAccessUseCases.SaveManagementFormAccess(managementFormAccessDTOList);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = managementFormAccessDTOList });
                }
                else
                {
                    log.LogMethodEntry();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "" });
                }
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }

        /// <summary>
        /// Post the ManagementFormAccessList collection
        /// <param name="managementFormAccessDTOList">ManagementFormAccessList</param>
        [HttpPut]
        [Route("api/HR/FormAccess")]
        [Authorize]
        public async Task<HttpResponseMessage> Put([FromBody]List<ManagementFormAccessDTO> managementFormAccessDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(managementFormAccessDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (managementFormAccessDTOList == null || managementFormAccessDTOList.Any(a => a.ManagementFormAccessId < 0))
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                IManagementFormAccessUseCases managementFormAccessUseCases = UserUseCaseFactory.GetManagementFormAccessUseCases(executionContext);
                await managementFormAccessUseCases.SaveManagementFormAccess(managementFormAccessDTOList);
                log.LogMethodExit(managementFormAccessDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = string.Empty });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }

    }
}
