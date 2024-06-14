/********************************************************************************************
 * Project Name - TransactionLine DTO
 * Description  - Data object of TransactionLine
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *1.00        31-Jul-2017   Lakshminarayana          Created 
 *2.70.2      05-Nov-2019   Akshay Gulaganji         Added Who Columns -  createdBy, creationDate, lastUpdatedBy and lastUpdateDate
 *2.70.2      26-Nov-2019   Lakshminarayana          Virtual store enhancement
 *2.80.0      04-Jun-2020   Nitin Pai                Moved from iTransaction to Transaction Project
 *2.90.0      29-Jun-2020   Jinto Thomas             Added cancelCode field part of cancelLine code change    
 *2.110.0     14-Dec-2020   Guru S A                 For Subscription changes
 *2.140.0     01-Jun-2021   Fiona Lishal             Modified for Delivery Order enhancements for F&B
 ********************************************************************************************/
using Semnox.Parafait.Transaction.KDS;
using Semnox.Parafait.Waiver;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Transaction
{

    /// <summary>
    /// This is the TransactionLine data object class. This acts as data holder for the TransactionLine business object
    /// </summary>
    public class TransactionLineDTO
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
            /// Search by lineId field
            /// </summary>
            LINE_ID,
            /// <summary>
            /// Search by site_Id field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by transactionId field
            /// </summary>
            TRANSACTION_ID_LIST,
            /// <summary>
            /// Search by SUBSCRIPTION_HEADER_ID field
            /// </summary>
            SUBSCRIPTION_HEADER_ID,
        }

        private int transactionId;
        private int lineId;
        private int productId;
        private decimal? price;
        private decimal? quantity;
        private decimal? amount;
        private int cardId;
        private string cardNumber;
        private decimal? credits;
        private decimal? courtesy;
        private decimal? taxPercentage;
        private int taxId;
        private decimal? time;
        private decimal? bonus;
        private decimal? tickets;
        private decimal? loyaltyPoints;
        private string remarks;
        private int promotionId;
        private bool receiptPrinted;
        private int parentLineId;
        private bool userPrice;
        private int? kOTPrintCount;
        private long gameplayId;
        private bool kDSSent;
        private int creditPlusConsumptionId;
        private DateTime? cancelledTime;
        private string cancelledBy;
        private string productDescription;
        private string isWaiverSignRequired;
        private int originalLineId;
        private int membershipId;
        private int membershipRewardsId;
        private string expireWithMembership;
        private string forMembershipOnly;
        private string cardGuid;
        private string approvedBy;
        private string guid;
        private string clientGuid;
        private string parentLineGuid;
        private bool synchStatus;
        private int siteId;
        private int masterEntityId;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private string cancelCode;
        public decimal? taxAmount;
        public string taxName;
        private string productDetail;
        private string productName;
        private string productTypeCode;
        private Product.ProductsDTO productsDTO;
        private List<WaiverSignatureDTO> waiverSignedDTOList;
        private AttractionBookingDTO attractionBookingDTO;
        private List<TransactionDiscountsDTO> transactionDiscountsDTOList;
        private SubscriptionHeaderDTO subscriptionHeaderDTO;
        private List<KDSOrderLineDTO> kDSOrderLineDTOList;
       
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public TransactionLineDTO()
        {
            log.LogMethodEntry();
            transactionId = -1;
            lineId = 0;
            creditPlusConsumptionId = -1;
            cardId = -1;
            gameplayId = -1;
            productId = -1;
            promotionId = -1;
            taxId = -1;
            parentLineId = -1;
            originalLineId = -1;
            masterEntityId = -1;
            membershipId = -1;
            membershipRewardsId = -1;
            quantity = 1;
            expireWithMembership = "N";
            forMembershipOnly = "N";
            remarks = string.Empty;
            cancelCode = string.Empty;
            approvedBy = string.Empty;
            subscriptionHeaderDTO = null;
            taxAmount = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public TransactionLineDTO(TransactionLineDTO transactionLineDTO)
        {
            log.LogMethodEntry(transactionLineDTO);
            this.transactionId = transactionLineDTO.transactionId;
            this.lineId = transactionLineDTO.lineId;
            this.productId = transactionLineDTO.productId;
            this.price = transactionLineDTO.price;
            this.quantity = transactionLineDTO.quantity;
            this.amount = transactionLineDTO.amount;
            this.cardId = transactionLineDTO.cardId;
            this.cardNumber = transactionLineDTO.cardNumber;
            this.credits = transactionLineDTO.credits;
            this.courtesy = transactionLineDTO.courtesy;
            this.taxPercentage = transactionLineDTO.taxPercentage;
            this.taxId = transactionLineDTO.taxId;
            this.time = transactionLineDTO.time;
            this.bonus = transactionLineDTO.bonus;
            this.tickets = transactionLineDTO.tickets;
            this.loyaltyPoints = transactionLineDTO.loyaltyPoints;
            this.remarks = transactionLineDTO.remarks;
            this.promotionId = transactionLineDTO.promotionId;
            this.parentLineId = transactionLineDTO.parentLineId;
            this.userPrice = transactionLineDTO.userPrice;
            this.kOTPrintCount = transactionLineDTO.kOTPrintCount;
            this.gameplayId = transactionLineDTO.gameplayId;
            this.kDSSent = transactionLineDTO.kDSSent;
            this.creditPlusConsumptionId = transactionLineDTO.creditPlusConsumptionId;
            this.cancelledTime = transactionLineDTO.cancelledTime;
            this.cancelledBy = transactionLineDTO.cancelledBy;
            this.productDescription = transactionLineDTO.productDescription;
            this.isWaiverSignRequired = transactionLineDTO.isWaiverSignRequired;
            this.originalLineId = transactionLineDTO.originalLineId;
            this.cardGuid = transactionLineDTO.cardGuid;
            this.cancelCode = transactionLineDTO.cancelCode;

            this.guid = transactionLineDTO.guid;
            this.synchStatus = transactionLineDTO.synchStatus;
            this.siteId = transactionLineDTO.siteId;
            this.masterEntityId = transactionLineDTO.masterEntityId;
            this.membershipId = transactionLineDTO.membershipId;
            this.membershipRewardsId = transactionLineDTO.membershipRewardsId;
            this.expireWithMembership = transactionLineDTO.expireWithMembership;
            this.forMembershipOnly = transactionLineDTO.forMembershipOnly;
            this.productsDTO = transactionLineDTO.productsDTO;
            this.subscriptionHeaderDTO = transactionLineDTO.subscriptionHeaderDTO;
            this.approvedBy = transactionLineDTO.approvedBy;
            this.taxAmount = transactionLineDTO.TaxAmount;
            this.taxName = transactionLineDTO.TaxName;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public TransactionLineDTO(int transactionId, int lineId, int productId, decimal? price,
                              decimal? quantity, decimal? amount, int cardId, string cardNumber,
                              decimal? credits, decimal? courtesy, decimal? taxPercentage,
                              int taxId, decimal? time, decimal? bonus, decimal? tickets,
                              decimal? loyaltyPoints, string remarks, int promotionId,
                              bool receiptPrinted, int parentLineId, bool userPrice, int? kOTPrintCount,
                              long gameplayId, bool kDSSent, int creditPlusConsumptionId, DateTime? cancelledTime,
                              string cancelledBy, string productDescription, string isWaiverSignRequired,
                              int originalLineId, string guid, bool synchStatus, int siteId, int masterEntityId, int membershipId, int membershipRewardsId, string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdatedDate, string cancelCode, string expireWithMembership = "N", string forMembershipOnly = "N", string approvedBy = "")
        {
            log.LogMethodEntry(transactionId, lineId, productId, price, quantity, amount, cardId, cardNumber, credits, courtesy, taxPercentage, taxId, time, bonus, tickets,
                               loyaltyPoints, remarks, promotionId, receiptPrinted, parentLineId, userPrice, kOTPrintCount, gameplayId, kDSSent, creditPlusConsumptionId, cancelledTime,
                               cancelledBy, productDescription, isWaiverSignRequired, originalLineId, guid, synchStatus, siteId, masterEntityId, membershipId, membershipRewardsId,
                               createdBy, creationDate, lastUpdatedBy, lastUpdatedDate, cancelCode, expireWithMembership, forMembershipOnly, approvedBy);
            this.transactionId = transactionId;
            this.lineId = lineId;
            this.productId = productId;
            this.price = price;
            this.quantity = quantity;
            this.amount = amount;
            this.cardId = cardId;
            this.cardNumber = cardNumber;
            this.credits = credits;
            this.courtesy = courtesy;
            this.taxPercentage = taxPercentage;
            this.taxId = taxId;
            this.time = time;
            this.bonus = bonus;
            this.tickets = tickets;
            this.loyaltyPoints = loyaltyPoints;
            this.remarks = remarks;
            this.promotionId = promotionId;
            this.parentLineId = parentLineId;
            this.userPrice = userPrice;
            this.kOTPrintCount = kOTPrintCount;
            this.gameplayId = gameplayId;
            this.kDSSent = kDSSent;
            this.creditPlusConsumptionId = creditPlusConsumptionId;
            this.cancelledTime = cancelledTime;
            this.cancelledBy = cancelledBy;
            this.productDescription = productDescription;
            this.isWaiverSignRequired = isWaiverSignRequired;
            this.originalLineId = originalLineId;

            this.guid = guid;
            this.synchStatus = synchStatus;
            this.siteId = siteId;
            this.masterEntityId = masterEntityId;
            this.membershipId = membershipId;
            this.membershipRewardsId = membershipRewardsId;
            this.expireWithMembership = expireWithMembership;
            this.forMembershipOnly = forMembershipOnly;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.cancelCode = cancelCode;
            this.subscriptionHeaderDTO = null;
            this.approvedBy = approvedBy;
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
        /// Get/Set method of the TransactionId field
        /// </summary>
        public int LineId
        {
            get
            {
                return lineId;
            }

            set
            {
                IsChanged = true;
                lineId = value;
            }
        }
        /// <summary>
        ///Get/Set method of the ApprovedBy
        /// </summary>
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
        /// Get/Set method of the TransactionId field
        /// </summary>
        public int ProductId
        {
            get
            {
                return productId;
            }

            set
            {
                IsChanged = true;
                productId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the TransactionId field
        /// </summary>
        public decimal? Price
        {
            get
            {
                return price;
            }

            set
            {
                IsChanged = true;
                price = value;
            }
        }

        /// <summary>
        /// Get/Set method of the TransactionId field
        /// </summary>
        public decimal? Quantity
        {
            get
            {
                return quantity;
            }

            set
            {
                IsChanged = true;
                quantity = value;
            }
        }

        /// <summary>
        /// Get/Set method of the TransactionId field
        /// </summary>
        public decimal? Amount
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
        /// Get/Set method of the TransactionId field
        /// </summary>
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
        /// Get/Set method of the TransactionId field
        /// </summary>
        public string CardNumber
        {
            get
            {
                return cardNumber;
            }

            set
            {
                IsChanged = true;
                cardNumber = value;
            }
        }

        /// <summary>
        /// Get/Set method of the TransactionId field
        /// </summary>
        public decimal? Credits
        {
            get
            {
                return credits;
            }

            set
            {
                IsChanged = true;
                credits = value;
            }
        }

        /// <summary>
        /// Get/Set method of the TransactionId field
        /// </summary>
        public decimal? Courtesy
        {
            get
            {
                return courtesy;
            }

            set
            {
                IsChanged = true;
                courtesy = value;
            }
        }

        /// <summary>
        /// Get/Set method of the TransactionId field
        /// </summary>
        public decimal? TaxPercentage
        {
            get
            {
                return taxPercentage;
            }

            set
            {
                IsChanged = true;
                taxPercentage = value;
            }
        }

        /// <summary>
        /// Get/Set method of the TransactionId field
        /// </summary>
        public int TaxId
        {
            get
            {
                return taxId;
            }

            set
            {
                IsChanged = true;
                taxId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the TransactionId field
        /// </summary>
        public decimal? Time
        {
            get
            {
                return time;
            }

            set
            {
                IsChanged = true;
                time = value;
            }
        }

        /// <summary>
        /// Get/Set method of the TransactionId field
        /// </summary>
        public decimal? Bonus
        {
            get
            {
                return bonus;
            }

            set
            {
                IsChanged = true;
                bonus = value;
            }
        }

        /// <summary>
        /// Get/Set method of the TransactionId field
        /// </summary>
        public decimal? Tickets
        {
            get
            {
                return tickets;
            }

            set
            {
                IsChanged = true;
                tickets = value;
            }
        }

        /// <summary>
        /// Get/Set method of the TransactionId field
        /// </summary>
        public decimal? LoyaltyPoints
        {
            get
            {
                return loyaltyPoints;
            }

            set
            {
                IsChanged = true;
                loyaltyPoints = value;
            }
        }

        /// <summary>
        /// Get/Set method of the TransactionId field
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
        /// Get/Set method of the TransactionId field
        /// </summary>
        public int PromotionId
        {
            get
            {
                return promotionId;
            }

            set
            {
                IsChanged = true;
                promotionId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the TransactionId field
        /// </summary>
        public bool ReceiptPrinted
        {
            get
            {
                return receiptPrinted;
            }

            set
            {
                IsChanged = true;
                receiptPrinted = value;
            }
        }

        /// <summary>
        /// Get/Set method of the TransactionId field
        /// </summary>
        public int ParentLineId
        {
            get
            {
                return parentLineId;
            }

            set
            {
                IsChanged = true;
                parentLineId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the TransactionId field
        /// </summary>
        public bool UserPrice
        {
            get
            {
                return userPrice;
            }

            set
            {
                IsChanged = true;
                userPrice = value;
            }
        }

        /// <summary>
        /// Get/Set method of the TransactionId field
        /// </summary>
        public int? KOTPrintCount
        {
            get
            {
                return kOTPrintCount;
            }

            set
            {
                IsChanged = true;
                kOTPrintCount = value;
            }
        }

        /// <summary>
        /// Get/Set method of the TransactionId field
        /// </summary>
        public long GameplayId
        {
            get
            {
                return gameplayId;
            }

            set
            {
                IsChanged = true;
                gameplayId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the TransactionId field
        /// </summary>
        public bool KDSSent
        {
            get
            {
                return kDSSent;
            }

            set
            {
                IsChanged = true;
                kDSSent = value;
            }
        }

        /// <summary>
        /// Get/Set method of the TransactionId field
        /// </summary>
        public int CreditPlusConsumptionId
        {
            get
            {
                return creditPlusConsumptionId;
            }

            set
            {
                IsChanged = true;
                creditPlusConsumptionId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the TransactionId field
        /// </summary>
        public DateTime? CancelledTime
        {
            get
            {
                return cancelledTime;
            }

            set
            {
                IsChanged = true;
                cancelledTime = value;
            }
        }

        /// <summary>
        /// Get/Set method of the TransactionId field
        /// </summary>
        public string CancelledBy
        {
            get
            {
                return cancelledBy;
            }

            set
            {
                IsChanged = true;
                cancelledBy = value;
            }
        }

        /// <summary>
        /// Get/Set method of the TransactionId field
        /// </summary>
        public string ProductDescription
        {
            get
            {
                return productDescription;
            }

            set
            {
                IsChanged = true;
                productDescription = value;
            }
        }

        /// <summary>
        /// Get/Set method of the TransactionId field
        /// </summary>
        public string IsWaiverSignRequired
        {
            get
            {
                return isWaiverSignRequired;
            }

            set
            {
                IsChanged = true;
                isWaiverSignRequired = value;
            }
        }

        /// <summary>
        /// Get/Set method of the TransactionId field
        /// </summary>
        public int OriginalLineId
        {
            get
            {
                return originalLineId;
            }

            set
            {
                IsChanged = true;
                originalLineId = value;
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
        /// Get method of the ClientGuid field
        /// </summary>
        public string ClientGuid
        {
            get
            {
                if (String.IsNullOrEmpty(clientGuid) && !String.IsNullOrEmpty(guid))
                    return guid;
                else
                    return clientGuid;
            }
            set
            {
                clientGuid = value;
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
            set
            {
                IsChanged = true;
                synchStatus = value;
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
            set
            {
                IsChanged = true;
                masterEntityId = value;
            }
        }
        /// <summary>
        /// Get/Set method of the MembershipId field
        /// </summary>
        public int MembershipId
        {
            get
            {
                return membershipId;
            }

            set
            {
                IsChanged = true;
                membershipId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the MembershipRewardsId field
        /// </summary>
        public int MembershipRewardsId
        {
            get
            {
                return membershipRewardsId;
            }

            set
            {
                IsChanged = true;
                membershipRewardsId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the ExpireWithMembership field
        /// </summary>
        public string ExpireWithMembership
        {
            get
            {
                return expireWithMembership;
            }

            set
            {
                IsChanged = true;
                expireWithMembership = value;
            }
        }
        /// <summary>
        /// Get/Set method of the ForMembershipOnly field
        /// </summary>
        public string ForMembershipOnly
        {
            get
            {
                return forMembershipOnly;
            }

            set
            {
                IsChanged = true;
                forMembershipOnly = value;
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
                    return notifyingObjectIsChanged;
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
        /// Get/Set method of the productDetail field
        /// </summary>
        public string ProductDetail
        {
            get { return productDetail; }
            set { productDetail = value; }
        }

        /// <summary>
        /// Get/Set method of the productName field
        /// </summary>
        public string ProductName
        {
            get { return productName; }
            set { productName = value; }
        }

        /// <summary>
        /// Get/Set method of the productTypeCode field
        /// </summary>
        public string ProductTypeCode
        {
            get { return productTypeCode; }
            set { productTypeCode = value; }
        }

        /// <summary>
        /// Used for windows UI. Determines whether the line is selected.
        /// </summary>
        public bool Selected
        {
            get;
            set;
        }


        /// <summary>
        /// Get/Set method of createdBy field
        /// </summary>
        public string CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }

        /// <summary>
        /// Get/Set method of creationDate field
        /// </summary>
        public DateTime CreationDate
        {
            get { return creationDate; }
            set { creationDate = value; }
        }

        /// <summary>
        /// Get/Set method of lastUpdatedBy field
        /// </summary>
        public string LastUpdatedBy
        {
            get { return lastUpdatedBy; }
            set { lastUpdatedBy = value; }
        }

        /// <summary>
        /// Get/Set method of lastUpdatedDate field
        /// </summary>
        public DateTime LastUpdatedDate
        {
            get { return lastUpdatedDate; }
            set { lastUpdatedDate = value; }
        }

        /// <summary>
        /// Get/Set method of cardGuid
        /// </summary>
        public string CardGuid
        {
            get { return cardGuid; }
            set { cardGuid = value; }
        }

        /// <summary>
        /// Get/Set method of ProductsDTO
        /// </summary>
        public Product.ProductsDTO ProductsDTO
        {
            get { return productsDTO; }
            set { productsDTO = value; }
        }

        /// <summary>
        /// Allows to accept the changes
        /// </summary>
        public void AcceptChanges()
        {
            this.IsChanged = false;
        }

        /// <summary>
        /// Get/Set method of WaiverSignedDTOList
        /// </summary>
        public List<WaiverSignatureDTO> WaiverSignedDTOList
        {
            get { return waiverSignedDTOList; }
            set { waiverSignedDTOList = value; }
        }

        /// <summary>
        /// Get/Set method of WaiverSignedDTOList
        /// </summary>
        public AttractionBookingDTO AttractionBookingDTO
        {
            get { return attractionBookingDTO; }
            set { attractionBookingDTO = value; }
        }

        /// <summary>
        /// Get method of the ParentLineGuid field
        /// </summary>
        public string ParentLineGuid
        {
            get
            {
                return parentLineGuid;
            }
            set
            {
                parentLineGuid = value;
            }
        }

        /// <summary>
        /// Get method of the TransactionDiscountsDTOList field
        /// </summary>
        public List<TransactionDiscountsDTO> TransactionDiscountsDTOList
        {
            get
            {
                return transactionDiscountsDTOList;
            }
            set
            {
                transactionDiscountsDTOList = value;
            }
        }
        /// <summary>
        /// Get/Set method of the KDSOrderLineDTOList field
        /// </summary>
        public List<KDSOrderLineDTO> KDSOrderLineDTOList
        {
            get
            {
                return kDSOrderLineDTOList;
            }
            set
            {
                kDSOrderLineDTOList = value;
            }
        }

        /// <summary>
        /// Get/Set method of the CancelCode field
        /// </summary>
        public string CancelCode
        {
            get
            {
                return cancelCode;
            }

            set
            {
                IsChanged = true;
                cancelCode = value;
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
        /// Get/Set method of the TaxName field
        /// </summary>
        public string TaxName
        {
            get
            {
                return taxName;
            }

            set
            {
                IsChanged = true;
                taxName = value;
            }
        }

        /// <summary>
        /// Get/Set method of the subscriptionHeaderDTO field
        /// </summary>
        public SubscriptionHeaderDTO SubscriptionHeaderDTO
        {
            get
            {
                return subscriptionHeaderDTO;
            }
            set
            {
                subscriptionHeaderDTO = value;
            }
        }

        /// <summary>
        /// Returns a string that represents the current TransactionLineDTO
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            log.Debug("Starts-ToString() method.");
            StringBuilder returnValue = new StringBuilder("\n-----------------------TransactionLineDTO-----------------------------\n");
            returnValue.Append(" TransactionId : " + TransactionId);
            returnValue.Append(" LineId : " + LineId);
            returnValue.Append(" ProductId : " + ProductId);
            returnValue.Append(" Price : " + Price);
            returnValue.Append(" Quantity : " + Quantity);
            returnValue.Append(" Amount : " + Amount);
            returnValue.Append(" CardId : " + CardId);
            returnValue.Append(" CardNumber : " + CardNumber);
            returnValue.Append(" Credits : " + Credits);
            returnValue.Append(" Courtesy : " + Courtesy);
            returnValue.Append(" TaxPercentage : " + TaxPercentage);
            returnValue.Append(" TaxId : " + TaxId);
            returnValue.Append(" Time : " + Time);
            returnValue.Append(" Bonus : " + Bonus);
            returnValue.Append(" Tickets : " + Tickets);
            returnValue.Append(" LoyaltyPoints : " + LoyaltyPoints);
            returnValue.Append(" Remarks : " + Remarks);
            returnValue.Append(" PromotionId : " + PromotionId);
            returnValue.Append(" ReceiptPrinted : " + ReceiptPrinted);
            returnValue.Append(" ParentLineId : " + ParentLineId);
            returnValue.Append(" UserPrice : " + UserPrice);
            returnValue.Append(" KOTPrintCount : " + KOTPrintCount);
            returnValue.Append(" GameplayId : " + GameplayId);
            returnValue.Append(" KDSSent : " + KDSSent);
            returnValue.Append(" CreditPlusConsumptionId : " + CreditPlusConsumptionId);
            returnValue.Append(" CancelledTime : " + CancelledTime);
            returnValue.Append(" CancelledBy : " + CancelledBy);
            returnValue.Append(" ProductDescription : " + ProductDescription);
            returnValue.Append(" IsWaiverSignRequired : " + IsWaiverSignRequired);
            returnValue.Append(" OriginalLineId : " + OriginalLineId);
            returnValue.Append("\n-------------------------------------------------------------\n");
            log.Debug("Ends-ToString() Method");
            return returnValue.ToString();

        }
    }
}
