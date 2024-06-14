/********************************************************************************************
 * Project Name - POS
 * Description  - Data object of POSMachineViewDTO
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By         Remarks          
 *********************************************************************************************
 *2.70        3- Jul- 2019   Girish Kundar       Modified : Added Constructor with required Parameter
 *2.110.0     11- Jan- 2021  Deeksha             Modified : POS UI redesign with REST API
 *2.130.0        12-Jul-2021    Lakshminarayana      Modified : Static menu enhancement
 *2.140.0     13-Dec-2021     Prajwal S           Modified : Added childs.
 ********************************************************************************************/
using System;
using Semnox.Parafait.Product;
using System.Collections.Generic;
using Semnox.Parafait.Device.Peripherals;

namespace Semnox.Parafait.POS
{
    /// <summary>
    /// This is the POSMachineViewDTO data object class. This acts as data holder for the POSMachine business object
    /// </summary>
    public class POSMachineContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int posMachineId;
        private string posName;
        private string attribute1;
        private int posTypeId;
        private int inventoryLocationId;
        private string guid;
        private List<PeripheralContainerDTO> peripheralContainerDTOList = new List<PeripheralContainerDTO>();
        private List<POSPrinterContainerDTO> pOSPrinterContainerDTOList = new List<POSPrinterContainerDTO>();
        private List<POSProductExclusionsContainerDTO> pOSProductExclusionsContainerDTOList = new List<POSProductExclusionsContainerDTO>();
        private List<POSPaymentModeInclusionContainerDTO> pOSPaymentModeInclusionContainerDTOList = new List<POSPaymentModeInclusionContainerDTO>();
        private List<ProductDisplayGroupFormatContainerDTO> productDisplayGroupFormatContainerDTOList = new List<ProductDisplayGroupFormatContainerDTO>();
        private List<int> includedProductIdList = new List<int>();
        private List<int> productMenuIdList = new List<int>();
        private List<int> excludedProductMenuPanelIdList = new List<int>();
        private List<POSCashdrawerContainerDTO> posCashdrawerContainerDTOList;
        /// <summary>
        /// Default constructor
        /// </summary>
        public POSMachineContainerDTO()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with Required data fields
        /// </summary>
        public POSMachineContainerDTO(int posMachineId, string posName, int inventoryLocationId, string guid, string ftPOSSystemId, int posTypeId)
            : this()
        {
            log.LogMethodEntry(posMachineId, posName, inventoryLocationId, guid);
            this.posMachineId = posMachineId;
            this.posName = posName;
            this.inventoryLocationId = inventoryLocationId;
            this.guid = guid;
            this.attribute1 = ftPOSSystemId;
            this.posTypeId = posTypeId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the POSMachineId field
        /// </summary>
        public int POSMachineId
        {
            get
            {
                return posMachineId;
            }

            set
            {
                posMachineId = value;
            }
        }



        /// <summary>
        /// Get/Set method of the posName field
        /// </summary>
        public string POSName
        {
            get
            {
                return posName;
            }

            set
            {
                posName = value;
            }
        }

        /// <summary>
        /// Get/Set method of the posTypeId field
        /// </summary>
        public int POSTypeId
        {
            get
            {
                return posTypeId;
            }

            set
            {
                posTypeId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the FTPOSSystemId field
        /// </summary>
        public string Attribute1
        {
            get
            {
                return attribute1;
            }

            set
            {
                attribute1 = value;
            }
        }

        /// <summary>
        /// Get/Set method of the inventoryLocationId field
        /// </summary>
        public int InventoryLocationId
        {
            get
            {
                return inventoryLocationId;
            }

            set
            {
                inventoryLocationId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the guid field
        /// </summary>
        public string Guid
        {
            get
            {
                return guid;
            }

            set
            {
                guid = value;
            }
        }

        /// <summary>
        /// Get/Set method of the peripheralContainerDTOList field
        /// </summary>
        public List<PeripheralContainerDTO> PeripheralContainerDTOList
        {
            get
            {
                return peripheralContainerDTOList;
            }

            set
            {
                peripheralContainerDTOList = value;
            }
        }

        /// <summary>
        /// Get/Set method of the POSPrinterContainerDTOList field
        /// </summary>
        public List<POSProductExclusionsContainerDTO> POSProductExclusionsContainerDTOList
        {
            get
            {
                return pOSProductExclusionsContainerDTOList;
            }

            set
            {
                pOSProductExclusionsContainerDTOList = value;
            }
        }

        /// <summary>
        /// Get/Set method of the POSPaymentModeInclusionContainerDTOList field
        /// </summary>
        public List<POSPaymentModeInclusionContainerDTO> POSPaymentModeInclusionContainerDTOList
        {
            get
            {
                return pOSPaymentModeInclusionContainerDTOList;
            }

            set
            {
                pOSPaymentModeInclusionContainerDTOList = value;
            }
        }

        /// <summary>
        /// Get/Set method of the peripheralContainerDTOList field
        /// </summary>
        public List<ProductDisplayGroupFormatContainerDTO> ProductDisplayGroupFormatContainerDTOList
        {
            get
            {
                return productDisplayGroupFormatContainerDTOList;
            }

            set
            {
                productDisplayGroupFormatContainerDTOList = value;
            }
        }

        /// <summary>
        /// Get/Set method of the POSPrinterContainerDTOList field
        /// </summary>
        public List<POSPrinterContainerDTO> POSPrinterContainerDTOList
        {
            get
            {
                return pOSPrinterContainerDTOList;
            }

            set
            {
                pOSPrinterContainerDTOList = value;
            }
        }

        /// <summary>
        /// Get/Set method of the guid field
        /// </summary>
        public List<int> IncludedProductIdList
        {
            get
            {
                return includedProductIdList;
            }

            set
            {
                includedProductIdList = value;
            }
        }


        /// <summary>
        /// Get/Set method of the productMenuIdList field
        /// </summary>
        public List<int> ProductMenuIdList
        {
            get
            {
                return productMenuIdList;
            }

            set
            {
                productMenuIdList = value;
            }
        }

        /// <summary>
        /// Get/Set method of the excludedProductMenuPanelIdList field
        /// </summary>
        public List<int> ExcludedProductMenuPanelIdList
        {
            get
            {
                return excludedProductMenuPanelIdList;
            }

            set
            {
                excludedProductMenuPanelIdList = value;
            }
        }

        /// <summary>
        /// Get/Set method of the POSCashdrawerContainerDTOList field
        /// </summary>
        public List<POSCashdrawerContainerDTO> POSCashdrawerContainerDTOList
        {
            get
            {
                return posCashdrawerContainerDTOList;
            }

            set
            {
                posCashdrawerContainerDTOList = value;
            }
        }

    }
}
