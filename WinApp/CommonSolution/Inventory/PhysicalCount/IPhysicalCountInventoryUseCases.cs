/********************************************************************************************
 * Project Name - Inventory
 * Description  - IPhysicalCountInventoryUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0      04-Jan-2021       Abhishek                 Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Inventory.PhysicalCount
{
    public interface IPhysicalCountInventoryUseCases
    {
        Task<List<PhysicalCountReviewDTO>> GetPhysicalCountReviews(List<KeyValuePair<PhysicalCountReviewDTO.SearchByPhysicalCountReviewParameters, string>> searchParameters, string advancedSearch, string filterText, int physicalCountId, DateTime startDate, int locationId, bool ismodifiedDuringPhysicalCount,
                                                                     int currentPage = 0, int pageSize = 10, SqlTransaction sqlTransaction = null);
        Task<string> SavePhysicalCountReviews(List<PhysicalCountReviewDTO> physicalCountReviewDTOList, int physicalCountId);
        Task<int> GetPhysicalCountInventoryCounts(List<KeyValuePair<PhysicalCountReviewDTO.SearchByPhysicalCountReviewParameters, string>> searchParameters,
                                                                       string advancedSearch, string filterText, int physicalCountId, DateTime startDate, int locationId,
                                                                       SqlTransaction sqlTransaction = null);
    }
}
