/********************************************************************************************
 * Project Name - Games
 * Description  - GameMasterList class to get the List of games from the container API
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         24-Aug-2020       Girish Kundar             Created : POS UI Redesign with REST API
 2.110.0     14-Dec-2020       Prajwal S                 Updated for new container API changes.
 2.150.2     6-Apr-2023        Yashodhara C H            Updated the container to latest code structure.
 ********************************************************************************************/

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Semnox.Core.Utilities;
namespace Semnox.Parafait.Game
{
    public static class GameContainerList
    {
        private static readonly object locker = new object();
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly ConcurrentDictionary<int, GameContainer> gameContainerDictionary = new ConcurrentDictionary<int, GameContainer>();
        private static Timer refreshTimer;

        static GameContainerList()
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
            List<int> uniqueKeyList = gameContainerDictionary.Keys.ToList();
            foreach (var uniqueKey in uniqueKeyList)
            {
                GameContainer gameContainer;
                if (gameContainerDictionary.TryGetValue(uniqueKey, out gameContainer))
                {
                    gameContainerDictionary[uniqueKey] = gameContainer.Refresh();
                }
            }
            log.LogMethodExit();
        }


        private static GameContainer GetGameContainer(int siteId, ExecutionContext executionContext = null) //added
        {
            log.LogMethodEntry(siteId);
            if (gameContainerDictionary.ContainsKey(siteId) == false)
            {
                gameContainerDictionary[siteId] = new GameContainer(siteId);
            }
            GameContainer result = gameContainerDictionary[siteId];
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Gets the GameContainerDTO Collection
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public static GameContainerDTOCollection GetGameContainerDTOCollection(int siteId)
        {
            log.LogMethodEntry(siteId);
            GameContainer container = GetGameContainer(siteId);
            GameContainerDTOCollection result = container.GetGameContainerDTOCollection();
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the GameContainerDTO based on the site and categoryId
        /// </summary>
        /// <param name="siteId">site Id</param>
        /// <param name="gameId">option value gameId</param>
        /// <returns></returns>
        public static GameContainerDTO GetGameContainerDTO(int siteId, int gameId)
        {
            log.LogMethodEntry(siteId, gameId);
            GameContainer categoryContainer = GetGameContainer(siteId);
            GameContainerDTO result = categoryContainer.GetGameContainerDTO(gameId);
            log.LogMethodExit();
            return result;
        }

        /// <summary>
        /// rebulds the container
        /// </summary>
        public static void Rebuild(int siteId) //added
        {
            log.LogMethodEntry();
            GameContainer gameContainer = GetGameContainer(siteId);
            gameContainerDictionary[siteId] = gameContainer.Refresh();
            log.LogMethodExit();
        }

        public static List<GameContainerDTO> GetGameContainerDTOList(int siteId, ExecutionContext executionContext)
        {
            log.LogMethodEntry(siteId);
            GameContainer container = GetGameContainer(siteId, executionContext);
            List<GameContainerDTO> gameContainerDTOList = container.GetGameContainerDTOList();
            log.LogMethodExit(gameContainerDTOList);
            return gameContainerDTOList;
        }

        /// <summary>
        /// Gets the GameContainerDTO based on the site and executionContext else returns null
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="gameId"></param>
        /// <returns></returns>
        public static GameContainerDTO GetGameContainerDTOOrDefault(ExecutionContext executionContext, int gameId)
        {
            log.LogMethodEntry(executionContext, gameId);
            log.LogMethodExit();
            return GetGameContainerDTOOrDefault(executionContext.SiteId, gameId);
        }

        /// <summary>
        /// /// Gets the GameContainerDTO based on the site and gameId else returns null
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="gameId"></param>
        /// <returns></returns>
        public static GameContainerDTO GetGameContainerDTOOrDefault(int siteId, int gameId)
        {
            log.LogMethodEntry(siteId, gameId);
            GameContainer container = GetGameContainer(siteId);
            var result = container.GetGameContainerDTOOrDefault(gameId);
            log.LogMethodExit(result);
            return result;
        }
    }
}
