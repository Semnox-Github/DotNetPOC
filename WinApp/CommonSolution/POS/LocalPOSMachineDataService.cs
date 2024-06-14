/********************************************************************************************
 * Project Name - POS 
 * Description  - LocalPOSMachineDataService class to get the data  from local DB 
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

namespace Semnox.Parafait.POS
{
    public class LocalPOSMachineDataService : IPOSMachineDataService
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalPOSMachineDataService(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public List<POSMachineDTO> GetPOSMachines(List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>> parameters , bool loadChildRecords = false)
        {
            log.LogMethodEntry(parameters);
            POSMachineList pOSMachineList = new POSMachineList(executionContext);
            List<POSMachineDTO> posMachineDTOList = pOSMachineList.GetAllPOSMachines(parameters,loadChildRecords,true);
            log.LogMethodExit(posMachineDTOList);
            return posMachineDTOList;
        }

        public string PostPOSMachines(List<POSMachineDTO> posMachineDTOList)
        {
            string result = string.Empty;
            try
            {
                log.LogMethodEntry(posMachineDTOList);
                POSMachineList pOSMachineList = new POSMachineList(executionContext, posMachineDTOList);
                pOSMachineList.Save();
                result = "Success";
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
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
