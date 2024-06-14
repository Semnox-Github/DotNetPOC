/********************************************************************************************
 * Project Name - DigitalSignage
 * Description  - LocalTickerUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.140.00    22-Apr-2021       Roshan Devadiga        Created : POS UI Redesign with REST API
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.DigitalSignage
{
    /// <summary>
    /// Implementation of Ticker use-cases
    /// </summary>
    public class LocalTickerUseCases:ITickerUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ExecutionContext executionContext;
        public LocalTickerUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<TickerDTO>> GetTickers(List<KeyValuePair<TickerDTO.SearchByParameters, string>> searchParameters)
        {
            return await Task<List<TickerDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                TickerListBL tickerListBL = new TickerListBL(executionContext);
                List<TickerDTO> tickerDTOList = tickerListBL.GetTickerDTOList(searchParameters);

                log.LogMethodExit(tickerDTOList);
                return tickerDTOList;
            });
        }
        public async Task<string> SaveTickers(List<TickerDTO> tickerDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(tickerDTOList);
                    if (tickerDTOList == null)
                    {
                        throw new ValidationException("tickerDTOList is Empty");
                    }
                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            TickerListBL tickerListBL = new TickerListBL(executionContext, tickerDTOList);
                            tickerListBL.Save();
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
