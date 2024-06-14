/********************************************************************************************
 * Project Name - InventoryLotController
 * Description  - Created to fetch inventory lot details.
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *2.150.0     22-Sep-2022    Abhishek         Web Inventory UI resdesign changes with REST API.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Net.Http;
using System.Web.Http;
using Semnox.Core.Utilities;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Inventory;
using Semnox.CommonAPI.Helpers;

namespace Semnox.CommonAPI.Controllers.Inventory
{
    public class InventoryLotController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Product Category.
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Inventory/Lots")]
        public async Task<HttpResponseMessage> Get(int lotId = -1, int purchaseOrderReceiveLineId = -1, string lotNumber = null, int uomId = -1, int currentPage = 0, int pageSize = 10)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(lotNumber,lotId,purchaseOrderReceiveLineId,uomId);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);

                List<KeyValuePair<InventoryLotDTO.SearchByInventoryLotParameters, string>> searchParameters = new List<KeyValuePair<InventoryLotDTO.SearchByInventoryLotParameters, string>>();
                searchParameters.Add(new KeyValuePair<InventoryLotDTO.SearchByInventoryLotParameters, string>(InventoryLotDTO.SearchByInventoryLotParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (!string.IsNullOrEmpty(lotNumber))
                {
                    searchParameters.Add(new KeyValuePair<InventoryLotDTO.SearchByInventoryLotParameters, string>(InventoryLotDTO.SearchByInventoryLotParameters.LOT_NUMBER, lotNumber));
                }
                if (lotId > -1)
                {
                    searchParameters.Add(new KeyValuePair<InventoryLotDTO.SearchByInventoryLotParameters, string>(InventoryLotDTO.SearchByInventoryLotParameters.LOT_ID, lotId.ToString()));
                }
                if (purchaseOrderReceiveLineId > -1)
                {
                    searchParameters.Add(new KeyValuePair<InventoryLotDTO.SearchByInventoryLotParameters, string>(InventoryLotDTO.SearchByInventoryLotParameters.PURCHASEORDER_RECEIVE_LINEID, purchaseOrderReceiveLineId.ToString()));
                }
                if (uomId > -1)
                {
                    searchParameters.Add(new KeyValuePair<InventoryLotDTO.SearchByInventoryLotParameters, string>(InventoryLotDTO.SearchByInventoryLotParameters.UOM_ID, uomId.ToString()));
                }

                IInventoryLotUseCases inventoryLotUseCases = InventoryUseCaseFactory.GetInventoryLotUseCases(executionContext);
                List<InventoryLotDTO> inventoryLotDTOList = await inventoryLotUseCases.GetInventoryLots(searchParameters, currentPage, pageSize);
                log.LogMethodExit(inventoryLotDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = inventoryLotDTOList, currentPageNo = currentPage });

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
