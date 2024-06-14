/********************************************************************************************
 * Project Name - Transaction
 * Description  - Created to update NotificationTagIssued time Details
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By               Remarks          
 *********************************************************************************************
 *2.150.6     22-Dec-2023   Abhishek              Created - Game Server Cloud Movement.
 ********************************************************************************************/
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Transaction;

namespace Semnox.CommonAPI.Controllers.Tags
{
    public class NotificationTagIssuedTimeController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Post the JSON NotificationTagIssuedDTO
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>        
        [HttpPost]
        [Route("api/Transaction/{notificationTagIssuedId}/NotificationTagIssued")]
        [Authorize]
        public async Task<HttpResponseMessage> Post(int notificationTagIssuedId, [FromBody] NotificationTagIssuedDTO notificationTagIssuedDTO)
        {
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(notificationTagIssuedId, notificationTagIssuedDTO);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                if (notificationTagIssuedDTO != null || notificationTagIssuedId > -1)
                {
                    INotificationTagIssuedUseCases notificationTagIssuedUseCases = TransactionUseCaseFactory.GetNotificationTagIssuedUseCases(executionContext);
                    NotificationTagIssuedDTO response = await notificationTagIssuedUseCases.SaveNotificationTagIssuedTime(notificationTagIssuedId, notificationTagIssuedDTO);
                    log.LogMethodExit(response);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = string.Empty });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
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
    }
}
