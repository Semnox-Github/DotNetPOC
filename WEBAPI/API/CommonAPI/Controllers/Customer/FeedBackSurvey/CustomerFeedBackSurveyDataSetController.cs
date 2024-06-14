/********************************************************************************************
 * Project Name - Customer                                                                          
 * Description  - Controller of the CustomerFeedbackSurveyDataSet class.
 *
 **************
 **Version Log
  *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.150.1      17-Feb-2023   Abhishek                Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Linq;
using System.Threading.Tasks;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer;

namespace Semnox.CommonAPI.Customer.FeedbackSurvey
{
    public class CustomerFeedBackSurveyDataSetController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Get the JSON Object FeedbackSurveyDataSet List
        /// </summary>        
        /// <returns>HttpMessage</returns>
        [HttpGet]
        [Route("api/Customer/FeedbackSurvey/FeedbackSurveyDataSet")]
        [Authorize]
        public async Task<HttpResponseMessage> Get(int custFbSurveyDataSetId = -1)
        {
            try
            {
                log.LogMethodEntry(custFbSurveyDataSetId);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                List<KeyValuePair<CustomerFeedbackSurveyDataSetDTO.SearchByCustomerFeedbackSurveyDataSetParameters, string>> searchByCustomerFeedBackSurveyParameters = new List<KeyValuePair<CustomerFeedbackSurveyDataSetDTO.SearchByCustomerFeedbackSurveyDataSetParameters, string>>();
                searchByCustomerFeedBackSurveyParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDataSetDTO.SearchByCustomerFeedbackSurveyDataSetParameters, string>(CustomerFeedbackSurveyDataSetDTO.SearchByCustomerFeedbackSurveyDataSetParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (custFbSurveyDataSetId > -1)
                {
                    searchByCustomerFeedBackSurveyParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDataSetDTO.SearchByCustomerFeedbackSurveyDataSetParameters, string>(CustomerFeedbackSurveyDataSetDTO.SearchByCustomerFeedbackSurveyDataSetParameters.CUST_FB_SURVEY_DATA_SET_ID, custFbSurveyDataSetId.ToString()));
                }
                ICustomerFeedBackSurveyDataSetUseCases customerFeedBackSurveyDataSetUseCases = CustomerUseCaseFactory.GetCustomerFeedbackSurveyDataSets(executionContext);
                List<CustomerFeedbackSurveyDataSetDTO> customerFeedbackSurveyDataSetDTOList = await customerFeedBackSurveyDataSetUseCases.GetCustomerFeedbackSurveyDataSets(searchByCustomerFeedBackSurveyParameters);
                log.LogMethodExit(customerFeedbackSurveyDataSetDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = customerFeedbackSurveyDataSetDTOList });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }

        /// <summary>
        /// Post the JSON Object of FeedbackSurveyDataSet List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/Customer/FeedbackSurvey/FeedbackSurveyDataSet")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody] List<CustomerFeedbackSurveyDataSetDTO> customerFeedbackSurveyDataSetDTOList)
        {
            try
            {
                log.LogMethodEntry(customerFeedbackSurveyDataSetDTOList);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);

                if (customerFeedbackSurveyDataSetDTOList == null)
                {
                    log.LogMethodExit(customerFeedbackSurveyDataSetDTOList);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                ICustomerFeedBackSurveyDataSetUseCases customerFeedBackSurveyDataSetUseCases = CustomerUseCaseFactory.GetCustomerFeedbackSurveyDataSets(executionContext);
                List<CustomerFeedbackSurveyDataSetDTO> response = await customerFeedBackSurveyDataSetUseCases.SaveCustomerFeedbackSurveyDataSets(customerFeedbackSurveyDataSetDTOList);
                log.LogMethodExit(response);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = response });
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
