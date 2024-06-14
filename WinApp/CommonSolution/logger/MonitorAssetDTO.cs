/********************************************************************************************
 * Project Name - Monitor Asset DTO
 * Description  - Data object of monitor asset 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        18-Feb-2016   Raghuveera          Created 
 *2.90        18-Jun-2020    Mushahid Faizan    Modified : 3 tier changes for Rest API.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.logger
{
    /// <summary>
    /// This is the monitor asset data object class. This acts as data holder for the monitor asset business object
    /// </summary>
    public class MonitorAssetDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        /// <summary>
        /// SearchByMonitorAssetParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByMonitorAssetParameters
        {
            /// <summary>
            /// Search by ASSET_ID field
            /// </summary>
            ASSET_ID,
            /// <summary>
            /// Search by HOST_NAME field
            /// </summary>
            HOST_NAME,
            /// <summary>
            /// Search by IP_ADDRESS field
            /// </summary>
            IP_ADDRESS,
            /// <summary>
            /// Search by MAC_ADDRESS field
            /// </summary>
            MAC_ADDRESS,
            /// <summary>
            /// Search by NAME field
            /// </summary>
            NAME,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by ASSET_TYPE_ID field
            /// </summary>
            ASSET_TYPE_ID,
            /// <summary>
            /// Search by ISACTIVE field
            /// </summary>
            ISACTIVE
        }

        private int assetId;
        private string name;
        private int assetTypeId;
        private string hostname;
        private string iPAddress;
        private string macAddress;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private DateTime creationDate;
        private int siteId;
        private string guid;
        private bool synchStatus;
        private bool isActive;
        private int masterEntityId;
        private string createdBy;

        /// <summary>
        /// Default constructor
        /// </summary>
        public MonitorAssetDTO()
        {
            log.LogMethodEntry();
            this.assetId = -1;
            this.assetTypeId = -1;
            this.isActive = true;
            this.masterEntityId = -1;
            log.LogMethodExit();
        }

        public MonitorAssetDTO(int assetId, string name, int assetTypeId, string hostname, string iPAddress, string macAddress, bool isActive)
        {
            log.LogMethodEntry(assetId, name, assetTypeId, hostname, iPAddress, macAddress, isActive);
            this.assetId = assetId;
            this.name = name;
            this.assetTypeId = assetTypeId;
            this.hostname = hostname;
            this.iPAddress = iPAddress;
            this.macAddress = macAddress;
            this.isActive = isActive;
            log.LogMethodExit();
        }
        /// <summary>         
        /// Constructor with all the data fields
        /// </summary>
        public MonitorAssetDTO(int assetId, string name, int assetTypeId, string hostname, string iPAddress,
                                string macAddress, string lastUpdatedBy, DateTime lastUpdatedDate, int siteId,
                                string guid, bool synchStatus, bool isActive, int masterEntityId, DateTime creationDate, string createdBy) : this(assetId, name, assetTypeId, hostname, iPAddress, macAddress, isActive)
        {
            log.LogMethodEntry(assetId, name, assetTypeId, hostname, iPAddress, macAddress, lastUpdatedBy, lastUpdatedDate, siteId, guid, synchStatus, isActive);
            //this.assetId = assetId;
            //this.name = name;
            //this.assetTypeId = assetTypeId;
            //this.hostname = hostname;
            //this.iPAddress = iPAddress;
            //this.macAddress = macAddress;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            // this.isActive = isActive;
            this.masterEntityId = masterEntityId;
            this.creationDate = creationDate;
            this.createdBy = createdBy;
            log.LogMethodExit();
        }
        /// <summary>
        /// Get/Set method of the AssetId field
        /// </summary>
        [DisplayName("Asset Id")]
        [ReadOnly(true)]
        public int AssetId { get { return assetId; } set { assetId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Name field
        /// </summary>
        [DisplayName("Name")]
        public string Name { get { return name; } set { name = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the AssetTypeId field
        /// </summary>
        [DisplayName("Asset Type")]
        public int AssetTypeId { get { return assetTypeId; } set { assetTypeId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Hostname field
        /// </summary>
        [DisplayName("Host Name")]
        public string Hostname { get { return hostname; } set { hostname = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the IPAddress field
        /// </summary>
        [DisplayName("IP Address")]
        public string IPAddress { get { return iPAddress; } set { iPAddress = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the MacAddress field
        /// </summary>
        [DisplayName("Mac Address")]
        public string MacAddress { get { return macAddress; } set { macAddress = value; this.IsChanged = true; } }
        /// <summary>
        /// Get method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("Updated By")]
        [Browsable(false)]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }
        /// <summary>
        /// Get method of the LastupdatedDate field
        /// </summary>
        [DisplayName("Updated Date")]
        [Browsable(false)]
        public DateTime LastupdatedDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value; } }
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
        /// Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("Is Active")]
        [Browsable(false)]
        public bool IsActive { get { return isActive; } set { isActive = value; } }
        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>

        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }

        [Browsable(false)]
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }

        [Browsable(false)]
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }

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
                    return notifyingObjectIsChanged || assetId < 0;
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
