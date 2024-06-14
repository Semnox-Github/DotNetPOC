/********************************************************************************************
 * Project Name - Product
 * Description  - IMembershipExclusionRuleUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
*2.140.00   14-Sep-2021        B Mahesh Pai           Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    public interface IMembershipExclusionRuleUseCases
    {
        Task<List<MembershipExclusionRuleDTO>> GetMembershipExclusionRules(List<KeyValuePair<MembershipExclusionRuleDTO.SearchByParameters, string>>
                          searchParameters, bool isPopulate, string productId);
        Task<string> SaveMembershipExclusionRules(List<MembershipExclusionRuleDTO> membershipExclusionRuleDTOList);

        Task<string> DeleteMembershipExclusionRules(List<MembershipExclusionRuleDTO> membershipExclusionRuleDTOList);
    }
}