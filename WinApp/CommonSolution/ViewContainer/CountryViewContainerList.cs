/********************************************************************************************
 * Project Name - ContainerView
 * Description  - CountryViewContainerList holds multiple  CountryView containers based on siteId, userId and POSMachineId
 * 
 *   
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.00    08-Jul-2021      Roshan Devadiga           Created : POS UI Redesign with REST API
********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Semnox.Parafait.ViewContainer
{
    // <summary>
    /// CountryViewContainerList holds multiple  CountryView containers based on siteId, userId and POSMachineId
    /// <summary>
    public class CountryViewContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Cache<int, CountryViewContainer> countryViewContainerCache = new Cache<int, CountryViewContainer>();
        private static Timer refreshTimer;

        static CountryViewContainerList()
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
            var uniqueKeyList = countryViewContainerCache.Keys;
            foreach (var uniqueKey in uniqueKeyList)
            {
                CountryViewContainer countryViewContainer;
                if (countryViewContainerCache.TryGetValue(uniqueKey, out countryViewContainer))
                {
                    countryViewContainerCache[uniqueKey] = countryViewContainer.Refresh(false);
                }
            }
            log.LogMethodExit();
        }
        private static CountryViewContainer GetCountryViewContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            var result = countryViewContainerCache.GetOrAdd(siteId, (k) => new CountryViewContainer(siteId));
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// Returns the POSMachineContainerDTOList for a given context
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <returns></returns>
        public static List<CountryContainerDTO> GetCountryContainerDTOList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            CountryViewContainer countryViewContainer = GetCountryViewContainer(executionContext.SiteId);
            List<CountryContainerDTO> result = countryViewContainer.GetCountryContainerDTOList();
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="hash"></param>
        /// <param name="rebuildCache"></param>
        /// <returns></returns>
        public static CountryContainerDTOCollection GetCountryContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(siteId);
            if (rebuildCache)
            {
                Rebuild(siteId);
            }
            CountryViewContainer container = GetCountryViewContainer(siteId);
            CountryContainerDTOCollection countryContainerDTOCollection = container.GetCountryContainerDTOCollection(hash);
            return countryContainerDTOCollection;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteId"></param>
        public static void Rebuild(int siteId)
        {
            log.LogMethodEntry();
            CountryViewContainer container = GetCountryViewContainer(siteId);
            countryViewContainerCache[siteId] = container.Refresh(true);
            log.LogMethodExit();
        }
    }
}
