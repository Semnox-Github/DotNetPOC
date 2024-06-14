/********************************************************************************************
 * Project Name - Maintenance
 * Description  - Create and update Comments
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.150.3    29-Mar-2022    Abhishek       Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Maintenance;
namespace Semnox.CommonAPI.Controllers.Maintenance
{
    public class MaintenanceCommentsController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get request for Maintenance Comments
        /// </summary>
        /// <param name="jobId">jobId</param>
        /// <param name="isActive">isActive</param>
        /// <param name="commentId">commentId</param>
        /// <param name="commentType">commentType</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("api/Maintenance/{jobId}/Comments")]
        public async Task<HttpResponseMessage> Get([FromUri]int jobId, string isActive = null, int commentId = -1, int commentType = -1)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(jobId, isActive, commentId, commentType);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                IMaintenanceCommentsUseCases maintenanceCommentsUseCases = MaintenanceUseCaseFactory.GetMaintenanceCommentsUseCases(executionContext);
                List<MaintenanceCommentsDTO> maintenanceCommentsDTOList = await maintenanceCommentsUseCases.GetMaintenanceCommentsDTOList(jobId, isActive, commentId, commentType);
                log.LogMethodExit(maintenanceCommentsDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = maintenanceCommentsDTOList });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        /// <summary>
        /// Post request for Maintenance Comments
        /// </summary>
        /// <param name="maintenanceCommentsDTOList"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("api/Maintenance/{jobId}/Comments")]
        public async Task<HttpResponseMessage> Post([FromUri]int jobId, [FromBody] List<MaintenanceCommentsDTO> maintenanceCommentsDTOList)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(jobId, maintenanceCommentsDTOList);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                IMaintenanceCommentsUseCases maintenanceCommentsUseCases = MaintenanceUseCaseFactory.GetMaintenanceCommentsUseCases(executionContext);
                List<MaintenanceCommentsDTO> result = await maintenanceCommentsUseCases.SaveMaintenanceComments(jobId, maintenanceCommentsDTOList);
                log.LogMethodExit(maintenanceCommentsDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = maintenanceCommentsDTOList });
            }
            catch (ValidationException valEx)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(valEx, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }
    }
}
