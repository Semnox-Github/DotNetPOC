using Semnox.Parafait.Customer;
using Semnox.Parafait.Customer.Accounts;
using System;
using System.Collections.Generic;

namespace Semnox.Parafait.Customer
{
    public class AccountServicesDTO
    {
        public enum ActivityType
        {
            BLANKACTIVITY,//Place holder 
            PURCHASES,
            GAMEPLAY,
            GAMEPLAYEXTENDED,
            REFUNDGAME,   
            REFUNDBALANCE,
            AVAILABLEBALANCE,
            LINKACCOUNT,
            TOTALAVAILABLEBALANCE,
            DEACTIVATEFINGERPRINT,
            LOADVALUE,
            BALANCETRANSFER,
            CHECKBALANCE,
            LOSTCARD,
            REFUNDTAG,
            DEDUCTBALANCE
        }

        private AccountDTO sourceAccountDTO;
        private AccountDTO destinationAccountDTO;
        private List<AccountDTO> destinationAccountDTOList;
        private List<AccountCreditPlusDTO> creditPlusDTOList;
        private int customerId;
        private decimal value;
        private string stringValue;
        private string entitlementType;
        private string remarks;
        private string reference;
        private Guid deviceUuid;
        private ActivityType serviceType;

        public AccountDTO SourceAccountDTO { get { return sourceAccountDTO; } set { sourceAccountDTO = value; } }
        public AccountDTO DestinationAccountDTO { get { return destinationAccountDTO; } set { destinationAccountDTO = value; } }
        public ActivityType ServiceType { get { return serviceType; } set { serviceType = value; } }
        public int CustomerId { get { return customerId; } set { customerId = value; } }
        //public int GameTrxId { get { return gameTrxId; } set { gameTrxId = value; } }
        //public int MachineId { get { return machineId; } set { machineId = value; } }
        //public int GameId { get { return gameId; } set { gameId = value; } }
        //public int NumberOfPlays { get { return numberOfPlays; } set { numberOfPlays = value; } }
        //public decimal CardDeposit { get { return cardDeposit; } set { cardDeposit = value; } }
        //public decimal Credits { get { return credits; } set { credits = value; } }
        //public decimal CreditPlus { get { return creditPlus; } set { creditPlus = value; } }
        //public decimal Bonus { get { return bonus; } set { bonus = value; } }
        public string EntitlementType { get { return entitlementType; } set { entitlementType = value; } }
        //public string UserId { get { return userId; } set { userId = value; } }
        //public string LoginId { get { return loginId; } set { loginId = value; } }
        public decimal Value { get { return value; } set { this.value = value; } }
        public string Remarks { get { return remarks; } set { remarks = value; } }
        public string Reference { get { return reference; } set { reference = value; } }
        public Guid DeviceUuid { get { return deviceUuid; } set { deviceUuid = value; } }
    }
}