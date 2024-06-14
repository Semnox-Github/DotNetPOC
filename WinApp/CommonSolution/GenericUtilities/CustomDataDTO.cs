/********************************************************************************************
 * Project Name - CustomData DTO
 * Description  - Data object of CustomData
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By              Remarks          
 *********************************************************************************************
 *1.00        15-May-2017   Lakshminarayana          Created
 *2.70.2        25-Jul-2019   Dakshakh raj             Modified : Added Parameterized costrustor. 
 * 
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// This is the CustomData data object class. This acts as data holder for the CustomData business object
    /// </summary>
    public class CustomDataDTO
    {
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        /// 
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  CustomDataID field
            /// </summary>
            CUSTOM_DATA_ID,
            
            /// <summary>
            /// Search by CustomDataSetID field
            /// </summary>
            CUSTOM_DATA_SET_ID,
            
            /// <summary>
            /// Search by CustomDataSetID List field
            /// </summary>
            CUSTOM_DATA_SET_ID_LIST,
            
            /// <summary>
            /// Search by CustomAttributeId field
            /// </summary>
            CUSTOM_ATTRIBUTE_ID,
            
            /// <summary>
            /// Search by ValueId field
            /// </summary>
            VALUE_ID,
            
            /// <summary>
            /// Search by site_id field
            /// </summary>
            SITE_ID,
            
            /// <summary>
            /// Search by MasterEntityId field
            /// </summary>
            MASTER_ENTITY_ID
        }

        private int customDataId;
        private int customDataSetId;
        private int customAttributeId;
        private int valueId;
        private string customDataText;
        private decimal? customDataNumber;
        private DateTime? customDataDate;

        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private int siteId;
        private int masterEntityId;
        private bool synchStatus;
        private string guid;

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public CustomDataDTO()
        {
            log.LogMethodEntry();
            customDataId = -1;
            customDataSetId = -1;
            customAttributeId = -1;
            valueId = -1;
            masterEntityId = -1;
            siteId = -1;
            log.LogMethodExit(null);
        }
        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public CustomDataDTO(int customDataId, int customDataSetId, int customAttributeId, int valueId, string customDataText,
                             decimal? customDataNumber, DateTime? customDataDate)
            :this()
        {
            log.LogMethodEntry(customDataId, customDataSetId, customAttributeId, valueId, customDataText, customDataNumber, customDataDate);
            this.customDataId = customDataId;
            this.customDataSetId = customDataSetId;
            this.customAttributeId = customAttributeId;
            this.valueId = valueId;
            this.customDataText = customDataText;
            this.customDataNumber = customDataNumber;
            this.customDataDate = customDataDate;
            log.LogMethodExit(null);
        }


        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public CustomDataDTO(int customDataId, int customDataSetId, int customAttributeId, int valueId, string customDataText, 
                             decimal? customDataNumber, DateTime? customDataDate, string createdBy,
                             DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate, int siteId, int masterEntityId, 
                             bool synchStatus, string guid)
            :this(customDataId, customDataSetId, customAttributeId, valueId, customDataText, customDataNumber, customDataDate)
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
            log.LogMethodExit(null);
        }

        /// <summary>
        /// Get/Set method of the CustomDataId field
        /// </summary>
        [DisplayName("ID")]
        [ReadOnly(true)]
        public int CustomDataId
        {
            get
            {
                return customDataId;
            }

            set
            {
                this.IsChanged = true;
                customDataId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the CustomDataSetId field
        /// </summary>
        [DisplayName("Custom Data Set Id")]
        public int CustomDataSetId
        {
            get
            {
                return customDataSetId;
            }

            set
            {
                this.IsChanged = true;
                customDataSetId = value;
            }
        }
        /// <summary>
        /// Get/Set method of the CustomAttributeId field
        /// </summary>
        [DisplayName("CustomAttributeId")]
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
        /// Get/Set method of the ValueId Text field
        /// </summary>
        [DisplayName("Value Id")]
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
        /// Get/Set method of the CustomDataText field
        /// </summary>
        [DisplayName("Custom Data Text")]
        public string CustomDataText
        {
            get
            {
                return customDataText;
            }

            set
            {
                this.IsChanged = true;
                customDataText = value;
            }
        }

        /// <summary>
        /// Get/Set method of the CustomDataNumber field
        /// </summary>
        [DisplayName("Custom Data Number")]
        public decimal? CustomDataNumber
        {
            get
            {
                return customDataNumber;
            }

            set
            {
                this.IsChanged = true;
                customDataNumber = value;
            }
        }

        /// <summary>
        /// Get/Set method of the CustomDataDate field
        /// </summary>
        [DisplayName("Custom Data Date")]
        public DateTime? CustomDataDate
        {
            get
            {
                return customDataDate;
            }

            set
            {
                this.IsChanged = true;
                customDataDate = value;
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
                this.IsChanged = true;
                guid = value;
            }
        }

        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        public bool IsChanged
        {
            get
            {
                lock(notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || customDataId < 0;
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
