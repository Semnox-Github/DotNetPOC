/********************************************************************************************
 * Project Name - RedemptionUtils
 * Description  - RemoteRedemptionCurrencyRuleUseCases class to get the data  from API by doing remote call  
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0          07-Nov-2020      Vikas Dwivedi             Created : POS UI Redesign with REST API
 2.110.1      14-Feb-2021      Mushahid Faizan           Modified : Web Inventory Phase 2 changes
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Redemption
{
    public class RemoteRedemptionCurrencyRuleUseCases : RemoteUseCases, IRedemptionCurrencyRuleUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string REDEMPTION_CURRENCY_RULE_URL = "/api/Inventory/RedemptionCurrencyRules";
        private const string REDEMPTION_CURRENCY_RULE_CONTAINER_URL = "/api/Inventory/RedemptionCurrencyRulesContainer";
        private const string REDEMPTION_CURRENCY_RULE_COUNT_URL = "/api/Inventory/RedemptionCurrencyRuleCounts";

        public RemoteRedemptionCurrencyRuleUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<List<RedemptionCurrencyRuleDTO>> GetRedemptionCurrencyRules(List<KeyValuePair<RedemptionCurrencyRuleDTO.SearchByRedemptionCurrencyRuleParameters, string>> parameters,
            bool loadChildRecords = false, bool activeChildRecords = true, int currentPage = 0, int pageSize = 0, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parameters);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("loadChildRecords".ToString(), loadChildRecords.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("activeChildRecords".ToString(), activeChildRecords.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("currentPage".ToString(), currentPage.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("pageSize".ToString(), pageSize.ToString()));
            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            try
            {
                List<RedemptionCurrencyRuleDTO> result = await Get<List<RedemptionCurrencyRuleDTO>>(REDEMPTION_CURRENCY_RULE_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<int> GetRedemptionCurrencyRuleCount(List<KeyValuePair<RedemptionCurrencyRuleDTO.SearchByRedemptionCurrencyRuleParameters, string>> parameters,
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
                int result = await Get<int>(REDEMPTION_CURRENCY_RULE_COUNT_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<RedemptionCurrencyRuleDTO.SearchByRedemptionCurrencyRuleParameters, string>> redemptionCurrencyRuleSearchParams)
        {
            log.LogMethodEntry(redemptionCurrencyRuleSearchParams);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<RedemptionCurrencyRuleDTO.SearchByRedemptionCurrencyRuleParameters, string> searchParameter in redemptionCurrencyRuleSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case RedemptionCurrencyRuleDTO.SearchByRedemptionCurrencyRuleParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                    case RedemptionCurrencyRuleDTO.SearchByRedemptionCurrencyRuleParameters.REDEMPTION_CURRENCY_RULE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("redemptionCurrencyRuleId".ToString(), searchParameter.Value));
                        }
                        break;
                    case RedemptionCurrencyRuleDTO.SearchByRedemptionCurrencyRuleParameters.REDEMPTION_CURRENCY_RULE_NAME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("redemptionCurrencyRuleName".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }

        public async Task<string> SaveRedemptionCurrencyRules(List<RedemptionCurrencyRuleDTO> redemptionCurrencyRuleDTOList)
        {
            log.LogMethodEntry(redemptionCurrencyRuleDTOList);
            try
            {
                string responseString = await Post<string>(REDEMPTION_CURRENCY_RULE_URL, redemptionCurrencyRuleDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<RedemptionCurrencyRuleContainerDTOCollection> GetRedemptionCurrencyRuleContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(hash, rebuildCache);
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("siteId", siteId.ToString()));
            if (string.IsNullOrWhiteSpace(hash) == false)
            {
                parameters.Add(new KeyValuePair<string, string>("hash", hash));
            }
            parameters.Add(new KeyValuePair<string, string>("rebuildCache", rebuildCache.ToString()));
            RedemptionCurrencyRuleContainerDTOCollection result = await Get<RedemptionCurrencyRuleContainerDTOCollection>(REDEMPTION_CURRENCY_RULE_CONTAINER_URL, parameters);
            log.LogMethodExit(result);
            return result;
        }
    }
}
