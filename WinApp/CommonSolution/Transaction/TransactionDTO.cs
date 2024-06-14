/********************************************************************************************
 * Project Name - Transaction DTO
 * Description  - Data object of Transaction
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *1.00        31-Jul-2017   Lakshminarayana          Created 
 *2.4.0       28-Sep-2018   Guru S A                 Modified for Online Transaction in Kiosk changes
 *2.70.0      01-Aug-2019   Nitin Pai                Added List<TransactionLineDTO>
 *2.70.2.0    30-Oct-2019   Nitin Pai                Changes for new transaction controller
 *                                                   adding new properties to home receipts and tickets
 *2.70.2      26-Nov-2019   Lakshminarayana          Virtual store enhancement
 *2.70.2      05-Jan-2019   Akshay                   ClubSpeed enhancement changes - Added searchParameters
 *2.70.2      04-Feb-2020   Nitin Pai                Guest App phase 2 changes - Added discount information and fixed save (recursive) logic
 *2.80.0      20-Mar-2020   Akshay G                 Added searchParameter - TRANSACTION_ID_LIST, HAS_PRODUCT_ID_LIST
 *2.80.0      04-Jun-2020   Nitin Pai                Moved from iTransaction to Transaction Project
 *2.90.0      20-Aug-2020   Girish Kundar            Modified: Added Payment mode field to DTO to implement excel download for View transaction report
 *2.110.0     29-Oct-2019   Girish Kundar            Modified: Center edge changes                                                      
 *2.110.0     14-Dec-2020   Dakshakh Raj             Modified: for Peru Invoice Enhancement
 *2.140.0     27-Jun-2021   Fiona Lishal             Modified for Delivery Order enhancements for F&B and Urban Piper
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Device.PaymentGateway;
using Semnox.Parafait.Discounts;
using Semnox.Parafait.POS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Semnox.Parafait.Transaction
{

    /// <summary>
    /// This is the Transaction data object class. This acts as data holder for the Transaction business object
    /// </summary>
    public class TransactionDTO
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
            /// Search by status field
            /// </summary>
            STATUS,
            /// <summary>
            /// Search by site_Id field
            /// </summary>
            SITE_ID,
            ///<summary>
            ///Search by posMachineId
            ///</summary>
            POS_MACHINE_ID,
            ///<summary>
            ///Search by posName
            ///</summary>
            POS_NAME,
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
            ///Search by customerGuidId
            ///</summary>
            CUSTOMER_GUID_ID,
            ///<summary>
            ///Search by trxFromDate
            ///</summary>
            TRANSACTION_FROM_DATE,
            ///<summary>
            ///Search by trxToDate
            ///</summary>
            TRANSACTION_TO_DATE,
            ///<summary>
            ///Search by LAST_UPDATE_FROM_TIME
            ///</summary>
            LAST_UPDATE_FROM_TIME,
            ///<summary>
            ///Search by LAST_UPDATE_TO_TIME
            ///</summary>
            LAST_UPDATE_TO_TIME,
            ///<summary>
            ///Search by CUSTOMER_SIGNED_WAIVER_ID
            ///</summary>
            CUSTOMER_SIGNED_WAIVER_ID,
            /// <summary>
            /// Search by STATUS_NOT_IN field
            /// </summary>
            STATUS_NOT_IN,
            ///<summary>
            ///Search by ORIGINAL_SYSTEM_REFERENCE
            ///</summary>
            ORIGINAL_SYSTEM_REFERENCE,
            ///<summary>
            ///Search by ONLINE_ONLY
            ///</summary>
            ONLINE_ONLY,
            ///<summary>
            ///Search by IS_RESERVATION_TRANSACTION
            ///</summary>
            IS_RESERVATION_TRANSACTION,
            ///<summary>
            ///Search by HAS_ATTRACTION_BOOKINGS
            ///</summary>
            HAS_ATTRACTION_BOOKINGS,
            ///<summary>
            ///Search by HAS_EXTERNAL_SYSTEM_REFERENCE
            ///</summary>
            HAS_EXTERNAL_SYSTEM_REFERENCE,
            ///<summary>
            ///Search by HAS_PRODUCT_TYPE
            ///</summary>
            HAS_PRODUCT_TYPE,
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
            ///Search by HAS_PRODUCT_ID_LIST
            ///</summary>
            HAS_PRODUCT_ID_LIST,
            ///<summary>
            ///Search by Guid
            ///</summary>
            GUID,
            ///<summary>
            ///Search by CUSTOMER_IDENTIFIER
            ///</summary>
            CUSTOMER_IDENTIFIER,
            ///<summary>
            ///Search by ORIGINAL_TRX_ID
            ///</summary>
            ORIGINAL_TRX_ID,
            ///<summary>
            ///Search by LINKED_BILL_CYCLE_TRX_FOR_SUBSCRIPTION_HEADER_ID
            ///</summary>
            LINKED_BILL_CYCLE_TRX_FOR_SUBSCRIPTION_HEADER_ID,
            ///<summary>
            ///Search by POS_OVERRIDE_OPTION_NAME_LIST
            ///</summary>
            POS_OVERRIDE_OPTION_NAME_LIST,
            ///<summary>
            ///Search by NEEDS_ORDER_DISPENSING
            ///</summary>
            NEEDS_ORDER_DISPENSING,
            /// <summary>
            /// Search by PAYMENT_MODE_ID
            /// </summary>
            TRX_PAYMENT_MODE_ID,
            /// <summary>
            /// Search by IS POS MACHINE INCLUDED IN BSP
            /// </summary>
            IS_POS_MACHINE_INCLUDED_IN_BSP,
            /// <summary>
            /// Search by TRX NOT IN EXSYS LOG
            /// </summary>
            TRX_NOT_IN_EXSYS_LOG,
            /// <summary>
            /// Search by AMOUNT GREATER THAN ZERO
            /// </summary>
            AMOUNT_GREATER_THAN_ZERO
        }

        public static SearchByParameters SearchByParametersFromString(string searchParameter)
        {
            switch (searchParameter)
            {
                case "TRANSACTION_ID":
                    return SearchByParameters.TRANSACTION_ID;
                case "ORDER_ID":
                    return SearchByParameters.ORDER_ID;
                case "STATUS":
                    return SearchByParameters.STATUS;
                case "SITE_ID":
                    return SearchByParameters.SITE_ID;
                case "POS_MACHINE_ID":
                    return SearchByParameters.POS_MACHINE_ID;
                case "POS_NAME":
                    return SearchByParameters.POS_NAME;
                case "TRANSACTION_OTP":
                    return SearchByParameters.TRANSACTION_OTP;
                case "EXTERNAL_SYSTEM_REFERENCE":
                    return SearchByParameters.EXTERNAL_SYSTEM_REFERENCE;
                case "CUSTOMER_ID":
                    return SearchByParameters.CUSTOMER_ID;
                case "CUSTOMER_GUID_ID":
                    return SearchByParameters.CUSTOMER_GUID_ID;
                case "TRANSACTION_FROM_DATE":
                    return SearchByParameters.TRANSACTION_FROM_DATE;
                case "TRANSACTION_TO_DATE":
                    return SearchByParameters.TRANSACTION_TO_DATE;
                case "LAST_UPDATE_FROM_TIME":
                    return SearchByParameters.LAST_UPDATE_FROM_TIME;
                case "LAST_UPDATE_TO_TIME":
                    return SearchByParameters.LAST_UPDATE_TO_TIME;
                case "CUSTOMER_SIGNED_WAIVER_ID":
                    return SearchByParameters.CUSTOMER_SIGNED_WAIVER_ID;
                case "STATUS_NOT_IN":
                    return SearchByParameters.STATUS_NOT_IN;
                case "ORIGINAL_SYSTEM_REFERENCE":
                    return SearchByParameters.ORIGINAL_SYSTEM_REFERENCE;
                case "ONLINE_ONLY":
                    return SearchByParameters.ONLINE_ONLY;
                case "IS_RESERVATION_TRANSACTION":
                    return SearchByParameters.IS_RESERVATION_TRANSACTION;
                case "HAS_ATTRACTION_BOOKINGS":
                    return SearchByParameters.HAS_ATTRACTION_BOOKINGS;
                case "HAS_EXTERNAL_SYSTEM_REFERENCE":
                    return SearchByParameters.HAS_EXTERNAL_SYSTEM_REFERENCE;
                case "HAS_PRODUCT_TYPE":
                    return SearchByParameters.HAS_PRODUCT_TYPE;
                case "USER_ID":
                    return SearchByParameters.USER_ID;
                case "TRANSACTION_NUMBER":
                    return SearchByParameters.TRANSACTION_NUMBER;
                case "REMARKS":
                    return SearchByParameters.REMARKS;
                case "TRANSACTION_ID_LIST":
                    return SearchByParameters.TRANSACTION_ID_LIST;
                case "HAS_PRODUCT_ID_LIST":
                    return SearchByParameters.HAS_PRODUCT_ID_LIST;
                case "GUID":
                    return SearchByParameters.GUID;
                case "CUSTOMER_IDENTIFIER":
                    return SearchByParameters.CUSTOMER_IDENTIFIER;
                default:
                    throw new ValidationException("Invalid transaction search parameter");
            }
        }

        private int transactionId;
        private DateTime transactionDate;
        private decimal? transactionAmount;
        private decimal? transactionDiscountPercentage;
        private decimal? transactionDiscountAmount;
        private decimal? taxAmount;
        private decimal? transactionNetAmount;
        private string posMachine;
        private int userId;
        private int paymentMode;
        private decimal? cashAmount;
        private decimal? creditCardAmount;
        private decimal? gameCardAmount;
        private string paymentReference;
        private int primaryCardId;
        private int orderId;
        private int pOSTypeId;
        private string transactionNumber;
        private string transactionOTP;
        private string remarks;
        private int pOSMachineId;
        private decimal? otherPaymentModeAmount;
        private string status;
        private int transactionProfileId;
        private DateTime lastUpdateTime;
        private string lastUpdatedBy;
        private string tokenNumber;
        private string originalSystemReference;
        private int customerId;
        private string externalSystemReference;
        private int reprintCount;
        private int originalTransactionId;
        private string tableNumber;
        private decimal? paid;
        private string userName;
        private string customerName;
        private int createdBy;
        private DateTime creationDate;
        private int orderTypeGroupId;
        private string guid;
        private bool synchStatus;
        private int siteId;
        private int masterEntityId;
        private string tickets;
        private string receipt;
        private string ticketsHTML;
        private string receiptHTML;
        private string primaryCard;
        private ReceiptDTO receiptDTO;
        private List<TicketDTO> ticketDTOList;
        private List<TicketPrinterMapDTO> ticketPrinterMapDTOList;
        private List<TransactionLineDTO> transactionLinesDTOList;
        private List<TransactionPaymentsDTO> trxPaymentsDTOList;
        List<DiscountsSummaryDTO> discountsSummaryDTOList;
        List<DiscountApplicationHistoryDTO> discountApplicationHistoryDTOList;
        private List<TransactionTaxLineDTO> transactionTaxLinesDTOList;
        private List<TrxPOSPrinterOverrideRulesDTO> trxPOSPrinterOverrideRulesDTOList;
        private string visitDate;
        private bool applyVisitDate;

        private string paymentModeName; //added for excel build for report

        private bool commitTransaction;
        private bool saveTransaction;
        private bool closeTransaction;
        private bool applyOffset;
        private bool paymentProcessingCompleted;
        private bool reverseTransaction;
        private string customerIdentifier;
        //private string paymentPageLink;

        private TransactionOrderDispensingDTO transctionOrderDispensingDTO;    

        


        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        

        /// <summary>
        /// 
        /// Default constructor
        /// </summary>
        public TransactionDTO()
        {
            transactionId = -1;
            primaryCardId = -1;
            createdBy = -1;
            customerId = -1;
            orderId = -1;
            pOSMachineId = -1;
            pOSTypeId = -1;
            originalTransactionId = -1;
            transactionProfileId = -1;
            userId = -1;
            paymentMode = -1;
            masterEntityId = -1;
            orderTypeGroupId = -1;
            transactionLinesDTOList = new List<TransactionLineDTO>();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public TransactionDTO(int transactionId, DateTime transactionDate, decimal? transactionAmount,
                              decimal? transactionDiscountPercentage, decimal? taxAmount,
                              decimal? transactionNetAmount, string posMachine, int userId,
                              int paymentMode, decimal? cashAmount, decimal? creditCardAmount,
                              decimal? gameCardAmount, string paymentReference, int primaryCardId,
                              int orderId, int pOSTypeId, string transactionNumber, string remarks,
                              int pOSMachineId, decimal? otherPaymentModeAmount, string status,
                              int transactionProfileId, DateTime lastUpdateTime, string lastUpdatedBy,
                              string tokenNumber, string originalSystemReference, int customerId,
                              string externalSystemReference, int reprintCount, int originalTransactionId,
                              int orderTypeGroupId, string tableNumber, decimal? paid, string userName, int createdBy, DateTime creationDate, string guid,
                              bool synchStatus, int siteId, int masterEntityId, string transactionOTP, string primaryCard, string customerName, string customerIdentifier)
        {
            log.LogMethodEntry(transactionId, transactionDate, transactionAmount, transactionDiscountPercentage, taxAmount, transactionNetAmount, posMachine, userId,
                              paymentMode, cashAmount, creditCardAmount, gameCardAmount, paymentReference, primaryCardId, orderId, pOSTypeId, transactionNumber, remarks,
                               pOSMachineId, otherPaymentModeAmount, status, transactionProfileId, lastUpdateTime, lastUpdatedBy, tokenNumber, originalSystemReference, customerId,
                               externalSystemReference, reprintCount, originalTransactionId, createdBy, guid, synchStatus, siteId, masterEntityId, transactionOTP, customerIdentifier);
            this.transactionId = transactionId;
            this.transactionDate = transactionDate;
            this.transactionAmount = transactionAmount;
            this.transactionDiscountPercentage = transactionDiscountPercentage;
            this.taxAmount = taxAmount;
            this.transactionNetAmount = transactionNetAmount;
            this.posMachine = posMachine;
            this.userId = userId;
            this.paymentMode = paymentMode;
            this.cashAmount = cashAmount;
            this.creditCardAmount = creditCardAmount;
            this.gameCardAmount = gameCardAmount;
            this.orderId = orderId;
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
            this.lastUpdateTime = lastUpdateTime;
            this.lastUpdatedBy = lastUpdatedBy;
            this.tokenNumber = tokenNumber;
            this.originalSystemReference = originalSystemReference;
            this.customerId = customerId;
            this.externalSystemReference = externalSystemReference;
            this.reprintCount = reprintCount;
            this.originalTransactionId = originalTransactionId;
            this.orderTypeGroupId = orderTypeGroupId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.tableNumber = tableNumber;
            this.paid = paid;
            this.userName = userName;
            this.customerName = customerName;
            this.customerIdentifier = customerIdentifier;

            this.guid = guid;
            this.synchStatus = synchStatus;
            this.siteId = siteId;
            this.masterEntityId = masterEntityId;
            this.transactionOTP = transactionOTP;
            this.primaryCard = primaryCard;
            if (this.transactionDiscountPercentage != null && this.transactionDiscountPercentage > 0)
            {
                this.transactionDiscountAmount = this.transactionAmount - this.transactionNetAmount;
            }
            else
            {
                this.transactionDiscountAmount = 0;
            }

            log.LogMethodExit();
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        public TransactionDTO(TransactionDTO transactionDTO)
        {
            log.LogMethodEntry(transactionId, transactionDate, transactionAmount, transactionDiscountPercentage, taxAmount, transactionNetAmount, posMachine, userId,
                              paymentMode, cashAmount, creditCardAmount, gameCardAmount, paymentReference, primaryCardId, orderId, pOSTypeId, transactionNumber, remarks,
                               pOSMachineId, otherPaymentModeAmount, status, transactionProfileId, lastUpdateTime, lastUpdatedBy, tokenNumber, originalSystemReference, customerId,
                               externalSystemReference, reprintCount, originalTransactionId, createdBy, guid, synchStatus, siteId, masterEntityId, transactionOTP);
            this.transactionId = transactionDTO.transactionId;
            this.transactionDate = transactionDTO.transactionDate;
            this.transactionAmount = transactionDTO.transactionAmount;
            this.transactionDiscountPercentage = transactionDTO.transactionDiscountPercentage;
            this.taxAmount = transactionDTO.taxAmount;
            this.transactionNetAmount = transactionDTO.transactionNetAmount;
            this.posMachine = transactionDTO.posMachine;
            this.userId = transactionDTO.userId;
            this.paymentMode = transactionDTO.paymentMode;
            this.cashAmount = transactionDTO.cashAmount;
            this.creditCardAmount = transactionDTO.creditCardAmount;
            this.gameCardAmount = transactionDTO.gameCardAmount;
            this.orderId = transactionDTO.orderId;
            this.paymentReference = transactionDTO.paymentReference;
            this.primaryCardId = transactionDTO.primaryCardId;
            this.orderId = transactionDTO.orderId;
            this.pOSTypeId = transactionDTO.pOSTypeId;
            this.transactionNumber = transactionDTO.transactionNumber;
            this.remarks = transactionDTO.remarks;
            this.pOSMachineId = transactionDTO.pOSMachineId;
            this.otherPaymentModeAmount = transactionDTO.otherPaymentModeAmount;
            this.status = transactionDTO.status;
            this.transactionProfileId = transactionDTO.transactionProfileId;
            this.lastUpdateTime = transactionDTO.lastUpdateTime;
            this.lastUpdatedBy = transactionDTO.lastUpdatedBy;
            this.tokenNumber = transactionDTO.tokenNumber;
            this.originalSystemReference = transactionDTO.originalSystemReference;
            this.customerId = transactionDTO.customerId;
            this.externalSystemReference = transactionDTO.externalSystemReference;
            this.reprintCount = transactionDTO.reprintCount;
            this.originalTransactionId = transactionDTO.originalTransactionId;
            this.orderTypeGroupId = transactionDTO.orderTypeGroupId;
            this.createdBy = transactionDTO.createdBy;
            this.creationDate = transactionDTO.creationDate;
            this.tableNumber = transactionDTO.tableNumber;
            this.paid = transactionDTO.paid;
            this.userName = transactionDTO.userName;
            this.customerName = transactionDTO.customerName;
            this.guid = transactionDTO.guid;
            this.synchStatus = transactionDTO.synchStatus;
            this.siteId = transactionDTO.siteId;
            this.masterEntityId = transactionDTO.masterEntityId;
            this.transactionOTP = transactionDTO.transactionOTP;
            this.primaryCard = transactionDTO.primaryCard;
            this.transactionLinesDTOList = new List<TransactionLineDTO>();
            if (transactionDTO.transactionLinesDTOList != null && transactionDTO.transactionLinesDTOList.Any())
            {
                foreach (TransactionLineDTO transactionLineDTO in transactionDTO.transactionLinesDTOList)
                {
                    this.transactionLinesDTOList.Add(new TransactionLineDTO(transactionLineDTO));
                }
            }
            this.customerIdentifier = transactionDTO.customerIdentifier;

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
                IsChanged = true;
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
                IsChanged = true;
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
                IsChanged = true;
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
                IsChanged = true;
                transactionDiscountPercentage = value;
            }
        }

        /// <summary>
        /// Get/Set method of the TransactionDiscountAmount field
        /// </summary>
        public decimal? TransactionDiscountAmount
        {
            get
            {
                return transactionDiscountAmount;
            }

            set
            {
                transactionDiscountAmount = value;
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
                IsChanged = true;
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
                IsChanged = true;
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
                IsChanged = true;
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
                IsChanged = true;
                userId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the PaymentMode field
        /// </summary>
        public int PaymentMode
        {
            get
            {
                return paymentMode;
            }

            set
            {
                IsChanged = true;
                paymentMode = value;
            }
        }

        /// <summary>
        /// Get/Set method of the PaymentModeName field
        /// </summary>
        public string PaymentModeName
        {
            get
            {
                return paymentModeName;
            }

            set
            {
                IsChanged = true;
                paymentModeName = value;
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
                IsChanged = true;
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
                IsChanged = true;
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
                IsChanged = true;
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
                IsChanged = true;
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
                IsChanged = true;
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
                IsChanged = true;
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
                IsChanged = true;
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
                IsChanged = true;
                transactionNumber = value;
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
                IsChanged = true;
                transactionOTP = value;
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
                IsChanged = true;
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
                IsChanged = true;
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
                IsChanged = true;
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
                IsChanged = true;
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
                IsChanged = true;
                transactionProfileId = value;
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
                IsChanged = true;
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
                IsChanged = true;
                lastUpdatedBy = value;
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
                IsChanged = true;
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
                IsChanged = true;
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
                IsChanged = true;
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
                IsChanged = true;
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
                IsChanged = true;
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
                IsChanged = true;
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
                IsChanged = true;
                orderTypeGroupId = value;
            }
        }

        /// <summary>
        /// Get method of the tableNumber field
        /// </summary>
        public string TableNumber
        {
            get
            {
                return tableNumber;
            }
            set
            {
                tableNumber = value;
            }
        }

        /// <summary>
        /// Get/Set method of the paid field
        /// </summary>
        public decimal? Paid
        {
            get
            {
                return paid;
            }

            set
            {
                paid = value;
            }
        }

        /// <summary>
        /// Get method of the userName field
        /// </summary>
        public string UserName
        {
            get
            {
                return userName;
            }
            set
            {
                userName = value;
            }
        }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        public int CreatedBy
        {
            get
            {
                return createdBy;
            }
            set
            {
                IsChanged = true;
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
                IsChanged = true;
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
                IsChanged = true;
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
                IsChanged = true;
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
        }

        /// <summary>
        /// Get/Set Method for select field
        /// Used in orderdetailsview for row selection 
        /// will not be saved to the db.
        /// </summary>
        public bool Selected
        {
            get; set;
        }

        /// <summary>
        /// Get method of the Tickets field
        /// </summary>
        public string Tickets
        {
            get
            {
                return tickets;
            }
            set
            {
                tickets = value;
            }
        }

        /// <summary>
        /// Get method of the receipt field
        /// </summary>
        public string Receipt
        {
            get
            {
                return receipt;
            }
            set
            {
                receipt = value;
            }
        }

        /// <summary>
        /// Get method of the Tickets field
        /// </summary>
        public string TicketsHTML
        {
            get
            {
                return ticketsHTML;
            }
            set
            {
                ticketsHTML = value;
            }
        }

        /// <summary>
        /// Get method of the receipt field
        /// </summary>
        public string ReceiptHTML
        {
            get
            {
                return receiptHTML;
            }
            set
            {
                receiptHTML = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string VisitDate
        {
            get
            {
                return visitDate;
            }
            set
            {
                visitDate = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Boolean ApplyVisitDate
        {
            get
            {
                return applyVisitDate;
            }
            set
            {
                applyVisitDate = value;
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
                    return notifyingObjectIsChanged || transactionId < 0;
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
        /// Returns whether customer or any child record is changed
        /// </summary>
        [Browsable(false)]
        public bool IsChangedRecursive
        {
            get
            {
                bool isChangedRecursive = IsChanged;
                if (TransactionLinesDTOList != null)
                {
                    foreach (var TransactionDTO in TransactionLinesDTOList)
                    {
                        isChangedRecursive = isChangedRecursive || TransactionDTO.IsChanged;
                    }
                }
                return isChangedRecursive;
            }
        }

        /// <summary>
        /// Allowes to accept the changes
        /// </summary>
        public void AcceptChanges()
        {
            this.IsChanged = false;
        }

        /// <summary>
        /// Collection of TransactionLinesDTO
        /// </summary>
        public List<TransactionLineDTO> TransactionLinesDTOList
        {
            get
            {
                return transactionLinesDTOList;
            }
            set
            {
                transactionLinesDTOList = value;
            }
        }

        public List<TransactionPaymentsDTO> TrxPaymentsDTOList
        {
            get
            {
                return trxPaymentsDTOList;
            }
            set
            {
                trxPaymentsDTOList = value;
            }
        }

        /// <summary>
        /// Get method of the TransactionDiscountsDTOList field
        /// </summary>
        public List<DiscountsSummaryDTO> DiscountsSummaryDTOList
        {
            get
            {
                return discountsSummaryDTOList;
            }
            set
            {
                discountsSummaryDTOList = value;
            }
        }

        /// <summary>
        /// Get method of the TransactionDiscountsDTOList field
        /// </summary>
        public List<DiscountApplicationHistoryDTO> DiscountApplicationHistoryDTOList
        {
            get
            {
                return discountApplicationHistoryDTOList;
            }
            set
            {
                discountApplicationHistoryDTOList = value;
            }
        }

        /// <summary>
        /// PrimaryCard
        /// </summary>
        public String PrimaryCard
        {
            get
            {
                return primaryCard;
            }
            set
            {
                primaryCard = value;
            }
        }

        /// <summary>
        /// ReceiptDTO
        /// </summary>
        public ReceiptDTO ReceiptDTO
        {
            get
            {
                return receiptDTO;
            }
            set
            {
                receiptDTO = value;
            }
        }

        /// <summary>
        /// TicketDTO
        /// </summary>
        public List<TicketDTO> TicketDTOList
        {
            get
            {
                return ticketDTOList;
            }
            set
            {
                ticketDTOList = value;
            }
        }

        /// <summary>
        /// TicketPrinterMapDTO
        /// </summary>
        public List<TicketPrinterMapDTO> TicketPrinterMapDTOList
        {
            get
            {
                return ticketPrinterMapDTOList;
            }
            set
            {
                ticketPrinterMapDTOList = value;
            }
        }

        /// <summary>
        /// Get method of the customerName field
        /// </summary>
        public string CustomerName
        {
            get { return customerName; }
            set { customerName = value; }
        }

        /// <summary>
        /// Get method of the CommitTransaction field used for Continue as Guest
        /// </summary>
        public bool CommitTransaction
        {
            get { return commitTransaction; }
            set { commitTransaction = value; }
        }

        /// <summary>
        /// Get method of the CustomerIdentifier field used for Continue as Guest
        /// </summary>
        public bool SaveTransaction
        {
            get { return saveTransaction; }
            set { saveTransaction = value; }
        }

        /// <summary>
        /// Get method of the CustomerIdentifier field used for Continue as Guest
        /// </summary>
        public bool CloseTransaction
        {
            get { return closeTransaction; }
            set { closeTransaction = value; }
        }

        /// <summary>
        /// Get method of the CustomerIdentifier field used for Continue as Guest
        /// </summary>
        public bool ApplyOffset
        {
            get { return applyOffset; }
            set { applyOffset = value; }
        }

        /// <summary>
        /// Get method of the CustomerIdentifier field used for Continue as Guest
        /// </summary>
        public bool PaymentProcessingCompleted
        {
            get { return paymentProcessingCompleted; }
            set { paymentProcessingCompleted = value; }
        }

        /// <summary>
        /// Get method of the CustomerIdentifier field used for Continue as Guest
        /// </summary>
        public bool ReverseTransaction
        {
            get { return reverseTransaction; }
            set { reverseTransaction = value; }
        }

        /// <summary>
        /// Get method of the CustomerIdentifier field used for Continue as Guest
        /// </summary>
        public string CustomerIdentifier
        {
            get { return customerIdentifier; }
            set { this.IsChanged = true;
                customerIdentifier = value; }
        }
        /// <summary>
        /// Get/Set method of the ReceiptPrintTemplateHeaderDTO field
        /// </summary>
        public List<TrxPOSPrinterOverrideRulesDTO> TrxPOSPrinterOverrideRulesDTO
        {
            get { return trxPOSPrinterOverrideRulesDTOList; }
            set { trxPOSPrinterOverrideRulesDTOList = value;
                this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the TransctionOrderDispensingDTOList field
        /// </summary>
        public TransactionOrderDispensingDTO TransctionOrderDispensingDTO
        {
            get { return transctionOrderDispensingDTO; }
            set { transctionOrderDispensingDTO = value;
                this.IsChanged = true;}
        }

        //public List<TransactionTaxLineDTO> TransactionTaxLinesDTOList
        //{
        //    get { return transactionTaxLinesDTOList; }
        //    set { transactionTaxLinesDTOList = value; }
        //}

        ///// <summary>
        ///// Get/Set method of the PaymentPageLink field
        ///// </summary>
        //public string PaymentPageLink
        //{
        //    get
        //    {
        //        return paymentPageLink;
        //    }
        //    set
        //    {
        //        paymentPageLink = value;
        //    } 
        //}
    }
}
