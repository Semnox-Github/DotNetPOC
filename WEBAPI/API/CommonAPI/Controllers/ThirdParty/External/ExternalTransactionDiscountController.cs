/********************************************************************************************
 * Project Name - CommonAPI
 * Description  - ExternalDiscountController  API -  add and delete discount data in Parafait
 * 
 **************
 **Version Log
 **************
 *Version     Date                  Modified By           Remarks          
 *********************************************************************************************
 *0.0        28-Sept-2020           Girish Kundar          Created 
 *2.130.7    07-Apr-2022             M S Shreyas            Modified( External  REST API.)
 ********************************************************************************************/
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.CommonAPI.Helpers;
using Semnox.Parafait.ThirdParty.External;
using Semnox.Parafait.ThirdParty.CenterEdge;

namespace Semnox.CommonAPI.Controllers.ThirdParty.External
{
    public class ExternalTransactionDiscountController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Discount
        /// </summary>       
        /// <param name="externalAddDiscountDTO">externalAddDiscountDTO</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Route("api/External/Transaction/{transactionId}/Discount")]
        [Authorize]
        public HttpResponseMessage Get([FromUri] int transactionId,[FromBody] ExternalAddDiscountDTO externalAddDiscountDTO)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(transactionId);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                ExternalAddDiscountDTO externalAddDiscount = new ExternalAddDiscountDTO(externalAddDiscountDTO.DiscountName, externalAddDiscountDTO.Amount,
                      externalAddDiscountDTO.CouponNumber, externalAddDiscountDTO.Remarks);
                log.LogMethodExit(externalAddDiscount);
                return Request.CreateResponse(HttpStatusCode.OK, externalAddDiscount);
            }
            catch (ValidationException ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { code = ErrorCode.badRequest.ToString(), message = customException });
            }
        }

        /// <summary>
        /// Post the JSON Discount
        /// </summary>       
        /// <param name="externalAddDiscountDTO">externalAddDiscountDTO</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/External/Transaction/{transactionId}/Discount")]
        [Authorize]
        public HttpResponseMessage Post([FromUri] int transactionId, [FromBody] ExternalAddDiscountDTO externalAddDiscountDTO)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(transactionId, externalAddDiscountDTO);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                ExternalAddDiscountDTO externalAddDiscountsDTO = new ExternalAddDiscountDTO();
                if (externalAddDiscountDTO == null)
                {
                    string customException = "Discount data cannot be null.Please enter the Discount Details";
                    log.Error(customException);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
                }
                if (externalAddDiscountDTO != null && (string.IsNullOrEmpty(externalAddDiscountDTO.CouponNumber) || string.IsNullOrEmpty(externalAddDiscountDTO.DiscountName)))
                {
                    string customException = "Please enter coupon number or discount name";
                    log.Error(customException);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
                }
                // ExternalTransactionListBL externalTransactionBL = new ExternalTransactionListBL(executionContext);
                //externalTransactionBL.AddDiscount(transactionId, externalAddDiscountDTO);
                string message = "Discount Added Successfully";
                log.LogMethodExit(externalAddDiscountDTO);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = externalAddDiscountDTO });
            }
            catch (ValidationException valEx)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(valEx, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { code = ErrorCode.badRequest.ToString(), message = customException });
            }
        }

        /// <summary>
        /// Delete the JSON Discount
        /// </summary>       
        /// <param name="externalRemoveDiscountDTO">externalRemoveDiscountDTO</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpDelete]
        [Route("api/External/Transaction/{transactionId}/Discount")]
        [Authorize]
        /// <summary>
        /// Removes Discount
        /// </summary>
        public HttpResponseMessage Delete([FromUri] int transactionId, [FromBody]ExternalRemoveDiscountDTO externalRemoveDiscountDTO)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(transactionId, externalRemoveDiscountDTO);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                //ExternalTransactionListBL externalTransactionBL = new ExternalTransactionListBL(executionContext);
                //externalTransactionBL.RemoveDiscount(transactionId, externalRemoveDiscountDTO);
                string message = "Discount Added Successfully";
                log.LogMethodExit(message);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = externalRemoveDiscountDTO });
            }
            catch (ValidationException valEx)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(valEx, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
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
