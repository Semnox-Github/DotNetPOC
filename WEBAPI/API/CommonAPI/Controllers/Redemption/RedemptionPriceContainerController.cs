/********************************************************************************************
 * Project Name - CommonAPI                                                                    
 * Description  - Controller of the MIFARE key controller.
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 2.200.0       17-Nov-2020   Lakshminarayana    Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Parafait.ViewContainer;
using Semnox.Parafait.Redemption;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;

namespace Semnox.CommonAPI.POS.Redemption
{
    public class RedemptionPriceContainerController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object Values List
        /// </summary>       
        /// <returns>HttpResponseRedemptionPrice</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Redemption/RedemptionPriceContainer")]
        public async Task<HttpResponseMessage> Get(int siteId = -1, string hash = null, bool rebuildCache = false)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(siteId, hash, rebuildCache);
                RedemptionPriceContainerDTOCollection RedemptionPriceContainerDTOCollection = await
                           Task<RedemptionPriceContainerDTOCollection>.Factory.StartNew(() => {
                                    if(rebuildCache)
                                    {
                                        RedemptionPriceViewContainerList.Rebuild(siteId);
                                    }
                                    return RedemptionPriceViewContainerList.GetRedemptionPriceContainerDTOCollection(siteId, hash);
                                  });
                log.LogMethodExit(RedemptionPriceContainerDTOCollection);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = RedemptionPriceContainerDTOCollection });
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