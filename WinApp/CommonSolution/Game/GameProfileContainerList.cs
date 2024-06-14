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
 2.110.0     15-Dec-2020       Prajwal S                 Updated for new container API changes.
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
    public static class GameProfileContainerList
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly ConcurrentDictionary<int, GameProfileContainer> gameProfileContainerDictionary = new ConcurrentDictionary<int, GameProfileContainer>();
        private static Timer refreshTimer;

        static GameProfileContainerList()
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
            List<int> uniqueKeyList = gameProfileContainerDictionary.Keys.ToList();
            foreach (var uniqueKey in uniqueKeyList)
            {
                GameProfileContainer GameProfileContainer;
                if (gameProfileContainerDictionary.TryGetValue(uniqueKey, out GameProfileContainer))
                {
                    gameProfileContainerDictionary[uniqueKey] = GameProfileContainer.Refresh();
                }
            }
            log.LogMethodExit();
        }


        private static GameProfileContainer GetGameProfileContainer(int siteId) //added
        {
            log.LogMethodEntry(siteId);
            if (gameProfileContainerDictionary.ContainsKey(siteId) == false)
            {
                gameProfileContainerDictionary[siteId] = new GameProfileContainer(siteId);
            }
            GameProfileContainer result = gameProfileContainerDictionary[siteId];
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Gets the GameProfileContainerDTO Collection
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public static GameProfileContainerDTOCollection GetGameProfileContainerDTOCollection(int siteId)
        {
            log.LogMethodEntry(siteId);
            GameProfileContainer container = GetGameProfileContainer(siteId);
            GameProfileContainerDTOCollection result = container.GetGameProfileContainerDTOCollection();
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// rebulds the container
        /// </summary>
        public static void Rebuild(int siteId) //added
        {
            log.LogMethodEntry();
            GameProfileContainer gameProfileContainer = GetGameProfileContainer(siteId);
            gameProfileContainerDictionary[siteId] = gameProfileContainer.Refresh();
            log.LogMethodExit();
        }


        /// <summary>
        /// Returns the GameProfileContainerDTO based on the site and categoryId
        /// </summary>
        /// <param name="siteId">site Id</param>
        /// <param name="gameProfileId">option value gameProfileId</param>
        /// <returns></returns>
        public static GameProfileContainerDTO GetGameProfileContainerDTO(int siteId, int gameProfileId)
        {
            log.LogMethodEntry(siteId, gameProfileId);
            GameProfileContainer categoryContainer = GetGameProfileContainer(siteId);
            GameProfileContainerDTO result = categoryContainer.GetGameProfileContainerDTO(gameProfileId);
            log.LogMethodExit();
            return result;
        }

        /// <summary>
        /// Returns the GameProfileContainerDTO based on the site and executionContext
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        internal static List<GameProfileContainerDTO> GetGameProfileContainerDTOList(int siteId, ExecutionContext executionContext)
        {
            log.LogMethodEntry(siteId);
            GameProfileContainer container = GetGameProfileContainer(siteId);
            List<GameProfileContainerDTO> gameContainerDTOList = container.GetGameProfileContainerDTOList();
            log.LogMethodExit(gameContainerDTOList);
            return gameContainerDTOList;
        }

        /// <summary>
        /// Gets the GameProfileContainerDTO based on the executionContext and GameProfileId else returns null
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="gameProfileId"></param>
        /// <returns></returns>
        public static GameProfileContainerDTO GetGameProfileContainerDTOOrDefault(ExecutionContext executionContext, int gameProfileId)
        {
            log.LogMethodEntry(executionContext, gameProfileId);
            log.LogMethodExit();
            return GetGameProfileContainerDTOOrDefault(executionContext.SiteId, gameProfileId);
        }

        /// <summary>
        /// /// Gets the GameProfileContainerDTO based on the site and gameProfileId else returns null
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="gameProfileId"></param>
        /// <returns></returns>
        public static GameProfileContainerDTO GetGameProfileContainerDTOOrDefault(int siteId, int gameProfileId)
        {
            log.LogMethodEntry(siteId, gameProfileId);
            GameProfileContainer container = GetGameProfileContainer(siteId);
            var result = container.GetGameProfileContainerDTOOrDefault(gameProfileId);
            log.LogMethodExit(result);
            return result;
        }
    }
}
