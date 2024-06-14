/********************************************************************************************
* Project Name - Customer
* Description  - Specification of the Customer use cases. 
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.120.00   04-May-2021   Roshan Devadiga          Created
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Customer
{
    /// <summary>
    /// ICustomerFeedbackQuestionsUseCases
    /// </summary>
    public interface ICustomerFeedbackQuestionsUseCases
    {
        Task<List<CustomerFeedbackQuestionsDTO>> GetCustomerFeedbackQuestions(List<KeyValuePair<CustomerFeedbackQuestionsDTO.SearchByCustomerFeedbackQuestionsParameters, string>> searchParameters, int LanguageId, SqlTransaction sqlTransaction = null);
        Task<string> SaveCustomerFeedbackQuestions(List<CustomerFeedbackQuestionsDTO> customerFeedbackQuestionsDTOList);

    }
}
