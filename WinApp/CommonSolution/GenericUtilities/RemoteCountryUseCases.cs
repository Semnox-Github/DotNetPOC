/********************************************************************************************
 * Project Name -GenericUtilities
 * Description  -RemoteCountryUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.00    11-May-2021       Roshan Devadiga        Created : POS UI Redesign with REST API
 ********************************************************************************************/
using Newtonsoft.Json;
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Core.GenericUtilities
{
    class RemoteCountryUseCases : RemoteUseCases, ICountryUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string COUNTRY_URL = "api/Common/Countries";
        private const string COUNTRY_CONTAINER_URL = "api/Common/CountriesContainer";
        public RemoteCountryUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }
        public async Task<List<CountryDTO>> GetCountries(CountryParams countryParams)
        {
            log.LogMethodEntry(countryParams);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            try
            {
                List<CountryDTO> result = await Get<List<CountryDTO>>(COUNTRY_URL, null);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        
        public async Task<string> SaveCountries(List<CountryDTO> countryDTOList)
        {
            log.LogMethodEntry(countryDTOList);
            try
            {
                string responseString = await Post<string>(COUNTRY_URL, countryDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        public async Task<String> Delete(List<CountryDTO> countryDTOList)
        {
            try
            {
                log.LogMethodEntry(countryDTOList);
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                string content = JsonConvert.SerializeObject(countryDTOList);
                string responseString = await Delete(COUNTRY_URL, content);
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

        public async Task<CountryContainerDTOCollection> GetCountryContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(hash, rebuildCache);
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("siteId", siteId.ToString()));
            if (string.IsNullOrWhiteSpace(hash) == false)
            {
                parameters.Add(new KeyValuePair<string, string>("hash", hash));
            }
            parameters.Add(new KeyValuePair<string, string>("rebuildCache", rebuildCache.ToString()));
            CountryContainerDTOCollection result = await Get<CountryContainerDTOCollection>(COUNTRY_CONTAINER_URL, parameters);
            log.LogMethodExit(result);
            return result;
        }
    }
}
