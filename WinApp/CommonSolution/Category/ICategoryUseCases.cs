/********************************************************************************************
 * Project Name - Category 
 * Description  - ICategoryUseCases
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 ********************************************************************************************
 *2.110.0     07-Oct-2020   Mushahid Faizan    Created as per inventory changes,
 ********************************************************************************************/
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Semnox.Parafait.Category
{
    public interface ICategoryUseCases
    {
        Task<List<CategoryDTO>> GetCategories(List<KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>>
                                  searchParameters, bool buildChildRecords, bool loadActiveChild,
                                 int currentPage = 0, int pageSize = 0);
        Task<DataTable> GetColumnsName(string tableName);
        Task<string> SaveCategories(List<CategoryDTO> locationDTOList);
        Task<string> DeleteCategories(List<CategoryDTO> locationDTOList);
        Task<int> GetCategoryCount(List<KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>> searchParameters);

        Task<CategoryContainerDTOCollection> GetCategoryContainerDTOCollection(int siteId, string hash, bool rebuildCache);
    }
}
