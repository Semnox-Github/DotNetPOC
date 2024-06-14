/********************************************************************************************
 * Project Name - Tags
 * Description  - Data Object of the NotificationTagStatus class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.90        21-jul-2020   Mushahid Faizan         Created.
 ********************************************************************************************/
using System;


namespace Semnox.Parafait.Tags
{
    public class NotificationTagStatusDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        /// 
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by NOTIFICATIONTAGSTATUSID
            /// </summary>
            NOTIFICATIONTAGSTATUSID,
            /// <summary>
            /// Search by NOTIFICATIONTAGID
            /// </summary>
            NOTIFICATIONTAGID,
            /// <summary>
            /// Search by Channel field
            /// </summary>
            CHANNEL,
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
            /// Search by Timestamp field
            /// </summary>
            TIMESTAMP
        }
        private int notificationTagStatusId;
        private int notificationTagId;
        private DateTime timestamp;
        private bool pingStatus;
        private Decimal batteryStatusPercentage;
        private string signalStrength;
        private string deviceStatus;
       // private string remarks;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private int siteId;
        private string channel;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default Constructor
        /// </summary>
        public NotificationTagStatusDTO()
        {
            log.LogMethodEntry();
            notificationTagStatusId = -1;
            notificationTagId = -1;
            masterEntityId = -1;
            pingStatus = true;
            isActive = true;
            siteId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with Required data fields
        /// </summary>
        public NotificationTagStatusDTO(int notificationTagStatusId, int notificationTagId, DateTime timestamp, bool pingStatus,
                                        Decimal batteryStatusPercentage, string signalStrength, string deviceStatus, string channel, bool isActive) : this()
        {
            log.LogMethodEntry(notificationTagStatusId, notificationTagId, timestamp, pingStatus, batteryStatusPercentage, signalStrength, deviceStatus, channel, isActive);

            this.notificationTagStatusId = notificationTagStatusId;
            this.notificationTagId = notificationTagId;
            this.timestamp = timestamp;
            this.pingStatus = pingStatus;
            this.batteryStatusPercentage = batteryStatusPercentage;
            this.signalStrength = signalStrength;
            this.deviceStatus = deviceStatus;
          //  this.remarks = remarks;
            this.isActive = isActive;
            this.channel = channel;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public NotificationTagStatusDTO(int notificationTagStatusId, int notificationTagId, DateTime timestamp, bool pingStatus,
                                        Decimal batteryStatusPercentage, string signalStrength, string deviceStatus, string channel, bool isActive, string createdBy,
                                       DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate,
                                       int siteId, int masterEntityId, bool synchStatus, string guid)
            : this(notificationTagStatusId, notificationTagId, timestamp, pingStatus, batteryStatusPercentage, signalStrength, deviceStatus, channel, isActive)
        {
            log.LogMethodEntry(notificationTagStatusId, notificationTagId, timestamp, pingStatus, batteryStatusPercentage, signalStrength, deviceStatus, channel, isActive, createdBy, creationDate, lastUpdatedBy, lastUpdateDate, siteId,
                                         masterEntityId, synchStatus, guid);
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            this.siteId = siteId;
            this.masterEntityId = masterEntityId;
            this.synchStatus = synchStatus;
            this.guid = guid;
        }

        /// <summary>
        /// Get/Set method of the DeviceStatus field
        /// </summary>
        public string DeviceStatus { get { return deviceStatus; } set { deviceStatus = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SignalStrength field
        /// </summary>
        public string SignalStrength { get { return signalStrength; } set { signalStrength = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the NotificationTagId field
        /// </summary>
        public int NotificationTagId { get { return notificationTagId; } set { this.IsChanged = true; notificationTagId = value; } }

        /// <summary>
        /// Get/Set method of the NotificationTagStatusId field
        /// </summary>
        public int NotificationTagStatusId { get { return notificationTagStatusId; } set { this.IsChanged = true; notificationTagStatusId = value; } }

        /// <summary>
        /// Get method of the TimeStamp field
        /// </summary>
        public DateTime TimeStamp { get { return timestamp; } set { this.IsChanged = true; timestamp = value; } }

        /// <summary>
        /// Get method of the BatteryStatusPercentage field
        /// </summary>
        public Decimal BatteryStatusPercentage { get { return batteryStatusPercentage; } set { this.IsChanged = true; batteryStatusPercentage = value; } }

        /// <summary>
        /// Get/Set method of the PingStatus field
        /// </summary>
        public bool PingStatus { get { return pingStatus; } set { pingStatus = value; this.IsChanged = true; } }
        
        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the DefaultChannel field
        /// </summary>
        public string Channel { get { return channel; } set { channel = value; this.IsChanged = true; } }
        
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
                    return notifyingObjectIsChanged || notificationTagStatusId < 0;
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

    /// <summary>
    /// Signal Strength of Radian device
    /// </summary>
    public enum RadianDeviceStatusRSSI
    {
        UNKNOWN = 0,
        Excellent_20 = 1,
        Good_70 = 2,
        Fair_85 = 3,
        Weak_129 = 4
    }

    /// <summary>
    /// Device Status based on communication with Radian device
    /// </summary>
    public enum RadianDeviceStatus
    {
        NONE = 0,
        IDLE = 1,
        DEPLOY = 2,
        STORAGE = 4,
        EXPIRED = 5,
        CONN_LOST_DEPLOY = 7,
        CONN_LOST_IDLE = 8,
        RECONNECTION_IDLE = 9,
        RECONNECTION_DEPLOY = 10
    }
}
