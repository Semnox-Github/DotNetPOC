/********************************************************************************************
 * Project Name - POS
 * Description  - POSMachineContainer class holds all the POS machines
 *  
 **************
 * Version Log
 **************
 * Version       Date           Modified By          Remarks          
 *********************************************************************************************
 *2.110.0        04-Dec-2020    Vikas Dwivedi        Modified.
 *2.110.0        11- Jan- 2021  Deeksha              Modified : POS UI redesign with REST API
 *2.130.0        12-Jul-2021    Lakshminarayana      Modified : Static menu enhancement 
 *2.140.0        13-Dec-2021    Prajwal S            Modified : Added Childs of POSMachine DTO.
 **********************************************************************************************/
using Semnox.Parafait.Product;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Semnox.Parafait.Device.Peripherals;

namespace Semnox.Parafait.POS
{
    /// <summary>
    /// Class holds the parafait default values.
    /// </summary>
    public class POSMachineContainer
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly List<POSMachineDTO> posMachineDTOList;
        private readonly POSMachineContainerDTOCollection posMachineContainerDTOCollection;
        private readonly ConcurrentDictionary<int, POSMachineDTO> posMachineDTODictionary = new ConcurrentDictionary<int, POSMachineDTO>();
        private readonly ConcurrentDictionary<int, POSMachineContainerDTO> posMachineContainerDTODictionary = new ConcurrentDictionary<int, POSMachineContainerDTO>();
        private readonly DateTime? posModuleLastUpdateTime;
        private readonly int siteId;
        //private readonly DateTime? maxLastUpdateTime;

        public POSMachineContainer(int siteId) : this(siteId, GetPosMachineDTOList(siteId), GetPOSMachineModuleLastUpdateTime(siteId))
        {
            log.LogMethodEntry(siteId);
            log.LogMethodExit();
        }

        public POSMachineContainer(int siteId, List<POSMachineDTO> posMachineDTOList, DateTime? posModuleLastUpdateTime)
        {
            log.LogMethodEntry(siteId);
            this.siteId = siteId;
            this.posMachineDTOList = posMachineDTOList;
            this.posModuleLastUpdateTime = posModuleLastUpdateTime;
            //maxLastUpdateTime = GetMaxUpdateTime();
            List<POSMachineContainerDTO> posMachineContainerDTOList = new List<POSMachineContainerDTO>();
            posMachineDTODictionary = new ConcurrentDictionary<int, POSMachineDTO>();
            if (posMachineDTOList != null && posMachineDTOList.Any())
            {
                foreach (POSMachineDTO posMachineDTO in posMachineDTOList)
                {
                    POSMachineContainerDTO posMachineContainerDTO = CreatePOSMachineContainerDTO(posMachineDTO);
                    posMachineContainerDTOList.Add(posMachineContainerDTO);
                    posMachineDTODictionary[posMachineDTO.POSMachineId] = posMachineDTO;
                    posMachineContainerDTODictionary[posMachineDTO.POSMachineId] = posMachineContainerDTO;
                }
            }
            else
            {
                posMachineDTOList = new List<POSMachineDTO>();
            }
            posMachineContainerDTOCollection = new POSMachineContainerDTOCollection(posMachineContainerDTOList);
            log.LogMethodExit();
        }
        private static List<POSMachineDTO> GetPosMachineDTOList(int siteId)
        {
            log.LogMethodEntry(siteId);
            List<POSMachineDTO> posMachineDTOList = null;
            try
            {
                POSMachineList posMachineList = new POSMachineList();
                List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>> searchParameters = new List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>>();
                searchParameters.Add(new KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>(POSMachineDTO.SearchByPOSMachineParameters.ISACTIVE, "1"));
                searchParameters.Add(new KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>(POSMachineDTO.SearchByPOSMachineParameters.SITE_ID, siteId.ToString()));
                posMachineDTOList = posMachineList.GetAllPOSMachines(searchParameters, true, true);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving the POS Machine.", ex);
            }
            if (posMachineDTOList == null)
            {
                posMachineDTOList = new List<POSMachineDTO>();
            }
            log.LogMethodExit(posMachineDTOList);
            return posMachineDTOList;
        }

