
/********************************************************************************************
 * Project Name - ImagesController
 * Description  - API to return images
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By       Remarks          
 *********************************************************************************************
 *2.110       20-Dec-2020   Nitin Pai         Created
 *2.140       09-Sep-2021   Laster Menezes    Telerik Sales dashboard changes
 *2.140.6     28-Jul-2023   Rakshith Shetty   Issue Fix:Removing duplicate declaration of search parameter DASHBOARD_TYPE
 ********************************************************************************************/
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
using Semnox.Parafait.User;

namespace Semnox.CommonAPI.Controllers.Report
{
    public class TableauDashboardsController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object of ReportDTO
        /// </summary>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Report/TableauDashboards")]
        public HttpResponseMessage Get(string dashboardType = null)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            Boolean tableauDashboard = true;
            try
            {
                log.LogMethodEntry(dashboardType);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                ReportsList reportsListBL = new ReportsList();
                List<KeyValuePair<ReportsDTO.SearchByReportsParameters, string>> searchParameters = new List<KeyValuePair<ReportsDTO.SearchByReportsParameters, string>>();
                searchParameters.Add(new KeyValuePair<ReportsDTO.SearchByReportsParameters, string>(ReportsDTO.SearchByReportsParameters.DASHBOARD_TYPE, string.IsNullOrWhiteSpace(dashboardType)?"T":dashboardType));
                searchParameters.Add(new KeyValuePair<ReportsDTO.SearchByReportsParameters, string>(ReportsDTO.SearchByReportsParameters.IS_DASHBOARD, "true"));
                searchParameters.Add(new KeyValuePair<ReportsDTO.SearchByReportsParameters, string>(ReportsDTO.SearchByReportsParameters.SITE_ID, executionContext.GetIsCorporate() ? executionContext.GetSiteId().ToString() : "-1"));
                searchParameters.Add(new KeyValuePair<ReportsDTO.SearchByReportsParameters, string>(ReportsDTO.SearchByReportsParameters.IS_ACTIVE, "true"));

                if (string.IsNullOrWhiteSpace(dashboardType) || dashboardType.ToLower() != "t")
                    tableauDashboard = false;

                List<ReportsDTO> reportsDTOList = reportsListBL.GetAllReports(searchParameters);

                List<ReportsDTO> allowedReportsDTOList = new List<ReportsDTO>();

                if (reportsDTOList != null && reportsDTOList.Any())
                {
                    Users user = new Users(executionContext, executionContext.GetUserId(), executionContext.GetSiteId());
                    ManagementFormAccessListBL managementFormAccessListBL = new ManagementFormAccessListBL(executionContext);
                    List<KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>>();
                    searchParams.Add(new KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>(ManagementFormAccessDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                    searchParams.Add(new KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>(ManagementFormAccessDTO.SearchByParameters.ROLE_ID, user.UserDTO.RoleId.ToString()));
                    searchParams.Add(new KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>(ManagementFormAccessDTO.SearchByParameters.ISACTIVE, "1"));
                    List<ManagementFormAccessDTO> managementFormAccessList = managementFormAccessListBL.GetManagementFormAccessDTOList(searchParams);
                    List<ManagementFormAccessDTO> managementFormAccessDTOList = new List<ManagementFormAccessDTO>();

                    if (reportsDTOList != null && reportsDTOList.Any())
                    {
                        reportsDTOList = reportsDTOList.OrderBy(m => m.ReportName).ToList();
                        for (int i = 0; i < reportsDTOList.Count; i++)
                        {
                            var isreportsNotExist = managementFormAccessList.Find(m => m.FormName == reportsDTOList[i].ReportName && m.MainMenu == "Run Reports" && m.FunctionGroup == "Reports" && m.RoleId == user.UserDTO.RoleId
                                                                                        && m.AccessAllowed == true);
                            if (isreportsNotExist != null)
                            {
                                if (tableauDashboard == null || Convert.ToBoolean(tableauDashboard) == true)
                                {
                                    reportsDTOList[i].DBQuery = reportsDTOList[i].DBQuery.Replace("@tableauURL", ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "TABLEAU_DASHBOARDS_URL"));
                                }
                                else
                                {
                                    reportsDTOList[i].DBQuery = reportsDTOList[i].DBQuery.Replace("@reportURL", ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "REPORT_WEBSITE_URL"));
                                }
                                allowedReportsDTOList.Add(reportsDTOList[i]);
                            }
                        }
                    }
                }

                log.LogMethodExit(allowedReportsDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = allowedReportsDTOList });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        /// <summary>
        /// Get the one time use tableau token
        /// </summary>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Report/TableauDashboards/Token")]
        public HttpResponseMessage Get(int reportId, string deviceGuid)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(reportId, deviceGuid);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                String token = "";
                Semnox.Parafait.Reports.Reports reportBL = new Semnox.Parafait.Reports.Reports(executionContext, reportId);
                token = reportBL.GetTableauToken(deviceGuid);

                log.LogMethodExit(token);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = token });
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
