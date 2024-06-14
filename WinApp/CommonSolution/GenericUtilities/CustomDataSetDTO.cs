/********************************************************************************************
 * Project Name - CustomDataSet DTO
 * Description  - Data object of CustomDataSet
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By              Remarks          
 *********************************************************************************************
 *1.00        15-May-2017   Lakshminarayana          Created 
 *2.60.2      10-Jun-2019   Akshay Gulaganji         Code merge from Development to WebManagementStudio
 *2.70.2        25-Jul-2019   Dakshakh raj             Modified : Added Parameterized costrustor. 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// This is the CustomDataSet data object class. This acts as data holder for the CustomDataSet business object
    /// </summary>
    public class CustomDataSetDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        /// 
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  CustomDataSetID field
            /// </summary>
            CUSTOM_DATA_SET_ID,
            
            /// <summary>
            /// Search by  CustomDataSetID List field
            /// </summary>
            CUSTOM_DATA_SET_ID_LIST,
           
            /// <summary>
            /// Search by site_id field
            /// </summary>
            SITE_ID,
           
            /// <summary>
            /// Search by MasterEntityId field
            /// </summary>
            MASTER_ENTITY_ID
        }

        private int customDataSetId;
        private string dummy;


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

        List<CustomDataDTO> customDataDTOList;

        /// <summary>
        /// Default constructor
        /// </summary>
        public CustomDataSetDTO()
        {
            log.LogMethodEntry();
            customDataSetId = -1;
            masterEntityId = -1;
            dummy = string.Empty;
            siteId = -1;
            customDataDTOList = new List<CustomDataDTO>();
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public CustomDataSetDTO(int customDataSetId, string dummy)
            : this()
        {
            log.LogMethodEntry(customDataSetId, dummy);
            this.customDataSetId = customDataSetId;
            this.dummy = dummy;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public CustomDataSetDTO(int customDataSetId, string dummy, string createdBy,
                                DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate,
                                int siteId, int masterEntityId, bool synchStatus, string guid)
            : this(customDataSetId, dummy)
        {
            log.LogMethodEntry(createdBy, creationDate, lastUpdatedBy, lastUpdateDate,
                               siteId, masterEntityId, synchStatus, guid);
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
        /// Get/Set method of the CustomDataSetId field
        /// </summary>
        [DisplayName("Id")]
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
        /// Get/Set method of the Name field
        /// </summary>
        [DisplayName("Dummy")]
        public string Dummy
        {
            get
            {
                return dummy;
            }

            set
            {
                this.IsChanged = true;
                dummy = value;
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
        /// Get/Set method of the LastUpdatedDate field
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
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || (customDataSetId < 0);
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
        /// Returns whether customdataset or any child record is changed
        /// </summary>
        [Browsable(false)]
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (customDataDTOList != null &&
                    customDataDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Get/Set method of the CustomDataDTOList field
        /// </summary>
        public List<CustomDataDTO> CustomDataDTOList
        {
            get
            {
                return customDataDTOList;
            }

            set
            {
                customDataDTOList = value;
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
