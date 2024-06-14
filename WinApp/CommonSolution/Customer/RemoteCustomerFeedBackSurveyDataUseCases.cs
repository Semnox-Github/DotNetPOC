/********************************************************************************************
 * Project Name - CustomerFeedBackSurveyData
 * Description  - RemoteCustomerFeedBackSurveyDataUseCases class
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
    class RemoteCustomerFeedBackSurveyDataUseCases: RemoteUseCases, ICustomerFeedBackSurveyDataUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string CustomerFeedBackSurveyData_URL = "api/Customer/FeedbackSurvey/FeedbackSurveyData";
        public RemoteCustomerFeedBackSurveyDataUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }
        public async Task<List<CustomerFeedbackSurveyDataDTO>> GetCustomerFeedbackSurveyDatas(List<KeyValuePair<CustomerFeedbackSurveyDataDTO.SearchByCustomerFeedbackSurveyDataParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
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
                List<CustomerFeedbackSurveyDataDTO> result = await Get<List<CustomerFeedbackSurveyDataDTO>>(CustomerFeedBackSurveyData_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<CustomerFeedbackSurveyDataDTO.SearchByCustomerFeedbackSurveyDataParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<CustomerFeedbackSurveyDataDTO.SearchByCustomerFeedbackSurveyDataParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case CustomerFeedbackSurveyDataDTO.SearchByCustomerFeedbackSurveyDataParameters.CUST_FB_RESPONSE_DATE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("CustFbResponseDate".ToString(), searchParameter.Value));
                        }
                        break;
                    case CustomerFeedbackSurveyDataDTO.SearchByCustomerFeedbackSurveyDataParameters.CUST_FB_RESPONSE_TEXT:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("CustFbResponseText".ToString(), searchParameter.Value));
                        }
                        break;
                    case CustomerFeedbackSurveyDataDTO.SearchByCustomerFeedbackSurveyDataParameters.CUST_FB_RESPONSE_VALUE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("CustFbResposnseValueId".ToString(), searchParameter.Value));
                        }
                        break;
                    case CustomerFeedbackSurveyDataDTO.SearchByCustomerFeedbackSurveyDataParameters.CUST_FB_SURVEY_DATASET_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("CustFbSurveyDatasetId".ToString(), searchParameter.Value));
                        }
                        break;
                    case CustomerFeedbackSurveyDataDTO.SearchByCustomerFeedbackSurveyDataParameters.CUST_FB_SURVEY_DATASET_ID_LIST:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("CustFbSurveyDatasetIdList".ToString(), searchParameter.Value));
                        }
                        break;
                    case CustomerFeedbackSurveyDataDTO.SearchByCustomerFeedbackSurveyDataParameters.CUST_FB_SURVEY_DATA_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("CustFbSurveyDataId".ToString(), searchParameter.Value));
                        }
                        break;
                    case CustomerFeedbackSurveyDataDTO.SearchByCustomerFeedbackSurveyDataParameters.CUST_FB_SURVEY_DETAIL_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("CustFbSurveyDetailId".ToString(), searchParameter.Value));
                        }
                        break;
                    case CustomerFeedbackSurveyDataDTO.SearchByCustomerFeedbackSurveyDataParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("IsActive".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }
        public async Task<string> SaveCustomerFeedbackSurveyDatas(List<CustomerFeedbackSurveyDataDTO> customerFeedbackSurveyDataDTOList)
        {
            log.LogMethodEntry(customerFeedbackSurveyDataDTOList);
            try
            {
                string responseString = await Post<string>(CustomerFeedBackSurveyData_URL, customerFeedbackSurveyDataDTOList);
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
