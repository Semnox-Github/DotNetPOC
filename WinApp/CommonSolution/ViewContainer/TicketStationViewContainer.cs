
/********************************************************************************************
 * Project Name - ViewContainer
 * Description  - TicketStationViewContainer 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 *2.110.0      21-Dec-2020      Abhishek           Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Redemption;

namespace Semnox.Parafait.ViewContainer
{
    /// <summary>
    /// LanguageViewContainer holds the parafait default values for a given siteId, userId and POSMachineId
    /// </summary>
    public class TicketStationViewContainer : AbstractViewContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly TicketStationContainerDTOCollection ticketStationContainerDTOCollection;
        private readonly ConcurrentDictionary<int, TicketStationContainerDTO> ticketStationContainerDTODictionary = new ConcurrentDictionary<int, TicketStationContainerDTO>();
        private readonly int siteId;

        /// <summary>
        /// Parametrized constructor
        /// </summary>
        /// <param name="siteId">site id</param>
        /// <param name="ticketStationContainerDTOCollection">ticketStationContainerDTOCollection</param>
        internal TicketStationViewContainer(int siteId, TicketStationContainerDTOCollection ticketStationContainerDTOCollection)
        {
            log.LogMethodEntry(siteId, ticketStationContainerDTOCollection);
            this.siteId = siteId;
            this.ticketStationContainerDTOCollection = ticketStationContainerDTOCollection;
            if (ticketStationContainerDTOCollection != null &&
                ticketStationContainerDTOCollection.TicketStationContainerDTOList != null &&
                ticketStationContainerDTOCollection.TicketStationContainerDTOList.Any())
            {
                foreach (var ticketStationContainerDTO in ticketStationContainerDTOCollection.TicketStationContainerDTOList)
                {
                    ticketStationContainerDTODictionary[ticketStationContainerDTO.Id] = ticketStationContainerDTO;
                }
            }
            log.LogMethodExit();
        }

        public TicketStationViewContainer(int siteId)
            : this(siteId, GetTicketStationContainerDTOCollection(siteId, null, false))
        {
            log.LogMethodEntry(siteId);
            log.LogMethodExit();
        }

        private static TicketStationContainerDTOCollection GetTicketStationContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(siteId);
            TicketStationContainerDTOCollection result;
            try
            {
                ExecutionContext executionContext = GetSystemUserExecutionContext();
                ITicketStationUseCases ticketStationUseCases = RedemptionUseCaseFactory.GetTicketStationUseCases(executionContext);
                using (NoSynchronizationContextScope.Enter())
                {
                    Task<TicketStationContainerDTOCollection> task = ticketStationUseCases.GetTicketStationContainerDTOCollection(siteId, hash, rebuildCache);
                    task.Wait();
                    result = task.Result;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving TicketStationContainerDTOCollection.", ex);
                result = new TicketStationContainerDTOCollection();
            }

            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// returns the TicketStationContainerDTO for the stationId
        /// </summary>
        /// <param name="stationId"></param>
        /// <returns></returns>
        public TicketStationContainerDTO GetTicketStationContainerDTO(int stationId)
        {
            log.LogMethodEntry(stationId);
            if (ticketStationContainerDTODictionary.ContainsKey(stationId) == false)
            {
                string errorMessage = "Ticket Station with Station Id :" + stationId + " doesn't exists.";
                log.LogMethodExit("Throwing Exception - " + errorMessage);
                throw new Exception(errorMessage);
            }
            TicketStationContainerDTO result = ticketStationContainerDTODictionary[stationId];
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// returns the latest in TicketStationContainerDTOCollection
        /// </summary>
        /// <returns></returns>
        internal TicketStationContainerDTOCollection GetTicketStationContainerDTOCollection(string hash)
        {
            log.LogMethodEntry(hash);
            if (ticketStationContainerDTOCollection.Hash == hash)
            {
                log.LogMethodExit(null, "No changes to the cache");
                return null;
            }
            log.LogMethodExit(ticketStationContainerDTOCollection);
            return ticketStationContainerDTOCollection;
        }

        internal TicketStationViewContainer Refresh(bool rebuildCache)
        {
            log.LogMethodEntry();
            if (LastRefreshTime.AddMinutes(MinimimViewContainerRefreshWaitPeriod.GetValueInMinutes()) > DateTime.Now)
            {
                log.LogMethodExit(this, "Last Refresh Time is " + LastRefreshTime);
                return this;
            }
            LastRefreshTime = DateTime.Now;
            TicketStationContainerDTOCollection latestTicketStationContainerDTOCollection = GetTicketStationContainerDTOCollection(siteId, ticketStationContainerDTOCollection.Hash, rebuildCache);
            if (latestTicketStationContainerDTOCollection == null ||
                latestTicketStationContainerDTOCollection.TicketStationContainerDTOList == null ||
                latestTicketStationContainerDTOCollection.TicketStationContainerDTOList.Any() == false)
            {
                log.LogMethodExit(this, "No changes to the cache");
                return this;
            }
            TicketStationViewContainer result = new TicketStationViewContainer(siteId, latestTicketStationContainerDTOCollection);
            log.LogMethodExit(result);
            return result;
        }

        internal List<TicketStationContainerDTO> GetTicketStationContainerDTOList()
        {
            log.LogMethodEntry();
            log.LogMethodExit(ticketStationContainerDTOCollection.TicketStationContainerDTOList);
            return ticketStationContainerDTOCollection.TicketStationContainerDTOList;
        }
        internal TicketStationContainerDTO GetTicketStation(string barCode)
        {
            log.LogMethodEntry(barCode);
            TicketStationContainerDTO result = new TicketStationContainerDTO();
            foreach (TicketStationContainerDTO ticketStationContainerDTO in ticketStationContainerDTOCollection.TicketStationContainerDTOList.OrderByDescending(x => x.TicketStationId.Length).ToList())
            {
                if (barCode.Length < ticketStationContainerDTO.TicketStationId.Length)
                {
                    log.Debug("barcode length is less than the station Id");
                    continue;
                }
                if (ticketStationContainerDTO.TicketStationId == barCode.Substring(0, ticketStationContainerDTO.TicketStationId.Length) && ticketStationContainerDTO.VoucherLength == barCode.Length)
                {
                    result = ticketStationContainerDTO;
                    break;
                }
            }
            log.LogMethodExit(result);
            return result;
        }
    }
}
