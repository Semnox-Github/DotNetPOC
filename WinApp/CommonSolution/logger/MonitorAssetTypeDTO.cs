/********************************************************************************************
 * Project Name - Monitor Asset Type DTO
 * Description  - Data object of monitor asset type 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *1.00        23-Feb-2016   Raghuveera          Created 
 *2.70        16-Jul-2019   Dakshakh raj        Modified : Added Parameterized costrustor,
 *                                                         WHO columns, 
 *                                                         MasterEntityId to searchparameter
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.logger
{
    /// <summary>
    /// This is the monitor asset type data object class. This acts as data holder for the monitor asset type business object
    /// </summary>
    public class MonitorAssetTypeDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
       
        /// <summary>
        /// SearchByMonitorAssetParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByMonitorAssetTypeParameters
        {
            /// <summary>
            /// Search by ASSET TYPE ID field
            /// </summary>
            ASSET_TYPE_ID,
            /// <summary>
            /// Search by ASSET TYPE NAME field
            /// </summary>
            ASSET_TYPE_NAME,
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTERENTITYID field
            /// </summary>
            MASTERENTITYID

        }
        private int assetTypeId;
        private string assetType;
        private int siteId;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;

        /// <summary>
        /// Default constructor
        /// </summary>
        public MonitorAssetTypeDTO()
        {
            log.LogMethodEntry();
            assetTypeId = -1;
            siteId = -1;
            masterEntityId = -1;
            log.LogMethodExit();
        }
        
        /// <summary>
        /// Constructor with required fields
        /// </summary>
        public MonitorAssetTypeDTO(int assetTypeId, string assetType)
            :this()
        {
            log.LogMethodEntry(assetTypeId, assetType);
            this.assetTypeId = assetTypeId;
            this.assetType = assetType;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public MonitorAssetTypeDTO(int assetTypeId, string assetType, int siteId, string guid, bool synchStatus, int masterEntityId, string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate)
            : this(assetTypeId, assetType)
        {
            log.LogMethodEntry( siteId,  guid,  synchStatus,  masterEntityId,  createdBy,  creationDate,  lastUpdatedBy,  lastUpdateDate);
            this.siteId = siteId;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the AssetTypeId field
        /// </summary>
        [DisplayName("Type Id")]
        [ReadOnly(true)]
        public int AssetTypeId { get { return assetTypeId; } set { assetTypeId = value; this.IsChanged = true; } }
        
        /// <summary>
        /// Get/Set method of the assetType field
        /// </summary>
        [DisplayName("Name")]
        public string AssetType { get { return assetType; } set { assetType = value; this.IsChanged = true; } }
        
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        [Browsable(false)]
        public string Guid { get { return guid; } set { guid = value; } }
        
        /// <summary>
        /// Get/Set method of the Siteid field
        /// </summary>
        [DisplayName("Site id")]
        [Browsable(false)]
        public int Siteid { get { return siteId; } set { siteId = value; } }
        
        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("Synch Status")]
        [Browsable(false)]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }

        /// <summary>
        /// Get/Set method of the createdBy fields
        /// </summary>
        public string CreatedBy { get { return createdBy; } set { createdBy = value; IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CreatedDate fields
        /// </summary>
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; IsChanged = true; } }
        
        /// <summary>
        /// Get/Set method of the lastUpdatedBy fields
        /// </summary>
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the lastUpdatedAt fields
        /// </summary>
        public DateTime LastUpdateDate { get { return lastUpdateDate; } set { lastUpdateDate = value; IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }
        
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
                    return notifyingObjectIsChanged || assetTypeId < 0;
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
