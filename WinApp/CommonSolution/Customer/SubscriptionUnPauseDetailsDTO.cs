/********************************************************************************************
 * Project Name - SubscriptionUnPauseDetailsDTO DTO  
 * Description  - DTO class for SubscriptionUnPauseDetails
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************   
 *2.120.0     24-Feb-2021    Guru S A           Created for Subscription phase 2 changes                                                                               
 ********************************************************************************************/
using Semnox.Core.Utilities; 
using Semnox.Parafait.Languages;
using System; 

namespace Semnox.Parafait.Customer
{
    /// <summary>
    /// SubscriptionUnPauseDetailsDTO
    /// </summary>
    public class SubscriptionUnPauseDetailsDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private int subscriptionHeaderId;  
        private int subscriptionBillingScheduleId;
        private string unPauseAction;
        private DateTime oldBillFromDate;
        private DateTime? newBillFromDate; 
        private string unPauseApprovedBy; 
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        /// <summary>
        /// SubscriptionUnPauseDetailsDTO
        /// </summary>
        /// <param name="executionContext"></param>
        public SubscriptionUnPauseDetailsDTO(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            subscriptionHeaderId = -1;
            subscriptionBillingScheduleId = -1; 
            log.LogMethodExit();
        }
        /// <summary>
        /// SubscriptionHeaderDTO
        /// </summary> 
        public SubscriptionUnPauseDetailsDTO(ExecutionContext executionContext, int subscriptionHeaderId, int subscriptionBillingScheduleId, 
                                           DateTime oldBillFromDate, string unPauseAction, DateTime? newBillFromDate, string unPauseApprovedBy) : this(executionContext)
        {
            log.LogMethodEntry(executionContext, subscriptionHeaderId, subscriptionBillingScheduleId, oldBillFromDate, unPauseAction, newBillFromDate, unPauseApprovedBy);
            this.subscriptionHeaderId = subscriptionHeaderId;
            this.subscriptionBillingScheduleId = subscriptionBillingScheduleId;
            this.oldBillFromDate = oldBillFromDate;
            if (SubscriptionUnPauseOptions.ValidSubscriptionUnPauseOptions(unPauseAction) == false)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2967));
                //"Please set valid option for Unpause Action field"
            }
            this.unPauseAction = unPauseAction;
            this.newBillFromDate = newBillFromDate;
            this.unPauseApprovedBy = unPauseApprovedBy;
            IsChanged = true;
            log.LogMethodExit();
        } 
        /// <summary>
        /// Get/Set method of the SubscriptionHeaderId field
        /// </summary>
        public int SubscriptionHeaderId
        {
            get { return subscriptionHeaderId; } 
            set  { subscriptionHeaderId = value; IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the subscriptionBillingScheduleId field
        /// </summary>
        public int SubscriptionBillingScheduleId
        {
            get { return subscriptionBillingScheduleId; }
            set { subscriptionBillingScheduleId = value; IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the unPauseAction field
        /// </summary>
        public string UnPauseAction
        {
            get { return unPauseAction; }
            set { unPauseAction = value; IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the oldBillFromDate field
        /// </summary>
        public DateTime OldBillFromDate
        {
            get { return oldBillFromDate; }
            set { oldBillFromDate = value; IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the newBillFromDate field
        /// </summary>
        public DateTime? NewBillFromDate
        {
            get { return newBillFromDate; }
            set { newBillFromDate = value; IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the unPauseApprovedBy field
        /// </summary>
        public string UnPauseApprovedBy
        {
            get { return unPauseApprovedBy; }
            set { unPauseApprovedBy = value; IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged;
                }
            }

            set
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    if (!Boolean.Equals(notifyingObjectIsChanged, value))
                    {
                        notifyingObjectIsChanged = value;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Subscription UnPause Options
    /// </summary>
    public static class SubscriptionUnPauseOptions
    {
        // <summary>
        /// BILL status
        /// </summary>
        public const string BILL = "BILL";
        // <summary>
        /// CANCEL status
        /// </summary>
        public const string CANCEL = "CANCEL";
        // <summary>
        /// POSTPONE status
        /// </summary>
        public const string POSTPONE = "POSTPONE";
        /// <summary>
        /// ValidSubscriptionUnPauseOptions
        /// </summary>
        /// <param name="statusValue"></param>
        /// <returns></returns>
        public static bool ValidSubscriptionUnPauseOptions(string statusValue)
        {
            bool validValue = false;
            if (statusValue == BILL || statusValue == CANCEL || statusValue == POSTPONE)
            {
                validValue = true;
            }
            return validValue;
        } 
    }
}
