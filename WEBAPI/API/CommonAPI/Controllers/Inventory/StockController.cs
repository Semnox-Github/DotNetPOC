/********************************************************************************************
 * Project Name -  Stock Controller
 * Description  - Created to fetch the  Inventory entity.
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.110.0   19-Nov-2020  Girish Kundar         Created.
 *2.110.1   09-Jan-2021  Mushahid Faizan       Modified for Web Inventory UI redesign.
 *2.120.0   19-Apr-2021  Abhishek              Modified for POS UI redesign.
 ***************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Inventory;

namespace Semnox.CommonAPI.Controllers.Inventory
{
    public class StockController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON String
        /// </summary>
        [HttpGet]
        [Authorize]
        [Route("api/Inventory/Stocks")]
        public async Task<HttpResponseMessage> Get(string productIdList = null, int posMachineId = -1, int locationId = -1, string isRedeemable = null, string isSellable = null, DateTime? updatedAfterDate = null)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(productIdList, posMachineId, locationId);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                List<KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>> inventorySearchParams = new List<KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>>();

                if (!string.IsNullOrEmpty(productIdList))
                {
                    char[] arrayOfCharacters = new Char[] { ',' };
                    List<int> productList = new List<int>();

                    productList = productIdList.Split(arrayOfCharacters, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();

                    String productsIdListString = String.Join(",", productList.ToArray());
                    inventorySearchParams.Add(new KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>(InventoryDTO.SearchByInventoryParameters.PRODUCT_ID_LIST, productsIdListString));
                }
                if (posMachineId > -1)
                {
                    inventorySearchParams.Add(new KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>(InventoryDTO.SearchByInventoryParameters.POS_MACHINE_ID, posMachineId.ToString()));
                }
                if (locationId > -1)
                {
                    inventorySearchParams.Add(new KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>(InventoryDTO.SearchByInventoryParameters.LOCATION_ID, locationId.ToString()));
                }
                if (!string.IsNullOrEmpty(isRedeemable))
                {
                    inventorySearchParams.Add(new KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>(InventoryDTO.SearchByInventoryParameters.IS_REDEEMABLE, isRedeemable.ToString()));
                }
                if (!string.IsNullOrEmpty(isSellable))
                {
                    inventorySearchParams.Add(new KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>(InventoryDTO.SearchByInventoryParameters.IS_SELLABLE, isSellable.ToString()));
                }
                if (updatedAfterDate != null)
                {
                    DateTime updatedDate = Convert.ToDateTime(updatedAfterDate);
                    inventorySearchParams.Add(new KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>(InventoryDTO.SearchByInventoryParameters.UPDATED_AFTER_DATE, updatedDate.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture)));
                }
                inventorySearchParams.Add(new KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>(InventoryDTO.SearchByInventoryParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                IInventoryStockUseCases inventoryStockUseCases = InventoryUseCaseFactory.GetInventoryStockUseCases(executionContext);
                var content = await inventoryStockUseCases.GetInventoryDTOList(inventorySearchParams);
                log.LogMethodExit(content);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content });

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
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }
    }
}
