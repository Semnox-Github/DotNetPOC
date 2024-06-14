/********************************************************************************************
 * Project Name - User Role
 * Description  -  Controller of the User Roles Access Controller class.
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By               Remarks          
 *********************************************************************************************
  2.150.01    22-Feb-2023     Yashodhara C H      Created
  ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.User;
using Semnox.CommonAPI.Helpers;
using System.Threading.Tasks;

namespace Semnox.CommonAPI.HR
{
    public class UserAccessController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
       

        /// <summary>
        /// Get the JSON Object User Access List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/HR/UserAccess")]
        public async Task<HttpResponseMessage> Get()
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry();
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);

                
                IUserUseCases userUseCases = UserUseCaseFactory.GetUserUseCases(executionContext);
                List<ManagementFormAccessContainerDTO> managementFormAccessContainerDTOList = await userUseCases.GetUserManagementFormAccess();

                log.LogMethodExit(managementFormAccessContainerDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = managementFormAccessContainerDTOList });
            }
            catch (ValidationException ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
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
