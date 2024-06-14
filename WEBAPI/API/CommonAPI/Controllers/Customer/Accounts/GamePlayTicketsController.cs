/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - API for the Game Play Tickets
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.150.2     29-Nov-2022   Mathew Ninan   Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Semnox.Parafait.Game;
using System.Web.Http;
using System.Threading.Tasks;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Linq;
using System.Text;
using System.IO;
using Semnox.Parafait.Languages;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.ServerCore;
using Semnox.CommonAPI.Helpers;

namespace Semnox.CommonAPI.Controllers.Transaction.V2
{
    public class GamePlayTicketsController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        /// <summary>
        /// Get the JSON Object Game Plays List
        /// </summary>        
        /// <returns>HttpMessgae</returns>
        [HttpPost]
        [Route("api/Account/{AccountId}/{MachineId}/GameplayTickets")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromUri] int accountId, [FromUri] int machineId, [FromBody] int ticketCount)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(accountId, machineId, ticketCount);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                IGameTransactionUseCases gameTransactionUseCases = GameTransactionUseCaseFactory.GetGameTransactionUseCases(executionContext);
                var content = await gameTransactionUseCases.GameplayTickets(accountId, machineId, ticketCount);
                log.LogMethodExit(content);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content });
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
