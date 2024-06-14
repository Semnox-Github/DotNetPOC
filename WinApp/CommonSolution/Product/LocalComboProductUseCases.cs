/********************************************************************************************
 * Project Name - Product
 * Description  - LocalComboProductUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
*2.140.00   14-Sep-2021       Prajwal S               Created 
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
    class LocalComboProductUseCases : IComboProductUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalComboProductUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<ComboProductDTO>> GetComboProduct(List<KeyValuePair<ComboProductDTO.SearchByParameters, string>>
                          searchParameters, int currentPage = 0, int pageSize = 0, SqlTransaction sqlTransaction = null
                         )
        {
            return await Task<List<ComboProductDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                ComboProductList comboProductListBL = new ComboProductList(executionContext);
                List<ComboProductDTO> comboProductDTOList = comboProductListBL.GetComboProductDTOList(searchParameters,  /*currentPage, pageSize,*/ sqlTransaction);

                log.LogMethodExit(comboProductDTOList);
                return comboProductDTOList;
            });
        }

        //public async Task<int> GetComboProductCount(List<KeyValuePair<ComboProductDTO.SearchByParameters, string>>
        //                                              searchParameters, SqlTransaction sqlTransaction = null
        //                     )
        //{
        //    return await Task<int>.Factory.StartNew(() =>
        //    {
        //        log.LogMethodEntry(searchParameters);

        //        ComboProductListBL ComboProductsListBL = new ComboProductListBL(executionContext);
        //        int count = ComboProductsListBL.GetComboProductCount(searchParameters, sqlTransaction);

        //        log.LogMethodExit(count);
        //        return count;
        //    });
        //}

        public async Task<List<ComboProductDTO>> SaveComboProduct(List<ComboProductDTO> comboProductDTOList)
        {
            return await Task<List<ComboProductDTO>>.Factory.StartNew(() =>
            {
                using (ParafaitDBTransaction transaction = new ParafaitDBTransaction())
                {
                    transaction.BeginTransaction();
                    ComboProductList comboProductList = new ComboProductList(executionContext, comboProductDTOList);
                    List<ComboProductDTO> result = comboProductList.SaveUpdateComboProductList();
                    transaction.EndTransaction();
                    return result;
                }
            });
        }

        public async Task<string> DeleteComboProduct(List<ComboProductDTO> comboProductDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                using (ParafaitDBTransaction transaction = new ParafaitDBTransaction())
                {
                    transaction.BeginTransaction();
                    ComboProductList comboProductList = new ComboProductList(executionContext, comboProductDTOList);
                    comboProductList.DeleteComboProductList();
                    transaction.EndTransaction();
                    string result = "success";
                    return result;
                }
            });
        }
    }
}
