/********************************************************************************************
 * Project Name - Games
 * Description  - MachineMasterList class to get the List of games from the container API
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         24-Aug-2020       Girish Kundar             Created : POS UI Redesign with REST API
 2.110.0     14-Dec-2020       Prajwal S                 Updated to new changes.
 ********************************************************************************************/
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Game
{
    public static class MachineContainerList
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly ConcurrentDictionary<int, MachineContainer> machineContainerDictionary = new ConcurrentDictionary<int, MachineContainer>();
        private static Timer refreshTimer;

        static MachineContainerList()
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
            List<int> uniqueKeyList = machineContainerDictionary.Keys.ToList();
            foreach (var uniqueKey in uniqueKeyList)
            {
                MachineContainer machineContainer;
                if (machineContainerDictionary.TryGetValue(uniqueKey, out machineContainer))
                {
                    machineContainerDictionary[uniqueKey] = machineContainer.Refresh();
                }
            }
            log.LogMethodExit();
        }


        private static MachineContainer GetMachineContainer(int siteId) //added
        {
            log.LogMethodEntry(siteId);
            if (machineContainerDictionary.ContainsKey(siteId) == false)
            {
                machineContainerDictionary[siteId] = new MachineContainer(siteId);
            }
            MachineContainer result = machineContainerDictionary[siteId];
            log.LogMethodExit(result);
            return result;
        }


        /// <summary>
        /// rebulds the container
        /// </summary>
        public static void Rebuild(int siteId) //added
        {
            log.LogMethodEntry();
            MachineContainer machineContainer = GetMachineContainer(siteId);
            machineContainerDictionary[siteId] = machineContainer.Refresh();
            log.LogMethodExit();
        }

        internal static List<MachineContainerDTO> GetMachineContainerDTOList(int siteId) 
        {

            log.LogMethodEntry(siteId);
            MachineContainer container = GetMachineContainer(siteId);
            List<MachineContainerDTO> machineContainerDTOList = container.GetMachineContainerDTOList();
            log.LogMethodExit(machineContainerDTOList);
            return machineContainerDTOList;


        }
    }
}
