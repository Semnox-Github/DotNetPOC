/********************************************************************************************
 * Project Name - Utilities 
 * Description  - LocalSystemOptionDataService class to get the data  from local DB 
 * 
 *   
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0      17-Nov-2020      Lakshminarayana           Created : POS UI Redesign with REST API
 ********************************************************************************************/

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Semnox.Core.Utilities
{
    public class LocalSystemOptionUseCases : LocalUseCases, ISystemOptionUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public LocalSystemOptionUseCases(ExecutionContext executionContext) 
            :base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<SystemOptionContainerDTOCollection> GetSystemOptionContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            return await Task<SystemOptionContainerDTOCollection>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(hash, rebuildCache);
                if (rebuildCache)
                {
                    SystemOptionContainerList.Rebuild(siteId);
                }
                SystemOptionContainerDTOCollection result = SystemOptionContainerList.GetSystemOptionContainerDTOCollection(siteId);
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
