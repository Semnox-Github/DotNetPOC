/********************************************************************************************
 * Project Name - GenericOTP DTO
 * Description  - Data object of the GenericOTP
 * 
 **************
 **Version Log
 **************
 *Version      Date         Modified By       Remarks          
 *********************************************************************************************
 *2.130.11   12-Aug-2022    Yashodhara C H     Created 
 ********************************************************************************************/
using System;

namespace Semnox.Core.GenericUtilities
{
    ///<summary>
    /// This is a GenericOTP data object class.
    /// </summary>
    public class GenericOTPDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        ///<summary>
        ///SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by id field
            /// </summary>
            ID,
            /// <summary>
            /// Search by code field
            /// </summary>
            CODE,
            /// <summary>
            /// Search by phone field
            /// </summary>
            PHONE,
            /// <summary>
            /// Search by emailId field
            /// </summary>
            EMAIL_ID,
            /// <summary>
            /// Search by source field
            /// </summary>
            SOURCE,
            /// <summary>
            /// Search by is_verified field
            /// </summary>
            IS_VERIFIED,
            /// <summary>
            /// Search by is_active field
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search by remaining_attempts field
            /// </summary>
            REMAINING_ATTEMPTS,
            /// <summary>
            /// Search by master_entity_id field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by site_id field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by IS_UNEXPIRED field
            /// </summary>
            IS_UNEXPIRED,
        }

        ///<summary>
        /// SourceType enum
        /// </summary>
        public enum SourceType
        {
            /// <summary>
            ///  generic field
            /// </summary>
            GENERIC,
            /// <summary>
            /// logging field
            /// </summary>
            LOGIN,
            /// <summary>
            /// customer_delete field
            /// </summary>
            CUSTOMERDELETE
        }

        private int id;
        private string code;
        private string phone;
        private string countryCode;
        private string emailId;
        private string source;
        private bool? isVerified;
        private int remainingAttempts;
        private DateTime expiryTime;
        private bool isActive;
        private string guid;
        private bool synchStatus;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime? lastUpdatedDate;
        private int masterEntityId;
        private int siteId;

        /// <summary>
        /// Default constructor
        /// </summary>
        public GenericOTPDTO()
        {
            log.LogMethodEntry();
            id = -1;
            isActive = true;
            isVerified = false;
            siteId = -1;
            masterEntityId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor Required data fields
        /// </summary>
        public GenericOTPDTO(int id, string code, string phone, string countryCode, string emailId, string source, bool isVerified, int remainingAttempts, DateTime expiryTime, bool isActive)
            :this()
        {
            log.LogMethodEntry(id, code, phone, countryCode, emailId, source, isVerified, remainingAttempts, expiryTime, isActive);
            this.id = id;
            this.code = code;
            this.phone = phone;
            this.countryCode = countryCode;
            this.emailId = emailId;
            this.source = source;
            this.isVerified = isVerified;
            this.expiryTime = expiryTime;
            this.remainingAttempts = remainingAttempts;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor All data fields
        /// </summary>
        public GenericOTPDTO(int id, string code, string phone, string countryCode, string emailId, string source, bool isVerified, int numberOfAttempts, DateTime expiryTime, bool isActive, string guid,
                             int siteId, bool synchStatus, int masterEntityId, string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime? lastUpdatedDate)
            :this(id, code, phone, countryCode, emailId, source, isVerified, numberOfAttempts, expiryTime, isActive)
        {
            log.LogMethodEntry(id, code, phone, source, isVerified, numberOfAttempts, expiryTime, isActive, guid, siteId, synchStatus, masterEntityId, createdBy, creationDate,
                                lastUpdatedBy, lastUpdatedDate);
            this.siteId = siteId;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>
        public int Id { get { return id; } set { id = value; this.IsChanged = true;  } }
        ///<summary>
        ///Get/Set method code field
        /// </summary>
        public string Code { get { return code; } set { code = value; this.IsChanged = true; }  }
        ///<summary>
        ///Get/Set method Phone field
        /// </summary>
        public string Phone { get { return phone; } set { phone = value; this.IsChanged = true; } }

        ///<summary>
        ///Get/Set method CountryCode field
        /// </summary>
        public string CountryCode { get { return countryCode; } set { countryCode = value; this.IsChanged = true; } }
        ///<summary>
        ///Get/Set method EmailId field
        /// </summary>
        public string EmailId { get { return emailId; } set { emailId = value; this.IsChanged = true; } }
        ///<summary>
        ///Get/Set method source field
        /// </summary>
        public string Source { get { return source; } set { source = value; this.IsChanged = true;  } }
        ///<summary>
        ///Get/Set method isVerified field
        /// </summary>
        public bool? IsVerified { get { return isVerified; } set { isVerified = value; this.IsChanged = true;  } }
        ///<summary>
        ///Get/Set method RemainingAttempts field
        /// </summary>
        public int RemainingAttempts { get { return remainingAttempts; } set { remainingAttempts = value; this.IsChanged = true; } }
        ///<summary>
        ///Get/Set method expiryDate field
        /// </summary>
        public DateTime ExpiryTime { get { return expiryTime; } set { expiryTime = value; this.IsChanged = true; } }
        ///<summary>
        ///Get/Set method Guid field
        /// </summary>
        public string Guid { get { return guid; } set { guid = value; } }
        ///<summary>
        ///Get/Set method isActive field
        /// </summary>
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }
        ///<summary>
        ///Get/Set method siteId field
        /// </summary>
        public int SiteId { get { return siteId; } set { siteId = value; } }
        ///<summary>
        ///Get/Set method synchStatus field
        /// </summary>
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }
        ///<summary>
        ///Get/Set method masterEntityId field
        /// </summary>
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }
        ///<summary>
        ///Get/Set method lastUpdatedBy field
        /// </summary>
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }
        ///<summary>
        ///Get/Set method lastUpdatedDate field
        /// </summary>
        public DateTime? LastUpdatedDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value; } }
        ///<summary>
        ///Get/Set method creationDate field
        /// </summary>
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }
        ///<summary>
        ///Get/Set method creationDate field
        /// </summary>
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }

        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || id < 0;
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
            log.LogMethodExit(null);
        }
    }
}