        internal List<POSMachineContainerDTO> GetPOSMachineContainerDTOList()
        {
            log.LogMethodEntry();
            log.LogMethodExit(posMachineContainerDTOCollection.POSMachineContainerDTOList);
            return posMachineContainerDTOCollection.POSMachineContainerDTOList;
        }

        private static DateTime? GetPOSMachineModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            DateTime? result = null;
            try
            {
                POSMachineList posMachineList = new POSMachineList();
                result = posMachineList.GetPOSModuleLastUpdateTime(siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving the POSMachine max last update date.", ex);
                result = null;
            }
            log.LogMethodExit(result);
            return result;
        }

        private POSMachineContainerDTO CreatePOSMachineContainerDTO(POSMachineDTO posMachineDTO)
        {
            log.LogMethodEntry(posMachineDTO);
            POSMachineContainerDTO posMachineContainerDTO = new POSMachineContainerDTO(posMachineDTO.POSMachineId, posMachineDTO.POSName, posMachineDTO.InventoryLocationId, posMachineDTO.Guid, posMachineDTO.Attribute1, posMachineDTO.POSTypeId);
            if (posMachineDTO.PeripheralsDTOList != null)
            {                                           
                posMachineContainerDTO.PeripheralContainerDTOList.AddRange(posMachineDTO.PeripheralsDTOList.Select(x => new PeripheralContainerDTO(x.DeviceId, x.DeviceName, x.PosMachineId, x.DeviceType, x.DeviceSubType, x.Vid, x.Pid, x.OptionalString, x.EnableTagDecryption, x.ExcludeDecryptionForTagLength, x.ReaderIsForRechargeOnly)));
            }
            if (posMachineDTO.PosProductDisplayList != null && posMachineDTO.PosProductDisplayList.Any())
            {
                List<int> includedDisplayGroupIdList = posMachineDTO.PosProductDisplayList.Select(x => x.Id).ToList();
                includedDisplayGroupIdList.Add(-1);
                List<ProductsContainerDTO> productsContainerDTOList = ProductsContainerList.GetProductContainerDTOListOfDisplayGroups(siteId, includedDisplayGroupIdList);
                posMachineContainerDTO.IncludedProductIdList = productsContainerDTOList.Where(x => x.POSTypeId == posMachineDTO.POSTypeId || x.POSTypeId == -1).Select(x => x.ProductId).ToList();
            }
            // Build cashdrawers
            if (posMachineDTO.POSCashdrawerDTOList != null && posMachineDTO.POSCashdrawerDTOList.Any())
            {
                posMachineContainerDTO.POSCashdrawerContainerDTOList = new List<POSCashdrawerContainerDTO>();
                foreach (POSCashdrawerDTO posCashdrawerDTO in posMachineDTO.POSCashdrawerDTOList)
                {
                    POSCashdrawerContainerDTO pOSCashdrawerContainerDTO = new POSCashdrawerContainerDTO(posCashdrawerDTO.POSCashdrawerId,
                                                                              posCashdrawerDTO.POSMachineId,
                                                                              posCashdrawerDTO.CashdrawerId,
                                                                              posCashdrawerDTO.IsActive);
                    posMachineContainerDTO.POSCashdrawerContainerDTOList.Add(pOSCashdrawerContainerDTO);
                }
            }
            if (posMachineDTO.PosPrinterDtoList != null && posMachineDTO.PosPrinterDtoList.Any())
            {
                foreach (POSPrinterDTO pOSPrinterDTO in posMachineDTO.PosPrinterDtoList)
                {
                    POSPrinterContainerDTO pOSPrinterContainerDTO = new POSPrinterContainerDTO(pOSPrinterDTO.POSPrinterId, pOSPrinterDTO.POSMachineId, pOSPrinterDTO.PrinterId, pOSPrinterDTO.POSTypeId, pOSPrinterDTO.SecondaryPrinterId, pOSPrinterDTO.OrderTypeGroupId
                                                                           , pOSPrinterDTO.PrintTemplateId, pOSPrinterDTO.PrinterTypeId);
                    posMachineContainerDTO.POSPrinterContainerDTOList.Add(pOSPrinterContainerDTO);
                }
            }
            if (posMachineDTO.PosProductExclusionDtoList != null && posMachineDTO.PosProductExclusionDtoList.Any())
            {
                foreach (POSProductExclusionsDTO pOSProductExclusionsDTO in posMachineDTO.PosProductExclusionDtoList)
                {
                    POSProductExclusionsContainerDTO pOSProductExclusionsContainerDTO = new POSProductExclusionsContainerDTO(pOSProductExclusionsDTO.ExclusionId, pOSProductExclusionsDTO.PosMachineId, pOSProductExclusionsDTO.ProductGroup, pOSProductExclusionsDTO.PosTypeId, pOSProductExclusionsDTO.ProductDisplayGroupFormatId);
                    posMachineContainerDTO.POSProductExclusionsContainerDTOList.Add(pOSProductExclusionsContainerDTO);
                }
            }
            if (posMachineDTO.POSPaymentModeInclusionDTOList != null && posMachineDTO.POSPaymentModeInclusionDTOList.Any())
            {
                foreach (POSPaymentModeInclusionDTO pOSPaymentModeInclusionDTO in posMachineDTO.POSPaymentModeInclusionDTOList)
                {
                    POSPaymentModeInclusionContainerDTO pOSPaymentModeInclusionContainerDTO = new POSPaymentModeInclusionContainerDTO(pOSPaymentModeInclusionDTO.POSPaymentModeInclusionId, pOSPaymentModeInclusionDTO.POSMachineId, pOSPaymentModeInclusionDTO.PaymentModeId, pOSPaymentModeInclusionDTO.FriendlyName);
                    posMachineContainerDTO.POSPaymentModeInclusionContainerDTOList.Add(pOSPaymentModeInclusionContainerDTO);
                }
            }
            if(posMachineDTO.PosProductDisplayList != null && posMachineDTO.PosProductDisplayList.Any())
            {
                foreach(ProductDisplayGroupFormatDTO productDisplayGroupFormatDTO in posMachineDTO.PosProductDisplayList)
                {
                    ProductDisplayGroupFormatContainerDTO productDisplayGroupFormatContainerDTO = new ProductDisplayGroupFormatContainerDTO(productDisplayGroupFormatDTO.Id, productDisplayGroupFormatDTO.DisplayGroup, productDisplayGroupFormatDTO.SortOrder, productDisplayGroupFormatDTO.ImageFileName,
                                                                                                                                             productDisplayGroupFormatDTO.ButtonColor, productDisplayGroupFormatDTO.TextColor, productDisplayGroupFormatDTO.Font, productDisplayGroupFormatDTO.Description, productDisplayGroupFormatDTO.ExternalSourceReference, productDisplayGroupFormatDTO.BackgroundImageFileName);
                    posMachineContainerDTO.ProductDisplayGroupFormatContainerDTOList.Add(productDisplayGroupFormatContainerDTO);
                }
            }
            if (posMachineDTO.ProductMenuPOSMachineMapDTOList != null &&
               posMachineDTO.ProductMenuPOSMachineMapDTOList.Any())
            {
                posMachineContainerDTO.ProductMenuIdList = posMachineDTO.ProductMenuPOSMachineMapDTOList.Select(x => x.MenuId).ToList();
            }
            if (posMachineDTO.ProductMenuPanelExclusionDTOList != null &&
                posMachineDTO.ProductMenuPanelExclusionDTOList.Any())
            {
                posMachineContainerDTO.ExcludedProductMenuPanelIdList = posMachineDTO.ProductMenuPanelExclusionDTOList.Select(x => x.PanelId).ToList();
            }
            log.LogMethodExit(posMachineContainerDTO);
            return posMachineContainerDTO;
        }

