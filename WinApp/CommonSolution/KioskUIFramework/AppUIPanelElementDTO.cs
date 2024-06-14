/********************************************************************************************
* Project Name - KioskUIFramework
* Description  - Data object of AppUIPanelElement
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.70      16-May-2019   Girish Kundar           Created 
*2.90      04-Jun-2020   Mushahid Faizan         Added elementIndex column.
*2.90        24-Aug-2020   Girish Kundar            Modified : Issue Fix Child entity delete
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;

namespace Semnox.Parafait.KioskUIFamework
{
    /// <summary>
    /// This is the AppUIPanelElement data object class
    /// </summary>
    public class AppUIPanelElementDTO
    {
        /// <summary>
        /// This is the AppUIPanelElement data object class. This acts as data holder for the AppUIPanelElement business object
        /// </summary>
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by UI PANEL ELEMENT ID field
            /// </summary>
            UI_PANEL_ELEMENT_ID,
            /// <summary>
            /// Search by UI PANEL ELEMENT ID LIST field
            /// </summary>
            UI_PANEL_ELEMENT_ID_LIST,
            /// <summary>
            /// Search by ACTION SCREEN ID field
            /// </summary>
            ACTION_SCREEN_ID,
            /// <summary>
            /// Search by UI PANEL ID field
            /// </summary>
            UI_PANEL_ID,
            /// <summary>
            /// Search by UI PANEL ID LIST field
            /// </summary>
            UI_PANEL_ID_LIST,
            /// <summary>
            /// Search by ELEMENT NAME field
            /// </summary>
            ELEMENT_NAME,
            /// <summary>
            /// Search by ACTIVE FLAG field
            /// </summary>
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

        private int uiPanelElementId;
        private int uiPanelId;
        private string elementName;
        private int elementIndex;
        private int actionScreenId;
        private bool activeFlag;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private int siteId;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        private string createdBy;
        private DateTime creationDate;
        private List<AppUIElementParameterDTO> appUIElementParametersDTOList;
        private List<AppUIPanelElementAttributeDTO> appUIPanelElementAttributeDTOList;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        /// <summary>
        /// Default constructor
        /// </summary>
        public AppUIPanelElementDTO()
        {
            log.LogMethodEntry();
            uiPanelId = -1;
            uiPanelElementId = -1;
            actionScreenId = -1;
            siteId = -1;
            masterEntityId = -1;
            activeFlag = true;
            appUIElementParametersDTOList = new List<AppUIElementParameterDTO>();
            appUIPanelElementAttributeDTOList = new List<AppUIPanelElementAttributeDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public AppUIPanelElementDTO(int uIPanelElementId, int uIPanelId, string elementName, int actionScreenId, bool activeFlag, int elementIndex)
            : this()
        {
            log.LogMethodEntry(uIPanelElementId, uIPanelId, elementName, actionScreenId, activeFlag);
            this.uiPanelElementId = uIPanelElementId;
            this.uiPanelId = uIPanelId;
            this.elementName = elementName;
            this.actionScreenId = actionScreenId;
            this.activeFlag = activeFlag;
            this.elementIndex = elementIndex;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public AppUIPanelElementDTO(int uIPanelElementId, int uIPanelId, string elementName, int actionScreenId,
                                    bool activeFlag, string lastUpdatedBy, DateTime lastUpdatedDate, int siteId,
                                    string guid, bool synchStatus, int masterEntityId, string createdBy,
                                    DateTime creationDate, int elementIndex)
            : this(uIPanelElementId, uIPanelId, elementName, actionScreenId, activeFlag, elementIndex)
        {
            log.LogMethodEntry(uIPanelElementId, uIPanelId, elementName, actionScreenId, activeFlag, lastUpdatedBy,
                                lastUpdatedDate, siteId, guid, synchStatus, masterEntityId, createdBy, creationDate, elementIndex);
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
        /// Get/Set method of the UIPanelElementId field
        /// </summary>
        public int UIPanelElementId
        {
            get { return uiPanelElementId; }
            set { uiPanelElementId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the UIPanelId field
        /// </summary>
        public int UIPanelId
        {
            get { return uiPanelId; }
            set { uiPanelId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// /// <summary>
        /// Get/Set method of the UIPanelId field
        /// </summary>
        public int  ElementIndex
        {
            get { return elementIndex; }
            set { elementIndex = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the ElementName field
        /// </summary>
        public string ElementName
        {
            get { return elementName; }
            set { elementName = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the ActionScreenId field
        /// </summary>
        public int ActionScreenId
        {
            get { return actionScreenId; }
            set { actionScreenId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the ActiveFlag field
        /// </summary>
        public bool ActiveFlag
        {
            get { return activeFlag; }
            set { activeFlag = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get method of the LastUpdated By field
        /// </summary>
        public string LastUpdatedBy
        {
            get { return lastUpdatedBy; }
            set { lastUpdatedBy = value; }
        }
        /// <summary>
        ///  Get/Set method of the LastUpdate Date field
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
            set { masterEntityId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the Created By field
        /// </summary>
        public string CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        /// <summary>
        /// Get/Set method of the Creation Date field
        /// </summary>
        public DateTime CreationDate
        {
            get { return creationDate; }
            set { creationDate = value; }
        }
        /// <summary>
        /// Get/Set method of the AppUIElementParameterDTOList field
        /// </summary>
        public List<AppUIElementParameterDTO> AppUIElementParameterDTOList
        {
            get { return appUIElementParametersDTOList; }
            set { appUIElementParametersDTOList = value; }
        }
        /// <summary>
        /// Get/Set method of the  AppUIPanelElementAttributeDTOList field
        /// </summary>
        public List<AppUIPanelElementAttributeDTO> AppUIPanelElementAttributeDTOList
        {
            get { return appUIPanelElementAttributeDTOList; }
            set { appUIPanelElementAttributeDTOList = value; }
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
                    return notifyingObjectIsChanged || UIPanelElementId < 0;
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
        /// Returns true or false whether the AppUIPanelElementDTO changed or any of its children are changed
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
                if (appUIElementParametersDTOList != null &&
                    appUIElementParametersDTOList.Any(x => x.IsChangedRecursive))
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