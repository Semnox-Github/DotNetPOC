/********************************************************************************************
* Project Name - Products
* Description  - RemoteProductsDisplayGroupUseCases  class 
*  
**************
**Version Log
**************
*Version     Date             Modified By               Remarks          
*********************************************************************************************
2.120.00    06-Apr-2021       B Mahesh Pai        Created : POS UI Redesign with REST API
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
    public class RemoteProductsDisplayGroupUseCases: RemoteUseCases, IProductDisplayGroupUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string PRODUCTSDISPLAYGROUP_URL = "api/Product/ProductDisplayGroups";


        public RemoteProductsDisplayGroupUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }
        public async Task<List<ProductsDisplayGroupDTO>> GetProductsDisplayGroups(List<KeyValuePair<ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters, string>> parameters, SqlTransaction sqlTransaction = null)
                        
        {
            log.LogMethodEntry(parameters);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();

            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            try
            {
                List<ProductsDisplayGroupDTO> result = await Get<List<ProductsDisplayGroupDTO>>(PRODUCTSDISPLAYGROUP_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters.ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("Id".ToString(), searchParameter.Value));
                        }
                        break;
                    case ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters.PRODUCT_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("ProductId".ToString(), searchParameter.Value));
                        }
                        break;
                    case ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters.DISPLAYGROUP_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("DisplayGroupId".ToString(), searchParameter.Value));
                        }
                        break;
                    case ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters.DISPLAYGROUP_ID_LIST:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("DisplayGroupIdList".ToString(), searchParameter.Value));
                        }
                        break;
                    case ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                  
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }
        public async Task<string> SaveProductsDisplayGroups(List<ProductsDisplayGroupDTO> productsDisplayGroupDTOList)
        {
            log.LogMethodEntry(productsDisplayGroupDTOList);
            try
            {
                string responseString = await Post<string>(PRODUCTSDISPLAYGROUP_URL, productsDisplayGroupDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<string> Delete(List<ProductsDisplayGroupDTO> productsDisplayGroupDTOList)
        {
            try
            {
                log.LogMethodEntry(productsDisplayGroupDTOList);
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                string content = JsonConvert.SerializeObject(productsDisplayGroupDTOList);
                string responseString = await Delete(PRODUCTSDISPLAYGROUP_URL, content);
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
    }
}
