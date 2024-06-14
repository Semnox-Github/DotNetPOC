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
 ****************************************************************************************************/

using GenCode128;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Printer;
using Semnox.Parafait.Product;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;

namespace Semnox.CommonAPI.Products
{
    public class GenerateUPCBarcodeController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly SecurityTokenBL securityTokenBL = new SecurityTokenBL();
        private SecurityTokenDTO securityTokenDTO;
        private ExecutionContext executionContext;
        /// <summary>
        /// Genrates Barcodes for products inventory details generate upc barcode
        /// Convert a bitmap image to memorystream and send the response
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>        
        [HttpGet]
        [Route("api/Products/GenerateUPCBarcode/")]
        [Authorize]
        public HttpResponseMessage Get(int productId, int preferredVendor, string productCode)
        {
            try
            {
                log.LogMethodEntry(productId, preferredVendor, productCode);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                                
                ProductBL productBL = new ProductBL(executionContext, productId, false, false);
                productBL.GenerateUPCBarCode(productId, preferredVendor, productCode);
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
