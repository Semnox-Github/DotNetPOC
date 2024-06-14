/********************************************************************************************
 * Project Name - Products
 * Description  - Created to fetch, update and insert offer group product map in the product module.
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 *********************************************************************************************
 *2.60        21-Jan-2019   Mushahid Faizan    Created to get, insert, update and Delete Methods.
 *2.60        17-Mar-2019   Manoj Durgam       Added ExecutionContext to the constructor
 *2.60        21-Mar-2019   Nagesh Badiger     Added ExecutionContext and added Custom Generic Exception 
 *2.110.0     10-Sep-2020   Girish Kundar      Modified :  REST API Standards.
 *2.120.00   19-Apr-2021    Roshan Devadiga     Modified Get,Post and Added Put method
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
using System.Linq;
using System.Threading.Tasks;
using Semnox.CommonAPI.Helpers;

namespace Semnox.CommonAPI.Products
{
    public class SaleGroupProductMapController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON SaleGroupProductMap.
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Product/SaleGroupProductMaps")]
        public async Task<HttpResponseMessage> Get(string isActive)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(isActive);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<SaleGroupProductMapDTO.SearchBySaleGroupProductMapParameters, string>> searchParameters = new List<KeyValuePair<SaleGroupProductMapDTO.SearchBySaleGroupProductMapParameters, string>>();
                searchParameters.Add(new KeyValuePair<SaleGroupProductMapDTO.SearchBySaleGroupProductMapParameters, string>(SaleGroupProductMapDTO.SearchBySaleGroupProductMapParameters.SITE_ID, Convert.ToString(securityTokenDTO.SiteId)));
                if (isActive.ToString() == "1")
                {
                    searchParameters.Add(new KeyValuePair<SaleGroupProductMapDTO.SearchBySaleGroupProductMapParameters, string>(SaleGroupProductMapDTO.SearchBySaleGroupProductMapParameters.IS_ACTIVE, isActive));
                }

                ISaleGroupProductMapUseCases saleGroupProductMapUseCases = ProductsUseCaseFactory.GetSaleGroupProductMapUseCases(executionContext);
                List<SaleGroupProductMapDTO> saleGroupProductMapDTOList = await saleGroupProductMapUseCases.GetSaleGroupProductMaps(searchParameters);
                log.LogMethodExit(saleGroupProductMapDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = saleGroupProductMapDTOList });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }

        /// <summary>
        /// Post the JSON Object Setup Offer Group Product Map
        /// </summary>
        /// <param name="saleGroupProductMapList">saleGroupProductMapList</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/Product/SaleGroupProductMaps")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody]List<SaleGroupProductMapDTO> saleGroupProductMapList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(saleGroupProductMapList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (saleGroupProductMapList == null )
                {
                    log.LogMethodExit(saleGroupProductMapList);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                ISaleGroupProductMapUseCases saleGroupProductMapUseCases = ProductsUseCaseFactory.GetSaleGroupProductMapUseCases(executionContext);
                await saleGroupProductMapUseCases.SaveSaleGroupProductMaps(saleGroupProductMapList);
                log.LogMethodExit(saleGroupProductMapList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = string.Empty });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }

        /// <summary>
        /// Delete the Setup Offer Group Product Map
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpDelete]
        [Route("api/Product/SaleGroupProductMaps")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody]List<SaleGroupProductMapDTO> saleGroupProductMapList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(saleGroupProductMapList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (saleGroupProductMapList != null && saleGroupProductMapList.Any())
                {
                    SaleGroupProductMapList saleGroupProductMap = new SaleGroupProductMapList(executionContext, saleGroupProductMapList);
                    saleGroupProductMap.SaveUpdateSetupOfferGroupProductMap();
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "" });
                }
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }
        /// <summary>
        /// Post the SaleGroupProductMapList collection
        /// <param name="saleGroupProductMapList">SaleGroupProductMapList</param>
        [HttpPut]
        [Route("api/Product/SaleGroupProductMaps")]
        [Authorize]
        public async Task<HttpResponseMessage> Put([FromBody]List<SaleGroupProductMapDTO> saleGroupProductMapList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(saleGroupProductMapList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (saleGroupProductMapList == null || saleGroupProductMapList.Any(a => a.TypeMapId < 0))
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                ISaleGroupProductMapUseCases saleGroupProductMapUseCases = ProductsUseCaseFactory.GetSaleGroupProductMapUseCases(executionContext);
                await saleGroupProductMapUseCases.SaveSaleGroupProductMaps(saleGroupProductMapList);
                log.LogMethodExit(saleGroupProductMapList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = string.Empty });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }

    }
}
