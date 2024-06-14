/**************************************************************************************************
 * Project Name - HR 
 * Description  - Controller for WorkShift
 *  
 **************
 **Version Log
 **************
 *Version     Date              Modified By               Remarks          
 **************************************************************************************************
 *1.0        27-Jul-2020       Girish Kundar              Created
 *2.120.0     01-Apr-2021       Prajwal S                 Modified.
 **************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.User;

namespace Semnox.CommonAPI.HR
{
    public class WorkShiftController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object of WorkShiftDTO
        /// </summary>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/HR/WorksShifts")]
        public async Task<HttpResponseMessage> Get(int workShiftId= -1 , string isActive = null ,string shiftName = null, DateTime? startDate =null, string frequency = null,
                                        string status = null , DateTime? endDate = null,bool loadChildRecords = false,bool activeRecords = false)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(workShiftId, shiftName, startDate, endDate, status, frequency);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                List<KeyValuePair<WorkShiftDTO.SearchByWorkShiftParameters, string>> shiftSearchParameter = new List<KeyValuePair<WorkShiftDTO.SearchByWorkShiftParameters, string>>();
                shiftSearchParameter.Add(new KeyValuePair<WorkShiftDTO.SearchByWorkShiftParameters, string>(WorkShiftDTO.SearchByWorkShiftParameters.SITE_ID, Convert.ToString(executionContext.GetSiteId())));
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1")
                    {
                        shiftSearchParameter.Add(new KeyValuePair<WorkShiftDTO.SearchByWorkShiftParameters, string>(WorkShiftDTO.SearchByWorkShiftParameters.IS_ACTIVE, isActive));
                    }
                }
                if (startDate != null)
                {
                    DateTime shiftFromDate = Convert.ToDateTime(startDate);
                    shiftSearchParameter.Add(new KeyValuePair<WorkShiftDTO.SearchByWorkShiftParameters, string>(WorkShiftDTO.SearchByWorkShiftParameters.STARTDATE, shiftFromDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                }
                if (endDate != null)
                {
                    DateTime shiftendDate = Convert.ToDateTime(endDate);
                    shiftSearchParameter.Add(new KeyValuePair<WorkShiftDTO.SearchByWorkShiftParameters, string>(WorkShiftDTO.SearchByWorkShiftParameters.ENDDATE, shiftendDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                }
               
                if (workShiftId > -1)
                {
                    shiftSearchParameter.Add(new KeyValuePair<WorkShiftDTO.SearchByWorkShiftParameters, string>(WorkShiftDTO.SearchByWorkShiftParameters.WORK_SHIFT_ID, workShiftId.ToString()));
                }
                if (!string.IsNullOrEmpty(shiftName))
                {
                    shiftSearchParameter.Add(new KeyValuePair<WorkShiftDTO.SearchByWorkShiftParameters, string>(WorkShiftDTO.SearchByWorkShiftParameters.NAME, shiftName.ToString()));
                }
                if (!string.IsNullOrEmpty(status))
                {
                    shiftSearchParameter.Add(new KeyValuePair<WorkShiftDTO.SearchByWorkShiftParameters, string>(WorkShiftDTO.SearchByWorkShiftParameters.STATUS, status.ToString()));
                }
                if (!string.IsNullOrEmpty(frequency))
                {
                    shiftSearchParameter.Add(new KeyValuePair<WorkShiftDTO.SearchByWorkShiftParameters, string>(WorkShiftDTO.SearchByWorkShiftParameters.FREQUENCY, frequency.ToString()));
                }
                IWorkShiftUseCases workShiftUseCases = UserUseCaseFactory.GetWorkShiftUseCases(executionContext);
                List<WorkShiftDTO> workShiftDTOList = await workShiftUseCases.GetWorkShift(shiftSearchParameter, loadChildRecords, activeRecords);
                log.LogMethodExit(workShiftDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = workShiftDTOList });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }

        /// <summary>
        /// Post the JSON Object of WorkShiftDTO List
        /// </summary>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Authorize]
        [Route("api/HR/WorksShifts")]
        public async Task<HttpResponseMessage> Post([FromBody] List<WorkShiftDTO> workShiftDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(workShiftDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (workShiftDTOList == null || workShiftDTOList.Any(a => a.WorkShiftId > -1))
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                if (workShiftDTOList != null && workShiftDTOList.Any())
                {
                    IWorkShiftUseCases workShiftUseCases = UserUseCaseFactory.GetWorkShiftUseCases(executionContext);
                    List<WorkShiftDTO> workShiftDTOLists = await workShiftUseCases.SaveWorkShift(workShiftDTOList);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = workShiftDTOLists });
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

        /// <summary>
        /// Post the WorkShiftList collection
        /// <param name="workShiftDTOList">WorkShiftList</param>
        [HttpPut]
        [Route("api/HR/WorksShifts")]
        [Authorize]
        public async Task<HttpResponseMessage> Put([FromBody]List<WorkShiftDTO> workShiftDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(workShiftDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (workShiftDTOList == null || workShiftDTOList.Any(a => a.WorkShiftId < 0))
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                IWorkShiftUseCases workShiftUseCases = UserUseCaseFactory.GetWorkShiftUseCases(executionContext);
                workShiftDTOList = await workShiftUseCases.SaveWorkShift(workShiftDTOList);
                log.LogMethodExit(workShiftDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = workShiftDTOList });
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
