/********************************************************************************************
 * Project Name - Tags
 * Description  - Data Object of the NotificationTag class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.90        20-jul-2020   Mushahid Faizan         Created.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;


namespace Semnox.Parafait.Tags
{
    public class NotificationTagsDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        /// 
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by NOTIFICATIONTAGID
            /// </summary>
            NOTIFICATIONTAGID,
            /// <summary>
            /// Search by IS_ACTIVE field
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by ISINSTORAGE field
            /// </summary>
            ISINSTORAGE,
            /// <summary>
            /// Search by Marked for Storage field
            /// </summary>
            MARKED_FOR_STORAGE,
            /// <summary>
            /// Search by Tag Number field
            /// </summary>
            TAGNUMBER,
            /// <summary>
            /// Search by TAGSTATUS field
            /// </summary>
            TAGNOTIFICATIONSTATUS,
            /// <summary>
            /// Search by TAGSTATUS List field
            /// </summary>
            TAG_NOTIFICATION_STATUS_LIST,
            /// <summary>
            /// Search by DEFAULT CHANNEL field
            /// </summary>
            DEFAULT_CHANNEL
        }

        private int notificationTagId;
        private string tagNumber;
        private string tagNotificationStatus;
        private bool markedForStorage;
        private DateTime lastStorageMarkedDate;
        private bool isInStorage;
        private bool isActive;
        private string remarks;
        private string defaultChannel;
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

        private List<NotificationTagStatusDTO> notificationTagStatusDTOList;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public NotificationTagsDTO()
        {
            log.LogMethodEntry();
            notificationTagId = -1;
            masterEntityId = -1;
            isActive = true;
            siteId = -1;
            notificationTagStatusDTOList = new List<NotificationTagStatusDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="notificationTagPatternId"></param>
        /// <param name="notificationTagPatternName"></param>
        /// <param name="patternDurationInSeconds"></param>
        /// <param name="ledPatternNumber"></param>
        /// <param name="buzzerPatternNumber"></param>
        /// <param name="isActive"></param>
        public NotificationTagsDTO(int notificationTagId, string tagNumber, string tagNotificationStatus, bool markedForStorage, DateTime lastStorageMarkedDate, bool isInStorage, string remarks, string defaultChannel, bool isActive)
        {
            log.LogMethodEntry(notificationTagId, tagNumber, tagNotificationStatus, markedForStorage, lastStorageMarkedDate, isInStorage, remarks, defaultChannel, isActive);

            this.notificationTagId = notificationTagId;
            this.tagNumber = tagNumber;
            this.tagNotificationStatus = tagNotificationStatus;
            this.markedForStorage = markedForStorage;
            this.lastStorageMarkedDate = lastStorageMarkedDate;
            this.isInStorage = isInStorage;
            this.remarks = remarks;
            this.defaultChannel = defaultChannel;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        public NotificationTagsDTO(int notificationTagId, string tagNumber, string tagNotificationStatus, bool markedForStorage, DateTime lastStorageMarkedDate, bool isInStorage, string remarks, string defaultChannel,
            bool isActive, string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate, int siteId, int masterEntityId, bool synchStatus, string guid)
            : this(notificationTagId, tagNumber, tagNotificationStatus, markedForStorage, lastStorageMarkedDate, isInStorage,  remarks,  defaultChannel, isActive)
        {
            log.LogMethodEntry(notificationTagId, tagNumber, tagNotificationStatus, markedForStorage, lastStorageMarkedDate, isInStorage, remarks, defaultChannel, isActive,
                                createdBy, creationDate, lastUpdatedBy, lastUpdateDate, siteId, masterEntityId, synchStatus, guid);

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
        /// Get/Set method of the NotificationTagId field
        /// </summary>
        public int NotificationTagId { get { return notificationTagId; } set { this.IsChanged = true; notificationTagId = value; } }

        /// <summary>
        /// Get/Set method of the LastStorageMarkedDate field
        /// </summary>
        public DateTime LastStorageMarkedDate { get { return lastStorageMarkedDate; } set { this.IsChanged = true; lastStorageMarkedDate = value; } }


        /// <summary>
        /// Get/Set method of the TagNumber field
        /// </summary>
        public string TagNumber { get { return tagNumber; } set { tagNumber = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the TagNotificationStatus field
        /// </summary>
        public string TagNotificationStatus { get { return tagNotificationStatus; } set { tagNotificationStatus = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the MarkedForStorage field
        /// </summary>
        public bool MarkedForStorage { get { return markedForStorage; } set { markedForStorage = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the IsInStorage field
        /// </summary>
        public bool IsInStorage { get { return isInStorage; } set { isInStorage = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Remarks field
        /// </summary>
        public string Remarks { get { return remarks; } set { remarks = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the DefaultChannel field
        /// </summary>
        public string DefaultChannel { get { return defaultChannel; } set { defaultChannel = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }

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
        /// Get/Set method of the NotificationTagIssuedDTOList field
        /// </summary>
        public List<NotificationTagStatusDTO> NotificationTagStatusDTOList
        {
            get { return notificationTagStatusDTOList; }
            set { notificationTagStatusDTOList = value; }
        }


        /// <summary>
        /// Returns whether the NotificationTagsDTO changed or any of its NotificationTagStatusDTOList  are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (notificationTagStatusDTOList != null &&
                    notificationTagStatusDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                return false;
            }
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
                    return notifyingObjectIsChanged || notificationTagId < 0;
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

    public enum TagNotificationStatus
    {
        /// <summary>
        /// In Use
        /// </summary>
        IN_USE,
        /// <summary>
        /// Not in Use
        /// </summary>
        NOT_IN_USE,
        /// <summary>
        /// Lost Tag
        /// </summary>
        LOST,
        /// <summary>
        /// Dormant - Not communicating
        /// </summary>
        DORMANT
    }
    /// <summary>
    /// Converts TagNotificationStatus from/to string
    /// </summary>
    public class TagNotificationStatusConverter
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Converts TagNotificationStatus from string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static TagNotificationStatus FromString(string value)
        {
            log.LogMethodEntry("value");
            switch (value.ToUpper())
            {
                case "NU":
                    {
                        return TagNotificationStatus.NOT_IN_USE;
                    }
                case "U":
                    {
                        return TagNotificationStatus.IN_USE;
                    }
                case "L":
                    {
                        return TagNotificationStatus.LOST;
                    }
                case "D":
                    {
                        return TagNotificationStatus.DORMANT;
                    }
                default:
                    {
                        log.Error("Error :Not a valid Tag Notification Status type ");
                        log.LogMethodExit(null, "Throwing Exception");
                        throw new ArgumentException("Not a valid Tag Notification Status type");
                    }
            }
        }
        /// <summary>
        /// Converts TagNotificationStatus to string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToString(TagNotificationStatus value)
        {
            log.LogMethodEntry("value");
            switch (value)
            {
                case TagNotificationStatus.NOT_IN_USE:
                    {
                        return "NU";
                    }
                case TagNotificationStatus.IN_USE:
                    {
                        return "U";
                    }
                case TagNotificationStatus.LOST:
                    {
                        return "L";
                    }
                case TagNotificationStatus.DORMANT:
                    {
                        return "D";
                    }
                default:
                    {
                        log.Error("Error :Not a valid Tag Notification Status type ");
                        log.LogMethodExit(null, "Throwing Exception");
                        throw new ArgumentException("Not a valid Tag Notification Status");
                    }
            }
        }
    }
}
