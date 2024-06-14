/********************************************************************************************
 * Project Name - CommonAPI
 * Description  - Created to hold the transaction details .
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.130.7    07-Apr-2022   M S Shreyas             Created : External  REST API.
 ***************************************************************************************************/
using System;
using System.Collections.Generic;

namespace Semnox.Parafait.ThirdParty.External
{

    public class ExternalTransactionDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// AdjustmentTypes enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum AdjustmentTypes
        {
            /// <summary>
            /// Search by AddValue field
            /// </summary>
            AddValue,
            /// <summary>
            /// Search by AddProduct field
            /// </summary>
            AddProduct,
            /// <summary>
            /// Search by RemoveProduct field
            /// </summary>
            RemoveProduct,
            /// <summary>
            /// Search by RemoveValue field
            /// </summary>
            RemoveValue,
            /// <summary>
            /// Search by TransactionRefund field
            /// </summary>
            TransactionRefund,
            /// <summary>
            /// Search by Create field
            /// </summary>
            Create
        }

        /// <summary>
        /// Get/Set for Type
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// Get/Set for TransactionTime
        /// </summary>
        public DateTime? TransactionTime { get; set; }
        /// <summary>
        /// Get/Set for LoyaltyCardNumber
        /// </summary>
        public string LoyaltyCardNumber { get; set; }
        /// <summary>
        /// Get/Set for Operators
        /// </summary>
        public Operators Operators { get; set; }
        /// <summary>
        /// Get/Set for Adjustments
        /// </summary>
        public List<Adjustments> Adjustments { get; set; }
        /// <summary>
        /// Get/Set for NetAmount
        /// </summary>
        public decimal? NetAmount { get; set; }
        /// <summary>
        /// Get/Set for ExternalReference
        /// </summary>
        public string ExternalReference { get; set; }
        /// <summary>
        /// Get/Set for Payments
        /// </summary>
        public List<Payments> Payments { get; set; }
        /// <summary>
        /// Get/Set for Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Get/Set for OTP
        /// </summary>
        public string OTP { get; set; }
        /// <summary>
        /// Get/Set for Tax rates
        /// </summary>
        //public List<TaxRates> TaxRates { get; set; }
        /// <summary>
        /// Get/Set for Discounts
        /// </summary>
        public List<Discounts> Discounts { get; set; }
        public List<TaxRates> TaxRates { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public ExternalTransactionDTO()
        {
            log.LogMethodEntry();
            Type = String.Empty;
            TransactionTime = null;
            LoyaltyCardNumber = String.Empty;
            Operators = new Operators();
            Adjustments = new List<Adjustments>();
            ExternalReference = String.Empty;
            Payments = new List<Payments>();
            Id = -1;
            OTP = String.Empty;
            TaxRates = new List<TaxRates>();
            Discounts = new List<Discounts>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with parameter
        /// </summary>
        public ExternalTransactionDTO(string type, DateTime? transactionTime, string loyaltyCardNumber, decimal? netAmount, string externalReference,
                                      int id, string oTP)
        {
            log.LogMethodEntry(type, transactionTime, loyaltyCardNumber, netAmount, externalReference,  id, oTP);
            this.Type = type;
            this.TransactionTime = transactionTime;
            this.LoyaltyCardNumber = loyaltyCardNumber;
            this.NetAmount = netAmount;
            this.ExternalReference = externalReference;
            this.Id = id;
            this.OTP = oTP;
            log.LogMethodExit();
        }
    }

    public class Operators
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get/Set for Type
        /// </summary>
        public string LoginId { get; set; }
        /// <summary>
        /// Get/Set for ProductName
        /// </summary>
        public int SiteId { get; set; }
        /// <summary>
        /// Get/Set for Point
        /// </summary>
        public string PosMachine { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public Operators()
        {
            log.LogMethodEntry();
            LoginId = String.Empty; ;
            SiteId = -1;
            PosMachine = string.Empty;
            log.LogMethodExit();
        }
        /// <summary>
        /// constructor with parameter
        /// </summary>
        public Operators(string loginId, int siteId, string posName)
        {
            log.LogMethodEntry(loginId, siteId, posName);
            this.LoginId = loginId;
            this.SiteId = siteId;
            this.PosMachine = posName;
            log.LogMethodExit();
        }
    }

    public class Adjustments
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Get/Set for Type
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// Get/Set for ProductId
        /// </summary>
        public int ProductId { get; set; }
        /// <summary>
        /// Get/Set for ProductName
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// Get/Set for ProductName
        /// </summary>
        public string ProductReference { get; set; }
        /// <summary>
        /// Get/Set for ProductName
        /// </summary>
        public string CardNumber { get; set; }
        /// <summary>
        /// Get/Set for Quantity
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// Get/Set for amount
        /// </summary>
        public decimal? Amount { get; set; }
        /// <summary>
        /// Get/Set for Point
        /// </summary>
        public List<Points> Point { get; set; }
        //public List<TaxRates> TaxRates { get; set; }
        
        /// <summary>
        /// Default constructor
        /// </summary>
        public Adjustments()
        {
            log.LogMethodEntry();
            Type = String.Empty; ;
            ProductName = string.Empty;
            CardNumber = string.Empty;
            ProductReference = string.Empty;
            Quantity = -1;
            Point = new List<Points>();
            ProductId = -1;
            //TaxRates = new List<TaxRates>();
            log.LogMethodExit();
        }

        /// <summary>
        /// constructor with parameter
        /// </summary>
        public Adjustments(string type, int productId, string productName, string productReference, string cardNumber, int quantity, 
            decimal? amount, List<Points> point)
        {
            log.LogMethodEntry(type, productId, productName, productReference, cardNumber, quantity, amount, point);
            this.Type = type;
            this.ProductId = productId;
            this.ProductName = productName;
            this.ProductReference = productReference;
            this.CardNumber = cardNumber;
            this.Quantity = quantity;
            this.Amount = amount;
            this.Point = point;
            log.LogMethodExit();
        }
    }
    public class TaxRates
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        /// <summary>
        /// Get/Set for TotalTaxAmount
        /// </summary>
        public decimal TotalTaxAmount { get; set; }
        public string TaxName { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public TaxRates()
        {
            log.LogMethodEntry();
            TotalTaxAmount = 0;
            TaxName = string.Empty;
            log.LogMethodExit();
        }

        /// <summary>
        /// constructor with parameter
        /// </summary>
        public TaxRates(string taxName, decimal totalTaxAmount)
        {
            log.LogMethodEntry(totalTaxAmount);
            this.TotalTaxAmount = totalTaxAmount;
            this.TaxName = taxName;
            log.LogMethodExit();
        }
    }

