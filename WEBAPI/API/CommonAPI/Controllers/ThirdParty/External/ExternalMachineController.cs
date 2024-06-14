/********************************************************************************************
 * Project Name - CommonAPI
 * Description  - Created to fetch machine details.
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
using Semnox.Parafait.ThirdParty.External;

namespace Semnox.CommonAPI.Controllers.ThirdParty.External
{
    public class ExternalMachineController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Machines
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/External/Game/Machines")]
        public async Task<HttpResponseMessage> Get(string isActive = null, bool loadAttribute = false, int machineId = -1,
                                       int gameId = -1, int masterId = -1, int referenceMachineId = -1, string machineName = null, int currentPage = 0, int pageSize = 0,
                                       string externalMachineReference = null, bool virtualArcade = false)
        {

            log.LogMethodEntry(isActive, machineId, gameId, masterId, referenceMachineId, machineName, externalMachineReference, virtualArcade);
            ExecutionContext executionContext = null;
            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                List<KeyValuePair<MachineDTO.SearchByMachineParameters, string>> searchParameters = new List<KeyValuePair<MachineDTO.SearchByMachineParameters, string>>();
                searchParameters.Add(new KeyValuePair<MachineDTO.SearchByMachineParameters, string>(MachineDTO.SearchByMachineParameters.SITE_ID, executionContext.SiteId.ToString()));
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y" || isActive.ToString() == "T")
                    {
                        searchParameters.Add(new KeyValuePair<MachineDTO.SearchByMachineParameters, string>(MachineDTO.SearchByMachineParameters.IS_ACTIVE, isActive));
                    }
                }
                if (machineId > -1)
                {
                    searchParameters.Add(new KeyValuePair<MachineDTO.SearchByMachineParameters, string>(MachineDTO.SearchByMachineParameters.MACHINE_ID, machineId.ToString()));
                }
                if (gameId > -1)
                {
                    searchParameters.Add(new KeyValuePair<MachineDTO.SearchByMachineParameters, string>(MachineDTO.SearchByMachineParameters.GAME_ID, gameId.ToString()));
                }
                if (referenceMachineId > -1)
                {
                    searchParameters.Add(new KeyValuePair<MachineDTO.SearchByMachineParameters, string>(MachineDTO.SearchByMachineParameters.REFERENCE_MACHINE_ID, referenceMachineId.ToString()));
                }
                if (masterId > -1)
                {
                    searchParameters.Add(new KeyValuePair<MachineDTO.SearchByMachineParameters, string>(MachineDTO.SearchByMachineParameters.MASTER_ID, masterId.ToString()));
                }
                if (string.IsNullOrEmpty(machineName) == false)
                {
                    searchParameters.Add(new KeyValuePair<MachineDTO.SearchByMachineParameters, string>(MachineDTO.SearchByMachineParameters.MACHINE_NAME, machineName));
                }
                if (string.IsNullOrEmpty(externalMachineReference) == false)
                {
                    searchParameters.Add(new KeyValuePair<MachineDTO.SearchByMachineParameters, string>(MachineDTO.SearchByMachineParameters.EXTERNAL_MACHINE_REFERENCE, externalMachineReference));
                }
                if (virtualArcade)
                {
                    searchParameters.Add(new KeyValuePair<MachineDTO.SearchByMachineParameters, string>(MachineDTO.SearchByMachineParameters.IS_VIRTUAL_ARCADE, "1"));
                }
                List<MachineDTO> machineDTOList = new List<MachineDTO>();
                IMachineUseCases machineDataService = GameUseCaseFactory.GetMachineUseCases(executionContext);
                var content = await machineDataService.GetMachines(searchParameters, loadAttribute, currentPage, pageSize);
                List<ExternalMachinesDTO> externalMachinesDTOList = new List<ExternalMachinesDTO>();
                ExternalMachinesDTO externalMachines = new ExternalMachinesDTO();
                if (content != null && content.Any())
                {
                    foreach (MachineDTO machineDTO in content)
                    {
                        List<KeyValuePair<GameDTO.SearchByGameParameters, string>> searchGameParameters = new List<KeyValuePair<GameDTO.SearchByGameParameters, string>>();
                        searchGameParameters.Add(new KeyValuePair<GameDTO.SearchByGameParameters, string>(GameDTO.SearchByGameParameters.SITE_ID, Convert.ToString(executionContext.SiteId)));
                        if (gameId > -1)
                        {
                            searchGameParameters.Add(new KeyValuePair<GameDTO.SearchByGameParameters, string>(GameDTO.SearchByGameParameters.GAME_ID, gameId.ToString()));
                        }
                        IGameUseCases gamePlayUseCases = GameUseCaseFactory.GetGameUseCases(executionContext);
                        List<GameDTO> gameDTOList = await gamePlayUseCases.GetGames(searchGameParameters, true);
                        bool isVirtualGame = false;
                        if(gameDTOList == null && !gameDTOList.Any())
                        {
                            isVirtualGame = false;
                        }
                        else
                        {
                            isVirtualGame = gameDTOList[0].IsVirtualGame;
                        }
                        externalMachines = new ExternalMachinesDTO(machineDTO.MachineId, machineDTO.MachineName, machineDTO.MachineAddress, isVirtualGame,
                                machineDTO.TicketAllowed, machineDTO.TimerMachine, machineDTO.TimerInterval, machineDTO.NumberOfCoins, machineDTO.TicketMode,
                                machineDTO.ExternalMachineReference, machineDTO.GameId, machineDTO.GameName, machineDTO.HubName, machineDTO.PurchasePrice, machineDTO.MachineCharacteristics);
                        externalMachinesDTOList.Add(externalMachines);
                    }
                }
                log.LogMethodExit(externalMachinesDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, externalMachinesDTOList);
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