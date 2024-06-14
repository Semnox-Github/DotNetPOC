/********************************************************************************************
 * Project Name - Redemption
 * Description  - TicketStationsContainer class to get the List of lookup from the container API
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0      21-Dec-2020      Abhishek           Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System;
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Semnox.Parafait.Redemption
{
    /// <summary>
    /// Class holds the parafait default values.
    /// </summary>
    public class TicketStationsContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly Dictionary<int, TicketStationContainerDTO> ticketStationContainerDTODictionary = new Dictionary<int, TicketStationContainerDTO>();
        private readonly List<TicketStationDTO> ticketStationDTOList;
        private readonly TicketStationContainerDTOCollection ticketStationContainerDTOCollection;
        private readonly DateTime? ticketStationModuleLastUpdateTime;
        private readonly int siteId;
       
        internal TicketStationsContainer(int siteId) : this(siteId, GetTicketStationDTOList(siteId), GetTicketStationModuleLastUpdateTime(siteId))
        {
            log.LogMethodEntry(siteId);
            log.LogMethodExit();
        }

        private TicketStationsContainer(int siteId, List<TicketStationDTO> ticketStationDTOList, DateTime? ticketStationModuleLastUpdateTime)
        {
            log.LogMethodEntry(siteId);
            this.siteId = siteId;
            this.ticketStationDTOList = ticketStationDTOList;
            this.ticketStationModuleLastUpdateTime = ticketStationModuleLastUpdateTime;
            List<TicketStationContainerDTO> ticketStationDTOContainerDTOList = new List<TicketStationContainerDTO>();
            foreach (TicketStationDTO ticketStationDTO in ticketStationDTOList)
            {
                TicketStationContainerDTO ticketStationContainerDTO = new TicketStationContainerDTO(ticketStationDTO.Id, ticketStationDTO.TicketStationId, ticketStationDTO.VoucherLength, ticketStationDTO.CheckDigit, ticketStationDTO.TicketLength, ticketStationDTO.TicketStationType, ticketStationDTO.CheckBitAlgorithm);
                ticketStationDTOContainerDTOList.Add(ticketStationContainerDTO);
                ticketStationContainerDTODictionary.Add(ticketStationDTO.Id, ticketStationContainerDTO);

            }
            ticketStationContainerDTOCollection = new TicketStationContainerDTOCollection(ticketStationDTOContainerDTOList);
            log.LogMethodExit();
        }

        private static List<TicketStationDTO> GetTicketStationDTOList(int siteId)
        {
            log.LogMethodEntry(siteId);
            List<TicketStationDTO> ticketStationDTOList = null;
            try
            {
                TicketStationListBL ticketStationListBL = new TicketStationListBL();
                List<KeyValuePair<TicketStationDTO.SearchByTicketStationParameters, string>> searchParameters = new List<KeyValuePair<TicketStationDTO.SearchByTicketStationParameters, string>>();
                searchParameters.Add(new KeyValuePair<TicketStationDTO.SearchByTicketStationParameters, string>(TicketStationDTO.SearchByTicketStationParameters.IS_ACTIVE, "1"));
                searchParameters.Add(new KeyValuePair<TicketStationDTO.SearchByTicketStationParameters, string>(TicketStationDTO.SearchByTicketStationParameters.SITE_ID, siteId.ToString()));
                ticketStationDTOList = ticketStationListBL.GetTicketStationDTOList(searchParameters);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while creating the ticket station container.", ex);
            }

            if (ticketStationDTOList == null)
            {
                ticketStationDTOList = new List<TicketStationDTO>();
            }
            log.LogMethodExit(ticketStationDTOList);
            return ticketStationDTOList;
        }

        private static DateTime? GetTicketStationModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            DateTime? result = null;
            try
            {
                TicketStationListBL ticketStationListBL = new TicketStationListBL();
                result = ticketStationListBL.GetTicketStationModuleLastUpdateTime(siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving the ticket station max last update date.", ex);
                result = null;
            }
            log.LogMethodExit(result);
            return result;
        }

        public TicketStationContainerDTOCollection GetTicketStationContainerDTOCollection()
        {
            log.LogMethodEntry();
            log.LogMethodExit(ticketStationContainerDTOCollection);
            return ticketStationContainerDTOCollection;
        }

        public TicketStationContainerDTO GetTicketStationContainerDTO(int stationId)
        {
            log.LogMethodEntry(stationId);
            TicketStationContainerDTO result = null;
            if (ticketStationContainerDTODictionary.ContainsKey(stationId))
            {
                result = ticketStationContainerDTODictionary[stationId];
            }
            log.LogMethodExit(result);
            return result;
        }

        public TicketStationsContainer Refresh()
        {
            log.LogMethodEntry();
            TicketStationListBL ticketStationListBL = new TicketStationListBL();
            DateTime? updateTime = ticketStationListBL.GetTicketStationModuleLastUpdateTime(siteId);
            if (ticketStationModuleLastUpdateTime.HasValue
                && ticketStationModuleLastUpdateTime >= updateTime)
            {
                log.LogMethodExit(this, "No changes in Ticket Station since " + updateTime.Value.ToString(CultureInfo.InvariantCulture));
                return this;
            }
            TicketStationsContainer result = new TicketStationsContainer(siteId);
            log.LogMethodExit(result);
            return result;
        }
    }
}