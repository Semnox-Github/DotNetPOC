/********************************************************************************************
 * Project Name - CommonAPI
 * Description  - ExternalTransactionCreditsController  API -  add and delete credits to the card in Parafait
 * 
 **************
 **Version Log
 **************
 *Version     Date                  Modified By           Remarks          
 *********************************************************************************************
 *0.0        28-Sept-2020           Girish Kundar          Created 
 *2.130.7    07-Apr-2022            Ashish Bhat            Modified( External  REST API.)
 ********************************************************************************************/
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.ThirdParty.External;

namespace Semnox.CommonAPI.Controllers.ThirdParty.External
{
    public class ExternalTransactionCreditsController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Post the JSON Credits
        /// </summary>       
        /// <param name="externalAddCreditsDTO">externalAddCreditsDTO</param>
        /// <returns>HttpResponseMessage</returns>        
        [HttpPost]
        [Route("api/External/Account/{accountId}/Credits")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromUri] int accountId, [FromBody]ExternalAddCreditsDTO externalAddCreditsDTO)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(accountId, externalAddCreditsDTO);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                if (externalAddCreditsDTO == null)
                {
                    string customException = "Credits data cannot be null.Please enter the Credits Details";
                    log.Error(customException);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
                }
                ExternalTransactionListBL externalTransactionBL = new ExternalTransactionListBL(executionContext);
                externalTransactionBL.AddValue(accountId, externalAddCreditsDTO);
                string message = "Credits Added Successfully";
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
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }

        /// <summary>
        /// Delete the JSON Credits
        /// </summary>     
        /// <param name="externalRemoveCreditsDTO">externalRemoveCreditsDTO</param>
        /// <returns>HttpResponseMessage</returns>    
        [HttpDelete]
        [Route("api/External/Account/{accountId}/Credits")]
        [Authorize]
        public async Task<HttpResponseMessage> Delete([FromUri] int accountId, [FromBody]ExternalRemoveCreditsDTO externalRemoveCreditsDTO)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(accountId, externalRemoveCreditsDTO);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                if (externalRemoveCreditsDTO == null)
                {
                    string customException = "Credits data cannot be null.Please enter the Credits Details";
                    log.Error(customException);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
                }
                ExternalTransactionListBL externalTransactionBL = new ExternalTransactionListBL(executionContext);
                externalTransactionBL.RemoveValue(accountId, externalRemoveCreditsDTO);
                log.LogMethodExit();
                string message = "Credits Removed Successfully";
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
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }
    }
}
