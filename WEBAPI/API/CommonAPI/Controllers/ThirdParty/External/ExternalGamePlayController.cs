/********************************************************************************************
 * Project Name - CommonAPI
 * Description  - Created to save Game Play.
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.130.7    11-Apr-2022   Abhishek                 Created - External  REST API
 ***************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.Game;
using Semnox.Parafait.ServerCore;
using Semnox.Parafait.ThirdParty.External;

namespace Semnox.CommonAPI.Controllers.ThirdParty.External
{
    public class ExternalGamePlayController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object Game Plays List
        /// </summary>        
        /// <returns>HttpMessgae</returns>
        [Route("api/External/Game/GamePlays")]
        [Authorize]
        public HttpResponseMessage Get(string machineId = "", int gamePlayId = -1, int accountId = -1)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(machineId);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);

                List<KeyValuePair<GamePlayDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<GamePlayDTO.SearchByParameters, string>>();
                if (!String.IsNullOrEmpty(machineId))
                    searchParameters.Add(new KeyValuePair<GamePlayDTO.SearchByParameters, string>(GamePlayDTO.SearchByParameters.MACHINE_ID, machineId));
                if (gamePlayId > -1)
                    searchParameters.Add(new KeyValuePair<GamePlayDTO.SearchByParameters, string>(GamePlayDTO.SearchByParameters.GAME_PLAY_ID, gamePlayId.ToString()));
                if (accountId > -1)
                    searchParameters.Add(new KeyValuePair<GamePlayDTO.SearchByParameters, string>(GamePlayDTO.SearchByParameters.CARD_ID, accountId.ToString()));

                GamePlayListBL gamePlayBL = new GamePlayListBL(executionContext);
                List<GamePlayDTO> gamePlayDTOList = gamePlayBL.GetGamePlayDTOListWithTagNumber(searchParameters, null);
                List<ExternalGamePlaysDTO> externalGamePlaysDTOList = new List<ExternalGamePlaysDTO>();
                foreach (GamePlayDTO gamePlayDTO in gamePlayDTOList)
                {
                    ExternalGamePlaysDTO externalGamePlaysDTO = new ExternalGamePlaysDTO(gamePlayDTO.GameplayId, gamePlayDTO.MachineId,
                                    gamePlayDTO.CardNumber, gamePlayDTO.PlayDate, true);
                    externalGamePlaysDTOList.Add(externalGamePlaysDTO);
                }
                log.LogMethodExit(externalGamePlaysDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = externalGamePlaysDTOList });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
            }
        }

        /// <summary>
        /// Post the JSON Object Game Play
        /// </summary>
        /// <param name="externalGamePlayRequestDTOList">externalGamePlayDTOList</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/External/Game/GamePlays")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody]List<ExternalGamePlayRequest> externalGamePlayRequestDTOList)
        {
            log.LogMethodEntry(externalGamePlayRequestDTOList);
            ExecutionContext executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
            List<GamePlayDTO> gamePlayDTOList = new List<GamePlayDTO>();
            GamePlayDTO gamePlayDTO = new GamePlayDTO();
            List<ExternalGamePlaysDTO> externalGamePlayDTOList = new List<ExternalGamePlaysDTO>();
            if (externalGamePlayRequestDTOList == null || !externalGamePlayRequestDTOList.Any())
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(new InvalidOperationException("Invalid Input"), executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
            }
            foreach (ExternalGamePlayRequest externalGamePlayRequest in externalGamePlayRequestDTOList)
            {
                if (externalGamePlayRequest.MachineId == -1 && string.IsNullOrEmpty(externalGamePlayRequest.MachineReference) || string.IsNullOrEmpty(externalGamePlayRequest.CardNumber))
                {
                    string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(new InvalidOperationException("Invalid Input"), executionContext);
                    log.Error(customException);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
                }
                gamePlayDTO.ExternalSystemReference = externalGamePlayRequest.MachineReference;
                gamePlayDTO.MachineId = externalGamePlayRequest.MachineId;
                gamePlayDTO.CardNumber = externalGamePlayRequest.CardNumber;
                gamePlayDTO.PlayDate = externalGamePlayRequest.PlayDate;
                gamePlayDTOList.Add(gamePlayDTO);
            }
            gamePlayDTO = gamePlayDTOList[0];
            int noOfGamePlays = gamePlayDTOList.Count;

            Semnox.Parafait.Game.Machine machine = null;
            GamePlayDTO createdGamePlayDTOs = new GamePlayDTO();
            if (gamePlayDTO.MachineId > -1)
            {
                machine = new Semnox.Parafait.Game.Machine(gamePlayDTO.MachineId);
            }
            else if (string.IsNullOrWhiteSpace(gamePlayDTO.ExternalSystemReference) == false)
            {
                List<KeyValuePair<MachineDTO.SearchByMachineParameters, string>> searchParameters = new List<KeyValuePair<MachineDTO.SearchByMachineParameters, string>>();
                searchParameters.Add(new KeyValuePair<MachineDTO.SearchByMachineParameters, string>(MachineDTO.SearchByMachineParameters.SITE_ID, executionContext.SiteId.ToString()));
                searchParameters.Add(new KeyValuePair<MachineDTO.SearchByMachineParameters, string>(MachineDTO.SearchByMachineParameters.EXTERNAL_MACHINE_REFERENCE, gamePlayDTO.ExternalSystemReference));
                IMachineUseCases machineDataService = GameUseCaseFactory.GetMachineUseCases(executionContext);
                var content = await machineDataService.GetMachines(searchParameters, false, 0, 1);
                if (content != null && content.Any())
                {
                    machine = new Parafait.Game.Machine(content.FirstOrDefault(), false);
                }
            }

            if (machine == null || machine.GetMachineDTO == null || machine.GetMachineDTO.MachineId < 0)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(new InvalidOperationException("Machine is Invalid"), executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
            }
            AccountBL accountBL = new AccountBL(executionContext, externalGamePlayRequestDTOList[0].CardNumber);
            if (accountBL.AccountDTO == null)
            {
                log.Error("Cards not found.");
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "Cards not found." });
            }
            gamePlayDTO = gamePlayDTOList[0];
            gamePlayDTO.CardId = accountBL.AccountDTO.AccountId;
            gamePlayDTO.MachineId = machine.GetMachineDTO.MachineId;

            List<GamePlayBuildDTO> gamePlayBuildDTOList = new List<GamePlayBuildDTO>();
            GamePlayBuildDTO gamePlayBuildDTO = new GamePlayBuildDTO("R", gamePlayDTO, new GameServerEnvironment.GameServerPlayDTO(machine.GetMachineDTO.MachineId),
                string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, -1, externalGamePlayRequestDTOList[0].CommitGamePlay);
            gamePlayBuildDTOList.Add(gamePlayBuildDTO);
            try
            {
                IGameTransactionUseCases gameTransactionUseCases = GameTransactionUseCaseFactory.GetGameTransactionUseCases(executionContext);
                List<GamePlayDTO> gamePlayList = await gameTransactionUseCases.AccountGamePlay(accountBL.AccountDTO.AccountId, gamePlayBuildDTOList);
                ExternalGamePlaysDTO externalGamePlays = null;
                if (gamePlayList != null && gamePlayList[0].GameplayId > -1)
                {
                    externalGamePlays = new ExternalGamePlaysDTO(gamePlayList[0].GameplayId, gamePlayList[0].MachineId,
                    externalGamePlayRequestDTOList[0].CardNumber, gamePlayList[0].PlayDate, externalGamePlayRequestDTOList[0].CommitGamePlay);
                    externalGamePlayDTOList.Add(externalGamePlays);
                }
                else
                {
                    log.LogMethodExit("GamePlay can be performed.CommitGameplay is false");
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "GamePlay can be performed.CommitGameplay is false" });
                }
                //Utilities utilities;
                //string connstring = "";
                //connstring = ConfigurationManager.ConnectionStrings["ParafaitConnectionString"].ConnectionString;
                //using (utilities = new Utilities(connstring))
                //{
                //    utilities.ParafaitEnv.SiteId = executionContext.GetSiteId();
                //    utilities.ParafaitEnv.IsCorporate = executionContext.GetIsCorporate();
                //    utilities.ExecutionContext.SetIsCorporate(executionContext.GetIsCorporate());
                //    utilities.ExecutionContext.SetSiteId(executionContext.GetSiteId());
                //    utilities.ExecutionContext.SetUserId(executionContext.GetUserId());
                //    utilities.ParafaitEnv.Initialize();

                //    Semnox.Parafait.ServerCore.GameServerEnvironment gameServerEnvironment = new Semnox.Parafait.ServerCore.GameServerEnvironment(utilities.ExecutionContext);
                //    Semnox.Parafait.ServerCore.Machine servercoreMachine = new Semnox.Parafait.ServerCore.Machine(machine.GetMachineDTO.MachineId, gameServerEnvironment);

                //    SqlConnection conn = utilities.getConnection();
                //    using (SqlTransaction trx = conn.BeginTransaction())
                //    {
                //        try
                //        {

                //            for (int i = 0; i < noOfGamePlays; i++)
                //            {
                //                createdGamePlayDTOs = servercoreMachine.PlayGame(machine.GetMachineDTO.MachineId, gamePlayDTO.CardNumber, trx);
                //                ExternalGamePlaysDTO externalGamePlays = new ExternalGamePlaysDTO(createdGamePlayDTOs.GameplayId, createdGamePlayDTOs.MachineId,
                //                    gamePlayDTO.CardNumber, createdGamePlayDTOs.PlayDate);
                //                //externalGamePlays.GameplayId = createdGamePlayDTOs.GameplayId;
                //                //externalGamePlays.MachineId = createdGamePlayDTOs.MachineId;
                //                //externalGamePlays.CardNumber = gamePlayDTO.CardNumber;
                //                //externalGamePlays.PlayDate = createdGamePlayDTOs.PlayDate;
                //                externalGamePlayDTOList.Add(externalGamePlays);

                //            }
                //            trx.Commit();
                //        }
                //        catch (Exception ex)
                //        {
                //            trx.Rollback();
                //            string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(new InvalidOperationException(ex.Message), executionContext);
                //            log.Error(customException);
                //            return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
                //        }
                //    }
                //}
            }
            catch (ValidationException ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
            }
            log.LogMethodExit(createdGamePlayDTOs);
            return Request.CreateResponse(HttpStatusCode.OK, externalGamePlayDTOList);
        }
    }
}