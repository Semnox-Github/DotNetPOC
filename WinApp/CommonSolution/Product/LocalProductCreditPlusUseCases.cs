/********************************************************************************************
 * Project Name - Product
 * Description  - LocalProductCreditPlusUseCases Class
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.00    11-Mar-2021       B Mahesh Pai       Created : POS UI Redesign with REST API
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
   public class LocalProductCreditPlusUseCases:IProductCreditPlusUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalProductCreditPlusUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<ProductCreditPlusDTO>> GetProductCreditPlus(List<KeyValuePair<ProductCreditPlusDTO.SearchByParameters, string>>
                         searchParameters)
        {
            return await Task<List<ProductCreditPlusDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                ProductCreditPlusBLList productCreditPlusBLList = new ProductCreditPlusBLList(executionContext);
                List<ProductCreditPlusDTO> productCreditPlusDTOList = productCreditPlusBLList.GetAllProductCreditPlusListDTOList(searchParameters);

                log.LogMethodExit(productCreditPlusDTOList);
                return productCreditPlusDTOList;
            });
        }
        public async Task<string> SaveProductCreditPlus(List<ProductCreditPlusDTO> productCreditPlusDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                log.LogMethodEntry(productCreditPlusDTOList);
                if (productCreditPlusDTOList == null)
                {
                    throw new ValidationException("productCreditPlusDTOList is Empty");
                }

                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (ProductCreditPlusDTO productCreditPlusDTO in productCreditPlusDTOList)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            ProductCreditPlusBL productCreditPlusBL = new ProductCreditPlusBL(executionContext, productCreditPlusDTO);
                            productCreditPlusBL.Save(parafaitDBTrx.SQLTrx);
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
                }

                result = "Success";
                log.LogMethodExit(result);
                return result;
            });
        }

    }
}
