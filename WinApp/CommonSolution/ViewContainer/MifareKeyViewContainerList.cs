/********************************************************************************************
 * Project Name - Generic Utilities
 * Description  - Holds the MifareKeyViewContainer object for each of the site
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0      17-Nov-2020      Lakshminarayana           Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System.Collections.Generic;
using System.Timers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.ViewContainer
{
    /// <summary>
    /// Holds the MifareKeyViewContainer object for each of the site
    /// </summary>
    public  class MifareKeyViewContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Cache<int, MifareKeyViewContainer> mifareKeyViewContainerCache = new Cache<int, MifareKeyViewContainer>();
        private static Timer refreshTimer;

        static MifareKeyViewContainerList()
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
            var uniqueKeyList = mifareKeyViewContainerCache.Keys;
            foreach (var uniqueKey in uniqueKeyList)
            {
                MifareKeyViewContainer mifareKeyViewContainer;
                if (mifareKeyViewContainerCache.TryGetValue(uniqueKey, out mifareKeyViewContainer))
                {
                    mifareKeyViewContainerCache[uniqueKey] = mifareKeyViewContainer.Refresh(false);
                }
            }
            log.LogMethodExit();
        }

        private static MifareKeyViewContainer GetMifareKeyViewContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            MifareKeyViewContainer result = mifareKeyViewContainerCache.GetOrAdd(siteId, (k)=>new MifareKeyViewContainer(siteId));
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// returns the latest in MifareKeyContainerDTOCollection
        /// </summary>
        /// <returns></returns>
        public static MifareKeyContainerDTOCollection GetMifareKeyContainerDTOCollection(int siteId, string hash)
        {
            log.LogMethodEntry(siteId, hash);
            MifareKeyViewContainer mifareKeyViewContainer = GetMifareKeyViewContainer(siteId);
            MifareKeyContainerDTOCollection result = mifareKeyViewContainer.GetMifareKeyContainerDTOCollection(hash);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// return the MifareKeyContainerDTO list
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public static IEnumerable<MifareKeyContainerDTO> GetMifareKeyContainerDTOList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            MifareKeyViewContainer mifareKeyViewContainer = GetMifareKeyViewContainer(executionContext.SiteId);
            IEnumerable<MifareKeyContainerDTO> result = mifareKeyViewContainer.GetMifareKeyContainerDTOList();
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// rebuilds the container
        /// </summary>
        public static void Rebuild(int siteId)
        {
            log.LogMethodEntry();
            MifareKeyViewContainer mifareKeyViewContainer = GetMifareKeyViewContainer(siteId);
            mifareKeyViewContainerCache[siteId] = mifareKeyViewContainer.Refresh(true);
            log.LogMethodExit();
        }
    }
}

