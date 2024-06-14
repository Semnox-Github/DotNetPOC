/********************************************************************************************
* Project Name - Customer
* Description  - Specification of the LocalAddressTypeUseCases use cases. 
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
   public  class LocalAddressTypeUseCases:IAddressTypeUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ExecutionContext executionContext;
        public LocalAddressTypeUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<AddressTypeDTO>> GetAddressTypes(List<KeyValuePair<AddressTypeDTO.SearchByParameters, string>> searchParameters)
        {
            return await Task<List<AddressTypeDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                AddressTypeListBL addressTypeListBL = new AddressTypeListBL(executionContext);
                List<AddressTypeDTO> addressTypeDTOList = addressTypeListBL.GetAddressTypeDTOList(searchParameters);

                log.LogMethodExit(addressTypeDTOList);
                return addressTypeDTOList;
            });
        }
        public async Task<AddressTypeContainerDTOCollection> GetAddressTypeContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            return await Task<AddressTypeContainerDTOCollection>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(siteId, hash, rebuildCache);
                if (rebuildCache)
                {
                    AddressTypeContainerList.Rebuild(siteId);
                }
                List<AddressTypeContainerDTO> addressTypeContainerDTOList = AddressTypeContainerList.GetAddressTypeContainerDTOList(siteId);
                AddressTypeContainerDTOCollection result = new AddressTypeContainerDTOCollection(addressTypeContainerDTOList);
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
