/******************************************************************************************
 * Project Name - Tools Controller
 * Description  - Created to fetch, update and insert machine groups
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By               Remarks          
 *********************************************************************************************
 *2.60        13-May-2019   Jagan Mohana Rao          Created to Get and Post Methods.
 *2.90        04-Jun-2020   Mushahid Faizan           Modified :As per Rest API standard, Added SearchParams and Renamed controller from MachineGroupsController to MachineGroupController
 ********************************************************************************************/

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

namespace Semnox.CommonAPI.Games
{
    public class MachineGroupController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON MachineGroups
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Games/MachineGroups")]
        public async Task<HttpResponseMessage> Get(string isActive = null, int machineGroupId = -1, string groupName = null, int pageSize= 0, int currentpage = 0, bool loadActiveChild = false, bool buildChildRecords = false)
        {
            log.LogMethodEntry(isActive);

            ExecutionContext executionContext = null;
            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                List<KeyValuePair<MachineGroupsDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<MachineGroupsDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<MachineGroupsDTO.SearchByParameters, string>(MachineGroupsDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<MachineGroupsDTO.SearchByParameters, string>(MachineGroupsDTO.SearchByParameters.ISACTIVE, isActive.ToString()));
                    }
                }
                if (machineGroupId > 0)
                {
                    searchParameters.Add(new KeyValuePair<MachineGroupsDTO.SearchByParameters, string>(MachineGroupsDTO.SearchByParameters.MACHINE_GROUP_ID, machineGroupId.ToString()));
                }
                if (!string.IsNullOrEmpty(groupName))
                {
                    searchParameters.Add(new KeyValuePair<MachineGroupsDTO.SearchByParameters, string>(MachineGroupsDTO.SearchByParameters.GROUP_NAME, groupName.ToString()));
                }
                IMachineGroupsUseCases machineGroupsUseCases = GameUseCaseFactory.GetMachineGroupsUseCases(executionContext);
                var content = await machineGroupsUseCases.GetMachineGroups(searchParameters, buildChildRecords, loadActiveChild, currentpage, pageSize);
                log.LogMethodExit(content);
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
        /// Post the JSON machineGroupsDTOList
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>        
        [HttpPost]
        [Route("api/Games/MachineGroups")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody] List<MachineGroupsDTO> machineGroupsDTOList)
        {
            log.LogMethodEntry(machineGroupsDTOList);

            ExecutionContext executionContext = null;
            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                if (machineGroupsDTOList != null && machineGroupsDTOList.Any())
                {
                    IMachineGroupsUseCases machineGroupsUseCases = GameUseCaseFactory.GetMachineGroupsUseCases(executionContext);
                    var content = await machineGroupsUseCases.SaveMachineGroups(machineGroupsDTOList);
                    log.LogMethodExit(content);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = machineGroupsDTOList });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
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
    }
}
