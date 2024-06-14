/******************************************************************************************************
 * Project Name - CommonAPI
 * Description  - API for the Clear Discounted Games Operation.
 *  
 **************
 **Version Log
 **************
 *Version   Date            Modified By               Remarks          
 ******************************************************************************************************
 *2.170     20-Aug-2023     Lakshminarayana           Created
 *******************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Discounts;

namespace Semnox.CommonAPI.Controllers.Discount
{
    public class ClearDiscountedGamesController : ApiController
    {
        private Semnox.Parafait.logging.Logger log;

        /// <summary>
        /// API for the Clear Discounted Games Operation.
        /// </summary>
        /// <param name="discountId">discountId</param>
        /// <returns>HttpMessgae</returns>
        [HttpPost]
        [Route("api/Discount/ClearDiscountedGames")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody]int discountId)
        {
            ExecutionContext executionContext = null;
            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                log = LogManager.GetLogger(executionContext, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                log.LogMethodEntry(discountId);
                IDiscountUseCases discountUseCases = DiscountUseCaseFactory.GetDiscountUseCases(executionContext, RequestIdentifierHelper.GetRequestIdentifier(Request));
                await discountUseCases.ClearDiscountedGames(discountId);
                log.LogMethodExit();
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                });
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