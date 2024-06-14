/********************************************************************************************
 * Project Name - Country List Programs 
 * Description  - Data object of the CountryList
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By        Remarks          
 *********************************************************************************************
 *1.00        20-April-2017   Rakshith           Created
 *2.60        13-May-2019     Mushahid Faizan    Modified - GetCountryList()
 *2.70.2        09-Aug-2019     Deeksha            Modified logger methods.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Semnox.Core.GenericUtilities
{
    public class CountryList
    {
        private static readonly Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static string STATE_LOOKUP_FOR_COUNTRY = "STATE_LOOKUP_FOR_COUNTRY";


        /// <summary>
        ///Takes LookupParams as parameter
        /// </summary>
        /// <returns>Returns List<KeyValuePair<CountryDTO.SearchByParameters, string>> by converting countryParams</returns>
        public List<KeyValuePair<CountryDTO.SearchByParameters, string>> BuildCountrySearchParametersList(CountryParams countryParams)
        {
            log.LogMethodEntry(countryParams);
            List<KeyValuePair<CountryDTO.SearchByParameters, string>> customerSearchParams = new List<KeyValuePair<CountryDTO.SearchByParameters, string>>();
            if (countryParams != null)
            {
                if (countryParams.CountryId > 0)
                    customerSearchParams.Add(new KeyValuePair<CountryDTO.SearchByParameters, string>(CountryDTO.SearchByParameters.COUNTRY_ID, countryParams.CountryId.ToString()));
                if (!string.IsNullOrEmpty(countryParams.CountryName))
                    customerSearchParams.Add(new KeyValuePair<CountryDTO.SearchByParameters, string>(CountryDTO.SearchByParameters.COUNTRY_NAME, countryParams.CountryName));
                if (countryParams.SiteId > 0)
                    customerSearchParams.Add(new KeyValuePair<CountryDTO.SearchByParameters, string>(CountryDTO.SearchByParameters.SITE_ID, countryParams.SiteId.ToString()));
            }
            log.LogMethodExit(customerSearchParams);

            return customerSearchParams;
        }

        /// <summary>
        /// Returns the List of CountryDTO
        /// </summary>
        public List<CountryDTO> GetCountryList(CountryParams countryParams)
        {
            log.LogMethodEntry(countryParams);
            CountryDataHandler countryDatahandler = new CountryDataHandler();
            if (countryParams.ShowLookupCountry)
            {
                CountryDTO countryDTO = countryDatahandler.GetCountryDTOByLookup(STATE_LOOKUP_FOR_COUNTRY, countryParams.SiteId);
                if (countryDTO.CountryId > 0)
                {
                    countryParams.CountryId = countryDTO.CountryId;
                }
            }
            List<KeyValuePair<CountryDTO.SearchByParameters, string>> countrySearchParams = BuildCountrySearchParametersList(countryParams);

            List<CountryDTO> countryDTOList = countryDatahandler.GetCountryDTOList(countrySearchParams);
            if (countryDTOList != null && countryParams.ShowState)
            {
                StateList stateList = new StateList();
                StateParams stateParams = new StateParams();
                foreach (CountryDTO countryDTO in countryDTOList)
                {
                    stateParams.CountryId = countryDTO.CountryId;
                    stateParams.SiteId = countryDTO.SiteId;
                    countryDTO.StateList = stateList.GetStateList(stateParams);
                }
            }
            log.LogMethodExit(countryDTOList);
            return countryDTOList;
        }

        public DateTime? GetCountryLastUpdateTime(int siteId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(siteId, sqlTransaction);
            CountryDataHandler countryDataHandler = new CountryDataHandler(sqlTransaction);
            DateTime? result = countryDataHandler.GetCountryLastUpdateTime(siteId);
            log.LogMethodExit(result);
            return result;
        }
    }
}
