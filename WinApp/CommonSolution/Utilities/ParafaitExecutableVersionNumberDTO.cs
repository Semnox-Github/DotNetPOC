/********************************************************************************************
 * Project Name - Utilities
 * Description  - Data object of ParafaitExecutableVersionNumber
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70.2.0      23-Sep-2019   Mithesh                 Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Core.Utilities
{
    /// <summary>
    /// This is the ParafaitExecutableVersionNumberDTO data object class. This acts as data holder for the ParafaitExecutableVersionNumber business object
    /// </summary>
    public class ParafaitExecutableVersionNumberDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by ID field
            /// </summary>
            ID,
            /// <summary>
            /// Search by PARAFAIT EXECUTABLE NAME field
            /// </summary>
            PARAFAIT_EXECUTABLE_NAME,
            /// <summary>
            /// Search by  MAJOR VERSION field
            /// </summary>
            MAJOR_VERSION,
            /// <summary>
            /// Search by MINOR VERSION field
            /// </summary>
            MINOR_VERSION,
            /// <summary>
            /// Search by  PATCH VERSION field
            /// </summary>
            PATCH_VERSION,
            /// <summary>
            /// Search by  EXECUTABLE GENERATED AT field
            /// </summary>
            EXECUTABLE_GENERATED_AT,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID
        }

        private int id;
        private string parafaitExecutableName;
        private int majorVersion;
        private int minorVersion;
        private int patchVersion;
        private DateTime executableGeneratedAt;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ParafaitExecutableVersionNumberDTO()
        {
            log.LogMethodEntry();
            id = -1;
            majorVersion = -1;
            minorVersion = -1;
            patchVersion = -1;
            siteId = -1;
            masterEntityId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with Required fields
        /// </summary>
        public ParafaitExecutableVersionNumberDTO(int id, string parafaitExecutableName, int majorVersion, int minorVersion, int patchVersion, DateTime executableGeneratedAt)
            : this()
        {
            log.LogMethodEntry();
            this.id = id;
            this.parafaitExecutableName = parafaitExecutableName;
            this.majorVersion = majorVersion;
            this.minorVersion = minorVersion;
            this.patchVersion = patchVersion;
            this.executableGeneratedAt = executableGeneratedAt;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with all the fields
        /// </summary>
        public ParafaitExecutableVersionNumberDTO(int id, string parafaitExecutableName, int majorVersion, int minorVersion, int patchVersion, DateTime executableGeneratedAt, bool isActive, string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdatedDate, string guid, int siteId, bool synchStatus, int masterEntityId)
            : this(id, parafaitExecutableName, majorVersion, minorVersion, patchVersion, executableGeneratedAt)
        {
            log.LogMethodEntry(id, parafaitExecutableName, majorVersion, minorVersion, patchVersion, executableGeneratedAt, isActive, createdBy, creationDate, lastUpdatedBy, lastUpdatedDate, guid, siteId, synchStatus, masterEntityId);
            this.isActive = isActive;
            this.lastUpdatedDate = lastUpdatedDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the id field
        /// </summary>
        public int Id
        {
            get { return id; }
            set { id = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the ParafaitExecutableName field
        /// </summary>
        public string ParafaitExecutableName
        {
            get { return parafaitExecutableName; }
            set { parafaitExecutableName = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the MajorVersion field
        /// </summary>
        public int MajorVersion
        {
            get { return majorVersion; }
            set { majorVersion = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the MinorVersion field
        /// </summary>
        public int MinorVersion
        {
            get { return minorVersion; }
            set { minorVersion = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the PatchVersion field
        /// </summary>
        public int PatchVersion
        {
            get { return patchVersion; }
            set { patchVersion = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the ExecutableGeneratedAt field
        /// </summary>
        public DateTime ExecutableGeneratedAt
        {
            get { return executableGeneratedAt; }
            set { executableGeneratedAt = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        public string CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        public DateTime CreationDate
        {
            get { return creationDate; }
            set { creationDate = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        public string LastUpdatedBy
        {
            get { return lastUpdatedBy; }
            set { lastUpdatedBy = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the LastupdatedDate field
        /// </summary>
        public DateTime LastupdatedDate
        {
            get { return lastUpdatedDate; }
            set { lastUpdatedDate = value; this.IsChanged = true; }
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
        /// Get/Set method of the SiteId field
        /// </summary>
        public int SiteId
        {
            get { return siteId; }
            set { siteId = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        public bool SynchStatus
        {
            get { return synchStatus; }
            set { synchStatus = value; this.IsChanged = true; }
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
        /// Get/Set method to track changes to the object
        /// </summary>
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