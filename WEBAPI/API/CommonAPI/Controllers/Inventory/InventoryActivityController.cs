/********************************************************************************************
 * Project Name -  InventoryActivityLogController
 * Description  -  Created to fetch the  Inventory Activity Logs.
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.110.0   19-Nov-2020    Mushahid Faizan           Created.
 *2.110.0   01-Dec-2020    Abhishek                  Modified: added factory
 ***************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Inventory;

namespace Semnox.CommonAPI.Controllers.Inventory
{
    public class InventoryActivityController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the Location Type.
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Inventory/InventoryActivities")]
        public async Task<HttpResponseMessage> Get(int invTableKey = -1, string message = null, string tableName=null, 
                                                   string sourceSystemId = null, int currentPage = 0, int pageSize = 10)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(invTableKey);
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
                if (!string.IsNullOrEmpty(sourceSystemId))
                {
                    searchParameters.Add(new KeyValuePair<InventoryActivityLogDTO.SearchByParameters, string>(InventoryActivityLogDTO.SearchByParameters.SOURCE_SYSTEM_ID, sourceSystemId));
                }

                InventoryActivityLogBLList inventoryActivityLogBLList = new InventoryActivityLogBLList(executionContext);
                int totalNoOfPages = 0;
                int totalInventoryActivityLogCount = await Task<int>.Factory.StartNew(() => { return inventoryActivityLogBLList.GetInventoryActivityLogCount(searchParameters, null); });
                log.LogVariableState("totalCount", totalInventoryActivityLogCount);
                totalNoOfPages = (totalInventoryActivityLogCount / pageSize) + ((totalInventoryActivityLogCount % pageSize) > 0 ? 1 : 0);

                IInventoryActivityLogUseCases invenoryActivityLogUseCases = InventoryUseCaseFactory.GetInventoryActivityLogUseCases(executionContext);
                List<InventoryActivityLogDTO> IinventoryActivityLogDTOList = await invenoryActivityLogUseCases.GetInventoryAcitvityLogs(searchParameters, currentPage, pageSize, null);
                log.LogMethodExit(IinventoryActivityLogDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = IinventoryActivityLogDTOList, currentPageNo = currentPage, TotalCount = totalInventoryActivityLogCount });

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
