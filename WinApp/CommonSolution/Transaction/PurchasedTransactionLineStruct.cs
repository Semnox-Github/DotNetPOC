/********************************************************************************************
 * Project Name - Transaction
 * Description  - Data object of PurchasedTransactionLineStruct Struct
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        07-Aug-2019   Divya A          Created 
 ********************************************************************************************/
using System;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    ///  PurchasedTransactionLineStruct Class
    /// </summary>
    public class PurchasedTransactionLineStruct
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        private int transactionId;
        private string productName;
        private string productType;
        private int quantity;
        private decimal productPrice;
        private decimal serviceCharge;
        private decimal tax;
        private decimal discountPercentage;
        private decimal totalAmount;
        private string userName;
        private DateTime transactionDate;
        private string posMachineName;
        private string voidString; 
        
        public PurchasedTransactionLineStruct()
        {
            log.LogMethodEntry();
            transactionId = -1;
            quantity = 0;
            productPrice = 0;
            serviceCharge = 0;
            tax = 0;
            discountPercentage = 0;
            totalAmount = 0;
            log.LogMethodExit();
        }

        public int TransactionId { get { return transactionId; } set { transactionId = value; } }
        public string ProductName { get { return productName; } set { productName = value; } }
        public string ProductType { get { return productType; } set { productType = value; } }
        public int Quantity { get { return quantity; } set { quantity = value; } }
        public decimal ProductPrice { get { return productPrice; } set { productPrice = value; } }
        public decimal ServiceCharge { get { return serviceCharge; } set { serviceCharge = value; } }
        public decimal Tax { get { return tax; } set { tax = value; } }
        public decimal DiscountPercentage { get { return discountPercentage; } set { discountPercentage = value; } }
        public decimal TotalAmount { get { return totalAmount; } set { totalAmount = value; } }
        public string UserName { get { return userName; } set { userName = value; } }
        public DateTime TransactionDate { get { return transactionDate; } set { transactionDate = value; } }
        public string PosMachineName { get { return posMachineName; } set { posMachineName = value; } }
        public string Void { get { return voidString; } set { voidString = value; } }
        
    }
}
