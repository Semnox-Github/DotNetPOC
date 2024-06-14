/********************************************************************************************
 * Project Name - CardCoreDTO Programs 
 * Description  - Data object of the CardCoreDTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 *********************************************************************************************
 *1.00       15-Nov-2016   Rakshith           Created 
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
    public class CardCoreDTO
    {
        int cardId;
        string card_number;
        DateTime issue_date;
        float face_value;
        char refund_flag;
        float refund_amount;
        DateTime refund_date;
        char valid_flag;
        int ticket_count;
        string notes;
        string last_update_time;
        decimal credits;
        decimal courtesy;
        decimal bonus;
        decimal time;
        int customer_id;
        decimal credits_played;
        char ticket_allowed;
        char real_ticket_mode;
        char vip_customer;
        int site_id;
        DateTime start_time;
        DateTime last_played_time;
        char technician_card;
        int tech_games;
        char timer_reset_card;
        int loyalty_points;
        string lastUpdatedBy;
        int cardTypeId;
        string guid;
        int upload_site_id;
        DateTime upload_time;
        bool synchStatus;
        DateTime expiryDate;
        int downloadBatchId;
        DateTime refreshFromHQTime;
        int masterEntityId;
        string cardIdentifier;
        string primaryCard;
        List<CardCreditPlusDTO> cardCreditPlusDTOList;
        List<CardGamesDTO> cardGamesDTOList;
        List<CardDiscountsDTO> cardDiscountsDTOList;

        CustomerFingerPrintDTO customerFingerPrintDTO;
        CardCreditPlusBalanceDTO cardCreditPlusBalanceDTO;

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        bool notifyingObjectIsChanged;
        readonly object notifyingObjectIsChangedSyncRoot = new Object();

        public enum SearchByParameters
        {
            /// <summary>
            /// Search by CARD_NUMBER field
            /// </summary>
            CARD_NUMBER = 0,
            /// <summary>
            /// Search by CARD_ID field
            /// </summary>
            CARD_ID = 1,
            /// <summary>
            /// Search by CUSTOMER_ID field
            /// </summary>
            CUSTOMER_ID = 2,
            /// <summary>
            /// Search by VALID_FLAG field
            /// </summary>
            VALID_FLAG = 3,
            /// <summary>
            /// Search by CUSTOMER_PHONE_NUMBER field
            /// </summary>
            CUSTOMER_PHONE_NUMBER = 4,

        }

        /// <summary>
        /// AccountEntitlementValidityStatus enum defines value for entitlement status
        /// </summary>
        public enum CardValidityStatus
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

        /// <summary>
        /// Default Constructor
        /// </summary>
        public CardCoreDTO()
        {
            this.cardId = -1;
            this.customer_id = -1;
            this.site_id = -1;
            this.card_number = "";
            this.credits =0;
            this.technician_card = 'N';
            this.Valid_flag= 'Y';
            this.cardIdentifier = "";
            this.primaryCard = "N";
            cardCreditPlusDTOList = new List<CardCreditPlusDTO>();
            cardGamesDTOList = new List<CardGamesDTO>();
            cardDiscountsDTOList = new List<CardDiscountsDTO>();

        }


        /// <summary>
        /// Parameterized Contructor
        /// </summary>
        public CardCoreDTO(int cardId, string card_number, DateTime issue_date, float face_value, char refund_flag, float refund_amount, DateTime refund_date, char valid_flag,
                           int ticket_count, string notes, string last_update_time, decimal credits, decimal courtesy, decimal bonus,
                           decimal time, int customer_id, decimal credits_played, char ticket_allowed, char real_ticket_mode,
                           char vip_customer, int site_id, DateTime start_time, DateTime last_played_time, char technician_card, int tech_games,
                           char timer_reset_card, int loyalty_points, string lastUpdatedBy, int cardTypeId, string guid, int upload_site_id, DateTime upload_time,
                           bool synchStatus, DateTime expiryDate, int downloadBatchId, DateTime refreshFromHQTime, int masterEntityId, string primaryCard)
        {
            this.cardId = cardId;
            this.card_number = card_number;
            this.issue_date = issue_date;
            this.face_value = face_value;
            this.refund_flag = refund_flag;
            this.refund_amount = refund_amount;
            this.refund_date = refund_date;
            this.valid_flag = valid_flag;
            this.ticket_count = ticket_count;
            this.notes = notes;
            this.last_update_time = last_update_time;
            this.credits = credits;
            this.courtesy = courtesy;
            this.bonus = bonus;
            this.time = time;
            this.customer_id = customer_id;
            this.credits_played = credits_played;
            this.ticket_allowed = ticket_allowed;
            this.real_ticket_mode = real_ticket_mode;
            this.vip_customer = vip_customer;
            this.site_id = site_id;
            this.start_time = start_time;
            this.last_played_time = last_played_time;
            this.technician_card = technician_card;
            this.tech_games = tech_games;
            this.timer_reset_card = timer_reset_card;
            this.loyalty_points = loyalty_points;
            this.lastUpdatedBy = lastUpdatedBy;
            this.cardTypeId = cardTypeId;
            this.guid = guid;
            this.Upload_site_id = upload_site_id;
            this.upload_time = upload_time;
            this.synchStatus = synchStatus;
            this.expiryDate = expiryDate;
            this.downloadBatchId = downloadBatchId;
            this.refreshFromHQTime = refreshFromHQTime;
            this.masterEntityId = masterEntityId;
            this.primaryCard = primaryCard;
        }



        [DefaultValue(-1)]
        public int CardId { get { return cardId; } set { cardId = value; this.IsChanged = true; } }

        public string Card_number { get { return card_number; } set { card_number = value; this.IsChanged = true; } }
        public DateTime Issue_date { get { return issue_date; } set { issue_date = value; this.IsChanged = true; } }
        public float Face_value { get { return face_value; } set { face_value = value; this.IsChanged = true; } }
        public char Refund_flag { get { return refund_flag; } set { refund_flag = value; this.IsChanged = true; } }
        public float Refund_amount { get { return refund_amount; } set { refund_amount = value; this.IsChanged = true; } }
        public DateTime Refund_date { get { return refund_date; } set { refund_date = value; this.IsChanged = true; } }
        public char Valid_flag { get { return valid_flag; } set { valid_flag = value; this.IsChanged = true; } }
        public int Ticket_count { get { return ticket_count; } set { ticket_count = value; this.IsChanged = true; } }
        public string Notes { get { return notes; } set { notes = value; this.IsChanged = true; } }
        public decimal Credits { get { return credits; } set { credits = value; this.IsChanged = true; } }
        public decimal Courtesy { get { return courtesy; } set { courtesy = value; this.IsChanged = true; } }
        public decimal Bonus { get { return bonus; } set { bonus = value; this.IsChanged = true; } }
        public decimal Time { get { return time; } set { time = value; this.IsChanged = true; } }
        public int Customer_id { get { return customer_id; } set { customer_id = value; this.IsChanged = true; } }
        public decimal Credits_played { get { return credits_played; } set { credits_played = value; this.IsChanged = true; } }
        public char Ticket_allowed { get { return ticket_allowed; } set { ticket_allowed = value; this.IsChanged = true; } }
        public char Real_ticket_mode { get { return real_ticket_mode; } set { real_ticket_mode = value; this.IsChanged = true; } }
        public char Vip_customer { get { return vip_customer; } set { vip_customer = value; this.IsChanged = true; } }
        public DateTime Start_time { get { return start_time; } set { start_time = value; this.IsChanged = true; } }
        public DateTime Last_played_time { get { return last_played_time; } set { last_played_time = value; this.IsChanged = true; } }
        public char Technician_card { get { return technician_card; } set { technician_card = value; this.IsChanged = true; } }
        public int Tech_games { get { return tech_games; } set { tech_games = value; this.IsChanged = true; } }
        public char Timer_reset_card { get { return timer_reset_card; } set { timer_reset_card = value; this.IsChanged = true; } }
        public int Loyalty_points { get { return loyalty_points; } set { loyalty_points = value; this.IsChanged = true; } }
        public int CardTypeId { get { return cardTypeId; } set { cardTypeId = value; this.IsChanged = true; } }
        public int Upload_site_id { get { return upload_site_id; } set { upload_site_id = value; this.IsChanged = true; } }
        public DateTime Upload_time { get { return upload_time; } set { upload_time = value; this.IsChanged = true; } }
        public DateTime ExpiryDate { get { return expiryDate; } set { expiryDate = value; this.IsChanged = true; } }
        public int DownloadBatchId { get { return downloadBatchId; } set { downloadBatchId = value; this.IsChanged = true; } }
        public DateTime RefreshFromHQTime { get { return refreshFromHQTime; } set { refreshFromHQTime = value; this.IsChanged = true; } }
        
        public int Site_id { get { return site_id; } set { site_id = value;  } }
        public string Guid { get { return guid; } set { guid = value;  } }
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value;  } }
        public string Last_update_time { get { return last_update_time; } set { last_update_time = value; } }
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value;  } }
        public CustomerFingerPrintDTO CustomerFingerPrintDTO { get { return customerFingerPrintDTO; } set { customerFingerPrintDTO = value; } }
        public CardCreditPlusBalanceDTO CardCreditPlusBalanceDTO { get { return cardCreditPlusBalanceDTO; } set { cardCreditPlusBalanceDTO = value; } }


        public string CardIdentifier { get { return cardIdentifier; } set { cardIdentifier = value; this.IsChanged = true; } }

        public string PrimaryCard { get { return primaryCard; } set { primaryCard = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set methods for cardCoreDTOList 
        /// </summary>
        public List<CardCreditPlusDTO> CardCreditPlusDTOList
        {
            get
            {
                return cardCreditPlusDTOList;
            }

            set
            {
                cardCreditPlusDTOList = value;
                this.IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set methods for cardCoreDTOList 
        /// </summary>
        public List<CardGamesDTO> CardGamesDTOList
        {
            get
            {
                return cardGamesDTOList;
            }

            set
            {
                cardGamesDTOList = value;
                this.IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set methods for cardDiscountsDTOList 
        /// </summary>
        public List<CardDiscountsDTO> CardDiscountsDTOList
        {
            get
            {
                return cardDiscountsDTOList;
            }

            set
            {
                cardDiscountsDTOList = value;
                this.IsChanged = true;
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
                    return notifyingObjectIsChanged || cardId < 0 ;
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
            log.Debug("Starts-AcceptChanges() Method.");
            this.IsChanged = false;
            log.Debug("Ends-AcceptChanges() Method.");
        }


    }
}
