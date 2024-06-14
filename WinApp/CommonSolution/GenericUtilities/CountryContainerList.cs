/********************************************************************************************
* Project Name - GenericUtilities
* Description  - CountryContainerList class 
*  
**************
**Version Log
**************
*Version     Date             Modified By               Remarks          
*********************************************************************************************
2.120.00    08-Jul-2021       Roshan Devadiga        Created : POS UI Redesign with REST API
********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Semnox.Core.GenericUtilities
{
    public class CountryContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Cache<int, CountryContainer> countryContainerDictionary = new Cache<int, CountryContainer>();
        private static Timer refreshTimer;

        static CountryContainerList()
        {
            log.LogMethodEntry();
            refreshTimer = new Timer(DataRefreshFrequency.GetValue());
            refreshTimer.Elapsed += OnRefreshTimer;
            refreshTimer.Start();
            log.LogMethodExit();
        }
        private static void OnRefreshTimer(object sender, ElapsedEventArgs e)
        {
            log.LogMethodEntry();
            var uniqueKeyList = countryContainerDictionary.Keys;
            foreach (var uniqueKey in uniqueKeyList)
            {
                CountryContainer countryContainer;
                if (countryContainerDictionary.TryGetValue(uniqueKey, out countryContainer))
                {
                    countryContainerDictionary[uniqueKey] = countryContainer.Refresh();
                }
            }
            log.LogMethodExit();
        }
        private static CountryContainer GetCountryContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            CountryContainer result = countryContainerDictionary.GetOrAdd(siteId, (k) => new CountryContainer(siteId));
            log.LogMethodExit(result);
            return result;
        }
        public static List<CountryContainerDTO> GetCountryContainerDTOList(int siteId)
        {
            log.LogMethodEntry(siteId);
            CountryContainer container = GetCountryContainer(siteId);
            List<CountryContainerDTO> countryContainerDTOList = container.GetCountryContainerDTOList();
            log.LogMethodExit(countryContainerDTOList);
            return countryContainerDTOList;
        }

        public static string GetCountryCode(int siteId, int countryId)
        {
            log.LogMethodEntry(siteId);
            CountryContainer container = GetCountryContainer(siteId);
            string countryCode = container.GetCountryContainerDTOList().Find(x => x.CountryId == countryId).CountryCode;
            log.LogMethodExit(countryCode);
            return countryCode;
        }

        /// <summary>
        /// rebulds the container
        /// </summary>
        public static void Rebuild(int siteId)
        {
            log.LogMethodEntry();
            CountryContainer countryContainer = GetCountryContainer(siteId);
            countryContainerDictionary[siteId] = countryContainer.Refresh();
            log.LogMethodExit();
        }
    }
}
