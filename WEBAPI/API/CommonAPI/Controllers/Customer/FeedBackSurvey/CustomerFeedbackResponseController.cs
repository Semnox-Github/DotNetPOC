/********************************************************************************************
 * Project Name - Customer                                                                         
 * Description  - Controller of the CustomerFeedbackResponse class
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
*2.80        24-Feb-2020   Mushahid Faizan        Created 
*2.120.00    04-May-2021   Roshan Devadiga        Modified Get,Post and Added Put method
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Semnox.Core.Utilities;
using System.Web;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Customer;
using System.Linq;
using System.Threading.Tasks;
using Semnox.CommonAPI.Helpers;

namespace Semnox.CommonAPI.Customer.FeedbackSurvey
{
    public class CustomerFeedbackResponseController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly SecurityTokenBL securityTokenBL = new SecurityTokenBL();
        private SecurityTokenDTO securityTokenDTO = null;
        private ExecutionContext executionContext;

        /// <summary>
        /// Get the JSON Object Customer Feedback Response List
        /// </summary>        
        /// <returns>HttpMessage</returns>
        [HttpGet]
        [Route("api/Customer/FeedbackSurvey/FeedbackResponses")]
        [Authorize]
        public async Task<HttpResponseMessage> Get(string isActive = null, bool loadActiveChild = false, string responseName = null, bool buildChildRecords = false)
        {
            try
            {
                log.LogMethodEntry(isActive, loadActiveChild, responseName, buildChildRecords);
                securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                List<KeyValuePair<CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters, string>> searchByCustomerFeedbackResponseParameters = new List<KeyValuePair<CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters, string>>();
                searchByCustomerFeedbackResponseParameters.Add(new KeyValuePair<CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters, string>(CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        loadActiveChild = true;
                        searchByCustomerFeedbackResponseParameters.Add(new KeyValuePair<CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters, string>(CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters.IS_ACTIVE, isActive));
                    }
                }
                if (!string.IsNullOrEmpty(responseName))
                {
                    searchByCustomerFeedbackResponseParameters.Add(new KeyValuePair<CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters, string>(CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters.RESPONSE_NAME, responseName));
                }

                ICustomerFeedbackResponseUseCases customerFeedbackResponseUseCases = CustomerUseCaseFactory.GetCustomerFeedbackResponseUseCases(executionContext);
                List<CustomerFeedbackResponseDTO> customerFeedbackResponseList = await customerFeedbackResponseUseCases.GetCustomerFeedbackResponses(searchByCustomerFeedbackResponseParameters, buildChildRecords, loadActiveChild, null);
                log.LogMethodExit(customerFeedbackResponseList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = customerFeedbackResponseList });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }
        /// <summary>
        /// Post the JSON Object Customer Feedback Response List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/Customer/FeedbackSurvey/FeedbackResponses")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody] List<CustomerFeedbackResponseDTO> customerFeedbackResponseDTOList)
        {
            try
            {
                log.LogMethodEntry(customerFeedbackResponseDTOList);
                securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (customerFeedbackResponseDTOList == null)
                {
                    log.LogMethodExit(customerFeedbackResponseDTOList);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                ICustomerFeedbackResponseUseCases customerFeedbackResponseUseCases = CustomerUseCaseFactory.GetCustomerFeedbackResponseUseCases(executionContext);
                await customerFeedbackResponseUseCases.SaveCustomerFeedbackResponses(customerFeedbackResponseDTOList);
                log.LogMethodExit(customerFeedbackResponseDTOList);
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
        /// Put
        /// </summary>
        /// <param name="customerFeedbackResponseDTOList"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/Customer/FeedbackSurvey/FeedbackResponses")]
        [Authorize]
        public async Task<HttpResponseMessage> Put([FromBody]List<CustomerFeedbackResponseDTO> customerFeedbackResponseDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(customerFeedbackResponseDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (customerFeedbackResponseDTOList == null || customerFeedbackResponseDTOList.Any(a => a.CustFbResponseId < 0))
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                ICustomerFeedbackResponseUseCases customerFeedbackResponseUseCases = CustomerUseCaseFactory.GetCustomerFeedbackResponseUseCases(executionContext);
                await customerFeedbackResponseUseCases.SaveCustomerFeedbackResponses(customerFeedbackResponseDTOList);
                log.LogMethodExit(customerFeedbackResponseDTOList);
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
