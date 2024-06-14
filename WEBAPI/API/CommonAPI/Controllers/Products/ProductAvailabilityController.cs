/********************************************************************************************
 * Project Name - ProductAvailabilityController
 * Description  - Created to fetch, update and insert ProductAvailability details
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By              Remarks          
 *********************************************************************************************
 *2.110.0     10-Sep-2020   Girish Kundar           Created
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Product;

namespace Semnox.CommonAPI.Products.Controllers.Products
{
    public class ProductAvailabilityController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON ProductType 
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Product/AvailableProducts")]
        public HttpResponseMessage Get(string isActive = null, bool searchUnavailableProduct = false)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(isActive, searchUnavailableProduct);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                List<ProductsDisplayGroupDTO> productsOfExcludedDisplayGroups = null;
                ProductDisplayGroupList displayGroupList = new ProductDisplayGroupList(executionContext);
                List<ProductDisplayGroupFormatDTO>  displayFormatGroups = displayGroupList.GetConfiguredDisplayGroupListForLogin(securityTokenDTO.LoginId);

                ProductsDisplayGroupList productsByDisplayGroupList = new ProductsDisplayGroupList(executionContext);
                List<KeyValuePair<ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters, string>> displayGroupSearchParameters
                    = new List<KeyValuePair<ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters, string>>();
                List<ProductsDisplayGroupDTO> productsByDisplayGroups = productsByDisplayGroupList.GetAllProductsDisplayGroup(displayGroupSearchParameters);

                if (displayFormatGroups != null)
                {
                    productsOfExcludedDisplayGroups = productsByDisplayGroups.Where(x => !displayFormatGroups.Any(y => y.Id == x.DisplayGroupId)).ToList();
                    List<ProductsDisplayGroupDTO> productsOfincludedDisplayGroups = productsByDisplayGroups.Where(x => displayFormatGroups.Any(y => y.Id == x.DisplayGroupId)).ToList();
                    productsOfExcludedDisplayGroups = productsByDisplayGroups.Where(x => !productsOfincludedDisplayGroups.Any(y => y.ProductId == x.ProductId)).ToList();
                }

                ProductsAvailabilityListBL pAListBL = new ProductsAvailabilityListBL(executionContext);
                List<ProductsAvailabilityDTO> productsAvailabilityDTOList = pAListBL.GetAvailableProductsList(productsOfExcludedDisplayGroups);
                List<ProductsAvailabilityDTO> unAvailableProducts = pAListBL.GetUnAvailableProductsList(productsAvailabilityDTOList, productsOfExcludedDisplayGroups);
                productsAvailabilityDTOList = productsAvailabilityDTOList.Where(x => !unAvailableProducts.Any(y => y.ProductId == x.ProductId)).ToList();

                if(searchUnavailableProduct == false)
                {
                    unAvailableProducts = new List<ProductsAvailabilityDTO>();
                }
                log.LogMethodExit(productsAvailabilityDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = productsAvailabilityDTOList , UnAvailableProducts  = unAvailableProducts });
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

        /// <summary>
        /// Post the JSON ProductsAvailabilityDTO.
        /// </summary>
        /// <param name="productsAvailabilityDTOList"></param>
        /// <returns>HttpMessage</returns>
        [HttpPost]
        [Route("api/Product/AvailableProducts")]
        [Authorize]
        public HttpResponseMessage Post([FromBody]List<ProductsAvailabilityDTO> productsAvailabilityDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(productsAvailabilityDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (productsAvailabilityDTOList != null && productsAvailabilityDTOList.Any())
                {
                    ProductsAvailabilityListBL productsAvailabilityBL = new ProductsAvailabilityListBL(executionContext, productsAvailabilityDTOList);
                    List<ValidationError> errorsList = productsAvailabilityBL.Save(securityTokenDTO.LoginId);
                    if (errorsList != null && errorsList.Count > 0)
                    {
                        String message = string.Join("-", errorsList);
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext,2053));
                    }
                    
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "" });
                }
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
