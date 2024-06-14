/********************************************************************************************
 * Project Name - POS
 * Description  - POSMachineMasterList class to get the List of 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         24-Aug-2020       Girish Kundar             Created : POS UI Redesign with REST API
 *2.130.0    12-Jul-2021       Lakshminarayana           Modified : Static menu enhancement
 ********************************************************************************************/
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Semnox.Core.Utilities;
namespace Semnox.Parafait.POS
{
    public static class POSMachineContainerList
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Cache<int, POSMachineContainer> posMachineContainerCache = new Cache<int, POSMachineContainer>();
        private static Timer refreshTimer;
        static POSMachineContainerList()
        {
            log.LogMethodEntry();
            refreshTimer = new Timer(DataRefreshFrequency.GetValue());
            refreshTimer.Elapsed += OnRefreshTimer;
            refreshTimer.Start();
            log.LogMethodExit();
        }

        private static void OnRefreshTimer(object sender, ElapsedEventArgs e)
        {
            log.LogMethodEntry();
            var uniqueKeyList = posMachineContainerCache.Keys;
            foreach (var uniqueKey in uniqueKeyList)
            {
                POSMachineContainer posMachineServerContainer;
                if (posMachineContainerCache.TryGetValue(uniqueKey, out posMachineServerContainer))
                {
                    posMachineContainerCache[uniqueKey] = posMachineServerContainer.Refresh();
                }
            }
            log.LogMethodExit();
        }

        private static POSMachineContainer GetPOSMachineContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            POSMachineContainer result = posMachineContainerCache.GetOrAdd(siteId, (k) => new POSMachineContainer(siteId));
            log.LogMethodExit(result);
            return result;
        }

        public static POSMachineContainerDTO GetPOSMachineContainerDTOOrDefault(int siteId, int posMachineId)
        {
            log.LogMethodEntry(siteId, posMachineId);
            POSMachineContainer posMachineContainer = GetPOSMachineContainer(siteId);
            POSMachineContainerDTO result = posMachineContainer.GetPOSMachineContainerDTOOrDefault(posMachineId);
            log.LogMethodExit(result);
            return result;
        }

        public static POSMachineContainerDTO GetPOSMachineContainerDTOOrDefault(int siteId, string machineName, string ipAddress, int posTypeId)
        {
            log.LogMethodEntry(siteId, machineName, ipAddress, posTypeId);
            POSMachineContainer posMachineContainer = GetPOSMachineContainer(siteId);
            POSMachineContainerDTO result = posMachineContainer.GetPOSMachineContainerDTOOrDefault(machineName, ipAddress, posTypeId);
            log.LogMethodExit(result);
            return result;
        }

        public static POSMachineContainerDTOCollection GetPOSMachineContainerDTOCollection(int siteId)
        {
            log.LogMethodEntry(siteId);
            POSMachineContainer posMachineContainer = GetPOSMachineContainer(siteId);
            POSMachineContainerDTOCollection result = posMachineContainer.GetPOSMachineContainerDTOCollection();
            return result;
        }

        public static List<POSMachineContainerDTO> GetPOSMachineContainerDTOList(int siteId)
        {
            log.LogMethodEntry(siteId);
            POSMachineContainer posMachineContainer = GetPOSMachineContainer(siteId);
            var result = posMachineContainer.GetPOSMachineContainerDTOList();
            return result;
        }

        /// <summary>
        /// rebuilds the container
        /// </summary>
        public static void Rebuild(int siteId)
        {
            log.LogMethodEntry();
            POSMachineContainer pOSMachineContainer = GetPOSMachineContainer(siteId);
            posMachineContainerCache[siteId] = pOSMachineContainer.Refresh();
            log.LogMethodExit();
        }
    }
}
