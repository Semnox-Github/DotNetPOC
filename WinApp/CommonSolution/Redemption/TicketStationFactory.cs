/********************************************************************************************
 * Project Name - TicketReceipt
 * Description  - Ticket Station factory class
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.3.0       05-Jun-2018      Archana/Guru S A     Created 
 *2.70.2.0      16-Sept -2019    Girish Kundar        Modified : Part of Ticket Eater enhancements. 
 ********************************************************************************************/
//using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Semnox.Parafait.Redemption
{
    /// <summary>
    /// Get TicketStation Object
    /// </summary>
    /// <param name="barCode">Parameter of type string</param>
    /// <returns>TicketStationBL object</returns>
    public class TicketStationFactory
    {
        private static readonly Logger log = new Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Semnox.Core.Utilities.ExecutionContext machineUserContext = Semnox.Core.Utilities.ExecutionContext.GetExecutionContext();

        /// <summary>
        /// Get TicketStation Object
        /// </summary>
        /// <param name="barCode">barCode</param>
        /// <returns>TicketStationBL</returns>

        public TicketStationBL GetTicketStationObject(string barCode)
        {
            log.LogMethodEntry(barCode);
            TicketStationContainer ticketStationContainer = TicketStationContainer.GetInstance;
            List<TicketStationDTO> ticketStationDTOList = ticketStationContainer.GetAllTicketStationDTOList;
            TicketStationBL ticketStation = null;
            ticketStationDTOList = ticketStationDTOList.OrderByDescending(x => x.TicketStationId.Length).ToList();
            foreach (TicketStationDTO ticketStationDTO in ticketStationDTOList)
            {
                if (barCode.Length < ticketStationDTO.TicketStationId.Length)
                {
                    log.Debug("barcode length is less than the station Id");
                    continue;
                }
                string stationId = barCode.Substring(0, ticketStationDTO.TicketStationId.Length);
                if (ticketStationDTO.TicketStationId == stationId && ticketStationDTO.VoucherLength == barCode.Length)
                {
                    if (ticketStationDTO.TicketStationType == TicketStationDTO.TICKETSTATIONTYPE.POS_TICKET_STATION)
                    {
                        ticketStation = GetPosCounterTicketStationObject();
                        if (ticketStation == null)
                        {
                            string message = MessageContainerList.GetMessage(machineUserContext, 2322);
                            throw new Exception(message);
                        }
                        break;
                    }
                    else if (ticketStationDTO.CheckDigit && ticketStationDTO.CheckBitAlgorithm == TicketStationAlgorithm.MODULO_TEN_WEIGHT_THREE)
                    {
                        ticketStation = new ModuleTenWtThreeTicketStationBL(ticketStationDTO);
                        break;
                    }
                    else // default/NA
                    {
                        ticketStation = new TicketStationBL(machineUserContext, ticketStationDTO);
                        break;
                    }
                    
                }
               
            }
            if (ticketStation == null)
            {
                log.Error("Unable to find the matching station");
                return ticketStation;
            }
            log.LogMethodExit(ticketStation);
            return ticketStation;
        }


        /// <summary>
        /// Get Primary TicketStation Object
        /// </summary>
        /// <returns>TicketStationBL object</returns>
        public POSCounterTicketStationBL GetPosCounterTicketStationObject()
        {
            log.LogMethodEntry();
            POSCounterTicketStationBL posCounterTicketStationBL = null; 
            TicketStationContainer ticketStationContainer = TicketStationContainer.GetInstance;
            List<TicketStationDTO> ticketStationDTOList = ticketStationContainer.GetAllTicketStationDTOList;
            int Id = Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault<int>(machineUserContext, "DEFAULT_POS_COUNTER_TICKET_STATION",-1);
            if (Id > -1)
            {
                TicketStationDTO ticketStationDTO = ticketStationDTOList.Where(x => x.Id == Id).First();
                posCounterTicketStationBL = new POSCounterTicketStationBL(ticketStationDTO);
                log.LogMethodExit(posCounterTicketStationBL);
                return posCounterTicketStationBL;
            }
            else
            {
                log.Error("Default POS counter Ticket Station is Not Set.");
                return posCounterTicketStationBL;
            }
        }
    }
}
