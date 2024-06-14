/********************************************************************************************
 * Project Name - Transaction
 * Description  - API for the Staff Card details
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By               Remarks          
 *********************************************************************************************
 *2.120.1    26-May-2021    Abhishek                   Created
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Semnox.Core.Utilities;
using System.Web;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Product;
using Semnox.Parafait.Transaction;
using Semnox.CommonAPI.Helpers;
using System.Threading.Tasks;
using Semnox.Parafait.User;

namespace Semnox.CommonAPI.Transaction
{
    public class StaffCardController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Performs a Post operation on StaffCardDTOList details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/Transaction/StaffCards")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody] List<StaffCardDTO> staffCardDTOList)
        {
            log.LogMethodEntry(staffCardDTOList);
            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                ITransactionUseCases transactionUseCases = TransactionUseCaseFactory.GetTransactionUseCases(executionContext);
                var result = await transactionUseCases.CreateStaffCard(staffCardDTOList);
                log.LogMethodExit(result);
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    data = result,
                });
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
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }

        [HttpDelete]
        [Route("api/Transaction/StaffCards")]
        [Authorize]
        public async Task<HttpResponseMessage> Delete([FromBody] List<StaffCardDTO> staffCardDTOs)
        {
            log.LogMethodEntry(staffCardDTOs);
            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                ITransactionUseCases transactionUseCases = TransactionUseCaseFactory.GetTransactionUseCases(executionContext);
                var result = await transactionUseCases.DeactivateStaffCard(staffCardDTOs);
                log.LogMethodExit(result);
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    data = result,
                });
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
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }

        //[HttpPost]
        //[Route("api/Transaction/StaffCards")]
        //[Authorize]
        //public async Task<HttpResponseMessage> Post([FromBody] List<TransactionDTO> transactionDTOList)
        //{
        //    log.LogMethodEntry(transactionDTOList);
        //    try
        //    {
        //        executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
        //        ITransactionUseCases transactionUseCases = TransactionUseCaseFactory.GetStaffCardsUseCases(executionContext);
        //        var result = await transactionUseCases.SaveStaffCards(transactionDTOList);
        //        log.LogMethodExit(result);
        //        return Request.CreateResponse(HttpStatusCode.OK, new
        //        {
        //            data = result,
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
        //        log.Error(customException);
        //        return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
        //    }
        //}
    }
}