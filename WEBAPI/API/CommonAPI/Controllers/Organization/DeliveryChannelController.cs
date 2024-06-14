/********************************************************************************************
* Project Name - CommnonAPI - Organization
* Description  - API for the DeliveryChannel Controller.
* 
**************
**Version Log
**************
*Version     Date            Modified By         Remarks          
*********************************************************************************************
*2.120.0     11-Mar-2021    Prajwal S           Created
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
using Semnox.Parafait.DeliveryIntegration;
namespace Semnox.CommonAPI.Organization
{
    /// <summary>
    /// DeliveryChannelController
    /// </summary>
    public class DeliveryChannelController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Performs a Get operation 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("api/Organization/DeliveryChannels")]
        public async Task<HttpResponseMessage> Get(int deliveryChannelId = -1, string channelName = null, string isActive = null)
        {
            log.LogMethodEntry(deliveryChannelId, channelName, isActive);
            ExecutionContext executionContext = null;
            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                List<KeyValuePair<DeliveryChannelDTO.SearchByParameters, string>> deliveryChannelSearchParameters
                                                                 = new List<KeyValuePair<DeliveryChannelDTO.SearchByParameters, string>>();
                deliveryChannelSearchParameters.Add(new KeyValuePair<DeliveryChannelDTO.SearchByParameters,
                                                    string>(DeliveryChannelDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (deliveryChannelId > -1)
                {
                    deliveryChannelSearchParameters.Add(new KeyValuePair<DeliveryChannelDTO.SearchByParameters,
                                                     string>(DeliveryChannelDTO.SearchByParameters.DELIVERY_CHANNEL_ID, deliveryChannelId.ToString()));
                }
                if (!string.IsNullOrWhiteSpace(channelName))
                {
                    deliveryChannelSearchParameters.Add(new KeyValuePair<DeliveryChannelDTO.SearchByParameters,
                                                     string>(DeliveryChannelDTO.SearchByParameters.CHANNEL_NAME, channelName.ToString()));
                }
                if (!string.IsNullOrEmpty(isActive))
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        deliveryChannelSearchParameters.Add(new KeyValuePair<DeliveryChannelDTO.SearchByParameters, string>(DeliveryChannelDTO.SearchByParameters.IS_ACTIVE, isActive.ToString()));
                    }
                }
                IDeliveryChannelUseCases deliveryChannelUseCases = DeliveryChannelUseCaseFactory.GetDeliveryChannelUseCases(executionContext);
                List<DeliveryChannelDTO> deliveryChannelDTOLists = await deliveryChannelUseCases.GetDeliveryChannel(deliveryChannelSearchParameters); 
                log.LogMethodExit(deliveryChannelDTOLists);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = deliveryChannelDTOLists, });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }
        /// <summary>
        /// Performs a Post operation 
        /// </summary>
        /// <param name="deliveryChannelDTOList"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("api/Organization/DeliveryChannels")]
        public async Task<HttpResponseMessage> Post([FromBody] List<DeliveryChannelDTO> deliveryChannelDTOList)
        {
            log.LogMethodEntry(deliveryChannelDTOList);
            ExecutionContext executionContext = null;
            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                IDeliveryChannelUseCases deliveryChannelUseCases = DeliveryChannelUseCaseFactory.GetDeliveryChannelUseCases(executionContext);
                List<DeliveryChannelDTO> deliveryChannelDTOLists = await deliveryChannelUseCases.SaveDeliveryChannel(deliveryChannelDTOList);
                log.LogMethodExit(deliveryChannelDTOLists);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = deliveryChannelDTOLists, });
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
