/********************************************************************************************
 * Project Name - LocalReaderConfigurationContainerDataService  Class
 * Description  - LocalReaderConfigurationContainerDataService class to get the data  from local DB 
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
    public class LocalReaderConfigurationContainerDataService : IReaderConfigurationContainerDataService
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalReaderConfigurationContainerDataService(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public List<MachineAttributeDTO> Get(DateTime? maxLastUpdatedDate, string hash)
        {
            log.LogMethodEntry(maxLastUpdatedDate, hash);
            MachineAttributeListBL machineAttributeListBL = new MachineAttributeListBL(executionContext);
            int siteId = GetSiteId();
            DateTime? updateTime = machineAttributeListBL.GetAttributeModuleLastUpdateTime(siteId);
            DateTime updateTimeutc;
            if (updateTime.HasValue)
            {
                updateTimeutc = (DateTime)updateTime;
                if (maxLastUpdatedDate.HasValue
                && maxLastUpdatedDate >= updateTimeutc.ToUniversalTime())
                {
                    log.LogMethodExit(null, "No changes in Game module since " + maxLastUpdatedDate);
                    return null;
                }
            }
            machineAttributeListBL = new MachineAttributeListBL(executionContext);
            List<KeyValuePair<MachineAttributeDTO.SearchByParameters, string>> searchByParameters = new List<KeyValuePair<MachineAttributeDTO.SearchByParameters, string>>();
            searchByParameters.Add(new KeyValuePair<MachineAttributeDTO.SearchByParameters, string>(MachineAttributeDTO.SearchByParameters.SITE_ID, Convert.ToString(executionContext.GetSiteId())));
            searchByParameters.Add(new KeyValuePair<MachineAttributeDTO.SearchByParameters, string>(MachineAttributeDTO.SearchByParameters.IS_ACTIVE, "1" ));
            List<MachineAttributeDTO> machineAttributeDTOList = machineAttributeListBL.GetMachineAttributes(searchByParameters,MachineAttributeDTO.AttributeContext.SYSTEM);
            log.LogMethodExit(machineAttributeDTOList);
            return machineAttributeDTOList;
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
