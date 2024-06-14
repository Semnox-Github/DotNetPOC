/********************************************************************************************
 * Project Name - ViewContainer
 * Description  - Holds the Game container object
 *
 **************
 ** Version Log
  **************
  * Version     Date               Modified By             Remarks
 *********************************************************************************************
  2.110.0       18-Dec-2020         Prajwal S               Created : POS UI Redesign with REST API
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
    /// Holds the Game container object
    /// </summary>
    public class GameViewContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly ConcurrentDictionary<int, GameViewContainer> gameViewContainerDictionary = new ConcurrentDictionary<int, GameViewContainer>();
        private static Timer refreshTimer;
        static GameViewContainerList()
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
            List<int> uniqueKeyList = gameViewContainerDictionary.Keys.ToList();
            foreach (var uniqueKey in uniqueKeyList)
            {
                GameViewContainer gameViewContainer;
                if (gameViewContainerDictionary.TryGetValue(uniqueKey, out gameViewContainer))
                {
                    gameViewContainerDictionary[uniqueKey] = gameViewContainer.Refresh();
                }
            }
            log.LogMethodExit();
        }

        private static GameViewContainer GetGameViewContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            if (gameViewContainerDictionary.ContainsKey(siteId) == false)
            {
                gameViewContainerDictionary[siteId] = new GameViewContainer(siteId);
            }
            GameViewContainer result = gameViewContainerDictionary[siteId];
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the Game role container DTO list 
        /// </summary>
        /// <param name="executionContext">current application execution context</param>
        /// <returns></returns>
        public static List<GameContainerDTO> GetGameContainerDTOList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            GameViewContainer gameViewContainer = GetGameViewContainer(executionContext.SiteId);
            List<GameContainerDTO> result = gameViewContainer.GetGameContainerDTOList();
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the GameContainerDTOCollection for a given siteId and hash
        /// If the hash matches then no data is returned as the caller is holding the latest data
        /// </summary>
        /// <param name="siteId">site Id</param>
        /// <param name="hash">hash</param>
        /// /// <param name="rebuildCache">hash</param>
        /// <returns></returns>
        public static GameContainerDTOCollection GetGameContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(siteId);
            if (rebuildCache)
            {
                Rebuild(siteId);
            }
            GameViewContainer container = GetGameViewContainer(siteId);
            GameContainerDTOCollection gameContainerDTOCollection = container.GetGameDTOCollection(hash);
            return gameContainerDTOCollection;
        }

        public static void Rebuild(int siteId)
        {
            log.LogMethodEntry();
            GameViewContainer container = GetGameViewContainer(siteId);
            gameViewContainerDictionary[siteId] = container.Refresh();
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the GameContainerDTO based on the execution context
        /// </summary>
        /// <param name="executionContext">current application execution context</param>
        /// <returns></returns>
        public static GameContainerDTO GetGameContainerDTO(ExecutionContext executionContext)
        {
            return GetGameContainerDTO(executionContext.SiteId, executionContext.MachineId);
        }

        /// <summary>
        /// Returns the GameContainerDTO based on the siteId and GameId
        /// </summary>
        /// <param name="siteId">site Id</param>
        /// <param name="gameId">Game Id</param>
        /// <returns></returns>
        public static GameContainerDTO GetGameContainerDTO(int siteId, int gameId)
        {
            log.LogMethodEntry(siteId, gameId);
            GameViewContainer gameViewContainer = GetGameViewContainer(siteId);
            GameContainerDTO result = gameViewContainer.GetGameContainerDTO(gameId);
            log.LogMethodExit();
            return result;
        }
    }
}

