/********************************************************************************************
 * Project Name - Redemption Data Handler
 * Description  - Data object of  RedemptionTicketAllocation 
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By          Remarks          
 *********************************************************************************************
 *2.3.0       25-Jun-2018     Archana/Guru S A     Created
 *2.4.0       19-Jul-2019     Deeksha              Modified : Added a new Constructor with required fields.
 *2.70.2        23-Aug-2019   Dakshakh             Modified : Added new fields RedemptionCurrencyRuleId, RedemptionCurrencyRuleTicket 
 *                                                          and  SourceCurrencyRule Id. Updated constructors and set, get properties accordingly
 *2.70.3      30-Jan-2020     Archana              Modified to add MANUAL_TICKETS_PER_DAY_BY_LOGIN_ID search parameter                                           
 ********************************************************************************************/

using System;
using System.ComponentModel;
using System.Text;

namespace Semnox.Parafait.Redemption
{
    /// <summary>
    /// TicketSource enum
    /// </summary>
    public enum TicketSource
    {
        /// <summary>
        /// Receipt
        /// </summary>
        Receipt,
        /// <summary>
        /// Cards
        /// </summary>
        Cards,
        /// <summary>
        /// Manual
        /// </summary>
        Manual,
        /// <summary>
        /// Grace
        /// </summary>
        Grace,
        /// <summary>
        /// Currency
        /// </summary>
        Currency
    };

