/********************************************************************************************
 * Project Name - ExsysSynchLog COunt Controller
 * Description  - Controller for ExsysSynchLog Resource
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.160.0    06-Jan-2023      Deeksha         Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Semnox.Core.Utilities;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.JobUtils;
using Semnox.Parafait.Site;
using System.Linq;
using Semnox.Parafait.ThirdParty;
using Semnox.Parafait.Transaction;
using System.Configuration;
using Semnox.Parafait.Languages;

namespace Semnox.CommonAPI.Jobs
{
    public class AlohaBSPErrorStateCountController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly SecurityTokenBL securityTokenBL = new SecurityTokenBL();
        private SecurityTokenDTO securityTokenDTO;
        private ExecutionContext executionContext;

        /// <summary>   
        /// Get the ExsysSynchLog Count JSON.
        /// </summary>       
        [HttpGet]
        [Route("api/ThirdParty/AlohaBSP/StatusCount")]
        [Authorize]
        public HttpResponseMessage Get(int parafaitObjectId = -1, DateTime? fromDate = null, DateTime? toDate = null,
                                       string errorStatusList = null, int siteCode = -1, string bSPID = null)
        {
            try
            {
                log.LogMethodEntry(parafaitObjectId, fromDate, toDate);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                int toralNoOfLogs = 0;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                List<KeyValuePair<ExSysSynchLogDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ExSysSynchLogDTO.SearchByParameters, string>>();
                if (parafaitObjectId > -1)
                {
                    searchParameters.Add(new KeyValuePair<ExSysSynchLogDTO.SearchByParameters, string>(ExSysSynchLogDTO.SearchByParameters.PARAFAIT_OBJECT_ID, parafaitObjectId.ToString()));
                }
                if (string.IsNullOrWhiteSpace(errorStatusList) == false)
                {
                    searchParameters.Add(new KeyValuePair<ExSysSynchLogDTO.SearchByParameters, string>(ExSysSynchLogDTO.SearchByParameters.UNIQUE_OBJECT_STATUS_LIST, errorStatusList));
                }
                else
                {
                    searchParameters.Add(new KeyValuePair<ExSysSynchLogDTO.SearchByParameters, string>(ExSysSynchLogDTO.SearchByParameters.UNIQUE_OBJECT_STATUS_LIST, "TE,Error,PE,P"));
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
                log.Debug("noOfDaysToBeProcessed" + noOfDaysToBeProcessed);
                if (fromDate != null)
                {
                    DateTime startDate = Convert.ToDateTime(fromDate.ToString());
                    DateTime daysToBeProcessed = ServerDateTime.Now.AddDays(-noOfDaysToBeProcessed);
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
                // searchParameters.Add(new KeyValuePair<ExSysSynchLogDTO.SearchByParameters, string>(ExSysSynchLogDTO.SearchByParameters.STATUS_NOT_IN_SUCCESS, ""));
                AlohaBSPErrorStateViewBL exSysSynchLogListBL = new AlohaBSPErrorStateViewBL(executionContext);
                toralNoOfLogs = exSysSynchLogListBL.GetExsysSynchLogCount(searchParameters);
                log.LogMethodExit(toralNoOfLogs);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = toralNoOfLogs });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
            }
        }
    }
}
