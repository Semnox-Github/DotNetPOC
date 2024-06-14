/********************************************************************************************
 * Project Name - Game
 * Description  - LocalGameUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By                Remarks          
 *********************************************************************************************
 2.110.0      10-Dec-2020      Prajwal S                  Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Game
{
    class LocalGameUseCases : IGameUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalGameUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<GameDTO>> GetGames(List<KeyValuePair<GameDTO.SearchByGameParameters, string>>
                          searchParameters, bool loadChildRecords = true, int currentPage = 0, int pageSize = 0,
                         bool activateChildRecords = false)
        {
            return await Task<List<GameDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                GameList gameListBL = new GameList(executionContext);
                List<GameDTO> gameDTOList = gameListBL.GetGameList(searchParameters, loadChildRecords, null, currentPage, pageSize, activateChildRecords);
                log.LogMethodExit(gameDTOList);
                return gameDTOList;
            });
        }

        public async Task<int> GetGameCount(List<KeyValuePair<GameDTO.SearchByGameParameters, string>>
                                                      searchParameters, SqlTransaction sqlTransaction = null
                             )
        {
            return await Task<int>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                GameList gameListBL = new GameList(executionContext);
                int count = gameListBL.GetGameCount(searchParameters, sqlTransaction);

                log.LogMethodExit(count);
                return count;
            });
        }

        public async Task<List<GameDTO>> SaveGames(List<GameDTO> gameDTOList)
        {
            return await Task<List<GameDTO>>.Factory.StartNew(() =>
            {
                List<GameDTO> result = null;
                log.LogMethodEntry(gameDTOList);
                if (gameDTOList == null)
                {
                    throw new ValidationException("GameDTOList is Empty");
                }
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    parafaitDBTrx.BeginTransaction();
                    GameList gameList = new GameList(gameDTOList, executionContext);
                    result = gameList.SaveUpdateGameList(parafaitDBTrx.SQLTrx);
                    parafaitDBTrx.EndTransaction();
                }
                log.LogMethodExit(result);
                return result;
            });
        }


        public async Task<GameContainerDTOCollection> GetGameContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            return await Task<GameContainerDTOCollection>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(siteId, hash, rebuildCache);
                if (rebuildCache)
                {
                    GameContainerList.Rebuild(siteId);
                }
                List<GameContainerDTO> gameContainerDTOList = GameContainerList.GetGameContainerDTOList(siteId, executionContext);
                GameContainerDTOCollection result = new GameContainerDTOCollection(gameContainerDTOList);
                if (hash == result.Hash)
                {
                    log.LogMethodExit(null, "No changes to the cache");
                    return null;
                }
                log.LogMethodExit(result);
                return result;
            });
        }

        public async Task<string> DeleteGames(List<GameDTO> gameDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(gameDTOList);
                    GameList gameList = new GameList(gameDTOList, executionContext);
                    gameList.DeleteGameList();
                    result = "Success";
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    result = "Falied";
                    throw ex;
                }
                log.LogMethodExit(result);
                return result;
            });
        }
        public async Task<List<AllowedMachineNamesDTO>> GetAllowedMachineNames(int allowedMachineId = -1, int gameId = -1, string machineName = null, string isActive = null, int siteId = -1)
        {
            return await Task<List<AllowedMachineNamesDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(allowedMachineId,gameId,machineName,isActive,siteId);
                List<KeyValuePair<AllowedMachineNamesDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<AllowedMachineNamesDTO.SearchByParameters, string>>();
                if (siteId == -1)
                {
                    searchParameters.Add(new KeyValuePair<AllowedMachineNamesDTO.SearchByParameters, string>(AllowedMachineNamesDTO.SearchByParameters.SITE_ID, Convert.ToString(executionContext.GetSiteId())));
                }
                else
                {
                    searchParameters.Add(new KeyValuePair<AllowedMachineNamesDTO.SearchByParameters, string>(AllowedMachineNamesDTO.SearchByParameters.SITE_ID, Convert.ToString(siteId)));
                }
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<AllowedMachineNamesDTO.SearchByParameters, string>(AllowedMachineNamesDTO.SearchByParameters.IS_ACTIVE, isActive));
                    }
                }
                if (gameId > -1)
                {
                    searchParameters.Add(new KeyValuePair<AllowedMachineNamesDTO.SearchByParameters, string>(AllowedMachineNamesDTO.SearchByParameters.GAME_ID, gameId.ToString()));
                }
                if (allowedMachineId > -1)
                {
                    searchParameters.Add(new KeyValuePair<AllowedMachineNamesDTO.SearchByParameters, string>(AllowedMachineNamesDTO.SearchByParameters.ALLOWED_MACHINE_ID, allowedMachineId.ToString()));
                }
                if (!string.IsNullOrWhiteSpace(machineName))
                {
                    searchParameters.Add(new KeyValuePair<AllowedMachineNamesDTO.SearchByParameters, string>(AllowedMachineNamesDTO.SearchByParameters.MACHINE_NAME, machineName));
                }
                AllowedMachineNamesListBL allowedMachineNamesListBL = new AllowedMachineNamesListBL(executionContext);
                List<AllowedMachineNamesDTO> allowedMachineNamesDTOList = allowedMachineNamesListBL.GetAllowedMachineNamesList(searchParameters, null);
                log.LogMethodExit(allowedMachineNamesDTOList);
                return allowedMachineNamesDTOList;
            });
        }
        public async Task<string> SaveAllowedMachineNames(int gameId, List<AllowedMachineNamesDTO> allowedMachineNamesDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = null;
                try
                {
                    if (allowedMachineNamesDTOList == null)
                    {
                        string errorMessage = "allowedMachineNamesDTOList is empty";
                        log.LogMethodExit("Throwing Exception- " + errorMessage);
                        throw new ValidationException(errorMessage);
                    }
                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                        parafaitDBTrx.BeginTransaction();
                        foreach (AllowedMachineNamesDTO allowedMachineNamesDTO in allowedMachineNamesDTOList)
                        {
                            allowedMachineNamesDTO.GameId = gameId;
                        }
                        AllowedMachineNamesListBL allowedMachineNamesListBL = new AllowedMachineNamesListBL(executionContext, allowedMachineNamesDTOList);
                        allowedMachineNamesListBL.Save(parafaitDBTrx.SQLTrx);
                        parafaitDBTrx.EndTransaction();
                    }
                    log.LogMethodExit(result);
                    result = "Sucess";
                }
                catch (Exception ex)
                {
                    result = "Failure";
                    log.Error(ex);
                    throw ex;
                }
                return result;

            });
        }
    }
}

