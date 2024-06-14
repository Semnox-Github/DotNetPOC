/********************************************************************************************
 * Project Name - Product
 * Description  - CustomerProfilingGroupDTO
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.150.0      24-Mar-2022     Girish Kundar              Created : Check in check out changes
 ********************************************************************************************/
using System;
using System.Collections.Generic;

namespace Semnox.Parafait.Product
{
    public class CustomerProfilingGroupDTO
    {
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int customerProfileGroupId;
        private string groupName;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;
        private List<CustomerProfilingDTO> customerProfilingDTOList;

        //Summary Fields
        private int? ageUpperLimit;
        private int? ageLowerLimit;

        public enum SearchByParameters
        {
            /// <summary>
            /// Search By customerProfileGroupId
            /// </summary>
            CUSTOMER_PROFILE_GROUP_ID,
            /// <summary>
            /// Search By groupName 
            /// </summary>
            GROUP_NAME,
            /// <summary>
            /// Search By ACTIVE FLAG
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search By SITE ID
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search By MASTER ENTITY ID
            /// </summary>
            MASTER_ENTITY_ID
        }

        /// <summary>
        /// default constructor
        /// </summary>
        public CustomerProfilingGroupDTO() 
        {
            log.LogMethodEntry();
            customerProfileGroupId = -1;
            groupName = string.Empty;
            siteId = -1;
            masterEntityId = -1;
            isActive = true;
            ageUpperLimit = null;
            ageLowerLimit = null;
            customerProfilingDTOList = new List<CustomerProfilingDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields.
        /// </summary>
        public CustomerProfilingGroupDTO(int customerProfileGroupId, string groupName, bool isActive)
          : this()
        {
            log.LogMethodEntry(customerProfileGroupId, groupName, isActive);
            this.customerProfileGroupId = customerProfileGroupId;
            this.groupName = groupName;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with Required  data fields
        /// </summary>
        public CustomerProfilingGroupDTO(CustomerProfilingGroupDTO customerProfilingGroupDTO)
        {
            log.LogMethodEntry(customerProfilingGroupDTO);
            this.customerProfileGroupId = customerProfilingGroupDTO.CustomerProfilingGroupId;
            this.groupName = customerProfilingGroupDTO.GroupName;
            this.isActive = customerProfilingGroupDTO.IsActive;
            this.createdBy = customerProfilingGroupDTO.CreatedBy;
            this.creationDate = customerProfilingGroupDTO.CreationDate;
            this.lastUpdatedBy = customerProfilingGroupDTO.LastUpdatedBy;
            this.lastUpdateDate = customerProfilingGroupDTO.LastUpdateDate;
            this.siteId = customerProfilingGroupDTO.SiteId;
            this.masterEntityId = customerProfilingGroupDTO.MasterEntityId;
            this.synchStatus = customerProfilingGroupDTO.SynchStatus;
            this.guid = customerProfilingGroupDTO.Guid;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with All data fields.
        /// </summary>
        public CustomerProfilingGroupDTO(int customerProfileGroupId, string groupName, bool isActive, string createdBy, DateTime creationDate, string lastUpdatedBy,
                                          DateTime lastUpdateDate, string guid, int siteId, bool synchStatus, int masterEntityId)
                  : this(customerProfileGroupId, groupName, isActive)
        {
            log.LogMethodEntry(customerProfileGroupId, groupName, isActive, createdBy,
                                creationDate, lastUpdatedBy, lastUpdateDate, guid, siteId, synchStatus, masterEntityId);
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
        /// Get/Set method of the CustomerProfileGroupId field
        /// </summary>
        public int CustomerProfilingGroupId { get { return customerProfileGroupId; } set { this.IsChanged = true; customerProfileGroupId = value; } }
        /// <summary>
        /// Get/Set method of the GroupName field
        /// </summary>
        public string GroupName { get { return groupName; } set { this.IsChanged = true; groupName = value; } }
        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        public bool IsActive { get { return isActive; } set { this.IsChanged = true; isActive = value; } }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }

        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        public DateTime LastUpdateDate { get { return lastUpdateDate; } set { lastUpdateDate = value; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        public int SiteId { get { return siteId; } set { siteId = value; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId { get { return masterEntityId; } set { this.IsChanged = true; masterEntityId = value; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid { get { return guid; } set { this.IsChanged = true; guid = value; } }

        /// <summary>
        /// Get/Set method of the CustomerProfilingDTO field
        /// </summary>
        public List<CustomerProfilingDTO> CustomerProfilingDTOList { get { return customerProfilingDTOList; } set { this.IsChanged = true; customerProfilingDTOList = value; } }

        /// <summary>
        /// Get/Set method of the AgeUpperLimit field
        /// </summary>
        public int? AgeUpperLimit
        {
            get { return ageUpperLimit; }
            set { ageUpperLimit = value; }
        }
        /// <summary>
        /// Get/Set method of the ageLowerLimit field
        /// </summary>
        public int? AgeLowerLimit
        {
            get { return ageLowerLimit; }
            set { ageLowerLimit = value; }
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
                    return notifyingObjectIsChanged || customerProfileGroupId < 0;
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
