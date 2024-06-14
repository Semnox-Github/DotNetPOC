/********************************************************************************************
 * Project Name - TicketStationContainer
 * Description  - Container class for the TicketStation  
 * 
 **************
 **Version Log
 **************
 *Version     Date              Modified By        Remarks          
 *********************************************************************************************
 *2.70.2        05-Oct-2019       Girish Kundar       Created.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;

namespace Semnox.Parafait.Redemption
{
    /// <summary>
    /// Sealed class for the TicketStation
    /// </summary>
    public sealed class TicketStationContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static List<TicketStationDTO> ticketStationDTOList = new List<TicketStationDTO>();
        private static TicketStationDTO ticketStationDTO;
        private Utilities utilities = new Utilities();
        /// <summary>
        /// Default constructor for TicketStationContainer
        /// </summary>
        private TicketStationContainer()
        {
            log.LogMethodEntry();
            TicketStationListBL ticketStationListBL = new TicketStationListBL(ExecutionContext.GetExecutionContext());
            ticketStationDTOList = ticketStationListBL.GetAllTicketStations();
            log.LogMethodExit();
        }
        private static readonly TicketStationContainer instance = new TicketStationContainer();

        /// <summary>
        /// Gets the TicketStationContainer object
        /// </summary>
        public static TicketStationContainer GetInstance
        {
            get
            {
                return instance;
            }
        }

        /// <summary>
        /// Gets the All TicketStationDTO List
        /// </summary>
        public List<TicketStationDTO> GetAllTicketStationDTOList
        {
            get { return ticketStationDTOList; }
        }

        /// <summary>
        /// Gets the TicketStationDTO based on the barcode
        /// </summary>
        /// <param name="barcode">barcode</param>
        /// <returns>TicketStationDTO</returns>
        public TicketStationDTO GetTicketStationDTO(string barcode)
        {
            log.LogMethodEntry(barcode);
            TicketStationDTO ticketStationDTO = null;
            TicketStationFactory ticketStationFactory = new TicketStationFactory();
            TicketStationBL ticketStationBL = ticketStationFactory.GetTicketStationObject(barcode);
            if (ticketStationBL != null)
            {
                if (ticketStationBL.BelongsToThisStation(barcode))
                {
                    log.LogMethodExit(ticketStationBL.TicketStationDTO);
                    return ticketStationBL.TicketStationDTO;
                }
            }
            else
            {
                log.Error("Unable to find the matching station");
                throw new Exception("Unable to find the matching station.");
            }
            return ticketStationDTO;
        }
    }
}
