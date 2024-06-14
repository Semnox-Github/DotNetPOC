/********************************************************************************************
 * Project Name - ViewContainer
 * Description  - Holds the Machine container object
 *
 **************
 ** Version Log
  **************
  * Version     Date               Modified By             Remarks
 *********************************************************************************************
  2.110.0       21-Dec-2020         Prajwal S               Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Semnox.Core.Utilities;
using Semnox.Parafait.Game;

namespace Semnox.Parafait.ViewContainer
{
    /// <summary>
    /// Holds the Machine container object
    /// </summary>
    public class MachineViewContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly ConcurrentDictionary<int, MachineViewContainer> machineViewContainerDictionary = new ConcurrentDictionary<int, MachineViewContainer>();
        private static Timer refreshTimer;
        static MachineViewContainerList()
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
            List<int> uniqueKeyList = machineViewContainerDictionary.Keys.ToList();
            foreach (var uniqueKey in uniqueKeyList)
            {
                MachineViewContainer machineViewContainer;
                if (machineViewContainerDictionary.TryGetValue(uniqueKey, out machineViewContainer))
                {
                    machineViewContainerDictionary[uniqueKey] = machineViewContainer.Refresh();
                }
            }
            log.LogMethodExit();
        }

        private static MachineViewContainer GetMachineViewContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            if (machineViewContainerDictionary.ContainsKey(siteId) == false)
            {
                machineViewContainerDictionary[siteId] = new MachineViewContainer(siteId);
            }
            MachineViewContainer result = machineViewContainerDictionary[siteId];
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the MachineContainerDTOCollection for a given siteId and hash
        /// If the hash matches then no data is returned as the caller is holding the latest data
        /// </summary>
        /// <param name="siteId">site Id</param>
        /// <param name="hash">hash</param>
        /// /// <param name="rebuildCache">hash</param>
        /// <returns></returns>
        public static MachineContainerDTOCollection GetMachineContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(siteId);
            if (rebuildCache)
            {
                Rebuild(siteId);
            }
            MachineViewContainer container = GetMachineViewContainer(siteId);
            MachineContainerDTOCollection machineContainerDTOCollection = container.GetMachineDTOCollection(hash);
            return machineContainerDTOCollection;
        }

        public static void Rebuild(int siteId)
        {
            log.LogMethodEntry();
            MachineViewContainer container = GetMachineViewContainer(siteId);
            machineViewContainerDictionary[siteId] = container.Refresh();
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the MachineContainerDTO based on the execution context
        /// </summary>
        /// <param name="executionContext">current application execution context</param>
        /// <returns></returns>
        public static MachineContainerDTO GetMachineContainerDTO(ExecutionContext executionContext)
        {
            return GetMachineContainerDTO(executionContext.SiteId, executionContext.MachineId);
        }

        /// <summary>
        /// Returns the MachineContainerDTO based on the siteId and MachineId
        /// </summary>
        /// <param name="siteId">site Id</param>
        /// <param name="MachineId">Machine Id</param>
        /// <returns></returns>
        public static MachineContainerDTO GetMachineContainerDTO(int siteId, int MachineId)
        {
            log.LogMethodEntry(siteId, MachineId);
            MachineViewContainer machineViewContainer = GetMachineViewContainer(siteId);
            MachineContainerDTO result = machineViewContainer.GetMachineContainerDTO(MachineId);
            log.LogMethodExit();
            return result;
        }
}
}


