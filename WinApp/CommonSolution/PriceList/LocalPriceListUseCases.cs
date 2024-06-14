/********************************************************************************************
 * Project Name - PriceList
 * Description  - LocalPriceListUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.00    10-Mar-2021       Roshan Devadiga        Created : POS UI Redesign with REST API
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.PriceList
{
    // <summary>
    /// Implementation of priceList use-cases
    /// </summary>
    public class LocalPriceListUseCases:IPriceListUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ExecutionContext executionContext;
        public LocalPriceListUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<PriceListDTO>> GetPriceLists(List<KeyValuePair<PriceListDTO.SearchByPriceListParameters, string>>
                         searchParameters, bool loadActiveRecordsOnly = false, SqlTransaction sqlTransaction = null)
        {
            return await Task<List<PriceListDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters,loadActiveRecordsOnly,sqlTransaction);

                PriceListList priceListList = new PriceListList(executionContext);
                List<PriceListDTO> priceListDTOList = priceListList.GetAllPriceListProducts(searchParameters,loadActiveRecordsOnly,sqlTransaction);

                log.LogMethodExit(priceListDTOList);
                return priceListDTOList;
            });
        }
        public async Task<string> SavePriceLists(List<PriceListDTO> priceListDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                log.LogMethodEntry(priceListDTOList);
                if (priceListDTOList == null)
                {
                    throw new ValidationException("priceListDTOList is Empty");
                }

                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (PriceListDTO priceListDTO in priceListDTOList)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            PriceList priceList = new PriceList(executionContext,priceListDTO);
                            priceList.Save(parafaitDBTrx.SQLTrx);
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
        public async Task<string> Delete(List<PriceListDTO> priceListDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(priceListDTOList);
                    PriceListList priceListList = new PriceListList(executionContext, priceListDTOList);
                    priceListList.DeletePriceListList();
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
