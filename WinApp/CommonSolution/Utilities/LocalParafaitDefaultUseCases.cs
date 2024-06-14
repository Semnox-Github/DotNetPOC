/********************************************************************************************
 * Project Name - Utilities 
 * Description  - LocalParafaitDefaultDataService class to get the data  from local DB 
 * 
 *   
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         24-Aug-2020       Girish Kundar             Created : POS UI Redesign with REST API
 ********************************************************************************************/

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Semnox.Core.Utilities
{
    public class LocalParafaitDefaultUseCases : LocalUseCases, IParafaitDefaultUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public LocalParafaitDefaultUseCases(ExecutionContext executionContext)
            :base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<ParafaitDefaultContainerDTOCollection> GetParafaitDefaultContainerDTOCollection(int siteId, int userPkId, int machineId, string hash, bool rebuildCache)
        {
            return await Task<ParafaitDefaultContainerDTOCollection>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(hash, rebuildCache);
                if (rebuildCache)
                {
                    ParafaitDefaultContainerList.Rebuild(siteId);
                }
                ParafaitDefaultContainerDTOCollection result = ParafaitDefaultContainerList.GetParafaitDefaultContainerDTOCollection(siteId, userPkId, machineId);
                if (hash == result.Hash)
                {
                    log.LogMethodExit(null, "No changes to the cache");
                    return null;
                }
                log.LogMethodExit(result);
                return result;
            });

        }

    }
}
