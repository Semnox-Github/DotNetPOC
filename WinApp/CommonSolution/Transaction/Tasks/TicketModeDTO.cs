/********************************************************************************************
 * Project Name - Transaction
 * Description  - Data object of Ticket Mode
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.110.1     23-Mar-2021   Vikas Dwivedi           Created 
 ********************************************************************************************/
namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// This is the Ticket Mode type enum
    /// </summary>
    public enum TicketModeType
    {
        ///<summary>
        ///REAL
        ///</summary>
        REAL = 1,

        ///<summary>
        ///ETICKET
        ///</summary>
        ETICKET = 0
    }
    /// <summary>
    /// This is the Ticket Mode data object class. This acts as data holder for the Ticket Mode business object
    /// </summary>
    public class TicketModeDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int cardId;
        private int managerId;
        private TicketModeType ticketMode;
        private string remarks;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public TicketModeDTO()
        {
            log.LogMethodEntry();
            cardId = -1;
            managerId = -1;
            ticketMode = TicketModeType.ETICKET;
            remarks = string.Empty;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public TicketModeDTO(int cardId, int managerId, TicketModeType ticketMode, string remarks)
        {
            log.LogMethodEntry();
            this.cardId = cardId;
            this.managerId = managerId;
            this.ticketMode = ticketMode;
            this.remarks = remarks;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the CardId field
        /// </summary>
        public int CardId
        {
            get { return cardId; }
            set { cardId = value; }
        }

        /// <summary>
        /// Get/Set method of the ManagerId field
        /// </summary>
        public int ManagerId
        {
            get { return managerId; }
            set { managerId = value; }
        }

        /// <summary>
        /// Get/Set method of the Remarks field
        /// </summary>
        public string Remarks
        {
            get { return remarks; }
            set { remarks = value; }
        }

        /// <summary>
        /// Get/Set method of the TicketMode field
        /// </summary>
        public bool TicketMode
        {
            get
            {
                if (ticketMode == TicketModeType.REAL)
                    return true;
                else
                    return false;
            }
            set
            {
                if (value)
                    ticketMode = TicketModeType.REAL;
                else
                    ticketMode = TicketModeType.ETICKET;
            }
        }
    }
}
