/********************************************************************************************
 * Project Name - Customer UI Meta data Controller
 * Description  - Controller for Customer UI Meta data Resource
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.60        07-May-2019      Nitin Pai      Initial Version 
 *2.80        22-Oct-2019      Nitin Pai      Guest App P2 and Customer Registration Changes 
 *2.80        05-Apr-2020      Girish Kundar  Modified: API path changes and token removed form the response body
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Site;

namespace Semnox.CommonAPI.Controllers.Customer
{
    public class CustomerUIMetaDataController : ApiController
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        /// <summary>
        /// Get the JSON Object Collections of customer ui meta data.
        /// </summary>       
        [HttpGet]
        [Route("api/Customer/CustomerUIMetadata")]
        public HttpResponseMessage Get()
        {
            SecurityTokenDTO securityTokenDTO = null;
            try
            {
                log.LogMethodEntry();
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                
                // the UI metadata is always set up at HQ level. If no HQ is set up, take the site id from execution context
                // commenting as the customer save is being done to the site and the ui metadata can be different there
                int siteId = securityTokenDTO.SiteId;
                //SiteList siteList = new SiteList();
                //SiteDTO HQSite = siteList.GetMasterSiteFromHQ();
                //if (HQSite != null && HQSite.SiteId != -1)
                //{
                //    siteId = HQSite.SiteId;
                //}

                log.Debug("Customer ui meta data site " + siteId);
                CustomerUIMetadataBL customerUIMetadataBL = new CustomerUIMetadataBL(executionContext);
                var content = customerUIMetadataBL.GetCustomerUIMetadataDTOList(siteId); 
                if (content.Count > 0)
                {
                    log.LogMethodExit(content);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = content });
                }
                else
                {
                    log.LogMethodExit("Customer UI Metadata not found");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "No data found" });
                }
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException});
            }
        }
    }
}
