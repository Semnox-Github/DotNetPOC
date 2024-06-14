/********************************************************************************************
 * Project Name - TransactionSummaryView DTO
 * Description  - Data object of TransactionSummaryView
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.150.01    16-Feb-2023       Yashodhara C H     Created
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Discounts;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Semnox.Parafait.Transaction
{

    /// <summary>
    /// This is the Transaction data object class. This acts as data holder for the Transaction business object
    /// </summary>
    public class TransactionSummaryViewDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by transactionId field
            /// </summary>
            TRANSACTION_ID,
            /// <summary>
            /// Search by orderId field
            /// </summary>
            ORDER_ID,
            /// <summary>
            /// Search by site_Id field
            /// </summary>
            SITE_ID,
            ///<summary>
            ///Search by posMachineId
            ///</summary>
            POS_MACHINE_ID,
            ///<summary>
            ///Search by TransactionOTP
            ///</summary>
            TRANSACTION_OTP,
            ///<summary>
            ///Search by externalSystemReference
            ///</summary>
            EXTERNAL_SYSTEM_REFERENCE,
            ///<summary>
            ///Search by customerId
            ///</summary>
            CUSTOMER_ID,
            /// <summary>
            /// Search by pOSTypeId
            /// </summary>
            POS_TYPE_ID,
            ///<summary>
            ///Search by trxFromDate
            ///</summary>
            FROM_DATE,
            ///<summary>
            ///Search by trxToDate
            ///</summary>
            TO_DATE,
            ///<summary>
            ///Search by trxFromDate
            ///</summary>
            CREATED_FROM_DATE,
            ///<summary>
            ///Search by trxToDate
            ///</summary>
            CREATED_TO_DATE,
            ///<summary>
            ///Search by ORIGINAL_SYSTEM_REFERENCE
            ///</summary>
            ORIGINAL_SYSTEM_REFERENCE,
            ///<summary>
            ///Search by USER_ID
            ///</summary>
            USER_ID,
            ///<summary>
            ///Search by TRANSACTION_NUMBER
            ///</summary>
            TRANSACTION_NUMBER,
            ///<summary>
            ///Search by REMARKS
            ///</summary>
            REMARKS,
            ///<summary>
            ///Search by TRANSACTION_ID_LIST
            ///</summary>
            TRANSACTION_ID_LIST,
            ///<summary>
            ///Search by TRANSACTION_NUMBER_LIST
            ///</summary>
            TRANSACTION_NUMBER_LIST,
            ///<summary>
            ///Search by TRANSACTION_OTP_LIST
            ///</summary>
            TRANSACTION_OTP_LIST,
            ///<summary>
            ///Search by ORIGINAL_TRX_ID
            ///</summary>
            ORIGINAL_TRX_ID,
            /// <summary>
            /// Search by EMAIL_ID
            /// </summary>
            EMAIL_ID,
            /// <summary>
            /// Search by PHONE_NUMBER
            /// </summary>
            PHONE_NUMBER,
        }

        private int transactionId;
        private DateTime transactionDate;
        private decimal? transactionAmount;
        private decimal? transactionDiscountPercentage;
        private decimal? taxAmount;
        private decimal? transactionNetAmount;
        private string posMachine;
        private int userId;
        private int payment_mode_Id;
        private decimal? cashAmount;
        private decimal? creditCardAmount;
        private decimal? gameCardAmount;
        private string paymentReference;
        private int primaryCardId;
        private int orderId;
        private int pOSTypeId;
        private string transactionNumber;
        private string remarks;
        private int pOSMachineId;
        private decimal? otherPaymentModeAmount;
        private string status;
        private int transactionProfileId;
        private string tokenNumber;
        private string originalSystemReference;
        private int customerId;
        private string externalSystemReference;
        private int reprintCount;
        private int originalTransactionId;
        private int orderTypeGroupId;
        private string transactionOTP;
        private string customerIdentifier;
        private int printCount;
        private DateTime? saveStartTime;
        private DateTime? saveEndTime;
        private DateTime? printStartTime;
        private DateTime? printEndTime;
        private DateTime? transactionInitiatedTime;
        private string transactionIdentifier;
        private string guestName;
        private string tentNumber;
        private string phoneNumber;
        private string channel;
        private decimal? transactionDiscountTotal;
        private decimal? transactionPaymentTotal;
        private int guestCount;
        private bool? isNonChargeable;
        private string transactionPaymentStatus;
        private DateTime? transactionPaymentStatusChangeDate;
        private DateTime? transactionStatusChangeDate;
        private string emailId;
        private int sessionId;
        private int transactionReopenedCount;
        private DateTime? transactionClosedTime;
        private DateTime? transactionCancelledTime;
        private DateTime? transactionReopenedTime;
        private int approvedBy;
        private DateTime? lockedTime;
        private int transactionStatusId;
        private int transactionPaymentStatusId;
        private int lockedByPOSMachineId; 
        private string lockStatus;
        private int lockedBySiteId;
        private int lockedByUserId;
        private bool isActive;
        private bool? processedForLoyalty;
        private int transactionTypeId;
        private string transactionStatus;
        private string accountNumber;
        private string posCounter;
        private string paymentMode;
        private string profileName;
        private string customerName;
        private decimal? cashRatio;
        private decimal? creditCardRatio;
        private decimal? gameCardRatio;
        private decimal? otherModeRatio;
        private DateTime lastUpdateTime;
        private string lastUpdatedBy;
        private string createdBy;
        private DateTime creationDate;
        private string guid;
        private bool synchStatus;
        private int siteId;
        private int masterEntityId;

        /// <summary>
        /// 
        /// Default constructor
        /// </summary>
        public TransactionSummaryViewDTO()
        {
            transactionId = -1;
            primaryCardId = -1;
            customerId = -1;
            orderId = -1;
            pOSMachineId = -1;
            pOSTypeId = -1;
            originalTransactionId = -1;
            transactionProfileId = -1;
            userId = -1;
            payment_mode_Id = -1;
            masterEntityId = -1;
            orderTypeGroupId = -1;
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public TransactionSummaryViewDTO(int transactionId, DateTime transactionDate, decimal? transactionAmount,
                        decimal? transactionDiscountPercentage, decimal? taxAmount,
                        decimal? transactionNetAmount, string posMachine, int userId,
                        int payment_mode_Id, decimal? cashAmount, decimal? creditCardAmount,
                        decimal? gameCardAmount, string paymentReference, int primaryCardId,
                        int orderId, int pOSTypeId, string transactionNumber, string remarks,
                        int pOSMachineId, decimal? otherPaymentModeAmount, string status,
                        int transactionProfileId, string tokenNumber, string originalSystemReference, int customerId,
                        string externalSystemReference, int reprintCount, int originalTransactionId,
                        int orderTypeGroupId, string transactionOTP, string customerIdentifier, int printCount, DateTime? saveStartTime,
                        DateTime? saveEndTime, DateTime? printStartTime, DateTime? printEndTime, DateTime? transactionInitiatedTime,
                        string transactionIdentifier, string guestName, string tentNumber, 
                        string channel, decimal? transactionDiscountTotal, decimal? transactionPaymentTotal,
                        int guestCount, bool? isNonChargeable, string transactionPaymentStatus, DateTime? transactionPaymentStatusChangeDate,
                        DateTime? transactionStatusChangeDate, int sessionId, int transactionReopenedCount, DateTime? transactionClosedTime, 
                        DateTime? transactionCancelledTime, DateTime? transactionReopenedTime, int approvedBy, DateTime? lockedTime, 
                        int transactionStatusId, int transactionPaymentStatusId, int lockedByPOSMachineId, string lockStatus, int lockedBySiteId,
                        int lockedByUserId, bool isActive, bool? processedForLoyalty, int transactionTypeId, string transactionStatus, string emailId,
                        string phoneNumber, string accountNumber, string posCounter, string paymentMode, string profileName, string customerName,
                        decimal? cashRatio, decimal? creditCardRatio, decimal? gameCardRatio, decimal? otherModeRatio)
            : this()
        {
            log.LogMethodEntry(transactionId, transactionDate, transactionAmount, transactionDiscountPercentage, taxAmount, transactionNetAmount, posMachine, userId,
                              payment_mode_Id, cashAmount, creditCardAmount, gameCardAmount, paymentReference, primaryCardId, orderId, pOSTypeId, transactionNumber, remarks,
                               pOSMachineId, otherPaymentModeAmount, status, transactionProfileId, tokenNumber, originalSystemReference, customerId,
                               externalSystemReference, reprintCount, originalTransactionId, orderTypeGroupId, transactionOTP, customerIdentifier, printCount, saveStartTime, saveEndTime, printStartTime,
                               printEndTime, transactionInitiatedTime, transactionIdentifier, guestName, tentNumber, channel,
                               transactionDiscountTotal, transactionPaymentTotal, guestCount, isNonChargeable, transactionPaymentStatus, transactionPaymentStatusChangeDate,
                               transactionStatusChangeDate, sessionId, transactionReopenedCount, transactionClosedTime, transactionCancelledTime,
                               transactionReopenedTime, approvedBy, lockedTime, transactionStatusId, transactionPaymentStatusId, lockedByPOSMachineId, lockStatus, lockedBySiteId,
                               lockedByUserId, isActive, processedForLoyalty, transactionTypeId, transactionStatus, emailId, phoneNumber, accountNumber, posCounter, paymentMode, profileName, customerName,
                               cashRatio, creditCardRatio, gameCardRatio, otherModeRatio);
            this.transactionId = transactionId;
            this.transactionDate = transactionDate;
            this.transactionAmount = transactionAmount;
            this.transactionDiscountPercentage = transactionDiscountPercentage;
            this.taxAmount = taxAmount;
            this.transactionNetAmount = transactionNetAmount;
            this.posMachine = posMachine;
            this.userId = userId;
            this.payment_mode_Id = payment_mode_Id;
            this.cashAmount = cashAmount;
            this.creditCardAmount = creditCardAmount;
            this.gameCardAmount = gameCardAmount;
            this.paymentReference = paymentReference;
            this.primaryCardId = primaryCardId;
            this.orderId = orderId;
            this.pOSTypeId = pOSTypeId;
            this.transactionNumber = transactionNumber;
            this.remarks = remarks;
            this.pOSMachineId = pOSMachineId;
            this.otherPaymentModeAmount = otherPaymentModeAmount;
            this.status = status;
            this.transactionProfileId = transactionProfileId;
            this.tokenNumber = tokenNumber;
            this.originalSystemReference = originalSystemReference;
            this.customerId = customerId;
            this.externalSystemReference = externalSystemReference;
            this.reprintCount = reprintCount;
            this.originalTransactionId = originalTransactionId;
            this.orderTypeGroupId = orderTypeGroupId;
            this.transactionOTP = transactionOTP;
            this.customerIdentifier = customerIdentifier;
            this.printCount = printCount;
            this.saveStartTime = saveStartTime;
            this.saveEndTime = saveEndTime;
            this.printStartTime = printStartTime;
            this.printEndTime = printEndTime;
            this.transactionInitiatedTime = transactionInitiatedTime;
            this.transactionIdentifier = transactionIdentifier;
            this.guestName = guestName;
            this.tentNumber = tentNumber;
            this.channel = channel;
            this.transactionDiscountTotal = transactionDiscountTotal;
            this.transactionPaymentTotal = transactionPaymentTotal;
            this.guestCount = guestCount;
            this.isNonChargeable = isNonChargeable;
            this.transactionPaymentStatus = transactionPaymentStatus;
            this.transactionPaymentStatusChangeDate = transactionPaymentStatusChangeDate;
            this.transactionStatusChangeDate = transactionStatusChangeDate;
            this.sessionId = sessionId;
            this.transactionReopenedCount = transactionReopenedCount;
            this.transactionClosedTime = transactionClosedTime;
            this.transactionCancelledTime = transactionCancelledTime;
            this.transactionReopenedTime = transactionReopenedTime;
            this.approvedBy = approvedBy;
            this.lockedTime = lockedTime;
            this.transactionStatusId = transactionStatusId;
            this.transactionPaymentStatusId = transactionPaymentStatusId;
            this.lockedByPOSMachineId = lockedByPOSMachineId;
            this.lockStatus = lockStatus;
            this.lockedBySiteId = lockedBySiteId;
            this.lockedByUserId = lockedByUserId;
            this.isActive = isActive;
            this.processedForLoyalty = processedForLoyalty;
            this.transactionTypeId = transactionTypeId;
            this.transactionStatus = transactionStatus;
            this.emailId = emailId;
            this.phoneNumber = phoneNumber;
            this.accountNumber = accountNumber;
            this.posCounter = posCounter;
            this.paymentMode = paymentMode;
            this.profileName = profileName;
            this.customerName = customerName;
            this.cashRatio = cashRatio;
            this.creditCardRatio = creditCardRatio;
            this.gameCardRatio = gameCardRatio;
            this.otherModeRatio = otherModeRatio;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public TransactionSummaryViewDTO(int transactionId, DateTime transactionDate, decimal? transactionAmount,
                        decimal? transactionDiscountPercentage, decimal? taxAmount,
                        decimal? transactionNetAmount, string posMachine, int userId,
                        int payment_mode_Id, decimal? cashAmount, decimal? creditCardAmount,
                        decimal? gameCardAmount, string paymentReference, int primaryCardId,
                        int orderId, int pOSTypeId, string transactionNumber, string remarks,
                        int pOSMachineId, decimal? otherPaymentModeAmount, string status,
                        int transactionProfileId, string tokenNumber, string originalSystemReference, int customerId,
                        string externalSystemReference, int reprintCount, int originalTransactionId,
                        int orderTypeGroupId, string transactionOTP, string customerIdentifier, int printCount, DateTime? saveStartTime, 
                        DateTime? saveEndTime, DateTime? printStartTime, DateTime? printEndTime, DateTime? transactionInitiatedTime,
                        string transactionIdentifier, string guestName, string tentNumber,
                        string channel, decimal? transactionDiscountTotal,
                        decimal? transactionPaymentTotal, int guestCount, bool? isNonChargeable,
                        string transactionPaymentStatus, DateTime? transactionPaymentStatusChangeDate,
                        DateTime? transactionStatusChangeDate, int sessionId, int transactionReopenedCount,
                        DateTime? transactionClosedTime, DateTime? transactionCancelledTime,
                        DateTime? transactionReopenedTime, int approvedBy, DateTime? lockedTime, int transactionStatusId,
                        int transactionPaymentStatusId, int lockedByPOSMachineId, string lockStatus, int lockedBySiteId,
                        int lockedByUserId, bool isActive, bool? processedForLoyalty, int transactionTypeId, string transactionStatus, string emailId,
                        string phoneNumber, string accountNumber, string posCounter, string paymentMode, string profileName, string customerName,
                        decimal? cashRatio, decimal? creditCardRatio, decimal? gameCardRatio, decimal? otherModeRatio,
                        string createdBy, DateTime creationDate, DateTime lastUpdateTime, string lastUpdatedBy, string guid,
                        bool synchStatus, int siteId, int masterEntityId)
            : this(transactionId, transactionDate, transactionAmount, transactionDiscountPercentage, taxAmount, transactionNetAmount, posMachine, userId,
                              payment_mode_Id, cashAmount, creditCardAmount, gameCardAmount, paymentReference, primaryCardId, orderId, pOSTypeId, transactionNumber, remarks,
                               pOSMachineId, otherPaymentModeAmount, status, transactionProfileId, tokenNumber, originalSystemReference, customerId,
                               externalSystemReference, reprintCount, originalTransactionId, orderTypeGroupId, transactionOTP, customerIdentifier, printCount, saveStartTime, saveEndTime, printStartTime,
                               printEndTime, transactionInitiatedTime, transactionIdentifier, guestName, tentNumber, channel,
                               transactionDiscountTotal, transactionPaymentTotal, guestCount, isNonChargeable, transactionPaymentStatus, transactionPaymentStatusChangeDate,
                               transactionStatusChangeDate, sessionId, transactionReopenedCount, transactionClosedTime, transactionCancelledTime,
                               transactionReopenedTime, approvedBy, lockedTime, transactionStatusId, transactionPaymentStatusId, lockedByPOSMachineId, lockStatus, lockedBySiteId,
                               lockedByUserId, isActive, processedForLoyalty, transactionTypeId, transactionStatus, emailId, phoneNumber, accountNumber, posCounter, paymentMode, profileName, customerName,
                               cashRatio, creditCardRatio, gameCardRatio, otherModeRatio)
        {
            log.LogMethodEntry(transactionId, transactionDate, transactionAmount, transactionDiscountPercentage, taxAmount, transactionNetAmount, posMachine, userId,
                              payment_mode_Id, cashAmount, creditCardAmount, gameCardAmount, paymentReference, primaryCardId, orderId, pOSTypeId, transactionNumber, remarks,
                               pOSMachineId, otherPaymentModeAmount, status, transactionProfileId, tokenNumber, originalSystemReference, customerId,
                               externalSystemReference, reprintCount, originalTransactionId, orderTypeGroupId, transactionOTP, customerIdentifier, printCount, saveStartTime, saveEndTime, printStartTime,
                               printEndTime, transactionInitiatedTime, transactionIdentifier, guestName, tentNumber, channel,
                               transactionDiscountTotal, transactionPaymentTotal, guestCount, isNonChargeable, transactionPaymentStatus, transactionPaymentStatusChangeDate,
                               transactionStatusChangeDate, sessionId, transactionReopenedCount, transactionClosedTime, transactionCancelledTime,
                               transactionReopenedTime, approvedBy, lockedTime, transactionStatusId, transactionPaymentStatusId, lockedByPOSMachineId, lockStatus, lockedBySiteId,
                               lockedByUserId, isActive, processedForLoyalty, transactionTypeId, transactionStatus, emailId, phoneNumber, accountNumber, posCounter, paymentMode, profileName, customerName,
                               cashRatio, creditCardRatio, gameCardRatio, otherModeRatio, createdBy, creationDate, lastUpdateTime, lastUpdatedBy, guid, synchStatus, siteId, masterEntityId);
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdateTime = lastUpdateTime;
            this.lastUpdatedBy = lastUpdatedBy;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.siteId = siteId;
            this.masterEntityId = masterEntityId;

            log.LogMethodExit();
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        public TransactionSummaryViewDTO(TransactionSummaryViewDTO transactionSummaryViewDTO)
        {
            log.LogMethodEntry(transactionId, transactionDate, transactionAmount, transactionDiscountPercentage, taxAmount, transactionNetAmount, posMachine, userId,
                              payment_mode_Id, cashAmount, creditCardAmount, gameCardAmount, paymentReference, primaryCardId, orderId, pOSTypeId, transactionNumber, remarks,
                               pOSMachineId, otherPaymentModeAmount, status, transactionProfileId, tokenNumber, originalSystemReference, customerId,
                               externalSystemReference, reprintCount, originalTransactionId, orderTypeGroupId, transactionOTP, customerIdentifier, printCount, saveStartTime, saveEndTime, printStartTime,
                               printEndTime, transactionInitiatedTime, transactionIdentifier, guestName, tentNumber, channel,
                               transactionDiscountTotal, transactionPaymentTotal, guestCount, isNonChargeable, transactionPaymentStatus, transactionPaymentStatusChangeDate,
                               transactionStatusChangeDate, sessionId, transactionReopenedCount, transactionClosedTime, transactionCancelledTime,
                               transactionReopenedTime, approvedBy, lockedTime, transactionStatusId, transactionPaymentStatusId, lockedByPOSMachineId, lockStatus, lockedBySiteId,
                               lockedByUserId, isActive, processedForLoyalty, transactionTypeId, transactionStatus, emailId, phoneNumber, accountNumber, posCounter, paymentMode, profileName, customerName,
                               cashRatio, creditCardRatio, gameCardRatio, otherModeRatio, createdBy, creationDate, lastUpdateTime, lastUpdatedBy, guid, synchStatus, siteId, masterEntityId);
            this.transactionId = transactionSummaryViewDTO.transactionId;
            this.transactionDate = transactionSummaryViewDTO.transactionDate;
            this.transactionAmount = transactionSummaryViewDTO.transactionAmount;
            this.transactionDiscountPercentage = transactionSummaryViewDTO.transactionDiscountPercentage;
            this.taxAmount = transactionSummaryViewDTO.taxAmount;
            this.transactionNetAmount = transactionSummaryViewDTO.transactionNetAmount;
            this.posMachine = transactionSummaryViewDTO.posMachine;
            this.userId = transactionSummaryViewDTO.userId;
            this.payment_mode_Id = transactionSummaryViewDTO.payment_mode_Id;
            this.cashAmount = transactionSummaryViewDTO.cashAmount;
            this.creditCardAmount = transactionSummaryViewDTO.creditCardAmount;
            this.gameCardAmount = transactionSummaryViewDTO.gameCardAmount;
            this.paymentReference = transactionSummaryViewDTO.paymentReference;
            this.primaryCardId = transactionSummaryViewDTO.primaryCardId;
            this.orderId = transactionSummaryViewDTO.orderId;
            this.pOSTypeId = transactionSummaryViewDTO.pOSTypeId;
            this.transactionNumber = transactionSummaryViewDTO.transactionNumber;
            this.remarks = transactionSummaryViewDTO.remarks;
            this.pOSMachineId = transactionSummaryViewDTO.pOSMachineId;
            this.otherPaymentModeAmount = transactionSummaryViewDTO.otherPaymentModeAmount;
            this.status = transactionSummaryViewDTO.status;
            this.transactionProfileId = transactionSummaryViewDTO.transactionProfileId;
            this.tokenNumber = transactionSummaryViewDTO.tokenNumber;
            this.originalSystemReference = transactionSummaryViewDTO.originalSystemReference;
            this.customerId = transactionSummaryViewDTO.customerId;
            this.externalSystemReference = transactionSummaryViewDTO.externalSystemReference;
            this.reprintCount = transactionSummaryViewDTO.reprintCount;
            this.originalTransactionId = transactionSummaryViewDTO.originalTransactionId;
            this.orderTypeGroupId = transactionSummaryViewDTO.orderTypeGroupId;
            this.transactionOTP = transactionSummaryViewDTO.transactionOTP;
            this.customerIdentifier = transactionSummaryViewDTO.customerIdentifier;
            this.printCount = transactionSummaryViewDTO.printCount;
            this.saveStartTime = transactionSummaryViewDTO.saveStartTime;
            this.saveEndTime = transactionSummaryViewDTO.saveEndTime;
            this.printStartTime = transactionSummaryViewDTO.printStartTime;
            this.printEndTime = transactionSummaryViewDTO.printEndTime;
            this.transactionInitiatedTime = transactionSummaryViewDTO.transactionInitiatedTime;
            this.transactionIdentifier = transactionSummaryViewDTO.transactionIdentifier;
            this.guestName = transactionSummaryViewDTO.guestName;
            this.tentNumber = transactionSummaryViewDTO.tentNumber;
            this.channel = transactionSummaryViewDTO.channel;
            this.transactionDiscountTotal = transactionSummaryViewDTO.transactionDiscountTotal;
            this.transactionPaymentTotal = transactionSummaryViewDTO.transactionPaymentTotal;
            this.guestCount = transactionSummaryViewDTO.guestCount;
            this.isNonChargeable = transactionSummaryViewDTO.isNonChargeable;
            this.transactionPaymentStatus = transactionSummaryViewDTO.transactionPaymentStatus;
            this.transactionPaymentStatusChangeDate = transactionSummaryViewDTO.transactionPaymentStatusChangeDate;
            this.transactionStatusChangeDate = transactionSummaryViewDTO.transactionStatusChangeDate;
            this.sessionId = transactionSummaryViewDTO.sessionId;
            this.transactionReopenedCount = transactionSummaryViewDTO.transactionReopenedCount;
            this.transactionClosedTime = transactionSummaryViewDTO.transactionClosedTime;
            this.transactionCancelledTime = transactionSummaryViewDTO.transactionCancelledTime;
            this.transactionReopenedTime = transactionSummaryViewDTO.transactionReopenedTime;
            this.approvedBy = transactionSummaryViewDTO.approvedBy;
            this.lockedTime = transactionSummaryViewDTO.lockedTime;
            this.transactionStatusId = transactionSummaryViewDTO.transactionStatusId;
            this.transactionPaymentStatusId = transactionSummaryViewDTO.transactionPaymentStatusId;
            this.lockedByPOSMachineId = transactionSummaryViewDTO.lockedByPOSMachineId;
            this.lockStatus = transactionSummaryViewDTO.lockStatus;
            this.lockedBySiteId = transactionSummaryViewDTO.lockedBySiteId;
            this.lockedByUserId = transactionSummaryViewDTO.lockedByUserId;
            this.isActive = transactionSummaryViewDTO.isActive;
            this.processedForLoyalty = transactionSummaryViewDTO.processedForLoyalty;
            this.transactionTypeId = transactionSummaryViewDTO.transactionTypeId;
            this.transactionStatus = transactionSummaryViewDTO.transactionStatus;
            this.emailId = transactionSummaryViewDTO.emailId;
            this.phoneNumber = transactionSummaryViewDTO.phoneNumber;
            this.accountNumber = transactionSummaryViewDTO.accountNumber;
            this.posCounter = transactionSummaryViewDTO.posCounter;
            this.paymentMode = transactionSummaryViewDTO.paymentMode;
            this.profileName = transactionSummaryViewDTO.profileName;
            this.customerName = transactionSummaryViewDTO.customerName;
            this.cashRatio = transactionSummaryViewDTO.cashRatio;
            this.creditCardRatio = transactionSummaryViewDTO.creditCardRatio;
            this.gameCardRatio = transactionSummaryViewDTO.gameCardRatio;
            this.otherModeRatio = transactionSummaryViewDTO.otherModeRatio;
            this.createdBy = transactionSummaryViewDTO.createdBy;
            this.creationDate = transactionSummaryViewDTO.creationDate;
            this.lastUpdateTime = transactionSummaryViewDTO.lastUpdateTime;
            this.lastUpdatedBy = transactionSummaryViewDTO.lastUpdatedBy;
            this.guid = transactionSummaryViewDTO.guid;
            this.synchStatus = transactionSummaryViewDTO.synchStatus;
            this.siteId = transactionSummaryViewDTO.siteId;
            this.masterEntityId = transactionSummaryViewDTO.masterEntityId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the TransactionId field
        /// </summary>
        public int TransactionId
        {
            get
            {
                return transactionId;
            }

            set
            {
                transactionId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the TransactionDate field
        /// </summary>
        public DateTime TransactionDate
        {
            get
            {
                return transactionDate;
            }

            set
            {
                transactionDate = value;
            }
        }

        /// <summary>
        /// Get/Set method of the TransactionAmount field
        /// </summary>
        public decimal? TransactionAmount
        {
            get
            {
                return transactionAmount;
            }

            set
            {
                transactionAmount = value;
            }
        }

        /// <summary>
        /// Get/Set method of the TransactionDiscountPercentage field
        /// </summary>
        public decimal? TransactionDiscountPercentage
        {
            get
            {
                return transactionDiscountPercentage; 
            }

            set
            {
                transactionDiscountPercentage = value;
            }
        }

        /// <summary>
        /// Get/Set method of the TaxAmount field
        /// </summary>
        public decimal? TaxAmount
        {
            get
            {
                return taxAmount;
            }

            set
            {
                taxAmount = value;
            }
        }

        /// <summary>
        /// Get/Set method of the TransactionNetAmount field
        /// </summary>
        public decimal? TransactionNetAmount
        {
            get
            {
                return transactionNetAmount;
            }

            set
            {
                transactionNetAmount = value;
            }
        }

        /// <summary>
        /// Get/Set method of the PosMachine field
        /// </summary>
        public string PosMachine
        {
            get
            {
                return posMachine;
            }

            set
            {
                posMachine = value;
            }
        }

        /// <summary>
        /// Get/Set method of the UserId field
        /// </summary>
        public int UserId
        {
            get
            {
                return userId;
            }

            set
            {
                userId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Payment_mode_Id field
        /// </summary>
        public int Payment_mode_Id
        {
            get
            {
                return payment_mode_Id;
            }

            set
            {
                payment_mode_Id = value;
            }
        }

        /// <summary>
        /// Get/Set method of the CashAmount field
        /// </summary>
        public decimal? CashAmount
        {
            get
            {
                return cashAmount;
            }

            set
            {
                cashAmount = value;
            }
        }

        /// <summary>
        /// Get/Set method of the CreditCardAmount field
        /// </summary>
        public decimal? CreditCardAmount
        {
            get
            {
                return creditCardAmount;
            }

            set
            {
                creditCardAmount = value;
            }
        }

        /// <summary>
        /// Get/Set method of the GameCardAmount field
        /// </summary>
        public decimal? GameCardAmount
        {
            get
            {
                return gameCardAmount;
            }

            set
            {
                gameCardAmount = value;
            }
        }

        /// <summary>
        /// Get/Set method of the PaymentReference field
        /// </summary>
        public string PaymentReference
        {
            get
            {
                return paymentReference;
            }

            set
            {
                paymentReference = value;
            }
        }

        /// <summary>
        /// Get/Set method of the PrimaryCardId field
        /// </summary>
        public int PrimaryCardId
        {
            get
            {
                return primaryCardId;
            }

            set
            {
                primaryCardId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the OrderId field
        /// </summary>
        public int OrderId
        {
            get
            {
                return orderId;
            }

            set
            {
                orderId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the POSTypeId field
        /// </summary>
        public int POSTypeId
        {
            get
            {
                return pOSTypeId;
            }

            set
            {
                pOSTypeId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the TransactionNumber field
        /// </summary>
        public string TransactionNumber
        {
            get
            {
                return transactionNumber;
            }

            set
            {
                transactionNumber = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Remarks field
        /// </summary>
        public string Remarks
        {
            get
            {
                return remarks;
            }

            set
            {
                remarks = value;
            }
        }

        /// <summary>
        /// Get/Set method of the POSMachineId field
        /// </summary>
        public int POSMachineId
        {
            get
            {
                return pOSMachineId;
            }

            set
            {
                pOSMachineId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the OtherPaymentModeAmount field
        /// </summary>
        public decimal? OtherPaymentModeAmount
        {
            get
            {
                return otherPaymentModeAmount;
            }

            set
            {
                otherPaymentModeAmount = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Status field
        /// </summary>
        public string Status
        {
            get
            {
                return status;
            }

            set
            {
                status = value;
            }
        }

        /// <summary>
        /// Get/Set method of the TransactionProfileId field
        /// </summary>
        public int TransactionProfileId
        {
            get
            {
                return transactionProfileId;
            }

            set
            {
                transactionProfileId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the TokenNumber field
        /// </summary>
        public string TokenNumber
        {
            get
            {
                return tokenNumber;
            }

            set
            {
                tokenNumber = value;
            }
        }

        /// <summary>
        /// Get/Set method of the OriginalSystemReference field
        /// </summary>
        public string OriginalSystemReference
        {
            get
            {
                return originalSystemReference;
            }

            set
            {
                originalSystemReference = value;
            }
        }

        /// <summary>
        /// Get/Set method of the CustomerId field
        /// </summary>
        public int CustomerId
        {
            get
            {
                return customerId;
            }

            set
            {
                customerId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the ExternalSystemReference field
        /// </summary>
        public string ExternalSystemReference
        {
            get
            {
                return externalSystemReference;
            }

            set
            {
                externalSystemReference = value;
            }
        }

        /// <summary>
        /// Get/Set method of the ReprintCount field
        /// </summary>
        public int ReprintCount
        {
            get
            {
                return reprintCount;
            }

            set
            {
                reprintCount = value;
            }
        }

        /// <summary>
        /// Get/Set method of the OriginalTransactionId field
        /// </summary>
        public int OriginalTransactionId
        {
            get
            {
                return originalTransactionId;
            }

            set
            {
                originalTransactionId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the orderTypeGroupId field
        /// </summary>
        public int OrderTypeGroupId
        {
            get
            {
                return orderTypeGroupId;
            }

            set
            {
                orderTypeGroupId = value;
            }
        }


        /// <summary>
        /// Get/Set method of the TransactionOTP field
        /// </summary>
        public string TransactionOTP
        {
            get
            {
                return transactionOTP;
            }

            set
            {
                transactionOTP = value;
            }
        }

        /// <summary>
        /// Get method of the CustomerIdentifier field 
        /// </summary>
        public string CustomerIdentifier
        {
            get
            {
                    return customerIdentifier;
                }
            set
            {
                    customerIdentifier = value;
            }
        }

        /// <summary>
        /// Get/Set method of the PrintCount field
        /// </summary>
        public int PrintCount
        {
            get
            {
                return printCount;
            }
            set
            {
                printCount = value;
            }
        }

        /// <summary>
        /// Get/Set method of the saveStartTime field
        /// </summary>
        public DateTime? SaveStartTime
        {
            get
            {
                return saveStartTime;
            }
            set
            {
                saveStartTime = value;
            }
        }

        /// <summary>
        /// Get/Set method of the SaveEndTime field
        /// </summary>
        public DateTime? SaveEndTime
        {
            get
            {
                return saveEndTime;
            }
            set
            {
                saveEndTime = value;
            }
        }

        /// <summary>
        /// Get/Set method of the PrintStartTime field
        /// </summary>
        public DateTime? PrintStartTime
        {
            get
            {
                return printStartTime;
            }
            set
            {
                printStartTime = value;
            }
        }

        /// <summary>
        /// Get/Set method of the PrintEndTime field
        /// </summary>
        public DateTime? PrintEndTime
        {
            get
            {
                return printEndTime;
            }
            set
            {
                printEndTime = value;
            }
        }

        /// <summary>
        /// Get/Set method of the TransactionInitiatedTime field
        /// </summary>
        public DateTime? TransactionInitiatedTime
        {
            get
            {
                return transactionInitiatedTime;
            }
            set
            {
                transactionInitiatedTime = value;
            }
        }

        /// <summary>
        /// Get/Set method of the TransactionIdentifier field
        /// </summary>
        public string TransactionIdentifier
        {
            get
            {
                return transactionIdentifier;
            }
            set
            {
                transactionIdentifier = value;
            }
        }

        /// <summary>
        /// Get/Set method of the GuestName field
        /// </summary>
        public string GuestName
        {
            get
            {
                return guestName;
            }
            set
            {
                guestName = value;
            }
        }

        /// <summary>
        /// Get/Set method of the TentNumber field
        /// </summary>
        public string TentNumber
        {
            get
            {
                return tentNumber;
            }
            set
            {
                tentNumber = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Channel field
        /// </summary>
        public string Channel
        {
            get
            {
                return channel;
            }
            set
            {
                channel = value;
            }
        }

        /// <summary>
        /// Get/Set method of the TransactionDiscountTotal field
        /// </summary>
        public decimal? TransactionDiscountTotal
        {
            get
            {
                return transactionDiscountTotal;
            }
            set
            {
                transactionDiscountTotal = value;
            }
        }

        /// <summary>
        /// Get/Set method of the TransactionPaymentTotal field
        /// </summary>
        public decimal? TransactionPaymentTotal
        {
            get
            {
                return transactionPaymentTotal;
            }
            set
            {
                transactionPaymentTotal = value;
            }
        }

        /// <summary>
        /// Get/Set method of the GuestCount field
        /// </summary>
        public int GuestCount
        {
            get
            {
                return guestCount;
            }
            set
            {
                guestCount = value;
            }
        }

        /// <summary>
        /// Get/Set method of the IsNonChargeable field
        /// </summary>
        public bool? IsNonChargeable
        {
            get
            {
                return isNonChargeable;
            }
            set
            {
                isNonChargeable = value;
            }
        }

        /// <summary>
        /// Get/Set method of the TransactionPaymentStatus field
        /// </summary>
        public string TransactionPaymentStatus
        {
            get
            {
                return transactionPaymentStatus;
            }
            set
            {
                transactionPaymentStatus = value;
            }
        }

        /// <summary>
        /// Get/Set method of the TransactionPaymentStatusChangeDate field
        /// </summary>
        public DateTime? TransactionPaymentStatusChangeDate
        {
            get
            {
                return transactionPaymentStatusChangeDate;
            }
            set
            {
                transactionPaymentStatusChangeDate = value;
            }
        }

        /// <summary>
        /// Get/Set method of the TransactionStatusChangeDate field
        /// </summary>
        public DateTime? TransactionStatusChangeDate
        {
            get
            {
                return transactionStatusChangeDate;
            }
            set
            {
                transactionStatusChangeDate = value;
            }
        }

        /// <summary>
        /// Get/Set method of the SessionId field
        /// </summary>
        public int SessionId
        {
            get
            {
                return sessionId;
            }
            set
            {
                sessionId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the TransactionReopenedCount field
        /// </summary>
        public int TransactionReopenedCount
        {
            get
            {
                return transactionReopenedCount;
            }
            set
            {
                transactionReopenedCount = value;
            }
        }

        /// <summary>
        /// Get/Set method of the TransactionClosedTime field
        /// </summary>
        public DateTime? TransactionClosedTime
        {
            get
            {
                return transactionClosedTime;
            }
            set
            {
                transactionClosedTime = value;
            }
        }

        /// <summary>
        /// Get/Set method of the TransactionCancelledTime field
        /// </summary>
        public DateTime? TransactionCancelledTime
        {
            get
            {
                return transactionCancelledTime;
            }
            set
            {
                transactionCancelledTime = value;
            }
        }

        /// <summary>
        /// Get/Set method of the TransactionReopenedTime field
        /// </summary>
        public DateTime? TransactionReopenedTime
        {
            get
            {
                return transactionReopenedTime;
            }
            set
            {
                transactionReopenedTime = value;
            }
        }

        /// <summary>
        /// Get/Set method of the ApprovedBy field
        /// </summary>
        public int ApprovedBy
        {
            get
            {
                return approvedBy;
            }
            set
            {
                approvedBy = value;
            }
        }

        /// <summary>
        /// Get/Set method of the LockedTime field
        /// </summary>
        public DateTime? LockedTime
        {
            get
            {
                return lockedTime;
            }
            set
            {
                lockedTime = value;
            }
        }

        /// <summary>
        /// Get/Set method of the TransactionStatusId field
        /// </summary>
        public int TransactionStatusId
        {
            get
            {
                return transactionStatusId;
            }
            set
            {
                transactionStatusId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the TransactionPaymentStatusId field
        /// </summary>
        public int TransactionPaymentStatusId
        {
            get
            {
                return transactionPaymentStatusId;
            }
            set
            {
                transactionPaymentStatusId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the LockedByPOSMachineId field
        /// </summary>
        public int LockedByPOSMachineId
        {
            get
            {
                return lockedByPOSMachineId;
            }
            set
            {
                lockedByPOSMachineId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the LockStatus field
        /// </summary>
        public string LockStatus
        {
            get
            {
                return lockStatus;
            }
            set
            {
                lockStatus = value;
            }
        }

        /// <summary>
        /// Get/Set method of the LockedBySiteId field
        /// </summary>
        public int LockedBySiteId
        {
            get
            {
                return lockedBySiteId;
            }
            set
            {
                lockedBySiteId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the LockedByUserId field
        /// </summary>
        public int LockedByUserId
        {
            get
            {
                return lockedByUserId;
            }
            set
            {
                lockedByUserId = value;
            }
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
                isActive = value;
            }
        }

        /// <summary>
        /// Get/Set method of the ProcessedForLoyalty field
        /// </summary>
        public bool? ProcessedForLoyalty
        {
            get
            {
                return processedForLoyalty;
            }
            set
            {
                processedForLoyalty = value;
            }
        }

        /// <summary>
        /// Get/Set method of the TransactiontypeId field
        /// </summary>
        public int TransactiontypeId
        {
            get
            {
                return transactionTypeId;
            }
            set
            {
                transactionTypeId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the TransactionStatus field
        /// </summary>
        public string TransactionStatus
        {
            get
            {
                return transactionStatus;
            }
            set
            {
                transactionStatus = value;
            }
        }

        /// <summary>
        /// Get/Set method of the EmailId field
        /// </summary>
        public string EmailId
        {
            get
            {
                return emailId;
            }
            set
            {
                emailId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the PhoneNumber field
        /// </summary>
        public string PhoneNumber
        {
            get
            {
                return phoneNumber;
            }
            set
            {
                phoneNumber = value;
            }
        }

        /// <summary>
        /// Get/Set method of the AccountNumber field
        /// </summary>
        public string AccountNumber
        {
            get
            {
                return accountNumber;
            }
            set
            {
                accountNumber = value;
            }
        }

        /// <summary>
        /// Get/Set method of the PosCounter field
        /// </summary>
        public string PosCounter
        {
            get
            {
                return posCounter;
            }
            set
            {
                posCounter = value;
            }
        }

        /// <summary>
        /// Get/Set method of the PaymentMode field
        /// </summary>
        public string PaymentMode
        {
            get
            {
                return paymentMode;
            }
            set
            {
                paymentMode = value;
            }
        }

        /// <summary>
        /// Get/Set method of the ProfileName field
        /// </summary>
        public string ProfileName
        {
            get
            {
                return profileName;
            }
            set
            {
                profileName = value;
            }
        }

        /// <summary>
        /// Get method of the CustomerName field
        /// </summary>
        public string CustomerName
        {
            get
            {
                return customerName;
            }
            set
            {
                customerName = value;
            }
        }

        /// <summary>
        /// Get method of the customerName field
        /// </summary>
        public Decimal? CashRatio
        {
            get
            {
                return cashRatio;
            }
            set
            {
                cashRatio = value;
            }
        }

        /// <summary>
        /// Get method of the customerName field
        /// </summary>
        public Decimal? CreditCardRatio
        {
            get
            {
                return creditCardRatio;
            }
            set
            {
                creditCardRatio = value;
            }
        }

        /// <summary>
        /// Get method of the customerName field
        /// </summary>
        public Decimal? GameCardRatio
        {
            get
            {
                return gameCardRatio;
            }
            set
            {
                gameCardRatio = value;
            }
        }

        /// <summary>
        /// Get method of the customerName field
        /// </summary>
        public Decimal? OtherModeRatio
        {
            get
            {
                return otherModeRatio;
            }
            set
            {
                otherModeRatio = value;
            }
        }

        /// <summary>
        /// Get method of the LastUpdateTime field
        /// </summary>
        public DateTime LastUpdateTime
        {
            get
            {
                return lastUpdateTime;
            }
            set
            {
                lastUpdateTime = value;
            }
        }

        /// <summary>
        /// Get method of the LastUpdatedBy field
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
        /// Get/Set method of the creationDate field
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
        /// Get method of the Guid field
        /// </summary>
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
        /// Get method of the SynchStatus field
        /// </summary>
        public bool SynchStatus
        {
            get
            {
                return synchStatus;
            }
        }

        /// <summary>
        /// Get method of the SiteId field
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
        /// Get method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId
        {
            get
            {
                return masterEntityId;
            }
            set
            {
                masterEntityId = value;
            }
        }
    }
}
