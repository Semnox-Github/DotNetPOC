/********************************************************************************************
 * Project Name - Customer                                                                          
 * Description  - Controller of the CustomerFeedbackSurveyData class.
 *
 **************
 **Version Log
  *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.80        24-Feb-2020   Mushahid Faizan          Created 
 *2.120.0     04-May-2021   B Mahesh Pai            Modified Get,Post  and Added Put method
 ********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Linq;
using System.Threading.Tasks;
using Semnox.CommonAPI.Helpers;

namespace Semnox.CommonAPI.Customer.FeedbackSurvey
{
    public class CustomerFeedBackSurveyDataController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly SecurityTokenBL securityTokenBL = new SecurityTokenBL();
        private SecurityTokenDTO securityTokenDTO = null;
        private ExecutionContext executionContext;

        /// <summary>
        /// Get the JSON Object FeedbackSurveyData List
        /// </summary>        
        /// <returns>HttpMessage</returns>
        [HttpGet]
        [Route("api/Customer/FeedbackSurvey/FeedbackSurveyData")]
        [Authorize]
        public async Task<HttpResponseMessage> Get(string isActive = null)
        {
            try
            {
                log.LogMethodEntry(isActive);
                securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                List<KeyValuePair<CustomerFeedbackSurveyDataDTO.SearchByCustomerFeedbackSurveyDataParameters, string>> searchByCustomerFeedBackSurveyParameters = new List<KeyValuePair<CustomerFeedbackSurveyDataDTO.SearchByCustomerFeedbackSurveyDataParameters, string>>();
                searchByCustomerFeedBackSurveyParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDataDTO.SearchByCustomerFeedbackSurveyDataParameters, string>(CustomerFeedbackSurveyDataDTO.SearchByCustomerFeedbackSurveyDataParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchByCustomerFeedBackSurveyParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDataDTO.SearchByCustomerFeedbackSurveyDataParameters, string>(CustomerFeedbackSurveyDataDTO.SearchByCustomerFeedbackSurveyDataParameters.IS_ACTIVE, isActive));
                    }
                }
                ICustomerFeedBackSurveyDataUseCases customerFeedBackSurveyDataUseCases = CustomerUseCaseFactory.GetCustomerFeedbackSurveyDatas(executionContext);
                List<CustomerFeedbackSurveyDataDTO> customerFeedbackSurveyDataDTOList = await customerFeedBackSurveyDataUseCases.GetCustomerFeedbackSurveyDatas(searchByCustomerFeedBackSurveyParameters);
                log.LogMethodExit(customerFeedbackSurveyDataDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = customerFeedbackSurveyDataDTOList });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }

        /// <summary>
        /// Post the JSON Object of FeedbackSurveyData List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/Customer/FeedbackSurvey/FeedbackSurveyData")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody] List<CustomerFeedbackSurveyDataDTO> customerFeedbackSurveyDataDTOList)
        {
            try
            {
                log.LogMethodEntry(customerFeedbackSurveyDataDTOList);
                securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (customerFeedbackSurveyDataDTOList == null)
                {
                    log.LogMethodExit(customerFeedbackSurveyDataDTOList);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                ICustomerFeedBackSurveyDataUseCases customerFeedBackSurveyDataUseCases = CustomerUseCaseFactory.GetCustomerFeedbackSurveyDatas(executionContext);
                await customerFeedBackSurveyDataUseCases.SaveCustomerFeedbackSurveyDatas(customerFeedbackSurveyDataDTOList);
                log.LogMethodExit(customerFeedbackSurveyDataDTOList);
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
        /// <param name="customerFeedbackSurveyDataDTOList">CustomerFeedbackSurveyDataDTOList</param>
        [HttpPut]
        [Route("api/Customer/FeedbackSurvey/FeedbackSurveyData")]
        [Authorize]
        public async Task<HttpResponseMessage> Put([FromBody]List<CustomerFeedbackSurveyDataDTO> customerFeedbackSurveyDataDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(customerFeedbackSurveyDataDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (customerFeedbackSurveyDataDTOList == null || customerFeedbackSurveyDataDTOList.Any(a => a.CustFbSurveyDataId < 0))
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                ICustomerFeedBackSurveyDataUseCases customerFeedBackSurveyDataUseCases = CustomerUseCaseFactory.GetCustomerFeedbackSurveyDatas(executionContext);
                await customerFeedBackSurveyDataUseCases.SaveCustomerFeedbackSurveyDatas(customerFeedbackSurveyDataDTOList);
                log.LogMethodExit(customerFeedbackSurveyDataDTOList);
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
