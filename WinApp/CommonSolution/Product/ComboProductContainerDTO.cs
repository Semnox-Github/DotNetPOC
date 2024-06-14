/********************************************************************************************
 * Project Name - Products
 * Description  - ComboProductsContainerDTO class 
 *  
 **************
 **Version Log
 **************
 *Version     Date              Modified By      Remarks          
 *********************************************************************************************
 2.110.0      02-Dec-2020       Deeksha          Created : POS UI Redesign with REST API
 2.150.0      22-Mar-2022       Girish Kundar    Modified:  Added new column MaximumQuantity 
 ********************************************************************************************/
using System.ComponentModel;

namespace Semnox.Parafait.Product
{
    public class ComboProductContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int comboProductId;
        private int product_id;
        private int childProductId;
        private string childProductName;
        private string childProductType;
        private string childProductAutoGenerateCardNumber;
        private decimal quantity;
        private int categoryId;
        private int? sortOrder;
        private double? price;
        private bool priceInclusive;
        private int displayGroupId;
        private string displayGroup;
        private bool additionalProduct;
        private string categoryName;
        private int? maximumQuantity;
        private decimal finalPriceWithTax;
        private decimal finalPrice;

        /// <summary>
        /// Default constructor
        /// </summary>
        public ComboProductContainerDTO()
        {
            log.LogMethodEntry();
            categoryName = string.Empty;
            maximumQuantity = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public ComboProductContainerDTO(int comboProductId, int product_id, int childProductId, 
                                        string childProductName, string childProductType,
                                        string childProductAutoGenerateCardNumber, decimal quantity,
                                        int categoryId, int? sortOrder, double? price, bool priceInclusive,
                                        int displayGroupId, string displayGroup, bool additionalProduct, int? maximumQuantity, decimal finalPriceWithTax, decimal finalPrice)
            : this()
        {
            log.LogMethodEntry(comboProductId, product_id, childProductId, childProductName, childProductType,
                               childProductAutoGenerateCardNumber, quantity, categoryId, sortOrder, maximumQuantity, finalPriceWithTax, finalPrice);
            this.comboProductId = comboProductId;
            this.product_id = product_id;
            this.childProductId = childProductId;
            this.childProductName = childProductName;
            this.childProductType = childProductType;
            this.childProductAutoGenerateCardNumber = childProductAutoGenerateCardNumber;
            this.quantity = quantity;
            this.categoryId = categoryId;
            this.sortOrder = sortOrder;
            this.price = price;
            this.priceInclusive = priceInclusive;
            this.displayGroupId = displayGroupId;
            this.displayGroup = displayGroup;
            this.additionalProduct = additionalProduct;
            this.maximumQuantity = maximumQuantity;
            this.finalPriceWithTax = finalPriceWithTax;
            this.finalPrice = finalPrice;
            log.LogMethodExit();
        }

        /// <summary>
        /// Copy Constructor
        /// </summary>
        /// <param name="comboProductContainerDTO"></param>
        public ComboProductContainerDTO(ComboProductContainerDTO comboProductContainerDTO)
            : this(comboProductContainerDTO.comboProductId, comboProductContainerDTO.product_id, comboProductContainerDTO.childProductId,
                                        comboProductContainerDTO.childProductName, comboProductContainerDTO.childProductType,
                                        comboProductContainerDTO.childProductAutoGenerateCardNumber, comboProductContainerDTO.quantity,
                                        comboProductContainerDTO.categoryId, comboProductContainerDTO.sortOrder, comboProductContainerDTO.price,
                                        comboProductContainerDTO.priceInclusive, comboProductContainerDTO.displayGroupId, comboProductContainerDTO.displayGroup,
                                        comboProductContainerDTO.additionalProduct, comboProductContainerDTO.maximumQuantity, comboProductContainerDTO.FinalPriceWithTax, comboProductContainerDTO.FinalPrice)
        {
            log.LogMethodEntry(comboProductContainerDTO);
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the Combo Product Id field
        /// </summary>
        [DisplayName("Combo Product Id")]
        [ReadOnly(true)]
        public int ComboProductId { get { return comboProductId; } set { comboProductId = value; } }

        /// <summary>
        /// Get/Set method of the ProductId field
        /// </summary>
        [DisplayName("Product Id")]
        [ReadOnly(true)]
        public int ProductId { get { return product_id; } set { product_id = value; } }

        /// <summary>
        /// Get/Set method of the Child Product Id field
        /// </summary>
        [DisplayName("Child Product Id")]
        [ReadOnly(true)]
        public int ChildProductId { get { return childProductId; } set { childProductId = value; } }

        /// <summary>
        /// Get/Set method of the Child Product Name field
        /// </summary>
        [DisplayName("Child Product Name")]
        public string ChildProductName { get { return childProductName; } set { childProductName = value; } }

        /// <summary>
        /// Get/Set method of the CategoryName field
        /// </summary>
        [DisplayName("CategoryName")]
        public string CategoryName { get { return categoryName; } set { categoryName = value; } }

        /// <summary>
        /// Get/Set method of the Child Product Type field
        /// </summary>
        [DisplayName("Child Product Type")]
        public string ChildProductType { get { return childProductType; } set { childProductType = value; } }
        
        /// <summary>
        /// Get/Set method of the Child Product Auto Generated Number field
        /// </summary>
        [DisplayName("Child Product Auto Generated Number")]
        public string ChildProductAutoGenerateCardNumber { get { return childProductAutoGenerateCardNumber; } set { childProductAutoGenerateCardNumber = value; } }

        /// <summary>
        /// Get/Set method of the Quantity field
        /// </summary>
        [DisplayName("Quantity")]
        public decimal Quantity { get { return quantity; } set { quantity = value; } }

        /// <summary>
        /// Get/Set method of the Category Id field
        /// </summary>
        [DisplayName("Category Id")]
        public int CategoryId { get { return categoryId; } set { categoryId = value; } }

        /// <summary>
        /// Get/Set method of the DisplayGroupId field
        /// </summary>
        [DisplayName("DisplayGroupId")]
        public int DisplayGroupId { get { return displayGroupId; } set { displayGroupId = value; } }

        /// <summary>
        /// Get/Set method of the DisplayGroup field
        /// </summary>
        [DisplayName("DisplayGroup")]
        public string DisplayGroup { get { return displayGroup; } set { displayGroup = value; } }

        /// <summary>
        /// Get/Set method of the PriceInclusive field
        /// </summary>
        [DisplayName("PriceInclusive")]
        public bool PriceInclusive { get { return priceInclusive; } set { priceInclusive = value; } }

        /// <summary>
        /// Get/Set method of the AdditionalProduct field
        /// </summary>
        [DisplayName("AdditionalProduct")]
        public bool AdditionalProduct { get { return additionalProduct; } set { additionalProduct = value; } }

        /// <summary>
        /// Get/Set method of the SortOrder field
        /// </summary>
        [DisplayName("Sort Order")]
        public int? SortOrder { get { return sortOrder; } set { sortOrder = value; } }

        /// <summary>
        /// Get/Set method of the Price field
        /// </summary>
        [DisplayName("Price")]
        public double? Price { get { return price; } set { price = value; } }

        /// <summary>
        /// Get/Set method of the MaximumQuantity field
        /// </summary>
        [DisplayName("MaximumQuantity")]
        public int? MaximumQuantity { get { return maximumQuantity; } set { maximumQuantity = value; } }

        /// <summary>
        /// Get/Set method of the FinalPriceWithTax field
        /// </summary>
        [DisplayName("FinalPriceWithTax")]
        public decimal FinalPriceWithTax { get { return finalPriceWithTax; } set { finalPriceWithTax = value; } }

        /// <summary>
        /// Get/Set method of the FinalPrice field
        /// </summary>
        [DisplayName("FinalPrice")]
        public decimal FinalPrice { get { return finalPrice; } set { finalPrice = value; } }
    }
}
