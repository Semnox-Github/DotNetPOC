/********************************************************************************************
 * Project Name - Transaction
 * Description  -TransactionServiceDTO for activity like load bonus , tickets,consolidate cards,redeem loyalty points
 **************
 **Version Log
 **************
 *Version    Date          Modified By            Remarks          
 *********************************************************************************************
 *2.80.0     12-Mar-2020   Girish Kundar          Created
 ********************************************************************************************/

using System.Collections.Generic;
using Semnox.Parafait.Customer.Accounts;
namespace Semnox.Parafait.Transaction
{

    /// <summary>
    ///  TransactionServiceDTO 
    /// </summary>
    public class TransactionServiceDTO
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private AccountDTO sourceAccountDTO;
        private AccountDTO destinationAccountDTO;
        private List<AccountDTO> accountDTOList;
        private int customerId;
        private int gamePlayId;
        private decimal loyaltyPoints;
        private int tickets;
        private bool refundable;
        private bool invalidateSourceCard;
        private decimal bonus;
        private decimal value;
        private string entitlementType;
        private string remarks;
        private string loyaltyAttribute;
        private bool mergeHistoryDuringSourceInactivation;

        public TransactionServiceDTO()
        {
            log.LogMethodEntry();
            sourceAccountDTO = new AccountDTO();
            destinationAccountDTO = new AccountDTO();
            accountDTOList = new List<AccountDTO>();
            customerId = -1;
            gamePlayId = -1;
            loyaltyPoints = 0;
            tickets = 0;
            refundable = false;
            invalidateSourceCard = false;
            mergeHistoryDuringSourceInactivation = false;
            log.LogMethodExit();

        }
        /// <summary>
        /// SourceAccountDTO
        /// </summary>
        public AccountDTO SourceAccountDTO { get { return sourceAccountDTO; } set { sourceAccountDTO = value; } }
        /// <summary>
        /// destinationAccountDTO
        /// </summary>
        public AccountDTO DestinationAccountDTO { get { return destinationAccountDTO; } set { destinationAccountDTO = value; } }
        /// <summary>
        /// AccountDTOList
        /// </summary>
        public List<AccountDTO> AccountDTOList { get { return accountDTOList; } set { accountDTOList = value; } }

        /// <summary>
        /// CustomerId
        /// </summary>
        public int CustomerId { get { return customerId; } set { customerId = value; } }

        /// <summary>
        /// GamePlayId
        /// </summary>
        public int GamePlayId { get { return gamePlayId; } set { gamePlayId = value; } }

        /// <summary>
        /// InvalidateSourceCard
        /// </summary>
        public bool InvalidateSourceCard { get { return invalidateSourceCard; } set { invalidateSourceCard = value; } }

        /// <summary>
        /// Refundable
        /// </summary>
        public bool Refundable { get { return refundable; } set { refundable = value; } }

        /// <summary>
        /// Tickets
        /// </summary>
        public int Tickets { get { return tickets; } set { tickets = value; } }

        /// <summary>
        /// LoyaltyPoints
        /// </summary>
        public decimal LoyaltyPoints { get { return loyaltyPoints; } set { loyaltyPoints = value; } }

        /// <summary>
        /// Bonus
        /// </summary>
        public decimal Bonus { get { return bonus; } set { bonus = value; } }

        /// <summary>
        /// EntitlementType
        /// </summary>
        public string EntitlementType { get { return entitlementType; } set { entitlementType = value; } }

        /// <summary>
        /// Value
        /// </summary>
        public decimal Value { get { return value; } set { this.value = value; } }

        /// <summary>
        /// Remarks
        /// </summary>
        public string Remarks { get { return remarks; } set { remarks = value; } }

        /// <summary>
        ///  LoyaltyAttribute
        /// </summary>
        public string LoyaltyAttribute { get { return loyaltyAttribute; } set { loyaltyAttribute = value; } }
        /// <summary>
        ///  mergeHistoryDuringSourceInactivation
        /// </summary>
        public bool MergeHistoryDuringSourceInactivation { get { return mergeHistoryDuringSourceInactivation; } set { mergeHistoryDuringSourceInactivation = value; } }

    }
}
