/********************************************************************************************
 * Project Name - Product DTO
 * Description  - Data object of product 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        13-Apr-2016   Raghuveera          Created 
 ********************************************************************************************
 *1.00        11-Aug-2016   Soumya              Updated to add more columns 
 *******************************************************************************************
 *1.00        18-Aug-2016   Suneetha            Modified 
 ********************************************************************************************
 *2.60       12-Apr-2019    Girish              Modified : Added PurchaseTaxId field and Get/Set 
 * ****************************************************************************************
 *2.60        10-Apr-2019   Archana             Include/Exclude for redeemable products changes
 *2.90.       03-Jun-2020   Deeksha             Modified : Bulk product publish & weighted avg costing changes
 *2.100.0     26-Jul-2020   Deeksha             Modified for Recipe Management enhancement
 *2.110.0     15-Dec-2020   Deeksha             Modified for Web Inventory enhancement
 *2.110.0     01-Jan-2020   Mushahid Faizan     Modified : Added Image field to DTO
 *2.120.0     18-May-2021   Mushahid Faizan     Modified : Added search parameter as part of Web Inventory changes
 *********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Semnox.Core.GenericUtilities;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// This is the product data object class. This acts as data holder for the product business object
    /// </summary>
    public class ProductDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new  Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// SearchByProductParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByProductParameters
        {
            /// <summary>
            /// Search by PRODUCT_ID field
            /// </summary>
            PRODUCT_ID = 0,
            /// <summary>
            /// Search by SEGMENT_CATEGORY_ID field
            /// </summary>
            SEGMENT_CATEGORY_ID = 1,
            /// <summary>
            /// Search by IS_ACTIVE field
            /// </summary>
            IS_ACTIVE = 2,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID = 3,
            /// <summary>
            /// Search by CODE field
            /// </summary>
            CODE = 4,
            /// <summary>
            /// Search by DESCRIPTION field
            /// </summary>
            DESCRIPTION = 5,
            /// <summary>
            /// Search by ISREDEEMABLE field
            /// </summary>
            ISREDEEMABLE = 6,
            /// <summary>
            /// Search by ISSELLABLE field
            /// </summary>
            ISSELLABLE = 7,
            /// <summary>
            /// Search by MARKET LIST ITEM field
            /// </summary>
            MARKET_LIST_ITEM = 8,
            /// <summary>
            /// Search by CATEGORY field
            /// </summary>
            CATEGORY = 9,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID = 10, //Added search parameter 16-May-2017
            /// <summary>
            /// Search by PRODUCT_NAME field
            /// </summary>
            PRODUCT_NAME = 11,
            ///<summary>
            ///Search by BARCODE field
            ///</summary>
            BARCODE = 12,
            ///<summary>
            ///Search by UOM_ID field
            ///</summary>
            UOMID = 13,
            ///<summary>
            ///Search by IS_PUBLISHED field
            ///</summary>
            IS_PUBLISHED = 14,
            ///<summary>
            ///Search by PRICE_IN_TICKETS field
            ///</summary>
            PRICE_IN_TICKETS = 15,
            ///<summary>
            ///Search Exact CODE
            ///</summary>
            CODE_EXACT_MATCH = 16,
            ///<summary>
            ///Search Exact PRODUCT_NAME
            ///</summary>
            PRODUCT_NAME_EXACT_MATCH = 17,
            ///<summary>
            ///Search Exact CODE
            ///</summary>
            DESCRIPTION_EXACT_MATCH = 18,
            ///<summary>
            ///Search Exact CODE
            ///</summary>
            BARCODE_EXACT_MATCH = 19,
            ///<summary>
            ///Search Manual Product Id
            ///</summary>
            MANUAL_PRODUCT_ID = 20,
             ///<summary>
             ///Search Default Vendor Id
             ///</summary>
            VENDOR_ID,
            ///<summary>
            ///Search DISPLAY GROUP NAME
            ///</summary>
            DISPLAY_GROUP_NAME,
            ///<summary>
            ///Search LOT CONTROLLABLE
            ///</summary>
            LOT_CONTROLLABLE,
            ///<summary>
            ///Search LOT Inventory UOM Id
            ///</summary>
            INVENTORY_UOM_ID,
            ///<summary>
            ///Search LOT ItemType Id
            ///</summary>
            ITEM_TYPE,
            ///<summary>
            ///Search Include In Plan
            ///</summary>
            INCLUDE_IN_PLAN,
            ///<summary>
            ///Search Manual Product Id List
            ///</summary>
            MANUAL_PRODUCT_ID_LIST,
            ///<summary>
            ///Search Manual Product Id List
            ///</summary>
            PRODUCT_ID_LIST,
            CATEGORY_ID_LIST,
            UOM_ID_LIST,
            ITEM_TYPE_ID_LIST,
            ///<summary>
            ///Search IsPurchaseable Product
            ///</summary>
            ISPURCHASEABLE,
            ///<summary>
            ///Search Default Location Id 
            ///</summary>
            DEFAULT_LOCATION_ID,
            ///<summary>
            ///Search Default Location Id 
            ///</summary>
            CODE_OR_DESCRIPTION
        }

        private int productId;
        private string code;
        private string description;
        private string remarks;
        private int categoryId;
        private int defaultLocationId;
        private double reorderPoint;
        private double reorderQuantity;
        private int uomId;
        private double masterPackQty;
        private double innerPackQty;
        private int defaultVendorId;
        private double cost;
        private double lastPurchasePrice;
        private string isRedeemable;
        private string isSellable;
        private string isPurchaseable;
        private string lastModUserId;
        private DateTime lastModDttm;
        private bool isActive;
        private double priceInTickets;
        private int outboundLocationId;
        private double salePrice;
        //int taxId;
        private string taxInclusiveCost;
        private string imageFileName;
        private double lowerLimitCost;
        private double upperLimitCost;
        private double costVariancePercentage;
        private int siteId;
        private string guid;
        private bool synchStatus;
        private int turnInPriceInTickets;
        private int segmentCategoryId;
        private int expiryDays;
        //Start update 11-Aug-2016
        //Added columns
        private int masterEntityId;
        private int customDataSetId;
        private bool lotControlled;
        private bool marketListItem;
        private string expiryType;
        private string issuingApproach;
        //End update 11-Aug-2016
        private string uomValue;
        private string barCode;
        private double itemMarkupPercent;
        private bool autoUpdateMarkup;
        private string productName;
        private string createdBy;
        private DateTime? creationDate;
        private int manualProductId;
        private int purchaseTaxId;
        private bool costIncludeTax;
        private int itemType;
        private decimal? yieldPercentage;
        private bool? includeInPlan;
        private string recipeDescription;
        private int inventoryUOMId;
        private decimal quantity;
        private int? preparationTime;
        private byte[] img;

        private List<ComboProductDTO> comboProductDTOList;
        private CustomDataSetDTO customDataSetDTO;
        private List<ProductBarcodeDTO> productBarcodeDTOList;
        private List<BOMDTO> productBOMList;

        /// <summary>
        /// Default constructor
        /// </summary>
        public ProductDTO()
        {
            log.LogMethodEntry();
            productId = -1;
            categoryId = -1;
            defaultLocationId = -1;
            uomId = -1;
            defaultVendorId = -1;
            outboundLocationId = -1;
            //taxId = -1;
            siteId = -1;
            turnInPriceInTickets = 0;
            segmentCategoryId = -1;
            isActive = true;
            isRedeemable = "N";
            isSellable = "N";
            isPurchaseable = "N";
            //Start update 11-Aug-2016
            //Added columns
            masterEntityId = -1;
            customDataSetId = -1;
            lotControlled = false;
            marketListItem = false;
            expiryType = "N";
            issuingApproach = "None";
            expiryDays = 0;
            autoUpdateMarkup = false;
            manualProductId = -1;
            purchaseTaxId = -1;
            costIncludeTax = true;
            //End update 11-Aug-2016
            itemType = -1;
            yieldPercentage = null;
            includeInPlan = null;
            recipeDescription = string.Empty;
            inventoryUOMId = -1;
            productBOMList = new List<BOMDTO>();
            preparationTime = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public ProductDTO(int productId, string code, string description, string remarks, int categoryId, int defaultLocationId,
                            double reorderPoint, double reorderQuantity, int uomId, double masterPackQty, double innerPackQty,
                            int defaultVendorId, double cost, double lastPurchasePrice, string isRedeemable, string isSellable,
                            string isPurchaseable, string lastModUserId, DateTime lastModDttm, bool isActive, double priceInTickets,
                            int outboundLocationId, double salePrice, string taxInclusiveCost, string imageFileName,
                            double lowerLimitCost, double upperLimitCost, double costVariancePercentage, int siteId, string guid,
                            bool synchStatus, int turnInPriceInTickets, int segmentCategoryId, int masterEntityId, int customDataSetId,
                            bool lotControlled, bool marketListItem, string expiryType, string issuingApproach, string uomPassed, 
                            int expiryDays, string barCode, double itemMarkupPercent, bool autoUpdateMarkup, string productName,
                            string createdBy, DateTime? creationDate, int manualProductId, int purchaseTaxId,bool costIncludeTax,
                             int itemType, decimal? yieldPercentage, bool? includeInPlan, string recipeDescription, int inventoryUOMId, int? preparationTime)
        {
            log.LogMethodEntry();
            this.productId = productId;
            this.code = code;
            this.description = description;
            this.remarks = remarks;
            this.categoryId = categoryId;
            this.defaultLocationId = defaultLocationId;
            this.reorderPoint = reorderPoint;
            this.reorderQuantity = reorderQuantity;
            this.uomId = uomId;
            this.masterPackQty = masterPackQty;
            this.innerPackQty = innerPackQty;
            this.defaultVendorId = defaultVendorId;
            this.cost = cost;
            this.lastPurchasePrice = lastPurchasePrice;
            this.isRedeemable = isRedeemable;
            this.isSellable = isSellable;
            this.isPurchaseable = isPurchaseable;
            this.lastModUserId = lastModUserId;
            this.lastModDttm = lastModDttm;
            this.isActive = isActive;
            this.priceInTickets = priceInTickets;
            this.outboundLocationId = outboundLocationId;
            this.salePrice = salePrice;
            //this.taxId = taxId;
            this.taxInclusiveCost = taxInclusiveCost;
            this.imageFileName = imageFileName;
            this.lowerLimitCost = lowerLimitCost;
            this.upperLimitCost = upperLimitCost;
            this.costVariancePercentage = costVariancePercentage;
            this.siteId = siteId;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.turnInPriceInTickets = turnInPriceInTickets;
            this.segmentCategoryId = segmentCategoryId;
            this.masterEntityId = masterEntityId;
            this.customDataSetId = customDataSetId;
            this.lotControlled = lotControlled;
            this.marketListItem = marketListItem;
            this.expiryType = expiryType;
            this.issuingApproach = issuingApproach;
            this.uomValue = uomPassed;
            this.expiryDays = expiryDays;
            this.barCode = barCode;
            this.itemMarkupPercent = itemMarkupPercent;
            this.autoUpdateMarkup = autoUpdateMarkup;
            this.productName = productName;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.manualProductId = manualProductId;
            this.purchaseTaxId = purchaseTaxId;
            this.costIncludeTax = costIncludeTax;
            this.itemType = itemType;
            this.yieldPercentage = yieldPercentage;
            this.includeInPlan = includeInPlan;
            this.recipeDescription = recipeDescription;
            this.inventoryUOMId = inventoryUOMId;
            this.preparationTime = preparationTime;
            log.LogMethodExit();
        }
        /// <summary>
        /// Get/Set method of the ProductId field
        /// </summary>
        [DisplayName("Product Id")]
        [ReadOnly(true)]
        public int ProductId { get { return productId; } set { productId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Code field
        /// </summary>
        [DisplayName("Code")]
        public string Code { get { return code; } set { code = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Description field
        /// </summary>
        [DisplayName("Description")]
        public string Description { get { return description; } set { description = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Remarks field
        /// </summary>
        [DisplayName("Remarks")]
        public string Remarks { get { return remarks; } set { remarks = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the CategoryId field
        /// </summary>
        [DisplayName("Category Id")]
        public int CategoryId { get { return categoryId; } set { categoryId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the DefaultLocationId field
        /// </summary>
        [DisplayName("Default Location Id")]
        public int DefaultLocationId { get { return defaultLocationId; } set { defaultLocationId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the ReorderPoint field
        /// </summary>
        [DisplayName("Reorder Point")]
        public double ReorderPoint { get { return reorderPoint; } set { reorderPoint = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the ReorderQuantity field
        /// </summary>
        [DisplayName("Reorder Quantity")]
        public double ReorderQuantity { get { return reorderQuantity; } set { reorderQuantity = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the UomId field
        /// </summary>
        [DisplayName("UomId")]
        public int UomId { get { return uomId; } set { uomId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the MasterPackQty field
        /// </summary>
        [DisplayName("Master Pack Qty")]
        public double MasterPackQty { get { return masterPackQty; } set { masterPackQty = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the InnerPackQty field
        /// </summary>
        [DisplayName("Inner Pack Qty")]
        public double InnerPackQty { get { return innerPackQty; } set { innerPackQty = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the DefaultVendorId field
        /// </summary>
        [DisplayName("Default Vendor Id")]
        public int DefaultVendorId { get { return defaultVendorId; } set { defaultVendorId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Cost field
        /// </summary>
        [DisplayName("Cost")]
        public double Cost { get { return cost; } set { cost = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the LastPurchasePrice field
        /// </summary>
        [DisplayName("Last Purchase Price")]
        public double LastPurchasePrice { get { return lastPurchasePrice; } set { lastPurchasePrice = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the IsRedeemable field
        /// </summary>
        [DisplayName("Redeemable?")]
        public string IsRedeemable { get { return isRedeemable; } set { isRedeemable = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the IsSellable field
        /// </summary>
        [DisplayName("Sellable?")]
        public string IsSellable { get { return isSellable; } set { isSellable = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the IsPurchaseable field
        /// </summary>
        [DisplayName("Purchaseable?")]
        public string IsPurchaseable { get { return isPurchaseable; } set { isPurchaseable = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the LastModUserId field
        /// </summary>
        [DisplayName("Last Mod User Id")]
        [Browsable(false)]
        public string LastModUserId { get { return lastModUserId; } set { lastModUserId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the LastModDttm field
        /// </summary>
        [DisplayName("Last Modified Date")]
        [Browsable(false)]
        public DateTime LastModDttm { get { return lastModDttm; } set { lastModDttm = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("Active?")]
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the PriceInTickets field
        /// </summary>
        [DisplayName("Price In Tickets")]
        public double PriceInTickets { get { return priceInTickets; } set { priceInTickets = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the OutboundLocationId field
        /// </summary>
        [DisplayName("Outbound Location Id")]
        public int OutboundLocationId { get { return outboundLocationId; } set { outboundLocationId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the SalePrice field
        /// </summary>
        [DisplayName("Sale Price")]
        public double SalePrice { get { return salePrice; } set { salePrice = value; this.IsChanged = true; } }
       // /// <summary>
        ///// Get/Set method of the TaxId field
        ///// </summary>
        //[DisplayName("TaxId")]
       // public int TaxId { get { return taxId; } set { taxId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the TaxInclusiveCost field
        /// </summary>
        [DisplayName("Tax Inclusive Cost")]
        public string TaxInclusiveCost { get { return taxInclusiveCost; } set { taxInclusiveCost = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the ImageFileName field
        /// </summary>
        [DisplayName("Image File Name")]
        public string ImageFileName { get { return imageFileName; } set { imageFileName = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the LowerLimitCost field
        /// </summary>
        [DisplayName("Lower Limit Cost")]
        public double LowerLimitCost { get { return lowerLimitCost; } set { lowerLimitCost = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the UpperLimitCost field
        /// </summary>
        [DisplayName("Upper Limit Cost")]
        public double UpperLimitCost { get { return upperLimitCost; } set { upperLimitCost = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the CostVariancePercentage field
        /// </summary>
        [DisplayName("Cost Variance Percentage")]
        public double CostVariancePercentage { get { return costVariancePercentage; } set { costVariancePercentage = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("SiteId")]
        [Browsable(false)]
        public int SiteId { get { return siteId; } set { siteId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        [Browsable(false)]
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("Synch Status")]
        [Browsable(false)]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the TurnInPriceInTickets field
        /// </summary>
        [DisplayName("TurnInPriceInTickets")]
        public int TurnInPriceInTickets { get { return turnInPriceInTickets; } set { turnInPriceInTickets = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the SegmentCategoryId field
        /// </summary>
        [DisplayName("Segment Category Id")]
        public int SegmentCategoryId { get { return segmentCategoryId; } set { segmentCategoryId = value; this.IsChanged = true; } }
        //Start update 11-Aug-2016
        //Added columns
        /// <summary>
        /// Get/Set method of the masterEntityId field
        /// </summary>
        [DisplayName("MasterEntityId")]
        [Browsable(false)]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the customDataSetId field
        /// </summary>
        [DisplayName("CustomDataSetId")]
        public int CustomDataSetId { get { return customDataSetId; } set { customDataSetId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the lotControlled field
        /// </summary>
        [DisplayName("LotControlled")]
        public bool LotControlled { get { return lotControlled; } set { lotControlled = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the MarketListItem field
        /// </summary>
        [DisplayName("MarketListItem")]
        public bool MarketListItem { get { return marketListItem; } set { marketListItem = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the expiryType field
        /// </summary>
        [DisplayName("ExpiryType")]
        public string ExpiryType { get { return expiryType; } set { expiryType = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the issuingApproach field
        /// </summary>
        [DisplayName("IssuingApproach")]
        public string IssuingApproach { get { return issuingApproach; } set { issuingApproach = value; this.IsChanged = true; } }
        //End update 11-Aug-2016

        /// <summary>
        /// Get/Set method of the MarketListItem field
        /// </summary>
        [DisplayName("Unit of Measure")]
        public string UOMValue { get { return uomValue; } set { uomValue = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ExpiryDays field
        /// </summary>
        [DisplayName("Expiry Days")]
        public int ExpiryDays { get { return expiryDays; } set { expiryDays = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the BarCode field
        /// </summary>
        [DisplayName("BarCode")]
        public string BarCode { get { return barCode; } set { barCode = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Purchase Tax Id field
        /// </summary>
        [DisplayName("Purchase Tax Id")]
        public int PurchaseTaxId { get { return purchaseTaxId; } set { purchaseTaxId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ComboProductDTOList Tax Id field
        /// </summary>
        [DisplayName("ComboProductDTOList")]
        public List<ComboProductDTO> ComboProductDTOList { get { return comboProductDTOList; } set { comboProductDTOList = value; this.IsChanged = true; } }
        
        /// <summary>
        /// Get/Set method of the Cost Include Tax field
        /// </summary>
        [DisplayName("Cost Include Tax")]
        public bool CostIncludesTax { get { return costIncludeTax; } set { costIncludeTax = value; this.IsChanged = true; }  }
        
        /// <summary>
        /// Get/Set method of the yieldPercentage field
        /// </summary>
        [DisplayName("YieldPercentage")]
        public decimal? YieldPercentage { get { return yieldPercentage; } set { yieldPercentage = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the IncludeInPlan field
        /// </summary>
        [DisplayName("IncludeInPlan")]
        public bool? IncludeInPlan { get { return includeInPlan; } set { includeInPlan = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ItemType field
        /// </summary>
        [DisplayName("ItemType")]
        public int ItemType { get { return itemType; } set { itemType = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the RecipeDescription field
        /// </summary>
        [DisplayName("RecipeDescription")]
        public string RecipeDescription { get { return recipeDescription; } set { recipeDescription = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the InventoryUOMId field
        /// </summary>
        [DisplayName("InventoryUOMId")]
        public int InventoryUOMId { get { return inventoryUOMId; } set { inventoryUOMId = value; this.IsChanged = true; } }
        
        /// <summary>
        /// Get/Set method of the PreparationTime field
        /// </summary>
        [DisplayName("PreparationTime")]
        public int? PreparationTime {get { return preparationTime; } set { preparationTime = value; this.IsChanged = true; } }

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
                    return notifyingObjectIsChanged || productId < 0;
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
        /// Returns whether customdataset or any child record is changed
        /// </summary>
        [Browsable(false)]
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (CustomDataSetDTO != null &&
                    CustomDataSetDTO.IsChangedRecursive)
                {
                    return true;
                }
                if (productBarcodeDTOList != null &&
                   productBarcodeDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                if(productBOMList != null &&
                    productBOMList.Any(x => x.IsChanged))
                {
                    return true;
                }
                return false;
            }
        }
        /// <summary>
        /// Get/Set method of the ItemMarkupPercent field
        /// </summary>
        [DisplayName("Item Markup Percent")]
        public double ItemMarkupPercent { get { return itemMarkupPercent; } set { itemMarkupPercent = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the AutoUpdateMarkup field
        /// </summary>
        [DisplayName("AutoUpdateMarkup")]
        public bool AutoUpdateMarkup { get { return autoUpdateMarkup; } set { autoUpdateMarkup = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the ProductName field
        /// </summary>
        [DisplayName("ProductName")]
        public string ProductName { get { return productName; } set { productName = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        public string CreatedBy { get { return createdBy; } set { createdBy = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        public DateTime? CreationDate { get { return creationDate; } set { creationDate = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the ManualProductId field
        /// </summary>
        public int ManualProductId { get { return manualProductId; } set { manualProductId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the CustomDataSetDTO field
        /// </summary>
        public CustomDataSetDTO CustomDataSetDTO { get { return customDataSetDTO; } set { customDataSetDTO = value; } }
        /// <summary>
        /// Get/Set method of the ProductBarcodeDTOList field
        /// </summary>
        public List<ProductBarcodeDTO> ProductBarcodeDTOList { get { return productBarcodeDTOList; } set { productBarcodeDTOList = value; } }

        /// <summary>
        /// Get/Set method of the ProductBarcodeDTOList field
        /// </summary>
        public List<BOMDTO> ProductBOMDTOList { get { return productBOMList; } set { productBOMList = value; } }

        /// <summary>
        /// Get/Set method of the Quantity field
        /// </summary>
        public decimal Quantity { get { return quantity; } set { quantity = value; } }

        /// <summary>
        /// Get/Set method of the Image field
        /// </summary>

        public byte[] Image { get { return img; } set { img = value; this.IsChanged = true; } }

        /// <summary>
        /// AcceptChanges
        /// </summary>
        public void AcceptChanges()
        {
            log.LogMethodEntry();
            this.IsChanged = false;
            log.LogMethodExit();
        }
    }
}
