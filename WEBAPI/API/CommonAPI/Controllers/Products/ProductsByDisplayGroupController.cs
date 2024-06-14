/********************************************************************************************
 * Project Name - ProductsByDisplayGroupController
 * Description  - Created to fetch product by display groups 
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By           Remarks          
 *2.60        03-Apr-2019    Nitin Pai             Created 
 *2.110.0     21-Nov-2019   Girish Kundar       Modified :  REST API changes for Inventory UI redesign
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Semnox.Parafait.Product;
using System.Web.Http;
using Semnox.Core.Utilities;
using System.Web;
using Semnox.Core.GenericUtilities;

namespace Semnox.CommonAPI.Controllers.Products
{
    public class ProductsByDisplayGroupController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON products list.
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Product/ProductsByDisplayGroup")]
        public HttpResponseMessage Get(int siteId, int displayGroupId, bool requiresCard, bool newCard)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(displayGroupId);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                int selectedSite = siteId == -1 ? securityTokenDTO.SiteId : siteId;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, selectedSite, -1, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                ProductsFilterParams productsFilterParams = new ProductsFilterParams();
                productsFilterParams.IsActive = true;
                productsFilterParams.SiteId = selectedSite;
                productsFilterParams.DateOfPurchase = DateTime.Today;
                productsFilterParams.ProductId = -1;
                productsFilterParams.DisplayGroupId = displayGroupId;
                productsFilterParams.RequiresCardProduct = requiresCard;
                productsFilterParams.NewCard = newCard;

                ProductsList productList = new ProductsList(executionContext);
                List<ProductsDTO> productsList = productList.GetProductListByFilterparams(productsFilterParams);
                if (productsList != null && productsList.Count > 0)
                {
                    log.LogMethodExit(productsList);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = productsList  });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ""  });
                }

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
