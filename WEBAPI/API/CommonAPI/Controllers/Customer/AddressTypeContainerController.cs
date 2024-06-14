/********************************************************************************************
 * Project Name - CommonAPI                                                                    
 * Description  - Controller of the AddressType.
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.120.00    08-Jul-2021   Roshan Devadiga       Created : POS UI redesign
 ********************************************************************************************/
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer;
using Semnox.Parafait.ViewContainer;

namespace Semnox.CommonAPI.Controllers.Customer
{
    public class AddressTypeContainerController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object Values List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Customer/AddressTypesContainer")]
        public async Task<HttpResponseMessage> Get(int siteId = -1, string hash = null, bool rebuildCache = false)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(siteId, hash, rebuildCache);

                AddressTypeContainerDTOCollection addressTypeContainerDTOCollection = await
                          Task<AddressTypeContainerDTOCollection>.Factory.StartNew(() =>
                          {
                              return AddressTypeViewContainerList.GetAddressTypeContainerDTOCollection(siteId, hash, rebuildCache);
                          });

                log.LogMethodExit(addressTypeContainerDTOCollection);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = addressTypeContainerDTOCollection });
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
