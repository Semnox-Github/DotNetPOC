/********************************************************************************************
 * Project Name -Product
 * Description  -ModifierSetUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.140.00    08-Mar-2021       Roshan Devadiga        Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System;
using Newtonsoft.Json;
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    class RemoteModifierSetUseCases : RemoteUseCases, IModifierSetUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string MODIFIERSET_URL = "api/Product/ProductsModifierSets";
        private const string MODIFIERSET_CONTAINER_URL = "api/Product/ProductsModifierSetContainer";

        public RemoteModifierSetUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<List<ModifierSetDTO>> GetModifierSets(List<KeyValuePair<ModifierSetDTO.SearchByParameters, string>>
                      parameters, bool loadChildRecords = false, bool loadActiveChildRecords = false, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parameters, loadChildRecords, loadActiveChildRecords, sqlTransaction);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("loadChildRecords".ToString(), loadChildRecords.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("loadActiveChildRecords".ToString(), loadActiveChildRecords.ToString()));

            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            try
            {
                List<ModifierSetDTO> result = await Get<List<ModifierSetDTO>>(MODIFIERSET_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<ModifierSetDTO.SearchByParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<ModifierSetDTO.SearchByParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case ModifierSetDTO.SearchByParameters.MODIFIER_SET_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>(" modifierSetId".ToString(), searchParameter.Value));
                        }
                        break;
                    case ModifierSetDTO.SearchByParameters.MODIFIER_SET_NAME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("setName".ToString(), searchParameter.Value));
                        }
                        break;
                    case ModifierSetDTO.SearchByParameters.ISACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                    case ModifierSetDTO.SearchByParameters.MODIFIER_SET_ID_LIST:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>(" modifierSetIdList".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }

        public async Task<string> SaveModifierSets(List<ModifierSetDTO> modifierSetDTOList)
        {
            log.LogMethodEntry(modifierSetDTOList);
            try
            {
                string responseString = await Post<string>(MODIFIERSET_URL, modifierSetDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<string> Delete(List<ModifierSetDTO> modifierSetDTOList)
        {
            try
            {
                log.LogMethodEntry(modifierSetDTOList);
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                string content = JsonConvert.SerializeObject(modifierSetDTOList);
                string responseString = await Delete(MODIFIERSET_URL, content);
                dynamic response = JsonConvert.DeserializeObject(responseString);
                log.LogMethodExit(response);
                return response;
            }
            catch (WebApiException wex)
            {
                log.Error(wex);
                throw;
            }
        }

        public async Task<ModifierSetContainerDTOCollection> GetModifierSetContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(hash, rebuildCache);
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("siteId", siteId.ToString()));
            if (string.IsNullOrWhiteSpace(hash) == false)
            {
                parameters.Add(new KeyValuePair<string, string>("hash", hash));
            }
            parameters.Add(new KeyValuePair<string, string>("rebuildCache", rebuildCache.ToString()));
            ModifierSetContainerDTOCollection result = await Get<ModifierSetContainerDTOCollection>(MODIFIERSET_CONTAINER_URL, parameters);
            log.LogMethodExit(result);
            return result;
        }

    }
}
