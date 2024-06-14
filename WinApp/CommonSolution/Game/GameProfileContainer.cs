/********************************************************************************************
 * Project Name - GameProfileContainer  Class
 * Description  - GameProfileContainer class to get the data    
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         24-Aug-2020       Girish Kundar             Created : POS UI Redesign with REST API
 2.110.0     14-Dec-2020       Prajwal S                 Updated for Changes in Container API.
 2.150.2     6-Apr-2023        Yashodhara C H            Updated the container to latest code structure.
 ********************************************************************************************/


using System;
using System.Collections.Generic;
using System.Globalization;
namespace Semnox.Parafait.Game
{
    class GameProfileContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly int siteId;
        private readonly DateTime? gameProfileModuleLastUpdateTime;
        private readonly Dictionary<int, GameProfileDTO> gameProfileIdGameProfileDTODictionary = new Dictionary<int, GameProfileDTO>();
        private readonly Dictionary<int, GameProfileContainerDTO> gameProfileIdGameProfileContainerDTODictionary = new Dictionary<int, GameProfileContainerDTO>();
        private readonly List<GameProfileDTO> gameProfileDTOList;
        private readonly GameProfileContainerDTOCollection gameProfileContainerDTOCollection;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="siteId"></param>
        public GameProfileContainer(int siteId)
            : this(siteId, GetGameProfileDTOList(siteId), GetGameProfileModuleLastUpdateTime(siteId))
        {
            log.LogMethodEntry(siteId);

            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor 
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="gameProfileDTOList"></param>
        /// <param name="gameProfileModuleLastUpdateTime"></param>
        public GameProfileContainer(int siteId, List<GameProfileDTO> gameProfileDTOList, DateTime? gameProfileModuleLastUpdateTime)
        {
            log.LogMethodEntry(siteId, gameProfileDTOList, gameProfileModuleLastUpdateTime);
            this.siteId = siteId;
            this.gameProfileDTOList = gameProfileDTOList;
            this.gameProfileModuleLastUpdateTime = gameProfileModuleLastUpdateTime;
            foreach(var gameProfileDTO in gameProfileDTOList)
            {
                if (gameProfileIdGameProfileDTODictionary.ContainsKey(gameProfileDTO.GameProfileId))
                {
                    continue;
                }
                gameProfileIdGameProfileDTODictionary.Add(gameProfileDTO.GameProfileId, gameProfileDTO);
            }
            List<GameProfileContainerDTO> gameProfileContainerDTOList = new List<GameProfileContainerDTO>();
            foreach (GameProfileDTO gameProfileDTO in gameProfileDTOList)
            {
                if (gameProfileIdGameProfileContainerDTODictionary.ContainsKey(gameProfileDTO.GameProfileId))
                {
                    continue;
                }
                GameProfileContainerDTO gameProfileContainerDTO = new GameProfileContainerDTO(gameProfileDTO.GameProfileId, gameProfileDTO.ProfileName, gameProfileDTO.CreditAllowed,
                                                                                              gameProfileDTO.BonusAllowed, gameProfileDTO.CourtesyAllowed, gameProfileDTO.TimeAllowed,
                                                                                              gameProfileDTO.TicketAllowedOnCredit, gameProfileDTO.TicketAllowedOnCourtesy, gameProfileDTO.TicketAllowedOnBonus,
                                                                                              gameProfileDTO.TicketAllowedOnTime, gameProfileDTO.PlayCredits, gameProfileDTO.VipPlayCredits, gameProfileDTO.InternetKey,
                                                                                              gameProfileDTO.RedemptionToken, gameProfileDTO.PhysicalToken, gameProfileDTO.TokenPrice, gameProfileDTO.RedeemTokenTo,
                                                                                              gameProfileDTO.ThemeNumber, gameProfileDTO.ThemeId, gameProfileDTO.ShowAd, gameProfileDTO.IsTicketEater, gameProfileDTO.UserIdentifier,
                                                                                              gameProfileDTO.CustomDataSetId, gameProfileDTO.ProfileIdentifier, gameProfileDTO.ForceRedeemToCard);
                gameProfileContainerDTOList.Add(gameProfileContainerDTO);
                gameProfileIdGameProfileContainerDTODictionary.Add(gameProfileContainerDTO.GameProfileId, gameProfileContainerDTO);
                if(gameProfileDTO.GamePriceTierDTOList != null)
                {
                    foreach (var gamePriceTierDTO in gameProfileDTO.GamePriceTierDTOList)
                    {
                        GamePriceTierContainerDTO gamePriceTierContainerDTO = new GamePriceTierContainerDTO(gamePriceTierDTO.GamePriceTierId, gamePriceTierDTO.GameId, gamePriceTierDTO.GameProfileId, gamePriceTierDTO.Name, gamePriceTierDTO.Description, gamePriceTierDTO.PlayCount, gamePriceTierDTO.PlayCredits, gamePriceTierDTO.VipPlayCredits, gamePriceTierDTO.SortOrder);
                        gameProfileContainerDTO.GamePriceTierContainerDTOList.Add(gamePriceTierContainerDTO);
                    }
                }
            }
            gameProfileContainerDTOCollection = new GameProfileContainerDTOCollection(gameProfileContainerDTOList);
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the list of GameProfileContainerDTO
        /// </summary>
        /// <returns></returns>
        internal List<GameProfileContainerDTO> GetGameProfileContainerDTOList()
        {
            log.LogMethodEntry();
            log.LogMethodExit(gameProfileContainerDTOCollection.GameProfileContainerDTOList);
            return gameProfileContainerDTOCollection.GameProfileContainerDTOList;
        }

        /// <summary>
        /// Gets the GameProfieDTO Details
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        private static List<GameProfileDTO> GetGameProfileDTOList(int siteId)
        {
            log.LogMethodEntry(siteId);
            List<GameProfileDTO> gameProfileDTOList = null;
            try
            {
                GameProfileList gameProfileList = new GameProfileList();
        List<KeyValuePair<GameProfileDTO.SearchByGameProfileParameters, string>> searchParameters = new List<KeyValuePair<GameProfileDTO.SearchByGameProfileParameters, string>>();
        searchParameters.Add(new KeyValuePair<GameProfileDTO.SearchByGameProfileParameters, string>(GameProfileDTO.SearchByGameProfileParameters.IS_ACTIVE, "1"));
                searchParameters.Add(new KeyValuePair<GameProfileDTO.SearchByGameProfileParameters, string>(GameProfileDTO.SearchByGameProfileParameters.SITE_ID, siteId.ToString()));
                gameProfileDTOList = gameProfileList.GetGameProfileDTOList(searchParameters, siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving the GameProfile.", ex);
            }

            if (gameProfileDTOList == null)
            {
                gameProfileDTOList = new List<GameProfileDTO>();
            }
            log.LogMethodExit();
            return gameProfileDTOList;
        }

        /// <summary>
        /// Gets the GameProfileContainerDTCollection
        /// </summary>
        /// <returns></returns>
        public GameProfileContainerDTOCollection GetGameProfileContainerDTOCollection()
        {
            log.LogMethodEntry();
            log.LogMethodExit(gameProfileContainerDTOCollection);
            return gameProfileContainerDTOCollection;
        }

        /// <summary>
        /// Gets the Last upadted date of the record.
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public static DateTime? GetGameProfileModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            DateTime? result = null;
            try
            {
                GameProfileList gameProfileList = new GameProfileList();
                result = gameProfileList.GetGameProfileModuleLastUpdateTime(siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving the game profile max last update date.", ex);
                result = null;
            }
            log.LogMethodExit();
            return result;
        }

        public GameProfileContainerDTO GetGameProfileContainerDTOOrDefault(int gameProfileId)
        {
            log.LogMethodEntry(gameProfileId);
            if (gameProfileIdGameProfileContainerDTODictionary.ContainsKey(gameProfileId) == false)
            {
                string message = "Products with gameProfileId : " + gameProfileId + " doesn't exist.";
                log.LogMethodExit(null, message);
                return null;
            }
            var result = gameProfileIdGameProfileContainerDTODictionary[gameProfileId];
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Gets the GameProfileContainerDTO details based on gameProfileId
        /// </summary>
        /// <param name="gameProfileId"></param>
        /// <returns></returns>
        public GameProfileContainerDTO GetGameProfileContainerDTO(int gameProfileId)
        {
            log.LogMethodEntry(gameProfileId);
            if (gameProfileIdGameProfileContainerDTODictionary.ContainsKey(gameProfileId))
            {
                string errorMessage = "GameProfile with gameProfile Id :" + gameProfileId + " doesn't exists.";
                log.LogMethodExit("Throwing Exception - " + errorMessage);
                throw new Exception(errorMessage);
            }
            GameProfileContainerDTO gameProfileContainerDTO = gameProfileIdGameProfileContainerDTODictionary[gameProfileId];
            log.LogMethodExit(gameProfileContainerDTO);
            return gameProfileContainerDTO;
        }

        /// <summary>
        /// Rebulds the container
        /// </summary>
        public GameProfileContainer Refresh() //added
        {
            log.LogMethodEntry();
            DateTime? updateTime = GetGameProfileModuleLastUpdateTime(siteId);
            if (gameProfileModuleLastUpdateTime.HasValue
                && gameProfileModuleLastUpdateTime >= updateTime)
            {
                log.LogMethodExit(this, "No changes in Game since " + updateTime.Value.ToString(CultureInfo.InvariantCulture));
                return this;
            }
            GameProfileContainer result = new GameProfileContainer(siteId);
            log.LogMethodExit(result);
            return result;
        }
    }
}
