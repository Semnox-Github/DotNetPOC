/********************************************************************************************
 * Project Name - ViewContainer
 * Description  - TicketStationViewContainerList 
 
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0      21-Dec-2020      Abhishek           Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System.Collections.Generic;
using System.Timers;
using Semnox.Core.Utilities;
using Semnox.Parafait.Redemption;

namespace Semnox.Parafait.ViewContainer
{
    /// <summary>
    /// TicketStationViewContainerList holds multiple  TicketStationView containers based on siteId, userId and POSMachineId
    /// </summary>
    public class TicketStationViewContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Cache<int, TicketStationViewContainer> ticketStationViewContainerCache = new Cache<int, TicketStationViewContainer>();
        private static Timer refreshTimer;

        static TicketStationViewContainerList()
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
            var uniqueKeyList = ticketStationViewContainerCache.Keys;
            foreach (var uniqueKey in uniqueKeyList)
            {
                TicketStationViewContainer ticketStationViewContainer;
                if (ticketStationViewContainerCache.TryGetValue(uniqueKey, out ticketStationViewContainer))
                {
                    ticketStationViewContainerCache[uniqueKey] = ticketStationViewContainer.Refresh(false);
                }
            }
            log.LogMethodExit();
        }



        private static TicketStationViewContainer GetTicketStationViewContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            TicketStationViewContainer result = ticketStationViewContainerCache.GetOrAdd(siteId, (k) => new TicketStationViewContainer(siteId));
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the TicketStationContainerDTOCollection for a given siteId and hash
        /// If the hash matches then no data is returned as the caller is holding the latest data
        /// </summary>
        /// <param name="siteId">site Id</param>
        /// <param name="hash">hash</param>
        /// <returns></returns>
        public static TicketStationContainerDTOCollection GetTicketStationContainerDTOCollection(int siteId, string hash)
        {
            log.LogMethodEntry(siteId, hash);
            TicketStationViewContainer ticketStationViewContainer = GetTicketStationViewContainer(siteId);
            TicketStationContainerDTOCollection result = ticketStationViewContainer.GetTicketStationContainerDTOCollection(hash);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// rebuilds the container
        /// </summary>
        public static void Rebuild(int siteId)
        {
            log.LogMethodEntry(siteId);
            TicketStationViewContainer ticketStationViewContainer = GetTicketStationViewContainer(siteId);
            ticketStationViewContainerCache[siteId] = ticketStationViewContainer.Refresh(true);
            log.LogMethodExit();
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

        /// <summary>
        /// Returns the TicketStationContainerDTO based on the siteId and stationId
        /// </summary>
        /// <param name="siteId">site Id</param>
        /// <param name="stationId">station Id</param>
        /// <returns></returns>
        public static TicketStationContainerDTO GetTicketStationContainerDTO(int siteId, int stationId)
        {
            log.LogMethodEntry(siteId, stationId);
            TicketStationViewContainer ticketStationViewContainer = GetTicketStationViewContainer(siteId);
            TicketStationContainerDTO result = ticketStationViewContainer.GetTicketStationContainerDTO(stationId);
            log.LogMethodExit();
            return result;
        }

        public static List<TicketStationContainerDTO> GetTicketStationContainerDTOList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            TicketStationViewContainer ticketStationViewContainer = GetTicketStationViewContainer(executionContext.GetSiteId());
            List<TicketStationContainerDTO> result = ticketStationViewContainer.GetTicketStationContainerDTOList();
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the TicketStationContainerDTO for the ticket barcode
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="barCode">barCode</param>
        /// <returns></returns>
        public static TicketStationContainerDTO GetTicketStation(ExecutionContext executionContext, string barCode)
        {
            log.LogMethodEntry(executionContext, barCode);
            TicketStationViewContainer ticketStationViewContainer = GetTicketStationViewContainer(executionContext.SiteId);
            TicketStationContainerDTO result = ticketStationViewContainer.GetTicketStation(barCode);
            log.LogMethodExit(result);
            return result;
        }
    }
}
