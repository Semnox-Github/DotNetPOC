/********************************************************************************************
* Project Name - Customer
* Description  - Specification of the LocalCustomerUIMetadataUseCases use cases. 
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
    public class LocalCustomerUIMetadataUseCases:ICustomerUIMetadataUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ExecutionContext executionContext;
        public LocalCustomerUIMetadataUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<CustomerUIMetadataDTO>> GetCustomerUIMetadatas(int siteId)
        {
            return await Task<List<CustomerUIMetadataDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(siteId);
                CustomerUIMetadataBL customerUIMetadataBL = new CustomerUIMetadataBL(executionContext);
                List<CustomerUIMetadataDTO> customerUIMetadataDTOList = customerUIMetadataBL.GetCustomerUIMetadataDTOList(siteId);

                log.LogMethodExit(customerUIMetadataDTOList);
                return customerUIMetadataDTOList;
            });
        }
        public async Task<CustomerUIMetadataContainerDTOCollection> GetCustomerUIMetadataContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            return await Task<CustomerUIMetadataContainerDTOCollection>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(siteId, hash, rebuildCache);
                if (rebuildCache)
                {
                    CustomerUIMetadataContainerList.Rebuild(siteId);
                }
                List<CustomerUIMetadataContainerDTO> customerUIMetadataContainerDTOList = CustomerUIMetadataContainerList.GetCustomerUIMetadataContainerDTOList(siteId);
                CustomerUIMetadataContainerDTOCollection result = new CustomerUIMetadataContainerDTOCollection(customerUIMetadataContainerDTOList);
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
