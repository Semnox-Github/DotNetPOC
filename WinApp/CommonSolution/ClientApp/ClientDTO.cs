/********************************************************************************************
 * Project Name - ClientDTO Program
 * Description  - Data object of the ClientDTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 *********************************************************************************************
 *1.00        04-May-2016   Rakshith           Created   
 ---------------------------------------------------------------------------------------------
 */
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
    public class ClientDTO : IChangeTracking
    {

        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected int clientId;
        protected string companyName;
        protected string securityToken;
        protected string gatewayUrl;
        protected bool active;
        protected int licenseCount;
        protected DateTime createdDate;
        protected string createdBy;
        protected DateTime lastUpdatedDate;
        protected string lastUpdatedBy;
        protected string guid;
        protected string companyEmail;

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// SearchParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchParameters
        {
            /// <summary>
            /// Search by ACTIVE field
            /// </summary>
            ACTIVE = 0,
            /// <summary>
            /// Search by SECURITY_TOKEN field
            /// </summary>
            SECURITY_TOKEN = 1 ,
             /// <summary>
            /// Search by GUID field
            /// </summary>
            GUID = 2
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public ClientDTO()
        {
            this.clientId = -1;
            this.companyName = "";
            this.companyEmail = "";
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public ClientDTO(int clientId, string companyName, string securityToken, string gatewayUrl, bool active, int licenseCount, DateTime createdDate, string createdBy, DateTime lastUpdatedDate, string lastUpdatedBy, string guid,string companyEmail)
        {
            this.clientId = clientId;
            this.companyName = companyName;
            this.securityToken = securityToken;
            this.gatewayUrl = gatewayUrl;
            this.active = active;
            this.licenseCount = licenseCount;
            this.createdDate = createdDate;
            this.createdBy = createdBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.guid = guid;
            this.companyEmail = companyEmail;
         
        }

        /// <summary>
        /// Get/Set method of the ClientId field
        /// </summary>
        [DisplayName("ClientId")]
        public int ClientId { get { return clientId; } set { clientId = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the ComapnyName field
        /// </summary>
        [DisplayName("ComapnyName")]
        public string CompanyName { get { return companyName; } set { companyName = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SecurityToken field
        /// </summary>
        [DisplayName("SecurityToken")]
        public string SecurityToken { get { return securityToken; } set { securityToken = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the GatewayUrl field
        /// </summary>
        [DisplayName("GatewayUrl")]
        public string GatewayUrl { get { return gatewayUrl; } set { gatewayUrl = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the Active field
        /// </summary>
        [DisplayName("Active")]
        public bool Active { get { return active; } set { active = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the LicenseCount field
        /// </summary>
        [DisplayName("LicenseCount")]
        public int LicenseCount { get { return licenseCount; } set { licenseCount = value; this.IsChanged = true; } }


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


        /// <summary>
        /// Get method of the CompanyEmail field
        /// </summary>
        [DisplayName("CompanyEmail")]
        public string CompanyEmail { get { return companyEmail; } set { companyEmail = value; this.IsChanged = true; } }

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
        /// <summary>
        /// Allowes to accept the changes
        /// </summary>
        public void AcceptChanges()
        {
            log.Debug("Starts-AcceptChanges() Method.");
            this.IsChanged = false;
            log.Debug("Ends-AcceptChanges() Method.");
        }

    }
}
