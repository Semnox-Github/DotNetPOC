/********************************************************************************************
 * Project Name - Asset Group DTO
 * Description  - Data object of asset group
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *1.00        23-Dec-2015   Raghuveera          Created 
 *2.70        02-Jul-2019   Dakshakh raj        Modified(Added Parameterized costrustor)  
 *2.80        10-May-2020   Girish Kundar       Modified: REST API Changes    
 ********************************************************************************************/

using System;
using System.ComponentModel;

namespace Semnox.Parafait.Maintenance
{
    /// <summary>
    /// This is the Asset Group data object class. This acts as data holder for the Asset Group business object
    /// </summary>
    public class AssetGroupDTO : IChangeTracking
    {
        private static readonly Semnox.Parafait.logging.Logger log = new  Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// SearchByAssetGroupParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByAssetGroupParameters
        {
            /// <summary>
            /// Search by AssetGroup id field
            /// </summary>
            ASSETGROUP_ID,
            /// <summary>
            /// Search by AssetGroup name field
            /// </summary>
            ASSETGROUP_NAME,
            /// <summary>
            /// Search by ACTIVE FLAG field
            /// </summary>
            ACTIVE_FLAG,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID
        }
        private int assetGroupId;
        private string assetGroupName;
        private int masterEntityId;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private string guid;
        private int siteId;
        private bool synchStatus;

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        
        /// <summary>
        /// Default constructor
        /// </summary>
        public AssetGroupDTO()
        {
            log.LogMethodEntry();
            assetGroupId = -1;
            masterEntityId = -1;
            isActive = true;
            siteId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with Required fields
        /// </summary>
        public AssetGroupDTO(int assetGroupId, string assetGroupName, bool isActive)
            : this()
        {
            log.LogMethodEntry(assetGroupId,assetGroupName,isActive);
            this.assetGroupId = assetGroupId;
            this.assetGroupName=assetGroupName;
            this.isActive=isActive;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public AssetGroupDTO(int assetGroupId, string assetGroupName, int masterEntityId, bool isActive, string createdBy, DateTime creationDate,
                               string lastUpdatedBy, DateTime lastUpdatedDate, string guid, int siteId, bool synchStatus)
            :this(assetGroupId, assetGroupName, isActive)
        {
            log.LogMethodEntry(assetGroupId,assetGroupName,masterEntityId,isActive,createdBy,creationDate,lastUpdatedBy,lastUpdatedDate,guid,siteId,synchStatus);
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.guid = guid;
            this.siteId = siteId;            
            this.synchStatus = synchStatus;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the AssetGroupId field
        /// </summary>        
        [DisplayName("Group Id")]
        [ReadOnly(true)]
        public int AssetGroupId { get { return assetGroupId; } set { assetGroupId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the AssetGroupName field
        /// </summary>
        [DisplayName("Group Name")]
        public string AssetGroupName { get { return assetGroupName; } set { assetGroupName = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>        
        [DisplayName("MasterEntityId")]
        [Browsable(false)]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("Active?")]
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }

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
        /// Get/Set method of the LastUpdatedUserId field
        /// </summary>
        [DisplayName("Last Updated By")]
        [Browsable(false)]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        [Browsable(false)]
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }
        
        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("Site Id")]
        [Browsable(false)]
        public int SiteId { get { return siteId; } set { siteId = value; } }

        
        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("Synch Status")]
        [Browsable(false)]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }

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
                    return notifyingObjectIsChanged || assetGroupId < 0;
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
        /// Allowes to accept the changes
        /// </summary>
        public void AcceptChanges()
        {
            log.LogMethodEntry();
            this.IsChanged = false;
            log.LogMethodExit();
        }

    }
}
