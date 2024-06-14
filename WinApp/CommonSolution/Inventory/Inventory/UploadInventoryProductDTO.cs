/********************************************************************************************
 * Project Name - Inventory                                                                          
 * Description  - Mirror Object of ProductDTO 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.0    28-Jun-2019      Mehraj        Created
 *2.90.0    02-Jul-2020      Deeksha       Inventory process : Weighted Avg Costing changes.
 *2.100.0   28-Jul-2020      Deeksha       modified for Recipe Management enhancement
 ********************************************************************************************/
using System;
using System.Collections.Generic;

namespace Semnox.Parafait.Inventory
{

    public class UploadInventoryProductDTO
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// SearchByUploadInventoryProduct enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByUploadInventoryProductParameters
        {
            /// <summary>
            /// Search by CODE field
            /// </summary>
            CODE,

            /// <summary>
            /// Search by PRODUCT NAME field
            /// </summary>
            PRODUCT_NAME,

            /// <summary>
            /// Search by CATEGORY NAME field
            /// </summary>
            CATEGORY_NAME,

            /// <summary>
            /// Search by BAR CODE field
            /// </summary>
            BAR_CODE,

            /// <summary>
            /// Search by HSNSAC CODE field
            /// </summary>
            HSNSAC_CODE

        }

