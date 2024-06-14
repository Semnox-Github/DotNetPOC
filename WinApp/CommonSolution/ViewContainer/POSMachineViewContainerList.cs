/********************************************************************************************
 * Project Name - POS
 * Description  - POSMachineViewContainerList class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         24-Aug-2020       Girish Kundar             Created : POS UI Redesign with REST API
 ********************************************************************************************/

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Semnox.Core.Utilities;
using Semnox.Parafait.POS;

namespace Semnox.Parafait.ViewContainer
{
    /// <summary>
    /// POSMachineViewContainerList holds multiple  POSMachineView containers based on siteId, userId and POSMachineId
    /// </summary>
    public static class POSMachineViewContainerList
    {
        private static readonly logging.Logger log = new logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Cache<int, POSMachineViewContainer> posMachineViewContainerCache = new Cache<int, POSMachineViewContainer>();
        private static Timer refreshTimer;

        static POSMachineViewContainerList()
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
            var uniqueKeyList = posMachineViewContainerCache.Keys;
            foreach (var uniqueKey in uniqueKeyList)
            {
                POSMachineViewContainer posMachineViewContainer;
                if (posMachineViewContainerCache.TryGetValue(uniqueKey, out posMachineViewContainer))
                {
                    posMachineViewContainerCache[uniqueKey] = posMachineViewContainer.Refresh(false);
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the POSMachineContainerDTO for given execution context
        /// </summary>
        /// <param name="executionContext">Current Application Execution Context</param>
        /// <returns></returns>
        public static POSMachineContainerDTO GetPOSMachineContainerDTO(ExecutionContext executionContext)
        {
            return GetPOSMachineContainerDTO(executionContext.SiteId, executionContext.MachineId);
        }
        
        /// <summary>
        /// Returns the POSMachineContainerDTO for given siteId and machineId
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="machineId"></param>
        /// <returns></returns>
        public static POSMachineContainerDTO GetPOSMachineContainerDTO(int siteId, int machineId)
        {
            log.LogMethodEntry(siteId, machineId);
            POSMachineViewContainer posMachineViewContainer = GetPOSMachineViewContainer(siteId);
            POSMachineContainerDTO result = posMachineViewContainer.GetPOSMachineContainerDTO(machineId);
            log.LogMethodExit(result);
            return result;
        }

        private static POSMachineViewContainer GetPOSMachineViewContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            POSMachineViewContainer result = posMachineViewContainerCache.GetOrAdd(siteId, (k)=> new POSMachineViewContainer(siteId));
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the POSMachineContainerDTOList for a given context
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <returns></returns>
        public static List<POSMachineContainerDTO> GetPOSMachineContainerDTOList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            POSMachineViewContainer posmachineViewContainer = GetPOSMachineViewContainer(executionContext.SiteId);
            List<POSMachineContainerDTO> result = posmachineViewContainer.GetPOSMachineContainerDTOList();
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// returns all the transaction DefaultViewDTOCollection defined in the system
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="hash"></param>
        /// <returns></returns>
        public static POSMachineContainerDTOCollection GetPOSMachineContainerDTOCollection(int siteId, string hash)
        {
            log.LogMethodEntry();
            //if (rebuildCache)
            //{
            //    Rebuild(siteId);
            //}
            POSMachineViewContainer posMachineViewContainer = GetPOSMachineViewContainer(siteId);
            POSMachineContainerDTOCollection result = posMachineViewContainer.GetPOSMachineContainerDTOCollection(hash);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// rebulds the container
        /// </summary>
        public static void Rebuild(int siteId)
        {
            log.LogMethodEntry();
            POSMachineViewContainer result = GetPOSMachineViewContainer(siteId);
            posMachineViewContainerCache[siteId] = result.Refresh(true);
            log.LogMethodExit();
        }
    }
}
