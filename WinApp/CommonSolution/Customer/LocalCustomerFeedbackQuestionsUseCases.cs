/********************************************************************************************
* Project Name - Customer
* Description  - LocalCustomerFeedbackQuestionsUseCases class 
*  
**************
**Version Log
**************
*Version     Date             Modified By               Remarks          
*********************************************************************************************
2.120.00    04-May-2021       Roshan Devadiga            Created
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
    /// LocalCustomerFeedbackQuestionsUseCases
    /// </summary>
    public class LocalCustomerFeedbackQuestionsUseCases:ICustomerFeedbackQuestionsUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalCustomerFeedbackQuestionsUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<CustomerFeedbackQuestionsDTO>> GetCustomerFeedbackQuestions(List<KeyValuePair<CustomerFeedbackQuestionsDTO.SearchByCustomerFeedbackQuestionsParameters, string>> searchParameters, int LanguageId, SqlTransaction sqlTransaction = null)
        {
            return await Task<List<CustomerFeedbackQuestionsDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters, LanguageId, sqlTransaction);


                CustomerFeedbackQuestionsList customerFeedbackQuestionsList = new CustomerFeedbackQuestionsList(executionContext);

                List<CustomerFeedbackQuestionsDTO> customerFeedbackQuestionsDTOList = customerFeedbackQuestionsList.GetAllCustomerFeedbackQuestions(searchParameters, LanguageId, sqlTransaction);

                log.LogMethodExit(customerFeedbackQuestionsDTOList);
                return customerFeedbackQuestionsDTOList;
            });
        }
        public async Task<string> SaveCustomerFeedbackQuestions(List<CustomerFeedbackQuestionsDTO> customerFeedbackQuestionsDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(customerFeedbackQuestionsDTOList);
                    if (customerFeedbackQuestionsDTOList == null)
                    {
                        throw new ValidationException("customerFeedbackQuestionsDTOList is Empty");
                    }
                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            CustomerFeedbackQuestionsList customerFeedbackQuestionsList = new CustomerFeedbackQuestionsList(executionContext, customerFeedbackQuestionsDTOList);
                            customerFeedbackQuestionsList.Save();
                            parafaitDBTrx.EndTransaction();
                        }
                        catch (ValidationException valEx)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(valEx);
                            throw valEx;
                        }
                        catch (Exception ex)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            throw ex;
                        }
                    }
                    result = "Success";
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    result = "Falied";
                }
                log.LogMethodExit(result);
                return result;
            });
        }
    }
}
