/********************************************************************************************
 * Project Name - Generic Utilities
 * Description  - Remote proxy to the actual implementation of MIFARE Key entity use cases .
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
using Newtonsoft.Json;
using Semnox.Core.Utilities;

namespace Semnox.Core.GenericUtilities
{
    public class RemoteMifareKeyUseCases : RemoteUseCases, IMifareKeyUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string MIFARE_KEY_CONTAINER_URL = "api/Configuration/MifareKeyContainer";

        public RemoteMifareKeyUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }
        public async Task<MifareKeyContainerDTOCollection> GetMifareKeyContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(siteId, hash, rebuildCache);

            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("siteId", siteId.ToString()));
            if (string.IsNullOrWhiteSpace(hash) == false)
            {
                parameters.Add(new KeyValuePair<string, string>("hash", hash));
            }
            parameters.Add(new KeyValuePair<string, string>("rebuildCache", rebuildCache.ToString()));
            MifareKeyContainerDTOCollection result = await Get<MifareKeyContainerDTOCollection>(MIFARE_KEY_CONTAINER_URL, parameters); ;
            return result;
        }
    }
}
