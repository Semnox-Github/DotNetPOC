﻿/********************************************************************************************
 * Project Name - CustomerFeedBackSurveyData
 * Description  - RemoteCustomerFeedBackSurveyDataSetUseCases class
 *
 **************
 ** Version Log
  **************
  * Version      Date              Modified By         Remarks
 *********************************************************************************************
 *2.150.1      17-Feb-2023         Abhishek            Created 
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
    /// <summary>
    /// RemoteTransactionUseCases
    /// </summary>
    public class RemoteCustomerFeedBackSurveyDataSetUseCases : RemoteUseCases, ICustomerFeedBackSurveyDataSetUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string CUSTOMER_FEEDBACK_SURVEY_DATASET_URL = "api/Customer/FeedbackSurvey/FeedbackSurveyDataSet";

        /// <summary>
        /// RemoteCustomerFeedBackSurveyDataSetUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        public RemoteCustomerFeedBackSurveyDataSetUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        /// <summary>
        /// GetCustomerFeedbackSurveyDataSets
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns>CustomerFeedbackSurveyDataSetDTO List</returns>
        public async Task<List<CustomerFeedbackSurveyDataSetDTO>> GetCustomerFeedbackSurveyDataSets(List<KeyValuePair<CustomerFeedbackSurveyDataSetDTO.SearchByCustomerFeedbackSurveyDataSetParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();

            if (searchParameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(searchParameters));
            }
            try
            {
                List<CustomerFeedbackSurveyDataSetDTO> result = await Get<List<CustomerFeedbackSurveyDataSetDTO>>(CUSTOMER_FEEDBACK_SURVEY_DATASET_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }


        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<CustomerFeedbackSurveyDataSetDTO.SearchByCustomerFeedbackSurveyDataSetParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<CustomerFeedbackSurveyDataSetDTO.SearchByCustomerFeedbackSurveyDataSetParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {
                    case CustomerFeedbackSurveyDataSetDTO.SearchByCustomerFeedbackSurveyDataSetParameters.CUST_FB_SURVEY_DATA_SET_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("custFbSurveyDataSetId".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }

        /// <summary>
        /// SaveCustomerFeedbackSurveyDataSets
        /// </summary>
        /// <returns></returns>
        public async Task<List<CustomerFeedbackSurveyDataSetDTO>> SaveCustomerFeedbackSurveyDataSets(List<CustomerFeedbackSurveyDataSetDTO> customerFeedbackSurveyDataSetDTOList)
        {
            log.LogMethodEntry(customerFeedbackSurveyDataSetDTOList);
            try
            {
                List<CustomerFeedbackSurveyDataSetDTO> responseData = await Post<List<CustomerFeedbackSurveyDataSetDTO>>(CUSTOMER_FEEDBACK_SURVEY_DATASET_URL, customerFeedbackSurveyDataSetDTOList);
                log.LogMethodExit(responseData);
                return responseData;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
    }
}