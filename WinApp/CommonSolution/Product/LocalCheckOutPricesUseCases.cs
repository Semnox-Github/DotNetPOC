/********************************************************************************************
 * Project Name - Product
 * Description  - LocalCheckOutPricesUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
*2.140.00   14-Sep-2021      Roshan Devadiga            Created 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    // <summary>
    /// Implementation of checkOutPrices use-cases
    /// </summary>
    public class LocalCheckOutPricesUseCases: ICheckOutPricesUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ExecutionContext executionContext;
        public LocalCheckOutPricesUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<CheckOutPricesDTO>> GetCheckOutPrices(List<KeyValuePair<CheckOutPricesDTO.SearchByParameters, string>>
                         searchParameters)
        {
            return await Task<List<CheckOutPricesDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                CheckOutPricesBLList checkOutPricesBLList = new CheckOutPricesBLList(executionContext);
                List<CheckOutPricesDTO> checkOutPricesDTOList = checkOutPricesBLList.GetAllCheckOutPricesList(searchParameters);

                log.LogMethodExit(checkOutPricesDTOList);
                return checkOutPricesDTOList;
            });
        }
        public async Task<string> SaveCheckOutPrices(List<CheckOutPricesDTO> checkOutPricesDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                log.LogMethodEntry(checkOutPricesDTOList);
                if (checkOutPricesDTOList == null)
                {
                    throw new ValidationException("checkOutPricesDTOList is Empty");
                }

                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (CheckOutPricesDTO checkOutPricesDTO in checkOutPricesDTOList)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            CheckOutPricesBL checkOutPricesBL = new CheckOutPricesBL(executionContext, checkOutPricesDTO);
                            checkOutPricesBL.Save(parafaitDBTrx.SQLTrx);
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
        public async Task<string> Delete(List<CheckOutPricesDTO> checkOutPricesDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(checkOutPricesDTOList);
                    CheckOutPricesBLList checkOutPricesBLList = new CheckOutPricesBLList(executionContext, checkOutPricesDTOList);
                    checkOutPricesBLList.Delete();
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
