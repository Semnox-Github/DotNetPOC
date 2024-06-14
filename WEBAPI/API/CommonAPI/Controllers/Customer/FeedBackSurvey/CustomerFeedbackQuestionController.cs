/********************************************************************************************
 * Project Name - Customer                                                                      
 * Description  - Controller of the CustomerFeedbackQuestions class
 *
 **************
 **Version Log
  *Version     Date          Modified By            Remarks          
 *********************************************************************************************
 *2.80        24-Feb-2020   Mushahid Faizan          Created 
 *2.120.00    04-May-2021   Roshan Devadiga          Modified Get,Post  and Added Put method
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
    public class CustomerFeedbackQuestionController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly SecurityTokenBL securityTokenBL = new SecurityTokenBL();
        private SecurityTokenDTO securityTokenDTO = null;
        private ExecutionContext executionContext;
        /// <summary>
        /// Get the JSON Object Questions List
        /// </summary>        
        /// <returns>HttpMessage</returns>
        [HttpGet]
        [Route("api/Customer/FeedbackSurvey/FeedbackQuestions")]
        [Authorize]
        public async Task<HttpResponseMessage> Get(string isActive = null, string questionNumber = null, int languageId = -1)
        {
            try
            {
                log.LogMethodEntry(isActive, questionNumber, languageId);
                securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                List<KeyValuePair<CustomerFeedbackQuestionsDTO.SearchByCustomerFeedbackQuestionsParameters, string>> searchByCustomerFeedbackQuestionsParameters = new List<KeyValuePair<CustomerFeedbackQuestionsDTO.SearchByCustomerFeedbackQuestionsParameters, string>>();
                searchByCustomerFeedbackQuestionsParameters.Add(new KeyValuePair<CustomerFeedbackQuestionsDTO.SearchByCustomerFeedbackQuestionsParameters, string>(CustomerFeedbackQuestionsDTO.SearchByCustomerFeedbackQuestionsParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchByCustomerFeedbackQuestionsParameters.Add(new KeyValuePair<CustomerFeedbackQuestionsDTO.SearchByCustomerFeedbackQuestionsParameters, string>(CustomerFeedbackQuestionsDTO.SearchByCustomerFeedbackQuestionsParameters.IS_ACTIVE, isActive));
                    }
                }
                if (!string.IsNullOrEmpty(questionNumber))
                {
                    searchByCustomerFeedbackQuestionsParameters.Add(new KeyValuePair<CustomerFeedbackQuestionsDTO.SearchByCustomerFeedbackQuestionsParameters, string>(CustomerFeedbackQuestionsDTO.SearchByCustomerFeedbackQuestionsParameters.QUESTION_NO, questionNumber));
                }
                ICustomerFeedbackQuestionsUseCases customerFeedbackQuestionsUseCases = CustomerUseCaseFactory.GetCustomerFeedbackQuestionsUseCases(executionContext);
                List<CustomerFeedbackQuestionsDTO> customerFeedbackQuestionsList = await customerFeedbackQuestionsUseCases.GetCustomerFeedbackQuestions(searchByCustomerFeedbackQuestionsParameters, languageId);
                log.LogMethodExit(customerFeedbackQuestionsList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = customerFeedbackQuestionsList });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }
        /// <summary>
        /// Post the JSON Object Questions List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/Customer/FeedbackSurvey/FeedbackQuestions")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody] List<CustomerFeedbackQuestionsDTO> customerFeedbackQuestionsDTOList)
        {
            try
            {
                log.LogMethodEntry(customerFeedbackQuestionsDTOList);
                securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (customerFeedbackQuestionsDTOList == null)
                {
                    log.LogMethodExit(customerFeedbackQuestionsDTOList);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                ICustomerFeedbackQuestionsUseCases customerFeedbackQuestionsUseCases = CustomerUseCaseFactory.GetCustomerFeedbackQuestionsUseCases(executionContext);
                await customerFeedbackQuestionsUseCases.SaveCustomerFeedbackQuestions(customerFeedbackQuestionsDTOList);
                log.LogMethodExit(customerFeedbackQuestionsDTOList);
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
        /// <param name="customerFeedbackQuestionsDTOList"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/Customer/FeedbackSurvey/FeedbackQuestions")]
        [Authorize]
        public async Task<HttpResponseMessage> Put([FromBody]List<CustomerFeedbackQuestionsDTO> customerFeedbackQuestionsDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(customerFeedbackQuestionsDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (customerFeedbackQuestionsDTOList == null || customerFeedbackQuestionsDTOList.Any(a => a.CustFbQuestionId < 0))
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                ICustomerFeedbackQuestionsUseCases customerFeedbackQuestionsUseCases = CustomerUseCaseFactory.GetCustomerFeedbackQuestionsUseCases(executionContext);
                await customerFeedbackQuestionsUseCases.SaveCustomerFeedbackQuestions(customerFeedbackQuestionsDTOList);
                log.LogMethodExit(customerFeedbackQuestionsDTOList);
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
