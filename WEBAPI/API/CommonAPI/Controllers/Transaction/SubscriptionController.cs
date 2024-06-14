/********************************************************************************************
 * Project Name - SubscriptionController
 * Description  - Created to fetch, update and insert Subscription details.   
 *  
 **************
 **Version Log
 **************
 *Version     Date            Modified By              Remarks          
 ********************************************************************************************* 
 *2.110.0     25-Jan-2021     Guru S A                 Created for Subscription feature
 *2.130.0     07-Sep-2021     Nitin Pai                Removed site id check from filter as subscription is across sites
 ********************************************************************************************/


using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.User;

namespace Semnox.CommonAPI.Transaction
{
    public class SubscriptionController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Get the JSON Object Subscription Details List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Transaction/SubscriptionHeaders")]
        public async Task<HttpResponseMessage> Get(string isActive = null, int customerId = -1, int customerCreditCardsId = -1, int productsId = -1, int customerContactId = -1,
                                                   int subscriptionHeaderId = -1, int transactionId = -1, int productSubscriptionId =-1, string subscriptionProductName = null, string status = null, 
                                                   bool? autoRenew = null, string selectedPaymentCollectionMode = null,  bool? hasUnbilledCycles = null, int? reachedPaymentRetryLimit = null, int? notReachedPaymentRetryLimit = null,
                                                   string customerFirstNameLike = null, bool? latestBillCycleHasPaymentError = null, bool? hasExpiredcreditCard = null,
                                                   bool? creditCardExpiresBeforeNextBilling = null, int? subscriptionExpiresInXDays = null, DateTime? creationDateLessThan = null,
                                                   DateTime? creationDateGreaterEqualTo = null, bool? subscriptionIsExpired = null, bool? hasPastPendingBillCycles = null, 
                                                   bool? renewalReminderExpiresInXDaysIsTrue = null, bool loadChildern = true)
        {
            log.LogMethodEntry(isActive, customerId, customerCreditCardsId, productsId, customerContactId, subscriptionHeaderId, transactionId, productSubscriptionId, subscriptionProductName, status, 
                                                    autoRenew, selectedPaymentCollectionMode, hasUnbilledCycles, reachedPaymentRetryLimit, notReachedPaymentRetryLimit, customerFirstNameLike, latestBillCycleHasPaymentError,
                                                    hasExpiredcreditCard, creditCardExpiresBeforeNextBilling, subscriptionExpiresInXDays, creationDateLessThan, 
                                           creationDateGreaterEqualTo, subscriptionIsExpired, hasPastPendingBillCycles, renewalReminderExpiresInXDaysIsTrue);
            ExecutionContext executionContext = null;
            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                Users user = new Users(executionContext, executionContext.GetUserId(), executionContext.GetSiteId());
                Semnox.Core.Utilities.Utilities Utilities = new Semnox.Core.Utilities.Utilities();
                Utilities.ParafaitEnv.IsCorporate = executionContext.GetIsCorporate();
                Utilities.ParafaitEnv.User_Id = user.UserDTO.UserId;
                Utilities.ParafaitEnv.LoginID = user.UserDTO.LoginId;
                Utilities.ParafaitEnv.RoleId = user.UserDTO.RoleId;
                Utilities.ParafaitEnv.SetPOSMachine("", Environment.MachineName);
                Utilities.ParafaitEnv.IsCorporate = executionContext.GetIsCorporate();
                Utilities.ParafaitEnv.SiteId = executionContext.GetSiteId();
                Utilities.ExecutionContext.SetIsCorporate(executionContext.GetIsCorporate());
                Utilities.ExecutionContext.SetSiteId(executionContext.GetSiteId());
                Utilities.ExecutionContext.SetUserId(user.UserDTO.LoginId);
                Utilities.ParafaitEnv.Initialize();  

                ISubscriptionHeaderUseCases subscriptionHeaderUseCases = TransactionUseCaseFactory.GetSubscriptionHeaderUseCases(executionContext);
                List<KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>>();
                //searchParameters.Add(new KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>(SubscriptionHeaderDTO.SearchByParameters.SITE_ID, Convert.ToString(executionContext.SiteId)));
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>(SubscriptionHeaderDTO.SearchByParameters.IS_ACTIVE, isActive));
                    }
                }
                if (customerId > -1)
                {
                    searchParameters.Add(new KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>(SubscriptionHeaderDTO.SearchByParameters.CUSTOMER_ID, customerId.ToString()));
                }

                if (customerCreditCardsId > -1)
                {
                    searchParameters.Add(new KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>(SubscriptionHeaderDTO.SearchByParameters.CUSTOMER_CREDIT_CARD_ID, customerCreditCardsId.ToString()));
                }

                if (productsId > -1)
                {
                    searchParameters.Add(new KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>(SubscriptionHeaderDTO.SearchByParameters.PRODUCTS_ID, productsId.ToString()));
                }
                if (customerContactId > -1)
                {
                    searchParameters.Add(new KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>(SubscriptionHeaderDTO.SearchByParameters.CUSTOMER_CONTACT_ID, customerContactId.ToString()));
                }

                if (subscriptionHeaderId > -1)
                {
                    searchParameters.Add(new KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>(SubscriptionHeaderDTO.SearchByParameters.SUBSCRIPTION_HEADER_ID, subscriptionHeaderId.ToString()));
                }

                if (transactionId > -1)
                {
                    searchParameters.Add(new KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>(SubscriptionHeaderDTO.SearchByParameters.TRANSACTION_ID, transactionId.ToString()));
                }

                if (productSubscriptionId > -1)
                {
                    searchParameters.Add(new KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>(SubscriptionHeaderDTO.SearchByParameters.PRODUCT_SUBSCRIPTION_ID, productSubscriptionId.ToString()));
                }   
                if (string.IsNullOrWhiteSpace(subscriptionProductName) == false)
                {
                    searchParameters.Add(new KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>(SubscriptionHeaderDTO.SearchByParameters.PRODUCT_SUBSCRIPTION_NAME, subscriptionProductName));
                }

                if (string.IsNullOrWhiteSpace(status) == false)
                {
                    searchParameters.Add(new KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>(SubscriptionHeaderDTO.SearchByParameters.STATUS, status));
                }
                
                if (autoRenew != null)
                {
                    searchParameters.Add(new KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>(SubscriptionHeaderDTO.SearchByParameters.AUTO_RENEW, (autoRenew == true? "1":"0")));
                }

                if (string.IsNullOrWhiteSpace(selectedPaymentCollectionMode) == false)
                {
                    searchParameters.Add(new KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>(SubscriptionHeaderDTO.SearchByParameters.SELECTED_PAYMENT_COLLECTION_MODE, selectedPaymentCollectionMode));
                }

                if (hasUnbilledCycles != null)
                {
                    searchParameters.Add(new KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>(SubscriptionHeaderDTO.SearchByParameters.HAS_UNBILLED_CYCLES, (hasUnbilledCycles == true ? "1" : "0")));
                }

                if (reachedPaymentRetryLimit != null)
                {
                    searchParameters.Add(new KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>(SubscriptionHeaderDTO.SearchByParameters.REACHED_PAYMENT_RETRY_LIMIT, ((int)reachedPaymentRetryLimit).ToString()));
                }
                if (notReachedPaymentRetryLimit != null)
                {
                    searchParameters.Add(new KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>(SubscriptionHeaderDTO.SearchByParameters.NOT_REACHED_PAYMENT_RETRY_LIMIT, ((int)notReachedPaymentRetryLimit).ToString()));
                }

                if (string.IsNullOrWhiteSpace(customerFirstNameLike) == false)
                {
                    searchParameters.Add(new KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>(SubscriptionHeaderDTO.SearchByParameters.CUSTOMER_FIRST_NAME_LIKE, customerFirstNameLike));
                }

                if (latestBillCycleHasPaymentError != null)
                {
                    searchParameters.Add(new KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>(SubscriptionHeaderDTO.SearchByParameters.LATEST_BILL_CYCLE_HAS_PAYMENT_ERROR, (latestBillCycleHasPaymentError == true ? "1" : "0")));
                }

                if (hasExpiredcreditCard != null)
                {
                    searchParameters.Add(new KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>(SubscriptionHeaderDTO.SearchByParameters.HAS_EXPIRED_CREDIT_CARD, (hasExpiredcreditCard == true ? "1" : "0")));
                }

                if (creditCardExpiresBeforeNextBilling != null)
                {
                    searchParameters.Add(new KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>(SubscriptionHeaderDTO.SearchByParameters.CREDIT_CARD_EXPIRES_BEFORE_NEXT_BILLING, (creditCardExpiresBeforeNextBilling == true ? "1" : "0")));
                }

                if (subscriptionExpiresInXDays != null)
                {
                    searchParameters.Add(new KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>(SubscriptionHeaderDTO.SearchByParameters.SUBSCRIPTION_EXPIRES_IN_XDAYS, subscriptionExpiresInXDays.ToString()));
                }
                
                if (creationDateLessThan != null)
                {
                    searchParameters.Add(new KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>(SubscriptionHeaderDTO.SearchByParameters.CREATION_DATE_LESS_THAN, ((DateTime)creationDateLessThan).ToString("MM-dd-yyyy HH:mm:ss")));
                }

                if (creationDateGreaterEqualTo != null)
                {
                    searchParameters.Add(new KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>(SubscriptionHeaderDTO.SearchByParameters.CREATION_DATE_GREATER_EQUAL_TO, ((DateTime)creationDateGreaterEqualTo).ToString("MM-dd-yyyy HH:mm:ss")));
                }

                if (subscriptionIsExpired != null)
                {
                    searchParameters.Add(new KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>(SubscriptionHeaderDTO.SearchByParameters.SUBSCRIPTION_IS_EXPIRED, (subscriptionIsExpired == true ? "1" : "0")));
                }

                if (hasPastPendingBillCycles != null)
                {
                    searchParameters.Add(new KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>(SubscriptionHeaderDTO.SearchByParameters.HAS_PAST_PENDING_BILL_CYCLES, (hasPastPendingBillCycles == true ? "1" : "0")));
                }

                if (renewalReminderExpiresInXDaysIsTrue != null)
                {
                    searchParameters.Add(new KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>(SubscriptionHeaderDTO.SearchByParameters.RENEWAL_REMINDER_IN_X_DAYS_IS_TRUE, (renewalReminderExpiresInXDaysIsTrue == true ? "1" : "0")));
                } 

               // SubscriptionHeaderListBL productSubscriptionListBL = new SubscriptionHeaderListBL(executionContext);
                List<SubscriptionHeaderDTO> SubscriptionHeaderDTOList = await subscriptionHeaderUseCases.GetSubscriptionHeader(searchParameters, Utilities, loadChildern);
                log.LogMethodExit(SubscriptionHeaderDTOList); 
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    data = SubscriptionHeaderDTOList 
                });
            }
            catch (UnauthorizedException ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }
        /// <summary>
        /// Performs a Post operation on SubscriptionHeaderDTO details.
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/Transaction/SubscriptionHeaders")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody] List<SubscriptionHeaderDTO> subscriptionHeaderDTOList)
        {
            ExecutionContext executionContext = null;
            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                ISubscriptionHeaderUseCases subscriptionHeaderUseCases = TransactionUseCaseFactory.GetSubscriptionHeaderUseCases(executionContext);
                //SubscriptionHeaderListBL productSubscriptionListBL = new SubscriptionHeaderListBL(executionContext, subscriptionHeaderDTOList);
                var content = await subscriptionHeaderUseCases.SaveSubscriptionHeader(subscriptionHeaderDTOList);
                log.LogMethodExit(content);
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    data = content
                });
            }
            catch (UnauthorizedException ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }
    }
}
