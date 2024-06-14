/********************************************************************************************
 * Project Name - MembershipRule
 * Description  - IMembershipRuleUseCases class
 *
 **************
 ** Version Log
  **************
  * Version      Date              Modified By         Remarks
 *********************************************************************************************
 2.120.0         05-May-2021       B Mahesh Pai       Created
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Customer.Membership
{
    public interface IMembershipRuleUseCases
    {
        Task<List<MembershipRuleDTO>> GetMembershipRules(List<KeyValuePair<MembershipRuleDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null);
        Task<string> SaveMembershipRules(List<MembershipRuleDTO> membershipRuleDTOList);

    }
}
