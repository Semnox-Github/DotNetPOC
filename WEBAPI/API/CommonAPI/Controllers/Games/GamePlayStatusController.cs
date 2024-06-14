/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - API for the GamePlayStatusController
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.150.2     02-Mar-2022   Abhishek    Created - Game Server Cloud Movement.
 ********************************************************************************************/
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.ServerCore;
namespace Semnox.CommonAPI.Games
{
    public class GamePlayStatusController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Post the Machines Details
        /// </summary>
        /// <param name="machinesDTOList">machineDtoList</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/Game/GamePlay/{machineId}/Status")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromUri]int machineId,[FromBody] bool isGameSuccess = false)
        {

            log.LogMethodEntry(isGameSuccess);
            ExecutionContext executionContext = null;
            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                if (machineId > -1)
                {
                    IGameTransactionUseCases gameTransactionUseCases = GameTransactionUseCaseFactory.GetGameTransactionUseCases(executionContext);
                    await gameTransactionUseCases.UpdateGamePlayStatus(machineId, isGameSuccess);
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "" });
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