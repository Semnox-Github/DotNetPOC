/********************************************************************************************
 * Project Name - Product
 * Description  - ProductDisplayGroupFormatUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.140.00    30-Mar-2021       Roshan Devadiga        Created : POS UI Redesign with REST API
 ********************************************************************************************/

using Newtonsoft.Json;
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    public class RemoteProductDisplayGroupFormatUseCases:RemoteUseCases,IProductDisplayGroupFormatUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string PRODUCTDISPLAYGROUPFORMAT_URL = "api/Product/FormatDisplayGroups";
        private const string PRODUCTAVAILABILITY_URL = "api/Product/AvailableProducts";
        private const string PRODUCT_DISPLAY_GROUP_FORMAT_CONTAINER_URL = "api/Product/ProductDisplayGroupFormatContainer";
        public RemoteProductDisplayGroupFormatUseCases(ExecutionContext executionContext)
           : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<List<ProductDisplayGroupFormatDTO>> GetProductDisplayGroupFormats(List<KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>>
                         parameters, bool loadChildRecords = false, bool loadActiveChildRecords = false, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parameters,loadChildRecords, loadActiveChildRecords, sqlTransaction);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("loadChildRecords".ToString(), loadChildRecords.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("loadactiveChildRecords".ToString(), loadActiveChildRecords.ToString()));                
            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            try
            {
                List<ProductDisplayGroupFormatDTO> result = await Get<List<ProductDisplayGroupFormatDTO>>(PRODUCTDISPLAYGROUPFORMAT_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<List<ProductDisplayGroupFormatDTO>> GetConfiguredDisplayGroupListForLogin(string loginId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(loginId,sqlTransaction);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("loginId".ToString(), loginId.ToString())); 
            try
            {
                List<ProductDisplayGroupFormatDTO> result = await Get<List<ProductDisplayGroupFormatDTO>>(PRODUCTAVAILABILITY_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }


        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case ProductDisplayGroupFormatDTO.SearchByDisplayParameters.DISPLAY_GROUP:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("displayGroup".ToString(), searchParameter.Value));
                        }
                        break;
                    case ProductDisplayGroupFormatDTO.SearchByDisplayParameters.DISPLAY_GROUP_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("id".ToString(), searchParameter.Value));
                        }
                        break;
                    case ProductDisplayGroupFormatDTO.SearchByDisplayParameters.DISPLAY_GROUP_FORMAT_ID_LIST:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("displayGroupIdList".ToString(), searchParameter.Value));
                        }
                        break;
                    case ProductDisplayGroupFormatDTO.SearchByDisplayParameters.POS_MACHINE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("posMachineId".ToString(), searchParameter.Value));
                        }
                        break;
                    case ProductDisplayGroupFormatDTO.SearchByDisplayParameters.SORT_ORDER:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("sortOrder".ToString(), searchParameter.Value));
                        }
                        break;
                    case ProductDisplayGroupFormatDTO.SearchByDisplayParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }
        public async Task<string> SaveProductDisplayGroupFormats(List<ProductDisplayGroupFormatDTO> productDisplayGroupFormatDTOList)
        {
            log.LogMethodEntry(productDisplayGroupFormatDTOList);
            try
            {
                string responseString = await Post<string>(PRODUCTDISPLAYGROUPFORMAT_URL, productDisplayGroupFormatDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        public async Task<string> Delete(List<ProductDisplayGroupFormatDTO> productDisplayGroupFormatDTOList)
        {
            try
            {
                log.LogMethodEntry(productDisplayGroupFormatDTOList);
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                string content = JsonConvert.SerializeObject(productDisplayGroupFormatDTOList);
                string responseString = await Delete(PRODUCTDISPLAYGROUPFORMAT_URL, content);
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

        public async Task<ProductDisplayGroupFormatContainerDTOCollection> GetProductDisplayGroupFormatContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(hash, rebuildCache);
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("siteId", siteId.ToString()));
            if (string.IsNullOrWhiteSpace(hash) == false)
            {
                parameters.Add(new KeyValuePair<string, string>("hash", hash));
            }
            parameters.Add(new KeyValuePair<string, string>("rebuildCache", rebuildCache.ToString()));
            ProductDisplayGroupFormatContainerDTOCollection result = await Get<ProductDisplayGroupFormatContainerDTOCollection>(PRODUCT_DISPLAY_GROUP_FORMAT_CONTAINER_URL, parameters);
            log.LogMethodExit(result);
            return result;
        }

    }
}
