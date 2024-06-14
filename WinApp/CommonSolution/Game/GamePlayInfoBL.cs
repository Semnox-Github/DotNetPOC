/********************************************************************************************
 * Project Name -GamePlayInfo 
 * Description  - GamePlayInfoBL
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 ********************************************************************************************* 
 *2.70.2        24-Jul-2019   Deeksha          Modified : save method returns DTO.Added log() methods.
  *2.110.0       13-Dec-2020   Prajwal S        Modified : Constructor with Id Parameter.
 *                                             Added : Constructor with ExecutionContext as parameter. Added Save for List.
 ********************************************************************************************/

using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Semnox.Parafait.Game
{

    /// <summary>
    /// GamePlayInfoBL
    /// </summary>
    public class GamePlayInfoBL
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private GamePlayInfoDTO gamePlayInfoDTO;
        private SqlTransaction sqlTransaction;
        private ExecutionContext executionContext;

        /// <summary>
        /// Default constructor
        /// </summary>
        public GamePlayInfoBL()
        {
            log.LogMethodEntry();
            gamePlayInfoDTO = null;
            log.LogMethodExit();
        }

        private GamePlayInfoBL(ExecutionContext executionContext) //added constructor
           : this()
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the id parameter
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public GamePlayInfoBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null) //modified
            : this(executionContext)
        {
            log.LogMethodEntry(id, sqlTransaction);
            GamePlayInfoDataHandler gamePlayDataHandler = new GamePlayInfoDataHandler(sqlTransaction);
            this.gamePlayInfoDTO = gamePlayDataHandler.GetGamePlayInfoDTO(id);
            if (gamePlayInfoDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "gamePlayInfoDTO", id);    //added condition
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the GamePlayInfoDTO parameter
        /// </summary>
        /// <param name="gamePlayInfoDTO">GamePlayInfoDTO</param>
        public GamePlayInfoBL(ExecutionContext executionContext, GamePlayInfoDTO gamePlayInfoDTO)
        {
            log.LogMethodEntry(gamePlayInfoDTO);
            this.executionContext = executionContext;
            this.gamePlayInfoDTO = gamePlayInfoDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// get GamePlayInfoDTO Object
        /// </summary>
        public GamePlayInfoDTO GetGamePlayInfoDTO
        {
            get { return gamePlayInfoDTO; }
        }

        /// <summary>
        /// Saves the GamePlayInfoDTO
        /// Checks if the Id is not less than or equal to 0
        ///     If it is less than or equal to 0, then inserts
        ///     else updates
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            GamePlayInfoDataHandler gamePlayInfoDataHandler = new GamePlayInfoDataHandler(sqlTransaction);
            if (gamePlayInfoDTO.Id < 0)
            {
                gamePlayInfoDTO = gamePlayInfoDataHandler.InsertGamePlayInfoDTO(gamePlayInfoDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                gamePlayInfoDTO.AcceptChanges();
            }
            else
            {
                if (gamePlayInfoDTO.IsChanged )
                {
                    gamePlayInfoDataHandler.UpdateGamePlayInfoDTO(gamePlayInfoDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    gamePlayInfoDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }
    }

    /// <summary>
    /// Manages the list of GamePlayInfoDTO
    /// </summary>
    public class GamePlayInfoList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<GamePlayInfoDTO> gamePlayInfoDTOList = new List<GamePlayInfoDTO>(); //added
        private ExecutionContext executionContext; //added
        /// <summary>
        /// Returns the GetGamePlayInfoDTOList list
        ///<param name="searchParameters">searchParameters</param>
        ///<param name="sqlTransaction">sqlTransaction</param>
        /// </summary>
        /// 

        public GamePlayInfoList(ExecutionContext executionContext)  //added constructor
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// parameterized constructor 
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public GamePlayInfoList(ExecutionContext executionContext, List<GamePlayInfoDTO> gamePlayInfoDTOList) : this(executionContext)  //added constructor
        {
            log.LogMethodEntry(executionContext, gamePlayInfoDTOList);
            this.gamePlayInfoDTOList = gamePlayInfoDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the GetGamePlayInfoDTOList list
        ///<param name="searchParameters">searchParameters</param>
        ///<param name="sqlTransaction">sqlTransaction</param>
        /// </summary>
        public List<GamePlayInfoDTO> GetGamePlayInfoDTOList(List<KeyValuePair<GamePlayInfoDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            GamePlayInfoDataHandler gamePlayInfoDataHandler = new GamePlayInfoDataHandler(sqlTransaction);
            List<GamePlayInfoDTO> gamePlayInfoDTOs = new List<GamePlayInfoDTO>();
            gamePlayInfoDTOs= gamePlayInfoDataHandler.GetGamePlayInfoDTOList(searchParameters);
            log.LogMethodExit(gamePlayInfoDTOs);
            return gamePlayInfoDTOs;
        }

        /// <summary>
        ///Takes LookupParams as parameter
        /// </summary>
        /// <param name="gamePalyInfoDTO">gamePalyInfoDTO</param>
        /// <returns>Returns List of KeyValuePair (GamePlayInfoDTO.SearchByParameters, string) by converting GamePlayInfoDTO</returns>
        public List<KeyValuePair<GamePlayInfoDTO.SearchByParameters, string>> BuildGamePlayInfoDTOSearchParametersList(GamePlayInfoDTO gamePalyInfoDTO)
        {
            log.LogMethodEntry(gamePalyInfoDTO);
            List<KeyValuePair<GamePlayInfoDTO.SearchByParameters, string>> gamePlayInfoDTOSearchParams = new List<KeyValuePair<GamePlayInfoDTO.SearchByParameters, string>>();
            if (gamePalyInfoDTO != null)
            {
                if (gamePalyInfoDTO.Id > 0)
                    gamePlayInfoDTOSearchParams.Add(new KeyValuePair<GamePlayInfoDTO.SearchByParameters, string>(GamePlayInfoDTO.SearchByParameters.ID, gamePalyInfoDTO.Id.ToString()));

                if (gamePalyInfoDTO.GameplayId > 0)
                    gamePlayInfoDTOSearchParams.Add(new KeyValuePair<GamePlayInfoDTO.SearchByParameters, string>(GamePlayInfoDTO.SearchByParameters.GAME_PLAY_ID, gamePalyInfoDTO.GameplayId.ToString()));

                if (gamePalyInfoDTO.SiteId > 0)
                    gamePlayInfoDTOSearchParams.Add(new KeyValuePair<GamePlayInfoDTO.SearchByParameters, string>(GamePlayInfoDTO.SearchByParameters.SITE_ID, gamePalyInfoDTO.SiteId.ToString()));

                if (gamePalyInfoDTO.ReaderRecordId > 0)
                    gamePlayInfoDTOSearchParams.Add(new KeyValuePair<GamePlayInfoDTO.SearchByParameters, string>(GamePlayInfoDTO.SearchByParameters.READER_RECORD_ID, gamePalyInfoDTO.ReaderRecordId.ToString()));

                if (!(string.IsNullOrEmpty(gamePalyInfoDTO.Status)))
                    gamePlayInfoDTOSearchParams.Add(new KeyValuePair<GamePlayInfoDTO.SearchByParameters, string>(GamePlayInfoDTO.SearchByParameters.STATUS, gamePalyInfoDTO.Status.ToString()));
            }
            log.LogMethodExit(gamePlayInfoDTOSearchParams);
            return gamePlayInfoDTOSearchParams;
        }


        /// <summary>
        /// GetGamePlayInfoDTOsList(GamePlayInfoDTO gamePlayInfoDTO) method search based on gamePlayInfoDTO
        /// </summary>
        /// <param name="gamePlayInfoDTO">GamePlayInfoDTO turnstileDTO</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>List of GamePlayInfoDTO object</returns>
        public List<GamePlayInfoDTO> GetGamePlayInfoDTOList(GamePlayInfoDTO gamePlayInfoDTO, SqlTransaction sqlTransaction = null)
        {
            try
            {
                log.LogMethodEntry(gamePlayInfoDTO, sqlTransaction);
                List<KeyValuePair<GamePlayInfoDTO.SearchByParameters, string>> searchParameters = BuildGamePlayInfoDTOSearchParametersList(gamePlayInfoDTO);
                GamePlayInfoDataHandler gamePlayDataHandler = new GamePlayInfoDataHandler(sqlTransaction);
                List<GamePlayInfoDTO> gamePlayInfoDTOs = new List<GamePlayInfoDTO>();
                gamePlayInfoDTOs = gamePlayDataHandler.GetGamePlayInfoDTOList(searchParameters);
                log.LogMethodExit(gamePlayInfoDTOs);
                return gamePlayInfoDTOs;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "throwing exception");
                throw;
            }
        }

        public void Save(SqlTransaction sqlTransaction = null)  //added save for List
        {
            log.LogMethodEntry(sqlTransaction);
            if (gamePlayInfoDTOList == null ||
                gamePlayInfoDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < gamePlayInfoDTOList.Count; i++)
            {
                var gamePlayInfoDTO = gamePlayInfoDTOList[i];
                if (gamePlayInfoDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    GamePlayInfoBL gamePlayInfoBL = new GamePlayInfoBL(executionContext, gamePlayInfoDTO);
                    gamePlayInfoBL.Save(sqlTransaction);
                }
                catch (SqlException sqlEx)
                {
                    log.Error(sqlEx);
                    log.LogMethodExit(null, "Throwing Validation Exception : " + sqlEx.Message);
                    if (sqlEx.Number == 547)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869));
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (ValidationException valEx)
                {
                    log.Error(valEx);
                    log.LogMethodExit(null, "Throwing Validation Exception : " + valEx.Message);
                    throw;
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving gamePlayInfoDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("gamePlayInfoDTO", gamePlayInfoDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }

    }
}




