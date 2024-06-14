/********************************************************************************************
* Project Name - CustomerCreditCardsDTO
* Description - DTO for CustomerCreditCards 
*
**************
**Version Log 
**************
*Version    Date          Modified By     Remarks
*********************************************************************************************
*2.110.0    08-Dec-2020   Guru S A          Created for subscription feature
*********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// CustomerCreditCardsDTO
    /// </summary>
    public class CustomerCreditCardsDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int customerCreditCardsId;
        private int customerId;
        private string cardProfileId;
        private string tokenId;
        private string customerNameOnCard;
        private string creditCardNumber;
        private string cardExpiry;
        private int paymentModeId;
        private DateTime? lastCreditCardExpiryReminderSentOn;
        private int? creditCardExpiryReminderCount;
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

        /// <summary>
        /// SearchByParameters
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  customerCreditCardsId field
            /// </summary>
            CUSTOMER_CREDITCARDS_ID,
            /// <summary>
            /// Search by  customerId field
            /// </summary>
            CUSTOMER_ID,
            /// <summary>
            /// Search by  cardProfileId field
            /// </summary>
            CARD_PROFILE_ID,
            /// <summary>
            /// Search by  PAYMENT_MODE_ID field
            /// </summary>
            PAYMENT_MODE_ID,
            /// <summary>
            /// Search by  tokenId field
            /// </summary>
            TOKEN_ID,
            /// <summary>
            /// Search by  IS_ACTIVE field
            /// </summary> 
            IS_ACTIVE,
            /// <summary>
            /// Search by site_id field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by Master entity id field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search byEXPIREDCARD_LINKED_WITH_UNBILLED_SUBSCRIPTIONS field
            /// </summary>
            EXPIREDCARD_LINKED_WITH_UNBILLED_SUBSCRIPTIONS,
            /// <summary>
            /// Search by CARDS_EXPIRING_BEFORE_NEXT_UNBILLED_CYCLE field
            /// </summary>
            CARDS_EXPIRING_BEFORE_NEXT_UNBILLED_CYCLE,
            /// <summary>
            /// Search byCARDS_EXPIRING_IN_X_DAYS field
            /// </summary>
            CARDS_EXPIRING_IN_X_DAYS,
            /// <summary>
            /// Search by LINKED_WITH_ACTIVE_SUBSCRIPTIONS field
            /// </summary>
            LINKED_WITH_ACTIVE_SUBSCRIPTIONS
        }

        /// <summary>
        /// CustomerCreditCardsDTO
        /// </summary>
        public CustomerCreditCardsDTO()
        {
            log.LogMethodEntry();
            customerCreditCardsId = -1;
            customerId = -1;
            masterEntityId = -1;
            paymentModeId = -1;
            isActive = true;
            siteId = -1; 
            log.LogMethodExit();
        }
        /// <summary>
        /// CustomerCreditCardsDTO
        /// </summary>
        /// <param name="customerCreditCardsId"></param>
        /// <param name="customerId"></param>
        /// <param name="cardProfileId"></param>
        /// <param name="paymentModeId"></param>
        /// <param name="tokenId"></param>
        /// <param name="customerNameOnCard"></param>
        /// <param name="creditCardNumber"></param>
        /// <param name="cardExpiry"></param>
        /// <param name="lastCreditCardExpiryReminderSentOn"></param>
        /// <param name="creditCardExpiryReminderCount"></param>
        public CustomerCreditCardsDTO(int customerCreditCardsId, int customerId, string cardProfileId, int paymentModeId, string tokenId, string customerNameOnCard,
                                      string creditCardNumber, string cardExpiry, DateTime? lastCreditCardExpiryReminderSentOn, int? creditCardExpiryReminderCount) : this()
        {
            log.LogMethodEntry(customerCreditCardsId, customerId, cardProfileId, paymentModeId, tokenId, "customerNameOnCard", "creditCardNumber", "cardExpiry",
                lastCreditCardExpiryReminderSentOn, creditCardExpiryReminderCount);
            this.customerCreditCardsId = customerCreditCardsId;
            this.customerId = customerId;
            this.cardProfileId = cardProfileId;
            this.paymentModeId = paymentModeId;
            this.tokenId = tokenId;
            this.customerNameOnCard = customerNameOnCard;
            this.creditCardNumber = creditCardNumber;
            this.cardExpiry = cardExpiry;
            this.lastCreditCardExpiryReminderSentOn = lastCreditCardExpiryReminderSentOn;
            this.creditCardExpiryReminderCount = creditCardExpiryReminderCount;
            log.LogMethodExit();
        }

        /// <summary>
        /// CustomerCreditCardsDTO
        /// </summary>
        /// <param name="customerCreditCardsId"></param>
        /// <param name="customerId"></param>
        /// <param name="cardProfileId"></param>
        /// <param name="paymentModeId"></param>
        /// <param name="tokenId"></param>
        /// <param name="customerNameOnCard"></param>
        /// <param name="creditCardNumber"></param>
        /// <param name="cardExpiry"></param>
        /// <param name="lastCreditCardExpiryReminderSentOn"></param>
        /// <param name="creditCardExpiryReminderCount"></param>
        /// <param name="isActive"></param>
        /// <param name="createdBy"></param>
        /// <param name="creationDate"></param>
        /// <param name="lastUpdatedBy"></param>
        /// <param name="lastUpdateDate"></param>
        /// <param name="siteId"></param>
        /// <param name="masterEntityId"></param>
        /// <param name="synchStatus"></param>
        /// <param name="guid"></param>
        public CustomerCreditCardsDTO(int customerCreditCardsId, int customerId, string cardProfileId, int paymentModeId, string tokenId, string customerNameOnCard, 
              string creditCardNumber, string cardExpiry, DateTime? lastCreditCardExpiryReminderSentOn, int? creditCardExpiryReminderCount
            , bool isActive, string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate,
            int siteId, int masterEntityId, bool synchStatus, string guid)
            : this(customerCreditCardsId, customerId, cardProfileId, paymentModeId, tokenId, customerNameOnCard, creditCardNumber, cardExpiry,
                  lastCreditCardExpiryReminderSentOn, creditCardExpiryReminderCount)

        {
            log.LogMethodEntry(customerCreditCardsId, customerId, cardProfileId, paymentModeId, tokenId, "customerNameOnCard", "creditCardNumber", "cardExpiry",
                lastCreditCardExpiryReminderSentOn, creditCardExpiryReminderCount, isActive, createdBy, creationDate, lastUpdatedBy, lastUpdateDate); 
            this.isActive = isActive;
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
        /// Get/Set method of the CustomerCreditCardsId field
        /// </summary>
        public int CustomerCreditCardsId
        {
            get
            {
                return customerCreditCardsId;
            }

            set
            {
                this.IsChanged = true;
                customerCreditCardsId = value;
            }
        }
        /// <summary>
        /// Get/Set method of the customerId field
        /// </summary>
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
        /// Get/Set method of the cardProfileId field
        /// </summary>
        public string CardProfileId
        {
            get
            {
                return cardProfileId;
            }

            set
            {
                this.IsChanged = true;
                cardProfileId = value;
            }
        }
        /// <summary>
        /// Get/Set method of the paymentModeId field
        /// </summary>
        public int PaymentModeId
        {
            get
            {
                return paymentModeId;
            }

            set
            {
                this.IsChanged = true;
                paymentModeId = value;
            }
        }
        /// <summary>
        /// Get/Set method of the tokenId field
        /// </summary>
        public string TokenId
        {
            get
            {
                return tokenId;
            }

            set
            {
                this.IsChanged = true;
                tokenId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the CustomerNameOnCard field
        /// </summary>
        public string CustomerNameOnCard
        {
            get
            {
                return customerNameOnCard;
            }

            set
            {
                this.IsChanged = true;
                customerNameOnCard = value;
            }
        }

        /// <summary>
        /// Get/Set method of the creditCardNumber field
        /// </summary>
        public string CreditCardNumber
        {
            get
            {
                return creditCardNumber;
            }

            set
            {
                this.IsChanged = true;
                creditCardNumber = value;
            }
        }

        /// <summary>
        /// Get/Set method of the cardExpiry field
        /// </summary>
        public string CardExpiry
        {
            get
            {
                return cardExpiry;
            }

            set
            {
                this.IsChanged = true;
                cardExpiry = value;
            }
        }

        /// <summary>
        /// Get/Set method of the lastCreditCardExpiryReminderSentOn field
        /// </summary>
        public DateTime? LastCreditCardExpiryReminderSentOn
        {
            get { return lastCreditCardExpiryReminderSentOn; }
            set { lastCreditCardExpiryReminderSentOn = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the creditCardExpiryReminderCount field
        /// </summary>
        public int? CreditCardExpiryReminderCount
        {
            get { return creditCardExpiryReminderCount; }
            set { creditCardExpiryReminderCount = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
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

                creationDate = value;
            }
        }
        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
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
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
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
        /// Get/Set method to track changes to the object
        /// </summary>
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || customerCreditCardsId < 0;
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
