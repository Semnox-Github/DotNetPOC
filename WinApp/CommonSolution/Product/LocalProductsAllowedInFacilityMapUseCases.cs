/********************************************************************************************
* Project Name - User
* Description  - LocalProductsAllowedInFacilityMap  class 
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
   public class LocalProductsAllowedInFacilityMapUseCases:IProductsAllowedInFacilityMapUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ExecutionContext executionContext;
        public LocalProductsAllowedInFacilityMapUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<ProductsAllowedInFacilityMapDTO>> GetProductsAllowedInFacilityMaps(List<KeyValuePair<ProductsAllowedInFacilityMapDTO.SearchByParameters, string>>
                         searchParameters, bool loadProductsDTO = false, SqlTransaction sqlTransaction = null)
        {
            return await Task<List<ProductsAllowedInFacilityMapDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters, loadProductsDTO, sqlTransaction);

                ProductsAllowedInFacilityMapListBL productsAllowedInFacilityMapListBL = new ProductsAllowedInFacilityMapListBL(executionContext);
                List<ProductsAllowedInFacilityMapDTO> productsAllowedInFacilityMapDTOList = productsAllowedInFacilityMapListBL.GetProductsAllowedInFacilityMapDTOList(searchParameters, loadProductsDTO, sqlTransaction);

                log.LogMethodExit(productsAllowedInFacilityMapDTOList);
                return productsAllowedInFacilityMapDTOList;
            });
        }
       
        public async Task<string> SaveProductsAllowedInFacilityMaps(List<ProductsAllowedInFacilityMapDTO> productsAllowedInFacilityMapDTO)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(productsAllowedInFacilityMapDTO);
                    if (productsAllowedInFacilityMapDTO == null)
                    {
                        throw new ValidationException("productsAllowedInFacilityMapDTO is Empty");
                    }
                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            ProductsAllowedInFacilityMapListBL productsAllowedInFacilityMapListBL = new ProductsAllowedInFacilityMapListBL(executionContext, productsAllowedInFacilityMapDTO);
                            productsAllowedInFacilityMapListBL.Save();
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
    }
}
