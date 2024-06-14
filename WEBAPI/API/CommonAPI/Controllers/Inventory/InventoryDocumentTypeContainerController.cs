/********************************************************************************************
 * Project Name - CommonAPI                                                                    
 * Description  - Container Controller for the InventoryDocumentType.
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.150.0      08-Sep-2022   Abhishek             Created : Inventory UI redesign
 ********************************************************************************************/
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Inventory;
using Semnox.Parafait.ViewContainer;

namespace Semnox.CommonAPI.Controllers.Inventory
{
    public class InventoryDocumentTypeContainerController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Get the JSON Object Values List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Inventory/InventoryDocumentTypeContainer")]
        public async Task<HttpResponseMessage> Get(int siteId = -1, string hash = null, bool rebuildCache = false)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(siteId, hash, rebuildCache);

                InventoryDocumentTypeContainerDTOCollection inventoryDocumentTypeContainerDTOCollection = await
                          Task<InventoryDocumentTypeContainerDTOCollection>.Factory.StartNew(() =>
                          {
                              return InventoryDocumentTypeViewContainerList.GetInventoryDocumentTypeContainerDTOCollection(siteId, hash, rebuildCache);
                          });

                log.LogMethodExit(inventoryDocumentTypeContainerDTOCollection);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = inventoryDocumentTypeContainerDTOCollection });
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
