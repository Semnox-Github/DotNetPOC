/********************************************************************************************
 * Project Name - Transactions
 * Description  - API for the Attendance Roles for Users details
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.60        19-Mar-2019   Jagan Mohana Rao          Created 
              08-May-2019   Mushahid Faizan           Added log Method Entry & Exit &
                                                      Declared Global ExecutionContext, SecurityTokenDTO, SecurityTokenBL.
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Semnox.Core.Utilities;
using System.Web;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.User;

namespace Semnox.CommonAPI.SiteSetup
{
    public class AttendanceRoleController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private SecurityTokenDTO securityTokenDTO;
        private SecurityTokenBL securityTokenBL = new SecurityTokenBL();
        /// <summary>
        /// Get the JSON Object InvoiceSequenceSetup List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/SiteSetup/AttendanceRole/")]
        public HttpResponseMessage Get(string userRoleId)
        {
            try
            {
                log.LogMethodEntry(userRoleId);
                this.securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                AttendanceRolesList attendanceRolesList = new AttendanceRolesList(executionContext);
                List<KeyValuePair<AttendanceRoleDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<AttendanceRoleDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<AttendanceRoleDTO.SearchByParameters, string>(AttendanceRoleDTO.SearchByParameters.SITE_ID, Convert.ToString(securityTokenDTO.SiteId)));
                searchParameters.Add(new KeyValuePair<AttendanceRoleDTO.SearchByParameters, string>(AttendanceRoleDTO.SearchByParameters.ROLE_ID, userRoleId ));

                var content = attendanceRolesList.GetAttendanceRoles(searchParameters);
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
        /// Performs a Post operation on AttendanceRoleDTOsList details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/SiteSetup/AttendanceRole/")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<AttendanceRoleDTO> attendanceRoleDTOsList)
        {
            try
            {
                log.LogMethodEntry(attendanceRoleDTOsList);
                this.securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (attendanceRoleDTOsList != null)
                {
                    // if AttendanceRoleDTOsList.id is less than zero then insert or else update
                    AttendanceRolesList attendanceRolesList = new AttendanceRolesList(executionContext, attendanceRoleDTOsList);
                    attendanceRolesList.SaveUpdateAttendanceRolesList();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "", token = securityTokenDTO.Token });
                }
                else
                {
                    log.LogMethodEntry();
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

        /// <summary>
        /// Performs a Delete operation on AttendanceRoleDTOsList details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpDelete]
        [Route("api/SiteSetup/AttendanceRole/")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody] List<AttendanceRoleDTO> attendanceRoleDTOsList)
        {
            try
            {
                log.LogMethodEntry(attendanceRoleDTOsList);
                this.securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (attendanceRoleDTOsList != null)
                {
                    // if AttendanceRoleDTOsList.id is less than zero then insert or else update
                    AttendanceRolesList attendanceRolesList = new AttendanceRolesList(executionContext, attendanceRoleDTOsList);
                    attendanceRolesList.SaveUpdateAttendanceRolesList();
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