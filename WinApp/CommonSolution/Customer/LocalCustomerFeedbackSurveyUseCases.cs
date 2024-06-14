/********************************************************************************************
 * Project Name - CustomerFeedbackSurvey
 * Description  - LocalCustomerFeedbackSurveyUseCases class
 *
 **************
 ** Version Log
  **************
  * Version      Date              Modified By         Remarks
 *********************************************************************************************
 2.120.0         05-May-2021       B Mahesh Pai        Created
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
    class LocalCustomerFeedbackSurveyUseCases:ICustomerFeedbackSurveyUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ExecutionContext executionContext;
        public LocalCustomerFeedbackSurveyUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<CustomerFeedbackSurveyDTO>> GetCustomerFeedbackSurveys(List<KeyValuePair<CustomerFeedbackSurveyDTO.SearchByCustomerFeedBackSurveyParameters, string>> searchParameters,
                                             string posMachine = null, bool loadChildRecords = false, bool activeChildRecords = true,
                                             SqlTransaction sqlTransaction = null)

        {
            return await Task<List<CustomerFeedbackSurveyDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters, loadChildRecords, activeChildRecords, sqlTransaction);

                CustomerFeedbackSurveyList customerFeedbackSurveyList = new CustomerFeedbackSurveyList(executionContext);
                List<CustomerFeedbackSurveyDTO> customerFeedbackSurveyDTOList = customerFeedbackSurveyList.GetAllCustomerFeedbackSurvey(searchParameters, posMachine, loadChildRecords, activeChildRecords, null);

                log.LogMethodExit(customerFeedbackSurveyDTOList);
                return customerFeedbackSurveyDTOList;
            });
        }
        public async Task<string> SaveCustomerFeedbackSurveys(List<CustomerFeedbackSurveyDTO> customerFeedbackSurveyDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(customerFeedbackSurveyDTOList);
                    if (customerFeedbackSurveyDTOList == null)
                    {
                        throw new ValidationException("customerFeedbackSurveyDTOList is Empty");
                    }
                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            CustomerFeedbackSurveyList customerFeedbackSurveyList = new CustomerFeedbackSurveyList(executionContext, customerFeedbackSurveyDTOList);
                            customerFeedbackSurveyList.Save();
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
