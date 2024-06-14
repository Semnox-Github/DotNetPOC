/********************************************************************************************
* Project Name - CommnonAPI - HR Module 
* Description  - API for the Department.
* 
**************
**Version Log
**************
*Version     Date            Modified By         Remarks          
*********************************************************************************************
*2.80.0      19-Oct-2019     Indrajeet Kumar     Created
*2.90        20-May-2020     Vikas Dwivedi       Modified as per the Standard CheckList
*2.120.0     01-Apr-2021     Prajwal S                  Modified.
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
    public class DepartmentController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object of Department List
        /// This method will fetch all the Department record
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/HR/Departments")]
        public async Task<HttpResponseMessage> Get(int departmentId = -1, string departmentName = null, string isActive = null)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(departmentId);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<DepartmentDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<DepartmentDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<DepartmentDTO.SearchByParameters, string>(DepartmentDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (departmentId > -1)
                {
                    searchParameters.Add(new KeyValuePair<DepartmentDTO.SearchByParameters, string>(DepartmentDTO.SearchByParameters.DEPARTMENT_ID, departmentId.ToString()));
                }
                if (!string.IsNullOrEmpty(departmentName))
                {
                    searchParameters.Add(new KeyValuePair<DepartmentDTO.SearchByParameters, string>(DepartmentDTO.SearchByParameters.DEPARTMENT_NAME, departmentName.ToString()));
                }
                if (!string.IsNullOrEmpty(isActive))
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<DepartmentDTO.SearchByParameters, string>(DepartmentDTO.SearchByParameters.ISACTIVE, isActive.ToString()));
                    }
                }
                IDepartmentUseCases departmentUseCases = UserUseCaseFactory.GetDepartmentUseCases(executionContext);
                List<DepartmentDTO> departmentDTOList = await departmentUseCases.GetDepartments(searchParameters);
                log.LogMethodExit(departmentDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    data = departmentDTOList,
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
        /// Performs a Post operation on Department Details
        /// </summary>
        /// <param name="departmentDTOList"></param>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Authorize]
        [Route("api/HR/Departments")]
        public async Task<HttpResponseMessage> Post([FromBody] List<DepartmentDTO> departmentDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(departmentDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (departmentDTOList == null || departmentDTOList.Any(a => a.DepartmentId > -1))
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                IDepartmentUseCases departmentUseCases = UserUseCaseFactory.GetDepartmentUseCases(executionContext);
                await departmentUseCases.SaveDepartments(departmentDTOList);
                log.LogMethodExit(departmentDTOList);
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
        /// Post the DepartmentList collection
        /// <param name="departmentDTOList">DepartmentList</param>
        [HttpPut]
        [Route("api/HR/Departments")]
        [Authorize]
        public async Task<HttpResponseMessage> Put([FromBody]List<DepartmentDTO> departmentDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(departmentDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (departmentDTOList == null || departmentDTOList.Any(a => a.DepartmentId < 0))
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                IDepartmentUseCases departmentUseCases = UserUseCaseFactory.GetDepartmentUseCases(executionContext);
                await departmentUseCases.SaveDepartments(departmentDTOList);
                log.LogMethodExit(departmentDTOList);
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
        /// Delete the JSON Department
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>        
        [HttpDelete]
        [Route("api/HR/Departments")]
        [Authorize]
        public async Task<HttpResponseMessage> Delete([FromBody] List<DepartmentDTO> departmentDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(departmentDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (departmentDTOList != null && departmentDTOList.Any())
                {
                    IDepartmentUseCases departmentUseCases = UserUseCaseFactory.GetDepartmentUseCases(executionContext);
                    await departmentUseCases.Delete(departmentDTOList);
                    log.LogMethodExit(departmentDTOList);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = string.Empty });
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
