/********************************************************************************************
* Project Name - Tools Controller
* Description  - Created to fetch, update and insert monitor master data point
*  
**************
**Version Log
**************
*Version     Date          Modified By               Remarks          
*********************************************************************************************
*2.60        06-May-2019   Jagan Mohana Rao          Created to Get and Post Methods.
*2.80        28-May-2020   Mushahid Faizan           Modified :As per Rest API standard and Renamed controller from MonitorMasterDataController to MonitorPriorityController
********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Linq;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.logger;

namespace Semnox.CommonAPI.Environments
{
    public class MonitorPriorityController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON monitors master data list
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Environment/MonitorPriorities")]
        public HttpResponseMessage Get(string isActive = null, int priorityId = -1, string priorityName = null)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(isActive);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                MonitorPriorityList monitorPriorityList = new MonitorPriorityList(executionContext);
                List<KeyValuePair<MonitorPriorityDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<MonitorPriorityDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<MonitorPriorityDTO.SearchByParameters, string>(MonitorPriorityDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<MonitorPriorityDTO.SearchByParameters, string>(MonitorPriorityDTO.SearchByParameters.ISACTIVE, isActive.ToString()));
                    }
                }
                if (!string.IsNullOrEmpty(priorityName))
                {
                    searchParameters.Add(new KeyValuePair<MonitorPriorityDTO.SearchByParameters, string>(MonitorPriorityDTO.SearchByParameters.PRIORITY_NAME, priorityName.ToString()));
                }
                if (priorityId > -1)
                {
                    searchParameters.Add(new KeyValuePair<MonitorPriorityDTO.SearchByParameters, string>(MonitorPriorityDTO.SearchByParameters.PRIORITY_ID, priorityId.ToString()));
                }
                var content = monitorPriorityList.GetAllMonitorPriorityList(searchParameters);
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
        /// Post the JSON monitors
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>        
        [HttpPost]
        [Route("api/Environment/MonitorPriorities")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<MonitorPriorityDTO> monitorPriorityDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(monitorPriorityDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (monitorPriorityDTOList != null && monitorPriorityDTOList.Any())
                {
                    MonitorPriorityList monitorPriorityList = new MonitorPriorityList(executionContext, monitorPriorityDTOList);
                    monitorPriorityList.Save();
                    log.LogMethodExit(monitorPriorityDTOList);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = monitorPriorityDTOList });
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
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }
    }
}