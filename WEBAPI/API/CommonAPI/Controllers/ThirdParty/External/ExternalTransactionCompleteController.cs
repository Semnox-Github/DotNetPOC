﻿/********************************************************************************************
 * Project Name - CommonAPI
 * Description  - ExternalTransactionCompleteController  API -  Complete the transaction
 * 
 **************
 **Version Log
 **************
 *Version     Date                  Modified By           Remarks          
 *********************************************************************************************
 *2.130.7    22-Jul-2022            Abhishek             Created - External  REST API
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
    public class ExternalTransactionCompleteController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Post the JSON Transaction
        /// </summary>       
        /// <param name="transactionId">transactionId</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/External/Transaction/{transactionId}/Complete")]
        [Authorize]
        public HttpResponseMessage Post([FromUri] int transactionId)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(transactionId);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                ExternalAddDiscountDTO externalAddDiscount = new ExternalAddDiscountDTO();
                ExternalTransactionListBL externalTransactionBL =  new ExternalTransactionListBL(executionContext);
                externalTransactionBL.CompleteTransaction(transactionId);
                string message = "Transaction Completed Successfully";
                log.LogMethodExit(message);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = message });
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
    }
}