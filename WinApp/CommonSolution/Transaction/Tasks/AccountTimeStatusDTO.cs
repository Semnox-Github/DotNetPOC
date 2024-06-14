/********************************************************************************************
 * Project Name - Transaction
 * Description  - Data object of Account Pause
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.110.1     23-Mar-2021   Vikas Dwivedi           Created 
 ********************************************************************************************/

using System.ComponentModel;
using Semnox.Parafait.Customer.Accounts;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// This is the Account Pause data object class. This acts as data holder for the Account Pause business object
    /// </summary>
    public class AccountTimeStatusDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int cardId;
        private int managerId;
        private AccountDTO.AccountTimeStatusEnum timeStatus;
        private string remarks;
        /// <summary>
        /// Default Constructor
        /// </summary>
        public AccountTimeStatusDTO()
        {
            log.LogMethodEntry();
            cardId = -1;
            managerId = -1;
            timeStatus = AccountDTO.AccountTimeStatusEnum.PAUSED;
            remarks = string.Empty;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public AccountTimeStatusDTO(int cardId, int managerId, AccountDTO.AccountTimeStatusEnum accountTimeStatusEnum, string remarks)
        {
            log.LogMethodEntry();
            this.cardId = cardId;
            this.managerId = managerId;
            this.timeStatus = accountTimeStatusEnum;
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
        public AccountDTO.AccountTimeStatusEnum TimeStatus
        {
            get { return timeStatus; }
            set { timeStatus = value; }
        }
    }
}
