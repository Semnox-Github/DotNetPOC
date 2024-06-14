/********************************************************************************************
 * Project Name - Common API                                                                    
 * Description  - API to GET GamePlayWinning details
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 ********************************************************************************************* 
  *2.110.0     01-Feb-2021   Girish Kundar    Created : Virtual Arcade changes
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.Transaction.VirtualArcade;

namespace Semnox.CommonAPI.Games.Controllers.Games
{
    public class GamePlayWinningController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON LeaderBoardDTO
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Game/GamePlayWinnings")]
        public async Task<HttpResponseMessage> Get(int customerId = -1)
        {

            log.LogMethodEntry(customerId);
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (customerId < 0)
                {
                    string customException = "Please enter valid customer Id";
                    log.Error(customException);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
                }
                ICustomerGamePlayLevelResultUseCases customerGamePlayLevelResultUseCase = TransactionUseCaseFactory.GetCustomerGamePlayLevelResultUseCases(executionContext);
                List<GamePlayWinningsDTO> gamePlayWinningsDTOList = await customerGamePlayLevelResultUseCase.GetCustomerGamePlayWinnings(customerId);
                log.LogMethodExit(gamePlayWinningsDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = gamePlayWinningsDTOList });
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
