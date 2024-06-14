/********************************************************************************************
 * Project Name - KioskUIFramework
 * Description  - Data object of AppScreenProfile
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.60        15-May-2019   Girish Kundar           Created 
 *2.80        09-Jun-2020   Mushahid Faizan         Modified.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.KioskUIFamework
{
    /// <summary>
    /// This is the AppScreenProfile data object class. This acts as data holder for the AppScreenProfile business object
    /// </summary>
    public class AppScreenProfileDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  APP_SCREEN_PROFILE ID field
            /// </summary>
            APP_SCREEN_PROFILE_ID,
            /// <summary>
            /// Search by  APP_SCREEN_PROFILE ID LIST field
            /// </summary>
            APP_SCREEN_PROFILE_ID_LIST,
            /// <summary>
            /// Search by  Profile Name field
            /// </summary>
            APP_SCREEN_PROFILE_NAME,
            /// <summary>
            /// Search by site_id field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID
        }
        private int appScreenProfileId;
        private string appScreenProfileName;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private int siteId;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        private DateTime creationDate;
        private string createdBy;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        /// <summary>
        /// Default constructor
        /// </summary>
        public AppScreenProfileDTO()
        {
            log.LogMethodEntry();
            appScreenProfileId = -1;
            siteId = -1;
            masterEntityId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with Required data fields
        /// </summary>
        public AppScreenProfileDTO(int appScreenProfileId, string appScreenProfileName)
            :this()
        {
            log.LogMethodEntry(appScreenProfileId, appScreenProfileName);
            this.appScreenProfileId = appScreenProfileId;
            this.appScreenProfileName = appScreenProfileName;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public AppScreenProfileDTO(int appScreenProfileId, string appScreenProfileName, string lastUpdatedBy, DateTime lastUpdatedDate,
                          int siteId, string guid, bool synchStatus, int masterEntityId,string createdBy, DateTime creationDate)
            :this(appScreenProfileId, appScreenProfileName)
        {
            log.LogMethodEntry(appScreenProfileId, appScreenProfileName, lastUpdatedBy, lastUpdatedDate, siteId, guid, synchStatus, masterEntityId,createdBy, creationDate);
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.siteId = siteId;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the AppScreenProfileId field
        /// </summary>
        public int AppScreenProfileId
        {
            get { return appScreenProfileId; }
            set { this.IsChanged = true; appScreenProfileId = value; }
        }
        /// <summary>
        /// Get/Set method of the appScreenProfileName field
        /// </summary>
        public string AppScreenProfileName
        {
            get { return appScreenProfileName; }
            set { this.IsChanged = true; appScreenProfileName = value; }
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
            set {  lastUpdatedDate = value; }
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
            set { this.IsChanged = true; guid = value; }
        }
        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        public bool SynchStatus
        {
            get { return synchStatus; }
            set {  synchStatus = value; }
        }
        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId
        {
            get { return masterEntityId; }
            set { this.IsChanged = true; masterEntityId = value; }
        }
        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        public string CreatedBy
        {
            get { return createdBy; }
            set {  createdBy = value; }
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
                    return notifyingObjectIsChanged || appScreenProfileId < 0;
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
