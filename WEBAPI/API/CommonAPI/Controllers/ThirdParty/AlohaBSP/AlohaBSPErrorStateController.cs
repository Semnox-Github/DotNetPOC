﻿/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - API for ExSysSynchLog
 * 
 **************
 **Version Log
 **************
 *Version      Date           Modified By     Remarks          
 *********************************************************************************************
 *2.160.0      07-Feb-2022    Deeksha        Created
 ********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.JobUtils;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Site;
using Semnox.Parafait.Transaction;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Semnox.CommonAPI.Jobs
{
    public class AlohaBSPErrorStateController : ApiController
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
        [Route("api/ThirdParty/AlohaBSP/Status")]
        [Authorize]
        public HttpResponseMessage Get(int parafaitObjectId = -1, DateTime? fromDate = null, DateTime? toDate = null,
                                       string errorStatusList = null, int siteCode = -1, string bSPID = null)
        {
            try
            {
                log.LogMethodEntry(parafaitObjectId, fromDate, toDate);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                List<AlohaBSPErrorStateViewDTO> result = new List<AlohaBSPErrorStateViewDTO>();
                List<KeyValuePair<ExSysSynchLogDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ExSysSynchLogDTO.SearchByParameters, string>>();
                if (string.IsNullOrWhiteSpace(errorStatusList) == false)
                {
                    searchParameters.Add(new KeyValuePair<ExSysSynchLogDTO.SearchByParameters, string>(ExSysSynchLogDTO.SearchByParameters.UNIQUE_OBJECT_STATUS_LIST, errorStatusList));
                }
                else
                {
                    searchParameters.Add(new KeyValuePair<ExSysSynchLogDTO.SearchByParameters, string>(ExSysSynchLogDTO.SearchByParameters.UNIQUE_OBJECT_STATUS_LIST, "TE,Error,PE,P"));
                }
                if (parafaitObjectId > -1)
                {
                    searchParameters.Add(new KeyValuePair<ExSysSynchLogDTO.SearchByParameters, string>(ExSysSynchLogDTO.SearchByParameters.PARAFAIT_OBJECT_ID, parafaitObjectId.ToString()));
                }
                if (string.IsNullOrWhiteSpace(bSPID) == false)
                {
                    searchParameters.Add(new KeyValuePair<ExSysSynchLogDTO.SearchByParameters, string>(ExSysSynchLogDTO.SearchByParameters.BSP_ID, bSPID));
                }
                if (siteCode != -1)
                {
                    int site_Id = -1;
                    SiteList siteListBL = new SiteList(executionContext);
                    List<KeyValuePair<SiteDTO.SearchBySiteParameters, string>> siteSerachParams = new List<KeyValuePair<SiteDTO.SearchBySiteParameters, string>>();
                    siteSerachParams.Add(new KeyValuePair<SiteDTO.SearchBySiteParameters, string>(SiteDTO.SearchBySiteParameters.SITE_CODE, siteCode.ToString()));
                    List<SiteDTO> siteDTOList = siteListBL.GetAllSites(siteSerachParams);
                    if (siteDTOList != null && siteDTOList.Any())
                    {
                        site_Id = siteDTOList[0].SiteId;
                        searchParameters.Add(new KeyValuePair<ExSysSynchLogDTO.SearchByParameters, string>(ExSysSynchLogDTO.SearchByParameters.SITE_ID, site_Id.ToString()));
                    }
                    else
                    {
                        string customException = "Please enter valid site code";
                        log.Error(customException);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
                    }
                }
                int noOfDaysToBeProcessed = 10;
                if (ConfigurationManager.AppSettings.AllKeys.Contains("NoOfDaysForAlohaBSPProcessing")
                    && !string.IsNullOrEmpty(ConfigurationManager.AppSettings["NoOfDaysForAlohaBSPProcessing"]))
                    noOfDaysToBeProcessed = Convert.ToInt32(ConfigurationManager.AppSettings["NoOfDaysForAlohaBSPProcessing"]);
                DateTime daysToBeProcessed = ServerDateTime.Now.AddDays(-noOfDaysToBeProcessed);
                if (fromDate != null)
                {
                    DateTime startDate = Convert.ToDateTime(fromDate.ToString());
                    if (startDate < daysToBeProcessed)
                    {
                        string customException = MessageContainerList.GetMessage(executionContext, 5532);  //"The entered date range exceeds the allowed processing duration of NoOfDaysForAlohaBSPProcessing";
                        log.Error(customException);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
                    }
                    searchParameters.Add(new KeyValuePair<ExSysSynchLogDTO.SearchByParameters, string>(ExSysSynchLogDTO.SearchByParameters.TRX_FROM_DATE, startDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                }
                if (toDate != null)
                {
                    DateTime endDate = Convert.ToDateTime(toDate.ToString());
                    searchParameters.Add(new KeyValuePair<ExSysSynchLogDTO.SearchByParameters, string>(ExSysSynchLogDTO.SearchByParameters.TRX_TO_DATE, endDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                }
                searchParameters.Add(new KeyValuePair<ExSysSynchLogDTO.SearchByParameters, string>(ExSysSynchLogDTO.SearchByParameters.EX_SYSTEM_NAME, "Aloha"));
                searchParameters.Add(new KeyValuePair<ExSysSynchLogDTO.SearchByParameters, string>(ExSysSynchLogDTO.SearchByParameters.NOT_IN_SUCCESS_STATE, ""));
                //searchParameters.Add(new KeyValuePair<ExSysSynchLogDTO.SearchByParameters, string>(ExSysSynchLogDTO.SearchByParameters.STATUS_NOT_IN_SUCCESS, ""));
                AlohaBSPErrorStateViewBL exSysSynchLogViewListBL = new AlohaBSPErrorStateViewBL(executionContext);
                result = exSysSynchLogViewListBL.GetExSysSynchLogViewDTOList(searchParameters);
                if (result != null && result.Any())
                {
                    if (result.Exists(x => x.Timestamp < daysToBeProcessed))
                    {
                        string customException = MessageContainerList.GetMessage(executionContext, 5533);//"The resulted transaction date range exceeds the allowed processing duration of NoOfDaysForAlohaBSPProcessing";
                        log.Error(customException);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
                    }
                }
                log.LogMethodExit(result);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = result });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = ex.Message });
            }
        }

        /// <summary>
        /// Performs a Post operation on ExSysSynchLog Error States
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        // POST: api/Subscriber
        [HttpPost]
        [Route("api/ThirdParty/AlohaBSP/Status")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<int> exsysSynchLogIDList, string status)
        {
            try
            {
                log.LogMethodEntry(exsysSynchLogIDList);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                AlohaBSPErrorStateViewBL exSysSynchLogListBL = new AlohaBSPErrorStateViewBL(executionContext);
                exSysSynchLogListBL.UpdateExsysSynchLogRequestIDAndStatus(exsysSynchLogIDList, status);
                return Request.CreateResponse(HttpStatusCode.OK, new { string.Empty });
            }
            catch (ValidationException ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = ex.Message });
            }
        }
    }
}