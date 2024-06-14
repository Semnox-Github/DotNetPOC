/********************************************************************************************
 * Project Name - TransactionDetailsExcelDTODefinition
 * Description  - TransactionDetailsExcelDTODefinition
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.90        19-Aug-2019   Vikas          Created 
 ********************************************************************************************/
using System;
using Semnox.Core.GenericUtilities.Excel;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Transaction
{
    public class TransactionDetailsDTODefinition
    {
        private int transactionId;
        private string transactionNumber;
        private DateTime transactionDate;
        private decimal? transactionAmount;
        private decimal? transactionNetAmount;
        private string posMachine;
        private string paymentMode;
        private string status;
        private string userName;
        private int lineId;
        private decimal? price;
        private decimal? quantity;
        private decimal? amount;
        private string waiverCustomer;
        private string cardNumber;
        private decimal? credits;
        private decimal? courtesy;
        private decimal? taxPercentage;
        private decimal? time;
        private decimal? bonus;
        private decimal? tickets;
        private decimal? loyaltyPoints;
        private string remarks;
        private bool userPrice;
        private string productName;
        private string taxName;
        private bool wavierSigned;

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
                lineId = value;
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
                amount = value;
            }
        }

        /// <summary>
        /// Get/Set method of the TransactionId field
        /// </summary>
        public string WaiverCustomer
        {
            get
            {
                return waiverCustomer;
            }

            set
            {
                waiverCustomer = value;
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
                taxPercentage = value;
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
                remarks = value;
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
                userPrice = value;
            }
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
        /// Get/Set of the taxName field
        /// </summary>
        public string TaxName { get { return taxName; } set { taxName = value; } }

        /// <summary>
        /// Get/Set of the wavierSigned field
        /// </summary>
        public bool WaiverSigned { get { return wavierSigned; } set { wavierSigned = value; } }

    }

    public class TransactionDetailsExcelDTODefinition : ComplexAttributeDefinition
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public TransactionDetailsExcelDTODefinition(ExecutionContext executionContext, string fieldName) : base(fieldName, typeof(TransactionDetailsDTODefinition))
        {
            log.LogMethodEntry(executionContext, fieldName);
            attributeDefinitionList.Add(new SimpleAttributeDefinition("TransactionId", "ID", new IntValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("TransactionNumber", "Trx No", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("TransactionDate", "Date", new DateTimeValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("TransactionAmount", "Amount", new NullableDecimalValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("TransactionNetAmount", "Net_Amount", new NullableDecimalValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("PaymentMode", "pay_mode", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("PosMachine", "POS", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("UserName", "Cashier", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("ProductName", "Product", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Price", "Price/Disc%", new NullableDecimalValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Amount", "Line_amount", new NullableDecimalValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("WaiverCustomer", "Waiver_Customer", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("CardNumber", "card_number", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Credits", "credits", new NullableDecimalValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Courtesy", "courtesy", new NullableDecimalValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Bonus", "bonus", new NullableDecimalValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Time", "time", new NullableDecimalValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Tickets", "tickets", new NullableDecimalValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("TaxName", "tax_name", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("TaxPercentage", "Tax %", new NullableDecimalValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Quantity", "quantity", new NullableDecimalValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("LoyaltyPoints", "loyalty_points", new NullableDecimalValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("LineId", "Line", new IntValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Status", "Status", new StringValueConverter()));
            log.LogVariableState("AttributeDefinitionList", attributeDefinitionList);
            log.LogMethodExit();
        }
    }
}
