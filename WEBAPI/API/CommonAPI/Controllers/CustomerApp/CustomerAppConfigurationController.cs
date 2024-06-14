/********************************************************************************************
 * Project Name - CustomerAppConfigurationController
 * Description  - Controller for Getting the Configuration Setting for the Customer App
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.60        07-May-2019      Nitin Pai      Initial Version 
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
using Semnox.Parafait.CustomerApp;
using Semnox.Parafait.POS;
using Semnox.Parafait.Site;

namespace Semnox.CommonAPI.Controllers.CustomerApp
{
    public class CustomerAppConfigurationController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        /// <summary>
        /// Get the JSON Object Collections of customer app configuration.
        /// </summary>       
        [HttpGet]
        [Route("api/CustomerApp/CustomerAppConfiguration")]
        public HttpResponseMessage Get(int siteId = -1, string machineName = null)
        {
            SecurityTokenDTO securityTokenDTO = null;
            try
            {
                log.LogMethodEntry();
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                //siteId = securityTokenDTO.SiteId;
                if (siteId == -1)
                {
                    this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                    SiteList siteList = new SiteList(executionContext);
                    SiteDTO HQSite = siteList.GetMasterSiteFromHQ();
                    if (HQSite != null && HQSite.SiteId != -1)
                    {
                        siteId = HQSite.SiteId;
                        //securityTokenDTO.SiteId = siteId;
                    }

                    List<SiteDTO> sites = siteList.GetAllSites(-1, -1, -1);
                    if (sites != null && sites.Count > 1)
                    {
                        HttpContext.Current.Application["IsCorporate"] = "True";
                    }
                }
                else
                {
                    //siteId = securityTokenDTO.SiteId;
                    log.LogVariableState("siteId:", siteId);
                }

                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, siteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (!string.IsNullOrEmpty(machineName))
                {
                    List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>> searchParameters = new List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>>();
                    searchParameters.Add(new KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>(POSMachineDTO.SearchByPOSMachineParameters.SITE_ID, Convert.ToString(siteId)));
                    searchParameters.Add(new KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>(POSMachineDTO.SearchByPOSMachineParameters.POS_OR_COMPUTER_NAME, machineName));
                    searchParameters.Add(new KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>(POSMachineDTO.SearchByPOSMachineParameters.ISACTIVE, "1"));
                    POSMachineList pOSMachineList = new POSMachineList(executionContext);
                    List<POSMachineDTO> machineList = pOSMachineList.GetAllPOSMachines(searchParameters, false, false);
                    if (machineList == null || !machineList.Any())
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "POS Machine " + machineName + " is not set up.", token = securityTokenDTO.Token });
                    }
                    executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, machineList[0].POSMachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                }

                CustomerAppConfigurationBL customerAppConfigurationBL = new CustomerAppConfigurationBL(executionContext, siteId);

                if (customerAppConfigurationBL != null)
                {
                    var content = customerAppConfigurationBL.GetConfigurationDTO();
                    log.LogMethodExit(content);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = content, token = securityTokenDTO.Token });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { data = "", token = securityTokenDTO.Token });
                }
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, token = securityTokenDTO.Token });
            }
        }
    }
}
