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
 ********************************************************************************************/
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Semnox.Core.Utilities;
using System.Web;
using Semnox.Core.GenericUtilities;
using System;

namespace Semnox.CommonAPI.SiteSetup
{
    public class TaskTypeController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private SecurityTokenDTO securityTokenDTO;
        private SecurityTokenBL securityTokenBL = new SecurityTokenBL();

        /// <summary>
        /// Get the JSON Object Task types Details List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/SiteSetup/TaskType/")]
        public HttpResponseMessage Get()
        {
            try
            {
                log.LogMethodEntry();
                this.securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<TaskTypesDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<TaskTypesDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<TaskTypesDTO.SearchByParameters, string>(TaskTypesDTO.SearchByParameters.SITE_ID, Convert.ToString(securityTokenDTO.SiteId)));

                TaskTypesList taskTypesList = new TaskTypesList(executionContext);
                var content = taskTypesList.GetAllTaskTypes(searchParameters);
                log.LogMethodExit(content);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content, token = securityTokenDTO.Token });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ex.Message, token = securityTokenDTO.Token });
            }
        }
        /// <summary>
        /// Performs a Post operation on taskTypesDTOs details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/SiteSetup/TaskType/")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<TaskTypesDTO> taskTypesDTOs)
        {
            try
            {
                log.LogMethodEntry(taskTypesDTOs);
                this.securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId)); ;
                if (taskTypesDTOs != null)
                {
                    // if taskTypesDTOs.taskTypeId is less than zero then insert or else update
                    TaskTypesList taskTypesList = new TaskTypesList(executionContext, taskTypesDTOs);
                    taskTypesList.SaveUpdateTaskTypesList();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "", token = securityTokenDTO.Token });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { data = "", token = securityTokenDTO.Token });
                }
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, token = securityTokenDTO.Token });
            }
        }
    }
}
