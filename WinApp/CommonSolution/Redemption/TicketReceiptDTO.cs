/********************************************************************************************
 * Project Name - TicketReceiptDTO
 * Description  - Data object
 * 
 **************
 **Version Log
 **************
 *Version     Date             	Modified By        Remarks          
 *********************************************************************************************
 *2.7.0       08-Jul-2019       Archana            Modified: Redemption Receipt changes to show ticket allocation details
 *2.70.2        19-Jul-2019       Deeksha            Modifications as per three tier standard.
 ********************************************************************************************/

using System;
using System.ComponentModel;

namespace Semnox.Parafait.Redemption
{
    /// <summary>
    /// This is the ticket receipt data object class. This acts as data holder for the ticket receipt business object
    /// </summary>
    public class TicketReceiptDTO
    {
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// SearchByTicketReceiptParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByTicketReceiptParameters
        {
            /// <summary>
            /// Search by ID field
            /// </summary>
            ID ,
            /// <summary>
            /// Search by REDEMPTION ID field
            /// </summary>
            REDEMPTION_ID ,
            /// <summary>
            /// Search by MANUAL TICKET RECEIPT NO field
            /// </summary>
            MANUAL_TICKET_RECEIPT_NO,
            /// <summary>
            /// Search by BALANCE TICKETS field
            /// </summary>
            BALANCE_TICKETS ,
            /// <summary>
            /// Search by BALANCE  TICKETS FROM field
            /// </summary>
            BALANCE_TICKETS_FROM ,
            /// <summary>
            /// Search by BALANCE TICKETS TO field
            /// </summary>
            BALANCE_TICKETS_TO ,
            /// <summary>
            /// Search by IS SUSPECTED field
            /// </summary>
            IS_SUSPECTED ,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID ,
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID ,
            /// <summary>
            /// Search by UPDATED FROM TIME field
            /// </summary>
            UPDATED_FROM_TIME,
            /// <summary>
            /// Search by UPDATED TO TIME field
            /// </summary>
            UPDATED_TO_TIME ,
            /// <summary>
            /// Search by UPDATED TO TIME field
            /// </summary>
            MANUAL_RECEIPT_IDS ,
            /// <summary>
            /// Search by SOURCE REDEMPTION ID field
            /// </summary>
            SOURCE_REDEMPTION_ID ,
            /// <summary>
            /// Search by ISSUE TO DATE field
            /// </summary>
            ISSUE_TO_DATE,
            /// <summary>
            /// Search by ISSUE FROM DATE field
            /// </summary>
            ISSUE_FROM_DATE,
            /// <summary>
            /// Search by SOURCE REDEMPTION ID LIST field
            /// </summary>
            SOURCE_REDEMPTION_ID_LIST,
            /// <summary>
            /// Search by MANUAL TICKET RECEIPT NO LIKE field
            /// </summary>
            MANUAL_TICKET_RECEIPT_NO_LIKE,

        }

        private int id;
        private int redemptionId;
        private string manualTicketReceiptNo;
        private int siteId;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        private int tickets;
        private int balanceTickets;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private bool isSuspected;
        private int sourceRedemptionId;
        private DateTime issueDate;
        private string createdBy;
        private DateTime creationDate;
        private int reprintCount;

