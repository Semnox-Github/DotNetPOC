/********************************************************************************************
 * Project Name - Logger
 * Description  - Data object of MonitorAppModule
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.60        29-May-2019   Girish Kundar           Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.logger
{
    /// <summary>
    /// This is the MonitorAppModuleDTO data object class. This acts as data holder for the MonitorAppModule business object
    /// </summary>
    public class MonitorAppModuleDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by MODULE ID field
            /// </summary>
            MODULE_ID,
            /// <summary>
            /// Search by MODULE NAME field
            /// </summary>
            MODULE_NAME,
            /// <summary>
            /// Search by DESCRIPTION field
            /// </summary>
            DESCRIPTION,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID
        }
        private int moduleId;
        private string moduleName;
        private string description;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private DateTime lastUpdateDate;
        private string lastUpdatedBy;
        private int masterEntityId;
        private string createdBy;
        private DateTime creationDate;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default Constructor
        /// </summary>
        public MonitorAppModuleDTO()
        {
            log.LogMethodEntry();
            moduleId = -1;
            siteId = -1;
            masterEntityId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor required fields
        /// </summary>
        public MonitorAppModuleDTO(int moduleId, string moduleName, string description)
            : this()
        {
            log.LogMethodEntry(moduleId, moduleName, description);
            this.moduleId = moduleId;
            this.moduleName = moduleName;
            this.description = description;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor with all the fields
        /// </summary>
        public MonitorAppModuleDTO(int moduleId, string moduleName, string description, string guid, int siteId, bool synchStatus, DateTime lastUpdateDate, string lastUpdatedBy,
                                   int masterEntityId, string createdBy, DateTime creationDate)
            : this(moduleId, moduleName, description)
        {
            log.LogMethodEntry(moduleId, moduleName, description, guid, siteId, synchStatus, lastUpdateDate, lastUpdatedBy, masterEntityId, createdBy, creationDate);
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.lastUpdateDate = lastUpdateDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the ModuleId  field
        /// </summary>
        public int ModuleId
        {
            get { return moduleId; }
            set { moduleId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the ModuleName  field
        /// </summary>
        public string ModuleName
        {
            get { return moduleName; }
            set { moduleName = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the Description  field
        /// </summary>
        public string Description
        {
            get { return description; }
            set { description = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        public string LastUpdatedBy
        {
            get { return lastUpdatedBy; }
            set { lastUpdatedBy = value; }
        }
        /// <summary>
        ///  Get/Set method of the LastUpdateDate field
        /// </summary>
        public DateTime LastUpdateDate
        {
            get { return lastUpdateDate; }
            set { lastUpdateDate = value; }
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
        /// Get/Set method to track changes to the object
        /// </summary>
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || moduleId < 0;
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
            IsChanged = false;
            log.LogMethodExit();
        }
    }
}
