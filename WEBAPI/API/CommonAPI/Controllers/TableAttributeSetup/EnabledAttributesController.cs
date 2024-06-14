/********************************************************************************************
 * Project Name - CommonAPI                                                                    
 * Description  - Controller of the EnabledAttributes.
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.140.0    20-Aug-2021     Fiona               Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Threading.Tasks;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.TableAttributeSetup;
using Semnox.CommonAPI.Helpers;

namespace Semnox.CommonAPI.Controllers.TableAttributeSetup
{
    public class EnabledAttributesController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Get Payment Gateways permitted for the respective device.
        /// </summary>
        [HttpGet]
        [Route("api/TableAttributeSetUp/EnabledAttributes")]
        [Authorize]
        public async Task<HttpResponseMessage> Get(int enabledAttributesId = -1, string isActive = null, string tableName = null, string recordGuid = null, string mandatoryOrOptional = null, string enabledAttributeName = null)
        {
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(enabledAttributesId);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                List<EnabledAttributesDTO> enabledAttributesDTOList = new List<EnabledAttributesDTO>();
                List<int> EnabledAttributesIdList = new List<int>();
               
                List<KeyValuePair<EnabledAttributesDTO.SearchByParameters, string>> searchEnabledAttributesParameters = new List<KeyValuePair<EnabledAttributesDTO.SearchByParameters, string>>();
                searchEnabledAttributesParameters.Add(new KeyValuePair<EnabledAttributesDTO.SearchByParameters, string>(EnabledAttributesDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchEnabledAttributesParameters.Add(new KeyValuePair<EnabledAttributesDTO.SearchByParameters, string>(EnabledAttributesDTO.SearchByParameters.IS_ACTIVE, isActive.ToString()));
                    }
                }

                if (enabledAttributesId > -1)
                {
                    searchEnabledAttributesParameters.Add(new KeyValuePair<EnabledAttributesDTO.SearchByParameters, string>(EnabledAttributesDTO.SearchByParameters.ENABLED_ATTRIBUTE_ID, enabledAttributesId.ToString()));
                }
                if (string.IsNullOrEmpty(tableName) == false)
                {
                    searchEnabledAttributesParameters.Add(new KeyValuePair<EnabledAttributesDTO.SearchByParameters, string>(EnabledAttributesDTO.SearchByParameters.TABLE_NAME, tableName.ToString()));
                }
                if (string.IsNullOrEmpty(recordGuid) == false)
                {
                    searchEnabledAttributesParameters.Add(new KeyValuePair<EnabledAttributesDTO.SearchByParameters, string>(EnabledAttributesDTO.SearchByParameters.RECORD_GUID, recordGuid.ToString()));
                }
                if (string.IsNullOrEmpty(mandatoryOrOptional) == false)
                {
                    searchEnabledAttributesParameters.Add(new KeyValuePair<EnabledAttributesDTO.SearchByParameters, string>(EnabledAttributesDTO.SearchByParameters.MANDATORY_OR_OPTIONAL, mandatoryOrOptional.ToString()));
                }
                if (string.IsNullOrEmpty(enabledAttributeName) == false)
                {
                    searchEnabledAttributesParameters.Add(new KeyValuePair<EnabledAttributesDTO.SearchByParameters, string>(EnabledAttributesDTO.SearchByParameters.ENABLED_ATTRIBUTE_NAME, enabledAttributeName.ToString()));
                }
                IEnabledAttributesUseCases enabledAttributessUseCases = EnabledAttributesUseCaseFactory.GetEnabledAttributesUseCases(executionContext);
                enabledAttributesDTOList = await enabledAttributessUseCases.GetEnabledAttributes(searchEnabledAttributesParameters);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = enabledAttributesDTOList });

            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }
        /// <summary>
        /// Performs a Post operation on payment modes details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/TableAttributeSetUp/EnabledAttributes")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody] List<EnabledAttributesDTO> enabledAttributesDTOList)
        {
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(enabledAttributesDTOList);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                if (enabledAttributesDTOList != null && enabledAttributesDTOList.Count > 0)
                {
                    IEnabledAttributesUseCases enabledAttributessUseCases = EnabledAttributesUseCaseFactory.GetEnabledAttributesUseCases(executionContext);
                    var content = await enabledAttributessUseCases.SaveEnabledAttributes(enabledAttributesDTOList);
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