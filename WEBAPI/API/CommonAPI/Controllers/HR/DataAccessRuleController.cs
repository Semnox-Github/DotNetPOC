/********************************************************************************************
 * Project Name - Transactions
 * Description  - API for the Data Access  Rules for UserRoles details
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By               Remarks          
 *********************************************************************************************
 *2.60        19-Mar-2019   Jagan Mohana Rao          Created 
              09-Apr-2019   Mushahid Faizan           Modified- Added log Method Entry & Exit &
                                                           declared Global ExecutionContext, SecurityTokenDTO, SecurityTokenBL.
*2.90.0      14-Jun-2020   Girish Kundar             Modified : REST API phase 2 changes/standard 
*2.110.0     21-Nov-2020  Mushahid Faizan           Modified for Service Request Enhancement
*2.120.0     01-Apr-2021   Prajwal S                  Modified.
 ********************************************************************************************/


using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Linq;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.User;
using Semnox.CommonAPI.Helpers;
using System.Threading.Tasks;

namespace Semnox.CommonAPI.HR
{
    public class DataAccessRuleController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext = null;
        /// <summary>
        /// Get the JSON Object UserRolesDataAccessRule List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/HR/DataAccessRules")]
        public async Task<HttpResponseMessage> Get(string isActive = null, int dataAccessRuleId = -1, string name = null, bool loadActiveChild = false, bool buildChildRecords = false )
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(dataAccessRuleId);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                DataAccessRuleList dataAccessRuleList = new DataAccessRuleList(executionContext);
                List<KeyValuePair<DataAccessRuleDTO.SearchByDataAccessRuleParameters, string>> searchParameters = new List<KeyValuePair<DataAccessRuleDTO.SearchByDataAccessRuleParameters, string>>();
                searchParameters.Add(new KeyValuePair<DataAccessRuleDTO.SearchByDataAccessRuleParameters, string>(DataAccessRuleDTO.SearchByDataAccessRuleParameters.SITE_ID, executionContext.GetSiteId().ToString()));

                if (!string.IsNullOrEmpty(name))
                {
                    searchParameters.Add(new KeyValuePair<DataAccessRuleDTO.SearchByDataAccessRuleParameters, string>(DataAccessRuleDTO.SearchByDataAccessRuleParameters.NAME, name));
                }
                if (dataAccessRuleId > -1)
                {
                    searchParameters.Add(new KeyValuePair<DataAccessRuleDTO.SearchByDataAccessRuleParameters, string>(DataAccessRuleDTO.SearchByDataAccessRuleParameters.DATA_ACCESS_RULE_ID, dataAccessRuleId.ToString()));
                }
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<DataAccessRuleDTO.SearchByDataAccessRuleParameters, string>(DataAccessRuleDTO.SearchByDataAccessRuleParameters.ACTIVE_FLAG, isActive));
                    }
                }
                IDataAccessRuleUseCases dataAccessRuleUseCases = UserUseCaseFactory.GetDataAccessRuleUseCases(executionContext);
                List<DataAccessRuleDTO> dataAccessRuleDTOList = await dataAccessRuleUseCases.GetDataAccessRule(searchParameters, buildChildRecords, loadActiveChild);
                log.LogMethodExit(dataAccessRuleDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    data = dataAccessRuleDTOList,
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
        /// Performs a Post operation on DataAccessRuleDTOs details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        // POST: api/Subscriber
        [HttpPost]
        [Route("api/HR/DataAccessRules")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody] List<DataAccessRuleDTO> dataAccessRuleDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(dataAccessRuleDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (dataAccessRuleDTOList == null || dataAccessRuleDTOList.Any(a => a.DataAccessRuleId > -1))
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                IDataAccessRuleUseCases dataAccessRuleUseCases = UserUseCaseFactory.GetDataAccessRuleUseCases(executionContext);
                List<DataAccessRuleDTO> dataAccessRuleDTOLists = await dataAccessRuleUseCases.SaveDataAccessRule(dataAccessRuleDTOList);
                log.LogMethodExit(dataAccessRuleDTOLists);
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    data = dataAccessRuleDTOLists,
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
        /// Post the DataAccessRuleList collection
        /// <param name="dataAccessRuleDTOList">DataAccessRuleList</param>
        [HttpPut]
        [Route("api/HR/DataAccessRules")]
        [Authorize]
        public async Task<HttpResponseMessage> Put([FromBody]List<DataAccessRuleDTO> dataAccessRuleDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(dataAccessRuleDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (dataAccessRuleDTOList == null || dataAccessRuleDTOList.Any(a => a.DataAccessRuleId < 0))
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                IDataAccessRuleUseCases dataAccessRuleUseCases = UserUseCaseFactory.GetDataAccessRuleUseCases(executionContext);
                await dataAccessRuleUseCases.SaveDataAccessRule(dataAccessRuleDTOList);
                log.LogMethodExit(dataAccessRuleDTOList);
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
        /// Performs a Post operation on DataAccessRuleDTOs details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        // POST: api/Subscriber
        [HttpDelete]
        [Route("api/HR/DataAccessRules")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody] List<DataAccessRuleDTO> dataAccessRuleDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(dataAccessRuleDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (dataAccessRuleDTOList != null)
                {
                    IDataAccessRuleUseCases dataAccessRuleUseCases = UserUseCaseFactory.GetDataAccessRuleUseCases(executionContext);
                    dataAccessRuleUseCases.SaveDataAccessRule(dataAccessRuleDTOList);
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
