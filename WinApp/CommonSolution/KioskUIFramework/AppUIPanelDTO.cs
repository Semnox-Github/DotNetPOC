/********************************************************************************************
 * Project Name - KioskUIFramework
 * Description  - Data object of AppUIPanel
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70      15-May-2019   Girish Kundar           Created 
 *2.80      04-Jun-2020   Mushahid Faizan          Modified : 3 Tier Changes for Rest API
 ********************************************************************************************/
using System;
using System.Collections.Generic; 
using System.Linq; 

namespace Semnox.Parafait.KioskUIFamework
{
    /// <summary>
    /// This is the AppUIPanel data object class. This acts as data holder for the AppUIPanel business object
    /// </summary>
    public class AppUIPanelDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        /// 
        public enum SearchByParameters
        {
            
            /// <summary>
            /// Search by UI PANEL ID field
            /// </summary>
            UI_PANEL_ID,
            /// <summary>
            /// Search by UI PANEL ID LIST field
            /// </summary>
            UI_PANEL_ID_LIST,
            /// <summary>
            /// Search by APP SCREEN PROFILE ID LIST field
            /// </summary>
            APP_SCREEN_PROFILE_ID_LIST,
            /// <summary>
            /// Search by APP SCREEN PROFILE ID field
            /// </summary>
            APP_SCREEN_PROFILE_ID,
            /// <summary>
            /// Search by UI PANEL NAME field
            /// </summary>
            UI_PANEL_NAME,
            /// <summary>
            /// Search by UI PANEL KEY field
            /// </summary>
            UI_PANEL_KEY,
            ///<summary>
            /// Search by ACTIVE FLAG field
            /// <summary>
            ACTIVE_FLAG,
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID
        }
        private int uiPanelId;
        private string uiPanelName;
        private string uiPanelKey ;
        private int? panelWidth;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private int siteId;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        private int appScreenProfileId;
        private string createdBy;
        private DateTime creationDate;
        private bool activeFlag; 
        private List<AppUIPanelElementDTO> appUIPanelElementsDTOList;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        /// <summary>
        /// Default constructor
        /// </summary>
        public AppUIPanelDTO()
        {
            log.LogMethodEntry();
            uiPanelId = -1;
            appScreenProfileId = -1;
            siteId = -1;
            masterEntityId = -1;
            activeFlag = true;
            appUIPanelElementsDTOList = new List<AppUIPanelElementDTO>();
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public AppUIPanelDTO(int uiPanelId ,string uiPanelName, string uiPanelKey ,int? panelWidth, int appScreenProfileId, bool activeFlag)
            :this()
        {
            log.LogMethodEntry(uiPanelId, uiPanelName, uiPanelKey, panelWidth,lastUpdatedBy, lastUpdatedDate, siteId,guid , synchStatus,
                masterEntityId,  appScreenProfileId, creationDate, createdBy);
            this.uiPanelId = uiPanelId;
            this.uiPanelName = uiPanelName;
            this.uiPanelKey = uiPanelKey;
            this.panelWidth = panelWidth;
            this.appScreenProfileId = appScreenProfileId;
            this.activeFlag = activeFlag;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public AppUIPanelDTO(int uiPanelId, string uiPanelName, string uiPanelKey, int? panelWidth, string lastUpdatedBy, DateTime lastUpdatedDate,
                             int siteId, string guid, bool synchStatus, int masterEntityId, int appScreenProfileId, string createdBy, DateTime creationDate, bool activeFlag):
            this(uiPanelId, uiPanelName, uiPanelKey, panelWidth, appScreenProfileId, activeFlag)
        {
            log.LogMethodEntry(uiPanelId, uiPanelName, uiPanelKey, panelWidth, lastUpdatedBy, lastUpdatedDate, siteId, guid, synchStatus,
                               masterEntityId, appScreenProfileId, creationDate, createdBy);
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
        /// Get/Set method of the UIPanelId field
        /// </summary>
        public int UIPanelId
        {
            get { return uiPanelId; }
            set {  uiPanelId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the AppScreenProfileId field
        /// </summary>
        public int AppScreenProfileId
        {
            get { return appScreenProfileId; }
            set {  appScreenProfileId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the UIPanelName field
        /// </summary>
        public string UIPanelName
        {
            get { return uiPanelName; }
            set {  uiPanelName = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the UIPanelKey field
        /// </summary>
        public string UIPanelKey
        {
            get { return uiPanelKey; }
            set {  uiPanelKey = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the PanelWidth field
        /// </summary>
        public int? PanelWidth
        {
            get { return panelWidth; }
            set { panelWidth = value; this.IsChanged = true; }
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
        ///  Get/Set method of the LastUpdatedDate field
        /// </summary>
        public DateTime LastUpdatedDate
        {
            get { return lastUpdatedDate; }
            set { lastUpdatedDate = value; }
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
            set {  masterEntityId = value; this.IsChanged = true; }
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
        /// Get/Set method of the ActiveFlag field
        /// </summary>
        public bool ActiveFlag
        {
            get { return activeFlag; }
            set {  activeFlag = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the appUIPanelElementsDTOList field
        /// </summary>
        public List<AppUIPanelElementDTO> AppUIPanelElementsDTOList
        {
            get { return appUIPanelElementsDTOList; }
            set { appUIPanelElementsDTOList = value; }
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
                    return notifyingObjectIsChanged || uiPanelId < 0;
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
        /// Returns true or false whether the AppUIPanelDTO changed or any of its children are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if(IsChanged)
                {
                    return true;
                }
                if(appUIPanelElementsDTOList != null && 
                   appUIPanelElementsDTOList.Any(x => x.IsChangedRecursive))
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
            IsChanged = false;
            log.LogMethodExit();
        }
    }
}
