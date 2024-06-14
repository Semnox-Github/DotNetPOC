/********************************************************************************************
 * Project Name - Achievements
 * Description  - LoyaltyAttributeUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.00    04-Mar-2021       Roshan Devadiga        Created : POS UI Redesign with REST API
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Promotions
{
    class RemoteLoyaltyAttributeUseCases : RemoteUseCases, ILoyaltyAttributeUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string LOYALTYATTRIBUTE_URL = "api/Promotion/LoyaltyAttributes";
        private const string LOYALTYATTRIBUTE_CONTAINER_URL = "api/Promotion/LoyaltyAttributesContainer";

        public RemoteLoyaltyAttributeUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<List<LoyaltyAttributesDTO>> GetLoyaltyAttributes(List<KeyValuePair<LoyaltyAttributesDTO.SearchByParameters, string>>
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
                List<LoyaltyAttributesDTO> result = await Get<List<LoyaltyAttributesDTO>>(LOYALTYATTRIBUTE_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<LoyaltyAttributesDTO.SearchByParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<LoyaltyAttributesDTO.SearchByParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case LoyaltyAttributesDTO.SearchByParameters.LOYALTY_ATTRIBUTE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("loyaltyAttributeId".ToString(), searchParameter.Value));
                        }
                        break;
                    case LoyaltyAttributesDTO.SearchByParameters.PURCHASE_APPLICABLE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("purchaseApplicable".ToString(), searchParameter.Value));
                        }
                        break;
                    case LoyaltyAttributesDTO.SearchByParameters.CONSUMPTION_APPLICABLE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("consumptionApplicable".ToString(), searchParameter.Value));
                        }
                        break;
                    case LoyaltyAttributesDTO.SearchByParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("activeFlag".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }
        public async Task<string> SaveLoyaltyAttributes(List<LoyaltyAttributesDTO> loyaltyAttributeDTOList)
        {
            log.LogMethodEntry(loyaltyAttributeDTOList);
            try
            {
                string responseString = await Post<string>(LOYALTYATTRIBUTE_URL, loyaltyAttributeDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        public async Task<LoyaltyAttributeContainerDTOCollection> GetLoyaltyAttributeContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(hash, rebuildCache);
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("siteId", siteId.ToString()));
            if (string.IsNullOrWhiteSpace(hash) == false)
            {
                parameters.Add(new KeyValuePair<string, string>("hash", hash));
            }
            parameters.Add(new KeyValuePair<string, string>("rebuildCache", rebuildCache.ToString()));
            LoyaltyAttributeContainerDTOCollection result = await Get<LoyaltyAttributeContainerDTOCollection>(LOYALTYATTRIBUTE_CONTAINER_URL, parameters);
            log.LogMethodExit(result);
            return result;
        }
    }
}
