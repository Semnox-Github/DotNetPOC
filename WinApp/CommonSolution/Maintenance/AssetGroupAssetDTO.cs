/********************************************************************************************
 * Project Name - Asset Group Asset
 * Description  - Data object of Asset group asset
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *1.00        24-Dec-2015   Raghuveera          Created 
 *1.00        18-Jul-2016   Raghuveera          Modified 
 *2.70        05-Jul-2019   Dakshakh raj        Modified(Added Parameterized constructor)
 *2.80        10-May-2020   Girish Kundar       Modified: REST API Changes    
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.Maintenance
{

    /// <summary>
    /// This is the asset group asset data object class. This acts as data holder for the asset group asset business object
    /// </summary>

    public class AssetGroupAssetDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByAssetGroupAssetParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>

        public enum SearchByAssetGroupAssetParameters
        {
            /// <summary>
            /// Search by ASSET GROUP ASSET ID field
            /// </summary>
            ASSET_GROUP_ASSET_ID,
            /// <summary>
            /// Search by ASSET GROUP ID field
            /// </summary>
            ASSET_GROUP_ID,
            /// <summary>
            /// Search by ASSET ID field
            /// </summary>
            ASSET_ID,
            /// <summary>
            /// Search by ACTIVE FLAG field
            /// </summary>
            ACTIVE_FLAG,
            /// <summary>//starts:Modification on 18-Jul-2016 for publish feature
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID//Ends:Modification on 18-Jul-2016 for publish feature
        }
        private int assetGroupAssetId;
        private int assetGroupId;
        private int assetId;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;//Modification on 18-Jul-2016 for publish feature

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>

        public AssetGroupAssetDTO()
        {
            log.LogMethodEntry();
            assetGroupAssetId = -1;
            assetGroupId = -1;
            assetId = -1;
            isActive = true;
            siteId = -1;
            masterEntityId = -1;//Modification on 18-Jul-2016 for publish feature
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with Required fields
        /// </summary>

        public AssetGroupAssetDTO(int assetGroupAssetId, int assetGroupId, int assetId,bool isActive)
            : this()
        {
            log.LogMethodEntry(assetGroupAssetId, assetGroupId, assetId, isActive);
            this.assetGroupAssetId = assetGroupAssetId;
            this.assetGroupId = assetGroupId;
            this.assetId = assetId;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public AssetGroupAssetDTO(int assetGroupAssetId, int assetGroupId, int assetId, bool isActive, string createdBy,
                                  DateTime creationDate, string lastUpdatedBy, DateTime lastUpdatedDate, string guid,
                                  int siteId, bool synchStatus, int masterEntityId)//Modification on 18-Jul-2016 for publish feature
            :this(assetGroupAssetId, assetGroupId, assetId, isActive)
        {
            log.LogMethodEntry(); 
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;//Modification on 18-Jul-2016 for publish feature
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the AssetGroupAssetId field
        /// </summary>
        [DisplayName("Asset Group Asset Id")]
        [ReadOnly(true)]
        public int AssetGroupAssetId { get { return assetGroupAssetId; } set { assetGroupAssetId = value; this.IsChanged = true; } }
        
        /// <summary>
        /// Get/Set method of the AssetGroupId field
        /// </summary>
        [DisplayName("Asset Group")]
        public int AssetGroupId { get { return assetGroupId; } set { assetGroupId = value; this.IsChanged = true; } }
        
        /// <summary>
        /// Get/Set method of the AssetId field
        /// </summary>
        [DisplayName("Asset")]
        public int AssetId { get { return assetId; } set { assetId = value; this.IsChanged = true; } }
        
        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("Active?")]
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }
                     
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [Browsable(false)]
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }
        
        /// <summary>
        /// Get/Set method of the Siteid field
        /// </summary>
        [Browsable(false)]
        public int Siteid { get { return siteId; } set { siteId = value; this.IsChanged = true; } }
        
        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [Browsable(false)]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; this.IsChanged = true; } }

        /// <summary>//starts:Modification on 18-Jul-2016 for publish feature
        /// Get/Set method of the MasterEntityId field
        /// </summary>        
        [DisplayName("MasterEntityId")]
        [Browsable(false)]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; } }//Ends:Modification on 18-Jul-2016 for publish feature

        /// <summary>
        /// Get/Set method of the LastUpdatedUserId field
        /// </summary>
        [DisplayName("Last Updated By")]
        [Browsable(false)]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; this.IsChanged = true; } }

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
                IsChanged = true;
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
                IsChanged = true;
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
                IsChanged = true;
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
                    return notifyingObjectIsChanged || assetGroupAssetId < 0;
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
