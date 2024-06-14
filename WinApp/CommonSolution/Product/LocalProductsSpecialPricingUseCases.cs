/********************************************************************************************
 * Project Name - Product
 * Description  - LocalProductsSpecialPricingUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.00    09-Mar-2021       Roshan Devadiga        Created : POS UI Redesign with REST API
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// Implementation of productsSpecialPricing use-cases
    /// </summary>
   public class LocalProductsSpecialPricingUseCases: IProductsSpecialPricingUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ExecutionContext executionContext;
        public LocalProductsSpecialPricingUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<ProductsSpecialPricingDTO>> GetProductsSpecialPricings(List<KeyValuePair<ProductsSpecialPricingDTO.SearchByProductsSpecialPricingParameters, string>>
                         searchParameters)
        {
            return await Task<List<ProductsSpecialPricingDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                ProductsSpecialPricingListBL productsSpecialPricingListBL = new ProductsSpecialPricingListBL(executionContext);
                List<ProductsSpecialPricingDTO> productsSpecialPricingDTOList = productsSpecialPricingListBL.GetAllProductsSpecialPricing(searchParameters);

                log.LogMethodExit(productsSpecialPricingDTOList);
                return productsSpecialPricingDTOList;
            });
        }
      
        public async Task<string> SaveProductsSpecialPricings(List<ProductsSpecialPricingDTO> productsSpecialPricingList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(productsSpecialPricingList);
                    if (productsSpecialPricingList == null)
                    {
                        throw new ValidationException("productsSpecialPricingList is Empty");
                    }
                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            ProductsSpecialPricingListBL productsSpecialPricingListBL = new ProductsSpecialPricingListBL(executionContext, productsSpecialPricingList);
                            productsSpecialPricingListBL.SaveUpdateProductsSpecialPricingList();
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
                            throw ex;
                        }
                    }
                    result = "Success";
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    result = "Falied";
                    throw;
                }
                log.LogMethodExit(result);
                return result;
            });
        }
        public async Task<string> Delete(List<ProductsSpecialPricingDTO> productsSpecialPricingList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(productsSpecialPricingList);
                    ProductsSpecialPricingListBL productsSpecialPricingListBL = new ProductsSpecialPricingListBL(executionContext, productsSpecialPricingList);
                    productsSpecialPricingListBL.DeleteProductsSpecialPricingList();
                    result = "Success";
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    result = "Falied";
                }
                log.LogMethodExit(result);
                return result;
            });
        }
    }
}
