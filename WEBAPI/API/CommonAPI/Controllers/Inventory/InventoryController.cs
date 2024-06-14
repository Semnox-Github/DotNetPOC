/**************************************************************************************************
 * Project Name - Inventory Controller
 * Description  - Created to fetch records of InventoryAdjustments
 *  
 **************
 **Version Log
 **************
 *Version     Date                  Modified By               Remarks          
 **************************************************************************************************
 *2.80        10-Jan-2020           Vikas Dwivedi             Created to Get Method.
 **************************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Semnox.CommonAPI.Controllers
{
    public class InventoryController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly SecurityTokenBL securityTokenBL = new SecurityTokenBL();
        private SecurityTokenDTO securityTokenDTO;
        private ExecutionContext executionContext;

        /// <summary>
        /// Get the JSON String
        /// </summary>
        [HttpGet]
        [Authorize]
        [Route("api/Inventory/Inventory/")]
        public async Task<HttpResponseMessage> Get(int siteId = -1, int pageNumber = 0, int pageSize = 500, bool redeemableInventoryOnly = false, bool buildChildRecords = false)
        {
            try
            {
                log.LogMethodEntry();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                int totalNoOfPages = 0;

                //InventoryList inventoryList = new InventoryList(executionContext);
                //List<KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>> searchParameters = new List<KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>>();

                //if (siteId > -1)
                //{
                //    searchParameters.Add(new KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>(InventoryDTO.SearchByInventoryParameters.SITE_ID, Convert.ToString(securityTokenDTO.SiteId)));
                //}
                //if(redeemableInventoryOnly)
                //{
                //    searchParameters.Add(new KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>(InventoryDTO.SearchByInventoryParameters.IS_REDEEMABLE_PRODUCT, "Y"));
                //}

                //int numberOfRecords = await Task<int>.Factory.StartNew(() => { return inventoryList.GetInventoryRecordsCount(searchParameters, null); });
                //log.Debug("Number of account:" + numberOfRecords);

                //IList<InventoryDTO> inventoryDTOList = null;
                //if (numberOfRecords > 0)
                //{
                //    pageNumber = pageNumber < -1 || pageNumber > totalNoOfPages ? 0 : pageNumber;
                //    pageSize = pageSize > 500 || pageSize == 0 ? 500 : pageSize;

                //    totalNoOfPages = (numberOfRecords / pageSize) + ((numberOfRecords % pageSize) > 0 ? 1 : 0);
                //    log.Debug("Number of pages:" + totalNoOfPages);

                //    inventoryDTOList = await Task<List<InventoryDTO>>.Factory.StartNew(() => {
                //        return inventoryList.GetAllInventoryList(searchParameters, pageNumber, pageSize, true, null); 
                //    });
                //}
                //log.LogMethodExit();
                //return Request.CreateResponse(HttpStatusCode.OK, new { data = inventoryDTOList, token = securityTokenDTO.Token });
                return Request.CreateResponse(HttpStatusCode.OK, new { data = "", token = securityTokenDTO.Token });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, token = securityTokenDTO.Token });
            }
        }
    }
}
