/********************************************************************************************
 * Project Name - CustomerFeedbackSurvey
 * Description  - ICustomerFeedbackSurveyUseCases class
 *
 **************
 ** Version Log
  **************
  * Version      Date              Modified By         Remarks
 *********************************************************************************************
 2.120.0         05-May-2021       B Mahesh Pai       Created
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Customer
{
    public interface ICustomerFeedbackSurveyUseCases
    {
        Task<List<CustomerFeedbackSurveyDTO>> GetCustomerFeedbackSurveys(List<KeyValuePair<CustomerFeedbackSurveyDTO.SearchByCustomerFeedBackSurveyParameters, string>> searchParameters,
                                             string posMachine = null, bool loadChildRecords = false, bool activeChildRecords = true,
                                             SqlTransaction sqlTransaction = null);
        Task<string> SaveCustomerFeedbackSurveys(List<CustomerFeedbackSurveyDTO> customerFeedbackSurveyDTOList);

    }
}
