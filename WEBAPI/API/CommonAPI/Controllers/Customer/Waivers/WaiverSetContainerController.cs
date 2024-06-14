/********************************************************************************************
 * Project Name - CommonAPI                                                                    
 * Description  - Controller of the WaiverSetContainer.
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.130.0    20-Jul-2021   Mushahid Faizan       Created 
 ********************************************************************************************/
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.Utilities;
using Semnox.Parafait.ViewContainer;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Waiver;

namespace Semnox.CommonAPI.Controllers.Customer.Waivers
{
    public class WaiverSetContainerController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object Values List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Customer/Waiver/WaiverSetContainer")]
        public async Task<HttpResponseMessage> Get(int siteId = -1, int languageId = -1,string hash = null, bool rebuildCache = false)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(siteId, hash, rebuildCache);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                WaiverSetContainerDTOCollection waiverSetContainerDTOCollection = await
                          Task<WaiverSetContainerDTOCollection>.Factory.StartNew(() =>
                          {
                              return WaiverSetViewContainerList.GetWaiverSetContainerDTOCollection(siteId, languageId,hash, rebuildCache);
                          });

                log.LogMethodExit(waiverSetContainerDTOCollection);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = waiverSetContainerDTOCollection });
            }
            catch (UnauthorizedException ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
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
