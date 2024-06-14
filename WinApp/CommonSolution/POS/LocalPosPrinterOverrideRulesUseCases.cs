/********************************************************************************************
 * Project Name - POS 
 * Description  - LocalPosPrinterOverrideRulesUseCases class to get the data  from local DB 
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
    public class LocalPOSPrinterOverrideRulesUseCases : LocalUseCases, IPOSPrinterOverrideRulesUseCases
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType); 

        public LocalPOSPrinterOverrideRulesUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        public async Task<List<POSPrinterOverrideRulesDTO>> GetPOSPrinterOverrideRules(List<KeyValuePair<POSPrinterOverrideRulesDTO.SearchByParameters, string>> parameters,
                                                                                       SqlTransaction sqlTransaction = null)
        {
            return await Task<List<POSPrinterOverrideRulesDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(parameters);
                POSPrinterOverrideRulesListBL pOSPrinterOverrideRulesListBL = new POSPrinterOverrideRulesListBL(executionContext);
                List<POSPrinterOverrideRulesDTO> pOSPrinterOverrideRulesDTOList = pOSPrinterOverrideRulesListBL.GetPOSPrinterOverrideRulesDTOList(parameters, sqlTransaction);
                log.LogMethodExit(pOSPrinterOverrideRulesDTOList);
                return pOSPrinterOverrideRulesDTOList;
            });
        }
        public async Task<string> SavePOSPrinterOverrideRules(List<POSPrinterOverrideRulesDTO> pOSPrinterOverrideRulesDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                log.LogMethodEntry(pOSPrinterOverrideRulesDTOList);
                if (pOSPrinterOverrideRulesDTOList == null)
                {
                    throw new ValidationException("POSPrinterOverrideRulesDTOList is Empty");
                }

                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (POSPrinterOverrideRulesDTO pOSPrinterOverrideRulesDTO in pOSPrinterOverrideRulesDTOList)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            POSPrinterOverrideRulesBL pOSPrinterOverrideRulesBL = new POSPrinterOverrideRulesBL(executionContext, pOSPrinterOverrideRulesDTO);
                            pOSPrinterOverrideRulesBL.Save(parafaitDBTrx.SQLTrx);
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
