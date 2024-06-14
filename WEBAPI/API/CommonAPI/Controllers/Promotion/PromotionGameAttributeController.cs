/********************************************************************************************
 * Project Name - Promotions
 * Description  - Controller for the Promotions class.
 *  
 **************
 **Version Log
 **************
 *Version     Date            Modified By        Remarks          
 *********************************************************************************************
 *2.130.8     12-Apr-2022      Abhishek          Created : Promotion Game Attribute Enhancement
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Game;
using Semnox.Parafait.Promotions;

namespace Semnox.CommonAPI.Promotion
{
    public class PromotionGameAttributeController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object of Promotion Game Attribute Details
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Promotion/Promotions/{promotionId}/GameAttributes")]
        public HttpResponseMessage Get([FromUri]int promotionId)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(promotionId);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                PromotionListBL promotionListBL = new PromotionListBL(executionContext);
                List<MachineAttributeDTO> machineAttributeDTOList = promotionListBL.GetPromotionGameAttributes(promotionId);
                string configHardWareVersion = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "READER_HARDWARE_VERSION");
                log.LogMethodExit(machineAttributeDTOList, configHardWareVersion);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = machineAttributeDTOList, hardWareVersion = configHardWareVersion });
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
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        /// <summary>
        /// Performs a Post operation on Promotion Game Attribute Details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/Promotion/Promotions/{promotionId}/GameAttributes")]
        [Authorize]
        public HttpResponseMessage Post([FromUri] int promotionId, [FromBody] List<MachineAttributeDTO> machineAttributeDTOList)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(machineAttributeDTOList);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);

                if (machineAttributeDTOList != null && machineAttributeDTOList.Any())
                {
                    PromotionListBL promotionListBL = new PromotionListBL(executionContext);
                    promotionListBL.SaveAndUpdateGameAttribute(machineAttributeDTOList, promotionId);
                    log.LogMethodExit(machineAttributeDTOList);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = machineAttributeDTOList });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
            }
            catch (UnauthorizedException ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
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
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        /// <summary>
        /// Delete the Reader Configuration record 
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpDelete]
        [Route("api/Promotion/Promotions/{promotionId}/GameAttributes")]
        [Authorize]
        public HttpResponseMessage Delete([FromUri] int promotionId, MachineAttributeDTO machineAttributeDTO)
        {
            log.LogMethodEntry(promotionId, machineAttributeDTO);
            ExecutionContext executionContext = null;
            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                if (machineAttributeDTO != null && promotionId > 0)
                {
                    PromotionListBL promotionListBL = new PromotionListBL(executionContext);
                    if (machineAttributeDTO.AttributeId > 0 && machineAttributeDTO.ContextOfAttribute == MachineAttributeDTO.AttributeContext.PROMOTION && machineAttributeDTO.EnableForPromotion)
                    {
                        promotionListBL.DeleteMachineAttribute(machineAttributeDTO.AttributeId, promotionId);
                    }
                    log.LogMethodExit(machineAttributeDTO);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = machineAttributeDTO });
                }
                else
                {
                    log.LogMethodExit();
                    string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(new InvalidOperationException("Invalid Input"), executionContext);
                    log.Error(customException);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
                }
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }
    }
}
