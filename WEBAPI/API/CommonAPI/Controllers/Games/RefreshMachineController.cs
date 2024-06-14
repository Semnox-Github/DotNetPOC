/********************************************************************************************
 * Project Name - Promotions
 * Description  - Controller to get the Game Promotions.
 *  
 **************
 **Version Log
 **************
 *Version     Date            Modified By        Remarks          
 *********************************************************************************************
 *2.150.2     18-Jan-2022      Abhishek          Created - Game Server Cloud Movement.
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
    public class RefreshMachineController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object Machine Details List
        /// </summary>
        /// <param name="machineId">Machine Id</param>
        /// <param name="isPromotionActive">Is Promotion Active</param>
        /// <returns>HttpResponseMessage</returns>
        [Route("api/Game/Machine/{machineId}/Refresh")]
        [Authorize]
        public async Task<HttpResponseMessage> Get([FromUri]int machineId = -1, string isPromotionActive = "N")
        {

            log.LogMethodEntry(machineId);
            ExecutionContext executionContext = null;
            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                IGameTransactionUseCases gameTransactionUseCases = GameTransactionUseCaseFactory.GetGameTransactionUseCases(executionContext);
                var content = await gameTransactionUseCases.RefreshMachine(machineId, isPromotionActive);
                log.LogMethodExit(content);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content });
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