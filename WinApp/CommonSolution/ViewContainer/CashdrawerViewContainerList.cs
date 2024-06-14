/********************************************************************************************
 * Project Name - ContainerView
 * Description  - CashdrawerViewContainerList holds multiple  CountryView containers based on siteId, userId and POSMachineId
 * 
 *   
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.130.00    27-Jul-2021       Girish Kundar             Created : Multicash drawer enhancement
********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Printer.Cashdrawers;
using System.Collections.Generic;
using System.Timers;

namespace Semnox.Parafait.ViewContainer
{
    // <summary>
    /// CashdrawerViewContainerList holds multiple  CashdrawerViewContainer 
    /// <summary>
    public class CashdrawerViewContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Cache<int, CashdrawerViewContainer> cashdrawerViewContainerCache = new Cache<int, CashdrawerViewContainer>();
        private static Timer refreshTimer;

        static CashdrawerViewContainerList()
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
            var uniqueKeyList = cashdrawerViewContainerCache.Keys;
            foreach (var uniqueKey in uniqueKeyList)
            {
                CashdrawerViewContainer CashdrawerViewContainer;
                if (cashdrawerViewContainerCache.TryGetValue(uniqueKey, out CashdrawerViewContainer))
                {
                    cashdrawerViewContainerCache[uniqueKey] = CashdrawerViewContainer.Refresh(false);
                }
            }
            log.LogMethodExit();
        }
        private static CashdrawerViewContainer GetCashdrawerViewContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            var result = cashdrawerViewContainerCache.GetOrAdd(siteId, (k) => new CashdrawerViewContainer(siteId));
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// Returns the CashdrawerContainerDTO for a given context
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <returns></returns>
        public static List<CashdrawerContainerDTO> GetCashdrawerContainerDTOList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            CashdrawerViewContainer cashdrawerViewContainer = GetCashdrawerViewContainer(executionContext.SiteId);
            List<CashdrawerContainerDTO> result = cashdrawerViewContainer.GetCashdrawerContainerDTOList();
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
        public static CashdrawerContainerDTOCollection GetCashdrawerContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(siteId);
            if (rebuildCache)
            {
                Rebuild(siteId);
            }
            CashdrawerViewContainer container = GetCashdrawerViewContainer(siteId);
            CashdrawerContainerDTOCollection cashdrawerContainerDTOCollection = container.GetCashdrawerContainerDTOCollection(hash);
            return cashdrawerContainerDTOCollection;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteId"></param>
        public static void Rebuild(int siteId)
        {
            log.LogMethodEntry();
            CashdrawerViewContainer container = GetCashdrawerViewContainer(siteId);
            cashdrawerViewContainerCache[siteId] = container.Refresh(true);
            log.LogMethodExit();
        }
    }
}
