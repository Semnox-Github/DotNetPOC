/********************************************************************************************
* Project Name - CommnonAPI - Reports Module 
* Description  - API for the GamePerformanceReports - Report.
* 
**************
**Version Log
**************
*Version     Date            Modified By         Remarks          
*********************************************************************************************
*2.90        20-May-2020     Vikas Dwivedi       Created
********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.GenericUtilities.Excel;
using Semnox.Core.Utilities;
using Semnox.Parafait.Reports;

namespace Semnox.CommonAPI.Reports
{
    public class GamePerformanceReportsController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get Method
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("api/Report/GamePerformanceReports")]
        public HttpResponseMessage Get(int periodId = -1, bool loadTabsData = false, bool loadOverViewData = false, string profileName = null, bool downloadExcel = false)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(periodId, loadTabsData, loadOverViewData, profileName, downloadExcel);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                List<string> columnNameList = new List<string>();
                GamePerformanceListBL gamePerformanceListBL = new GamePerformanceListBL(executionContext);
                var content = gamePerformanceListBL.GetProfileTabsName();
                DataTable tabsData = new DataTable();

                if (loadTabsData)
                {
                    tabsData = gamePerformanceListBL.GetProfileTabsData(periodId, loadOverViewData, profileName);
                    if (tabsData.Rows.Count > 1 && string.IsNullOrEmpty(profileName) == false)
                    {
                        int numberOfDaysColumns = gamePerformanceListBL.GetDaysCount(periodId);
                        log.Debug("numberOfDaysColumns : " + numberOfDaysColumns);
                        int count = numberOfDaysColumns + 5;
                        for (int i = 5; i < count; i++) 
                        {
                            columnNameList.Add(tabsData.Columns[i].ColumnName.ToString());
                        }
                    }
                    log.LogMethodExit(tabsData);
                }
                log.LogMethodExit(content);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content, tabsDataTable = tabsData, list = columnNameList /*, sheets = sheetList*/ });
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
