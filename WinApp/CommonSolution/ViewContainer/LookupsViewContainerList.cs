using Semnox.Core.Utilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Semnox.Parafait.ViewContainer
{
    /// <summary>
    /// LookupsViewContainerList
    /// </summary>
    public class LookupsViewContainerList
    { /// <summary>
      /// Holds the Lookups container object
      /// </summary>
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly ConcurrentDictionary<int, LookupsViewContainer> lookupsViewContainerDictionary = new ConcurrentDictionary<int, LookupsViewContainer>();
        private static Timer refreshTimer;
        static LookupsViewContainerList()
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
            List<int> uniqueKeyList = lookupsViewContainerDictionary.Keys.ToList();
            foreach (var uniqueKey in uniqueKeyList)
            {
                LookupsViewContainer lookupsViewContainer;
                if (lookupsViewContainerDictionary.TryGetValue(uniqueKey, out lookupsViewContainer))
                {
                    lookupsViewContainerDictionary[uniqueKey] = lookupsViewContainer.Refresh();
                }
            }
            log.LogMethodExit();
        }

        private static LookupsViewContainer GetLookupsViewContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            if (lookupsViewContainerDictionary.ContainsKey(siteId) == false)
            {
                lookupsViewContainerDictionary[siteId] = new LookupsViewContainer(siteId);
            }
            LookupsViewContainer result = lookupsViewContainerDictionary[siteId];
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the Lookups role container DTO list 
        /// </summary>
        /// <param name="executionContext">current application execution context</param>
        /// <returns></returns>
        public static List<LookupsContainerDTO> GetLookupsContainerDTOList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            LookupsViewContainer lookupsViewContainer = GetLookupsViewContainer(executionContext.SiteId);
            List<LookupsContainerDTO> result = lookupsViewContainer.GetLookupsContainerDTOList();
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the LookupsContainerDTOCollection for a given siteId and hash
        /// If the hash matches then no data is returned as the caller is holding the latest data
        /// </summary>
        /// <param name="siteId">site Id</param>
        /// <param name="hash">hash</param>
        /// /// <param name="rebuildCache">hash</param>
        /// <returns></returns>
        public static LookupsContainerDTOCollection GetLookupsContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(siteId);
            if (rebuildCache)
            {
                Rebuild(siteId);
            }
            LookupsViewContainer container = GetLookupsViewContainer(siteId);
            LookupsContainerDTOCollection lookupsContainerDTOCollection = container.GetLookupsDTOCollection(hash);
            return lookupsContainerDTOCollection;
        }
        /// <summary>
        /// Rebuild
        /// </summary>
        /// <param name="siteId"></param>
        public static void Rebuild(int siteId)
        {
            log.LogMethodEntry();
            LookupsViewContainer container = GetLookupsViewContainer(siteId);
            lookupsViewContainerDictionary[siteId] = container.Refresh();
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the LookupsContainerDTO based on the siteId and LookupsId
        /// </summary>
        /// <param name="siteId">site Id</param>
        /// <param name="lookupName">Lookups Id</param>
        /// <returns></returns>
        public static LookupsContainerDTO GetLookupsContainerDTO(int siteId, string lookupName)
        {
            log.LogMethodEntry(siteId, lookupName);
            LookupsViewContainer lookupsViewContainer = GetLookupsViewContainer(siteId);
            LookupsContainerDTO result = lookupsViewContainer.GetLookupsContainerDTO(lookupName);
            log.LogMethodExit();
            return result;
        }

        /// <summary>
        /// Returns the LookupsContainerDTO based on the siteId and LookupsId
        /// </summary>
        /// <param name="siteId">site Id</param>
        /// <param name="lookupValueId">Lookups Id</param>
        /// <returns></returns>
        public static LookupValuesContainerDTO GetLookupValuesContainerDTO(int siteId, int lookupValueId)
        {
            log.LogMethodEntry(siteId, lookupValueId);
            LookupsViewContainer lookupsViewContainer = GetLookupsViewContainer(siteId);
            LookupValuesContainerDTO result = lookupsViewContainer.GetLookupValuesContainerDTO(lookupValueId);
            log.LogMethodExit();
            return result;
        }
    }
}


