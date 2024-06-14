/********************************************************************************************
* Project Name - KioskUIFramework
* Description  - Data object of AppScreenUIPanels
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.60        16-May-2019   Girish Kundar           Created 
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
    /// This is the AppScreenUIPanelDTO data object class. This acts as data holder for the AppScreenUIPanel business object
    /// </summary>
    public class AppScreenUIPanelDTO
    {
        /// <summary>
        /// This is the AppScreenUIPanels data object class. This acts as data holder for the AppScreenUIPanels business object
        /// </summary>
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by SCREEN_PANEL_ID field
            /// </summary>
            SCREEN_PANEL_ID,
            /// <summary>
            /// Search by SCREEN_PANEL_ID LIST field
            /// </summary>
            SCREEN_PANEL_ID_LIST,
            /// <summary>
            /// Search by   SCREEN_ID, field
            /// </summary>
            SCREEN_ID,
            /// <summary>
            /// Search by   SCREEN_ID LIST, field
            /// </summary>
            SCREEN_ID_LIST,
            /// <summary>
            /// Search by   UI_PANEL_ID field
            /// </summary>
            UI_PANEL_ID,
            /// <summary>
            /// Search by   UI_PANEL_INDEX field
            /// </summary>
            UI_PANEL_INDEX,
            /// <summary>
            /// Search by   ACTIVE_FLAG field
            /// </summary>
            ACTIVE_FLAG,
            /// <summary>
            /// Search by site_id field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID
        }

        private int screenUIPanelId;
        private int screenId;
        private int uiPanelId;
        private int uiPanelIndex;
        private bool activeFlag;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private int siteId;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        private string createdBy;
        private DateTime creationDate;
        private List<AppUIPanelElementAttributeDTO> appUIPanelElementAttributeDTOList;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// Default constructor
        /// </summary>
        public AppScreenUIPanelDTO()
        {
            log.LogMethodEntry();
            screenUIPanelId = -1;
            screenId = -1;
            uiPanelId = -1;
            siteId = -1;
            masterEntityId = -1;
            activeFlag = true;
            appUIPanelElementAttributeDTOList = new List<AppUIPanelElementAttributeDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with Required data fields
        /// </summary>
        public AppScreenUIPanelDTO(int screenUIPanelId, int screenId, int uiPanelId, int uiPanelIndex, bool activeFlag)
            :this()
        {
            log.LogMethodEntry(screenUIPanelId, screenId, uiPanelId, uiPanelIndex, activeFlag);

            this.screenUIPanelId = screenUIPanelId;
            this.screenId = screenId;
            this.uiPanelId = uiPanelId;
            this.uiPanelIndex = uiPanelIndex;
            this.activeFlag = activeFlag;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public AppScreenUIPanelDTO(int screenUIPanelId,int screenId,int uiPanelId,int uiPanelIndex, bool activeFlag,string lastUpdatedBy, 
            DateTime lastUpdatedDate, int siteId, string guid, bool synchStatus, int masterEntityId, string createdBy, DateTime creationDate  )
            :this(screenUIPanelId, screenId, uiPanelId, uiPanelIndex, activeFlag)
        {
            log.LogMethodEntry(screenUIPanelId,screenId,uiPanelId,uiPanelIndex,activeFlag,lastUpdatedBy,lastUpdatedDate,siteId,guid,synchStatus,masterEntityId,createdBy,creationDate);
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
        /// Get/Set method of the ScreenUIPanelId field
        /// </summary>
        public int ScreenUIPanelId
        {
            get { return screenUIPanelId; }
            set { this.IsChanged = true; screenUIPanelId = value; }
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
        /// Get/Set method of the UIPanelId field
        /// </summary>
        public int UIPanelId
        {
            get { return uiPanelId; }
            set { this.IsChanged = true; uiPanelId = value; }
        }
        /// <summary>
        /// Get/Set method of the UIPanelIndex field
        /// </summary>
        public int UIPanelIndex
        {
            get { return uiPanelIndex; }
            set { this.IsChanged = true; uiPanelIndex = value; }
        }
        /// <summary>
        /// Get/Set method of the ActiveFlag field
        /// </summary>
        public bool ActiveFlag
        {
            get { return activeFlag; }
            set { this.IsChanged = true; activeFlag = value; }
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
            set { lastUpdatedDate = value; }
        }
        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        public int SiteId
        {
            get { return siteId; }
            set {siteId = value; }
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
        /// Get/Set method of the    AppUIPanelElementAttributeDTOList field
        /// </summary>
        public List<AppUIPanelElementAttributeDTO> AppUIPanelElementAttributeDTOList
        {
            get { return appUIPanelElementAttributeDTOList; }
            set { appUIPanelElementAttributeDTOList = value; }
        }

        /// <summary>
        /// Returns whether the AppUIPanelElementAttributeDTO changed or any of its children are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (appUIPanelElementAttributeDTOList != null &&
                   appUIPanelElementAttributeDTOList.Any(x => x.IsChanged))
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
                    return notifyingObjectIsChanged || screenUIPanelId < 0;
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