        private string code;
        private string productName;
        private string description;
        private string categoryName;
        private double priceInTickets;
        private double cost;
        private double reorderPoint;
        private double reorderQty;
        private decimal salePrice;
        private string vendorName;
        private string barCode;
        private bool lotControlled;
        private bool marketListItem;
        private string expiryType;
        private string issuingApproach;
        private int expiryDays;
        private string remarks;
        private string redeemable;
        private string sellable;
        private string uom;
        private double itemMarkUp;
        private bool autoUpdatePit;
        private string displayInPos;
        private string displayGroup;
        private string hsnSacCode;
        private double openingQty;
        private double receivePrice;
        private string expiryDate;
        private Dictionary<string, string> customSegmentDefinitionList;
        private string salesTax;
        private bool costIncludesTax;
        private string itemType;
        private decimal? yieldPercentage;
        private bool? includeInPlan;
        private string recipeDescription;
        private string inventoryUOM;
        private int? preparationTime;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public UploadInventoryProductDTO()
        {
            log.LogMethodEntry();
            //vendorId = -1;
            //categoryId = -1;
            customSegmentDefinitionList = new Dictionary<string, string>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        public UploadInventoryProductDTO(string code, string productName, string description, string categoryName, double priceInTickets,
            double cost, double reorderPoint, double reorderQty, decimal salePrice, string vendorName, string barCode, bool lotControlled,
            bool marketListItem, string expiryType, string issuingApproach, int expiryDays, string remarks, string redeemable, 
            string sellable, string uom, double itemMarkUp,  bool autoUpdatePit, string displayInPos, string displayGroup,
            string hsnSacCode, double openingQty, double receivePrice, string expiryDate,string taxName, bool costIncludesTax,
            string itemType, decimal? yieldPercentage, bool? includeInPlan, string recipeDescription, string inventoryUOM, int? preparationTime
            )
        {
            /// If the input parameter got removed or add into this constructor, Then in UploadInventoryDataHandler().GetUploadInventoryProductDTO() method : inventoryProductColumnCount should be updated.

            log.LogMethodEntry(code, productName, description, categoryName, priceInTickets,
             cost, reorderPoint, reorderQty, salePrice, vendorName, barCode, lotControlled,
             marketListItem, expiryType, issuingApproach, expiryDays, remarks, redeemable, sellable, uom, itemMarkUp,
             autoUpdatePit, displayInPos, displayGroup, hsnSacCode, openingQty, receivePrice, expiryDate, taxName);
            this.code = code;
            this.productName = productName;
            this.description = description;
            this.categoryName = categoryName;
            this.priceInTickets = priceInTickets;
            this.cost = cost;
            this.reorderPoint = reorderPoint;
            this.reorderQty = reorderQty;
            this.salePrice = salePrice;
            this.vendorName = vendorName;
            this.barCode = barCode;
            this.lotControlled = lotControlled;
            this.marketListItem = marketListItem;
            //this.expiryType = expiryDate;
            this.issuingApproach = issuingApproach;
            this.expiryDays = expiryDays;
            this.remarks = remarks;
            this.redeemable = redeemable;
            this.sellable = sellable;
            this.uom = uom;
            this.itemMarkUp = itemMarkUp;
            this.autoUpdatePit = autoUpdatePit;
            this.displayInPos = displayInPos;
            this.displayGroup = displayGroup;
            this.hsnSacCode = hsnSacCode;
            this.openingQty = openingQty;
            this.receivePrice = receivePrice;
            this.expiryDate = expiryDate;
            this.salesTax = taxName;
            this.costIncludesTax = costIncludesTax;
            this.itemType = itemType;
            this.yieldPercentage = yieldPercentage;
            this.includeInPlan = includeInPlan;
            this.recipeDescription = recipeDescription;
            this.inventoryUOM = inventoryUOM;
            this.preparationTime = preparationTime;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the Code field
        /// </summary>
        public string Code { get { return code; } set { code = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the ProductName field
        /// </summary>
        public string ProductName { get { return productName; } set { productName = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Description field
        /// </summary>
        public string Description { get { return description; } set { description = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the CategoryName field
        /// </summary>
        public string CategoryName { get { return categoryName; } set { categoryName = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the PriceInTickets field
        /// </summary>
        public double PriceInTickets { get { return priceInTickets; } set { priceInTickets = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Cost field
        /// </summary>
        public double Cost { get { return cost; } set { cost = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the ReorderPointfield
        /// </summary>
        public double ReorderPoint { get { return reorderPoint; } set { reorderPoint = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the ReorderQty field
        /// </summary>
        public double ReorderQty { get { return reorderQty; } set { reorderQty = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the SalePrice field
        /// </summary>
        public decimal SalePrice { get { return salePrice; } set { salePrice = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Vendor field
        /// </summary>
        public string Vendor { get { return vendorName; } set { vendorName = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the BarCode field
        /// </summary>
        public string BarCode { get { return barCode; } set { barCode = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the LotControlled field
        /// </summary>
        public bool LotControlled { get { return lotControlled; } set { lotControlled = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the MarketListItem field
        /// </summary>
        public bool MarketListItem { get { return marketListItem; } set { marketListItem = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the ExpiryType field
        /// </summary>
        public string ExpiryType { get { return expiryType; } set { expiryType = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the IssuingApproach field
        /// </summary>
        public string IssuingApproach { get { return issuingApproach; } set { issuingApproach = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the ExpiryDays field
        /// </summary>
        public int ExpiryDays { get { return expiryDays; } set { expiryDays = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Remarks field
        /// </summary>
        public string Remarks { get { return remarks; } set { remarks = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Redeemable field
        /// </summary>
        public string Redeemable { get { return redeemable; } set { redeemable = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Sellable field
        /// </summary>
        public string Sellable { get { return sellable; } set { sellable = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Uom field
        /// </summary>
        public string Uom { get { return uom; } set { uom = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the ItemMarkUp field
        /// </summary>
        public double ItemMarkUp { get { return itemMarkUp; } set { itemMarkUp = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the AutoUpdatePit field
        /// </summary>
        public bool AutoUpdatePit { get { return autoUpdatePit; } set { autoUpdatePit = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the DisplayInPos field
        /// </summary>
        public string DisplayInPos { get { return displayInPos; } set { displayInPos = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the DisplayGroup field
        /// </summary>
        public string DisplayGroup { get { return displayGroup; } set { displayGroup = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the HsnSacCode field
        /// </summary>
        public string HsnSacCode { get { return hsnSacCode; } set { hsnSacCode = value; this.IsChanged = true; } }
        public string SalesTax { get { return salesTax; } set { salesTax = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the OpeningQty field
        /// </summary>
        public double OpeningQty { get { return openingQty; } set { openingQty = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the ReceivePrice field
        /// </summary>
        public double ReceivePrice { get { return receivePrice; } set { receivePrice = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the ExpiryDate field
        /// </summary>
        public string ExpiryDate { get { return expiryDate; } set { expiryDate = value; this.IsChanged = true; } }
        
        /// <summary>
        /// Get/Set method of the CostIncludesTax field
        /// </summary>
        public bool CostIncludesTax { get { return costIncludesTax; } set { costIncludesTax = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CustomSegmentDefinitionList field
        /// </summary>
        public Dictionary<string, string> CustomSegmentDefinitionList { get { return customSegmentDefinitionList; } set { customSegmentDefinitionList = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the yieldPercentage field
        /// </summary>
        public decimal? YieldPercentage { get { return yieldPercentage; } set { yieldPercentage = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the IncludeInPlan field
        /// </summary>
        public bool? IncludeInPlan { get { return includeInPlan; } set { includeInPlan = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ItemType field
        /// </summary>
        public string ItemType { get { return itemType; } set { itemType = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the RecipeDescription field
        /// </summary>
        public string RecipeDescription { get { return recipeDescription; } set { recipeDescription = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the InventoryUOMI field
        /// </summary>
        public string InventoryUOM { get { return inventoryUOM; } set { inventoryUOM = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the PreparationTime field
        /// </summary>
        public int? PreparationTime { get { return preparationTime; } set { preparationTime = value; this.IsChanged = true; } }



        /// <summary>
        /// Get/Set method of the AttributeCount field
        /// </summary>
        public static int AttributeCount
        {
            get
            {
                Type type = typeof(UploadInventoryProductDTO);
                int x = type.GetProperties().Length - 3;
                return x;
            }
        }
        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged;
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
        public void AcceptChanges()
        {
            log.LogMethodEntry();
            this.IsChanged = false;
            log.LogMethodExit();
        }
    }
}
