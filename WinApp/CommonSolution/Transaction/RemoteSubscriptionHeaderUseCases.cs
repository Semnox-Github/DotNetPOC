/********************************************************************************************
 * Project Name - RemoteSubscriptionHeaderUseCases
 * Description  - RemoteSubscriptionHeaderUseCases class 
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
using System.Threading.Tasks;
using Newtonsoft.Json;
using Semnox.Core.Utilities;
using Semnox.Parafait.Transaction;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// RemoteSubscriptionHeaderUseCases
    /// </summary>
    public class RemoteSubscriptionHeaderUseCases : RemoteUseCases, ISubscriptionHeaderUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string SUBSCRIPTION_HEADER_URL = "api/Transaction/SubscriptionHeaders";
        /// <summary>
        /// RemoteSubscriptionHeaderUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        public RemoteSubscriptionHeaderUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }
        /// <summary>
        /// GetSubscriptionHeader
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="utilities"></param>
        /// <param name="loadChildren"></param>
        /// <returns></returns>
        public async Task<List<SubscriptionHeaderDTO>> GetSubscriptionHeader(List<KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>> parameters, Utilities utilities, bool loadChildren)
        {
            log.LogMethodEntry(parameters, loadChildren);

            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters, loadChildren));
            }
            try
            {
                List<SubscriptionHeaderDTO> result = await Get<List<SubscriptionHeaderDTO>>(SUBSCRIPTION_HEADER_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
         

        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>> subscriptionHeaderSearchParams, bool loadChildren)
        {
            log.LogMethodEntry(subscriptionHeaderSearchParams, loadChildren);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string> searchParameter in subscriptionHeaderSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case SubscriptionHeaderDTO.SearchByParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                    case SubscriptionHeaderDTO.SearchByParameters.CUSTOMER_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("customerId".ToString(), searchParameter.Value));
                        }
                        break;
                    case SubscriptionHeaderDTO.SearchByParameters.CUSTOMER_CREDIT_CARD_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("customerCreditCardsId".ToString(), searchParameter.Value));
                        }
                        break;
                    case SubscriptionHeaderDTO.SearchByParameters.PRODUCTS_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("productsId".ToString(), searchParameter.Value));
                        }
                        break;
                    case SubscriptionHeaderDTO.SearchByParameters.CUSTOMER_CONTACT_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("customerContactId".ToString(), searchParameter.Value));
                        }
                        break;
                    case SubscriptionHeaderDTO.SearchByParameters.SUBSCRIPTION_HEADER_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("subscriptionHeaderId".ToString(), searchParameter.Value));
                        }
                        break;
                    case SubscriptionHeaderDTO.SearchByParameters.TRANSACTION_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("transactionId".ToString(), searchParameter.Value));
                        }
                        break;
                    case SubscriptionHeaderDTO.SearchByParameters.PRODUCT_SUBSCRIPTION_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("productSubscriptionId".ToString(), searchParameter.Value));
                        }
                        break;
                    case SubscriptionHeaderDTO.SearchByParameters.PRODUCT_SUBSCRIPTION_NAME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("subscriptionProductName".ToString(), searchParameter.Value));
                        }
                        break;
                    case SubscriptionHeaderDTO.SearchByParameters.STATUS:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("status".ToString(), searchParameter.Value));
                        }
                        break;
                    case SubscriptionHeaderDTO.SearchByParameters.AUTO_RENEW:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("autoRenew".ToString(), searchParameter.Value));
                        }
                        break;
                    case SubscriptionHeaderDTO.SearchByParameters.SELECTED_PAYMENT_COLLECTION_MODE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("selectedPaymentCollectionMode".ToString(), searchParameter.Value));
                        }
                        break;
                    case SubscriptionHeaderDTO.SearchByParameters.HAS_UNBILLED_CYCLES:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("hasUnbilledCycles".ToString(), searchParameter.Value));
                        }
                        break;
                    case SubscriptionHeaderDTO.SearchByParameters.NOT_REACHED_PAYMENT_RETRY_LIMIT:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("notReachedPaymentRetryLimit".ToString(), searchParameter.Value));
                        }
                        break;
                    case SubscriptionHeaderDTO.SearchByParameters.REACHED_PAYMENT_RETRY_LIMIT:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("reachedPaymentRetryLimit".ToString(), searchParameter.Value));
                        }
                        break;
                    case SubscriptionHeaderDTO.SearchByParameters.CUSTOMER_FIRST_NAME_LIKE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("customerFirstNameLike".ToString(), searchParameter.Value));
                        }
                        break;
                    case SubscriptionHeaderDTO.SearchByParameters.LATEST_BILL_CYCLE_HAS_PAYMENT_ERROR:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("latestBillCycleHasPaymentError".ToString(), searchParameter.Value));
                        }
                        break;
                    case SubscriptionHeaderDTO.SearchByParameters.HAS_EXPIRED_CREDIT_CARD:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("hasExpiredcreditCard".ToString(), searchParameter.Value));
                        }
                        break;
                    case SubscriptionHeaderDTO.SearchByParameters.CREDIT_CARD_EXPIRES_BEFORE_NEXT_BILLING:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("creditCardExpiresBeforeNextBilling".ToString(), searchParameter.Value));
                        }
                        break;
                    case SubscriptionHeaderDTO.SearchByParameters.SUBSCRIPTION_EXPIRES_IN_XDAYS:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("subscriptionExpiresInXDays".ToString(), searchParameter.Value));
                        }
                        break;
                    case SubscriptionHeaderDTO.SearchByParameters.CREATION_DATE_LESS_THAN:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("creationDateLessThan".ToString(), searchParameter.Value));
                        }
                        break;
                    case SubscriptionHeaderDTO.SearchByParameters.CREATION_DATE_GREATER_EQUAL_TO:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("creationDateGreaterEqualTo".ToString(), searchParameter.Value));
                        }
                        break;
                    case SubscriptionHeaderDTO.SearchByParameters.SUBSCRIPTION_IS_EXPIRED:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("subscriptionIsExpired".ToString(), searchParameter.Value));
                        }
                        break;
                    case SubscriptionHeaderDTO.SearchByParameters.HAS_PAST_PENDING_BILL_CYCLES:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("hasPastPendingBillCycles".ToString(), searchParameter.Value));
                        }
                        break;
                    case SubscriptionHeaderDTO.SearchByParameters.RENEWAL_REMINDER_IN_X_DAYS_IS_TRUE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("renewalReminderExpiresInXDaysIsTrue".ToString(), searchParameter.Value));
                        }
                        break; 
                }
                searchParameterList.Add(new KeyValuePair<string, string>("loadChildern".ToString(),(loadChildren ? "1":"0")));
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }
        /// <summary>
        /// SaveSubscriptionHeader
        /// </summary>
        /// <param name="subscriptionHeaderDTOList"></param>
        /// <returns></returns>
        public async Task<string> SaveSubscriptionHeader(List<SubscriptionHeaderDTO> subscriptionHeaderDTOList)
        {
            log.LogMethodEntry(subscriptionHeaderDTOList);
            try
            {
                string result = await Post<string>(SUBSCRIPTION_HEADER_URL, subscriptionHeaderDTOList);
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
