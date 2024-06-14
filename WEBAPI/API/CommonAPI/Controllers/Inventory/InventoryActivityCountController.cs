/********************************************************************************************
 * Project Name - InventoryActivityCount Controller
 * Description  - Created InventoryActivityCount Controller
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.110.1   10-feb-2021   Likhitha Reddy            created
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
    public class InventoryActivityCountController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the Location Type.
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Inventory/InventoryActivityCounts")]
        public async Task<HttpResponseMessage> Get(string isActive = null, int invTableKey = -1, string message = null, string tableName = null
                                                   , int currentPage = 0, int pageSize = 10)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(isActive);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));


                List<KeyValuePair<InventoryActivityLogDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<InventoryActivityLogDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<InventoryActivityLogDTO.SearchByParameters, string>(InventoryActivityLogDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));

                if (invTableKey > -1)
                {
                    searchParameters.Add(new KeyValuePair<InventoryActivityLogDTO.SearchByParameters, string>(InventoryActivityLogDTO.SearchByParameters.INV_TABLE_KEY, invTableKey.ToString()));
                }
                if (!string.IsNullOrEmpty(tableName))
                {
                    searchParameters.Add(new KeyValuePair<InventoryActivityLogDTO.SearchByParameters, string>(InventoryActivityLogDTO.SearchByParameters.SOURCE_TABLE_NAME, tableName));
                }
                if (!string.IsNullOrEmpty(message))
                {
                    searchParameters.Add(new KeyValuePair<InventoryActivityLogDTO.SearchByParameters, string>(InventoryActivityLogDTO.SearchByParameters.MESSAGE, message));
                }

                InventoryActivityLogBLList inventoryActivityLogBLList = new InventoryActivityLogBLList(executionContext);
                IInventoryActivityLogUseCases inventoryActivityUseCases = InventoryUseCaseFactory.GetInventoryActivityLogUseCases(executionContext);
                int totalNoOfPages = 0;
                int totalCount = await inventoryActivityUseCases.GetInventoryAcitvityCount(searchParameters);
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
