/********************************************************************************************
 * Project Name - RedemptionCards Data Handler
 * Description  - Data object of the RedemptionCards
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 *********************************************************************************************
 *2.3.0       25-Jun-2018   Archana/Guru S A   Modified  to add totalCardTickets
 *2.70.2        19-Jul-2019   Deeksha            Modifications as per three tier standard.
 ********************************************************************************************/
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Semnox.Parafait.Redemption
{

    /// <summary>
    /// RedemptionCardsDTO
    /// </summary>
    [Table("Redemption_Cards")]
    public partial class RedemptionCardsDTO
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        /// 
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  Redemption Card ID field
            /// </summary>
            REDEMPTION_CARD_ID,
            /// <summary>
            /// Search by From RedemptionID field
            /// </summary>
            REDEMPTION_ID,
            /// <summary>
            /// Search by To Card Number field
            /// </summary>
            CARD_NUMBER,
            /// <summary>
            /// Search by CURRENCY ID field
            /// </summary>
            CURRENCY_ID,
            /// <summary>
            /// Search by site_id field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MasterEntityId field
            /// </summary>
            MASTER_ENTITY_ID 
        }

        private int redemptionCardId;
        private int redemptionId;
        private string cardNumber;
        private int cardId;
        private int? ticketCount;
        private int? currencyId;
        private int? currencyQuantity;
        private int? currencyValueInTickets;
        private string currencyName;
        private int siteId;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        private DateTime lastUpdateDate;
        private string lastUpdatedBy;
        private DateTime creationDate;
        private string createdBy;
        private int? totalCardTickets;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private int? sourceCurrencyRuleId;
        private int? currencyRuleId;
        private int? viewGroupingNumber;
        /// <summary>
        /// RedemptionCardsDTO Default Constructor
        /// </summary>
        public RedemptionCardsDTO()
        {
            log.LogMethodEntry();
            RedemptionCardsId = -1;
            redemptionId = -1;
            cardId = -1;
            currencyId = -1;
            currencyQuantity = -1;
            masterEntityId = -1;
            siteId = -1;
            log.LogMethodExit();

        }

        /// <summary>
        /// copy Constructor 
        /// </summary>
        public RedemptionCardsDTO(RedemptionCardsDTO tocopyredemptionCardsDTO)
            : this()
        {
            log.LogMethodEntry(tocopyredemptionCardsDTO);
            this.redemptionCardId = tocopyredemptionCardsDTO.redemptionCardId;
            this.redemptionId = tocopyredemptionCardsDTO.redemptionId;
            this.cardNumber = tocopyredemptionCardsDTO.cardNumber;
            this.cardId = tocopyredemptionCardsDTO.cardId;
            this.ticketCount = tocopyredemptionCardsDTO.ticketCount;
            this.currencyId = tocopyredemptionCardsDTO.currencyId;
            this.currencyQuantity = tocopyredemptionCardsDTO.currencyQuantity;
            this.currencyValueInTickets = tocopyredemptionCardsDTO.currencyValueInTickets;
            this.currencyName = tocopyredemptionCardsDTO.currencyName;
            this.totalCardTickets = tocopyredemptionCardsDTO.totalCardTickets;
            this.ViewGroupingNumber = tocopyredemptionCardsDTO.ViewGroupingNumber;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with Required data fields
        /// </summary>
        public RedemptionCardsDTO(int redemptionCardId, int redemptionId, string cardNumber, int cardId, int? ticketCount,
                                    int? currencyId, int? currencyQuantity, int? currencyValueInTickets, string currencyName, int? totalCardTickets, int? viewGroupingNumber)
            : this()
        {
            log.LogMethodEntry(redemptionCardId, redemptionId, cardNumber, cardId, ticketCount, currencyId, currencyQuantity, currencyValueInTickets, currencyName, totalCardTickets, viewGroupingNumber);
            this.redemptionCardId = redemptionCardId;
            this.redemptionId = redemptionId;
            this.cardNumber = cardNumber;
            this.cardId = cardId;
            this.ticketCount = ticketCount;
            this.currencyId = currencyId;
            this.currencyQuantity = currencyQuantity;
            this.currencyValueInTickets = currencyValueInTickets;
            this.currencyName = currencyName;
            this.totalCardTickets = totalCardTickets;
            this.viewGroupingNumber = viewGroupingNumber;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public RedemptionCardsDTO(int redemptionCardId, int redemptionId, string cardNumber, int cardId, int? ticketCount,
                                   int? currencyId, int? currencyQuantity, int siteId, string guid, bool synchStatus, int masterEntityId,
                                   DateTime lastUpdateDate, string lastUpdatedBy, int? currencyValueInTickets, string currencyName,
                                   DateTime creationDate, string createdBy, int? totalCardTickets, int? sourceCurrencyRuleId, int? currencyRuleId, int? viewGroupingNumber)
           : this(redemptionCardId, redemptionId, cardNumber, cardId, ticketCount, currencyId, currencyQuantity, currencyValueInTickets, currencyName, totalCardTickets, viewGroupingNumber)
        {
            log.LogMethodEntry(redemptionCardId, redemptionId, cardNumber, cardId, ticketCount, currencyId, currencyQuantity,
                                   siteId, guid, synchStatus, masterEntityId, lastUpdateDate, lastUpdatedBy, currencyValueInTickets, currencyName, creationDate, createdBy, totalCardTickets, sourceCurrencyRuleId, currencyRuleId,viewGroupingNumber);
            this.siteId = siteId;
            this.masterEntityId = masterEntityId;
            this.synchStatus = synchStatus;
            this.guid = guid;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            this.creationDate = creationDate;
            this.createdBy = createdBy;
            this.sourceCurrencyRuleId = sourceCurrencyRuleId;
            this.currencyRuleId = currencyRuleId;
            log.LogMethodExit();
        }


        /// <summary>
        /// Get/Set method of the Redemption Cards Id field
        /// </summary>
        [Key]
        [Column("redemption_cards_id")]
        public int RedemptionCardsId { get { return redemptionCardId; } set { redemptionCardId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the RedemptionId field
        /// </summary>
        [Column("redemption_id")]
        public int RedemptionId { get { return redemptionId; } set { redemptionId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Card Number field
        /// </summary>
        [Column("card_number")]
        public string CardNumber { get { return cardNumber; } set { cardNumber = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CardId field
        /// </summary>
        [Column("card_id")]
        public int CardId { get { return cardId; } set { cardId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Ticket Count field
        /// </summary>
        [Column("ticket_count")]
        public int? TicketCount { get { return ticketCount; } set { ticketCount = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Currency Id field
        /// </summary>
        public int? CurrencyId { get { return currencyId; } set { currencyId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Currency Quantity field
        /// </summary>
        public int? CurrencyQuantity { get { return currencyQuantity; } set { currencyQuantity = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [Column("site_id")]
        [Browsable(false)]
        public int SiteId { get { return siteId; } set { siteId = value; this.IsChanged = true; } }

        /// <summary>
        ///  Get/Set method of the Guid field
        /// </summary>
        [Browsable(false)]
        public string Guid { get { return guid; } set { guid = value; } }

        /// <summary>
        ///  Get/Set method of the SynchStatus field
        /// </summary>
        [Browsable(false)]
        public Nullable<bool> SynchStatus { get { return synchStatus; } }

        /// <summary>
        ///  Get/Set method of the MasterEntityId field
        /// </summary>
        [Browsable(false)]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Last Updated By field
        /// </summary>
        [Column("LastUpdateBy")]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }

        /// <summary>
        /// Get/Set method of the Last Update Date field
        /// </summary>
        [Column("LastUpdateDate")]
        public DateTime LastUpdateDate { get { return lastUpdateDate; } set { lastUpdateDate = value; } }

        //currencyValueInTickets; currencyName;
        /// <summary>
        /// Get/Set method of the currencyValueInTickets
        /// </summary>
        [Column("CurrencyValueInTickets")]
        public int? CurrencyValueInTickets { get { return currencyValueInTickets; } }

        /// <summary>
        /// Get/Set method of the CurrencyName
        /// </summary>
        [Column("CurrencyName")]
        public string CurrencyName { get { return currencyName; } }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        [Column("CreatedBy")]
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }

        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        [Column("CreationDate")]
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }

        /// <summary>
        /// Get/Set method of the TotalCardTickets field
        /// </summary>
        [Column("TotalCardTickets")]
        public int? TotalCardTickets { get { return totalCardTickets; } set { totalCardTickets = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Currency Rule Id field
        /// </summary>
        public int? CurrencyRuleId { get { return currencyRuleId; } set { currencyRuleId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Source Currency Rule Id field
        /// </summary>
        public int? SourceCurrencyRuleId { get { return sourceCurrencyRuleId; } set { sourceCurrencyRuleId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ViewGroupingNumber field
        /// </summary>
        public int? ViewGroupingNumber { get { return viewGroupingNumber; } set { viewGroupingNumber = value; this.IsChanged = true; } }

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
                    return notifyingObjectIsChanged || redemptionCardId < 0;
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
