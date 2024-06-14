/********************************************************************************************
 * Project Name - Maintenance
 * Description  - Data Object of the AssetTechnicianMapping class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.100.0    22-Sept-2020   Mushahid Faizan         Created.
 ********************************************************************************************/
using System;

namespace Semnox.Parafait.Maintenance
{
    public class AssetTechnicianMappingDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        /// 
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by MAPID
            /// </summary>
            MAP_ID,
            /// <summary>
            /// Search by ASSETID
            /// </summary>
            ASSET_ID,
            /// <summary>
            /// Search by ASSETTYPEID
            /// </summary>
            ASSET_TYPE_ID,
            /// <summary>
            /// Search by USER_ID
            /// </summary>
            USER_ID,
            /// <summary>
            /// Search by IS_ACTIVE field
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by IS_PRIMARY field
            /// </summary>
            IS_PRIMARY,
        }

        private int mapId;
        private int assetId;
        private int assetTypeId;
        private int userId;
        private bool isActive;
        private bool isPrimary;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private int siteId;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        // AssetMapperDTO need to add

        /// <summary>
        /// Default Constructor
        /// </summary>
        public AssetTechnicianMappingDTO()
        {
            log.LogMethodEntry();
            mapId = -1;
            assetId = -1;
            assetTypeId = -1;
            userId = -1;
            masterEntityId = -1;
            isActive = true;
            isPrimary = false;
            siteId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required fields
        /// </summary>
        /// <param name="mapId"></param>
        /// <param name="assetId"></param>
        /// <param name="assetTypeId"></param>
        /// <param name="userId"></param>
        /// <param name="isActive"></param>
        /// <param name="isPrimary"></param>
        public AssetTechnicianMappingDTO(int mapId, int assetId, int assetTypeId, int userId, bool isActive, bool isPrimary)
        {
            log.LogMethodEntry(mapId, assetId, assetTypeId, userId, isActive);
            this.mapId = mapId;
            this.assetId = assetId;
            this.assetTypeId = assetTypeId;
            this.userId = userId;
            this.isActive = isActive;
            this.isPrimary = isPrimary;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the fields
        /// </summary>
        /// <param name="mapId"></param>
        /// <param name="assetId"></param>
        /// <param name="assetTypeId"></param>
        /// <param name="userId"></param>
        /// <param name="isActive"></param>
        /// <param name="isPrimary"></param>
        /// <param name="createdBy"></param>
        /// <param name="creationDate"></param>
        /// <param name="lastUpdatedBy"></param>
        /// <param name="lastUpdateDate"></param>
        /// <param name="siteId"></param>
        /// <param name="masterEntityId"></param>
        /// <param name="synchStatus"></param>
        /// <param name="guid"></param>
        public AssetTechnicianMappingDTO(int mapId, int assetId, int assetTypeId, int userId, bool isActive, bool isPrimary, string createdBy, DateTime creationDate,
                                          string lastUpdatedBy, DateTime lastUpdateDate, int siteId, int masterEntityId, bool synchStatus, string guid)
                                          : this(mapId, assetId, assetTypeId, userId, isActive, isPrimary)
        {
            log.LogMethodEntry(mapId, assetId, assetTypeId, userId, isActive, isPrimary,
                                createdBy, creationDate, lastUpdatedBy, lastUpdateDate, siteId, masterEntityId, synchStatus, guid);
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
        /// Get/Set method of the MapId field
        /// </summary>
        public int MapId { get { return mapId; } set { this.IsChanged = true; mapId = value; } }

        /// <summary>
        /// Get/Set method of the AssetId field
        /// </summary>
        public int AssetId { get { return assetId; } set { this.IsChanged = true; assetId = value; } }

        /// <summary>
        /// Get/Set method of the AssetTypeId field
        /// </summary>
        public int AssetTypeId { get { return assetTypeId; } set { this.IsChanged = true; assetTypeId = value; } }

        /// <summary>
        /// Get/Set method of the userId field
        /// </summary>
        public int UserId { get { return userId; } set { this.IsChanged = true; userId = value; } }
      
        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the IsPrimary field
        /// </summary>
        public bool IsPrimary { get { return isPrimary; } set { isPrimary = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the CreatedBy field
        /// </summary>
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }

        /// <summary>
        /// Get method of the CreationDate field
        /// </summary>
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }

        /// <summary>
        /// Get method of the LastUpdatedBy field
        /// </summary>
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }

        /// <summary>
        /// Get method of the LastUpdateDate field
        /// </summary>
        public DateTime LastUpdateDate { get { return lastUpdateDate; } set { lastUpdateDate = value; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        public int SiteId { get { return siteId; } set { siteId = value; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId { get { return masterEntityId; } set { this.IsChanged = true; masterEntityId = value; } }

        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || mapId < 0;
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
