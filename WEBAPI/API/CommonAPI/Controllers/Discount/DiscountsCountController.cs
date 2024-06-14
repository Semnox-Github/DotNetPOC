/******************************************************************************************************
 * Project Name - Products
 * Description  - Created to fetch, update and inserts for Discounts Setup Entity.
 *  
 **************
 **Version Log
 **************
 *Version   Date            Modified By               Remarks          
 ******************************************************************************************************
 *2.60      25-Jan-2019     Akshay Gulaganji          Created to Get, POST and Delete Methods.
 ******************************************************************************************************
 *2.60      08-Mar-2019     Akshay Gulaganji          Added loadChildRecords and activeChildRecords
 *2.90      08-May-2020     Girish Kundar             Modified : Moved to Discount folder and REST API standard changes
 *2.90      19-jun-2020     Mushahid Faizan           Modified : return status Code in the catch block. and passed executionContect site Id in search parameters.
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
    public class DiscountsCountController : ApiController
    {
        private Semnox.Parafait.logging.Logger log;

        /// <summary>
        /// Get the JSON Discounts Setup.
        /// </summary>
        /// <param name="discountType">discountType i.e., T for Transaction G for Game Play and L for Loyalty</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Discount/DiscountsCount")]
        public async Task<HttpResponseMessage> Get(string isActive = null,
                                                   int discountId = -1,
                                                   string discountName = null,
                                                   string discountType = null,
                                                   string displayInPOS = null,
                                                   decimal? minimumCreditsGreaterThan = null,
                                                   decimal? minimumSaleAmountGreaterThan = null,
                                                   string automaticApply = null,
                                                   string couponMandatory = null,
                                                   int discountedGameId = -1,
                                                   int discountedProductId = -1,
                                                   int discountedCategoryId = -1,
                                                   int siteId = -1)
        {
            ExecutionContext executionContext = null;

            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                log = LogManager.GetLogger(executionContext, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                log.LogMethodEntry(isActive, discountId, discountName, discountType, displayInPOS, minimumCreditsGreaterThan,
                                   minimumSaleAmountGreaterThan, automaticApply, couponMandatory, discountedGameId, discountedProductId,
                                   discountedCategoryId, siteId);


                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                IDiscountUseCases discountUseCases = DiscountUseCaseFactory.GetDiscountUseCases(executionContext,
                    RequestIdentifierHelper.GetRequestIdentifier(Request));
                int result = await discountUseCases.GetDiscountsDTOListCount(isActive, discountId, discountName, discountType, displayInPOS, minimumCreditsGreaterThan,
                                   minimumSaleAmountGreaterThan, automaticApply, couponMandatory, discountedGameId, discountedProductId,
                                   discountedCategoryId, siteId);
                log.LogMethodExit(result);
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    data = result,
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