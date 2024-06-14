/********************************************************************************************
 * Project Name - ProductSubscriptionUseCases
 * Description  - ProductSubscriptionUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By           Remarks          
 *********************************************************************************************
 2.110.0      25-Jan-2021      Guru S A              For Subscription changes
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// LocalProductSubscriptionUseCases
    /// </summary>
    public class LocalProductSubscriptionUseCases : IProductSubscriptionUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        /// <summary>
        /// LocalProductSubscriptionUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        public LocalProductSubscriptionUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// GetProductSubscription
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        public async Task<List<ProductSubscriptionDTO>> GetProductSubscription(List<KeyValuePair<ProductSubscriptionDTO.SearchByParameters, string>> searchParameters)
        {
            return await Task<List<ProductSubscriptionDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);
                ProductSubscriptionListBL productSubscriptionListBL = new ProductSubscriptionListBL(executionContext);
                List<ProductSubscriptionDTO> productSubscriptionDTOList = productSubscriptionListBL.GetProductSubscriptionDTOList(searchParameters);
                log.LogMethodExit(productSubscriptionDTOList);
                return productSubscriptionDTOList;
            });
        }
        /// <summary>
        /// SaveProductSubscription
        /// </summary>
        /// <param name="productSubscriptionDTOList"></param>
        /// <returns></returns>
        public async Task<string> SaveProductSubscription(List<ProductSubscriptionDTO> productSubscriptionDTOList)
        {
            log.LogMethodEntry(productSubscriptionDTOList);
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    if (productSubscriptionDTOList == null)
                    {
                        throw new ValidationException("productsDTOList is empty");
                    }

                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    { 
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            ProductSubscriptionListBL productSubscriptionListBL = new ProductSubscriptionListBL(executionContext, productSubscriptionDTOList);
                            productSubscriptionListBL.Save(parafaitDBTrx.SQLTrx);
                            parafaitDBTrx.EndTransaction();
                        }
                        catch (ValidationException valEx)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(valEx);
                            throw valEx;
                        }
                        catch (Exception ex)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            throw new Exception(ex.Message, ex);
                        }
                    }
                    result = "Success";
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    throw ex;
                }
                log.LogMethodExit(result);
                return result;
            });
        }
         
    }
}
