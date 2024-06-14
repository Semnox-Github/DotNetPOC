/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - API for the MachineATtributeLog
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
*2.130.0      20-Jul-2021   Abhishek       Created
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
    public class MachineAttributeLogController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object Machine logs
        /// </summary>
        /// <param name="activeFlag">activeFlag</param>
        /// <returns>HttpResponseMessage</returns>
        [Route("api/Game/MachineAttributeLogs")]
        [Authorize]
        public async Task<HttpResponseMessage> Get(int machineAttributeLogId = -1, int gameMachineId = -1,
                                       int posMachineId = -1, string posMachineName = null, string updateType = null, bool status = false)
        {
            log.LogMethodEntry(machineAttributeLogId, gameMachineId, posMachineName, updateType, status);
            ExecutionContext executionContext = null;
            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                List<KeyValuePair<MachineAttributeLogDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<MachineAttributeLogDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<MachineAttributeLogDTO.SearchByParameters, string>(MachineAttributeLogDTO.SearchByParameters.SITE_ID, executionContext.SiteId.ToString()));
                if (machineAttributeLogId > -1)
                {
                    searchParameters.Add(new KeyValuePair<MachineAttributeLogDTO.SearchByParameters, string>(MachineAttributeLogDTO.SearchByParameters.ID, machineAttributeLogId.ToString()));
                }
                if (gameMachineId > -1)
                {
                    searchParameters.Add(new KeyValuePair<MachineAttributeLogDTO.SearchByParameters, string>(MachineAttributeLogDTO.SearchByParameters.MACHINE_ID, gameMachineId.ToString()));
                }
                if (posMachineId > -1)
                {
                    searchParameters.Add(new KeyValuePair<MachineAttributeLogDTO.SearchByParameters, string>(MachineAttributeLogDTO.SearchByParameters.POS_MACHINEID, posMachineId.ToString()));
                }
                if (string.IsNullOrEmpty(posMachineName) == false)
                {
                    searchParameters.Add(new KeyValuePair<MachineAttributeLogDTO.SearchByParameters, string>(MachineAttributeLogDTO.SearchByParameters.POS_NAME, posMachineName));
                }
                if (status)
                {
                    searchParameters.Add(new KeyValuePair<MachineAttributeLogDTO.SearchByParameters, string>(MachineAttributeLogDTO.SearchByParameters.STATUS, status.ToString()));
                }
                if (string.IsNullOrEmpty(updateType))
                {
                    searchParameters.Add(new KeyValuePair<MachineAttributeLogDTO.SearchByParameters, string>(MachineAttributeLogDTO.SearchByParameters.UPDATE_TYPE, updateType));
                }
                List<MachineAttributeLogDTO> machineAttributeLogDTOList = new List<MachineAttributeLogDTO>();
                IMachineUseCases machineUseCases = GameUseCaseFactory.GetMachineUseCases(executionContext);
                machineAttributeLogDTOList = await machineUseCases.GetMachineAttributeLogs(searchParameters);
                log.LogMethodExit(machineAttributeLogDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = machineAttributeLogDTOList });
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
    }
}