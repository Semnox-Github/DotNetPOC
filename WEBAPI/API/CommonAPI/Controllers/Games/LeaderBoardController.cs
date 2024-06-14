/********************************************************************************************
 * Project Name - Common API                                                                    
 * Description  - API to GET LeaderBoard details
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
using Semnox.Parafait.Game;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.Transaction.VirtualArcade;

namespace Semnox.CommonAPI.Games.Controllers.Games
{
    public class LeaderBoardController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON LeaderBoardDTO
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Game/LeaderBoards")]
        public async Task<HttpResponseMessage> Get(int gameMachineLevelId = -1)
        {
            log.LogMethodEntry(gameMachineLevelId);
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (gameMachineLevelId < 0)
                {
                    string customException = "Please enter valid game machine level id";
                    log.Error(customException);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
                }
                ICustomerGamePlayLevelResultUseCases customerGamePlayLevelResultUseCase = TransactionUseCaseFactory.GetCustomerGamePlayLevelResultUseCases(executionContext);
                List<LeaderBoardDTO> leaderBoardDTOList = await customerGamePlayLevelResultUseCase.GetLeaderBoard(gameMachineLevelId);
                log.LogMethodExit(leaderBoardDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = leaderBoardDTOList });
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
