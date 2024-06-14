/********************************************************************************************
 * Project Name - SetupSpecialPricingOptions Controller
 * Description  - Created to fetch, update and insert SetupSpecialPricingOptions in the Setup.
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By               Remarks          
 *********************************************************************************************
 *2.60        14-Feb-2019   Indrajeet Kumar           Created to get, insert, update and Delete Methods.
 *2.70        29-Jun-2019   Akshay Gulaganji          modified Delete() method
 *2.110.0     10-Sep-2020   Girish Kundar      Modified :  REST API Standards.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;

namespace Semnox.CommonAPI.Products
{
    public class SpecialPricingOptionsController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
     
        /// <summary>
        /// Get the JSON Special Pricing Options
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Product/SpecialPricingOptions")]
        public HttpResponseMessage Get(string activeFlag)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(activeFlag);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                List<KeyValuePair<SpecialPricingOptionsDTO.SearchBySpecialPricingOptionsParameters, string>> searchParameters = new List<KeyValuePair<SpecialPricingOptionsDTO.SearchBySpecialPricingOptionsParameters, string>>();
                searchParameters.Add(new KeyValuePair<SpecialPricingOptionsDTO.SearchBySpecialPricingOptionsParameters, string>(SpecialPricingOptionsDTO.SearchBySpecialPricingOptionsParameters.SITE_ID, Convert.ToString(securityTokenDTO.SiteId)));
                bool loadActiveChildRecords = false;
                if (activeFlag == "1")
                {
                    loadActiveChildRecords = true;
                    searchParameters.Add(new KeyValuePair<SpecialPricingOptionsDTO.SearchBySpecialPricingOptionsParameters, string>(SpecialPricingOptionsDTO.SearchBySpecialPricingOptionsParameters.ACTIVE_FLAG, activeFlag));
                }
                SpecialPricingOptionsBLList specialPricingOptionsBLList = new SpecialPricingOptionsBLList(executionContext);
                var content = specialPricingOptionsBLList.GetSpecialPricingOptionsList(searchParameters, true, loadActiveChildRecords);
                log.LogMethodExit(content);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content });
            }
            catch (ValidationException valEx)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(valEx, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ex.Message });
            }
        }

        /// <summary>
        /// Post the JSON Special Pricing Options.
        /// </summary>
        /// <param name="specialPricingOptionsList"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Product/SpecialPricingOptions")]
        [Authorize]
        public HttpResponseMessage Post([FromBody]List<SpecialPricingOptionsDTO> specialPricingOptionsList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(specialPricingOptionsList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (specialPricingOptionsList != null)
                {
                    SpecialPricingOptionsBLList specialPricingOptionsBLList = new SpecialPricingOptionsBLList(executionContext, specialPricingOptionsList);
                    specialPricingOptionsBLList.SaveUpdateSpecialPricingOptionsList();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = " " });
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
                log.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ex.Message });
            }
        }

        /// <summary>
        /// Delete the JSON Special Pricing Options.
        /// </summary>
        /// <param name="specialPricingOptionsList"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("api/Product/SpecialPricingOptions")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody]List<SpecialPricingOptionsDTO> specialPricingOptionsList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(specialPricingOptionsList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (specialPricingOptionsList != null)
                {
                    SpecialPricingOptionsBLList specialPricingOptionsBLList = new SpecialPricingOptionsBLList(executionContext, specialPricingOptionsList);
                    specialPricingOptionsBLList.DeleteSpecialPricingOptionsList();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = " " });
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
                log.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = ex.Message });
            }
        }
    }
}
