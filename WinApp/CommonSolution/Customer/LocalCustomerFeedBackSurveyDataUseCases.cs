/********************************************************************************************
 * Project Name - CustomerFeedBackSurveyData
 * Description  - LocalCustomerFeedBackSurveyDataUseCases class
 *
 **************
 ** Version Log
  **************
  * Version      Date              Modified By         Remarks
 *********************************************************************************************
 2.120.0         04-May-2021       B Mahesh Pai       Created
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
    class LocalCustomerFeedBackSurveyDataUseCases:ICustomerFeedBackSurveyDataUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalCustomerFeedBackSurveyDataUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<CustomerFeedbackSurveyDataDTO>> GetCustomerFeedbackSurveyDatas(List<KeyValuePair<CustomerFeedbackSurveyDataDTO.SearchByCustomerFeedbackSurveyDataParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            return await Task<List<CustomerFeedbackSurveyDataDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                CustomerFeedbackSurveyDataList customerFeedbackSurveyDataList = new CustomerFeedbackSurveyDataList(executionContext);
                List<CustomerFeedbackSurveyDataDTO> customerFeedbackSurveyDataDTOList = customerFeedbackSurveyDataList.GetAllCustomerFeedbackSurveyData(searchParameters, sqlTransaction);

                log.LogMethodExit(customerFeedbackSurveyDataDTOList);
                return customerFeedbackSurveyDataDTOList;
            });
        }

        public async Task<string> SaveCustomerFeedbackSurveyDatas(List<CustomerFeedbackSurveyDataDTO> customerFeedbackSurveyDataDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(customerFeedbackSurveyDataDTOList);
                    if (customerFeedbackSurveyDataDTOList == null)
                    {
                        throw new ValidationException("customerFeedbackSurveyDataDTOList is Empty");
                    }
                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            CustomerFeedbackSurveyDataList customerFeedbackSurveyDataList = new CustomerFeedbackSurveyDataList(executionContext, customerFeedbackSurveyDataDTOList);
                            customerFeedbackSurveyDataList.Save();
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
