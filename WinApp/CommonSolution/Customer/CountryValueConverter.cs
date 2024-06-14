/********************************************************************************************
 * Project Name - Customer
 * Description  - Class for  of CountryValueConverter      
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        10-Aug-2019   Girish kundar  Modified : Added Logger Methods and Removed Unused namespace's.
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using Semnox.Core.GenericUtilities;
using Semnox.Core.GenericUtilities.Excel;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Customer
{
    class CountryValueConverter : ValueConverter
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        Dictionary<int, CountryDTO> countryIdCountryDTODictionary;
        Dictionary<string, CountryDTO> countryNameCountryDTODictionary;

        public CountryValueConverter(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            countryNameCountryDTODictionary = new Dictionary<string, CountryDTO>();
            countryIdCountryDTODictionary = new Dictionary<int, CountryDTO>();
            List<CountryDTO> countryList = null;
            CountryDTOList countryDTOList = new CountryDTOList(executionContext);
            List<KeyValuePair<CountryDTO.SearchByParameters, string>> searchCountryParams = new List<KeyValuePair<CountryDTO.SearchByParameters, string>>();
            searchCountryParams.Add(new KeyValuePair<CountryDTO.SearchByParameters, string>(CountryDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            countryList = countryDTOList.GetCountryDTOList(searchCountryParams);
            if (countryList != null && countryList.Count > 0)
            {
                foreach (var country in countryList)
                {
                    countryIdCountryDTODictionary.Add(country.CountryId, country);
                    countryNameCountryDTODictionary.Add(country.CountryName.ToUpper(), country);
                }
            }
            log.LogMethodExit();

        }

        public override object FromString(string stringValue)
        {
            log.LogMethodEntry(stringValue);
            int countryId = -1;
            if (countryNameCountryDTODictionary != null && countryNameCountryDTODictionary.ContainsKey(stringValue.ToUpper()))
            {
                countryId = countryNameCountryDTODictionary[stringValue.ToUpper()].CountryId;
            }
            log.LogMethodExit(countryId);
            return countryId;
        }

        public override string ToString(object value)
        {
            log.LogMethodEntry(value);
            string countryName = string.Empty;
            if (countryIdCountryDTODictionary != null && countryIdCountryDTODictionary.ContainsKey(Convert.ToInt32(value)))
            {
                countryName = countryIdCountryDTODictionary[Convert.ToInt32(value)].CountryName;
            }
            log.LogMethodExit(countryName);
            return countryName;
        }
    }
}
