/********************************************************************************************
 * Project Name - Redemption
 * Description  - Created to fetch the  Application Remarks.
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.110.00   09-Dec-2020   Vikas Dwivedi            Created.
 ***************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Redemption;

namespace Semnox.CommonAPI.Controllers.Redemption
{
    public class ApplicationRemarksController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON String
        /// </summary>
        [HttpGet]
        [Authorize]
        [Route("api/Redemption/ApplicationRemarks")]
        public async Task<HttpResponseMessage> Get(string moduleName = null, string sourceName = null)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(moduleName, sourceName);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);

                List<KeyValuePair<ApplicationRemarksDTO.SearchByApplicationRemarksParameters, string>> applicationRemarksSearchParams = new List<KeyValuePair<ApplicationRemarksDTO.SearchByApplicationRemarksParameters, string>>();
                applicationRemarksSearchParams.Add(new KeyValuePair<ApplicationRemarksDTO.SearchByApplicationRemarksParameters, string>(ApplicationRemarksDTO.SearchByApplicationRemarksParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (!string.IsNullOrEmpty(moduleName))
                {
                    applicationRemarksSearchParams.Add(new KeyValuePair<ApplicationRemarksDTO.SearchByApplicationRemarksParameters, string>(ApplicationRemarksDTO.SearchByApplicationRemarksParameters.MODULE_NAME, moduleName));
                }
                if (!string.IsNullOrEmpty(sourceName))
                {
                    applicationRemarksSearchParams.Add(new KeyValuePair<ApplicationRemarksDTO.SearchByApplicationRemarksParameters, string>(ApplicationRemarksDTO.SearchByApplicationRemarksParameters.SOURCE_NAME, sourceName));
                }

                IRedemptionUseCases redemptionUseCase = RedemptionUseCaseFactory.GetRedemptionUseCases(executionContext);
                List<ApplicationRemarksDTO> applicationRemarksDTOList = await redemptionUseCase.GetApplicationRemarks(applicationRemarksSearchParams);
                log.LogMethodExit(); 
                return Request.CreateResponse(HttpStatusCode.OK, new { data = applicationRemarksDTOList });

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
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }
        /// <summary>
        /// Post the JSON Object Application Remarks
        /// </summary>
        /// <param name="applicationRemarksDTOList"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Redemption/ApplicationRemarks")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody] List<ApplicationRemarksDTO> applicationRemarksDTOList)
        {

            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(applicationRemarksDTOList);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);

                if (applicationRemarksDTOList != null && applicationRemarksDTOList.Any())
                {
                    IRedemptionUseCases redemptionUseCases = RedemptionUseCaseFactory.GetRedemptionUseCases(executionContext);
                    ApplicationRemarksDTO content = await redemptionUseCases.SaveApplicationRemarks(applicationRemarksDTOList);
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = content });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = string.Empty });
                }
            }
            catch (UnauthorizedException ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
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
