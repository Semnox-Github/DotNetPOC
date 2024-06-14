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
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Product
{
    public class TaxSetupController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SecurityTokenBL securityTokenBL = new SecurityTokenBL();
        private ExecutionContext executionContext;
        private SecurityTokenDTO securityTokenDTO;

        /// <summary>
        /// Gets the TaxList collection.
        /// </summary>       
        [HttpGet]
        [Authorize]
        [Route("api/Products/TaxSetup/")]
        public HttpResponseMessage Get(string isActive)
        {
            try
            {
                log.LogMethodEntry(isActive);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<TaxDTO.SearchByTaxParameters, string>> searchParameters = new List<KeyValuePair<TaxDTO.SearchByTaxParameters, string>>();
                searchParameters.Add(new KeyValuePair<TaxDTO.SearchByTaxParameters, string>(TaxDTO.SearchByTaxParameters.SITE_ID, Convert.ToString(securityTokenDTO.SiteId)));
                bool loadActiveRecords = false;
                if (isActive == "1")
                {
                    loadActiveRecords = true;
                    searchParameters.Add(new KeyValuePair<TaxDTO.SearchByTaxParameters, string>(TaxDTO.SearchByTaxParameters.ACTIVE_FLAG, isActive));
                }
                TaxList taxList = new TaxList(executionContext);
                List<TaxDTO> taxDTOList = taxList.GetAllTaxes(searchParameters, true, loadActiveRecords);               
                log.LogMethodExit(taxDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = taxDTOList, token = securityTokenDTO.Token });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, token = securityTokenDTO.Token });
            }
        }
        /// <summary>
        /// Post the TaxList collection
        /// <param name="taxDTOList">TaxList</param>
        [HttpPost]
        [Route("api/Products/TaxSetup/")]
        [Authorize]
        public HttpResponseMessage Post([FromBody]List<TaxDTO> taxDTOList)
        {
            try
            {
                log.LogMethodEntry(taxDTOList);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (taxDTOList != null || taxDTOList.Count != 0)
                {
                    TaxList taxList = new TaxList(executionContext, taxDTOList);
                    taxList.SaveUpdateTaxList();
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "", token = securityTokenDTO.Token });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { data = "NotFound", token = securityTokenDTO.Token });
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
        /// Delete the TaxList collection
        /// <param name="taxDTOList">TaxList</param>
        [HttpDelete]
        [Route("api/Products/TaxSetup/")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody]List<TaxDTO> taxDTOList)
        {
            try
            {
                log.LogMethodEntry(taxDTOList);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (taxDTOList != null || taxDTOList.Count != 0)
                {
                    TaxList taxList = new TaxList(executionContext, taxDTOList);
                    taxList.DeleteTaxList();
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "", token = securityTokenDTO.Token });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { data = "NotFound", token = securityTokenDTO.Token });
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
