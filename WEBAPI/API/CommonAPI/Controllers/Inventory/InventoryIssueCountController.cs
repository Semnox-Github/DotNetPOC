/********************************************************************************************
 * Project Name - InventoryIssueCount Controller
 * Description  - Created InventoryIssueCount Controller
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.110.1   11-feb-2021   Likhitha Reddy            created
 ***************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.Utilities;
using Semnox.Parafait.Inventory;

namespace Semnox.CommonAPI.Controllers.Inventory
{
    public class InventoryIssueCountController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the InventoryIssueHeaders.
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Inventory/IssueCounts")]
        public async Task<HttpResponseMessage> Get(string isActive = null, int inventoryIssueId = -1, int documentTypeId = -1, int purchaseOrderId = -1, int requisitionId = -1,
                                                    string status = null, int currentPage = 0, int pageSize = 10)
        {

            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(isActive, inventoryIssueId, documentTypeId, currentPage, pageSize);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters, string>> searchParameters = new List<KeyValuePair<InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters, string>>();
                searchParameters.Add(new KeyValuePair<InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters, string>(InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters, string>(InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters.ACTIVE_FLAG, isActive.ToString()));
                    }
                }
                if (!string.IsNullOrEmpty(status))
                {
                    searchParameters.Add(new KeyValuePair<InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters, string>(InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters.STATUS, status.ToString()));
                }
                if (inventoryIssueId > -1)
                {
                    searchParameters.Add(new KeyValuePair<InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters, string>(InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters.INVENTORY_ISSUE_ID, inventoryIssueId.ToString()));
                }
                if (documentTypeId > -1)
                {
                    searchParameters.Add(new KeyValuePair<InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters, string>(InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters.DOCUMENT_TYPE_ID, documentTypeId.ToString()));
                }
                if (purchaseOrderId > -1)
                {
                    searchParameters.Add(new KeyValuePair<InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters, string>(InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters.PURCHASE_ORDER_ID, purchaseOrderId.ToString()));
                }
                if (requisitionId > -1)
                {
                    searchParameters.Add(new KeyValuePair<InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters, string>(InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters.REQUISITION_ID, requisitionId.ToString()));
                }

                InventoryIssueHeaderList inventoryIssueHeadersList = new InventoryIssueHeaderList();
                IInventoryIssueHeaderUseCases inventoryIssueCount = InventoryUseCaseFactory.GetInventoryIssueHeadersUseCases(executionContext);
                int totalNoOfPages = 0;
                int totalCount = await inventoryIssueCount.GetInventoryIssueCount(searchParameters); 
                log.LogVariableState("totalCount", totalCount);
                totalNoOfPages = (totalCount / pageSize) + ((totalCount % pageSize) > 0 ? 1 : 0);
                log.LogMethodExit(totalCount);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = totalCount, currentPageNo = currentPage, TotalNoOfPages = totalNoOfPages });

            }
            catch (Exception ex)
            {
                log.Error(ex);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new
                {
                    data = ExceptionSerializer.Serialize(ex)
                });
            }
        }
    }
}
