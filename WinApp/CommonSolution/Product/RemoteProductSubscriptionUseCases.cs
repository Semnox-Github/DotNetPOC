/********************************************************************************************
 * Project Name - RemoteProductSubscriptionUseCases
 * Description  - RemoteProductSubscriptionUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date              Modified By        Remarks          
 *********************************************************************************************
 2.110.0      14-Dec-2020       Deeksha            Created :Inventory UI/POS UI re-design with REST API
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// RemoteProductSubscriptionUseCases
    /// </summary>
    public class RemoteProductSubscriptionUseCases : RemoteUseCases, IProductSubscriptionUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string PRODUCTS_SUBSCRIPTION_URL = "api/Product/ProductSubscriptions";
        /// <summary>
        /// RemoteProductSubscriptionUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        public RemoteProductSubscriptionUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }
        /// <summary>
        /// GetProductSubscription
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<List<ProductSubscriptionDTO>> GetProductSubscription(List<KeyValuePair<ProductSubscriptionDTO.SearchByParameters, string>> parameters)
        {
            log.LogMethodEntry(parameters);

            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            try
            {
                List<ProductSubscriptionDTO> result = await Get<List<ProductSubscriptionDTO>>(PRODUCTS_SUBSCRIPTION_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }         

        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<ProductSubscriptionDTO.SearchByParameters, string>> searchParams)
        {
            log.LogMethodEntry(searchParams);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<ProductSubscriptionDTO.SearchByParameters, string> searchParameter in searchParams)
            {
                switch (searchParameter.Key)
                {

                    case ProductSubscriptionDTO.SearchByParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break; 
                    case ProductSubscriptionDTO.SearchByParameters.PRODUCTS_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("productId".ToString(), searchParameter.Value));
                        }
                        break;
                    case ProductSubscriptionDTO.SearchByParameters.ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("productSubscriptionId".ToString(), searchParameter.Value));
                        }
                        break; 
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }
        /// <summary>
        /// SaveProductSubscription
        /// </summary>
        /// <param name="productSubscriptionDTOList"></param>
        /// <returns></returns>
        public async Task<string> SaveProductSubscription(List<ProductSubscriptionDTO> productSubscriptionDTOList)
        {
            log.LogMethodEntry(productSubscriptionDTOList);
            try
            {
                string result = await Post<string>(PRODUCTS_SUBSCRIPTION_URL, productSubscriptionDTOList);
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
