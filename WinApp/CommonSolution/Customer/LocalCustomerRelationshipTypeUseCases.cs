/********************************************************************************************
 * Project Name - Customer
 * Description  - LocalCustomerRelationshipTypeUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date              Modified By        Remarks          
 *********************************************************************************************
 2.130.0      31-Aug-2021       Mushahid Faizan    Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Customer
{
    public class LocalCustomerRelationshipTypeUseCases : ICustomerRelationshipTypeUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        /// <summary>
        /// LocalCustomerRelationshipTypeUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        public LocalCustomerRelationshipTypeUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// GetCustomerRelationshipType
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        public async Task<List<CustomerRelationshipTypeDTO>> GetCustomerRelationshipType(List<KeyValuePair<CustomerRelationshipTypeDTO.SearchByParameters, string>> searchParameters)
        {
            return await Task<List<CustomerRelationshipTypeDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);
                CustomerRelationshipTypeListBL customerRelationshipTypeListBL = new CustomerRelationshipTypeListBL(executionContext);
                List<CustomerRelationshipTypeDTO> customerRelationshipTypeDTOList = customerRelationshipTypeListBL.GetCustomerRelationshipTypeDTOList(searchParameters);
                log.LogMethodExit(customerRelationshipTypeDTOList);
                return customerRelationshipTypeDTOList;
            });
        }


        public async Task<CustomerRelationshipTypeContainerDTOCollection> GetCustomerRelationshipTypeContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            return await Task<CustomerRelationshipTypeContainerDTOCollection>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(siteId, hash, rebuildCache);
                if (rebuildCache)
                {
                    CustomerRelationshipTypeContainerList.Rebuild(siteId);
                }
                CustomerRelationshipTypeContainerDTOCollection result = CustomerRelationshipTypeContainerList.GetCustomerRelationshipTypeContainerDTOCollection(siteId);
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
