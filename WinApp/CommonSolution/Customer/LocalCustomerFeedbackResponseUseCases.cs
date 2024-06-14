/********************************************************************************************
* Project Name - Customer
* Description  - LocalCustomerFeedbackResponseUseCases class 
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
    public class LocalCustomerFeedbackResponseUseCases:ICustomerFeedbackResponseUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalCustomerFeedbackResponseUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<CustomerFeedbackResponseDTO>> GetCustomerFeedbackResponses(List<KeyValuePair<CustomerFeedbackResponseDTO.SearchByCustomerFeedbackResponseParameters, string>> searchParameters,
        bool loadChildRecords = false, bool activeChildRecords = true,
        SqlTransaction sqlTransaction = null)
        {
            return await Task<List<CustomerFeedbackResponseDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters, loadChildRecords, activeChildRecords, sqlTransaction);

                CustomerFeedbackResponseList customerFeedbackResponseList = new CustomerFeedbackResponseList(executionContext);
                List<CustomerFeedbackResponseDTO> customerFeedbackResponseDTOList = customerFeedbackResponseList.GetAllCustomerFeedbackResponseDTOList(searchParameters, loadChildRecords, activeChildRecords, sqlTransaction);

                log.LogMethodExit(customerFeedbackResponseDTOList);
                return customerFeedbackResponseDTOList;
            });
        }
        public async Task<string> SaveCustomerFeedbackResponses(List<CustomerFeedbackResponseDTO> customerFeedbackResponseDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(customerFeedbackResponseDTOList);
                    if (customerFeedbackResponseDTOList == null)
                    {
                        throw new ValidationException("customerFeedbackResponseDTOList is Empty");
                    }
                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            CustomerFeedbackResponseList customerFeedbackResponseList = new CustomerFeedbackResponseList(executionContext, customerFeedbackResponseDTOList);
                            customerFeedbackResponseList.Save();
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
