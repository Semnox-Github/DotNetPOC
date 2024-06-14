/********************************************************************************************
 * Project Name - CustomAttributeValueList DTO
 * Description  - Data object of CustomAttributeValueList
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By              Remarks          
 *********************************************************************************************
 *1.00        15-May-2017   Lakshminarayana          Created 
 *2.50.0      12-dec-2018   Guru S A                 Who column changes
 *2.70.2        25-Jul-2019   Dakshakh raj             Modified : Added Parameterized costrustor. 
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// This is the CustomAttributeValueList data object class. This acts as data holder for the CustomAttributeValueList business object
    /// </summary>
    public class CustomAttributeValueListDTO
    {
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        /// 
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  ValueId field
            /// </summary>
            VALUE_ID,
           
            /// <summary>
            /// Search by Value field
            /// </summary>
            VALUE,
            
            /// <summary>
            /// Search by CustomAttributeId field
            /// </summary>
            CUSTOM_ATTRIBUTE_ID,
            
            /// <summary>
            /// Search by CustomAttributeId field
            /// </summary>
            CUSTOM_ATTRIBUTE_ID_LIST,
            
            /// <summary>
            /// Search by site id field
            /// </summary>
            SITE_ID,
           
            /// <summary>
            /// Search by MasterEntityId field
            /// </summary>
            MASTER_ENTITY_ID,
           
            /// <summary>
            /// Search by IsActive field
            /// </summary>
            IS_ACTIVE
        }

        private int valueId;
        private string value;
        private int customAttributeId;
        private string isDefault;

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

        /// <summary>
        /// Default constructor
        /// </summary>
        public CustomAttributeValueListDTO()
        {
            log.LogMethodEntry();
            valueId = -1;
            masterEntityId = -1;
            customAttributeId = -1;
            isDefault = "N";
            isActive = true;
            siteId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public CustomAttributeValueListDTO(int valueId, string value, int customAttributeId, string isDefault, bool isActive)
            :this()
        {
            log.LogMethodEntry(valueId, value, customAttributeId, isDefault, isActive);
            this.valueId = valueId;
            this.value = value;
            this.customAttributeId = customAttributeId;
            this.isDefault = isDefault;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public CustomAttributeValueListDTO(int valueId, string value, int customAttributeId, string isDefault, string createdBy,
                                           DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate,
                                           int siteId, int masterEntityId, bool synchStatus, string guid, bool isActive)
            :this(valueId, value, customAttributeId, isDefault, isActive)
        {
            log.LogMethodEntry(createdBy, creationDate, lastUpdatedBy, lastUpdateDate, siteId, masterEntityId, synchStatus, guid);
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
        /// Get/Set method of the ValueId field
        /// </summary>
        [DisplayName("Id")]
        [ReadOnly(true)]
        public int ValueId
        {
            get
            {
                return valueId;
            }

            set
            {
                this.IsChanged = true;
                valueId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Value field
        /// </summary>
        [DisplayName("Value")]
        public string Value
        {
            get
            {
                return value;
            }

            set
            {
                this.IsChanged = true;
                this.value = value;
            }
        }

        /// <summary>
        /// Get/Set method of the CustomAttributeId field
        /// </summary>
        [DisplayName("Custom Attribute Id")]
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
        /// Get/Set method of the IsDefault field
        /// </summary>
        [DisplayName("IsDefault")]
        public string IsDefault
        {
            get
            {
                return isDefault;
            }

            set
            {
                this.IsChanged = true;
                isDefault = value;
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
                lock(notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || valueId < 0;
                }
            }

            set
            {
                lock(notifyingObjectIsChangedSyncRoot)
                {
                    if(!Boolean.Equals(notifyingObjectIsChanged, value))
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
            log.LogMethodExit(null);
        }
    }
}
    