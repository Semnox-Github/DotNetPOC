/********************************************************************************************
 * Project Name - Discounts
 * Description  - Data structure of DiscountPurchaseCriteriaContainerDTO
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.150.0      13-Apr-2021       Abhishek           Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Discounts
{
    public class DiscountPurchaseCriteriaContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int criteriaId;
        private int discountId;
        private int productId;
        private int productGroupId;
        private int categoryId;
        private int? minQuantity;
        private List<int> productIdList;
        /// <summary>
        /// Default constructor
        /// </summary>
        public DiscountPurchaseCriteriaContainerDTO()
        {
            log.LogMethodEntry();
            criteriaId = -1;
            discountId = -1;
            productId = -1;
            productGroupId = -1;
            categoryId = -1;
            minQuantity = -1;
            productIdList = new List<int>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with all fields
        /// </summary>
        public DiscountPurchaseCriteriaContainerDTO(int criteriaId, 
                                                    int discountId, 
                                                    int productId,
                                                    int productGroupId,
                                                    int categoryId, 
                                                    int? minQuantity)
            :this()
        {
            log.LogMethodEntry(criteriaId, discountId, productId, productGroupId, categoryId, minQuantity);
            this.criteriaId = criteriaId;
            this.discountId = discountId;
            this.productId = productId;
            this.productGroupId = productGroupId;
            this.categoryId = categoryId;
            this.minQuantity = minQuantity;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of criteriaId field
        /// </summary>
        public int CriteriaId
        {
            get
            {
                return criteriaId;
            }
            set
            {
                criteriaId = value;
            }
        }

        /// <summary>
        /// Get/Set method of discountId field
        /// </summary>
        public int DiscountId
        {
            get
            {
                return discountId;
            }
            set
            {
                discountId = value;
            }
        }

        /// <summary>
        /// Get/Set method of productId field
        /// </summary>
        public int ProductId
        {
            get
            {
                return productId;
            }
            set
            {
                productId = value;
            }
        }

        /// <summary>
        /// Get/Set method of productGroupId field
        /// </summary>
        public int ProductGroupId
        {
            get
            {
                return productGroupId;
            }
            set
            {
                productGroupId = value;
            }
        }

        /// <summary>
        /// Get/Set method of categoryId field 
        /// </summary>
        public int CategoryId
        {
            get
            {
                return categoryId;
            }
            set
            {
                categoryId = value;
            }
        }

        /// <summary>
        /// Get/Set method of minQuantity field 
        /// </summary>
        public int? MinQuantity
        {
            get
            {
                return minQuantity;
            }
            set
            {
                minQuantity = value;
            }
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
