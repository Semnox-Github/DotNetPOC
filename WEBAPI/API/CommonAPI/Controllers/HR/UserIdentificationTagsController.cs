using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.User;

namespace Semnox.CommonAPI.Controllers.HR
{
    public class UserIdentificationTagsController: ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        /// <summary>
        /// Performs a Put operation on UserIdentificationTagsDTO details
        /// </summary>         
        /// <returns>HttpResponseMessage</returns>
        [HttpPut]
        [Route("api/HR/Users/{userId}/Tags/{tagId}/Status")]
        [Authorize]
        public async Task<HttpResponseMessage> Put([FromUri]int userId, [FromUri]int tagId, UserIdentificationTagsDTO userIdentificationTagsDTO)
        {
            log.LogMethodEntry(userId, tagId, userIdentificationTagsDTO);
            ExecutionContext executionContext = null;
            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                if (userId < 0 && tagId < 0)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                IUserUseCases userUseCases = UserUseCaseFactory.GetUserUseCases(executionContext);
                UserIdentificationTagsDTO result = await userUseCases.UpdateUserIdentificationTagStatus(userId, tagId, userIdentificationTagsDTO);
                log.LogMethodExit(result);
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    data = result,
                });
            }
            catch (UnauthorizedException ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
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