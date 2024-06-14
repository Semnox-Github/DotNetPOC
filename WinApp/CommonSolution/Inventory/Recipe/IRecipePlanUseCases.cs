/********************************************************************************************
 * Project Name - Inventory  
 * Description  - IRecipePlanUseCases class to get the data  from API by doing remote call  
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.00     13-Nov-2020         Abhishek             Created : POS UI Redesign with REST API
 2.130.0     13-Jun-2021         Mushahid Faizan       Modified : Web Inventory UI Changes.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Inventory.Recipe
{
    public interface IRecipePlanUseCases
    {
        Task<List<RecipePlanHeaderDTO>> GetRecipePlans(List<KeyValuePair<RecipePlanHeaderDTO.SearchByParameters, string>> parameters, bool loadChildRecords = false, bool activeChildRecords = false);
        Task<string> SaveRecipePlans(List<RecipePlanHeaderDTO> recipePlanHeaderDTOList);
        Task<string> CreateKPN(List<RecipePlanDetailsDTO> recipeManufacturingHeaderDTOList, int planHeaderId);

    }
}
