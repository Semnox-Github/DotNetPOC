/********************************************************************************************
 * Project Name - SalesDashboardController                                                                         
 * Description  - Controller for the getting the collections for sales dashboard.
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.120.7      00-Apr-2022   Nitin Pai            Created 
 *2.155.0      07-Jul-2023   Abhishek             Modified:Added RoleId to display Sales/Consumption Based on ManagementFormAccess. 
 ********************************************************************************************/
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.DashBoard;

namespace Semnox.CommonAPI.Controllers.Report
{
    public class SalesDashboardController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ExecutionContext executionContext = null;

        /// //<summary>
        /// Get the JSON Object of ParafaitDashBoard DTO
        /// </summary>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Report/SalesDashboard")]
        public async Task<HttpResponseMessage> Get(string reportType = "HEADER", int siteId = -1, string posMachine = "", int roleId = -1)
        {
            try
            {
                log.LogMethodEntry(reportType, siteId, posMachine, roleId);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                ISalesDashboardUseCases salesDashboardUseCases = SalesDashboardUseCaseFactory.GetSalesDashboardUseCases(executionContext);

                if (reportType.Equals("HEADER"))
                {
                    var content = await salesDashboardUseCases.GetWeeklyCollectionList(roleId);
                    log.LogMethodExit(content);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = content });
                }
                else
                {
                    var content = await salesDashboardUseCases.GetWeeklyCollectionPOSList(siteId, posMachine);
                    log.LogMethodExit(content);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = content });
                }
            }
            catch (ValidationException valex)
            {
                log.Error(valex);
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(valex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
            }
            catch (Exception ex)
            {
                log.Error(ex);
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }
    }
}
