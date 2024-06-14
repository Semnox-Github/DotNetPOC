/**************************************************************************************************
 * Project Name - Reports 
 * Description  - Controller for ParafaitDashboard 
 *  
 **************
 **Version Log
 **************
 *Version     Date              Modified By               Remarks          
 **************************************************************************************************
 *2.90        29-May-2020       Vikas Dwivedi             Created to Get Methods.
 **************************************************************************************************/

using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Reports;

namespace Semnox.CommonAPI.Games.Controllers.Reports
{
    public class ParafaitDashboardController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// //<summary>
        /// Get the JSON Object of ParafaitDashBoard DTO
        /// </summary>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Report/ParafaitDashboard")]
        public HttpResponseMessage Get(DateTime fromDate, DateTime toDate, bool loadGraphData = false)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(fromDate, toDate);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                ParafaitDashBoardListBL parafaitDashBoardListBL = new ParafaitDashBoardListBL(executionContext);
                var content = parafaitDashBoardListBL.GetCollections(fromDate, toDate);
                dynamic graphData = null;
                if (loadGraphData)
                {
                    graphData = parafaitDashBoardListBL.GetGraphTable(fromDate, toDate);
                    log.LogMethodExit(graphData);
                }
                log.LogMethodExit(content);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content, graph = graphData });
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
