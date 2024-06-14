/********************************************************************************************
 * Project Name - ViewContainer
 * Description  - Holds the Hub container object
 *
 **************
 ** Version Log
  **************
  * Version     Date               Modified By             Remarks
 *********************************************************************************************
  2.110.0       16-Dec-2020         Prajwal S               Created : POS UI Redesign with REST API
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
        /// Holds the Hub container object
        /// </summary>
        public class HubViewContainerList
        {
            private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            private static readonly ConcurrentDictionary<int, HubViewContainer> hubViewContainerDictionary = new ConcurrentDictionary<int, HubViewContainer>();
            private static Timer refreshTimer;
            static HubViewContainerList()
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
                List<int> uniqueKeyList = hubViewContainerDictionary.Keys.ToList();
                foreach (var uniqueKey in uniqueKeyList)
                {
                    HubViewContainer hubViewContainer;
                    if (hubViewContainerDictionary.TryGetValue(uniqueKey, out hubViewContainer))
                    {
                        hubViewContainerDictionary[uniqueKey] = hubViewContainer.Refresh();
                    }
                }
                log.LogMethodExit();
            }

            private static HubViewContainer GetHubViewContainer(int siteId)
            {
                log.LogMethodEntry(siteId);
                if (hubViewContainerDictionary.ContainsKey(siteId) == false)
                {
                    hubViewContainerDictionary[siteId] = new HubViewContainer(siteId);
                }
                HubViewContainer result = hubViewContainerDictionary[siteId];
                log.LogMethodExit(result);
                return result;
            }

            /// <summary>
            /// Returns the HubContainerDTOCollection for a given siteId and hash
            /// If the hash matches then no data is returned as the caller is holding the latest data
            /// </summary>
            /// <param name="siteId">site Id</param>
            /// <param name="hash">hash</param>
            /// /// <param name="rebuildCache">hash</param>
            /// <returns></returns>
            public static HubContainerDTOCollection GetHubContainerDTOCollection(int siteId, string hash, bool rebuildCache)
            {
                log.LogMethodEntry(siteId);
                if (rebuildCache)
                {
                    Rebuild(siteId);
                }
                HubViewContainer container = GetHubViewContainer(siteId);
                HubContainerDTOCollection hubContainerDTOCollection = container.GetHubDTOCollection(hash);
                return hubContainerDTOCollection;
            }

            public static void Rebuild(int siteId)
            {
                log.LogMethodEntry();
                HubViewContainer container = GetHubViewContainer(siteId);
                hubViewContainerDictionary[siteId] = container.Refresh();
                log.LogMethodExit();
            }

            /// <summary>
            /// Returns the HubContainerDTO based on the execution context
            /// </summary>
            /// <param name="executionContext">current application execution context</param>
            /// <returns></returns>
            public static HubContainerDTO GetHubContainerDTO(ExecutionContext executionContext)
            {
                return GetHubContainerDTO(executionContext.SiteId, executionContext.MachineId);
            }

            /// <summary>
            /// Returns the HubContainerDTO based on the siteId and HubId
            /// </summary>
            /// <param name="siteId">site Id</param>
            /// <param name="HubId">Hub Id</param>
            /// <returns></returns>
            public static HubContainerDTO GetHubContainerDTO(int siteId, int HubId)
            {
                log.LogMethodEntry(siteId, HubId);
                HubViewContainer hubViewContainer = GetHubViewContainer(siteId);
                HubContainerDTO result = hubViewContainer.GetHubContainerDTO(HubId);
                log.LogMethodExit();
                return result;
            }
        }
    }
