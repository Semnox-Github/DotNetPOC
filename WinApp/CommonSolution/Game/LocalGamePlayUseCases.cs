/********************************************************************************************
 * Project Name - Game
 * Description  - LocalGamepPlayUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0      16-Dec-2020     Prajwal S                  Created : POS UI Redesign with REST API
 2.110.0      04-Feb-2020     Fiona                      Modified by adding GetGamePlayCount()
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Game
{
    class LocalGamePlayUseCases : IGamePlayUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalGamePlayUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<GamePlayDTO>> GetGamePlays(List<KeyValuePair<GamePlayDTO.SearchByParameters, string>>
                          searchParameters, bool loadChildRecords = false, int currentPage = 0, int pageSize = 0, SqlTransaction sqlTransaction = null )
        {
            return await Task<List<GamePlayDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);
                GamePlayListBL gamePlayListBL = new GamePlayListBL(executionContext);
                List<GamePlayDTO> gamePlayDTOList = gamePlayListBL.GetGamePlayDTOList(searchParameters,loadChildRecords, currentPage, pageSize, sqlTransaction);
                log.LogMethodExit(gamePlayDTOList);
                return gamePlayDTOList;
            });
        }

        public async Task<int> GetGamePlayCount(List<KeyValuePair<GamePlayDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            return await Task<int>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                GamePlayListBL gamePlayListBL = new GamePlayListBL(executionContext);
                int count = gamePlayListBL.GetGamePlayCount(searchParameters, sqlTransaction);

                log.LogMethodExit(count);
                return count;
            });
        }

        public async Task<string> SaveGamePlays(List<GamePlayDTO> gamePlayDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(gamePlayDTOList);
                    if (gamePlayDTOList == null)
                    {
                        throw new ValidationException("GamePlayDTOList is Empty");
                    }

                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                        foreach (GamePlayDTO gamePlayDTO in gamePlayDTOList)
                        {
                            try
                            {
                                parafaitDBTrx.BeginTransaction();
                                GamePlayBL gamePlayBL = new GamePlayBL(executionContext, gamePlayDTO);
                                gamePlayBL.Save(parafaitDBTrx.SQLTrx);
                                parafaitDBTrx.EndTransaction();
                            }
                            catch (ValidationException valEx)
                            {
                                parafaitDBTrx.RollBack();
                                log.Error(valEx);
                                throw valEx;
                            }
                            catch (Exception ex)
                            {
                                parafaitDBTrx.RollBack();
                                log.Error(ex);
                                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                                throw ex;
                            }
                        }
                    }

                    result = "Success";
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    result = "Falied";
                }
                log.LogMethodExit(result);
                return result;
            });
        }
    }
}

