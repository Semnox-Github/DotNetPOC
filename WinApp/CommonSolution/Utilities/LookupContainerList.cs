/********************************************************************************************
 * Project Name - Utilities
 * Description  - LookupMasterList class to get the List of lookup from the container API
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         24-Aug-2020       Girish Kundar             Created : POS UI Redesign with REST API
 2.160.0     24-Jul-2022       Prajwal                   Modified : To latest format.
 ********************************************************************************************/
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Semnox.Core.Utilities;
namespace Semnox.Core.Utilities
{
    public static class LookupsContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Cache<int, LookupsContainer> lookupsContainerCache = new Cache<int, LookupsContainer>();
        private static Timer refreshTimer;

        /// <summary>
        /// Default Constructor.
        /// </summary>
        static LookupsContainerList()
        {
            log.LogMethodEntry();
            refreshTimer = new Timer(DataRefreshFrequency.GetValue());
            refreshTimer.Elapsed += OnRefreshTimer;
            refreshTimer.Start();
            log.LogMethodExit();
        }

        /// <summary>
        /// Container Refresh after elapsing the time set for refresh.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnRefreshTimer(object sender, ElapsedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            var uniqueKeyList = lookupsContainerCache.Keys;
            foreach (var uniqueKey in uniqueKeyList)
            {
                LookupsContainer lookupsContainer;
                if (lookupsContainerCache.TryGetValue(uniqueKey, out lookupsContainer))
                {
                    lookupsContainerCache[uniqueKey] = lookupsContainer.Refresh();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the Container Data for the Site.
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        private static LookupsContainer GetLookupsContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            LookupsContainer result = lookupsContainerCache.GetOrAdd(siteId, (k) => new LookupsContainer(siteId));
            log.LogMethodExit(result);
            return result;
        }

        public static LookupsContainerDTOCollection GetLookupsContainerDTOCollection(int siteId)
        {
            log.LogMethodEntry(siteId);
            LookupsContainer container = GetLookupsContainer(siteId);
            LookupsContainerDTOCollection result = container.GetLookupsContainerDTOCollection();
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// rebuilds the container
        /// </summary>
        public static void Rebuild(int siteId)
        {
            log.LogMethodEntry(siteId);
            LookupsContainer lookupsContainer = GetLookupsContainer(siteId);
            lookupsContainerCache[siteId] = lookupsContainer.Refresh();
            log.LogMethodExit();
        }
        public static LookupsContainerDTO GetLookupsContainerDTO(int siteId, string lookupName, ExecutionContext executionContext = null)
        {
            log.LogMethodEntry(siteId, lookupName);
            LookupsContainer lookupsContainer = GetLookupsContainer(siteId);
            LookupsContainerDTO result = lookupsContainer.GetLookupsContainerDTO(lookupName);
            log.LogMethodExit(result);
            return result;
        }

        public static LookupsContainerDTO GetLookupsContainerDTOOrDefault(int siteId, string lookupName)
        {
            log.LogMethodEntry(siteId, lookupName);
            LookupsContainer lookupsContainer = GetLookupsContainer(siteId);
            LookupsContainerDTO result = lookupsContainer.GetLookupsContainerDTOOrDefault(lookupName);
            log.LogMethodExit(result);
            return result;
        }

        public static LookupValuesContainerDTO GetLookupValuesContainerDTO(int siteId, int lookupValueId)
        {
            log.LogMethodEntry(siteId, lookupValueId);
            LookupsContainer lookupsContainer = GetLookupsContainer(siteId);
            LookupValuesContainerDTO result = lookupsContainer.GetLookupValuesContainerDTO(lookupValueId);
            log.LogMethodExit(result);
            return result;
        }

        public static LookupValuesContainerDTO GetLookupValuesContainerDTO(int siteId, string lookupName, string lookupValue)
        {
            log.LogMethodEntry(siteId, lookupName, lookupValue);
            LookupsContainer lookupsContainer = GetLookupsContainer(siteId);
            LookupValuesContainerDTO result = lookupsContainer.GetLookupValuesContainerDTO(lookupName, lookupValue);
            log.LogMethodExit(result);
            return result;
        }
        public static LookupValuesContainerDTO GetLookupValuesContainerDTOOrDefault(int siteId, string lookupName, string lookupValue)
        {
            log.LogMethodEntry(siteId, lookupName, lookupValue);
            LookupsContainer lookupsContainer = GetLookupsContainer(siteId);
            LookupValuesContainerDTO result = lookupsContainer.GetLookupValuesContainerDTOOrDefault(lookupName, lookupValue);
            log.LogMethodExit(result);
            return result;
        }
    }
}

