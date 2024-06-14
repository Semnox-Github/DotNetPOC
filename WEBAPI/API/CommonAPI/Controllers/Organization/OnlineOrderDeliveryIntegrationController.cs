/********************************************************************************************
 * Project Name - CommonAPI
 * Description  - Save/Get OnlineOrderDeliveryIntegration 
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By                Remarks          
 *********************************************************************************************
 *2.150.0     13-Jul-2022     Guru S A                  Created
 * ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks; 
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.DeliveryIntegration; 

namespace Semnox.CommonAPI.Organization
{
    /// <summary>
    /// OnlineOrderDeliveryIntegrationController
    /// </summary>
    public class OnlineOrderDeliveryIntegrationController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);       
        /// <summary>
        /// Get the JSON Object  
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Route("api/Organization/DeliveryIntegration")]
        [Authorize]
        public async Task<HttpResponseMessage> Get(string integrationName = null, int siteId = -1, int deliveryIntegrationId = -1, bool loadChildRecords = false, bool loadActiveChildRecords = false, bool? isActive = null)
        {
            log.LogMethodEntry(integrationName, siteId, deliveryIntegrationId, loadChildRecords, loadActiveChildRecords, isActive);
            ExecutionContext executionContext = null;
            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                if (siteId < 0)
                {
                    siteId = executionContext.GetSiteId();
                }
                List<KeyValuePair<OnlineOrderDeliveryIntegrationDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<OnlineOrderDeliveryIntegrationDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<OnlineOrderDeliveryIntegrationDTO.SearchByParameters, string>(OnlineOrderDeliveryIntegrationDTO.SearchByParameters.SITE_ID, siteId.ToString()));
                if (string.IsNullOrWhiteSpace(integrationName) == false)
                {
                    searchParameters.Add(new KeyValuePair<OnlineOrderDeliveryIntegrationDTO.SearchByParameters, string>(OnlineOrderDeliveryIntegrationDTO.SearchByParameters.INTEGRATION_NAME, integrationName));
                }
                if (deliveryIntegrationId > -1)
                {
                    searchParameters.Add(new KeyValuePair<OnlineOrderDeliveryIntegrationDTO.SearchByParameters, string>(OnlineOrderDeliveryIntegrationDTO.SearchByParameters.DELIVERY_INTEGRATION_ID, deliveryIntegrationId.ToString()));
                }
                if (isActive != null)
                {
                    searchParameters.Add(new KeyValuePair<OnlineOrderDeliveryIntegrationDTO.SearchByParameters, string>(OnlineOrderDeliveryIntegrationDTO.SearchByParameters.IS_ACTIVE, (bool)isActive ? "1":"0"));
                }
                IOnlineOrderDeliveryIntegrationUseCases deliveryIntegrationUseCases 
                                                     = OnlineOrderDeliveryIntegrationUseCaseFactory.GetOnlineOrderDeliveryIntegrationUseCases(executionContext);
                List<OnlineOrderDeliveryIntegrationDTO> result = await deliveryIntegrationUseCases.GetOnlineOrderDeliveryIntegration(searchParameters, loadChildRecords, loadActiveChildRecords);
                log.LogMethodExit(result);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = result });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }

        /// <summary>
        /// Performs a Post operation on DTO details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/Organization/DeliveryIntegration")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody] List<OnlineOrderDeliveryIntegrationDTO> onlineOrderDeliveryIntegrationDTOList)
        {
            log.LogMethodEntry(onlineOrderDeliveryIntegrationDTOList);
            ExecutionContext executionContext = null;
            try
            { 
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                IOnlineOrderDeliveryIntegrationUseCases deliveryIntegrationUseCases 
                    = OnlineOrderDeliveryIntegrationUseCaseFactory.GetOnlineOrderDeliveryIntegrationUseCases(executionContext);
                List<OnlineOrderDeliveryIntegrationDTO> result = await deliveryIntegrationUseCases.SaveOnlineOrderDeliveryIntegration(onlineOrderDeliveryIntegrationDTOList);
                log.LogMethodExit(result);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = result });
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