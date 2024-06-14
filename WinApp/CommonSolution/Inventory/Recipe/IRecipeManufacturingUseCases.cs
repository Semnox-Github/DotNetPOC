/********************************************************************************************
 * Project Name - Inventory  
 * Description  - IRecipeManufacturingUseCases class to get the data  from API by doing remote call  
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
  2.110.00     13-Nov-2020         Abhishek             Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Inventory.Recipe
{
    public interface IRecipeManufacturingUseCases
    {
        Task<List<RecipeManufacturingHeaderDTO>> GetRecipeManufacturings(List<KeyValuePair<RecipeManufacturingHeaderDTO.SearchByParameters, string>> parameters, bool loadChildRecords = false, bool activeChildRecords = false);
        Task<string> SaveRecipeManufacturings(List<RecipeManufacturingHeaderDTO> recipeManufacturingHeaderDTOList);
    }
}
