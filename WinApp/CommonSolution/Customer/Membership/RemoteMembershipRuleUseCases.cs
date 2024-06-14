/********************************************************************************************
 * Project Name - MembershipRule
 * Description  - RemoteMembershipRuleUseCases class
 *
 **************
 ** Version Log
  **************
  * Version      Date              Modified By         Remarks
 *********************************************************************************************
 2.120.0         05-May-2021       B Mahesh Pai       Created
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Customer.Membership
{
    class RemoteMembershipRuleUseCases: RemoteUseCases, IMembershipRuleUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string MEMBERSHIPRULE_URL = "api / Customer / Membership / MembershipRules";
        public RemoteMembershipRuleUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }
        public async Task<List<MembershipRuleDTO>> GetMembershipRules(List<KeyValuePair<MembershipRuleDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);

            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();

            if (searchParameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(searchParameters));
            }
            try
            {
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                List<MembershipRuleDTO> result = await Get<List<MembershipRuleDTO>>(MEMBERSHIPRULE_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<MembershipRuleDTO.SearchByParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<MembershipRuleDTO.SearchByParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case MembershipRuleDTO.SearchByParameters.MEMBERSHIP_RULE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("MembershipRuleId".ToString(), searchParameter.Value));
                        }
                        break;
                    case MembershipRuleDTO.SearchByParameters.ISACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("IsActive".ToString(), searchParameter.Value));
                        }
                        break;
                  
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }
        public async Task<string> SaveMembershipRules(List<MembershipRuleDTO> MembershipRuleDTO)
        {
            log.LogMethodEntry(MembershipRuleDTO);
            try
            {
                string responseString = await Post<string>(MEMBERSHIPRULE_URL, MembershipRuleDTO);
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
