/********************************************************************************************
 * Project Name - SubscriptionHeader BL
 * Description  -BL class of the SubscriptionHeader 
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
using Semnox.Parafait.Customer;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Product;
using Semnox.Parafait.Site;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// SubscriptionHeaderBL
    /// </summary>
    public class SubscriptionHeaderBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SubscriptionHeaderDTO subscriptionHeaderDTO;
        private readonly ExecutionContext executionContext;
        private LookupValuesList serverDateTime;
        private const string SUBSCRIPTION_FEATURE_SETUP = "SUBSCRIPTION_FEATURE_SETUP";
        private const string NUMBEROFMESSAGESFORPAYMENTRETRYLIMIT = "NumberOfMessagesForPaymentRetryLimit";
        private const string FREQUENCYINDAYSFORPAYMENTRETRYLIMITMESSAGE = "FrequencyInDaysForPaymentRetryLimitMessage";
        private const string SUBSCRIPTIONORDER = "SubscriptionOrder";

        private int numberOfMessagesForPaymentRetryLimit = 3;
        private int frequencyInDaysForPaymentRetryLimitMessage = 5;
        private const string MANAGER_APPROVAL_FOR_PAUSE_UNPAUSE_SUBSCRIPTION = "MANAGER_APPROVAL_FOR_PAUSE_UNPAUSE_SUBSCRIPTION";
        /// <summary>
        /// Default constructor of SubscriptionHeaderBL class
        /// </summary>
        private SubscriptionHeaderBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            subscriptionHeaderDTO = null;
            this.executionContext = executionContext;
            this.serverDateTime = new LookupValuesList(executionContext);
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates SubscriptionHeaderBL object using the SubscriptionHeaderDTO
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="subscriptionHeaderDTO"></param>
        public SubscriptionHeaderBL(ExecutionContext executionContext, SubscriptionHeaderDTO subscriptionHeaderDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(subscriptionHeaderDTO);
            this.subscriptionHeaderDTO = subscriptionHeaderDTO;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with the SubscriptionHeader id as the parameter
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="id"></param>
        /// <param name="loadChildren"></param>
        /// <param name="sqlTransaction"></param>
        public SubscriptionHeaderBL(ExecutionContext executionContext, int id, bool loadChildren = false, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(id, loadChildren, sqlTransaction);
            LoadSubscriptionHeaderDTO(id, loadChildren, sqlTransaction);
            log.LogMethodExit(subscriptionHeaderDTO);
        }

        private void LoadSubscriptionHeaderDTO(int id, bool loadChildren, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(id, loadChildren, sqlTransaction);
            SubscriptionHeaderDataHandler subscriptionHeaderDataHandler = new SubscriptionHeaderDataHandler(sqlTransaction);
            subscriptionHeaderDTO = subscriptionHeaderDataHandler.GetSubscriptionHeaderDTO(id);
            if (subscriptionHeaderDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "SubscriptionHeader", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            if (loadChildren)
            {
                LoadChildRecords(id, sqlTransaction);
            }
            SetFromSiteTimeOffset();
            log.LogMethodExit();
        }

        private void LoadChildRecords(int id, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(id, sqlTransaction);
            SubscriptionBillingScheduleListBL subscriptionBillingScheduleListBL = new SubscriptionBillingScheduleListBL(executionContext);
            List<KeyValuePair<SubscriptionBillingScheduleDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<SubscriptionBillingScheduleDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<SubscriptionBillingScheduleDTO.SearchByParameters, string>(SubscriptionBillingScheduleDTO.SearchByParameters.SUBSCRIPTION_HEADER_ID, id.ToString()));
            searchParameters.Add(new KeyValuePair<SubscriptionBillingScheduleDTO.SearchByParameters, string>(SubscriptionBillingScheduleDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            List<SubscriptionBillingScheduleDTO> subscriptionBillingScheduleDTOList = subscriptionBillingScheduleListBL.GetSubscriptionBillingScheduleDTOList(searchParameters, sqlTransaction);
            if (subscriptionBillingScheduleDTOList != null && subscriptionBillingScheduleDTOList.Any())
            {
                subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList = subscriptionBillingScheduleDTOList;
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Saves the SubscriptionHeader
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            SubscriptionHeaderDataHandler subscriptionHeaderDataHandler = new SubscriptionHeaderDataHandler(sqlTransaction);

            if (subscriptionHeaderDTO.IsChangedRecursive == false && subscriptionHeaderDTO.SubscriptionHeaderId > -1)
            {
                log.LogMethodExit(null, "SubscriptionHeaderDTO is not changed.");
                return;
            }
            ValidateSubscriptionHeader(sqlTransaction);
            SetToSiteTimeOffset();
            if (subscriptionHeaderDTO.SubscriptionHeaderId < 0)
            {
                subscriptionHeaderDTO.SubscriptionNumber = GetNextSubscriptipnNumber(sqlTransaction);
                subscriptionHeaderDTO = subscriptionHeaderDataHandler.InsertSubscriptionHeader(subscriptionHeaderDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                subscriptionHeaderDTO.AcceptChanges();
            }
            else
            {
                if (subscriptionHeaderDTO.IsChanged)
                {
                    subscriptionHeaderDTO = subscriptionHeaderDataHandler.UpdateSubscriptionHeader(subscriptionHeaderDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    subscriptionHeaderDTO.AcceptChanges();
                }
            }
            if (subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList != null
                && subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList.Any()
                && (subscriptionHeaderDTO.IsChangedRecursive))
            {
                List<SubscriptionBillingScheduleDTO> updateSubscriptionBillingScheduleDTOList = new List<SubscriptionBillingScheduleDTO>();
                for (int i = 0; i < subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList.Count; i++)
                {
                    if (subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList[i].SubscriptionHeaderId != subscriptionHeaderDTO.SubscriptionHeaderId)
                    {
                        subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList[i].SubscriptionHeaderId = subscriptionHeaderDTO.SubscriptionHeaderId;
                    }
                    if (subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList[i].IsChanged || subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList[i].SubscriptionBillingScheduleId < 0)
                    {
                        updateSubscriptionBillingScheduleDTOList.Add(subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList[i]);
                    }
                }
                if (updateSubscriptionBillingScheduleDTOList != null && updateSubscriptionBillingScheduleDTOList.Any())
                {
                    SubscriptionBillingScheduleListBL subscriptionBillingScheduleListBL = new SubscriptionBillingScheduleListBL(executionContext, updateSubscriptionBillingScheduleDTOList);
                    subscriptionBillingScheduleListBL.Save(sqlTransaction);
                }
            }
            SetFromSiteTimeOffset(); //reset back for display purpse
            log.LogMethodExit();
        }        

        /// <summary>
        /// Validates the SubscriptionHeaderDTO. 
        /// </summary>
        public void ValidateSubscriptionHeader(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (subscriptionHeaderDTO == null)
            {
                throw new ArgumentNullException(MessageContainerList.GetMessage(executionContext, "Subscription Header DTO") + " " + MessageContainerList.GetMessage(executionContext, "is null"));
            }

            if (string.IsNullOrWhiteSpace(subscriptionHeaderDTO.ProductSubscriptionName))
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Subscription Name") + " " + MessageContainerList.GetMessage(executionContext, "is null"));
            }

            if (string.IsNullOrWhiteSpace(subscriptionHeaderDTO.ProductSubscriptionDescription))
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Subscription Description") + " " + MessageContainerList.GetMessage(executionContext, "is null"));
            }

            if (subscriptionHeaderDTO.SubscriptionCycle <= 0)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Subscription Cycle") + " " + MessageContainerList.GetMessage(executionContext, "is null"));
            }

            if (string.IsNullOrWhiteSpace(subscriptionHeaderDTO.UnitOfSubscriptionCycle))
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Unit of Subscription Cycle") + " " + MessageContainerList.GetMessage(executionContext, "is null"));
            }
            else
            {
                if (UnitOfSubscriptionCycle.ValidUnitOfSubscriptionCycle(subscriptionHeaderDTO.UnitOfSubscriptionCycle) == false)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Unit of Subscription Cycle")));//Please enter valid value for &1
                }
            }

            if (subscriptionHeaderDTO.SubscriptionCycleValidity <= 0)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Subscription Cycle Validity") + " " + MessageContainerList.GetMessage(executionContext, "is null"));
            }


            if (string.IsNullOrWhiteSpace(subscriptionHeaderDTO.SelectedPaymentCollectionMode))
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Payment Collection Mode") + " " + MessageContainerList.GetMessage(executionContext, "is null"));
            }
            else
            {
                if (SubscriptionPaymentCollectionMode.ValidPaymentCollectionMode(subscriptionHeaderDTO.SelectedPaymentCollectionMode) == false)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Payment Collection Mode")));//Please enter valid value for &1
                }
            }

            if (SubscriptionStatus.ValidSubscritionHeaderStatus(subscriptionHeaderDTO.Status) == false)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Subscription Status")));//Please enter valid value for &1
            }
            if (string.IsNullOrWhiteSpace(subscriptionHeaderDTO.CancellationOption))
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Cancellation Option") + " " + MessageContainerList.GetMessage(executionContext, "is null"));
            }
            if (subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList != null && subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList.Any())
            {
                for (int i = 0; i < subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList.Count; i++)
                {
                    if (subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList[i].IsChanged || subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList[i].SubscriptionBillingScheduleId < 0)
                    {
                        SubscriptionBillingScheduleBL subscriptionBillingScheduleBL = new SubscriptionBillingScheduleBL(executionContext, subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList[i]);
                        subscriptionBillingScheduleBL.ValidateSubscriptionBillingSchedule(sqlTransaction);
                    }
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// CancelSubscription
        /// </summary>
        public void CancelSubscription(Utilities utilities, SqlTransaction sqlTrx = null)
        {
            log.LogMethodEntry(sqlTrx);
            if (subscriptionHeaderDTO != null && subscriptionHeaderDTO.SubscriptionHeaderId > -1
                && subscriptionHeaderDTO.IsActive && subscriptionHeaderDTO.Status == SubscriptionStatus.ACTIVE)
            {
                if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "CANCEL_SUBSCRIPTION_REQUIRES_MANAGER_APPROVAL")
                    && string.IsNullOrWhiteSpace(subscriptionHeaderDTO.CancellationApprovedBy))
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 268));
                    //Manager Approval Required for this Task
                }
                if (subscriptionHeaderDTO.CancellationOption == SubscriptionCancellationOption.CANCELL_UNBILLED_CYCLES)
                {
                    CancelUnbilledCycles(utilities, sqlTrx);
                }
                else if (subscriptionHeaderDTO.CancellationOption == SubscriptionCancellationOption.CANCEL_AUTO_RENEWAL_ONLY)
                {
                    DisableAutoRenewalForCancellation(utilities, sqlTrx);
                }
                else
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2299, MessageContainerList.GetMessage(executionContext, "Cancellation Option")));
                    // "Value is not defined for &1 " 
                }
            }
            else
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2877));// "Sorry, only active subscription can be cancelled" 
            }
            log.LogMethodExit();
        }

        private void CancelUnbilledCycles(Utilities utilities, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry();
            if (subscriptionHeaderDTO != null && subscriptionHeaderDTO.CancellationOption == SubscriptionCancellationOption.CANCELL_UNBILLED_CYCLES)
            {
                ParafaitDBTransaction dBTransaction = null;
                try
                {
                    if (sqlTrx == null)
                    {
                        dBTransaction = new ParafaitDBTransaction();
                        dBTransaction.BeginTransaction();
                        sqlTrx = dBTransaction.SQLTrx;
                    }
                    subscriptionHeaderDTO.IsActive = false;
                    if (subscriptionHeaderDTO.AutoRenew)
                    {
                        subscriptionHeaderDTO.AutoRenew = false;
                    }
                    if (subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList != null && subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList.Any()
                         && subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList.Exists(sbs => sbs.IsActive == true && sbs.Status != SubscriptionStatus.CANCELLED
                                                                                                   && sbs.TransactionId == -1))
                    {
                        subscriptionHeaderDTO.Status = SubscriptionStatus.CANCELLED; //leave the status as is if it is already billed
                    }
                    subscriptionHeaderDTO.CancelledBy = executionContext.GetUserId();
                    List<int> subscriptionBillingCycleIdList = new List<int>();
                    if (subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList != null && subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList.Any())
                    {
                        for (int i = 0; i < subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList.Count; i++)
                        {
                            SubscriptionBillingScheduleDTO subscriptionBillingScheduleDTO = subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList[i];
                            subscriptionBillingCycleIdList = MarkUnBilledScheduleAsCancelled(subscriptionBillingScheduleDTO, subscriptionHeaderDTO.CancelledBy, subscriptionHeaderDTO.CancellationApprovedBy, subscriptionBillingCycleIdList);
                        }
                        CancelSubscriptionBillingCycleEntitlements(subscriptionBillingCycleIdList, sqlTrx);
                        this.Save(sqlTrx);
                        if (dBTransaction != null)
                        {
                            dBTransaction.EndTransaction();
                            dBTransaction.Dispose();
                            dBTransaction = null;
                            sqlTrx = null;
                        }
                        SendCancelSubscriptionMessage(utilities, sqlTrx);
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    if (dBTransaction != null)
                    {
                        dBTransaction.RollBack();
                        dBTransaction.Dispose();
                        dBTransaction = null;
                        sqlTrx = null;
                    }
                    //restore DTO
                    LoadSubscriptionHeaderDTO(subscriptionHeaderDTO.SubscriptionHeaderId, true, sqlTrx);
                    throw;
                }
            }
            log.LogMethodExit();
        }

        private List<int> MarkUnBilledScheduleAsCancelled(SubscriptionBillingScheduleDTO subscriptionBillingScheduleDTO, string cancelledBy, string cancellationApprovedBy, List<int> subscriptionBillingCycleIdList, SubscriptionUnPauseDetailsDTO subscriptionUnPauseDetailsDTO = null)
        {
            log.LogMethodEntry(subscriptionBillingScheduleDTO, cancelledBy, cancellationApprovedBy, subscriptionBillingCycleIdList, subscriptionUnPauseDetailsDTO);
            List<int> returnVale = new List<int>(subscriptionBillingCycleIdList);
            if (subscriptionBillingScheduleDTO.Status != SubscriptionStatus.CANCELLED
                 && subscriptionBillingScheduleDTO.IsActive && subscriptionBillingScheduleDTO.TransactionId == -1)
            {
                //Mark active unbilled cycles as cancelled
                subscriptionBillingScheduleDTO.IsActive = false;
                subscriptionBillingScheduleDTO.CancelledBy = cancelledBy;
                subscriptionBillingScheduleDTO.CancellationApprovedBy = cancellationApprovedBy;
                subscriptionBillingScheduleDTO.Status = SubscriptionStatus.CANCELLED;
                if (subscriptionUnPauseDetailsDTO != null)
                {
                    subscriptionBillingScheduleDTO = SetPostponeDates(subscriptionUnPauseDetailsDTO, subscriptionBillingScheduleDTO);
                }
                returnVale.Add(subscriptionBillingScheduleDTO.SubscriptionBillingScheduleId);
            }
            log.LogMethodExit(returnVale);
            return returnVale;
        }

        private void CancelSubscriptionBillingCycleEntitlements(List<int> subscriptionBillingCycleIdList, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(subscriptionBillingCycleIdList, sqlTrx);
            if (subscriptionBillingCycleIdList != null && subscriptionBillingCycleIdList.Any())
            {
                //Get linked accounts
                AccountListBL accountListBL = new AccountListBL(executionContext);
                List<KeyValuePair<AccountDTO.SearchByParameters, string>> searchParam = new List<KeyValuePair<AccountDTO.SearchByParameters, string>>();
                searchParam.Add(new KeyValuePair<AccountDTO.SearchByParameters, string>(AccountDTO.SearchByParameters.SUBSCRIPTION_HEADER_ID, subscriptionHeaderDTO.SubscriptionHeaderId.ToString()));
                searchParam.Add(new KeyValuePair<AccountDTO.SearchByParameters, string>(AccountDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                List<AccountDTO> accountDTOList = accountListBL.GetAccountDTOList(searchParam, true, true, sqlTrx);
                if (accountDTOList != null && accountDTOList.Any())
                {
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

        private void DisableAutoRenewalForCancellation(Utilities utilities, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(sqlTrx);
            if (subscriptionHeaderDTO != null && subscriptionHeaderDTO.CancellationOption == SubscriptionCancellationOption.CANCEL_AUTO_RENEWAL_ONLY)
            {
                ParafaitDBTransaction dBTransaction = null;
                try
                {
                    //subscriptionHeaderDTO.IsActive = false;
                    if (subscriptionHeaderDTO.AutoRenew)
                    {
                        subscriptionHeaderDTO.AutoRenew = false;
                    }
                    else
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2995));
                        // "Invalid cancellation operation as subscription is not enabled for auto renewal"
                    }
                    subscriptionHeaderDTO.CancelledBy = executionContext.GetUserId();
                    if (sqlTrx == null)
                    {
                        dBTransaction = new ParafaitDBTransaction();
                        dBTransaction.BeginTransaction();
                        sqlTrx = dBTransaction.SQLTrx;
                    }
                    this.Save(sqlTrx);
                    if (dBTransaction != null)
                    {
                        dBTransaction.EndTransaction();
                        dBTransaction.Dispose();
                        dBTransaction = null;
                        sqlTrx = null;
                    }
                    SendCancelSubscriptionMessage(utilities, sqlTrx);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    if (dBTransaction != null)
                    {
                        dBTransaction.RollBack();
                        dBTransaction.Dispose();
                        dBTransaction = null;
                        sqlTrx = null;
                    }
                    //restore DTO
                    LoadSubscriptionHeaderDTO(subscriptionHeaderDTO.SubscriptionHeaderId, true, sqlTrx);
                    throw;
                }
            }
            log.LogMethodExit();
        }
        private void SendCancelSubscriptionMessage(Utilities utilities, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(sqlTrx);
            SubscriptionEventsBL subscriptionEventsBL = new SubscriptionEventsBL(utilities, ParafaitFunctionEvents.CANCEL_SUBSCRIPTION_EVENT, this.subscriptionHeaderDTO, null, sqlTrx);
            subscriptionEventsBL.SendMessage(MessagingClientDTO.MessagingChanelType.NONE, sqlTrx);
            log.LogMethodExit();
        }
        /// <summary>
        /// Send Renewal Reminder
        /// </summary>
        public void SendRenewalReminder(Utilities utilities)
        {
            log.LogMethodEntry();
            try
            {
                if (CanSendRenewalMsg())
                {
                    SendRenewalReminderAndSave(utilities);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Send Renewal Reminder
        /// </summary>
        public void SendManualRenewalReminder(Utilities utilities)
        {
            log.LogMethodEntry();
            try
            {
                CanSendManualRenewalMsg();
                SendRenewalReminderAndSave(utilities);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            log.LogMethodExit();
        }

        private void SendRenewalReminderAndSave(Utilities utilities)
        {
            log.LogMethodEntry();
            SubscriptionEventsBL subscriptionEventsBL = new SubscriptionEventsBL(utilities, ParafaitFunctionEvents.RENEWAL_REMINDER_EVENT, this.subscriptionHeaderDTO, null);
            subscriptionEventsBL.SendMessage(MessagingClientDTO.MessagingChanelType.NONE);
            this.subscriptionHeaderDTO.LastRenewalReminderSentOn = serverDateTime.GetServerDateTime();
            this.subscriptionHeaderDTO.RenewalReminderCount = (this.subscriptionHeaderDTO.RenewalReminderCount == null ? 1
                                                                 : subscriptionHeaderDTO.RenewalReminderCount + 1);
            Save(null);
            log.LogMethodExit();
        }

        /// <summary>
        /// CanSendCardExpiryMsg
        /// </summary>
        public bool CanSendRenewalMsg()
        {
            log.LogMethodEntry();
            bool canSend = true;
            DateTime currentDate = serverDateTime.GetServerDateTime().Date;
            int frequencyInDays = (subscriptionHeaderDTO.ReminderFrequencyInDays == null ? 1 : (int)subscriptionHeaderDTO.ReminderFrequencyInDays);
            SubscriptionBillingScheduleDTO lastBillingScheduleDTO = GetLastBillingScheduleDTO();
            int sendRemindersXdaysBefore = (subscriptionHeaderDTO.SendFirstReminderBeforeXDays == null ? 0 : (int)subscriptionHeaderDTO.SendFirstReminderBeforeXDays);
            if ((lastBillingScheduleDTO.BillToDate - currentDate).Days > sendRemindersXdaysBefore)
            {
                canSend = false;
            }
            if (subscriptionHeaderDTO.LastRenewalReminderSentOn != null
                 && (((DateTime)subscriptionHeaderDTO.LastRenewalReminderSentOn).Date - currentDate).Days < frequencyInDays)
            {
                canSend = false;
            }
            log.LogMethodExit(canSend);
            return canSend;
        }
        /// <summary>
        /// CanSendCardExpiryMsg
        /// </summary>
        private void CanSendManualRenewalMsg()
        {
            log.LogMethodEntry();
            DateTime currentDate = serverDateTime.GetServerDateTime().Date;
            SubscriptionBillingScheduleDTO lastBillingScheduleDTO = GetLastBillingScheduleDTO();
            if (currentDate < lastBillingScheduleDTO.BillFromDate)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2893,
                                                lastBillingScheduleDTO.BillFromDate.ToString(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "DATE_FORMAT"))));
                //'You can send reminder only after &1'
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Send Payment Failure Message
        /// </summary>
        public void SendPaymentFailureMessage(Utilities utilities)
        {
            log.LogMethodEntry();
            LoadAlertSetupDetails();
            if (CanSendPaymentFailureMessage())
            {
                SubscriptionEventsBL subscriptionEventsBL = new SubscriptionEventsBL(utilities, ParafaitFunctionEvents.SUBSCRIPTION_PAYMENT_FAILURE_EVENT, subscriptionHeaderDTO, null);
                subscriptionEventsBL.SendMessage(MessagingClientDTO.MessagingChanelType.NONE);
                this.subscriptionHeaderDTO.LastPaymentRetryLimitReminderSentOn = serverDateTime.GetServerDateTime();
                this.subscriptionHeaderDTO.PaymentRetryLimitReminderCount = (this.subscriptionHeaderDTO.PaymentRetryLimitReminderCount == null ? 1 : subscriptionHeaderDTO.PaymentRetryLimitReminderCount + 1);
                Save(null);
            }
            log.LogMethodExit();
        }
        private bool CanSendPaymentFailureMessage()
        {
            log.LogMethodEntry();
            bool canSend = false;
            int alertCount = this.subscriptionHeaderDTO.PaymentRetryLimitReminderCount == null ? 0 : (int)this.subscriptionHeaderDTO.PaymentRetryLimitReminderCount;
            DateTime serverDateTimeValue = serverDateTime.GetServerDateTime();
            if (this.subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList.Exists(sbs => sbs.IsActive
                                                                                           && sbs.LineType == SubscriptionLineType.BILLING_LINE
                                                                                           && sbs.BillOnDate < serverDateTimeValue
                                                                                           && sbs.TransactionId == -1)
                && alertCount < numberOfMessagesForPaymentRetryLimit)
            {
                canSend = true;
            }
            //If last sent date is within frequence then dont send for now
            if (this.subscriptionHeaderDTO.LastPaymentRetryLimitReminderSentOn != null
               && ((DateTime)this.subscriptionHeaderDTO.LastPaymentRetryLimitReminderSentOn).Date.AddDays(frequencyInDaysForPaymentRetryLimitMessage) > serverDateTime.GetServerDateTime())
            {
                canSend = false;
            }
            log.LogMethodExit(canSend);
            return canSend;
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
                        case NUMBEROFMESSAGESFORPAYMENTRETRYLIMIT:
                            if (int.TryParse(lookupValuesDTOList[i].Description, out value))
                            {
                                numberOfMessagesForPaymentRetryLimit = value;
                            }
                            break;
                        case FREQUENCYINDAYSFORPAYMENTRETRYLIMITMESSAGE:
                            if (int.TryParse(lookupValuesDTOList[i].Description, out value))
                            {
                                frequencyInDaysForPaymentRetryLimitMessage = value;
                            }
                            break;
                        //case CUSTOMERCARDEXPIRESINXDAYS:
                        //    if (int.TryParse(lookupValuesDTOList[i].Description, out value))
                        //    {
                        //        customerCardExpiresInXDays = value;
                        //    }
                        //    break;
                        //case NUMBEROFMESSAGESFORCUSTOMERCARDEXPIRY:
                        //    if (int.TryParse(lookupValuesDTOList[i].Description, out value))
                        //    {
                        //        numberOfMessagesForCustomerCardExpiry = value;
                        //    }
                        //    break;
                        //case FREQUENCYINDAYSFORCUSTOMERCARDEXPIRYMESSAGE:
                        //    if (int.TryParse(lookupValuesDTOList[i].Description, out value))
                        //    {
                        //        frequencyInDaysForCustomerCardExpiryMessage = value;
                        //    }
                        //    break;
                        default:
                            break;
                    }
                }
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Get Last Billing ScheduleDTO
        /// </summary>
        /// <returns></returns>
        public SubscriptionBillingScheduleDTO GetLastBillingScheduleDTO()
        {
            log.LogMethodEntry();
            SubscriptionBillingScheduleDTO lastBillingScheduleDTO = null;
            if (subscriptionHeaderDTO != null && subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList != null && subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList.Any())
            {
                DateTime maxBillToDate = subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList.Where(sbs => sbs.LineType == SubscriptionLineType.BILLING_LINE).Max(sbsMax => sbsMax.BillToDate);
                lastBillingScheduleDTO = subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList.Find(sbs => sbs.BillToDate == maxBillToDate && sbs.LineType == SubscriptionLineType.BILLING_LINE);
            }
            else
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Subscription Billing ScheduleDTO List") + " " + MessageContainerList.GetMessage(executionContext, "is null"));
            }
            log.LogMethodExit(lastBillingScheduleDTO);
            return lastBillingScheduleDTO;
        }
        /// <summary>
        /// Get method for subscriptionHeaderDTO
        /// </summary>
        public SubscriptionHeaderDTO SubscriptionHeaderDTO { get { return subscriptionHeaderDTO; } }
        /// <summary>
        /// MarkAsExpired
        /// </summary>
        /// <param name="sqlTrx"></param>
        public void MarkAsExpired(Utilities utilities, SqlTransaction sqlTrx = null)
        {
            log.LogMethodEntry(sqlTrx);
            if (this.SubscriptionHeaderDTO != null)
            {
                CanExpireTheSubscription(utilities, sqlTrx);
                this.subscriptionHeaderDTO.Status = SubscriptionStatus.EXPIRED;
                this.Save(sqlTrx);
            }
            log.LogMethodExit();
        }

        private void CanExpireTheSubscription(Utilities utilities, SqlTransaction sqlTrx = null)
        {
            log.LogMethodEntry(sqlTrx);
            if (this.subscriptionHeaderDTO != null)
            {
                if (subscriptionHeaderDTO.SubscriptionHeaderId == -1)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 665));
                    //Please save changes before this operation
                }
                if (subscriptionHeaderDTO.AutoRenew)
                {
                    SubscriptionHeaderListBL subscriptionHeaderListBL = new SubscriptionHeaderListBL(executionContext);
                    List<KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>> searchParam = new List<KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>>();
                    searchParam.Add(new KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>(SubscriptionHeaderDTO.SearchByParameters.SOURCE_SUBSCRIPTION_HEADER_ID, subscriptionHeaderDTO.SubscriptionHeaderId.ToString()));
                    searchParam.Add(new KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>(SubscriptionHeaderDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                    List<SubscriptionHeaderDTO> subscriptionHeaderDTOList = subscriptionHeaderListBL.GetSubscriptionHeaderDTOList(searchParam, utilities, false, sqlTrx);
                    if (subscriptionHeaderDTOList == null || subscriptionHeaderDTOList.Any() == false)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2898));
                        //Auto renewal is enabled for this subscription. Cannot expire
                    }
                }

                if (subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList == null || subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList.Any() == false)
                {
                    LoadChildRecords(subscriptionHeaderDTO.SubscriptionHeaderId, sqlTrx);
                }
                SubscriptionBillingScheduleDTO lastBillCycle = GetLastBillingScheduleDTO();
                if (lastBillCycle != null && lastBillCycle.BillToDate > serverDateTime.GetServerDateTime())
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2897));
                    //Sorry cannot expire active subscription
                }
                if (subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList != null && subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList.Any()
                    && subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList.Exists(sbs => sbs.IsActive && sbs.Status == SubscriptionStatus.ACTIVE && sbs.TransactionId == -1))
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Sorry cannot expire subscription with unbilled active schedules"));
                    //Sorry cannot expire subscription with unbilled active schedules
                }
            }
            else
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2317, "SubscriptionHeader"));
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// PauseSubscription
        /// </summary>
        /// <param name="utilities"></param>
        /// <param name="sqlTrx"></param>
        public void PauseSubscription(Utilities utilities, SqlTransaction sqlTrx = null)
        {
            log.LogMethodEntry();
            if (subscriptionHeaderDTO != null)
            {
                CanPauseSubscription();
                DateTime serverTime = serverDateTime.GetServerDateTime();
                ParafaitDBTransaction dBTransaction = null;
                try
                {
                    if (sqlTrx == null)
                    {
                        dBTransaction = new ParafaitDBTransaction();
                        dBTransaction.BeginTransaction();
                        sqlTrx = dBTransaction.SQLTrx;
                    }
                    subscriptionHeaderDTO.Status = SubscriptionStatus.PAUSED;
                    subscriptionHeaderDTO.PausedBy = executionContext.GetUserId();
                    List<int> subscriptionBillingCycleIdList = new List<int>();
                    for (int i = 0; i < subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList.Count; i++)
                    {
                        if (subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList[i].BillToDate <= serverTime)
                        {
                            continue; //skipp past cycles
                        }
                        else if (subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList[i].IsActive
                                 && subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList[i].Status == SubscriptionStatus.ACTIVE)
                        {
                            subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList[i].Status = SubscriptionStatus.PAUSED;
                            subscriptionBillingCycleIdList.Add(subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList[i].SubscriptionBillingScheduleId);
                        }
                    }
                    if (subscriptionBillingCycleIdList != null && subscriptionBillingCycleIdList.Any())
                    {//Get linked accounts
                        AccountListBL accountListBL = new AccountListBL(executionContext);
                        List<KeyValuePair<AccountDTO.SearchByParameters, string>> searchParam = new List<KeyValuePair<AccountDTO.SearchByParameters, string>>();
                        searchParam.Add(new KeyValuePair<AccountDTO.SearchByParameters, string>(AccountDTO.SearchByParameters.SUBSCRIPTION_HEADER_ID, subscriptionHeaderDTO.SubscriptionHeaderId.ToString()));
                        searchParam.Add(new KeyValuePair<AccountDTO.SearchByParameters, string>(AccountDTO.SearchByParameters.VALID_FLAG, "Y"));
                        searchParam.Add(new KeyValuePair<AccountDTO.SearchByParameters, string>(AccountDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<AccountDTO> accountDTOList = accountListBL.GetAccountDTOList(searchParam, true, true, sqlTrx);
                        if (accountDTOList != null && accountDTOList.Any())
                        {
                            for (int i = 0; i < accountDTOList.Count; i++)
                            {
                                AccountDTO accountDTO = accountDTOList[i];
                                AccountBL accountBL = new AccountBL(executionContext, accountDTO);
                                accountBL.PauseSubscriptionBillingCycleEntitlements(subscriptionBillingCycleIdList, sqlTrx);
                            }
                        }
                    }
                    this.Save(sqlTrx);
                    if (dBTransaction != null)
                    {
                        dBTransaction.EndTransaction();
                        dBTransaction.Dispose();
                        dBTransaction = null;
                        sqlTrx = null;
                    }
                    SendPauseSubscriptionMessage(utilities, sqlTrx);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    if (dBTransaction != null)
                    {
                        dBTransaction.RollBack();
                        dBTransaction.Dispose();
                        dBTransaction = null;
                        sqlTrx = null;
                    }
                    //restore DTO
                    LoadSubscriptionHeaderDTO(subscriptionHeaderDTO.SubscriptionHeaderId, true, sqlTrx);
                    throw;
                }
            }
            log.LogMethodExit();
        }

        private void CanPauseSubscription()
        {
            log.LogMethodEntry();
            if (subscriptionHeaderDTO.AllowPause == false)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2964));
                // "Sorry, Pause operation is not allowed for this subscription"
            }
            if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, MANAGER_APPROVAL_FOR_PAUSE_UNPAUSE_SUBSCRIPTION)
               && string.IsNullOrWhiteSpace(subscriptionHeaderDTO.PauseApprovedBy))
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 268));
                //Manager Approval Required for this Task
            }
            if (subscriptionHeaderDTO.Status == SubscriptionStatus.PAUSED)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2966));
                // "Invalid operation, Subscription is already in Paused status"
            }
            DateTime serverTime1 = serverDateTime.GetServerDateTime();
            if (subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList != null
                && subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList.Exists(sbc => sbc.BillToDate >= serverTime1 && sbc.IsActive
                                                                                          && (sbc.Status != SubscriptionStatus.CANCELLED
                                                                                                  || sbc.Status != SubscriptionStatus.PAUSED)) == false)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2965));
                // "No active billing schedules to pause"
            }
            log.LogMethodExit();
        }

        private void SendPauseSubscriptionMessage(Utilities utilities, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(sqlTrx);
            SubscriptionEventsBL subscriptionEventsBL = new SubscriptionEventsBL(utilities, ParafaitFunctionEvents.PAUSE_SUBSCRIPTION_EVENT, this.subscriptionHeaderDTO, null, sqlTrx);
            subscriptionEventsBL.SendMessage(MessagingClientDTO.MessagingChanelType.NONE, sqlTrx);
            log.LogMethodExit();
        }
        /// <summary>
        /// UnPauseSubscription
        /// </summary>
        /// <param name="utilities"></param>
        /// <param name="subscriptionUnPauseDetailsDTOList"></param>
        /// <param name="sqlTrx"></param>
        public void UnPauseSubscription(Utilities utilities, List<SubscriptionUnPauseDetailsDTO> subscriptionUnPauseDetailsDTOList, SqlTransaction sqlTrx = null)
        {
            log.LogMethodEntry(subscriptionUnPauseDetailsDTOList, sqlTrx);
            CanPerformUnPauseSubscription(subscriptionUnPauseDetailsDTOList);
            List<int> subscriptionBillingCycleIdListForResume = new List<int>();
            List<int> subscriptionBillingCycleIdListForCancellation = new List<int>();
            List<SubscriptionUnPauseDetailsDTO> listForPostPone = new List<SubscriptionUnPauseDetailsDTO>();
            string loginUser = executionContext.GetUserId();
            ParafaitDBTransaction dBTransaction = null;
            try
            {
                if (sqlTrx == null)
                {
                    dBTransaction = new ParafaitDBTransaction();
                    dBTransaction.BeginTransaction();
                    sqlTrx = dBTransaction.SQLTrx;
                }
                for (int i = 0; i < subscriptionUnPauseDetailsDTOList.Count; i++)
                {
                    if (subscriptionUnPauseDetailsDTOList[i].UnPauseAction == SubscriptionUnPauseOptions.BILL)
                    {
                        EnableBillingScheduleForBilling(subscriptionUnPauseDetailsDTOList[i].SubscriptionBillingScheduleId);
                        subscriptionBillingCycleIdListForResume.Add(subscriptionUnPauseDetailsDTOList[i].SubscriptionBillingScheduleId);
                    }
                    else if (subscriptionUnPauseDetailsDTOList[i].UnPauseAction == SubscriptionUnPauseOptions.CANCEL)
                    {
                        SubscriptionBillingScheduleDTO subscriptionBillingScheduleDTO = this.subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList.Find(sbc => sbc.SubscriptionBillingScheduleId == subscriptionUnPauseDetailsDTOList[i].SubscriptionBillingScheduleId);
                        if (subscriptionBillingScheduleDTO != null)
                        {
                            subscriptionBillingCycleIdListForCancellation = MarkUnBilledScheduleAsCancelled(subscriptionBillingScheduleDTO, loginUser, subscriptionHeaderDTO.UnPauseApprovedBy, subscriptionBillingCycleIdListForCancellation, subscriptionUnPauseDetailsDTOList[i]);
                        }
                    }
                    else if (subscriptionUnPauseDetailsDTOList[i].UnPauseAction == SubscriptionUnPauseOptions.POSTPONE)
                    {
                        SetPostponeDetails(subscriptionUnPauseDetailsDTOList[i]);
                        listForPostPone.Add(subscriptionUnPauseDetailsDTOList[i]);
                    }
                }
                ResumeBillingCycleEntitlements(subscriptionBillingCycleIdListForResume, sqlTrx);
                CancelSubscriptionBillingCycleEntitlements(subscriptionBillingCycleIdListForCancellation, sqlTrx);
                PostponeSubscriptionBillingCycleEntitlements(listForPostPone, sqlTrx);
                subscriptionHeaderDTO.Status = SubscriptionStatus.ACTIVE;
                subscriptionHeaderDTO.UnPausedBy = loginUser;
                UpdateSubscriptionEndDate();
                Save(sqlTrx);
                if (dBTransaction != null)
                {
                    dBTransaction.EndTransaction();
                    dBTransaction.Dispose();
                    dBTransaction = null;
                    sqlTrx = null;
                }
                SendUnPauseSubscriptionMessage(utilities, sqlTrx);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                if (dBTransaction != null)
                {
                    dBTransaction.RollBack();
                    dBTransaction.Dispose();
                    dBTransaction = null;
                    sqlTrx = null;
                }
                //restore DTO
                LoadSubscriptionHeaderDTO(subscriptionHeaderDTO.SubscriptionHeaderId, true, sqlTrx);
                throw;
            }
            log.LogMethodExit();
        }
        private void ResumeBillingCycleEntitlements(List<int> subscriptionBillingCycleIdList, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(subscriptionBillingCycleIdList, sqlTrx);
            if (subscriptionBillingCycleIdList != null && subscriptionBillingCycleIdList.Any())
            {
                //Get linked accounts
                AccountListBL accountListBL = new AccountListBL(executionContext);
                List<KeyValuePair<AccountDTO.SearchByParameters, string>> searchParam = new List<KeyValuePair<AccountDTO.SearchByParameters, string>>();
                searchParam.Add(new KeyValuePair<AccountDTO.SearchByParameters, string>(AccountDTO.SearchByParameters.SUBSCRIPTION_HEADER_ID, subscriptionHeaderDTO.SubscriptionHeaderId.ToString()));
                searchParam.Add(new KeyValuePair<AccountDTO.SearchByParameters, string>(AccountDTO.SearchByParameters.VALID_FLAG, "Y"));
                searchParam.Add(new KeyValuePair<AccountDTO.SearchByParameters, string>(AccountDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                List<AccountDTO> accountDTOList = accountListBL.GetAccountDTOList(searchParam, true, true, sqlTrx);
                if (accountDTOList != null && accountDTOList.Any())
                {
                    for (int i = 0; i < accountDTOList.Count; i++)
                    {
                        AccountDTO accountDTO = accountDTOList[i];
                        AccountBL accountBL = new AccountBL(executionContext, accountDTO);
                        accountBL.ResumeSubscriptionBillingCycleEntitlements(subscriptionBillingCycleIdList, sqlTrx);
                    }
                }
            }
            log.LogMethodExit();
        }
        private void CanPerformUnPauseSubscription(List<SubscriptionUnPauseDetailsDTO> subscriptionUnPauseDetailsDTOList)
        {
            log.LogMethodEntry(subscriptionUnPauseDetailsDTOList);

            if (subscriptionHeaderDTO == null || subscriptionHeaderDTO.Status != SubscriptionStatus.PAUSED)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2969));
                //"Sorry cannot proceed. Subscription should be in Pause status"
            }
            if (subscriptionUnPauseDetailsDTOList == null || subscriptionUnPauseDetailsDTOList.Any() == false)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2968));
                //"Unpause details are missing"
            }
            if (subscriptionUnPauseDetailsDTOList != null && subscriptionUnPauseDetailsDTOList.Any() && subscriptionUnPauseDetailsDTOList.Exists(sud => string.IsNullOrWhiteSpace(sud.UnPauseAction) == true))
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2972));
                // "Unpause action is not defined"
            }
            if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, MANAGER_APPROVAL_FOR_PAUSE_UNPAUSE_SUBSCRIPTION)
               && string.IsNullOrWhiteSpace(subscriptionHeaderDTO.UnPauseApprovedBy))
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 268));
                //Manager Approval Required for this Task
            }
            ValidatePostponeItems(subscriptionUnPauseDetailsDTOList);
            ReceivedActionForAllPausedCycles(subscriptionUnPauseDetailsDTOList);
            log.LogMethodExit();
        }
        /// <summary>
        /// ValidatePostponeItems
        /// </summary>
        /// <param name="subscriptionUnPauseDetailsDTOList"></param>
        public void ValidatePostponeItems(List<SubscriptionUnPauseDetailsDTO> subscriptionUnPauseDetailsDTOList)
        {
            log.LogMethodEntry(subscriptionUnPauseDetailsDTOList);
            bool postponeChainStarted = false;
            List<ValidationError> validationErrorList = new List<ValidationError>();
            for (int i = 0; i < subscriptionUnPauseDetailsDTOList.Count; i++)
            {
                if (subscriptionUnPauseDetailsDTOList[i].UnPauseAction == SubscriptionUnPauseOptions.POSTPONE && postponeChainStarted == false)
                {
                    postponeChainStarted = true;
                }
                if (subscriptionUnPauseDetailsDTOList[i].UnPauseAction == SubscriptionUnPauseOptions.POSTPONE && subscriptionUnPauseDetailsDTOList[i].NewBillFromDate == null)
                {
                    validationErrorList.Add(new ValidationError("SubscriptionUnPauseDetailsDTO", "NewBillFromDate", MessageContainerList.GetMessage(executionContext, 2970)));
                    //"New bill from date is missing"
                }
                if (postponeChainStarted && subscriptionUnPauseDetailsDTOList[i].UnPauseAction == SubscriptionUnPauseOptions.BILL)
                {
                    validationErrorList.Add(new ValidationError("SubscriptionUnPauseDetailsDTO", "UnPauseAction", MessageContainerList.GetMessage(executionContext, 2971)));
                    // "Invalid unpause action, cannot set BILL after Postpone is initiated" 
                }
            }
            if (validationErrorList != null && validationErrorList.Any())
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Validation"), validationErrorList);
            }
            log.LogMethodExit();
        }
        private void ReceivedActionForAllPausedCycles(List<SubscriptionUnPauseDetailsDTO> subscriptionUnPauseDetailsDTOList)
        {
            log.LogMethodEntry(subscriptionUnPauseDetailsDTOList);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            string dateFormat = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "DATE_FORMAT");
            if (subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList != null && subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList.Any())
            {
                for (int i = 0; i < subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList.Count; i++)
                {
                    if (subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList[i].Status == SubscriptionStatus.PAUSED
                        && subscriptionUnPauseDetailsDTOList.Exists(sud => sud.SubscriptionHeaderId == subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList[i].SubscriptionHeaderId
                                                                   && sud.SubscriptionBillingScheduleId == subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList[i].SubscriptionBillingScheduleId) == false)
                    {
                        validationErrorList.Add(new ValidationError("subscriptionHeaderDTO", "SubscriptionBillingScheduleDTO",
                                                                    MessageContainerList.GetMessage(executionContext, 2973,
                                                                    subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList[i].BillFromDate.ToString(dateFormat))));
                        //"Unpause details are missing for &1"
                    }
                }
            }
            else
            {
                validationErrorList.Add(new ValidationError("subscriptionHeaderDTO", "SubscriptionBillingScheduleDTOList", MessageContainerList.GetMessage(executionContext, 2299, "SubscriptionBillingScheduleDTOList")));
                //Value is not defined for &1  
            }
            log.LogMethodExit();
        }
        private void EnableBillingScheduleForBilling(int subscriptionBillingScheduleId)
        {
            log.LogMethodEntry(subscriptionBillingScheduleId);
            SubscriptionBillingScheduleDTO subscriptionBillingScheduleDTO = this.subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList.Find(sbc => sbc.SubscriptionBillingScheduleId == subscriptionBillingScheduleId);
            subscriptionBillingScheduleDTO.Status = SubscriptionStatus.ACTIVE;
            log.LogMethodExit();
        }

        private void SetPostponeDetails(SubscriptionUnPauseDetailsDTO subscriptionUnPauseDetailsDTO)
        {
            log.LogMethodEntry(subscriptionUnPauseDetailsDTO);
            SubscriptionBillingScheduleDTO subscriptionBillingScheduleDTO = this.subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList.Find(sbc => sbc.SubscriptionBillingScheduleId == subscriptionUnPauseDetailsDTO.SubscriptionBillingScheduleId);
            subscriptionBillingScheduleDTO = SetPostponeDates(subscriptionUnPauseDetailsDTO, subscriptionBillingScheduleDTO);
            subscriptionBillingScheduleDTO.Status = SubscriptionStatus.ACTIVE;
            log.LogMethodExit();
        }
        private static SubscriptionBillingScheduleDTO SetPostponeDates(SubscriptionUnPauseDetailsDTO subscriptionUnPauseDetailsDTO, SubscriptionBillingScheduleDTO subscriptionBillingScheduleDTO)
        {
            log.LogMethodEntry(subscriptionUnPauseDetailsDTO, subscriptionBillingScheduleDTO);
            if (subscriptionUnPauseDetailsDTO.NewBillFromDate != null)
            {
                TimeSpan billTODateDiff = subscriptionBillingScheduleDTO.BillToDate - subscriptionBillingScheduleDTO.BillFromDate;
                TimeSpan billOnDateDiff = subscriptionBillingScheduleDTO.BillOnDate - subscriptionBillingScheduleDTO.BillFromDate;
                subscriptionBillingScheduleDTO.BillFromDate = (DateTime)subscriptionUnPauseDetailsDTO.NewBillFromDate;
                subscriptionBillingScheduleDTO.BillToDate = subscriptionBillingScheduleDTO.BillFromDate.Add(billTODateDiff);
                if (subscriptionBillingScheduleDTO.TransactionId == -1) //Retain bill on date is cycle is already billed.
                {
                    subscriptionBillingScheduleDTO.BillOnDate = subscriptionBillingScheduleDTO.BillFromDate.Add(billOnDateDiff);
                }
            }
            log.LogMethodExit(subscriptionBillingScheduleDTO);
            return subscriptionBillingScheduleDTO;
        }
        private void PostponeSubscriptionBillingCycleEntitlements(List<SubscriptionUnPauseDetailsDTO> listForPostPone, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(listForPostPone, sqlTrx);
            if (listForPostPone != null && listForPostPone.Any())
            {
                //Get linked accounts
                AccountListBL accountListBL = new AccountListBL(executionContext);
                List<KeyValuePair<AccountDTO.SearchByParameters, string>> searchParam = new List<KeyValuePair<AccountDTO.SearchByParameters, string>>();
                searchParam.Add(new KeyValuePair<AccountDTO.SearchByParameters, string>(AccountDTO.SearchByParameters.SUBSCRIPTION_HEADER_ID, subscriptionHeaderDTO.SubscriptionHeaderId.ToString()));
                searchParam.Add(new KeyValuePair<AccountDTO.SearchByParameters, string>(AccountDTO.SearchByParameters.VALID_FLAG, "Y"));
                searchParam.Add(new KeyValuePair<AccountDTO.SearchByParameters, string>(AccountDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                List<AccountDTO> accountDTOList = accountListBL.GetAccountDTOList(searchParam, true, true, sqlTrx);
                if (accountDTOList != null && accountDTOList.Any())
                {
                    for (int i = 0; i < accountDTOList.Count; i++)
                    {
                        AccountDTO accountDTO = accountDTOList[i];
                        AccountBL accountBL = new AccountBL(executionContext, accountDTO);
                        accountBL.PostponeSubscriptionBillingCycleEntitlements(listForPostPone, sqlTrx);
                    }
                }
            }
            log.LogMethodExit();
        }
        private void SendUnPauseSubscriptionMessage(Utilities utilities, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(sqlTrx);
            SubscriptionEventsBL subscriptionEventsBL = new SubscriptionEventsBL(utilities, ParafaitFunctionEvents.UNPAUSE_SUBSCRIPTION_EVENT, this.subscriptionHeaderDTO, null, sqlTrx);
            subscriptionEventsBL.SendMessage(MessagingClientDTO.MessagingChanelType.NONE, sqlTrx);
            log.LogMethodExit();
        }
        private void UpdateSubscriptionEndDate()
        {
            log.LogMethodEntry();
            if (this.subscriptionHeaderDTO != null && this.subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList != null
                && this.subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList.Any())
            {
                subscriptionHeaderDTO.SubscriptionEndDate = this.subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList.Where(sbs => sbs.LineType == SubscriptionLineType.BILLING_LINE).Max(sbs => sbs.BillToDate);
            }
            log.LogMethodExit();
        }
        internal void ClearSubscriptionEntity(string approverLoginId, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(approverLoginId, sqlTrx);
            if (subscriptionHeaderDTO != null && subscriptionHeaderDTO.SubscriptionHeaderId > -1 && subscriptionHeaderDTO.Status != SubscriptionStatus.CANCELLED)
            {
                ParafaitDBTransaction dBTransaction = null;
                try
                {
                    if (sqlTrx == null)
                    {
                        dBTransaction = new ParafaitDBTransaction();
                        dBTransaction.BeginTransaction();
                        sqlTrx = dBTransaction.SQLTrx;
                    }
                    subscriptionHeaderDTO.IsActive = false;
                    if (subscriptionHeaderDTO.AutoRenew)
                    {
                        subscriptionHeaderDTO.AutoRenew = false;
                    }
                    subscriptionHeaderDTO.Status = SubscriptionStatus.CANCELLED;
                    subscriptionHeaderDTO.CancelledBy = executionContext.GetUserId();
                    if (string.IsNullOrWhiteSpace(approverLoginId) == false)
                    {
                        subscriptionHeaderDTO.CancellationApprovedBy = approverLoginId;
                    }
                    List<int> subscriptionBillingCycleIdList = new List<int>();
                    if (subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList != null && subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList.Any())
                    {
                        for (int i = 0; i < subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList.Count; i++)
                        {
                            SubscriptionBillingScheduleDTO subscriptionBillingScheduleDTO = subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList[i];
                            if (subscriptionBillingScheduleDTO.Status != SubscriptionStatus.CANCELLED)
                            {
                                subscriptionBillingScheduleDTO.IsActive = false;
                                subscriptionBillingScheduleDTO.CancelledBy = executionContext.GetUserId();
                                if (string.IsNullOrWhiteSpace(approverLoginId) == false)
                                {
                                    subscriptionBillingScheduleDTO.CancellationApprovedBy = approverLoginId;
                                }
                                subscriptionBillingScheduleDTO.Status = SubscriptionStatus.CANCELLED;
                                subscriptionBillingCycleIdList.Add(subscriptionBillingScheduleDTO.SubscriptionBillingScheduleId);
                            }
                        }
                        CancelSubscriptionBillingCycleEntitlements(subscriptionBillingCycleIdList, sqlTrx);
                        this.Save(sqlTrx);
                        if (dBTransaction != null)
                        {
                            dBTransaction.EndTransaction();
                            dBTransaction.Dispose();
                            dBTransaction = null;
                            sqlTrx = null;
                        }
                        //SendCancelSubscriptionMessage(utilities, sqlTrx);
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    if (dBTransaction != null)
                    {
                        dBTransaction.RollBack();
                        dBTransaction.Dispose();
                        dBTransaction = null;
                        sqlTrx = null;
                    }
                    //restore DTO
                    LoadSubscriptionHeaderDTO(subscriptionHeaderDTO.SubscriptionHeaderId, true, sqlTrx);
                    throw;
                }
            }
            log.LogMethodExit();
        }
        private void SetFromSiteTimeOffset()
        {
            log.LogMethodEntry(subscriptionHeaderDTO);
            if (executionContext != null && executionContext.IsCorporate)
            {
                if (subscriptionHeaderDTO != null)
                {
                    subscriptionHeaderDTO.SubscriptionStartDate = SiteContainerList.FromSiteDateTime(subscriptionHeaderDTO.SiteId, subscriptionHeaderDTO.SubscriptionStartDate);
                    subscriptionHeaderDTO.SubscriptionEndDate = SiteContainerList.FromSiteDateTime(subscriptionHeaderDTO.SiteId, subscriptionHeaderDTO.SubscriptionEndDate);
                    if (subscriptionHeaderDTO.SeasonStartDate != null)
                    {
                        subscriptionHeaderDTO.SeasonStartDate = SiteContainerList.FromSiteDateTime(subscriptionHeaderDTO.SiteId, (DateTime)subscriptionHeaderDTO.SeasonStartDate);
                    }
                    if (subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList != null && subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList.Any())
                    {
                        for (int j = 0; j < subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList.Count; j++)
                        {
                            subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList[j].BillFromDate = SiteContainerList.FromSiteDateTime(subscriptionHeaderDTO.SiteId, subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList[j].BillFromDate);
                            subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList[j].BillToDate = SiteContainerList.FromSiteDateTime(subscriptionHeaderDTO.SiteId, subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList[j].BillToDate);
                            subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList[j].BillOnDate = SiteContainerList.FromSiteDateTime(subscriptionHeaderDTO.SiteId, subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList[j].BillOnDate);
                            subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList[j].AcceptChanges();
                        }
                    }
                    subscriptionHeaderDTO.AcceptChanges();
                }
            }
            log.LogMethodExit(subscriptionHeaderDTO);
        }
        private void SetToSiteTimeOffset()
        {
            log.LogMethodEntry(subscriptionHeaderDTO);
            if (executionContext != null && executionContext.IsCorporate)
            {
                if (subscriptionHeaderDTO != null && (subscriptionHeaderDTO.IsChangedRecursive || subscriptionHeaderDTO.SubscriptionHeaderId == -1))
                {
                    int siteId = executionContext.GetSiteId();
                    log.Info(siteId);
                    subscriptionHeaderDTO.SubscriptionStartDate = SiteContainerList.ToSiteDateTime(siteId, subscriptionHeaderDTO.SubscriptionStartDate);
                    subscriptionHeaderDTO.SubscriptionEndDate = SiteContainerList.ToSiteDateTime(siteId, subscriptionHeaderDTO.SubscriptionEndDate);
                    if (subscriptionHeaderDTO.SeasonStartDate != null)
                    {
                        subscriptionHeaderDTO.SeasonStartDate = SiteContainerList.ToSiteDateTime(siteId, (DateTime)subscriptionHeaderDTO.SeasonStartDate);
                    }
                    if (subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList != null && subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList.Any())
                    {
                        for (int j = 0; j < subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList.Count; j++)
                        {
                            subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList[j].BillFromDate = SiteContainerList.ToSiteDateTime(siteId, subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList[j].BillFromDate);
                            subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList[j].BillToDate = SiteContainerList.ToSiteDateTime(siteId, subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList[j].BillToDate);
                            subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList[j].BillOnDate = SiteContainerList.ToSiteDateTime(siteId, subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList[j].BillOnDate);
                        }
                    }
                }
            }
            log.LogMethodExit(subscriptionHeaderDTO);
        }

        internal bool AccountHoldsActiveSubscriptionEntitlements(AccountDTO accountDTO, SqlTransaction inSQLTrx)
        {
            log.LogMethodEntry((accountDTO != null ? accountDTO.AccountId : -1), inSQLTrx);
            bool accountHoldsSubscriptionEntitlements = false;
            if (accountDTO != null)
            {
                DateTime serverTime = serverDateTime.GetServerDateTime();
                //Subscription is active then only proceed.
                if (subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList != null &&
                    subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList.Exists(sbs => sbs.Status != SubscriptionStatus.CANCELLED && sbs.Status != SubscriptionStatus.EXPIRED
                                                                                           && sbs.BillFromDate >= serverTime && sbs.IsActive))
                {
                    List<int> accountBillIdList = GetSubscriptionBillCycleIdFromAccount(accountDTO);
                    if (accountBillIdList != null && accountBillIdList.Any())
                    {
                        try
                        {
                            for (int i = 0; i < subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList.Count; i++)
                            {
                                SubscriptionBillingScheduleDTO billDTO = subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList[i];
                                if (billDTO.IsActive && billDTO.Status != SubscriptionStatus.CANCELLED && billDTO.Status != SubscriptionStatus.EXPIRED && billDTO.BillFromDate >= serverTime.Date)
                                {
                                    if (accountBillIdList.Exists(asbsId => asbsId == billDTO.SubscriptionBillingScheduleId) == false)
                                    {
                                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 4003));
                                        // "account is not linked with all active bill cycles for the transaction"
                                    }
                                }
                            }
                            //No error faced
                            accountHoldsSubscriptionEntitlements = true;
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                        }
                    }
                }
            }
            log.LogMethodExit(accountHoldsSubscriptionEntitlements);
            return accountHoldsSubscriptionEntitlements;
        }

        private List<int> GetSubscriptionBillCycleIdFromAccount(AccountDTO accountDTO)
        {
            log.LogMethodEntry((accountDTO != null ? accountDTO.AccountId : -1));
            List<int> subsriptionBillingCycleIdList = new List<int>();
            if (accountDTO.AccountCreditPlusDTOList != null && accountDTO.AccountCreditPlusDTOList.Any()
                                    && accountDTO.AccountCreditPlusDTOList.Exists(cp => cp.SubscriptionBillingScheduleId > -1))
            {
                List<int> tempIdList = accountDTO.AccountCreditPlusDTOList.Where(cp => cp.SubscriptionBillingScheduleId > -1).Select(cp => cp.SubscriptionBillingScheduleId).ToList();
                if (tempIdList != null && tempIdList.Any())
                {
                    tempIdList = tempIdList.Distinct().ToList();
                    subsriptionBillingCycleIdList.AddRange(tempIdList);
                }
            }
            if (accountDTO.AccountGameDTOList != null && accountDTO.AccountGameDTOList.Any()
                && accountDTO.AccountGameDTOList.Exists(cg => cg.SubscriptionBillingScheduleId > -1))
            {
                List<int> tempIdList = accountDTO.AccountGameDTOList.Where(cg => cg.SubscriptionBillingScheduleId > -1).Select(cg => cg.SubscriptionBillingScheduleId).ToList();
                if (tempIdList != null && tempIdList.Any())
                {
                    tempIdList = tempIdList.Distinct().ToList();
                    subsriptionBillingCycleIdList.AddRange(tempIdList);
                }
            }
            if (accountDTO.AccountDiscountDTOList != null && accountDTO.AccountDiscountDTOList.Any()
               && accountDTO.AccountDiscountDTOList.Exists(cd => cd.SubscriptionBillingScheduleId > -1))
            {
                List<int> tempIdList = accountDTO.AccountDiscountDTOList.Where(cd => cd.SubscriptionBillingScheduleId > -1).Select(cd => cd.SubscriptionBillingScheduleId).ToList();
                if (tempIdList != null && tempIdList.Any())
                {
                    tempIdList = tempIdList.Distinct().ToList();
                    subsriptionBillingCycleIdList.AddRange(tempIdList);
                }
            }
            if (subsriptionBillingCycleIdList != null && subsriptionBillingCycleIdList.Any())
            {
                subsriptionBillingCycleIdList = subsriptionBillingCycleIdList.Distinct().ToList();
            }
            log.LogMethodExit(subsriptionBillingCycleIdList);
            return subsriptionBillingCycleIdList;
        }

        private string GetNextSubscriptipnNumber(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            string nextSequenceNumber = string.Empty;
            SequencesListBL sequencesListBL = new SequencesListBL(executionContext);
            List<KeyValuePair<SequencesDTO.SearchByParameters, string>> searchBySeqParameters = new List<KeyValuePair<SequencesDTO.SearchByParameters, string>>();
            searchBySeqParameters.Add(new KeyValuePair<SequencesDTO.SearchByParameters, string>(SequencesDTO.SearchByParameters.SEQUENCE_NAME, SUBSCRIPTIONORDER));
            searchBySeqParameters.Add(new KeyValuePair<SequencesDTO.SearchByParameters, string>(SequencesDTO.SearchByParameters.SITE_ID, executionContext.SiteId.ToString()));
            List<SequencesDTO> sequencesDTOList = sequencesListBL.GetAllSequencesList(searchBySeqParameters);
            SequencesDTO sequencesDTO = null;
            if (sequencesDTOList != null && sequencesDTOList.Any())
            {
                sequencesDTO = sequencesDTOList[0];
            }
            SequencesBL sequencesBL = new SequencesBL(executionContext, sequencesDTO);
            if (sequencesDTO != null)
            {
                nextSequenceNumber = sequencesBL.GetNextSequenceNo(sqlTransaction);
            }
            log.LogMethodExit(nextSequenceNumber);
            return nextSequenceNumber;
        }
    }

        /// <summary>
        /// Subscription Header List BL class
        /// </summary>
        public class SubscriptionHeaderListBL
        {
            private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            private List<SubscriptionHeaderDTO> subscriptionHeaderDTOList;
            private ExecutionContext executionContext;

            /// <summary>
            /// Parameterized Constructor with ExecutionContext
            /// </summary>
            /// <param name="executionContext"></param>
            public SubscriptionHeaderListBL(ExecutionContext executionContext)
            {
                log.LogMethodEntry(executionContext);
                this.executionContext = executionContext;
                this.subscriptionHeaderDTOList = null;
                log.LogMethodExit();
            }

            /// <summary>
            /// Parameterized constructor
            /// </summary>
            /// <param name="executionContext"></param>
            /// <param name="subscriptionHeaderDTOList"></param>
            public SubscriptionHeaderListBL(ExecutionContext executionContext, List<SubscriptionHeaderDTO> subscriptionHeaderDTOList)
            {
                log.LogMethodEntry(executionContext, subscriptionHeaderDTOList);
                this.executionContext = executionContext;
                this.subscriptionHeaderDTOList = subscriptionHeaderDTOList;
                log.LogMethodExit();
            }

            /// <summary>
            /// Returns the SubscriptionHeader list
            /// </summary>
            public List<SubscriptionHeaderDTO> GetSubscriptionHeaderDTOList(List<KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string>> searchParameters, Utilities utilities, bool loadChildren = false, SqlTransaction sqlTransaction = null)//modified
            {
                log.LogMethodEntry(searchParameters, loadChildren, sqlTransaction);
                SubscriptionHeaderDataHandler subscriptionHeaderDataHandler = new SubscriptionHeaderDataHandler(sqlTransaction);
                this.subscriptionHeaderDTOList = subscriptionHeaderDataHandler.GetSubscriptionHeaderDTOList(searchParameters);
                if (subscriptionHeaderDTOList != null && subscriptionHeaderDTOList.Any())
                {
                    foreach (KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string> searchParameter in searchParameters)
                    {
                        if (searchParameter.Key == SubscriptionHeaderDTO.SearchByParameters.HAS_EXPIRED_CREDIT_CARD)
                        {
                            FilterRecordsOnHasExpiredCreditCardSearchKey(utilities, searchParameter.Value, sqlTransaction);
                        }
                    }
                }

                if (loadChildren)
                {
                    LoadsubscriptionHeaderDTOChildren(sqlTransaction);
                }
                //Need to have child loaded
                foreach (KeyValuePair<SubscriptionHeaderDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (searchParameter.Key == SubscriptionHeaderDTO.SearchByParameters.CREDIT_CARD_EXPIRES_BEFORE_NEXT_BILLING)
                    {
                        FilterRecordsOnCardExpiresBeforeNextBillingSearchKey(utilities, searchParameter.Value, sqlTransaction);
                    }
                }
                subscriptionHeaderDTOList = SetFromSiteTimeOffset(subscriptionHeaderDTOList);
                log.LogMethodExit(subscriptionHeaderDTOList);
                return subscriptionHeaderDTOList;
            }

            private void FilterRecordsOnHasExpiredCreditCardSearchKey(Utilities utilities, string searchKeyValue, SqlTransaction sqlTransaction)
            {
                log.LogMethodEntry(searchKeyValue, sqlTransaction);
                //get card id list
                if (subscriptionHeaderDTOList != null && subscriptionHeaderDTOList.Any())
                {
                    List<int> creditCardsIdList = subscriptionHeaderDTOList.Select(sh => sh.CustomerCreditCardsId).Distinct().ToList();
                    CustomerCreditCardsListBL customerCreditCardsListBL = new CustomerCreditCardsListBL(executionContext);
                    List<CustomerCreditCardsDTO> customerCreditCardsDTOList = customerCreditCardsListBL.GetCustomerCreditCardsDTOList(creditCardsIdList, sqlTransaction);
                    List<KeyValuePair<int, bool>> cardsExpiryStatusList = new List<KeyValuePair<int, bool>>();
                    //Build keyValue pair of id and expirystatus
                    List<int> eligibleCreditCards = new List<int>();
                    if (customerCreditCardsDTOList != null && customerCreditCardsDTOList.Any())
                    {
                        for (int i = 0; i < customerCreditCardsDTOList.Count; i++)
                        {
                            CustomerCreditCardsBL customerCreditCardsBL = new CustomerCreditCardsBL(executionContext, customerCreditCardsDTOList[i]);
                            cardsExpiryStatusList.Add(new KeyValuePair<int, bool>(customerCreditCardsDTOList[i].CustomerCreditCardsId, customerCreditCardsBL.CustomerCreditCardHasExpired(utilities)));
                        }
                    }
                    //based on search value fetch active or expired card ids
                    eligibleCreditCards = cardsExpiryStatusList.Where(ccKey => ccKey.Value == (searchKeyValue == "1")).Select(ccKey => ccKey.Key).ToList();
                    //filter records based on selected card ids only
                    this.subscriptionHeaderDTOList = this.subscriptionHeaderDTOList.Where(sh => eligibleCreditCards.Exists(ccId => ccId == sh.CustomerCreditCardsId)).ToList();
                }
                log.LogMethodExit();
            }
            private void FilterRecordsOnCardExpiresBeforeNextBillingSearchKey(Utilities utilities, string searchKeyValue, SqlTransaction sqlTransaction)
            {
                log.LogMethodEntry(searchKeyValue, sqlTransaction);
                //get card id list
                if (subscriptionHeaderDTOList != null && subscriptionHeaderDTOList.Any())
                {
                    List<int> creditCardsIdList = subscriptionHeaderDTOList.Select(sh => sh.CustomerCreditCardsId).Distinct().ToList();
                    CustomerCreditCardsListBL customerCreditCardsListBL = new CustomerCreditCardsListBL(executionContext);
                    List<CustomerCreditCardsDTO> customerCreditCardsDTOList = customerCreditCardsListBL.GetCustomerCreditCardsDTOList(creditCardsIdList, sqlTransaction);
                    List<KeyValuePair<int, bool>> cardsExpiryStatusList = new List<KeyValuePair<int, bool>>();
                    for (int i = 0; i < this.subscriptionHeaderDTOList.Count; i++)
                    {
                        SubscriptionHeaderDTO sDTO = subscriptionHeaderDTOList[i];
                        if (sDTO.SubscriptionBillingScheduleDTOList != null && sDTO.SubscriptionBillingScheduleDTOList.Any())
                        {
                            CustomerCreditCardsDTO customerCreditCardsDTO = customerCreditCardsDTOList.Find(cc => cc.CustomerCreditCardsId == sDTO.CustomerCreditCardsId);
                            List<SubscriptionBillingScheduleDTO> unBilledCyclesDTOList = sDTO.SubscriptionBillingScheduleDTOList.Where(sbs => sbs.IsActive
                                                                                                                 && sbs.TransactionId == -1
                                                                                                                 && sbs.LineType == SubscriptionLineType.BILLING_LINE).ToList();
                            DateTime nextBillOnDate = DateTime.MaxValue;
                            if (unBilledCyclesDTOList != null && unBilledCyclesDTOList.Any())
                            {
                                nextBillOnDate = sDTO.SubscriptionBillingScheduleDTOList.Where(sbs => sbs.IsActive && sbs.TransactionId == -1
                                                                                                                 && sbs.LineType == SubscriptionLineType.BILLING_LINE).Min(sbs => sbs.BillOnDate);
                            }
                            SubscriptionBillingScheduleDTO nextBillingCycle = sDTO.SubscriptionBillingScheduleDTOList.Find(sbs => sbs.IsActive
                                                                                                                       && sbs.LineType == SubscriptionLineType.BILLING_LINE
                                                                                                                       && sbs.TransactionId == -1 && sbs.BillOnDate == nextBillOnDate);
                            if ((nextBillingCycle == null || nextBillOnDate == DateTime.MaxValue) && (searchKeyValue == "1" || searchKeyValue == "Y"))
                            {
                                //not a match and we are looking for expired records. hence remove
                                this.subscriptionHeaderDTOList.Remove(sDTO);
                                i = i - 1;
                            }
                            else
                            {
                                CustomerCreditCardsBL customerCreditCardsBL = new CustomerCreditCardsBL(executionContext, customerCreditCardsDTO);
                                bool expiresBefore = (nextBillingCycle == null ? false : customerCreditCardsBL.CustomerCreditCardExpiresbefore(utilities, nextBillingCycle.BillOnDate));
                                if ((searchKeyValue == "1" || searchKeyValue == "Y") && expiresBefore == false)
                                {
                                    //not a match hence remove
                                    this.subscriptionHeaderDTOList.Remove(sDTO);
                                    i = i - 1;
                                }
                                if ((searchKeyValue == "0" || searchKeyValue == "N") && expiresBefore == true)
                                {
                                    //not a match hence remove
                                    this.subscriptionHeaderDTOList.Remove(sDTO);
                                    i = i - 1;
                                }
                            }
                        }
                        else
                        {
                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2903, SubscriptionHeaderDTO.SearchByParameters.CREDIT_CARD_EXPIRES_BEFORE_NEXT_BILLING.ToString()));
                            //Can not search by &1 without loading chil records
                        }
                    }
                }
                log.LogMethodExit();
            }
            /// <summary>
            /// GetSubscriptionHeaderDTOList
            /// </summary>
            /// <param name="subscriptionHeaderIdList"></param>
            /// <param name="loadChildren"></param>
            /// <param name="sqlTransaction"></param>
            /// <returns></returns>
            public List<SubscriptionHeaderDTO> GetSubscriptionHeaderDTOList(List<int> subscriptionHeaderIdList, bool loadChildren = false, SqlTransaction sqlTransaction = null)
            {
                log.LogMethodEntry(subscriptionHeaderIdList, loadChildren, sqlTransaction);
                SubscriptionHeaderDataHandler subscriptionHeaderDataHandler = new SubscriptionHeaderDataHandler(sqlTransaction);
                this.subscriptionHeaderDTOList = subscriptionHeaderDataHandler.GetSubscriptionHeaderDTOList(subscriptionHeaderIdList);
                if (loadChildren)
                {
                    LoadsubscriptionHeaderDTOChildren(sqlTransaction);
                }
                subscriptionHeaderDTOList = SetFromSiteTimeOffset(subscriptionHeaderDTOList);
                log.LogMethodExit(subscriptionHeaderDTOList);
                return subscriptionHeaderDTOList;
            }
            /// <summary>
            /// GetSubscriptionHeaderListByCreditCards
            /// </summary>
            /// <param name="customerCreditCardsIdList"></param>
            /// <param name="loadChildren"></param>
            /// <param name="sqlTransaction"></param>
            /// <returns></returns>
            public List<SubscriptionHeaderDTO> GetSubscriptionHeaderListByCreditCards(List<int> customerCreditCardsIdList, bool loadChildren = false, SqlTransaction sqlTransaction = null)
            {
                log.LogMethodEntry(customerCreditCardsIdList, loadChildren, sqlTransaction);
                SubscriptionHeaderDataHandler subscriptionHeaderDataHandler = new SubscriptionHeaderDataHandler(sqlTransaction);
                this.subscriptionHeaderDTOList = subscriptionHeaderDataHandler.GetSubscriptionHeaderListByCreditCards(customerCreditCardsIdList);
                if (loadChildren)
                {
                    LoadsubscriptionHeaderDTOChildren(sqlTransaction);
                }
                subscriptionHeaderDTOList = SetFromSiteTimeOffset(subscriptionHeaderDTOList);
                log.LogMethodExit(subscriptionHeaderDTOList);
                return subscriptionHeaderDTOList;
            }
            /// <summary>
            /// Validates and saves the subscriptionHeaderDTOList to the db
            /// </summary>
            /// <param name="sqlTransaction"></param>
            internal void Save(SqlTransaction sqlTransaction = null)
            {
                log.LogMethodEntry(sqlTransaction);
                if (subscriptionHeaderDTOList == null ||
                    subscriptionHeaderDTOList.Any() == false)
                {
                    log.LogMethodExit(null, "List is empty");
                    throw new Exception(MessageContainerList.GetMessage(executionContext, "Cant save empty list"));
                }
                for (int i = 0; i < subscriptionHeaderDTOList.Count; i++)
                {
                    SubscriptionHeaderDTO subscriptionHeaderDTO = subscriptionHeaderDTOList[i];
                    SubscriptionHeaderBL subscriptionHeaderBL = new SubscriptionHeaderBL(executionContext, subscriptionHeaderDTO);
                    subscriptionHeaderBL.Save(sqlTransaction);
                }
                log.LogMethodExit();
            }

            private void LoadsubscriptionHeaderDTOChildren(SqlTransaction sqlTransaction)
            {
                log.LogMethodEntry(sqlTransaction);
                if (this.subscriptionHeaderDTOList != null && this.subscriptionHeaderDTOList.Any())
                {
                    Dictionary<int, SubscriptionHeaderDTO> SubscriptionHeaderDTOSubscriptionHeaderIdMap = new Dictionary<int, SubscriptionHeaderDTO>();
                    List<int> subscriptionHeaderIdList = new List<int>();
                    for (int i = 0; i < subscriptionHeaderDTOList.Count; i++)
                    {
                        if (SubscriptionHeaderDTOSubscriptionHeaderIdMap.ContainsKey(subscriptionHeaderDTOList[i].SubscriptionHeaderId))
                        {
                            continue;
                        }
                        SubscriptionHeaderDTOSubscriptionHeaderIdMap.Add(subscriptionHeaderDTOList[i].SubscriptionHeaderId, subscriptionHeaderDTOList[i]);
                        subscriptionHeaderIdList.Add(subscriptionHeaderDTOList[i].SubscriptionHeaderId);
                    }
                    SubscriptionBillingScheduleListBL subscriptionBillingScheduleListBL = new SubscriptionBillingScheduleListBL(executionContext);
                    List<SubscriptionBillingScheduleDTO> subscriptionBillingScheduleDTOList = subscriptionBillingScheduleListBL.GetSubscriptionBillingScheduleDTOList(subscriptionHeaderIdList, sqlTransaction);
                    if (subscriptionBillingScheduleDTOList != null && subscriptionBillingScheduleDTOList.Any())
                    {
                        for (int i = 0; i < subscriptionBillingScheduleDTOList.Count; i++)
                        {
                            if (SubscriptionHeaderDTOSubscriptionHeaderIdMap.ContainsKey(subscriptionBillingScheduleDTOList[i].SubscriptionHeaderId) == false)
                            {
                                continue;
                            }
                            SubscriptionHeaderDTO subscriptionHeaderDTO = SubscriptionHeaderDTOSubscriptionHeaderIdMap[subscriptionBillingScheduleDTOList[i].SubscriptionHeaderId];

                            if (subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList == null)
                            {
                                subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList = new List<SubscriptionBillingScheduleDTO>();
                            }
                            subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList.Add(subscriptionBillingScheduleDTOList[i]);
                        }
                    }
                }
            }
            private List<SubscriptionHeaderDTO> SetFromSiteTimeOffset(List<SubscriptionHeaderDTO> subscriptionHeaderDTOList)
            {
                log.LogMethodEntry(subscriptionHeaderDTOList);
                if (executionContext != null && executionContext.IsCorporate)
                {
                    if (subscriptionHeaderDTOList != null && subscriptionHeaderDTOList.Any())
                    {
                        for (int i = 0; i < subscriptionHeaderDTOList.Count; i++)
                        {
                            subscriptionHeaderDTOList[i].SubscriptionStartDate = SiteContainerList.FromSiteDateTime(subscriptionHeaderDTOList[i].SiteId, subscriptionHeaderDTOList[i].SubscriptionStartDate);
                            subscriptionHeaderDTOList[i].SubscriptionEndDate = SiteContainerList.FromSiteDateTime(subscriptionHeaderDTOList[i].SiteId, subscriptionHeaderDTOList[i].SubscriptionEndDate);
                            if (subscriptionHeaderDTOList[i].SeasonStartDate != null)
                            {
                                subscriptionHeaderDTOList[i].SeasonStartDate = SiteContainerList.FromSiteDateTime(subscriptionHeaderDTOList[i].SiteId, (DateTime)subscriptionHeaderDTOList[i].SeasonStartDate);
                            }
                            if (subscriptionHeaderDTOList[i].SubscriptionBillingScheduleDTOList != null && subscriptionHeaderDTOList[i].SubscriptionBillingScheduleDTOList.Any())
                            {
                                for (int j = 0; j < subscriptionHeaderDTOList[i].SubscriptionBillingScheduleDTOList.Count; j++)
                                {
                                    subscriptionHeaderDTOList[i].SubscriptionBillingScheduleDTOList[j].BillFromDate = SiteContainerList.FromSiteDateTime(subscriptionHeaderDTOList[i].SiteId, subscriptionHeaderDTOList[i].SubscriptionBillingScheduleDTOList[j].BillFromDate);
                                    subscriptionHeaderDTOList[i].SubscriptionBillingScheduleDTOList[j].BillToDate = SiteContainerList.FromSiteDateTime(subscriptionHeaderDTOList[i].SiteId, subscriptionHeaderDTOList[i].SubscriptionBillingScheduleDTOList[j].BillToDate);
                                    subscriptionHeaderDTOList[i].SubscriptionBillingScheduleDTOList[j].BillOnDate = SiteContainerList.FromSiteDateTime(subscriptionHeaderDTOList[i].SiteId, subscriptionHeaderDTOList[i].SubscriptionBillingScheduleDTOList[j].BillOnDate);
                                    subscriptionHeaderDTOList[i].SubscriptionBillingScheduleDTOList[j].AcceptChanges();
                                }
                            }
                            subscriptionHeaderDTOList[i].AcceptChanges();
                        }
                    }
                }
                log.LogMethodExit(subscriptionHeaderDTOList);
                return subscriptionHeaderDTOList;
            }

        }
    }
