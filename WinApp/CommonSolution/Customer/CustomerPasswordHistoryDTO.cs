/********************************************************************************************
* Project Name - CustomerPasswordHistoryDTO
* Description  - Data object of CustomerPasswordHistoryDTO
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.80        25-Jun-2020   Indrajeet Kumar         Created 
********************************************************************************************/

using System;
using System.ComponentModel;

namespace Semnox.Parafait.Customer
{    
    public class CustomerPasswordHistoryDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public enum SearchByCustomerPasswordHistoryParameters
        {
            /// <summary>
            /// Search by userPasswordHistory Id field
            /// </summary>
            CUSTOMER_PASSWORD_HISTORY_ID,
            /// <summary>
            /// Search by changeDate Id field
            /// </summary>
            CHANGE_DATE,
            /// <summary>
            /// Search by userId field
            /// </summary>
            PROFILE_ID,
            /// <summary>
            /// Search by Site Id field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID
        }

        private int customerPasswordHistoryId;
        private int profileId;
        private string password;
        private DateTime changeDate;
        private int site_id;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public CustomerPasswordHistoryDTO()
        {
            log.LogMethodEntry();
            customerPasswordHistoryId = -1;
            profileId = -1;
            site_id = -1;
            masterEntityId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor for CustomerPasswordHistoryDTO with required fields
        /// </summary>
        public CustomerPasswordHistoryDTO(int customerPasswordHistoryId, int profileId, string password)
            :this()
        {
            log.LogMethodEntry(customerPasswordHistoryId, profileId, password);
            this.customerPasswordHistoryId = customerPasswordHistoryId;
            this.profileId = profileId;
            this.password = password;            
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor for CustomerPasswordHistoryDTO with all fields
        /// </summary>
        public CustomerPasswordHistoryDTO(int customerPasswordHistoryId, int profileId, string password, DateTime changeDate, int siteId,
            string guid, bool synchStatus, int masterEntityId, string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate)
            :this(customerPasswordHistoryId, profileId, password)
        {
            log.LogMethodEntry();
            this.site_id = siteId;
            this.guid = guid;
            this.changeDate = changeDate;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>
        public int CustomerPasswordHistoryId { get { return customerPasswordHistoryId; } set { customerPasswordHistoryId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ProfileId field
        /// </summary>
        public int ProfileId { get { return profileId; } set { profileId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Password field
        /// </summary>
        public string Password { get { return password; } set { password = value; IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ChangeDate field
        /// </summary>
        public DateTime ChangeDate { get { return changeDate; } set { changeDate = value; IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        public int SiteId { get {return site_id; } set { site_id = value; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid { get { return guid; } set { guid = value; IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        public bool SynchStatus { get { return synchStatus;} set { synchStatus = value; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }
        
        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }
        
        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        public DateTime LastUpdateDate {get { return lastUpdateDate; } set { lastUpdateDate = value; } }

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
                    return notifyingObjectIsChanged || customerPasswordHistoryId < 0;
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
