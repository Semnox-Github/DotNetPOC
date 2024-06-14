/********************************************************************************************
 * Project Name - Site DTO
 * Description  - Data object of site  
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        29-Feb-2016   Raghuveera          Created 
 *1.00        28-Jun-2016   Raghuveera          Modified
 *2.60        15-May-2019   Nitin Pai           Modified for Guest Appp
 *2.70.2        23-Jul-2019   Deeksha             Modifications as per three tier standard.
 *            26-Jul-2019    Mushahid Faizan     Added Logo column              
 *2.100.0     24-Sept-2020   Mushahid Faizan     Modified for Service Request Enhancement       
 *2.110.0     01-Feb-2021    Girish Kundar       Modified : Urban Piper changes
 *2.140.0     21-Jun-2021    Fiona Lishal        Modified for Delivery Order enhancements for F&B and Urban Piper
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Semnox.Parafait.Site
{
    /// <summary>
    /// This is the site data object class. This acts as data holder for the site business object
    /// </summary>
    public class SiteDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        /// <summary>
        /// SearchBySiteParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchBySiteParameters
        {
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID ,
            /// <summary>
            /// Search by SITE NAME field
            /// </summary>
            SITE_NAME,
            /// <summary>
            /// Search by ORG ID field
            /// </summary>
            ORG_ID ,
            /// <summary>//Starts: Modification on 28-Jun-2016 for added site code for filter option.
            /// Search by SITE CODE field
            /// </summary>
            SITE_CODE,//Ends:Modification on 28-Jun-2016 for added site code for filter option.
            /// <summary>
            /// Search only MASTER SITE field
            /// </summary>
            SHOW_ONLY_MASTER_SITE ,
            /// <summary>
            /// Search only GUID field
            /// </summary>
            GUID ,
            /// <summary>
            /// Search only IS ACTIVE field
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search only MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search only ONLINE ENABLED field
            /// </summary>
            ONLINE_ENABLED,
            /// <summary>
            /// Search by LAST UPDATED DATE field
            /// </summary>
            LAST_UPDATED_DATE,
            /// <summary>
            /// Search only SITE_GUID field
            /// </summary>
            SITE_GUID,
        }

        private int siteId;
        private string siteName;
        private string siteAddress;
        private string notes;
        private string siteGuid;
        private string guid;
        private string version;
        private int orgId;
        private DateTime lastUploadTime;
        private DateTime lastUploadServerTime;
        private string lastUploadMessage;
        private string initialLoadDone;
        private string maxCards;
        private string customerKey;
        private int companyId;
        private bool synchStatus;
        private int siteCode;
        private bool isMasterSite;
        private string onlineEnabled;
        private string siteShortName;       
        private string aboutUs;
        private bool isActive;
        private DateTime creationDate;
        private String createdBy;
        private DateTime lastUpdateDate;
        private String lastUpdatedBy;
        private string description;
        private DateTime openDate;
        private DateTime closureDate;
        private String siteURL;
        private int size;
        private String storeType;
        private String city;
        private String state;
        private String country;
        private String pinCode;
        private Decimal openTime;
        private Decimal closeTime;
        private int masterEntityId;
        private byte[] logo;
        private string storeRanking;
        private string externalSourceReference;
        private string  email;
        private string phoneNumber;
        private decimal? latitude;
        private decimal? longitude;
        private string siteConfigComplete;

        private List<SiteDetailDTO> siteDetailDTOList;
        /// <summary>
        /// Default constructor
        /// </summary>
        public SiteDTO()
        {
            log.LogMethodEntry();
            orgId = -1;
            siteId = -1;
            masterEntityId = -1;
            companyId = -1;
            siteCode = -1;
            isActive = true;
            isMasterSite = false;
			onlineEnabled = "N";
            siteConfigComplete = "N";
            aboutUs = string.Empty;
            pinCode = string.Empty;
            siteURL = string.Empty;
            email = string.Empty;
            phoneNumber = string.Empty;
            openTime = 9.00M;
            openTime = 23.00M;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public SiteDTO(int siteId, string siteName, string siteAddress, string notes, string siteGuid, 
                        string version, int orgId, DateTime lastUploadTime, DateTime lastUploadServerTime, string lastUploadMessage,
                        string initialLoadDone, string maxCards, string customerKey, int companyId, int siteCode, 
                        bool isMasterSite, string siteShortName, string onlineEnabled, string aboutUs, string pinCode, bool isActive,
                        string description, DateTime openDate, DateTime closureDate, String siteURL, int size, String storeType, 
                        String city, String state, String country, Decimal openTime, Decimal closeTime, byte[] logo, string storeRanking,string externalSourceReference, string email, string phoneNumber, decimal? latitude, decimal? longitude,
                        string siteConfigComplete)
            :this()
        {
            log.LogMethodEntry(siteId, siteName, siteAddress, notes, siteGuid, version, orgId, lastUploadTime,
                                lastUploadServerTime, lastUploadMessage, initialLoadDone, maxCards, customerKey, companyId,
                                  siteCode, isMasterSite, siteShortName, onlineEnabled, aboutUs, pinCode, isActive,
                                 description, openDate, closureDate,siteURL, size, storeType, city, state, country, openTime, closeTime, logo, storeRanking, externalSourceReference, email, phoneNumber, latitude, longitude, siteConfigComplete);
            this.siteId = siteId;
            this.siteName = siteName;
            this.siteAddress = siteAddress;
            this.notes = notes;
            this.siteGuid = siteGuid;            
            this.version = version;
            this.orgId = orgId;
            this.lastUploadTime = lastUploadTime;
            this.lastUploadServerTime = lastUploadServerTime;
            this.lastUploadMessage = lastUploadMessage;
            this.initialLoadDone = initialLoadDone;
            this.maxCards = maxCards;
            this.customerKey = customerKey;
            this.companyId = companyId;           
            this.siteCode = siteCode;
            this.isMasterSite = isMasterSite;
			this.siteShortName = siteShortName;
			this.onlineEnabled = onlineEnabled;            
            this.aboutUs = aboutUs;
            this.pinCode = pinCode;
            this.isActive = isActive;          
            this.description = description;
            this.openDate = openDate;
            this.closureDate = closureDate;
            this.siteURL = siteURL;
            this.size = size;
            this.storeType = storeType;
            this.city = city;
            this.state = state;
            this.country = country;
            this.openTime = openTime;
            this.closeTime = closeTime;           
            this.logo = logo;
            this.storeRanking = storeRanking;
            this.externalSourceReference = externalSourceReference;
            this.email = email;
            this.phoneNumber = phoneNumber;
            this.latitude = latitude;
            this.longitude = longitude;
            this.siteConfigComplete = siteConfigComplete;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public SiteDTO(int siteId, string siteName, string siteAddress, string notes, string siteGuid, string guid,
                        string version, int orgId, DateTime lastUploadTime, DateTime lastUploadServerTime, string lastUploadMessage,
                        string initialLoadDone, string maxCards, string customerKey, int companyId, bool synchStatus, int siteCode, bool isMasterSite, string siteShortName,
                        string onlineEnabled, string aboutUs, string pinCode, bool isActive, DateTime creationDate, String createdBy, DateTime lastUpdateDate,
                        String lastUpdatedBy, string description, DateTime openDate, DateTime closureDate, String siteURL, int size, String storeType, String city, String state,
                        String country, Decimal openTime, Decimal closeTime, int masterEntityId, byte[] logo, string storeRanking,string externalSourceReference, string email, string phoneNumber,
                        decimal? latitude, decimal? longitude, string siteConfigComplete)
            :this(siteId, siteName, siteAddress, notes, siteGuid, version, orgId, lastUploadTime,
                                lastUploadServerTime, lastUploadMessage, initialLoadDone, maxCards, customerKey, companyId,
                                  siteCode, isMasterSite, siteShortName, onlineEnabled, aboutUs, pinCode, isActive,
                                 description, openDate, closureDate, siteURL, size, storeType, city, state, country, openTime, closeTime, logo, storeRanking, externalSourceReference, email, phoneNumber,
                                 latitude, longitude, siteConfigComplete)
        {
            log.LogMethodEntry(siteId, siteName, siteAddress, notes, siteGuid, guid, version, orgId, lastUploadTime,
                                lastUploadServerTime, lastUploadMessage, initialLoadDone, maxCards, customerKey, companyId,
                                 synchStatus, siteCode, isMasterSite, siteShortName, onlineEnabled, aboutUs, pinCode, isActive,
                                  creationDate, createdBy, lastUpdateDate, lastUpdatedBy, description, openDate, closureDate,
                                  siteURL, size, storeType, city, state, country, openTime, closeTime, masterEntityId, logo, storeRanking, externalSourceReference, email, phoneNumber, latitude, longitude, siteConfigComplete);
            this.guid = guid;
            this.creationDate = creationDate;
            this.createdBy = createdBy;
            this.lastUpdateDate = lastUpdateDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("Site Id")]
        [ReadOnly(true)]
        public int SiteId { get { return siteId; } set { siteId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the SiteName field
        /// </summary>
        [DisplayName("Site Name")]
        public string SiteName { get { return siteName; } set { siteName = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the SiteAddress field
        /// </summary>
        [DisplayName("Site Address")]
        public string SiteAddress { get { return siteAddress; } set { siteAddress = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Notes field
        /// </summary>
        [DisplayName("Notes")]
        public string Notes { get { return notes; } set { notes = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Site Guid field
        /// </summary>
        [DisplayName("Site Guid")]
        public string SiteGuid { get { return siteGuid; } set { siteGuid = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Version field
        /// </summary>
        [DisplayName("Version")]
        public string Version { get { return version; } set { version = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the OrgId field
        /// </summary>
        [DisplayName("OrgId")]
        public int OrgId { get { return orgId; } set { orgId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the LastUploadTime field
        /// </summary>
        [DisplayName("Last Upload Time")]
        public DateTime LastUploadTime { get { return lastUploadTime; } set { lastUploadTime = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the LastUploadServerTime field
        /// </summary>
        [DisplayName("Last Upload Server Time")]
        public DateTime LastUploadServerTime { get { return lastUploadServerTime; } set { lastUploadServerTime = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the LastUploadMessage field
        /// </summary>
        [DisplayName("Last Upload Message")]
        public string LastUploadMessage { get { return lastUploadMessage; } set { lastUploadMessage = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the InitialLoadDone field
        /// </summary>
        [DisplayName("Initial Load Done")]
        public string InitialLoadDone { get { return initialLoadDone; } set { initialLoadDone = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the SiteAddress field
        /// </summary>
        [DisplayName("Max Cards")]
        public string MaxCards { get { return maxCards; } set { maxCards = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the CustomerKey field
        /// </summary>
        [DisplayName("Customer Key")]
        public string CustomerKey { get { return customerKey; } set { customerKey = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the SiteConfigComplete field
        /// </summary>
        [DisplayName("Site Config Complete")]
        public string SiteConfigComplete { get { return siteConfigComplete; } set { siteConfigComplete = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the CompanyId field
        /// </summary>
        [DisplayName("Company Id")]
        public int CompanyId { get { return companyId; } set { companyId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Site code field
        /// </summary>
        [DisplayName("Site code")]
        public int SiteCode { get { return siteCode; } set { siteCode = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("Synch Status")]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }

        /// <summary>
        /// Get/Set method of the IsMasterSite field
        /// </summary>
        [DisplayName("IsMasterSite")]
        public bool IsMasterSite { get { return isMasterSite; } set { isMasterSite = value; this.IsChanged = true; } }

		/// Get/Set method of the SiteShortName field
		/// </summary>
		[DisplayName("SiteShortName")]
		public string SiteShortName { get { return siteShortName; } set { siteShortName = value; this.IsChanged = true; } }

		/// <summary>
		/// Get/Set method of the OnlineEnabled field
		/// </summary>
		[DisplayName("OnlineEnabled")]
		public string OnlineEnabled { get { return onlineEnabled; } set { onlineEnabled = value; this.IsChanged = true; } }

        /// <summary>
		/// Get/Set method of the AboutUs field
		/// </summary>
		[DisplayName("AboutUs")]
        public string AboutUs { get { return aboutUs; } set { aboutUs = value; this.IsChanged = true; } }
        /// <summary>
		/// Get/Set method of the PinCode field
		/// </summary>
		[DisplayName("PinCode")]
        public string PinCode { get { return pinCode; } set { pinCode = value; this.IsChanged = true; } }
        
         /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("IsActive")]
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        [DisplayName("CreationDate")]
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        [DisplayName("CreatedBy")]
        public String CreatedBy { get { return createdBy; } set { createdBy = value; } }

        /// <summary>
        /// Get/Set method of the LastUpdateDate field
        /// </summary>
        [DisplayName("LastUpdateDate")]
        public DateTime LastUpdateDate { get { return lastUpdateDate; } set { lastUpdateDate = value; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("LastUpdatedBy")]
        public String LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }

        /// <summary>
        /// Get/Set method of the description field
        /// </summary>
        [DisplayName("Description")]
        public String Description { get { return description; } set { description = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the OpenDate field
        /// </summary>
        [DisplayName("OpenDate")]
        public DateTime OpenDate { get { return openDate; } set { openDate = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CloseDate field
        /// </summary>
        [DisplayName("CloseDate")]
        public DateTime CloseDate { get { return closureDate; } set { closureDate = value; this.IsChanged = true; }  }

        /// <summary>
		/// Get/Set method of the SiteURL field
		/// </summary>
		[DisplayName("SiteURL")]
        public string SiteURL { get { return siteURL; } set { siteURL = value; this.IsChanged = true; } }

        /// <summary>
		/// Get/Set method of the Size field
		/// </summary>
		[DisplayName("Size")]
        public int Size { get { return size; } set { size = value; this.IsChanged = true; } }

        /// <summary>
		/// Get/Set method of the StoreType field
		/// </summary>
		[DisplayName("StoreType")]
        public string StoreType { get { return storeType; } set { storeType = value; this.IsChanged = true; } }

        /// <summary>
		/// Get/Set method of the City field
		/// </summary>
		[DisplayName("City")]
        public string City { get { return city; } set { city = value; this.IsChanged = true; } }

        /// <summary>
		/// Get/Set method of the State field
		/// </summary>
		[DisplayName("State")]
        public string State { get { return state; } set { state = value; this.IsChanged = true; } }

        /// <summary>
		/// Get/Set method of the Country field
		/// </summary>
		[DisplayName("Country")]
        public string Country { get { return country; } set { country = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the StoreRanking field
        /// </summary>
        [DisplayName("StoreRanking")]
        public string StoreRanking { get { return storeRanking; } set { storeRanking = value; this.IsChanged = true; } }

        /// <summary>
		/// Get/Set method of the OpenTime field
		/// </summary>
		[DisplayName("OpenTime")]
        public Decimal OpenTime { get { return openTime; } set { openTime = value; this.IsChanged = true; } }

        /// <summary>
		/// Get/Set method of the CloseTime field
		/// </summary>
		[DisplayName("CloseTime")]
        public Decimal CloseTime { get { return closeTime; } set { closeTime = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the MasterEntityId
        /// </summary>
        [DisplayName("MasterEntityId")]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }


        public string ExternalSourceReference { get { return externalSourceReference; } set { externalSourceReference = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId
        /// </summary>
        [DisplayName("SiteDetailDTOList")]
        public List<SiteDetailDTO> SiteDetailDTOList { get { return siteDetailDTOList; } set { siteDetailDTOList = value;} }
        /// <summary>
        /// Get/Set method of the PhoneNumber
        /// </summary>
        public string PhoneNumber { get { return phoneNumber; } set { phoneNumber = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Email
        /// </summary>
        public string Email {  get { return email; } set { email = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the Logo field
        /// </summary>
        [DisplayName("Logo")]
        public byte[] Logo { get { return logo; } set { logo = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the lLatitude field
        /// </summary>
        [DisplayName("Latitude")]
        public decimal? Latitude { get { return latitude; } set { latitude = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Longitude field
        /// </summary>
        [DisplayName("Longitude")]
        public decimal? Longitude { get { return longitude; } set { longitude = value; this.IsChanged = true; } }

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
                    return notifyingObjectIsChanged || siteId < 0;
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
        /// Returns whether the SiteDTO changed or any of its Child  are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (siteDetailDTOList != null &&
                   siteDetailDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                return false;
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
