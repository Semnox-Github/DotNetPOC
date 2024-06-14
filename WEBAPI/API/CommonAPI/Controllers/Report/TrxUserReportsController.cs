/**************************************************************************************************
 * Project Name - Reports 
 * Description  - Controller for ViewTransactions - TrxUserLogs
 *  
 **************
 **Version Log
 **************
 *Version     Date              Modified By               Remarks          
 **************************************************************************************************
 *2.80        07-Jun-2020       Vikas Dwivedi             Created to Get Methods.
 **************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Transaction;

namespace Semnox.CommonAPI.Reports
{
    public class TrxUserReportsController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
      

        /// <summary>
        /// Get the JSON Object of TrxUserLogDTO
        /// </summary>
        /// <returns>HttpResponseMessage</returns>

        [HttpGet]
        [Authorize]
        [Route("api/Report/TrxUserReports")]
        public HttpResponseMessage Get(int trxUserLogId = -1, int trxId = -1, int lineId = -1, string loginId = null, int posMachineId = -1)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(trxUserLogId, trxId, lineId, loginId, posMachineId);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                
                // Fetching Trx_UserLogs
                List<KeyValuePair<TrxUserLogsDTO.SearchByParameters, string>> trxUserLogsSearchParameter = new List<KeyValuePair<TrxUserLogsDTO.SearchByParameters, string>>();
                trxUserLogsSearchParameter.Add(new KeyValuePair<TrxUserLogsDTO.SearchByParameters, string>(TrxUserLogsDTO.SearchByParameters.SITE_ID, Convert.ToString(executionContext.GetSiteId())));
                if (trxUserLogId > -1)
                {
                    trxUserLogsSearchParameter.Add(new KeyValuePair<TrxUserLogsDTO.SearchByParameters, string>(TrxUserLogsDTO.SearchByParameters.TRX_USER_LOG_ID, Convert.ToString(trxUserLogId)));
                }
                if (trxId > -1)
                {
                    trxUserLogsSearchParameter.Add(new KeyValuePair<TrxUserLogsDTO.SearchByParameters, string>(TrxUserLogsDTO.SearchByParameters.TRX_ID, Convert.ToString(trxId)));
                }
                if (lineId > -1)
                {
                    trxUserLogsSearchParameter.Add(new KeyValuePair<TrxUserLogsDTO.SearchByParameters, string>(TrxUserLogsDTO.SearchByParameters.LINE_ID, Convert.ToString(lineId)));
                }
                if (!string.IsNullOrEmpty(loginId))
                {
                    trxUserLogsSearchParameter.Add(new KeyValuePair<TrxUserLogsDTO.SearchByParameters, string>(TrxUserLogsDTO.SearchByParameters.LOGIN_ID, Convert.ToString(loginId)));
                }
                if (posMachineId > -1)
                {
                    trxUserLogsSearchParameter.Add(new KeyValuePair<TrxUserLogsDTO.SearchByParameters, string>(TrxUserLogsDTO.SearchByParameters.POS_MACHINE_ID, Convert.ToString(posMachineId)));
                }
                TrxUserLogsList trxUserLogsList = new TrxUserLogsList(executionContext);
                List<TrxUserLogsDTO> trxUserLogsDTOList = trxUserLogsList.GetAllTrxUserLogs(trxUserLogsSearchParameter);

                log.LogMethodExit(trxUserLogsDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = trxUserLogsDTOList });
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
