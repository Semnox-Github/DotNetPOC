/********************************************************************************************
 * Project Name - Games
 * Description  - HubMasterList class to get the List of games from the container API
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0     14-Dec-2020       Prajwal S                Created : POS UI Redesign with REST API
 ********************************************************************************************/

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Semnox.Core.Utilities;
namespace Semnox.Parafait.Game
{
    public static class HubContainerList
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly ConcurrentDictionary<int, HubContainer> hubContainerDictionary = new ConcurrentDictionary<int, HubContainer>();
        private static Timer refreshTimer;

        static HubContainerList() //added
        {
            log.LogMethodEntry();
            refreshTimer = new Timer(DataRefreshFrequency.GetValue());
            refreshTimer.Elapsed += OnRefreshTimer;
            refreshTimer.Start();
            log.LogMethodExit();
        }

        private static void OnRefreshTimer(object sender, ElapsedEventArgs e) //added
        {
            log.LogMethodEntry();
            List<int> uniqueKeyList = hubContainerDictionary.Keys.ToList();
            foreach (var uniqueKey in uniqueKeyList)
            {
                HubContainer HubContainer;
                if (hubContainerDictionary.TryGetValue(uniqueKey, out HubContainer))
                {
                    hubContainerDictionary[uniqueKey] = HubContainer.Refresh();
                }
            }
            log.LogMethodExit();
        }
        internal static List<HubContainerDTO> GetHubContainerDTOList(int siteId) //added
        {

            log.LogMethodEntry(siteId);
            HubContainer container = GetHubContainer(siteId);
            List<HubContainerDTO> hubContainerDTOList = container.GetHubContainerDTOList();
            log.LogMethodExit(hubContainerDTOList);
            return hubContainerDTOList;


        }

        private static HubContainer GetHubContainer(int siteId)  //added
        {
            log.LogMethodEntry(siteId);
            if (hubContainerDictionary.ContainsKey(siteId) == false)
            {
                hubContainerDictionary[siteId] = new HubContainer(siteId);
            }
            HubContainer result = hubContainerDictionary[siteId];
            log.LogMethodExit(result);
            return result;
        }
        

        public static void Rebuild(int siteId) //added
        {
            log.LogMethodEntry();
            HubContainer hubContainer = GetHubContainer(siteId);
            hubContainerDictionary[siteId] = hubContainer.Refresh();
            log.LogMethodExit();
        }

    }
}
