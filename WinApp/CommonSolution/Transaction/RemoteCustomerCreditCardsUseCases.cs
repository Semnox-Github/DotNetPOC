/********************************************************************************************
 * Project Name - RemoteCustomerCreditCardsUseCases
 * Description  - RemoteCustomerCreditCardsUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date              Modified By        Remarks          
 *********************************************************************************************
 2.110.0      14-Dec-2020       Deeksha            Created :Inventory UI/POS UI re-design with REST API
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Semnox.Core.Utilities;
using Semnox.Parafait.Transaction;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// RemoteCustomerCreditCardsUseCases
    /// </summary>
    public class RemoteCustomerCreditCardsUseCases : RemoteUseCases, ICustomerCreditCardsUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string CUSTOMER_CREDIT_CARDS_HEADER_URL = "api/Transaction/CustomerCreditCards";
        /// <summary>
        /// RemoteCustomerCreditCardsUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        public RemoteCustomerCreditCardsUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }
        /// <summary>
        /// GetCustomerCreditCards
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="utilities"></param> 
        /// <returns></returns>
        public async Task<List<CustomerCreditCardsDTO>> GetCustomerCreditCards(List<KeyValuePair<CustomerCreditCardsDTO.SearchByParameters, string>> parameters, Utilities utilities)
        {
            log.LogMethodEntry(parameters);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            try
            {
                List<CustomerCreditCardsDTO> result = await Get<List<CustomerCreditCardsDTO>>(CUSTOMER_CREDIT_CARDS_HEADER_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
         

        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<CustomerCreditCardsDTO.SearchByParameters, string>> searchParams)
        {
            log.LogMethodEntry(searchParams);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<CustomerCreditCardsDTO.SearchByParameters, string> searchParameter in searchParams)
            {
                switch (searchParameter.Key)
                {

                    case CustomerCreditCardsDTO.SearchByParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                    case CustomerCreditCardsDTO.SearchByParameters.CUSTOMER_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("customerId".ToString(), searchParameter.Value));
                        }
                        break;
                    case CustomerCreditCardsDTO.SearchByParameters.CUSTOMER_CREDITCARDS_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("customerCreditCardsId".ToString(), searchParameter.Value));
                        }
                        break;
                    case CustomerCreditCardsDTO.SearchByParameters.TOKEN_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("tokeId".ToString(), searchParameter.Value));
                        }
                        break;
                    case CustomerCreditCardsDTO.SearchByParameters.CARD_PROFILE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("cardProfileId".ToString(), searchParameter.Value));
                        }
                        break;
                    case CustomerCreditCardsDTO.SearchByParameters.PAYMENT_MODE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("paymentModeId".ToString(), searchParameter.Value));
                        }
                        break;
                    case CustomerCreditCardsDTO.SearchByParameters.LINKED_WITH_ACTIVE_SUBSCRIPTIONS:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("linkedWithActiveSubscriptions".ToString(), searchParameter.Value));
                        }
                        break;
                    case CustomerCreditCardsDTO.SearchByParameters.EXPIREDCARD_LINKED_WITH_UNBILLED_SUBSCRIPTIONS:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("expiredCardLinkedWithUnbilledSubscriptions".ToString(), searchParameter.Value));
                        }
                        break;
                    case CustomerCreditCardsDTO.SearchByParameters.CARDS_EXPIRING_IN_X_DAYS:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("cardExpiresInXDays".ToString(), searchParameter.Value));
                        }
                        break;
                    case CustomerCreditCardsDTO.SearchByParameters.CARDS_EXPIRING_BEFORE_NEXT_UNBILLED_CYCLE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("cardExpiringBeforeNextUnbilledCycle".ToString(), searchParameter.Value));
                        }
                        break; 
                } 
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }
        /// <summary>
        /// SaveCustomerCreditCards
        /// </summary>
        /// <param name="customerCreditCardsDTOList"></param>
        /// <returns></returns>
        public async Task<string> SaveCustomerCreditCards(List<CustomerCreditCardsDTO> customerCreditCardsDTOList)
        {
            log.LogMethodEntry(customerCreditCardsDTOList);
            try
            {
                string result = await Post<string>(CUSTOMER_CREDIT_CARDS_HEADER_URL, customerCreditCardsDTOList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            } 
        }
         
    }
}
