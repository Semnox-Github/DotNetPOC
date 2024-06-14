/********************************************************************************************
 * Project Name - CheckOutPrices Controller
 * Description  - Created to fetch, update and insert CheckOutPrices in the product Setup.
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By               Remarks          
 *********************************************************************************************
 *2.60        08-Feb-2019   Indrajeet Kumar           Created to get, insert, update and Delete Methods.
 **********************************************************************************************
 *2.60        18-Mar-2019   Akshay Gulaganji          Added ExecutionContext and CustomGenericException
 *2.110.0     10-Sep-2020   Girish Kundar             Modified as per the REST API Standards.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;

namespace Semnox.CommonAPI.Products
{
    public class CheckOutPricesController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON CheckOutPrices.
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Product/CheckOutPrices")]
        public HttpResponseMessage Get(string isActive = null, int productId = -1, int checkOutPriceId = -1)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(isActive, productId, checkOutPriceId);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                List<KeyValuePair<CheckOutPricesDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CheckOutPricesDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<CheckOutPricesDTO.SearchByParameters, string>(CheckOutPricesDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (productId > 0)
                {
                    searchParameters.Add(new KeyValuePair<CheckOutPricesDTO.SearchByParameters, string>(CheckOutPricesDTO.SearchByParameters.PRODUCT_ID, Convert.ToString(productId)));
                }
                if (checkOutPriceId > 0)
                {
                    searchParameters.Add(new KeyValuePair<CheckOutPricesDTO.SearchByParameters, string>(CheckOutPricesDTO.SearchByParameters.ID, Convert.ToString(checkOutPriceId)));
                }
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<CheckOutPricesDTO.SearchByParameters, string>(CheckOutPricesDTO.SearchByParameters.ISACTIVE, isActive));
                    }
                }
                CheckOutPricesBLList checkOutPricesBLList = new CheckOutPricesBLList(executionContext);
                List<CheckOutPricesDTO> checkOutPricesDTOList = checkOutPricesBLList.GetAllCheckOutPricesList(searchParameters);
                log.LogMethodExit(checkOutPricesDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = checkOutPricesDTOList });
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
        /// Post the JSON CheckOutPrices.
        /// </summary>
        /// <param name="checkOutPricesList"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Product/CheckOutPrices")]
        [Authorize]
        public HttpResponseMessage Post([FromBody]List<CheckOutPricesDTO> checkOutPricesList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(checkOutPricesList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (checkOutPricesList != null && checkOutPricesList.Any())
                {
                    CheckOutPricesBLList checkOutPrices = new CheckOutPricesBLList(executionContext, checkOutPricesList);
                    checkOutPrices.Save();
                    log.LogMethodExit();
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
        /// Delete the JSON CheckOutPrices.
        /// </summary>
        /// <param name="checkOutPricesList"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("api/Product/CheckOutPrices")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody]List<CheckOutPricesDTO> checkOutPricesList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(checkOutPricesList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                    
                if (checkOutPricesList != null && checkOutPricesList.Any())
                {
                    CheckOutPricesBLList checkOutPrices = new CheckOutPricesBLList(executionContext, checkOutPricesList);
                    checkOutPrices.Delete();
                    log.LogMethodExit();
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
