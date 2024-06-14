/********************************************************************************************
 * Project Name - Product Special Prices BL
 * Description  - Bussiness logic of Product Special Prices
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70        18-Feb-2019   Indrajeet Kumar         Created 
 *            22-Mar-2019   Nagesh Badiger          modify: removed default constructor and added log method entry and method exit
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Product
{
    public class ProductSpecialPricesBL
    {
        private ProductSpecialPricesDTO productSpecialPricesDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Parameterized Coonstructor
        /// </summary>
        /// <param name="executionContext"></param>
        public ProductSpecialPricesBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.productSpecialPricesDTO = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="productSpecialPricesDTO"></param>
        /// <param name="executionContext"></param>
        public ProductSpecialPricesBL(ProductSpecialPricesDTO productSpecialPricesDTO, ExecutionContext executionContext)
        {
            log.LogMethodEntry(productSpecialPricesDTO, executionContext);
            this.executionContext = executionContext;
            this.productSpecialPricesDTO = productSpecialPricesDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// save and updates the record 
        /// </summary>
        public void Save(SqlTransaction sqlTransaction=null)
        {
            log.LogMethodEntry();
            ProductSpecialPricesDataHandler productSpecialPricesDataHandler = new ProductSpecialPricesDataHandler(sqlTransaction);
            if (productSpecialPricesDTO.ChangePrice == null)
            {
                int? rowDeleted = productSpecialPricesDataHandler.DeleteProductSpecialPrices(productSpecialPricesDTO.SpecialPricingId, productSpecialPricesDTO.ProductId);
            }
            else
            {
                int rowUpdated = productSpecialPricesDataHandler.UpdateProductSpecialPrices(productSpecialPricesDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                productSpecialPricesDTO.AcceptChanges();
                if (rowUpdated == 0)
                {
                    int specialPricingId = productSpecialPricesDataHandler.InsertProductSpecialPrices(productSpecialPricesDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    productSpecialPricesDTO.SpecialPricingId = specialPricingId;
                }
            }
            log.LogMethodExit();
        }
    }

    public class ProductSpecialPricesBLList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<ProductSpecialPricesDTO> productSpecialPricesList;

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public ProductSpecialPricesBLList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.productSpecialPricesList = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="productSpecialPricesList"></param>
        /// <param name="executionContext"></param>
        public ProductSpecialPricesBLList(List<ProductSpecialPricesDTO> productSpecialPricesList, ExecutionContext executionContext)
        {
            log.LogMethodEntry(productSpecialPricesList, executionContext);
            this.executionContext = executionContext;
            this.productSpecialPricesList = productSpecialPricesList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get GetProductSpecialPricesInfo method based on specialPricingId percentage 
        /// </summary>
        /// <param name="specialPricingId"></param>
        /// <param name="percentage"></param>
        /// <returns></returns>
        public List<ProductSpecialPricesDTO> GetProductSpecialPricesInfo(int specialPricingId, decimal percentage,int siteId)
        {
            log.LogMethodEntry(specialPricingId, percentage, siteId);
            ProductSpecialPricesDataHandler productSpecialPricesDataHandler = new ProductSpecialPricesDataHandler(null);
            log.LogMethodExit();
            return productSpecialPricesDataHandler.GetProductSpecialPrices(specialPricingId, percentage, siteId);
        }

    }
}
