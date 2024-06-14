/********************************************************************************************
 * Project Name - Product
 * Description  - LocalProductTypeUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
*2.140.00   14-Sep-2021        Prajwal S           Modified
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    class LocalProductTypeUseCases : IProductTypeUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalProductTypeUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }


        public async Task<List<ProductTypeDTO>> GetProductType(List<KeyValuePair<ProductTypeDTO.SearchByParameters, string>>
                        searchParameters, SqlTransaction sqlTransaction = null
                       )
        {
            return await Task<List<ProductTypeDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                ProductTypeListBL productTypeListBL = new ProductTypeListBL(executionContext);
                List<ProductTypeDTO> productTypeDTOList = productTypeListBL.GetProductTypeDTOList(searchParameters, sqlTransaction);

                log.LogMethodExit(productTypeDTOList);
                return productTypeDTOList;
            });
        }
        public async Task<ProductTypeContainerDTOCollection> GetProductTypeContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            return await Task<ProductTypeContainerDTOCollection>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(siteId, hash, rebuildCache);
                if (rebuildCache)
                {
                    ProductTypeContainerList.Rebuild(siteId);
                }
                ProductTypeContainerDTOCollection result = ProductTypeContainerList.GetProductTypeContainerDTOCollection(siteId);
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
