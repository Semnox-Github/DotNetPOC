/********************************************************************************************
 * Project Name - RedemptionUtils
 * Description  - RemoteRedemptionCurrencyUseCase class to get the data  from API by doing remote call  
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0          07-Nov-2020      Vikas Dwivedi             Created : POS UI Redesign with REST API
 2.110.1      17-Feb-2021      Mushahid Faizan           Modified - Web Inventory Phase 2 changes
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Redemption
{
    public class RemoteRedemptionCurrencyUseCases : RemoteUseCases, IRedemptionCurrencyUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string REDEMPTION_CURRENCY_URL = "/api/Inventory/RedemptionCurrencies";
        private const string REDEMPTION_CURRENCY_COUNT_URL = "/api/Inventory/RedemptionCurrencyCounts";
        private const string REDEMPTION_CURRENCY_CONTAINER_URL = "/api/Inventory/RedemptionCurrenciesContainer";
       

        public RemoteRedemptionCurrencyUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters, string>> redemptionCurrencySearchParams)
        {
            log.LogMethodEntry(redemptionCurrencySearchParams);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters, string> searchParameter in redemptionCurrencySearchParams)
            {
                switch (searchParameter.Key)
                {

                    case RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters.ISACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                    case RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters.CURRENCY_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("currencyId".ToString(), searchParameter.Value));
                        }
                        break;
                    case RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters.BARCODE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("currencyCode".ToString(), searchParameter.Value));
                        }
                        break;
                    case RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters.PRODUCT_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("productId".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }

        public async Task<string> SaveRedemptionCurrencies(List<RedemptionCurrencyDTO> redemptionCurrencyDTOList)
        {
            log.LogMethodEntry(redemptionCurrencyDTOList);
            try
            {
                string responseString = await Post<string>(REDEMPTION_CURRENCY_URL, redemptionCurrencyDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<RedemptionCurrencyContainerDTOCollection> GetRedemptionCurrencyContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(hash, rebuildCache);
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("siteId", siteId.ToString()));
            if (string.IsNullOrWhiteSpace(hash) == false)
            {
                parameters.Add(new KeyValuePair<string, string>("hash", hash));
            }
            parameters.Add(new KeyValuePair<string, string>("rebuildCache", rebuildCache.ToString()));
            RedemptionCurrencyContainerDTOCollection result = await Get<RedemptionCurrencyContainerDTOCollection>(REDEMPTION_CURRENCY_CONTAINER_URL, parameters);
            log.LogMethodExit(result);
            return result;
        }

        public async Task<List<RedemptionCurrencyDTO>> GetRedemptionCurrencies(List<KeyValuePair<RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters, string>> parameters, 
            int currentPage = 0, int pageSize = 0, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parameters);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            try
            {
                List<RedemptionCurrencyDTO> result = await Get<List<RedemptionCurrencyDTO>>(REDEMPTION_CURRENCY_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        public async Task<int> GetRedemptionCurrencyCount(List<KeyValuePair<RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters, string>> parameters)
        {
            log.LogMethodEntry(parameters);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            try
            {
                int result = await Get<int>(REDEMPTION_CURRENCY_COUNT_URL, searchParameterList);
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
