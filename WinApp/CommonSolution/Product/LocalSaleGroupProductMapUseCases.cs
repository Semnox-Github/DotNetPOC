/********************************************************************************************
* Project Name - Product
* Description  - LocalSaleGroupProductMapUseCases class 
*  
**************
**Version Log
**************
*Version     Date             Modified By               Remarks          
*********************************************************************************************
2.120.00    19-Feb-2021       Roshan Devadiga        Created : POS UI Redesign with REST API
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
    /// Implementation of saleGroupProductMap use-cases
    /// </summary>
    public class LocalSaleGroupProductMapUseCases:ISaleGroupProductMapUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ExecutionContext executionContext;
        public LocalSaleGroupProductMapUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<SaleGroupProductMapDTO>> GetSaleGroupProductMaps(List<KeyValuePair<SaleGroupProductMapDTO.SearchBySaleGroupProductMapParameters, string>>
                         searchParameters)
        {
            return await Task<List<SaleGroupProductMapDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                SaleGroupProductMapList saleGroupProductMapList = new SaleGroupProductMapList(executionContext);
                List<SaleGroupProductMapDTO> saleGroupProductMapDTOList = saleGroupProductMapList.GetAllSaleGroupProductMaps(searchParameters);

                log.LogMethodExit(saleGroupProductMapDTOList);
                return saleGroupProductMapDTOList;
            });
        }
        public async Task<string> SaveSaleGroupProductMaps(List<SaleGroupProductMapDTO> saleGroupProductMapList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(saleGroupProductMapList);
                    if (saleGroupProductMapList == null)
                    {
                        throw new ValidationException("saleGroupProductMapList is Empty");
                    }
                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            SaleGroupProductMapList saleGroupProductMap = new SaleGroupProductMapList(executionContext, saleGroupProductMapList);
                            saleGroupProductMap.SaveUpdateSetupOfferGroupProductMap();
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
