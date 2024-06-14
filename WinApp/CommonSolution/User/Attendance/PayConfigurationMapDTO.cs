/********************************************************************************************
 * Project Name - PayConfigurationMapDTO
 * Description  - Data object of Pay Configuration Map
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 *********************************************************************************************
 *2.90.0      01-JUL-2020   Akshay Gulaganji   Created   
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.User
{
    /// <summary>
    /// This is the PayConfigurationMap data object class. This acts as data holder for the PayConfigurationMap business object
    /// </summary>
    public class PayConfigurationMapDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by PAY CONFIGURATION MAP ID field
            /// </summary>
            PAY_CONFIGURATION_MAP_ID,
            /// <summary>
            /// Search by USER ROLE ID field
            /// </summary>
            USER_ROLE_ID,
            /// <summary>
            /// Search by USER ID field
            /// </summary>
            USER_ID,
            /// <summary>
            /// Search by PAY CONFIGURATION ID field
            /// </summary>
            PAY_CONFIGURATION_ID,
            /// <summary>
            /// Search by EFFECTIVE DATE GREATER THAN field
            /// </summary>
            EFFECTIVE_DATE_GREATER_THAN,
            /// <summary>
            /// Search by END DATE LESS THAN OR EQUALS field
            /// </summary>
            END_DATE_LESS_THAN_OR_EQUALS,
            /// <summary>
            /// Search by IS ACTIVE field
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by USER ROLE ID LIST field
            /// </summary>
            USER_ROLE_ID_LIST,
            /// <summary>
            /// Search by USER ID LIST field
            /// </summary>
            USER_ID_LIST,
            /// <summary>
            /// Search by PAY CONFIGURATION ID LIST field
            /// </summary>
            PAY_CONFIGURATION_ID_LIST
        }

        private int payConfigurationMapId;
        private int userRoleId;
        private int userId;
        private int payConfigurationId;
        private DateTime effectiveDate;
        private DateTime? endDate;
        private bool isActive;
        private string guid;
        private int masterEntityId;
        private int siteId;
        private bool synchStatus;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private string createdBy;
        private DateTime creationDate;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default Constructor
        /// </summary>
        public PayConfigurationMapDTO()
        {
            log.LogMethodEntry();
            payConfigurationMapId = -1;
            userRoleId = -1;
            userId = -1;
            payConfigurationId = -1;
            isActive = true;
            siteId = -1;
            masterEntityId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with required fields
        /// </summary>
        /// <param name="payConfigurationMapId"></param>
        /// <param name="userRoleId"></param>
        /// <param name="userId"></param>
        /// <param name="payConfigurationId"></param>
        /// <param name="effectiveDate"></param>
        /// <param name="endDate"></param>
        /// <param name="isActive"></param>
        public PayConfigurationMapDTO(int payConfigurationMapId, int userRoleId, int userId, int payConfigurationId, DateTime effectiveDate, DateTime? endDate, bool isActive)
            : this()
        {
            log.LogMethodEntry(payConfigurationMapId, userRoleId, userId, payConfigurationId, effectiveDate, endDate, isActive);
            this.payConfigurationMapId = payConfigurationMapId;
            this.userRoleId = userRoleId;
            this.userId = userId;
            this.payConfigurationId = payConfigurationId;
            this.effectiveDate = effectiveDate;
            this.endDate = endDate;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with all fields
        /// </summary>
        /// <param name="payConfigurationMapId"></param>
        /// <param name="userRoleId"></param>
        /// <param name="userId"></param>
        /// <param name="payConfigurationId"></param>
        /// <param name="effectiveDate"></param>
        /// <param name="endDate"></param>
        /// <param name="isActive"></param>
        /// <param name="guid"></param>
        /// <param name="siteId"></param>
        /// <param name="synchStatus"></param>
        /// <param name="masterEntityId"></param>
        /// <param name="lastUpdatedDate"></param>
        /// <param name="lastUpdatedBy"></param>
        /// <param name="createdBy"></param>
        /// <param name="creationDate"></param>
        public PayConfigurationMapDTO(int payConfigurationMapId, int userRoleId, int userId, int payConfigurationId, DateTime effectiveDate, DateTime? endDate, bool isActive,
                                        string guid, int siteId, bool synchStatus, int masterEntityId, DateTime lastUpdatedDate, string lastUpdatedBy, string createdBy, DateTime creationDate)
            : this(payConfigurationMapId, userRoleId, userId, payConfigurationId, effectiveDate, endDate, isActive)
        {
            log.LogMethodEntry(payConfigurationMapId, userRoleId, userId, payConfigurationId, effectiveDate, endDate, isActive, guid, siteId,
                                synchStatus, masterEntityId, lastUpdatedDate, lastUpdatedBy, createdBy, creationDate);
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.lastUpdatedDate = lastUpdatedDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the Pay Configuration Map Id field
        /// </summary>
        public int PayConfigurationMapId
        {
            get { return payConfigurationMapId; }
            set { payConfigurationMapId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the Pay Configuration Id field
        /// </summary>
        public int PayConfigurationId
        {
            get { return payConfigurationId; }
            set { payConfigurationId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the User Role Id field
        /// </summary>
        public int UserRoleId
        {
            get { return userRoleId; }
            set { userRoleId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the User Id field
        /// </summary>
        public int UserId
        {
            get { return userId; }
            set { userId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the Effective Date field
        /// </summary>
        public DateTime EffectiveDate
        {
            get { return effectiveDate; }
            set { effectiveDate = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the End Date field
        /// </summary>
        public DateTime? EndDate
        {
            get { return endDate; }
            set { endDate = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the Is Active field
        /// </summary>
        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid
        {
            get { return guid; }
            set { guid = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the Master Entity Id field
        /// </summary>
        public int MasterEntityId
        {
            get { return masterEntityId; }
            set { masterEntityId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the Site Id field
        /// </summary>
        public int SiteId
        {
            get { return siteId; }
            set { siteId = value; }
        }
        /// <summary>
        /// Get/Set method of the Synch Status field
        /// </summary>
        public bool SynchStatus
        {
            get { return synchStatus; }
            set { synchStatus = value; }
        }
        /// <summary>
        /// Get/Set method of the Last Updated By field
        /// </summary>
        public string LastUpdatedBy
        {
            get { return lastUpdatedBy; }
            set { lastUpdatedBy = value; }
        }
        /// <summary>
        ///  Get/Set method of the Last Update Date field
        /// </summary>
        public DateTime LastUpdatedDate
        {
            get { return lastUpdatedDate; }
            set { lastUpdatedDate = value; }
        }
        /// <summary>
        /// Get/Set method of the Created By field
        /// </summary>
        public string CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        /// <summary>
        /// Get/Set method of the Creation Date field
        /// </summary>
        public DateTime CreationDate
        {
            get { return creationDate; }
            set { creationDate = value; }
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
                    return notifyingObjectIsChanged || payConfigurationMapId < 0;
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
            IsChanged = false;
            log.LogMethodExit();
        }
    }
}
