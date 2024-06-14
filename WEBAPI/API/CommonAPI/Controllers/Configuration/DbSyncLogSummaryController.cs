/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - API for the DB Synch Log Summary
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By            Remarks          
 *********************************************************************************************
 *2.155.0     29-Sep-2023   Lakshminarayana Rao    Created 
 ********************************************************************************************/


using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Semnox.Core.Utilities;
using Semnox.Parafait.DBSynch;

namespace Semnox.CommonAPI.Configuration
{
    public class DBSynchLogSummaryController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Get the JSON Object DBSynch List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Configuration/DBSyncLogSummary")]
        public HttpResponseMessage Get(string tableName = null, string isProcessed = null)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry();
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                DBSynchLogSummaryListBL dBSynchLogSummaryListBL = new DBSynchLogSummaryListBL();
                List<KeyValuePair<DBSynchLogSummaryDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<DBSynchLogSummaryDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<DBSynchLogSummaryDTO.SearchByParameters, string>(DBSynchLogSummaryDTO.SearchByParameters.SITE_ID, Convert.ToString(securityTokenDTO.SiteId)));
                if(string.IsNullOrWhiteSpace(tableName) == false)
                {
                    searchParameters.Add(new KeyValuePair<DBSynchLogSummaryDTO.SearchByParameters, string>(DBSynchLogSummaryDTO.SearchByParameters.TABLE_NAME, tableName));
                }
                if (string.IsNullOrWhiteSpace(isProcessed) == false)
                {
                    searchParameters.Add(new KeyValuePair<DBSynchLogSummaryDTO.SearchByParameters, string>(DBSynchLogSummaryDTO.SearchByParameters.IS_PROCESSED, isProcessed));
                }
                var content = dBSynchLogSummaryListBL.GetDBSynchLogSummaryDTOList(searchParameters);
                log.LogMethodExit();
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = ex.Message });
            }
        }

    }
}