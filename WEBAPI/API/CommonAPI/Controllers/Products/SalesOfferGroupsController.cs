/********************************************************************************************
 * Project Name - Products Controller
 * Description  - Created to fetch, update and insert offer groups in the product entity.   
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By       Remarks          
 *********************************************************************************************
 *2.60       21-Jan-2019   Indrajith          Created to get, insert, update and Delete Methods.
 *2.60       17-Mar-2019   Manoj Durgam      Added ExecutionContext to the constructor
 *2.60       21-Mar-2019   Nagesh Badiger    Added Custom Generic Exception 
 *2.110.0     10-Sep-2020   Girish Kundar      Modified :  REST API Standards.
 *2.120.00    11-Mar-2021   Roshan Devadiga     Modified Get,Post and Added Put method
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
    public class SalesOfferGroupsController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// Get the JSON SalesOfferGroups
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Product/SalesOfferGroups")]
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

                List<KeyValuePair<SalesOfferGroupDTO.SearchBySalesOfferGroupParameters, string>> searchParameters = new List<KeyValuePair<SalesOfferGroupDTO.SearchBySalesOfferGroupParameters, string>>();
                searchParameters.Add(new KeyValuePair<SalesOfferGroupDTO.SearchBySalesOfferGroupParameters, string>(SalesOfferGroupDTO.SearchBySalesOfferGroupParameters.SITE_ID, Convert.ToString(securityTokenDTO.SiteId)));

                if (isActive.ToString() == "1")
                {
                    searchParameters.Add(new KeyValuePair<SalesOfferGroupDTO.SearchBySalesOfferGroupParameters, string>(SalesOfferGroupDTO.SearchBySalesOfferGroupParameters.IS_ACTIVE, isActive));
                }
                ISalesOfferGroupUseCases salesOfferGroupUseCases = ProductsUseCaseFactory.GetSalesOfferGroups(executionContext);
                List<SalesOfferGroupDTO> salesOfferGroupsList = await salesOfferGroupUseCases.GetSalesOfferGroups(searchParameters);
                log.LogMethodExit(salesOfferGroupsList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = salesOfferGroupsList });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }

        /// <summary>
        /// Post the JSON Object SalesOfferGroups
        /// </summary>
        /// <param name="salesOfferGroupsList">salesOfferGroupsList</param>
        /// <returns>HttpResponseMessage</returns>
        /// 

        [HttpPost]
        [Route("api/Product/SalesOfferGroups")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody]List<SalesOfferGroupDTO> salesOfferGroupsList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(salesOfferGroupsList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (salesOfferGroupsList == null )
                {
                    log.LogMethodExit(salesOfferGroupsList);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }

                ISalesOfferGroupUseCases salesOfferGroupUseCases = ProductsUseCaseFactory.GetSalesOfferGroups(executionContext);
                await salesOfferGroupUseCases.SaveSalesOfferGroups(salesOfferGroupsList);
                log.LogMethodExit(salesOfferGroupsList);
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
        /// Post the SalesOfferGroupDTO collection
        /// <param name="salesOfferGroupsList">SalesOfferGroupDTO</param>
        [HttpPut]
        [Route("api/Product/SalesOfferGroups")]
        [Authorize]
        public async Task<HttpResponseMessage> Put([FromBody]List<SalesOfferGroupDTO> salesOfferGroupsList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(salesOfferGroupsList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (salesOfferGroupsList == null || salesOfferGroupsList.Any(a => a.SaleGroupId < 0))
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                ISalesOfferGroupUseCases salesOfferGroupUseCases = ProductsUseCaseFactory.GetSalesOfferGroups(executionContext);
                await salesOfferGroupUseCases.SaveSalesOfferGroups(salesOfferGroupsList);
                log.LogMethodExit(salesOfferGroupsList);
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
        /// Delete the salesOfferGroups Modifiers
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpDelete]
        [Route("api/Product/SalesOfferGroups")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody]List<SalesOfferGroupDTO> salesOfferGroupsList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(salesOfferGroupsList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));


                if (salesOfferGroupsList != null && salesOfferGroupsList.Any())
                {
                    SalesOfferGroupList salesOfferGroup = new SalesOfferGroupList(executionContext, salesOfferGroupsList);
                    salesOfferGroup.SaveUpdateSalesOfferGroupsList();
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