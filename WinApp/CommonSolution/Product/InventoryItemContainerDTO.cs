/********************************************************************************************
 * Project Name - Products
 * Description  - InventoryItemContainerDTO class 
 *  
 **************
 **Version Log
 **************
 *Version     Date              Modified By      Remarks          
 *********************************************************************************************
 2.110.0      02-Dec-2020       Deeksha          Created : POS UI Redesign with REST API
 2.150.0      15-Sep-2022       Abhishek         Modified : Web Inventory Redesign - Added fields 
                                                 UomId, InventoryUomId and MasterEntityId 
 2.150.5      03-Nov-2023       Abhishek         Modified : Web Inventory Redesign - Addition of field 
                                                 PurchaseTaxId to calculate product price on order.                                                                          
 ********************************************************************************************/
using System.Collections.Generic;
using System.ComponentModel;

namespace Semnox.Parafait.Product
{
    public class InventoryItemContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int productId;
        private string code;
        private string description;
        private int categoryId;
        private int defaultLocationId;
        private int outboundLocationId;
        private string isRedeemable;
        private string isSellable;
        private decimal priceInTickets;
        private string imageFileName;
        private int turnInPriceInTickets;
        private bool lotControlled;
        private bool marketListItem;
        private decimal? lastPurchasePrice;
        private string taxInclusiveCost;
        private string expiryType;
        private int expiryInDays;
        private int uomId;
        private int inventoryUomId;
        private double cost;
        private double reorderQuantity;
        private int purchaseTaxId;
        private int masterEntityId;
        private List<ProductBarcodeContainerDTO> productBarcodeContainerDTOList;

