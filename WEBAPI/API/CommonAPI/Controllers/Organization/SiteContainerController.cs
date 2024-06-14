/********************************************************************************************
 * Project Name - CommonAPI                                                                    
 * Description  - Controller of the MIFARE key controller.
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 2.200.0       17-Nov-2020   Lakshminarayana    Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Site;
using Semnox.Parafait.ViewContainer;

namespace Semnox.CommonAPI.Controllers.Organization
{
    public class SiteContainerController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object Values List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Organization/SiteContainer")]
        public async Task<HttpResponseMessage> Get(string hash = null, bool rebuildCache = false, bool onlineEnabledOnly = false, bool fnBEnabledOnly = false)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(hash, rebuildCache);
                SiteContainerDTOCollection siteContainerDTOCollection = await
                           Task<SiteContainerDTOCollection>.Factory.StartNew(() => {
                                    if(rebuildCache)
                                    {
                                        SiteViewContainerList.Rebuild();
                                    }
                                    return SiteViewContainerList.GetSiteContainerDTOCollection(hash, onlineEnabledOnly, fnBEnabledOnly);
                                  });
                log.LogMethodExit(siteContainerDTOCollection);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = siteContainerDTOCollection });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }
    }
}