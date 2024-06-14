/********************************************************************************************
 * Project Name - Games
 * Description  - RemoteReaderConfigurationUseCases class to get the data  from API by doing remote call  
 *  
 **************
 **Version Log
 ************** 
 *Version     Date              Modified By               Remarks          
 *********************************************************************************************
 2.110.0      08-Dec-2020       Prajwal S                 Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System;
using Newtonsoft.Json;
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Semnox.Parafait.Game
{
    public class RemoteReaderConfigurationUseCases : RemoteUseCases, IReaderConfigurationUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string MachineAttribute_URL = "api/Game/ReaderConfigs";
        private const string MachineAttribute_CONTAINER_URL = "api/Game/ReaderConfigurationContainer";

        public RemoteReaderConfigurationUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<List<MachineAttributeDTO>> GetMachineAttributes(List<KeyValuePair<string, string>> parameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parameters);
            try
            {
            List<MachineAttributeDTO> result = await Get<List<MachineAttributeDTO>>(MachineAttribute_URL, parameters);
            log.LogMethodExit(result);
            return result;
            }
            catch (Exception ex)
            {
             log.Error(ex);
             throw ex;
            }
            
        }

        public async Task<string> SaveMachineAttributes(List<MachineAttributeDTO> machineAttributeDTOList, string moduleName, string moduleId)
        {
            log.LogMethodEntry(machineAttributeDTOList, moduleName, moduleId);
            try
            {
                string responseString = await Post<string>(MachineAttribute_URL + "/?moduleName=" + moduleName + "&moduleId=" + moduleId, machineAttributeDTOList);
                log.LogMethodExit(responseString);;
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<ReaderConfigurationContainerDTOCollection> GetMachineAttributeContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(hash, rebuildCache);
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("siteId", siteId.ToString()));
            if (string.IsNullOrWhiteSpace(hash) == false)
            {
                parameters.Add(new KeyValuePair<string, string>("hash", hash));
            }
            parameters.Add(new KeyValuePair<string, string>("rebuildCache", rebuildCache.ToString()));
            ReaderConfigurationContainerDTOCollection result = await Get<ReaderConfigurationContainerDTOCollection>(MachineAttribute_CONTAINER_URL, parameters);
            log.LogMethodExit(result);
            return result;
        }

        public async Task<MachineAttributeDTO> DeleteMachineAttributes(MachineAttributeDTO machineAttributeDTO, string entityName, string entityId)
        {
            log.LogMethodEntry(entityName, entityId);
            try
            {
                MachineAttributeDTO responseString = await Delete<MachineAttributeDTO>(MachineAttribute_URL + "/?entityName=" + entityName + "&entityId=" + entityId, machineAttributeDTO);
                log.LogMethodExit(responseString); ;
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
    }
}

        