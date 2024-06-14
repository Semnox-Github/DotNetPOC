/********************************************************************************************
 * Project Name - POSMachineDTO
 * Description  - Data object of the posmachine
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        02-Jan-2017   Raghuveera          Created 
 *2.60        11-Feb-2019    Mushahid Faizan  Modified - Added isActive SearchByParameter
 *2.60        22-Feb-2019    Mushahid Faizan  Modified - Added  Child DTO's :
                                                                POSPrinterDTO ,POSProductExclusionsDTO , ProductDisplayGroupFormatDTO
*2.00        04-Mar-2019   Indhu          Modified for Remote Shift Open/Close changes                                        
 *2.60        25-May-2019   Nitin Pai      Modified for guest app\ webmanagement studio
 *2.70        08-Jul-2019   Deeksha        Modified:Added new Constructor with required fields
 *                                         Added LastUpdatedBy,CreationDate & lastUpdateDate fields.
 *2.70.2      04-Feb-2020   Nitin Pai      Guest App phase 2 changes   
 *2.80        10-May-2020   Girish Kundar  Modified: REST API Changes merge from WMS  
 *2.80.0      02-Apr-2020   Akshay G       Added POS_MACHINE_ID_LIST searchParameter
 *2.90.0      02-Jun-2020   Girish Kundar  Modified :  added posPaymentModeInclusionDTOList for Payment mode display enhancement
 *2.130.0     12-Jul-2021   Lakshminarayana Modified : Static menu enhancement
 ********************************************************************************************/
