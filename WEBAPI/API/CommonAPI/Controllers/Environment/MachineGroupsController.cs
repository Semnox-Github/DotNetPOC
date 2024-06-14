/******************************************************************************************
 * Project Name - Tools Controller
 * Description  - Created to fetch, update and insert machine groups
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By               Remarks          
 *********************************************************************************************
 *2.60        13-May-2019   Jagan Mohana Rao          Created to Get and Post Methods.
 ********************************************************************************************/
using Semnox.Parafait.Game;
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Semnox.Core.GenericUtilities;

namespace Semnox.CommonAPI.Tools
{
    public class MachineGroupsController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON MachineGroups
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Tools/MachineGroups/")]
        public HttpResponseMessage Get(string isActive)
        {

            log.LogMethodEntry(isActive);
            SecurityTokenBL securityTokenBL = new SecurityTokenBL();
            securityTokenBL.GenerateJWTToken();
            SecurityTokenDTO securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
            ExecutionContext executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
            try
            {
                MachineGroupsList machineGroupsList = new MachineGroupsList(executionContext);
                List<KeyValuePair<MachineGroupsDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<MachineGroupsDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<MachineGroupsDTO.SearchByParameters, string>(MachineGroupsDTO.SearchByParameters.SITE_ID, Convert.ToString(securityTokenDTO.SiteId)));
                bool activeChildRecords = false;
                if (isActive == "1")
                {
                    activeChildRecords = true;
                    searchParameters.Add(new KeyValuePair<MachineGroupsDTO.SearchByParameters, string>(MachineGroupsDTO.SearchByParameters.ISACTIVE, isActive));
                }
                var content = machineGroupsList.GetAllMachineGroupsDTOList(searchParameters, true, activeChildRecords);
                log.LogMethodExit(content);
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
        /// Post the JSON locker panels
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>        
        [HttpPost]
        [Route("api/Tools/MachineGroups/")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<MachineGroupsDTO> machineGroupsDTOList)
        {
            log.LogMethodEntry(machineGroupsDTOList);
            SecurityTokenBL securityTokenBL = new SecurityTokenBL();
            securityTokenBL.GenerateJWTToken();
            SecurityTokenDTO securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
            ExecutionContext executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
            try
            {
                if (machineGroupsDTOList != null || machineGroupsDTOList.Count > 0)
                {
                    MachineGroupsList machineGroupsList = new MachineGroupsList(executionContext, machineGroupsDTOList);
                    machineGroupsList.SaveUpdateMachineGroups();
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "" });
                }
            }
            catch (ValidationException vexp)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(vexp, executionContext);
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
        /// Delete locker panels Record
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpDelete]
        [Route("api/Tools/MachineGroups/")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody] List<MachineGroupsDTO> machineGroupsDTOList)
        {

            log.LogMethodEntry(machineGroupsDTOList);
            SecurityTokenBL securityTokenBL = new SecurityTokenBL();
            securityTokenBL.GenerateJWTToken();
            SecurityTokenDTO securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
            ExecutionContext executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
            try
            {
                if (machineGroupsDTOList != null || machineGroupsDTOList.Count > 0)
                {
                    MachineGroupsList machineGroupsList = new MachineGroupsList(executionContext, machineGroupsDTOList);
                    machineGroupsList.SaveUpdateMachineGroups();
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { data = "" });
                }
            }
            catch (ValidationException vexp)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(vexp, executionContext);
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
