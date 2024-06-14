/********************************************************************************************
 * Project Name - Organization Structure DTO
 * Description  - Data object of organization structure
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By      Remarks          
 *********************************************************************************************
 *2.60        11-Mar-2019   Jagan Mohan      Created 
              29-Mar-2019   Mushahid Faizan  Added LogMethodEntry & LogMethodExit,Removed unused namespaces & enum numbering.
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.Site
{
    /// <summary>
    /// This is the Organization Structure data object class. This acts as data holder for the Organization Structure business object
    /// </summary>
    public class OrganizationStructureDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// SearchByOrganizationParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by STRUCTURE_ID field
            /// </summary>
            STRUCTURE_ID,
            /// <summary>
            /// Search by STRUCTURE_NAME field
            /// </summary>
            STRUCTURE_NAME,
            /// <summary>
            /// Search by PARENT_STRUCTURE_ID field
            /// </summary>
            PARENT_STRUCTURE_ID,
            /// <summary>
            /// Search by COMPANY_ID field
            /// </summary>
            COMPANY_ID
        }
        int structureId;
        string structureName;
        int parentStructureId;
        int companyId;
        string autoRoam;
        string createdBy;
        DateTime creationDate;
        string lastUpdatedBy;
        DateTime lastUpdateDate;

        /// <summary>
        /// Default constructor
        /// </summary>
        public OrganizationStructureDTO()
        {
            log.LogMethodEntry();
            structureId = -1;
            parentStructureId = -1;
            companyId = -1;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public OrganizationStructureDTO(int structureId, string structureName, int parentStructureId, int companyId, string autoRoam, string createdBy,
                                        DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate)
        {
            log.LogMethodEntry(structureId, structureName, parentStructureId, companyId, autoRoam, createdBy, creationDate, lastUpdatedBy, lastUpdateDate);
            this.structureId = structureId;
            this.structureName = structureName;
            this.parentStructureId = parentStructureId;
            this.companyId = companyId;
            this.autoRoam = autoRoam;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            log.LogMethodExit();
        }


        /// <summary>
        /// Get/Set method of the StructureId field
        /// </summary>
        [DisplayName("Structure Id")]
        [ReadOnly(true)]
        public int StructureId { get { return structureId; } set { structureId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the StructureName field
        /// </summary>
        [DisplayName("Structure Name")]
        public string StructureName { get { return structureName; } set { structureName = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the ParentStructureId field
        /// </summary>
        [DisplayName("Parent Structure Id")]
        public int ParentStructureId { get { return parentStructureId; } set { parentStructureId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the CompanyId field
        /// </summary>
        [DisplayName("CompanyId")]
        public int CompanyId { get { return companyId; } set { companyId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the AutoRoam field
        /// </summary>
        [DisplayName("AutoRoam")]
        public string AutoRoam { get { return autoRoam; } set { autoRoam = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        [DisplayName("Created By")]
        public string CreatedBy { get { return createdBy; } set { createdBy = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        [DisplayName("CreationDate")]
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("LastUpdatedBy")]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the LastUpdateDate field
        /// </summary>
        [DisplayName("LastUpdateDate")]
        public DateTime LastUpdateDate { get { return lastUpdateDate; } set { lastUpdateDate = value; this.IsChanged = true; } }

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
                    return notifyingObjectIsChanged || structureId < 0;
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