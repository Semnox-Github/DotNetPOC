/********************************************************************************************
 * Project Name - CommonAPI                                                                    
 * Description  - Controller of the Location.
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.110.0         09-Nov-2020   Mushahid Faizan        Created : POS UI redesign
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Inventory.Location;

namespace Semnox.CommonAPI.Controllers.Inventory
{
    public class LocationContainerController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Get the JSON Object Values List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Inventory/LocationContainer")]
        public async Task<HttpResponseMessage> Get(int siteId = -1, string hash = null, bool rebuildCache = false)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(siteId, hash, rebuildCache);

                LocationContainerDTOCollection locationContainerDTOCollection = await
                          Task<LocationContainerDTOCollection>.Factory.StartNew(() =>
                          {
                              return LocationViewContainerList.GetLocationContainerDTOCollection(siteId, hash, rebuildCache);
                          });

                log.LogMethodExit(locationContainerDTOCollection);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = locationContainerDTOCollection });
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
