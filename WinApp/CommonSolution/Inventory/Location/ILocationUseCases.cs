/********************************************************************************************
 * Project Name - Inventory
 * Description  - ILocationUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 *2.110.0         09-Nov-2020       Mushahid Faizan         Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Semnox.Parafait.Inventory.Location
{
    public interface ILocationUseCases
    {
        Task<List<LocationDTO>> GetLocations(List<KeyValuePair<LocationDTO.SearchByLocationParameters, string>>
                          searchParameters, SqlTransaction sqlTransaction = null,
                          bool includeWastageLocation = false, int currentPage = 0, int pageSize = 0);
        Task<string> SaveLocations(List<LocationDTO> locationDTOList);
        Task<LocationContainerDTOCollection> GetLocationContainerDTOCollection(int siteId,string hash, bool rebuildCache);
        Task<int> GetLocationCount(List<KeyValuePair<LocationDTO.SearchByLocationParameters, string>>
                          searchParameters, SqlTransaction sqlTransaction = null);
    }
}
