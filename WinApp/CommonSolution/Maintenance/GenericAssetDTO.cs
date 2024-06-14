/********************************************************************************************
 * Project Name - Generic Asset DTO
 * Description  - Data object of the assets
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        23-Dec-2015   Raghuveera          Created 
 *2.70        07-Jul-2019   Dakshakh raj   Modified(Added Parameterized costrustor) 
 *2.80        10-May-2020   Girish Kundar  Modified: REST API Changes merge from WMS  
 ********************************************************************************************/

using System;
using System.ComponentModel;

namespace Semnox.Parafait.Maintenance
{
    /// <summary>
    /// This is the Generic Asset data object class. This acts as data holder for the Generic Asset business object
    /// </summary>
    public class GenericAssetDTO : IChangeTracking
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByAssetParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByAssetParameters
        {
            /// <summary>
            /// Search by Asset id field
            /// </summary>
            ASSET_ID,
            /// <summary>
            /// Search by ASSET_NAME field
            /// </summary>
            ASSET_NAME,
            /// <summary>
            /// Search by ASSET_TYPE_ID field
            /// </summary>
            ASSET_TYPE_ID,
            /// <summary>
            /// Search by URN field
            /// </summary>
            URN,
            /// <summary>
            /// Search by LOCATION_ID field
            /// </summary>            
            LOCATION,
            /// <summary>
            /// Search by ASSET_STATUS field
            /// </summary> 
            ASSET_STATUS,
            /// <summary>
            /// Search by ACTIVE_FLAG field
            /// </summary>
            ACTIVE_FLAG,
            /// <summary>
            /// Search by LASTUPDATEDDATE field
            /// </summary>
            LASTUPDATEDDATE,
            /// <summary>
            /// Search by MACHINEID field
            /// </summary>
            MACHINEID,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID
        }

