/********************************************************************************************
 * Project Name - Membership
 * Description  - DTO of Membership
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.2       19-Jul-2019    Girish Kundar  Modified : Added Constructor with required Parameter
 *                                                    and IsChangedRecurssive for DTOList.  
  ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Semnox.Parafait.Customer.Membership
{
    /// <summary>
    /// This is the Membership data object class. This acts as data holder for the Membership business object
    /// </summary>
    public class MembershipDTO
    {
        private static  readonly Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>

        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  MEMBERSHIP_ID field
            /// </summary>
            MEMBERSHIP_ID ,
            /// <summary>
            /// Search by IsActive field
            /// </summary>
            IS_ACTIVE ,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID ,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by MEMBERSHIP_NAME field
            /// </summary>
            MEMBERSHIP_NAME
        }

        private int membershipID;
        private string membershipName;
        private string description;
        private bool vip;
        private bool autoApply;
        private int baseMembershipID;
        private int membershipRuleID;
        private double redemptionDiscount;
        private int priceListId;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private int siteId;
        private int masterEntityId;
        private bool synchStatus;
        private string guid;
        private IList<MembershipRewardsDTO> membershipRewardsDTOList;
        private MembershipRuleDTO membershipRuleDTORecord;

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public MembershipDTO()
        {
            log.LogMethodEntry();
            membershipID = -1;
            membershipRuleID = -1;
            baseMembershipID = -1;
            membershipName = "";
            masterEntityId = -1;
            priceListId = -1;
            vip = false;
            autoApply = false;
            isActive = true;
            redemptionDiscount = 0;
            membershipRewardsDTOList = new List<MembershipRewardsDTO>();
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with Required data fields
        /// </summary>
        public MembershipDTO(int membershipID, string membershipName, string description, bool vip, bool autoApply,
                                 int baseMembershipID, int membershipRuleID, double redemptionDiscount, int priceListId,
                                 bool isActive)
            :this()
        {
            log.LogMethodEntry(membershipID,  membershipName,  description,  vip,  autoApply,
                                  baseMembershipID,  membershipRuleID,  redemptionDiscount, priceListId,  isActive);
            this.membershipID = membershipID;
            this.membershipName = membershipName;
            this.description = description;
            this.vip = vip;
            this.autoApply = autoApply;
            this.baseMembershipID = baseMembershipID;
            this.membershipRuleID = membershipRuleID;
            this.redemptionDiscount = redemptionDiscount;
            this.priceListId = priceListId;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public MembershipDTO(int membershipID, string membershipName, string description, bool vip, bool autoApply,
                                 int baseMembershipID, int membershipRuleID, double redemptionDiscount, int priceListId,
                                 bool isActive, string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdatedDate,
                                 int siteId, int masterEntityId, bool synchStatus, string guid)
            :this(membershipID, membershipName, description, vip, autoApply,
                                  baseMembershipID, membershipRuleID, redemptionDiscount, priceListId, isActive)
        {
            log.LogMethodEntry(membershipID, membershipName, description, vip, autoApply,
                                  baseMembershipID, membershipRuleID, redemptionDiscount, priceListId, isActive,
                                  createdBy, creationDate, lastUpdatedBy, lastUpdatedDate,
                                  siteId, masterEntityId, synchStatus, guid);
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.siteId = siteId;
            this.masterEntityId = masterEntityId;
            this.synchStatus = synchStatus;
            this.guid = guid;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the MembershipID field
        /// </summary>
        [DisplayName("Membership ID")]
        [ReadOnly(true)]
        public int MembershipID
        {
            get
            {
                return membershipID;
            }

            set
            {
                this.IsChanged = true;
                membershipID = value;
            }
        }

        /// <summary>
        /// Get/Set method of the MembershipName field
        /// </summary>
        [DisplayName("Membership Name")]
        public string MembershipName
        {
            get
            {
                return membershipName;
            }

            set
            {
                this.IsChanged = true;
                membershipName = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Description field
        /// </summary>
        [DisplayName("Description")]
        public string Description
        {
            get
            {
                return description;
            }

            set
            {
                this.IsChanged = true;
                description = value;
            }
        }

        /// <summary>
        /// Get/Set method of the VIP field
        /// </summary>
        [DisplayName("VIP")]
        public bool VIP
        {
            get
            {
                return vip;
            }

            set
            {
                this.IsChanged = true;
                vip = value;
            }
        }

        /// <summary>
        /// Get/Set method of the AutoApply field
        /// </summary>
        [DisplayName("Auto Apply")]
        public bool AutoApply
        {
            get
            {
                return autoApply;
            }

            set
            {
                this.IsChanged = true;
                autoApply = value;
            }
        }
        /// <summary>
        /// Get/Set method of the BaseMembershipID field
        /// </summary>
        [DisplayName("Base Membership")]
        public int BaseMembershipID
        {
            get
            {
                return baseMembershipID;
            }

            set
            {
                this.IsChanged = true;
                baseMembershipID = value;
            }
        }

        /// <summary>
        /// Get/Set method of the MembershipRuleID field
        /// </summary>
        [DisplayName("Membership Rule")]
        public int MembershipRuleID
        {
            get
            {
                return membershipRuleID;
            }

            set
            {
                this.IsChanged = true;
                membershipRuleID = value;
            }
        }

        /// <summary>
        /// Get/Set method of the RedemptionDiscount field
        /// </summary>
        [DisplayName("Redemption Discount")]
        public double RedemptionDiscount
        {
            get
            {
                return redemptionDiscount;
            }

            set
            {
                this.IsChanged = true;
                redemptionDiscount = value;
            }
        }

        /// <summary>
        /// Get/Set method of the PriceListId field
        /// </summary>
        [DisplayName("Price List")]
        public int PriceListId
        {
            get
            {
                return priceListId;
            }

            set
            {
                this.IsChanged = true;
                priceListId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("Is Active")]
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
        /// Get method of the CreatedBy field
        /// </summary>
        [DisplayName("Created By")]
        [Browsable(false)]
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
        /// Get method of the CreationDate field
        /// </summary>
        [DisplayName("Created Date")]
        [Browsable(false)]
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
        /// Get method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("Last Updated By")]
        [Browsable(false)]
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
        /// Get method of the LastUpdatedDate field
        /// </summary>
        [DisplayName("Last Updated Date")]
        [Browsable(false)]
        public DateTime LastUpdatedDate
        {
            get
            {
                return lastUpdatedDate;
            }
            set
            {
                lastUpdatedDate = value;
            }
        }

        /// <summary>
        /// Get method of the SiteId field
        /// </summary>
        [Browsable(false)]
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
        [Browsable(false)]
        public int MasterEntityId
        {
            get
            {
                return masterEntityId;
            }
            set
            {
                masterEntityId = value;
            }
        }
        /// <summary>
        /// Member ship rewards
        /// </summary>
        [Browsable(false)]
        public IList<MembershipRewardsDTO> MembershipRewardsDTOList
        {
            get
            {
                return membershipRewardsDTOList;
            }

            set
            {
                this.IsChanged = true;
                membershipRewardsDTOList = value;
            }
        }
        /// <summary>
        /// Member ship rule
        /// </summary>
        [Browsable(false)]
        public MembershipRuleDTO MembershipRuleDTORecord
        {
            get
            {
                return membershipRuleDTORecord;
            }
            set
            { 
                membershipRuleDTORecord = value;
            }

        }
        /// <summary>
        /// Get method of the SynchStatus field
        /// </summary>
        [Browsable(false)]
        public bool SynchStatus
        {
            get
            {
                return synchStatus;
            }
        }

        /// <summary>
        /// Get method of the Guid field
        /// </summary>
        [Browsable(false)]
        public string Guid
        {
            get
            {
                return guid;
            }
            set
            {
                guid = value;
            }
        }


        /// <summary>
        /// Returns whether the MembershipDTO is changed or any  membershipRewardsDTO is changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (membershipRewardsDTOList != null &&
                    membershipRewardsDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                return false;
            }
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
                    return notifyingObjectIsChanged || membershipID < 0;
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
