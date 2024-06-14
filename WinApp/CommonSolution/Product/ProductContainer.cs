/********************************************************************************************
* Project Name - Inventory 
* Description  - Container Class for Product to get all the Product DTO list 
* 
**************
**Version Log
**************
*Version     Date          Modified By         Remarks          
*********************************************************************************************
*2.100.0    17-Sep-20      Deeksha              Created 
********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace Semnox.Parafait.Product
{
    public class ProductContainer
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static List<ProductDTO> productDTOList;
        private DateTime? productModuleLastUpdateTime;
        private DateTime? refreshTime;
        private readonly ExecutionContext executionContext;
        private readonly Timer refreshTimer;

        public ProductContainer(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            if(productDTOList == null || productDTOList.Count < 0)
            {
                productDTOList = GetAllProductDTOList();
            }
            refreshTimer = new Timer(5 * 60 * 1000);
            refreshTimer.Elapsed += OnRefreshTimer;
            RefreshProductDTOList();
            refreshTimer.Start();
            productModuleLastUpdateTime = GetLastUpdatedDate();
            log.LogMethodExit();
        }

        private DateTime? GetLastUpdatedDate()
        {
            log.LogMethodEntry();
            ProductList productListBL = new ProductList(executionContext);
            DateTime? updateTime = productListBL.GetProductModuleLastUpdateTime(executionContext.GetSiteId());
            log.LogMethodExit(updateTime);
            return updateTime;
        }

        private void OnRefreshTimer(object sender, ElapsedEventArgs e)
        {
            log.LogMethodEntry();
            RefreshProductDTOList();
            log.LogMethodExit();
        }

        /// <summary>
        /// Method to Get All ProductDTO's List
        /// </summary>
        /// <returns></returns>
        public List<ProductDTO> GetAllProductDTOList()
        {
            log.LogMethodEntry();
            ProductList productBL = new ProductList(executionContext);
            List<KeyValuePair<ProductDTO.SearchByProductParameters, string>> searchParameters = new List<KeyValuePair<ProductDTO.SearchByProductParameters, string>>();
            searchParameters.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.IS_ACTIVE, "Y"));
            searchParameters.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            List<ProductDTO> productDTOList = productBL.GetAllProducts(searchParameters, true, true);
            log.LogMethodExit(productDTOList);
            return productDTOList;
        }

        /// <summary>
        /// Method to refresh the UOM List
        /// </summary>
        private void RefreshProductDTOList()
        {
            log.LogMethodEntry();
            if (refreshTime.HasValue && refreshTime > DateTime.UtcNow)
            {
                log.LogMethodExit(null, "Refreshed the list in last 5 minutes.");
                return;
            }

            refreshTime = DateTime.UtcNow.AddMinutes(5);
            ProductList productListBL = new ProductList(executionContext);
            DateTime? updateTime = productListBL.GetProductModuleLastUpdateTime(executionContext.GetSiteId());
            if (updateTime.HasValue && productModuleLastUpdateTime.HasValue
                && productModuleLastUpdateTime >= updateTime)
            {
                log.LogMethodExit(null, "No changes in Product module since " + productModuleLastUpdateTime);
                return;
            }
            productModuleLastUpdateTime = updateTime;
            productDTOList = GetAllProductDTOList();
            log.LogMethodExit();
        }
    }
}
