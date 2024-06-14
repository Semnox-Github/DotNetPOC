/********************************************************************************************
 * Project Name - Inventory
 * Description  - ILocationTypeUseCases
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
using Semnox.Parafait.Inventory.Location;

namespace Semnox.Parafait.Inventory
{
    public interface ILocationTypeUseCases
    {
        Task<List<LocationTypeDTO>> GetLocationTypes(List<KeyValuePair<LocationTypeDTO.SearchByLocationTypeParameters, string>> searchParameters);
        Task<string> SaveLocationTypes(List<LocationTypeDTO> locationTypeDTOList);
        Task<LocationTypeContainerDTOCollection> GetLocationTypeContainerDTOCollection(int siteId, string hash, bool rebuildCache);
    }
}
