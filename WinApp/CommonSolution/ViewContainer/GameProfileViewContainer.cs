/********************************************************************************************
 * Project Name - ViewContainer
 * Description  - GameProfileViewContainer class
 *
 **************
 ** Version Log
  **************
  * Version     Date Modified By Remarks
 *********************************************************************************************
 0.0         10-Nov-2020       Mushahid Faizan         Created : POS UI Redesign with REST API
 ********************************************************************************************/

using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using Semnox.Parafait.Game;

namespace Semnox.Parafait.ViewContainer
{
    /// <summary>
    /// Holds the list of GameProfiles
    /// </summary>
    public class GameProfileViewContainer : AbstractViewContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly GameProfileContainerDTOCollection gameProfileDTOCollection;
        private readonly ConcurrentDictionary<int, GameProfileContainerDTO> gameProfileContainerDTODictionary = new ConcurrentDictionary<int, GameProfileContainerDTO>();
        private readonly int siteId;
        //private DateTime lastRefreshTime;
        //private readonly object locker = new object();

        internal GameProfileViewContainer(int siteId, GameProfileContainerDTOCollection gameProfileDTOCollection)
        {
            log.LogMethodEntry(siteId, gameProfileDTOCollection);
            this.siteId = siteId;
            this.gameProfileDTOCollection = gameProfileDTOCollection;
            //lastRefreshTime = DateTime.Now;
            if (gameProfileDTOCollection != null &&
               gameProfileDTOCollection.GameProfileContainerDTOList != null &&
               gameProfileDTOCollection.GameProfileContainerDTOList.Any())
            {
                foreach (var gameProfileContainerDTO in gameProfileDTOCollection.GameProfileContainerDTOList)
                {
                    gameProfileContainerDTODictionary[gameProfileContainerDTO.GameProfileId] = gameProfileContainerDTO;
                }
            }
            log.LogMethodExit();
        }

        internal GameProfileViewContainer(int siteId) :
            this(siteId, GetGameProfileContainerDTOCollection(siteId, null, false))
        {
            log.LogMethodEntry(siteId);
            log.LogMethodExit();
        }

        private static GameProfileContainerDTOCollection GetGameProfileContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {

            log.LogMethodEntry(siteId, hash, rebuildCache);
            GameProfileContainerDTOCollection result;
            try
            {
                ExecutionContext executionContext = GetSystemUserExecutionContext();
                IGameProfileUseCases gameProfileUseCases = GameUseCaseFactory.GetGameProfileUseCases(executionContext);
                using (NoSynchronizationContextScope.Enter())
                {
                    Task<GameProfileContainerDTOCollection> gameProfileViewDTOCollectionTask = gameProfileUseCases.GetGameProfileContainerDTOCollection(siteId, hash, rebuildCache);
                    gameProfileViewDTOCollectionTask.Wait();
                    result = gameProfileViewDTOCollectionTask.Result;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving gameProfileContainerDTOCollection.", ex);
                result = new GameProfileContainerDTOCollection();
            }

            return result;
        }

        /// <summary>
        /// returns the latest in gameProfileDTOCollection
        /// </summary>
        /// <returns></returns>
        internal GameProfileContainerDTOCollection GetGameProfileDTOCollection(string hash)
        {
            log.LogMethodEntry(hash);
            if (gameProfileDTOCollection.Hash == hash)
            {
                log.LogMethodExit(null, "No changes to the cache");
                return null;
            }
            log.LogMethodExit(gameProfileDTOCollection);
            return gameProfileDTOCollection;
        }

        /// <summary>
        /// returns the GameProfileContainerDTO for the GameProfileId
        /// </summary>
        /// <param name="gameProfileId"></param>
        /// <returns></returns>
        public GameProfileContainerDTO GetGameProfileContainerDTO(int gameProfileId)
        {
            log.LogMethodEntry(gameProfileId);
            if (gameProfileContainerDTODictionary.ContainsKey(gameProfileId) == false)
            {
                string errorMessage = "GameProfile with GameProfile Id :" + gameProfileId + " doesn't exists.";
                log.LogMethodExit("Throwing Exception - " + errorMessage);
                throw new Exception(errorMessage);
            }
            GameProfileContainerDTO result = gameProfileContainerDTODictionary[gameProfileId];
            log.LogMethodExit(result);
            return result;
        }


        internal GameProfileViewContainer Refresh()
        {
            log.LogMethodEntry();
            //if (LastRefreshTime.AddMinutes(MinimimViewContainerRefreshWaitPeriod.GetValueInMinutes()) > DateTime.Now)
            //{
            //    log.LogMethodExit(this, "Last Refresh Time is " + LastRefreshTime);
            //    return this;
            //}
            LastRefreshTime = DateTime.Now;
            GameProfileContainerDTOCollection latestgameProfileDTOCollection = GetGameProfileContainerDTOCollection(siteId, gameProfileDTOCollection.Hash, true);
            if (latestgameProfileDTOCollection == null ||
                latestgameProfileDTOCollection.GameProfileContainerDTOList == null ||
                latestgameProfileDTOCollection.GameProfileContainerDTOList.Any() == false)
            {
                log.LogMethodExit(this, "No changes to the cache");
                return this;
            }
            GameProfileViewContainer result = new GameProfileViewContainer(siteId, latestgameProfileDTOCollection);
            log.LogMethodExit(result);
            return result;
        }

      
    }
}
