/**************************************************************************************************
 * Project Name - Reports 
 * Description  - Controller for WirelessDashboard - GetMachines
 *  
 **************
 **Version Log
 **************
 *Version     Date              Modified By               Remarks          
 **************************************************************************************************
 *2.90        07-Jun-2020       Vikas Dwivedi             Created to Get Methods.
 **************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Reports;

namespace Semnox.CommonAPI.Reports
{
    public class WirelessDashBoardMachinesReportsController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object of MachineDTO
        /// </summary>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Report/WirelessDashBoardMachinesReports")]
        public HttpResponseMessage Get(DateTime reportFrom, int masterId = -1)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(reportFrom, masterId);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                WirelessDashBoardListBL wirelessDashBoardListBL = new WirelessDashBoardListBL(executionContext);
                List<WirelessDashBoardDTO> wirelessDashBoardDTOList = wirelessDashBoardListBL.GetMachineDetailsForWirelessDashBoard(masterId, reportFrom);

                log.LogMethodExit(wirelessDashBoardDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = wirelessDashBoardDTOList });
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
