/********************************************************************************************
 * Project Name - Products
 * Description  - Created to fetch, update and insert product exclusion pos management counters in the product details.
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By                Remarks          
 *********************************************************************************************
 *2.60       23-Jan-2019    Nagesh Badiger          Created to get, insert, update and Delete Methods.
 **********************************************************************************************
 *2.60       20-Mar-2019    Akshay Gulaganji        Added CustomGenericException and ExecutionContext
 *********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.POS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Semnox.CommonAPI.Products
{
    public class POSManagementCountersController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly SecurityTokenBL securityTokenBL = new SecurityTokenBL();
        private SecurityTokenDTO securityTokenDTO;
        private ExecutionContext executionContext;
        /// <summary>
        /// Get the POSManagementCounters.
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Products/POSManagementCounters/")]
        public HttpResponseMessage Get(string isActive)
        {
            try
            {
                log.LogMethodEntry(isActive);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<POSTypeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<POSTypeDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<POSTypeDTO.SearchByParameters, string>(POSTypeDTO.SearchByParameters.SITE_ID, Convert.ToString(securityTokenDTO.SiteId)));

                POSTypeListBL posTypeListBL = new POSTypeListBL(executionContext);
                if (isActive == "1")
                {
                    searchParameters.Add(new KeyValuePair<POSTypeDTO.SearchByParameters, string>(POSTypeDTO.SearchByParameters.IS_ACTIVE, isActive.ToString()));
                }
                var content = posTypeListBL.GetPOSTypeDTOList(searchParameters);

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
        /// Post the JSON Object POSManagementCounters
        /// </summary>
        /// <param name="posTypeDTOList">POSTypeListBL</param>
        /// <returns>HttpMessgae</returns>
        [HttpPost]
        [Route("api/Products/POSManagementCounters/")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<POSTypeDTO> posTypeDTOList, bool isLicensedPOSMachines = false)
        {
            try
            {
                log.LogMethodEntry(posTypeDTOList);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (posTypeDTOList != null || posTypeDTOList.Count > 0)
                {
                    POSTypeListBL poSTypeBL = new POSTypeListBL(executionContext, posTypeDTOList);
                    poSTypeBL.Save(isLicensedPOSMachines);
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "", token = securityTokenDTO.Token });
                }
                else
                {
                    log.LogMethodExit(null);
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { data = "", token = securityTokenDTO.Token });
                }
            }
            catch (ValidationException ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.Accepted, new { data = customException, token = securityTokenDTO.Token });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, token = securityTokenDTO.Token });
            }
        }

        /// <summary>
        /// Delete the POSManagement Counters
        /// </summary>        
        /// <returns>HttpMessgae</returns>
        [HttpDelete]
        [Route("api/Products/POSManagementCounters/")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody] List<POSTypeDTO> posTypeDTOList)
        {
            try
            {
                log.LogMethodEntry(posTypeDTOList);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (posTypeDTOList != null || posTypeDTOList.Count > 0)
                {
                    POSTypeListBL poSTypeBL = new POSTypeListBL(executionContext, posTypeDTOList);
                    poSTypeBL.DeletePOSTypesList();
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