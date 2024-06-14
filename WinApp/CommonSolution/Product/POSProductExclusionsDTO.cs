/********************************************************************************************
 * Project Name - POSProductExclusions DTO
 * Description  - Data object of POSProductExclusions 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.60.0      08-Mar-2019   Archana                 Created 
 *2.60.2      10-Jun-2019   Akshay Gulaganji        Code merge from Development to WebManagementStudio
 *2.80        10-Mar-2020   Vikas Dwivedi           Modified as per the Standards for RESTApi Phase 1 changes.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// This is the POSProductExclusion data object class. This acts as data holder for the POSProductExclusion business object
    /// </summary>
    public class POSProductExclusionsDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  Exclusion id field
            /// </summary>
            EXCLUSION_ID,
            /// <summary>
            /// Search by Name field
            /// </summary>
            POS_MACHINE_ID,
            /// <summary>
            /// Search by Id list field
            /// </summary>
            POS_MACHINE_ID_LIST,
            /// <summary>
            /// Search by ProductDisplayGroupFormat id field
            /// </summary>
            PRODUCT_DISPLAY_GROUP_FORMAT_ID,
            /// <summary>
            /// Search by POS type id field
            /// </summary>
            POS_TYPE_ID,
            /// <summary>
            /// Search by IsActive field
            /// </summary>
            ACTIVE_FLAG,
            /// <summary>
            /// Search by site_id field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MasterEntityId field
            /// </summary>
            MASTER_ENTITY_ID
        }

        private int exclusionId;
        private int posMachineId;
        private bool isActive;
        private string productGroup;
        private int productDisplayGroupFormatId;
        private int posTypeId;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private int siteId;
        private int masterEntityId;
        private List<ProductDisplayGroupFormatDTO> posProductDisplayGroupFormatList;
        private bool synchStatus;
        private string guid;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor 
        /// </summary>
        public POSProductExclusionsDTO()
        {
            log.LogMethodEntry();
            exclusionId = -1;
            masterEntityId = -1;
            productDisplayGroupFormatId = -1;
            posMachineId = -1;
            posTypeId = -1;
            isActive = true;
            siteId = -1;
            posProductDisplayGroupFormatList = new List<ProductDisplayGroupFormatDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public POSProductExclusionsDTO(int exclusionId, int posMachineId, string productGroup, bool isActive, int posTypeId, int productDisplayGroupFormatId, string createdBy,
                        DateTime creationDate, string lastUpdatedBy, DateTime lastUpdatedDate, int siteId,
                         int masterEntityId, bool synchStatus, string guid)
        {
            log.LogMethodEntry(exclusionId, posMachineId, productGroup, isActive, posTypeId, productDisplayGroupFormatId, createdBy, creationDate, lastUpdatedBy, lastUpdatedDate, siteId, masterEntityId, synchStatus, guid);
            this.exclusionId = exclusionId;
            this.posMachineId = posMachineId;
            this.posTypeId = posTypeId;
            this.productDisplayGroupFormatId = productDisplayGroupFormatId;
            this.productGroup = productGroup;
            this.isActive = isActive;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.siteId = siteId;
            this.masterEntityId = masterEntityId;
            this.synchStatus = synchStatus;
            this.guid = guid;
            this.posProductDisplayGroupFormatList = new List<ProductDisplayGroupFormatDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>
        [DisplayName("SI#")]
        [ReadOnly(true)]
        public int ExclusionId
        {
            get
            {
                return exclusionId;
            }

            set
            {
                this.IsChanged = true;
                exclusionId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Name field
        /// </summary>
        [DisplayName("PosMachineId")]
        public int PosMachineId
        {
            get
            {
                return posMachineId;
            }

            set
            {
                this.IsChanged = true;
                posMachineId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the PosTypeId field
        /// </summary>
        [DisplayName("PosTypeId")]
        public int PosTypeId
        {
            get
            {
                return posTypeId;
            }

            set
            {
                this.IsChanged = true;
                posTypeId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the ProductDisplayGroupFormatId field
        /// </summary>
        [DisplayName("ProductDisplayGroupFormatId")]
        public int ProductDisplayGroupFormatId
        {
            get
            {
                return productDisplayGroupFormatId;
            }

            set
            {
                this.IsChanged = true;
                productDisplayGroupFormatId = value;
            }
        }


        /// <summary>
        /// Get/Set method of the ProductGroup field
        /// </summary>
        [DisplayName("ProductGroup")]
        public string ProductGroup
        {
            get
            {
                return productGroup;
            }

            set
            {
                this.IsChanged = true;
                productGroup = value;
            }
        }


        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("Active")]
        public bool IsActive
        {
            get
            {
                return isActive;
            }

            set
            {
                this.IsChanged = true;
                isActive = value;
            }
        }
        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        [Browsable(false)]
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
        [Browsable(false)]
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
        [Browsable(false)]
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
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        [Browsable(false)]
        public DateTime LastUpdatedDate
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
        [Browsable(false)]
        public int SiteId
        {
            get
            {
                return siteId;
            }
            set
            {
                siteId = value;
            }
        }
        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [Browsable(false)]
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
        [Browsable(false)]
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
        [Browsable(false)]
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
        /// Get/Set method of the PosProductDisplayList field
        /// </summary>
        [Browsable(false)]
        public List<ProductDisplayGroupFormatDTO> PosProductDisplayGroupFormatList
        {
            get
            {
                return posProductDisplayGroupFormatList;
            }

            set
            {
                posProductDisplayGroupFormatList = value;
            }
        }

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
                    return notifyingObjectIsChanged || exclusionId < 0;
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
        /// Returns whether the POSProductExclusionsDTO changed or any of its posProductDisplayGroupFormatList  are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (posProductDisplayGroupFormatList != null &&
                   posProductDisplayGroupFormatList.Any(x => x.IsChanged))
                {
                    return true;
                }
                return false;
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
