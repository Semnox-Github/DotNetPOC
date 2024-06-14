/********************************************************************************************
 * Project Name - Transaction
 * Description  - Represents a group of KDS Order lines used for display purpose
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70.2        17-Sep-2019   Lakshminarayana         Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;

namespace Semnox.Parafait.Transaction.KDS
{
    /// <summary>
    /// Represents a group of KDS Order lines used for display purpose
    /// </summary>
    public class KDSOrderLineGroupDTO
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int productId;
        private string productNameOffset;
        private string productName;
        private string productType;
        private string productDescription;
        private string transactionLineRemarks;
        private decimal quantity;
        private string status;
        private bool cancelled;
        private List<int> transactionLineIdList;
        private bool parentLine;

        /// <summary>
        /// Default constructor
        /// </summary>
        public KDSOrderLineGroupDTO()
        {
            log.LogMethodEntry();
            productId = -1;
            productName = string.Empty;
            productNameOffset = string.Empty;
            productDescription = string.Empty;
            transactionLineRemarks = string.Empty;
            quantity = 0;
            status = string.Empty;
            cancelled = false;
            parentLine = false;
            transactionLineIdList = new List<int>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="productId">product id</param>
        /// <param name="productName">product name</param>
        /// <param name="productType">product type</param>
        /// <param name="productDescription">product description</param>
        /// <param name="quantity">quantity</param>
        /// <param name="status">status</param>
        /// <param name="lineId">transaction line id</param>
        /// <param name="cancelled">whether the line is cancelled</param>
        /// <param name="parentLine">Whether this is a parent line</param>
        /// <param name="transactionLineRemarks"></param>
        public KDSOrderLineGroupDTO(int productId, string productName, string productType, string productDescription, decimal quantity, string status, int lineId, bool cancelled, bool parentLine, string transactionLineRemarks)
        :this()
        {
            log.LogMethodEntry(productId, productName, productType, quantity, status, lineId, cancelled);
            this.productId = productId;
            this.productName = productName;
            this.productType = productType;
            this.productDescription = productDescription;
            this.quantity = quantity;
            this.status = status;
            this.cancelled = cancelled;
            this.parentLine = parentLine;
            this.transactionLineRemarks = transactionLineRemarks;
            transactionLineIdList.Add(lineId);
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set Method of productId field
        /// </summary>
        public int ProductId
        {
            get { return productId; }
            set { productId = value; }
        }

        /// <summary>
        /// Get/Set Method of productName field
        /// </summary>
        public string ProductName
        {
            get { return productName; }
            set { productName = value; }
        }

        /// <summary>
        /// Get/Set Method of quantity field
        /// </summary>
        public decimal Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }

        /// <summary>
        /// Get/Set Method of status field
        /// </summary>
        public string Status
        {
            get { return status; }
            set { status = value; }
        }

        /// <summary>
        /// Get/Set Method of transactionLineIdList field
        /// </summary>
        public List<int> TransactionLineIdList
        {
            get { return transactionLineIdList; }
            set { transactionLineIdList = value; }
        }

        /// <summary>
        /// Get/Set Method of Product Description field
        /// </summary>
        public string ProductDescription
        {
            get { return productDescription; }
            set { productDescription = value; }
        }

        /// <summary>
        /// Get/Set Method of Product Name Offset field
        /// </summary>
        public string ProductNameOffset
        {
            get { return productNameOffset; }
            set { productNameOffset = value; }
        }

        /// <summary>
        /// Get/Set Method of Product Name With Offset field
        /// </summary>
        public string ProductNameWithOffset
        {
            get { return productNameOffset + productName; }
            set { log.LogVariableState("value", value); }
        }


        /// <summary>
        /// Get/Set Method of Product Name With Offset field
        /// </summary>
        public string ProductNameWithOffsetAndLineRemarks
        {
            get
            {
                string result = productNameOffset + productName;
                if (string.IsNullOrWhiteSpace(transactionLineRemarks) == false)
                {
                    result += Environment.NewLine + transactionLineRemarks;
                }
                return result;
            }
            set { log.LogVariableState("value", value); }
        }

        /// <summary>
        /// Get/Set Method of cancelled field
        /// </summary>
        public bool Cancelled
        {
            get { return cancelled; }
            set { cancelled = value; }
        }

        /// <summary>
        /// Get/Set Method of parentLine field
        /// </summary>
        public bool ParentLine
        {
            get { return parentLine; }
            set { parentLine = value; }
        }

        /// <summary>
        /// Get/Set Method of productType field
        /// </summary>
        public string ProductType
        {
            get { return productType; }
            set { productType = value; }
        }

        /// <summary>
        /// Get/Set Method of transactionLineRemarks field
        /// </summary>
        public string TransactionLineRemarks
        {
            get { return transactionLineRemarks; }
            set { transactionLineRemarks = value; }
        }
    }
}
