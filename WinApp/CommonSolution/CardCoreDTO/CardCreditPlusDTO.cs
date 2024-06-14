/********************************************************************************************
 * Project Name - CardCore project 
 * Description  - Data object of the CardcreditPlusDTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 *********************************************************************************************
 *1.00        17-May-2017     Rakshith          Created 
 *2.80.0      20-Mar-2020   Mathew NInan       Added new field ValidityStatus to track
 *                                                  status of entitlements
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Semnox.Parafait.CardCore
{
    /// <summary>
    /// CardCoreDTO Class
    /// </summary>
    public class CardCreditPlusDTO
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        bool notifyingObjectIsChanged;
        readonly object notifyingObjectIsChangedSyncRoot = new Object();


        public enum SearchByParameters
        {
            /// <summary>
            /// Search by CARD_ID field
            /// </summary>
            CARD_ID = 0,
            /// <summary>
            /// Search by CREDITPLUSTYPE field
            /// </summary>
            CREDITPLUSTYPE = 1,
            /// <summary>
            /// Search by ForMembershipOnly field
            /// </summary>
            FORMEMBERSHIPONLY = 2,
            /// <summary>
            /// Search by expireWithMembership field
            /// </summary>
            EXPIREWITHMEMBERSHIP = 3,
            /// <summary>
            /// Search by membershipRewardsId field
            /// </summary>
            MEMBERSHIP_REWARDS_ID = 4,
            /// <summary>
            /// Search by membershipId field
            /// </summary>
            MEMBERSHIPS_ID =5,
            /// <summary>
            /// Search by site_id field
            /// </summary>
            SITE_ID = 6,
            /// <summary>
            /// Search by TRANSACTION_ID field
            /// </summary>
            TRANSACTION_ID=7,
            /// <summary>
            /// Search by cardId list field
            /// </summary>
            CARD_ID_LIST = 8,
            /// <summary>
            /// Search by Pause Allowed field
            /// </summary>
            PAUSE_ALLOWED,
            /// <summary>
            /// Search by ValidityStatus field
            /// </summary>
            VALIDITYSTATUS
        } 

        int cardCreditPlusId;
        double creditPlus;
        string creditPlusType;
        string refundable;
        string remarks;
        int card_id;
        int trxId;
        int lineId;
        double creditPlusBalance;
        DateTime periodFrom;
        DateTime periodTo;
        double timeFrom;
        double timeTo;
        int numberOfDays;
        string monday;
        string tuesday;
        string wednesday;
        string thursday;
        string friday;
        string saturday;
        string sunday;
        double minimumSaleAmount;
        int loyaltyRuleId;
        DateTime creationDate;
        DateTime lastupdatedDate;
        string lastUpdatedBy;
        string guid;
        int siteId;
        bool synchStatus;
        string extendOnReload;
        DateTime playStartTime;
        bool ticketAllowed;
        int masterEntityId;
        string forMembershipOnly;
        string expireWithMembership;
        int membershipRewardsId;
        int membershipId;
        bool pauseAllowed;
        CardCoreDTO.CardValidityStatus validityStatus;




        /// <summary>
        /// Default Constructor
        /// </summary>
        public CardCreditPlusDTO()
        {
            this.cardCreditPlusId = -1;
            this.creditPlus = -1;
            this.creditPlusType = "";
            this.refundable = "N";
            this.remarks = "";
            this.card_id = -1;
            this.trxId = -1;
            this.lineId = -1;
            this.creditPlusBalance = 0.0;
            this.periodFrom = DateTime.MinValue;
            this.periodTo = DateTime.MinValue;
            this.timeFrom = -1;
            this.timeTo = -1;
            this.numberOfDays = -1;
            this.monday = "Y";
            this.tuesday = "Y";
            this.wednesday = "Y";
            this.thursday = "Y";
            this.friday = "Y";
            this.saturday = "Y";
            this.sunday = "Y";
            this.minimumSaleAmount = -1;
            this.loyaltyRuleId = -1;
            this.siteId = -1;
            this.extendOnReload = "";
            this.masterEntityId = -1;
            this.membershipId = -1;
            this.membershipRewardsId = -1;
            this.pauseAllowed = true;
        }

        /// <summary>
        /// Parameterized Contructor
        /// </summary>
        public CardCreditPlusDTO(int cardCreditPlusId, double creditPlus, string creditPlusType, string refundable, string remarks,
                                 int card_id, int trxId, int lineId, double creditPlusBalance, DateTime periodFrom, DateTime periodTo,
                                 double timeFrom, double timeTo, int numberOfDays, string monday, string tuesday, string wednesday, string thursday,
                                 string friday, string saturday, string sunday, double minimumSaleAmount, int loyaltyRuleId, DateTime creationDate,
                                 DateTime lastupdatedDate, string lastUpdatedBy, string guid, int siteId, bool synchStatus, string extendOnReload,
                                 DateTime playStartTime, bool ticketAllowed, int masterEntityId, string forMembershipOnly, string expireWithMembership,
                                 int membershipRewardsId, int membershipId, bool pauseAllowed, CardCoreDTO.CardValidityStatus validityStatus)
        {
            this.cardCreditPlusId = cardCreditPlusId;
            this.creditPlus = creditPlus;
            this.creditPlusType = creditPlusType;
            this.refundable = refundable;
            this.remarks = remarks;
            this.card_id = card_id;
            this.trxId = trxId;
            this.lineId = lineId;
            this.creditPlusBalance = creditPlusBalance;
            this.periodFrom = periodFrom;
            this.periodTo = periodTo;
            this.timeFrom = timeFrom;
            this.timeTo = timeTo;
            this.numberOfDays = numberOfDays;
            this.monday = monday;
            this.tuesday = tuesday;
            this.wednesday = wednesday;
            this.thursday = thursday;
            this.friday = friday;
            this.saturday = saturday;
            this.sunday = sunday;
            this.minimumSaleAmount = minimumSaleAmount;
            this.loyaltyRuleId = loyaltyRuleId;
            this.creationDate = creationDate;
            this.lastupdatedDate = lastupdatedDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.extendOnReload = extendOnReload;
            this.playStartTime = playStartTime;
            this.ticketAllowed = ticketAllowed;
            this.masterEntityId = masterEntityId;
            this.forMembershipOnly = forMembershipOnly;
            this.expireWithMembership = expireWithMembership;
            this.membershipRewardsId = membershipRewardsId;
            this.membershipId = membershipId;
            this.pauseAllowed = pauseAllowed;
            this.validityStatus = validityStatus;
        }


        public int CardCreditPlusId { get { return cardCreditPlusId; } set { cardCreditPlusId = value; this.IsChanged = true; } }
        public double CreditPlus { get { return creditPlus; } set { creditPlus = value; this.IsChanged = true; } }
        public string CreditPlusType { get { return creditPlusType; } set { creditPlusType = value; this.IsChanged = true; } }
        public string Refundable { get { return refundable; } set { refundable = value; this.IsChanged = true; } }
        public string Remarks { get { return remarks; } set { remarks = value; this.IsChanged = true; } }
        public int CardId { get { return card_id; } set { card_id = value; this.IsChanged = true; } }
        public int TrxId { get { return trxId; } set { trxId = value; this.IsChanged = true; } }
        public int LineId { get { return lineId; } set { lineId = value; this.IsChanged = true; } }
        public double CreditPlusBalance { get { return creditPlusBalance; } set { creditPlusBalance = value; this.IsChanged = true; } }
        public DateTime PeriodFrom { get { return periodFrom; } set { periodFrom = value; this.IsChanged = true; } }
        public DateTime PeriodTo { get { return periodTo; } set { periodTo = value; this.IsChanged = true; } }
        public double TimeFrom { get { return timeFrom; } set { timeFrom = value; this.IsChanged = true; } }
        public double TimeTo { get { return timeTo; } set { timeTo = value; this.IsChanged = true; } }
        public int NumberOfDays { get { return numberOfDays; } set { numberOfDays = value; this.IsChanged = true; } }
        public string Monday { get { return monday; } set { monday = value; this.IsChanged = true; } }
        public string Tuesday { get { return tuesday; } set { tuesday = value; this.IsChanged = true; } }
        public string Wednesday { get { return wednesday; } set { wednesday = value; this.IsChanged = true; } }
        public string Thursday { get { return thursday; } set { thursday = value; this.IsChanged = true; } }
        public string Friday { get { return friday; } set { friday = value; this.IsChanged = true; } }
        public string Saturday { get { return saturday; } set { saturday = value; this.IsChanged = true; } }
        public string Sunday { get { return sunday; } set { sunday = value; this.IsChanged = true; } }
        public double MinimumSaleAmount { get { return minimumSaleAmount; } set { minimumSaleAmount = value; this.IsChanged = true; } }
        public int LoyaltyRuleId { get { return loyaltyRuleId; } set { loyaltyRuleId = value; this.IsChanged = true; } }
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value;  } }
        public DateTime LastupdatedDate { get { return lastupdatedDate; } set { lastupdatedDate = value;  } }
        public string LastUpdatedBy { get { return lastUpdatedBy; } }
        public string Guid { get { return guid; } set { guid = value; } }
        public int SiteId { get { return siteId; } set { siteId = value; this.IsChanged = true; } }
        public bool SynchStatus { get { return synchStatus; } }
        public string ExtendOnReload { get { return extendOnReload; } set { extendOnReload = value; this.IsChanged = true; } }
        public DateTime PlayStartTime { get { return playStartTime; } set { playStartTime = value; this.IsChanged = true; } }
        public bool TicketAllowed { get { return ticketAllowed; } set { ticketAllowed = value; this.IsChanged = true; } }
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; } }

        public string ForMembershipOnly { get { return forMembershipOnly; } set { forMembershipOnly = value; this.IsChanged = true; } }

        public string ExpireWithMembership { get { return expireWithMembership; } set { expireWithMembership = value; this.IsChanged = true; } }

        public int MembershipRewardsId { get { return membershipRewardsId; } set { membershipRewardsId = value; this.IsChanged = true; } }

        public int MembershipId { get { return membershipId; } set { membershipId = value; this.IsChanged = true; } }

        public CardCoreDTO.CardValidityStatus ValidityStatus { get { return validityStatus; } set { validityStatus = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the PauseAllowed field
        /// </summary>
        [DisplayName("PauseAllowed")]
        public bool PauseAllowed
        {
            get
            {
                return pauseAllowed;
            }

            set
            {
                this.IsChanged = true;
                pauseAllowed = value;
            }
        }

        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        [Browsable(false)]
        [DefaultValue(false)]
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || cardCreditPlusId < 0;
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
            log.LogMethodExit();
        }



    }
}
