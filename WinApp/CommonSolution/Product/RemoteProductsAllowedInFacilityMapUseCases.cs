/********************************************************************************************
* Project Name - User
* Description  - RemoteProductsAllowedInFacilityMap  class 
*  
**************
**Version Log
**************
*Version     Date             Modified By               Remarks          
*********************************************************************************************
2.120.00    06-Apr-2021       B Mahesh Pai        Created : POS UI Redesign with REST API
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
   public class RemoteProductsAllowedInFacilityMapUseCases: RemoteUseCases, IProductsAllowedInFacilityMapUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string PRODUCTSALLOWEDINFACILITYMAP_URL = "api/Product/ProductsAllowedInFacilityMaps";



        public RemoteProductsAllowedInFacilityMapUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<List<ProductsAllowedInFacilityMapDTO>> GetProductsAllowedInFacilityMaps(List<KeyValuePair<ProductsAllowedInFacilityMapDTO.SearchByParameters, string>>
                         parameters, bool loadProducts = false, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parameters);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();

            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            try
            {
                List<ProductsAllowedInFacilityMapDTO> result = await Get<List<ProductsAllowedInFacilityMapDTO>>(PRODUCTSALLOWEDINFACILITYMAP_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<ProductsAllowedInFacilityMapDTO.SearchByParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<ProductsAllowedInFacilityMapDTO.SearchByParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case ProductsAllowedInFacilityMapDTO.SearchByParameters.PRODUCTS_ALLOWED_IN_FACILITY_MAP_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("productAllowedInFacility".ToString(), searchParameter.Value));
                        }
                        break;
                    case ProductsAllowedInFacilityMapDTO.SearchByParameters.FACILITY_MAP_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("facilityMapId".ToString(), searchParameter.Value));
                        }
                        break;
                    case ProductsAllowedInFacilityMapDTO.SearchByParameters.FACILITY_MAP_ID_LIST:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("FacilityMapIdList".ToString(), searchParameter.Value));
                        }
                        break;
                    case ProductsAllowedInFacilityMapDTO.SearchByParameters.PRODUCTS_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("ProductId".ToString(), searchParameter.Value));
                        }
                        break;
                    case ProductsAllowedInFacilityMapDTO.SearchByParameters.DEFAULT_RENTAL_PRODUCT:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("DefaultRentalProduct".ToString(), searchParameter.Value));
                        }
                        break;
                    case ProductsAllowedInFacilityMapDTO.SearchByParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                    case ProductsAllowedInFacilityMapDTO.SearchByParameters.HAVING_PRODUCT_TYPES_IN:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("HavingProductTypesIn".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }
        public async Task<string> SaveProductsAllowedInFacilityMaps(List<ProductsAllowedInFacilityMapDTO> productsAllowedInFacilityMapDTO)
        {
            log.LogMethodEntry(productsAllowedInFacilityMapDTO);
            try
            {
                string responseString = await Post<string>(PRODUCTSALLOWEDINFACILITYMAP_URL, productsAllowedInFacilityMapDTO);
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
