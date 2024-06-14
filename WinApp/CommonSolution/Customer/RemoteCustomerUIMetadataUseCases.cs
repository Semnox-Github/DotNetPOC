/********************************************************************************************
* Project Name - Customer
* Description  - Specification of the RemoteCustomerUIMetadataUseCases use cases. 
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.120.00   09-Jul-2021   Roshan Devadiga        Created 
********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Customer
{
    class RemoteCustomerUIMetadataUseCases : RemoteUseCases, ICustomerUIMetadataUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string CUSTOMER_UI_METADATA_URL = "api/Customer/CustomerUIMetadata";
        private const string CUSTOMER_UI_METADATA_CONTAINER_URL = "api/Customer/CustomerUIMetadataContainer";
        public RemoteCustomerUIMetadataUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }
        public async Task<List<CustomerUIMetadataDTO>> GetCustomerUIMetadatas(int siteId)
        {
            log.LogMethodEntry(siteId);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            try
            {
                List<CustomerUIMetadataDTO> result = await Get<List<CustomerUIMetadataDTO>>(CUSTOMER_UI_METADATA_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        public async Task<CustomerUIMetadataContainerDTOCollection> GetCustomerUIMetadataContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(hash, rebuildCache);
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("siteId", siteId.ToString()));
            if (string.IsNullOrWhiteSpace(hash) == false)
            {
                parameters.Add(new KeyValuePair<string, string>("hash", hash));
            }
            parameters.Add(new KeyValuePair<string, string>("rebuildCache", rebuildCache.ToString()));
            CustomerUIMetadataContainerDTOCollection result = await Get<CustomerUIMetadataContainerDTOCollection>(CUSTOMER_UI_METADATA_CONTAINER_URL, parameters);
            log.LogMethodExit(result);
            return result;
        }
    }
}
