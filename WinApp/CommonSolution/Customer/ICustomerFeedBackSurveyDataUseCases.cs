/********************************************************************************************
 * Project Name - CustomerFeedBackSurveyData
 * Description  - ICustomerFeedBackSurveyDataUseCases class
 *
 **************
 ** Version Log
  **************
  * Version      Date              Modified By         Remarks
 *********************************************************************************************
 2.120.0         04-May-2021       B Mahesh Pai       Created
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Customer
{
    public interface ICustomerFeedBackSurveyDataUseCases
    {
        Task<List<CustomerFeedbackSurveyDataDTO>> GetCustomerFeedbackSurveyDatas(List<KeyValuePair<CustomerFeedbackSurveyDataDTO.SearchByCustomerFeedbackSurveyDataParameters, string>> searchParameters, SqlTransaction sqlTransaction = null);
        Task<string> SaveCustomerFeedbackSurveyDatas(List<CustomerFeedbackSurveyDataDTO> customerFeedbackSurveyDataDTOList);
    }
}
