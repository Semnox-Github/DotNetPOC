/********************************************************************************************
 * Project Name - Product
 * Description  - RemoteProductTypeUseCases class to get the data  from API by doing remote call  
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.1    24-Jun-2021          Abhishek           Created : POS UI Redesign with REST API
 2.140.0     14-09-2021          Prajwal s           Modified : Added use cases.
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    public class RemoteProductTypeUseCases : RemoteUseCases, IProductTypeUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string ProductType_URL = "api/Product/ProductType";
        private const string PRODUCT_TYPE_CONTAINER_URL = "api/Product/ProductTypeContainer";
        public RemoteProductTypeUseCases(ExecutionContext executionContext)
                : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }


        public async Task<List<ProductTypeDTO>> GetProductType(List<KeyValuePair<ProductTypeDTO.SearchByParameters, string>>
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
                List<ProductTypeDTO> result = await Get<List<ProductTypeDTO>>(ProductType_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<ProductTypeDTO.SearchByParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<ProductTypeDTO.SearchByParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case ProductTypeDTO.SearchByParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                    case ProductTypeDTO.SearchByParameters.PRODUCT_TYPE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("productTypeId".ToString(), searchParameter.Value));
                        }
                        break;
                    case ProductTypeDTO.SearchByParameters.ORDERTYPEID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("orderTypeId".ToString(), searchParameter.Value));
                        }
                        break;
                    case ProductTypeDTO.SearchByParameters.PRODUCT_TYPE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("productType".ToString(), searchParameter.Value));
                        }
                        break;
                    case ProductTypeDTO.SearchByParameters.SITE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("siteId".ToString(), searchParameter.Value));
                        }
                        break;

                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }
        public async Task<ProductTypeContainerDTOCollection> GetProductTypeContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(hash, rebuildCache);
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("siteId", siteId.ToString()));
            if (string.IsNullOrWhiteSpace(hash) == false)
            {
                parameters.Add(new KeyValuePair<string, string>("hash", hash));
            }
            parameters.Add(new KeyValuePair<string, string>("rebuildCache", rebuildCache.ToString()));
            ProductTypeContainerDTOCollection result = await Get<ProductTypeContainerDTOCollection>(PRODUCT_TYPE_CONTAINER_URL, parameters);
            log.LogMethodExit(result);
            return result;
        }
    }
}
