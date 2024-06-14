/********************************************************************************************
 * Project Name - Inventory
 * Description  - BarcodeScanSummary
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
  *2.70.2        13-Aug-2019   Deeksha       Added logger methods.
 ********************************************************************************************/
using Semnox.Parafait.logging;
using System.ComponentModel;

namespace Semnox.Parafait.Inventory
{
    /// <summary>
    /// This is the summary Barcode Scan data object class. 
    /// </summary>
    public class BarcodeScanSummaryDTO
    {
       private static readonly Logger log = new Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //select 1 Trx_qty, Code, i.LotId, il.LotNumber, Description, i.Quantity Current_Stock, 
        //l.Name trx_from_location, p.ProductId, p.DefaultLocationId LocationId
        double trxQuantity;
        string code;
        int lotId;
        string lotNumber;
        string description;
        double quantity;
        string locationName;
        int productId;
        int locationId;

        /// <summary>
        /// Parameterized Cnstructor
        /// </summary>
        public BarcodeScanSummaryDTO(double TrxQuantity, string Code, int LotId, string LotNumber, string Description, double Quantity,
                                            string LocationName, int ProductId, int LocationId)
        {
            log.LogMethodEntry(TrxQuantity, Code, LotId, LotNumber, Description, Quantity,  LocationName, ProductId, LocationId);
            this.trxQuantity = TrxQuantity;
            this.code = Code;
            this.lotId = LotId;
            this.lotNumber = LotNumber;
            this.description = Description;
            this.quantity = Quantity;
            this.locationName = LocationName;
            this.productId = ProductId;
            this.locationId = LocationId;
            log.LogMethodExit();
        }

        ///<summary>
        ///Get/Set method of the TrxQuantity field
        ///</summary>
        [DisplayName("Trx_Quantity")]
        public double TrxQuantity
        {
            get
            {
                return trxQuantity;
            }
        }

        /// <summary>
        /// Get/Set method of the Barcode field
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
        /// Get/Set method of the Quantity field
        /// </summary>
        [DisplayName("Quantity")]
        public double Quantity
        {
            get
            {
                return quantity;
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
    }
}
