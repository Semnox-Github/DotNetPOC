/********************************************************************************************
 * Project Name - Inventory                                                                          
 * Description  - Product Activity View List BL
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.100.0     12-Aug-2020   Deeksha        Modified for Recipe Management enhancement.
 *2.110.00    30-Nov-2020   Abhishek        Modified : Modified to 3 Tier Standard 
 *2.110.00    31-Dec-2020   Abhishek        Modified : Modified for web API changes  
 ********************************************************************************************/
using System;
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Semnox.Parafait.Product
{
    public class ProductActivityViewList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ProductActivityViewList(ExecutionContext executionContext)
        {
            log.LogMethodEntry();
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the product list
        /// </summary>
        public List<ProductActivityViewDTO> GetProductActivity(int locationId, int productId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(locationId, productId, sqlTransaction);
            ProductActivityViewDataHandler productActivityViewDataHandler = new ProductActivityViewDataHandler(sqlTransaction);
            List<ProductActivityViewDTO> productActivityViewDTOList = productActivityViewDataHandler.GetProductActivity(locationId, productId);
            //SiteContainerList.FromSiteDateTime(productActivityViewDTOList);
            log.LogMethodExit(productActivityViewDTOList);
            return productActivityViewDTOList;
        }

        /// <summary>
        /// Returns the product list
        /// </summary>
        public List<ProductActivityViewDTO> GetAllProductActivity(int locationId, int productId, int lotId, int currentPage = 0, int pageSize = 10, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(locationId, productId, lotId, sqlTransaction);
            ProductActivityViewDataHandler productActivityViewDataHandler = new ProductActivityViewDataHandler(sqlTransaction);
            List<ProductActivityViewDTO> productActivityViewDTOList = productActivityViewDataHandler.GetProductActivityViewDTOList(locationId, productId, lotId, currentPage, pageSize, sqlTransaction);
            //SiteContainerList.FromSiteDateTime(productActivityViewDTOList);
            log.LogMethodExit(productActivityViewDTOList);
            return productActivityViewDTOList;
        }

        /// <summary>
        /// Returns the no of Product Activity matching the search Parameters
        /// </summary>
        /// <param name="locationId"> locationId</param>
        /// <param name="productId"> productId</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        /// <returns></returns>
        public int GetProductActivityCount(int locationId, int productId, int lotId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(locationId, productId, sqlTransaction);
            ProductActivityViewDataHandler productActivityViewDataHandler = new ProductActivityViewDataHandler(sqlTransaction);
            int inventoryWastagesCount = productActivityViewDataHandler.GetProductActivityCount(locationId, productId, lotId, sqlTransaction);
            log.LogMethodExit(inventoryWastagesCount);
            return inventoryWastagesCount;
        }
    }
}
