/********************************************************************************************
 * Project Name - Printer Products DTO
 * Description  - DTO
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *1.00        16-Sep-2018      Mathew Ninan   Created
 *2.60        20-May-2019      Mushahid Faizan Modified IsActive from string to bool.
 *2.70.2        18-Jul-2019      Deeksha        Modifications as per 3 tier standard.
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.Printer
{
    /// <summary>
    /// DTO class for PrinterProducts
    /// </summary>
    public class PrinterProductsDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private bool notifyingObjectIsChanged;

        ///// <summary>
        ///// SearchByUserParameters enum controls the search fields, this can be expanded to include additional fields
        ///// </summary>
        public enum SearchByParameters
        {   /// <summary>
            /// Search by printerProductId
            /// </summary>
            PRINTER_PRODUCT_ID,
            /// <summary>
            /// Search by Printer Id
            /// </summary>
            PRINTERID,
            /// <summary>
            /// Search by Product Id
            /// </summary>
            PRODUCTID,
            /// <summary>
            /// Search by isactive
            /// </summary>
            ISACTIVE,
            /// <summary>
            /// Search by site_id
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by Master Entity Id
            /// </summary>
            MASTER_ENTITY_ID
        }
        private int printerProductId;
        private int printerId;
        private int productId;
        private bool isActive;
        private DateTime creationDate;
        private string createdBy;
        private DateTime lastUpdateDate;
        private string lastUpdatedBy;
        private int site_id;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public PrinterProductsDTO()
        {
            log.LogMethodEntry();
            printerProductId = -1;
            printerId = -1;
            productId = -1;
            site_id = -1;
            isActive = true;
            masterEntityId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor with Required data fields
        /// </summary>
        public PrinterProductsDTO(int printerProductId, int printerId, int productId, bool isActive)
            : this()
        {
            log.LogMethodEntry(printerProductId, printerId, productId, isActive);
            this.printerProductId = printerProductId;
            this.printerId = printerId;
            this.productId = productId;
            this.isActive = isActive;           
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor with all the data fields
        /// </summary>
        public PrinterProductsDTO(int printerProductId, int printerId, int productId, bool isActive, DateTime creationDate,
                                    string createdBy, DateTime lastUpdateDate, string lastUpdatedBy, int site_id, string guid,
                                    bool synchStatus, int masterEntityId)
            : this(printerProductId, printerId, productId, isActive)
        {
            log.LogMethodEntry(printerProductId, printerId, productId, isActive, creationDate, createdBy, lastUpdateDate,
                               lastUpdatedBy, site_id, guid, synchStatus, masterEntityId);
            this.creationDate = creationDate;
            this.createdBy = createdBy;
            this.lastUpdateDate = lastUpdateDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.site_id = site_id;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            log.LogMethodExit();
        }

        /// <summary>
        /// copy constructor
        /// </summary>
        public PrinterProductsDTO(PrinterProductsDTO printerProductsDTO)
            :this(printerProductsDTO.printerProductId, printerProductsDTO.printerId, printerProductsDTO.productId, printerProductsDTO.isActive, printerProductsDTO.creationDate,
                 printerProductsDTO.createdBy, printerProductsDTO.lastUpdateDate, printerProductsDTO.lastUpdatedBy, printerProductsDTO.site_id, printerProductsDTO.guid,
                 printerProductsDTO.synchStatus, printerProductsDTO.masterEntityId)
        {
            log.LogMethodEntry(printerProductsDTO);
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the PrinterProductId field
        /// </summary>
        [DisplayName("PrinterProductId")]
        public int PrinterProductId
        {
            get
            {
                return printerProductId;
            }

            set
            {
                this.IsChanged = true;
                printerProductId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the PrinterId field
        /// </summary>
        [DisplayName("PrinterId")]
        public int PrinterId
        {
            get
            {
                return printerId;
            }

            set
            {
                this.IsChanged = true;
                printerId = value;
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

            set
            {
                this.IsChanged = true;
                productId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the isActive field
        /// </summary>
        [DisplayName("Active")]
        public bool IsActive
        {
            get
            {
                return isActive;
            }

            set
            {
                this.IsChanged = true;
                isActive = value;
            }
        }

        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        public DateTime CreationDate
        {
            get
            {
                return creationDate;
            }
            set
            {
                creationDate = value;           
            }
        }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        public string CreatedBy
        {
            get
            {
                return createdBy;
            }
            set
            {
                createdBy = value;           
            }
        }

        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        [DisplayName("Last Updated Date")]
        public DateTime LastUpdatedDate
        {
            get
            {
                return lastUpdateDate;
            }
            set
            {           
                lastUpdateDate = value;
            }
        }

        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("Last Updated By")]
        public string LastUpdatedBy
        {
            get
            {
                return lastUpdatedBy;
            }
            set
            {                
                lastUpdatedBy = value;
            }
        }

        /// <summary>
        /// Get/Set method of the site_id field
        /// </summary>
        public int Site_Id
        {
            get
            {
                return site_id;
            }
            set
            {
                site_id = value;
            }
        }
       
        /// <summary>
        /// Get/Set method of the guid field
        /// </summary>
        public string GUID
        {
            get
            {
                return guid;
            }
            set
            {
                this.IsChanged = true;
                guid = value;
            }
        }

        /// <summary>
        /// Get/Set method of the synchStatus field
        /// </summary>
        public bool SynchStatus
        {
            get
            {
                return synchStatus;
            }
            set
            {
                synchStatus = value;
            }
        }

        /// <summary>
        /// Get/Set method of the masterEntityId field
        /// </summary>
        public int MasterEntityId
        {
            get
            {
                return masterEntityId;
            }
            set
            {
                this.IsChanged = true;
                masterEntityId = value;
            }
        }

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
                    return notifyingObjectIsChanged || printerProductId < 0;
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