        public POSMachineContainerDTOCollection GetPOSMachineContainerDTOCollection()
        {
            log.LogMethodEntry();
            log.LogMethodExit(posMachineContainerDTOCollection);
            return posMachineContainerDTOCollection;
        }

        public POSMachineContainerDTO GetPOSMachineContainerDTOOrDefault(int posMachineId)
        {
            log.LogMethodEntry(posMachineId);
            POSMachineContainerDTO result = null;
            if (posMachineContainerDTODictionary.ContainsKey(posMachineId) == false)
            {
                string message = "POS machine with posMachineId : " + posMachineId + " doesn't exist.";
                log.LogMethodExit(result, message);
                return result;
            }
            result = posMachineContainerDTODictionary[posMachineId];
            log.LogMethodExit(result);
            return result;
        }

        public POSMachineContainerDTO GetPOSMachineContainerDTOOrDefault(string machineName, string ipAddress, int posTypeId)
        {
            log.LogMethodEntry(machineName, ipAddress, posTypeId);
            POSMachineContainerDTO result = null;
            POSMachineDTO posMachineDTO = GetPOSMachineDTO(machineName, ipAddress, posTypeId);
            if (posMachineDTO != null)
            {
                result = posMachineContainerDTODictionary[posMachineDTO.POSMachineId];
            }
            log.LogMethodExit(result);
            return result;
        }

