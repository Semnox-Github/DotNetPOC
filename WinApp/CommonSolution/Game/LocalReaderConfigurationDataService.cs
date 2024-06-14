/********************************************************************************************
 * Project Name - Gamea 
 * Description  - LocalHubDataService class to get the data  from local DB 
 * 
 *   
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         24-Aug-2020       Girish Kundar             Created : POS UI Redesign with REST API
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Game
{
    public class LocalReaderConfigurationDataService : IReaderConfigurationDataService
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalReaderConfigurationDataService(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public List<MachineAttributeDTO> GetReaderConfigurations(List<KeyValuePair<string, string>> parameters)
        {
            log.LogMethodEntry(parameters);
            int siteId = GetSiteId();
            string moduleName = string.Empty;
            int moduleRowId = -1;
            if(parameters != null && parameters.Exists(x=>x.Key =="moduleName"))
            {
                moduleName = parameters.Find(x=>x.Key =="moduleName").Value.ToString();
                moduleRowId = Convert.ToInt32(parameters.Find(x=>x.Key == "moduleRowId").Value);
            }
            GameSystem gameSystem = new GameSystem(moduleName, Convert.ToInt32(moduleRowId), executionContext);
            List<MachineAttributeDTO> machineAttributeDTOList = gameSystem.GetMachineAttributes();
            log.LogMethodExit(machineAttributeDTOList);
            return machineAttributeDTOList;
        }

        public string PostReaderConfigurations(List<MachineAttributeDTO> machineAttributeDTOList, string moduleName, string moduleId)
        {
            string result = string.Empty;
            try
            {
                log.LogMethodEntry(machineAttributeDTOList);
                GameSystemList gameSystem = new GameSystemList(executionContext, machineAttributeDTOList, moduleName, Convert.ToInt32(moduleId));
                gameSystem.SaveGameSystemList();
                result = "Success";
            }
            catch (Exception ex)
            {
                log.Error(ex);
                result = "Falied";
            }
            log.LogMethodExit(result);
            return result;
        }

        private int GetSiteId()
        {
            log.LogMethodEntry();
            int siteId = -1;
            if (executionContext.GetIsCorporate())
            {
                siteId = executionContext.GetSiteId();
            }
            log.LogMethodExit(siteId);
            return siteId;
        }
    }
}
