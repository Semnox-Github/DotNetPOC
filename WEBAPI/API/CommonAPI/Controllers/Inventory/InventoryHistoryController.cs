/********************************************************************************************
 * Project Name - Inventory
 * Description  - Created to fetch, update and insert in the InventoryHistory .
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.110.1     01-Mar-2021      Mushahid Faizan       Created
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

namespace Semnox.CommonAPI.Controllers.Inventory
{
    public class InventoryHistoryController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the InventoryHistory.
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Inventory/InventoryHistory")]
        public async Task<HttpResponseMessage> Get(int inventoryHistoryId = -1, int productId = -1, int locationId = -1, int physicalCountId = -1, int quantity = -1, bool ismodifiedDuringPhysicalCount = true,
                                     int lotId = -1, int uomId = -1, int currentPage = 0, int pageSize = 10)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(inventoryHistoryId, productId, locationId, physicalCountId, quantity, ismodifiedDuringPhysicalCount,lotId, uomId, currentPage, pageSize);
               // ExecutionContext executionContext = ExecutionContextBuilder.GetExecutionContext(Request);


                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));


                List<KeyValuePair<InventoryHistoryDTO.SearchByInventoryHistoryParameters, string>> searchParameters = new List<KeyValuePair<InventoryHistoryDTO.SearchByInventoryHistoryParameters, string>>();
                searchParameters.Add(new KeyValuePair<InventoryHistoryDTO.SearchByInventoryHistoryParameters, string>(InventoryHistoryDTO.SearchByInventoryHistoryParameters.SITE_ID, executionContext.GetSiteId().ToString()));

                if (inventoryHistoryId > -1)
                {
                    searchParameters.Add(new KeyValuePair<InventoryHistoryDTO.SearchByInventoryHistoryParameters, string>(InventoryHistoryDTO.SearchByInventoryHistoryParameters.ID, inventoryHistoryId.ToString()));
                }
                if (productId > -1)
                {
                    searchParameters.Add(new KeyValuePair<InventoryHistoryDTO.SearchByInventoryHistoryParameters, string>(InventoryHistoryDTO.SearchByInventoryHistoryParameters.PRODUCT_ID, productId.ToString()));
                }
                if (locationId > -1)
                {
                    searchParameters.Add(new KeyValuePair<InventoryHistoryDTO.SearchByInventoryHistoryParameters, string>(InventoryHistoryDTO.SearchByInventoryHistoryParameters.LOCATION_ID, locationId.ToString()));
                }
                if (physicalCountId > -1)
                {
                    searchParameters.Add(new KeyValuePair<InventoryHistoryDTO.SearchByInventoryHistoryParameters, string>(InventoryHistoryDTO.SearchByInventoryHistoryParameters.PHYSICAL_COUNT_ID, physicalCountId.ToString()));
                }
                if (lotId > -1)
                {
                    searchParameters.Add(new KeyValuePair<InventoryHistoryDTO.SearchByInventoryHistoryParameters, string>(InventoryHistoryDTO.SearchByInventoryHistoryParameters.LOT_ID, lotId.ToString()));
                }
                if (!ismodifiedDuringPhysicalCount) //If IsModifiedDuringPhysicalCount = false then it will return UnModified records.
                {
                    searchParameters.Add(new KeyValuePair<InventoryHistoryDTO.SearchByInventoryHistoryParameters, string>(InventoryHistoryDTO.SearchByInventoryHistoryParameters.MODIFIED_DURING_PHYSICAL_COUNT, "0"));
                }
                if (uomId > -1)
                {
                    searchParameters.Add(new KeyValuePair<InventoryHistoryDTO.SearchByInventoryHistoryParameters, string>(InventoryHistoryDTO.SearchByInventoryHistoryParameters.UOM_ID, uomId.ToString()));
                }
                if (quantity > -1)
                {
                    searchParameters.Add(new KeyValuePair<InventoryHistoryDTO.SearchByInventoryHistoryParameters, string>(InventoryHistoryDTO.SearchByInventoryHistoryParameters.QUANTITY, quantity.ToString()));
                }

                
                IinventoryHistoryUsecases inventoryHistoryUsecases = InventoryUseCaseFactory.GetinventoryHistoryUsecases(executionContext);
                List<InventoryHistoryDTO> inventoryHistoryDTOList = await inventoryHistoryUsecases.GetInventoryHistory(searchParameters, currentPage, pageSize);

                log.LogMethodExit(inventoryHistoryDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = inventoryHistoryDTOList });

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

        /// <summary>
        /// Post the JSON Object Inventory History 
        /// </summary>
        /// <param name="inventoryHistoryDTOList">inventoryHistoryDTOList</param>
        /// <returns>HttpMessgae</returns>
        [HttpPost]
        [Route("api/Inventory/InventoryHistory")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody] List<InventoryHistoryDTO> inventoryHistoryDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(inventoryHistoryDTOList);
                //  ExecutionContext executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));


                if (inventoryHistoryDTOList == null || inventoryHistoryDTOList.Any(a => a.Id > 0))
                {
                    log.LogMethodExit(inventoryHistoryDTOList);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }

                IinventoryHistoryUsecases inventoryHistoryUsecases = InventoryUseCaseFactory.GetinventoryHistoryUsecases(executionContext);
                await inventoryHistoryUsecases.SaveInventoryHistory(inventoryHistoryDTOList);


                log.LogMethodExit(inventoryHistoryDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = inventoryHistoryDTOList });
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

        /// <summary>
        /// Post the JSON Object Inventory History 
        /// </summary>
        /// <param name="inventoryHistoryDTOList">inventoryHistoryDTOList</param>
        /// <returns>HttpMessgae</returns>
        [HttpPut]
        [Route("api/Inventory/InventoryHistory")]
        [Authorize]
        public async Task<HttpResponseMessage> Put([FromBody] List<InventoryHistoryDTO> inventoryHistoryDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(inventoryHistoryDTOList);
                // ExecutionContext executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (inventoryHistoryDTOList == null || inventoryHistoryDTOList.Any(a => a.Id < 0))
                {
                    log.LogMethodExit(inventoryHistoryDTOList);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }

                IinventoryHistoryUsecases inventoryHistoryUsecases = InventoryUseCaseFactory.GetinventoryHistoryUsecases(executionContext);
                await inventoryHistoryUsecases.SaveInventoryHistory(inventoryHistoryDTOList);


                log.LogMethodExit(inventoryHistoryDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = inventoryHistoryDTOList });
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