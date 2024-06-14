/********************************************************************************************
 * Project Name - Printer                                                                        
 * Description  -CashdrawerContainerList
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 ********************************************************************************************* 
 *2.140.0     11-Aug-2021      Girish Kundar     Created 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System.Timers;

namespace Semnox.Parafait.Printer.Cashdrawers
{
    public class CashdrawerContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static Timer refreshTimer;
        private static readonly Cache<int, CashdrawerContainer> CashdrawerContainerCache = new Cache<int, CashdrawerContainer>();


        static CashdrawerContainerList()
        {
            log.LogMethodEntry();
            refreshTimer = new Timer(DataRefreshFrequency.GetValue());
            refreshTimer.Elapsed += OnRefreshTimer;
            log.LogMethodExit();
        }

        private static void OnRefreshTimer(object sender, ElapsedEventArgs e)
        {
            log.LogMethodEntry();
            var uniqueKeyList = CashdrawerContainerCache.Keys;
            foreach (var uniqueKey in uniqueKeyList)
            {
                CashdrawerContainer CashdrawerContainer;
                if (CashdrawerContainerCache.TryGetValue(uniqueKey, out CashdrawerContainer))
                {
                    CashdrawerContainerCache[uniqueKey] = CashdrawerContainer.Refresh();
                }
            }
            log.LogMethodExit();
        }

        private static CashdrawerContainer GetCashdrawerContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            CashdrawerContainer result = CashdrawerContainerCache.GetOrAdd(siteId, (k) => new CashdrawerContainer(siteId));
            log.LogMethodExit(result);
            return result;
        }

        public static CashdrawerContainerDTOCollection GetCashdrawerContainerDTOCollection(int siteId)
        {
            log.LogMethodEntry(siteId);
            CashdrawerContainer container = GetCashdrawerContainer(siteId);
            CashdrawerContainerDTOCollection result = container.GetCashdrawerContainerDTOCollection();
            log.LogMethodExit(result);
            return result;
        }

        public static CashdrawerContainerDTO GetCashdrawerContainerDTO(ExecutionContext executionContext, int cashdrawerId)
        {
            log.LogMethodEntry(executionContext, cashdrawerId);
            log.LogMethodExit();
            return GetCashdrawerContainerDTO(executionContext.SiteId, cashdrawerId);
        }

        public static CashdrawerContainerDTO GetCashdrawerContainerDTO(int siteId, int cashdrawerId)
        {
            log.LogMethodEntry(siteId, cashdrawerId);
            CashdrawerContainer container = GetCashdrawerContainer(siteId);
            var result = container.GetCashdrawerContainerDTO(cashdrawerId);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// rebuilds the container
        /// </summary>
        public static void Rebuild(int siteId)
        {
            log.LogMethodEntry();
            CashdrawerContainer CashdrawerContainer = GetCashdrawerContainer(siteId);
            CashdrawerContainerCache[siteId] = CashdrawerContainer.Refresh();
            log.LogMethodExit();
        }
    }
}
