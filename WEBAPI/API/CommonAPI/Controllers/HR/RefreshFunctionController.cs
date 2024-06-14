/********************************************************************************************
 * Project Name - User Role
 * Description  -  Controller of the User Roles Refresh Functions.
 * 
 **************
 **Version Log
 **************
 *Version     Date                  Modified By             Remarks          
 *********************************************************************************************
 *2.80.0      14-Oct-2019           Jagan Mohana            Created
 *2.90.0      14-Jun-2020           Girish Kundar           Modified : REST API phase 2 changes/standard 
 *2.120.0     01-Apr-2021            Prajwal S              Modified.
  ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.User;
namespace Semnox.CommonAPI.HR
{
    public class RefreshFunctionController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
      

        /// <summary>
        /// Get the JSON Object User Roles Management Form Access List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/HR/RereshFormAccess")]
        public async Task<HttpResponseMessage> Get(int userRoleId =-1)
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
                searchParameters.Add(new KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>(ManagementFormAccessDTO.SearchByParameters.ROLE_ID, userRoleId.ToString()));

                ManagementFormAccessService managementFormAccessServiceBL = new ManagementFormAccessService(executionContext);
                List<ManagementFormAccessDTO> managementFormAccessDTOList = managementFormAccessServiceBL.RefreshUserRoles();
                if(managementFormAccessDTOList != null && managementFormAccessDTOList.Count != 0)
                {
                    IManagementFormAccessUseCases managementFormAccessUseCases = UserUseCaseFactory.GetManagementFormAccessUseCases(executionContext);
                    managementFormAccessDTOList = await managementFormAccessUseCases.SaveManagementFormAccess(managementFormAccessDTOList);
                }
                log.LogMethodExit();
                return Request.CreateResponse(HttpStatusCode.OK, new { data = ""  });
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
