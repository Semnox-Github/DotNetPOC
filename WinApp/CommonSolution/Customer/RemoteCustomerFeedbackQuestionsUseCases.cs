/********************************************************************************************
 * Project Name -Customer
 * Description  -RemoteCustomerFeedbackQuestionsUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.00    04-May-2021       Roshan Devadiga        Created : POS UI Redesign with REST API
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
    class RemoteCustomerFeedbackQuestionsUseCases:RemoteUseCases,ICustomerFeedbackQuestionsUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string CUSTOMER_FEEDBACK_QUESTIONS_URL = "api/Customer/FeedbackSurvey/FeedbackQuestions";
        public RemoteCustomerFeedbackQuestionsUseCases(ExecutionContext executionContext)
           : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

         public async Task<List<CustomerFeedbackQuestionsDTO>> GetCustomerFeedbackQuestions(List<KeyValuePair<CustomerFeedbackQuestionsDTO.SearchByCustomerFeedbackQuestionsParameters, string>> searchParameters, int LanguageId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters,LanguageId,sqlTransaction);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();

            if (searchParameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(searchParameters));
            }
            try
            {
                List<CustomerFeedbackQuestionsDTO> result = await Get<List<CustomerFeedbackQuestionsDTO>>(CUSTOMER_FEEDBACK_QUESTIONS_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<CustomerFeedbackQuestionsDTO.SearchByCustomerFeedbackQuestionsParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<CustomerFeedbackQuestionsDTO.SearchByCustomerFeedbackQuestionsParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case CustomerFeedbackQuestionsDTO.SearchByCustomerFeedbackQuestionsParameters.CUST_FB_QUESTION_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("custFbQuestionId".ToString(), searchParameter.Value));
                        }
                        break;
                    case CustomerFeedbackQuestionsDTO.SearchByCustomerFeedbackQuestionsParameters.CUST_FB_QUESTION_ID_LIST:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("custFbQuestionIdList".ToString(), searchParameter.Value));
                        }
                        break;
                    case CustomerFeedbackQuestionsDTO.SearchByCustomerFeedbackQuestionsParameters.QUESTION_NO:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("questionNo".ToString(), searchParameter.Value));
                        }
                        break;
                    case CustomerFeedbackQuestionsDTO.SearchByCustomerFeedbackQuestionsParameters.QUESTION:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("question".ToString(), searchParameter.Value));
                        }
                        break;
                    case CustomerFeedbackQuestionsDTO.SearchByCustomerFeedbackQuestionsParameters.CUST_FB_RESPONSE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("custFbResponseId".ToString(), searchParameter.Value));
                        }
                        break;
                    case CustomerFeedbackQuestionsDTO.SearchByCustomerFeedbackQuestionsParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }
        public async Task<string> SaveCustomerFeedbackQuestions(List<CustomerFeedbackQuestionsDTO> customerFeedbackQuestionsDTOList)
        {
            log.LogMethodEntry(customerFeedbackQuestionsDTOList);
            try
            {
                string responseString = await Post<string>(CUSTOMER_FEEDBACK_QUESTIONS_URL,customerFeedbackQuestionsDTOList);
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
