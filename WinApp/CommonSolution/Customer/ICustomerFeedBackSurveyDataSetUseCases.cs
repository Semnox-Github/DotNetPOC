/********************************************************************************************
 * Project Name - CustomerFeedBackSurveyData
 * Description  - ICustomerFeedBackSurveyDataSetUseCases class
 *
 **************
 ** Version Log
  **************
  * Version      Date              Modified By         Remarks
 *********************************************************************************************
 *2.150.1      17-Feb-2023         Abhishek            Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Semnox.Parafait.Customer
{
    public interface ICustomerFeedBackSurveyDataSetUseCases
    {
        /// <summary>
        /// GetCustomerFeedbackSurveyDataSets
        /// </summary>    
        /// <param name="searchParameters"></param>
        /// <returns>CustomerFeedbackSurveyDataSetDTOList</returns>
        Task<List<CustomerFeedbackSurveyDataSetDTO>> GetCustomerFeedbackSurveyDataSets(List<KeyValuePair<CustomerFeedbackSurveyDataSetDTO.SearchByCustomerFeedbackSurveyDataSetParameters, string>> searchParameters, SqlTransaction sqlTransaction = null);
        /// <summary>
        /// SaveCustomerFeedbackSurveyDataSets
        /// </summary>    
        /// <returns>CustomerFeedbackSurveyDataSetDTOList</returns>
        Task<List<CustomerFeedbackSurveyDataSetDTO>> SaveCustomerFeedbackSurveyDataSets(List<CustomerFeedbackSurveyDataSetDTO> customerFeedbackSurveyDataSetDTOList);
    }
}
