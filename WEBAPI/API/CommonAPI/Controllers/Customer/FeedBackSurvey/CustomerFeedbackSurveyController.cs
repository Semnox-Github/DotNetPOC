/********************************************************************************************
 * Project Name - Customer                                                                          
 * Description  - Controller of the customerFeedbackSurvey class.
 *
 **************
 **Version Log
  *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.80        24-Feb-2020   Mushahid Faizan          Created 
 *2.120.0     05-May-2021   B Mahesh Pai            Modified Get,Post and Added Put method
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
using Semnox.CommonAPI.Helpers;
using System.Threading.Tasks;

namespace Semnox.CommonAPI.Customer.FeedbackSurvey
{
    public class CustomerFeedbackSurveyController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly SecurityTokenBL securityTokenBL = new SecurityTokenBL();
        private SecurityTokenDTO securityTokenDTO = null;
        private ExecutionContext executionContext;
        /// <summary>
        /// Get the JSON Object customerFeedbackSurvey List
        /// </summary>        
        /// <returns>HttpMessage</returns>
        [HttpGet]
        [Route("api/Customer/FeedbackSurvey/FeedbackSurveys")]
        [Authorize]
        public async Task<HttpResponseMessage> Get(string isActive = null, bool loadActiveChild = false, bool buildChildRecords = false, string surveyName = null, string posmachine = null)
        {
            try
            {
                log.LogMethodEntry(isActive, loadActiveChild, buildChildRecords, surveyName, posmachine);
                securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                List<KeyValuePair<CustomerFeedbackSurveyDTO.SearchByCustomerFeedBackSurveyParameters, string>> searchByCustomerFeedBackSurveyParameters = new List<KeyValuePair<CustomerFeedbackSurveyDTO.SearchByCustomerFeedBackSurveyParameters, string>>();
                searchByCustomerFeedBackSurveyParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDTO.SearchByCustomerFeedBackSurveyParameters, string>(CustomerFeedbackSurveyDTO.SearchByCustomerFeedBackSurveyParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        loadActiveChild = true;
                        searchByCustomerFeedBackSurveyParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDTO.SearchByCustomerFeedBackSurveyParameters, string>(CustomerFeedbackSurveyDTO.SearchByCustomerFeedBackSurveyParameters.IS_ACTIVE, isActive));
                    }
                }
                if (!string.IsNullOrEmpty(surveyName))
                {
                    searchByCustomerFeedBackSurveyParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDTO.SearchByCustomerFeedBackSurveyParameters, string>(CustomerFeedbackSurveyDTO.SearchByCustomerFeedBackSurveyParameters.SURVEY_NAME, surveyName));
                }
                ICustomerFeedbackSurveyUseCases customerFeedbackSurveyUseCases = CustomerUseCaseFactory.GetCustomerFeedbackSurveys(executionContext);
                List<CustomerFeedbackSurveyDTO> customerFeedbackSurveyDTOList = await customerFeedbackSurveyUseCases.GetCustomerFeedbackSurveys(searchByCustomerFeedBackSurveyParameters, posmachine, buildChildRecords, loadActiveChild, null);
                log.LogMethodExit(customerFeedbackSurveyDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = customerFeedbackSurveyDTOList });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }

        }
        /// <summary>
        /// Post the JSON Object customerFeedbackSurvey List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/Customer/FeedbackSurvey/FeedbackSurveys")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody] List<CustomerFeedbackSurveyDTO> customerFeedbackSurveyDTOList)
        {
            try
            {
                log.LogMethodEntry(customerFeedbackSurveyDTOList);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (customerFeedbackSurveyDTOList ==null )
                {
                    log.LogMethodExit(customerFeedbackSurveyDTOList);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                ICustomerFeedbackSurveyUseCases customerFeedbackSurveyUseCases = CustomerUseCaseFactory.GetCustomerFeedbackSurveys(executionContext);
                await customerFeedbackSurveyUseCases.SaveCustomerFeedbackSurveys(customerFeedbackSurveyDTOList);
                log.LogMethodExit(customerFeedbackSurveyDTOList);
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
        /// <param name="customerFeedbackSurveyDTOList">CustomerFeedbackSurveyDTO</param>
        [HttpPut]
        [Route("api/Customer/FeedbackSurvey/FeedbackSurveys")]
        [Authorize]
        public async Task<HttpResponseMessage> Put([FromBody]List<CustomerFeedbackSurveyDTO> customerFeedbackSurveyDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(customerFeedbackSurveyDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (customerFeedbackSurveyDTOList == null || customerFeedbackSurveyDTOList.Any(a => a.CustFbSurveyId < 0))
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                ICustomerFeedbackSurveyUseCases customerFeedbackSurveyUseCases = CustomerUseCaseFactory.GetCustomerFeedbackSurveys(executionContext);
                await customerFeedbackSurveyUseCases.SaveCustomerFeedbackSurveys(customerFeedbackSurveyDTOList);
                log.LogMethodExit(customerFeedbackSurveyDTOList);
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
