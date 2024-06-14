/********************************************************************************************
 * Project Name - CustomerFeedbackSurveyDetail
 * Description  - LocalCustomerFeedbackSurveyDetailsUseCases class
 *
 **************
 ** Version Log
  **************
  * Version      Date              Modified By         Remarks
 *********************************************************************************************
 2.120.0         04-May-2021       B Mahesh Pai        Created
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
    class LocalCustomerFeedbackSurveyDetailsUseCases:ICustomerFeedbackSurveyDetailsUseCases
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalCustomerFeedbackSurveyDetailsUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<CustomerFeedbackSurveyDetailsDTO>> GetCustomerFeedbackSurveyDetails(List<KeyValuePair<CustomerFeedbackSurveyDetailsDTO.SearchByCustomerFeedbackSurveyDetailsParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
      {
            return await Task<List<CustomerFeedbackSurveyDetailsDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                CustomerFeedbackSurveyDetailsList customerFeedbackSurveyDetailsList = new CustomerFeedbackSurveyDetailsList(executionContext);
                List<CustomerFeedbackSurveyDetailsDTO> customerFeedbackSurveyDetailsDTOlist = customerFeedbackSurveyDetailsList.GetCustomerFeedbackSurveyDetailsOfInitialLoadList(searchParameters, sqlTransaction);

                log.LogMethodExit(customerFeedbackSurveyDetailsDTOlist);
                return customerFeedbackSurveyDetailsDTOlist;
            });
        }
        public async Task<string> SaveCustomerFeedbackSurveyDetails(List<CustomerFeedbackSurveyDetailsDTO> customerFeedbackSurveyDetailsDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(customerFeedbackSurveyDetailsDTOList);
                    if (customerFeedbackSurveyDetailsDTOList == null)
                    {
                        throw new ValidationException("customerFeedbackSurveyDetailsDTOList is Empty");
                    }
                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            CustomerFeedbackSurveyDetailsList customerFeedbackSurveyDetailsList = new CustomerFeedbackSurveyDetailsList(executionContext, customerFeedbackSurveyDetailsDTOList);
                            customerFeedbackSurveyDetailsList.Save();
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
