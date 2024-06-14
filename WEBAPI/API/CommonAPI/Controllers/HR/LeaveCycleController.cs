/********************************************************************************************
* Project Name - CommnonAPI - HR Module 
* Description  - API for the Holiday.
* 
**************
**Version Log
**************
*Version     Date            Modified By         Remarks          
*********************************************************************************************
*2.80.0      19-Oct-2019     Indrajeet Kumar     Created
*2.90        20-May-2020     Vikas Dwivedi       Modified as per the Standard CheckList
*2.120.0     01-Apr-2021     Prajwal S           Modified.
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
    public class LeaveCycleController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get Method
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("api/HR/LeaveCycles")]
        public async Task<HttpResponseMessage> Get(int leaveCycleId = -1, string name = null, string isActive = null)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(leaveCycleId, name);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<LeaveCycleDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<LeaveCycleDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<LeaveCycleDTO.SearchByParameters, string>(LeaveCycleDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (leaveCycleId > -1)
                {
                    searchParameters.Add(new KeyValuePair<LeaveCycleDTO.SearchByParameters, string>(LeaveCycleDTO.SearchByParameters.LEAVE_CYCLE_ID, leaveCycleId.ToString()));
                }
                if (!string.IsNullOrEmpty(name))
                {
                    searchParameters.Add(new KeyValuePair<LeaveCycleDTO.SearchByParameters, string>(LeaveCycleDTO.SearchByParameters.NAME, name.ToString()));
                }
                if (!string.IsNullOrEmpty(isActive))
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<LeaveCycleDTO.SearchByParameters, string>(LeaveCycleDTO.SearchByParameters.IS_ACTIVE, isActive.ToString()));
                    }
                }
                ILeaveCycleUseCases attendanceRoleUseCases = UserUseCaseFactory.GetLeaveCycleUseCases(executionContext);
                List<LeaveCycleDTO> attendanceRoleDTOList = await attendanceRoleUseCases.GetLeaveCycle(searchParameters);
                log.LogMethodExit(attendanceRoleDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    data = attendanceRoleDTOList,
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
        /// <param name="leaveCycleDTOList"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("api/HR/LeaveCycles")]
        public async Task<HttpResponseMessage> Post([FromBody] List<LeaveCycleDTO> leaveCycleDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(leaveCycleDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (leaveCycleDTOList == null || leaveCycleDTOList.Any(a => a.LeaveCycleId > -1))
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                ILeaveCycleUseCases leaveCycleUseCases = UserUseCaseFactory.GetLeaveCycleUseCases(executionContext);
                List<LeaveCycleDTO> leaveCycleDTOLists = await leaveCycleUseCases.SaveLeaveCycle(leaveCycleDTOList);
                log.LogMethodExit(leaveCycleDTOLists);
                return Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        data = leaveCycleDTOLists,
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
        /// Post the LeaveCycleList collection
        /// <param name="leaveCycleDTOList">LeaveCycleList</param>
        [HttpPut]
        [Route("api/HR/LeaveCycles")]
        [Authorize]
        public async Task<HttpResponseMessage> Put([FromBody]List<LeaveCycleDTO> leaveCycleDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(leaveCycleDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (leaveCycleDTOList == null || leaveCycleDTOList.Any(a => a.LeaveCycleId < 0))
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                ILeaveCycleUseCases leaveCycleUseCases = UserUseCaseFactory.GetLeaveCycleUseCases(executionContext);
                await leaveCycleUseCases.SaveLeaveCycle(leaveCycleDTOList);
                log.LogMethodExit(leaveCycleDTOList);
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
        /// <param name="leaveCycleDTOList"></param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize]
        [Route("api/HR/LeaveCycles")]
        public HttpResponseMessage Delete([FromBody] List<LeaveCycleDTO> leaveCycleDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(leaveCycleDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (leaveCycleDTOList != null && leaveCycleDTOList.Any())
                {
                    ILeaveCycleUseCases leaveCycleUseCases = UserUseCaseFactory.GetLeaveCycleUseCases(executionContext);
                    leaveCycleUseCases.DeleteLeaveCycle(leaveCycleDTOList);
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
