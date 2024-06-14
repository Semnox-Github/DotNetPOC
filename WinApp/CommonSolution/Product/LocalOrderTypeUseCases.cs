/********************************************************************************************
 * Project Name - Product
 * Description  - LocalOrderTypeUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date              Modified By        Remarks          
 *********************************************************************************************
 2.130.0      19-Jul-2021       Mushahid Faizan    Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Product
{
    public class LocalOrderTypeUseCases :  IOrderTypeUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        /// <summary>
        /// LocalOrderTypeUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        public LocalOrderTypeUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// GetOrderType
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        public async Task<List<OrderTypeDTO>> GetOrderType(List<KeyValuePair<OrderTypeDTO.SearchByParameters, string>> searchParameters)
        {
            return await Task<List<OrderTypeDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);
                OrderTypeListBL orderTypeListBL = new OrderTypeListBL(executionContext);
                List<OrderTypeDTO> orderTypeDTOList = orderTypeListBL.GetOrderTypeDTOList(searchParameters);
                log.LogMethodExit(orderTypeDTOList);
                return orderTypeDTOList;
            });
        }


        public async Task<OrderTypeContainerDTOCollection> GetOrderTypeContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            return await Task<OrderTypeContainerDTOCollection>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(siteId, hash, rebuildCache);
                if (rebuildCache)
                {
                    OrderTypeContainerList.Rebuild(siteId);
                }
                OrderTypeContainerDTOCollection result = OrderTypeContainerList.GetOrderTypeContainerDTOCollection(siteId);
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
