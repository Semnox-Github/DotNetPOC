/********************************************************************************************
* Project Name - CommnonAPI - HR Module 
* Description  - API for the Holiday.
* 
**************
**Version Log
**************
*Version     Date            Modified By         Remarks          
*********************************************************************************************
*2.80.0      15-Oct-2019     Indrajeet Kumar     Created
*2.90        20-May-2020     Vikas Dwivedi       Modified as per the Standard CheckList
2.120.0      01-Apr-2021      Prajwal S          Modified.
********************************************************************************************/

using System;
using System.Collections.Generic;
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
    public class LeaveTemplateController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get Method
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("api/HR/LeaveTemplates")]
        public async Task<HttpResponseMessage> Get(int leaveTemplateId = -1, int departmentId = -1, int roleId = -1, int leaveTypeId = -1, string isActive = null)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(leaveTemplateId, departmentId, roleId, leaveTypeId, isActive);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<LeaveTemplateDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<LeaveTemplateDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<LeaveTemplateDTO.SearchByParameters, string>(LeaveTemplateDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (leaveTemplateId > -1)
                {
                    searchParameters.Add(new KeyValuePair<LeaveTemplateDTO.SearchByParameters, string>(LeaveTemplateDTO.SearchByParameters.LEAVE_TEMPLATE_ID, leaveTemplateId.ToString()));
                }
                if (departmentId > -1)
                {
                    searchParameters.Add(new KeyValuePair<LeaveTemplateDTO.SearchByParameters, string>(LeaveTemplateDTO.SearchByParameters.DEPARTMENT_ID, departmentId.ToString()));
                }
                if (roleId > -1)
                {
                    searchParameters.Add(new KeyValuePair<LeaveTemplateDTO.SearchByParameters, string>(LeaveTemplateDTO.SearchByParameters.ROLE_ID, roleId.ToString()));
                }
                if (leaveTypeId > -1)
                {
                    searchParameters.Add(new KeyValuePair<LeaveTemplateDTO.SearchByParameters, string>(LeaveTemplateDTO.SearchByParameters.LEAVE_TYPE_ID, leaveTypeId.ToString()));
                }
                if (!string.IsNullOrEmpty(isActive))
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<LeaveTemplateDTO.SearchByParameters, string>(LeaveTemplateDTO.SearchByParameters.IS_ACTIVE, isActive.ToString()));
                    }
                }
                ILeaveTemplateUseCases leaveTemplateUseCases = UserUseCaseFactory.GetLeaveTemplateUseCases(executionContext);
                List<LeaveTemplateDTO> leaveTemplateDTOList = await leaveTemplateUseCases.GetLeaveTemplate(searchParameters);
                log.LogMethodExit(leaveTemplateDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    data = leaveTemplateDTOList,
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
        /// <param name="leaveTemplateDTOList"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("api/HR/LeaveTemplates")]
        public async Task<HttpResponseMessage> Post([FromBody] List<LeaveTemplateDTO> leaveTemplateDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(leaveTemplateDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (leaveTemplateDTOList == null || leaveTemplateDTOList.Any(a => a.LeaveTemplateId > -1))
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                ILeaveTemplateUseCases attendanceRoleUseCases = UserUseCaseFactory.GetLeaveTemplateUseCases(executionContext);
                List<LeaveTemplateDTO> attendanceRoleDTOLists = await attendanceRoleUseCases.SaveLeaveTemplate(leaveTemplateDTOList);
                log.LogMethodExit(attendanceRoleDTOLists);
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    data = attendanceRoleDTOLists,
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
        /// Post the LeaveTemplateList collection
        /// <param name="leaveTemplateDTOList">LeaveTemplateList</param>
        [HttpPut]
        [Route("api/HR/LeaveTemplates")]
        [Authorize]
        public async Task<HttpResponseMessage> Put([FromBody]List<LeaveTemplateDTO> leaveTemplateDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(leaveTemplateDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (leaveTemplateDTOList == null || leaveTemplateDTOList.Any(a => a.LeaveTemplateId < 0))
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                ILeaveTemplateUseCases leaveTemplateUseCases = UserUseCaseFactory.GetLeaveTemplateUseCases(executionContext);
                await leaveTemplateUseCases.SaveLeaveTemplate(leaveTemplateDTOList);
                log.LogMethodExit(leaveTemplateDTOList);
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
        /// <param name="leaveTemplateDTOList"></param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize]
        [Route("api/HR/LeaveTemplates")]
        public HttpResponseMessage Delete([FromBody] List<LeaveTemplateDTO> leaveTemplateDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(leaveTemplateDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (leaveTemplateDTOList != null && leaveTemplateDTOList.Any())
                {

                    ILeaveTemplateUseCases leaveTemplateUseCases = UserUseCaseFactory.GetLeaveTemplateUseCases(executionContext);
                    leaveTemplateUseCases.DeleteLeaveTemplate(leaveTemplateDTOList);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });
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
