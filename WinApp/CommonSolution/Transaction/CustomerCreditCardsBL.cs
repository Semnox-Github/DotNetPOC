/********************************************************************************************
 * Project Name - CustomerCreditCardsBL
 * Description  - BL CustomerCreditCards
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.110.0     09-Dec-2020       Guru S A       Created for Subscription changes                   
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Device.PaymentGateway;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// CustomerCreditCardsBL
    /// </summary>
    public class CustomerCreditCardsBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private CustomerCreditCardsDTO customerCreditCardsDTO;
        private int customerCardExpiresInXDays = 30;
        private int numberOfMessagesForCustomerCardExpiry = 3;
        private int frequencyInDaysForCustomerCardExpiryMessage = 5;

        private const string SUBSCRIPTION_FEATURE_SETUP = "SUBSCRIPTION_FEATURE_SETUP";
        private const string CUSTOMERCARDEXPIRESINXDAYS = "CustomerCardExpiresInXDays";
        private const string NUMBEROFMESSAGESFORCUSTOMERCARDEXPIRY = "NumberOfMessagesForCustomerCardExpiry";
        private const string FREQUENCYINDAYSFORCUSTOMERCARDEXPIRYMESSAGE = "FrequencyInDaysForCustomerCardExpiryMessage";
        private LookupValuesList serverDateTime;
        private PaymentModeDTO paymentModeDTO = null;
        private string passPhrase;

        /// <summary>
        /// Parameterized constructor of CustomerCreditCardsBL class
        /// </summary>
        private CustomerCreditCardsBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.serverDateTime = new LookupValuesList(executionContext);
            this.passPhrase = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CUSTOMER_ENCRYPTION_PASS_PHRASE");
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the customerCreditCards id as the parameter
        /// Would fetch the customerCreditCards object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="id">Id</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        public CustomerCreditCardsBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            CustomerCreditCardsDataHandler customerCreditCardsDataHandler = new CustomerCreditCardsDataHandler(this.passPhrase, sqlTransaction);
            customerCreditCardsDTO = customerCreditCardsDataHandler.GetCustomerCreditCardsDTO(id);
            if (customerCreditCardsDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "CustomerCreditCards", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates CustomerCreditCardsBL object using the CustomerCreditCardsDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="customerCreditCardsDTO">CustomerCreditCardsDTO object</param>
        public CustomerCreditCardsBL(ExecutionContext executionContext, CustomerCreditCardsDTO customerCreditCardsDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, customerCreditCardsDTO);
            this.customerCreditCardsDTO = customerCreditCardsDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the CustomerCreditCards
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            CustomerCreditCardsDataHandler customerCreditCardsDataHandler = new CustomerCreditCardsDataHandler(this.passPhrase, sqlTransaction);
            Validate(sqlTransaction);
            if (customerCreditCardsDTO.CustomerCreditCardsId < 0)
            {
                customerCreditCardsDTO = customerCreditCardsDataHandler.InsertCustomerCreditCards(customerCreditCardsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                customerCreditCardsDTO.AcceptChanges();
            }
            else
            {
                if (customerCreditCardsDTO.IsChanged)
                {
                    customerCreditCardsDTO = customerCreditCardsDataHandler.UpdateCustomerCreditCards(customerCreditCardsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    customerCreditCardsDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public CustomerCreditCardsDTO CustomerCreditCardsDTO
        {
            get
            {
                return customerCreditCardsDTO;
            }
        }

        /// <summary>
        /// Validates the customer relationship DTO
        /// </summary>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            if (customerCreditCardsDTO == null)
            {
                validationErrorList.Add(new ValidationError("customerCreditCards", "CustomerCreditCardsDTO", MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Customer Credit Cards DTO"))));
            }
            else
            {
                if (customerCreditCardsDTO.CustomerId == -1)
                {
                    validationErrorList.Add(new ValidationError("CustomerCreditCards", "CustomerId", MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Customer Id"))));
                }
                if (string.IsNullOrWhiteSpace(customerCreditCardsDTO.CardProfileId))
                {
                    validationErrorList.Add(new ValidationError("CustomerCreditCards", "CardProfileId", MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Card Profile Id"))));
                }
                if (string.IsNullOrWhiteSpace(customerCreditCardsDTO.TokenId))
                {
                    validationErrorList.Add(new ValidationError("CustomerCreditCards", "TokenId", MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Token Id"))));
                }
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }
        /// <summary>
        /// SendCardExpiryMessage
        /// </summary>
        /// <returns></returns>
        public void SendCardExpiryMessage(Utilities utilities)
        {
            log.LogMethodEntry(utilities);
            //bool ableToSend = false;
            try
            {
                if (CanSendCardExpiryMsg(utilities))
                {
                    SubscriptionEventsBL subscriptionEventsBL = new SubscriptionEventsBL(utilities, this.customerCreditCardsDTO);
                    subscriptionEventsBL.SendMessage(MessagingClientDTO.MessagingChanelType.NONE);
                    //ableToSend = true;
                    this.customerCreditCardsDTO.LastCreditCardExpiryReminderSentOn = serverDateTime.GetServerDateTime();
                    this.customerCreditCardsDTO.CreditCardExpiryReminderCount = (this.customerCreditCardsDTO.CreditCardExpiryReminderCount == null ? 1 : customerCreditCardsDTO.CreditCardExpiryReminderCount + 1);
                    Save(null);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                //ableToSend = false;
                throw;
            }
            log.LogMethodExit();
            //return ableToSend;
        }
        /// <summary>
        /// CanSendCardExpiryMsg
        /// </summary>
        public bool CanSendCardExpiryMsg(Utilities utilities)
        {
            log.LogMethodEntry();
            bool canSend = true;
            LoadAlertSetupDetails();
            bool expiredCard = CustomerCreditCardHasExpired(utilities);
            if (expiredCard)
            {
                //Sent count has crossed then dont send again
                if (this.customerCreditCardsDTO.CreditCardExpiryReminderCount != null
                    && this.customerCreditCardsDTO.CreditCardExpiryReminderCount > numberOfMessagesForCustomerCardExpiry)
                {
                    canSend = false;
                }
                //If last sent date is within frequence then dont send for now
                if (this.customerCreditCardsDTO.LastCreditCardExpiryReminderSentOn != null
                   && ((DateTime)this.customerCreditCardsDTO.LastCreditCardExpiryReminderSentOn).Date.AddDays(frequencyInDaysForCustomerCardExpiryMessage) < serverDateTime.GetServerDateTime())
                {
                    canSend = false;
                }
            }
            else
            {
                canSend = false;
            }
            log.LogMethodExit(canSend);
            return canSend;
        }
        /// <summary>
        /// CustomerCreditCardHasExpired
        /// </summary>
        /// <returns></returns>
        public bool CustomerCreditCardHasExpired(Utilities utilities)
        {
            log.LogMethodEntry();
            bool expiredCard = false;
            int expiryMonth = GetExpiryMonth(utilities);
            int expiryYear = GetExpiryYear(utilities);
            DateTime currentDateTime = serverDateTime.GetServerDateTime();
            int currentMonth = currentDateTime.Month;
            int currentYear = currentDateTime.Year;
            DateTime currentDate = new DateTime(currentYear, currentMonth, 01);
            DateTime cardExpiryDate = new DateTime(expiryYear, expiryMonth, 01);
            if (currentDate >= cardExpiryDate)
            {
                expiredCard = true;
            }
            log.LogMethodExit(expiredCard);
            return expiredCard;
        }

        private int GetExpiryYear(Utilities utilities)
        {
            log.LogMethodEntry();
            SetPaymentModeDTO();
            int yearValue = CreditCardPaymentGateway.GetCreditCardExpiryYear(utilities, paymentModeDTO, customerCreditCardsDTO.CardExpiry);

            log.LogMethodExit(yearValue);
            return yearValue;
        }

        private int GetExpiryMonth(Utilities utilities)
        {
            log.LogMethodEntry();
            SetPaymentModeDTO();
            int monthValue = CreditCardPaymentGateway.GetCreditCardExpiryMonth(utilities, paymentModeDTO, customerCreditCardsDTO.CardExpiry);
            log.LogMethodExit(monthValue);
            return monthValue;
        }

        private void SetPaymentModeDTO()
        {
            log.LogMethodEntry();
            if (paymentModeDTO == null)
            {
                PaymentMode paymentModeBL = new PaymentMode(executionContext, this.customerCreditCardsDTO.PaymentModeId);
                paymentModeDTO = paymentModeBL.GetPaymentModeDTO;
            }
            log.LogMethodExit();
        }

        private void LoadAlertSetupDetails()
        {
            log.LogMethodEntry();
            LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
            List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
            searchParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, SUBSCRIPTION_FEATURE_SETUP));
            List<LookupValuesDTO> lookupValuesDTOList = lookupValuesList.GetAllLookupValues(searchParameters);
            if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
            {
                for (int i = 0; i < lookupValuesDTOList.Count(); i++)
                {
                    int value = 0;
                    switch (lookupValuesDTOList[i].LookupValue)
                    {
                        case CUSTOMERCARDEXPIRESINXDAYS:
                            if (int.TryParse(lookupValuesDTOList[i].Description, out value))
                            {
                                customerCardExpiresInXDays = value;
                            }
                            break;
                        case NUMBEROFMESSAGESFORCUSTOMERCARDEXPIRY:
                            if (int.TryParse(lookupValuesDTOList[i].Description, out value))
                            {
                                numberOfMessagesForCustomerCardExpiry = value;
                            }
                            break;
                        case FREQUENCYINDAYSFORCUSTOMERCARDEXPIRYMESSAGE:
                            if (int.TryParse(lookupValuesDTOList[i].Description, out value))
                            {
                                frequencyInDaysForCustomerCardExpiryMessage = value;
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// CustomerCreditCardExpiresbefore
        /// </summary>
        /// <param name="utilities"></param>
        /// <param name="billOnDate"></param>
        /// <returns></returns>
        public bool CustomerCreditCardExpiresbefore(Utilities utilities, DateTime billOnDate)
        {
            log.LogMethodEntry(billOnDate);
            bool expiredCard = false;
            int expiryMonth = GetExpiryMonth(utilities);
            int expiryYear = GetExpiryYear(utilities);
            DateTime currentDateTime = serverDateTime.GetServerDateTime();
            int inputMonth = billOnDate.Month;
            int inputYear = billOnDate.Year;
            DateTime inputDate = new DateTime(inputYear, inputMonth, 01);
            DateTime cardExpiryDate = new DateTime(expiryYear, expiryMonth, 01);
            if (inputDate >= cardExpiryDate)
            {
                expiredCard = true;
            }
            log.LogMethodExit(expiredCard);
            return expiredCard;
        }
    }

    /// <summary>
    /// Manages the list of CustomerCreditCards
    /// </summary>
    public class CustomerCreditCardsListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<CustomerCreditCardsDTO> customerCreditCardsDTOList;
        private readonly ExecutionContext executionContext;
        private string passPhrase;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public CustomerCreditCardsListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.passPhrase = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CUSTOMER_ENCRYPTION_PASS_PHRASE");
            log.LogMethodExit();
        }
        /// <summary>
        /// CustomerCreditCardsListBL
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="customerCreditCardsDTOList"></param>
        public CustomerCreditCardsListBL(ExecutionContext executionContext, List<CustomerCreditCardsDTO> customerCreditCardsDTOList)
            :this(executionContext)
        {
            log.LogMethodEntry(customerCreditCardsDTOList, executionContext);
            this.customerCreditCardsDTOList = customerCreditCardsDTOList; 
            log.LogMethodExit();
        }

        /// <summary>
        /// GetCustomerCreditCardsDTOList
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="utilities"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public List<CustomerCreditCardsDTO> GetCustomerCreditCardsDTOList(List<KeyValuePair<CustomerCreditCardsDTO.SearchByParameters, string>> searchParameters, Utilities utilities, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            CustomerCreditCardsDataHandler customerCreditCardsDataHandler = new CustomerCreditCardsDataHandler(this.passPhrase, sqlTransaction);
            List<CustomerCreditCardsDTO> returnValue = customerCreditCardsDataHandler.GetCustomerCreditCardsDTOList(searchParameters);
            if (returnValue != null && returnValue.Any())
            {
                foreach (KeyValuePair<CustomerCreditCardsDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (searchParameter.Key == CustomerCreditCardsDTO.SearchByParameters.EXPIREDCARD_LINKED_WITH_UNBILLED_SUBSCRIPTIONS &&
                        (searchParameter.Value == "1" || searchParameter.Value == "Y"))
                    {
                        for (int i = 0; i < returnValue.Count; i++)
                        {
                            CustomerCreditCardsDTO customerCreditCardsDTO = returnValue[i];
                            //List<SubscriptionBillingScheduleDTO> subscriptionBillingScheduleDTOList = GetUnbilledBillingRecords(customerCreditCardsDTO, sqlTransaction);
                            //if (subscriptionBillingScheduleDTOList != null && subscriptionBillingScheduleDTOList.Any())
                            //{
                            DateTime expiryDate = DateTime.Now.Date;
                            CustomerCreditCardsBL customerCreditCardsBL = new CustomerCreditCardsBL(executionContext, customerCreditCardsDTO);
                            if (customerCreditCardsBL.CustomerCreditCardExpiresbefore(utilities, expiryDate) == false)
                            {
                                returnValue.Remove(customerCreditCardsDTO);
                                i = i - 1;
                            }
                            //}
                            //else
                            //{
                            //    //Since not linked with any unbilled record, remove it from the list
                            //    returnValue.Remove(customerCreditCardsDTO);
                            //    i = i - 1;
                            //}
                        }
                    }
                    else if (searchParameter.Key == CustomerCreditCardsDTO.SearchByParameters.EXPIREDCARD_LINKED_WITH_UNBILLED_SUBSCRIPTIONS
                          && (searchParameter.Value == "0" || searchParameter.Value == "N"))
                    {
                        for (int i = 0; i < returnValue.Count; i++)
                        {
                            CustomerCreditCardsDTO customerCreditCardsDTO = returnValue[i];
                            //List<SubscriptionBillingScheduleDTO> subscriptionBillingScheduleDTOList = GetUnbilledBillingRecords(customerCreditCardsDTO, sqlTransaction);
                            //if (subscriptionBillingScheduleDTOList != null && subscriptionBillingScheduleDTOList.Any())
                            //{
                            DateTime expiryDate = DateTime.Now.Date;
                            CustomerCreditCardsBL customerCreditCardsBL = new CustomerCreditCardsBL(executionContext, customerCreditCardsDTO);
                            if (customerCreditCardsBL.CustomerCreditCardExpiresbefore(utilities, expiryDate))
                            {
                                returnValue.Remove(customerCreditCardsDTO);
                                i = i - 1;
                            }
                            //}
                        }
                    }
                    else if (searchParameter.Key == CustomerCreditCardsDTO.SearchByParameters.CARDS_EXPIRING_BEFORE_NEXT_UNBILLED_CYCLE
                             && (searchParameter.Value == "1" || searchParameter.Value == "Y"))
                    {
                        for (int i = 0; i < returnValue.Count; i++)
                        {
                            CustomerCreditCardsDTO customerCreditCardsDTO = returnValue[i];
                            List<SubscriptionBillingScheduleDTO> subscriptionBillingScheduleDTOList = GetUnbilledBillingRecords(customerCreditCardsDTO, sqlTransaction);
                            if (subscriptionBillingScheduleDTOList != null && subscriptionBillingScheduleDTOList.Any())
                            {
                                DateTime expiryDate = subscriptionBillingScheduleDTOList.Min(sbs => sbs.BillOnDate);
                                CustomerCreditCardsBL customerCreditCardsBL = new CustomerCreditCardsBL(executionContext, customerCreditCardsDTO);
                                if (customerCreditCardsBL.CustomerCreditCardExpiresbefore(utilities, expiryDate) == false)
                                {
                                    returnValue.Remove(customerCreditCardsDTO);
                                    i = i - 1;
                                }
                            }
                            else
                            {
                                //Since not linked with any unbilled record, remove it from the list
                                returnValue.Remove(customerCreditCardsDTO);
                                i = i - 1;
                            }
                        }
                    }
                    else if (searchParameter.Key == CustomerCreditCardsDTO.SearchByParameters.CARDS_EXPIRING_BEFORE_NEXT_UNBILLED_CYCLE
                          && (searchParameter.Value == "0" || searchParameter.Value == "N"))
                    {
                        for (int i = 0; i < returnValue.Count; i++)
                        {
                            CustomerCreditCardsDTO customerCreditCardsDTO = returnValue[i];
                            List<SubscriptionBillingScheduleDTO> subscriptionBillingScheduleDTOList = GetUnbilledBillingRecords(customerCreditCardsDTO, sqlTransaction);
                            if (subscriptionBillingScheduleDTOList != null && subscriptionBillingScheduleDTOList.Any())
                            {
                                DateTime expiryDate = subscriptionBillingScheduleDTOList.Min(sbs => sbs.BillOnDate);
                                CustomerCreditCardsBL customerCreditCardsBL = new CustomerCreditCardsBL(executionContext, customerCreditCardsDTO);
                                if (customerCreditCardsBL.CustomerCreditCardExpiresbefore(utilities, expiryDate))
                                {
                                    returnValue.Remove(customerCreditCardsDTO);
                                    i = i - 1;
                                }
                            }
                        }
                    }
                    else if (searchParameter.Key == CustomerCreditCardsDTO.SearchByParameters.CARDS_EXPIRING_IN_X_DAYS)
                    {
                        int xDays = Convert.ToInt32(string.IsNullOrWhiteSpace(searchParameter.Value) ? "1" : searchParameter.Value);
                        DateTime expiryDate = DateTime.Now.Date.AddDays(xDays);
                        for (int i = 0; i < returnValue.Count; i++)
                        {
                            CustomerCreditCardsDTO customerCreditCardsDTO = returnValue[i];
                            CustomerCreditCardsBL customerCreditCardsBL = new CustomerCreditCardsBL(executionContext, customerCreditCardsDTO);
                            if (customerCreditCardsBL.CustomerCreditCardExpiresbefore(utilities, expiryDate) == false)
                            {
                                returnValue.Remove(customerCreditCardsDTO);
                                i = i - 1;
                            }
                        }
                    }
                }
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        private List<SubscriptionBillingScheduleDTO> GetUnbilledBillingRecords(CustomerCreditCardsDTO customerCreditCardsDTO, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(customerCreditCardsDTO, sqlTransaction);
            SubscriptionBillingScheduleListBL subscriptionBillingScheduleListBL = new SubscriptionBillingScheduleListBL(executionContext);
            List<KeyValuePair<SubscriptionBillingScheduleDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<SubscriptionBillingScheduleDTO.SearchByParameters, string>>();
            searchParams.Add(new KeyValuePair<SubscriptionBillingScheduleDTO.SearchByParameters, string>(SubscriptionBillingScheduleDTO.SearchByParameters.STATUS, SubscriptionStatus.ACTIVE));
            searchParams.Add(new KeyValuePair<SubscriptionBillingScheduleDTO.SearchByParameters, string>(SubscriptionBillingScheduleDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            searchParams.Add(new KeyValuePair<SubscriptionBillingScheduleDTO.SearchByParameters, string>(SubscriptionBillingScheduleDTO.SearchByParameters.CUSTOMER_CREDIT_CARDS_ID, customerCreditCardsDTO.CustomerCreditCardsId.ToString()));
            searchParams.Add(new KeyValuePair<SubscriptionBillingScheduleDTO.SearchByParameters, string>(SubscriptionBillingScheduleDTO.SearchByParameters.UNBILLED_CYCLE, "1"));
            List<SubscriptionBillingScheduleDTO> subscriptionBillingScheduleDTOList = subscriptionBillingScheduleListBL.GetSubscriptionBillingScheduleDTOList(searchParams, sqlTransaction);
            log.LogMethodExit();
            return subscriptionBillingScheduleDTOList;
        }

        /// <summary>
        /// GetCustomerCreditCardsDTOList
        /// </summary>
        /// <param name="customerCreditCardsIdList"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public List<CustomerCreditCardsDTO> GetCustomerCreditCardsDTOList(List<int> customerCreditCardsIdList, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(customerCreditCardsIdList);
            CustomerCreditCardsDataHandler customerCreditCardsDataHandler = new CustomerCreditCardsDataHandler(this.passPhrase, sqlTransaction);
            List<CustomerCreditCardsDTO> returnValue = customerCreditCardsDataHandler.GetCustomerCreditCardsDTOList(customerCreditCardsIdList);
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// Save
        /// </summary>
        /// <param name="sqlTrx"></param>
        public void Save(SqlTransaction sqlTrx = null)
        {
            log.LogMethodEntry();
            if (customerCreditCardsDTOList == null ||
                customerCreditCardsDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                throw new Exception(MessageContainerList.GetMessage(executionContext, "Cant save empty list"));
            }
            for (int i = 0; i < customerCreditCardsDTOList.Count; i++)
            {
                CustomerCreditCardsDTO customerCreditCardsDTO = customerCreditCardsDTOList[i];
                CustomerCreditCardsBL customerCreditCardsBL = new CustomerCreditCardsBL(executionContext, customerCreditCardsDTO);
                customerCreditCardsBL.Save(sqlTrx);
            }
            log.LogMethodExit();
        }
    }
}
