/* Project Name - BOMExcelDTO 
* Description  - Data handler object of the BOMExcel
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
********************************************************************************************* 
 *2.150.0    29-Dec-2022        Abhishek                Created 
********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// This is the BOMExcelDTO data object class. This acts as data holder for the BOM business object
    /// </summary>
    public class BOMExcelDTO
    {
         Semnox.Parafait.logging.Logger log = new  Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        private string productCode;
        private string productName;
        private string childProductCode;
        private string childProductName;
        private decimal quantity;
        private string uOM;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public BOMExcelDTO()
        {
            log.LogMethodEntry();
            quantity = 0;
            productCode = string.Empty;
            productName = string.Empty;
            childProductCode = string.Empty;
            childProductName = string.Empty;
            uOM = string.Empty;
            log.LogMethodExit();
        }

        // <summary>
        // Parameterized Constructor with required data fields
        // </summary>
        public BOMExcelDTO(string productCode, string productName, string childProductCode, string childProductName, decimal quantity, 
                           string uOM)
            : this()
        {
            log.LogMethodEntry(productCode, productName, childProductCode, childProductName, quantity, uOM);
            this.productCode = productCode;
            this.productName = productName;
            this.childProductCode = childProductCode;
            this.childProductName = childProductName;
            this.quantity = quantity;
            this.uOM = uOM;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the Quantity field
        /// </summary>
        public decimal Quantity { get { return quantity; } set { quantity = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ProductCode field
        /// </summary>
        [Browsable(false)]
        public string ProductCode { get { return productCode; } set { productCode = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the ProductName field
        /// </summary>
        public string ProductName { get { return productName; } set { productName = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the ChildProductCode field
        /// </summary>
        public string ChildProductCode
        {
            get { return childProductCode; }
            set { childProductCode = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get method of the ChildProductName field
        /// </summary>
        public string ChildProductName
        {
            get { return childProductName; }
            set { childProductName = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get method of the UOM field
        /// </summary>
        public string UOM
        {
            get { return uOM; }
            set { uOM = value; this.IsChanged = true; }
        }

        /// <summary>
        ///  Get/Set method to track changes to the object
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
