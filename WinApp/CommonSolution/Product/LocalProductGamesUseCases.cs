/********************************************************************************************
 * Project Name - Product
 * Description  - LocalProductGamesUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.00    07-Apr-2021       Roshan Devadiga        Created : POS UI Redesign with REST API
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
   public class LocalProductGamesUseCases:IProductGamesUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ExecutionContext executionContext;
        public LocalProductGamesUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<ProductGamesDTO>> GetProductGames(List<KeyValuePair<ProductGamesDTO.SearchByProductGamesParameters, string>>
                         searchParameters, SqlTransaction sqlTransaction = null)
        {
            return await Task<List<ProductGamesDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                ProductGamesListBL productGamesListBL = new ProductGamesListBL(executionContext);
                List<ProductGamesDTO> productGamesDTOList = productGamesListBL.GetProductGamesDTOList(searchParameters, sqlTransaction);

                log.LogMethodExit(productGamesDTOList);
                return productGamesDTOList;
            });
        }
        public async Task<string> SaveProductGames(List<ProductGamesDTO> productGamesDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(productGamesDTOList);
                    if (productGamesDTOList == null)
                    {
                        throw new ValidationException("productGamesDTOList is Empty");
                    }
                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            ProductGamesListBL productGamesListBL = new ProductGamesListBL(executionContext, productGamesDTOList);
                            productGamesListBL.SaveUpdateProductGamesList();
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

        public async Task<string> Delete(List<ProductGamesDTO> productGamesDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(productGamesDTOList);
                    ProductGamesListBL productGamesListBL = new ProductGamesListBL(executionContext,productGamesDTOList);
                    productGamesListBL.DeleteProductGamesList();
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
