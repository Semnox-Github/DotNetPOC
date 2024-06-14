/********************************************************************************************
 * Project Name - Game
 * Description  - LocalHubUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By                Remarks          
 *********************************************************************************************
 2.110.0      10-Dec-2020      Prajwal S                  Created : POS UI Redesign with REST API
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Game
{
    class LocalHubUseCases : IHubUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalHubUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<HubDTO>> GetHubs(List<KeyValuePair<HubDTO.SearchByHubParameters, string>>
                          searchParameters, bool loadMachineCount = false, bool loadChild = false, int currentPage = 0, int pageSize = 0, SqlTransaction sqlTransaction = null
                          )
        {
            return await Task<List<HubDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                HubList hubListBL = new HubList(executionContext);
                List<HubDTO> hubDTOList = hubListBL.GetHubSearchList(searchParameters, sqlTransaction, loadMachineCount, loadChild, currentPage, pageSize);

                log.LogMethodExit(hubDTOList);
                return hubDTOList;
            });
        }

        public async Task<string> SaveHubs(List<HubDTO> hubDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = "Falied";
                log.LogMethodEntry(hubDTOList);
                if (hubDTOList == null)
                {
                    throw new ValidationException("HubDTOList is Empty");
                }

                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    try
                    {
                        parafaitDBTrx.BeginTransaction();
                        HubList hubList = new HubList(executionContext, hubDTOList);
                        hubList.Save(parafaitDBTrx.SQLTrx);
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
                        throw new Exception(ex.Message, ex);
                    }
                }
                result = "Success";
                log.LogMethodExit(result);
                return result;
            });
        }

        public async Task<int> GetHubCount(List<KeyValuePair<HubDTO.SearchByHubParameters, string>>
                                                       searchParameters,SqlTransaction sqlTransaction = null
                              )
        {
            return await Task<int>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                HubList hubListBL = new HubList(executionContext);
                int count = hubListBL.GetHubCount(searchParameters, sqlTransaction);

                log.LogMethodExit(count);
                return count;
            });
        }


        public async Task<HubContainerDTOCollection> GetHubContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            return await Task<HubContainerDTOCollection>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(siteId, hash, rebuildCache);
                if (rebuildCache)
                {
                    HubContainerList.Rebuild(siteId);
                }
                List<HubContainerDTO> hubContainerDTOList = HubContainerList.GetHubContainerDTOList(siteId);
                HubContainerDTOCollection result = new HubContainerDTOCollection(hubContainerDTOList);
                if (hash == result.Hash)
                {
                    log.LogMethodExit(null, "No changes to the cache");
                    return null;
                }
                log.LogMethodExit(result);
                return result;
            });
        }

        public async Task<string> DeleteHubs(List<HubDTO> hubDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(hubDTOList);
                    HubList hubList = new HubList(executionContext, hubDTOList);
                    hubList.Delete();
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

        /// <summary>
        /// SaveHubStatus
        /// </summary>
        /// <param name="hubId">hubId</param>
        /// <param name="hubStatusDTO">hubStatusDTO</param>
        /// <returns>HubDTO</returns>
        public async Task<HubDTO> SaveHubStatus(int hubId, HubStatusDTO hubStatusDTO)
        {
            return await Task<HubDTO>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(hubId, hubStatusDTO);
                Hub hub = new Hub(hubId);
                if (hub.GetHubDTO == null)
                {
                    string message = MessageContainerList.GetMessage(executionContext, "Hub Not Found");
                    log.Error(message);
                    throw new ValidationException(message);
                }
                HubDTO hubDTO = hub.GetHubDTO;
                hubDTO.RestartAP = hubStatusDTO.RestartAP;
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    try
                    {
                        parafaitDBTrx.BeginTransaction();
                        hub = new Hub(executionContext, hubDTO);
                        hub.Save(parafaitDBTrx.SQLTrx);
                        hubDTO = hub.GetHubDTO;
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
                        throw new Exception(ex.Message, ex);
                    }
                }
                log.LogMethodExit(hubDTO);
                return hubDTO;
            });
        }
    }
}
