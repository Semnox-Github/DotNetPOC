/********************************************************************************************
 * Project Name - RedemptionUserLogsDTO
 * Description  - Data object of RedemptionUserLogs
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70.2        29-Jul-2019   Archana                 Created 
 ********************************************************************************************/

using System;
using System.ComponentModel;

namespace Semnox.Parafait.Redemption
{
    /// <summary>
    /// This is the RedemptionUserLogsDTO data object class. This acts as data holder for the RedemptionUserLogs business object
    /// </summary>
    public class RedemptionUserLogsDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by REDEMPTION LOG ID field
            /// </summary>
            REDEMPTION_LOG_ID,
            /// <summary>
            /// Search by REDEMPTION ID field
            /// </summary>
            REDEMPTION_ID,
            /// <summary>
            /// Search by  POS MACHINE ID field
            /// </summary>
            POS_MACHINE_ID,
            /// <summary>
            /// Search by  TICKET RECEIPT ID field
            /// </summary>
            TICKET_RECEIPT_ID,
            /// <summary>
            /// Search by CURRENCY ID field
            /// </summary>
            CURRENCY_ID,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID
        }
        /// <summary>
        /// Enum field for different redemption actions
        /// </summary>
        public enum RedemptionAction
        {
            ///<summary>
            ///OPEN
            ///</summary>
            [Description("Reprint Ticket")] REPRINT_TICKET,
            [Description("Load Ticket")] LOAD_TICKET,
            [Description("Load ticket to Card")] LOAD_TO_CARD,
            [Description("Load ticket to Physical Ticket")] LOAD_TO_PHYSICAL_TICKET,
            [Description("Redemption Creattion")] REDEMPTION,
            [Description("Consolidate tickets")] CONSOLIDATE_TICKET,
            [Description("Create Manual Ticket")] CREATE_MANUAL_TICKET,
            [Description("Abandoned")] REDEMPTION_ABANDONED,
            [Description("Prepared")] REDEMPTION_PREPARED,
            [Description("Delivered")] REDEMPTION_DELIVERED,
            [Description("Create Suspended redemption")] SUSPEND_REDEMPTION,
            [Description("Create Turn in redemption")] TURN_IN_REDEMPTION,
            [Description("Redemption Reversal")] REVERSAL_REDEMPTION
        };
        private int redemptionLogId;
        private int redemptionId;
        private int currencyId;
        private int ticketReceiptId;
        private string loginId;
        private DateTime activityDate;
        private int posMachineId;
        private string action;
        private string activity;
        private DateTime creationDate;
        private string createdBy;
        private DateTime lastUpdateDate;
        private string lastUpdatedBy;
        private int siteId;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        private string approverId;
        private DateTime? approvalTime;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        /// <summary>
        /// Default Constructor
        /// </summary>
        public RedemptionUserLogsDTO()
        {
            log.LogMethodEntry();
            redemptionLogId = -1;
            redemptionId = -1;
            currencyId = -1;
            ticketReceiptId = -1;
            posMachineId = -1;
            siteId = -1;
            masterEntityId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with Required fields
        /// </summary>
        public RedemptionUserLogsDTO(int redemptionLogId, int redemptionId, int currencyId, int ticketReceiptId, string loginId,
                          DateTime activityDate, int posMachineId, string action, string activity, string approverId, DateTime approvalTime)
            : this()
        {
            log.LogMethodEntry(redemptionLogId, redemptionId, currencyId, ticketReceiptId, loginId, activityDate, posMachineId,
                               action, activity, approverId, approvalTime);
            this.redemptionLogId = redemptionLogId;
            this.redemptionId = redemptionId;
            this.currencyId = currencyId;
            this.ticketReceiptId = ticketReceiptId;
            this.loginId = loginId;
            this.activityDate = activityDate;
            this.posMachineId = posMachineId;
            this.action = action;
            this.activity = activity;
            this.approverId = approverId;
            this.approvalTime = approvalTime;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with all the fields
        /// </summary>
        public RedemptionUserLogsDTO(int redemptionLogId, int redemptionId, int currencyId, int ticketReceiptId, string loginId,
                          DateTime activityDate, int posMachineId, string action, string activity, string createdBy, DateTime creationDate, string lastUpdatedBy,
                          DateTime lastUpdateDate, int siteId,string guid,  bool synchStatus, int masterEntityId, string approverId, DateTime approvalTime)
            : this(redemptionLogId, redemptionId, currencyId, ticketReceiptId, loginId,
                          activityDate, posMachineId, action, activity, approverId, approvalTime)
        {
            log.LogMethodEntry(redemptionLogId, redemptionId, currencyId, ticketReceiptId, loginId, activityDate, posMachineId,
                               action, activity, createdBy, creationDate, lastUpdatedBy, lastUpdateDate,
                               siteId, synchStatus, guid, synchStatus, masterEntityId, approverId, approvalTime);
            this.lastUpdateDate = lastUpdateDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the RedemptionLogId field
        /// </summary>
        public int RedemptionLogId
        {
            get { return redemptionLogId; }
            set { redemptionLogId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the RedemptionId field
        /// </summary>
        public int RedemptionId
        {
            get { return redemptionId; }
            set { redemptionId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the CurrencyId field
        /// </summary>
        public int CurrencyId
        {
            get { return currencyId; }
            set { currencyId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the TicketReceiptId field
        /// </summary>
        public int TicketReceiptId
        {
            get { return ticketReceiptId; }
            set { ticketReceiptId = value; }
        }
        /// <summary>
        /// Get/Set method of the LoginId field
        /// </summary>
        public string LoginId
        {
            get { return loginId; }
            set { loginId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the ActivityDate field
        /// </summary>
        public DateTime ActivityDate
        {
            get { return activityDate; }
            set { activityDate = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the PosMachineId field
        /// </summary>
        public int PosMachineId
        {
            get { return posMachineId; }
            set { posMachineId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the Action field
        /// </summary>
        public string Action
        {
            get { return action; }
            set { action = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the Activity field
        /// </summary>
        public string Activity
        {
            get { return activity; }
            set { activity = value; this.IsChanged = true; }
        }


        /// <summary>
        /// Get/Set method of the ApproverId field
        /// </summary>
        public string ApproverId
        {
            get { return approverId; }
            set { approverId = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the ApprovalTime field
        /// </summary>
        public DateTime? ApprovalTime
        {
            get { return approvalTime; }
            set { approvalTime = value; this.IsChanged = true; }
        }

        

        /// <summary>
        /// Get method of the LastUpdatedBy field
        /// </summary>
        public string LastUpdatedBy
        {
            get { return lastUpdatedBy; }
            set { lastUpdatedBy = value; }
        }
        /// <summary>
        ///  Get/Set method of the LastUpdateDate field
        /// </summary>
        public DateTime LastUpdateDate
        {
            get { return lastUpdateDate; }
            set { lastUpdateDate = value; }
        }
        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        public int SiteId
        {
            get { return siteId; }
            set { siteId = value; }
        }
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid
        {
            get { return guid; }
            set { guid = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        public bool SynchStatus
        {
            get { return synchStatus; }
            set { synchStatus = value; }
        }
        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId
        {
            get { return masterEntityId; }
            set { masterEntityId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        public string CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        public DateTime CreationDate
        {
            get { return creationDate; }
            set { creationDate = value; }
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
                    return notifyingObjectIsChanged || redemptionLogId <0 ;
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
        /// <summary>
        /// Allows to accept the changes
        /// </summary>
        public void AcceptChanges()
        {
            log.LogMethodEntry();
            IsChanged = false;
            log.LogMethodExit();
        }
    }
}
