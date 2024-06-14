/********************************************************************************************
* Project Name - Tools Controller
* Description  - Created to fetch, update and insert concurrent programs
*  
**************
**Version Log
**************
*Version     Date          Modified By               Remarks          
*********************************************************************************************
*2.60        24-May-2019   Jagan Mohana Rao          Created to Get and Post Methods.
*2.90        26-May-2020   Mushahid Faizan           Modified :As per Rest API standard, Added SearchParams and Renamed controller from ConcurrentProgramsController to ConcurrentProgramController
********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Linq;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.JobUtils;
namespace Semnox.CommonAPI.Jobs
{
    public class ConcurrentProgramController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Concurrent Programs
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Jobs/ConcurrentJobs")]
        public HttpResponseMessage Get(string isActive = null, string programName = null, string executableName = null, string systemProgram = null, string errorNotificationMailId = null, bool keepRunning = false,
                                       int programId = -1, string successNotificationMailId = null, bool loadActiveChild = false, bool buildChildRecords = false)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(isActive, programName, executableName, systemProgram, errorNotificationMailId, keepRunning, programId, successNotificationMailId);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<ConcurrentProgramsDTO.SearchByProgramsParameters, string>> searchParameters = new List<KeyValuePair<ConcurrentProgramsDTO.SearchByProgramsParameters, string>>();
                searchParameters.Add(new KeyValuePair<ConcurrentProgramsDTO.SearchByProgramsParameters, string>(ConcurrentProgramsDTO.SearchByProgramsParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        loadActiveChild = true;
                        searchParameters.Add(new KeyValuePair<ConcurrentProgramsDTO.SearchByProgramsParameters, string>(ConcurrentProgramsDTO.SearchByProgramsParameters.ACTIVE_FLAG, isActive.ToString()));
                    }
                }
                if (!string.IsNullOrEmpty(systemProgram))
                {
                    searchParameters.Add(new KeyValuePair<ConcurrentProgramsDTO.SearchByProgramsParameters, string>(ConcurrentProgramsDTO.SearchByProgramsParameters.SYSTEM_PROGRAM, systemProgram.ToString()));
                }
                if (!string.IsNullOrEmpty(errorNotificationMailId))
                {
                    searchParameters.Add(new KeyValuePair<ConcurrentProgramsDTO.SearchByProgramsParameters, string>(ConcurrentProgramsDTO.SearchByProgramsParameters.ERROR_NOTIFICATION_MAIL, errorNotificationMailId.ToString()));
                }
                if (!string.IsNullOrEmpty(executableName))
                {
                    searchParameters.Add(new KeyValuePair<ConcurrentProgramsDTO.SearchByProgramsParameters, string>(ConcurrentProgramsDTO.SearchByProgramsParameters.EXECUTABLE_NAME, executableName.ToString()));
                }
                if (keepRunning)
                {
                    searchParameters.Add(new KeyValuePair<ConcurrentProgramsDTO.SearchByProgramsParameters, string>(ConcurrentProgramsDTO.SearchByProgramsParameters.KEEP_RUNNING, "1"));
                }
                if (programId > -1)
                {
                    searchParameters.Add(new KeyValuePair<ConcurrentProgramsDTO.SearchByProgramsParameters, string>(ConcurrentProgramsDTO.SearchByProgramsParameters.PROGRAM_ID, programId.ToString()));
                }
                if (!string.IsNullOrEmpty(programName))
                {
                    searchParameters.Add(new KeyValuePair<ConcurrentProgramsDTO.SearchByProgramsParameters, string>(ConcurrentProgramsDTO.SearchByProgramsParameters.PROGRAM_NAME, programName.ToString()));
                }
                if (!string.IsNullOrEmpty(successNotificationMailId))
                {
                    searchParameters.Add(new KeyValuePair<ConcurrentProgramsDTO.SearchByProgramsParameters, string>(ConcurrentProgramsDTO.SearchByProgramsParameters.SUCCESS_NOTIFICATION_MAIL, successNotificationMailId.ToString()));
                }
                ConcurrentProgramList concurrentProgramList = new ConcurrentProgramList(executionContext);

                var content = concurrentProgramList.GetAllConcurrentPrograms(searchParameters, buildChildRecords, loadActiveChild);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content });

            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }
        /// <summary>
        /// Post the JSON Concurrent Programs
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>        
        [HttpPost]
        [Route("api/Jobs/ConcurrentJobs")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<ConcurrentProgramsDTO> concurrentProgramsDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(concurrentProgramsDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (concurrentProgramsDTOList != null && concurrentProgramsDTOList.Any())
                {
                    ConcurrentProgramList concurrentProgramList = new ConcurrentProgramList(concurrentProgramsDTOList, executionContext);
                    concurrentProgramList.Save();
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = concurrentProgramsDTOList });
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
    }
}