/********************************************************************************************
 * Project Name - PriceList
 * Description  - PriceListUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.00    10-Mar-2021       Roshan Devadiga        Created : POS UI Redesign with REST API
 ********************************************************************************************/
using Newtonsoft.Json;
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.PriceList
{
   public class RemotePriceListUseCases:RemoteUseCases,IPriceListUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string PRICELIST_URL = "api/Product/PriceLists";

        public RemotePriceListUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<List<PriceListDTO>> GetPriceLists(List<KeyValuePair<PriceListDTO.SearchByPriceListParameters, string>>
                         parameters, bool loadActiveRecordsOnly = false, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parameters,loadActiveRecordsOnly,sqlTransaction);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("loadActiveRecordsOnly".ToString(), loadActiveRecordsOnly.ToString()));

            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            try
            {
                List<PriceListDTO> result = await Get<List<PriceListDTO>>(PRICELIST_URL,searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<PriceListDTO.SearchByPriceListParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<PriceListDTO.SearchByPriceListParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case PriceListDTO.SearchByPriceListParameters.PRICE_LIST_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("priceListId".ToString(), searchParameter.Value));
                        }
                        break;
                    case PriceListDTO.SearchByPriceListParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }
        public async Task<string> SavePriceLists(List<PriceListDTO> priceListDTOList)
        {
            log.LogMethodEntry(priceListDTOList);
            try
            {
                string responseString = await Post<string>(PRICELIST_URL, priceListDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<string> Delete(List<PriceListDTO> priceListDTOList)
        {
            try
            {
                log.LogMethodEntry(priceListDTOList);
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                string content = JsonConvert.SerializeObject(priceListDTOList);
                string responseString = await Delete(PRICELIST_URL,content);
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
