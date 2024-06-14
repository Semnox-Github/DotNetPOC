/********************************************************************************************
 * Project Name - Waiver
 * Description  - WaiverSetContainerList class 
 *  
 **************
 **Version Log
 **************
 *Version     Date              Modified By        Remarks          
 *********************************************************************************************
 2.130.0      20-Jul-2021       Mushahid Faizan    Created 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Timers;

namespace Semnox.Parafait.Waiver
{
    public class WaiverSetContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Cache<int, WaiverSetContainers> waiverSetContainerDictionary = new Cache<int, WaiverSetContainers>();
        private static Timer refreshTimer;

        static WaiverSetContainerList()
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
            var uniqueKeyList = waiverSetContainerDictionary.Keys;
            foreach (var uniqueKey in uniqueKeyList)
            {
                WaiverSetContainers waiverSetContainer;
                if (waiverSetContainerDictionary.TryGetValue(uniqueKey, out waiverSetContainer))
                {
                    waiverSetContainerDictionary[uniqueKey] = waiverSetContainer.Refresh();
                }
            }
            log.LogMethodExit();
        }
        private static WaiverSetContainers GetWaiverSetContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            WaiverSetContainers result = waiverSetContainerDictionary.GetOrAdd(siteId, (k) => new WaiverSetContainers(siteId));
            log.LogMethodExit(result);
            return result;
        }
        public static List<WaiverSetContainerDTO> GetWaiverSetContainerDTOList(int siteId, int languageId)
        {
            log.LogMethodEntry(siteId);
            WaiverSetContainers container = GetWaiverSetContainer(siteId);
            List<WaiverSetContainerDTO> waiverSetContainerDTOList = container.GetWaiverSetContainerDTOList(languageId);
            log.LogMethodExit(waiverSetContainerDTOList);
            return waiverSetContainerDTOList;
        }

        public static WaiverSetContainerDTOCollection GetWaiverSetContainerDTOCollection(int siteId, int languageId)
        {
            log.LogMethodEntry(siteId);
            WaiverSetContainers container = GetWaiverSetContainer(siteId);
            WaiverSetContainerDTOCollection result = container.GetWaiverSetContainerDTOCollection(languageId);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the WaiverSetContainerDTO based on the site and waiverSetId
        /// </summary>
        /// <param name="siteId">site Id</param>
        /// <param name="waiverSetId">option value waiverSetId</param>
        /// <returns></returns>
        public static WaiverSetContainerDTO GetWaiverSetContainerDTO(int siteId, int waiverSetId)
        {
            log.LogMethodEntry(siteId, waiverSetId);
            WaiverSetContainers waiverSetContainer = GetWaiverSetContainer(siteId);
            WaiverSetContainerDTO result = waiverSetContainer.GetWaiverSetContainerDTO(waiverSetId);
            log.LogMethodExit();
            return result;
        }

        /// <summary>
        /// Returns the WaiverSetContainerDTO based on the execution context
        /// </summary>
        /// <param name="executionContext">current application execution context</param>
        /// <param name="waiverSetId">waiverSetId</param>
        /// <returns></returns>
        public static WaiverSetContainerDTO GetWaiverSetContainerDTO(ExecutionContext executionContext, int waiverSetId)
        {
            log.LogMethodEntry(executionContext, waiverSetId);
            WaiverSetContainerDTO waiverSetContainerDTO = GetWaiverSetContainerDTO(executionContext.GetSiteId(), waiverSetId);
            log.LogMethodExit(waiverSetContainerDTO);
            return waiverSetContainerDTO;
        }
        /// <summary>
        /// rebulds the container
        /// </summary>
        public static void Rebuild(int siteId)
        {
            log.LogMethodEntry();
            WaiverSetContainers waiverSetContainer = GetWaiverSetContainer(siteId);
            waiverSetContainerDictionary[siteId] = waiverSetContainer.Refresh();
            log.LogMethodExit();
        }
    }
}
