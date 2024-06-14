/********************************************************************************************
 * Project Name - POS 
 * Description  - LocalTrxPOSPrinterOverrideRulesUseCases class to get the data  from local DB 
 * 
 *   
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0      06-Jan-2021      Dakshakh Raj              Created : Peru Invoice changes
 2.120.3      01-Mar-2022      Nitin Pai                 Entry should be made for all the entries under the override rule set by the use case
 ********************************************************************************************/
using System;
using Semnox.Core.Utilities;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Collections.Generic;
using Semnox.Parafait.POS;
using System.Linq;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// Local Trx POS PrinterOverride Rules UseCases
    /// </summary>
    public class LocalTrxPOSPrinterOverrideRulesUseCases : LocalUseCases, ITrxPOSPrinterOverrideRulesUseCases
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Local Trx POS PrinterOverride Rules UseCases
        /// </summary>
        /// <param name="executionContext"></param>
        public LocalTrxPOSPrinterOverrideRulesUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// GetTrxPOSPrinterOverrideRules
        /// </summary>
        /// <param name="parameters">parameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        public async Task<List<TrxPOSPrinterOverrideRulesDTO>> GetTrxPOSPrinterOverrideRules(List<KeyValuePair<TrxPOSPrinterOverrideRulesDTO.SearchByParameters, string>> parameters,
                                                                                       SqlTransaction sqlTransaction = null)
        {
            return await Task<List<TrxPOSPrinterOverrideRulesDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(parameters);
                TrxPOSPrinterOverrideRulesListBL trxPOSPrinterOverrideRulesListBL = new TrxPOSPrinterOverrideRulesListBL(executionContext);
                List<TrxPOSPrinterOverrideRulesDTO> trxPOSPrinterOverrideRulesDTOList = trxPOSPrinterOverrideRulesListBL.GetTrxPOSPrinterOverrideRulesDTOList(parameters, sqlTransaction);
                log.LogMethodExit(trxPOSPrinterOverrideRulesDTOList);
                return trxPOSPrinterOverrideRulesDTOList;
            });
        }

        /// <summary>
        /// Save Trx POS Printer Override Rules
        /// </summary>
        /// <param name="trxPOSPrinterOverrideRulesDTOList">trxPOSPrinterOverrideRulesDTOList</param>
        /// <returns></returns>
        public async Task<string> SaveTrxPOSPrinterOverrideRules(List<TrxPOSPrinterOverrideRulesDTO> trxPOSPrinterOverrideRulesDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                log.LogMethodEntry(trxPOSPrinterOverrideRulesDTOList);
                if (trxPOSPrinterOverrideRulesDTOList == null || !trxPOSPrinterOverrideRulesDTOList.Any())
                {
                    throw new ValidationException("TrxPOSPrinterOverrideRulesDTOList is Empty");
                }

                int trxId = trxPOSPrinterOverrideRulesDTOList[0].TransactionId;
                if (trxId == -1)
                {
                    throw new ValidationException("Invalid Transaction Id");
                }

                List<KeyValuePair<POSPrinterOverrideRulesDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<POSPrinterOverrideRulesDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<POSPrinterOverrideRulesDTO.SearchByParameters, string>(POSPrinterOverrideRulesDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                searchParameters.Add(new KeyValuePair<POSPrinterOverrideRulesDTO.SearchByParameters, string>(POSPrinterOverrideRulesDTO.SearchByParameters.IS_ACTIVE, "1"));
                searchParameters.Add(new KeyValuePair<POSPrinterOverrideRulesDTO.SearchByParameters, string>(POSPrinterOverrideRulesDTO.SearchByParameters.POS_PRINTER_ID, trxPOSPrinterOverrideRulesDTOList[0].POSPrinterId.ToString()));
                searchParameters.Add(new KeyValuePair<POSPrinterOverrideRulesDTO.SearchByParameters, string>(POSPrinterOverrideRulesDTO.SearchByParameters.POS_PRINTER_OVERRIDE_OPTION_ID, trxPOSPrinterOverrideRulesDTOList[0].POSPrinterOverrideOptionId.ToString()));

                POSPrinterOverrideRulesListBL pOSPrinterOverrideRulesListBL = new POSPrinterOverrideRulesListBL(executionContext);
                List<POSPrinterOverrideRulesDTO> pOSPrinterOverrideRulesDTOList = pOSPrinterOverrideRulesListBL.GetPOSPrinterOverrideRulesDTOList(searchParameters);

                if (pOSPrinterOverrideRulesDTOList == null || !pOSPrinterOverrideRulesDTOList.Any())
                {
                    throw new ValidationException("TrxPOSPrinterOverrideRulesDTOList is Empty");
                }

                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    try
                    {
                        parafaitDBTrx.BeginTransaction();
                        foreach (POSPrinterOverrideRulesDTO selectedDTO in pOSPrinterOverrideRulesDTOList)
                        {
                            TrxPOSPrinterOverrideRulesDTO trxPOSPrinterOverrideRulesDTO = new TrxPOSPrinterOverrideRulesDTO(-1, trxId, selectedDTO.POSPrinterId, selectedDTO.POSPrinterOverrideRuleId, selectedDTO.POSPrinterOverrideOptionId,
                                                                                                                        (POSPrinterOverrideOptionItemCode)Enum.Parse(typeof(POSPrinterOverrideOptionItemCode), selectedDTO.OptionItemCode),
                                                                                                                        selectedDTO.ItemSourceColumnGuid, true);

                            TrxPOSPrinterOverrideRulesBL trxPOSPrinterOverrideRulesBL = new TrxPOSPrinterOverrideRulesBL(executionContext, trxPOSPrinterOverrideRulesDTO);
                            trxPOSPrinterOverrideRulesBL.Save(parafaitDBTrx.SQLTrx);
                        }
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
                        throw new Exception(ex.Message, ex);
                    }
                }
                result = "Success";
                log.LogMethodExit(result);
                return result;
            });
        }

    }
}
