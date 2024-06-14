/********************************************************************************************
 * Project Name - CustomerFeedbackSurveyDetail
 * Description  - RemoteCustomerFeedbackSurveyDetailsUseCases class
 *
 **************
 ** Version Log
  **************
  * Version      Date              Modified By         Remarks
 *********************************************************************************************
 2.120.0         04-May-2021       B Mahesh Pai       Created
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Customer
{
    class RemoteCustomerFeedbackSurveyDetailsUseCases : RemoteUseCases, ICustomerFeedbackSurveyDetailsUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string CustomerFeedbackSurveyDetails_URL = "api/Customer/FeedbackSurvey/FeedbackSurveyData";
        public RemoteCustomerFeedbackSurveyDetailsUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }
        public async Task<List<CustomerFeedbackSurveyDetailsDTO>> GetCustomerFeedbackSurveyDetails(List<KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);

            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();

            if (searchParameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(searchParameters));
            }
            try
            {
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                List<CustomerFeedbackSurveyDetailsDTO> result = await Get<List<CustomerFeedbackSurveyDetailsDTO>>(CustomerFeedbackSurveyDetails_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.CRITERIA_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("CriteriaId".ToString(), searchParameter.Value));
                        }
                        break;
                    case CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.CRITERIA_VALUE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("CriteriaValue".ToString(), searchParameter.Value));
                        }
                        break;
                    case CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.CUST_FB_QUESTION_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("CustFbQuestionId".ToString(), searchParameter.Value));
                        }
                        break;
                    case CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.CUST_FB_RESPONSE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("CustFbResponseId".ToString(), searchParameter.Value));
                        }
                        break;
                    case CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.CUST_FB_SURVEY_DETAIL_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("CustFbSurveyDetailId".ToString(), searchParameter.Value));
                        }
                        break;
                    case CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.CUST_FB_SURVEY_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("custFbSurveyId".ToString(), searchParameter.Value));
                        }
                        break;
                    case CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.EXPECTED_CUST_FB_RESPONSE_VALUE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("ExpectedCustFbResponseValueId".ToString(), searchParameter.Value));
                        }
                        break;
                    case CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("IsActive".ToString(), searchParameter.Value));
                        }
                        break;
                    case CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.IS_RESPONSE_MANDATORY:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("IsResponseMandatory".ToString(), searchParameter.Value));
                        }
                        break;
                    case CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters.NEXT_QUESTION_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("NextQuestionId".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }
        public async Task<string> SaveCustomerFeedbackSurveyDetails(List<CustomerFeedbackSurveyDetailsDTO> customerFeedbackSurveyDetailsDTOList)
        {
            log.LogMethodEntry(customerFeedbackSurveyDetailsDTOList);
            try
            {
                string responseString = await Post<string>(CustomerFeedbackSurveyDetails_URL, customerFeedbackSurveyDetailsDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
    }
}
