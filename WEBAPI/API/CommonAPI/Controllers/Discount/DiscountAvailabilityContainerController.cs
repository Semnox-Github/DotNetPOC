/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - DiscountsContainerController
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.150.0    12-Apr-2021     Abhishek      Created : POSUI redesign using REST API 
 ********************************************************************************************/
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Discounts;
using Semnox.Parafait.Game;
using Semnox.Parafait.ViewContainer;

namespace Semnox.CommonAPI.Games.Controllers.Games
{
    public class DiscountAvailabilityContainerController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        [HttpGet]
        [Route("api/Discount/DiscountAvailabilityContainer")]
        [Authorize]
        public async Task<HttpResponseMessage> Get(int siteId = -1, string hash = null, bool rebuildCache = false, DateTime? startDate = null, DateTime? endDate = null)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(rebuildCache, startDate, endDate, hash);
                if (startDate == null || endDate == null)
                {
                    log.LogMethodExit(null, "Bad Request");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                DateTime startDateTime = (DateTime)startDate;
                DateTime endDateTime = (DateTime)endDate;
                DiscountAvailabilityContainerDTOCollection discountAvailabilityContainerDTOCollection = await
                           Task<DiscountAvailabilityContainerDTOCollection>.Factory.StartNew(() =>
                           {
                               if (rebuildCache)
                               {
                                   DiscountViewContainerList.Rebuild(siteId);
                               }
                               return discountAvailabilityContainerDTOCollection = DiscountViewContainerList.GetDiscountAvailabilityContainerDTOCollection(siteId, hash, startDateTime, endDateTime);
                           });
                log.LogMethodExit(discountAvailabilityContainerDTOCollection);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = discountAvailabilityContainerDTOCollection });
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
