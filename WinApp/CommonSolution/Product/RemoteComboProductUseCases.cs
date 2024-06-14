/********************************************************************************************
 * Project Name - Product
 * Description  - RemoteComboProductUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.140.00    14-Sep-2021       Prajwal                   Created : POS UI Redesign with REST API
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
    class RemoteComboProductUseCases : RemoteUseCases, IComboProductUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string ComboProduct_URL = "api/HR/ComboProduct";
        private const string ComboProduct_COUNT_URL = "api/HR/ComboProductCount";

        public RemoteComboProductUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<List<ComboProductDTO>> GetComboProduct(List<KeyValuePair<ComboProductDTO.SearchByParameters, string>>
                          parameters, int currentPage = 0, int pageSize = 0, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parameters);

            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("currentPage".ToString(), currentPage.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("pageSize".ToString(), pageSize.ToString()));
            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            try
            {
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                List<ComboProductDTO> result = await Get<List<ComboProductDTO>>(ComboProduct_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<ComboProductDTO.SearchByParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<ComboProductDTO.SearchByParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case ComboProductDTO.SearchByParameters.CHILD_PRODUCT_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("childProductId".ToString(), searchParameter.Value));
                        }
                        break;
                    case ComboProductDTO.SearchByParameters.COMBOPRODUCT_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("comboProductId".ToString(), searchParameter.Value));
                        }
                        break;
                    case ComboProductDTO.SearchByParameters.DISPLAY_GROUP_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("displayGroupId".ToString(), searchParameter.Value));
                        }
                        break;
                    case ComboProductDTO.SearchByParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                    case ComboProductDTO.SearchByParameters.PRODUCT_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("productId".ToString(), searchParameter.Value));
                        }
                        break;
                    case ComboProductDTO.SearchByParameters.PRICE_INCLUSIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("priceInclusive".ToString(), searchParameter.Value));
                        }
                        break;
                    case ComboProductDTO.SearchByParameters.ADDITIONAL_PRODUCT:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("additionalProduct".ToString(), searchParameter.Value));
                        }
                        break;
                    case ComboProductDTO.SearchByParameters.CHILD_PRODUCT_TYPE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("childProductType".ToString(), searchParameter.Value));
                        }
                        break;
                    case ComboProductDTO.SearchByParameters.CATEGORY_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("categoryId".ToString(), searchParameter.Value));
                        }
                        break;
            }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }

        public async Task<List<ComboProductDTO>> SaveComboProduct(List<ComboProductDTO> comboProductDTOList)
        {
            log.LogMethodEntry(comboProductDTOList);
            try
            {
                List<ComboProductDTO> responseString = await Post<List<ComboProductDTO>>(ComboProduct_URL, comboProductDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<string> DeleteComboProduct(List<ComboProductDTO> comboProductDTOList)
        {
            try
            {
                log.LogMethodEntry(comboProductDTOList);
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                string content = JsonConvert.SerializeObject(comboProductDTOList);
                string responseString = await Delete(ComboProduct_URL, content);
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

        public async Task<int> GetComboProductCount(List<KeyValuePair<ComboProductDTO.SearchByParameters, string>>
                         parameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parameters);

            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            try
            {
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                int result = await Get<int>(ComboProduct_COUNT_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

    }
}