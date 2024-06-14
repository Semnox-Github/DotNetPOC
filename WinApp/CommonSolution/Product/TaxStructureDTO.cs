/********************************************************************************************
 * Project Name -TaxStructure DTO
 * Description  - Data object of asset TaxStructure.
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *2.60        30-Jan-2019   Mushahid Faizan     Created
 *2.60        11-Apr-2019   Girish Kundar       Copied this file to Inventory Module 
 *2.70        02-Apr-2019   Akshay Gulaganji    added isActive property
 *2.70.2        21-Nov-2019   Girish Kundar       Modified - Made all members private and removed unused namespace.
 ********************************************************************************************/

using System;
using System.ComponentModel;

namespace Semnox.Parafait.Product
{

    /// <summary>
    /// This is the TaxStructure data object class. This acts as data holder for the TaxStructure business object
    /// </summary>
    public class TaxStructureDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByTaxStructureParameters
        {
            /// <summary>
            /// Search by  taxStructureId field
            /// </summary>
            TAX_STRUCTURE_ID,
            /// <summary>
            /// Search by taxId field
            /// </summary>
            TAX_ID,
            /// <summary>
            /// Search by structureName field
            /// </summary>
            STRUCTURE_NAME,

            /// <summary>
            /// Search by Percentage field
            /// </summary>
            TAX_PERCENTAGE,
            /// <summary>
            /// Search by PARENT_STRUCTURE_ID field
            /// </summary>
            PARENT_STRUCTURE_ID,
            /// <summary>
            /// Search by PARENT_STRUCTURE_ID field
            /// </summary>
            TAX_STRUCTURE_NAME,
            /// <summary>
            /// Search by site_id field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by masterEntityId field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by IS_ACTIVE field
            /// </summary>
            IS_ACTIVE
        }

      private int taxStructureId;
      private int taxId;
      private string structureName;
      private double percentage;
      private int parentStructureId;
      private string guid;
      private bool synchStatus;
      private int siteId;
      private int masterEntityId;
      private string createdBy;
      private DateTime creationDate;
      private string lastUpdatedBy;
      private DateTime lastUpdatedDate;
      private bool deleteTaxStructure;
      private bool isActive;

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public TaxStructureDTO()
        {
            log.LogMethodEntry();
            taxStructureId = -1;
            taxId = -1;
            percentage = -1;
            parentStructureId = -1;
            siteId = -1;
            masterEntityId = -1;
            deleteTaxStructure = false;
            isActive = true;
            structureName = string.Empty;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public TaxStructureDTO(int taxStructureId, int taxId, string structureName, double percentage, int parentStructureId, bool isActive)
            : this()
        {
            log.LogMethodEntry(taxStructureId, taxId, structureName, percentage, parentStructureId, isActive);
            this.taxStructureId = taxStructureId;
            this.taxId = taxId;
            this.structureName = structureName;
            this.percentage = percentage;
            this.parentStructureId = parentStructureId;
            deleteTaxStructure = false;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public TaxStructureDTO(int taxStructureId, int taxId, string structureName, double percentage, int parentStructureId, string guid, bool synchStatus, int siteId,
                               int masterEntityId, string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdatedDate, bool isActive)
            : this(taxStructureId, taxId, structureName, percentage, parentStructureId, isActive)
        {
            log.LogMethodEntry(taxStructureId, taxId, structureName, percentage, parentStructureId, guid, synchStatus, siteId, masterEntityId, createdBy, creationDate,
                              lastUpdatedBy, lastUpdatedDate, isActive);
            this.taxStructureId = taxStructureId;
            this.taxId = taxId;
            this.structureName = structureName;
            this.percentage = percentage;
            this.parentStructureId = parentStructureId;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.siteId = siteId;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            deleteTaxStructure = false;
            this.isActive = isActive;
            log.LogMethodExit();
        }
        /// <summary>
        /// Get/Set method of the StructureName field
        /// </summary>
        public bool IsActive
        {
            get { return isActive; }
            set
            {
                this.IsChanged = true;
                isActive = value;
            }
        }


        /// <summary>
        /// Get/Set method of the StructureName field
        /// </summary>
        public string StructureName
        {
            get { return structureName; }
            set
            {
                this.IsChanged = true;
                structureName = value;
            }
        }
        /// <summary>
        /// Get/Set method of the ParentStructureId field
        /// </summary>
        public int ParentStructureId
        {
            get { return parentStructureId; }
            set
            {
                this.IsChanged = true;
                parentStructureId = value;
            }
        }
        /// <summary>
        /// Get/Set method of the Percentage field
        /// </summary>
        public double Percentage
        {
            get { return percentage; }
            set
            {
                this.IsChanged = true;
                percentage = value;
            }
        }
        /// <summary>
        /// Get/Set method of the TaxId field
        /// </summary>
        public int TaxId
        {
            get { return taxId; }
            set
            {
                this.IsChanged = true;
                taxId = value;
            }
        }
        /// <summary>
        /// Get/Set method of the TaxStructureId field
        /// </summary>
        public int TaxStructureId
        {
            get { return taxStructureId; }
            set
            {
                this.IsChanged = true;
                taxStructureId = value;
            }
        }
        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        public string CreatedBy
        {
            get
            {
                return createdBy;
            }
            set
            {
                createdBy = value;
            }
        }
        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        public DateTime CreationDate
        {
            get
            {
                return creationDate;
            }
            set
            {
                creationDate = value;
            }
        }
        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        public string LastUpdatedBy
        {
            get
            {
                return lastUpdatedBy;
            }
            set
            {
                lastUpdatedBy = value;
            }
        }
        /// <summary>
        /// Get/Set method of the LastUpdateDate field
        /// </summary>
        public DateTime LastUpdateDate
        {
            get
            {
                return lastUpdatedDate;
            }
            set
            {
                lastUpdatedDate = value;
            }
        }
        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        public int site_Id
        {
            get
            {
                return siteId;
            }
            set
            {
                this.IsChanged = true;
                siteId = value;
            }
        }
        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId
        {
            get
            {
                return masterEntityId;
            }

            set
            {
                this.IsChanged = true;
                masterEntityId = value;
            }
        }
        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        public bool SynchStatus
        {
            get
            {
                return synchStatus;
            }
            set
            {
                synchStatus = value;
            }
        }
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid
        {
            get
            {
                return guid;
            }
            set
            {
                guid = value;
            }
        }

        /// <summary>
        /// Set method of the DeleteTaxStructure field
        /// </summary>
        public bool DeleteTaxStructure
        {
            get
            {
                return deleteTaxStructure;
            }
            set
            {
                deleteTaxStructure = value;
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
                    return notifyingObjectIsChanged || taxStructureId < 0;
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