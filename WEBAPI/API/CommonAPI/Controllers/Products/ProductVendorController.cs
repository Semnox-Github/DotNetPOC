/********************************************************************************************
 * Project Name - Product Vendor Controller
 * Description  - Created to fetch, update and insert in the Product Vendor entity.
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.60.2     13-Jun-2019   Nagesh Badiger           Created 
 ***************************************************************************************************/

using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Vendor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Semnox.CommonAPI.Games.Controllers.Products
{
    public class ProductVendorController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SecurityTokenBL securityTokenBL = new SecurityTokenBL();
        private ExecutionContext executionContext;
        private SecurityTokenDTO securityTokenDTO;

        /// <summary>
        /// Get the Product Vendor.
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Products/ProductVendor/")]
        public HttpResponseMessage Get()
        {
            try
            {
                log.LogMethodEntry();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                List<KeyValuePair<VendorDTO.SearchByVendorParameters, string>> searchParameters = new List<KeyValuePair<VendorDTO.SearchByVendorParameters, string>>();
                searchParameters.Add(new KeyValuePair<VendorDTO.SearchByVendorParameters, string>(VendorDTO.SearchByVendorParameters.SITEID, Convert.ToString(securityTokenDTO.SiteId)));                
                VendorList vendorList = new VendorList(executionContext);
                var content = vendorList.GetAllVendors(searchParameters);
                log.LogMethodExit(content);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content, token = securityTokenDTO.Token });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, token = securityTokenDTO.Token });
            }
        }

        /// <summary>
        /// Post the JSON Object Product Vendor
        /// </summary>
        /// <param name="vendorDTOList"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Products/ProductVendor/")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<VendorDTO> vendorDTOList)
        {
            try
            {
                log.LogMethodEntry(vendorDTOList);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (vendorDTOList != null && vendorDTOList.Count != 0)
                {
                    VendorList vendorList = new VendorList(executionContext, vendorDTOList);
                    vendorList.Save();
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "", token = securityTokenDTO.Token });
                }
                else
                {
                    log.LogMethodExit(null);
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { data = "", token = securityTokenDTO.Token });
                }
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, token = securityTokenDTO.Token });
            }
        }

        /// <summary>
        /// Delete the Product Vendor
        /// </summary>
        /// <param name="vendorDTOList"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("api/Products/ProductVendor/")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody]List<VendorDTO> vendorDTOList)
        {
            try
            {
                log.LogMethodEntry(vendorDTOList);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (vendorDTOList != null && vendorDTOList.Count != 0)
                {
                    VendorList vendorList = new VendorList(executionContext, vendorDTOList);
                    vendorList.Save();
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "", token = securityTokenDTO.Token });
                }
                else
                {
                    log.LogMethodExit(null);
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { data = "", token = securityTokenDTO.Token });
                }
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
