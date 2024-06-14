/********************************************************************************************
 * Project Name - Game
 * Description  - RemoteMachineUseCases class to get the data  from API by doing remote call  
 *  
 **************
 **Version Log
 ************** 
 *Version     Date              Modified By               Remarks          
 *********************************************************************************************
 2.110.0      08-Dec-2020       Prajwal S                 Created : POS UI Redesign with REST API
 2.110.0      04-Jan-2021       Fiona                       Modified to get Machine Count
 2.130.0      06-Aug-2021      Abhishek                     Modified to get Machine attribute use case
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Semnox.Core.Utilities;
using static Semnox.Parafait.Game.MachineConfigurationClass;

namespace Semnox.Parafait.Game
{
    public class RemoteMachineUseCases : RemoteUseCases, IMachineUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string Machine_URL = "api/Game/Machines";
        private const string Machine_CONTAINER_URL = "api/Game/MachineContainer";
        private const string Machine_COUNT_URL = "api/Game/MachineCount";
        private const string Machine_ATTRIBUTE_LOG_URL = "api/Game/MachineAttributeLogs";
        private const string Machine_QRCode_URL = "api/Game/Machine/{machineId}/QRcode";
        private const string Machine_Configuration_URL = "api/Game/{machineId}/MachineConfiguration";

        public RemoteMachineUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<List<MachineDTO>> GetMachines(List<KeyValuePair<MachineDTO.SearchByMachineParameters, string>>
                          parameters, bool loadChildRecords = false, int currentPage = 0, int pageSize = 0, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parameters);

            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("loadAttribute".ToString(), loadChildRecords.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("currentPage".ToString(), currentPage.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("pageSize".ToString(), pageSize.ToString()));
            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            try
            {
                List<MachineDTO> result = await Get<List<MachineDTO>>(Machine_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<MachineDTO.SearchByMachineParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<MachineDTO.SearchByMachineParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case MachineDTO.SearchByMachineParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                    case MachineDTO.SearchByMachineParameters.MACHINE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("machineId".ToString(), searchParameter.Value));
                        }
                        break;
                    case MachineDTO.SearchByMachineParameters.GAME_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("gameId".ToString(), searchParameter.Value));
                        }
                        break;
                    case MachineDTO.SearchByMachineParameters.MASTER_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("masterId".ToString(), searchParameter.Value));
                        }
                        break;
                    case MachineDTO.SearchByMachineParameters.REFERENCE_MACHINE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("referenceMahineId".ToString(), searchParameter.Value));
                        }
                        break;
                    case MachineDTO.SearchByMachineParameters.MACHINE_NAME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("machineName".ToString(), searchParameter.Value));
                        }
                        break;
                    case MachineDTO.SearchByMachineParameters.EXTERNAL_MACHINE_REFERENCE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("externalMachineReference".ToString(), searchParameter.Value));
                        }
                        break;

                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }

        public async Task<string> SaveMachines(List<MachineDTO> machineDTOList)
        {
            log.LogMethodEntry(machineDTOList);
            try
            {
                string responseString = await Post<string>(Machine_URL, machineDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<MachineContainerDTOCollection> GetMachineContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(hash, rebuildCache);
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("siteId", siteId.ToString()));
            if (string.IsNullOrWhiteSpace(hash) == false)
            {
                parameters.Add(new KeyValuePair<string, string>("hash", hash));
            }
            parameters.Add(new KeyValuePair<string, string>("rebuildCache", rebuildCache.ToString()));
            MachineContainerDTOCollection result = await Get<MachineContainerDTOCollection>(Machine_CONTAINER_URL, parameters);
            log.LogMethodExit(result);
            return result;
        }
        public async Task<string> DeleteMachines(List<MachineDTO> machineDTOList)
        {
            try
            {
                log.LogMethodEntry(machineDTOList);
                string responseString = await Delete<string>(Machine_URL, machineDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (WebApiException wex)
            {
                log.Error(wex);
                throw;
            }
        }

        public async Task<int> GetMachineCount(List<KeyValuePair<MachineDTO.SearchByMachineParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);

            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            if (searchParameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(searchParameters));
            }
            try
            {
                int result = await Get<int>(Machine_COUNT_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        public async Task<List<MachineAttributeLogDTO>> GetMachineAttributeLogs(List<KeyValuePair<MachineAttributeLogDTO.SearchByParameters, string>> parameters)
        {
            log.LogMethodEntry(parameters);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            if (parameters != null)
            {
                searchParameterList.AddRange(BuildMachineAttributeLogSearchParameter(parameters));
            }
            try
            {
                List<MachineAttributeLogDTO> result = await Get<List<MachineAttributeLogDTO>>(Machine_ATTRIBUTE_LOG_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        private List<KeyValuePair<string, string>> BuildMachineAttributeLogSearchParameter(List<KeyValuePair<MachineAttributeLogDTO.SearchByParameters, string>> parameters)
        {
            log.LogMethodEntry(parameters);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<MachineAttributeLogDTO.SearchByParameters, string> searchParameter in parameters)
            {
                switch (searchParameter.Key)
                {

                    case MachineAttributeLogDTO.SearchByParameters.ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("machineAttributeLogId".ToString(), searchParameter.Value));
                        }
                        break;
                    case MachineAttributeLogDTO.SearchByParameters.MACHINE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("gameMachineId".ToString(), searchParameter.Value));
                        }
                        break;
                    case MachineAttributeLogDTO.SearchByParameters.POS_MACHINEID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("posMachineId".ToString(), searchParameter.Value));
                        }
                        break;
                    case MachineAttributeLogDTO.SearchByParameters.POS_NAME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("posMachineName".ToString(), searchParameter.Value));
                        }
                        break;
                    case MachineAttributeLogDTO.SearchByParameters.UPDATE_TYPE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("updateType".ToString(), searchParameter.Value));
                        }
                        break;
                    case MachineAttributeLogDTO.SearchByParameters.STATUS:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("status".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }

        public async Task<List<clsConfig>> GetMachineConfiguration(int machineId, int promotionDetailId)
        {
            log.LogMethodEntry(machineId, promotionDetailId);

            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("machineId".ToString(), machineId.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("promotionDetailId".ToString(), promotionDetailId.ToString()));

            List<clsConfig> result = await Get<List<clsConfig>>(Machine_Configuration_URL.Replace("{machineId}", machineId.ToString()), searchParameterList);
            log.LogMethodExit(result);
            return result;
        }
    }
}



