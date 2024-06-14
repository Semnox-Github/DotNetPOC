/********************************************************************************************
 * Project Name - Maintenance
 * Description  - Creates a Group of MaintenanceTasks called TaskGroup
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.60        23-Apr-2019   Muhammed Mehraj          Created 
 *2.70        09-Apr-2019   Rakesh Kumar             Modify Get method
 *2.80        22-Apr-2020   Mushahid Faizan          Modified end points and Removed token from response body.
 *2.80        22-Apr-2020   Girish Kundar            Modified : Added int taskGroupId =-1 , string taskGroupName = null, bool? hasActiveTask = null to GET
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
    public class TaskGroupController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get request for TaskGroup
        /// </summary>
        /// <param name="isActive"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("api/Maintenance/TaskGroups")]
        public HttpResponseMessage Get(string isActive = null, int taskGroupId =-1 , string taskGroupName = null, bool? hasActiveTask = null)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(isActive, taskGroupId, taskGroupName , hasActiveTask);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                List<KeyValuePair<JobTaskGroupDTO.SearchByJobTaskGroupParameters, string>> maintenanceTaskGroupSearchParams = new List<KeyValuePair<JobTaskGroupDTO.SearchByJobTaskGroupParameters, string>>();
                maintenanceTaskGroupSearchParams.Add(new KeyValuePair<JobTaskGroupDTO.SearchByJobTaskGroupParameters, string>(JobTaskGroupDTO.SearchByJobTaskGroupParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        maintenanceTaskGroupSearchParams.Add(new KeyValuePair<JobTaskGroupDTO.SearchByJobTaskGroupParameters, string>(JobTaskGroupDTO.SearchByJobTaskGroupParameters.IS_ACTIVE, isActive));
                    }
                }
                if (taskGroupId > -1)
                {
                    maintenanceTaskGroupSearchParams.Add(new KeyValuePair<JobTaskGroupDTO.SearchByJobTaskGroupParameters, string>(JobTaskGroupDTO.SearchByJobTaskGroupParameters.JOB_TASK_GROUP_ID, taskGroupId.ToString()));
                }
                if (string.IsNullOrEmpty(taskGroupName) == false)
                {
                    maintenanceTaskGroupSearchParams.Add(new KeyValuePair<JobTaskGroupDTO.SearchByJobTaskGroupParameters, string>(JobTaskGroupDTO.SearchByJobTaskGroupParameters.TASK_GROUP_NAME, taskGroupName));
                }
                if (hasActiveTask != null)
                {
                    bool value = Convert.ToBoolean(hasActiveTask);
                    maintenanceTaskGroupSearchParams.Add(new KeyValuePair<JobTaskGroupDTO.SearchByJobTaskGroupParameters, string>(JobTaskGroupDTO.SearchByJobTaskGroupParameters.HAS_ACTIVE_TASKS, "1"));
                }
                JobTaskGroupList maintenanceTaskGroupList = new JobTaskGroupList(executionContext);
                List<JobTaskGroupDTO> maintenanceTaskGroupListOnDisplay = maintenanceTaskGroupList.GetAllJobTaskGroups(maintenanceTaskGroupSearchParams);
                log.LogMethodExit();
                return Request.CreateResponse(HttpStatusCode.OK, new { data = maintenanceTaskGroupListOnDisplay });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        /// <summary>
        /// Post request for TaskGroup
        /// </summary>
        /// <param name="jobTaskGroupDTOList"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("api/Maintenance/TaskGroups")]
        public HttpResponseMessage Post([FromBody] List<JobTaskGroupDTO> jobTaskGroupDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry();
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (jobTaskGroupDTOList != null || jobTaskGroupDTOList.Count > 0)
                {
                    JobTaskGroupList maintenanceTaskGroupList = new JobTaskGroupList(executionContext, jobTaskGroupDTOList);
                    maintenanceTaskGroupList.SaveJobTaskGroup();
                    log.LogMethodExit(jobTaskGroupDTOList);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = jobTaskGroupDTOList });
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
        /// Post request for TaskGroup
        /// </summary>
        /// <param name="jobTaskGroupDTOList"></param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize]
        [Route("api/Maintenance/TaskGroups")]
        public HttpResponseMessage Delete([FromBody] List<JobTaskGroupDTO> jobTaskGroupDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry();
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (jobTaskGroupDTOList != null || jobTaskGroupDTOList.Count > 0)
                {
                    JobTaskGroupList maintenanceTaskGroupList = new JobTaskGroupList(executionContext, jobTaskGroupDTOList);
                    maintenanceTaskGroupList.SaveJobTaskGroup();
                    log.LogMethodExit(jobTaskGroupDTOList);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = jobTaskGroupDTOList });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
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
