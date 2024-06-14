/********************************************************************************************
 * Project Name -Customer
 * Description  -RemoteCustomerFeedbackResponseUseCases class 
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
    class RemoteCustomerFeedbackResponseUseCases:RemoteUseCases,ICustomerFeedbackResponseUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string CUSTOMER_FEEDBACK_RESPONSE_URL = "api/Customer/FeedbackSurvey/FeedbackResponses";
        public RemoteCustomerFeedbackResponseUseCases(ExecutionContext executionContext)
           : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }
        public async Task<List<CustomerFeedbackResponseDTO>> GetCustomerFeedbackResponses(List<KeyValuePair<CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters, string>> searchParameters,
                                          bool loadChildRecords = false, bool activeChildRecords = true,
                                          SqlTransaction sqlTransaction = null)

        {
            log.LogMethodEntry(searchParameters, loadChildRecords, activeChildRecords, sqlTransaction);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("loadChildRecords".ToString(), loadChildRecords.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("activeChildRecords".ToString(), activeChildRecords.ToString()));
            if (searchParameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(searchParameters));
            }
            try
            {
                List<CustomerFeedbackResponseDTO> result = await Get<List<CustomerFeedbackResponseDTO>>(CUSTOMER_FEEDBACK_RESPONSE_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters.CUST_FB_RESPONSE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("custFbResponseId".ToString(), searchParameter.Value));
                        }
                        break;
                    case CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters.CUST_FB_RESPONSE_ID_LIST:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("custFbResponseIdList".ToString(), searchParameter.Value));
                        }
                        break;
                    case CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters.RESPONSE_NAME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("responseName".ToString(), searchParameter.Value));
                        }
                        break;
                    case CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters.RESPONSE_TYPE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("responseTypeId".ToString(), searchParameter.Value));
                        }
                        break;
                    case CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }
        public async Task<string> SaveCustomerFeedbackResponses(List<CustomerFeedbackResponseDTO> customerFeedbackResponseDTOList)
        {
            log.LogMethodEntry(customerFeedbackResponseDTOList);
            try
            {
                string responseString = await Post<string>(CUSTOMER_FEEDBACK_RESPONSE_URL,customerFeedbackResponseDTOList);
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
