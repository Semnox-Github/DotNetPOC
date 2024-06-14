/********************************************************************************************
 * Project Name - CardDiscounts DTO
 * Description  - Data object of CardDiscounts
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *1.00        15-Jul-2017   Lakshminarayana          Created 
 *2.80.0      19-Mar-2020   Mathew NInan            Added new field ValidityStatus to track
 *                                                  status of entitlements
 *2.110.0     11-Jan-2021   Guru S A                Subscription changes 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.CardCore
{
    /// <summary>
    /// This is the CardDiscounts data object class. This acts as data holder for the CardDiscounts business object
    /// </summary>
    public class CardDiscountsDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by CardDiscountId field
            /// </summary>
            CARD_DISCOUNT_ID,
            /// <summary>
            /// Search by CardId field
            /// </summary>
            CARD_ID,
            /// <summary>
            /// Search by ExpiryDate Greater Than field
            /// </summary>
            EXPIRY_DATE_GREATER_THAN,
            /// <summary>
            /// Search by ExpiryDate Less Than field
            /// </summary>
            EXPIRY_DATE_LESS_THAN,
            /// <summary>
            /// Search by DiscountId field
            /// </summary>
            DISCOUNT_ID,
            /// <summary>
            /// Search by IsActive field
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search by site_id field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MasterEntityId field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by TransactionId field
            /// </summary>
            TRANSACTION_ID,
            /// <summary>
            /// Search by LineId field
            /// </summary>
            LINE_ID,
            /// <summary>
            /// Search by TaskId field
            /// </summary>
            TASK_ID,
            /// <summary>
            /// Search by expireWithMembership field
            /// </summary>
            EXPIREWITHMEMBERSHIP,
            /// <summary>
            /// Search by membershipRewardsId field
            /// </summary>
            MEMBERSHIPREWARDSID,
            /// <summary>
            /// Search by membershipId field
            /// </summary>
            MEMBERSHIPSID,
            /// <summary>
            /// Search by ValidityStatus field
            /// </summary>
            VALIDITYSTATUS
        }

        int cardDiscountId;
        int cardId;
        int discountId;
        DateTime? expiryDate;
        string lastUpdatedUser;
        DateTime lastUpdatedDate;
        int? internetKey;
        int cardTypeId;
        int transactionId;
        int lineId;
        int taskId;
        string isActive;

        int siteId;
        int masterEntityId;
        bool synchStatus;
        string guid;
        string expireWithMembership;
        int membershipRewardsId;
        int membershipId;
        string createdBy;
        DateTime? creationDate;
        CardCoreDTO.CardValidityStatus validityStatus;
        int subscriptionBillingScheduleId;

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public CardDiscountsDTO()
        {
            log.LogMethodEntry();
            cardDiscountId = -1;
            discountId = -1;
            cardId = -1;
            cardTypeId = -1;
            transactionId = -1;
            lineId = -1;
            taskId = -1;
            isActive = "Y";
            masterEntityId = -1; 
            this.membershipId = -1;
            this.membershipRewardsId = -1;
            expireWithMembership = "N";
            validityStatus = CardCoreDTO.CardValidityStatus.Valid;
            subscriptionBillingScheduleId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public CardDiscountsDTO(int cardDiscountId, int cardId, int discountId, DateTime? expiryDate, 
                                int transactionId, int lineId, int taskId, string lastUpdatedUser,
                                DateTime lastUpdatedDate, int? internetKey, int cardTypeId, string isActive, int siteId,
                                int masterEntityId, bool synchStatus, string guid, string expireWithMembership,
                                 int membershipRewardsId, int membershipId, string createdBy, DateTime? creationDate,
                                 int subscriptionBillingScheduleId, 
                                 CardCoreDTO.CardValidityStatus validityStatus = CardCoreDTO.CardValidityStatus.Valid):this()
        {
            log.LogMethodEntry(cardDiscountId, cardId, discountId, expiryDate, transactionId, lineId, taskId, lastUpdatedUser, lastUpdatedDate, internetKey,  cardTypeId, isActive,
                              siteId,  masterEntityId, synchStatus, guid, expireWithMembership, membershipRewardsId, membershipId, createdBy,  creationDate,
                                 subscriptionBillingScheduleId, validityStatus);
            this.cardDiscountId = cardDiscountId;
            this.cardId = cardId;
            this.discountId = discountId;
            this.expiryDate = expiryDate;
            this.lastUpdatedUser = lastUpdatedUser;
            this.lastUpdatedDate = lastUpdatedDate;
            this.internetKey = internetKey;
            this.cardTypeId = cardTypeId;
            this.transactionId = transactionId;
            this.lineId = lineId;
            this.taskId = taskId;
            this.isActive = isActive;

            this.siteId = siteId;
            this.masterEntityId = masterEntityId;
            this.synchStatus = synchStatus;
            this.guid = guid;

            this.expireWithMembership = expireWithMembership;
            this.membershipRewardsId = membershipRewardsId;
            this.membershipId = membershipId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.validityStatus = validityStatus;
            this.subscriptionBillingScheduleId = subscriptionBillingScheduleId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the CardDiscountId field
        /// </summary>
        [DisplayName("Id")]
        [ReadOnly(true)]
        public int CardDiscountId
        {
            get
            {
                return cardDiscountId;
            }

            set
            {
                this.IsChanged = true;
                cardDiscountId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the CardId field
        /// </summary>
        [Browsable(false)]
        public int CardId
        {
            get
            {
                return cardId;
            }

            set
            {
                this.IsChanged = true;
                cardId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the DiscountId field
        /// </summary>
        [DisplayName("Discount Name")]
        public int DiscountId
        {
            get
            {
                return discountId;
            }

            set
            {
                this.IsChanged = true;
                discountId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the ExpiryDate field
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
        /// Get/Set method of the CreatedBy field
        /// </summary>
        [DisplayName("CreatedBy")]
        public string CreatedBy { get { return createdBy; } set { createdBy = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        [DisplayName("Creation Date")]
        public DateTime? CreationDate { get { return creationDate; } set { creationDate = value; this.IsChanged = true; } }


        /// <summary>
        /// Get method of the LastUpdatedUser field
        /// </summary>
        [DisplayName("Last Updated User")]
        public string LastUpdatedUser
        {
            get
            {
                return lastUpdatedUser;
            }
        }


        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        [DisplayName("Last Updated Date")]
        public DateTime LastUpdatedDate
        {
            get
            {
                return lastUpdatedDate;
            }
        }

        /// <summary>
        /// Get/Set method of the InternetKey field
        /// </summary>
        [Browsable(false)]
        public int? InternetKey
        {
            get
            {
                return internetKey;
            }

            set
            {
                this.IsChanged = true;
                internetKey = value;
            }
        }

        /// <summary>
        /// Get/Set method of the CardTypeId field
        /// </summary>
        [Browsable(false)]
        public int CardTypeId
        {
            get
            {
                return cardTypeId;
            }

            set
            {
                this.IsChanged = true;
                cardTypeId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the TransactionId field
        /// </summary>
        [Browsable(false)]
        public int TransactionId
        {
            get
            {
                return transactionId;
            }

            set
            {
                this.IsChanged = true;
                transactionId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the LineId field
        /// </summary>
        [Browsable(false)]
        public int LineId
        {
            get
            {
                return lineId;
            }

            set
            {
                this.IsChanged = true;
                lineId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the TaskId field
        /// </summary>
        [Browsable(false)]
        public int TaskId
        {
            get
            {
                return taskId;
            }

            set
            {
                this.IsChanged = true;
                taskId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("Active")]
        public string IsActive
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
        /// Get/Set method of the ValidityStatus field
        /// </summary>
        [DisplayName("Validity Status")]
        public CardCoreDTO.CardValidityStatus ValidityStatus
        {
            get
            {
                return validityStatus;
            }

            set
            {
                this.IsChanged = true;
                validityStatus = value;
            }
        }

        /// <summary>
        /// Get/Set method of the subscriptionBillingScheduleId field
        /// </summary>
        [DisplayName("Subscription Billing Schedule Id")]
        public int SubscriptionBillingScheduleId
        {
            get
            {
                return subscriptionBillingScheduleId;
            }

            set
            {
                this.IsChanged = true;
                subscriptionBillingScheduleId = value;
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
                this.IsChanged = true;
                masterEntityId = value;
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
        }
        /// <summary>
        /// Get/Set method of the ExpireWithMembership field
        /// </summary>
        [DisplayName("Expire With Membership")]
        public string ExpireWithMembership { get { return expireWithMembership; } set { expireWithMembership = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the MembershipRewardsId field
        /// </summary>
        [DisplayName("MembershipRewards Id")]
        public int MembershipRewardsId { get { return membershipRewardsId; } set { membershipRewardsId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the MembershipId field
        /// </summary>
        [DisplayName("Membership Id")]
        public int MembershipId { get { return membershipId; } set { membershipId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        [Browsable(false)]
        public bool IsChanged
        {
            get
            {
                lock(notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged|| cardDiscountId < 0;
                }
            }

            set
            {
                lock(notifyingObjectIsChangedSyncRoot)
                {
                    if(!Boolean.Equals(notifyingObjectIsChanged, value))
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
            log.Debug("Starts-AcceptChanges() Method.");
            this.IsChanged = false;
            log.Debug("Ends-AcceptChanges() Method.");
        }

        /// <summary>
        /// Returns a string that represents the current CardDiscountsDTO
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            log.Debug("Starts-ToString() method.");
            StringBuilder returnValue = new StringBuilder("\n-----------------------CardDiscountsDTO-----------------------------\n");
            returnValue.Append(" CardDiscountId : " + CardDiscountId);
            returnValue.Append(" CardId : " + CardId);
            returnValue.Append(" DiscountId : " + DiscountId);
            returnValue.Append(" ExpiryDate : " + ExpiryDate);
            returnValue.Append(" LastUpdatedUser : " + LastUpdatedUser);
            returnValue.Append(" LastUpdatedDate : " + LastUpdatedDate);
            returnValue.Append(" InternetKey : " + InternetKey);
            returnValue.Append(" CardTypeId : " + CardTypeId);
            returnValue.Append(" IsActive : " + IsActive);
            returnValue.Append(" ValidityStatus : " + ValidityStatus);
            returnValue.Append("\n-------------------------------------------------------------\n");
            log.Debug("Ends-ToString() Method");
            return returnValue.ToString();

        }
    }
}
