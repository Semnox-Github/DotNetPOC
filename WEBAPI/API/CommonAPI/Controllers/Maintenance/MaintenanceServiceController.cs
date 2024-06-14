/********************************************************************************************
 * Project Name - Maintenance Service
 * Description  - Created to insert update Maintenance Requests    -- Maintenance Job Details
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *2.80        15-Nov-2019   Jagan Mohana        Created
 *2.80        24-Dec-2019   Vikas Dwivedi       Modified scheduleFromDate and scheduleToDate in Get()
 *2.80        22-Apr-2020   Mushahid Faizan     Modified end points and Removed token from response body.
 *2.100.0     24-Sept-2020  Mushahid Faizan     Modified for Service Request enhancement
 ********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.GenericUtilities.Excel;
using Semnox.Core.Utilities;
using Semnox.Parafait.Maintenance;
using Semnox.Parafait.Site;
using Semnox.Parafait.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Semnox.CommonAPI.Controllers.Maintenance
{
    public class MaintenanceServiceController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get Request to get the collection of completed and pending jobs
        /// </summary>
        /// <param name="isActive"></param>
        /// <param name="jobName"></param>
        /// <param name="assetId"></param>
        /// <param name="assignedTo"></param>
        /// <param name="requestType"></param>
        /// <param name="status"></param>
        /// <param name="priority"></param>
        /// <param name="scheduleFromDate"></param>
        /// <param name="scheduleToDate"></param>
        /// <param name="reqFromDate"></param>
        /// <param name="reqToDate"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("api/Maintenance/MaintenanceServices")]
        public HttpResponseMessage Get(string activityType = "MAINTENANCEREQUESTS", string isActive = "1", int assetId = -1, int status = -1, int requestType = -1, string title = null, int priority = -1,
                                      string scheduleFromDate = null, string scheduleToDate = null, string reqFromDate = null, string reqToDate = null,
                                      int jobId = -1, int taskId = -1, int assignedTo = -1, bool jobsPastDueDate = false, bool loadActiveChild = false, bool buildChildRecords = false,
                                      string requestedBy = null, string jobNumber = null)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(isActive, title, assetId, requestType, status, priority, scheduleFromDate, scheduleToDate, reqFromDate, reqToDate);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), -1);

                List<KeyValuePair<UsersDTO.SearchByUserParameters, string>> searchByParameters = new List<KeyValuePair<UsersDTO.SearchByUserParameters, string>>();
                searchByParameters.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.SITE_ID, securityTokenDTO.SiteId.ToString()));
                searchByParameters.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.LOGIN_ID, securityTokenDTO.LoginId));
                UsersList usersList = new UsersList(executionContext);
                List<UsersDTO> usersDTOs = usersList.GetAllUsers(searchByParameters);
                UsersDTO user = usersDTOs.Find(x => x.LoginId == securityTokenDTO.LoginId);
                //executionContext = new ExecutionContext(user.LoginId, user.SiteId, -1, user.UserId, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), -1);

                List<KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string>> maintenanceJobSearchParams = new List<KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string>>();
                if (executionContext.GetSiteId() > -1)
                {
                    maintenanceJobSearchParams.Add(new KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string>(UserJobItemsDTO.SearchByUserJobItemsParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                }
                LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupOpenValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                lookupOpenValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (activityType.ToUpper().ToString() == "MAINTENANCEREQUESTS" || activityType.ToUpper().ToString() == "MAINTENANCEREQUESTSEXPORT")
                {
                    lookupOpenValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "MAINT_JOB_TYPE"));
                    lookupOpenValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, "Service Request"));
                }
                if (activityType.ToUpper().ToString() == "MAINTENANCEJOBDETAILS")
                {
                    lookupOpenValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "MAINT_JOB_TYPE"));
                    lookupOpenValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, "Job"));
                }
                List<LookupValuesDTO> lookupOpenValuesDTOList = lookupValuesList.GetAllLookupValues(lookupOpenValuesSearchParams);
                int jobTypeId = -1;
                if (lookupOpenValuesDTOList != null && lookupOpenValuesDTOList.Count != 0)
                {
                    jobTypeId = lookupOpenValuesDTOList[0].LookupValueId;
                }
                if (isActive == "1")
                {
                    maintenanceJobSearchParams.Add(new KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string>(UserJobItemsDTO.SearchByUserJobItemsParameters.IS_ACTIVE, isActive));
                }
                /// JobTypeId for the both JobDetails and Service Request and it will fetch from lookups
                if (jobTypeId > 0)
                {
                    maintenanceJobSearchParams.Add(new KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string>(UserJobItemsDTO.SearchByUserJobItemsParameters.JOB_TYPE, (jobTypeId == -1) ? "" : jobTypeId.ToString()));
                }
                if (assetId > 0)
                {
                    maintenanceJobSearchParams.Add(new KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string>(UserJobItemsDTO.SearchByUserJobItemsParameters.ASSET_ID, Convert.ToString(assetId)));
                }
                if (status > 0)
                {
                    maintenanceJobSearchParams.Add(new KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string>(UserJobItemsDTO.SearchByUserJobItemsParameters.STATUS, Convert.ToString(status)));
                }
                if (!string.IsNullOrEmpty(scheduleFromDate))
                {
                    maintenanceJobSearchParams.Add(new KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string>(UserJobItemsDTO.SearchByUserJobItemsParameters.SCHEDULE_FROM_DATE, DateTime.Parse(scheduleFromDate).ToString("yyyy-MM-dd HH:mm:ss")));
                }
                if (!string.IsNullOrEmpty(scheduleToDate))
                {
                    maintenanceJobSearchParams.Add(new KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string>(UserJobItemsDTO.SearchByUserJobItemsParameters.SCHEDULE_TO_DATE, DateTime.Parse(scheduleToDate).ToString("yyyy-MM-dd HH:mm:ss")));
                }
                if (!string.IsNullOrEmpty(requestedBy))
                {
                    maintenanceJobSearchParams.Add(new KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string>(UserJobItemsDTO.SearchByUserJobItemsParameters.REQUESTED_BY, requestedBy));
                }
                if (jobId > 0)
                {
                    maintenanceJobSearchParams.Add(new KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string>(UserJobItemsDTO.SearchByUserJobItemsParameters.USER_JOB_ID, jobId.ToString()));
                }
                if (!string.IsNullOrEmpty(jobNumber))
                {
                    maintenanceJobSearchParams.Add(new KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string>(UserJobItemsDTO.SearchByUserJobItemsParameters.JOB_NUMBER, jobNumber));
                }
                if (activityType.ToUpper().ToString() == "MAINTENANCEREQUESTS" || activityType.ToUpper().ToString() == "MAINTENANCEREQUESTSEXPORT")
                {
                    if (requestType > 0)
                    {
                        maintenanceJobSearchParams.Add(new KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string>(UserJobItemsDTO.SearchByUserJobItemsParameters.REQUEST_TYPE_ID, Convert.ToString(requestType)));
                    }
                    if (!string.IsNullOrEmpty(title))
                    {
                        maintenanceJobSearchParams.Add(new KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string>(UserJobItemsDTO.SearchByUserJobItemsParameters.JOB_NAME, title));
                    }
                    if (priority > 0)
                    {
                        maintenanceJobSearchParams.Add(new KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string>(UserJobItemsDTO.SearchByUserJobItemsParameters.PRIORITY, Convert.ToString(priority)));
                    }
                    if (!string.IsNullOrEmpty(reqFromDate))
                    {
                        maintenanceJobSearchParams.Add(new KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string>(UserJobItemsDTO.SearchByUserJobItemsParameters.REQUEST_FROM_DATE, DateTime.Parse(reqFromDate).ToString("yyyy-MM-dd")));
                    }
                    else
                    {
                        reqFromDate = DateTime.Now.ToString();
                    }
                    if (!string.IsNullOrEmpty(reqToDate))
                    {
                        maintenanceJobSearchParams.Add(new KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string>(UserJobItemsDTO.SearchByUserJobItemsParameters.REQUEST_TO_DATE, DateTime.Parse(reqToDate).ToString("yyyy-MM-dd")));
                    }
                    else
                    {
                        reqToDate = DateTime.Now.ToString();
                    }
                }

                if (activityType.ToUpper().ToString() == "MAINTENANCEJOBDETAILS")
                {
                    if (taskId > 0)
                    {
                        maintenanceJobSearchParams.Add(new KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string>(UserJobItemsDTO.SearchByUserJobItemsParameters.TASK_ID, Convert.ToString(taskId)));
                    }
                    if (assignedTo > 0)
                    {
                        maintenanceJobSearchParams.Add(new KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string>(UserJobItemsDTO.SearchByUserJobItemsParameters.ASSIGNED_TO, Convert.ToString(assignedTo)));
                    }
                    if (jobsPastDueDate)
                    {
                        maintenanceJobSearchParams.Add(new KeyValuePair<UserJobItemsDTO.SearchByUserJobItemsParameters, string>(UserJobItemsDTO.SearchByUserJobItemsParameters.PAST_DUE_DATE, "Y"));
                    }
                }

                UserJobItemsListBL userJobItemsListBL = new UserJobItemsListBL(executionContext);
                List<UserJobItemsDTO> userJobItemsDTOList = new List<UserJobItemsDTO>();
                if (activityType.ToUpper().ToString() == "MAINTENANCEREQUESTS")
                {
                    userJobItemsDTOList = userJobItemsListBL.GetAllUserJobItemDTOList(maintenanceJobSearchParams, -1, buildChildRecords, loadActiveChild);
                }
                else
                {
                    userJobItemsDTOList = userJobItemsListBL.GetAllUserJobItemDTOList(maintenanceJobSearchParams, user.UserId, buildChildRecords, loadActiveChild);
                }



                Sheet sheet = new Sheet();
                if (activityType.ToUpper().ToString() == "MAINTENANCEREQUESTSEXPORT")
                {
                    userJobItemsListBL = new UserJobItemsListBL(executionContext, userJobItemsDTOList);
                    string siteName = string.Empty;
                    List<KeyValuePair<SiteDTO.SearchBySiteParameters, string>> searchParameters = new List<KeyValuePair<SiteDTO.SearchBySiteParameters, string>>();
                    searchParameters.Add(new KeyValuePair<SiteDTO.SearchBySiteParameters, string>(SiteDTO.SearchBySiteParameters.SITE_ID, Convert.ToString(securityTokenDTO.SiteId)));
                    SiteList siteList = new SiteList(executionContext);
                    var content = siteList.GetAllSites(searchParameters);
                    if (content != null)
                    {
                        siteName = content[0].SiteName;
                    }
                    sheet = userJobItemsListBL.BuildTemplate(reqFromDate, reqToDate, siteName);
                }
                if (userJobItemsDTOList != null && userJobItemsDTOList.Any())
                {
                    userJobItemsDTOList = userJobItemsDTOList.OrderBy(x => x.MaintChklstdetId).ToList();
                }
                log.LogMethodExit(userJobItemsDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = userJobItemsDTOList, Sheet = sheet });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        /// <summary>
        /// Post the jobs collection inserts or updates the jobs
        /// </summary>
        /// <param name="userJobItemsDTOList"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("api/Maintenance/MaintenanceServices")]
        public HttpResponseMessage Post(string activityType, [FromBody]List<UserJobItemsDTO> userJobItemsDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(activityType, userJobItemsDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (userJobItemsDTOList != null || userJobItemsDTOList.Count > 0)
                {
                    UserJobItemsListBL userJobItemsListBL = new UserJobItemsListBL(executionContext, userJobItemsDTOList);
                    userJobItemsListBL.SaveJobDetails(activityType);
                    log.LogMethodExit(userJobItemsDTOList);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = userJobItemsDTOList });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = string.Empty });
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
    }
}
