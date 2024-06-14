/********************************************************************************************
 * Project Name - Inventory
 * Description  - Created to fetch, update and insert in the Inventory Notes .
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.110.0    11-Dec-2020  Mushahid Faizan         Created.
 ***************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Inventory;


namespace Semnox.CommonAPI.Controllers.Inventory
{
    public class InventoryNoteController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the InventoryNotes.
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Inventory/Notes")]
        public async Task<HttpResponseMessage> Get(string isActive = null, int inventoryNotesId = -1, int notetypeId = -1, string parafaitObjectName = null,
                                                int parafaitObjectId = -1, int currentPage = 0, int pageSize = 10, string inventoryNotesIdList = null)
        {

            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(isActive, inventoryNotesId, notetypeId, currentPage, pageSize);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<InventoryNotesDTO.SearchByInventoryNotesParameters, string>> searchParameters = new List<KeyValuePair<InventoryNotesDTO.SearchByInventoryNotesParameters, string>>();
                searchParameters.Add(new KeyValuePair<InventoryNotesDTO.SearchByInventoryNotesParameters, string>(InventoryNotesDTO.SearchByInventoryNotesParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<InventoryNotesDTO.SearchByInventoryNotesParameters, string>(InventoryNotesDTO.SearchByInventoryNotesParameters.ISACTIVE, isActive.ToString()));
                    }
                }
                if (!string.IsNullOrEmpty(parafaitObjectName))
                {
                    searchParameters.Add(new KeyValuePair<InventoryNotesDTO.SearchByInventoryNotesParameters, string>(InventoryNotesDTO.SearchByInventoryNotesParameters.PARAFAIT_OBJECT_NAME, parafaitObjectName.ToString()));
                }
                if (inventoryNotesId > -1)
                {
                    searchParameters.Add(new KeyValuePair<InventoryNotesDTO.SearchByInventoryNotesParameters, string>(InventoryNotesDTO.SearchByInventoryNotesParameters.INVENTORY_NOTE_ID, inventoryNotesId.ToString()));
                }
                if (notetypeId > -1)
                {
                    searchParameters.Add(new KeyValuePair<InventoryNotesDTO.SearchByInventoryNotesParameters, string>(InventoryNotesDTO.SearchByInventoryNotesParameters.NOTE_TYPE_ID, notetypeId.ToString()));
                }
                if (parafaitObjectId > -1)
                {
                    searchParameters.Add(new KeyValuePair<InventoryNotesDTO.SearchByInventoryNotesParameters, string>(InventoryNotesDTO.SearchByInventoryNotesParameters.PARAFAIT_OBJECT_ID, parafaitObjectId.ToString()));
                }

                if (!string.IsNullOrEmpty(inventoryNotesIdList))
                {
                    char[] arrayOfCharacters = new Char[] { ',' };
                    List<int> notesIdList = new List<int>();

                    notesIdList = inventoryNotesIdList.Split(arrayOfCharacters, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();

                    String purchaseOrderListString = String.Join(",", notesIdList.ToArray());
                    searchParameters.Add(new KeyValuePair<InventoryNotesDTO.SearchByInventoryNotesParameters, string>(InventoryNotesDTO.SearchByInventoryNotesParameters.INVENTORY_NOTE_ID_LIST, notesIdList.ToString()));
                }

                InventoryNotesList inventoryNotesList = new InventoryNotesList(executionContext);

                int totalNoOfPages = 0;
                int totalCount = await Task<int>.Factory.StartNew(() => { return inventoryNotesList.GetInventoryNotesCount(searchParameters, null); });
                log.LogVariableState("totalCount", totalCount);
                totalNoOfPages = (totalCount / pageSize) + ((totalCount % pageSize) > 0 ? 1 : 0);

                IInventoryNotesUseCases inventoryNotesUseCases = InventoryUseCaseFactory.GetInventoryNotesUseCases(executionContext);
                List<InventoryNotesDTO> inventoryNotesDTOList = await inventoryNotesUseCases.GetInventoryNotes(searchParameters, currentPage, pageSize);

                log.LogMethodExit(inventoryNotesDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = inventoryNotesDTOList, currentPageNo = currentPage, TotalCount = totalCount });

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

        /// <summary>
        /// Post the JSON Object InventoryNotesDTO
        /// </summary>
        /// <param name="inventoryNotesDTOList">inventoryNotesDTOList</param>
        /// <returns>HttpMessgae</returns>
        [HttpPost]
        [Route("api/Inventory/Notes")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody] List<InventoryNotesDTO> inventoryNotesDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(inventoryNotesDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (inventoryNotesDTOList == null || inventoryNotesDTOList.Any(a => a.InventoryNoteId > 0))
                {
                    log.LogMethodExit(inventoryNotesDTOList);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }

                IInventoryNotesUseCases inventoryNotesUseCases = InventoryUseCaseFactory.GetInventoryNotesUseCases(executionContext);
                await inventoryNotesUseCases.SaveInventoryNotes(inventoryNotesDTOList);

                log.LogMethodExit(inventoryNotesDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = inventoryNotesDTOList });
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

        /// <summary>
        /// Update the JSON Object InventoryNotesDTO
        /// </summary>
        /// <param name="inventoryNotesDTOList">inventoryNotesDTOList</param>
        /// <returns>HttpMessgae</returns>
        [HttpPut]
        [Route("api/Inventory/Notes")]
        [Authorize]
        public async Task<HttpResponseMessage> Put([FromBody] List<InventoryNotesDTO> inventoryNotesDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(inventoryNotesDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (inventoryNotesDTOList == null || inventoryNotesDTOList.Any(a => a.InventoryNoteId < 0))
                {
                    log.LogMethodExit(inventoryNotesDTOList);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }

                IInventoryNotesUseCases inventoryNotesUseCases = InventoryUseCaseFactory.GetInventoryNotesUseCases(executionContext);
                await inventoryNotesUseCases.SaveInventoryNotes(inventoryNotesDTOList);

                log.LogMethodExit(inventoryNotesDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = inventoryNotesDTOList });
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
