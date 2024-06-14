/********************************************************************************************
 * Project Name - Asset Type DTO
 * Description  - Data object of asset type
 * 
 **************
 **Version Log
 **************
 *Version      Date          Modified By       Remarks          
 *********************************************************************************************
 *1.00        23-Dec-2015    Raghuveera        Created 
 *2.60        09-May-2019   Mehraj              Modified
 *2.70        04-Jul-2019    Dakshakh raj      Modified(Added parameterized constructor) 
 ********************************************************************************************/

using System;
using System.ComponentModel;

namespace Semnox.Parafait.Maintenance
{
    /// <summary>
    /// This is the Asset Type data object class. This acts as data holder for the Asset Type business object
    /// </summary>
    public class AssetTypeDTO : IChangeTracking
    {
        private static readonly Semnox.Parafait.logging.Logger log = new  Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// SearchByAssetTypeParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByAssetTypeParameters
        {
            /// <summary>
            /// Search by ASSETTYPE ID field
            /// </summary>
            ASSETTYPE_ID,
            /// <summary>
            /// Search by ASSETTYPE NAME field
            /// </summary>
            ASSETTYPE_NAME,
            /// <summary>
            /// Search by ACTIVE FLAG field
            /// </summary>
            ACTIVE_FLAG,
            /// <summary>
            /// Search by LASTUPDATEDDATE field
            /// </summary>
            LASTUPDATEDDATE,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID
        }
        private int assetTypeId;
        private string name;
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
        public AssetTypeDTO()
        {
            log.LogMethodEntry();
            assetTypeId = -1;
            masterEntityId = -1;
            isActive = true;
            siteId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with Required fields
        /// </summary>
        public AssetTypeDTO(int assetTypeId, string name, bool isActive,string createdBy, DateTime creationDate,string lastUpdatedBy,DateTime lastUpdatedDate,string guid)
            : this()
        {
            log.LogMethodEntry(assetTypeId,name,isActive,createdBy,creationDate,lastUpdatedBy,lastUpdatedDate,guid);
            this.assetTypeId = assetTypeId;
            this.name = name;
            this.isActive =isActive;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.guid = guid;

            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public AssetTypeDTO(int assetTypeId, string name, int masterEntityId, bool isActive, string createdBy, DateTime creationDate,
                            string lastUpdatedBy, DateTime lastUpdatedDate, string guid, int siteId, bool synchStatus)
            :this(assetTypeId, name, isActive, createdBy, creationDate, lastUpdatedBy, lastUpdatedDate, guid)
        {
            log.LogMethodEntry(assetTypeId,name,masterEntityId,isActive,createdBy,creationDate,lastUpdatedBy,lastUpdatedDate,guid,siteId,synchStatus);

            this.assetTypeId = assetTypeId;
            this.name = name;
            this.masterEntityId = masterEntityId;
            this.isActive = isActive;
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
        /// Get/Set method of the AssetTypeId field
        /// </summary>
        [DisplayName("Type Id")]
        [ReadOnly(true)]
        public int AssetTypeId { get { return assetTypeId; } set { assetTypeId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Asset name field
        /// </summary>        
        [DisplayName("Type Name")]
        public string Name { get { return name; } set { name = value; this.IsChanged = true; } }

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
        /// Get method of the CreatedBy field
        /// </summary>
        [DisplayName("Created By")]
        [Browsable(false)]
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }

        /// <summary>
        /// Get method of the CreationDate field
        /// </summary>
        [DisplayName("Creation Date")]
        [Browsable(false)]
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }

        /// <summary>
        /// Get method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("Updated By")]
        [Browsable(false)]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }

        /// <summary>
        /// Get method of the LastUpdatedDate field
        /// </summary>
        [DisplayName("Updated Date")]
        [Browsable(false)]
        public DateTime LastUpdatedDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        [Browsable(false)]
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("SiteId")]
        [Browsable(false)]
        public int SiteId { get { return siteId; } set { siteId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("SynchStatus")]
        [Browsable(false)]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; this.IsChanged = true; } }

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
