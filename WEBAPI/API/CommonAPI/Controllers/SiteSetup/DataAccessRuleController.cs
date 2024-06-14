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
    public class DataAccessRuleController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private SecurityTokenDTO securityTokenDTO;
        private SecurityTokenBL securityTokenBL = new SecurityTokenBL();
        /// <summary>
        /// Get the JSON Object UserRolesDataAccessRule List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/SiteSetup/DataAccessRule/")]
        public HttpResponseMessage Get(string isActive)
        {
            try
            {
                log.LogMethodEntry(isActive);
                this.securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                DataAccessRuleList dataAccessRuleList = new DataAccessRuleList(executionContext);
                List<KeyValuePair<DataAccessRuleDTO.SearchByDataAccessRuleParameters, string>> searchParameters = new List<KeyValuePair<DataAccessRuleDTO.SearchByDataAccessRuleParameters, string>>();
                searchParameters.Add(new KeyValuePair<DataAccessRuleDTO.SearchByDataAccessRuleParameters, string>(DataAccessRuleDTO.SearchByDataAccessRuleParameters.SITE_ID, Convert.ToString(securityTokenDTO.SiteId)));
                bool loadActiveChildRecords = false;
                if (isActive == "1")
                {
                    loadActiveChildRecords = true;
                    searchParameters.Add(new KeyValuePair<DataAccessRuleDTO.SearchByDataAccessRuleParameters, string>(DataAccessRuleDTO.SearchByDataAccessRuleParameters.ACTIVE_FLAG, isActive));
                }
                var content = dataAccessRuleList.GetAllDataAccessRule(searchParameters, loadActiveChildRecords);
                
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
        /// Performs a Post operation on DataAccessRuleDTOs details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        // POST: api/Subscriber
        [HttpPost]
        [Route("api/SiteSetup/DataAccessRule/")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<DataAccessRuleDTO> dataAccessRuleDTOList)
        {
            try
            {
                log.LogMethodEntry(dataAccessRuleDTOList);
                this.securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId)); ;
                if (dataAccessRuleDTOList != null)
                {
                    // if dataAccessRuleDTOs.dataAccessRuleId is less than zero then insert or else update
                    DataAccessRuleList dataAccessRuleList = new DataAccessRuleList(executionContext, dataAccessRuleDTOList);
                    dataAccessRuleList.SaveUpdateDataAccessRuleList();
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
        /// Performs a Post operation on DataAccessRuleDTOs details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        // POST: api/Subscriber
        [HttpDelete]
        [Route("api/SiteSetup/DataAccessRule/")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody] List<DataAccessRuleDTO> dataAccessRuleDTOList)
        {
            try
            {
                log.LogMethodEntry(dataAccessRuleDTOList);
                this.securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId)); ;
                if (dataAccessRuleDTOList != null)
                {
                    // if dataAccessRuleDTOs.dataAccessRuleId is less than zero then insert or else update
                    DataAccessRuleList dataAccessRuleList = new DataAccessRuleList(executionContext, dataAccessRuleDTOList);
                    dataAccessRuleList.SaveUpdateDataAccessRuleList();
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
