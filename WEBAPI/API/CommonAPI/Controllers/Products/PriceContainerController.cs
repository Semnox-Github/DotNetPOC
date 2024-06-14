/********************************************************************************************
 * Project Name - CommonAPI
 * Description  - Represents the controller class of price container
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.130.1      10-Aug-2021      Lakshminarayana           Created : price container enhancement
 ********************************************************************************************/
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Semnox.Core.Utilities;
using Semnox.Core.GenericUtilities;
using System.Threading.Tasks;
using Semnox.Parafait.ProductPrice;
using Semnox.Parafait.ViewContainer;
using Semnox.CommonAPI.Helpers;

namespace Semnox.CommonAPI.Products
{
    public class PriceContainerController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// Get the JSON Object Values List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Product/PriceContainer")]
        public async Task<HttpResponseMessage> Get(int siteId = -1, int membershipId = -1, int userRoleId = -1, int transactionProfileId = -1, DateTime? startDateTime = null, DateTime? endDateTime = null, string hash = null, bool rebuildCache = false)
        {
            ExecutionContext executionContext = null;
            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                log.LogMethodEntry(siteId, membershipId, userRoleId, transactionProfileId, startDateTime, endDateTime, rebuildCache, hash);
                if (startDateTime.HasValue == false ||
                    endDateTime.HasValue == false ||
                    startDateTime.Value >= endDateTime.Value)
                {
                    log.LogMethodExit(null, "Bad Request");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }

                PriceContainerDTOCollection priceContainerDTOCollection = await
                           Task<PriceContainerDTOCollection>.Factory.StartNew(() =>
                           {
                               if (rebuildCache)
                               {
                                   PriceViewContainerList.Rebuild(siteId, membershipId, userRoleId, transactionProfileId, new DateTimeRange(startDateTime.Value, endDateTime.Value));
                               }
                               return PriceViewContainerList.GetPriceContainerDTOCollection(siteId, membershipId, userRoleId, transactionProfileId, startDateTime.Value, endDateTime.Value, hash);
                           });
                log.LogMethodExit(priceContainerDTOCollection);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = priceContainerDTOCollection });
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