/********************************************************************************************
 * Project Name - AccountService
 * Description  -AccountServiceDTO for activity like Link Accounts
Link Account to Customer
Transfer Account
Transfer balance 
Redeem bonus and tickets
Exchange credits and token
Exchange credits and time
Refund Entitlements
Pause Card
 **************
 **Version Log
 **************
 *Version    Date          Modified By            Remarks          
 *********************************************************************************************
 *2.80.0     12-Mar-2020   Girish Kundar          Created - Initial version
 ********************************************************************************************/
using Semnox.Parafait.Customer;
using Semnox.Parafait.Customer.Accounts;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// Account service class
    /// </summary>
    public class AccountServiceDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private AccountDTO sourceAccountDTO;
        private AccountDTO destinationAccountDTO;
        private List<AccountDTO> accountDTOList;
        private List<AccountCreditPlusDTO> accountCreditPlusDTOList;
        private CustomerDTO customerDTO;
        private bool redeemBonusForTickets;
        private decimal cardDeposit;
        private decimal credits;
        private int tickets;
        private int tokens;
        private int trxId;
        private int trxLineId;
        private decimal creditPlus;
        private decimal bonus;
        private decimal time;
        private decimal value;
        private string entitlementType;
        private string loginId;
        private string remarks;
        private decimal refundAmount;
        private Guid deviceUuid;
        private bool exchangeCreditsForToken ;
        private bool makeNewCardOnFullRefund ;
        private bool exchangeCreditsForTime;
        private Dictionary<string, decimal> entitlementsToTransfer;
        private Dictionary<int, decimal> transferredIdMap;


        /// <summary>
        /// SourceAccountDTO
        /// </summary>
        public AccountDTO SourceAccountDTO { get { return sourceAccountDTO; }          set { sourceAccountDTO = value; }     }

        /// <summary>
        /// DestinationAccountDTO
        /// </summary>
        public AccountDTO DestinationAccountDTO { get { return destinationAccountDTO; } set { destinationAccountDTO = value; } }

        /// <summary>
        /// AccountDTOList
        /// </summary>
        public List<AccountDTO> AccountDTOList { get { return accountDTOList; } set { accountDTOList = value; } }

        /// <summary>
        /// ExchangeCreditsForToken
        /// </summary>
        [Browsable(true)]
        public bool ExchangeCreditsForToken { get { return exchangeCreditsForToken; }    set { exchangeCreditsForToken = value; } }

        /// <summary>
        /// AccountCreditPlusDTOList
        /// </summary>
        public List<AccountCreditPlusDTO> AccountCreditPlusDTOList { get { return accountCreditPlusDTOList; } set { accountCreditPlusDTOList = value; } }

        /// <summary>
        /// CustomerDTO
        /// </summary>
        public CustomerDTO CustomerDTO { get { return customerDTO; } set { customerDTO = value; } }


        /// <summary>
        /// RedeemBonusForTickets
        /// </summary>
        public bool RedeemBonusForTickets { get { return redeemBonusForTickets; } set { redeemBonusForTickets = value; } }

        /// <summary>
        /// CardDeposit
        /// </summary>
        public decimal CardDeposit { get { return cardDeposit; } set { cardDeposit = value; } }

        /// <summary>
        /// RefundAmount
        /// </summary>
        public decimal RefundAmount { get { return refundAmount; } set { refundAmount = value; } }

        /// <summary>
        /// Credits
        /// </summary>
        public decimal Credits { get { return credits; } set { credits = value; } }

        /// <summary>
        /// CreditPlus
        /// </summary>
        public decimal CreditPlus { get { return creditPlus; } set { creditPlus = value; } }

        /// <summary>
        /// Bonus
        /// </summary>
        public decimal Bonus { get { return bonus; } set { bonus = value; } }

        /// <summary>
        /// Time
        /// </summary>
        public decimal Time { get { return time; } set { time = value; } }

        /// <summary>
        /// Tokens
        /// </summary>
        public int Tokens { get { return tokens; } set { tokens = value; } }

        /// <summary>
        /// Tickets
        /// </summary>
        public int Tickets { get { return tickets; } set { tickets = value; } }

        /// <summary>
        /// TrxId
        /// </summary>
        public int TrxId { get { return trxId; } set { trxId = value; } }

        /// <summary>
        /// TrxLineId
        /// </summary>
        public int TrxLineId { get { return trxLineId; } set { trxLineId = value; } }

        /// <summary>
        /// EntitlementType
        /// </summary>
        public string EntitlementType { get { return entitlementType; } set { entitlementType = value; } }

        /// <summary>
        /// LoginId
        /// </summary>
        public string LoginId { get { return loginId; } set { loginId = value; } }

        /// <summary>
        /// Value
        /// </summary>
        public decimal Value { get { return value; } set { this.value = value; } }

        /// <summary>
        /// MakeNewCardOnFullRefund
        /// </summary>
        public bool MakeNewCardOnFullRefund { get { return makeNewCardOnFullRefund; } set { makeNewCardOnFullRefund = value; } }

        /// <summary>
        /// ExchangeCreditsForTime
        /// </summary>
        public bool ExchangeCreditsForTime { get { return exchangeCreditsForTime; } set { exchangeCreditsForTime = value; } }


        /// <summary>
        /// Remarks
        /// </summary>
        public string Remarks { get { return remarks; } set { remarks = value; } }

        /// <summary>
        /// DeviceUuid
        /// </summary>
        public Guid DeviceUuid { get { return deviceUuid; } set { deviceUuid = value; } }

        /// <summary>
        /// EntitlementsToTransfer
        /// </summary>
        public Dictionary<string, decimal> EntitlementsToTransfer { get { return entitlementsToTransfer; } set { entitlementsToTransfer = value; } }

        /// <summary>
        /// TransferredIdMap
        /// </summary>
        public Dictionary<int, decimal> TransferredIdMap { get { return transferredIdMap; } set { transferredIdMap = value; } }


    }
}
