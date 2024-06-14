/********************************************************************************************
 * Project Name - Transaction
 * Description  - Data object of Account Transfer
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
    /// This is the Account Transfer data object class. This acts as data holder for the  Account Transfer business object
    /// </summary>
    public class AccountTransferDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int fromCardId;
        private int managerId;
        private string toCardNumber;
        private string remarks;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public AccountTransferDTO()
        {
            log.LogMethodEntry();
            fromCardId = -1;
            managerId = -1;
            toCardNumber = string.Empty;
            remarks = string.Empty;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public AccountTransferDTO(int fromCardId, int managerId, string toCardNumber, string remarks)
        {
            log.LogMethodEntry();
            this.fromCardId = fromCardId;
            this.managerId = managerId;
            this.toCardNumber = toCardNumber;
            this.remarks = remarks;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the FromCardId field
        /// </summary>
        public int FromCardId
        {
            get { return fromCardId; }
            set { fromCardId = value; }
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
        /// Get/Set method of the ToCardNumber field
        /// </summary>
        public string ToCardNumber
        {
            get { return toCardNumber; }
            set { toCardNumber = value; }
        }
    }
}
