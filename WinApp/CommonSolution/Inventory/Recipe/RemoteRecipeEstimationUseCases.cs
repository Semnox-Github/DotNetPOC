/********************************************************************************************
 * Project Name - Inventory
 * Description  - RemoteRecipeEstimationUseCases class to get the data  from API by doing remote call  
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.00      16-Nov-2020        Abhishek             Created : POS UI Redesign with REST API
 2.130.00      21-Apr-2021        Deeksha              Modified to add get for Build Forecast data
 2.130.00     15-Jun-2021        Mushahid Faizan      Web Inventory UI Changes
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using Semnox.Core.Utilities;
using System.Threading.Tasks;

namespace Semnox.Parafait.Inventory.Recipe
{
    public class RemoteRecipeEstimationUseCases : RemoteUseCases, IRecipeEstimationUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string RECIPE_ESTIMATION_URL = "api/Inventory/Recipe/RecipeEstimations";
        private const string CREATE_PLAN = "api/Inventory/Recipe/RecipeEstimationPlans";

        public RemoteRecipeEstimationUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<List<RecipeEstimationHeaderDTO>> GetRecipeEstimations(List<KeyValuePair<RecipeEstimationHeaderDTO.SearchByParameters, string>> parameters, bool loadChildRecords = false, bool activeChildRecords = false)
        {
            log.LogMethodEntry(parameters);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("loadChildRecords".ToString(), loadChildRecords.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("activeChildRecords".ToString(), activeChildRecords.ToString()));
            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            try
            {
                List<RecipeEstimationHeaderDTO> recipeEstimationHeaderDTOList = await Get<List<RecipeEstimationHeaderDTO>>(RECIPE_ESTIMATION_URL, searchParameterList);
                log.LogMethodExit(recipeEstimationHeaderDTOList);
                return recipeEstimationHeaderDTOList;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<RecipeEstimationHeaderDTO.SearchByParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<RecipeEstimationHeaderDTO.SearchByParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {
                    case RecipeEstimationHeaderDTO.SearchByParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                    case RecipeEstimationHeaderDTO.SearchByParameters.SITE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("siteId".ToString(), searchParameter.Value));
                        }
                        break;
                    case RecipeEstimationHeaderDTO.SearchByParameters.MASTER_ENTITY_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("masterEntityId".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }

        public async Task<string> SaveRecipeEstimations(List<RecipeEstimationHeaderDTO> recipeEstimationHeaderDTOList)
        {
            log.LogMethodEntry(recipeEstimationHeaderDTOList);
            try
            {
                string responseString = await Post<string>(RECIPE_ESTIMATION_URL, recipeEstimationHeaderDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
          public async Task<string> CreatePlan(List<RecipeEstimationHeaderDTO> recipeEstimationHeaderDTOList)
        {
            log.LogMethodEntry(recipeEstimationHeaderDTOList);
            try
            {
                string responseString = await Post<string>(CREATE_PLAN, recipeEstimationHeaderDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<string> DeleteRecipeEstimations(List<RecipeEstimationHeaderDTO> recipeEstimationHeaderDTOList)
        {
            log.LogMethodEntry(recipeEstimationHeaderDTOList);
            try
            {
                string responseString = await Delete<string>(RECIPE_ESTIMATION_URL, recipeEstimationHeaderDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<List<RecipeEstimationHeaderDTO>> GetRecipeEstimationHeaders(int recipeEstimationHeaderId = -1, DateTime? fromDate = null, DateTime? toDate = null, decimal? aspirationalPerc = null,
                                                    decimal? seasonalPerc = null, bool isEvent = false, int historicalDataInDays = 0, int eventOffset = 0, bool isFinishedItem = false,
                                                    bool isSemiFinishedItem = false, bool isActive = true)
        {
            log.LogMethodEntry();
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("recipeEstimationHeaderId".ToString(), recipeEstimationHeaderId.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("fromDate".ToString(), fromDate.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("toDate".ToString(), toDate.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("aspirationalPerc".ToString(), aspirationalPerc.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("seasonalPerc".ToString(), seasonalPerc.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("isEvent".ToString(), isEvent.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("historicalDataInDays".ToString(), historicalDataInDays.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("eventOffset".ToString(), eventOffset.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("isFinishedItem".ToString(), isFinishedItem.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("isSemiFinishedItem".ToString(), isSemiFinishedItem.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), isActive.ToString()));
            try
            {
                List<RecipeEstimationHeaderDTO> recipeEstimationHeaderDTOList = await Get<List<RecipeEstimationHeaderDTO>>(RECIPE_ESTIMATION_URL, searchParameterList);
                log.LogMethodExit(recipeEstimationHeaderDTOList);
                return recipeEstimationHeaderDTOList;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        public async Task<List<RecipeEstimationHeaderDTO>> GetRecipeForecastingSummary( DateTime? fromDate = null, DateTime? toDate = null, decimal? aspirationalPerc = null,
                                                    decimal? seasonalPerc = null, bool isEvent = false, int historicalDataInDays = 0, int eventOffset = 0, bool isFinishedItem = false,
                                                    bool isSemiFinishedItem = false, string generateForecastData = null)
        {
            log.LogMethodEntry();
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            //searchParameterList.Add(new KeyValuePair<string, string>("recipeEstimationHeaderId".ToString(), recipeEstimationHeaderId.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("fromDate".ToString(), fromDate.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("toDate".ToString(), toDate.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("aspirationalPerc".ToString(), aspirationalPerc.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("seasonalPerc".ToString(), seasonalPerc.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("isEvent".ToString(), isEvent.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("historicalDataInDays".ToString(), historicalDataInDays.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("eventOffset".ToString(), eventOffset.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("isFinishedItem".ToString(), isFinishedItem.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("isSemiFinishedItem".ToString(), isSemiFinishedItem.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("generateForecastData".ToString(), generateForecastData.ToString()));
            try
            {
                List<RecipeEstimationHeaderDTO> recipeEstimationHeaderDTOList = await Get<List<RecipeEstimationHeaderDTO>>(RECIPE_ESTIMATION_URL, searchParameterList);
                log.LogMethodExit(recipeEstimationHeaderDTOList);
                return recipeEstimationHeaderDTOList;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
    }
}
