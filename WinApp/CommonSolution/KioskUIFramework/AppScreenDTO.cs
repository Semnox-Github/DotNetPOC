/********************************************************************************************
 * Project Name - KioskUIFramework
 * Description  - Data object of AppScreens
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.60        15-May-2019   Girish Kundar           Created 
 *2.80      02-Jun-2020   Mushahid Faizan         Modified : 3 tier changes for RestAPI.
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
    /// This is the AppScreens data object class. This acts as data holder for the AppScreens business object
    /// </summary>
    public class AppScreenDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  SCREEN_ID, field
            /// </summary>
            SCREEN_ID,
            /// <summary>
            /// Search by  SCREEN_ID, field
            /// </summary>
            SCREEN_ID_LIST,
            /// <summary>
            /// Search by  APP_SCREEN_PROFILE ID field
            /// </summary>
            APP_SCREEN_PROFILE_ID,
            /// <summary>
            /// Search by  APP_SCREEN_PROFILE ID LIST field
            /// </summary>
            APP_SCREEN_PROFILE_ID_LIST,
            /// <summary>
            /// Search by SCREEN_NAME field
            /// </summary>
            SCREEN_NAME,
            /// <summary>
            /// Search by ACTIVE_FLAG field
            /// </summary>
            ACTIVE_FLAG,
            /// <summary>
            /// Search by SCREEN_KEY field
            /// </summary>
            SCREEN_KEY,
            /// <summary>
            /// Search by CODE OBJECT NAME field
            /// </summary>
            CODE_OBJECT_NAME,
            /// <summary>
            /// Search by SITE ID  field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID
        }
        private int screenId;
        private string screenName;
        private string codeObjectName;
        private string screenKey;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private int siteId;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        private int appScreenProfileId;
        private string createdBy;
        private DateTime creationDate;
        private bool activeFlag; // new member added
        //private List<AppUIPanelDTO> appUIPanelsDTOList;
        private List<AppScreenUIPanelDTO> appScreenUIPanelDTOList;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private bool notifyingObjectIsChanged;
        /// <summary>
        /// Default constructor
        /// </summary>
        public AppScreenDTO()
        {
            log.LogMethodEntry();
            screenId = -1;
            appScreenProfileId = -1;
            siteId = -1;
            masterEntityId = -1;
            activeFlag = true;
            //appUIPanelsDTOList = new List<AppUIPanelDTO>();
            appScreenUIPanelDTOList = new List<AppScreenUIPanelDTO>();
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with Required data fields
        /// </summary>
        public AppScreenDTO(int screenId, string screenName, string codeObjectName, string screenKey,  int appScreenProfileId,bool activeFlag)
            :this()
        {
            log.LogMethodEntry(screenId, screenName, codeObjectName, screenKey,appScreenProfileId,activeFlag);
            this.screenId = screenId;
            this.screenName = screenName;
            this.screenKey = screenKey;
            this.codeObjectName = codeObjectName;
            this.appScreenProfileId = appScreenProfileId;
            this.activeFlag = activeFlag;
            log.LogMethodExit(null);
        }
        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public AppScreenDTO(int screenId, string screenName, string codeObjectName, string screenKey, string lastUpdatedBy, DateTime lastUpdatedDate,
                          int siteId, string guid, bool synchStatus, int masterEntityId, int appScreenProfileId, string createdBy, DateTime creationDate, bool activeFlag)
            :this(screenId, screenName, codeObjectName, screenKey, appScreenProfileId, activeFlag)
        {
            log.LogMethodEntry(screenId, screenName, codeObjectName, screenKey, lastUpdatedBy, lastUpdatedDate, siteId, guid, synchStatus, masterEntityId, appScreenProfileId, creationDate, createdBy, activeFlag);
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
        /// Get/Set method of the ScreenId field
        /// </summary>
        public int ScreenId
        {
            get { return screenId; }
            set { this.IsChanged = true; screenId = value; }
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
        /// Get/Set method of the screenName field
        /// </summary>
        public string ScreenName
        {
            get { return screenName; }
            set { this.IsChanged = true; screenName = value; }
        }
        /// <summary>
        /// Get/Set method of the screenName field
        /// </summary>
        public string ScreenKey
        {
            get { return screenKey; }
            set { this.IsChanged = true; screenKey = value; }
        }
        /// <summary>
        /// Get/Set method of the screenName field
        /// </summary>
        public string CodeObjectName
        {
            get { return codeObjectName; }
            set { this.IsChanged = true; codeObjectName = value; }
        }
        /// <summary>
        /// Get method of the LastUpdatedBy field
        /// </summary>
        public string LastUpdatedBy
        {
            get { return lastUpdatedBy; }
            set {  lastUpdatedBy = value; }
        }
        /// <summary>
        ///  Get/Set method of the LastUpdateDate field
        /// </summary>
        public DateTime LastUpdatedDate
        {
            get { return lastUpdatedDate; }
            set {lastUpdatedDate = value; }
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
            set {synchStatus = value; }
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
            set { createdBy = value; }
        }
        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        public DateTime CreationDate
        {
            get { return creationDate; }
            set {  creationDate = value; }
        }
        /// <summary>
        /// Get/Set method of the ActiveFlag field
        /// </summary>
        public bool ActiveFlag
        {
            get { return activeFlag; }
            set { this.IsChanged = true; activeFlag = value; }
        }
        ///// <summary>
        ///// Get/Set method of the    AppUIPanelsDTOList field
        ///// </summary>
        //public List<AppUIPanelDTO> AppUIPanelsDTOList
        //{
        //    get { return appUIPanelsDTOList; }
        //    set { appUIPanelsDTOList = value; }
        //}
        /// <summary>
        /// Get/Set method of the    AppUIPanelsDTOList field
        /// </summary>
        public List<AppScreenUIPanelDTO> AppScreenUIPanelDTOList
        {
            get { return appScreenUIPanelDTOList; }
            set { appScreenUIPanelDTOList = value; }
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
                    return notifyingObjectIsChanged || screenId < 0;
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
        /// Returns whether the AppScreensDTO changed or any of its childeren are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (appScreenUIPanelDTOList != null &&
                   appScreenUIPanelDTOList.Any(x => x.IsChangedRecursive))
                {
                    return true;
                }
                return false;
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
