/********************************************************************************************
 * Project Name - LocalCustomerCreditCardsUseCases
 * Description  - LocalCustomerCreditCardsUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By           Remarks          
 *********************************************************************************************
 2.110.0      25-Jan-2021      Guru S A             For Subscription changes
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// LocalCustomerCreditCardsUseCases
    /// </summary>
    public class LocalCustomerCreditCardsUseCases : ICustomerCreditCardsUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        /// <summary>
        /// LocalSubscriptionHeaderUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        public LocalCustomerCreditCardsUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// GetCustomerCreditCards
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="utilities"></param> 
        /// <returns></returns>
        public async Task<List<CustomerCreditCardsDTO>> GetCustomerCreditCards(List<KeyValuePair<CustomerCreditCardsDTO.SearchByParameters, string>> searchParameters, Utilities utilities)
        {
            log.LogMethodEntry(searchParameters);
            return await Task<List<CustomerCreditCardsDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);
                CustomerCreditCardsListBL customerCreditCardsListBL = new CustomerCreditCardsListBL(executionContext);
                List<CustomerCreditCardsDTO> customerCreditCardsDTOList = customerCreditCardsListBL.GetCustomerCreditCardsDTOList(searchParameters, utilities);
                log.LogMethodExit(customerCreditCardsDTOList);
                return customerCreditCardsDTOList;
            });
        }
        /// <summary>
        /// SaveCustomerCreditCards
        /// </summary>
        /// <param name="customerCreditCardsDTOList"></param>
        /// <returns></returns>
        public async Task<string> SaveCustomerCreditCards(List<CustomerCreditCardsDTO> customerCreditCardsDTOList)
        {
            log.LogMethodEntry(customerCreditCardsDTOList);
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    if (customerCreditCardsDTOList == null)
                    {
                        throw new ValidationException("CustomerCreditCardsDTO list is empty");
                    }

                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    { 
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            CustomerCreditCardsListBL customerCreditCardsListBL = new CustomerCreditCardsListBL(executionContext, customerCreditCardsDTOList);
                            customerCreditCardsListBL.Save(parafaitDBTrx.SQLTrx);
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
                            throw new Exception(ex.Message, ex);
                        }
                    }
                    result = "Success";
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    throw ex;
                }
                log.LogMethodExit(result);
                return result;
            });
        }
         
    }
}
