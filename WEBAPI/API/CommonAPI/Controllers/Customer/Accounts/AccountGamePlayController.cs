/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - API for the Account Game Plays
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.150.2   12-Dec-2022    Mathew Ninan   Created 
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Game;
using Semnox.Parafait.Customer.Accounts;
using Semnox.CommonAPI.Helpers;
using Semnox.Parafait.ServerCore;

namespace Semnox.CommonAPI.Controllers.Customer.Accounts
{
    public class AccountGamePlayController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// POST Game play
        /// </summary>        
        /// <returns>HttpMessage</returns>
        [HttpPost]
        [Route("api/Customer/Account/{accountId}/AccountGamePlay")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromUri] int accountId, [FromBody] List<GamePlayBuildDTO> gamePlayBuildDTOList)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(accountId, gamePlayBuildDTOList);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                IGameTransactionUseCases gameTransactionUseCases = GameTransactionUseCaseFactory.GetGameTransactionUseCases(executionContext);
                var content = await gameTransactionUseCases.AccountGamePlay(accountId, gamePlayBuildDTOList);
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
