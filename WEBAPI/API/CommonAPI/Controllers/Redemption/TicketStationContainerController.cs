/********************************************************************************************
 * Project Name - CommonAPI                                                                    
 * Description  - Controller of the TicketStationContainer controller.
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.110.0      21-Dec-2020      Abhishek           Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Redemption;
using Semnox.Parafait.ViewContainer;

namespace Semnox.CommonAPI.Controllers.Device
{
    public class TicketStationContainerController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// Get the JSON Object Values List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Device/TicketStationContainer")]
        public async Task<HttpResponseMessage> Get (int siteId = -1, string hash = null, bool rebuildCache = false)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(rebuildCache, hash);
                TicketStationContainerDTOCollection ticketStationContainerDTOCollection = await
                           Task<TicketStationContainerDTOCollection>.Factory.StartNew(() => {
                                    if(rebuildCache)
                                    {
                                        TicketStationViewContainerList.Rebuild(siteId);
                                    }
                                    return TicketStationViewContainerList.GetTicketStationContainerDTOCollection(siteId, hash);
                           });
                log.LogMethodExit(ticketStationContainerDTOCollection);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = ticketStationContainerDTOCollection });
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