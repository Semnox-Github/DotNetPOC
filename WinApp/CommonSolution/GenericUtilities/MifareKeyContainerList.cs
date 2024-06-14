/********************************************************************************************
 * Project Name - Generic Utilities
 * Description  - Holds the MifareKeyContainer object for each of the site
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0      17-Nov-2020      Lakshminarayana           Created : POS UI Redesign with REST API
 ********************************************************************************************/

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Semnox.Core.Utilities;
namespace Semnox.Core.GenericUtilities
{
    public static class MifareKeyContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Cache<int, MifareKeyContainer> mifareKeyContainerDictionary = new Cache<int, MifareKeyContainer>();
        private static Timer refreshTimer;

        static MifareKeyContainerList()
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
            var uniqueKeyList = mifareKeyContainerDictionary.Keys;
            foreach (var uniqueKey in uniqueKeyList)
            {
                MifareKeyContainer mifareKeyContainer;
                if (mifareKeyContainerDictionary.TryGetValue(uniqueKey, out mifareKeyContainer))
                {
                    mifareKeyContainerDictionary[uniqueKey] = mifareKeyContainer.Refresh();
                }
            }
            log.LogMethodExit();
        }

        private static MifareKeyContainer GetMifareKeyContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            MifareKeyContainer result = mifareKeyContainerDictionary.GetOrAdd(siteId, (k)=>new MifareKeyContainer(siteId));
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the mifareKeyContainerDTOCollection for a given site
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public static MifareKeyContainerDTOCollection GetMifareKeyContainerDTOCollection(int siteId)
        {
            MifareKeyContainer container = GetMifareKeyContainer(siteId);
            return container.GetMifareKeyContainerDTOCollection();
        }

        /// <summary>
        /// rebuilds the container
        /// </summary>
        public static void Rebuild(int siteId)
        {
            log.LogMethodEntry();
            MifareKeyContainer mifareKeyContainer = GetMifareKeyContainer(siteId);
            mifareKeyContainerDictionary[siteId] = mifareKeyContainer.Refresh();
            log.LogMethodExit();
        }
    }
}
