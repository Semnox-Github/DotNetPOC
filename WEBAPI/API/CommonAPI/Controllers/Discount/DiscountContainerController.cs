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
    public class DiscountContainerController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        [HttpGet]
        [Route("api/Discount/DiscountContainer")]
        [Authorize]
        public async Task<HttpResponseMessage> Get(int siteId = -1, string hash = null, bool rebuildCache = false)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(rebuildCache, hash);
                DiscountContainerDTOCollection discountContainerDTOCollection = await
                           Task<DiscountContainerDTOCollection>.Factory.StartNew(() =>
                           {
                               if (rebuildCache)
                               {
                                   DiscountViewContainerList.Rebuild(siteId);
                               }
                               return discountContainerDTOCollection = DiscountViewContainerList.GetDiscountContainerDTOCollection(siteId, hash);
                           });
                log.LogMethodExit(discountContainerDTOCollection);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = discountContainerDTOCollection });
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
