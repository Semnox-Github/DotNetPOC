/********************************************************************************************
 * Project Name - Customer                                                                        
 * Description  - Controller of the CustomerFeedbackSurveyDetails class
 *
 **************
 **Version Log
  *Version     Date          Modified By           Remarks          
 *********************************************************************************************
 *2.80        24-Feb-2020   Mushahid Faizan          Created 
 *2.120.0     04-May-2021   B Mahesh Pai            Modified Get,Post  and Added Put method
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
    public class CustomerFeedbackSurveyDetailController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly SecurityTokenBL securityTokenBL = new SecurityTokenBL();
        private SecurityTokenDTO securityTokenDTO = null;
        private ExecutionContext executionContext;

        /// <summary>
        /// Get the JSON Object FeedbackSurveyDetails List
        /// </summary>        
        /// <returns>HttpMessage</returns>
        [HttpGet]
        [Route("api/Customer/FeedbackSurvey/FeedbackSurveyDetails")]
        [Authorize]
        public async Task<HttpResponseMessage> Get(string isActive = null)
        {
            try
            {
                log.LogMethodEntry(isActive);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                List<KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>> customerFeedbackSurveyDetailsSearchParams = new List<KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>>();
                customerFeedbackSurveyDetailsSearchParams.Add(new KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>(CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        customerFeedbackSurveyDetailsSearchParams.Add(new KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>(CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.IS_ACTIVE, isActive));
                    }
                }
                ICustomerFeedbackSurveyDetailsUseCases customerFeedbackSurveyDetailsUseCases = CustomerUseCaseFactory.GetCustomerFeedbackSurveyDetails(executionContext);
                List<CustomerFeedbackSurveyDetailsDTO> customerFeedbackSurveyDetailsDTOList = await customerFeedbackSurveyDetailsUseCases.GetCustomerFeedbackSurveyDetails(customerFeedbackSurveyDetailsSearchParams);
                log.LogMethodExit(customerFeedbackSurveyDetailsDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = customerFeedbackSurveyDetailsDTOList });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }
        /// <summary>
        /// Post the JSON Object FeedbackSurveyDetails List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/Customer/FeedbackSurvey/FeedbackSurveyDetails")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody] List<CustomerFeedbackSurveyDetailsDTO> customerFeedbackSurveyDetailsDTOList)
        {
            try
            {
                log.LogMethodEntry(customerFeedbackSurveyDetailsDTOList);
                securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (customerFeedbackSurveyDetailsDTOList == null)
                {
                    log.LogMethodExit(customerFeedbackSurveyDetailsDTOList);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                ICustomerFeedbackSurveyDetailsUseCases customerFeedbackSurveyDetailsUseCases = CustomerUseCaseFactory.GetCustomerFeedbackSurveyDetails(executionContext);
                await customerFeedbackSurveyDetailsUseCases.SaveCustomerFeedbackSurveyDetails(customerFeedbackSurveyDetailsDTOList);
                log.LogMethodExit(customerFeedbackSurveyDetailsDTOList);
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
        /// Post the CustomerFeedBackSurveyData collection
        /// <param name="customerFeedbackSurveyDetailsDTOList">CustomerFeedbackSurveyDetailsDTOList</param>
        [HttpPut]
        [Route("api/Customer/FeedbackSurvey/FeedbackSurveyDetails")]
        [Authorize]
        public async Task<HttpResponseMessage> Put([FromBody]List<CustomerFeedbackSurveyDetailsDTO> customerFeedbackSurveyDetailsDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(customerFeedbackSurveyDetailsDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (customerFeedbackSurveyDetailsDTOList == null || customerFeedbackSurveyDetailsDTOList.Any(a => a.CustFbSurveyDetailId < 0))
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                ICustomerFeedbackSurveyDetailsUseCases customerFeedbackSurveyDetailsUseCases = CustomerUseCaseFactory.GetCustomerFeedbackSurveyDetails(executionContext);
                await customerFeedbackSurveyDetailsUseCases.SaveCustomerFeedbackSurveyDetails(customerFeedbackSurveyDetailsDTOList);
                log.LogMethodExit(customerFeedbackSurveyDetailsDTOList);
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
