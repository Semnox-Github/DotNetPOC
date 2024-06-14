/********************************************************************************************
 * Project Name - Inventory
 * Description  - POTaxViewController
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.110.0    11-Jan-2021    Abhishek               Created : As part of Inventory UI Redesign    
 ***************************************************************************************************/
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Inventory;

namespace Semnox.CommonAPI.Controllers.Inventory
{
    public class PurchaseOrderTaxViewController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Generate the base64String for Issue.
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Inventory/PurchaseOrderTaxes")]
        public async Task<HttpResponseMessage> Get(int purchaseOrderId, int purchaseOrderLineId = -1, bool viewTotalPOTax = false)
        {

            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(purchaseOrderId, purchaseOrderLineId, viewTotalPOTax);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                IPOTaxViewUseCases inventoryPhysicalCountUseCases = InventoryUseCaseFactory.GetPOTaxViewsUseCases(executionContext);
                List<PurchaseOrderTaxLineDTO> purchaseOrderTaxLineDTOList = await inventoryPhysicalCountUseCases.GetPurchaseOrderTaxViews(purchaseOrderId, purchaseOrderLineId, viewTotalPOTax);
                log.LogMethodExit(purchaseOrderTaxLineDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = purchaseOrderTaxLineDTOList });
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
