/********************************************************************************************
 * Project Name - Printer DTO
 * Description  - DTO class
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *1.00        11-Sep-2017      Vinayaka V     Created 
 *2.00        18-Sep-2018      Mathew Ninan   Updated to reflect POSPrinters objects 
 *2.60        22-Mar-2019      Akshay Gulaganji modified isActive dataType from string to bool
 *2.70        08-Jul-2019      Deeksha        Modified:Added new Constructor with required fields
 *2.80        10-May-2020      Girish Kundar  Modified: REST API Changes merge from WMS  
 *2.110       11-Dec-2020      Dakshakh Raj   Modified: for Peru Invoice Enhancement  (added list of POSPrinterOverrideRulesDTO as a field)
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using Semnox.Parafait.Printer;

namespace Semnox.Parafait.POS
{
    /// <summary>
    /// DTO class for printers
    /// </summary>
    ///Serializable
    public class POSPrinterDTO
    {
        private static readonly Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private bool notifyingObjectIsChanged;

        ///// <summary>
        ///// SearchByUserParameters enum controls the search fields, this can be expanded to include additional fields
        ///// </summary>
        public enum SearchByParameters
        {   /// <summary>
            /// Search by POSprinterId
            /// </summary>
            POS_PRINTER_ID,
            /// <summary>
            /// Search by POSMachineId
            /// </summary>
            POS_MACHINE_ID,
            /// <summary>
            /// Search by POSMachineId List
            /// </summary>
            POS_MACHINE_ID_LIST,
            /// <summary>
            /// Search by POSTypeId
            /// </summary>
            POS_TYPE_ID,
            /// <summary>
            /// Search by printerId
            /// </summary>
            PRINTER_ID,
            /// <summary>
            /// Search by isactive
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search by Printer Name
            /// </summary>
            PRINTER_NAME,
            /// <summary>
            /// Search by Printer Location
            /// </summary>
            PRINTER_LOCATION,
            /// <summary>
            /// Search by IP Address
            /// </summary>
            IP_ADDRESS,
            /// <summary>
            /// Search by Print Template id
            /// </summary>
            PRINT_TEMPLATE_ID,
            /// <summary>
            /// Search by Secondary Printer Id
            /// </summary>
            SECONDARY_PRINTER_ID,
            /// <summary>
            /// Search by site_id
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER ENTITY ID
            /// </summary>
            MASTER_ENTITY_ID
        }

        private int posPrinterId;
        private int printerId;
        private int printTemplateId;
        private int posTypeId;
        private int posMachineId;
        private int orderTypeGroupId;
        private bool isActive;
        private DateTime creationDate;
        private string createdBy;
        private DateTime lastUpdateDate;
        private string lastUpdatedBy;
        private int site_id;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        private int secondaryPrinterId;
        private string printerName;
        private string printerLocation;
        private string kOTPrinter;
        private bool kDSTerminal;
        private bool ticketPrinter;
        private DataTable receiptTemplate;
        private DataTable printOnlyTheseProducts;
        private POSPrinterDTO secondaryPrinter;
        private PrinterDTO printerDTO;
        private PrinterDTO secondaryPrinterDTO;
        private int printerTypeId;
        private ReceiptPrintTemplateHeaderDTO receiptPrintTemplateHeaderDTO;
        private List<POSPrinterOverrideRulesDTO> pOSPrinterOverrideRulesDTOList;

        /// <summary>
        /// Default constructor
        /// </summary>
        public POSPrinterDTO()
        {
            log.LogMethodEntry();
            printerId = -1;
            posMachineId = -1;
            printTemplateId = -1;
            secondaryPrinterId = -1;
            printerName = string.Empty;
            printerLocation = string.Empty;
            kOTPrinter = string.Empty;
            isActive = true;
            site_id = -1;
            kDSTerminal = false;
            ticketPrinter = false;
            receiptTemplate = null;
            printOnlyTheseProducts = null;
            secondaryPrinter = null;
            printerTypeId = -1;
            orderTypeGroupId = -1;
            posTypeId = -1;
            printerTypeId = -1;
            pOSPrinterOverrideRulesDTOList = new List<POSPrinterOverrideRulesDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// constructor with Required parameter
        /// </summary>
        public POSPrinterDTO(int posPrinterId, int posMachineId, int printerId, int posTypeId, int secondaryPrinterId,
                             int orderTypeGroupId, int printTemplateId, PrinterDTO printerDTO, PrinterDTO secondaryPrinterDTO,
                             ReceiptPrintTemplateHeaderDTO receiptPrintTemplateHeaderDTO, bool isActive, int printerTypeId)
            :this()
        {
            log.LogMethodEntry(posPrinterId,posMachineId, printerId,posTypeId, secondaryPrinterId,orderTypeGroupId, 
                            printTemplateId, printerDTO,  secondaryPrinterDTO,receiptPrintTemplateHeaderDTO,  isActive, printerTypeId);
            this.posPrinterId = posPrinterId;
            this.printerId = printerId;
            this.posMachineId = posMachineId;
            this.posTypeId = posTypeId;
            this.secondaryPrinterId = secondaryPrinterId;
            this.orderTypeGroupId = orderTypeGroupId;
            this.printTemplateId = printTemplateId;
            this.printerDTO = printerDTO;
            this.secondaryPrinterDTO = secondaryPrinterDTO;
            this.receiptPrintTemplateHeaderDTO = receiptPrintTemplateHeaderDTO;
            this.isActive = isActive;
            this.printerTypeId = printerTypeId;
            log.LogMethodExit();
        }

        /// <summary>
        /// constructor with all the parameter
        /// </summary>
        public POSPrinterDTO(int posPrinterId, int posMachineId, int printerId, int posTypeId, int secondaryPrinterId,
                             int orderTypeGroupId, int printTemplateId, PrinterDTO printerDTO, PrinterDTO secondaryPrinterDTO,
                             ReceiptPrintTemplateHeaderDTO receiptPrintTemplateHeaderDTO, bool isActive, DateTime creationDate, 
                             string createdBy, DateTime lastUpdateDate, string lastUpdatedBy, int site_id, string guid,
                             bool synchStatus, int masterEntityId,int printerTypeId)
            :this(posPrinterId, posMachineId, printerId, posTypeId, secondaryPrinterId, orderTypeGroupId,
                            printTemplateId, printerDTO, secondaryPrinterDTO, receiptPrintTemplateHeaderDTO, isActive, printerTypeId)
        {
            log.LogMethodEntry(posPrinterId, posMachineId, printerId, posTypeId, secondaryPrinterId, orderTypeGroupId,
                            printTemplateId, printerDTO, secondaryPrinterDTO, receiptPrintTemplateHeaderDTO, isActive,creationDate,
                            createdBy,  lastUpdateDate,lastUpdatedBy,  site_id,  guid,synchStatus,  masterEntityId, printerTypeId);
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
        public POSPrinterDTO(POSPrinterDTO posPrinterDTO)
            : this(posPrinterDTO.POSPrinterId, posPrinterDTO.posMachineId, posPrinterDTO.printerId, posPrinterDTO.posTypeId, posPrinterDTO.secondaryPrinterId,
                   posPrinterDTO.orderTypeGroupId, posPrinterDTO.printTemplateId, posPrinterDTO.printerDTO, posPrinterDTO.secondaryPrinterDTO, posPrinterDTO.receiptPrintTemplateHeaderDTO, posPrinterDTO.isActive, posPrinterDTO.creationDate,
                             posPrinterDTO.createdBy, posPrinterDTO.lastUpdateDate, posPrinterDTO.lastUpdatedBy, posPrinterDTO.site_id, posPrinterDTO.guid,
                             posPrinterDTO.synchStatus, posPrinterDTO.masterEntityId, posPrinterDTO.printerTypeId)
        {
            log.LogMethodEntry(posPrinterDTO);
            if (posPrinterDTO.secondaryPrinter != null)
            {
                secondaryPrinter = new POSPrinterDTO(posPrinterDTO.secondaryPrinter);
            }
            if (posPrinterDTO.printerDTO != null)
            {
                printerDTO = new PrinterDTO(posPrinterDTO.printerDTO);
            }
            if (posPrinterDTO.secondaryPrinterDTO != null)
            {
                secondaryPrinterDTO = new PrinterDTO(posPrinterDTO.secondaryPrinterDTO);
            }
            if (posPrinterDTO.receiptPrintTemplateHeaderDTO != null)
            {
                receiptPrintTemplateHeaderDTO = new ReceiptPrintTemplateHeaderDTO(posPrinterDTO.receiptPrintTemplateHeaderDTO);
            }
            if (posPrinterDTO.pOSPrinterOverrideRulesDTOList != null)
            {
                pOSPrinterOverrideRulesDTOList = new List<POSPrinterOverrideRulesDTO>();
                foreach (var posPrinterOverrideRulesDTO in posPrinterDTO.pOSPrinterOverrideRulesDTOList)
                {
                    pOSPrinterOverrideRulesDTOList.Add(new POSPrinterOverrideRulesDTO(posPrinterOverrideRulesDTO));
                }
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Get/Set method of the POSPrinterId field
        /// </summary>
        [DisplayName("POSPrinterId")]
        public int POSPrinterId
        {
            get
            {
                return posPrinterId;
            }
            set
            {
                posPrinterId = value;
                this.IsChanged = true;
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
                printerId = value;
                this.IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the SecondaryPrinterId field
        /// </summary>
        [DisplayName("SecondaryPrinterId")]
        public int SecondaryPrinterId
        {
            get
            {
                return secondaryPrinterId;
            }
            set
            {
                secondaryPrinterId = value;
                this.IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the OrderTypeGroupId field
        /// </summary>
        [DisplayName("OrderTypeGroupId")]
        public int OrderTypeGroupId
        {
            get
            {
                return orderTypeGroupId;
            }
            set
            {
                orderTypeGroupId = value;
                this.IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the POSMachineId field
        /// </summary>
        [DisplayName("POSMachineId")]
        public int POSMachineId
        {
            get
            {
                return posMachineId;
            }
            set
            {
                posMachineId = value;
                this.IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the PrinterId field
        /// </summary>
        [DisplayName("POSTypeId")]
        public int POSTypeId
        {
            get
            {
                return posTypeId;
            }
            set
            {
                posTypeId = value;
                this.IsChanged = true;
            }
        }
        
        /// <summary>
        /// Get/Set method of the TemplateId field
        /// </summary>
        [DisplayName("PrintTemplateId")]
        public int PrintTemplateId
        {
            get
            {
                return printTemplateId;
            }
            set
            {
                printTemplateId = value;
                this.IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the PrinterName field
        /// </summary>
        [DisplayName("PrinterName")]
        public String PrinterName
        {
            get
            {
                return printerName;
            }
            set
            {
                printerName = value;
                this.IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the PrinterLocation field
        /// </summary>
        [DisplayName("PrinterLocation")]
        public String PrinterLocation
        {
            get
            {
                return printerLocation;
            }
            set
            {
                printerLocation = value;
                this.IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the KOTPrinter field
        /// </summary>
        [DisplayName("KOT_Printer")]
        public String KOTPrinter
        {
            get
            {
                return kOTPrinter;
            }
            set
            {
                kOTPrinter = value;
                this.IsChanged = true;
            }
        }


        /// <summary>
        /// Get/Set method of the IsKDSTerminal field
        /// </summary>
        [DisplayName("IsKDSTerminal")]
        public bool KDSTerminal
        {
            get
            {
                return kDSTerminal;
            }
            set
            {
                kDSTerminal = value;
                this.IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the TicketPrinter field
        /// </summary>
        [DisplayName("TicketPrinter")]
        public bool TicketPrinter
        {
            get
            {
                return ticketPrinter;
            }
            set
            {
                ticketPrinter = value; this.IsChanged = true;
            }
        }

        
        /// <summary>
        /// Get/Set method of the ReceiptTemplate field
        /// </summary>
        [DisplayName("ReceiptTemplate")]
        public DataTable ReceiptTemplate
        {
            get
            {
                return receiptTemplate;
            }
            set
            {
                receiptTemplate = value; this.IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the PrintOnlyTheseProducts field
        /// </summary>
        [DisplayName("PrintOnlyTheseProducts")]
        public DataTable PrintOnlyTheseProducts
        {
            get
            {
                return printOnlyTheseProducts;
            }
            set
            {
                printOnlyTheseProducts = value;
                this.IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the SecondaryPrinter field
        /// </summary>
        [DisplayName("SecondaryPrinter")]
        public POSPrinterDTO SecondaryPrinter
        {
            get
            {
                return secondaryPrinter;
            }
            set
            {
                secondaryPrinter = value; this.IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the PrinterDTO field
        /// </summary>
        [DisplayName("SecondaryPrinterDTO")]
        public PrinterDTO SecondaryPrinterDTO
        {
            get
            {
                return secondaryPrinterDTO;
            }
            set
            {
                secondaryPrinterDTO = value; this.IsChanged = true;
            }
        }
        
        /// <summary>
        /// Get/Set method of the PrinterDTO field
        /// </summary>
        [DisplayName("PrinterDTO")]
        public PrinterDTO PrinterDTO
        {
            get
            {
                return printerDTO;
            }
            set
            {
                printerDTO = value; this.IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the ReceiptPrintTemplateHeaderDTO field
        /// </summary>
        [DisplayName("ReceiptPrintTemplateHeaderDTO")]
        public ReceiptPrintTemplateHeaderDTO ReceiptPrintTemplateHeaderDTO
        {
            get
            {
                return receiptPrintTemplateHeaderDTO;
            }
            set
            {
                receiptPrintTemplateHeaderDTO = value;
                this.IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the ReceiptPrintTemplateHeaderDTO field
        /// </summary>
        [DisplayName("POSPrinterOverrideRulesDTOList")]
        public List<POSPrinterOverrideRulesDTO> POSPrinterOverrideRulesDTOList
        {
            get
            {
                return pOSPrinterOverrideRulesDTOList;
            }
            set
            {
                pOSPrinterOverrideRulesDTOList = value;
                this.IsChanged = true;
            }
        }
        /// <summary>
        /// Get/Set method of the IsActive field
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
        /// Get/Set method of the siteId field
        /// </summary>
        public int SiteId
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
        /// Get/Set method of the SecondaryPrinterId field
        /// </summary>
        [DisplayName("PrinterTypeId")]
        public int PrinterTypeId
        {
            get
            {
                return printerTypeId;
            }
            set
            {
                printerTypeId = value;
                this.IsChanged = true;
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
                    return notifyingObjectIsChanged || posPrinterId < 0;
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
        /// Allowes to accept the changes
        /// </summary>
        public void AcceptChanges()
        {
            log.LogMethodEntry();
            this.IsChanged = false;
            log.LogMethodExit();
        }
    }
}
