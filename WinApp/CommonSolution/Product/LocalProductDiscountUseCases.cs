/********************************************************************************************
 * Project Name - Product
 * Description  - LocalProductDiscountUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.00    30-Mar-2021       Roshan Devadiga        Created : POS UI Redesign with REST API
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
    /// Implementation of productDiscount use-cases
    /// </summary>
    public class LocalProductDiscountUseCases : IProductDiscountUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ExecutionContext executionContext;
        public LocalProductDiscountUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<ProductDiscountsDTO>> GetProductDiscounts(List<KeyValuePair<ProductDiscountsDTO.SearchByParameters, string>>
                         searchParameters)
        {
            return await Task<List<ProductDiscountsDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                ProductDiscountsListBL productDiscountsListBL = new ProductDiscountsListBL(executionContext);
                List<ProductDiscountsDTO> productDiscountsDTOList = productDiscountsListBL.GetProductDiscountsDTOList(searchParameters);

                log.LogMethodExit(productDiscountsDTOList);
                return productDiscountsDTOList;
            });
        }
        public async Task<string> SaveProductDiscounts(List<ProductDiscountsDTO> productDiscountsDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(productDiscountsDTOList);
                    if (productDiscountsDTOList == null)
                    {
                        throw new ValidationException("productDiscountsDTOList is Empty");
                    }
                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            ProductDiscountsListBL productModifiers = new ProductDiscountsListBL(executionContext, productDiscountsDTOList);
                            productModifiers.Save();
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

        
        public async Task<string> Delete(List<ProductDiscountsDTO> productDiscountsDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(productDiscountsDTOList);
                    ProductDiscountsListBL productModifiers = new ProductDiscountsListBL(executionContext, productDiscountsDTOList);
                    productModifiers.Save();
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
