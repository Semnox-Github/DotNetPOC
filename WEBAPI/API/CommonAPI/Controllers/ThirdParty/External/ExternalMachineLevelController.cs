/********************************************************************************************
 * Project Name - CommonAPI
 * Description  - Created to fetch machine levels.
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
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Game;
using Semnox.Parafait.Game.VirtualArcade;
using Semnox.Parafait.ThirdParty.External;

namespace Semnox.CommonAPI.Controllers.ThirdParty.External
{
    public class ExternalMachineLevelController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON GameMachineLevels
        /// </summary>       
        /// <param name="gameMachineLevelId">gameMachineLevelId</param>
        /// <param name="machineId">machineId</param>
        /// <param name="levelName">levelName</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/External/Game/GameMachineLevels")]
        public async Task<HttpResponseMessage> Get(string isActive = null, int gameMachineLevelId = -1, int machineId = -1, string levelName = null)
        {
            log.LogMethodEntry(isActive, gameMachineLevelId, machineId);
            ExecutionContext executionContext = null;
            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                List<KeyValuePair<GameMachineLevelDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<GameMachineLevelDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<GameMachineLevelDTO.SearchByParameters, string>(GameMachineLevelDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));

                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<GameMachineLevelDTO.SearchByParameters, string>(GameMachineLevelDTO.SearchByParameters.IS_ACTIVE, isActive));
                    }
                }
                if (!string.IsNullOrEmpty(levelName))
                {
                    searchParameters.Add(new KeyValuePair<GameMachineLevelDTO.SearchByParameters, string>(GameMachineLevelDTO.SearchByParameters.LEVEL_NAME, levelName));
                }
                if (gameMachineLevelId > -1)
                {
                    searchParameters.Add(new KeyValuePair<GameMachineLevelDTO.SearchByParameters, string>(GameMachineLevelDTO.SearchByParameters.GAME_MACHINE_LEVEL_ID, gameMachineLevelId.ToString()));
                }
                if (machineId > -1)
                {
                    searchParameters.Add(new KeyValuePair<GameMachineLevelDTO.SearchByParameters, string>(GameMachineLevelDTO.SearchByParameters.MACHINE_ID, machineId.ToString()));
                }

                IGameMachineLevelUseCases gameMachineLevelUseCase = GameUseCaseFactory.GetGameMachineLevelUseCases(executionContext);
                List<GameMachineLevelDTO> gameMachineLevelDTOList = await gameMachineLevelUseCase.GetGameMachineLevels(searchParameters);
                List<ExternalMachineLevelDTO> externalMachineLevelDTOList = new List<ExternalMachineLevelDTO>();
                ExternalMachineLevelDTO externalMachineLevel = new ExternalMachineLevelDTO();
                if (gameMachineLevelDTOList != null && gameMachineLevelDTOList.Any())
                {
                    foreach (GameMachineLevelDTO gameMachineLevelDTO in gameMachineLevelDTOList)
                    {
                        externalMachineLevel = new ExternalMachineLevelDTO(gameMachineLevelDTO.GameMachineLevelId, gameMachineLevelDTO.MachineId,
                            gameMachineLevelDTO.LevelName, gameMachineLevelDTO.LevelCharacteristics, gameMachineLevelDTO.QualifyingScore, gameMachineLevelDTO.ScoreToVPRatio,
                            gameMachineLevelDTO.ScoreToXPRatio, gameMachineLevelDTO.TranslationFileName, gameMachineLevelDTO.ImageFileName, gameMachineLevelDTO.AutoLoadEntitlement, gameMachineLevelDTO.EntitlementType);
                        externalMachineLevelDTOList.Add(externalMachineLevel);
                    }
                }
                log.LogMethodExit(externalMachineLevelDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, externalMachineLevelDTOList);
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
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }
    }
}