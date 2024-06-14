/********************************************************************************************
* Project Name - Achievements
* Description  - Specification of the LoyaltyAttribute use cases. 
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.120.00   04-Mar-2021   Roshan Devadiga        Created 
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Promotions
{
    public interface ILoyaltyAttributeUseCases
    {
        Task<List<LoyaltyAttributesDTO>> GetLoyaltyAttributes(List<KeyValuePair<LoyaltyAttributesDTO.SearchByParameters, string>> parameters);
        Task<string> SaveLoyaltyAttributes(List<LoyaltyAttributesDTO> loyaltyAttributeDTOList);
        Task<LoyaltyAttributeContainerDTOCollection> GetLoyaltyAttributeContainerDTOCollection(int siteId, string hash, bool rebuildCache);
    }
}
