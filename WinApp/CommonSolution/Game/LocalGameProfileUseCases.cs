/********************************************************************************************
 * Project Name - Game
 * Description  - LocalGamepProfileUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0      18-Dec-2020     Prajwal S                  Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Game
{
    class LocalGameProfileUseCases : IGameProfileUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalGameProfileUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<GameProfileDTO>> GetGameProfiles(List<KeyValuePair<GameProfileDTO.SearchByGameProfileParameters, string>>
                          searchParameters, bool loadChildRecords = true, int currentPage = 0, int pageSize = 0, SqlTransaction sqlTransaction = null,
                         bool activeChildRecords = true)
        {
            return await Task<List<GameProfileDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                GameProfileList gameProfileListBL = new GameProfileList(executionContext);
                List<GameProfileDTO> gameProfileDTOList = gameProfileListBL.GetGameProfileDTOList(searchParameters, loadChildRecords,currentPage, pageSize, sqlTransaction, activeChildRecords);//currentPage, pageSize, sqlTransaction);
                log.LogMethodExit(gameProfileDTOList);
                return gameProfileDTOList;
            });
        }

        public async Task<List<GameProfileDTO>> SaveGameProfiles(List<GameProfileDTO> gameProfileDTOList)
        {
            return await Task<List<GameProfileDTO>>.Factory.StartNew(() =>
            {
                List<GameProfileDTO> result = null;
                log.LogMethodEntry(gameProfileDTOList);
                if (gameProfileDTOList == null)
                {
                    throw new ValidationException("GameProfileDTOList is Empty");
                }

                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    parafaitDBTrx.BeginTransaction();
                    GameProfileList gameProfileList = new GameProfileList(gameProfileDTOList, executionContext);
                    result = gameProfileList.SaveUpdateGameProfileList(parafaitDBTrx.SQLTrx);
                    parafaitDBTrx.EndTransaction();

                }
                log.LogMethodExit(result);
                return result;
            });
        }


        public async Task<GameProfileContainerDTOCollection> GetGameProfileContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            return await Task<GameProfileContainerDTOCollection>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(siteId, hash, rebuildCache);
                if (rebuildCache)
                {
                    GameProfileContainerList.Rebuild(siteId);
                }
                List<GameProfileContainerDTO> gameProfileContainerDTOList = GameProfileContainerList.GetGameProfileContainerDTOList(siteId,executionContext);
                GameProfileContainerDTOCollection result = new GameProfileContainerDTOCollection(gameProfileContainerDTOList);
                if (hash == result.Hash)
                {
                    log.LogMethodExit(null, "No changes to the cache");
                    return null;
                }
                log.LogMethodExit(result);
                return result;
            });
        }

        public async Task<int> GetGameProfileCount(List<KeyValuePair<GameProfileDTO.SearchByGameProfileParameters, string>>
                                                  searchParameters, SqlTransaction sqlTransaction = null
                         )
        {
            return await Task<int>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                GameProfileList gameProfileListBL = new GameProfileList(executionContext);
                int count = gameProfileListBL.GetGameProfileCount(searchParameters, sqlTransaction);

                log.LogMethodExit(count);
                return count;
            });
        }

        public async Task<string> DeleteGameProfiles(List<GameProfileDTO> gameProfileDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(gameProfileDTOList);
                    GameProfileList gameProfileList = new GameProfileList(gameProfileDTOList, executionContext);
                    gameProfileList.DeleteGameProfileList();
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
    }
}

    

