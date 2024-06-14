/********************************************************************************************
 * Project Name - Transaction
 * Description  - Data Object of the NotificationTagProfile class
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

namespace Semnox.Parafait.Transaction
{
    public class NotificationTagProfileDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        /// 
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by NOTIFICATIONTAGPROFILEID
            /// </summary>
            NOTIFICATIONTAGPROFILEID,
            /// <summary>
            /// Search by NOTIFICATIONTAGPROFILENAME
            /// </summary>
            NOTIFICATIONTAGPROFILENAME,
            /// <summary>
            /// Search by IS_ACTIVE field
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by GUID field
            /// </summary>
            GUID
        }

        private int notificationTagProfileId;
        private string notificationTagProfileName;
        private Decimal sessionAlertFrequencyInMinutes;
        private int sessionAlertNotificationPatternId;
        private Decimal alertTimeInMinsBeforeExpiry;
        private Decimal alertFrequencySecsBeforeExpiry;
        private int alertPatternIdBeforeExpiry;
        private int alertPatternIdOnExpiry;
        private Decimal alertFrequencySecsOnExpiry;
        private Decimal alertDurationMinsAfterExpiry;
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
        public NotificationTagProfileDTO()
        {
            log.LogMethodEntry();
            notificationTagProfileId = -1;
            sessionAlertNotificationPatternId = -1;
            masterEntityId = -1;
            isActive = true;
            siteId = -1;
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
        public NotificationTagProfileDTO(int notificationTagProfileId, string notificationTagProfileName, Decimal sessionAlertFrequencyInMinutes, int sessionAlertNotificationPatternId,
                                        Decimal alertTimeInMinsBeforeExpiry, Decimal alertFrequencySecsBeforeExpiry, int alertPatternIdBeforeExpiry, int alertPatternIdOnExpiry,
                                        Decimal alertFrequencySecsOnExpiry, Decimal alertDurationMinsAfterExpiry, bool isActive)
        {
            log.LogMethodEntry(notificationTagProfileId, notificationTagProfileName, sessionAlertFrequencyInMinutes, sessionAlertNotificationPatternId,
                               alertTimeInMinsBeforeExpiry, alertFrequencySecsBeforeExpiry, alertPatternIdBeforeExpiry, alertPatternIdOnExpiry, alertFrequencySecsOnExpiry,
                               alertDurationMinsAfterExpiry,isActive);
            this.notificationTagProfileId = notificationTagProfileId;
            this.notificationTagProfileName = notificationTagProfileName;
            this.sessionAlertFrequencyInMinutes = sessionAlertFrequencyInMinutes;
            this.sessionAlertNotificationPatternId = sessionAlertNotificationPatternId;
            this.alertTimeInMinsBeforeExpiry = alertTimeInMinsBeforeExpiry;
            this.alertFrequencySecsBeforeExpiry = alertFrequencySecsBeforeExpiry;
            this.alertPatternIdBeforeExpiry = alertPatternIdBeforeExpiry;
            this.alertPatternIdOnExpiry = alertPatternIdOnExpiry;
            this.alertFrequencySecsOnExpiry = alertFrequencySecsOnExpiry;
            this.alertDurationMinsAfterExpiry = alertDurationMinsAfterExpiry;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        public NotificationTagProfileDTO(int notificationTagProfileId, string notificationTagProfileName, Decimal sessionAlertFrequencyInMinutes, int sessionAlertNotificationPatternId,
                                        Decimal alertTimeInMinsBeforeExpiry, Decimal alertFrequencySecsBeforeExpiry, int alertPatternIdBeforeExpiry, int alertPatternIdOnExpiry,
                                        Decimal alertFrequencySecsOnExpiry, Decimal alertDurationMinsAfterExpiry, bool isActive, string createdBy, DateTime creationDate, string lastUpdatedBy, 
                                        DateTime lastUpdateDate, int siteId, int masterEntityId, bool synchStatus, string guid)
            : this(notificationTagProfileId, notificationTagProfileName, sessionAlertFrequencyInMinutes, sessionAlertNotificationPatternId,
                               alertTimeInMinsBeforeExpiry, alertFrequencySecsBeforeExpiry, alertPatternIdBeforeExpiry, alertPatternIdOnExpiry, alertFrequencySecsOnExpiry,
                               alertDurationMinsAfterExpiry, isActive)
        {
            log.LogMethodEntry(notificationTagProfileId, notificationTagProfileName, sessionAlertFrequencyInMinutes, sessionAlertNotificationPatternId,
                               alertTimeInMinsBeforeExpiry, alertFrequencySecsBeforeExpiry, alertPatternIdBeforeExpiry, alertPatternIdOnExpiry, alertFrequencySecsOnExpiry,
                               alertDurationMinsAfterExpiry, isActive, createdBy, creationDate, lastUpdatedBy, lastUpdateDate, siteId, masterEntityId, synchStatus, guid);

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
        /// Get/Set method of the NotificationTagProfileId field
        /// </summary>
        public int NotificationTagProfileId { get { return notificationTagProfileId; } set { this.IsChanged = true; notificationTagProfileId = value; } }

        /// <summary>
        /// Get/Set method of the SessionAlertFrequencyInMinutes field
        /// </summary>
        public Decimal SessionAlertFrequencyInMinutes { get { return sessionAlertFrequencyInMinutes; } set { this.IsChanged = true; sessionAlertFrequencyInMinutes = value; } }

        /// <summary>
        /// Get/Set method of the SessionAlertNotificationPatternId field
        /// </summary>
        public int SessionAlertNotificationPatternId { get { return sessionAlertNotificationPatternId; } set { this.IsChanged = true; sessionAlertNotificationPatternId = value; } }

        /// <summary>
        /// Get/Set method of the AlertTimeInMinsBeforeExpiry field
        /// </summary>
        public Decimal AlertTimeInMinsBeforeExpiry { get { return alertTimeInMinsBeforeExpiry; } set { this.IsChanged = true; alertTimeInMinsBeforeExpiry = value; } }

        /// <summary>
        /// Get/Set method of the AlertFrequencySecsBeforeExpiry field
        /// </summary>
        public Decimal AlertFrequencySecsBeforeExpiry { get { return alertFrequencySecsBeforeExpiry; } set { this.IsChanged = true; alertFrequencySecsBeforeExpiry = value; } }

        /// <summary>
        /// Get/Set method of the AlertPatternIdBeforeExpiry field
        /// </summary>
        public int AlertPatternIdBeforeExpiry { get { return alertPatternIdBeforeExpiry; } set { this.IsChanged = true; alertPatternIdBeforeExpiry = value; } }

        /// <summary>
        /// Get/Set method of the AlertPatternIdOnExpiry field
        /// </summary>
        public int AlertPatternIdOnExpiry { get { return alertPatternIdOnExpiry; } set { this.IsChanged = true; alertPatternIdOnExpiry = value; } }

        /// <summary>
        /// Get/Set method of the AlertFrequencySecsOnExpiry field
        /// </summary>
        public Decimal AlertFrequencySecsOnExpiry { get { return alertFrequencySecsOnExpiry; } set { this.IsChanged = true; alertFrequencySecsOnExpiry = value; } }

        /// <summary>
        /// Get/Set method of the AlertDurationMinsAfterExpiry field
        /// </summary>
        public Decimal AlertDurationMinsAfterExpiry { get { return alertDurationMinsAfterExpiry; } set { this.IsChanged = true; alertDurationMinsAfterExpiry = value; } }

        /// <summary>
        /// Get/Set method of the NotificationTagProfileName field
        /// </summary>
        public string NotificationTagProfileName { get { return notificationTagProfileName; } set { this.IsChanged = true; notificationTagProfileName = value; } }

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
        /// Returns whether the NotificationTagProfileDTO changed or any of its NotificationTagIssuedDTOList  are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
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
                    return notifyingObjectIsChanged || notificationTagProfileId < 0;
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
