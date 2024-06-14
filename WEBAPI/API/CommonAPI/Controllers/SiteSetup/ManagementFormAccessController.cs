/********************************************************************************************
 * Project Name - User Role
 * Description  -  Controller of the User Roles Management Form Access Controller class.
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By      Remarks          
 *********************************************************************************************
 *2.70.0      25-Jun-2019   Mushahid Faizan  Created
  ********************************************************************************************/
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Semnox.Core.Utilities;
using System.Web;
using System;
using Semnox.Parafait.Printer;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.User;

namespace Semnox.CommonAPI.SiteSetup
{
    public class ManagementFormAccessController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private SecurityTokenDTO securityTokenDTO;
        private SecurityTokenBL securityTokenBL = new SecurityTokenBL();

        /// <summary>
        /// Get the JSON Object User Roles Management Form Access List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/SiteSetup/ManagementFormAccess/")]
        public HttpResponseMessage Get(string userRoleId)
        {
            try
            {
                log.LogMethodEntry(userRoleId);
                this.securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>(ManagementFormAccessDTO.SearchByParameters.ROLE_ID, userRoleId.ToString()));

                ManagementFormAccessListBL managementFormAccessListBL = new ManagementFormAccessListBL(executionContext);
                var content = managementFormAccessListBL.GetManagementFormAccessDTOList(searchParameters);
                log.LogMethodExit(content);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content, token = securityTokenDTO.Token });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, token = securityTokenDTO.Token });
            }
        }

        /// <summary>
        /// Performs a Post operation on managementFormAccessDTOList details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/SiteSetup/ManagementFormAccess/")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<ManagementFormAccessDTO> managementFormAccessDTOList)
        {
            try
            {
                log.LogMethodEntry(managementFormAccessDTOList);
                this.securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId)); ;
                if (managementFormAccessDTOList != null)
                {
                    // if managementFormAccessDTOList.managementFormAccessId is less than zero then insert or else update
                    ManagementFormAccessListBL managementFormAccessListBL = new ManagementFormAccessListBL(executionContext, managementFormAccessDTOList);
                    managementFormAccessListBL.SaveUpdateManagementFormAccessList();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "", token = securityTokenDTO.Token });
                }
                else
                {
                    log.LogMethodEntry();
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { data = "", token = securityTokenDTO.Token });
                }
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, token = securityTokenDTO.Token });
            }
        }        

    }
}
