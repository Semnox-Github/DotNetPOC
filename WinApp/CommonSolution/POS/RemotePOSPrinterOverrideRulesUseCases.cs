/********************************************************************************************
 * Project Name - POS 
 * Description  - Remote PosPrinterOverrideRulesUseCases class to get the data 
 * 
 *   
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0      30-Dec-2020      Dakshakh Raj              Created : Peru Invoice changes
 ********************************************************************************************/
using Newtonsoft.Json;
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.POS
{
    public class RemotePOSPrinterOverrideRulesUseCases : RemoteUseCases, IPOSPrinterOverrideRulesUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string POS_PRINTER_OVERRIDE_RULE_URL = "api/POS/POSPrinterOverrideRules";

        public RemotePOSPrinterOverrideRulesUseCases(ExecutionContext executionContext)
           : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        /// <summary>
        /// Get POS Printer Override Rules
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public async Task<List<POSPrinterOverrideRulesDTO>> GetPOSPrinterOverrideRules(List<KeyValuePair<POSPrinterOverrideRulesDTO.SearchByParameters, string>> parameters,
                                                                    SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parameters);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            try
            {
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                List<POSPrinterOverrideRulesDTO> result = await Get<List<POSPrinterOverrideRulesDTO>>(POS_PRINTER_OVERRIDE_RULE_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        /// <summary>
        /// BuildSearchParameter
        /// </summary>
        /// <param name="POSPrinterOverrideRulesSearchParams"></param>
        /// <returns></returns>
        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<POSPrinterOverrideRulesDTO.SearchByParameters, string>> POSPrinterOverrideRulesSearchParams)
        {
            log.LogMethodEntry(POSPrinterOverrideRulesSearchParams);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<POSPrinterOverrideRulesDTO.SearchByParameters, string> searchParameter in POSPrinterOverrideRulesSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case POSPrinterOverrideRulesDTO.SearchByParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                    case POSPrinterOverrideRulesDTO.SearchByParameters.POS_PRINTER_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("pOSPrinterId".ToString(), searchParameter.Value));
                        }
                        break;
                    case POSPrinterOverrideRulesDTO.SearchByParameters.POS_PRINTER_OVERRIDE_RULE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("pOSPrinterOverrideRuleId".ToString(), searchParameter.Value));
                        }
                        break;
                    case POSPrinterOverrideRulesDTO.SearchByParameters.POS_PRINTER_OVERRIDE_OPTION_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("pOSPrinterOverrideOptionId".ToString(), searchParameter.Value));
                        }
                        break;
                    case POSPrinterOverrideRulesDTO.SearchByParameters.OPTION_ITEM_CODE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("optionItemCode".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }

        /// <summary>
        /// Save POS Printer Override Rules
        /// </summary>
        /// <param name="pOSPrinterOverrideRulesDTOList"></param>
        /// <returns></returns>
        public async Task<string> SavePOSPrinterOverrideRules(List<POSPrinterOverrideRulesDTO> pOSPrinterOverrideRulesDTOList)
        {
            log.LogMethodEntry(pOSPrinterOverrideRulesDTOList);
            try
            {
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                string content = JsonConvert.SerializeObject(pOSPrinterOverrideRulesDTOList);
                string responseString = await Post<string>(POS_PRINTER_OVERRIDE_RULE_URL, content);
                //dynamic response = JsonConvert.DeserializeObject(responseString);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
    }
}
