/********************************************************************************************
 * Project Name - POS 
 * Description  - LocalPosPrinterOverrideOptionsUseCases class to get the data  from local DB 
 * 
 *   
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0      30-Dec-2020      Dakshakh Raj              Created : Peru Invoice changes
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace Semnox.Parafait.POS
{
    public class LocalPOSPrinterOverrideOptionsUseCases : LocalUseCases, IPOSPrinterOverrideOptionsUseCases
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType); 

        public LocalPOSPrinterOverrideOptionsUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        public async Task<List<POSPrinterOverrideOptionsDTO>> GetPOSPrinterOverrideOptions(List<KeyValuePair<POSPrinterOverrideOptionsDTO.SearchByParameters, string>> parameters,
                                                                                       SqlTransaction sqlTransaction = null)
        {
            return await Task<List<POSPrinterOverrideOptionsDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(parameters);
                POSPrinterOverrideOptionsListBL pOSPrinterOverrideOptionsListBL = new POSPrinterOverrideOptionsListBL(executionContext);
                List<POSPrinterOverrideOptionsDTO> pOSPrinterOverrideOptionsDTOList = pOSPrinterOverrideOptionsListBL.GetPOSPrinterOverrideOptionsDTOList(parameters, sqlTransaction);
                log.LogMethodExit(pOSPrinterOverrideOptionsDTOList);
                return pOSPrinterOverrideOptionsDTOList;
            });
        }
        public async Task<string> SavePOSPrinterOverrideOptions(List<POSPrinterOverrideOptionsDTO> pOSPrinterOverrideOptionsDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                log.LogMethodEntry(pOSPrinterOverrideOptionsDTOList);
                if (pOSPrinterOverrideOptionsDTOList == null)
                {
                    throw new ValidationException("POSPrinterOverrideOptionsDTOList is Empty");
                }

                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (POSPrinterOverrideOptionsDTO pOSPrinterOverrideOptionsDTO in pOSPrinterOverrideOptionsDTOList)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            POSPrinterOverrideOptionsBL pOSPrinterOverrideOptionsBL = new POSPrinterOverrideOptionsBL(executionContext, pOSPrinterOverrideOptionsDTO);
                            pOSPrinterOverrideOptionsBL.Save(parafaitDBTrx.SQLTrx);
                            parafaitDBTrx.EndTransaction();
                        }
                        catch (ValidationException valEx)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(valEx);
                            throw;
                        }
                        catch (Exception ex)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            throw;
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
