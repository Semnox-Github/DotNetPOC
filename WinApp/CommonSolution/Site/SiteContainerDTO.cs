/********************************************************************************************
 * Project Name - Site
 * Description  - SiteContainerDTO class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         10-Nov-2020       Mushahid Faizan         Created : POS UI Redesign with REST API
*2.140.0     21-Jun-2021       Fiona Lishal            Modified for Delivery Order enhancements for F&B and Urban Piper
 *2.150.0     09-Mar-2022       Lakshminarayana         Modified : Added GetUTCDateTime() as a part of SiteDateTime Enhancement
  *2.150.2     16-Mar-2023       Abhishek                Modified : Added new field CustomerKey as part of Cloud GamePlay
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Semnox.Parafait.Site
{
    public class SiteContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int siteId;
        private string siteName;
        private string email;
        private string phoneNumber;
        private bool isMasterSite;
        private string siteAddress;
        private bool onlineEnabled;
        private String city;
        private String state;
        private String country;
        private String pinCode;
        private Decimal? openTime;
        private Decimal? closeTime;
        private string storeRanking;
        private string siteShortName;
        private String siteURL;
        private string storeType;
        private DateTime? openDate;
        private DateTime? closureDate;
        private byte[] logo;
        private int businessDayStartTime;
        private string timeZoneName;
        private decimal? latitude;
        private decimal? longitude;
        private string customerKey;
        private List<SiteDetailContainerDTO> siteDeliveryDetailsDTOList;

        public SiteContainerDTO()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public SiteContainerDTO(int siteId, string siteName, string siteAddress,
                        bool isMasterSite, bool onlineEnabled, string pinCode, string siteURL, String siteShortName,
                        String city, String state, String country, Decimal openTime, Decimal closeTime,
                        string storeRanking, string storeType, DateTime? openDate, DateTime? closureDate, 
                        byte[] logo, string email, string phoneNumber, int businessDayStartTime, string timeZoneName, decimal? latitude, decimal? longitude, string customerKey)
            : this()
        {
            log.LogMethodEntry(siteId, siteName, siteAddress,
                                   isMasterSite, onlineEnabled, pinCode,
                                  siteURL, siteShortName, city, state, country, openTime, closeTime, storeRanking, businessDayStartTime, timeZoneName, latitude, longitude, customerKey);
            this.siteId = siteId;
            this.siteName = siteName;
            this.siteAddress = siteAddress;
            this.email = email;
            this.phoneNumber = phoneNumber;
            this.isMasterSite = isMasterSite;
            this.onlineEnabled = onlineEnabled;
            this.pinCode = pinCode;
            this.siteURL = siteURL;
            this.siteShortName = siteShortName;
            this.city = city;
            this.state = state;
            this.country = country;
            this.openTime = openTime;
            this.closeTime = closeTime;
            this.storeRanking = storeRanking;
            this.storeType = storeType;
            this.openDate = openDate;
            this.closureDate = closureDate;
            this.logo = logo;
            this.businessDayStartTime = businessDayStartTime;
            this.timeZoneName = timeZoneName;
            this.latitude = latitude;
            this.longitude = longitude;
            this.customerKey = customerKey;
            log.LogMethodExit();
        }

        /// <summary>
        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("Site Id")]
        [ReadOnly(true)]
        public int SiteId { get { return siteId; } set { siteId = value; } }
        /// <summary>
        /// Get/Set method of the SiteName field
        /// </summary>
        [DisplayName("Site Name")]
        public string SiteName { get { return siteName; } set { siteName = value; } }
        /// <summary>
        /// Get/Set method of the SiteAddress field
        /// </summary>
        [DisplayName("Site Address")]
        public string SiteAddress { get { return siteAddress; } set { siteAddress = value; } }

        /// <summary>
        /// Get/Set method of the IsMasterSite field
        /// </summary>
        [DisplayName("IsMasterSite")]
        public bool IsMasterSite { get { return isMasterSite; } set { isMasterSite = value; } }

        /// <summary>
        /// Get/Set method of the OnlineEnabled field
        /// </summary>
        [DisplayName("OnlineEnabled")]
        public bool OnlineEnabled { get { return onlineEnabled; } set { onlineEnabled = value; } }

        /// <summary>
		/// Get/Set method of the PinCode field
		/// </summary>
		[DisplayName("PinCode")]
        public string PinCode { get { return pinCode; } set { pinCode = value; } }

        /// <summary>
        /// Get/Set method of the siteURL field
        /// </summary>
        [DisplayName("siteURL")]
        public string SiteURL { get { return siteURL; } set { siteURL = value; } }

        /// <summary>
		/// Get/Set method of the siteShortName field
		/// </summary>
		[DisplayName("SiteShortName")]
        public string SiteShortName { get { return siteShortName; } set { siteShortName = value; } }

        /// <summary>
		/// Get/Set method of the City field
		/// </summary>
		[DisplayName("City")]
        public string City { get { return city; } set { city = value; } }

        /// <summary>
		/// Get/Set method of the State field
		/// </summary>
		[DisplayName("State")]
        public string State { get { return state; } set { state = value; } }

        /// <summary>
		/// Get/Set method of the Country field
		/// </summary>
		[DisplayName("Country")]
        public string Country { get { return country; } set { country = value; } }

        /// <summary>
        /// Get/Set method of the StoreType field
        /// </summary>
        [DisplayName("StoreType")]
        public string StoreType { get { return storeType; } set { storeType = value; } }

        /// <summary>
        /// Get/Set method of the Logo field
        /// </summary>
        [DisplayName("Logo")]
        public byte[] Logo { get { return logo; } set { logo = value; } }

        /// <summary>
        /// Get/Set method of the StoreRanking field
        /// </summary>
        [DisplayName("StoreRanking")]
        public string StoreRanking { get { return storeRanking; } set { storeRanking = value; } }

        /// <summary>
		/// Get/Set method of the OpenTime field
		/// </summary>
		[DisplayName("OpenTime")]
        public Decimal? OpenTime { get { return openTime; } set { openTime = value; } }

        /// <summary>
		/// Get/Set method of the CloseTime field
		/// </summary>
		[DisplayName("CloseTime")]
        public Decimal? CloseTime { get { return closeTime; } set { closeTime = value; } }

        /// <summary>
        /// Get/Set method of the OpenDate field
        /// </summary>
        [DisplayName("OpenDate")]
        public DateTime? OpenDate { get { return openDate; } set { openDate = value; } }  
        
        /// <summary>
		/// Get/Set method of the CloseTime field
		/// </summary>
		[DisplayName("CloseTime")]
        public DateTime? ClosureDate { get { return closureDate; } set { closureDate = value; } }

        /// <summary>
        /// Get/Set method of the businessDayStartTime field
        /// </summary>
        public int BusinessDayStartTime { get { return businessDayStartTime; } set { businessDayStartTime = value; } }

        /// <summary>
		/// Get/Set method of the timeZoneName field
		/// </summary>
        public string TimeZoneName { get { return timeZoneName; } set { timeZoneName = value; } }

        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        public string PhoneNumber
        {
            get { return phoneNumber; }
            set { phoneNumber = value; }
        }


        /// <summary>
        /// Get/Set method of the latitude field
        /// </summary>
        public decimal? Latitude { get { return latitude; } set { latitude = value; } }

        /// <summary>
        /// Get/Set method of the Longitude field
        /// </summary>
        public decimal? Longitude { get { return longitude; } set { longitude = value; } }

        /// <summary>
        /// Get/Set method of the customerKey field
        /// </summary>
        public string CustomerKey { get { return customerKey; } set { customerKey = value; } }

        /// <summary>
        /// Get/Set method of the SiteDeliveryDetailsDTOList field
        /// </summary>

        public List<SiteDetailContainerDTO> SiteDeliveryDetailsDTOList { get { return siteDeliveryDetailsDTOList; } set { siteDeliveryDetailsDTOList = value;} }
        
    }
}
