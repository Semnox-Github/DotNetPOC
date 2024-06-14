/********************************************************************************************
 * Project Name - Product
 * Description  - CustomerProfilingDTO
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.150.0      24-Mar-2022     Girish Kundar              Created : Check in check out changes
 ********************************************************************************************/
using System;

namespace Semnox.Parafait.Product
{
    public class CustomerProfilingDTO
    {
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int customerProfilingId;
        private int customerProfilingGroupId;
        private int profileType;
        private string profileTypeName;
        private string compareOperator;
        private decimal? profileValue;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;

        public enum SearchByParameters
        {
            /// <summary>
            /// Search By customerProfileId
            /// </summary>
            CUSTOMER_PROFILE_ID,
            /// <summary>
            /// Search By customerProfileGroupId
            /// </summary>
            CUSTOMER_PROFILE_GROUP_ID,
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
        public CustomerProfilingDTO()
        {
            log.LogMethodEntry();
            customerProfilingGroupId = -1;
            customerProfilingId = -1;
            profileType = -1;
            profileTypeName = string.Empty;
            compareOperator = string.Empty;
            profileValue = null;
            siteId = -1;
            masterEntityId = -1;
            isActive = true;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields.
        /// </summary>
        public CustomerProfilingDTO(int customerProfileId, int customerProfileGroupId, int profileType,
                                    string compareOperator, decimal? profileValue, bool isActive)
          : this()
        {
            log.LogMethodEntry(customerProfileId, customerProfileGroupId, profileType, profileValue, compareOperator, isActive);
            this.customerProfilingId = customerProfileId;
            this.customerProfilingGroupId = customerProfileGroupId;
            this.profileType = profileType;
            this.compareOperator = compareOperator;
            this.profileValue = profileValue;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with Required  data fields
        /// </summary>
        public CustomerProfilingDTO(CustomerProfilingDTO customerProfilingGroupDTO)
        {
            log.LogMethodEntry(customerProfilingGroupDTO);
            this.customerProfilingGroupId = customerProfilingGroupDTO.CustomerProfilingGroupId;
            this.customerProfilingId = customerProfilingGroupDTO.CustomerProfilingId;
            this.profileType = customerProfilingGroupDTO.profileType;
            this.profileTypeName = customerProfilingGroupDTO.ProfileTypeName;
            this.compareOperator = customerProfilingGroupDTO.CompareOperator;
            this.profileValue = customerProfilingGroupDTO.ProfileValue;
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
        public CustomerProfilingDTO(int customerProfileId, int customerProfileGroupId, int profileType, string compareOperator,
                                     decimal? profileValue , bool isActive, string createdBy, DateTime creationDate,
                                     string lastUpdatedBy,DateTime lastUpdateDate, string guid, int siteId, bool synchStatus, int masterEntityId)
                : this(customerProfileId, customerProfileGroupId, profileType, compareOperator, profileValue, isActive)
        {
            log.LogMethodEntry(customerProfileId, customerProfileGroupId, profileType, compareOperator,profileValue, isActive, createdBy,
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
        /// Get/Set method of the customerProfileId field
        /// </summary>
        public int CustomerProfilingId { get { return customerProfilingId; } set { this.IsChanged = true; customerProfilingId = value; } }
        /// <summary>
        /// Get/Set method of the CustomerProfileGroupId field
        /// </summary>
        public int CustomerProfilingGroupId { get { return customerProfilingGroupId; } set { this.IsChanged = true; customerProfilingGroupId = value; } }
        /// <summary>
        /// Get/Set method of the profileType field
        /// </summary>
        public int ProfileType { get { return profileType; } set { this.IsChanged = true; profileType = value; } }
        /// <summary>
        /// Get/Set method of the profileTypeName field
        /// </summary>
        public string ProfileTypeName { get { return profileTypeName; } set { this.IsChanged = true; profileTypeName = value; } }
        /// <summary>
        /// Get/Set method of the CompareOperator field
        /// </summary>
        public string CompareOperator { get { return compareOperator; } set { this.IsChanged = true; compareOperator = value; } }
       /// <summary>
        /// Get/Set method of the CompareOperator field
        /// </summary>
        public decimal? ProfileValue { get { return profileValue; } set { this.IsChanged = true; profileValue = value; } }
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
        /// Get/Set method to track changes to the object
        /// </summary>
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || CustomerProfilingGroupId < 0;
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
