/********************************************************************************************
* Project Name - Tools Controller
* Description  - Created to fetch, update and insert locker access point
*  
**************
**Version Log
**************
*Version     Date          Modified By               Remarks          
*********************************************************************************************
*2.60        03-May-2019   Jagan Mohana Rao          Created to Get and Post Methods.
*2.90        28-May-2020   Mushahid Faizan           Modified :As per Rest API standard and Renamed controller from MonitorsController to MonitorController
********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.logger;
namespace Semnox.CommonAPI.Environments
{
    public class MonitorController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        ///  Get the JSON monitors list
        /// </summary>
        /// <param name="isActive"></param>
        /// <param name="siteId"></param> Monitor Entity --> Dashboard --> Based on the siteId should fetch the monitor record.
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Environment/Monitors")]
        public async Task<HttpResponseMessage> Get(string isActive = null, int siteId = -1, int monitorId = -1, int priorityId = -1, int assetId = -1, int monitorTypeId = -1, int applicationId = -1, int appModuleId = -1, string monitorName = null,
                                        bool loadActiveChild = false, bool buildChildRecords = false, int currentPage = 0, int pageSize = 5)
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

                List<KeyValuePair<MonitorDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<MonitorDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<MonitorDTO.SearchByParameters, string>(MonitorDTO.SearchByParameters.SITE_ID, siteId == -1 ? executionContext.GetSiteId().ToString() : siteId.ToString()));
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<MonitorDTO.SearchByParameters, string>(MonitorDTO.SearchByParameters.ISACTIVE, isActive));
                    }
                }
                if (applicationId > -1)
                {
                    searchParameters.Add(new KeyValuePair<MonitorDTO.SearchByParameters, string>(MonitorDTO.SearchByParameters.APPLICATION_ID, applicationId.ToString()));
                }
                if (appModuleId > -1)
                {
                    searchParameters.Add(new KeyValuePair<MonitorDTO.SearchByParameters, string>(MonitorDTO.SearchByParameters.APPMODULE_ID, appModuleId.ToString()));
                }
                if (assetId > -1)
                {
                    searchParameters.Add(new KeyValuePair<MonitorDTO.SearchByParameters, string>(MonitorDTO.SearchByParameters.ASSET_ID, assetId.ToString()));
                }
                if (monitorId > -1)
                {
                    searchParameters.Add(new KeyValuePair<MonitorDTO.SearchByParameters, string>(MonitorDTO.SearchByParameters.MONITOR_ID, monitorId.ToString()));
                }
                if (priorityId > -1)
                {
                    searchParameters.Add(new KeyValuePair<MonitorDTO.SearchByParameters, string>(MonitorDTO.SearchByParameters.PRIORITY_ID, priorityId.ToString()));
                }
                if (!string.IsNullOrEmpty(monitorName))
                {
                    searchParameters.Add(new KeyValuePair<MonitorDTO.SearchByParameters, string>(MonitorDTO.SearchByParameters.MONITOR_NAME, monitorName.ToString()));
                }
                if (monitorTypeId > -1)
                {
                    searchParameters.Add(new KeyValuePair<MonitorDTO.SearchByParameters, string>(MonitorDTO.SearchByParameters.MONITOR_TYPE_ID, monitorTypeId.ToString()));
                }
                MonitorList monitorList = new MonitorList(executionContext);

                int totalNoOfPages = 0;
                int totalNoOfMontors = await Task<int>.Factory.StartNew(() => { return monitorList.GetMonitorDTOCount(searchParameters); });
                log.LogVariableState("totalNoOfMontors", totalNoOfMontors);
                totalNoOfPages = (totalNoOfMontors / pageSize) + ((totalNoOfMontors % pageSize) > 0 ? 1 : 0);

                var content = monitorList.GetAllMonitorDTOList(searchParameters, currentPage, pageSize, buildChildRecords, loadActiveChild);
                log.LogMethodExit(content);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content, currentPageNo = currentPage, TotalCount = totalNoOfMontors });
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
        [Route("api/Environment/Monitors")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<MonitorDTO> monitorDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(monitorDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (monitorDTOList != null && monitorDTOList.Any())
                {
                    MonitorList monitorList = new MonitorList(monitorDTOList, executionContext);
                    monitorList.Save();
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = monitorDTOList });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
            }
            catch (ValidationException valEx)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(valEx, executionContext);
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
        /// Delete monitors Record
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpDelete]
        [Route("api/Environment/Monitors")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody] List<MonitorDTO> monitorDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(monitorDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (monitorDTOList != null && monitorDTOList.Any())
                {
                    MonitorList monitorList = new MonitorList(monitorDTOList, executionContext);
                    monitorList.Delete();
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "" });
                }
            }
            catch (ValidationException valEx)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(valEx, executionContext);
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
