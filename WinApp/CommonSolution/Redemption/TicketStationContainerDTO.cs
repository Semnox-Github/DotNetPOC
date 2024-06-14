/********************************************************************************************
 * Project Name - Redemption
 * Description  - Data structure of TicketStationViewContainer
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *2.110.0     21-Dec-2020      Abhishek          Created: POS Redesign
 ********************************************************************************************/

namespace Semnox.Parafait.Redemption
{
    /// <summary>
    /// Data structure of LanguageViewContainer
    /// </summary>
    public class TicketStationContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int id;
        private string ticketStationId;
        private int voucherLength;
        private bool checkDigit;
        private TicketStationDTO.TICKETSTATIONTYPE ticketStationType;
        private int ticketLength;
        private string checkBitAlgorithm;

        /// <summary>
        /// Default constructor
        /// </summary>
        public TicketStationContainerDTO()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with Required data fields
        /// </summary>
        public TicketStationContainerDTO(int id, string ticketStationId, int voucherLength, bool checkDigit, int ticketLength,
                                         TicketStationDTO.TICKETSTATIONTYPE ticketStationType = TicketStationDTO.TICKETSTATIONTYPE.PHYSICAL_TICKET_STATION, string checkBitAlgorithm = "Default/NA")
            : this()
        {
            log.LogMethodEntry(id, ticketStationId, voucherLength, checkDigit, ticketStationType, ticketLength);
            this.id = id;
            this.ticketStationId = ticketStationId;
            this.voucherLength = voucherLength;
            this.checkDigit = checkDigit;
            this.ticketStationType = ticketStationType;
            this.ticketLength = ticketLength;
            this.checkBitAlgorithm = checkBitAlgorithm;
            log.LogMethodExit();
        }

        public int Id
        {
            get { return id; }
            set { id = value;  }
        }

        /// <summary>
        /// Get method of the TicketStationId field
        /// </summary>
        public string TicketStationId { get { return ticketStationId; } set { ticketStationId = value;  } }

        /// <summary>
        /// Get method of the CheckDigit field
        /// </summary>
        public bool CheckDigit { get { return checkDigit; } set { checkDigit = value;  } }

        /// <summary>
        /// Get method of the VoucherLength field
        /// </summary>
        public int VoucherLength { get { return voucherLength; } set { voucherLength = value;  } }


        /// <summary>
        /// Get method of the TicketLength field
        /// </summary>
        public int TicketLength { get { return ticketLength; } set { ticketLength = value;  } }

        /// <summary>
        /// Get method of the checkBitAlgorithm field
        /// </summary>
        public string CheckBitAlgorithm { get { return checkBitAlgorithm; } set { checkBitAlgorithm = value;  } }
        /// <summary>
        /// Get method of the ticketStationType field
        /// </summary>
        public TicketStationDTO.TICKETSTATIONTYPE TicketStationType { get { return ticketStationType; } set { ticketStationType = value;  } }

    }
}
