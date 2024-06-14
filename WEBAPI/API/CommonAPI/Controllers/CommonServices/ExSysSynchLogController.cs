/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - API for ExSysSynchLog
 * 
 **************
 **Version Log
 **************
 *Version     Date           Modified By     Remarks          
 *********************************************************************************************
 *2.80.5      15-Oct-2021    Girish Kundar     Created
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.JobUtils;
using System.Linq;
using Semnox.Core.Utilities;
using System.Globalization;

namespace Semnox.CommonAPI.CommonServices
{
    public class ExSysSynchLogController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly SecurityTokenBL securityTokenBL = new SecurityTokenBL();
        private SecurityTokenDTO securityTokenDTO;
        private ExecutionContext executionContext;

        /// <summary>
        /// Get the JSON Object Themes and Screen transitions List Details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>        
        [HttpGet]
        [Route("api/CommonServices/ExSysSynchLogs")]
        [Authorize]
        public HttpResponseMessage Get(bool isSuccessful= false, string exSystemName = null,
                                       string parafaitObject = null,
                                       int parafaitObjectId = -1, string parafaitObjectGuid = null,
                                       int siteId =-1,
                                       string status = null,DateTime? fromTime = null,
                                       DateTime? toTime = null ,
                                       string parafaitObjectIdList = null,
                                       string alohaOrderStatus = null)
        {
            try
            {
                log.LogMethodEntry(isSuccessful, exSystemName, parafaitObject ,parafaitObjectId ,
                                                parafaitObjectGuid , status , fromTime , toTime,
                                                parafaitObjectIdList, alohaOrderStatus);
                log.Info("GET Response Entry for " + parafaitObjectGuid + "aloha order status-" + alohaOrderStatus);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                int contextSiteId = (siteId != -1 ? siteId : securityTokenDTO.SiteId);
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, contextSiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                List<KeyValuePair<ExSysSynchLogDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ExSysSynchLogDTO.SearchByParameters, string>>();
                if (isSuccessful)
                {
                        searchParameters.Add(new KeyValuePair<ExSysSynchLogDTO.SearchByParameters, string>(ExSysSynchLogDTO.SearchByParameters.IS_SUCCESSFUL, "1"));
                }
                if (string.IsNullOrEmpty(exSystemName) == false)
                {
                    searchParameters.Add(new KeyValuePair<ExSysSynchLogDTO.SearchByParameters, string>(ExSysSynchLogDTO.SearchByParameters.EX_SYSTEM_NAME, exSystemName));
                }
                if (string.IsNullOrEmpty(parafaitObject) == false)
                {
                    searchParameters.Add(new KeyValuePair<ExSysSynchLogDTO.SearchByParameters, string>(ExSysSynchLogDTO.SearchByParameters.PARAFAIT_OBJECT, parafaitObject));
                }
                if (string.IsNullOrEmpty(parafaitObjectGuid) == false)
                {
                    searchParameters.Add(new KeyValuePair<ExSysSynchLogDTO.SearchByParameters, string>(ExSysSynchLogDTO.SearchByParameters.PARAFAIT_OBJECT_GUID, parafaitObjectGuid));
                }
                if (string.IsNullOrEmpty(status) == false)
                {
                    searchParameters.Add(new KeyValuePair<ExSysSynchLogDTO.SearchByParameters, string>(ExSysSynchLogDTO.SearchByParameters.STATUS, status));
                }
                if (parafaitObjectId > -1)
                {
                    searchParameters.Add(new KeyValuePair<ExSysSynchLogDTO.SearchByParameters, string>(ExSysSynchLogDTO.SearchByParameters.PARAFAIT_OBJECT_ID, parafaitObjectId.ToString()));
                }
                if (siteId > -1)
                {
                    searchParameters.Add(new KeyValuePair<ExSysSynchLogDTO.SearchByParameters, string>(ExSysSynchLogDTO.SearchByParameters.SITE_ID, siteId.ToString()));
                }
                else
                {
                    searchParameters.Add(new KeyValuePair<ExSysSynchLogDTO.SearchByParameters, string>(ExSysSynchLogDTO.SearchByParameters.SITE_ID, executionContext.GetIsCorporate() ? executionContext.GetSiteId().ToString() : "-1"));
                }
                if (string.IsNullOrWhiteSpace(parafaitObjectIdList ) == false)
                {
                    searchParameters.Add(new KeyValuePair<ExSysSynchLogDTO.SearchByParameters, string>(ExSysSynchLogDTO.SearchByParameters.PARAFAIT_OBJECT_ID_LIST, parafaitObjectIdList.ToString()));
                }
                DateTime startDate = ServerDateTime.Now;
                DateTime endDate = ServerDateTime.Now.AddDays(1);

                if (fromTime != null)
                {
                    startDate = Convert.ToDateTime(fromTime.ToString());
                    if (startDate == DateTime.MinValue)
                    {
                        string customException = "Invalid date format, expected format is yyyy-mm-dd hh:mm:ss";
                        log.Info("GET Response Invalid date format for " + parafaitObjectGuid + "aloha order status-" + alohaOrderStatus);
                        log.Error(customException);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
                    }
                }

                if (toTime != null)
                {
                    endDate = Convert.ToDateTime(toTime.ToString());
                    if (endDate == DateTime.MinValue)
                    {
                        string customException = "Invalid date format, expected format is yyyy-mm-dd hh:mm:ss";
                        log.Error(customException);
                        log.Info("GET Response Invalid date format for " + parafaitObjectGuid + "aloha order status-" + alohaOrderStatus);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
                    }
                }
                else
                {
                    endDate = ServerDateTime.Now;
                }

                if (fromTime != null || toTime != null)
                {
                    searchParameters.Add(new KeyValuePair<ExSysSynchLogDTO.SearchByParameters, string>(ExSysSynchLogDTO.SearchByParameters.TIMESTAMP_FROM, startDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                    searchParameters.Add(new KeyValuePair<ExSysSynchLogDTO.SearchByParameters, string>(ExSysSynchLogDTO.SearchByParameters.TIMESTAMP_TO, endDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                }

                ExSysSynchLogListBL exSysSynchLogListBL = new ExSysSynchLogListBL(executionContext);
                List<ExSysSynchLogDTO> exSysSynchLogDTOList = exSysSynchLogListBL.GetExSysSynchLogDTOList(searchParameters);
                log.LogMethodExit(exSysSynchLogDTOList);
                log.Info("GET Response Success for " + parafaitObjectGuid + "aloha order status-" + alohaOrderStatus);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = exSysSynchLogDTOList });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.Info("GET Response InternalServerError for " + parafaitObjectGuid + "aloha order status-" + alohaOrderStatus);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = ex.Message   });
            }
        }

