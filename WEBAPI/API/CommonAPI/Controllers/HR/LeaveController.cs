/********************************************************************************************
* Project Name - CommnonAPI - HR Module 
* Description  - API for the ApplyLeave.
* 
**************
**Version Log
**************
*Version     Date            Modified By         Remarks          
*********************************************************************************************
*2.80.0      13-Nov-2019     Indrajeet Kumar     Created
*2.90        20-May-2020     Vikas Dwivedi       Modified as per the Standard CheckList
*2.120.0     01-Apr-2021     Prajwal S           Modified.
********************************************************************************************/

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
    public class LeaveController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        [HttpGet]
        [Authorize]
        [Route("api/HR/Leaves")]
        public async Task<HttpResponseMessage> Get(int leaveId = -1, int leaveCycleId = -1, int userId = -1, int leaveTypeId = -1, string type = null, int leaveTemplateId = -1,
                                       int approvedBy = -1, DateTime? startDate = null, DateTime? endDate = null, string isActive = null)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(leaveId, leaveCycleId, userId, leaveTypeId, type, leaveTemplateId, approvedBy, startDate, endDate);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<LeaveDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<LeaveDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<LeaveDTO.SearchByParameters, string>(LeaveDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (leaveId > -1)
                {
                    searchParameters.Add(new KeyValuePair<LeaveDTO.SearchByParameters, string>(LeaveDTO.SearchByParameters.LEAVE_ID, leaveId.ToString()));
                }
                if (leaveCycleId > -1)
                {
                    searchParameters.Add(new KeyValuePair<LeaveDTO.SearchByParameters, string>(LeaveDTO.SearchByParameters.LEAVE_CYCLE_ID, leaveCycleId.ToString()));
                }
                if (userId > -1)
                {
                    searchParameters.Add(new KeyValuePair<LeaveDTO.SearchByParameters, string>(LeaveDTO.SearchByParameters.USER_ID, userId.ToString()));
                }
                if (leaveTypeId > -1)
                {
                    searchParameters.Add(new KeyValuePair<LeaveDTO.SearchByParameters, string>(LeaveDTO.SearchByParameters.LEAVE_TYPE_ID, leaveTypeId.ToString()));
                }
                if (!string.IsNullOrEmpty(type))
                {
                    searchParameters.Add(new KeyValuePair<LeaveDTO.SearchByParameters, string>(LeaveDTO.SearchByParameters.TYPE, type.ToString()));
                }
                if (leaveTemplateId > -1)
                {
                    searchParameters.Add(new KeyValuePair<LeaveDTO.SearchByParameters, string>(LeaveDTO.SearchByParameters.LEAVE_TEMPLATE_ID, leaveTemplateId.ToString()));
                }
                if (approvedBy > -1)
                {
                    searchParameters.Add(new KeyValuePair<LeaveDTO.SearchByParameters, string>(LeaveDTO.SearchByParameters.APPROVED_BY, approvedBy.ToString()));
                }
                DateTime fromDate = DateTime.Now;
                DateTime toDate = DateTime.Now.AddDays(1);
                if (startDate != null)
                {
                    fromDate = Convert.ToDateTime(startDate.ToString());
                    if (fromDate == DateTime.MinValue)
                    {
                        string customException = "Invalid date format, expected format is yyyy-mm-dd hh:mm:ss";
                        log.Error(customException);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
                    }
                    else
                    {
                        searchParameters.Add(new KeyValuePair<LeaveDTO.SearchByParameters, string>(LeaveDTO.SearchByParameters.START_DATE, fromDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                    }
                }
                if (endDate != null)
                {
                    toDate = Convert.ToDateTime(endDate.ToString());
                    if (toDate == DateTime.MinValue)
                    {
                        string customException = "Invalid date format, expected format is yyyy-mm-dd hh:mm:ss";
                        log.Error(customException);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
                    }
                    else
                    {
                        searchParameters.Add(new KeyValuePair<LeaveDTO.SearchByParameters, string>(LeaveDTO.SearchByParameters.END_DATE, toDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                    }
                }
                if (!string.IsNullOrEmpty(isActive))
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<LeaveDTO.SearchByParameters, string>(LeaveDTO.SearchByParameters.IS_ACTIVE, isActive.ToString()));
                    }
                }
                ILeaveUseCases leaveUseCases = UserUseCaseFactory.GetLeaveUseCases(executionContext);
                List<LeaveDTO> leaveDTOList = await leaveUseCases.GetLeave(searchParameters);
                log.LogMethodExit(leaveDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    data = leaveDTOList,
                });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }

        /// <summary>
        /// Post Method
        /// </summary>
        /// <param name="leaveDTOList"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("api/HR/Leaves")]
        public async Task<HttpResponseMessage> Post([FromBody] List<LeaveDTO> leaveDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(leaveDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (leaveDTOList == null || leaveDTOList.Any(a => a.LeaveId > -1))
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                ILeaveUseCases leaveUseCases = UserUseCaseFactory.GetLeaveUseCases(executionContext);
                List<LeaveDTO> leaveDTOLists = await leaveUseCases.SaveLeave(leaveDTOList);
                log.LogMethodExit(leaveDTOLists);
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    data = leaveDTOLists,
                });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }

        /// <summary>
        /// Post the LeaveList collection
        /// <param name="leaveDTOList">LeaveList</param>
        [HttpPut]
        [Route("api/HR/Leaves")]
        [Authorize]
        public async Task<HttpResponseMessage> Put([FromBody]List<LeaveDTO> leaveDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(leaveDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (leaveDTOList == null || leaveDTOList.Any(a => a.LeaveId < 0))
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                ILeaveUseCases leaveUseCases = UserUseCaseFactory.GetLeaveUseCases(executionContext);
                await leaveUseCases.SaveLeave(leaveDTOList);
                log.LogMethodExit(leaveDTOList);
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
        /// Delete Method
        /// </summary>
        /// <param name="leaveDTOList"></param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize]
        [Route("api/HR/Leaves")]
        public HttpResponseMessage Delete([FromBody] List<LeaveDTO> leaveDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(leaveDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (leaveDTOList != null && leaveDTOList.Count > 0 && leaveDTOList.Any())
                {
                    {
                        ILeaveUseCases leaveUseCases = UserUseCaseFactory.GetLeaveUseCases(executionContext);
                        leaveUseCases.DeleteLeave(leaveDTOList);
                        return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });
                    }
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
