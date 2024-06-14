/********************************************************************************************
 * Project Name - ClientAppDTO
 * Description  - API to return images
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By       Remarks          
 *********************************************************************************************
 *2.110       20-Dec-2020   Nitin Pai         Created
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Semnox.Parafait.ClientApp
{
    public class ClientAppDTO : IChangeTracking
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int clientAppId;
        private string appId;
        private string appName;
        private bool active;
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
            ACTIVE = 0,
            /// <summary>
            /// Search by SECURITY_TOKEN field
            /// </summary>
            APP_ID = 1,
            /// <summary>
            /// Search by GUID field
            /// </summary>
            GUID = 2
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public ClientAppDTO()
        {
            this.clientAppId = -1;
            this.appId = "";
            this.appName = "";
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public ClientAppDTO(int clientAppId, string appId, string appName,  bool active, string createdBy, DateTime createdDate, string lastUpdatedBy, DateTime lastUpdatedDate,  string guid,bool synchStatus,int masterEntityId,int siteId)
        {
            this.clientAppId = clientAppId;
            this.appId = appId;
            this.appName = appName;
            this.active = active;
            this.createdDate = createdDate;
            this.createdBy = createdBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.guid = guid;
            this.masterEntityId = masterEntityId;
            this.siteId = siteId;
            this.SynchStatus = synchStatus;
        }

        /// <summary>
        /// Get/Set method of the clientAppId field
        /// </summary>
        [DisplayName("clientAppId")]
        public int ClientAppId { get { return clientAppId; } set { clientAppId = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the ComapnyName field
        /// </summary>
        [DisplayName("AppId")]
        public string AppId { get { return this.appId; } set { appId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the AppName field
        /// </summary>
        [DisplayName("AppName")]
        public string AppName { get { return appName; } set { appName = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Active field
        /// </summary>
        [DisplayName("Active")]
        public bool Active { get { return active; } set { active = value; this.IsChanged = true; } }

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
        /// Get method of the SiteId field
        /// </summary>
        public int SiteId
        {
            get { return siteId; }
            set { siteId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId
        {
            get { return masterEntityId; }
            set { masterEntityId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get method of the SynchStatus field
        /// </summary>
        public bool SynchStatus
        {
            get { return synchStatus; }
            set { synchStatus = value; }
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
