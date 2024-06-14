/********************************************************************************************
 * Project Name - Application RemarksDTO
 * Description  - Data object of ApplicationRemarksDTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By      Remarks          
 *********************************************************************************************
 *2.70.2        25-Jul-2019   Dakshakh raj     Modified : Added Parameterized costrustor. 
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// This is the application remarks data object class. This acts as data holder for the application remarks business object
    /// </summary>
    public class ApplicationRemarksDTO
    {
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// SearchByApplicationRemarksParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByApplicationRemarksParameters
        {
            /// <summary>
            /// Search by ID field
            /// </summary>
            ID,
            
            /// <summary>
            /// Search by MODULE NAME field
            /// </summary>
            MODULE_NAME,
           
            /// <summary>
            /// Search by SOURCE NAME field
            /// </summary>
            SOURCE_NAME,
            
            /// <summary>
            /// Search by SOURCE GUID field
            /// </summary>
            SOURCE_GUID,
            
            /// <summary>
            /// Search by ACTIVE FLAG field
            /// </summary>
            ACTIVE_FLAG,
            
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID,
            
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID
        }

        private int id;
        private string moduleName;
        private string sourceName;
        private string sourceGuid;
        private string remarks;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;

        /// <summary>
        /// Default constructor
        /// </summary>
        public ApplicationRemarksDTO()
        {
            log.LogMethodEntry();
            id = -1;
            masterEntityId = -1;
            siteId = -1;
            isActive = true;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required fields
        /// </summary>
        public ApplicationRemarksDTO(int id, string moduleName, string sourceName, string sourceGuid, string remarks, bool isActive)
            :this()
        {
            log.LogMethodEntry(id, moduleName, sourceName, sourceGuid, remarks, isActive);
            this.id = id;
            this.moduleName = moduleName;
            this.sourceName = sourceName;
            this.sourceGuid = sourceGuid;
            this.remarks = remarks;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public ApplicationRemarksDTO(int id, string moduleName, string sourceName, string sourceGuid, string remarks, bool isActive,
                                     string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdatedDate,
                                     string guid, int siteId, bool synchStatus, int masterEntityId)
            :this(id, moduleName, sourceName, sourceGuid, remarks, isActive)
        {
            log.LogMethodEntry(createdBy, creationDate, lastUpdatedBy, lastUpdatedDate, guid, siteId, synchStatus, masterEntityId);
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>
        [DisplayName("Id")]
        [ReadOnly(true)]
        public int Id { get { return id; } set { id = value; this.IsChanged = true; } }
        
        /// <summary>
        /// Get/Set method of the Module Name field
        /// </summary>
        [DisplayName("Module Name")]
        [Browsable(false)]
        public string ModuleName { get { return moduleName; } set { moduleName = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Source Name field
        /// </summary>        
        [DisplayName("Source Name")]
        [Browsable(false)]
        public string SourceName { get { return sourceName; } set { sourceName = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SourceGuid field
        /// </summary>        
        [DisplayName("SourceGuid")]
        [Browsable(false)]
        public string SourceGuid { get { return sourceGuid; } set { sourceGuid = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Remarks field
        /// </summary>        
        [DisplayName("Remarks")]
        public string Remarks { get { return remarks; } set { remarks = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("Active?")]
        [Browsable(false)]
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the CreatedBy field
        /// </summary>
        [DisplayName("Created By")]
        [Browsable(false)]
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }

        /// <summary>
        /// Get method of the CreationDate field
        /// </summary>
        [DisplayName("Creation Date")]
        [Browsable(false)]
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }

        /// <summary>
        /// Get method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("Updated By")]
        //[Browsable(false)]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }

        /// <summary>
        /// Get method of the LastUpdatedDate field
        /// </summary>
        [DisplayName("Updated Date")]
        //[Browsable(false)]
        public DateTime LastUpdatedDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        [Browsable(false)]
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("SiteId")]
        [Browsable(false)]
        public int SiteId { get { return siteId; } set { siteId = value; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("SynchStatus")]
        [Browsable(false)]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value;  } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>        
        [DisplayName("MasterEntityId")]
        [Browsable(false)]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }



        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        [Browsable(false)]
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || id < 0;
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
        /// Allowes to accept the changes
        /// </summary>
        public void AcceptChanges()
        {
            log.LogMethodEntry();
            this.IsChanged = false;
            log.LogMethodExit(null);
        }
    }
}
