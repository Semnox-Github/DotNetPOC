/********************************************************************************************
 * Project Name - Inventory
 * Description  - IUOMUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         30-Nov-2020       Mushahid Faizan         Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Semnox.Parafait.Inventory
{
    public interface IUOMUseCases
    {
        Task<List<UOMDTO>> GetUOMs(List<KeyValuePair<UOMDTO.SearchByUOMParameters, string>>
                                           searchParameters, bool loadChildRecords, bool activeChildRecords = true, int currentPage = 0, int pageSize = 0);
        Task<string> SaveUOMs(List<UOMDTO> locationDTOList);
        Task<int> GetUOMCounts(List<KeyValuePair<UOMDTO.SearchByUOMParameters, string>>
                                           searchParameters);
    }
}
