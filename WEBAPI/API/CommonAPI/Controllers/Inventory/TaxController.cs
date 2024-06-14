/********************************************************************************************
 * Project Name - Products
 * Description  - Created to fetch, update and insert Tax details in the product->Tax --> TaxSet entity.
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 *********************************************************************************************
 *2.60        30-Jan-2019   Mushahid Faizan    Created
 *2.60        17-Mar-2019   Manoj Durgam       Added ExecutionContext to the constructor 
 *2.60        21-Mar-2019   Nagesh Badiger     Added ExecutionContext and added Custom Generic Exception 
 *2.100.0     04-Oct-2020  Mushahid Faizan    Modified: as per API Standards, namespace changes, endPoint Changes, added searchParameters in get(),
 *                                            Renamed Controller from TaxSetup to PurchaseTaxController
 *2.110.0    23-Nov-2020   Mushahid Faizan         Web Inventory UI resdesign changes with REST API.
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;

namespace Semnox.Parafait.Inventory
{
    public class TaxController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Gets the TaxList collection.
        /// </summary>       
        [HttpGet]
        [Authorize]
        [Route("api/Inventory/Taxes")]
        public async Task<HttpResponseMessage> Get(string isActive = null, int taxId = -1, string taxName = null, int taxPercentage = -1, bool loadActiveChild = false,
                                          bool buildChildRecords = false, int currentPage = 0, int pageSize = 0)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(isActive);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<TaxDTO.SearchByTaxParameters, string>> searchParameters = new List<KeyValuePair<TaxDTO.SearchByTaxParameters, string>>();
                searchParameters.Add(new KeyValuePair<TaxDTO.SearchByTaxParameters, string>(TaxDTO.SearchByTaxParameters.SITE_ID, executionContext.GetSiteId().ToString()));

                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<TaxDTO.SearchByTaxParameters, string>(TaxDTO.SearchByTaxParameters.ACTIVE_FLAG, isActive));
                    }
                }
                if (!string.IsNullOrEmpty(taxName))
                {
                    searchParameters.Add(new KeyValuePair<TaxDTO.SearchByTaxParameters, string>(TaxDTO.SearchByTaxParameters.TAX_NAME, taxName));
                }
                if (taxId > -1)
                {
                    searchParameters.Add(new KeyValuePair<TaxDTO.SearchByTaxParameters, string>(TaxDTO.SearchByTaxParameters.TAX_ID, taxId.ToString()));
                }
                if (taxPercentage > -1)
                {
                    searchParameters.Add(new KeyValuePair<TaxDTO.SearchByTaxParameters, string>(TaxDTO.SearchByTaxParameters.TAX_PERCENTAGE, taxPercentage.ToString()));
                }

                TaxList taxList = new TaxList(executionContext);

                
                int totalNoTaxes = await Task<int>.Factory.StartNew(() => { return taxList.GetTaxCount(searchParameters, null); });
                log.LogVariableState("totalNoTaxes", totalNoTaxes);
         

                ITaxUseCases taxUseCases = InventoryUseCaseFactory.GetTaxUseCases(executionContext);
                List<TaxDTO> taxDTOList = await taxUseCases.GetTaxes(searchParameters, buildChildRecords, loadActiveChild, null, currentPage, pageSize);
                log.LogMethodExit(taxDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = taxDTOList, currentPageNo = currentPage, TotalCount = totalNoTaxes });

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
        /// Post the TaxList collection
        /// <param name="taxDTOList">TaxList</param>
        [HttpPost]
        [Route("api/Inventory/Taxes")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody]List<TaxDTO> taxDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(taxDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (taxDTOList == null || taxDTOList.Any(a => a.TaxId > 0))
                {
                    log.LogMethodExit(taxDTOList);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                ITaxUseCases taxUseCases = InventoryUseCaseFactory.GetTaxUseCases(executionContext);
                await taxUseCases.SaveTaxes(taxDTOList);
                log.LogMethodExit(taxDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = taxDTOList });
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
        /// Post the TaxList collection
        /// <param name="taxDTOList">TaxList</param>
        [HttpPut]
        [Route("api/Inventory/Taxes")]
        [Authorize]
        public async Task<HttpResponseMessage> Put([FromBody]List<TaxDTO> taxDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(taxDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (taxDTOList == null || taxDTOList.Any(a => a.TaxId < 0))
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                ITaxUseCases taxUseCases = InventoryUseCaseFactory.GetTaxUseCases(executionContext);
                await taxUseCases.SaveTaxes(taxDTOList);
                log.LogMethodExit(taxDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = taxDTOList });
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
        /// Delete the TaxList collection
        /// <param name="taxDTOList">TaxList</param>
        [HttpDelete]
        [Route("api/Inventory/Taxes")]
        [Authorize]
        public async Task<HttpResponseMessage> Delete([FromBody]List<TaxDTO> taxDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(taxDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (taxDTOList != null && taxDTOList.Any())
                {
                    ITaxUseCases taxUseCases = InventoryUseCaseFactory.GetTaxUseCases(executionContext);
                    await taxUseCases.DeleteTaxes(taxDTOList);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = string.Empty });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
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
