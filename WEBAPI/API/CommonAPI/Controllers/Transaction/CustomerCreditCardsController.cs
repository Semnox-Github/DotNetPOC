/********************************************************************************************
 * Project Name - CustomerCreditCardsController
 * Description  - Created to fetch, update and insert CustomerCreditCards details.   
 *  
 **************
 **Version Log
 **************
 *Version     Date            Modified By              Remarks          
 ********************************************************************************************* 
 *2.110.0     25-Jan-2021     Guru S A                 Created for Subscription feature
 *2.130.0     07-Sep-2021     Nitin Pai                Removed site id check from filter as this entity is across sites
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
using Semnox.Parafait.Transaction;
using Semnox.Parafait.User;

namespace Semnox.CommonAPI.Transaction
{
    /// <summary>
    /// CustomerCreditCardsController
    /// </summary>
    public class CustomerCreditCardsController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Get the JSON Object for CustomerCreditCards list
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Transaction/CustomerCreditCards")]
        public async Task<HttpResponseMessage> Get(string isActive = null, int customerId = -1, int customerCreditCardsId = -1, string tokeId = null, string cardProfileId = null,
                                                  int paymentModeId = -1, bool? linkedWithActiveSubscriptions = null, bool? expiredCardLinkedWithUnbilledSubscriptions = null,
                                                  int? cardExpiresInXDays = null, bool? cardExpiringBeforeNextUnbilledCycle = null)
        {
            log.LogMethodEntry(isActive, customerId, customerCreditCardsId, tokeId, cardProfileId, paymentModeId, linkedWithActiveSubscriptions,
                               expiredCardLinkedWithUnbilledSubscriptions, cardExpiresInXDays, cardExpiringBeforeNextUnbilledCycle);
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

                ICustomerCreditCardsUseCases customerCreditCardsUseCases = TransactionUseCaseFactory.GetCustomerCreditCardsUseCases(executionContext);
                List<KeyValuePair<CustomerCreditCardsDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CustomerCreditCardsDTO.SearchByParameters, string>>();
                //searchParameters.Add(new KeyValuePair<CustomerCreditCardsDTO.SearchByParameters, string>(CustomerCreditCardsDTO.SearchByParameters.SITE_ID, Convert.ToString(executionContext.SiteId)));
                if (string.IsNullOrWhiteSpace(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<CustomerCreditCardsDTO.SearchByParameters, string>(CustomerCreditCardsDTO.SearchByParameters.IS_ACTIVE, isActive));
                    }
                }
                if (customerId > -1)
                {
                    searchParameters.Add(new KeyValuePair<CustomerCreditCardsDTO.SearchByParameters, string>(CustomerCreditCardsDTO.SearchByParameters.CUSTOMER_ID, customerId.ToString()));
                }

                if (customerCreditCardsId > -1)
                {
                    searchParameters.Add(new KeyValuePair<CustomerCreditCardsDTO.SearchByParameters, string>(CustomerCreditCardsDTO.SearchByParameters.CUSTOMER_CREDITCARDS_ID, customerCreditCardsId.ToString()));
                }

                if (string.IsNullOrWhiteSpace(tokeId) == false)
                {
                    searchParameters.Add(new KeyValuePair<CustomerCreditCardsDTO.SearchByParameters, string>(CustomerCreditCardsDTO.SearchByParameters.TOKEN_ID, tokeId));
                }

                if (string.IsNullOrWhiteSpace(cardProfileId) == false)
                {
                    searchParameters.Add(new KeyValuePair<CustomerCreditCardsDTO.SearchByParameters, string>(CustomerCreditCardsDTO.SearchByParameters.CARD_PROFILE_ID, cardProfileId));
                }

                if (paymentModeId > -1)
                {
                    searchParameters.Add(new KeyValuePair<CustomerCreditCardsDTO.SearchByParameters, string>(CustomerCreditCardsDTO.SearchByParameters.PAYMENT_MODE_ID, paymentModeId.ToString()));
                }
                if (linkedWithActiveSubscriptions != null)
                {
                    searchParameters.Add(new KeyValuePair<CustomerCreditCardsDTO.SearchByParameters, string>(CustomerCreditCardsDTO.SearchByParameters.LINKED_WITH_ACTIVE_SUBSCRIPTIONS, ((bool)linkedWithActiveSubscriptions ? "1": "0")));
                }

                if (expiredCardLinkedWithUnbilledSubscriptions != null)
                {
                    searchParameters.Add(new KeyValuePair<CustomerCreditCardsDTO.SearchByParameters, string>(CustomerCreditCardsDTO.SearchByParameters.EXPIREDCARD_LINKED_WITH_UNBILLED_SUBSCRIPTIONS, ((bool)expiredCardLinkedWithUnbilledSubscriptions ? "1" : "0")));
                }  
                if (cardExpiresInXDays != null)
                {
                    searchParameters.Add(new KeyValuePair<CustomerCreditCardsDTO.SearchByParameters, string>(CustomerCreditCardsDTO.SearchByParameters.CARDS_EXPIRING_IN_X_DAYS, ((int)cardExpiresInXDays).ToString()));
                }

                if (cardExpiringBeforeNextUnbilledCycle != null)
                {
                    searchParameters.Add(new KeyValuePair<CustomerCreditCardsDTO.SearchByParameters, string>(CustomerCreditCardsDTO.SearchByParameters.CARDS_EXPIRING_BEFORE_NEXT_UNBILLED_CYCLE, ((bool)cardExpiringBeforeNextUnbilledCycle ? "1" : "0")));
                } 

                //CustomerCreditCardsListBL customerCreditCardsListBL = new CustomerCreditCardsListBL(executionContext);
                List<CustomerCreditCardsDTO> customerCreditCardsDTOList = await customerCreditCardsUseCases.GetCustomerCreditCards(searchParameters, Utilities);
                log.LogMethodExit(customerCreditCardsDTOList); 
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    data = customerCreditCardsDTOList 
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
        /// Performs a Post operation on CustomerCreditCardsDTO details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/Transaction/CustomerCreditCards")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody] List<CustomerCreditCardsDTO> customerCreditCardsDTOList)
        {
            ExecutionContext executionContext = null;
            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                ICustomerCreditCardsUseCases customerCreditCardsUseCases = TransactionUseCaseFactory.GetCustomerCreditCardsUseCases(executionContext);
                //CustomerCreditCardsListBL customerCreditCardsListBL = new CustomerCreditCardsListBL(executionContext, customerCreditCardsDTOList);
                var content = await customerCreditCardsUseCases.SaveCustomerCreditCards(customerCreditCardsDTOList);
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
