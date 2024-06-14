/********************************************************************************************
 * Project Name - Product Activity View DTO
 * Description  - Data object of product activity view 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        09-Sep-2017   Indhu          Created 
 *2.100.0     12-Aug-2020   Deeksha        Modified for Recipe Management enhancement.
 *2.140.00    30-Nov-2022   Abhishek       Modified : Web Inventory Redesign, Added parameter LotId
                                           ProductActivityViewExcel class to download product activity view             
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// This is the product activity view  data object class. This acts as data holder for the product activity view  business object
    /// </summary>
    public class ProductActivityViewDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new  Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// SearchByProductActivityParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByProductActivityViewParameters
        {
            ///<summary>
            ///Search by PRODUCT ID field
            ///</summary>
            PRODUCT_ID ,
            ///<summary>
            ///Search by LOCATION ID field
            ///</summary>
            LOCATION_ID
        }

        private string trxType;
        private int productId;
        private int locationId;
        private int transferLocationId;
        private double quantity;
        private DateTime timeStamp;
        private string userName;
        private int uomId;
        private string uomName;
        private int lotId;

        /// <summary>
        /// Default constructor
        /// </summary>
        public ProductActivityViewDTO()
        {
            log.LogMethodEntry();
            productId = -1;
            transferLocationId = -1;
            locationId = -1;
            lotId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        public ProductActivityViewDTO(string trxType ,int productId ,int locationId ,int transferLocationId,
                                        double quantity,DateTime timeStamp,string userName,int uomId, int lotId)
        {
            log.LogMethodEntry(trxType, productId, locationId, transferLocationId, quantity, timeStamp, userName, uomId, lotId);
            this.trxType = trxType;
            this.productId = productId;
            this.locationId = locationId;
            this.transferLocationId = transferLocationId;
            this.quantity = quantity;
            this.timeStamp = timeStamp;
            this.userName = userName;
            this.uomId = uomId;
            this.lotId = lotId;
            log.LogMethodExit();
        }

        ///<summary>
        ///Get/Set of the TrxType
        ///</summary>
        public string Trx_Type { get { return trxType; } set { trxType = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the TimeStamp field
        /// </summary>
        public DateTime TimeStamp { get { return timeStamp; } set { timeStamp = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ProductId field
        /// </summary>
        public int ProductId { get { return productId; } set { productId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Location field
        /// </summary>
        public int LocationId { get { return locationId; } set { locationId= value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the TransferLocation field
        /// </summary>
        public int TransferLocationId { get { return transferLocationId; } set { transferLocationId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Quantity field
        /// </summary>
        public double Quantity { get { return quantity; } set { quantity= value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the UserName field
        /// </summary>
        public string UserName { get { return userName; } set { userName= value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the UOMid field
        /// </summary>
        public int UOMId { get { return uomId; } set { uomId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the UOM field
        /// </summary>
        public string UOM { get { return uomName; } set { uomName = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the LotId field
        /// </summary>
        public int LotId { get { return lotId; } set { lotId = value; this.IsChanged = true; } }

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

    /// <summary>
    /// This is the product activity view  excel class. This acts as data holder for the product activity view excel object
    /// </summary>
    public class ProductActivityViewExcel
    {
        private DateTime? trx_Date;
        private string location;
        private string transferLocation;
        private double quantity;
        private string trxType;
        private string uom;
        private string userName;
        private int lotId;

        /// <summary>
        /// Get/Set method of the Trx_Date field
        /// </summary>
        public DateTime? Trx_Date { get { return trx_Date; } set { trx_Date = value; } }

        /// <summary>
        /// Get/Set method of the Quantity field
        /// </summary>
        public string Location { get { return location; } set { location = value; } }

        /// <summary>
        /// Get/Set method of the Quantity field
        /// </summary>
        public string TransferLocation { get { return transferLocation; } set { transferLocation = value; } }

        /// <summary>
        /// Get/Set method of the Quantity field
        /// </summary>
        public string TrxType { get { return trxType; } set { trxType = value; } }

        /// <summary>
        /// Get/Set method of the Quantity field
        /// </summary>
        public string UOM { get { return uom; } set { uom = value; } }

        /// <summary>
        /// Get/Set method of the UserName field
        /// </summary>
        public string UserName { get { return userName; } set { userName = value; } }

        /// <summary>
        /// Get/Set method of the Quantity field
        /// </summary>
        public double Quantity { get { return quantity; } set { quantity = value; } }

        /// <summary>
        /// Get/Set method of the LotId field
        /// </summary>
        public int LotId { get { return lotId; } set { lotId = value; } }
    }
}
