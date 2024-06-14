/********************************************************************************************
 * Project Name - UserPayRateDTO
 * Description  - DTO to setup User Pay Rate
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.130       05-Jul-2021      Nitin Pai  Added: Attendance and Pay Rate enhancement
 ********************************************************************************************/
using System;

namespace Semnox.Parafait.User
{
    public class UserPayRateDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int userPayRateId;
        private int? userId;
        private int? userRoleId;
        private string payType;
        private decimal? regularPayRate;
        private decimal? overTimePayRate;
        private DateTime effectiveDate;
        private bool isActive;
        private int siteId;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by user pay rate ID field
            /// </summary>
            USER_PAY_RATE_ID,
            /// <summary> 
            /// Search by USER_ID field
            /// </summary>
            USER_ID,
            /// <summary>
            /// Search by user role ID field
            /// </summary>
            USER_ROLE_ID,
            /// <summary>
            /// Search by pay type field
            /// </summary>
            PAY_TYPE,
            /// <summary>
            /// Search by site id field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER ENTITY id field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by EFFECTIVE DATE field
            /// </summary>
            EFFECTIVE_DATE,
            /// <summary>
            /// Search by IS ACTIVE field
            /// </summary>
            IS_ACTIVE,
            /// <summary> 
            /// Search by USER_ID_IS_NULL field
            /// </summary>
            USER_ID_IS_NULL,
            /// <summary>
            /// Search by USER_ROLE_ID_IS_NULL field
            /// </summary>
            USER_ROLE_ID_IS_NULL,

        }
        /// <summary>
        /// Default constructor for UserPayRateDTO
        /// </summary>
        public UserPayRateDTO()
        {
            log.LogMethodEntry();
            userPayRateId = -1;
            userId = null;
            userRoleId = null;
            payType = "";
            siteId = -1;
            masterEntityId = -1;
            isActive = true;
            log.LogMethodExit();
        }


        /// <summary>
        /// Parameterized constructor for UserPayRateDTO with required fields
        /// </summary>
        public UserPayRateDTO(int userPayRateId, int? userId, int? userRoleId, string payType,
                            decimal? regularPayRate, decimal? overTimePayRate, DateTime effectiveDate, bool isActive)
            : this()
        {
            log.LogMethodEntry();
            this.userPayRateId = userPayRateId;
            this.userId = userId;
            this.userRoleId = userRoleId;
            this.payType = payType;
            this.regularPayRate = regularPayRate;
            this.overTimePayRate = overTimePayRate;
            this.effectiveDate = effectiveDate;
            this.isActive = isActive;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor for CardTypeDTO with required fields
        /// </summary>
        public UserPayRateDTO(int userPayRateId, int? userId, int? userRoleId, string payType,
                            decimal? regularPayRate, decimal? overTimePayRate, DateTime effectiveDate, bool isActive, int siteId, string guid,
                            bool synchStatus, int masterEntityId, string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate
              )
            : this(userPayRateId, userId, userRoleId,payType, regularPayRate, overTimePayRate, effectiveDate,
                             isActive)
        {
            log.LogMethodEntry(userPayRateId, userId, userRoleId, payType, regularPayRate, overTimePayRate, effectiveDate,
                             isActive, siteId, guid,
                             synchStatus, masterEntityId, createdBy, creationDate, lastUpdatedBy, lastUpdateDate);
            this.siteId = siteId;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            log.LogMethodExit();
        }


        public int UserPayRateId { get { return userPayRateId; } set { userPayRateId = value; this.IsChanged = true; } }
        public int? UserId { get { return userId; } set { userId = value; this.IsChanged = true; } }
        public int? UserRoleId { get { return userRoleId; } set { userRoleId = value; this.IsChanged = true; } }
        public string PayType { get { return payType; } set { payType = value; this.IsChanged = true; } }
        public decimal? RegularPayRate { get { return regularPayRate; } set { regularPayRate = value; this.IsChanged = true; } }
        public decimal? OverTimePayRate { get { return overTimePayRate; } set { overTimePayRate = value; this.IsChanged = true; } }
        public DateTime EffectiveDate { get { return effectiveDate; } set { effectiveDate = value; } }
        public bool IsActive { get { return isActive; } set { this.IsChanged = true; isActive = value; } }
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }
        public DateTime LastUpdatedDate { get { return lastUpdateDate; } set { lastUpdateDate = value; } }
        public string Guid { get { return guid; } set { guid = value; } }
        public int SiteId { get { return siteId; } set { siteId = value; } }
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }

        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
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
 
        
 
