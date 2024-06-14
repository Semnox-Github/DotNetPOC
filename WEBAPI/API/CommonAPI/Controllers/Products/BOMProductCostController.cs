/********************************************************************************************
 * Project Name - Products
 * Description  - Created to fetch inventorydetail BOM cost in the product details entity.   
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By               Remarks          
 *********************************************************************************************
 *2.70.0      02-July-2019  Jagan Mohana              Created
 *2.110.0     10-Sep-2020   Girish Kundar             Modified as per the REST API Standards.
 ********************************************************************************************/


using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;

namespace Semnox.CommonAPI.Controllers.Inventory
{
    public class InventoryDetailsBuildFromBOMController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
       
        /// <summary>
        /// Gets the Jason Object of Inventory details BOM build cost.
        /// </summary>
        /// <param name="productId">productId</param>        
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("api/Inventory/BOMProductCost")]
        public HttpResponseMessage Get(int productId)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(productId);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                ProductList productList = new ProductList();
                List<KeyValuePair<ProductDTO.SearchByProductParameters, string>> SearchParameter = new List<KeyValuePair<ProductDTO.SearchByProductParameters, string>>();
                if (productId > -1)
                {
                    SearchParameter.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.PRODUCT_ID, Convert.ToString(productId)));
                }

                List<ProductDTO> products = productList.GetAdancedAllProducts(SearchParameter);
                string content = string.Empty;
                if (products != null)
                {
                    object bomCost = productList.GetBOMProductCost(productId);
                    if (bomCost != DBNull.Value)
                    {                        
                        content = Convert.ToDecimal(bomCost).ToString(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "INVENTORY_COST_FORMAT"));
                    }
                }
                log.LogMethodExit(content);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content  });
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
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, token = securityTokenDTO.Token });
            }
        }
    }
}
