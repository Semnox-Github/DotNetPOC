/********************************************************************************************
 * Project Name - Inventory
 * Description  - RemoteRecipePlanUseCases class to get the data  from API by doing remote call  
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.00      16-Nov-2020        Abhishek             Created : POS UI Redesign with REST API
 2.130.0     13-Jun-2021         Mushahid Faizan       Modified : Web Inventory UI Changes.
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
    public class RemoteRecipePlanUseCases : RemoteUseCases, IRecipePlanUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string RECIPE_PLAN_URL = "api/Inventory/Recipe/RecipePlans";
        private const string CREATE_KPN_URL = "api/Inventory/Recipe/RecipePlans/{planHeaderId}/PlanDetails";

        public RemoteRecipePlanUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<List<RecipePlanHeaderDTO>> GetRecipePlans(List<KeyValuePair<RecipePlanHeaderDTO.SearchByParameters, string>> parameters, bool loadChildRecords = false, bool activeChildRecords = false)
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
                List<RecipePlanHeaderDTO> recipePlanHeaderDTOList = await Get<List<RecipePlanHeaderDTO>>(RECIPE_PLAN_URL, searchParameterList);
                log.LogMethodExit(recipePlanHeaderDTOList);
                return recipePlanHeaderDTOList;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<RecipePlanHeaderDTO.SearchByParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<RecipePlanHeaderDTO.SearchByParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {
                    case RecipePlanHeaderDTO.SearchByParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                    case RecipePlanHeaderDTO.SearchByParameters.SITE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("siteId".ToString(), searchParameter.Value));
                        }
                        break;
                    case RecipePlanHeaderDTO.SearchByParameters.MASTER_ENTITY_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("masterEntityId".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }

        public async Task<string> SaveRecipePlans(List<RecipePlanHeaderDTO> recipePlanHeaderDTOList)
        {
            log.LogMethodEntry(recipePlanHeaderDTOList);
            try
            {
                string responseString = await Post<string>(RECIPE_PLAN_URL, recipePlanHeaderDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        public async Task<string> CreateKPN(List<RecipePlanDetailsDTO> recipePlanDetailsDTOList, int planHeaderId)
        {
            log.LogMethodEntry(recipePlanDetailsDTOList);
            try
            {
                string responseString = await Post<string>(CREATE_KPN_URL, recipePlanDetailsDTOList);
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