        /// <summary>
        /// Default constructor
        /// </summary>
        public InventoryItemContainerDTO()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public InventoryItemContainerDTO(int productId, string code, string description, int categoryId,
                                         int defaultLocationId, int outboundLocationId, string isRedeemable,
                                         string isSellable, decimal priceInTickets, string imageFileName,
                                         int turnInPriceInTickets, bool lotControlled,decimal? lastPurchasePrice,
                                         string taxInclusiveCost,string expiryType,int expiryInDays,bool marketListItem,
                                         int uomId, int inventoryUomId, double cost, double reorderQuantity, int purchaseTaxId, int masterEntityId)
            : this()
        {
            log.LogMethodEntry(productId, code, description, categoryId, defaultLocationId, outboundLocationId, 
                               isRedeemable, isSellable, priceInTickets, imageFileName, turnInPriceInTickets, lotControlled,
                               lastPurchasePrice, taxInclusiveCost, expiryType, expiryInDays, marketListItem, uomId, inventoryUomId, cost, reorderQuantity, purchaseTaxId, masterEntityId);
            this.productId = productId;
            this.code = code;
            this.description = description;
            this.categoryId = categoryId;
            this.defaultLocationId = defaultLocationId;
            this.outboundLocationId = outboundLocationId;
            this.isRedeemable = isRedeemable;
            this.isSellable = isSellable;
            this.priceInTickets = priceInTickets;
            this.imageFileName = imageFileName;
            this.turnInPriceInTickets = turnInPriceInTickets;
            this.lotControlled = lotControlled;
            this.lastPurchasePrice = lastPurchasePrice;
            this.taxInclusiveCost = taxInclusiveCost;
            this.expiryType = expiryType;
            this.expiryInDays = expiryInDays;
            this.marketListItem = marketListItem;
            this.uomId = uomId;
            this.inventoryUomId = inventoryUomId;
            this.cost = cost;
            this.reorderQuantity = reorderQuantity;
            this.purchaseTaxId = purchaseTaxId;
            this.masterEntityId = masterEntityId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Copy Constructor
        /// </summary>
        public InventoryItemContainerDTO(InventoryItemContainerDTO inventoryItemContainerDTO)
            : this(inventoryItemContainerDTO.productId, inventoryItemContainerDTO.code, inventoryItemContainerDTO.description, inventoryItemContainerDTO.categoryId,
                                         inventoryItemContainerDTO.defaultLocationId, inventoryItemContainerDTO.outboundLocationId, inventoryItemContainerDTO.isRedeemable,
                                         inventoryItemContainerDTO.isSellable, inventoryItemContainerDTO.priceInTickets, inventoryItemContainerDTO.imageFileName,
                                         inventoryItemContainerDTO.turnInPriceInTickets, inventoryItemContainerDTO.lotControlled, inventoryItemContainerDTO.lastPurchasePrice,
                                         inventoryItemContainerDTO.taxInclusiveCost, inventoryItemContainerDTO.expiryType, inventoryItemContainerDTO.expiryInDays, inventoryItemContainerDTO.marketListItem,
                                         inventoryItemContainerDTO.UomId, inventoryItemContainerDTO.InventoryUomId, inventoryItemContainerDTO.cost, inventoryItemContainerDTO.reorderQuantity,
                                         inventoryItemContainerDTO.purchaseTaxId, inventoryItemContainerDTO.masterEntityId)
        {
            log.LogMethodEntry(inventoryItemContainerDTO);
            if(inventoryItemContainerDTO.productBarcodeContainerDTOList != null)
            {
                productBarcodeContainerDTOList = new List<ProductBarcodeContainerDTO>();
                foreach (var productBarcodeContainerDTO in inventoryItemContainerDTO.productBarcodeContainerDTOList)
                {
                    ProductBarcodeContainerDTO productBarcodeContainerDTOCopy = new ProductBarcodeContainerDTO(productBarcodeContainerDTO);
                    productBarcodeContainerDTOList.Add(productBarcodeContainerDTOCopy);
                }
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Get/Set method of the ProductId field
        /// </summary>
        [DisplayName("Product Id")]
        [ReadOnly(true)]
        public int ProductId { get { return productId; } set { productId = value;} }

        /// <summary>
        /// Get/Set method of the Code field
        /// </summary>
        [DisplayName("Code")]
        [ReadOnly(true)]
        public string Code { get { return code; } set { code = value; } }

        /// <summary>
        /// Get/Set method of the Description field
        /// </summary>
        [DisplayName("Description")]
        public string Description { get { return description; } set { description = value;} }

        /// <summary>
        /// Get/Set method of the Category Id field
        /// </summary>
        [DisplayName("Category Id")]
        public int CategoryId { get { return categoryId; } set { categoryId = value; } }

        /// <summary>
        /// Get/Set method of the Default Location Id field
        /// </summary>
        [DisplayName("Default Location Id")]
        public int DefaultLocationId { get { return defaultLocationId; } set { defaultLocationId = value;} }
        
        /// <summary>
        /// Get/Set method of the Outbound Location Id field
        /// </summary>
        [DisplayName("Outbound Location id")]
        public int OutBoundLocationId { get { return outboundLocationId; } set { outboundLocationId = value;} }

        /// <summary>
        /// Get/Set method of the Is Redeemable field
        /// </summary>
        [DisplayName("Is Redeemable")]
        public string IsRedeemable { get { return isRedeemable; } set { isRedeemable = value;} }

        /// <summary>
        /// Get/Set method of the Is Sellable field
        /// </summary>
        [DisplayName("Is Sellable")]
        public string IsSellable { get { return isSellable; } set { isSellable = value;} }

        /// <summary>
        /// Get/Set method of the Price In Tickets field
        /// </summary>
        [DisplayName("Price In Tickets")]
        public decimal PriceInTickets { get { return priceInTickets; } set { priceInTickets = value; } }

        // <summary>
        /// Get/Set method of the Image File Name field
        /// </summary>
        [DisplayName("Image File Name")]
        public string ImageFileName { get { return imageFileName; } set { imageFileName = value; } }

        /// <summary>
        /// Get/Set method of the Turn-In Price In Tickets field
        /// </summary>
        [DisplayName("Turn-In Price In Tickets")]
        public int TurnInPriceInTickets { get { return turnInPriceInTickets; } set { turnInPriceInTickets = value; } }

        /// <summary>
        /// Get/Set method of the LotControlled Tickets field
        /// </summary>
        [DisplayName("LotControlled")]
        public bool LotControlled { get { return lotControlled; } set { lotControlled = value; } }

        /// <summary>
        /// Get/Set method of the lastPurchasePrice  field
        /// </summary>
        [DisplayName("LastPurchasePrice")]
        public decimal? LastPurchasePrice { get { return lastPurchasePrice; } set { lastPurchasePrice = value; } } /// <summary>

        /// Get/Set method of the taxInclusiveCost  field
        /// </summary>
        [DisplayName("TaxInclusiveCost")]
        public string TaxInclusiveCost { get { return taxInclusiveCost; } set { taxInclusiveCost = value; } }

        /// Get/Set method of the ExpiryType  field
        /// </summary>
        [DisplayName("ExpiryType")]
        public string ExpiryType { get { return expiryType; } set { expiryType = value; } }

        /// Get/Set method of the ExpiryInDays  field
        /// </summary>
        [DisplayName("ExpiryInDays")]
        public int ExpiryInDays { get { return expiryInDays; } set { expiryInDays = value; } } 
        
        /// Get/Set method of the ExpiryInDays  field
        /// </summary>
        [DisplayName("MarketListItem")]
        public bool MarketListItem { get { return marketListItem; } set { marketListItem = value; } }

        /// <summary>
        /// Get/Set method of the UomId field
        /// </summary>
        public int UomId { get { return uomId; } set { uomId = value; } }

        /// <summary>
        /// Get/Set method of the InventoryUomId field
        /// </summary>
        public int InventoryUomId { get { return inventoryUomId; } set { inventoryUomId = value; } }

        /// <summary>
        /// Get/Set method of the Cost field
        /// </summary>
        public double Cost { get { return cost; } set { cost = value; } }

        /// <summary>
        /// Get/Set method of the ReorderQuantity field
        /// </summary>
        public double ReorderQuantity { get { return reorderQuantity; } set { reorderQuantity = value; } }

        /// <summary>
        /// Get/Set method of the PurchaseTaxId field
        /// </summary>
        public int PurchaseTaxId { get { return purchaseTaxId; } set { purchaseTaxId = value; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; } }

        /// <summary>
        /// Get/Set method of the ProductBarcodeContainerDTO field
        /// </summary>
        public List<ProductBarcodeContainerDTO> ProductBarcodeContainerDTOList { get { return productBarcodeContainerDTOList; } set { productBarcodeContainerDTOList = value; } }

    }
}
