/********************************************************************************************
 * Project Name - User Role
 * Description  -  Controller of the User Roles Refresh Functions.
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By      Remarks          
 *********************************************************************************************
 *2.80.0      14-Oct-2019   Jagan Mohana  Created
  ********************************************************************************************/
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Semnox.Core.Utilities;
using System.Web;
using System;
using Semnox.CommonAPI.Helpers;
using Semnox.Parafait.Printer;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.User;

namespace Semnox.CommonAPI.SiteSetup
{
    public class RefreshFunctionController : ApiController
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
        [Route("api/SiteSetup/RefreshFunction/")]
        public HttpResponseMessage Get(int userRoleId)
        {
            try
            {
                log.LogMethodEntry(userRoleId);
                this.securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>(ManagementFormAccessDTO.SearchByParameters.ROLE_ID, userRoleId.ToString()));

                ManagementFormAccessService managementFormAccessServiceBL = new ManagementFormAccessService(executionContext);
                List<ManagementFormAccessDTO> managementFormAccessDTOList = managementFormAccessServiceBL.GetManagementFormAccessList(userRoleId);
                if(managementFormAccessDTOList != null && managementFormAccessDTOList.Count != 0)
                {
                    ManagementFormAccessListBL managementFormAccessListBL = new ManagementFormAccessListBL(executionContext, managementFormAccessDTOList);
                    managementFormAccessListBL.SaveUpdateManagementFormAccessList();
                }
                log.LogMethodExit();
                return Request.CreateResponse(HttpStatusCode.OK, new { data = "", token = securityTokenDTO.Token });
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
