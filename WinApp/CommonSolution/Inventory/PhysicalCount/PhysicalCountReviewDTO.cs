/********************************************************************************************
 * Project Name - PhysicalCountReview
 * Description  - DTO of PhysicalCountReview
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By      Remarks          
 *********************************************************************************************
 *2.70.2        19-Aug-2019   Deeksha        Added logger methods.
 *2.70.2        26-Dec-2019   Deeksha        Inventory Next-Rel Enhancement changes.
 *2.90.0        28-Jul-2020   Deeksha        Physical Count Lot controllable products Issue fix
 *2.100.0       14-Sep-2020   Deeksha        Added UOMID field.
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.Inventory
{
    /// <summary>
    /// This is the PhysicalCountReview data object class. This acts as data holder for the PhysicalCountReview object
    /// </summary>
    public class PhysicalCountReviewDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// SearchByPhysicalCountReviewParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByPhysicalCountReviewParameters
        {
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID ,

            /// <summary>
            /// Search by LOCATIONID field
            /// </summary>
            LOCATIONID ,

            /// <summary>
            /// Search by PRODUCT_ID field
            /// </summary>
            PRODUCT_ID,

            /// <summary>
            /// Search by INVENTORYITEMSONLY field
            /// </summary>
            INVENTORYITEMSONLY ,

            /// <summary>
            /// Search by CODE field
            /// </summary>
            CODE ,

            /// <summary>
            /// Search by DESCRIPTION field
            /// </summary>
            DESCRIPTION ,

            /// <summary>
            /// Search by BARCODE field
            /// </summary>
            BARCODE ,

            /// <summary>
            /// Search by CATEGORY field
            /// </summary>
            CATEGORYID ,

            /// <summary>
            /// Search by INVENTORYQTY field
            /// </summary>
            INVENTORYQTY,
            /// <summary>
            /// Search by UOM_ID field
            /// </summary>
            UOM_ID,
            /// <summary>
            /// Search by SYMBOL field
            /// </summary>
            SYMBOL,
            /// <summary>
            /// Search by Quantity field
            /// </summary>
            Quantity
        }

        private string code;
        private string description;
        private string category;
        private string barcode;
        private string sku;
        private string lotNumber;
        private string location;
        private double startingQuantity;
        private double updatedPhysicalQuantity;
        private double currentInventoryQuantity;
        private double newQuantity;
        private string physicalCountRemarks;
        private int productID;
        private int locationID;
        private int lotID;
        private int siteID;
        private int categoryID;
        private string isPurchaseable;
        private string remarksMandatory;
        private int historyId;
        private bool lotControlled;
        private string uom;
        private int uomId;
        private int physicalCountId;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public PhysicalCountReviewDTO()
        {
            log.LogMethodEntry();
            category = "";
            locationID = -1;
            siteID = -1;
            categoryID = -1;
            newQuantity = -1;
            physicalCountRemarks = "";
            remarksMandatory = "N";
            historyId = -1;
            uomId = -1;
            physicalCountId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor with all the data fields
        /// </summary>
        public PhysicalCountReviewDTO(string code, string description, string category, string barcode, string sku, string lotNumber,
                                        string location, double startingQuantity, double updatedPhysicalQuantity,
                                        double currentInventoryQuantity, double newQuantity,
                                        string physicalCountRemarks, int productID, int locationID, int lotID,
                                        int siteID, int categoryID, string isPurchaseable, string remarksMandatory, int historyId ,bool lotControlled,
                                        string uom, int uomId, int physicalCountId)
        {
            log.LogMethodEntry(code, description, category, barcode, sku, lotNumber, location, startingQuantity, updatedPhysicalQuantity,
                               currentInventoryQuantity, newQuantity, physicalCountRemarks, productID, locationID, lotID, siteID, categoryID,
                               isPurchaseable, remarksMandatory, historyId, lotControlled, uom, uomId, physicalCountId);
            this.code = code;
            this.description = description;
            this.category = category;
            this.barcode = barcode;
            this.sku = sku;
            this.lotNumber = lotNumber;
            this.location = location;
            this.startingQuantity = startingQuantity;
            this.updatedPhysicalQuantity = updatedPhysicalQuantity;
            this.currentInventoryQuantity = currentInventoryQuantity;
            this.newQuantity = newQuantity;
            this.physicalCountRemarks = physicalCountRemarks;
            this.productID = productID;
            this.locationID = locationID;
            this.lotID = lotID;
            this.siteID = siteID;
            this.categoryID = categoryID;
            this.isPurchaseable = isPurchaseable;
            this.remarksMandatory = remarksMandatory;
            this.historyId = historyId;
            this.lotControlled = lotControlled;
            this.uom = uom;
            this.uomId = uomId;
            this.physicalCountId = physicalCountId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the Code field
        /// </summary>
        [DisplayName("Code")]
        [ReadOnly(true)]
        public string Code { get { return code; } set { code = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the Description field
        /// </summary>
        [DisplayName("Description")]
        [ReadOnly(true)]
        public string Description { get { return description; } set { description = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the Category field
        /// </summary>
        [DisplayName("Category")]
        [ReadOnly(true)]
        public string Category { get { return category; } set { category = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the Bar code field
        /// </summary>
        [DisplayName("Bar code")]
        [ReadOnly(true)]
        public string Barcode { get { return barcode; } set { barcode = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the SKU field
        /// </summary>
        [DisplayName("SKU")]
        [ReadOnly(true)]
        public string SKU { get { return sku; } set { sku = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the LotNumber field
        /// </summary>
        [DisplayName("LotNumber")]
        [ReadOnly(true)]
        public string LotNumber { get { return lotNumber; } set { lotNumber = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the Location field
        /// </summary>
        [DisplayName("Location")]
        [ReadOnly(true)]
        public string Location { get { return location; } set { location = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the StartingQuantity field
        /// </summary>
        [DisplayName("Starting Quantity")]
        [ReadOnly(true)]
        public double StartingQuantity { get { return startingQuantity; } set { startingQuantity = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the UpdatedPhysicalQuantity field
        /// </summary>
        [DisplayName("Updated Physical Quantity")]
        [ReadOnly(true)]
        public double UpdatedPhysicalQuantity { get { return updatedPhysicalQuantity; } set { updatedPhysicalQuantity = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the CurrentInventoryQuantity field
        /// </summary>
        [DisplayName("Current Inventory Quantity")]
        [ReadOnly(true)]
        public double CurrentInventoryQuantity { get { return currentInventoryQuantity; } set { currentInventoryQuantity = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the NewQuantity field
        /// </summary>
        [DisplayName("New Quantity")]
        public double NewQuantity { get { return newQuantity; } set { newQuantity = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the PhysicalCountRemarks field
        /// </summary>
        [DisplayName("Remarks")]
        public string PhysicalCountRemarks { get { return physicalCountRemarks; } set { physicalCountRemarks = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the ProductID field
        /// </summary>
        [DisplayName("Product ID")]
        [ReadOnly(true)]
        public int ProductID { get { return productID; } set { productID = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the LocationID field
        /// </summary>
        [DisplayName("Location ID")]
        [ReadOnly(true)]
        public int LocationID { get { return locationID; } set { locationID = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the LotID field
        /// </summary>
        [DisplayName("Lot ID")]
        [ReadOnly(true)]
        public int LotID { get { return lotID; } set { lotID = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the Site_ID field
        /// </summary>
        [DisplayName("Site ID")]
        [ReadOnly(true)]
        public int Site_ID { get { return siteID; } set { siteID = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CategoryID field
        /// </summary>
        [DisplayName("CategoryID")]
        [ReadOnly(true)]
        public int CategoryID { get { return categoryID; } set { categoryID = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the IsPurchaseable field
        /// </summary>
        [DisplayName("IsPurchaseable")]
        [ReadOnly(true)]
        public string IsPurchaseable { get { return isPurchaseable; } set { isPurchaseable = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the RemarksMandatory field
        /// </summary>
        [DisplayName("RemarksMandatory")]
        [ReadOnly(true)]
        public string RemarksMandatory { get { return remarksMandatory; } set { remarksMandatory = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the HistoryId field
        /// </summary>
        [DisplayName("HistoryId")]
        [ReadOnly(true)]
        public int HistoryId { get { return historyId; } set { historyId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the lotControlled field
        /// </summary>
        [DisplayName("lotControlled")]
        [ReadOnly(true)]
        public bool LotControlled { get { return lotControlled; } set { lotControlled = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the UOM field
        /// </summary>
        [DisplayName("UOM")]
        [ReadOnly(true)]
        public string UOM { get { return uom; } set { uom = value; this.IsChanged = true; } }


        /// <summary>
        /// Get method of the uomId field
        /// </summary>
        [DisplayName("uomId")]
        [ReadOnly(true)]
        public int UOMId { get { return uomId; } set { uomId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the PhysicalCountId field
        /// </summary>
        public int PhysicalCountId { get { return physicalCountId; } set { physicalCountId = value; this.IsChanged = true; } }


        /// <summary>
        ///  Get/Set method to track changes to the object
        /// </summary>
        [Browsable(false)]
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
