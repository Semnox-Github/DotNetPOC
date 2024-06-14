/********************************************************************************************
 * Project Name - Category 
 * Description  - RemoteCategoryUseCases
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 ********************************************************************************************
 *2.110.0     07-Oct-2020   Mushahid Faizan    Created as per inventory changes,
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Category
{
    public class RemoteCategoryUseCases : RemoteUseCases, ICategoryUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string CATEGORY_URL = "api/Inventory/Categories";
        private const string COLUMN_NAME_URL = "api/Inventory/TableColumns";
        private const string CATEGORY_COUNT_URL = "api/Inventory/CategoryCounts";
        private const string CATEGORY_CONTAINER_URL = "api/Inventory/CategoryContainer";

        public RemoteCategoryUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<List<CategoryDTO>> GetCategories(List<KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>>
                          parameters, bool buildChildRecords, bool loadActiveChild, int currentPage = 0, int pageSize = 0)
        {
            log.LogMethodEntry(parameters);

            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("buildChildRecords".ToString(), buildChildRecords.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("loadActiveChild".ToString(), loadActiveChild.ToString()));
            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            try
            {
                List<CategoryDTO> result = await Get<List<CategoryDTO>>(CATEGORY_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<int> GetCategoryCount(List<KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>> parameters)
        {
            log.LogMethodEntry(parameters);

            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            try
            {
                int result = await Get<int>(CATEGORY_COUNT_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>> lookupSearchParams)
        {

            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<CategoryDTO.SearchByCategoryParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case CategoryDTO.SearchByCategoryParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                    case CategoryDTO.SearchByCategoryParameters.NAME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("categoryName".ToString(), searchParameter.Value));
                        }
                        break;
                    case CategoryDTO.SearchByCategoryParameters.CATEGORY_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("categoryId".ToString(), searchParameter.Value));
                        }
                        break;

                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }

        public async Task<DataTable> GetColumnsName(string tableName)
        {
            log.LogMethodEntry(tableName);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("tableName".ToString(), tableName));

            dynamic result = null;
            try
            {
                if (tableName.ToUpper() == "CATEGORY")
                {
                    RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                    result = await Get<dynamic>(COLUMN_NAME_URL, searchParameterList);
                    log.LogMethodExit(result);
                    return result;
                }
                else if (tableName.ToUpper() == "VENDOR")
                {
                    RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                    result = await Get<dynamic>(COLUMN_NAME_URL, searchParameterList);
                    log.LogMethodExit(result);
                    return result;
                }
                else
                {
                    log.Debug("Invalid TableName");
                    return result;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<string> SaveCategories(List<CategoryDTO> categoryDTOList)
        {
            log.LogMethodEntry(categoryDTOList);
            try
            {
                string responseString = await Post<string>(CATEGORY_URL, categoryDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        public async Task<string> DeleteCategories(List<CategoryDTO> categoryDTOList)
        {
            log.LogMethodEntry(categoryDTOList);
            try
            {
                string responseString = await Delete<string>(CATEGORY_URL, categoryDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }


        public async Task<CategoryContainerDTOCollection> GetCategoryContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(hash, rebuildCache);
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("siteId", siteId.ToString()));
            if (string.IsNullOrWhiteSpace(hash) == false)
            {
                parameters.Add(new KeyValuePair<string, string>("hash", hash));
            }
            parameters.Add(new KeyValuePair<string, string>("rebuildCache", rebuildCache.ToString()));
            CategoryContainerDTOCollection result = await Get<CategoryContainerDTOCollection>(CATEGORY_CONTAINER_URL, parameters);
            log.LogMethodExit(result);
            return result;
        }

    }
}
