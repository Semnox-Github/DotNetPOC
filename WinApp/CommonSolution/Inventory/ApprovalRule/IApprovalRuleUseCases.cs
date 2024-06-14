/********************************************************************************************
 * Project Name - Inventory
 * Description  - IApprovalRuleUseCases
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.110.0    14-Oct-2020   Mushahid Faizan Created 
 ********************************************************************************************/
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Semnox.Parafait.Inventory
{
    public interface IApprovalRuleUseCases
    {
        Task<List<ApprovalRuleDTO>> GetApprovalRules(List<KeyValuePair<ApprovalRuleDTO.SearchByApprovalRuleParameters, string>>
                         searchParameters, int currentPage = 0, int pageSize = 0);
        Task<string> SaveApprovalRules(List<ApprovalRuleDTO> locationDTOList);
        Task<int> GetApprovalRuleCount(List<KeyValuePair<ApprovalRuleDTO.SearchByApprovalRuleParameters, string>> searchParameters);

        Task<List<InventoryDocumentTypeDTO>> GetInventoryDocumentTypes(List<KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>> searchParameters);

        Task<InventoryDocumentTypeContainerDTOCollection> GetInventoryDocumentTypeContainerDTOCollection(int siteId, string hash, bool rebuildCache);

    }
}
