/********************************************************************************************
 * Project Name - Redemption
 * Description  - TicketStationContainerList class to get the List of lookup from the container API
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 *2.110.0     21-Dec-2020      Abhishek          Created: POS Redesign
 ********************************************************************************************/
using System.Timers;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Redemption
{
    public static class TicketStationContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Cache<int, TicketStationsContainer> ticketStationContainerCache = new Cache<int, TicketStationsContainer>();
        private static Timer refreshTimer;

        static TicketStationContainerList()
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
            var uniqueKeyList = ticketStationContainerCache.Keys;
            foreach (var uniqueKey in uniqueKeyList)
            {
                TicketStationsContainer ticketStationsContainer;
                if (ticketStationContainerCache.TryGetValue(uniqueKey, out ticketStationsContainer))
                {
                    ticketStationContainerCache[uniqueKey] = ticketStationsContainer.Refresh();
                }
            }
            log.LogMethodExit();
        }

        private static TicketStationsContainer GetTicketStationContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            TicketStationsContainer result = ticketStationContainerCache.GetOrAdd(siteId, (k) => new TicketStationsContainer(siteId));
            log.LogMethodExit(result);
            return result;
        }

        public static TicketStationContainerDTOCollection GetTicketStationContainerDTOCollection(int siteId)
        {
            log.LogMethodEntry(siteId);
            TicketStationsContainer container = GetTicketStationContainer(siteId);
            TicketStationContainerDTOCollection result = container.GetTicketStationContainerDTOCollection();
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// rebuilds the container
        /// </summary>
        public static void Rebuild(int siteId)
        {
            log.LogMethodEntry();
            TicketStationsContainer ticketStationsContainer = GetTicketStationContainer(siteId);
            ticketStationContainerCache[siteId] = ticketStationsContainer.Refresh();
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the TicketStationContainerDTO based on the site and stationId
        /// </summary>
        /// <param name="siteId">site Id</param>
        /// <param name="stationId">station Id</param>
        /// <returns></returns>
        public static TicketStationContainerDTO GetTicketStationContainerDTO(int siteId, int stationId)
        {
            log.LogMethodEntry(siteId, stationId);
            TicketStationsContainer ticketStationsContainer = GetTicketStationContainer(siteId);
            TicketStationContainerDTO result = ticketStationsContainer.GetTicketStationContainerDTO(stationId);
            log.LogMethodExit();
            return result;
        }

        /// <summary>
        /// Returns the TicketStationContainerDTO based on the execution context
        /// </summary>
        /// <param name="executionContext">current application execution context</param>
        /// <returns></returns>
        public static TicketStationContainerDTO GetTicketStationContainerDTO(ExecutionContext executionContext, int stationId)
        {
            log.LogMethodEntry(executionContext, stationId);
            TicketStationContainerDTO ticketStationContainerDTO = GetTicketStationContainerDTO(executionContext.GetSiteId(), stationId);
            log.LogMethodExit(ticketStationContainerDTO);
            return ticketStationContainerDTO;
        }
    }
}
