/********************************************************************************************
 * Project Name - Gamea 
 * Description  - LocalMachineDataService class to get the data  from local DB 
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
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Game
{
    public class LocalMachineDataService : IMachineDataService
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalMachineDataService(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public List<MachineDTO> GetMachines(List<KeyValuePair<MachineDTO.SearchByMachineParameters, string>> parameters , bool loadChildRecords = false)
        {
            log.LogMethodEntry(parameters);
            MachineList machineList = new MachineList(executionContext);
            int siteId = GetSiteId();
            List<MachineDTO> machineDTOList = machineList.GetMachineList(parameters, loadChildRecords);
            log.LogMethodExit(machineDTOList);
            return machineDTOList;
        }

        public string PostMachines(List<MachineDTO> machineDTOList)
        {
            string result = string.Empty;
            try
            {
                log.LogMethodEntry(machineDTOList);
                MachineList machineList = new MachineList(executionContext, machineDTOList);
                machineList.SaveUpdateMachinesList();
                result = "Success";
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            log.LogMethodExit(result);
            return result;
        }

        public string DeleteMachines(List<MachineDTO> machineDTOList)
        {
            string result = string.Empty;
            try
            {
                log.LogMethodEntry(machineDTOList);
                MachineList machineList = new MachineList(executionContext, machineDTOList);
                machineList.DeleteMachineList();
                result = "Success";
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
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
