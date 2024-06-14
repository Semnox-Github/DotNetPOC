/********************************************************************************************
 * Project Name - TransactionCoreDetailedPurchaseStruct Program
 * Description  - TransactionCoreDetailedPurchaseStruct object of TransactionCoreDetailedPurchaseStruct
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By       Remarks          
 *********************************************************************************************
 *1.00        01-June-2016   Rakshith          Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// DetailedPurchasesStruct Class
    /// </summary>
    public class TransactionCoreDetailedPurchaseStruct
    {
        private int transactionId;
        private string transactionDate;
        private string issueDate;
        private string refundDate;
        private int newCardId;
        private int refundCardId;
        private string productName;
        private string productType;
        private string quantity;
        private int userId;
        private string posMachine;
        private double amount;
        private int posTypeId;
        private string poscounter;
        private double cashRatio;
        private double creditCardRatio;
        private double gameRatio;
        private double otherModeRatio;
        private double netAmount;
        private double tax;
        private string displayGroup;
        private int productId;
        private int discountId;
        private int lineId;
        private int siteId;
        private int transactionNumber;
        private string paymentReference;
        private string remarks;
        private string lineRemarks;
        private int cardId;
        private string cardSale;
        private double transactionPrice;
        private double productPrice;
        private string productTypeDescription;
        private double discountAmount;
        private string status;
        private int paymentMode;
        private string reportGroup;

        /// <summary>
        /// Defaul Constructor
        /// </summary>
        public TransactionCoreDetailedPurchaseStruct()
        {
        }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        public TransactionCoreDetailedPurchaseStruct(int transactionId, string transactionDate, string issueDate, string refundDate,
                                        int newCardId, int refundCardId, string productName, string productType, string quantity,
                                        int userId, string posMachine, double amount, int posTypeId, string poscounter,
                                        double cashRatio, double creditCardRatio, double gameRatio, double otherModeRatio,
                                        double netAmount, double tax, string displayGroup, int productId, int discountId,
                                        int lineId, int siteId, int transactionNumber, string paymentReference, string remarks,
                                        string lineRemarks, int cardId, string cardSale, double transactionPrice, double productPrice,
                                        string productTypeDescription, double discountAmount, string status, int paymentMode,
                                        string reportGroup)
        {
            this.transactionId = transactionId;
            this.transactionDate = transactionDate;
            this.issueDate = issueDate;
            this.refundDate = refundDate;
            this.newCardId = newCardId;
            this.refundCardId = refundCardId;
            this.productName = productName;
            this.productType = productType;
            this.quantity = quantity;
            this.userId = userId;
            this.posMachine = posMachine;
            this.amount = amount;
            this.posTypeId = posTypeId;
            this.poscounter = poscounter;
            this.cashRatio = cashRatio;
            this.creditCardRatio = creditCardRatio;
            this.gameRatio = gameRatio;
            this.otherModeRatio = otherModeRatio;
            this.netAmount = netAmount;
            this.tax = tax;
            this.displayGroup = displayGroup;
            this.productId = productId;
            this.discountId = discountId;
            this.lineId = lineId;
            this.siteId = siteId;
            this.transactionNumber = transactionNumber;
            this.paymentReference = paymentReference;
            this.remarks = remarks;
            this.lineRemarks = lineRemarks;
            this.cardId = cardId;
            this.cardSale = cardSale;
            this.transactionPrice = transactionPrice;
            this.productPrice = productPrice;
            this.productTypeDescription = productTypeDescription;
            this.discountAmount = discountAmount;
            this.status = status;
            this.paymentMode = paymentMode;
            this.reportGroup = reportGroup;
            Console.Write(" a reportGroup " + reportGroup);
        }

        /// <summary>
        /// Get/Set method of the TransactionId field
        /// </summary>

        public int TransactionId { get { return transactionId; } set { transactionId = value; } }

        /// <summary>
        /// Get/Set method of the TransactionDate field
        /// </summary>
        public string TransactionDate { get { return transactionDate; } set { transactionDate = value; } }

        /// <summary>
        /// Get/Set method of the IssueDate field
        /// </summary>
        public string IssueDate { get { return issueDate; } set { issueDate = value; } }

        /// <summary>
        /// Get/Set method of the RefundDate field
        /// </summary>
        public string RefundDate { get { return refundDate; } set { refundDate = value; } }

        /// <summary>
        /// Get/Set method of the NewCardId field
        /// </summary>
        public int NewCardId { get { return newCardId; } set { newCardId = value; } }

        /// <summary>
        /// Get/Set method of the RefundCardId field
        /// </summary>
        public int RefundCardId { get { return refundCardId; } set { refundCardId = value; } }

        /// <summary>
        /// Get/Set method of the ProductName field
        /// </summary>
        public string ProductName { get { return productName; } set { productName = value; } }

        /// <summary>
        /// Get/Set method of the ProductType field
        /// </summary>
        public string ProductType { get { return productType; } set { productType = value; } }

        /// <summary>
        /// Get/Set method of the Quantity field
        /// </summary>
        public string Quantity { get { return quantity; } set { quantity = value; } }

        /// <summary>
        /// Get/Set method of the UserId field
        /// </summary>
        public int UserId { get { return userId; } set { userId = value; } }

        /// <summary>
        /// Get/Set method of the PosMachine field
        /// </summary>
        public string PosMachine { get { return posMachine; } set { posMachine = value; } }

        /// <summary>
        /// Get/Set method of the Amount field
        /// </summary>
        public double Amount { get { return amount; } set { amount = value; } }

        /// <summary>
        /// Get/Set method of the PosTypeId field
        /// </summary>
        public int PosTypeId { get { return posTypeId; } set { posTypeId = value; } }

        /// <summary>
        /// Get/Set method of the Poscounter field
        /// </summary>
        public string Poscounter { get { return poscounter; } set { poscounter = value; } }

        /// <summary>
        /// Get/Set method of the CashRatio field
        /// </summary>
        public double CashRatio { get { return cashRatio; } set { cashRatio = value; } }

        /// <summary>
        /// Get/Set method of the CreditCardRatio field
        /// </summary>
        public double CreditCardRatio { get { return creditCardRatio; } set { creditCardRatio = value; } }

        /// <summary>
        /// Get/Set method of the GameRatio field
        /// </summary>
        public double GameRatio { get { return gameRatio; } set { gameRatio = value; } }

        /// <summary>
        /// Get/Set method of the OtherModeRatio field
        /// </summary>
        public double OtherModeRatio { get { return otherModeRatio; } set { otherModeRatio = value; } }

        /// <summary>
        /// Get/Set method of the NetAmount field
        /// </summary>
        public double NetAmount { get { return netAmount; } set { netAmount = value; } }

        /// <summary>
        /// Get/Set method of the Tax field
        /// </summary>
        public double Tax { get { return tax; } set { tax = value; } }

        /// <summary>
        /// Get/Set method of the DisplayGroup field
        /// </summary>
        public string DisplayGroup { get { return displayGroup; } set { displayGroup = value; } }

        /// <summary>
        /// Get/Set method of the ProductId field
        /// </summary>
        public int ProductId { get { return productId; } set { productId = value; } }

        /// <summary>
        /// Get/Set method of the DiscountId field
        /// </summary>
        public int DiscountId { get { return discountId; } set { discountId = value; } }

        /// <summary>
        /// Get/Set method of the LineId field
        /// </summary>
        public int LineId { get { return lineId; } set { lineId = value; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        public int SiteId { get { return siteId; } set { siteId = value; } }

        /// <summary>
        /// Get/Set method of the TransactionNumber field
        /// </summary>
        public int TransactionNumber { get { return transactionNumber; } set { transactionNumber = value; } }

        /// <summary>
        /// Get/Set method of the PaymentReference field
        /// </summary>
        public string PaymentReference { get { return paymentReference; } set { paymentReference = value; } }

        /// <summary>
        /// Get/Set method of the Remarks field
        /// </summary>
        public string Remarks { get { return remarks; } set { remarks = value; } }

        /// <summary>
        /// Get/Set method of the LineRemarks field
        /// </summary>
        public string LineRemarks { get { return lineRemarks; } set { lineRemarks = value; } }

        /// <summary>
        /// Get/Set method of the CardId field
        /// </summary>
        public int CardId { get { return cardId; } set { cardId = value; } }

        /// <summary>
        /// Get/Set method of the CardSale field
        /// </summary>
        public string CardSale { get { return cardSale; } set { cardSale = value; } }

        /// <summary>
        /// Get/Set method of the TransactionPrice field
        /// </summary>
        public double TransactionPrice { get { return transactionPrice; } set { transactionPrice = value; } }

        /// <summary>
        /// Get/Set method of the ProductPrice field
        /// </summary>
        public double ProductPrice { get { return productPrice; } set { productPrice = value; } }

        /// <summary>
        /// Get/Set method of the ProductTypeDescription field
        /// </summary>
        public string ProductTypeDescription { get { return productTypeDescription; } set { productTypeDescription = value; } }

        /// <summary>
        /// Get/Set method of the DiscountAmount field
        /// </summary>
        public double DiscountAmount { get { return discountAmount; } set { discountAmount = value; } }

        /// <summary>
        /// Get/Set method of the Status field
        /// </summary>
        public string Status { get { return status; } set { status = value; } }

        /// <summary>
        /// Get/Set method of the PaymentMode field
        /// </summary>
        public int PaymentMode { get { return paymentMode; } set { paymentMode = value; } }

        /// <summary>
        /// Get/Set method of the ReportGroup field
        /// </summary>
        public string ReportGroup { get { return reportGroup; } set { reportGroup = value; } }


    }
}