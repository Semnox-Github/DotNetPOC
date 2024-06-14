/********************************************************************************************
 * Project Name - Customer DTO
 * Description  - Data object of Customer
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *1.00        02-Feb-2017   Lakshminarayana           Created 
 *2.70.2      19-Jul-2019   Girish Kundar             Modified : Added Constructor with required Parameter
 *2.70.2.0    26-Sep-2019   Guru S A                  Waiver phase 2 enhancement changes
 *2.70.3      14-Feb-2020   Lakshminarayana           Modified: Creating unregistered customer during check-in process
 *2.70.3      14-Feb-2020   Girish Kundar             Modified: Added GetCustomerSearchByParametersDisplayName() method to get the display name for search parameters
 *2.70.3      10-Mar-2020   Jeevan                    Modified: removed securitytoken DTO parameter
 *2.90        03-July-2020  Girish Kundar             Modified : Change as part of CardCodeDTOList replaced with AccountDTOList in CustomerDTO  
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.CardCore;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.Customer.Waivers;

namespace Semnox.Parafait.Customer
{
    /// <summary>
    /// This is the Customer data object class. This acts as data holder for the Customer business object
    /// </summary>
    public class CustomerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int id;
        private int profileId;
        private int membershipId;
        private string cardNumber;
        private string channel;
        private string externalSystemReference;
        private int customDataSetId;
        private bool verified;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private int siteId;
        private int masterEntityId;
        private bool synchStatus;
        private string guid;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private ProfileDTO profileDTO;
        private CustomDataSetDTO customDataSetDTO;
        private CustomerVerificationDTO customerVerificationDTO;
        //private List<CardCoreDTO> cardCoreDTOList;
        private List<AccountDTO> accountDTOList;
        private List<CustomerMembershipProgressionDTO> customerMembershipProgressionDTOList;
        private List<CustomerMembershipRewardsLogDTO> customerMembershipRewardsLogDTOList;
        private List<CustomerSignedWaiverDTO> customerSignedWaiverDTOList;
        //private List<TransactionDTO> customerOrders;
        private DateTime lastVisitedDate;
        private List<ActiveCampaignCustomerInfoDTO> activeCampaignCustomerInfoDTOList;
        private CustomerType customerType;

        //To be removed after reconciliation
        DataTable customerCuponsDT;

        /// <summary>
        /// Default constructor
        /// </summary>
        public CustomerDTO()
        {
            log.LogMethodEntry();
            id = -1;
            profileId = -1;
            membershipId = -1;
            masterEntityId = -1;
            customDataSetId = -1;
            isActive = true;
            profileDTO = new ProfileDTO();
            customDataSetDTO = new CustomDataSetDTO();
           // cardCoreDTOList = new List<CardCoreDTO>();
            accountDTOList = new List<AccountDTO>();
            customerMembershipProgressionDTOList = new List<CustomerMembershipProgressionDTO>();
            customerMembershipRewardsLogDTOList = new List<CustomerMembershipRewardsLogDTO>();
            customerSignedWaiverDTOList = new List<CustomerSignedWaiverDTO>();
	        activeCampaignCustomerInfoDTOList = new List<ActiveCampaignCustomerInfoDTO>();
            customerType = CustomerType.REGISTERED;
            siteId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public CustomerDTO(int id, int profileId, int membershipId, string channel,
                         int customDataSetId, bool verified, string externalSystemReference, bool isActive, string cardNumber, CustomerType customerType)
            : this()
        {
            log.LogMethodEntry(id, profileId, membershipId, channel, customDataSetId,
                               verified, externalSystemReference, isActive, cardNumber);
            this.id = id;
            this.profileId = profileId;
            this.membershipId = membershipId;
            this.channel = channel;
            this.customDataSetId = customDataSetId;
            this.verified = verified;
            this.externalSystemReference = externalSystemReference;
            this.cardNumber = cardNumber;
            this.isActive = isActive;
            this.customerType = customerType;
            if (customDataSetId < 0)
            {
                customDataSetDTO = new CustomDataSetDTO();
            }
            //if (profileId < 0)
            //{
            //    profileDTO = new ProfileDTO();
            //}
            customerMembershipProgressionDTOList = new List<CustomerMembershipProgressionDTO>();
            customerMembershipRewardsLogDTOList = new List<CustomerMembershipRewardsLogDTO>();
            customerSignedWaiverDTOList = new List<CustomerSignedWaiverDTO>();
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public CustomerDTO(int id, int profileId, int membershipId, string channel,
                           int customDataSetId, bool verified, string externalSystemReference,
                           bool isActive, string cardNumber, CustomerType customerType, string createdBy,
                           DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate,
                           int siteId, int masterEntityId, bool synchStatus, string guid)
            : this(id, profileId, membershipId, channel, customDataSetId, verified, externalSystemReference, isActive, cardNumber, customerType)
        {
            log.LogMethodEntry(id, profileId, membershipId, channel, customDataSetId,
                               verified, externalSystemReference,
                               createdBy, creationDate, lastUpdatedBy, lastUpdateDate,
                               siteId, masterEntityId, synchStatus, guid);

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
        [DisplayName("Customer Id")]
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
        /// Get/Set method of the isActive field
        /// </summary>
        [DisplayName("IsActive")]
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
        /// Get/Set method of the ProfileId field
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
        /// Get/Set method of the MembershipId field
        /// </summary>
        [DisplayName("Membership")]
        public int MembershipId
        {
            get
            {
                return membershipId;
            }

            set
            {
                this.IsChanged = true;
                membershipId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Title Text field
        /// </summary>
        [DisplayName("Title")]
        public string Title
        {
            get
            {
                return profileDTO == null ? "" : profileDTO.Title;
            }

            set
            {
                if (profileDTO != null)
                {
                    profileDTO.Title = value;
                }
            }
        }

        /// <summary>
        /// Get/Set method of the FirstName Text field
        /// </summary>
        [DisplayName("FirstName")]
        public string FirstName
        {
            get
            {
                return profileDTO == null ? "" : profileDTO.FirstName;
            }

            set
            {
                if (profileDTO != null)
                {
                    profileDTO.FirstName = value;
                }
            }
        }

        /// <summary>
        /// Get/Set method of the MiddleName Text field
        /// </summary>
        [DisplayName("Middle")]
        public string MiddleName
        {
            get
            {
                return profileDTO == null ? "" : profileDTO.MiddleName;
            }

            set
            {
                if (profileDTO != null)
                {
                    profileDTO.MiddleName = value;
                }
            }
        }

        /// <summary>
        /// Get/Set method of the LastName Text field
        /// </summary>
        [DisplayName("Last")]
        public string LastName
        {
            get
            {
                return profileDTO == null ? "" : profileDTO.LastName;
            }

            set
            {
                if (profileDTO != null)
                {
                    profileDTO.LastName = value;
                }
            }
        }

        /// <summary>
        /// Get/Set method of the ExternalSystemReference Text field
        /// </summary>
        [DisplayName("External System Reference")]
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

        /// <summary>
        /// Get/Set method of the CustomerType Text field
        /// </summary>
        [DisplayName("Type")]
        public CustomerType CustomerType
        {
            get
            {
                return customerType;
            }

            set
            {
                this.IsChanged = true;
                customerType = value;
            }
        }

        /// <summary>
        /// Get/Set method of the UniqueIdentifier Text field
        /// </summary>
        [DisplayName("Unique_ID")]
        public string UniqueIdentifier
        {
            get
            {
                return profileDTO == null ? "" : profileDTO.UniqueIdentifier;
            }

            set
            {
                if (profileDTO != null)
                {
                    profileDTO.UniqueIdentifier = value;
                }
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
                return profileDTO == null ? "" : profileDTO.TaxCode;
            }

            set
            {
                if (profileDTO != null)
                {
                    profileDTO.TaxCode = value;
                }
            }
        }

        /// <summary>
        /// Get/Set method of the DateOfBirth Text field
        /// </summary>
        [DisplayName("Date of Birth")]
        public DateTime? DateOfBirth
        {
            get
            {
                return profileDTO == null ? null : profileDTO.DateOfBirth;
            }

            set
            {
                if (profileDTO != null)
                {
                    profileDTO.DateOfBirth = value;
                }
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
                return profileDTO == null ? "" : profileDTO.Gender;
            }

            set
            {
                if (profileDTO != null)
                {
                    profileDTO.Gender = value;
                }
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
                return profileDTO == null ? null : profileDTO.Anniversary;
            }

            set
            {
                if (profileDTO != null)
                {
                    profileDTO.Anniversary = value;
                }
            }
        }

        /// <summary>
        /// Get/Set method of the TeamUser field
        /// </summary>
        [DisplayName("Team User")]
        public bool TeamUser
        {
            get
            {
                return profileDTO == null ? false : profileDTO.TeamUser;
            }

            set
            {
                if (profileDTO != null)
                {
                    profileDTO.TeamUser = value;
                }
            }
        }

        /// <summary>
        /// Get/Set method of the RightHanded field
        /// </summary>
        [DisplayName("Right Handed")]
        public bool RightHanded
        {
            get
            {
                return profileDTO == null ? false : profileDTO.RightHanded;
            }

            set
            {
                if (profileDTO != null)
                {
                    profileDTO.RightHanded = value;
                }
            }
        }

        /// <summary>
        /// Get/Set method of the Promotion Opt in field
        /// </summary>
        [DisplayName("Promotion Opt-in")]
        public bool OptInPromotions
        {
            get
            {
                return profileDTO == null ? false : profileDTO.OptInPromotions;
            }

            set
            {
                if (profileDTO != null)
                {
                    profileDTO.OptInPromotions = value;
                }
            }
        }

        /// <summary>
        /// Get/Set method of the Promotion mode field
        /// </summary>
        [DisplayName("Promotion mode")]
        public string OptInPromotionsMode
        {
            get
            {
                return profileDTO == null ? string.Empty : profileDTO.OptInPromotionsMode;
            }

            set
            {
                if (profileDTO != null)
                {
                    profileDTO.OptInPromotionsMode = value;
                }
            }
        }
        /// <summary>
        /// Get/Set method of the PolicyTermsAccepted in field
        /// </summary>
        [DisplayName("PolicyTermsAccepted")]
        public bool PolicyTermsAccepted
        {
            get
            {
                return profileDTO == null ? false : profileDTO.PolicyTermsAccepted;
            }

            set
            {
                if (profileDTO != null)
                {
                    profileDTO.PolicyTermsAccepted = value;
                }
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
                return profileDTO == null ? "" : profileDTO.Company;
            }

            set
            {
                if (profileDTO != null)
                {
                    profileDTO.Company = value;
                }
            }
        }

        /// <summary>
        /// Get/Set method of the Username Text field
        /// </summary>
        [DisplayName("Username")]
        public string UserName
        {
            get
            {
                return profileDTO == null ? "" : profileDTO.UserName;
            }

            set
            {
                if (profileDTO != null)
                {
                    profileDTO.UserName = value;
                }
            }
        }

        /// <summary>
        /// Get/Set method of the PhotoURL Text field
        /// </summary>
        [DisplayName("Customer Photo")]
        public string PhotoURL
        {
            get
            {
                return profileDTO == null ? "" : profileDTO.PhotoURL;
            }

            set
            {
                if (profileDTO != null)
                {
                    profileDTO.PhotoURL = value;
                }
            }
        }

        /// <summary>
        /// Get/Set method of the IdProofFileURL Text field
        /// </summary>
        [DisplayName("Customer ID Proof")]
        public string IdProofFileURL
        {
            get
            {
                return profileDTO == null ? "" : profileDTO.IdProofFileURL;
            }

            set
            {
                if (profileDTO != null)
                {
                    profileDTO.IdProofFileURL = value;
                }
            }
        }

        /// <summary>
        /// Get/Set method of the LastLoginTime Text field
        /// </summary>
        [DisplayName("Last Login Time")]
        public DateTime? LastLoginTime
        {
            get
            {
                return profileDTO == null ? null : profileDTO.LastLoginTime;
            }

            set
            {
                if (profileDTO != null)
                {
                    profileDTO.LastLoginTime = value;
                }
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
                return profileDTO == null ? "" : profileDTO.Designation;
            }

            set
            {
                if (profileDTO != null)
                {
                    profileDTO.Designation = value;
                }
            }
        }

        /// <summary>
        /// Get/Set method of the CustomDataSetId Text field
        /// </summary>
        [DisplayName("Custom Data")]
        public int CustomDataSetId
        {
            get
            {
                return customDataSetId;
            }

            set
            {
                this.IsChanged = true;
                customDataSetId = value;
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
                return profileDTO == null ? "" : profileDTO.Notes;
            }

            set
            {
                if (profileDTO != null)
                {
                    profileDTO.Notes = value;
                }
            }
        }

        /// <summary>
        /// Get/Set method of the cardNumber field
        /// </summary>
        [DisplayName("Card Number")]
        public string CardNumber
        {
            get
            {
                return cardNumber;
            }
            set
            {
                this.IsChanged = true;
                cardNumber = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Channel Text field
        /// </summary>
        [DisplayName("Channel")]
        public string Channel
        {
            get
            {
                return channel;
            }

            set
            {
                this.IsChanged = true;
                channel = value;
            }
        }



        /// <summary>
        /// Get/Set method of the Verified Text field
        /// </summary>
        [DisplayName("Verified")]
        public bool Verified
        {
            get
            {
                return verified;
            }

            set
            {
                this.IsChanged = true;
                verified = value;
            }
        }

        /// <summary>
        /// Get/Set method of the addressDTOList field
        /// </summary>
        public List<AddressDTO> AddressDTOList
        {
            get
            {
                return profileDTO == null ? null : profileDTO.AddressDTOList;
            }

            set
            {
                if (profileDTO != null)
                {
                    profileDTO.AddressDTOList = value;
                }
            }
        }

        /// <summary>
        /// Get/Set method of the contactDTOList field
        /// </summary>
        public List<ContactDTO> ContactDTOList
        {
            get
            {
                return profileDTO == null ? null : profileDTO.ContactDTOList;
            }

            set
            {
                if (profileDTO != null)
                {
                    profileDTO.ContactDTOList = value;
                }
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
                if (ProfileId != profileDTO.Id)
                {
                    ProfileId = profileDTO.Id;
                }
            }
        }

        /// <summary>
        /// Get/Set method of the customerVerificationDTO field
        /// </summary>
        public CustomerVerificationDTO CustomerVerificationDTO
        {
            get
            {
                return customerVerificationDTO;
            }

            set
            {
                customerVerificationDTO = value;
            }
        }

        /// <summary>
        /// Get/Set method of the customDataSetDTO field
        /// </summary>
        public CustomDataSetDTO CustomDataSetDTO
        {
            get
            {
                return customDataSetDTO;
            }

            set
            {
                customDataSetDTO = value;
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
                this.IsChanged = true;
                creationDate = value;
            }
        }
        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("Last Updated By")]
        public string LastUpdatedBy
        {
            get
            {
                return lastUpdatedBy;
            }
            set
            {
                this.IsChanged = true;
                lastUpdatedBy = value;
            }
        }
        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        [DisplayName("Last Update Date")]
        public DateTime LastUpdateDate
        {
            get
            {
                return lastUpdateDate;
            }

            set
            {
                this.IsChanged = true;
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
                this.IsChanged = true;
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
                this.IsChanged = true;
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
        /// Get/Set method of the Phone Number field
        /// </summary>
        public string PhoneNumber
        {
            get
            {
                string phoneNumber = string.Empty;
                if (profileDTO.ContactDTOList != null && profileDTO.ContactDTOList.Count > 0)
                {
                    ContactDTO contactDTO = profileDTO.ContactDTOList.Where((x) => x.ContactType == ContactType.PHONE && x.IsActive).OrderByDescending((x) => x.LastUpdateDate).FirstOrDefault();
                    if (contactDTO != null)
                    {
                        phoneNumber = contactDTO.Attribute1;
                    }
                }
                return phoneNumber;
            }
            set { }
        }

        /// <summary>
        /// Get/Set method of password field
        /// </summary>
        public string Password
        {
            get
            {
                return profileDTO != null ? profileDTO.Password : "";
            }

            set
            {
                if (profileDTO != null)
                {
                    profileDTO.Password = value;
                }
            }
        }

        /// <summary>
        /// Returns the latest address DTO
        /// </summary>
        public AddressDTO LatestAddressDTO
        {
            get
            {
                AddressDTO addressDTO = new AddressDTO();
                if (profileDTO.AddressDTOList != null && profileDTO.AddressDTOList.Count > 0)
                {
                    addressDTO = profileDTO.AddressDTOList.Where((x) => x.IsActive).OrderByDescending((x) => x.LastUpdateDate).FirstOrDefault();
                }
                return addressDTO;
            }

        }

        /// <summary>
        /// Get/Set method of the Phone Number field
        /// </summary>
        public string SecondaryPhoneNumber
        {
            get
            {
                string phoneNumber = string.Empty;
                if (profileDTO.ContactDTOList != null && profileDTO.ContactDTOList.Count > 0)
                {
                    var orderedContactPhoneList = profileDTO.ContactDTOList.Where((x) => x.ContactType == ContactType.PHONE && x.IsActive).OrderByDescending((x) => x.LastUpdateDate);
                    if (orderedContactPhoneList.Count() > 1)
                    {
                        ContactDTO contactDTO = orderedContactPhoneList.Skip(1).Take(1).FirstOrDefault();
                        phoneNumber = contactDTO.Attribute1;
                    }
                }
                return phoneNumber;
            }
            set { }
        }

        /// <summary>
        /// Get/Set method of the FBUserId field
        /// </summary>
        public string FBUserId
        {
            get
            {
                string fBUserId = string.Empty;
                if (profileDTO.ContactDTOList != null && profileDTO.ContactDTOList.Count > 0)
                {
                    ContactDTO contactDTO = profileDTO.ContactDTOList.Where((x) => x.ContactType == ContactType.FACEBOOK && x.IsActive).OrderByDescending((x) => x.LastUpdateDate).FirstOrDefault();
                    if (contactDTO != null)
                    {
                        fBUserId = contactDTO.Attribute1;
                    }
                }
                return fBUserId;
            }
            set { }
        }

        /// <summary>
        /// Get/Set method of the FBAccessToken field
        /// </summary>
        public string FBAccessToken
        {
            get
            {
                string fBAccessToken = string.Empty;
                if (profileDTO.ContactDTOList != null && profileDTO.ContactDTOList.Count > 0)
                {
                    ContactDTO contactDTO = profileDTO.ContactDTOList.Where((x) => x.ContactType == ContactType.FACEBOOK && x.IsActive).OrderByDescending((x) => x.LastUpdateDate).FirstOrDefault();
                    if (contactDTO != null)
                    {
                        fBAccessToken = contactDTO.Attribute2;
                    }
                }
                return fBAccessToken;
            }
            set { }
        }

        /// <summary>
        /// Get/Set method of the FBAccessToken field
        /// </summary>
        public string TWAccessToken
        {
            get
            {
                string tWAccessToken = string.Empty;
                if (profileDTO.ContactDTOList != null && profileDTO.ContactDTOList.Count > 0)
                {
                    ContactDTO contactDTO = profileDTO.ContactDTOList.Where((x) => x.ContactType == ContactType.TWITTER && x.IsActive).OrderByDescending((x) => x.LastUpdateDate).FirstOrDefault();
                    if (contactDTO != null)
                    {
                        tWAccessToken = contactDTO.Attribute1;
                    }
                }
                return tWAccessToken;
            }
            set { }
        }

        /// <summary>
        /// Get/Set method of the FBAccessToken field
        /// </summary>
        public string TWAccessSecret
        {
            get
            {
                string tWAccessSecret = string.Empty;
                if (profileDTO.ContactDTOList != null && profileDTO.ContactDTOList.Count > 0)
                {
                    ContactDTO contactDTO = profileDTO.ContactDTOList.Where((x) => x.ContactType == ContactType.TWITTER && x.IsActive).OrderByDescending((x) => x.LastUpdateDate).FirstOrDefault();
                    if (contactDTO != null)
                    {
                        tWAccessSecret = contactDTO.Attribute2;
                    }
                }
                return tWAccessSecret;
            }
            set { }
        }

        /// <summary>
        /// Get/Set method of the Email field
        /// </summary>
        public string Email
        {
            get
            {
                string email = string.Empty;
                if (profileDTO.ContactDTOList != null && profileDTO.ContactDTOList.Count > 0)
                {
                    ContactDTO contactDTO = profileDTO.ContactDTOList.Where((x) => x.ContactType == ContactType.EMAIL && x.IsActive).OrderByDescending((x) => x.LastUpdateDate).FirstOrDefault();
                    if (contactDTO != null)
                    {
                        email = contactDTO.Attribute1;
                    }
                }
                return email;
            }
            set { }
        }

        /// <summary>
        /// Get/Set method of the WeChatAccessToken field
        /// </summary>
        public string WeChatAccessToken
        {
            get
            {
                string weChatAccessToken = string.Empty;
                if (profileDTO.ContactDTOList != null && profileDTO.ContactDTOList.Count > 0)
                {
                    if (profileDTO.ContactDTOList != null && profileDTO.ContactDTOList.Count > 0)
                    {
                        ContactDTO contactDTO = profileDTO.ContactDTOList.Where((x) => x.ContactType == ContactType.WECHAT && x.IsActive).OrderByDescending((x) => x.LastUpdateDate).FirstOrDefault();
                        if (contactDTO != null)
                        {
                            weChatAccessToken = contactDTO.Attribute1;
                        }
                    }
                }
                return weChatAccessToken;
            }
            set { }
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
        /// Returns whether customer or any child record is changed
        /// </summary>
        [Browsable(false)]
        public bool IsChangedRecursive
        {
            get
            {
                bool isChangedRecursive = IsChanged;
                if (profileDTO != null)
                {
                    isChangedRecursive = isChangedRecursive || profileDTO.IsChangedRecursive;
                }
                if (customDataSetDTO != null)
                {
                    isChangedRecursive = isChangedRecursive || customDataSetDTO.IsChangedRecursive;
                }
                return isChangedRecursive;
            }
        }

        /// <summary>
        /// Get/Set methods for customerCuponsDT field
        /// </summary>
        public DataTable CustomerCuponsDT
        {
            get
            {
                return customerCuponsDT;
            }

            set
            {
                customerCuponsDT = value;
            }
        }

        /// <summary>
        /// Get/Set methods for cardCoreDTOList 
        /// </summary>
        public List<AccountDTO> AccountDTOList
        {
            get
            {
                return accountDTOList;
            }

            set
            {
                accountDTOList = value;
            }
        }



        /// <summary>
        /// Get/Set methods for customerMembershipProgressionDTOList 
        /// </summary>
        public List<CustomerMembershipProgressionDTO> CustomerMembershipProgressionDTOList
        {
            get
            {
                return customerMembershipProgressionDTOList;
            }

            set
            {
                customerMembershipProgressionDTOList = value;
            }
        }

        /// <summary>
        /// Get/Set methods for CustomerMembershipRewardsLogDTOList 
        /// </summary>
        public List<CustomerMembershipRewardsLogDTO> CustomerMembershipRewardsLogDTOList
        {
            get
            {
                return customerMembershipRewardsLogDTOList;
            }

            set
            {
                customerMembershipRewardsLogDTOList = value;
            }
        }
        /// <summary>
        /// Get/Set methods for CustomerSignedWaiverDTOList 
        /// </summary>
        public List<CustomerSignedWaiverDTO> CustomerSignedWaiverDTOList
        {
            get { return customerSignedWaiverDTOList; }
            set { customerSignedWaiverDTOList = value; }
        }
        

        /// <summary>
        /// Get/Set methods for ActiveCampaignCustomerInfoDTOList 
        /// </summary>
        public List<ActiveCampaignCustomerInfoDTO> ActiveCampaignCustomerInfoDTOList
        {
            get
            {
                return activeCampaignCustomerInfoDTOList;
            }

            set
            {
                activeCampaignCustomerInfoDTOList = value;
            }
        }

        /// <summary>
        /// Get/Set methods for LastVisitedDate 
        /// </summary>
        public DateTime LastVisitedDate
        {
            get
            {
                return lastVisitedDate;
            }

            set
            {
                lastVisitedDate = value;
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


        public static string GetCustomerSearchByParametersDisplayName(ExecutionContext executionContext, string customerSearchByParameterName)
        {
            log.LogMethodEntry(executionContext, customerSearchByParameterName);
            string searchParamDisplayName = string.Empty;
            switch (customerSearchByParameterName)
            {
                case "PROFILE_FIRST_NAME": searchParamDisplayName = MessageContainerList.GetMessage(executionContext, "First Name"); break;
                case "PROFILE_MIDDLE_NAME": searchParamDisplayName = MessageContainerList.GetMessage(executionContext, "Middle Name"); break;
                case "PROFILE_LAST_NAME": searchParamDisplayName = MessageContainerList.GetMessage(executionContext, "Last Name"); break;
                case "PROFILE_TITLE": searchParamDisplayName = MessageContainerList.GetMessage(executionContext, "Title"); break;
                case "PROFILE_NOTES": searchParamDisplayName = MessageContainerList.GetMessage(executionContext, "Notes"); break;
                case "PROFILE_DATE_OF_BIRTH": searchParamDisplayName = MessageContainerList.GetMessage(executionContext, "Date of Birth"); break;
                case "PROFILE_UNIQUE_IDENTIFIER": searchParamDisplayName = MessageContainerList.GetMessage(executionContext, "Unique Identifier"); break;
                case "PROFILE_TAX_CODE": searchParamDisplayName = MessageContainerList.GetMessage(executionContext, "TaxCode"); break;
                case "PROFILE_USER_NAME": searchParamDisplayName = MessageContainerList.GetMessage(executionContext, "Username"); break;
                case "ADDRESS_LINE1": searchParamDisplayName = MessageContainerList.GetMessage(executionContext, "Address Line 1"); break;
                case "ADDRESS_CITY": searchParamDisplayName = MessageContainerList.GetMessage(executionContext, "City"); break;
                case "ADDRESS_POSTAL_CODE": searchParamDisplayName = MessageContainerList.GetMessage(executionContext, "Postal Code"); break;
                case "PHONE_NUMBER_LIST": searchParamDisplayName = MessageContainerList.GetMessage(executionContext, "Phone Number"); break;
                case "EMAIL_LIST": searchParamDisplayName = MessageContainerList.GetMessage(executionContext, "Email List"); break;
                case "PHONE": searchParamDisplayName = MessageContainerList.GetMessage(executionContext, "Phone Number"); break;
                case "EMAIL": searchParamDisplayName = MessageContainerList.GetMessage(executionContext, "Email"); break;
                case "ADDRESS_LINE2": searchParamDisplayName = MessageContainerList.GetMessage(executionContext, "Address Line 2"); break;
                case "WECHAT_LIST": searchParamDisplayName = MessageContainerList.GetMessage(executionContext, "WeChat"); break;
                case "FB_USERID_LIST": searchParamDisplayName = MessageContainerList.GetMessage(executionContext, "Facebook User Id"); break;
                case "TW_ACCESS_TOKEN_LIST": searchParamDisplayName = MessageContainerList.GetMessage(executionContext, "Twitter Access Token"); break;
                case "FB_ACCESS_TOKEN_LIST": searchParamDisplayName = MessageContainerList.GetMessage(executionContext, "Facebook Token"); break;
                case "TW_ACCESS_SECRET_LIST": searchParamDisplayName = MessageContainerList.GetMessage(executionContext, "Twitter Secret Key"); break;
                case "ADDRESS_ADDRESS_TYPE": searchParamDisplayName = MessageContainerList.GetMessage(executionContext, "Address Type"); break;
                case "CONTACT_CONTACT_TYPE": searchParamDisplayName = MessageContainerList.GetMessage(executionContext, "Contact Type"); break;
                case "ADDRESS_STATE_ID": searchParamDisplayName = MessageContainerList.GetMessage(executionContext, "State "); break;
                case "ADDRESS_COUNTRY_ID": searchParamDisplayName = MessageContainerList.GetMessage(executionContext, "Country "); break;
                case "PROFILE_GENDER": searchParamDisplayName = MessageContainerList.GetMessage(executionContext, "Gender "); break;
                case "PROFILE_DESIGNATION": searchParamDisplayName = MessageContainerList.GetMessage(executionContext, "Designation "); break;
                case "PROFILE_PASSWORD": searchParamDisplayName = MessageContainerList.GetMessage(executionContext, "Password "); break;
                case "PROFILE_EXTERNAL_SYSTEM_REFERENCE": searchParamDisplayName = MessageContainerList.GetMessage(executionContext, "External Reference Number "); break;
                case "PROFILE_ANNIVERSARY": searchParamDisplayName = MessageContainerList.GetMessage(executionContext, "Anniversary Date "); break;
            }
            log.LogMethodExit(searchParamDisplayName);
            return searchParamDisplayName;
        }
    }
}
