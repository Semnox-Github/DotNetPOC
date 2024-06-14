/********************************************************************************************
 * Project Name -  LocationType Controller
 * Description  - Created to fetch the  InventoryDocumentType entity.
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.110.0     28-Oct-2020  Mushahid Faizan         Created.
 ***************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Inventory;

namespace Semnox.CommonAPI.Controllers.Inventory
{
    public class InventoryDocumentTypeController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the inventoryDocumentTypeList.
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Inventory/InventoryDocumentTypes")]
        public async Task<HttpResponseMessage> Get(string isActive = null, int documentTypeId = -1, string documentName = null, string applicability=null, string code=null)
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


                List<KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>> searchParameters = new List<KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>>();
                searchParameters.Add(new KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>(InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>(InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters.ACTIVE_FLAG, isActive.ToString()));
                    }
                }
                if (documentTypeId > -1)
                {
                    searchParameters.Add(new KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>(InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters.DOCUMENT_TYPE_ID, documentTypeId.ToString()));
                }
                if (!string.IsNullOrEmpty(documentName))
                {
                    searchParameters.Add(new KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>(InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters.NAME, documentName));
                }
                if (!string.IsNullOrEmpty(code))
                {
                    searchParameters.Add(new KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>(InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters.CODE, code));
                }
                if (!string.IsNullOrEmpty(applicability))
                {
                    searchParameters.Add(new KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>(InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters.APPLICABILITY, applicability));
                }

                IApprovalRuleUseCases approvalRuleUseCases = InventoryUseCaseFactory.GetApprovalRuleUseCases(executionContext);
                List<InventoryDocumentTypeDTO> inventoryDocumentTypeDTOList = await approvalRuleUseCases.GetInventoryDocumentTypes(searchParameters);
                log.LogMethodExit(inventoryDocumentTypeDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = inventoryDocumentTypeDTOList });

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
