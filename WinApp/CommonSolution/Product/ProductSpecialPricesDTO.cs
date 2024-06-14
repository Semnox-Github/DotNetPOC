/********************************************************************************************
 * Project Name - Product Special Prices DTO
 * Description  - Data object of Product Special Prices DTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By                     Remarks          
 *********************************************************************************************
 *2.50        16-Feb-2019   Indrajeet Kumar                 Created
 *2.60        22-Mar-2019   Nagesh Badiger                  Added ActiveFlag, log method entry and method exit
 **********************************************************************************************
 *2.60        25-Mar-2019   Akshay Gulaganji                commented activeFlag
 *2.70.0      08-Aug-2019   Akshay Gulaganji                modified properties
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.Product
{
    public class ProductSpecialPricesDTO
    {
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        ///  SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// ID search field
            /// </summary>
            SPECIALPRICING_ID,
            /// <summary>
            /// PRODUCT_ID search field
            /// </summary>
            PRODUCT_ID,
            /// <summary>
            /// PERCENTAGE search field
            /// </summary>
            PERCENTAGE,
            // <summary>
            ///  ISACTIVE search field
            /// </summary>
            OVERRIDDEN,
            // <summary>
            ///  SITE_ID search field
            /// </summary>
            SITE_ID
        }

        int specialPricingId;
        int productId;
        string productName;
        string productType;
        int? price;
        int? specialPrice;
        decimal? changePrice;
        int siteId;
        string overridden;
        // bool activeFlag;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ProductSpecialPricesDTO()
        {
            log.LogMethodEntry();
            this.specialPricingId = -1;
            this.productId = -1;
            this.siteId = -1;
            //  this.activeFlag = true;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields. Used for ProductSpecialPrices
        /// </summary>

        public ProductSpecialPricesDTO(int specialPricingId, int productId, string productName, string productType, int? price, int? specialPrice, decimal? changePrice, string overridden)
        {
            log.LogMethodEntry(specialPricingId, productId, productName, productType, price, specialPrice, changePrice, overridden);
            this.specialPricingId = specialPricingId;
            this.productId = productId;
            this.productName = productName;
            this.productType = productType;
            this.price = price;
            this.specialPrice = specialPrice;
            this.changePrice = changePrice;
            this.overridden = overridden;
            // this.activeFlag = activeFlag;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the SpecialPricingId field
        /// </summary>
        public int SpecialPricingId { get { return specialPricingId; } set { specialPricingId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Product_id field
        /// </summary>
        public int ProductId { get { return productId; } set { productId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Product_name field
        /// </summary>
        public string ProductName { get { return productName; } set { productName = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Product_type field
        /// </summary>
        public string ProductType { get { return productType; } set { productType = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Price field
        /// </summary>
        public int? Price { get { return price; } set { price = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Special_price field
        /// </summary>
        public int? SpecialPrice { get { return specialPrice; } set { specialPrice = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Change_price field
        /// </summary>
        public decimal? ChangePrice { get { return changePrice; } set { changePrice = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Overridden field
        /// </summary>
        public string Overridden { get { return overridden; } set { overridden = value; this.IsChanged = true; } }

        ///// <summary>
        ///// Get/Set method of the ActiveFlag field
        ///// </summary>
        //public bool ActiveFlag { get { return activeFlag; } set { activeFlag = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        public int SiteId { get { return siteId; } set { siteId = value; this.IsChanged = true; } }

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
                    return notifyingObjectIsChanged || specialPricingId < 0;
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
        /// Allows to accept the changes
        /// </summary>
        public void AcceptChanges()
        {
            log.LogMethodEntry();
            this.IsChanged = false;
            log.LogMethodExit();
        }

    }
}
