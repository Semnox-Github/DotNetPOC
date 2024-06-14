/********************************************************************************************
 * Project Name - Inventory
 * Description  - Data object of the VendorReturns DTO 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 *********************************************************************************************
 *2.70.2        09-Jul-2019   Girish Kundar      Modified : Added constructor with required parameters.
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.Inventory
{
    /// <summary>
    /// VendorReturnsDTO class
    /// </summary>
    public class VendorReturnsDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// SearchByVendorReturnParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByVendorReturnParameters
        {
            /// <summary>
            /// Search by VENDOR_NAME field
            /// </summary>
            VENDOR_NAME ,
            /// <summary>
            /// Search by ORDER_NUMBER field
            /// </summary>
            ORDER_NUMBER ,
            /// <summary>
            /// Search by VENDOR_BILL_NUMBER NUMBER field
            /// </summary>
            VENDOR_BILL_NUMBER,
            /// <summary>
            /// Search by GRN field
            /// </summary>
            GRN,
            /// <summary>
            /// Search by ACTIVE_FLAG field
            /// </summary>
            ACTIVE_FLAG,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID
        }

       private int receiptId;
       private string poNumber;
       private DateTime poDate;
       private string vendorName;
       private double receiptAmount;
       private string orderNo;
       private string vendorBillNo;
       private string grn;
       private bool isActive;

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        /// <summary>
        /// Default constructor
        /// </summary>
        public VendorReturnsDTO()
        {
            log.LogMethodEntry();
            isActive = true;
            receiptId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public VendorReturnsDTO(int receiptIdPassed, string poNpPassed, DateTime poDatePassed,string vendorNamePassed, double receiptAamt, 
            string orderNoPassed, string vendorBillNoPassed, string grnPassed)
        {
            log.LogMethodEntry(receiptIdPassed, poNpPassed, poDatePassed, vendorNamePassed, receiptAamt,
                               orderNoPassed,  vendorBillNoPassed, grnPassed);
            this.receiptId = receiptIdPassed;
            this.poNumber = poNpPassed;
            this.poDate = poDatePassed;
            this.vendorName = vendorNamePassed;
            this.receiptAmount = receiptAamt;
            this.orderNo = orderNoPassed;
            this.vendorBillNo = vendorBillNoPassed;
            this.grn = grnPassed;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public VendorReturnsDTO(string orderNoPassed,DateTime poDatePassed, string vendorNamePassed, double receiptAmt)
        {
            log.LogMethodEntry(orderNoPassed, poDatePassed, vendorNamePassed, vendorNamePassed, receiptAmt);
            this.poDate = poDatePassed;
            this.vendorName = vendorNamePassed;
            this.receiptAmount = receiptAmt;
            this.orderNo = orderNoPassed;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the Receipt Id fields
        /// </summary>
        [DisplayName("Receipt Id")]
        public int ReceiptId { get { return receiptId; } set { receiptId = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the PO Number fields
        /// </summary>
        [DisplayName("PO Number")]
        public string PONumber { get { return poNumber; } set { poNumber = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the PO date fields
        /// </summary>
        [DisplayName("PO Date")]
        public DateTime PODate { get { return poDate; } set { poDate = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the VendorName fields
        /// </summary>
        [DisplayName("Vendor Name")]
        public string VendorName { get { return vendorName; } set { vendorName = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Receipt Amount fields
        /// </summary>
        [DisplayName("Total Amount")]
        public double ReceiptAmount { get { return receiptAmount; } set { receiptAmount = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Order Number fields
        /// </summary>
        [DisplayName("Order Number")]
        public string OrderNumber { get { return orderNo; } set { orderNo = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the vendor bill number fields
        /// </summary>
        [DisplayName("Vendor Bill Number")]
        public string VendorBillNumber { get { return vendorBillNo; } set { vendorBillNo = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the EstimatedValue fields
        /// </summary>
        [DisplayName("GRN")]
        public string GRN { get { return grn; } set { grn = value; IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the   isActive  fields
        /// </summary>
        [DisplayName("IsActive ")]
        public bool IsActive { get { return isActive; } set { isActive = value; IsChanged = true; } }

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
                    return notifyingObjectIsChanged || receiptId < 0;
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
