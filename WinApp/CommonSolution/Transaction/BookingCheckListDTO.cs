/********************************************************************************************
 * Project Name - BookingCheckListDTO
 * Description  - Data object of BookingCheckListDetails
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70.2        12-Nov-2019   Jinto Thomas            Created for waiver phase2 
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.Transaction
{
    public class BookingCheckListDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public enum SearchByParameters
        {
            /// <summary>
            /// Search by   BOOKINGCHECKLIST ID field
            /// </summary>
            ID,
            /// <summary>
            /// Search by   BOOKING ID field
            /// </summary>
            BOOKING_ID,
            /// <summary>
            /// Search by   EVENT HOST USER ID field
            /// </summary>
            EVENT_HOST_USER_ID,
            /// <summary>
            /// Search by   CHECK LIST TASK GROUP ID field
            /// </summary>
            CHECKLIST_TASK_GROUP_USER_ID,
            /// <summary>
            /// Search by   IS ACTIVE field
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search by   SITE ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by   MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID,
        }
        private int bookingCheckListId;
        private int bookingId;
        private int eventHostUserId;
        private string hostName;
        private int checklistTaskGroupId;
        private string checkListTaskGroupName;
        private bool isActive;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default Constructor
        /// </summary>

        public BookingCheckListDTO()
        {
            log.LogMethodEntry();
            bookingCheckListId = -1;
            bookingId = -1;
            eventHostUserId = -1;
            checklistTaskGroupId = -1;
            siteId = -1;
            masterEntityId = -1;
            log.LogMethodExit();
        }
        /// <summary>
        ///  Parameterized constructor with Required fields
        /// </summary>
        
        public BookingCheckListDTO(int bookingCheckListId, int bookingId, int eventHostUserId, int checklistTaskGroupId, bool isActive) 
            : this()
        {
            log.LogMethodEntry(bookingCheckListId, bookingId, eventHostUserId, checklistTaskGroupId, isActive);
            this.bookingCheckListId = bookingCheckListId;
            this.bookingId = bookingId;
            this.eventHostUserId = eventHostUserId;
            this.checklistTaskGroupId = checklistTaskGroupId;
            this.isActive = isActive;
            log.LogMethodExit();
        }
        /// <summary>
        ///  Parameterized constructor with all the fields
        /// </summary>
        
        public BookingCheckListDTO(int bookingCheckListId, int bookingId, int eventHostUserId, int checklistTaskGroupId, bool isActive, string guid, int siteId,
                                     bool synchStatus, int masterEntityId, string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate,
                                     string hostName, string checkListTaskGroupName) :
            this(bookingCheckListId, bookingId, eventHostUserId, checklistTaskGroupId, isActive)
        {
            log.LogMethodEntry(bookingCheckListId, bookingId, eventHostUserId, checklistTaskGroupId, isActive, guid, SiteId, synchStatus, masterEntityId,
                createdBy, creationDate, lastUpdatedBy, lastUpdateDate, hostName, checkListTaskGroupName);
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            this.hostName = hostName;
            this.checkListTaskGroupName = checkListTaskGroupName;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the BookingCheckList field
        /// </summary>
        [DisplayName("Id")]

        public int BookingCheckListId
        {
            get { return bookingCheckListId; }
            set { bookingCheckListId = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the BookingId field
        /// </summary>
        [DisplayName("Booking Id")]
        public int BookingId
        {
            get { return bookingId; }
            set { bookingId = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the EventHostUserId field
        /// </summary>
        [DisplayName("Host Id")]
        public int EventHostUserId
        {
            get { return eventHostUserId; }
            set { eventHostUserId = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the HostName field
        /// </summary>
        [DisplayName("Host Name")]
        public string HostName
        {
            get { return hostName; }
            set { hostName = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the ChecklistTaskGroupId field
        /// </summary>
        [DisplayName("Check List Id")]
        public int ChecklistTaskGroupId
        {
            get { return checklistTaskGroupId; }
            set { checklistTaskGroupId = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the CheckListTaskGroupName field
        /// </summary>
        [DisplayName("Check List Name")]
        public string CheckListTaskGroupName
        {
            get { return checkListTaskGroupName; }
            set { checkListTaskGroupName = value; this.IsChanged = true; }
        }


        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("Active?")]
        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        [Browsable(false)]
        public string Guid
        {
            get { return guid; }
            set { guid = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("Site Id")]
        [Browsable(false)]
        public int SiteId
        {
            get { return siteId; }
            set { siteId = value; }
        }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("Synch Status")]
        [Browsable(false)]
        public bool SynchStatus
        {
            get { return synchStatus; }
            set { synchStatus = value; }
        }
        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("Master Entity Id")]
        [Browsable(false)]
        public int MasterEntityId
        {
            get { return masterEntityId; }
            set { masterEntityId = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        [DisplayName("Created By")]
        public string CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }

        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        [DisplayName("Creation Date")]
        public DateTime CreationDate
        {
            get { return creationDate; }
            set { creationDate = value; }
        }

        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("Last Update By")]
        public string LastUpdatedBy
        {
            get { return lastUpdatedBy; }
            set { lastUpdatedBy = value; }
        }

        /// <summary>
        /// Get/Set method of the LastUpdateDate field
        /// </summary>
        [DisplayName("Last Update Date")]
        public DateTime LastUpdateDate
        {
            get { return lastUpdateDate; }
            set { lastUpdateDate = value; }
        }
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
                    return notifyingObjectIsChanged || bookingCheckListId < 0;
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

        public void AcceptChanges()
        {
            log.LogMethodEntry();
            IsChanged = false;
            log.LogMethodExit();
        }
    }
}
