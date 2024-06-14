/********************************************************************************************
 * Project Name - Site Setup                                                                     
 * Description  - Controller of the custom attributes list
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.60         08-Mar-2019   Jagan Mohana         Created 
 *2.60         26-Apr-2019   Mushahid Faizan      Modified- Added log Method Entry & Exit &
                                                           Declared Global ExecutionContext, SecurityTokenDTO, SecurityTokenBL.
 *2.80        05-Apr-2020    Girish Kundar        Modified: Moved to common folder ,API end point change and removed token from the body 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Semnox.Core.Utilities;
using Semnox.Core.GenericUtilities;
using Semnox.CommonAPI.Helpers;

namespace Semnox.CommonAPI.Common
{
    public class CustomAttributeController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Get the JSON Object custom attributes List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Common/CustomAttributes")]
        public HttpResponseMessage Get(int siteId = -1, string isActive = "1")
        {
            try
            {
                log.LogMethodEntry();
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                CustomAttributesListBL customAttributesListBL = new CustomAttributesListBL(executionContext);
                if(siteId == -1)
                {
                    siteId = executionContext.SiteId;
                }
                List<KeyValuePair<CustomAttributesDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CustomAttributesDTO.SearchByParameters, string>>
                {
                    new KeyValuePair<CustomAttributesDTO.SearchByParameters, string>(CustomAttributesDTO.SearchByParameters.SITE_ID, Convert.ToString(executionContext.GetSiteId())),
                    new KeyValuePair<CustomAttributesDTO.SearchByParameters, string>(CustomAttributesDTO.SearchByParameters.IS_ACTIVE, isActive)
                };

                var content = customAttributesListBL.GetCustomAttributesDTOList(searchParameters, true);
                log.LogMethodExit(content);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ex.Message });
            }
        }

        /// <summary>
        /// Performs a Post operation on custom attributes details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/Common/CustomAttributes")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<CustomAttributesDTO> customAttributesDTOList)
        {
            try
            {
                log.LogMethodEntry(customAttributesDTOList);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                if (customAttributesDTOList != null)
                {
                    // if customAttributesDTOList.customAttributeId is less than zero then insert or else update
                    CustomAttributesListBL customAttributesListBL = new CustomAttributesListBL(executionContext, customAttributesDTOList);
                    customAttributesListBL.SaveUpdateCustomAttributesList();
                    log.LogMethodExit(customAttributesDTOList);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = customAttributesDTOList });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "" });
                }
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

        /// <summary>
        /// Performs a Delete operation on custom attributes details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpDelete]
        [Route("api/Common/CustomAttributes")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody] List<CustomAttributesDTO> customAttributesDTOList)
        {
            try
            {
                log.LogMethodEntry(customAttributesDTOList);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                if (customAttributesDTOList != null)
                {
                    CustomAttributesListBL countryDTOList = new CustomAttributesListBL(executionContext, customAttributesDTOList);
                    countryDTOList.SaveUpdateCustomAttributesList();
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "" });
                }
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
