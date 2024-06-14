/********************************************************************************************
 * Project Name - Products Controller/ProductDisplayGroupFormat/
 * Description  - Created to fetch product details in the Attraction Plays.   
 *  
 **************
 **Version Log
 **************
 *Version     Date          Created By               Remarks          
 ***************************************************************************************************
 *2.60      4-Feb-2019      Nagesh Badiger          Created to get.
 ****************************************************************************************************
 *2.70      18-Mar-2019     Akshay Gulaganji        Added Post()
 *2.70      01-Jul-2019     Akshay Gulaganji        Added Delete method and modified Get() method
 ***************************************************************************************************/


using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Semnox.CommonAPI.Products
{
    public class FormatDisplayGroupsController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private SecurityTokenDTO securityTokenDTO;
        private SecurityTokenBL securityTokenBL = new SecurityTokenBL();

        /// <summary>
        /// Get the ProductDisplayGroupFormat.
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Product/FormatDisplayGroups")]
        public HttpResponseMessage Get(string isActive = null)
        {
            try
            {
                log.LogMethodEntry(isActive);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                List<KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>> searchParameters = new List<KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>>();
                if (!string.IsNullOrEmpty(isActive) && isActive == "1")
                {
                    searchParameters.Add(new KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>(ProductDisplayGroupFormatDTO.SearchByDisplayParameters.IS_ACTIVE, isActive));
                }
                searchParameters.Add(new KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>(ProductDisplayGroupFormatDTO.SearchByDisplayParameters.SITE_ID, executionContext.GetSiteId().ToString()));

                ProductDisplayGroupList productDisplayGroupList = new ProductDisplayGroupList(executionContext);
                var content = productDisplayGroupList.GetAllProductDisplayGroup(searchParameters);
                if(content != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = content.OrderBy(m => m.SortOrder)  });
                }                
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = content  });
                }
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException  });
            }
        }

        /// <summary>
        /// Post ProductDisplayGroupFormatDTOList collection
        /// </summary>
        /// <param name="productDisplayGroupFormatDTOList">productDisplayGroupFormatDTOList</param>        
        [HttpPost]
        [Route("api/Product/FormatDisplayGroups")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<ProductDisplayGroupFormatDTO> productDisplayGroupFormatDTOList)
        {
            try
            {
                log.LogMethodEntry(productDisplayGroupFormatDTOList);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (productDisplayGroupFormatDTOList != null && productDisplayGroupFormatDTOList.Count != 0)
                {
                    ProductDisplayGroupList productDisplayGroupList = new ProductDisplayGroupList(executionContext, productDisplayGroupFormatDTOList);
                    productDisplayGroupList.Save();
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = ""  });
                }
                else
                {
                    log.LogMethodExit(null);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = ""  });
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
        /// <summary>
        /// Delete the Display Group Format
        /// </summary>
        /// <param name="productDisplayGroupFormatDTOList">productDisplayGroupList</param>
        /// <returns>HttpMessage</returns>
        [HttpDelete]
        [Route("api/Product/FormatDisplayGroups")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody]List<ProductDisplayGroupFormatDTO> productDisplayGroupFormatDTOList)
        {
            try
            {
                log.LogMethodEntry(productDisplayGroupFormatDTOList);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (productDisplayGroupFormatDTOList != null && productDisplayGroupFormatDTOList.Any())
                {
                    ProductDisplayGroupList productDisplay = new ProductDisplayGroupList(executionContext, productDisplayGroupFormatDTOList);
                    productDisplay.DeleteProductDisplayGroupFormatList();
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = ""  });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = ""  });
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
