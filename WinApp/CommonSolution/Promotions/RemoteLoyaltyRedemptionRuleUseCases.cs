/********************************************************************************************
 * Project Name -Promotions
 * Description  -LoyaltyRedemptionUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.00    05-Mar-2021       Roshan Devadiga        Created : POS UI Redesign with REST API
 2.120.00    25-Mar-2021       Fiona                   Modified to add Delete UseCase
 ********************************************************************************************/
using Newtonsoft.Json;
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Promotions
{
    class RemoteLoyaltyRedemptionRuleUseCases : RemoteUseCases, ILoyaltyRedemptionRuleUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string LOYALTYREDEMPTIONRULE_URL = "api/Promotion/LoyaltyRedemptionRules";
        private const string LOYALTYREDEMPTIONRULE_CONTAINER_URL = "api/Promotion/LoyaltyRedemptionRulesContainer";

        public RemoteLoyaltyRedemptionRuleUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<List<LoyaltyRedemptionRuleDTO>> GetLoyaltyRedemptionRules(List<KeyValuePair<LoyaltyRedemptionRuleDTO.SearchByParameters, string>>
                         parameters)
        {
            log.LogMethodEntry(parameters);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();

            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            try
            {
                List<LoyaltyRedemptionRuleDTO> result = await Get<List<LoyaltyRedemptionRuleDTO>>(LOYALTYREDEMPTIONRULE_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<LoyaltyRedemptionRuleDTO.SearchByParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<LoyaltyRedemptionRuleDTO.SearchByParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case LoyaltyRedemptionRuleDTO.SearchByParameters.REDEMPTION_RULE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("redemptionRuleId".ToString(), searchParameter.Value));
                        }
                        break;
                    case LoyaltyRedemptionRuleDTO.SearchByParameters.LOYALTY_ATTR_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("loyaltyAttributeId".ToString(), searchParameter.Value));
                        }
                        break;
                    case LoyaltyRedemptionRuleDTO.SearchByParameters.EXPIRY_DATE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("expiryDate".ToString(), searchParameter.Value));
                        }
                        break;
                    case LoyaltyRedemptionRuleDTO.SearchByParameters.ACTIVE_FLAG:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("activeFlag".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }
        public async Task<string> SaveLoyaltyRedemptionRules(List<LoyaltyRedemptionRuleDTO> loyaltyRedemptionRuleDTOList)
        {
            log.LogMethodEntry(loyaltyRedemptionRuleDTOList);
            try
            {
                string responseString = await Post<string>(LOYALTYREDEMPTIONRULE_URL, loyaltyRedemptionRuleDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        public async Task<LoyaltyRedemptionRuleContainerDTOCollection> GetLoyaltyRedemptionRuleContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(hash, rebuildCache);
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("siteId", siteId.ToString()));
            if (string.IsNullOrWhiteSpace(hash) == false)
            {
                parameters.Add(new KeyValuePair<string, string>("hash", hash));
            }
            parameters.Add(new KeyValuePair<string, string>("rebuildCache", rebuildCache.ToString()));
            LoyaltyRedemptionRuleContainerDTOCollection result = await Get<LoyaltyRedemptionRuleContainerDTOCollection>(LOYALTYREDEMPTIONRULE_CONTAINER_URL, parameters);
            log.LogMethodExit(result);
            return result;
        }

        public async Task<string> Delete(List<LoyaltyRedemptionRuleDTO> loyaltyRedemptionRuleDTOList)
        {
            try
            {
                log.LogMethodEntry(loyaltyRedemptionRuleDTOList);
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                string content = JsonConvert.SerializeObject(loyaltyRedemptionRuleDTOList);
                string responseString = await Delete(LOYALTYREDEMPTIONRULE_URL, content);
                dynamic response = JsonConvert.DeserializeObject(responseString);
                log.LogMethodExit(response);
                return response;
            }
            catch (WebApiException wex)
            {
                log.Error(wex);
                throw;
            }
        }
    }
}
