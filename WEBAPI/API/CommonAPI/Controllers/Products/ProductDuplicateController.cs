/********************************************************************************************
 * Project Name - Products Controller
 * Description  - Created to insert product details in the product entity.   
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By            Remarks          
 *********************************************************************************************
 *2.70.2        28-Jun-2019   Jagan Mohana Rao     Created
 *2.110.0       21-Nov-2019   Girish Kundar       Modified :  REST API changes for Inventory UI redesign
 *********************************************************************************************/
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;
using Semnox.Core.GenericUtilities;

namespace Semnox.CommonAPI.Products
{
    public class ProductDuplicateController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON product details
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Authorize]
        [Route("api/Product/Duplicate")]
        public HttpResponseMessage Post([FromBody] ProductsDTO productsDTO)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(productsDTO);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                Semnox.Parafait.Product.Products products = new Parafait.Product.Products(executionContext);
                if (productsDTO.ProductId > -1)
                {
                    products.SaveDuplicateProductDetails(productsDTO.ProductId);
                }
                log.LogMethodExit();
                return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });
            }
            catch (ValidationException ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
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
