/********************************************************************************************
 * Project Name - Product
 * Description  - LocalProductActivitiesUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0     31-Dec-2020         Abhishek                Created 
 ********************************************************************************************/
using System;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    public class LocalProductActivityUseCases : LocalUseCases, IProductActivityUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        public LocalProductActivityUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<ProductActivityViewDTO>> GetProductActivities(int locationId, int productId, int lotId, int currentPage = 0, int pageSize = 10, SqlTransaction sqlTransaction = null)
        {
            return await Task<List<ProductActivityViewDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(locationId, productId, lotId, currentPage, pageSize);
                ProductActivityViewList productActivityViewListBL = new ProductActivityViewList(executionContext);
                List<ProductActivityViewDTO> productActivityViewDTOList = productActivityViewListBL.GetAllProductActivity(locationId, productId, lotId, currentPage, pageSize, sqlTransaction);
                log.LogMethodExit(productActivityViewDTOList);
                return productActivityViewDTOList;
            });
        }

        public async Task<int> GetProductActivityCount(int locationId, int productId,  SqlTransaction sqlTransaction = null)
        {
            return await Task<int>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(locationId, productId);
                ProductActivityViewList productActivityViewListBL = new ProductActivityViewList(executionContext);
                int productActivityCount = productActivityViewListBL.GetProductActivityCount(locationId, productId, -1, sqlTransaction);
                log.LogMethodExit(productActivityCount);
                return productActivityCount;
            });
        }
    }
}
