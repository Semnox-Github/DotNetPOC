/********************************************************************************************
* Project Name - CommonAPI
* Description  - ProductUserGroupsController - Created to get the product user groups
*  
**************
**Version Log
**************
*Version     Date          Modified By               Remarks          
*********************************************************************************************
*2.110.00     17-Nov-2020     Abhishek              Created : As part of Inventory UI Redesign
      
********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Semnox.CommonAPI.Controllers.Products
{
    public class ProductUserGroupsController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Get the JSON Object of ProductUserGroupsDTO
        /// </summary>
        /// <param name="productUserGroupsId">productUserGroupsId</param>
        /// <param name="buildChildRecords">buildChildRecords</param>
        /// <param name="activeRecordsOnly">activeRecordsOnly</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Product/ProductUserGroups")]
        public async Task<HttpResponseMessage> Get(int productUserGroupsId = -1, bool buildChildRecords = false, string isActive = null, bool loadActiveChild = false)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(productUserGroupsId, buildChildRecords, isActive);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                List<KeyValuePair<ProductUserGroupsDTO.SearchByParameters, string>> productUserGroupsSearchParameter = new List<KeyValuePair<ProductUserGroupsDTO.SearchByParameters, string>>();
                productUserGroupsSearchParameter.Add(new KeyValuePair<ProductUserGroupsDTO.SearchByParameters, string>(ProductUserGroupsDTO.SearchByParameters.SITE_ID, Convert.ToString(executionContext.GetSiteId())));
                if (productUserGroupsId > 0)
                {
                    productUserGroupsSearchParameter.Add(new KeyValuePair<ProductUserGroupsDTO.SearchByParameters, string>(ProductUserGroupsDTO.SearchByParameters.PRODUCT_USER_GROUPS_ID, Convert.ToString(productUserGroupsId)));
                }
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        loadActiveChild = true;
                        productUserGroupsSearchParameter.Add(new KeyValuePair<ProductUserGroupsDTO.SearchByParameters, string>(ProductUserGroupsDTO.SearchByParameters.IS_ACTIVE, isActive));
                    }
                }
                IProductUserGroupsUseCases productUserGroupsUseCases = ProductUserGroupsUseCaseFactory.GetProductUserGroupsUseCases(executionContext);
                var content = await productUserGroupsUseCases.GetProductUserGroups(productUserGroupsSearchParameter, buildChildRecords, loadActiveChild);
                log.LogMethodExit(content);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        /// <summary>
        /// Post the JSON Object of ProductUserGroupsDTO List
        /// </summary>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Authorize]
        [Route("api/Product/ProductUserGroups")]
        public async Task<HttpResponseMessage> Post([FromBody] List<ProductUserGroupsDTO> productUserGroupsDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(productUserGroupsDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (productUserGroupsDTOList != null && productUserGroupsDTOList.Any())
                {
                    IProductUserGroupsUseCases productUserGroupsUseCases = ProductUserGroupsUseCaseFactory.GetProductUserGroupsUseCases(executionContext);
                    var content = await productUserGroupsUseCases.SaveProductUserGroups(productUserGroupsDTOList);
                    log.LogMethodExit(content);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = string.Empty });
                }
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
        /// Put the JSON Object of ProductUserGroupsDTO
        /// </summary>
        /// <returns>HttpResponseMessage</returns>
        [HttpPut]
        [Authorize]
        [Route("api/Product/ProductUserGroups")]
        public async Task<HttpResponseMessage> Put([FromBody] List<ProductUserGroupsDTO> productUserGroupsDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry();
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (productUserGroupsDTOList != null && productUserGroupsDTOList.Any())
                {
                    var list = productUserGroupsDTOList.Where(x => x.ProductUserGroupsId < 0).ToList();
                    if (list!=null && list.Any())
                    {
                        string message = MessageContainerList.GetMessage(executionContext, 2196, "Product User Groups", list);
                        log.LogMethodExit(null, "Throwing Exception - " + message);
                        throw new EntityNotFoundException(message);
                    }
                    IProductUserGroupsUseCases productUserGroupsUseCases = ProductUserGroupsUseCaseFactory.GetProductUserGroupsUseCases(executionContext);
                    var content = await productUserGroupsUseCases.SaveProductUserGroups(productUserGroupsDTOList);
                    log.LogMethodExit(content);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = string.Empty });
                }
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