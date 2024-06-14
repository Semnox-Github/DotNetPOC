/********************************************************************************************
 * Project Name - Printer DTO
 * Description  - DTO
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By     Remarks          
 *********************************************************************************************
 *1.00         16-Sep-2018      Mathew Ninan         Created
 *2.60         16-May-2019      Mushahid Faizan      Added inner list of PrinterProductsDTO,PrinterDisplayGroupDTO
 *2.70.2       18-Jul-2019      Deeksha              Modifications as per 3 tier standard.
 *2.130.5      01-Mar-2021      Girish Kundar        Modified : Added new column Models to printer
 *2.130.9      16-Jun-2022      Guru S A             Execute online transaction changes in Kiosk
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Semnox.Parafait.Printer
{
    /// <summary>
    /// DTO class for printers
    /// </summary>
    ///Serializable
    public class PrinterDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private bool notifyingObjectIsChanged;

        ///// <summary>
        ///// SearchByUserParameters enum controls the search fields, this can be expanded to include additional fields
        ///// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by printerId
            /// </summary>
            PRINTERID,
            /// <summary>
            /// Search by isactive
            /// </summary>
            ISACTIVE,
            /// <summary>
            /// Search by Printer Name
            /// </summary>
            PRINTERNAME,
            /// <summary>
            /// Search by Printer Location
            /// </summary>
            PRINTERLOCATION,
            /// <summary>
            /// Search by IP Address
            /// </summary>
            IPADDRESS,
            /// <summary>
            /// Search by Printer Type id
            /// </summary>
            PRINTERTYPEID,
            /// <summary>
            /// Search by site_id
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by Mater Entity Id
            /// </summary>
            MASTER_ENTIY_ID
        }

        /// <summary>
        /// PrinterTypes enum
        /// </summary>
        public enum PrinterTypes
        {
            ReceiptPrinter,
            KOTPrinter,
            TicketPrinter,
            CardPrinter,
            RFIDWBPrinter,
            KDSTerminal,
            RDSPrinter
        }

        public static PrinterTypes GetPrinterTypesEnumValue(string printerTypeString)
        {
            log.LogMethodEntry(printerTypeString);
            PrinterTypes printerType;
            if (Enum.TryParse(printerTypeString, out printerType) == false)
            {
                throw new InvalidEnumArgumentException(printerTypeString);
            } 
            log.LogMethodExit(printerType);
            return printerType;

         }

        private int printerId;
        private string printerName;
        private string printerLocation;
        private int kDSTerminal;
        private bool isActive;
        private DateTime creationDate;
        private string createdBy;
        private DateTime lastUpdateDate;
        private string lastUpdatedBy;
        private string ipAddress;
        private string remarks;
        private int printerTypeId;
        private int site_id;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        private PrinterTypes printerType;
        private List<int> printableProductIds;
        private int wbPrinterModel;
        private int paperSizeId;
        private List<PrinterProductsDTO> printProductsList;
        private List<PrinterDisplayGroupDTO> printerDisplayGroups;

        /// <summary>
        /// Default constructor
        /// </summary>
        public PrinterDTO()
        {
            log.LogMethodEntry();
            printerId = -1;
            isActive = true;
            site_id = -1;
            masterEntityId = -1;
            printerTypeId = -1;
            printProductsList = new List<PrinterProductsDTO>();
            printerDisplayGroups = new List<PrinterDisplayGroupDTO>();
            wbPrinterModel = -1;
            paperSizeId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// constructor with Required parameter
        /// </summary>
        public PrinterDTO(int printerId, string printerName, string printerLocation, int kDSTerminal, bool isActive,
                           string ipAddress, string remarks, int printerTypeId, PrinterTypes printerType, int model, int paperSizeId)
           : this()
        {
            log.LogMethodEntry(printerId, printerName, printerLocation, kDSTerminal, isActive, ipAddress, remarks, printerTypeId, printerType, model, paperSizeId);
            this.printerId = printerId;
            this.printerName = printerName;
            this.printerLocation = printerLocation;
            this.kDSTerminal = kDSTerminal;
            this.isActive = isActive;
            this.ipAddress = ipAddress;
            this.remarks = remarks;
            this.printerTypeId = printerTypeId;
            this.printerType = printerType;
            this.wbPrinterModel = model;
            this.paperSizeId = paperSizeId;
            log.LogMethodExit();
        }

        /// <summary>
        /// constructor with all the parameter
        /// </summary>
        public PrinterDTO(int printerId, string printerName, string printerLocation,
            int kDSTerminal, bool isActive, DateTime creationDate, string createdBy, DateTime lastUpdateDate,
            string lastUpdatedBy, string ipAddress, string remarks, int printerTypeId, PrinterTypes printerType, int site_id, string guid,
            bool synchStatus, int masterEntityId, int model, int paperSizeId)
            : this(printerId, printerName, printerLocation, kDSTerminal, isActive, ipAddress, remarks, printerTypeId, printerType, model, paperSizeId)
        {
            log.LogMethodEntry(printerId, printerName, printerLocation, kDSTerminal, isActive, creationDate, createdBy, lastUpdateDate,
                                lastUpdatedBy, ipAddress, remarks, printerTypeId, site_id, guid, synchStatus, masterEntityId, paperSizeId);
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
        public PrinterDTO(PrinterDTO printerDTO)
           : this(printerDTO.printerId, printerDTO.printerName, printerDTO.printerLocation, printerDTO.kDSTerminal, printerDTO.isActive, printerDTO.creationDate, printerDTO.createdBy,
                 printerDTO.lastUpdateDate, printerDTO.lastUpdatedBy, printerDTO.ipAddress, printerDTO.remarks, printerDTO.printerTypeId, printerDTO.printerType, printerDTO.site_id, 
                 printerDTO.guid, printerDTO.synchStatus, printerDTO.masterEntityId, printerDTO.wbPrinterModel, printerDTO.paperSizeId)
        {
            log.LogMethodEntry(printerDTO);
            if (printerDTO.printProductsList != null)
            {
                printProductsList = new List<PrinterProductsDTO>();
                foreach (var printerProductsDTO in printerDTO.printProductsList)
                {
                    printProductsList.Add(new PrinterProductsDTO(printerProductsDTO));
                }
            }
            if (printerDTO.printerDisplayGroups != null)
            {
                printerDisplayGroups = new List<PrinterDisplayGroupDTO>();
                foreach (var printerDisplayGroupDTO in printerDTO.printerDisplayGroups)
                {
                    printerDisplayGroups.Add(new PrinterDisplayGroupDTO(printerDisplayGroupDTO));
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the PrinterId field
        /// </summary>
        [DisplayName("Printer Id")]
        [ReadOnly(true)]
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
        /// Get/Set method of the PrinterName field
        /// </summary>
        [DisplayName("Printer Name")]
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
        [DisplayName("Printer Location")]
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
        /// Get/Set method of the IpAddress field
        /// </summary>
        [DisplayName("IP Address")]
        public string IpAddress
        {
            get
            {
                return ipAddress;
            }
            set
            {
                this.IsChanged = true;
                ipAddress = value;
            }
        }

        /// <summary>
        /// Get/Set method of the printerTypeId field
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
                this.IsChanged = true;
                printerTypeId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the printerTypeId field
        /// </summary>
        [DisplayName("Printer Type")]
        public PrinterTypes PrinterType
        {
            get
            {
                return printerType;
            }
            set
            {
                this.IsChanged = true;
                printerType = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Remarks field
        /// </summary>
        [DisplayName("Remarks")]
        public string Remarks
        {
            get
            {
                return remarks;
            }
            set
            {
                this.IsChanged = true;
                remarks = value;
            }
        }

        /// <summary>
        /// Get/Set method of the IsKDSTerminal field
        /// </summary>
        [DisplayName("KDSTerminal")]
        public int KDSTerminal
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
        /// Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("Active Flag")]
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
        [Browsable(false)]
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
        /// Get/Set method of the printableProductIds field
        /// </summary>
        public List<int> PrintableProductIds
        {
            get
            {
                return printableProductIds;
            }
            set
            {
                this.IsChanged = true;
                printableProductIds = value;
            }
        }

        /// <summary>
        /// Get/Set method of the guid field
        /// </summary>
        [Browsable(false)]
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
        [Browsable(false)]
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
        [Browsable(false)]
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
        /// Get/Set method of the PrinterId field
        /// </summary>
        [DisplayName("WBPrinterModel")]
        public int WBPrinterModel
        {
            get
            {
                return wbPrinterModel;
            }
            set
            {
                wbPrinterModel = value;
                this.IsChanged = true;
            }
        }


        /// <summary>
        /// Get/Set method of the paperSizeId field
        /// </summary>
        [DisplayName("PaperSizeId")]
        public int PaperSizeId
        {
            get
            {
                return paperSizeId;
            }
            set
            {
                paperSizeId = value;
                IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the Print only these product List field
        /// </summary>
        [Browsable(false)]
        public List<PrinterProductsDTO> PrintProductsList
        {
            get
            {
                return printProductsList;
            }

            set
            {
                printProductsList = value;
            }
        }
        /// <summary>
        /// Get/Set method of the Print These Display Groups field
        /// </summary>
        [Browsable(false)]
        public List<PrinterDisplayGroupDTO> PrinterDisplayGroups
        {
            get
            {
                return printerDisplayGroups;
            }

            set
            {
                printerDisplayGroups = value;
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
                    return notifyingObjectIsChanged || printerId < 0;
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
