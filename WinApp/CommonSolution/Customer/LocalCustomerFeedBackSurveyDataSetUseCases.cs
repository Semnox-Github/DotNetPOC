/********************************************************************************************
 * Project Name - CustomerFeedBackSurveyData
 * Description  - LocalCustomerFeedBackSurveyDataSetUseCases class
 *
 **************s
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
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Customer
{
    /// <summary>
    /// Implementation of LocalCustomerFeedBackSurveyDataSet use-cases
    /// </summary>
    public class LocalCustomerFeedBackSurveyDataSetUseCases : ICustomerFeedBackSurveyDataSetUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// LocalCustomerFeedBackSurveyDataSetUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        public LocalCustomerFeedBackSurveyDataSetUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// GetCustomerFeedbackSurveyDataSets
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        public async Task<List<CustomerFeedbackSurveyDataSetDTO>> GetCustomerFeedbackSurveyDataSets(List<KeyValuePair<CustomerFeedbackSurveyDataSetDTO.SearchByCustomerFeedbackSurveyDataSetParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            return await Task<List<CustomerFeedbackSurveyDataSetDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                CustomerFeedbackSurveyDataSetList customerFeedbackSurveyDataSetList = new CustomerFeedbackSurveyDataSetList(executionContext);
                List<CustomerFeedbackSurveyDataSetDTO> customerFeedbackSurveyDataSetDTOList = customerFeedbackSurveyDataSetList.GetAllCustomerFeedbackSurveyDataSetList(searchParameters, true, false, sqlTransaction);

                log.LogMethodExit(customerFeedbackSurveyDataSetDTOList);
                return customerFeedbackSurveyDataSetDTOList;
            });
        }

        /// <summary>
        /// SaveCustomerFeedbackSurveyDataSets
        /// </summary>
        /// <returns></returns>
        public async Task<List<CustomerFeedbackSurveyDataSetDTO>> SaveCustomerFeedbackSurveyDataSets(List<CustomerFeedbackSurveyDataSetDTO> customerFeedbackSurveyDataSetDTOList)
        {
            return await Task<List<CustomerFeedbackSurveyDataSetDTO>>.Factory.StartNew(() =>
            {
                List<CustomerFeedbackSurveyDataSetDTO> result = null;
                log.LogMethodEntry(customerFeedbackSurveyDataSetDTOList);
                if (customerFeedbackSurveyDataSetDTOList == null)
                {
                    throw new ValidationException("CustomerFeedbackSurveyDataSetDTOList is Empty");
                }
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    try
                    {
                        parafaitDBTrx.BeginTransaction();
                        CustomerFeedbackSurveyDataSetList customerFeedbackSurveyDataSetList = new CustomerFeedbackSurveyDataSetList(executionContext, customerFeedbackSurveyDataSetDTOList);
                        result = customerFeedbackSurveyDataSetList.Save(parafaitDBTrx.SQLTrx);
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
                log.LogMethodExit(result);
                return result;
            });
        }
    }
}
