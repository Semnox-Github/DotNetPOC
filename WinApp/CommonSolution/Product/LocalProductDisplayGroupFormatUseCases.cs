/********************************************************************************************
 * Project Name - Product
 * Description  - LocalProductDisplayGroupFormatUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
*2.140.00   14-Sep-2021      Roshan Devadiga            Created :POS UI Redesign with REST API
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
    /// <summary>
    /// Implementation of productDisplayGroupFormat use-cases
    /// </summary>
    public class LocalProductDisplayGroupFormatUseCases:IProductDisplayGroupFormatUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ExecutionContext executionContext;
        public LocalProductDisplayGroupFormatUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<ProductDisplayGroupFormatDTO>> GetProductDisplayGroupFormats(List<KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>>
                         searchParameters, bool loadChildRecords = false, bool loadActiveChildRecords = false, SqlTransaction sqlTransaction = null)
        {
            return await Task<List<ProductDisplayGroupFormatDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters, loadChildRecords, loadActiveChildRecords, sqlTransaction);

                ProductDisplayGroupList productDisplayGroupList = new ProductDisplayGroupList(executionContext);
                List<ProductDisplayGroupFormatDTO> productDisplayGroupFormatDTOList = productDisplayGroupList.GetAllProductDisplayGroup(searchParameters, loadChildRecords, loadActiveChildRecords, sqlTransaction);

                log.LogMethodExit(productDisplayGroupFormatDTOList);
                return productDisplayGroupFormatDTOList;
            });
        }
        public async Task<string> SaveProductDisplayGroupFormats(List<ProductDisplayGroupFormatDTO> productDisplayGroupFormatDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                log.LogMethodEntry(productDisplayGroupFormatDTOList);
                if (productDisplayGroupFormatDTOList == null)
                {
                    throw new ValidationException("productDisplayGroupFormatDTOList is Empty");
                }

                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (ProductDisplayGroupFormatDTO productDisplayGroupFormatDTO in productDisplayGroupFormatDTOList)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            ProductDisplayGroupFormat productDisplayGroupFormat = new ProductDisplayGroupFormat(executionContext, productDisplayGroupFormatDTO);
                            productDisplayGroupFormat.Save(parafaitDBTrx.SQLTrx);
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
        public async Task<string> Delete(List<ProductDisplayGroupFormatDTO> productDisplayGroupFormatDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(productDisplayGroupFormatDTOList);
                    ProductDisplayGroupList productDisplayGroupList = new ProductDisplayGroupList(executionContext, productDisplayGroupFormatDTOList);
                    productDisplayGroupList.DeleteProductDisplayGroupFormatList();
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
        public async Task<List<ProductDisplayGroupFormatDTO>> GetConfiguredDisplayGroupListForLogin(string loginId, SqlTransaction sqlTransaction = null)
        {
            return await Task<List<ProductDisplayGroupFormatDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(loginId,sqlTransaction);

                ProductDisplayGroupList productDisplayGroupList = new ProductDisplayGroupList(executionContext);
                List<ProductDisplayGroupFormatDTO> productDisplayGroupFormatDTOList = productDisplayGroupList.GetConfiguredDisplayGroupListForLogin(loginId, false, false, sqlTransaction);

                log.LogMethodExit(productDisplayGroupFormatDTOList);
                return productDisplayGroupFormatDTOList;
            });
        }

        public async Task<ProductDisplayGroupFormatContainerDTOCollection> GetProductDisplayGroupFormatContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            return await Task<ProductDisplayGroupFormatContainerDTOCollection>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(hash, rebuildCache);
                if (rebuildCache)
                {
                    ProductDisplayGroupFormatContainerList.Rebuild(siteId);
                }
                ProductDisplayGroupFormatContainerDTOCollection result = ProductDisplayGroupFormatContainerList.GetProductDisplayGroupFormatContainerDTOCollection(siteId);
                if (hash == result.Hash)
                {
                    log.LogMethodExit(null, "No changes to the cache");
                    return null;
                }
                log.LogMethodExit(result);
                return result;
            });
        }
    }
}
