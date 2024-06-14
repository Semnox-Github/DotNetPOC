/********************************************************************************************
 * Project Name - Product
 * Description  - RemoteMembershipExclusionRuleUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.140.0     30-Mar-2021       B Mahesh Pai         Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System;
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Semnox.Parafait.Product
{
   public class RemoteMembershipExclusionRuleUseCases:RemoteUseCases,IMembershipExclusionRuleUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string MEMBERSHIPEXCLUSIONRULE_URL = "api/Product/MembershipExclusionRules";
       

        public RemoteMembershipExclusionRuleUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }
        public async Task<List<MembershipExclusionRuleDTO>> GetMembershipExclusionRules(List<KeyValuePair<MembershipExclusionRuleDTO.SearchByParameters, string>>
                          searchParameters, bool isPopulate, string productId)
        {
            log.LogMethodEntry(searchParameters);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();

            if (searchParameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(searchParameters));
            }
            try
            {
                List<MembershipExclusionRuleDTO> result = await Get<List<MembershipExclusionRuleDTO>>(MEMBERSHIPEXCLUSIONRULE_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<MembershipExclusionRuleDTO.SearchByParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<MembershipExclusionRuleDTO.SearchByParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case MembershipExclusionRuleDTO.SearchByParameters.ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("id".ToString(), searchParameter.Value));
                        }
                        break;
                    case MembershipExclusionRuleDTO.SearchByParameters.PRODUCT_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("productId".ToString(), searchParameter.Value));
                        }
                        break;
                    case MembershipExclusionRuleDTO.SearchByParameters.GAME_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("gameId".ToString(), searchParameter.Value));
                        }
                        break;
                    case MembershipExclusionRuleDTO.SearchByParameters.GAME_PROFILE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("gameProfileId".ToString(), searchParameter.Value));
                        }
                        break;
                    case MembershipExclusionRuleDTO.SearchByParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                    case MembershipExclusionRuleDTO.SearchByParameters.DISALLOWED:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("disallowed".ToString(), searchParameter.Value));
                        }
                        break;
                    case MembershipExclusionRuleDTO.SearchByParameters.MEMBERSHIP_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("membershipId".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }
        public async Task<string> SaveMembershipExclusionRules(List<MembershipExclusionRuleDTO> membershipExclusionRuleDTOList)
        {
            log.LogMethodEntry(membershipExclusionRuleDTOList);
            try
            {
                string responseString = await Post <string>(MEMBERSHIPEXCLUSIONRULE_URL, membershipExclusionRuleDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<string> DeleteMembershipExclusionRules(List<MembershipExclusionRuleDTO> membershipExclusionRuleDTOList)
        {
            try
            {
                log.LogMethodEntry(membershipExclusionRuleDTOList);
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                string content = JsonConvert.SerializeObject(membershipExclusionRuleDTOList);
                string responseString = await Delete(MEMBERSHIPEXCLUSIONRULE_URL, content);
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
