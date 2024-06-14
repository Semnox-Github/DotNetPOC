/********************************************************************************************
 * Project Name - CustomerFeedbackSurvey
 * Description  - RemoteCustomerFeedbackSurveyUseCases class
 *
 **************
 ** Version Log
  **************
  * Version      Date              Modified By         Remarks
 *********************************************************************************************
 2.120.0         05-May-2021       B Mahesh Pai       Created
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
    class RemoteCustomerFeedbackSurveyUseCases: RemoteUseCases, ICustomerFeedbackSurveyUseCases
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string CUSTOMERFEEDBACKSURVEY_URL = "api/Customer/FeedbackSurvey/FeedbackSurveys";
        public RemoteCustomerFeedbackSurveyUseCases(ExecutionContext executionContext)
           : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }
        public async Task<List<CustomerFeedbackSurveyDTO>> GetCustomerFeedbackSurveys(List<KeyValuePair<CustomerFeedbackSurveyDTO.SearchByCustomerFeedBackSurveyParameters, string>> searchParameters,
                                              string posMachine = null, bool loadChildRecords = false, bool activeChildRecords = true,
                                              SqlTransaction sqlTransaction = null)

        {
            log.LogMethodEntry(searchParameters, loadChildRecords, activeChildRecords, sqlTransaction);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("loadChildRecords".ToString(), loadChildRecords.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("posMachine".ToString(), posMachine.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("activeChildRecords".ToString(), activeChildRecords.ToString()));
            if (searchParameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(searchParameters));
            }
            try
            {
                List<CustomerFeedbackSurveyDTO> result = await Get<List<CustomerFeedbackSurveyDTO>>(CUSTOMERFEEDBACKSURVEY_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<CustomerFeedbackSurveyDTO.SearchByCustomerFeedBackSurveyParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<CustomerFeedbackSurveyDTO.SearchByCustomerFeedBackSurveyParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case CustomerFeedbackSurveyDTO.SearchByCustomerFeedBackSurveyParameters.CUST_FB_SURVEY_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("CustFbSurveryId".ToString(), searchParameter.Value));
                        }
                        break;
                    case CustomerFeedbackSurveyDTO.SearchByCustomerFeedBackSurveyParameters.FROM_DATE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("FromDate".ToString(), searchParameter.Value));
                        }
                        break;
                    case CustomerFeedbackSurveyDTO.SearchByCustomerFeedBackSurveyParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("IsActive".ToString(), searchParameter.Value));
                        }
                        break;
                    case CustomerFeedbackSurveyDTO.SearchByCustomerFeedBackSurveyParameters.SURVEY_NAME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("SurveyName".ToString(), searchParameter.Value));
                        }
                        break;
                    case CustomerFeedbackSurveyDTO.SearchByCustomerFeedBackSurveyParameters.TO_DATE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("ToDate".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }
        public async Task<string> SaveCustomerFeedbackSurveys(List<CustomerFeedbackSurveyDTO> customerFeedbackSurveyDTOList)
        {
            log.LogMethodEntry(customerFeedbackSurveyDTOList);
            try
            {
                string responseString = await Post<string>(CUSTOMERFEEDBACKSURVEY_URL, customerFeedbackSurveyDTOList);
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
