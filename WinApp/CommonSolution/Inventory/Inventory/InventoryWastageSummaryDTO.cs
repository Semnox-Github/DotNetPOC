/********************************************************************************************
 * Project Name - Inventory
 * Description  -Data object of Inventory  Wastage Summary 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
  *2.70.2     28-Nov-2019    Girish Kundar          Created
  *2.100.0    16-Aug-2020    Deeksha                Modified for Recipe Management enhancement.
 ********************************************************************************************/
using System;
using Semnox.Core.Utilities;
using Semnox.Parafait.logging;

namespace Semnox.Parafait.Inventory
{
    public class InventoryWastageSummaryDTO
    {
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByInventoryParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByInventoryWastageParameters
        {
            /// <summary>
            /// Search by PRODUCT ID field
            /// </summary>
            PRODUCT_ID,
            /// <summary>
            /// Search by LOCATION ID field
            /// </summary>
            LOCATION_ID,
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by CATEGORY field
            /// </summary>
            CATEGORY,
            /// <summary>
            /// Search by WASTAGE_FROM_DATE field
            /// </summary>
            WASTAGE_FROM_DATE,
            /// <summary>
            /// Search by WASTAGE_TO_DATE ONLY field
            /// </summary>
            WASTAGE_TO_DATE,
            /// <summary>
            /// Search by PRODUCT DESCRIPTION field
            /// </summary>
            PRODUCT_DESCRIPTION,
            /// <summary>
            /// Search by CODE field
            /// </summary>
            CODE,
            /// <summary>
            /// Search by BARCODE field
            /// </summary>
            BARCODE,
            /// <summary>
            /// Search by INVENTORY ID field
            /// </summary>
            INVENTORY_ID
           
        }