    /// <summary>
    /// RedemptionTicketAllocationDTO
    /// </summary>
    public class RedemptionTicketAllocationDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        /// 
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by RedemptionTicketAllocationID field
            /// </summary>
            REDEMPTION_TICKET_ALLOCATION_ID,
            /// <summary>
            /// Search by RedemptionId field
            /// </summary>
            REDEMPTION_ID,
            /// <summary>
            /// Search by REDEMPTION_GIFT_ID field
            /// </summary>
            REDEMPTION_GIFT_ID,
            /// <summary>
            /// Search by REDEMPTION_GIFT_ID field
            /// </summary>
            CURRENCY_ID,
            /// <summary>
            /// Search by REDEMPTION_GIFT_ID field
            /// </summary>
            MANUAL_TICKET_RECEIPT_ID,
            /// <summary>
            /// Search by REDEMPTION_GIFT_ID field
            /// </summary>
            TRX_ID,
            /// <summary>
            /// Search by MAnualTicketReceiptNo field
            /// </summary>
            MANUAL_TICKET_RECEIPT_NO,
            /// <summary>
            /// Search by site_id field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MasterEntityId field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by RedemptionCurrencyRuleId field
            /// </summary>
            REDEMPTION_CURRENCY_RULE_ID,
            /// <summary>
            /// Search by SourceCurrencyRuleId field
            /// </summary>
            SOURCE_CURRENCY_RULE_ID,
            /// <summary>
            /// Search by ManualTicketsPerDayByLoginId field
            /// </summary>
            MANUAL_TICKETS_PER_DAY_BY_LOGIN_ID
        }

        private int id;
        private int redemptionId;
        private int redemptionGiftId;
        private int? manualTickets;
        private int? graceTickets;
        private int cardId;
        private int? eTickets;
        private int currencyId;
        private decimal? currencyQuantity;
        private int? currencyTickets;
        private string manualTicketReceiptNo;
        private int? receiptTickets;
        private int? turnInTickets;

        private int siteId;
        private int masterEntityId;
        private bool synchStatus;
        private string guid;

        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private DateTime creationDate;
        private string createdBy;
        private int manualTicketReceiptId;
        private int trxId;
        private int trxLineId;
        private string ticketSource;


        private int balanceTickets;
        private int redemptionCurrencyRuleId;
        private int? redemptionCurrencyRuleTicket;
        private int sourceCurrencyRuleId;

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public RedemptionTicketAllocationDTO()
        {
            log.LogMethodExit();
            id = -1;
            masterEntityId = -1;
            redemptionId = -1;
            redemptionGiftId = -1;
            cardId = -1;
            currencyId = -1;
            siteId = -1;
            manualTicketReceiptId = -1;
            trxId = -1;
            trxLineId = -1;
            redemptionCurrencyRuleId = -1;
            sourceCurrencyRuleId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public RedemptionTicketAllocationDTO(int id, int redemptionId, int redemptionGiftId, int? manualTickets, int? graceTickets, int cardId,
                                             int? eTickets, int currencyId, decimal? currencyQuantity, int? currencyTickets, string manualTicketReceiptNo,
                                             int? receiptTickets, int? turnInTickets, int manualTicketReceiptId, int trxId, int trxLineId,
                                             int redemptionCurrencyRuleId, int? redemptionCurrencyRuleTicket, int sourceCurrencyRuleId)
            : this()
        {
            log.LogMethodEntry(id, redemptionId, redemptionGiftId, manualTickets, graceTickets, cardId,
                                eTickets, currencyId, currencyQuantity, currencyTickets, manualTicketReceiptNo,
                                receiptTickets, turnInTickets, manualTicketReceiptId, trxId, trxLineId, redemptionCurrencyRuleId,
                                redemptionCurrencyRuleTicket, sourceCurrencyRuleId);
            this.id = id;
            this.redemptionId = redemptionId;
            this.redemptionGiftId = redemptionGiftId;
            this.manualTickets = manualTickets;
            this.graceTickets = graceTickets;
            this.cardId = cardId;
            this.graceTickets = graceTickets;
            this.eTickets = eTickets;
            this.currencyId = currencyId;
            this.currencyQuantity = currencyQuantity;
            this.currencyTickets = currencyTickets;
            this.manualTicketReceiptNo = manualTicketReceiptNo;
            this.turnInTickets = turnInTickets;
            this.manualTicketReceiptId = manualTicketReceiptId;
            this.receiptTickets = receiptTickets;
            this.trxId = trxId;
            this.trxLineId = trxLineId;
            this.redemptionCurrencyRuleId = redemptionCurrencyRuleId;
            this.redemptionCurrencyRuleTicket = redemptionCurrencyRuleTicket;
            this.sourceCurrencyRuleId = sourceCurrencyRuleId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public RedemptionTicketAllocationDTO(int id, int redemptionId, int redemptionGiftId, int? manualTickets, int? graceTickets, int cardId,
                         int? eTickets, int currencyId, decimal? currencyQuantity, int? currencyTickets, string manualTicketReceiptNo,
                         int? receiptTickets, int? turnInTickets, int siteId, int masterEntityId, bool synchStatus, string guid, string lastUpdatedBy, DateTime lastUpdatedDate,
                         DateTime creationDate, string createdBy, int manualTicketReceiptId, int trxId, int trxLineId, int redemptionCurrencyRuleId,
                         int? redemptionCurrencyRuleTicket, int sourceCurrencyRuleId)
            : this(id, redemptionId, redemptionGiftId, manualTickets, graceTickets, cardId,
                  eTickets, currencyId, currencyQuantity, currencyTickets, manualTicketReceiptNo,
                  receiptTickets, turnInTickets, manualTicketReceiptId, trxId, trxLineId, redemptionCurrencyRuleId,
                  redemptionCurrencyRuleTicket, sourceCurrencyRuleId)
        {
            log.LogMethodEntry(siteId, masterEntityId, synchStatus, guid, lastUpdatedBy, lastUpdatedDate, creationDate, createdBy);
            this.siteId = siteId;
            this.masterEntityId = masterEntityId;
            this.synchStatus = synchStatus;
            this.guid = guid;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.creationDate = creationDate;
            this.createdBy = createdBy;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>
        [DisplayName("Id")]
        [ReadOnly(true)]
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
        /// Get/Set method of the RedemptionId field
        /// </summary>
        [Browsable(false)]
        public int RedemptionId
        {
            get
            {
                return redemptionId;
            }

            set
            {
                this.IsChanged = true;
                redemptionId = value;
            }
        }
        /// <summary>
        /// Get/Set method of the RedemptionGiftId Text field
        /// </summary>
        [DisplayName("Redemption Gift Id")]
        public int RedemptionGiftId
        {
            get
            {
                return redemptionGiftId;
            }

            set
            {
                this.IsChanged = true;
                redemptionGiftId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the ManualTickets field
        /// </summary>
        [DisplayName("ManualTickets")]
        public int? ManualTickets
        {
            get
            {
                return manualTickets;
            }

            set
            {
                this.IsChanged = true;
                manualTickets = value;
            }
        }

        /// <summary>
        /// Get/Set method of the GraceTickets field
        /// </summary>
        [DisplayName("GraceTickets")]
        public int? GraceTickets
        {
            get
            {
                return graceTickets;
            }

            set
            {
                this.IsChanged = true;
                graceTickets = value;
            }
        }

        /// <summary>
        /// Get/Set method of the CardId field
        /// </summary>
        [DisplayName("Card Id")]
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
        /// Get/Set method of the ETickets field
        /// </summary>
        [DisplayName("ETickets")]
        public int? ETickets
        {
            get
            {
                return eTickets;
            }

            set
            {
                this.IsChanged = true;
                eTickets = value;
            }
        }

        /// <summary>
        /// Get/Set method of the CurrencyId field
        /// </summary>
        [DisplayName("CurrencyId")]
        public int CurrencyId
        {
            get
            {
                return currencyId;
            }

            set
            {
                this.IsChanged = true;
                currencyId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the CurrencyQuantity field
        /// </summary>
        [DisplayName("Currency Quantity")]
        public decimal? CurrencyQuantity
        {
            get
            {
                return currencyQuantity;
            }

            set
            {
                this.IsChanged = true;
                currencyQuantity = value;
            }
        }

        /// <summary>
        /// Get/Set method of the CurrencyTickets field
        /// </summary>
        [DisplayName("Currency Tickets")]
        public int? CurrencyTickets
        {
            get
            {
                return currencyTickets;
            }

            set
            {
                this.IsChanged = true;
                currencyTickets = value;
            }
        }

        /// <summary>
        /// Get/Set method of the ManualTicketReceiptNo field
        /// </summary>
        [DisplayName("ManualTicket Receipt No")]
        public string ManualTicketReceiptNo
        {
            get
            {
                return manualTicketReceiptNo;
            }

            set
            {
                this.IsChanged = true;
                manualTicketReceiptNo = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Receipt Tickets field
        /// </summary>
        [DisplayName("ReceiptTickets")]
        public int? ReceiptTickets
        {
            get
            {
                return receiptTickets;
            }

            set
            {
                this.IsChanged = true;
                receiptTickets = value;
            }
        }

        /// <summary>
        /// Get/Set method of the TurnInTickets field
        /// </summary>
        [DisplayName("TurnInTickets")]
        public int? TurnInTickets
        {
            get
            {
                return turnInTickets;
            }

            set
            {
                this.IsChanged = true;
                turnInTickets = value;
            }
        }


        /// <summary>
        /// Get/Set method of the ManualTicketReceiptId field
        /// </summary>
        [DisplayName("ManualTicketReceiptId")]
        public int ManualTicketReceiptId
        {
            get
            {
                return manualTicketReceiptId;
            }

            set
            {
                this.IsChanged = true;
                manualTicketReceiptId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the SiteId field
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
                this.IsChanged = true;
                masterEntityId = value;
            }
        }
        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [Browsable(false)]
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
        [Browsable(false)]
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
        /// Get/Set method of the LastUpdatedBy  field
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
                lastUpdatedBy = value;
            }
        }

        /// <summary>
        /// Get/Set method of the LastUpdatedDate  field
        /// </summary>
        [DisplayName("Last Updated Date")]
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
        /// Get/Set method of the CreationDate  field
        /// </summary>
        [DisplayName("Creation Date")]
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
        /// Get/Set method of the CreatedBy  field
        /// </summary>
        [DisplayName("Created By")]
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
        /// Get/Set method of the TrxId  field
        /// </summary>
        [DisplayName("TrxId")]
        public int TrxId
        {
            get
            {
                return trxId;
            }

            set
            {
                this.IsChanged = true;
                trxId = value;
            }
        }


        /// <summary>
        /// Get/Set method of the TrxLineId field
        /// </summary>
        [DisplayName("TrxLineId")]
        public int TrxLineId
        {
            get
            {
                return trxLineId;
            }

            set
            {
                trxLineId = value;
                IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the TicketSource field
        /// </summary>
        [DisplayName("Ticket Source")]
        public string TicketSource
        {
            get
            {
                return ticketSource;
            }

            set
            {
                ticketSource = value;
                IsChanged = true;
            }
        }
        /// <summary>
        /// Get/Set method of the balanceTickets field
        /// </summary>
        [DisplayName("balanceTickets")]
        public int BalanceTickets
        {
            get
            {
                return balanceTickets;
            }

            set
            {
                balanceTickets = value;
                IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the balanceTickets field
        /// </summary>
        [DisplayName("RedemptionCurrencyRuleId")]
        public int RedemptionCurrencyRuleId
        {
            get
            {
                return redemptionCurrencyRuleId;
            }

            set
            {
                redemptionCurrencyRuleId = value;
                IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the balanceTickets field
        /// </summary>
        [DisplayName("RedemptionCurrencyRuleTicket")]
        public int? RedemptionCurrencyRuleTicket
        {
            get
            {
                return redemptionCurrencyRuleTicket;
            }

            set
            {
                redemptionCurrencyRuleTicket = value;
                IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the balanceTickets field
        /// </summary>
        [DisplayName("SourceCurrencyRuleId")]
        public int SourceCurrencyRuleId
        {
            get
            {
                return sourceCurrencyRuleId;
            }

            set
            {
                sourceCurrencyRuleId = value;
                IsChanged = true;
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
        /// <summary>
        /// Returns a string that represents the current RedemptionTicketAllocationDTO.
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            log.LogMethodEntry();
            StringBuilder returnValue = new StringBuilder("\n-----------------------RedemptionTicketAllocationDTO-----------------------------\n");
            returnValue.Append(" Id : " + Id);
            returnValue.Append(" RedemptionId : " + RedemptionId);
            returnValue.Append(" RedemptionGiftId : " + RedemptionGiftId);
            returnValue.Append(" ManualTickets : " + ManualTickets);
            returnValue.Append(" GraceTickets : " + GraceTickets);
            returnValue.Append(" CardId : " + CardId);
            returnValue.Append(" ETickets : " + ETickets);
            returnValue.Append(" CurrencyId : " + CurrencyId);
            returnValue.Append(" CurrencyQuantity : " + CurrencyQuantity);
            returnValue.Append(" CurrencyTickets : " + CurrencyTickets);
            returnValue.Append(" ManualTicketReceiptNo : " + ManualTicketReceiptNo);
            returnValue.Append(" ReceiptTickets : " + ReceiptTickets);
            returnValue.Append(" TurnInTickets : " + TurnInTickets);
            returnValue.Append(" LastUpdatedBy : " + LastUpdatedBy);
            returnValue.Append(" LastUpdateDate : " + LastUpdatedDate);
            returnValue.Append(" CreationDate : " + CreationDate);
            returnValue.Append(" CreatedBy : " + CreatedBy);
            returnValue.Append(" ManualTicketReceiptId : " + ManualTicketReceiptId);
            returnValue.Append(" TrxId : " + TrxId);
            returnValue.Append(" TrxLineId : " + TrxLineId);
            returnValue.Append("\n-------------------------------------------------------------\n");
            log.LogMethodExit();
            return returnValue.ToString();
        }
    }
}
