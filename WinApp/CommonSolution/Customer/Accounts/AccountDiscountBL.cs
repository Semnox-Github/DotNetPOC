/********************************************************************************************
 * Project Name - AccountDiscountBL
 * Description  - BL AccountDiscountBL
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.80.0      21-May-2020      Girish Kundar  Modified : Made default constructor as Private   
 *2.110.0     10-Dec-2020      Guru S A       For Subscription changes
 *2.120.0     18-Mar-2021      Guru S A       For Subscription phase 2 changes
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.DBSynch;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Customer.Accounts
{
    /// <summary>
    /// Business logic for AccountDiscount class.
    /// </summary>
    public class AccountDiscountBL
    {
        AccountDiscountDTO accountDiscountDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of AccountDiscountBL class
        /// </summary>
        private AccountDiscountBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the accountDiscount id as the parameter
        /// Would fetch the accountDiscount object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="id">Id</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        public AccountDiscountBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            AccountDiscountDataHandler accountDiscountDataHandler = new AccountDiscountDataHandler(sqlTransaction);
            accountDiscountDTO = accountDiscountDataHandler.GetAccountDiscountDTO(id);
            if (accountDiscountDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "AccountDiscount", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates AccountDiscountBL object using the AccountDiscountDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="accountDiscountDTO">AccountDiscountDTO object</param>
        public AccountDiscountBL(ExecutionContext executionContext, AccountDiscountDTO accountDiscountDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, accountDiscountDTO);
            this.accountDiscountDTO = accountDiscountDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the AccountDiscount
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        internal void Save(int parentSiteId, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(parentSiteId, sqlTransaction);
            AccountDiscountDataHandler accountDiscountDataHandler = new AccountDiscountDataHandler(sqlTransaction);
            if (accountDiscountDTO.IsChanged)
            {
                if (accountDiscountDTO.AccountDiscountId < 0)
                {
                    accountDiscountDTO = accountDiscountDataHandler.InsertAccountDiscount(accountDiscountDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    accountDiscountDTO.AcceptChanges();
                }
                else
                {
                    if (accountDiscountDTO.IsChanged)
                    {
                        accountDiscountDTO = accountDiscountDataHandler.UpdateAccountDiscount(accountDiscountDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                        accountDiscountDTO.AcceptChanges();
                    }
                }
                CreateRoamingData(parentSiteId, sqlTransaction);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public AccountDiscountDTO AccountDiscountDTO
        {
            get
            {
                return accountDiscountDTO;
            }
        }

        /// <summary>
        /// Validates the customer relationship DTO
        /// </summary>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            if (accountDiscountDTO.IsActive)
            {
                if (accountDiscountDTO.DiscountId == -1)
                {
                    validationErrorList.Add(new ValidationError("AccountDiscount", "DiscountId", MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Discount Name"))));
                }
                if (accountDiscountDTO.AccountDiscountId < 0)
                {
                    List<KeyValuePair<AccountDiscountDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<AccountDiscountDTO.SearchByParameters, string>>();
                    searchParameters.Add(new KeyValuePair<AccountDiscountDTO.SearchByParameters, string>(AccountDiscountDTO.SearchByParameters.DISCOUNT_ID, accountDiscountDTO.DiscountId.ToString()));
                    searchParameters.Add(new KeyValuePair<AccountDiscountDTO.SearchByParameters, string>(AccountDiscountDTO.SearchByParameters.ACCOUNT_ID, accountDiscountDTO.AccountId.ToString()));
                    searchParameters.Add(new KeyValuePair<AccountDiscountDTO.SearchByParameters, string>(AccountDiscountDTO.SearchByParameters.EXPIRY_DATE_GREATER_THAN, ServerDateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                    searchParameters.Add(new KeyValuePair<AccountDiscountDTO.SearchByParameters, string>(AccountDiscountDTO.SearchByParameters.IS_ACTIVE, "1"));
                    AccountDiscountListBL accountDiscountListBL = new AccountDiscountListBL(executionContext);
                    List<AccountDiscountDTO> accountDiscountDTOs = accountDiscountListBL.GetAccountDiscountDTOList(searchParameters, sqlTransaction);
                    if (accountDiscountDTOs != null && accountDiscountDTOs.Any())
                    {
                        validationErrorList.Add(new ValidationError("AccountDiscount", "DiscountId", MessageContainerList.GetMessage(executionContext, 2608, MessageContainerList.GetMessage(executionContext, "Discount"))));
                    }
                }
                if (accountDiscountDTO.AccountDiscountId > -1)
                {
                    List<KeyValuePair<AccountDiscountDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<AccountDiscountDTO.SearchByParameters, string>>();
                    searchParameters.Add(new KeyValuePair<AccountDiscountDTO.SearchByParameters, string>(AccountDiscountDTO.SearchByParameters.DISCOUNT_ID, accountDiscountDTO.DiscountId.ToString()));
                    searchParameters.Add(new KeyValuePair<AccountDiscountDTO.SearchByParameters, string>(AccountDiscountDTO.SearchByParameters.ACCOUNT_ID, accountDiscountDTO.AccountId.ToString()));
                    searchParameters.Add(new KeyValuePair<AccountDiscountDTO.SearchByParameters, string>(AccountDiscountDTO.SearchByParameters.EXPIRY_DATE_GREATER_THAN, ServerDateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                    searchParameters.Add(new KeyValuePair<AccountDiscountDTO.SearchByParameters, string>(AccountDiscountDTO.SearchByParameters.IS_ACTIVE, "1"));
                    AccountDiscountListBL accountDiscountListBL = new AccountDiscountListBL(executionContext);
                    List<AccountDiscountDTO> accountDiscountDTOs = accountDiscountListBL.GetAccountDiscountDTOList(searchParameters, sqlTransaction);
                    if (accountDiscountDTOs != null && accountDiscountDTOs.Any() && accountDiscountDTOs.Count > 1)
                    {
                        validationErrorList.Add(new ValidationError("AccountDiscount", "DiscountId", MessageContainerList.GetMessage(executionContext, 2608, MessageContainerList.GetMessage(executionContext, "Discount"))));
                    }
                    else if (accountDiscountDTOs != null && accountDiscountDTOs.Count == 1 && accountDiscountDTOs.Exists(x => x.AccountDiscountId != accountDiscountDTO.AccountDiscountId))
                    {
                        validationErrorList.Add(new ValidationError("AccountDiscount", "DiscountId", MessageContainerList.GetMessage(executionContext, 2608, MessageContainerList.GetMessage(executionContext, "Discount"))));
                    }
                }
            }

            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }
        internal void ActivateSubscriptionEntitlements(int transactionId, int transactionLineId, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry();
            if (accountDiscountDTO != null && accountDiscountDTO.IsActive)
            {
                if (accountDiscountDTO.TransactionId == -1)
                { //update only if it is not already set
                    accountDiscountDTO.TransactionId = transactionId;
                }
                accountDiscountDTO.ValidityStatus = AccountDTO.AccountValidityStatus.Valid;
            }
            log.LogMethodExit();
        }

        internal void CancelSubscriptionBillingCycleEntitlements()
        {
            log.LogMethodEntry();
            if (accountDiscountDTO != null && accountDiscountDTO.IsActive)
            {
                LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                accountDiscountDTO.IsActive = false;
                accountDiscountDTO.ExpiryDate = lookupValuesList.GetServerDateTime();
            }
            log.LogMethodExit();
        }
        internal void PauseSubscriptionBillingCycleEntitlements()
        {
            log.LogMethodEntry();
            if (accountDiscountDTO != null && accountDiscountDTO.IsActive)
            {
                accountDiscountDTO.ValidityStatus = AccountDTO.AccountValidityStatus.Hold;
            }
            log.LogMethodExit();
        }

        internal void PostponeSubscriptionBillingCycleEntitlements(SubscriptionUnPauseDetailsDTO subscriptionUnPauseDetailsDTO)
        {
            log.LogMethodEntry(subscriptionUnPauseDetailsDTO);
            if (accountDiscountDTO != null && accountDiscountDTO.IsActive)
            {
                accountDiscountDTO.ValidityStatus = (accountDiscountDTO.TransactionId > -1) ? AccountDTO.AccountValidityStatus.Valid : AccountDTO.AccountValidityStatus.Hold;
                if (subscriptionUnPauseDetailsDTO.OldBillFromDate == null)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2907, "Old Bill From Date"));
                }
                if (subscriptionUnPauseDetailsDTO.NewBillFromDate == null)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2907, "New Bill From Date"));
                }
                if (accountDiscountDTO.ExpiryDate != null)
                {
                    TimeSpan expireDateDiff = (DateTime)accountDiscountDTO.ExpiryDate - (DateTime)subscriptionUnPauseDetailsDTO.OldBillFromDate;
                    accountDiscountDTO.ExpiryDate = ((DateTime)subscriptionUnPauseDetailsDTO.NewBillFromDate).Add(expireDateDiff);
                }
            }
            log.LogMethodExit();
        }
        internal void ResumeSubscriptionBillingCycleEntitlements(SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(sqlTrx);
            if (accountDiscountDTO != null && accountDiscountDTO.IsActive && accountDiscountDTO.ValidityStatus == AccountDTO.AccountValidityStatus.Hold
               && accountDiscountDTO.SubscriptionBillingScheduleId > -1)
            {
                log.Info("accountDiscountDTO.TransactionId: " + accountDiscountDTO.TransactionId.ToString());
                if (accountDiscountDTO.TransactionId > -1)
                {
                    AccountDiscountListBL accountDiscountListBL = new AccountDiscountListBL(executionContext);
                    List<KeyValuePair<AccountDiscountDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<AccountDiscountDTO.SearchByParameters, string>>();
                    searchParameters.Add(new KeyValuePair<AccountDiscountDTO.SearchByParameters, string>(AccountDiscountDTO.SearchByParameters.SUBSCRIPTION_BILLING_SCHEDULE_ID, accountDiscountDTO.SubscriptionBillingScheduleId.ToString()));
                    searchParameters.Add(new KeyValuePair<AccountDiscountDTO.SearchByParameters, string>(AccountDiscountDTO.SearchByParameters.SUBSCRIPTION_BILL_SCHEDULE_IS_BILLED, "1"));
                    List<AccountDiscountDTO> billedSubscriptionCDDTOList = accountDiscountListBL.GetAccountDiscountDTOList(searchParameters, sqlTrx);
                    if (billedSubscriptionCDDTOList != null && billedSubscriptionCDDTOList.Any())
                    {
                        accountDiscountDTO.ValidityStatus = AccountDTO.AccountValidityStatus.Valid;
                    }
                    else
                    {
                        log.Info("Unbilled entitlement, leave it on hold");
                    }
                }
            }
            log.LogMethodExit();
        }

        private void CreateRoamingData(int parentSiteId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parentSiteId, sqlTransaction);
            if (parentSiteId > -1 && parentSiteId != accountDiscountDTO.SiteId && executionContext.GetSiteId() > -1
                    && accountDiscountDTO.AccountDiscountId > -1)
            {
                DBSynchLogDTO dBSynchLogDTO = new DBSynchLogDTO("I", accountDiscountDTO.Guid, "CardDiscounts", DateTime.Now, parentSiteId);
                DBSynchLogBL dBSynchLogBL = new DBSynchLogBL(executionContext, dBSynchLogDTO);
                dBSynchLogBL.Save(sqlTransaction);
            }
            log.LogMethodExit();
        }
    }

    /// <summary>
    /// Manages the list of AccountDiscount
    /// </summary>
    public class AccountDiscountListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public AccountDiscountListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the AccountDiscount list
        /// </summary>
        public List<AccountDiscountDTO> GetAccountDiscountDTOList(List<KeyValuePair<AccountDiscountDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            AccountDiscountDataHandler accountDiscountDataHandler = new AccountDiscountDataHandler(sqlTransaction);
            List<AccountDiscountDTO> returnValue = accountDiscountDataHandler.GetAccountDiscountDTOList(searchParameters);
            log.LogMethodExit(returnValue);
            return returnValue;
        }
        /// <summary>
        /// Returns the AccountDiscount list
        /// </summary>
        public List<AccountDiscountDTO> GetAccountDiscountDTOListByAccountIds(List<int> accountIdList, bool activeChildRecords, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(accountIdList, activeChildRecords, sqlTransaction);
            AccountDiscountDataHandler accountDiscountDataHandler = new AccountDiscountDataHandler(sqlTransaction);
            List<AccountDiscountDTO> returnValue = accountDiscountDataHandler.GetAccountDiscountDTOListByAccountIdList(accountIdList, activeChildRecords);
            log.LogMethodExit(returnValue);
            return returnValue;
        }
    }
}
