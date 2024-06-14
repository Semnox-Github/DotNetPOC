/********************************************************************************************
 * Project Name - KioskUIFramework
 * Description  - Data object of AppUIElementParameters
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.60        15-May-2019   Girish Kundar           Created 
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
    /// This is the AppUIElementParameters data object class. This acts as data holder for the AppUIElementParameters object
    /// </summary>
    public class AppUIElementParameterDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by   PARAMETER_ID field
            /// </summary>
            PARAMETER_ID,
            /// <summary>
            /// Search by   PARAMETER_ID field
            /// </summary>
            PARAMETER_ID_LIST,
            /// <summary>
            /// Search by   PARAMETER_NAME field
            /// </summary>
            PARAMETER_NAME,
            /// <summary>
            /// Search by   PARAMETER field
            /// </summary>
            PARAMETER,
            /// <summary>
            /// Search by   UI_PANEL_ELEMENT_ID field
            /// </summary>
            UI_PANEL_ELEMENT_ID,
            /// <summary>
            /// Search by   UI_PANEL_ELEMENT_ID field
            /// </summary>
            UI_PANEL_ELEMENT_ID_LIST,
            /// <summary>
            /// Search by   ACTIVE_FLAG field
            /// </summary>
            ACTIVE_FLAG,
            /// <summary>
            /// Search by   PARENT_PARAMETER_ID field
            /// </summary>
            PARENT_PARAMETER_ID,
            /// <summary>
            /// Search by   ACTION_SCREEN_ID field
            /// </summary>
            ACTION_SCREEN_ID,
            /// <summary>
            /// Search by   SCREEN_GROUP field
            /// </summary>
            SCREEN_GROUP,
            /// <summary>
            /// Search by   SQL_BIND_NAME field
            /// </summary>
            SQL_BIND_NAME,
            /// <summary>
            /// Search by site_id field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID
        }

        private int parameterId;
        private string parameterName;
        private string parameter;
        private int uiPanelElementId;
        private int displayIndex;
        private bool activeFlag;
        private string sqlBindName;
        private int parentParameterId;
        private int masterEntityId;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private int siteId;
        private string guid;
        private bool synchStatus;
        private int screenGroup;
        private int actionScreenId;
        private string createdBy;
        private DateTime creationDate;
        private List<AppUIElementParameterAttributeDTO> appUIElementParameterAttributeDTOList;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        /// <summary>
        /// Default constructor
        /// </summary>
        public AppUIElementParameterDTO()
        {
            log.LogMethodEntry();
            parameterId = -1;
            parentParameterId = -1;
            uiPanelElementId = -1;
            siteId = -1;
            activeFlag = true;
            masterEntityId = -1;
            actionScreenId = -1;
            displayIndex = -1;
            appUIElementParameterAttributeDTOList = new List<AppUIElementParameterAttributeDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with Required data fields
        /// </summary>
        public AppUIElementParameterDTO(int parameterId, string parameterName, string parameter, int uIPanelElementId, int displayIndex, bool activeFlag,
            string sqlBindName, int parentParameterId, int screenGroup, int actionScreenId)
            :this()
        {
            log.LogMethodEntry(parameterId, parameterName, parameter, uIPanelElementId, displayIndex, activeFlag, sqlBindName,
            parentParameterId,screenGroup, actionScreenId);
            this.parameterId = parameterId;
            this.parameterName = parameterName;
            this.parameter = parameter;
            this.uiPanelElementId = uIPanelElementId;
            this.displayIndex = displayIndex;
            this.activeFlag = activeFlag;
            this.sqlBindName = sqlBindName;
            this.parentParameterId = parentParameterId;
            this.screenGroup = screenGroup;
            this.actionScreenId = actionScreenId;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public AppUIElementParameterDTO(int parameterId, string parameterName, string parameter, int uIPanelElementId, int displayIndex, bool activeFlag,
            string sqlBindName, int parentParameterId, int masterEntityId, string lastUpdatedBy, DateTime lastUpdatedDate, int siteId,
            string guid, bool synchStatus, int screenGroup, int actionScreenId, string createdBy, DateTime creationDate)
            :this(parameterId, parameterName, parameter, uIPanelElementId, displayIndex, activeFlag, sqlBindName,
                  parentParameterId, screenGroup, actionScreenId)
        {
            log.LogMethodEntry(parameterId, parameterName, parameter, uIPanelElementId, displayIndex, activeFlag, sqlBindName,
            parentParameterId, masterEntityId, lastUpdatedBy, lastUpdatedDate, siteId, guid, synchStatus, screenGroup, actionScreenId, createdBy, creationDate);
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
        /// Get/Set method of the ParameterId field
        /// </summary>
        public int ParameterId
        {
            get { return parameterId; }
            set { this.IsChanged = true; parameterId = value; }
        }
        /// <summary>
        /// Get/Set method of the ParameterName field
        /// </summary>
        public string ParameterName
        {
            get { return parameterName; }
            set { this.IsChanged = true; parameterName = value; }
        }
        /// <summary>
        /// Get/Set method of the Parameter field
        /// </summary>
        public string Parameter
        {
            get { return parameter; }
            set { this.IsChanged = true; parameter = value; }
        }
        /// <summary>
        /// Get/Set method of the UIPanelElementId field
        /// </summary>
        public int UIPanelElementId
        {
            get { return uiPanelElementId; }
            set { this.IsChanged = true; uiPanelElementId = value; }
        }
        /// <summary>
        /// Get/Set method of the DisplayIndex field
        /// </summary>
        public int DisplayIndex
        {
            get { return displayIndex; }
            set { this.IsChanged = true; displayIndex = value; }
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
        /// Get/Set method of the SQLBindName field
        /// </summary>
        public string SQLBindName
        {
            get { return sqlBindName; }
            set { this.IsChanged = true; sqlBindName = value; }
        }
        /// <summary>
        /// Get/Set method of the ParentParemeterId field
        /// </summary>
        public int ParentParameterId
        {
            get { return parentParameterId; }
            set { this.IsChanged = true; parentParameterId = value; }
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
            set {  lastUpdatedDate = value; }
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
        /// Get/Set method of the ScreenGroup field
        /// </summary>
        public int ScreenGroup
        {
            get { return screenGroup; }
            set { this.IsChanged = true; screenGroup = value; }
        }
        /// <summary>
        /// Get/Set method of the ActionScreenId field
        /// </summary>
        public int ActionScreenId
        {
            get { return actionScreenId; }
            set { this.IsChanged = true; actionScreenId = value; }
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
        /// Get/Set method of the  appUIElementParameterAttributeDTOList field
        /// </summary>
        public List<AppUIElementParameterAttributeDTO> AppUIElementParameterAttributeDTOList
        {
            get { return appUIElementParameterAttributeDTOList; }
            set { appUIElementParameterAttributeDTOList = value; }
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
                    return notifyingObjectIsChanged || parameterId < 0;
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
        /// Returns whether the  AppUIElementParameter changed or any of its childeren are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (AppUIElementParameterAttributeDTOList != null &&
                   AppUIElementParameterAttributeDTOList.Any(x => x.IsChanged))
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
            log.LogMethodExit(null);
        }
    }
}
