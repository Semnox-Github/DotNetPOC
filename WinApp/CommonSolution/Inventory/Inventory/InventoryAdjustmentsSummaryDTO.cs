/********************************************************************************************
 * Project Name -Inventory AdjustmentsSummary DTO
 * Description  -Data object of AdjustmentsSummary
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By               Remarks          
 *********************************************************************************************
  *2.70.2       07-Aug-2019    Deeksha                modifications as per 3 tier standards
  *2.100.0      27-Jul-2020    Deeksha                Modified : Added UOMId field.
 ********************************************************************************************/
using Semnox.Parafait.logging;
using System.ComponentModel;

namespace Semnox.Parafait.Inventory
{

    /// <summary>
    /// This is the summary inventory Adjustments data object class. This acts as data holder for the summary inventory Adjustments object
    /// </summary>
    public class InventoryAdjustmentsSummaryDTO
    {
        
        private static readonly Logger log = new Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// SearchByInventoryAdjustmentsSummaryParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByInventoryAdjustmentsSummaryParameters
        {
            ///<summary>
            ///Search by PRODUCT CODE field
            ///</summary>
            PRODUCT_CODE,
            ///<summary>
            ///Search by DESCRIPTION field
            ///</summary>
            DESCRIPTION ,
            ///<summary>
            ///Search by LOCATION ID field
            ///</summary>
            LOCATION_ID ,
            ///<summary>
            ///Search ny BARCODE field
            ///</summary>
            BARCODE ,
            ///<summary>
            ///Search by  PURCHASEABLE field
            ///</summary>
            PURCHASEABLE ,
            ///<summary>
            ///Search by SITE ID field
            ///</summary>
            SITE_ID,
            /// <summary>
            /// Search by PRODUCT ID LIST field
            /// </summary>
            PRODUCT_ID_LIST
        }

        private string code;
        private string barCode;
        private int lotId;
        private string lotNumber;
        private string description;
        private string sKU;
        private string locationName;
        private double avlQuantity;
        private double allocated;
        private double totalCost;
        private double reOrderQuantity;
        private double reOrderPoint;
        private int productId;
        private int locationId;
        private bool lotControlled;
        private int uomId;

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public InventoryAdjustmentsSummaryDTO(string Code, string Barcode, int LotId, string LotNumber, string Description, string SKU,
                                            string LocationName, double AvlQty, double Allocated, double TotalCost,
                                            double ReorderQuantity, double ReorderPoint, int ProductId, int LocationId, bool LotControlled,int uomId)
        {
            log.LogMethodEntry( Code,  Barcode,  LotId,  LotNumber,  Description,  SKU, LocationName,  AvlQty,  Allocated,  TotalCost,
                                             ReorderQuantity,  ReorderPoint,  ProductId,  LocationId,  LotControlled);
            this.code = Code;
            this.barCode = Barcode;
            this.lotId = LotId;
            this.lotNumber = LotNumber;
            this.description = Description;
            this.sKU = SKU;
            this.locationName = LocationName;
            this.avlQuantity = AvlQty;
            this.allocated = Allocated;
            this.totalCost = TotalCost;
            this.reOrderQuantity = ReorderQuantity;
            this.reOrderPoint = ReorderPoint;
            this.productId = ProductId;
            this.locationId = LocationId;
            this.lotControlled = LotControlled;
            this.uomId = uomId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the code field
        /// </summary>
        [DisplayName("Code")]
        public string Code
        {
            get
            {
                return code;
            }
        }

        /// <summary>
        /// Get/Set method of the Barcode field
        /// </summary>
        [DisplayName("Barcode")]
        public string Barcode
        {
            get
            {
                return barCode;
            }
        }
        /// <summary>
        /// Get/Set method of the LotId field
        /// </summary>
        [DisplayName("LotId")]
        public int LotId
        {
            get
            {
                return lotId;
            }
        }
        /// <summary>
        /// Get/Set method of the LotNumber field
        /// </summary>
        [DisplayName("LotNumber")]
        public string LotNumber
        {
            get
            {
                return lotNumber;
            }
        }
        /// <summary>
        /// Get/Set method of the Description field
        /// </summary>
        [DisplayName("Description")]
        public string Description
        {
            get
            {
                return description;
            }
        }
        /// <summary>
        /// Get/Set method of the SKU field
        /// </summary>
        [DisplayName("SKU")]
        public string SKU
        {
            get
            {
                return sKU;
            }
        }
        /// <summary>
        /// Get/Set method of the LocationName field
        /// </summary>
        [DisplayName("Location_Name")]
        public string LocationName
        {
            get
            {
                return locationName;
            }
        }
        /// <summary>
        /// Get/Set method of the AvlQuantity field
        /// </summary>
        [DisplayName("Avl_Qty")]
        public double AvlQuantity
        {
            get
            {
                return avlQuantity;
            }
        }
        /// <summary>
        /// Get/Set method of the Allocated field
        /// </summary>
        [DisplayName("Allocated")]
        public double Allocated
        {
            get
            {
                return allocated;
            }
        }
        /// <summary>
        /// Get/Set method of the TotalCost field
        /// </summary>
        [DisplayName("Total_Cost")]
        public double TotalCost
        {
            get
            {
                return totalCost;
            }
        }
        /// <summary>
        /// Get/Set method of the ReorderQuantity field
        /// </summary>
        [DisplayName("Reorder_Quantity")]
        public double ReorderQuantity
        {
            get
            {
                return reOrderQuantity;
            }
        }
        /// <summary>
        /// Get/Set method of the ReorderPoint field
        /// </summary>
        [DisplayName("Reorder_Point")]
        public double ReorderPoint
        {
            get
            {
                return reOrderPoint;
            }
        }
        /// <summary>
        /// Get/Set method of the ProductId field
        /// </summary>
        [DisplayName("ProductId")]
        public int ProductId
        {
            get
            {
                return productId;
            }
        }
        /// <summary>
        /// Get/Set method of the LocationId field
        /// </summary>
        [DisplayName("LocationId")]
        public int LocationId
        {
            get
            {
                return locationId;
            }
        }
        /// <summary>
        /// Get/Set method of the LotControlled field
        /// </summary>
        [DisplayName("LotControlled")]
        public bool LotControlled
        {
            get
            {
                return lotControlled;
            }
        }

        /// <summary>
        /// Get/Set method of the UOMId field
        /// </summary>
        [DisplayName("UOMId")]
        public int UOMId
        {
            get
            {
                return uomId;
            }
        }
    }
}
