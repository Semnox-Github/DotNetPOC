/********************************************************************************************
 * Project Name -Product
 * Description  -ProductsCalenderUseCases class 
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
    class RemoteProductsCalenderUseCases : RemoteUseCases, IProductsCalenderUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string PRODUCTSCALENDER_URL = "api/Product/ProductCalendars";
        public RemoteProductsCalenderUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<List<ProductsCalenderDTO>> GetProductsCalenders(List<KeyValuePair<ProductsCalenderDTO.SearchByParameters, string>>
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
                List<ProductsCalenderDTO> result = await Get<List<ProductsCalenderDTO>>(PRODUCTSCALENDER_URL,searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<ProductsCalenderDTO.SearchByParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<ProductsCalenderDTO.SearchByParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case ProductsCalenderDTO.SearchByParameters.PRODUCT_CALENDER_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("productCalendarId".ToString(), searchParameter.Value));
                        }
                        break;
                    case ProductsCalenderDTO.SearchByParameters.PRODUCT_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("product_Id".ToString(), searchParameter.Value));
                        }
                        break;
                    case ProductsCalenderDTO.SearchByParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                    case ProductsCalenderDTO.SearchByParameters.PRODUCT_ID_LIST:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("product_Id_List".ToString(), searchParameter.Value));
                        }
                        break;
                    case ProductsCalenderDTO.SearchByParameters.SHOWHIDE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("showHide".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }
        public async Task<string> SaveProductsCalenders(List<ProductsCalenderDTO> productsCalenderList)
        {
            log.LogMethodEntry(productsCalenderList);
            try
            {
                string responseString = await Post<string>(PRODUCTSCALENDER_URL, productsCalenderList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        public async Task<string> Delete(List<ProductsCalenderDTO> productsCalenderDTOList)
        {
            try
            {
                log.LogMethodEntry(productsCalenderDTOList);
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                string content = JsonConvert.SerializeObject(productsCalenderDTOList);
                string responseString = await Delete(PRODUCTSCALENDER_URL, content);
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
