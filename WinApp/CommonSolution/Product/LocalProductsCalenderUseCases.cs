/********************************************************************************************
 * Project Name - Product
 * Description  - LocalProductsCalenderUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.00    08-Mar-2021       Roshan Devadiga        Created : POS UI Redesign with REST API
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
    /// Implementation of productsCalender use-cases
    /// </summary>
   public class LocalProductsCalenderUseCases :IProductsCalenderUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ExecutionContext executionContext;
        public LocalProductsCalenderUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<ProductsCalenderDTO>> GetProductsCalenders(List<KeyValuePair<ProductsCalenderDTO.SearchByParameters, string>>
                         searchParameters)
        {
            return await Task<List<ProductsCalenderDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                ProductsCalenderList productsCalenderList = new ProductsCalenderList(executionContext);
                List<ProductsCalenderDTO> productsCalenderDTOList = productsCalenderList.GetAllProductCalenderList(searchParameters);

                log.LogMethodExit(productsCalenderDTOList);
                return productsCalenderDTOList;
            });
        }

        public async Task<string> SaveProductsCalenders(List<ProductsCalenderDTO> productsCalenderList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(productsCalenderList);
                    if (productsCalenderList == null)
                    {
                        throw new ValidationException("productsCalenderList is Empty");
                    }
                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            ProductsCalenderList productsCalender = new ProductsCalenderList(executionContext, productsCalenderList);
                            productsCalender.SaveUpdateProductCalenderList();
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
                }
                log.LogMethodExit(result);
                return result;
            });
        }
        
        public async Task<string> Delete(List<ProductsCalenderDTO> productsCalenderDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(productsCalenderDTOList);
                    ProductsCalenderList productsCalenderList = new ProductsCalenderList(executionContext, productsCalenderDTOList);
                    productsCalenderList.DeleteProductCalenderList();
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
