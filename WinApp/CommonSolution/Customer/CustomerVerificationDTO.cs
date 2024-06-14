/********************************************************************************************
 * Project Name - CustomerVerification DTO
 * Description  - Data object of CustomerVerification
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *1.00        02-Feb-2017   Lakshminarayana          Created 
 *2.70.2        22-Jul-2019   Girish Kundar           Modified : Added Constructor with required Parameter
 *2.80        29-Nov-2019   Mushahid Faizan         Added ResendEmailToken property for Account Activation Verification.
 ********************************************************************************************/

using System;
using System.ComponentModel;

namespace Semnox.Parafait.Customer
{
    /// <summary>
    /// This is the CustomerVerification data object class. This acts as data holder for the CustomerVerification business object
    /// </summary>
    public class CustomerVerificationDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        /// 
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  ID field
            /// </summary>
            ID,
            /// <summary>
            /// Profile Id
            /// </summary>
            PROFILE_ID,
            /// <summary>
            /// Related Customer Id
            /// </summary>
            VERIFICATION_CODE,
            /// <summary>
            /// CustomerVerification Id
            /// </summary>
            CUSTOMER_ID,
            /// <summary>
            /// Search by IsActive field
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search by site_id field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by Master Entity Id field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by Guid field
            /// </summary>
            GUID,
        }

        private int id;
        private int customerId;
        private int profileId;
        private string source;
        private string verificationCode;
        private DateTime? expiryDate;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private int siteId;
        private int masterEntityId;
        private bool synchStatus;
        private string guid;
        private bool resendEmailToken;
        private ProfileDTO profileDTO;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public CustomerVerificationDTO()
        {
            log.LogMethodEntry();
            id = -1;
            customerId = -1;
            profileId = -1;
            masterEntityId = -1;
            isActive = true;
            siteId = -1;
            resendEmailToken = false;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with Required data fields
        /// </summary>
        public CustomerVerificationDTO(int id, int customerId, int profileId,
                                       string source, string verificationCode,
                                       DateTime? expiryDate, bool isActive)
            : this()
        {
            log.LogMethodEntry(id, customerId, profileId, source, verificationCode, expiryDate, isActive);
            this.id = id;
            this.customerId = customerId;
            this.profileId = profileId;
            this.source = source;
            this.verificationCode = verificationCode;
            this.expiryDate = expiryDate;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public CustomerVerificationDTO(int id, int customerId, int profileId,
                                       string source, string verificationCode,
                                       DateTime? expiryDate, bool isActive, string createdBy,
                                       DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate,
                                       int siteId, int masterEntityId, bool synchStatus, string guid)
            :this(id, customerId, profileId, source, verificationCode, expiryDate, isActive)
        {
            log.LogMethodEntry(id, customerId, profileId, source, verificationCode, expiryDate, isActive,
                               createdBy, creationDate, lastUpdatedBy, lastUpdateDate, siteId,
                               masterEntityId, synchStatus, guid);
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            this.siteId = siteId;
            this.masterEntityId = masterEntityId;
            this.synchStatus = synchStatus;
            this.guid = guid;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>
        public int Id
        {
            get
            {
                return id;
            }

            set
            {
                this.IsChanged = true;
                id = value;
            }
        }

        /// <summary>
        /// Get/Set method of the CustomerId field
        /// </summary>
        public int CustomerId
        {
            get
            {
                return customerId;
            }

            set
            {
                this.IsChanged = true;
                customerId = value;
            }
        }

        /// <summary>
        /// Get method of the profileId field
        /// </summary>
        public int ProfileId
        {
            get
            {
                return profileId;
            }

            set
            {
                this.IsChanged = true;
                profileId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the source field
        /// </summary>
        [DisplayName("Source")]
        public string Source
        {
            get
            {
                return source;
            }

            set
            {
                this.IsChanged = true;
                source = value;
            }
        }

        /// <summary>
        /// Get/Set method of the verificationCode field
        /// </summary>
        [DisplayName("Verification Code")]
        public string VerificationCode
        {
            get
            {
                return verificationCode;
            }

            set
            {
                this.IsChanged = true;
                verificationCode = value;
            }
        }

        /// <summary>
        /// Get/Set method of the expiryDate field
        /// </summary>
        [DisplayName("Expiry Date")]
        public DateTime? ExpiryDate
        {
            get
            {
                return expiryDate;
            }

            set
            {
                this.IsChanged = true;
                expiryDate = value;
            }
        }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("Active?")]
        public bool IsActive
        {
            get
            {
                return isActive;
            }

            set
            {
                this.IsChanged = true;
                isActive = value;
            }
        }

        /// <summary>
        /// Get/Set method of the ResendEmailToken field
        /// </summary>
        [DisplayName("ResendEmailToken")]
        public bool ResendEmailToken
        {
            get
            {
                return resendEmailToken;
            }

            set
            {
                resendEmailToken = value;
            }
        }
        /// <summary>
        /// Get/Set method of the profileDTO field
        /// </summary>
        public ProfileDTO ProfileDTO
        {
            get
            {
                return profileDTO;
            }

            set
            {
                profileDTO = value;
            }
        }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        public string CreatedBy
        {
            get
            {
                return createdBy;
            }

            set
            {
                this.IsChanged = true;
                createdBy = value;
            }
        }
        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        public DateTime CreationDate
        {
            get
            {
                return creationDate;
            }

            set
            {
               
                creationDate = value;
            }
        }
        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        public string LastUpdatedBy
        {
            get
            {
                return lastUpdatedBy;
            }

            set
            {
              
                lastUpdatedBy = value;
            }
        }
        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        public DateTime LastUpdateDate
        {
            get
            {
                return lastUpdateDate;
            }
            set
            {
                lastUpdateDate = value;
            }
        }
        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        public int SiteId
        {
            get
            {
                return siteId;
            }
            set
            {
                siteId = value;
            }
        }
        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId
        {
            get
            {
                return masterEntityId;
            }
            set
            {
                this.IsChanged = true;
                masterEntityId = value;
            }
        }
        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        public bool SynchStatus
        {
            get
            {
                return synchStatus;
            }
            set
            {
                synchStatus = value;
            }
        }
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid
        {
            get
            {
                return guid;
            }
            set
            {
                this.IsChanged = true;
                guid = value;
            }
        }

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
