using System;
using Semnox.Core.Utilities;
using Semnox.Parafait.Game;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace Semnox.Parafait.ViewContainer
{
    /// <summary>
    /// ReaderConfigurationViewMasterList holds multiple   ReaderConfigurationView containers
    /// </summary>
    public class ReaderConfigurationViewContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly ConcurrentDictionary<int, ReaderConfigurationViewContainer> readerConfigurationViewContainerDictionary = new ConcurrentDictionary<int, ReaderConfigurationViewContainer>();
        private static Timer refreshTimer;
        static ReaderConfigurationViewContainerList()
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
            List<int> uniqueKeyList = readerConfigurationViewContainerDictionary.Keys.ToList();
            foreach (var uniqueKey in uniqueKeyList)
            {
                ReaderConfigurationViewContainer readerConfigurationViewContainer;
                if (readerConfigurationViewContainerDictionary.TryGetValue(uniqueKey, out readerConfigurationViewContainer))
                {
                    readerConfigurationViewContainerDictionary[uniqueKey] = readerConfigurationViewContainer.Refresh();
                }
            }
            log.LogMethodExit();
        }

        private static ReaderConfigurationViewContainer GetReaderConfigurationViewContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            if (readerConfigurationViewContainerDictionary.ContainsKey(siteId) == false)
            {
                readerConfigurationViewContainerDictionary[siteId] = new ReaderConfigurationViewContainer(siteId);
            }
            ReaderConfigurationViewContainer result = readerConfigurationViewContainerDictionary[siteId];
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the ReaderConfigurationContainerDTOCollection for a given siteId and hash
        /// If the hash matches then no data is returned as the caller is holding the latest data
        /// </summary>
        /// <param name="siteId">site Id</param>
        /// <param name="hash">hash</param>
        /// /// <param name="rebuildCache">hash</param>
        /// <returns></returns>
        public static ReaderConfigurationContainerDTOCollection GetReaderConfigurationContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(siteId);
            if (rebuildCache)
            {
                Rebuild(siteId);
            }
            ReaderConfigurationViewContainer container = GetReaderConfigurationViewContainer(siteId);
            ReaderConfigurationContainerDTOCollection readerConfigurationContainerDTOCollection = container.GetReaderConfigurationDTOCollection(hash);
            return readerConfigurationContainerDTOCollection;
        }

        public static void Rebuild(int siteId)
        {
            log.LogMethodEntry();
            ReaderConfigurationViewContainer container = GetReaderConfigurationViewContainer(siteId);
            readerConfigurationViewContainerDictionary[siteId] = container.Refresh();
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the ReaderConfigurationContainerDTO based on the execution context
        /// </summary>
        /// <param name="executionContext">current application execution context</param>
        /// <returns></returns>
        public static ReaderConfigurationContainerDTO GetReaderConfigurationContainerDTO(ExecutionContext executionContext)
        {
            return GetReaderConfigurationContainerDTO(executionContext.SiteId, executionContext.MachineId);
        }

        /// <summary>
        /// Returns the ReaderConfigurationContainerDTO based on the siteId and ReaderConfigurationId
        /// </summary>
        /// <param name="siteId">site Id</param>
        /// <param name="readerConfigurationId">ReaderConfiguration Id</param>
        /// <returns></returns>
        public static ReaderConfigurationContainerDTO GetReaderConfigurationContainerDTO(int siteId, int readerConfigurationId)
        {
            log.LogMethodEntry(siteId, readerConfigurationId);
            ReaderConfigurationViewContainer readerConfigurationViewContainer = GetReaderConfigurationViewContainer(siteId);
            ReaderConfigurationContainerDTO result = readerConfigurationViewContainer.GetReaderConfigurationContainerDTO(readerConfigurationId);
            log.LogMethodExit();
            return result;
        }
    }
}

