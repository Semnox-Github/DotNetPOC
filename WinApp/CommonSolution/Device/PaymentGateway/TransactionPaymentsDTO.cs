/********************************************************************************************
 * Project Name - TransactionPaymentsDTO Programs
 * Description  - Data object of TransactionPayments DTO  
 *  
 **************
 **Version Log
 **************
 *Version     Date           Modified By       Remarks          
 *********************************************************************************************
 *1.00        26-Nov-2017     Lakshmi          Created 
 *2.50.0      05-Dec-2018     Mathew Ninan     Transaction re-design. Added missing fields and 
 *                                             PaymentModeDTO
 *2.60.0      08-May-2019     Nitin Pai        Set CouponSetId to -1 in constructors                                      
 *2.70.2.0    11-Jul-2019     Girish Kundar    Modified : Added Private modifiers for data members 
 *                                                     LogMethodEntry()  and LogMethodExit().     
 *2.100.0     25-Aug-2020     Mathew Ninan     ApprovedBy field added       
 *2.110.0     08-Dec-2020     Guru S A         Subscription changes                                             
 *2.110.0     09-Feb-2021     Abhishek         Modified : Added new field ExternalSourceReference   
 *2.130.7     13-Apr-2022     Guru S A         Payment mode OTP validation changes   
  *2.140.0    09-Nov-2021     Laster Menezes   Added LAST_UPDATE_FROM_TIME to SearchByParameters
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Semnox.Parafait.Device.PaymentGateway
{
    public enum SubscriptionAuthorizationMode
    {
        /// <summary>
        /// None
        /// </summary>
        [Description("None")]
        N,
        /// <summary>
        /// Initial Authorization
        /// </summary>
        [Description("Initial Authorization")]
        I,
        /// <summary>
        /// Subscription Payment
        /// </summary>
        [Description("Subscription Payment")]
        P 
    }
    /// <summary>
    /// This is the TransactionPayments data object class. This acts as data holder for the TransactionPayments business object
    /// </summary>
    public class TransactionPaymentsDTO
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by TrxId field
            /// </summary>
            TRANSACTION_ID,
            /// <summary>
            /// Search by PaymentModeId field
            /// </summary>
            PAYMENT_MODE_ID,
            /// <summary>
            /// Search by CardId field
            /// </summary>
            CARD_ID,
            /// <summary>
            /// Search by OrderId field
            /// </summary>
            ORDER_ID,
            /// <summary>
            /// Search by CCResponseId field
            /// </summary>
            CCRESPONSE_ID,
            /// <summary>
            /// Search by ParentPaymentId field
            /// </summary>
            PARENT_PAYMENT_ID,
            /// <summary>
            /// Search by SplitId field
            /// </summary>
            SPLIT_ID,
            /// <summary>
            /// Search by PosMachine field
            /// </summary>
            POS_MACHINE,
            /// <summary>
            /// Search by LastUpdatedUser field
            /// </summary>
            LAST_UPDATED_USER,
            /// <summary>
            /// Search by site_Id field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by CreditCardAuthorization field
            /// </summary>
            CREDIT_CARD_AUTHORIZATION,
            /// <summary>
            /// Search by Payment Id field
            /// </summary>
            PAYMENT_ID,
            /// <summary>
            /// Search by Master Entity Id field
            /// </summary>
            MASTER_ENTITY_ID,

            /// <summary>
            /// Search by LastUpdatedDate field
            /// </summary>
            LAST_UPDATE_FROM_TIME,
            /// <summary>
            /// Search by ATTRIBUTE1 field
            /// </summary>
            ATTRIBUTE1,
            /// <summary>
            /// Search by ATTRIBUTE2 field
            /// </summary>
            ATTRIBUTE2,
            /// <summary>
            /// Search by ATTRIBUTE3 field
            /// </summary>
            ATTRIBUTE3,
            /// <summary>
            /// Search by ATTRIBUTE4 field
            /// </summary>
            ATTRIBUTE4,
            /// <summary>
            /// Search by ATTRIBUTE5 field
            /// </summary>
            ATTRIBUTE5,
            /// <summary>
            /// Search by DELIVERY_CHANNEL_ID field
            /// </summary>
            DELIVERY_CHANNEL_ID,
            /// <summary>
            /// Search by TRANSACTION_FROM_DATE field
            /// </summary>
            TRANSACTION_FROM_DATE,
            /// <summary>
            /// Search by TRANSACTION_TO_DATE field
            /// </summary>
            TRANSACTION_TO_DATE,
            /// <summary>
            /// Search by Non Reversed Payment field
            /// </summary>
            NON_REVERSED_PAYMENT
        }

        private int paymentId;
        private int transactionId;
        private int paymentModeId;
        private double amount;
        private string creditCardNumber;
        private string nameOnCreditCard;
        private string creditCardName;
        private string creditCardExpiry;
        private string creditCardAuthorization;
        private int cardId;
        private string cardEntitlementType;
        private int cardCreditPlusId;
        private double paymentUsedCreditPlus;
        private int orderId;
        private string reference;
        private string guid;
        private bool synchStatus;
        private int siteId;
        private int cCResponseId;
        private string memo;
        private DateTime paymentDate;
        private string lastUpdatedUser;
        private string createdBy;
        private DateTime creationDate;
        private DateTime lastUpdateDate;
        private int parentPaymentId;
        private double? tenderedAmount;
        private double tipAmount;
        private int splitId;
        private string posMachine;
        private int masterEntityId;
        private string currencyCode;
        private double? currencyRate;
        private double? couponValue;
        private string mcc;
        private int couponSetId;
        private string paymentCardNumber;
        private bool? isTaxable;
        private string approvedBy;
        private SubscriptionAuthorizationMode subscriptionAuthorizationMode;
        private string customerCardProfileId;
        private string externalSourceReference;
        public bool gatewayPaymentProcessed;
        public PaymentModeDTO paymentModeDTO;
        private string attribute1;
        private string attribute2;
        private string attribute3;
        private string attribute4;
        private string attribute5;
        private string paymentModeOTP; 
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();


        /// <summary>
        /// Default constructor
        /// </summary>
        public TransactionPaymentsDTO()
        {
            paymentId = -1;
            splitId = -1;
            cardCreditPlusId = -1;
            paymentUsedCreditPlus = 0;
            cardId = -1;
            cCResponseId = -1;
            orderId = -1;
            paymentModeId = -1;
            transactionId = -1;
            couponSetId = -1;
            parentPaymentId = -1;
            creditCardNumber = string.Empty;
            creditCardExpiry = string.Empty;
            nameOnCreditCard = string.Empty;
            externalSourceReference = string.Empty;
            isTaxable = null;
            couponValue = null;
            gatewayPaymentProcessed = false;
            tipAmount = 0;
            siteId = -1;
            masterEntityId = -1;
            mcc = "";
            customerCardProfileId = string.Empty;
            paymentModeOTP = string.Empty;
            subscriptionAuthorizationMode = SubscriptionAuthorizationMode.N;
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public TransactionPaymentsDTO(int paymentId, int transactionId, int paymentModeId, double amount, string creditCardNumber,
                                      string nameOnCreditCard, string creditCardName, string creditCardExpiry,
                                      string creditCardAuthorization, int cardId, string cardEntitlementType, int cardCreditPlusId,
                                      int orderId, string reference, string guid, bool synchStatus, int siteId, int cCResponseId,
                                      string memo, DateTime paymentDate, string lastUpdatedUser, int parentPaymentId,
                                      double? tenderedAmount, double tipAmount, int splitId, string posMachine, int masterEntityId,
                                      string currencyCode, double? currencyRate)
            :this()
        {
            log.LogMethodEntry(paymentId, transactionId, paymentModeId, amount, "creditCardNumber",
                                "nameOnCreditCard", "creditCardName", "creditCardExpiry",
                                "creditCardAuthorization", cardId, cardEntitlementType, cardCreditPlusId,
                                orderId, reference, guid, synchStatus, siteId, cCResponseId,
                                memo, paymentDate, lastUpdatedUser, parentPaymentId,
                                tenderedAmount, tipAmount, splitId, posMachine, masterEntityId,
                                currencyCode, currencyRate);
            this.couponSetId = -1;
            this.paymentId = paymentId;
            this.transactionId = transactionId;
            this.paymentModeId = paymentModeId;
            this.amount = amount;
            this.creditCardNumber = creditCardNumber;
            this.nameOnCreditCard = nameOnCreditCard;
            this.creditCardName = creditCardName;
            this.creditCardExpiry = creditCardExpiry;
            this.creditCardAuthorization = creditCardAuthorization;
            this.cardId = cardId;
            this.cardEntitlementType = cardEntitlementType;
            this.cardCreditPlusId = cardCreditPlusId;
            this.orderId = orderId;
            this.reference = reference;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.siteId = siteId;
            this.cCResponseId = cCResponseId;
            this.memo = memo;
            this.paymentDate = paymentDate;
            this.lastUpdatedUser = lastUpdatedUser;
            this.parentPaymentId = parentPaymentId;
            this.tenderedAmount = tenderedAmount;
            this.tipAmount = tipAmount;
            this.splitId = splitId;
            this.posMachine = posMachine;
            this.masterEntityId = masterEntityId;
            this.currencyCode = currencyCode;
            this.currencyRate = currencyRate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields :Additional field  - paymentUsedCreditPlus
        /// </summary>
        public TransactionPaymentsDTO(int paymentId, int transactionId, int paymentModeId, double amount, string creditCardNumber,
                                      string nameOnCreditCard, string creditCardName, string creditCardExpiry,
                                      string creditCardAuthorization, int cardId, string cardEntitlementType, int cardCreditPlusId,
                                      double paymentUsedCreditPlus, int orderId, string reference, string guid, bool synchStatus, int siteId, int cCResponseId,
                                      string memo, DateTime paymentDate, string lastUpdatedUser, int parentPaymentId,
                                      double? tenderedAmount, double tipAmount, int splitId, string posMachine, int masterEntityId,
                                      string currencyCode, double? currencyRate)
            :this(paymentId, transactionId, paymentModeId, amount, creditCardNumber,
                                nameOnCreditCard, creditCardName, creditCardExpiry,
                                creditCardAuthorization, cardId, cardEntitlementType, cardCreditPlusId,
                                orderId, reference, guid, synchStatus, siteId, cCResponseId,
                                memo, paymentDate, lastUpdatedUser, parentPaymentId,
                                tenderedAmount, tipAmount, splitId, posMachine, masterEntityId,
                                currencyCode, currencyRate)
        {
            log.LogMethodEntry(paymentId, transactionId, paymentModeId, amount, "creditCardNumber",
                                "nameOnCreditCard", "creditCardName", "creditCardExpiry",
                                "creditCardAuthorization", cardId, cardEntitlementType, cardCreditPlusId,
                                paymentUsedCreditPlus, orderId, reference, guid, synchStatus, siteId, cCResponseId,
                                memo, paymentDate, lastUpdatedUser, parentPaymentId,
                                tenderedAmount, tipAmount, splitId, posMachine, masterEntityId,
                                currencyCode, currencyRate);

            this.paymentUsedCreditPlus = paymentUsedCreditPlus;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public TransactionPaymentsDTO(int paymentId, int transactionId, int paymentModeId, double amount, string creditCardNumber,
                                      string nameOnCreditCard, string creditCardName, string creditCardExpiry,
                                      string creditCardAuthorization, int cardId, string cardEntitlementType, int cardCreditPlusId,
                                      int orderId, string reference, string guid, bool synchStatus, int siteId, int cCResponseId,
                                      string memo, DateTime paymentDate, string lastUpdatedUser, int parentPaymentId,
                                      double? tenderedAmount, double tipAmount, int splitId, string posMachine, int masterEntityId,
                                      string currencyCode, double? currencyRate, bool? isTaxable, double? couponValue,
                                      bool gatewayPaymentProcessed)
            : this(paymentId, transactionId, paymentModeId, amount, creditCardNumber,
                                nameOnCreditCard, creditCardName, creditCardExpiry,
                                creditCardAuthorization, cardId, cardEntitlementType, cardCreditPlusId,
                                orderId, reference, guid, synchStatus, siteId, cCResponseId,
                                memo, paymentDate, lastUpdatedUser, parentPaymentId,
                                tenderedAmount, tipAmount, splitId, posMachine, masterEntityId,
                                currencyCode, currencyRate)
        {
            log.LogMethodEntry(paymentId, transactionId, paymentModeId, amount, "creditCardNumber",
                                "nameOnCreditCard", "creditCardName", "creditCardExpiry",
                                creditCardAuthorization, cardId, cardEntitlementType, cardCreditPlusId,
                                orderId, reference, guid, synchStatus, siteId, cCResponseId,
                                memo, paymentDate, lastUpdatedUser, parentPaymentId,
                                tenderedAmount, tipAmount, splitId, posMachine, masterEntityId,
                                currencyCode, currencyRate, isTaxable, couponValue, gatewayPaymentProcessed);
            this.isTaxable = isTaxable;
            this.couponValue = couponValue;
            this.gatewayPaymentProcessed = gatewayPaymentProcessed;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public TransactionPaymentsDTO(int paymentId, int transactionId, int paymentModeId, double amount, string creditCardNumber,
                                      string nameOnCreditCard, string creditCardName, string creditCardExpiry,
                                      string creditCardAuthorization, int cardId, string cardEntitlementType, int cardCreditPlusId,
                                      int orderId, string reference, string guid, bool synchStatus, int siteId, int cCResponseId,
                                      string memo, DateTime paymentDate, string lastUpdatedUser, int parentPaymentId,
                                      double? tenderedAmount, double tipAmount, int splitId, string posMachine, int masterEntityId,
                                      string currencyCode, double? currencyRate, bool? isTaxable, double? couponValue,
                                      string createdBy, DateTime lastUpdateDate, DateTime creationDate, bool gatewayPaymentProcessed)
            :this(paymentId, transactionId, paymentModeId, amount, creditCardNumber,
                                       nameOnCreditCard, creditCardName, creditCardExpiry,
                                       creditCardAuthorization, cardId, cardEntitlementType, cardCreditPlusId,
                                       orderId, reference, guid, synchStatus, siteId, cCResponseId,
                                       memo, paymentDate, lastUpdatedUser, parentPaymentId,
                                       tenderedAmount, tipAmount, splitId, posMachine, masterEntityId,
                                       currencyCode, currencyRate, isTaxable, couponValue,gatewayPaymentProcessed)
        {
            log.LogMethodEntry(paymentId, transactionId, paymentModeId, amount, "creditCardNumber",
                                "nameOnCreditCard", "creditCardName", "creditCardExpiry",
                                       creditCardAuthorization, cardId, cardEntitlementType, cardCreditPlusId,
                                       orderId, reference, guid, synchStatus, siteId, cCResponseId,
                                       memo, paymentDate, lastUpdatedUser, parentPaymentId,
                                       tenderedAmount, tipAmount, splitId, posMachine, masterEntityId,
                                       currencyCode, currencyRate, isTaxable, couponValue,
                                       createdBy, lastUpdateDate, creationDate, gatewayPaymentProcessed);
            
            this.createdBy = createdBy;
            this.lastUpdateDate = lastUpdateDate;
            this.creationDate = creationDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public TransactionPaymentsDTO(int paymentId, int transactionId, int paymentModeId, double amount, string creditCardNumber,
                                      string nameOnCreditCard, string creditCardName, string creditCardExpiry,
                                      string creditCardAuthorization, int cardId, string cardEntitlementType, int cardCreditPlusId,
                                      int orderId, string reference, string guid, bool synchStatus, int siteId, int cCResponseId,
                                      string memo, DateTime paymentDate, string lastUpdatedUser, int parentPaymentId,
                                      double? tenderedAmount, double tipAmount, int splitId, string posMachine, int masterEntityId,
                                      string currencyCode, double? currencyRate, bool? isTaxable, double? couponValue,
                                      string createdBy, DateTime lastUpdateDate, DateTime creationDate, bool gatewayPaymentProcessed, string approvedBy, string customerCardProfileId, string externalSourceReference, 
                                      string attribute1, string attribute2, string attribute3, string attribute4, string attribute5, string paymentModeOTP)
             : this(paymentId, transactionId, paymentModeId, amount, creditCardNumber,
                                       nameOnCreditCard, creditCardName, creditCardExpiry,
                                       creditCardAuthorization, cardId, cardEntitlementType, cardCreditPlusId,
                                       orderId, reference, guid, synchStatus, siteId, cCResponseId,
                                       memo, paymentDate, lastUpdatedUser, parentPaymentId,
                                       tenderedAmount, tipAmount, splitId, posMachine, masterEntityId,
                                       currencyCode, currencyRate, isTaxable, couponValue, gatewayPaymentProcessed)
        {
            log.LogMethodEntry(paymentId, transactionId, paymentModeId, amount, "creditCardNumber", "nameOnCreditCard", "creditCardName", "creditCardExpiry", "creditCardAuthorization", cardId, 
                              cardEntitlementType, cardCreditPlusId, orderId, reference, guid, synchStatus, siteId, cCResponseId, memo, paymentDate, lastUpdatedUser, parentPaymentId,
                              tenderedAmount, tipAmount, splitId, posMachine, masterEntityId, currencyCode, currencyRate, isTaxable, couponValue, createdBy, lastUpdateDate, creationDate, 
                              approvedBy, customerCardProfileId, externalSourceReference, attribute1, attribute2, attribute3, attribute4, attribute5, paymentModeOTP);

            this.approvedBy = approvedBy;
            this.customerCardProfileId = customerCardProfileId;
            this.externalSourceReference = externalSourceReference;
            this.attribute1 = attribute1;
            this.attribute2 = attribute2;
            this.attribute3 = attribute3;
            this.attribute4 = attribute4;
            this.attribute5 = attribute5;
            this.paymentModeOTP = paymentModeOTP;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the PaymentId field
        /// </summary>
        [Browsable(false)]
        public int PaymentId
        {
            get
            {
                return paymentId;
            }

            set
            {
                IsChanged = true;
                paymentId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the TransactionId field
        /// </summary>
        [DisplayName("Transaction Id")]
        [ReadOnly(true)]
        public int TransactionId
        {
            get
            {
                return transactionId;
            }

            set
            {
                IsChanged = true;
                transactionId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the PaymentModeId field
        /// </summary>
        [Browsable(false)]
        public int PaymentModeId
        {
            get
            {
                return paymentModeId;
            }

            set
            {
                IsChanged = true;
                paymentModeId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Amount field
        /// </summary>
        [DisplayName("Amount")]
        [ReadOnly(true)]
        public double Amount
        {
            get
            {
                return amount;
            }

            set
            {
                IsChanged = true;
                amount = value;
            }
        }

        /// <summary>
        /// Get/Set method of the CreditCardNumber field
        /// </summary>
        //[Browsable(false)]
        [DisplayName("Credit Card Number")]
        [ReadOnly(true)]
        public string CreditCardNumber
        {
            get
            {
                return creditCardNumber;
            }

            set
            {
                IsChanged = true;
                creditCardNumber = value;
            }
        }

        /// <summary>
        /// Get/Set method of the NameOnCreditCard field
        /// </summary>
        [Browsable(false)]
        public string NameOnCreditCard
        {
            get
            {
                return nameOnCreditCard;
            }

            set
            {
                IsChanged = true;
                nameOnCreditCard = value;
            }
        }

        /// <summary>
        /// Get/Set method of the CreditCardName field
        /// </summary>
        [DisplayName("Credit Card Name")]
        [ReadOnly(true)]
        public string CreditCardName
        {
            get
            {
                return creditCardName;
            }

            set
            {
                IsChanged = true;
                creditCardName = value;
            }
        }

        /// <summary>
        /// Get/Set method of the CreditCardExpiry field
        /// </summary>
        [Browsable(false)]
        public string CreditCardExpiry
        {
            get
            {
                return creditCardExpiry;
            }

            set
            {
                IsChanged = true;
                creditCardExpiry = value;
            }
        }

        /// <summary>
        /// Get/Set method of the CreditCardAuthorization field
        /// </summary>
        [Browsable(false)]
        public string CreditCardAuthorization
        {
            get
            {
                return creditCardAuthorization;
            }

            set
            {
                IsChanged = true;
                creditCardAuthorization = value;
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
                IsChanged = true;
                cardId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the CardEntitlementType field
        /// </summary>
        [Browsable(false)]
        public string CardEntitlementType
        {
            get
            {
                return cardEntitlementType;
            }

            set
            {
                IsChanged = true;
                cardEntitlementType = value;
            }
        }

        /// <summary>
        /// Get/Set method of the CardCreditPlusId field
        /// </summary>
        [Browsable(false)]
        public int CardCreditPlusId
        {
            get
            {
                return cardCreditPlusId;
            }

            set
            {
                IsChanged = true;
                cardCreditPlusId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the PaymentUsedCreditPlus field
        /// </summary>
        [Browsable(false)]
        public double PaymentUsedCreditPlus
        {
            get
            {
                return paymentUsedCreditPlus;
            }

            set
            {
                paymentUsedCreditPlus = value;
            }
        }

        /// <summary>
        /// Get/Set method of the OrderId field
        /// </summary>
        [Browsable(false)]
        public int OrderId
        {
            get
            {
                return orderId;
            }

            set
            {
                IsChanged = true;
                orderId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the CouponSetId field
        /// </summary>
        [Browsable(false)]
        public int CouponSetId
        {
            get
            {
                return couponSetId;
            }

            set
            {
                couponSetId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the PaymentCardNumber field
        /// </summary>
        [Browsable(false)]
        public string PaymentCardNumber
        {
            get
            {
                return paymentCardNumber;
            }

            set
            {
                paymentCardNumber = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Reference field
        /// </summary>
        [Browsable(false)]
        public string Reference
        {
            get
            {
                return reference;
            }

            set
            {
                IsChanged = true;
                reference = value;
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
                guid = value;
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
        /// Get/Set method of the CCResponseId field
        /// </summary>
        [Browsable(false)]
        public int CCResponseId
        {
            get
            {
                return cCResponseId;
            }

            set
            {
                IsChanged = true;
                cCResponseId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Memo field
        /// </summary>
        [Browsable(false)]
        public string Memo
        {
            get
            {
                return memo;
            }

            set
            {
                IsChanged = true;
                memo = value;
            }
        }

        /// <summary>
        /// Get method of the PaymentDate field
        /// </summary>
        [DisplayName("Payment Date")]
        [ReadOnly(true)]
        public DateTime PaymentDate
        {
            get
            {
                return paymentDate;
            }
        }

        /// <summary>
        /// Get method of the LastUpdateDate field
        /// </summary>
        [DisplayName("Last Update Date")]
        [ReadOnly(true)]
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
        /// Get method of the CreationDate field
        /// </summary>
        [DisplayName("Creation Date")]
        [ReadOnly(true)]
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
        /// Get method of the CreatedBy field
        /// </summary>
        [DisplayName("CreatedBy")]
        [ReadOnly(true)]
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
        /// Get/Set method of the LastUpdatedUser field
        /// </summary>
        [DisplayName("User Name")]
        [ReadOnly(true)]
        public string LastUpdatedUser
        {
            get
            {
                return lastUpdatedUser;
            }
            set
            {
                lastUpdatedUser = value;
            }
        }

        /// <summary>
        /// Get/Set method of the ParentPaymentId field
        /// </summary>
        [Browsable(false)]
        public int ParentPaymentId
        {
            get
            {
                return parentPaymentId;
            }

            set
            {
                IsChanged = true;
                parentPaymentId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the TenderedAmount field
        /// </summary>
        [Browsable(false)]
        public double? TenderedAmount
        {
            get
            {
                return tenderedAmount;
            }

            set
            {
                IsChanged = true;
                tenderedAmount = value;
            }
        }

        /// <summary>
        /// Get/Set method of the TipAmount field
        /// </summary>
        [DisplayName("Tip Amount")]
        [ReadOnly(true)]
        public double TipAmount
        {
            get
            {
                return tipAmount;
            }

            set
            {
                IsChanged = true;
                tipAmount = value;
            }
        }

        /// <summary>
        /// Get/Set method of the SplitId field
        /// </summary>
        [Browsable(false)]
        public int SplitId
        {
            get
            {
                return splitId;
            }

            set
            {
                IsChanged = true;
                splitId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the PosMachine field
        /// </summary>
        [Browsable(false)]
        public string PosMachine
        {
            get
            {
                return posMachine;
            }

            set
            {
                IsChanged = true;
                posMachine = value;
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
                IsChanged = true;
                masterEntityId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the CurrencyCode field
        /// </summary>
        [Browsable(false)]
        public string CurrencyCode
        {
            get
            {
                return currencyCode;
            }

            set
            {
                IsChanged = true;
                currencyCode = value;
            }
        }

        /// <summary>
        /// Get/Set method of the CurrencyRate field
        /// </summary>
        [Browsable(false)]
        public double? CurrencyRate
        {
            get
            {
                return currencyRate;
            }

            set
            {
                IsChanged = true;
                currencyRate = value;
            }
        }

        /// <summary>
        /// Get/Set method of the IsTaxable field
        /// </summary>
        [Browsable(false)]
        public bool? IsTaxable
        {
            get
            {
                return isTaxable;
            }

            set
            {
                IsChanged = true;
                isTaxable = value;
            }
        }

        /// <summary>
        /// Get/Set method of the CouponValue field
        /// </summary>
        [Browsable(false)]
        public double? CouponValue
        {
            get
            {
                return couponValue;
            }

            set
            {
                IsChanged = true;
                couponValue = value;
            }
        }

        /// <summary>
        /// Get/Set method of the CouponValue field
        /// </summary>
        [Browsable(false)]
        public string ApprovedBy
        {
            get
            {
                return approvedBy;
            }

            set
            {
                IsChanged = true;
                approvedBy = value;
            }
        }

        /// <summary>
        /// Get/Set method of the MCC field
        /// </summary>
        [Browsable(false)]
        public string MCC
        {
            get
            {
                return mcc;
            }

            set
            {
                mcc = value;
            }
        }

        /// <summary>
        /// Get/Set method of the GatewayPaymentProcessed field
        /// </summary>
        [Browsable(false)]
        public bool GatewayPaymentProcessed
        {
            get
            {
                return gatewayPaymentProcessed;
            }

            set
            {
                IsChanged = true;
                gatewayPaymentProcessed = value;
            }
        }

        /// <summary>
        /// Get/Set method of the paymentModeDTO field
        /// </summary>
        [Browsable(false)]
        public PaymentModeDTO PaymentModeDTO
        {
            get
            {
                return paymentModeDTO;
            }

            set
            { 
                paymentModeDTO = value;
            }
        }
        /// <summary>
        /// Get/Set method of the subscriptionAuthorizationMode field
        /// </summary> 
        public SubscriptionAuthorizationMode SubscriptionAuthorizationMode
        {
            get
            {
                return subscriptionAuthorizationMode;
            }

            set
            {
                IsChanged = true;
                subscriptionAuthorizationMode = value;
            }
        }

        /// <summary>
        /// Get/Set method of the customerCardProfileId field
        /// </summary> 
        public string CustomerCardProfileId
        {
            get
            {
                return customerCardProfileId;
            }

            set
            {
                IsChanged = true;
                customerCardProfileId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the ExternalSourceReference field
        /// </summary>
        public string ExternalSourceReference
        {
            get
            {
                return externalSourceReference;
            }

            set
            {
                IsChanged = true;
                externalSourceReference = value;
            }
        }
        /// <summary>
        /// Get/Set method of the Attribute1 field
        /// </summary> 
        public string Attribute1
        {
            get { return attribute1; }
            set { IsChanged = true; attribute1 = value;  }
        }
        /// <summary>
        /// Get/Set method of the Attribute2 field
        /// </summary> 
        public string Attribute2
        {
            get { return attribute2; }
            set { IsChanged = true; attribute2 = value; }
        }
        /// <summary>
        /// Get/Set method of the attribute3 field
        /// </summary> 
        public string Attribute3
        {
            get { return attribute3; }
            set { IsChanged = true; attribute3 = value; }
        }
        /// <summary>
        /// Get/Set method of the attribute4 field
        /// </summary> 
        public string Attribute4
        {
            get { return attribute4; }
            set { IsChanged = true; attribute4 = value; }
        }
        /// <summary>
        /// Get/Set method of the attribute5 field
        /// </summary> 
        public string Attribute5
        {
            get { return attribute5; }
            set { IsChanged = true; attribute5 = value; }
        }

        /// <summary>
        /// Get/Set method of the paymentModeOTP field
        /// </summary>
        public string PaymentModeOTP
        {
            get
            {
                return paymentModeOTP;
            }

            set
            {
                IsChanged = true;
                paymentModeOTP = value;
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
                lock(notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || paymentId < 0;
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
        /// Allows to accept the changes
        /// </summary>
        public void AcceptChanges()
        {
            log.LogMethodEntry();
            this.IsChanged = false;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns a string that represents the current TransactionPaymentsDTO
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            log.LogMethodEntry();
            StringBuilder returnValue = new StringBuilder("\n-----------------------TransactionPaymentsDTO-----------------------------\n");
            returnValue.Append(" PaymentId : " + PaymentId);
            returnValue.Append(" TransactionId : " + TransactionId);
            returnValue.Append(" PaymentModeId : " + PaymentModeId);
            returnValue.Append(" Amount : " + Amount);
            returnValue.Append(" CreditCardNumber : " + "CreditCardNumber");
            returnValue.Append(" NameOnCreditCard : " + "NameOnCreditCard");
            returnValue.Append(" CreditCardName : " + "CreditCardName");
            returnValue.Append(" CreditCardExpiry : " + "CreditCardExpiry");
            returnValue.Append(" CreditCardAuthorization : " + "CreditCardAuthorization");
            returnValue.Append(" CardId : " + CardId);
            returnValue.Append(" CardEntitlementType : " + CardEntitlementType);
            returnValue.Append(" CardCreditPlusId : " + CardCreditPlusId);
            returnValue.Append(" OrderId : " + OrderId);
            returnValue.Append(" Reference : " + Reference);
            returnValue.Append(" CCResponseId : " + CCResponseId);
            returnValue.Append(" Memo : " + Memo);
            returnValue.Append(" PaymentDate : " + PaymentDate);
            returnValue.Append(" ParentPaymentId : " + ParentPaymentId);
            returnValue.Append(" TenderedAmount : " + TenderedAmount);
            returnValue.Append(" TipAmount : " + TipAmount);
            returnValue.Append(" SplitId : " + SplitId);
            returnValue.Append(" PosMachine : " + PosMachine);
            returnValue.Append(" CurrencyCode : " + CurrencyCode);
            returnValue.Append(" CurrencyRate : " + CurrencyRate);
            returnValue.Append(" IsTaxable : " + IsTaxable);
            returnValue.Append(" Coupon Value : " + CouponValue);
            returnValue.Append("\n-------------------------------------------------------------\n");
            log.LogMethodExit();
            return returnValue.ToString();

        }
    }
}
