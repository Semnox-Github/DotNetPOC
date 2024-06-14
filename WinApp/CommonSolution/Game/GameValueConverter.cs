/********************************************************************************************
 * Project Name - Machine                                                                          
 * Description  - Bulk Upload Mapper GameDTO Class 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.60       16-Mar-2019   Muhammed Mehraj  Created   
 *2.70.2       12-Aug-2019   Deeksha          Added logger methods.
 ********************************************************************************************/
using Semnox.Core.GenericUtilities.Excel;
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;

namespace Semnox.Parafait.Game
{
    class GameValueConverter : ValueConverter
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        Dictionary<int, GameDTO> gameIdGameDTODictionary;
        Dictionary<string, GameDTO> gameNameGameDTODictionary;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public GameValueConverter(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            gameNameGameDTODictionary = new Dictionary<string, GameDTO>();
            gameIdGameDTODictionary = new Dictionary<int, GameDTO>();
            List<GameDTO> gameList = null;
            GameList gameDTOList = new GameList(executionContext);
            List<KeyValuePair<GameDTO.SearchByGameParameters, string>> searchParams = new List<KeyValuePair<GameDTO.SearchByGameParameters, string>>();
            searchParams.Add(new KeyValuePair<GameDTO.SearchByGameParameters, string>(GameDTO.SearchByGameParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            gameList = gameDTOList.GetGameList(searchParams, false);
            if (gameList != null && gameList.Count > 0)
            {
                foreach (GameDTO gameDTO in gameList)
                {
                    gameIdGameDTODictionary.Add(gameDTO.GameId, gameDTO);
                    gameNameGameDTODictionary.Add(gameDTO.GameName.ToUpper(), gameDTO);
                }
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Converts gamename to gameid
        /// </summary>
        /// <param name="stringValue"></param>
        /// <returns></returns>
        public override object FromString(string stringValue)
        {
            log.LogMethodEntry(stringValue);
            int gameId = -1;
            if (gameNameGameDTODictionary != null && gameNameGameDTODictionary.ContainsKey(stringValue.ToUpper()))
            {
                gameId = gameNameGameDTODictionary[stringValue.ToUpper()].GameId;
            }
            log.LogMethodExit(gameId);
            return gameId;
        }
        /// <summary>
        /// Converts gameid to gamename
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>

        public override string ToString(object value)
        {
            log.LogMethodEntry(value);
            string gameName = string.Empty;
            if (gameIdGameDTODictionary != null && gameIdGameDTODictionary.ContainsKey(Convert.ToInt32(value)))
            {
                gameName = gameIdGameDTODictionary[Convert.ToInt32(value)].GameName;
            }
            log.LogMethodExit(gameName);
            return gameName;
        }
    }
}
