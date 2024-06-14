/********************************************************************************************
 * Project Name - Machine
 * Description  - LocalMachineUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0      18-Dec-2020      Prajwal S                 Created : POS UI Redesign with REST API
 2.110.0      04-Feb-2021      Fiona                     Modified to get Machine Count
 2.130.0      06-Aug-2021      Fiona                     Modified to get Machine attribute use case
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using static Semnox.Parafait.Game.MachineConfigurationClass;

namespace Semnox.Parafait.Game
{
    class LocalMachineUseCases : IMachineUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalMachineUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<MachineDTO>> GetMachines(List<KeyValuePair<MachineDTO.SearchByMachineParameters, string>>
                          searchParameters, bool loadChildRecords = false, int currentPage = 0, int pageSize = 0, SqlTransaction sqlTransaction = null
                         )
        {
            return await Task<List<MachineDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                MachineList machineListBL = new MachineList(executionContext);
                List<MachineDTO> machineDTOList = machineListBL.GetMachineList(searchParameters, loadChildRecords, sqlTransaction, currentPage, pageSize);

                log.LogMethodExit(machineDTOList);
                return machineDTOList;
            });
        }

        public async Task<string> SaveMachines(List<MachineDTO> machineDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = "Failed";
                log.LogMethodEntry(machineDTOList);
                if (machineDTOList == null)
                {
                    throw new ValidationException("MachineDTOList is Empty");
                }

                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {

                    try
                    {
                        parafaitDBTrx.BeginTransaction();
                        MachineList machineList = new MachineList(executionContext, machineDTOList);
                        machineList.SaveUpdateMachinesList(parafaitDBTrx.SQLTrx);
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
                result = "Success";
                log.LogMethodExit(result);
                return result;
            });
        }


        public async Task<MachineContainerDTOCollection> GetMachineContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            return await Task<MachineContainerDTOCollection>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(siteId, hash, rebuildCache);
                if (rebuildCache)
                {
                    MachineContainerList.Rebuild(siteId);
                }
                List<MachineContainerDTO> machineContainerDTOList = MachineContainerList.GetMachineContainerDTOList(siteId);
                MachineContainerDTOCollection result = new MachineContainerDTOCollection(machineContainerDTOList);
                if (hash == result.Hash)
                {
                    log.LogMethodExit(null, "No changes to the cache");
                    return null;
                }
                log.LogMethodExit(result);
                return result;
            });
        }

        public async Task<string> DeleteMachines(List<MachineDTO> machineDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(machineDTOList);
                    MachineList MachineList = new MachineList(executionContext, machineDTOList);
                    MachineList.DeleteMachineList();
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

        public async Task<int> GetMachineCount(List<KeyValuePair<MachineDTO.SearchByMachineParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            return await Task<int>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                MachineList machineListListBL = new MachineList(executionContext);
                int count = machineListListBL.GetMachineCount(searchParameters, sqlTransaction);

                log.LogMethodExit(count);
                return count;
            });
        }
        public async Task<List<MachineAttributeLogDTO>> GetMachineAttributeLogs(List<KeyValuePair<MachineAttributeLogDTO.SearchByParameters, string>> searchParameters)
        {
            return await Task<List<MachineAttributeLogDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);
                MachineAttributeLogListBL machineAttributeLogListBL = new MachineAttributeLogListBL(executionContext);
                List<MachineAttributeLogDTO> machineAttributeLogDTOList = machineAttributeLogListBL.GetMachineAttributeLogs(searchParameters);
                log.LogMethodEntry(machineAttributeLogDTOList);
                return machineAttributeLogDTOList;
            });
        }

        /// <summary>
        ///GetMachineConfiguration
        /// </summary>
        /// <param name="machineId">Machine Id</param>
        /// <param name="promotionDetailId">promotionDetailId</param>
        /// <returns>MachineConfigurationClass</returns>
        public async Task<List<clsConfig>> GetMachineConfiguration(int machineId, int promotionDetailId)
        {
            return await Task<List<clsConfig>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(machineId, promotionDetailId);
                List<clsConfig> clsConfigs = new List<clsConfig>();
                MachineAttributeListBL machineAttributeListBL = new MachineAttributeListBL(executionContext);
                clsConfigs = machineAttributeListBL.Populate(machineId, promotionDetailId);
                log.LogMethodEntry(clsConfigs);
                return clsConfigs;
            });
        }
    }
}

