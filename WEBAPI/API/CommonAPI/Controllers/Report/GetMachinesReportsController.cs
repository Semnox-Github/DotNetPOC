/**************************************************************************************************
 * Project Name - Reports 
 * Description  - Controller for WirelessDashboard - GetMachines
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
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Game;

namespace Semnox.CommonAPI.Reports
{
    public class GetMachinesReportsController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
     

        /// <summary>
        /// Get the JSON Object of MachineDTO
        /// </summary>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        //[Route("api/Report/WirelessDashboard/GetMachines")]
        [Route("api/Report/GetMachines")]
        public HttpResponseMessage Get(DateTime reportFrom, int masterId = -1, string masterName = null, string isActive = null, string isEByte = null)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {

                log.LogMethodEntry(reportFrom, masterId, masterName, isActive, isEByte);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                List<KeyValuePair<HubDTO.SearchByHubParameters, string>> hubSearchParameter = new List<KeyValuePair<HubDTO.SearchByHubParameters, string>>();
                hubSearchParameter.Add(new KeyValuePair<HubDTO.SearchByHubParameters, string>(HubDTO.SearchByHubParameters.SITE_ID, Convert.ToString(executionContext.GetSiteId())));
                if (masterId > -1)
                {
                    hubSearchParameter.Add(new KeyValuePair<HubDTO.SearchByHubParameters, string>(HubDTO.SearchByHubParameters.HUB_ID, masterId.ToString()));
                }
                if (!string.IsNullOrEmpty(masterName))
                {
                    hubSearchParameter.Add(new KeyValuePair<HubDTO.SearchByHubParameters, string>(HubDTO.SearchByHubParameters.HUB_NAME, masterName.ToString()));
                }
                if (!string.IsNullOrEmpty(isActive))
                {
                    hubSearchParameter.Add(new KeyValuePair<HubDTO.SearchByHubParameters, string>(HubDTO.SearchByHubParameters.IS_ACTIVE, isActive.ToString()));
                }
                if (!string.IsNullOrEmpty(isEByte))
                {
                    hubSearchParameter.Add(new KeyValuePair<HubDTO.SearchByHubParameters, string>(HubDTO.SearchByHubParameters.IS_EBYTE, isEByte.ToString()));
                }
                HubList hubList = new HubList(executionContext);
                List<HubDTO> hubDTOList = hubList.GetMachines(masterId, reportFrom);

                log.LogMethodExit(hubDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = hubDTOList });
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
