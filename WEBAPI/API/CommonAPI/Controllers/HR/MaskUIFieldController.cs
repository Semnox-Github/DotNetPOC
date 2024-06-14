/********************************************************************************************
 * Project Name - CommonAPI
 * Description  - API to get the list of UI fields to hide.
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By               Remarks          
 *********************************************************************************************
 *2.151.0    04-Oct-2023    Abhishek                  Created: Web Inventory Redesign.
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
    public class MaskUIFieldController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext = null;

        /// <summary>
        /// Get the JSON Object UserRolesDataAccessRule List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/HR/MaskUIFields")]
        public async Task<HttpResponseMessage> Get(string uiName)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(uiName);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                IDataAccessRuleUseCases dataAccessRuleUseCases = UserUseCaseFactory.GetDataAccessRuleUseCases(executionContext);
                List<EntityExclusionDetailDTO> entityExclusionDetailDTOList = await dataAccessRuleUseCases.GetMaskUIFields(uiName);
                log.LogMethodExit(entityExclusionDetailDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    data = entityExclusionDetailDTOList,
                });
            }
            catch (ValidationException ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
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
