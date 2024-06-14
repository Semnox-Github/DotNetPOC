/********************************************************************************************
 * Project Name - TransactionRefreshLinesExcelDTODefinition
 * Description  - TransactionRefreshLinesExcelDTODefinition
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.90        19-Aug-2019   Vikas          Created 
 ********************************************************************************************/
using Semnox.Core.GenericUtilities.Excel;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Transaction
{
    public class TransactionRefreshLineDTODefinition
    {
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
        private int waviersSignedCount;

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
        public int WaiversSignedCount { get { return waviersSignedCount; } set { waviersSignedCount = value; } }

    }

    public class TransactionRefreshLinesExcelDTODefinition : ComplexAttributeDefinition
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public TransactionRefreshLinesExcelDTODefinition(ExecutionContext executionContext, string fieldName) : base(fieldName, typeof(TransactionRefreshLineDTODefinition))
        {
            log.LogMethodEntry(executionContext, fieldName);
            attributeDefinitionList.Add(new SimpleAttributeDefinition("ProductName", "Product", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Quantity", "quantity", new NullableDecimalValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Price", "Price", new NullableDecimalValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Amount", "amount", new NullableDecimalValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("WaiverCustomer", "Waiver_Customer", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("CardNumber", "card_number", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Credits", "credits", new NullableDecimalValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Courtesy", "courtesy", new NullableDecimalValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Bonus", "bonus", new NullableDecimalValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Time", "time", new NullableDecimalValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Tickets", "tickets", new NullableDecimalValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("TaxName", "tax_name", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("TaxPercentage", "Tax %", new NullableDecimalValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("LoyaltyPoints", "loyalty_points", new NullableDecimalValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("UserPrice", "UserPrice", new BooleanValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("LineId", "Line", new IntValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("WaiversSignedCount", "Waivers_Signed", new IntValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Remarks", "Remarks", new StringValueConverter()));
            log.LogVariableState("AttributeDefinitionList", attributeDefinitionList);
            log.LogMethodExit();
        }
    }
}
