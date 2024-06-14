/********************************************************************************************
 * Project Name - Semnox.Parafait.Tags -CardGamesDTO
 * Description  - CardGamesDTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.80.0      19-Mar-2020   Mathew NInan            Added new field ValidityStatus to track
 *                                                  status of entitlements
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Tags
{
    /// <summary>
    /// This is the CardGames data object class. This acts as data holder for the CardGames business object
    /// </summary>
    public class CardGamesDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        /// 
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  CARD_GAME_ID field
            /// </summary>
            CARD_GAME_ID,
            /// <summary>
            /// CARD Id
            /// </summary>
            CARD_ID,
            /// <summary>
            /// GAME Id
            /// </summary>
            GAME_ID,
            /// <summary>
            /// GAME PROFILE Id
            /// </summary>
            GAME_PROFILE_ID,
            /// <summary>
            /// Search by site_id field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by EXPIREWITHMEMBERSHIP field
            /// </summary>
            EXPIRE_WITH_MEMBERSHIP,
            /// <summary>
            /// Search by MEMBERSHIP_ID field
            /// </summary>
            MEMBERSHIP_ID,
            /// <summary>
            /// Search by MEMBERSHIP_REWARDS_ID field
            /// </summary>
            MEMBERSHIP_REWARDS_ID,
            /// <summary>
            /// Search by TRANSACTION_ID field
            /// </summary>
            TRANSACTION_ID,
            /// <summary>
            ///  Search by ValidityStatus Id
            /// </summary>
            VALIDITYSTATUS
        }

        /// <summary>
        /// TagValidityStatus enum defines value for entitlement status
        /// </summary>
        public enum TagValidityStatus
        {
            /// <summary>
            /// Valid status of entitlement, entitlement is available for use. Default value will be NULL or Y
            /// </summary>
            Valid,
            /// <summary>
            /// Hold status of entitlement, entitlement is not available for use
            /// </summary>
            Hold
        }

        int card_game_id;
        int card_id;
        int game_id;
        double quantity;
        DateTime? expiryDate;
        int game_profile_id;
        string frequency;
        DateTime? lastPlayedTime;
        int balanceGames;
        string guid;
        int site_id;
        DateTime? last_update_date;
        int cardTypeId;
        int trxId;
        int trxLineId;
        string entitlementType;
        string optionalAttribute;
        bool synchStatus;
        int customDataSetId;
        bool ticketAllowed;
        DateTime? fromDate;
        int masterEntityId;
        string monday;
        string tuesday;
        string wednesday;
        string thursday;
        string friday;
        string saturday;
        string sunday;
        string expireWithMembership;
        int membershipId;
        int membershipRewardsId;
        TagValidityStatus validityStatus;
        string createdBy;
        DateTime? creationDate;
        string lastUpdatedBy; 
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public CardGamesDTO()
        {
            log.LogMethodEntry();
            card_game_id = -1;
            card_id = -1;
            game_id = -1;
            expiryDate = null;
            game_profile_id = -1;
            balanceGames = -1;
            site_id = -1;
            cardTypeId = -1;
            trxId = -1;
            trxLineId = -1; 
            customDataSetId = -1;
            masterEntityId = -1; 
            membershipId = 1;
            membershipRewardsId = -1;
            expireWithMembership = "N";
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public CardGamesDTO(int card_game_id, int card_id, int game_id, double quantity, DateTime? expiryDate, int game_profile_id, string frequency, DateTime? lastPlayedTime, int balanceGames, string guid, int site_id, DateTime? last_update_date, int cardTypeId, int trxId, int trxLineId, string entitlementType, string optionalAttribute, bool synchStatus, int customDataSetId, bool ticketAllowed, DateTime? fromDate, int masterEntityId, string monday, string tuesday, string wednesday, string thursday, string friday, string saturday, string sunday, 
                           string expireWithMembership, int membershipId, int membershipRewardsId, string CreatedBy, DateTime? CreationDate, string LastUpdatedBy, TagValidityStatus validityStatus)
        {
            log.LogMethodEntry(card_game_id, card_id, game_id, quantity, expiryDate, game_profile_id, frequency, lastPlayedTime, balanceGames, guid, site_id, last_update_date, cardTypeId, trxId, trxLineId, entitlementType, optionalAttribute, synchStatus, customDataSetId, ticketAllowed, fromDate, masterEntityId, monday, tuesday, wednesday, thursday, friday, saturday, sunday, expireWithMembership, membershipId, membershipRewardsId, CreatedBy, CreationDate, LastUpdatedBy);
            this.card_game_id = card_game_id;
            this.card_id = card_id;
            this.game_id = game_id;
            this.quantity = quantity;
            this.expiryDate = expiryDate;
            this.game_profile_id = game_profile_id;
            this.frequency = frequency;
            this.lastPlayedTime = lastPlayedTime;
            this.balanceGames = balanceGames;
            this.guid = guid;
            this.site_id = site_id;
            this.last_update_date = last_update_date;
            this.cardTypeId = cardTypeId;
            this.trxId = trxId;
            this.trxLineId = trxLineId;
            this.entitlementType = entitlementType;
            this.optionalAttribute = optionalAttribute;
            this.synchStatus = synchStatus;
            this.customDataSetId = customDataSetId;
            this.ticketAllowed = ticketAllowed;
            this.fromDate = fromDate;
            this.masterEntityId = masterEntityId;
            this.monday = monday;
            this.tuesday = tuesday;
            this.wednesday = wednesday;
            this.thursday = thursday;
            this.friday = friday;
            this.saturday = saturday;
            this.sunday = saturday;
            this.expireWithMembership = expireWithMembership;
            this.membershipId = membershipId;
            this.membershipRewardsId = membershipRewardsId;
            this.createdBy = CreatedBy;
            this.creationDate = CreationDate;
            this.lastUpdatedBy = LastUpdatedBy;
            this.validityStatus = validityStatus;
            log.LogMethodExit(null);
        }

        /// <summary>
        /// Get/Set method of the card_game_id field
        /// </summary>
        [DisplayName("Card Game Id")]
        public int CardGameId
        {
            get { return card_game_id; }
            set { card_game_id = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the card_id field
        /// </summary>
        [DisplayName("Card Id")]
        public int CardId
        {
            get { return card_id; }
            set { card_id = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the game_id field
        /// </summary>
        [DisplayName("Game Id")]
        public int GameId
        {
            get { return game_id; }
            set { game_id = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get method of the customerName field
        /// </summary>
        [DisplayName("Quantity")]
        public Double Quantity
        {
            get { return quantity; }
            set { quantity = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get method of the expiryDate
        /// </summary>
        [DisplayName("Expiry Date")]
        public DateTime? ExpiryDate
        {
            get { return expiryDate; }
            set { expiryDate = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the game_profile_id field
        /// </summary>
        [DisplayName("Game Profile Id")]
        public int GameProfileId
        {
            get { return game_profile_id; }
            set { game_profile_id = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the frequency field
        /// </summary>
        [DisplayName("Frequency")]
        public string Frequency
        {
            get { return frequency; }
            set { frequency = value; this.IsChanged = true; }
        }


        /// <summary>
        /// Get/Set method of the lastPlayedTime field
        /// </summary>
        [DisplayName("Last Played Time")]
        public DateTime? LastPlayedTime
        {
            get { return lastPlayedTime; }
            set { lastPlayedTime = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the balanceGames field
        /// </summary>
        [DisplayName("balanceGames")]
        public int BalanceGames
        {
            get { return balanceGames; }
            set { balanceGames = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the guid field
        /// </summary>
        [DisplayName("Guid")]
        public string Guid
        {
            get { return guid; }
            set { guid = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the site_id field
        /// </summary>
        [DisplayName("Site Id")]
        public int Site_Id
        {
            get { return site_id; }
            set { site_id = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the last_update_date field
        /// </summary>
        [DisplayName("Last Update Date")]
        public DateTime? LastUpdateDate
        {
            get { return last_update_date; }
            set { last_update_date = value; this.IsChanged = true; }
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
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("Last Updated By")]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the cardTypeId field
        /// </summary>
        [DisplayName("Card Type Id")]
        public int CardTypeId
        {
            get { return cardTypeId; }
            set { cardTypeId = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the trxId field
        /// </summary>
        [DisplayName("Trx Id")]
        public int TrxId
        {
            get { return trxId; }
            set { trxId = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the trxLineId field
        /// </summary>
        [DisplayName("Trx Line Id")]
        public int TrxLineId
        {
            get { return trxLineId; }
            set { trxLineId = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the entitlementType field
        /// </summary>
        [DisplayName("Entitlement Type")]
        public string EntitlementType
        {
            get { return entitlementType; }
            set { entitlementType = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the optionalAttribute field
        /// </summary>
        [DisplayName("Optional Attribute")]
        public string OptionalAttribute
        {
            get { return optionalAttribute; }
            set { optionalAttribute = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the synchStatus field
        /// </summary>
        [DisplayName("Synch Status")]
        public bool SynchStatus
        {
            get { return synchStatus; }
            set { synchStatus = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the customDataSetId field
        /// </summary>
        [DisplayName("Custom DataSet Id")]
        public int CustomDataSetId
        {
            get { return customDataSetId; }
            set { customDataSetId = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the ticketAllowed field
        /// </summary>
        [DisplayName("Ticket Allowed")]
        public bool TicketAllowed
        {
            get { return ticketAllowed; }
            set { ticketAllowed = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the fromDate field
        /// </summary>
        [DisplayName("From Date")]
        public DateTime? FromDate
        {
            get { return fromDate; }
            set { fromDate = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the masterEntityId field
        /// </summary>
        [DisplayName("Master Entity Id")]
        public int MasterEntityId
        {
            get { return masterEntityId; }
            set { masterEntityId = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the monday field
        /// </summary>
        [DisplayName("Monday")]
        public string Monday
        {
            get { return monday; }
            set { monday = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the Tuesday field
        /// </summary>
        [DisplayName("Tuesday")]
        public string Tuesday
        {
            get { return tuesday; }
            set { tuesday = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the wednesday field
        /// </summary>
        [DisplayName("Wednesday")]
        public string Wednesday
        {
            get { return wednesday; }
            set { wednesday = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the thursday field
        /// </summary>
        [DisplayName("Thursday")]
        public string Thursday
        {
            get { return thursday; }
            set { thursday = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the friday field
        /// </summary>
        [DisplayName("Friday")]
        public string Friday
        {
            get { return friday; }
            set { friday = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the saturday field
        /// </summary>
        [DisplayName("Saturday")]
        public string Saturday
        {
            get { return saturday; }
            set { saturday = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the sunday field
        /// </summary>
        [DisplayName("Sunday")]
        public string Sunday
        {
            get { return sunday; }
            set { sunday = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the expireWithMembership field
        /// </summary>
        [DisplayName("Expire With Membership")]
        public string ExpireWithMembership
        {
            get { return expireWithMembership; }
            set { expireWithMembership = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the validityStatus field
        /// </summary>
        public TagValidityStatus ValidityStatus
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
        /// Get/Set method of the membershipId field
        /// </summary>
        [DisplayName("Membership Id")]
        public int MembershipId
        {
            get { return membershipId; }
            set { membershipId = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the membershipRewardsId field
        /// </summary>
        [DisplayName("MembershipRewardsId")]
        public int MembershipRewardsId
        {
            get { return membershipRewardsId; }
            set { membershipRewardsId = value; this.IsChanged = true; }
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
                    return notifyingObjectIsChanged || card_game_id < 0;
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
            log.LogMethodExit(null);
        }

    }
}
