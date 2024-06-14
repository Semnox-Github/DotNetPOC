/********************************************************************************************
 * Project Name - Communication
 * Description  - Proxy class to remote implementation of the message entity uses cases 
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

namespace Semnox.Parafait.Languages
{
    public class RemoteMessageUseCases : RemoteUseCases, IMessageUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string MESSAGE_URL = "api/Communication/Messages";
        private const string MESSAGE_CONTAINER_URL = "api/Communication/MessageContainer";

        public RemoteMessageUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<MessageContainerDTOCollection> GetMessageContainerDTOCollection(int siteId, int languageId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(siteId, languageId, hash, rebuildCache);
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("siteId", siteId.ToString()));
            parameters.Add(new KeyValuePair<string, string>("languageId", languageId.ToString()));
            if (string.IsNullOrWhiteSpace(hash) == false)
            {
                parameters.Add(new KeyValuePair<string, string>("hash", hash));
            }
            parameters.Add(new KeyValuePair<string, string>("rebuildCache", rebuildCache.ToString()));
           MessageContainerDTOCollection result = await Get<MessageContainerDTOCollection>(MESSAGE_CONTAINER_URL, parameters);
            log.LogMethodExit(result);
            return result;
        }
    }
}