using Semnox.Parafait.Device.Peripherals;
using Semnox.Parafait.Product;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Semnox.Parafait.POS
{
    /// <summary>
    /// This is the POS Machine data object class. This acts as data holder for the POS Machine business object
    /// </summary>
    public class POSMachineDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByPOSMachineParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByPOSMachineParameters
        {
            /// <summary>
            /// Search by POS MACHINE ID field
            /// </summary>
            POS_MACHINE_ID,
            /// <summary>
            /// Search by POS NAME field
            /// </summary>
            POS_NAME ,
            /// <summary>
            /// Search by LEGAL ENTITY field
            /// </summary>
            LEGAL_ENTITY ,
            /// <summary>
            /// Search by POS TYPE ID field
            /// </summary>
            POS_TYPE_ID ,
            /// <summary>
            /// Search by INVENTORY LOCATION ID field
            /// </summary>            
            INVENTORY_LOCATION_ID,
            /// <summary>
            /// Search by IP ADDRESS field
            /// </summary> 
            IP_ADDRESS ,
            /// <summary>
            /// Search by COMPUTER NAME field
            /// </summary>
            COMPUTER_NAME ,
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID ,
            ///<summary>
            ///Search by POS TYPE ID LIST field
            ///</summary>
            POS_TYPE_ID_LIST ,
            ///<summary>
            ///Search by POS NAME LIST field
            ///</summary>
            POS_NAME_LIST ,
            ///<summary>
            ///Search by MASTER ENTITY ID field
            ///</summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by  isActive field
            /// </summary>
            ISACTIVE,
            /// <summary>
            /// Search by POS NAME OR COMPUTER NAME field
            /// </summary>
            POS_OR_COMPUTER_NAME,
            /// <summary>
            /// Search by POS NAME OR COMPUTER NAME field
            /// </summary>
            POS_MACHINE_ID_LIST,
        }
        private int posMachineId;
        private string posName;
        private string legalEntity;
        private string remarks;
        private string friendlyName;
        private int posTypeId;
        private int inventoryLocationId;
        private string ipAddress;
        private string computerName;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;
        private DateTime dayBeginTime;
        private DateTime dayEndTime;
        private DateTime xReportRunTime;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private string attribute1;
        private string attribute2;
        private string attribute3;
        private string attribute4;
        private string attribute5;
        private bool isActive;
        private List<POSPrinterDTO> posPrinterDTOList;
        private List<POSProductExclusionsDTO> posProductExclusionDTOList;
        private List<PeripheralsDTO> peripheralsDTOList;
        private List<ProductDisplayGroupFormatDTO> posProductDisplayList; // This list for Inclusion List
        private List<POSPaymentModeInclusionDTO> posPaymentModeInclusionDTOList;
        private List<ProductMenuPOSMachineMapDTO> productMenuPOSMachineMapDTOList;
        private List<ProductMenuPanelExclusionDTO> productMenuPanelExclusionDTOList;
        private List<POSCashdrawerDTO> posCashdrawerDTOList;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public POSMachineDTO()
        {
            log.LogMethodEntry();
            posMachineId = -1;
            posTypeId = -1;
            inventoryLocationId = -1;
            isActive = true;
            masterEntityId = -1;
            siteId = -1;
            posPrinterDTOList = new List<POSPrinterDTO>();
            posProductExclusionDTOList = new List<POSProductExclusionsDTO>();
            peripheralsDTOList = new List<PeripheralsDTO>();
            posProductDisplayList = new List<ProductDisplayGroupFormatDTO>();
            posPaymentModeInclusionDTOList = new List<POSPaymentModeInclusionDTO>();
            productMenuPOSMachineMapDTOList = new List<ProductMenuPOSMachineMapDTO>();
            productMenuPanelExclusionDTOList = new List<ProductMenuPanelExclusionDTO>();
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with required fields
        /// </summary>
        public POSMachineDTO(int posMachineId, string posName, string legalEntity, string remarks,string friendlyName,
                             int posTypeId, int inventoryLocationId, string ipAddress,string computerName,
                             DateTime dayBeginTime,DateTime dayEndTime, DateTime xReportRunTime, bool isActive,
                             string attribute1, string attribute2, string attribute3, 
                            string attribute4, string attribute5)
            :this()
        {
            log.LogMethodEntry(posMachineId,posName, legalEntity,remarks, friendlyName, posTypeId, inventoryLocationId,
                               ipAddress, computerName,dayBeginTime, dayEndTime,xReportRunTime, isActive,
                               attribute1, attribute2, attribute3, attribute4, attribute5);
            this.posMachineId = posMachineId;
            this.posName = posName;
            this.legalEntity = legalEntity;
            this.remarks = remarks;
            this.friendlyName = friendlyName;
            this.posTypeId = posTypeId;
            this.inventoryLocationId = inventoryLocationId;
            this.ipAddress = ipAddress;
            this.computerName = computerName;
            this.dayBeginTime = dayBeginTime;
            this.dayEndTime = dayEndTime;
            this.xReportRunTime = xReportRunTime;
            this.isActive = isActive;
            this.attribute1 = attribute1;
            this.attribute2 = attribute2;
            this.attribute3 = attribute3;
            this.attribute4 = attribute4;
            this.attribute5 = attribute5;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public POSMachineDTO(int posMachineId,string posName,string legalEntity,string remarks,
                             string friendlyName,int posTypeId,int inventoryLocationId,string ipAddress,
                             string computerName,string guid,int siteId,bool synchStatus,int masterEntityId, DateTime dayBeginTime,
                             DateTime dayEndTime, DateTime xReportRunTime,DateTime creationDate,
                             string lastUpdatedBy,DateTime lastUpdateDate, bool isActive,
                             string attribute1, string attribute2, string attribute3, string attribute4, string attribute5)
            :this(posMachineId, posName, legalEntity, remarks, friendlyName, posTypeId, inventoryLocationId,ipAddress,
                 computerName, dayBeginTime, dayEndTime, xReportRunTime, isActive, attribute1, attribute2, attribute3, attribute4, attribute5)
        {
            log.LogMethodEntry(posMachineId, posName, legalEntity, remarks, friendlyName, posTypeId, inventoryLocationId,
                               ipAddress, computerName, guid, siteId, synchStatus, masterEntityId,dayBeginTime, dayEndTime,
                               xReportRunTime, creationDate, lastUpdatedBy,lastUpdateDate, isActive, 
                               attribute1, attribute2, attribute3, attribute4, attribute5);
            this.masterEntityId = masterEntityId;
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.creationDate = creationDate;
            this.lastUpdatedBy = LastUpdatedBy;
            this.lastUpdateDate = LastUpdateDate;
            this.isActive = isActive;
            this.peripheralsDTOList = new List<PeripheralsDTO>();
            this.posPrinterDTOList = new List<POSPrinterDTO>();
            this.posProductExclusionDTOList = new List<POSProductExclusionsDTO>();
            this.PosProductDisplayList = new List<ProductDisplayGroupFormatDTO>();
            this.productMenuPOSMachineMapDTOList = new List<ProductMenuPOSMachineMapDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the POSMachineId field
        /// </summary>
        [DisplayName("POS MachineId")]
        [ReadOnly(true)]
        public int POSMachineId
        {
            get {return posMachineId; }
            set { posMachineId = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the POS Name field
        /// </summary>
        [DisplayName("POS Name")]
        public string POSName
        {
            get { return posName; }
            set { posName = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the Legal Entity field
        /// </summary>
        [DisplayName("Legal Entity")]
        public string LegalEntity
        {
            get { return legalEntity; }
            set { legalEntity = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the Remarks field
        /// </summary>
        [DisplayName("Remarks")]
        public string Remarks
        {
            get { return remarks; }
            set { remarks = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the Friendly Name field
        /// </summary>
        [DisplayName("Friendly Name")]
        public string FriendlyName
        {
            get { return friendlyName; }
            set { friendlyName = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the POS TypeId field
        /// </summary>
        [DisplayName("POS TypeId")]
        public int POSTypeId
        {
            get { return posTypeId; }
            set { posTypeId = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the Inventory LocationId field
        /// </summary>
        [DisplayName("Inventory LocationId")]
        public int InventoryLocationId
        {
            get { return inventoryLocationId; }
            set { inventoryLocationId = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the IP Address field
        /// </summary>
        [DisplayName("IP Address")]
        public string IPAddress
        {
            get { return ipAddress; }
            set { ipAddress = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the Computer Name field
        /// </summary>
        [DisplayName("Computer Name")]
        public string ComputerName
        {
            get { return computerName; }
            set { computerName = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        public string Guid
        {
            get { return guid; }
            set { guid = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("Site Id")]
        public int SiteId
        {
            get { return siteId; }
            set { siteId = value; }
        }

        /// <summary>
        /// Get/Set method of the Synch Status field
        /// </summary>
        [DisplayName("Synch Status")]
        public bool SynchStatus
        {
            get { return synchStatus; }
            set { synchStatus = value; }
        }

        /// <summary>
        /// Get/Set method of the Master EntityId field
        /// </summary>
        [DisplayName("Master EntityId")]
        public int MasterEntityId
        {
            get { return masterEntityId; }
            set { masterEntityId = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the DayBeginTime field
        /// </summary>
        [DisplayName("Day Begin Time")]
        public DateTime DayBeginTime
        {
            get { return dayBeginTime; }
            set { dayBeginTime = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the DayEndTime field
        /// </summary>
        [DisplayName("Day End Time")]
        public DateTime DayEndTime
        {
            get { return dayEndTime; }
            set { dayEndTime = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the XReportRunTime field
        /// </summary>
        [DisplayName("X-Report Run Time")]
        public DateTime XReportRunTime
        {
            get { return xReportRunTime; }
            set { xReportRunTime = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("IsActive")]
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        [DisplayName("CreationDate")]
        public DateTime CreationDate
        {
            get { return creationDate; }
            set { creationDate = value;}
        }


        /// <summary>
        /// Get/Set method of the LastUpdateBy field
        /// </summary>
        [DisplayName("LastUpdatedBy")]
        public String LastUpdatedBy
        {
            get { return lastUpdatedBy; }
            set { lastUpdatedBy = value;}
        }

        /// <summary>
        /// Get/Set method of the LastUpdateDate field
        /// </summary>
        [DisplayName("LastUpdateDate")]
        public DateTime LastUpdateDate
        {
            get { return lastUpdateDate; }
            set { lastUpdateDate = value;}
        }


        /// <summary>
        /// Attribute1
        /// </summary>
        public string Attribute1
        {
            get { return attribute1; }
            set { this.IsChanged = true; attribute1 = value; }
        }

        /// <summary>
        /// Attribute2
        /// </summary>
        public string Attribute2
        {
            get { return attribute2; }
            set { this.IsChanged = true; attribute2 = value; }
        }

        /// <summary>
        /// Attribute3
        /// </summary>
        public string Attribute3
        {
            get { return attribute3; }
            set { this.IsChanged = true; attribute3 = value; }
        }

        /// <summary>
        /// Attribute4
        /// </summary>
        public string Attribute4
        {
            get { return attribute4; }
            set { this.IsChanged = true; attribute4 = value; }
        }

        /// <summary>
        /// Attribute5
        /// </summary>
        public string Attribute5
        {
            get { return attribute5; }
            set { this.IsChanged = true; attribute5 = value; }
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
                    return notifyingObjectIsChanged || posMachineId < 0;
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
        /// Get/Set method of the PeripheralsDTOList field
        /// </summary>
        [Browsable(false)]
        public List<PeripheralsDTO> PeripheralsDTOList
        {
            get
            {
                return peripheralsDTOList;
            }

            set
            {
                peripheralsDTOList = value;
            }
        }

        /// <summary>
        /// Get/Set method of the posPrinterDTOList field
        /// </summary>
        [Browsable(false)]
        public List<POSPrinterDTO> PosPrinterDtoList
        {
            get
            {
                return posPrinterDTOList;
            }

            set
            {
                posPrinterDTOList = value;
            }
        }

        /// <summary>
        /// Get/Set method of the PosProductExclusionDtoList field
        /// </summary>
        [Browsable(false)]
        public List<POSProductExclusionsDTO> PosProductExclusionDtoList
        {
            get
            {
                return posProductExclusionDTOList;
            }

            set
            {
                posProductExclusionDTOList = value;
            }
        }

        /// <summary>
        /// Get/Set method of the PosProductDisplayList field
        /// </summary>
        [Browsable(false)]
        public List<ProductDisplayGroupFormatDTO> PosProductDisplayList
        {
            get
            {
                return posProductDisplayList;
            }

            set
            {
                posProductDisplayList = value;
            }
        }
        /// <summary>
        /// Get/Set method of the posPaymentModeInclusionDTOList field
        /// </summary>
        [Browsable(false)]
        public List<POSPaymentModeInclusionDTO> POSPaymentModeInclusionDTOList
        {
            get
            {
                return posPaymentModeInclusionDTOList;
            }

            set
            {
                posPaymentModeInclusionDTOList = value;
            }
        }


        /// <summary>
        /// Get/Set method of the productMenuPOSMachineMapDTOList field
        /// </summary>
        [Browsable(false)]
        public List<ProductMenuPOSMachineMapDTO> ProductMenuPOSMachineMapDTOList
        {
            get
            {
                return productMenuPOSMachineMapDTOList;
            }

            set
            {
                productMenuPOSMachineMapDTOList = value;
            }
        }

        /// <summary>
        /// Get/Set method of the productMenuPanelExclusionDTOList field
        /// </summary>
        [Browsable(false)]
        public List<ProductMenuPanelExclusionDTO> ProductMenuPanelExclusionDTOList
        {
            get
            {
                return productMenuPanelExclusionDTOList;
            }

            set
            {
                productMenuPanelExclusionDTOList = value;
            }
        }

        /// <summary>
        /// Get/Set method of the posCashdrawerDTOList field
        /// </summary>
        [Browsable(false)]
        public List<POSCashdrawerDTO> POSCashdrawerDTOList
        {
            get
            {
                return posCashdrawerDTOList;
            }

            set
            {
                posCashdrawerDTOList = value;
            }
        }
        /// <summary>
        /// Returns whether the PosMachineDTO changed or any of its posProductDisplayList  are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (posPrinterDTOList != null &&
                    posPrinterDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                if (posProductExclusionDTOList != null &&
                    posProductExclusionDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                if (peripheralsDTOList != null &&
                    peripheralsDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                if (posProductDisplayList != null &&
                   posProductDisplayList.Any(x => x.IsChanged))
                {
                    return true;
                }
                if (posPaymentModeInclusionDTOList != null &&
                   posPaymentModeInclusionDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                if(productMenuPOSMachineMapDTOList != null &&
                    productMenuPOSMachineMapDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                if (productMenuPanelExclusionDTOList != null &&
                    productMenuPanelExclusionDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                return false;
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
