using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Semnox.Parafait.ClientApp
{
    /// <summary>
    /// This is the Client DTO Holds Client Data
    /// </summary>
    public class ClientAppVersionMappingDTO : IChangeTracking
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int clientAppVersionMappingId;
        private string appId;
        private int clientId;
        private string releaseNumber;
        private string gateWayURL;
        private string tokenizationKey;
        private string securityCode;
        private string apiKey;
        private DateTime createdDate;
        private string createdBy;
        private DateTime lastUpdatedDate;
        private string lastUpdatedBy;
        private string guid;
        private bool isActive;
        private string deprecated;
        private string remarks;



        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        /// <summary>
        /// SearchParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        /// 
        public enum SearchParameters
        {
            /// <summary>
            /// Search by  APP_ID field
            /// </summary>
            APP_ID,
            /// <summary>
            /// Search by BUILD_NUMBER field
            /// </summary>
            RELEASE_NUMBER,
            /// <summary>
            /// Search by SECURITY_TOKEN field
            /// </summary>
            SECURITY_TOKEN,
            /// <summary>
            /// Search by SECURITY_TOKEN field
            /// </summary>
            SECURITY_TOKEN_PARTIAL,

        }
        /// <summary>
        /// Default constructor
        /// </summary>
        /// 
        public ClientAppVersionMappingDTO()
        {
            clientAppVersionMappingId = -1;
            appId = "";
            releaseNumber = "";
            clientId = -1;
            tokenizationKey = "";
            securityCode = "";

        }
        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public ClientAppVersionMappingDTO(int clientAppVersionMappingId, string appId, int clientId, bool isActive, string createdBy, DateTime createdDate, string lastUpdatedBy, DateTime lastUpdatedDate, string releaseNumber, string gateWayURL,
            string tokenizationKey, string apiKey, string securityCode, string deprecated, string remarks)
        {
            this.clientAppVersionMappingId = clientAppVersionMappingId;
            this.clientId = clientId;
            this.appId = appId;
            this.releaseNumber = releaseNumber;
            this.tokenizationKey = tokenizationKey;
            this.apiKey = apiKey;
            this.gateWayURL = gateWayURL;
            this.createdDate = createdDate;
            this.createdBy = createdBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.securityCode = securityCode;
            this.deprecated = deprecated;
            this.remarks = remarks;
        }

        /// <summary>
        /// Get/Set method of the clientAppVersionMappingId field
        /// </summary>
        [DisplayName("ClientAppVersionMappingId")]
        public int ClientAppVersionMappingId { get { return clientAppVersionMappingId; } set { clientAppVersionMappingId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the AppId field
        /// </summary>
        [DisplayName("AppId")]
        public string AppId { get { return appId; } set { appId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ClientId field
        /// </summary>
        [DisplayName("ClientId")]
        public int ClientId { get { return clientId; } set { clientId = value; this.IsChanged = true; } }

        // <summary>
        /// Get/Set method of the Deprecated field
        /// </summary>
        [DisplayName("Deprecated")]
        public string Deprecated
        {
            get { return deprecated; }
            set { deprecated = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the ReleaseNumber field
        /// </summary>
        [DisplayName("ReleaseNumber")]
        public string ReleaseNumber { get { return releaseNumber; } set { releaseNumber = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the TokenizationKey field
        /// </summary>
        [DisplayName("TokenizationKey")]
        public string TokenizationKey { get { return tokenizationKey; } set { tokenizationKey = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the SecurityCode field
        /// </summary>
        [DisplayName("SecurityCode")]
        public string SecurityCode
        {
            get { return securityCode; }
            set { securityCode = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the ApiKey field
        /// </summary>
        [DisplayName("ApiKey")]
        public string ApiKey { get { return apiKey; } set { apiKey = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the GateWayURL field
        /// </summary>
        [DisplayName("GateWayURL")]
        public string GateWayURL { get { return gateWayURL; } set { gateWayURL = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the GateWayURL field
        /// </summary>
        [DisplayName("GateWayURL")]
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }
        /// <summary>
        /// Get method of the CreatedBy field
        /// </summary>
        [DisplayName("Created By")]
        public string CreatedBy { get { return createdBy; } set { createdBy = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the CreationDate field
        /// </summary>
        [DisplayName("Created Date")]
        public DateTime CreationDate { get { return createdDate; } set { createdDate = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("Updated By")]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the LastupdatedDate field
        /// </summary>
        [DisplayName("Updated Date")]
        public DateTime LastupdatedDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }

        // <summary>
        /// Get/Set method of the Remarks field
        /// </summary>
        [DisplayName("Remarks")]
        public string Remarks
        {
            get { return remarks; }
            set { remarks = value; this.IsChanged = true; }
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
                    return notifyingObjectIsChanged;
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


        public void AcceptChanges()
        {
            //log.Debug("Starts-AcceptChanges() Method.");
            log.LogMethodEntry();
            this.IsChanged = false;
            //log.Debug("Ends-AcceptChanges() Method.");
            log.LogMethodExit();
        }
    }
}
