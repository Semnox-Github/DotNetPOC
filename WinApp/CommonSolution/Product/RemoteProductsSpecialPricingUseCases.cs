/********************************************************************************************
 * Project Name -Product
 * Description  -ProductsSpecialPricingUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.00    09-Mar-2021       Roshan Devadiga        Created : POS UI Redesign with REST API
 ********************************************************************************************/
using Newtonsoft.Json;
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    public class RemoteProductsSpecialPricingUseCases: RemoteUseCases, IProductsSpecialPricingUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string PRODUCTSSPECIALPRICING_URL = "api/Product/SpecialPricing";

        public RemoteProductsSpecialPricingUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<List<ProductsSpecialPricingDTO>> GetProductsSpecialPricings(List<KeyValuePair<ProductsSpecialPricingDTO.SearchByProductsSpecialPricingParameters, string>>
                         searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();

            if (searchParameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(searchParameters));
            }
            try
            {
                List<ProductsSpecialPricingDTO> result = await Get<List<ProductsSpecialPricingDTO>>(PRODUCTSSPECIALPRICING_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<ProductsSpecialPricingDTO.SearchByProductsSpecialPricingParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<ProductsSpecialPricingDTO.SearchByProductsSpecialPricingParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case ProductsSpecialPricingDTO.SearchByProductsSpecialPricingParameters.PRODUCT_PRICING_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("productPricingId".ToString(), searchParameter.Value));
                        }
                        break;
                    case ProductsSpecialPricingDTO.SearchByProductsSpecialPricingParameters.PRODUCT_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("productId".ToString(), searchParameter.Value));
                        }
                        break;
                    case ProductsSpecialPricingDTO.SearchByProductsSpecialPricingParameters.ACTIVE_FLAG:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("activeFlag".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }
        public async Task<string> SaveProductsSpecialPricings(List<ProductsSpecialPricingDTO> productsSpecialPricingList)
        {
            log.LogMethodEntry(productsSpecialPricingList);
            try
            {
                string responseString = await Post<string>(PRODUCTSSPECIALPRICING_URL, productsSpecialPricingList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        public async Task<string> Delete(List<ProductsSpecialPricingDTO> productsSpecialPricingList)
        {
            try
            {
                log.LogMethodEntry(productsSpecialPricingList);
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                string content = JsonConvert.SerializeObject(productsSpecialPricingList);
                string responseString = await Delete(PRODUCTSSPECIALPRICING_URL, content);
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
