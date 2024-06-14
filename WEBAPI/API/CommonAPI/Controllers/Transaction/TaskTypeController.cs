/********************************************************************************************
 * Project Name - Site Setup                                                                     
 * Description  - Controller of the Task Types Controller class
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.60         12-Mar-2019   Jagan Mohana         Created
               08-Apr-2019   Mushahid Faizan      Modified- Added log Method Entry & Exit &
                                                           declared Global ExecutionContext, SecurityTokenDTO, SecurityTokenBL.
*2.90          17-Jul-2020  Girish Kundar         Modified : Moved to transaction resource folder
 ********************************************************************************************/


using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using System.Threading.Tasks;

namespace Semnox.CommonAPI.Transaction
{
    public class TaskTypeController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext = null;
        /// <summary>
        /// Get the JSON Object Task types Details List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Route("api/Transaction/TaskTypes")]
        [Authorize]
        public async Task<HttpResponseMessage> Get(int taskTypeId = -1, string taskType = null)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry();
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<TaskTypesDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<TaskTypesDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<TaskTypesDTO.SearchByParameters, string>(TaskTypesDTO.SearchByParameters.SITE_ID, Convert.ToString(executionContext.SiteId)));
                if (taskTypeId > -1)
                {
                    searchParameters.Add(new KeyValuePair<TaskTypesDTO.SearchByParameters, string>(TaskTypesDTO.SearchByParameters.TASK_TYPE_ID, taskTypeId.ToString()));
                }
                if (!string.IsNullOrEmpty(taskType))
                {
                    searchParameters.Add(new KeyValuePair<TaskTypesDTO.SearchByParameters, string>(TaskTypesDTO.SearchByParameters.TASK_TYPE, taskType.ToString()));
                }
                ITaskTypesUseCases taskTypesUseCases = TaskTypesUseCaseFactory.GetTaskTypesUseCases(executionContext);
                List<TaskTypesDTO> taskTypesDTOList = await taskTypesUseCases.GetTaskTypes(searchParameters);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = taskTypesDTOList });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }
        /// <summary>
        /// Performs a Post operation on taskTypesDTOs details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/Transaction/TaskTypes")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody] List<TaskTypesDTO> taskTypesDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(taskTypesDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));


                if (taskTypesDTOList != null)
                {
                    ITaskTypesUseCases taskTypesUseCases = TaskTypesUseCaseFactory.GetTaskTypesUseCases(executionContext);
                    await taskTypesUseCases.SaveTaskTypes(taskTypesDTOList);
                    log.LogMethodExit(taskTypesDTOList);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = string.Empty });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "" });
                }
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
