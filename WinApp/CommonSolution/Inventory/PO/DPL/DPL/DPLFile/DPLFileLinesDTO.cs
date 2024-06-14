using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Parafait.Product;

namespace Semnox.Parafait.Inventory
{
    class DPLFileLinesDTO
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        string vendorInvoiceNumber;
        DateTime receivedDate;
        int vendorID;
        int poID;
        double dplMarkupPercent;
        int productId;
        string productCode;
        string productDescription;
        double productQuantity;
        double remainQuantity;
        //int uomId;
        int prodPackSize;
        double prodPrice;
        double prodPriceInTicket;
        DateTime prodExpiryDate;
        //int defaultLocationId;
        //int taxId;
        double taxPercentage;
        //string taxInclusive;
        //bool lotControlled;
        string remarks;
        ProductDTO productDTORec;

        public DPLFileLinesDTO()
        {
            log.Debug("Starts-DPLFileLinesDTO() default constructor.");
            vendorID = -1;
            poID = -1;
            productId = -1;
            //uomId = -1;
            //defaultLocationId = -1;
            log.Debug("Ends-DPLFileLinesDTO() default constructor.");
        }

        public DPLFileLinesDTO(string vendorInvoiceNumber,DateTime receivedDate, int vendorID, int poID, double dplMarkupPercent,int productId, string productCode, string productDescription, double productQuantity,
        double remainQuantity, int prodPackSize, double prodPrice,double prodPriceInTicket, DateTime prodExpiryDate, double taxPercentage,
        string remarks, ProductDTO productDTO)
        {
            log.Debug("Starts-DPLFileLinesDTO() all fields constructor.");
            this.vendorInvoiceNumber = vendorInvoiceNumber;
            this.receivedDate = receivedDate;
            this.vendorID = vendorID;
            this.poID = poID;
            this.dplMarkupPercent = dplMarkupPercent;
            this.productId = productId;
            this.productCode = productCode;
            this.productDescription = productDescription;
            this.productQuantity = productQuantity;
            this.remainQuantity = remainQuantity;
            //this.uomId = uomId;
            this.prodPackSize = prodPackSize;
            this.prodPrice = prodPrice;
            this.prodPriceInTicket = prodPriceInTicket;
            this.prodExpiryDate = prodExpiryDate;
            //this.defaultLocationId = defaultLocationId;
            //this.taxId = taxId;
            this.taxPercentage = taxPercentage;
            //this.taxInclusive = taxInclusive;
            //this.lotControlled = lotControlled;
            this.remarks = remarks;
            this.productDTORec = productDTO;
            log.Debug("Ends-DPLFileLinesDTO() all fields constructor.");
        }
        /// <summary>
        /// Get/Set method of the VendorInvoiceNumber field
        /// </summary>
        [DisplayName("VendorInvoiceNumber")]
        public string VendorInvoiceNumber { get { return vendorInvoiceNumber; } set { vendorInvoiceNumber = value; } }
        /// <summary>
        /// Get/Set method of the ReceivedDate field
        /// </summary>
        [DisplayName("ReceivedDate")]
        public DateTime ReceivedDate { get { return receivedDate; } set { receivedDate = value; } }
        /// <summary>
        /// Get/Set method of the VendorID field
        /// </summary>
        [DisplayName("VendorID")]
        public int VendorID { get { return vendorID; } set { vendorID = value; } }
        /// <summary>
        /// Get/Set method of the POID field
        /// </summary>
        [DisplayName("POID")]
        public int POID { get { return poID; } set { poID = value; } }
        /// <summary>
        /// Get/Set method of the dplMarkupPercent field
        /// </summary>
        [DisplayName("DPLMarkupPercent")]
        public double DPLMarkupPercent { get { return dplMarkupPercent; } set { dplMarkupPercent = value; } }
        /// <summary>
        /// Get/Set method of the ProductId field
        /// </summary>
        [DisplayName("ProductId")]
        public int ProductId { get { return productId; } set { productId = value; } }
        /// <summary>
        /// Get/Set method of the ProductCode field
        /// </summary>
        [DisplayName("ProductCode")]
        public string ProductCode { get { return productCode; } set { productCode = value; } }
        /// <summary>
        /// Get/Set method of the ProductDescription field
        /// </summary>
        [DisplayName("ProductDescription")]
        public string ProductDescription { get { return productDescription; } set { productDescription = value; } }
        /// <summary>
        /// Get/Set method of the ProductQuantity field
        /// </summary>
        [DisplayName("ProductQuantity")]
        public double ProductQuantity { get { return productQuantity; } set { productQuantity = value; } }
        /// <summary>
        /// Get/Set method of the RemainQuantity field
        /// </summary>
        [DisplayName("RemainQuantity")]
        public double RemainQuantity { get { return remainQuantity; } set { remainQuantity = value; } }        
        /// <summary>
        /// Get/Set method of the ProdPackSize field
        /// </summary>
        [DisplayName("ProdPackSize")]
        public int ProdPackSize { get { return prodPackSize; } set { prodPackSize = value; } }
        /// <summary>
        /// Get/Set method of the ProdPrice field
        /// </summary>
        [DisplayName("ProdPrice")]
        public double ProdPrice { get { return prodPrice; } set { prodPrice = value; } }
        /// <summary>
        /// Get/Set method of the ProdPriceInTicket field
        /// </summary>
        [DisplayName("ProdPriceInTicket")]
        public double ProdPriceInTicket { get { return prodPriceInTicket; } set { prodPriceInTicket = value; } }
        /// <summary>
        /// Get/Set method of the ProdExpiryDate field
        /// </summary>
        [DisplayName("ProdExpiryDate")]
        public DateTime ProdExpiryDate { get { return prodExpiryDate; } set { prodExpiryDate = value; } }
        /// <summary>
        /// Get/Set method of the TaxPercentage field
        /// </summary>
        [DisplayName("TaxPercentage")]
        public double TaxPercentage { get { return taxPercentage; } set { taxPercentage = value; } }
        /// <summary>
        /// Get/Set method of the Remarks field
        /// </summary>
        [DisplayName("Remarks")]
        public string Remarks { get { return remarks; } set { remarks = value; } }
        /// <summary>
        /// Get/Set method of the ProductDTORec field
        /// </summary>
        [DisplayName("ProductDTORec")]
        public ProductDTO ProductDTORec { get { return productDTORec; } set { productDTORec = value; } }
    }
}