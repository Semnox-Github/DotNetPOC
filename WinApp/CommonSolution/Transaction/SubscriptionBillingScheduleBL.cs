/********************************************************************************************
 * Project Name - SubscriptionBillingSchedule BL
 * Description  -BL class of the SubscriptionBillingSchedule 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************   
 *2.110.0     09-Dec-2020    Guru S A           Created for Subscription changes                                                                               
 *2.120.0     18-Mar-2021    Guru S A           For Subscription phase 2 changes
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// SubscriptionBillingScheduleBL
    /// </summary>
    public class SubscriptionBillingScheduleBL
    {
        private SubscriptionBillingScheduleDTO subscriptionBillingScheduleDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private const string MANAGER_APPROVAL_TO_OVERRIDE_SUBSCRIPTION_PRICE = "MANAGER_APPROVAL_TO_OVERRIDE_SUBSCRIPTION_PRICE";
        /// <summary>
        /// Default constructor of SubscriptionBillingScheduleBL class
        /// </summary>
        private SubscriptionBillingScheduleBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            subscriptionBillingScheduleDTO = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// SubscriptionBillingScheduleBL
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="subscriptionBillingScheduleDTO"></param>
        public SubscriptionBillingScheduleBL(ExecutionContext executionContext, SubscriptionBillingScheduleDTO subscriptionBillingScheduleDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(subscriptionBillingScheduleDTO);
            this.subscriptionBillingScheduleDTO = subscriptionBillingScheduleDTO;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with the SubscriptionBillingSchedule id as the parameter
        /// Would fetch the SubscriptionBillingSchedule object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="id"></param>
        /// <param name="sqlTransaction"></param>
        public SubscriptionBillingScheduleBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(id, sqlTransaction);
            SubscriptionBillingScheduleDataHandler subscriptionBillingScheduleDataHandler = new SubscriptionBillingScheduleDataHandler(sqlTransaction);
            subscriptionBillingScheduleDTO = subscriptionBillingScheduleDataHandler.GetSubscriptionBillingScheduleDTO(id);
            if (subscriptionBillingScheduleDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "SubscriptionBillingSchedule", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit(subscriptionBillingScheduleDTO);
        }
        /// <summary>
        /// Saves the SubscriptionBillingSchedule
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            SubscriptionBillingScheduleDataHandler subscriptionBillingScheduleDataHandler = new SubscriptionBillingScheduleDataHandler(sqlTransaction);

            if (subscriptionBillingScheduleDTO.IsChanged == false && subscriptionBillingScheduleDTO.SubscriptionBillingScheduleId > -1)
            {
                log.LogMethodExit(null, "SubscriptionBillingScheduleDTO is not changed.");
                return;
            }
            ValidateSubscriptionBillingSchedule(sqlTransaction);
            if (subscriptionBillingScheduleDTO.SubscriptionBillingScheduleId < 0)
            {
                subscriptionBillingScheduleDTO = subscriptionBillingScheduleDataHandler.InsertSubscriptionBillingSchedule(subscriptionBillingScheduleDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                subscriptionBillingScheduleDTO.AcceptChanges();
            }
            else
            {
                if (subscriptionBillingScheduleDTO.IsChanged)
                {
                    subscriptionBillingScheduleDTO = subscriptionBillingScheduleDataHandler.UpdateSubscriptionBillingSchedule(subscriptionBillingScheduleDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    subscriptionBillingScheduleDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the SubscriptionBillingScheduleDTO. 
        /// </summary>
        public void ValidateSubscriptionBillingSchedule(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (subscriptionBillingScheduleDTO == null)
            {
                throw new ArgumentNullException(MessageContainerList.GetMessage(executionContext, "Subscription Billing Schedule DTO") + " " + MessageContainerList.GetMessage(executionContext, "is null"));
            }
            if (subscriptionBillingScheduleDTO.BillFromDate == DateTime.MinValue)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Bill From Date") + " " + MessageContainerList.GetMessage(executionContext, "is null"));
            }
            if (subscriptionBillingScheduleDTO.BillToDate == DateTime.MinValue)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Bill To Date") + " " + MessageContainerList.GetMessage(executionContext, "is null"));
            }
            if (subscriptionBillingScheduleDTO.BillOnDate == DateTime.MinValue)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Bill On Date") + " " + MessageContainerList.GetMessage(executionContext, "is null"));
            }
            if (string.IsNullOrWhiteSpace(subscriptionBillingScheduleDTO.Status))
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Subscription Bill Cycle Status") + " " + MessageContainerList.GetMessage(executionContext, "is null"));
            }
            if (string.IsNullOrWhiteSpace(subscriptionBillingScheduleDTO.LineType))
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Line Type") + " " + MessageContainerList.GetMessage(executionContext, "is null"));
            }
            else
            {
                if (SubscriptionLineType.ValidSubscriptionLineType(subscriptionBillingScheduleDTO.LineType) == false)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Line Type")));
                    //Please enter valid value for &1
                }
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Mark Cycle As Billed
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="transactionLineId"></param>
        /// <param name="sqlTrx"></param>
        public void MarkCycleAsBilled(int transactionId, int transactionLineId, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(transactionId, transactionLineId, sqlTrx);
            if (subscriptionBillingScheduleDTO != null)
            {
                if (subscriptionBillingScheduleDTO.SubscriptionBillingScheduleId > -1 && subscriptionBillingScheduleDTO.IsActive 
                    && subscriptionBillingScheduleDTO.Status == SubscriptionStatus.ACTIVE && subscriptionBillingScheduleDTO.TransactionId == -1)
                {
                    subscriptionBillingScheduleDTO.TransactionId = transactionId;
                    subscriptionBillingScheduleDTO.TransactionLineId = transactionLineId;
                    subscriptionBillingScheduleDTO.PaymentProcessingFailureCount = 0;
                    AccountListBL accountListBL = new AccountListBL(executionContext);
                    List<KeyValuePair<AccountDTO.SearchByParameters, string>> searchParmAcnt = new List<KeyValuePair<AccountDTO.SearchByParameters, string>>();
                    searchParmAcnt.Add(new KeyValuePair<AccountDTO.SearchByParameters, string>(AccountDTO.SearchByParameters.SUBSCRIPTION_BILLING_SCHEDULE_ID, subscriptionBillingScheduleDTO.SubscriptionBillingScheduleId.ToString()));
                    searchParmAcnt.Add(new KeyValuePair<AccountDTO.SearchByParameters, string>(AccountDTO.SearchByParameters.VALID_FLAG, "Y"));
                    searchParmAcnt.Add(new KeyValuePair<AccountDTO.SearchByParameters, string>(AccountDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString())); 
                    List<AccountDTO> accountDTOList = accountListBL.GetAccountDTOList(searchParmAcnt,true,true,sqlTrx);
                    if (accountDTOList != null && accountDTOList.Any())
                    {
                        for (int i = 0; i < accountDTOList.Count; i++)
                        {
                            AccountBL accountBL = new AccountBL(executionContext, accountDTOList[i]);
                            accountBL.ActivateSubscriptionEntitlements(subscriptionBillingScheduleDTO.SubscriptionBillingScheduleId, transactionId, transactionLineId, sqlTrx);
                            accountBL.Save(sqlTrx);
                        }
                    }
                    Save(sqlTrx);
                }
            }
            else
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Subscription Bill Cycle DTO") + " " + MessageContainerList.GetMessage(executionContext, "is null"));
            }
        }
        /// <summary>
        /// OverrideBillCyclePrice
        /// </summary>
        /// <param name="sqlTrx"></param>
        public void OverrideBillCyclePrice( SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(sqlTrx);
            if (subscriptionBillingScheduleDTO != null)
            {
                if (subscriptionBillingScheduleDTO.TransactionId > -1)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2974));
                    //"Subscription billing schedule is already billed, cannot override bill amount"
                }
                if (subscriptionBillingScheduleDTO.Status == SubscriptionStatus.CANCELLED || subscriptionBillingScheduleDTO.IsActive == false)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2975));
                    // "Subscription billing schedule is in Cancelled/Inactive status, cannot override bill amount"
                }
                if (subscriptionBillingScheduleDTO.OverridedBillAmount == null)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2976));
                    // "Please provide new override bill amount"
                }
                else if (subscriptionBillingScheduleDTO.OverridedBillAmount < 0)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Override Bill Amount")));
                    //Please enter valid value for &1
                }
                if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, MANAGER_APPROVAL_TO_OVERRIDE_SUBSCRIPTION_PRICE)
                     && string.IsNullOrWhiteSpace(subscriptionBillingScheduleDTO.OverrideApprovedBy))
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 268));
                    //Manager Approval Required for this Task
                }
                if (subscriptionBillingScheduleDTO.SubscriptionBillingScheduleId > -1 && subscriptionBillingScheduleDTO.TransactionId == -1)
                {
                    subscriptionBillingScheduleDTO.OverrideBy = executionContext.GetUserId();
                    Save(sqlTrx);
                }
            }
            else
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Subscription Bill Cycle DTO") + " " + MessageContainerList.GetMessage(executionContext, "is null"));
            }
        }
        /// <summary>
        /// CancelSubscriptionLine
        /// </summary>
        public void CancelSubscriptionLine(SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(sqlTrx);
            ParafaitDBTransaction dBTrx = null;
            try
            {
                if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "CANCEL_SUBSCRIPTION_REQUIRES_MANAGER_APPROVAL")
                       && string.IsNullOrWhiteSpace(subscriptionBillingScheduleDTO.CancellationApprovedBy))
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 268));
                    //Manager Approval Required for this Task
                }
                if (subscriptionBillingScheduleDTO.IsActive == false 
                    || subscriptionBillingScheduleDTO.Status == SubscriptionStatus.CANCELLED
                    || subscriptionBillingScheduleDTO.Status == SubscriptionStatus.EXPIRED)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2975));
                    //'Subscription billing schedule is in Cancelled/Inactive status, cannot perform the action' 
                }
                if (subscriptionBillingScheduleDTO.IsActive && subscriptionBillingScheduleDTO.Status == SubscriptionStatus.PAUSED)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2966));
                    //'Invalid operation, Subscription is already in Paused status 
                }
                if (sqlTrx == null)
                {
                    dBTrx = new ParafaitDBTransaction();
                    dBTrx.BeginTransaction();
                    sqlTrx = dBTrx.SQLTrx;
                }
                subscriptionBillingScheduleDTO.PaymentProcessingFailureCount = 0;
                subscriptionBillingScheduleDTO.IsActive = false;
                subscriptionBillingScheduleDTO.CancelledBy = executionContext.GetUserId();
                subscriptionBillingScheduleDTO.Status = SubscriptionStatus.CANCELLED;
                Save(sqlTrx);
                CancelSubscriptionBillingCycleEntitlements(sqlTrx);
                if (dBTrx != null)
                {
                    dBTrx.EndTransaction();
                    dBTrx.Dispose();
                    sqlTrx = null;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                if (dBTrx != null)
                {
                    dBTrx.RollBack();
                    dBTrx.Dispose();
                    sqlTrx = null;
                }
                throw;
            }
            log.LogMethodExit();
        }
        private void CancelSubscriptionBillingCycleEntitlements(SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(sqlTrx);
            if (subscriptionBillingScheduleDTO != null)
            {
                //Get linked accounts
                AccountListBL accountListBL = new AccountListBL(executionContext);
                List<KeyValuePair<AccountDTO.SearchByParameters, string>> searchParam = new List<KeyValuePair<AccountDTO.SearchByParameters, string>>();
                searchParam.Add(new KeyValuePair<AccountDTO.SearchByParameters, string>(AccountDTO.SearchByParameters.SUBSCRIPTION_HEADER_ID, subscriptionBillingScheduleDTO.SubscriptionHeaderId.ToString()));
                searchParam.Add(new KeyValuePair<AccountDTO.SearchByParameters, string>(AccountDTO.SearchByParameters.SUBSCRIPTION_BILLING_SCHEDULE_ID, subscriptionBillingScheduleDTO.SubscriptionBillingScheduleId.ToString()));
                searchParam.Add(new KeyValuePair<AccountDTO.SearchByParameters, string>(AccountDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                List<AccountDTO> accountDTOList = accountListBL.GetAccountDTOList(searchParam, true, true, sqlTrx);
                if (accountDTOList != null && accountDTOList.Any())
                {
                    List<int> subscriptionBillingCycleIdList = new List<int>();
                    subscriptionBillingCycleIdList.Add(subscriptionBillingScheduleDTO.SubscriptionBillingScheduleId);
                    for (int i = 0; i < accountDTOList.Count; i++)
                    {
                        AccountDTO accountDTO = accountDTOList[i];
                        AccountBL accountBL = new AccountBL(executionContext, accountDTO);
                        accountBL.CancelSubscriptionBillingCycleEntitlements(subscriptionBillingCycleIdList, sqlTrx);
                    }
                }
            }
            log.LogMethodExit();
        }
    }
    /// <summary>
    /// SubscriptionBillingScheduleListBL
    /// </summary>
    public class SubscriptionBillingScheduleListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<SubscriptionBillingScheduleDTO> subscriptionBillingScheduleList;
        private ExecutionContext executionContext;

        /// <summary>
        /// Parameterized Constructor with ExecutionContext
        /// </summary>
        /// <param name="executionContext"></param>
        public SubscriptionBillingScheduleListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.subscriptionBillingScheduleList = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="subscriptionBillingScheduleList"></param>
        public SubscriptionBillingScheduleListBL(ExecutionContext executionContext, List<SubscriptionBillingScheduleDTO> subscriptionBillingScheduleList)
        {
            log.LogMethodEntry(executionContext, subscriptionBillingScheduleList);
            this.executionContext = executionContext;
            this.subscriptionBillingScheduleList = subscriptionBillingScheduleList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the SubscriptionBillingSchedule list
        /// </summary>
        public List<SubscriptionBillingScheduleDTO> GetSubscriptionBillingScheduleDTOList(List<KeyValuePair<SubscriptionBillingScheduleDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)//modified
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            SubscriptionBillingScheduleDataHandler subscriptionBillingScheduleDataHandler = new SubscriptionBillingScheduleDataHandler(sqlTransaction);
            List<SubscriptionBillingScheduleDTO> subscriptionBillingScheduleList = subscriptionBillingScheduleDataHandler.GetSubscriptionBillingScheduleDTOList(searchParameters);
            log.LogMethodExit(subscriptionBillingScheduleList);
            return subscriptionBillingScheduleList;
        }

        /// <summary>
        /// Returns the SubscriptionBillingSchedule list
        /// </summary>
        /// <param name="subscriptionHeaderIdList">integer list parameter</param>
        /// <returns>Returns List of ProductBarcodeSetDTO</returns>
        public List<SubscriptionBillingScheduleDTO> GetSubscriptionBillingScheduleDTOList(List<int> subscriptionHeaderIdList, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(subscriptionHeaderIdList, sqlTransaction);
            SubscriptionBillingScheduleDataHandler subscriptionBillingScheduleDataHandler = new SubscriptionBillingScheduleDataHandler(sqlTransaction);
            List<SubscriptionBillingScheduleDTO> subscriptionBillingScheduleList = subscriptionBillingScheduleDataHandler.GetSubscriptionBillingScheduleDTOList(subscriptionHeaderIdList);

            log.LogMethodExit(subscriptionBillingScheduleList);
            return subscriptionBillingScheduleList;
        }
        /// <summary>
        /// Returns the SubscriptionBillingSchedule list for the ID
        /// </summary>
        /// <param name="subscriptionBillingScheduleIdList">integer list parameter</param>
        /// <returns>Returns List of ProductBarcodeSetDTO</returns>
        public List<SubscriptionBillingScheduleDTO> GetSubscriptionBillingScheduleDTOListById(List<int> subscriptionBillingScheduleIdList, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(subscriptionBillingScheduleIdList, sqlTransaction);
            SubscriptionBillingScheduleDataHandler subscriptionBillingScheduleDataHandler = new SubscriptionBillingScheduleDataHandler(sqlTransaction);
            List<SubscriptionBillingScheduleDTO> subscriptionBillingScheduleList = subscriptionBillingScheduleDataHandler.GetSubscriptionBillingScheduleDTOListById(subscriptionBillingScheduleIdList);

            log.LogMethodExit(subscriptionBillingScheduleList);
            return subscriptionBillingScheduleList;
        }
        /// <summary>
        /// Validates and saves the subscriptionBillingScheduleList to the db
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (subscriptionBillingScheduleList == null ||
                subscriptionBillingScheduleList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                throw new Exception(MessageContainerList.GetMessage(executionContext, "Cant save empty list"));
            }
            for (int i = 0; i < subscriptionBillingScheduleList.Count; i++)
            {
                SubscriptionBillingScheduleDTO subscriptionBillingScheduleDTO = subscriptionBillingScheduleList[i];
                SubscriptionBillingScheduleBL subscriptionBillingScheduleBL = new SubscriptionBillingScheduleBL(executionContext, subscriptionBillingScheduleDTO);
                subscriptionBillingScheduleBL.Save(sqlTransaction);
            }
            log.LogMethodExit();
        } 
    }
}
