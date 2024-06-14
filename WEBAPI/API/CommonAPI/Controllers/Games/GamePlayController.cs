/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - API for the Machines Game Plays
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.40        12-Sept-2018   Jagan          Created 
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Semnox.Parafait.Game;
using System.Web.Http;
using Semnox.Core.Utilities;
using System.Web;
using Semnox.Core.GenericUtilities;
using System.Data.SqlClient;
using System.Configuration;
using Semnox.Parafait.CardCore;
using System.Linq;
using Semnox.Parafait.User;

namespace Semnox.CommonAPI.Games
{
    public class GamePlayController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly SecurityTokenBL securityTokenBL = new SecurityTokenBL();
        private SecurityTokenDTO securityTokenDTO = null;
        private ExecutionContext executionContext;
        /// <summary>
        /// Get the JSON Object Game Plays List
        /// </summary>        
        /// <returns>HttpMessgae</returns>
        [Route("api/Games/GamePlay/")]
        [Authorize]
        public HttpResponseMessage Get(string machineId = "", int gamePlayId = -1, int accountId = -1)
        {
            try
            {
                log.LogMethodEntry(machineId);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<GamePlayDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<GamePlayDTO.SearchByParameters, string>>();
                if (!String.IsNullOrEmpty(machineId))
                    searchParameters.Add(new KeyValuePair<GamePlayDTO.SearchByParameters, string>(GamePlayDTO.SearchByParameters.MACHINE_ID, machineId));
                if (gamePlayId > -1)
                    searchParameters.Add(new KeyValuePair<GamePlayDTO.SearchByParameters, string>(GamePlayDTO.SearchByParameters.GAME_PLAY_ID, gamePlayId.ToString()));
                if (accountId > -1)
                    searchParameters.Add(new KeyValuePair<GamePlayDTO.SearchByParameters, string>(GamePlayDTO.SearchByParameters.CARD_ID, accountId.ToString()));

                GamePlayListBL gamePlayBL = new GamePlayListBL(executionContext);
                var content = gamePlayBL.GetGamePlayDTOListWithTagNumber(searchParameters, null);
                log.LogMethodExit(content);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content, token = securityTokenDTO.Token });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, token = securityTokenDTO.Token });
            }
        }

        /// <summary>
        /// Post the JSON Object Game Play
        /// </summary>
        /// <param name="machinesDTOList">machineDtoList</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/Games/GamePlay/")]
        [Authorize]
        public HttpResponseMessage Post([FromBody]List<GamePlayDTO> gamePlayDTOList)
        {
            log.LogMethodEntry(gamePlayDTOList);
            securityTokenBL.GenerateJWTToken();
            securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
            executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

            if (gamePlayDTOList == null || !gamePlayDTOList.Any())
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(new InvalidOperationException("Invalid Input"), executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, token = securityTokenDTO.Token });
            }

            List<GamePlayDTO> createdGamePlayDTOs = new List<GamePlayDTO>();
            try
            {
                GamePlayDTO gamePlayDTO = gamePlayDTOList[0];
                int noOfGamePlays = gamePlayDTOList.Count;

                Semnox.Parafait.Game.Machine machine = new Semnox.Parafait.Game.Machine(gamePlayDTO.MachineId);
                if (machine.GetMachineDTO == null || machine.GetMachineDTO.MachineId < 0)
                {
                    string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(new InvalidOperationException("Machine is Invalid"), executionContext);
                    log.Error(customException);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, token = securityTokenDTO.Token });
                }
                CardCoreBL cardCoreBL = new CardCoreBL(gamePlayDTO.CardNumber);
                CardCoreDTO cardCoreDTO = cardCoreBL.GetCardCoreDTO;
                if (cardCoreDTO == null || cardCoreDTO.CardId < 0)
                {
                    string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(new InvalidOperationException("Card Number is Invalid"), executionContext);
                    log.Error(customException);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, token = securityTokenDTO.Token });
                }
                Utilities utilities;
                string connstring = "";
                connstring = ConfigurationManager.ConnectionStrings["ParafaitConnectionString"].ConnectionString;
                using (utilities = new Utilities(connstring))
                {
                    utilities.ParafaitEnv.SiteId = executionContext.GetSiteId();
                    utilities.ParafaitEnv.IsCorporate = executionContext.GetIsCorporate();
                    utilities.ExecutionContext.SetIsCorporate(executionContext.GetIsCorporate());
                    utilities.ExecutionContext.SetSiteId(executionContext.GetSiteId());
                    utilities.ExecutionContext.SetUserId(executionContext.GetUserId());
                    utilities.ParafaitEnv.Initialize();

                    Semnox.Parafait.ServerCore.ServerStatic serverStatic = new Semnox.Parafait.ServerCore.ServerStatic(utilities);
                    Semnox.Parafait.ServerCore.Machine servercoreMachine = new Semnox.Parafait.ServerCore.Machine(machine.GetMachineDTO.MachineId, serverStatic);
                    SqlConnection conn = utilities.getConnection();
                    using (SqlTransaction trx = conn.BeginTransaction())
                    {
                        try
                        {
                            for (int i = 0; i < noOfGamePlays; i++)
                            {
                                createdGamePlayDTOs.Add(servercoreMachine.PlayGame(machine.GetMachineDTO, cardCoreDTO, trx));
                            }
                            trx.Commit();
                        }
                        catch (Exception ex)
                        {
                            trx.Rollback();
                            string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(new InvalidOperationException(ex.Message), executionContext);
                            log.Error(customException);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, token = securityTokenDTO.Token });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, token = securityTokenDTO.Token });
            }
            log.LogMethodExit(createdGamePlayDTOs);
            return Request.CreateResponse(HttpStatusCode.OK, new { data = createdGamePlayDTOs, token = securityTokenDTO.Token });
        }
    }
}
