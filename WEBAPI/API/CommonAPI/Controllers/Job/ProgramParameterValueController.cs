/********************************************************************************************
* Project Name - ProgramParameterController
* Description  - Created to Get,Post,Put   programs Parameters
*  
**************
**Version Log
**************
*Version     Date          Modified By               Remarks          
*********************************************************************************************
2.120.1      18-May-2021   B Mahesh Pai             Created as part of AWS concurrent program enhancements
2.120.1      09-Jun-2021   Deeksha                  Modified Post to throw validation ex
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
using System.Threading.Tasks;
using Semnox.CommonAPI.Helpers;


namespace Semnox.CommonAPI.Jobs
{
    public class ProgramParameterValueController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Concurrent Programs
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Jobs/ProgramParameters")]
        public async Task<HttpResponseMessage> Get(string isActive = null, int programParameterValueId = -1,int programId = -1,
                                                        int parameterId = -1, int concurrentProgramScheduleId = -1,
                                                        bool loadActiveChild = false, bool buildChildRecords = false)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(isActive, programParameterValueId);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<ProgramParameterValueDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ProgramParameterValueDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<ProgramParameterValueDTO.SearchByParameters, string>(ProgramParameterValueDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        loadActiveChild = true;
                        searchParameters.Add(new KeyValuePair<ProgramParameterValueDTO.SearchByParameters, string>(ProgramParameterValueDTO.SearchByParameters.IS_ACTIVE, isActive.ToString()));
                    }
                }              
                if (programParameterValueId > -1)
                {
                    searchParameters.Add(new KeyValuePair<ProgramParameterValueDTO.SearchByParameters, string>(ProgramParameterValueDTO.SearchByParameters.PROGRAM_PARAMETER_VALUE_ID, programParameterValueId.ToString()));
                }
                if (programId > -1)
                {
                    searchParameters.Add(new KeyValuePair<ProgramParameterValueDTO.SearchByParameters, string>(ProgramParameterValueDTO.SearchByParameters.PROGRAM_ID, programId.ToString()));
                }
                if (parameterId > -1)
                {
                    searchParameters.Add(new KeyValuePair<ProgramParameterValueDTO.SearchByParameters, string>(ProgramParameterValueDTO.SearchByParameters.PARAMETER_ID, parameterId.ToString()));
                }
                if (concurrentProgramScheduleId > -1)
                {
                    searchParameters.Add(new KeyValuePair<ProgramParameterValueDTO.SearchByParameters, string>(ProgramParameterValueDTO.SearchByParameters.CONCURRENTPROGRAM_SCHEDULE_ID, concurrentProgramScheduleId.ToString()));
                }
                IProgramParameterValueUseCases programParameterValueUseCases = JobUseCaseFactory.GetProgramParameterValues(executionContext);
                List<ProgramParameterValueDTO> programParameterValueDTOList = await programParameterValueUseCases.GetProgramParameterValues(searchParameters, buildChildRecords, loadActiveChild, null);
                log.LogMethodExit(programParameterValueDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = programParameterValueDTOList });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }

        /// <summary>
        /// Post the JSON Concurrent Programs
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/Jobs/ProgramParameters")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody] List<ProgramParameterValueDTO> programParameterValueDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(programParameterValueDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (programParameterValueDTOList != null && programParameterValueDTOList.Any(a => a.ProgramParameterValueId > -1))
                {
                    log.LogMethodExit(programParameterValueDTOList);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }

                IProgramParameterValueUseCases programParameterValueUseCases = JobUseCaseFactory.GetProgramParameterValues(executionContext);
                await programParameterValueUseCases.SaveProgramParameterValues(programParameterValueDTOList);
                log.LogMethodExit(programParameterValueDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = string.Empty });

            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }
        /// <summary>
        /// Post the ProgramParameterValueDTO collection
        /// <param name="programParameterValueDTO">ProgramParameterValueDTO</param>
        [HttpPut]
        [Route("api/Jobs/ProgramParameters")]
        [Authorize]
        public async Task<HttpResponseMessage> Put([FromBody]List<ProgramParameterValueDTO> programParameterValueDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(programParameterValueDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (programParameterValueDTOList == null || programParameterValueDTOList.Any(a => a.ProgramParameterValueId < 0))
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                IProgramParameterValueUseCases programParameterValueUseCases = JobUseCaseFactory.GetProgramParameterValues(executionContext);
                await programParameterValueUseCases.SaveProgramParameterValues(programParameterValueDTOList);
                log.LogMethodExit(programParameterValueDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = string.Empty });
            }

            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }
        /// <summary>
        /// Delete ProgramParameters Record
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpDelete]
        [Route("api/Jobs/ProgramParameters")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody] List<ProgramParameterValueDTO> programParameterValueDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                //programParameterValueDTOList[0].IsChanged = false;
                log.LogMethodEntry(programParameterValueDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (programParameterValueDTOList != null && programParameterValueDTOList.Any())
                {
                    IProgramParameterValueUseCases programParameterValueUseCases = JobUseCaseFactory.GetProgramParameterValues(executionContext);
                    programParameterValueUseCases.Delete(programParameterValueDTOList);
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });
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
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }
    }
}
    
