/********************************************************************************************
 * Project Name - PriceList Controller
 * Description  - Created to fetch, update and insert Price List in the Product Setup.
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.60        6-Feb-2019   Indrajeet Kumar          Created to get, insert, update and Delete Methods.
 **********************************************************************************************
 *2.60        20-Mar-2019   Akshay Gulaganji        added isActive check in Get(), customGenericException
 *2.70        29-Jun-2019   Akshay Gulaganji        modified Delete() method
 *2.110.0     10-Sep-2020   Vikas Dwivedi           Modified as per the REST API Standards.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.PriceList;

namespace Semnox.CommonAPI.Products
{
    public class PriceListController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Price List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Product/PriceLists")]
        public HttpResponseMessage Get(int priceListId = -1, string isActive = null, bool loadActiveRecordsOnly = false)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(priceListId, isActive, loadActiveRecordsOnly);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<PriceListDTO.SearchByPriceListParameters, string>> searchParameters = new List<KeyValuePair<PriceListDTO.SearchByPriceListParameters, string>>();
                searchParameters.Add(new KeyValuePair<PriceListDTO.SearchByPriceListParameters, string>(PriceListDTO.SearchByPriceListParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (priceListId > 0)
                {
                    searchParameters.Add(new KeyValuePair<PriceListDTO.SearchByPriceListParameters, string>(PriceListDTO.SearchByPriceListParameters.PRICE_LIST_ID, priceListId.ToString()));
                }
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<PriceListDTO.SearchByPriceListParameters, string>(PriceListDTO.SearchByPriceListParameters.IS_ACTIVE, isActive));
                    }
                }
                PriceListList priceListList = new PriceListList(executionContext);
                List<PriceListDTO> priceListDTOList = priceListList.GetAllPriceListProducts(searchParameters, loadActiveRecordsOnly);
                log.LogMethodExit(priceListDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = priceListDTOList });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        /// <summary>
        /// Post the JSON Price List
        /// </summary>
        /// <param name="priceListList"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Product/PriceLists")]
        [Authorize]
        public HttpResponseMessage Post([FromBody]List<PriceListDTO> priceListList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(priceListList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (priceListList != null && priceListList.Count != 0)
                {
                    PriceListList priceListBLObj = new PriceListList(executionContext, priceListList);
                    priceListBLObj.SaveUpdatePriceList();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });
                }
                else
                {
                    log.LogMethodExit(null);
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
        /// Delete the JSON Price List
        /// </summary>
        /// <param name="priceListList"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("api/Product/PriceLists")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody]List<PriceListDTO> priceListList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(priceListList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (priceListList != null && priceListList.Any())
                {
                    PriceListList priceListBLObj = new PriceListList(executionContext, priceListList);
                    priceListBLObj.DeletePriceListList();
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