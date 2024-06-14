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
    public class DiscountsController : ApiController
    {
        private Semnox.Parafait.logging.Logger log;

        /// <summary>
        /// Get the JSON Discounts Setup.
        /// </summary>
        /// <param name="discountType">discountType i.e., T for Transaction G for Game Play and L for Loyalty</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Discount/Discounts")]
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
                                                   int siteId = -1,
                                                   int pageNumber = 0,
                                                   int pageSize = 0,
                                                   bool loadChildRecords = true,
                                                   bool loadActiveChildRecords = true,
                                                   bool onlyDiscountedChildRecords = false)
        {
            ExecutionContext executionContext = null;

            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                log = LogManager.GetLogger(executionContext, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                log.LogMethodEntry(isActive, discountId, discountName, discountType, displayInPOS, minimumCreditsGreaterThan,
                                   minimumSaleAmountGreaterThan, automaticApply, couponMandatory, discountedGameId, discountedProductId,
                                   discountedCategoryId, siteId, pageNumber, pageSize, loadChildRecords, loadActiveChildRecords,
                                   onlyDiscountedChildRecords);


                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                IDiscountUseCases discountUseCases = DiscountUseCaseFactory.GetDiscountUseCases(executionContext,
                    RequestIdentifierHelper.GetRequestIdentifier(Request));
                List<DiscountsDTO> discountsDTOList = await discountUseCases.GetDiscountsDTOList(isActive, discountId, discountName, discountType, displayInPOS, minimumCreditsGreaterThan,
                                   minimumSaleAmountGreaterThan, automaticApply, couponMandatory, discountedGameId, discountedProductId,
                                   discountedCategoryId, siteId, pageNumber, pageSize, loadChildRecords, loadActiveChildRecords,
                                   onlyDiscountedChildRecords);
                log.LogMethodExit(discountsDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    data = discountsDTOList,
                });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }

        /// <summary>
        /// Post the JSON Object Discounts Setup
        /// </summary>
        /// <param name="discountDTOList">discountDTOList</param>
        /// <returns>HttpMessgae</returns>
        [HttpPost]
        [Route("api/Discount/Discounts")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody]List<DiscountsDTO> discountDTOList)
        {
            ExecutionContext executionContext = null;
            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                log = LogManager.GetLogger(executionContext, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                log.LogMethodEntry(discountDTOList);
                IDiscountUseCases discountUseCases = DiscountUseCaseFactory.GetDiscountUseCases(executionContext, RequestIdentifierHelper.GetRequestIdentifier(Request));
                List<DiscountsDTO> savedDiscountsDTOList = await discountUseCases.SaveDiscountsDTOList(discountDTOList);
                log.LogMethodExit(savedDiscountsDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    data = savedDiscountsDTOList,
                });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }

        /// <summary>
        /// Deletes the JSON Object Discounts Setup
        /// </summary>
        /// <param name="discountDTOList">discountDTOList</param>
        /// <returns>HttpMessgae</returns>
        [HttpDelete]
        [Route("api/Discount/Discounts")]
        [Authorize]
        public async Task<HttpResponseMessage> Delete([FromBody]List<DiscountsDTO> discountDTOList)
        {
            ExecutionContext executionContext = null;
            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                log = LogManager.GetLogger(executionContext, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                log.LogMethodEntry(discountDTOList);
                IDiscountUseCases discountUseCases = DiscountUseCaseFactory.GetDiscountUseCases(executionContext, RequestIdentifierHelper.GetRequestIdentifier(Request));
                List<DiscountsDTO> savedDiscountsDTOList = await discountUseCases.SaveDiscountsDTOList(discountDTOList);
                log.LogMethodExit(savedDiscountsDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    data = savedDiscountsDTOList,
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