        /// <summary>
        /// Performs a Post operation on ExSysSynchLogs
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        // POST: api/Subscriber
        [HttpPost]
        [Route("api/CommonServices/ExSysSynchLogs")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<ExSysSynchLogDTO> exSysSynchLogDTOList,
                                            [FromUri]int siteId, [FromUri]string alohaOrderStatus,
                                            [FromUri]string objectGuid)
        {
            try
            {
                log.LogMethodEntry(exSysSynchLogDTOList, alohaOrderStatus, objectGuid, siteId,
                                        alohaOrderStatus, objectGuid);
                log.Info("POST Response Entry for " + objectGuid + "aloha order status-" + alohaOrderStatus);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                int contextSiteId = (siteId != -1 ? siteId : securityTokenDTO.SiteId);
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, contextSiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (exSysSynchLogDTOList != null && exSysSynchLogDTOList.Any())
                {
                    ExSysSynchLogListBL exSysSynchLogListBL = new ExSysSynchLogListBL(executionContext, exSysSynchLogDTOList);
                    exSysSynchLogListBL.Save(null);
                    log.Info("POST Response Success for " + objectGuid + "aloha order status-" + alohaOrderStatus);
                    return Request.CreateResponse(HttpStatusCode.OK, new { exSysSynchLogDTOList });
                }
                else
                {
                    log.Info("POST Response BadRequest for " + objectGuid + "aloha order status-" + alohaOrderStatus);
                    log.Debug("ExSysSynchLogs-Post() Method.");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ""   });
                }
            }
            catch (ValidationException ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                    log.Info("POST Response BadRequest for " + objectGuid + "aloha order status-" + alohaOrderStatus);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                    log.Info("POST Response InternalServerError for " + objectGuid + "aloha order status-" + alohaOrderStatus);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = ex.Message   });
            }
        }
    }
}