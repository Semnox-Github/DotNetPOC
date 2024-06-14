/********************************************************************************************
 * Project Name - Inventory
 * Description  - RemoteRecipeManufacturingUseCases class to get the data  from API by doing remote call  
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.00      16-Nov-2020        Abhishek             Created : POS UI Redesign with REST API
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Semnox.Core.Utilities;
namespace Semnox.Parafait.Inventory.Recipe
{
    public class RemoteRecipeManufacturingUseCases : RemoteUseCases, IRecipeManufacturingUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string RECIPE_MANUFACTURING_URL = "api/Inventory/Recipe/RecipeManufacturings";
        
        public RemoteRecipeManufacturingUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<List<RecipeManufacturingHeaderDTO>> GetRecipeManufacturings(List<KeyValuePair<RecipeManufacturingHeaderDTO.SearchByParameters, string>> parameters, bool loadChildRecords = false, bool activeChildRecords = false)
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
                List<RecipeManufacturingHeaderDTO> recipeManufacturingHeaderDTOList = await Get<List<RecipeManufacturingHeaderDTO>>(RECIPE_MANUFACTURING_URL, searchParameterList);
                log.LogMethodExit(recipeManufacturingHeaderDTOList);
                return recipeManufacturingHeaderDTOList;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<RecipeManufacturingHeaderDTO.SearchByParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<RecipeManufacturingHeaderDTO.SearchByParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {
                    case RecipeManufacturingHeaderDTO.SearchByParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                    case RecipeManufacturingHeaderDTO.SearchByParameters.SITE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("siteId".ToString(), searchParameter.Value));
                        }
                        break;
                    case RecipeManufacturingHeaderDTO.SearchByParameters.MASTER_ENTITY_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("masterEntityId".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }

        public async Task<string> SaveRecipeManufacturings(List<RecipeManufacturingHeaderDTO> recipeManufacturingHeaderDTOList)
        {
            log.LogMethodEntry(recipeManufacturingHeaderDTOList);
            try
            {
                string responseString = await Post<string>(RECIPE_MANUFACTURING_URL, recipeManufacturingHeaderDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
    }
}
