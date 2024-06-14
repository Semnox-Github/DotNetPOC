/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - API for the Machines
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.40        07-Sept-2018   Jagan          Created 
 *2.80        12-May-2020    Girish Kundar  Modified :Added  Get parameters - loadAttributes = false,  machineId = -1,gameId = -1,  masterId = -1,  referenceMachineId = -1,  machineName ,  externalMachineReference
 *2.90       16-Jun-2020    Mushahid Faizan Modified :  Removed Post method and Moved Sheet object logic to SheetUploadController.
*2.100       27-Oct-2020    Girish Kundar        Modified : Implemented factory class to get/save the data
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Game;
namespace Semnox.CommonAPI.Games
{
    public class MachinesController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object Machine Details List
        /// </summary>
        /// <param name="activeFlag">activeFlag</param>
        /// <returns>HttpResponseMessage</returns>
        [Route("api/Game/Machines")]
        [Authorize]
        public async Task<HttpResponseMessage> Get(string isActive = null, bool loadAttribute = false, int machineId = -1,
                                       int gameId = -1, int masterId = -1, int referenceMachineId = -1, string machineName = null, int currentPage = 0, int pageSize = 0,
                                       string externalMachineReference = null, bool virtualArcade = false)
        {

            log.LogMethodEntry(isActive, machineId, gameId, masterId, referenceMachineId, machineName, externalMachineReference);
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
                log.LogMethodExit(machineDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content });
            }
            catch (UnauthorizedException ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        /// <summary>
        /// Post the JSON Object Machines Details
        /// </summary>
        /// <param name="machinesDTOList">machineDtoList</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/Game/Machines")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody] List<MachineDTO> machinesDTOList)
        {

            log.LogMethodEntry(machinesDTOList);
            ExecutionContext executionContext = null;
            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                if (machinesDTOList != null && machinesDTOList.Any())
                {
                    IMachineUseCases machineDataService = GameUseCaseFactory.GetMachineUseCases(executionContext);
                    var response = await machineDataService.SaveMachines(machinesDTOList);
                    log.LogMethodExit(response);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = machinesDTOList });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "" });
                }
            }
            catch (UnauthorizedException ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
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

        /// <summary>
        /// Delete the Machine Details
        /// </summary>
        /// <param name="machinesList">machineDtoList</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpDelete]
        [Route("api/Game/Machines")]
        [Authorize]
        public async Task<HttpResponseMessage> Delete(List<MachineDTO> machinesList)
        {
            log.LogMethodEntry(machinesList);
            ExecutionContext executionContext = null;
            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                if (machinesList != null && machinesList.Any())
                {
                    IMachineUseCases machineDataService = GameUseCaseFactory.GetMachineUseCases(executionContext);
                    var response = await machineDataService.DeleteMachines(machinesList);
                    log.LogMethodExit(response);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "" });
                }
            }
            catch (UnauthorizedException ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
            catch (ValidationException ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
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