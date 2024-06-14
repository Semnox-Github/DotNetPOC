/********************************************************************************************
 * Project Name - Inventory
 * Description  - ISegmentDefinitionUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0         09-Nov-2020       Mushahid Faizan         Created 
 ********************************************************************************************/

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    public interface ISegmentDefinitionUseCases
    {
        Task<List<SegmentDefinitionDTO>> GetSegmentDefinitions(List<KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>>
             searchParameters, bool buildChildRecords, bool loadActiveChild, int currentPage = 0, int pageSize = 0);
        Task<string> SaveSegmentDefinitions(List<SegmentDefinitionDTO> locationDTOList);
        Task<int> GetSegmentCount(List<KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>>
             searchParameters);
    }
}
