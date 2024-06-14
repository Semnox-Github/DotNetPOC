/********************************************************************************************
* Project Name - Products
* Description  - LocalProductsDisplayGroupUseCases  class 
*  
**************
**Version Log
**************
*Version     Date             Modified By               Remarks          
*********************************************************************************************
2.120.00    06-Apr-2021       B Mahesh Pai        Created : POS UI Redesign with REST API
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
    public class LocalProductsDisplayGroupUseCases : IProductDisplayGroupUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ExecutionContext executionContext;
        public LocalProductsDisplayGroupUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<ProductsDisplayGroupDTO>> GetProductsDisplayGroups(List<KeyValuePair<ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            return await Task<List<ProductsDisplayGroupDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters, sqlTransaction);

                ProductsDisplayGroupList productsDisplayGroupList = new ProductsDisplayGroupList(executionContext);
                List<ProductsDisplayGroupDTO> productsDisplayGroupDTOList = productsDisplayGroupList.GetAllProductsDisplayGroup(searchParameters, sqlTransaction);

                log.LogMethodExit(productsDisplayGroupDTOList);
                return productsDisplayGroupDTOList;
            });
        }
       
        public async Task<string> SaveProductsDisplayGroups(List<ProductsDisplayGroupDTO> productsDisplayGroupDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(productsDisplayGroupDTOList);
                    if (productsDisplayGroupDTOList == null)
                    {
                        throw new ValidationException("productsDisplayGroupDTOList is Empty");
                    }
                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            ProductsDisplayGroupList productsDisplayGroupList = new ProductsDisplayGroupList(executionContext, productsDisplayGroupDTOList);
                            productsDisplayGroupList.Save();
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
        public async Task<string> Delete(List<ProductsDisplayGroupDTO> productsDisplayGroupDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(productsDisplayGroupDTOList);
                    ProductsDisplayGroupList productsDisplayGroupList = new ProductsDisplayGroupList(executionContext, productsDisplayGroupDTOList);
                    productsDisplayGroupList.DeleteProductDisplayGroupList();
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