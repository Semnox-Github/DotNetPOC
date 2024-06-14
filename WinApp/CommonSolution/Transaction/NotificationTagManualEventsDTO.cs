/********************************************************************************************
* Project Name - NotificationTagManualEventsDTO
* Description - DTO for NotificationTagManualEvents 
*
**************
**Version Log 
**************
*Version    Date        Modified By     Remarks
*********************************************************************************************
*2.110.0    07-Jan-2021  Fiona          Created 
*********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Transaction
{
    public class NotificationTagManualEventsDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public enum SearchByParameters
        {
            /// <summary>
            /// Search by NOTIFICATION_TAG_EVENT_ID
            /// </summary>
            NOTIFICATION_TAG_EVENT_ID,
            /// <summary>
            /// Search by NOTIFICATION_TAG_ID
            /// </summary>
            NOTIFICATION_TAG_ID,
            /// <summary>
            /// Search by IsActive field
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search by site_id field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by Master Entity Id field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by Guid field
            /// </summary>
            GUID,
            /// <summary>
            /// Search by Processing Status field
            /// </summary>
            PROCESSING_STATUS,
            /// <summary>
            /// Search by Timestamp field
            /// </summary>
            TIMESTAMP
        }
        private int notificationTagMEventId;
        private int notificationTagId;
        private string command;
        private int notificationTagProfileId;
        private DateTime? lastSessionAlertTime;
        private DateTime? lastAlertTimeBeforeExpiry;
        private DateTime? lastAlertTimeOnExpiry;
        private DateTime? timestamp;
        private string processingStatus;
        private DateTime? processDate;
        private string remarks;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private int siteId;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default Constructor
        /// </summary>
        public NotificationTagManualEventsDTO()
        {
            this.notificationTagMEventId = -1;
            this.notificationTagId = -1;
            this.isActive = true;
            this.siteId = -1;
            this.masterEntityId = -1;
        }
        /// <summary>
        /// Constructor with Required data fields
        /// </summary>
        /// <param name="notificationTagMEventId"></param>
        /// <param name="notificationTagId"></param>
        /// <param name="command"></param>
        /// <param name="notificationTagProfileId"></param>
        /// <param name="lastSessionAlertTime"></param>
        /// <param name="lastAlertTimeBeforeExpiry"></param>
        /// <param name="lastAlertTimeOnExpiry"></param>
        /// <param name="timestamp"></param>
        /// <param name="processingStatus"></param>
        /// <param name="processDate"></param>
        /// <param name="isActive"></param>
        public NotificationTagManualEventsDTO(int notificationTagMEventId, int notificationTagId, string command, int notificationTagProfileId, DateTime? lastSessionAlertTime, DateTime? lastAlertTimeBeforeExpiry, DateTime? lastAlertTimeOnExpiry, DateTime? timestamp, string processingStatus, DateTime? processDate, bool isActive, string remarks)
            :this()
        {
            log.LogMethodEntry(notificationTagMEventId, notificationTagId, command, notificationTagProfileId, lastSessionAlertTime, lastAlertTimeBeforeExpiry, lastAlertTimeOnExpiry, timestamp, processingStatus, processDate, isActive, remarks);
            this.notificationTagMEventId = notificationTagMEventId;
            this.notificationTagId = notificationTagId;
            this.command = command;
            this.notificationTagProfileId = notificationTagProfileId;
            this.lastSessionAlertTime = lastSessionAlertTime;
            this.lastAlertTimeBeforeExpiry = lastAlertTimeBeforeExpiry;
            this.lastAlertTimeOnExpiry = lastAlertTimeOnExpiry;
            this.timestamp = timestamp;
            this.processingStatus = processingStatus;
            this.processDate = processDate;
            this.isActive = isActive;
            this.remarks = remarks;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
       
        public NotificationTagManualEventsDTO(int notificationTagMEventId, int notificationTagId, string command, int notificationTagProfileId, DateTime? lastSessionAlertTime, DateTime? lastAlertTimeBeforeExpiry, DateTime? lastAlertTimeOnExpiry, DateTime? timestamp, string processingStatus, DateTime? processDate, bool isActive, string remarks, string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate, int siteId, string guid, bool synchStatus, int masterEntityId) : 
            this(notificationTagMEventId, notificationTagId, command, notificationTagProfileId, lastSessionAlertTime, lastAlertTimeBeforeExpiry, lastAlertTimeOnExpiry, timestamp, processingStatus, processDate, isActive, remarks)
        {
            log.LogMethodEntry(notificationTagMEventId, notificationTagId, command, notificationTagProfileId, lastSessionAlertTime,  lastAlertTimeBeforeExpiry,  lastAlertTimeOnExpiry, timestamp,  processingStatus,  processDate,  isActive, remarks, createdBy,  creationDate,  lastUpdatedBy,  lastUpdateDate,  siteId,  guid,  synchStatus,  masterEntityId);
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            this.siteId = siteId;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            log.LogMethodExit();
        }



        /// <summary>
        /// Get/Set method of the NotificationTagMEventId field
        /// </summary>
        public int NotificationTagMEventId
        {
            get { return notificationTagMEventId; }
            set { notificationTagMEventId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the NotificationTagId field
        /// </summary>
        public int NotificationTagId
        {
            get { return notificationTagId; }
            set { notificationTagId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the Command field
        /// </summary>
        public string Command
        {
            get { return command; }
            set { command = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the NotificationTagProfileId field
        /// </summary>
        public int NotificationTagProfileId
        {
            get { return notificationTagProfileId; }
            set { notificationTagProfileId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the LastSessionAlertTime field
        /// </summary>
        public DateTime? LastSessionAlertTime
        {
            get { return lastSessionAlertTime; }
            set { lastSessionAlertTime = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the LastAlertTimeBeforeExpiry field
        /// </summary>
        public DateTime? LastAlertTimeBeforeExpiry
        {
            get { return lastAlertTimeBeforeExpiry; }
            set { lastAlertTimeBeforeExpiry = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the LastAlertTimeOnExpiry field
        /// </summary>
        public DateTime? LastAlertTimeOnExpiry
        {
            get { return lastAlertTimeOnExpiry; }
            set { lastAlertTimeOnExpiry = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the Timestamp field
        /// </summary>
        public DateTime? Timestamp
        {
            get { return timestamp; }
            set { timestamp = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the ProcessingStatus field
        /// </summary>
        public string ProcessingStatus
        {
            get { return processingStatus; }
            set { processingStatus = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the ProcessDate field
        /// </summary>
        public DateTime? ProcessDate
        {
            get { return processDate; }
            set { processDate = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the remarks field
        /// </summary>
        public string Remarks { get { return remarks; } set { remarks = value; } }


        /// <summary>
        /// Get method of the CreatedBy field
        /// </summary>
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }

        /// <summary>
        /// Get method of the CreationDate field
        /// </summary>
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }

        /// <summary>
        /// Get method of the LastUpdatedBy field
        /// </summary>
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }

        /// <summary>
        /// Get method of the LastUpdateDate field
        /// </summary>
        public DateTime LastUpdateDate { get { return lastUpdateDate; } set { lastUpdateDate = value; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        public int SiteId { get { return siteId; } set { siteId = value; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId { get { return masterEntityId; } set { this.IsChanged = true; masterEntityId = value; } }

        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || notificationTagMEventId < 0;
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
            this.IsChanged = false;
            log.LogMethodExit();
        }
    }    
}
