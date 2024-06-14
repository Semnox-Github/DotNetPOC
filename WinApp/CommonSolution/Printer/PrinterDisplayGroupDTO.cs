/********************************************************************************************
 * Project Name - Printer Display Group DTO
 * Description  - DTO
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *1.00        16-Sep-2018      Mathew Ninan   Created
 *2.60        20-May-2019      Mushahid Faizan Modified isActive from string to bool.
 *2.70.2        18-Jul-2019      Deeksha        Modifications as per 3 tier standard.
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.Printer
{
    /// <summary>
    /// DTO class for PrinterDisplayGroup
    /// </summary>
    public class PrinterDisplayGroupDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private bool notifyingObjectIsChanged;

        ///// <summary>
        ///// SearchByUserParameters enum controls the search fields, this can be expanded to include additional fields
        ///// </summary>
        public enum SearchByParameters
        {   /// <summary>
            /// Search by printerDisplayGroupId
            /// </summary>
            PRINTER_DISPLAY_GROUP_ID,
            /// <summary>
            /// Search by isactive
            /// </summary>
            ISACTIVE,
            /// <summary>
            /// Search by MasterEntityId
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by Printer Id
            /// </summary>
            PRINTER_ID,
            /// <summary>
            /// Search by DisplayGroupId
            /// </summary>
            DISPLAY_GROUP_ID,
            /// <summary>
            /// Search by site id
            /// </summary>
            SITE_ID
        }
        private int printerDisplayGroupId;
        private int printerId;
        private int displayGroupId;
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
        public PrinterDisplayGroupDTO()
        {
            log.LogMethodEntry();
            printerDisplayGroupId = -1;
            printerId = -1;
            displayGroupId = -1;
            isActive = true;
            site_id = -1;
            masterEntityId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// constructor with Required parameter
        /// </summary>
        public PrinterDisplayGroupDTO(int printerDisplayGroupId, int printerId, int displayGroupId, bool isActive) 
            : this()
        {
            log.LogMethodEntry(printerDisplayGroupId, printerId, displayGroupId, isActive);
            this.printerDisplayGroupId = printerDisplayGroupId;
            this.printerId = printerId;
            this.displayGroupId = displayGroupId;
            this.isActive = isActive;            
            log.LogMethodExit();

        }

        /// <summary>
        /// constructor with all the parameter
        /// </summary>
        public PrinterDisplayGroupDTO(int printerDisplayGroupId, int printerId, int displayGroupId, bool isActive, DateTime creationDate,
                                    string createdBy, DateTime lastUpdateDate, string lastUpdatedBy, int site_id, string guid, bool synchStatus,
                                    int masterEntityId)
            : this(printerDisplayGroupId, printerId, displayGroupId, isActive)
        {
            log.LogMethodEntry(printerDisplayGroupId, printerId, displayGroupId, isActive, creationDate, createdBy, lastUpdateDate,
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
        public PrinterDisplayGroupDTO(PrinterDisplayGroupDTO printerDisplayGroupDTO)
            : this(printerDisplayGroupDTO.printerDisplayGroupId, printerDisplayGroupDTO.printerId, printerDisplayGroupDTO.displayGroupId, printerDisplayGroupDTO.isActive,
                 printerDisplayGroupDTO.creationDate, printerDisplayGroupDTO.createdBy, printerDisplayGroupDTO.lastUpdateDate, printerDisplayGroupDTO.lastUpdatedBy,
                 printerDisplayGroupDTO.site_id, printerDisplayGroupDTO.guid, printerDisplayGroupDTO.synchStatus, printerDisplayGroupDTO.masterEntityId)
        {
            log.LogMethodEntry(printerDisplayGroupDTO);
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the printerDisplayGroupId field
        /// </summary>
        [DisplayName("PrinterDisplayGroupId")]
        public int PrinterDisplayGroupId
        {
            get
            {
                return printerDisplayGroupId;
            }

            set
            {
                this.IsChanged = true;
                printerDisplayGroupId = value;
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
        /// Get/Set method of the DisplayGroupId field
        /// </summary>
        [DisplayName("DisplayGroupId")]
        public int DisplayGroupId
        {
            get
            {
                return displayGroupId;
            }

            set
            {
                this.IsChanged = true;
                displayGroupId = value;
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
        [DisplayName("LastUpdatedDate")]
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
        [DisplayName("LastUpdatedBy")]
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
                    return notifyingObjectIsChanged || printerDisplayGroupId < 0;
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
