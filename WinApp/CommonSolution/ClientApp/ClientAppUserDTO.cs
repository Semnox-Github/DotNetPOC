/********************************************************************************************
 * Project Name - ClientAppUserDTO Program
 * Description  - Data object of the ClientAppUserDTO
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
    /// This is the ClientAppUserDTO Holds Client App User Data
    /// </summary>
    public class ClientAppUserDTO :  IChangeTracking
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int clientAppUserId;
        private int clientAppId;      
        private int userId;
        private int customerId;
        private string deviceGuid;
        private string deviceType;
        private bool userSignedIn;
        private DateTime signInExpiry;
        private bool isActive;
        private DateTime createdDate;
        private string createdBy;
        private DateTime lastUpdatedDate;
        private string lastUpdatedBy;
        private string guid;
        private int siteId;
        private int masterEntityId;
        private bool synchStatus;        
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
            IS_ACTIVE,
            /// <summary>
            /// Search by CLIENT_APP_ID field
            /// </summary>
            CLIENT_APP_ID,
            /// <summary>
            /// Search by EMAIL_ID field
            /// </summary>
            EMAIL_ID,
            /// <summary>
            /// Search by DEVICE_ID field
            /// </summary>
            DEVICE_GUID,
            /// <summary>
            /// Search by LOGINID field
            /// </summary>
            LOGINID,
            /// <summary>
            /// Search by IS_SIGNED_IN field
            /// </summary>
            IS_SIGNED_IN,
            /// <summary>
            /// Search by USER_ID field
            /// </summary>
            USER_ID,
            /// <summary>
            /// Search by CUSTOMER_ID field
            /// </summary>
            CUSTOMER_ID,
            /// <summary>
            /// Search by CLIENT_APP_USER_ID field
            /// </summary>
            CLIENT_APP_USER_ID,
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public ClientAppUserDTO()
        {
            this.userId = -1;
            this.customerId = -1;
            this.deviceGuid = "";
            this.isActive = true;
            this.masterEntityId = -1;
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public ClientAppUserDTO(int clientAppUserId,int clientAppId, int userId, int customerId, string deviceGuid, string deviceType,bool userSignedIn, DateTime signInExpiry, bool active,
                                    DateTime createdDate, string createdBy, string lastUpdatedBy,  DateTime lastUpdatedDate,string guid,int siteId, int masterEntityId, bool synchStatus)
        {
            this.clientAppUserId = clientAppUserId;
            this.clientAppId = clientAppId;
            this.userId = userId;
            this.customerId = customerId;
            this.deviceGuid = deviceGuid;
            this.deviceType = deviceType;
            this.userSignedIn = userSignedIn;
            this.signInExpiry = signInExpiry;
            this.isActive = active;
            this.createdDate = createdDate;
            this.createdBy = createdBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.guid = guid;
            this.siteId = siteId;
            this.masterEntityId = masterEntityId;
            this.synchStatus = synchStatus;
        }
        /// <summary>
        /// Get/Set method of the ClientAppUserId field
        /// </summary>
        [DisplayName("ClientAppUserId")]
        public int ClientAppUserId
        {
            get { return clientAppUserId; }
            set { clientAppUserId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the ClientAppId field
        /// </summary>
        [DisplayName("ClientAppId")]
        public int ClientAppId
        {
            get { return clientAppId; }
            set { clientAppId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the UserId field
        /// </summary>
        [DisplayName("UserId")]
        public int UserId { get { return userId; } set { userId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the customerId field
        /// </summary>
        [DisplayName("customerId")]
        public int CustomerId { get { return customerId; } set { customerId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the DeviceGuid field
        /// </summary>
        [DisplayName("DeviceGuid")]
        public string DeviceGuid { get { return deviceGuid; } set { deviceGuid = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the DeviceType field
        /// </summary>
        [DisplayName("DeviceType")]
        public string DeviceType { get { return deviceType; } set { deviceType = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the UserSignedIn field
        /// </summary>
        [DisplayName("UserSignedIn")]
        public bool UserSignedIn
        {
            get { return userSignedIn; }
            set { userSignedIn = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the UserSignedIn field
        /// </summary>
        [DisplayName("UserSignedIn")]
        public DateTime SignInExpiry
        {
            get { return signInExpiry; }
            set { signInExpiry = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the Active field
        /// </summary>
        [DisplayName("Active")]
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the CreatedBy field
        /// </summary>
        [DisplayName("Created By")]
        public string CreatedBy { get { return createdBy; }  set { createdBy = value; }}

        /// <summary>
        /// Get method of the CreationDate field
        /// </summary>
        [DisplayName("CreationDate")]
        public DateTime CreationDate { get { return createdDate; } set { createdDate = value;  }}

        /// <summary>
        /// Get method of the LastupdatedDate field
        /// </summary>
        [DisplayName("LastupdatedDate")]
        public DateTime LastupdatedDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("LastUpdatedBy")]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; this.IsChanged = true; } }

        

        /// <summary>
        /// Get method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }
        /// <summary>
        /// Get method of the SiteId field
        /// </summary>
        [DisplayName("SiteId")]
        public int SiteId
        {
            get { return siteId; }
            set { siteId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get method of the MasterEntityId field
        /// </summary>
        [DisplayName("MasterEntityId")]
        public int MasterEntityId
        {
            get { return masterEntityId; }
            set { masterEntityId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Ge/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("SynchStatus")]
        
        public bool SynchStatus
        {
            get { return synchStatus; }
            set { synchStatus = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        /// 
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
            //log.Debug("Starts-AcceptChanges() Method.");
            log.LogMethodEntry();
            this.IsChanged = false;
            //log.Debug("Ends-AcceptChanges() Method.");
            log.LogMethodExit();
        }

    }
}
