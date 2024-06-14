/********************************************************************************************
 * Project Name - Products Controller
 * Description  - Created to fetch, update and insert waiver signing option.
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By               Remarks          
 *********************************************************************************************
 *2.60        21-Jan-2019   Jagan Mohana Rao          Created to get, insert, update and Delete Methods.
 *2.60        22-Mar-2019   Nagesh Badiger            Added Custom Generic Exception 
 *2.110.0     10-Sep-2020    Girish Kundar           Modified :  REST API Standards.
 *2.120.00    26-Apr-2021    Roshan Devadiga          Modified Get,Post and Added Put method
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Semnox.Core.Utilities;
using System.Web;
using Semnox.Parafait.Waiver;
using Semnox.Core.GenericUtilities;
using System.Threading.Tasks;
using Semnox.CommonAPI.Helpers;
using System.Linq;

namespace Semnox.CommonAPI.Products
{
    public class WaiverSigningOptionController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SecurityTokenBL securityTokenBL = new SecurityTokenBL();
        private ExecutionContext executionContext;
        private SecurityTokenDTO securityTokenDTO;

        /// <summary>
        /// Get the JSON Waiver Signing Option
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Product/WaiverSigningOptions")]
        public async Task<HttpResponseMessage> Get(string waiverSetId)
        {
            try
            {
                log.LogMethodEntry(waiverSetId);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<WaiverSetSigningOptionsDTO.SearchByParameters, string>> searchChannelParameters = new List<KeyValuePair<WaiverSetSigningOptionsDTO.SearchByParameters, string>>();
                searchChannelParameters.Add(new KeyValuePair<WaiverSetSigningOptionsDTO.SearchByParameters, string>(WaiverSetSigningOptionsDTO.SearchByParameters.WAIVER_SET_ID, waiverSetId));
                searchChannelParameters.Add(new KeyValuePair<WaiverSetSigningOptionsDTO.SearchByParameters, string>(WaiverSetSigningOptionsDTO.SearchByParameters.SITE_ID, securityTokenDTO.SiteId.ToString()));

                IWaiverSetSigningOptionsUseCases waiverSetSigningOptionsUseCases = WaiverUseCaseFactory.GetWaiverSetSigningOptionsUseCases(executionContext);
                List<WaiverSetSigningOptionsDTO> waiverSetSigningOptionsDTOList = await waiverSetSigningOptionsUseCases.GetWaiverSetSigningOptions(searchChannelParameters);
                log.LogMethodExit(waiverSetSigningOptionsDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = waiverSetSigningOptionsDTOList });

            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }

        /// <summary>
        /// Post the JSON Object Product Discounts
        /// </summary>
        /// <param name="WaiverSetSigningOptionsDTO">WaiverSetSigningOptionsDTO</param>
        /// <returns>HttpMessgae</returns>
        [HttpPost]
        [Route("api/Product/WaiverSigningOptions")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody]List<WaiverSetSigningOptionsDTO> waiverSetSigningOptionsDTOs)
        {
            try
            {
                log.LogMethodEntry(waiverSetSigningOptionsDTOs);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (waiverSetSigningOptionsDTOs == null )
                {
                    log.LogMethodExit(waiverSetSigningOptionsDTOs);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                IWaiverSetSigningOptionsUseCases waiverSetSigningOptionsUseCases = WaiverUseCaseFactory.GetWaiverSetSigningOptionsUseCases(executionContext);
                await waiverSetSigningOptionsUseCases.SaveWaiverSetSigningOptions(waiverSetSigningOptionsDTOs);
                log.LogMethodExit(waiverSetSigningOptionsDTOs);
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
        /// Post the WaiverSetSigningOptionsDTOs collection
        /// <param name="waiverSetSigningOptionsDTOs">WaiverSetSigningOptionsDTOs</param>
        [HttpPut]
        [Route("api/Product/WaiverSigningOptions")]
        [Authorize]
        public async Task<HttpResponseMessage> Put([FromBody]List<WaiverSetSigningOptionsDTO> waiverSetSigningOptionsDTOs)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(waiverSetSigningOptionsDTOs);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (waiverSetSigningOptionsDTOs == null || waiverSetSigningOptionsDTOs.Any(a => a.Id < 0))
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                IWaiverSetSigningOptionsUseCases waiverSetSigningOptionsUseCases = WaiverUseCaseFactory.GetWaiverSetSigningOptionsUseCases(executionContext);
                await waiverSetSigningOptionsUseCases.SaveWaiverSetSigningOptions(waiverSetSigningOptionsDTOs);
                log.LogMethodExit(waiverSetSigningOptionsDTOs);
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
