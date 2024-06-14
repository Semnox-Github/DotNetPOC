/********************************************************************************************
* Project Name - Promotions
* Description  - Specification of the LoyaltyRedemption use cases. 
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.120.00   05-Mar-2021   Roshan Devadiga        Created 
*2.120.00   25-Mar-2021       Fiona              Modified to add Delete UseCase
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Promotions
{
    public interface ILoyaltyRedemptionRuleUseCases
    {
        Task<List<LoyaltyRedemptionRuleDTO>> GetLoyaltyRedemptionRules(List<KeyValuePair<LoyaltyRedemptionRuleDTO.SearchByParameters, string>> parameters);
        Task<string> SaveLoyaltyRedemptionRules(List<LoyaltyRedemptionRuleDTO> loyaltyRedemptionRuleDTOList);
        Task<LoyaltyRedemptionRuleContainerDTOCollection> GetLoyaltyRedemptionRuleContainerDTOCollection(int siteId, string hash, bool rebuildCache);
        Task<string> Delete(List<LoyaltyRedemptionRuleDTO> loyaltyRedemptionRuleDTOList);
    }
}
