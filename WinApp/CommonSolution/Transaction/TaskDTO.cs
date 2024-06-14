/********************************************************************************************
 * Project Name - Transaction
 * Description  - Data object of Task
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70        22-May-2019   Girish Kundar           Created 
 *2.80.0      19-Mar-2020   Jinto Thomas            Modified constructor for adding new column trxid
 *2.130.0     07-Jul-2021   Prashanth               Handle ischanged
 *2.130.2     12-Dec-2021   Deeksha                 Added additional fields as part of Transfer Entitlement enhancements
 ********************************************************************************************/
using System;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// This is the Task data object class. This acts as data holder for the tasks business object
    /// </summary>
    public class TaskDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  TASK ID field
            /// </summary>
            TASK_ID,
            /// <summary>
            /// Search by  TASK ID LIST field
            /// </summary>
            TASK_ID_LIST,
            /// <summary>
            /// Search by CARD ID field
            /// </summary>
            CARD_ID,
            /// <summary>
            /// Search by CONSOLIDATED CARD ID 1 field
            /// </summary>
            CONSOLIDATED_CARD_ID1,
            /// <summary>
            /// Search by CONSOLIDATED CARD ID 2 field
            /// </summary>
            CONSOLIDATED_CARD_ID2,
            /// <summary>
            /// Search by CONSOLIDATED_CARD_ID 3 field
            /// </summary>
            CONSOLIDATED_CARD_ID3,
            /// <summary>
            /// Search by CONSOLIDATED CARD ID 4 field
            /// </summary>
            CONSOLIDATED_CARD_ID4,
            /// <summary>
            /// Search by CONSOLIDATED CARD ID 5 field
            /// </summary>
            CONSOLIDATED_CARD_ID5,
            /// <summary>
            /// Search by  CARD TYPE ID field
            /// </summary>
            TASK_TYPE_ID,
            /// <summary>
            /// Search by  TRANSFERRED TO  CARD ID field
            /// </summary>
            TRANSFERRED_TO_CARD_ID,
            /// <summary>
            /// Search by  USER ID field
            /// </summary>
            USER_ID,
            ///// <summary>
            ///// Search by  ACTIVE FLAG field
            ///// <summary>
            //ACTIVE_FLAG,
            /// <summary>
            /// Search by  POS MACHINE field
            /// </summary>
            POS_MACHINE,
            /// <summary>
            /// Search by TASK DATE field
            /// </summary>
            TASK_DATE,
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by TRX_ID field
            /// </summary>
            TRX_ID
        }
        private int taskId;
        private int cardId;
        private int taskTypeId;
        private decimal valueLoaded;
        private int transferToCardId;
        private decimal creditsExchanged;
        private decimal tokensExchanged;
        private int consolidateCard1;
        private int consolidateCard2;
        private int consolidateCard3;
        private int consolidateCard4;
        private int consolidateCard5;
        private DateTime taskdate;
        private int userId;
        private string posMachine;
        private string remarks;
        private int approvedBy;
        private int attribute1;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int attribute2;
        private decimal credits;
        private decimal courtesy;
        private decimal bonus;
        private decimal tickets;
        private int masterEntityId;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        //private bool activeFlag;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private int trxId;
        private decimal virtualPoints;
        private decimal counterItems;
        private decimal playCredits;
        private decimal time;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public TaskDTO()
        {
            log.LogMethodEntry();
            taskId = -1;
            cardId = -1;
            taskTypeId = -1;
            transferToCardId = -1;
            consolidateCard1 = -1;
            consolidateCard2 = -1;
            consolidateCard3 = -1;
            consolidateCard4 = -1;
            consolidateCard5 = -1;
            userId = -1;
            approvedBy = -1;
            siteId = -1;
            masterEntityId = -1;
            trxId = -1;
            attribute1 = -1;
            attribute2 = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with Required fields
        /// </summary>
        public TaskDTO(int taskId, int cardId, int taskTypeId, decimal valueLoaded, int transferToCardId, decimal creditsExchanged,
                       decimal tokensExchanged, int consolidateCard1, int consolidateCard2, int consolidateCard3, int consolidateCard4,
                       int consolidateCard5, DateTime taskdate, int userId, string posMachine, string remarks, int approvedBy,
                       int attribute1, int attribute2, decimal credits, decimal courtesy,
                       decimal bonus, decimal tickets, int trxId,decimal virtualPoints, decimal counterItems, decimal playCredits, decimal time)
            :this()
        {
            log.LogMethodEntry(taskId, cardId, taskTypeId, valueLoaded, transferToCardId, creditsExchanged, tokensExchanged,
                               consolidateCard1, consolidateCard2, consolidateCard3, consolidateCard4, consolidateCard5, taskdate,
                               userId, posMachine, remarks, approvedBy, attribute1, attribute2,
                               credits, courtesy, bonus, tickets, trxId, virtualPoints, counterItems, playCredits, time);
            this.taskId = taskId;
            this.cardId = cardId;
            this.taskTypeId = taskTypeId;
            this.valueLoaded = valueLoaded;
            this.transferToCardId = transferToCardId;
            this.creditsExchanged = creditsExchanged;
            this.tokensExchanged = tokensExchanged;
            this.consolidateCard1 = consolidateCard1;
            this.consolidateCard2 = consolidateCard2;
            this.consolidateCard3 = consolidateCard3;
            this.consolidateCard4 = consolidateCard4;
            this.consolidateCard5 = consolidateCard5;
            this.taskdate = taskdate;
            this.userId = userId;
            this.posMachine = posMachine;
            this.remarks = remarks;
            this.approvedBy = approvedBy;
            this.attribute1 = attribute1;
            this.attribute2 = attribute2;
            this.credits = credits;
            this.courtesy = courtesy;
            this.bonus = bonus;
            this.tickets = tickets;
            this.trxId = trxId;
            this.virtualPoints = virtualPoints;
            this.counterItems = counterItems;
            this.playCredits = playCredits;
            this.time = time;
            log.LogMethodExit();
        }


        /// <summary>
        /// Parameterized constructor with all the fields
        /// </summary>
        public TaskDTO(int taskId, int cardId, int taskTypeId, decimal valueLoaded, int transferToCardId, decimal creditsExchanged,
                       decimal tokensExchanged, int consolidateCard1, int consolidateCard2, int consolidateCard3, int consolidateCard4,
                       int consolidateCard5, DateTime taskdate, int userId, string posMachine, string remarks, int approvedBy,
                       int attribute1, string guid, int siteId, bool synchStatus, int attribute2, decimal credits, decimal courtesy,
                       decimal bonus, decimal tickets, int masterEntityId, string createdBy, DateTime creationDate,
                       string lastUpdatedBy, DateTime lastUpdatedDate, int trxId,decimal virtualPoints, decimal counterItems, decimal playCredits, decimal time)
            : this(taskId, cardId, taskTypeId, valueLoaded, transferToCardId, creditsExchanged, tokensExchanged,
                               consolidateCard1, consolidateCard2, consolidateCard3, consolidateCard4, consolidateCard5, taskdate,
                               userId, posMachine, remarks, approvedBy, attribute1, attribute2,
                               credits, courtesy, bonus, tickets, trxId, virtualPoints, counterItems, playCredits, time)
        {
            log.LogMethodEntry(taskId, cardId, taskTypeId, valueLoaded, transferToCardId, creditsExchanged, tokensExchanged,
                               consolidateCard1, consolidateCard2, consolidateCard3, consolidateCard4, consolidateCard5, taskdate,
                               userId, posMachine, remarks, approvedBy, attribute1, guid, siteId, synchStatus, attribute2,
                               credits, courtesy, bonus, tickets, masterEntityId, createdBy, creationDate, lastUpdatedBy, lastUpdatedDate, counterItems, playCredits, time);
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedDate = lastUpdatedDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.masterEntityId = masterEntityId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the TaskId field
        /// </summary>
        public int TaskId
        {
            get { return taskId; }
            set { taskId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the CardId field
        /// </summary>
        public int CardId
        {
            get { return cardId; }
            set { cardId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the Task Type Id field
        /// </summary>
        public int TaskTypeId
        {
            get { return taskTypeId; }
            set { taskTypeId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the ValueLoaded field
        /// </summary>
        public decimal ValueLoaded
        {
            get { return valueLoaded; }
            set { valueLoaded = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the TransferTo CardId field
        /// </summary>
        public int TransferToCardId
        {
            get { return transferToCardId; }
            set { transferToCardId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the TokensExchanged field
        /// </summary>
        public decimal TokensExchanged
        {
            get { return tokensExchanged; }
            set { tokensExchanged = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the CreditsExchanged field
        /// </summary>
        public decimal CreditsExchanged
        {
            get { return creditsExchanged; }
            set { creditsExchanged = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the Consolidate Card1 field
        /// </summary>
        public int ConsolidateCard1
        {
            get { return consolidateCard1; }
            set { consolidateCard1 = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the Consolidate Card2 field
        /// </summary>
        public int ConsolidateCard2
        {
            get { return consolidateCard2; }
            set { consolidateCard2 = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the Consolidate Card3 field
        /// </summary>
        public int ConsolidateCard3
        {
            get { return consolidateCard3; }
            set { consolidateCard3 = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the Consolidate Card4 field
        /// </summary>
        public int ConsolidateCard4
        {
            get { return consolidateCard4; }
            set { consolidateCard4 = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the Consolidate Card5 field
        /// </summary>
        public int ConsolidateCard5
        {
            get { return consolidateCard5; }
            set { consolidateCard5 = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the Task date field
        /// </summary>
        public DateTime Taskdate
        {
            get { return taskdate; }
            set { taskdate = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the UserId field
        /// </summary>
        public int UserId
        {
            get { return userId; }
            set { userId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the POSMachine field
        /// </summary>
        public string POSMachine
        {
            get { return posMachine; }
            set { posMachine = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the Remarks field
        /// </summary>
        public string Remarks
        {
            get { return remarks; }
            set { remarks = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the ApprovedBy field
        /// </summary>
        public int ApprovedBy
        {
            get { return approvedBy; }
            set { approvedBy = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the Attribute1 field
        /// </summary>
        public int Attribute1
        {
            get { return attribute1; }
            set { attribute1 = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        public int SiteId
        {
            get { return siteId; }
            set { siteId = value;  }
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
            set { synchStatus = value;  }
        }
        /// <summary>
        /// Get/Set method of the Attribute2 field
        /// </summary>
        public int Attribute2
        {
            get { return attribute2; }
            set { attribute2 = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the Credits field
        /// </summary>
        public decimal Credits
        {
            get { return credits; }
            set { credits = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the Courtesy field
        /// </summary>
        public decimal Courtesy
        {
            get { return courtesy; }
            set { courtesy = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the Bonus field
        /// </summary>
        public decimal Bonus
        {
            get { return bonus; }
            set { bonus = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the Tickets field
        /// </summary>
        public decimal Tickets
        {
            get { return tickets; }
            set { tickets = value; this.IsChanged = true; }
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
        public DateTime LastUpdatedDate
        {
            get { return lastUpdatedDate; }
            set { lastUpdatedDate = value;  }
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
            set { createdBy = value;  }
        }
        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        public DateTime CreationDate
        {
            get { return creationDate; }
            set { creationDate = value;  }
        }

        /// <summary>
        /// Get/Set method of the TrxId field
        /// </summary>
        public int TrxId
        {
            get { return trxId; }
            set { trxId = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the VirtualPoints field
        /// </summary>
        public decimal VirtualPoints
        {
            get { return virtualPoints; }
            set { virtualPoints = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the CounterItems field
        /// </summary>
        public decimal CounterItems
        {
            get { return counterItems; }
            set { counterItems = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the Time field
        /// </summary>
        public decimal Time
        {
            get { return time; }
            set { time = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the PlayCredits field
        /// </summary>
        public decimal PlayCredits
        {
            get { return playCredits; }
            set { playCredits = value; this.IsChanged = true; }
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
                    return notifyingObjectIsChanged || taskId < 0;
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