        private int assetId;
        private string name;
        private string description;
        private int machineid;
        private int assetTypeId;
        private string location;
        private string assetStatus;
        private string urn;
        private string purchaseDate;
        private string saleDate;
        private string scrapDate;
        private int assetTaxTypeId;
        private double purchaseValue;
        private double saleValue;
        private double scrapValue;
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
        public GenericAssetDTO()
        {
            log.LogMethodEntry();
            assetId = -1;
            assetTaxTypeId = -1;
            assetTypeId = -1;
            masterEntityId = -1;
            isActive = true;
            machineid = -1;
            siteId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with Required fields
        /// </summary>
        public GenericAssetDTO(int assetId, string name, string description, int machineid,int assetTypeId,string location,string assetStatus,
             string urn,string purchaseDate,string saleDate,string scrapDate,int assetTaxTypeId,double purchaseValue,double saleValue,
             double scrapValue, bool isActive)
            : this()
        {
            log.LogMethodEntry(assetId,name,description,machineid,assetTypeId,location,assetStatus,urn,purchaseDate,saleDate,scrapDate,assetTaxTypeId,purchaseValue,saleValue,scrapValue,isActive);
            this.assetId = assetId;
            this.name = name;
            this.description = description;
            this.machineid = machineid;
            this.assetTypeId = assetTypeId;
            this.location = location;
            this.assetStatus = assetStatus;
            this.urn = urn;
            this.purchaseDate = purchaseDate;
            this.saleDate = saleDate;
            this.scrapDate = scrapDate;
            this.assetTaxTypeId = assetTaxTypeId;
            this.purchaseValue = purchaseValue;
            this.saleValue = SaleValue;
            this.scrapValue = scrapValue;
            this.isActive = isActive;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public GenericAssetDTO(int assetId, string name, string description, int machineid, int assetTypeId, string location,
                               string assetStatus, string urn, string purchaseDate, string saleDate, string scrapDate, int assetTaxTypeId,
                               double purchaseValue, double saleValue, double scrapValue, int masterEntityId, bool isActive, string createdBy,
                               DateTime creationDate, string lastUpdatedBy, DateTime lastUpdatedDate,string guid, int siteId, bool synchStatus)
            
            :this(assetId, name, description, machineid, assetTypeId, location, assetStatus, urn, purchaseDate, saleDate, scrapDate, 
                 assetTaxTypeId, purchaseValue, saleValue, scrapValue, isActive)
        {
            log.LogMethodEntry(assetId,name,description,machineid, assetTypeId,location,assetStatus, urn,purchaseDate,saleDate,
                               scrapDate,assetTaxTypeId,purchaseValue, saleValue,scrapValue, masterEntityId,isActive,createdBy,
                               creationDate,lastUpdatedBy,lastUpdatedDate,guid,siteId,synchStatus);
            this.assetId = assetId;
            this.name = name;
            this.description = description;
            this.machineid = machineid;
            this.assetTypeId = assetTypeId;
            this.location = location;
            this.assetStatus = assetStatus;
            this.urn = urn;
            this.purchaseDate = purchaseDate;
            this.saleDate = saleDate;
            this.scrapDate = scrapDate;
            this.assetTaxTypeId = assetTaxTypeId;
            this.purchaseValue = purchaseValue;
            this.saleValue = saleValue;
            this.scrapValue = scrapValue;
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
        /// Get/Set method of the AssetId field
        /// </summary>
        [DisplayName("Asset Id")]
        [ReadOnly(true)]
        public int AssetId { get { return assetId; } set { assetId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Name field
        /// </summary>
        [DisplayName("Asset Name")]
        public string Name { get { return name; } set { name = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Description field
        /// </summary>
        [DisplayName("Description")]
        public string Description { get { return description; } set { description = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Machineid field
        /// </summary>
        [DisplayName("Machine Id")]
        [ReadOnly(true)]
        public int Machineid { get { return machineid; } set { machineid = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the AssetTypeId field
        /// </summary>
        [DisplayName("Asset Type")]
        public int AssetTypeId { get { return assetTypeId; } set { assetTypeId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Location field
        /// </summary>
        [DisplayName("Location")]
        public string Location { get { return location; } set { location = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the AssetStatus field
        /// </summary>
        [DisplayName("Asset Status")]
        public string AssetStatus { get { return assetStatus; } set { assetStatus = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Barcode field
        /// </summary>
        [DisplayName("URN")]
        public string URN { get { return urn; } set { urn = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the PurchaseDate field
        /// </summary>
        [DisplayName("Purchase Date")]
        public string PurchaseDate { get { return purchaseDate; } set { purchaseDate = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SaleDate field
        /// </summary>
        [DisplayName("Sale Date")]
        public string SaleDate { get { return saleDate; } set { saleDate = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ScrapDate field
        /// </summary>
        [DisplayName("Scrap Date")]
        public string ScrapDate { get { return scrapDate; } set { scrapDate = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the AssetTaxType field
        /// </summary>
        [DisplayName("Tax Type")]
        public int AssetTaxTypeId { get { return assetTaxTypeId; } set { assetTaxTypeId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the PurchaseValue field
        /// </summary>
        [DisplayName("Purchase Value")]
        public double PurchaseValue { get { return purchaseValue; } set { purchaseValue = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SaleValue field
        /// </summary>
        [DisplayName("Sale Value")]
        public double SaleValue { get { return saleValue; } set { saleValue = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ScrapValue field
        /// </summary>
        [DisplayName("Scrap Value")]
        public double ScrapValue { get { return scrapValue; } set { scrapValue = value; this.IsChanged = true; } }

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
        /// Get method of the LastupdatedDate field
        /// </summary>
        [DisplayName("Updated Date")]
        [Browsable(false)]
        public DateTime LastUpdatedDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("GUID")]
        [Browsable(false)]
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Siteid field
        /// </summary>
        [DisplayName("Site Id")]
        [Browsable(false)]
        public int Siteid { get { return siteId; } set { siteId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("Synch Status")]
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
