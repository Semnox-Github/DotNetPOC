/********************************************************************************************
 * Project Name - Communication
 * Description  - Implementation of the message entity uses cases 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0      17-Nov-2020      Lakshminarayana           Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Languages
{
    public class LocalMessageUseCases : IMessageUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalMessageUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        public async Task<MessageContainerDTOCollection> GetMessageContainerDTOCollection(int siteId, int languageId, string hash, bool rebuildCache)
        {
            return await Task<MessageContainerDTOCollection>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(siteId, languageId, hash, rebuildCache);
                if (rebuildCache)
                {
                    MessageContainerList.Rebuild(siteId);
                }
                MessageContainerDTOCollection result = MessageContainerList.GetMessageContainerDTOCollection(siteId, languageId);
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
