/********************************************************************************************
 * Project Name - Generic Utilities
 * Description  - Actual implementation of MIFARE Key entity use cases.
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
using Semnox.Core.Utilities;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// Actual implementation of MIFARE Key entity use cases.
    /// </summary>
    public class LocalMifareKeyUseCases : IMifareKeyUseCases
    {
        private static readonly Parafait.logging.Logger log = new Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public LocalMifareKeyUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the MifareKeyContainerDTO Collection for the given siteId and hash. If Hash is same null is returned as there
        /// is no difference between server and client data
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="hash"></param>
        /// <param name="rebuildCache"></param>
        /// <returns></returns>
        public async Task<MifareKeyContainerDTOCollection> GetMifareKeyContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            return await Task<MifareKeyContainerDTOCollection>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(siteId, hash, rebuildCache);
            if (rebuildCache)
            {
                MifareKeyContainerList.Rebuild(siteId);
            }
            MifareKeyContainerDTOCollection result = MifareKeyContainerList.GetMifareKeyContainerDTOCollection(siteId);
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
