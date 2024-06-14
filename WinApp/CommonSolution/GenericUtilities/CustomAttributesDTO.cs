/********************************************************************************************
 * Project Name - CustomAttributes DTO
 * Description  - Data object of CustomAttributes
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By              Remarks          
 *********************************************************************************************
 *1.00        15-May-2017   Lakshminarayana          Created 
 *2.50.0      12-dec-2018   Guru S A                 Who column changes 
 *2.70.2      25-Jul-2019   Dakshakh raj             Modified : Added Parameterized costrustor. 
 *2.80.0      30-Apr-2020   Akshay G                 Added NAME_LIST as searchParameter
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// This is the CustomAttributes data object class. This acts as data holder for the CustomAttributes business object
    /// </summary>
    public class CustomAttributesDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        /// 
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  CustomAttributesID field
            /// </summary>
            CUSTOM_ATTRIBUTE_ID,
            
            /// <summary>
            /// Search by Type field
            /// </summary>
            NAME,
            
            /// <summary>
            /// Search by Type field
            /// </summary>
            TYPE,
            
            /// <summary>
            /// Search by Applicability field
            /// </summary>
            APPLICABILITY,
           
            /// <summary>
            /// Search by Access field
            /// </summary>
            ACCESS,
           
            /// <summary>
            /// Search by site_id field
            /// </summary>
            SITE_ID,
           
            /// <summary>
            /// Search by MasterEntityId field
            /// </summary>
            MASTER_ENTITY_ID,
            
            /// <summary>
            /// Search by IsActive field
            /// </summary>
            IS_ACTIVE,

            /// <summary>
            /// Search by NAME LIST field
            /// </summary>
            NAME_LIST
        }

        private int customAttributeId;
        private string name;
        private int sequence;
        private string type;
        private string applicability;
        private string access;

        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private int siteId;
        private int masterEntityId;
        private bool synchStatus;
        private string guid;
        private bool isActive;

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

       private  List<CustomAttributeValueListDTO> customAttributeValueListDTOList;

        /// <summary>
        /// Default constructor
        /// </summary>
        public CustomAttributesDTO()
        {
            log.LogMethodEntry();
            customAttributeId = -1;
            masterEntityId = -1;
            isActive = true;
            siteId = -1;
            customAttributeValueListDTOList = new List<CustomAttributeValueListDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public CustomAttributesDTO(int customAttributeId, string name, int sequence, string type, string applicability,
                                   string access, bool isActive)
            :this()
        {
            log.LogMethodEntry(customAttributeId, name, sequence, type, applicability, access, isActive);
            this.customAttributeId = customAttributeId;
            this.name = name;
            this.sequence = sequence;
            this.type = type;
            this.applicability = applicability;
            this.access = access;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public CustomAttributesDTO(int customAttributeId, string name, int sequence, string type, string applicability,
                                   string access, string createdBy, DateTime creationDate, string lastUpdatedBy,
                                   DateTime lastUpdateDate, int siteId, int masterEntityId, bool synchStatus, string guid, bool isActive)
            :this(customAttributeId, name, sequence, type, applicability, access, isActive)
        {
            log.LogMethodEntry(customAttributeId, name, sequence, type, applicability, access, createdBy, creationDate, lastUpdatedBy, lastUpdateDate, siteId,
                               masterEntityId, synchStatus, guid, isActive);
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            this.siteId = siteId;
            this.masterEntityId = masterEntityId;
            this.synchStatus = synchStatus;
            this.guid = guid;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the CustomAttributesId field
        /// </summary>
        [DisplayName("ID")]
        [ReadOnly(true)]
        public int CustomAttributeId
        {
            get
            {
                return customAttributeId;
            }

            set
            {
                this.IsChanged = true;
                customAttributeId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Name field
        /// </summary>
        [DisplayName("Name")]
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                this.IsChanged = true;
                name = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Sequence field
        /// </summary>
        [DisplayName("Sequence")]
        public int Sequence
        {
            get
            {
                return sequence;
            }

            set
            {
                this.IsChanged = true;
                sequence = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Type field
        /// </summary>
        [DisplayName("Type")]
        public string Type
        {
            get
            {
                return type;
            }

            set
            {
                this.IsChanged = true;
                type = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Applicability field
        /// </summary>
        [DisplayName("Applicability")]
        public string Applicability
        {
            get
            {
                return applicability;
            }

            set
            {
                this.IsChanged = true;
                applicability = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Access field
        /// </summary>
        [DisplayName("Access")]
        public string Access
        {
            get
            {
                return access;
            }

            set
            {
                this.IsChanged = true;
                access = value;
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
        /// Get/Set method of the LastUpdateDate field
        /// </summary>
        [Browsable(false)]
        public DateTime LastUpdateDate
        {
            get
            {
                return lastUpdateDate;
            }
            set
            {
                lastUpdateDate = value;
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
                this.IsChanged = true;
                guid = value;
            }
        }

        /// <summary>
        /// Get/Set method of the CustomAttributeValueListDTOList field
        /// </summary>
        [Browsable(false)]
        public List<CustomAttributeValueListDTO> CustomAttributeValueListDTOList
        {
            get
            {
                return customAttributeValueListDTOList;
            }

            set
            {
                customAttributeValueListDTOList = value;
            }
        }
       
        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary> 
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
        /// Get/Set method to track changes to the object
        /// </summary>
        [Browsable(false)]
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || customAttributeId < 0;
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
        /// IsChangedRecursive
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (customAttributeValueListDTOList != null &&
                   customAttributeValueListDTOList.Any(x => x.IsChanged))
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