        private POSMachineDTO GetPOSMachineDTO(string machineName, string ipAddress, int posTypeId)
        {
            POSMachineDTO result = null;
            List<POSMachineDTO> filterdPOSMachineDTOList = posMachineDTOList;
            if (posTypeId > -1)
            {
                filterdPOSMachineDTOList = posMachineDTOList.Where(x => x.POSTypeId == posTypeId).ToList();
                if (filterdPOSMachineDTOList.Count == 1)
                {
                    result = filterdPOSMachineDTOList[0];
                    log.LogMethodExit(result, "exactly one POS defined for counter of user");
                    return result;
                }
            }
            if (string.IsNullOrWhiteSpace(ipAddress) == false)
            {
                result = filterdPOSMachineDTOList.FirstOrDefault(x => x.IPAddress.ToLower() == ipAddress.ToLower());
                if (result != null)
                {
                    log.LogMethodExit(result, "POS with IPAddress found");
                    return result;
                }
            }
            if (string.IsNullOrWhiteSpace(machineName) == false)
            {
                result = filterdPOSMachineDTOList.FirstOrDefault(x => x.ComputerName.ToLower() == machineName.ToLower() || x.POSName.ToLower() == machineName.ToLower() || (string.IsNullOrWhiteSpace(x.ComputerName) && x.POSName == machineName.ToLower()));
                if (result != null)
                {
                    log.LogMethodExit(result, "POS with ComputerName or POSName found");
                    return result;
                }
                result = filterdPOSMachineDTOList.FirstOrDefault(x => string.IsNullOrWhiteSpace(x.FriendlyName) == false && machineName.Contains(x.FriendlyName));
                if (result != null)
                {
                    log.LogMethodExit(result, "POS with FriendlyName found");
                }
                else
                {
                    log.LogMethodExit(result, "Unable to find a POS");
                }
            }
            else
            {
                log.LogMethodExit(result, "Unable to find a POS");
            }
            return result;
        }

        public POSMachineContainer Refresh()
        {
            log.LogMethodEntry();
            POSMachineList pOSMachineListBL = new POSMachineList();
            DateTime? updateTime = pOSMachineListBL.GetPOSModuleLastUpdateTime(/*executionContext.GetIsCorporate() ? executionContext.GetSiteId() : -1*/siteId);
            //DateTime? updateTime = GetMaxUpdateTime();
            if (posModuleLastUpdateTime.HasValue
                && posModuleLastUpdateTime >= updateTime)
            {
                log.LogMethodExit(this, "No changes in POSMachine since " + updateTime.Value.ToString(CultureInfo.InvariantCulture));
                return this;
            }
            //ProductsContainerList.Rebuild(siteId);
            POSMachineContainer result = new POSMachineContainer(siteId);
            log.LogMethodExit(result);
            return result;
        }


        //private DateTime? GetMaxUpdateTime()
        //{
        //    log.LogMethodEntry();
        //    ProductsList productsList = new ProductsList();
        //    POSMachineList posMachineList = new POSMachineList();
        //    DateTime? productModuleMaxUpdateTime = productsList.GetProductsLastUpdateTime(siteId);
        //    DateTime? posMachineipModuleMaxUpdateTime = posMachineList.GetPOSModuleLastUpdateTime(siteId);
        //    DateTime? result = productModuleMaxUpdateTime > posMachineipModuleMaxUpdateTime ? productModuleMaxUpdateTime : posMachineipModuleMaxUpdateTime;
        //    log.LogMethodExit(result);
        //    return result;
        //}
    }
}
