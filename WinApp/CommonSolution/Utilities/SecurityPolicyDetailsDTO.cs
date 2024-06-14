/********************************************************************************************
 * Project Name - Utilities
 * Description  - DTO of security policy details
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70        24-Mar-2019   Jagan Mohana          Created 
              09-Apr-2019   Mushahid Faizan       Added LogMethodEntry & LogMethodExit method.
              29-Jul-2019   Mushahid Faizan       Added IsActive Column
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Core.Utilities
{
    public class SecurityPolicyDetailsDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by POLICY_DETAIL_ID field
            /// </summary>
            POLICY_DETAIL_ID,
            /// <summary>
            /// Search by POLICY_ID field
            /// </summary>
            POLICY_ID,
            /// <summary>
            /// Search by IsActive Field
            /// </summary>
            ISACTIVE
        }
        int policyDetailId;
        int policyId;
        string createdBy;
        DateTime creationDate;
        string lastUpdatedBy;
        DateTime lastUpdateDate;
        int siteId;
        string guid;
        bool synchStatus;
        int passwordChangeFrequency;
        int passwordMinLength;
        int passwordMinAlphabets;
        int passwordMinNumbers;
        int passwordMinSpecialChars;
        int rememberPasswordsCount;
        int invalidAttemptsBeforeLockout;
        int lockoutDuration;
        int userSessionTimeout;
        int maxUserInactivityDays;
        int maxDaysToLoginAfterUserCreation;
        int masterEntityId;
        bool isActive;


        /// <summary>
        /// Default constructor
        /// Added siteId = -1 by Mushahid Faizan on 09-Apr-2019
        /// </summary>
        public SecurityPolicyDetailsDTO()
        {
            log.LogMethodEntry();
            this.policyDetailId = -1;
            this.policyId = -1;
            this.masterEntityId = -1;
            this.siteId = -1;
            this.isActive = true;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public SecurityPolicyDetailsDTO(int policyDetailId, int policyId, string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate,
                                 int siteId, string guid, bool synchStatus, int passwordChangeFrequency, int passwordMinLength, int passwordMinAlphabets,
                                 int passwordMinNumbers, int passwordMinSpecialChars, int rememberPasswordsCount, int invalidAttemptsBeforeLockout, int lockoutDuration,
                                 int userSessionTimeout, int maxUserInactivityDays, int maxDaysToLoginAfterUserCreation, int masterEntityId, bool isActive)
        {
            log.LogMethodEntry(policyDetailId, policyId, createdBy, creationDate, lastUpdatedBy, lastUpdateDate, siteId, guid, synchStatus, passwordChangeFrequency,
                               passwordMinLength, passwordMinAlphabets, passwordMinNumbers, passwordMinSpecialChars, rememberPasswordsCount, invalidAttemptsBeforeLockout,
                               lockoutDuration, userSessionTimeout, maxUserInactivityDays, maxDaysToLoginAfterUserCreation, masterEntityId, isActive);
            this.policyDetailId = policyDetailId;
            this.policyId = policyId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            this.siteId = siteId;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.passwordChangeFrequency = passwordChangeFrequency;
            this.passwordMinLength = passwordMinLength;
            this.passwordMinAlphabets = passwordMinAlphabets;
            this.passwordMinNumbers = passwordMinNumbers;
            this.passwordMinSpecialChars = passwordMinSpecialChars;
            this.rememberPasswordsCount = rememberPasswordsCount;
            this.invalidAttemptsBeforeLockout = invalidAttemptsBeforeLockout;
            this.lockoutDuration = lockoutDuration;
            this.userSessionTimeout = userSessionTimeout;
            this.maxUserInactivityDays = maxUserInactivityDays;
            this.maxDaysToLoginAfterUserCreation = maxDaysToLoginAfterUserCreation;
            this.masterEntityId = masterEntityId;
            this.isActive = isActive;
            log.LogMethodExit();
        }
        /// <summary>
        /// Get/Set method of the PolicyDetailId field
        /// </summary>
        [DisplayName("PolicyDetailId")]
        [ReadOnly(true)]
        public int PolicyDetailId { get { return policyDetailId; } set { policyDetailId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the PolicyId field
        /// </summary>
        [DisplayName("PolicyId")]
        public int PolicyId { get { return policyId; } set { policyId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("SiteId")]
        public int SiteId { get { return siteId; } set { siteId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the SyncStatus field
        /// </summary>
        [DisplayName("SyncStatus")]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("MasterEntityId")]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        [DisplayName("Created By")]
        public string CreatedBy { get { return createdBy; } set { createdBy = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        [DisplayName("CreationDate")]
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("LastUpdatedBy")]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the LastUpdateDate field
        /// </summary>
        [DisplayName("LastUpdateDate")]
        public DateTime LastUpdateDate { get { return lastUpdateDate; } set { lastUpdateDate = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the PasswordChangeFrequency field
        /// </summary>
        [DisplayName("Password Change Frequency")]
        public int PasswordChangeFrequency { get { return passwordChangeFrequency; } set { passwordChangeFrequency = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the PasswordMinLength field
        /// </summary>
        [DisplayName("Password Min Length")]
        public int PasswordMinLength { get { return passwordMinLength; } set { passwordMinLength = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the PasswordMinAlphabets field
        /// </summary>
        [DisplayName("Password Min Alphabets")]
        public int PasswordMinAlphabets { get { return passwordMinAlphabets; } set { passwordMinAlphabets = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the PasswordMinNumbers field
        /// </summary>
        [DisplayName("Password Min Numbers")]
        public int PasswordMinNumbers { get { return passwordMinNumbers; } set { passwordMinNumbers = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the PasswordMinSpecialChars field
        /// </summary>
        [DisplayName("Password Min Special Chars")]
        public int PasswordMinSpecialChars { get { return passwordMinSpecialChars; } set { passwordMinSpecialChars = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the RememberPasswordsCount field
        /// </summary>
        [DisplayName("Remember Passwords Count")]
        public int RememberPasswordsCount { get { return rememberPasswordsCount; } set { rememberPasswordsCount = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the InvalidAttemptsBeforeLockout field
        /// </summary>
        [DisplayName("Invalid Attempts Before Lockout")]
        public int InvalidAttemptsBeforeLockout { get { return invalidAttemptsBeforeLockout; } set { invalidAttemptsBeforeLockout = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the LockoutDuration field
        /// </summary>
        [DisplayName("Lockout Duration")]
        public int LockoutDuration { get { return lockoutDuration; } set { lockoutDuration = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the UserSessionTimeout field
        /// </summary>
        [DisplayName("User Session Timeout")]
        public int UserSessionTimeout { get { return userSessionTimeout; } set { userSessionTimeout = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the MaxUserInactivityDays field
        /// </summary>
        [DisplayName("MaxUser Inactivity Days")]
        public int MaxUserInactivityDays { get { return maxUserInactivityDays; } set { maxUserInactivityDays = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the MaxDaysToLoginAfterUserCreation field
        /// </summary>
        [DisplayName("MaxDays To Login After User Creation")]
        public int MaxDaysToLoginAfterUserCreation { get { return maxDaysToLoginAfterUserCreation; } set { maxDaysToLoginAfterUserCreation = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the isActive field
        /// </summary>
        public bool IsActive { get { return isActive; } set { isActive = value; } }

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
                    return notifyingObjectIsChanged || policyDetailId < 0;
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