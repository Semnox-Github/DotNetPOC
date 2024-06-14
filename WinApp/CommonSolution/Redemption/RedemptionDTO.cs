/********************************************************************************************
 * Project Name - Redemption DTO
 * Description  - Data object of Redemption
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By              Remarks          
 *********************************************************************************************
 *1.00        11-May-2017   Lakshminarayana          Created 
 *2.3.0       03-Jul-2018   Archana/Guru S A         Redemption kioks related changes 
 *2.4.0       03-Sep-2018   Archana/Guru S A         Modified to add customerId, posMachineId 
 *                                                   for redemption kiosk phase 2 changes
 *2.7.0       08-Jul-2019   Archana                  Redemption Receipt changes to show ticket allocation details
 *2.70.2        19-Jul-2019   Deeksha                  Modifications as per three tier standard.
 *2.70.2        16-Sep-2019   Dakshakh raj             Redemption currency rule enhancement   
 ***********************************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Semnox.Parafait.Redemption
{
    /// <summary>
    /// This is the Redemption data object class. This acts as data holder for the Redemption business object
    /// </summary>
    public class RedemptionDTO
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        /// 
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  RedemptionID field
            /// </summary>
            REDEPTION_ID,
            /// <summary>
            /// Search by From Redemption date field
            /// </summary>
            FROM_REDEMPTION_DATE,
            /// <summary>
            /// Search by To Redemption date field
            /// </summary>
            TO_REDEMPTION_DATE,
            /// <summary>
            /// Search by site_id field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MasterEntityId field
            /// </summary>
            MASTER_ENTITY_ID,
            ///<summary>
            ///Search by RedemptionOrderNo
            ///</summary>
            REDEMPTION_ORDER_NO,
            ///<summary>
            ///Search by PRIMARY_CARD
            ///</summary>
            PRIMARY_CARD,
            ///<summary>
            ///Search by CustomerName
            ///</summary>
            CUSTOMER_NAME,
            ///<summary>
            ///Search by GiftCodeDescBarCode
            ///</summary>
            GIFT_CODE_DESC_BARCODE,
            ///<summary>
            ///Search by Redemption Status
            ///</summary>
            REDEMPTION_STATUS,
            ///<summary>
            ///Search by RedemptionOrderNo
            ///</summary>
            REDEMPTION_ORDER_NO_LIKE,
            ///<summary>
            ///Search by Fetch Gift Redemptions
            ///</summary>
            FETCH_GIFT_REDEMPTIONS_ONLY,
            ///<summary>
            ///Search parameter ask loading of complete details of order
            ///</summary>
            LOAD_GIFT_CARD_TICKET_ALLOCATION_DETAILS,
            ///<summary>
            ///Search by From Redemption order delivered date field
            ///</summary>
            FROM_REDEMPTION_ORDER_DELIVERED_DATE,
            ///<summary>
            ///Search by To Redemption order delivered date field
            ///</summary>
            TO_REDEMPTION_ORDER_DELIVERED_DATE,
            ///<summary>
            ///Search by From Redemption order delivered date field
            ///</summary>
            FROM_REDEMPTION_ORDER_COMPLETED_DATE,
            ///<summary>
            ///Search by To Redemption order delivered date field
            ///</summary>
            TO_REDEMPTION_ORDER_COMPLETED_DATE,
            ///<summary>
            ///Search by POSMAchineId
            ///</summary>
            POS_MACHINE_ID,
            ///<summary>
            ///Search by CustomerId
            ///</summary>
            CUSTOMER_ID,
            ///<summary>
            ///Search by CARD_NUMBER
            ///</summary>
            CARD_NUMBER,
            ///<summary>
            ///Search by REDEMPTION_STATUS_NOT_IN
            ///</summary>
            REDEMPTION_STATUS_NOT_IN
        }

        /// <summary>
        /// RedemptionSource enum
        /// </summary>
        /// 
        public enum RedemptionStatusEnum
        {
            ///<summary>
            ///OPEN
            ///</summary>
            [Description("Open")] OPEN,

            ///<summary>
            ///PREPARED
            ///</summary>
            [Description("Prepared")] PREPARED,

            ///<summary>
            ///DELIVERED
            ///</summary>
            [Description("Delivered")] DELIVERED,
            ///<summary>
            ///SUSPENDED
            ///</summary>
            [Description("Suspended")] SUSPENDED,
            ///<summary>
            ///ABANDONED
            ///</summary>
            [Description("ABANDONED")] ABANDONED,
            ///<summary>
            ///NEW
            ///</summary>
            [Description("New")] NEW

        }

        /// <summary>
        /// RedemptionTicketSource enum
        /// </summary>
        /// 
        public enum RedemptionTicketSource
        {
            ///<summary>
            ///CARD
            ///</summary>
            [Description("Card")] CARD,

            ///<summary>
            ///TICKET_RECEIPT
            ///</summary>
            [Description("Receipt")] TICKET_RECEIPT,

            ///<summary>
            ///REDEMPTION_CURRENCY
            ///</summary>
            [Description("Currency")] REDEMPTION_CURRENCY,

            ///<summary>
            ///GRACE_TICKETS
            ///</summary>
            [Description("Grace")] GRACE_TICKETS,

            ///<summary>
            ///MANUAL_TICKETS
            ///</summary>
            [Description("Manual")] MANUAL_TICKETS,

            ///<summary>
            ///TURNIN_TICKETS
            ///</summary>
            [Description("TurnIn")] TURNIN_TICKETS,
            ///<summary>
            ///CURRENCY_RULE
            ///</summary>
            [Description("RedemptionCurrencyRule")] REDEMPTION_CURRENCY_RULE
        }

        private int redemptionId;
        private string primaryCardNumber;
        private int? manualTickets;
        private int? eTickets;
        private DateTime? redeemedDate;
        private int cardId;
        private int origRedemptionId;
        private String remarks;
        private int? graceTickets;
        private int? receiptTickets;
        private int? currencyTickets;
        private string lastUpdatedBy;
        private int siteId;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        private string redemptionOrderNo;           //Added 5-Jan-2018
        private string source;
        private DateTime lastUpdateDate;
        private DateTime? orderCompletedDate;
        private DateTime? orderDeliveredDate;
        private string redemptionStatus;
        private DateTime creationDate;
        private string createdBy;
        private string customerName;
        private int posMachineId;
        private int customerId;
        private string posMachineName;
        private string originalRedemptionOrderNo;

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        private List<RedemptionGiftsDTO> redemptionGiftsListDTO;

        private List<RedemptionCardsDTO> redemptionCardsListDTO;
        private List<TicketReceiptDTO> ticketReceiptListDTO;
        private List<RedemptionTicketAllocationDTO> redemptionTicketAllocationListDTO;

        /// <summary>
        /// Default constructor
        /// </summary>
        public RedemptionDTO()
        {
            log.LogMethodEntry();
            redemptionId = -1;
            masterEntityId = -1;
            siteId = -1;
            cardId = -1;
            origRedemptionId = -1;
            posMachineId = -1;
            customerId = -1;
            redemptionGiftsListDTO = new List<RedemptionGiftsDTO>();
            redemptionCardsListDTO = new List<RedemptionCardsDTO>();
            ticketReceiptListDTO = new List<TicketReceiptDTO>();
            redemptionTicketAllocationListDTO = new List<RedemptionTicketAllocationDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with Required data fields
        /// </summary>
        public RedemptionDTO(int redemptionId, string primaryCardNumber, int? manualTickets, int? eTickets, DateTime? redeemedDate, int cardId,
                         int origRedemptionId, String remarks, int? graceTickets, int? receiptTickets, int? currencyTickets, string source, string redemptionOrderNo,
                         DateTime? orderCompletedDate, DateTime? orderDeliveredDate, string redemptionStatus, List<RedemptionGiftsDTO> redemptionGiftsListDTO,
                         List<RedemptionCardsDTO> redemptionCardsListDTO, List<TicketReceiptDTO> ticketReceiptListDTO, List<RedemptionTicketAllocationDTO> redemptionTicketAllocationListDTO, string customerName, int posMachineId, int customerId, string posMachineName, string originalRedemptionOrderNo)
            : this()
        {
            log.LogMethodEntry(redemptionId, primaryCardNumber, manualTickets, eTickets, redeemedDate, cardId,
                         origRedemptionId, remarks, graceTickets, receiptTickets, currencyTickets, source, redemptionOrderNo,
                          orderCompletedDate, orderDeliveredDate, redemptionStatus, redemptionGiftsListDTO,
                          redemptionCardsListDTO, ticketReceiptListDTO, redemptionTicketAllocationListDTO, customerName, posMachineId, customerId, posMachineName, originalRedemptionOrderNo);

            this.redemptionId = redemptionId;
            this.primaryCardNumber = primaryCardNumber;
            this.manualTickets = manualTickets;
            this.eTickets = eTickets;
            this.redeemedDate = redeemedDate;
            this.cardId = cardId;
            this.origRedemptionId = origRedemptionId;
            this.remarks = remarks;
            this.graceTickets = graceTickets;
            this.receiptTickets = receiptTickets;
            this.currencyTickets = currencyTickets;
            this.source = source;
            this.redemptionOrderNo = redemptionOrderNo;
            this.orderCompletedDate = orderCompletedDate;
            this.orderDeliveredDate = orderDeliveredDate;
            this.redemptionStatus = redemptionStatus;

            if (redemptionGiftsListDTO != null)
            {
                this.redemptionGiftsListDTO = redemptionGiftsListDTO;
            }
            else
            {
                this.redemptionGiftsListDTO = new List<RedemptionGiftsDTO>();
            }
            if (redemptionCardsListDTO != null)
            {
                this.redemptionCardsListDTO = redemptionCardsListDTO;
            }
            else
            {
                this.redemptionCardsListDTO = new List<RedemptionCardsDTO>();
            }
            if (ticketReceiptListDTO != null)
            {
                this.ticketReceiptListDTO = ticketReceiptListDTO;
            }
            else
            {
                this.ticketReceiptListDTO = new List<TicketReceiptDTO>();
            }
            if (redemptionTicketAllocationListDTO != null)
            {
                this.redemptionTicketAllocationListDTO = redemptionTicketAllocationListDTO;
            }
            else
            {
                this.redemptionTicketAllocationListDTO = new List<RedemptionTicketAllocationDTO>();
            }
            this.customerName = customerName;
            this.posMachineId = posMachineId;
            this.customerId = customerId;
            this.posMachineName = posMachineName;
            this.originalRedemptionOrderNo = originalRedemptionOrderNo;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public RedemptionDTO(int redemptionId, string primaryCardNumber, int? manualTickets, int? eTickets, DateTime? redeemedDate, int cardId,
                         int origRedemptionId, String remarks, int? graceTickets, int? receiptTickets, int? currencyTickets,
                         string lastUpdatedBy, int siteId, string guid, bool synchStatus, int masterEntityId, string source, string redemptionOrderNo, DateTime lastUpdateDate,
                         DateTime? orderCompletedDate, DateTime? orderDeliveredDate, string redemptionStatus, DateTime creationDate, string createdBy, List<RedemptionGiftsDTO> redemptionGiftsListDTO,
                         List<RedemptionCardsDTO> redemptionCardsListDTO, List<TicketReceiptDTO> ticketReceiptListDTO, List<RedemptionTicketAllocationDTO> redemptionTicketAllocationListDTO, string customerName, int posMachineId, int customerId, string posMachineName, string originalRedemptionOrderNo)
            : this(redemptionId, primaryCardNumber, manualTickets, eTickets, redeemedDate, cardId,
                         origRedemptionId, remarks, graceTickets, receiptTickets, currencyTickets, source, redemptionOrderNo,
                          orderCompletedDate, orderDeliveredDate, redemptionStatus, redemptionGiftsListDTO,
                          redemptionCardsListDTO, ticketReceiptListDTO, redemptionTicketAllocationListDTO, customerName, posMachineId, customerId, posMachineName, originalRedemptionOrderNo)
        {
            log.LogMethodEntry(redemptionId, primaryCardNumber, manualTickets, eTickets, redeemedDate, cardId,
                         origRedemptionId, remarks, graceTickets, receiptTickets, currencyTickets,
                          lastUpdatedBy, siteId, guid, synchStatus, masterEntityId, source, redemptionOrderNo, lastUpdateDate,
                          orderCompletedDate, orderDeliveredDate, redemptionStatus, creationDate, createdBy, redemptionGiftsListDTO,
                          redemptionCardsListDTO, ticketReceiptListDTO, redemptionTicketAllocationListDTO, customerName, posMachineId, customerId, posMachineName, originalRedemptionOrderNo);

            this.lastUpdatedBy = lastUpdatedBy;
            this.siteId = siteId;
            this.masterEntityId = masterEntityId;
            this.synchStatus = synchStatus;
            this.guid = guid;
            this.creationDate = creationDate;
            this.createdBy = createdBy;
            this.lastUpdateDate = lastUpdateDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the RedemptionId field
        /// </summary>
        [DisplayName("ID")]
        [ReadOnly(true)]
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
        /// Get/Set method of the PrimaryCardNumber field
        /// </summary>
        [DisplayName("Primary Card Number")]
        public string PrimaryCardNumber
        {
            get
            {
                return primaryCardNumber;
            }

            set
            {
                this.IsChanged = true;
                primaryCardNumber = value;
            }
        }
        /// <summary>
        /// Get/Set method of the ManualTickets field
        /// </summary>
        [DisplayName("Manual Tickets")]
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
        /// Get/Set method of the ETickets field
        /// </summary>
        [DisplayName("e-Tickets")]
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
        /// Get/Set method of the RedeemedDate field
        /// </summary>
        [DisplayName("Redeemed Date")]
        public DateTime? RedeemedDate
        {
            get
            {
                return redeemedDate;
            }

            set
            {
                this.IsChanged = true;
                redeemedDate = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Card Id field
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
        /// Get/Set method of the OrigRedemptionId field
        /// </summary>
        [DisplayName("Original Redemption Id")]
        public int OrigRedemptionId
        {
            get
            {
                return origRedemptionId;
            }

            set
            {
                this.IsChanged = true;
                origRedemptionId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Remarks field
        /// </summary>
        [DisplayName("Remarks")]
        public string Remarks
        {
            get
            {
                return remarks;
            }

            set
            {
                this.IsChanged = true;
                remarks = value;
            }
        }

        /// <summary>
        /// Get/Set method of the GraceTickets field
        /// </summary>
        [DisplayName("Grace Tickets")]
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
        /// Get/Set method of the ReceiptTickets field
        /// </summary>
        [DisplayName("Receipt Tickets")]
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
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
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

        ///<summary>
        ///Get/Set method of the RedemptionOrderNo field
        ///</summary>
        [Browsable(true)]
        public string RedemptionOrderNo
        {
            get
            {
                return redemptionOrderNo;
            }

            set
            {
                this.IsChanged = true;
                redemptionOrderNo = value;
            }
        }
        ///<summary>
        ///Get/Set method of the Source field
        ///</summary>
        [Browsable(true)]
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

        ///<summary>
        ///Get/Set method of the LastUpdateDate field
        ///</summary>
        [Browsable(true)]
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

        ///<summary>
        ///Get/Set method of the OrderCompletedDate field
        ///</summary>
        [Browsable(true)]
        public DateTime? OrderCompletedDate
        {
            get
            {
                return orderCompletedDate;
            }

            set
            {
                this.IsChanged = true;
                orderCompletedDate = value;
            }
        }

        ///<summary>
        ///Get/Set method of the OrderDeliveredDate field
        ///</summary>
        [Browsable(true)]
        public DateTime? OrderDeliveredDate
        {
            get
            {
                return orderDeliveredDate;
            }

            set
            {
                this.IsChanged = true;
                orderDeliveredDate = value;
            }
        }

        ///<summary>
        ///Get/Set method of the RedemptionStatus field
        ///</summary>
        [Browsable(true)]
        public string RedemptionStatus
        {
            get
            {
                return redemptionStatus;
            }

            set
            {
                this.IsChanged = true;
                redemptionStatus = value;
            }
        }

        ///<summary>
        ///Get/Set method of the CreationDate field
        ///</summary>
        [Browsable(true)]
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

        ///<summary>
        ///Get/Set method of the CreatedBy field
        ///</summary>
        [Browsable(true)]
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
        /// Get/Set method of the POSMachineId field
        /// </summary>
        [DisplayName("POS Machine Id")]
        public int POSMachineId
        {
            get
            {
                return posMachineId;
            }

            set
            {
                this.IsChanged = true;
                posMachineId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the CustomerId field
        /// </summary>
        [DisplayName("Customer Id")]
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
        /// Get method of the RedemptionGiftsListDTO field
        /// </summary>
        [DisplayName("RedemptionGiftsListDTO")]
        public List<RedemptionGiftsDTO> RedemptionGiftsListDTO
        {
            get
            {
                return redemptionGiftsListDTO;
            }
            set
            {
                redemptionGiftsListDTO = value;
                this.IsChanged = true;
            }
        }
        /// <summary>
        /// Get method of the RedemptionCardsListDTO field
        /// </summary>
        [DisplayName("RedemptionCardsListDTO")]
        public List<RedemptionCardsDTO> RedemptionCardsListDTO
        {
            get
            {
                return redemptionCardsListDTO;
            }
            set
            {
                redemptionCardsListDTO = value;
                this.IsChanged = true;
            }
        }

        /// <summary>
        /// Get method of the TicketReceiptDTO field
        /// </summary>
        [DisplayName("TicketReceiptDTO")]
        public List<TicketReceiptDTO> TicketReceiptListDTO
        {
            get
            {
                return ticketReceiptListDTO;
            }
            set
            {
                ticketReceiptListDTO = value;
                this.IsChanged = true;
            }
        }


        /// <summary>
        /// Get method of the RedemptionTicketAllocationListDTO field
        /// </summary>
        [DisplayName("RedemptionTicketAllocationListDTO")]
        public List<RedemptionTicketAllocationDTO> RedemptionTicketAllocationListDTO
        {
            get
            {
                return redemptionTicketAllocationListDTO;
            }
            set
            {
                redemptionTicketAllocationListDTO = value;
                this.IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the CustomerName field
        /// </summary>
        [DisplayName("Customer Name")]
        public string CustomerName
        {
            get
            {
                return customerName;
            }
        }

        /// <summary>
        /// Get method of the posName field
        /// </summary>
        [DisplayName("POS machine Name")]
        public string PosMachineName
        {
            get
            {
                return posMachineName;
            }
        }

        /// <summary>
        /// Get/Set method of the OriginalRedemptionOrderNo field
        /// </summary>
        [DisplayName("Original Redemption Order No")]
        public string OriginalRedemptionOrderNo
        {
            get
            {
                return originalRedemptionOrderNo;
            }
        }

        /// <summary>
        /// Returns whether the redemptiontDTO changed or any of its redemptionLists  are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (redemptionGiftsListDTO != null &&
                   redemptionGiftsListDTO.Any(x => x.IsChanged))
                {
                    return true;
                }
                if (redemptionCardsListDTO != null &&
                   redemptionCardsListDTO.Any(x => x.IsChanged))
                {
                    return true;
                }
                if (ticketReceiptListDTO != null &&
                  ticketReceiptListDTO.Any(x => x.IsChanged))
                {
                    return true;
                }
                if (redemptionTicketAllocationListDTO != null &&
                  redemptionTicketAllocationListDTO.Any(x => x.IsChanged))
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
                    return notifyingObjectIsChanged || redemptionId < 0;
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
        /// Returns a string that represents the current RedemptionDTO.
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            log.LogMethodEntry();
            StringBuilder returnValue = new StringBuilder("\n-----------------------RedemptionDTO-----------------------------\n");
            returnValue.Append(" RedemptionID : " + RedemptionId);
            returnValue.Append(" PrimaryCardNumber : " + PrimaryCardNumber);
            returnValue.Append(" ManualTickets : " + ManualTickets);
            returnValue.Append(" E-Tickets : " + ETickets);
            returnValue.Append(" RedeemedDate : " + RedeemedDate);
            returnValue.Append(" CardId : " + CardId);
            returnValue.Append(" OrigRedemptionId : " + OrigRedemptionId);
            returnValue.Append(" OriginalRedemptionOrderNo : " + OriginalRedemptionOrderNo);
            returnValue.Append(" Remarks : " + Remarks);
            returnValue.Append(" GraceTickets : " + GraceTickets);
            returnValue.Append(" ReceiptTickets : " + ReceiptTickets);
            returnValue.Append(" CurrencyTickets : " + CurrencyTickets);
            returnValue.Append(" Source : " + Source);
            returnValue.Append(" RedemptionOrderNo : " + RedemptionOrderNo);
            returnValue.Append(" LastUpdateDate : " + LastUpdateDate);
            returnValue.Append(" OrderCompletedDate : " + OrderCompletedDate);
            returnValue.Append(" OrderDeliveredDate : " + OrderDeliveredDate);
            returnValue.Append(" RedemptionStatus : " + RedemptionStatus);
            returnValue.Append(" customerName : " + CustomerName);
            returnValue.Append(" CustomerId : " + CustomerId);
            returnValue.Append(" POSMachineId : " + POSMachineId);
            returnValue.Append(" PosMachineName : " + PosMachineName);
            returnValue.Append("\n-------------------------------------------------------------\n");
            log.LogMethodExit(returnValue.ToString());
            return returnValue.ToString();
        }
    }
}