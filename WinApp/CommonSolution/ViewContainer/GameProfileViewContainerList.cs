/********************************************************************************************
 * Project Name - ViewContainer
 * Description  - Holds the GameProfile container object
 *
 **************
 ** Version Log
  **************
  * Version     Date               Modified By             Remarks
 *********************************************************************************************
  2.110.0       16-Dec-2020         Prajwal S               Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Semnox.Core.Utilities;
using Semnox.Parafait.Game;

namespace Semnox.Parafait.ViewContainer
{
    /// <summary>
    /// Holds the GameProfile container object
    /// </summary>
    public class GameProfileViewContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly ConcurrentDictionary<int, GameProfileViewContainer> gameProfileViewContainerDictionary = new ConcurrentDictionary<int, GameProfileViewContainer>();
        private static Timer refreshTimer;
        static GameProfileViewContainerList()
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
            List<int> uniqueKeyList = gameProfileViewContainerDictionary.Keys.ToList();
            foreach (var uniqueKey in uniqueKeyList)
            {
                GameProfileViewContainer GameProfileViewContainer;
                if (gameProfileViewContainerDictionary.TryGetValue(uniqueKey, out GameProfileViewContainer))
                {
                    gameProfileViewContainerDictionary[uniqueKey] = GameProfileViewContainer.Refresh();
                }
            }
            log.LogMethodExit();
        }

        private static GameProfileViewContainer GetGameProfileViewContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            if (gameProfileViewContainerDictionary.ContainsKey(siteId) == false)
            {
                gameProfileViewContainerDictionary[siteId] = new GameProfileViewContainer(siteId);
            }
            GameProfileViewContainer result = gameProfileViewContainerDictionary[siteId];
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the GameProfileContainerDTOCollection for a given siteId and hash
        /// If the hash matches then no data is returned as the caller is holding the latest data
        /// </summary>
        /// <param name="siteId">site Id</param>
        /// <param name="hash">hash</param>
        /// /// <param name="rebuildCache">hash</param>
        /// <returns></returns>
        public static GameProfileContainerDTOCollection GetGameProfileContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(siteId);
            if (rebuildCache)
            {
                Rebuild(siteId);
            }
            GameProfileViewContainer container = GetGameProfileViewContainer(siteId);
            GameProfileContainerDTOCollection GameProfileContainerDTOCollection = container.GetGameProfileDTOCollection(hash);
            return GameProfileContainerDTOCollection;
        }

        public static void Rebuild(int siteId)
        {
            log.LogMethodEntry();
            GameProfileViewContainer container = GetGameProfileViewContainer(siteId);
            gameProfileViewContainerDictionary[siteId] = container.Refresh();
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the GameProfileContainerDTO based on the execution context
        /// </summary>
        /// <param name="executionContext">current application execution context</param>
        /// <returns></returns>
        public static GameProfileContainerDTO GetGameProfileContainerDTO(ExecutionContext executionContext)
        {
            return GetGameProfileContainerDTO(executionContext.SiteId, executionContext.MachineId);
        }

        /// <summary>
        /// Returns the GameProfileContainerDTO based on the siteId and GameProfileId
        /// </summary>
        /// <param name="siteId">site Id</param>
        /// <param name="gameProfileId">GameProfile Id</param>
        /// <returns></returns>
        public static GameProfileContainerDTO GetGameProfileContainerDTO(int siteId, int gameProfileId)
        {
            log.LogMethodEntry(siteId, gameProfileId);
            GameProfileViewContainer gameProfileViewContainer = GetGameProfileViewContainer(siteId);
            GameProfileContainerDTO result = gameProfileViewContainer.GetGameProfileContainerDTO(gameProfileId);
            log.LogMethodExit();
            return result;
        }
    }
}