        private int productId;
        private int categoryId;
        private int uomId;
        private int inventoryId;
        private int adjustmentId;
        private string productCode;
        private string locationName;
        private string uom;
        private string category;
        private string productDescription;
        private string productName;
        private int fromLocationId;
        private decimal originalQuantity;
        private decimal wastageQuantity;
        private decimal todaysWastageQuantity;
        private decimal availableQuantity;
        private string remarks;
        private int lotId;
        private DateTime wastageAdjustedDate;
        private int siteId;
        private string adjustmentUpdatedBy;
        private DateTime adjustmentUpdateDate;
        private string lotNumber;
        private int adjustmentTypeId;
        /// <summary>
        /// Default constructor
        /// </summary>
        public InventoryWastageSummaryDTO()
        {
            log.LogMethodEntry();
            productId = -1;
            categoryId = -1;
            uomId = -1;
            inventoryId = -1;
            adjustmentId = -1;
            originalQuantity = 0;
            productCode = string.Empty;
            fromLocationId = -1;
            productDescription = string.Empty;
            wastageQuantity = 0;
            availableQuantity = 0;
            todaysWastageQuantity = 0;
            lotId = -1;
            uom = string.Empty;
            category = string.Empty;
            productName = string.Empty;
            locationName = string.Empty;
            remarks = string.Empty;
            wastageAdjustedDate = ServerDateTime.Now;
            siteId = -1;
            adjustmentTypeId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with Required fields
        /// </summary>
        public InventoryWastageSummaryDTO(int productId,string category,string uom, int adjustmentId ,int inventoryId,
                                          string productCode, string productDescription,int fromLocationId,
                                          string locationName, decimal wastageQuantity, decimal availableQuantity,
                                          decimal originalQuantity,string remarks,int lotId, string lastUpdatedBy,
                                          DateTime lastUpdateDate,int siteId,int categoryId , int uomId,
                                          DateTime wastageAdjustedDate, decimal todaysWastageQuantity,string lotNumber,int adjustmentTypeId)
            : this()
        {
            log.LogMethodEntry(productId, category , uom,adjustmentId, inventoryId ,productCode,  productDescription,
                                fromLocationId, locationName, wastageQuantity, availableQuantity, originalQuantity,
                                remarks, lotId, lastUpdatedBy,  lastUpdateDate,siteId, categoryId,uomId,
                                wastageAdjustedDate, todaysWastageQuantity, lotNumber, adjustmentTypeId);
            this.productId = productId;
            this.uom = uom;
            this.category = category;
            this.inventoryId = inventoryId;
            this.locationName = locationName;
            this.adjustmentId = adjustmentId;
            this.originalQuantity = originalQuantity;
            this.productCode = productCode;
            this.productDescription = productDescription;
            this.fromLocationId = fromLocationId;
            this.wastageQuantity = wastageQuantity;
            this.availableQuantity = availableQuantity;
            this.remarks = remarks;
            this.lotId = lotId;
            this.adjustmentUpdatedBy = lastUpdatedBy;
            this.adjustmentUpdateDate = lastUpdateDate;
            this.siteId = siteId;
            this.categoryId = categoryId;
            this.uomId = uomId;
            this.wastageAdjustedDate = wastageAdjustedDate;
            this.todaysWastageQuantity = todaysWastageQuantity;
            this.lotNumber = lotNumber;
            this.adjustmentTypeId = adjustmentTypeId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get /Set method for ProductId
        /// </summary>
        public int ProductId
        {
            get  {  return productId; }
            set  {  productId = value; IsChanged = true; }
        }

        /// <summary>
        /// Get /Set method for CategoryId
        /// </summary>
        public int CategoryId
        {
            get { return categoryId; }
            set { categoryId = value; IsChanged = true; }
        }

        /// <summary>
        /// Get /Set method for UomId
        /// </summary>
        public int UomId
        {
            get { return uomId; }
            set { uomId = value; IsChanged = true; }
        }

        /// <summary>
        /// Get /Set method for SiteId
        /// </summary>
        public int SiteId
        {
            get { return siteId; }
            set { siteId = value; }
        }

        /// <summary>
        /// Get /Set method for inventoryId
        /// </summary>
        public int InventoryId
        {
            get { return inventoryId; }
            set { inventoryId = value; IsChanged = true; }
        }


        /// <summary>
        /// Get /Set method for productName
        /// </summary>
        public string ProductName
        {
            get { return productName; }
            set { productName = value; IsChanged = true; }
        }

        /// <summary>
        /// Get /Set method for location Name
        /// </summary>
        public string LocationName
        {
            get { return locationName; }
            set { locationName = value; IsChanged = true; }
        }
        /// <summary>
        /// Get /Set method for lot Number Name
        /// </summary>
        public string LotNumber
        {
            get { return lotNumber; }
            set { lotNumber = value; IsChanged = true; }
        }
        /// <summary>
        /// Get /Set method for UOM
        /// </summary>
        public string UOM
        {
            get { return uom; }
            set { uom = value; IsChanged = true; }
        }

        /// <summary>
        /// Get /Set method for Category
        /// </summary>
        public string Category
        {
            get { return category; }
            set { category = value; IsChanged = true; }
        }
        /// <summary>
        /// Get /Set method for adjustmentId
        /// </summary>
        public int AdjustmentId
        {
            get { return adjustmentId; }
            set { adjustmentId = value; IsChanged = true; }
        }

        /// <summary>
        /// Get /Set method for original quantity
        /// </summary>
        public decimal OriginalQuantity
        {
            get { return originalQuantity; }
            set { originalQuantity = value;  }
        }
        /// <summary>
        /// Get /Set method for todays Wastage Quantity 
        /// </summary>
        public decimal TodaysWastageQuantity
        {
            get { return todaysWastageQuantity; }
            set { todaysWastageQuantity = value;  }
        }
        /// <summary>
        /// Get /Set method for LotId
        /// </summary>
        public int LotId
        {
            get { return lotId; }
            set { lotId = value; IsChanged = true; }
        }

        /// <summary>
        /// Get /Set method for ProductCode
        /// </summary>
        public string ProductCode
        {
            get { return productCode; }
            set { productCode = value;  }
        }

        /// <summary>
        /// Get /Set method for ProductDescription
        /// </summary>
        public string ProductDescription
        {
            get { return productDescription; }
            set { productDescription = value; IsChanged = true; }
        }

        /// <summary>
        /// Get /Set method for LocationId
        /// </summary>
        public int LocationId
        {
            get { return fromLocationId; }
            set { fromLocationId = value; IsChanged = true; }
        }

        /// <summary>
        /// Get /Set method for WastageQuantity
        /// </summary>
        public decimal WastageQuantity
        {
            get { return wastageQuantity; }
            set { wastageQuantity = value; IsChanged = true; }
        }

        /// <summary>
        /// Get /Set method for AvailableQuantity
        /// </summary>
        public decimal AvailableQuantity
        {
            get { return availableQuantity; }
            set { availableQuantity = value; IsChanged = true; }
        }

        /// <summary>
        /// Get /Set method for Remarks
        /// </summary>
        public string Remarks
        {
            get { return remarks; }
            set { remarks = value;}
        }

        /// <summary>
        /// Get/Set method of the adjustmentUpdateDate field
        /// </summary>
        public DateTime AdjustmentUpdateDate
        {
            get   {  return adjustmentUpdateDate; }
            set   {  adjustmentUpdateDate = value; }
        }
        /// <summary>
        /// Get/Set method of the wastageAdjustedDate field
        /// </summary>
        public DateTime WastageAdjustedDate
        {
            get   {  return wastageAdjustedDate; }
            set   { wastageAdjustedDate = value; }
        }

        /// <summary>
        /// Get/Set method of the adjustmentUpdatedBy field
        /// </summary>
        public string AdjustmentUpdatedBy
        {
            get  { return adjustmentUpdatedBy; }
            set  { adjustmentUpdatedBy = value;  }
        }

        /// <summary>
        /// Get/Set method of the AdjustementTypeId field
        /// </summary>
        public int AdjustmentTypeId
        {
            get { return adjustmentTypeId; }
            set { adjustmentTypeId = value; IsChanged = true; }
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
