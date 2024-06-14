/********************************************************************************************
 * Project Name - Maintenance
 * Description  - Create and update Maintenance Images
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
    public class MaintenanceImagesController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get request for Maintenance Images
        /// </summary>
        /// <param name="jobId">jobId</param>
        /// <param name="isActive">isActive</param>
        /// <param name="imageId">imageId</param>
        /// <param name="imageType">imageType</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("api/Maintenance/{jobId}/Images")]
        public async Task<HttpResponseMessage> Get([FromUri]int jobId, string isActive = null, int imageId = -1, int imageType = -1)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(jobId, isActive, imageId, imageType);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                IMaintenanceImagesUseCases maintenanceImagesUseCases = MaintenanceUseCaseFactory.GetMaintenanceImagesUseCases(executionContext);
                List<MaintenanceImagesDTO> maintenanceImagesDTOList = await maintenanceImagesUseCases.GetMaintenanceImagesDTOList(jobId, isActive, imageId, imageType);
                log.LogMethodExit(maintenanceImagesDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = maintenanceImagesDTOList });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        /// <summary>
        /// Post request for Maintenance Images
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="maintenanceImagesDTOList"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("api/Maintenance/{jobId}/Images")]
        public async Task<HttpResponseMessage> Post([FromUri]int jobId, [FromBody] List<MaintenanceImagesDTO> maintenanceImagesDTOList)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(jobId, maintenanceImagesDTOList);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                IMaintenanceImagesUseCases maintenanceImagesUseCases = MaintenanceUseCaseFactory.GetMaintenanceImagesUseCases(executionContext);
                List<MaintenanceImagesDTO> result = await maintenanceImagesUseCases.SaveMaintenanceImages(jobId, maintenanceImagesDTOList);
                log.LogMethodExit(result);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = result });
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
