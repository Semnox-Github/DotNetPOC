/********************************************************************************************
 * Project Name - CardCore / Account
 * Description  - Data object of DailyCardBalance
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70        22-May-2019   Girish Kundar           Modified : Changed the structure  
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.CardCore
{
    public class DailyCardBalanceDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        /// 
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  dailyCardBalanceId field
            /// </summary>
            DAILY_CARD_BALANCE_ID,
            /// <summary>
            /// Search by  customerId field
            /// </summary>
            CUSTOMER_ID,
            /// <summary>
            /// Search by  cardId field
            /// </summary>
            CARD_ID,
            /// <summary>
            /// Search by  cardBalanceDate field
            /// </summary>
            CARD_BALANCE_DATE,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by  cardBalanceDate field
            /// </summary>
            CARD_BALANCE_DATE_FROM,
            /// <summary>
            /// Search by  cardBalanceDate field
            /// </summary>
            CARD_BALANCE_DATE_TO,
            /// <summary>
            /// Search by  creditPlusAttribute field
            /// </summary>
            CREDIT_PLUS_ATTRIBUTE,
            
        }

           private int dailyCardBalanceId;
           private int customerId;
           private int cardId;
           private DateTime? cardBalanceDate;
           private double totalCreditPlusBalance;
           private double earnedCreditPlusBalance;
           private string creditPlusAttribute;
           private string createdBy;
           private DateTime creationDate;
           private string lastUpdatedBy;
           private DateTime lastUpdateDate;
           private string guid;
           private int siteId;
           private int masterEntityId;
           private bool synchStatus;
           private bool notifyingObjectIsChanged;
           private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public DailyCardBalanceDTO()
        {
            log.LogMethodEntry();
            dailyCardBalanceId = -1;
            customerId = -1;
            cardId = -1; 
            siteId = -1;
            masterEntityId = -1;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with Required data fields
        /// </summary>
        public DailyCardBalanceDTO(int dailyCardBalanceId, int customerId, int cardId, DateTime? cardBalanceDate, 
                                   double totalCreditPlusBalance, double earnedCreditPlusBalance, string creditPlusAttribute)
            :this()
        {
            log.LogMethodEntry(dailyCardBalanceId, customerId, cardId, cardBalanceDate, totalCreditPlusBalance, earnedCreditPlusBalance, 
                               creditPlusAttribute);
            this.dailyCardBalanceId = dailyCardBalanceId;
            this.customerId = customerId;
            this.cardId = cardId;
            this.cardBalanceDate = cardBalanceDate;
            this.totalCreditPlusBalance = totalCreditPlusBalance;
            this.earnedCreditPlusBalance = earnedCreditPlusBalance;
            this.creditPlusAttribute = creditPlusAttribute;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public DailyCardBalanceDTO(int dailyCardBalanceId, int customerId, int cardId, DateTime? cardBalanceDate, double totalCreditPlusBalance, double earnedCreditPlusBalance, string creditPlusAttribute, 
                                  string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate, string guid, int siteId, int masterEntityId, bool synchStatus)
            :this(dailyCardBalanceId, customerId, cardId, cardBalanceDate, totalCreditPlusBalance, earnedCreditPlusBalance, 
                  creditPlusAttribute)
        {
            log.LogMethodEntry(dailyCardBalanceId, customerId, cardId, cardBalanceDate, totalCreditPlusBalance, earnedCreditPlusBalance, creditPlusAttribute,
                               createdBy, creationDate, lastUpdatedBy, lastUpdateDate, guid, siteId, masterEntityId, synchStatus);
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            this.guid = guid;
            this.siteId = siteId;
            this.masterEntityId = masterEntityId;
            this.synchStatus = synchStatus;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the dailyCardBalanceId field
        /// </summary>
        [DisplayName("DailyCardBalanceId")]
        public int DailyCardBalanceId
        {
            get
            {
                return dailyCardBalanceId;
            }

            set
            {
                this.IsChanged = true;
                dailyCardBalanceId = value;
            }
        }
        /// <summary>
        /// Get/Set method of the CustomerId field
        /// </summary>
        [DisplayName("CustomerId")]
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
        /// Get/Set method of the CardId field
        /// </summary>
        [DisplayName("CardId")]
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
        /// Get/Set method of the CardBalanceDate field
        /// </summary>
        [DisplayName("CardBalanceDate")]
        public DateTime? CardBalanceDate
        {
            get
            {
                return cardBalanceDate;
            }
            set
            {
                this.IsChanged = true;
                cardBalanceDate = value;
            }
        }
        /// <summary>
        /// Get/Set method of the TotalCreditPlusBalance field
        /// </summary>
        [DisplayName("TotalCreditPlusBalance")]
        public double TotalCreditPlusBalance
        {
            get
            {
                return totalCreditPlusBalance;
            }
            set
            {
                this.IsChanged = true;
                totalCreditPlusBalance = value;
            }
        }

        /// <summary>
        /// Get/Set method of the EarnedCreditPlusBalance field
        /// </summary>
        [DisplayName("EarnedCreditPlusBalance")]
        public double EarnedCreditPlusBalance
        {
            get
            {
                return earnedCreditPlusBalance;
            }
            set
            {
                this.IsChanged = true;
                earnedCreditPlusBalance = value;
            }
        }
        /// <summary>
        /// Get/Set method of the CreditPlusAttribute field
        /// </summary>
        [DisplayName("CreditPlusAttribute")]
        public string CreditPlusAttribute
        {
            get
            {
                return creditPlusAttribute;
            }
            set
            {
                this.IsChanged = true;
                creditPlusAttribute = value;
            }
        }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        [DisplayName("CreatedBy")]
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
        [DisplayName("CreationDate")]
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
        [DisplayName("LastUpdatedBy")]
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
        /// Get/Set method of the LastUpdateDate field
        /// </summary>
        [DisplayName("LastUpdateDate")]
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
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
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
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("SiteId")]
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
        [DisplayName("MasterEntityId")]
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
        [DisplayName("SynchStatus")]
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
        /// Get/Set method to track changes to the object
        /// </summary>
        [Browsable(false)]
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || dailyCardBalanceId < 0;
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