    public class Payments
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Get/Set for Type
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// Get/Set for Amount
        /// </summary>
        public decimal? Amount { get; set; }
        //object
        /// <summary>
        /// Get/Set for ExpireDate
        /// </summary>
        public int PaymentId { get; set; }
        /// <summary>
        /// Get/Set for PaymentModeId
        /// </summary>
        public int PaymentModeId { get; set; }
        /// <summary>
        /// Get/Set for CardName
        /// </summary>
        public string PaymentDate { get; set; }
        /// <summary>
        /// Get/Set for CreditCard
        /// </summary>
        public CreditCard CreditCard { get; set; }
        /// <summary>
        /// Get/Set for Remarks
        /// </summary>
        public string Remarks { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public Payments()
        {
            log.LogMethodEntry();
            Type = String.Empty;
            PaymentId = -1;
            PaymentModeId = -1;
            PaymentDate = String.Empty;
            CreditCard = new CreditCard();
            Remarks = string.Empty;
            log.LogMethodExit();
        }

        /// <summary>
        /// constructor with parameter
        /// </summary>
        public Payments(string type, decimal? amount, int paymentId, int paymentModeId, string paymentDate, string remarks)
        {
            log.LogMethodEntry(type, amount, paymentId, paymentModeId, paymentDate);
            this.Type = type;
            this.Amount = amount;
            this.PaymentId = paymentId;
            this.PaymentModeId = paymentModeId;
            this.PaymentDate = paymentDate;
            this.Remarks = remarks;
            log.LogMethodExit();
        }
    }

    public class CreditCard
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Get/Set for card number
        /// </summary>
        public string CardNumber { get; set; }
        /// <summary>
        /// Get/Set for MaskedCardNumber
        /// </summary>
        public string MaskedCardNumber { get; set; }
        //object
        /// <summary>
        /// Get/Set for ExpireDate
        /// </summary>
        public string ExpiryDate { get; set; }
        /// <summary>
        /// Get/Set for CardName
        /// </summary>
        public string CardName { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public CreditCard()
        {
            log.LogMethodEntry();
            CardNumber = String.Empty;
            MaskedCardNumber = String.Empty;
            ExpiryDate = String.Empty;
            CardName = String.Empty;
            log.LogMethodExit();

        }
        /// <summary>
        /// constructor with parameter
        /// </summary>
        public CreditCard(string cardNumber, string maskedCardNumber, string expiryDate, string cardName)
        {
            log.LogMethodEntry(cardNumber, maskedCardNumber, expiryDate, cardName);
            this.CardNumber = cardNumber;
            this.MaskedCardNumber = maskedCardNumber;
            this.ExpiryDate = expiryDate;
            this.CardName = cardName;
            log.LogMethodExit();
        }
    }

    public class Discounts
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Get/Set for DiscountName
        /// </summary>
        public string DiscountName { get; set; }
        /// <summary>
        /// Get/Set for Amount
        /// </summary>
        public int Amount { get; set; }
        /// <summary>
        /// Get/Set for CouponNumber
        /// </summary>
        public string CouponNumber { get; set; }
        /// <summary>
        /// Get/Set for Remark
        /// </summary>
        public string Remarks { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public Discounts()
        {
            log.LogMethodEntry();
            DiscountName = String.Empty; ;
            Amount = -1;
            CouponNumber = String.Empty; ;
            Remarks = String.Empty; ;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with Parameter
        /// </summary>
        ///
        public Discounts(string discountName, int amount, string couponNumber, string remarks)
        {
            log.LogMethodEntry(discountName, amount, couponNumber, remarks);
            this.DiscountName = discountName;
            this.Amount = amount;
            this.CouponNumber = couponNumber;
            this.Remarks = remarks;
            log.LogMethodExit();
        }
    }
}