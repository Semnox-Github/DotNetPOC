/********************************************************************************************
 * Project Name - Product
 * Description  - RemoteProductDiscount useCase class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.00    30-Mar-2021       Roshan Devadiga        Created : POS UI Redesign with REST API
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
    public class RemoteProductDiscountUseCases : RemoteUseCases, IProductDiscountUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string PRODUCTDISCOUNT_URL = "api/Product/ProductDiscounts";
        public RemoteProductDiscountUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<List<ProductDiscountsDTO>> GetProductDiscounts(List<KeyValuePair<ProductDiscountsDTO.SearchByParameters, string>>
                         parameters)
        {
            log.LogMethodEntry(parameters);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();

            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            try
            {
                List<ProductDiscountsDTO> result = await Get<List<ProductDiscountsDTO>>(PRODUCTDISCOUNT_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<ProductDiscountsDTO.SearchByParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<ProductDiscountsDTO.SearchByParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case ProductDiscountsDTO.SearchByParameters.DISCOUNT_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("discountId".ToString(), searchParameter.Value));
                        }
                        break;
                    case ProductDiscountsDTO.SearchByParameters.PRODUCT_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("productId".ToString(), searchParameter.Value));
                        }
                        break;
                    case ProductDiscountsDTO.SearchByParameters.PRODUCT_DISCOUNT_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("productDiscountId".ToString(), searchParameter.Value));
                        }
                        break;
                    case ProductDiscountsDTO.SearchByParameters.EXPIRY_DATE_GREATER_THAN:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("expiryDateGreaterThan".ToString(), searchParameter.Value));
                        }
                        break;
                    case ProductDiscountsDTO.SearchByParameters.EXPIRY_DATE_LESS_THAN:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("expiryDateLessThan".ToString(), searchParameter.Value));
                        }
                        break;
                    case ProductDiscountsDTO.SearchByParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }
        public async Task<string> SaveProductDiscounts(List<ProductDiscountsDTO> productDiscountsDTOList)
        {
            log.LogMethodEntry(productDiscountsDTOList);
            try
            {
                string responseString = await Post<string>(PRODUCTDISCOUNT_URL, productDiscountsDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        public async Task<string> Delete(List<ProductDiscountsDTO> productDiscountsDTOList)
        {
            try
            {
                log.LogMethodEntry(productDiscountsDTOList);
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                string content = JsonConvert.SerializeObject(productDiscountsDTOList);
                string responseString = await Delete(PRODUCTDISCOUNT_URL, content);
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
