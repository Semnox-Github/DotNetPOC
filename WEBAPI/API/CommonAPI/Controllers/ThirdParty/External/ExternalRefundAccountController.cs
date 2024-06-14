﻿/********************************************************************************************
 * Project Name - CommonAPI
 * Description  - Created to refond the card.
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.150.0    18-Sep-2023   Abhishek                 Created - External  REST API
 ***************************************************************************************************/
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Transaction;

namespace Semnox.CommonAPI.Controllers.ThirdParty.External
{
    public class ExternalRefundAccountController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Post the JSON Object RefundAccount
        /// </summary>
        /// <param name="refundCardDTO">refundCardDTO</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/External/RefundAccount")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody] RefundCardDTO refundCardDTO)
        {
            log.LogMethodEntry(refundCardDTO);
            ExecutionContext executionContext = null;
            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                ITaskUseCases taskUseCases = TaskUseCaseFactory.GetTaskUseCases(executionContext);
                RefundCardDTO result = await taskUseCases.RefundCard(refundCardDTO);
                log.LogMethodExit(result);
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    data = result,
                });
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
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }
    }
}