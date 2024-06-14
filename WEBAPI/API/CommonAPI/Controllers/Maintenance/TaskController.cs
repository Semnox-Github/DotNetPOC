/********************************************************************************************
 * Project Name - Maintenance
 * Description  - Create and update maintenance tasks
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.60       23-Apr-2019   Muhammed Mehraj          Created 
 *2.70       09-Apr-2020   Rakesh Kumar             Modify Get method
 *2.80       22-Apr-2020   Mushahid Faizan          Modified end points and Removed token from response body.
 *2.80       22-Apr-2020   Girish Kundar            Modified:Added int taskId =-1 , int taskGroupId =-1, string taskName = null as GET parameter
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Maintenance;
namespace Semnox.CommonAPI.Controllers.Maintenance
{
    public class TaskController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get request for Maintenance Tasks
        /// </summary>
        /// <param name="isActive"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("api/Maintenance/Tasks")]
        public HttpResponseMessage Get(string isActive = null, int taskId =-1 , int taskGroupId =-1, string taskName = null)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(isActive, taskId , taskGroupId , taskName);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                JobTaskList maintenanceTaskList = new JobTaskList(executionContext);
                List<KeyValuePair<JobTaskDTO.SearchByJobTaskParameters, string>> maintenanceTaskSearchParams = new List<KeyValuePair<JobTaskDTO.SearchByJobTaskParameters, string>>();
                maintenanceTaskSearchParams.Add(new KeyValuePair<JobTaskDTO.SearchByJobTaskParameters, string>(JobTaskDTO.SearchByJobTaskParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        maintenanceTaskSearchParams.Add(new KeyValuePair<JobTaskDTO.SearchByJobTaskParameters, string>(JobTaskDTO.SearchByJobTaskParameters.IS_ACTIVE, isActive));
                    }
                }
                if (taskId > -1)
                {
                    maintenanceTaskSearchParams.Add(new KeyValuePair<JobTaskDTO.SearchByJobTaskParameters, string>(JobTaskDTO.SearchByJobTaskParameters.JOB_TASK_ID, taskId.ToString()));
                }
                if (taskGroupId > -1)
                {
                    maintenanceTaskSearchParams.Add(new KeyValuePair<JobTaskDTO.SearchByJobTaskParameters, string>(JobTaskDTO.SearchByJobTaskParameters.JOB_TASK_GROUP_ID, taskGroupId.ToString()));
                }
                if (string.IsNullOrEmpty(taskName) == false)
                {
                    maintenanceTaskSearchParams.Add(new KeyValuePair<JobTaskDTO.SearchByJobTaskParameters, string>(JobTaskDTO.SearchByJobTaskParameters.TASK_NAME, taskName));
                }
                List<JobTaskDTO> maintenanceTaskListOnDisplay = maintenanceTaskList.GetAllJobTasks(maintenanceTaskSearchParams);
                log.LogMethodExit();
                return Request.CreateResponse(HttpStatusCode.OK, new { data = maintenanceTaskListOnDisplay });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }
        /// <summary>
        /// Post request for Tasks
        /// </summary>
        /// <param name="jobTaskDTOList"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("api/Maintenance/Tasks")]
        public HttpResponseMessage Post([FromBody] List<JobTaskDTO> jobTaskDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(jobTaskDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (jobTaskDTOList != null && jobTaskDTOList.Count > 0)
                {
                    JobTaskList maintenanceTaskList = new JobTaskList(executionContext, jobTaskDTOList);
                    maintenanceTaskList.SaveJobTasks();
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = jobTaskDTOList });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
              
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
        /// Post request for Tasks
        /// </summary>
        /// <param name="jobTaskDTOList"></param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize]
        [Route("api/Maintenance/Tasks")]
        public HttpResponseMessage Delete([FromBody] List<JobTaskDTO> jobTaskDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(jobTaskDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (jobTaskDTOList != null && jobTaskDTOList.Count > 0)
                {
                    JobTaskList maintenanceTaskList = new JobTaskList(executionContext, jobTaskDTOList);
                    maintenanceTaskList.SaveJobTasks();
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = jobTaskDTOList });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = string.Empty });
                }

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
