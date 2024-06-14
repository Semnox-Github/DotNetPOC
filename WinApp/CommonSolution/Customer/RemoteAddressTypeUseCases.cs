/********************************************************************************************
* Project Name - Customer
* Description  - Specification of the RemoteAddressTypeUseCases use cases. 
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.120.00   08-Jul-2021   Roshan Devadiga        Created 
********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Customer
{
    class RemoteAddressTypeUseCases : RemoteUseCases, IAddressTypeUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string ADDRESSTYPE_URL = "api/Customer/AddressTypes";
        private const string ADDRESSTYPE_CONTAINER_URL = "api/Customer/AddressTypesContainer";
        public RemoteAddressTypeUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }
        public async Task<List<AddressTypeDTO>> GetAddressTypes(List<KeyValuePair<AddressTypeDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            try
            {
                List<AddressTypeDTO> result = await Get<List<AddressTypeDTO>>(ADDRESSTYPE_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        public async Task<AddressTypeContainerDTOCollection> GetAddressTypeContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(hash, rebuildCache);
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("siteId", siteId.ToString()));
            if (string.IsNullOrWhiteSpace(hash) == false)
            {
                parameters.Add(new KeyValuePair<string, string>("hash", hash));
            }
            parameters.Add(new KeyValuePair<string, string>("rebuildCache", rebuildCache.ToString()));
            AddressTypeContainerDTOCollection result = await Get<AddressTypeContainerDTOCollection>(ADDRESSTYPE_CONTAINER_URL, parameters);
            log.LogMethodExit(result);
            return result;
        }

    }
}
