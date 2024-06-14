/********************************************************************************************
 * Project Name - Machine
 * Description  - MachineContainer class 
 *  
 **************
 **Version Log
 **************
 *Version     Date              Modified By        Remarks          
 *********************************************************************************************
 2.130.0      20-Jul-2021       Mushahid Faizan    Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Semnox.Parafait.Game
{
    public class MachineContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly Dictionary<int, MachineContainerDTO> machineIdmachineContainerDTODictionary = new Dictionary<int, MachineContainerDTO>();
        private readonly MachineContainerDTOCollection machineContainerDTOCollection;
        private readonly DateTime? machineModuleLastUpdateTime;
        private readonly int siteId;

        public MachineContainer(int siteId) : this(siteId, GetMachineDTOList(siteId), GetMachineModuleLastUpdateTime(siteId))
        {
            log.LogMethodEntry(siteId);
            log.LogMethodExit();
        }

        public MachineContainer(int siteId, List<MachineDTO> machineDTOList, DateTime? machineModuleLastUpdateTime)
        {
            log.LogMethodEntry(siteId);
            this.siteId = siteId;
            this.machineModuleLastUpdateTime = machineModuleLastUpdateTime;
            List<MachineContainerDTO> machineContainerDTOList = new List<MachineContainerDTO>();
            foreach (MachineDTO machineDTO in machineDTOList)
            {
                if (machineIdmachineContainerDTODictionary.ContainsKey(machineDTO.MachineId))
                {
                    continue;
                }
                MachineContainerDTO machineContainerDTO = new MachineContainerDTO(machineDTO.MachineId,
                                                                                  machineDTO.MachineName,
                                                                                  machineDTO.MachineAddress,
                                                                                  machineDTO.GameId,
                                                                                  machineDTO.MasterId,
                                                                                  machineDTO.Notes,
                                                                                  machineDTO.TicketAllowed,
                                                                                  machineDTO.TimerMachine,
                                                                                  machineDTO.TimerInterval,
                                                                                  machineDTO.GroupTimer,
                                                                                  machineDTO.NumberOfCoins,
                                                                                  machineDTO.TicketMode,
                                                                                  machineDTO.CustomDataSetId,
                                                                                  machineDTO.ThemeId,
                                                                                  machineDTO.ThemeNumber,
                                                                                  machineDTO.ShowAd,
                                                                                  machineDTO.IPAddress,
                                                                                  machineDTO.TCPPort,
                                                                                  machineDTO.MacAddress,
                                                                                  machineDTO.Description,
                                                                                  machineDTO.SerialNumber,
                                                                                  machineDTO.SoftwareVersion,
                                                                                  machineDTO.PurchasePrice,
                                                                                  machineDTO.ReaderType,
                                                                                  machineDTO.PayoutCost,
                                                                                  machineDTO.InventoryLocationId,
                                                                                  machineDTO.ReferenceMachineId,
                                                                                  machineDTO.ExternalMachineReference,
                                                                                  machineDTO.MachineTag,
                                                                                  machineDTO.CommunicationSuccessRatio,
                                                                                  machineDTO.PreviousMachineId,
                                                                                  machineDTO.NextMachineId,
                                                                                  machineDTO.MachineArrivalDate);
                machineIdmachineContainerDTODictionary.Add(machineDTO.MachineId, machineContainerDTO);
                GameContainerDTO gameContainerDTO = GameContainerList.GetGameContainerDTOOrDefault(siteId, machineDTO.GameId);
                if(gameContainerDTO != null)
                {
                    GameProfileContainerDTO gameProfileContainerDTO = GameProfileContainerList.GetGameProfileContainerDTOOrDefault(siteId, gameContainerDTO.GameProfileId);
                    if(gameProfileContainerDTO != null)
                    {
                        if(gameContainerDTO.GamePriceTierContainerDTOList != null && gameContainerDTO.GamePriceTierContainerDTOList.Any())
                        {
                            machineContainerDTO.GamePriceTierContainerDTOList = gameContainerDTO.GamePriceTierContainerDTOList;
                        }
                        else if(gameProfileContainerDTO.GamePriceTierContainerDTOList != null && gameProfileContainerDTO.GamePriceTierContainerDTOList.Any())
                        {
                            machineContainerDTO.GamePriceTierContainerDTOList = gameProfileContainerDTO.GamePriceTierContainerDTOList;
                        }
                    }
                }
                machineContainerDTOList.Add(machineContainerDTO);
            }
            machineContainerDTOCollection = new MachineContainerDTOCollection(machineContainerDTOList);
            log.LogMethodExit();
        }

        internal List<MachineContainerDTO> GetMachineContainerDTOList()
        {
            log.LogMethodEntry();
            log.LogMethodExit(machineContainerDTOCollection.MachineContainerDTOList);
            return machineContainerDTOCollection.MachineContainerDTOList;
        }

        private static List<MachineDTO> GetMachineDTOList(int siteId)
        {
            log.LogMethodEntry(siteId);
            List<MachineDTO> machineDTOList = null;
            try
            {
                MachineList machineList = new MachineList();
                List<KeyValuePair<MachineDTO.SearchByMachineParameters, string>> searchParameters = new List<KeyValuePair<MachineDTO.SearchByMachineParameters, string>>();
                searchParameters.Add(new KeyValuePair<MachineDTO.SearchByMachineParameters, string>(MachineDTO.SearchByMachineParameters.IS_ACTIVE, "1"));
                searchParameters.Add(new KeyValuePair<MachineDTO.SearchByMachineParameters, string>(MachineDTO.SearchByMachineParameters.SITE_ID, siteId.ToString()));
                machineDTOList = machineList.GetMachineList(searchParameters);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving the machines.", ex);
            }

            if (machineDTOList == null)
            {
                machineDTOList = new List<MachineDTO>();
            }
            log.LogMethodExit(machineDTOList);
            return machineDTOList;
        }

        private static DateTime? GetMachineModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            DateTime? result = null;
            try
            {
                MachineList machineList = new MachineList();
                result = machineList.GetMachineModuleLastUpdateTime(siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving the machine max last update date.", ex);
                result = null;
            }
            log.LogMethodExit(result);
            return result;
        }

        public MachineContainerDTO GetMachineContainerDTO(int machineId)
        {
            log.LogMethodEntry(machineId);
            if (machineIdmachineContainerDTODictionary.ContainsKey(machineId) == false)
            {
                string errorMessage = "Machine with machine Id :" + machineId + " doesn't exists.";
                log.LogMethodExit("Throwing Exception - " + errorMessage);
                throw new Exception(errorMessage);
            }
            MachineContainerDTO result = machineIdmachineContainerDTODictionary[machineId]; ;
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// gets the MachineContainer 
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns></returns>
        public MachineContainerDTO GetMachineContainerDTOOrDefault(int machineId)
        {
            log.LogMethodEntry(machineId);
            if (machineIdmachineContainerDTODictionary.ContainsKey(machineId) == false)
            {
                string message = "Machine with machineId : " + machineId + " doesn't exist.";
                log.LogMethodExit(null, message);
                return null;
            }
            var result = machineIdmachineContainerDTODictionary[machineId];
            log.LogMethodExit(result);
            return result;
        }

        public MachineContainerDTOCollection GetMachineContainerDTOCollection()
        {
            log.LogMethodEntry();
            log.LogMethodExit(machineContainerDTOCollection);
            return machineContainerDTOCollection;
        }

        public MachineContainer Refresh()
        {
            log.LogMethodEntry();
            DateTime? updateTime = GetMachineModuleLastUpdateTime(siteId);
            if (machineModuleLastUpdateTime.HasValue
                && machineModuleLastUpdateTime >= updateTime)
            {
                log.LogMethodExit(this, "No changes in machine since " + updateTime.Value.ToString(CultureInfo.InvariantCulture));
                return this;
            }
            MachineContainer result = new MachineContainer(siteId);
            log.LogMethodExit(result);
            return result;
        }

        public IEnumerable<MachineContainerDTO> GetActiveMachineContainerDTOList(Func<MachineContainerDTO, bool> predicate)
        {
            log.LogMethodEntry(predicate);
            IEnumerable<MachineContainerDTO> result;
            result = machineContainerDTOCollection.MachineContainerDTOList.Where(predicate);
            log.LogMethodExit(result);
            return result;
        }
    }
}
