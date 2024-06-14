/********************************************************************************************
 * Project Name - Inventory
 * Description  - Created to fetch, update and insert in the Issue Headers and Lines .
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.150.1    09-Aug-2022   Abhishek                  Created. - Web Inventory Redesign
 ***************************************************************************************************/
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
using Semnox.Parafait.Inventory;
using Semnox.Parafait.Inventory.PhysicalCount;

namespace Semnox.CommonAPI.Controllers.Inventory
{
    public class InventoryPhysicalCountStatusController : ApiController
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Update the JSON Object InventoryPhysicalCountDTO
        /// </summary>
        /// <param name="inventoryPhysicalCountDTOList">inventoryPhysicalCountDTOList</param>
        /// <returns>HttpMessgae</returns>
        [HttpPost]
        [Route("api/Inventory/PhysicalCount/Status")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody] List<InventoryPhysicalCountDTO> inventoryPhysicalCountDTOList)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(inventoryPhysicalCountDTOList);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                if (inventoryPhysicalCountDTOList == null || inventoryPhysicalCountDTOList.Any(a => a.PhysicalCountID < 0))
                {
                    log.LogMethodExit(inventoryPhysicalCountDTOList);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }

                IInventoryPhysicalCountUseCases inventoryPhysicalCountUseCases = InventoryUseCaseFactory.GetInventoryPhysicalCountsUseCases(executionContext);
                List<InventoryPhysicalCountDTO> savedInventoryPhysicalCountDTOList = await inventoryPhysicalCountUseCases.UpdatePhysicalCountStatus(inventoryPhysicalCountDTOList);

                log.LogMethodExit(savedInventoryPhysicalCountDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = savedInventoryPhysicalCountDTOList });
            }
            catch (ValidationException valEx)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(valEx, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
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
