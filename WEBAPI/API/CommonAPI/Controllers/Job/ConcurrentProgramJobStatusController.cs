/********************************************************************************************
* Project Name - Tools Controller
* Description  - Created to fetch concurrent programs job status
*  
**************
**Version Log
**************
*Version     Date          Modified By               Remarks          
*********************************************************************************************
*2.90        27-May-2020     Mushahid Faizan         Created
*2.120.1     08-Jun-2021     Deeksha                 Modified as part of AWS concurrent program enhancements
********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.JobUtils;
namespace Semnox.CommonAPI.Jobs
{
    public class ConcurrentProgramJobStatusController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Concurrent Programs job status
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Jobs/ProgramStatus")]
        public HttpResponseMessage Get(DateTime? fromDate = null, DateTime? toDate = null, string phase = null, 
            string status = null, int programId = -1, string programName = null)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(fromDate, toDate, phase, status, programId);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                LookupValuesList serverTimeObject = new LookupValuesList(executionContext);

                List<KeyValuePair<ConcurrentProgramJobStatusDTO.SearchByConcurrentProgramJobStatusParameters, string>> searchParameters = new List<KeyValuePair<ConcurrentProgramJobStatusDTO.SearchByConcurrentProgramJobStatusParameters, string>>();
                searchParameters.Add(new KeyValuePair<ConcurrentProgramJobStatusDTO.SearchByConcurrentProgramJobStatusParameters, string>(ConcurrentProgramJobStatusDTO.SearchByConcurrentProgramJobStatusParameters.SITE_ID, executionContext.GetSiteId().ToString()));

                DateTime startDate = serverTimeObject.GetServerDateTime();
                DateTime endDate = startDate.AddDays(1);

                if (fromDate != null)
                {
                    startDate = Convert.ToDateTime(fromDate.ToString());
                    if (startDate == DateTime.MinValue)
                    {
                        string customException = "Invalid date format, expected format is yyyy-mm-dd hh:mm:ss";
                        log.Error(customException);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
                    }
                }

                if (toDate != null)
                {
                    endDate = Convert.ToDateTime(toDate.ToString());
                    if (endDate == DateTime.MinValue)
                    {
                        string customException = "Invalid date format, expected format is yyyy-mm-dd hh:mm:ss";
                        log.Error(customException);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
                    }
                }
                else
                {
                    endDate = serverTimeObject.GetServerDateTime();
                }


                if (fromDate != null || toDate != null)
                {
                    searchParameters.Add(new KeyValuePair<ConcurrentProgramJobStatusDTO.SearchByConcurrentProgramJobStatusParameters, string>(ConcurrentProgramJobStatusDTO.SearchByConcurrentProgramJobStatusParameters.START_TIME, startDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                    searchParameters.Add(new KeyValuePair<ConcurrentProgramJobStatusDTO.SearchByConcurrentProgramJobStatusParameters, string>(ConcurrentProgramJobStatusDTO.SearchByConcurrentProgramJobStatusParameters.END_TIME, endDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                }

                if (!string.IsNullOrEmpty(phase))
                {
                    searchParameters.Add(new KeyValuePair<ConcurrentProgramJobStatusDTO.SearchByConcurrentProgramJobStatusParameters, string>(ConcurrentProgramJobStatusDTO.SearchByConcurrentProgramJobStatusParameters.PHASE, phase));
                }
                if (!string.IsNullOrEmpty(status))
                {
                    searchParameters.Add(new KeyValuePair<ConcurrentProgramJobStatusDTO.SearchByConcurrentProgramJobStatusParameters, string>(ConcurrentProgramJobStatusDTO.SearchByConcurrentProgramJobStatusParameters.STATUS, status));
                }
                if (!string.IsNullOrEmpty(programName))
                {
                    searchParameters.Add(new KeyValuePair<ConcurrentProgramJobStatusDTO.SearchByConcurrentProgramJobStatusParameters, string>(ConcurrentProgramJobStatusDTO.SearchByConcurrentProgramJobStatusParameters.PROGRAM_NAME, programName));
                }
                if (programId > -1)
                {
                    searchParameters.Add(new KeyValuePair<ConcurrentProgramJobStatusDTO.SearchByConcurrentProgramJobStatusParameters, string>(ConcurrentProgramJobStatusDTO.SearchByConcurrentProgramJobStatusParameters.PROGRAM_ID, programId.ToString()));
                }

                ConcurrentProgramJobStatusList concurrentProgramJobStatusList = new ConcurrentProgramJobStatusList(executionContext);
                List<ConcurrentProgramJobStatusDTO> content = concurrentProgramJobStatusList.GetAllConcurrentProgramStatusList(searchParameters);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content });

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
