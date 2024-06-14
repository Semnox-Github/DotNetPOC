/********************************************************************************************
* Project Name - KioskUIFramework
* Description  - Data object of AppUIPanelElementAttribute
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.70      16-May-2019    Girish Kundar           Created 
*2.80      02-Jun-2020   Mushahid Faizan         Modified : 3 tier changes for RestAPI.
********************************************************************************************/
using System;

namespace Semnox.Parafait.KioskUIFamework
{
    /// <summary>
    ///  This is the AppUIPanelElementAttribute data object class
    /// </summary>
    public class AppUIPanelElementAttributeDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by UIPANEL ELEMENT ATTRIBUTE ID field
            /// </summary>
            UI_PANEL_ELEMENT_ATTRIBUTE_ID,
            /// <summary>
            /// Search by UI PANEL ELEMENT ATTRIBUTE LIST ID field
            /// </summary>
            UI_PANEL_ELEMENT_ATTRIBUTE_ID_LIST,
            /// <summary>
            /// Search by UI PANEL ELEMENT ID  field
            /// </summary>
            UI_PANEL_ELEMENT_ID,
            /// <summary>
            /// Search by UI ELEMENT ID LIST field
            /// </summary>
            UI_ELEMENT_ID_LIST,
            /// <summary>
            /// Search by UI PANEL ELEMENT ID  field
            /// </summary>
            UI_PANEL_ELEMENT_ID_LIST,
            /// <summary>
            /// Search by LANGUAGE ID field
            /// </summary>
            LANGUAGE_ID,
            /// <summary>
            /// Search by FILE NAME field
            /// </summary>
            FILE_NAME,
            /// <summary>
            /// Search by ACTIVE FLAG field
            /// </summary>
            ACTIVE_FLAG,
            /// <summary>
            /// Search by SCREEN PANEL ID, field
            /// </summary>
            SCREEN_UI_PANEL_ID,
            /// <summary>
            /// Search by SCREEN PANEL ID, field
            /// </summary>
            SCREEN_UI_PANEL_ID_LIST,
            /// <summary>
            /// Search by DISPLAY TEXT field
            /// </summary>
            DISPLAY_TEXT,
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID
        }

        private int uiPanelElementAttributeId;
        private int uiPanelElementId;
        private string displayText;
        private string fileName;
        private byte[] image;
        private string actionScreenTitle1;
        private string actionScreenTitle2;
        private string actionScreenFooter1;
        private string actionScreenFooter2;
        private bool activeFlag;
        private int languageId;
        private int screenUIPanelId;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private int siteId;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        private string createdBy;
        private DateTime creationDate;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        /// <summary>
        /// Default constructor
        /// </summary>
        public AppUIPanelElementAttributeDTO()
        {
            log.LogMethodEntry();
            uiPanelElementAttributeId = -1;
            uiPanelElementId = -1;
            screenUIPanelId = -1;
            languageId = -1;
            siteId = -1;
            masterEntityId = -1;
            activeFlag = true;
            fileName = string.Empty;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with Required data fields
        /// </summary>
        public AppUIPanelElementAttributeDTO(int uiPanelElementAttributeId, int uiPanelElementId, string displayText,
                                             string fileName, byte[] image, string actionScreenTitle1, string actionScreenTitle2,
                                             string actionScreenFooter1, string actionScreenFooter2, bool activeFlag,
                                             int languageId, int screenUIPanelId)
            :this()
        {
            log.LogMethodEntry(uiPanelElementAttributeId, uiPanelElementId, displayText, fileName, image, actionScreenTitle1,
                               actionScreenTitle2, actionScreenFooter1, actionScreenFooter2, activeFlag, languageId,screenUIPanelId);
            this.uiPanelElementAttributeId = uiPanelElementAttributeId;
            this.uiPanelElementId = uiPanelElementId;
            this.displayText = displayText;
            this.fileName = fileName;
            this.image = image;
            this.actionScreenTitle1 = actionScreenTitle1;
            this.actionScreenTitle2 = actionScreenTitle2;
            this.actionScreenFooter1 = actionScreenFooter1;
            this.actionScreenFooter2 = actionScreenFooter2;
            this.languageId = languageId;
            this.screenUIPanelId = screenUIPanelId;
            this.activeFlag = activeFlag;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public AppUIPanelElementAttributeDTO(int uiPanelElementAttributeId,int uiPanelElementId,string displayText, 
                                             string fileName,byte[] image, string actionScreenTitle1, string actionScreenTitle2,
                                             string actionScreenFooter1,string actionScreenFooter2,  bool activeFlag,
                                             int languageId,int screenUIPanelId, string lastUpdatedBy, DateTime lastUpdatedDate, 
                                             int siteId,string guid,bool synchStatus,int masterEntityId,string createdBy ,DateTime creationDate)
            :this(uiPanelElementAttributeId, uiPanelElementId, displayText, fileName, image, actionScreenTitle1,
                               actionScreenTitle2, actionScreenFooter1, actionScreenFooter2, activeFlag, languageId,
                               screenUIPanelId)
        {
            log.LogMethodEntry(uiPanelElementAttributeId, uiPanelElementId, displayText, fileName, image, actionScreenTitle1,  
                               actionScreenTitle2, actionScreenFooter1, actionScreenFooter2, activeFlag, languageId,
                               screenUIPanelId, lastUpdatedBy, lastUpdatedDate, siteId, guid, synchStatus, masterEntityId, createdBy, creationDate);
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
        /// Get/Set method of the UIPanelElementAttributeId field
        /// </summary>
        public int UIPanelElementAttributeId
        {
            get { return uiPanelElementAttributeId; }
            set { uiPanelElementAttributeId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the UIPanelElementId field
        /// </summary>
        public int UIPanelElementId
        {
            get { return uiPanelElementId; }
            set {  uiPanelElementId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the DisplayText field
        /// </summary>
        public string DisplayText
        {
            get { return displayText; }
            set {  displayText = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the FileName field
        /// </summary>
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the Image field
        /// </summary>
        public byte[] Image
        {
            get { return image; }
            set { image = value; this.IsChanged = true; }
        }
       
        /// <summary>
        /// Get/Set method of the ActionScreen Title1 field
        /// </summary>
        public string ActionScreenTitle1
        {
            get { return actionScreenTitle1; }
            set {  actionScreenTitle1 = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the ActionScreenTitle2 field
        /// </summary>
        public string ActionScreenTitle2
        {
            get { return actionScreenTitle2; }
            set {  actionScreenTitle2 = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the ActionScreenFooter1 field
        /// </summary>
        public string ActionScreenFooter1
        {
            get { return actionScreenFooter1; }
            set { actionScreenFooter1 = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the ActionScreenFooter2 field
        /// </summary>
        public string ActionScreenFooter2
        {
            get { return actionScreenFooter2; }
            set {  actionScreenFooter2 = value; this.IsChanged = true; }
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
        /// Get/Set method of the LanguageId field
        /// </summary>
        public int LanguageId
        {
            get { return languageId; }
            set {  languageId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the ScreenUIPanelId field
        /// </summary>
        public int ScreenUIPanelId
        {
            get { return screenUIPanelId; }
            set {  screenUIPanelId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get method of the LastUpdatedBy field
        /// </summary>
        public string LastUpdatedBy
        {
            get { return lastUpdatedBy; }
            set { lastUpdatedBy = value;  }
        }
        /// <summary>
        ///  Get/Set method of the LastUpdateDate field
        /// </summary>
        public DateTime LastUpdatedDate
        {
            get { return lastUpdatedDate; }
            set {  lastUpdatedDate = value;  }
        }
        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        public int SiteId
        {
            get { return siteId; }
            set { siteId = value;}
        }
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid
        {
            get { return guid; }
            set {  guid = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        public bool SynchStatus
        {
            get { return synchStatus; }
            set { synchStatus = value;  }
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
        /// Get/Set method of the CreatedBy field
        /// </summary>
        public string CreatedBy
        {
            get { return createdBy; }
            set {  createdBy = value;  }
        }
        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        public DateTime CreationDate
        {
            get { return creationDate; }
            set {  creationDate = value;  }
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
                    return notifyingObjectIsChanged || uiPanelElementAttributeId < 0;
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
