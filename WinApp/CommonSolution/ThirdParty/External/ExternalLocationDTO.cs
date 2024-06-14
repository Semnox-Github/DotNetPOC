/********************************************************************************************
 * Project Name - CommonAPI
 * Description  - Created to hold the External location details .
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.130.7    07-Apr-2022   Ashish Bhat             Created : External  REST API.
 ***************************************************************************************************/
using System;

namespace Semnox.Parafait.ThirdParty.External
{
    public class ExternalLocationDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get/Set for SiteId
        /// </summary>
        public int SiteId { get; set; }
        /// <summary>
        /// Get/Set for ExternalSourceReference
        /// </summary>
        public string ExternalSourceReference { get; set; }
        /// <summary>
        /// Get/Set for SiteUrl
        /// </summary>
        public string SiteURL { get; set; }
        /// <summary>
        /// Get/Set for City
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// Get/Set for State
        /// </summary>
        public string State { get; set; }
        /// <summary>
        /// Get/Set for Country
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        /// Get/Set for StoreRanking
        /// </summary>
        public string StoreRanking { get; set; }
        /// <summary>
        /// Get/Set for SiteShortName
        /// </summary>
        public string SiteShortName { get; set; }
        /// <summary>
        /// Get/Set for OnlineEnabled
        /// </summary>
        public string OnlineEnabled { get; set; }
        /// <summary>
        /// Get/Set for Aboutus
        /// </summary>
        public string AboutUs { get; set; }
        /// <summary>
        /// Get/Set for Pincode
        /// </summary>
        public string PinCode { get; set; }
        /// <summary>
        /// Get/Set for 
        /// Get/Set for Description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Get/Set for SiteName
        /// </summary>
        public string SiteName { get; set; }
        /// <summary>
        /// Get/Set for SiteAdress
        /// </summary>
        public string SiteAddress { get; set; }
        /// <summary>
        /// Get/Set for OpenTime
        /// </summary>
        public decimal OpenTime { get; set; }
        /// <summary>
        /// Get/Set for CloseTime
        /// </summary>
        public decimal CloseTime { get; set; }
        /// <summary>
        /// Get/Set for OpenDate
        /// </summary>
        public DateTime? OpenDate { get; set; }
        /// <summary>
        /// Get/Set for CloseDate
        /// </summary>
        public DateTime? CloseDate { get; set; }
        /// <summary>
        /// Get/Set for SiteCode
        /// </summary>
        public int SiteCode { get; set; }
        /// <summary>
        /// Get/Set for IsMasterSite
        /// </summary>
        public bool IsMasterSite { get; set; }
        /// <summary>
        /// Default Constructor
        /// </summary>

        public ExternalLocationDTO()
        {
            log.LogMethodEntry();
            SiteId = -1;
            SiteName = string.Empty;
            SiteAddress = string.Empty;
            SiteCode = -1;
            IsMasterSite = true;
            SiteShortName = string.Empty;
            OnlineEnabled = string.Empty;
            AboutUs = string.Empty;
            PinCode = string.Empty;
            Description = string.Empty;
            SiteURL = string.Empty;
            City = string.Empty;
            State = string.Empty;
            Country = string.Empty;
            StoreRanking = string.Empty;
            OpenTime = 0;
            CloseTime = 0;
            ExternalSourceReference = string.Empty;
            OpenDate = null;
            CloseDate = null;
            log.LogMethodExit();

        }
        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        public ExternalLocationDTO(int siteId, string siteName, int siteCode, bool isMasterSite, string siteShortName, string onlineEnabled,
                                    string aboutUs, string pinCode, string description, string siteURL, 
                                    string city, string state, string country, string storeRanking,
                                    decimal openTime, decimal closeTime, string externalSourceReference, 
                                    DateTime openDate, DateTime closeDate)
        {

            log.LogMethodEntry(siteId, siteName, siteCode, isMasterSite, siteShortName, onlineEnabled, aboutUs, pinCode, description,
                               siteURL, city, state, country, storeRanking, openTime, closeTime, externalSourceReference,
                               openDate, closeDate, state);
            this.SiteId = siteId;
            this.SiteName = siteName;
            this.SiteCode = siteCode;
            this.IsMasterSite = isMasterSite;
            this.SiteShortName = siteShortName;
            this.OnlineEnabled = onlineEnabled;
            this.AboutUs = aboutUs;
            this.PinCode = pinCode;
            this.Country = country;
            this.StoreRanking = storeRanking;
            this.OpenTime = openTime;
            this.CloseTime = closeTime;
            this.ExternalSourceReference = externalSourceReference;
            this.OpenDate = openDate;
            this.CloseDate = closeDate;
            this.State = state;
            this.City = city;
            this.SiteURL = siteURL;
            this.Description = description;
            this.SiteAddress = SiteAddress;
            log.LogMethodExit();

        }
    }
}