        /// <summary>
        /// Default constructor
        /// </summary>
        public TicketReceiptDTO()
        {
            log.LogMethodEntry();
            id = -1;
            masterEntityId = -1;
            redemptionId = -1;
            siteId = -1;
            balanceTickets = 0;
            tickets = 0;
            reprintCount = 0;
            isSuspected = false;
            sourceRedemptionId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public TicketReceiptDTO(int id, int redemptionId, string manualTicketReceiptNo, int tickets, int balanceTickets, bool isSuspected,
                                int sourceRedemptionId, DateTime issueDate)
            :this()
        {
            log.LogMethodEntry(id, redemptionId, manualTicketReceiptNo,tickets, balanceTickets,isSuspected, sourceRedemptionId, issueDate);
            this.id = id;
            this.redemptionId = redemptionId;
            this.manualTicketReceiptNo = manualTicketReceiptNo;
            this.tickets = tickets;
            this.balanceTickets = balanceTickets;
            this.isSuspected = isSuspected;
            this.sourceRedemptionId = sourceRedemptionId;
            this.issueDate = issueDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public TicketReceiptDTO(int id, int redemptionId, string manualTicketReceiptNo, int siteId,
                                string guid, bool synchStatus, int masterEntityId, int tickets,
                                int balanceTickets, string lastUpdatedBy, DateTime lastUpdatedDate, bool isSuspected,
                                int sourceRedemptionId, DateTime issueDate,string createdBy,DateTime creationDate)
            :this(id, redemptionId, manualTicketReceiptNo, tickets, balanceTickets, isSuspected, sourceRedemptionId, issueDate)
        {
            log.LogMethodEntry(id, redemptionId, manualTicketReceiptNo, siteId, guid, synchStatus, masterEntityId, tickets, balanceTickets, lastUpdatedBy, lastUpdatedDate, isSuspected);
            this.sourceRedemptionId = sourceRedemptionId;
            this.issueDate = issueDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public TicketReceiptDTO(int id, int redemptionId, string manualTicketReceiptNo, int siteId,
                                string guid, bool synchStatus, int masterEntityId, int tickets,
                                int balanceTickets, string lastUpdatedBy, DateTime lastUpdatedDate, bool isSuspected,
                                int sourceRedemptionId, DateTime issueDate, string createdBy, DateTime creationDate, int reprintCount)
            : this(id, redemptionId, manualTicketReceiptNo, tickets, balanceTickets, isSuspected, sourceRedemptionId, issueDate)
        {
            log.LogMethodEntry(id, redemptionId, manualTicketReceiptNo, siteId, guid, synchStatus, masterEntityId, tickets, balanceTickets, lastUpdatedBy, lastUpdatedDate, isSuspected);
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.reprintCount = reprintCount;
            log.LogMethodExit();
        }
       
        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>
        [DisplayName("Id")]
        [ReadOnly(true)]
        public int Id { get { return id; } set { id = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Redemption Id field
        /// </summary>
        [DisplayName("RedemptionId")]
        [Browsable(false)]
        public int RedemptionId { get { return redemptionId; } set { redemptionId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Manual Ticket Receipt No field
        /// </summary>        
        [DisplayName("Ticket Receipt No")]
        public string ManualTicketReceiptNo { get { return manualTicketReceiptNo; } set { manualTicketReceiptNo = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the tickets field
        /// </summary>        
        [DisplayName("Tickets")]
        public int Tickets { get { return tickets; } set { tickets = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the BalanceTickets field
        /// </summary>        
        [DisplayName("Balance Tickets")]
        public int BalanceTickets { get { return balanceTickets; } set { balanceTickets = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the isSuspected field
        /// </summary>
        [DisplayName("IsSuspected?")]        
        public bool IsSuspected { get { return isSuspected; } set { isSuspected = value; this.IsChanged = true; } }        

        /// <summary>
        /// Get method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("Updated By")]
        //[Browsable(false)]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }

        /// <summary>
        /// Get method of the LastUpdatedDate field
        /// </summary>
        [DisplayName("Updated Date")]
        //[Browsable(false)]
        public DateTime LastUpdatedDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value; } }


        /// <summary>
        /// Get method of the CreatedBy field
        /// </summary>
        [DisplayName("CreatedBy")]
        //[Browsable(false)]
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }

        /// <summary>
        /// Get method of the CreationDate field
        /// </summary>
        [DisplayName("CreationDate")]
        //[Browsable(false)]
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        [Browsable(false)]
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("SiteId")]
        [Browsable(false)]
        public int SiteId { get { return siteId; } set { siteId = value;  } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("SynchStatus")]
        [Browsable(false)]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value;  } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>        
        [DisplayName("MasterEntityId")]
        [Browsable(false)]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SOurceRedemptionId Id field
        /// </summary>
        [DisplayName("SOurceRedemptionId")]
        [Browsable(false)]
        public int SOurceRedemptionId { get { return sourceRedemptionId; } set { sourceRedemptionId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the Issue Date field
        /// </summary>
        [DisplayName("Issue Date")]
        //[Browsable(false)]
        public DateTime IssueDate { get { return issueDate; } set { issueDate = value; } }

        /// <summary>
        /// Get/Set method of the Reprint count field
        /// </summary>
        [DisplayName("ReprintCount")]
        public int ReprintCount { get { return reprintCount; } set { reprintCount = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        [Browsable(false)]
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || id < 0;
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
        /// Allowes to accept the changes
        /// </summary>
        public void AcceptChanges()
        {
            log.LogMethodEntry();
            this.IsChanged = false;
            log.LogMethodExit();
        }
    }
}
