/********************************************************************************************
 * Project Name - Profile DTO
 * Description  - Data object of Profile
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *1.00        02-Feb-2017   Lakshminarayana          Created 
 *2.70.2      19-Jul-2019   Girish Kundar            Modified : Added Constructor with required Parameter
 *2.70.2      09-Oct-2019   Akshay Gulaganji         ClubSpeed interface phase-1 enhancement changes - Added ExternalSystemReference
 *2.90        23-Jun-2020   Indrajeet Kumar          Added Property to enhance the Password Option.
*2.140.0     14-Sep-2021      Prajwal S                Modified : Changes in IsRecursive method.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Semnox.Parafait.Customer
{
    /// <summary>
    /// This is the Profile data object class. This acts as data holder for the Profile business object
    /// </summary>
    public class ProfileDTO
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
            /// Search by  ID List 
            /// </summary>
            ID_LIST,
            /// <summary>
            /// Profile Type Id
            /// </summary>
            PROFILE_TYPE_ID,
            /// <summary>
            /// Profile Type
            /// </summary>
            PROFILE_TYPE,
            /// <summary>
            /// Search by FirstName field
            /// </summary>
            FIRST_NAME,
            /// <summary>
            /// Search by LastName field
            /// </summary>
            LAST_NAME,
            /// <summary>
            /// Search by NickName field
            /// </summary>
            NICK_NAME,
            /// <summary>
            /// Search by LastName field
            /// </summary>
            OPT_IN_PROMOTIONS,
            /// <summary>
            /// Search by LastName field
            /// </summary>
            POLICY_TERMS_ACCEPTED,
            /// <summary>
            /// Search by IsActive field
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search by site_id field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by Master entity id field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by EXTERNAL_SYSTEM_REFERENCE field
            /// </summary>
            EXTERNAL_SYSTEM_REFERENCE,
            /// <summary>
            /// Search by LAST_UPDATE_FROM_DATE field
            /// </summary>
            LAST_UPDATE_FROM_DATE,
            /// <summary>
            /// Search by LAST_UPDATE_FROM_DATE field
            /// </summary>
            LAST_UPDATE_TO_DATE,
            /// <summary>
            /// Search by OPT_OUT_WHATSAPP field
            /// </summary>
            OPT_OUT_WHATSAPP
        }

        private int id;
        private int profileTypeId;
        private ProfileType profileType;
        private string title;
        private string firstName;
        private string middleName;
        private string lastName;
        private string notes;
        private DateTime? dateOfBirth;
        private string gender;
        private DateTime? anniversary;
        private string photoURL;
        private bool rightHanded;
        private bool teamUser;
        private string uniqueIdentifier;
        private string idProofFileURL;
        private string taxCode;
        private string company;
        private string designation;
        private string userName;
        private string password;
        private DateTime? lastLoginTime;
        private bool optInPromotions;
        private string optInPromotionsMode;
        private DateTime? optInLastUpdatedDate;
        private bool policyTermsAccepted;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private int siteId;
        private int masterEntityId;
        private bool synchStatus;
        private string guid;
        private string externalSystemReference;
        private List<AddressDTO> addressDTOList;
        private List<ContactDTO> contactDTOList;
        private List<ProfileContentHistoryDTO> profileContentHistoryDTOList;
        private string userStatus;
        private DateTime? passwordChangeDate;
        private bool passwordChangeOnNextLogin;
        private int invalidAccessAttempts;
        private DateTime? lockedOutTime;
        private bool notifyingObjectIsChanged;
        private bool optOutWhatsApp;
        private string nickName;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public ProfileDTO()
        {
            log.LogMethodEntry();
            id = -1;
            profileTypeId = -1;
            masterEntityId = -1;
            isActive = true;
            siteId = -1;
            profileType = ProfileType.PERSON;
            optOutWhatsApp = false;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with Required data fields
        /// </summary>
        public ProfileDTO(int id, int profileTypeId, ProfileType profileType, string title, string firstName, string middleName, string lastName,
                         string notes, DateTime? dateOfBirth, string gender, DateTime? anniversary, string photoURL,
                         bool rightHanded, bool teamUser, string uniqueIdentifier, string idProofFileURL, string taxCode,
                         string company, string designation, string userName, string password, DateTime? lastLoginTime,
                         bool optInPromotions, string optInPromotionsMode, DateTime? optInLastUpdatedDate, bool policyTermsAccepted,
                         bool isActive, string userStatus, DateTime? passwordChangeDate, int invalidAccessAttempts, DateTime? lockedOutTime, 
                         bool passwordChangeOnNextLogin, bool optOutWhatsApp, string nickName)
            :this()
        {
            log.LogMethodEntry(id, profileTypeId, profileType, title, firstName, middleName, lastName, notes, dateOfBirth,
                                gender, anniversary, photoURL, rightHanded, teamUser, uniqueIdentifier,
                                idProofFileURL, taxCode, company, designation, userName, "password", lastLoginTime,
                                optInPromotions, optInPromotionsMode, optInLastUpdatedDate, policyTermsAccepted,
                                isActive, userStatus, passwordChangeDate, invalidAccessAttempts, lockedOutTime, passwordChangeOnNextLogin, optOutWhatsApp, nickName);
            this.id = id;
            this.profileTypeId = profileTypeId;
            this.profileType = profileType;
            this.title = title;
            this.firstName = firstName;
            this.middleName = middleName;
            this.lastName = lastName;
            this.notes = notes;
            this.dateOfBirth = dateOfBirth;
            this.gender = gender;
            this.anniversary = anniversary;
            this.photoURL = photoURL;
            this.rightHanded = rightHanded;
            this.teamUser = teamUser;
            this.uniqueIdentifier = uniqueIdentifier;
            this.idProofFileURL = idProofFileURL;
            this.taxCode = taxCode;
            this.company = company;
            this.designation = designation;
            this.userName = userName;
            this.password = password;
            this.lastLoginTime = lastLoginTime;
            this.optInPromotions = optInPromotions;
            this.optInPromotionsMode = optInPromotionsMode;
            this.optInLastUpdatedDate = optInLastUpdatedDate;
            this.policyTermsAccepted = policyTermsAccepted;
            this.isActive = isActive;
            this.userStatus = userStatus;
            this.passwordChangeDate = passwordChangeDate;
            this.invalidAccessAttempts = invalidAccessAttempts;
            this.lockedOutTime = lockedOutTime;
            this.passwordChangeOnNextLogin = passwordChangeOnNextLogin;
            this.optOutWhatsApp = optOutWhatsApp;
            this.nickName = nickName;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public ProfileDTO(int id, int profileTypeId, ProfileType profileType, string title, string firstName, string middleName, string lastName,
                         string notes, DateTime? dateOfBirth, string gender, DateTime? anniversary, string photoURL,
                         bool rightHanded, bool teamUser, string uniqueIdentifier, string idProofFileURL, string taxCode,
                         string company, string designation, string userName, string password, DateTime? lastLoginTime,
                         bool optInPromotions, string optInPromotionsMode, DateTime? optInLastUpdatedDate, bool policyTermsAccepted,
                         bool isActive, string createdBy, DateTime creationDate, string lastUpdatedBy,
                         DateTime lastUpdateDate, int siteId, int masterEntityId, bool synchStatus, string guid, string externalSystemReference, 
                         string userStatus, DateTime? passwordChangeDate, int invalidAccessAttempts, DateTime? lockedOutTime, bool passwordChangeOnNextLogin, bool optOutWhatsApp, string nickName)
            :this(id, profileTypeId, profileType, title, firstName, middleName, lastName, notes, dateOfBirth,
                                gender, anniversary, photoURL, rightHanded, teamUser, uniqueIdentifier,
                                idProofFileURL, taxCode, company, designation, userName, password, lastLoginTime,
                                optInPromotions, optInPromotionsMode, optInLastUpdatedDate, policyTermsAccepted,
                                isActive, userStatus, passwordChangeDate, invalidAccessAttempts, lockedOutTime, passwordChangeOnNextLogin, optOutWhatsApp, nickName)
        {
            log.LogMethodEntry(id, profileTypeId, profileType, title, firstName, middleName, lastName, notes, dateOfBirth,
                                gender, anniversary, photoURL, rightHanded, teamUser, uniqueIdentifier,
                                idProofFileURL, taxCode, company, designation, userName, "password", lastLoginTime,
                                optInPromotions, optInPromotionsMode, optInLastUpdatedDate, policyTermsAccepted,
                                isActive, createdBy, creationDate, lastUpdatedBy, lastUpdateDate,
                                siteId, masterEntityId, synchStatus, guid, externalSystemReference, nickName);
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            this.siteId = siteId;
            this.masterEntityId = masterEntityId;
            this.synchStatus = synchStatus;
            this.guid = guid;
            this.externalSystemReference = externalSystemReference;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>
        [DisplayName("Id")]
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
        /// Get method of the ProfileTypeId field
        /// </summary>
        public int ProfileTypeId
        {
            get
            {
                return profileTypeId;
            }
            set
            {
                this.IsChanged = true;
                profileTypeId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the ProfileType field
        /// </summary>
        [DisplayName("Profile Type")]
        public ProfileType ProfileType
        {
            get
            {
                return profileType;
            }

            set
            {
                this.IsChanged = true;
                profileType = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Title field
        /// </summary>
        [DisplayName("Title")]
        public string Title
        {
            get
            {
                return title;
            }

            set
            {
                this.IsChanged = true;
                title = value;
            }
        }

        /// <summary>
        /// Get/Set method of the FirstName Text field
        /// </summary>
        [DisplayName("First Name")]
        public string FirstName
        {
            get
            {
                return firstName;
            }

            set
            {
                this.IsChanged = true;
                firstName = value;
            }
        }

        /// <summary>
        /// Get/Set method of the MiddleName Text field
        /// </summary>
        [DisplayName("Middle Name")]
        public string MiddleName
        {
            get
            {
                return middleName;
            }

            set
            {
                this.IsChanged = true;
                middleName = value;
            }
        }

        /// <summary>
        /// Get/Set method of the LastName Text field
        /// </summary>
        [DisplayName("Last Name")]
        public string LastName
        {
            get
            {
                return lastName;
            }

            set
            {
                this.IsChanged = true;
                lastName = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Notes Text field
        /// </summary>
        [DisplayName("Notes")]
        public string Notes
        {
            get
            {
                return notes;
            }

            set
            {
                this.IsChanged = true;
                notes = value;
            }
        }

        /// <summary>
        /// Get/Set method of the DateOfBirth Text field
        /// </summary>
        [DisplayName("Date Of Birth")]
        public DateTime? DateOfBirth
        {
            get
            {
                return dateOfBirth;
            }

            set
            {
                this.IsChanged = true;
                dateOfBirth = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Gender Text field
        /// </summary>
        [DisplayName("Gender")]
        public string Gender
        {
            get
            {
                return gender;
            }

            set
            {
                this.IsChanged = true;
                gender = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Anniversary Text field
        /// </summary>
        [DisplayName("Anniversary")]
        public DateTime? Anniversary
        {
            get
            {
                return anniversary;
            }

            set
            {
                this.IsChanged = true;
                anniversary = value;
            }
        }

        /// <summary>
        /// Get/Set method of the PhotoURL Text field
        /// </summary>
        [DisplayName("PhotoURL")]
        public string PhotoURL
        {
            get
            {
                return photoURL;
            }

            set
            {
                this.IsChanged = true;
                photoURL = value;
            }
        }

        /// <summary>
        /// Get/Set method of the RightHanded Text field
        /// </summary>
        [DisplayName("RightHanded")]
        public bool RightHanded
        {
            get
            {
                return rightHanded;
            }

            set
            {
                this.IsChanged = true;
                rightHanded = value;
            }
        }

        /// <summary>
        /// Get/Set method of the TeamUser Text field
        /// </summary>
        [DisplayName("TeamUser")]
        public bool TeamUser
        {
            get
            {
                return teamUser;
            }

            set
            {
                this.IsChanged = true;
                teamUser = value;
            }
        }

        /// <summary>
        /// Get/Set method of the UniqueIdentifier field
        /// </summary>
        [DisplayName("UniqueIdentifier")]
        public string UniqueIdentifier
        {
            get
            {
                return uniqueIdentifier;
            }

            set
            {
                this.IsChanged = true;
                uniqueIdentifier = value;
            }
        }

        /// <summary>
        /// Get/Set method of the IdProofFileURL field
        /// </summary>
        [DisplayName("IdProof File URL")]
        public string IdProofFileURL
        {
            get
            {
                return idProofFileURL;
            }

            set
            {
                this.IsChanged = true;
                idProofFileURL = value;
            }
        }

        /// <summary>
        /// Get/Set method of the TaxCode Text field
        /// </summary>
        [DisplayName("Tax Code")]
        public string TaxCode
        {
            get
            {
                return taxCode;
            }

            set
            {
                this.IsChanged = true;
                taxCode = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Designation Text field
        /// </summary>
        [DisplayName("Designation")]
        public string Designation
        {
            get
            {
                return designation;
            }

            set
            {
                this.IsChanged = true;
                designation = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Company Text field
        /// </summary>
        [DisplayName("Company")]
        public string Company
        {
            get
            {
                return company;
            }

            set
            {
                this.IsChanged = true;
                company = value;
            }
        }

        /// <summary>
        /// Get/Set method of the UserName Text field
        /// </summary>
        [DisplayName("User Name")]
        public string UserName
        {
            get
            {
                return userName;
            }

            set
            {
                this.IsChanged = true;
                userName = value;
            }
        }


        /// <summary>
        /// Get/Set method of the password Text field
        /// </summary>
        public string Password
        {
            get
            {
                return password;
            }

            set
            {
                this.IsChanged = true;
                password = value;
            }
        }


        /// <summary>
        /// Get/Set method of the lastLoginTime field
        /// </summary>
        public DateTime? LastLoginTime
        {
            get
            {
                return lastLoginTime;
            }

            set
            {
                this.IsChanged = true;
                lastLoginTime = value;
            }
        }

        /// <summary>
        /// Get/Set method of the contactDTOList field
        /// </summary>
        public List<ContactDTO> ContactDTOList
        {
            get
            {
                return contactDTOList;
            }

            set
            {
                contactDTOList = value;
            }
        }

        /// <summary>
        /// Get/Set method of the addressDTOList field
        /// </summary>
        public List<AddressDTO> AddressDTOList
        {
            get
            {
                return addressDTOList;
            }

            set
            {
                addressDTOList = value;
            }
        }


        /// <summary>
        /// Get/Set method of the ProfileContentHistoryDTOList field
        /// </summary>
        public List<ProfileContentHistoryDTO> ProfileContentHistoryDTOList
        {
            get
            {
                return profileContentHistoryDTOList;
            }

            set
            {
                profileContentHistoryDTOList = value;
            }
        }

        /// <summary>
        /// Get/Set method of the OptInPromotions field
        /// </summary>
        [DisplayName("OptInPromotions")]
        public bool OptInPromotions
        {
            get
            {
                return optInPromotions;
            }

            set
            {
                this.IsChanged = true;
                optInPromotions = value;
            }
        }

        /// <summary>
        /// Get/Set method of the OptInPromotionsMode field
        /// </summary>
        [DisplayName("OptInPromotionsMode")]
        public string OptInPromotionsMode
        {
            get
            {
                return optInPromotionsMode;
            }

            set
            {
                this.IsChanged = true;
                optInPromotionsMode = value;
            }
        }

        /// <summary>
        /// Get/Set method of the OptInLastUpdatedDate field
        /// </summary>
        [DisplayName("OptInLastUpdatedDate")]
        public DateTime? OptInLastUpdatedDate
        {
            get
            {
                return optInLastUpdatedDate;
            }

            set
            {
                this.IsChanged = true;
                optInLastUpdatedDate = value;
            }
        }

        /// <summary>
        /// Get/Set method of the PolicyTermsAccepted field
        /// </summary>
        [DisplayName("PolicyTermsAccepted")]
        public bool PolicyTermsAccepted
        {
            get
            {
                return policyTermsAccepted;
            }

            set
            {
                this.IsChanged = true;
                policyTermsAccepted = value;
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
        /// Get/Set method of the ExternalSystemReference field
        /// </summary>
        public string ExternalSystemReference
        {
            get
            {
                return externalSystemReference;
            }
            set
            {
                this.IsChanged = true;
                externalSystemReference = value;
            }
        }

        public bool OptOutWhatsApp
        {
            get
            {
                return optOutWhatsApp;
            }
            set
            {
                this.IsChanged = true;
                optOutWhatsApp = value;
            }
        }
        /// <summary>
        /// Get/Set method of the UserStatus field
        /// </summary>
        [DisplayName("User Status")]
        public string UserStatus { get { return userStatus; } set { userStatus = value; this.IsChanged = true; } }

        [DisplayName("Password Change Date")]
        public DateTime? PasswordChangeDate { get { return passwordChangeDate; } set { passwordChangeDate = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the InvalidAccessAttempts field
        /// </summary>
        [DisplayName("Invalid Access Attempts")]
        public int InvalidAccessAttempts { get { return invalidAccessAttempts; } set { invalidAccessAttempts = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the LockedOutTime field
        /// </summary>
        [DisplayName("Locked Out Time")]
        public DateTime? LockedOutTime { get { return lockedOutTime; } set { lockedOutTime = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the PasswordChangeOnNextLogin field
        /// </summary>
        [DisplayName("Password Change On Next Login?")]
        public bool PasswordChangeOnNextLogin { get { return passwordChangeOnNextLogin; } set { passwordChangeOnNextLogin = value; this.IsChanged = true; } }

        public string NickName
        {
            get
            {
                return nickName;
            }
            set
            {
                this.IsChanged = true;
                nickName = value; 
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
        /// Returns whether profile or any child record is changed
        /// </summary>
        [Browsable(false)]
        public bool IsChangedRecursive
        {
            get
            {
                bool isChangedRecursive = IsChanged;
                if (addressDTOList != null)
                {
                    foreach (var addressDTO in addressDTOList)
                    {
                        isChangedRecursive = isChangedRecursive || addressDTO.IsChanged;
                    }
                }
                if (contactDTOList != null)
                {
                    foreach (var contactDTO in contactDTOList)
                    {
                        isChangedRecursive = isChangedRecursive || contactDTO.IsChanged;
                    }
                }
                if (profileContentHistoryDTOList != null)
                {
                    foreach (var profileContentHistoryDTO in profileContentHistoryDTOList)
                    {
                        isChangedRecursive = isChangedRecursive || profileContentHistoryDTO.IsChanged;
                    }
                }
                return isChangedRecursive;
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
