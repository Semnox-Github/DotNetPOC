/**************************************************************************************************
 * Project Name - Reports 
 * Description  - Controller for Reports 
 *  
 **************
 **Version Log
 **************
 *Version     Date              Modified By               Remarks          
 **************************************************************************************************
 *2.110        28-Nov-2020      Nitin                     Created.
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
using Semnox.Parafait.Reports;

namespace Semnox.CommonAPI.Controllers.Report
{
    public class ReportsController : ApiController
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object of MachineDTO
        /// </summary>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Report/Reports")]
        public HttpResponseMessage Get(int reportId = -1, string reportKey = null, string reportName = null, string reportGroup = null, Boolean? isDashboard = true)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(reportId, reportKey, reportName, reportGroup);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                ReportsList reportsListBL = new ReportsList();
                List<KeyValuePair<ReportsDTO.SearchByReportsParameters, string>> searchParameters = new List<KeyValuePair<ReportsDTO.SearchByReportsParameters, string>>();
                if(!string.IsNullOrEmpty(reportKey))
                    searchParameters.Add(new KeyValuePair<ReportsDTO.SearchByReportsParameters, string>(ReportsDTO.SearchByReportsParameters.REPORT_KEY, reportKey));

                if (!string.IsNullOrEmpty(reportName))
                    searchParameters.Add(new KeyValuePair<ReportsDTO.SearchByReportsParameters, string>(ReportsDTO.SearchByReportsParameters.REPORT_NAME, reportName));

                if (!string.IsNullOrEmpty(reportGroup))
                    searchParameters.Add(new KeyValuePair<ReportsDTO.SearchByReportsParameters, string>(ReportsDTO.SearchByReportsParameters.REPORT_GROUP, reportGroup));

                if (reportId > -1)
                    searchParameters.Add(new KeyValuePair<ReportsDTO.SearchByReportsParameters, string>(ReportsDTO.SearchByReportsParameters.REPORT_ID, reportId.ToString()));

                if (isDashboard != null)
                    searchParameters.Add(new KeyValuePair<ReportsDTO.SearchByReportsParameters, string>(ReportsDTO.SearchByReportsParameters.IS_DASHBOARD, isDashboard.ToString()));

                searchParameters.Add(new KeyValuePair<ReportsDTO.SearchByReportsParameters, string>(ReportsDTO.SearchByReportsParameters.SITE_ID, executionContext.GetIsCorporate() ? executionContext.GetSiteId().ToString() : "-1"));

                List<ReportsDTO> ReportsDTOList = reportsListBL.GetAllReports(searchParameters);

                log.LogMethodExit(ReportsDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = ReportsDTOList });
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
