/********************************************************************************************
 * Project Name - Inventory  
 * Description  - IRecipeEstimationUseCases class to get the data  from API by doing remote call  
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.00     13-Nov-2020       Abhishek             Created : POS UI Redesign with REST API
 2.120.00     21-Apr-2021        Deeksha              Modified to add get for Build Forecast data
 2.120.00     05-May-2021        Mushahid Faizan      Web Inventory UI Changes
 2.130.0     15-Jun-2021        Mushahid Faizan      Web Inventory UI Changes
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Semnox.Parafait.Inventory.Recipe
{
    public interface IRecipeEstimationUseCases
    {
        Task<List<RecipeEstimationHeaderDTO>> GetRecipeEstimationHeaders(int recipeEstimationHeaderId = -1, DateTime? fromDate = null, DateTime? toDate = null, decimal? aspirationalPerc = null,
                                                    decimal? seasonalPerc = null, bool isEvent = false, int historicalDataInDays = 0, int eventOffset = 0, bool isFinishedItem = false,
                                                    bool isSemiFinishedItem = false, bool isActive = true);

        Task<List<RecipeEstimationHeaderDTO>> GetRecipeEstimations(List<KeyValuePair<RecipeEstimationHeaderDTO.SearchByParameters, string>> parameters, bool loadChildRecords = false, bool activeChildRecords = false);
        Task<List<RecipeEstimationHeaderDTO>> GetRecipeForecastingSummary(DateTime? fromDate = null, DateTime? toDate = null, decimal? aspirationalPerc = null,
                                                    decimal? seasonalPerc = null, bool isEvent = false, int historicalDataInDays = 0, int eventOffset = 0, bool isFinishedItem = false,
                                                    bool isSemiFinishedItem = false, string generateForecastData = null);
        Task<string> SaveRecipeEstimations(List<RecipeEstimationHeaderDTO> recipeEstimationHeaderDTOList);
        Task<string> CreatePlan(List<RecipeEstimationHeaderDTO> recipeEstimationHeaderDTOList);
        Task<string> DeleteRecipeEstimations(List<RecipeEstimationHeaderDTO> recipeEstimationHeaderDTOList);
    }
}
