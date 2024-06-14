/********************************************************************************************
 * Project Name - Redemption 
 * Description  - Data object of TicketStationContainerDTOCollection
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 2.110.0      21-Dec-2020      Abhishek           Created : POS UI Redesign with REST API
 ********************************************************************************************/

using System.Collections.Generic;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Redemption;

namespace Semnox.Parafait.Redemption
{
    public class TicketStationContainerDTOCollection
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<TicketStationContainerDTO> ticketStationContainerDTOList;
        private string hash;

        public TicketStationContainerDTOCollection()
        {
            log.LogMethodEntry();
            ticketStationContainerDTOList = new List<TicketStationContainerDTO>();
            log.LogMethodExit();
        }

        public TicketStationContainerDTOCollection(List<TicketStationContainerDTO> ticketStationContainerDTOList)
        {
            log.LogMethodEntry(ticketStationContainerDTOList);
            this.ticketStationContainerDTOList = ticketStationContainerDTOList;
            if (this.ticketStationContainerDTOList == null)
            {
                this.ticketStationContainerDTOList = new List<TicketStationContainerDTO>();
            }
            hash = new DtoListHash(ticketStationContainerDTOList);
            log.LogMethodExit();
        }

        public List<TicketStationContainerDTO> TicketStationContainerDTOList
        {
            get
            {
                return ticketStationContainerDTOList;
            }

            set
            {
                ticketStationContainerDTOList = value;
            }
        }

        public string Hash
        {
            get
            {
                return hash;
            }
            set
            {
                hash = value;
            }
        }

    }
}
