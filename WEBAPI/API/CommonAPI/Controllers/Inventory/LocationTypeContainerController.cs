/********************************************************************************************
 * Project Name - CommonAPI                                                                    
 * Description  - Controller of the LocationTypeContainer.
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.110.0      15-Jan-2021   Vikas Dwivedi        Created : POS UI redesign
 ********************************************************************************************/
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Inventory.Location;
using Semnox.Parafait.ViewContainer;

namespace Semnox.CommonAPI.Controllers.Inventory
{
    public class LocationTypeContainerController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Get the JSON Object Values List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Inventory/LocationTypeContainer")]
        public async Task<HttpResponseMessage> Get(int siteId = -1, string hash = null, bool rebuildCache = false)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(siteId, hash, rebuildCache);

                LocationTypeContainerDTOCollection locationTypeContainerDTOCollection = await
                          Task<LocationTypeContainerDTOCollection>.Factory.StartNew(() =>
                          {
                              return LocationTypeViewContainerList.GetLocationTypeContainerDTOCollection(siteId, hash, rebuildCache);
                          });

                log.LogMethodExit(locationTypeContainerDTOCollection);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = locationTypeContainerDTOCollection });
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
