/********************************************************************************************
 * Project Name - Discounts
 * Description  - Data structure of DiscountedProductsContainerDTO
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.150.0      13-Apr-2021      Abhishek           Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Discounts
{
    /// <summary>
    /// Data structure of DiscountedProductsContainerDTO
    /// </summary>
    public class DiscountedProductsContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int id;
        private int discountId;
        private int productId;
        private int productGroupId;
        private int categoryId;
        private double? discountPercentage;
        private double? discountAmount;
        private decimal? discountedPrice;
        private int? quantity;
        private string discounted;
        private List<int> productIdList;

        /// <summary>
        /// Default constructor
        /// </summary>
        public DiscountedProductsContainerDTO()
        {
            log.LogMethodEntry();
            id = -1;
            discountId = -1;
            productId = -1;
            productGroupId = -1;
            categoryId = -1;
            discountPercentage = 0;
            discountAmount = 0;
            discountAmount = 0;
            productIdList = new List<int>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with all fields
        /// </summary>
        public DiscountedProductsContainerDTO(int id, 
                                              int discountId, 
                                              int productId, 
                                              int productGroupId, 
                                              int categoryId, 
                                              int? quantity, 
                                              double? discountPercentage, 
                                              double? discountAmount, 
                                              decimal? discountedPrice, 
                                              string discounted)
            :this()
        {
            log.LogMethodEntry(id, discountId, 
                               productId, productGroupId,
                               categoryId, quantity,discountPercentage, 
                               discountAmount,discountedPrice,
                               discounted);
            this.id = id;
            this.discountId = discountId;
            this.productId = productId;
            this.productGroupId = productGroupId;
            this.categoryId = categoryId;
            this.quantity = quantity;
            this.discountPercentage = discountPercentage;
            this.discountAmount = discountAmount;
            this.discountedPrice = discountedPrice;
            this.discounted = discounted;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of discountId field
        /// </summary>
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// Get/Set method of discountId field
        /// </summary>
        public int DiscountId
        {
            get { return discountId; }
            set { discountId = value; }
        }

        /// <summary>
        /// Get/Set method of productId field
        /// </summary>
        public int ProductId
        {
            get { return productId; }
            set { productId = value; }
        }

        /// <summary>
        /// Get/Set method of productGroupId field
        /// </summary>
        public int ProductGroupId
        {
            get { return productGroupId; }
            set { productGroupId = value; }
        }

        /// <summary>
        /// Get/Set method of categoryId field 
        /// </summary>
        public int CategoryId
        {
            get { return categoryId; }
            set { categoryId = value; }
        }

        /// <summary>
        /// Get/Set method of quantity field 
        /// </summary>
        public int? Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }

        /// <summary>
        /// Get/Set method of discountPercentage field 
        /// </summary>
        public double? DiscountPercentage
        {
            get { return discountPercentage; }
            set { discountPercentage = value; }
        }

        /// <summary>
        /// Get/Set method of discountAmount field 
        /// </summary>
        public double? DiscountAmount
        {
            get { return discountAmount; }
            set { discountAmount = value; }
        }

        /// <summary>
        /// Get/Set method of discountedPrice field 
        /// </summary>
        public decimal? DiscountedPrice
        {
            get { return discountedPrice; }
            set { discountedPrice = value; }
        }

        /// <summary>
        /// Get/Set method of discounted field 
        /// </summary>
        public string Discounted
        {
            get { return discounted; }
            set { discounted = value; }
        }

        /// <summary>
        /// Get/Set method of productIdList field 
        /// </summary>
        public List<int> ProductIdList
        {
            get
            {
                return productIdList;
            }
            set
            {
                productIdList = value;
            }
        }
    }
}
