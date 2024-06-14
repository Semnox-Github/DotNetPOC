/********************************************************************************************
 * Project Name - GamePlayBL                                                                     
 * Description  - BL for GamePlay 
 *
 **************
 **Version Log
 *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.70.2         29-Jul-2019   Deeksha              Modified: Save method returns DTO.
 *2.110.0        29-Oct-2019   Girish Kundar        Modified: Center edge changes 
*2.110.0        14-Dec-2020   Prajwal S            Modified: Save, Constructor with Id parameter.
 *                                                  Added: Save for List, Build for Child class.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Game
{
    /// <summary>
    /// Business logic for GamePlay class.
    /// </summary>
    public class GamePlayBL
    {
        private GamePlayDTO gamePlayDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of GamePlayBL class
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public GamePlayBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            gamePlayDTO = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the GamePlayBL id as the parameter
        /// Would fetch the GamePlayBL object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="id">Id</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        public GamePlayBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            GamePlayDataHandler gamePlayDataHandler = new GamePlayDataHandler(sqlTransaction);
            gamePlayDTO = gamePlayDataHandler.GetGamePlayDTO(id, sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the GamePlay Id  as the parameter
        /// Would fetch the GamePlayDTO object from the database based on the GamePlay id passed. 
        /// </summary>
        /// <param name="GamePlayId">GamePlay id </param>
        public GamePlayBL(ExecutionContext executionContext, int gamePlayId, bool activeChildRecords, bool loadChildRecords = false, SqlTransaction sqlTransaction = null) //modified.
            : this(executionContext)
        {
            log.LogMethodEntry(gamePlayId, sqlTransaction);
            GamePlayDataHandler gamePlayDataHandler = new GamePlayDataHandler(sqlTransaction);
            gamePlayDTO = gamePlayDataHandler.GetGamePlayDTO(gamePlayId, sqlTransaction);
            if (gamePlayDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "GamePlay", gamePlayId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            if (loadChildRecords)
            {
                Build(activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit(gamePlayDTO);
        }

        /// <summary>
        /// Generate PriceListProductsDTO list
        /// </summary>
        /// <param name="activeChildRecords">Bool for active only records</param>
        /// <param name="sqlTransaction">sql transaction</param>
        private void Build(bool activeChildRecords = true, SqlTransaction sqlTransaction = null) //added Build to get childRecords.
        {
            log.LogMethodEntry(activeChildRecords, sqlTransaction);

            GamePlayInfoList gamePlayInfoList = new GamePlayInfoList(executionContext);

            List<KeyValuePair<GamePlayInfoDTO.SearchByParameters, string>> searchContentMapParameters = new List<KeyValuePair<GamePlayInfoDTO.SearchByParameters, string>>();
            searchContentMapParameters.Add(new KeyValuePair<GamePlayInfoDTO.SearchByParameters, string>(GamePlayInfoDTO.SearchByParameters.SITE_ID, Convert.ToString(executionContext.GetSiteId())));
            searchContentMapParameters.Add(new KeyValuePair<GamePlayInfoDTO.SearchByParameters, string>(GamePlayInfoDTO.SearchByParameters.GAME_PLAY_ID, Convert.ToString(gamePlayDTO.GameplayId)));
            gamePlayDTO.GamePlayInfoDTOList = gamePlayInfoList.GetGamePlayInfoDTOList(searchContentMapParameters, sqlTransaction);
            gamePlayDTO.AcceptChanges();
            log.LogMethodExit();
        }


        /// <summary>
        /// Creates GamePlayBL object using the GamePlayDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="gamePlayDTO">GamePlayDTO object</param>
        public GamePlayBL(ExecutionContext executionContext, GamePlayDTO gamePlayDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, gamePlayDTO);
            this.gamePlayDTO = gamePlayDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the GamePlay
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            GamePlayDataHandler gamePlayDataHandler = new GamePlayDataHandler(sqlTransaction);
            if (gamePlayDTO.GameplayId < 0)
            {
                gamePlayDTO = gamePlayDataHandler.InsertGamePlayDTO(gamePlayDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                gamePlayDTO.AcceptChanges();
            }
            else
            {
                if (gamePlayDTO.IsChanged)
                {
                    gamePlayDTO = gamePlayDataHandler.UpdateGamePlayDTO(gamePlayDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    gamePlayDTO.AcceptChanges();
                }
            }
            if (gamePlayDTO.GamePlayInfoDTOList != null && gamePlayDTO.GamePlayInfoDTOList.Count > 0)
            {
                foreach (GamePlayInfoDTO gamePlayInfoDTO in gamePlayDTO.GamePlayInfoDTOList)
                {
                    if (gamePlayInfoDTO != null)
                    {
                        if (gamePlayInfoDTO.GameplayId != gamePlayDTO.GameplayId)
                        {
                            gamePlayInfoDTO.GameplayId = gamePlayDTO.GameplayId;
                        }
                        GamePlayInfoBL gamePlayInfoBLObj = new GamePlayInfoBL(executionContext, gamePlayInfoDTO);
                        gamePlayInfoBLObj.Save(sqlTransaction);
                    }
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public GamePlayDTO GamePlayDTO
        {
            get
            {
                return gamePlayDTO;
            }
        }

    }

    /// <summary>
    /// Manages the list of GamePlay
    /// </summary>
    public class GamePlayListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<GamePlayDTO> gamePlayDTOList;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public GamePlayListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with ExecutionContext and DTO parameter
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="GamePlayDTOList">GamePlayDTOList</param>
        public GamePlayListBL(ExecutionContext executionContext, List<GamePlayDTO> gamePlayDTOList) //added.
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, gamePlayDTOList);
            this.gamePlayDTOList = gamePlayDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the GamePlay list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>returnValue</returns>
        public List<GamePlayDTO> GetGamePlayDTOList(List<KeyValuePair<GamePlayDTO.SearchByParameters, string>> searchParameters, bool loadChild = false, int currentPage = 0, int pageSize = 0, SqlTransaction sqlTransaction = null) //modified
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<GamePlayDTO> gamePlayDTOList;
            List<GamePlayInfoDTO> gamePlayInfoDTOList;
            GamePlayDataHandler gamePlayDataHandler = new GamePlayDataHandler(sqlTransaction);

            gamePlayDTOList = gamePlayDataHandler.GetGamePlays(searchParameters, currentPage, pageSize);
            if (gamePlayDTOList != null && gamePlayDTOList.Count > 0 && loadChild)
            {
                GamePlayInfoList gamePlayInfoList = new GamePlayInfoList(executionContext);
                foreach (GamePlayDTO gamePlayDTO in gamePlayDTOList)
                {

                    List<KeyValuePair<GamePlayInfoDTO.SearchByParameters, string>> searchByParameters = new List<KeyValuePair<GamePlayInfoDTO.SearchByParameters, string>>();
                    searchByParameters.Add(new KeyValuePair<GamePlayInfoDTO.SearchByParameters, string>(GamePlayInfoDTO.SearchByParameters.GAME_PLAY_ID, gamePlayDTO.GameplayId.ToString()));
                    gamePlayInfoDTOList = gamePlayInfoList.GetGamePlayInfoDTOList(searchByParameters, sqlTransaction); //added sqltransaction
                    if (gamePlayInfoList != null)
                    {
                        gamePlayDTO.GamePlayInfoDTOList = new List<GamePlayInfoDTO>(gamePlayInfoDTOList);
                        gamePlayDTO.AcceptChanges();
                    }


                }
            }

            log.LogMethodEntry(gamePlayDTOList);
            return gamePlayDTOList;
    }

        /// <summary>
        /// Returns the GamePlay list
        /// </summary>
        /// <param name="lastRunTime">lastRunTime</param>
        /// <param name="currentRunTime">currentRunTime</param>
        /// <param name="siteId">siteId</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>returnList</returns>
        public List<GamePlayDTO> GetGamePlayByDateTime(DateTime lastRunTime, DateTime currentRunTime, int siteId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(lastRunTime, currentRunTime, siteId, sqlTransaction);
            GamePlayDataHandler gamePlayDataHandler = new GamePlayDataHandler(sqlTransaction);
            List<GamePlayDTO> returnList = gamePlayDataHandler.GetGamePlayByDateTime(lastRunTime, currentRunTime, siteId);
            log.LogMethodExit(returnList);
            return returnList;
        }

        /// <summary>
        /// Returns the GamePlay list with tag number
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <param name="PopulateTagNumber">PopulateTagNumber</param>
        /// <returns>returnValue</returns>
        public List<GamePlayDTO> GetGamePlayDTOListWithTagNumber(List<KeyValuePair<GamePlayDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null, bool PopulateTagNumber = false)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction, PopulateTagNumber);
            GamePlayDataHandler gamePlayDataHandler = new GamePlayDataHandler(sqlTransaction);
            List<GamePlayDTO> returnValue = gamePlayDataHandler.GetGamePlayDTOListWithTagNumber(searchParameters);
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        public int GetGamePlayCount(List<KeyValuePair<GamePlayDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            GamePlayDataHandler gamePlayDataHandler = new GamePlayDataHandler(sqlTransaction);
            int count = gamePlayDataHandler.GetGamePlayCount(searchParameters);
            log.LogMethodExit(count);
            return count;
        }

        public List<GamePlayDTO> GetGamePlays(List<KeyValuePair<GamePlayDTO.SearchByParameters, string>> searchParameters,
                                               int take , int skip, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            GamePlayDataHandler gamePlayDataHandler = new GamePlayDataHandler(sqlTransaction);
            List<GamePlayDTO> returnValue = gamePlayDataHandler.GetGamePlays(searchParameters, skip, take);
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// Save and Update for Price List.
        /// </summary>
        public void SaveUpdateGamePlay(SqlTransaction sqlTransaction = null)  //added save for List
        {
            try
            {
                log.LogMethodEntry(sqlTransaction);
                if (gamePlayDTOList != null)
                {
                    foreach (GamePlayDTO gamePlayDTO in gamePlayDTOList)
                    {
                        GamePlayBL gamePlayObj = new GamePlayBL(executionContext, gamePlayDTO);
                        gamePlayObj.Save(sqlTransaction);
                    }
                }
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
        }
    }
}
