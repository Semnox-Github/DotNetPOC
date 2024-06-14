/********************************************************************************************
 * Project Name - Products 
 * Description  - Created to  Genrate Barcodes for products using BarcodeLib dll
 *  
 **************
 **Version Log
 **************
 *Version     Date            Created By               Remarks          
 ***************************************************************************************************
 *2.70.0      27-June-2019    Jagan Mohana             Created to Generate Barcodes for Inventory details.
 *2.110.0     10-Sep-2020     Girish Kundar            Modified : Moved to inventory resource and  REST API Standards.
 ****************************************************************************************************/
using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;

namespace Semnox.CommonAPI.Controllers.Inventory
{
    public class UPCBarcodeController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
     
        /// <summary>
        /// Generates Barcodes for products inventory details generate upc barcode
        /// Convert a bitmap image to memorystream and send the response
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>        
        [HttpGet]
        [Route("api/Inventory/UPCBarcodes")]
        [Authorize]
        public HttpResponseMessage Get(int productId, int preferredVendor, string productCode)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(productId, preferredVendor, productCode);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                                
                ProductBL productBL = new ProductBL(executionContext, productId, false, false);
                productBL.GenerateUPCBarCode(productId, preferredVendor, productCode);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });
            }
            catch (ValidationException valEx)
            {
                string validationException = GenericExceptionMessage.GetValidCustomExeptionMessage(valEx, executionContext);
                log.Error(validationException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = validationException });
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
