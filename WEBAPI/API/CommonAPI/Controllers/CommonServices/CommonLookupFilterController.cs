/********************************************************************************************
 * Project Name - CommonLookupController Controller                                                                         
 * Description  - Controller for the Mater Data tables all modules
 *                It will filter the details lookups based on the dynamic properties for the parent to child entity lookups
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.60        20-Nov-2018    Jagan Mohana Rao    Created 
 *2.60        11-Apr-2019    Akshay Gulaganji    modified Get method
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Semnox.Core.Utilities;
using System.Linq;
using Semnox.Parafait.Product;
using Semnox.Core.GenericUtilities;
using Semnox.CommonAPI.Lookups;

namespace Semnox.CommonAPI.CommonServices
{
    public class CommonLookupFilterController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        
        /// <summary>
        /// Get method to fetch all the master table values for all modules
        /// </summary>
        /// <param name="moduleName"></param>
        /// <param name="entityName"></param>
        /// <param name="keyValuePairsString"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("api/CommonServices/CommonLookupFilter/")]
        public HttpResponseMessage Get([FromUri] string moduleName, string entityName,string keyValuePair)
        {
            // keyValuePairsString = "{\"ProductId\":\"118\",\"ProductTypeId\":\"56\"}";
            // string keyValuePairsString = "{\"InvProductCode\":\"001\"}";
            SecurityTokenDTO securityTokenDTO = null;
            try
            {
                log.LogMethodEntry(moduleName, entityName, keyValuePair);
                string exisitingToken = null;
                IEnumerable<string> authzHeaders;
                if (Request.Headers.TryGetValues("Authorization", out authzHeaders) || authzHeaders.Count() > 1)
                {
                    var bearerToken = authzHeaders.ElementAt(0);
                    exisitingToken = bearerToken.StartsWith("Bearer ") ? bearerToken.Substring(7) : bearerToken;
                }

                SecurityTokenBL securityTokenBL = new SecurityTokenBL(exisitingToken);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
               
                if (moduleName != null && entityName != null)
                {
                    log.Debug("CommonLookupController-Get() Method.");
                    List<CommonLookupsDTO> lookups = new List<CommonLookupsDTO>();
                    string isActive = string.Empty;
                    switch (moduleName.ToUpper().ToString())
                    {
                        case "PRODUCTS":
                            isActive = "1";
                            ProductsLookupBL productLookupBL = new ProductsLookupBL(entityName, executionContext, keyValuePair);                            
                            lookups = productLookupBL.GetLookupFilteredList();
                            break;
                        case "SITESETUP":
                            isActive = "1";
                            SiteSetupLookupBL siteSetupLookupBL = new SiteSetupLookupBL(entityName, executionContext, keyValuePair);                            
                            lookups = siteSetupLookupBL.GetLookupFilteredList();
                            break;
                    }
                    log.LogMethodExit(lookups);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = lookups, token = securityTokenDTO.Token });
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
